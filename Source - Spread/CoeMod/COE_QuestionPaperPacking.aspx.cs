using System;
using System.IO;
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
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using FarPoint.Web.Spread;
using System.Text;
using Gios.Pdf;

public partial class CoeMod_COE_QuestionPaperPacking : System.Web.UI.Page
{
    string CollegeCode;

    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsss = new DataSet();
    Hashtable hashall = new Hashtable();
    Boolean flag_true = false;
    Boolean saveflag = false;

    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
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

            CollegeCode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {

                year1();
                loadtype();
                btnView1.Visible = false;
                CheckBox1.Checked = true;
                fpspread.Sheets[0].RowCount = 0;
                fpspread.Sheets[0].RowHeader.Visible = false;
                fpspread.CommandBar.Visible = false;

                fpspread1.Visible = false;
                fpspread.Sheets[0].ColumnCount = 6;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                fpspread.Sheets[0].Columns[0].Width = 60;
                fpspread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[0].Locked = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Hall No";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                fpspread.Sheets[0].Columns[1].Locked = true;

                fpspread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

                fpspread.Sheets[0].Columns[1].Width = 100;



                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree Details";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                fpspread.Sheets[0].Columns[2].Locked = true;
                fpspread.Sheets[0].Columns[2].Width = 260;
                fpspread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;

                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Subject Code And Name";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                fpspread.Sheets[0].Columns[3].Locked = true;
                fpspread.Sheets[0].Columns[3].Width = 280;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Student";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Select";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                fpspread.Sheets[0].Columns[4].Width = 140;
                fpspread.Sheets[0].Columns[5].Width = 80;
                fpspread.Sheets[0].Columns[4].Locked = true;

                fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                fpspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                fpspread.Visible = false;
            }
        }
        catch(Exception ex)
        {
        }
    }
    protected void CheckBox1_click(object sender, EventArgs e)
    {
        if (CheckBox1.Checked == true)
        {

            CheckBox2.Checked = false;
            btnView.Visible = true;
            btnView1.Visible = false;
            fpspread1.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            lblnorec.Visible = false;
            Printcontrol.Visible = false;
            year1();
            month1();
            date();
            secss();
            halll();



        }
    }
    protected void CheckBox2_click(object sender, EventArgs e)
    {
        if (CheckBox2.Checked == true)
        {
            CheckBox1.Checked = false;
            btnView.Visible = false;
            btnView1.Visible = true;
            btngenerate.Visible = false;
            fpspread.Visible = false;

            year1();
            month1();
            date();
            secss();
            halll();
        }
    }
    public void year1()
    {
        ddlYear.Items.Clear();
        dsss.Clear();
        dsss = da.Examyear();

        if (dsss.Tables[0].Rows.Count > 0)
        {
            ddlYear.DataSource = dsss;
            ddlYear.DataTextField = "Exam_year";
            ddlYear.DataValueField = "Exam_year";
            ddlYear.DataBind();
        }
        if (CheckBox2.Checked == true)
        {
        }
        else
        {
            ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
        }
    }
    protected void month1()
    {
        try
        {
            ddlMonth.Items.Clear();
            dsss.Clear();
            string year1 = ddlYear.SelectedValue;
            dsss = da.Exammonth(year1);
            if (dsss.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = dsss;
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
            }
            //if (CheckBox2.Checked == true)
            //{
            //}
            //else
            //{
            ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
            //}
        }
        catch
        {
        }

    }
    public void loadtype()
    {
        try
        {
            ddltype.Items.Clear();
            string strtypequery = "select distinct type from course where isnull(type,'')>''";
            DataSet dstype = da.select_method_wo_parameter(strtypequery, "text");
            if (dstype.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = dstype;
                ddltype.DataTextField = "type";
                ddltype.DataBind();

                ddltype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("ALL", "ALL"));
            }
            else
            {
                ddltype.Enabled = false;
            }
        }
        catch
        {
        }
    }
    protected void date()
    {
        try
        {
            ddlDate.Items.Clear();
            dsss.Clear();


            string strtype = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim() != "" && ddltype.SelectedItem.ToString().Trim() != "ALL")
                {
                    strtype = "and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtype = "and c.type in('Day','MCA')";
                }
            }

            //string s = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,et.exam_date from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' order by et.exam_date";
            string s = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,et.exam_date from exmtt_det et,exmtt e,Degree d,Course c where et.exam_code=e.exam_code and d.Degree_Code=e.degree_code and d.Course_Id=c.Course_Id and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' " + strtype + " order by et.exam_date";
            dsss = da.select_method_wo_parameter(s, "txt");
            if (dsss.Tables[0].Rows.Count > 0)
            {
                ddlDate.DataSource = dsss;
                ddlDate.DataTextField = "ExamDate";
                ddlDate.DataValueField = "ExamDate";
                ddlDate.DataBind();
            }
            //if (CheckBox2.Checked == true)
            //{
            //}
            //else
            //{
            ddlDate.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
            if (dsss.Tables[0].Rows.Count > 0)
            {
                ddlDate.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "1"));
            }
            // }
        }
        catch
        {
        }
    }
    protected void secss()
    {
        try
        {
            string date = "";
            if (CheckBox2.Checked == true)
            {
            }
            else if (ddlDate.SelectedItem.Text == "All")
            {
                date = "";
            }
            else
            {
                string datee = ddlDate.SelectedValue.ToString();
                string[] dd = datee.Split('-');
                datee = dd[2].ToString() + "-" + dd[1].ToString() + "-" + dd[0].ToString();
                date = "and et.exam_date='" + datee + "'";
            }

            string strtype = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim() != "" && ddltype.SelectedItem.ToString().Trim() != "ALL")
                {
                    strtype = "and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtype = "and c.type in('Day','MCA')";
                }
            }
            ddlSession.Items.Clear();
            dsss.Clear();
            //string s = "select distinct  et.exam_session as exam_session from exmtt_det et,exmtt e,exam_seating es where es.subject_no=et.subject_no and et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'" + date + "";
            string s = "select distinct  et.exam_session as exam_session from exmtt_det et,exmtt e,exam_seating es where es.subject_no=et.subject_no and et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'" + date + "";
            dsss = da.select_method_wo_parameter(s, "txt");
            if (dsss.Tables[0].Rows.Count > 0)
            {
                ddlSession.DataSource = dsss;
                ddlSession.DataTextField = "exam_session";
                ddlSession.DataValueField = "exam_session";
                ddlSession.DataBind();
            }
            if (CheckBox2.Checked == true)
            {
            }
            else
            {

                ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
                if (dsss.Tables[0].Rows.Count > 0)
                {
                    ddlSession.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "1"));
                }
            }
        }
        catch
        {
        }
    }
    protected void halll()
    {
        try
        {
            string date = "";
            string sessn = "";
            if (CheckBox2.Checked == true)
            {
            }
            else if (ddlDate.SelectedItem.Text == "All")
            {
                date = "";
            }
            else
            {
                string datee = ddlDate.SelectedValue.ToString();
                string[] dd = datee.Split('-');
                datee = dd[2].ToString() + "-" + dd[1].ToString() + "-" + dd[0].ToString();
                date = "and et.exam_date='" + datee + "'";
            }
            if (CheckBox2.Checked == true)
            {
            }

            else if (ddlSession.SelectedItem.Text == "All")
            {
                sessn = "";
            }
            else
            {
                string datee1 = ddlSession.SelectedValue.ToString();
                sessn = "and et.exam_session='" + datee1 + "'";
            }
            string strtype = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim() != "" && ddltype.SelectedItem.ToString().Trim() != "ALL")
                {
                    strtype = "and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtype = "and c.type in('Day','MCA')";
                }
            }
            ddlhall.Items.Clear();
            dsss.Clear();
            // string s = "select distinct  es.roomno as roomno from exmtt_det et,exmtt e,exam_seating es where es.subject_no=et.subject_no and et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'" + date + "" + sessn + "";
            string s = "select distinct  es.roomno as roomno from exmtt_det et,exmtt e,exam_seating es,Degree d,Course c where es.subject_no=et.subject_no and et.exam_code=e.exam_code and d.Degree_Code=e.degree_code and d.Course_Id=c.Course_Id " + strtype + " and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'" + date + "" + sessn + "";
            dsss = da.select_method_wo_parameter(s, "txt");
            if (dsss.Tables[0].Rows.Count > 0)
            {
                ddlhall.DataSource = dsss;
                ddlhall.DataTextField = "roomno";
                ddlhall.DataValueField = "roomno";
                ddlhall.DataBind();
            }

            ddlhall.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
            if (dsss.Tables[0].Rows.Count > 0)
            {
                ddlhall.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "1"));
            }

        }
        catch
        {
        }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btngenerate.Visible = false;
            fpspread.Visible = false;
            date();
            secss();
            if (CheckBox2.Checked == true)
            {
                fpspread1.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                lblerror.Visible = false;
            }
        }
        catch
        {
        }


    }
    //public void loadhall()
    //{
    //    try
    //    {
    //        hashall.Clear();
    //        hashall.Add("ExamMonth", ddlMonth.SelectedItem.Value.ToString());
    //        hashall.Add("ExamYear", ddlYear.SelectedItem.Text.ToString());
    //        string date11 = ddlDate.SelectedItem.Text.ToString();
    //        //string[] datesplit = date11.Split('-');
    //        //if (datesplit.GetUpperBound(0)>1)
    //        //{
    //        //date11 = datesplit[2] + "-" + datesplit[1] + "-" + datesplit[0];
    //        //}
    //        hashall.Add("Date", date11);
    //        hashall.Add("Session", ddlSession.SelectedItem.Text.ToString());
    //        ds = da.select_method("ProcExamSeatingHallDetails", hashall, "sp");
    //        ArrayList arry_hallno = new ArrayList();
    //        ddlhall.Items.Clear();
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlhall.Enabled = true;
    //            btnView.Enabled = true;
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                if (!arry_hallno.Contains(ds.Tables[0].Rows[i]["HallNo"]))
    //                {
    //                    arry_hallno.Add(ds.Tables[0].Rows[i]["HallNo"]);
    //                    ddlhall.Items.Insert(0, Convert.ToString(ds.Tables[0].Rows[i]["HallNo"]));
    //                    // ddlhall.Items.Add(ds.Tables[0].Rows[i]["HallNo"]);
    //                }
    //            }
    //            ddlhall.Items.Insert(0, "All");
    //        }
    //        else
    //        {
    //            ddlhall.Enabled = false;
    //            btnView.Enabled = false;
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btngenerate.Visible = false;
            fpspread.Visible = false;
            month1();
            if (CheckBox2.Checked == true)
            {
                fpspread1.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                lblerror.Visible = false;
            }
        }
        catch
        {
        }

    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btngenerate.Visible = false;
            fpspread.Visible = false;
            date();
            secss();
            halll();
            if (CheckBox2.Checked == true)
            {
                fpspread1.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                lblerror.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void ddldate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btngenerate.Visible = false;
            fpspread.Visible = false;
            secss();
            halll();
            if (CheckBox2.Checked == true)
            {
                fpspread1.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                lblerror.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void ddlhall_SelectedIndexChanged(object sender, EventArgs e)
    {
        fpspread.Visible = false;
        btngenerate.Visible = false;
        if (CheckBox2.Checked == true)
        {
            fpspread1.Visible = false;
            btnExcel.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            lblerror.Visible = false;
        }
    }
    protected void ddlsession_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btngenerate.Visible = false;
            fpspread.Visible = false;
            halll();
            if (CheckBox2.Checked == true)
            {
                fpspread1.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                lblerror.Visible = false;
            }
        }
        catch
        {
        }

    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {

            fpspread1.Visible = false;
            fpspread.Sheets[0].RowCount = 0;


            fpspread.Sheets[0].RowCount = 0;
            fpspread.Sheets[0].RowHeader.Visible = false;
            fpspread.CommandBar.Visible = false;

            fpspread1.Visible = false;
            fpspread.Sheets[0].ColumnCount = 8;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpspread.Sheets[0].Columns[0].Width = 60;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Exam Date";
            fpspread.Sheets[0].Columns[1].Width = 80;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Exam Session";
            fpspread.Sheets[0].Columns[2].Width = 80;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Hall No";
            fpspread.Sheets[0].Columns[3].Width = 100;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree Details";
            fpspread.Sheets[0].Columns[4].Width = 260;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Code And Name";
            fpspread.Sheets[0].Columns[5].Width = 280;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total Student";
            fpspread.Sheets[0].Columns[6].Width = 50;
            fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Select";
            fpspread.Sheets[0].Columns[7].Width = 80;

            fpspread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            fpspread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;

            fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            fpspread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            fpspread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;

            fpspread.Sheets[0].Columns[2].Locked = true;
            fpspread.Sheets[0].Columns[1].Locked = true;
            fpspread.Sheets[0].Columns[3].Locked = true;
            fpspread.Sheets[0].Columns[4].Locked = true;
            fpspread.Sheets[0].Columns[5].Locked = true;
            fpspread.Sheets[0].Columns[6].Locked = true;
            fpspread.Sheets[0].Columns[4].Visible = false;

            string exammonth1 = ddlMonth.SelectedItem.Value.ToString();
            string ExamYear = ddlYear.SelectedItem.Text.ToString();
            string examdate11 = ddlDate.SelectedItem.Text.ToString();
            string[] examdatesplit = examdate11.Split('-');
            if (examdatesplit.GetUpperBound(0) > 1)
            {
                examdate11 = examdatesplit[1] + "/" + examdatesplit[0] + "/" + examdatesplit[2];

                examdate11 = "and es.edate='" + examdate11 + "'";
            }
            else
            {
                examdate11 = "";

            }
            string Session = ddlSession.SelectedItem.Text.ToString();
            if (Session.Trim() == "All")
            {
                Session = "";
            }
            else
            {
                Session = "and es.ses_sion='" + Session + "'";
            }
            string hallno = ddlhall.SelectedItem.Text.ToString();
            if (hallno.Trim() == "All")
            {
                hallno = "";

            }
            else
            {
                hallno = "and  es.roomno='" + hallno + "' ";
            }

            string strtype = "";
            if (ddltype.Items.Count > 0 && ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.ToString().Trim() != "" && ddltype.SelectedItem.ToString().Trim() != "ALL")
                {
                    strtype = "and c.type='" + ddltype.SelectedItem.ToString() + "'";
                }
                if (ddltype.SelectedItem.ToString().Trim().ToLower() == "day")
                {
                    strtype = "and c.type in('Day','MCA')";
                }
            }

            //string strsql = "select count( distinct es.regno) as totstu, es.roomno,s.subject_code,s.subject_name,CONVERT(varchar(50),et.exam_date,105)as exam_date,e.Exam_Month,e.Exam_year,e.batch_year,e.current_semester,et.exam_session, c.Course_Name,d.Degree_Code,d.Acronym,de.Dept_Name,s.subject_no from Exam_Details e,subject s,exmtt_det et,  Degree d,Department de,course c,exam_seating es  where s.subject_no=et.subject_no and e.degree_code=d.Degree_Code   and d.Dept_Code=de.Dept_Code   and d.Course_Id=c.Course_Id  and d.Degree_Code=es.degree_code " + examdate11 + "  and e.Exam_Month='" + exammonth1 + "' and e.Exam_year='" + ExamYear + "' " + Session + " " + hallno + " and et.exam_date=es.edate and et.subject_no=es.subject_no and et.exam_session=es.ses_sion group by s.subject_code,s.subject_name,exam_date,e.Exam_Month, e.Exam_year,e.batch_year,e.current_semester,et.exam_session, c.Course_Name,d.Degree_Code,d.Acronym,de.Dept_Name,es.roomno,de.Dept_Name,s.subject_no";
            // string strsql = "select et.exam_date,CONVERT(varchar(50),et.exam_date,105)as examdate,et.exam_session,es.roomno,c.Course_Name,de.Dept_Name,e.degree_code,s.subject_name,s.subject_code,count(es.regno) as totstu from exmtt e,exmtt_det et,exam_seating es,subject s,sub_sem ss,Degree d,Course c,Department de where e.exam_code=et.exam_code and et.subject_no=es.subject_no and et.exam_date=es.edate and et.exam_session=es.ses_sion and et.subject_no=s.subject_no and es.subject_no=s.subject_no and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and s.subType_no=ss.subType_no and e.Exam_Month='" + exammonth1 + "' and e.Exam_year='" + ExamYear + "' " + examdate11 + "  " + Session + " " + hallno + " group by et.exam_date,et.exam_session,es.roomno,c.Course_Name,de.Dept_Name,s.subject_name,s.subject_code ,e.degree_code order by et.exam_date,et.exam_session desc,es.roomno,totstu desc,s.subject_name,s.subject_code";
            string strsql = "select et.exam_date,CONVERT(varchar(50),et.exam_date,105)as examdate,et.exam_session,es.roomno,s.subject_name,s.subject_code,count(es.regno) as totstu from exmtt e,exmtt_det et,exam_seating es,subject s,sub_sem ss,Degree d,Course c,Department de where e.exam_code=et.exam_code and et.subject_no=es.subject_no and et.exam_date=es.edate and et.exam_session=es.ses_sion and et.subject_no=s.subject_no and es.subject_no=s.subject_no and e.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and s.subType_no=ss.subType_no and e.Exam_Month='" + exammonth1 + "' and e.Exam_year='" + ExamYear + "' " + examdate11 + "  " + Session + " " + hallno + " " + strtype + " group by et.exam_date,et.exam_session,es.roomno,s.subject_name,s.subject_code order by et.exam_date,et.exam_session desc,es.roomno,totstu desc,s.subject_name,s.subject_code";

            ds = da.select_method_wo_parameter(strsql, "Text");

            FarPoint.Web.Spread.CheckBoxCellType cheall = new FarPoint.Web.Spread.CheckBoxCellType();

            FarPoint.Web.Spread.CheckBoxCellType cheselectall = new FarPoint.Web.Spread.CheckBoxCellType();
            cheselectall.AutoPostBack = true;

            int sno = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, fpspread.Sheets[0].ColumnCount - 1].CellType = cheselectall;
                fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 7);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    fpspread.Sheets[0].RowCount = fpspread.Sheets[0].RowCount + 1;
                    // string degreedetails = ds.Tables[0].Rows[i]["Course_Name"] + "-" + ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                    string fphallno = Convert.ToString(ds.Tables[0].Rows[i]["roomno"]);
                    string fpsubject = ds.Tables[0].Rows[i]["subject_code"] + " - " + Convert.ToString(ds.Tables[0].Rows[i]["subject_name"]);
                    string fptot_stud = Convert.ToString(ds.Tables[0].Rows[i]["totstu"]);
                    string date = Convert.ToString(ds.Tables[0].Rows[i]["exam_date"]);
                    string edate = Convert.ToString(ds.Tables[0].Rows[i]["examdate"]);
                    string sessnn = Convert.ToString(ds.Tables[0].Rows[i]["exam_session"]);
                    // string degcode = Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                    string scode = Convert.ToString(ds.Tables[0].Rows[i]["subject_code"]);

                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = edate;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Tag = date;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = sessnn;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = fphallno;
                    //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = degreedetails;
                    //fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Tag = degcode;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = fpsubject;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Tag = scode;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = fptot_stud;
                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = cheall;


                }
                fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                fpspread.Visible = true;
                btngenerate.Visible = true;
                lblerror.Text = "";
            }
            else
            {
                btngenerate.Visible = false;
                lblerror.Text = "No Records Found";
                fpspread.Visible = false;
                fpspread1.Visible = false;
            }
            fpspread.Width = 1000;
            fpspread.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            fpspread.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }
    protected void fpspread_OnUpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = e.CommandArgument.ToString();
            if (flag_true == false && actrow == "0")
            {
                for (int j = 1; j < Convert.ToInt16(fpspread.Sheets[0].RowCount); j++)
                {
                    string actcol = e.SheetView.ActiveColumn.ToString();
                    string seltext = e.EditValues[7].ToString();
                    if (seltext != "System.Object" && seltext != "Selector For All")
                    {
                        fpspread.Sheets[0].Cells[j, 7].Text = seltext.ToString();
                    }
                }
                flag_true = true;
            }
        }
        catch
        {
        }

    }
    protected void btngenerate_click(object sender, EventArgs e)
    {
        try
        {
            fpspread.SaveChanges();
            bindpdf();

        }
        catch
        {
        }
    }
    public void bindpdf()
    {
        try
        {
            fpspread.SaveChanges();
            Font Fontbold = new Font("Times New Roman", 18, FontStyle.Bold);
            Font Fontbold2 = new Font("Times New Roman", 30, FontStyle.Bold);
            Font Fontbold22 = new Font("Times New Roman", 26, FontStyle.Bold);
            Font Fontbold222 = new Font("Times New Roman", 22, FontStyle.Bold);

            string collegecode = Session["collegecode"].ToString();
            Gios.Pdf.PdfDocument myprovdoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);

            Gios.Pdf.PdfDocument myprovdoc1 = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);

            string clgquery = "select UPPER(collname)as collname,category,university,UPPER(address3) as address3, UPPER(district) as district,UPPER (state) as state,pincode,affliatedby from collinfo where college_code='" + collegecode + "'";
            ds = da.select_method_wo_parameter(clgquery, "Text");

            string examdate11 = "";
            if (ddlDate.SelectedItem.Text.ToString().Trim() != "All" && ddlDate.SelectedItem.Text.ToString().Trim() != "")
            {
                examdate11 = ddlDate.SelectedItem.Text.ToString();
                string[] examdatesplit = examdate11.Split('-');
                if (examdatesplit.GetUpperBound(0) > 1)
                {
                    examdate11 = examdatesplit[1] + "/" + examdatesplit[0] + "/" + examdatesplit[2];
                    examdate11 = "and es.edate='" + examdate11 + "'";
                }
            }

            string eSession = "";
            if (ddlSession.SelectedItem.Text.ToString().Trim() != "All" && ddlSession.SelectedItem.Text.ToString().Trim() != "")
            {
                eSession = "and es.ses_sion='" + ddlSession.SelectedItem.Text.ToString() + "'";
            }

            string data = "select es.edate,es.ses_sion,es.roomno,es.seat_no,r.Reg_No,ss.subject_type,s.subject_name,s.subject_code from exmtt e,exmtt_det et,exam_seating es,Registration r,subject s,sub_sem ss where e.exam_code=et.exam_code and et.subject_no=es.subject_no and et.exam_date=es.edate and et.exam_session=es.ses_sion and es.subject_no=s.subject_no ";
            data = data + " and et.subject_no=s.subject_no and r.Reg_No=es.regno and es.subject_no=s.subject_no and ss.subType_no=s.subType_no  and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' " + examdate11 + " " + eSession + " order by es.edate,es.ses_sion desc,es.roomno,es.seat_no";
            ds1 = da.select_method_wo_parameter(data, "Text");

            string subjectquery = "select distinct subject_name,subject_code from subject";
            DataSet dssubject = da.select_method_wo_parameter(subjectquery, "text");

            string equalsubject = "select * from tbl_equal_paper_Matching ";
            DataSet dvsubequal = da.select_method_wo_parameter(equalsubject, "Text");


            for (int i = 1; i < fpspread.Sheets[0].Rows.Count; i++)
            {

                int isval = 0;
                isval = Convert.ToInt32(fpspread.Sheets[0].Cells[i, 7].Value);
                if (isval == 1)
                {
                    saveflag = true;
                    Gios.Pdf.PdfPage myprov_pdfpage = myprovdoc.NewPage();
                    string clgname = ds.Tables[0].Rows[0]["collname"].ToString();
                    string category = ds.Tables[0].Rows[0]["category"].ToString().Trim().ToUpper();
                    string monthfyear = fpspread.Sheets[0].Cells[i, 1].Note.ToString();
                    string dateofexam = fpspread.Sheets[0].Cells[i, 1].Tag.ToString();
                    string subjectnamecode = fpspread.Sheets[0].Cells[i, 5].Text.ToString();
                    string hallno = fpspread.Sheets[0].Cells[i, 3].Text.ToString();
                    string sessd = fpspread.Sheets[0].Cells[i, 2].Text.ToString();
                    string subnos = fpspread.Sheets[0].Cells[i, 5].Tag.ToString();
                    string totalhall = "";
                    string overstng = "";
                    //   string degcode = fpspread.Sheets[0].Cells[i, 4].Tag.ToString();
                    //  string degreedetails = fpspread.Sheets[0].Cells[i, 4].Text.ToString();
                    string seatno = "";


                    string commsub = "";
                    dvsubequal.Tables[0].DefaultView.RowFilter = "Equal_Subject_Code='" + subnos + "' or Com_Subject_Code='" + subnos + "'";
                    DataView dveqsub = dvsubequal.Tables[0].DefaultView;
                    if (dveqsub.Count > 0)
                    {
                        Hashtable hatequal = new Hashtable();
                        dssubject.Tables[0].DefaultView.RowFilter = "Subject_Code='" + dveqsub[0]["Com_Subject_Code"].ToString() + "'";
                        DataView dssub = dssubject.Tables[0].DefaultView;
                        if (dssub.Count > 0)
                        {
                            string subva = dveqsub[0]["Com_Subject_Code"].ToString() + " " + dssub[0]["subject_name"].ToString();
                            if (commsub == "")
                            {
                                hatequal.Add(subva.Trim().ToLower(), subva);
                                commsub = subva;
                            }

                            for (int eq = 0; eq < dveqsub.Count; eq++)
                            {
                                dssubject.Tables[0].DefaultView.RowFilter = "Subject_Code='" + dveqsub[eq]["Equal_Subject_Code"].ToString() + "'";
                                dssub = dssubject.Tables[0].DefaultView;
                                if (dssub.Count > 0)
                                {
                                    subva = dveqsub[eq]["Equal_Subject_Code"].ToString() + " " + dssub[0]["subject_name"].ToString();
                                    if (!hatequal.Contains(subva.Trim().ToLower()))
                                    {
                                        if (commsub == "")
                                        {
                                            commsub = subva;
                                        }
                                        else
                                        {
                                            commsub = commsub + "~" + subva;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (commsub.Trim() == "")
                    {
                        commsub = subjectnamecode;
                    }


                    DateTime dt = Convert.ToDateTime(dateofexam);

                    string subjecttype = "";


                    // ds1.Tables[0].DefaultView.RowFilter = " roomno='" + hallno + "' and edate='" + dt.ToString("MM/dd/yyyy") + "' and ses_sion='" + sessd + "'  and subject_code='" + subnos + "' and degree_code='" + degcode + "'";
                    ds1.Tables[0].DefaultView.RowFilter = " roomno='" + hallno + "' and edate='" + dt.ToString("MM/dd/yyyy") + "' and ses_sion='" + sessd + "'  and subject_code='" + subnos + "'";
                    DataView dv = ds1.Tables[0].DefaultView;
                    for (int sn = 0; sn < dv.Count; sn++)
                    {
                        if (seatno == "")
                        {
                            seatno = dv[sn]["seat_no"].ToString();
                        }
                        else
                        {
                            seatno = seatno + ", " + dv[sn]["seat_no"].ToString();
                        }
                        subjecttype = dv[sn]["subject_type"].ToString();
                    }
                    overstng = dv.Count.ToString();

                    //ds1.Tables[0].DefaultView.RowFilter = " edate='" + dt.ToString("MM/dd/yyyy") + "' and ses_sion='" + sessd + "'  and subject_code='" + subnos + "' and degree_code='" + degcode + "'";
                    ds1.Tables[0].DefaultView.RowFilter = " edate='" + dt.ToString("MM/dd/yyyy") + "' and ses_sion='" + sessd + "'  and subject_code='" + subnos + "'";
                    DataView dvove = ds1.Tables[0].DefaultView;
                    totalhall = dvove.Count.ToString();

                    if (sessd == "A.N")
                    {
                        sessd = "AFTERNOON";
                    }
                    else
                    {
                        sessd = "FORENOON";
                    }
                    int colto = 10;
                    PdfTextArea ptc1 = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                     new PdfArea(myprovdoc, 30, colto, 750, 170), System.Drawing.ContentAlignment.MiddleCenter, clgname + " ( " + category + " )");

                    myprov_pdfpage.Add(ptc1);

                    colto = colto + 15;
                    PdfTextArea ptc7 = new PdfTextArea(Fontbold22, System.Drawing.Color.Black,
                                                                  new PdfArea(myprovdoc, 26, colto, 750, 230), System.Drawing.ContentAlignment.MiddleCenter, " END OF SEMESTER EXAMINATION");

                    myprov_pdfpage.Add(ptc7);

                    colto = colto + 50;
                    PdfTextArea ptc11 = new PdfTextArea(Fontbold22, System.Drawing.Color.Black,
                                                                new PdfArea(myprovdoc, 40, colto, 750, 230), System.Drawing.ContentAlignment.MiddleCenter, dt.ToString("dd-MMM-yy") + "       " + sessd + "      ROOM -" + hallno + "");

                    myprov_pdfpage.Add(ptc11);

                    colto = colto + 50;
                    //PdfTextArea ptc22 = new PdfTextArea(Fontbold22, System.Drawing.Color.Black,
                    //                                            new PdfArea(myprovdoc, 60, colto, 750, 230), System.Drawing.ContentAlignment.MiddleCenter, degreedetails + "    " + subjecttype);

                    //myprov_pdfpage.Add(ptc22);

                    PdfTextArea ptc22 = new PdfTextArea(Fontbold22, System.Drawing.Color.Black,
                                                               new PdfArea(myprovdoc, 60, colto, 750, 230), System.Drawing.ContentAlignment.MiddleCenter, subjecttype);

                    myprov_pdfpage.Add(ptc22);


                    string[] spsub = commsub.Split('~');
                    for (int svs = 0; svs <= spsub.GetUpperBound(0); svs++)
                    {
                        colto = colto + 30;
                        PdfTextArea ptc13 = new PdfTextArea(Fontbold222, System.Drawing.Color.Black,
                                                                   new PdfArea(myprovdoc, 50, colto, 750, 230), System.Drawing.ContentAlignment.MiddleCenter, spsub[svs].ToString());

                        myprov_pdfpage.Add(ptc13);
                    }

                    colto = colto + 50;
                    PdfTextArea ptc14 = new PdfTextArea(Fontbold22, System.Drawing.Color.Black,
                                                              new PdfArea(myprovdoc, 50, colto, 750, 230), System.Drawing.ContentAlignment.MiddleLeft, "Seat No  :  ");

                    myprov_pdfpage.Add(ptc14);

                    colto = colto + 30;
                    PdfTextArea ptc15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                             new PdfArea(myprovdoc, 50, colto, 750, 320), System.Drawing.ContentAlignment.MiddleLeft, seatno);

                    myprov_pdfpage.Add(ptc15);

                    colto = 440;
                    PdfTextArea ptc144 = new PdfTextArea(Fontbold22, System.Drawing.Color.Black,
                                                             new PdfArea(myprovdoc, 50, colto, 750, 230), System.Drawing.ContentAlignment.MiddleLeft, "Total  :  " + overstng + " / " + totalhall);

                    myprov_pdfpage.Add(ptc144);

                    myprov_pdfpage.SaveToDocument();
                }
            }
            if (saveflag == true)
            {
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = DateTime.Now.ToString("ddMMyyyyhhmmsstt") + "Questionpackage.pdf";
                    myprovdoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            else
            {
                lblerror.Text = "Please Select the Subject Code And Name and Proceed";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }
    protected void btnView1_Click(object sender, EventArgs e)
    {
        try
        {
            Formattwo();
            //Printcontrol.Visible = false;
            //btngenerate.Visible = false;
            //Hashtable hat = new Hashtable();
            //fpspread1.Sheets[0].RowCount = 0;
            //fpspread1.Sheets[0].ColumnCount = 3;
            //fpspread1.Sheets[0].ColumnHeader.RowCount = 3;
            //fpspread.Visible = false;


            //fpspread1.RowHeader.Visible = false;
            //fpspread1.CommandBar.Visible = false;
            //string exammonth1 = ddlMonth.SelectedItem.Value.ToString();
            //string ExamYear = ddlYear.SelectedItem.Text.ToString();
            //string examdate11 = ddlDate.SelectedItem.Text.ToString();
            //string[] examdatesplit = examdate11.Split('-');
            //string strsql = "";
            //int tot = 0;
            //examdate11 = examdatesplit[1].ToString() + "-" + examdatesplit[0] + "-" + examdatesplit[2];
            //if (examdatesplit.GetUpperBound(0) > 1)
            //{
            //    examdate11 = examdatesplit[1] + "/" + examdatesplit[0] + "/" + examdatesplit[2];

            //    examdate11 = "and es.edate='" + examdate11 + "'";
            //}
            //else
            //{
            //    examdate11 = "";

            //}
            //string Session = ddlSession.SelectedItem.Text.ToString();
            //if (Session.Trim() == "All")
            //{
            //    Session = "";
            //}
            //else
            //{
            //    Session = "and es.ses_sion='" + Session + "'";
            //}
            //string hallno = ddlhall.SelectedItem.Text.ToString();
            //if (hallno.Trim() == "All")
            //{
            //    hallno = "";

            //}
            //else
            //{
            //    hallno = "and  es.roomno='" + hallno + "' ";
            //}
            //if (ddlhall.SelectedItem.Text == "All")
            //{
            //    strsql = "select count( distinct es.regno) as totstu, es.roomno,s.subject_code,s.subject_name,CONVERT(varchar(50),et.exam_date,105)as exam_date,et.exam_date,e.Exam_Month,e.Exam_year,et.exam_session from Exam_Details e,subject s,exmtt_det et, Degree d,Department de,course c,exam_seating es,exmtt em where s.subject_no=et.subject_no and e.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and em.exam_code=et.exam_code and em.Exam_month=e.Exam_Month and em.Exam_year=e.Exam_year and em.degree_code=e.degree_code and e.batch_year=em.batchFrom and em.degree_code=es.degree_code and em.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Degree_Code=es.degree_code and et.exam_date=es.edate and et.subject_no=es.subject_no and et.exam_session=es.ses_sion and e.Exam_Month='" + ddlMonth.SelectedItem.Value + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + examdate11 + " and es.ses_sion='" + ddlSession.SelectedItem.Text + "' group by s.subject_code,s.subject_name,exam_date,e.Exam_Month,e.Exam_year,et.exam_session,es.roomno  order by et.exam_date,et.exam_session,s.subject_code,es.roomno";
            //}
            //else
            //{
            //    strsql = "select count( distinct es.regno) as totstu, es.roomno,s.subject_code,s.subject_name,CONVERT(varchar(50),et.exam_date,105)as exam_date,et.exam_date,e.Exam_Month,e.Exam_year,et.exam_session from Exam_Details e,subject s,exmtt_det et, Degree d,Department de,course c,exam_seating es,exmtt em where s.subject_no=et.subject_no and e.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and em.exam_code=et.exam_code and em.Exam_month=e.Exam_Month and em.Exam_year=e.Exam_year and em.degree_code=e.degree_code and e.batch_year=em.batchFrom and em.degree_code=es.degree_code and em.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Degree_Code=es.degree_code and et.exam_date=es.edate and et.subject_no=es.subject_no and et.exam_session=es.ses_sion and e.Exam_Month='" + ddlMonth.SelectedItem.Value + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + examdate11 + " and es.ses_sion='" + ddlSession.SelectedItem.Text + "' and es.roomno='" + ddlhall.SelectedItem.Text + "' group by s.subject_code,s.subject_name,exam_date,e.Exam_Month,e.Exam_year,et.exam_session,es.roomno order by et.exam_date,et.exam_session,s.subject_code,es.roomno";
            //}

            //ds = da.select_method_wo_parameter(strsql, "Text");


            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    fpspread1.Sheets[0].RowCount++;
            //    int add = 0;
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {

            //        string subno = Convert.ToString(ds.Tables[0].Rows[i]["subject_code"]);
            //        string tot_stud = Convert.ToString(ds.Tables[0].Rows[i]["totstu"]);
            //        string sessnn = Convert.ToString(ds.Tables[0].Rows[i]["exam_session"]);
            //        string date = Convert.ToString(ds.Tables[0].Rows[i]["exam_date"]);
            //        fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(fpspread1.Sheets[0].RowCount - 1, 0, 1, 3);
            //        fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Question Paper Requirements";
            //        fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            //        fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            //        fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

            //        fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, 2);

            //        fpspread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Date:" + date;
            //        fpspread1.Sheets[0].ColumnHeader.Cells[1, 0].Font.Size = FontUnit.Medium;
            //        fpspread1.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Left;

            //        fpspread1.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Session:" + sessnn;
            //        fpspread1.Sheets[0].ColumnHeader.Cells[1, 2].Font.Size = FontUnit.Medium;
            //        fpspread1.Sheets[0].ColumnHeader.Cells[1, 2].HorizontalAlign = HorizontalAlign.Left;


            //    }
            //    fpspread1.Sheets[0].SpanModel.Add(fpspread1.Sheets[0].RowCount - 1, 0, 1, 3);
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 0].Text = "Subject Code";
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Center;
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 0].Font.Size = FontUnit.Medium;
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 0].Font.Bold = true;
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 1].Text = "Hall No";
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 1].Font.Size = FontUnit.Medium;
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 1].Font.Bold = true;
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 2].Text = "No of Question";
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 2].HorizontalAlign = HorizontalAlign.Center;
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 2].Font.Size = FontUnit.Medium;
            //    fpspread1.Sheets[0].ColumnHeader.Cells[2, 2].Font.Bold = true;
            //    fpspread1.Sheets[0].ColumnHeader.Columns[0].Width = 30;
            //    fpspread1.Sheets[0].ColumnHeader.Columns[1].Width = 50;
            //    fpspread1.Sheets[0].ColumnHeader.Columns[2].Width = 50;

            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {

            //        string subno = Convert.ToString(ds.Tables[0].Rows[i]["subject_code"]);
            //        if (!hat.ContainsKey(subno))
            //        {
            //            if (fpspread1.Sheets[0].RowCount > 1)
            //            {
            //                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(add);
            //                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
            //                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
            //                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            //                tot = tot + add;
            //                fpspread1.Sheets[0].SpanModel.Add(fpspread1.Sheets[0].RowCount - 1, 0, 1, 2);
            //                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Text = "Total";
            //                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
            //                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            //                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;


            //            }
            //        }

            //        string halln = Convert.ToString(ds.Tables[0].Rows[i]["roomno"]);
            //        string tot_stud = Convert.ToString(ds.Tables[0].Rows[i]["totstu"]);
            //        string sum = "";
            //        fpspread1.Sheets[0].RowCount++;
            //        fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Text = subno;
            //        fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            //        fpspread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //        fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
            //        fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Text = halln;
            //        fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            //        fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
            //        fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

            //        fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Text = tot_stud;
            //        fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            //        fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
            //        fpspread1.Sheets[0].RowCount++;
            //        sum = Convert.ToString(ds.Tables[0].Rows[i]["totstu"]);

            //        if (!hat.ContainsKey(subno))
            //        {
            //            hat.Add(subno, sum);

            //            add = 0;
            //            add = add + Convert.ToInt32(sum);

            //        }
            //        else
            //        {

            //            add = add + Convert.ToInt32(sum);

            //        }

            //        if (i == ds.Tables[0].Rows.Count - 1)
            //        {

            //            fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(add);
            //            fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
            //            fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            //            fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;

            //            tot = tot + add;

            //            fpspread1.Sheets[0].SpanModel.Add(fpspread1.Sheets[0].RowCount - 1, 0, 1, 2);
            //            fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Text = "Total";
            //            fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
            //            fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            //            fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            //        }
            //    }

            //    fpspread1.Sheets[0].RowCount++;
            //    fpspread1.Sheets[0].SpanModel.Add(fpspread1.Sheets[0].RowCount - 1, 0, 1, 2);
            //    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
            //    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            //    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
            //    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            //    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(tot);
            //    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
            //    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            //    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
            //    fpspread1.SaveChanges();

            //    fpspread1.Sheets[0].PageSize = fpspread1.Sheets[0].RowCount;

            //    lblerror.Text = "";
            //    txtexcelname.Visible = true;
            //    fpspread1.Visible = true;
            //    btnExcel.Visible = true;
            //    btnprintmaster.Visible = true;
            //    lblrptname.Visible = true;
            //}
            //else
            //{
            //    lblerror.Text = "No Records Found";
            //    fpspread1.Visible = false;
            //    btnExcel.Visible = false;
            //    btnprintmaster.Visible = false;
            //    lblrptname.Visible = false;
            //    txtexcelname.Visible = false;
            //    lblerror.Visible = true;
            //}
        }
        catch
        {
        }
    }


    public void Formattwo()
    {
        try
        {
            Printcontrol.Visible = false;
            btngenerate.Visible = false;
            Hashtable hat = new Hashtable();
            fpspread1.Sheets[0].RowCount = 0;
            fpspread1.Sheets[0].ColumnCount = 3;
            fpspread1.Sheets[0].ColumnHeader.RowCount = 3;
            fpspread.Visible = false;

            fpspread1.RowHeader.Visible = false;
            fpspread1.CommandBar.Visible = false;
            string exammonth1 = ddlMonth.SelectedItem.Value.ToString();
            string ExamYear = ddlYear.SelectedItem.Text.ToString();
            string examdate11 = ddlDate.SelectedItem.Text.ToString();
            string[] examdatesplit = examdate11.Split('-');
            string strsql = "";
            int tot = 0;
            string subno = "";
            string sum = "";
            string tot_stud = "";
            string halln = "";
            int add = 0;
            string date = ddlDate.SelectedItem.Text;
            string sessnn = ddlSession.SelectedItem.Text;
            //fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(fpspread1.Sheets[0].RowCount - 1, 0, 1, 2);
            fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Question Paper Requirements";
            fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 3);
            fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            //fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(fpspread1.Sheets[0].RowCount - 1, 0, 1, 2);
            fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, 2);

            fpspread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Date:" + date;
            fpspread1.Sheets[0].ColumnHeader.Cells[1, 0].Font.Size = FontUnit.Medium;
            fpspread1.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Left;

            fpspread1.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Session:" + sessnn;
            fpspread1.Sheets[0].ColumnHeader.Cells[1, 2].Font.Size = FontUnit.Medium;
            fpspread1.Sheets[0].ColumnHeader.Cells[1, 2].HorizontalAlign = HorizontalAlign.Left;


            //fpspread1.Sheets[0].SpanModel.Add(fpspread1.Sheets[0].RowCount - 1, 0, 1, 3);
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 0].Text = "Subject Code";
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Center;
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 0].Font.Size = FontUnit.Medium;
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 0].Font.Bold = true;
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 1].Text = "Hall No";
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 1].Font.Size = FontUnit.Medium;
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 1].Font.Bold = true;
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 2].Text = "No of Question";
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 2].HorizontalAlign = HorizontalAlign.Center;
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 2].Font.Size = FontUnit.Medium;
            fpspread1.Sheets[0].ColumnHeader.Cells[2, 2].Font.Bold = true;
            fpspread1.Sheets[0].ColumnHeader.Columns[0].Width = 30;
            fpspread1.Sheets[0].ColumnHeader.Columns[1].Width = 50;
            fpspread1.Sheets[0].ColumnHeader.Columns[2].Width = 50;

            examdate11 = examdatesplit[1].ToString() + "-" + examdatesplit[0] + "-" + examdatesplit[2];
            if (examdatesplit.GetUpperBound(0) > 1)
            {
                examdate11 = examdatesplit[1] + "/" + examdatesplit[0] + "/" + examdatesplit[2];

                examdate11 = "and es.edate='" + examdate11 + "'";
            }
            else
            {
                examdate11 = "";

            }
            string Session = ddlSession.SelectedItem.Text.ToString();
            if (Session.Trim() == "All")
            {
                Session = "";
            }
            else
            {
                Session = "and es.ses_sion='" + Session + "'";
            }
            string hallno = ddlhall.SelectedItem.Text.ToString();
            if (hallno.Trim() == "All")
            {
                hallno = "";

            }
            else
            {
                hallno = "and  es.roomno='" + hallno + "' ";
            }
            if (ddlhall.SelectedItem.Text == "All")
            {
                strsql = "select count( distinct es.regno) as totstu, es.roomno,s.subject_code,s.subject_name,CONVERT(varchar(50),et.exam_date,105)as exam_date,et.exam_date,e.Exam_Month,e.Exam_year,et.exam_session from Exam_Details e,subject s,exmtt_det et, Degree d,Department de,course c,exam_seating es,exmtt em where s.subject_no=et.subject_no and e.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and em.exam_code=et.exam_code and em.Exam_month=e.Exam_Month and em.Exam_year=e.Exam_year and em.degree_code=e.degree_code and e.batch_year=em.batchFrom and em.degree_code=es.degree_code and em.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Degree_Code=es.degree_code and et.exam_date=es.edate and et.subject_no=es.subject_no and et.exam_session=es.ses_sion and e.Exam_Month='" + ddlMonth.SelectedItem.Value + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + examdate11 + " and es.ses_sion='" + ddlSession.SelectedItem.Text + "' group by s.subject_code,s.subject_name,exam_date,e.Exam_Month,e.Exam_year,et.exam_session,es.roomno  order by et.exam_date,et.exam_session,s.subject_code,es.roomno";
            }
            else
            {
                strsql = "select count( distinct es.regno) as totstu, es.roomno,s.subject_code,s.subject_name,CONVERT(varchar(50),et.exam_date,105)as exam_date,et.exam_date,e.Exam_Month,e.Exam_year,et.exam_session from Exam_Details e,subject s,exmtt_det et, Degree d,Department de,course c,exam_seating es,exmtt em where s.subject_no=et.subject_no and e.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and em.exam_code=et.exam_code and em.Exam_month=e.Exam_Month and em.Exam_year=e.Exam_year and em.degree_code=e.degree_code and e.batch_year=em.batchFrom and em.degree_code=es.degree_code and em.degree_code=d.Degree_Code and es.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Degree_Code=es.degree_code and et.exam_date=es.edate and et.subject_no=es.subject_no and et.exam_session=es.ses_sion and e.Exam_Month='" + ddlMonth.SelectedItem.Value + "' and e.Exam_year='" + ddlYear.SelectedItem.Text + "' " + examdate11 + " and es.ses_sion='" + ddlSession.SelectedItem.Text + "' and es.roomno='" + ddlhall.SelectedItem.Text + "' group by s.subject_code,s.subject_name,exam_date,e.Exam_Month,e.Exam_year,et.exam_session,es.roomno order by es.roomno";
            }
            ds = da.select_method_wo_parameter(strsql, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    subno = Convert.ToString(ds.Tables[0].Rows[i]["subject_code"]);
                    tot_stud = Convert.ToString(ds.Tables[0].Rows[i]["totstu"]);
                    halln = Convert.ToString(ds.Tables[0].Rows[i]["roomno"]);
                    fpspread1.Sheets[0].RowCount++;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Text = subno;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Text = halln;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;



                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Text = tot_stud;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    fpspread1.Sheets[0].RowCount++;
                    sum = Convert.ToString(ds.Tables[0].Rows[i]["totstu"]);
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Text = "Total";
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(sum);
                    fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    tot = tot + Convert.ToInt32(sum);
                }

                if (!hat.ContainsKey(subno))
                {
                    hat.Add(subno, sum);

                    add = 0;
                    add = add + Convert.ToInt32(sum);

                }
                else
                {

                    add = add + Convert.ToInt32(sum);

                }

                fpspread1.Sheets[0].RowCount++;
                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Text = " Grand Total";
                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, 2);
                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(tot);
                fpspread1.Sheets[0].Cells[fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;


                fpspread1.Sheets[0].PageSize = fpspread1.Sheets[0].RowCount;
                fpspread1.Visible = true;

                lblerror.Text = "";
                txtexcelname.Visible = true;
                fpspread1.Visible = true;
                btnExcel.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;


            }
            else
            {

                lblerror.Text = "No Records Found";
                fpspread1.Visible = false;
                btnExcel.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                lblerror.Visible = true;

            }


        }
        catch (Exception ex)
        {
        }
    }



    protected void btnExcel_Click(object sender, EventArgs e)
    {

        try
        {
            string report = txtexcelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreport(fpspread1, report);
                lblnorec.Visible = false;
            }
            else
            {
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
            }
            btnprintmaster.Focus();
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy");
            string pagename = "questionpackage.aspx";
            string degreedetails = "questionpackage" + date;
            Printcontrol.loadspreaddetails(fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch (Exception ex)
        {

        }
    }

}