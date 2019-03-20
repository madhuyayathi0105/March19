using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

public partial class IndividualStudentGPA : System.Web.UI.Page
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

    string strbatchsplit = string.Empty;
    string strbranchsplit = string.Empty;
    string strsecsplit = string.Empty;


    string syllcode = "";
    string subname = "";
    string section = "";
    string flag = "true";
    string flagc = "true";
    string syll_code = "";
    string examcodeval = "";
    string examcodevalg = "";


    int count = 0;
    int count1 = 0;
    int count2 = 0;
    int count3 = 0;
    int count4 = 0;

    static int batchcnt = 0;
    static int degreecnt = 0;
    static int branchcnt = 0;
    static int sectioncnt = 0;

    string sqlcmdbatch = "", sqlcmdbranch = "", sqlcmdseme = "";
    int strexam;
    string strexamyear = "";
    string strexammonth = "";
    string sqlcmdretriveunialldet;

    string degree_code1 = "";
    string current_sem1 = "";
    string batch_year1 = "";
    string exam_month1 = "";
    string exam_year1 = "";
    int semdec = 0;

    string strgradetempgrade = "";
    string strtotgrac = "";
    double strtot = 0;
    double strgradetempfrm = 0;
    double strgradetempto = 0;
    string tempexmonth = "";
    string tempexyear = "";
    string gtempexmonth = "";
    string gtempexyear = "";
    int gtempejval = 0;

    string degtemp1 = "";
    string batchtemp1 = "";

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet1();
    DataSet ds2 = new DataSet1();
    DataSet dgo = new DataSet();
    DataSet dgo2 = new DataSet();
    Hashtable hat = new Hashtable();
    DataSet dggradetot = new DataSet();
    DataSet dggradegra = new DataSet();
    DataSet dssem = new DataSet();

    SqlCommand cmd;

    SqlConnection con_Grade = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Getfunc = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

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
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (!IsPostBack)
            {


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
                Button1.Visible = false;
                LabelE.Visible = false;
                lblnorec.Visible = false;
                errmsg.Visible = false;
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                BindSectionDetailmult(collegecode);
                Bindmultiseme(collegecode);

            }
        }
        catch(Exception ex)
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




    protected void btngo_Click(object sender, EventArgs e)
    {

        try
        {

            // go method

            gomethod();
         

        }
        catch (Exception ex)
        {
            string exe = ex.ToString();
            errmsg.Visible = true;
            errmsg.Text = exe.ToString();

        }
    }


    public void gomethod()
    {
        string sqlcmdgraderstotal = "";
        string GPA_Val = "";
        string CGPA_Val = "";
        string RollNo1 = "";
        string semval = "";
        string strtempseme = "";

        string latmode1 = "";




        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].ColumnHeader.Visible = true;
        FpSpread1.Sheets[0].ColumnCount++;



        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
        FpSpread1.Sheets[0].ColumnCount = 8;
        FpSpread1.Sheets[0].RowCount = 0;

        string sqlcmdall = "select distinct ROW_NUMBER() OVER (ORDER BY  Roll_no) As SrNo,Convert(Varchar,batch_year) + '-' + acronym as Degree,roll_no,reg_no,stud_name,stud_type,Convert(Varchar,r.batch_year) + '-' + Convert(Varchar,r.degree_code) as DegreeCode,mode as Mode from registration r,degree d where r.degree_code=d.degree_code";

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


        sqlcmdall = sqlcmdall + "and cc=0 and delflag=0 and exam_flag<>'debar' order by r.reg_no";
        dgo = d2.select_method(sqlcmdall, hat, "Text");
        FarPoint.Web.Spread.TextCellType textcell = new FarPoint.Web.Spread.TextCellType();
        if (dgo != null && dgo.Tables[0] != null && dgo.Tables[0].Rows.Count > 0)
        {
            FpSpread1.Visible = true;

            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
            FpSpread1.Sheets[0].Columns[3].CellType = textcell;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Type";
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].Visible = false;
            FpSpread1.Sheets[0].Columns[7].Visible = false;

            //FpSpread1.DataSource = dgo;
            //FpSpread1.DataBind();
            int slno = 0;
            for (int cnt = 0; cnt < dgo.Tables[0].Rows.Count; cnt++)
            {
                slno++;
                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                int rc = FpSpread1.Sheets[0].RowCount - 1;
                //magesh 27/2/18
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                FpSpread1.Sheets[0].Cells[rc, 2].CellType = txt;//magesh 27/1/18
                FpSpread1.Sheets[0].Cells[rc, 0].Text = slno.ToString();
                FpSpread1.Sheets[0].Cells[rc, 1].Text = dgo.Tables[0].Rows[cnt]["Degree"].ToString();
                FpSpread1.Sheets[0].Cells[rc, 2].Text = dgo.Tables[0].Rows[cnt]["roll_no"].ToString();
                FpSpread1.Sheets[0].Cells[rc, 3].Text = dgo.Tables[0].Rows[cnt]["reg_no"].ToString();
                FpSpread1.Sheets[0].Cells[rc, 4].Text = dgo.Tables[0].Rows[cnt]["stud_name"].ToString();
                FpSpread1.Sheets[0].Cells[rc, 5].Text = dgo.Tables[0].Rows[cnt]["stud_type"].ToString();
                FpSpread1.Sheets[0].Cells[rc, 6].Text = dgo.Tables[0].Rows[cnt]["DegreeCode"].ToString();
                FpSpread1.Sheets[0].Cells[rc, 7].Text = dgo.Tables[0].Rows[cnt]["Mode"].ToString();
            }


            sqlcmdretriveunialldet = "select distinct exam_month,exam_year,DATENAME(MONTH,'1990/' + CAST(exam_month AS VARCHAR(3)) + '/23')as exam_month1 from exam_details where " + sqlcmdbranch + " " + sqlcmdbatch + " " + sqlcmdseme + " order by exam_year,exam_month";

            dgo2 = d2.select_method(sqlcmdretriveunialldet, hat, "Text");
            if (dgo2 != null && dgo2.Tables[0] != null && dgo2.Tables[0].Rows.Count > 0)
            {
                for (int o = 0; o < dgo2.Tables[0].Rows.Count; o++)
                {
                    strexammonth = Convert.ToString(dgo2.Tables[0].Rows[o]["exam_month1"]);
                    strexamyear = dgo2.Tables[0].Rows[o]["exam_year"].ToString();


                    FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(strexammonth + "-" + strexamyear);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(dgo2.Tables[0].Rows[o]["exam_month"]);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(strexamyear);
                }

                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "CGPA";
            }


            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

            for (int q = 0; q < FpSpread1.Sheets[0].RowCount; q++)
            {
                int col_count = FpSpread1.Sheets[0].ColumnCount;
                string tempt1 = "", tempt2 = "";
                Boolean checkfailstatus = false;//added By srinath

                for (int r = 8; r < col_count; r++)
                {
                    
                    RollNo1 = Convert.ToString(FpSpread1.Sheets[0].Cells[q, 2].Text);
                    string[] degcodebatchyr = Convert.ToString(FpSpread1.Sheets[0].Cells[q, 6].Text).Split(new Char[] { '-' });
                    if (degcodebatchyr.GetUpperBound(0) >= 1)
                    {
                        batch_year1 = Convert.ToString(degcodebatchyr[0]);
                        degree_code1 = Convert.ToString(degcodebatchyr[1]);
                    }

                    exam_month1 = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, r].Note);
                    exam_year1 = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, r].Tag);

                    latmode1 = Convert.ToString(dgo.Tables[0].Rows[q]["mode"]);



                    if (RollNo1 != null && RollNo1 != "" && degree_code1 != null && degree_code1 != "" && batch_year1 != null && batch_year1 != "" && exam_month1 != null && exam_month1 != "" && exam_year1 != null && exam_year1 != "" && semval != null && semval != "" && latmode1 != null && latmode1 != "")
                    {

                        sqlcmdgraderstotal = " select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code1 + " and batch_year=" + batch_year1 + " and college_code=" + Session["collegecode"] + "";
                        dggradetot = d2.select_method(sqlcmdgraderstotal, hat, "Text");

                        //Added By Srinath 12/2/2013 ==Start
                        string syll_code = "";
                        syll_code = GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code1 + "' and e.batch_year=" + batch_year1 + " and exam_month=" + exam_month1 + " and exam_year=" + exam_year1 + "");

                        //string checkresult = "Select Mark_Entry.result,Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject,exam_details e where Mark_Entry.Subject_No = Subject.Subject_No and e.exam_code=mark_entry.exam_code and e.degree_code=" + degree_code1 + " and e.batch_year=" + batch_year1 + " and subject.syll_code="+ syll_code +" and roll_no='" + RollNo1 + "' and result<>'Pass'";
                        string checkresult = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + RollNo1 + "') and roll_no ='" + RollNo1 + "'  and subject.syll_code=" + syll_code.ToString() + " )";
                        DataSet dscheckresult = d2.select_method(checkresult, hat, "Text");

                       //Rajkumar for fail CGPA on 29-5-2018 ===========
                        bool ArrerCheckFlag = false;
                        string val1 = d2.GetFunctionv("select value from Master_Settings where settings = 'include gpa for fail student'");//Rajkumar on 28-5-2018
                        if (val1.Trim() == "true" || val1.Trim() == "1")
                            ArrerCheckFlag = true;
                        //=====================
                        if (dscheckresult.Tables[0].Rows.Count > 0 && !ArrerCheckFlag)
                        {
                            GPA_Val = "-";
                            checkfailstatus = true;
                        }
                        else
                        {
                            // GPA_Val = Calulat_GPA(RollNo1,degree_code1,batch_year1,exam_month1 ,exam_year1 );//modified By Srinath 12/2/2013
                            GPA_Val = d2.Calulat_GPA_Semwise(RollNo1, degree_code1, batch_year1, exam_month1, exam_year1, collegecode);

                        }
                        //------------------rajkumar
                        FpSpread1.Sheets[0].Cells[q, r].HorizontalAlign = HorizontalAlign.Center;
                        if (Convert.ToString(GPA_Val) == "0")
                        {
                            FpSpread1.Sheets[0].Cells[q, r].Text = "-";
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[q, r].Text = Convert.ToString(GPA_Val);
                        }
                        //===End

                        if (r >= col_count - 2)
                        { //Added By Srinath 12/2/2013 ==Start
                            if (checkfailstatus == false)
                            {
                                CGPA_Val = d2.Calculete_CGPA(RollNo1, semval, degree_code1, batch_year1, latmode1, collegecode);
                            }
                            else
                            {
                                CGPA_Val = "-";
                            }
                            //  CGPA_Val = Calculete_CGPA(RollNo1, semval, degree_code1, batch_year1, exam_month1, exam_year1, latmode1);//Hiden By Srinath 12/2/2013
                            //===End
                            FpSpread1.Sheets[0].Cells[q, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            if (Convert.ToString(CGPA_Val) == "0")
                            {
                                FpSpread1.Sheets[0].Cells[q, FpSpread1.Sheets[0].ColumnCount - 1].Text = "-";
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[q, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(CGPA_Val);
                            }

                        }
                    }
                }
            }


            btnxl.Visible = true;
            Button1.Visible = true;



        }


    }

    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        con_Getfunc.Close();
        con_Getfunc.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con_Getfunc);
        SqlDataReader drnew;
        SqlCommand funcmd = new SqlCommand(sqlstr);
        funcmd.Connection = con_Getfunc;
        drnew = funcmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "0";
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        Printcontrol.Visible = true;
        string degreedetails = "Individual Student GPA Report ";
        string pagename = "IndividualStudentGPA.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        FpSpread1.Visible = true;
        errmsg.Visible = false;

    }

}