using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AjaxControlToolkit;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using FarPoint.Web.Spread.Design;
using Gios.Pdf;

public partial class Admission_Report : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    DataView dv = new DataView();
    DataView dv1 = new DataView();
    DataSet dnew = new DataSet();
    string collegecode = "";
    string usercode = "";
    string columnfield = string.Empty;
    string singleuser = "";
    string group_user = "";
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
            usercode = Session["usercode"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                type();
                batch();
                report();
                edu();
                rptprint.Visible = false;
                fpspread.Visible = false;
            }
            lblnorec.Visible = false;
            Errorlable.Visible = false;
        }
        catch (Exception ex) { }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("default.aspx");
        }
        catch (Exception ex)
        {
        }
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




        lbl.Add(lblcollege);

        

        fields.Add(0);

     


        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }


    public void loadcollege()
    {
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
        {
            columnfield = " and group_code='" + group_user + "'";
        }
        else
        {
            columnfield = " and user_code='" + Session["usercode"] + "'";
        }
        hat.Clear();
        hat.Add("column_field", columnfield.ToString());
        ds.Dispose();
        ds.Reset();
        ds = da.select_method("bind_college", hat, "sp");
        ddlcollege.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();

        }
    }

    public void type()
    {
        ds.Dispose();
        ds.Reset();
        ds = da.select_method_wo_parameter("select distinct type  from Course where college_code ='" + ddlcollege.SelectedItem.Value + "'", "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddltype.DataSource = ds;
            ddltype.DataTextField = "type";
            ddltype.DataValueField = "type";
            ddltype.DataBind();
        }

    }
    public void batch()
    {


        ds.Dispose();
        ds.Reset();
        ds = da.BindBatch();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlbatch.DataSource = ds;
            ddlbatch.DataTextField = "Batch_year";
            ddlbatch.DataValueField = "Batch_year";
            ddlbatch.DataBind();
            ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
        }


    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpspread.Visible = false;
        rptprint.Visible = false;
        type();

    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpspread.Visible = false;
        rptprint.Visible = false;

    }
    protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpspread.Visible = false;
        rptprint.Visible = false;
        if (ddlreport.SelectedIndex == 1 || ddlreport.SelectedIndex == 3)
        {
            txtbatch.Enabled = false;
        }
        else
        {
            txtbatch.Enabled = true;
        }
        if (ddlreport.SelectedIndex == 3)
        {
            ddlbatch.Enabled = false;
        }
        else
        {
            ddlbatch.Enabled = true;
        }
    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpspread.Visible = false;
        rptprint.Visible = false;
        edu();

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            if (appPath != "")
            {
                strexcelname = txtexcelname.Text;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {
                    print = strexcelname;
                    //FpEntry.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
                    //Aruna on 26feb2013============================
                    string szPath = appPath + "/Report/";
                    string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                    fpspread.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Flush();
                    Response.WriteFile(szPath + szFile);
                    //=============================================
                }
                else
                {
                    lblnorec.Text = "Please enter your Report Name";
                    lblnorec.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }

    }



    public void report()
    {
        ddlreport.Items.Add("Form I");
        ddlreport.Items.Add("Form II");
        ddlreport.Items.Add("Form III");
        ddlreport.Items.Add("Form IV");
        ddlreport.Items.Add("Form V");
        ddlreport.DataBind();
    }
    public void edu()
    {
        try
        {
            int count1 = 0;
            chklsbatch.Items.Clear();
            ds = da.select_method_wo_parameter("select distinct Edu_Level from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + ddlcollege.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' and course.type='" + ddltype.SelectedItem.Text + "'  order by Edu_Level desc", "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                chklsbatch.DataSource = ds;
                chklsbatch.DataTextField = "Edu_Level";
                chklsbatch.DataValueField = "Edu_Level";
                chklsbatch.DataBind();

            }
            if (chklsbatch.Items.Count > 0)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                }
                txtbatch.Text = "Edu Level(" + (chklsbatch.Items.Count) + ")";
            }


        }
        catch (Exception ex)
        {

        }
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            string college = Convert.ToString(ddlcollege.SelectedItem.Value);
            string type = Convert.ToString(ddltype.SelectedItem.Text);
            string batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            string edulevel = "";
            ArrayList edulevelarray = new ArrayList();
            string mainvalue = "";
            string setvalue = "";

            if (chklsbatch.Items.Count > 0)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        edulevelarray.Add(chklsbatch.Items[i].Text);
                        if (mainvalue == "")
                        {
                            mainvalue = chklsbatch.Items[i].Text;
                            setvalue = chklsbatch.Items[i].Text + "" + "(I year)";
                        }
                        else
                        {
                            mainvalue = mainvalue + "'" + "," + "'" + chklsbatch.Items[i].Text;
                            setvalue = setvalue + "," + chklsbatch.Items[i].Text + "" + "(I year)";
                        }
                    }
                }
            }

            string selectquery = "";
            //string selectquery = "select Distinct a.app_no ,r.degree_code,Edu_Level,C.type,sex ,r.batch_year,d.college_code,criteria_Code,community  from applyn a ,Registration r ,Degree d  ,Department dt ,Course c ,selectcriteria s   where a.app_no =r.App_No and a.degree_code =r.degree_code and a.degree_code =d.Degree_Code  and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id   and admission_status ='1' and s.degree_code =d.Degree_Code and s.degree_code =r.degree_code and s.degree_code =a.degree_code  and s.app_no =a.app_no and s.app_no =r.App_No and s.college_code =d.college_code and isapprove =4 and admit_confirm =1 and CC =0 and DelFlag =0 and Exam_Flag <>'DEBAR' and a.college_code =d.college_code  and r.batch_year =" + batch + " and Edu_Level in ('" + mainvalue + "')   and type ='" + type + "' and d.college_code =" + college + "";
            if (ddlreport.SelectedIndex == 0)
            {
                selectquery = " select Distinct a.app_no ,r.degree_code,Edu_Level,C.type,sex ,r.batch_year,d.college_code,community  from applyn a ,Registration r ,Degree d  ,Department dt ,Course c where a.app_no =r.App_No and a.degree_code =r.degree_code and a.degree_code =d.Degree_Code  and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and CC =0   and DelFlag =0 and Exam_Flag <>'DEBAR' and a.college_code =d.college_code  and r.batch_year =" + batch + " and Edu_Level in ('" + mainvalue + "')  and type ='" + type + "' and d.college_code =" + college + " and is_Enroll=2";
                selectquery = selectquery + "  select SUM(No_Of_seats),Edu_Level,type  from Degree d,Department dt,Course c,DeptPrivilages dp where dp.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.college_code =" + college + " and user_code =" + usercode + " and Edu_Level in ('" + mainvalue + "') and type ='" + type + "' group by Edu_Level,type ";
                selectquery = selectquery + "  select * from TextValTable where TextCriteria ='comm' and TextCriteria2 ='comm1' and college_code ='" + college + "' order by TextVal ";
                ds.Clear();
                ds = da.select_method_wo_parameter(selectquery, "Text");
                Session["data"] = ds;
                form1(college, batch, type, edulevelarray, ds, setvalue);
            }
            else if (ddlreport.SelectedIndex == 1)
            {
                selectquery = " select Distinct a.app_no ,r.degree_code,Edu_Level,C.type,sex ,r.batch_year,d.college_code,community  from applyn a ,Registration r ,Degree d  ,Department dt ,Course c where a.app_no =r.App_No and a.degree_code =r.degree_code and a.degree_code =d.Degree_Code  and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and CC =0   and DelFlag =0 and Exam_Flag <>'DEBAR' and a.college_code =d.college_code  and r.batch_year =" + batch + "  and type ='" + type + "' and d.college_code =" + college + " and is_Enroll=2";
                selectquery = selectquery + "  select SUM(No_Of_seats),Edu_Level,type  from Degree d,Department dt,Course c,DeptPrivilages dp where dp.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.college_code =" + college + " and user_code =" + usercode + " and Edu_Level in ('" + mainvalue + "') and type ='" + type + "' group by Edu_Level,type ";
                selectquery = selectquery + "  select * from TextValTable where TextCriteria ='comm' and TextCriteria2 ='comm1' and college_code ='" + college + "' order by TextVal ";
                ds.Clear();
                ds = da.select_method_wo_parameter(selectquery, "Text");
                Session["data"] = ds;
                form2(college, batch, type, edulevelarray, ds, setvalue);
            }
            else if (ddlreport.SelectedIndex == 2)
            {
                selectquery = " select Distinct a.app_no ,r.degree_code,Edu_Level,C.type,sex ,r.batch_year,d.college_code,community  from applyn a ,Registration r ,Degree d  ,Department dt ,Course c where a.app_no =r.App_No and a.degree_code =r.degree_code and a.degree_code =d.Degree_Code  and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and CC =0   and DelFlag =0 and Exam_Flag <>'DEBAR' and a.college_code =d.college_code  and Edu_Level in ('" + mainvalue + "') and r.batch_year =" + batch + "  and type ='" + type + "' and d.college_code =" + college + " and is_Enroll=2";
                selectquery = selectquery + "  select SUM(No_Of_seats),Edu_Level,type  from Degree d,Department dt,Course c,DeptPrivilages dp where dp.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.college_code =" + college + " and user_code =" + usercode + " and Edu_Level in ('" + mainvalue + "')  and type ='" + type + "' group by Edu_Level,type ";
                selectquery = selectquery + "  select * from TextValTable where TextCriteria ='comm' and TextCriteria2 ='comm1' and college_code ='" + college + "' order by TextVal ";
                ds.Clear();
                ds = da.select_method_wo_parameter(selectquery, "Text");
                Session["data"] = ds;
                form3(college, batch, type, edulevelarray, ds, setvalue);
            }
            else if (ddlreport.SelectedIndex == 3)
            {
                selectquery = " select Distinct a.app_no ,r.degree_code,Edu_Level,C.type,sex ,r.batch_year,d.college_code,community  from applyn a ,Registration r ,Degree d  ,Department dt ,Course c where a.app_no =r.App_No and a.degree_code =r.degree_code and a.degree_code =d.Degree_Code  and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and CC =0   and DelFlag =0 and Exam_Flag <>'DEBAR' and a.college_code =d.college_code   and type ='" + type + "' and d.college_code =" + college + " and is_Enroll=2";
                selectquery = selectquery + "  select SUM(No_Of_seats),Edu_Level,type  from Degree d,Department dt,Course c,DeptPrivilages dp where dp.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.college_code =" + college + " and user_code =" + usercode + " and Edu_Level in ('" + mainvalue + "') and type ='" + type + "' group by Edu_Level,type ";
                selectquery = selectquery + "  select * from TextValTable where TextCriteria ='comm' and TextCriteria2 ='comm1' and college_code ='" + college + "' order by TextVal ";
                ds.Clear();
                ds = da.select_method_wo_parameter(selectquery, "Text");
                Session["data"] = ds;
                form2(college, batch, type, edulevelarray, ds, setvalue);
            }
            else if (ddlreport.SelectedIndex == 4)
            {
                ArrayList checkarray = new ArrayList();
                DataView dcom = new DataView();
                DataView dcom1 = new DataView();
                string selectvalue = "";
                string selctnewquery = "select * from TextValTable where TextCriteria = 'comm' and TextCriteria2 ='comm1' and (TextVal ='SC' or TextVal ='ST')";
                dnew.Clear();
                dnew = da.select_method_wo_parameter(selctnewquery, "Text");
                if (dnew.Tables[0].Rows.Count > 0)
                {
                    selectvalue = Convert.ToString(dnew.Tables[0].Rows[0]["TextCode"]);
                    selectvalue = selectvalue + "'" + "," + "'" + Convert.ToString(dnew.Tables[0].Rows[1]["TextCode"]);
                    checkarray.Add(dnew.Tables[0].Rows[0]["TextCode"]);
                    checkarray.Add(dnew.Tables[0].Rows[1]["TextCode"]);
                }

                selectquery = " select distinct t.TextCode,T.TextVal,c.Edu_Level from Degree d,Department dt,Course c,tbl_DeptGrouping tg,TextValTable t  where  dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and t.TextCode =tg.Groupcode and Tg.Deptcode =dt.Dept_Code  and d.college_code ='" + college + "' and c.type ='" + type + "' and c.Edu_Level in ('" + mainvalue + "') order by Edu_Level desc";
                selectquery = selectquery + "   select distinct t.TextCode,T.TextVal from Degree d,Department dt,Course c,tbl_DeptGrouping tg,TextValTable t  where  dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and t.TextCode =tg.Groupcode and Tg.Deptcode =dt.Dept_Code and d.college_code ='" + college + "' and c.type ='" + type + "' and c.Edu_Level in ('" + mainvalue + "')";
                selectquery = selectquery + "   select COUNT(*),tg.Groupcode,a.community,Edu_Level   from applyn a, Registration r,Degree d,Department dt,Course c,tbl_DeptGrouping tg where a.app_no =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code = dt.Dept_Code and d.Dept_Code =tg.Deptcode and dt.Dept_Code = tg.Deptcode and d.Course_Id =c.Course_Id  and c.type =tg.type and r.Batch_Year =" + ddlbatch.SelectedItem.Value + " and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and c.type ='" + type + "' and c.Edu_Level in ('" + mainvalue + "') and a.community in ('" + selectvalue + "') group by tg.Groupcode ,a.community,Edu_Level";
                selectquery = selectquery + "   select COUNT(*),tg.Groupcode,Edu_Level  from  Registration r,Degree d,Department dt,Course c,tbl_DeptGrouping tg where  r.degree_code =d.Degree_Code and d.Dept_Code = dt.Dept_Code and d.Dept_Code =tg.Deptcode and dt.Dept_Code = tg.Deptcode and d.Course_Id =c.Course_Id and c.type =tg.type and r.Batch_Year =" + ddlbatch.SelectedItem.Value + " and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and c.type ='" + type + "' and c.Edu_Level in ('" + mainvalue + "')  group by tg.Groupcode,Edu_Level ";
               
                //selectquery = selectquery + "   select distinct r.App_No,case when  percentage < 2 then 10 when percentage < 3 then '20'  when percentage < 4 then '30' when percentage < 5 then '40' when percentage < 6 then '50' else percentage end as percentage ,Edu_Level,tg.Groupcode,a.community from applyn a,Registration r,Stud_prev_details sd,perv_marks_history p,Degree d,Department dt,Course c,tbl_DeptGrouping tg where a.app_no =r.App_No and  r.degree_code =d.Degree_Code and d.Dept_Code = dt.Dept_Code and d.Dept_Code =tg.Deptcode and dt.Dept_Code = tg.Deptcode and d.Course_Id =c.Course_Id and c.type =tg.type and r.App_No=sd.app_no and sd.course_entno =p.course_entno and r.Batch_Year =" + ddlbatch.SelectedItem.Value + " and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and c.type ='" + type + "' and c.Edu_Level in ('" + mainvalue + "') and a.community in ('" + selectvalue + "') and percentage <>0 and percentage >40 group by r.App_No,percentage,Edu_Level,tg.Groupcode,a.community order by percentage asc"; //23nov2017

                selectquery = selectquery + "   select distinct r.App_No,case when  percentage < 2 then 10 when percentage < 3 then '20'  when percentage < 4 then '30' when percentage < 5 then '40' when percentage < 6 then '50' else percentage end as percentage ,Edu_Level,tg.Groupcode,a.community from applyn a,Registration r,Stud_prev_details sd,Degree d,Department dt,Course c,tbl_DeptGrouping tg where a.app_no =r.App_No and  r.degree_code =d.Degree_Code and d.Dept_Code = dt.Dept_Code and d.Dept_Code =tg.Deptcode and dt.Dept_Code = tg.Deptcode and d.Course_Id =c.Course_Id and c.type =tg.type and r.App_No=sd.app_no  and r.Batch_Year =" + ddlbatch.SelectedItem.Value + " and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and c.type ='" + type + "' and c.Edu_Level in ('" + mainvalue + "') and a.community in ('" + selectvalue + "') and percentage <>0 and percentage >40 group by r.App_No,percentage,Edu_Level,tg.Groupcode,a.community order by percentage asc";  //23nov2017

                selectquery = selectquery + "   select count(*) as total,t.Groupcode,c.Edu_Level from applyn a,Degree d,Department dt,tbl_DeptGrouping t, Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and t.Deptcode = d.Dept_Code and t.Deptcode = dt.Dept_Code and c.type =t.type and c.type in ('" + type + "') and c.Edu_Level in ('" + mainvalue + "') and batch_year =" + ddlbatch.SelectedItem.Value + " and isconfirm ='1' group by t.Groupcode ,c.Edu_Level";
                selectquery = selectquery + "   select count(*) as total,t.Groupcode,c.Edu_Level,a.community from applyn a,Degree d,Department dt,tbl_DeptGrouping t, Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and t.Deptcode = d.Dept_Code and t.Deptcode = dt.Dept_Code and c.type =t.type and c.type in ('" + type + "') and c.Edu_Level in ('" + mainvalue + "') and batch_year =" + ddlbatch.SelectedItem.Value + " and isconfirm ='1' and a.community in ('" + selectvalue + "') group by t.Groupcode ,c.Edu_Level,a.community";
                selectquery = selectquery + "   select count(*) as total,t.Groupcode,c.Edu_Level from applyn a,Degree d,Department dt,tbl_DeptGrouping t, Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and t.Deptcode = d.Dept_Code and t.Deptcode = dt.Dept_Code and c.type =t.type and c.type in ('" + type + "') and c.Edu_Level in ('" + mainvalue + "') and batch_year =" + ddlbatch.SelectedItem.Value + " and isconfirm ='1' and admission_status ='1' group by t.Groupcode ,c.Edu_Level";
                selectquery = selectquery + "   select count(*) as total,t.Groupcode,c.Edu_Level,a.community from applyn a,Degree d,Department dt,tbl_DeptGrouping t, Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and t.Deptcode = d.Dept_Code and t.Deptcode = dt.Dept_Code and c.type =t.type and c.type in ('" + type + "') and c.Edu_Level in ('" + mainvalue + "') and batch_year =" + ddlbatch.SelectedItem.Value + " and isconfirm ='1' and a.community in ('" + selectvalue + "') and admission_status ='1' group by t.Groupcode ,c.Edu_Level,a.community";

                ds.Clear();
                ds = da.select_method_wo_parameter(selectquery, "Text");
                Session["data"] = ds;
                //form2(college, batch, type, edulevelarray, ds, setvalue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fpspread.Sheets[0].RowCount = 0;
                    fpspread.Sheets[0].ColumnCount = 0;
                    fpspread.Sheets[0].ColumnHeader.RowCount = 2;
                    fpspread.Sheets[0].ColumnCount = 2;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "1";
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                    fpspread.RowHeader.Visible = false;
                    fpspread.CommandBar.Visible = false;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ds.Tables[0].DefaultView.RowFilter = "TextCode='" + Convert.ToString(ds.Tables[1].Rows[row]["TextCode"]) + "'";
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    fpspread.Sheets[0].ColumnCount++;
                                    fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[row]["TextVal"]);
                                    for (int r = 0; r < dv.Count; r++)
                                    {
                                        if (r == 0)
                                        {
                                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dv[r]["Edu_Level"]);
                                        }
                                        else
                                        {
                                            fpspread.Sheets[0].ColumnCount++;
                                            fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dv[r]["Edu_Level"]);
                                        }

                                    }
                                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - dv.Count, 1, dv.Count);
                                }
                            }
                        }

                        fpspread.Sheets[0].RowCount = 19;
                        fpspread.Sheets[0].Cells[0, 0].Text = "1";
                        fpspread.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[0, 1].Text = "Number of students applied (a) In Total";
                        fpspread.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Left;
                        fpspread.Sheets[0].Cells[0, 1].Font.Name = "Book Antiqua";

                        fpspread.Sheets[0].Cells[1, 0].Text = "";
                        fpspread.Sheets[0].Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[1, 1].Text = "(b) Of with SC";
                        fpspread.Sheets[0].Cells[1, 1].HorizontalAlign = HorizontalAlign.Left;
                        fpspread.Sheets[0].Cells[1, 1].Font.Name = "Book Antiqua";

                        fpspread.Sheets[0].Cells[2, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[2, 1].Text = "(c) Of with ST";
                        fpspread.Sheets[0].Cells[2, 1].HorizontalAlign = HorizontalAlign.Left;
                        fpspread.Sheets[0].Cells[2, 1].Font.Name = "Book Antiqua";

                        fpspread.Sheets[0].Cells[3, 0].Text = "2";
                        fpspread.Sheets[0].Cells[3, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[3, 1].Text = "Number of students Admitted (a) In Total";
                        fpspread.Sheets[0].Cells[3, 1].HorizontalAlign = HorizontalAlign.Left;

                        fpspread.Sheets[0].Cells[4, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[4, 1].Text = "(b) Of with SC";
                        fpspread.Sheets[0].Cells[4, 1].HorizontalAlign = HorizontalAlign.Left;

                        fpspread.Sheets[0].Cells[5, 0].Text = "";
                        fpspread.Sheets[0].Cells[5, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[5, 1].Text = "(c) Of with ST";
                        fpspread.Sheets[0].Cells[5, 1].HorizontalAlign = HorizontalAlign.Left;

                        fpspread.Sheets[0].Cells[6, 0].Text = "3";
                        fpspread.Sheets[0].Cells[6, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[6, 1].Text = "Number of students admitted in general merit list not to be counted towards reservation quota (a) SC";
                        fpspread.Sheets[0].Cells[6, 1].HorizontalAlign = HorizontalAlign.Left;

                        fpspread.Sheets[0].Cells[7, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[7, 1].Text = "(b) ST";
                        fpspread.Sheets[0].Cells[7, 1].HorizontalAlign = HorizontalAlign.Left;
                        fpspread.Sheets[0].SpanModel.Add(7, 2, 1, fpspread.Sheets[0].ColumnCount);

                        fpspread.Sheets[0].Cells[8, 0].Text = "4";
                        fpspread.Sheets[0].Cells[8, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[8, 1].Text = "Percentage of SC & ST students admitted to total number of students admitted (Excluding these admitted in general merit list) (a) SC";
                        fpspread.Sheets[0].Cells[8, 1].HorizontalAlign = HorizontalAlign.Left;

                        fpspread.Sheets[0].Cells[9, 0].Text = "";
                        fpspread.Sheets[0].Cells[9, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[9, 1].Text = "(b) ST";
                        fpspread.Sheets[0].Cells[9, 1].HorizontalAlign = HorizontalAlign.Left;
                        // fpspread.Sheets[0].SpanModel.Add(8, 2, 1, fpspread.Sheets[0].ColumnCount);

                        // old

                        fpspread.Sheets[0].Cells[10, 0].Text = "5";
                        fpspread.Sheets[0].Cells[10, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[10, 1].Text = "Least percentage of marks of admitted general students(Including SC & ST Students admitted in general merit list)";
                        fpspread.Sheets[0].Cells[10, 1].HorizontalAlign = HorizontalAlign.Left;
                        fpspread.Sheets[0].Cells[10, 1].Font.Name = "Book Antiqua";

                        fpspread.Sheets[0].Cells[11, 0].Text = "6";
                        fpspread.Sheets[0].Cells[11, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[11, 1].Text = "Least percentage of marks of admitted SC & ST Students (a) SC";
                        fpspread.Sheets[0].Cells[11, 1].HorizontalAlign = HorizontalAlign.Left;
                        fpspread.Sheets[0].Cells[11, 1].Font.Name = "Book Antiqua";

                        fpspread.Sheets[0].Cells[12, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[12, 1].Text = "(B) ST";
                        fpspread.Sheets[0].Cells[12, 1].HorizontalAlign = HorizontalAlign.Left;
                        fpspread.Sheets[0].Cells[12, 1].Font.Name = "Book Antiqua";

                        fpspread.Sheets[0].Cells[13, 0].Text = "7";
                        fpspread.Sheets[0].Cells[13, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[13, 1].Text = "Prescribed percentage of reservation (a) SC";
                        fpspread.Sheets[0].Cells[13, 1].HorizontalAlign = HorizontalAlign.Left;

                        fpspread.Sheets[0].Cells[14, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[14, 1].Text = "(b) ST";
                        fpspread.Sheets[0].Cells[14, 1].HorizontalAlign = HorizontalAlign.Left;

                        fpspread.Sheets[0].Cells[15, 0].Text = "8";
                        fpspread.Sheets[0].Cells[15, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[15, 1].Text = "Whether reservation completely fulfilled in respect of (a) SC";
                        fpspread.Sheets[0].Cells[15, 1].HorizontalAlign = HorizontalAlign.Left;

                        fpspread.Sheets[0].Cells[16, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[16, 1].Text = "(b) ST";
                        fpspread.Sheets[0].Cells[16, 1].HorizontalAlign = HorizontalAlign.Left;

                        fpspread.Sheets[0].Cells[17, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[17, 1].Text = "If not precise reason";
                        fpspread.Sheets[0].Cells[17, 1].HorizontalAlign = HorizontalAlign.Left;
                        fpspread.Sheets[0].SpanModel.Add(7, 2, 1, fpspread.Sheets[0].ColumnCount);
                        fpspread.Sheets[0].Cells[18, 0].Text = "9";
                        fpspread.Sheets[0].Cells[18, 0].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[18, 1].Text = "Steps taken by college / University Departments to fill up the reserved seats for which SC & ST students not available";
                        fpspread.Sheets[0].Cells[18, 1].HorizontalAlign = HorizontalAlign.Left;
                        fpspread.Sheets[0].SpanModel.Add(18, 2, 1, fpspread.Sheets[0].ColumnCount);

                        int col = 1;
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "TextCode='" + Convert.ToString(ds.Tables[1].Rows[row]["TextCode"]) + "'";
                                    dv = ds.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        for (int r = 0; r < dv.Count; r++)
                                        {
                                            col++;
                                            ds.Tables[4].DefaultView.RowFilter = "Edu_Level='" + Convert.ToString(dv[r]["Edu_Level"]) + "' and Groupcode='" + Convert.ToString(dv[r]["TextCode"]) + "'";
                                            dv1 = ds.Tables[4].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                fpspread.Sheets[0].Cells[10, col].Text = Convert.ToString(dv1[0]["percentage"]);
                                                fpspread.Sheets[0].Cells[10, col].HorizontalAlign = HorizontalAlign.Center;
                                            }

                                            ds.Tables[5].DefaultView.RowFilter = "Edu_Level='" + Convert.ToString(dv[r]["Edu_Level"]) + "' and Groupcode='" + Convert.ToString(dv[r]["TextCode"]) + "'";
                                            dv1 = ds.Tables[5].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                fpspread.Sheets[0].Cells[0, col].Text = Convert.ToString(dv1[0]["total"]);
                                                fpspread.Sheets[0].Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                                            }

                                            ds.Tables[7].DefaultView.RowFilter = "Edu_Level='" + Convert.ToString(dv[r]["Edu_Level"]) + "' and Groupcode='" + Convert.ToString(dv[r]["TextCode"]) + "'";
                                            dv1 = ds.Tables[7].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                fpspread.Sheets[0].Cells[3, col].Text = Convert.ToString(dv1[0]["total"]);
                                                fpspread.Sheets[0].Cells[3, col].HorizontalAlign = HorizontalAlign.Center;
                                            }


                                            if (checkarray.Count > 0)
                                            {
                                                int rs = 10;
                                                int rs1 = 12;
                                                int rsval = 0;
                                                int rsadd = 3;
                                                for (int c = 0; c < checkarray.Count; c++)
                                                {
                                                    ds.Tables[4].DefaultView.RowFilter = "Edu_Level='" + Convert.ToString(dv[r]["Edu_Level"]) + "' and Groupcode='" + Convert.ToString(dv[r]["TextCode"]) + "' and community ='" + Convert.ToString(checkarray[c]) + "'";
                                                    dv1 = ds.Tables[4].DefaultView;
                                                    rs++;
                                                    rsadd++;
                                                    if (dv1.Count > 0)
                                                    {
                                                        fpspread.Sheets[0].Cells[rs, col].Text = Convert.ToString(dv1[0]["percentage"]);
                                                        fpspread.Sheets[0].Cells[rs, col].HorizontalAlign = HorizontalAlign.Center;
                                                    }
                                                    rsval++;
                                                    if (ds.Tables[2].Rows.Count > 0)
                                                    {
                                                        double comcount = 0;
                                                        double comcontoverall = 0;
                                                        rs1++;

                                                        ds.Tables[2].DefaultView.RowFilter = "Edu_Level='" + Convert.ToString(dv[r]["Edu_Level"]) + "' and Groupcode='" + Convert.ToString(dv[r]["TextCode"]) + "' and community ='" + Convert.ToString(checkarray[c]) + "'";
                                                        dcom = ds.Tables[2].DefaultView;
                                                        if (dcom.Count > 0)
                                                        {
                                                            string comcountvalue = Convert.ToString(dcom[0][0]);
                                                            if (comcountvalue.Trim() == "")
                                                            {
                                                                comcountvalue = "0";
                                                            }
                                                            comcount = Convert.ToDouble(comcountvalue);
                                                        }
                                                        ds.Tables[3].DefaultView.RowFilter = "Edu_Level='" + Convert.ToString(dv[r]["Edu_Level"]) + "' and Groupcode='" + Convert.ToString(dv[r]["TextCode"]) + "'";
                                                        dcom1 = ds.Tables[3].DefaultView;
                                                        if (dcom1.Count > 0)
                                                        {
                                                            string comcountvaluenew = Convert.ToString(dcom1[0][0]);
                                                            if (comcountvaluenew.Trim() == "")
                                                            {
                                                                comcountvaluenew = "0";
                                                            }
                                                            comcontoverall = Convert.ToDouble(comcountvaluenew);
                                                        }
                                                        double totalvalue = comcount / comcontoverall * 100;
                                                        fpspread.Sheets[0].Cells[rs1, col].Text = Convert.ToString(Math.Round(totalvalue, 2));
                                                        fpspread.Sheets[0].Cells[rs1, col].HorizontalAlign = HorizontalAlign.Center;
                                                    }

                                                    ds.Tables[6].DefaultView.RowFilter = "Edu_Level='" + Convert.ToString(dv[r]["Edu_Level"]) + "' and Groupcode='" + Convert.ToString(dv[r]["TextCode"]) + "' and community ='" + Convert.ToString(checkarray[c]) + "'";
                                                    dv1 = ds.Tables[6].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        fpspread.Sheets[0].Cells[rsval, col].Text = Convert.ToString(dv1[0]["total"]);
                                                        fpspread.Sheets[0].Cells[rsval, col].HorizontalAlign = HorizontalAlign.Center;
                                                    }


                                                    ds.Tables[8].DefaultView.RowFilter = "Edu_Level='" + Convert.ToString(dv[r]["Edu_Level"]) + "' and Groupcode='" + Convert.ToString(dv[r]["TextCode"]) + "' and community ='" + Convert.ToString(checkarray[c]) + "'";
                                                    dv1 = ds.Tables[8].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        fpspread.Sheets[0].Cells[rsadd, col].Text = Convert.ToString(dv1[0]["total"]);
                                                        fpspread.Sheets[0].Cells[rsadd, col].HorizontalAlign = HorizontalAlign.Center;
                                                    }
                                                }
                                            }



                                        }
                                    }
                                }
                            }
                        }
                    }
                    fpspread.Visible = true;
                    fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                    fpspread.Sheets[0].AutoPostBack = true;
                    rptprint.Visible = true;
                }
            }
        }
        catch
        {

        }
    }
    public void form3(string college, string batch, string type, ArrayList newarray, DataSet ds1, string setvalue)
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                int count = newarray.Count;
                string collegename = Convert.ToString(ddlcollege.SelectedItem.Text);
                if (count > 0)
                {
                    count = count * 2;
                    fpspread.Sheets[0].AutoPostBack = true;
                    fpspread.CommandBar.Visible = false;
                    fpspread.Sheets[0].RowHeader.Visible = false;
                    fpspread.Sheets[0].ColumnCount = count + 4;
                    fpspread.Sheets[0].RowCount = 0;
                    fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "DEPARTMENT OF COLLEGEIATE EDUCATION, CHENNAI - 600 006";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Student Admission Strength 2017 - 18  " + setvalue + " Including all subjects";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Govt / Aided / Constituent / Self Finance Colleges";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "email Id: dcemsection@gmail.com";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Name of the RJD";
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "";
                    fpspread.Sheets[0].RowCount += 2;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, 0, 2, 1);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, 0].Text = "College Name";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;
                    int column = 0;
                    if (newarray.Count > 0)
                    {
                        for (int i = 0; i < newarray.Count; i++)
                        {
                            column += 2;
                            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, column - 1, 1, 2);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].Text = Convert.ToString(newarray[i]);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].VerticalAlign = VerticalAlign.Middle;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].Text = "Boys";
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].VerticalAlign = VerticalAlign.Middle;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = "Girls";
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        }

                        column += 2;
                        fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, column - 1, 1, 2);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].Text = "Total";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].VerticalAlign = VerticalAlign.Middle;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].Text = "Boys";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].VerticalAlign = VerticalAlign.Middle;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = "Girls";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        column++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].Text = "Grand Total";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].VerticalAlign = VerticalAlign.Middle;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = "Boys + Girls";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        column = 0;
                        fpspread.Sheets[0].RowCount++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(collegename);
                        int boyscount = 0;
                        int girlscount = 0;
                        if (newarray.Count > 0)
                        {
                            for (int i = 0; i < newarray.Count; i++)
                            {

                                string value = Convert.ToString(newarray[i]);
                                column++;
                                ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and Edu_Level='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='0'";
                                dv = ds.Tables[0].DefaultView;
                                boyscount = boyscount + dv.Count;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(dv.Count);
                                ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and Edu_Level='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='1'";
                                dv = ds.Tables[0].DefaultView;
                                girlscount = girlscount + dv.Count;
                                column++;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(dv.Count);
                            }
                        }
                        column++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(boyscount);
                        column++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(girlscount);
                        column++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Convert.ToInt32(boyscount + girlscount));

                        fpspread.Visible = true;
                        rptprint.Visible = true;
                        Errorlable.Visible = false;
                    }
                    if (fpspread.Sheets[0].ColumnCount > 0)
                    {
                        for (int col = 0; col < fpspread.Sheets[0].ColumnCount; col++)
                        {
                            fpspread.Sheets[0].Columns[col].Font.Name = "Book Antiqua";
                            fpspread.Sheets[0].Columns[col].Font.Size = FontUnit.Medium;
                            fpspread.Sheets[0].Columns[col].Font.Bold = true;
                        }
                    }
                }
                else
                {
                    fpspread.Visible = false;
                    rptprint.Visible = false;
                    Errorlable.Visible = true;
                    Errorlable.Text = "No Records Found";
                }
            }
            else
            {
                fpspread.Visible = false;
                rptprint.Visible = false;
                Errorlable.Visible = true;
                Errorlable.Text = "No Records Found";
            }
        }
        catch
        {

        }
    }


    public void form2(string college, string batch, string type, ArrayList newarray, DataSet ds1, string setvalue)
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                int count = ds.Tables[2].Rows.Count;
                string collegename = Convert.ToString(ddlcollege.SelectedItem.Text);
                if (count > 0)
                {
                    count = count * 2;
                    fpspread.Sheets[0].AutoPostBack = true;
                    fpspread.CommandBar.Visible = false;
                    fpspread.Sheets[0].RowHeader.Visible = false;
                    fpspread.Sheets[0].ColumnCount = count + 4;
                    fpspread.Sheets[0].RowCount = 0;
                    fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "DEPARTMENT OF COLLEGEIATE EDUCATION, CHENNAI - 600 006";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Admission Student Strength 2017 - 18  (Community Wise)  " + setvalue + " Including all subjects";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Govt / Aided / Constituent / Self Finance Colleges";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "email Id: dcemsection@gmail.com";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Name of the RJD";
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "";
                    fpspread.Sheets[0].RowCount += 2;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, 0, 2, 1);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, 0].Text = "College Name";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;
                    int column = 0;
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            column += 2;
                            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, column - 1, 1, 2);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].Text = Convert.ToString(ds.Tables[2].Rows[i]["TextVal"]);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].VerticalAlign = VerticalAlign.Middle;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].Text = "Boys";
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].VerticalAlign = VerticalAlign.Middle;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = "Girls";
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        }

                        column += 2;
                        fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, column - 1, 1, 2);
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].Text = "Total";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].VerticalAlign = VerticalAlign.Middle;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].Text = "Boys";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].VerticalAlign = VerticalAlign.Middle;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = "Girls";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        column++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].Text = "Grand Total";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].VerticalAlign = VerticalAlign.Middle;

                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = "Boys + Girls";
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        column = 0;
                        fpspread.Sheets[0].RowCount++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(collegename);
                        int boyscount = 0;
                        int girlscount = 0;
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                            {

                                string value = Convert.ToString(ds.Tables[2].Rows[i]["TextCode"]);
                                column++;
                                if (ddlreport.SelectedIndex == 1)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and community='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='0'";
                                    dv = ds.Tables[0].DefaultView;
                                }
                                else
                                {
                                    ds.Tables[0].DefaultView.RowFilter = " batch_year=" + batch + " and  community='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='0'";
                                    dv = ds.Tables[0].DefaultView;
                                }
                                boyscount = boyscount + dv.Count;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(dv.Count);
                                if (ddlreport.SelectedIndex == 1)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and community='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='1'";
                                    dv = ds.Tables[0].DefaultView;
                                }
                                else
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and community='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='1'";
                                    dv = ds.Tables[0].DefaultView;
                                }
                                girlscount = girlscount + dv.Count;
                                column++;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(dv.Count);
                            }
                        }
                        column++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(boyscount);
                        column++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(girlscount);
                        column++;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Convert.ToInt32(boyscount + girlscount));

                        fpspread.Visible = true;
                        rptprint.Visible = true;
                        Errorlable.Visible = false;
                    }
                    if (fpspread.Sheets[0].ColumnCount > 0)
                    {
                        for (int col = 0; col < fpspread.Sheets[0].ColumnCount; col++)
                        {
                            fpspread.Sheets[0].Columns[col].Font.Name = "Book Antiqua";
                            fpspread.Sheets[0].Columns[col].Font.Size = FontUnit.Medium;
                            fpspread.Sheets[0].Columns[col].Font.Bold = true;
                        }
                    }
                }
                else
                {
                    fpspread.Visible = false;
                    rptprint.Visible = false;
                    Errorlable.Visible = true;
                    Errorlable.Text = "No Records Found";
                }
            }
            else
            {
                fpspread.Visible = false;
                rptprint.Visible = false;
                Errorlable.Visible = true;
                Errorlable.Text = "No Records Found";
            }
        }
        catch
        {

        }
    }

    public void form1(string college, string batch, string type, ArrayList newarray, DataSet ds1, string setvalue)
    {
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                int count = newarray.Count;
                string collegename = Convert.ToString(ddlcollege.SelectedItem.Text);
                if (count > 0)
                {
                    count = count * 3;
                    fpspread.Sheets[0].AutoPostBack = true;
                    fpspread.CommandBar.Visible = false;
                    fpspread.Sheets[0].RowHeader.Visible = false;
                    fpspread.Sheets[0].ColumnCount = count + 5;
                    fpspread.Sheets[0].RowCount = 0;
                    fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                    fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "DEPARTMENT OF COLLEGEIATE EDUCATION, CHENNAI - 600 006";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Student Admission Strength " + ddlbatch.SelectedItem.Text + " - " + (Convert.ToInt32(ddlbatch.SelectedItem.Text) + 1) + "  " + setvalue + " Including all subjects";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Govt / Aided / Constituent / Self Finance Colleges";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "email Id: dcemsection@gmail.com";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Name of the RJD";
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "";
                    fpspread.Sheets[0].RowCount += 2;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, 0, 2, 1);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, 0].Text = "College Name";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;
                    int column = 0;
                    if (newarray.Count > 0)
                    {
                        for (int i = 0; i < newarray.Count; i++)
                        {
                            column++;
                            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, column, 2, 1);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].Text = "Sanctioned Strength";
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].VerticalAlign = VerticalAlign.Middle;
                            column += 2;
                            fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, column - 1, 1, 2);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].Text = "Admitted  " + newarray[i] + "";
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].Text = "Boys";
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = "Girls";
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }
                    column++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, column, 2, 1);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].Text = "Sanctioned Strength";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].VerticalAlign = VerticalAlign.Middle;
                    column += 2;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, column - 1, 1, 2);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].Text = "Total";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column - 1].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].Text = "Male";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column - 1].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = "Female";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    column++;
                    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 2, column, 2, 1);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].Text = "Grand Total";
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 2, column].VerticalAlign = VerticalAlign.Middle;
                    column = 0;
                    fpspread.Sheets[0].RowCount++;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(collegename);
                    int totalstrengthcount = 0;
                    int boyscount = 0;
                    int girlscount = 0;
                    if (newarray.Count > 0)
                    {
                        for (int i = 0; i < newarray.Count; i++)
                        {

                            string value = Convert.ToString(newarray[i]);
                            column++;
                            ds.Tables[1].DefaultView.RowFilter = "Edu_Level='" + value + "' and type='" + type + "'";
                            dv1 = ds.Tables[1].DefaultView;
                            totalstrengthcount = totalstrengthcount + Convert.ToInt32(dv1[0][0]);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(dv1[0][0]);
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                            column++;
                            ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and Edu_Level='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='0'";
                            dv = ds.Tables[0].DefaultView;
                            boyscount = boyscount + dv.Count;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(dv.Count);
                            ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and Edu_Level='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='1'";
                            dv = ds.Tables[0].DefaultView;
                            girlscount = girlscount + dv.Count;
                            column++;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                            fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(dv.Count);
                        }
                    }
                    column++;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(totalstrengthcount);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                    column++;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(boyscount);
                    column++;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(girlscount);
                    column++;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, column].Text = Convert.ToString(Convert.ToInt32(boyscount + girlscount));

                    fpspread.Visible = true;
                    rptprint.Visible = true;
                    Errorlable.Visible = false;
                    if (fpspread.Sheets[0].ColumnCount > 0)
                    {
                        for (int col = 0; col < fpspread.Sheets[0].ColumnCount; col++)
                        {
                            fpspread.Sheets[0].Columns[col].Font.Name = "Book Antiqua";
                            fpspread.Sheets[0].Columns[col].Font.Size = FontUnit.Medium;
                            fpspread.Sheets[0].Columns[col].Font.Bold = true;
                        }
                    }
                }
                else
                {
                    fpspread.Visible = false;
                    rptprint.Visible = false;
                    Errorlable.Visible = true;
                    Errorlable.Text = "No Records Found";
                }
            }
            else
            {
                fpspread.Visible = false;
                rptprint.Visible = false;
                Errorlable.Visible = true;
                Errorlable.Text = "No Records Found";
            }
        }
        catch
        {

        }
    }



    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {

            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                }
                txtbatch.Text = "Edu Level(" + (chklsbatch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                }
                txtbatch.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txtbatch.Text = "--Select--";
            chkbatch.Checked = false;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbatch.Text = "Edu Level(" + commcount.ToString() + ")";
                if (commcount == chklsbatch.Items.Count)
                {
                    chkbatch.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        if (ddlreport.SelectedIndex == 4)
        {
            string pagename = "Admission_Report.aspx";
            string student = "Form V $ Stream : " + ddltype.SelectedItem.Text + "";
            Printcontrol.loadspreaddetails(fpspread, pagename, student);
            Printcontrol.Visible = true;
        }
        else
        {
            print();
        }
    }

    public void print()
    {
        try
        {
            DataSet dsnew = (DataSet)Session["data"];
            string college = Convert.ToString(ddlcollege.SelectedItem.Value);
            string type = Convert.ToString(ddltype.SelectedItem.Text);
            string batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            ArrayList edulevelarray = new ArrayList();
            string mainvalue = "";
            string setvalue = "";
            if (txtbatch.Enabled == true)
            {
                if (chklsbatch.Items.Count > 0)
                {
                    for (int i = 0; i < chklsbatch.Items.Count; i++)
                    {
                        if (chklsbatch.Items[i].Selected == true)
                        {
                            edulevelarray.Add(chklsbatch.Items[i].Text);
                            if (mainvalue == "")
                            {
                                mainvalue = chklsbatch.Items[i].Text;
                                setvalue = chklsbatch.Items[i].Text + "" + "(I year)";
                            }
                            else
                            {
                                mainvalue = mainvalue + "'" + "," + "'" + chklsbatch.Items[i].Text;
                                setvalue = setvalue + "," + chklsbatch.Items[i].Text + "" + "(I year)";
                            }
                        }
                    }
                }
            }
            if (ddlreport.SelectedIndex == 0)
            {
                pdf1(college, batch, type, edulevelarray, dsnew, setvalue);
            }
            else if (ddlreport.SelectedIndex == 1)
            {
                pdf2(college, batch, type, edulevelarray, dsnew, setvalue);
            }
            else if (ddlreport.SelectedIndex == 2)
            {
                pdf3(college, batch, type, edulevelarray, dsnew, setvalue);
            }
            else if (ddlreport.SelectedIndex == 3)
            {
                pdf2(college, batch, type, edulevelarray, dsnew, setvalue);
            }
        }
        catch
        {

        }
    }

    public void pdf3(string college, string batch, string type, ArrayList newarray, DataSet ds, string setvalue)
    {
        try
        {
            Gios.Pdf.PdfDocument mydoc;
            Font Fontbold = new Font("Book Antiqua", 18, FontStyle.Regular);
            Font fbold = new Font("Book Antiqua", 18, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 8, FontStyle.Regular);
            Font fontname = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font fontmedium = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font fontmediumb = new Font("Book Antiqua", 8, FontStyle.Bold);
            mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage;
            //  Gios.Pdf.PdfTable table1 = myprovdoc.NewTable(Fontsmall, prov_cnt + 8, 6, 1);
            Gios.Pdf.PdfTable table;
            Gios.Pdf.PdfTablePage myprov_pdfpage1;
            int count = newarray.Count;
            string collegename = Convert.ToString(ddlcollege.SelectedItem.Text);
            if (count > 0)
            {
                count = count * 2;
                count = count + 4;
                table = mydoc.NewTable(Fontsmall, 9, count, 5);
                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                table.VisibleHeaders = false;
                table.Columns[0].SetWidth(15);
                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 0).SetFont(fontname);
                table.Cell(0, 0).SetContent("DEPARTMENT OF COLLEGEIATE EDUCATION, CHENNAI - 600 006");
                foreach (PdfCell pr in table.CellRange(0, 0, 0, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(1, 0).SetFont(fontname);
                table.Cell(1, 0).SetContent("Student Admission Strength 2017 - 18  " + setvalue + " Including all subjects");
                foreach (PdfCell pr in table.CellRange(1, 0, 1, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(2, 0).SetFont(fontname);
                table.Cell(2, 0).SetContent("Govt / Aided / Constituent / Self Finance Colleges");
                foreach (PdfCell pr in table.CellRange(2, 0, 2, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                table.Cell(3, 0).SetFont(fontname);
                table.Cell(3, 0).SetContent("email ID: ");
                foreach (PdfCell pr in table.CellRange(3, 0, 3, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(4, 0).SetFont(fontname);
                table.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                table.Cell(4, 0).SetContent("Name of the RJD");
                foreach (PdfCell pr in table.CellRange(4, 0, 4, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                foreach (PdfCell pr in table.CellRange(5, 0, 5, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(6, 0).SetContent("College Name");

                foreach (PdfCell pr in table.CellRange(6, 0, 6, 0).Cells)
                {
                    pr.RowSpan = 2;
                }
                int column = 0;
                if (newarray.Count > 0)
                {
                    for (int i = 0; i < newarray.Count; i++)
                    {
                        //column++;
                        //table.Cell(6, column).SetContent("Sanctioned Strength");
                        //foreach (PdfCell pr in table.CellRange(6, column, 6, column).Cells)
                        //{
                        //    pr.RowSpan = 2;
                        //}
                        column += 2;
                        table.Cell(6, column - 1).SetContent(Convert.ToString(newarray[i]));
                        foreach (PdfCell pr in table.CellRange(6, column - 1, 6, column - 1).Cells)
                        {
                            pr.ColSpan = 2;
                        }
                        table.Cell(7, column - 1).SetContent("Boys");
                        table.Cell(7, column).SetContent("Girls");
                    }
                    //column++;
                    //table.Cell(6, column).SetContent("Sanctioned Strength");
                    //foreach (PdfCell pr in table.CellRange(6, column, 6, column).Cells)
                    //{
                    //    pr.RowSpan = 2;
                    //}
                    column += 2;
                    table.Cell(6, column - 1).SetContent("Total");
                    foreach (PdfCell pr in table.CellRange(6, column - 1, 6, column - 1).Cells)
                    {
                        pr.ColSpan = 2;
                    }
                    table.Cell(7, column - 1).SetContent("Boys");
                    table.Cell(7, column).SetContent("Girls");
                    column++;
                    table.Cell(6, column).SetContent("Grand Total");
                    table.Cell(7, column).SetContent("Boys + Girls");
                    //foreach (PdfCell pr in table.CellRange(6, column, 6, column).Cells)
                    //{
                    //    pr.RowSpan = 2;
                    //}
                    column = 0;
                    table.Cell(8, 0).SetContent(collegename);
                    //int totalstrengthcount = 0;
                    int boyscount = 0;
                    int girlscount = 0;
                    if (newarray.Count > 0)
                    {
                        for (int i = 0; i < newarray.Count; i++)
                        {

                            string value = Convert.ToString(newarray[i]);
                            //column++;
                            //ds.Tables[1].DefaultView.RowFilter = "Edu_Level='" + value + "' and type='" + type + "'";
                            //dv1 = ds.Tables[1].DefaultView;
                            //totalstrengthcount = totalstrengthcount + Convert.ToInt32(dv1[0][0]);
                            //table.Cell(8, column).SetContent(Convert.ToString(dv1[0][0]));
                            //table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                            column++;
                            ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and Edu_Level='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='0'";
                            dv = ds.Tables[0].DefaultView;
                            boyscount = boyscount + dv.Count;
                            table.Cell(8, column).SetContent(Convert.ToString(dv.Count));
                            table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                            ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and Edu_Level='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='1'";
                            dv = ds.Tables[0].DefaultView;
                            girlscount = girlscount + dv.Count;
                            column++;
                            table.Cell(8, column).SetContent(Convert.ToString(dv.Count));
                            table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                    }
                    //column++;
                    //table.Cell(8, column).SetContent(Convert.ToString(totalstrengthcount));
                    //table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                    column++;
                    table.Cell(8, column).SetContent(Convert.ToString(boyscount));
                    table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                    column++;
                    table.Cell(8, column).SetContent(Convert.ToString(girlscount));
                    table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                    column++;
                    table.Cell(8, column).SetContent(Convert.ToString(Convert.ToInt32(boyscount + girlscount)));
                    table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                }
                mypdfpage = mydoc.NewPage();
                PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 30, 100, 800, 30), System.Drawing.ContentAlignment.MiddleCenter, ddlreport.SelectedItem.Text);
                mypdfpage.Add(ptc);
                myprov_pdfpage1 = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 30, 170, 800, 500));
                mypdfpage.Add(myprov_pdfpage1);
                mypdfpage.SaveToDocument();
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Test.pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch
        {

        }
    }

    public void pdf2(string college, string batch, string type, ArrayList newarray, DataSet ds, string setvalue)
    {
        try
        {
            Gios.Pdf.PdfDocument mydoc;
            Font Fontbold = new Font("Book Antiqua", 18, FontStyle.Regular);
            Font fbold = new Font("Book Antiqua", 18, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 8, FontStyle.Regular);
            Font fontname = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font fontmedium = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font fontmediumb = new Font("Book Antiqua", 8, FontStyle.Bold);
            mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage;
            //  Gios.Pdf.PdfTable table1 = myprovdoc.NewTable(Fontsmall, prov_cnt + 8, 6, 1);
            Gios.Pdf.PdfTable table;
            Gios.Pdf.PdfTablePage myprov_pdfpage1;
            int count = ds.Tables[2].Rows.Count;
            string collegename = Convert.ToString(ddlcollege.SelectedItem.Text);
            if (count > 0)
            {
                count = count * 2;
                count = count + 4;
                table = mydoc.NewTable(Fontsmall, 9, count, 5);
                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                table.VisibleHeaders = false;
                table.Columns[0].SetWidth(15);
                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 0).SetFont(fontname);
                table.Cell(0, 0).SetContent("DEPARTMENT OF COLLEGEIATE EDUCATION, CHENNAI - 600 006");
                foreach (PdfCell pr in table.CellRange(0, 0, 0, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(1, 0).SetFont(fontname);
                table.Cell(1, 0).SetContent("Admission Student Strength 2017 - 18  " + setvalue + " (Community Wise) Including all subjects");
                foreach (PdfCell pr in table.CellRange(1, 0, 1, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(2, 0).SetFont(fontname);
                table.Cell(2, 0).SetContent("Govt / Aided / Constituent / Self Finance Colleges");
                foreach (PdfCell pr in table.CellRange(2, 0, 2, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                table.Cell(3, 0).SetFont(fontname);
                table.Cell(3, 0).SetContent("email ID: ");
                foreach (PdfCell pr in table.CellRange(3, 0, 3, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(4, 0).SetFont(fontname);
                table.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                table.Cell(4, 0).SetContent("Name of the RJD");
                foreach (PdfCell pr in table.CellRange(4, 0, 4, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                foreach (PdfCell pr in table.CellRange(5, 0, 5, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(6, 0).SetContent("College Name");

                foreach (PdfCell pr in table.CellRange(6, 0, 6, 0).Cells)
                {
                    pr.RowSpan = 2;
                }
                int column = 0;
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        //column++;
                        //table.Cell(6, column).SetContent("Sanctioned Strength");
                        //foreach (PdfCell pr in table.CellRange(6, column, 6, column).Cells)
                        //{
                        //    pr.RowSpan = 2;
                        //}
                        column += 2;
                        table.Cell(6, column - 1).SetContent(Convert.ToString(ds.Tables[2].Rows[i]["TextVal"]));
                        foreach (PdfCell pr in table.CellRange(6, column - 1, 6, column - 1).Cells)
                        {
                            pr.ColSpan = 2;
                        }
                        table.Cell(7, column - 1).SetContent("Boys");
                        table.Cell(7, column).SetContent("Girls");
                    }
                    //column++;
                    //table.Cell(6, column).SetContent("Sanctioned Strength");
                    //foreach (PdfCell pr in table.CellRange(6, column, 6, column).Cells)
                    //{
                    //    pr.RowSpan = 2;
                    //}
                    column += 2;
                    table.Cell(6, column - 1).SetContent("Total");
                    foreach (PdfCell pr in table.CellRange(6, column - 1, 6, column - 1).Cells)
                    {
                        pr.ColSpan = 2;
                    }
                    table.Cell(7, column - 1).SetContent("Boys");
                    table.Cell(7, column).SetContent("Girls");
                    column++;
                    table.Cell(6, column).SetContent("Grand Total");
                    table.Cell(7, column).SetContent("Boys + Girls");
                    //foreach (PdfCell pr in table.CellRange(6, column, 6, column).Cells)
                    //{
                    //    pr.RowSpan = 2;
                    //}
                    column = 0;
                    table.Cell(8, 0).SetContent(collegename);
                    //int totalstrengthcount = 0;
                    int boyscount = 0;
                    int girlscount = 0;
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {

                            string value = Convert.ToString(Convert.ToString(ds.Tables[2].Rows[i]["Textcode"]));
                            //column++;
                            //ds.Tables[1].DefaultView.RowFilter = "Edu_Level='" + value + "' and type='" + type + "'";
                            //dv1 = ds.Tables[1].DefaultView;
                            //totalstrengthcount = totalstrengthcount + Convert.ToInt32(dv1[0][0]);
                            //table.Cell(8, column).SetContent(Convert.ToString(dv1[0][0]));
                            //table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                            column++;
                            if (ddlreport.SelectedIndex == 1)
                            {
                                ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and community='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='0'";
                                dv = ds.Tables[0].DefaultView;
                            }
                            else
                            {
                                ds.Tables[0].DefaultView.RowFilter = "community='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='0'";
                                dv = ds.Tables[0].DefaultView;
                            }
                            boyscount = boyscount + dv.Count;
                            table.Cell(8, column).SetContent(Convert.ToString(dv.Count));
                            table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                            if (ddlreport.SelectedIndex == 1)
                            {
                                ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and community='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='1'";
                                dv = ds.Tables[0].DefaultView;
                            }
                            else
                            {
                                ds.Tables[0].DefaultView.RowFilter = "community='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='1'";
                                dv = ds.Tables[0].DefaultView;
                            }
                            girlscount = girlscount + dv.Count;
                            column++;
                            table.Cell(8, column).SetContent(Convert.ToString(dv.Count));
                            table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                    }
                    //column++;
                    //table.Cell(8, column).SetContent(Convert.ToString(totalstrengthcount));
                    //table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                    column++;
                    table.Cell(8, column).SetContent(Convert.ToString(boyscount));
                    table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                    column++;
                    table.Cell(8, column).SetContent(Convert.ToString(girlscount));
                    table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                    column++;
                    table.Cell(8, column).SetContent(Convert.ToString(Convert.ToInt32(boyscount + girlscount)));
                    table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                }
                mypdfpage = mydoc.NewPage();
                PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 30, 100, 800, 30), System.Drawing.ContentAlignment.MiddleCenter, ddlreport.SelectedItem.Text);
                mypdfpage.Add(ptc);
                myprov_pdfpage1 = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 30, 170, 800, 500));
                mypdfpage.Add(myprov_pdfpage1);
                mypdfpage.SaveToDocument();
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Test.pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);

            }
        }
        catch
        {

        }
    }

    public void pdf1(string college, string batch, string type, ArrayList newarray, DataSet ds, string setvalue)
    {
        try
        {
            Gios.Pdf.PdfDocument mydoc;
            Font Fontbold = new Font("Book Antiqua", 18, FontStyle.Regular);
            Font fbold = new Font("Book Antiqua", 18, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 8, FontStyle.Regular);
            Font fontname = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font fontmedium = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font fontmediumb = new Font("Book Antiqua", 8, FontStyle.Bold);
            mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            Gios.Pdf.PdfPage mypdfpage;
            //  Gios.Pdf.PdfTable table1 = myprovdoc.NewTable(Fontsmall, prov_cnt + 8, 6, 1);
            Gios.Pdf.PdfTable table;
            Gios.Pdf.PdfTablePage myprov_pdfpage1;
            int count = newarray.Count;
            string collegename = Convert.ToString(ddlcollege.SelectedItem.Text);
            if (count > 0)
            {
                count = count * 3;
                count = count + 5;
                table = mydoc.NewTable(Fontsmall, 9, count, 5);
                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                table.VisibleHeaders = false;
                table.Columns[0].SetWidth(15);
                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 0).SetFont(fontname);
                table.Cell(0, 0).SetContent("DEPARTMENT OF COLLEGEIATE EDUCATION, CHENNAI - 600 006");
                foreach (PdfCell pr in table.CellRange(0, 0, 0, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(1, 0).SetFont(fontname);
                table.Cell(1, 0).SetContent("Student Admission Strength 2017 - 18  " + setvalue + " Including all subjects");
                foreach (PdfCell pr in table.CellRange(1, 0, 1, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(2, 0).SetFont(fontname);
                table.Cell(2, 0).SetContent("Govt / Aided / Constituent / Self Finance Colleges");
                foreach (PdfCell pr in table.CellRange(2, 0, 2, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                table.Cell(3, 0).SetFont(fontname);
                table.Cell(3, 0).SetContent("email ID: ");
                foreach (PdfCell pr in table.CellRange(3, 0, 3, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(4, 0).SetFont(fontname);
                table.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                table.Cell(4, 0).SetContent("Name of the RJD");
                foreach (PdfCell pr in table.CellRange(4, 0, 4, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                foreach (PdfCell pr in table.CellRange(5, 0, 5, 0).Cells)
                {
                    pr.ColSpan = count;
                }
                table.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(6, 0).SetContent("College Name");

                foreach (PdfCell pr in table.CellRange(6, 0, 6, 0).Cells)
                {
                    pr.RowSpan = 2;
                }
                int column = 0;
                if (newarray.Count > 0)
                {
                    for (int i = 0; i < newarray.Count; i++)
                    {
                        column++;
                        table.Cell(6, column).SetContent("Sanctioned Strength");
                        foreach (PdfCell pr in table.CellRange(6, column, 6, column).Cells)
                        {
                            pr.RowSpan = 2;
                        }
                        column += 2;
                        table.Cell(6, column - 1).SetContent("Admitted  " + newarray[i] + "");
                        foreach (PdfCell pr in table.CellRange(6, column - 1, 6, column - 1).Cells)
                        {
                            pr.ColSpan = 2;
                        }
                        table.Cell(7, column - 1).SetContent("Boys");
                        table.Cell(7, column).SetContent("Girls");
                    }
                    column++;
                    table.Cell(6, column).SetContent("Sanctioned Strength");
                    foreach (PdfCell pr in table.CellRange(6, column, 6, column).Cells)
                    {
                        pr.RowSpan = 2;
                    }
                    column += 2;
                    table.Cell(6, column - 1).SetContent("Total");
                    foreach (PdfCell pr in table.CellRange(6, column - 1, 6, column - 1).Cells)
                    {
                        pr.ColSpan = 2;
                    }
                    table.Cell(7, column - 1).SetContent("Boys");
                    table.Cell(7, column).SetContent("Girls");
                    column++;
                    table.Cell(6, column).SetContent("Grand Total");
                    foreach (PdfCell pr in table.CellRange(6, column, 6, column).Cells)
                    {
                        pr.RowSpan = 2;
                    }
                    column = 0;
                    table.Cell(8, 0).SetContent(collegename);
                    int totalstrengthcount = 0;
                    int boyscount = 0;
                    int girlscount = 0;
                    if (newarray.Count > 0)
                    {
                        for (int i = 0; i < newarray.Count; i++)
                        {

                            string value = Convert.ToString(newarray[i]);
                            column++;
                            ds.Tables[1].DefaultView.RowFilter = "Edu_Level='" + value + "' and type='" + type + "'";
                            dv1 = ds.Tables[1].DefaultView;
                            totalstrengthcount = totalstrengthcount + Convert.ToInt32(dv1[0][0]);
                            table.Cell(8, column).SetContent(Convert.ToString(dv1[0][0]));
                            table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                            column++;
                            ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and Edu_Level='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='0'";
                            dv = ds.Tables[0].DefaultView;
                            boyscount = boyscount + dv.Count;
                            table.Cell(8, column).SetContent(Convert.ToString(dv.Count));
                            table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                            ds.Tables[0].DefaultView.RowFilter = "batch_year=" + batch + " and Edu_Level='" + value + "' and type='" + type + "' and college_code='" + college + "' and sex='1'";
                            dv = ds.Tables[0].DefaultView;
                            girlscount = girlscount + dv.Count;
                            column++;
                            table.Cell(8, column).SetContent(Convert.ToString(dv.Count));
                            table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                        }
                    }
                    column++;
                    table.Cell(8, column).SetContent(Convert.ToString(totalstrengthcount));
                    table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                    column++;
                    table.Cell(8, column).SetContent(Convert.ToString(boyscount));
                    table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                    column++;
                    table.Cell(8, column).SetContent(Convert.ToString(girlscount));
                    table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                    column++;
                    table.Cell(8, column).SetContent(Convert.ToString(Convert.ToInt32(boyscount + girlscount)));
                    table.Cell(8, column).SetContentAlignment(ContentAlignment.MiddleCenter);
                }
                mypdfpage = mydoc.NewPage();
                PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 30, 100, 800, 30), System.Drawing.ContentAlignment.MiddleCenter, ddlreport.SelectedItem.Text);
                mypdfpage.Add(ptc);
                myprov_pdfpage1 = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 30, 170, 800, 500));
                mypdfpage.Add(myprov_pdfpage1);
                mypdfpage.SaveToDocument();
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Test.pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);

            }
        }
        catch
        {

        }
    }

}