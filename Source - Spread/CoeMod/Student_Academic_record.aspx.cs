using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using BalAccess;


public partial class Student_Academic_record : System.Web.UI.Page
{
    string atten = "";
    string Master = "";
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";

    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string strorder = "";
    string strregorder = "";
    Hashtable hat = new Hashtable();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DAccess2 daccess2 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds_load = new DataSet();
    DataSet dsprint = new DataSet();

    string group_code = "", columnfield = "";
    DAccess2 dacces2 = new DAccess2();
    protected void lb2_Click(object sender, EventArgs e)
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
            ddlBatch.Items.Clear();
            ds_load = daccess2.select_method_wo_parameter("bind_batch", "sp");
            int count = ds_load.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlBatch.DataSource = ds_load;
                ddlBatch.DataTextField = "batch_year";
                ddlBatch.DataValueField = "batch_year";
                ddlBatch.DataBind();
            }
            int count1 = ds_load.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
                ddlBatch.SelectedValue = max_bat.ToString();

            }
        }
        catch
        {
        }
    }
    public void bindbranch()
    {
        try
        {
            ddlBranch.Items.Clear();
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["InternalCollegeCode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddlDegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

            ds_load = daccess2.select_method("bind_branch", hat, "sp");
            int count2 = ds_load.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlBranch.DataSource = ds_load;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
        }
        catch
        {
        }
    }
    public void bindexammonth()
    {
        try
        {
            ddlMonth.Items.Clear();
            ddlYear.Items.Clear();

            string yearquery11 = "select distinct exam_month from exam_details where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and current_semester='" + ddlSemYr.SelectedValue.ToString() + "'";
            DataSet dssem1 = d2.select_method_wo_parameter(yearquery11, "Text");
            if (dssem1.Tables[0].Rows.Count > 0)
            {
                for (int jk1 = 0; jk1 < dssem1.Tables[0].Rows.Count; jk1++)
                {
                    int exammonth = Convert.ToInt16(dssem1.Tables[0].Rows[jk1]["exam_month"].ToString());
                    string monthtext = bindmonthname(exammonth);

                    ddlMonth.Items.Add(new System.Web.UI.WebControls.ListItem(monthtext.ToString(), exammonth.ToString()));
                }
            }
            ddlMonth.Items.Add("Select");
        }
        catch
        {
        }
    }
    public string bindmonthname(int mon)
    {
        int value = mon;
        string textvalue = "";
        switch (value)
        {
            case 1:
                textvalue = "Jan";
                break;

            case 2:
                textvalue = "Feb";
                break;

            case 3:
                textvalue = "Mar";
                break;

            case 4:
                textvalue = "Apr";
                break;

            case 5:
                textvalue = "May";
                break;

            case 6:
                textvalue = "Jun";
                break;

            case 7:
                textvalue = "Jul";
                break;

            case 8:
                textvalue = "Aug";
                break;

            case 9:
                textvalue = "Sep";
                break;

            case 10:
                textvalue = "Oct";
                break;

            case 11:
                textvalue = "Nov";
                break;

            case 12:
                textvalue = "Dec";
                break;
        }
        return textvalue;
    }
    public void bindexamyear()
    {
        try
        {
            ddlYear.Items.Clear();
            ddlYear.Items.Clear();
            string yearquery = "select distinct exam_year from exam_details where batch_year='" + ddlBatch.SelectedValue.ToString() + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and current_semester='" + ddlSemYr.SelectedValue.ToString() + "'";
            DataSet dssem = d2.select_method_wo_parameter(yearquery, "Text");

            if (dssem.Tables[0].Rows.Count > 0)
            {
                for (int jk = 0; jk < dssem.Tables[0].Rows.Count; jk++)
                {
                    ddlYear.Items.Add(dssem.Tables[0].Rows[jk]["exam_year"].ToString());
                }
            }
            ddlYear.Items.Add("Select");
        }
        catch
        {
        }
    }
    public void binddegree()
    {
        try
        {
            ddlDegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["InternalCollegeCode"].ToString();
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
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds_load = daccess2.select_method("bind_degree", hat, "sp");
            int count1 = ds_load.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddlDegree.DataSource = ds_load;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
            }
        }
        catch
        {
        }
    }
    public void bindsec()
    {
        try
        {
            ddlSec.Items.Clear();
            hat.Clear();
            hat.Add("batch_year", ddlBatch.SelectedValue.ToString());
            hat.Add("degree_code", ddlBranch.SelectedValue);
            ds_load = daccess2.select_method("bind_sec", hat, "sp");
            int count5 = ds_load.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = ds_load;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Enabled = true;
            }
            else
            {
                ddlSec.Enabled = false;
            }
        }
        catch
        {
        }
    }
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = FpSpread1.FindControl("Update");
        Control cntCancelBtn = FpSpread1.FindControl("Cancel");
        Control cntCopyBtn = FpSpread1.FindControl("Copy");
        Control cntCutBtn = FpSpread1.FindControl("Clear");
        Control cntPasteBtn = FpSpread1.FindControl("Paste");

        Control cntPagePrintBtn = FpSpread1.FindControl("Print");

        if ((cntUpdateBtn != null))
        {
            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tc = (TableCell)cntPagePrintBtn.Parent;
            tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }


    //public string GetFunction(string sqlQuery)
    //{
    //    string sqlstr;
    //    sqlstr = sqlQuery;
    //    con.Close();
    //    con.Open();
    //    SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
    //    SqlDataReader drnew;
    //    SqlCommand cmd = new SqlCommand(sqlstr);
    //    cmd.Connection = con;
    //    drnew = cmd.ExecuteReader();
    //    drnew.Read();
    //    if (drnew.HasRows == true)
    //    {
    //        return drnew[0].ToString();
    //    }
    //    else
    //    {
    //        return "";
    //    }
    //}
    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblerror.Visible = false;
        lblerr.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        FpEntry.Visible = false;
        FpSpread1.Visible = false;
        Button2.Visible = false;
        FpSpread3.Visible = false;
        btnExcel.Visible = false;
        BtnPrint.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        string course_id = ddlDegree.SelectedValue.ToString();
        string collegecode = Session["InternalCollegeCode"].ToString();
        string usercode = Session["usercode"].ToString();
        ddlBranch.Items.Clear();
        collegecode = ddlcollege.SelectedValue.ToString();
        usercode = Session["UserCode"].ToString();
        DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }
        bindsem();
        bindsec();
        bindexammonth();
        bindexamyear();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblerror.Visible = false;
        lblerr.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        FpEntry.Visible = false;
        FpSpread1.Visible = false;
        Button2.Visible = false;
        FpSpread3.Visible = false;
        btnExcel.Visible = false;
        BtnPrint.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        bindsem();
        bindsec();
        bindexammonth();
        bindexamyear();
        try
        {
            if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
            {
                bindsem();
                bindsec();
                bindexammonth();
                bindexamyear();
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }

    }

    public void BindSectionDetail()
    {
        try
        {
            string branch = ddlBranch.SelectedValue.ToString();
            string batch = ddlBatch.SelectedValue.ToString();
            string getdeteails = "select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";
            DataSet dssem = d2.select_method_wo_parameter(getdeteails, "Text");
            int count5 = dssem.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = dssem;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
            }
            else
            {
                ddlSec.Enabled = false;
            }
        }
        catch
        {
        }
    }
    public void bindsem()
    {
        try
        {
            ddlSemYr.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            string getdeteails = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "";
            DataSet dssem = d2.select_method_wo_parameter(getdeteails, "Text");
            if (dssem.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(dssem.Tables[0].Rows[0]["first_year_nonsemester"].ToString());
                duration = Convert.ToInt16(dssem.Tables[0].Rows[0]["ndurations"].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                ddlSemYr.Items.Clear();
                string getdeteails1 = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "";
                DataSet dssem11 = d2.select_method_wo_parameter(getdeteails1, "Text");
                if (dssem11.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(dssem11.Tables[0].Rows[0]["first_year_nonsemester"].ToString());
                    duration = Convert.ToInt16(dssem11.Tables[0].Rows[0]["duration"].ToString());

                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSemYr.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSemYr.Items.Add(i.ToString());
                        }
                    }
                }

            }
        }

        catch
        {
        }
    }
    public void Get_Semester()
    {
        try
        {
            Boolean first_year;
            first_year = false;
            int duration = 0;
            string batch_calcode_degree;
            //int typeval = 4;

            string batch = ddlBatch.SelectedValue.ToString();
            string collegecode = ddlcollege.SelectedValue.ToString();
            string degree = ddlBranch.SelectedValue.ToString();
            batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
            DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                ddlSemYr.Items.Clear();
                for (int i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                }
            }
        }
        catch
        {
        }
    }


    //public double roundresult(string nstr)
    //{
    //    con.Close();
    //    con.Open();
    //    double roundresult;
    //    if ((nstr) != "")
    //    {

    //        double ag1;
    //        ag1 = Convert.ToDouble(Math.Round(Convert.ToDecimal(nstr), 2));

    //        roundresult = ag1;
    //    }
    //    else
    //    {
    //        roundresult = 0;
    //    }
    //    return roundresult;
    //}
    //    //*****
    //private string Splitter(string p, string p_2)
    //{
    //    throw new NotImplementedException();
    //}
    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        lblerr.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        FpEntry.Visible = false;
        FpSpread1.Visible = false;
        Button2.Visible = false;
        lblnorec.Visible = false;
        FpSpread3.Visible = false;
        btnExcel.Visible = false;
        BtnPrint.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }

        bindsec();
        bindexammonth();
        bindexamyear();
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        lblerr.Visible = false;
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        lblerr.Visible = false;
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        //if (ddlMonth.SelectedItem.Text == "Select" || ddlYear.SelectedItem.Text == "Select")
        //{
        //    lblerror.Visible = true;
        //    lblerror.Text = " No Records Found";
        //    FpSpread1.Visible = false;
        //    lblrptname.Visible = false;
        //    txtexcelname.Visible = false;
        //    btnExcel.Visible = false;
        //    BtnPrint.Visible = false;
        //    return;
        //}
        //else
        {
            buttonG0();
        }
    }
    protected void buttonG0()
    {
        try
        {
            Hashtable hatrollrae = new Hashtable();
            btnExcel.Visible = true;
            BtnPrint.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;

            string collnamenew1 = "";
            string collnamenew12 = "";
            string address1 = "";
            string address2 = "";

            string Phoneno = "";
            string Faxno = "";
            string phnfax = "";

            string district = "";
            string email = "";
            if (Session["InternalCollegeCode"].ToString() != null && Session["InternalCollegeCode"].ToString() != "")
            {
                string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["InternalCollegeCode"] + "";
                DataSet dssem22 = d2.select_method_wo_parameter(college, "Text");
                if (dssem22.Tables[0].Rows.Count > 0)
                {
                    collnamenew1 = dssem22.Tables[0].Rows[0]["collname"].ToString();
                    address1 = dssem22.Tables[0].Rows[0]["address1"].ToString();
                    address2 = dssem22.Tables[0].Rows[0]["address2"].ToString();
                    district = dssem22.Tables[0].Rows[0]["district"].ToString();
                    collnamenew12 = collnamenew1 + "-" + district;
                    Phoneno = dssem22.Tables[0].Rows[0]["phoneno"].ToString();
                    Faxno = dssem22.Tables[0].Rows[0]["faxno"].ToString();
                    phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno + ".";
                    email = "E-Mail:" + dssem22.Tables[0].Rows[0]["email"].ToString() + " " + "Web Site:" + dssem22.Tables[0].Rows[0]["website"].ToString();

                }
            }
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;

            FpSpread1.Sheets[0].ColumnHeader.Visible = true;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 4;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

            if (Session["Rollflag"].ToString() == "1")
            {
                FpSpread1.Sheets[0].Columns[1].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[1].Visible = false;
            }

            if (Session["Regflag"].ToString() == "1")
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
            }
            else
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
            }

            string yrsemm = "1";
            int count6 = 0;
            int K = 0;
            DataSet dsbatch = new DataSet();
            DataSet dsbatch6 = new DataSet();
            string strquery25 = "select distinct semester  from seminfo  where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by semester  ";
            dsbatch.Dispose();
            dsbatch.Reset();
            dsbatch = d2.select_method_wo_parameter(strquery25, "Text");
            string semconts = ddlSemYr.SelectedValue.ToString();
            int semyr = Convert.ToInt16(semconts);
            if (semyr > 0)
            {
                for (int i = 1; i <= semyr; i++)
                {
                    string semester = Convert.ToString(i);
                    yrsemm = yrsemm + "," + semester;
                    Boolean headflag = false;
                    string year = "";
                    string yr = "";
                    string strquery256 = "select s.subject_code,s.subject_name,s.syll_code,sm.degree_code,sm.semester,sm.Batch_Year,sm.syllabus_year from subject s,sub_sem ss,syllabus_master sm where  sm.degree_code=" + ddlBranch.SelectedValue.ToString() + " and sm.Batch_Year=" + ddlBatch.SelectedValue.ToString() + "  and s.syll_code=ss.syll_code and s.subType_no=ss.subType_no and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.promote_count=1 order by sm.semester";
                    dsbatch6.Dispose();
                    dsbatch6.Reset();
                    dsbatch6 = d2.select_method_wo_parameter(strquery256, "Text");
                    if (dsbatch6.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = new DataView();
                        dsbatch6.Tables[0].DefaultView.RowFilter = "Semester=" + semester + "";
                        dv = dsbatch6.Tables[0].DefaultView;
                        if (semester == "1")
                        {
                            year = "I SEM";
                            yr = "1";
                        }
                        if (semester == "2")
                        {
                            year = "II SEM";
                            yr = "2";
                        }
                        if (semester == "3")
                        {
                            year = "III SEM";
                            yr = "3";
                        }
                        if (semester == "4")
                        {
                            year = "IV SEM";
                            yr = "4";
                        }
                        if (semester == "5")
                        {
                            year = "V SEM";
                            yr = "5";
                        }
                        if (semester == "6")
                        {
                            year = "VI SEM";
                            yr = "6";
                        }
                        if (semester == "7")
                        {
                            year = "VII SEM";
                            yr = "7";
                        }
                        if (semester == "8")
                        {
                            year = "VIII SEM";
                            yr = "8";
                        }
                        if (semester == "9")
                        {
                            year = "IX SEM";
                            yr = "9";
                        }
                        if (semester == "10")
                        {
                            year = "X SEM";
                            yr = "10";
                        }
                        count6 = dv.Count;
                        if (count6 > 0)
                        {
                            for (int n = 0; n < count6; n++)
                            {
                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                if (headflag == false)
                                {

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = year;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = yr;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    headflag = true;
                                }
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = dv[n]["subject_code"].ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = dv[n]["subject_code"].ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 10;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;

                            }
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - count6, 1, count6);
                        }
                    }
                }
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 2);
                //MyImg2 mi3 = new MyImg2();
                //mi3.ImageUrl = "Handler/Handler2.ashx?";

                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi3;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorLeft = Color.White;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = Color.White;

                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2, 3, 2);
                //MyImg2 mi4 = new MyImg2();
                //mi4.ImageUrl = "Handler/Handler5.ashx?";
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].CellType = mi4;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Border.BorderColorLeft = Color.White;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Border.BorderColorRight = Color.White;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Bold = true;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Font.Size = FontUnit.Medium;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Border.BorderColor = Color.White;

                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4);
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = collnamenew12;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = Color.White;

                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4);
                //FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Department  of " + ddlBranch.SelectedItem.ToString();
                //FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Font.Bold = true;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Font.Size = FontUnit.Medium;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColor = Color.White;

                ////FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Text = phnfax;
                ////FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Text = email;
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count);
                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, 0].Text = "STUDENT ACADEMIC RECORD";
                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, 0].Font.Bold = true;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, 0].Font.Size = FontUnit.Medium;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColor = Color.White;
                ////FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].Text = "Batch Year:  " + ddlBatch.SelectedValue.ToString() + " " + "Course: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
                ////FpSpread1.Sheets[0].ColumnHeader.Cells[6, 1].Text = "Test: " + test + " " + "Semester: " + ddlSemYr.SelectedValue.ToString() + "Section: " + ddlSec.SelectedValue.ToString();

                //FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = false;
                //FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = false;
                //FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = false;
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, 0, 1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count);
                //FpSpread1.Sheets[0].ColumnHeader.Cells[3, 0].Text = "   CLASS : " + ddlBranch.SelectedItem.ToString() + "-" + ddlSec.SelectedValue.ToString();
                //FpSpread1.Sheets[0].ColumnHeader.Cells[3, 0].Font.Bold = true;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[3, 0].HorizontalAlign = HorizontalAlign.Left;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[3, 0].Font.Size = FontUnit.Medium;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[3, 0].Border.BorderColor = Color.White;
                FpSpread1.Sheets[0].RowCount = 0;


                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "HISTROY OF ARREARS";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;

                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "NO OF STANDING ARREARS";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread1.Sheets[0].AutoPostBack = true;

                DataView dv_data = new DataView();
                DataSet dsover = new DataSet();
                DataSet dsroll = new DataSet();
                DataSet dssem = new DataSet();
                int rowdatacounts = 0;
                string collegecode1 = ddlcollege.SelectedValue.ToString();
                string deptcode1 = ddlBranch.SelectedValue.ToString();
                string batch1 = ddlBatch.SelectedValue.ToString();
                string secs = ddlSec.SelectedValue.ToString();
                string seccsc = "";
                if (secs == "")
                {
                    seccsc = "";
                }
                else
                {
                    seccsc = "and re.Sections='" + secs + "'";
                }
                string exmmonth = ddlMonth.SelectedValue.ToString();
                string exmyer = ddlYear.SelectedValue.ToString();

                if (exmmonth == "Select" || exmyer == "Select")
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select the Correct semester";
                    FpSpread1.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    BtnPrint.Visible = false;
                    return;
                }
                else
                {
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    string Sqlstr1213 = "select distinct result,re.Stud_Name ,me.roll_no,s.subject_code,s.subject_name,s.syll_code,sm.degree_code,sm.semester,sm.Batch_Year,me.passorfail from subject s,sub_sem ss,syllabus_master sm,mark_entry me,Registration re,exam_details examde where  re.Roll_No=me.roll_no and examde.exam_code=me.exam_code and me.subject_no=s.subject_no  and sm.semester in (" + yrsemm + ") and re.college_code=" + collegecode1 + " and re.Batch_Year=" + batch1 + " and re.degree_code=" + deptcode1 + "" + seccsc + " and s.syll_code=ss.syll_code and  me.passorfail=0 and me.subject_no=s.subject_no and s.subType_no=ss.subType_no and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.promote_count=1 order by sm.semester";
                    dsover = d2.select_method_wo_parameter(Sqlstr1213, "Text");

                    string strarrearpass = "select distinct result,re.Stud_Name ,me.roll_no,s.subject_code,s.subject_name,s.syll_code,sm.degree_code,sm.semester,sm.Batch_Year,me.passorfail from subject s,sub_sem ss,syllabus_master sm,mark_entry me,Registration re,exam_details examde where  re.Roll_No=me.roll_no and examde.exam_code=me.exam_code and me.subject_no=s.subject_no  and sm.semester in (" + yrsemm + ") and re.college_code=" + collegecode1 + " and re.Batch_Year=" + batch1 + " and re.degree_code=" + deptcode1 + "" + seccsc + " and s.syll_code=ss.syll_code and  me.passorfail=1 and me.subject_no=s.subject_no and s.subType_no=ss.subType_no and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.promote_count=1 order by sm.semester";
                    DataSet dsarreapass = d2.select_method_wo_parameter(strarrearpass, "Text");

                    if (dsover.Tables[0].Rows.Count > 0)
                    {
                        string Sqlstrroll = "select distinct me.roll_no,re.Stud_Name,re.Reg_No,re.Roll_No from subject s,sub_sem ss,syllabus_master sm,mark_entry me,Registration re,exam_details examde where  re.Roll_No=me.roll_no and examde.exam_code=me.exam_code   and me.subject_no=s.subject_no and re.college_code=" + collegecode1 + " and sm.semester in (" + yrsemm + ")  and re.Batch_Year=" + batch1 + " and re.degree_code=" + deptcode1 + "" + seccsc + " and s.syll_code=ss.syll_code and  me.passorfail=0 and me.subject_no=s.subject_no and s.subType_no=ss.subType_no and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.promote_count=1 order by re.Roll_No";
                        dsroll = d2.select_method_wo_parameter(Sqlstrroll, "Text");

                        string Sqlstrsem = "select distinct sm.semester from subject s,sub_sem ss,syllabus_master sm,mark_entry me,Registration re,exam_details examde where  re.Roll_No=me.roll_no and examde.exam_code=me.exam_code   and me.subject_no=s.subject_no and re.college_code=" + collegecode1 + " and sm.semester in (" + yrsemm + ") and re.Batch_Year=" + batch1 + " and re.degree_code=" + deptcode1 + "" + seccsc + " and s.syll_code=ss.syll_code and  me.passorfail=0 and me.subject_no=s.subject_no and s.subType_no=ss.subType_no and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.promote_count=1 order by sm.semester";
                        dssem = d2.select_method_wo_parameter(Sqlstrsem, "Text");

                        for (int i = 0; i < dsroll.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            rowdatacounts++;
                            if ((rowdatacounts % 2) == 0)
                            {
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.White;
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = rowdatacounts.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;

                            string stdname = dsroll.Tables[0].Rows[i]["Stud_Name"].ToString();
                            string stdregno = dsroll.Tables[0].Rows[i]["Reg_No"].ToString();
                            string stdrollno = dsroll.Tables[0].Rows[i]["Roll_No"].ToString();

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = stdrollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;


                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = stdregno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Columns[2].Width = 100;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = stdname;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;

                            Hashtable hatchecsub = new Hashtable();
                            for (int j = 0; j < dssem.Tables[0].Rows.Count; j++)
                            {
                                for (int co = 4; co < FpSpread1.Sheets[0].ColumnCount; co++)
                                {
                                    string semsub = FpSpread1.Sheets[0].ColumnHeader.Cells[1, co].Note;
                                    string stdroll = dsroll.Tables[0].Rows[i]["roll_no"].ToString();
                                    dsover.Tables[0].DefaultView.RowFilter = "subject_code = '" + semsub + "' and roll_no='" + stdroll + "'";
                                    dv_data = dsover.Tables[0].DefaultView;
                                    if (dv_data.Count > 0)
                                    {
                                        if (dv_data[0]["result"].ToString().Trim().ToLower() == "fail" || dv_data[0]["result"].ToString().Trim() == "")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, co].Text = "*";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, co].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, co].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        if (dv_data[0]["result"].ToString().Trim().ToLower() == "aaa")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, co].Text = "•";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, co].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, co].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        dsarreapass.Tables[0].DefaultView.RowFilter = "subject_code = '" + semsub + "' and roll_no='" + stdroll + "'";
                                        DataView dvarrpass = dsarreapass.Tables[0].DefaultView;
                                        if (!hatchecsub.Contains(stdroll + '-' + semsub))
                                        {
                                            if (stdroll == "14JEAE220")
                                            {
                                            }
                                            if (dvarrpass.Count > 0)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, co].ForeColor = Color.Green;

                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, co].ForeColor = Color.Red;
                                                if (hatrollrae.Contains(stdroll))
                                                {
                                                    int gertat = Convert.ToInt32(hatrollrae[stdroll]);
                                                    gertat++;
                                                    hatrollrae[stdroll] = gertat;
                                                }
                                                else
                                                {
                                                    hatrollrae.Add(stdroll, 1);
                                                }
                                            }
                                            hatchecsub.Add(stdroll + '-' + semsub, 1);
                                        }
                                    }
                                }
                            }
                        }

                        //FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        //lblerror.Visible = false;

                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                        string countper = "";
                        for (int col = 4; col < FpSpread1.Sheets[0].ColumnCount; col++)
                        {
                            int conttol = 0;
                            for (int co = 0; co < FpSpread1.Sheets[0].RowCount - 1; co++)
                            {
                                countper = FpSpread1.Sheets[0].Cells[co, col].Text.ToString();
                                if (countper == "" || countper == "0")
                                {
                                    countper = "NIL";
                                }
                                else if (countper != "")
                                {
                                    conttol++;
                                }

                            }
                            string conttols = Convert.ToString(conttol);
                            if (conttols == "0")
                            {
                                conttols = "NIL";
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(conttols);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Large;
                        }
                        int mainovercount = 0;
                        string countper1 = "";
                        int nofoallstaarre = 0;
                        for (int co1 = 0; co1 < FpSpread1.Sheets[0].RowCount - 1; co1++)
                        {
                            string getroll = FpSpread1.Sheets[0].Cells[co1, 1].Text;
                            int conttol1 = 0;
                            for (int col1 = 4; col1 < FpSpread1.Sheets[0].ColumnCount; col1++)
                            {
                                countper1 = FpSpread1.Sheets[0].Cells[co1, col1].Text.ToString();
                                if (countper1 == "" || countper1 == "0")
                                {
                                    countper1 = "0";
                                }
                                else if (countper1 != "")
                                {
                                    conttol1++;
                                }
                            }
                            FpSpread1.Sheets[0].Cells[co1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(conttol1);
                            FpSpread1.Sheets[0].Cells[co1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[co1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                            int nostatarrear = 0;
                            if (hatrollrae.Contains(getroll))
                            {
                                nostatarrear = Convert.ToInt32(hatrollrae[getroll]);
                                nofoallstaarre = nofoallstaarre + nostatarrear;
                            }
                            FpSpread1.Sheets[0].Cells[co1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(nostatarrear);
                            FpSpread1.Sheets[0].Cells[co1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[co1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            if (nostatarrear == 0)
                            {
                                FpSpread1.Sheets[0].Cells[co1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.Green;
                            }
                        }

                        string countper112 = "";
                        for (int co112 = 4; co112 < FpSpread1.Sheets[0].ColumnCount - 2; co112++)
                        {
                            for (int col112 = FpSpread1.Sheets[0].RowCount - 1; col112 <= FpSpread1.Sheets[0].RowCount - 1; col112++)
                            {
                                countper112 = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, co112].Text.ToString();
                                if (countper112 == "" || countper112 == "NIL")
                                {
                                    countper112 = "0";
                                }
                                else if (countper112 != "")
                                {
                                    mainovercount = mainovercount + Convert.ToInt16(countper112);
                                }
                            }
                        }

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(mainovercount);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(nofoallstaarre);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = " *  -  ATTENDED / FAIL ";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = Color.White;

                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "•-ABSENT";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = Color.White;



                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;

                        int colcomt = FpSpread1.Sheets[0].ColumnCount / 2;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcomt);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "HOD";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = Color.White;

                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, colcomt, 1, FpSpread1.Sheets[0].ColumnCount);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcomt].Text = "PRINCIPAL";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcomt].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcomt].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcomt].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcomt].Font.Size = FontUnit.Large;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcomt].Border.BorderColor = Color.White;

                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        lblerror.Visible = false;
                    }

                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "No Records Found";
                        FpSpread1.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnExcel.Visible = false;
                        BtnPrint.Visible = false;
                        return;
                    }
                    // FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    //lblerror.Visible = false;
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Records Found";
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                BtnPrint.Visible = false;
                return;
            }
            // FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            //lblerror.Visible = false;
        }
        catch
        {
        }
    }
    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblerror.Visible = false;
        lblerr.Visible = false;
        btnExcel.Visible = false;
        BtnPrint.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        lblnorec.Visible = false;
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        lblerr.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        FpEntry.Visible = false;
        FpSpread1.Visible = false;
        Button2.Visible = false;
        FpSpread3.Visible = false;
        btnExcel.Visible = false;
        BtnPrint.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            ddlSemYr.Items.Clear();
            Get_Semester();
        }
        bindexammonth();
        bindexamyear();
        ddlSec.SelectedIndex = -1;
    }
    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        TextBoxother.Text = "";
        if (DropDownListpage.Text == "Others")
        {
            TextBoxother.Visible = true;
            TextBoxother.Focus();
        }
        else
        {
            TextBoxother.Visible = false;
            FpEntry.Visible = true;
            FpEntry.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
        FpEntry.CurrentPage = 0;

        lblerror.Visible = false;
        lblerr.Visible = false;
    }

    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }
    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxpage.Text.Trim() != "")
            {
                if (Convert.ToInt16(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Exceed The Page Limit";
                    FpEntry.Visible = true;
                    TextBoxpage.Text = "";
                }
                else if (Convert.ToInt32(TextBoxpage.Text) == 0)
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Search should be greater than zero";
                    TextBoxpage.Text = "";
                }
                else
                {
                    LabelE.Visible = false;
                    FpEntry.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    FpEntry.Visible = true;
                }
            }
        }
        catch
        {
            TextBoxpage.Text = "";
        }
    }
    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxother.Text != "")
            {
                FpEntry.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                CalculateTotalPages();
            }
        }
        catch
        {
            TextBoxother.Text = "";
        }
    }
    protected void FpEntry_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }
    public string Getdate(string Att_strqueryst)
    {
        SqlConnection mycon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        string sqlstr;
        sqlstr = Att_strqueryst;
        mycon1.Close();
        mycon1.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, mycon1);
        SqlCommand cmd5a = new SqlCommand(sqlstr);
        cmd5a.Connection = mycon1;
        SqlDataReader drnew;
        drnew = cmd5a.ExecuteReader();
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

    public string getattval(int att_leavetype)
    {

        switch (att_leavetype)
        {
            case 1:

                atten = "P";
                break;
            case 2:
                atten = "A";
                break;
            case 3:
                atten = "OD";
                break;
            case 4:
                atten = "ML";
                break;
            case 5:
                atten = "SOD";
                break;
            case 6:
                atten = "NSS";
                break;
            case 7:
                atten = "H";
                break;
            case 8:
                atten = "NJ";
                break;
            case 9:
                atten = "S";
                break;
            case 10:
                atten = "L";
                break;
        }
        return atten;
    }
    protected void RadioButtonList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Sheets[0].RowCount = 0;
        TextBoxother.Visible = false;
        TextBoxother.Text = "";
        TextBoxpage.Text = "";
        FpSpread1.CurrentPage = 0;

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
    }
    protected void LinkButton4_Click(object sender, EventArgs e)
    {
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string reportname = txtexcelname.Text;
        if (reportname.ToString().Trim() != "")
        {
            dacces2.printexcelreport(FpSpread1, reportname.ToString().Trim());
            lblerr.Visible = false;
        }
        else
        {
            lblerr.Text = "Please Enter Your Report Name";
            lblerr.Visible = true;
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string secton = "";
        string classs = "";
        if (ddlSec.Enabled == true)
        {

            if (ddlSec.SelectedItem.Text == "")
            {
                secton = "";
            }
            else
            {
                secton = "-" + ddlSec.SelectedItem.Text.ToString();
            }
        }
        classs = ddlBranch.SelectedItem.Text.ToString() + secton;
        Session["column_header_row_count"] = 3;
        string dcommt = "Student Academic Record Report" + '@' + "Batch :" + ddlBatch.SelectedItem.ToString() + '@' + "Class :" + classs + '@' + "Date :" + DateTime.Now.ToString("dd/MM/yyyy") + "";
        Printcontrol.loadspreaddetails(FpSpread1, "Student_Academic_record.aspx", dcommt);
        Printcontrol.Visible = true;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        lblerr.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        FpEntry.Visible = false;
        FpSpread1.Visible = false;
        Button2.Visible = false;
        FpSpread3.Visible = false;
        btnExcel.Visible = false;
        BtnPrint.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();

        bindbatch();
        binddegree();
        bindbranch();
        bindsem();
        bindsec();
        bindexammonth();
        bindexamyear();
    }
    protected void Page_Load(object sender, EventArgs e)
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
        lblnorec.Visible = false;
        lblerr.Visible = false;
        lblerror.Visible = false;
        try
        {
            lblnorec.Visible = false;
            lblerr.Visible = false;
            lblerror.Visible = false;
            if (!IsPostBack)
            {
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                lblnorec.Visible = false;
                lblerr.Visible = false;
                lblerror.Visible = false;

                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnExcel.Visible = true;


                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Sex"] = "0";
                Session["flag"] = "-1";

                group_code = Session["group_code"].ToString();
                if (group_code.Contains(';'))
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
                dsprint = dacces2.select_method("bind_college", hat, "sp");
                ddlcollege.Items.Clear();
                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.DataSource = dsprint;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    ddlcollege_SelectedIndexChanged(sender, e);
                }
                else
                {
                    lblnorec.Text = "Give college rights to the staff";
                    lblnorec.Visible = true;

                    FpEntry.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    return;
                }


                FpEntry.Sheets[0].SheetName = " ";
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                FpEntry.Visible = false;

                FpEntry.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                FpEntry.Sheets[0].PageSize = 10;

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 12;
                style.Font.Bold = true;
                FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpEntry.Sheets[0].AllowTableCorner = true;
                FarPoint.Web.Spread.SheetView svsort = new FarPoint.Web.Spread.SheetView();
                svsort = FpEntry.ActiveSheetView;
                svsort.AllowSort = true;
                FpEntry.CommandBar.Visible = true;

                FpEntry.Sheets[0].SheetCorner.RowCount = 7;
                FpEntry.Sheets[0].SheetCorner.Cells[6, 0].Text = "S.No";
                FpEntry.Sheets[0].SheetCorner.Cells[6, 0].BackColor = Color.AliceBlue;


                //FpEntry.Sheets[0].Columns[1].Width = 100;
                FpEntry.Sheets[0].SheetCornerSpanModel.Add(0, 0, 5, 1);

                FpEntry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpEntry.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpEntry.Pager.Align = HorizontalAlign.Right;
                FpEntry.Pager.Font.Bold = true;
                FpEntry.Pager.Font.Name = "Book Antiqua";
                FpEntry.Pager.ForeColor = Color.DarkGreen;
                FpEntry.Pager.BackColor = Color.Beige;
                FpEntry.Pager.BackColor = Color.AliceBlue;
                FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpEntry.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpEntry.Sheets[0].DefaultStyle.Font.Bold = false;
                FpEntry.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].FrozenColumnCount = 4;
                FpEntry.Sheets[0].Columns[0].Width = 70;

                FpEntry.Pager.PageCount = 5;
                FpEntry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpEntry.Sheets[0].AutoPostBack = true;


                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    Master = "select * from Master_Settings where group_code=" + Session["group_code"] + "";
                }
                else
                {
                    Master = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                }

                DataSet dsmaseter = dacces2.select_method(Master, hat, "Text");
                string regularflag = "";
                if (dsmaseter.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsmaseter.Tables[0].Rows.Count; i++)
                    {
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Register No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "sex" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Sex"] = "1";
                        }

                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "General" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {

                            Session["flag"] = 0;

                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "As Per Lesson" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {

                            Session["flag"] = 1;

                        }

                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Male" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {

                            genderflag = " and (a.sex='0'";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Female" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (genderflag != "" && genderflag != "\0")
                            {
                                genderflag = genderflag + " or a.sex='1'";
                            }
                            else
                            {
                                genderflag = " and (a.sex='1'";
                            }
                        }

                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Days Scholor" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (strdayflag != null && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or registration.Stud_Type='Day Scholar'";

                            }
                            else
                            {
                                strdayflag = " and (registration.Stud_Type='Day Scholar'";
                            }
                        }

                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Hostel" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (strdayflag != null && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or registration.Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (registration.Stud_Type='Hostler'";
                            }
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Regular")
                        {
                            regularflag = "and ((registration.mode=1)";
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Lateral")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=3)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=3)";
                            }
                        }
                        if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Transfer")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=2)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=2)";
                            }
                        }
                    }
                }

                if (strdayflag != null && strdayflag != "")
                {
                    strdayflag = strdayflag + ")";
                }
                Session["strvar"] = strdayflag;

                if (regularflag != "")
                {
                    regularflag = regularflag + ")";
                }
                if (genderflag != "")
                {
                    genderflag = genderflag + ")";
                }
                Session["strvar"] = Session["strvar"] + regularflag + genderflag;


                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                collegecode = Session["InternalCollegeCode"].ToString();
                usercode = Session["usercode"].ToString();
                bindbatch();
                binddegree();

                if (ddlDegree.Text != "")
                {
                    bindbranch();
                }
                else
                {
                    lblnorec.Text = "Give degree rights to the staff";
                    lblnorec.Visible = true;
                }
                bindsem();
                bindsec();
                bindexammonth();
                bindexamyear();
            }

        }
        catch
        {
        }
    }
}

