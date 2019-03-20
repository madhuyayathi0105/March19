using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;
using System.Text;
using System.Collections.Generic;


public partial class Cam_Performance_Report : System.Web.UI.Page
{
    DAccess2 datest = new DAccess2();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string examcodeval = string.Empty;
    string strgrade = string.Empty;
    string strsec = string.Empty;
    string strsection = string.Empty;
    string strsection1 = string.Empty;
    string strsection2 = string.Empty;
    string sturollno = string.Empty;
    string strsubcrd = string.Empty;
    string graders = string.Empty;
    string sqlstr = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string strbranchname = string.Empty;
    string strsem = string.Empty;
    string syllcode = string.Empty;
    string staff_name = string.Empty;
    string strbatchsplit = string.Empty;
    string strbranchsplit = string.Empty;
    string strsecsplit = string.Empty;
    string strecode = string.Empty;
    DataSet dsprint = new DataSet();
    string collnamenew1 = "";
    string address1 = "";
    string address2 = "";
    string address = "";
    string MultiISO = "";

    string address3 = "";
    string affliated = "";
    string category = "";

    string Phoneno = "";
    string Faxno = "";
    string phnfax = "";
    string pincode = "";
    string state = "";
    int subjectcount = 0;
    string district = "";
    string email = "";
    string website = "";

    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    DAccess2 dacces = new DAccess2();
    double passcnt = 0;
    double failcnt = 0;
    double absentcnt = 0;
    double totapp = 0;
    double totapp1 = 0;
    double passpercen = 0;
    double passpercen_round = 0;
    double failpercen = 0;
    double failpercen_round = 0;
    double absentpercen = 0;
    double absentpercen_round = 0;




    static int batchcnt = 0;
    static int degreecnt = 0;
    static int branchcnt = 0;
    static int sectioncnt;
    static int criteria_cnt = 0;

    int count = 0;
    int count1 = 0;
    int count2 = 0;
    int count3 = 0;
    int criteriacnt = 0;
    int criteria_count = 0;

