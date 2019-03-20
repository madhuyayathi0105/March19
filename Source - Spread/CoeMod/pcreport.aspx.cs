using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;
using wc = System.Web.UI.WebControls;
using System.Text;
using System.Configuration;

public partial class pcreport : System.Web.UI.Page
{
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";

    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable has = new Hashtable();

    int count = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            lblerrormsg.Visible = false;

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
                bindbatch();
                binddegree();
                binddept();
                bindyearmonth();
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
                checkBoxListselectOrDeselect(chcklistbatch, true);
                CallCheckboxListChange(chckbatch, chcklistbatch, txtbatch, lblbatch.Text, "--Select--");
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
            int count1 = ds1.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                chcklistdegree.DataSource = ds1;
                chcklistdegree.DataTextField = "course_name";
                chcklistdegree.DataValueField = "course_id";
                chcklistdegree.DataBind();
            }
            checkBoxListselectOrDeselect(chcklistdegree, true);
            CallCheckboxListChange(chckdegree, chcklistdegree, txtdegree, lbldegree.Text, "--Select--");
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
                checkBoxListselectOrDeselect(cbldept, true);
                CallCheckboxListChange(chckdept, cbldept, txtdept, lbldept.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void bindyearmonth()
    {
        try
        {
            int cont = 0;
            cblmonth.Items.Clear();

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

            //string qurymnth = "select distinct ltrim(str(Exam_year)+'-'+upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))) as monthName,Exam_year,Exam_Month from Exam_Details order by Exam_year desc,Exam_Month desc";

            //string qurymnth = "select distinct ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+' '+ ltrim(str(Exam_year))) as monthName,Exam_year,Exam_Month from Exam_Details order by Exam_year desc,Exam_Month desc";
            if (!string.IsNullOrEmpty(buildvalue) && !string.IsNullOrEmpty(buildvalue1))
            {
                string qurymnth = "select distinct ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,e.Exam_year,e.Exam_Month from Exam_Details e,Registration r where e.degree_code=r.Degree_Code and r.Batch_Year=e.batch_year and e.batch_year in ('" + buildvalue + "') and e.Degree_Code in ('" + buildvalue1 + "') order by e.Exam_year desc,e.Exam_Month desc";
                ds.Clear();
                ds = da.select_method_wo_parameter(qurymnth, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblmonth.DataSource = ds;
                    cblmonth.DataTextField = "monthName";
                    cblmonth.DataValueField = "monthName";
                    cblmonth.DataBind();

                    string max_bat = "";
                    max_bat = (ds.Tables[0].Rows[0][0].ToString());
                    cblmonth.SelectedValue = max_bat.ToString();
                }

                if (cblmonth.Items.Count > 0)
                {
                    for (int h = 0; h < cblmonth.Items.Count; h++)
                    {
                        cont++;
                        txtmonth.Text = "Month & Year " + "(" + cont + ")";
                        cblmonth.Items[h].Selected = true;
                        checkmonth.Checked = true;
                    }
                }
                else
                {
                    txtmonth.Text = "--Select--";
                    checkmonth.Checked = false;
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
           
            CallCheckboxChange(chckbatch, chcklistbatch, txtbatch, lblbatch.Text, "--Select--");
            binddegree();
            binddept();
            bindyearmonth();
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
            CallCheckboxListChange(chckbatch, chcklistbatch, txtbatch, lblbatch.Text, "--Select--");
            binddegree();
            binddept();
            bindyearmonth();
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
            CallCheckboxChange(chckdegree, chcklistdegree, txtdegree, lbldegree.Text, "--Select--");
            binddept();
            bindyearmonth();
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
            CallCheckboxListChange(chckdegree, chcklistdegree, txtdegree, lbldegree.Text, "--Select--");
            binddept();
            bindyearmonth();
          
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
            
            CallCheckboxChange(chckdept, cbldept, txtdept, lbldept.Text, "--Select--");
            bindyearmonth();
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
            CallCheckboxListChange(chckdept, cbldept, txtdept, lbldept.Text, "--Select--");
            bindyearmonth();
           
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void checkmonth_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";

            if (checkmonth.Checked == true)
            {
                for (int i = 0; i < cblmonth.Items.Count; i++)
                {
                    if (checkmonth.Checked == true)
                    {
                        cblmonth.Items[i].Selected = true;
                        txtmonth.Text = "Month & Year (" + (cblmonth.Items.Count) + ")";
                        build1 = cblmonth.Items[i].Value.ToString();
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
            }
            else
            {
                for (int i = 0; i < cblmonth.Items.Count; i++)
                {
                    cblmonth.Items[i].Selected = false;
                    txtmonth.Text = "--Select--";
                    cblmonth.ClearSelection();
                    checkmonth.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void cblmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int seatcount = 0;

            checkmonth.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cblmonth.Items.Count; i++)
            {
                if (cblmonth.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txtmonth.Text = "Select All";
                    build = cblmonth.Items[i].Value.ToString();
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

            if (seatcount == cblmonth.Items.Count)
            {
                txtmonth.Text = "Month & Year (" + seatcount.ToString() + ")";
                checkmonth.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtmonth.Text = "--Select--";
                checkmonth.Text = "Select All";
            }
            else
            {
                txtmonth.Text = "Month & Year (" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
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

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Batch Year";

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

            // 1st query
            //string qurymnth = "select distinct ltrim(str(Exam_year)+'-'+upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))) as monthName,Exam_year,Exam_Month from Exam_Details order by Exam_year desc,Exam_Month desc";

            // 2nd query 
            //string qurymnth = "select distinct ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,Exam_year,Exam_Month from Exam_Details order by Exam_year desc,Exam_Month desc";

            string qurymnth = "select distinct ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,e.Exam_year,e.Exam_Month from Exam_Details e,Degree d where e.degree_code=d.Degree_Code and e.batch_year in ('" + buildvalue + "') and d.Degree_Code in ('" + buildvalue1 + "') order by e.Exam_year desc,e.Exam_Month desc";
            ds.Clear();
            ds = da.select_method_wo_parameter(qurymnth, "text");

            string head = "", head1 = "", sk5 = "", sk4 = "", sk2 = "", sk3 = "", buildvalue2 = "";
            int cnt = 0;

            // ---------------------------- Month and Year
            for (int i = 0; i < cblmonth.Items.Count; i++)
            {
                if (cblmonth.Items[i].Selected == true)
                {
                    string build2 = cblmonth.Items[i].Value.ToString();
                    if (buildvalue2 == "")
                    {
                        buildvalue2 = build2;
                    }
                    else
                    {
                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                    }

                    string state_value = ds.Tables[0].Rows[i]["Exam_month"].ToString();

                    if (state_value != "11")
                    {
                        if (state_value != "12")
                        {
                            string[] split_value = state_value.Split('-');
                            sk3 = state_value[0].ToString();
                        }
                        else
                        {
                            sk3 = state_value;
                        }
                    }
                    else
                    {
                        sk3 = state_value;
                    }

                    if (sk2 == "")
                    {
                        sk2 = sk3;
                    }
                    else
                    {
                        //if (sk2.Contains(sk3) != sk3.Contains(sk3))
                        //{
                        sk2 = sk2 + "'" + "," + "'" + sk3;
                        //}
                    }

                    string state_value1 = ds.Tables[0].Rows[i]["Exam_year"].ToString();
                    string[] split_value1 = state_value1.Split('-');
                    sk4 = split_value1[0].ToString();
                    if (sk5 == "")
                    {
                        sk5 = sk4;
                    }
                    else
                    {
                        //if (sk5.Contains(sk4) != sk4.Contains(sk4))
                        //{
                        sk5 = sk5 + "'" + "," + "'" + sk4;
                        //}
                    }
                }
            }

            int km = 1, kn = 1, lm = 1, st = 0;
            Hashtable htc = new Hashtable();
            DataView dv1 = new DataView();
            string mnthyr = "";
            // -------------------------- query1 for Bind Heading eg: ECE, EEE and 2015-MAY, 2014-JUNE start
            //string query1 = "select distinct ltrim(str(Exam_year)+'-'+upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))) as monthName, e.Exam_year,e.Exam_Month,d.Degree_Code,c.Course_Name,de.dept_acronym from Exam_Details e,mark_entry m,Degree d,Course c,Department de where m.exam_code=e.exam_code and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and e.batch_year in ('" + buildvalue + "') and e.degree_code in ('" + buildvalue1 + "') and e.Exam_year in ('" + sk5 + "') and e.Exam_Month in ('" + sk2 + "') order by d.Degree_Code,e.Exam_year desc,e.Exam_Month desc";

            string query1 = "select distinct ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName, e.Exam_year,e.Exam_Month,d.Degree_Code,c.Course_Name,de.dept_acronym from Exam_Details e,mark_entry m,Degree d,Course c,Department de where m.exam_code=e.exam_code and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and e.batch_year in ('" + buildvalue + "') and e.degree_code in ('" + buildvalue1 + "') and e.Exam_year in ('" + sk5 + "') and e.Exam_Month in ('" + sk2 + "') order by d.Degree_Code,e.Exam_year desc,e.Exam_Month desc";//Rajkumar 28-5-2018
            DataSet ds1 = ds1 = da.select_method_wo_parameter(query1, "text");

            if (ds1.Tables[0].Rows.Count > 0)
            {
                reportfalg = true;
                //FpSpread1.Width = 900;
                FpSpread1.Sheets[0].AutoPostBack = true;

                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    head = ds1.Tables[0].Rows[i]["dept_acronym"].ToString();
                    head1 = ds1.Tables[0].Rows[i]["Degree_Code"].ToString();

                    ds1.Tables[0].DefaultView.RowFilter = "Degree_Code ='" + head1 + "'";

                    if (!ht.ContainsKey(ds1.Tables[0].Rows[i]["Degree_Code"].ToString()))
                    {
                        ht.Add(ds1.Tables[0].Rows[i]["Degree_Code"].ToString(), cnt);
                        cnt++;
                        dv1 = ds1.Tables[0].DefaultView;

                        if (dv1.Count > 0)
                        {
                            for (int dvyr = 0; dvyr < dv1.Count; dvyr++)
                            {
                                // ---------------------------  Heading for Bind Dept Name eg: ECE, EEE start
                                if (kn == 1)
                                {
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = head;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = head1;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = System.Drawing.Color.White;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.Color.Teal;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    kn++;
                                    htc.Add(dv1[dvyr]["Degree_Code"].ToString(), cnt);
                                }
                                else
                                {
                                    if (!htc.ContainsKey(dv1[dvyr]["Degree_Code"].ToString()))
                                    {
                                        htc.Add(dv1[dvyr]["Degree_Code"].ToString(), cnt);
                                        cnt++;
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = head;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = head1;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = System.Drawing.Color.White;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.Color.Teal;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = head;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = head1;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = System.Drawing.Color.White;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.Color.Teal;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        lm++;
                                    }
                                }
                                // ---------------------------  Heading for Bind Dept Name eg: ECE, EEE end

                                mnthyr = dv1[dvyr]["monthName"].ToString();

                                // ---------------------------  Heading for Bind Exam Month & Year Name eg: 2015-MAY, 2014-JUNE start
                                if (km == 1)
                                {
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Text = mnthyr;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Tag = mnthyr;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].ForeColor = System.Drawing.Color.White;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].BackColor = System.Drawing.Color.Teal;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Font.Name = "Book Antiqua";
                                    km++;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = mnthyr;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = mnthyr;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = System.Drawing.Color.White;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.Color.Teal;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                }
                                // ---------------------------  Heading for Bind Exam Month & Year Name eg: 2015-MAY, 2014-JUNE end
                                st = dvyr;
                            }

                            // --------------------------- ColumnHeader Spanning start
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - lm, 1, lm);
                            // --------------------------- ColumnHeader Spanning end
                            lm = 1;
                        }
                    }
                }
                FpSpread1.Sheets[0].SetRowMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }

            // ------------------------- query2 for Calculating Overall Percentage start
            //string query2 = "select count(distinct m.roll_no) as Appear,dept_acronym,ltrim(str(Exam_year)+'-'+upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))) as monthName,r.degree_code,Exam_Month,Exam_year,r.Batch_Year,c.Course_Name,de.Dept_Name from Exam_Details ed,mark_entry m,Registration r,Degree d,Department de,Course c where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.degree_code=d.Degree_Code and d.Degree_Code=ed.degree_code and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code in ('" + buildvalue1 + "') and r.Batch_Year in ('" + buildvalue + "') group by r.degree_code,Exam_Month,Exam_year,r.Batch_Year,dept_acronym,c.Course_Name,de.Dept_Name,r.degree_code,r.Batch_Year order by r.Batch_Year desc,r.degree_code select count(distinct m.roll_no) as pass,dept_acronym,ltrim(str(Exam_year)+'-'+upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))) as monthName,r.degree_code,Exam_Month,Exam_year,r.Batch_Year,c.Course_Name,de.Dept_Name from Exam_Details ed,mark_entry m,Registration r,Degree d,Department de,Course c where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.degree_code=d.Degree_Code and d.Degree_Code=ed.degree_code and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code in ('" + buildvalue1 + "') and m.roll_no not in(select m1.roll_no from mark_entry m1,Exam_Details ed1 where m1.exam_code=ed1.exam_code and ed1.Exam_year in ('" + sk5 + "') and ed1.Exam_Month in ('" + sk2 + "') and m1.result<>'pass' and ed1.degree_code in ('" + buildvalue1 + "')) and r.Batch_Year in ('" + buildvalue + "') group by r.degree_code,Exam_Month,Exam_year,dept_acronym,r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,r.Batch_Year order by r.Batch_Year desc,r.degree_code";

            Hashtable hattotal = new Hashtable();

            // ----------------------- For Batch Year eg: 2014
            for (int i = 0; i < chcklistbatch.Items.Count; i++)
            {
                if (chcklistbatch.Items[i].Selected == true)
                {
                    string year = "";
                    string build = chcklistbatch.Items[i].Value.ToString();
                    int startrow = FpSpread1.Sheets[0].RowCount;
                    // ---------------------- For Degree Code eg: 45
                    for (int kk = 0; kk < cbldept.Items.Count; kk++)
                    {
                        if (cbldept.Items[kk].Selected == true)
                        {
                            string build1 = cbldept.Items[kk].Value.ToString();

                            // ------------------- For Month and Year eg: May-2014
                            for (int ip = 0; ip < cblmonth.Items.Count; ip++)
                            {
                                if (cblmonth.Items[ip].Selected == true)
                                {
                                    string build2 = cblmonth.Items[ip].Value.ToString();

                                    string state_value = ds.Tables[0].Rows[ip]["Exam_month"].ToString();
                                    //string[] split_value = state_value.Split('-');
                                    sk2 = state_value;

                                    string state_value1 = ds.Tables[0].Rows[ip]["Exam_year"].ToString();
                                    //string[] split_value1 = state_value1.Split('-');
                                    sk5 = state_value1;

                                    // ----- old query 1
                                    //string query2 = "select count(distinct m.roll_no) as Appear,dept_acronym,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,Exam_Month,Exam_year,r.Batch_Year,c.Course_Name,de.Dept_Name from Exam_Details ed,mark_entry m,Registration r,Degree d,Department de,Course c where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.degree_code=d.Degree_Code and d.Degree_Code=ed.degree_code and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' group by r.degree_code,Exam_Month,Exam_year,r.Batch_Year,dept_acronym,c.Course_Name,de.Dept_Name,r.degree_code,r.Batch_Year order by r.Batch_Year desc,r.degree_code select count(distinct m.roll_no) as pass,dept_acronym,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,Exam_Month,Exam_year,r.Batch_Year,c.Course_Name,de.Dept_Name from Exam_Details ed,mark_entry m,Registration r,Degree d,Department de,Course c,syllabus_master sy,sub_sem ss,subject s where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.degree_code=d.Degree_Code and d.Degree_Code=ed.degree_code and r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and d.Dept_Code=de.Dept_Code and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and ss.syll_code=s.syll_code and ss.promote_count=1 and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and r.Current_Semester=sy.semester and m.subject_no=s.subject_no and d.Course_Id=c.Course_Id and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and m.roll_no not in (select m1.roll_no from mark_entry m1 where m1.exam_code=ed.exam_code and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and m1.result<>'pass' and ed.degree_code = '" + build1 + "' and m1.subject_no=s.subject_no) and r.Batch_Year = '" + build + "' group by r.degree_code,Exam_Month,Exam_year,dept_acronym,r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,r.Batch_Year order by r.Batch_Year desc,r.degree_code";

                                    // ----- old query 2
                                    //string query2 = "select count(distinct m.roll_no) as strength,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,Exam_Month,Exam_year,r.Batch_Year from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' group by r.degree_code,Exam_Month,Exam_year,r.Batch_Year,r.degree_code,r.Batch_Year order by r.Batch_Year desc,r.degree_code select count(distinct m.roll_no) as pass,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,Exam_Month,Exam_year,r.Batch_Year from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' and m.roll_no not in(select m1.roll_no from mark_entry m1,Exam_Details ed1,subject s1,syllabus_master sy1 where m1.exam_code=ed1.exam_code and sy1.degree_code=ed1.degree_code and sy1.Batch_Year=ed1.batch_year and ed1.current_semester=sy1.semester and sy1.syll_code=s1.syll_code and m1.subject_no=s1.subject_no and ed1.Exam_year in ('" + sk5 + "') and ed1.Exam_Month in ('" + sk2 + "') and ed1.degree_code = '" + build1 + "' and ed1.Batch_Year = '" + build + "' and m1.result<>'Pass') group by r.degree_code,Exam_Month,Exam_year,r.Batch_Year ";

                                    // ----- old query 3 right answr
                                    //string query2 = "select count(distinct m.roll_no) as strength,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,Exam_Month,Exam_year,r.Batch_Year from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' group by r.degree_code,Exam_Month,Exam_year,r.Batch_Year,r.degree_code,r.Batch_Year order by r.Batch_Year desc,r.degree_code select count(distinct m.roll_no) as pass,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' and m.roll_no not in(select m1.roll_no from mark_entry m1,subject s1,syllabus_master sy1 where m1.exam_code=ed.exam_code and sy1.syll_code=s1.syll_code and sy1.Batch_Year=ed.batch_year and ed.degree_code=sy1.degree_code and sy1.semester=ed.current_semester and m1.subject_no=s1.subject_no and m1.result<>'Pass') group by r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year";

                                    // ----- old query 4 right answr modified use it
                                    //string query2 = "select count(distinct m.roll_no) as strength,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,Exam_Month,Exam_year,r.Batch_Year from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' group by r.degree_code,Exam_Month,Exam_year,r.Batch_Year,r.degree_code,r.Batch_Year order by r.Batch_Year desc,r.degree_code select count(distinct m.roll_no) as pass,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' and m.roll_no not in(select m1.roll_no from mark_entry m1,subject s1,syllabus_master sy1 where m1.exam_code=ed.exam_code and sy1.syll_code=s1.syll_code and sy1.Batch_Year=ed.batch_year and ed.degree_code=sy1.degree_code and sy1.semester=ed.current_semester and sy1.Batch_Year=r.batch_year and r.degree_code=sy1.degree_code and m1.subject_no=s1.subject_no and sy.degree_code = '" + build1 + "' and sy.Batch_Year = '" + build + "' and m1.result<>'Pass') group by r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year";

                                    // ----- old query 5 right query compressed query 4
                                    string query2 = "select count(distinct m.roll_no) as strength,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,Exam_Month,Exam_year,r.Batch_Year from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' group by r.degree_code,Exam_Month,Exam_year,r.Batch_Year,r.degree_code,r.Batch_Year order by r.Batch_Year desc,r.degree_code select count(distinct m.roll_no) as pass,ltrim(upper(convert(varchar(3),DateAdd(month,Exam_month,-1)))+'-'+ ltrim(str(Exam_year))) as monthName,r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year from Exam_Details ed,mark_entry m,Registration r,syllabus_master sy,subject s where ed.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and sy.Batch_Year=r.Batch_Year and sy.degree_code=r.degree_code and sy.Batch_Year=ed.Batch_Year and sy.degree_code=ed.degree_code and sy.semester=ed.Current_Semester and sy.syll_code=s.syll_code and s.subject_no=m.subject_no and ed.Exam_year in ('" + sk5 + "') and ed.Exam_Month in ('" + sk2 + "') and r.degree_code = '" + build1 + "' and r.Batch_Year = '" + build + "' and m.roll_no not in(select m1.roll_no from mark_entry m1,subject s1,syllabus_master sy1 ,Registration r1,Exam_Details ed1 where m1.exam_code=ed1.exam_code and sy1.syll_code=s1.syll_code and sy1.Batch_Year=ed1.batch_year and m1.roll_no = r1.Roll_No and ed1.degree_code=sy1.degree_code and r1.Batch_Year=sy1.Batch_Year and sy1.semester=ed1.current_semester and r1.degree_code=sy1.degree_code and m1.subject_no=s1.subject_no and sy1.degree_code = '" + build1 + "' and sy1.Batch_Year = '" + build + "' and m1.result<>'Pass' and ed1.Exam_year in ('" + sk5 + "') and ed1.Exam_Month in ('" + sk2 + "')) group by r.degree_code,ed.exam_code,Exam_Month,Exam_year,r.Batch_Year";
                                    DataSet ds12 = da.select_method_wo_parameter(query2, "text");
                                    Hashtable ht2 = new Hashtable();
                                    Hashtable htblr = new Hashtable();
                                    Hashtable ht1 = new Hashtable();
                                    double pass = 0.0;
                                    DataView dv3 = new DataView();
                                    DataView dv2 = new DataView();

                                    if (ds12.Tables[0].Rows.Count > 0)
                                    {
                                        reportfalg = true;
                                        for (int tr = 0; tr < ds12.Tables[0].Rows.Count; tr++)
                                        {
                                            if (!ht2.ContainsKey(ds12.Tables[0].Rows[tr]["degree_code"].ToString() + "-" + ds12.Tables[0].Rows[tr]["Batch_Year"].ToString() + "-" + ds12.Tables[0].Rows[tr]["monthName"].ToString()))
                                            {
                                                ht2.Add(ds12.Tables[0].Rows[tr]["degree_code"].ToString() + "-" + ds12.Tables[0].Rows[tr]["Batch_Year"].ToString() + "-" + ds12.Tables[0].Rows[tr]["monthName"].ToString(), cnt);
                                                cnt++;

                                                // ------------- filter Batch Year eg: 2011, 2011
                                                string batyr = ds12.Tables[0].Rows[tr]["Batch_Year"].ToString();
                                                string monthnam1 = ds12.Tables[0].Rows[tr]["monthName"].ToString();
                                                ds12.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batyr + "'and monthName='" + monthnam1 + "' ";
                                                dv2 = ds12.Tables[0].DefaultView;

                                                if (dv2.Count > 0)
                                                {
                                                    //if (ds12.Tables[1].Rows.Count > 0 && tr < ds12.Tables[1].Rows.Count)
                                                    //{
                                                    string dept1 = dv2[0]["Batch_Year"].ToString();
                                                    string monthnam = dv2[0]["monthName"].ToString();
                                                    //ds12.Tables[1].DefaultView.RowFilter = "Batch_Year='" + batyr + "' ";
                                                    ds12.Tables[1].DefaultView.RowFilter = "Batch_Year='" + batyr + "' and monthName='" + monthnam + "' ";
                                                    dv3 = ds12.Tables[1].DefaultView;
                                                    //}

                                                    int startvalue = 1;

                                                    for (int kg = 0; kg < dv2.Count; kg++)
                                                    {
                                                        double appeared = Convert.ToDouble(dv2[kg]["strength"]);

                                                        //if (ds12.Tables[1].Rows.Count > 0 && tr < ds12.Tables[1].Rows.Count)
                                                        //{
                                                        if (dv3.Count > 0 && kg < dv3.Count)
                                                        {

                                                            pass = Convert.ToDouble(dv3[kg]["pass"]);
                                                        }
                                                        else
                                                        {
                                                            pass = 0.0;
                                                        }
                                                        //}
                                                        //else
                                                        //{
                                                        //    pass = 0.0;
                                                        //}

                                                        double percentage = pass / appeared * 100;
                                                        percentage = Math.Round(percentage, 2);

                                                        string getyr = dv2[kg]["monthName"].ToString();
                                                        string getyrhd = dv2[kg]["degree_code"].ToString();

                                                        // -------------------- Bind Batch Year eg: 2013, 2012, 2011 start
                                                        if (year != batyr)
                                                        {
                                                            if (!htblr.ContainsKey(ds12.Tables[0].Rows[tr]["Batch_Year"].ToString()))
                                                            {
                                                                htblr.Add(ds12.Tables[0].Rows[tr]["Batch_Year"].ToString(), cnt);
                                                                cnt++;
                                                                FpSpread1.Sheets[0].RowCount++;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = batyr;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                                                year = batyr;
                                                            }
                                                        }
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = batyr;
                                                        // -------------------- Bind Batch Year eg: 2013, 2012, 2011 end

                                                        for (int c = startvalue; c < FpSpread1.Sheets[0].ColumnCount; c++)
                                                        {
                                                            string gethead = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, c].Tag);
                                                            string getyrmnth = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Tag);
                                                            if (getyr.Contains(getyrmnth) && getyrhd.Contains(gethead))
                                                            {
                                                                for (int r = startrow; r <= FpSpread1.Sheets[0].RowCount; r++)
                                                                {
                                                                    if (!htblr.ContainsKey(dv2[kg]["Batch_Year"].ToString() + "-" + dv2[kg]["monthName"].ToString() + "-" + dv2[kg]["degree_code"].ToString()))
                                                                    {
                                                                        htblr.Add(dv2[kg]["Batch_Year"].ToString() + "-" + dv2[kg]["monthName"].ToString() + "-" + dv2[kg]["degree_code"].ToString(), cnt);
                                                                        cnt++;
                                                                        FpSpread1.Sheets[0].Cells[r, c].Text = percentage.ToString();
                                                                        FpSpread1.Sheets[0].Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                                                                        if (!hattotal.Contains(Convert.ToString(c)))
                                                                        {
                                                                            hattotal.Add(Convert.ToString(c), percentage);
                                                                        }
                                                                        else
                                                                        {
                                                                            string prev = Convert.ToString(hattotal[Convert.ToString(c)]);
                                                                            if (prev.Trim() != "")
                                                                            {
                                                                                double totalp = Convert.ToDouble(prev) + percentage;
                                                                                hattotal.Remove(Convert.ToString(c));
                                                                                hattotal.Add(Convert.ToString(c), totalp);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "OVERALL-%";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = System.Drawing.Color.Indigo;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.Thistle;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].Columns[0].Width = 100;
            //FpSpread1.Width = 1000;

            if (FpSpread1.Sheets[0].ColumnCount > 0)
            {
                for (int col = 1; col < FpSpread1.Sheets[0].ColumnCount; col++)
                {
                    int count = 0;
                    for (int row = 0; row < FpSpread1.Sheets[0].RowCount; row++)
                    {
                        string vlaue = Convert.ToString(FpSpread1.Sheets[0].Cells[row, col].Text);
                        if (vlaue.Trim() != "")
                        {
                            count++;
                        }
                    }
                    double per = Convert.ToDouble(hattotal[Convert.ToString(col)]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(Convert.ToDouble(Math.Round(Convert.ToDouble(per) / Convert.ToDouble(count), 2)));
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].ForeColor = System.Drawing.Color.Indigo;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = System.Drawing.Color.Thistle;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                }
            }

            if (reportfalg == true)
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

            MyClass ms = new MyClass();
            ms.Dispose();
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public class MyClass : IDisposable
    {
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // called via myClass.Dispose(). 
                    // OK to use any private object references
                }

                disposed = true;
            }
            disposed = true;
        }

        public void Dispose() // Implement IDisposable
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MyClass() // the finalizer
        {
            Dispose(false);
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
            //string degreedetails = "PERFORMANCE COMPARISON REPORT" + '@' + "                                                                                                  " + "BATCHWISE PERFORMANCE COMPARISON" + '@';

            string degreedetails = "BATCHWISE PERFORMANCE COMPARISON REPORT" + '@';
            string pagename = "pcreport.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        StringBuilder selectedvalue = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        StringBuilder selectedText = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedText.ToString();
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
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
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
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

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    private bool getSelectedCheckBoxListCount(CheckBoxList cbl, out int selectedCount)
    {
        selectedCount = 0;
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                if (li.Selected)
                {
                    selectedCount++;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="c">Only Data Bound Controls eg.DropDownList,RadioButtonList,CheckBoxList </param>
    /// <param name="selectedValue"></param>
    /// <param name="selectedText"></param>
    /// <param name="type">0 - Index; 1 - Text; 2 - Value;</param>
    private void SelectDataBound(Control c, string selectedValue, string selectedText)
    {
        try
        {
            bool isDataBoundControl = false;
            if (c is DataBoundControl)
            {
                if (c is CheckBoxList || c is DropDownList || c is RadioButtonList)
                {
                    isDataBoundControl = true;
                }
                if (isDataBoundControl)
                {
                    ListControl lstControls = (ListControl)c;
                    if (lstControls.Items.Count > 0)
                    {
                        ListItem[] listItem = new ListItem[lstControls.Items.Count];
                        lstControls.Items.CopyTo(listItem, 0);
                        if (listItem.Contains(new ListItem(selectedText, selectedValue)))
                        {
                            lstControls.SelectedValue = selectedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion
}