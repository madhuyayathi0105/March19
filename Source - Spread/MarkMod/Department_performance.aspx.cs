using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Configuration;


public partial class Department_performance : System.Web.UI.Page
{

    Dictionary<int, string> yeartest = new Dictionary<int, string>();
    Dictionary<int, string> year1 = new Dictionary<int, string>();
    Dictionary<int, string> year2 = new Dictionary<int, string>();
    Dictionary<int, string> sectiontest = new Dictionary<int, string>();
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    //string staff = "";
    //double perofpass = 0;
    //double avg = 0;
    string criteriano = string.Empty;
    //Boolean IsFirstcol = false;
    //Boolean Isfirst = false;
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    //string strorder = "";
    //string strregorder = "";
    //string absentcolumn = "";
    //int headspan = 0;

    int frstrow = 0;
    int scndrow = 0;
    int thrdrow = 0;
    int frthrow = 0;
    int fifthrow = 0;

    int frst = 0;
    int snd = 0;
    int thrd = 0;
    int frth = 0;
    int fifth = 0;

    double frstper = 0;
    double sndper = 0;
    double thrdper = 0;
    double frthper = 0;
    double fifthper = 0;
    System.Text.StringBuilder textpass = new System.Text.StringBuilder();
    int frstperc = 0;
    int sndperc = 0;
    int thrdperc = 0;
    int frthperc = 0;
    int fifthperc = 0;
    DataTable dtdep = new DataTable();
    DataRow drper;
    Hashtable hat = new Hashtable();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds33 = new DataSet();
    DataView dv_count_data = new DataView();
    DataView dv_count_data1 = new DataView();
    DataView dv_count_data2 = new DataView();
    DataView dv_count_data3 = new DataView();
    DAccess2 daccess2 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds_load = new DataSet();
    DataSet dsprint = new DataSet();

    string collnamenew1 = "";
    string address1 = "";
    string address2 = "";
    string address = "";
    string Phoneno = "";
    string Faxno = "";
    string phnfax = "";
    string district = "";
    string email = "";
    string form_heading_name = "";
    string batch_degree_branch = "";

    //int final_print_col_cnt = 0;
    string footer_text = "";
    //int temp_count = 0;
    //int split_col_for_footer = 0;
    //int footer_balanc_col = 0;
    int footer_count = 0;
    string group_code = "", columnfield = "";
    DAccess2 dacces2 = new DAccess2();

