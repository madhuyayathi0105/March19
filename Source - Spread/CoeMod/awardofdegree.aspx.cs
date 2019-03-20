using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Configuration;


public partial class awardofdegree : System.Web.UI.Page
{

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string strsem = string.Empty;
    string strbranchname = string.Empty;
    string strsec = string.Empty;
    string strsection = string.Empty;
    string strsection1 = string.Empty;
    string strsecti = "";
    string sqlcmdall1 = "";
    string strbat = "", strdegr = "", strseme = "";
    string sqlcmdbatch = "", sqlcmdbranch = "", sqlcmdseme = "";

    static int batchcnt = 0;
    static int degreecnt = 0;
    static int branchcnt = 0;
    static int sectioncnt = 0;

    int count = 0;
    int count1 = 0;
    int count2 = 0;
    int count3 = 0;
    int count4 = 0;

    string collnamenew1 = "", address1 = "", address2 = "", address3 = "", pincode = "", categery = "", Affliated = "";
    //'---------------------------new
    string address = "";
    string Phoneno = "";
    string Faxno = "";
    string phnfax = "";
    string district = "";
    string email = "";
    string website = "";

    string degree = "", degreetemp = "", veflag = "T", degreeve = "", yearv = "";
    string strveldate = "";

    Hashtable hat = new Hashtable();
    Hashtable all_pass_roll = new Hashtable();
    Hashtable all_pass_criteria = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet1();
    DataSet dgo = new DataSet1();
    DataSet allpass = new DataSet();

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

            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            errmsg.Visible = false;
            perrmsg.Visible = false;
            if (!IsPostBack)
            {

                PRefnosettings.Visible = false;
                FpSpread1.Width = 1000;
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.CommandBar.Visible = true;
                FpSpread1.Sheets[0].SheetName = " ";
                FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = System.Drawing.Color.Black;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].AllowTableCorner = true;

                //---------------page number

                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Right;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Book Antiqua";
                FpSpread1.Pager.ForeColor = System.Drawing.Color.DarkGreen;
                FpSpread1.Pager.BackColor = System.Drawing.Color.Beige;
                FpSpread1.Pager.BackColor = System.Drawing.Color.AliceBlue;
                FpSpread1.Pager.PageCount = 100;

                FpSpread1.Visible = false;
                btnxl.Visible = false;
                LabelE.Visible = false;
                lblnorec.Visible = false;
                errmsg.Visible = false;
                btnprint.Visible = false;
                lblmonth.Visible = false;
                lblyear.Visible = false;
                ddlmonth.Visible = false;
                ddlyear.Visible = false;
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                BindSectionDetailmult(collegecode);
                Bindmultiseme(collegecode);

                ddlmonth.Items.Insert(0, new ListItem(" ", "0"));
                ddlmonth.Items.Insert(1, new ListItem("JANUARY", "1"));
                ddlmonth.Items.Insert(2, new ListItem("FEBUARY", "2"));
                ddlmonth.Items.Insert(3, new ListItem("MARCH", "3"));
                ddlmonth.Items.Insert(4, new ListItem("APRIL", "4"));
                ddlmonth.Items.Insert(5, new ListItem("MAY", "5"));
                ddlmonth.Items.Insert(6, new ListItem("JUNE", "6"));
                ddlmonth.Items.Insert(7, new ListItem("JULY", "7"));
                ddlmonth.Items.Insert(8, new ListItem("AUGUST", "8"));
                ddlmonth.Items.Insert(9, new ListItem("SEPTEMBER", "9"));
                ddlmonth.Items.Insert(10, new ListItem("OCTOBER", "10"));
                ddlmonth.Items.Insert(11, new ListItem("NOVEMBER", "11"));
                ddlmonth.Items.Insert(12, new ListItem("DECEMBER", "12"));

