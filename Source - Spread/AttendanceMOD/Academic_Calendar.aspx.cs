using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AttendanceMOD_Academic_Calendar : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    static Boolean forschoolsetting = false;
    Hashtable hat = new Hashtable();
    Hashtable has = new Hashtable();
    DataSet ds_load = new DataSet();
    DAccess2 daccess = new DAccess2();
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("AttendanceHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/AttendanceMOD/AttendanceHome.aspx");
                    return;
                }
            }

            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            // Session["Semester"] =Convert.ToString(ddlSem.SelectedValue);

            if (!IsPostBack)
            {
                //'--------------------------------------
                collegecode = Session["collegecode"].ToString();
                usercode = Session["usercode"].ToString();
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyy");
                txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyy");
                //Lblreport.Visible = false;
                //txtexcl.Visible = false;
                //btnprnt.Visible = false;
                //btnxcl.Visible = false;
                //lblexer.Visible = false;
                colg();
                acyear();
                binddegree();
                bindbranch();
                binddate();
                batch();
                sem();
                ddlreason.Attributes.Add("onfocus", "frelig()");
                // bindsem();
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                // Added By Sridharan 12 Mar 2015
                //{
                DataSet schoolds = new DataSet();
                string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = daccess.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                    if (schoolvalue.Trim() == "0")
                    {
                        forschoolsetting = true;
                        Lblclg.Text = "School";
                        lblBatch.Text = "Year";
                        lblDegree.Text = "School Type";
                        lblBranch.Text = "Standard";
                        //lblsem.Text = "Term";
                        //lblDegree.Attributes.Add("style", " width: 95px;");
                        //lblBranch.Attributes.Add("style", " width: 67px;");
                        //ddlBranch.Attributes.Add("style", " width: 241px;");
                    }
                    else
                    {
                        forschoolsetting = false;
                    }
                }
                //} Sridharan
            }
            // lblexer.Visible = false;
        }
        catch
        {
        }
    }
    public void acyear()
    {
        try
        {

            DataSet dsAcd = batchLoad();
            DataRow drYEar;
            if (dsAcd.Tables.Count > 0 && dsAcd.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsAcd.Tables[0].Rows.Count; row++)
                {
                    int yeaR = 0;
                    int.TryParse(Convert.ToString(dsAcd.Tables[0].Rows[row]["batch_year"]), out yeaR);
                    ddlBatch.Items.Insert(row, Convert.ToString(yeaR) + "-" + Convert.ToString(++yeaR));

                }
            }
        }
        catch
        {
        }
    }
    protected DataSet batchLoad()
    {
        DataSet dsBatch = new DataSet();
        try
        {
            string strsql = "select distinct batch_year from applyn where batch_year<>'-1' and batch_year<>''order by batch_year desc";
            dsBatch = d2.select_method_wo_parameter(strsql, "Text");
        }
        catch { dsBatch.Clear(); }
        return dsBatch;
    }

    public void colg()
    {

        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }

        string group_code = Session["group_code"].ToString();
        string columnfield = "";
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
        ds_load = d2.select_method("bind_college", hat, "sp");
        ddlclg.Items.Clear();
        string clgname = "select college_code,collname from collinfo ";
       // if (clgname != "")
       // {
           // ds_load = daccess.select_method(clgname, hat, "Text");
        if (ds_load.Tables[0].Rows.Count > 0)
        {
            ddlclg.DataSource = ds_load;
            ddlclg.DataTextField = "collname";
            ddlclg.DataValueField = "college_code";
            ddlclg.DataBind();
        }

    }
    public void bindbatch()
    {
        ddlBatch.Items.Clear();
        ds_load = daccess.select_method_wo_parameter("bind_batch", "sp");
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
        }
    }
    public void bindbranch()
    {
        cbl_branch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Convert.ToString(ddlclg.SelectedValue);
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }

        string typ = string.Empty;
        if (cbl_degree.Items.Count > 0)
        {
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (typ == "")
                    {
                        typ = "" + cbl_degree.Items[i].Value + "";
                    }
                    else
                    {
                        typ = typ + "" + "," + "" + cbl_degree.Items[i].Value + "";
                    }
                }

            }
        }
        if (typ != "")
        {
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", typ);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

            ds_load = d2.BindBranchMultiple(singleuser, group_user, typ, collegecode, usercode);
         //   ds_load = daccess.select_method("bind_branch", hat, "sp");
            int count2 = ds_load.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                cbl_branch.DataSource = ds_load;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
            }
        }
        binddate();
    }
    public void binddegree()
    {
        cbl_degree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Convert.ToString(ddlclg.SelectedValue);
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load = daccess.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            cbl_degree.DataSource = ds_load;
            cbl_degree.DataTextField = "course_name";
            cbl_degree.DataValueField = "course_id";
            cbl_degree.DataBind();
        }
    }


    public void ddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
    }
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
                txt_degree.Text = "--Select--";
            }
            bindbranch();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_degree.Text = "--Select--";
            cb_degree.Checked = false;
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_degree.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
            }
            bindbranch();
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch.Checked == true)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            binddate();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cbl_branch.Items.Count)
                {
                    cb_branch.Checked = true;
                }
            }
            binddate();
        }
        catch (Exception ex)
        {
        }
    }
  
    public void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            gview.Visible = false;
            Hashtable headrow = new Hashtable();
            Hashtable headyear = new Hashtable();
            Hashtable headholi = new Hashtable();
            DataTable dtyear = new DataTable();
            DataRow dyear;
            dtyear.Columns.Add("Date01");
            dtyear.Columns.Add("Day01");
            dtyear.Columns.Add("Holiday01");
            dtyear.Columns.Add("Holi_day01");
            dtyear.Columns.Add("Date02");
            dtyear.Columns.Add("Day02");
            dtyear.Columns.Add("Holiday02");
            dtyear.Columns.Add("Holi_day02");
            dtyear.Columns.Add("Date03");
            dtyear.Columns.Add("Day03");
            dtyear.Columns.Add("Holiday03");
            dtyear.Columns.Add("Holi_day03");
            dtyear.Columns.Add("Date04");
            dtyear.Columns.Add("Day04");
            dtyear.Columns.Add("Holiday04");
            dtyear.Columns.Add("Holi_day04");
            dtyear.Columns.Add("Date05");
            dtyear.Columns.Add("Day05");
            dtyear.Columns.Add("Holiday05");
            dtyear.Columns.Add("Holi_day05");
            dtyear.Columns.Add("Date06");
            dtyear.Columns.Add("Day06");
            dtyear.Columns.Add("Holiday06");
            dtyear.Columns.Add("Holi_day06");
            dtyear.Columns.Add("Date07");
            dtyear.Columns.Add("Day07");
            dtyear.Columns.Add("Holiday07");
            dtyear.Columns.Add("Holi_day07");
            dtyear.Columns.Add("Date08");
            dtyear.Columns.Add("Day08");
            dtyear.Columns.Add("Holiday08");
            dtyear.Columns.Add("Holi_day08");
            dtyear.Columns.Add("Date09");
            dtyear.Columns.Add("Day09");
            dtyear.Columns.Add("Holiday09");
            dtyear.Columns.Add("Holi_day09");
            dtyear.Columns.Add("Date10");
            dtyear.Columns.Add("Day10");
            dtyear.Columns.Add("Holiday10");
            dtyear.Columns.Add("Holi_day10");
            dtyear.Columns.Add("Date11");
            dtyear.Columns.Add("Day11");
            dtyear.Columns.Add("Holiday11");
            dtyear.Columns.Add("Holi_day11");
            dtyear.Columns.Add("Date12");
            dtyear.Columns.Add("Day12");
            dtyear.Columns.Add("Holiday12");
            dtyear.Columns.Add("Holi_day12");
            string typ1 = string.Empty;
            if (cbl_branch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + cbl_branch.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + cbl_branch.Items[i].Value + "";
                        }
                    }

                }
            }
            string quer=string.Empty;
            int datre = 0;
            string sql = "  select top 1 s.batch_year , Dept_Name,batch_year,d.degree_code,CONVERT(varchar,start_date,103) as start_date,CONVERT(datetime,start_date,103) as st_date,CONVERT(datetime,end_date,103) as en_date,CONVERT(varchar,end_date,103) as end_date,s.semester,no_of_working_Days,no_of_working_hrs,schOrder,starting_dayorder,nodays  from Degree d,Department de,course c,seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and s.semester=p.semester and p.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code=de.college_code and s.degree_code in('" + typ1 + "')  and batch_year in('" + Lbbatch.Text + "')   and s.semester in('" + lbsem.Text + "') and c.college_code='" + Convert.ToString(ddlclg.SelectedValue) + "' order by   c.college_code,s.batch_year asc ,s.semester desc,d.Degree_Code,datepart(year,start_date) desc , datepart(month ,start_date) desc";
            
            DataSet dsattabsent = daccess.select_method_wo_parameter(sql, "text");
            if (dsattabsent.Tables.Count > 0 && dsattabsent.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsattabsent.Tables[0].Rows.Count; i++)
                {
                    string batch_year = Convert.ToString(dsattabsent.Tables[0].Rows[i]["batch_year"]);
                    string degree_code = Convert.ToString(dsattabsent.Tables[0].Rows[i]["degree_code"]);
                    string semester = Convert.ToString(dsattabsent.Tables[0].Rows[i]["semester"]);
                    string start_dayorder = Convert.ToString(dsattabsent.Tables[0].Rows[i]["starting_dayorder"]);
                    string nodays = Convert.ToString(dsattabsent.Tables[0].Rows[i]["nodays"]);
                    string sch = Convert.ToString(dsattabsent.Tables[0].Rows[0]["schOrder"]);
                    DateTime st_date = Convert.ToDateTime(dsattabsent.Tables[0].Rows[0]["st_date"]);
                    DateTime semst_date = Convert.ToDateTime(dsattabsent.Tables[0].Rows[0]["st_date"]);
                    DateTime en_date = Convert.ToDateTime(dsattabsent.Tables[0].Rows[0]["en_date"]);
                    int s=0;
                    string[] sps = st_date.ToString().Split('/');
                    string semdate = sps[0] + '/' + sps[1] + '/' + sps[2];
                    string Day_Order = string.Empty;
                    string double_dayorder = string.Empty;
                    headrow.Clear();
                    headyear.Clear();
                    headholi.Clear();
                    int headsn = 0;
                     int holidays=0;
                    string strdayoredr = "Select * from tbl_consider_day_order where degree_code=" + degree_code + " and semester=" + semester + " and batch_year=" + batch_year + "  and ((From_Date between '" + st_date.ToString("yyyy-MM-dd") + "' and '" + en_date.ToString("yyyy-MM-dd") + "') or (To_Date between '" + st_date.ToString("yyyy-MM-dd") + "' and '" + en_date.ToString("yyyy-MM-dd") + "'))";
                    DataSet dsdayorder = d2.select_method_wo_parameter(strdayoredr, "Text");
                    Hashtable hatdoc = new Hashtable();
                    Dictionary<DateTime, byte> dicAlternateDayOrder = new Dictionary<DateTime, byte>();
                    for (int doc = 0; doc < dsdayorder.Tables[0].Rows.Count; doc++)
                    {
                        DateTime dtsdoc = Convert.ToDateTime(dsdayorder.Tables[0].Rows[doc]["from_date"].ToString());
                        DateTime dtedoc = Convert.ToDateTime(dsdayorder.Tables[0].Rows[doc]["to_date"].ToString());
                        string reason = dsdayorder.Tables[0].Rows[doc]["Reason"].ToString();
                        string alternateDayOrder = Convert.ToString(dsdayorder.Tables[0].Rows[doc]["DayOrder"]).Trim();
                        byte alternateDay = 0;
                       
                        byte.TryParse(alternateDayOrder, out alternateDay);
                        for (DateTime dtc = dtsdoc; dtc <= dtedoc; dtc = dtc.AddDays(1))
                        {
                            if (!hatdoc.Contains(dtc))
                            {
                                hatdoc.Add(dtc, reason);
                            }
                            if (!dicAlternateDayOrder.ContainsKey(dtc))
                            {
                                dicAlternateDayOrder.Add(dtc, alternateDay);
                            }
                        }
                    }
                    string mons = st_date.ToString("MM");
              st_date= st_date.AddDays(-1);
              Boolean doubleday1 = false;
              string startmons = string.Empty;
              string nextmon = string.Empty;
              string months = st_date.ToString("MM");
              string coldt = string.Empty;
              int stcol = 0;
              int daycun = 0;
              if (mons != months)
                  st_date = st_date.AddDays(1);
                    while (st_date <= en_date)
                    {
                       
                        string mon = st_date.ToString("MM");

                        string munth = st_date.ToString("M");
                        string year = st_date.ToString("yyyy");
                        string date = st_date.ToString("dd");
                        string day = st_date.ToString("ddd");
                        string weekday=st_date.ToString("dddd");
                        int col = 0;
                        int.TryParse(mon, out col);
                       
                        if (!headrow.ContainsValue(col))
                        {
                            headsn++;
                            headrow.Add(headsn, col);
                            headyear.Add(headsn, year);
                        }
                        int row = 0;
                        int.TryParse(date, out row);
                        if (sch == "1")
                        {
                            if (s == 0)
                            {
                                int.TryParse(date, out datre);

                                for (int j = 1; j <= datre; j++)
                                {
                                    startmons = mon;
                                    if (stcol == 0)
                                    {
                                        stcol++;
                                        if (stcol <= 9)
                                            coldt = "0" + stcol;
                                        else
                                            coldt = Convert.ToString(stcol);
                                    }
                                    string[] sp = st_date.ToString().Split('/');
                                    string curdate = mon + '/' + j + '/' + sp[2];
                                    DateTime starday = Convert.ToDateTime(curdate);
                                    string day1 = starday.ToString("ddd");

                                    Day_Order = "0-" + Convert.ToString(day1);
                                    if (dtyear.Rows.Count < j)
                                    {
                                        dyear = dtyear.NewRow();
                                        dtyear.Rows.Add(dyear);
                                    }
                                    dtyear.Rows[j - 1]["date" + coldt + ""] = j;
                                    dtyear.Rows[j - 1]["day" + coldt + ""] = day1;
                                    dtyear.Rows[j - 1]["Holiday" + coldt + ""] = "";
                                    dtyear.Rows[j - 1]["Holi_day" + coldt + ""] = "";
                                }
                                s = 1;
                            }
                            else
                            {
                                if (startmons != mon)
                                {
                                    startmons = mon;
                                    stcol++;
                                    if (stcol <= 9)
                                        coldt = "0" + stcol;
                                    else
                                        coldt = Convert.ToString(stcol);
                                }
                                if (dtyear.Rows.Count < row)
                                {
                                    dyear = dtyear.NewRow();
                                    dtyear.Rows.Add(dyear);
                                }
                                dtyear.Rows[row - 1]["date" + coldt + ""] = date;
                                dtyear.Rows[row - 1]["day" + coldt + ""] = day;
                                
                                //dyear["date" + mon + ""] = date;
                                //dyear["day" + mon + ""] = day;
                                string holi = d2.GetFunction("select holiday_desc from holidaystudents where degree_code in('" + degree_code + "')   and semester=" + semester + " and holiday_date='" + st_date + "'");
                                if (holi != "" && holi != "0")
                                {
                                    holidays++;
                                    dtyear.Rows[row - 1]["Holiday" + coldt + ""] = holi;
                                    dtyear.Rows[row - 1]["Holi_day" + coldt + ""] = st_date;
                                    headholi.Add(holidays, row - 1 + "-" + coldt + "");
                                }
                                else
                                {
                                    dtyear.Rows[row - 1]["Holiday" + coldt + ""] = weekday;
                                    dtyear.Rows[row - 1]["Holi_day" + coldt + ""] = st_date;

                                }
                                if (st_date == en_date)
                                {
                                    st_date = st_date.AddDays(1);
                                    date = st_date.ToString("dd");
                                    int.TryParse(date, out datre);
                                     string monthyear = st_date.ToString("MM");
                                     if (mon == monthyear)
                                     {
                                         for (int j = datre; j <= 30; j++)
                                         {
                                             if (startmons != mon)
                                             {
                                                 startmons = mon;
                                                 stcol++;
                                                 if (stcol <= 9)
                                                     coldt = "0" + stcol;
                                                 else
                                                     coldt = Convert.ToString(stcol);
                                             }
                                             date = Convert.ToString(j);
                                             string[] sp = st_date.ToString().Split('/');
                                             string curdate = mon + '/' + j + '/' + sp[2];
                                             DateTime starday = Convert.ToDateTime(curdate);
                                             string mon1 = starday.ToString("MM");
                                             string munth1 = starday.ToString("M");
                                             string date1 = starday.ToString("dd");
                                             string day1 = starday.ToString("ddd");
                                             if (dtyear.Rows.Count < j)
                                             {
                                                 dyear = dtyear.NewRow();
                                                 dtyear.Rows.Add(dyear);
                                             }
                                             //dyear["date" + mon + ""] = j;
                                             //dyear["day" + mon + ""] = day1;
                                             //dyear["Holiday" + mon + ""] = "";
                                             dtyear.Rows[j - 1]["date" + coldt + ""] = j;
                                             dtyear.Rows[j - 1]["day" + coldt + ""] = day1;
                                             dtyear.Rows[j - 1]["Holiday" + coldt + ""] = "";
                                             //dtyear.Rows.Add(dyear);
                                             dtyear.Rows[row - 1]["Holi_day" + coldt + ""] = "";
                                         }
                                     }

                                }
                            }
                        }
                        else
                        {
                            if (s == 0)
                            {
                                int.TryParse(date, out datre);

                                for (int j = 1; j <= datre; j++)
                                {
                                    if (startmons != mon)
                                    {
                                        startmons = mon;
                                        stcol++;
                                        if (stcol <= 9)
                                        {
                                             coldt = "0" + stcol;
                                            //int.TryParse(coldt, out stcol);
                                            //stcol = Convert.ToInt32("0" + stcol);
                                        }
                                        else
                                            coldt = Convert.ToString(stcol);
                                    }
                                    string[] sp = st_date.ToString().Split('/');
                                    string curdate = mon + '/' + j + '/' + sp[2];
                                    DateTime starday = Convert.ToDateTime(curdate);
                                    string mon1 = starday.ToString("MM");
                                    string munth1 = starday.ToString("M");
                                    string date1 = starday.ToString("dd");
                                    string day1 = starday.ToString("ddd");
                                    if (dtyear.Rows.Count < j)
                                    {
                                        dyear = dtyear.NewRow();
                                        dtyear.Rows.Add(dyear);
                                    }
                                    //dyear["date" + mon + ""] = j;
                                    //dyear["day" + mon + ""] = day1;
                                    //dyear["Holiday" + mon + ""] = "";
                                    dtyear.Rows[j - 1]["date" + coldt + ""] = j;
                                    dtyear.Rows[j - 1]["day" + coldt + ""] = day1;
                                    dtyear.Rows[j - 1]["Holiday" + coldt + ""] = "";
                                    dtyear.Rows[j - 1]["Holi_day" + coldt + ""] = "";
                                    //dtyear.Rows.Add(dyear);
                                }
                                s = 1;
                            }
                            else
                            {
                                if (startmons != mon)
                                {
                                    startmons = mon;
                                    stcol++;
                                    if (stcol <= 9)
                                        coldt = "0" + stcol;
                                    else
                                        coldt = Convert.ToString(stcol);
                                }
                                if (dtyear.Rows.Count < row)
                                {
                                    dyear = dtyear.NewRow();
                                    dtyear.Rows.Add(dyear);
                                }
                                dtyear.Rows[row - 1]["date" + coldt + ""] = date;
                                dtyear.Rows[row - 1]["day" + coldt + ""] = day;
                                
                                //dyear["date" + mon + ""] = date;
                                //dyear["day" + mon + ""] = day;
                                string holi = d2.GetFunction("select holiday_desc from holidaystudents where degree_code in('" + degree_code + "')   and semester=" + semester + " and holiday_date='" + st_date + "'");
                                if (holi != "" && holi != "0")
                                {
                                    holidays++;
                                    dtyear.Rows[row - 1]["Holiday" + coldt + ""] = holi;
                                    dtyear.Rows[row - 1]["Holi_day" + coldt + ""] = st_date;
                                    headholi.Add(holidays, row - 1 + "-" + coldt + "");
                                }
                                // dyear["Holiday" + mon + ""] = holi;
                                else
                                {
                                    daycun++;
                                    DateTime st_semdate = st_date;
                                    if (doubleday1 == true)
                                        st_semdate = st_date.AddDays(+1);
                                    string[] sp = st_semdate.ToString().Split('/');
                                    string curdate = sp[0] + '/' + sp[1] + '/' + sp[2];
                                    Session["SchOrderdouble"] = sch;
                                    string strday = d2.findday(curdate, degree_code, semester, batch_year, semdate, nodays, start_dayorder);
                                    if (doubleday1 == true)
                                        st_semdate = st_semdate.AddDays(-1);
                                    if (dicAlternateDayOrder.ContainsKey(st_semdate))
                                    {
                                        strday = findDayName(dicAlternateDayOrder[st_date]);
                                        Day_Order = Convert.ToString(dicAlternateDayOrder[st_date]).Trim();
                                         Day_Order =  "Day" + Day_Order;
                                        if(Day_Order=="Day0")
                                            Day_Order = Convert.ToString(hatdoc[st_date]).Trim(); 


                                    }
                                    else
                                    {
                                        if (strday.Trim().ToLower() == "mon")
                                            Day_Order = "Day1";
                                        else if (strday.Trim().ToLower() == "tue")
                                            Day_Order = "Day2";
                                        else if (strday.Trim().ToLower() == "wed")
                                            Day_Order = "Day3";
                                        else if (strday.Trim().ToLower() == "thu")
                                            Day_Order = "Day4";
                                        else if (strday.Trim().ToLower() == "fri")
                                            Day_Order = "Day5";
                                        else if (strday.Trim().ToLower() == "sat")
                                            Day_Order = "Day6";
                                        else if (strday.Trim().ToLower() == "sun")
                                            Day_Order = "Day7";
                                    }
                                    if (doubleday1 == true)
                                        Day_Order = double_dayorder + ',' + Day_Order;
                                    if (sch == "0")
                                    {
                                        string chkdoubleday = d2.GetFunction("select * from doubledayorder where doubleDate='" + curdate + "' and batchYear='" + batch_year + "' and degreecode='" + degree_code + "'");
                                        if (chkdoubleday != "" && chkdoubleday != "0")
                                        {
                                            if (doubleday1 == false)
                                            {
                                                doubleday1 = true;
                                                Session["doubledayshk"] = "true";
                                                double_dayorder = Day_Order;
                                               st_date = st_date.AddDays(-1);
                                               daycun--;
                                            }
                                            else
                                            {
                                                doubleday1 = false;
                                                Session["doubledayshk"] = "false";

                                            }
                                        }
                                        else
                                        {
                                            doubleday1 = false;
                                            Session["doubledayshk"] = "false";
                                        }
                                    }
                                  

                                    //dyear["Holiday" + mon + ""] = holi;
                                    dtyear.Rows[row - 1]["Holiday" + coldt + ""] = Day_Order + '/' + daycun;
                                    dtyear.Rows[row - 1]["Holi_day" + coldt + ""] = st_date;
                                }

                               // dtyear.Rows.Add(dyear);
                                if (st_date == en_date)
                                {
                                   
                                    st_date = st_date.AddDays(1);
                                    date = st_date.ToString("dd");
                                    int.TryParse(date, out datre);
                                       string monthyear = st_date.ToString("MM");
                                       if (mon == monthyear)
                                       {
                                           for (int j = datre; j <= 30; j++)
                                           {
                                               if (startmons != mon)
                                               {
                                                   startmons = mon;
                                                   stcol++;
                                                   if (stcol <= 9)
                                                       coldt = "0" + stcol;
                                                   else
                                                       coldt = Convert.ToString(stcol);
                                               }
                                               date = Convert.ToString(j);
                                               string[] sp = st_date.ToString().Split('/');
                                               string curdate = mon + '/' + j + '/' + sp[2];
                                               DateTime starday = Convert.ToDateTime(curdate);
                                               string mon1 = starday.ToString("MM");
                                               string munth1 = starday.ToString("M");
                                               string date1 = starday.ToString("dd");
                                               string day1 = starday.ToString("ddd");
                                               if (dtyear.Rows.Count < j)
                                               {
                                                   dyear = dtyear.NewRow();
                                                   dtyear.Rows.Add(dyear);
                                               }
                                               //dyear["date" + mon + ""] = j;
                                               //dyear["day" + mon + ""] = day1;
                                               //dyear["Holiday" + mon + ""] = "";
                                               dtyear.Rows[j - 1]["date" + coldt + ""] = j;
                                               dtyear.Rows[j - 1]["day" + coldt + ""] = day1;
                                               dtyear.Rows[j - 1]["Holiday" + coldt + ""] = "";
                                               //dtyear.Rows.Add(dyear);
                                               dtyear.Rows[row - 1]["Holi_day" + coldt + ""] = "";
                                           }
                                       }

                                }
                            }
                        }
                        gview.Columns[(stcol * 3)-3 ].Visible = true;
                        gview.Columns[((stcol * 3)-3)  + 1].Visible = true;
                        gview.Columns[((stcol * 3)-3) + 2].Visible = true;
                        st_date = st_date.AddDays(1);
                      
                    }

                }
            }
            if (dtyear.Rows.Count > 1)
            {

               
                gview.DataSource = dtyear;
                gview.DataBind();
                gview.Visible = true;
                if (gview.HeaderRow.Cells.Count > 0)
                {
                    for (int ms = 0; ms < gview.HeaderRow.Cells.Count; ms++)
                    {
                        gview.HeaderRow.Cells[ms].Visible = false;

                    }
                }
                if (gview.Rows.Count > 0)
                {
                    for (int ms = 0; ms < gview.Rows.Count; ms++)
                    {
                        for (int h = 0; h < gview.Rows[ms].Cells.Count; h++)
                            gview.Rows[ms].Cells[h].Visible = false;

                    }
                }
                if (headrow.Count > 0)
                {
                    for (int head = 0; head < headrow.Count; head++)
                    {
                        int val = Convert.ToInt32(headrow[head + 1]);
                        string years = Convert.ToString(headyear[head + 1]);
                        gview.HeaderRow.Cells[(head * 3) ].ColumnSpan = 3;
                        gview.HeaderRow.Cells[(head * 3) ].Visible = true;
                        gview.HeaderRow.Cells[((head * 3)) + 1].Visible = false;
                        gview.HeaderRow.Cells[((head * 3)) + 2].Visible = false;
                        string year = string.Empty;
                        if (val == 1)
                            year = "January";
                        if (val == 2)
                            year = "February";
                        if (val == 3)
                            year = "March";
                        if (val == 4)
                            year = "April";
                        if (val == 5)
                            year = "May";
                        if (val == 6)
                            year = "June";
                        if (val == 7)
                            year = "July";
                        if (val == 8)
                            year = "August";
                        if (val == 9)
                            year = "September";
                        if (val == 10)
                            year = "October";
                        if (val == 11)
                            year = "November";
                        if (val == 12)
                            year = "December";
                        gview.HeaderRow.Cells[(head * 3)].Text = year +'-' + years;
                        if (gview.Rows.Count > 0)
                        {
                            for (int ms = 0; ms < gview.Rows.Count; ms++)
                            {

                                gview.Rows[ms].Cells[(head * 3) ].Visible = true;
                                gview.Rows[ms].Cells[((head * 3) ) + 1].Visible = true;
                                gview.Rows[ms].Cells[((head * 3) ) + 2].Visible = true;

                            }
                        }
                       
                    }
                }
                if (headholi.Count > 0)
                {
                    for (int head = 0; head < headholi.Count; head++)
                    {
                        string val = Convert.ToString(headholi[head + 1]);
                        string[] spl = val.Split('-');
                        int row = Convert.ToInt32(spl[0]);
                        int col = Convert.ToInt32(spl[1]);

                        gview.Rows[row].Cells[((col * 3) - 3) + 2].BackColor = Color.DarkSeaGreen;
                        //gview.HeaderRow.Cells[((val * 3) - 3) + 1].Visible = false;
                        //gview.HeaderRow.Cells[((val * 3) - 3) + 2].Visible = false;

                    }
                }
                //Div4.Visible = true;
               // RowHead(gview);
            }
        }
        catch
        {
        }
    }
    private string findDayName(byte dayOrder)
    {
        string dayName = string.Empty;
        switch (dayOrder)
        {
            case 0:
                dayName = string.Empty;
                break;
            case 1:
                dayName = "Mon";
                break;
            case 2:
                dayName = "Tue";
                break;
            case 3:
                dayName = "Wed";
                break;
            case 4:
                dayName = "Thu";
                break;
            case 5:
                dayName = "Fri";
                break;
            case 6:
                dayName = "Sat";
                break;
            case 7:
                dayName = "Sun";
                break;
            default:
                break;
        }
        return dayName;
    }
    public void binddate()
    {
        try
        {


           
          
            string typ1 = string.Empty;
            if (cbl_branch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + cbl_branch.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + cbl_branch.Items[i].Value + "";
                        }
                    }

                }
            }
            //string startdate = d2.GetFunction("Select convert(nvarchar(15),start_date,103) as date from seminfo where batch_year in('" + Lbbatch.Text + "') and degree_code in('" + typ1 + "' ) and semester in('" + lbsem.Text + " ') order by start_date desc ");

            //string enddate = d2.GetFunction("Select convert(nvarchar(15),end_date,103) as date from seminfo where batch_year in('" + Lbbatch.Text + "') and degree_code in('" + typ1 + " ') and semester in('" + lbsem.Text + " ')order by end_date desc ");
            if (lbsem.Text != "" && typ1 != "" && Lbbatch.Text != "")
            {
                string sql = "select top 1 s.batch_year , Dept_Name,batch_year,d.degree_code,CONVERT(nvarchar(15),start_date,103) as start_date,CONVERT(datetime,start_date,103) as st_date,CONVERT(datetime,end_date,103) as en_date,CONVERT(varchar,end_date,103) as end_date,s.semester,no_of_working_Days,no_of_working_hrs,schOrder,starting_dayorder,nodays  from Degree d,Department de,course c,seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and s.semester=p.semester and p.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code=de.college_code and s.degree_code in('" + typ1 + "')  and batch_year in('" + Lbbatch.Text + "')   and s.semester in('" + lbsem.Text + "')  and c.college_code='"+Convert.ToString(ddlclg.SelectedValue)+"' order by   c.college_code,s.batch_year asc ,s.semester desc,d.Degree_Code,datepart(year,start_date) desc , datepart(month ,start_date) desc";

                DataSet dsattabsent = daccess.select_method_wo_parameter(sql, "text");
                if (dsattabsent.Tables.Count > 0 && dsattabsent.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsattabsent.Tables[0].Rows.Count; i++)
                    {
                        string batch_year = Convert.ToString(dsattabsent.Tables[0].Rows[i]["batch_year"]);
                        string degree_code = Convert.ToString(dsattabsent.Tables[0].Rows[i]["degree_code"]);
                        string semester = Convert.ToString(dsattabsent.Tables[0].Rows[i]["semester"]);
                        string start_dayorder = Convert.ToString(dsattabsent.Tables[0].Rows[i]["starting_dayorder"]);
                        string nodays = Convert.ToString(dsattabsent.Tables[0].Rows[i]["nodays"]);
                        string sch = Convert.ToString(dsattabsent.Tables[0].Rows[0]["schOrder"]);
                        string st_date = Convert.ToString(dsattabsent.Tables[0].Rows[0]["start_date"]);
                        DateTime semst_date = Convert.ToDateTime(dsattabsent.Tables[0].Rows[0]["st_date"]);
                        string en_date = Convert.ToString(dsattabsent.Tables[0].Rows[0]["end_date"]);

                        txt_startdate.Text = Convert.ToString(st_date);
                        txt_enddate.Text = Convert.ToString(en_date);


                    }
                }

                else
                {
                    txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyy");
                    txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyy");
                }
            }
            else
            {
                txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyy");
                txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyy");
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void sem()
    {
        lbsem.Text = "";
        Lbl_sem.Text = "";
        string typ1 = string.Empty;
         string typ = string.Empty;
        int semes = 0;
        Boolean semtr = false;
        string quer = string.Empty;
        if (ddlsem.SelectedIndex == 0)
            quer = "Order by textval asc";
        else
            quer = "Order by textval desc";
        string sem = "select textval from FT_ACADEMICYEAR_DETAILED dt,textvaltable tv where ACD_YEAR='" + Convert.ToString(ddlBatch.SelectedValue) + "' and TextCode=ACD_FEECATEGORY and college_code='" + ddlclg.SelectedValue + "' and  TextCriteria='FEECA' " + quer + " ";
        DataSet dsattabsent = daccess.select_method_wo_parameter(sem, "text");
        if (dsattabsent.Tables.Count > 0 && dsattabsent.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsattabsent.Tables[0].Rows.Count; i++)
            {
                semtr = false;
                string[] semester =Convert.ToString(dsattabsent.Tables[0].Rows[i]["textval"]).Split();
                if (semester.Length > 1)
                    sem = semester[0];
                int.TryParse(sem, out semes);
                if (ddlsem.SelectedIndex == 1)
                {
                    if (semes % 2 == 0)
                    {
                        semtr = true;
                    }
                }
                else
                {
                    if (semes % 2 != 0)
                    {
                        semtr = true;
                    }
                }
                if (semtr == true)
                {
                    if (typ1 == "")
                    {
                        typ1 = "" + sem + "";
                        typ=  sem ;
                    }
                    else
                    {
                        typ1 = typ1 + "'" + "," + "'" + sem + "";
                        typ = typ  +"," + sem ;
                    }
                }
            }
            lbsem.Text = typ1;
            Lbl_sem.Text = typ;
        }
      
    }
    public void batch()
    {
        Lbbatch.Text = "";
        Lbl_batch.Text = "";
        string typ1 = string.Empty;
        string typ = string.Empty;
           string batch = " select distinct ACD_BATCH_YEAR from FT_ACADEMICYEAR_DETAILED where ACD_YEAR='" + Convert.ToString(ddlBatch.SelectedValue) + "'";
           DataSet dsattabsent = daccess.select_method_wo_parameter(batch, "text");
           if (dsattabsent.Tables.Count > 0 && dsattabsent.Tables[0].Rows.Count > 0)
           {
               for (int i = 0; i < dsattabsent.Tables[0].Rows.Count; i++)
               {
                   string batchs= Convert.ToString(dsattabsent.Tables[0].Rows[i]["ACD_BATCH_YEAR"]);
                  
                   if (typ1 == "")
                   {
                       typ1 = "" + batchs + "";
                       typ = batchs;
                   }
                   else
                   {
                       typ1 = typ1 + "'" + "," + "'" + batchs + "";
                       typ = typ + "," + batchs ;
                   }
               }
               Lbbatch.Text = typ1;
               Lbl_batch.Text = typ;
           }
    }
    public void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        sem();
        binddate();
    }
    public void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        batch();
        sem();
        binddate();
    }


    protected void gview_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblerrmsg.Text = "";
            txtholiday.Text = "";
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string activerow = Convert.ToString(rowIndex);
            string activecol = Convert.ToString(selectedCellIndex); int rows = 0;
            int.TryParse(activerow, out rows);
            int col = 0;
            int.TryParse(activecol, out col);
            col+=3;
            col = col / 3;
            string lblname = "Lbl_" + col + "";
            string Name = ((gview.Rows[rows].FindControl(lblname) as Label).Text);
            lbldate.Text = Name;
            if (Name != "")
            {
                string lbltext = "Lbl" + col + "";
                string lblval = ((gview.Rows[rows].FindControl(lbltext) as Label).Text);
                imgdiv2.Visible = true;
                string typ1 = string.Empty;
                if (cbl_branch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_branch.Items.Count; i++)
                    {
                        if (cbl_branch.Items[i].Selected == true)
                        {
                            if (typ1 == "")
                            {
                                typ1 = "" + cbl_branch.Items[i].Value + "";
                            }
                            else
                            {
                                typ1 = typ1 + "'" + "," + "'" + cbl_branch.Items[i].Value + "";
                            }
                        }

                    }
                }
                string fdate = lbldate.Text.ToString();
                string[] fd = fdate.Split('/');
                DateTime dt1 = Convert.ToDateTime(fd[0] + '/' + fd[1] + '/' + fd[2]);
                string holiday = "select * from holidaystudents where  degree_code in('" + typ1 + "') and semester in('" + lbsem.Text + "') and holiday_date = '" + dt1 + "'";
                DataSet dsdayorder = d2.select_method_wo_parameter(holiday, "Text");
                if (dsdayorder.Tables.Count > 0 && dsdayorder.Tables[0].Rows.Count > 0)
                {
                    btnsaveholiday.Text = "Update";
                    txtholiday.Text = lblval;
                    btnholiday.Visible = true;
                }
                else
                {
                    btnsaveholiday.Text = "Save";
                    txtholiday.Text = "";
                    btnholiday.Visible = false;
                }

                string doubdate = " select * from doubledayorder where batchYear in('" + Lbbatch.Text + "') and doubleDate='" + dt1 + "' and degreecode in('" + typ1 + "') and college_code='" + Convert.ToString(ddlclg.SelectedValue) + "'";
                DataSet dsdoubdate = d2.select_method_wo_parameter(doubdate, "Text");

                if (dsdoubdate.Tables.Count > 0 && dsdoubdate.Tables[0].Rows.Count > 0)
                {
                    btndoubl.Text = "Update";
                    btnNo.Visible = true;
                }
                else
                {
                    btndoubl.Text = "Save";
                    btnNo.Visible = false;
                }
                loadreason();
            }
            else
            {
                imgdiv2.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void btnreasonadd_Click(object sender, EventArgs e)
    {
        textreason.Text = string.Empty;
        panelreason.Visible = true;


    }

    protected void btnreasondelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopErr.Text = string.Empty;
            divPopErr.Visible = false;
            if (ddlreason.Items.Count > 0)
            {
                string reason = ddlreason.SelectedItem.ToString();
                string strquery = "select * from tbl_consider_day_order where Reason='" + reason + "'";
                DataSet dsexres = d2.select_method_wo_parameter(strquery, "Text");
                if (dsexres.Tables[0].Rows.Count == 0)
                {
                    string insertvalue = "Delete from textvaltable where TextVal='" + reason + "' and TextCriteria='dayor'";
                    int inserty = d2.update_method_wo_parameter(insertvalue, "Text");
                    loadreason();
                    lblPopErr.Text = "Reason Deleted Successfully";
                    divPopErr.Visible = true;
                }
                else
                {
                    lblerrmsg.Text = "You Can't Delete This Because  Already Exists The Day Order Change This Reason";
                    lblerrmsg.Visible = true;
                    return;
                }
            }
            else
            {
                lblerrmsg.Text = "No Reason For Delete";
                lblerrmsg.Visible = true;
                return;
            }
        }
        catch
        {
        }
    }

    protected void btnreasonsave_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopErr.Text = string.Empty;
            divPopErr.Visible = false;
            collegecode = Convert.ToString(ddlclg.SelectedValue);
            string reason = textreason.Text.ToString();
            if (reason.Trim() != "" && reason != null)
            {
                string insvalues = "select * from textvaltable where TextCriteria='dayor' and TextVal='" + reason + "'";
                DataSet ds = new DataSet();
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(insvalues, "Text");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    string insertvalue = "insert into textvaltable(TextVal,TextCriteria,college_code) values('" + reason + "','dayor','" + collegecode + "')";
                    int inserty = d2.update_method_wo_parameter(insertvalue, "Text");
                    textreason.Text = string.Empty;
                    loadreason();
                    lblPopErr.Text = "Reason Successfully Saved";
                    divPopErr.Visible = true;
                }
                else
                {
                    lblreasonerr.Visible = true;
                    lblreasonerr.Text = "Already Exists Reason";
                    return;
                }
            }
            else
            {
                lblreasonerr.Visible = true;
                lblreasonerr.Text = "Please Enter Reason";
                return;
            }
        }
        catch
        {
        }
    }

    protected void btnreasonexit_Click(object sender, EventArgs e)
    {
        panelreason.Visible = false;
    }
    public void loadreason()
    {
        try
        {
           
            ddlreason.Items.Clear();
            string strquery = "select * from textvaltable where TextCriteria='dayor' and college_code='" + ddlclg.SelectedValue + "'";
            DataSet ds = new DataSet();
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
               

                ddlreason.DataSource = ds;
                ddlreason.DataTextField = "TextVal";
                ddlreason.DataValueField = "Textcode";
                ddlreason.DataBind();
            }
        }
        catch { }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {


            Boolean batchset = false;
            Boolean deptset = false;
            string existdetails = string.Empty;
            lblPopErr.Text = string.Empty;
            divPopErr.Visible = false;
            if (ddlreason.Items.Count == 0)
            {
                lblerrmsg.Text = "Please Enter Reason";
                lblerrmsg.Visible = true;
                return;
            }
            string reason = ddlreason.SelectedItem.ToString();
            string strsem = "Select Distinct Current_Semester,Degree_code,Batch_Year from registration where cc=0 and delflag=0 and exam_flag<>'debar' ;";
            strsem = strsem + " Select * from Seminfo";
            DataSet dssem = d2.select_method_wo_parameter(strsem, "Text");

            string fdate = lbldate.Text.ToString();
            string[] fd = fdate.Split('/');
            string tdate = lbldate.Text.ToString();
            string[] ttda = tdate.Split('/');
            DateTime dtfrom = Convert.ToDateTime(fd[0] + '/' + fd[1] + '/' + fd[2]);
            DateTime dtto = Convert.ToDateTime(ttda[0] + '/' + ttda[1] + '/' + ttda[2]);
            if (dtfrom > dtto)
            {
                lblerrmsg.Text = "Please Enter To Date Must Be Greater Than From Date";
                lblerrmsg.Visible = true;
                return;
            }

            string getaldegree = "select de.Dept_Name,c.Course_Name,d.Degree_Code from Degree d,Department de,course c where d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id ";
            DataSet dsdegree = d2.select_method_wo_parameter(getaldegree, "Text");

            Boolean saveflag = false;

            string batch = Lbl_batch.Text;
            string[] spl = batch.Split(',');
            for (int ba = 0; ba <spl.Count(); ba++)
            {

                string batchyear = spl[ba];
                    batchset = true;
                    for (int br = 0; br < cbl_branch.Items.Count; br++)
                    {
                        if (cbl_branch.Items[br].Selected == true)
                        {
                            deptset = true;
                            string degree = cbl_branch.Items[br].Value.ToString();
                            dssem.Tables[0].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "'";
                            DataView dvsem = dssem.Tables[0].DefaultView;
                            for (int se = 0; se < dvsem.Count; se++)
                            {
                                string sem = dvsem[se]["Current_Semester"].ToString();
                                dssem.Tables[1].DefaultView.RowFilter = " batch_year='" + batchyear + "' and degree_code='" + degree + "' and semester='" + sem + "'";
                                DataView dvseminfo = dssem.Tables[1].DefaultView;
                                for (int si = 0; si < dvseminfo.Count; si++)
                                {
                                    string sdate = dvseminfo[si]["start_date"].ToString();
                                    string edate = dvseminfo[si]["end_date"].ToString();
                                    DateTime dtstart = Convert.ToDateTime(sdate);
                                    DateTime dtend = Convert.ToDateTime(edate);
                                    if (dtfrom >= dtstart && dtfrom <= dtend && dtto >= dtstart && dtto <= dtend)
                                    {
                                        string strexistrecdord = "select * from tbl_consider_day_order where Degree_code='" + degree + "' and Batch_year='" + batchyear + "' and Semester='" + sem + "' and ((from_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "') or  (to_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'))";
                                        DataSet dsex = d2.select_method_wo_parameter(strexistrecdord, "Text");
                                        int asperday = 0;
                                        int includeattn = 0;
                                        int skipday = 0;
                                        int nextday = 0;
                                        string alternateDayOrder = "0";
                                      
                                            if (ddlAlternateDayOrder.Items.Count > 0)
                                            {
                                                alternateDayOrder = Convert.ToString(ddlAlternateDayOrder.SelectedValue).Trim();
                                            }



                                            if (dsex.Tables[0].Rows.Count == 0)
                                            {
                                                // if (Chkasperday.Checked == true)
                                                if (rdbasperday.Checked == true)
                                                    asperday = 1;

                                                else if (rdbskipday.Checked == true)
                                                    asperday = 2;

                                                else if (rdbnextorder.Checked == true)
                                                    asperday = 3;

                                                if (Chkincludeattendance.Checked == true)
                                                    includeattn = 1;
                                                else
                                                    includeattn = 0;
                                                string insertvalue = "if exists (select * from tbl_consider_day_order where Degree_code='" + degree + "' and Batch_year='" + batchyear + "' and Semester='" + sem + "' and ((from_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "') or  (to_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "')) )update  tbl_consider_day_order set DayOrder='" + alternateDayOrder + "',asperday='" + asperday + "',include_attendance='" + includeattn + "' where Degree_code='" + degree + "' and Batch_year='" + batchyear + "' and Semester='" + sem + "' and ((from_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "') or  (to_date between '" + dtfrom.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "')) else insert into tbl_consider_day_order (From_Date,To_Date,Reason,Batch_year,Degree_code,Semester,DayOrder,asperday,include_attendance)";

                                                insertvalue = insertvalue + " Values('" + dtfrom + "','" + dtto + "','" + reason + "','" + batchyear + "','" + degree + "','" + sem + "','" + alternateDayOrder + "','" + asperday + "','" + includeattn + "')";
                                                int insert = d2.update_method_wo_parameter(insertvalue, "Text");
                                                saveflag = true;
                                            }
                                            else
                                            {
                                                dsdegree.Tables[0].DefaultView.RowFilter = " Degree_code='" + degree + "'";
                                                DataView dvdegree = dsdegree.Tables[0].DefaultView;
                                                if (existdetails == "")
                                                {
                                                    existdetails = batchyear + " - " + dvdegree[0]["Course_Name"].ToString() + " - " + dvdegree[0]["Dept_Name"].ToString() + " - " + sem + " Sem ";
                                                }
                                                else
                                                {
                                                    existdetails = existdetails + " , " + batchyear + " - " + dvdegree[0]["Course_Name"].ToString() + " - " + dvdegree[0]["Dept_Name"].ToString() + " - " + sem + " Sem ";
                                                }
                                            }
                                    }
                                }
                            }
                        }
                    }
                
            }


            if (batchset == false)
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Please Select the Batch and Proceed";
                return;
            }
            if (deptset == false)
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Please Select the Degree,Branch and Proceed";
                return;
            }
            if (saveflag == true)
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Update Successfully";
            }
            else
            {
                if (existdetails == "")
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Please Update Semeter Information";
                }
            }
            if (existdetails.Trim() != "")
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = existdetails + " Already Exists The Given Date";
            }
            btnGo_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void btnPopErrClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopErr.Text = string.Empty;
            divPopErr.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        try
        {
           
            imgdiv2.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void rdbonchecked(object sender, EventArgs e)
    {
        try
        {
            if (rdbdayorder.Checked == true)
            {
                dayorder.Visible = true;
                fbholiday.Visible = false;
                fbDouble.Visible = false;
            }
            else if (rdbdoubleday.Checked == true)
            {
                dayorder.Visible = false;
                fbholiday.Visible = false;
                fbDouble.Visible = true;
               
            }
            else if (rdbholiday.Checked == true)
            {
                dayorder.Visible = false;
                fbholiday.Visible = true;
                fbDouble.Visible = false;
            }

            else
            {
                dayorder.Visible = false;
                fbholiday.Visible = false;
                fbDouble.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void btndoubl_Click(object sender, EventArgs e)
    {
        try
        {
            hat.Clear();
              string batch = Lbbatch.Text;
              string batchs = Lbl_batch.Text;
              string[] spl = batchs.Split(',');
              string fdate = lbldate.Text.ToString();
            string[] fd = fdate.Split('/');
            DateTime dt1 = Convert.ToDateTime(fd[0] + '/' + fd[1] + '/' + fd[2]);
            int count =0;
            int belltm=0;
            if (chkbell.Checked == true)
                belltm = 1;
             string typ1 = string.Empty;
                if (cbl_branch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_branch.Items.Count; i++)
                    {
                        if (cbl_branch.Items[i].Selected == true)
                        {
                            if (typ1 == "")
                            {
                                typ1 = "" + cbl_branch.Items[i].Value + "";
                            }
                            else
                            {
                                typ1 = typ1 + "'" + "," + "'" + cbl_branch.Items[i].Value + "";
                            }
                        }

                    }
                }
                string doubdate = " select * from doubledayorder where batchYear in('" + batch + "') and doubleDate='" + dt1 + "' and degreecode in('" + typ1 + "') and college_code='" + Convert.ToString(ddlclg.SelectedValue) + "'";
                DataSet dsdoubdate = d2.select_method_wo_parameter(doubdate, "Text");
                DataView dvdoubledate = new DataView();
            for (int ba = 0; ba < spl.Count(); ba++)
            {

                string batchYear = spl[ba];
                for (int br = 0; br < cbl_branch.Items.Count; br++)
                {
                    if (cbl_branch.Items[br].Selected == true)
                    {
                        string degCode = cbl_branch.Items[br].Value.ToString();
                        dsdoubdate.Tables[0].DefaultView.RowFilter = "batchYear in(" + batchYear + ") and doubleDate='" + dt1 + "' and degreecode in(" + degCode + ") and college_code='" + Convert.ToString(ddlclg.SelectedValue) + "'";
                        dvdoubledate = dsdoubdate.Tables[0].DefaultView;
                        if (dvdoubledate.Count ==0)
                        {
                            //hat.Clear();
                            //hat.Add("@batchyear", batchYear);
                            //hat.Add("@degreecode", degCode);
                            //hat.Add("@doubleDate", dt1.ToString("MM/dd/yyyy"));
                            //hat.Add("@college_code", Convert.ToString(ddlclg.SelectedValue));
                            //hat.Add("@belltime", belltm);
                            string doubleda = " if exists (select * from doubledayorder where batchYear in('" + batch + "') and doubleDate='" + dt1 + "' and degreecode in('" + typ1 + "') and college_code='" + Convert.ToString(ddlclg.SelectedValue) + "') update doubledayorder set belltime='" + belltm + "' where  batchYear in('" + batch + "') and doubleDate='" + dt1 + "' and degreecode in('" + typ1 + "') and college_code='" + Convert.ToString(ddlclg.SelectedValue) + "' else insert into  doubledayorder(batchYear,degreecode,college_code,doubleDate,belltime)values ('" + batchYear + "','" + degCode + "','" + Convert.ToString(ddlclg.SelectedValue) + "','" + dt1.ToString("MM/dd/yyyy") + "','" + belltm + "')";
                             count = d2.update_method_wo_parameter(doubleda, "Text");
                        }
                    }
                }
            }
            if (count > 0)
            {
                divPopErr.Visible = true;
                lblPopErr.Visible = true;
                lblPopErr.Text = "Saved Successfully";
                btnGo_Click(sender, e);
            }
                            
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        try
        {
            hat.Clear();
            string batch = Lbbatch.Text;
            string[] spl = batch.Split(',');
            string fdate = lbldate.Text.ToString();
            string[] fd = fdate.Split('/');
            DateTime dt1 = Convert.ToDateTime(fd[0] + '/' + fd[1] + '/' + fd[2]);
            int count = 0;
            string typ1 = string.Empty;
            if (cbl_branch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + cbl_branch.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + cbl_branch.Items[i].Value + "";
                        }
                    }

                }
            }
            string doubdate = "delete from doubledayorder where batchYear in('" + batch + "') and doubleDate='" + dt1 + "' and degreecode in('" + typ1 + "') and college_code='" + Convert.ToString(ddlclg.SelectedValue) + "'";
            int dsdoubdate = daccess.update_method_wo_parameter(doubdate, "text"); ;

            if (dsdoubdate > 0)
            {
                divPopErr.Visible = true;
                lblPopErr.Visible = true;
                lblPopErr.Text = "Delete Successfully";
                btnGo_Click(sender, e);
            }

        }
        catch (Exception ex)
        {
        }
    }
   
    protected void imgbutn_Click(object sender, EventArgs e)
    {
        try
        {

  
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnsaveholiday_Click(object sender, EventArgs e)
    {
        try
        {
            
             //   string batch = Lbbatch.Text;
            string semster = Lbl_sem.Text;
            string[] spl = semster.Split(',');
                string fdate = lbldate.Text.ToString();
                string[] fd = fdate.Split('/');
                DateTime dt1 = Convert.ToDateTime(fd[0] + '/' + fd[1] + '/' + fd[2]);
                int upholi = 0;
               
              
                    for (int br = 0; br < cbl_branch.Items.Count; br++)
                    {
                        if (cbl_branch.Items[br].Selected == true)
                        {
                            for (int ba = 0; ba < spl.Count(); ba++)
                            {

                                string sem = spl[ba];
                            string degCode = cbl_branch.Items[br].Value.ToString();

                            string holiday = " if exists (select * from holidaystudents where  degree_code in('" + degCode + "') and semester in('" + sem + "') and holiday_date = '" + dt1 + "' ) update  holidaystudents set holiday_desc='" + txtholiday.Text + "' where   degree_code in('" + degCode + "') and semester in('" + sem + "') and holiday_date = '" + dt1 + "' else insert into holidaystudents(degree_code,holiday_date,holiday_desc,semester,halforfull,morning,evening) values(" + degCode + ",'" + dt1 + "','" + txtholiday.Text + "'," + sem + ",0,0,0)";
                            upholi = daccess.update_method_wo_parameter(holiday, "text");
                        }
                    }
                }
                if (upholi > 0)
                {
                    divPopErr.Visible = true;
                    lblPopErr.Visible = true;
                    lblPopErr.Text = "Saved Successfully";
                    btnGo_Click(sender, e);
                }
    
               
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnholiday_Click(object sender, EventArgs e)
    {
        try
        {

            string batch = Lbbatch.Text;
            string[] spl = batch.Split(',');
            string fdate = lbldate.Text.ToString();
            string[] fd = fdate.Split('/');
            DateTime dt1 = Convert.ToDateTime(fd[0] + '/' + fd[1] + '/' + fd[2]);
            int upholi = 0;
         
                string typ1 = string.Empty;
                if (cbl_branch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_branch.Items.Count; i++)
                    {
                        if (cbl_branch.Items[i].Selected == true)
                        {
                            if (typ1 == "")
                            {
                                typ1 = "" + cbl_branch.Items[i].Value + "";
                            }
                            else
                            {
                                typ1 = typ1 + "'" + "," + "'" + cbl_branch.Items[i].Value + "";
                            }
                        }

                    }
                }
                string holiday = "delete  from holidaystudents where  degree_code in('" + typ1 + "') and semester in('" + lbsem.Text + "') and holiday_date = '" + dt1 + "'";
                upholi = daccess.update_method_wo_parameter(holiday, "text");
            
            if (upholi > 0)
            {
                divPopErr.Visible = true;
                lblPopErr.Visible = true;
                lblPopErr.Text = "Delete Successfully";
                btnGo_Click(sender, e);
            }

        }
        catch (Exception ex)
        {
        }
    }
}