    protected void Page_Load(object sender, EventArgs e)
    {
       
        lblerr.Visible = false;
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
            if (!IsPostBack)
            {
                Session["QueryString"] = "";
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
                    lblnorec.Text = "";
                    lblnorec.Visible = false;
                    ddlcollege.DataSource = dsprint;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    ddlcollege_SelectedIndexChanged(sender, e);
                    GetTest();
                }
                else
                {
                    lblnorec.Text = "Set college rights to the staff";
                    lblnorec.Visible = true;
                    lblerror.Visible = false;
                    gridviewdep.Visible = false;
                    btnsection.Visible = false;
                    lblsctnn.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    BtnPrint.Visible = false;
                    return;
                }


                Pageload(sender, e);
            }
        }
        catch
        {
        }
    }

    public void GetTest()
    {
        try
        {
            ddlTest.Items.Clear();
            collegecode = ddlcollege.SelectedValue.ToString();
            string Sqlstr = "select distinct c.criteria from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.current_semester=s.semester and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar'  and r.college_code='" + collegecode + "' order by criteria asc";
            ds2 = d2.select_method_wo_parameter(Sqlstr, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlTest.Items.Clear();
                ddlTest.DataSource = ds2;
                //ddlTest.DataValueField = "Criteria_No";
                ddlTest.DataTextField = "Criteria";
                ddlTest.DataBind();
                ddlTest.Items.Add("--Select--");
                ddlTest.SelectedIndex = ddlTest.Items.Count - 1;
            }

        }
        catch
        {

        }

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

    //public string result(string st)
    //{
    //    con.Close();
    //    con.Open();
    //    string result = "";
    //    SqlDataReader drr;
    //    SqlCommand commmand = new SqlCommand(st, con);
    //    drr = commmand.ExecuteReader();


    //    if (drr.HasRows == true)
    //    {
    //        while (drr.Read())
    //        {
    //            if (drr[0] != null)
    //            {
    //                result = drr[0].ToString();
    //            }
    //            else
    //            {
    //                result = "0";
    //            }
    //        }
    //    }
    //    else if (drr.HasRows == false)
    //    {
    //        result = "";
    //    }

    //    return result;
    //}
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

    protected void btnGo_Click(object sender, EventArgs e)
    {

    }

    protected void BtnPrint_Click(object sender, EventArgs e)
    {

    }

    protected void ddlTest_SelectedIndexChanged1(object sender, EventArgs e)
    {

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

        }

    }


    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        try
        {

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
        }
        catch
        {
            TextBoxother.Text = "";
        }
    }
    protected void FpEntry_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }
    //public string Getdate(string Att_strqueryst)
    //{
    //    string sqlstr;
    //    sqlstr = Att_strqueryst;
    //    mycon1.Close();
    //    mycon1.Open();
    //    SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
    //    SqlCommand cmd5a = new SqlCommand(sqlstr);
    //    cmd5a.Connection = mycon1;
    //    SqlDataReader drnew;
    //    drnew = cmd5a.ExecuteReader();
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

    //public string getattval(int att_leavetype)
    //{

    //    switch (att_leavetype)
    //    {
    //        case 1:

    //            atten = "P";
    //            break;
    //        case 2:
    //            atten = "A";
    //            break;
    //        case 3:
    //            atten = "OD";
    //            break;
    //        case 4:
    //            atten = "ML";
    //            break;
    //        case 5:
    //            atten = "SOD";
    //            break;
    //        case 6:
    //            atten = "NSS";
    //            break;
    //        case 7:
    //            atten = "H";
    //            break;
    //        case 8:
    //            atten = "NJ";
    //            break;
    //        case 9:
    //            atten = "S";
    //            break;
    //        case 10:
    //            atten = "L";
    //            break;
    //    }
    //    return atten;


    //}
    protected void RadioButtonList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FpSpread1.Sheets[0].RowCount = 0;

        TextBoxother.Visible = false;
        TextBoxother.Text = "";
        TextBoxpage.Text = "";
        //FpSpread1.CurrentPage = 0;

        if ((ddlTest.SelectedIndex != 0) && (ddlTest.Text != ""))
        {

        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
    }
    protected void LinkButton4_Click(object sender, EventArgs e)
    {
    }

    //int subjectcount = 0;
    //int topstud = 3;
    //string subno = "";
    //string subject_code = "";
    //string resminmrk = "";
    //string exam_code = "";
    //string examdate;
    //string subname = "";
    //int substcount;
    //int totalstcount;
    //int resultstcount;
    //int photostcount;
    ////subj_bind
    //string subj_code = "";
    //int sno = 0;
    //string srno = "";
    //int sno2 = 0;
    //string srno2 = "";
    //int snotb3 = 0;
    //string srnotb3 = "";
    //string test = "";
    //string Rank = "";
    //string stude_RollNumber = "";
    //string Pertc = "";
    //string Total_Mark = "";
    //string sub_code = "";
    //string stud_Nameof = "";
    //string mark_obt = "";
    //string table2_Roll_No = "";
    //string table2_Stud_Name = "";
    //string table2_Subj_code = "";
    //string table2_Mark = "";
    //int total_pass_fail = 0;
    //string table3_subj_code = "";
    //string table3_subj_name = "";
    //string table3_staff_inc = "";
    //string table3_Pass = "";
    //string table4_Avg = "";
    //private int columnCount;
    //private string count;



    public void filteration()
    {

        string orderby_Setting = dacces2.GetFunction("select value from master_Settings where settings='order_by'");

        if (orderby_Setting == "")
        {
            strorder = "ORDER BY r.Roll_No";
            strregorder = "ORDER BY registration.Roll_No";
        }
        else
        {
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY r.Roll_No";
                strregorder = "ORDER BY registration.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY r.Reg_No";
                strregorder = "ORDER BY registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strregorder = "ORDER BY registration.Stud_Name";
                strorder = "ORDER BY r.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Roll_No,registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No";
                strregorder = "ORDER BY registration.Roll_No,registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Roll_No,registration.Stud_Name";
            }
        }

    }




    protected void Button2_Click(object sender, EventArgs e)
    {

    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["QueryString"]) != "")
        {

            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove(Convert.ToString(Session["QueryString"]));
            Request.QueryString.Clear();

        }

        Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
        Pageload(sender, e);
        GetTest();
    }
    public void Pageload(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        gridviewdep.Visible = false;
        btnsection.Visible = false;
        lblsctnn.Visible = false;
        btnExcel.Visible = false;
        BtnPrint.Visible = false;
        //Added By Srinath 28/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;

        if (strdayflag != "")
        {
            strdayflag = strdayflag + ")";
        }
        Session["strvar"] = strdayflag;
        if (regularflag != "")
        {
            regularflag = regularflag + ")";
        }
        Session["strvar"] = Session["strvar"] + regularflag;
        if (genderflag != "")
        {
            genderflag = genderflag + ")";
        }
        Session["strvar"] = Session["strvar"] + regularflag + genderflag;
        collegecode = Session["InternalCollegeCode"].ToString();
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        if (Request.QueryString["val"] != null)
        {

            Session["QueryString"] = Request.QueryString["val"].ToString();
            string get_pageload_value = Request.QueryString["val"];
            if (get_pageload_value.ToString() != null)
            {
                string[] spl_pageload_val = get_pageload_value.Split(',');

                ddlcollege.SelectedIndex = Convert.ToInt32(spl_pageload_val[4].ToString());
                Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();

                GetTest();
                ddlTest.SelectedIndex = Convert.ToInt32(spl_pageload_val[5].ToString());
                lblnorec.Visible = false;
                btnGo_Click(sender, e);
            }
        }
        else
        {



        }
    }

    //----------------start----------- //Added By thirumalai 29/9/2014
    public void loadheader()
    {
        try
        {
            //FpSpread1.Sheets[0].SheetName = " ";


            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = Color.Black;

            Boolean headflag1 = false;
            Boolean headflag2 = false;
            Boolean headflag3 = false;
            Boolean headflag4 = false;
            Boolean headflag5 = false;

            int count6 = 0;
            int K = 0;
            DataSet dsbatch = new DataSet();
            string collegecde = ddlcollege.SelectedValue.ToString();
            string strquery25 = "select  Current_Semester from Registration where CC=0 and DelFlag=0 and Exam_Flag<>'debar' and college_code=" + collegecde + " group by Current_Semester order by Current_Semester ; select distinct isnull(sections,'')as sections ,current_semester from  Registration where college_code=" + collegecde + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar'  order by sections";
            dsbatch.Dispose();
            dsbatch.Reset();
            dsbatch = d2.select_method_wo_parameter(strquery25, "Text");

            dtdep.Columns.Add("SNo");
            dtdep.Columns.Add("Course");
            //dtdep.Columns.Add("degree_code");
            //dtdep.Columns.Add("year");
            int col = 0;
            int columncnt = 2;

            if (dsbatch.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsbatch.Tables[0].Rows.Count; i++)
                {
                    string semester = dsbatch.Tables[0].Rows[i]["Current_Semester"].ToString();
                    Boolean headflag = false;

                    string year = "";
                    string yr = "";
                    if (dsbatch.Tables[1].Rows.Count > 0)
                    {
                        DataView dv = new DataView();
                        dsbatch.Tables[1].DefaultView.RowFilter = "Current_Semester=" + semester + "";
                        dv = dsbatch.Tables[1].DefaultView;
                        if (headflag1 == false)
                        {
                            if (semester == "1" || semester == "2")
                            {
                                year = "1st Year";
                                yr = "1";
                                headflag1 = true;
                            }

                        }
                        if (headflag2 == false)
                        {
                            if (semester == "3" || semester == "4")
                            {
                                year = "2nd Year";
                                yr = "2";
                                headflag2 = true;
                            }
                        }
                        if (headflag3 == false)
                        {
                            if (semester == "5" || semester == "6")
                            {
                                year = "3rd Year";
                                yr = "3";
                                headflag3 = true;
                            }
                        }
                        if (headflag4 == false)
                        {
                            if (semester == "7" || semester == "8")
                            {
                                year = "4th Year";
                                yr = "4";
                                headflag4 = true;
                            }
                        }
                        if (headflag5 == false)
                        {
                            if (semester == "9" || semester == "10")
                            {
                                year = "5th Year";
                                yr = "5";
                                headflag5 = true;
                            }
                        }
                        int cnt = 0;
                        count6 = dv.Count;

                        if (year == "1st Year" && yr == "1")
                        {
                            if (count6 > 0)
                            {
                                yeartest.Add(1, year + "-" + count6);
                                for (int n = 0; n < count6; n++)
                                {
                                    //if (dv[n]["sections"] != "")
                                    //{
                                        col++;
                                        columncnt = columncnt + 4;
                                        if (headflag == false)
                                        {

                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Text = year;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = true;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Note = yr;
                                            year1.Add(columncnt - 4, yr);
                                            headflag = true;
                                        }
                                        sectiontest.Add(col, dv[n]["sections"].ToString());
                                        year2.Add(columncnt - 4, yr + "-" + dv[n]["sections"].ToString());
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Text = dv[n]["sections"].ToString();
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = true;
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Note = yr + "-" + dv[n]["sections"].ToString();
                                        //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4, 1, 4);

                                        textpass = new System.Text.StringBuilder("Strength");

                                        AddTableColumn(dtdep, textpass);

                                        textpass = new System.Text.StringBuilder("Appear");

                                        AddTableColumn(dtdep, textpass);

                                        textpass = new System.Text.StringBuilder("No of Pass");

                                        AddTableColumn(dtdep, textpass);
                                        textpass = new System.Text.StringBuilder("%");

                                        AddTableColumn(dtdep, textpass);


                                        cnt = cnt + 4;
                                        frstrow = frstrow + 4;
                                   // }
                                }


                            }
                        }
                        if (year == "2nd Year" && yr == "2")
                        {
                            if (count6 > 0)
                            {
                                yeartest.Add(2, year + "-" + count6);
                                for (int n = 0; n < count6; n++)
                                {
                                    //if (dv[n]["sections"] != "")
                                    //{
                                        col++;
                                        columncnt = columncnt + 4;
                                        if (headflag == false)
                                        {
                                            year1.Add(columncnt - 4, yr);
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Text = year;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = true;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Note = yr;
                                            headflag = true;
                                        }
                                        year2.Add(columncnt - 4, yr + "-" + dv[n]["sections"].ToString());
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Text = dv[n]["sections"].ToString();
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = true;
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Note = yr + "-" + dv[n]["sections"].ToString();
                                        //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4, 1, 4);

                                        sectiontest.Add(col, dv[n]["sections"].ToString());

                                        textpass = new System.Text.StringBuilder("Strength");

                                        AddTableColumn(dtdep, textpass);

                                        textpass = new System.Text.StringBuilder("Appear");

                                        AddTableColumn(dtdep, textpass);

                                        textpass = new System.Text.StringBuilder("No of Pass");

                                        AddTableColumn(dtdep, textpass);
                                        textpass = new System.Text.StringBuilder("%");

                                        AddTableColumn(dtdep, textpass);

                                        cnt = cnt + 4;
                                        scndrow = scndrow + 4;
                                  //  }
                                }
                                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - cnt, 1, cnt);

                            }
                        }
                        if (year == "3rd Year" && yr == "3")
                        {
                            if (count6 > 0)
                            {
                                yeartest.Add(3, year + "-" + count6);
                                for (int n = 0; n < count6; n++)
                                {
                                    //if (dv[n]["sections"] != "")
                                    //{
                                        col++;
                                        columncnt = columncnt + 4;
                                        if (headflag == false)
                                        {
                                            year1.Add(columncnt - 4, yr);
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Text = year;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = true;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Note = yr;
                                            headflag = true;
                                        }
                                        year2.Add(columncnt - 4, yr + "-" + dv[n]["sections"].ToString());
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Text = dv[n]["sections"].ToString();
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = true;
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Note = yr + "-" + dv[n]["sections"].ToString();
                                        //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4, 1, 4);
                                        sectiontest.Add(col, dv[n]["sections"].ToString());


                                        textpass = new System.Text.StringBuilder("Strength");

                                        AddTableColumn(dtdep, textpass);

                                        textpass = new System.Text.StringBuilder("Appear");

                                        AddTableColumn(dtdep, textpass);

                                        textpass = new System.Text.StringBuilder("No of Pass");

                                        AddTableColumn(dtdep, textpass);
                                        textpass = new System.Text.StringBuilder("%");

                                        AddTableColumn(dtdep, textpass);

                                        cnt = cnt + 4;
                                        thrdrow = thrdrow + 4;
                                    //}
                                }
                                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - cnt, 1, cnt);

                            }
                        }
                        if (year == "4th Year" && yr == "4")
                        {
                            if (count6 > 0)
                            {
                                yeartest.Add(4, year + "-" + count6);
                                for (int n = 0; n < count6; n++)
                                {
                                    //if (dv[n]["sections"] != "")
                                    //{
                                        col++;
                                        columncnt = columncnt + 4;
                                        if (headflag == false)
                                        {
                                            year1.Add(columncnt - 4, yr);
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Text = year;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = true;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Note = yr;
                                            headflag = true;
                                        }
                                        year2.Add(columncnt - 4, yr + "-" + dv[n]["sections"].ToString());
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Text = dv[n]["sections"].ToString();
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = true;
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Note = yr + "-" + dv[n]["sections"].ToString();
                                        //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4, 1, 4);

                                        sectiontest.Add(col, dv[n]["sections"].ToString());


                                        textpass = new System.Text.StringBuilder("Strength");

                                        AddTableColumn(dtdep, textpass);

                                        textpass = new System.Text.StringBuilder("Appear");

                                        AddTableColumn(dtdep, textpass);

                                        textpass = new System.Text.StringBuilder("No of Pass");

                                        AddTableColumn(dtdep, textpass);
                                        textpass = new System.Text.StringBuilder("%");

                                        AddTableColumn(dtdep, textpass);

                                        cnt = cnt + 4;
                                        frthrow = frthrow + 4;
                                  //  }
                                }
                                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - cnt, 1, cnt);

                            }
                        }
                        if (year == "5th Year" && yr == "5")
                        {
                            if (count6 > 0)
                            {
                                yeartest.Add(5, year + "-" + count6);

                                for (int n = 0; n < count6; n++)
                                {
                                    //if (dv[n]["sections"] != "")
                                    //{
                                        col++;
                                        columncnt = columncnt + 4;
                                        if (headflag == false)
                                        {
                                            year1.Add(columncnt - 4, yr);
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Text = year;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = true;
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Note = yr; 
                                            headflag = true;
                                        }
                                        year2.Add(columncnt - 4, yr + "-" + dv[n]["sections"].ToString());
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Text = dv[n]["sections"].ToString();
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = true;
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Note = yr + "-" + dv[n]["sections"].ToString();
                                        //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4, 1, 4);

                                        sectiontest.Add(col, dv[n]["sections"].ToString());


                                        textpass = new System.Text.StringBuilder("Strength");

                                        AddTableColumn(dtdep, textpass);

                                        textpass = new System.Text.StringBuilder("Appear");

                                        AddTableColumn(dtdep, textpass);

                                        textpass = new System.Text.StringBuilder("No of Pass");

                                        AddTableColumn(dtdep, textpass);
                                        textpass = new System.Text.StringBuilder("%");

                                        AddTableColumn(dtdep, textpass);

                                        cnt = cnt + 4;
                                        fifthrow = fifthrow + 4;
                                  //  }
                                }
                                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - cnt, 1, cnt);

                            }
                        }
                    }
                }
            }

        }

        catch
        {
            lblerror.Visible = true;
            lblerror.Text = "No Records Found";
            gridviewdep.Visible = false;
        }
    }
    protected void btnGo_Click1(object sender, EventArgs e)
    {
        try
        {
            if (ddlTest.Items.Count == 0)
            {
                lblerror.Text = "No Test Conducted";
                lblerror.Visible = true;

                return;
            }
            if (ddlTest.SelectedItem.ToString() == "--Select--")
            {
                lblerror.Text = "Please Select Test ";
                lblerror.Visible = true;

                return;
            }
            clear();
            DataSet ds456 = new DataSet();
            DataView dv_dergree_data4 = new DataView();
            DataView dv_dergree_data3 = new DataView();
            DataView dv_dergree_data2 = new DataView();
            gridviewdep.Visible = true;

            loadheader();
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            txtexcelname.Text = "";
            BtnPrint.Visible = true;
            btnExcel.Visible = true;

            string collegecode1 = ddlcollege.SelectedValue.ToString();
            string Sqlstr123 = "Select distinct course.Course_Name + '-' +dept_acronym as dept,degree.degree_code from department,degree,course,registration as r where  r.college_code='" + collegecode1 + "'  and r.degree_code=degree.Degree_Code  and degree.course_id = course.course_id  and degree.dept_code = department.dept_code and r.cc =0 and r.delflag = 0 and r.exam_flag <>'debar' and r.current_semester is not null and r.batch_year is not null order by degree.degree_code";
            ds456 = d2.select_method_wo_parameter(Sqlstr123, "Text");
            int SNo = 0;


            if (ds456.Tables[0].Rows.Count > 0)
            {
                //for (int i = 0; i < ds456.Tables[0].Rows.Count; i++)
                //{
                //    drper = dtdep.NewRow();
                //    SNo++;
                //    drper["SNo"] = SNo.ToString();
                //    drper["Course"] = ds456.Tables[0].Rows[i]["dept"].ToString();
                //    drper["degree_code"] = ds456.Tables[0].Rows[i]["degree_code"].ToString();
                //    dtdep.Rows.Add(drper);
                //}

            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Records Found";
                gridviewdep.Visible = false;
                btnsection.Visible = false;
                lblsctnn.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                BtnPrint.Visible = false;
                return;
            }




            string insquery = "if exists(select * from sysobjects where name='tbl_mark_calcu' and Type='U') drop table tbl_mark_calcu ;create table tbl_mark_calcu (roll_no nvarchar(25),totalmarks float,percentage float)";
            int a = d2.update_method_wo_parameter(insquery, "Text");
            string test = ddlTest.SelectedItem.ToString();
            string strquery = "select distinct c.criteria,c.criteria_no from criteriaforinternal c,registration r,syllabus_master s where  r.college_code='" + collegecode1 + "' and r.degree_code=s.degree_code and  r.batch_year=s.batch_year and r.current_semester=s.semester and c.syll_code=s.syll_code and cc=0 and delflag=0";
            strquery = strquery + "and r.exam_flag<>'debar' AND C.criteria='" + test + "'";
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.select_method_wo_parameter(strquery, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                string criteriano12 = string.Empty;
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    string value = ds2.Tables[0].Rows[i]["criteria_no"].ToString();
                    if (value.Trim() != "")
                    {
                        if (criteriano12 == "")
                        {
                            criteriano12 = value;
                        }
                        else
                        {
                            criteriano12 = criteriano12 + "," + value;
                        }
                    }


                }
                ds4.Dispose();
                ds4.Reset();
                hat.Clear();
                string collegecode13 = ddlcollege.SelectedValue.ToString();
                string strquery12 = "  select isnull(count(distinct rt.roll_no),0) as 'pass',r.degree_code,r.Current_Semester,r.Batch_Year,r.Sections  from registration r, applyn a , result rt,exam_type et, subject s where a.app_no=r.app_no and  et.subject_no=s.subject_no and  RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no in(" + criteriano12 + ") and r.roll_no=rt.roll_no  and cc=0 and exam_flag <> 'DEBAR' and delflag=0  group by r.degree_code,r.Current_Semester,r.Batch_Year ,r.Sections ;       select isnull(count(distinct rt.roll_no),0) as 'pass',r.degree_code,r.Current_Semester,r.Batch_Year,r.Sections  from registration r, applyn a , result rt,exam_type et, subject s where a.app_no=r.app_no and  et.subject_no=s.subject_no and  RollNo_Flag<>0 and et.exam_code=rt.exam_code  and (marks_obtained<et.min_mark  and marks_obtained<>'-2' and marks_obtained<>'-3' ) and et.criteria_no in(" + criteriano12 + ") and r.roll_no=rt.roll_no  and cc=0 and exam_flag <> 'DEBAR' and delflag=0  group by r.degree_code,r.Current_Semester,r.Batch_Year ,r.Sections ;select isnull(count(distinct rt.roll_no),0) as 'pass',r.degree_code,r.Current_Semester,r.Batch_Year,r.Sections  from registration r, applyn a , result rt,exam_type et, subject s where a.app_no=r.app_no and  et.subject_no=s.subject_no and  RollNo_Flag<>0 and et.exam_code=rt.exam_code  and (marks_obtained>=0  or marks_obtained='-2' or marks_obtained='-3') and et.criteria_no in(" + criteriano12 + ") and r.roll_no=rt.roll_no  and cc=0 and exam_flag <> 'DEBAR' and delflag=0  group by r.degree_code,r.Current_Semester,r.Batch_Year ,r.Sections";
                ds4 = d2.select_method_wo_parameter(strquery12, "Text");
                string gettotalstudent = "select Batch_Year,degree_code,Current_Semester,Sections,count(roll_no) strength from Registration where cc=0 and exam_flag <> 'DEBAR' and delflag=0  group by Batch_Year,degree_code,Current_Semester,Sections ";
                DataSet dsstuacount = dacces2.select_method_wo_parameter(gettotalstudent, "Text");
                Double sectotal = 0;
                Double secpass = 0;
                Double percentage = 0;
                string secc = "";
                string secc1 = "";
                Double secpass1 = 0;

                if (ds4.Tables[0].Rows.Count > 0)
                {
                    for (int co = 0; co < ds456.Tables[0].Rows.Count; co++)
                    {
                        drper = dtdep.NewRow();
                        SNo++;
                        drper["SNo"] = SNo.ToString();
                        drper["Course"] = ds456.Tables[0].Rows[co]["dept"].ToString();
                        // drper["degree_code"] = ds456.Tables[0].Rows[co]["degree_code"].ToString();
                        dtdep.Rows.Add(drper);

                        ds4.Tables[0].DefaultView.RowFilter = "degree_code = '" + ds456.Tables[0].Rows[co]["degree_code"].ToString() + "'";
                        dv_dergree_data2 = ds4.Tables[0].DefaultView;

                        if (dv_dergree_data2.Count > 0)
                        {
                            for (int s = 0; s < dv_dergree_data2.Count; s++)
                            {
                                string batcd = dv_dergree_data2[s]["Batch_Year"].ToString();
                                string degre = dv_dergree_data2[s]["degree_code"].ToString();
                                string sem = dv_dergree_data2[s]["Current_Semester"].ToString();
                                string sec = dv_dergree_data2[s]["sections"].ToString();

                                if (sec == "")
                                {
                                    secc = "";
                                }
                                else
                                {
                                    secc = "and rt.Sections='" + sec + "' ";
                                }

                                string yr = "";
                                string yr1 = "";
                                if (sem == "1" || sem == "2")
                                {
                                    yr = "1" + "-" + sec;
                                    yr1 = "1";
                                }
                                if (sem == "3" || sem == "4")
                                {
                                    yr = "2" + "-" + sec;
                                    yr1 = "2";
                                }
                                if (sem == "5" || sem == "6")
                                {
                                    yr = "3" + "-" + sec;
                                    yr1 = "3";
                                }
                                if (sem == "7" || sem == "8")
                                {
                                    yr = "4" + "-" + sec;
                                    yr1 = "4";
                                }
                                if (sem == "9" || sem == "10")
                                {
                                    yr = "5" + "-" + sec;
                                    yr1 = "5";
                                }

                                if (sec == "")
                                {
                                    secc = "";
                                }
                                else
                                {
                                    secc1 = "and Sections='" + sec + "' ";
                                }


                                for (int col = 2; col < dtdep.Columns.Count; col++)
                                {
                                    if (secc != "")
                                    {
                                        ds4.Tables[1].DefaultView.RowFilter = "degree_code = '" + ds456.Tables[0].Rows[co]["degree_code"].ToString() + "' and Batch_Year = '" + batcd + "' and Current_Semester='" + sem + "' " + secc1 + "";
                                        dv_dergree_data3 = ds4.Tables[1].DefaultView;

                                        ds4.Tables[2].DefaultView.RowFilter = "degree_code = '" + ds456.Tables[0].Rows[co]["degree_code"].ToString() + "' and Batch_Year = '" + batcd + "' and Current_Semester='" + sem + "' " + secc1 + "";
                                        dv_dergree_data4 = ds4.Tables[2].DefaultView;

                                        if (year2.ContainsKey(col))
                                        {
                                            if (year2[col] == yr)
                                            {

                                                sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());

                                                string pass22 = "";
                                                string pass22p = "";
                                                if (dv_dergree_data3.Count > 0)
                                                {
                                                    pass22 = dv_dergree_data3[0]["pass"].ToString();
                                                }
                                                else
                                                {
                                                    pass22 = "0";
                                                }
                                                if (pass22 == "")
                                                {
                                                    pass22 = "0";
                                                }
                                                int tolpss = Convert.ToInt32(sectotal) - Convert.ToInt32(pass22);

                                                if (dv_dergree_data4.Count > 0)
                                                {
                                                    pass22p = dv_dergree_data4[0]["pass"].ToString();
                                                }
                                                else
                                                {
                                                    pass22p = "0";
                                                }

                                                secpass1 = Convert.ToDouble(pass22p);
                                                secpass = Convert.ToDouble(tolpss);

                                                percentage = Math.Round(secpass / secpass1 * 100, 2);

                                                dsstuacount.Tables[0].DefaultView.RowFilter = "degree_code = '" + ds456.Tables[0].Rows[co]["degree_code"].ToString() + "' and Batch_Year = '" + batcd + "' and Current_Semester='" + sem + "' " + secc1 + "";
                                                DataView dvstucou = dsstuacount.Tables[0].DefaultView;
                                                string stucou = "0";
                                                if (dvstucou.Count > 0)
                                                {
                                                    stucou = dvstucou[0]["strength"].ToString();
                                                }

                                                dtdep.Rows[dtdep.Rows.Count - 1][col] = Convert.ToString(stucou);
                                                dtdep.Rows[dtdep.Rows.Count - 1][col + 1] = Convert.ToString(secpass1);
                                                dtdep.Rows[dtdep.Rows.Count - 1][col + 2] = Convert.ToString(tolpss);
                                                dtdep.Rows[dtdep.Rows.Count - 1][col + 3] = Convert.ToString(percentage);
                                                col++;
                                                if (yr1 == "1")
                                                {
                                                    string pass2211 = "";

                                                    sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());

                                                    if (dv_dergree_data3.Count > 0)
                                                    {
                                                        pass2211 = dv_dergree_data3[0]["pass"].ToString();
                                                    }
                                                    else
                                                    {
                                                        pass2211 = "0";
                                                    }

                                                    if (pass2211 == "")
                                                    {
                                                        pass2211 = "0";
                                                    }
                                                    int tolpss11 = Convert.ToInt32(sectotal) - Convert.ToInt32(pass2211);

                                                    frst = frst + tolpss11;
                                                    frstper = frstper + percentage;
                                                    frstperc++;
                                                }
                                                if (yr1 == "2")
                                                {
                                                    sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());
                                                    string pass2221 = "";

                                                    if (dv_dergree_data3.Count > 0)
                                                    {
                                                        pass2221 = dv_dergree_data3[0]["pass"].ToString();
                                                    }
                                                    else
                                                    {
                                                        pass2221 = "0";
                                                    }


                                                    if (pass2221 == "")
                                                    {
                                                        pass2221 = "0";
                                                    }
                                                    int tolpss21 = Convert.ToInt32(sectotal) - Convert.ToInt32(pass2221);

                                                    snd = snd + tolpss21;
                                                    sndper = sndper + percentage;
                                                    sndperc++;
                                                }
                                                if (yr1 == "3")
                                                {
                                                    string pass2231 = "";
                                                    sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());
                                                    if (dv_dergree_data3.Count > 0)
                                                    {
                                                        pass2231 = dv_dergree_data3[0]["pass"].ToString();
                                                    }
                                                    else
                                                    {
                                                        pass2231 = "0";
                                                    }


                                                    if (pass2231 == "")
                                                    {
                                                        pass2231 = "0";
                                                    }
                                                    int tolpss31 = Convert.ToInt32(sectotal) - Convert.ToInt32(pass2231);

                                                    thrd = thrd + tolpss31;
                                                    thrdper = thrdper + percentage;
                                                    thrdperc++;
                                                }
                                                if (yr1 == "4")
                                                {
                                                    string pass2241 = "";
                                                    sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());
                                                    if (dv_dergree_data3.Count > 0)
                                                    {
                                                        pass2241 = dv_dergree_data3[0]["pass"].ToString();
                                                    }
                                                    else
                                                    {
                                                        pass2241 = "0";
                                                    }


                                                    if (pass2241 == "")
                                                    {
                                                        pass2241 = "0";
                                                    }
                                                    int tolpss41 = Convert.ToInt32(sectotal) - Convert.ToInt32(pass2241);

                                                    frth = frth + tolpss41;
                                                    frthper = frthper + percentage;
                                                    frthperc++;
                                                }
                                                if (yr1 == "5")
                                                {
                                                    string pass2251 = "";
                                                    sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());
                                                    if (dv_dergree_data3.Count > 0)
                                                    {
                                                        pass2251 = dv_dergree_data3[0]["pass"].ToString();
                                                    }
                                                    else
                                                    {
                                                        pass2251 = "0";
                                                    }

                                                    if (pass2251 == "")
                                                    {
                                                        pass2251 = "0";
                                                    }
                                                    int tolpss51 = Convert.ToInt32(sectotal) - Convert.ToInt32(pass2251);

                                                    fifth = fifth + tolpss51;
                                                    fifthper = fifthper + percentage;
                                                    fifthperc++;
                                                }
                                            }
                                        }
                                    }
                                    if (secc == "")
                                    {
                                        ds4.Tables[1].DefaultView.RowFilter = "degree_code = '" + ds456.Tables[0].Rows[co]["degree_code"].ToString() + "' and Batch_Year = '" + batcd + "' and Current_Semester='" + sem + "'";
                                        dv_dergree_data3 = ds4.Tables[1].DefaultView;

                                        ds4.Tables[2].DefaultView.RowFilter = "degree_code = '" + ds456.Tables[0].Rows[co]["degree_code"].ToString() + "' and Batch_Year = '" + batcd + "' and Current_Semester='" + sem + "'";
                                        dv_dergree_data4 = ds4.Tables[2].DefaultView;
                                        if (year1.ContainsKey(col))
                                        {
                                            if (year1[col] == yr1)
                                            {
                                                //  string strquery2334 = "select COUNT(1) as total,Sections from Registration where   degree_code = '" + degre + "' and Batch_Year = '" + batcd + "' and Sections='" + sec + "'  and CC=0 and DelFlag=0 group by Sections order by Sections asc ";
                                                // string strquery2334 = "select isnull(count(distinct rt.roll_no),0) AS total,R.Sections from result rt,registration r,criteriaforinternal c,exam_type et where rt.exam_Code=et.exam_code and c.Criteria_no=et.criteria_no and  rt.roll_no=r.roll_no and  r.degree_code = '" + degre + "' and r.Batch_Year = '" + batcd + "'  and r.cc=0 and r.exam_flag <>'DEBAR'  and r.delflag=0 and r.RollNo_Flag<>0  group by R.Sections order by R.Sections asc ";
                                                //string strquery2334 = "select isnull(count(distinct rt.roll_no),0) as 'total',rt.degree_code,rt.Current_Semester,rt.Batch_Year,rt.Sections  from result r,registration rt,criteriaforinternal c,exam_type e,syllabus_master sm where r.exam_code=e.exam_code and e.criteria_no=c.Criteria_no and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and e.batch_year=rt.Batch_Year and sm.Batch_Year=e.batch_year  and sm.semester=rt.Current_Semester and rt.college_code=" + collegecode13 + "  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and  c.Criteria_no  in(" + criteriano12 + ") and  rt.degree_code = '" + degre + "' and rt.Batch_Year = '" + batcd + "'   and rt.delflag=0 and rt.RollNo_Flag<>0   group by rt.degree_code,rt.Current_Semester,rt.Batch_Year,rt.Sections ";
                                                //ds33.Dispose();
                                                //ds33.Reset();
                                                //ds33 = d2.select_method_wo_parameter(strquery2334, "Text");
                                                sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());
                                                string pass22 = "";
                                                string pass22ps = "";
                                                if (dv_dergree_data3.Count > 0)
                                                {
                                                    pass22 = dv_dergree_data3[0]["pass"].ToString();
                                                }
                                                else
                                                {
                                                    pass22 = "0";
                                                }
                                                if (pass22 == "")
                                                {
                                                    pass22 = "0";
                                                }
                                                int tolpss = Convert.ToInt32(sectotal) - Convert.ToInt32(pass22);

                                                if (dv_dergree_data4.Count > 0)
                                                {
                                                    pass22ps = dv_dergree_data4[0]["pass"].ToString();
                                                }
                                                else
                                                {
                                                    pass22ps = "0";
                                                }
                                                secpass1 = Convert.ToDouble(pass22ps);
                                                secpass = Convert.ToDouble(tolpss);

                                                percentage = Math.Round(secpass / secpass1 * 100, 2);

                                                dsstuacount.Tables[0].DefaultView.RowFilter = "degree_code = '" + ds456.Tables[0].Rows[co]["degree_code"].ToString() + "' and Batch_Year = '" + batcd + "' and Current_Semester='" + sem + "'";
                                                DataView dvstucou = dsstuacount.Tables[0].DefaultView;
                                                string stucou = "0";
                                                if (dvstucou.Count > 0)
                                                {
                                                    stucou = dvstucou[0]["strength"].ToString();
                                                }

                                                dtdep.Rows[dtdep.Rows.Count - 1][col] = Convert.ToString(stucou);
                                                dtdep.Rows[dtdep.Rows.Count - 1][col + 1] = Convert.ToString(secpass1);
                                                dtdep.Rows[dtdep.Rows.Count - 1][col + 2] = Convert.ToString(tolpss);
                                                dtdep.Rows[dtdep.Rows.Count - 1][col + 3] = Convert.ToString(percentage);

                                                col++;
                                                if (yr1 == "1")
                                                {
                                                    string pass2211 = "";
                                                    sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());
                                                    if (dv_dergree_data3.Count > 0)
                                                    {
                                                        pass2211 = dv_dergree_data3[0]["pass"].ToString();
                                                    }
                                                    else
                                                    {
                                                        pass2211 = "0";
                                                    }
                                                    if (pass2211 == "")
                                                    {
                                                        pass2211 = "0";
                                                    }
                                                    int tolpss11 = Convert.ToInt32(sectotal) - Convert.ToInt32(pass2211);

                                                    frst = frst + tolpss11;
                                                    frstper = frstper + percentage;
                                                    frstperc++;
                                                }
                                                if (yr1 == "2")
                                                {
                                                    string pass2221 = "";
                                                    sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());
                                                    if (dv_dergree_data3.Count > 0)
                                                    {
                                                        pass2221 = dv_dergree_data3[0]["pass"].ToString();
                                                    }
                                                    else
                                                    {
                                                        pass2221 = "0";
                                                    }
                                                    if (pass2221 == "")
                                                    {
                                                        pass2221 = "0";
                                                    }
                                                    int tolpss21 = Convert.ToInt32(sectotal) - Convert.ToInt32(pass2221);

                                                    snd = snd + tolpss21;
                                                    sndper = sndper + percentage;
                                                    sndperc++;
                                                }
                                                if (yr1 == "3")
                                                {
                                                    string pass2231 = "";
                                                    sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());
                                                    if (dv_dergree_data3.Count > 0)
                                                    {
                                                        pass2231 = dv_dergree_data3[0]["pass"].ToString();
                                                    }
                                                    else
                                                    {
                                                        pass2231 = "0";
                                                    }
                                                    if (pass2231 == "")
                                                    {
                                                        pass2231 = "0";
                                                    }
                                                    int tolpss31 = Convert.ToInt32(sectotal) - Convert.ToInt32(pass2231);

                                                    thrd = thrd + tolpss31;
                                                    thrdper = thrdper + percentage;
                                                    thrdperc++;
                                                }
                                                if (yr1 == "4")
                                                {
                                                    string pass2241 = "";
                                                    sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());
                                                    if (dv_dergree_data3.Count > 0)
                                                    {
                                                        pass2241 = dv_dergree_data3[0]["pass"].ToString();
                                                    }
                                                    else
                                                    {
                                                        pass2241 = "0";
                                                    }
                                                    if (pass2241 == "")
                                                    {
                                                        pass2241 = "0";
                                                    }
                                                    int tolpss41 = Convert.ToInt32(sectotal) - Convert.ToInt32(pass2241);

                                                    frth = frth + tolpss41;
                                                    frthper = frthper + percentage;
                                                    frthperc++;
                                                }
                                                if (yr1 == "5")
                                                {
                                                    string pass2251 = "";
                                                    sectotal = Convert.ToDouble(dv_dergree_data2[s]["pass"].ToString());
                                                    if (dv_dergree_data3.Count > 0)
                                                    {
                                                        pass2251 = dv_dergree_data3[0]["pass"].ToString();
                                                    }
                                                    else
                                                    {
                                                        pass2251 = "0";
                                                    }
                                                    if (pass2251 == "")
                                                    {
                                                        pass2251 = "0";
                                                    }
                                                    int tolpss51 = Convert.ToInt32(sectotal) - Convert.ToInt32(pass2251);

                                                    fifth = fifth + tolpss51;
                                                    fifthper = fifthper + percentage;
                                                    fifthperc++;
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        //dtdep.Rows.Add(drper);
                    }
                    //  dtdep.Columns.Remove("degree_code");
                    // dtdep.Columns.Remove("year");

                    drper = dtdep.NewRow();
                    drper["SNo"] = "Total";
                    dtdep.Rows.Add(drper);

                    drper = dtdep.NewRow();
                    drper["SNo"] = "%";
                    dtdep.Rows.Add(drper);

                    drper = dtdep.NewRow();
                    drper["SNo"] = "Overall";
                    dtdep.Rows.Add(drper);

                    drper = dtdep.NewRow();
                    drper["SNo"] = "Overall %";
                    dtdep.Rows.Add(drper);


                    int trr = frstrow;
                    int scc = scndrow + frstrow;
                    int frr = thrdrow + scndrow + frstrow;
                    int fihh = frthrow + thrdrow + scndrow + frstrow + fifthrow;
                    int sell = fihh - frstrow;
                    int trrrs = sell - scndrow;
                    int frrrs = trrrs - thrdrow;
                    int fiii = frrrs - frthrow;
                    int intperss = 0;

                    string frst1 = "";
                    string frst2 = "";
                    string frst3 = "";
                    string frst4 = "";
                    string frst5 = "";

                    double frst11 = 0;
                    double frst22 = 0;
                    double frst33 = 0;
                    double frst44 = 0;
                    double frst55 = 0;

                  

                    int colcnt = 0;

                    if (frstrow > 0)
                    {
                        dtdep.Rows[dtdep.Rows.Count - 4][2] = Convert.ToString(frst);

                        double total_frstclg1 = Math.Round(frstper / frstperc, 2);
                        frst1 = Convert.ToString(total_frstclg1);
                        if (frst1 != "NaN")
                        {
                            frst11 = total_frstclg1;
                            intperss++;
                        }
                        dtdep.Rows[dtdep.Rows.Count - 3][2] = Convert.ToString(total_frstclg1);


                        if (yeartest.ContainsKey(1))
                        {
                            string value = yeartest[1];
                            string[] spilt = value.Split('-');
                            int colspan = Convert.ToInt32(spilt[1]);

                         

                            colcnt = colspan * 4;
                        }


                    }

                    if (scndrow > 0)
                    {
                        dtdep.Rows[dtdep.Rows.Count - 4][colcnt + 2] = Convert.ToString(snd);

                        double total_frstclg2 = Math.Round(sndper / sndperc, 2);
                        frst2 = Convert.ToString(total_frstclg2);
                        if (frst2 != "NaN")
                        {
                            frst22 = total_frstclg2;
                            intperss++;
                        }

                        dtdep.Rows[dtdep.Rows.Count - 3][colcnt + 2] = Convert.ToString(total_frstclg2);
                        if (yeartest.ContainsKey(2))
                        {
                            string value = yeartest[2];
                            string[] spilt = value.Split('-');
                            int colspan = Convert.ToInt32(spilt[1]);


                            colcnt = colcnt + (colspan * 4);
                        }

                    }



                    if (thrdrow > 0)
                    {
                        dtdep.Rows[dtdep.Rows.Count - 4][colcnt + 2] = Convert.ToString(thrd);

                        double total_frstclg3 = Math.Round(thrdper / thrdperc, 2);
                        frst3 = Convert.ToString(total_frstclg3);
                        if (frst3 != "NaN")
                        {
                            frst33 = total_frstclg3;
                            intperss++;
                        }
                        dtdep.Rows[dtdep.Rows.Count - 3][colcnt + 2] = Convert.ToString(total_frstclg3);


                        if (yeartest.ContainsKey(3))
                        {
                            string value = yeartest[3];
                            string[] spilt = value.Split('-');
                            int colspan = Convert.ToInt32(spilt[1]);

                            
                            colcnt = colcnt + (colspan * 4);
                        }


                    }

                    if (frthrow > 0)
                    {
                        dtdep.Rows[dtdep.Rows.Count - 4][colcnt + 2] = Convert.ToString(frth);

                        double total_frstclg4 = Math.Round(frthper / frthperc, 2);
                        frst4 = Convert.ToString(total_frstclg4);
                        if (frst4 != "NaN")
                        {
                            frst44 = total_frstclg4;
                            intperss++;
                        }
                        dtdep.Rows[dtdep.Rows.Count - 3][colcnt + 2] = Convert.ToString(total_frstclg4);


                        if (yeartest.ContainsKey(4))
                        {
                            string value = yeartest[4];
                            string[] spilt = value.Split('-');
                            int colspan = Convert.ToInt32(spilt[1]);

                          

                            colcnt = colcnt + (colspan * 4);
                        }


                    }

                    if (fifthrow > 0)
                    {
                        dtdep.Rows[dtdep.Rows.Count - 4][colcnt + 2] = Convert.ToString(fifth);

                        double total_frstclg5 = Math.Round(fifthper / fifthperc, 2);
                        frst5 = Convert.ToString(total_frstclg5);
                        if (frst5 != "NaN")
                        {
                            frst55 = total_frstclg5;
                            intperss++;
                        }
                        dtdep.Rows[dtdep.Rows.Count - 3][colcnt + 2] = Convert.ToString(total_frstclg5);


                        if (yeartest.ContainsKey(5))
                        {
                            string value = yeartest[1];
                            string[] spilt = value.Split('-');
                            int colspan = Convert.ToInt32(spilt[1]);


                            colcnt = colcnt + (colspan * 4);
                        }


                    }
                    int totalover = frst + snd + thrd + frth + fifth;


                    dtdep.Rows[dtdep.Rows.Count - 2][2] = Convert.ToString(totalover);

                    int tollpercont = frstperc + sndperc + thrdperc + frthperc + fifthperc;
                    double tollperr = frstper + sndper + thrdper + frthper + fifthper;
                    double totaloverper = Math.Round(tollperr / tollpercont, 2);

                    double overpres = frst11 + frst22 + frst33 + frst44 + frst55;

                    double pertollss = Math.Round(overpres / intperss, 2);



                    dtdep.Rows[dtdep.Rows.Count - 1][2] = Convert.ToString(pertollss);
                  
                    if (dtdep.Rows.Count > 0 && dtdep.Columns.Count > 0)
                    {
                        gridviewdep.DataSource = dtdep;
                        gridviewdep.DataBind();
                        gridviewdep.Visible = true;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnExcel.Visible = true;
                        BtnPrint.Visible = true;
                        btnsection.Visible = true;
                        lblsctnn.Visible = true;

                      

                        int columncn = 2;
                        int columncn1 = 3;
                        int colct = 2;
                        int h = 0;
                        foreach (KeyValuePair<int, string> dr in yeartest)
                        {
                            h++;
                            string value = dr.Value.ToString();
                            string[] spilt = value.Split('-');
                            int colspan = Convert.ToInt32(spilt[1]);
                            if (colspan == 0)
                                colspan = 1;

                            gridviewdep.Rows[dtdep.Rows.Count - 3].Cells[columncn].HorizontalAlign = HorizontalAlign.Center;
                            gridviewdep.Rows[dtdep.Rows.Count - 3].Cells[columncn].ColumnSpan = (colspan * 4);
                           
                            gridviewdep.Rows[dtdep.Rows.Count - 4].Cells[columncn].HorizontalAlign = HorizontalAlign.Center;
                            gridviewdep.Rows[dtdep.Rows.Count - 4].Cells[columncn].ColumnSpan = (colspan * 4);
                            
                            for (int m = columncn1; m < (colspan * 4) + 2; m++)
                            {
                                gridviewdep.Rows[dtdep.Rows.Count - 4].Cells[m].Visible = false;
                            }
                            for (int m = columncn1; m < (colspan * 4) + 2; m++)
                            {
                                gridviewdep.Rows[dtdep.Rows.Count - 3].Cells[m].Visible = false;
                            }
                            int co = colspan * 4;

                            columncn = co + 1;
                          //  if (h != 1)
                                colct = colct + columncn;
                           
                          //  colct++;
                           
                            columncn1 = columncn + columncn1 + 1;
                        }

                        //foreach (KeyValuePair<int, string> dr in yeartest)
                        //{
                        //    string value = dr.Value.ToString();
                        //    string[] spilt = value.Split('-');
                        //    int colspan = Convert.ToInt32(spilt[1]);
                        //    if (colspan == 0)
                        //        colspan = 1;

                        //    gridviewdep.Rows[
                        //}

                     
                    }

                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "No Records Found";
                    gridviewdep.Visible = false;
                    btnsection.Visible = false;
                    lblsctnn.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    BtnPrint.Visible = false;
                    btnsection.Visible = false;
                    lblsctnn.Visible = false;
                    return;
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Records Found";
                gridviewdep.Visible = false;
                btnsection.Visible = false;
                lblsctnn.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                BtnPrint.Visible = false;
                btnsection.Visible = false;
                lblsctnn.Visible = false;
                return;
            }
        }
        catch
        {

        }
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            GridViewRow HeaderGridRow1 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

            Table table = (Table)gridviewdep.Controls[0];
            TableRow headerow = table.Rows[0];
            for (int i = 0; i < 2; i++)
            {
                TableCell headerCell = headerow.Cells[0];
                headerow.Cells.RemoveAt(0);
                HeaderGridRow.Cells.Add(headerCell);
                headerCell.RowSpan = 3;
            }

            TableCell HeaderCell1 = new TableCell();
            foreach (KeyValuePair<int, string> dr in yeartest)
            {
                string value = dr.Value.ToString();
                string[] spilt = value.Split('-');
                int colspan = Convert.ToInt32(spilt[1]);
                HeaderCell1 = new TableCell();

                if (colspan == 0)
                    colspan = 1;

                HeaderCell1.Text = spilt[0].ToString();
                HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell1.ColumnSpan = colspan * 4;
                HeaderGridRow.Cells.Add(HeaderCell1);
            }
            gridviewdep.Controls[0].Controls.AddAt(0, HeaderGridRow);
            TableCell HeaderCell2 = new TableCell();
            foreach (KeyValuePair<int, string> dr in sectiontest)
            {
                string value = dr.Value.ToString();

                HeaderCell2 = new TableCell();

                HeaderCell2.Text = value.ToString();
                HeaderCell2.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell2.ColumnSpan = 4;
                HeaderGridRow1.Cells.Add(HeaderCell2);
            }


            gridviewdep.Controls[0].Controls.AddAt(1, HeaderGridRow1);

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int j = 1; j < dtdep.Columns.Count; j++)
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
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
    protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();

    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        // Printcontrol.loadspreaddetails(FpSpread1, "Department_performance.aspx", "Over All Department Performance Report @ Date :" + DateTime.Now.ToString("dd/MM/yyyy") + "");
        Printcontrol.Visible = true;
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                //  d2.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                lblerr.Text = "Please Enter Your Report Name";
                lblerr.Visible = true;
            }
        }
        catch
        {
        }
    }

    public void clear()
    {
        gridviewdep.Visible = false;
        btnsection.Visible = false;
        lblsctnn.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        txtexcelname.Text = "";
        lblerror.Visible = false;
        BtnPrint.Visible = false;
        btnExcel.Visible = false;
    }
    //----------------end----------- //Added By thirumalai 29/9/2014
    public bool DefaultView { get; set; }

    public string Batch_Year { get; set; }

    public string strorder { get; set; }

    public string strregorder { get; set; }
}