    Hashtable hat = new Hashtable();
    Hashtable hashappcnt = new Hashtable();
    Hashtable hashpasscnt = new Hashtable();

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet1();
    DataSet ds2 = new DataSet1();
    DataSet ds3 = new DataSet();
    DataTable data1 = new DataTable();
    Dictionary<int, int> dicrowspanpassper = new Dictionary<int, int>();
    Dictionary<int, string> dicrowcolcriteria = new Dictionary<int, string>();
    DataTable data = new DataTable();
    //---------Page_Load Functions-------
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {


            //''------------clg left logo
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl;
            img.Width = Unit.Percentage(70);
            img.Height = Unit.Percentage(70);
            return img;

            //'-------------clg right logo
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = this.ImageUrl;
            img2.Width = Unit.Percentage(0);
            img2.Height = Unit.Percentage(70);
            return img2;

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            errmsg.Visible = false;

            if (!IsPostBack)
            {
                //--------Spread Design Format-----------

                Showgrid.Visible = false;
                btnPrintMaster.Visible = false;
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnPrint.Visible = false;
                //btnpdf.Visible = false;
                //norecordlbl.Visible = false;

                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                if (chklstdegree.Items.Count > 0)
                {
                    BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                    BindSectionDetail(strbatch, strbranch);
                    BindSem(strbranch, strbatchyear, collegecode);
                    //BindSubject(strbatch, strbranch, strsem, strsec);
                    //BindTest(strbatch, strbranch);
                    bindstaff();


                    chksection.Checked = true;
                    chksection_CheckedChanged(sender, e);
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Give degree rights to staff";

                }
                GetTest();
            }

        }
        catch (Exception ex) { }
    }

    public void GetTest() // sridharan 13 mar 2015
    {
        try
        {
            //string staffquery = "";
            //string staffyear = "";
            //string staffsection = "";
            string testbatchyear = "";
            string testsec = "";


            for (int j = 0; j < chklstbatch.Items.Count; j++)
            {
                if (chklstbatch.Items[j].Selected == true)
                {

                    if (testbatchyear == "")
                    {
                        testbatchyear = "'" + chklstbatch.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        testbatchyear = testbatchyear + "," + "'" + chklstbatch.Items[j].Value.ToString() + "'";
                    }
                }

            }
            string testdegree = "";
            for (int j = 0; j < chklstdegree.Items.Count; j++)
            {
                if (chklstdegree.Items[j].Selected == true)
                {

                    if (testdegree == "")
                    {
                        testdegree = "'" + chklstdegree.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        testdegree = testdegree + "," + "'" + chklstdegree.Items[j].Value.ToString() + "'";
                    }
                }

            }


            string testbranch = "";
            for (int j = 0; j < chklstbranch.Items.Count; j++)
            {
                if (chklstbranch.Items[j].Selected == true)
                {

                    if (testbranch == "")
                    {
                        testbranch = "'" + chklstbranch.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        testbranch = testbranch + "," + "'" + chklstbranch.Items[j].Value.ToString() + "'";
                    }

                }
            }




            for (int j = 0; j < chklstsection.Items.Count; j++)
            {
                if (chklstsection.Items[j].Selected == true)
                {

                    if (testsec == "")
                    {
                        testsec = "'" + chklstsection.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        testsec = testsec + "," + "'" + chklstsection.Items[j].Value.ToString() + "'";
                    }
                }

            }


            chktest.Checked = false;
            txttest.Text = "---Select---";

            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code in (" + testbranch + ") and semester =" + ddlsemester.SelectedValue.ToString() + " and batch_year in (" + testbatchyear + ")";
            SyllabusYr = GetFunction(SyllabusQry.ToString());
            string Sqlstr;
            Sqlstr = "";
            //  Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code in (" + testbranch + ") and semester=" + ddlsemester.SelectedValue.ToString() + " and syllabus_year='" + SyllabusYr.ToString() + "' and batch_year in (" + testbatchyear + ") order by criteria";
            Sqlstr = "select distinct criteria from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code in (" + testbranch + ") and semester=" + ddlsemester.SelectedValue.ToString() + " and syllabus_year='" + SyllabusYr.ToString() + "' and batch_year in (" + testbatchyear + ") order by criteria";
            DataSet titles = new DataSet();
            titles.Clear();
            titles.Dispose();
            titles = datest.select_method_wo_parameter(Sqlstr, "Test");
            chkltest.Items.Clear();
            if (titles.Tables[0].Rows.Count > 0)
            {
                chkltest.DataSource = titles;
                chkltest.DataTextField = "criteria";
                chkltest.DataBind();
                //  ddlTest.Items.Insert(0, new ListItem("--Select--", "-1"));


            }

        }
        catch
        {

        }

    }

    //------Load Function for the Batch Details-----

    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbatch.DataSource = ds2;
                chklstbatch.DataTextField = "Batch_year";
                chklstbatch.DataValueField = "Batch_year";
                chklstbatch.DataBind();
                chklstbatch.SelectedIndex = chklstbatch.Items.Count - 1;
                txtbatch.Text = "Batch(1)";
                //for (int i = 0; i < chklstbatch.Items.Count; i++)
                //{
                //    chklstbatch.Items[i].Selected = true;
                //    if (chklstbatch.Items[i].Selected == true)
                //    {
                //        count += 1;
                //    }
                //    if (chklstbatch.Items.Count == count)
                //    {
                //        chkbatch.Checked = true;
                //    }

                //}
                //chklstbatch.Items[].Selected = true;

            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Degree Details-----

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
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
                chklstdegree.Items[0].Selected = true;
                txtdegree.Text = "Degree(1)";
                //for (int i = 0; i < chklstdegree.Items.Count; i++)
                //{
                //    chklstdegree.Items[i].Selected = true;
                //    if (chklstdegree.Items[i].Selected == true)
                //    {
                //        count1 += 1;
                //    }
                //    if (chklstdegree.Items.Count == count1)
                //    {
                //        chkdegree.Checked = true;
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Branch Details-----

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
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
            //course_id = chklstdegree.SelectedValue.ToString();
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
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
                txtbranch.Text = "Branch(1)";
                //for (int i = 0; i < chklstbranch.Items.Count; i++)
                //{
                //    chklstbranch.Items[i].Selected = true;
                //    if (chklstbranch.Items[i].Selected == true)
                //    {
                //        count2 += 1;
                //    }
                //    if (chklstbranch.Items.Count == count2)
                //    {
                //        chkbranch.Checked = true;
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = "Please Select the Degree";
        }
    }

    //------Load Function for the Section Details-----

    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                if (chklstbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklstbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklstbatch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            //strbranch = chklstbranch.SelectedValue.ToString();

            chklstsection.Items.Clear();
            ds2.Dispose();
            ds2.Reset();

            chklstsection.Items.Insert(0, " ");
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {

                chklstsection.DataSource = ds2;
                chklstsection.DataTextField = "sections";
                chklstsection.DataBind();

                chklstsection.Items.Insert(0, "Empty");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    chklstsection.Enabled = false;
                }
                else
                {
                    chklstsection.Enabled = true;
                    chklstsection.SelectedIndex = chklstsection.Items.Count - 2;
                    //chklstsection.Items[0].Selected = true;
                    for (int i = 0; i < chklstsection.Items.Count; i++)
                    {
                        chklstsection.Items[i].Selected = true;
                        if (chklstsection.Items[i].Selected == true)
                        {
                            count3 += 1;
                        }
                        if (chklstsection.Items.Count == count3)
                        {
                            chksection.Checked = true;
                            txtsection.Text = "Section(" + count3 + ")";
                        }
                    }

                }
            }
            else
            {
                //chklstsection.Items[0].Selected = false;
                //  chklstsection.Enabled = false;
                chksection.Checked = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = " Please Select the Branch";
        }

    }

    //------Load Function for the Semester Details-----

    public void BindSem(string strbranch, string strbatchyear, string collegecode)
    {
        try
        {
            for (int j = 0; j < chklstbatch.Items.Count; j++)
            {
                if (chklstbatch.Items[j].Selected == true)
                {
                    if (strbatchyear == "")
                    {
                        strbatchyear = "'" + chklstbatch.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatchyear = strbatchyear + "," + "'" + chklstbatch.Items[j].Value.ToString() + "'";
                    }
                }
            }

            for (int j = 0; j < chklstbranch.Items.Count; j++)
            {
                if (chklstbranch.Items[j].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[j].Value.ToString() + "'";
                    }
                }
            }
            //strbatchyear = chklstbatch.Text.ToString();
            //strbranch = chklstbranch.SelectedValue.ToString();

            ddlsemester.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                int rowcount = Convert.ToInt32(ds2.Tables[0].Rows.Count);
                first_year = Convert.ToBoolean(Convert.ToString(ds2.Tables[0].Rows[rowcount - 1][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds2.Tables[0].Rows[rowcount - 1][0]).ToString());

                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //Added By srinath 12/1/13
    //------Load Function for the Staff Details-----

    public void bindstaff()
    {
        chksatff.Checked = false;
        txtstaff.Text = "---Select---";
        string staffquery = "";
        string staffyear = "";
        string staffsection = "";
        string staffyear1 = "";
        string staffsection1 = "";
        int staffcount = 0;

        for (int j = 0; j < chklstbatch.Items.Count; j++)
        {
            if (chklstbatch.Items[j].Selected == true)
            {
                staffcount++;
                if (staffyear1 == "")
                {
                    staffyear1 = "'" + chklstbatch.Items[j].Value.ToString() + "'";
                }
                else
                {
                    staffyear1 = staffyear1 + "," + "'" + chklstbatch.Items[j].Value.ToString() + "'";
                }
            }
            if (staffcount != 0)
            {
                staffyear = "and r.batch_year in (" + staffyear1 + ")";
            }
        }

        staffcount = 0;
        string staffdegree = "";
        for (int j = 0; j < chklstbranch.Items.Count; j++)
        {
            if (chklstbranch.Items[j].Selected == true)
            {
                staffcount++;
                if (staffdegree == "")
                {
                    staffdegree = "'" + chklstbranch.Items[j].Value.ToString() + "'";
                }
                else
                {
                    staffdegree = staffdegree + "," + "'" + chklstbranch.Items[j].Value.ToString() + "'";
                }

            }
        }
        if (staffcount != 0)
        {
            staffdegree = "and sy.Degree_code in (" + staffdegree + ")";
        }
        else
        {
            staffdegree = "";
        }

        staffcount = 0;
        for (int j = 0; j < chklstsection.Items.Count; j++)
        {
            if (chklstsection.Items[j].Selected == true)
            {
                staffcount++;
                if (staffsection1 == "")
                {
                    staffsection1 = "'" + chklstsection.Items[j].Value.ToString() + "'";
                }
                else
                {
                    staffsection1 = staffsection1 + "," + "'" + chklstsection.Items[j].Value.ToString() + "'";
                }
            }

        }
        if (staffcount != 0)
        {
            staffsection = "and r.sections in (" + staffsection1 + ")";
        }
        else
        {
            staffsection = "";
        }
        chklststaff.Items.Clear();
        ds2.Dispose();
        ds2.Reset();
        chklststaff.Items.Insert(0, " ");
        if (Session["Staff_Code"].ToString().Trim() != "")
        {
            staffquery = "select distinct m.staff_name,m.staff_code from staffmaster m,staff_selector r where m.staff_code=r.staff_code " + staffyear + " " + staffsection + " and m.staff_code='" + Session["Staff_Code"].ToString() + "' and m.college_code='" + Session["collegecode"].ToString() + "' ";
        }
        else
        {
            //staffquery = "select distinct m.staff_name,m.staff_code from staffmaster m,staff_selector r where m.staff_code=r.staff_code " + staffyear + " " + staffsection + " and m.college_code='" + Session["collegecode"].ToString() + "' ";
            staffquery = "select distinct m.staff_name,m.staff_code from staffmaster m,staff_selector r,syllabus_master sy,sub_sem sm,subject s where m.staff_code=r.staff_code and s.subject_no=r.subject_no and s.subType_no=sm.subType_no and sm.syll_code=sy.syll_code and sy.Batch_Year=r.batch_year " + staffyear + " " + staffdegree + " and sy.semester='" + ddlsemester.SelectedValue.ToString() + "' and m.college_code='" + Session["collegecode"].ToString() + "' ";
        }


        ds2 = d2.select_method(staffquery, hat, "Text");
        if (ds2.Tables[0].Rows.Count > 0)
        {
            chklststaff.DataSource = ds2;
            chklststaff.DataTextField = "staff_name";
            chklststaff.DataValueField = "staff_code";
            chklststaff.DataBind();
        }

    }
    //Added By srinath 12/1/13
    protected void chksatff_CheckedChanged(object sender, EventArgs e)
    {
        if (chksatff.Checked == true)
        {
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                chklststaff.Items[i].Selected = true;
                txtstaff.Text = "Staff(" + (chklststaff.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklststaff.Items.Count; i++)
            {
                chklststaff.Items[i].Selected = false;
                txtstaff.Text = "---Select---";
            }
        }
    }
    //Added By srinath 12/1/13
    protected void chklststaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        int batchcount = 0;
        string value = "";
        string code = "";
        for (int i = 0; i < chklststaff.Items.Count; i++)
        {
            if (chklststaff.Items[i].Selected == true)
            {
                value = chklststaff.Items[i].Text;
                code = chklststaff.Items[i].Value.ToString();
                batchcount = batchcount + 1;
                txtstaff.Text = "Staff(" + batchcount.ToString() + ")";
            }

        }

    }

    protected void chktest_CheckedChanged(object sender, EventArgs e)
    {
        if (chktest.Checked == true)
        {
            for (int i = 0; i < chkltest.Items.Count; i++)
            {
                chkltest.Items[i].Selected = true;
                txttest.Text = "Test(" + (chkltest.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chkltest.Items.Count; i++)
            {
                chkltest.Items[i].Selected = false;
                txttest.Text = "---Select---";
            }
        }
    }
    protected void chkltest_SelectedIndexChanged(object sender, EventArgs e)
    {
        int testcount = 0;
        for (int i = 0; i < chkltest.Items.Count; i++)
        {
            if (chkltest.Items[i].Selected == true)
            {
                testcount++;
            }
            txttest.Text = "Test(" + (testcount) + ")";
        }

    }


    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnPrintMaster.Visible = false;
            Showgrid.Visible = false;
            btnPrint.Visible = false;
            BindSectionDetail(strbatch, strbranch);
            bindstaff();
            //BindSubject(strbatch, strbranch, strsem, strsec);
            //BindTest(strbatch, strbranch);
            errmsg.Visible = false;
            chksection.Checked = true;
            chksection_CheckedChanged(sender, e);
            GetTest();// Added By Sridharan 13 Mar 2015
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //----------Batch Dropdown Extender-----------------

    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbatch.Checked == true)
        {
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                chklstbatch.Items[i].Selected = true;
                txtbatch.Text = "Batch(" + (chklstbatch.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                chklstbatch.Items[i].Selected = false;
                txtbatch.Text = "---Select---";
            }
        }
        bindstaff(); //Added By srinath 12/1/13
        GetTest();// Added By Sridharan 13 Mar 2015
    }

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pbatch.Focus();

        int batchcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstbatch.Items.Count; i++)
        {
            if (chklstbatch.Items[i].Selected == true)
            {

                value = chklstbatch.Items[i].Text;
                code = chklstbatch.Items[i].Value.ToString();
                batchcount = batchcount + 1;
                txtbatch.Text = "Batch(" + batchcount.ToString() + ")";
            }

        }

        if (batchcount == 0)
            txtbatch.Text = "---Select---";
        else
        {
            Label lbl = batchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = batchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(batchimg_Click);
        }
        batchcnt = batchcount;

        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        //BindSem(strbranch, strbatchyear, collegecode);
        //BindSectionDetail(strbatch, strbranch);
        //BindSubject(strbatch, strbranch, strsem, strsec);
        //BindTest(strbatch, strbranch);
        bindstaff(); //Added By srinath 12/1/13
        GetTest();// Added By Sridharan 13 Mar 2015
    }

    protected void LinkButtonbatch_Click(object sender, EventArgs e)
    {

        chklstbatch.ClearSelection();
        batchcnt = 0;
        txtbatch.Text = "---Select---";
    }

    public void batchimg_Click(object sender, ImageClickEventArgs e)
    {
        batchcnt = batchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstbatch.Items[r].Selected = false;

        txtbatch.Text = "Batch(" + batchcnt.ToString() + ")";
        if (txtbatch.Text == "Batch(0)")
        {
            txtbatch.Text = "---Select---";

        }

    }

    public Label batchlabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton batchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }


    //----------Degree Dropdown Extender-----------------

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdegree.Checked == true)
        {
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                chklstdegree.Items[i].Selected = true;
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                chklstdegree.Items[i].Selected = false;
                txtdegree.Text = "---Select---";
            }
        }
        GetTest();// Added By Sridharan 13 Mar 2015
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        pdegree.Focus();

        int degreecount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstdegree.Items.Count; i++)
        {
            if (chklstdegree.Items[i].Selected == true)
            {

                value = chklstdegree.Items[i].Text;
                code = chklstdegree.Items[i].Value.ToString();
                degreecount = degreecount + 1;
                txtdegree.Text = "Degree(" + degreecount.ToString() + ")";
            }

        }

        if (degreecount == 0)
            txtdegree.Text = "---Select---";
        else
        {
            Label lbl = degreelabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = degreeimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(degreeimg_Click);
        }
        degreecnt = degreecount;
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        //BindSem(strbranch, strbatchyear, collegecode);
        //BindSectionDetail(strbatch, strbranch);
        //BindSubject(strbatch, strbranch, strsem, strsec);
        //BindTest(strbatch, strbranch);
        GetTest();// Added By Sridharan 13 Mar 2015
    }

    protected void LinkButtondegree_Click(object sender, EventArgs e)
    {

        chklstdegree.ClearSelection();
        degreecnt = 0;
        txtdegree.Text = "---Select---";
    }

    public void degreeimg_Click(object sender, ImageClickEventArgs e)
    {
        degreecnt = degreecnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstdegree.Items[r].Selected = false;

        txtdegree.Text = "Degree(" + degreecnt.ToString() + ")";
        if (txtdegree.Text == "Degree(0)")
        {
            txtdegree.Text = "---Select---";

        }

    }

    public Label degreelabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton degreeimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }


    //----------Branch Dropdown Extender-----------------

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbranch.Checked == true)
        {
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                chklstbranch.Items[i].Selected = true;
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                chklstbranch.Items[i].Selected = false;
                txtbranch.Text = "---Select---";
            }
        }
        GetTest();// Added By Sridharan 13 Mar 2015
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pbranch.Focus();

        int branchcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {

                value = chklstbranch.Items[i].Text;
                code = chklstbranch.Items[i].Value.ToString();
                branchcount = branchcount + 1;
                txtbranch.Text = "Branch(" + branchcount.ToString() + ")";
            }

        }

        if (branchcount == 0)
            txtbranch.Text = "---Select---";
        else
        {
            Label lbl = branchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = branchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(branchimg_Click);
        }
        branchcnt = branchcount;

        BindSem(strbranch, strbatchyear, collegecode);
        BindSectionDetail(strbatch, strbranch);
        bindstaff();
        //BindSubject(strbatch, strbranch, strsem, strsec);
        //BindTest(strbatch, strbranch);
        GetTest();// Added By Sridharan 13 Mar 2015

    }

    protected void LinkButtonbranch_Click(object sender, EventArgs e)
    {

        chklstbranch.ClearSelection();
        branchcnt = 0;
        txtbranch.Text = "---Select---";
    }

    public void branchimg_Click(object sender, ImageClickEventArgs e)
    {
        branchcnt = branchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstbranch.Items[r].Selected = false;

        txtdegree.Text = "Branch(" + branchcnt.ToString() + ")";
        if (txtdegree.Text == "Branch(0)")
        {
            txtdegree.Text = "---Select---";

        }

    }

    public Label branchlabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton branchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }


    //----------Section Dropdown Extender-----------------

    protected void chksection_CheckedChanged(object sender, EventArgs e)
    {
        if (chksection.Checked == true)
        {
            for (int i = 0; i < chklstsection.Items.Count; i++)
            {
                chklstsection.Items[i].Selected = true;
                txtsection.Text = "Section(" + (chklstsection.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstsection.Items.Count; i++)
            {
                chklstsection.Items[i].Selected = false;
                txtsection.Text = "---Select---";
            }
        }
        bindstaff();//Added By srinath 12/1/13
        GetTest();// Added By Sridharan 13 Mar 2015
    }

    protected void chklstsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        psection.Focus();

        int sectioncount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstsection.Items.Count; i++)
        {
            if (chklstsection.Items[i].Selected == true)
            {

                value = chklstsection.Items[i].Text;
                code = chklstsection.Items[i].Value.ToString();
                sectioncount = sectioncount + 1;
                txtsection.Text = "Section(" + sectioncount.ToString() + ")";
            }

        }

        if (sectioncount == 0)
            txtsection.Text = "---Select---";
        else
        {
            Label lbl = sectionlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = sectionimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(sectionimg_Click);
        }
        sectioncnt = sectioncount;
        //BindSubject(strbatch, strbranch, strsem, strsec);
        //BindTest(strbatch, strbranch);
        bindstaff();//Added By srinath 12/1/13
        GetTest();// Added By Sridharan 13 Mar 2015
    }

    protected void LinkButtonsection_Click(object sender, EventArgs e)
    {

        chklstsection.ClearSelection();
        sectioncnt = 0;
        txtsection.Text = "---Select---";
    }

    public void sectionimg_Click(object sender, ImageClickEventArgs e)
    {
        sectioncnt = sectioncnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstsection.Items[r].Selected = false;

        txtsection.Text = "Section(" + sectioncnt.ToString() + ")";
        if (txtsection.Text == "Section(0)")
        {
            txtsection.Text = "---Select---";

        }

    }

    public Label sectionlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton sectionimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }


    //----------Criteria Dropdown Extender-----------------

    protected void chkcriteria_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcriteria.Checked == true)
        {
            for (int i = 0; i < chklstcriteria.Items.Count; i++)
            {
                chklstcriteria.Items[i].Selected = true;
                txtcriteria.Text = "Criteria(" + (chklstcriteria.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstcriteria.Items.Count; i++)
            {
                chklstcriteria.Items[i].Selected = false;
                txtcriteria.Text = "---Select---";
            }
        }
    }

    protected void chklstcriteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        pcriteria.Focus();

        int criteriacount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstcriteria.Items.Count; i++)
        {
            if (chklstcriteria.Items[i].Selected == true)
            {

                value = chklstcriteria.Items[i].Text;
                code = chklstcriteria.Items[i].Value.ToString();
                criteriacount = criteriacount + 1;
                txtcriteria.Text = "Criteria(" + criteriacount.ToString() + ")";
            }

        }

        if (criteriacount == 0)
            txtcriteria.Text = "---Select---";
        else
        {
            Label lbl = criterialabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = criteriaimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(criteriaimg_Click);
        }
        criteria_cnt = criteriacount;
        //BindSubject(strbatch, strbranch, strsem, strsec);
        //BindTest(strbatch, strbranch);

    }

    protected void LinkButtoncriteria_Click(object sender, EventArgs e)
    {

        chklstcriteria.ClearSelection();
        criteria_cnt = 0;
        txtcriteria.Text = "---Select---";
    }

    public void criteriaimg_Click(object sender, ImageClickEventArgs e)
    {
        criteria_cnt = criteria_cnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstcriteria.Items[r].Selected = false;

        txtcriteria.Text = "Criteria(" + criteria_cnt.ToString() + ")";
        if (txtcriteria.Text == "Criteria(0)")
        {
            txtcriteria.Text = "---Select---";

        }

    }

    public Label criterialabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton criteriaimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }


    //----------Subject Dropdown Extender-----------------

    //protected void chksubject_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chksubject.Checked == true)
    //    {
    //        for (int i = 0; i < chklstsubject.Items.Count; i++)
    //        {
    //            chklstsubject.Items[i].Selected = true;
    //            txtsubject.Text = "Subject(" + (chklstsubject.Items.Count) + ")";
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < chklstsubject.Items.Count; i++)
    //        {
    //            chklstsubject.Items[i].Selected = false;
    //            txtsubject.Text = "---Select---";
    //        }
    //    }
    //}

    //protected void chklstsubject_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    psubject.Focus();

    //    int subjectcount = 0;
    //    string value = "";
    //    string code = "";


    //    for (int i = 0; i < chklstsubject.Items.Count; i++)
    //    {
    //        if (chklstsubject.Items[i].Selected == true)
    //        {

    //            value = chklstsubject.Items[i].Text;
    //            code = chklstsubject.Items[i].Value.ToString();
    //            subjectcount = subjectcount + 1;
    //            txtsubject.Text = "Subject(" + subjectcount.ToString() + ")";
    //        }

    //    }

    //    if (subjectcount == 0)
    //        txtsubject.Text = "---Select---";
    //    else
    //    {
    //        Label lbl = subjectlabel();
    //        lbl.Text = " " + value + " ";
    //        lbl.ID = "lbl1-" + code.ToString();
    //        ImageButton ib = subjectimage();
    //        ib.ID = "imgbut1_" + code.ToString();
    //        ib.Click += new ImageClickEventHandler(subjectimg_Click);
    //    }
    //    subjectcnt = subjectcount;
    //    BindTest(strbatch, strbranch);

    //}

    //protected void LinkButtonsubject_Click(object sender, EventArgs e)
    //{

    //    chklstsubject.ClearSelection();
    //    subjectcnt = 0;
    //    txtsubject.Text = "---Select---";
    //}

    //public void subjectimg_Click(object sender, ImageClickEventArgs e)
    //{
    //    subjectcnt = subjectcnt - 1;
    //    ImageButton b = sender as ImageButton;
    //    int r = Convert.ToInt32(b.CommandArgument);
    //    chklstsubject.Items[r].Selected = false;

    //    txtsubject.Text = "Subject(" + sectioncnt.ToString() + ")";
    //    if (txtsubject.Text == "Subject(0)")
    //    {
    //        txtsubject.Text = "---Select---";

    //    }

    //}

    //public Label subjectlabel()
    //{
    //    Label lbc = new Label();

    //    ViewState["lseatcontrol"] = true;
    //    return (lbc);
    //}

    //public ImageButton subjectimage()
    //{
    //    ImageButton imc = new ImageButton();
    //    imc.ImageUrl = "xb.jpeg";
    //    imc.Height = 9;
    //    imc.Width = 9;
    //    ViewState["iseatcontrol"] = true;
    //    return (imc);
    //}

    //public object GetCorrespondingKey(object key, Hashtable hashTable)
    //{

    //    IDictionaryEnumerator e = hashTable.GetEnumerator();
    //    while (e.MoveNext())
    //    {
    //        if (e.Key.ToString() == key.ToString())
    //        {
    //            return e.Value.ToString();
    //        }
    //    }

    //    return null;
    //}

    //------Method for the Go Button -----

    protected void btngo_Click(object sender, EventArgs e)
    {
        btnPrint11();
        if (chklstdegree.Items.Count > 0)
        {
            errmsg.Text = "";
            errmsg.Visible = false;
            gobutton();
        }
        else
        {
            errmsg.Text = "Give degree rights to staff";
            errmsg.Visible = true;
            return;
        }

    }
    public string GetFunction(string Att_strqueryst)
    {

        string sqlstr;
        sqlstr = Att_strqueryst;
        getsql.Close();
        getsql.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, getsql);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = getsql;
        drnew = cmd.ExecuteReader();
        drnew.Read();

        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }
    public void gobutton()
    {
        errmsg.Visible = false;

        try
        {
            string temp = string.Empty;
            string temp1 = string.Empty;
            string temp2 = string.Empty;
            string strcritno = string.Empty;
            string strsubno = string.Empty;
            string strsyllcode = string.Empty;
            string str_section = string.Empty;
            string str_section1 = string.Empty;
            string str_section2 = string.Empty;
            string strcriteriacnt = string.Empty;
            string strcriteriano = string.Empty;
            string strcriteriano1 = string.Empty;

            int rowcount = 0;
            int ini_column = 0;
            int no_column = 0;
            int sectioncnt = 0;
            int subjectcount = 1;
            int subcnt = 0;
            int subnocnt = 0;
            int cnt = 0;
            int slnocnt = 0;
            int cricount = 0;
            int minmark = 0;
            int colcount = 0;
            int eopcount = 0;
            int colno = 0;
            int rowno = 0;
            int passrowno = 0;
            int dscount = 0;
            int critestcnt = 0;
            int columcnt = 0;
            int ctcnt = 0;


            Boolean headerrow = false;
            double failure = 0;
            double expnopass = 0;
            double passpercount = 0;
            double exppasspercen = 0;
            double exppasspercen_round = 0;
            double expfailpercen = 0;
            double expfailpercen_round = 0;
            Dictionary<int, string> dicriteriano = new Dictionary<int, string>();
            Dictionary<int, string> dicsyssecno = new Dictionary<int, string>();
            Dictionary<int, string> dicsecno = new Dictionary<int, string>();
            Dictionary<int, string> dicmaxmark = new Dictionary<int, string>();
            Dictionary<int, string> dicriteriano1 = new Dictionary<int, string>();
            Dictionary<int, string> diccolspan = new Dictionary<int, string>();
            Dictionary<int, string> dicrowspanpass = new Dictionary<int, string>();
            Dictionary<string, string> dicrowcolcrit = new Dictionary<string, string>();
            int colcunt = 2;
            int colcunt1 = -1;
            int rowcnt = -1;
            int criteriarowcnt = 2;
            int subjcnt = 0;
            int totlcolumncount = 0;
            int totcolumncount = 0;
            data.Clear();
            System.Text.StringBuilder criteria = new System.Text.StringBuilder();
            DataRow drow;

            ArrayList arrColHdrNames1 = new ArrayList();
            ArrayList arrColHdrNames2 = new ArrayList();
            ArrayList arrColHdrNames3 = new ArrayList();


            arrColHdrNames1.Add("S.No");
            arrColHdrNames2.Add("S.No");
            arrColHdrNames3.Add("S.No");
            arrColHdrNames1.Add("Subject - Subject Code");
            arrColHdrNames2.Add("Subject - Subject Code");
            arrColHdrNames3.Add("Subject - Subject Code");
            arrColHdrNames1.Add("Name of the Staff");
            arrColHdrNames2.Add("Name of the Staff");
            arrColHdrNames3.Add("Name of the Staff");
            data.Columns.Add("SNo", typeof(string));
            data.Columns.Add("Subject - Subject Code", typeof(string));
            data.Columns.Add("Name of the Staff", typeof(string));

            DataSet dstable = new DataSet();
            // datasettable.Tables.Add();
            strsem = ddlsemester.SelectedValue.ToString();


            Showgrid.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnPrintMaster.Visible = false;
            btnPrint.Visible = false;

            if (chklstcriteria.Items[0].Selected == true)
            {
                criteria_count++;
            }
            if (chklstcriteria.Items[1].Selected == true)
            {
                criteria_count++;
            }
            if (chklstcriteria.Items[2].Selected == true)
            {
                criteria_count++;
            }
            if (chklstcriteria.Items[3].Selected == true)
            {
                criteria_count++;
            }
            if (chklstcriteria.Items[4].Selected == true)
            {
                criteria_count++;
            }
            if (chklstcriteria.Items[5].Selected == true)
            {
                criteria_count++;
            }
            if (criteria_count != 0)//Added By srinath 12/1/13
            {
                int testcounterr = 0;
                for (int i = 0; i < chkltest.Items.Count; i++)
                {
                    if (chkltest.Items[i].Selected == true)
                    {
                        testcounterr++;
                    }

                }
                if (testcounterr == 0)
                {
                    testerrmsg.Text = "Please Select Test";
                    return;
                }
                else
                {
                    testerrmsg.Text = "";

                }
                string strstaffcode = ""; //Added By srinath 12/1/13
                string staffvalue = "";
                for (int staff = 0; staff < chklststaff.Items.Count; staff++)
                {
                    if (chklststaff.Items[staff].Selected == true)
                    {
                        staffvalue = chklststaff.Items[staff].Value.ToString();
                        if (strstaffcode == "")
                        {
                            strstaffcode = "'" + staffvalue + "'";
                        }
                        else
                        {
                            strstaffcode = " " + strstaffcode + ",'" + staffvalue + "'";
                        }

                    }
                }
                if (strstaffcode != "")
                {
                    strstaffcode = "and s.staff_code in (" + strstaffcode + ")";
                }

                for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                {
                    if (chklstbatch.Items[batch].Selected == true)
                    {
                        strbatch = chklstbatch.Items[batch].Value.ToString();

                        for (int branch = 0; branch < chklstbranch.Items.Count; branch++)
                        {
                            string checksec = "";
                            if (chklstbranch.Items[branch].Selected == true)
                            {
                                strbranch = chklstbranch.Items[branch].Value.ToString();
                                strbranchname = chklstbranch.Items[branch].Text.ToString();
                                //strsec = "";
                                checksec = GetFunction("select distinct isnull(Sections,'') as Sections from registration where batch_year='" + strbatch.ToString() + "' and degree_code='" + strbranch.ToString() + "' and sections <> '' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'");
                                if (checksec != "" && checksec != null)
                                {
                                    //chklstsection.Items[0].Selected = false;
                                    for (int ss = 1; ss <= chklstsection.Items.Count - 1; ss++)
                                    {

                                        // chklstsection.Items[ss].Selected = true;
                                    }

                                }
                                else
                                {
                                    //chklstsection.Items[0].Selected = true;
                                    for (int ss = 1; ss <= chklstsection.Items.Count - 1; ss++)
                                    {
                                        // chklstsection.Items[ss].Selected = false;

                                    }

                                }
                                int first = 0;
                                int getcoulmn = 0;
                                //chklstsection.Items[0].Selected = true;

                                for (int section = 0; section <= chklstsection.Items.Count - 1; section++)
                                {
                                    slnocnt = 0;

                                    dicsecno.Clear();
                                    colcunt = 2;
                                    colcunt1 = -1;
                                    rowcnt = -1;
                                    criteriarowcnt = 2;
                                    subjcnt = 0;
                                    totlcolumncount = 0;
                                    totcolumncount = 0;

                                    if (chklstsection.Items[section].Selected == true)
                                    {
                                        //if (checksec != "" && checksec != null)
                                        //{

                                        //}
                                        string se = chklstsection.Items[section].Value;
                                        first++;
                                        if (se == "Empty" && checksec != "")
                                        {
                                            goto l1;
                                        }
                                        string vsection = "";
                                        strsec = "";

                                        if (chklstsection.Items[section].Text.ToString().Trim() != "")
                                        {
                                            strsec = "'" + chklstsection.Items[section].Value.ToString() + "'";
                                        }
                                        vsection = strsec;
                                        if (vsection == "" || vsection == null)
                                        {
                                            vsection = "";
                                        }
                                        else if (strsec == "' '")
                                        {
                                            vsection = "and sections=''''";
                                        }
                                        else
                                        {
                                            vsection = "and sections=" + strsec + "";
                                        }

                                        string strv = "select  count(*) from registration where degree_code='" + strbranch + "' and batch_year='" + strbatch + "' and  RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' " + vsection + "";
                                        string strsecsql = d2.GetFunctionv(strv);

                                        int ck = 0;
                                        string staffsections = "";
                                        if (Convert.ToInt32(strsecsql) > 0)
                                        {
                                            if (chklstsection.Items.Count > 0)
                                            {


                                                if (strsec == "" || strsec == "' '")
                                                {
                                                    strsection = "";
                                                    strsection1 = "";
                                                    strsection2 = "";
                                                }
                                                else
                                                {
                                                    strsection = "and registration.sections = " + strsec + " ";
                                                    strsection1 = "and sections= " + strsec + " ";
                                                    strsection2 = "and exam_type.sections= " + strsec + " ";
                                                    staffsections = " and s.Sections=" + strsec + " ";
                                                }
                                            }
                                            int coltestcnt = 3;
                                            if (sectioncnt > 0 && data.Rows.Count != 4)
                                            {
                                                if (headerrow)
                                                {
                                                    drow = data.NewRow();
                                                    data.Rows.Add(drow);
                                                    drow = data.NewRow();
                                                    drow["SNo"] = "S.No";
                                                    drow["Subject - Subject Code"] = "Subject - Subject Code";
                                                    drow["Name of the Staff"] = "Name of the Staff";
                                                    data.Rows.Add(drow);
                                                    drow = data.NewRow();
                                                    data.Rows.Add(drow);
                                                    drow = data.NewRow();

                                                    data.Rows.Add(drow);
                                                }
                                            }

                                            if (strbatch != "" && strbranch != "" && strsem != "")
                                            {
                                                ds.Dispose();
                                                ds.Reset();
                                                string strsql = "select syll_code from syllabus_master where degree_code='" + strbranch + "' and semester ='" + strsem + "' and batch_year='" + strbatch + "'";
                                                ds = d2.select_method(strsql, hat, "Text");
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    syllcode = ds.Tables[0].Rows[0]["syll_code"].ToString();
                                                    if (syllcode != "")
                                                    {
                                                        ds1.Dispose();
                                                        ds1.Reset();

                                                        string test = "select distinct criteria,c.criteria_no,min(isnull(e.min_mark,0)) as min_mark from criteriaforinternal c,exam_type e where c.criteria_no=e.criteria_no and c.syll_code = " + syllcode + " group by criteria,c.criteria_no order by criteria ";

                                                        ds1 = d2.select_method(test, hat, "Text");
                                                        string testcheck = "";
                                                        for (int j = 0; j < chkltest.Items.Count; j++)
                                                        {
                                                            if (chkltest.Items[j].Selected == true)
                                                            {
                                                                coltestcnt++;
                                                                if (testcheck == "")
                                                                {
                                                                    testcheck = "'" + chkltest.Items[j].Text.ToString() + "'";
                                                                }
                                                                else
                                                                {
                                                                    testcheck = testcheck + "," + "'" + chkltest.Items[j].Text.ToString() + "'";
                                                                }

                                                            }
                                                        }

                                                        DataView dvfortest = new DataView();
                                                        ds1.Tables[0].DefaultView.RowFilter = "criteria in (" + testcheck + ")";
                                                        dvfortest = ds1.Tables[0].DefaultView;
                                                        int ttsri = dvfortest.Count;
                                                        DataTable testdt = new DataTable();

                                                        testdt = dvfortest.ToTable();

                                                        ds1.Clear();

                                                        ds1.Tables.Remove(testdt.TableName);
                                                        ds1.Tables.Add(testdt);
                                                        if (ds1.Tables[0].Rows.Count > 0)
                                                        {
                                                            ck = 1;
                                                            if (criteria_count == 0)
                                                            {
                                                                criteria_count = 1;

                                                            }
                                                            cricount = (ds1.Tables[0].Rows.Count * criteria_count) + 3;
                                                            criteriacnt = Convert.ToInt32(ds1.Tables[0].Rows.Count * 0.50);

                                                            ini_column = 3;
                                                            no_column = 0;
                                                            colno = 2;
                                                            rowno = 0;
                                                            passrowno = 0;
                                                            dscount = 0;
                                                            critestcnt = 0;
                                                            int passpercol = 0;
                                                            int critcol = 2;
                                                            if (!headerrow)
                                                            {
                                                                for (int crit = 0; crit < ds1.Tables[0].Rows.Count; crit++)
                                                                {

                                                                    headerrow = true;
                                                                    columcnt = 0;
                                                                    columcnt = data.Columns.Count - 3;
                                                                    ctcnt = 0;
                                                                    ctcnt = criteria_count - 1;
                                                                    dscount = ds1.Tables[0].Rows.Count * criteria_count;

                                                                    if (crit == 0)
                                                                    {
                                                                        strcriteriano = "'" + ds1.Tables[0].Rows[crit]["criteria_no"].ToString() + "'";
                                                                    }
                                                                    else
                                                                    {
                                                                        strcriteriano = strcriteriano + "," + "'" + ds1.Tables[0].Rows[crit]["criteria_no"].ToString() + "'";
                                                                    }

                                                                    if (columcnt < dscount)
                                                                    {

                                                                        colno = (data.Columns.Count) - criteria_count;
                                                                        rowno = 1;
                                                                        passrowno = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (crit == 0)
                                                                        {
                                                                            colno++;
                                                                        }
                                                                        else
                                                                        {
                                                                            colno = colno + criteria_count;
                                                                        }

                                                                    }

                                                                    if (sectioncnt > 0 && data.Rows.Count != 4 && colno == data.Columns.Count - criteria_count)
                                                                    {
                                                                        if (crit == 0)
                                                                        {
                                                                            colno = 2;
                                                                            colno++;
                                                                        }
                                                                        else
                                                                        {
                                                                            //   colno = colno + criteria_count;

                                                                        }

                                                                    }


                                                                    {

                                                                        colcunt++;
                                                                        rowcnt++;

                                                                        dicriteriano.Add(colcunt, ds1.Tables[0].Rows[crit]["criteria_no"].ToString());
                                                                        if (dicsecno.ContainsKey(rowcnt))
                                                                        {
                                                                            dicsecno.Remove(rowcnt);
                                                                            dicsecno.Add(rowcnt, strsection + '-' + strsection2);
                                                                        }
                                                                        else
                                                                        {
                                                                            dicsecno.Add(rowcnt, strsection + '-' + strsection2);
                                                                        }
                                                                        if (ds1.Tables[0].Rows[crit]["min_mark"] != "NULL" && ds1.Tables[0].Rows[crit]["min_mark"] != " ")
                                                                        {
                                                                            if (dicmaxmark.ContainsKey(rowno))
                                                                            {
                                                                                dicmaxmark.Remove(rowno);
                                                                                dicmaxmark.Add(rowno, Convert.ToString(ds1.Tables[0].Rows[crit]["min_mark"]));

                                                                            }
                                                                            else
                                                                            {
                                                                                dicmaxmark.Add(rowno, Convert.ToString(ds1.Tables[0].Rows[crit]["min_mark"]));

                                                                            }
                                                                        }

                                                                        critestcnt = colno;
                                                                    }
                                                                    int colspan = 0;
                                                                    {

                                                                        for (int cmncnt = ctcnt; cmncnt < criteria_count; cmncnt++)
                                                                        {
                                                                            headerrow = true;
                                                                            if (chklstcriteria.Items[0].Selected == true)
                                                                            {
                                                                                arrColHdrNames1.Add("Pass Percentage");
                                                                                arrColHdrNames2.Add(ds1.Tables[0].Rows[crit]["criteria"].ToString());
                                                                                arrColHdrNames3.Add("Pass Count");

                                                                                criteria = new System.Text.StringBuilder("Pass Count");

                                                                                AddTableColumn(data, criteria);
                                                                                totlcolumncount++;
                                                                                critestcnt++;
                                                                                no_column = no_column + 1;
                                                                                passpercol++;
                                                                                colspan++;
                                                                            }
                                                                            if (chklstcriteria.Items[1].Selected == true)
                                                                            {
                                                                                arrColHdrNames1.Add("Pass Percentage");
                                                                                arrColHdrNames2.Add(ds1.Tables[0].Rows[crit]["criteria"].ToString());
                                                                                arrColHdrNames3.Add("Pass %");
                                                                                criteria = new System.Text.StringBuilder("Pass %");

                                                                                AddTableColumn(data, criteria);
                                                                                critestcnt++;
                                                                                totlcolumncount++;
                                                                                no_column = no_column + 1;
                                                                                passpercol++;
                                                                                colspan++;
                                                                            }
                                                                            if (chklstcriteria.Items[2].Selected == true)
                                                                            {
                                                                                arrColHdrNames1.Add("Pass Percentage");
                                                                                arrColHdrNames2.Add(ds1.Tables[0].Rows[crit]["criteria"].ToString());
                                                                                arrColHdrNames3.Add("Fail Count");

                                                                                criteria = new System.Text.StringBuilder("Fail Count");

                                                                                AddTableColumn(data, criteria);
                                                                                critestcnt++;
                                                                                no_column = no_column + 1;
                                                                                totlcolumncount++;
                                                                                passpercol++;
                                                                                colspan++;
                                                                            }
                                                                            if (chklstcriteria.Items[3].Selected == true)
                                                                            {
                                                                                arrColHdrNames1.Add("Pass Percentage");
                                                                                arrColHdrNames2.Add(ds1.Tables[0].Rows[crit]["criteria"].ToString());

                                                                                arrColHdrNames3.Add("Fail %");
                                                                                criteria = new System.Text.StringBuilder("Fail %");
                                                                                totlcolumncount++;
                                                                                AddTableColumn(data, criteria);
                                                                                critestcnt++;
                                                                                no_column = no_column + 1;
                                                                                passpercol++;
                                                                                colspan++;
                                                                            }
                                                                            if (chklstcriteria.Items[4].Selected == true)
                                                                            {
                                                                                arrColHdrNames1.Add("Pass Percentage");
                                                                                arrColHdrNames2.Add(ds1.Tables[0].Rows[crit]["criteria"].ToString());
                                                                                arrColHdrNames3.Add("Absent Count");
                                                                                criteria = new System.Text.StringBuilder("Absent Count");

                                                                                AddTableColumn(data, criteria);
                                                                                totlcolumncount++;

                                                                                critestcnt++;
                                                                                no_column = no_column + 1;
                                                                                passpercol++;
                                                                                colspan++;
                                                                            }
                                                                            if (chklstcriteria.Items[5].Selected == true)
                                                                            {
                                                                                arrColHdrNames1.Add("Pass Percentage");
                                                                                arrColHdrNames2.Add(ds1.Tables[0].Rows[crit]["criteria"].ToString());
                                                                                arrColHdrNames3.Add("Absent %");
                                                                                criteria = new System.Text.StringBuilder("Absent %");

                                                                                AddTableColumn(data, criteria);

                                                                                totlcolumncount++;
                                                                                critestcnt++;
                                                                                no_column = no_column + 1;
                                                                                passpercol++;
                                                                                colspan++;
                                                                            }
                                                                        }



                                                                    }
                                                                    string criteria1 = ds1.Tables[0].Rows[crit]["criteria"].ToString() + '-' + Convert.ToInt32(colspan);
                                                                    dicrowcolcriteria.Add(crit, criteria1);
                                                                }
                                                                dicrowspanpassper.Add(4, passpercol);
                                                                if (no_column != 0)
                                                                {
                                                                    criteriarowcnt++;
                                                                    dicriteriano1.Add(criteriarowcnt, criteriacnt.ToString());
                                                                    // FpSpread1.Sheets[0].Cells[passrowno, ini_column].Text = "Pass Percentage";

                                                                    // FpSpread1.Sheets[0].Cells[passrowno, ini_column].Tag = strcriteriano;
                                                                }
                                                                if (first == 1)
                                                                {
                                                                    getcoulmn = data.Columns.Count;
                                                                }
                                                                if (chklstcriteria.Items[6].Selected == true)
                                                                {
                                                                    if (cricount >= data.Columns.Count)
                                                                    {
                                                                        arrColHdrNames1.Add("Expected No of Pass");
                                                                        arrColHdrNames2.Add("Expected No of Pass");
                                                                        arrColHdrNames3.Add("Expected No of Pass");
                                                                        data.Columns.Add("Expected No of Pass", typeof(string));
                                                                        totcolumncount++;
                                                                        cricount++;
                                                                    }
                                                                    else
                                                                    {
                                                                        arrColHdrNames1.Add("Expected No of Pass");
                                                                        arrColHdrNames2.Add("Expected No of Pass");
                                                                        arrColHdrNames3.Add("Expected No of Pass");
                                                                        data.Columns.Add("Expected No of Pass", typeof(string));
                                                                        totcolumncount++;
                                                                        cricount++;
                                                                    }
                                                                }

                                                                if (chklstcriteria.Items[7].Selected == true)
                                                                {
                                                                    if (cricount >= data.Columns.Count)
                                                                    {
                                                                        arrColHdrNames1.Add("Expected Pass Percentage");
                                                                        arrColHdrNames2.Add("Expected Pass Percentage");
                                                                        arrColHdrNames3.Add("Expected Pass Percentage");
                                                                        data.Columns.Add("Expected Pass Percentage", typeof(string));
                                                                        totcolumncount++;

                                                                        cricount++;
                                                                    }
                                                                    else
                                                                    {
                                                                        arrColHdrNames1.Add("Expected Pass Percentage");
                                                                        arrColHdrNames2.Add("Expected Pass Percentage");
                                                                        arrColHdrNames3.Add("Expected Pass Percentage");
                                                                        data.Columns.Add("Expected Pass Percentage", typeof(string));
                                                                        totcolumncount++;

                                                                        cricount++;
                                                                    }
                                                                }

                                                                if (chklstcriteria.Items[8].Selected == true)
                                                                {
                                                                    if (cricount >= data.Columns.Count)
                                                                    {
                                                                        arrColHdrNames1.Add("Expected No of Failures");
                                                                        arrColHdrNames2.Add("Expected No of Failures");
                                                                        arrColHdrNames3.Add("Expected No of Failures");
                                                                        data.Columns.Add("Expected No of Failures", typeof(string));
                                                                        totcolumncount++;

                                                                        cricount++;
                                                                    }
                                                                    else
                                                                    {
                                                                        arrColHdrNames1.Add("Expected No of Failures");
                                                                        arrColHdrNames2.Add("Expected No of Failures");
                                                                        arrColHdrNames3.Add("Expected No of Failures");
                                                                        data.Columns.Add("Expected No of Failures", typeof(string));
                                                                        totcolumncount++;
                                                                        cricount++;
                                                                    }
                                                                }

                                                                if (chklstcriteria.Items[9].Selected == true)
                                                                {
                                                                    if (cricount >= data.Columns.Count)
                                                                    {
                                                                        arrColHdrNames1.Add("Expected Fail Percentage");
                                                                        arrColHdrNames2.Add("Expected Fail Percentage");
                                                                        arrColHdrNames3.Add("Expected Fail Percentage");
                                                                        data.Columns.Add("Expected Fail Percentage", typeof(string));
                                                                        totcolumncount++;

                                                                        cricount++;
                                                                    }
                                                                    else
                                                                    {
                                                                        arrColHdrNames1.Add("Expected Fail Percentage");
                                                                        arrColHdrNames2.Add("Expected Fail Percentage");
                                                                        arrColHdrNames3.Add("Expected Fail Percentage");
                                                                        data.Columns.Add("Expected Fail Percentage", typeof(string));
                                                                        totcolumncount++;
                                                                        cricount++;
                                                                    }
                                                                }
                                                                if (chklstcriteria.Items[10].Selected == true)
                                                                {
                                                                    if (cricount >= data.Columns.Count)
                                                                    {
                                                                        arrColHdrNames1.Add("Total Strength");
                                                                        arrColHdrNames2.Add("Total Strength");
                                                                        arrColHdrNames3.Add("Total Strength");
                                                                        data.Columns.Add("Total Strength", typeof(string));
                                                                        totcolumncount++;
                                                                        cricount++;
                                                                    }
                                                                    else
                                                                    {
                                                                        arrColHdrNames1.Add("Total Strength");
                                                                        arrColHdrNames2.Add("Total Strength");
                                                                        arrColHdrNames3.Add("Total Strength");
                                                                        data.Columns.Add("Total Strength", typeof(string));
                                                                        totcolumncount++;
                                                                        cricount++;
                                                                    }
                                                                }
                                                                if (sectioncnt > 0 && data.Rows.Count != 4)
                                                                {
                                                                    cnt += 5;
                                                                }

                                                                //
                                                                DataRow drHdr1 = data.NewRow();
                                                                DataRow drHdr2 = data.NewRow();
                                                                DataRow drHdr3 = data.NewRow();
                                                                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                                                                {
                                                                    drHdr1[grCol] = arrColHdrNames1[grCol];
                                                                    drHdr2[grCol] = arrColHdrNames2[grCol];
                                                                    drHdr3[grCol] = arrColHdrNames3[grCol];

                                                                }

                                                                data.Rows.Add(drHdr1);
                                                                data.Rows.Add(drHdr2);
                                                                data.Rows.Add(drHdr3);
                                                            }
                                                            else
                                                            {
                                                                int passcolspan = 0;
                                                                int critcolspan = 0;
                                                                int rowcolumntext = 3;
                                                                int rowcolumntxt = 3;
                                                                int rowtxtcnt = 0;
                                                                int startcol = 0;
                                                                data.Rows[data.Rows.Count - 3][3] = "Pass Percentage";

                                                                for (int crit = 0; crit < ds1.Tables[0].Rows.Count; crit++)
                                                                {
                                                                    rowcolumntxt = rowcolumntxt - 1;
                                                                    columcnt = 0;
                                                                    critcolspan = 0;
                                                                    columcnt = data.Columns.Count - 3;
                                                                    ctcnt = 0;
                                                                    startcol = 0;
                                                                    ctcnt = criteria_count - 1;
                                                                    dscount = ds1.Tables[0].Rows.Count * criteria_count;

                                                                    if (crit == 0)
                                                                    {
                                                                        strcriteriano = "'" + ds1.Tables[0].Rows[crit]["criteria_no"].ToString() + "'";
                                                                    }
                                                                    else
                                                                    {
                                                                        strcriteriano = strcriteriano + "," + "'" + ds1.Tables[0].Rows[crit]["criteria_no"].ToString() + "'";
                                                                    }

                                                                    if (columcnt < dscount)
                                                                    {

                                                                        colno = (data.Columns.Count) - criteria_count;
                                                                        rowno = 1;
                                                                        passrowno = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (crit == 0)
                                                                        {
                                                                            colno++;
                                                                        }
                                                                        else
                                                                        {
                                                                            colno = colno + criteria_count;
                                                                        }

                                                                    }

                                                                    if (sectioncnt > 0 && data.Rows.Count != 4 && colno == data.Columns.Count - criteria_count)
                                                                    {
                                                                        if (crit == 0)
                                                                        {
                                                                            colno = 2;
                                                                            colno++;
                                                                        }
                                                                        else
                                                                        {
                                                                            //   colno = colno + criteria_count;

                                                                        }

                                                                    }

                                                                    if (dicsecno.ContainsKey(0))
                                                                    {
                                                                        dicsecno.Remove(0);
                                                                        dicsecno.Add(0, strsection + '-' + strsection2);
                                                                    }
                                                                    else
                                                                    {
                                                                        dicsecno.Add(0, strsection + '-' + strsection2);
                                                                    }




                                                                    data.Rows[data.Rows.Count - 2][rowcolumntext] = ds1.Tables[0].Rows[crit]["criteria"].ToString();

                                                                    for (int cmncnt = ctcnt; cmncnt < criteria_count; cmncnt++)
                                                                    {

                                                                        rowcolumntxt++;
                                                                        if (chklstcriteria.Items[0].Selected == true)
                                                                        {

                                                                            data.Rows[data.Rows.Count - 1][rowcolumntxt] = "Pass Count";
                                                                            passcolspan++;
                                                                            rowcolumntxt++;
                                                                            critcolspan++;
                                                                        }
                                                                        if (chklstcriteria.Items[1].Selected == true)
                                                                        {
                                                                            data.Rows[data.Rows.Count - 1][rowcolumntxt] = "Pass %";
                                                                            passcolspan++;
                                                                            rowcolumntxt++;
                                                                            critcolspan++;
                                                                        }

                                                                        if (chklstcriteria.Items[2].Selected == true)
                                                                        {
                                                                            data.Rows[data.Rows.Count - 1][rowcolumntxt] = "Fail Count";
                                                                            passcolspan++;
                                                                            rowcolumntxt++;
                                                                            critcolspan++;
                                                                        }
                                                                        if (chklstcriteria.Items[3].Selected == true)
                                                                        {
                                                                            data.Rows[data.Rows.Count - 1][rowcolumntxt] = "Fail %";
                                                                            passcolspan++;
                                                                            rowcolumntxt++;
                                                                            critcolspan++;
                                                                        }
                                                                        if (chklstcriteria.Items[4].Selected == true)
                                                                        {
                                                                            data.Rows[data.Rows.Count - 1][rowcolumntxt] = "Absent Count";
                                                                            passcolspan++;
                                                                            rowcolumntxt++;
                                                                            critcolspan++;
                                                                        }
                                                                        if (chklstcriteria.Items[5].Selected == true)
                                                                        {
                                                                            data.Rows[data.Rows.Count - 1][rowcolumntxt] = "Absent %";
                                                                            passcolspan++;
                                                                            rowcolumntxt++;
                                                                            critcolspan++;
                                                                        }

                                                                    }
                                                                    string colspan = Convert.ToString(data.Rows.Count - 2) + '-' + rowcolumntext.ToString() + '-' + critcolspan.ToString();
                                                                    if (dicrowcolcrit.ContainsKey(ds1.Tables[0].Rows[crit]["criteria"].ToString()))
                                                                    {

                                                                        dicrowcolcrit.Remove(ds1.Tables[0].Rows[crit]["criteria"].ToString());
                                                                        dicrowcolcrit.Add(ds1.Tables[0].Rows[crit]["criteria"].ToString(), colspan);
                                                                    }
                                                                    else
                                                                    {
                                                                        dicrowcolcrit.Add(ds1.Tables[0].Rows[crit]["criteria"].ToString(), colspan);
                                                                    }
                                                                    rowcolumntext = rowcolumntext + rowcolumntxt - 3;
                                                                }


                                                                int colcnttext = rowcolumntxt;
                                                                startcol = rowcolumntxt;
                                                                if (chklstcriteria.Items[6].Selected == true)
                                                                {
                                                                    if (cricount >= data.Columns.Count)
                                                                    {
                                                                        data.Rows[data.Rows.Count - 3][rowcolumntxt] = "Expected No of Pass";
                                                                        rowcolumntxt++;
                                                                        rowtxtcnt++;
                                                                        colcnttext++;
                                                                    }
                                                                    else
                                                                    {
                                                                        data.Rows[data.Rows.Count - 3][rowcolumntxt] = "Expected No of Pass";
                                                                        rowcolumntxt++;
                                                                        rowtxtcnt++;
                                                                        colcnttext++;
                                                                    }
                                                                }

                                                                if (chklstcriteria.Items[7].Selected == true)
                                                                {
                                                                    if (cricount >= data.Columns.Count)
                                                                    {
                                                                        data.Rows[data.Rows.Count - 3][rowcolumntxt] = "Expected Pass Percentage";
                                                                        rowcolumntxt++;
                                                                        rowtxtcnt++;
                                                                        colcnttext++;
                                                                    }
                                                                    else
                                                                    {
                                                                        data.Rows[data.Rows.Count - 3][rowcolumntxt] = "Expected Pass Percentage";
                                                                        rowcolumntxt++;
                                                                        rowtxtcnt++;
                                                                        colcnttext++;
                                                                    }
                                                                }

                                                                if (chklstcriteria.Items[8].Selected == true)
                                                                {
                                                                    if (cricount >= data.Columns.Count)
                                                                    {
                                                                        data.Rows[data.Rows.Count - 3][rowcolumntxt] = "Expected No of Failures";
                                                                        rowcolumntxt++;
                                                                        rowtxtcnt++;
                                                                        colcnttext++;
                                                                    }
                                                                    else
                                                                    {
                                                                        data.Rows[data.Rows.Count - 3][rowcolumntxt] = "Expected No of Failures";
                                                                        rowcolumntxt++;
                                                                        rowtxtcnt++;
                                                                        colcnttext++;
                                                                    }
                                                                }

                                                                if (chklstcriteria.Items[9].Selected == true)
                                                                {
                                                                    if (cricount >= data.Columns.Count)
                                                                    {
                                                                        data.Rows[data.Rows.Count - 3][rowcolumntxt] = "Expected Fail Percentage";
                                                                        rowtxtcnt++;
                                                                        rowcolumntxt++;
                                                                        colcnttext++;
                                                                    }
                                                                    else
                                                                    {
                                                                        data.Rows[data.Rows.Count - 3][rowcolumntxt] = "Expected Fail Percentage";
                                                                        rowcolumntxt++;
                                                                        rowtxtcnt++;
                                                                        colcnttext++;
                                                                    }
                                                                }
                                                                if (chklstcriteria.Items[10].Selected == true)
                                                                {
                                                                    if (cricount >= data.Columns.Count)
                                                                    {
                                                                        data.Rows[data.Rows.Count - 3][rowcolumntxt] = "Total Strength";
                                                                        rowtxtcnt++;
                                                                        rowcolumntxt++;
                                                                        colcnttext++;
                                                                    }
                                                                    else
                                                                    {
                                                                        data.Rows[data.Rows.Count - 3][rowcolumntxt] = "Total Strength";
                                                                        rowtxtcnt++;
                                                                        rowcolumntxt++;
                                                                        colcnttext++;
                                                                    }
                                                                }



                                                                string rowspancnt = passcolspan.ToString() + '-' + startcol.ToString() + '-' + rowtxtcnt.ToString();
                                                                if (dicrowspanpass.ContainsKey(data.Rows.Count - 3))
                                                                {
                                                                    dicrowspanpass.ContainsKey(data.Rows.Count - 3);
                                                                    dicrowspanpass.Add(data.Rows.Count - 3, rowspancnt);
                                                                }
                                                                else
                                                                {
                                                                    dicrowspanpass.Add(data.Rows.Count - 3, rowspancnt);
                                                                }

                                                            }
                                                            if (ds1.Tables[0].Rows.Count > 0)
                                                            {
                                                                colcunt1++;
                                                                drow = data.NewRow();
                                                                drow["SNo"] = "Batch :" + strbatch + "- Branch :" + strbranchname + "- Semester :" + ddlsemester.SelectedValue.ToString() + "- Section :" + strsec + ""; ;
                                                                data.Rows.Add(drow);

                                                                diccolspan.Add(data.Rows.Count, Convert.ToString(data.Columns.Count));
                                                                string codeandsec = syllcode + '-' + strsection1;

                                                                if (dicsyssecno.ContainsKey(colcunt1))
                                                                {
                                                                    dicsyssecno.Remove(colcunt1);
                                                                    dicsyssecno.Add(colcunt1, codeandsec);
                                                                }
                                                                else
                                                                {

                                                                    dicsyssecno.Add(colcunt1, codeandsec);
                                                                }
                                                                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = syllcode;
                                                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = strsection1;
                                                            }

                                                            ds1.Dispose();
                                                            ds1.Reset();
                                                            //Modified By srinath 12/1/13
                                                            //Modified By srinath 02/04/2014
                                                            //string strsql1 = "select distinct subjectchooser.subject_no,subject_name,subject_code,s.staff_code,staff_name from subject,subjectchooser,registration,sub_sem,exam_type e ,staff_selector s,staffmaster st where e.subject_no=subject.subject_no and  sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no   and registration.degree_code=" + strbranch + " " + strsection + " and registration.batch_year= " + strbatch + "  and subject.syll_code =" + syllcode + " and s.staff_code=st.staff_code and subject.subject_no = s.subject_no and registration.batch_year=" + strbatch + " " + strsection + " " + strstaffcode + " order by subjectchooser.subject_no,subject_name,subject_code,s.staff_code,staff_name";
                                                            string strsql1 = "select distinct subjectchooser.subject_no,subject_name,subject_code,s.staff_code,staff_name from subject,subjectchooser,registration,sub_sem,exam_type e ,staff_selector s,staffmaster st where e.subject_no=subject.subject_no and  sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no   and registration.degree_code=" + strbranch + " " + strsection + " and registration.batch_year= " + strbatch + "  and subject.syll_code =" + syllcode + " and s.staff_code=st.staff_code and subject.subject_no = s.subject_no and registration.batch_year=" + strbatch + " " + strsection + " " + strstaffcode + " " + staffsections + " order by subjectchooser.subject_no,subject_name,subject_code,s.staff_code,staff_name";
                                                            ds1 = d2.select_method(strsql1, hat, "Text");

                                                            if (ds1.Tables[0].Rows.Count > 0)
                                                            {
                                                                for (int subject = 0; subject < ds1.Tables[0].Rows.Count; subject++)
                                                                {
                                                                    subjcnt = 2;
                                                                    subjectcount = 2;
                                                                    string subjrctno = "";
                                                                    if ((temp.ToString() != "") && (temp.ToString() == ds1.Tables[0].Rows[subject]["subject_no"].ToString()))
                                                                    {
                                                                        //Modified by Srinath 2/04/2014===========STart
                                                                        if (data.Rows.Count > 0)
                                                                        {
                                                                            staff_name = data.Rows[data.Rows.Count - 1][2].ToString();

                                                                            if (Convert.ToString(ds1.Tables[0].Rows[subject]["staff_name"]) != "")
                                                                            {
                                                                                staff_name = staff_name + "," + Convert.ToString(ds1.Tables[0].Rows[subject]["staff_name"]);
                                                                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Text = staff_name.ToString();
                                                                                data.Rows[data.Rows.Count - 1][2] = staff_name.ToString();

                                                                            }
                                                                        }
                                                                        //===============En
                                                                    }

                                                                    else
                                                                    {
                                                                        drow = data.NewRow();
                                                                        subjcnt++;

                                                                        slnocnt++;
                                                                        colcount = 3;


                                                                        temp = ds1.Tables[0].Rows[subject]["subject_no"].ToString();
                                                                        drow["SNo"] = slnocnt.ToString();
                                                                        drow["Subject - Subject Code"] = ds1.Tables[0].Rows[subject]["subject_name"].ToString() + " - " + ds1.Tables[0].Rows[subject]["subject_code"].ToString();
                                                                        drow["Name of the Staff"] = ds1.Tables[0].Rows[subject]["staff_name"].ToString();
                                                                        data.Rows.Add(drow);

                                                                        subjrctno = ds1.Tables[0].Rows[subject]["subject_no"].ToString();



                                                                        if (chklstcriteria.Items[10].Selected == true)
                                                                        {
                                                                            string getstdcount = d2.GetFunction("select COUNT(distinct Registration.roll_no) from Registration,subjectchooser s where registration.Roll_No=s.roll_no and registration.degree_code=" + strbranch + " " + strsection + " and registration.batch_year= " + strbatch + " and subject_no='" + ds1.Tables[0].Rows[subject]["subject_no"].ToString() + "' and cc=0 and delflag=0 and exam_flag <> 'DEBAR'");
                                                                            data.Rows[data.Rows.Count - 1][data.Columns.Count - 1] = getstdcount;

                                                                        }

                                                                        staff_name = "";

                                                                        strsubno = Convert.ToString(subjrctno);
                                                                        strcriteriano1 = strcriteriano;
                                                                        string sysandsce = dicsyssecno[subjcnt - 3];
                                                                        string[] split = sysandsce.Split('-');
                                                                        strsyllcode = Convert.ToString(split[0]);

                                                                        str_section = Convert.ToString(split[1]);
                                                                        if (strcriteriano1 != "")
                                                                        {

                                                                            string strexmcode = "select distinct exam_code,staff_code,c.criteria_no,e.min_mark from subject s,criteriaforinternal c,exam_type e where c.criteria_no=e.criteria_no and s.syll_code=c.syll_code and s.subject_no=e.subject_no and c.criteria_no in (" + strcriteriano1 + ") and s.subject_no='" + strsubno + "' and s.syll_code=" + strsyllcode + " " + str_section + " order by c.criteria_no";
                                                                            ds2.Dispose();
                                                                            ds2.Reset();
                                                                            ds2 = d2.select_method(strexmcode, hat, "Text");

                                                                            for (int colcnt = 3; colcnt < coltestcnt; colcnt++)
                                                                            {


                                                                                if (dicriteriano.ContainsKey(colcnt))
                                                                                    strcritno = dicriteriano[colcnt];

                                                                                if (dicmaxmark.ContainsKey(subjcnt - 2))
                                                                                {
                                                                                    string mark = dicmaxmark[subjcnt - 2];
                                                                                    minmark = Convert.ToInt32(mark);
                                                                                }
                                                                                string sections = dicsecno[subjcnt - 3];

                                                                                string[] spilt1 = sections.Split('-');
                                                                                str_section1 = Convert.ToString(spilt1[0]);
                                                                                str_section2 = Convert.ToString(spilt1[1]);
                                                                                strcriteriacnt = dicriteriano1[subjcnt];

                                                                                if (ds2.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    if (strsubno != "" || strcritno.Trim() != "" || strsyllcode != "")
                                                                                    {
                                                                                        strecode = "";
                                                                                        for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                                                                                        {
                                                                                            if (strcritno == Convert.ToString(ds2.Tables[0].Rows[i]["criteria_no"])) //&& strsubno == Convert.ToString(ds2.Tables[0].Rows[i]["subject_no"]))
                                                                                            {
                                                                                                strecode = ds2.Tables[0].Rows[i]["exam_code"].ToString();
                                                                                                break;
                                                                                            }
                                                                                        }

                                                                                        ds3.Dispose();
                                                                                        ds3.Reset();
                                                                                        if (strecode != "")
                                                                                        {
                                                                                            ds3 = d2.SubjectOverAllCount(strecode, minmark);

                                                                                            if (ds3 != null)
                                                                                            {
                                                                                                if (ds3.Tables[3].Rows.Count > 0)
                                                                                                {
                                                                                                    totapp = Convert.ToInt32(ds3.Tables[3].Rows[0]["No.of Appeared"]);
                                                                                                }

                                                                                                if (chklstcriteria.Items[0].Selected == true)
                                                                                                {
                                                                                                    if (ds3.Tables[0].Rows.Count > 0)
                                                                                                    {
                                                                                                        passcnt = Convert.ToDouble(ds3.Tables[0].Rows[0]["Pass_Count"]);
                                                                                                        data.Rows[data.Rows.Count - 1][colcount] = passcnt;

                                                                                                        colcount++;
                                                                                                    }
                                                                                                }

                                                                                                if (chklstcriteria.Items[1].Selected == true)
                                                                                                {
                                                                                                    if (ds3.Tables[0].Rows.Count > 0)
                                                                                                    {
                                                                                                        passcnt = Convert.ToDouble(ds3.Tables[0].Rows[0]["Pass_Count"]);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        passcnt = 0;
                                                                                                    }
                                                                                                    passpercen = Convert.ToDouble((passcnt / totapp) * 100);
                                                                                                    passpercen_round = Math.Round(passpercen, 2);
                                                                                                    data.Rows[data.Rows.Count - 1][colcount] = Convert.ToString(passpercen_round);

                                                                                                    colcount++;
                                                                                                }

                                                                                                if (chklstcriteria.Items[2].Selected == true)
                                                                                                {
                                                                                                    if (ds3.Tables[1].Rows.Count > 0)
                                                                                                    {
                                                                                                        failcnt = Convert.ToDouble(ds3.Tables[1].Rows[0]["Fail_Count_Without_AB"]);
                                                                                                        data.Rows[data.Rows.Count - 1][colcount] = Convert.ToString(failcnt);


                                                                                                        colcount++;
                                                                                                    }
                                                                                                }

                                                                                                if (chklstcriteria.Items[3].Selected == true)
                                                                                                {
                                                                                                    if (ds3.Tables[1].Rows.Count > 0)
                                                                                                    {
                                                                                                        failcnt = Convert.ToDouble(ds3.Tables[1].Rows[0]["Fail_Count_Without_AB"]);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        failcnt = 0;
                                                                                                    }
                                                                                                    failpercen = Convert.ToDouble((failcnt / totapp) * 100);
                                                                                                    failpercen_round = Math.Round(failpercen, 2);
                                                                                                    data.Rows[data.Rows.Count - 1][colcount] = Convert.ToString(failpercen_round);


                                                                                                    colcount++;
                                                                                                }
                                                                                                if (chklstcriteria.Items[4].Selected == true)
                                                                                                {
                                                                                                    if (ds3.Tables[2].Rows.Count > 0)
                                                                                                    {
                                                                                                        absentcnt = Convert.ToDouble(ds3.Tables[2].Rows[0]["Absent"]);
                                                                                                        data.Rows[data.Rows.Count - 1][colcount] = Convert.ToString(absentcnt);

                                                                                                        colcount++;
                                                                                                    }
                                                                                                }
                                                                                                if (chklstcriteria.Items[5].Selected == true)
                                                                                                {
                                                                                                    if (ds3.Tables[2].Rows.Count > 0)
                                                                                                    {
                                                                                                        absentcnt = Convert.ToDouble(ds3.Tables[2].Rows[0]["Absent"]);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        absentcnt = 0;
                                                                                                    }
                                                                                                    totapp1 = totapp + absentcnt;
                                                                                                    absentpercen = Convert.ToDouble((absentcnt / totapp1) * 100);
                                                                                                    absentpercen_round = Math.Round(absentpercen, 2);
                                                                                                    data.Rows[data.Rows.Count - 1][colcount] = Convert.ToString(absentpercen_round);

                                                                                                    colcount++;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }

                                                                            }
                                                                        }//Modified By Srianth 2/4/2014
                                                                        if (getcoulmn <= 1)
                                                                        {
                                                                            eopcount = colcount;
                                                                        }
                                                                        else
                                                                        {
                                                                            eopcount = getcoulmn;//Modified By Srianth 2/4/2014
                                                                        }
                                                                        subnocnt++;
                                                                        if (strsubno != "" && strcriteriacnt != "")
                                                                        {
                                                                            ds2.Dispose();
                                                                            ds2.Reset();
                                                                            string strfail = "select result.roll_no,count(marks_obtained) as Marks from result, registration,exam_type where  registration.roll_no = result.roll_no " + str_section1 + " and exam_type.exam_code=result.exam_code  and exam_type.subject_no = " + strsubno + "and result.marks_obtained is not null And result.marks_obtained < exam_type.min_mark " + str_section2 + " And result.marks_obtained >=0 and registration.delflag=0 and cc=0 and exam_flag<>'DEBAR' group by result.roll_no having count(marks_obtained)>=" + strcriteriacnt + "";
                                                                            ds2 = d2.select_method(strfail, hat, "Text");
                                                                            if (ds2.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                failure = ds2.Tables[0].Rows.Count;
                                                                            }
                                                                            else
                                                                            {
                                                                                failure = 0;
                                                                            }
                                                                        }

                                                                        if (strsubno != "")
                                                                        {
                                                                            ds2.Dispose();
                                                                            ds2.Reset();
                                                                            string strpass = "select r.roll_no,stud_name from registration r,subjectchooser sc where r.roll_no=sc.roll_no and batch_year= " + strbatch + " and degree_code=" + strbranch + " and semester=" + strsem + " " + str_section + " and subject_no =" + strsubno + "";
                                                                            ds2 = d2.select_method(strpass, hat, "Text");
                                                                            if (ds2.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                passpercount = ds2.Tables[0].Rows.Count;
                                                                                expnopass = passpercount - failure;
                                                                            }
                                                                        }

                                                                        if (chklstcriteria.Items[6].Selected == true)
                                                                        {
                                                                            data.Rows[data.Rows.Count - 1][eopcount] = Convert.ToString(expnopass);

                                                                            eopcount++;
                                                                        }

                                                                        if (chklstcriteria.Items[7].Selected == true)
                                                                        {
                                                                            exppasspercen = (expnopass / passpercount) * 100;
                                                                            exppasspercen_round = Math.Round(exppasspercen, 2);
                                                                            data.Rows[data.Rows.Count - 1][eopcount] = Convert.ToString(exppasspercen_round);

                                                                            eopcount++;
                                                                        }

                                                                        if (chklstcriteria.Items[8].Selected == true)
                                                                        {
                                                                            data.Rows[data.Rows.Count - 1][eopcount] = Convert.ToString(failure);

                                                                            eopcount++;
                                                                        }

                                                                        if (chklstcriteria.Items[9].Selected == true)
                                                                        {
                                                                            expfailpercen = (failure / passpercount) * 100;
                                                                            expfailpercen_round = Math.Round(expfailpercen, 2);
                                                                            data.Rows[data.Rows.Count - 1][eopcount] = Convert.ToString(expfailpercen_round);

                                                                            eopcount++;
                                                                        }
                                                                    }

                                                                    eopcount = 0;
                                                                    btnxl.Visible = true;
                                                                    lblrptname.Visible = true;
                                                                    txtexcelname.Visible = true;
                                                                    btnPrintMaster.Visible = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                // FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount - 5;
                                                            }
                                                            subcnt = subnocnt;
                                                        }
                                                    }
                                                    if (ck == 0)
                                                    {
                                                        //FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount - 4;
                                                    }
                                                }
                                            }
                                        }
                                    l1:
                                        {

                                        }
                                    }
                                    sectioncnt++;

                                }
                                int d = Convert.ToInt32(data.Columns.Count);
                                if (data.Columns.Count > 0 && data.Rows.Count > 3)
                                {
                                    drow = data.NewRow();
                                    drow["SNo"] = "Class Advisor";

                                    data.Rows.Add(drow);
                                    data.Rows[data.Rows.Count - 1][2] = "HOD";
                                    int colkcheck = data.Columns.Count - 5;
                                    if (colkcheck <= 2)
                                    {
                                        colkcheck = 3;
                                    }

                                    data.Rows[data.Rows.Count - 1][3] = "Principal";

                                    Showgrid.DataSource = data;
                                    Showgrid.DataBind();
                                    Showgrid.Visible = true;
                                    btnPrint.Visible = true;

                                    foreach (KeyValuePair<int, string> dr in diccolspan)
                                    {
                                        int g = dr.Key;
                                        Showgrid.Rows[g - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                        Showgrid.Rows[g - 1].Cells[0].Font.Bold = true;
                                        Showgrid.Rows[g - 1].Cells[0].ColumnSpan = d;
                                        for (int a = 1; a < d; a++)
                                            Showgrid.Rows[g - 1].Cells[a].Visible = false;

                                    }

                                    foreach (KeyValuePair<int, string> dr in dicrowspanpass)
                                    {
                                        int rowstno = dr.Key;
                                        string colspn = dr.Value;
                                        string[] split = colspn.Split('-');


                                        Showgrid.Rows[rowstno - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                        Showgrid.Rows[rowstno - 1].Cells[0].Font.Bold = true;
                                        Showgrid.Rows[rowstno - 1].Cells[0].ColumnSpan = d;
                                        for (int a = 1; a < d; a++)
                                            Showgrid.Rows[rowstno - 1].Cells[a].Visible = false;

                                        Showgrid.Rows[rowstno].Cells[3].HorizontalAlign = HorizontalAlign.Center;
                                        Showgrid.Rows[rowstno].Cells[3].Font.Bold = true;
                                        Showgrid.Rows[rowstno].Cells[3].ColumnSpan = Convert.ToInt32(split[0]);
                                        for (int a = 4; a < Convert.ToInt32(split[0]) + 3; a++)
                                            Showgrid.Rows[rowstno].Cells[a].Visible = false;




                                        int span = rowstno + 3;

                                        Showgrid.Rows[rowstno].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                        Showgrid.Rows[rowstno].Cells[0].Font.Bold = true;
                                        Showgrid.Rows[rowstno].Cells[0].RowSpan = 3;
                                        Showgrid.Rows[rowstno].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                        Showgrid.Rows[rowstno].Cells[1].Font.Bold = true;
                                        Showgrid.Rows[rowstno].Cells[1].RowSpan = 3;
                                        Showgrid.Rows[rowstno].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                                        Showgrid.Rows[rowstno].Cells[2].Font.Bold = true;
                                        Showgrid.Rows[rowstno].Cells[2].RowSpan = 3;
                                        for (int b = Convert.ToInt32(split[1]); b < Convert.ToInt32(split[2]) + Convert.ToInt32(split[1]); b++)
                                        {
                                            Showgrid.Rows[rowstno].Cells[b].HorizontalAlign = HorizontalAlign.Center;
                                            Showgrid.Rows[rowstno].Cells[b].Font.Bold = true;
                                            Showgrid.Rows[rowstno].Cells[b].RowSpan = 3;

                                        }
                                        for (int a = rowstno + 1; a < span; a++)
                                        {
                                            Showgrid.Rows[a].Cells[0].Visible = false;
                                            Showgrid.Rows[a].Cells[1].Visible = false;
                                            Showgrid.Rows[a].Cells[2].Visible = false;
                                            for (int b = Convert.ToInt32(split[1]); b < Convert.ToInt32(split[2]) + Convert.ToInt32(split[1]); b++)
                                            {
                                                Showgrid.Rows[a].Cells[b].Visible = false;
                                            }
                                        }

                                    }


                                    foreach (KeyValuePair<string, string> dr in dicrowcolcrit)
                                    {
                                        string colspn = dr.Value;
                                        string[] split = colspn.Split('-');
                                        int rowstno = Convert.ToInt32(split[0]);
                                        Showgrid.Rows[rowstno].Cells[Convert.ToInt32(split[1])].HorizontalAlign = HorizontalAlign.Center;
                                        Showgrid.Rows[rowstno].Cells[Convert.ToInt32(split[1])].Font.Bold = true;
                                        Showgrid.Rows[rowstno].Cells[Convert.ToInt32(split[1])].ColumnSpan = Convert.ToInt32(split[2]);
                                        //Showgrid.Rows[rowstno].Cells[Convert.ToInt32(split[0])].BackColor = Color.LightBlue;
                                        //Showgrid.Rows[rowstno].Cells[Convert.ToInt32(split[0])].BorderColor = Color.Black;
                                        for (int a = Convert.ToInt32(split[1]) + 1; a < Convert.ToInt32(split[2]) + Convert.ToInt32(split[1]); a++)
                                            Showgrid.Rows[rowstno].Cells[a].Visible = false;
                                    }

                                    for (int i = 0; i < data.Columns.Count; i++)
                                    {
                                        if (i != 1 && i != 2)
                                        {
                                            for (int j = 1; j < Showgrid.Rows.Count; j++)
                                            {
                                                if (!dicrowspanpass.ContainsKey(j) && !dicrowspanpass.ContainsKey(j - 1))
                                                    Showgrid.Rows[j].Cells[i].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                    }
                                    int row = data.Rows.Count;
                                    int col = data.Columns.Count - 3;
                                    Showgrid.Rows[row - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                    Showgrid.Rows[row - 1].Cells[0].Font.Bold = true;
                                    Showgrid.Rows[row - 1].Cells[0].ColumnSpan = 2;
                                    Showgrid.Rows[row - 1].Cells[1].Visible = false;

                                    Showgrid.Rows[row - 1].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                                    Showgrid.Rows[row - 1].Cells[2].Font.Bold = true;
                                    Showgrid.Rows[row - 1].Cells[2].ColumnSpan = 1;


                                    Showgrid.Rows[row - 1].Cells[3].HorizontalAlign = HorizontalAlign.Center;
                                    Showgrid.Rows[row - 1].Cells[3].Font.Bold = true;
                                    Showgrid.Rows[row - 1].Cells[3].ColumnSpan = col;

                                    for (int a = 4; a < data.Columns.Count; a++)
                                        Showgrid.Rows[row - 1].Cells[a].Visible = false;


                                    int rowcnt1 = Showgrid.Rows.Count - 3;
                                    //Rowspan
                                    for (int rowIndex = Showgrid.Rows.Count - rowcnt1 - 1; rowIndex >= 0; rowIndex--)
                                    {
                                        GridViewRow row1 = Showgrid.Rows[rowIndex];
                                        GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                                        Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                        Showgrid.Rows[rowIndex].Font.Bold = true;
                                        Showgrid.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;

                                        for (int i = 0; i < row1.Cells.Count; i++)
                                        {
                                            if (row1.Cells[i].Text == previousRow.Cells[i].Text)
                                            {
                                                row1.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                                       previousRow.Cells[i].RowSpan + 1;
                                                previousRow.Cells[i].Visible = false;
                                            }

                                        }


                                    }

                                    //ColumnSpan
                                    for (int rowIndex = Showgrid.Rows.Count - rowcnt1 - 1; rowIndex >= 0; rowIndex--)
                                    {
                                        for (int cell = Showgrid.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                                        {
                                            TableCell colum = Showgrid.Rows[rowIndex].Cells[cell];
                                            TableCell previouscol = Showgrid.Rows[rowIndex].Cells[cell - 1];
                                            if (colum.Text == previouscol.Text)
                                            {
                                                if (previouscol.ColumnSpan == 0)
                                                {
                                                    if (colum.ColumnSpan == 0)
                                                    {
                                                        previouscol.ColumnSpan += 2;

                                                    }
                                                    else
                                                    {
                                                        previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                                    }
                                                    colum.Visible = false;

                                                }
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    Showgrid.Visible = false;
                                    btnPrint.Visible = false;
                                    errmsg.Text = "No Record Found";
                                    errmsg.Visible = true;
                                    btnxl.Visible = false;
                                    lblrptname.Visible = false;
                                    txtexcelname.Visible = false;
                                    btnPrintMaster.Visible = false;
                                }
                            }
                        }
                    }
                }

            }//Added By srinath 12/1/13
            else
            {
                Showgrid.Visible = false;
                btnPrint.Visible = false;
                btnPrintMaster.Visible = false;
                errmsg.Text = "Please Select Criteria";
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                errmsg.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            //errmsg.Text = ex.ToString();
            //errmsg.Visible = true;
        }
    }

    private static void AddTableColumn(DataTable resultsTable, StringBuilder ColumnName)
    {
        try
        {
            DataColumn tableCol = new DataColumn(ColumnName.ToString());
            resultsTable.Columns.Add(tableCol);
        }
        catch (System.Data.DuplicateNameException)
        {
            ColumnName.Append(" ");
            AddTableColumn(resultsTable, ColumnName);
        }
    }

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;

            }
        }
        catch
        {


        }

    }

    //------Method for the Excel Coversion -----

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            //    string appPath = HttpContext.Current.Server.MapPath("~");
            //    string print = "";
            //    if (appPath != "")
            //    {
            //        int i = 1;
            //        appPath = appPath.Replace("\\", "/");
            //    e:
            //        try
            //        {
            //            print = "Cam_performance" + i;
            //            //FpSpread1.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly);
            //            //Aruna on 26feb2013============================
            //            string szPath = appPath + "/Report/";
            //            string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

            //            FpSpread1.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
            //            Response.Clear();
            //            Response.ClearHeaders();
            //            Response.ClearContent();
            //            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
            //            Response.ContentType = "application/vnd.ms-excel";
            //            Response.Flush();
            //            Response.WriteFile(szPath + szFile);
            //            //=============================================


            //        }
            //        catch
            //        {
            //            i++;
            //            goto e;

            //        }
            //    }
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                d2.printexcelreportgrid(Showgrid, reportname);
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter Report Name";
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnPrintMaster_Click(object sender, EventArgs e)
    {
        //  gobutton();
      
        string clmnheadrname = "";
        Session["page_redirect_value"] = "Cam_Performance_Report.aspx";
        Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "Cam_Performance_Report.aspx" + ":" + "Cam Performance Report");

    }

    public void printhead()
    {
        hat.Clear();
        hat.Add("college_code", Session["InternalCollegeCode"].ToString());
        hat.Add("form_name", "Cam_Performance_Report.aspx");
        DataSet dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {

        }
    }

    public void printheadnew()
    {
        string addressvalue = "";
        if (Session["InternalCollegeCode"].ToString() != null && Session["InternalCollegeCode"].ToString() != "")
        {

        }
    }

    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Subjectwise Performance";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
}