using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.UI.HtmlControls;
using System.Configuration;

public partial class StudentMod_AlumniReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataTable data = new DataTable();
    string user_code = "";
    string college_code = "";

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
            user_code = Session["usercode"].ToString();
            college_code = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                bindtype();
                bindedulevel();
                degree();
                bindbranch();
                btn_pdf.Visible = false;
                txt_todate.Attributes.Add("readonly", "readonly");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_fromdate.Text = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));
                txt_todate.Text = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));
            }
            if (cbdegreewise.Checked == true)
            {
                txt_degree.Enabled = true;
                txt_department.Enabled = true;
            }
            else
            {
                txt_degree.Enabled = false;
                txt_department.Enabled = false;
            }
        }
        catch (Exception ex) { }
    }

    public void degree()
    {
        try
        {

            cbldegree.Items.Clear();
            if (ddledulevel.SelectedItem.Text == "Both")
            {
                ds = d2.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where     course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and course.type='" + ddltype.SelectedItem.Text + "'", "Text");
            }
            else
            {
                ds = d2.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where     course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and course.type='" + ddltype.SelectedItem.Text + "' and course.Edu_Level='" + ddledulevel.SelectedItem.Value + "'", "Text");
            }
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                cbldegree.DataSource = ds;
                cbldegree.DataTextField = "course_name";
                cbldegree.DataValueField = "course_id";
                cbldegree.DataBind();

            }
            if (cbldegree.Items.Count > 0)
            {
                int count11 = 0;
                cbdegree.Checked = true;
                for (int j = 0; j < cbldegree.Items.Count; j++)
                {
                    count11++;
                    cbldegree.Items[j].Selected = true;
                }
                txt_degree.Text = "Degree(" + count11 + ")";
            }
            else
            {
                cbldegree.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            string commname = "";
            string branch = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                if (cbldegree.Items.Count > 0)
                {
                    for (int i = 0; i < cbldegree.Items.Count; i++)
                    {
                        if (cbldegree.Items[i].Selected == true)
                        {
                            if (branch == "")
                            {
                                branch = cbldegree.Items[i].Value;
                            }
                            else
                            {
                                branch = branch + "'" + "," + "'" + cbldegree.Items[i].Value;
                            }

                        }
                    }
                }
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code";
            }
            {
                ds = d2.select_method_wo_parameter(commname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldepartment.DataSource = ds;
                    cbldepartment.DataTextField = "dept_name";
                    cbldepartment.DataValueField = "degree_code";
                    cbldepartment.DataBind();
                }
                if (cbldepartment.Items.Count > 0)
                {
                    int count11 = 0;
                    cbdepartment1.Checked = true;
                    for (int j = 0; j < cbldepartment.Items.Count; j++)
                    {
                        count11++;
                        cbldepartment.Items[j].Selected = true;
                    }
                    txt_department.Text = "Department(" + count11 + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Click(object sender, EventArgs e)
    {
        try
        {
            string degree = "";
            if (txt_department.Enabled == true)
            {
                for (int i = 0; i < cbldepartment.Items.Count; i++)
                {
                    if (cbldepartment.Items[i].Selected == true)
                    {
                        if (degree == "")
                        {
                            degree = cbldepartment.Items[i].Value;
                        }
                        else
                        {
                            degree = degree + "'" + "," + "'" + cbldepartment.Items[i].Value;
                        }
                    }
                }
            }

            string[] Fromdate = txt_fromdate.Text.ToString().Split('/');
            string Fdate = Fromdate[1] + "/" + Fromdate[0] + "/" + Fromdate[2];

            string[] Todate = txt_todate.Text.ToString().Split('/');
            string Tdate = Todate[1] + "/" + Todate[0] + "/" + Todate[2];

            string type = ddltype.SelectedItem.Text;
            string eduleve = ddledulevel.SelectedItem.Text;

            if (cbapply.Checked == true)
            {
                string query = "";
                if (degree == "")
                {
                    if (ddledulevel.SelectedItem.Text == "Both")
                    {
                        query = "select a.stud_name,r.Reg_No,(c.Course_Name+'-'+dt.Dept_Name)as dept,CONVERT(varchar(10),a.dob,103) as dob,r.Batch_Year ,StuPer_Id,Student_Mobile from  applyn a,Registration r,Degree d,Department dt,course c where a.app_no =r.App_No and a.degree_code=d.Degree_Code and d.Degree_Code=r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id=d.Course_Id and a.college_code=r.college_code    and CC=1 and Exam_Flag ='OK' and DelFlag=0 and isalumni='1' and c.type='" + type + "'  and AlumnregisterDate between '" + Fdate.ToString() + "' and '" + Tdate.ToString() + "'  order by case sex when 2 then -1 else sex end desc ";
                    }
                    else
                    {
                        query = "select a.stud_name,r.Reg_No,(c.Course_Name+'-'+dt.Dept_Name)as dept,CONVERT(varchar(10),a.dob,103) as dob,r.Batch_Year ,StuPer_Id,Student_Mobile from  applyn a,Registration r,Degree d,Department dt,course c where a.app_no =r.App_No and a.degree_code=d.Degree_Code and d.Degree_Code=r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id=d.Course_Id and a.college_code=r.college_code    and CC=1 and Exam_Flag ='OK' and DelFlag=0 and isalumni='1' and c.type='" + type + "' and c.Edu_Level ='" + eduleve + "' and AlumnregisterDate between '" + Fdate.ToString() + "' and '" + Tdate.ToString() + "'  order by case sex when 2 then -1 else sex end desc ";
                    }
                }
                else
                {
                    if (ddledulevel.SelectedItem.Text == "Both")
                    {
                        query = "select a.stud_name,r.Reg_No,(c.Course_Name+'-'+dt.Dept_Name)as dept,CONVERT(varchar(10),a.dob,103) as dob,r.Batch_Year ,StuPer_Id,Student_Mobile from  applyn a,Registration r,Degree d,Department dt,course c where a.app_no =r.App_No and a.degree_code=d.Degree_Code and d.Degree_Code=r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id=d.Course_Id and a.college_code=r.college_code    and CC=1 and Exam_Flag ='OK' and DelFlag=0 and isalumni='1' and a.degree_code=r.degree_code and r.degree_code in ('" + degree + "') and c.type='" + type + "' and AlumnregisterDate between '" + Fdate.ToString() + "' and '" + Tdate.ToString() + "'  order by case sex when 2 then -1 else sex end desc ";
                    }
                    else
                    {
                        query = "select a.stud_name,r.Reg_No,(c.Course_Name+'-'+dt.Dept_Name)as dept,CONVERT(varchar(10),a.dob,103) as dob,r.Batch_Year ,StuPer_Id,Student_Mobile from  applyn a,Registration r,Degree d,Department dt,course c where a.app_no =r.App_No and a.degree_code=d.Degree_Code and d.Degree_Code=r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id=d.Course_Id and a.college_code=r.college_code    and CC=1 and Exam_Flag ='OK' and DelFlag=0 and isalumni='1' and a.degree_code=r.degree_code and r.degree_code in ('" + degree + "') and c.type='" + type + "' and c.Edu_Level ='" + eduleve + "' and AlumnregisterDate between '" + Fdate.ToString() + "' and '" + Tdate.ToString() + "'  order by case sex when 2 then -1 else sex end desc ";
                    }

                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    data.Columns.Add("S.No", typeof(string));
                    data.Columns.Add("Reg.No", typeof(string));
                    data.Columns.Add("Student Name", typeof(string));
                    data.Columns.Add("Class & Group", typeof(string));
                    data.Columns.Add("Remarks", typeof(string));
                    int sno = 0;
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        sno++;
                        data.Rows.Add(sno, ds.Tables[0].Rows[j]["Reg_No"], ds.Tables[0].Rows[j]["stud_name"], ds.Tables[0].Rows[j]["dept"], "");
                    }

                    if (data.Rows.Count > 0)
                    {
                        Showgrid.DataSource = data;
                        Showgrid.DataBind();
                        Showgrid.Visible = true;
                        btn_pdf.Visible = true;
                        btnexcel.Visible = true;
                        if (Showgrid.Rows.Count > 0)
                        {
                            for (int row = 0; row < Showgrid.Rows.Count; row++)
                            {
                                Showgrid.Rows[row].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    else
                    {
                        Showgrid.Visible = false;
                        btn_pdf.Visible = false;
                        btnexcel.Visible = false;
                    }
                }
                else
                {
                    Showgrid.Visible = false;
                    btn_pdf.Visible = false;
                    btnexcel.Visible = false;
                }

            }
            else if (cbnotapply.Checked == true)
            {
                string query = "";
                if (degree == "")
                {
                    if (ddledulevel.SelectedItem.Text == "Both")
                    {
                        query = "select a.stud_name,r.Reg_No,(c.Course_Name+'-'+dt.Dept_Name)as dept,CONVERT(varchar(10),a.dob,103) as dob,r.Batch_Year ,StuPer_Id,Student_Mobile from  applyn a,Registration r,Degree d,Department dt,course c where a.app_no =r.App_No and a.degree_code=d.Degree_Code and d.Degree_Code=r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id=d.Course_Id and a.college_code=r.college_code    and CC=1 and Exam_Flag ='OK' and DelFlag=0 and isalumni='0' and c.type='" + ddltype.SelectedItem.Text + "' and AlumnregisterDate between '" + Fdate.ToString() + "' and '" + Tdate.ToString() + "'  order by case sex when 2 then -1 else sex end desc";
                    }
                    else
                    {
                        query = "select a.stud_name,r.Reg_No,(c.Course_Name+'-'+dt.Dept_Name)as dept,CONVERT(varchar(10),a.dob,103) as dob,r.Batch_Year ,StuPer_Id,Student_Mobile from  applyn a,Registration r,Degree d,Department dt,course c where a.app_no =r.App_No and a.degree_code=d.Degree_Code and d.Degree_Code=r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id=d.Course_Id and a.college_code=r.college_code    and CC=1 and Exam_Flag ='OK' and DelFlag=0 and isalumni='0' and c.type='" + ddltype.SelectedItem.Text + "'  and c.Edu_Level='" + ddledulevel.SelectedItem.Text + "' and AlumnregisterDate between '" + Fdate.ToString() + "' and '" + Tdate.ToString() + "' order by case sex when 2 then -1 else sex end desc";
                    }
                }
                else
                {
                    if (ddledulevel.SelectedItem.Text == "Both")
                    {
                        query = "select a.stud_name,r.Reg_No,(c.Course_Name+'-'+dt.Dept_Name)as dept,CONVERT(varchar(10),a.dob,103) as dob,r.Batch_Year ,StuPer_Id,Student_Mobile from  applyn a,Registration r,Degree d,Department dt,course c where a.app_no =r.App_No and a.degree_code=d.Degree_Code and d.Degree_Code=r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id=d.Course_Id and a.college_code=r.college_code    and CC=1 and Exam_Flag ='OK' and DelFlag=0 and isalumni='0' and a.degree_code=r.degree_code and r.degree_code in ('" + degree + "') and c.type='" + ddltype.SelectedItem.Text + "' and AlumnregisterDate between '" + Fdate.ToString() + "' and '" + Tdate.ToString() + "'  order by case sex when 2 then -1 else sex end desc";
                    }
                    else
                    {
                        query = "select a.stud_name,r.Reg_No,(c.Course_Name+'-'+dt.Dept_Name)as dept,CONVERT(varchar(10),a.dob,103) as dob,r.Batch_Year ,StuPer_Id,Student_Mobile from  applyn a,Registration r,Degree d,Department dt,course c where a.app_no =r.App_No and a.degree_code=d.Degree_Code and d.Degree_Code=r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id=d.Course_Id and a.college_code=r.college_code    and CC=1 and Exam_Flag ='OK' and DelFlag=0 and isalumni='0' and a.degree_code=r.degree_code and r.degree_code in ('" + degree + "') and c.type='" + ddltype.SelectedItem.Text + "' and c.Edu_Level='" + ddledulevel.SelectedItem.Text + "' and AlumnregisterDate between '" + Fdate.ToString() + "' and '" + Tdate.ToString() + "'  order by case sex when 2 then -1 else sex end desc";
                    }
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    data.Columns.Add("S.No", typeof(string));
                    data.Columns.Add("Reg.No", typeof(string));
                    data.Columns.Add("Student Name", typeof(string));
                    data.Columns.Add("Class & Group", typeof(string));
                    data.Columns.Add("Remarks", typeof(string));
                    int sno = 0;
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        sno++;
                        data.Rows.Add(sno, ds.Tables[0].Rows[j]["Reg_No"], ds.Tables[0].Rows[j]["stud_name"], ds.Tables[0].Rows[j]["dept"], "");
                    }

                    if (data.Rows.Count > 0)
                    {
                        Showgrid.DataSource = data;
                        Showgrid.DataBind();
                        Showgrid.Visible = true;
                        btn_pdf.Visible = true;
                        btnexcel.Visible = true;
                        if (Showgrid.Rows.Count > 0)
                        {
                            for (int row = 0; row < Showgrid.Rows.Count; row++)
                            {
                                Showgrid.Rows[row].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    else
                    {
                        Showgrid.Visible = false;
                        btn_pdf.Visible = false;
                        btnexcel.Visible = false;
                    }
                }
                else
                {
                    Showgrid.Visible = false;
                    btn_pdf.Visible = false;
                    btnexcel.Visible = false;
                }
            }
        }
        catch
        {

        }

    }

    protected void cbdegree_Changed(object sender, EventArgs e)
    {
        try
        {


            if (cbdegree.Checked == true)
            {
                for (int i = 0; i < cbldegree.Items.Count; i++)
                {

                    cbldegree.Items[i].Selected = true;
                    txt_degree.Text = "Degree(" + (cbldegree.Items.Count) + ")";
                }
                bindbranch();

            }
            else
            {
                for (int i = 0; i < cbldegree.Items.Count; i++)
                {
                    cbldegree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                }
            }
        }
        catch
        {

        }
    }

    protected void cbldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int seatcount = 0;
            cbdegree.Checked = false;
            for (int i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
                bindbranch();
            }
            txt_degree.Text = "Degree(" + seatcount.ToString() + ")";

        }
        catch
        {

        }
    }

    protected void cbdepartment_Changed(object sender, EventArgs e)
    {
        try
        {

            if (cbdepartment1.Checked == true)
            {
                for (int i = 0; i < cbldepartment.Items.Count; i++)
                {

                    cbldepartment.Items[i].Selected = true;
                    txt_department.Text = "Department(" + (cbldepartment.Items.Count) + ")";
                }

            }
            else
            {

                for (int i = 0; i < cbldepartment.Items.Count; i++)
                {
                    cbldepartment.Items[i].Selected = false;
                    txt_department.Text = "--Select--";
                }
            }
        }
        catch
        {

        }
    }

    protected void cbldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cbdepartment1.Checked = false;
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                if (cbldepartment.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            txt_department.Text = "Department(" + seatcount.ToString() + ")";

        }
        catch
        {

        }
    }

    protected void lnk_logout(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch
        {

        }
    }
    protected void pdf_Click(object sender, EventArgs e)
    {
        try
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=UserDetails.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Showgrid.AllowPaging = false;
            Showgrid.HeaderRow.Style.Add("width", "50px");
            //Showgrid.HeaderRow.Style.Add("font-size", "10px");
            Showgrid.HeaderRow.Style.Add("text-align", "center");
            //Showgrid.Style.Add("text-decoration", "none");
            //Showgrid.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            //Showgrid.Style.Add("font-size", "8px");
            Showgrid.Style.Add("text-align", "center");
            Showgrid.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            Paragraph p = new Paragraph();
            p.Alignment = Element.ALIGN_CENTER;
            Font fontNormal = new Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));
            // Font font = new Font(Font.HELVETICA, 11, new Color(255, 0, 0));
            string tx = "\n";
            string txt = "Madras Christian College(Autonomous) \n";
            string txt1 = "";

            if (cbapply.Checked == true)
            {
                txt1 = "List of Graduates Attending the Graduation Day";
            }
            else
            {
                txt1 = "List of Graduates Not Attending the Graduation Day";
            }
            string tx11 = "\n \n";

            p.Add(txt);
            p.Add(tx);
            p.Add(txt1);
            p.Add(tx11);
            pdfDoc.Open();
            pdfDoc.Add(p);
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch
        {
        }

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */

    }

    protected void btn_excelClcik(object sender, EventArgs e)
    {
        try
        {
            string attachment = "attachment; filename=Export.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            HtmlForm frm = new HtmlForm();
            Showgrid.Parent.Controls.Add(frm);
            frm.Attributes["runat"] = "server";
            frm.Controls.Add(Showgrid);
            frm.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
        catch
        {

        }
    }

    public void bindtype()
    {
        try
        {
            string typequery = "select distinct type  from course where college_code =" + Session["collegecode"].ToString() + "";
            ds = d2.select_method_wo_parameter(typequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataBind();

            }

        }
        catch
        {

        }
    }

    public void bindedulevel()
    {
        string query = "select distinct Edu_Level  from course where type='" + ddltype.SelectedItem.Text + "' and college_code=" + Session["collegecode"].ToString() + " order by Edu_Level desc";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddledulevel.DataSource = ds;
            ddledulevel.DataTextField = "Edu_Level";
            ddledulevel.DataBind();
            if (ddledulevel.Items.Count > 1)
            {
                ddledulevel.Items.Insert(0, "Both");
            }
        }
    }
    protected void type_Change(object sender, EventArgs e)
    {
        try
        {
            bindedulevel();
            degree();
            bindbranch();
        }
        catch
        {
        }
    }

    protected void edulevel_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            degree();
            bindbranch();
        }
        catch
        {
        }
    }

}