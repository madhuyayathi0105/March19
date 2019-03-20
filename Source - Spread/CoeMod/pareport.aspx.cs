using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class pareport : System.Web.UI.Page
{
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    string year1 = "", year2 = "", year3 = "", year4 = "", year5 = "";
    static string yearhead = "";

    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable has = new Hashtable();

    int count = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            lblerror.Visible = false;

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
                Rbtn.SelectedIndex = 1;
                lbltest.Visible = false;
                ddltest.Visible = false;

                bindbatch();
                binddegree();
                binddept();
                bindtestname();
                bindyearmonth();
                clear();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void go_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    public void clear()
    {
        lblerrormsg.Visible = false;
        FpSpread1.Visible = false;
        lblexportxl.Visible = false;
        txtexcell.Visible = false;
        btnexcel.Visible = false;
        btnprint.Visible = false;
        lblerror.Visible = false;
        Printcontrol.Visible = false;
    }

    public void bindbatch()
    {
        try
        {
            count = 0;
            chcklistbatch.Items.Clear();
            chckbatch.Checked = false;
            txtbatch.Text = "--Select--";
            ds.Dispose();
            ds.Reset();
            ds = da.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                chcklistbatch.DataSource = ds;
                chcklistbatch.DataTextField = "Batch_year";
                chcklistbatch.DataValueField = "Batch_year";
                chcklistbatch.DataBind();
                for (int i = 0; i < chcklistbatch.Items.Count; i++)
                {
                    chcklistbatch.Items[i].Selected = true;
                    count++;
                }
                if (count > 0)
                {
                    if (chcklistbatch.Items.Count == count)
                    {
                        chckbatch.Checked = true;
                        txtbatch.Text = "Batch (" + (chcklistbatch.Items.Count) + ")";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void binddegree()
    {
        try
        {
            int count = 0;
            chcklistdegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            DataSet ds1 = new DataSet();
            ds1 = da.select_method("bind_degree", has, "sp");

            if (ds1.Tables[0].Rows.Count > 0)
            {
                chcklistdegree.DataSource = ds1;
                chcklistdegree.DataTextField = "course_name";
                chcklistdegree.DataValueField = "course_id";
                chcklistdegree.DataBind();
            }
            if (chcklistdegree.Items.Count > 0)
            {
                for (int j = 0; j < chcklistdegree.Items.Count; j++)
                {
                    count++;
                    chcklistdegree.Items[j].Selected = true;
                }
                txtdegree.Text = "Degree " + "(" + count + ")";
                chckdegree.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void binddept()
    {
        try
        {
            int count = 0;
            string mainvalue = "";
            cbldept.Items.Clear();
            if (chcklistdegree.Items.Count > 0)
            {
                for (int i = 0; i < chcklistdegree.Items.Count; i++)
                {
                    if (chcklistdegree.Items[i].Selected == true)
                    {
                        string subvalue = "";
                        subvalue = chcklistdegree.Items[i].Value;
                        if (mainvalue == "")
                        {
                            mainvalue = subvalue;
                        }
                        else
                        {
                            mainvalue = mainvalue + "," + subvalue;
                        }
                    }
                }
                if (mainvalue.Trim() != "")
                {
                    ds.Clear();
                    ds = da.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), mainvalue, Session["collegecode"].ToString(), Session["usercode"].ToString());
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbldept.DataSource = ds;
                        cbldept.DataTextField = "dept_name";
                        cbldept.DataValueField = "degree_code";
                        cbldept.DataBind();
                    }
                }
                if (cbldept.Items.Count > 0)
                {
                    for (int h = 0; h < cbldept.Items.Count; h++)
                    {
                        count++;
                        cbldept.Items[h].Selected = true;
                    }
                    chckdept.Checked = true;
                    txtdept.Text = "Branch " + "(" + count + ")";
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void bindtestname()
    {
        try
        {
            ddltest.Items.Clear();

            // ------------- For Batch Year 
            string buildvalue = "";
            for (int i = 0; i < chcklistbatch.Items.Count; i++)
            {
                if (chcklistbatch.Items[i].Selected == true)
                {
                    string build = chcklistbatch.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }

            // ------------- For Degree Code
            string buildvalue1 = "";
            for (int i = 0; i < cbldept.Items.Count; i++)
            {
                if (cbldept.Items[i].Selected == true)
                {
                    string build1 = cbldept.Items[i].Value.ToString();
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                    }
                }
            }

            string qreryraj = "select distinct criteria from criteriaforinternal,syllabus_master sy,Registration r where criteriaforinternal.syll_code=sy.syll_code and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and r.Current_Semester=sy.semester and sy.degree_code in ('" + buildvalue1 + "') and r.Batch_Year in ('" + buildvalue + "') and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' order by criteria";
            ds.Clear();
            ds = da.select_method_wo_parameter(qreryraj, "text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddltest.Items.Add(ds.Tables[0].Rows[i]["criteria"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Visible = true;
            lblerrormsg.Text = ex.ToString();
        }
    }

    public void bindyearmonth()
    {
        try
        {
            ddlmonth.Items.Clear();

            string buildvalue = "";
            for (int i = 0; i < chcklistbatch.Items.Count; i++)
            {
                if (chcklistbatch.Items[i].Selected == true)
                {
                    string build = chcklistbatch.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }

            // ---------------------------- Dept
            string buildvalue1 = "";
            for (int i = 0; i < cbldept.Items.Count; i++)
            {
                if (cbldept.Items[i].Selected == true)
                {
                    string build1 = cbldept.Items[i].Value.ToString();
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                    }
                }
            }

            string qurymnth = "select distinct ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,e.Exam_year,e.Exam_Month from Exam_Details e,Registration r where e.degree_code=r.Degree_Code and r.Batch_Year=e.batch_year and e.batch_year in ('" + buildvalue + "') and e.Degree_Code in ('" + buildvalue1 + "') order by e.Exam_year desc,e.Exam_Month desc";
            ds.Clear();
            ds = da.select_method_wo_parameter(qurymnth, "text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlmonth.Items.Add(ds.Tables[0].Rows[i]["monthName"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void checkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtbatch.Text = "--Select--";
            if (chckbatch.Checked == true)
            {
                for (int i = 0; i < chcklistbatch.Items.Count; i++)
                {
                    chcklistbatch.Items[i].Selected = true;
                    txtbatch.Text = "Batch (" + (chcklistbatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chcklistbatch.Items.Count; i++)
                {
                    chcklistbatch.Items[i].Selected = false;
                }
            }

            if (Rbtn.SelectedItem.Text == "External")
            {
                binddegree();
                binddept();
                bindyearmonth();
            }
            else
            {
                binddegree();
                binddept();
                bindtestname();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }


    protected void cheklistBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtbatch.Text = "--Select--";
            chckbatch.Checked = false;
            int commcount = 0;

            for (int i = 0; i < chcklistbatch.Items.Count; i++)
            {
                if (chcklistbatch.Items[i].Selected == true)
                {
                    commcount++;
                }
            }
            if (commcount > 0)
            {
                txtbatch.Text = "Batch (" + commcount.ToString() + ")";
                if (commcount == chcklistbatch.Items.Count)
                {
                    chckbatch.Checked = true;
                }
            }
            if (Rbtn.SelectedItem.Text == "External")
            {
                binddegree();
                binddept();
                bindyearmonth();
            }
            else
            {
                binddegree();
                binddept();
                bindtestname();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void checkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chckdegree.Checked == true)
            {
                for (int i = 0; i < chcklistdegree.Items.Count; i++)
                {
                    chcklistdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree (" + (chcklistdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chcklistdegree.Items.Count; i++)
                {
                    chcklistdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "--Select--";
                txtdept.Text = "--Select--";
                cbldept.ClearSelection();
                chckdept.Checked = false;
            }

            if (Rbtn.SelectedItem.Text == "External")
            {
                binddept();
                bindyearmonth();
            }
            else
            {
                binddept();
                bindtestname();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }


    protected void cheklist_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int degreecount = 0;
            txtdegree.Text = "---Select---";
            chckdegree.Checked = false;
            for (int i = 0; i < chcklistdegree.Items.Count; i++)
            {
                if (chcklistdegree.Items[i].Selected == true)
                {
                    degreecount = degreecount + 1;
                }
            }
            if (degreecount > 0)
            {
                txtdegree.Text = "Degree (" + degreecount + ")";
                if (degreecount == chcklistdegree.Items.Count)
                {
                    chckdegree.Checked = true;
                }
            }
            else
            {
                txtdept.Text = "--Select--";
                cbldept.ClearSelection();
                chckdept.Checked = false;
            }

            if (Rbtn.SelectedItem.Text == "External")
            {
                binddept();
                bindyearmonth();
            }
            else
            {
                binddept();
                bindtestname();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void checkdept_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtdept.Text = "--Select--";
            if (chckdept.Checked == true)
            {
                for (int i = 0; i < cbldept.Items.Count; i++)
                {
                    cbldept.Items[i].Selected = true;
                }
                if (cbldept.Items.Count > 0)
                {
                    txtdept.Text = "Branch (" + (cbldept.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbldept.Items.Count; i++)
                {
                    cbldept.Items[i].Selected = false;
                }
            }

            if (Rbtn.SelectedItem.Text == "External")
            {
                bindyearmonth();
            }
            else
            {
                bindtestname();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void cbldept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int branchcount = 0;
            txtdept.Text = "--Select--";
            chckdept.Checked = false;

            for (int i = 0; i < cbldept.Items.Count; i++)
            {
                if (cbldept.Items[i].Selected == true)
                {
                    branchcount = branchcount + 1;
                }
            }
            if (branchcount > 0)
            {
                txtdept.Text = "Branch (" + branchcount.ToString() + ")";
                if (branchcount == cbldept.Items.Count)
                {
                    chckdept.Checked = true;
                }
            }

            if (Rbtn.SelectedItem.Text == "External")
            {
                bindyearmonth();
            }
            else
            {
                bindtestname();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddltest_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlmonth_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void Rbtn_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Rbtn.Items[0].Selected == true)
            {
                lbltest.Visible = true;
                ddltest.Visible = true;
                lblmonth.Visible = false;
                ddlmonth.Visible = false;
                lblerrormsg.Visible = false;
                FpSpread1.Visible = false;
                lblexportxl.Visible = false;
                txtexcell.Visible = false;
                btnexcel.Visible = false;
                btnprint.Visible = false;
                lblerror.Visible = false;
                Printcontrol.Visible = false;
                bindtestname();
            }
            else
            {
                lblmonth.Visible = true;
                ddlmonth.Visible = true;
                lbltest.Visible = false;
                ddltest.Visible = false;
                lblerrormsg.Visible = false;
                FpSpread1.Visible = false;
                lblexportxl.Visible = false;
                txtexcell.Visible = false;
                btnexcel.Visible = false;
                btnprint.Visible = false;
                lblerror.Visible = false;
                Printcontrol.Visible = false;
                bindyearmonth();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
        {
            clear();

            if (Rbtn.SelectedItem.Text == "Internal")
            {
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                FpSpread1.Sheets[0].ColumnCount = 16;
                FpSpread1.CommandBar.Visible = false;

                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = System.Drawing.Color.White;
                style2.BackColor = System.Drawing.Color.Teal;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Department";

                Boolean reportfalg = false;
                Hashtable hat = new Hashtable();
                Hashtable ht = new Hashtable();

                // ---------------------------- Batch
                string buildvalue = "";
                for (int i = 0; i < chcklistbatch.Items.Count; i++)
                {
                    if (chcklistbatch.Items[i].Selected == true)
                    {
                        hat.Add(chcklistbatch.Items[i].Text, chcklistbatch.Items[i].Text);
                        string build = chcklistbatch.Items[i].Value.ToString();
                        if (buildvalue == "")
                        {
                            buildvalue = build;
                        }
                        else
                        {
                            buildvalue = buildvalue + "'" + "," + "'" + build;
                        }
                    }
                }

                // ---------------------------- Dept
                string buildvalue1 = "";
                for (int i = 0; i < cbldept.Items.Count; i++)
                {
                    if (cbldept.Items[i].Selected == true)
                    {
                        string build1 = cbldept.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }

                int lk = 1, kn = 1, kn1 = 1, kn2 = 1, kn3 = 1, kn4 = 1, kn5 = 1, lm = 1, lm1 = 1, hj = 1, hj1 = 1, hj2 = 1, hj3 = 1, hj4 = 1, cnt = 0, cn1 = 0;
                Hashtable htc = new Hashtable();
                Hashtable htc1 = new Hashtable();
                Hashtable htc2 = new Hashtable();
                Hashtable htc3 = new Hashtable();
                DataView dv1 = new DataView();
                string y1 = "", y2 = "", y3 = "", y4 = "", y5 = "", head = "", head1 = ""; Boolean headfalg = false;

                if (ddltest.Items.Count > 0)
                {
                    reportfalg = true; headfalg = true;

                    cnt++;

                    FpSpread1.Width = 1170;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "I YEAR";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Tag = "I YEAR";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 3);
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 3);
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 3);
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 1, 3);
                    hj++; y1 = "I YEAR"; year1 = "I";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Text = "APPEARED";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Tag = head1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].ForeColor = System.Drawing.Color.White;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].BackColor = System.Drawing.Color.Teal;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Font.Name = "Book Antiqua";
                    kn++; lm++;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Text = "PASS";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Tag = head1;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Text = "PASS%";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Tag = head1;
                    lm++;

                    cnt++;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "II YEAR";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Tag = "II YEAR";
                    hj1++; y2 = "II YEAR"; year2 = "II";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Text = "APPEARED";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Tag = head1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].ForeColor = System.Drawing.Color.White;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].BackColor = System.Drawing.Color.Teal;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Font.Name = "Book Antiqua";
                    kn1++; lm++;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Text = "PASS";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Tag = head1;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "PASS%";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Tag = head1;
                    lm++;
                    hj1++;

                    cnt++;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "III YEAR";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Tag = "III YEAR";
                    hj2++; y3 = "III YEAR"; year3 = "III";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Text = "APPEARED";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Tag = head1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].ForeColor = System.Drawing.Color.White;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].BackColor = System.Drawing.Color.Teal;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Font.Name = "Book Antiqua";
                    kn++; lm++;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Text = "PASS";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Tag = head1;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Text = "PASS%";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Tag = head1;
                    lm++;

                    cnt++;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "IV YEAR";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Tag = "IV YEAR";
                    hj3++; y4 = "IV YEAR"; year4 = "IV";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Text = "APPEARED";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Tag = head1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].ForeColor = System.Drawing.Color.White;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].BackColor = System.Drawing.Color.Teal;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Font.Name = "Book Antiqua";
                    kn++; lm++;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Text = "PASS";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 11].Tag = head1;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Text = "PASS%";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 12].Tag = head1;
                    lm++;
                }
                y1 = ""; y2 = ""; y3 = ""; y4 = ""; y5 = "";
                // --------------  Heading for Bind Year eg: I YEAR, II YEAR, III YEAR end

                // --------------------------- ColumnHeader Spanning start
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                // --------------------------- ColumnHeader Spanning end
                lm = 1;

                // ---------- For Year I, II, III, IV in PDF start
                if (year1 != "")
                {
                    yearhead = year1;
                }
                if (year2 != "")
                {
                    if (yearhead.Contains(year2) != year2.Contains(year2))
                    {
                        if (year1 == "")
                        {
                            yearhead = year2;
                        }
                        else
                        {
                            yearhead = yearhead + ", " + year2;
                        }
                    }
                }
                if (year3 != "")
                {
                    if (yearhead.Contains(year3) != year3.Contains(year3))
                    {
                        if (yearhead == "")
                        {
                            yearhead = year3;
                        }
                        else
                        {
                            yearhead = yearhead + ", " + year3;
                        }
                    }
                }
                if (year4 != "")
                {
                    if (yearhead.Contains(year4) != year4.Contains(year4))
                    {
                        if (yearhead == "")
                        {
                            yearhead = year4;
                        }
                        else
                        {
                            yearhead = yearhead + ", " + year4;
                        }
                    }

                    FpSpread1.Sheets[0].SetRowMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    // ---------------- query1 for Bind Year eg: I YEAR, II YEAR, III YEAR and APPEARED, PASS, PASS% end
                }

                // ----------------------- For Dept Acronym eg: EEE, ECE, CSC
                Hashtable hattotal = new Hashtable();
                Hashtable hatappear = new Hashtable();
                Hashtable hatpass = new Hashtable();
                Hashtable htapprrow = new Hashtable();
                Hashtable htpassrow = new Hashtable();
                string year = "", dptc = "", byr = "", byr1 = ""; int rcount = 0, fmnth = 1; Boolean flagmnth = false;

                // ---------------------- For Batch year eg: 2014, 2015
                for (int i = 0; i < chcklistbatch.Items.Count; i++)
                {
                    if (chcklistbatch.Items[i].Selected == true)
                    {
                        string build = chcklistbatch.Items[i].Value.ToString();
                        int startrow = FpSpread1.Sheets[0].RowCount;

                        // ---------------------- For Degree Code eg: 45
                        for (int kk = 0; kk < cbldept.Items.Count; kk++)
                        {
                            if (cbldept.Items[kk].Selected == true)
                            {
                                string build1 = cbldept.Items[kk].Value.ToString();
                                startrow = FpSpread1.Sheets[0].RowCount;

                                if (ddltest.Items.Count > 0)
                                {
                                    if (headfalg == true)
                                    {
                                        // ------------ query2 for Strength & Pass Count                              
                                        //string query2 = "select isnull(count(distinct rt.roll_no),0) as strength, dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester from result r,registration rt,CriteriaForInternal c,Exam_type e,Degree dg,course cs,Department dp where r.exam_code=e.exam_code and e.criteria_no=c.Criteria_no and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' group by dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester select isnull(count(distinct rt.roll_no),0) as pass,dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester from result r,registration rt ,CriteriaForInternal c,Exam_type e,Degree dg,course cs,Department dp where r.exam_code=e.exam_code and e.criteria_no=c.Criteria_no and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3'or marks_obtained='-1') and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' group by dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester select isnull(count(distinct rt.roll_no),0) as fail, dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester from result r,registration rt,CriteriaForInternal c,Exam_type e,Degree dg,course cs,Department dp where r.exam_Code=e.exam_code and e.criteria_no=c.Criteria_no and rt.roll_no=r.roll_no and rt.degree_code = '" + build1 + "' and rt.batch_year = '" + build + "'  and (r.marks_obtained<e.min_mark and r.marks_obtained<>'-3' and r.marks_obtained<>'-2') and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' group by dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester ";

                                        // ------ old 1
                                        //string query2 = "select isnull(count(distinct rt.roll_no),0) as strength, dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester from result r,registration rt,CriteriaForInternal c,Exam_type e,Degree dg,course cs,Department dp where r.exam_code=e.exam_code and e.criteria_no=c.Criteria_no and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' group by dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester select isnull(count(distinct rt.roll_no),0) as pass,dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester from result r,registration rt ,CriteriaForInternal c,Exam_type e,Degree dg,course cs,Department dp where r.exam_code=e.exam_code and e.criteria_no=c.Criteria_no and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' group by dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester select isnull(count(distinct rt.roll_no),0) as fail, rt.Batch_Year,rt.degree_code,dp.dept_acronym,dp.Dept_Name,rt.Current_Semester from result r,registration rt,CriteriaForInternal c,Exam_type e,syllabus_master sy,subject s,Degree dg,course cs,Department dp where r.exam_Code=e.exam_code and e.criteria_no=c.Criteria_no and rt.roll_no=r.roll_no and sy.Batch_Year=rt.Batch_Year and sy.degree_code=rt.degree_code and sy.semester=rt.Current_Semester and s.syll_code=sy.syll_code and sy.syll_code=c.syll_code and rt.degree_code = '" + build1 + "' and rt.batch_year = '" + build + "'  and (r.marks_obtained<e.min_mark) and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code group by rt.Batch_Year,rt.degree_code,dp.dept_acronym,dp.Dept_Name,rt.Current_Semester";

                                        // ------ old 2
                                        //string query2 = "select isnull(count(distinct rt.roll_no),0) as strength, dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester from result r,registration rt,CriteriaForInternal c,Exam_type e,Degree dg,course cs,Department dp where r.exam_code=e.exam_code and e.criteria_no=c.Criteria_no and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' group by dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester select isnull(count(distinct rt.roll_no),0) as pass,dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester from result r,registration rt ,CriteriaForInternal c,Exam_type e,Degree dg,course cs,Department dp where r.exam_code=e.exam_code and e.criteria_no=c.Criteria_no and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' group by dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester  select isnull(count(distinct rt.roll_no),0) as fail, rt.Batch_Year,rt.degree_code,dp.dept_acronym,dp.Dept_Name,rt.Current_Semester from result r,registration rt,CriteriaForInternal c,Exam_type e,syllabus_master sy,subject s,Degree dg,course cs,Department dp where r.exam_Code=e.exam_code and e.criteria_no=c.Criteria_no and rt.roll_no=r.roll_no and sy.Batch_Year=rt.Batch_Year and sy.degree_code=rt.degree_code and sy.semester=rt.Current_Semester and s.syll_code=sy.syll_code and sy.syll_code=c.syll_code and rt.degree_code = '" + build1 + "' and rt.batch_year = '" + build + "'  and (r.marks_obtained<e.min_mark) and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code group by rt.Batch_Year,rt.degree_code,dp.dept_acronym,dp.Dept_Name,rt.Current_Semester";

                                        string query2 = "select isnull(count(distinct rt.roll_no),0) as strength, dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester,syll_code from result r,registration rt,CriteriaForInternal c,Exam_type e,Degree dg,course cs,Department dp where r.exam_code=e.exam_code and e.criteria_no=c.Criteria_no and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' and rt.Sections=e.sections group by dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester ,syll_code select isnull(count(distinct rt.roll_no),0) as pass,dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester from result r,registration rt ,CriteriaForInternal c,Exam_type e,Degree dg,course cs,Department dp where r.exam_code=e.exam_code and e.criteria_no=c.Criteria_no and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' group by dp.dept_acronym,dp.dept_name, rt.Batch_Year,rt.degree_code,rt.Current_Semester  select isnull(count(distinct rt.roll_no),0) as fail, rt.Batch_Year,rt.degree_code,dp.dept_acronym,dp.Dept_Name,rt.Current_Semester from result r,registration rt,CriteriaForInternal c,Exam_type e,syllabus_master sy,subject s,Degree dg,course cs,Department dp where r.exam_Code=e.exam_code and e.criteria_no=c.Criteria_no and rt.roll_no=r.roll_no and sy.Batch_Year=rt.Batch_Year and sy.degree_code=rt.degree_code and s.syll_code=sy.syll_code and sy.syll_code=c.syll_code and rt.degree_code = '" + build1 + "' and rt.batch_year = '" + build + "' and (r.marks_obtained<e.min_mark and marks_obtained<>'-2' and marks_obtained<>'-3') and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and c.criteria = '" + ddltest.SelectedItem.Text + "' and rt.degree_code = '" + build1 + "' and rt.Batch_Year = '" + build + "' and rt.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code group by rt.Batch_Year,rt.degree_code,dp.dept_acronym,dp.Dept_Name,rt.Current_Semester";
                                        DataSet ds12 = da.select_method_wo_parameter(query2, "text");
                                        Hashtable ht2 = new Hashtable();
                                        Hashtable htblr = new Hashtable();
                                        Hashtable ht1 = new Hashtable();
                                        double PASS = 0.0, FAIL = 0.0, PASS1 = 0.0;
                                        DataView dv3 = new DataView();
                                        DataView dv4 = new DataView();
                                        DataView dv2 = new DataView();

                                        if (ds12.Tables[0].Rows.Count > 0)
                                        {
                                            reportfalg = true; fmnth++;
                                            FpSpread1.Sheets[0].AutoPostBack = true;

                                            for (int tr = 0; tr < ds12.Tables[0].Rows.Count; tr++)
                                            {
                                                if (!ht2.ContainsKey(ds12.Tables[0].Rows[tr]["degree_code"].ToString()))
                                                {
                                                    ht2.Add(ds12.Tables[0].Rows[tr]["degree_code"].ToString(), cnt);
                                                    cnt++;

                                                    // ------------- filter Dept eg: ECE, EEE, CSC
                                                    string dptacrnym = ds12.Tables[0].Rows[tr]["dept_acronym"].ToString();
                                                    string dptaccode = ds12.Tables[0].Rows[tr]["degree_code"].ToString();

                                                    // ------------ for strength
                                                    ds12.Tables[0].DefaultView.RowFilter = "dept_acronym='" + dptacrnym + "'";
                                                    dv2 = ds12.Tables[0].DefaultView;

                                                    if (dv2.Count > 0)
                                                    {
                                                        string dept1 = dv2[0]["Batch_Year"].ToString();

                                                        // ------------ for pass
                                                        ds12.Tables[1].DefaultView.RowFilter = "dept_acronym='" + dptacrnym + "' ";
                                                        dv3 = ds12.Tables[1].DefaultView;

                                                        // ------------ for fail
                                                        ds12.Tables[2].DefaultView.RowFilter = "dept_acronym='" + dptacrnym + "' ";
                                                        dv4 = ds12.Tables[2].DefaultView;

                                                        int startvalue = 1;

                                                        for (int kg = 0; kg < dv2.Count; kg++)
                                                        {
                                                            double APPEARED = Convert.ToDouble(dv2[kg]["strength"]);

                                                            if (dv3.Count > 0 && kg < dv3.Count)
                                                            {

                                                                PASS1 = Convert.ToDouble(dv3[kg]["pass"]);
                                                            }
                                                            else
                                                            {
                                                                PASS1 = 0.0;
                                                            }

                                                            if (dv4.Count > 0 && kg < dv4.Count)
                                                            {

                                                                FAIL = Convert.ToDouble(dv4[kg]["fail"]);
                                                            }
                                                            else
                                                            {
                                                                FAIL = 0.0;
                                                            }

                                                            PASS = PASS1 - FAIL;
                                                            double percentage = PASS / APPEARED * 100;
                                                            percentage = Math.Round(percentage, 2);
                                                            string byear = dv2[kg]["Batch_Year"].ToString();
                                                            // ------------- Bind Dept Acronym eg: EEE, ECE, CSC start
                                                            if (dptc.Contains(dptaccode) != dptaccode.Contains(dptaccode))
                                                            {
                                                                if (!htblr.ContainsKey(dv2[tr]["degree_code"].ToString() + "-" + dv2[tr]["Batch_Year"].ToString()))
                                                                {
                                                                    htblr.Add(dv2[tr]["degree_code"].ToString() + "-" + dv2[tr]["Batch_Year"].ToString(), cn1);
                                                                    cn1++;
                                                                    FpSpread1.Sheets[0].RowCount++;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = dptacrnym;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                                                    if (year == "")
                                                                    {
                                                                        year = dptacrnym;
                                                                        dptc = dptaccode;
                                                                        byr = byear;
                                                                    }
                                                                    else
                                                                    {
                                                                        year = year + "," + dptacrnym;
                                                                        dptc = dptc + "," + dptaccode;
                                                                        byr = byr + "," + byear;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                startrow = 0;
                                                                string[] array = year.Split(',');
                                                                if (array.Length > 0)
                                                                {
                                                                    for (int jv = 0; jv < array.Length; jv++)
                                                                    {
                                                                        string arry = array[jv].ToString();
                                                                        if (dptacrnym == arry)
                                                                        {
                                                                            startrow = jv;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = dptaccode;
                                                            // ------------- Bind Dept Acronym eg: EEE, ECE, CSC end

                                                            string getsem = dv2[kg]["Current_Semester"].ToString();
                                                            string getsem2 = dv2[kg]["Current_Semester"].ToString();
                                                            if (getsem != "")
                                                            {
                                                                if (getsem == "1" || getsem == "2")
                                                                {
                                                                    getsem = "I YEAR";
                                                                }
                                                                else if (getsem == "3" || getsem == "4")
                                                                {
                                                                    getsem = "II YEAR";
                                                                }
                                                                else if (getsem == "5" || getsem == "6")
                                                                {
                                                                    getsem = "III YEAR";
                                                                }
                                                                else if (getsem == "7" || getsem == "8")
                                                                {
                                                                    getsem = "IV YEAR";
                                                                }
                                                                else if (getsem == "9" || getsem == "10")
                                                                {
                                                                    getsem = "V YEAR";
                                                                }
                                                            }
                                                            string getdegcode1 = dv2[kg]["degree_code"].ToString();

                                                            // -------------------- Bind Strength, Pass Count end
                                                            for (int c = startvalue; c < FpSpread1.Sheets[0].ColumnCount; c++)
                                                            {
                                                                string getsem1 = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, c].Tag);
                                                                string getdeptcode = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag);
                                                                if (getsem == getsem1 && getdegcode1 == getdeptcode)
                                                                {
                                                                    for (int r = startrow; r <= FpSpread1.Sheets[0].RowCount; r++)
                                                                    {
                                                                        if (!htblr.ContainsKey(dv2[kg]["Batch_Year"].ToString() + "-" + dv2[kg]["degree_code"].ToString() + "-" + dv2[kg]["Current_Semester"].ToString()))
                                                                        {
                                                                            htblr.Add(dv2[kg]["Batch_Year"].ToString() + "-" + dv2[kg]["degree_code"].ToString() + "-" + dv2[kg]["Current_Semester"].ToString(), cnt);
                                                                            cnt++;
                                                                            FpSpread1.Sheets[0].Cells[r, c].Text = APPEARED.ToString();
                                                                            FpSpread1.Sheets[0].Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread1.Sheets[0].Cells[r, c + 1].Text = PASS.ToString();
                                                                            FpSpread1.Sheets[0].Cells[r, c + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread1.Sheets[0].Cells[r, c + 2].Text = percentage.ToString();
                                                                            FpSpread1.Sheets[0].Cells[r, c + 2].ForeColor = System.Drawing.Color.Brown;
                                                                            FpSpread1.Sheets[0].Cells[r, c + 2].HorizontalAlign = HorizontalAlign.Center;

                                                                            // ----------- For Appeared Row-wise
                                                                            if (!htapprrow.Contains(Convert.ToString(r)))
                                                                            {
                                                                                htapprrow.Add(Convert.ToString(r), APPEARED);
                                                                            }
                                                                            else
                                                                            {
                                                                                string prev1row = Convert.ToString(htapprrow[Convert.ToString(r)]);
                                                                                if (prev1row.Trim() != "")
                                                                                {
                                                                                    double totalp1row = Convert.ToDouble(prev1row) + APPEARED;
                                                                                    htapprrow.Remove(Convert.ToString(r));
                                                                                    htapprrow.Add(Convert.ToString(r), totalp1row);
                                                                                }
                                                                            }

                                                                            // ----------- For Pass Row-wise
                                                                            if (!htpassrow.Contains(Convert.ToString(r)))
                                                                            {
                                                                                htpassrow.Add(Convert.ToString(r), PASS);
                                                                            }
                                                                            else
                                                                            {
                                                                                string prev2row = Convert.ToString(htpassrow[Convert.ToString(r)]);
                                                                                if (prev2row.Trim() != "")
                                                                                {
                                                                                    double totalp2row = Convert.ToDouble(prev2row) + PASS;
                                                                                    htpassrow.Remove(Convert.ToString(r));
                                                                                    htpassrow.Add(Convert.ToString(r), totalp2row);
                                                                                }
                                                                            }

                                                                            // ----------- For Appeared Column-wise
                                                                            if (!hatappear.Contains(Convert.ToString(c)))
                                                                            {
                                                                                hatappear.Add(Convert.ToString(c), APPEARED);
                                                                            }
                                                                            else
                                                                            {
                                                                                string prev1 = Convert.ToString(hatappear[Convert.ToString(c)]);
                                                                                if (prev1.Trim() != "")
                                                                                {
                                                                                    double totalp1 = Convert.ToDouble(prev1) + APPEARED;
                                                                                    hatappear.Remove(Convert.ToString(c));
                                                                                    hatappear.Add(Convert.ToString(c), totalp1);
                                                                                }
                                                                            }

                                                                            // ----------- For Pass Column-wise
                                                                            if (!hatpass.Contains(Convert.ToString(c + 1)))
                                                                            {
                                                                                hatpass.Add(Convert.ToString(c + 1), PASS);
                                                                            }
                                                                            else
                                                                            {
                                                                                string prev2 = Convert.ToString(hatpass[Convert.ToString(c + 1)]);
                                                                                if (prev2.Trim() != "")
                                                                                {
                                                                                    double totalp2 = Convert.ToDouble(prev2) + PASS;
                                                                                    hatpass.Remove(Convert.ToString(c + 1));
                                                                                    hatpass.Add(Convert.ToString(c + 1), totalp2);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            } // -------------------- Bind Strength, Pass Count end
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lblerrormsg.Text = "No Records Found";
                                            lblerrormsg.Visible = true;
                                            FpSpread1.Visible = false;
                                            lblexportxl.Visible = false;
                                            txtexcell.Visible = false;
                                            btnexcel.Visible = false;
                                            btnprint.Visible = false;
                                            lblerror.Visible = false;
                                            Printcontrol.Visible = false;
                                        }
                                    }
                                }
                                else
                                {
                                    fmnth = 1; flagmnth = true;
                                    lblerrormsg.Text = "Please Select Test";
                                    lblerrormsg.Visible = true;
                                    FpSpread1.Visible = false;
                                    lblexportxl.Visible = false;
                                    txtexcell.Visible = false;
                                    btnexcel.Visible = false;
                                    btnprint.Visible = false;
                                    lblerror.Visible = false;
                                    Printcontrol.Visible = false;
                                }
                            }
                        }
                    }
                    rcount = Convert.ToInt32(FpSpread1.Sheets[0].Rows.Count);
                }

                // ----------- Calculating Total Column-wise start --- End Total
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "TOTAL";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = System.Drawing.Color.Indigo;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.Thistle;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                // ----------- Calculating Total Row-wise
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "TOTAL";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Tag = head1;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].ForeColor = System.Drawing.Color.White;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].BackColor = System.Drawing.Color.Teal;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].Text = "APPEARED";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].Tag = head1;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].ForeColor = System.Drawing.Color.White;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].BackColor = System.Drawing.Color.Teal;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 13].Font.Name = "Book Antiqua";
                lm1++;

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 14].Text = "PASS";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 14].Tag = head1;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 14].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 14].ForeColor = System.Drawing.Color.White;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 14].BackColor = System.Drawing.Color.Teal;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 14].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 14].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 14].Font.Name = "Book Antiqua";

                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 15].Text = "PASS%";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 15].Tag = head1;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 15].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 15].ForeColor = System.Drawing.Color.White;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 15].BackColor = System.Drawing.Color.Teal;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 15].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 15].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 15].Font.Name = "Book Antiqua";
                lm1++; lk++;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 13, 1, 3);

                int jh = 1, jh1 = 1, jh2 = 1, jh3 = 1, jh4 = 1, jh5 = 1;
                double totapp = 0.0, totpass = 0.0, perappear = 0.0, perpass = 0.0, val = 0.0, val1 = 0.0, perappear1 = 0.0, perpass1 = 0.0, perappear2 = 0.0, perpass2 = 0.0, perappear3 = 0.0, perpass3 = 0.0;

                if (FpSpread1.Sheets[0].ColumnCount > 0)
                {
                    int colcount = Convert.ToInt32(FpSpread1.Sheets[0].ColumnCount - 1);
                    int colcount1 = Convert.ToInt32(FpSpread1.Sheets[0].ColumnCount - 4);
                    int valcount = Convert.ToInt32(colcount) / 3;
                    int valcount1 = Convert.ToInt32(colcount1) / 3;
                    int valcount12 = Convert.ToInt32(colcount1) / 3;
                    int rowcount = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);

                    for (int col = 1; col <= colcount1; col++)
                    {
                        for (int row = 0; row < rowcount; row++)
                        {
                            int row1 = row + 1;
                            if (colcount1 != col)
                            {
                                if (valcount > row1)
                                {
                                    if (valcount1 >= col)
                                    {
                                        if (jh == 1)
                                        {
                                            valcount1--;
                                            perappear = Convert.ToDouble(hatappear[Convert.ToString(col)]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(perappear);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = System.Drawing.Color.Indigo;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = System.Drawing.Color.Thistle;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            totapp = totapp + perappear;

                                            perpass = Convert.ToDouble(hatpass[Convert.ToString(col + 1)]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].Text = Convert.ToString(perpass);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].ForeColor = System.Drawing.Color.Indigo;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].BackColor = System.Drawing.Color.Thistle;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].Font.Name = "Book Antiqua";
                                            totpass = totpass + perpass;

                                            double percentage1 = perpass / perappear * 100;
                                            percentage1 = Math.Round(percentage1, 2);

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].ForeColor = System.Drawing.Color.Indigo;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].BackColor = System.Drawing.Color.Thistle;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                            jh++;
                                        }
                                        else
                                        {
                                            if (valcount1 >= col)
                                            {
                                                if (jh1 == 1)
                                                {
                                                    valcount1--;
                                                    perappear1 = Convert.ToDouble(hatappear[Convert.ToString(col + 3)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].Text = Convert.ToString(perappear1);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].ForeColor = System.Drawing.Color.Indigo;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].BackColor = System.Drawing.Color.Thistle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].Font.Name = "Book Antiqua";
                                                    totapp = totapp + perappear;

                                                    perpass1 = Convert.ToDouble(hatpass[Convert.ToString(col + 4)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].Text = Convert.ToString(perpass1);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].ForeColor = System.Drawing.Color.Indigo;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].BackColor = System.Drawing.Color.Thistle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].Font.Name = "Book Antiqua";
                                                    totpass = totpass + perpass;

                                                    double percentage1 = perpass1 / perappear1 * 100;
                                                    percentage1 = Math.Round(percentage1, 2);

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].ForeColor = System.Drawing.Color.Indigo;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].BackColor = System.Drawing.Color.Thistle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].Font.Name = "Book Antiqua";
                                                    jh1++;
                                                }
                                                else
                                                {
                                                    if (valcount1 >= col)
                                                    {
                                                        if (jh2 == 1)
                                                        {
                                                            valcount1--;

                                                            if (valcount1 != 0)
                                                            {
                                                                perappear2 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 6)]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].Text = Convert.ToString(perappear2);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].Font.Name = "Book Antiqua";
                                                                totapp = totapp + perappear;

                                                                perpass2 = Convert.ToDouble(hatpass[Convert.ToString(valcount1 + 7)]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Text = Convert.ToString(perpass2);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Name = "Book Antiqua";
                                                                totpass = totpass + perpass;

                                                                double percentage1 = perpass2 / perappear2 * 100;
                                                                percentage1 = Math.Round(percentage1, 2);

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Name = "Book Antiqua";
                                                                jh2++;
                                                            }
                                                            else
                                                            {
                                                                perappear2 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 7)]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Text = Convert.ToString(perappear2);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Name = "Book Antiqua";
                                                                totapp = totapp + perappear;

                                                                perpass2 = Convert.ToDouble(hatpass[Convert.ToString(valcount1 + 8)]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Text = Convert.ToString(perpass2);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Name = "Book Antiqua";
                                                                totpass = totpass + perpass;

                                                                double percentage1 = perpass2 / perappear2 * 100;
                                                                percentage1 = Math.Round(percentage1, 2);

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Name = "Book Antiqua";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            perappear3 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 9)]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Text = Convert.ToString(perappear3);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].ForeColor = System.Drawing.Color.Indigo;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].BackColor = System.Drawing.Color.Thistle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Name = "Book Antiqua";
                                                            totapp = totapp + perappear;

                                                            perpass3 = Convert.ToDouble(hatpass[Convert.ToString(col + 10)]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].Text = Convert.ToString(perpass3);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].ForeColor = System.Drawing.Color.Indigo;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].BackColor = System.Drawing.Color.Thistle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].Font.Name = "Book Antiqua";
                                                            totpass = totpass + perpass;

                                                            double percentage1 = perpass3 / perappear3 * 100;
                                                            percentage1 = Math.Round(percentage1, 2);

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].ForeColor = System.Drawing.Color.Indigo;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].BackColor = System.Drawing.Color.Thistle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].Font.Name = "Book Antiqua";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (valcount1 <= 1)
                                        {
                                            valcount1--;

                                            if (valcount1 > 0)
                                            {
                                                if (jh4 == 1)
                                                {
                                                    perappear3 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 7)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Text = Convert.ToString(perappear3);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].ForeColor = System.Drawing.Color.Indigo;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].BackColor = System.Drawing.Color.Thistle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Name = "Book Antiqua";
                                                    totapp = totapp + perappear;

                                                    perpass3 = Convert.ToDouble(hatpass[Convert.ToString(valcount1 + 8)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Text = Convert.ToString(perpass3);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].ForeColor = System.Drawing.Color.Indigo;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].BackColor = System.Drawing.Color.Thistle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Name = "Book Antiqua";
                                                    totpass = totpass + perpass;

                                                    double percentage1 = perpass3 / perappear3 * 100;
                                                    percentage1 = Math.Round(percentage1, 2);

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].ForeColor = System.Drawing.Color.Indigo;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].BackColor = System.Drawing.Color.Thistle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Name = "Book Antiqua";
                                                    jh4++;
                                                }
                                                else
                                                {
                                                    perappear2 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 7)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Text = Convert.ToString(perappear2);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].ForeColor = System.Drawing.Color.Indigo;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].BackColor = System.Drawing.Color.Thistle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Name = "Book Antiqua";
                                                    totapp = totapp + perappear;

                                                    perpass2 = Convert.ToDouble(hatpass[Convert.ToString(valcount1 + 8)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Text = Convert.ToString(perpass2);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].ForeColor = System.Drawing.Color.Indigo;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].BackColor = System.Drawing.Color.Thistle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Name = "Book Antiqua";
                                                    totpass = totpass + perpass;

                                                    double percentage1 = perpass2 / perappear2 * 100;
                                                    percentage1 = Math.Round(percentage1, 2);

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].ForeColor = System.Drawing.Color.Indigo;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].BackColor = System.Drawing.Color.Thistle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Name = "Book Antiqua";
                                                }
                                            }
                                            else
                                            {
                                                if (valcount1 == 0)
                                                {
                                                    if (jh5 == 1)
                                                    {
                                                        perappear2 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 7)]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Text = Convert.ToString(perappear2);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Name = "Book Antiqua";
                                                        totapp = totapp + perappear;

                                                        perpass2 = Convert.ToDouble(hatpass[Convert.ToString(valcount1 + 8)]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Text = Convert.ToString(perpass2);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Name = "Book Antiqua";
                                                        totpass = totpass + perpass;

                                                        double percentage1 = perpass2 / perappear2 * 100;
                                                        percentage1 = Math.Round(percentage1, 2);

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Name = "Book Antiqua";
                                                        jh5++;
                                                    }
                                                }
                                                else
                                                {
                                                    if (valcount12 != 2)
                                                    {
                                                        if (valcount1 == -1 && row1 > valcount12 || valcount1 == -1 && row1 < valcount12)
                                                        {
                                                            int value = valcount1 + 2;
                                                            perappear3 = Convert.ToDouble(hatappear[Convert.ToString(value + 9)]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].Text = Convert.ToString(perappear3);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].ForeColor = System.Drawing.Color.Indigo;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].BackColor = System.Drawing.Color.Thistle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].Font.Name = "Book Antiqua";
                                                            totapp = totapp + perappear;

                                                            perpass3 = Convert.ToDouble(hatpass[Convert.ToString(value + 10)]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].Text = Convert.ToString(perpass3);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].ForeColor = System.Drawing.Color.Indigo;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].BackColor = System.Drawing.Color.Thistle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].Font.Name = "Book Antiqua";
                                                            totpass = totpass + perpass;

                                                            double percentage1 = perpass3 / perappear3 * 100;
                                                            percentage1 = Math.Round(percentage1, 2);

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].ForeColor = System.Drawing.Color.Indigo;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].BackColor = System.Drawing.Color.Thistle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].Font.Name = "Book Antiqua";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            else
                            {
                                if (rowcount != row1)
                                {
                                    double perappear4 = Convert.ToDouble(htapprrow[Convert.ToString(row)]);
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 1].Text = perappear4.ToString();
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Name = "Book Antiqua";
                                    val = val + perappear4;

                                    double pass4 = Convert.ToDouble(htpassrow[Convert.ToString(row)]);
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 2].Text = pass4.ToString();
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Name = "Book Antiqua";
                                    val1 = val1 + pass4;

                                    double percentage2 = pass4 / perappear4 * 100;
                                    percentage2 = Math.Round(percentage2, 2);

                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Text = Convert.ToString(Convert.ToDouble(percentage2));
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].BackColor = System.Drawing.Color.Gainsboro;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].ForeColor = System.Drawing.Color.Brown;
                                }
                                else
                                {
                                    // ----------------- Column-wise Total for End ------> Row-wise Total eg: 1803, 1422
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 1].Text = Convert.ToString(val);
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 1].ForeColor = System.Drawing.Color.Indigo;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 1].BackColor = System.Drawing.Color.Thistle;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 1].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 2].Text = Convert.ToString(val1);
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 2].ForeColor = System.Drawing.Color.Indigo;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 2].BackColor = System.Drawing.Color.Thistle;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 2].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 2].Font.Name = "Book Antiqua";

                                    double totpercen = val1 / val * 100;
                                    totpercen = Math.Round(totpercen, 2);

                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Text = Convert.ToString(Convert.ToDouble(totpercen));
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].ForeColor = System.Drawing.Color.Indigo;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].BackColor = System.Drawing.Color.Thistle;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Name = "Book Antiqua";
                                }
                            }
                        }
                    }
                }
                // ----------- Calculating Total Column-wise end

                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();

                if (reportfalg == true && fmnth > 1)
                {
                    if (reportfalg == true && fmnth > 1)
                    {
                        FpSpread1.Visible = true;
                        Printcontrol.Visible = false;
                        lblexportxl.Visible = true;
                        txtexcell.Visible = true;
                        btnexcel.Visible = true;
                        btnprint.Visible = true;
                        lblerror.Visible = false;
                        Printcontrol.Visible = false;
                        lblerrormsg.Visible = false;
                    }
                }
                else if (flagmnth == true && fmnth == 1)
                {
                    lblerrormsg.Text = "Please Select Test";
                    lblerrormsg.Visible = true;
                    FpSpread1.Visible = false;
                    lblexportxl.Visible = false;
                    txtexcell.Visible = false;
                    btnexcel.Visible = false;
                    btnprint.Visible = false;
                    lblerror.Visible = false;
                    Printcontrol.Visible = false;
                }
                else
                {
                    lblerrormsg.Text = "No Records Found";
                    lblerrormsg.Visible = true;
                    FpSpread1.Visible = false;
                    lblexportxl.Visible = false;
                    txtexcell.Visible = false;
                    btnexcel.Visible = false;
                    btnprint.Visible = false;
                    lblerror.Visible = false;
                    Printcontrol.Visible = false;
                }
            }
            else
            {
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                FpSpread1.Sheets[0].ColumnCount = 1;
                FpSpread1.CommandBar.Visible = false;

                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = System.Drawing.Color.White;
                style2.BackColor = System.Drawing.Color.Teal;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Department";

                Boolean reportfalg = false;
                Hashtable hat = new Hashtable();
                Hashtable ht = new Hashtable();

                // ---------------------------- Batch
                string buildvalue = "";
                for (int i = 0; i < chcklistbatch.Items.Count; i++)
                {
                    if (chcklistbatch.Items[i].Selected == true)
                    {
                        string build = chcklistbatch.Items[i].Value.ToString();
                        if (buildvalue == "")
                        {
                            buildvalue = build;
                        }
                        else
                        {
                            buildvalue = buildvalue + "'" + "," + "'" + build;
                        }
                    }
                }

                // ---------------------------- Dept
                string buildvalue1 = "";
                for (int i = 0; i < cbldept.Items.Count; i++)
                {
                    if (cbldept.Items[i].Selected == true)
                    {
                        string build1 = cbldept.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }

                // ------------------- query for Month and Year eg: 2014, 10
                if (ddlmonth.Items.ToString() != "")
                {
                    string qurymnth = "select distinct ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,e.Exam_year,e.Exam_Month from Exam_Details e,Degree d where e.degree_code=d.Degree_Code and e.batch_year in ('" + buildvalue + "') and d.Degree_Code in ('" + buildvalue1 + "') order by e.Exam_year desc,e.Exam_Month desc";
                    DataSet dsyr = da.select_method_wo_parameter(qurymnth, "text");

                    string head = "", head1 = "", sk5 = "", sk2 = "", buildvalue2 = "", buildvaluea = "", buildvalue1a = "";
                    int cnt = 0; Boolean headfalg = false;

                    int lk = 1, kn = 1, lm = 1, lm1 = 1, hj = 1, hj1 = 1, hj2 = 1, hj3 = 1, hj4 = 1, cn1 = 0, jm = 1;
                    Hashtable htc = new Hashtable();
                    Hashtable htc1 = new Hashtable();
                    Hashtable htc2 = new Hashtable();
                    Hashtable htc3 = new Hashtable();
                    DataView dv1 = new DataView();
                    string y1 = "", y2 = "", y3 = "", y4 = "", y5 = "";

                    // ---------------------------- Batch
                    for (int iw = 0; iw < chcklistbatch.Items.Count; iw++)
                    {
                        if (chcklistbatch.Items[iw].Selected == true)
                        {
                            buildvaluea = chcklistbatch.Items[iw].Value.ToString();

                            // ---------------------------- Dept
                            for (int ia = 0; ia < cbldept.Items.Count; ia++)
                            {
                                if (cbldept.Items[ia].Selected == true)
                                {
                                    buildvalue1a = cbldept.Items[ia].Value.ToString();

                                    // ---------------------------- Month and Year eg: 2014, 10
                                    if (dsyr.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dsyr.Tables[0].Rows.Count; i++)
                                        {
                                            if (ddlmonth.Items.Count > 0)
                                            {
                                                buildvalue2 = ddlmonth.SelectedItem.Text;
                                                string valmnth = dsyr.Tables[0].Rows[i]["monthName"].ToString();

                                                if (buildvalue2 == valmnth)
                                                {
                                                    string state_value = dsyr.Tables[0].Rows[i]["Exam_month"].ToString();
                                                    sk2 = state_value;

                                                    string state_value1 = dsyr.Tables[0].Rows[i]["Exam_year"].ToString();
                                                    sk5 = state_value1;
                                                }
                                            }
                                        }
                                    }

                                    // ---------------- query1 for Bind Year eg: I YEAR, II YEAR, III YEAR  and APPEARED, PASS, PASS% start
                                    //string query1 = "select distinct ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,e.current_semester, e.Exam_year,e.Exam_Month,d.Degree_Code,c.Course_Name,de.dept_acronym from Exam_Details e,mark_entry m,Degree d,Course c,Department de where m.exam_code=e.exam_code and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and e.batch_year in ('" + buildvalue + "') and e.degree_code in ('" + buildvalue1 + "') and e.Exam_year in ('" + sk5 + "') and e.Exam_Month in ('" + sk2 + "') order by e.current_semester,d.Degree_Code,e.Exam_year,e.Exam_Month";

                                    string query1 = "select distinct e.current_semester,d.Degree_Code from Exam_Details e,mark_entry m,Degree d,Course c,Department de,Registration r where m.exam_code=e.exam_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and e.degree_code=d.Degree_Code and r.degree_code=d.Degree_Code and r.CC=0 and r.Exam_Flag<>'debar' and r.DelFlag=0 and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and e.batch_year in ('" + buildvaluea + "') and e.degree_code in ('" + buildvalue1a + "') and e.Exam_year = '" + sk5 + "' and e.Exam_Month = '" + sk2 + "' order by e.current_semester";
                                    DataSet ds1 = ds1 = da.select_method_wo_parameter(query1, "text");

                                    if (ddlmonth.Items.Count > 0)
                                    {
                                        if (ds1.Tables[0].Rows.Count > 0)
                                        {
                                            headfalg = true;
                                            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                                            {
                                                head = ds1.Tables[0].Rows[i]["current_semester"].ToString();
                                                head1 = ds1.Tables[0].Rows[i]["Degree_Code"].ToString();

                                                ds1.Tables[0].DefaultView.RowFilter = "Degree_Code='" + head1 + "' and current_semester='" + head + "'";
                                                if (!ht.ContainsKey(ds1.Tables[0].Rows[i]["current_semester"].ToString() + "-" + ds1.Tables[0].Rows[i]["Degree_Code"].ToString()))
                                                {
                                                    ht.Add(ds1.Tables[0].Rows[i]["current_semester"].ToString() + "-" + ds1.Tables[0].Rows[i]["Degree_Code"].ToString(), cnt);
                                                    cnt++;
                                                    dv1 = ds1.Tables[0].DefaultView;

                                                    if (dv1.Count > 0)
                                                    {
                                                        for (int dvyr = 0; dvyr < dv1.Count; dvyr++)
                                                        {
                                                            string semyear = dv1[dvyr]["current_semester"].ToString();

                                                            // --------------  Heading for Bind Year eg: I YEAR, II YEAR, III YEAR start
                                                            if (semyear == "1" || semyear == "2")
                                                            {
                                                                if (!htc.ContainsKey(ds1.Tables[0].Rows[i]["current_semester"].ToString()))
                                                                {
                                                                    if (hj == 1)
                                                                    {
                                                                        htc.Add(ds1.Tables[0].Rows[i]["current_semester"].ToString(), cnt);
                                                                        cnt++;
                                                                        FpSpread1.Sheets[0].ColumnCount++;
                                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "I YEAR";
                                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "I YEAR";

                                                                        hj++;
                                                                        y1 = "I YEAR"; year1 = "I";
                                                                    }
                                                                }
                                                            }
                                                            else if (semyear == "3" || semyear == "4")
                                                            {
                                                                if (!htc.ContainsKey(ds1.Tables[0].Rows[i]["current_semester"].ToString()))
                                                                {
                                                                    if (hj1 == 1)
                                                                    {
                                                                        htc.Add(ds1.Tables[0].Rows[i]["current_semester"].ToString(), cnt);
                                                                        cnt++;
                                                                        FpSpread1.Sheets[0].ColumnCount++;
                                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "II YEAR";
                                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "II YEAR";
                                                                        hj1++; y2 = "II YEAR"; year2 = "II";
                                                                    }
                                                                }
                                                            }
                                                            else if (semyear == "5" || semyear == "6")
                                                            {
                                                                if (!htc.ContainsKey(ds1.Tables[0].Rows[i]["current_semester"].ToString()))
                                                                {
                                                                    if (hj2 == 1)
                                                                    {
                                                                        htc.Add(ds1.Tables[0].Rows[i]["current_semester"].ToString(), cnt);
                                                                        cnt++;
                                                                        FpSpread1.Sheets[0].ColumnCount++;
                                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "III YEAR";
                                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "III YEAR";
                                                                        hj2++; y3 = "III YEAR"; year3 = "III";
                                                                    }
                                                                }
                                                            }
                                                            else if (semyear == "7" || semyear == "8")
                                                            {
                                                                if (!htc.ContainsKey(ds1.Tables[0].Rows[i]["current_semester"].ToString()))
                                                                {
                                                                    if (hj3 == 1)
                                                                    {
                                                                        htc.Add(ds1.Tables[0].Rows[i]["current_semester"].ToString(), cnt);
                                                                        cnt++;
                                                                        FpSpread1.Sheets[0].ColumnCount++;
                                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "IV YEAR";
                                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "IV YEAR";
                                                                        hj3++; y4 = "IV YEAR"; year4 = "IV";
                                                                    }
                                                                }
                                                            }
                                                            else if (semyear == "9" || semyear == "10")
                                                            {
                                                                if (!htc.ContainsKey(ds1.Tables[0].Rows[i]["current_semester"].ToString()))
                                                                {
                                                                    if (hj4 == 1)
                                                                    {
                                                                        htc.Add(ds1.Tables[0].Rows[i]["current_semester"].ToString(), cnt);
                                                                        cnt++;
                                                                        FpSpread1.Sheets[0].ColumnCount++;
                                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "V YEAR";
                                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "V YEAR";
                                                                        hj4++; y5 = "V YEAR"; year5 = "V";
                                                                    }
                                                                }
                                                            }
                                                            // --------------  Heading for Bind Year eg: I YEAR, II YEAR, III YEAR end

                                                            // --------------  Heading for Bind eg: APPEARED, PASS, PASS% start
                                                            if (y1 == "I YEAR" || y2 == "II YEAR" || y3 == "III YEAR" || y4 == "IV YEAR" || y5 == "V YEAR")
                                                            {
                                                                if (kn == 1)
                                                                {
                                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Text = "APPEARED";
                                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Tag = head1;
                                                                    kn++; lm++;
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "APPEARED";
                                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = head1;
                                                                    lm++;
                                                                }

                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "PASS";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = head1;

                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "PASS%";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = head1;
                                                                lm++;
                                                            }
                                                            // --------------  Heading for Bind eg: APPEARED, PASS, PASS% start
                                                        }
                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = System.Drawing.Color.White;
                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.Color.Teal;
                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                        y1 = ""; y2 = ""; y3 = ""; y4 = ""; y5 = "";
                                                        // --------------  Heading for Bind Year eg: I YEAR, II YEAR, III YEAR end

                                                        // --------------------------- ColumnHeader Spanning start
                                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - lm, 1, lm);
                                                        // --------------------------- ColumnHeader Spanning end
                                                        lm = 1;

                                                        // ---------- For Year I, II, III, IV in PDF start
                                                        if (year1 != "")
                                                        {
                                                            yearhead = year1;
                                                        }
                                                        if (year2 != "")
                                                        {
                                                            if (yearhead.Contains(year2) != year2.Contains(year2))
                                                            {
                                                                if (year1 == "")
                                                                {
                                                                    yearhead = year2;
                                                                }
                                                                else
                                                                {
                                                                    yearhead = yearhead + ", " + year2;
                                                                }
                                                            }
                                                        }
                                                        if (year3 != "")
                                                        {
                                                            if (yearhead.Contains(year3) != year3.Contains(year3))
                                                            {
                                                                if (yearhead == "")
                                                                {
                                                                    yearhead = year3;
                                                                }
                                                                else
                                                                {
                                                                    yearhead = yearhead + ", " + year3;
                                                                }
                                                            }
                                                        }
                                                        if (year4 != "")
                                                        {
                                                            if (yearhead.Contains(year4) != year4.Contains(year4))
                                                            {
                                                                if (yearhead == "")
                                                                {
                                                                    yearhead = year4;
                                                                }
                                                                else
                                                                {
                                                                    yearhead = yearhead + ", " + year4;
                                                                }
                                                            }
                                                        }

                                                        //if (year1 != "")
                                                        //{
                                                        //    yearhead = year1;
                                                        //}
                                                        //if (year2 != "")
                                                        //{
                                                        //    if (yearhead.Contains(year2) != year2.Contains(year2))
                                                        //    {
                                                        //        if (year1 == "")
                                                        //        {
                                                        //            yearhead = year2;
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            yearhead = yearhead + ", " + year2;
                                                        //        }
                                                        //    }
                                                        //}
                                                        //if (year3 != "")
                                                        //{
                                                        //    if (yearhead.Contains(year3) != year3.Contains(year3))
                                                        //    {
                                                        //        yearhead = yearhead + ", " + year3;
                                                        //    }
                                                        //}
                                                        //if (year4 != "")
                                                        //{
                                                        //    if (yearhead.Contains(year4) != year4.Contains(year4))
                                                        //    {
                                                        //        yearhead = yearhead + ", " + year4;
                                                        //    }
                                                        //}
                                                        // ---------- For Year I, II, III, IV in PDF end
                                                    }
                                                }
                                            }
                                            FpSpread1.Sheets[0].SetRowMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                    }
                    // ---------------- query1 for Bind Year eg: I YEAR, II YEAR, III YEAR and APPEARED, PASS, PASS% end


                    // ----------------------- For Dept Acronym eg: EEE, ECE, CSC
                    Hashtable hattotal = new Hashtable();
                    Hashtable hatappear = new Hashtable();
                    Hashtable hatpass = new Hashtable();
                    Hashtable htapprrow = new Hashtable();
                    Hashtable htpassrow = new Hashtable();
                    string year = ""; int rcount = 0, fmnth = 1; Boolean flagmnth = false;

                    // ---------------------- For Batch year eg: 2014, 2015
                    for (int i = 0; i < chcklistbatch.Items.Count; i++)
                    {
                        if (chcklistbatch.Items[i].Selected == true)
                        {
                            string build = chcklistbatch.Items[i].Value.ToString();
                            int startrow = FpSpread1.Sheets[0].RowCount;

                            // ---------------------- For Degree Code eg: 45
                            for (int kk = 0; kk < cbldept.Items.Count; kk++)
                            {
                                if (cbldept.Items[kk].Selected == true)
                                {
                                    string build1 = cbldept.Items[kk].Value.ToString();
                                    startrow = FpSpread1.Sheets[0].RowCount;

                                    // ------------------- For Month and Year eg: May-2014
                                    if (dsyr.Tables[0].Rows.Count > 0)
                                    {
                                        for (int ig = 0; ig < dsyr.Tables[0].Rows.Count; ig++)
                                        {
                                            if (ddlmonth.Items.Count > 0)
                                            {
                                                buildvalue2 = ddlmonth.SelectedItem.Text;
                                                string valmnth = dsyr.Tables[0].Rows[ig]["monthName"].ToString();

                                                if (buildvalue2 == valmnth)
                                                {
                                                    string state_value = dsyr.Tables[0].Rows[ig]["Exam_month"].ToString();
                                                    sk2 = state_value;

                                                    string state_value1 = dsyr.Tables[0].Rows[ig]["Exam_year"].ToString();
                                                    sk5 = state_value1;
                                                }
                                            }
                                            else
                                            {
                                                flagmnth = true;
                                                lblerrormsg.Text = "Please Select Month & Year";
                                                lblerrormsg.Visible = true;
                                                FpSpread1.Visible = false;
                                                lblexportxl.Visible = false;
                                                txtexcell.Visible = false;
                                                btnexcel.Visible = false;
                                                btnprint.Visible = false;
                                                lblerror.Visible = false;
                                                Printcontrol.Visible = false;
                                            }
                                        }
                                    }

                                    if (dsyr.Tables[0].Rows.Count > 0)
                                    {
                                        if (headfalg == true)
                                        {
                                            // ------------ query2 for Strength & Pass Count
                                            //string query2 = "select count(distinct m.roll_no) as strength,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,Exam_Month,Exam_year,r.Batch_Year from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' group by r.degree_code,Exam_Month,Exam_year,r.Batch_Year,r.degree_code,r.Batch_Year order by r.Batch_Year desc,r.degree_code select count(distinct m.roll_no) as pass,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' and m.roll_no not in(select m1.roll_no from mark_entry m1,subject s1,syllabus_master sy1 ,Registration r1,Exam_Details ed1 where m1.exam_code=ed1.exam_code and sy1.syll_code=s1.syll_code and sy1.Batch_Year=ed1.batch_year and m1.roll_no = r1.Roll_No and ed1.degree_code=sy1.degree_code and r1.Batch_Year=sy1.Batch_Year and sy1.semester=ed1.current_semester and r1.degree_code=sy1.degree_code and m1.subject_no=s1.subject_no and sy1.degree_code = '" + build1 + "' and sy1.Batch_Year = '" + build + "' and m1.result<>'Pass' and ed1.Exam_year in ('" + sk5 + "') and ed1.Exam_Month in ('" + sk2 + "')) group by r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year";

                                            // old query correct
                                            //string query2 = "select count(distinct m.roll_no) as strength,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,ed.Current_Semester,r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year ,dp.dept_acronym,dp.Dept_Name from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s ,Degree dg,course cs,Department dp where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year = '" + sk5 + "' and ed.Exam_Month = '" + sk2 + "' and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' and r.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code group by r.degree_code,ed.Current_Semester,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year,r.degree_code,r.Batch_Year,dept_acronym,dp.Dept_Name order by r.Batch_Year desc,r.degree_code  select count(distinct m.roll_no) as pass,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,ed.Current_Semester,r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year,dept_acronym,dp.Dept_Name from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s,Degree dg,course cs,Department dp where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year = '" + sk5 + "' and ed.Exam_Month  = '" + sk2 + "' and r.degree_code  = '" + build1 + "' and r.Batch_Year = '" + build + "' and m.roll_no not in(select m1.roll_no from mark_entry m1,subject s1,syllabus_master sy1 ,Registration r1,Exam_Details ed1 where m1.exam_code=ed1.exam_code and sy1.syll_code=s1.syll_code and sy1.Batch_Year=ed1.batch_year and m1.roll_no = r1.Roll_No and ed1.degree_code=sy1.degree_code and r1.Batch_Year=sy1.Batch_Year and sy1.semester=ed1.current_semester and r1.degree_code=sy1.degree_code and m1.subject_no=s1.subject_no and sy1.degree_code  = '" + build1 + "' and sy1.Batch_Year = '" + build + "' and m1.result<>'Pass' and ed1.Exam_year = '" + sk5 + "' and ed1.Exam_Month  = '" + sk2 + "') and r.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code group by r.degree_code,ed.Current_Semester,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year,dept_acronym,dp.Dept_Name order by r.Batch_Year desc,r.degree_code ";

                                            string query2 = "select count(distinct m.roll_no) as strength,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,ed.Current_Semester,r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year ,dp.dept_acronym,dp.Dept_Name from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s ,Degree dg,course cs,Department dp where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year = '" + sk5 + "' and ed.Exam_Month = '" + sk2 + "' and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' and r.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code group by r.degree_code,ed.Current_Semester,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year,r.degree_code,r.Batch_Year,dept_acronym,dp.Dept_Name order by r.Batch_Year desc,r.degree_code  select count(distinct m.roll_no) as pass,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,ed.Current_Semester,r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year,dept_acronym,dp.Dept_Name from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s,Degree dg,course cs,Department dp where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.degree_code=r.degree_code and sy.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.Batch_Year=ed.Batch_Year and r.Batch_Year=ed.batch_year and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year = '" + sk5 + "' and ed.Exam_Month  = '" + sk2 + "' and r.degree_code  = '" + build1 + "' and r.Batch_Year = '" + build + "' and m.roll_no not in (select m1.roll_no from mark_entry m1,subject s1,syllabus_master sy1 ,Registration r1,Exam_Details ed1 where m1.exam_code=ed1.exam_code and ed1.exam_code=m1.exam_code and sy1.syll_code=s1.syll_code and sy1.Batch_Year=ed1.batch_year and m1.roll_no = r1.Roll_No and ed1.degree_code=sy1.degree_code and ed1.degree_code=r.degree_code and r1.degree_code=sy1.degree_code and r1.Batch_Year=sy1.Batch_Year and ed1.batch_year=r1.Batch_Year and sy1.Batch_Year=r1.Batch_Year and sy1.semester=ed1.current_semester and m1.subject_no=s1.subject_no and sy1.degree_code  = '" + build1 + "' and sy1.Batch_Year = '" + build + "' and m1.result<>'Pass' and ed1.Exam_year = '" + sk5 + "' and ed1.Exam_Month  = '" + sk2 + "') and r.degree_code = dg.Degree_Code and dg.Course_Id = cs.Course_Id and dg.Dept_Code = dp.Dept_Code group by r.degree_code,ed.Current_Semester,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year,dept_acronym,dp.Dept_Name order by r.Batch_Year desc,r.degree_code ";
                                            DataSet ds12 = da.select_method_wo_parameter(query2, "text");
                                            Hashtable ht2 = new Hashtable();
                                            Hashtable htblr = new Hashtable();
                                            Hashtable ht1 = new Hashtable();
                                            double PASS = 0.0;
                                            DataView dv3 = new DataView();
                                            DataView dv2 = new DataView();

                                            if (ds12.Tables[0].Rows.Count > 0)
                                            {
                                                reportfalg = true; fmnth++;
                                                FpSpread1.Sheets[0].AutoPostBack = true;

                                                for (int tr = 0; tr < ds12.Tables[0].Rows.Count; tr++)
                                                {
                                                    if (!ht2.ContainsKey(ds12.Tables[0].Rows[tr]["degree_code"].ToString()))
                                                    {
                                                        ht2.Add(ds12.Tables[0].Rows[tr]["degree_code"].ToString(), cnt);
                                                        cnt++;

                                                        // ------------- filter Dept eg: ECE, EEE, CSC
                                                        string dptacrnym = ds12.Tables[0].Rows[tr]["dept_acronym"].ToString();
                                                        string dptaccode = ds12.Tables[0].Rows[tr]["degree_code"].ToString();

                                                        string monthnam1 = ds12.Tables[0].Rows[tr]["monthName"].ToString();
                                                        ds12.Tables[0].DefaultView.RowFilter = "dept_acronym='" + dptacrnym + "'and monthName='" + monthnam1 + "' ";
                                                        dv2 = ds12.Tables[0].DefaultView;

                                                        if (dv2.Count > 0)
                                                        {
                                                            string dept1 = dv2[0]["Batch_Year"].ToString();
                                                            string monthnam = dv2[0]["monthName"].ToString();

                                                            ds12.Tables[1].DefaultView.RowFilter = "dept_acronym='" + dptacrnym + "' and monthName='" + monthnam + "' ";
                                                            dv3 = ds12.Tables[1].DefaultView;

                                                            int startvalue = 1;

                                                            for (int kg = 0; kg < dv2.Count; kg++)
                                                            {
                                                                double APPEARED = Convert.ToDouble(dv2[kg]["strength"]);

                                                                if (dv3.Count > 0 && kg < dv3.Count)
                                                                {

                                                                    PASS = Convert.ToDouble(dv3[kg]["pass"]);
                                                                }
                                                                else
                                                                {
                                                                    PASS = 0.0;
                                                                }

                                                                double percentage = PASS / APPEARED * 100;
                                                                percentage = Math.Round(percentage, 2);

                                                                // ------------- Bind Dept Acronym eg: EEE, ECE, CSC start
                                                                if (year.Contains(dptaccode) != dptaccode.Contains(dptaccode))
                                                                {
                                                                    if (!htblr.ContainsKey(dv2[tr]["degree_code"].ToString()))
                                                                    {
                                                                        htblr.Add(dv2[tr]["degree_code"].ToString(), cn1);
                                                                        cn1++;
                                                                        FpSpread1.Sheets[0].RowCount++;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = dptacrnym;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                                                        if (year == "")
                                                                        {
                                                                            year = dptaccode;
                                                                        }
                                                                        else
                                                                        {
                                                                            year = year + "," + dptaccode;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    startrow = 0;
                                                                    string[] array = year.Split(',');
                                                                    if (array.Length > 0)
                                                                    {
                                                                        for (int jv = 0; jv < array.Length; jv++)
                                                                        {
                                                                            string arry = array[jv].ToString();
                                                                            if (dptaccode == arry)
                                                                            {
                                                                                startrow = jv;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = dptaccode;
                                                                // ------------- Bind Dept Acronym eg: EEE, ECE, CSC end

                                                                string getsem = dv2[kg]["Current_Semester"].ToString();
                                                                if (getsem != "")
                                                                {
                                                                    if (getsem == "1" || getsem == "2")
                                                                    {
                                                                        getsem = "I YEAR";
                                                                    }
                                                                    else if (getsem == "3" || getsem == "4")
                                                                    {
                                                                        getsem = "II YEAR";
                                                                    }
                                                                    else if (getsem == "5" || getsem == "6")
                                                                    {
                                                                        getsem = "III YEAR";
                                                                    }
                                                                    else if (getsem == "7" || getsem == "8")
                                                                    {
                                                                        getsem = "IV YEAR";
                                                                    }
                                                                    else if (getsem == "9" || getsem == "10")
                                                                    {
                                                                        getsem = "V YEAR";
                                                                    }
                                                                }
                                                                string getdegcode1 = dv2[kg]["degree_code"].ToString();

                                                                // -------------------- Bind Strength, Pass Count end
                                                                for (int c = startvalue; c < FpSpread1.Sheets[0].ColumnCount; c++)
                                                                {
                                                                    string getsem1 = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, c].Tag);
                                                                    string getdeptcode = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag);
                                                                    if (getsem == getsem1 && getdegcode1 == getdeptcode)
                                                                    {
                                                                        for (int r = startrow; r <= FpSpread1.Sheets[0].RowCount; r++)
                                                                        {
                                                                            //    //FpSpread1.Sheets[0].RowCount++;
                                                                            //    if (getsem == getsem1 && getdegcode1 == getdeptcode)
                                                                            //{
                                                                            if (!htblr.ContainsKey(dv2[kg]["Batch_Year"].ToString() + "-" + dv2[kg]["degree_code"].ToString()))
                                                                            {
                                                                                htblr.Add(dv2[kg]["Batch_Year"].ToString() + "-" + dv2[kg]["degree_code"].ToString(), cnt);
                                                                                cnt++;
                                                                                FpSpread1.Sheets[0].Cells[r, c].Text = APPEARED.ToString();
                                                                                FpSpread1.Sheets[0].Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                                                                                FpSpread1.Sheets[0].Cells[r, c + 1].Text = PASS.ToString();
                                                                                FpSpread1.Sheets[0].Cells[r, c + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                                FpSpread1.Sheets[0].Cells[r, c + 2].Text = percentage.ToString();
                                                                                //FpSpread1.Sheets[0].Cells[r, c + 2].BackColor = System.Drawing.Color.Gainsboro;
                                                                                FpSpread1.Sheets[0].Cells[r, c + 2].ForeColor = System.Drawing.Color.Brown;
                                                                                FpSpread1.Sheets[0].Cells[r, c + 2].HorizontalAlign = HorizontalAlign.Center;

                                                                                // ----------- For Appeared Row-wise
                                                                                if (!htapprrow.Contains(Convert.ToString(r)))
                                                                                {
                                                                                    htapprrow.Add(Convert.ToString(r), APPEARED);
                                                                                }
                                                                                else
                                                                                {
                                                                                    string prev1row = Convert.ToString(htapprrow[Convert.ToString(r)]);
                                                                                    if (prev1row.Trim() != "")
                                                                                    {
                                                                                        double totalp1row = Convert.ToDouble(prev1row) + APPEARED;
                                                                                        htapprrow.Remove(Convert.ToString(r));
                                                                                        htapprrow.Add(Convert.ToString(r), totalp1row);
                                                                                    }
                                                                                }

                                                                                // ----------- For Pass Row-wise
                                                                                if (!htpassrow.Contains(Convert.ToString(r)))
                                                                                {
                                                                                    htpassrow.Add(Convert.ToString(r), PASS);
                                                                                }
                                                                                else
                                                                                {
                                                                                    string prev2row = Convert.ToString(htpassrow[Convert.ToString(r)]);
                                                                                    if (prev2row.Trim() != "")
                                                                                    {
                                                                                        double totalp2row = Convert.ToDouble(prev2row) + PASS;
                                                                                        htpassrow.Remove(Convert.ToString(r));
                                                                                        htpassrow.Add(Convert.ToString(r), totalp2row);
                                                                                    }
                                                                                }

                                                                                // ----------- For Appeared Column-wise
                                                                                if (!hatappear.Contains(Convert.ToString(c)))
                                                                                {
                                                                                    hatappear.Add(Convert.ToString(c), APPEARED);
                                                                                }
                                                                                else
                                                                                {
                                                                                    string prev1 = Convert.ToString(hatappear[Convert.ToString(c)]);
                                                                                    if (prev1.Trim() != "")
                                                                                    {
                                                                                        double totalp1 = Convert.ToDouble(prev1) + APPEARED;
                                                                                        hatappear.Remove(Convert.ToString(c));
                                                                                        hatappear.Add(Convert.ToString(c), totalp1);
                                                                                    }
                                                                                }

                                                                                //----------- For Pass Column-wise
                                                                                if (!hatpass.Contains(Convert.ToString(c + 1)))
                                                                                {
                                                                                    hatpass.Add(Convert.ToString(c + 1), PASS);
                                                                                }
                                                                                else
                                                                                {
                                                                                    string prev2 = Convert.ToString(hatpass[Convert.ToString(c + 1)]);
                                                                                    if (prev2.Trim() != "")
                                                                                    {
                                                                                        double totalp2 = Convert.ToDouble(prev2) + PASS;
                                                                                        hatpass.Remove(Convert.ToString(c + 1));
                                                                                        hatpass.Add(Convert.ToString(c + 1), totalp2);
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                } // -------------------- Bind Strength, Pass Count end
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lblerrormsg.Text = "No Records Found";
                                                lblerrormsg.Visible = true;
                                                FpSpread1.Visible = false;
                                                lblexportxl.Visible = false;
                                                txtexcell.Visible = false;
                                                btnexcel.Visible = false;
                                                btnprint.Visible = false;
                                                lblerror.Visible = false;
                                                Printcontrol.Visible = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        flagmnth = true; fmnth = 1;
                                        lblerrormsg.Text = "Please Select Month & Year";
                                        lblerrormsg.Visible = true;
                                        FpSpread1.Visible = false;
                                        lblexportxl.Visible = false;
                                        txtexcell.Visible = false;
                                        btnexcel.Visible = false;
                                        btnprint.Visible = false;
                                        lblerror.Visible = false;
                                        Printcontrol.Visible = false;
                                    }
                                }
                            }
                        }
                    }

                    // ----------- Calculating Total Column-wise start --- End Total
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "TOTAL";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = System.Drawing.Color.Indigo;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.Thistle;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    // ----------- Calculating Total Row-wise
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "TOTAL";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = head1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = System.Drawing.Color.White;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.Color.Teal;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "APPEARED";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = head1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = System.Drawing.Color.White;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.Color.Teal;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    lm1++;

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "PASS";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = head1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = System.Drawing.Color.White;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.Color.Teal;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "PASS%";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = head1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = System.Drawing.Color.White;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.Color.Teal;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    lm1++; lk++;
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - lm1, 1, lm1);

                    int jh = 1, jh1 = 1, jh2 = 1, jh3 = 1, jh4 = 1, jh5 = 1;
                    double totapp = 0.0, totpass = 0.0, perappear = 0.0, perpass = 0.0, val = 0.0, val1 = 0.0, perappear1 = 0.0, perpass1 = 0.0, perappear2 = 0.0, perpass2 = 0.0, perappear3 = 0.0, perpass3 = 0.0;

                    if (FpSpread1.Sheets[0].ColumnCount > 0)
                    {
                        int colcount = Convert.ToInt32(FpSpread1.Sheets[0].ColumnCount - 1);
                        int colcount1 = Convert.ToInt32(FpSpread1.Sheets[0].ColumnCount - 4);
                        int valcount = Convert.ToInt32(colcount) / 3;
                        int valcount1 = Convert.ToInt32(colcount1) / 3;
                        int valcount12 = Convert.ToInt32(colcount1) / 3;
                        int rowcount = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);

                        for (int col = 1; col <= colcount1; col++)
                        {
                            for (int row = 0; row < rowcount; row++)
                            {
                                int row1 = row + 1;
                                if (colcount1 != col)
                                {
                                    if (valcount > row1)
                                    {
                                        if (valcount1 >= col)
                                        {
                                            if (jh == 1)
                                            {
                                                valcount1--;
                                                perappear = Convert.ToDouble(hatappear[Convert.ToString(col)]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(perappear);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = System.Drawing.Color.Indigo;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = System.Drawing.Color.Thistle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                totapp = totapp + perappear;

                                                perpass = Convert.ToDouble(hatpass[Convert.ToString(col + 1)]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].Text = Convert.ToString(perpass);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].ForeColor = System.Drawing.Color.Indigo;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].BackColor = System.Drawing.Color.Thistle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 1].Font.Name = "Book Antiqua";
                                                totpass = totpass + perpass;

                                                double percentage1 = perpass / perappear * 100;
                                                percentage1 = Math.Round(percentage1, 2);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].ForeColor = System.Drawing.Color.Indigo;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].BackColor = System.Drawing.Color.Thistle;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 2].Font.Name = "Book Antiqua";
                                                jh++;
                                            }
                                            else
                                            {
                                                if (valcount1 >= col)
                                                {
                                                    if (jh1 == 1)
                                                    {
                                                        valcount1--;
                                                        perappear1 = Convert.ToDouble(hatappear[Convert.ToString(col + 3)]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].Text = Convert.ToString(perappear1);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 3].Font.Name = "Book Antiqua";
                                                        totapp = totapp + perappear;

                                                        perpass1 = Convert.ToDouble(hatpass[Convert.ToString(col + 4)]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].Text = Convert.ToString(perpass1);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 4].Font.Name = "Book Antiqua";
                                                        totpass = totpass + perpass;

                                                        double percentage1 = perpass1 / perappear1 * 100;
                                                        percentage1 = Math.Round(percentage1, 2);

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col + 5].Font.Name = "Book Antiqua";
                                                        jh1++;
                                                    }
                                                    else
                                                    {
                                                        if (valcount1 >= col)
                                                        {
                                                            if (jh2 == 1)
                                                            {
                                                                valcount1--;

                                                                if (valcount1 != 0)
                                                                {
                                                                    perappear2 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 6)]);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].Text = Convert.ToString(perappear2);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].ForeColor = System.Drawing.Color.Indigo;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].BackColor = System.Drawing.Color.Thistle;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].Font.Bold = true;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 6].Font.Name = "Book Antiqua";
                                                                    totapp = totapp + perappear;

                                                                    perpass2 = Convert.ToDouble(hatpass[Convert.ToString(valcount1 + 7)]);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Text = Convert.ToString(perpass2);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].ForeColor = System.Drawing.Color.Indigo;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].BackColor = System.Drawing.Color.Thistle;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Bold = true;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Name = "Book Antiqua";
                                                                    totpass = totpass + perpass;

                                                                    double percentage1 = perpass2 / perappear2 * 100;
                                                                    percentage1 = Math.Round(percentage1, 2);

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].ForeColor = System.Drawing.Color.Indigo;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].BackColor = System.Drawing.Color.Thistle;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Bold = true;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Name = "Book Antiqua";
                                                                    jh2++;
                                                                }
                                                                else
                                                                {
                                                                    perappear2 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 7)]);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Text = Convert.ToString(perappear2);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].ForeColor = System.Drawing.Color.Indigo;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].BackColor = System.Drawing.Color.Thistle;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Bold = true;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Name = "Book Antiqua";
                                                                    totapp = totapp + perappear;

                                                                    perpass2 = Convert.ToDouble(hatpass[Convert.ToString(valcount1 + 8)]);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Text = Convert.ToString(perpass2);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].ForeColor = System.Drawing.Color.Indigo;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].BackColor = System.Drawing.Color.Thistle;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Bold = true;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Name = "Book Antiqua";
                                                                    totpass = totpass + perpass;

                                                                    double percentage1 = perpass2 / perappear2 * 100;
                                                                    percentage1 = Math.Round(percentage1, 2);

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].ForeColor = System.Drawing.Color.Indigo;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].BackColor = System.Drawing.Color.Thistle;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Bold = true;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Size = FontUnit.Medium;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Name = "Book Antiqua";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                perappear3 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 9)]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Text = Convert.ToString(perappear3);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Name = "Book Antiqua";
                                                                totapp = totapp + perappear;

                                                                perpass3 = Convert.ToDouble(hatpass[Convert.ToString(col + 10)]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].Text = Convert.ToString(perpass3);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 10].Font.Name = "Book Antiqua";
                                                                totpass = totpass + perpass;

                                                                double percentage1 = perpass3 / perappear3 * 100;
                                                                percentage1 = Math.Round(percentage1, 2);

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 11].Font.Name = "Book Antiqua";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (valcount1 <= 1)
                                            {
                                                valcount1--;

                                                if (valcount1 > 0)
                                                {
                                                    if (jh4 == 1)
                                                    {
                                                        perappear3 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 7)]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Text = Convert.ToString(perappear3);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Name = "Book Antiqua";
                                                        totapp = totapp + perappear;

                                                        perpass3 = Convert.ToDouble(hatpass[Convert.ToString(valcount1 + 8)]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Text = Convert.ToString(perpass3);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Name = "Book Antiqua";
                                                        totpass = totpass + perpass;

                                                        double percentage1 = perpass3 / perappear3 * 100;
                                                        percentage1 = Math.Round(percentage1, 2);

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Name = "Book Antiqua";
                                                        jh4++;
                                                    }
                                                    else
                                                    {
                                                        perappear2 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 7)]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Text = Convert.ToString(perappear2);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Name = "Book Antiqua";
                                                        totapp = totapp + perappear;

                                                        perpass2 = Convert.ToDouble(hatpass[Convert.ToString(valcount1 + 8)]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Text = Convert.ToString(perpass2);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Name = "Book Antiqua";
                                                        totpass = totpass + perpass;

                                                        double percentage1 = perpass2 / perappear2 * 100;
                                                        percentage1 = Math.Round(percentage1, 2);

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].ForeColor = System.Drawing.Color.Indigo;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].BackColor = System.Drawing.Color.Thistle;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Name = "Book Antiqua";
                                                    }
                                                }
                                                else
                                                {
                                                    if (valcount1 == 0)
                                                    {
                                                        if (jh5 == 1)
                                                        {
                                                            perappear2 = Convert.ToDouble(hatappear[Convert.ToString(valcount1 + 7)]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Text = Convert.ToString(perappear2);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].ForeColor = System.Drawing.Color.Indigo;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].BackColor = System.Drawing.Color.Thistle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 7].Font.Name = "Book Antiqua";
                                                            totapp = totapp + perappear;

                                                            perpass2 = Convert.ToDouble(hatpass[Convert.ToString(valcount1 + 8)]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Text = Convert.ToString(perpass2);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].ForeColor = System.Drawing.Color.Indigo;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].BackColor = System.Drawing.Color.Thistle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 8].Font.Name = "Book Antiqua";
                                                            totpass = totpass + perpass;

                                                            double percentage1 = perpass2 / perappear2 * 100;
                                                            percentage1 = Math.Round(percentage1, 2);

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].ForeColor = System.Drawing.Color.Indigo;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].BackColor = System.Drawing.Color.Thistle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, valcount1 + 9].Font.Name = "Book Antiqua";
                                                            jh5++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (valcount12 != 2)
                                                        {
                                                            if (valcount1 == -1 && row1 > valcount12 || valcount1 == -1 && row1 < valcount12)
                                                            {
                                                                int value = valcount1 + 2;
                                                                perappear3 = Convert.ToDouble(hatappear[Convert.ToString(value + 9)]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].Text = Convert.ToString(perappear3);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 9].Font.Name = "Book Antiqua";
                                                                totapp = totapp + perappear;

                                                                perpass3 = Convert.ToDouble(hatpass[Convert.ToString(value + 10)]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].Text = Convert.ToString(perpass3);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 10].Font.Name = "Book Antiqua";
                                                                totpass = totpass + perpass;

                                                                double percentage1 = perpass3 / perappear3 * 100;
                                                                percentage1 = Math.Round(percentage1, 2);

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].Text = Convert.ToString(Convert.ToDouble(percentage1));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].ForeColor = System.Drawing.Color.Indigo;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].BackColor = System.Drawing.Color.Thistle;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, value + 11].Font.Name = "Book Antiqua";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    if (rowcount != row1)
                                    {
                                        double perappear4 = Convert.ToDouble(htapprrow[Convert.ToString(row)]);
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 1].Text = perappear4.ToString();
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Name = "Book Antiqua";
                                        val = val + perappear4;

                                        double pass4 = Convert.ToDouble(htpassrow[Convert.ToString(row)]);
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 2].Text = pass4.ToString();
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Name = "Book Antiqua";
                                        val1 = val1 + pass4;

                                        double percentage2 = pass4 / perappear4 * 100;
                                        percentage2 = Math.Round(percentage2, 2);

                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Text = Convert.ToString(Convert.ToDouble(percentage2));
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].BackColor = System.Drawing.Color.Gainsboro;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].ForeColor = System.Drawing.Color.Brown;
                                    }
                                    else
                                    {
                                        // ----------------- Column-wise Total for End ------> Row-wise Total eg: 1803, 1422
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 1].Text = Convert.ToString(val);
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 1].ForeColor = System.Drawing.Color.Indigo;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 1].BackColor = System.Drawing.Color.Thistle;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 1].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 2].Text = Convert.ToString(val1);
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 2].ForeColor = System.Drawing.Color.Indigo;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 2].BackColor = System.Drawing.Color.Thistle;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 2].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 2].Font.Name = "Book Antiqua";

                                        double totpercen = val1 / val * 100;
                                        totpercen = Math.Round(totpercen, 2);

                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Text = Convert.ToString(Convert.ToDouble(totpercen));
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].ForeColor = System.Drawing.Color.Indigo;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].BackColor = System.Drawing.Color.Thistle;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[row, colcount1 + 3].Font.Name = "Book Antiqua";
                                    }
                                }
                            }
                        }
                    }
                    // ----------- Calculating Total Column-wise end
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();

                    if (reportfalg == true && fmnth > 1)
                    {
                        if (reportfalg == true && fmnth > 1)
                        {
                            FpSpread1.Visible = true;
                            Printcontrol.Visible = false;
                            lblexportxl.Visible = true;
                            txtexcell.Visible = true;
                            btnexcel.Visible = true;
                            btnprint.Visible = true;
                            lblerror.Visible = false;
                            Printcontrol.Visible = false;
                            lblerrormsg.Visible = false;
                        }
                    }
                    else if (flagmnth == true && fmnth == 1)
                    {
                        lblerrormsg.Text = "Please Select Month & Year";
                        lblerrormsg.Visible = true;
                        FpSpread1.Visible = false;
                        lblexportxl.Visible = false;
                        txtexcell.Visible = false;
                        btnexcel.Visible = false;
                        btnprint.Visible = false;
                        lblerror.Visible = false;
                        Printcontrol.Visible = false;
                    }
                    else
                    {
                        lblerrormsg.Text = "No Records Found";
                        lblerrormsg.Visible = true;
                        FpSpread1.Visible = false;
                        lblexportxl.Visible = false;
                        txtexcell.Visible = false;
                        btnexcel.Visible = false;
                        btnprint.Visible = false;
                        lblerror.Visible = false;
                        Printcontrol.Visible = false;
                    }
                }
                else
                {
                    lblerrormsg.Text = "Please Select Month & Year";
                    lblerrormsg.Visible = true;
                    FpSpread1.Visible = false;
                    lblexportxl.Visible = false;
                    txtexcell.Visible = false;
                    btnexcel.Visible = false;
                    btnprint.Visible = false;
                    lblerror.Visible = false;
                    Printcontrol.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btnexcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcell.Text;

            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(FpSpread1, reportname);
                lblerror.Visible = false;
            }
            else
            {
                lblerror.Text = "Please Enter Your Report Name";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btnprint_OnClick(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "";
            if (Rbtn.SelectedItem.Text == "External")
            {
                degreedetails = "PERFORMANCE ANALYSIS REPORT" + '@' + "                                                                    " + "Performance Analysis of External " + "Exam Month & Year: " + ddlmonth.SelectedItem.Text + " ---- " + yearhead + " YEAR";
            }
            else
            {
                degreedetails = "PERFORMANCE ANALYSIS REPORT" + '@' + "                                                                                " + "Performance Analysis of Internal " + ddltest.SelectedItem.Text + " ---- " + yearhead + " YEAR";
            }

            string pagename = "pareport.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
}