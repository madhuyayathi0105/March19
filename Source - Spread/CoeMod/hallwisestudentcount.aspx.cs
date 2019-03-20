using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Configuration;
public partial class hallwisestudentcount : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DAccess2 dt = new DAccess2();
    string college_code = "";
    string norow = "";
    string nocol = "";
    string[] arrang;
    string[] spcel;
    string allotseat = "";
    static int btngen = 0;
    int hss = 0;
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
            lblmsg.Visible = false;
            college_code = Session["collegecode"].ToString();

            if (!IsPostBack)
            {

                loadYear();
                loadmonth();
                mode();
                loaddatesession();
                hss = 0;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void loadYear()
    {
        try
        {
            ds = dt.Examyear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataValueField = "Exam_year";
                ddlYear.DataBind();
            }

        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public void loadmonth()
    {
        try
        {
            ds.Clear();
            string year = ddlYear.SelectedValue;
            ds = dt.Exammonth(year);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthname";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    public void mode()
    {
        try
        {
            string mode = "select distinct type from course where college_code='" + college_code + "' and type is not null and type<>''";
            ds = dt.select_method_wo_parameter(mode, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataValueField = "type";
                ddltype.DataBind();
            }
            else
            {
                ddltype.Items.Insert(0, "");
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    protected void Logout_btn_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }




    public void loaddatesession()
    {
        try
        {
            if (ddlMonth.SelectedIndex != -1)
            {
                string s = "select distinct convert(varchar(20),et.exam_date,105) as ExamDate,et.exam_date from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "' order by et.exam_date";
                ds = dt.select_method_wo_parameter(s, "txt");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlDate.Enabled = true;
                    ddlSession.Enabled = true;
                    ddlDate.Items.Clear();
                    ddlDate.DataSource = ds;
                    ddlDate.DataTextField = "ExamDate";
                    ddlDate.DataValueField = "ExamDate";
                    ddlDate.DataBind();
                    ddlDate.Items.Insert(0, "All");
                }
                string s1 = "select distinct  et.exam_session from exmtt_det et,exmtt e where et.exam_code=e.exam_code and  e.exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_Year='" + ddlYear.SelectedItem.Text.ToString() + "'";
                ds = dt.select_method_wo_parameter(s1, "txt");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlDate.Enabled = true;
                    ddlSession.Enabled = true;
                    ddlSession.Items.Clear();
                    ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));

                    ddlSession.DataSource = ds;
                    ddlSession.DataTextField = "exam_session";
                    ddlSession.DataValueField = "exam_session";
                    ddlSession.DataBind();
                }
                else
                {
                    ddlDate.Items.Clear();
                    ddlSession.Items.Clear();
                    ddlDate.Enabled = false;
                    ddlSession.Enabled = false;
                }
                ddlhall.Items.Clear();
            }

        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loaddatesession();
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadmonth();
        loaddatesession();
    }
    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlhall.Items.Clear();
        if (ddlDate.Items.Count > 0)
        {
            if (ddlDate.SelectedItem.ToString() != "All")
            {
                string[] spd = ddlDate.SelectedItem.ToString().Split('-');
                string typequery = "";
                if (ddltype.SelectedItem.Text != "All")
                {
                    if (ddltype.SelectedItem.Text != "")
                    {
                        typequery = "and c.type='" + ddltype.SelectedItem.Text + "'";
                    }
                }
                string hl = "select distinct es.roomno from exmtt e,exmtt_det et,exam_seating es,course c,Degree d where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id " + typequery + " and e.Exam_year='" + ddlYear.SelectedItem.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and es.edate='" + spd[1] + '/' + spd[0] + '/' + spd[2] + "' and es.ses_sion='" + ddlSession.SelectedItem.ToString() + "'";
                ds = dt.select_method_wo_parameter(hl, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlhall.Enabled = true;
                    ddlhall.DataSource = ds;
                    ddlhall.DataTextField = "roomno";
                    ddlhall.DataValueField = "roomno";
                    ddlhall.DataBind();
                }
            }
        }

    }
    protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlhall.Items.Clear();
        clear();
        if (ddlDate.Items.Count > 0)
        {
            if (ddlDate.SelectedItem.ToString() != "All")
            {
                string[] spd = ddlDate.SelectedItem.ToString().Split('-');
                string typequery = "";
                if (ddltype.SelectedItem.Text != "All")
                {
                    if (ddltype.SelectedItem.Text != "")
                    {
                        typequery = "and c.type='" + ddltype.SelectedItem.Text + "'";
                    }
                }
                string hl = "select distinct es.roomno from exmtt e,exmtt_det et,exam_seating es,course c,Degree d where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id " + typequery + " and e.Exam_year='" + ddlYear.SelectedItem.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and es.edate='" + spd[1] + '/' + spd[0] + '/' + spd[2] + "' and es.ses_sion='" + ddlSession.SelectedItem.ToString() + "'";
                ds = dt.select_method_wo_parameter(hl, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlhall.Enabled = true;
                    ddlhall.DataSource = ds;
                    ddlhall.DataTextField = "roomno";
                    ddlhall.DataValueField = "roomno";
                    ddlhall.DataBind();
                }
            }
        }
    }
    protected void ddlhall_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlhall.Items.Clear();
        clear();
        if (ddlDate.Items.Count > 0)
        {
            if (ddlDate.SelectedItem.ToString() != "All")
            {
                string[] spd = ddlDate.SelectedItem.ToString().Split('-');
                string typequery = "";
                if (ddltype.SelectedItem.Text != "All")
                {
                    if (ddltype.SelectedItem.Text != "")
                    {
                        typequery = "and c.type='" + ddltype.SelectedItem.Text + "'";
                    }
                }
                string hl = "select distinct es.roomno from exmtt e,exmtt_det et,exam_seating es,course c,Degree d where e.exam_code=et.exam_code and et.subject_no=es.subject_no and e.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id " + typequery + " and e.Exam_year='" + ddlYear.SelectedItem.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and es.edate='" + spd[1] + '/' + spd[0] + '/' + spd[2] + "' and es.ses_sion='" + ddlSession.SelectedItem.ToString() + "'";
                ds = dt.select_method_wo_parameter(hl, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlhall.Enabled = true;
                    ddlhall.DataSource = ds;
                    ddlhall.DataTextField = "roomno";
                    ddlhall.DataValueField = "roomno";
                    ddlhall.DataBind();
                }
            }
        }
    }



    protected void printseating_click(object sender, EventArgs e)
    {
        try
        {
            string pagename = "hallwisestudentcount.aspx";
            string degreedetails = "Office of the Controller of Examinations $Room Wise Strength Report @Date & Session : " + ddlDate.SelectedItem.Text + " & " + ddlSession.SelectedItem.Text + "@ROOM - " + ddlhall.SelectedItem.ToString();
            if (chkconsolidate.Checked == true)
            {
                degreedetails = "Office of the Controller of Examinations $Room Wise Strength Report @Date & Session : " + ddlDate.SelectedItem.Text + " & " + ddlSession.SelectedItem.Text + "@ROOM - " + ddlhall.SelectedItem.ToString();
            }
            Printcontrol.loadspreaddetails(Fpseating, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }

    protected void Excelseating_click(object sender, EventArgs e)
    {
        try
        {
            string report = txtexseat.Text;
            if (report.ToString().Trim() != "")
            {
                dt.printexcelreport(Fpseating, report);
                lblmessage1.Visible = false;
            }
            else
            {
                lblmessage1.Text = "Please Enter Your Report Name";
                lblmessage1.Visible = true;
            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void chkconsolidate_checkedchange(object sender, EventArgs e)
    {
        clear();
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            DataView dv = new DataView();
            Fpseating.Sheets[0].AutoPostBack = false;
            string dat = "";
            if (ddlhall.Items.Count > 0)
            {


                string strmode = "";
                if (ddltype.SelectedItem.Text != "All")
                {
                    if (ddltype.SelectedItem.Text != "")
                    {
                        strmode = " and Mode='" + ddltype.SelectedItem.Text + "'";
                    }
                }

                if (ddlhall.SelectedItem.Text != "")
                {

                    Fpseating.Sheets[0].ColumnCount = 0;
                    if (ddlDate.SelectedItem.Text != "All")
                    {
                        dat = ddlDate.SelectedItem.Text;
                        string[] datt = dat.Split('-');
                        dat = datt[2].ToString() + "-" + datt[1].ToString() + "-" + datt[0].ToString();
                    }
                    else if (ddlDate.SelectedItem.Text == "All")
                    {
                        dat = ddlDate.Items[1].Text;
                        string[] datt = dat.Split('-');
                        dat = datt[2].ToString() + "-" + datt[1].ToString() + "-" + datt[0].ToString();
                    }

                    Fpseating.Sheets[0].RowHeader.Visible = false;
                    Fpseating.Sheets[0].AutoPostBack = true;
                    Fpseating.CommandBar.Visible = false;
                    Fpseating.Sheets[0].RowCount = 0;
                    Fpseating.Sheets[0].ColumnCount = 0;
                    Fpseating.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpseating.Height = 600;
                    Fpseating.Width = 850;

                    Fpseating.Sheets[0].ColumnCount = 3;
                    Fpseating.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpseating.Sheets[0].Columns[0].Width = 20;
                    Fpseating.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    Fpseating.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                    Fpseating.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                    Fpseating.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;

                    Fpseating.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Details";
                    Fpseating.Sheets[0].Columns[1].Width = 750;
                    Fpseating.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                    Fpseating.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                    Fpseating.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                    Fpseating.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;

                    Fpseating.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Count";
                    Fpseating.Sheets[0].Columns[2].Width = 50;
                    Fpseating.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    Fpseating.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                    Fpseating.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                    Fpseating.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;

                    if (chkconsolidate.Checked == false)
                    {
                        Fpseating.Sheets[0].ColumnCount++;
                        Fpseating.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Absentees";
                        Fpseating.Sheets[0].Columns[3].Width = 50;
                        Fpseating.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                        Fpseating.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                        Fpseating.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                        Fpseating.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                    }


                    FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                    style2.Font.Size = 13;
                    style2.Font.Name = "Book Antiqua";
                    style2.Font.Bold = true;
                    style2.HorizontalAlign = HorizontalAlign.Center;
                    style2.ForeColor = System.Drawing.Color.White;
                    style2.BackColor = System.Drawing.Color.Teal;
                    Fpseating.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

                    Fpseating.Sheets[0].SheetName = " ";
                    Fpseating.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                    Fpseating.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                    Fpseating.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                    Fpseating.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    Fpseating.Sheets[0].DefaultStyle.Font.Bold = false;

                    int srno = 0;
                    string strquery = "select s.subject_code,s.subject_name,COUNT(es.regno) as num from exmtt e,exmtt_det et,exam_seating es,subject s where e.exam_code=et.exam_code and et.subject_no=es.subject_no and es.edate=et.exam_date and es.ses_sion=et.exam_session and et.subject_no=s.subject_no and es.subject_no=s.subject_no and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + dat + "' and et.exam_session='" + ddlSession.SelectedValue.ToString() + "' and es.roomno='" + ddlhall.SelectedItem.Text + "' group by s.subject_code,s.subject_name order by num desc";
                    DataSet dsquery = dt.select_method_wo_parameter(strquery, "text");
                    if (dsquery.Tables[0].Rows.Count > 0)
                    {
                        txtexseat.Visible = true;
                        lblexcsea.Visible = true;
                        Excel_seating.Visible = true;
                        Print_seating.Visible = true;
                        Fpseating.Visible = true;

                        int grandtotal = 0;
                        string strstydetails = "select s.subject_code,s.subject_name,r.Batch_Year,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code,r.Reg_No,es.seat_no from exmtt e,exmtt_det et,exam_seating es,subject s,Degree d,Course c,Department de,Registration r where e.exam_code=et.exam_code and et.subject_no=es.subject_no and es.edate=et.exam_date and es.ses_sion=et.exam_session and et.subject_no=s.subject_no and es.regno=r.Reg_No and es.subject_no=s.subject_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + dat + "' and et.exam_session='" + ddlSession.SelectedValue.ToString() + "' and es.roomno='" + ddlhall.SelectedItem.Text + "' order by  s.subject_code,r.batch_year desc,r.Reg_No,es.seat_no";
                        if (chkconsolidate.Checked == true)
                        {
                            strstydetails = "select s.subject_code,s.subject_name,r.Batch_Year,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code,count(r.Reg_No) as stustrenth from exmtt e,exmtt_det et,exam_seating es,subject s,Degree d,Course c,Department de,Registration r where e.exam_code=et.exam_code and et.subject_no=es.subject_no and es.edate=et.exam_date and es.ses_sion=et.exam_session and et.subject_no=s.subject_no and es.regno=r.Reg_No and es.subject_no=s.subject_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.Exam_year='" + ddlYear.SelectedValue.ToString() + "' and et.exam_date='" + dat + "' and et.exam_session='" + ddlSession.SelectedValue.ToString() + "' and es.roomno='" + ddlhall.SelectedItem.Text + "' group by s.subject_code,s.subject_name,r.Batch_Year,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code";
                        }
                        DataSet dsconcolidate = dt.select_method_wo_parameter(strstydetails, "Text");
                        for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                        {
                            int noofstuforsubwise = 0;
                            string subjectcode = dsquery.Tables[0].Rows[i]["subject_code"].ToString();
                            string subname = dsquery.Tables[0].Rows[i]["subject_name"].ToString();
                            Fpseating.Sheets[0].RowCount++;
                            int subjecrrow = Fpseating.Sheets[0].RowCount - 1;
                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Text = subjectcode + "  " + subname;
                            Fpseating.Sheets[0].Rows[Fpseating.Sheets[0].RowCount - 1].BackColor = Color.LightSeaGreen;
                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                            if (chkconsolidate.Checked == true)
                            {
                                Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 0, 1, 3);
                            }
                            else
                            {
                                Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 0, 1, 4);
                            }
                            string getrollquery = "";
                            int noofstu = 0;
                            Hashtable hatdegree = new Hashtable();
                            dsconcolidate.Tables[0].DefaultView.RowFilter = "subject_code='" + subjectcode + "'";
                            DataView dvstu = dsconcolidate.Tables[0].DefaultView;
                            for (int s = 0; s < dvstu.Count; s++)
                            {
                                string batch = dvstu[s]["Batch_Year"].ToString();
                                string eduleve = dvstu[s]["Edu_Level"].ToString();
                                string course = dvstu[s]["Course_Name"].ToString();
                                string department = dvstu[s]["Dept_Name"].ToString();
                                string degreecode = dvstu[s]["Degree_Code"].ToString();
                                if (chkconsolidate.Checked == true)
                                {
                                    string strength = dvstu[s]["stustrenth"].ToString();
                                    srno++;
                                    Fpseating.Sheets[0].RowCount++;
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Text = batch + " - " + eduleve + " - " + course + " - " + department;
                                    Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Text = strength;
                                    grandtotal = grandtotal + Convert.ToInt32(strength);
                                    noofstuforsubwise = noofstuforsubwise + Convert.ToInt32(strength);
                                }
                                else
                                {
                                    string regno = dvstu[s]["Reg_No"].ToString();
                                    string seat = dvstu[s]["seat_no"].ToString();
                                    if (seat.Length == 1)
                                    {
                                        seat = "0" + seat;
                                    }
                                    string setval = regno + " [" + seat + "]";
                                    if (!hatdegree.Contains(batch + '-' + degreecode))
                                    {
                                        if (s > 0)
                                        {
                                            Fpseating.Sheets[0].RowCount++;
                                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Text = getrollquery;
                                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Text = noofstu.ToString();
                                        }
                                        hatdegree.Add(batch + '-' + degreecode, batch + '-' + degreecode);
                                        srno++;

                                        Fpseating.Sheets[0].RowCount++;
                                        Fpseating.Sheets[0].Rows[Fpseating.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
                                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Text = batch + " - " + eduleve + " - " + course + " - " + department;
                                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                        Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 1, 1, 3);
                                        getrollquery = "";
                                        noofstu = 0;
                                    }

                                    noofstuforsubwise++;
                                    noofstu++;
                                    grandtotal++;
                                    if (getrollquery == "")
                                    {
                                        getrollquery = setval;
                                    }
                                    else
                                    {
                                        getrollquery = getrollquery + ",    " + setval;
                                    }

                                    if (s == dvstu.Count - 1)
                                    {
                                        Fpseating.Sheets[0].RowCount++;
                                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 1].Text = getrollquery;
                                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Text = noofstu.ToString();
                                    }
                                }
                            }
                            Fpseating.Sheets[0].RowCount++;
                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Text = "Total No.of Student";
                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 0, 1, 2);
                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Text = noofstuforsubwise.ToString();
                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Large;

                        }

                        Fpseating.Sheets[0].RowCount++;
                        Fpseating.Sheets[0].Rows[Fpseating.Sheets[0].RowCount - 1].BackColor = Color.LightSlateGray;
                        Fpseating.Sheets[0].SpanModel.Add(Fpseating.Sheets[0].RowCount - 1, 0, 1, 2);
                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Text = "Grand Total No.of Student";
                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Large;
                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Large;
                        Fpseating.Sheets[0].Cells[Fpseating.Sheets[0].RowCount - 1, 2].Text = grandtotal.ToString();
                    }
                    else
                    {
                        lblmsg.Text = "No Records Found";
                        lblmsg.Visible = true;
                        txtexseat.Visible = false;
                        lblexcsea.Visible = false;
                        Excel_seating.Visible = false;
                        Print_seating.Visible = false;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    public void clear()
    {
        Printcontrol.Visible = false;
        lblmessage1.Visible = false;
        lblmsg.Visible = false;
        Fpseating.Visible = false;
        Print_seating.Visible = false;
        Excel_seating.Visible = false;
        lblexcsea.Visible = false;
        txtexseat.Visible = false;
    }
    protected void btnsms_click(object sender, EventArgs e)
    {
        try
        {
            clear();
            string strsmstext = txtsms.Text.ToString();
            if (strsmstext.Trim() != "")
            {
                if (ddlDate.SelectedItem.ToString() != "All")
                {
                    college_code = Session["collegecode"].ToString();

                    string strquery = "select Convert(nvarchar(15),et.exam_date,105) edate,et.exam_session,s.subject_code,s.subject_name,r.Reg_No,r.Roll_No,r.Stud_Name,es.seat_no,es.roomno,a.Student_Mobile from exmtt e,exmtt_det et,exam_seating es,subject s,Registration r,applyn a where e.exam_code=et.exam_code and et.subject_no=es.subject_no and es.subject_no=s.subject_no and es.regno=r.Reg_No";
                    strquery = strquery + " and r.App_No=a.app_no and isnull(a.Student_Mobile,'')<>'' and e.Exam_year='" + ddlYear.SelectedItem.ToString() + "' and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and Convert(nvarchar(15),et.exam_date,105)='" + ddlDate.SelectedItem.ToString() + "' and es.ses_sion='" + ddlSession.SelectedItem.ToString() + "' order by es.roomno,es.seat_no";
                    DataSet ds = dt.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + college_code + "'";
                        ds1.Dispose();
                        ds1.Reset();
                        ds1 = dt.select_method_wo_parameter(strsenderquery, "Text");
                        string user_id = "";
                        string SenderID = "", Password = "";
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
                        }
                        string getval = dt.GetUserapi(user_id);
                        string[] spret = getval.Split('-');
                        if (spret.GetUpperBound(0) == 1)
                        {
                            SenderID = spret[0].ToString();
                            Password = spret[1].ToString();
                            Session["api"] = user_id;
                            Session["senderid"] = SenderID;
                        }
                        if (SenderID.Trim() != "")
                        {
                            for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                            {
                                string roll = ds.Tables[0].Rows[r]["Roll_No"].ToString();
                                string reg = ds.Tables[0].Rows[r]["Reg_No"].ToString();
                                string name = ds.Tables[0].Rows[r]["subject_name"].ToString();
                                string date = ds.Tables[0].Rows[r]["edate"].ToString();
                                string sess = ds.Tables[0].Rows[r]["exam_session"].ToString();
                                string subject = ds.Tables[0].Rows[r]["subject_name"].ToString();
                                string scode = ds.Tables[0].Rows[r]["subject_code"].ToString();
                                string seat = ds.Tables[0].Rows[r]["seat_no"].ToString();
                                string room = ds.Tables[0].Rows[r]["roomno"].ToString();
                                string mobilenos = ds.Tables[0].Rows[r]["Student_Mobile"].ToString();

                                string strbval = strsmstext;
                                strbval = strbval.ToUpper().Replace("$ROLLNO$", "" + roll + "");
                                strbval = strbval.ToUpper().Replace("$REGNO$", "" + reg + "");
                                strbval = strbval.ToUpper().Replace("$NAME$", "" + name + "");
                                strbval = strbval.ToUpper().Replace("$SUBJECT$", "" + subject + "");
                                strbval = strbval.ToUpper().Replace("$SCODE$", "" + scode + "");
                                strbval = strbval.ToUpper().Replace("$DATE$", "" + date + "");
                                strbval = strbval.ToUpper().Replace("$SESSION$", "" + sess + "");
                                strbval = strbval.ToUpper().Replace("$ROOM$", "" + room + "");
                                strbval = strbval.ToUpper().Replace("$SEAT$", "" + seat + "");
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strbval + "&priority=ndnd&stype=normal";
                                //string isst = "0";
                                //smsreport(strpath, isst, mobilenos, strbval);
                                int nofosmssend = dt.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), mobilenos, strbval, "0");
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Meassege Sended Sucessfully')", true);
                                return;
                            }
                        }
                        else
                        {
                            lblmsg.Text = "Please Update SMS Sender Parameters";
                            lblmsg.Visible = true;
                        }
                    }
                    else
                    {
                        lblmsg.Text = "Please Allot The Exam Seating Arrangements";
                        lblmsg.Visible = true;
                    }
                }
                else
                {
                    lblmsg.Text = "Please Select One Date";
                    lblmsg.Visible = true;
                }
            }
            else
            {
                lblmsg.Text = "Please Enter The SMS Content";
                lblmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }

    }
    public void smsreport(string uril, string isstaff, string mobilenos, string strmsg)
    {
        try
        {
            college_code = Session["collegecode"].ToString();
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();

            string groupmsgid = "";
            groupmsgid = strvel.Trim().ToString(); //aruna 02oct2013 strvel;       

            int sms = 0;
            string smsreportinsert = "";

            string[] split_id = groupmsgid.Split(' ');

            string[] split_mobileno = mobilenos.Split(new Char[] { ',' });

            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                string group_id = split_id[icount].ToString();
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date )values( '" + split_mobileno[icount] + "','" + group_id + "','" + strmsg + "','" + college_code + "','" + isstaff + "','" + date + "' )"; //Modify By M.SakthiPriya 11-12-2014
                sms = dt.update_method_wo_parameter(smsreportinsert, "Text");
            }

            if (sms == 1)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Detail's added Succefully";
            }
            else
            {
                lblmsg.Text = "Detail's added failed";
                lblmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = "Detail's added failed";
            lblmsg.Visible = true;
        }
    }

}