                int year;
                year = Convert.ToInt16(DateTime.Today.Year);
                ddlyear.Items.Clear();
                ddlyear.Items.Insert(0, new ListItem(" ", "0"));
                for (int l = 0; l <= 5; l++)
                {
                    ddlyear.Items.Add(Convert.ToString(year - l));
                }

            }
        }
        catch
        {
        }


    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        string semval = "";
        string strtempseme = "";
        PRefnosettings.Visible = false;
        string latmode1 = "";
        btnprint.Visible = false;
        lblmonth.Visible = false;
        lblyear.Visible = false;
        ddlmonth.Visible = false;
        ddlyear.Visible = false;
        btnxl.Visible = false;

        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].ColumnHeader.Visible = true;
        FpSpread1.Sheets[0].ColumnCount++;
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 6;
        FpSpread1.Sheets[0].ColumnCount = 13;
        FpSpread1.Sheets[0].RowCount = 0;
        //        FpSpread1.Width = 1000;

        try
        {
            if (CheckBox1.Checked == true || CheckBox2.Checked==true)
            {
                format1();
            }
            else
            {
                string regluarrefno = "";
                string lateralrefno = "";
                string transferrefno = "";

                string strqueryref = "select * from Master_Settings where settings='Regular Referance Number'";
                strqueryref = strqueryref + " select * from Master_Settings where settings='Lateral Referance Number'";
                strqueryref = strqueryref + " select * from Master_Settings where settings='Transfer Referance Number'";
                DataSet dsref = d2.select_method_wo_parameter(strqueryref, "Text");
                if (dsref.Tables[0].Rows.Count > 0)
                {
                    regluarrefno = dsref.Tables[0].Rows[0]["value"].ToString().Trim();
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter The Regular Student Reference Number";
                    return;
                }
                if (dsref.Tables[1].Rows.Count > 0)
                {
                    lateralrefno = dsref.Tables[1].Rows[0]["value"].ToString().Trim();
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter The Lateral Entry Student Reference Number";
                    return;
                }
                if (dsref.Tables[2].Rows.Count > 0)
                {
                    transferrefno = dsref.Tables[2].Rows[0]["value"].ToString().Trim();
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter The Transfer Student Reference Number";
                    return;
                }

                string sqlcmdall = "select distinct ROW_NUMBER() OVER (ORDER BY  r.Roll_no) As SrNo,r.roll_no,r.reg_no,r.stud_name,c.course_name as Degree,dt.dept_acronym,Convert(Varchar,d.degree_code)+'-'+Convert(Varchar,r.batch_year)+'-'+Convert(Varchar,mode) +'-'+ Convert(Varchar,current_semester) as v,edu_level,r.mode from registration r,degree d,course c,department dt,mark_entry e where r.degree_code=d.degree_code and d.dept_code=dt.dept_code and c.course_id=d.course_id";

                if (txtbatch.Text != "---Select---" || chklstbatch.Items.Count != null)
                {
                    int itemcount = 0;


                    for (itemcount = 0; itemcount < chklstbatch.Items.Count; itemcount++)
                    {
                        if (chklstbatch.Items[itemcount].Selected == true)
                        {
                            if (strbatch == "")
                                strbatch = chklstbatch.Items[itemcount].Value.ToString();
                            else
                                strbatch = strbatch + "," + chklstbatch.Items[itemcount].Value.ToString();
                        }
                    }


                    if (strbatch != "")
                    {
                        strbatch = " in(" + strbatch + ")";
                    }
                    sqlcmdall = sqlcmdall + " and r.batch_year   " + strbatch + "";
                    sqlcmdbatch = "  batch_year   " + strbatch + "";

                }
                else
                {
                    // errmsg.Visible = true;
                    errmsg.Text = "Plaese Choose Batch";
                }

                if (txtbranch.Text != "---Select---" || chklstbranch.Items.Count != null)
                {


                    int itemcount1 = 0;

                    for (itemcount1 = 0; itemcount1 < chklstbranch.Items.Count; itemcount1++)
                    {
                        if (chklstbranch.Items[itemcount1].Selected == true)
                        {
                            if (strbranch == "")
                                strbranch = chklstbranch.Items[itemcount1].Value.ToString();
                            else
                                strbranch = strbranch + "," + chklstbranch.Items[itemcount1].Value.ToString();
                        }
                    }


                    if (strbranch != "")
                    {
                        strbranch = " in (" + strbranch + ")";
                    }
                    sqlcmdall = sqlcmdall + "  and r.degree_code" + strbranch + "";
                    sqlcmdbranch = "  degree_code" + strbranch + " and";
                }
                else
                {
                    // errmsg.Visible = true;
                    errmsg.Text = "Plaese Choose Degree";
                }


                if (chklstsection.Items.Count > 0)
                {
                    if (txtsection.Text != "---Select---" || chklstsection.Items.Count != null)
                    {
                        int itemcount = 0;

                        if (chklstsection.Items[chklstsection.Items.Count - 1].Selected == true)
                        {
                            sqlcmdall1 = "or sections is null or sections=''";
                        }

                        for (itemcount = 0; itemcount < chklstsection.Items.Count - 1; itemcount++)
                        {
                            if (chklstsection.Items[itemcount].Selected == true)
                            {
                                if (strsecti == "")
                                    strsecti = "'" + chklstsection.Items[itemcount].Value.ToString() + "'";
                                else
                                    strsecti = strsecti + "," + "'" + chklstsection.Items[itemcount].Value.ToString() + "'";
                            }
                        }


                        if (strsecti != "")
                        {
                            strsecti = " in(" + strsecti + ")";
                            sqlcmdall = sqlcmdall + " and (sections  " + strsecti + sqlcmdall1 + ")";
                        }

                    }

                }


                if (txtseme.Text != "---Select---" || chklstseme.Items.Count != null)
                {
                    int itemcount3 = 0;


                    for (itemcount3 = 0; itemcount3 < chklstseme.Items.Count; itemcount3++)
                    {
                        if (chklstseme.Items[itemcount3].Selected == true)
                        {

                            if (strseme == "")
                                strseme = chklstseme.Items[itemcount3].Value.ToString();
                            else
                                strseme = strseme + "," + chklstseme.Items[itemcount3].Value.ToString();

                            if (strseme == "")
                            {

                                strtempseme = chklstseme.Items[itemcount3].Value.ToString();
                            }
                            else
                            {
                                strtempseme = strtempseme + "," + chklstseme.Items[itemcount3].Value.ToString();

                                string[] semecount = strtempseme.Split(new Char[] { ',' });
                                if (semecount.GetUpperBound(0) >= 0)
                                {
                                    int semcount = semecount.GetUpperBound(0);
                                    semval = Convert.ToString(semecount[semcount]);
                                }

                            }


                        }
                    }


                    if (strseme != "")
                    {
                        strseme = " in(" + strseme + ")";
                        sqlcmdseme = " and current_semester  " + strseme + "";

                    }

                }
                else
                {
                    // errmsg.Visible = true;
                    errmsg.Text = "Plaese Choose Semester";
                }


                sqlcmdall = sqlcmdall + "and r.roll_no=e.roll_no  group by r.roll_no,r.reg_no,r.stud_name,c.course_name,dt.dept_acronym,Convert(Varchar,d.degree_code)+'-'+Convert(Varchar,r.batch_year)+'-'+Convert(Varchar,mode) +'-'+ Convert(Varchar,current_semester),edu_level,r.mode order by r.reg_no";
                dgo = d2.select_method(sqlcmdall, hat, "Text");
                int ccount = FpSpread1.Sheets[0].ColumnCount;
                //aruna allpassssss=========================================================
                all_pass_roll.Clear();
                string all_pass = "select distinct m.roll_no from mark_entry m ,registration r where r.roll_no=m.roll_no ";
                if ((strbranch.ToString() != "") && (strbatch.ToString() != ""))
                {
                    all_pass = all_pass + " and r.degree_code " + strbranch + " and batch_year " + strbatch;
                }

                if (ddlattempt.SelectedItem.ToString() == "Single")
                {
                    all_pass = all_pass + " and m.roll_no not in ( " + all_pass + " and m.attempts>1" + ")";
                }
                else if (ddlattempt.SelectedItem.ToString() == "Multiple")
                {
                    all_pass = all_pass + " and m.roll_no in ( " + all_pass + " and m.attempts<1" + ")";
                }
                allpass.Clear();
                allpass.Reset();

                allpass = d2.select_method(all_pass, hat, "Text");
                if (allpass.Tables[0].Rows.Count > 0)
                {
                    for (int alp = 0; alp < allpass.Tables[0].Rows.Count; alp++)
                    {
                        if (all_pass_roll.Contains(allpass.Tables[0].Rows[alp]["roll_no"].ToString()) != true)
                        {
                            all_pass_roll.Add(allpass.Tables[0].Rows[alp]["roll_no"].ToString(), "1");
                        }
                    }
                }

                all_pass_criteria.Clear();
                string allpasscriteria = "";
                allpasscriteria = "select distinct edu_level,isnull(cgpa,0) as cgpa,classification from coe_classification_allpass where college_code=" + Session["collegecode"].ToString() + "";
                allpass.Clear();
                allpass.Reset();

                allpass = d2.select_method(allpasscriteria, hat, "Text");
                if (allpass.Tables[0].Rows.Count > 0)
                {
                    for (int alp = 0; alp < allpass.Tables[0].Rows.Count; alp++)
                    {
                        if (all_pass_criteria.Contains(allpass.Tables[0].Rows[alp]["edu_level"].ToString()) != true)
                        {
                            string keyval = allpass.Tables[0].Rows[alp]["cgpa"].ToString() + "-" + allpass.Tables[0].Rows[alp]["classification"].ToString();
                            all_pass_criteria.Add(allpass.Tables[0].Rows[alp]["edu_level"].ToString(), keyval.ToString());
                        }
                    }
                }
                //=============================================================================

                Boolean recflag = false;
                if (dgo != null && dgo.Tables[0].Rows.Count > 0)
                {
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    FpSpread1.Visible = true;
                    errmsg.Visible = false;
                    btnxl.Visible = true;
                    btnprint.Visible = true;
                    lblmonth.Visible = true;
                    lblyear.Visible = true;
                    ddlmonth.Visible = true;
                    ddlyear.Visible = true;
                    //FpSpread1.DataSource = dgo;  //Modified by srinath 11/1/13
                    //FpSpread1.DataBind();
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[6].Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 13;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].Text = "Roll No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 2].Text = "Reg No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 3].Text = "Name of the Candidate";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Text = "Degree";
                    FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 5].Text = "Branch / Specialization (if any)";
                    FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 7].Text = "CGPA";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 8].Text = "Total CGPA";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 9].Text = "Attempt";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 10].Text = "Classification";
                    FpSpread1.Sheets[0].Columns[11].Width = 300;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 11].Text = "Ref.No.of the University approving the admission of the student";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 12].Text = "Remarks";
                    degreeve = "";

                    int sno = 0;//modified by srinath 11/1/13
                    for (int rowcnt = 0; rowcnt < dgo.Tables[0].Rows.Count; rowcnt++)
                    {

                        string regno = "";//modified by srinath 11/1/13
                        string name = "";
                        string degree1 = "";
                        string dept = "";

                        string sturollno = "";
                        string stusemval = "";
                        string studegreecode = "";
                        string stubatch = "";
                        string stulatmode = "";
                        string edulevel = "";
                        string studegrecbatchmodesem = "";

                        sturollno = dgo.Tables[0].Rows[rowcnt]["roll_no"].ToString();
                        studegrecbatchmodesem = dgo.Tables[0].Rows[rowcnt]["v"].ToString();
                        edulevel = dgo.Tables[0].Rows[rowcnt]["edu_level"].ToString();

                        regno = dgo.Tables[0].Rows[rowcnt]["reg_no"].ToString();
                        name = dgo.Tables[0].Rows[rowcnt]["stud_name"].ToString();
                        degree1 = dgo.Tables[0].Rows[rowcnt]["Degree"].ToString();
                        dept = dgo.Tables[0].Rows[rowcnt]["dept_acronym"].ToString();
                        string mode = dgo.Tables[0].Rows[rowcnt]["mode"].ToString();

                        string[] strstudegrecbatchmodesem = studegrecbatchmodesem.Split(new Char[] { '-' });
                        if (Convert.ToInt32(strstudegrecbatchmodesem[strstudegrecbatchmodesem.GetUpperBound(0)]) >= 1)
                        {
                            stusemval = strstudegrecbatchmodesem[3].ToString();
                            studegreecode = strstudegrecbatchmodesem[0].ToString();
                            stubatch = strstudegrecbatchmodesem[1].ToString();
                            stulatmode = strstudegrecbatchmodesem[2].ToString();

                            if (veflag == "T" && degreetemp != degree)
                            {
                                degree = "[" + stubatch + "-" + dgo.Tables[0].Rows[rowcnt]["Degree"].ToString() + "-" + dgo.Tables[0].Rows[rowcnt]["dept_acronym"].ToString() + "]";
                                if (degreeve == "")
                                {
                                    degreeve = degree;
                                }
                                else
                                {
                                    degreeve = degreeve + "," + degree;
                                }
                                veflag = "T";
                            }
                            degreetemp = "[" + stubatch + "-" + dgo.Tables[0].Rows[rowcnt]["Degree"].ToString() + "-" + dgo.Tables[0].Rows[rowcnt]["dept_acronym"].ToString() + "]";


                            if (rowcnt == 0)
                            {
                                yearv = d2.GetFunctionv("select Convert(Varchar, datename(month,dateadd(month, exam_month -1 , 0)))+'   '+ Convert(Varchar,exam_year) from exam_details where current_semester='" + stusemval + "' and degree_code='" + studegreecode + "' and batch_year='" + stubatch + "' ");
                            }

                            string strarrcount = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + sturollno + "') and roll_no ='" + sturollno + "' and Semester >= '1' and Semester <= '" + stusemval + "')";
                            string arrcount = "";

                            DataSet dsarrcount = new DataSet();
                            dsarrcount = d2.select_method(strarrcount, hat, "Text");
                            if (dsarrcount.Tables[0].Rows.Count == 0)
                            {
                                arrcount = Convert.ToString(dsarrcount.Tables[0].Rows.Count);
                            }
                            else
                            {
                                arrcount = "1";
                            }
                            if (Convert.ToInt32(arrcount) == 0)
                            {
                                string cgpa = d2.Calculete_CGPA(sturollno, stusemval, studegreecode, stubatch, stulatmode, collegecode);

                                string clasvel = classi(cgpa, sturollno, edulevel).ToString();//added by srinath 11/1/13
                                string[] classfi = clasvel.Split(new Char[] { '+' });

                                string classfication = "";
                                classfication = classfi[0].ToString();
                                string classfirange = "";
                                if (classfi.GetUpperBound(0) >= 1)
                                {
                                    classfirange = classfi[1].ToString();
                                }


                                string level = "";
                                if (ddlattempt.Text == "Single")
                                {
                                    level = "1";
                                }
                                if (ddlattempt.Text == "Multiple")
                                {
                                    level = "2";
                                }
                                if (ddlattempt.Text == "Both")
                                {
                                    level = classfirange;
                                }

                                int strattemptes = Convert.ToInt32(d2.GetFunction("select isnull(max(m.attempts),0) from mark_entry m,Exam_Details ed where m.exam_code=ed.exam_code and ed.batch_year='" + stubatch + "' and ed.degree_code='" + studegreecode.ToString() + "' and ed.current_semester " + strseme + " and m.roll_no='" + sturollno + "'"));
                                if (strattemptes > 1)
                                {
                                    level = "2";
                                }
                                else
                                {
                                    level = "1";
                                }
                                if (ddlattempt.Text == "Both")
                                {
                                    classfirange = level;
                                }
                                if (ddlattempt.Text == "Multiple")
                                {
                                    if (strattemptes > 1)
                                    {
                                        classfirange = level;
                                    }
                                    else
                                    {
                                        classfirange = "50";
                                    }
                                }
                                if (ddlattempt.Text == "Single")
                                {
                                    if (strattemptes > 1)
                                    {
                                        classfirange = "50";
                                    }
                                    else
                                    {
                                        classfirange = level;
                                    }
                                }

                                if (classfirange == level)
                                {
                                    if (cgpa != "" && cgpa != null)
                                    {
                                        recflag = true;
                                        sno++;//modified by srinath 11/1/13
                                        FpSpread1.Sheets[0].RowCount++;
                                        int roe = FpSpread1.Sheets[0].RowCount - 1;
                                        FpSpread1.Sheets[0].Cells[roe, 0].Text = sno.ToString();
                                        FpSpread1.Sheets[0].Cells[roe, 1].CellType = txt;
                                        FpSpread1.Sheets[0].Cells[roe, 1].Text = sturollno;
                                        FpSpread1.Sheets[0].Cells[roe, 2].CellType = txt;
                                        FpSpread1.Sheets[0].Cells[roe, 2].Text = regno;
                                        FpSpread1.Sheets[0].Cells[roe, 3].Text = name;
                                        FpSpread1.Sheets[0].Cells[roe, 4].Text = degree1;
                                        FpSpread1.Sheets[0].Cells[roe, 5].Text = dept;
                                        FpSpread1.Sheets[0].Cells[roe, 7].Text = cgpa.ToString();
                                        FpSpread1.Sheets[0].Cells[roe, 10].Text = classfication.ToString();
                                        FpSpread1.Sheets[0].Cells[roe, 8].Text = "10";
                                        if (level.ToString() == "1")
                                        {
                                            FpSpread1.Sheets[0].Cells[roe, 9].Text = "No";
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[roe, 9].Text = "Yes";
                                        }

                                        FpSpread1.Sheets[0].Cells[roe, 11].CellType = txt;
                                        if (mode == "1")
                                        {
                                            FpSpread1.Sheets[0].Cells[roe, 11].Text = regluarrefno;
                                        }
                                        else if (mode == "2")
                                        {
                                            FpSpread1.Sheets[0].Cells[roe, 11].Text = lateralrefno;
                                        }
                                        else if (mode == "3")
                                        {
                                            FpSpread1.Sheets[0].Cells[roe, 11].Text = transferrefno;
                                        }
                                    }
                                }
                            }

                        }

                    }

                    FpSpread1.Visible = true;
                    FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
                    if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                    {
                        string str = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(pincode,' ') as pincode,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website,isnull(address3,'-') as address3 from collinfo where college_code=" + Session["collegecode"] + "";

                        DataSet dscolv = new DataSet();

                        dscolv = d2.select_method(str, hat, "");


                        if (dscolv != null && dscolv.Tables[0].Rows.Count > 0)
                        {
                            collnamenew1 = dscolv.Tables[0].Rows[0]["collname"].ToString();
                            address1 = dscolv.Tables[0].Rows[0]["address1"].ToString();
                            address2 = dscolv.Tables[0].Rows[0]["address2"].ToString();
                            district = dscolv.Tables[0].Rows[0]["district"].ToString();
                            address3 = dscolv.Tables[0].Rows[0]["address3"].ToString();
                            address = address1 + "-" + address2 + "-" + district;
                            pincode = dscolv.Tables[0].Rows[0]["pincode"].ToString();
                            categery = dscolv.Tables[0].Rows[0]["category"].ToString();
                            Affliated = dscolv.Tables[0].Rows[0]["affliated"].ToString();
                            Phoneno = dscolv.Tables[0].Rows[0]["phoneno"].ToString();
                            Faxno = dscolv.Tables[0].Rows[0]["faxno"].ToString();
                            phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
                            email = "E-Mail:" + dscolv.Tables[0].Rows[0]["email"].ToString() + " " + "Web Site:" + dscolv.Tables[0].Rows[0]["website"].ToString();
                        }

                    }


                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = collnamenew1 + ", " + address3 + "- " + pincode;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.White;

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = "( An " + categery + " Institution - Affiliated to " + Affliated + ".)";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorBottom = Color.White;
                    //'----------------------------------------------------new----------------------------

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, 0].Text = "Details of Candidates eligible for the award of Degree  - " + yearv + "  Examinations."; //address;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorBottom = Color.White;
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[3, 0].Text = "Degree & Branch : " + degreeve + "";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[3, 0].Border.BorderColorBottom = Color.White;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                }

                if (recflag == true)
                {
                    FpSpread1.Visible = true;
                    btnxl.Visible = true;
                }
                else
                {
                    FpSpread1.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "No Records Found";
                    btnxl.Visible = false;
                }
            }
        }
        catch (Exception ev)
        {
            string evetri = ev.ToString();
        }
    }

    public string classi(string strcgpa, string studrollno, string edulevel)
    {
        string returnc = "", classi = "";
        double cgpa = 0;

        //all pass Aruna======================================================

        if (all_pass_roll.Contains(studrollno.ToString()))
        {
            if (all_pass_criteria.Contains(edulevel.ToString()))
            {
                string keyval = Convert.ToString(GetCorrespondingKey(edulevel.ToString(), all_pass_criteria));
                string[] criteria = keyval.Split('-');
                double cgpa_crit = Convert.ToDouble(criteria[0]);
                string classification = Convert.ToString(criteria[1]);
                if (Convert.ToDouble(strcgpa) >= Convert.ToDouble(cgpa_crit))
                {
                    string i = "+1";//added by srinath 11/1/13
                    returnc = classification.ToString() + i;
                }
                else
                {
                    string strretriveclassi = "select frompoint,topoint,classification,collegecode from coe_classification where collegecode='" + collegecode + "' and edu_level='" + edulevel + "'";
                    DataSet dsclassi = new DataSet();
                    double from = 0, to = 0;
                    dsclassi = d2.select_method(strretriveclassi, hat, "");
                    cgpa = Math.Round((Convert.ToDouble(strcgpa)), 2);

                    if (dsclassi != null && dsclassi.Tables[0].Rows.Count > 0)
                    {
                        for (int aaa = 0; aaa < dsclassi.Tables[0].Rows.Count; aaa++)
                        {
                            from = Math.Round((Convert.ToDouble(dsclassi.Tables[0].Rows[aaa]["frompoint"].ToString())), 2);
                            to = Math.Round((Convert.ToDouble(dsclassi.Tables[0].Rows[aaa]["topoint"].ToString())), 2);
                            classi = Convert.ToString(dsclassi.Tables[0].Rows[aaa]["classification"].ToString());

                            if (from <= cgpa && to >= cgpa)
                            {
                                string i = "+1";//added by srinath 11/1/13
                                returnc = classi + i;

                            }

                        }
                    }
                }
            }
        }
        else
        {
            //===========================================================

            string strretriveclassi = "select frompoint,topoint,classification,collegecode from coe_classification where collegecode='" + collegecode + "' and edu_level='" + edulevel + "'";
            DataSet dsclassi = new DataSet();
            double from = 0, to = 0;
            dsclassi = d2.select_method(strretriveclassi, hat, "");
            cgpa = Math.Round((Convert.ToDouble(strcgpa)), 2);

            if (dsclassi != null && dsclassi.Tables[0].Rows.Count > 0)
            {
                for (int aaa = 0; aaa < dsclassi.Tables[0].Rows.Count; aaa++)
                {
                    from = Math.Round((Convert.ToDouble(dsclassi.Tables[0].Rows[aaa]["frompoint"].ToString())), 2);
                    to = Math.Round((Convert.ToDouble(dsclassi.Tables[0].Rows[aaa]["topoint"].ToString())), 2);
                    classi = Convert.ToString(dsclassi.Tables[0].Rows[aaa]["classification"].ToString());

                    if (from <= cgpa && to >= cgpa)
                    {
                        string i = "+2";//added by srinath 11/1/13
                        returnc = classi + i;

                    }

                }
            }
        }

        return returnc;
    }


    #region "vetri previouse methods"

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
                for (int i = 0; i < chklstbatch.Items.Count; i++)
                {
                    chklstbatch.Items[i].Selected = true;
                    if (chklstbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                    }

                }

            }
        }
        catch (Exception ex)
        {
            errmsg.Text = "Please Choose Batch";
            //errmsg.Visible = true;
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
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count1 += 1;
                    }
                    if (chklstdegree.Items.Count == count1)
                    {
                        chkdegree.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = "Please Choose Degree";
            //errmsg.Visible = true;
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
            //chklstbranch.Items.Clear();
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
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count2 += 1;
                    }
                    if (chklstbranch.Items.Count == count2)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = "Please Choose Degree";
            //errmsg.Visible = true;
        }
    }

    //----- load function for multiseme----------

    public void Bindmultiseme(string collegecode)
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;

        try
        {
            chklstseme.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindmultSem(collegecode);
            if (ds2.Tables[0].Rows.Count > 0)
            {

                int rowcount = Convert.ToInt32(ds2.Tables[0].Rows.Count);
                first_year = Convert.ToBoolean(Convert.ToString(ds2.Tables[0].Rows[rowcount - 1][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds2.Tables[0].Rows[rowcount - 1][0]).ToString());

                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        chklstseme.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        chklstseme.Items.Add(i.ToString());
                    }
                }


                for (int v = 0; v < chklstseme.Items.Count; v++)
                {
                    chklstseme.Items[v].Selected = true;
                    if (chklstseme.Items[v].Selected == true)
                    {
                        count4 += 1;
                    }
                    if (chklstseme.Items.Count == count4)
                    {

                        chkseme.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = "Please Choose Semester";
            //errmsg.Visible = true;
        }
    }

    //------Load Function for the Section Details-----

    public void BindSectionDetailmult(string collegecode)
    {
        try
        {
            int takecount = 0;
            //strbranch = chklstbranch.SelectedValue.ToString();

            chklstsection.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetailmult(collegecode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                takecount = ds2.Tables[0].Rows.Count;
                chklstsection.DataSource = ds2;
                chklstsection.DataTextField = "sections";
                chklstsection.DataBind();
                chklstsection.Items.Insert(takecount, "Empty");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    chklstsection.Enabled = false;
                }
                else
                {
                    chklstsection.Enabled = true;
                    chklstsection.SelectedIndex = chklstsection.Items.Count - 2;
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
                        }
                    }
                }
            }
            else
            {
                chklstsection.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = "Please Choose Section";
            //  errmsg.Visible = true;
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
        //BindSem(strbranch, strbatchyear, collegecode);
        BindSectionDetailmult(collegecode);


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



    //----------Semester Dropdown Extender-----------------

    protected void chkseme_CheckedChanged(object sender, EventArgs e)
    {
        if (chkseme.Checked == true)
        {
            for (int i = 0; i < chklstseme.Items.Count; i++)
            {
                chklstseme.Items[i].Selected = true;
                txtseme.Text = "Semester(" + (chklstseme.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstseme.Items.Count; i++)
            {
                chklstseme.Items[i].Selected = false;
                txtseme.Text = "---Select---";
            }
        }
    }

    protected void chklstseme_SelectedIndexChanged(object sender, EventArgs e)
    {
        pseme.Focus();

        int sectioncount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstseme.Items.Count; i++)
        {
            if (chklstseme.Items[i].Selected == true)
            {

                value = chklstseme.Items[i].Text;
                code = chklstseme.Items[i].Value.ToString();
                sectioncount = sectioncount + 1;
                txtseme.Text = "Semester(" + sectioncount.ToString() + ")";
            }

        }

        if (sectioncount == 0)
            txtseme.Text = "---Select---";
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


    }

    protected void LinkButtonseme_Click(object sender, EventArgs e)
    {

        chklstseme.ClearSelection();
        sectioncnt = 0;
        txtseme.Text = "---Select---";
    }

    public void semeimg_Click(object sender, ImageClickEventArgs e)
    {
        sectioncnt = sectioncnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstseme.Items[r].Selected = false;

        txtseme.Text = "Semester(" + sectioncnt.ToString() + ")";
        if (txtseme.Text == "Semester(0)")
        {
            txtseme.Text = "---Select---";

        }

    }

    public Label semelabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrolseme"] = true;
        return (lbc);
    }

    public ImageButton semeimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["lseatcontrolseme"] = true;
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


    protected void btnxl_Click(object sender, EventArgs e)
    {
        string appPath = HttpContext.Current.Server.MapPath("~");
        string print = "";
        if (appPath != "")
        {
            int i = 1;
            appPath = appPath.Replace("\\", "/");
        e:
            try
            {
                print = "Consolidated Student GPA And CGPA" + i;
                //FpSpread1.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly);
                //Aruna on 26feb2013============================
                string szPath = appPath + "/Report/";
                string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                FpSpread1.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Flush();
                Response.WriteFile(szPath + szFile);
                //=============================================
            }
            catch
            {
                i++;
                goto e;

            }
        }
        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);

    }


    #endregion
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }

        return null;
    }
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();

            Font Fontboldbig = new Font("Book Antiqua", 15, FontStyle.Bold);
            Font Fontbold = new Font("Book Antiqua", 13, FontStyle.Bold);
            Font Fontbold2 = new Font("Book Antiqua", 11, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 11, FontStyle.Regular);
            Font Fontsmal2 = new Font("Book Antiqua", 9, FontStyle.Regular);

            string collname = "";
            string address = "";
            string university = "";
            string category = "";

            string exammonth = ddlmonth.SelectedItem.ToString();
            if (exammonth.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Exam Month";
                return;
            }
            string examyear = ddlyear.SelectedItem.ToString();
            if (examyear.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Exam Year";
                return;
            }

            string strcoll = "select * from collinfo where  college_code='" + Session["collegecode"].ToString() + "'";
            DataSet dshall = d2.select_method_wo_parameter(strcoll, "Text");
            if (dshall.Tables[0].Rows.Count > 0)
            {
                collname = dshall.Tables[0].Rows[0]["collname"].ToString();
                string add1 = dshall.Tables[0].Rows[0]["address1"].ToString();
                string add2 = dshall.Tables[0].Rows[0]["address2"].ToString();
                string add3 = dshall.Tables[0].Rows[0]["address3"].ToString();
                string pincode = dshall.Tables[0].Rows[0]["pincode"].ToString();
                university = dshall.Tables[0].Rows[0]["university"].ToString();
                category = dshall.Tables[0].Rows[0]["category"].ToString();
                if (add1.Trim() != "")
                {
                    address = add1;
                }
                if (add2.Trim() != "")
                {
                    if (address == "")
                    {
                        address = add2;
                    }
                    else
                    {
                        address = address + ", " + add2;
                    }
                }
                if (add3.Trim() != "")
                {
                    if (address == "")
                    {
                        address = add3;
                    }
                    else
                    {
                        address = address + ", " + add3;
                    }
                }
                if (pincode.Trim() != "")
                {
                    if (address == "")
                    {
                        address = pincode;
                    }
                    else
                    {
                        address = address + " - " + pincode;
                    }
                }
            }
            int coltop = 5;
            PdfTextArea ptchead = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Details of the Candidates eligible for the award of Degree");
            coltop = coltop + 20;
            PdfTextArea ptccol = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 80, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, collname + ", " + address);

            PdfTextArea ptcmonth = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 580, coltop, 150, 50), System.Drawing.ContentAlignment.MiddleLeft, "Month and Year of ");

            PdfTextArea ptcyearmonth = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 675, coltop, 150, 50), System.Drawing.ContentAlignment.MiddleLeft, exammonth + " " + examyear);

            coltop = coltop + 15;
            PdfTextArea ptccatuni = new PdfTextArea(Fontsmal2, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 80, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "(An " + category + " Institution - Affiliated to" + university + ")");

            PdfTextArea ptcexamination = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, 580, coltop, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, "Passing Examination");

            PdfTextArea ptcfooter1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, 100, 550, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "CONTROLLER OF EXAMINATIONS");

            PdfTextArea ptcfooter1a = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, 125, 563, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "(COLLEGE/INSTITUTION)");

            PdfTextArea ptcfooter2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, 460, 550, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "PRINCIPAL/DEAN OF THE COLLEGE/INSTITUTION");

            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            {
                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                mypdfpage.Add(LogoImage, 10, 5, 500);
            }

            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
            {
                PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                mypdfpage.Add(leftimage, 780, 5, 500);
            }

            FpSpread1.SaveChanges();

            string batchyear = "";

            for (int itemcount = 0; itemcount < chklstbatch.Items.Count; itemcount++)
            {
                if (chklstbatch.Items[itemcount].Selected == true)
                {
                    if (strbatch == "")
                        strbatch = chklstbatch.Items[itemcount].Value.ToString();
                    else
                        strbatch = strbatch + "," + chklstbatch.Items[itemcount].Value.ToString();
                }
            }
            if (strbatch != "")
            {
                strbatch = " and r.batch_year in (" + strbatch + ")";
            }

            for (int itemcount1 = 0; itemcount1 < chklstbranch.Items.Count; itemcount1++)
            {
                if (chklstbranch.Items[itemcount1].Selected == true)
                {
                    if (strbranch == "")
                        strbranch = chklstbranch.Items[itemcount1].Value.ToString();
                    else
                        strbranch = strbranch + "," + chklstbranch.Items[itemcount1].Value.ToString();
                }
            }
            if (strbranch != "")
            {
                strbranch = " and r.degree_code in (" + strbranch + ")";
            }

            string strquery = "select r.Roll_No,r.Reg_No,s.Photo from Registration r,stdphoto s where r.App_No=s.app_no and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + strbatch + " " + strbranch + "";
            DataSet dsphoto = d2.select_method_wo_parameter(strquery, "Text");

            int noorstu = FpSpread1.Sheets[0].RowCount;

            coltop = coltop + 40;
            int nooftablerow = 6;
            if (noorstu < 5)
            {
                nooftablerow = noorstu + 1;
            }
            Gios.Pdf.PdfTable table = mydocument.NewTable(Fontbold, nooftablerow, 11, 4);
            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
            table.VisibleHeaders = false;
            table.Columns[0].SetWidth(30);
            table.Columns[1].SetWidth(130);
            table.Columns[2].SetWidth(250);
            table.Columns[3].SetWidth(80);
            table.Columns[4].SetWidth(100);
            table.Columns[5].SetWidth(60);
            table.Columns[6].SetWidth(60);
            table.Columns[7].SetWidth(80);
            table.Columns[8].SetWidth(90);
            table.Columns[9].SetWidth(100);
            table.Columns[10].SetWidth(70);


            table.Cell(0, 0).SetFont(Fontbold2);
            table.Cell(0, 1).SetFont(Fontbold2);
            table.Cell(0, 2).SetFont(Fontbold2);
            table.Cell(0, 3).SetFont(Fontbold2);
            table.Cell(0, 4).SetFont(Fontbold2);
            table.Cell(0, 5).SetFont(Fontbold2);
            table.Cell(0, 6).SetFont(Fontbold2);
            table.Cell(0, 7).SetFont(Fontbold2);
            table.Cell(0, 8).SetFont(Fontbold2);
            table.Cell(0, 9).SetFont(Fontbold2);
            table.Cell(0, 10).SetFont(Fontbold2);

            table.Cell(0, 0).SetContent("S.No");
            table.Cell(0, 1).SetContent("Reg.No");
            table.Cell(0, 2).SetContent("Name of the Candidate");
            table.Cell(0, 3).SetContent("Degree");
            table.Cell(0, 4).SetContent("Branch / Specialization (if any)");
            table.Cell(0, 5).SetContent("Total Secured / CGPA");
            table.Cell(0, 6).SetContent("Total Max. Secured / CGPA");
            table.Cell(0, 7).SetContent("Attempts (Say Yes/No)");
            table.Cell(0, 8).SetContent("Classification");
            table.Cell(0, 9).SetContent("Ref.No.of the University approving the admission of the student");
            table.Cell(0, 10).SetContent("Photo");

            int row = 0;
            int photop = coltop + 30;
            for (int r = 0; r < FpSpread1.Sheets[0].RowCount; r++)
            {
                row++;
                if ((row % 6) == 0)
                {
                    photop = coltop + 30;
                    mypdfpage.Add(ptchead);
                    mypdfpage.Add(ptccol);
                    mypdfpage.Add(ptcmonth);
                    mypdfpage.Add(ptccatuni);
                    mypdfpage.Add(ptcyearmonth);
                    mypdfpage.Add(ptcexamination);
                    mypdfpage.Add(ptcfooter1);
                    mypdfpage.Add(ptcfooter1a);
                    mypdfpage.Add(ptcfooter2);


                    Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 825, 1000));
                    mypdfpage.Add(newpdftabpage);

                    Double getheigh = newpdftabpage.Area.Height;
                    getheigh = Math.Round(getheigh, 0);


                    PdfTextArea ptctablefoot = new PdfTextArea(Fontsmal2, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 40, getheigh + 65, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "We certify that the particulars furnished above are true extracts of the academic records of the candidates.");

                    mypdfpage.Add(ptctablefoot);
                    mypdfpage.SaveToDocument();

                    mypdfpage = mydocument.NewPage();

                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 10, 5, 500);
                    }

                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    {
                        PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypdfpage.Add(leftimage, 780, 5, 500);
                    }

                    int getremro = noorstu - r;
                    if (getremro < 5)
                    {
                        getremro = getremro + 1;
                    }
                    else
                    {
                        getremro = 6;
                    }
                    table = mydocument.NewTable(Fontbold, getremro, 11, 4);
                    table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    table.VisibleHeaders = false;

                    table.Columns[0].SetWidth(30);
                    table.Columns[1].SetWidth(130);
                    table.Columns[2].SetWidth(250);
                    table.Columns[3].SetWidth(80);
                    table.Columns[4].SetWidth(100);
                    table.Columns[5].SetWidth(60);
                    table.Columns[6].SetWidth(60);
                    table.Columns[7].SetWidth(80);
                    table.Columns[8].SetWidth(90);
                    table.Columns[9].SetWidth(100);
                    table.Columns[10].SetWidth(70);


                    table.Cell(0, 0).SetFont(Fontbold2);
                    table.Cell(0, 1).SetFont(Fontbold2);
                    table.Cell(0, 2).SetFont(Fontbold2);
                    table.Cell(0, 3).SetFont(Fontbold2);
                    table.Cell(0, 4).SetFont(Fontbold2);
                    table.Cell(0, 5).SetFont(Fontbold2);
                    table.Cell(0, 6).SetFont(Fontbold2);
                    table.Cell(0, 7).SetFont(Fontbold2);
                    table.Cell(0, 8).SetFont(Fontbold2);
                    table.Cell(0, 9).SetFont(Fontbold2);
                    table.Cell(0, 10).SetFont(Fontbold2);

                    table.Cell(0, 0).SetContent("S.No");
                    table.Cell(0, 1).SetContent("Reg.No");
                    table.Cell(0, 2).SetContent("Name of the Candidate");
                    table.Cell(0, 3).SetContent("Degree");
                    table.Cell(0, 4).SetContent("Branch / Specialization (if any)");
                    table.Cell(0, 5).SetContent("Total Secured / CGPA");
                    table.Cell(0, 6).SetContent("Total Max. Secured / CGPA");
                    table.Cell(0, 7).SetContent("Attempts (Say Yes/No)");
                    table.Cell(0, 8).SetContent("Classification");
                    table.Cell(0, 9).SetContent("Ref.No.of the University approving the admission of the student");
                    table.Cell(0, 10).SetContent("Photo");
                    row = 1;
                }

                photop = photop + 62;

                string srno = FpSpread1.Sheets[0].Cells[r, 0].Text;
                string roll = FpSpread1.Sheets[0].Cells[r, 1].Text;
                string regno = FpSpread1.Sheets[0].Cells[r, 2].Text;
                string sname = FpSpread1.Sheets[0].Cells[r, 3].Text;
                string degree = FpSpread1.Sheets[0].Cells[r, 4].Text;
                string branch = FpSpread1.Sheets[0].Cells[r, 5].Text;
                string cgpa = FpSpread1.Sheets[0].Cells[r, 7].Text;
                string toatlcgpa = FpSpread1.Sheets[0].Cells[r, 8].Text;
                string attenmpts = FpSpread1.Sheets[0].Cells[r, 9].Text;
                string classifi = FpSpread1.Sheets[0].Cells[r, 10].Text;
                string regnouni = FpSpread1.Sheets[0].Cells[r, 11].Text;

                if (sname.Length > 25)
                {
                    photop = photop + 5;
                }

                table.Cell(row, 0).SetContent(srno);
                table.Cell(row, 1).SetContent(regno);
                table.Cell(row, 2).SetContent(sname);
                table.Cell(row, 3).SetContent(degree);
                table.Cell(row, 4).SetContent(branch);
                table.Cell(row, 5).SetContent(cgpa);
                table.Cell(row, 6).SetContent(toatlcgpa);
                table.Cell(row, 7).SetContent(attenmpts);
                table.Cell(row, 8).SetContent(classifi);
                table.Cell(row, 9).SetContent(regnouni);

                dsphoto.Tables[0].DefaultView.RowFilter = "Roll_No='" + roll + "'";
                DataView dvphoto = dsphoto.Tables[0].DefaultView;
                if (dvphoto.Count > 0)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                    {
                        if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                        {
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                            {
                                byte[] file = (byte[])dvphoto[0]["photo"];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                                memoryStream.Dispose();
                                memoryStream.Close();
                            }
                        }
                    }
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                    {
                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"));
                        mypdfpage.Add(LogoImage, 800, photop, 950);
                    }
                }

                table.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(row, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                table.Cell(row, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                table.Cell(row, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                table.Cell(row, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                table.Cell(row, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(row, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(row, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(row, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(row, 9).SetContentAlignment(ContentAlignment.MiddleCenter);


                table.Cell(row, 0).SetFont(Fontsmall);
                table.Cell(row, 1).SetFont(Fontsmall);
                table.Cell(row, 2).SetFont(Fontsmall);
                table.Cell(row, 3).SetFont(Fontsmall);
                table.Cell(row, 4).SetFont(Fontsmall);
                table.Cell(row, 5).SetFont(Fontsmall);
                table.Cell(row, 6).SetFont(Fontsmall);
                table.Cell(row, 7).SetFont(Fontsmall);
                table.Cell(row, 8).SetFont(Fontsmall);
                table.Cell(row, 9).SetFont(Fontsmall);
                Double celpad = 25;
                string[] spre = regnouni.Split(' ');
                if (spre.GetUpperBound(0) == 1)
                {
                    celpad = 20;
                }
                else if (spre.GetUpperBound(0) == 2)
                {
                    celpad = 15;
                }
                else if (spre.GetUpperBound(0) == 3)
                {
                    celpad = 10;
                }
                else if (spre.GetUpperBound(0) == 4)
                {
                    celpad = 5;
                }
                else
                {
                    celpad = 0.1;
                }

                table.Cell(row, 0).SetCellPadding(celpad);
                table.Cell(row, 1).SetCellPadding(celpad);
                table.Cell(row, 2).SetCellPadding(celpad);
                table.Cell(row, 3).SetCellPadding(celpad);
                table.Cell(row, 4).SetCellPadding(celpad);
                table.Cell(row, 5).SetCellPadding(celpad);
                table.Cell(row, 6).SetCellPadding(celpad);
                table.Cell(row, 7).SetCellPadding(celpad);
                table.Cell(row, 8).SetCellPadding(celpad);
                table.Cell(row, 9).SetCellPadding(celpad);
                table.Cell(row, 10).SetCellPadding(celpad);
            }

            mypdfpage.Add(ptchead);
            mypdfpage.Add(ptccol);
            mypdfpage.Add(ptcmonth);
            mypdfpage.Add(ptccatuni);
            mypdfpage.Add(ptcexamination);
            mypdfpage.Add(ptcyearmonth);
            mypdfpage.Add(ptcfooter1);
            mypdfpage.Add(ptcfooter1a);
            mypdfpage.Add(ptcfooter2);

            Gios.Pdf.PdfTablePage newpdftabpage2 = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 825, 1000));
            mypdfpage.Add(newpdftabpage2);

            Double getheigh1 = newpdftabpage2.Area.Height;
            getheigh1 = Math.Round(getheigh1, 0);


            PdfTextArea ptctablefoot1a = new PdfTextArea(Fontsmal2, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 40, getheigh1 + 65, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "We certify that the particulars furnished above are true extracts of the academic records of the candidates.");

            mypdfpage.Add(ptctablefoot1a);

            mypdfpage.SaveToDocument();
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Degree Award.pdf";
                mydocument.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnrefsettings_Click(object sender, EventArgs e)
    {
        try
        {
            PRefnosettings.Visible = true;
            txtregular.Text = "";
            txtlateral.Text = "";
            txttransfer.Text = "";

            string strqueryref = "select * from Master_Settings where settings='Regular Referance Number'";
            strqueryref = strqueryref + " select * from Master_Settings where settings='Lateral Referance Number'";
            strqueryref = strqueryref + " select * from Master_Settings where settings='Transfer Referance Number'";
            DataSet dsref = d2.select_method_wo_parameter(strqueryref, "Text");
            if (dsref.Tables[0].Rows.Count > 0)
            {
                txtregular.Text = dsref.Tables[0].Rows[0]["value"].ToString().Trim();
            }
            if (dsref.Tables[1].Rows.Count > 0)
            {
                txtlateral.Text = dsref.Tables[1].Rows[0]["value"].ToString().Trim();
            }
            if (dsref.Tables[2].Rows.Count > 0)
            {
                txttransfer.Text = dsref.Tables[2].Rows[0]["value"].ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            perrmsg.Visible = true;
            perrmsg.Text = ex.ToString();
        }
    }
    protected void btnrefsave_Click(object sender, EventArgs e)
    {
        try
        {
            string strrugularefno = txtregular.Text.ToString().Trim();
            if (strrugularefno.Trim() == "")
            {
                perrmsg.Visible = true;
                perrmsg.Text = "Please Enter The Regular Student Reference Number";
                return;
            }

            string laterrefno = txtlateral.Text.ToString().Trim();
            if (laterrefno.Trim() == "")
            {
                perrmsg.Visible = true;
                perrmsg.Text = "Please Enter The Lateral Entry Studnet Reference Number";
                return;
            }

            string transferrefno = txttransfer.Text.ToString().Trim();
            if (transferrefno.Trim() == "")
            {
                perrmsg.Visible = true;
                perrmsg.Text = "Please Enter The Transfer Entry Studnet Reference Number";
                return;
            }

            string insupdquery = "if not exists(select * from Master_Settings where settings='Regular Referance Number')";
            insupdquery = insupdquery + " insert into Master_Settings (settings,value)values('Regular Referance Number','" + strrugularefno + "')";
            insupdquery = insupdquery + " else update Master_Settings set value='" + strrugularefno + "' where settings='Regular Referance Number'";

            int insupdval = d2.update_method_wo_parameter(insupdquery, "Text");


            insupdquery = "if not exists(select * from Master_Settings where settings='Lateral Referance Number')";
            insupdquery = insupdquery + " insert into Master_Settings (settings,value)values('Lateral Referance Number','" + laterrefno + "')";
            insupdquery = insupdquery + " else update Master_Settings set value='" + laterrefno + "' where settings='Lateral Referance Number'";

            insupdval = d2.update_method_wo_parameter(insupdquery, "Text");

            insupdquery = "if not exists(select * from Master_Settings where settings='Transfer Referance Number')";
            insupdquery = insupdquery + " insert into Master_Settings (settings,value)values('Transfer Referance Number','" + transferrefno + "')";
            insupdquery = insupdquery + " else update Master_Settings set value='" + transferrefno + "' where settings='Transfer Referance Number'";

            insupdval = d2.update_method_wo_parameter(insupdquery, "Text");

            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
        }
        catch (Exception ex)
        {
            perrmsg.Visible = true;
            perrmsg.Text = ex.ToString();
        }
    }
    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            PRefnosettings.Visible = false;
        }
        catch (Exception ex)
        {
            perrmsg.Visible = true;
            perrmsg.Text = ex.ToString();
        }
    }


    protected void CheckBox1_click(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox1.Checked == true)
            {
                CheckBox2.Checked = false;
                CheckBox1.Checked = true;
            }

        }
        catch
        {
        }
    }
    protected void CheckBox2_click(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox2.Checked == true)
            {
                CheckBox2.Checked = true;
                CheckBox1.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void format1()
    {
        try
        {
            string semval = "";
            string strtempseme = "";


            string sqlcmdall = "select distinct ROW_NUMBER() OVER (ORDER BY  r.Roll_no) As SrNo,r.roll_no,r.reg_no,r.app_no,r.stud_name,c.course_name as Degree,dt.dept_acronym,dt.Dept_Name,edu_level,r.mode from registration r,degree d,course c,department dt,mark_entry e where r.degree_code=d.degree_code and d.dept_code=dt.dept_code and c.course_id=d.course_id";

            if (txtbatch.Text != "---Select---" || chklstbatch.Items.Count != null)
            {
                int itemcount = 0;


                for (itemcount = 0; itemcount < chklstbatch.Items.Count; itemcount++)
                {
                    if (chklstbatch.Items[itemcount].Selected == true)
                    {
                        if (strbatch == "")
                            strbatch = chklstbatch.Items[itemcount].Value.ToString();
                        else
                            strbatch = strbatch + "," + chklstbatch.Items[itemcount].Value.ToString();
                    }
                }


                if (strbatch != "")
                {
                    strbatch = " in(" + strbatch + ")";
                }
                sqlcmdall = sqlcmdall + " and r.batch_year   " + strbatch + "";
                sqlcmdbatch = "  batch_year   " + strbatch + "";

            }
            else
            {
               
                errmsg.Text = "Plaese Choose Batch";
            }
            if (txtbranch.Text != "---Select---" || chklstbranch.Items.Count != null)
            {


                int itemcount1 = 0;

                for (itemcount1 = 0; itemcount1 < chklstbranch.Items.Count; itemcount1++)
                {
                    if (chklstbranch.Items[itemcount1].Selected == true)
                    {
                        if (strbranch == "")
                            strbranch = chklstbranch.Items[itemcount1].Value.ToString();
                        else
                            strbranch = strbranch + "," + chklstbranch.Items[itemcount1].Value.ToString();
                    }
                }


                if (strbranch != "")
                {
                    strbranch = " in (" + strbranch + ")";
                }
                sqlcmdall = sqlcmdall + "  and r.degree_code" + strbranch + "";
                sqlcmdbranch = "  degree_code" + strbranch + " and";
            }
            else
            {
              
                errmsg.Text = "Plaese Choose Degree";
            }
            if (chklstsection.Items.Count > 0)
            {
                if (txtsection.Text != "---Select---" || chklstsection.Items.Count != null)
                {
                    int itemcount = 0;

                    if (chklstsection.Items[chklstsection.Items.Count - 1].Selected == true)
                    {
                        sqlcmdall1 = "or sections is null or sections=''";
                    }

                    for (itemcount = 0; itemcount < chklstsection.Items.Count - 1; itemcount++)
                    {
                        if (chklstsection.Items[itemcount].Selected == true)
                        {
                            if (strsecti == "")
                                strsecti = "'" + chklstsection.Items[itemcount].Value.ToString() + "'";
                            else
                                strsecti = strsecti + "," + "'" + chklstsection.Items[itemcount].Value.ToString() + "'";
                        }
                    }


                    if (strsecti != "")
                    {
                        strsecti = " in(" + strsecti + ")";
                        sqlcmdall = sqlcmdall + " and (sections  " + strsecti + sqlcmdall1 + ")";
                    }

                }

            }

            if (txtseme.Text != "---Select---" || chklstseme.Items.Count != null)
            {
                int itemcount3 = 0;


                for (itemcount3 = 0; itemcount3 < chklstseme.Items.Count; itemcount3++)
                {
                    if (chklstseme.Items[itemcount3].Selected == true)
                    {

                        if (strseme == "")
                            strseme = chklstseme.Items[itemcount3].Value.ToString();
                        else
                            strseme = strseme + "," + chklstseme.Items[itemcount3].Value.ToString();

                        if (strseme == "")
                        {

                            strtempseme = chklstseme.Items[itemcount3].Value.ToString();
                        }
                        else
                        {
                            strtempseme = strtempseme + "," + chklstseme.Items[itemcount3].Value.ToString();

                            string[] semecount = strtempseme.Split(new Char[] { ',' });
                            if (semecount.GetUpperBound(0) >= 0)
                            {
                                int semcount = semecount.GetUpperBound(0);
                                semval = Convert.ToString(semecount[semcount]);
                            }

                        }


                    }
                }


                if (strseme != "")
                {
                    strseme = " in(" + strseme + ")";
                    sqlcmdseme = " and current_semester  " + strseme + "";

                }

            }
            else
            {
               
                errmsg.Text = "Plaese Choose Semester";
            }
            sqlcmdall = sqlcmdall + "and r.roll_no=e.roll_no  group by r.roll_no,r.reg_no,r.stud_name,r.app_no,c.course_name,dt.dept_acronym,dt.Dept_Name,edu_level,r.mode order by r.reg_no";
            dgo = d2.select_method(sqlcmdall, hat, "Text");
            int ccount = FpSpread1.Sheets[0].ColumnCount;
            all_pass_roll.Clear();
            string all_pass = "select distinct m.roll_no from mark_entry m ,registration r where r.roll_no=m.roll_no ";
            if ((strbranch.ToString() != "") && (strbatch.ToString() != ""))
            {
                all_pass = all_pass + " and r.degree_code " + strbranch + " and batch_year " + strbatch;
            }

            if (ddlattempt.SelectedItem.ToString() == "Single")
            {
                all_pass = all_pass + " and m.roll_no not in ( " + all_pass + " and m.attempts>1" + ")";
            }
            else if (ddlattempt.SelectedItem.ToString() == "Multiple")
            {
                all_pass = all_pass + " and m.roll_no in ( " + all_pass + " and m.attempts<1" + ")";
            }
            allpass.Clear();
            allpass.Reset();

            allpass = d2.select_method(all_pass, hat, "Text");
            if (allpass.Tables[0].Rows.Count > 0)
            {
                for (int alp = 0; alp < allpass.Tables[0].Rows.Count; alp++)
                {
                    if (all_pass_roll.Contains(allpass.Tables[0].Rows[alp]["roll_no"].ToString()) != true)
                    {
                        all_pass_roll.Add(allpass.Tables[0].Rows[alp]["roll_no"].ToString(), "1");
                    }
                }
            }
            Boolean recflag = false;
            string parttyp = "select distinct Part_Type from subject where Part_Type<>'0'  order by Part_Type asc";
            DataSet dspart = d2.select_method_wo_parameter(parttyp, "text");

            string clname = string.Empty;
            string clcode = string.Empty;
            string collg = "select collname,college_code from collinfo where college_code=" + Convert.ToString(Session["collegecode"]) + "";
            DataSet clgname = d2.select_method_wo_parameter(collg, "text");
            if (clgname.Tables[0].Rows.Count > 0 && clgname.Tables.Count > 0)
            {
                clname = Convert.ToString(clgname.Tables[0].Rows[0]["collname"]);
                clcode = Convert.ToString(clgname.Tables[0].Rows[0]["college_code"]);
            }
            if (CheckBox1.Checked == true)
            {
                if (dgo.Tables.Count > 0 && dgo.Tables[0].Rows.Count > 0)
                {

                    FpSpread2.CommandBar.Visible = false;
                    FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread2.Sheets[0].RowHeader.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.Black;
                    FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    darkstyle.Font.Name = "Book Antiqua";
                    darkstyle.Font.Size = FontUnit.Medium;
                    darkstyle.HorizontalAlign = HorizontalAlign.Center;
                    darkstyle.VerticalAlign = VerticalAlign.Middle;
                    FpSpread2.Visible = true;
                    FpSpread2.Sheets[0].RowCount = 0;
                    FpSpread2.Sheets[0].ColumnCount = 0;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.NO";
                    FpSpread2.Sheets[0].Columns[0].Width = 70;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "COLLEGE NAME";
                    FpSpread2.Sheets[0].Columns[1].Width = 220;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "COLLEGE CODE";
                    FpSpread2.Sheets[0].Columns[2].Width = 80;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "REGISTER NO.";
                    FpSpread2.Sheets[0].Columns[3].Width = 130;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "STUDENT NAME";
                    FpSpread2.Sheets[0].Columns[4].Width = 200;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "SEX";
                    FpSpread2.Sheets[0].Columns[5].Width = 90;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "DEGREE";
                    FpSpread2.Sheets[0].Columns[6].Width = 100;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "BRANCH";
                    FpSpread2.Sheets[0].Columns[7].Width = 200;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Text = "PART SUBJECT1";
                    FpSpread2.Sheets[0].Columns[8].Width = 190;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;

                    int partcount = dspart.Tables[0].Rows.Count;
                    int subpartcount = 0;
                    int c = 8;
                    if (dspart.Tables.Count > 0 && dspart.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < dspart.Tables[0].Rows.Count; i++)
                        {
                            FpSpread2.Sheets[0].ColumnCount++;
                            string partno = Convert.ToString(dspart.Tables[0].Rows[i]["part_type"]);
                            if (partno == "")
                            {
                                partno = "0";
                            }
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "PART " + partno + "";
                            FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }

                    //string regno1 = Convert.ToString(dgo.Tables[0].Rows[0]["reg_no"]);
                    //string rolno1 = "select roll_no from registration where reg_no='" + regno1 + "'";
                    //DataSet dsroll1 = d2.select_method_wo_parameter(rolno1, "text");
                    //double totalmarks1 = 0;
                    //string sql31 = "select sm.semester,sm.batch_year,sm.degree_code,Subject_Type,subject_name,subject_code,s.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,grade,cp,credit_points,maxtotal,me.exam_code,s.Part_Type,ed.Exam_Month,ed.Exam_year from Mark_Entry me,Subject s,sub_sem sem,syllabus_master sm,Exam_Details ed  where me.exam_code=ed.exam_code and sm.syll_code=s.syll_code and me.Subject_No = s.Subject_No and s.subtype_no= sem.subtype_no and  result='pass'  and roll_no='" + Convert.ToString(dsroll1.Tables[0].Rows[0]["roll_no"]).Trim() + "' order by sm.semester,subject_type,sem.lab,s.subjectpriority,s.subject_no,Part_Type";
                    //DataSet studclass1 = d2.select_method_wo_parameter(sql31, "text");
                    //if (studclass1.Tables.Count > 0 && studclass1.Tables[0].Rows.Count > 0)
                    //{
                    //    DataTable dtPart11 = studclass1.Tables[0].DefaultView.ToTable(true, "Part_Type");
                    //    dtPart11.DefaultView.Sort = "Part_Type ASC";
                    //    dtPart11 = dtPart11.DefaultView.ToTable();
                    //    if (dtPart11.Rows.Count > 0)
                    //    {
                    //        foreach (DataRow dtpart in dtPart11.Rows)
                    //        {
                    //            FpSpread2.Sheets[0].ColumnCount++;
                    //            string partno = Convert.ToString(dtpart["Part_Type"]);
                    //            if (partno == "")
                    //            {
                    //                partno = "0";
                    //            }
                    //            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "PART " + partno + "";
                    //            FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                    //            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //        }


                    //    }
                    //}
                        
                    int colcout = FpSpread2.Sheets[0].ColumnCount;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "MONTH OF PASSING";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "YEAR OF PASSING";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                    int sno = 0;
                    for (int j = 0; j < dgo.Tables[0].Rows.Count; j++)
                    {
                        FpSpread2.Sheets[0].RowCount++;
                        sno++;


                        string regno = Convert.ToString(dgo.Tables[0].Rows[j]["reg_no"]);
                        string studname = Convert.ToString(dgo.Tables[0].Rows[j]["stud_name"]);
                        string degre = Convert.ToString(dgo.Tables[0].Rows[j]["Degree"]);
                        string branch = Convert.ToString(dgo.Tables[0].Rows[j]["Dept_Name"]);
                        string appno = Convert.ToString(dgo.Tables[0].Rows[j]["app_no"]);
                        string apno = "select sex from applyn where app_no='" + appno + "'";
                        DataSet app = d2.select_method_wo_parameter(apno, "text");
                        string gender = string.Empty;
                        if (app.Tables[0].Rows[0]["sex"].ToString() == "0")
                        {
                            gender = "MALE";

                        }
                        else
                        {
                            gender = "FEMALE";
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = clname;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = clcode;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = regno;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = studname;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = gender;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = degre;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = branch;
                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = studname;
                        // FpSpread2.Sheets[0].ColumnCount=9;
                        int colcount = 9;
                        #region classification
                        string degCode = string.Empty;
                        DataSet gradeds = new DataSet();
                        string rolno = "select roll_no from registration where reg_no='" + regno + "'";
                        DataSet dsroll = d2.select_method_wo_parameter(rolno, "text");

                        string sql3 = "select sm.semester,sm.batch_year,sm.degree_code,Subject_Type,subject_name,subject_code,s.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,grade,cp,credit_points,maxtotal,me.exam_code,s.Part_Type,ed.Exam_Month,ed.Exam_year from Mark_Entry me,Subject s,sub_sem sem,syllabus_master sm,Exam_Details ed  where me.exam_code=ed.exam_code and sm.syll_code=s.syll_code and me.Subject_No = s.Subject_No and s.subtype_no= sem.subtype_no and  result='pass'  and roll_no='" + Convert.ToString(dsroll.Tables[0].Rows[0]["roll_no"]).Trim() + "' order by sm.semester,subject_type,sem.lab,s.subjectpriority,s.subject_no,Part_Type";

                        string sql4 = "select sm.semester,sm.batch_year,sm.degree_code,Subject_Type,subject_name,subject_code,s.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,grade,cp,credit_points,maxtotal,me.exam_code,s.Part_Type,ed.Exam_Month,ed.Exam_year from Mark_Entry me,Subject s,sub_sem sem,syllabus_master sm,Exam_Details ed  where me.exam_code=ed.exam_code and sm.syll_code=s.syll_code and me.Subject_No = s.Subject_No and s.subtype_no= sem.subtype_no and  result='pass' and roll_no='" + Convert.ToString(dsroll.Tables[0].Rows[0]["roll_no"]).Trim() + "' and Part_Type=1 order by sm.semester,subject_type,sem.lab,s.subjectpriority,s.subject_no,Part_Type";
                        DataSet partsub = d2.select_method_wo_parameter(sql4, "text");
                        string partsubject = string.Empty;
                        if (partsub.Tables[0].Rows.Count > 0 && partsub.Tables.Count > 0)
                        {
                            for (int k = 0; k < partsub.Tables[0].Rows.Count; k++)
                            {

                                if (string.IsNullOrEmpty(partsubject))
                                {
                                    partsubject = partsub.Tables[0].Rows[k]["Subject_Type"].ToString().Trim();
                                }
                                else
                                {
                                    partsubject = partsubject + "," + partsub.Tables[0].Rows[k]["Subject_Type"].ToString().Trim();
                                }
                            }
                        }
                        if (partsubject == "")
                        {
                            partsubject = "-";
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = partsubject;


                        DataSet studclass = d2.select_method_wo_parameter(sql3, "text");
                        if (studclass.Tables.Count > 0 && studclass.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtPart1 = studclass.Tables[0].DefaultView.ToTable(true, "Part_Type");
                            double partsums = 0.000;
                            double partwpmsum = 0.000;
                            int partrowcount = 0;
                            double Credit_Points = 0.0;
                            double grade_points = 0.0;
                            double creditstotal = 0;
                            double overalltotgrade = 0;
                            double Marks = 0;
                            dtPart1.DefaultView.Sort = "Part_Type ASC";
                            dtPart1 = dtPart1.DefaultView.ToTable();
                            if (dtPart1.Rows.Count > 0)
                            {

                                int subpart = dtPart1.Rows.Count;
                                int partcount1 = dspart.Tables[0].Rows.Count;
                                foreach (DataRow dtpart in dtPart1.Rows)
                                {
                                    string patva = Convert.ToString(dtpart["Part_Type"]);
                                    partsums = 0;
                                    partrowcount = 0;
                                    creditstotal = 0;
                                    partwpmsum = 0;
                                    overalltotgrade = 0;
                                    // row++;
                                    studclass.Tables[0].DefaultView.RowFilter = "Part_Type='" + patva + "' and result='pass'";
                                    DataTable dtPartwise = studclass.Tables[0].DefaultView.ToTable();
                                    string sumpart = string.Empty;
                                    string wpm = string.Empty;
                                    for (int sum = 0; sum < dtPartwise.Rows.Count; sum++)
                                    {
                                        double checkmarkmm = 0;
                                        double.TryParse(Convert.ToString(dtPartwise.Rows[sum]["total"]).Trim(), out checkmarkmm);
                                        double maxsubbtotal = 0;
                                        double.TryParse(Convert.ToString(dtPartwise.Rows[sum]["maxtotal"]).Trim(), out maxsubbtotal);
                                        if (maxsubbtotal != 0)
                                            checkmarkmm = checkmarkmm / maxsubbtotal * 100;
                                        checkmarkmm = Math.Round(checkmarkmm, 0, MidpointRounding.AwayFromZero);
                                        string sem = Convert.ToString(dtPartwise.Rows[sum]["semester"]).Trim();
                                        degCode = Convert.ToString(dtPartwise.Rows[sum]["degree_code"]).Trim();
                                        string batchYear = Convert.ToString(dtPartwise.Rows[sum]["batch_year"]).Trim();
                                        string gradesql1 = "select * from Grade_Master where College_Code='" + Convert.ToString(Session["collegecode"]).Trim() + "'and semester='" + sem + "' and Degree_Code='" + degCode + "' and batch_year='" + batchYear + "' and '" + checkmarkmm + "' between frange and trange";

                                        gradeds.Clear();
                                        gradeds = d2.select_method_wo_parameter(gradesql1, "Text");
                                        if (gradeds.Tables.Count > 0 && gradeds.Tables[0].Rows.Count == 0)
                                        {
                                            gradesql1 = "select * from Grade_Master where College_Code='" + Convert.ToString(Session["collegecode"]).Trim() + "' and Degree_Code='" + degCode + "' and batch_year='" + batchYear + "'  and '" + checkmarkmm + "' between frange and trange";
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesql1, "Text");
                                        }
                                        if (gradeds.Tables.Count > 0 && gradeds.Tables[0].Rows.Count > 0)
                                        {
                                            for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                            {
                                                if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                                {
                                                    grade_points = checkmarkmm;
                                                    grade_points = grade_points / 10;
                                                    double.TryParse(Convert.ToString(dtPartwise.Rows[sum]["credit_points"]), out Credit_Points);
                                                    creditstotal = creditstotal + Credit_Points;
                                                    partwpmsum += (Credit_Points * checkmarkmm);
                                                    partsums = partsums + (grade_points * Credit_Points);
                                                }
                                            }
                                        }
                                    }
                                    if (creditstotal == 0)
                                    {
                                        sumpart = "0.000";
                                        wpm = "0.00";
                                    }
                                    else if (creditstotal > 0)
                                    {
                                        partsums = (partsums / creditstotal);
                                        partsums = Math.Round(partsums, 3, MidpointRounding.AwayFromZero);
                                        partwpmsum = (partwpmsum / creditstotal);
                                        partwpmsum = Math.Round(partwpmsum, 2, MidpointRounding.AwayFromZero);
                                        sumpart = String.Format("{0:0.000}", partsums);
                                        wpm = string.Format("{0:0.00}", partwpmsum);
                                    }
                                    else
                                    {
                                        sumpart = "0.000";
                                        wpm = "0.00";
                                    }
                                    double sumpartgrade = 0;
                                    if (double.TryParse(sumpart, out sumpartgrade))
                                    {
                                        sumpartgrade = Convert.ToDouble(sumpart);
                                        overalltotgrade = overalltotgrade + sumpartgrade;
                                    }
                                    else
                                    {
                                        sumpartgrade = 0;
                                    }
                                    // string cgpa=d2.Calculete_CGPA(rolno,semval,degCode,batch_year,latmode,collegecode,false
                                    string edulevel = "select c.Edu_Level from Degree d,course c where d.Degree_Code='" + degCode + "' and c.Course_Id=d.Course_Id";
                                    DataSet edu = d2.select_method_wo_parameter(edulevel, "text");
                                    string gradesqlclass = "select * from coe_classification where '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  edu_level='" + Convert.ToString(edu.Tables[0].Rows[0]["Edu_Level"]) + "'";
                                    gradeds.Clear();
                                    gradeds = d2.select_method_wo_parameter(gradesqlclass, "Text");
                                    string cclass = string.Empty;
                                    string letterGrade = string.Empty;
                                    if (gradeds.Tables.Count > 0 && gradeds.Tables[0].Rows.Count > 0)
                                    {
                                        cclass = Convert.ToString(gradeds.Tables[0].Rows[0]["classification"]);
                                        letterGrade = Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]);
                                    }
                                    else
                                    {
                                        cclass = "-";
                                    }
                                    if (cclass == "")
                                    {
                                        cclass = "-";
                                    }
                                    colcount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcount - 1].Text = cclass;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcount - 1].HorizontalAlign = HorizontalAlign.Center;


                                }

                                if (partcount1 != subpart)
                                {
                                    int cout = partcount1 - subpart;
                                    for (int i1 = 0; i1 < cout; i1++)
                                    {
                                        colcount++;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcount - 1].Text = "-";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcount - 1].HorizontalAlign = HorizontalAlign.Center;

                                    }
                                }

                            }


                        }
                        #endregion
                        string exammonth = string.Empty;
                        string examyear = string.Empty;
                        string exmmonyr = "select top 1 ed.Exam_Month,ed.Exam_year from mark_entry me,Exam_Details ed  where ed.exam_code=me.exam_code and roll_no='" + dsroll.Tables[0].Rows[0]["roll_no"].ToString() + "' order by ed.Exam_year desc, ed.Exam_Month desc";
                        DataSet exmoyr = d2.select_method_wo_parameter(exmmonyr, "text");
                        if (exmoyr.Tables[0].Rows.Count > 0 && exmoyr.Tables.Count > 0)
                        {
                            exammonth = exmoyr.Tables[0].Rows[0]["Exam_Month"].ToString();
                            examyear = exmoyr.Tables[0].Rows[0]["Exam_year"].ToString();
                        }
                        else
                        {
                            exammonth = "-";
                            examyear = "-";
                        }
                        if (exammonth == "1")
                        {
                            exammonth = "JANUARY";
                        }
                        if (exammonth == "2")
                        {
                            exammonth = "FEBUARY";
                        }
                        if (exammonth == "3")
                        {
                            exammonth = "MARCH";
                        }
                        if (exammonth == "4")
                        {
                            exammonth = "APRIL";
                        }
                        if (exammonth == "5")
                        {
                            exammonth = "MAY";
                        }
                        if (exammonth == "6")
                        {
                            exammonth = "JUNE";
                        }
                        if (exammonth == "7")
                        {
                            exammonth = "JULY";
                        }
                        if (exammonth == "8")
                        {
                            exammonth = "AUGUST";
                        }
                        if (exammonth == "9")
                        {
                            exammonth = "SEPTEMBER";
                        }
                        if (exammonth == "10")
                        {
                            exammonth = "OCTOBER";
                        }
                        if (exammonth == "11")
                        {
                            exammonth = "NOVEMBER";
                        }
                        if (exammonth == "12")
                        {
                            exammonth = "DECEMBER";
                        }
                        int colcout1 = colcout;
                        colcout1++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcout1 - 1].Text = exammonth;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcout1 - 1].HorizontalAlign = HorizontalAlign.Center;
                        colcout1++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcout1 - 1].Text = examyear;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcout1 - 1].HorizontalAlign = HorizontalAlign.Center;

                    }
                    divtable.Visible = true;
                    FpSpread2.Visible = true;
                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    FpSpread2.Height = (FpSpread2.Sheets[0].RowCount * 20) + 100;
                    FpSpread2.SaveChanges();
                    div_report.Visible = true;


                }
                else
                {
                    FpSpread2.Visible = false;
                    divPopupAlert.Visible = true;
                    divAlertContent.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Records Found";
                    div_report.Visible = false;

                }
            }
            else if (CheckBox2.Checked == true)
            {
                if (dgo.Tables[0].Rows.Count > 0 && dgo.Tables.Count > 0)
                {
                    FpSpread2.CommandBar.Visible = false;
                    FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;
                    FpSpread2.Sheets[0].RowHeader.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.Black;
                    FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    darkstyle.Font.Name = "Book Antiqua";
                    darkstyle.Font.Size = FontUnit.Medium;
                    darkstyle.HorizontalAlign = HorizontalAlign.Center;
                    darkstyle.VerticalAlign = VerticalAlign.Middle;
                    FpSpread2.Visible = true;
                    FpSpread2.Sheets[0].RowCount = 0;
                    FpSpread2.Sheets[0].ColumnCount = 0;

                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.NO";
                    FpSpread2.Sheets[0].Columns[0].Width = 70;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "REGISTER NO.";
                    FpSpread2.Sheets[0].Columns[1].Width = 150;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "STUDENT NAME";
                    FpSpread2.Sheets[0].Columns[2].Width = 200;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "SEX";
                    FpSpread2.Sheets[0].Columns[2].Width = 200;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                    int c = 8;
                    int m = FpSpread2.Sheets[0].ColumnCount;
                    int n = FpSpread2.Sheets[0].ColumnCount;
                    int n1 = n;
                    //if (dspart.Tables.Count > 0 && dspart.Tables[0].Rows.Count > 0)
                    //{

                    //    for (int i = 0; i < dspart.Tables[0].Rows.Count; i++)
                    //    {
                    //        FpSpread2.Sheets[0].ColumnCount++;
                    //        string partno = Convert.ToString(dspart.Tables[0].Rows[i]["part_type"]);
                    //        if (partno == "")
                    //        {
                    //            partno = "0";
                    //        }
                    //        if (partno == "1")
                    //        {
                    //            partno = " I";
                    //        }
                    //        else if (partno == "2")
                    //        {
                    //            partno = " II";
                    //        }
                    //        else if (partno == "3")
                    //        {
                    //            partno = " III";
                    //        }
                    //        else if (partno == "4")
                    //        {
                    //            partno = " IV";
                    //        }
                    //        else if (partno == "5")
                    //        {
                    //            partno = " V";
                    //        }
                    //        else if (partno == "6")
                    //        {
                    //            partno = " VI";
                    //        }
                    //        FpSpread2.Sheets[0].ColumnCount++;
                    //        FpSpread2.Sheets[0].ColumnHeader.Cells[0, m].Text = "MARKS SECURED IN PART-" + partno + "";
                    //        FpSpread2.Sheets[0].Columns[m].Width = 80;
                    //        FpSpread2.Sheets[0].ColumnHeader.Cells[0, m].HorizontalAlign = HorizontalAlign.Center;

                    //        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n].Text = "OUT OF " + totalmarks1 + "";
                    //        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n].HorizontalAlign = HorizontalAlign.Center;
                    //        FpSpread2.Sheets[0].ColumnCount++;

                    //        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n + 1].Text = "CLASS AWARDED";
                    //        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n + 1].HorizontalAlign = HorizontalAlign.Center;
                    //        FpSpread2.Sheets[0].ColumnCount++;
                    //        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n + 2].Text = "CGPA";
                    //        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n + 2].HorizontalAlign = HorizontalAlign.Center;
                    //        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, m, 1, 3);

                    //        n = n + 3;
                    //        m = m + 3;
                           

                            string regno1 = Convert.ToString(dgo.Tables[0].Rows[0]["reg_no"]);
                            string rolno1 = "select roll_no from registration where reg_no='" + regno1 + "'";
                            DataSet dsroll1 = d2.select_method_wo_parameter(rolno1, "text");
                            double totalmarks1=0;
                            string sql31 = "select sm.semester,sm.batch_year,sm.degree_code,Subject_Type,subject_name,subject_code,s.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,grade,cp,credit_points,maxtotal,me.exam_code,s.Part_Type,ed.Exam_Month,ed.Exam_year from Mark_Entry me,Subject s,sub_sem sem,syllabus_master sm,Exam_Details ed  where me.exam_code=ed.exam_code and sm.syll_code=s.syll_code and me.Subject_No = s.Subject_No and s.subtype_no= sem.subtype_no and  result='pass'  and roll_no='" + Convert.ToString(dsroll1.Tables[0].Rows[0]["roll_no"]).Trim() + "' order by sm.semester,subject_type,sem.lab,s.subjectpriority,s.subject_no,Part_Type";
                            DataSet studclass1 = d2.select_method_wo_parameter(sql31, "text");
                            if (studclass1.Tables.Count > 0 && studclass1.Tables[0].Rows.Count > 0)
                            {
                                DataTable dtPart11 = studclass1.Tables[0].DefaultView.ToTable(true, "Part_Type");
                                dtPart11.DefaultView.Sort = "Part_Type ASC";
                                dtPart11 = dtPart11.DefaultView.ToTable();
                                if (dtPart11.Rows.Count > 0)
                                {

                                    foreach (DataRow dtpart in dtPart11.Rows)
                                    {
                                        totalmarks1 = 0;
                                        string patva1 = Convert.ToString(dtpart["Part_Type"]);
                                        studclass1.Tables[0].DefaultView.RowFilter = "Part_Type='" + patva1 + "' and result='pass'";
                                        DataTable dtPartwise1 = studclass1.Tables[0].DefaultView.ToTable();
                                        if (patva1 == "1")
                                        {
                                            patva1 = " I";
                                        }
                                        else if (patva1 == "2")
                                        {
                                            patva1 = " II";
                                        }
                                        else if (patva1 == "3")
                                        {
                                            patva1 = " III";
                                        }
                                        else if (patva1 == "4")
                                        {
                                            patva1 = " IV";
                                        }
                                        else if (patva1 == "5")
                                        {
                                            patva1 = " V";
                                        }
                                        else if (patva1 == "6")
                                        {
                                            patva1 = " VI";
                                        }
                            
                                        for (int sum1 = 0; sum1 < dtPartwise1.Rows.Count; sum1++)
                                        {
                                            double checkmarkmm = 0;
                                            double.TryParse(Convert.ToString(dtPartwise1.Rows[sum1]["total"]).Trim(), out checkmarkmm);


                                            double maxsubbtotal1 = 0;
                                            double.TryParse(Convert.ToString(dtPartwise1.Rows[sum1]["maxtotal"]).Trim(), out maxsubbtotal1);
                                            totalmarks1 = totalmarks1 + maxsubbtotal1;
                                        }
                                        FpSpread2.Sheets[0].ColumnCount++;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, m].Text = "MARKS SECURED IN PART-" + patva1 + "";
                                        FpSpread2.Sheets[0].Columns[m].Width = 80;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, m].HorizontalAlign = HorizontalAlign.Center;

                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n].Text = "OUT OF " + totalmarks1 + "";
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].ColumnCount++;

                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n + 1].Text = "CLASS AWARDED";
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n + 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].ColumnCount++;
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n + 2].Text = "CGPA";
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, n + 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, m, 1, 3);

                                        n = n + 3;
                                        m = m + 3;
                                    }
                                }
                            }                                                                                                                        
                      // }
                   // }
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "FOR THE USE OF COE(BU) OFFICE";
                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 280;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 1, 2, 1);
                    int sno = 0;
                    int n2=0;
                    for (int j = 0; j < dgo.Tables[0].Rows.Count; j++)
                    {
                        
                        FpSpread2.Sheets[0].RowCount++;
                        sno++;
                        n2=n1;
                        string regno = Convert.ToString(dgo.Tables[0].Rows[j]["reg_no"]);
                        string studname = Convert.ToString(dgo.Tables[0].Rows[j]["stud_name"]);
                        string appno = Convert.ToString(dgo.Tables[0].Rows[j]["app_no"]);
                        string apno = "select sex from applyn where app_no='" + appno + "'";
                        DataSet app = d2.select_method_wo_parameter(apno, "text");
                        string gender = string.Empty;
                        if (app.Tables[0].Rows[0]["sex"].ToString() == "0")
                        {
                            gender = "MALE";

                        }
                        else
                        {
                            gender = "FEMALE";
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = regno;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = studname;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = gender;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                        #region classification
                        string degCode = string.Empty;
                        DataSet gradeds = new DataSet();
                        string rolno = "select roll_no from registration where reg_no='" + regno + "'";
                        DataSet dsroll = d2.select_method_wo_parameter(rolno, "text");

                        string sql3 = "select sm.semester,sm.batch_year,sm.degree_code,Subject_Type,subject_name,subject_code,s.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,grade,cp,credit_points,maxtotal,me.exam_code,s.Part_Type,ed.Exam_Month,ed.Exam_year from Mark_Entry me,Subject s,sub_sem sem,syllabus_master sm,Exam_Details ed  where me.exam_code=ed.exam_code and sm.syll_code=s.syll_code and me.Subject_No = s.Subject_No and s.subtype_no= sem.subtype_no and  result='pass'  and roll_no='" + Convert.ToString(dsroll.Tables[0].Rows[0]["roll_no"]).Trim() + "' order by sm.semester,subject_type,sem.lab,s.subjectpriority,s.subject_no,Part_Type";

                     
                          double marktotal=0;
                          double totalmarks = 0;
                        DataSet studclass = d2.select_method_wo_parameter(sql3, "text");
                        if (studclass.Tables.Count > 0 && studclass.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtPart1 = studclass.Tables[0].DefaultView.ToTable(true, "Part_Type");
                            double partsums = 0.000;
                            double partwpmsum = 0.000;
                            int partrowcount = 0;
                            double Credit_Points = 0.0;
                            double grade_points = 0.0;
                            double creditstotal = 0;
                            double overalltotgrade = 0;
                            double Marks = 0;
                            dtPart1.DefaultView.Sort = "Part_Type ASC";
                            dtPart1 = dtPart1.DefaultView.ToTable();
                            if (dtPart1.Rows.Count > 0)
                            {

                                foreach (DataRow dtpart in dtPart1.Rows)
                                {
                                    string patva = Convert.ToString(dtpart["Part_Type"]);
                                    partsums = 0;
                                    partrowcount = 0;
                                    creditstotal = 0;
                                    partwpmsum = 0;
                                    overalltotgrade = 0;
                                     marktotal = 0;
                                     totalmarks = 0;
                                    // row++;
                                    studclass.Tables[0].DefaultView.RowFilter = "Part_Type='" + patva + "' and result='pass'";
                                    DataTable dtPartwise = studclass.Tables[0].DefaultView.ToTable();
                                    string sumpart = string.Empty;
                                    string wpm = string.Empty;
                                    for (int sum = 0; sum < dtPartwise.Rows.Count; sum++)
                                    {
                                        double checkmarkmm = 0;
                                        double.TryParse(Convert.ToString(dtPartwise.Rows[sum]["total"]).Trim(), out checkmarkmm);
                                       
                                        marktotal = marktotal + checkmarkmm;
                                        double maxsubbtotal = 0;
                                        double.TryParse(Convert.ToString(dtPartwise.Rows[sum]["maxtotal"]).Trim(), out maxsubbtotal);
                                        totalmarks = totalmarks + maxsubbtotal;
                                        if (maxsubbtotal != 0)
                                            checkmarkmm = checkmarkmm / maxsubbtotal * 100;
                                        checkmarkmm = Math.Round(checkmarkmm, 0, MidpointRounding.AwayFromZero);
                                        string sem = Convert.ToString(dtPartwise.Rows[sum]["semester"]).Trim();
                                        degCode = Convert.ToString(dtPartwise.Rows[sum]["degree_code"]).Trim();
                                        string batchYear = Convert.ToString(dtPartwise.Rows[sum]["batch_year"]).Trim();
                                        string gradesql1 = "select * from Grade_Master where College_Code='" + Convert.ToString(Session["collegecode"]).Trim() + "'and semester='" + sem + "' and Degree_Code='" + degCode + "' and batch_year='" + batchYear + "' and '" + checkmarkmm + "' between frange and trange";

                                        gradeds.Clear();
                                        gradeds = d2.select_method_wo_parameter(gradesql1, "Text");
                                        if (gradeds.Tables.Count > 0 && gradeds.Tables[0].Rows.Count == 0)
                                        {
                                            gradesql1 = "select * from Grade_Master where College_Code='" + Convert.ToString(Session["collegecode"]).Trim() + "' and Degree_Code='" + degCode + "' and batch_year='" + batchYear + "'  and '" + checkmarkmm + "' between frange and trange";
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesql1, "Text");
                                        }
                                        if (gradeds.Tables.Count > 0 && gradeds.Tables[0].Rows.Count > 0)
                                        {
                                            for (int grd = 0; grd < gradeds.Tables[0].Rows.Count; grd++)
                                            {
                                                if (Convert.ToInt32(gradeds.Tables[0].Rows[grd][1]) <= checkmarkmm && Convert.ToInt32(gradeds.Tables[0].Rows[grd][2]) >= checkmarkmm)
                                                {
                                                    grade_points = checkmarkmm;
                                                    grade_points = grade_points / 10;
                                                    double.TryParse(Convert.ToString(dtPartwise.Rows[sum]["credit_points"]), out Credit_Points);
                                                    creditstotal = creditstotal + Credit_Points;
                                                    partwpmsum += (Credit_Points * checkmarkmm);
                                                    partsums = partsums + (grade_points * Credit_Points);
                                                }
                                            }
                                        }
                                    }
                                    if (creditstotal == 0)
                                    {
                                        sumpart = "0.000";
                                        wpm = "0.00";
                                    }
                                    else if (creditstotal > 0)
                                    {
                                        partsums = (partsums / creditstotal);
                                        partsums = Math.Round(partsums, 3, MidpointRounding.AwayFromZero);
                                        partwpmsum = (partwpmsum / creditstotal);
                                        partwpmsum = Math.Round(partwpmsum, 2, MidpointRounding.AwayFromZero);
                                        sumpart = String.Format("{0:0.000}", partsums);
                                        wpm = string.Format("{0:0.00}", partwpmsum);
                                    }
                                    else
                                    {
                                        sumpart = "0.000";
                                        wpm = "0.00";
                                    }
                                    double sumpartgrade = 0;
                                    if (double.TryParse(sumpart, out sumpartgrade))
                                    {
                                        sumpartgrade = Convert.ToDouble(sumpart);
                                        overalltotgrade = overalltotgrade + sumpartgrade;
                                    }
                                    else
                                    {
                                        sumpartgrade = 0;
                                    }
                                    // string cgpa=d2.Calculete_CGPA(rolno,semval,degCode,batch_year,latmode,collegecode,false
                                    string edulevel = "select c.Edu_Level from Degree d,course c where d.Degree_Code='" + degCode + "' and c.Course_Id=d.Course_Id";
                                    DataSet edu = d2.select_method_wo_parameter(edulevel, "text");
                                    string gradesqlclass = "select * from coe_classification where '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'<= topoint and  edu_level='" + Convert.ToString(edu.Tables[0].Rows[0]["Edu_Level"]) + "'";
                                    gradeds.Clear();
                                    gradeds = d2.select_method_wo_parameter(gradesqlclass, "Text");
                                    string cclass = string.Empty;
                                    string letterGrade = string.Empty;
                                    if (gradeds.Tables.Count > 0 && gradeds.Tables[0].Rows.Count > 0)
                                    {
                                        cclass = Convert.ToString(gradeds.Tables[0].Rows[0]["classification"]);
                                        letterGrade = Convert.ToString(gradeds.Tables[0].Rows[0]["grade"]);
                                    }
                                    else
                                    {
                                        cclass = "-";
                                    }
                                    if (cclass == "")
                                    {
                                        cclass = "-";
                                    }
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, n2].Text = Convert.ToString(marktotal);
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, n2 + 1].Text = Convert.ToString(cclass);
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, n2 + 2].Text = Convert.ToString(overalltotgrade);
                                    n2 = n2 + 3;

                                }
                            }


                        }
                        #endregion
                                             
                    }
                    divtable.Visible = true;
                    FpSpread2.Visible = true;
                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    FpSpread2.Height = (FpSpread2.Sheets[0].RowCount * 20) + 100;
                    FpSpread2.SaveChanges();
                    div_report.Visible = true;


                }
                else
                {
                    FpSpread2.Visible = false;
                    divPopupAlert.Visible = true;
                    divAlertContent.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "No Records Found";
                    div_report.Visible = false;

                }
            }

                                             
                
            






        }
        catch
        {
        }
    }
    public void format2()
    {
        try
        {

        }
        catch
        {
        }
    }
    protected void btn_printmaster_Click1(object sender, EventArgs e)
    {
        try
        {
            string studenteligblity = "SAMPLE OF UNIVERSITY REPORT";
            string pagename = "awardofdegree.aspx";
            Printcontrol1.loadspreaddetails(FpSpread2, pagename, studenteligblity);
            Printcontrol1.Visible = true;
        }
        catch { }
    }
    protected void btnExcel_Click1(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname1.Text;
            if (report.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread2, report);
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
            }
            btn_Excel1.Focus();
        }
        catch
        {

        }

    }
    protected void txtexcelname_TextChanged1(object sender, EventArgs e)
    {
        try
        {
            txt_excelname1.Visible = true;
            btn_Excel1.Visible = true;
            btn_printmaster1.Visible = true;
            lbl_reportname1.Visible = true;
            btn_Excel1.Focus();
            if (txt_excelname1.Text == "")
            {
                lbl_norec1.Visible = true;
            }
            else
            {
                lbl_norec1.Visible = false;
            }
        }
        catch { }



    }
    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        divPopupAlert.Visible = false;
        divAlertContent.Visible = false;
    }
}