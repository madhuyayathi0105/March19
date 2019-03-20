using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Data;
using Gios.Pdf;
using System.Text;

public partial class HRMOD_DepartmentWise_attendance_Report : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet dts = new DataSet();
    DataSet ds1 = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    DAccess2 d2 = new DAccess2();
    static string clgcode = string.Empty;
    string group_user = "";
    Boolean cellclick = false;
    string tempdept_ = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
            //            Response.Redirect(redrURI, false);
            //            return;
            //        }
            //    }
            //}
            //if (Session["collegecode"] == null)
            //{
            //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            //    Response.Redirect(redrURI, false);
            //    return;
            //}
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            if (!IsPostBack)
            {
                binddept();
                Txtentryfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch (Exception ex) { }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdbformate1.Checked == true)
            {
                Printcontrol.Visible = false;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                Hashtable total_leavetypeval = new Hashtable();
                int sno = 0;
                string query = "";
                Fpspread1.Sheets[0].Visible = true;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = false;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].ColumnCount = 3;
                FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle2.ForeColor = Color.Black;
                darkstyle2.HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                double totalabsent = 0; string value = ""; double tot = 0; double val = 0; double totalabsents = 0;
                double totalpresent = 0;


                string[] dtfrom;
                dtfrom = Txtentryfrom.Text.Split('/');
                int date = Convert.ToInt32(dtfrom[0]);
                string month = dtfrom[1];
                string year = dtfrom[2];
                // string atdate = (date.TrimStart('0'));
                string atmonth = (month.TrimStart('0'));
                string monyear = atmonth + "/" + year;

                //string monthyr = Convert.ToString(month + '/' + year);

                DateTime strstartdate = Convert.ToDateTime(dtfrom[1] + '/' + dtfrom[0] + '/' + dtfrom[2]).Date;


                string department = Convert.ToString(ddldept.SelectedItem.Value);
                string DEPTFILTER = string.Empty;
                if (ddldept.SelectedIndex != 0)
                    DEPTFILTER = " and st.dept_code='" + department + "'";

                query = "select count(sm.staff_code)StaffCount,hr.dept_name,st.dept_code  from stafftrans st,staffmaster sm,hrdept_master hr where st.staff_code=sm.staff_code and hr.dept_code=st.dept_code and st.latestrec=1  and ((sm.resign=0 and sm.settled=0)and (Discontinue =0 or Discontinue is null)) " + DEPTFILTER + " group by hr.dept_name,st.dept_code order by hr.dept_name";
                query += " select distinct category,shortname,LeaveMasterPK from leave_category where college_code='" + Session["collegecode"].ToString() + "'";
                query += " select [" + date + "] as date from staff_attnd sa,staffmaster sm,stafftrans st where sm.staff_code=sa.staff_code and st.staff_code=sm.staff_code and sm.resign='0' and sm.settled='0' and sa.staff_code=st.staff_code  and sa.mon_year='" + monyear + "' AND [" + date + "] NOT LIKE 'P-%'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    //ermsg.Visible = false;
                    Fpspread1.Visible = true;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[1].Label = "Name of the department ";
                    Fpspread1.Sheets[0].ColumnHeader.Columns[2].Label = "Dept Staff Strength";
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnCount++;
                        string clgshortname = ds.Tables[1].Rows[i]["shortname"].ToString();
                        string leavefk = ds.Tables[1].Rows[i]["LeaveMasterPK"].ToString();

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = clgshortname;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = leavefk;


                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 80;
                    }
                    Fpspread1.Sheets[0].ColumnCount++;
                    Fpspread1.Sheets[0].ColumnHeader.Columns[Fpspread1.Sheets[0].ColumnCount - 1].Label = "Total Absent";


                    Fpspread1.Sheets[0].Columns[0].Width = 80;
                    Fpspread1.Sheets[0].Columns[1].Width = 300;
                    Fpspread1.Sheets[0].Columns[2].Width = 80;
                    Fpspread1.Sheets[0].Columns[3].Width = 80;

                    Fpspread1.Sheets[0].Columns[0].Locked = true;
                    Fpspread1.Sheets[0].Columns[1].Locked = true;
                    Fpspread1.Sheets[0].Columns[2].Locked = true;
                    //Fpspread1.Sheets[0].Columns[3].Locked = true;
                    //Fpspread1.Sheets[0].Columns[4].Locked = true;
                    //Fpspread1.Sheets[0].Columns[5].Locked = true;
                    //Fpspread1.Sheets[0].Columns[6].Locked = true;
                    //Fpspread1.Sheets[0].Columns[7].Locked = true;
                    //Fpspread1.Sheets[0].Columns[8].Locked = true;
                    //Fpspread1.Sheets[0].Columns[9].Locked = true;
                    //Fpspread1.Sheets[0].Columns[10].Locked = true;
                    //Fpspread1.Sheets[0].Columns[11].Locked = true;
                    int totalcount = 0;
                    int count = 0;
                    // int tot = 0;
                    string query1 = "select COUNT(sm.staff_code)staffCount,st.dept_code,[" + date + "]  from stafftrans st,staffmaster sm,staff_attnd sat,hrdept_master dt where dt.dept_code=st.dept_code and st.staff_code=sm.staff_code and latestrec='1' and sm.resign=0 and sm.settled=0  and sat.staff_code=st.staff_code and dt.college_code='" + Session["collegecode"].ToString() + "' and mon_year ='" + monyear + "' and [" + date + "] is not null " + DEPTFILTER + " group by st.dept_code,[" + date + "] ";
                    DataSet ds2 = new DataSet();
                    ds2.Clear();
                    ds2 = d2.select_method_wo_parameter(query1, "Text");


                    for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                    {

                        string dept = ds.Tables[0].Rows[rolcount]["dept_name"].ToString();
                        string deptcode = ds.Tables[0].Rows[rolcount]["dept_code"].ToString();


                        string staffcount = ds.Tables[0].Rows[rolcount]["StaffCount"].ToString();

                        double total = 0;
                        sno++;
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dept);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(deptcode);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(staffcount);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        count = Convert.ToInt32(staffcount);
                        totalcount = totalcount + count;




                        for (int i = 3; i < Fpspread1.Sheets[0].ColumnCount; i++)
                        {
                            string leavetype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, i].Text);
                            double.TryParse(Convert.ToString(ds2.Tables[0].Compute("Sum(staffCount)", "  dept_code in ('" + deptcode + "') and  [" + date + "] like'" + leavetype + "-%'")), out totalabsent);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, i].Text = (Convert.ToString(totalabsent) == "0") ? "-" : Convert.ToString(totalabsent);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                            total = total + totalabsent;
                            totalabsents = totalabsents + totalabsent;


                            if (total_leavetypeval.Contains(leavetype))
                            {
                                value = "";
                                value = total_leavetypeval[leavetype].ToString();
                                total_leavetypeval.Remove(leavetype);

                                tot = Convert.ToInt32(value) + Convert.ToInt32(totalabsent);
                                total_leavetypeval.Add(leavetype, tot);
                            }
                            else
                            {
                                total_leavetypeval.Add(leavetype, Convert.ToInt32(totalabsent));
                            }


                        }



                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(total);

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;


                    }



                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "Total Faculty Strength ";
                    Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 2);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(totalcount);

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    for (int i = 3; i < Fpspread1.Sheets[0].ColumnCount; i++)
                    {
                        string leavetype = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, i].Text);//delsi
                        if (total_leavetypeval.Count > 0)
                        {
                            value = "";
                            if (total_leavetypeval.Contains(leavetype))
                            {
                                value = total_leavetypeval[leavetype].ToString();
                            }
                            else
                            {
                                value = "0";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, i].Text = Convert.ToString(value);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
                        }


                    }
                    totalpresent = totalcount - totalabsents;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalabsents);

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;


                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "Total Present ";
                    Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 2);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(totalpresent);

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "Total Absent ";
                    Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 2);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(totalabsents);

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].RowCount++;


                    if (total_leavetypeval.Contains("UL"))
                    {
                        value = Convert.ToString(total_leavetypeval["UL"]);
                    }


                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "*Un Authorised Leave ";
                    Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 2);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(value);

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "PRINCIPAL";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;


                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Width = 1000;
                    Fpspread1.Height = 500;
                    rptprint.Visible = true;

                }
                else
                {
                    Fpspread1.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Records Found";
                    rptprint.Visible = false;
                }

            }
            else if (rdbformate2.Checked == true)
            {
                Fpspread1.Visible = false;
                Hashtable total_leavetypeval = new Hashtable();
                int sno = 0;
                string query = "";
                double totalabsent = 0; string value = ""; double tot = 0; double val = 0; double totalabsents = 0;
                double totalpresent = 0;

                ArrayList arrColHdrNames = new ArrayList();
                ArrayList arrcolleavehead = new ArrayList();
                string[] dtfrom;
                dtfrom = Txtentryfrom.Text.Split('/');
                int date = Convert.ToInt32(dtfrom[0]);
                string month = dtfrom[1];
                string year = dtfrom[2];

                string atmonth = (month.TrimStart('0'));
                string monyear = atmonth + "/" + year;

                DateTime strstartdate = Convert.ToDateTime(dtfrom[1] + '/' + dtfrom[0] + '/' + dtfrom[2]).Date;


                string department = Convert.ToString(ddldept.SelectedItem.Value);
                DataTable dtstaffdeptleave = new DataTable();
                DataRow drow;
                string DEPTFILTER = string.Empty;
                if (ddldept.SelectedIndex != 0)
                    DEPTFILTER = " and st.dept_code='" + department + "'";
                if (ddlsession.SelectedValue == "M")
                {
                    query = "select count(sm.staff_code)StaffCount,hr.dept_name,st.dept_code  from stafftrans st,staffmaster sm,hrdept_master hr where st.staff_code=sm.staff_code and hr.dept_code=st.dept_code and st.latestrec=1  and ((sm.resign=0 and sm.settled=0)and (Discontinue =0 or Discontinue is null)) " + DEPTFILTER + " group by hr.dept_name,st.dept_code order by hr.dept_name";
                    query += " select distinct category,shortname,LeaveMasterPK from leave_category where college_code='" + Session["collegecode"].ToString() + "'";
                    query += " select [" + date + "] as date from staff_attnd sa,staffmaster sm,stafftrans st where sm.staff_code=sa.staff_code and st.staff_code=sm.staff_code and sm.resign='0' and sm.settled='0' and sa.staff_code=st.staff_code  and sa.mon_year='" + monyear + "' AND [" + date + "] NOT LIKE 'P-%'";
                }
                else if (ddlsession.SelectedValue == "E")
                {
                    query = "select count(sm.staff_code)StaffCount,hr.dept_name,st.dept_code  from stafftrans st,staffmaster sm,hrdept_master hr where st.staff_code=sm.staff_code and hr.dept_code=st.dept_code and st.latestrec=1  and ((sm.resign=0 and sm.settled=0)and (Discontinue =0 or Discontinue is null)) " + DEPTFILTER + " group by hr.dept_name,st.dept_code order by hr.dept_name";
                    query += " select distinct category,shortname,LeaveMasterPK from leave_category where college_code='" + Session["collegecode"].ToString() + "'";
                    query += " select [" + date + "] as date from staff_attnd sa,staffmaster sm,stafftrans st where sm.staff_code=sa.staff_code and st.staff_code=sm.staff_code and sm.resign='0' and sm.settled='0' and sa.staff_code=st.staff_code  and sa.mon_year='" + monyear + "' AND [" + date + "] NOT LIKE '%-P'";
                 
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    arrColHdrNames.Add("S.No");
                    dtstaffdeptleave.Columns.Add("Sno");
                    arrColHdrNames.Add("Name of the Department");
                    dtstaffdeptleave.Columns.Add("deptname");
                    arrColHdrNames.Add("Dept Staff Strength");
                    dtstaffdeptleave.Columns.Add("deptstfstrength");
                    arrColHdrNames.Add("A");
                    dtstaffdeptleave.Columns.Add("absent");
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        string clgshortname = ds.Tables[1].Rows[i]["shortname"].ToString();
                        string leavefk = ds.Tables[1].Rows[i]["LeaveMasterPK"].ToString();

                        arrColHdrNames.Add(clgshortname);
                        dtstaffdeptleave.Columns.Add(leavefk);
                    }
                    arrColHdrNames.Add("Total Absent");
                    dtstaffdeptleave.Columns.Add("totabs");
                    DataRow drHdr1 = dtstaffdeptleave.NewRow();
                    for (int grCol = 0; grCol < dtstaffdeptleave.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    }
                    int getcoulcount = (ds.Tables[1].Rows.Count) + 4;

                    dtstaffdeptleave.Rows.Add(drHdr1);

                    int totalcount = 0;
                    int count = 0;

                    string query1 = "select COUNT(sm.staff_code)staffCount,st.dept_code,[" + date + "]  from stafftrans st,staffmaster sm,staff_attnd sat,hrdept_master dt where dt.dept_code=st.dept_code and st.staff_code=sm.staff_code and latestrec='1' and sm.resign=0 and sm.settled=0  and sat.staff_code=st.staff_code and dt.college_code='" + Session["collegecode"].ToString() + "' and mon_year ='" + monyear + "' and [" + date + "] is not null " + DEPTFILTER + " group by st.dept_code,[" + date + "] ";
                    DataSet ds2 = new DataSet();
                    ds2.Clear();
                    ds2 = d2.select_method_wo_parameter(query1, "Text");

                    double totabs = 0;
                    double overallabscount = 0;
                    for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                    {

                        string dept = ds.Tables[0].Rows[rolcount]["dept_name"].ToString();
                        string deptcode = ds.Tables[0].Rows[rolcount]["dept_code"].ToString();


                        string staffcount = ds.Tables[0].Rows[rolcount]["StaffCount"].ToString();

                        double total = 0;
                        sno++;

                        count = Convert.ToInt32(staffcount);
                        totalcount = totalcount + count;

                        drow = dtstaffdeptleave.NewRow();
                        drow[0] = Convert.ToString(sno);
                        drow[1] = Convert.ToString(dept);
                        drow[2] = Convert.ToString(staffcount);

                        double abscount = 0;
                        if (ddlsession.SelectedValue == "M")
                        {
                            double.TryParse(Convert.ToString(ds2.Tables[0].Compute("Sum(staffCount)", "  dept_code in ('" + deptcode + "') and  [" + date + "] like'A-%'")), out abscount);
                            drow[3] = Convert.ToString(abscount);
                        }
                        else if (ddlsession.SelectedValue == "E")
                        {
                            double.TryParse(Convert.ToString(ds2.Tables[0].Compute("Sum(staffCount)", "  dept_code in ('" + deptcode + "') and  [" + date + "] like'%-A'")), out abscount);
                            drow[3] = Convert.ToString(abscount);
                        
                        }
                        totabs = totabs + abscount;
                        double totabsdept = 0;

                        for (int i = 4; i < dtstaffdeptleave.Columns.Count; i++)
                        {
                            string leavetype = Convert.ToString(dtstaffdeptleave.Columns[i]);
                            string leavetypeget = d2.GetFunction("select shortname from leave_category where LeaveMasterPK='" + leavetype + "' and college_code='" + Session["collegecode"] + "'");
                            double.TryParse(Convert.ToString(ds2.Tables[0].Compute("Sum(staffCount)", "  dept_code in ('" + deptcode + "') and  [" + date + "] like'" + leavetypeget + "-%'")), out totalabsent);
                            drow[i] = (Convert.ToString(totalabsent) == "0") ? "-" : Convert.ToString(totalabsent);

                            totabsdept = totabsdept + totalabsent;
                            total = total + totalabsent;
                            totalabsents = totalabsents + totalabsent;

                            if (total_leavetypeval.Contains(leavetypeget))
                            {
                                value = "";
                                value = total_leavetypeval[leavetypeget].ToString();
                                total_leavetypeval.Remove(leavetypeget);

                                tot = Convert.ToInt32(value) + Convert.ToInt32(totalabsent);
                                total_leavetypeval.Add(leavetypeget, tot);
                            }
                            else
                            {
                                total_leavetypeval.Add(leavetypeget, Convert.ToInt32(totalabsent));
                            }
                        }
                        totabsdept = totabsdept + abscount;
                        drow[getcoulcount] = Convert.ToString(totabsdept);
                        dtstaffdeptleave.Rows.Add(drow);
                        overallabscount = overallabscount + totabsdept;
                    }
                    drow = dtstaffdeptleave.NewRow();
                    drow[1] = "Total";
                    drow[2] = Convert.ToString(totalcount);
                    drow[3] = Convert.ToString(totabs);
                    for (int i = 4; i < dtstaffdeptleave.Columns.Count; i++)
                    {
                        string leavetype = Convert.ToString(dtstaffdeptleave.Columns[i]);
                        string leavetypeget = d2.GetFunction("select shortname from leave_category where LeaveMasterPK='" + leavetype + "' and college_code='" + Session["collegecode"] + "'");
                        if (total_leavetypeval.Count > 0)
                        {
                            value = "";
                            if (total_leavetypeval.Contains(leavetypeget))
                            {
                                value = total_leavetypeval[leavetypeget].ToString();
                            }
                            else
                            {
                                value = "0";
                            }

                        }

                        drow[i] = Convert.ToString(value);
                    }
                    drow[getcoulcount] = Convert.ToString(overallabscount);
                    dtstaffdeptleave.Rows.Add(drow);

                }
                grddepartmentwiseLeave.DataSource = dtstaffdeptleave;
                grddepartmentwiseLeave.DataBind();
                grddepartmentwiseLeave.Visible = true;
                int totcoulcount = grddepartmentwiseLeave.Rows.Count;
                totcoulcount--;
                for (int i = 0; i < grddepartmentwiseLeave.Rows.Count; i++)
                {
                    for (int j = 0; j < grddepartmentwiseLeave.HeaderRow.Cells.Count; j++)
                    {
                        if (j == 0)
                        {
                            grddepartmentwiseLeave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            grddepartmentwiseLeave.Rows[i].Cells[j].Width = 40;

                        }
                        else if (j == 1)
                        {
                            grddepartmentwiseLeave.Rows[i].Cells[j].Width = 200;
                            grddepartmentwiseLeave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;
                        }
                        else if (j == 2)
                        {
                            grddepartmentwiseLeave.Rows[i].Cells[j].Width = 50;
                            grddepartmentwiseLeave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                        }
                        else if (j == 3)
                        {
                            grddepartmentwiseLeave.Rows[i].Cells[j].Width = 50;
                            grddepartmentwiseLeave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                        }
                        else
                        {
                            grddepartmentwiseLeave.Rows[i].Cells[j].Width = 50;
                            grddepartmentwiseLeave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                        }
                     
                        if (i == 0)
                        {
                            grddepartmentwiseLeave.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            grddepartmentwiseLeave.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            grddepartmentwiseLeave.Rows[i].Cells[j].BorderColor = Color.Black;
                            grddepartmentwiseLeave.Rows[i].Cells[j].Font.Bold = true;
                            grddepartmentwiseLeave.Rows[i].Cells[j].Font.Name = "Book Antiqua";
                            grddepartmentwiseLeave.Rows[i].Cells[j].Font.Size = FontUnit.Medium;

                        }
                    }

                }

                DataTable dtleave = new DataTable();
                DataSet checkstf = new DataSet();

                string attendencequery = "select m.staff_code,staff_name,h.dept_acronym,s.category_name,d.desig_acronym from staffmaster m,stafftrans t,desig_master d,hrdept_master h,staffcategorizer s where  t.staff_code=m.staff_code  and t.desig_code=d.desig_code and h.dept_code=t.dept_code and s.category_code=t.category_code and m.college_code = d.collegeCode and m.college_code = h.college_code and s.college_code = m.college_code   and t.latestrec = 1 and ((resign=0 and settled =0) and (Discontinue =0 or Discontinue is null))";
                attendencequery = attendencequery + " and m.college_code in('" + Session["collegecode"] + "')";
                checkstf = d2.select_method_wo_parameter(attendencequery, "text");
                DataView dv = new DataView();
                if (checkstf.Tables[0].Rows.Count > 0)
                {
                    arrcolleavehead.Add("S.No");
                    dtleave.Columns.Add("Sno");
                    arrcolleavehead.Add("Staff Code");
                    dtleave.Columns.Add("staffcode");
                    arrcolleavehead.Add("Staff Name");
                    dtleave.Columns.Add("staffname");
                    arrcolleavehead.Add("Staff Category");
                    dtleave.Columns.Add("staffcategory");
                    arrcolleavehead.Add("Department");
                    dtleave.Columns.Add("department");
                    arrcolleavehead.Add("Designation");
                    dtleave.Columns.Add("designation");
                    arrcolleavehead.Add("Leave Reason");
                    dtleave.Columns.Add("leavereason");

                    DataRow drHdr = dtleave.NewRow();
                    for (int grCol = 0; grCol < dtleave.Columns.Count; grCol++)
                    {
                        drHdr[grCol] = arrcolleavehead[grCol];
                    }
                    dtleave.Rows.Add(drHdr);
                    DataSet dsleav = new DataSet();
                    DataSet abscountds = new DataSet();
                    string absquery = string.Empty;
                    if (ddlsession.SelectedValue == "M")
                    {

                        absquery = "select sm.staff_code from staff_attnd sa,staffmaster sm,stafftrans st where sm.staff_code=sa.staff_code and st.staff_code=sm.staff_code and sm.resign='0' and sm.settled='0' and sa.staff_code=st.staff_code  and sa.mon_year='" + monyear + "' AND [" + date + "] like'A-%'";
                        abscountds = d2.select_method_wo_parameter(absquery, "text");
                    }
                    else if (ddlsession.SelectedValue == "E")
                    {
                        absquery = "select sm.staff_code from staff_attnd sa,staffmaster sm,stafftrans st where sm.staff_code=sa.staff_code and st.staff_code=sm.staff_code and sm.resign='0' and sm.settled='0' and sa.staff_code=st.staff_code  and sa.mon_year='" + monyear + "' AND [" + date + "] like'%-A'";
                        abscountds = d2.select_method_wo_parameter(absquery, "text");
                    
                    }
                    if (abscountds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drHdr1 = dtleave.NewRow();
                        drHdr1[0] = Convert.ToString("Leave Type A");
                        dtleave.Rows.Add(drHdr1);
                        int sgno = 0;
                        for (int lv = 0; lv < abscountds.Tables[0].Rows.Count; lv++)
                        {
                            string stfcode = Convert.ToString(abscountds.Tables[0].Rows[lv]["staff_code"]);
                            checkstf.Tables[0].DefaultView.RowFilter = "staff_code ='" + stfcode + "'";
                            dv = checkstf.Tables[0].DefaultView;
                            string reason = d2.GetFunction("select  (select mastervalue from co_mastervalues where mastercode=gatereqreason)Reason ,convert(varchar(10),leavefrom,103)leavefrom,convert(varchar(10),leaveto,103)leaveto,datediff(dd,leavefrom,leaveto) LeaveDaysCount from rq_requisition rq,staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and sa.appl_id=rq.ReqAppNo and requesttype='5' and rq.reqAppstatus=1 AND sm.staff_code='" + stfcode + "' and '" + strstartdate + "' between leavefrom and leaveto");
                            string getreason = string.Empty;
                            if (reason == "0" || reason == "")
                            {
                                getreason = "Not Entered";
                            }
                            else
                            {
                                getreason = reason;
                            }
                            if (dv.Count > 0)
                            {
                                DataRow drrec = dtleave.NewRow();
                                sgno++;
                                string getstaffcode = dv[0]["staff_code"].ToString();
                                string getstaffname = dv[0]["staff_name"].ToString();
                                string getcate = dv[0]["category_name"].ToString();
                                string getdeptacr = dv[0]["dept_acronym"].ToString();
                                string designacr = dv[0]["desig_acronym"].ToString();

                                drrec["Sno"] = Convert.ToString(sgno);
                                drrec["staffcode"] = Convert.ToString(getstaffcode);
                                drrec["staffname"] = Convert.ToString(getstaffname);
                                drrec["staffcategory"] = Convert.ToString(getcate);
                                drrec["department"] = Convert.ToString(getdeptacr);
                                drrec["designation"] = Convert.ToString(designacr);
                                drrec["leavereason"] = Convert.ToString(getreason);
                                dtleave.Rows.Add(drrec);
                            }
                        }
                    
                    }
                 
                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                    {
                        string clgshortname = ds.Tables[1].Rows[j]["shortname"].ToString();
                        string leavefk = ds.Tables[1].Rows[j]["LeaveMasterPK"].ToString();

                        
                        int snum = 0;
                        string quer=string.Empty;

                        if (ddlsession.SelectedValue == "M")
                        {
                            quer = "select sm.staff_code from staff_attnd sa,staffmaster sm,stafftrans st where sm.staff_code=sa.staff_code and st.staff_code=sm.staff_code and sm.resign='0' and sm.settled='0' and sa.staff_code=st.staff_code  and sa.mon_year='" + monyear + "' AND [" + date + "] like'" + clgshortname + "-%' ";
                            dsleav = d2.select_method_wo_parameter(quer, "text");
                        }
                        else if (ddlsession.SelectedValue == "E")
                        {
                            quer = "select sm.staff_code from staff_attnd sa,staffmaster sm,stafftrans st where sm.staff_code=sa.staff_code and st.staff_code=sm.staff_code and sm.resign='0' and sm.settled='0' and sa.staff_code=st.staff_code  and sa.mon_year='" + monyear + "' AND [" + date + "] like'%-" + clgshortname + "'";
                            dsleav = d2.select_method_wo_parameter(quer, "text");
                        }

                        if (dsleav.Tables[0].Rows.Count > 0)
                        {
                            DataRow drHdr1 = dtleave.NewRow();
                            drHdr1[0] = Convert.ToString("Leave Type " + clgshortname);
                            dtleave.Rows.Add(drHdr1);
                            int snumb = 0;
                            for (int values = 0; values < dsleav.Tables[0].Rows.Count; values++)
                            {
                                string staffcode = Convert.ToString(dsleav.Tables[0].Rows[values]["staff_code"]);
                                checkstf.Tables[0].DefaultView.RowFilter = "staff_code ='" + staffcode + "'";
                                dv = checkstf.Tables[0].DefaultView;
                                string reason = d2.GetFunction("select  (select mastervalue from co_mastervalues where mastercode=gatereqreason)Reason ,convert(varchar(10),leavefrom,103)leavefrom,convert(varchar(10),leaveto,103)leaveto,datediff(dd,leavefrom,leaveto) LeaveDaysCount from rq_requisition rq,staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and sa.appl_id=rq.ReqAppNo and requesttype='5' and rq.reqAppstatus=1 AND sm.staff_code='" + staffcode + "' and '" + strstartdate + "' between leavefrom and leaveto");
                                string getreason = string.Empty;
                                if (reason == "0" || reason == "")
                                {
                                    getreason = "Not Entered";
                                }
                                else
                                {
                                    getreason = reason;
                                }

                                if (dv.Count > 0)
                                {
                                    DataRow drrec = dtleave.NewRow();
                                    snumb++;
                                    string getstaffcode = dv[0]["staff_code"].ToString();
                                    string getstaffname = dv[0]["staff_name"].ToString();
                                    string getcate = dv[0]["category_name"].ToString();
                                    string getdeptacr = dv[0]["dept_acronym"].ToString();
                                    string designacr = dv[0]["desig_acronym"].ToString();

                                    drrec["Sno"] = Convert.ToString(snumb);
                                    drrec["staffcode"] = Convert.ToString(getstaffcode);
                                    drrec["staffname"] = Convert.ToString(getstaffname);
                                    drrec["staffcategory"] = Convert.ToString(getcate);
                                    drrec["department"] = Convert.ToString(getdeptacr);
                                    drrec["designation"] = Convert.ToString(designacr);
                                    drrec["leavereason"] = Convert.ToString(getreason);
                                    dtleave.Rows.Add(drrec);
                                }
                            }

                        }
                       

                    }


                }
                grdleavedetails.DataSource = dtleave;
                grdleavedetails.DataBind();
                grdleavedetails.Visible = true;


                for (int i = 0; i < grdleavedetails.Rows.Count; i++)
                {
                    for (int j = 0; j < grdleavedetails.HeaderRow.Cells.Count; j++)
                    {
                        if (j == 0)
                        {
                            grdleavedetails.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            grdleavedetails.Rows[i].Cells[j].Width = 40;

                        }
                        else if (j == 1)
                        {
                               grdleavedetails.Rows[i].Cells[j].Width = 100;
                        }
                        else if (j == 2)
                        {
                            grdleavedetails.Rows[i].Cells[j].Width = 150;
                        }
                        else if (j == 3)
                        {
                            grdleavedetails.Rows[i].Cells[j].Width = 300;
                        }
                        else if (j == 4)
                        {
                            grdleavedetails.Rows[i].Cells[j].Width = 150;
                        }
                        if (i == 0)
                        {
                            grdleavedetails.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                            grdleavedetails.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            grdleavedetails.Rows[i].Cells[j].BorderColor = Color.Black;
                            grdleavedetails.Rows[i].Cells[j].Font.Bold = true;
                            grdleavedetails.Rows[i].Cells[j].Font.Name = "Book Antiqua";
                            grdleavedetails.Rows[i].Cells[j].Font.Size = FontUnit.Medium;

                        }
                        else
                        {
                            
                            int colspan = 1;

                            if (j == 0)
                            {
                                while (grdleavedetails.Rows[i].Cells[j].Text != "&nbsp;" && grdleavedetails.Rows[i].Cells[j + colspan].Text == "&nbsp;")
                                {
                                    colspan++;
                                    if (grdleavedetails.HeaderRow.Cells.Count == j + colspan)
                                        break;

                                }
                            }

                            if (colspan != 1)
                            {
                                grdleavedetails.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                grdleavedetails.Rows[i].Cells[j].BackColor =Color.LightPink; //ColorTranslator.FromHtml("#0CA6CA");
                                grdleavedetails.Rows[i].Cells[j].ColumnSpan = colspan;
                                for (int a = j + 1; a < j + colspan; a++)
                                    grdleavedetails.Rows[i].Cells[a].Visible = false;
                            }

                        }

                    }

                }

            }

        }
        catch (Exception ex)
        {

        }


    }
    protected void ddldepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddldept.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddldept.SelectedItem.Value);
            }


        }
        catch (Exception ex)
        {

        }
    }

    protected void binddept()
    {
        ddldept.Visible = true;
        ddldept.Items.Clear();
        ds.Clear();
        ListItem lsitem = new ListItem();

        Hashtable hat = new Hashtable();
        string deptquery = "";
        string singleuser = Session["single_user"].ToString();
        if (singleuser == "True")
        {
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name";
        }

        else
        {

            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name";
        }
        if (deptquery != "")
        {
            ds = d2.select_method(deptquery, hat, "Text");
            ddldept.DataSource = ds.Tables[0];
            ddldept.DataTextField = "dept_name";
            ddldept.DataValueField = "dept_code";
            ddldept.DataBind();
            lsitem.Text = "All";
            ddldept.Items.Insert(0, lsitem);

        }

    }

    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        cellclick = true;
    }


    protected void Fpspread1_ButtonCommand(object sender, EventArgs e)
    {
        try
        {
            if (Fpspread1.Sheets[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(Fpspread1.Sheets[0].Cells[0, 1].Value) == 1)
                {
                    for (int intF = 0; intF < Fpspread1.Sheets[0].Rows.Count; intF++)
                    {
                        Fpspread1.Sheets[0].Cells[intF, 1].Value = 1;
                    }
                }
                else
                {
                    for (int intF = 0; intF < Fpspread1.Sheets[0].Rows.Count; intF++)
                    {
                        Fpspread1.Sheets[0].Cells[intF, 1].Value = 0;
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
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Department wise Attendance Report";
            string pagename = "DepartmentWise_Attendance_Report.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
            lblvalidation1.Visible = false;
        }
        catch
        {

        }

    }

    protected void Validate_Date(object sender, EventArgs e)
    {

        string[] dtfrom;
        dtfrom = Txtentryfrom.Text.Split('/');
        int date = Convert.ToInt32(dtfrom[0]);
        string month = dtfrom[1];
        string year = dtfrom[2];
        // string atdate = (date.TrimStart('0'));
        string atmonth = (month.TrimStart('0'));
        string monyear = atmonth + "/" + year;

        DateTime strstartdate = Convert.ToDateTime(dtfrom[1] + '/' + dtfrom[0] + '/' + dtfrom[2]).Date;
        if (strstartdate > DateTime.Today)
        {
            Fpspread1.Visible = false;
            rptprint.Visible = false;
            lblnorec.Visible = true;
            lblnorec.Text = "Please Enter the Date Less Than or Equal to Today's Date";


        }
        else
        {
            lblnorec.Visible = false;
        }
    }
    protected void rdbformate1_changed(object sender, EventArgs e)
    {
        if (rdbformate1.Checked == true)
        {
            rdbformate2.Checked = false;
        }

    }

    protected void rdbformate2_changed(object sender, EventArgs e)
    {
        if (rdbformate2.Checked == true)
        {
            rdbformate1.Checked = false;
        }
    }

    protected void grddepartmentwiseLeave_RowDataBound(object sende, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (e.Row.RowIndex == 0)
                //{
                //    e.Row.BackColor = Color.FromArgb(12, 166, 202);
                //    e.Row.HorizontalAlign = HorizontalAlign.Center;
                //    e.Row.Width = 200;
                //    e.Row.Font.Bold = true;
                //}
                //e.Row.Cells[0].Width = 50;
                //e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                //if (e.Row.RowIndex != 0)
                //{
                //}
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void grdleavedetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    if (e.Row.RowIndex == 0)
            //    {
            //        e.Row.BackColor = Color.FromArgb(12, 166, 202);
            //        e.Row.HorizontalAlign = HorizontalAlign.Center;
            //        e.Row.Width = 200;
            //        e.Row.Font.Bold = true;
            //    }
            //    e.Row.Cells[0].Width = 50;
            //    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            //    if (e.Row.RowIndex != 0)
            //    {
            //    }
            //}
        }
        catch (Exception ex)
        {

        }


    }
}