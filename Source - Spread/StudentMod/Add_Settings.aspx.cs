using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using FarPoint.Web.Spread;
using System.Drawing;
using System.Globalization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


public partial class Add_Settings : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 dt = new DAccess2();
    Hashtable hat = new Hashtable();
    string college_code = "";
    string user_code = "";
    string college = "";
    string singleuser = "";
    static int spvl = 0;
    static int val = 0;
    static int val1 = 0;
    static Hashtable hashcheck = new Hashtable();
    string group_user = "";
    FarPoint.Web.Spread.CheckBoxCellType chkcel1 = new FarPoint.Web.Spread.CheckBoxCellType();
    static ArrayList columsvales = new ArrayList();
    static ArrayList rowdegvalue = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }
            //****************************************************//
            user_code = Session["usercode"].ToString();
            college_code = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                Maintable.Visible = false;
                bindcollege();
                maindiv.Visible = true;
                txt_Religion.Enabled = true;
                txt_Community.Enabled = true;
                txt_Branch.Enabled = true;
                txt_Degree.Enabled = true;
                batch();
                rdbancillary.Enabled = false;
                rdbCompulsory.Enabled = false;
                type();
                edu();
                degree();
                bindbranch(college);
                TabContainer1.ActiveTabIndex = 2;
                spvl = 4;
                columsvales.Clear();
                rowdegvalue.Clear();
                bindreglion();
                bindcommunity();
                val = 0;
                spread1();
                val1 = 0;
                TabContainer1_ActiveTabChanged(sender, e);
                orderbypercentage();
                bindtype();
                retrieveIncMarks();
                // mpemsgboxdelete.Show();
            }
            // step6.Visible = true;
        }
        catch (Exception ex) { }
    }

    public void orderbypercentage()
    {
        string[] orderby = { "Total percentage of marks in all subjects", "Total % of Marks in Major subjects alone", "Total percentage marks in major/Allied" };
        string[] orderby1 = { "percentage", "major_percent", "majorallied_percent" };
        for (int i = 0; i < 3; i++)
        {

            ddl_percentagesetting.Items.Add(new System.Web.UI.WebControls.ListItem(orderby[i], orderby1[i]));

        }
    }

    public void batch()
    {
        try
        {

            ds = dt.select_method_wo_parameter(" select MAX(Batch_Year) as batch from applyn ", "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                txtbatch.Text = ds.Tables[0].Rows[0]["batch"].ToString();
                //txtbatch.Text = DateTime.Now.ToString("yyyy");
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void type()
    {
        try
        {
            ddltype.Items.Clear();
            ds = dt.select_method_wo_parameter("select distinct type from degree,course,deptprivilages where     course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + user_code + "' and type is not null and type<>''", "Text");

            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataValueField = "type";
                ddltype.DataBind();
                //ddltype.Items.Insert(0, "All");
            }
            else
            {
                ddltype.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void edu()
    {
        try
        {
            ddledu.Items.Clear();
            ds = dt.select_method_wo_parameter("select distinct Edu_Level from degree,course,deptprivilages where     course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + user_code + "' and course.type='" + ddltype.SelectedItem.Text + "'  order by Edu_Level desc", "Text");

            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddledu.DataSource = ds;
                ddledu.DataTextField = "Edu_Level";
                ddledu.DataValueField = "Edu_Level";
                ddledu.DataBind();

            }
            else
            {
                ddledu.Items.Insert(0, "--Select--");
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

            cheklist_Degree.Items.Clear();
            //if (ddltype.Text != "0")
            //{
            if (ddltype.SelectedItem.Text != "All")
            {
                ds = dt.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where     course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and course.type='" + ddltype.SelectedItem.Text + "' and user_code='" + user_code + "' and Edu_Level ='" + ddledu.SelectedItem.Text + "'", "Text");
            }
            else
            {
                ds = dt.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where     course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + user_code + "' and Edu_Level ='" + ddledu.SelectedItem.Text + "'", "Text");
            }
            //}
            //else
            //{
            //    ds = dt.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where     course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + college_code + "' and deptprivilages.degree_code=degree.degree_code and user_code='" + user_code + "' and course.type='" + ddltype.Text + "'", "text");
            //}
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                cheklist_Degree.DataSource = ds;
                cheklist_Degree.DataTextField = "course_name";
                cheklist_Degree.DataValueField = "course_id";
                cheklist_Degree.DataBind();

            }
            if (cheklist_Degree.Items.Count > 0)
            {
                int count11 = 0;
                checkDegree.Checked = true;
                for (int j = 0; j < cheklist_Degree.Items.Count; j++)
                {
                    count11++;
                    cheklist_Degree.Items[j].Selected = true;
                }
                txt_Degree.Text = "Degree(" + count11 + ")";
            }
            else
            {
                cheklist_Degree.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void checkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";

            if (checkDegree.Checked == true)
            {
                for (int i = 0; i < cheklist_Degree.Items.Count; i++)
                {

                    if (checkDegree.Checked == true)
                    {
                        cheklist_Degree.Items[i].Selected = true;
                        txt_Degree.Text = "Degree(" + (cheklist_Degree.Items.Count) + ")";
                        build1 = cheklist_Degree.Items[i].Value.ToString();
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
                bindbranch(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cheklist_Degree.Items.Count; i++)
                {
                    cheklist_Degree.Items[i].Selected = false;
                    txt_Degree.Text = "--Select--";
                    txt_Branch.Text = "--Select--";
                    cheklist_Branch.ClearSelection();
                    checkBranch.Checked = false;
                }
            }
            // Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void checkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (checkBranch.Checked == true)
            {
                for (int i = 0; i < cheklist_Branch.Items.Count; i++)
                {

                    cheklist_Branch.Items[i].Selected = true;
                    txt_Branch.Text = "Branch(" + (cheklist_Branch.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cheklist_Branch.Items.Count; i++)
                {
                    cheklist_Branch.Items[i].Selected = false;
                    txt_Branch.Text = "--Select--";
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void cheklist_Branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            checkBranch.Checked = false;
            for (int i = 0; i < cheklist_Branch.Items.Count; i++)
            {
                if (cheklist_Branch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            if (seatcount == cheklist_Branch.Items.Count)
            {
                txt_Branch.Text = "Branch(" + seatcount.ToString() + ")";
                checkBranch.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_Degree.Text = "--Select--";
            }
            else
            {
                txt_Branch.Text = "Branch(" + seatcount.ToString() + ")";
            }
            // Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    protected void cheklist_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;

            checkDegree.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cheklist_Degree.Items.Count; i++)
            {
                if (cheklist_Degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_Branch.Text = "--Select--";
                    build = cheklist_Degree.Items[i].Value.ToString();
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
            bindbranch(buildvalue);
            if (seatcount == cheklist_Degree.Items.Count)
            {
                txt_Degree.Text = "Degree(" + seatcount.ToString() + ")";
                checkDegree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_Degree.Text = "--Select--";
                txt_Branch.Text = "--Select--";
            }
            else
            {
                txt_Degree.Text = "Degree(" + seatcount.ToString() + ")";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch(string branch)
    {
        try
        {
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                if (cheklist_Degree.Items.Count > 0)
                {
                    for (int i = 0; i < cheklist_Degree.Items.Count; i++)
                    {
                        if (cheklist_Degree.Items[i].Selected == true)
                        {
                            if (branch == "")
                            {
                                branch = cheklist_Degree.Items[i].Value;
                            }
                            else
                            {
                                branch = branch + "'" + "," + "'" + cheklist_Degree.Items[i].Value;
                            }

                        }
                    }
                }
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code";
            }
            {
                ds = dt.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cheklist_Branch.DataSource = ds;
                    cheklist_Branch.DataTextField = "dept_name";
                    cheklist_Branch.DataValueField = "degree_code";
                    cheklist_Branch.DataBind();
                }
                if (cheklist_Branch.Items.Count > 0)
                {
                    int count11 = 0;
                    checkBranch.Checked = true;
                    for (int j = 0; j < cheklist_Branch.Items.Count; j++)
                    {
                        count11++;
                        cheklist_Branch.Items[j].Selected = true;
                    }
                    txt_Branch.Text = "Branch(" + count11 + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddltype_select(object sender, EventArgs e)
    {
        edu();
        degree();
        bindbranch(college);
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
    }

    protected void ddldegreeselected(object sender, EventArgs e)
    {
        bindbranch(college);
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
    }

    protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
    {
        spvl = 0;
        val1 = 0;
        if (TabContainer1.ActiveTabIndex == 0)
        {
            Maintable.Visible = true;
            spvl = 4;
            txt_Religion.Enabled = true;
            txt_Community.Enabled = true;
            txt_Branch.Enabled = true;
            txt_Degree.Enabled = true;
            rdbancillary.Enabled = false;
            rdbCompulsory.Enabled = false;
            upd1.Visible = true;
            //orderbysetting.Visible = false;
        }
        if (TabContainer1.ActiveTabIndex == 1)
        {
            Maintable.Visible = true;
            txt_Branch.Enabled = true;
            txt_Degree.Enabled = true;
            txt_Religion.Enabled = false;
            txt_Community.Enabled = false;
            rdbancillary.Enabled = true;
            rdbCompulsory.Enabled = true;
            // orderbysetting.Visible = false;
            spvl = 2;
        }
        if (TabContainer1.ActiveTabIndex == 2)
        {
            Maintable.Visible = false;
            spvl = 3;
            column();
            selectcolumn();
            selectcolumnfunction();
            panel7.Visible = true;
            rdbancillary.Enabled = false;
            rdbCompulsory.Enabled = false;

            string type = dt.GetFunction(" select setcolumn from admitcolumnset where textcriteria='Section process Format' and user_code='" + user_code + "' and college_code='" + college_code + "'");

            if (type == "0")
            {
                rdb_formate1.Checked = true;
                rdb_formate2.Checked = false;
            }
            if (type == "1")
            {
                rdb_formate1.Checked = false;
                rdb_formate2.Checked = true;
            }
        }
        //if (TabContainer1.ActiveTabIndex == 3)
        //{
        //    spvl = 4;
        //    maindiv.Visible = true;
        //    string type = ddltype.SelectedItem.Text;
        //    string edulevel = ddledu.SelectedItem.Text;
        //    string concat = type + "(" + edulevel + ")";
        //    selecttype.InnerHtml = concat;


        //}
        if (TabContainer1.ActiveTabIndex == 3)
        {

            Maintable.Visible = true;
            // orderbysetting.Visible = true;
            txt_Degree.Enabled = false;
            txt_Branch.Enabled = false;
            ddledu.Enabled = false;
            ddltype.Enabled = false;
            loadreligion();
            loadcommunity();
            spvl = 4;
            columnbasic();
            //selectcolumnbasicorderby();
            maindiv.Visible = true;
            panel2.Visible = true;
            columnorderby();
            selectcolumnORDERBY();
            selectcolumnfunctionorderby();
            string percentagevalue = dt.GetFunction("select column_name from admitcolumnset where textcriteria='percent'");
            if (percentagevalue != "0")
            {
                ddl_percentagesetting.SelectedIndex = ddl_percentagesetting.Items.IndexOf(ddl_percentagesetting.Items.FindByValue(Convert.ToString(percentagevalue)));
            }
        }
        //if (TabContainer1.ActiveTabIndex == 4)
        //{
        //    spvl = 5;
        //    Maintable.Visible = false;
        //    //maindiv.Visible = true;
        //    //string type = ddltype.SelectedItem.Text;
        //    //string edulevel = ddledu.SelectedItem.Text;
        //    //string concat = type + "(" + edulevel + ")";
        //    //selecttype.InnerHtml = concat;


        //}
        spread1();

    }

    public void bindreglion()
    {
        try
        {
            cblreligion.Items.Clear();
            string selectquery = "select TextVal,TextCode from textvaltable where TextCriteria='relig' and TextCriteria2='relig1' and TextVal<>'' and college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' order by TextVal";
            ds.Clear();
            ds = dt.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblreligion.DataSource = ds;
                cblreligion.DataTextField = "TextVal";
                cblreligion.DataValueField = "TextCode";
                cblreligion.DataBind();


            }
            if (cblreligion.Items.Count > 0)
            {
                int seatcount = 0;
                for (int i = 0; i < cblreligion.Items.Count; i++)
                {
                    seatcount++;
                    cblreligion.Items[i].Selected = true;
                }
                txt_Religion.Text = "Religion(" + seatcount.ToString() + ")";
            }


        }
        catch
        {
        }
    }

    public void bindcommunity()
    {
        try
        {
            cblcommunity.Items.Clear();
            string selectquery = "select TextVal,TextCode  from textvaltable where TextCriteria='comm' and TextCriteria2='comm1'  and college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' and TextVal<>'' order by TextVal ";
            ds.Clear();
            ds = dt.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblcommunity.DataSource = ds;
                cblcommunity.DataTextField = "TextVal";
                cblcommunity.DataValueField = "TextCode";
                cblcommunity.DataBind();
            }
            if (cblcommunity.Items.Count > 0)
            {
                int seatcount = 0;
                for (int i = 0; i < cblcommunity.Items.Count; i++)
                {
                    seatcount++;
                    cblcommunity.Items[i].Selected = true;
                }
                txt_Community.Text = "Community(" + seatcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void loadreligion()
    {
        try
        {
            string religion = "";
            cblreligion.Items.Clear();
            string reliquery = "select distinct Textval,textcode from Textvaltable t,applyn a where a.religion=t.TextCode and TextCriteria ='relig' and batch_year='" + txtbatch.Text + "' and t.college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' ";
            ds.Clear();
            ds = dt.select_method_wo_parameter(reliquery, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblreligion.DataSource = ds;
                    cblreligion.DataTextField = "TextVal";
                    cblreligion.DataValueField = "textcode";
                    cblreligion.DataBind();

                    cbl_region1.DataSource = ds;
                    cbl_region1.DataTextField = "TextVal";
                    cbl_region1.DataValueField = "TextCode";
                    cbl_region1.DataBind();

                    if (cblreligion.Items.Count > 0)
                    {
                        for (int i = 0; i < cblreligion.Items.Count; i++)
                        {
                            cblreligion.Items[i].Selected = true;
                            religion = Convert.ToString(cblreligion.Items[i].Text);
                        }
                        if (cblreligion.Items.Count == 1)
                        {
                            txt_Religion.Text = "" + religion + "";
                        }
                        else
                        {
                            txt_Religion.Text = "Religion(" + cblreligion.Items.Count + ")";
                        }
                        cbreligion.Checked = true;
                    }
                }
            }
            else
            {
                txt_Religion.Text = "--Select--";
                cbreligion.Checked = false;
            }
        }
        catch
        {
        }
    }

    public void loadcommunity()
    {
        try
        {
            string comm = "";
            string selq = "SELECT Distinct community,T.TextVal  FROM applyn A,TextValTable T WHERE  T.TextCode =A.community  AND TextVal<>'' and t.college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "'  ";
            ds.Clear();
            ds = dt.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblcommunity.DataSource = ds;
                    cblcommunity.DataTextField = "TextVal";
                    cblcommunity.DataValueField = "community";
                    cblcommunity.DataBind();
                    if (cblcommunity.Items.Count > 0)
                    {
                        for (int i = 0; i < cblcommunity.Items.Count; i++)
                        {
                            cblcommunity.Items[i].Selected = true;
                            comm = Convert.ToString(cblcommunity.Items[i].Text);
                        }
                        if (cblcommunity.Items.Count == 1)
                        {
                            txt_Community.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_Community.Text = "Community(" + cblcommunity.Items.Count + ")";
                        }
                        cbcommunity.Checked = true;
                    }
                }
            }
            else
            {
                txt_Community.Text = "--Select--";
                cbcommunity.Checked = false;
            }
        }
        catch
        {

        }
    }

    protected void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            chkIncMarks.Checked = false;
            setIncludeMarks();
            string mainvalue1 = ""; bool chk = false;
            for (int j = 0; j < cheklist_Branch.Items.Count; j++)
            {
                if (cheklist_Branch.Items[j].Selected == true)
                {
                    string subvalue = cheklist_Branch.Items[j].Value;
                    if (mainvalue1 == "")
                    {
                        mainvalue1 = subvalue;
                    }
                    else
                    {
                        mainvalue1 = mainvalue1 + "'" + "," + "'" + subvalue;
                    }
                }

            }
            if (spvl == 2)
            {
                if (ddledu.SelectedItem.Text == "UG")
                {
                    if (rdbCompulsory.Checked == true)
                    {
                        string query = "delete admitcolumnset where textcriteria ='subjec' and user_code =" + user_code + " and college_code =" + college_code + " and setcolumn in('" + mainvalue1 + "') ";
                        int d = dt.update_method_wo_parameter(query, "Text");
                        if (d != 0)
                        {
                            chk = true;
                        }
                        FpSpread1.SaveChanges();
                        if (FpSpread1.Sheets[0].RowCount > 0)
                        {
                            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                for (int j = 4; j < FpSpread1.Sheets[0].ColumnCount; j++)
                                {
                                    int value = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, j].Value);
                                    if (value == 1)
                                    {
                                        FpSpread1.Sheets[0].Cells[i, j].Value = 0;
                                    }

                                }
                            }
                        }
                        FpSpread1.SaveChanges();
                    }
                    if (rdbancillary.Checked == true)
                    {
                        string query = "delete admitcolumnset where textcriteria ='subjea' and user_code =" + user_code + " and college_code =" + college_code + " and setcolumn in('" + mainvalue1 + "') ";
                        int d = dt.update_method_wo_parameter(query, "Text");
                        if (d != 0)
                        {
                            chk = true;
                        }
                        FpSpread4.SaveChanges();
                        if (FpSpread4.Sheets[0].RowCount > 0)
                        {
                            for (int i = 0; i < FpSpread4.Sheets[0].RowCount; i++)
                            {
                                for (int j = 4; j < FpSpread4.Sheets[0].ColumnCount; j++)
                                {
                                    int value = Convert.ToInt32(FpSpread4.Sheets[0].Cells[i, j].Value);
                                    if (value == 1)
                                    {
                                        FpSpread4.Sheets[0].Cells[i, j].Value = 0;
                                    }

                                }
                            }
                        }
                        FpSpread4.SaveChanges();
                    }
                    if (rdblanguage.Checked == true)
                    {
                        string query = "delete admitcolumnset where textcriteria ='subjea' and user_code =" + user_code + " and college_code =" + college_code + " and setcolumn in('" + mainvalue1 + "') ";
                        int d = dt.update_method_wo_parameter(query, "Text");
                        if (d != 0)
                        {
                            chk = true;
                        }
                        FpSpread5.SaveChanges();
                        if (FpSpread5.Sheets[0].RowCount > 0)
                        {
                            for (int i = 0; i < FpSpread5.Sheets[0].RowCount; i++)
                            {
                                for (int j = 4; j < FpSpread5.Sheets[0].ColumnCount; j++)
                                {
                                    int value = Convert.ToInt32(FpSpread5.Sheets[0].Cells[i, j].Value);
                                    if (value == 1)
                                    {
                                        FpSpread5.Sheets[0].Cells[i, j].Value = 0;
                                    }

                                }
                            }
                        }
                        FpSpread5.SaveChanges();
                    }
                }
                if (ddledu.SelectedItem.Text == "PG")
                {
                    string query = "delete admitcolumnset where textcriteria ='subjec' and user_code =" + user_code + " and college_code =" + college_code + " and setcolumn in('" + mainvalue1 + "') ";
                    int d = dt.update_method_wo_parameter(query, "Text");
                    if (d != 0)
                    {
                        chk = true;
                    }
                    FpSpread2.SaveChanges();
                    if (FpSpread2.Sheets[0].RowCount > 0)
                    {
                        for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
                        {
                            for (int j = 4; j < FpSpread2.Sheets[0].ColumnCount; j++)
                            {
                                int value = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, j].Value);
                                if (value == 1)
                                {
                                    FpSpread2.Sheets[0].Cells[i, j].Value = 0;
                                }

                            }
                        }
                    }
                    FpSpread2.SaveChanges();
                }
            }
            else if (spvl == 1 || spvl == 0)
            {
                spread1();
                if (religiongrid.Rows.Count > 0)
                {
                    int columncount = religiongrid.Rows[0].Cells.Count;
                    ArrayList columsvalesnew = new ArrayList();
                    ArrayList rowsvalesnew = new ArrayList();
                    columsvalesnew = (ArrayList)ViewState["Columnvalues"];
                    rowsvalesnew = (ArrayList)ViewState["Rowsvalues"];
                    for (int i = 0; i < religiongrid.Rows.Count; i++)
                    {
                        for (int j = 5; j < columncount; j++)
                        {
                            string id = "txtcoummunity" + j;
                            (religiongrid.Rows[i].FindControl(id) as TextBox).Text = "";

                        }

                    }
                }
            }
            else if (spvl == 3)
            {
                string query = "delete admitcolumnset where textcriteria ='column' and user_code =" + user_code + " and college_code =" + college_code + " ";
                int d = dt.update_method_wo_parameter(query, "Text");
                if (d != 0)
                {
                    chk = true;
                }
                val = 0;
                column();
                FpSpread7.Visible = false;
                selectcolumn();

            }
            else if (spvl == 4)
            {
                string textcriteria = "";
                string priority = "";
                if (rdo_comm.Checked == true)
                {
                    textcriteria = "comm";
                    priority = "priority1";
                }
                else if (rdo_reli.Checked == true)
                {
                    textcriteria = "relig";
                    priority = "priority2";
                }

                string query = "Update textvaltable set  " + priority + "=''  where textcriteria ='" + textcriteria + "' and college_code ='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' ";
                int d = dt.update_method_wo_parameter(query, "Text");

                query = "Update Master_Settings set  value=''  where settings='orderbymarks'";
                d = dt.update_method_wo_parameter(query, "Text");
                if (d != 0)
                {
                    chk = true;
                }
                //textcriteria = "OrderByMarks";
                //query = "delete admitcolumnset where textcriteria ='" + textcriteria + "' and user_code =" + user_code + " and college_code =" + college_code + " ";
                //d = dt.update_method_wo_parameter(query, "Text");
                val = 0;
                val1 = 0;
                columnorderby();
                FpSpread9.Visible = false;
                selectcolumnORDERBY();

            }
            if (chk == true)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Reset Successfully\");", true);
            }

            //else if (spvl == 4)
            //{
            //    txt_deprecom.Text = "";
            //    txt_mangquta.Text = "";
            //    string type = ddltype.SelectedItem.Text;
            //    string level = ddledu.SelectedItem.Text;
            //    string concate = type + "-" + level;
            //    string allocate = "Managmentallocate" + concate + "";
            //    string allocate1 = "Departmentallocate" + concate + "";
            //    string value = "delete from admitcolumnset where user_code='" + user_code + "' and college_code='" + college_code + "' and textcriteria='relig' and setcolumn in('" + mainvalue1 + "')";
            //    value = value + " delete from admitcolumnset where user_code='" + user_code + "' and college_code='" + college_code + "' and textcriteria='community' and setcolumn in('" + mainvalue1 + "')";
            //    value = value + " delete from Master_Settings where settings='" + allocate + "'";
            //    value = value + " delete from Master_Settings where settings='" + allocate1 + "'";
            //    int d = dt.update_method_wo_parameter(value, "Text");
            //    if (religrid.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < religrid.Rows.Count; i++)
            //        {
            //            (religrid.Rows[i].FindControl("txt_percentageornumber") as TextBox).Text = "";
            //        }
            //    }
            //    if (gridcommunity.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < gridcommunity.Rows.Count; i++)
            //        {
            //            (gridcommunity.Rows[i].FindControl("txt_compercent") as TextBox).Text = "";
            //        }
            //    }
            //    report_grid.Visible = false;
            //}
        }
        catch
        {

        }

    }

    protected void btnset_Click(object sender, EventArgs e)
    {
        try
        {
            setIncludeMarks();
            if (TabContainer1.ActiveTabIndex == 2)
            {
                string type = "";
                if (rdb_formate1.Checked == true)
                {
                    type = "0";
                }
                if (rdb_formate2.Checked == true)
                {
                    type = "1";
                }

                string q1 = "if exists(select*from admitcolumnset where textcriteria='Section process Format' and user_code='" + user_code + "' and college_code='" + college_code + "')update  admitcolumnset set setcolumn='" + type + "' where textcriteria='Section process Format' and user_code='" + user_code + "' and college_code='" + college_code + "' else insert into admitcolumnset (user_code,college_code,textcriteria,setcolumn) values('" + user_code + "','" + college_code + "','Section process Format','" + type + "')";
                int up = dt.update_method_wo_parameter(q1, "Text");
            }

            val1 = 0;
            Dictionary<int, int> dicsubcol = new Dictionary<int, int>();
            if (spvl == 2)
            {
                int h = 0;
                if (ddledu.SelectedItem.Text == "UG")
                {
                    if (rdbCompulsory.Checked == true)
                    {
                        FpSpread1.SaveChanges();

                        val = 0;

                        for (int g = 0; g < FpSpread1.Rows.Count; g++)
                        {
                            string dl = "delete from admitcolumnset where user_code='" + user_code + "' and college_code='" + college_code + "' and textcriteria='subjec' and setcolumn='" + FpSpread1.Sheets[0].Cells[g, 3].Tag + "'";
                            int f1 = dt.update_method_wo_parameter(dl, "text");
                            for (int t = 0; t < FpSpread1.Columns.Count; t++)
                            {
                                if (t >= 4)
                                {
                                    int value = Convert.ToInt32(FpSpread1.Sheets[0].Cells[g, t].Value);
                                    if (value == 1)
                                    {
                                        int head = Convert.ToInt32(FpSpread1.Sheets[0].ColumnHeader.Cells[0, t].Tag);
                                        h = 1;

                                        string set = "insert into admitcolumnset(user_code,setcolumn,column_name,college_code,textcriteria) values(" + user_code + ",'" + FpSpread1.Sheets[0].Cells[g, 3].Tag + "','" + head + "'," + college_code + ",'subjec')";
                                        int f = dt.update_method_wo_parameter(set, "text");
                                    }
                                }
                            }
                        }
                    }
                    if (rdbancillary.Checked == true)
                    {
                        FpSpread4.SaveChanges();

                        val = 0;

                        for (int g = 0; g < FpSpread4.Rows.Count; g++)
                        {
                            string dl = "delete from admitcolumnset where user_code='" + user_code + "' and college_code='" + college_code + "' and textcriteria='subjea' and setcolumn='" + FpSpread4.Sheets[0].Cells[g, 3].Tag + "'";
                            int f1 = dt.update_method_wo_parameter(dl, "text");
                            for (int t = 0; t < FpSpread4.Columns.Count; t++)
                            {
                                if (t >= 4)
                                {
                                    int value = Convert.ToInt32(FpSpread4.Sheets[0].Cells[g, t].Value);
                                    if (value == 1)
                                    {
                                        int head = Convert.ToInt32(FpSpread4.Sheets[0].ColumnHeader.Cells[0, t].Tag);
                                        h = 1;

                                        string set = "insert into admitcolumnset(user_code,setcolumn,column_name,college_code,textcriteria) values(" + user_code + ",'" + FpSpread4.Sheets[0].Cells[g, 3].Tag + "','" + head + "'," + college_code + ",'subjea')";
                                        int f = dt.update_method_wo_parameter(set, "text");
                                    }
                                }
                            }
                        }
                    }
                    if (rdblanguage.Checked == true)
                    {
                        FpSpread5.SaveChanges();

                        val = 0;

                        for (int g = 0; g < FpSpread5.Rows.Count; g++)
                        {
                            string dl = "delete from admitcolumnset where user_code='" + user_code + "' and college_code='" + college_code + "' and textcriteria='subjel' and setcolumn='" + FpSpread5.Sheets[0].Cells[g, 3].Tag + "'";
                            int f1 = dt.update_method_wo_parameter(dl, "text");
                            for (int t = 0; t < FpSpread5.Columns.Count; t++)
                            {
                                if (t >= 4)
                                {
                                    int value = Convert.ToInt32(FpSpread5.Sheets[0].Cells[g, t].Value);
                                    if (value == 1)
                                    {
                                        int head = Convert.ToInt32(FpSpread5.Sheets[0].ColumnHeader.Cells[0, t].Tag);
                                        h = 1;

                                        string set = "insert into admitcolumnset(user_code,setcolumn,column_name,college_code,textcriteria) values(" + user_code + ",'" + FpSpread5.Sheets[0].Cells[g, 3].Tag + "','" + head + "'," + college_code + ",'subjel')";
                                        int f = dt.update_method_wo_parameter(set, "text");
                                    }
                                }
                            }
                        }
                    }

                }
                if (ddledu.SelectedItem.Text == "PG")
                {
                    FpSpread2.SaveChanges();

                    val = 0;

                    for (int g = 0; g < FpSpread2.Rows.Count; g++)
                    {
                        string dl = "delete from admitcolumnset where user_code='" + user_code + "' and college_code='" + college_code + "' and textcriteria='subjec' and setcolumn='" + FpSpread2.Sheets[0].Cells[g, 3].Tag + "'";
                        int f1 = dt.update_method_wo_parameter(dl, "text");
                        for (int t = 0; t < FpSpread2.Columns.Count; t++)
                        {
                            if (t >= 4)
                            {
                                int value = Convert.ToInt32(FpSpread2.Sheets[0].Cells[g, t].Value);
                                if (value == 1)
                                {
                                    int head = Convert.ToInt32(FpSpread2.Sheets[0].ColumnHeader.Cells[0, t].Tag);
                                    h = 1;

                                    string set = "insert into admitcolumnset(user_code,setcolumn,column_name,college_code,textcriteria) values(" + user_code + ",'" + FpSpread2.Sheets[0].Cells[g, 3].Tag + "','" + head + "'," + college_code + ",'subjec')";
                                    int f = dt.update_method_wo_parameter(set, "text");
                                }
                            }
                        }
                    }
                }

                if (h == 1)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\" Not Saved\");", true);
                }
            }
            else if (spvl == 0 || spvl == 1)
            {
                if (ddl_percentagesetting.Items.Count > 0)
                {
                    string q = "insert into admitcolumnset (user_code,setcolumn,column_name,priority,college_code,textcriteria) values('" + user_code + "','" + ddl_percentagesetting.SelectedItem.Text + "','" + ddl_percentagesetting.SelectedItem.Value + "','1','" + Convert.ToString(ddl_college.SelectedItem.Value) + "','percent')";
                    dt.select_method_wo_parameter(q, "text");
                }

                if (religiongrid.Rows.Count > 0)
                {
                    int h = 0;
                    int columncount = religiongrid.Rows[0].Cells.Count;
                    ArrayList columsvalesnew = new ArrayList();
                    ArrayList rowsvalesnew = new ArrayList();
                    columsvalesnew = (ArrayList)ViewState["Columnvalues"];
                    rowsvalesnew = (ArrayList)ViewState["Rowsvalues"];
                    for (int i = 0; i < religiongrid.Rows.Count; i++)
                    {
                        int col = 0;
                        string rowvalue = Convert.ToString(rowsvalesnew[i]);
                        for (int j = 5; j < columncount; j++)
                        {
                            string id = "txtcoummunity" + j;
                            TextBox vlaue = (TextBox)religiongrid.Rows[i].FindControl(id);
                            string txt = vlaue.Text;
                            if (txt.Trim() == "")
                            {
                                txt = "0";
                            }
                            string columnsvalue = Convert.ToString(columsvalesnew[col]);
                            col++;
                            if (txt.Trim() != "")
                            {
                                string selectupadatequery = "if not exists (select priority  from admitcolumnset where textcriteria ='relig' and college_code=" + Convert.ToString(ddl_college.SelectedItem.Value) + " and priority<>0 and setcolumn='" + rowvalue + "' and column_name ='" + columnsvalue + "' and user_code=" + user_code + ")";
                                selectupadatequery = selectupadatequery + " insert into admitcolumnset (user_code,setcolumn,column_name,priority,college_code,textcriteria) values('" + user_code + "','" + rowvalue + "','" + columnsvalue + "','" + txt + "','" + Convert.ToString(ddl_college.SelectedItem.Value) + "','relig')";
                                selectupadatequery = selectupadatequery + "else update admitcolumnset set priority='" + txt + "' where textcriteria='relig' and college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' and user_code='" + user_code + "' and setcolumn='" + rowvalue + "' and column_name ='" + columnsvalue + "'";
                                int a = dt.update_method_wo_parameter(selectupadatequery, "Text");
                                if (a != 0)
                                {
                                    h = 1;
                                }
                            }

                        }
                        if (h == 1)
                        {
                            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\" Not Saved\");", true);
                        }
                    }
                }
            }
            else if (spvl == 3)
            {
                FpSpread6.SaveChanges();
                val = 0;
                int h = 0;
                string dl = "delete from admitcolumnset where user_code='" + user_code + "' and college_code='" + college_code + "' and textcriteria='column'";
                int f1 = dt.update_method_wo_parameter(dl, "text");
                for (int g = 0; g < FpSpread6.Rows.Count; g++)
                {
                    int value = Convert.ToInt32(FpSpread6.Sheets[0].Cells[g, 3].Value);
                    if (value == 1)
                    {
                        h = 1;
                        string set = "insert into admitcolumnset(user_code,setcolumn,column_name,priority,college_code,textcriteria) values(" + user_code + ",'" + FpSpread6.Sheets[0].Cells[g, 1].Text + "','" + FpSpread6.Sheets[0].Cells[g, 2].Text + "','" + FpSpread6.Sheets[0].Cells[g, 4].Text + "'," + college_code + ",'column')";
                        int f = dt.update_method_wo_parameter(set, "text");
                    }
                }
                if (h == 1)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\" Not Saved\");", true);
                }

            }
            else if (spvl == 4)
            {
                FpSpread8.SaveChanges();
                val = 0;
                int h = 0;
                string textcriteria = "";
                string priority1 = "";
                if (rdo_comm.Checked == true)
                {
                    textcriteria = "comm";
                    priority1 = "priority1";
                }
                else if (rdo_reli.Checked == true)
                {
                    textcriteria = "relig";
                    priority1 = "priority2";
                }
                if (ddl_percentagesetting.Items.Count > 0)
                {
                    string qu = "if exists(select * from admitcolumnset where textcriteria='percent')update admitcolumnset set setcolumn='" + ddl_percentagesetting.SelectedItem.Text + "',column_name='" + ddl_percentagesetting.SelectedItem.Value + "' where textcriteria='percent' else insert into admitcolumnset (user_code,setcolumn,column_name,priority,college_code,textcriteria) values('" + user_code + "','" + ddl_percentagesetting.SelectedItem.Text + "','" + ddl_percentagesetting.SelectedItem.Value + "','1','" + Convert.ToString(ddl_college.SelectedItem.Value) + "','percent')";
                    dt.update_method_wo_parameter(qu, "text");
                }
                for (int g = 0; g < FpSpread8.Rows.Count; g++)
                {
                    int value = Convert.ToInt32(FpSpread8.Sheets[0].Cells[g, 3].Value);
                    if (value == 1)
                    {
                        h = 1;
                        string set = "Update textvaltable set " + priority1 + "='" + FpSpread8.Sheets[0].Cells[g, 4].Text + "' where textcriteria='" + textcriteria + "' and TextCode='" + FpSpread8.Sheets[0].Cells[g, 2].Text + "'  and college_code='" + Convert.ToString(ddl_college.SelectedItem.Value) + "'";
                        int f = dt.update_method_wo_parameter(set, "text");
                    }
                }
                string add = "";
                int value1 = 0;

                for (int y = 1; y < 5; y++)
                {
                    for (int g = 0; g < FpSpread10.Rows.Count; g++)
                    {
                        value1 = Convert.ToInt32(FpSpread10.Sheets[0].Cells[g, 3].Value);
                        if (value1 == 1)
                        {
                            if (add == "")
                            {
                                if (FpSpread10.Sheets[0].Cells[g, 4].Text == Convert.ToString(y))
                                {
                                    add = FpSpread10.Sheets[0].Cells[g, 2].Text;
                                }
                            }
                            else
                            {
                                if (FpSpread10.Sheets[0].Cells[g, 4].Text == Convert.ToString(y))
                                {
                                    add = add + "," + FpSpread10.Sheets[0].Cells[g, 2].Text;
                                }
                            }
                        }


                    }
                }

                if (h == 1)
                {
                    string set1 = "if exists(select * from Master_Settings where settings='OrderbyMarks')update Master_Settings set value='" + add + "' where settings='OrderbyMarks'  else insert into Master_Settings (settings,value) values('OrderbyMarks','" + add + "')";

                    int f1 = dt.update_method_wo_parameter(set1, "text");

                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\" Not Saved\");", true);
                }

            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void FpSpread1_command(object sender, EventArgs e)
    {
        try
        {
            Button8.Focus();
        }
        catch (Exception ex)
        {

        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        spread1();
    }

    public void spread1()
    {
        try
        {
            string mainvalue = "";
            for (int j = 0; j < cheklist_Degree.Items.Count; j++)
            {
                if (cheklist_Degree.Items[j].Selected == true)
                {
                    string subvalue = cheklist_Degree.Items[j].Value;
                    if (mainvalue == "")
                    {
                        mainvalue = subvalue;
                    }
                    else
                    {
                        mainvalue = mainvalue + "'" + "," + "'" + subvalue;
                    }
                }

            }

            string mainvalue1 = "";
            for (int j = 0; j < cheklist_Branch.Items.Count; j++)
            {
                if (cheklist_Branch.Items[j].Selected == true)
                {
                    string subvalue = cheklist_Branch.Items[j].Value;
                    if (mainvalue1 == "")
                    {
                        mainvalue1 = subvalue;
                    }
                    else
                    {
                        mainvalue1 = mainvalue1 + "'" + "," + "'" + subvalue;
                    }
                }

            }


            string branchvl = "select distinct degree.degree_code,degree.No_Of_seats,department.dept_name,degree.Acronym,course.Course_Name  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + mainvalue + "') and degree.Degree_Code in('" + mainvalue1 + "') and deptprivilages.Degree_code=degree.Degree_code";
            ds = dt.select_method_wo_parameter(branchvl, "text");

            ArrayList rowsvales = new ArrayList();

            int coluncount = 0;
            int rowscount = 0;
            if (spvl == 0 || spvl == 1)
            {
                string priority = "select * from textvaltable where TextCriteria='relig'  and college_code='" + college_code + "'  and textval!='' order by textval";
                priority = priority + " select * from textvaltable where TextCriteria='SCast'  and college_code='" + college_code + "' and textval!='' order by textval";
                ds1 = dt.select_method_wo_parameter(priority, "text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable data = new DataTable();
                    DataRow datarow = null;
                    data.Columns.Add("S.No", typeof(string));
                    data.Columns.Add("Education Level", typeof(string));
                    data.Columns.Add("Course", typeof(string));
                    data.Columns.Add("Department", typeof(string));
                    data.Columns.Add("Total No Of Seat", typeof(string));

                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToString(ds1.Tables[0].Rows[i]["TextVal"]).ToUpper() != "CHRISTIAN")
                        {
                            data.Columns.Add(Convert.ToString(ds1.Tables[0].Rows[i]["TextVal"]));
                            columsvales.Add(Convert.ToString(ds1.Tables[0].Rows[i]["TextCode"]));
                        }

                        else
                        {
                            if (ds1.Tables[1].Rows.Count > 0)
                            {
                                for (int jk = 0; jk < ds1.Tables[1].Rows.Count; jk++)
                                {
                                    data.Columns.Add(Convert.ToString(ds1.Tables[1].Rows[jk]["TextVal"]));
                                    columsvales.Add(Convert.ToString(ds1.Tables[1].Rows[jk]["TextCode"]));
                                }
                            }
                        }
                    }

                    ViewState["Columnvalues"] = columsvales;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int count = 0;
                        rowscount = ds.Tables[0].Rows.Count;
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {

                            count++;
                            datarow = data.NewRow();
                            datarow[0] = Convert.ToString(count);
                            datarow[1] = Convert.ToString(ds.Tables[0].Rows[j]["Course_Name"]);
                            datarow[2] = Convert.ToString(ds.Tables[0].Rows[j]["dept_name"]);
                            datarow[3] = Convert.ToString(ds.Tables[0].Rows[j]["Acronym"]);
                            datarow[4] = Convert.ToString(ds.Tables[0].Rows[j]["No_Of_seats"]);
                            rowdegvalue.Add(Convert.ToString(ds.Tables[0].Rows[j]["degree_code"]));
                            data.Rows.Add(datarow);
                        }

                    }
                    ViewState["Rowsvalues"] = rowdegvalue;
                    //ViewState["CurrentTable1"] = data;
                    religiongrid.DataSource = data;
                    religiongrid.DataBind();

                }
            }
            if (spvl == 2)
            {
                //spvl = 0;

                if (ddledu.SelectedItem.Text == "UG")
                {
                    FpSpread2.Visible = false;
                    FpSpread1.Visible = true;
                    FpSpread4.Visible = false;
                    FpSpread5.Visible = false;
                    chkcel1.AutoPostBack = false;

                    if (rdbCompulsory.Checked == true)
                    {
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#393965");
                        darkstyle.ForeColor = System.Drawing.Color.White;
                        // darkstyle.Border = new FarPoint.Web.Spread.Border(Color.Crimson);
                        // Apply the new style.
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread1.Visible = true;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 4;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = false;
                        FpSpread1.Height = 495;
                        FpSpread1.Width = 960;
                        FpSpread1.CommandBar.Visible = false;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].BackColor = Color.MistyRose;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Education Level";
                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].BackColor = Color.MistyRose;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Course";
                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].BackColor = Color.MistyRose;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].BackColor = Color.MistyRose;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].Columns[0].Width = 40;
                        FpSpread1.Sheets[0].Columns[1].Width = 100;
                        FpSpread1.Sheets[0].Columns[2].Width = 100;
                        FpSpread1.Sheets[0].Columns[3].Width = 150;
                        FpSpread1.Sheets[0].Columns[1].Visible = false;
                        FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].FrozenColumnCount = 4;


                        string priority = "select distinct t.TextCode ,TextVal from applyn a, Stud_prev_details s,perv_marks_history p,TextValTable t where a.app_no =s.app_no and s.course_entno =p.course_entno and p.psubjectno=t.TextCode and a.degree_code in ('" + mainvalue1 + "') and a.college_code='" + Session["collegecode"].ToString() + "' and batch_year =2015 and TextVal<>'---Select---' order by TextVal asc";
                        priority = priority + "  select * from admitcolumnset where textcriteria ='subjec' and college_code='" + Session["collegecode"].ToString() + "' and  setcolumn in('" + mainvalue1 + "')";
                        priority = priority + "  select distinct t.TextCode ,TextVal from applyn a, Stud_prev_details s,perv_marks_history p,TextValTable t where a.app_no =s.app_no and s.course_entno =p.course_entno and s.Part1Language =t.TextCode  and a.degree_code in ('" + mainvalue1 + "') and a.college_code='" + Session["collegecode"].ToString() + "' and batch_year =2015 and TextVal<>'---Select---' order by TextVal asc ";
                        priority = priority + "  select distinct t.TextCode ,TextVal from applyn a, Stud_prev_details s,perv_marks_history p,TextValTable t where a.app_no =s.app_no and s.course_entno =p.course_entno and s.Part2Language =t.TextCode  and a.degree_code in ('" + mainvalue1 + "') and a.college_code='" + Session["collegecode"].ToString() + "' and batch_year =2015 and TextVal<>'---Select---' order by TextVal asc ";
                        ds1 = dt.select_method_wo_parameter(priority, "text");
                        int count = 1;
                        int g = 0;
                        ArrayList checkarray = new ArrayList();
                        //int f = 0;
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            if (ds1.Tables[3].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds1.Tables[2].Rows.Count; i++)
                                {


                                    if (!checkarray.Contains(ds1.Tables[2].Rows[i]["textval"].ToString()))
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = ds1.Tables[2].Rows[i]["textval"].ToString();
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[2].Rows[i]["TextCode"].ToString();
                                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        checkarray.Add(ds1.Tables[2].Rows[i]["textval"].ToString());
                                    }

                                }
                            }
                            if (ds1.Tables[3].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds1.Tables[3].Rows.Count; i++)
                                {
                                    if (!checkarray.Contains(ds1.Tables[3].Rows[i]["textval"].ToString()))
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;

                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = ds1.Tables[3].Rows[i]["textval"].ToString();
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[3].Rows[i]["TextCode"].ToString();
                                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        checkarray.Add(ds1.Tables[3].Rows[i]["textval"].ToString());
                                    }
                                }

                            }
                            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                            {
                                if (!checkarray.Contains(ds1.Tables[0].Rows[i]["textval"].ToString()))
                                {
                                    FpSpread1.Sheets[0].ColumnCount++;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = ds1.Tables[0].Rows[i]["textval"].ToString();
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[0].Rows[i]["TextCode"].ToString();
                                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    checkarray.Add(ds1.Tables[0].Rows[i]["textval"].ToString());
                                }
                            }

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(count);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Times New Roman";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ddledu.SelectedItem.Text;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Times New Roman";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[k]["Course_Name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Times New Roman";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[k]["dept_name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[k]["degree_code"].ToString();
                                    hat.Add(ds.Tables[0].Rows[k]["degree_code"].ToString(), ds.Tables[0].Rows[k]["dept_name"].ToString());

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Times New Roman";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                    count++;
                                    for (int i = 4; i < checkarray.Count + 4; i++)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].CellType = chkcel1;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].Value = 0;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].Value = "0";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    g++;

                                }

                            }

                            if (ds1.Tables[1].Rows.Count > 0)
                            {
                                if (FpSpread1.Sheets[0].RowCount > 0)
                                {
                                    DataView dv = new DataView();
                                    DataView dv1 = new DataView();
                                    for (int jk = 0; jk < FpSpread1.Sheets[0].RowCount; jk++)
                                    {
                                        string degree_code = Convert.ToString(FpSpread1.Sheets[0].Cells[jk, 3].Tag);
                                        ds1.Tables[1].DefaultView.RowFilter = "setcolumn=" + degree_code + "";
                                        dv = ds1.Tables[1].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            for (int ik = 4; ik < FpSpread1.Sheets[0].ColumnCount; ik++)
                                            {
                                                string columnname = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, ik].Tag);
                                                if (columnname.Trim() != "")
                                                {
                                                    ds1.Tables[1].DefaultView.RowFilter = "setcolumn=" + degree_code + " and column_name='" + columnname + "'";
                                                    dv1 = ds1.Tables[1].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[jk, ik].Value = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

                        }
                    }
                    if (rdbancillary.Checked == true)
                    {
                        FpSpread1.Visible = false;
                        FpSpread5.Visible = false;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#393965");
                        darkstyle.ForeColor = System.Drawing.Color.White;
                        // darkstyle.Border = new FarPoint.Web.Spread.Border(Color.Crimson);
                        // Apply the new style.
                        FpSpread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread4.Visible = true;
                        FpSpread4.Sheets[0].RowCount = 0;
                        FpSpread4.Sheets[0].ColumnCount = 4;
                        FpSpread4.Sheets[0].RowHeader.Visible = false;
                        FpSpread4.Sheets[0].AutoPostBack = false;
                        FpSpread4.Height = 495;
                        FpSpread4.Width = 960;
                        FpSpread4.CommandBar.Visible = false;

                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].BackColor = Color.MistyRose;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Education Level";
                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].BackColor = Color.MistyRose;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Course";
                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].BackColor = Color.MistyRose;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].BackColor = Color.MistyRose;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread4.Sheets[0].Columns[0].Width = 40;
                        FpSpread4.Sheets[0].Columns[1].Width = 100;
                        FpSpread4.Sheets[0].Columns[2].Width = 100;
                        FpSpread4.Sheets[0].Columns[3].Width = 150;
                        FpSpread4.Sheets[0].Columns[1].Visible = false;
                        FpSpread4.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread4.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread4.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread4.Sheets[0].FrozenColumnCount = 4;


                        string priority = "select distinct t.TextCode ,TextVal from applyn a, Stud_prev_details s,perv_marks_history p,TextValTable t where a.app_no =s.app_no and s.course_entno =p.course_entno and p.psubjectno=t.TextCode and a.degree_code in ('" + mainvalue1 + "') and a.college_code='" + Session["collegecode"].ToString() + "' and batch_year =2015 and TextVal<>'---Select---' order by TextVal asc";
                        priority = priority + "  select * from admitcolumnset where textcriteria ='subjea' and college_code='" + Session["collegecode"].ToString() + "' and  setcolumn in('" + mainvalue1 + "')";
                        priority = priority + "  select distinct t.TextCode ,TextVal from applyn a, Stud_prev_details s,perv_marks_history p,TextValTable t where a.app_no =s.app_no and s.course_entno =p.course_entno and s.Part1Language =t.TextCode  and a.degree_code in ('" + mainvalue1 + "') and a.college_code='" + Session["collegecode"].ToString() + "' and batch_year =2015 and TextVal<>'---Select---' order by TextVal asc ";
                        priority = priority + "  select distinct t.TextCode ,TextVal from applyn a, Stud_prev_details s,perv_marks_history p,TextValTable t where a.app_no =s.app_no and s.course_entno =p.course_entno and s.Part2Language =t.TextCode  and a.degree_code in ('" + mainvalue1 + "') and a.college_code='" + Session["collegecode"].ToString() + "' and batch_year =2015 and TextVal<>'---Select---' order by TextVal asc ";
                        ds1 = dt.select_method_wo_parameter(priority, "text");
                        int count = 1;
                        int g = 0;
                        ArrayList checkarray = new ArrayList();
                        //int f = 0;
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            if (ds1.Tables[3].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds1.Tables[2].Rows.Count; i++)
                                {


                                    if (!checkarray.Contains(ds1.Tables[2].Rows[i]["textval"].ToString()))
                                    {
                                        FpSpread4.Sheets[0].ColumnCount++;
                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Text = ds1.Tables[2].Rows[i]["textval"].ToString();
                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[2].Rows[i]["TextCode"].ToString();
                                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        checkarray.Add(ds1.Tables[2].Rows[i]["textval"].ToString());
                                    }

                                }
                            }
                            if (ds1.Tables[3].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds1.Tables[3].Rows.Count; i++)
                                {
                                    if (!checkarray.Contains(ds1.Tables[3].Rows[i]["textval"].ToString()))
                                    {
                                        FpSpread4.Sheets[0].ColumnCount++;

                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Text = ds1.Tables[3].Rows[i]["textval"].ToString();
                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[3].Rows[i]["TextCode"].ToString();
                                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        checkarray.Add(ds1.Tables[3].Rows[i]["textval"].ToString());
                                    }
                                }

                            }
                            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                            {
                                if (!checkarray.Contains(ds1.Tables[0].Rows[i]["textval"].ToString()))
                                {
                                    FpSpread4.Sheets[0].ColumnCount++;

                                    FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Text = ds1.Tables[0].Rows[i]["textval"].ToString();
                                    FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[0].Rows[i]["TextCode"].ToString();
                                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                    FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    checkarray.Add(ds1.Tables[0].Rows[i]["textval"].ToString());
                                }
                            }

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                {
                                    FpSpread4.Sheets[0].RowCount++;
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(count);
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Font.Name = "Times New Roman";
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Text = ddledu.SelectedItem.Text;
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Font.Name = "Times New Roman";
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[k]["Course_Name"].ToString();
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Font.Name = "Times New Roman";
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Font.Bold = true;

                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[k]["dept_name"].ToString();
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[k]["degree_code"].ToString();
                                    hat.Add(ds.Tables[0].Rows[k]["degree_code"].ToString(), ds.Tables[0].Rows[k]["dept_name"].ToString());

                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Font.Name = "Times New Roman";
                                    FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                    count++;
                                    for (int i = 4; i < checkarray.Count + 4; i++)
                                    {
                                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, i].CellType = chkcel1;
                                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, i].Value = 0;
                                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, i].Value = "0";
                                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    g++;

                                }

                            }

                            if (ds1.Tables[1].Rows.Count > 0)
                            {
                                if (FpSpread4.Sheets[0].RowCount > 0)
                                {
                                    DataView dv = new DataView();
                                    DataView dv1 = new DataView();
                                    for (int jk = 0; jk < FpSpread4.Sheets[0].RowCount; jk++)
                                    {
                                        string degree_code = Convert.ToString(FpSpread4.Sheets[0].Cells[jk, 3].Tag);
                                        ds1.Tables[1].DefaultView.RowFilter = "setcolumn=" + degree_code + "";
                                        dv = ds1.Tables[1].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            for (int ik = 4; ik < FpSpread4.Sheets[0].ColumnCount; ik++)
                                            {
                                                string columnname = Convert.ToString(FpSpread4.Sheets[0].ColumnHeader.Cells[0, ik].Tag);
                                                if (columnname.Trim() != "")
                                                {
                                                    ds1.Tables[1].DefaultView.RowFilter = "setcolumn=" + degree_code + " and column_name='" + columnname + "'";
                                                    dv1 = ds1.Tables[1].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        FpSpread4.Sheets[0].Cells[jk, ik].Value = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            FpSpread4.Sheets[0].PageSize = FpSpread4.Sheets[0].RowCount;

                        }
                    }
                    if (rdblanguage.Checked == true)
                    {
                        FpSpread1.Visible = false;
                        FpSpread4.Visible = false;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#393965");
                        darkstyle.ForeColor = System.Drawing.Color.White;
                        // darkstyle.Border = new FarPoint.Web.Spread.Border(Color.Crimson);
                        // Apply the new style.
                        FpSpread5.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread5.Visible = true;
                        FpSpread5.Sheets[0].RowCount = 0;
                        FpSpread5.Sheets[0].ColumnCount = 4;
                        FpSpread5.Sheets[0].RowHeader.Visible = false;
                        FpSpread5.Sheets[0].AutoPostBack = false;
                        FpSpread5.Height = 495;
                        FpSpread5.Width = 960;
                        FpSpread5.CommandBar.Visible = false;

                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].BackColor = Color.MistyRose;
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Education Level";
                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].BackColor = Color.MistyRose;
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Course";
                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].BackColor = Color.MistyRose;
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].BackColor = Color.MistyRose;
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread5.Sheets[0].Columns[0].Width = 40;
                        FpSpread5.Sheets[0].Columns[1].Width = 100;
                        FpSpread5.Sheets[0].Columns[2].Width = 100;
                        FpSpread5.Sheets[0].Columns[3].Width = 150;
                        FpSpread5.Sheets[0].Columns[1].Visible = false;
                        FpSpread5.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread5.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread5.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread5.Sheets[0].FrozenColumnCount = 4;


                        string priority = "select distinct t.TextCode ,TextVal from applyn a, Stud_prev_details s,perv_marks_history p,TextValTable t where a.app_no =s.app_no and s.course_entno =p.course_entno and p.psubjectno=t.TextCode and a.degree_code in ('" + mainvalue1 + "') and a.college_code='" + Session["collegecode"].ToString() + "' and batch_year =2015 and TextCriteria2 ='subj1' and  TextVal<>'---Select---' order by TextVal asc";
                        priority = priority + "  select * from admitcolumnset where textcriteria ='subjel' and college_code='" + Session["collegecode"].ToString() + "' and  setcolumn in('" + mainvalue1 + "')";
                        priority = priority + "  select distinct t.TextCode ,TextVal from applyn a, Stud_prev_details s,perv_marks_history p,TextValTable t where a.app_no =s.app_no and s.course_entno =p.course_entno and s.Part1Language =t.TextCode  and a.degree_code in ('" + mainvalue1 + "') and a.college_code='" + Session["collegecode"].ToString() + "' and batch_year =2015 and TextVal<>'---Select---' order by TextVal asc ";
                        priority = priority + "  select distinct t.TextCode ,TextVal from applyn a, Stud_prev_details s,perv_marks_history p,TextValTable t where a.app_no =s.app_no and s.course_entno =p.course_entno and s.Part2Language =t.TextCode  and a.degree_code in ('" + mainvalue1 + "') and a.college_code='" + Session["collegecode"].ToString() + "' and batch_year =2015 and TextVal<>'---Select---' order by TextVal asc ";
                        ds1 = dt.select_method_wo_parameter(priority, "text");
                        int count = 1;
                        int g = 0;
                        ArrayList checkarray = new ArrayList();
                        //int f = 0;
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            if (ds1.Tables[3].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds1.Tables[2].Rows.Count; i++)
                                {


                                    if (!checkarray.Contains(ds1.Tables[2].Rows[i]["textval"].ToString()))
                                    {
                                        FpSpread5.Sheets[0].ColumnCount++;
                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Text = ds1.Tables[2].Rows[i]["textval"].ToString();
                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[2].Rows[i]["TextCode"].ToString();
                                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        checkarray.Add(ds1.Tables[2].Rows[i]["textval"].ToString());
                                    }

                                }
                            }
                            if (ds1.Tables[3].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds1.Tables[3].Rows.Count; i++)
                                {
                                    if (!checkarray.Contains(ds1.Tables[3].Rows[i]["textval"].ToString()))
                                    {
                                        FpSpread5.Sheets[0].ColumnCount++;

                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Text = ds1.Tables[3].Rows[i]["textval"].ToString();
                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[3].Rows[i]["TextCode"].ToString();
                                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        checkarray.Add(ds1.Tables[3].Rows[i]["textval"].ToString());
                                    }
                                }

                            }
                            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                            {
                                if (!checkarray.Contains(ds1.Tables[0].Rows[i]["textval"].ToString()))
                                {
                                    FpSpread5.Sheets[0].ColumnCount++;

                                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Text = ds1.Tables[0].Rows[i]["textval"].ToString();
                                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[0].Rows[i]["TextCode"].ToString();
                                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread5.Sheets[0].ColumnHeader.Cells[0, FpSpread5.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    checkarray.Add(ds1.Tables[0].Rows[i]["textval"].ToString());
                                }
                            }

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                {
                                    FpSpread5.Sheets[0].RowCount++;
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(count);
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].Font.Name = "Times New Roman";
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 1].Text = ddledu.SelectedItem.Text;
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 1].Font.Name = "Times New Roman";
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[k]["Course_Name"].ToString();
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].Font.Name = "Times New Roman";
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 2].Font.Bold = true;

                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[k]["dept_name"].ToString();
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[k]["degree_code"].ToString();
                                    hat.Add(ds.Tables[0].Rows[k]["degree_code"].ToString(), ds.Tables[0].Rows[k]["dept_name"].ToString());

                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Font.Name = "Times New Roman";
                                    FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                    count++;
                                    for (int i = 4; i < checkarray.Count + 4; i++)
                                    {
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, i].CellType = chkcel1;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, i].Value = 0;
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, i].Value = "0";
                                        FpSpread5.Sheets[0].Cells[FpSpread5.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    g++;

                                }

                            }

                            if (ds1.Tables[1].Rows.Count > 0)
                            {
                                if (FpSpread5.Sheets[0].RowCount > 0)
                                {
                                    DataView dv = new DataView();
                                    DataView dv1 = new DataView();
                                    for (int jk = 0; jk < FpSpread5.Sheets[0].RowCount; jk++)
                                    {
                                        string degree_code = Convert.ToString(FpSpread5.Sheets[0].Cells[jk, 3].Tag);
                                        ds1.Tables[1].DefaultView.RowFilter = "setcolumn=" + degree_code + "";
                                        dv = ds1.Tables[1].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            for (int ik = 4; ik < FpSpread5.Sheets[0].ColumnCount; ik++)
                                            {
                                                string columnname = Convert.ToString(FpSpread5.Sheets[0].ColumnHeader.Cells[0, ik].Tag);
                                                if (columnname.Trim() != "")
                                                {
                                                    ds1.Tables[1].DefaultView.RowFilter = "setcolumn=" + degree_code + " and column_name='" + columnname + "'";
                                                    dv1 = ds1.Tables[1].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        FpSpread5.Sheets[0].Cells[jk, ik].Value = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            FpSpread5.Sheets[0].PageSize = FpSpread5.Sheets[0].RowCount;

                        }
                    }
                }
                if (ddledu.SelectedItem.Text == "PG")
                {
                    FpSpread2.Visible = true;
                    FpSpread1.Visible = false;


                    chkcel1.AutoPostBack = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#393965");
                    darkstyle.ForeColor = System.Drawing.Color.White;
                    // darkstyle.Border = new FarPoint.Web.Spread.Border(Color.Crimson);
                    // Apply the new style.
                    FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FpSpread2.Visible = true;
                    FpSpread2.Sheets[0].RowCount = 0;
                    FpSpread2.Sheets[0].ColumnCount = 4;
                    FpSpread2.Sheets[0].RowHeader.Visible = false;
                    FpSpread2.Sheets[0].AutoPostBack = false;
                    FpSpread2.Height = 495;
                    FpSpread2.Width = 960;
                    FpSpread2.CommandBar.Visible = false;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].BackColor = Color.MistyRose;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Education Level";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].BackColor = Color.MistyRose;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Course";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].BackColor = Color.MistyRose;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                    //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].BackColor = Color.MistyRose;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                    string colquery = "select distinct branch_code,TextVal  from applyn a, Stud_prev_details s,TextValTable t where a.app_no =s.app_no  and s.branch_code=t.TextCode and a.degree_code in ('" + mainvalue1 + "') and a.college_code='" + college_code + "'  and batch_year =" + Convert.ToString(System.DateTime.Now.ToString("yyyy")) + " and TextVal<>'---Select---' order by branch_code asc  ";
                    colquery = colquery + " select * from admitcolumnset where textcriteria ='subjec' and college_code='" + Session["collegecode"].ToString() + "' and  setcolumn in('" + mainvalue1 + "')";
                    DataSet dsnew = new DataSet();
                    dsnew = dt.select_method_wo_parameter(colquery, "Text");
                    if (dsnew.Tables[0].Rows.Count > 0)
                    {
                        for (int col = 0; col < dsnew.Tables[0].Rows.Count; col++)
                        {
                            FpSpread2.Sheets[0].ColumnCount++;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dsnew.Tables[0].Rows[col]["TextVal"]);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dsnew.Tables[0].Rows[col]["branch_code"]);
                        }
                    }


                    FpSpread2.Sheets[0].Columns[0].Width = 40;
                    FpSpread2.Sheets[0].Columns[1].Width = 100;
                    FpSpread2.Sheets[0].Columns[2].Width = 100;
                    FpSpread2.Sheets[0].Columns[3].Width = 150;
                    FpSpread2.Sheets[0].Columns[1].Visible = false;
                    FpSpread2.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread2.Sheets[0].FrozenColumnCount = 4;

                    int count = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        {
                            count++;
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(count);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Times New Roman";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ddledu.SelectedItem.Text;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Times New Roman";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[k]["Course_Name"].ToString();
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Times New Roman";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Bold = true;

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[k]["dept_name"].ToString();
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[k]["degree_code"].ToString();
                            //hat.Add(ds.Tables[0].Rows[k]["degree_code"].ToString(), ds.Tables[0].Rows[k]["dept_name"].ToString());
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Times New Roman";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Bold = true;

                            for (int i = 4; i < dsnew.Tables[0].Rows.Count + 4; i++)
                            {
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, i].CellType = chkcel1;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, i].Value = 0;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, i].Value = "0";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }

                    if (dsnew.Tables[1].Rows.Count > 0)
                    {
                        if (FpSpread2.Sheets[0].RowCount > 0)
                        {
                            DataView dv = new DataView();
                            DataView dv1 = new DataView();
                            for (int jk = 0; jk < FpSpread2.Sheets[0].RowCount; jk++)
                            {
                                string degree_code = Convert.ToString(FpSpread2.Sheets[0].Cells[jk, 3].Tag);
                                dsnew.Tables[1].DefaultView.RowFilter = "setcolumn=" + degree_code + "";
                                dv = dsnew.Tables[1].DefaultView;
                                if (dv.Count > 0)
                                {
                                    for (int ik = 4; ik < FpSpread2.Sheets[0].ColumnCount; ik++)
                                    {
                                        string columnname = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, ik].Tag);
                                        if (columnname.Trim() != "")
                                        {
                                            dsnew.Tables[1].DefaultView.RowFilter = "setcolumn=" + degree_code + " and column_name='" + columnname + "'";
                                            dv1 = dsnew.Tables[1].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[jk, ik].Value = 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    FpSpread2.SaveChanges();

                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    FpSpread2.Visible = true;
                }


            }
            if (spvl == 4)
            {
                DataSet ds3 = new DataSet();

                string type = ddltype.SelectedItem.Text;
                string edulevel = ddledu.SelectedItem.Text;
                string concat = type + "(" + edulevel + ")";
                string concat1 = type + "-" + edulevel;
                selecttype.InnerHtml = concat;
                string mainvalue2 = "";
                txt_deprecom.Text = "";
                txt_mangquta.Text = "";

                string bindselectquery = "select value from Master_Settings where settings ='Departmentallocate" + concat1 + "' and usercode ='" + user_code + "'";
                bindselectquery = bindselectquery + " select value from Master_Settings where settings ='Managmentallocate" + concat1 + "' and usercode ='" + user_code + "'";
                ds.Clear();
                ds = dt.select_method_wo_parameter(bindselectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string departmentallocate = Convert.ToString(ds.Tables[0].Rows[0][0]);
                    if (departmentallocate.Trim() != "")
                    {
                        txt_deprecom.Text = Convert.ToString(departmentallocate);
                    }
                }
                else
                {
                    txt_deprecom.Text = "";
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    string managmentallocate = Convert.ToString(ds.Tables[1].Rows[0][0]);
                    if (managmentallocate.Trim() != "")
                    {
                        txt_mangquta.Text = Convert.ToString(managmentallocate);
                    }
                }
                else
                {
                    txt_mangquta.Text = "";
                }

                for (int j = 0; j < cblreligion.Items.Count; j++)
                {
                    if (cblreligion.Items[j].Selected == true)
                    {
                        string subvalue = cblreligion.Items[j].Value;
                        if (mainvalue2 == "")
                        {
                            mainvalue2 = subvalue;
                        }
                        else
                        {
                            mainvalue2 = mainvalue2 + "'" + "," + "'" + subvalue;
                        }
                    }

                }

                string mainvalue13 = "";
                for (int j = 0; j < cblcommunity.Items.Count; j++)
                {
                    if (cblcommunity.Items[j].Selected == true)
                    {
                        string subvalue = cblcommunity.Items[j].Value;
                        if (mainvalue13 == "")
                        {
                            mainvalue13 = subvalue;
                        }
                        else
                        {
                            mainvalue13 = mainvalue13 + "'" + "," + "'" + subvalue;
                        }
                    }

                }
                ds.Clear();
                if (mainvalue2.Trim() != "" || mainvalue13.Trim() != "")
                {
                    string religquery = "";

                    if (mainvalue2.Trim() != "")
                    {

                        religquery = "select TextVal,TextCode  from textvaltable where TextCode in ('" + mainvalue2 + "') and college_code =" + college_code + " order by TextVal ";

                        ds = dt.select_method_wo_parameter(religquery, "Text");
                    }
                    if (mainvalue13.Trim() != "")
                    {
                        string religquery1 = " select TextVal,TextCode  from textvaltable where TextCode in ('" + mainvalue13 + "') and college_code =" + college_code + " order by TextVal";
                        ds3.Clear();
                        ds3 = dt.select_method_wo_parameter(religquery1, "Text");
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        religrid.DataSource = ds.Tables[0];
                        religrid.DataBind();
                        religiondiv.Visible = true;
                    }
                    else
                    {
                        religiondiv.Visible = false;
                    }
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        gridcommunity.DataSource = ds3.Tables[0];
                        gridcommunity.DataBind();
                        communitydiv.Visible = true;
                    }
                    else
                    {
                        communitydiv.Visible = false;
                    }
                }
                else
                {
                    religiondiv.Visible = false;
                    communitydiv.Visible = false;
                }
                allocatereport();

            }


        }
        catch
        {

        }
    }

    public void spread()
    {
        try
        {

            string mainvalue = "";
            for (int j = 0; j < cheklist_Degree.Items.Count; j++)
            {
                if (cheklist_Degree.Items[j].Selected == true)
                {
                    string subvalue = cheklist_Degree.Items[j].Value;
                    if (mainvalue == "")
                    {
                        mainvalue = subvalue;
                    }
                    else
                    {
                        mainvalue = mainvalue + "'" + "," + "'" + subvalue;
                    }
                }

            }

            string mainvalue1 = "";
            for (int j = 0; j < cheklist_Branch.Items.Count; j++)
            {
                if (cheklist_Branch.Items[j].Selected == true)
                {
                    string subvalue = cheklist_Branch.Items[j].Value;
                    if (mainvalue1 == "")
                    {
                        mainvalue1 = subvalue;
                    }
                    else
                    {
                        mainvalue1 = mainvalue1 + "'" + "," + "'" + subvalue;
                    }
                }

            }


            string branchvl = "select distinct degree.degree_code,degree.No_Of_seats,department.dept_name,degree.Acronym,course.Course_Name  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + mainvalue + "') and degree.Degree_Code in('" + mainvalue1 + "') and deptprivilages.Degree_code=degree.Degree_code";
            ds = dt.select_method_wo_parameter(branchvl, "text");

            if (spvl == 0 || spvl == 1)
            {
                //spvl = 0;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = System.Drawing.Color.Brown;
                darkstyle.ForeColor = System.Drawing.Color.White;
                // darkstyle.Border = new FarPoint.Web.Spread.Border(Color.Crimson);
                // Apply the new style.
                FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread3.Visible = true;
                FpSpread3.Sheets[0].RowCount = 0;
                FpSpread3.Sheets[0].ColumnCount = 5;
                FpSpread3.Sheets[0].RowHeader.Visible = false;
                FpSpread3.Sheets[0].AutoPostBack = false;
                FpSpread3.Height = 550;
                FpSpread3.Width = 850;
                FpSpread3.CommandBar.Visible = false;
                FarPoint.Web.Spread.TextCellType chkcel1 = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.DoubleCellType intgrcell = new FarPoint.Web.Spread.DoubleCellType();
                intgrcell.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
                //  intgrcell.MaximumValue = Convert.ToInt32(100);
                intgrcell.MinimumValue = 0;
                intgrcell.ErrorMessage = "Enter valid Number";


                FpSpread3.Sheets[0].ColumnHeader.RowCount = 2;

                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                //  FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].BackColor = Color.MistyRose;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Education Level";
                FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                //   FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].BackColor = Color.MistyRose;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Course";
                FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                //FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].BackColor = Color.MistyRose;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                // FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].BackColor = Color.MistyRose;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total No Of Seat";
                FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                //  FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].BackColor = Color.MistyRose;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread3.Sheets[0].Columns[0].Width = 40;
                FpSpread3.Sheets[0].Columns[1].Width = 100;
                FpSpread3.Sheets[0].Columns[2].Width = 100;
                FpSpread3.Sheets[0].Columns[3].Width = 150;
                FpSpread3.Sheets[0].Columns[1].Visible = false;
                FpSpread3.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread3.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread3.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread3.Sheets[0].FrozenColumnCount = 5;

                string priority = "select * from textvaltable where TextCriteria='relig'  and college_code='" + college_code + "'  and textval!='' order by textval";
                priority = priority + " select * from textvaltable where TextCriteria='SCast' and college_code='" + college_code + "' and textval!='' order by textval";
                priority = priority + " select * from admitcolumnset where textcriteria ='relig' and college_code=" + college_code + " and priority<>0";
                ds1 = dt.select_method_wo_parameter(priority, "text");
                int count = 1;
                int g = 0;
                //int f = 0;
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        FpSpread3.Sheets[0].ColumnCount++;

                        if (ds1.Tables[0].Rows[i]["textval"].ToString() == "Christian")
                        {
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = ds1.Tables[0].Rows[i]["textval"].ToString();
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[0].Rows[i]["TextCode"].ToString();
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            // FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].CellType = intgrcell;
                            FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].Font.Name = "Times New Roman";
                            FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            if (ds1.Tables[1].Rows.Count > 0)
                            {

                                for (int j = 0; j < ds1.Tables[1].Rows.Count; j++)
                                {
                                    FpSpread3.Sheets[0].ColumnHeader.Cells[1, FpSpread3.Sheets[0].ColumnCount - 1].Text = ds1.Tables[1].Rows[j]["textval"].ToString();
                                    FpSpread3.Sheets[0].ColumnHeader.Cells[1, FpSpread3.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[1].Rows[j]["TextCode"].ToString();
                                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[0].Rows[i]["TextCode"].ToString();
                                    FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = ds1.Tables[0].Rows[i]["textval"].ToString();
                                    // FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                    FpSpread3.Sheets[0].ColumnHeader.Cells[1, FpSpread3.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread3.Sheets[0].ColumnHeader.Cells[1, FpSpread3.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    FpSpread3.Sheets[0].ColumnHeader.Cells[1, FpSpread3.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].CellType = intgrcell;
                                    FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread3.Sheets[0].ColumnCount++;
                                }
                                FpSpread3.Sheets[0].ColumnCount--;

                            }
                            FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread3.Sheets[0].ColumnCount - ds1.Tables[1].Rows.Count, 1, ds1.Tables[1].Rows.Count);

                        }
                        else
                        {

                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = ds1.Tables[0].Rows[i]["textval"].ToString();
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[0].Rows[i]["TextCode"].ToString();
                            FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread3.Sheets[0].ColumnCount - 1, 2, 1);
                            // FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].CellType = intgrcell;
                            FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].Font.Name = "Times New Roman";
                            FpSpread3.Sheets[0].Columns[FpSpread3.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //FpSpread3.Sheets[0].SpanModel.Add(FpSpread3.Sheets[0].RowCount - 1, 1, ds.Tables[0].Rows.Count, 1);
                        //FpSpread3.Sheets[0].SpanModel.Add(f, 2, 2, 1);

                        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        {

                            FpSpread3.Sheets[0].RowCount++;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(count);
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Font.Name = "Times New Roman";
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = ddledu.SelectedItem.Text;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Name = "Times New Roman";
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[k]["Course_Name"].ToString();
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Name = "Times New Roman";
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[k]["dept_name"].ToString();
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[k]["degree_code"].ToString();
                            hat.Add(ds.Tables[0].Rows[k]["degree_code"].ToString(), ds.Tables[0].Rows[k]["dept_name"].ToString());
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Name = "Times New Roman";
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[k]["No_Of_seats"].ToString();

                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Name = "Times New Roman";
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            count++;

                            //if (g % 2 == 0)
                            //{
                            //    FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].BackColor = Color.MintCream;
                            //}
                            //else
                            //{
                            //    FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].BackColor = Color.Lavender;
                            //}

                            for (int i1 = 0; i1 < FpSpread3.Sheets[0].Rows.Count; i1++)
                            {
                                string vall = FpSpread3.Sheets[0].Cells[i1, 4].Text;
                                FarPoint.Web.Spread.DoubleCellType intgrcell1 = new FarPoint.Web.Spread.DoubleCellType();
                                intgrcell1.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
                                intgrcell1.MaximumValue = Convert.ToInt32(vall);

                                intgrcell1.MinimumValue = 0;
                                intgrcell1.ErrorMessage = "Enter valid Number";
                                for (int i = 5; i < FpSpread3.Sheets[0].Columns.Count; i++)
                                {
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, i].CellType = intgrcell1;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, i].Text = "";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, i].Font.Size = FontUnit.Medium;
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, i].Font.Name = "Times New Roman";
                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            g++;

                        }

                        if (ds1.Tables[2].Rows.Count > 0)
                        {
                            if (FpSpread3.Sheets[0].RowCount > 0)
                            {
                                DataView dv = new DataView();
                                DataView dv1 = new DataView();
                                for (int jk = 0; jk < FpSpread3.Sheets[0].RowCount; jk++)
                                {
                                    string degree_code = Convert.ToString(FpSpread3.Sheets[0].Cells[jk, 3].Tag);
                                    ds1.Tables[2].DefaultView.RowFilter = "setcolumn=" + degree_code + "";
                                    dv = ds1.Tables[2].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        for (int ik = 5; ik < FpSpread3.Sheets[0].ColumnCount; ik++)
                                        {
                                            string columnname = FpSpread3.Sheets[0].ColumnHeader.Cells[0, ik].Text;
                                            string tagvalue = "";
                                            string subtag = "";
                                            string maintag = "";
                                            if (columnname == "Christian")
                                            {
                                                tagvalue = Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[0, ik].Tag);
                                                subtag = Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[1, ik].Tag);
                                                maintag = tagvalue + "-" + subtag;
                                            }
                                            else
                                            {
                                                maintag = Convert.ToString(FpSpread3.Sheets[0].ColumnHeader.Cells[0, ik].Tag);
                                            }

                                            if (maintag.Trim() != "")
                                            {
                                                ds1.Tables[2].DefaultView.RowFilter = "setcolumn=" + degree_code + " and column_name='" + maintag + "'";
                                                dv1 = ds1.Tables[2].DefaultView;
                                                if (dv1.Count > 0)
                                                {
                                                    string alot_Value = dv1[0]["priority"].ToString();
                                                    FpSpread3.Sheets[0].Cells[jk, ik].Text = alot_Value;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }

                    }


                    FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                }
            }
            if (spvl == 2)
            {
                //spvl = 0;
                chkcel1.AutoPostBack = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = System.Drawing.Color.Brown;
                darkstyle.ForeColor = System.Drawing.Color.White;
                // darkstyle.Border = new FarPoint.Web.Spread.Border(Color.Crimson);
                // Apply the new style.
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread1.Visible = true;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 4;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Height = 550;
                FpSpread1.Width = 960;
                FpSpread1.CommandBar.Visible = false;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].BackColor = Color.MistyRose;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Education Level";
                //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].BackColor = Color.MistyRose;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Course";
                //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].BackColor = Color.MistyRose;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].BackColor = Color.MistyRose;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[0].Width = 40;
                FpSpread1.Sheets[0].Columns[1].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Width = 100;
                FpSpread1.Sheets[0].Columns[3].Width = 150;
                FpSpread1.Sheets[0].Columns[1].Visible = false;
                FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].FrozenColumnCount = 4;
                string priority = "select * from textvaltable where TextCriteria='subje' and college_code='" + college_code + "' and textval!=''";
                priority = priority + " select * from admitcolumnset where textcriteria ='subjec' and college_code=13";
                ds1 = dt.select_method_wo_parameter(priority, "text");
                int count = 1;
                int g = 0;
                //int f = 0;
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {

                        FpSpread1.Sheets[0].ColumnCount++;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = ds1.Tables[0].Rows[i]["textval"].ToString();
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = ds1.Tables[0].Rows[i]["TextCode"].ToString();
                        //  FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(count);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Times New Roman";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ddledu.SelectedItem.Text;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Times New Roman";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[k]["Course_Name"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Times New Roman";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[k]["dept_name"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[k]["degree_code"].ToString();
                            hat.Add(ds.Tables[0].Rows[k]["degree_code"].ToString(), ds.Tables[0].Rows[k]["dept_name"].ToString());

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Times New Roman";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            count++;
                            for (int i = 4; i < ds1.Tables[0].Rows.Count + 4; i++)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].CellType = chkcel1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].Value = 0;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].Value = "0";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                            }
                            g++;

                        }

                    }

                    if (ds1.Tables[1].Rows.Count > 0)
                    {
                        if (FpSpread1.Sheets[0].RowCount > 0)
                        {
                            DataView dv = new DataView();
                            DataView dv1 = new DataView();
                            for (int jk = 0; jk < FpSpread1.Sheets[0].RowCount; jk++)
                            {
                                string degree_code = Convert.ToString(FpSpread1.Sheets[0].Cells[jk, 3].Tag);
                                ds1.Tables[1].DefaultView.RowFilter = "setcolumn=" + degree_code + "";
                                dv = ds1.Tables[1].DefaultView;
                                if (dv.Count > 0)
                                {
                                    for (int ik = 4; ik < FpSpread1.Sheets[0].ColumnCount; ik++)
                                    {
                                        string columnname = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, ik].Tag);
                                        if (columnname.Trim() != "")
                                        {
                                            ds1.Tables[1].DefaultView.RowFilter = "setcolumn=" + degree_code + " and column_name='" + columnname + "'";
                                            dv1 = ds1.Tables[1].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                FpSpread1.Sheets[0].Cells[jk, ik].Value = 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;


                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Logout_btn_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    protected void religiondataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int count = e.Row.Cells.Count;
            int rowvalue = e.Row.RowIndex;
            string id = e.Row.ClientID;
            e.Row.Cells[1].ForeColor = System.Drawing.Color.Green;
            e.Row.Cells[1].Font.Bold = true;
            e.Row.Cells[2].ForeColor = System.Drawing.Color.Blue;
            e.Row.Cells[0].ForeColor = System.Drawing.Color.Black;
            e.Row.Cells[0].Font.Bold = true;
            e.Row.Cells[0].CssClass = "txe";
            e.Row.Cells[1].CssClass = "txe";
            e.Row.Height = 50;
            e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
            Label countlable = new Label();
            countlable.Width = 100;
            countlable.Font.Bold = true;
            countlable.Text = e.Row.Cells[4].Text;
            countlable.CssClass = "txe";
            e.Row.Cells[4].Controls.Add(countlable);
            //e.Row.Cells[4].Font.Bold = true;
            //e.Row.Cells[4].Width = 100;

            // txtCountry.Text = (e.Row.DataItem as DataRowView).Row["Country"].ToString();
            if (count > 0)
            {
                ArrayList columsvalesnew = new ArrayList();
                ArrayList rowsvalesnew = new ArrayList();
                columsvalesnew = (ArrayList)ViewState["Columnvalues"];
                rowsvalesnew = (ArrayList)ViewState["Rowsvalues"];

                //FilteredTextBoxExtender txt = new FilteredTextBoxExtender();
                //txt.FilterType = FilterTypes.UppercaseLetters;
                int add = 0;
                for (int i = 5; i < count; i++)
                {

                    string columnsvalue1 = Convert.ToString(columsvalesnew[add]);
                    add++;
                    string rowsvalues = Convert.ToString(rowsvalesnew[rowvalue]);
                    string selectquery = "select priority  from admitcolumnset where textcriteria ='relig' and college_code=" + college_code + " and priority<>0 and setcolumn='" + rowsvalues + "' and column_name ='" + columnsvalue1 + "'";
                    ds.Clear();
                    ds = dt.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string value = Convert.ToString(ds.Tables[0].Rows[0]["priority"]);
                        if (value.Trim() != "")
                        {
                            TextBox txtCountry = new TextBox();
                            txtCountry.ID = "txtcoummunity" + i;
                            txtCountry.Width = 80;
                            txtCountry.Height = 20;
                            txtCountry.CssClass = "textbox textbox1 txe";
                            txtCountry.MaxLength = 3;
                            txtCountry.Text = value.ToString();
                            //txt.TargetControlID = txtCountry.ID;
                            //txtCountry.pattern = "^[0-9]*$";
                            e.Row.Cells[i].Controls.Add(txtCountry);
                            //  TextBox txt1 = (TextBox)e.Row.FindControl("txtcoummunity");
                            txtCountry.Attributes.Add("onblur", "javascript:get('" + txtCountry.ClientID + "','" + i + "','" + rowvalue + "','" + countlable.Text + "','" + id + "')");
                            txtCountry.Attributes.Add("onkeyup", "javascript:checkvalue('" + txtCountry.ClientID + "')");
                        }
                        else
                        {
                            TextBox txtCountry = new TextBox();
                            txtCountry.ID = "txtcoummunity" + i;
                            txtCountry.Width = 80;
                            txtCountry.Height = 20;
                            txtCountry.CssClass = "textbox textbox1";
                            txtCountry.MaxLength = 3;
                            txtCountry.Text = "";
                            //txt.TargetControlID = txtCountry.ID;
                            //txtCountry.pattern = "^[0-9]*$";
                            e.Row.Cells[i].Controls.Add(txtCountry);
                            //  TextBox txt1 = (TextBox)e.Row.FindControl("txtcoummunity");
                            txtCountry.Attributes.Add("onblur", "javascript:get('" + txtCountry.ClientID + "','" + i + "','" + rowvalue + "','" + countlable.Text + "','" + id + "')");
                            txtCountry.Attributes.Add("onkeyup", "javascript:checkvalue('" + txtCountry.ClientID + "')");
                        }
                    }
                    else
                    {
                        TextBox txtCountry = new TextBox();
                        txtCountry.ID = "txtcoummunity" + i;
                        txtCountry.Width = 80;
                        txtCountry.Height = 20;
                        txtCountry.CssClass = "textbox textbox1";
                        txtCountry.MaxLength = 3;
                        txtCountry.Text = "";
                        //txt.TargetControlID = txtCountry.ID;
                        //txtCountry.pattern = "^[0-9]*$";
                        e.Row.Cells[i].Controls.Add(txtCountry);
                        //  TextBox txt1 = (TextBox)e.Row.FindControl("txtcoummunity");
                        txtCountry.Attributes.Add("onblur", "javascript:get('" + txtCountry.ClientID + "','" + i + "','" + rowvalue + "','" + countlable.Text + "','" + id + "')");
                        txtCountry.Attributes.Add("onkeyup", "javascript:checkvalue('" + txtCountry.ClientID + "')");
                    }

                }
            }


        }
        if (e.Row.RowType == DataControlRowType.Header)
        {

            //int count = e.Row.Cells.Count;
            //GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
            //HeaderRow.BorderColor = Color.Black;
            //TableCell HeaderCell2 = new TableCell();
            //HeaderCell2.Text = "";
            //HeaderCell2.ColumnSpan = 6;

            //TableCell HeaderCell3 = new TableCell();
            //HeaderCell3.Text = "Christian";
            //HeaderCell3.Font.Size = FontUnit.Medium;
            //HeaderCell3.Font.Bold = true;
            //HeaderCell3.ForeColor = Color.White;
            //HeaderCell3.CssClass = "txe";
            //HeaderCell3.ColumnSpan = count - 6;
            //HeaderRow.Cells.Add(HeaderCell2);
            //HeaderRow.Cells.Add(HeaderCell3);

            //religiongrid.Controls[0].Controls.AddAt(0, HeaderRow);






        }



    }

    protected void OnDataBound(object sender, EventArgs e)
    {
        //for (int i = religiongrid.Rows.Count - 1; i > 0; i--)
        //{
        //    GridViewRow row = religiongrid.Rows[i];
        //    GridViewRow previousRow = religiongrid.Rows[i - 1];
        //    for (int j = 1; j <= 1; j++)
        //    {
        //        //Label lnlname = (Label)row.FindControl("lblcoursename");
        //        // Label lnlname1 = (Label)previousRow.FindControl("lblcoursename");
        //        string firstvalue = row.Cells[1].Text;
        //        string secondvalue = previousRow.Cells[1].Text;

        //        if (firstvalue == secondvalue)
        //        {
        //            if (previousRow.Cells[j].RowSpan == 0)
        //            {
        //                if (row.Cells[j].RowSpan == 0)
        //                {
        //                    previousRow.Cells[j].RowSpan += 2;
        //                }
        //                else
        //                {
        //                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
        //                }
        //                row.Cells[j].Visible = false;
        //            }
        //        }
        //    }
        //}


    }

    protected void FpSpread3_command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            //  step6.Visible = true;
            string last = e.CommandArgument.ToString();
            bool flage = false;
            string activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
            string fvl = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
            int deg = Convert.ToInt32(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);


            //string dd = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].CellType.GetHashCode());
            if (last == activerow)
            {
                if (Convert.ToInt32(activecol) > 4)
                {
                    string seltext = e.EditValues[Convert.ToInt32(activecol)].ToString();
                    if (seltext.Trim() != "" && seltext != null)
                    {
                        if (Convert.ToInt32(fvl) < Convert.ToInt32(seltext))
                        {
                            flage = true;

                            // mpemsgboxdelete.Show();   
                            e.Handled = true;
                            FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text = "";
                            e.Handled = true;
                            // FpSpread3.SaveChanges();
                            // FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text = "";
                            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Generate Code\");", true);
                            //  ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"State Required\");", true);
                        }
                        else
                        {
                            if (hashcheck.ContainsKey(deg) == true)
                            {
                                int pvalue = Convert.ToInt32(hashcheck[deg]);
                                int newvalue = pvalue + Convert.ToInt32(seltext);
                                hashcheck.Remove(deg);
                                if (newvalue > Convert.ToInt32(fvl))
                                {
                                    e.Handled = true;
                                    FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text = "";
                                    e.Handled = true;
                                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Generate Code\");", true);
                                    // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The Enter leave is not available for this HR year')", true);
                                }
                                else
                                {
                                    hashcheck.Add(deg, newvalue);
                                }
                            }
                            else
                            {

                                hashcheck.Add(deg, seltext);
                            }
                        }
                    }

                }
            }



        }
        catch (Exception ex)
        {

        }
    }

    public void save()
    {
        string activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
        //  intgrcell1.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
        ////  intgrcell1.MaximumValue = Convert.ToInt32(vall);

        //  intgrcell1.MinimumValue = 0;
        //  intgrcell1.ErrorMessage = "Enter valid Number ghhh";
        //  //for (int i = 5; i < FpSpread3.Sheets[0].Columns.Count; i++)
        //  //{
        //      FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].CellType = intgrcell1;
        FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text = "dsfsadf";

        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, i].Font.Size = FontUnit.Medium;
        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, i].Font.Name = "Times New Roman";
        //FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
        // }
        return;
        //FpSpread3.SaveChanges();
    }

    protected void btn1_click(object sender, EventArgs e)
    {
        val = 0;
        panel7.Visible = false;
    }

    protected void FpSpread6_command(object sender, EventArgs e)
    {
        try
        {
            int isval1 = 0;
            string activerow = FpSpread6.ActiveSheetView.ActiveRow.ToString();
            string text = (FpSpread6.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
            isval1 = Convert.ToInt32(FpSpread6.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Value);

            if (FpSpread7.Sheets[0].Rows.Count > 0)
            {
                val = Convert.ToInt32(FpSpread7.Sheets[0].Rows.Count);

            }

            if (isval1 == 1)
            {
                val = val + 1;
                FpSpread6.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text = val.ToString();
                FpSpread6.Sheets[0].Cells[Convert.ToInt32(activerow), 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread6.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Font.Name = "Times New Roman";
                FpSpread6.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Locked = true;
                FpSpread7.Sheets[0].RowCount++;
                FpSpread7.Sheets[0].Cells[FpSpread7.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread7.Sheets[0].RowCount);
                FpSpread7.Sheets[0].Cells[FpSpread7.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread7.Sheets[0].Cells[FpSpread7.Sheets[0].RowCount - 1, 0].Font.Name = "Times New Roman";
                FpSpread7.Sheets[0].Cells[FpSpread7.Sheets[0].RowCount - 1, 1].Text = text;
                FpSpread7.Sheets[0].Cells[FpSpread7.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread7.Sheets[0].Cells[FpSpread7.Sheets[0].RowCount - 1, 1].Font.Name = "Times New Roman";
            }
            else if (isval1 == 0)
            {
                val = val - 1;
                FpSpread7.Sheets[0].RowCount--;
                FpSpread6.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text = "";
                FpSpread7.Sheets[0].Cells[Convert.ToInt32(activerow), 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread7.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Font.Name = "Times New Roman";
            }
            FpSpread7.Visible = true;
            FpSpread7.Sheets[0].PageSize = FpSpread7.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {

        }
    }

    public void selectcolumn()
    {
        FpSpread7.Sheets[0].RowCount = 0;
        FpSpread7.Sheets[0].ColumnCount = 2;
        FpSpread7.Sheets[0].RowHeader.Visible = false;
        FpSpread7.Sheets[0].AutoPostBack = true;
        FpSpread7.CommandBar.Visible = false;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#393965");
        darkstyle.ForeColor = System.Drawing.Color.White;
        // darkstyle.Border = new FarPoint.Web.Spread.Border(Color.Crimson);
        // Apply the new style.
        FpSpread7.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        FpSpread7.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread7.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        FpSpread7.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        FpSpread7.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        FpSpread7.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Selected Column";
        FpSpread7.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        FpSpread7.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        FpSpread7.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        FpSpread7.Sheets[0].Columns[0].Width = 50;
        FpSpread7.Sheets[0].Columns[1].Width = 200;
    }

    public void column()
    {
        try
        {
            ArrayList columnarray = new ArrayList();
            columnarray.Add("Student Name-stud_name");
            columnarray.Add("DOB-dob");
            columnarray.Add("Application Date-date_applied");
            columnarray.Add("Address-parent_addressP");
            columnarray.Add("Mobile No-Student_Mobile");
            columnarray.Add("Email_Id-StuPer_Id");
            columnarray.Add("Alternative Course-Alternativedegree_code");
            columnarray.Add("Gender-sex");
            columnarray.Add("Parent Name-parent_name");
            columnarray.Add("Religion-religion");
            columnarray.Add("Community-community");
            columnarray.Add("Caste-caste");
            columnarray.Add("Nationality-citizen");
            columnarray.Add("Occupation-parent_occu");
            columnarray.Add("Remarks-remarks");
            columnarray.Add("Application ID-app_formno");
            columnarray.Add("Batch Year-Batch_Year");
            columnarray.Add("Course-Course_Name");
            columnarray.Add("Department-Dept_Name");
            columnarray.Add("Semester-Current_Semester");
            columnarray.Add("Institute Name-Institute_Name");
            columnarray.Add("Institute Address-instaddress");
            columnarray.Add("Pass Month-PassMonth");
            columnarray.Add("Pass Year-PassYear");
            columnarray.Add("Marks-securedmark");
            columnarray.Add("Total Percentage-percentage");
            columnarray.Add("State-parent_statep");
            columnarray.Add("Mother Tongue-mother_tongue");
            columnarray.Add("TANCET Mark-tancet_mark");
            columnarray.Add("Island-TamilOrginFromAndaman");
            columnarray.Add("Ex serviceman-IsExService");
            columnarray.Add("Differently abled-isdisable");
            columnarray.Add("First generation-first_graduate");
            columnarray.Add("Sports-DistinctSport");
            columnarray.Add("Co Curricular Activites-co_curricular");
            columnarray.Add("BankReferenceNo-ApplBankRefNumber");
            columnarray.Add("BankReferenceDate-applbankrefdate");
            columnarray.Add("Vocational-vocational_stream");
            columnarray.Add("TotalFess-totalfees");
            columnarray.Add("Paid-PaidAmount");
            columnarray.Add("NoOfAttempt-noofattempts");

            FpSpread6.Visible = true;
            FpSpread6.Sheets[0].RowCount = 0;
            FpSpread6.Sheets[0].ColumnCount = 5;
            FpSpread6.Sheets[0].RowHeader.Visible = false;
            FpSpread6.Sheets[0].AutoPostBack = false;
            FpSpread6.Height = 450;
            FpSpread6.Width = 450;
            FpSpread6.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#393965");
            darkstyle.ForeColor = System.Drawing.Color.White;
            // darkstyle.Border = new FarPoint.Web.Spread.Border(Color.Crimson);
            // Apply the new style.
            FpSpread6.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.CheckBoxCellType chkcel1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Column";
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Column1";
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread6.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread6.Sheets[0].Columns[0].Width = 50;
            FpSpread6.Sheets[0].Columns[1].Width = 270;
            FpSpread6.Sheets[0].Columns[2].Width = 110;
            FpSpread6.Sheets[0].Columns[3].Width = 50;
            FpSpread6.Sheets[0].Columns[4].Width = 60;
            FpSpread6.Sheets[0].Columns[2].Visible = false;
            chkcel1.AutoPostBack = true;
            if (columnarray.Count > 0)
            {
                int con = 0;
                for (int i = 0; i < columnarray.Count; i++)
                {
                    con++;
                    FpSpread6.Sheets[0].RowCount++;
                    string column_value = columnarray[i].ToString();
                    string[] split_columnvalue = column_value.Split('-');
                    if (split_columnvalue.Length > 0)
                    {
                        FpSpread6.Sheets[0].Cells[i, 0].Text = Convert.ToString(con);
                        FpSpread6.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                        FpSpread6.Sheets[0].Cells[i, 0].Font.Bold = true;
                        FpSpread6.Sheets[0].Cells[i, 0].Font.Size = FontUnit.Medium;
                        FpSpread6.Sheets[0].Cells[i, 0].Locked = true;
                        FpSpread6.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread6.Sheets[0].Cells[i, 1].Text = Convert.ToString(split_columnvalue[0]);
                        FpSpread6.Sheets[0].Cells[i, 1].Font.Name = "Book Antiqua";
                        FpSpread6.Sheets[0].Cells[i, 1].Font.Bold = true;
                        FpSpread6.Sheets[0].Cells[i, 1].Font.Size = FontUnit.Medium;
                        FpSpread6.Sheets[0].Cells[i, 1].Locked = true;

                        FpSpread6.Sheets[0].Cells[i, 2].Text = Convert.ToString(split_columnvalue[1]);
                        FpSpread6.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                        FpSpread6.Sheets[0].Cells[i, 2].Font.Bold = true;
                        FpSpread6.Sheets[0].Cells[i, 2].Font.Size = FontUnit.Medium;
                        FpSpread6.Sheets[0].Cells[i, 2].Locked = true;

                        FpSpread6.Sheets[0].Cells[i, 3].CellType = chkcel1;
                        FpSpread6.Sheets[0].Cells[i, 3].Font.Name = "Book Antiqua";
                        FpSpread6.Sheets[0].Cells[i, 3].Font.Bold = true;
                        FpSpread6.Sheets[0].Cells[i, 3].Font.Size = FontUnit.Medium;
                        FpSpread6.Sheets[0].Cells[i, 3].Value = false;


                        FpSpread6.Sheets[0].Cells[i, 4].Text = "";
                        FpSpread6.Sheets[0].Cells[i, 4].Font.Name = "Book Antiqua";
                        FpSpread6.Sheets[0].Cells[i, 4].Font.Bold = true;
                        FpSpread6.Sheets[0].Cells[i, 4].Font.Size = FontUnit.Medium;
                        FpSpread6.Sheets[0].Cells[i, 4].Locked = true;
                        FpSpread6.Sheets[0].Cells[i, 4].HorizontalAlign = HorizontalAlign.Center;
                    }

                }
                FpSpread6.Sheets[0].PageSize = FpSpread6.Sheets[0].RowCount;
            }

        }
        catch (Exception ex)
        {

        }
    }

    public void selectcolumnfunction()
    {
        try
        {
            DataView dv1 = new DataView();
            FpSpread7.Sheets[0].RowCount = 0;
            string selectquery = "select * from admitcolumnset where textcriteria ='column' and college_code ='" + college_code + "' and user_code ='" + user_code + "' order by priority";
            ds.Clear();
            ds = dt.select_method_wo_parameter(selectquery, "Text");
            FpSpread7.Sheets[0].RowCount = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                int co = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    co++;
                    FpSpread7.Sheets[0].RowCount++;
                    FpSpread7.Sheets[0].Cells[i, 0].Text = Convert.ToString(co);
                    FpSpread7.Sheets[0].Cells[i, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["setcolumn"]);
                    FpSpread7.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Left;

                }
                if (FpSpread6.Sheets[0].Rows.Count > 0)
                {
                    for (int j = 0; j < FpSpread6.Sheets[0].Rows.Count; j++)
                    {
                        string headertext = FpSpread6.Sheets[0].Cells[j, 1].Text;
                        ds.Tables[0].DefaultView.RowFilter = "setcolumn='" + headertext + "'";
                        dv1 = ds.Tables[0].DefaultView;
                        if (dv1.Count > 0)
                        {
                            string text_priority = Convert.ToString(dv1[0]["priority"]);
                            if (text_priority.Trim() != "")
                            {
                                FpSpread6.Sheets[0].Cells[j, 3].Value = true;
                                FpSpread6.Sheets[0].Cells[j, 3].Locked = true;
                                FpSpread6.Sheets[0].Cells[j, 4].Text = text_priority;
                            }
                        }

                    }
                }
            }
            if (FpSpread7.Sheets[0].RowCount > 0)
            {
                FpSpread7.Visible = true;
                FpSpread7.Sheets[0].PageSize = FpSpread7.Sheets[0].RowCount;
            }
            else
            {
                FpSpread7.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void cbreligion_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbreligion.Checked == true)
            {
                for (int i = 0; i < cblreligion.Items.Count; i++)
                {

                    cblreligion.Items[i].Selected = true;
                    txt_Religion.Text = "Religion(" + (cblreligion.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cblreligion.Items.Count; i++)
                {
                    cblreligion.Items[i].Selected = false;
                    txt_Religion.Text = "--Select--";
                }
            }
        }
        catch
        {

        }
    }

    protected void cblreligion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cbreligion.Checked = false;
            for (int i = 0; i < cblreligion.Items.Count; i++)
            {
                if (cblreligion.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }

            txt_Religion.Text = "Religion(" + seatcount.ToString() + ")";

        }
        catch
        {

        }
    }

    protected void cbcommunity_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbcommunity.Checked == true)
            {
                for (int i = 0; i < cblcommunity.Items.Count; i++)
                {

                    cblcommunity.Items[i].Selected = true;
                    txt_Community.Text = "Community(" + (cblcommunity.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cblcommunity.Items.Count; i++)
                {
                    cblcommunity.Items[i].Selected = false;
                    txt_Community.Text = "--Select--";
                }
            }
        }
        catch
        {

        }
    }

    protected void cblcommunity_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cbcommunity.Checked = false;
            for (int i = 0; i < cblcommunity.Items.Count; i++)
            {
                if (cblcommunity.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            txt_Community.Text = "Religion(" + seatcount.ToString() + ")";

        }
        catch
        {

        }
    }

    protected void religrid_Onrowdatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int count = e.Row.Cells.Count;
                string type = ddltype.SelectedItem.Text;
                string level = ddledu.SelectedItem.Text;
                string compare = type + "-" + level;
                int rowvalue = e.Row.RowIndex;
                string id = e.Row.ClientID;
                string value = (e.Row.FindControl("lblreligcode") as Label).Text;
                string selectquery = "select priority  from admitcolumnset where column_name='" + value + "' and setcolumn ='" + compare + "' and textcriteria ='relig' and user_code ='" + user_code + "'  and college_code ='" + college_code + "'";
                ds1 = dt.select_method_wo_parameter(selectquery, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    string priority = Convert.ToString(ds1.Tables[0].Rows[0]["priority"]);
                    if (priority != "")
                    {
                        (e.Row.FindControl("txt_percentageornumber") as TextBox).Text = priority;
                    }
                }
            }

        }
        catch
        {
        }
    }

    protected void gridcommunity_Onrowdatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int count = e.Row.Cells.Count;
                string type = ddltype.SelectedItem.Text;
                string level = ddledu.SelectedItem.Text;
                string compare = type + "-" + level;
                int rowvalue = e.Row.RowIndex;
                string id = e.Row.ClientID;
                string value = (e.Row.FindControl("lblcommunitycode") as Label).Text;
                string selectquery = "select priority  from admitcolumnset where column_name='" + value + "' and setcolumn ='" + compare + "' and textcriteria ='community' and user_code ='" + user_code + "'  and college_code ='" + college_code + "'";
                ds1 = dt.select_method_wo_parameter(selectquery, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    string priority = Convert.ToString(ds1.Tables[0].Rows[0]["priority"]);
                    if (priority != "")
                    {
                        (e.Row.FindControl("txt_compercent") as TextBox).Text = priority;
                    }
                }
            }
        }
        catch
        {
        }

    }

    public void allocatereport()
    {
        try
        {
            string mainvalue = "";
            for (int j = 0; j < cheklist_Degree.Items.Count; j++)
            {
                if (cheklist_Degree.Items[j].Selected == true)
                {
                    string subvalue = cheklist_Degree.Items[j].Value;
                    if (mainvalue == "")
                    {
                        mainvalue = subvalue;
                    }
                    else
                    {
                        mainvalue = mainvalue + "'" + "," + "'" + subvalue;
                    }
                }

            }

            string mainvalue1 = "";
            for (int j = 0; j < cheklist_Branch.Items.Count; j++)
            {
                if (cheklist_Branch.Items[j].Selected == true)
                {
                    string subvalue = cheklist_Branch.Items[j].Value;
                    if (mainvalue1 == "")
                    {
                        mainvalue1 = subvalue;
                    }
                    else
                    {
                        mainvalue1 = mainvalue1 + "'" + "," + "'" + subvalue;
                    }
                }

            }
            string type = ddltype.SelectedItem.Text;
            string level = ddledu.SelectedItem.Text;
            string concate = type + "-" + level;
            int totalvalue = 0;
            double percentvalue = 0;
            ArrayList addarray = new ArrayList();
            ArrayList addreligarray = new ArrayList();
            ArrayList countcolum = new ArrayList();
            ArrayList countcolum1 = new ArrayList();
            ArrayList department_code = new ArrayList();
            Hashtable hashcount = new Hashtable();
            Hashtable hashcount1 = new Hashtable();
            Hashtable comm_value = new Hashtable();

            DataTable da = new DataTable();
            string updatequery = "";
            string updatequery1 = "";
            string allocate = "Departmentallocate" + concate + "";
            string mnagallocate = "Managmentallocate" + concate + "";
            if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                updatequery = " select (value+' %') as total, value from Master_Settings where group_code ='" + group_user + "' and settings ='" + allocate + "' ";
                updatequery1 = " select (value) as total, value from Master_Settings where group_code ='" + group_user + "' and settings ='" + mnagallocate + "' ";

            }
            else
            {
                updatequery = " select (value+' %') as total, value from Master_Settings where usercode ='" + user_code + "' and settings ='" + allocate + "'";
                updatequery1 = " select (value) as total, value from Master_Settings where usercode ='" + user_code + "' and settings ='" + mnagallocate + "' ";

            }

            string selectqury = "select distinct degree.degree_code,degree.No_Of_seats,department.dept_name,degree.Acronym,course.Course_Name  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + mainvalue + "') and degree.Degree_Code in('" + mainvalue1 + "') and deptprivilages.Degree_code=degree.Degree_code";
            selectqury = selectqury + " select column_name ,(Textval +' '+ CONVERT(varchar, priority)+' %') as value , priority,TextCode,Textval from admitcolumnset a,textvaltable t where a.column_name=t.TextCode and a.college_code =t.college_code and a.textcriteria ='relig' and a.college_code=" + college_code + " and user_code=" + user_code + " and setcolumn='" + concate + "'";
            selectqury = selectqury + updatequery;
            selectqury = selectqury + " select column_name ,(Textval +' '+ CONVERT(varchar, priority)+' %') as value , priority,Textval,TextCode from admitcolumnset a,textvaltable t where a.column_name=t.TextCode and a.college_code =t.college_code and a.textcriteria ='community' and a.college_code=" + college_code + " and user_code=" + user_code + " and setcolumn='" + concate + "' order by t.Textval";
            selectqury = selectqury + updatequery1;
            ds.Clear();
            ds = dt.select_method_wo_parameter(selectqury, "Text");
            if (ds.Tables[1].Rows.Count > 0)
            {
                da.Columns.Add("S.No", typeof(string));
                da.Columns.Add("Department", typeof(string));
                da.Columns.Add("SANCTIONED STRENGTH ", typeof(string));
                if (ds.Tables[2].Rows.Count > 0)
                {
                    da.Columns.Add(Convert.ToString(ds.Tables[2].Rows[0]["total"]));
                    totalvalue = Convert.ToInt32(ds.Tables[2].Rows[0]["value"]);
                }
                for (int jr = 0; jr < ds.Tables[1].Rows.Count; jr++)
                {
                    da.Columns.Add(Convert.ToString(ds.Tables[1].Rows[jr]["value"]));
                    addreligarray.Add(Convert.ToString(ds.Tables[1].Rows[jr]["priority"]));
                    countcolum1.Add(Convert.ToString(ds.Tables[1].Rows[jr]["Textval"]));
                    hashcount.Add(Convert.ToString(ds.Tables[1].Rows[jr]["Textval"]), Convert.ToString(ds.Tables[1].Rows[jr]["TextCode"]));
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int sub = 0; sub < ds.Tables[3].Rows.Count; sub++)
                    {
                        da.Columns.Add(Convert.ToString(ds.Tables[3].Rows[sub]["value"]));
                        addarray.Add(Convert.ToString(ds.Tables[3].Rows[sub]["priority"]));
                        comm_value.Add(Convert.ToString(ds.Tables[3].Rows[sub]["Textval"]), Convert.ToString(ds.Tables[3].Rows[sub]["priority"]));
                        countcolum.Add(Convert.ToString(ds.Tables[3].Rows[sub]["Textval"]));
                        hashcount1.Add(Convert.ToString(ds.Tables[3].Rows[sub]["Textval"]), Convert.ToString(ds.Tables[3].Rows[sub]["TextCode"]));
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int sno = 0;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        sno++;
                        da.Rows.Add(da.NewRow());
                        da.Rows[row][0] = Convert.ToString(sno);
                        da.Rows[row][1] = Convert.ToString(ds.Tables[0].Rows[row]["dept_name"]);

                        da.Rows[row][2] = Convert.ToString(ds.Tables[0].Rows[row]["No_Of_seats"]);
                        percentvalue = Convert.ToDouble(totalvalue) / 100 * Convert.ToDouble(ds.Tables[0].Rows[row]["No_Of_seats"]);
                        da.Rows[row][3] = Convert.ToString(Math.Round(percentvalue));

                        int add = 3;
                        double religaddpercent = 0;
                        double commpercent = 0;
                        if (addreligarray.Count > 0)
                        {

                            for (int count = 0; count < addreligarray.Count; count++)
                            {
                                add++;
                                string arrayvalue = Convert.ToString(addreligarray[count]);
                                religaddpercent = religaddpercent + Convert.ToDouble(arrayvalue);
                                double subpercent = Convert.ToDouble(arrayvalue) / 100 * Convert.ToDouble(percentvalue);
                                da.Rows[row][add] = Convert.ToString(Math.Round(subpercent));

                            }
                        }
                        if (addarray.Count > 0)
                        {
                            if (religaddpercent != 0)
                            {
                                Hashtable addhasnew = new Hashtable();
                                ArrayList mainchcek = new ArrayList();
                                commpercent = Convert.ToDouble(100 - religaddpercent) / 100 * Convert.ToDouble(percentvalue);
                                if (percentvalue > commpercent)
                                {
                                    for (int sub = 0; sub < addarray.Count; sub++)
                                    {
                                        string columnvalue = Convert.ToString(countcolum[sub]);
                                        string array1 = Convert.ToString(addarray[sub]);
                                        if (columnvalue == "BC" || columnvalue == "BCM")
                                        {
                                            if (!addhasnew.Contains("BC"))
                                            {
                                                addhasnew.Add(columnvalue, array1);
                                                mainchcek.Add("BC");
                                            }
                                            else
                                            {
                                                string addvalue = Convert.ToString(addhasnew["BC"]);
                                                double addsumvalue = Convert.ToDouble(addvalue) + Convert.ToDouble(array1);
                                                addhasnew.Remove("BC");
                                                addhasnew.Add("BC", addsumvalue);
                                            }
                                        }
                                        else if (columnvalue == "OC" || columnvalue == "ST")
                                        {
                                            if (!addhasnew.Contains("OC"))
                                            {
                                                addhasnew.Add(columnvalue, array1);
                                                mainchcek.Add("OC");
                                            }
                                            else
                                            {
                                                string addvalue = Convert.ToString(addhasnew["OC"]);
                                                double addsumvalue = Convert.ToDouble(addvalue) + Convert.ToDouble(array1);
                                                addhasnew.Remove("OC");
                                                addhasnew.Add("OC", addsumvalue);
                                            }
                                        }
                                        else if (columnvalue == "MBC/DNC")
                                        {
                                            if (!addhasnew.Contains(columnvalue))
                                            {
                                                addhasnew.Add(columnvalue, array1);
                                                mainchcek.Add("MBC/DNC");
                                            }
                                            else
                                            {
                                                string addvalue = Convert.ToString(addhasnew[columnvalue]);
                                                double addsumvalue = Convert.ToDouble(addvalue) + Convert.ToDouble(array1);
                                                addhasnew.Remove(columnvalue);
                                                addhasnew.Add(columnvalue, addsumvalue);
                                            }
                                        }
                                        else if (columnvalue == "SC" || columnvalue == "SC(Arunthathiyar)")
                                        {
                                            if (!addhasnew.Contains("SC"))
                                            {
                                                addhasnew.Add(columnvalue, array1);
                                                mainchcek.Add("SC");
                                            }
                                            else
                                            {
                                                string addvalue = Convert.ToString(addhasnew["SC"]);
                                                double addsumvalue = Convert.ToDouble(addvalue) + Convert.ToDouble(array1);
                                                addhasnew.Remove("SC");
                                                addhasnew.Add("SC", addsumvalue);
                                            }
                                        }
                                    }

                                    for (int invalue = 0; invalue < mainchcek.Count; invalue++)
                                    {
                                        add++;
                                        string columnvalue = Convert.ToString(mainchcek[invalue]);
                                        if (columnvalue == "BC")
                                        {
                                            string percentage = Convert.ToString(addhasnew[columnvalue]);
                                            double commvalue = Convert.ToDouble(percentage) / 100 * Convert.ToDouble(commpercent);
                                            double round = Math.Round(commvalue);
                                            double bcmcount = 0;
                                            if (round > 1)
                                            {
                                                string subpercentage = Convert.ToString(comm_value["BCM"]);
                                                bcmcount = Convert.ToDouble(subpercentage) / 100 * Convert.ToDouble(round);
                                                if (bcmcount > 0 || bcmcount == 0)
                                                {
                                                    bcmcount = 1;
                                                }
                                            }
                                            double bcvlaue = round - Convert.ToDouble(Math.Round(bcmcount));
                                            da.Rows[row][add] = Convert.ToString(bcvlaue);
                                            add++;
                                            da.Rows[row][add] = Convert.ToString(Math.Round(bcmcount));
                                        }
                                        else if (columnvalue == "OC")
                                        {
                                            string percentage = Convert.ToString(addhasnew[columnvalue]);
                                            double commvalue = Convert.ToDouble(percentage) / 100 * Convert.ToDouble(commpercent);
                                            double round = Math.Round(commvalue);
                                            double bcvlaue = round - 1;
                                            da.Rows[row][add] = Convert.ToString(bcvlaue);
                                        }
                                        else if (columnvalue == "SC")
                                        {
                                            string percentage = Convert.ToString(addhasnew[columnvalue]);
                                            double commvalue = Convert.ToDouble(percentage) / 100 * Convert.ToDouble(commpercent);
                                            double round = Math.Round(commvalue);
                                            double scacount = 0;
                                            if (round > 1)
                                            {
                                                string subpercentage = Convert.ToString(comm_value["SC(Arunthathiyar)"]);
                                                scacount = Convert.ToDouble(subpercentage) / 100 * Convert.ToDouble(round);
                                                if (scacount > 0 || scacount == 0)
                                                {
                                                    scacount = 1;
                                                }
                                            }
                                            double bcvlaue = round - scacount;
                                            da.Rows[row][add] = Convert.ToString(bcvlaue);
                                            add++;
                                            da.Rows[row][add] = Convert.ToString(Math.Round(scacount));
                                        }
                                        else if (columnvalue == "MBC/DNC")
                                        {
                                            string percentage = Convert.ToString(addhasnew[columnvalue]);
                                            double commvalue = Convert.ToDouble(percentage) / 100 * Convert.ToDouble(commpercent);
                                            double round = Math.Round(commvalue);
                                            da.Rows[row][add] = Convert.ToString(Math.Round(round));
                                        }
                                    }
                                    add++;
                                    da.Rows[row][add] = Convert.ToString(1);

                                }
                            }
                        }
                        if (da.Rows.Count > 0)
                        {
                            //int col_count = countcolum.Count - da.Columns.Count;
                            string degree_code = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                            int column = 3;
                            for (int tab = 0; tab < hashcount.Count; tab++)
                            {
                                column++;
                                string relig_value = Convert.ToString(countcolum1[tab]);
                                string relig_number = Convert.ToString(hashcount[relig_value]);
                                string community1 = Convert.ToString(da.Rows[row][column]);

                                string selectupadatequery = "if not exists (select priority  from admitcolumnset where textcriteria ='relig' and college_code=" + college_code + " and priority<>'0' and setcolumn='" + degree_code + "' and column_name ='" + relig_number + "' and user_code=" + user_code + ")";
                                selectupadatequery = selectupadatequery + " insert into admitcolumnset (user_code,setcolumn,column_name,priority,college_code,textcriteria) values('" + user_code + "','" + degree_code + "','" + relig_number + "','" + community1 + "','" + college_code + "','relig')";
                                selectupadatequery = selectupadatequery + "else update admitcolumnset set priority='" + community1 + "' where textcriteria='relig' and college_code='" + college_code + "' and user_code='" + user_code + "' and setcolumn='" + degree_code + "' and column_name ='" + relig_number + "'";
                                int j = dt.update_method_wo_parameter(selectupadatequery, "Text");
                            }

                            for (int tab = 0; tab < hashcount1.Count; tab++)
                            {
                                column++;
                                string relig_value = Convert.ToString(countcolum[tab]);
                                string relig_number = Convert.ToString(hashcount1[relig_value]);
                                string community1 = Convert.ToString(da.Rows[row][column]);

                                string selectupadatequery = "if not exists (select priority  from admitcolumnset where textcriteria ='community' and college_code=" + college_code + " and priority<>'0' and setcolumn='" + degree_code + "' and column_name ='" + relig_number + "' and user_code=" + user_code + ")";
                                selectupadatequery = selectupadatequery + " insert into admitcolumnset (user_code,setcolumn,column_name,priority,college_code,textcriteria) values('" + user_code + "','" + degree_code + "','" + relig_number + "','" + community1 + "','" + college_code + "','community')";
                                selectupadatequery = selectupadatequery + "else update admitcolumnset set priority='" + community1 + "' where textcriteria='community' and college_code='" + college_code + "' and user_code='" + user_code + "' and setcolumn='" + degree_code + "' and column_name ='" + relig_number + "'";
                                int j = dt.update_method_wo_parameter(selectupadatequery, "Text");
                            }
                            //string community1 = Convert.ToString(da.Rows[row][5]);
                            //string selectupadatequery = "if not exists (select priority  from admitcolumnset where textcriteria ='community' and college_code=" + college_code + " and priority<>0 and setcolumn='" + degree_code + "' and column_name ='" + religvalue + "' and user_code=" + user_code + ")";
                            //selectupadatequery = selectupadatequery + " insert into admitcolumnset (user_code,setcolumn,column_name,priority,college_code,textcriteria) values('" + user_code + "','" + degree_code + "','" + religvalue + "','" + txt_Value + "','" + college_code + "','community')";
                            //selectupadatequery = selectupadatequery + "else update admitcolumnset set priority='" + txt_Value + "' where textcriteria='community' and college_code='" + college_code + "' and user_code='" + user_code + "' and setcolumn='" + degree_code + "' and column_name ='" + religvalue + "'";
                            //int j = dt.update_method_wo_parameter(selectupadatequery, "Text");

                        }
                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            string degree_code = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                            string totalstreangthvalue = Convert.ToString(ds.Tables[0].Rows[row]["No_Of_seats"]);
                            string managementvalue = Convert.ToString(ds.Tables[4].Rows[0]["total"]);
                            if (managementvalue.Trim() != "" && totalstreangthvalue.Trim() != "")
                            {
                                double manage = Convert.ToDouble(totalstreangthvalue) / Convert.ToDouble(100) * Convert.ToDouble(managementvalue);
                                string getcode = dt.GetFunction("select TextCode from TextValTable where TextCriteria ='Mngt' and college_code ='" + college_code + "'");
                                string selectupadatequery = "if not exists (select priority  from admitcolumnset where textcriteria ='Management' and college_code=" + college_code + " and priority<>'0' and setcolumn='" + degree_code + "' and column_name ='" + getcode + "' and user_code=" + user_code + ")";
                                selectupadatequery = selectupadatequery + " insert into admitcolumnset (user_code,setcolumn,column_name,priority,college_code,textcriteria) values('" + user_code + "','" + degree_code + "','" + getcode + "','" + manage + "','" + college_code + "','Management')";
                                selectupadatequery = selectupadatequery + "else update admitcolumnset set priority='" + manage + "' where textcriteria='Management' and college_code='" + college_code + "' and user_code='" + user_code + "' and setcolumn='" + degree_code + "' and column_name ='" + getcode + "'";
                                int j = dt.update_method_wo_parameter(selectupadatequery, "Text");
                            }
                        }

                    }
                }

                report_grid.DataSource = da;
                report_grid.DataBind();
                reportdiv.Visible = true;
            }
            else
            {
                reportdiv.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void ddledu_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string empty = "";
            degree();
            bindbranch(empty);
            FpSpread2.Visible = false;
            FpSpread1.Visible = false;
        }
        catch
        {
        }
    }

    protected void report_grid_DataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int count = e.Row.Cells.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (i != 1)
                        {
                            e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[i].Width = 100;
                        }
                    }

                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int count = e.Row.Cells.Count;
                if (count > 0)
                {
                    for (int i = 4; i < count; i++)
                    {
                        string value = e.Row.Cells[i].Text;
                        string[] split = value.Split(' ');
                        if (split[0].Length == 2)
                        {
                            e.Row.Cells[i].Width = 300;
                        }
                        else
                        {
                            e.Row.Cells[i].Width = 100;
                        }

                    }

                }
            }
        }
        catch
        {
        }

    }

    public void bind()
    {
        StringReader sr = null;
        Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                //To Export all pages
                report_grid.AllowPaging = false;


                for (int i = 0; i < report_grid.HeaderRow.Cells.Count; i++)
                {
                    report_grid.HeaderRow.Cells[i].Style.Add("background-color", "#df5015");
                }
                report_grid.RenderControl(hw);
                Response.Write(sw.ToString());
                Response.End();
                sr = new StringReader(sw.ToString());
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();



            }
        }
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(pdfDoc);
        Response.End();
    }

    protected void Click_pdf(object sender, EventArgs e)
    {
        //bind();

        //Response.ContentType = "application/pdf";
        //Response.AddHeader("content-disposition", "attachment;filename=Vithal_Wadje.pdf");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);

        //report_grid.RenderControl(hw);
        //StringReader sr = new StringReader(sw.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();
        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.End();
        //report_grid.AllowPaging = true;

        //report_grid.DataBind();

        //Response.ContentType = "application/pdf";
        //Response.AddHeader("content-disposition", "attachment;filename=Export.pdf");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);
        //HtmlForm frm = new HtmlForm();
        //report_grid.Parent.Controls.Add(frm);
        //frm.Attributes["runat"] = "server";
        //frm.Controls.Add(report_grid);
        //frm.RenderControl(hw);
        //StringReader sr = new StringReader(sw.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();
        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.End();

        // Word Convert *********************

        //Response.AddHeader("content-disposition", "attachment;filename=Export.doc");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.ContentType = "application/vnd.word";

        //StringWriter stringWrite = new StringWriter();
        //HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

        //HtmlForm frm = new HtmlForm();
        //report_grid.Parent.Controls.Add(frm);
        //frm.Attributes["runat"] = "server";
        //frm.Controls.Add(report_grid);
        //frm.RenderControl(htmlWrite);

        //Response.Write(stringWrite.ToString());
        //Response.End();

        // Excel convert **************************

        //string attachment = "attachment; filename=Export.xls";
        //Response.ClearContent();
        //Response.AddHeader("content-disposition", attachment);
        //Response.ContentType = "application/ms-excel";
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter htw = new HtmlTextWriter(sw);
        //HtmlForm frm = new HtmlForm();
        //report_grid.Parent.Controls.Add(frm);
        //frm.Attributes["runat"] = "server";
        //frm.Controls.Add(report_grid);
        //frm.RenderControl(htw);
        //Response.Write(sw.ToString());
        //Response.End();



        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=UserDetails.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        report_grid.AllowPaging = false;
        report_grid.HeaderRow.Style.Add("width", "15%");
        report_grid.HeaderRow.Style.Add("font-size", "10px");
        report_grid.HeaderRow.Style.Add("text-align", "center");
        report_grid.Style.Add("text-decoration", "none");
        report_grid.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
        report_grid.Style.Add("font-size", "8px");
        report_grid.RenderControl(hw);
        StringReader sr = new StringReader(sw.ToString());
        Document pdfDoc = new Document(PageSize.A4);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        Paragraph p = new Paragraph();
        string txt = "";
        p.Add(txt);
        pdfDoc.Open();
        pdfDoc.Add(p);
        htmlparser.Parse(sr);
        pdfDoc.Close();
        Response.Write(pdfDoc);
        Response.End();

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */

    }

    public void columnorderby()
    {
        try
        {
            ArrayList columnarray = new ArrayList();

            int con = 0;
            FpSpread8.Visible = true;
            FpSpread8.Sheets[0].RowCount = 0;
            FpSpread8.Sheets[0].ColumnCount = 5;
            FpSpread8.Sheets[0].RowHeader.Visible = false;
            FpSpread8.Sheets[0].AutoPostBack = false;
            FpSpread8.Height = 430;
            FpSpread8.Width = 350;
            FpSpread8.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#393965");
            darkstyle.ForeColor = System.Drawing.Color.White;
            // darkstyle.Border = new FarPoint.Web.Spread.Border(Color.Crimson);
            // Apply the new style.
            FpSpread8.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.CheckBoxCellType chkcel1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Column";
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Column1";
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread8.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread8.Sheets[0].Columns[0].Width = 50;
            FpSpread8.Sheets[0].Columns[1].Width = 180;
            FpSpread8.Sheets[0].Columns[2].Width = 80;
            FpSpread8.Sheets[0].Columns[3].Width = 50;
            FpSpread8.Sheets[0].Columns[4].Width = 50;
            FpSpread8.Sheets[0].Columns[2].Visible = false;
            chkcel1.AutoPostBack = true;

            if (rdo_comm.Checked == true)
            {
                for (int i = 0; i < cblcommunity.Items.Count; i++)
                {
                    if (cblcommunity.Items[i].Selected == true)
                    {
                        con++;
                        FpSpread8.Sheets[0].RowCount++;
                        FpSpread8.Sheets[0].Cells[i, 0].Text = Convert.ToString(con);
                        FpSpread8.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                        FpSpread8.Sheets[0].Cells[i, 0].Font.Bold = true;
                        FpSpread8.Sheets[0].Cells[i, 0].Font.Size = FontUnit.Medium;
                        FpSpread8.Sheets[0].Cells[i, 0].Locked = true;
                        FpSpread8.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread8.Sheets[0].Cells[i, 1].Text = Convert.ToString(cblcommunity.Items[i].Text);
                        FpSpread8.Sheets[0].Cells[i, 1].Tag = "religion";
                        FpSpread8.Sheets[0].Cells[i, 1].Font.Name = "Book Antiqua";
                        FpSpread8.Sheets[0].Cells[i, 1].Font.Bold = true;
                        FpSpread8.Sheets[0].Cells[i, 1].Font.Size = FontUnit.Medium;
                        FpSpread8.Sheets[0].Cells[i, 1].Locked = true;

                        FpSpread8.Sheets[0].Cells[i, 2].Text = Convert.ToString(cblcommunity.Items[i].Value);
                        FpSpread8.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                        FpSpread8.Sheets[0].Cells[i, 2].Font.Bold = true;
                        FpSpread8.Sheets[0].Cells[i, 2].Font.Size = FontUnit.Medium;
                        FpSpread8.Sheets[0].Cells[i, 2].Locked = true;

                        FpSpread8.Sheets[0].Cells[i, 3].CellType = chkcel1;
                        FpSpread8.Sheets[0].Cells[i, 3].Font.Name = "Book Antiqua";
                        FpSpread8.Sheets[0].Cells[i, 3].Font.Bold = true;
                        FpSpread8.Sheets[0].Cells[i, 3].Font.Size = FontUnit.Medium;
                        FpSpread8.Sheets[0].Cells[i, 3].Value = false;


                        FpSpread8.Sheets[0].Cells[i, 4].Text = "";
                        FpSpread8.Sheets[0].Cells[i, 4].Font.Name = "Book Antiqua";
                        FpSpread8.Sheets[0].Cells[i, 4].Font.Bold = true;
                        FpSpread8.Sheets[0].Cells[i, 4].Font.Size = FontUnit.Medium;
                        FpSpread8.Sheets[0].Cells[i, 4].Locked = true;
                        FpSpread8.Sheets[0].Cells[i, 4].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            else
            {
                for (int i = 0; i < cblreligion.Items.Count; i++)
                {
                    if (cblreligion.Items[i].Selected == true)
                    {
                        con++;
                        FpSpread8.Sheets[0].RowCount++;
                        FpSpread8.Sheets[0].Cells[i, 0].Text = Convert.ToString(con);
                        FpSpread8.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                        FpSpread8.Sheets[0].Cells[i, 0].Font.Bold = true;
                        FpSpread8.Sheets[0].Cells[i, 0].Font.Size = FontUnit.Medium;
                        FpSpread8.Sheets[0].Cells[i, 0].Locked = true;
                        FpSpread8.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread8.Sheets[0].Cells[i, 1].Text = Convert.ToString(cblreligion.Items[i].Text);
                        FpSpread8.Sheets[0].Cells[i, 1].Tag = "religion";
                        FpSpread8.Sheets[0].Cells[i, 1].Font.Name = "Book Antiqua";
                        FpSpread8.Sheets[0].Cells[i, 1].Font.Bold = true;
                        FpSpread8.Sheets[0].Cells[i, 1].Font.Size = FontUnit.Medium;
                        FpSpread8.Sheets[0].Cells[i, 1].Locked = true;

                        FpSpread8.Sheets[0].Cells[i, 2].Text = Convert.ToString(cblreligion.Items[i].Value);
                        FpSpread8.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                        FpSpread8.Sheets[0].Cells[i, 2].Font.Bold = true;
                        FpSpread8.Sheets[0].Cells[i, 2].Font.Size = FontUnit.Medium;
                        FpSpread8.Sheets[0].Cells[i, 2].Locked = true;

                        FpSpread8.Sheets[0].Cells[i, 3].CellType = chkcel1;
                        FpSpread8.Sheets[0].Cells[i, 3].Font.Name = "Book Antiqua";
                        FpSpread8.Sheets[0].Cells[i, 3].Font.Bold = true;
                        FpSpread8.Sheets[0].Cells[i, 3].Font.Size = FontUnit.Medium;
                        FpSpread8.Sheets[0].Cells[i, 3].Value = false;


                        FpSpread8.Sheets[0].Cells[i, 4].Text = "";
                        FpSpread8.Sheets[0].Cells[i, 4].Font.Name = "Book Antiqua";
                        FpSpread8.Sheets[0].Cells[i, 4].Font.Bold = true;
                        FpSpread8.Sheets[0].Cells[i, 4].Font.Size = FontUnit.Medium;
                        FpSpread8.Sheets[0].Cells[i, 4].Locked = true;
                        FpSpread8.Sheets[0].Cells[i, 4].HorizontalAlign = HorizontalAlign.Center;
                    }
                }

            }


            //FpSpread8.Sheets[0].RowCount = ds.Tables[1].Rows.Count + 1 + ds.Tables[0].Rows.Count;
            //FpSpread8.Sheets[0].RowCount++;
            //FpSpread8.Sheets[0].Cells[CC + 1, 0].Text = Convert.ToString(con + 1);
            //FpSpread8.Sheets[0].Cells[CC + 1, 0].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread8.Sheets[0].Cells[CC + 1, 1].Text = "Total Mark";
            //FpSpread8.Sheets[0].Cells[CC + 1, 2].Text = "securedmark";
            //FpSpread8.Sheets[0].Cells[CC + 1, 3].CellType = chkcel1;
            //FpSpread8.Sheets[0].Cells[CC + 1, 4].Text = "";
            FpSpread8.Sheets[0].PageSize = FpSpread8.Sheets[0].RowCount;


        }
        catch (Exception ex)
        {

        }
    }

    public void selectcolumnORDERBY()
    {
        FpSpread9.Sheets[0].RowCount = 0;
        FpSpread9.Sheets[0].ColumnCount = 2;
        FpSpread9.Sheets[0].RowHeader.Visible = false;
        FpSpread9.Sheets[0].AutoPostBack = true;
        FpSpread9.CommandBar.Visible = false;
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#393965");
        darkstyle.ForeColor = System.Drawing.Color.White;
        // darkstyle.Border = new FarPoint.Web.Spread.Border(Color.Crimson);
        // Apply the new style.
        FpSpread9.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        FpSpread9.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread9.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        FpSpread9.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        FpSpread9.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        FpSpread9.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Selected Column";
        FpSpread9.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        FpSpread9.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        FpSpread9.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        FpSpread9.Sheets[0].Columns[0].Width = 50;
        FpSpread9.Sheets[0].Columns[1].Width = 200;
    }

    public void selectcolumnfunctionorderby()
    {
        try
        {
            DataView dv1 = new DataView();
            FpSpread9.Sheets[0].RowCount = 0;
            string textcriteria = "";
            string priority = "";

            if (rdo_comm.Checked == true)
            {
                textcriteria = "comm";
                priority = "priority1";

            }
            else if (rdo_reli.Checked == true)
            {
                textcriteria = "relig";
                priority = "priority2";

            }
            string selectquery1 = dt.GetFunction("select setcolumn from admitcolumnset where textcriteria ='OrderByMarks' and college_code ='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' and user_code ='" + user_code + "' order by priority");
            if (selectquery1 == "Percentage")
            {
                rdo_percent.Checked = true;
            }
            else
            {
                rdo_tot.Checked = true;
            }

            string selectquery = "select * from textvaltable where textcriteria ='" + textcriteria + "' and college_code ='" + Convert.ToString(ddl_college.SelectedItem.Value) + "' and ISNULL(" + priority + ",'0')!=0 order by " + priority + "";
            ds.Clear();
            ds = dt.select_method_wo_parameter(selectquery, "Text");
            FpSpread9.Sheets[0].RowCount = 0;

            if (ds.Tables[0].Rows.Count > 0)
            {
                int co = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    co++;
                    FpSpread9.Sheets[0].RowCount++;
                    FpSpread9.Sheets[0].Cells[i, 0].Text = Convert.ToString(co);
                    FpSpread9.Sheets[0].Cells[i, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["TextVal"]);
                    FpSpread9.Sheets[0].Cells[i, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["TextVal"]);
                    FpSpread9.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread9.Sheets[0].Cells[i, 1].Column.Width = 180;

                }
                if (FpSpread8.Sheets[0].Rows.Count > 0)
                {
                    for (int j = 0; j < FpSpread8.Sheets[0].Rows.Count; j++)
                    {
                        string headertext = FpSpread8.Sheets[0].Cells[j, 1].Text;
                        string headervalue = Convert.ToString(FpSpread8.Sheets[0].Cells[j, 2].Text);
                        ds.Tables[0].DefaultView.RowFilter = "TextVal='" + headertext + "'";
                        dv1 = ds.Tables[0].DefaultView;
                        if (dv1.Count > 0)
                        {
                            string text_priority = Convert.ToString(dv1[0][priority]);
                            if (text_priority.Trim() != "")
                            {
                                FpSpread8.Sheets[0].Cells[j, 3].Value = true;
                                FpSpread8.Sheets[0].Cells[j, 3].Locked = true;
                                FpSpread8.Sheets[0].Cells[j, 4].Text = text_priority;
                            }
                        }

                    }
                }
            }
            if (FpSpread9.Sheets[0].RowCount > 0)
            {
                FpSpread9.Visible = true;
                FpSpread9.Sheets[0].PageSize = FpSpread9.Sheets[0].RowCount;
            }
            else
            {
                FpSpread9.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void FpSpread8_command(object sender, EventArgs e)
    {
        try
        {
            int isval1 = 0;
            string activerow = FpSpread8.ActiveSheetView.ActiveRow.ToString();
            string text = (FpSpread8.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
            isval1 = Convert.ToInt32(FpSpread8.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Value);

            if (FpSpread9.Sheets[0].Rows.Count > 0)
            {
                val = Convert.ToInt32(FpSpread9.Sheets[0].Rows.Count);

            }

            if (isval1 == 1)
            {
                val = val + 1;
                FpSpread8.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text = val.ToString();
                FpSpread8.Sheets[0].Cells[Convert.ToInt32(activerow), 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread8.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Font.Name = "Times New Roman";
                FpSpread8.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Locked = true;
                FpSpread9.Sheets[0].RowCount++;
                FpSpread9.Sheets[0].Cells[FpSpread9.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread9.Sheets[0].RowCount);
                FpSpread9.Sheets[0].Cells[FpSpread9.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread9.Sheets[0].Cells[FpSpread9.Sheets[0].RowCount - 1, 0].Font.Name = "Times New Roman";
                FpSpread9.Sheets[0].Cells[FpSpread9.Sheets[0].RowCount - 1, 1].Text = text;
                FpSpread9.Sheets[0].Cells[FpSpread9.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread9.Sheets[0].Cells[FpSpread9.Sheets[0].RowCount - 1, 1].Font.Name = "Times New Roman";
            }
            else if (isval1 == 0)
            {
                val = val - 1;
                FpSpread9.Sheets[0].RowCount--;
                FpSpread9.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text = "";
                FpSpread9.Sheets[0].Cells[Convert.ToInt32(activerow), 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread9.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Font.Name = "Times New Roman";
            }
            FpSpread9.Visible = true;
            FpSpread9.Sheets[0].PageSize = FpSpread7.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {

        }
    }

    public void rdo_reli_CheckedChanged(object sender, EventArgs e)
    {
        columnorderby();
        val = 0;
        FpSpread9.Visible = false;
        FpSpread9.Sheets[0].RowCount = 0;
        selectcolumnORDERBY();
        selectcolumnfunctionorderby();
    }

    public void rdo_comm_CheckedChanged(object sender, EventArgs e)
    {
        columnorderby();
        val = 0;
        FpSpread9.Visible = false;
        FpSpread9.Sheets[0].RowCount = 0;
        selectcolumnORDERBY();
        selectcolumnfunctionorderby();
    }

    public void cb_attempt_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_attempt.Checked == true)
        {
            rdo_percent.Enabled = true;
            rdo_tot.Enabled = true;
        }
        else
        {
            rdo_percent.Enabled = false;
            rdo_tot.Enabled = false;
        }
    }

    protected void FpSpread10_command(object sender, EventArgs e)
    {
        try
        {
            int isval1 = 0;
            string activerow = FpSpread10.ActiveSheetView.ActiveRow.ToString();
            string text = (FpSpread10.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
            isval1 = Convert.ToInt32(FpSpread10.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Value);



            if (isval1 == 1)
            {
                val1 = val1 + 1;
                FpSpread10.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text = val1.ToString();
                FpSpread10.Sheets[0].Cells[Convert.ToInt32(activerow), 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread10.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Font.Name = "Times New Roman";
                FpSpread10.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Locked = true;

            }

        }
        catch (Exception ex)
        {

        }
    }

    public void columnbasic()
    {
        try
        {
            ArrayList columnarray = new ArrayList();
            columnarray.Add("Religion-ISNULL (tt.priority2,0) desc");
            columnarray.Add("Community-isnull(ts.priority1,0) desc");
            columnarray.Add("Attempt-noofattempts asc");
            columnarray.Add("Mark-Mark");


            FpSpread10.Visible = true;
            FpSpread10.Sheets[0].RowCount = 0;
            FpSpread10.Sheets[0].ColumnCount = 5;
            FpSpread10.Sheets[0].RowHeader.Visible = false;
            FpSpread10.Sheets[0].AutoPostBack = false;
            FpSpread10.Height = 200;
            FpSpread10.Width = 280;
            FpSpread10.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#393965");
            darkstyle.ForeColor = System.Drawing.Color.White;
            // darkstyle.Border = new FarPoint.Web.Spread.Border(Color.Crimson);
            // Apply the new style.
            FpSpread10.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.CheckBoxCellType chkcel1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Column";
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Column1";
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread10.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread10.Sheets[0].Columns[0].Width = 50;
            FpSpread10.Sheets[0].Columns[1].Width = 100;
            FpSpread10.Sheets[0].Columns[2].Width = 50;
            FpSpread10.Sheets[0].Columns[3].Width = 50;
            FpSpread10.Sheets[0].Columns[4].Width = 60;
            FpSpread10.Sheets[0].Columns[2].Visible = false;
            chkcel1.AutoPostBack = true;
            if (columnarray.Count > 0)
            {
                int con = 0;
                for (int i = 0; i < columnarray.Count; i++)
                {
                    con++;
                    FpSpread10.Sheets[0].RowCount++;
                    string column_value = columnarray[i].ToString();
                    string[] split_columnvalue = column_value.Split('-');
                    if (split_columnvalue.Length > 0)
                    {
                        FpSpread10.Sheets[0].Cells[i, 0].Text = Convert.ToString(con);
                        FpSpread10.Sheets[0].Cells[i, 0].Font.Name = "Book Antiqua";
                        FpSpread10.Sheets[0].Cells[i, 0].Font.Bold = true;
                        FpSpread10.Sheets[0].Cells[i, 0].Font.Size = FontUnit.Medium;
                        FpSpread10.Sheets[0].Cells[i, 0].Locked = true;
                        FpSpread10.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread10.Sheets[0].Cells[i, 1].Text = Convert.ToString(split_columnvalue[0]);
                        FpSpread10.Sheets[0].Cells[i, 1].Font.Name = "Book Antiqua";
                        FpSpread10.Sheets[0].Cells[i, 1].Font.Bold = true;
                        FpSpread10.Sheets[0].Cells[i, 1].Font.Size = FontUnit.Medium;
                        FpSpread10.Sheets[0].Cells[i, 1].Locked = true;

                        FpSpread10.Sheets[0].Cells[i, 2].Text = Convert.ToString(split_columnvalue[1]);
                        FpSpread10.Sheets[0].Cells[i, 2].Font.Name = "Book Antiqua";
                        FpSpread10.Sheets[0].Cells[i, 2].Font.Bold = true;
                        FpSpread10.Sheets[0].Cells[i, 2].Font.Size = FontUnit.Medium;
                        FpSpread10.Sheets[0].Cells[i, 2].Locked = true;

                        FpSpread10.Sheets[0].Cells[i, 3].CellType = chkcel1;
                        FpSpread10.Sheets[0].Cells[i, 3].Font.Name = "Book Antiqua";
                        FpSpread10.Sheets[0].Cells[i, 3].Font.Bold = true;
                        FpSpread10.Sheets[0].Cells[i, 3].Font.Size = FontUnit.Medium;
                        FpSpread10.Sheets[0].Cells[i, 3].Value = false;


                        FpSpread10.Sheets[0].Cells[i, 4].Text = "";
                        FpSpread10.Sheets[0].Cells[i, 4].Font.Name = "Book Antiqua";
                        FpSpread10.Sheets[0].Cells[i, 4].Font.Bold = true;
                        FpSpread10.Sheets[0].Cells[i, 4].Font.Size = FontUnit.Medium;
                        FpSpread10.Sheets[0].Cells[i, 4].Locked = true;
                        FpSpread10.Sheets[0].Cells[i, 4].HorizontalAlign = HorizontalAlign.Center;
                    }

                }
                FpSpread10.Sheets[0].PageSize = FpSpread10.Sheets[0].RowCount;
            }

        }
        catch (Exception ex)
        {

        }
    }

    public void selectcolumnbasicorderby()
    {
        try
        {
            DataView dv1 = new DataView();


            string selectquery = "select * from Master_Settings where settings='orderbymarks'";
            ds.Clear();
            ds = dt.select_method_wo_parameter(selectquery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {

                if (FpSpread10.Sheets[0].Rows.Count > 0)
                {


                    //ds.Tables[0].DefaultView.RowFilter = "TextVal='" + headertext + "'";
                    //dv1 = ds.Tables[0].DefaultView;
                    //if (dv1.Count > 0)
                    //{
                    string text_priority = Convert.ToString(ds.Tables[0].Rows[0]["value"]);
                    string[] sp = text_priority.Split(',');
                    //    if (text_priority.Trim() != "")
                    for (int s = 0; s < sp.Length; s++)
                    {

                        FpSpread10.Sheets[0].Cells[s, 3].Value = true;
                        FpSpread10.Sheets[0].Cells[s, 3].Locked = true;
                        FpSpread10.Sheets[0].Cells[s, 4].Text = sp[s];

                    }
                    // }

                }

            }

        }
        catch
        {
        }
    }

    //17.06.16
    protected void bindcollege()
    {
        try
        {
            string clgname = "select college_code,collname from collinfo ";
            ds = dt.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }

        }
        catch { }
    }

    protected void ddl_college_selectedindex(object sender, EventArgs e)
    {
        try
        {
            bindreglion();
            bindcommunity();
            if (rdo_reli.Checked == true)
            {
                rdo_reli_CheckedChanged(sender, e);
            }
            if (rdo_comm.Checked == true)
            {
                rdo_comm_CheckedChanged(sender, e);
            }
        }
        catch { }
    }

    protected void btn_deltype_OnClick(object sender, EventArgs e)
    {
        string sql = "delete from CO_MasterValues where MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and MasterCriteria='Region header' and CollegeCode='" + college_code + "' ";
        int delete = dt.update_method_wo_parameter(sql, "TEXT");
        if (delete != 0)
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Deleted Successfully\");", true);
            bindtype();
        }
        else
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No records found\");", true);
        }
    }

    protected void btn_addtype_OnClick(object sender, EventArgs e)
    {
        imgdiv33.Visible = true;
        panel_description11.Visible = true;
    }

    protected void btndescpopadd_Click(object sender, EventArgs e)
    {
        if (txt_description11.Text != "")
        {
            string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='Region header' and CollegeCode ='" + college_code + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='Region header' and CollegeCode ='" + college_code + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','Region header','" + college_code + "')";
            int insert = dt.update_method_wo_parameter(sql, "TEXT");
            if (insert != 0)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
                txt_description11.Text = "";
                bindtype();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Enter the description\");", true);
        }
    }

    protected void btnsavegroupbt_Click(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedItem.Value != "" && txt_region.Text != "--Select--")
        {
            string setgroupby = "";
            for (int m = 0; m < cbl_region1.Items.Count; m++)
            {
                if (cbl_region1.Items[m].Selected == true)
                {
                    string addrelival1 = cbl_region1.Items[m].Value.ToString();
                    if (setgroupby == "")
                    {
                        setgroupby = addrelival1;
                    }
                    else
                    {
                        setgroupby = setgroupby + "," + addrelival1;
                    }
                }
            }
            string sql = " if exists(select*from CO_MasterValues where MasterCriteria ='Region header' and MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' )  update CO_MasterValues set MasterCriteriaValue1='" + setgroupby + "',mastercriteria1='" + ddl_priority.SelectedItem.Value.ToString() + "' where  MasterCriteria ='Region header' and MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and  CollegeCode='" + college_code + "'";
            int insert = dt.update_method_wo_parameter(sql, "TEXT");
            if (insert != 0)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
            }
        }
    }

    protected void btndescpopexit_Click(object sender, EventArgs e)
    {
        imgdiv33.Visible = false;
    }

    protected void cbreligion1_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cb_region1.Checked == true)
            {
                for (int i = 0; i < cbl_region1.Items.Count; i++)
                {

                    cbl_region1.Items[i].Selected = true;
                    txt_region.Text = "Religion(" + (cbl_region1.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cbl_region1.Items.Count; i++)
                {
                    cbl_region1.Items[i].Selected = false;
                    txt_region.Text = "--Select--";
                }
            }
        }
        catch
        {

        }
    }

    protected void cblreligion1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_region1.Checked = false;
            for (int i = 0; i < cbl_region1.Items.Count; i++)
            {
                if (cbl_region1.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            txt_region.Text = "Religion(" + seatcount.ToString() + ")";
        }
        catch
        {

        }
    }

    protected void bindtype()
    {
        try
        {
            string q1 = "  select mastervalue,mastercode,mastercriteria1 from CO_MasterValues where MasterCriteria ='Region header' and CollegeCode ='" + college_code + "'";
            ds.Clear();
            ds = dt.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_coltypeadd.DataSource = ds;
                ddl_coltypeadd.DataTextField = "mastervalue";
                ddl_coltypeadd.DataValueField = "mastercode";
                ddl_coltypeadd.DataBind();

                ddl_priority.Items.Clear();
                for (int m = 1; m <= ddl_coltypeadd.Items.Count; m++)
                {
                    ddl_priority.Items.Add(m.ToString());
                }

                //string value = dt.GetFunction(" select mastercriteria1 from CO_MasterValues where MasterCriteria ='Region header' and MasterCode='" + Convert.ToString(ddl_coltypeadd.SelectedItem.Value) + "'");

                //if (value.Trim() != "" && value.Trim() != null)
                //{
                //    ddl_priority.Items.Remove(value);
                //}
            }
        }
        catch { }
    }

    protected void lnkbtn_religion_click(object sender, EventArgs e)
    {
        ddl_coltypeadd_selectedindexchange(sender, e);
        loadreligion();
        regionsettings.Visible = true;
    }

    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        regionsettings.Visible = false;
    }

    protected void ddl_coltypeadd_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            cbl_region1.ClearSelection(); txt_region.Text = "--Select--";
            if (ddl_coltypeadd.Items.Count > 0)
            {
                string q1 = " select mastercriteriavalue1,mastercriteria1 from co_mastervalues where MasterCriteria ='Region header' and  CollegeCode='" + college_code + "' and MasterCode='" + Convert.ToString(ddl_coltypeadd.SelectedItem.Value) + "' ";
                ds.Clear();
                ds = dt.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string[] value = Convert.ToString(ds.Tables[0].Rows[0][0]).Split(',');
                    int count = 0;
                    if (value[0].ToString() != "")
                    {
                        foreach (var val in value)
                        {
                            cbl_region1.Items.FindByValue(val).Selected = true;
                            count++;
                        }
                        txt_region.Text = "Religion (" + count + ")";
                    }
                    string val1 = Convert.ToString(ds.Tables[0].Rows[0][1]);
                    if (val1.Trim() != "" && val1.Trim() != null)
                    {
                        ddl_priority.SelectedIndex = ddl_priority.Items.IndexOf(ddl_priority.Items.FindByText(val1));
                    }
                }
                else
                {
                }
            }
        }
        catch { }
    }

    protected void ddl_priority_selectedindexchange(object sender, EventArgs e)
    {

    }

    //Added by Idhris 22-11-2016
    private void setIncludeMarks()
    {
        try
        {
            string markInc = "0";
            if (chkIncMarks.Checked == true)
            {
                markInc = "1";
            }

            string appsave = "if exists(select * from Master_Settings where settings='IncludeMarksSettings'  and usercode= " + user_code + " )update Master_Settings set value='" + markInc + "' where settings='IncludeMarksSettings' and usercode=" + user_code + " else insert into Master_Settings (usercode,settings,value) values(" + user_code + ",'IncludeMarksSettings','" + markInc + "')";
            int cc = dt.update_method_wo_parameter(appsave, "text");

            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\" Saved Successfully\");", true);
        }
        catch { }
    }

    private void retrieveIncMarks()
    {
        try
        {
            chkIncMarks.Checked = false;
            if (dt.GetFunction("select value from Master_Settings where settings='IncludeMarksSettings' and usercode= " + user_code + "").Trim() == "1")
            {
                chkIncMarks.Checked = true;
            }
        }
        catch { }
    }

    //Last Modified by Idhris  - 22-11-2016
}
