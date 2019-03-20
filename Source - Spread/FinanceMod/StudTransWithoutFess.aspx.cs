﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Configuration;

public partial class StudTransWithoutFess : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static int chosedmode = 0;
    static int personmode = 0;
    int userCode = 0;
    static byte roll = 0;
    static int admis = 0;
    static string colgcode = string.Empty;
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds1 = new DataSet();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            userCode = Convert.ToInt32(Convert.ToString(Session["usercode"]));
            // collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                    colgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                setLabelText();
                loadfromsetting();
                //to details
                bindclg();
                bindstream();
                bindBtch();
                binddeg();
                binddept();
                bindsem();
                bindSeat();
                bindsect();
                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_date.Attributes.Add("readonly", "readonly");
                rbmode_Selected(sender, e);
                RollAndRegSettings();
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                colgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }

    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                {
                    roll = 0;
                }
                else if (rollval == "1" && regval == "1" && addVal == "1")
                {
                    roll = 1;
                }
                else if (rollval == "1" && regval == "0" && addVal == "0")
                {
                    roll = 2;
                }
                else if (rollval == "0" && regval == "1" && addVal == "0")
                {
                    roll = 3;
                }
                else if (rollval == "0" && regval == "0" && addVal == "1")
                {
                    roll = 4;
                }
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex) { }
    }

    #region Entry

    public void bindcollege()
    {
        try
        {
            //ds.Clear();
            ddlcollege.Items.Clear();
            reuse.bindCollegeToDropDown(usercode, ddlcollege);
            //string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            //ds = d2.select_method_wo_parameter(selectQuery, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddl_colg.DataSource = ds;
            //    ddl_colg.DataTextField = "collname";
            //    ddl_colg.DataValueField = "college_code";
            //    ddl_colg.DataBind();
            //}            
        }
        catch (Exception ex) { }
    }

    #region roll no

    public void loadfromsetting()
    {
        try
        {
            ListItem lst1 = new ListItem("Roll No", "0");
            ListItem lst2 = new ListItem("Reg No", "1");
            ListItem lst3 = new ListItem("Admission No", "2");
            ListItem lst4 = new ListItem("App No", "3");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code ='" + collegecode + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rollno.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(lst4);
            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(lst1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    // txt_roll.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    // txt_roll.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    // txt_roll.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    // txt_roll.Text = "App No";
                    chosedmode = 3;
                    break;
            }



        }
        catch (Exception ex) { }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_roll.Text = "";
            txt_name.Text = "";
            txt_colg.Text = "";
            txt_strm.Text = "";
            txt_batch.Text = "";
            txt_degree.Text = "";
            txt_dept.Text = "";
            txt_sem.Text = "";
            txt_sec.Text = "";
            txt_seattype.Text = "";
            image2.ImageUrl = "";

            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {

                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    //  rbl_rollno.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    // rbl_rollno.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    // rbl_rollno.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    // rbl_rollno.Text = "App No";
                    chosedmode = 3;
                    break;
            }
        }
        catch (Exception ex) { }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();
            if (personmode == 0)
            {
                //student query
                //and (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0)
                if (chosedmode == 0)
                {
                    query = "select top 100 Roll_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and college_code='" + colgcode + "' and Roll_No like '" + prefixText + "%' ";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and college_code='" + colgcode + "' and Reg_No like '" + prefixText + "%' ";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and college_code='" + colgcode + "' and Roll_admit like '" + prefixText + "%' ";
                }
                else
                {
                    if (admis == 2)
                    {
                        query = "  select  top 100 app_formno from applyn a ,Registration r where a.app_no=r.App_No and admission_status =1 and selection_status=1 and isconfirm ='1' and DelFlag =0 and college_code='" + colgcode + "' and app_formno like '" + prefixText + "%' ";
                    }
                    else
                    {
                        query = "  select  top 100 app_formno from applyn where isconfirm ='1' and college_code='" + colgcode + "' and app_formno like '" + prefixText + "%' ";
                    }
                }
            }
            else if (personmode == 1)
            {
                //staff query
            }
            else if (personmode == 2)
            {
                //Vendor query
            }
            else
            {
                //Others query
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    #endregion

    public void txt_roll_TextChanged(object sender, EventArgs e)
    {
        try
        {

            string rollno = Convert.ToString(txt_roll.Text);
            string cursem = "";
            string query = "";
            if (!string.IsNullOrEmpty(rollno))
            {
                query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,seattype,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type    from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code and d.college_code='" + collegecode + "' ";
                //and r.Roll_no='" + rollno + "' and d.college_code=" + collegecode + "";

                if (rbl_rollno.Items.Count > 0)
                {
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                        query = query + "and r.Roll_no='" + rollno + "'";
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        query = query + "and r.Reg_No='" + rollno + "' ";
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        query = query + "and r.Roll_Admit='" + rollno + "'";
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                    {
                        query = "select ''parent_name,r.stud_name, app_formno as Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,''Sections ,r.Batch_Year,''parent_addressP,''parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,seattype,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,(select TextVal from TextValTable where TextCode =ISNULL( r.seattype,0)) as Seat_Type    from applyn r ,Degree d,course c,Department dt,collinfo co where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code and d.college_code='" + collegecode + "' ";
                        query = query + " and r.app_formno='" + rollno + "'";
                    }
                }
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        txt_name.Text = ds1.Tables[0].Rows[i]["stud_name"].ToString();
                        txt_batch.Text = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_degree.Text = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_dept.Text = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_seattype.Text = ds1.Tables[0].Rows[i]["Seat_Type"].ToString();
                        cursem = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        txt_sem.Text = cursem;
                        txt_colg.Text = ds1.Tables[0].Rows[i]["collname"].ToString();
                        txt_strm.Text = ds1.Tables[0].Rows[i]["type"].ToString();
                        txt_sec.Text = ds1.Tables[0].Rows[i]["Sections"].ToString();
                        //string seatype = ds1.Tables[0].Rows[i]["seattype"].ToString();
                        //Session["seatype"] = seatype;
                        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        //Session["clgcode"] = Convert.ToString(ds1.Tables[0].Rows[i]["college_code"]);
                        //if (Session["clgcode"] != null)
                        //    collegecode = Convert.ToString(Session["clgcode"]);
                        //else
                        //    collegecode = Convert.ToString(Session["collegecode"]);
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        rollno = d2.GetFunction("select roll_no from registration where reg_no='" + rollno + "'");

                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        rollno = d2.GetFunction("select roll_no from registration where Roll_admit='" + rollno + "'");

                    image2.ImageUrl = "~/Handler4.ashx?rollno=" + rollno;
                    enableval();
                }
                else
                    clear();
            }
            else
                clear();
        }
        catch (Exception ex) { }
    }
    protected void enableval()
    {
        try
        {
            txt_batch.Enabled = false;
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
            txt_sec.Enabled = false;
            txt_seattype.Enabled = false;
            txt_sem.Enabled = false;
            txt_colg.Enabled = false;
            txt_strm.Enabled = false;
            txt_name.Enabled = false;
        }
        catch { }
    }
    protected void clear()
    {
        try
        {
            txt_roll.Text = "";
            txt_batch.Text = "";
            txt_degree.Text = "";
            txt_dept.Text = "";
            txt_sec.Text = "";
            txt_seattype.Text = "";
            txt_sem.Text = "";
            txt_colg.Text = "";
            txt_strm.Text = "";
            txt_name.Text = "";
        }
        catch { }
    }
    #region
    public void bindclg()
    {
        try
        {
            //ds.Clear();
            ddl_colg.Items.Clear();
            reuse.bindCollegeToDropDown(usercode, ddl_colg);
            //string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            //ds = d2.select_method_wo_parameter(selectQuery, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddl_colg.DataSource = ds;
            //    ddl_colg.DataTextField = "collname";
            //    ddl_colg.DataValueField = "college_code";
            //    ddl_colg.DataBind();
            //}
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindsect();
            bindstream();
        }
        catch (Exception ex) { }
    }
    public void bindBtch()
    {
        try
        {
            ddl_batch.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
            binddeg();
            binddept();
        }
        catch (Exception ex) { }
    }
    public void binddeg()
    {
        try
        {
            ddl_degree.Items.Clear();

            string batch = "";
            if (ddl_colg.Items.Count > 0 && ddl_batch.Items.Count > 0)
            {
                batch = Convert.ToString(ddl_batch.SelectedValue.ToString());
                string stream = "";
                stream = Convert.ToString(ddl_strm.SelectedValue.ToString());
                if (batch != "")
                {
                    ds.Clear();

                    string sel = " select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + Convert.ToString(ddl_colg.SelectedValue) + "')  ";
                    if (stream != "")
                    {
                        sel = sel + "  and type in ('" + stream + "')";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sel, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_degree.DataSource = ds;
                        ddl_degree.DataTextField = "course_name";
                        ddl_degree.DataValueField = "course_id";
                        ddl_degree.DataBind();
                    }
                }
            }
        }
        catch (Exception ex) { }
    }
    public void binddept()
    {
        try
        {
            ddl_dept.Items.Clear();
            string degree = "";
            if (ddl_degree.Items.Count > 0 && ddl_colg.Items.Count > 0)
            {
                degree = Convert.ToString(ddl_degree.SelectedValue.ToString());

                if (degree != "")
                {
                    //ds.Clear();
                    //ds = d2.BindBranchMultiple(singleuser, group_user, degree, ddl_colg.SelectedItem.Value, usercode);
                    string sel = " select dt.Dept_Name,d.degree_code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.Course_Id in('" + degree + "') and d.college_code in('" + ddl_colg.SelectedItem.Value + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sel, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_dept.DataSource = ds;
                        ddl_dept.DataTextField = "dept_name";
                        ddl_dept.DataValueField = "degree_code";
                        ddl_dept.DataBind();

                    }
                }
            }
        }
        catch (Exception ex) { }
    }
    public void bindsem()
    {
        try
        {
            if (ddl_colg.Items.Count > 0 && ddl_batch.Items.Count > 0 && ddl_dept.Items.Count > 0)
            {
                DataSet ds3 = new DataSet();
                ddl_sem.Items.Clear();
                Boolean first_year;
                first_year = false;
                int duration = 0;
                int i = 0;


                string sqluery = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code= (" + ddl_dept.SelectedValue.ToString() + ") and batch_year  = (" + ddl_batch.SelectedValue.ToString() + ") and college_code=" + ddl_colg.SelectedValue.ToString() + "";

                ds3 = d2.select_method_wo_parameter(sqluery, "text");
                if (ds3.Tables.Count > 0)
                {
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
                        duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["ndurations"]);
                        for (i = 1; i <= duration; i++)
                        {
                            if (first_year == false)
                            {
                                ddl_sem.Items.Add(i.ToString());
                            }
                            else if (first_year == true && i != 2)
                            {
                                ddl_sem.Items.Add(i.ToString());
                            }

                        }
                    }
                    else
                    {
                        sqluery = "select distinct duration,first_year_nonsemester  from degree where degree_code in (" + ddl_dept.SelectedValue.ToString() + ") and college_code=" + ddl_colg.SelectedValue.ToString() + "";
                        ddl_sem.Items.Clear();
                        ds3 = d2.select_method_wo_parameter(sqluery, "text");
                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
                            duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["duration"]);
                            for (i = 1; i <= duration; i++)
                            {
                                if (first_year == false)
                                {

                                    ddl_sem.Items.Add(i.ToString());
                                }
                                else if (first_year == true && i != 2)
                                {

                                    ddl_sem.Items.Add(i.ToString());
                                }
                            }
                        }
                    }
                }
            }

        }
        catch (Exception ex) { }
    }
    public void bindSeat()
    {
        ddl_seattype.Items.Clear();
        try
        {
            if (ddl_colg.Items.Count > 0)
            {
                DataSet dsSeat = new DataSet();
                dsSeat = d2.select_method_wo_parameter("select TextVal,Textcode from TextValTable where textcriteria='seat' and college_code='" + ddl_colg.SelectedValue + "' order by Textval asc", "Text");
                if (dsSeat.Tables.Count > 0 && dsSeat.Tables[0].Rows.Count > 0)
                {
                    ddl_seattype.DataSource = dsSeat;
                    ddl_seattype.DataTextField = "TextVal";
                    ddl_seattype.DataValueField = "Textcode";
                    ddl_seattype.DataBind();
                }
            }
        }
        catch (Exception ex) { }
    }
    public string bindstudsem(int semester, string college)
    {
        string semesterquery = "";

        string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + college + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(settingquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
            if (linkvalue == "0")
            {
                semesterquery = d2.GetFunction("select * from textvaltable where TextCriteria = 'FEECA'and textval like '" + semester + " Semester' and textval not like '-1%' and college_code ='" + college + "'");

            }
            else
            {
                semesterquery = d2.GetFunction("select * from textvaltable where TextCriteria = 'FEECA'and textval like '" + semester + " Year' and textval not like '-1%' and college_code ='" + college + "'");

            }
        }

        return semesterquery;
    }
    public void bindsect()
    {
        try
        {
            ddl_sec.Items.Clear();
            if (ddl_colg.Items.Count > 0 && ddl_batch.Items.Count > 0 && ddl_dept.Items.Count > 0 && ddl_sem.Items.Count > 0)
            {

                string branch = ddl_dept.SelectedValue.ToString();
                string batch = ddl_batch.SelectedValue.ToString();
                ListItem item = new ListItem("Empty", " ");
                string sqlquery = "select distinct sections from registration where batch_year=" + batch + " and degree_code=" + branch + " and college_code=" + ddl_colg.SelectedValue.ToString() + " and Current_Semester=" + ddl_sem.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";

                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(sqlquery, "text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_sec.DataSource = ds;
                        ddl_sec.DataTextField = "sections";
                        ddl_sec.DataValueField = "sections";
                        ddl_sec.DataBind();
                        ddl_sec.Enabled = true;

                    }
                    else
                    {
                        ddl_sec.Enabled = false;
                    }
                }
                else
                {
                    ddl_sec.Enabled = false;
                }
                // ddl_sec.Items.Add(item);
            }

        }
        catch (Exception ex) { }
    }
    public void bindstream()
    {
        try
        {
            ddl_strm.Items.Clear();
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + ddl_colg.SelectedItem.Value + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm.DataSource = ds;
                ddl_strm.DataTextField = "type";
                ddl_strm.DataValueField = "type";
                ddl_strm.DataBind();
                ddl_strm.Enabled = true;
            }
            else
            {
                ddl_strm.Enabled = false;
            }
        }
        catch (Exception ex) { }
    }

    protected void ddl_colg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode = Convert.ToString(ddl_colg.SelectedItem.Value);
            bindstream();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindSeat();
            bindsect();
        }
        catch (Exception ex) { }

    }
    protected void ddl_strm_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddeg();
        binddept();
        bindsem();
        bindsect();
    }
    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddeg();
            binddept();
            bindsem();
            bindsect();
        }
        catch (Exception ex) { }
    }
    protected void ddl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddept();
            bindsem();
            bindsect();
        }
        catch (Exception ex) { }
    }
    protected void ddl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        bindsect();
    }
    protected void ddl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsect();
    }
    protected void ddl_seattype_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void txt_roll_noNotApp_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_roll_no.Text.Trim() != "")
            {
                string rollNo = d2.GetFunction("select roll_no from Registration where roll_no='" + txt_roll_no.Text.Trim() + "'").Trim();
                if (rollNo != "0")
                {
                    // imgAlert.Visible = true;
                    // lbl_alert.Text = "Roll No Already Exists";
                    txt_roll_no.Text = "";
                }
            }
        }
        catch { }
    }
    #endregion

    protected void btntransfer_Click(object sender, EventArgs e)
    {
        try
        {

            bool save = false;
            string chngeClgCode = string.Empty;
            string batchyr = string.Empty;
            string degcode = string.Empty;
            string deptcode = string.Empty;
            string feecat = string.Empty;
            string sec = string.Empty;
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string rollno = Convert.ToString(txt_roll.Text);
            if (ddl_colg.Items.Count > 0)
                chngeClgCode = Convert.ToString(ddl_colg.SelectedItem.Value);
            if (ddl_batch.Items.Count > 0)
                batchyr = Convert.ToString(ddl_batch.SelectedItem.Value);
            if (ddl_degree.Items.Count > 0)
                degcode = Convert.ToString(ddl_degree.SelectedItem.Value);
            if (ddl_dept.Items.Count > 0)
                deptcode = Convert.ToString(ddl_dept.SelectedItem.Value);
            if (ddl_sem.Items.Count > 0)
                feecat = Convert.ToString(ddl_sem.SelectedItem.Value);
            if (ddl_sec.Items.Count > 0)
                sec = Convert.ToString(ddl_sec.SelectedItem.Value);
            string fromdate = txt_date.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();

            if (!string.IsNullOrEmpty(rollno))
            {
                string app_no = "";
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    app_no = d2.GetFunction("select app_no from Registration where roll_no='" + rollno + "' and college_code='" + collegecode + "'");

                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    app_no = d2.GetFunction("select app_no from Registration where Reg_no='" + rollno + "' and college_code='" + collegecode + "'");

                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    app_no = d2.GetFunction("select app_no from Registration where Roll_admit='" + rollno + "' and college_code='" + collegecode + "'");
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                    app_no = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "' and college_code='" + collegecode + "'");
                if (app_no != "0")
                {
                    string SelReg = string.Empty;
                    bool boolTrans = false;
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                        SelReg = " select app_no,reg_no,roll_no,degree_code,batch_year,sections,college_code from registration where  app_no='" + app_no + "' and college_code='" + collegecode + "'";
                    else
                    {
                        boolTrans = true;
                        SelReg = " select app_no,app_formno as reg_no,app_formno as roll_no,degree_code,batch_year,''sections,college_code from applyn where  app_no='" + app_no + "' and college_code='" + collegecode + "'";
                    }
                    //college_code='" + collegecode + "' and
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(SelReg, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string batch = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                        string olddeg = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]);
                        string oldsec = Convert.ToString(ds.Tables[0].Rows[0]["sections"]);
                        string oldcolg = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                        bool saveval = CheckInsert(olddeg, deptcode, oldsec, sec, oldcolg, chngeClgCode);
                        if (saveval)
                            save = transfer(app_no, olddeg, deptcode, oldsec, sec, oldcolg, chngeClgCode, fromdate, batch, boolTrans);
                        else
                            save = false;
                    }
                }
                if (save == true)
                {
                    clear();
                    divalert.Visible = true;
                    lbalert.Text = "Transfer Succeed";
                    // Response.Write("<script>alert('Transfer Succeed')</script>");
                }
                else
                {
                    divalert.Visible = true;
                    lbalert.Text = "Please Select Differenct Categories";
                    // Response.Write("<script>alert('Please Select Differenct Categories')</script>");
                }
            }
            else
            {
                divalert.Visible = true;
                lbalert.Text = "You Enter Wrong Number";
                //   Response.Write("<script>alert('You Enter Wrong Number')</script>");
            }
        }
        catch { }
    }

    protected bool transfer(string app_no, string olddeg, string deptcode, string oldsec, string sec, string oldcolg, string chngeClgCode, string fromdate, string batch, bool boolTrans)
    {
        bool save = false;
        try
        {
            string insQ = "  insert into ST_Student_Transfer(AppNo,TransferDate,TransferTime,FromDegree,Todegree,FromSection,ToSection,FromCollege,Tocollege) values('" + app_no + "','" + fromdate + "','" + DateTime.Now.ToShortTimeString() + "','" + olddeg + "','" + deptcode + "','" + oldsec + "','" + sec + "','" + oldcolg + "','" + chngeClgCode + "')";
            int ins = d2.update_method_wo_parameter(insQ, "Text");
            if (ins > 0)
            {
                string StudPK = d2.GetFunction("select studentTransferPK from ST_Student_Transfer where AppNo='" + app_no + "' and TransferDate='" + fromdate + "' and FromDegree='" + olddeg + "' and FromSection='" + oldsec + "' and FromCollege='" + oldcolg + "'");
                if (StudPK != "0")
                {
                    string oldRoll = string.Empty;
                    string oldReg = string.Empty;
                    string oldRollAdmit = string.Empty;
                    string studAdmDate = string.Empty;
                    string selQReg = string.Empty;
                    if (!boolTrans)
                    {
                        selQReg = " select roll_no,reg_no,roll_admit,adm_date from registration where app_no='" + app_no + "'";
                    }
                    else
                    {
                        selQReg = " select app_formno as roll_no,app_formno as reg_no,app_formno as roll_admit,''adm_date from applyn where app_no='" + app_no + "'";
                    }
                    DataSet dsReg = d2.select_method_wo_parameter(selQReg, "Text");
                    if (dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0)
                    {
                        oldRoll = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_no"]);
                        oldReg = Convert.ToString(dsReg.Tables[0].Rows[0]["reg_no"]);
                        oldRollAdmit = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_admit"]);
                        studAdmDate = Convert.ToString(dsReg.Tables[0].Rows[0]["adm_date"]);
                    }
                    string insStudDetails = " insert into st_student_transfer_details(studentTransferfK,old_rollno,Old_RegNo,Old_RollAdmit,stud_admDate) values('" + StudPK + "','" + oldRoll + "','" + oldReg + "','" + oldRollAdmit + "','" + studAdmDate + "')";
                    int inss = d2.update_method_wo_parameter(insStudDetails, "Text");
                }
                if (boolTrans)
                {
                    string updQ = "update applyn set  degree_code='" + deptcode + "', batch_year='" + batch + "',college_code='" + chngeClgCode + "' where app_no='" + app_no + "' ";
                    int upd = d2.update_method_wo_parameter(updQ, "Text");
                }
                else
                {
                    string updQ = "update registration set sections='" + sec + "', degree_code='" + deptcode + "', batch_year='" + batch + "',college_code='" + chngeClgCode + "' where app_no='" + app_no + "' ";
                    int upd = d2.update_method_wo_parameter(updQ, "Text");
                }
                save = true;
            }
        }
        catch { }
        return save;
    }
    protected bool CheckInsert(string olddeg, string deptcode, string oldsec, string sec, string oldcolg, string chngeClgCode)
    {
        bool check = false;
        try
        {
            if (oldcolg == chngeClgCode)
            {
                if (olddeg == deptcode)
                {
                    if (!string.IsNullOrEmpty(oldsec) && !string.IsNullOrEmpty(sec))
                    {
                        if (oldsec != sec)
                            check = true;
                        else
                            check = false;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(oldsec) && !string.IsNullOrEmpty(sec))
                            check = true;
                        else if (!string.IsNullOrEmpty(oldsec) && string.IsNullOrEmpty(sec))
                            check = true;
                        else
                            check = false;
                    }
                }
                else
                    check = true;
            }
            else
                check = true;
        }
        catch { }
        return check;
    }

    #endregion

    protected void rbmode_Selected(object sender, EventArgs e)
    {
        if (rbmode.SelectedIndex == 0)
        {
            diventry.Visible = true;
            clear();
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

        lbl.Add(lblclg);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        //
        lbl.Add(lblclgs);
        lbl.Add(lbl_str2);
        lbl.Add(lbldegs);
        lbl.Add(lbldepts);
        lbl.Add(lblsems);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        //           
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    //
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcollege.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            colgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
    }
    protected void btnalert_Click(object sender, EventArgs e)
    {
        divalert.Visible = false;
    }

    // last modified 12-11-2016 sudhagar
}