using System;//-------------modified on 24/2/12
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BalAccess;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;
using System.Reflection;
using Gios.Pdf;
using System.IO;
using System.Collections.Generic;


public partial class overall : System.Web.UI.Page
{
    public DataSet Bind_Degree(string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    [Serializable()]
    public class MyImg1 : ImageCellType
    {


        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(60);
            //        img.Height = Unit.Percentage(70);
            return img;

        }
    }
    public class MyImg2 : ImageCellType
    {


        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            //''------------clg left logo
            System.Web.UI.WebControls.Image img1 = new System.Web.UI.WebControls.Image();
            img1.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img1.Width = Unit.Percentage(70);
            // img1.Height = Unit.Percentage(130);
            return img1;

        }
    }
    public class MyImg3 : ImageCellType
    {


        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            //'-------------clg right logo
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img2.Width = Unit.Percentage(80);
            //   img2.Height = Unit.Percentage(70);
            return img2;

        }
    }

    public class MyImgright : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            //'-------------clg right logo
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img2.Width = Unit.Percentage(105);
            //   img2.Height = Unit.Percentage(35);
            return img2;

        }
    }

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection condegree = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection rankcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    //string regularflag = "";
    string markglag = "";
    string rol_no = "";
    string courseid = "";
    string atten = "";
    string Master = "";
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    string staff = "";
    double perofpass = 0;
    double avg = 0;
    Boolean IsFirstcol = false;
    Boolean Isfirst = false;
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string strorder = "";
    string strregorder = "";
    //'--------------------------------new start------------------
    Hashtable hat = new Hashtable();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DAccess2 daccess2 = new DAccess2();

    DataSet ds_load = new DataSet();
    //'-------------------------------------new end------------------
    //----------------new start 05.04.12
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

    int final_print_col_cnt = 0;
    string footer_text = "";
    int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int footer_count = 0;
    //----------------------end 05.04.12
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
                setLabelText();
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
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 12;
                style.Font.Bold = true;
                style.Font.Name = "Book Antiqua";
                style.HorizontalAlign = HorizontalAlign.Center;
                style.ForeColor = Color.Black;
                style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpEntry.Sheets[0].ColumnHeader.DefaultStyle = style;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = style;
                FpSpread3.Sheets[0].ColumnHeader.DefaultStyle = style;
                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    lblnorec.Text = "";
                    lblnorec.Visible = false;
                    ddlcollege.DataSource = dsprint;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    ddlcollege_SelectedIndexChanged(sender, e);
                }
                else
                {
                    lblnorec.Text = "Set college rights to the staff";
                    lblnorec.Visible = true;
                    lblerror.Visible = false;
                    FpSpread1.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnExcel.Visible = false;
                    BtnPrint.Visible = false;
                    return;
                }
                Pageload(sender, e);
            }
            if (Label4.Text.Trim().ToLower() == "school")
            {
                lblYear.Text = "Year";
            }

        }
        catch
        {
        }
    }
    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(Label4);
        fields.Add(0);
        //lbl.Add(lbl_Stream);
        //fields.Add(1);
        lbl.Add(lblDegree);
        fields.Add(2);
        lbl.Add(lblBranch);
        fields.Add(3);
        lbl.Add(lblDuration);
        fields.Add(4);
        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    public void bindbatch()
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
            con.Close();
        }

    }
    public void bindbranch()
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

    public void binddegree()
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
    public void bindsec()
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
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = FpSpread1.FindControl("Update");
        Control cntCancelBtn = FpSpread1.FindControl("Cancel");
        Control cntCopyBtn = FpSpread1.FindControl("Copy");
        Control cntCutBtn = FpSpread1.FindControl("Clear");
        Control cntPasteBtn = FpSpread1.FindControl("Paste");
        //  Control cntPageNextBtn = FpSpread1.FindControl("Next");
        //   Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
        Control cntPagePrintBtn = FpSpread1.FindControl("Print");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntCancelBtn.Parent;
            //tr.Cells.Remove(tc);


            //tc = (TableCell)cntCopyBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntCutBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPasteBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPagePreviousBtn.Parent;
            //tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePrintBtn.Parent;
            tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }



    public void GetTest()
    {
        try
        {
            ddlTest.Items.Clear();
            con.Close();
            con.Open();
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SyllabusYr = GetFunction(SyllabusQry.ToString());
            string Sqlstr;
            Sqlstr = "";


            Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";


            SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, con);
            DataSet titles = new DataSet();
            con.Close();
            con.Open();
            sqlAdapter1.Fill(titles);
            if (titles.Tables[0].Rows.Count > 0)
            {

                ddlTest.Items.Clear();
                ddlTest.DataSource = titles;
                ddlTest.DataValueField = "Criteria_No";
                ddlTest.DataTextField = "Criteria";
                ddlTest.DataBind();
                ddlTest.Items.Add("--Select--");
                ddlTest.SelectedIndex = ddlTest.Items.Count - 1;
                //ddlTest.Items.Insert(0, new ListItem("--Select--", "-1"));

            }
        }
        catch
        {

        }

    }

    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        con.Close();
        con.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = con;
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



    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
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
        //Added By Srinath 28/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        string course_id = ddlDegree.SelectedValue.ToString();
        string collegecode = Session["InternalCollegeCode"].ToString();
        string usercode = Session["usercode"].ToString();
        bindbranch();

        //bind semester
        bindsem();
        //bind section
        bindsec();
        //bing test
        GetTest();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
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
        //Added By Srinath 28/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        if (!Page.IsPostBack == false)
        {
            //ddlSemYr.Items.Clear();
        }
        try
        {
            if ((ddlBranch.SelectedIndex != 0) && (ddlBranch.SelectedIndex > 0))
            {
                bindsem();
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }

        //bind section
        bindsec();
        //bing test
        GetTest();
    }

    public void BindSectionDetail()
    {

        string branch = ddlBranch.SelectedValue.ToString();
        string batch = ddlBatch.SelectedValue.ToString();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);

        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
        //ddlTest.Items.Insert(0, new ListItem("--Select--", "-1"));
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
            {
                ddlSec.Enabled = false;
                GetTest();
            }
            else
            {
                ddlSec.Enabled = true;
                GetTest();
            }
        }
        else
        {
            ddlSec.Enabled = false;
            GetTest();
        }
    }
    public void bindsem()
    {

        //--------------------semester load
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["InternalCollegeCode"] + "", con);
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            first_year = Convert.ToBoolean(dr[1].ToString());
            duration = Convert.ToInt16(dr[0].ToString());
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
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["InternalCollegeCode"] + "", con);
            ddlSemYr.Items.Clear();
            dr1 = cmd.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr1[1].ToString());
                duration = Convert.ToInt16(dr1[0].ToString());

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

            dr1.Close();
        }
        //FpMarkEntry.Visible = false;
        con.Close();
    }
    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        //int typeval = 4;

        string batch = ddlBatch.SelectedValue.ToString();
        string collegecode = Session["InternalCollegeCode"].ToString();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        //Session["InternalCollegeCode"].ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
        //ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
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

    public string result(string st)
    {
        con.Close();
        con.Open();
        string result = "";
        SqlDataReader drr;
        SqlCommand commmand = new SqlCommand(st, con);
        drr = commmand.ExecuteReader();


        if (drr.HasRows == true)
        {
            while (drr.Read())
            {
                if (drr[0] != null)
                {
                    result = drr[0].ToString();
                }
                else
                {
                    result = "0";
                }
            }
        }
        else if (drr.HasRows == false)
        {
            result = "";
        }

        return result;
    }
    public double roundresult(string nstr)
    {
        con.Close();
        con.Open();
        double roundresult;
        if ((nstr) != "")
        {

            double ag1;
            ag1 = Convert.ToDouble(Math.Round(Convert.ToDecimal(nstr), 2));

            roundresult = ag1;
        }
        else
        {
            roundresult = 0;
        }
        return roundresult;
    }
    //    //*****
    //private string Splitter(string p, string p_2)
    //{
    //    throw new NotImplementedException();
    //}







    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        FpEntry.Visible = false;
        FpSpread1.Visible = false;
        Button2.Visible = false;
        //lblEduration.Visible = false;
        lblnorec.Visible = false;
        FpSpread3.Visible = false;
        //ddlTest.Items.Clear();
        btnExcel.Visible = false;
        BtnPrint.Visible = false;
        //Added By Srinath 28/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        if (!Page.IsPostBack == false)
        {
            //ddlSec.Items.Clear();
        }
        bindsec();
        GetTest();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        buttonG0();
    }

    protected void BtnPrint_Click(object sender, EventArgs e)
    {
        overallperformprint1();
    }
    protected void buttonG0()
    {
        //sankar edit......May'30........................................
        FpSpread1.Visible = true;
        FpEntry.Sheets[0].RowCount = 0;
        TextBoxother.Visible = false;
        TextBoxother.Text = "";
        TextBoxpage.Text = "";
        FpEntry.CurrentPage = 0;




        if ((ddlTest.Text != "--Select--") && (ddlTest.Text.Trim() != ""))
        {
            //lblEtest.Visible = false;
            lblnorec.Visible = false;


            if ((ddlSec.Enabled == true && ddlSec.Text != "-1") || (ddlSec.Enabled == false))
            {

                overallperformprint();

            }



        }
        //Added by subburaj 04/09/2014*********//
        else
        {
            lblnorec.Text = "Please Select Any one Test";
            lblnorec.Visible = true;
            FpSpread1.Visible = false;
            btnExcel.Visible = false;
            BtnPrint.Visible = false;

            lblrptname.Visible = false;
            txtexcelname.Visible = false;
        }
        //*************End*********************//
    }
    protected void ddlTest_SelectedIndexChanged1(object sender, EventArgs e)
    {
        FpSpread3.Visible = false;

        ///  buttonG0(); //05.04.12

    }
    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetTest();
        btnExcel.Visible = false;
        BtnPrint.Visible = false;
        //Added By Srinath 28/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        lblnorec.Visible = false;
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
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
        //Added By Srinath 28/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        con.Open();

        string collegecode = Session["InternalCollegeCode"].ToString();
        string usercode = Session["usercode"].ToString();
        //binddegree();
        if (ddlDegree.Text != "")
        {
            //bindbranch();

            //bindsem();

            //bindsec();

            GetTest();
            lblnorec.Visible = false;
            ddlDegree.SelectedIndex = 0;
            ddlBranch.SelectedIndex = 0;
        }
        else
        {
            lblnorec.Text = "Give degree rights to the staff";
            lblnorec.Visible = true;
        }
        bindbranch();
        binddegree();
        bindsem();
        bindsec();
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
        string sqlstr;
        sqlstr = Att_strqueryst;
        mycon1.Close();
        mycon1.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
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

        if ((ddlTest.SelectedIndex != 0) && (ddlTest.Text != ""))
        {
            overallperformprint();
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
    }
    protected void LinkButton4_Click(object sender, EventArgs e)
    {
    }

    int subjectcount = 0;
    int topstud = 3;
    string subno = "";
    string subject_code = "";
    string resminmrk = "";
    string exam_code = "";
    string examdate;
    string subname = "";
    int substcount;
    int totalstcount;
    int resultstcount;
    int photostcount;
    //subj_bind
    string subj_code = "";
    int sno = 0;
    string srno = "";
    int sno2 = 0;
    string srno2 = "";
    int snotb3 = 0;
    string srnotb3 = "";
    string test = "";
    string Rank = "";
    string stude_RollNumber = "";
    string Pertc = "";
    string Total_Mark = "";
    string sub_code = "";
    string stud_Nameof = "";
    string mark_obt = "";
    string table2_Roll_No = "";
    string table2_Stud_Name = "";
    string table2_Subj_code = "";
    string table2_Mark = "";
    int total_pass_fail = 0;
    string table3_subj_code = "";
    string table3_subj_name = "";
    string table3_staff_inc = "";
    string table3_Pass = "";
    string table4_Avg = "";

    protected void overallperformprint1()
    {
        Font Fontbold = new Font("Times New Roman", 20, FontStyle.Bold);
        Font Fontsmall = new Font("Times New Roman", 18, FontStyle.Regular);
        Font Fontbold1 = new Font("Times New Roman", 14, FontStyle.Bold);
        Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
        Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
        btnExcel.Visible = true;
        BtnPrint.Visible = true;
        lblrptname.Visible = true;
        txtexcelname.Visible = true;

        if (txttop.Text != "")
        {
            topstud = Convert.ToInt16(txttop.Text);
        }
        else
        {
            topstud = 3;
        }

        if (Session["InternalCollegeCode"].ToString() != null && Session["InternalCollegeCode"].ToString() != "")
        {
            string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["InternalCollegeCode"] + "";
            SqlCommand collegecmd = new SqlCommand(college, con);
            SqlDataReader collegename;
            con.Close();
            con.Open();
            collegename = collegecmd.ExecuteReader();
            if (collegename.HasRows)
            {

                while (collegename.Read())
                {
                    collnamenew1 = collegename["collname"].ToString();
                    address1 = collegename["address1"].ToString();
                    address2 = collegename["address2"].ToString();
                    district = collegename["district"].ToString();
                    address = address1 + "-" + address2 + "-" + district;
                    Phoneno = collegename["phoneno"].ToString();
                    Faxno = collegename["faxno"].ToString();
                    phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno + ".";
                    email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
                }
            }
            con.Close();
        }

        string branch = "0";
        string degree = "0";
        if (ddlBranch.Items.Count > 0)
            branch = ddlBranch.SelectedItem.Text;
        if (ddlDegree.Items.Count > 0)
            degree = ddlDegree.SelectedItem.Text;


        string sem = ddlSemYr.SelectedValue;
        string sec = ddlSec.SelectedValue;
        test = ddlTest.SelectedItem.Text;
        filteration();
        string filterwithsection = "a.app_no=r.app_no and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0   and r.sections='" + ddlSec.SelectedValue.ToString() + "' " + strorder + ",s.subject_no";
        string filterwithoutsection = "a.app_no=r.app_no and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strorder + ",s.subject_no";

        hat.Clear();
        hat.Add("batchyear", ddlBatch.SelectedValue.ToString());
        hat.Add("degreecode", ddlBranch.SelectedValue.ToString());
        hat.Add("criteria_no", ddlTest.SelectedValue.ToString());
        hat.Add("sections", ddlSec.SelectedValue.ToString());
        hat.Add("filterwithsection", filterwithsection.ToString());
        hat.Add("filterwithoutsection", filterwithoutsection.ToString());
        ds2.Clear();
        ds2.Reset();
        ds2 = daccess2.select_method("PROC_STUD_ALL_SUBMARK", hat, "sp");
        string sqlStr = "";
        string sections = "";
        string strsec = "";

        double find_total = 0;
        int sum_max_mark = 0;
        double percent = 0;
        int fail_sub_cnt = 0;
        int ra_nk = 0;
        sections = ddlSec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
        {
            strsec = "";
        }
        else
        {
            strsec = " and registration.sections='" + sections.ToString() + "'";
        }

        ds4.Clear();
        ds4.Reset();
        ds4 = daccess2.select_method_wo_parameter("Delete_Rank_Table", "sp");
        if (ds2.Tables[1].Rows.Count == 0)
        {
            lblerror.Visible = true;
            lblerror.Text = "There is no record found";
            FpSpread1.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            BtnPrint.Visible = false;
            return;
        }

        int sub_code_new = 0;
        if (ds2.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
            {

                if (subject_code == "")
                {
                    sub_code_new++;
                    subno = ds2.Tables[1].Rows[i]["subject_no"].ToString();
                    subject_code = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                    //resmaxmrk = ds2.Tables[1].Rows[i]["max_mark"].ToString();
                    resminmrk = ds2.Tables[1].Rows[i]["min_mark"].ToString();
                    //resduration = ds2.Tables[1].Rows[i]["duration"].ToString();
                    exam_code = ds2.Tables[1].Rows[i]["exam_code"].ToString();
                    examdate = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                    subname = ds2.Tables[1].Rows[i]["subject_name"].ToString();

                    subj_code = subject_code;
                    sub_code = sub_code_new.ToString();
                }
                else
                {
                    subj_code = subj_code + '\n' + ds2.Tables[1].Rows[i]["subject_code"].ToString();
                    sub_code = sub_code + '\n' + sub_code_new.ToString();
                }
            }

            //sqlStr = "select distinct len(registration.Roll_No),registration.Roll_No as roll,registration.Reg_No as regno,registration.stud_name as studname,registration.stud_type as studtype,registration.App_No as ApplicationNumber from registration, applyn a,exam_type et,result rt where a.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0   " + strsec + " and  rt.exam_code=et.exam_code and registration.roll_no=rt.roll_no and et.criteria_no =" + ddlTest.SelectedValue.ToString() + " order by  len(registration.Roll_No),roll";
            sqlStr = "select distinct len(registration.Roll_No),registration.Roll_No as roll,registration.Reg_No as regno,registration.stud_name as studname,registration.stud_type as studtype,registration.App_No as ApplicationNumber from registration, applyn a,exam_type et,result rt where a.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0   " + strsec + " and  rt.exam_code=et.exam_code and registration.roll_no=rt.roll_no and et.criteria_no =" + ddlTest.SelectedValue.ToString() + " " + strregorder + " ";
            con.Close();
            con.Open();
            if (sqlStr != "")
            {
                SqlDataAdapter adaSyll1 = new SqlDataAdapter(sqlStr, con);
                ds1.Clear();
                ds1.Reset();
                adaSyll1.Fill(ds1);
                //   FpEntry.DataBind();
                int subrow = 0;
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds1.Tables[0].Rows.Count; row++)
                    {
                        // subrow = 1;
                        fail_sub_cnt = 0;
                        find_total = 0;
                        sum_max_mark = 0;
                        for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
                        {
                            if (subrow < Convert.ToInt32(ds2.Tables[0].Rows.Count))
                            {
                                if (ds1.Tables[0].Rows[row]["roll"].ToString() == ds2.Tables[0].Rows[subrow]["roll"].ToString())
                                {

                                    if (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) != -2 && double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) != -3 && (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) < double.Parse(ds2.Tables[1].Rows[j]["min_mark"].ToString())))
                                    {
                                        fail_sub_cnt++;
                                    }
                                    if (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) >= 0 && (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) >= Convert.ToInt32(ds2.Tables[1].Rows[j]["min_mark"].ToString())))
                                    {
                                        //'---------------total,percent,rank-------------------------------
                                        find_total = (Convert.ToDouble(find_total) + Convert.ToDouble(ds2.Tables[0].Rows[subrow]["mark"].ToString()));
                                        sum_max_mark = sum_max_mark + Convert.ToInt32(ds2.Tables[1].Rows[j]["max_mark"].ToString());
                                        percent = Convert.ToDouble((Convert.ToDouble(find_total) / sum_max_mark) * 100);
                                    }
                                }
                            }
                            subrow++;

                        }
                        if (fail_sub_cnt == 0)
                        {
                            hat.Clear();
                            hat.Add("RollNumber", ds1.Tables[0].Rows[row]["roll"].ToString());
                            hat.Add("criteria_no", ddlTest.SelectedValue.ToString());
                            hat.Add("Total", find_total.ToString());
                            hat.Add("avg", percent.ToString());
                            hat.Add("rank", "");
                            int o = daccess2.insert_method("INSERT_RANK", hat, "sp");
                        }
                        //'--------------------------------------------------------------


                    }
                    //'--------------------------------insert the rank---------------------------------
                    ra_nk = 1;
                    ds3.Clear();
                    ds3.Reset();
                    ds3 = daccess2.select_method_wo_parameter("SELECT_RANK", "sp");

                    if (ds3.Tables[0].Rows.Count != 0)
                    {
                        double temp_rank = 0;
                        int zx = 1;
                        for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
                        {
                            if (temp_rank == 0)
                            {
                                ra_nk = 1;
                                hat.Clear();
                                hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                hat.Add("criteria_no", ddlBranch.SelectedValue.ToString());
                                hat.Add("Total", Convert.ToString(find_total));
                                hat.Add("avg", Convert.ToString(percent));
                                hat.Add("rank", ra_nk.ToString());
                                int o = daccess2.insert_method("INSERT_RANK", hat, "sp");

                                temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                            }
                            else if (temp_rank != 0)
                            {
                                if (temp_rank > double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                                {
                                    //   ra_nk += 1;
                                    ra_nk = zx;
                                    hat.Clear();
                                    hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                    hat.Add("criteria_no", ddlBranch.SelectedValue.ToString());
                                    hat.Add("Total", Convert.ToString(find_total));
                                    hat.Add("avg", Convert.ToString(percent)); ;
                                    hat.Add("rank", ra_nk.ToString());
                                    int o = daccess2.insert_method("INSERT_RANK", hat, "sp");

                                    temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());

                                }
                                else if (temp_rank == double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                                {

                                    hat.Clear();
                                    hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                    hat.Add("criteria_no", ddlBranch.SelectedValue.ToString());
                                    hat.Add("Total", Convert.ToString(find_total));
                                    hat.Add("avg", Convert.ToString(percent));
                                    hat.Add("rank", ra_nk.ToString());
                                    int o = daccess2.insert_method("INSERT_RANK", hat, "sp");
                                    temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                                }
                            }
                            zx++;
                        }
                        //---------------new end 030412

                    }
                }
            }


            string strsection;
            string subsec = ddlSec.SelectedValue.ToString();
            if (subsec.ToString() == "All" || subsec.ToString() == "" || subsec.ToString() == "-1")
            {
                strsection = "";
            }
            else
            {
                strsection = " and R.sections='" + subsec.ToString() + "'";
            }


            if (ds3.Tables[0].Rows.Count > 0)
            {
                if (ds3.Tables[0].Rows.Count != 0)
                {
                    ds3.Clear();
                    ds3.Reset();
                    ds3 = daccess2.select_method_wo_parameter("SELECT_RANK", "sp");
                    int rank_row_count = 0;
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        if (rank_row_count < ds3.Tables[0].Rows.Count)
                        {
                            if (Convert.ToInt32(ds3.Tables[0].Rows[rank_row_count]["Rank"].ToString()) <= topstud)
                            {
                                sno++;
                                string roll_no = ds3.Tables[0].Rows[rank_row_count]["Rollno"].ToString();

                                if (Rank == "")
                                {
                                    Rank = ds3.Tables[0].Rows[rank_row_count]["Rank"].ToString();
                                    stude_RollNumber = ds3.Tables[0].Rows[rank_row_count]["Rollno"].ToString();

                                    srno = sno.ToString();
                                    //string sbno = FpSpread1.Sheets[0].Cells[9, j].Tag.ToString();
                                    sqlStr = "SELECT Marks_Obtained FROM Registration R,Result U,Exam_Type E,Subject S ";
                                    sqlStr = sqlStr + "WHERE R.Roll_No = U.Roll_No AND U.Exam_Code = E.Exam_Code AND E.Subject_No = S.Subject_No ";
                                    sqlStr = sqlStr + "AND R.Degree_Code =" + ddlBranch.SelectedValue.ToString() + " AND R.Batch_year =" + ddlBatch.SelectedValue.ToString() + " AND E.Criteria_No =" + ddlTest.SelectedValue.ToString() + strsection;
                                    sqlStr = sqlStr + "AND RollNo_Flag <> 0 AND CC = 0 AND Exam_Flag <> 'DEBAR' AND DelFlag = 0 ";
                                    sqlStr = sqlStr + " AND U.Roll_No ='" + roll_no + "' ";
                                    sqlStr = sqlStr + "ORDER BY S.Subject_No ";

                                    con.Close();
                                    con.Open();
                                    SqlCommand markcmd = new SqlCommand(sqlStr, con);
                                    SqlDataReader markds;
                                    markds = markcmd.ExecuteReader();
                                    if (markds.HasRows)
                                    {
                                        while (markds.Read())
                                        {
                                            if (mark_obt == "")
                                            {
                                                mark_obt = markds["Marks_Obtained"].ToString();
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    Rank = Rank + '\n' + ds3.Tables[0].Rows[rank_row_count]["Rank"].ToString();
                                    stude_RollNumber = stude_RollNumber + '\n' + ds3.Tables[0].Rows[rank_row_count]["Rollno"].ToString();
                                    srno = srno + '\n' + sno.ToString();

                                    sqlStr = "SELECT Marks_Obtained FROM Registration R,Result U,Exam_Type E,Subject S ";
                                    sqlStr = sqlStr + "WHERE R.Roll_No = U.Roll_No AND U.Exam_Code = E.Exam_Code AND E.Subject_No = S.Subject_No ";
                                    sqlStr = sqlStr + "AND R.Degree_Code =" + ddlBranch.SelectedValue.ToString() + " AND R.Batch_year =" + ddlBatch.SelectedValue.ToString() + " AND E.Criteria_No =" + ddlTest.SelectedValue.ToString() + strsection;
                                    sqlStr = sqlStr + "AND RollNo_Flag <> 0 AND CC = 0 AND Exam_Flag <> 'DEBAR' AND DelFlag = 0 ";
                                    sqlStr = sqlStr + " AND U.Roll_No ='" + roll_no + "' ";
                                    sqlStr = sqlStr + "ORDER BY S.Subject_No ";

                                    con.Close();
                                    con.Open();
                                    SqlCommand markcmd = new SqlCommand(sqlStr, con);
                                    SqlDataReader markds;
                                    markds = markcmd.ExecuteReader();
                                    if (markds.HasRows)
                                    {
                                        if (markds.Read())
                                        {
                                            if (mark_obt != "")
                                            {
                                                mark_obt = mark_obt + '\n' + markds["Marks_Obtained"].ToString();
                                            }
                                        }

                                    }

                                }


                                if (Pertc == "")
                                {
                                    Pertc = Math.Round(Convert.ToDouble(ds3.Tables[0].Rows[rank_row_count]["perc"].ToString()), 2).ToString();
                                    Total_Mark = Math.Round(Convert.ToDouble(ds3.Tables[0].Rows[rank_row_count]["Total"].ToString()), 2).ToString();
                                }
                                else
                                {
                                    Pertc = Pertc + '\n' + Math.Round(Convert.ToDouble(ds3.Tables[0].Rows[rank_row_count]["perc"].ToString()), 2).ToString();
                                    Total_Mark = Total_Mark + '\n' + Math.Round(Convert.ToDouble(ds3.Tables[0].Rows[rank_row_count]["Total"].ToString()), 2).ToString();
                                }


                                string fstname = "select registration.stud_name from registration where roll_no='" + ds3.Tables[0].Rows[rank_row_count]["Rollno"].ToString() + "'";
                                SqlCommand credit = new SqlCommand(fstname, con);
                                con.Close();
                                con.Open();
                                SqlDataReader studname;
                                studname = credit.ExecuteReader();
                                if (studname.HasRows)
                                {
                                    while (studname.Read())
                                    {

                                        if (stud_Nameof == "")
                                        {
                                            stud_Nameof = studname["stud_name"].ToString();
                                        }
                                        else
                                        {
                                            stud_Nameof = stud_Nameof + '\n' + studname["stud_name"].ToString();
                                        }
                                    }
                                }




                            }
                            rank_row_count++;
                        }
                    }
                }
            }
        }


        //table2 bind coding........................

        for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
        {
            hat.Clear();
            string secss = "";
            if (ddlSec.Enabled == false)
            {
                secss = "";
            }
            else
            {
                secss = ddlSec.SelectedItem.Text.ToString();
            }
            hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
            hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
            hat.Add("section", secss);
            ds4.Clear();
            ds4.Reset();
            ds4 = daccess2.select_method("Proc_All_Subject_Details", hat, "sp");

            if (ds4.Tables[10].Rows.Count > 0)
            {
                for (int dsrow = 0; dsrow < ds4.Tables[10].Rows.Count; dsrow++)
                {
                    sno2++;
                    if (table2_Roll_No == "")
                    {
                        table2_Roll_No = ds4.Tables[10].Rows[dsrow]["ROLL_NO"].ToString();
                        table2_Stud_Name = ds4.Tables[10].Rows[dsrow]["STUD_NAME"].ToString();
                        table2_Subj_code = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        table2_Mark = ds4.Tables[3].Rows[0]["MAX_MARK"].ToString();
                        srno2 = sno2.ToString();
                    }
                    else
                    {
                        table2_Roll_No = table2_Roll_No + '\n' + ds4.Tables[10].Rows[dsrow]["ROLL_NO"].ToString();
                        table2_Stud_Name = table2_Stud_Name + '\n' + ds4.Tables[10].Rows[dsrow]["STUD_NAME"].ToString();
                        table2_Subj_code = table2_Subj_code + '\n' + ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        table2_Mark = table2_Mark + '\n' + ds4.Tables[3].Rows[0]["MAX_MARK"].ToString();
                        srno2 = srno2 + '\n' + sno2.ToString();

                    }

                }
            }
        }



        //table3..................bind
        for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
        {
            snotb3++;
            hat.Clear();
            string secss = "";
            if (ddlSec.Enabled == false) // added by sridhar aug 2014
            {
                secss = "";
            }
            else
            {
                secss = ddlSec.SelectedItem.Text.ToString();
            }

            if (secss.ToString().Trim() == "-1" || secss.ToString().Trim() == "" || secss.ToString().Trim() == null)
            {
                secss = "";  // added by sridhar aug 2014
            }
            else
            {
                secss = ddlSec.SelectedItem.Text.ToString(); // added by sridhar aug 2014
            }
            hat.Add("exam_code", ds2.Tables[1].Rows[j]["exam_code"].ToString());
            hat.Add("min_marks", ds2.Tables[1].Rows[j]["min_mark"].ToString());
            hat.Add("section", secss);
            ds4.Clear();
            ds4.Reset();
            ds4 = daccess2.select_method("Proc_All_Subject_Details", hat, "sp");

            if (ds2.Tables[1].Rows.Count > 0)
            {
                if (table3_subj_code == "")
                {
                    table3_subj_code = ds2.Tables[1].Rows[j]["subject_code"].ToString();
                    table3_subj_name = ds2.Tables[1].Rows[j]["subject_name"].ToString();
                    total_pass_fail = Convert.ToInt32(ds4.Tables[1].Rows[0]["PASS_COUNT"]) + Convert.ToInt32(ds4.Tables[2].Rows[0]["FAIL_COUNT"]);
                    double cal_avg = Convert.ToDouble(ds4.Tables[0].Rows[0]["SUM"]) / Convert.ToDouble(total_pass_fail);
                    cal_avg = Math.Round(cal_avg, 2);
                    double pass_perc = (Convert.ToDouble(ds4.Tables[1].Rows[0]["PASS_COUNT"]) / Convert.ToDouble(total_pass_fail)) * 100;
                    pass_perc = Math.Round(pass_perc, 2);
                    table3_Pass = pass_perc.ToString();
                    table4_Avg = cal_avg.ToString();
                    if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                    {
                        strsec = "";
                    }
                    else
                    {
                        strsec = " and exam_type.sections='" + sections.ToString() + "'";
                    }
                    string temp = "";
                    if ((ds2.Tables[1].Rows[j]["subject_no"].ToString() != "") && (ddlTest.SelectedValue.ToString() != ""))
                    {
                        temp = GetFunction("Select distinct staff_code from result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subject_no =" + ds2.Tables[1].Rows[j]["subject_no"].ToString() + " " + strsec + " and exam_type.criteria_no =" + ddlTest.SelectedValue.ToString() + "");
                        if (temp != "")
                        {
                            staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                        }
                        table3_staff_inc = staff;

                    }

                    srnotb3 = snotb3.ToString();

                }
                else
                {
                    table3_subj_code = table3_subj_code + '\n' + ds2.Tables[1].Rows[j]["subject_code"].ToString();
                    table3_subj_name = table3_subj_name + '\n' + ds2.Tables[1].Rows[j]["subject_name"].ToString();
                    total_pass_fail = Convert.ToInt32(ds4.Tables[1].Rows[0]["PASS_COUNT"]) + Convert.ToInt32(ds4.Tables[2].Rows[0]["FAIL_COUNT"]);
                    double cal_avg = Convert.ToDouble(ds4.Tables[0].Rows[0]["SUM"]) / Convert.ToDouble(total_pass_fail);
                    cal_avg = Math.Round(cal_avg, 2);
                    double pass_perc = (Convert.ToDouble(ds4.Tables[1].Rows[0]["PASS_COUNT"]) / Convert.ToDouble(total_pass_fail)) * 100;
                    pass_perc = Math.Round(pass_perc, 2);
                    table3_Pass = table3_Pass + '\n' + pass_perc.ToString();
                    table4_Avg = table4_Avg + '\n' + cal_avg.ToString();
                    if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                    {
                        strsec = "";
                    }
                    else
                    {
                        strsec = " and exam_type.sections='" + sections.ToString() + "'";
                    }
                    string temp = "";
                    if ((ds2.Tables[1].Rows[j]["subject_no"].ToString() != "") && (ddlTest.SelectedValue.ToString() != ""))
                    {
                        temp = GetFunction("Select distinct staff_code from result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subject_no =" + ds2.Tables[1].Rows[j]["subject_no"].ToString() + " " + strsec + " and exam_type.criteria_no =" + ddlTest.SelectedValue.ToString() + "");
                        if (temp != "")
                        {
                            staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                        }
                        table3_staff_inc = table3_staff_inc + '\n' + staff;

                    }

                    srnotb3 = srnotb3 + '\n' + snotb3.ToString();


                }
            }

        }



        generateletterformat1(mydocument, Fontsmall, Fontbold, Fontbold1, ds2.Tables[1], Response);
    }

    public void filteration()
    {

        string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");

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


    protected void overallperformprint()
    {
        //try
        //{
        try
        {
            btnExcel.Visible = true;
            BtnPrint.Visible = true;
            //Added By Srinath 28/2/2013
            lblrptname.Visible = true;
            txtexcelname.Visible = true;

            string collnamenew1 = "";
            string address1 = "";
            string address2 = "";
            string address = "";
            string Phoneno = "";
            string Faxno = "";
            string phnfax = "";
            int subjectcount = 0;
            string district = "";
            string email = "";
            string website = "";
            int topstud = 3;

            string subno;
            string subject_code;
            string resminmrk;
            string exam_code;
            string examdate;
            string subname;
            int substcount;
            int totalstcount;
            int resultstcount;
            int photostcount;

            if (txttop.Text != "")
            {
                topstud = Convert.ToInt16(txttop.Text);
            }
            else
            {
                topstud = 3;
            }
            if (Session["InternalCollegeCode"].ToString() != null && Session["InternalCollegeCode"].ToString() != "")
            {
                string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["InternalCollegeCode"] + "";
                SqlCommand collegecmd = new SqlCommand(college, con);
                SqlDataReader collegename;
                con.Close();
                con.Open();
                collegename = collegecmd.ExecuteReader();
                if (collegename.HasRows)
                {

                    while (collegename.Read())
                    {
                        collnamenew1 = collegename["collname"].ToString();
                        address1 = collegename["address1"].ToString();
                        address2 = collegename["address2"].ToString();
                        district = collegename["district"].ToString();
                        address = address1 + "-" + address2 + "-" + district;
                        Phoneno = collegename["phoneno"].ToString();
                        Faxno = collegename["faxno"].ToString();
                        phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno + ".";
                        email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
                    }
                }
                con.Close();
            }

            Session["rank1roll"] = "";
            FpSpread1.Sheets[0].ColumnHeader.Visible = false;

            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.CommandBar.Visible = true;
            FpSpread1.Sheets[0].PageSize = 60;
            FpSpread1.Sheets[0].AutoPostBack = true;

            FpSpread1.Pager.Align = HorizontalAlign.Right;
            FpSpread1.Pager.Font.Bold = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antique";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;

            FpSpread1.Sheets[0].ColumnCount = 3;
            FpSpread1.Sheets[0].RowCount = 10;//rwcnt=7- 02.03.12
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].Columns[0].Width = 70;
            FpSpread1.Sheets[0].Columns[1].Width = 230;
            FpSpread1.Sheets[0].Columns[2].Width = 70;

            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;

            FpSpread1.Sheets[0].SpanModel.Add(0, 1, 1, 3);
            FpSpread1.Sheets[0].SpanModel.Add(1, 1, 1, 3);
            FpSpread1.Sheets[0].SpanModel.Add(2, 1, 1, 3);
            ////FpSpread1.Sheets[0].SpanModel.Add(3, 1, 1, 3);
            ////FpSpread1.Sheets[0].SpanModel.Add(4, 0, 1, 5);
            ////FpSpread1.Sheets[0].SpanModel.Add(5, 0, 1, 5);
            ////FpSpread1.Sheets[0].SpanModel.Add(6, 0, 1, 5);
            ////FpSpread1.Sheets[0].SpanModel.Add(7, 0, 1, 5);
            ////FpSpread1.Sheets[0].SpanModel.Add(8, 0, 1, 5);


            string branch = "0";
            string degree = "0";
            if (ddlBranch.Items.Count > 0)
                branch = ddlBranch.SelectedItem.Text;
            if (ddlDegree.Items.Count > 0)
                degree = ddlDegree.SelectedItem.Text;


            string sem = ddlSemYr.SelectedValue;
            string sec = ddlSec.SelectedValue;
            string test = ddlTest.SelectedItem.Text;

            ////'---------------------------------------------load theclg logo photo-------------------------------------
            //FpSpread1.Sheets[0].SpanModel.Add(0, 0, 4, 1);
            FpSpread1.Sheets[0].SpanModel.Add(0, 0, 7, 1);
            FpSpread1.Sheets[0].Columns[0].Width = 150;
            MyImg2 mi3 = new MyImg2();
            mi3.ImageUrl = "Handler/Handler2.ashx?";



            FpSpread1.Sheets[0].Cells[0, 0].CellType = mi3;
            FpSpread1.Sheets[0].Cells[0, 0].Border.BorderColorLeft = Color.Black;
            FpSpread1.Sheets[0].Cells[0, 0].Border.BorderColorRight = Color.White;
            FpSpread1.Sheets[0].Cells[0, 0].Border.BorderColorBottom = Color.White;
            FpSpread1.Sheets[0].Cells[4, 0].Border.BorderColorLeft = Color.Black;
            FpSpread1.Sheets[0].Cells[4, 0].Border.BorderColorBottom = Color.White;
            //'------------------span the 3 rows to display the img----------------

            //FpSpread1.Sheets[0].SpanModel.Add(0, 4, 4, 1);
            //MyImg3 mi4 = new MyImg3();
            //mi4.ImageUrl = "Handler/Handler5.ashx?";
            //FpSpread1.Sheets[0].Cells[0, 4].CellType = mi4;

            FpSpread1.Sheets[0].Cells[0, 1].Text = collnamenew1;
            FpSpread1.Sheets[0].Cells[1, 1].Text = address;
            FpSpread1.Sheets[0].Cells[2, 1].Text = phnfax;
            FpSpread1.Sheets[0].Cells[3, 1].Text = email;
            FpSpread1.Sheets[0].Cells[4, 1].Text = "OVERALL BEST PERFORMANCE";
            if (Label4.Text.Trim().ToLower() == "school")
            {
                FpSpread1.Sheets[0].Cells[5, 1].Text = "Year:  " + ddlBatch.SelectedValue.ToString() + " " + "Standard: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
                FpSpread1.Sheets[0].Cells[6, 1].Text = "Test: " + test + " " + "Term: " + ddlSemYr.SelectedValue.ToString() + "Section: " + ddlSec.SelectedValue.ToString();
            }
            else
            {
                FpSpread1.Sheets[0].Cells[5, 1].Text = "Batch Year:  " + ddlBatch.SelectedValue.ToString() + " " + "Course: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
                FpSpread1.Sheets[0].Cells[6, 1].Text = "Test: " + test + " " + "Semester: " + ddlSemYr.SelectedValue.ToString() + "Section: " + ddlSec.SelectedValue.ToString();
            }
            //   FpSpread1.Sheets[0].Cells[7, 0].Text = 


            FpSpread1.Sheets[0].Rows[4].Font.Bold = true;
            FpSpread1.Sheets[0].Rows[0].Font.Bold = true;
            FpSpread1.Sheets[0].Rows[1].Font.Bold = true;
            FpSpread1.Sheets[0].Rows[2].Font.Bold = true;
            FpSpread1.Sheets[0].Rows[3].Font.Bold = true;
            FpSpread1.Sheets[0].Rows[5].Font.Bold = true;
            FpSpread1.Sheets[0].Rows[9].Font.Bold = true;

            //FpSpread1.Sheets[0].Rows[5].Font.Underline = true;
            FpSpread1.Sheets[0].Rows[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Rows[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Rows[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Rows[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Rows[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Rows[5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Rows[6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Rows[7].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].Rows[6].Font.Bold = true;

            FpSpread1.Sheets[0].Rows[0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Rows[1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Rows[2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Rows[3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Rows[9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Rows[4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Rows[5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Rows[7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Rows[6].Font.Size = FontUnit.Medium;


            FpSpread1.Sheets[0].Rows[0].Border.BorderColor = Color.White;
            FpSpread1.Sheets[0].Rows[1].Border.BorderColor = Color.White;
            FpSpread1.Sheets[0].Rows[2].Border.BorderColor = Color.White;
            FpSpread1.Sheets[0].Rows[3].Border.BorderColor = Color.White;
            FpSpread1.Sheets[0].Rows[4].Border.BorderColor = Color.White;
            FpSpread1.Sheets[0].Rows[5].Border.BorderColor = Color.White;
            FpSpread1.Sheets[0].Rows[6].Border.BorderColor = Color.White;
            FpSpread1.Sheets[0].Rows[7].Border.BorderColor = Color.White;
            FpSpread1.Sheets[0].Rows[9].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].Cells[9, 0].Border.BorderColorLeft = Color.Black;
            FpSpread1.Sheets[0].Rows[0].Border.BorderColorTop = Color.Black;

            FpSpread1.Sheets[0].Cells[9, 0].Text = "Rank";
            FpSpread1.Sheets[0].Cells[9, 1].Text = "Roll No";


            FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();
            FpSpread1.Sheets[0].Columns[0].CellType = txttype;
            FpSpread1.Sheets[0].Columns[1].CellType = txttype;

            FpSpread1.Sheets[0].Cells[9, 2].Text = "Student Name";
            //FpSpread1.Sheets[0].Cells[9, 3].Text = "Marks";
            //   FpSpread1.Sheets[0].SpanModel.Add(6,3,1,2);
            //'-------------------------------------------------------mythili start----------------------------------------'
            //'-------------------------------------------- Query for Get the subjectno,sub code,acronym ,examdate,minmrk,maxmrk,entrydate and examcode
            filteration();
            string filterwithsection = "a.app_no=r.app_no and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and et.sections=r.sections and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0   and r.sections='" + ddlSec.SelectedValue.ToString() + "' " + strorder + ",s.subject_no";
            string filterwithoutsection = "a.app_no=r.app_no and r.degree_code='" + ddlBranch.SelectedValue.ToString() + "' and et.subject_no=s.subject_no and et.sections=r.sections and r.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and RollNo_Flag<>0 and et.exam_code=rt.exam_code and et.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strorder + ",s.subject_no";
            hat.Clear();
            hat.Add("batchyear", ddlBatch.SelectedValue.ToString());
            hat.Add("degreecode", ddlBranch.SelectedValue.ToString());
            hat.Add("criteria_no", ddlTest.SelectedValue.ToString());
            hat.Add("sections", ddlSec.SelectedValue.ToString());

            hat.Add("filterwithsection", filterwithsection.ToString());
            hat.Add("filterwithoutsection", filterwithoutsection.ToString());

            ds2.Clear();
            ds2.Reset();
            ds2 = daccess2.select_method("PROC_STUD_ALL_SUBMARK", hat, "sp");
            string sqlStr = "";
            string sections = "";
            string strsec = "";

            double find_total = 0;
            int sum_max_mark = 0;
            double percent = 0;
            int fail_sub_cnt = 0;
            int ra_nk = 0;
            sections = ddlSec.SelectedValue.ToString();
            if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and registration.sections='" + sections.ToString() + "'";
            }

            ds4.Clear();
            ds4.Reset();
            ds4 = daccess2.select_method_wo_parameter("Delete_Rank_Table", "sp");
            if (ds2.Tables[1].Rows.Count == 0)
            {
                lblerror.Visible = true;
                lblerror.Text = "There is no record found";
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnExcel.Visible = false;
                BtnPrint.Visible = false;
                return;
            }
           
            if (ds2.Tables[0].Rows.Count > 0)
            {

                //to display subject details



                for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                {
                   
                            subno = ds2.Tables[1].Rows[i]["subject_no"].ToString();
                            subject_code = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                            //resmaxmrk = ds2.Tables[1].Rows[i]["max_mark"].ToString();
                            resminmrk = ds2.Tables[1].Rows[i]["min_mark"].ToString();
                            //resduration = ds2.Tables[1].Rows[i]["duration"].ToString();
                            exam_code = ds2.Tables[1].Rows[i]["exam_code"].ToString();
                            examdate = ds2.Tables[1].Rows[i]["exam_date"].ToString();
                            subname = ds2.Tables[1].Rows[i]["subject_name"].ToString();

                            FpSpread1.Sheets[0].ColumnCount = Convert.ToInt32(FpSpread1.Sheets[0].ColumnCount) + 1;
                            substcount = FpSpread1.Sheets[0].ColumnCount - 1;
                            int incr = FpSpread1.Sheets[0].ColumnCount - 1;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, incr].Tag = examdate + "@" + exam_code;

                            FpSpread1.Sheets[0].Cells[9, incr].Text = subject_code;
                            FpSpread1.Sheets[0].Cells[9, incr].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[0, incr].Note = subno + "@" + subname + "@" + subject_code;
                            FpSpread1.Sheets[0].Cells[9, incr].Tag = subno;
                        }
                    
                
                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 2;
                totalstcount = FpSpread1.Sheets[0].ColumnCount - 2;
                FpSpread1.Sheets[0].Cells[9, totalstcount].Text = "Total";
                FpSpread1.Sheets[0].Cells[9, totalstcount].HorizontalAlign = HorizontalAlign.Center;

                int percentcount = FpSpread1.Sheets[0].ColumnCount - 1;
                FpSpread1.Sheets[0].Cells[9, percentcount].Text = "Percentage";
                FpSpread1.Sheets[0].Cells[9, percentcount].HorizontalAlign = HorizontalAlign.Center;
                // sqlStr = "";

                //'--------------------------result column----------------

                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                resultstcount = FpSpread1.Sheets[0].ColumnCount - 1;
                FpSpread1.Sheets[0].Cells[9, resultstcount].Text = "Result";
                FpSpread1.Sheets[0].Cells[9, resultstcount].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[resultstcount].Visible = false;

                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                photostcount = FpSpread1.Sheets[0].ColumnCount - 1;
                FpSpread1.Sheets[0].Cells[9, photostcount].Text = "Photo";

                // FpSpread1.Sheets[0].Columns[photostcount].Width = 100;
                FpSpread1.Sheets[0].Cells[9, photostcount].HorizontalAlign = HorizontalAlign.Center;



                //FpSpread1.Sheets[0].SpanModel.Add(0, 0, 4, 1);
                //FpSpread1.Sheets[0].Columns[0].Width = 150;
                //MyImg2 mi3 = new MyImg2();
                //mi3.ImageUrl = "Handler/Handler2.ashx?";



                //FpSpread1.Sheets[0].Cells[0, 0].CellType = mi3;




                FpSpread1.Sheets[0].SpanModel.Add(0, 1, 1, FpSpread1.Sheets[0].ColumnCount - 2);
                FpSpread1.Sheets[0].SpanModel.Add(1, 1, 1, FpSpread1.Sheets[0].ColumnCount - 2);
                FpSpread1.Sheets[0].SpanModel.Add(2, 1, 1, FpSpread1.Sheets[0].ColumnCount - 2);

                FpSpread1.Sheets[0].SpanModel.Add(3, 1, 1, FpSpread1.Sheets[0].ColumnCount - 2);
                FpSpread1.Sheets[0].SpanModel.Add(4, 1, 1, FpSpread1.Sheets[0].ColumnCount - 2);
                FpSpread1.Sheets[0].SpanModel.Add(5, 1, 1, FpSpread1.Sheets[0].ColumnCount - 2);
                FpSpread1.Sheets[0].SpanModel.Add(6, 1, 1, FpSpread1.Sheets[0].ColumnCount - 2);
                FpSpread1.Sheets[0].SpanModel.Add(7, 1, 1, FpSpread1.Sheets[0].ColumnCount - 2);





                FpSpread1.Sheets[0].SpanModel.Add(8, 0, 1, FpSpread1.Sheets[0].ColumnCount);


                //FpSpread1.Sheets[0].SpanModel.Add(3, 1, 1, photostcount + 2);
                //FpSpread1.Sheets[0].SpanModel.Add(4, 0, 1, photostcount + 1);
                //FpSpread1.Sheets[0].SpanModel.Add(5, 0, 1, photostcount + 1);
                //FpSpread1.Sheets[0].SpanModel.Add(6, 0, 1, photostcount + 1);
                //FpSpread1.Sheets[0].SpanModel.Add(7, 0, 1, photostcount + 1);
                //FpSpread1.Sheets[0].SpanModel.Add(8, 0, 1, photostcount + 1);

                //FpSpread1.Sheets[0].SpanModel.Add(3, 1, 1,photostcount-2);
                //FpSpread1.Sheets[0].SpanModel.Add(4, 0, 1, photostcount -2);
                //FpSpread1.Sheets[0].SpanModel.Add(5, 0, 1, photostcount -2);
                //FpSpread1.Sheets[0].SpanModel.Add(6, 0, 1, photostcount -2);
                //FpSpread1.Sheets[0].SpanModel.Add(7, 0, 1, photostcount -2);
                //FpSpread1.Sheets[0].SpanModel.Add(8, 0, 1, photostcount -2);

                //FpSpread1.Sheets[0].SpanModel.Add(0,photostcount-2,0);

                FpSpread1.Sheets[0].SpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 7, 1);
                MyImgright mi4 = new MyImgright();
                mi4.ImageUrl = "Handler/Handler5.ashx?";


                FpSpread1.Sheets[0].Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].CellType = mi4;

                // FpSpread1.Sheets[0].SpanModel.Add(0, 4, 4, 1);




                // sqlStr = "select distinct registration.Roll_No as roll,registration.Reg_No as regno,registration.stud_name as studname,registration.stud_type as studtype,registration.App_No as ApplicationNumber from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strsec + "  order by  roll  ";
                //sqlStr = "select distinct len(registration.Roll_No),registration.Roll_No as roll,registration.Reg_No as regno,registration.stud_name as studname,registration.stud_type as studtype,registration.App_No as ApplicationNumber from registration, applyn a,exam_type et,result rt where a.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0   " + strsec + " and  rt.exam_code=et.exam_code and registration.roll_no=rt.roll_no and et.criteria_no =" + ddlTest.SelectedValue.ToString() + " order by  len(registration.Roll_No),roll";
                sqlStr = "select distinct len(registration.Roll_No),registration.Roll_No as roll,registration.Reg_No as regno,registration.stud_name as studname,registration.stud_type as studtype,registration.App_No as ApplicationNumber from registration, applyn a,exam_type et,result rt where a.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0   " + strsec + " and  rt.exam_code=et.exam_code and registration.roll_no=rt.roll_no and et.criteria_no =" + ddlTest.SelectedValue.ToString() + " " + strregorder + " ";
                con.Close();
                con.Open();
                if (sqlStr != "")
                {
                    SqlDataAdapter adaSyll1 = new SqlDataAdapter(sqlStr, con);
                    ds1.Clear();
                    ds1.Reset();
                    adaSyll1.Fill(ds1);
                    //   FpEntry.DataBind();
                    int subrow = 0;
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds1.Tables[0].Rows.Count; row++)
                        {
                            // subrow = 1;
                            fail_sub_cnt = 0;
                            find_total = 0;
                            sum_max_mark = 0;
                            for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
                            {
                              
                                if (subrow < Convert.ToInt32(ds2.Tables[0].Rows.Count))
                                {
                                    if (ds1.Tables[0].Rows[row]["roll"].ToString() == ds2.Tables[0].Rows[subrow]["roll"].ToString())
                                    {

                                        if (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) != -2 && double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) != -3 && (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) < double.Parse(ds2.Tables[1].Rows[j]["min_mark"].ToString())))
                                        {
                                            fail_sub_cnt++;
                                        }
                                        if (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) >= 0 && (double.Parse(ds2.Tables[0].Rows[subrow]["mark"].ToString()) >= Convert.ToInt32(ds2.Tables[1].Rows[j]["min_mark"].ToString())))
                                        {
                                            //'---------------total,percent,rank-------------------------------
                                            find_total = (Convert.ToDouble(find_total) + Convert.ToDouble(ds2.Tables[0].Rows[subrow]["mark"].ToString()));
                                            sum_max_mark = sum_max_mark + Convert.ToInt32(ds2.Tables[1].Rows[j]["max_mark"].ToString());
                                            percent = Convert.ToDouble((Convert.ToDouble(find_total) / sum_max_mark) * 100);
                                        }
                                        subrow++;  //added by Mullai
                                    }
                                }
                              

                            }
                            if (fail_sub_cnt == 0)
                            {
                                hat.Clear();
                                hat.Add("RollNumber", ds1.Tables[0].Rows[row]["roll"].ToString());
                                hat.Add("criteria_no", ddlTest.SelectedValue.ToString());
                                hat.Add("Total", find_total.ToString());
                                hat.Add("avg", percent.ToString());
                                hat.Add("rank", "");
                                int o = daccess2.insert_method("INSERT_RANK", hat, "sp");
                            }
                            //'--------------------------------------------------------------


                        }
                        //'--------------------------------insert the rank---------------------------------
                        ra_nk = 1;
                        ds3.Clear();
                        ds3.Reset();
                        ds3 = daccess2.select_method_wo_parameter("SELECT_RANK", "sp");

                        if (ds3.Tables[0].Rows.Count != 0)
                        {
                            double temp_rank = 0;
                            int zx = 1;
                            for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
                            {
                                if (temp_rank == 0)
                                {
                                    ra_nk = 1;
                                    hat.Clear();
                                    hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                    hat.Add("criteria_no", ddlBranch.SelectedValue.ToString());
                                    hat.Add("Total", Convert.ToString(find_total));
                                    hat.Add("avg", Convert.ToString(percent));
                                    hat.Add("rank", ra_nk.ToString());
                                    int o = daccess2.insert_method("INSERT_RANK", hat, "sp");

                                    temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                                }
                                else if (temp_rank != 0)
                                {
                                    if (temp_rank > double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                                    {
                                        //   ra_nk += 1;
                                        ra_nk = zx;
                                        hat.Clear();
                                        hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                        hat.Add("criteria_no", ddlBranch.SelectedValue.ToString());
                                        hat.Add("Total", Convert.ToString(find_total));
                                        hat.Add("avg", Convert.ToString(percent)); ;
                                        hat.Add("rank", ra_nk.ToString());
                                        int o = daccess2.insert_method("INSERT_RANK", hat, "sp");

                                        temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());

                                    }
                                    else if (temp_rank == double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString()))
                                    {

                                        hat.Clear();
                                        hat.Add("RollNumber", ds3.Tables[0].Rows[rank]["Rollno"].ToString());
                                        hat.Add("criteria_no", ddlBranch.SelectedValue.ToString());
                                        hat.Add("Total", Convert.ToString(find_total));
                                        hat.Add("avg", Convert.ToString(percent));
                                        hat.Add("rank", ra_nk.ToString());
                                        int o = daccess2.insert_method("INSERT_RANK", hat, "sp");
                                        temp_rank = double.Parse(ds3.Tables[0].Rows[rank]["Total"].ToString());
                                    }
                                }
                                zx++;
                            }
                            //---------------new end 030412

                        }
                    }
                }
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    if (ds3.Tables[0].Rows.Count != 0)
                    {
                        ds3.Clear();
                        ds3.Reset();
                        ds3 = daccess2.select_method_wo_parameter("SELECT_RANK", "sp");
                        int rank_row_count = 0;
                        for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                        {
                            if (rank_row_count < ds3.Tables[0].Rows.Count)
                            {
                                //if (ds3.Tables[0].Rows[rank_row_count]["Rollno"].ToString() == ds1.Tables[0].Rows[i]["roll"].ToString())
                                //{
                                //if (Convert.ToInt32(ds3.Tables[0].Rows[rank_row_count]["Rank"].ToString()) < 4)
                                if (Convert.ToInt32(ds3.Tables[0].Rows[rank_row_count]["Rank"].ToString()) <= topstud)
                                {

                                    //FpSpread1.Sheets[0].SpanModel.Add(0, 4, 3, 1);


                                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Name = "Book Antique";
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Bold = false;
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Rank" + " " + ds3.Tables[0].Rows[rank_row_count]["Rank"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds3.Tables[0].Rows[rank_row_count]["Rollno"].ToString();
                                    string roll_no = ds3.Tables[0].Rows[rank_row_count]["Rollno"].ToString();

                                    //'------------------------load the student photo------------------
                                    MyImg3 mi5 = new MyImg3();
                                    mi5.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + roll_no;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, photostcount].CellType = mi5;
                                    FpSpread1.Sheets[0].Columns[4].Width = 120;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, percentcount].Text = Math.Round(Convert.ToDouble(ds3.Tables[0].Rows[rank_row_count]["perc"].ToString()), 2).ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, totalstcount].Text = Math.Round(Convert.ToDouble(ds3.Tables[0].Rows[rank_row_count]["Total"].ToString()), 2).ToString();


                                    string fstname = "select registration.stud_name from registration where roll_no='" + ds3.Tables[0].Rows[rank_row_count]["Rollno"].ToString() + "'";
                                    SqlCommand credit = new SqlCommand(fstname, con);
                                    con.Close();
                                    con.Open();
                                    SqlDataReader studname;
                                    studname = credit.ExecuteReader();
                                    if (studname.HasRows)
                                    {

                                        while (studname.Read())
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = studname["stud_name"].ToString();

                                        }
                                    }

                                }
                                rank_row_count++;
                                //  }
                            }
                        }
                    }//'--------------------------------end rank------------------------
                }
            }

            string strsection;
            string subsec = ddlSec.SelectedValue.ToString();
            if (subsec.ToString() == "All" || subsec.ToString() == "" || subsec.ToString() == "-1")
            {
                strsection = "";
            }
            else
            {
                strsection = " and R.sections='" + subsec.ToString() + "'";
            }

            for (int i = 10; i <= FpSpread1.Sheets[0].RowCount - 1; i++)
            {
                string strroll = FpSpread1.Sheets[0].Cells[i, 1].Text.ToString();
                for (int j = 3; j <= FpSpread1.Sheets[0].ColumnCount - 5; j++)
                {
                    string sbno = FpSpread1.Sheets[0].Cells[9, j].Tag.ToString();
                    sqlStr = "SELECT Marks_Obtained FROM Registration R,Result U,Exam_Type E,Subject S ";
                    sqlStr = sqlStr + "WHERE R.Roll_No = U.Roll_No AND U.Exam_Code = E.Exam_Code AND E.Subject_No = S.Subject_No ";
                    sqlStr = sqlStr + "AND R.Degree_Code =" + ddlBranch.SelectedValue.ToString() + " AND R.Batch_year =" + ddlBatch.SelectedValue.ToString() + " AND E.Criteria_No =" + ddlTest.SelectedValue.ToString() + strsection;
                    sqlStr = sqlStr + "AND RollNo_Flag <> 0 AND CC = 0 AND Exam_Flag <> 'DEBAR' AND DelFlag = 0 ";
                    sqlStr = sqlStr + "AND S.Subject_No =" + sbno + " AND U.Roll_No ='" + strroll + "' ";
                    sqlStr = sqlStr + "ORDER BY S.Subject_No ";

                    con.Close();
                    con.Open();
                    SqlCommand markcmd = new SqlCommand(sqlStr, con);
                    SqlDataReader markds;
                    markds = markcmd.ExecuteReader();
                    if (markds.HasRows)
                    {
                        while (markds.Read())
                        {
                            FpSpread1.Sheets[0].Cells[i, j].Text = markds["Marks_Obtained"].ToString();
                        }
                    }
                }

            }





            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "INDIVIDUAL SUBJECT TOPPER";
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Underline = true;
            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Roll No";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Student Name";
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Subject";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "Mark";
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 3, 1, 2);

            //'---------------------------------------------------new individual topper start-------------------
            for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
            {

                hat.Clear();
                string secss = "";
                if (ddlSec.Enabled == false)
                {
                    secss = "";
                }
                else
                {
                    secss = ddlSec.SelectedItem.Text.ToString();
                }

                if (secss.ToString().Trim() == "-1" || secss.ToString().Trim() == "" || secss.ToString().Trim() == null || secss.ToString().Trim() == "All")
                {
                    secss = "";  // added by sridhar aug 2014
                }
                else
                {
                    secss = ddlSec.SelectedItem.Text.ToString(); // added by sridhar aug 2014
                }
                hat.Add("exam_code", ds2.Tables[1].Rows[i]["exam_code"].ToString());
                hat.Add("min_marks", ds2.Tables[1].Rows[i]["min_mark"].ToString());
                hat.Add("section", secss);
                ds4.Clear();
                ds4.Reset();
                ds4 = daccess2.select_method("Proc_All_Subject_Details", hat, "sp");

                if (ds4.Tables[10].Rows.Count > 0)
                {
                    for (int dsrow = 0; dsrow < ds4.Tables[10].Rows.Count; dsrow++)
                    {
                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                        // FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 3, 1, 2);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds4.Tables[10].Rows[dsrow]["ROLL_NO"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds4.Tables[10].Rows[dsrow]["STUD_NAME"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds2.Tables[1].Rows[i]["subject_code"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds4.Tables[3].Rows[0]["MAX_MARK"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                    }
                }
            }

            //'---------------------------------------------------new individual topper end-------------------
            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
            // FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Name = "Book Antique";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Subject Code";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Subject Name";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "Staff Incharge";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "Pass%";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Avg%";
            //'--------------------------------new subject details start--------------------------------------------
            int total_pass_fail = 0;
            for (int j = 0; j < ds2.Tables[1].Rows.Count; j++)
            {
                hat.Clear();
                string secss = "";
                if (ddlSec.Enabled == false)// added by sridhar aug 2014
                {
                    secss = "";
                }
                else
                {
                    secss = ddlSec.SelectedItem.Text.ToString();
                }
                if (secss.ToString().Trim() == "-1" || secss.ToString().Trim() == "" || secss.ToString().Trim() == null || secss.ToString().Trim() == "All")
                {
                    secss = "";  // added by sridhar aug 2014
                }
                else
                {
                    secss = ddlSec.SelectedItem.Text.ToString(); // added by sridhar aug 2014
                }
                hat.Add("exam_code", ds2.Tables[1].Rows[j]["exam_code"].ToString());
                hat.Add("min_marks", ds2.Tables[1].Rows[j]["min_mark"].ToString());
                hat.Add("section", secss);
                ds4.Clear();
                ds4.Reset();
                ds4 = daccess2.select_method("Proc_All_Subject_Details", hat, "sp");

                if (ds2.Tables[1].Rows.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds2.Tables[1].Rows[j]["subject_code"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds2.Tables[1].Rows[j]["subject_name"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                    total_pass_fail = Convert.ToInt32(ds4.Tables[1].Rows[0]["PASS_COUNT"]) + Convert.ToInt32(ds4.Tables[2].Rows[0]["FAIL_COUNT"]);
                    double cal_avg = Convert.ToDouble(ds4.Tables[0].Rows[0]["SUM"]) / Convert.ToDouble(total_pass_fail);
                    cal_avg = Math.Round(cal_avg, 2);

                    double pass_perc = (Convert.ToDouble(ds4.Tables[1].Rows[0]["PASS_COUNT"]) / Convert.ToDouble(total_pass_fail)) * 100;
                    pass_perc = Math.Round(pass_perc, 2);

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = pass_perc.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = cal_avg.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                    if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                    {
                        strsec = "";
                    }
                    else
                    {
                        strsec = " and exam_type.sections='" + sections.ToString() + "'";
                    }
                    string temp = "";
                    if ((ds2.Tables[1].Rows[j]["subject_no"].ToString() != "") && (ddlTest.SelectedValue.ToString() != ""))
                    {
                        temp = GetFunction("Select distinct staff_code from result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subject_no =" + ds2.Tables[1].Rows[j]["subject_no"].ToString() + " " + strsec + " and exam_type.criteria_no =" + ddlTest.SelectedValue.ToString() + "");
                        if (temp != "")
                        {
                            staff = GetFunction("select staff_name from staffmaster where staff_code = '" + temp + "'");
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = staff;

                    }
                }
            }
            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 5;
            for (int i = FpSpread1.Sheets[0].RowCount - 5; i < FpSpread1.Sheets[0].RowCount - 1; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Border.BorderColor = Color.White;
                FpSpread1.Sheets[0].Rows[i].Font.Bold = true;
            }

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, 0].Border.BorderColorLeft = Color.Black;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, 0].Border.BorderColorLeft = Color.Black;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Border.BorderColorLeft = Color.Black;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorLeft = Color.Black;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorLeft = Color.Black;

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, 0].Border.BorderColorRight = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, 0].Border.BorderColorRight = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Border.BorderColorRight = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorRight = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorRight = Color.White;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
            ////FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 5, 0, 5, 1); //new 05.04.12
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColorRight = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Border.BorderColorRight = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Border.BorderColorRight = Color.White;

            FpSpread1.Sheets[0].Rows[5].Font.Underline = false;


            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, 0].Border.BorderColorBottom = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, 0].Border.BorderColorBottom = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.White;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].Rows.Count;

            //'-----------------------------------new subject end-----------------------------------------------------
            //'--------------------------------------------------------mythili end----------------------------------------------'


        }
        catch
        {
        }

        //}
        //catch
        //{

        //}
    }

    //public void generateletterformat1(Gios.Pdf.PdfDocument mydocument, Font Fontsmall, Font Fontbold, Font Fontbold1, DataTable dt, HttpResponse response)
    //{
    //    try
    //    {

    //        int subno = 0;
    //        int pagecount = sno / 15;
    //        int repage = sno % 15;

    //        int nopages = pagecount;
    //        if (repage > 0)
    //        {
    //            nopages++;
    //        }

    //        //table2.....................
    //        int subno2 = 1;
    //        int pagecount2 = sno2 / 25;
    //        int repage2 = sno2 % 25;
    //        int nopages2 = pagecount2;
    //        if (repage2 > 0)
    //        {
    //            nopages2++;
    //        }

    //        int final_pagecount = pagecount + pagecount2;
    //        int repages_Pagecount = repage + repage2;
    //        int nopages_pagecount = final_pagecount;
    //        if (repages_Pagecount > 0)
    //        {
    //            nopages_pagecount++;
    //        }

    //        if (sno2 < 50)
    //        {
    //            nopages_pagecount++;
    //        }

    //        int final_count_tb4 = nopages_pagecount - 1;

    //        int tab1_count = ds2.Tables[1].Rows.Count + 6;

    //        //table2.................
    //        //int row_cnt = 0;
    //        string rank_spli = "";
    //        string roll_split = "";
    //        string stud_split = "";
    //        string mark_split = "";
    //        string total_split = "";
    //        string per_split = "";
    //        int subno_mau = 2;
    //        //int val = 3;
    //        int ex_count = 0;
    //        string sub_new_code = "";

    //        //table2 string value...........
    //        string table2_rollsplit = "";
    //        string table2_stud_split = "";
    //        string table2_Sbj_split = "";
    //        string table2_mark_split = "";
    //        if (nopages > 0)
    //        {
    //            for (int row = 0; row < nopages_pagecount; row++)
    //            {
    //                int enter_page = 1;
    //                subno++;
    //                Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();

    //                PdfArea tete = new PdfArea(mydocument, 25, 10, 800, 1100);

    //                PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);

    //                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Sankar
    //                {
    //                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
    //                    mypdfpage.Add(LogoImage, 25, 25, 300);
    //                }

    //                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Sankar
    //                {
    //                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
    //                    mypdfpage.Add(LogoImage, 685, 25, 300);
    //                }

    //                //int col_count = Convert.ToInt32(sub_code) + Convert.ToInt32(srno);

    //                //int col_count = sub_code + Convert.ToInt32(srno);
    //                //table2........bind
    //                //int cnt2 = subno * sno2;
    //                //int cnt12 = subno * 25;

    //                int val1 = 0;
    //                int val2 = 0;

    //                int subno_val = 1;
    //                int cnt = subno_val * sno;
    //                int cnt1 = subno_val * 15;

    //                int cnt2 = subno_val * sno2;
    //                int cnt12 = subno_val * 25;

    //                Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontsmall, cnt2 + 1, 4, 1);
    //                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

    //                Gios.Pdf.PdfTable table = mydocument.NewTable(Fontsmall, cnt1 + 1, tab1_count, 1);
    //                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //                table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
    //                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table.Cell(0, 0).SetContent("Rank");
    //                table.Columns[0].SetWidth(100);

    //                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table.Cell(0, 1).SetContent("Roll No");
    //                table.Columns[1].SetWidth(180);

    //                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table.Cell(0, 2).SetContent("Student Name");
    //                table.Columns[2].SetWidth(250);
    //                int val = 3;
    //                //string sub_new_code = "";
    //                for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
    //                {
    //                    //int val = 3;
    //                    table.Cell(0, val).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    string[] splitsb_no = subj_code.Split(new Char[] { '\n' });
    //                    sub_new_code = splitsb_no[i];
    //                    table.Cell(0, val).SetContent(sub_new_code);
    //                    table.Columns[val].SetWidth(200);
    //                    val++;
    //                }

    //                int next_val = ds2.Tables[1].Rows.Count + 3;
    //                int next_val1 = ds2.Tables[1].Rows.Count + 4;

    //                table.Cell(0, val).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table.Cell(0, val).SetContent("Total");
    //                table.Columns[val].SetWidth(200);
    //                val++;

    //                table.Cell(0, val).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table.Cell(0, val).SetContent("Percentage");
    //                table.Columns[val].SetWidth(200);
    //                val++;

    //                table.Cell(0, val).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                table.Cell(0, val).SetContent("Photo");
    //                table.Columns[val].SetWidth(200);

    //                if (subno == 1)
    //                {
    //                    if (cnt < 15)
    //                    {


    //                        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                                new PdfArea(mydocument, 0, 0, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collnamenew1 + "");
    //                        PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydocument, 0, 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + address + "");

    //                        PdfTextArea pts1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydocument, 0, 40, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + phnfax + "");

    //                        PdfTextArea pts2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydocument, 0, 60, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + email + "");

    //                        PdfTextArea pts3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydocument, 0, 80, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "OVERALL BEST PERFORMANCE");
    //                        PdfTextArea pts5 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydocument, 0, 100, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Batch Year:  " + ddlBatch.SelectedValue.ToString() + " " + "Course: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString());
    //                        PdfTextArea pts6 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydocument, 0, 120, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Test: " + test + " " + "Semester: " + ddlSemYr.SelectedValue.ToString() + "Section: " + ddlSec.SelectedValue.ToString());

    //                        mypdfpage.Add(ptc);
    //                        mypdfpage.Add(pts);
    //                        mypdfpage.Add(pts1);
    //                        mypdfpage.Add(pts2);
    //                        mypdfpage.Add(pts3);
    //                        mypdfpage.Add(pts5);
    //                        mypdfpage.Add(pts6);

    //                        table = mydocument.NewTable(Fontsmall, cnt + 1, tab1_count, 1);
    //                        table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

    //                        table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
    //                        table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table.Cell(0, 0).SetContent("Rank");
    //                        table.Columns[0].SetWidth(100);

    //                        table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table.Cell(0, 1).SetContent("Roll No");
    //                        table.Columns[1].SetWidth(100);

    //                        table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table.Cell(0, 2).SetContent("Student Name");
    //                        table.Columns[2].SetWidth(150);
    //                        //string sub_new_code = "";
    //                        int val_new = 3;
    //                        for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
    //                        {

    //                            table.Cell(0, val_new).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            string[] splitsb_no = subj_code.Split(new Char[] { '\n' });
    //                            sub_new_code = splitsb_no[i];
    //                            table.Cell(0, val_new).SetContent(sub_new_code);
    //                            table.Columns[val_new].SetWidth(100);
    //                            val_new++;
    //                        }

    //                        //int next_val = ds2.Tables[1].Rows.Count + 3;
    //                        //int next_val1 = ds2.Tables[1].Rows.Count + 4;

    //                        table.Cell(0, val_new).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table.Cell(0, val_new).SetContent("Total");
    //                        table.Columns[val_new].SetWidth(100);
    //                        val_new++;

    //                        table.Cell(0, val_new).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table.Cell(0, val_new).SetContent("Percentage");
    //                        table.Columns[val_new].SetWidth(100);
    //                        val_new++;

    //                        table.Cell(0, val_new).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        table.Cell(0, val_new).SetContent("Photo");
    //                        table.Columns[val_new].SetWidth(100);


    //                        for (int i = 0; i < cnt; i++)
    //                        {

    //                            val1++;
    //                            int row_cnt = 0;
    //                            table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            string[] splitRank = Rank.Split(new Char[] { '\n' });
    //                            rank_spli = splitRank[i];
    //                            table.Cell(val1, 0).SetContent(rank_spli);
    //                            row_cnt++;

    //                            table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            string[] splitRoll_No = stude_RollNumber.Split(new Char[] { '\n' });
    //                            roll_split = splitRoll_No[i];
    //                            table.Cell(val1, row_cnt).SetContent(roll_split);
    //                            row_cnt++;

    //                            table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            string[] split_Stud_Name = stud_Nameof.Split(new Char[] { '\n' });
    //                            stud_split = split_Stud_Name[i];
    //                            table.Cell(val1, row_cnt).SetContent(stud_split);
    //                            row_cnt++;

    //                            for (int i1 = 0; i1 < ds2.Tables[1].Rows.Count; i1++)
    //                            {
    //                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] split_Mark = mark_obt.Split(new Char[] { '\n' });
    //                                mark_split = split_Mark[i1];
    //                                table.Cell(val1, row_cnt).SetContent(mark_split);
    //                                row_cnt++;

    //                            }
    //                            table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            string[] split_Total = Total_Mark.Split(new Char[] { '\n' });
    //                            total_split = split_Total[i];
    //                            table.Cell(val1, row_cnt).SetContent(total_split);
    //                            row_cnt++;

    //                            table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            string[] split_Per = Pertc.Split(new Char[] { '\n' });
    //                            per_split = split_Per[i];
    //                            table.Cell(val1, row_cnt).SetContent(per_split);
    //                            row_cnt++;
    //                            //Aruna 17apr2013 Add Student Photo===================================================
    //                            MemoryStream memoryStream = new MemoryStream();
    //                            SqlCommand cmd = new SqlCommand();
    //                            con.Close();
    //                            con.Open();
    //                            cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + roll_split + "')";
    //                            cmd.Connection = con;

    //                            SqlDataReader MyReader = cmd.ExecuteReader();
    //                            if (MyReader.Read())
    //                            {

    //                                byte[] file = (byte[])MyReader["photo"];
    //                                MyReader.Close();
    //                                memoryStream.Write(file, 0, file.Length);
    //                                if (file.Length > 0)
    //                                {
    //                                    //System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, true, true);                                                          
    //                                    //img.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
    //                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(100, 100, null, IntPtr.Zero);
    //                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

    //                                }
    //                                memoryStream.Dispose();
    //                                memoryStream.Close();
    //                                MyReader.Close();
    //                            }
    //                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")))
    //                            {
    //                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"));
    //                                table.Cell(val1, row_cnt).SetContent(leftimage);
    //                                //mypdfpage.Add(leftimage, 685, 25, 300);

    //                            }

    //                            //Sankar 17apr2013 Add Student Photo===================================================
    //                        }
    //                    }
    //                    else
    //                    {

    //                        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                                   new PdfArea(mydocument, 0, 0, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collnamenew1 + "");
    //                        PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                           new PdfArea(mydocument, 0, 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + address + "");

    //                        PdfTextArea pts1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydocument, 0, 40, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + phnfax + "");

    //                        PdfTextArea pts2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydocument, 0, 60, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + email + "");

    //                        PdfTextArea pts3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                          new PdfArea(mydocument, 0, 80, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "OVERALL BEST PERFORMANCE");
    //                        PdfTextArea pts5 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydocument, 0, 100, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Batch Year:  " + ddlBatch.SelectedValue.ToString() + " " + "Course: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString());
    //                        PdfTextArea pts6 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
    //                                                         new PdfArea(mydocument, 0, 120, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Test: " + test + " " + "Semester: " + ddlSemYr.SelectedValue.ToString() + "Section: " + ddlSec.SelectedValue.ToString());

    //                        mypdfpage.Add(ptc);
    //                        mypdfpage.Add(pts);
    //                        mypdfpage.Add(pts1);
    //                        mypdfpage.Add(pts2);
    //                        mypdfpage.Add(pts3);
    //                        mypdfpage.Add(pts5);
    //                        mypdfpage.Add(pts6);

    //                        for (int i = 0; i < cnt1; i++)
    //                        {

    //                            val1++;

    //                            int row_cnt = 0;
    //                            table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            string[] splitRank = Rank.Split(new Char[] { '\n' });
    //                            rank_spli = splitRank[i];
    //                            table.Cell(val1, 0).SetContent(rank_spli);
    //                            row_cnt++;

    //                            table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            string[] splitRoll_No = stude_RollNumber.Split(new Char[] { '\n' });
    //                            roll_split = splitRoll_No[i];
    //                            table.Cell(val1, row_cnt).SetContent(roll_split);
    //                            row_cnt++;

    //                            table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                            string[] split_Stud_Name = stud_Nameof.Split(new Char[] { '\n' });
    //                            stud_split = split_Stud_Name[i];
    //                            table.Cell(val1, row_cnt).SetContent(stud_split);
    //                            row_cnt++;

    //                            for (int i1 = 0; i1 < ds2.Tables[1].Rows.Count; i1++)
    //                            {
    //                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] split_Mark = mark_obt.Split(new Char[] { '\n' });
    //                                mark_split = split_Mark[i];
    //                                table.Cell(val1, row_cnt).SetContent(mark_split);
    //                                row_cnt++;

    //                            }
    //                            table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            string[] split_Total = Total_Mark.Split(new Char[] { '\n' });
    //                            total_split = split_Total[i];
    //                            table.Cell(val1, row_cnt).SetContent(total_split);
    //                            row_cnt++;

    //                            table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            string[] split_Per = Pertc.Split(new Char[] { '\n' });
    //                            per_split = split_Per[i];
    //                            table.Cell(val1, row_cnt).SetContent(per_split);
    //                            row_cnt++;
    //                            //Aruna 17apr2013 Add Student Photo===================================================
    //                            MemoryStream memoryStream = new MemoryStream();
    //                            SqlCommand cmd = new SqlCommand();
    //                            con.Close();
    //                            con.Open();
    //                            cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + roll_split + "')";
    //                            cmd.Connection = con;

    //                            SqlDataReader MyReader = cmd.ExecuteReader();
    //                            if (MyReader.Read())
    //                            {

    //                                byte[] file = (byte[])MyReader["photo"];
    //                                MyReader.Close();
    //                                memoryStream.Write(file, 0, file.Length);
    //                                if (file.Length > 0)
    //                                {
    //                                    //System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, true, true);                                                          
    //                                    //img.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
    //                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(50, 50, null, IntPtr.Zero);
    //                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")) == false)
    //                                    {
    //                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                    }


    //                                }
    //                                memoryStream.Dispose();
    //                                memoryStream.Close();
    //                                MyReader.Close();
    //                            }

    //                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")))
    //                            {
    //                                int imag_cnt = 260;
    //                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"));
    //                                //table.Cell(val1, row_cnt).SetContent(leftimage);
    //                                mypdfpage.Add(leftimage, 800, imag_cnt, 100);
    //                                //int imag_cnt = imag_cnt + 20;
    //                                imag_cnt++;
    //                            }

    //                            //Sankar 17apr2013 Add Student Photo==================================================
    //                        }
    //                    }
    //                }
    //                if (subno > 1)
    //                {
    //                    val1 = (subno - 1) * 15;
    //                    int ro = 0;

    //                    int remaindsubs = sno - val1;

    //                    if (remaindsubs < 7)
    //                    {
    //                        if (remaindsubs > 0)
    //                        {
    //                            table = mydocument.NewTable(Fontsmall, remaindsubs + 1, tab1_count, 1);
    //                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

    //                            table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
    //                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, 0).SetContent("Rank");
    //                            table.Columns[0].SetWidth(100);

    //                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, 1).SetContent("Roll No");
    //                            table.Columns[1].SetWidth(180);

    //                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, 2).SetContent("Student Name");
    //                            table.Columns[2].SetWidth(250);
    //                            int valrem = 3;
    //                            for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
    //                            {

    //                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] splitsb_no = subj_code.Split(new Char[] { '\n' });
    //                                sub_new_code = splitsb_no[i];
    //                                table.Cell(0, valrem).SetContent(sub_new_code);
    //                                table.Columns[valrem].SetWidth(200);
    //                                valrem++;
    //                            }

    //                            //int next_val = ds2.Tables[1].Rows.Count + 3;
    //                            //int next_val1 = ds2.Tables[1].Rows.Count + 4;

    //                            table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, valrem).SetContent("Total");
    //                            table.Columns[valrem].SetWidth(200);
    //                            valrem++;

    //                            table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, valrem).SetContent("Percentage");
    //                            table.Columns[valrem].SetWidth(200);
    //                            valrem++;

    //                            table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, valrem).SetContent("Photo");
    //                            table.Columns[valrem].SetWidth(200);
    //                            for (int fg = 0; fg < remaindsubs; fg++)
    //                            {
    //                                ro++;
    //                                int row_cnt = 0;
    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] splitRank = Rank.Split(new Char[] { '\n' });
    //                                rank_spli = splitRank[val1];
    //                                table.Cell(ro, 0).SetContent(rank_spli);
    //                                row_cnt++;

    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] splitRoll_No = stude_RollNumber.Split(new Char[] { '\n' });
    //                                roll_split = splitRoll_No[val1];
    //                                table.Cell(ro, row_cnt).SetContent(roll_split);
    //                                row_cnt++;

    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                string[] split_Stud_Name = stud_Nameof.Split(new Char[] { '\n' });
    //                                stud_split = split_Stud_Name[val1];
    //                                table.Cell(ro, row_cnt).SetContent(stud_split);
    //                                row_cnt++;

    //                                for (int i1 = 0; i1 < ds2.Tables[1].Rows.Count; i1++)
    //                                {
    //                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    string[] split_Mark = mark_obt.Split(new Char[] { '\n' });
    //                                    mark_split = split_Mark[val1];
    //                                    table.Cell(ro, row_cnt).SetContent(mark_split);
    //                                    row_cnt++;

    //                                }
    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] split_Total = Total_Mark.Split(new Char[] { '\n' });
    //                                total_split = split_Total[val1];
    //                                table.Cell(ro, row_cnt).SetContent(total_split);
    //                                row_cnt++;

    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] split_Per = Pertc.Split(new Char[] { '\n' });
    //                                per_split = split_Per[val1];
    //                                table.Cell(ro, row_cnt).SetContent(per_split);
    //                                row_cnt++;
    //                                //Aruna 17apr2013 Add Student Photo===================================================
    //                                MemoryStream memoryStream = new MemoryStream();
    //                                SqlCommand cmd = new SqlCommand();
    //                                con.Close();
    //                                con.Open();
    //                                cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + roll_split + "')";
    //                                cmd.Connection = con;

    //                                SqlDataReader MyReader = cmd.ExecuteReader();
    //                                if (MyReader.Read())
    //                                {

    //                                    byte[] file = (byte[])MyReader["photo"];
    //                                    MyReader.Close();
    //                                    memoryStream.Write(file, 0, file.Length);
    //                                    if (file.Length > 0)
    //                                    {
    //                                        //System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, true, true);                                                          
    //                                        //img.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
    //                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
    //                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")) == false)
    //                                        {
    //                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                        }

    //                                    }
    //                                    memoryStream.Dispose();
    //                                    memoryStream.Close();
    //                                    MyReader.Close();
    //                                }
    //                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")))
    //                                {
    //                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"));
    //                                    //table.Cell(val1, row_cnt).SetContent(leftimage);
    //                                    mypdfpage.Add(leftimage, 685, 25, 300);

    //                                }
    //                                val1++;

    //                            }

    //                            //table2 bind
    //                            table1 = mydocument.NewTable(Fontsmall, cnt12 + 1, 4, 1);
    //                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

    //                            table1.Columns[0].SetWidth(50);
    //                            table1.Columns[1].SetWidth(100);
    //                            table1.Columns[2].SetWidth(100);
    //                            table1.Columns[3].SetWidth(100);
    //                            table1.CellRange(0, 0, 0, 3).SetFont(Fontbold);
    //                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table1.Cell(0, 0).SetContent("Roll No");
    //                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table1.Cell(0, 1).SetContent("Student Name");
    //                            table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table1.Cell(0, 2).SetContent("Subject");

    //                            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table1.Cell(0, 3).SetContent("Mark");
    //                            //int val2 = 0;
    //                            if (cnt2 < 25)
    //                            {
    //                                for (int i = 0; i < cnt2; i++)
    //                                {
    //                                    val2++;
    //                                    table1.Cell(val2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
    //                                    table2_rollsplit = split_roll[i];
    //                                    table1.Cell(val2, 0).SetContent(table2_rollsplit);


    //                                    table1.Cell(val2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                    string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
    //                                    table2_stud_split = split_stu_tb2[i];
    //                                    table1.Cell(val2, 1).SetContent(table2_stud_split);


    //                                    table1.Cell(val2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
    //                                    table2_Sbj_split = split_sub_code[i];
    //                                    table1.Cell(val, 2).SetContent(table2_Sbj_split);

    //                                    table1.Cell(val2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
    //                                    table2_mark_split = splitsub_mark[i];
    //                                    table1.Cell(val2, 3).SetContent(table2_mark_split);


    //                                }
    //                                //subno_mau++;
    //                            }
    //                            else
    //                            {
    //                                for (int i = 0; i < cnt12; i++)
    //                                {
    //                                    val2++;
    //                                    table1.Cell(val2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
    //                                    table2_rollsplit = split_roll[i];
    //                                    table1.Cell(val2, 0).SetContent(table2_rollsplit);


    //                                    table1.Cell(val2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                    string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
    //                                    table2_stud_split = split_stu_tb2[i];
    //                                    table1.Cell(val2, 1).SetContent(table2_stud_split);


    //                                    table1.Cell(val2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
    //                                    table2_Sbj_split = split_sub_code[i];
    //                                    table1.Cell(val2, 2).SetContent(table2_Sbj_split);

    //                                    table1.Cell(val2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
    //                                    table2_mark_split = splitsub_mark[i];
    //                                    table1.Cell(val2, 3).SetContent(table2_mark_split);

    //                                }

    //                                //subno_mau++;
    //                                ex_count++;
    //                            }

    //                        }
    //                        else
    //                        {
    //                            if (ex_count == 0)
    //                            {
    //                                val2 = 0;
    //                            }
    //                            else
    //                            {
    //                                val2 = (subno_mau - 1) * 25;
    //                                subno_mau++;
    //                            }
    //                            //val2 = (subno_mau - 1) * 25;                      
    //                            int ro2 = 0;
    //                            int remaindsubs1 = sno2 - val2;

    //                            ex_count++;
    //                            if (remaindsubs1 > 0)
    //                            {
    //                                if (cnt2 == 10)
    //                                {
    //                                    //table2 full binding........................
    //                                    enter_page = 0;
    //                                    table1.Columns[0].SetWidth(50);
    //                                    table1.Columns[1].SetWidth(100);
    //                                    table1.Columns[2].SetWidth(100);
    //                                    table1.Columns[3].SetWidth(100);
    //                                    table1.CellRange(0, 0, 0, 3).SetFont(Fontbold);
    //                                    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    table1.Cell(0, 0).SetContent("Roll No");
    //                                    table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    table1.Cell(0, 1).SetContent("Student Name");
    //                                    table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    table1.Cell(0, 2).SetContent("Subject");

    //                                    table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    table1.Cell(0, 3).SetContent("Mark");

    //                                    if (cnt2 < 25)
    //                                    {
    //                                        for (int i = 0; i < cnt2; i++)
    //                                        {
    //                                            val2++;
    //                                            table1.Cell(val2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
    //                                            table2_rollsplit = split_roll[i];
    //                                            table1.Cell(val2, 0).SetContent(table2_rollsplit);


    //                                            table1.Cell(val2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                            string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
    //                                            table2_stud_split = split_stu_tb2[i];
    //                                            table1.Cell(val2, 1).SetContent(table2_stud_split);


    //                                            table1.Cell(val2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
    //                                            table2_Sbj_split = split_sub_code[i];
    //                                            table1.Cell(val, 2).SetContent(table2_Sbj_split);

    //                                            table1.Cell(val2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
    //                                            table2_mark_split = splitsub_mark[i];
    //                                            table1.Cell(val2, 3).SetContent(table2_mark_split);
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        for (int i = 0; i < cnt12; i++)
    //                                        {
    //                                            val2++;
    //                                            table1.Cell(val2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
    //                                            table2_rollsplit = split_roll[i];
    //                                            table1.Cell(val2, 0).SetContent(table2_rollsplit);


    //                                            table1.Cell(val2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                            string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
    //                                            table2_stud_split = split_stu_tb2[i];
    //                                            table1.Cell(val2, 1).SetContent(table2_stud_split);


    //                                            table1.Cell(val2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
    //                                            table2_Sbj_split = split_sub_code[i];
    //                                            table1.Cell(val2, 2).SetContent(table2_Sbj_split);

    //                                            table1.Cell(val2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
    //                                            table2_mark_split = splitsub_mark[i];
    //                                            table1.Cell(val2, 3).SetContent(table2_mark_split);
    //                                        }
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    if (remaindsubs1 < 25)
    //                                    {
    //                                        table1 = mydocument.NewTable(Fontsmall, remaindsubs1 + 1, 4, 1);
    //                                        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //                                        enter_page = 0;
    //                                        table1.Columns[0].SetWidth(50);
    //                                        table1.Columns[1].SetWidth(100);
    //                                        table1.Columns[2].SetWidth(100);
    //                                        table1.Columns[3].SetWidth(100);
    //                                        table1.CellRange(0, 0, 0, 3).SetFont(Fontbold);
    //                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        table1.Cell(0, 0).SetContent("Roll No");
    //                                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        table1.Cell(0, 1).SetContent("Student Name");
    //                                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        table1.Cell(0, 2).SetContent("Subject");

    //                                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        table1.Cell(0, 3).SetContent("Mark");
    //                                        for (int fg = 0; fg < remaindsubs1; fg++)
    //                                        {
    //                                            ro2++;
    //                                            table1.Cell(ro2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
    //                                            table2_rollsplit = split_roll[val2];
    //                                            table1.Cell(ro2, 0).SetContent(table2_rollsplit);


    //                                            table1.Cell(ro2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                            string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
    //                                            table2_stud_split = split_stu_tb2[val2];
    //                                            table1.Cell(ro2, 1).SetContent(table2_stud_split);


    //                                            table1.Cell(ro2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
    //                                            table2_Sbj_split = split_sub_code[val2];
    //                                            table1.Cell(ro2, 2).SetContent(table2_Sbj_split);

    //                                            table1.Cell(ro2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
    //                                            table2_mark_split = splitsub_mark[val2];
    //                                            table1.Cell(ro2, 3).SetContent(table2_mark_split);
    //                                            val2++;

    //                                        }


    //                                    }
    //                                    else
    //                                    {
    //                                        table1 = mydocument.NewTable(Fontsmall, 25 + 1, 4, 1);
    //                                        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
    //                                        enter_page = 0;
    //                                        table1.Columns[0].SetWidth(50);
    //                                        table1.Columns[1].SetWidth(100);
    //                                        table1.Columns[2].SetWidth(100);
    //                                        table1.Columns[3].SetWidth(100);
    //                                        table1.CellRange(0, 0, 0, 3).SetFont(Fontbold);
    //                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        table1.Cell(0, 0).SetContent("Roll No");
    //                                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        table1.Cell(0, 1).SetContent("Student Name");
    //                                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        table1.Cell(0, 2).SetContent("Subject");

    //                                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                        table1.Cell(0, 3).SetContent("Mark");
    //                                        for (int fg = 0; fg < 25; fg++)
    //                                        {
    //                                            ro2++;
    //                                            table1.Cell(ro2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
    //                                            table2_rollsplit = split_roll[val2];
    //                                            table1.Cell(ro2, 0).SetContent(table2_rollsplit);


    //                                            table1.Cell(ro2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                            string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
    //                                            table2_stud_split = split_stu_tb2[val2];
    //                                            table1.Cell(ro2, 1).SetContent(table2_stud_split);


    //                                            table1.Cell(ro2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
    //                                            table2_Sbj_split = split_sub_code[val2];
    //                                            table1.Cell(ro2, 2).SetContent(table2_Sbj_split);

    //                                            table1.Cell(ro2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                            string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
    //                                            table2_mark_split = splitsub_mark[val2];
    //                                            table1.Cell(ro2, 3).SetContent(table2_mark_split);
    //                                            val2++;

    //                                        }

    //                                    }
    //                                }

    //                            }
    //                            else
    //                            {

    //                            }
    //                        }
    //                        if (enter_page == 1)
    //                        {
    //                            Gios.Pdf.PdfTablePage newpdftabpage2 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 50, 500, 700, 1000));
    //                            mypdfpage.Add(newpdftabpage2);
    //                        }
    //                        else
    //                        {
    //                            Gios.Pdf.PdfTablePage newpdftabpage2 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 50, 100, 700, 1000));
    //                            mypdfpage.Add(newpdftabpage2);
    //                        }

    //                    }
    //                    else
    //                    {
    //                        if (remaindsubs < 15)
    //                        {
    //                            table = mydocument.NewTable(Fontsmall, remaindsubs + 1, tab1_count, 1);
    //                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

    //                            table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
    //                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, 0).SetContent("Rank");
    //                            table.Columns[0].SetWidth(100);

    //                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, 1).SetContent("Roll No");
    //                            table.Columns[1].SetWidth(180);

    //                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, 2).SetContent("Student Name");
    //                            table.Columns[2].SetWidth(250);
    //                            int valrem = 3;
    //                            for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
    //                            {

    //                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] splitsb_no = subj_code.Split(new Char[] { '\n' });
    //                                sub_new_code = splitsb_no[i];
    //                                table.Cell(0, valrem).SetContent(sub_new_code);
    //                                table.Columns[valrem].SetWidth(200);
    //                                valrem++;
    //                            }

    //                            //int next_val = ds2.Tables[1].Rows.Count + 3;
    //                            //int next_val1 = ds2.Tables[1].Rows.Count + 4;

    //                            table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, valrem).SetContent("Total");
    //                            table.Columns[valrem].SetWidth(200);
    //                            valrem++;

    //                            table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, valrem).SetContent("Percentage");
    //                            table.Columns[valrem].SetWidth(200);
    //                            valrem++;

    //                            table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, valrem).SetContent("Photo");
    //                            table.Columns[valrem].SetWidth(200);
    //                            for (int fg = 0; fg < remaindsubs; fg++)
    //                            {
    //                                ro++;
    //                                int row_cnt = 0;
    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] splitRank = Rank.Split(new Char[] { '\n' });
    //                                rank_spli = splitRank[val1];
    //                                table.Cell(ro, 0).SetContent(rank_spli);
    //                                row_cnt++;

    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] splitRoll_No = stude_RollNumber.Split(new Char[] { '\n' });
    //                                roll_split = splitRoll_No[val1];
    //                                table.Cell(ro, row_cnt).SetContent(roll_split);
    //                                row_cnt++;

    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                string[] split_Stud_Name = stud_Nameof.Split(new Char[] { '\n' });
    //                                stud_split = split_Stud_Name[val1];
    //                                table.Cell(ro, row_cnt).SetContent(stud_split);
    //                                row_cnt++;

    //                                for (int i1 = 0; i1 < ds2.Tables[1].Rows.Count; i1++)
    //                                {
    //                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    string[] split_Mark = mark_obt.Split(new Char[] { '\n' });
    //                                    mark_split = split_Mark[val1];
    //                                    table.Cell(ro, row_cnt).SetContent(mark_split);
    //                                    row_cnt++;

    //                                }
    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] split_Total = Total_Mark.Split(new Char[] { '\n' });
    //                                total_split = split_Total[val1];
    //                                table.Cell(ro, row_cnt).SetContent(total_split);
    //                                row_cnt++;

    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] split_Per = Pertc.Split(new Char[] { '\n' });
    //                                per_split = split_Per[val1];
    //                                table.Cell(ro, row_cnt).SetContent(per_split);
    //                                row_cnt++;
    //                                //Aruna 17apr2013 Add Student Photo===================================================
    //                                MemoryStream memoryStream = new MemoryStream();
    //                                SqlCommand cmd = new SqlCommand();
    //                                con.Close();
    //                                con.Open();
    //                                cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + roll_split + "')";
    //                                cmd.Connection = con;

    //                                SqlDataReader MyReader = cmd.ExecuteReader();
    //                                if (MyReader.Read())
    //                                {

    //                                    byte[] file = (byte[])MyReader["photo"];
    //                                    MyReader.Close();
    //                                    memoryStream.Write(file, 0, file.Length);
    //                                    if (file.Length > 0)
    //                                    {
    //                                        //System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, true, true);                                                          
    //                                        //img.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
    //                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
    //                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")) == false)
    //                                        {
    //                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                        }

    //                                    }
    //                                    memoryStream.Dispose();
    //                                    memoryStream.Close();
    //                                    MyReader.Close();
    //                                }
    //                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")))
    //                                {
    //                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"));
    //                                    //table.Cell(val1, row_cnt).SetContent(leftimage);
    //                                    mypdfpage.Add(leftimage, 685, 25, 300);

    //                                }
    //                                val1++;

    //                            }

    //                        }
    //                        else
    //                        {
    //                            table = mydocument.NewTable(Fontsmall, 15 + 1, tab1_count, 1);
    //                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

    //                            table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
    //                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, 0).SetContent("Rank");
    //                            table.Columns[0].SetWidth(100);

    //                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, 1).SetContent("Roll No");
    //                            table.Columns[1].SetWidth(180);

    //                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, 2).SetContent("Student Name");
    //                            table.Columns[2].SetWidth(250);
    //                            int valrem = 3;
    //                            for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
    //                            {

    //                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] splitsb_no = subj_code.Split(new Char[] { '\n' });
    //                                sub_new_code = splitsb_no[i];
    //                                table.Cell(0, valrem).SetContent(sub_new_code);
    //                                table.Columns[valrem].SetWidth(200);
    //                                valrem++;
    //                            }

    //                            //int next_val = ds2.Tables[1].Rows.Count + 3;
    //                            //int next_val1 = ds2.Tables[1].Rows.Count + 4;

    //                            table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, valrem).SetContent("Total");
    //                            table.Columns[valrem].SetWidth(200);
    //                            valrem++;

    //                            table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, valrem).SetContent("Percentage");
    //                            table.Columns[valrem].SetWidth(200);
    //                            valrem++;

    //                            table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                            table.Cell(0, valrem).SetContent("Photo");
    //                            table.Columns[valrem].SetWidth(200);
    //                            for (int fg = 0; fg < 15; fg++)
    //                            {
    //                                ro++;
    //                                int row_cnt = 0;
    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] splitRank = Rank.Split(new Char[] { '\n' });
    //                                rank_spli = splitRank[val1];
    //                                table.Cell(ro, 0).SetContent(rank_spli);
    //                                row_cnt++;

    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] splitRoll_No = stude_RollNumber.Split(new Char[] { '\n' });
    //                                roll_split = splitRoll_No[val1];
    //                                table.Cell(ro, row_cnt).SetContent(roll_split);
    //                                row_cnt++;

    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                                string[] split_Stud_Name = stud_Nameof.Split(new Char[] { '\n' });
    //                                stud_split = split_Stud_Name[val1];
    //                                table.Cell(ro, row_cnt).SetContent(stud_split);
    //                                row_cnt++;

    //                                for (int i1 = 0; i1 < ds2.Tables[1].Rows.Count; i1++)
    //                                {
    //                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    string[] split_Mark = mark_obt.Split(new Char[] { '\n' });
    //                                    mark_split = split_Mark[val1];
    //                                    table.Cell(ro, row_cnt).SetContent(mark_split);
    //                                    row_cnt++;

    //                                }
    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] split_Total = Total_Mark.Split(new Char[] { '\n' });
    //                                total_split = split_Total[val1];
    //                                table.Cell(ro, row_cnt).SetContent(total_split);
    //                                row_cnt++;

    //                                table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                string[] split_Per = Pertc.Split(new Char[] { '\n' });
    //                                per_split = split_Per[val1];
    //                                table.Cell(ro, row_cnt).SetContent(per_split);
    //                                row_cnt++;
    //                                //Aruna 17apr2013 Add Student Photo===================================================
    //                                MemoryStream memoryStream = new MemoryStream();
    //                                SqlCommand cmd = new SqlCommand();
    //                                con.Close();
    //                                con.Open();
    //                                cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + roll_split + "')";
    //                                cmd.Connection = con;

    //                                SqlDataReader MyReader = cmd.ExecuteReader();
    //                                if (MyReader.Read())
    //                                {

    //                                    byte[] file = (byte[])MyReader["photo"];
    //                                    MyReader.Close();
    //                                    memoryStream.Write(file, 0, file.Length);
    //                                    if (file.Length > 0)
    //                                    {
    //                                        //System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, true, true);                                                          
    //                                        //img.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
    //                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
    //                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")) == false)
    //                                        {
    //                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                        }

    //                                    }
    //                                    memoryStream.Dispose();
    //                                    memoryStream.Close();
    //                                    MyReader.Close();
    //                                }
    //                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")))
    //                                {
    //                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                                    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"));
    //                                    //table.Cell(val1, row_cnt).SetContent(leftimage);
    //                                    mypdfpage.Add(leftimage, 685, 25, 300);

    //                                }
    //                                val1++;

    //                            }
    //                        }
    //                    }


    //                }

    //                if (enter_page == 1)
    //                {

    //                    Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 50, 250, 700, 1000));
    //                    mypdfpage.Add(newpdftabpage);
    //                }

    //                if (row == final_count_tb4)
    //                {
    //                    int val_tb3 = 0;
    //                    int cnt_finaltb = snotb3;
    //                    Gios.Pdf.PdfTable table3 = mydocument.NewTable(Fontsmall, cnt_finaltb + 1, 5, 1);
    //                    table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

    //                    table3.Columns[0].SetWidth(150);
    //                    table3.Columns[1].SetWidth(180);
    //                    table3.Columns[2].SetWidth(200);
    //                    table3.Columns[3].SetWidth(100);
    //                    table3.Columns[4].SetWidth(100);

    //                    table3.CellRange(0, 0, 0, 4).SetFont(Fontbold);
    //                    table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table3.Cell(0, 0).SetContent("Subject Code");
    //                    table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table3.Cell(0, 1).SetContent("Subject Name");
    //                    table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table3.Cell(0, 2).SetContent("Staff Incharge");

    //                    table3.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table3.Cell(0, 3).SetContent("Pass%");
    //                    table3.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                    table3.Cell(0, 4).SetContent("Avg%");

    //                    for (int i = 0; i < cnt_finaltb; i++)
    //                    {
    //                        val_tb3++;
    //                        string table3_Subj_code = "";
    //                        string table3_Subj_Name = "";
    //                        string table3_Staff_Inc = "";
    //                        string table3_Pass_stud = "";
    //                        string table3_Avg = "";

    //                        table3.Cell(val_tb3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        string[] split_subj_tb = table3_subj_code.Split(new Char[] { '\n' });
    //                        table3_Subj_code = split_subj_tb[i];
    //                        table3.Cell(val_tb3, 0).SetContent(table3_Subj_code);


    //                        table3.Cell(val_tb3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        string[] splitb3_subjname = table3_subj_name.Split(new Char[] { '\n' });
    //                        table3_Subj_Name = splitb3_subjname[i];
    //                        table3.Cell(val_tb3, 1).SetContent(table3_Subj_Name);


    //                        table3.Cell(val_tb3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
    //                        string[] splitb3_staff_inc = table3_staff_inc.Split(new Char[] { '\n' });
    //                        table3_Staff_Inc = splitb3_staff_inc[i];
    //                        table3.Cell(val_tb3, 2).SetContent(table3_Staff_Inc);

    //                        table3.Cell(val_tb3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        string[] splitb3_pass = table3_Pass.Split(new Char[] { '\n' });
    //                        table3_Pass_stud = splitb3_pass[i];
    //                        table3.Cell(val_tb3, 3).SetContent(table3_Pass_stud);

    //                        table3.Cell(val_tb3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
    //                        string[] splitb3_avg = table4_Avg.Split(new Char[] { '\n' });
    //                        table3_Avg = splitb3_avg[i];
    //                        table3.Cell(val_tb3, 4).SetContent(table3_Avg);
    //                    }

    //                    Gios.Pdf.PdfTablePage newpdftabpagetb3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 50, 750, 700, 1000));
    //                    mypdfpage.Add(newpdftabpagetb3);
    //                }




    //                string appPath = HttpContext.Current.Server.MapPath("~");
    //                if (appPath != "")
    //                {
    //                    lblnorec.Visible = false;
    //                    lblnorec.Text = "";
    //                    //Sankar on 20May2013============================
    //                    string szPath = appPath + "/Report/";
    //                    string szFile = "Format1.pdf";
    //                    mypdfpage.SaveToDocument();
    //                    //mypdfpage1.SaveToDocument();
    //                    mydocument.SaveToFile(szPath + szFile);
    //                    Response.ClearHeaders();
    //                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
    //                    Response.ContentType = "application/pdf";
    //                    Response.WriteFile(szPath + szFile);
    //                    //=============================================

    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}
    public void generateletterformat1(Gios.Pdf.PdfDocument mydocument, Font Fontsmall, Font Fontbold, Font Fontbold1, DataTable dt, HttpResponse response)
    {
        try
        {

            int subno = 0;
            int pagecount = sno / 15;
            int repage = sno % 15;

            int nopages = pagecount;
            if (repage > 0)
            {
                nopages++;
            }

            //table2.....................
            int subno2 = 1;
            int pagecount2 = sno2 / 25;
            int repage2 = sno2 % 25;
            int nopages2 = pagecount2;
            if (repage2 > 0)
            {
                nopages2++;
            }

            int final_pagecount = pagecount + pagecount2;
            int repages_Pagecount = repage + repage2;
            int nopages_pagecount = final_pagecount;
            if (repages_Pagecount > 0)
            {
                nopages_pagecount++;
            }

            if (sno2 < 50)
            {
                nopages_pagecount++;
            }

            int final_count_tb4 = nopages_pagecount - 1;

            int tab1_count = ds2.Tables[1].Rows.Count + 6;

            //table2.................
            //int row_cnt = 0;
            string rank_spli = "";
            string roll_split = "";
            string stud_split = "";
            string mark_split = "";
            string total_split = "";
            string per_split = "";
            int subno_mau = 2;
            //int val = 3;
            int ex_count = 0;
            string sub_new_code = "";

            //table2 string value...........
            string table2_rollsplit = "";
            string table2_stud_split = "";
            string table2_Sbj_split = "";
            string table2_mark_split = "";
            if (nopages > 0)
            {
                for (int row = 0; row < nopages_pagecount; row++)
                {
                    int enter_page = 1;
                    subno++;
                    Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();

                    PdfArea tete = new PdfArea(mydocument, 25, 10, 800, 1100);

                    PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);

                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Sankar
                    {
                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 25, 25, 300);
                    }

                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Sankar
                    {
                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 685, 25, 300);
                    }

                    //int col_count = Convert.ToInt32(sub_code) + Convert.ToInt32(srno);

                    //int col_count = sub_code + Convert.ToInt32(srno);
                    //table2........bind
                    //int cnt2 = subno * sno2;
                    //int cnt12 = subno * 25;

                    int val1 = 0;
                    int val2 = 0;

                    int subno_val = 1;
                    int cnt = subno_val * sno;
                    int cnt1 = subno_val * 15;

                    int cnt2 = subno_val * sno2;
                    int cnt12 = subno_val * 25;

                    Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontsmall, cnt2 + 1, 4, 1);
                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                    Gios.Pdf.PdfTable table = mydocument.NewTable(Fontsmall, cnt1 + 1, tab1_count, 1);
                    table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
                    table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 0).SetContent("Rank");
                    table.Columns[0].SetWidth(100);

                    table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 1).SetContent("Roll No");
                    table.Columns[1].SetWidth(180);

                    table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 2).SetContent("Student Name");
                    table.Columns[2].SetWidth(250);
                    int val = 3;
                    //string sub_new_code = "";
                    for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                    {
                        //int val = 3;
                        table.Cell(0, val).SetContentAlignment(ContentAlignment.MiddleCenter);
                        string[] splitsb_no = subj_code.Split(new Char[] { '\n' });
                        sub_new_code = splitsb_no[i];
                        table.Cell(0, val).SetContent(sub_new_code);
                        table.Columns[val].SetWidth(200);
                        val++;
                    }

                    int next_val = ds2.Tables[1].Rows.Count + 3;
                    int next_val1 = ds2.Tables[1].Rows.Count + 4;

                    table.Cell(0, val).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, val).SetContent("Total");
                    table.Columns[val].SetWidth(200);
                    val++;

                    table.Cell(0, val).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, val).SetContent("Percentage");
                    table.Columns[val].SetWidth(200);
                    val++;

                    table.Cell(0, val).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, val).SetContent("Photo");
                    table.Columns[val].SetWidth(200);

                    if (subno == 1)
                    {
                        if (cnt < 15)
                        {


                            PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, 0, 0, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collnamenew1 + "");
                            PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 0, 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + address + "");

                            PdfTextArea pts1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 0, 40, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + phnfax + "");

                            PdfTextArea pts2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 0, 60, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + email + "");

                            PdfTextArea pts3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 0, 80, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "OVERALL BEST PERFORMANCE");
                            PdfTextArea pts5;
                            PdfTextArea pts6;
                            if (Label4.Text.Trim().ToLower() == "school")
                            {
                                pts5 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 0, 100, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Year:  " + ddlBatch.SelectedValue.ToString() + " " + "Standard: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString());
                                pts6 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 0, 120, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Test: " + test + " " + "Term: " + ddlSemYr.SelectedValue.ToString() + "Section: " + ddlSec.SelectedValue.ToString());
                            }
                            else
                            {
                                pts5 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 0, 100, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Batch Year:  " + ddlBatch.SelectedValue.ToString() + " " + "Course: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString());
                                pts6 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 0, 120, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Test: " + test + " " + "Semester: " + ddlSemYr.SelectedValue.ToString() + "Section: " + ddlSec.SelectedValue.ToString());
                            }

                            mypdfpage.Add(ptc);
                            mypdfpage.Add(pts);
                            mypdfpage.Add(pts1);
                            mypdfpage.Add(pts2);
                            mypdfpage.Add(pts3);
                            mypdfpage.Add(pts5);
                            mypdfpage.Add(pts6);

                            table = mydocument.NewTable(Fontsmall, cnt + 1, tab1_count, 1);
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                            table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 0).SetContent("Rank");
                            table.Columns[0].SetWidth(100);

                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 1).SetContent("Roll No");
                            table.Columns[1].SetWidth(100);

                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 2).SetContent("Student Name");
                            table.Columns[2].SetWidth(150);
                            //string sub_new_code = "";
                            int val_new = 3;
                            for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                            {

                                table.Cell(0, val_new).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitsb_no = subj_code.Split(new Char[] { '\n' });
                                sub_new_code = splitsb_no[i];
                                table.Cell(0, val_new).SetContent(sub_new_code);
                                table.Columns[val_new].SetWidth(100);
                                val_new++;
                            }

                            //int next_val = ds2.Tables[1].Rows.Count + 3;
                            //int next_val1 = ds2.Tables[1].Rows.Count + 4;

                            table.Cell(0, val_new).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, val_new).SetContent("Total");
                            table.Columns[val_new].SetWidth(100);
                            val_new++;

                            table.Cell(0, val_new).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, val_new).SetContent("Percentage");
                            table.Columns[val_new].SetWidth(100);
                            val_new++;

                            table.Cell(0, val_new).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, val_new).SetContent("Photo");
                            table.Columns[val_new].SetWidth(100);
                            int imag_cnt = 300;

                            for (int i = 0; i < cnt; i++)
                            {

                                val1++;
                                int row_cnt = 0;
                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitRank = Rank.Split(new Char[] { '\n' });
                                rank_spli = splitRank[i];
                                table.Cell(val1, 0).SetContent(rank_spli);
                                row_cnt++;

                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitRoll_No = stude_RollNumber.Split(new Char[] { '\n' });
                                roll_split = splitRoll_No[i];
                                table.Cell(val1, row_cnt).SetContent(roll_split);
                                row_cnt++;

                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] split_Stud_Name = stud_Nameof.Split(new Char[] { '\n' });
                                stud_split = split_Stud_Name[i];
                                table.Cell(val1, row_cnt).SetContent(stud_split);
                                row_cnt++;
                                int check_staff = 0;

                                string strsection;
                                string subsec = ddlSec.SelectedValue.ToString();
                                if (subsec.ToString() == "All" || subsec.ToString() == "" || subsec.ToString() == "-1")
                                {
                                    strsection = "";
                                }
                                else
                                {
                                    strsection = " and R.sections='" + subsec.ToString() + "'";
                                }

                                for (int i1 = 0; i1 < ds2.Tables[1].Rows.Count; i1++)
                                {
                                    //Edited............................
                                    string sqlStr = "";
                                    sqlStr = "SELECT Marks_Obtained FROM Registration R,Result U,Exam_Type E,Subject S ";
                                    sqlStr = sqlStr + "WHERE R.Roll_No = U.Roll_No AND U.Exam_Code = E.Exam_Code AND E.Subject_No = S.Subject_No ";
                                    sqlStr = sqlStr + "AND R.Degree_Code =" + ddlBranch.SelectedValue.ToString() + " AND R.Batch_year =" + ddlBatch.SelectedValue.ToString() + " AND E.Criteria_No =" + ddlTest.SelectedValue.ToString() + strsection;
                                    sqlStr = sqlStr + "AND RollNo_Flag <> 0 AND CC = 0 AND Exam_Flag <> 'DEBAR' AND DelFlag = 0 ";
                                    sqlStr = sqlStr + " AND U.Roll_No ='" + splitRoll_No[i] + "' ";
                                    sqlStr = sqlStr + "ORDER BY S.Subject_No ";
                                    con.Close();
                                    con.Open();
                                    SqlDataAdapter dt_staff = new SqlDataAdapter(sqlStr, con);
                                    DataTable dr_staff = new DataTable();
                                    dt_staff.Fill(dr_staff);
                                    if (check_staff == 0)
                                    {
                                        if (dr_staff.Rows.Count > 0)
                                        {
                                            for (int j = 0; j < dr_staff.Rows.Count; j++)
                                            {
                                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string staff_mark = "";
                                                staff_mark = dr_staff.Rows[j]["Marks_Obtained"].ToString();
                                                //string[] split_Mark = mark_obt.Split(new Char[] { '\n' });
                                                //mark_split = split_Mark[i];
                                                table.Cell(val1, row_cnt).SetContent(staff_mark);
                                                row_cnt++;
                                                check_staff++;

                                            }

                                        }
                                    }

                                }
                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] split_Total = Total_Mark.Split(new Char[] { '\n' });
                                total_split = split_Total[i];
                                table.Cell(val1, row_cnt).SetContent(total_split);
                                row_cnt++;

                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] split_Per = Pertc.Split(new Char[] { '\n' });
                                per_split = split_Per[i];
                                table.Cell(val1, row_cnt).SetContent(per_split);
                                row_cnt++;
                                //Aruna 17apr2013 Add Student Photo===================================================
                                MemoryStream memoryStream = new MemoryStream();
                                SqlCommand cmd = new SqlCommand();
                                con.Close();
                                con.Open();
                                cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + roll_split + "')";
                                cmd.Connection = con;

                                SqlDataReader MyReader = cmd.ExecuteReader();
                                if (MyReader.Read())
                                {

                                    byte[] file = (byte[])MyReader["photo"];
                                    MyReader.Close();
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        //System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, true, true);                                                          
                                        //img.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(100, 100, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                    MyReader.Close();
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")))
                                {

                                    table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"));
                                    //table.Cell(val1, row_cnt).SetContent(leftimage);
                                    mypdfpage.Add(leftimage, 800, imag_cnt, 300);
                                    // mypdfpage.Add(leftimage,800,imag_cnt,

                                    imag_cnt = imag_cnt + 30;

                                }

                                //Sankar 17apr2013 Add Student Photo===================================================
                            }
                        }
                        else
                        {

                            PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 0, 0, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collnamenew1 + "");
                            PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 0, 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + address + "");

                            PdfTextArea pts1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 0, 40, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + phnfax + "");

                            PdfTextArea pts2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 0, 60, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + email + "");

                            PdfTextArea pts3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 0, 80, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "OVERALL BEST PERFORMANCE");
                            PdfTextArea pts5 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 0, 100, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Batch Year:  " + ddlBatch.SelectedValue.ToString() + " " + "Course: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString());
                            PdfTextArea pts6 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 0, 120, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Test: " + test + " " + "Semester: " + ddlSemYr.SelectedValue.ToString() + "Section: " + ddlSec.SelectedValue.ToString());

                            mypdfpage.Add(ptc);
                            mypdfpage.Add(pts);
                            mypdfpage.Add(pts1);
                            mypdfpage.Add(pts2);
                            mypdfpage.Add(pts3);
                            mypdfpage.Add(pts5);
                            mypdfpage.Add(pts6);

                            for (int i = 0; i < cnt1; i++)
                            {

                                val1++;

                                int row_cnt = 0;
                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitRank = Rank.Split(new Char[] { '\n' });
                                rank_spli = splitRank[i];
                                table.Cell(val1, 0).SetContent(rank_spli);
                                row_cnt++;

                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] splitRoll_No = stude_RollNumber.Split(new Char[] { '\n' });
                                roll_split = splitRoll_No[i];
                                table.Cell(val1, row_cnt).SetContent(roll_split);
                                row_cnt++;

                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleLeft);
                                string[] split_Stud_Name = stud_Nameof.Split(new Char[] { '\n' });
                                stud_split = split_Stud_Name[i];
                                table.Cell(val1, row_cnt).SetContent(stud_split);
                                row_cnt++;

                                for (int i1 = 0; i1 < ds2.Tables[1].Rows.Count; i1++)
                                {
                                    table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] split_Mark = mark_obt.Split(new Char[] { '\n' });
                                    mark_split = split_Mark[i];
                                    table.Cell(val1, row_cnt).SetContent(mark_split);
                                    row_cnt++;

                                }
                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] split_Total = Total_Mark.Split(new Char[] { '\n' });
                                total_split = split_Total[i];
                                table.Cell(val1, row_cnt).SetContent(total_split);
                                row_cnt++;

                                table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                string[] split_Per = Pertc.Split(new Char[] { '\n' });
                                per_split = split_Per[i];
                                table.Cell(val1, row_cnt).SetContent(per_split);
                                row_cnt++;
                                //Aruna 17apr2013 Add Student Photo===================================================
                                MemoryStream memoryStream = new MemoryStream();
                                SqlCommand cmd = new SqlCommand();
                                con.Close();
                                con.Open();
                                cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + roll_split + "')";
                                cmd.Connection = con;

                                SqlDataReader MyReader = cmd.ExecuteReader();
                                if (MyReader.Read())
                                {

                                    byte[] file = (byte[])MyReader["photo"];
                                    MyReader.Close();
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        //System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, true, true);                                                          
                                        //img.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(50, 50, null, IntPtr.Zero);
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")) == false)
                                        {
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }


                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                    MyReader.Close();
                                }

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")))
                                {
                                    int imag_cnt = 260;
                                    table.Cell(val1, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"));
                                    //table.Cell(val1, row_cnt).SetContent(leftimage);
                                    mypdfpage.Add(leftimage, 800, imag_cnt, 100);
                                    //int imag_cnt = imag_cnt + 20;
                                    imag_cnt++;
                                }

                                //Sankar 17apr2013 Add Student Photo==================================================
                            }
                        }
                    }
                    if (subno > 1)
                    {
                        val1 = (subno - 1) * 15;
                        int ro = 0;

                        int remaindsubs = sno - val1;

                        if (remaindsubs < 7)
                        {
                            if (remaindsubs > 0)
                            {
                                table = mydocument.NewTable(Fontsmall, remaindsubs + 1, tab1_count, 1);
                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 0).SetContent("Rank");
                                table.Columns[0].SetWidth(100);

                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 1).SetContent("Roll No");
                                table.Columns[1].SetWidth(180);

                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 2).SetContent("Student Name");
                                table.Columns[2].SetWidth(250);
                                int valrem = 3;
                                for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                                {

                                    table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] splitsb_no = subj_code.Split(new Char[] { '\n' });
                                    sub_new_code = splitsb_no[i];
                                    table.Cell(0, valrem).SetContent(sub_new_code);
                                    table.Columns[valrem].SetWidth(200);
                                    valrem++;
                                }

                                //int next_val = ds2.Tables[1].Rows.Count + 3;
                                //int next_val1 = ds2.Tables[1].Rows.Count + 4;

                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, valrem).SetContent("Total");
                                table.Columns[valrem].SetWidth(200);
                                valrem++;

                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, valrem).SetContent("Percentage");
                                table.Columns[valrem].SetWidth(200);
                                valrem++;

                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, valrem).SetContent("Photo");
                                table.Columns[valrem].SetWidth(200);
                                for (int fg = 0; fg < remaindsubs; fg++)
                                {
                                    ro++;
                                    int row_cnt = 0;
                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] splitRank = Rank.Split(new Char[] { '\n' });
                                    rank_spli = splitRank[val1];
                                    table.Cell(ro, 0).SetContent(rank_spli);
                                    row_cnt++;

                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] splitRoll_No = stude_RollNumber.Split(new Char[] { '\n' });
                                    roll_split = splitRoll_No[val1];
                                    table.Cell(ro, row_cnt).SetContent(roll_split);
                                    row_cnt++;

                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    string[] split_Stud_Name = stud_Nameof.Split(new Char[] { '\n' });
                                    stud_split = split_Stud_Name[val1];
                                    table.Cell(ro, row_cnt).SetContent(stud_split);
                                    row_cnt++;

                                    for (int i1 = 0; i1 < ds2.Tables[1].Rows.Count; i1++)
                                    {
                                        table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        string[] split_Mark = mark_obt.Split(new Char[] { '\n' });
                                        mark_split = split_Mark[val1];
                                        table.Cell(ro, row_cnt).SetContent(mark_split);
                                        row_cnt++;

                                    }
                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] split_Total = Total_Mark.Split(new Char[] { '\n' });
                                    total_split = split_Total[val1];
                                    table.Cell(ro, row_cnt).SetContent(total_split);
                                    row_cnt++;

                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] split_Per = Pertc.Split(new Char[] { '\n' });
                                    per_split = split_Per[val1];
                                    table.Cell(ro, row_cnt).SetContent(per_split);
                                    row_cnt++;
                                    //Aruna 17apr2013 Add Student Photo===================================================
                                    MemoryStream memoryStream = new MemoryStream();
                                    SqlCommand cmd = new SqlCommand();
                                    con.Close();
                                    con.Open();
                                    cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + roll_split + "')";
                                    cmd.Connection = con;

                                    SqlDataReader MyReader = cmd.ExecuteReader();
                                    if (MyReader.Read())
                                    {

                                        byte[] file = (byte[])MyReader["photo"];
                                        MyReader.Close();
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            //System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, true, true);                                                          
                                            //img.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")) == false)
                                            {
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }

                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                        MyReader.Close();
                                    }
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")))
                                    {
                                        table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"));
                                        //table.Cell(val1, row_cnt).SetContent(leftimage);
                                        mypdfpage.Add(leftimage, 685, 25, 300);

                                    }
                                    val1++;

                                }

                                //table2 bind
                                table1 = mydocument.NewTable(Fontsmall, cnt12 + 1, 4, 1);
                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                table1.Columns[0].SetWidth(50);
                                table1.Columns[1].SetWidth(100);
                                table1.Columns[2].SetWidth(100);
                                table1.Columns[3].SetWidth(100);
                                table1.CellRange(0, 0, 0, 3).SetFont(Fontbold);
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 0).SetContent("Roll No");
                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 1).SetContent("Student Name");
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 2).SetContent("Subject");

                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 3).SetContent("Mark");
                                //int val2 = 0;
                                if (cnt2 < 25)
                                {
                                    for (int i = 0; i < cnt2; i++)
                                    {
                                        val2++;
                                        table1.Cell(val2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
                                        table2_rollsplit = split_roll[i];
                                        table1.Cell(val2, 0).SetContent(table2_rollsplit);


                                        table1.Cell(val2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
                                        table2_stud_split = split_stu_tb2[i];
                                        table1.Cell(val2, 1).SetContent(table2_stud_split);


                                        table1.Cell(val2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
                                        table2_Sbj_split = split_sub_code[i];
                                        table1.Cell(val2, 2).SetContent(table2_Sbj_split);

                                        table1.Cell(val2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
                                        table2_mark_split = splitsub_mark[i];
                                        table1.Cell(val2, 3).SetContent(table2_mark_split);


                                    }
                                    //subno_mau++;
                                }
                                else
                                {
                                    for (int i = 0; i < cnt12; i++)
                                    {
                                        val2++;
                                        table1.Cell(val2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
                                        table2_rollsplit = split_roll[i];
                                        table1.Cell(val2, 0).SetContent(table2_rollsplit);


                                        table1.Cell(val2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
                                        table2_stud_split = split_stu_tb2[i];
                                        table1.Cell(val2, 1).SetContent(table2_stud_split);


                                        table1.Cell(val2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
                                        table2_Sbj_split = split_sub_code[i];
                                        table1.Cell(val2, 2).SetContent(table2_Sbj_split);

                                        table1.Cell(val2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
                                        table2_mark_split = splitsub_mark[i];
                                        table1.Cell(val2, 3).SetContent(table2_mark_split);

                                    }

                                    //subno_mau++;
                                    ex_count++;
                                }

                            }
                            else
                            {
                                if (ex_count == 0)
                                {
                                    val2 = 0;
                                }
                                else
                                {
                                    val2 = (subno_mau - 1) * 25;
                                    subno_mau++;
                                }
                                //val2 = (subno_mau - 1) * 25;                      
                                int ro2 = 0;
                                int remaindsubs1 = sno2 - val2;

                                ex_count++;
                                if (remaindsubs1 > 0)
                                {
                                    if (cnt2 == 10)
                                    {
                                        //table2 full binding........................
                                        enter_page = 0;
                                        table1.Columns[0].SetWidth(50);
                                        table1.Columns[1].SetWidth(100);
                                        table1.Columns[2].SetWidth(100);
                                        table1.Columns[3].SetWidth(100);
                                        table1.CellRange(0, 0, 0, 3).SetFont(Fontbold);
                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1.Cell(0, 0).SetContent("Roll No");
                                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1.Cell(0, 1).SetContent("Student Name");
                                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1.Cell(0, 2).SetContent("Subject");

                                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1.Cell(0, 3).SetContent("Mark");

                                        if (cnt2 < 25)
                                        {
                                            for (int i = 0; i < cnt2; i++)
                                            {
                                                val2++;
                                                table1.Cell(val2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
                                                table2_rollsplit = split_roll[i];
                                                table1.Cell(val2, 0).SetContent(table2_rollsplit);

                                                table1.Cell(val2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
                                                table2_stud_split = split_stu_tb2[i];
                                                table1.Cell(val2, 1).SetContent(table2_stud_split);

                                                table1.Cell(val2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
                                                table2_Sbj_split = split_sub_code[i];
                                                table1.Cell(val2, 2).SetContent(table2_Sbj_split);

                                                table1.Cell(val2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
                                                table2_mark_split = splitsub_mark[i];
                                                table1.Cell(val2, 3).SetContent(table2_mark_split);
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 0; i < cnt12; i++)
                                            {
                                                val2++;
                                                table1.Cell(val2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
                                                table2_rollsplit = split_roll[i];
                                                table1.Cell(val2, 0).SetContent(table2_rollsplit);


                                                table1.Cell(val2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
                                                table2_stud_split = split_stu_tb2[i];
                                                table1.Cell(val2, 1).SetContent(table2_stud_split);


                                                table1.Cell(val2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
                                                table2_Sbj_split = split_sub_code[i];
                                                table1.Cell(val2, 2).SetContent(table2_Sbj_split);

                                                table1.Cell(val2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
                                                table2_mark_split = splitsub_mark[i];
                                                table1.Cell(val2, 3).SetContent(table2_mark_split);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (remaindsubs1 < 25)
                                        {
                                            table1 = mydocument.NewTable(Fontsmall, remaindsubs1 + 1, 4, 1);
                                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            enter_page = 0;
                                            table1.Columns[0].SetWidth(50);
                                            table1.Columns[1].SetWidth(100);
                                            table1.Columns[2].SetWidth(100);
                                            table1.Columns[3].SetWidth(100);
                                            table1.CellRange(0, 0, 0, 3).SetFont(Fontbold);
                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 0).SetContent("Roll No");
                                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 1).SetContent("Student Name");
                                            table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 2).SetContent("Subject");

                                            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 3).SetContent("Mark");
                                            for (int fg = 0; fg < remaindsubs1; fg++)
                                            {
                                                ro2++;
                                                table1.Cell(ro2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
                                                table2_rollsplit = split_roll[val2];
                                                table1.Cell(ro2, 0).SetContent(table2_rollsplit);


                                                table1.Cell(ro2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
                                                table2_stud_split = split_stu_tb2[val2];
                                                table1.Cell(ro2, 1).SetContent(table2_stud_split);


                                                table1.Cell(ro2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
                                                table2_Sbj_split = split_sub_code[val2];
                                                table1.Cell(ro2, 2).SetContent(table2_Sbj_split);

                                                table1.Cell(ro2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
                                                table2_mark_split = splitsub_mark[val2];
                                                table1.Cell(ro2, 3).SetContent(table2_mark_split);
                                                val2++;

                                            }


                                        }
                                        else
                                        {
                                            table1 = mydocument.NewTable(Fontsmall, 25 + 1, 4, 1);
                                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            enter_page = 0;
                                            table1.Columns[0].SetWidth(50);
                                            table1.Columns[1].SetWidth(100);
                                            table1.Columns[2].SetWidth(100);
                                            table1.Columns[3].SetWidth(100);
                                            table1.CellRange(0, 0, 0, 3).SetFont(Fontbold);
                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 0).SetContent("Roll No");
                                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 1).SetContent("Student Name");
                                            table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 2).SetContent("Subject");

                                            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 3).SetContent("Mark");
                                            for (int fg = 0; fg < 25; fg++)
                                            {
                                                ro2++;
                                                table1.Cell(ro2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] split_roll = table2_Roll_No.Split(new Char[] { '\n' });
                                                table2_rollsplit = split_roll[val2];
                                                table1.Cell(ro2, 0).SetContent(table2_rollsplit);


                                                table1.Cell(ro2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                string[] split_stu_tb2 = table2_Stud_Name.Split(new Char[] { '\n' });
                                                table2_stud_split = split_stu_tb2[val2];
                                                table1.Cell(ro2, 1).SetContent(table2_stud_split);


                                                table1.Cell(ro2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] split_sub_code = table2_Subj_code.Split(new Char[] { '\n' });
                                                table2_Sbj_split = split_sub_code[val2];
                                                table1.Cell(ro2, 2).SetContent(table2_Sbj_split);

                                                table1.Cell(ro2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                string[] splitsub_mark = table2_Mark.Split(new Char[] { '\n' });
                                                table2_mark_split = splitsub_mark[val2];
                                                table1.Cell(ro2, 3).SetContent(table2_mark_split);
                                                val2++;

                                            }

                                        }
                                    }

                                }
                                else
                                {

                                }
                            }
                            if (enter_page == 1)
                            {
                                Gios.Pdf.PdfTablePage newpdftabpage2 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 50, 500, 700, 1000));
                                mypdfpage.Add(newpdftabpage2);
                            }
                            else
                            {
                                Gios.Pdf.PdfTablePage newpdftabpage2 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 50, 100, 700, 1000));
                                mypdfpage.Add(newpdftabpage2);
                            }

                        }
                        else
                        {
                            if (remaindsubs < 15)
                            {
                                table = mydocument.NewTable(Fontsmall, remaindsubs + 1, tab1_count, 1);
                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 0).SetContent("Rank");
                                table.Columns[0].SetWidth(100);

                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 1).SetContent("Roll No");
                                table.Columns[1].SetWidth(180);

                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 2).SetContent("Student Name");
                                table.Columns[2].SetWidth(250);
                                int valrem = 3;
                                for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                                {

                                    table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] splitsb_no = subj_code.Split(new Char[] { '\n' });
                                    sub_new_code = splitsb_no[i];
                                    table.Cell(0, valrem).SetContent(sub_new_code);
                                    table.Columns[valrem].SetWidth(200);
                                    valrem++;
                                }

                                //int next_val = ds2.Tables[1].Rows.Count + 3;
                                //int next_val1 = ds2.Tables[1].Rows.Count + 4;

                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, valrem).SetContent("Total");
                                table.Columns[valrem].SetWidth(200);
                                valrem++;

                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, valrem).SetContent("Percentage");
                                table.Columns[valrem].SetWidth(200);
                                valrem++;

                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, valrem).SetContent("Photo");
                                table.Columns[valrem].SetWidth(200);
                                for (int fg = 0; fg < remaindsubs; fg++)
                                {
                                    ro++;
                                    int row_cnt = 0;
                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] splitRank = Rank.Split(new Char[] { '\n' });
                                    rank_spli = splitRank[val1];
                                    table.Cell(ro, 0).SetContent(rank_spli);
                                    row_cnt++;

                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] splitRoll_No = stude_RollNumber.Split(new Char[] { '\n' });
                                    roll_split = splitRoll_No[val1];
                                    table.Cell(ro, row_cnt).SetContent(roll_split);
                                    row_cnt++;

                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    string[] split_Stud_Name = stud_Nameof.Split(new Char[] { '\n' });
                                    stud_split = split_Stud_Name[val1];
                                    table.Cell(ro, row_cnt).SetContent(stud_split);
                                    row_cnt++;

                                    for (int i1 = 0; i1 < ds2.Tables[1].Rows.Count; i1++)
                                    {
                                        table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        string[] split_Mark = mark_obt.Split(new Char[] { '\n' });
                                        mark_split = split_Mark[val1];
                                        table.Cell(ro, row_cnt).SetContent(mark_split);
                                        row_cnt++;

                                    }
                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] split_Total = Total_Mark.Split(new Char[] { '\n' });
                                    total_split = split_Total[val1];
                                    table.Cell(ro, row_cnt).SetContent(total_split);
                                    row_cnt++;

                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] split_Per = Pertc.Split(new Char[] { '\n' });
                                    per_split = split_Per[val1];
                                    table.Cell(ro, row_cnt).SetContent(per_split);
                                    row_cnt++;
                                    //Aruna 17apr2013 Add Student Photo===================================================
                                    MemoryStream memoryStream = new MemoryStream();
                                    SqlCommand cmd = new SqlCommand();
                                    con.Close();
                                    con.Open();
                                    cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + roll_split + "')";
                                    cmd.Connection = con;

                                    SqlDataReader MyReader = cmd.ExecuteReader();
                                    if (MyReader.Read())
                                    {

                                        byte[] file = (byte[])MyReader["photo"];
                                        MyReader.Close();
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            //System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, true, true);                                                          
                                            //img.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")) == false)
                                            {
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }

                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                        MyReader.Close();
                                    }
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")))
                                    {
                                        table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"));
                                        //table.Cell(val1, row_cnt).SetContent(leftimage);
                                        mypdfpage.Add(leftimage, 685, 25, 300);

                                    }
                                    val1++;

                                }

                            }
                            else
                            {
                                table = mydocument.NewTable(Fontsmall, 15 + 1, tab1_count, 1);
                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                table.CellRange(0, 0, 0, 4).SetFont(Fontbold);
                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 0).SetContent("Rank");
                                table.Columns[0].SetWidth(100);

                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 1).SetContent("Roll No");
                                table.Columns[1].SetWidth(180);

                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 2).SetContent("Student Name");
                                table.Columns[2].SetWidth(250);
                                int valrem = 3;
                                for (int i = 0; i < ds2.Tables[1].Rows.Count; i++)
                                {

                                    table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] splitsb_no = subj_code.Split(new Char[] { '\n' });
                                    sub_new_code = splitsb_no[i];
                                    table.Cell(0, valrem).SetContent(sub_new_code);
                                    table.Columns[valrem].SetWidth(200);
                                    valrem++;
                                }

                                //int next_val = ds2.Tables[1].Rows.Count + 3;
                                //int next_val1 = ds2.Tables[1].Rows.Count + 4;

                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, valrem).SetContent("Total");
                                table.Columns[valrem].SetWidth(200);
                                valrem++;

                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, valrem).SetContent("Percentage");
                                table.Columns[valrem].SetWidth(200);
                                valrem++;

                                table.Cell(0, valrem).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, valrem).SetContent("Photo");
                                table.Columns[valrem].SetWidth(200);
                                for (int fg = 0; fg < 15; fg++)
                                {
                                    ro++;
                                    int row_cnt = 0;
                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] splitRank = Rank.Split(new Char[] { '\n' });
                                    rank_spli = splitRank[val1];
                                    table.Cell(ro, 0).SetContent(rank_spli);
                                    row_cnt++;

                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] splitRoll_No = stude_RollNumber.Split(new Char[] { '\n' });
                                    roll_split = splitRoll_No[val1];
                                    table.Cell(ro, row_cnt).SetContent(roll_split);
                                    row_cnt++;

                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    string[] split_Stud_Name = stud_Nameof.Split(new Char[] { '\n' });
                                    stud_split = split_Stud_Name[val1];
                                    table.Cell(ro, row_cnt).SetContent(stud_split);
                                    row_cnt++;

                                    for (int i1 = 0; i1 < ds2.Tables[1].Rows.Count; i1++)
                                    {
                                        table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        string[] split_Mark = mark_obt.Split(new Char[] { '\n' });
                                        mark_split = split_Mark[val1];
                                        table.Cell(ro, row_cnt).SetContent(mark_split);
                                        row_cnt++;

                                    }
                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] split_Total = Total_Mark.Split(new Char[] { '\n' });
                                    total_split = split_Total[val1];
                                    table.Cell(ro, row_cnt).SetContent(total_split);
                                    row_cnt++;

                                    table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    string[] split_Per = Pertc.Split(new Char[] { '\n' });
                                    per_split = split_Per[val1];
                                    table.Cell(ro, row_cnt).SetContent(per_split);
                                    row_cnt++;
                                    //Aruna 17apr2013 Add Student Photo===================================================
                                    MemoryStream memoryStream = new MemoryStream();
                                    SqlCommand cmd = new SqlCommand();
                                    con.Close();
                                    con.Open();
                                    cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + roll_split + "')";
                                    cmd.Connection = con;

                                    SqlDataReader MyReader = cmd.ExecuteReader();
                                    if (MyReader.Read())
                                    {

                                        byte[] file = (byte[])MyReader["photo"];
                                        MyReader.Close();
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            //System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, true, true);                                                          
                                            //img.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")) == false)
                                            {
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }

                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                        MyReader.Close();
                                    }
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg")))
                                    {
                                        table.Cell(ro, row_cnt).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll_split + ".jpeg"));
                                        //table.Cell(val1, row_cnt).SetContent(leftimage);
                                        mypdfpage.Add(leftimage, 685, 25, 300);

                                    }
                                    val1++;

                                }
                            }
                        }


                    }

                    if (enter_page == 1)
                    {

                        Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 50, 250, 700, 1000));
                        mypdfpage.Add(newpdftabpage);
                    }

                    if (row == final_count_tb4)
                    {
                        int val_tb3 = 0;
                        int cnt_finaltb = snotb3;
                        Gios.Pdf.PdfTable table3 = mydocument.NewTable(Fontsmall, cnt_finaltb + 1, 5, 1);
                        table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                        table3.Columns[0].SetWidth(150);
                        table3.Columns[1].SetWidth(180);
                        table3.Columns[2].SetWidth(200);
                        table3.Columns[3].SetWidth(100);
                        table3.Columns[4].SetWidth(100);

                        table3.CellRange(0, 0, 0, 4).SetFont(Fontbold);
                        table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table3.Cell(0, 0).SetContent("Subject Code");
                        table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table3.Cell(0, 1).SetContent("Subject Name");
                        table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table3.Cell(0, 2).SetContent("Staff Incharge");

                        table3.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table3.Cell(0, 3).SetContent("Pass%");
                        table3.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table3.Cell(0, 4).SetContent("Avg%");

                        for (int i = 0; i < cnt_finaltb; i++)
                        {
                            val_tb3++;
                            string table3_Subj_code = "";
                            string table3_Subj_Name = "";
                            string table3_Staff_Inc = "";
                            string table3_Pass_stud = "";
                            string table3_Avg = "";

                            table3.Cell(val_tb3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            string[] split_subj_tb = table3_subj_code.Split(new Char[] { '\n' });
                            table3_Subj_code = split_subj_tb[i];
                            table3.Cell(val_tb3, 0).SetContent(table3_Subj_code);


                            table3.Cell(val_tb3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            string[] splitb3_subjname = table3_subj_name.Split(new Char[] { '\n' });
                            table3_Subj_Name = splitb3_subjname[i];
                            table3.Cell(val_tb3, 1).SetContent(table3_Subj_Name);


                            table3.Cell(val_tb3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            string[] splitb3_staff_inc = table3_staff_inc.Split(new Char[] { '\n' });
                            table3_Staff_Inc = splitb3_staff_inc[i];
                            table3.Cell(val_tb3, 2).SetContent(table3_Staff_Inc);

                            table3.Cell(val_tb3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            string[] splitb3_pass = table3_Pass.Split(new Char[] { '\n' });
                            table3_Pass_stud = splitb3_pass[i];
                            table3.Cell(val_tb3, 3).SetContent(table3_Pass_stud);

                            table3.Cell(val_tb3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            string[] splitb3_avg = table4_Avg.Split(new Char[] { '\n' });
                            table3_Avg = splitb3_avg[i];
                            table3.Cell(val_tb3, 4).SetContent(table3_Avg);
                        }

                        Gios.Pdf.PdfTablePage newpdftabpagetb3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 50, 750, 700, 1000));
                        mypdfpage.Add(newpdftabpagetb3);
                    }




                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        lblnorec.Visible = false;
                        lblnorec.Text = "";
                        //Sankar on 20May2013============================
                        string szPath = appPath + "/Report/";
                        string szFile = "Format1.pdf";
                        mypdfpage.SaveToDocument();
                        //mypdfpage1.SaveToDocument();
                        mydocument.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                        //=============================================

                    }
                }
            }
        }
        catch
        {

        }
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


    protected void Button2_Click(object sender, EventArgs e)
    {

    }
    protected void btnPrintMaster_Click(object sender, EventArgs e)
    {
        if (ddlSec.Enabled == true)
        {
            Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + ddlSemYr.SelectedIndex + "," + ddlcollege.SelectedIndex.ToString() + "," + ddlTest.SelectedIndex + "," + ddlSec.SelectedIndex;
            string clmnheadrname = "";
            int total_clmn_count = FpSpread1.Sheets[0].ColumnCount;
            Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "overall.aspx" + ":" + ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + ddlSemYr.SelectedItem.ToString() + "-" + ddlSec.SelectedItem.ToString() + ":" + "OVERALL BEST PERFORMANCE");
        }
        else
        {
            Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + ddlSemYr.SelectedIndex + "," + ddlcollege.SelectedIndex.ToString() + "," + ddlTest.SelectedIndex;
            string clmnheadrname = "";
            int total_clmn_count = FpSpread1.Sheets[0].ColumnCount;
            Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "overall.aspx" + ":" + ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + ddlSemYr.SelectedItem.ToString() + ":" + "OVERALL BEST PERFORMANCE");
        }


    }
    public void func_header()
    {


        //'----------for header
        hat.Clear();
        hat.Add("college_code", Session["InternalCollegeCode"].ToString());
        hat.Add("form_name", "overall.aspx");
        dsprint = daccess2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {

            if (dsprint.Tables[0].Rows[0]["college_name"].ToString() != string.Empty)
            {
                collnamenew1 = dsprint.Tables[0].Rows[0]["college_name"].ToString();
            }
            if (dsprint.Tables[0].Rows[0]["address1"].ToString() != "")
            {
                address1 = dsprint.Tables[0].Rows[0]["address1"].ToString();
                address = address1;
            }
            if (dsprint.Tables[0].Rows[0]["address2"].ToString() != "")
            {
                address2 = dsprint.Tables[0].Rows[0]["address2"].ToString();
                address = address1 + "-" + address2;

            }
            if (dsprint.Tables[0].Rows[0]["address3"].ToString() != "")
            {
                district = dsprint.Tables[0].Rows[0]["address3"].ToString();
                address = address1 + "-" + address2 + "-" + district;
            }

            if (dsprint.Tables[0].Rows[0]["phoneno"].ToString() != "")
            {
                Phoneno = dsprint.Tables[0].Rows[0]["phoneno"].ToString();
                phnfax = "Phone :" + " " + Phoneno;
            }
            if (dsprint.Tables[0].Rows[0]["faxno"].ToString() != "")
            {
                Faxno = dsprint.Tables[0].Rows[0]["faxno"].ToString();
                phnfax = phnfax + "Fax  :" + " " + Faxno;
            }

            if ((dsprint.Tables[0].Rows[0]["email"].ToString() != ""))
            {
                email = "E-Mail:" + dsprint.Tables[0].Rows[0]["email"].ToString();
            }
            if (dsprint.Tables[0].Rows[0]["website"].ToString() != "")
            {
                email = email + " " + "Web Site:" + dsprint.Tables[0].Rows[0]["website"].ToString();
            }
            if (dsprint.Tables[0].Rows[0]["form_heading_name"].ToString() != "")
            {
                form_heading_name = dsprint.Tables[0].Rows[0]["form_heading_name"].ToString();
            }
            if (dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString() != "")
            {
                batch_degree_branch = dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString();
            }
            //--------------for footer name--------------------------------------
            if (dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
            {
                int index = 0;
                footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
                //FpSpread1.Sheets[0].RowCount += 2;
                footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
                string[] footer_text_split = footer_text.Split(',');

                if (footer_text_split.GetUpperBound(0) > 0)
                {
                    int get_span_value = (FpSpread1.Sheets[0].ColumnCount / (footer_text_split.GetUpperBound(0) + 1));
                    for (int concod_footer = 0; concod_footer < footer_text_split.GetUpperBound(0) + 1; concod_footer++)
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, concod_footer].Text = footer_text_split[concod_footer].ToString();
                    }

                }
                else
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 1].Text = footer_text;
                }

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 5, 0].Border.BorderColorBottom = Color.White;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, 0].Border.BorderColorBottom = Color.White;

                ////if (footer_count > FpSpread1.Sheets[0].ColumnCount)
                ////{
                ////    footer_text = "";
                ////    for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
                ////    {
                ////        if (footer_text == "")
                ////        {
                ////            footer_text = footer_text_split[concod_footer].ToString();
                ////        }
                ////        else
                ////        {
                ////            footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
                ////        }
                ////    }


                ////    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), 0, 1, FpSpread1.Sheets[0].ColumnCount);
                ////    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 2), 0, 1, FpSpread1.Sheets[0].ColumnCount);
                ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = footer_text;

                ////}
                ////else if (footer_count < FpSpread1.Sheets[0].ColumnCount)
                ////{

                ////    final_print_col_cnt = FpSpread1.Sheets[0].ColumnCount / footer_count;
                ////    for (int col_foot = 0; col_foot < FpEntry.Sheets[0].ColumnCount; col_foot += final_print_col_cnt)
                ////    {
                ////        if (index < footer_text_split.GetUpperBound(0) + 1)//check the condn for index value
                ////        {
                ////            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col_foot].Text = footer_text_split[index];
                ////            FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), 0, 1, FpSpread1.Sheets[0].ColumnCount);
                ////            FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 2), col_foot, 1, col_foot + final_print_col_cnt);
                ////            index++;
                ////        }
                ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col_foot].Border.BorderColorBottom = Color.White;
                ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col_foot].Border.BorderColorLeft = Color.White;
                ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, col_foot].Border.BorderColorRight = Color.White;
                ////    }
                ////}


                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorTop = Color.White;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColor = Color.White;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorRight = Color.White;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = Color.White;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.White;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;



            }
            //----end footer
            //start new header name
            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            {
                string[] header_text = Convert.ToString(dsprint.Tables[0].Rows[0]["new_header_name"]).Split(',');
                if (header_text.GetUpperBound(0) == 0)
                {
                    FpSpread1.Sheets[0].Cells[6, 0].Text = header_text[0].ToString();
                    FpSpread1.Sheets[0].Cells[6, 0].Border.BorderColorBottom = Color.Black;

                    FpSpread1.Sheets[0].Rows[6].Visible = true;
                    FpSpread1.Sheets[0].Rows[7].Visible = false;
                    FpSpread1.Sheets[0].Rows[8].Visible = false;
                }
                else if (header_text.GetUpperBound(0) == 1)
                {
                    FpSpread1.Sheets[0].Cells[6, 0].Text = header_text[0].ToString();
                    FpSpread1.Sheets[0].Cells[7, 0].Text = header_text[1].ToString();
                    FpSpread1.Sheets[0].Cells[6, 0].Border.BorderColorBottom = Color.Black;
                    FpSpread1.Sheets[0].Cells[7, 0].Border.BorderColorBottom = Color.Black;


                    FpSpread1.Sheets[0].Rows[6].Visible = true;
                    FpSpread1.Sheets[0].Rows[7].Visible = true;
                    FpSpread1.Sheets[0].Rows[8].Visible = false;
                }
                else if (header_text.GetUpperBound(0) == 2)
                {
                    FpSpread1.Sheets[0].Cells[6, 0].Text = header_text[0].ToString();
                    FpSpread1.Sheets[0].Cells[7, 0].Text = header_text[1].ToString();
                    FpSpread1.Sheets[0].Cells[8, 0].Text = header_text[2].ToString();

                    FpSpread1.Sheets[0].Cells[6, 0].Border.BorderColorBottom = Color.Black;
                    FpSpread1.Sheets[0].Cells[7, 0].Border.BorderColorBottom = Color.Black;
                    FpSpread1.Sheets[0].Cells[8, 0].Border.BorderColorBottom = Color.Black;


                    FpSpread1.Sheets[0].Rows[6].Visible = true;
                    FpSpread1.Sheets[0].Rows[7].Visible = true;
                    FpSpread1.Sheets[0].Rows[8].Visible = true;

                }
            }


        }
        else
        {
            if (Session["InternalCollegeCode"].ToString() != null && Session["InternalCollegeCode"].ToString() != "")
            {
                string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["InternalCollegeCode"] + "";
                SqlCommand collegecmd = new SqlCommand(college, con);
                SqlDataReader collegename;
                con.Close();
                con.Open();
                collegename = collegecmd.ExecuteReader();
                if (collegename.HasRows)
                {

                    while (collegename.Read())
                    {
                        collnamenew1 = collegename["collname"].ToString();
                        address1 = collegename["address1"].ToString();
                        address2 = collegename["address2"].ToString();
                        district = collegename["district"].ToString();
                        address = address1 + "-" + address2 + "-" + district;
                        Phoneno = collegename["phoneno"].ToString();
                        Faxno = collegename["faxno"].ToString();
                        phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno + ".";
                        email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
                    }
                }
                con.Close();
            }
        }
        FpSpread1.Sheets[0].Cells[0, 1].Text = collnamenew1;
        FpSpread1.Sheets[0].Cells[1, 1].Text = address;
        FpSpread1.Sheets[0].Cells[2, 1].Text = phnfax;
        FpSpread1.Sheets[0].Cells[3, 1].Text = email;
        FpSpread1.Sheets[0].Cells[4, 0].Text = form_heading_name;
        FpSpread1.Sheets[0].Cells[5, 0].Text = batch_degree_branch;
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
    }
    public void Pageload(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        FpSpread1.Visible = false;
        btnExcel.Visible = false;
        BtnPrint.Visible = false;
        //Added By Srinath 28/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        FpEntry.Visible = false;
        FpEntry.Sheets[0].PageSize = 10;
        //FpEntry.ActiveSheetView.AutoPostBack = true;
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 12;
        style.Font.Bold = true;
        style.Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpEntry.Sheets[0].AllowTableCorner = true;
        FarPoint.Web.Spread.SheetView svsort = new FarPoint.Web.Spread.SheetView();
        svsort = FpEntry.ActiveSheetView;
        svsort.AllowSort = true;
        FpEntry.CommandBar.Visible = false;

        FpSpread1.Sheets[0].SheetName = " ";
        FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
        FpEntry.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
        FpEntry.Sheets[0].Columns[1].Width = 180;
        FpSpread3.Sheets[0].AutoPostBack = true;
        FpEntry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        FpEntry.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
        FpEntry.Pager.Align = HorizontalAlign.Right;
        FpEntry.Pager.Font.Bold = true;
        FpEntry.Pager.Font.Name = "Book Antiqua";
        FpEntry.Pager.ForeColor = Color.DarkGreen;
        FpEntry.Pager.BackColor = Color.Beige;
        FpEntry.Pager.BackColor = Color.AliceBlue;
        //FpEntry.Sheets[0].AutoPostBack = false;
        FpEntry.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].DefaultStyle.Font.Bold = false;
        FpEntry.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].FrozenColumnCount = 4;
        //FpEntry.Sheets[0].DefaultStyle.Font.Bold = false;
        FpEntry.Sheets[0].Columns[0].Width = 70;
        FpEntry.Sheets[0].Columns[1].Width = 70;
        FpEntry.Sheets[0].Columns[2].Width = 200;
        // FpEntry.Sheets[0].PageSize = 10;
        FpEntry.Pager.PageCount = 5;
        FpEntry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;

        //Added by Srinath 21/3/2015
        string grouporusercode = "";

        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        Master = "select * from Master_Settings where " + grouporusercode + "";
        setcon.Close();
        setcon.Open();
        SqlDataReader mtrdr;

        SqlCommand mtcmd = new SqlCommand(Master, setcon);
        mtrdr = mtcmd.ExecuteReader();

        Session["strvar"] = "";
        Session["Rollflag"] = "0";
        Session["Regflag"] = "0";
        Session["Studflag"] = "0";
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
                if (mtrdr["settings"].ToString() == "Student_Type" && mtrdr["value"].ToString() == "1")
                {
                    Session["Studflag"] = "1";
                }
                if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                {
                    strdayflag = " and (registration.Stud_Type='Day Scholar'";
                }
                if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                {
                    if (strdayflag != "" && strdayflag != "\0")
                    {
                        strdayflag = strdayflag + " or registration.Stud_Type='Hostler'";
                    }
                    else
                    {
                        strdayflag = " and (registration.Stud_Type='Hostler'";
                    }
                }
                if (mtrdr["settings"].ToString() == "Regular")
                {
                    regularflag = "and ((registration.mode=1)";

                    // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                }
                if (mtrdr["settings"].ToString() == "Lateral")
                {
                    if (regularflag != "")
                    {
                        regularflag = regularflag + " or (registration.mode=3)";
                    }
                    else
                    {
                        regularflag = regularflag + " and ((registration.mode=3)";
                    }
                    //Session["strvar"] = Session["strvar"] + " and (mode=3)";
                }
                if (mtrdr["settings"].ToString() == "Transfer")
                {
                    if (regularflag != "")
                    {
                        regularflag = regularflag + " or (registration.mode=2)";
                    }
                    else
                    {
                        regularflag = regularflag + " and ((registration.mode=2)";
                    }
                    //Session["strvar"] = Session["strvar"] + " and (mode=2)";
                }

                if (mtrdr["settings"].ToString() == "Male" && mtrdr["value"].ToString() == "1")
                {
                    genderflag = " and (sex='0'";
                }
                if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                {
                    if (genderflag != "" && genderflag != "\0")
                    {
                        genderflag = genderflag + " or sex='1'";
                    }
                    else
                    {
                        genderflag = " and (sex='1'";
                    }

                }

                if (mtrdr["settings"].ToString() == "print_master_setting" && mtrdr["value"].ToString() == "1")
                {
                    btnPrintMaster.Visible = false;// true;
                }
                else
                {
                    btnPrintMaster.Visible = false;
                }
            }
        }
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

                ddlcollege.SelectedIndex = Convert.ToInt16(spl_pageload_val[4].ToString());
                Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();

                bindbatch();
                ddlBatch.SelectedIndex = Convert.ToInt32(spl_pageload_val[0].ToString());
                binddegree();
                ddlDegree.SelectedIndex = Convert.ToInt32(spl_pageload_val[1].ToString());
                if (ddlDegree.Text != "")
                {
                    bindbranch();
                    ddlBranch.SelectedIndex = Convert.ToInt32(spl_pageload_val[2].ToString());
                    bindsem();
                    ddlSemYr.SelectedIndex = Convert.ToInt32(spl_pageload_val[3].ToString());
                    bindsec();
                    if (ddlSec.Enabled == true)
                    {
                        ddlSec.SelectedIndex = Convert.ToInt32(spl_pageload_val[6].ToString());
                    }

                    GetTest();
                    ddlTest.SelectedIndex = Convert.ToInt32(spl_pageload_val[5].ToString());
                    lblnorec.Visible = false;

                    btnGo_Click(sender, e);
                    //func_header();

                }
                else
                {
                    lblnorec.Text = "Give degree rights to the staff";
                    lblnorec.Visible = true;
                }
            }
        }
        else
        {
            bindbatch();
            binddegree();
            if (ddlDegree.Text != "")
            {
                bindbranch();
                bindsem();
                bindsec();
                GetTest();
            }
            else
            {
                lblnorec.Text = "Give degree rights to the staff";
                lblnorec.Visible = true;
            }
        }
    }
}

