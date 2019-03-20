using System;//======modified on 15/5/12 by PRABHA(text cell type) , modified on 06/06/12 by mythili(rollno)removed
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BalAccess;
using DalConnection;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

public partial class addressslip : System.Web.UI.Page
{
    SqlCommand cmd;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    DAccess2 d2 = new DAccess2();
    string Master1 = "";
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";

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
            if (!Page.IsPostBack)
            {
                setLabelText();
                btnprintmaster.Visible = false;
                cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''order by batch_year", con);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                ddlbatch.DataSource = ds1;
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
                //ddlBatch.Items.Insert(0, new ListItem("--Select--", "-1"));
                int batch = 0;
                string batchcount = ddlbatch.Items.Count.ToString();
                if (int.TryParse(batchcount, out batch))
                    batch = batch - 1;
                ddlbatch.SelectedIndex = batch;
                con.Close();
                con.Open();
                string collegecode = Session["collegecode"].ToString();
                string usercode = Session["usercode"].ToString();
                //con.Open();
                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                {
                    //cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + Session["collegecode"] + " order by course.course_name ", con);
                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //DataSet ds = new DataSet();
                    //da.Fill(ds);
                    DataSet ds = Bind_Degree(collegecode.ToString(), usercode);
                    ddlDegree.DataSource = ds;
                    ddlDegree.DataValueField = "course_id";
                    ddlDegree.DataTextField = "course_name";
                    ddlDegree.DataBind();
                    //ddlDegree.Items.Insert(0, new ListItem("--Select--", "-1"));
                }
                //bind BRANCH on loaD...
                con.Close();
                con.Open();
                //cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddlDegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
                //SqlDataAdapter daBRANCH = new SqlDataAdapter(cmd);
                //DataSet dsbranch = new DataSet();
                //daBRANCH.Fill(dsbranch);
                //string collegecode = Session["collegecode"].ToString();
                //string usercode = Session["usercode"].ToString();
                string course_id = ddlDegree.SelectedValue.ToString();
                DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode);
                ddlBranch.DataSource = dsbranch;
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataBind();
                //bind semester
                bindsem();
                //bind section
                BindSectionDetail();
                //FpSpread1.CommandBar.Visible = false;
                loadHeader();
                contactRadio.Checked = true;
                //settings
                Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                setcon.Close();
                setcon.Open();
                SqlDataReader mtrdr;
                SqlCommand mtcmd = new SqlCommand(Master1, setcon);
                mtrdr = mtcmd.ExecuteReader();
                DateTime currentdate = System.DateTime.Now;
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
                            //if (genderflag != "" && genderflag != "\0")
                            //{
                            //    genderflag = genderflag + " or sex='1'";
                            //}
                            //else
                            //{
                            //genderflag = " and (sex='1'";
                            ////  }
                            if (genderflag != "" && genderflag != "\0")//rajasekar 24/07/2018
                            {
                                genderflag = genderflag + " or sex='1'";
                            }
                            else
                            {
                                genderflag = " and (sex='1'";
                            }
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
            }
        }
        catch (Exception ex) { }
    }

    //added by abarna for schoolsetting and collegesetting based label displayed on that screen
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




        lbl.Add(lblDegree);
        lbl.Add(lblBranch);
        lbl.Add(lblDuration);


        fields.Add(2);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = FpSpread2.FindControl("Update");
        Control cntCancelBtn = FpSpread2.FindControl("Cancel");
        Control cntCopyBtn = FpSpread2.FindControl("Copy");
        Control cntCutBtn = FpSpread2.FindControl("Clear");
        Control cntPasteBtn = FpSpread2.FindControl("Paste");
        //Control cntPageNextBtn = FpSpread1.FindControl("Next");
        //Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
        Control cntPagePrintBtn = FpSpread2.FindControl("Print");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);


            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);

            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPagePreviousBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPagePrintBtn.Parent;
            //tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }

    public void BindSectionDetail()
    {

        string branch = ddlBranch.SelectedValue.ToString();
        string batch = ddlbatch.SelectedValue.ToString();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        //ddlSec.Items.Insert(0, new ListItem("", "-1"));
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
            {
                ddlSec.Enabled = false;
                //Label8.Visible = false;
                //GetTest();
            }
            else
            {
                ddlSec.Enabled = true;

            }
        }
        else
        {
            ddlSec.Enabled = false;
            //Label8.Visible = false;
            //GetTest();
        }
    }

    public void bindsem()
    {

        //--------------------semester load
        ddlSem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
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
                    ddlSem.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSem.Items.Add(i.ToString());
                }

            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlSem.Items.Clear();
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
                        ddlSem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                }
            }

            dr1.Close();
        }
        //FpMarkEntry.Visible = false;
        con.Close();
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        con.Close();
        con.Open();
        //con.Open();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
        //if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        //{

        //    DataSet ds = Bind_Degree(collegecode.ToString(), usercode);
        //    ddlDegree.DataSource = ds;
        //    ddlDegree.DataValueField = "course_id";
        //    ddlDegree.DataTextField = "course_name";
        //    ddlDegree.DataBind();
        //    //ddlDegree.Items.Insert(0, new ListItem("--Select--", "-1"));
        //}
        //con.Close();
        //con.Open();

        //string course_id = ddlDegree.SelectedValue.ToString();
        //DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode);
        //ddlBranch.DataSource = dsbranch;
        //ddlBranch.DataValueField = "degree_code";
        //ddlBranch.DataTextField = "dept_name";
        //ddlBranch.DataBind();

        //bindsem();


        //BindSectionDetail();


        BtnSlip.Visible = false;
        lblnorec.Visible = false;
        FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;


    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        con.Close();
        con.Open();
        //cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddlDegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
        //SqlDataAdapter da = new SqlDataAdapter(cmd);
        //DataSet ds = new DataSet();
        //da.Fill(ds);
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
        string course_id = ddlDegree.SelectedValue.ToString();
        DataSet ds = Bind_Dept(course_id, collegecode, usercode);
        ddlBranch.DataSource = ds;
        ddlBranch.DataValueField = "degree_code";
        ddlBranch.DataTextField = "dept_name";
        ddlBranch.DataBind();
        //ddlBranch.Items.Insert(0, new ListItem("", "-1"));
        con.Close();

        //bind semester
        bindsem();
        //bind section
        BindSectionDetail();
        BtnSlip.Visible = false;
        lblnorec.Visible = false;
        FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        //bind section
        BindSectionDetail();
        BtnSlip.Visible = false;
        lblnorec.Visible = false;
        FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;

    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSectionDetail();
        BtnSlip.Visible = false;
        lblnorec.Visible = false;
        FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        BtnSlip.Visible = false;
        lblnorec.Visible = false;
        FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
    }

    public void slip()
    {
        try
        {
            Label2.Visible = true;
            FpSpread2.Sheets[0].RowCount = 0;
            BtnSlip.Visible = false;
            FpSpread1.SaveChanges();
            FpSpread2.SaveChanges();
            FpSpread2.Sheets[0].ColumnCount = 2;
            FpSpread2.Sheets[0].Columns[0].Width = 350;
            FpSpread2.Sheets[0].Columns[1].Width = 350;
            FpSpread2.Sheets[0].PageSize = 30;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].DefaultStyle.Font.Name = "Book Antique";
            FpSpread2.Sheets[0].DefaultStyle.Font.Bold = true;
            FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Visible = false;
            FpSpread2.CommandBar.Visible = true;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = false;
            int i = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
            {
                FpSpread1.SaveChanges();
                int isval = 0;
                string s = FpSpread1.Sheets[0].Cells[res, 0].Text;

                isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 0].Value);
                if (isval == 1)
                {
                    Label2.Visible = false;
                    if (PermanantRadio.Checked == true)
                    {
                        string rol_no = string.Empty;
                        if (ddlreportTye.SelectedItem.Value == "3")
                        {
                            rol_no = FpSpread1.Sheets[0].Cells[res, 1].Text;
                        }

                        if (ddlreportTye.SelectedItem.Value == "0" || ddlreportTye.SelectedItem.Value == "1" || ddlreportTye.SelectedItem.Value == "2")
                        {
                            rol_no = Convert.ToString(FpSpread1.Sheets[0].Cells[res, 1].Tag);

                        }
                        string Toaddress = string.Empty;
                        if (ddlreportTye.SelectedItem.Value == "3")
                        {

                            Toaddress = "select parent_name as pname,isnull(Student_Mobile,'') as Student_Mobile,parent_addressP as padd1,streetP as padd2,parent_pincodeP as ppin,cityp as pdistrict,parent_statep as pstate,sex as sex from applyn where app_no in (select app_no from registration where roll_no='" + rol_no + "')";

                        }
                        if (ddlreportTye.SelectedItem.Value == "0" || ddlreportTye.SelectedItem.Value == "1" || ddlreportTye.SelectedItem.Value == "2")
                        {

                            Toaddress = "select parent_name as pname,isnull(Student_Mobile,'') as Student_Mobile,parent_addressc as padd1,streetc as padd2,parent_pincodec as ppin,cityc as pdistrict,parent_statep as pstate,sex as sex from applyn where app_no in('" + rol_no + "')";
                        }
                        SqlCommand Toaddcmd = new SqlCommand(Toaddress, con);
                        SqlDataReader Toaddreader;
                        con.Close();
                        con.Open();
                        string toaddname = "";
                        string toadd1 = "";
                        string toadd2 = "";
                        string topin = "";
                        string todistrict = "";
                        string tostate = "";
                        string stustate = "";
                        string gender = "";
                        string stugender = "";
                        string studentname = string.Empty;
                        string mob_num = "";

                        if (ddlreportTye.SelectedItem.Value == "3")
                        {
                            studentname = FpSpread1.Sheets[0].Cells[res, 3].Text;
                        }
                        if (ddlreportTye.SelectedItem.Value == "0" || ddlreportTye.SelectedItem.Value == "1" || ddlreportTye.SelectedItem.Value == "2")
                        {
                            studentname = FpSpread1.Sheets[0].Cells[res, 2].Text;

                        }
                        Toaddreader = Toaddcmd.ExecuteReader();
                        if (Toaddreader.HasRows)
                        {
                            BtnSlip.Visible = true;

                            while (Toaddreader.Read())
                            {
                                toaddname = Toaddreader["pname"].ToString();
                                toadd1 = Toaddreader["padd1"].ToString();
                                toadd2 = Toaddreader["padd2"].ToString();
                                topin = Toaddreader["ppin"].ToString();
                                todistrict = Toaddreader["pdistrict"].ToString();
                                mob_num = Toaddreader["Student_Mobile"].ToString();
                                int num = 0;
                                if (int.TryParse(todistrict, out num))
                                {
                                    // todistrict = d2.GetFunction("select textval from textvaltable where TextCriteria='dis' and TextCode='" + todistrict + "'");
                                    todistrict = d2.GetFunction("select textval from textvaltable where TextCriteria='city' and TextCode='" + todistrict + "'");//20.06.2018
                                    if (todistrict.Trim() == "0" || todistrict.Trim() == "" || todistrict == null)
                                    {
                                        todistrict = "";
                                    }
                                }

                                tostate = Toaddreader["pstate"].ToString();
                                gender = Toaddreader["sex"].ToString();
                            }
                        }
                        if (tostate != "")
                        {
                            string state = "select textval from textvaltable where textcriteria='state' and textcode=" + tostate + "";
                            SqlCommand tostatecom = new SqlCommand(state, con);
                            SqlDataReader statereader;

                            con.Close();
                            con.Open();
                            statereader = tostatecom.ExecuteReader();
                            if (statereader.HasRows)
                            {
                                while (statereader.Read())
                                {
                                    stustate = statereader["textval"].ToString();
                                }
                            }
                        }
                        if (gender != "")
                        {
                            if (Convert.ToInt32(gender) == 0)
                            {
                                stugender = "S/O";
                            }
                            else if (Convert.ToInt32(gender) == 1)
                            {
                                stugender = "D/O";
                            }
                        }
                        if (i == 0)
                        {
                            FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 8, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Margin.Left = 10;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 8, 0].Text = rol_no; // changed by mythili on 060612
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 0].Text = studentname;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 0].Text = stugender + " " + toaddname;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 0].Text = toadd1;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 0].Text = toadd2;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 0].Text = todistrict;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = stustate + "-" + topin;

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 0].Text = studentname;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 0].Text = stugender + " " + toaddname;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 0].Text = toadd1;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 0].Text = toadd2 + " " + todistrict;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 0].Text = todistrict;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 0].Text = stustate + "-" + topin;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = Convert.ToString(mob_num);

                            i = 1;
                        }
                        else
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 8, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].Margin.Left = 10;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 8, 1].Text = rol_no; // changed by mythili on 060612
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 1].Text = studentname;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 1].Text = stugender + " " + toaddname;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 1].Text = toadd1;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 1].Text = toadd2;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 1].Text = todistrict;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = stustate + "-" + topin;

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 1].Text = studentname;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 1].Text = stugender + " " + toaddname;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 1].Text = toadd1;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 1].Text = toadd2 + " " + todistrict;
                            // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 1].Text = todistrict;//rajasekar 23/07/2018
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 1].Text = stustate + "-" + topin;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].Text = Convert.ToString(mob_num);

                            i = 0;
                        }
                    }
                    else
                    {
                        string rol_no = string.Empty;
                        if (ddlreportTye.SelectedItem.Value == "3")
                        {
                            rol_no = FpSpread1.Sheets[0].Cells[res, 1].Text;
                        }

                        if (ddlreportTye.SelectedItem.Value == "0" || ddlreportTye.SelectedItem.Value == "1" || ddlreportTye.SelectedItem.Value == "2")
                        {
                            rol_no = Convert.ToString(FpSpread1.Sheets[0].Cells[res, 1].Tag);

                        }
                        string Toaddress = string.Empty;
                        if (ddlreportTye.SelectedItem.Value == "3")
                        {

                            Toaddress = "select parent_name as pname,isnull(Student_Mobile,'') as Student_Mobile,parent_addressc as padd1,streetc as padd2,parent_pincodec as ppin,cityc as pdistrict,parent_statep as pstate,sex as sex from applyn where app_no in (select app_no from registration where roll_no='" + rol_no + "')";
                        }
                        if (ddlreportTye.SelectedItem.Value == "0" || ddlreportTye.SelectedItem.Value == "1" || ddlreportTye.SelectedItem.Value == "2")
                        {
                            Toaddress = "select parent_name as pname,isnull(Student_Mobile,'') as Student_Mobile,parent_addressc as padd1,streetc as padd2,parent_pincodec as ppin,cityc as pdistrict,parent_statep as pstate,sex as sex from applyn where app_no in('" + rol_no + "')";
                        }
                        SqlCommand Toaddcmd = new SqlCommand(Toaddress, con);
                        SqlDataReader Toaddreader;
                        con.Close();
                        con.Open();
                        string toaddname = "";
                        string toadd1 = "";
                        string toadd2 = "";
                        string topin = "";
                        string todistrict = "";
                        string tostate = "";
                        string stustate = "";
                        string gender = "";
                        string stugender = "";
                        string mob_num = "";

                        string studentname = string.Empty;
                        if (ddlreportTye.SelectedItem.Value == "3")
                        {
                            studentname = FpSpread1.Sheets[0].Cells[res, 3].Text;
                        }
                        if (ddlreportTye.SelectedItem.Value == "0" || ddlreportTye.SelectedItem.Value == "1" || ddlreportTye.SelectedItem.Value == "2")
                        {
                            studentname = FpSpread1.Sheets[0].Cells[res, 2].Text;
                        }
                        Toaddreader = Toaddcmd.ExecuteReader();
                        if (Toaddreader.HasRows)
                        {
                            BtnSlip.Visible = true;

                            while (Toaddreader.Read())
                            {
                                toaddname = Toaddreader["pname"].ToString();
                                toadd1 = Toaddreader["padd1"].ToString();
                                toadd2 = Toaddreader["padd2"].ToString();
                                topin = Toaddreader["ppin"].ToString();
                                todistrict = Toaddreader["pdistrict"].ToString();
                                mob_num = Toaddreader["Student_Mobile"].ToString();
                                int num = 0;
                                if (int.TryParse(todistrict, out num))
                                {
                                    todistrict = d2.GetFunction("select textval from textvaltable where TextCriteria='city' and TextCode='" + todistrict + "'");
                                    if (todistrict.Trim() == "0" || todistrict.Trim() == "" || todistrict == null)
                                    {
                                        todistrict = "";
                                    }
                                }
                                tostate = Toaddreader["pstate"].ToString();
                                gender = Toaddreader["sex"].ToString();
                            }
                        }
                        if (tostate != "")
                        {
                            string state = "select textval from textvaltable where textcriteria='state' and textcode=" + tostate + "";
                            SqlCommand tostatecom = new SqlCommand(state, con);
                            SqlDataReader statereader;

                            con.Close();
                            con.Open();
                            statereader = tostatecom.ExecuteReader();
                            if (statereader.HasRows)
                            {
                                while (statereader.Read())
                                {
                                    stustate = statereader["textval"].ToString();
                                }
                            }
                        }
                        if (gender != "")
                        {
                            if (Convert.ToInt32(gender) == 0)
                            {
                                stugender = "S/O";
                            }
                            else if (Convert.ToInt32(gender) == 1)
                            {
                                stugender = "D/O";
                            }
                        }
                        if (i == 0)
                        {
                            FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 8, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 0].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Margin.Left = 10;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 8, 0].Text = studentname;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 0].Text = stugender + " " + toaddname;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 0].Text = toadd1;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 0].Text = toadd2;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 0].Text = todistrict;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 0].Text = stustate + "-" + topin;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = Convert.ToString(mob_num);


                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 0].Text = studentname;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 0].Text = stugender + " " + toaddname;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 0].Text = toadd1;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 0].Text = toadd2 + " " + todistrict;
                            // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 0].Text = todistrict;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 0].Text = stustate + "-" + topin;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = Convert.ToString(mob_num);
                            i = 1;
                        }
                        else
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 8, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 1].Margin.Left = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].Margin.Left = 10;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 8, 1].Text = studentname;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 1].Text = stugender + " " + toaddname;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 1].Text = toadd1;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 1].Text = toadd2;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 1].Text = todistrict;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 1].Text = stustate + "-" + topin;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].Text = Convert.ToString(mob_num);

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 7, 1].Text = studentname;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 6, 1].Text = stugender + " " + toaddname;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 5, 1].Text = toadd1;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 1].Text = toadd2 + " " + todistrict;
                            //   FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 4, 1].Text = todistrict;//rajasekar 23/07/2018
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 3, 1].Text = stustate + "-" + topin;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 2, 1].Text = Convert.ToString(mob_num);

                            i = 0;
                        }
                    }




                }
            }
        }
        catch
        {
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {

            SqlDataReader reader;
            string sqlstr = "";
            string strsec = "";
            string sections = "";
            string rollno = "";
            string regno = "";
            string studentname = "";
            string studenttype = "";
            sections = ddlSec.SelectedValue.ToString();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Visible = true;
            Label2.Visible = false;
            if (!Page.IsPostBack == false)
            {
                if (sections.ToString() == "All" || sections.ToString() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = "and sections='" + sections.ToString() + "'";
                }
            }
            if (ddlreportTye.SelectedItem.Value == "3")
            {
                sqlstr = " Select distinct  registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.stud_type from registration,applyn where applyn.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + " " + strsec + "" + Session["strvar"] + "  and delflag=0 and exam_flag<>'Debar'"; //and cc=0-----modified by Mullai
            }
            if (ddlreportTye.SelectedItem.Value == "0" || ddlreportTye.SelectedItem.Value == "1" || ddlreportTye.SelectedItem.Value == "2")
            {

                sqlstr = "select app_no,stud_name,app_formno from applyn where degree_code='" + ddlBranch.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' ";
            }


            if (ddlreportTye.SelectedItem.Value == "0")
            {
                sqlstr = sqlstr + " and ISNULL( Admission_Status,'0')='0' and ISNULL(selection_status,'0')='0' and isconfirm='1'";

            }
            else if (ddlreportTye.SelectedItem.Value == "1")
            {
                sqlstr = sqlstr + " and ISNULL( Admission_Status,'0')='0' and ISNULL(selection_status,'0')='1'";

            }
            else if (ddlreportTye.SelectedItem.Value == "2")
            {
                sqlstr = sqlstr + " and ISNULL( Admission_Status,'0')='1' and ISNULL(selection_status,'0')='1' and app_no not in( select app_no from Registration  where  degree_code in('" + ddlBranch.SelectedValue.ToString() + "')and Batch_Year in('" + ddlbatch.SelectedValue.ToString() + "'))";

            }
            else if (ddlreportTye.SelectedItem.Value == "3")
            {
                sqlstr = sqlstr + " and ISNULL( Admission_Status,'0')='1' and ISNULL(selection_status,'0')='1' ";

            }

            SqlCommand cmd1 = new SqlCommand(sqlstr, con1);
            con1.Open();
            reader = cmd1.ExecuteReader();
            if (reader.HasRows)
            {
                if (ddlreportTye.SelectedItem.Value == "3")
                {
                    loadHeader();

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;

                    FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = chkcell1;
                    chkcell1.AutoPostBack = true;

                    while (reader.Read())
                    {

                        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                        FpSpread1.Sheets[0].Columns[1].CellType = textcel_type;
                        FpSpread1.Sheets[0].Columns[2].CellType = textcel_type;

                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                        rollno = reader["roll_no"].ToString();
                        regno = reader["reg_no"].ToString();
                        studentname = reader["stud_name"].ToString();
                        studenttype = reader["stud_type"].ToString();
                        //FpEntry.Sheets[0].ColumnCount = FpEntry.Sheets[0].ColumnCount + 1;
                        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpEntry.Sheets[0].ColumnCount - 1].Text = "Select";
                        //FpSpread1.Sheets[0].Columns[0, FpEntry.Sheets[0].ColumnCount - 1].Width = 50;
                        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                        // FpSpread1.Sheets[0].Columns[0].CellType = chkcell;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = chkcell;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = rollno;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = regno;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = studentname;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = studenttype;

                    }
                }
                if (ddlreportTye.SelectedItem.Value == "0" || ddlreportTye.SelectedItem.Value == "1" || ddlreportTye.SelectedItem.Value == "2")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Application No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
                    //  FpSpread1.Sheets[0].Columns[0].CellType = chkcell;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = chkcell1;
                    chkcell1.AutoPostBack = true;

                    while (reader.Read())
                    {

                        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                        FpSpread1.Sheets[0].Columns[1].CellType = textcel_type;
                        FpSpread1.Sheets[0].Columns[2].CellType = textcel_type;
                        FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;

                        rollno = reader["app_no"].ToString();
                        regno = reader["app_formno"].ToString();

                        studentname = reader["stud_name"].ToString();
                        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();


                        //  FpSpread1.Sheets[0].Columns[0].CellType = chkcell;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = chkcell;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = regno;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = rollno;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = studentname;

                    }

                }
            }
            if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) == 0)
            {
                BtnSlip.Visible = false;
                lblnorec.Visible = true;
                FpSpread1.Visible = false;
                btnprintmaster.Visible = false;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;

            }
            else
            {
                lblnorec.Visible = false;
                Buttontotal.Visible = true;
                lblrecord.Visible = true;
                DropDownListpage.Visible = true;
                TextBoxother.Visible = false;
                lblpage.Visible = true;
                TextBoxpage.Visible = true;
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                FpSpread1.ActiveSheetView.AutoPostBack = false;
                FpSpread1.Sheets[0].PageSize = 10;
                //FpEntry.Sheets[0].Columns[3].Width = 220;
                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Right;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Book Antiqua";
                FpSpread1.Pager.ForeColor = Color.DarkGreen;
                FpSpread1.Pager.BackColor = Color.Beige;
                FpSpread1.Pager.BackColor = Color.AliceBlue;
                //FpEntry.ActiveSheetView.SpanModel.Add((Convert.ToInt16(FpEntry.Sheets[0].RowCount) - 1), 0, 1, 2);
                //FpEntry.Sheets[0].SetText(Convert.ToInt16(FpEntry.Sheets[0].RowCount) - 1, 0, "Average");
                Double totalRows = 0;
                totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
                Buttontotal.Text = "Records : " + totalRows + " Pages : " + Session["totalPages"];
                DropDownListpage.Items.Clear();



                if (totalRows >= 10)
                {

                    FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    FpSpread1.Height = 335;

                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    FpSpread1.Height = 100;
                }
                else
                {
                    FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
                    FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
                }
            }
            //FpSpread1.Sheets[0].ColumnHeader.Cells[
        }
        catch
        {
        }
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        TextBoxother.Text = "";
        if (DropDownListpage.Text == "Others")
        {
            TextBoxpage.Text = "";
            //panels.Visible = false;
            //ddlSubject.Visible = false;
            TextBoxother.Visible = true;
            TextBoxother.Focus();

        }
        else
        {
            TextBoxother.Visible = false;
            FpSpread1.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }

    }

    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        try
        {

            if (TextBoxother.Text != "")
            {
                //panels.Visible = true;
                //ddlSubject.Visible = true;
                FpSpread1.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                CalculateTotalPages();
            }
        }
        catch
        {
            TextBoxother.Text = "";
        }
    }

    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "  Pages : " + Session["totalPages"];


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
                    FpSpread1.Visible = true;
                    btnprintmaster.Visible = true;
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
                    FpSpread1.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    FpSpread1.Visible = true;
                    btnprintmaster.Visible = true;
                }
            }
        }
        catch
        {
            TextBoxpage.Text = "";
        }
    }

    protected void contactRadio_CheckedChanged(object sender, EventArgs e)
    {
        //slip();
    }

    protected void PermanantRadio_CheckedChanged(object sender, EventArgs e)
    {
        //slip();
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        slip();
        FpSpread2.Visible = true;
    }

    protected void BtnSlip_Click(object sender, EventArgs e)
    {

    }

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = string.Empty;
        string section = string.Empty;


        if (ddlSec.Enabled != false && ddlSec.Text != "")
        {
            section = "Section:" + ddlSec.SelectedItem.ToString();
        }
        else
        {
            section = "";
        }

        Session["column_header_row_count"] = FpSpread1.Sheets[0].ColumnHeader.RowCount;

        degreedetails = "  Address Slip@Batch:" + ddlbatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "[" + ddlBranch.SelectedItem.ToString() + "] Sem" + ddlSem.SelectedItem.ToString() + section;
        string pagename = "addressslip.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void ddlreportTye_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            loadHeader();
        }
        catch
        {
        }
    }

    public void loadHeader()
    {

        if (ddlreportTye.SelectedItem.Value == "3")
        {
            FpSpread1.Sheets[0].ColumnCount = 5;
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Text = "";
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpSpread2.Sheets[0].PageSize = 30;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.Sheets[0].Columns[0].Width = 50;
            FpSpread1.Sheets[0].Columns[1].Width = 70;
            FpSpread1.Sheets[0].Columns[2].Width = 90;
            FpSpread1.Sheets[0].Columns[3].Width = 130;
            FpSpread1.Sheets[0].Columns[4].Width = 110;
            FpSpread1.CommandBar.Visible = false;
            FpSpread2.Visible = false;
            FpSpread1.SaveChanges();
        }

        if (ddlreportTye.SelectedItem.Value == "0" || ddlreportTye.SelectedItem.Value == "1")
        {
            FpSpread1.Sheets[0].ColumnCount = 3;
            FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread2.Sheets[0].PageSize = 30;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.Sheets[0].Columns[0].Width = 50;
            FpSpread1.Sheets[0].Columns[1].Width = 200;
            FpSpread1.Sheets[0].Columns[2].Width = 250;

            FpSpread1.CommandBar.Visible = false;
            FpSpread2.Visible = false;
            FpSpread1.SaveChanges();

        }

    }

    protected void fpspreadshow_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        FpSpread1.SaveChanges();
        byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[0, 0].Value);
        if (check == 1)
        {
            for (int ik = 1; ik < FpSpread1.Sheets[0].RowCount; ik++)
            {
                FpSpread1.Sheets[0].Cells[ik, 0].Value = 1;
            }
        }
        else
        {
            for (int ik = 1; ik < FpSpread1.Sheets[0].RowCount; ik++)
            {
                FpSpread1.Sheets[0].Cells[ik, 1].Value = 0;
            }
        }
    }

}