using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Gios.Pdf;
using System.Drawing;
using System.Collections;
using System.Configuration;


public partial class reval : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    ArrayList addvalue = new ArrayList();
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
            if (!IsPostBack)
            {
                txtdop.Attributes.Add("ReadOnly", "ReadOnly");
                string dt1 = DateTime.Today.ToShortDateString();
                string[] dsplit = dt1.Split(new Char[] { '/' });
                string dateconcat = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                txtdop.Text = dateconcat.ToString();
                MonthandYear();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void lnk_logout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);

        }
        catch
        {
        }
    }
    public void MonthandYear()
    {
        try
        {
            exammnth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            exammnth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            exammnth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            exammnth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            exammnth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            exammnth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
            exammnth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            exammnth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            exammnth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            exammnth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            exammnth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            exammnth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            exammnth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));


            int year;
            year = Convert.ToInt16(DateTime.Today.Year);
            ddlyear.Items.Clear();
            for (int l = 0; l <= 7; l++)
            {

                ddlyear.Items.Add(Convert.ToString(year - l));

            }
            ddlyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select-- ", "0"));
        }
        catch
        {

        }
    }

    protected void exammnth_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gridview1.Visible = false;
            gridviewrow.Visible = false;
            // Panel21.Visible = false;
            butgen.Visible = false;
            ddltyp.Enabled = true;
            visiblefalse();

        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlyear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gridview1.Visible = false;
            gridviewrow.Visible = false;
            butgen.Visible = false;
            ddltyp.Enabled = true;
            visiblefalse();
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddltyp_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gridview1.Visible = false;
            gridviewrow.Visible = false;
            butgen.Visible = false;
            ddltyp.Enabled = true;
            visiblefalse();
        }
        catch (Exception ex)
        {
        }
    }
    protected void butgo_Click(object sender, EventArgs e)
    {
        try
        {

            DataRow dr = null;
            DataSet dsgrid = new DataSet();


            dt.Columns.Add("Degree", typeof(string));
            dt.Columns.Add("Branch", typeof(string));
            dt.Columns.Add("Current_Semester", typeof(string));
            dt.Columns.Add("total", typeof(string));
            dt.Columns.Add("department", typeof(string));
            dt.Columns.Add("Course_Name", typeof(string));
            dt.Columns.Add("degree_code", typeof(string));
            dt.Columns.Add("batch_year", typeof(string));


            int ex_month = Convert.ToInt32(exammnth.SelectedItem.Value);
            int year = Convert.ToInt32(ddlyear.SelectedItem.Value);
            string query = "";
            if (ddltyp.SelectedItem.Value == "1")
            {
                query = "select COUNT(ea.roll_no)as total,d.degree_code,r.batch_year,c.Course_Name ,dt.Dept_Name as department ,r.Current_Semester from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code  and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month =" + ex_month + " and Exam_year =" + year + " and Exam_type=5  group by r.batch_year ,d.degree_code ,Course_Name,Dept_Name,r.Current_Semester";
            }
            else if (ddltyp.SelectedItem.Value == "2")
            {
                query = "select COUNT(ea.roll_no)as total,d.degree_code,r.batch_year,c.Course_Name ,dt.Dept_Name as department ,r.Current_Semester from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code  and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month =" + ex_month + " and Exam_year =" + year + " and Exam_type=3  group by r.batch_year ,d.degree_code ,Course_Name,Dept_Name,r.Current_Semester";
            }
            else if (ddltyp.SelectedItem.Value == "3")
            {
                query = "select COUNT(ea.roll_no)as total,d.degree_code,r.batch_year,c.Course_Name ,dt.Dept_Name as department ,r.Current_Semester from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code  and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month =" + ex_month + " and Exam_year =" + year + " and Exam_type=2  group by r.batch_year ,d.degree_code ,Course_Name,Dept_Name,r.Current_Semester";
            }
            else if (ddltyp.SelectedItem.Value == "4")
            {
                query = "select COUNT(ea.roll_no)as total,d.degree_code,r.batch_year,c.Course_Name ,dt.Dept_Name as department ,r.Current_Semester from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code  and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month =" + ex_month + " and Exam_year =" + year + " and Exam_type=4  group by r.batch_year ,d.degree_code ,Course_Name,Dept_Name,r.Current_Semester";

            }
            //string query = "select COUNT(ea.roll_no)as total,d.degree_code,r.batch_year,c.Course_Name ,dt.Dept_Name as department ,r.Current_Semester from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code  and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month =" + ex_month + " and Exam_year =" + year + " group by r.batch_year ,d.degree_code ,Course_Name,Dept_Name,r.Current_Semester";
            dsgrid.Clear();
            dsgrid = da.select_method_wo_parameter(query, "Text");
            if (dsgrid.Tables[0].Rows.Count > 0)
            {
                // ddltyp.Enabled = false;
                for (int row = 0; row < dsgrid.Tables[0].Rows.Count; row++)
                {
                    dr = dt.NewRow();
                    dr[0] = Convert.ToString(dsgrid.Tables[0].Rows[row]["degree_code"]);
                    dr[1] = Convert.ToString(dsgrid.Tables[0].Rows[row]["batch_year"]);
                    dr[2] = Convert.ToString(dsgrid.Tables[0].Rows[row]["Current_Semester"]);
                    dr[3] = Convert.ToString(dsgrid.Tables[0].Rows[row]["total"]);
                    dr[4] = Convert.ToString(dsgrid.Tables[0].Rows[row]["department"]);
                    dr[5] = Convert.ToString(dsgrid.Tables[0].Rows[row]["Course_Name"]);

                    string sem = Convert.ToString(dsgrid.Tables[0].Rows[row]["Current_Semester"]);
                    dt.Rows.Add(dr);

                }

            }
            else
            {

                gridview1.Visible = false;
                gridviewrow.Visible = false;
                butgen.Visible = false;

                // ddltyp.Visible = false;


            }

            if (dt.Rows.Count > 0)
            {
                gridview1.Visible = true;
                butgen.Visible = false;

                gridviewrow.Visible = false;
                gridview1.DataSource = dt;
                gridview1.DataBind();
            }
            else
            {

                gridview1.Visible = false;
                gridviewrow.Visible = false;
                butgen.Visible = false;

                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);


            }
            for (int j = 0; j < gridview1.Rows.Count; j++)
            {
                gridview1.Rows[j].Cells[3].HorizontalAlign = HorizontalAlign.Left;

            }
            for (int j = 0; j < gridviewrow.Rows.Count; j++)
            {

                gridviewrow.Rows[j].Cells[1].HorizontalAlign = HorizontalAlign.Left;
                gridviewrow.Rows[j].Cells[2].HorizontalAlign = HorizontalAlign.Left;


            }


        }

        catch
        {
        }


    }
    public void visiblefalse()
    {
        gridviewrow.Visible = false;
        gridview1.Visible = false;
        gridviewrow.Visible = false;
        butgen.Visible = false;
    }
    protected void gridview_databoud(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gridview1, "Total$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gridview1, "Total$" + e.Row.RowIndex);
                e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gridview1, "Total$" + e.Row.RowIndex);
                e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gridview1, "Total$" + e.Row.RowIndex);
                e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gridview1, "Total$" + e.Row.RowIndex);
            }
        }
        catch
        {
        }
    }
    public void bindpdf(Gios.Pdf.PdfDocument mydoc, Font Fontbold, Font Fontbold1, Font Fontbold2, Font Fontbold3, Font Fontbold4, Font font)
    {
        try
        {
            DataSet ds2 = new DataSet();
            string collegenew1 = "";
            string address1 = "";
            string address2 = "";
            string acronym = "";
            bool flag = false;

            if (gridviewrow.Rows.Count > 0)
            {

                for (int con = 0; con < gridviewrow.Rows.Count; con++)
                {
                    if ((gridviewrow.Rows[con].FindControl("cbSelect") as CheckBox).Checked == true)
                    {
                        flag = true;
                        string Regno = ((gridviewrow.Rows[con].FindControl("lblreg") as Label).Text);
                        string studname = ((gridviewrow.Rows[con].FindControl("lblstu") as Label).Text);
                        string Rollno = ((gridviewrow.Rows[con].FindControl("lblroll") as Label).Text);
                        string dept = Convert.ToString(ViewState["dept"]);
                        string degree_name = "";
                        string branch_name = "";
                        string semester = "";
                        string subjectcode = "";
                        string subjecttit = "";
                        string total = "";
                        string internal1 = "";
                        string external = "";
                        string batch_year = "";
                        string degree_code = "";
                        string gradeflage = "";
                        string pincode = "";
                        string[] split = dept.Split('-');
                        string sk = txtdop.Text;
                        if (split.Length > 0)
                        {
                            degree_name = Convert.ToString(split[0]);
                            branch_name = Convert.ToString(split[1]);
                        }
                        string collegetitle = "select isnull(collname,'') as collname,isnull(acr,'') as acr,isnull(address1,'') as address1,isnull(address3,'') as address3,district ,pincode,university,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code='" + Session["collegecode"] + "'";
                        collegetitle = collegetitle + "  select Batch_Year,degree_code  from Registration where Roll_No='" + Rollno + "' and college_code =" + Session["collegecode"] + "";
                        ds2.Clear();
                        ds2 = da.select_method_wo_parameter(collegetitle, "Text");

                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            for (int count = 0; count < ds2.Tables[0].Rows.Count; count++)
                            {

                                collegenew1 = Convert.ToString(ds2.Tables[0].Rows[count]["collname"]);
                                address1 = Convert.ToString(ds2.Tables[0].Rows[count]["university"]);
                                address2 = Convert.ToString(ds2.Tables[0].Rows[count]["district"]);
                                acronym = Convert.ToString(ds2.Tables[0].Rows[count]["acr"]);
                                pincode = Convert.ToString(ds2.Tables[0].Rows[count]["pincode"]);
                            }

                        }

                        if (ds2.Tables[1].Rows.Count > 0)
                        {
                            batch_year = Convert.ToString(ds2.Tables[1].Rows[0]["Batch_Year"]);
                            degree_code = Convert.ToString(ds2.Tables[1].Rows[0]["degree_code"]);
                        }

                        if (batch_year.Trim() != "" && degree_code.Trim() != "")
                        {
                            string gradquery = da.GetFunction("select grade_flag  from grademaster where batch_year ='" + batch_year + "' and degree_code ='" + degree_code + "' and exam_month ='" + exammnth.SelectedItem.Value + "' and exam_year ='" + ddlyear.SelectedItem.Value + "'");
                            if (gradquery == "1")
                            {
                                gradeflage = "Mark";
                            }
                            else if (gradquery == "2")
                            {
                                gradeflage = "Grade";
                            }
                            else if (gradquery == "3")
                            {
                                string dd = da.GetFunction("select linkvalue from inssettings where linkname='corresponding grade' and college_code='" + Session["collegecode"].ToString() + "'");
                                if (dd == "1")
                                {
                                    gradeflage = "Grade";
                                }
                                else if (dd == "0")
                                {
                                    gradeflage = "Mark";
                                }
                            }

                        }

                        int y = 50;

                        string query1 = "";
                        string exam_code_l = "";
                        if (ddltyp.SelectedItem.Value == "1")
                        {
                            query1 = "select distinct ea.subject_no ,e.roll_no,s.subject_name,m.internal_mark,m.external_mark ,m.total ,s.subject_code,sy.semester,ea.fee,re_tot,re_val,subject_type,grade,improvement_fee,CONVERT(varchar(20), LastDate ,103) as LastDate   from Exam_Details ed,exam_appl_details ea,exam_application e,subject s,mark_entry m,syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no and e.Exam_type =5 and m.exam_code =ed.exam_code and e.exam_code =m.exam_code and s.subject_no =ea.subject_no and e.roll_no =m.roll_no and s.subject_no =m.subject_no and m.subject_no =ea.subject_no and su.syll_code =sy.syll_code and su.subType_no =s.subType_no  and  sy.syll_code =s.syll_code and e.roll_no ='" + Rollno + "'and ed.exam_month='" + exammnth.SelectedItem.Value + "' and ed.exam_year='" + ddlyear.SelectedItem.Value + "'";
                        }
                        else if (ddltyp.SelectedItem.Value == "2")
                        {
                            query1 = "select distinct ea.subject_no ,e.roll_no,s.subject_name,m.internal_mark,m.external_mark ,m.total ,s.subject_code,sy.semester,ea.fee,re_tot,re_val,subject_type,grade,improvement_fee,CONVERT(varchar(20), LastDate ,103) as LastDate   from Exam_Details ed,exam_appl_details ea,exam_application e,subject s,mark_entry m,syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no and e.Exam_type =3 and m.exam_code =ed.exam_code and e.exam_code =m.exam_code and s.subject_no =ea.subject_no and e.roll_no =m.roll_no and s.subject_no =m.subject_no and m.subject_no =ea.subject_no and su.syll_code =sy.syll_code and su.subType_no =s.subType_no  and  sy.syll_code =s.syll_code and e.roll_no ='" + Rollno + "'and ed.exam_month='" + exammnth.SelectedItem.Value + "' and ed.exam_year='" + ddlyear.SelectedItem.Value + "'";
                        }
                        else if (ddltyp.SelectedItem.Value == "3")
                        {
                            query1 = "select distinct ea.subject_no ,e.roll_no,s.subject_name,m.internal_mark,m.external_mark ,m.total ,s.subject_code,sy.semester,ea.fee,re_tot,re_val,subject_type,grade,improvement_fee,CONVERT(varchar(20), LastDate ,103) as LastDate   from Exam_Details ed,exam_appl_details ea,exam_application e,subject s,mark_entry m,syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no and e.Exam_type =2 and m.exam_code =ed.exam_code and e.exam_code =m.exam_code and s.subject_no =ea.subject_no and e.roll_no =m.roll_no and s.subject_no =m.subject_no and m.subject_no =ea.subject_no and su.syll_code =sy.syll_code and su.subType_no =s.subType_no  and  sy.syll_code =s.syll_code and e.roll_no ='" + Rollno + "'and ed.exam_month='" + exammnth.SelectedItem.Value + "' and ed.exam_year='" + ddlyear.SelectedItem.Value + "'";
                        }
                        else if (ddltyp.SelectedItem.Value == "4")
                        {
                            string examcodeprev = da.GetFunction("select exam_code from exam_details where degree_code ='" + degree_code + "' and batch_year ='" + batch_year + "' and Exam_Month='" + exammnth.SelectedItem.Value + "' and Exam_year='" + ddlyear.SelectedItem.Value + "'");
                            exam_code_l = examcodeprev;
                            examcodeprev = da.GetFunction("select MAX(exam_code) from exam_details where degree_code ='" + degree_code + "' and batch_year ='" + batch_year + "' and exam_code <> '" + examcodeprev + "'");
                            //query1 = "select ea.subject_no ,e.roll_no,s.subject_name,m.internal_mark,m.external_mark ,m.total ,s.subject_code,sy.semester,ea.fee,re_tot,arr_fee,re_val,subject_type,grade,improvement_fee,CONVERT(varchar(20), LastDate ,103) as LastDate   from Exam_Details ed,exam_appl_details ea,exam_application e,subject s,mark_entry m,syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no and e.Exam_type =2 and m.exam_code =ed.exam_code and e.exam_code =m.exam_code and s.subject_no =ea.subject_no and e.roll_no =m.roll_no and s.subject_no =m.subject_no and m.subject_no =ea.subject_no and su.syll_code =sy.syll_code and su.subType_no =s.subType_no  and  sy.syll_code =s.syll_code and e.roll_no ='" + Rollno + "'and ed.exam_month='" + exammnth.SelectedItem.Value + "' and ed.exam_year='" + ddlyear.SelectedItem.Value + "'";
                            //  query1 = "select ea.subject_no ,e.roll_no,s.subject_name,m.internal_mark,m.external_mark ,m.total ,s.subject_code,sy.semester,ea.fee,re_tot,arr_fee,re_val,subject_type,grade,improvement_fee,CONVERT(varchar(20), LastDate ,103) as LastDate   from Exam_Details ed,exam_appl_details ea,exam_application e,subject s,mark_entry m,syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no and e.Exam_type =2 and m.exam_code =ed.exam_code and e.exam_code =m.exam_code and s.subject_no =ea.subject_no and e.roll_no =m.roll_no and s.subject_no =m.subject_no and m.subject_no =ea.subject_no and su.syll_code =sy.syll_code and su.subType_no =s.subType_no  and  sy.syll_code =s.syll_code and e.roll_no ='" + Rollno + "' and m.exam_code='" + examcodeprev + "'";
                            //query1 = "select distinct  ea.subject_no ,e.roll_no,s.subject_name,m.internal_mark,m.external_mark ,m.total ,s.subject_code,sy.semester,ea.fee,re_tot,arr_fee,re_val,subject_type,grade,improvement_fee   from Exam_Details ed,exam_appl_details ea,exam_application e,subject s,mark_entry m,syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no   and m.exam_code =ed.exam_code  and e.exam_code =m.exam_code and s.subject_no =ea.subject_no and e.roll_no =m.roll_no and  s.subject_no =m.subject_no and m.subject_no =ea.subject_no and su.syll_code =sy.syll_code and  su.subType_no =s.subType_no  and  sy.syll_code =s.syll_code and e.roll_no ='" + Rollno + "' and m.exam_code='" + examcodeprev + "'  and  s.subject_no  in (     select ea.subject_no   from Exam_Details ed,exam_appl_details ea,exam_application e,subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no   and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no   and  sy.syll_code =s.syll_code and e.roll_no ='" + Rollno + "' and e.Exam_type=4 and ed.Exam_Month='" + exammnth.SelectedItem.Value + "' and ed.Exam_year='" + ddlyear.SelectedItem.Value + "')";
                            query1 = "select  distinct ea.roll_no,s.subject_name,m.internal_mark,m.external_mark ,m.total ,s.subject_code,sy.semester,re_tot,arr_fee,re_val,subject_type,grade,improvement_fee from Exam_Details ed,exam_application ea,subject s,mark_entry m,sub_sem ss,syllabus_master sy where ed.exam_code=ea.exam_code  and m.exam_code=ed.exam_code and s.subType_no=ss.subType_no and ss.syll_code=sy.syll_code and sy.Batch_Year=ed.batch_year and sy.degree_code=ed.degree_code and ea.roll_no=m.roll_no and s.subject_no=m.subject_no and m.subject_no=s.subject_no  and ea.roll_no ='" + Rollno + "' and m.exam_code='" + examcodeprev + "'  and  s.subject_no  in (     select ea.subject_no   from Exam_Details ed,exam_appl_details ea,exam_application e,subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no   and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no   and  sy.syll_code =s.syll_code and e.roll_no ='" + Rollno + "' and e.Exam_type=4 and ed.Exam_Month='" + exammnth.SelectedItem.Value + "' and ed.Exam_year='" + ddlyear.SelectedItem.Value + "')";

                        }
                        ds2.Clear();
                        ds2 = da.select_method_wo_parameter(query1, "Text");



                        if (ddltyp.SelectedItem.Value == "1")
                        {
                            string lastdate = "";
                            double totalamt = 0;
                            double totalamt1 = 0;
                            double pepartotal = 0;
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                {

                                    lastdate = Convert.ToString(ds2.Tables[0].Rows[j]["LastDate"]);
                                    string fee = Convert.ToString(ds2.Tables[0].Rows[j]["improvement_fee"]);
                                    string subjecttype = Convert.ToString(ds2.Tables[0].Rows[j]["subject_type"]);
                                    string internalmark = Convert.ToString(ds2.Tables[0].Rows[j]["internal_mark"]);
                                    string externalmark = Convert.ToString(ds2.Tables[0].Rows[j]["external_mark"]);

                                    if (fee.Trim() != "")
                                    {
                                        totalamt = totalamt + Convert.ToDouble(fee);
                                    }
                                    if (externalmark.Trim() != "")
                                    {
                                        totalamt1 = totalamt1 + Convert.ToDouble(externalmark);
                                    }
                                    if (subjecttype.ToString().ToUpper() == "THEORY")
                                    {
                                        string pa = Convert.ToString(ds2.Tables[0].Rows[j]["improvement_fee"]);
                                        if (pa.Trim() != "")
                                        {
                                            pepartotal = Convert.ToDouble(pa);
                                        }
                                    }

                                }
                            }
                            /////////PHOTOCOPY REPORT///////
                            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 20, 10, 450);
                            PdfTextArea pdfk = new PdfTextArea(font, System.Drawing.Color.Black, new PdfArea(mydoc, 500, 10, 60, 60), System.Drawing.ContentAlignment.TopCenter, "P");
                            mypdfpage.Add(pdfk);
                            PdfTextArea pdf = new PdfTextArea(Fontbold4, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 20, 400, 30), System.Drawing.ContentAlignment.TopCenter, collegenew1);
                            PdfTextArea pdfaddr1 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 40, 400, 30), System.Drawing.ContentAlignment.TopCenter, address1);
                            PdfTextArea pdfaddr2 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 55, 400, 30), System.Drawing.ContentAlignment.TopCenter, address2 + " " + pincode);
                            PdfTextArea pdfintruct = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 80, 400, 30), System.Drawing.ContentAlignment.TopCenter, "INSTRUCTIONS TO THE CANDIDATES");
                            PdfTextArea pdf1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 50, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "1.Fee for Photocopy is Rs." + pepartotal + "/- per paper");
                            PdfTextArea pdf2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 60, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "2.Application for Photocopy must be submitted to the Controller of Examinations on or before " + sk + ". ");
                            PdfTextArea pdf3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 70, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "3.There is no provision for Photocopy of Practical & Project examination Papers. ");
                            PdfTextArea pdf4 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 80, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "4.Incomplete/defective application will be rejected.");
                            PdfTextArea pdf5 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 90, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "5.No Application will be accepted beyond the due date prescribed.");
                            PdfTextArea pdf6 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 105, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "6.The Head of the Department should ensure while recommending the application that the subject code and the subject(s) filled in the respective columns by the candidate are verified and found to be correct. ");
                            // PdfTextArea pdf7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 130, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "   the subject(s) filled in the respective columns by the candidate are verified and found to be correct ");

                            PdfTextArea pdf8 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 300, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "7. Subjects for which Photocopy is required :");
                            PdfTextArea pdf9 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 610, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "8. Recommendation of the HOD :");
                            PdfTextArea pdf10 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 640, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Signature of the HOD");
                            PdfTextArea pdf11_sig = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 10, y + 640, 400, 30), System.Drawing.ContentAlignment.BottomRight, "Signature of the Candidate");
                            PdfTextArea pdf12 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 670, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Station :");
                            PdfTextArea pdf13 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 690, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Date :");
                            PdfTextArea pdf14 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y + 670, 400, 30), System.Drawing.ContentAlignment.BottomRight, "Signature of the Controller of Examinations");
                            mypdfpage.Add(pdf);
                            mypdfpage.Add(pdfaddr1);
                            mypdfpage.Add(pdfaddr2);
                            mypdfpage.Add(pdfintruct);
                            mypdfpage.Add(pdf1);
                            mypdfpage.Add(pdf2);
                            mypdfpage.Add(pdf3);
                            mypdfpage.Add(pdf4);
                            mypdfpage.Add(pdf5);
                            mypdfpage.Add(pdf6);
                            // mypdfpage.Add(pdf7);
                            mypdfpage.Add(pdf8);
                            mypdfpage.Add(pdf9);
                            mypdfpage.Add(pdf10);
                            mypdfpage.Add(pdf11_sig);
                            mypdfpage.Add(pdf12);
                            mypdfpage.Add(pdf13);
                            mypdfpage.Add(pdf14);

                            Gios.Pdf.PdfTable table = mydoc.NewTable(Fontbold, 7, 2, 1);
                            Gios.Pdf.PdfTable table_photocopy = mydoc.NewTable(Fontbold, ds2.Tables[0].Rows.Count + 2, 6, 1);


                            table = mydoc.NewTable(Fontbold, 7, 2, 1);
                            table_photocopy = mydoc.NewTable(Fontbold, ds2.Tables[0].Rows.Count + 2, 6, 1);
                            table.VisibleHeaders = false;
                            // table.Rows[1].SetRowHeight(200);
                            table.SetRowHeight(100);
                            table.CellRange(0, 0, 0, 1).SetFont(Fontbold);

                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(0, 0).SetContent("1. Name :");

                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(0, 1).SetContent(studname);
                            table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(1, 0).SetContent("2. Register Number :");
                            table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(1, 1).SetContent(Regno);
                            table.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(2, 0).SetContent("3. Name of the Department :");
                            table.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(2, 1).SetContent(branch_name);
                            table.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(3, 0).SetContent("4. Degree & Branch Name :");
                            table.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(3, 1).SetContent(dept);
                            table.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(4, 0).SetContent("5. Month  & Year of Examination :");
                            table.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(4, 1).SetContent(exammnth.SelectedItem.Text + " - " + ddlyear.SelectedItem.Text);
                            table.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(5, 0).SetContent("6. No.of papers applied for Revaluation :");
                            table.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(5, 1).SetContent(Convert.ToString(ds2.Tables[0].Rows.Count));
                            table.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(6, 0).SetContent("7. Amount of fee paid :");
                            table.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(6, 1).SetContent(Convert.ToString("Rs" + "." + totalamt + "/-"));

                            table_photocopy.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table_photocopy.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_photocopy.Cell(0, 0).SetContent("Semester No. ");
                            //table_photocopy.Cell(1, 0).SetContent(semester);
                            table_photocopy.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_photocopy.Cell(0, 1).SetContent("Subject Code");
                            //table_photocopy.Cell(1, 1).SetContent(subjectcode);
                            table_photocopy.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_photocopy.Cell(0, 2).SetContent("Subject Title");
                            // table_photocopy.Cell(1, 2).SetContent(subjecttit);
                            table_photocopy.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_photocopy.Cell(0, 3).SetContent("Marks awarded");
                            table_photocopy.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_photocopy.Cell(1, 3).SetContent("IM");

                            table_photocopy.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_photocopy.Cell(1, 4).SetContent("UM");
                            // table_photocopy.Cell(0, 4).SetContent(totalamt1);
                            table_photocopy.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_photocopy.Cell(1, 5).SetContent("Total");
                            // table_photocopy.Cell(0, 5).SetContent(total);

                            foreach (PdfCell pr in table_photocopy.CellRange(0, 3, 0, 3).Cells)
                            {
                                pr.ColSpan = 3;
                            }

                            foreach (PdfCell pr in table_photocopy.CellRange(0, 0, 0, 0).Cells)
                            {
                                pr.RowSpan = 2;
                            }

                            foreach (PdfCell pr in table_photocopy.CellRange(0, 1, 0, 1).Cells)
                            {
                                pr.RowSpan = 2;
                            }

                            foreach (PdfCell pr in table_photocopy.CellRange(0, 2, 0, 2).Cells)
                            {
                                pr.RowSpan = 2;
                            }
                            table_photocopy.VisibleHeaders = false;
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                int row = 1;
                                for (int k = 0; k < ds2.Tables[0].Rows.Count; k++)
                                {
                                    row++;
                                    semester = Convert.ToString(ds2.Tables[0].Rows[k]["semester"]);
                                    subjectcode = Convert.ToString(ds2.Tables[0].Rows[k]["subject_code"]);
                                    subjecttit = Convert.ToString(ds2.Tables[0].Rows[k]["subject_name"]);
                                    total = Convert.ToString(ds2.Tables[0].Rows[k]["total"]);
                                    internal1 = Convert.ToString(ds2.Tables[0].Rows[k]["internal_mark"]);
                                    external = Convert.ToString(ds2.Tables[0].Rows[k]["external_mark"]);
                                    string grade = Convert.ToString(ds2.Tables[0].Rows[k]["grade"]);
                                    // table_photocopy.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_photocopy.Cell(row, 0).SetContent(semester);
                                    table_photocopy.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_photocopy.Cell(row, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_photocopy.Cell(row, 1).SetContent(subjectcode);

                                    table_photocopy.Cell(row, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table_photocopy.Cell(row, 2).SetContent(subjecttit);

                                    //table_photocopy.Cell(row, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                                    //table_photocopy.Cell(row, 4).SetContentAlignment(ContentAlignment.MiddleLeft);

                                    //table_photocopy.Cell(row, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                                    table_photocopy.Cell(row, 3).SetContent(internal1);
                                    table_photocopy.Cell(row, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_photocopy.Columns[0].SetWidth(12);
                                    table_photocopy.Columns[1].SetWidth(10);
                                    table_photocopy.Columns[2].SetWidth(50);
                                    table_photocopy.Cell(row, 4).SetContent(external);
                                    table_photocopy.Cell(row, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_photocopy.Cell(row, 5).SetContent(total);
                                    table_photocopy.Cell(row, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                                }
                                Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 200, 488, 500));
                                mypdfpage.Add(newpdftabpage);
                                Gios.Pdf.PdfTablePage newpdftabpage_photo = table_photocopy.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 380, 488, 400));
                                mypdfpage.Add(newpdftabpage_photo);
                                mypdfpage.SaveToDocument();
                            }


                        }
                        else if (ddltyp.SelectedItem.Value == "2")
                        {


                            //string query1 = "select ea.subject_no ,e.roll_no,s.subject_name,m.internal_mark,m.external_mark ,m.total ,s.subject_code,sy.semester,ea.fee,re_tot,re_val,subject_type,grade,improvement_fee  from Exam_Details ed,exam_appl_details ea,exam_application e,subject s,mark_entry m,syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no and e.Exam_type =2 and m.exam_code =ed.exam_code and e.exam_code =m.exam_code and s.subject_no =ea.subject_no and e.roll_no =m.roll_no and s.subject_no =m.subject_no and m.subject_no =ea.subject_no and su.syll_code =sy.syll_code and su.subType_no =s.subType_no  and  sy.syll_code =s.syll_code and e.roll_no ='" + Rollno + "'and ed.exam_month='" + exammnth.SelectedItem.Value + "' and ed.exam_year='" + ddlyear.SelectedItem.Value + "'";
                            //ds2.Clear();
                            //ds2 = da.select_method_wo_parameter(query1, "Text");
                            string lastdate = "";
                            double totalamt = 0;
                            double pepartotal = 0;
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                {
                                    lastdate = Convert.ToString(ds2.Tables[0].Rows[j]["LastDate"]);
                                    string fee = Convert.ToString(ds2.Tables[0].Rows[j]["re_tot"]);
                                    string subjecttype = Convert.ToString(ds2.Tables[0].Rows[j]["subject_type"]);
                                    if (fee.Trim() != "")
                                    {
                                        totalamt = totalamt + Convert.ToDouble(fee);
                                    }
                                    if (subjecttype.ToString().ToUpper() == "THEORY")
                                    {
                                        string pa = Convert.ToString(ds2.Tables[0].Rows[j]["re_tot"]);
                                        if (pa.Trim() != "")
                                        {
                                            pepartotal = Convert.ToDouble(pa);
                                        }
                                    }

                                }
                            }

                            ///////////RETOTALING REPORT///////////

                            Gios.Pdf.PdfPage mypdfpage1 = mydoc.NewPage();

                            PdfImage LogoImage1tot = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage1.Add(LogoImage1tot, 20, 10, 450);

                            //PdfImage LogoImage11tot = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            //mypdfpage1.Add(LogoImage11tot, 500, 10, 450);

                            PdfTextArea pdfk = new PdfTextArea(font, System.Drawing.Color.Black, new PdfArea(mydoc, 500, 10, 60, 60), System.Drawing.ContentAlignment.TopCenter, "RT");
                            mypdfpage1.Add(pdfk);

                            PdfTextArea pdfaddr = new PdfTextArea(Fontbold4, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 20, 400, 30), System.Drawing.ContentAlignment.TopCenter, collegenew1);
                            PdfTextArea pdfaddr11 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 40, 400, 30), System.Drawing.ContentAlignment.TopCenter, address1);
                            PdfTextArea pdfaddr22 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 55, 400, 30), System.Drawing.ContentAlignment.TopCenter, address2 + " " + pincode);
                            PdfTextArea pdfintruct_1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 80, 400, 30), System.Drawing.ContentAlignment.TopCenter, "INSTRUCTIONS TO THE CANDIDATES");
                            PdfTextArea pdf11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 55, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "1.Fee for Retotaling the answer scripts is Rs." + pepartotal + "/- per paper.");
                            PdfTextArea pdf22 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 65, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "2.Application for Retotaling the answer scripts must be submitted to the Controller of Examination on or before " + sk + ".");
                            PdfTextArea pdf33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 75, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "3.There is no provision for Retotaling the answer scripts of Practical & Project examinations Papers. ");
                            PdfTextArea pdf44 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 85, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "4.Incomplete/defective application will be rejected.");
                            PdfTextArea pdf55 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 95, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "5.No Application will be accepted beyond the due date prescribed.");
                            PdfTextArea pdf66 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 110, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "6.The Head of the Department should ensure while recommending the application that the subject code and the subject(s) filled in the respective columns by the candidate are verified and found to be correct. ");
                            // PdfTextArea pdf77 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 130, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "   the subject(s) filled in the respective columns by the candidate are verified and found to be correct ");
                            PdfTextArea pdf88 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 320, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "7. Subjects for which Retotal is required :");

                            PdfTextArea pdf99 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 610, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "8. Recommendation of the HOD :");
                            PdfTextArea pdf100 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 640, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Signature of the HOD");
                            PdfTextArea pdf11_sig1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 10, y + 640, 400, 30), System.Drawing.ContentAlignment.BottomRight, "Signature of the Candidate");
                            PdfTextArea pdf122 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 670, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Station :");
                            PdfTextArea pdf133 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 690, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Date :");
                            PdfTextArea pdf144 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y + 670, 400, 30), System.Drawing.ContentAlignment.BottomRight, "Signature of the Controller of Examinations");

                            mypdfpage1.Add(pdfaddr);
                            mypdfpage1.Add(pdfaddr11);
                            mypdfpage1.Add(pdfaddr22);
                            mypdfpage1.Add(pdfintruct_1);
                            mypdfpage1.Add(pdf11);
                            mypdfpage1.Add(pdf22);
                            mypdfpage1.Add(pdf33);
                            mypdfpage1.Add(pdf44);
                            mypdfpage1.Add(pdf55);
                            mypdfpage1.Add(pdf66);
                            //mypdfpage1.Add(pdf77);
                            mypdfpage1.Add(pdf88);

                            mypdfpage1.Add(pdf99);
                            mypdfpage1.Add(pdf100);
                            mypdfpage1.Add(pdf11_sig1);
                            mypdfpage1.Add(pdf122);
                            mypdfpage1.Add(pdf133);
                            mypdfpage1.Add(pdf144);
                            Gios.Pdf.PdfTable table1 = mydoc.NewTable(Fontbold, 7, 2, 1);

                            Gios.Pdf.PdfTable table_retotal = mydoc.NewTable(Fontbold, ds2.Tables[0].Rows.Count + 2, 6, 1);

                            table1 = mydoc.NewTable(Fontbold, 7, 2, 1);
                            table_retotal = mydoc.NewTable(Fontbold, ds2.Tables[0].Rows.Count + 2, 6, 1);
                            //   table1.SetRowHeight(100);
                            table1.VisibleHeaders = false;
                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(0, 0).SetContent("1. Name :");
                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(0, 1).SetContent(studname);
                            table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(1, 0).SetContent("2. Register Number :");

                            table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(1, 1).SetContent(Regno);
                            table1.Cell(2, 0).SetContent("3. Name of the Department :");
                            table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(2, 1).SetContent(branch_name);
                            table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(3, 0).SetContent("4. Degree & Branch Name :");
                            table1.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(3, 1).SetContent(dept);
                            table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(4, 0).SetContent("5. Month  & Year of Examination :");
                            table1.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(4, 1).SetContent(exammnth.SelectedItem.Text + " - " + ddlyear.SelectedItem.Text);
                            table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(5, 0).SetContent("6. No.of papers applied for Revaluation :");

                            table1.Cell(5, 1).SetContent(Convert.ToString(ds2.Tables[0].Rows.Count));
                            table1.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                            table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(6, 0).SetContent("7. Amount of fee paid : ");

                            table1.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table1.Cell(6, 1).SetContent(Convert.ToString("Rs" + "." + totalamt + "/-"));

                            table_retotal.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table_retotal.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_retotal.Cell(0, 0).SetContent("Semester No. ");
                            //table_retotal.Cell(1, 0).SetContent(semester);
                            table_retotal.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_retotal.Cell(0, 1).SetContent("Subject Code");
                            //table_retotal.Cell(1, 1).SetContent(subjectcode);
                            table_retotal.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_retotal.Cell(0, 2).SetContent("Subject Title");
                            // table_retotal.Cell(1, 2).SetContent(subjecttit);
                            table_retotal.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_retotal.Cell(0, 3).SetContent("Marks awarded");

                            foreach (PdfCell pr in table_retotal.CellRange(0, 3, 0, 3).Cells)
                            {
                                pr.ColSpan = 3;
                            }

                            foreach (PdfCell pr in table_retotal.CellRange(0, 0, 0, 0).Cells)
                            {
                                pr.RowSpan = 2;
                            }

                            foreach (PdfCell pr in table_retotal.CellRange(0, 1, 0, 1).Cells)
                            {
                                pr.RowSpan = 2;
                            }

                            foreach (PdfCell pr in table_retotal.CellRange(0, 2, 0, 2).Cells)
                            {
                                pr.RowSpan = 2;
                            }

                            table_retotal.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_retotal.Cell(1, 3).SetContent("IM");
                            table_retotal.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_retotal.Cell(1, 4).SetContent("UM");
                            // table_retotal.Cell(0, 4).SetContent(external);
                            table_retotal.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_retotal.Cell(1, 5).SetContent("Total");
                            //table_retotal.Cell(0, 5).SetContent(total);
                            table_retotal.VisibleHeaders = false;
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                int row = 1;
                                for (int k = 0; k < ds2.Tables[0].Rows.Count; k++)
                                {
                                    row++;
                                    semester = Convert.ToString(ds2.Tables[0].Rows[k]["semester"]);
                                    subjectcode = Convert.ToString(ds2.Tables[0].Rows[k]["subject_code"]);
                                    subjecttit = Convert.ToString(ds2.Tables[0].Rows[k]["subject_name"]);
                                    total = Convert.ToString(ds2.Tables[0].Rows[k]["total"]);
                                    internal1 = Convert.ToString(ds2.Tables[0].Rows[k]["internal_mark"]);
                                    external = Convert.ToString(ds2.Tables[0].Rows[k]["external_mark"]);
                                    string grade = Convert.ToString(ds2.Tables[0].Rows[k]["grade"]);
                                    //table_retotal.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_retotal.Cell(row, 0).SetContent(semester);
                                    table_retotal.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_retotal.Cell(row, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_retotal.Cell(row, 1).SetContent(subjectcode);
                                    table_retotal.Cell(row, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table_retotal.Cell(row, 2).SetContent(subjecttit);


                                    table_retotal.Columns[0].SetWidth(12);
                                    table_retotal.Columns[1].SetWidth(10);
                                    table_retotal.Columns[2].SetWidth(50);

                                    table_retotal.Cell(row, 3).SetContent(internal1);
                                    table_retotal.Cell(row, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_retotal.Cell(row, 4).SetContent(external);
                                    table_retotal.Cell(row, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_retotal.Cell(row, 5).SetContent(total);
                                    table_retotal.Cell(row, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    //table_retotal.Cell(row, 3).SetContent(internal1);
                                    //table_retotal.Cell(row, 4).SetContent(external);

                                }
                                Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 220, 488, 500));
                                mypdfpage1.Add(newpdftabpage1);
                                Gios.Pdf.PdfTablePage newpdftabpage_retotal = table_retotal.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 400, 488, 400));
                                mypdfpage1.Add(newpdftabpage_retotal);
                                mypdfpage1.SaveToDocument();

                            }


                        }
                        else if (ddltyp.SelectedItem.Value == "3")
                        {

                            double totalamt = 0;
                            double pepartotal = 0;
                            string lastdate = "";
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                {
                                    lastdate = Convert.ToString(ds2.Tables[0].Rows[j]["LastDate"]);
                                    string fee = Convert.ToString(ds2.Tables[0].Rows[j]["re_val"]);
                                    string subjecttype = Convert.ToString(ds2.Tables[0].Rows[j]["subject_type"]);
                                    if (fee.Trim() != "")
                                    {
                                        totalamt = totalamt + Convert.ToDouble(fee);
                                    }
                                    if (subjecttype.ToString().ToUpper() == "THEORY")
                                    {
                                        string pa = Convert.ToString(ds2.Tables[0].Rows[j]["re_val"]);
                                        if (pa.Trim() != "")
                                        {
                                            pepartotal = Convert.ToDouble(pa);
                                        }
                                    }

                                }
                            }

                            //////////////REVALUATION REPORT////////////////

                            Gios.Pdf.PdfPage mypdfpage2 = mydoc.NewPage();

                            PdfImage LogoImage1rev = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage2.Add(LogoImage1rev, 20, 10, 450);

                            //PdfImage LogoImage11rev = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            //mypdfpage2.Add(LogoImage1rev, 500, 10, 450);

                            PdfTextArea pdfk = new PdfTextArea(font, System.Drawing.Color.Black, new PdfArea(mydoc, 500, 10, 60, 60), System.Drawing.ContentAlignment.TopCenter, "RV");
                            mypdfpage2.Add(pdfk);

                            PdfTextArea pdfaddrrev = new PdfTextArea(Fontbold4, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 20, 400, 30), System.Drawing.ContentAlignment.TopCenter, collegenew1);
                            PdfTextArea pdfaddr111 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 40, 400, 30), System.Drawing.ContentAlignment.TopCenter, address1);
                            PdfTextArea pdfaddr222 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 55, 400, 30), System.Drawing.ContentAlignment.TopCenter, address2 + " " + pincode);
                            PdfTextArea pdfintruct_11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 80, 400, 30), System.Drawing.ContentAlignment.TopCenter, "INSTRUCTIONS TO THE CANDIDATES");
                            PdfTextArea pdf111 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 50, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "1.Fee for Revaluation is Rs." + pepartotal + "/- per paper.");
                            PdfTextArea pdf222 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 60, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "2.Application for Revaluation must be submitted to the Controller of Examinations on or before " + sk + ".");
                            PdfTextArea pdf333 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 70, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "3.There is no provision for Revaluation of Practical & Project examinations Papers.");
                            PdfTextArea pdf444 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 80, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "4.Incomplete/defective application will be rejected.");
                            PdfTextArea pdf555 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 90, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "5.No Application will be accepted beyond the due date prescribed.");
                            PdfTextArea pdf666 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 105, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "6.The Head of the Department should ensure while recommending the application that the subject code and the subject(s) filled in the respective columns by the candidate are verified and found to be correct.");
                            // PdfTextArea pdf777 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 120, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "   the subject(s) filled in the respective columns by the candidate are verified and found to be correct ");
                            PdfTextArea pdf888 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 300, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "7. Subjects for which Revaluation is required:");

                            PdfTextArea pdf999 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 610, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "8. Recommendation of the HOD :");
                            PdfTextArea pdf1000 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 640, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Signature of the HOD");
                            PdfTextArea pdf11_sig11 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 10, y + 640, 400, 30), System.Drawing.ContentAlignment.BottomRight, "Signature of the Candidate");
                            PdfTextArea pdf1222 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 670, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Station :");
                            PdfTextArea pdf1333 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 690, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Date :");
                            PdfTextArea pdf1444 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y + 670, 400, 30), System.Drawing.ContentAlignment.BottomRight, "Signature of the Controller of Examinations");

                            mypdfpage2.Add(pdfaddrrev);
                            mypdfpage2.Add(pdfaddr111);
                            mypdfpage2.Add(pdfaddr222);
                            mypdfpage2.Add(pdfintruct_11);
                            mypdfpage2.Add(pdf111);
                            mypdfpage2.Add(pdf222);
                            mypdfpage2.Add(pdf333);
                            mypdfpage2.Add(pdf444);
                            mypdfpage2.Add(pdf555);
                            mypdfpage2.Add(pdf666);
                            // mypdfpage2.Add(pdf777);
                            mypdfpage2.Add(pdf888);

                            mypdfpage2.Add(pdf999);
                            mypdfpage2.Add(pdf1000);
                            mypdfpage2.Add(pdf11_sig11);
                            mypdfpage2.Add(pdf1222);
                            mypdfpage2.Add(pdf1333);
                            mypdfpage2.Add(pdf1444);

                            Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontbold, 7, 2, 1);

                            Gios.Pdf.PdfTable table_reval = mydoc.NewTable(Fontbold, ds2.Tables[0].Rows.Count + 2, 6, 1);
                            table2 = mydoc.NewTable(Fontbold, 7, 2, 1);
                            table_reval = mydoc.NewTable(Fontbold, ds2.Tables[0].Rows.Count + 2, 6, 1);
                            table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(0, 0).SetContent("1. Name :");
                            table2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(0, 1).SetContent(studname);
                            table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(1, 0).SetContent("2. Register Number :");
                            table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(1, 1).SetContent(Regno);
                            table2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(2, 0).SetContent("3. Name of the Department :");
                            table2.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(2, 1).SetContent(branch_name);
                            table2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(3, 0).SetContent("4. Degree & Branch :");
                            table2.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(3, 1).SetContent(dept);
                            table2.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(4, 0).SetContent("5. Month  & Year of Examination :");
                            table2.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(4, 1).SetContent(exammnth.SelectedItem.Text + " - " + ddlyear.SelectedItem.Text);
                            table2.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(5, 0).SetContent("6. No.of papers applied for Revaluation :");

                            table2.Cell(5, 1).SetContent(Convert.ToString(ds2.Tables[0].Rows.Count));
                            table2.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                            table2.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(6, 0).SetContent("7. Amount of fee paid : ");

                            table2.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(6, 1).SetContent(Convert.ToString("Rs" + "." + totalamt + "/-"));
                            table2.VisibleHeaders = false;
                            table_reval.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table_reval.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_reval.Cell(0, 0).SetContent("Semester No. ");
                            // table_reval.Cell(1, 0).SetContent(semester);
                            table_reval.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_reval.Cell(0, 1).SetContent("Subject Code");
                            // table_reval.Cell(1, 1).SetContent(subjectcode);
                            table_reval.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_reval.Cell(0, 2).SetContent("Subject Title");
                            // table_reval.Cell(1, 2).SetContent(subjecttit);
                            table_reval.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_reval.Cell(0, 3).SetContent("Marks awarded");
                            table_reval.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_reval.Cell(1, 3).SetContent("IM");
                            table_reval.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_reval.Cell(1, 4).SetContent("UM");
                            // table_reval.Cell(0, 4).SetContent(external);
                            table_reval.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table_reval.Cell(1, 5).SetContent("Total");
                            table_reval.VisibleHeaders = false;
                            //table_reval.Cell(0, 5).SetContent(total);

                            foreach (PdfCell pr in table_reval.CellRange(0, 3, 0, 3).Cells)
                            {
                                pr.ColSpan = 3;
                            }

                            foreach (PdfCell pr in table_reval.CellRange(0, 0, 0, 0).Cells)
                            {
                                pr.RowSpan = 2;
                            }

                            foreach (PdfCell pr in table_reval.CellRange(0, 1, 0, 1).Cells)
                            {
                                pr.RowSpan = 2;
                            }

                            foreach (PdfCell pr in table_reval.CellRange(0, 2, 0, 2).Cells)
                            {
                                pr.RowSpan = 2;
                            }

                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                int row = 1;
                                for (int k = 0; k < ds2.Tables[0].Rows.Count; k++)
                                {
                                    row++;
                                    semester = Convert.ToString(ds2.Tables[0].Rows[k]["semester"]);

                                    subjectcode = Convert.ToString(ds2.Tables[0].Rows[k]["subject_code"]);
                                    subjecttit = Convert.ToString(ds2.Tables[0].Rows[k]["subject_name"]);
                                    total = Convert.ToString(ds2.Tables[0].Rows[k]["total"]);
                                    internal1 = Convert.ToString(ds2.Tables[0].Rows[k]["internal_mark"]);
                                    external = Convert.ToString(ds2.Tables[0].Rows[k]["external_mark"]);
                                    string grade = Convert.ToString(ds2.Tables[0].Rows[k]["grade"]);

                                    //table_reval.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_reval.Cell(row, 0).SetContent(semester);
                                    table_reval.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_reval.Cell(row, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_reval.Cell(row, 1).SetContent(subjectcode);
                                    table_reval.Cell(row, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table_reval.Cell(row, 2).SetContent(subjecttit);


                                    table_reval.Columns[0].SetWidth(12);
                                    table_reval.Columns[1].SetWidth(10);
                                    table_reval.Columns[2].SetWidth(50);

                                    table_reval.Cell(row, 3).SetContent(internal1);
                                    table_reval.Cell(row, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_reval.Cell(row, 4).SetContent(external);
                                    table_reval.Cell(row, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table_reval.Cell(row, 5).SetContent(total);
                                    table_reval.Cell(row, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    //table_reval.Cell(row, 3).SetContent(internal1);
                                    //table_reval.Cell(row, 4).SetContent(external);

                                }

                            }

                            Gios.Pdf.PdfTablePage newpdftabpage2 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 200, 488, 500));
                            mypdfpage2.Add(newpdftabpage2);
                            Gios.Pdf.PdfTablePage newpdftabpage_reval = table_reval.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 380, 488, 400));
                            mypdfpage2.Add(newpdftabpage_reval);
                            mypdfpage2.SaveToDocument();
                        }
                        else if (ddltyp.SelectedItem.Value == "4")
                        {
                            string lastdate = "";
                            double totalamt = 0;
                            double totalamt1 = 0;
                            double pepartotal = 0;
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                {
                                    // lastdate = Convert.ToString(ds2.Tables[0].Rows[j]["LastDate"]);
                                    lastdate = da.GetFunction("select CONVERT(varchar(20), LastDate ,103) as LastDate from exam_application where Exam_type=4 and roll_no ='" + Rollno + "' and exam_code='" + exam_code_l + "'");
                                    string fee = Convert.ToString(ds2.Tables[0].Rows[j]["arr_fee"]);
                                    string subjecttype = Convert.ToString(ds2.Tables[0].Rows[j]["subject_type"]);
                                    string internalmark = Convert.ToString(ds2.Tables[0].Rows[j]["internal_mark"]);
                                    string externalmark = Convert.ToString(ds2.Tables[0].Rows[j]["external_mark"]);

                                    if (fee.Trim() != "")
                                    {
                                        totalamt = totalamt + Convert.ToDouble(fee);
                                    }
                                    if (externalmark.Trim() != "")
                                    {
                                        totalamt1 = totalamt1 + Convert.ToDouble(externalmark);
                                    }
                                    if (subjecttype.ToString().ToUpper() == "THEORY")
                                    {
                                        string pa = Convert.ToString(ds2.Tables[0].Rows[j]["arr_fee"]);
                                        if (pa.Trim() != "")
                                        {
                                            pepartotal = Convert.ToDouble(pa);
                                        }
                                    }

                                }
                                /////////Supplementory REPORT///////
                                Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 20, 10, 450);
                                PdfTextArea pdfk = new PdfTextArea(font, System.Drawing.Color.Black, new PdfArea(mydoc, 500, 10, 60, 60), System.Drawing.ContentAlignment.TopCenter, "SP");
                                mypdfpage.Add(pdfk);
                                PdfTextArea pdf = new PdfTextArea(Fontbold4, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 20, 400, 30), System.Drawing.ContentAlignment.TopCenter, collegenew1);
                                PdfTextArea pdfaddr1 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 40, 400, 30), System.Drawing.ContentAlignment.TopCenter, address1);
                                PdfTextArea pdfaddr2 = new PdfTextArea(Fontbold3, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 55, 400, 30), System.Drawing.ContentAlignment.TopCenter, address2 + " " + pincode);
                                PdfTextArea pdfintruct = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 90, 80, 400, 30), System.Drawing.ContentAlignment.TopCenter, "INSTRUCTIONS TO THE CANDIDATES");
                                PdfTextArea pdf1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 50, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "1.Fee for Supplementary Exam is Rs." + pepartotal + "/- per paper");
                                PdfTextArea pdf2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 60, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "2.Application for Supplementary Exam must be submitted to the Controller of Examinations on or before " + sk + ". ");
                                PdfTextArea pdf3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 70, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "3.There is no provision for Supplementary Exam for Practical & Project examination Papers. ");
                                PdfTextArea pdf4 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 80, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "4. Incomplete/defective application will be rejected.");
                                PdfTextArea pdf5 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 90, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "5.No Application will be accepted beyond the due date prescribed. ");
                                PdfTextArea pdf6 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 105, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "6.The Head of the Department should ensure while recommending the application that the subject code and the subject(s) filled in the respective columns by the candidate are verified and found to be correct. ");
                                // PdfTextArea pdf7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 130, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "   the subject(s) filled in the respective columns by the candidate are verified and found to be correct ");

                                PdfTextArea pdf8 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 300, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "7.Subject Details :");
                                PdfTextArea pdf9 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 610, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "8. Recommendation of the HOD :");
                                PdfTextArea pdf10 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 640, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Signature of the HOD");
                                PdfTextArea pdf11_sig = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 10, y + 640, 400, 30), System.Drawing.ContentAlignment.BottomRight, "Signature of the Candidate");
                                PdfTextArea pdf12 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 670, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Station :");
                                PdfTextArea pdf13 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 40, y + 690, 400, 30), System.Drawing.ContentAlignment.BottomLeft, "Date :");
                                PdfTextArea pdf14 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 80, y + 670, 400, 30), System.Drawing.ContentAlignment.BottomRight, "Signature of the Controller of Examinations");
                                mypdfpage.Add(pdf);
                                mypdfpage.Add(pdfaddr1);
                                mypdfpage.Add(pdfaddr2);
                                mypdfpage.Add(pdfintruct);
                                mypdfpage.Add(pdf1);
                                mypdfpage.Add(pdf2);
                                mypdfpage.Add(pdf3);
                                mypdfpage.Add(pdf4);
                                mypdfpage.Add(pdf5);
                                mypdfpage.Add(pdf6);
                                // mypdfpage.Add(pdf7);
                                mypdfpage.Add(pdf8);
                                mypdfpage.Add(pdf9);
                                mypdfpage.Add(pdf10);
                                mypdfpage.Add(pdf11_sig);
                                mypdfpage.Add(pdf12);
                                mypdfpage.Add(pdf13);
                                mypdfpage.Add(pdf14);

                                Gios.Pdf.PdfTable table = mydoc.NewTable(Fontbold, 7, 2, 1);
                                Gios.Pdf.PdfTable table_photocopy = mydoc.NewTable(Fontbold, ds2.Tables[0].Rows.Count + 2, 6, 1);


                                table = mydoc.NewTable(Fontbold, 7, 2, 1);
                                table_photocopy = mydoc.NewTable(Fontbold, ds2.Tables[0].Rows.Count + 2, 6, 1);
                                table.VisibleHeaders = false;
                                // table.Rows[1].SetRowHeight(200);
                                table.SetRowHeight(100);
                                table.CellRange(0, 0, 0, 1).SetFont(Fontbold);

                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(0, 0).SetContent("1. Name :");

                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(0, 1).SetContent(studname);
                                table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(1, 0).SetContent("2. Register Number :");
                                table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(1, 1).SetContent(Regno);
                                table.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(2, 0).SetContent("3. Name of the Department :");
                                table.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(2, 1).SetContent(branch_name);
                                table.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(3, 0).SetContent("4. Degree & Branch Name :");
                                table.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(3, 1).SetContent(dept);
                                table.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(4, 0).SetContent("5. Month  & Year of Examination :");
                                table.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(4, 1).SetContent(exammnth.SelectedItem.Text + " - " + ddlyear.SelectedItem.Text);
                                table.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(5, 0).SetContent("6. No.of papers applied for Supplementary Exam : ");
                                table.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(5, 1).SetContent(Convert.ToString(ds2.Tables[0].Rows.Count));
                                table.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(6, 0).SetContent("7. Amount of fee paid :");
                                table.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(6, 1).SetContent(Convert.ToString("Rs" + "." + totalamt + "/-"));

                                table_photocopy.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table_photocopy.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table_photocopy.Cell(0, 0).SetContent("Semester No. ");
                                //table_photocopy.Cell(1, 0).SetContent(semester);
                                table_photocopy.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table_photocopy.Cell(0, 1).SetContent("Subject Code");
                                //table_photocopy.Cell(1, 1).SetContent(subjectcode);
                                table_photocopy.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table_photocopy.Cell(0, 2).SetContent("Subject Title");
                                // table_photocopy.Cell(1, 2).SetContent(subjecttit);
                                table_photocopy.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table_photocopy.Cell(0, 3).SetContent("Marks awarded");
                                table_photocopy.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table_photocopy.Cell(1, 3).SetContent("IM");

                                table_photocopy.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table_photocopy.Cell(1, 4).SetContent("UM");
                                // table_photocopy.Cell(0, 4).SetContent(totalamt1);
                                table_photocopy.Cell(1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table_photocopy.Cell(1, 5).SetContent("Total");
                                // table_photocopy.Cell(0, 5).SetContent(total);

                                foreach (PdfCell pr in table_photocopy.CellRange(0, 3, 0, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }

                                foreach (PdfCell pr in table_photocopy.CellRange(0, 0, 0, 0).Cells)
                                {
                                    pr.RowSpan = 2;
                                }

                                foreach (PdfCell pr in table_photocopy.CellRange(0, 1, 0, 1).Cells)
                                {
                                    pr.RowSpan = 2;
                                }

                                foreach (PdfCell pr in table_photocopy.CellRange(0, 2, 0, 2).Cells)
                                {
                                    pr.RowSpan = 2;
                                }
                                table_photocopy.VisibleHeaders = false;
                                if (ds2.Tables[0].Rows.Count > 0)
                                {
                                    int row = 1;
                                    for (int k = 0; k < ds2.Tables[0].Rows.Count; k++)
                                    {
                                        row++;
                                        semester = Convert.ToString(ds2.Tables[0].Rows[k]["semester"]);
                                        subjectcode = Convert.ToString(ds2.Tables[0].Rows[k]["subject_code"]);
                                        subjecttit = Convert.ToString(ds2.Tables[0].Rows[k]["subject_name"]);
                                        total = Convert.ToString(ds2.Tables[0].Rows[k]["total"]);
                                        internal1 = Convert.ToString(ds2.Tables[0].Rows[k]["internal_mark"]);
                                        external = Convert.ToString(ds2.Tables[0].Rows[k]["external_mark"]);
                                        string grade = Convert.ToString(ds2.Tables[0].Rows[k]["grade"]);
                                        // table_photocopy.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table_photocopy.Cell(row, 0).SetContent(semester);
                                        table_photocopy.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table_photocopy.Cell(row, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table_photocopy.Cell(row, 1).SetContent(subjectcode);

                                        table_photocopy.Cell(row, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table_photocopy.Cell(row, 2).SetContent(subjecttit);
                                        table_photocopy.Cell(row, 3).SetContent(internal1);
                                        table_photocopy.Cell(row, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table_photocopy.Columns[0].SetWidth(12);
                                        table_photocopy.Columns[1].SetWidth(10);
                                        table_photocopy.Columns[2].SetWidth(50);
                                        table_photocopy.Cell(row, 4).SetContent(external);
                                        table_photocopy.Cell(row, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table_photocopy.Cell(row, 5).SetContent(total);
                                        table_photocopy.Cell(row, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                                    }

                                    Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 200, 488, 500));
                                    mypdfpage.Add(newpdftabpage);
                                    Gios.Pdf.PdfTablePage newpdftabpage_photo = table_photocopy.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 380, 488, 400));
                                    mypdfpage.Add(newpdftabpage_photo);
                                    mypdfpage.SaveToDocument();
                                    string appPath1 = HttpContext.Current.Server.MapPath("~");
                                    if (appPath1 != "")
                                    {
                                        string szPath = appPath1 + "/Report/";
                                        string szFile = "supplementaryReport.pdf";


                                        mydoc.SaveToFile(szPath + szFile);
                                        Response.ClearHeaders();
                                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                        Response.ContentType = "application/pdf";
                                        Response.WriteFile(szPath + szFile);
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
                            }


                        }

                        string appPath = HttpContext.Current.Server.MapPath("~");
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            if (ddltyp.SelectedItem.Value == "1")
                            {

                                if (appPath != "")
                                {
                                    string szPath = appPath + "/Report/";
                                    string szFile = "PhotocopyReport.pdf";


                                    mydoc.SaveToFile(szPath + szFile);
                                    Response.ClearHeaders();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                    Response.ContentType = "application/pdf";
                                    Response.WriteFile(szPath + szFile);
                                }
                            }
                            if (ddltyp.SelectedItem.Value == "2")
                            {

                                if (appPath != "")
                                {
                                    string szPath = appPath + "/Report/";
                                    string szFile = "RetotalingReport.pdf";


                                    mydoc.SaveToFile(szPath + szFile);
                                    Response.ClearHeaders();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                    Response.ContentType = "application/pdf";
                                    Response.WriteFile(szPath + szFile);
                                }
                            }
                            if (ddltyp.SelectedItem.Value == "3")
                            {
                                if (appPath != "")
                                {
                                    string szPath = appPath + "/Report/";
                                    string szFile = "RevaluationReport.pdf";


                                    mydoc.SaveToFile(szPath + szFile);
                                    Response.ClearHeaders();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                    Response.ContentType = "application/pdf";
                                    Response.WriteFile(szPath + szFile);
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
                        }
                        //if (ddltyp.SelectedItem.Value == "4")
                        //{
                        //    if (appPath != "")
                        //    {
                        //        string szPath = appPath + "/Report/";
                        //        string szFile = "supplementaryReport.pdf";


                        //        mydoc.SaveToFile(szPath + szFile);
                        //        Response.ClearHeaders();
                        //        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        //        Response.ContentType = "application/pdf";
                        //        Response.WriteFile(szPath + szFile);
                        //    }
                        //}

                    }
                }
                if (flag == false)
                {
                    gridviewrow.Visible = true;
                    butgen.Visible = true;
                    lblty.Visible = true;
                    ddltyp.Visible = true;
                    gridview1.Visible = true;

                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Any One Student\");", true);

                }
            }
        }

        catch
        {

        }
    }

    protected void butgen_Click(object sender, EventArgs e)
    {
        try
        {

            Font Fontbold = new Font("Book Antiqua", 10, FontStyle.Regular);
            Font Fontbold1 = new Font("Book Antiqua", 6, FontStyle.Regular);
            Font Fontbold2 = new Font("Book Antiqua", 7, FontStyle.Regular);
            Font Fontbold3 = new Font("Book Antiqua", 12, FontStyle.Regular);
            Font Fontbold4 = new Font("Book Antiqua", 16, FontStyle.Regular);
            Font Fontbold5 = new Font("Book Antiqua", 30, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            bindpdf(mydoc, Fontbold, Fontbold1, Fontbold2, Fontbold3, Fontbold4, Fontbold5);
        }
        catch
        {
        }
    }

    protected void gridview2(object sender, GridViewCommandEventArgs e)
    {
        string degreecode = "";
        string department = "";
        string branch = "";

        try
        {
            DataRow dr = null;
            int row = Convert.ToInt32(e.CommandArgument);
            string month = Convert.ToString(exammnth.Text);
            string year = Convert.ToString(ddlyear.Text);
            dt.Columns.Add("Reg_No", typeof(string));
            dt.Columns.Add("Student_Name", typeof(string));
            dt.Columns.Add("Roll_No", typeof(string));
            if (gridview1.Rows.Count > 0)
            {
                for (int i = 0; i < gridview1.Rows.Count; i++)
                {
                    if (row == i)
                    {
                        gridview1.Rows[i].BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    }
                    else
                    {
                        gridview1.Rows[i].BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    }
                }
            }

            if (e.CommandName == "Total")
            {
                degreecode = ((gridview1.Rows[row].FindControl("lbldegree_code") as Label).Text);
                department = ((gridview1.Rows[row].FindControl("gd_branch") as Label).Text);
                branch = ((gridview1.Rows[row].FindControl("gd_degcr") as Label).Text);
                ViewState["dept"] = branch + "-" + department;
                string query = "select r.Reg_No,r.roll_no ,r.Stud_Name from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month ='" + month + "' and Exam_year ='" + year + "' and r.degree_code in('" + degreecode + "') order by len(r.reg_no),r.reg_no ";
                if (ddltyp.SelectedItem.Value == "1")
                {
                    //  query = "select COUNT(ea.roll_no)as total,d.degree_code,r.batch_year,c.Course_Name ,dt.Dept_Name as department ,r.Current_Semester from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code  and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month =" + ex_month + " and Exam_year =" + year + " and Exam_type=5  group by r.batch_year ,d.degree_code ,Course_Name,Dept_Name,r.Current_Semester";
                    query = "select r.Reg_No,r.roll_no ,r.Stud_Name from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month ='" + month + "' and Exam_year ='" + year + "' and r.degree_code in('" + degreecode + "')  and Exam_type=5  order by len(r.reg_no),r.reg_no ";
                }
                else if (ddltyp.SelectedItem.Value == "2")
                {
                    //query = "select COUNT(ea.roll_no)as total,d.degree_code,r.batch_year,c.Course_Name ,dt.Dept_Name as department ,r.Current_Semester from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code  and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month =" + ex_month + " and Exam_year =" + year + " and Exam_type=3  group by r.batch_year ,d.degree_code ,Course_Name,Dept_Name,r.Current_Semester";
                    query = "select r.Reg_No,r.roll_no ,r.Stud_Name from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month ='" + month + "' and Exam_year ='" + year + "' and r.degree_code in('" + degreecode + "')  and Exam_type=3  order by len(r.reg_no),r.reg_no ";
                }
                else if (ddltyp.SelectedItem.Value == "3")
                {
                    // query = "select COUNT(ea.roll_no)as total,d.degree_code,r.batch_year,c.Course_Name ,dt.Dept_Name as department ,r.Current_Semester from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code  and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month =" + ex_month + " and Exam_year =" + year + " and Exam_type=2  group by r.batch_year ,d.degree_code ,Course_Name,Dept_Name,r.Current_Semester";
                    query = "select r.Reg_No,r.roll_no ,r.Stud_Name from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month ='" + month + "' and Exam_year ='" + year + "' and r.degree_code in('" + degreecode + "') and Exam_type=2 order by len(r.reg_no),r.reg_no ";
                }
                else if (ddltyp.SelectedItem.Value == "4")
                {
                    // query = "select COUNT(ea.roll_no)as total,d.degree_code,r.batch_year,c.Course_Name ,dt.Dept_Name as department ,r.Current_Semester from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code  and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month =" + ex_month + " and Exam_year =" + year + " and Exam_type=4  group by r.batch_year ,d.degree_code ,Course_Name,Dept_Name,r.Current_Semester";
                    query = "select r.Reg_No,r.roll_no ,r.Stud_Name from exam_application ea ,Exam_Details ed,Degree d,Department dt,course c ,Registration r where ea.exam_code =ed.exam_code and d.Degree_Code =ed.degree_code and d.Course_Id =c.Course_Id and d.Dept_Code =dt.Dept_Code and r.Roll_No =ea.roll_no and Exam_Month ='" + month + "' and Exam_year ='" + year + "' and r.degree_code in('" + degreecode + "')  and Exam_type=4  order by len(r.reg_no),r.reg_no ";

                }
                ds = da.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString(ds.Tables[0].Rows[s]["Reg_No"]);
                        dr[1] = Convert.ToString(ds.Tables[0].Rows[s]["Stud_Name"]);
                        dr[2] = Convert.ToString(ds.Tables[0].Rows[s]["roll_no"]);
                        dt.Rows.Add(dr);
                        addvalue.Add(dt);
                    }

                }
                if (dt.Rows.Count > 0)
                {
                    gridviewrow.Visible = true;
                    butgen.Visible = true;
                    lblty.Visible = true;
                    ddltyp.Visible = true;
                    gridviewrow.DataSource = dt;
                    gridviewrow.DataBind();
                }
                else
                {
                    gridviewrow.Visible = false;
                    butgen.Visible = false;

                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
                }
            }

        }
        catch
        {
        }
    }
    protected void cbselectall_change(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkBoxHeader = (CheckBox)gridviewrow.HeaderRow.FindControl("cbselectall");
            foreach (GridViewRow row in gridviewrow.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("cbSelect");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                    // gridview1.Visible = false;
                    // gridviewrow.Visible = false;

                }
            }

        }
        catch
        {
        }
    }

    protected void OnDataBound(object sender, EventArgs e)
    {
        for (int i = gridview1.Rows.Count - 1; i > 0; i--)
        {
            GridViewRow row = gridview1.Rows[i];
            GridViewRow previousRow = gridview1.Rows[i - 1];
            for (int j = 1; j <= 2; j++)
            {
                if (j == 2)
                {
                    Label lnlname = (Label)row.FindControl("gd_degcr");
                    Label lnlname1 = (Label)previousRow.FindControl("gd_degcr");

                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
                if (j == 1)
                {
                    Label lnlname = (Label)row.FindControl("gd_yr");
                    Label lnlname1 = (Label)previousRow.FindControl("gd_yr");

                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }

    }
}
