using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class Default5 : System.Web.UI.Page
{
    string Day_Order = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    Hashtable hat = new Hashtable();
    int count = 0;
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("ScheduleHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
                    return;
                }
            }
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            errlbl.Visible = false;
            if (!Page.IsPostBack)
            {
                fromdatetxt.Attributes.Add("ReadOnly", "ReadOnly");//Added by Manikandan 15/08/2013
                todatetxt.Attributes.Add("ReadOnly", "ReadOnly");//Added by Manikandan 15/08/2013
                //if (Convert.ToString(Session["value"]) == "1")//==========back button visible
                //{
                //    LinkButton3.Visible = false;
                //    LinkButton2.Visible = true;
                //}
                //else
                //{
                //    LinkButton3.Visible = true;
                //    LinkButton2.Visible = false;
                //}

                alter_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                alter_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                alter_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                alter_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                alter_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                alter_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                alter_spread.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
                alter_spread.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
                alter_spread.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                alter_spread.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;//Added by Manikandan 20/08/2013
                alter_spread.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;//Added by Manikandan 20/08/2013
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = FontUnit.Medium;
                style.Font.Bold = true;

                alter_spread.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                alter_spread.Sheets[0].AllowTableCorner = true;
                alter_spread.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                alter_spread.Sheets[0].SheetCorner.Columns[0].Width = 40;

                errlbl.Visible = false;
                alter_spread.Visible = false;
                alter_spread.Sheets[0].AutoPostBack = true;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                lblrptname.Visible = false;
                Printcontrol.Visible = false;
                txtexcelname.Visible = false;

                Bindcollege();
                collegecode = ddlcollege.SelectedValue.ToString();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

                string dt = DateTime.Today.ToShortDateString();
                string[] dsplit = dt.Split(new Char[] { '/' });
                fromdatetxt.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                todatetxt.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                Session["curr_year"] = dsplit[2].ToString();
                int intNHrs = 0;
                string nofohrs = Convert.ToString(d2.GetFunction("Select max(No_of_hrs_per_day) from periodattndschedule"));
                if (nofohrs.Trim() != "" && nofohrs != null)
                {
                    intNHrs = Convert.ToInt16(nofohrs);
                }
                int item = 0;
                for (item = 1; item <= intNHrs; item++)
                {
                    ListItem acclist = new ListItem();
                    acclist.Value = (item.ToString());
                    acclist.Text = (item.ToString());
                    fromhrddl.Items.Add(acclist);
                    tohrddl.Items.Add(acclist);
                }
                int yr_value = 0;
                item = 0;

                string yearsmsa = Convert.ToString(d2.GetFunction("Select max(duration) from degree"));
                if (yearsmsa.ToString().Trim() != "" && yearsmsa != null)
                {
                    yr_value = (Convert.ToInt16(yearsmsa)) / 2;
                }

                for (item = 1; item <= yr_value; item++)
                {
                    //ListItem acclist = new ListItem();
                    //chklsbatch.DataTextField = (item.ToString());
                    //chklsbatch.DataValueField = (item.ToString());
                    //chklsbatch.Items.Add(acclist);
                    //chklsbatch.Items.Insert(index,ListItem acclist (item));
                    chklsbatch.Items.Add(Convert.ToString(item));
                }
            }
        }
        catch (Exception ex) { }
    }


    public void Bindcollege()
    {
        try
        {
            string columnfield = "";
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
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {

                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
            else
            {

            }
        }
        catch (Exception ex)
        {

        }
    }


    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {


            count = 0;
            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                chklstdegree.Items[0].Selected = true;
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
                if (chkdegree.Checked == true)
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = true;
                        txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = false;
                        txtdegree.Text = "---Select---";
                    }
                }
                txtdegree.Enabled = true;
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {

        }

    }
    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;
            collegecode = ddlcollege.SelectedValue.ToString();
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            if (course_id.Trim() != "")
            {
                ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds2;
                    chklstbranch.DataTextField = "dept_name";
                    chklstbranch.DataValueField = "degree_code";
                    chklstbranch.DataBind();
                    chklstbranch.Items[0].Selected = true;
                    for (int i = 0; i < chklstbranch.Items.Count; i++)
                    {
                        chklstbranch.Items[i].Selected = true;
                        if (chklstbranch.Items[i].Selected == true)
                        {
                            count += 1;
                        }
                        if (chklstbranch.Items.Count == count)
                        {
                            chkbranch.Checked = true;
                        }
                    }
                    if (chkbranch.Checked == true)
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chklstbranch.Items[i].Selected = true;
                            txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chkbranch.Checked = false;
                            chklstbranch.Items[i].Selected = false;
                            txtbranch.Text = "---Select---";
                        }
                    }
                }
            }
            else
            {
                txtbranch.Text = "---Select---";
                chklstbranch.Items.Clear();
            }
        }
        catch (Exception ex)
        {

        }
    }


    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            collegecode = ddlcollege.SelectedValue.ToString();
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "---Select---";
                txtbranch.Text = "---Select---";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

        }
        catch (Exception ex)
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
                txtbatch.Text = "Year(" + (chklsbatch.Items.Count) + ")";
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
                txtbatch.Text = "Year(" + commcount.ToString() + ")";
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
    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            collegecode = ddlcollege.SelectedValue.ToString();
            int commcount = 0;
            chkdegree.Checked = false;
            txtdegree.Text = "---Select---";
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                if (commcount == chklstdegree.Items.Count)
                {
                    chkdegree.Checked = true;
                }
            }

            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

        }
        catch (Exception ex)
        {

        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                }
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                }
                chkbranch.Checked = false;
                txtbranch.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            string clg = "";
            int commcount = 0;
            txtbranch.Text = "--Select--";
            chkbranch.Checked = false;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == chklstbranch.Items.Count)
                {
                    chkbranch.Checked = true;
                }
            }


        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {

        }
    }

    protected void gobtn_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            string srt_day = string.Empty;
            string degcode = string.Empty;
            string semester = string.Empty;
            string batchyear = string.Empty;
            string section = string.Empty;
            string grid_section = string.Empty;
            string strsec = string.Empty;
            string sdate = string.Empty;
            string noofdays = string.Empty;
            string startdayorder = string.Empty;
            string startdate = string.Empty;
            string todate = string.Empty;
            string allsemval = string.Empty;
            int SchOrder = 0, nodays = 0, intNHrs = 0, start_dayorder = 0;
            DateTime dtime_str;
            ArrayList arlst = new ArrayList();

            string date1 = "";
            string datefrom = "";
            string date2 = "";
            string dateto = "";
            string sql_s = "";
            string asql = "";
            string sqlstr = "";
            string strDay = "";
            string Strsql = "";
            string subj_staff_s = "";
            string sql1 = "";
            string subj_code = "";
            string staff_code = "";
            string text_val = "";
            string date_value = "";
            string str_year = "";
            string query_date = "";
            int noofhrs = 0;
            int day_diff = 0;
            int row_val = 0;
            int semi_split = 0;
            int fmhr = 0;
            int tohr = 0;
            int sno = 1;
            int span_count = 0;
            Boolean fflag = false;
            fmhr = Convert.ToInt16(fromhrddl.SelectedValue.ToString());
            tohr = Convert.ToInt16(tohrddl.SelectedValue.ToString());
            alter_spread.CurrentPage = 0;

            string mainvalue = "";
            if (chklstbranch.Items.Count > 0)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        if (mainvalue == "")
                        {
                            mainvalue = chklstbranch.Items[i].Value;
                        }
                        else
                        {
                            mainvalue = mainvalue + "," + chklstbranch.Items[i].Value;
                        }
                    }
                }
            }

            sqlstr = d2.GetFunction("select max(No_of_hrs_per_day) from PeriodAttndSchedule");
            if (sqlstr.Trim() != "" && sqlstr != null)//Added by srinath 6/1/2014
            {
                noofhrs = Convert.ToInt32(sqlstr);
            }

            if (mainvalue.Trim() != "" && txtbatch.Text != "--Select--")
            {
                date1 = fromdatetxt.Text;
                string[] split = date1.Split(new Char[] { '/' });
                if (split.GetUpperBound(0) == 2)
                {
                    if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                    {
                        datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                        date2 = todatetxt.Text.ToString();
                        string[] split1 = date2.Split(new Char[] { '/' });
                        if (split1.GetUpperBound(0) == 2)
                        {
                            if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                            {
                                dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                                DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                                TimeSpan t = dt2.Subtract(dt1);
                                long days = t.Days;
                                if (days >= 0)
                                {
                                    if (Convert.ToInt16(fromhrddl.SelectedValue.ToString()) <= Convert.ToInt16(tohrddl.SelectedValue.ToString()))
                                    {
                                        alter_spread.Sheets[0].RowCount = 0;
                                        alter_spread.Sheets[0].ColumnCount = 0;
                                        alter_spread.Sheets[0].SheetCorner.ColumnCount = 0;
                                        alter_spread.Sheets[0].ColumnHeader.RowCount = 2;
                                        alter_spread.Sheets[0].ColumnCount = 9;
                                        alter_spread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                                        alter_spread.Sheets[0].Columns[0].Width = 150;
                                        alter_spread.Sheets[0].Columns[1].Width = 50;
                                        alter_spread.Sheets[0].Columns[2].Width = 200;
                                        alter_spread.Sheets[0].Columns[3].Width = 200;
                                        alter_spread.Sheets[0].Columns[4].Width = 350;
                                        alter_spread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                        alter_spread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);


                                        alter_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                        alter_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                                        alter_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                                        alter_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                                        alter_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Period";
                                        alter_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                                        alter_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
                                        alter_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                                        alter_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Leave Details";
                                        alter_spread.Sheets[0].ColumnHeader.Cells[1, 4].Text = "Staff Name";
                                        alter_spread.Sheets[0].ColumnHeader.Cells[1, 5].Text = "Subject Name";
                                        alter_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 2);

                                        alter_spread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Reason";
                                        alter_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

                                        alter_spread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Alternate Details";
                                        alter_spread.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Staff Name";
                                        alter_spread.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Subject Name";
                                        alter_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 2);

                                        alter_spread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                        alter_spread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;

                                        alter_spread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                        alter_spread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                        alter_spread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                                        alter_spread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                                        alter_spread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                                        alter_spread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                                        alter_spread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                                        alter_spread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                                        alter_spread.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;

                                        alter_spread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                        alter_spread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                        alter_spread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                        alter_spread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                        alter_spread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);



                                        string alternate_query = "select * from alternate_schedule where fromdate between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' and degree_code in (" + mainvalue + ") order by fromdate,Batch_year,degree_code,Semester,Sections ";
                                        alternate_query = alternate_query + " ; Select No_of_hrs_per_day,schorder,nodays,degree_code,semester from periodattndschedule where  degree_code in (" + mainvalue + ") ";
                                        alternate_query = alternate_query + " ; select * from seminfo where  degree_code in (" + mainvalue + ") ";
                                        alternate_query = alternate_query + " ; select distinct current_semester from registration where cc=0 and delflag=0 and  degree_code in (" + mainvalue + ") ";
                                        alternate_query = alternate_query + " ; select c.course_name+'-'+de.dept_acronym as degreedetails,d.Degree_Code from Degree d,Department de,course c where d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and degree_code in (" + mainvalue + ")";
                                        alternate_query = alternate_query + " ; select staff_name,staff_code from staffmaster ";
                                        alternate_query = alternate_query + " ; select subject_name,subject_no from subject";
                                        alternate_query = alternate_query + " ; select * from Semester_Schedule order by FromDate desc";
                                        DataSet dsalldetails = d2.select_method_wo_parameter(alternate_query, "Text");

                                        arlst.Clear();
                                        //if (yrddl.SelectedItem.ToString().Trim() == "All")
                                        //{
                                        //    dsalldetails.Tables[3].DefaultView.RowFilter = " ";
                                        //    DataView dvseme = dsalldetails.Tables[3].DefaultView;
                                        //    for (int readallsem = 0; readallsem < dvseme.Count; readallsem++)
                                        //    {
                                        //        if (allsemval == "")
                                        //        {
                                        //            allsemval = dvseme[readallsem][0].ToString();
                                        //        }
                                        //        else
                                        //        {
                                        //            allsemval = allsemval + "," + dvseme[readallsem][0].ToString();
                                        //        }
                                        //        arlst.Add(dvseme[readallsem][0].ToString());
                                        //    }
                                        //    str_year = " and semester in(" + allsemval + " )";
                                        //    str_year = "";
                                        //}
                                        //else
                                        //{
                                        if (chklsbatch.Items.Count > 0)
                                        {
                                            for (int i = 0; i < chklsbatch.Items.Count; i++)
                                            {
                                                if (chklsbatch.Items[i].Selected == true)
                                                {
                                                    string year_find = string.Empty;
                                                    string year_find2 = string.Empty;
                                                    year_find = (Convert.ToInt16(chklsbatch.Items[i].Text) * 2).ToString();
                                                    year_find2 = (Convert.ToInt16(year_find) - 1).ToString();
                                                    str_year = " and semester in(" + semester + " )";
                                                    arlst.Add(year_find);
                                                    arlst.Add(year_find2);
                                                }
                                            }
                                        }
                                        //}


                                        for (day_diff = 0; day_diff <= days; day_diff++)
                                        {
                                            DateTime date_val = dt1.AddDays(day_diff);
                                            strDay = date_val.ToString("ddd");
                                            string date_final = "";
                                            date_value = Convert.ToString(date_val);
                                            string[] date_split = date_value.Split('/');
                                            date_final = date_split[1] + "/" + date_split[0] + "/" + date_split[2];
                                            string[] split_value = date_final.Split(' ');
                                            string[] split_val2 = split_value[0].Split('/');
                                            query_date = split_val2[2] + "/" + split_val2[1] + "/" + split_val2[0];

                                            dsalldetails.Tables[0].DefaultView.RowFilter = " fromdate='" + query_date + "'";
                                            DataView dvalter = dsalldetails.Tables[0].DefaultView;
                                            for (int i_loop = fmhr; i_loop <= tohr; i_loop++)
                                            {
                                                for (int dtalter = 0; dtalter < dvalter.Count; dtalter++)
                                                {
                                                    batchyear = dvalter[dtalter]["batch_year"].ToString();
                                                    semester = dvalter[dtalter]["semester"].ToString();
                                                    degcode = dvalter[dtalter]["degree_code"].ToString();
                                                    section = dvalter[dtalter]["Sections"].ToString();
                                                    if (section.Trim() != "" && section != null)
                                                    {
                                                        strsec = " and sections='" + section + "'";
                                                        grid_section = "-" + section;
                                                    }
                                                    else
                                                    {
                                                        strsec = "";
                                                        grid_section = "";
                                                    }

                                                    if (arlst.Contains(semester) == true)
                                                    {
                                                        dsalldetails.Tables[1].DefaultView.RowFilter = " degree_code='" + degcode + "' and semester = '" + semester + "'";
                                                        DataView dvperiodschedule = dsalldetails.Tables[1].DefaultView;
                                                        if (dvperiodschedule.Count > 0)
                                                        {
                                                            if ((dvperiodschedule[0]["No_of_hrs_per_day"].ToString()) != "")
                                                            {
                                                                intNHrs = Convert.ToInt16(dvperiodschedule[0]["No_of_hrs_per_day"]);
                                                                SchOrder = Convert.ToInt16(dvperiodschedule[0]["schorder"]);
                                                                nodays = Convert.ToInt16(dvperiodschedule[0]["nodays"]);
                                                            }
                                                        }


                                                        dsalldetails.Tables[2].DefaultView.RowFilter = " degree_code='" + degcode + "' and semester='" + semester + "' and batch_year='" + batchyear + "'";
                                                        DataView dvseminfo = dsalldetails.Tables[2].DefaultView;
                                                        if (dvseminfo.Count > 0)
                                                        {
                                                            if ((dvseminfo[0]["start_date"].ToString()) != "" && (dvseminfo[0]["start_date"].ToString()) != null && (dvseminfo[0]["start_date"].ToString()) != "\0" && (dvseminfo[0]["starting_dayorder"].ToString()) != null && (dvseminfo[0]["starting_dayorder"].ToString()) != "")
                                                            {
                                                                string[] tmpdate = dvseminfo[0]["start_date"].ToString().Split(new char[] { ' ' });
                                                                startdate = tmpdate[0].ToString();
                                                                start_dayorder = Convert.ToInt32(dvseminfo[0]["starting_dayorder"].ToString());
                                                            }
                                                            else
                                                            {
                                                                errlbl.Visible = true;
                                                                errlbl.Text = "Update semester Information";
                                                            }

                                                            if (intNHrs > 0)
                                                            {
                                                                if (SchOrder != 0)
                                                                {
                                                                    dtime_str = Convert.ToDateTime(query_date);
                                                                    srt_day = dtime_str.ToString("ddd");
                                                                }
                                                                else
                                                                {
                                                                    todate = query_date;
                                                                    srt_day = d2.findday(todate, degcode, semester, batchyear, startdate, Convert.ToString(nodays), Convert.ToString(start_dayorder));
                                                                }
                                                            }
                                                            sql1 = srt_day + i_loop.ToString();
                                                            if (srt_day != "Sun")
                                                            {
                                                                int alternatsestart = 0;
                                                                Boolean firstflag = false;
                                                                string stralrperiod = dvalter[dtalter][sql1].ToString();
                                                                if (stralrperiod.ToString().Trim() != "" && stralrperiod != null)
                                                                {
                                                                    subj_staff_s = stralrperiod;
                                                                    string[] subj_staff_s_splt = subj_staff_s.Split(';');
                                                                    if (subj_staff_s_splt.GetUpperBound(0) >= 0)
                                                                    {
                                                                        for (semi_split = 0; semi_split <= subj_staff_s_splt.GetUpperBound(0); semi_split++)
                                                                        {
                                                                            fflag = true;
                                                                            alter_spread.Sheets[0].Visible = true;
                                                                            string[] subj_staff_s_splt2 = subj_staff_s_splt[semi_split].Split('-');
                                                                            if (subj_staff_s_splt2.GetUpperBound(0) >= 1)
                                                                            {
                                                                                string reason = "";
                                                                                string staffnamede = "";
                                                                                for (int st = 1; st < subj_staff_s_splt2.GetUpperBound(0); st++)
                                                                                {
                                                                                    dsalldetails.Tables[5].DefaultView.RowFilter = "staff_code= '" + subj_staff_s_splt2[st].ToString() + "'";
                                                                                    DataView dvstaff = dsalldetails.Tables[5].DefaultView;
                                                                                    if (dvstaff.Count > 0)
                                                                                    {
                                                                                        string staffname = Convert.ToString(dvstaff[0]["staff_name"]);
                                                                                        if (staffname != null && staffname.Trim() != "")
                                                                                        {
                                                                                            if (staffnamede.Trim() == "")
                                                                                            {
                                                                                                staffnamede = staffname.ToString();
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                staffnamede = staffnamede + " , " + staffname.ToString();
                                                                                            }
                                                                                        }
                                                                                    }

                                                                                    string leaveres = d2.GetFunction("select remarks from staff_leave_details where apply_approve<>'2' and staff_code= '" + subj_staff_s_splt2[st].ToString() + "' and '" + query_date + "' between fdate and tdate  ");
                                                                                    if (leaveres.Trim() != "" && leaveres.Trim() != "0" && leaveres != null)
                                                                                    {
                                                                                        if (reason == "")
                                                                                        {
                                                                                            reason = leaveres;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            reason = reason + "," + leaveres;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                if (staffnamede.ToString().Trim() != "")
                                                                                {
                                                                                    subj_code = subj_staff_s_splt2[0].ToString();
                                                                                    staff_code = subj_staff_s_splt2[1].ToString();

                                                                                    int xx = 0;
                                                                                    if (int.TryParse(subj_code, out xx))
                                                                                    {
                                                                                        alter_spread.Sheets[0].RowCount++;
                                                                                        row_val = alter_spread.Sheets[0].RowCount - 1;
                                                                                        alter_spread.Sheets[0].Cells[row_val, 1].Text = split_value[0].ToString();//set date
                                                                                        alter_spread.Sheets[0].Cells[row_val, 2].Text = i_loop.ToString();//set period
                                                                                        if (firstflag == false)
                                                                                        {
                                                                                            alternatsestart = alter_spread.Sheets[0].RowCount - 1;
                                                                                            firstflag = true;
                                                                                        }


                                                                                        dsalldetails.Tables[4].DefaultView.RowFilter = "degree_code= '" + degcode + "'";
                                                                                        DataView dvdegree = dsalldetails.Tables[4].DefaultView;
                                                                                        if (dvdegree.Count > 0)
                                                                                        {
                                                                                            alter_spread.Sheets[0].Cells[row_val, 3].Text = batchyear.ToString() + "-" + dvdegree[0]["degreedetails"].ToString() + "-" + semester + grid_section;
                                                                                            alter_spread.Sheets[0].Cells[row_val, 3].Tag = text_val.ToString() + "-" + batchyear.ToString() + "-" + dvdegree[0]["degreedetails"].ToString() + "-" + semester + grid_section;
                                                                                        }

                                                                                        dsalldetails.Tables[6].DefaultView.RowFilter = "subject_no= '" + subj_code + "'";
                                                                                        DataView dvsubject = dsalldetails.Tables[6].DefaultView;
                                                                                        if (dvsubject.Count > 0)
                                                                                        {
                                                                                            text_val = dvsubject[0]["subject_name"].ToString();
                                                                                        }
                                                                                        alter_spread.Sheets[0].Cells[row_val, 6].Text = reason.ToString();
                                                                                        alter_spread.Sheets[0].Cells[row_val, 8].Text = text_val.ToString();
                                                                                        alter_spread.Sheets[0].Cells[row_val, 7].Text = staffnamede.ToString();

                                                                                        asql = split_value[0].ToString();
                                                                                        if (Strsql == "")
                                                                                        {
                                                                                            Strsql = asql;
                                                                                        }
                                                                                        if (Strsql != asql)
                                                                                        {
                                                                                            if (alter_spread.Sheets[0].RowCount == 1)
                                                                                            {
                                                                                                sno++;
                                                                                                alter_spread.Sheets[0].Cells[alter_spread.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                sno++;
                                                                                                alter_spread.Sheets[0].Cells[alter_spread.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                                            }
                                                                                            span_count = 1;
                                                                                            alter_spread.Sheets[0].SpanModel.Add(alter_spread.Sheets[0].RowCount - span_count, 0, span_count, 1);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            alter_spread.Sheets[0].Cells[alter_spread.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                                            span_count++;
                                                                                            alter_spread.Sheets[0].SpanModel.Add((alter_spread.Sheets[0].RowCount - span_count), 0, span_count, 1);
                                                                                        }
                                                                                        Strsql = asql;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    //========Semester Details==============
                                                                    firstflag = false;
                                                                    int altsro = alternatsestart;
                                                                    int rowspa = 1;
                                                                    dsalldetails.Tables[7].DefaultView.RowFilter = " degree_code='" + degcode + "' and semester='" + semester + "' and batch_year='" + batchyear + "' " + strsec + " and FromDate<='" + query_date + "'";
                                                                    DataView dvsemsched = dsalldetails.Tables[7].DefaultView;
                                                                    if (dvsemsched.Count > 0)
                                                                    {
                                                                        string getfied = Convert.ToString(dvsemsched[0][sql1]);
                                                                        if (getfied.Trim() != "" && getfied != null && getfied != "\0")
                                                                        {
                                                                            string[] subsched = getfied.Split(';');
                                                                            if (subsched.GetUpperBound(0) >= 0)
                                                                            {
                                                                                for (semi_split = 0; semi_split <= subsched.GetUpperBound(0); semi_split++)
                                                                                {
                                                                                    string[] subj_staff_s_splt2 = subsched[semi_split].Split('-');
                                                                                    if (subj_staff_s_splt2.GetUpperBound(0) >= 1)
                                                                                    {
                                                                                        string staffnamede = "";
                                                                                        for (int st = 1; st < subj_staff_s_splt2.GetUpperBound(0); st++)
                                                                                        {
                                                                                            dsalldetails.Tables[5].DefaultView.RowFilter = "staff_code= '" + subj_staff_s_splt2[st].ToString() + "'";
                                                                                            DataView dvstaff = dsalldetails.Tables[5].DefaultView;
                                                                                            if (dvstaff.Count > 0)
                                                                                            {
                                                                                                string staffname = Convert.ToString(dvstaff[0]["staff_name"]);
                                                                                                if (staffname != null && staffname.Trim() != "")
                                                                                                {
                                                                                                    if (staffnamede.Trim() == "")
                                                                                                    {
                                                                                                        staffnamede = staffname.ToString();
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        staffnamede = staffnamede + " , " + staffname.ToString();
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        if (subj_staff_s_splt2[1].ToString() != string.Empty)
                                                                                        {
                                                                                            subj_code = subj_staff_s_splt2[0].ToString();
                                                                                            staff_code = subj_staff_s_splt2[1].ToString();

                                                                                            int xx = 0;
                                                                                            if (int.TryParse(subj_code, out xx))
                                                                                            {
                                                                                                dsalldetails.Tables[6].DefaultView.RowFilter = "subject_no= '" + subj_code + "'";
                                                                                                DataView dvsubject = dsalldetails.Tables[6].DefaultView;
                                                                                                if (dvsubject.Count > 0)
                                                                                                {
                                                                                                    text_val = dvsubject[0]["subject_name"].ToString();
                                                                                                }
                                                                                                if (firstflag == true)
                                                                                                {
                                                                                                    alternatsestart++;
                                                                                                    if (alternatsestart >= alter_spread.Sheets[0].RowCount)
                                                                                                    {
                                                                                                        rowspa++;
                                                                                                        alter_spread.Sheets[0].RowCount++;
                                                                                                    }
                                                                                                    row_val = alter_spread.Sheets[0].RowCount - 1;
                                                                                                    alter_spread.Sheets[0].Cells[alternatsestart, 1].Text = split_value[0].ToString();//set date
                                                                                                    alter_spread.Sheets[0].Cells[alternatsestart, 2].Text = i_loop.ToString();//set period


                                                                                                    dsalldetails.Tables[4].DefaultView.RowFilter = "degree_code= '" + degcode + "'";
                                                                                                    DataView dvdegree = dsalldetails.Tables[4].DefaultView;
                                                                                                    if (dvdegree.Count > 0)
                                                                                                    {
                                                                                                        alter_spread.Sheets[0].Cells[alternatsestart, 3].Text = batchyear.ToString() + "-" + dvdegree[0]["degreedetails"].ToString() + "-" + semester + grid_section;
                                                                                                        alter_spread.Sheets[0].Cells[alternatsestart, 3].Tag = text_val.ToString() + "-" + batchyear.ToString() + "-" + dvdegree[0]["degreedetails"].ToString() + "-" + semester + grid_section;
                                                                                                    }
                                                                                                }
                                                                                                alter_spread.Sheets[0].Cells[alternatsestart, 5].Text = text_val.ToString();
                                                                                                alter_spread.Sheets[0].Cells[alternatsestart, 4].Text = staffnamede;
                                                                                                firstflag = true;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    if (rowspa > 1)
                                                                    {
                                                                        alter_spread.Sheets[0].SpanModel.Add(altsro, 7, rowspa, 1);
                                                                        alter_spread.Sheets[0].SpanModel.Add(altsro, 8, rowspa, 1);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (fflag == false)
                                        {
                                            errlbl.Visible = true;
                                            errlbl.Text = "No Record Found";
                                        }
                                        else
                                        {
                                            alter_spread.Visible = true;
                                            errlbl.Visible = false;
                                            lblrptname.Visible = true;
                                            txtexcelname.Visible = true;
                                            btnxl.Visible = true;
                                            btnprintmaster.Visible = true;
                                        }
                                    }
                                    else
                                    {
                                        errlbl.Visible = true;
                                        errlbl.Text = "To Period Should Be Greater Than From Period";
                                    }
                                }
                                else
                                {
                                    errlbl.Visible = true;
                                    errlbl.Text = "To Date Should Be Greater Than From Date";
                                }
                            }
                            else
                            {
                                errlbl.Visible = true;
                                errlbl.Text = "Enter Valid To Date";
                            }
                        }
                        else
                        {
                            errlbl.Visible = true;
                            errlbl.Text = "Enter Valid To Date";
                        }
                    }
                    else
                    {
                        alter_spread.Visible = false;

                        errlbl.Visible = false;
                        errlbl.Visible = true;
                        errlbl.Text = "Enter Valid From Date";
                    }
                }
                else
                {
                    alter_spread.Visible = false;

                    errlbl.Visible = false;
                    errlbl.Visible = true;
                    errlbl.Text = "Enter Valid From Date";
                }

                if (Convert.ToInt32(alter_spread.Sheets[0].RowCount) != 0)
                {
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(alter_spread.Sheets[0].RowCount);
                    if (totalRows >= 10)
                    {
                        alter_spread.Sheets[0].PageSize = alter_spread.Sheets[0].RowCount;
                        alter_spread.Height = 410;
                        alter_spread.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                        alter_spread.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    }
                    else if (totalRows == 0)
                    {
                        alter_spread.Height = 200;
                    }
                    else
                    {
                        alter_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);

                        alter_spread.Height = 30 + (38 * Convert.ToInt32(totalRows));
                    }
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / alter_spread.Sheets[0].PageSize);
                    if (Convert.ToInt32(alter_spread.Sheets[0].RowCount) == 0)
                    {

                        alter_spread.Visible = false;
                        errlbl.Visible = true;
                        errlbl.Text = "No Records found";
                    }
                }
                else
                {
                    alter_spread.Visible = false;
                    errlbl.Visible = true;
                    errlbl.Text = "No Records found";
                }
            }
            else
            {
                alter_spread.Visible = false;
                errlbl.Visible = true;
                errlbl.Text = "Please Select All Fields";
            }
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        Control cntPageNextBtn = alter_spread.FindControl("Next");
        Control cntPagePreviousBtn = alter_spread.FindControl("Prev");

        if ((cntPageNextBtn != null))
        {

            TableCell tc = (TableCell)cntPageNextBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);
            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);
        }

        base.Render(writer);
    }
    protected void fromdatetxt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string strf = fromdatetxt.Text.ToString();
            string strt = todatetxt.Text.ToString();
            string[] stf = strf.Split('/');
            string[] stt = strt.Split('/');
            DateTime df = Convert.ToDateTime(stf[1] + '/' + stf[0] + '/' + stf[2]);
            DateTime dt = Convert.ToDateTime(stt[1] + '/' + stt[0] + '/' + stt[2]);
            if (df > dt)
            {
                errlbl.Visible = true;
                errlbl.Text = "Please Entrt From Date Must Be Lesser than or Equal to To Date";
                fromdatetxt.Text = strt;
            }
        }
        catch
        {
        }
    }
    protected void todatetxt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string strf = fromdatetxt.Text.ToString();
            string strt = todatetxt.Text.ToString();
            string[] stf = strf.Split('/');
            string[] stt = strt.Split('/');
            DateTime df = Convert.ToDateTime(stf[1] + '/' + stf[0] + '/' + stf[2]);
            DateTime dt = Convert.ToDateTime(stt[1] + '/' + stt[0] + '/' + stt[2]);
            if (df > dt)
            {
                errlbl.Visible = true;
                errlbl.Text = "Please Entrt From Date Must Be Lesser than or Equal to To Date";
                fromdatetxt.Text = strt;
            }
        }
        catch
        {
        }
    }
    protected void fromhrddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string frange = fromhrddl.Text;
            string trgane = tohrddl.Text;
            int ft = Convert.ToInt32(frange);
            int tt = Convert.ToInt32(trgane);
            if (ft > tt)
            {
                errlbl.Visible = true;
                errlbl.Text = "To Period Should Be Greater Than From Period";
                fromhrddl.Text = "1";
            }
        }
        catch
        {
        }
    }
    protected void tohrddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string frange = fromhrddl.Text;
            string trgane = tohrddl.Text;
            int ft = Convert.ToInt32(frange);
            int tt = Convert.ToInt32(trgane);
            if (ft > tt)
            {
                errlbl.Visible = true;
                errlbl.Text = "To Period Should Be Greater Than From Period";
                fromhrddl.Text = "1";
            }
        }
        catch
        {
        }
    }
    protected void yrddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(alter_spread, reportname);
            }
            else
            {
                errlbl.Text = "Please Enter Your Report Name";
                errlbl.Visible = true;
            }
        }
        catch (Exception ex)
        {
            errlbl.Text = ex.ToString();
        }

    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "FACULTY ALTERNATE DETAILS" + " @ Date : " + fromdatetxt.Text.ToString() + " To " + todatetxt.Text.ToString();
        string pagename = "timetablechangerreport.aspx";
        Printcontrol.loadspreaddetails(alter_spread, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    public void clear()
    {
        btnprintmaster.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        Printcontrol.Visible = false;
        txtexcelname.Visible = false;
        errlbl.Visible = false;
        errlbl.Visible = false;
        alter_spread.Visible = false;
        txtexcelname.Text = "";
    }

    ////Start=================findday Method Added by Manikandan 13/08/2013
    //public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    //{
    //    int holiday = 0;
    //    if (no_days == "")
    //        return "";
    //    if (sdate != "")
    //    {
    //        string[] sp_date = sdate.Split(new Char[] { '/' });
    //        string start_date = sp_date[1].ToString() + "-" + sp_date[2].ToString() + "-" + sp_date[0].ToString();
    //        DateTime dt1 = Convert.ToDateTime(sdate);
    //        DateTime dt2 = Convert.ToDateTime(curday);
    //        TimeSpan ts = dt2 - dt1;
    //        string query1 = "select count(*)as count from holidaystudents  where degree_code=" + deg_code + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "'";
    //        string holday = d2.GetFunction(query1);
    //        if (holday != "")
    //            holiday = Convert.ToInt32(holday);
    //        int dif_days = ts.Days;
    //        int nodays = Convert.ToInt32(no_days);
    //        int order = (dif_days - holiday) % nodays;
    //        order = order + 1;

    //        if (stastdayorder.ToString().Trim() != "")
    //        {
    //            if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
    //            {
    //                order = order + (Convert.ToInt16(stastdayorder) - 1);
    //                if (order == (nodays + 1))
    //                    order = 1;
    //                else if (order > nodays)
    //                    order = order % nodays;
    //            }
    //        }
    //        string findday = "";
    //        if (order == 1)
    //            findday = "mon";
    //        else if (order == 2) findday = "tue";
    //        else if (order == 3) findday = "wed";
    //        else if (order == 4) findday = "thu";
    //        else if (order == 5) findday = "fri";
    //        else if (order == 6) findday = "sat";
    //        else if (order == 7) findday = "sun";

    //        Day_Order = Convert.ToString(order) + "-" + Convert.ToString(findday);
    //        return findday;
    //    }
    //    else
    //        return "";

    //}
}