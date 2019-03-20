using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Collections;
using System.Drawing;
using System.Configuration;


public partial class staffbelltiming : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet dg = new DataSet();
    DAccess2 d2 = new DAccess2();
    string CollegeCode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string usercode = string.Empty;
    static int personmode = 0;
    string popclg1 = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("MasterWizardMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MasterWizardMod/MasterWizardMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            CollegeCode = Convert.ToString(Session["collegecode"]);

            collegecode = Convert.ToString(Session["collegecode"]);

            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            usercode = Session["usercode"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                bindstaffcatType();
                bindyear();
                bindleav();
                bindhostel();
                bindstudyear();
                binddeg();
                binddept();
                bindcoursestud();
                bindsem();
                bindperiod();
                Txtentryto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                // btn_go_Click(sender, e);
            }
        }
        catch (Exception ex) { }
    }
    protected void ddlcol_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            ddlcol.Items.Clear();
            ddl_popclg1.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcol.DataSource = ds;
                ddlcol.DataTextField = "collname";
                ddlcol.DataValueField = "college_code";
                ddlcol.DataBind();

                ddl_popclg1.DataSource = ds;
                ddl_popclg1.DataTextField = "collname";
                ddl_popclg1.DataValueField = "college_code";
                ddl_popclg1.DataBind();

                ddlcollegename.DataSource = ds;
                ddlcollegename.DataTextField = "collname";
                ddlcollegename.DataValueField = "college_code";
                ddlcollegename.DataBind();
            }
        }
        catch { }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet loaddatads = new DataSet();
            ArrayList arrColHdrNames = new ArrayList();

            if (rdbstaff.Checked == true)
            {
                grdhost.Visible = false;
                grdstud.Visible = false;
                loaddatads.Clear();
                //string qry = "select category_name,right(Convert(nvarchar(100),intime,100),7) as intime,right(Convert(nvarchar(100),gracetime,100),7) as gracetime,right(Convert(nvarchar(100),latetime,100),7) as latetime,right(Convert(nvarchar(100),outtime,100),7) as outtime,right(Convert(nvarchar(100),lunch_st_time,100),7) as lunch_st_time,right(Convert(nvarchar(100),lunch_end_time,100),7) as lunch_end_time,nooflate,category_code,ISNULL(StfType,'') StfType,ISNULL(DayType,0) DayType,isnull(shift,'') shift from in_out_time where college_code = '" + ddlcol.SelectedItem.Value + "'";

                string qry = "select category_name,right(Convert(nvarchar(100),intime,100),7) as intime,right(Convert(nvarchar(100),gracetime,100),7) as gracetime,right(Convert(nvarchar(100),latetime,100),7) as latetime,right(Convert(nvarchar(100),outtime,100),7) as outtime,right(Convert(nvarchar(100),lunch_st_time,100),7) as lunch_st_time,right(Convert(nvarchar(100),lunch_end_time,100),7) as lunch_end_time,right(Convert(nvarchar(100),permission_time,100),7) as permission_time,right(Convert(nvarchar(100),extend_gracetime,100),7) as extend_gracetime,right(Convert(nvarchar(100),MorningOutTime,100),7) as MorningOutTime,nooflate,right(Convert(nvarchar(100),CompWorkDate,100),7) as CompWorkDate,category_code,ISNULL(StfType,'') StfType,ISNULL(DayType,0) DayType,isnull(shift,'') shift,noofper,morn_late,morn_per,manual_Settings,isUnRegLeave,UnRegLeave from in_out_time where college_code = '" + ddlcol.SelectedItem.Value + "'";
                if (ddlstafcatmain.SelectedItem.Text != "All")
                {
                    qry = qry + " and category_code='" + ddlstafcatmain.SelectedItem.Value + "'";
                }
                loaddatads = d2.select_method_wo_parameter(qry, "text");
                DataTable dtstaffbel = new DataTable();
                DataRow drow;
                if (loaddatads.Tables[0].Rows.Count > 0)
                {
                    arrColHdrNames.Add("S.No");
                    dtstaffbel.Columns.Add("Sno");
                    arrColHdrNames.Add("CategoryName/StaffType");
                    dtstaffbel.Columns.Add("stfcattype");
                    arrColHdrNames.Add("Shift");
                    dtstaffbel.Columns.Add("shift");
                    arrColHdrNames.Add("In Time");
                    dtstaffbel.Columns.Add("intime");
                    arrColHdrNames.Add("Grace Time");
                    dtstaffbel.Columns.Add("gracetime");
                    arrColHdrNames.Add("Late Time");
                    dtstaffbel.Columns.Add("latetime");
                    arrColHdrNames.Add("Out Time");
                    dtstaffbel.Columns.Add("outtime");
                    arrColHdrNames.Add("Lunch Start Time");
                    dtstaffbel.Columns.Add("lunch_st_time");
                    arrColHdrNames.Add("Lunch End Time");
                    dtstaffbel.Columns.Add("lunch_end_time");

                    arrColHdrNames.Add("No Of Late Allowed");
                    dtstaffbel.Columns.Add("Nooflate");

                    arrColHdrNames.Add("Permission Time");
                    dtstaffbel.Columns.Add("permission_time");

                    arrColHdrNames.Add("Extended Grace Time");
                    dtstaffbel.Columns.Add("extend_gracetime");

                    arrColHdrNames.Add("Morning Out Time");
                    dtstaffbel.Columns.Add("MorningOutTime");

                    arrColHdrNames.Add("No Of Permission");
                    dtstaffbel.Columns.Add("noofper");
                    arrColHdrNames.Add("MrngLate");
                    dtstaffbel.Columns.Add("morn_late");
                    arrColHdrNames.Add("MrngPer");
                    dtstaffbel.Columns.Add("morn_per");
                    arrColHdrNames.Add("Manual Setting");
                    dtstaffbel.Columns.Add("manual_Settings");
                    arrColHdrNames.Add("Is unreg");
                    dtstaffbel.Columns.Add("isUnRegLeave");
                    arrColHdrNames.Add("UnReg");
                    dtstaffbel.Columns.Add("UnRegLeave");
                    DataRow drHdr1 = dtstaffbel.NewRow();
                    for (int grCol = 0; grCol < dtstaffbel.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    }
                    dtstaffbel.Rows.Add(drHdr1);
                    int sno = 0;
                    if (loaddatads.Tables[0].Rows.Count > 0)
                    {
                        for (int val = 0; val < loaddatads.Tables[0].Rows.Count; val++)
                        {
                            string stafftypeorcat = string.Empty;
                            sno++;

                            string category = Convert.ToString(loaddatads.Tables[0].Rows[val]["category_name"]);
                            string stafftype = Convert.ToString(loaddatads.Tables[0].Rows[val]["StfType"]);
                            string shift = Convert.ToString(loaddatads.Tables[0].Rows[val]["Shift"]);
                            string intime = Convert.ToString(loaddatads.Tables[0].Rows[val]["intime"]);
                            string gracetime = Convert.ToString(loaddatads.Tables[0].Rows[val]["gracetime"]);
                            string latetime = Convert.ToString(loaddatads.Tables[0].Rows[val]["latetime"]);
                            string outtime = Convert.ToString(loaddatads.Tables[0].Rows[val]["outtime"]);
                            string lunctstart = Convert.ToString(loaddatads.Tables[0].Rows[val]["lunch_st_time"]);
                            string lunchend = Convert.ToString(loaddatads.Tables[0].Rows[val]["lunch_end_time"]);
                            string noflate = Convert.ToString(loaddatads.Tables[0].Rows[val]["nooflate"]);
                            string pertime = Convert.ToString(loaddatads.Tables[0].Rows[val]["permission_time"]);
                            string extgracTime = Convert.ToString(loaddatads.Tables[0].Rows[val]["extend_gracetime"]);
                            string MrngOutTime = Convert.ToString(loaddatads.Tables[0].Rows[val]["MorningOutTime"]);
                            string noofper = Convert.ToString(loaddatads.Tables[0].Rows[val]["noofper"]);
                            string morlate = Convert.ToString(loaddatads.Tables[0].Rows[val]["morn_late"]);
                            string morper = Convert.ToString(loaddatads.Tables[0].Rows[val]["morn_per"]);
                            string ismanualsetting = Convert.ToString(loaddatads.Tables[0].Rows[val]["manual_Settings"]);
                            string isunreggflag = Convert.ToString(loaddatads.Tables[0].Rows[val]["isUnRegLeave"]);
                            string unreggflag = Convert.ToString(loaddatads.Tables[0].Rows[val]["UnRegLeave"]);

                            if (category != "")
                            {
                                stafftypeorcat = category;
                            }
                            else
                            {
                                stafftypeorcat = stafftype;
                            }
                            drow = dtstaffbel.NewRow();
                            drow[0] = Convert.ToString(sno);
                            drow[1] = stafftypeorcat;
                            drow[2] = shift;
                            drow[3] = intime;
                            drow[4] = gracetime;
                            drow[5] = latetime;
                            drow[6] = outtime;
                            drow[7] = lunctstart;
                            drow[8] = lunchend;
                            drow[9] = noflate;
                            drow[10] = pertime;
                            drow[11] = extgracTime;
                            drow[12] = MrngOutTime;
                            drow[13] = noofper;
                            drow[14] = morlate;
                            drow[15] = morper;
                            drow[16] = ismanualsetting;
                            drow[17] = isunreggflag;
                            drow[18] = unreggflag;


                            dtstaffbel.Rows.Add(drow);
                        }
                    }

                    grdstaff.DataSource = dtstaffbel;
                    grdstaff.DataBind();
                    grdstaff.Visible = true;

                }
                else
                {
                    grdstaff.Visible = false;
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);

                }


            }

            if (rdbhostel.Checked == true)
            {
                grdstaff.Visible = false;
                grdstud.Visible = false;
                loaddatads.Clear();
                string qry = "select HostelName,right(Convert(nvarchar(100),Out_Time,100),7) as Out_Time,right(Convert(nvarchar(100),Grace_Time,100),7) as Grace_Time,right(Convert(nvarchar(100),ExtGrace_Time,100),7) as ExtGrace_Time,right(Convert(nvarchar(100),In_Time,100),7) as In_Time,right(Convert(nvarchar(100),Permission_Time,100),7) as Permission_Time,right(Convert(nvarchar(100),Late_Time,100),7) as Late_Time,Tot_Late,I.Hostel_Code,ISNULL(RegType,0) RegType,right(Convert(nvarchar(100),MorLate_Time,100),7) as MorLate_Time FROM Hostel_InOut_Time I,HM_HostelMaster H WHERE I.Hostel_Code = H.HostelMasterPK and I.College_Code ='" + ddlcol.SelectedItem.Value + "'";//I.College_Code ='" + ddlcol.SelectedItem.Value + "'

                if (ddlhostel.SelectedItem.Text != "All")
                {
                    qry = qry + " and Hostel_Code ='" + ddlhostel.SelectedItem.Value + "'";

                }
                loaddatads = d2.select_method_wo_parameter(qry, "text");
                DataTable dtstaffbel = new DataTable();
                DataRow drow;
                if (loaddatads.Tables[0].Rows.Count > 0)
                {
                    arrColHdrNames.Add("S.No");
                    dtstaffbel.Columns.Add("Sno");
                    arrColHdrNames.Add("Hostel Name");
                    dtstaffbel.Columns.Add("hostelname");
                    arrColHdrNames.Add("Out Time");
                    dtstaffbel.Columns.Add("outtime");
                    arrColHdrNames.Add("Morning Late Time");
                    dtstaffbel.Columns.Add("mrnglatetime");
                    arrColHdrNames.Add("In Time");
                    dtstaffbel.Columns.Add("intime");
                    arrColHdrNames.Add("Late Time");
                    dtstaffbel.Columns.Add("latetime");
                    arrColHdrNames.Add("No Of Late Allowed");
                    dtstaffbel.Columns.Add("nooflateallowed");


                    DataRow drHdr1 = dtstaffbel.NewRow();
                    for (int grCol = 0; grCol < dtstaffbel.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    }
                    dtstaffbel.Rows.Add(drHdr1);
                    int sno = 0;
                    if (loaddatads.Tables[0].Rows.Count > 0)
                    {
                        for (int val = 0; val < loaddatads.Tables[0].Rows.Count; val++)
                        {
                            sno++;
                            string hostelname = Convert.ToString(loaddatads.Tables[0].Rows[val]["HostelName"]);
                            string outtimeget = Convert.ToString(loaddatads.Tables[0].Rows[val]["Out_Time"]);
                            string mrnglate = Convert.ToString(loaddatads.Tables[0].Rows[val]["MorLate_Time"]);
                            // string extndgracetime=Convert.ToString(loaddatads.Tables[0].Rows[val]["ExtGrace_Time"]);
                            string intimeget = Convert.ToString(loaddatads.Tables[0].Rows[val]["In_Time"]);
                            // string  pertime=Convert.ToString(loaddatads.Tables[0].Rows[val]["Permission_Time"]);
                            string latetimeget = Convert.ToString(loaddatads.Tables[0].Rows[val]["Late_Time"]);
                            string totlate = Convert.ToString(loaddatads.Tables[0].Rows[val]["Tot_Late"]);
                            drow = dtstaffbel.NewRow();
                            drow[0] = Convert.ToString(sno);
                            drow[1] = hostelname;
                            drow[2] = outtimeget;
                            drow[3] = mrnglate;
                            drow[4] = intimeget;
                            drow[5] = latetimeget;
                            drow[6] = totlate;
                            dtstaffbel.Rows.Add(drow);
                        }
                    }

                    grdhost.DataSource = dtstaffbel;
                    grdhost.DataBind();
                    grdhost.Visible = true;

                }
                else
                {
                    grdhost.Visible = false;
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);

                }

            }
            if (rdbstudent.Checked == true)
            {
                grdstaff.Visible = false;
                grdhost.Visible = false;
                loaddatads.Clear();
                string studentselquery = "select right(Convert(nvarchar(100),start_time,100),7) as start_time,right(Convert(nvarchar(100),end_time,100),7) as end_time,Period1,Desc1,Degree_Code,Semester,batch_year from bellschedule where semester='" + ddlstusem.SelectedItem.Text + "' and batch_year='" + ddlstubatchyear.SelectedItem.Text + "' and Degree_Code='" + ddlstudegreecode.SelectedItem.Value + "'";

                loaddatads = d2.select_method_wo_parameter(studentselquery, "text");
                DataTable dtstudbel = new DataTable();
                DataRow drow;
                if (loaddatads.Tables[0].Rows.Count > 0)
                {
                    arrColHdrNames.Add("S.No");
                    dtstudbel.Columns.Add("Sno");
                    arrColHdrNames.Add("Period");
                    dtstudbel.Columns.Add("period");
                    arrColHdrNames.Add("Start Time");
                    dtstudbel.Columns.Add("starttime");
                    arrColHdrNames.Add("End Time");
                    dtstudbel.Columns.Add("endtime");
                    arrColHdrNames.Add("Description");
                    dtstudbel.Columns.Add("description");
                    arrColHdrNames.Add("sembatchdegree");
                    dtstudbel.Columns.Add("sembatchdegree");


                }
                DataRow drHdr1 = dtstudbel.NewRow();
                for (int grCol = 0; grCol < dtstudbel.Columns.Count; grCol++)
                {
                    drHdr1[grCol] = arrColHdrNames[grCol];
                }
                dtstudbel.Rows.Add(drHdr1);

                int sno = 0;
                if (loaddatads.Tables[0].Rows.Count > 0)
                {
                    for (int val = 0; val < loaddatads.Tables[0].Rows.Count; val++)
                    {
                        sno++;
                        string sembatchdegree = string.Empty;
                        string getsem = Convert.ToString(loaddatads.Tables[0].Rows[val]["Semester"]);
                        string batchyr = Convert.ToString(loaddatads.Tables[0].Rows[val]["batch_year"]);
                        string degree = Convert.ToString(loaddatads.Tables[0].Rows[val]["Degree_Code"]);
                        sembatchdegree = getsem + "-" + batchyr + "-" + degree;
                        string period = Convert.ToString(loaddatads.Tables[0].Rows[val]["Period1"]);
                        string starttimegrt = Convert.ToString(loaddatads.Tables[0].Rows[val]["start_time"]);
                        string endtimeget = Convert.ToString(loaddatads.Tables[0].Rows[val]["end_time"]);
                        string desc = Convert.ToString(loaddatads.Tables[0].Rows[val]["Desc1"]);
                        drow = dtstudbel.NewRow();
                        drow[0] = Convert.ToString(sno);
                        drow[1] = period;
                        drow[2] = starttimegrt;
                        drow[3] = endtimeget;
                        drow[4] = desc;
                        drow[5] = sembatchdegree;

                        dtstudbel.Rows.Add(drow);
                    }

                    grdstud.DataSource = dtstudbel;
                    grdstud.DataBind();
                    grdstud.Visible = true;
                    for (int i = 0; i < grdstud.Rows.Count; i++)
                    {
                        for (int j = 0; j < grdstud.HeaderRow.Cells.Count; j++)
                        {
                            if (j == 0)
                            {
                                grdstud.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                grdstud.Rows[i].Cells[j].Width = 40;

                            }
                            if (j == 1)
                            {
                                grdstud.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                grdstud.Rows[i].Cells[j].Width = 40;

                            }
                            if (j == 2)
                            {
                                grdstud.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                grdstud.Rows[i].Cells[j].Width = 100;

                            }
                            if (j == 3)
                            {
                                grdstud.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                grdstud.Rows[i].Cells[j].Width = 100;

                            }
                            if (j == 4)
                            {
                                grdstud.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                grdstud.Rows[i].Cells[j].Width = 250;

                            }
                            if (j == 5)
                            {
                                grdstud.Rows[i].Cells[j].Visible = false;

                            }

                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
                }
            }


        }
        catch (Exception ex)
        {

        }
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdbstaff.Checked == true)
            {
                btnsave.Visible = true;
                btnupdate.Visible = false;
                btndel.Visible = false;
                popper1.Visible = true;
                rdballdays.Checked = true;
                rdbselecteddate.Checked = false;
                rdbseelctedday.Checked = false;
                ddlsingleday.Visible = false;
                rdbstaffcategory.Checked = true;
                rdbstafftype.Checked = false;
                rdbstaffcategory.Text = "Staff Category";
                bindstaffcatType();
                bindshift();
                lblmonth.Visible = true;
                ddlmonth.Visible = true;
                lblyear.Visible = true;
                ddlyear.Visible = true;
                lbldate.Visible = false;
                Txtentryto.Visible = true;
                rdbselecteddatecomp.Enabled = false;
                rdbselecteddaycomp.Enabled = false;
                rdbselecteddatecomp.Checked = false;
                rdbselecteddaycomp.Checked = false;
                cbcompworking.Checked = false;
                ddlcompworking.Enabled = false;
                bindleav();
                DateTime FromTime = DateTime.Parse("00:00:00 AM");
                MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                if (FromTime.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                intime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);

                gracetime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                exgracetime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                latetime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                outtimebell.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                starttime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                endtime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                permission.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                eveningout.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
            }
            if (rdbhostel.Checked == true)
            {
                bindhostel();
                poppe2.Visible = true;
                btn_hostelsave.Visible = true;
                btn_hostelupdate.Visible = false;
            }
            if (rdbstudent.Checked == true)//delju
            {
                bindstudyear();

                DateTime FromTime = DateTime.Parse("00:00:00 AM");
                MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                if (FromTime.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                stustarttime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);

                stuendtime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                btnstudsave.Visible = true;
                btnstudexit.Visible = true;
                bthndeletestud.Visible = false;
                btnstudupdate.Visible = false;
                //DataSet studds = new DataSet();
                //string stuqry = "select distinct(degree_code),duration,First_Year_Nonsemester,(course_name + ' - ' + dept_name) as Department from degree,course,department where department.dept_code=degree.dept_code and course.course_id=degree.course_id and degree.college_code='" + Convert.ToString(Session["collegecode"]) + "' order by degree_code";
                //studds = d2.select_method_wo_parameter(stuqry, "text");
                //if (studds.Tables[0].Rows.Count > 0)
                //{
                //    DataSet dskit = new DataSet();
                //    DataTable dtstud = new DataTable();
                //    DataRow drow;

                //    dtstud.Columns.Add("DeptName", typeof(string));
                //    dtstud.Columns.Add("DeptCode", typeof(string));
                //    dtstud.Columns.Add("chkval", typeof(string));
                //    for (int row = 0; row < studds.Tables[0].Rows.Count; row++)
                //    {
                //        drow = dtstud.NewRow();
                //        drow["DeptName"] = Convert.ToString(studds.Tables[0].Rows[row]["Department"]);
                //        drow["DeptCode"] = Convert.ToString(studds.Tables[0].Rows[row]["degree_code"]);
                //        drow["chkval"] = false;
                //        dtstud.Rows.Add(drow);
                //    }
                //    grdstudbell.DataSource = dtstud;
                //    grdstudbell.DataBind();
                //    grdstudbell.Visible = true;

                //}

                poppe3.Visible = true;

            }
        }
        catch (Exception ex)
        {

        }

    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popper1.Visible = false;
    }
    protected void rdballdays_changed(object sender, EventArgs e)
    {
        if (rdballdays.Checked == true)
        {
            rdbselecteddate.Checked = false;
            rdbseelctedday.Checked = false;
            ddlsingleday.Visible = false;
            lblmonth.Visible = true;
            ddlmonth.Visible = true;
            lblyear.Visible = true;
            ddlyear.Visible = true;
            lbldate.Visible = false;
            Txtentryto.Visible = true;

        }
    }
    protected void rdbselecteddate_changed(object sender, EventArgs e)
    {
        if (rdbselecteddate.Checked == true)
        {
            rdballdays.Checked = false;
            rdbseelctedday.Checked = false;
            ddlsingleday.Visible = false;
            lbldate.Visible = true;
            Txtentryto.Visible = true;
            lblmonth.Visible = false;
            ddlmonth.Visible = false;
            lblyear.Visible = false;
            ddlyear.Visible = false;

        }
    }
    protected void rdbseelctedday_changed(object sender, EventArgs e)
    {
        if (rdbseelctedday.Checked == true)
        {
            rdballdays.Checked = false;
            rdbselecteddate.Checked = false;
            ddlsingleday.Visible = true;
        }

    }

    protected void rdbstaffcategory_changed(object sender, EventArgs e)
    {

        if (rdbstaffcategory.Checked == true)
        {
            rdbstafftype.Checked = false;
            lablstaff.Text = "Staff Category";
            bindstaffcatType();

        }

    }
    protected void rdbstafftype_changed(object sender, EventArgs e)
    {

        if (rdbstafftype.Checked == true)
        {
            rdbstaffcategory.Checked = false;
            lablstaff.Text = "Staff Type";
            bindstaffcatType();

        }

    }
    protected void bindstaffcatType()
    {
        ds.Clear();
        ddlstaffcategorytype.Items.Clear();
        string statequery = string.Empty;
        if (rdbstaffcategory.Checked == true)
        {
            statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + ddlcol.SelectedItem.Value + "' ";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstaffcategorytype.Enabled = true;
                ddlstaffcategorytype.DataSource = ds;
                ddlstaffcategorytype.DataTextField = "category_Name";
                ddlstaffcategorytype.DataValueField = "category_code";
                ddlstaffcategorytype.DataBind();
            }
        }
        if (rdbstafftype.Checked == true)
        {
            statequery = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + ddlcol.SelectedItem.Value + "'";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstaffcategorytype.Enabled = true;
                ddlstaffcategorytype.DataSource = ds;
                ddlstaffcategorytype.DataTextField = "stftype";
                ddlstaffcategorytype.DataValueField = "stftype";
                ddlstaffcategorytype.DataBind();
            }

        }
        if (rdbstaff.Checked == true)
        {
            statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + ddlcol.SelectedItem.Value + "' ";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlstafcatmain.DataSource = ds;
                ddlstafcatmain.DataTextField = "category_Name";
                ddlstafcatmain.DataValueField = "category_code";
                ddlstafcatmain.DataBind();
                ddlstafcatmain.Items.Insert(0, new ListItem("All", "0"));
            }

        }

    }

    protected void rdbselecteddatecomp_changed(object sender, EventArgs e)
    {
        if (rdbselecteddatecomp.Checked == true)
        {
            rdbselecteddaycomp.Checked = false;
            ddlcompworking.Enabled = false;
        }

    }
    protected void rdbselecteddaycomp_changed(object sender, EventArgs e)
    {
        try
        {
            if (rdbstaffcategory.Checked == true)
            {
                string staffcat = string.Empty;
                string shift = string.Empty;
                string intimes = string.Empty;
                string outtimes = string.Empty;
                string gracetimes = string.Empty;
                staffcat = Convert.ToString(ddlstaffcategorytype.SelectedItem.Value);
                shift = Convert.ToString(ddlshift.SelectedItem.Value);
                if (rdbselecteddaycomp.Checked == true)
                {
                    rdbselecteddatecomp.Checked = false;
                }

                //  DateTime F_time = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", intime.Hour, intime.Minute, intime.Second, intime.AmPm));

            }
            if (rdbselecteddaycomp.Checked == true)
            {
                ddlcompworking.Enabled = true;
            }


        }
        catch (Exception ex)
        {

        }

    }
    protected void btnsave_click(object sender, EventArgs e)
    {
        string stafftype = string.Empty;
        string staffcategory = string.Empty;
        DataSet ds = new DataSet();
        int IntEntryType = 0;
        int IntDayType = 0;
        string latevar = string.Empty;
        string pervar = string.Empty;
        int manualsettings = 0;
        if (rdbstaffcategory.Checked == true)
        {
            if (ddlstaffcategorytype.SelectedItem.Text == "")
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Choose a category\");", true);
                return;
            }

        }
        else
        {
            if (ddlstaffcategorytype.SelectedItem.Text == "")
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Choose a Staff Type\");", true);
                return;

            }
        }
        if (ddlshift.SelectedItem.Text.Trim() == "Select")
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Shift\");", true);
            return;
        }
        if (cbunregisteredstaff.Checked == true && ddlunregistered.Text == "")
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Shift\");", true);
            return;
        }
        if (cbcompworking.Checked == true)
        {
            if (rdbselecteddatecomp.Checked == false && rdbselecteddaycomp.Checked == false)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Compulsory working type\");", true);
                return;
            }


        }


        if (cbmanuelsetting.Checked == true)
        {


            if (ddl1.SelectedItem.Text == "Select" && ddl2.SelectedItem.Text == "Select")
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Proper Morning Late Time\");", true);
                return;

            }
            else if (ddl3.SelectedItem.Text == "Select" && ddl4.SelectedItem.Text == "Select")
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Proper Morning Permission Time\");", true);
                return;

            }
            latevar = ddl1.SelectedItem.Text + "-" + ddl2.SelectedItem.Text;
            pervar = ddl3.SelectedItem.Text + "-" + ddl4.SelectedItem.Text;
            manualsettings = 1;

        }
        if (txtnooflate.Text == "")
        {
            txtnooflate.Text = "0";
        }
        if (txtnoofper.Text == "")
        {
            txtnoofper.Text = "0";

        }
        string catcode = string.Empty;
        if (rdbstaffcategory.Checked == true)
        {
            IntEntryType = 0;
            catcode = Convert.ToString(ddlstaffcategorytype.SelectedItem.Value);
            staffcategory = Convert.ToString(ddlstaffcategorytype.SelectedItem.Text);
            //load catcode
        }
        else
        {
            IntEntryType = 1;
            stafftype = Convert.ToString(ddlstaffcategorytype.SelectedItem.Text);

            //load stftype

        }
        if (rdballdays.Checked == true)
        {
            IntDayType = 0;
        }
        else if (rdbselecteddate.Checked == true)
        {
            IntDayType = 1;

        }
        else if (rdbseelctedday.Checked == true)
        {
            IntDayType = 2;
        }

        string sql = string.Empty;


        sql = "SELECT * FROM In_Out_Time WHERE College_Code ='" + Session["collegecode"] + "' AND Shift ='" + ddlshift.SelectedItem.Text + "' AND DayType ='" + IntDayType + "'";

        if (rdbstaffcategory.Checked == true)
        {
            sql = sql + " AND Category_Name ='" + ddlstaffcategorytype.SelectedItem.Text + "'";
        }
        else
        {
            sql = sql + " AND StfType ='" + ddlstaffcategorytype.SelectedItem.Text + "'";

        }
        ds = d2.select_method_wo_parameter(sql, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Bell Timing is already Added\");", true);
            return;
        }
        int val = 0;
        if (chkotherpermission.Checked == true)
        {
            val = 1;
        }
        int checkunregistred = 0;
        string unregisteredleave = string.Empty;
        if (cbunregisteredstaff.Checked == true)
        {
            checkunregistred = 1;
            unregisteredleave = Convert.ToString(ddlunregistered.SelectedItem.Text);
        }
        int checkcompulsarywork = 0;
        int selecteddateorday = 1;
        string compulsarydatetime = string.Empty;
        string compulsaryworkday = string.Empty;
        DateTime comdatetime = new DateTime();

        if (cbcompworking.Checked == true)
        {
            checkcompulsarywork = 1;
            if (rdbselecteddatecomp.Checked == true)
            {
                selecteddateorday = 0;
                compulsarydatetime = Txtentryto.Text;
                if (compulsarydatetime.Contains('/'))
                {
                    string[] splitdate = compulsarydatetime.Split('/');
                    compulsarydatetime = splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2];
                    comdatetime = Convert.ToDateTime(compulsarydatetime);

                }


            }
            if (rdbselecteddaycomp.Checked == true)
            {
                compulsaryworkday = ddlcompworking.SelectedItem.Value;

            }

        }
        else
        {
            compulsarydatetime = Txtentryto.Text;
            if (compulsarydatetime.Contains('/'))
            {
                string[] splitdate = compulsarydatetime.Split('/');
                compulsarydatetime = splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2];
                comdatetime = Convert.ToDateTime(compulsarydatetime);

            }

        }
        //Added By Saranyadevi 7.3.2019
        string Nightshift = "";
        if (chk_NightShift.Checked)
        {
            Nightshift = "1";
        }
        else
        {
            Nightshift = "0";
        }
        int checkcalpayprocessdate = 0;
        if (cbpayprocess.Checked == true)
        {
            checkcalpayprocessdate = 1;
        }
        DateTime getIntime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", intime.Hour, intime.Minute, intime.Second, intime.AmPm));
        DateTime getgracetime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", gracetime.Hour, gracetime.Minute, gracetime.Second, gracetime.AmPm));
        DateTime getLatetime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", latetime.Hour, latetime.Minute, latetime.Second, latetime.AmPm));
        DateTime outtime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", eveningout.Hour, eveningout.Minute, eveningout.Second, eveningout.AmPm));
        DateTime lunchstartT = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", starttime.Hour, starttime.Minute, starttime.Second, starttime.AmPm));
        DateTime lunchEndT = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", endtime.Hour, endtime.Minute, endtime.Second, endtime.AmPm));
        DateTime getperTime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", permission.Hour, permission.Minute, permission.Second, permission.AmPm));
        DateTime getextend_gracetime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", exgracetime.Hour, exgracetime.Minute, exgracetime.Second, exgracetime.AmPm));
        DateTime getmrngouttime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", outtimebell.Hour, outtimebell.Minute, outtimebell.Second, outtimebell.AmPm));

        string qry = string.Empty;//catcode
        int chkupd = 0;
        if (rdballdays.Checked == true)
        {
            IntDayType = 0;
            qry = "insert into in_out_time(category_code,category_name,intime,gracetime,latetime,outtime,lunch_st_time,lunch_end_time,college_code,nooflate,permission_time,noofper,extend_gracetime,morn_late,morn_per,manual_Settings,Shift,Other_Per,EntryType,StfType,DayType,ApplyLeave,MorningOutTime,IsUnRegLeave,UnRegLeave,IsCompWork,CompWorkType,CompWorkDate,CompWorkDay,IsCalPayDate,ShiftType) values('" + catcode + "','" + staffcategory + "','" + getIntime + "','" + getgracetime + "','" + getLatetime + "','" + outtime + "','" + lunchstartT + "','" + lunchEndT + "','" + Session["collegecode"] + "','" + txtnooflate.Text + "','" + getperTime + "','" + txtnoofper.Text + "','" + getextend_gracetime + "','" + latevar + "','" + pervar + "','" + manualsettings + "','" + ddlshift.SelectedItem.Text + "','" + val + "','" + IntEntryType + "','" + stafftype + "','" + IntDayType + "','" + ddlleavecat.SelectedItem.Text + "','" + getmrngouttime + "','" + checkunregistred + "','" + unregisteredleave + "','" + checkcompulsarywork + "','" + selecteddateorday + "','" + comdatetime + "','" + compulsaryworkday + "','" + checkcalpayprocessdate + "','" + Nightshift + "')";

            chkupd = d2.update_method_wo_parameter(qry, "text");
        }
        else if (rdbselecteddate.Checked == true)
        {
            IntDayType = 1;
            qry = "insert into in_out_time(category_code,category_name,intime,gracetime,latetime,outtime,lunch_st_time,lunch_end_time,college_code,nooflate,permission_time,noofper,extend_gracetime,morn_late,morn_per,manual_Settings,Shift,Other_Per,EntryType,StfType,DayType,Bell_Date,ApplyLeave,MorningOutTime,IsUnRegLeave,UnRegLeave,IsCompWork,CompWorkType,CompWorkDate,CompWorkDay,IsCalPayDate,ShiftType) values('" + catcode + "','" + staffcategory + "','" + getIntime + "','" + getgracetime + "','" + getLatetime + "','" + outtime + "','" + lunchstartT + "','" + lunchEndT + "','" + Session["collegecode"] + "','" + txtnooflate.Text + "','" + getperTime + "','" + txtnoofper.Text + "','" + getextend_gracetime + "','" + latevar + "','" + pervar + "','" + manualsettings + "','" + ddlshift.SelectedItem.Text + "','" + val + "','" + IntEntryType + "','" + stafftype + "','" + IntDayType + "','" + comdatetime + "','" + ddlleavecat.SelectedItem.Text + "','" + getmrngouttime + "','" + checkunregistred + "','" + unregisteredleave + "','" + checkcompulsarywork + "','" + selecteddateorday + "','" + comdatetime + "','" + compulsaryworkday + "','" + checkcalpayprocessdate + "','" + Nightshift + "')";
            chkupd = d2.update_method_wo_parameter(qry, "text");

        }
        else if (rdbseelctedday.Checked == true)
        {
            IntDayType = 2;
            qry = "insert into in_out_time(category_code,category_name,intime,gracetime,latetime,outtime,lunch_st_time,lunch_end_time,college_code,nooflate,permission_time,noofper,extend_gracetime,morn_late,morn_per,manual_Settings,Shift,Other_Per,EntryType,StfType,DayType,Bell_Day,ApplyLeave,MorningOutTime,IsUnRegLeave,UnRegLeave,IsCompWork,CompWorkType,CompWorkDate,CompWorkDay,IsCalPayDate,ShiftType) values('" + catcode + "','" + staffcategory + "','" + getIntime + "','" + getgracetime + "','" + getLatetime + "','" + outtime + "','" + lunchstartT + "','" + lunchEndT + "','" + Session["collegecode"] + "','" + txtnooflate.Text + "','" + getperTime + "','" + txtnoofper.Text + "','" + getextend_gracetime + "','" + latevar + "','" + pervar + "','" + manualsettings + "','" + ddlshift.SelectedItem.Text + "','" + val + "','" + IntEntryType + "','" + stafftype + "','" + IntDayType + "','" + ddlsingleday.SelectedItem.Text + "','" + ddlleavecat.SelectedItem.Text + "','" + getmrngouttime + "','" + checkunregistred + "','" + unregisteredleave + "','" + checkcompulsarywork + "','" + selecteddateorday + "','" + comdatetime + "','" + compulsaryworkday + "','" + checkcalpayprocessdate + "','" + Nightshift + "')";
            chkupd = d2.update_method_wo_parameter(qry, "text");
        }
        if (chkupd > 0)
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Bell Timing is Added Successfully\");", true);

        }





        //If Opt_BellType(0).value = True Then
        //    If Cbo_Category.Text = "" Then
        //        MessageBox "Please Choose a category"
        //        Exit Sub
        //    End If
        //Else
        //    If Cbo_StaffType.Text = "" Then
        //        MessageBox "Please Choose a Staff Type"
        //        Exit Sub
        //    End If
        //End If
        //If Cbo_Category.Text = "Others" Then
        //    If txt_other.Text = "" Then
        //        MessageBox "Please enter a category name :"
        //        If txt_other.Visible = True And txt_other.Enabled = True Then txt_other.SetFocus
        //        Exit Sub
        //    End If
        //End If
        //If Trim(Cbo_Reason.Text) = "" Then
        //    MessageBox "Select Shift"
        //    Cbo_Reason.SetFocus
        //    Exit Sub
        //End If
        //If Chk_LeaveUnReg = 1 And Trim(Cbo_LeaveUnReg.Text) = "" Then
        //    MessageBox "Select Leave Type"
        //    If Cbo_LeaveUnReg.Enabled Then Cbo_LeaveUnReg.SetFocus
        //    Exit Sub
        //End If
        //If Chk_CompWork.value = 1 Then
        //    If Opt_CompWork(0).value = False And Opt_CompWork(1).value = False Then
        //        MessageBox "Select Compulsory working type"
        //        Exit Sub
        //    Else
        //        If Opt_CompWork(0).value = True And DTP_Calandar.ValueIsNull = True Then
        //            MessageBox "Select Compulsory working date"
        //            DTP_Calandar.SetFocus
        //            Exit Sub
        //        End If
        //        If Opt_CompWork(1).value = True And Cbo_CompWorkDay.Text = "" Then
        //            MessageBox "Select Compulsory working day"
        //            If Cbo_CompWorkDay.Enabled Then Cbo_CompWorkDay.SetFocus
        //            Exit Sub
        //        End If
        //    End If
        //End If

        //If chk_manual.value = 1 Then
        //    Dim latevar, pervar
        //    Dim manualsettings As Integer
        //    manualsettings = 0
        //    latevar = ""
        //    pervar = ""
        //    If late_from.ListIndex = -1 Or late_to.ListIndex = -1 Then
        //        MessageBox "Please Select Proper Morning Late Time"
        //        Exit Sub
        //    ElseIf per_from.ListIndex = -1 Or per_to.ListIndex = -1 Then
        //        MessageBox "Please Select Proper Morning Permission Time"
        //        Exit Sub
        //    End If
        //    latevar = late_from.Text & "-" & late_to.Text
        //    pervar = per_from.Text & "-" & per_to.Text
        //    manualsettings = 1
        //End If
        //If txtlate.Text = "" Then
        //    txtlate.Text = "0"
        //End If
        //If txtper.Text = "" Then
        //    txtper.Text = "0"
        //End If
        //If Trim(Cbo_ApplyLeave.Text) = "" Then
        //    Cbo_ApplyLeave.Text = "A"
        //End If

        //If Opt_BellType(0).value = True Then
        //    IntEntryType = 0
        //    strcatcode = StrStaffCat(Cbo_Category.ListIndex)
        //    StrCatName = Cbo_Category.Text
        //    StrStaffType = ""
        //Else
        //    IntEntryType = 1
        //    strcatcode = ""
        //    StrCatName = ""
        //    StrStaffType = Cbo_StaffType.Text
        //End If
        //If Opt_Day(0).value = True Then
        //    IntDayType = 0
        //ElseIf Opt_Day(1).value = True Then
        //    IntDayType = 1
        //ElseIf Opt_Day(2).value = True Then
        //    IntDayType = 2
        //End If
        //  ======================================  
        //sql = "SELECT * FROM In_Out_Time WHERE College_Code =" & genForAcad.collegecode
        //sql = sql & vbCrLf & " AND Shift ='" & Cbo_Reason.Text & "'"
        //sql = sql & vbCrLf & " AND DayType =" & IntDayType
        //If Opt_BellType(0).value = True Then
        //    sql = sql & vbCrLf & " AND Category_Name ='" & StrCatName & "'"
        //Else
        //    sql = sql & vbCrLf & " AND StfType ='" & StrStaffType & "'"
        //End If
        //If rs.state Then rs.Close
        //rs.Open sql, db
        //If Not rs.EOF Then
        //    InfoMsg "Bell Timing is already Added"
        //    Exit Sub
        //End If

        //If Spread_PerLOPDet.MaxRows > 0 Then  not included in code
        //    For i = 1 To Spread_PerLOPDet.MaxRows
        //        For j = 1 To Spread_PerLOPDet.MaxCols
        //            BlnIsVal = False
        //            Spread_PerLOPDet.GetText 2, i, varval
        //            If Trim(varval) <> "" Then
        //                BlnIsVal = True
        //                Spread_PerLOPDet.GetText j, i, varval
        //                StrPerLOPDet = StrPerLOPDet & val(varval) & ";"
        //            End If
        //        Next j
        //        If BlnIsVal = True Then
        //            StrPerLOPDet = StrPerLOPDet & "\"
        //        End If
        //    Next i
        //End If

        //==============================
        //If Opt_Day(0).value = True Then
        //    IntDayType = 0
        //    sql = "insert into in_out_time(category_code,category_name,intime,gracetime,latetime,outtime,lunch_st_time,lunch_end_time,college_code,nooflate,permission_time,noofper,extend_gracetime,morn_late,morn_per,manual_Settings,Shift,Other_Per,EntryType,StfType,DayType,ApplyLeave,MorningOutTime,IsUnRegLeave,UnRegLeave,IsCompWork,CompWorkType,CompWorkDate,CompWorkDay,IsCalPayDate) "
        //    sql = sql & vbCrLf & " values('" & strcatcode & "','" & StrCatName & "','" & Format(DTPicker1.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker2.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker3.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker4.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker5.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker6.value, "hh:mm:ss AM/PM") & "'," & genForAcad.collegecode & "," & txtlate.Text & ",'" & Format(DTPicker7.value, "hh:mm:ss AM/PM") & "'," & txtper.Text & ",'" & Format(DTPicker8.value, "hh:mm:ss AM/PM") & "','" & latevar & "','" & pervar & "'," & manualsettings & ",'" & Cbo_Reason.Text & "'," & IIf(Chk_OtherPermision.value, 1, 0) & "," & IntEntryType & ",'" & StrStaffType & "'," & IntDayType & ",'" & Cbo_ApplyLeave.Text & "','" & Format(DTP_MorningOutTime.value, "hh:mm:ss AM/PM") & "',"
        //    sql = sql & vbCrLf & IIf((Chk_LeaveUnReg.value = 1), 1, 0) & ",'" & IIf((Chk_LeaveUnReg.value = 1), Cbo_LeaveUnReg.Text, "") & "'," & IIf((Chk_CompWork.value = 1), 1, 0) & "," & IIf((Opt_CompWork(0).value = True), 0, 1) & ",'" & IIf((Opt_CompWork(0).value = True), DTP_Calandar.value, "") & "','" & IIf((Opt_CompWork(1).value = True), Cbo_CompWorkDay.Text, "") & "'," & IIf((Chk_IsCalPayDate.value = 1), 1, 0) & ")"
        //    db.Execute sql
        //ElseIf Opt_Day(1).value = True Then
        //    IntDayType = 1
        //    sql = "insert into in_out_time(category_code,category_name,intime,gracetime,latetime,outtime,lunch_st_time,lunch_end_time,college_code,nooflate,permission_time,noofper,extend_gracetime,morn_late,morn_per,manual_Settings,Shift,Other_Per,EntryType,StfType,DayType,Bell_Date,ApplyLeave,MorningOutTime,IsUnRegLeave,UnRegLeave,IsCompWork,CompWorkType,CompWorkDate,CompWorkDay,IsCalPayDate) "
        //    sql = sql & vbCrLf & " values('" & strcatcode & "','" & StrCatName & "','" & Format(DTPicker1.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker2.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker3.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker4.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker5.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker6.value, "hh:mm:ss AM/PM") & "'," & genForAcad.collegecode & "," & txtlate.Text & ",'" & Format(DTPicker7.value, "hh:mm:ss AM/PM") & "'," & txtper.Text & ",'" & Format(DTPicker8.value, "hh:mm:ss AM/PM") & "','" & latevar & "','" & pervar & "'," & manualsettings & ",'" & Cbo_Reason.Text & "'," & IIf(Chk_OtherPermision.value, 1, 0) & "," & IntEntryType & ",'" & StrStaffType & "'," & IntDayType & ",'" & DTP_Calandar.value & "','" & Cbo_ApplyLeave.Text & "','" & Format(DTP_MorningOutTime.value, "hh:mm:ss AM/PM") & "',"
        //    sql = sql & vbCrLf & IIf((Chk_LeaveUnReg.value = 1), 1, 0) & ",'" & IIf((Chk_LeaveUnReg.value = 1), Cbo_LeaveUnReg.Text, "") & "'," & IIf((Chk_CompWork.value = 1), 1, 0) & "," & IIf((Opt_CompWork(0).value = True), 0, 1) & ",'" & IIf((Opt_CompWork(0).value = True), DTP_Calandar.value, "") & "','" & IIf((Opt_CompWork(1).value = True), Cbo_CompWorkDay.Text, "") & "'," & IIf((Chk_IsCalPayDate.value = 1), 1, 0) & ")"
        //    db.Execute sql
        //ElseIf Opt_Day(2).value = True Then
        //    IntDayType = 2
        //    sql = "insert into in_out_time(category_code,category_name,intime,gracetime,latetime,outtime,lunch_st_time,lunch_end_time,college_code,nooflate,permission_time,noofper,extend_gracetime,morn_late,morn_per,manual_Settings,Shift,Other_Per,EntryType,StfType,DayType,Bell_Day,ApplyLeave,MorningOutTime,IsUnRegLeave,UnRegLeave,IsCompWork,CompWorkType,CompWorkDate,CompWorkDay,IsCalPayDate) "
        //    sql = sql & vbCrLf & " values('" & strcatcode & "','" & StrCatName & "','" & Format(DTPicker1.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker2.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker3.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker4.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker5.value, "hh:mm:ss AM/PM") & "','" & Format(DTPicker6.value, "hh:mm:ss AM/PM") & "'," & genForAcad.collegecode & "," & txtlate.Text & ",'" & Format(DTPicker7.value, "hh:mm:ss AM/PM") & "'," & txtper.Text & ",'" & Format(DTPicker8.value, "hh:mm:ss AM/PM") & "','" & latevar & "','" & pervar & "'," & manualsettings & ",'" & Cbo_Reason.Text & "'," & IIf(Chk_OtherPermision.value, 1, 0) & "," & IntEntryType & ",'" & StrStaffType & "'," & IntDayType & ",'" & Cbo_Day.Text & "','" & Cbo_ApplyLeave.Text & "','" & Format(DTP_MorningOutTime.value, "hh:mm:ss AM/PM") & "',"
        //    sql = sql & vbCrLf & IIf((Chk_LeaveUnReg.value = 1), 1, 0) & ",'" & IIf((Chk_LeaveUnReg.value = 1), Cbo_LeaveUnReg.Text, "") & "'," & IIf((Chk_CompWork.value = 1), 1, 0) & "," & IIf((Opt_CompWork(0).value = True), 0, 1) & ",'" & IIf((Opt_CompWork(0).value = True), DTP_Calandar.value, "") & "','" & IIf((Opt_CompWork(1).value = True), Cbo_CompWorkDay.Text, "") & "'," & IIf((Chk_IsCalPayDate.value = 1), 1, 0) & ")"
        //    db.Execute sql
        //End If

        //If Spread_PerLOPDet.MaxRows > 0 Then
        //    If Opt_BellType(0).value = True Then
        //        sql = "DELETE FROM StaffLateSettings WHERE category_code ='" & strcatcode & "' "
        //        db.Execute sql
        //        For i = 1 To Spread_PerLOPDet.MaxRows
        //            Spread_PerLOPDet.GetText 1, i, VarFromLA
        //            Spread_PerLOPDet.GetText 2, i, VarToLA
        //            Spread_PerLOPDet.GetText 3, i, VarLeaveType
        //            Spread_PerLOPDet.GetText 4, i, VarLeaveDays
        //            Spread_PerLOPDet.GetText 5, i, VarFinishLeave

        //            sql = "INSERT INTO StaffLateSettings(SettingType,Category_Code,FromLA,ToLA,ShortName,LeaveDays,AfterFinishLeave,CollegeCode) "
        //            sql = sql & vbCrLf & " VALUES(0,'" & strcatcode & "'," & VarFromLA & "," & VarToLA & ",'" & VarLeaveType & "'," & VarLeaveDays & ",'" & VarFinishLeave & "'," & genForAcad.collegecode & ")"
        //            db.Execute sql
        //        Next i
        //    Else
        //        sql = "DELETE FROM StaffLateSettings WHERE stftype ='" & StrStaffType & "' "
        //        db.Execute sql
        //        For i = 1 To Spread_PerLOPDet.MaxRows
        //            Spread_PerLOPDet.GetText 1, i, VarFromLA
        //            Spread_PerLOPDet.GetText 2, i, VarToLA
        //            Spread_PerLOPDet.GetText 3, i, VarLeaveType
        //            Spread_PerLOPDet.GetText 4, i, VarLeaveDays
        //            Spread_PerLOPDet.GetText 5, i, VarFinishLeave

        //            sql = "INSERT INTO StaffLateSettings(SettingType,Category_Code,FromLA,ToLA,ShortName,LeaveDays,AfterFinishLeave,CollegeCode) "
        //            sql = sql & vbCrLf & " VALUES(0,'" & StrStaffType & "'," & VarFromLA & "," & VarToLA & ",'" & VarLeaveType & "'," & VarLeaveDays & ",'" & VarFinishLeave & "'," & genForAcad.collegecode & ")"
        //            db.Execute sql
        //        Next i
        //    End If
        //End If

        //MessageBox "Bell Timing Added Sucessfully"
        //Call gen_admin_bell_info_New.Option1_Click

        //strupdateinfo = "Time Category Master"
        //ContNam = "Save the Time Category Information"
        //InsertUserAction Me.name, UserAct.FSave, MStudent, ContNam, strupdateinfo

        //'<EhFooter>
        //    Exit Sub
        //cmd_Save_Click_Err:
        //    genForAcad.ErrHandler "InsProPlus", "frmstaffcatmasterNew", "cmd_Save_Click", Erl, err.Number, err.Description
        //'</EhFooter>
        //End Sub

    }
    protected void btnexit_click(object sender, EventArgs e)//delsijus
    {
        popper1.Visible = false;
    }
    protected void btn_shift_OnClick(object sender, EventArgs e)
    {
        Plusapt.Visible = true;
        btn_plusAdd.Visible = true;
        txt_addstream.Text = "";
        headerapt.Text = "Shift";
    }
    protected void btn_shiftminus_OnClick(object sender, EventArgs e)
    {
        string Stream = Convert.ToString(ddlshift.SelectedItem);
        string query = "delete from TextValTable where TextVal='" + Stream + "' and college_code='" + ddlcol.SelectedItem.Value + "'";
        int count = d2.update_method_wo_parameter(query, "Text");
        bindshift();
    }

    protected void btn_plusAdd_OnClick(object sender, EventArgs e)
    {
        try
        {
            string stream = txt_addstream.Text;
            string criteria = "biost";
            string collcode = Convert.ToString(ddlcol.SelectedItem.Value);
            if (stream.Trim() != "")
            {
                string query = "insert into TextValTable(TextVal,TextCriteria,college_code )values ('" + stream + "','" + criteria + "','" + collcode + "')";
                int count = d2.update_method_wo_parameter(query, "Text");
                if (count > 0)
                {
                    bindshift();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                }


            }
        }
        catch { }
    }
    protected void btn_Plusexit_OnClick(object sender, EventArgs e)
    {
        Plusapt.Visible = false;
    }
    protected void bindshift()
    {
        ds.Clear();
        ddlshift.Items.Clear();

        string item = "select distinct TextVal,TextCode from TextValTable where TextCriteria='biost' ";
        ds = d2.select_method_wo_parameter(item, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlshift.DataSource = ds;
            ddlshift.DataTextField = "TextVal";
            ddlshift.DataValueField = "TextCode";
            ddlshift.DataBind();
            ddlshift.Items.Insert(0, "Select");
        }
        else
        {
            ddlshift.Items.Insert(0, "Select");
        }
    }

    protected void cbmanuelsetting_checkedchange(object sender, EventArgs e)
    {
        if (cbmanuelsetting.Checked == true)
        {
            ddl1.Enabled = true;
            ddl2.Enabled = true;
            ddl3.Enabled = true;
            ddl4.Enabled = true;
        }
        if (cbmanuelsetting.Checked == false)
        {
            ddl1.Enabled = false;
            ddl2.Enabled = false;
            ddl3.Enabled = false;
            ddl4.Enabled = false;
        }
    }
    protected void cbunregistered_checkedchange(object sender, EventArgs e)
    {
        if (cbunregisteredstaff.Checked == true)
        {
            ddlunregistered.Enabled = true;
            bindleav();

        }
        if (cbunregisteredstaff.Checked == false)
        {
            ddlunregistered.Enabled = false;
            ddlunregistered.Items.Clear();
        }

    }
    protected void bindyear()
    {
        ddlyear.Items.Clear();
        ddlstubatchyear.Items.Clear();
        ds = d2.BindBatch();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlyear.DataSource = ds;
            ddlyear.DataTextField = "batch_year";
            ddlyear.DataValueField = "batch_year";
            ddlyear.DataBind();
            ddlstubatchyear.DataSource = ds;
            ddlstubatchyear.DataTextField = "batch_year";
            ddlstubatchyear.DataValueField = "batch_year";
            ddlstubatchyear.DataBind();

        }
        //for (int j = DateTime.Now.Year; j >= 1900; j--)
        //{
        //    ddlyear.Items.Add(j.ToString());
        //    ddlstubatchyear.Items.Add(j.ToString());
        //}

    }
    protected void bindleav()
    {

        ddlunregistered.Items.Clear();
        ddlleavecat.Items.Clear();
        ds.Clear();
        string q1 = "Select shortname,LeaveMasterPK from leave_category where college_code='" + ddlcol.SelectedItem.Value + "'";
        ds = d2.select_method_wo_parameter(q1, "text");

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (cbunregisteredstaff.Checked == true)
            {
                ddlunregistered.DataSource = ds.Tables[0];
                ddlunregistered.DataTextField = "shortname";
                ddlunregistered.DataValueField = "LeaveMasterPK";
                ddlunregistered.DataBind();
            }
            ddlleavecat.DataSource = ds.Tables[0];
            ddlleavecat.DataTextField = "shortname";
            ddlleavecat.DataValueField = "LeaveMasterPK";
            ddlleavecat.DataBind();
            ddlleavecat.Items.Insert(0, "A");
        }
    }
    protected void cbcompworking_checkedchange(object sender, EventArgs e)
    {
        if (cbcompworking.Checked == true)
        {
            rdbselecteddatecomp.Enabled = true;
            rdbselecteddaycomp.Enabled = true;
            ddlcompworking.Enabled = false;
        }
        else
        {
            rdbselecteddatecomp.Enabled = false;
            rdbselecteddaycomp.Enabled = false;
            rdbselecteddatecomp.Checked = false;
            rdbselecteddaycomp.Checked = false;
            ddlcompworking.Enabled = false;
        }

    }
    protected void rdbstudent_changed(object sender, EventArgs e)
    {
        if (rdbstudent.Checked == true)
        {
            rdbstaff.Checked = false;
            rdbhostel.Checked = false;
            ddlstafcatmain.Visible = false;
            ddlhostel.Visible = false;


            lblstubatchyear.Visible = true;
            ddlstubatchyear.Visible = true;
            lblstudegreecode.Visible = true;
            ddlstudegreecode.Visible = true;
            lblstusem.Visible = true;
            ddlstusem.Visible = true;

        }

    }
    protected void rdbstaff_changed(object sender, EventArgs e)
    {
        if (rdbstaff.Checked == true)
        {
            rdbstudent.Checked = false;
            rdbhostel.Checked = false;
            bindstaffcatType();
            ddlstafcatmain.Visible = true;
            ddlhostel.Visible = false;

            lblstubatchyear.Visible = false;
            ddlstubatchyear.Visible = false;
            lblstudegreecode.Visible = false;
            ddlstudegreecode.Visible = false;
            lblstusem.Visible = false;
            ddlstusem.Visible = false;
        }
        else
        {
            ddlstafcatmain.Visible = false;

        }

    }
    protected void rdbhostel_changed(object sender, EventArgs e)
    {
        if (rdbhostel.Checked == true)
        {
            rdbstudent.Checked = false;
            rdbstaff.Checked = false;
            ddlhostel.Visible = true;
            ddlstafcatmain.Visible = false;
            lblstubatchyear.Visible = false;
            ddlstubatchyear.Visible = false;
            lblstudegreecode.Visible = false;
            ddlstudegreecode.Visible = false;
            lblstusem.Visible = false;
            ddlstusem.Visible = false;

        }
        else
        {

            ddlhostel.Visible = false;


        }

    }
    protected void SelectedIndexChanged(Object sender, EventArgs e)//delsijuu
    {
        try
        {
            btnupdate.Visible = true;
            btnsave.Visible = false;
            btndel.Visible = true;
            bindshift();
            bindstaffcatType();
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string stafftypecat = grdstaff.Rows[rowIndex].Cells[1].Text;
            string shift = grdstaff.Rows[rowIndex].Cells[2].Text;
            string intimes = grdstaff.Rows[rowIndex].Cells[3].Text;
            string gracetimes = grdstaff.Rows[rowIndex].Cells[4].Text;
            string latetimes = grdstaff.Rows[rowIndex].Cells[5].Text;
            string outtimes = grdstaff.Rows[rowIndex].Cells[6].Text;
            string lunchstarts = grdstaff.Rows[rowIndex].Cells[7].Text;
            string lunchends = grdstaff.Rows[rowIndex].Cells[8].Text;
            string nooflateallowed = grdstaff.Rows[rowIndex].Cells[9].Text;

            string perstart = grdstaff.Rows[rowIndex].Cells[10].Text;
            string extndgrcestarttime = grdstaff.Rows[rowIndex].Cells[11].Text;
            string mrngoutTime = grdstaff.Rows[rowIndex].Cells[12].Text;


            string totalnoper = grdstaff.Rows[rowIndex].Cells[13].Text;
            string morlatetext = grdstaff.Rows[rowIndex].Cells[14].Text;
            string morpertext = grdstaff.Rows[rowIndex].Cells[15].Text;
            string ismannsetting = grdstaff.Rows[rowIndex].Cells[16].Text;
            string isunreggflagcheck = grdstaff.Rows[rowIndex].Cells[17].Text;
            string unreggglagg = grdstaff.Rows[rowIndex].Cells[18].Text;
            popper1.Visible = true;
            ddlstaffcategorytype.SelectedItem.Text = stafftypecat;
            string catcodes = d2.GetFunction("select category_code from staffcategorizer where category_name='" + stafftypecat + "' and college_code='" + Session["collegecode"] + "'");
            ddlstaffcategorytype.SelectedItem.Value = catcodes;
            ddlshift.SelectedItem.Text = shift;
            //Added By Saranyadevi7.3.2019
            string ShiftType = d2.GetFunction("select ShiftType from in_out_time where category_code='" + catcodes + "' and Shift='" + shift + "' and category_name='" + stafftypecat + "' and college_code='" + Session["collegecode"] + "'");
            if (ShiftType == "1")
            {
                chk_NightShift.Checked = true;
            }
            else {
                chk_NightShift.Checked = false;
            }

            DateTime getintime = new DateTime();
            DateTime getgracetime = new DateTime();
            DateTime getlatetimes = new DateTime();
            DateTime getouttimes = new DateTime();
            DateTime getlunchstarts = new DateTime();
            DateTime getlunchend = new DateTime();
            DateTime getper = new DateTime();
            DateTime getexnd = new DateTime();
            DateTime getmrngOut = new DateTime();

            getintime = Convert.ToDateTime(intimes);
            getgracetime = Convert.ToDateTime(gracetimes);
            getlatetimes = Convert.ToDateTime(latetimes);
            getouttimes = Convert.ToDateTime(outtimes);
            getlunchstarts = Convert.ToDateTime(lunchstarts);
            getlunchend = Convert.ToDateTime(lunchends);
            getper = Convert.ToDateTime(perstart);
            getexnd = Convert.ToDateTime(extndgrcestarttime);
            getmrngOut = Convert.ToDateTime(mrngoutTime);
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm1;
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm2;
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm3;
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm4;
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm5;
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm6;
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm7;
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm8;
            if (getintime.ToString("tt") == "AM")
            {
                am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            if (getgracetime.ToString("tt") == "AM")
            {
                am_pm1 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm1 = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            if (getlatetimes.ToString("tt") == "AM")
            {
                am_pm2 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm2 = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            if (getouttimes.ToString("tt") == "AM")
            {
                am_pm3 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm3 = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            if (getlunchstarts.ToString("tt") == "AM")
            {
                am_pm4 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm4 = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }

            if (getlunchend.ToString("tt") == "AM")
            {
                am_pm5 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm5 = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            if (getper.ToString("tt") == "AM")
            {
                am_pm6 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm6 = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            if (getexnd.ToString("tt") == "AM")
            {
                am_pm7 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm7 = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }

            if (getmrngOut.ToString("tt") == "AM")
            {
                am_pm8 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm8 = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            intime.SetTime(getintime.Hour, getintime.Minute, getintime.Second, am_pm);
            gracetime.SetTime(getgracetime.Hour, getgracetime.Minute, getgracetime.Second, am_pm1);
            latetime.SetTime(getlatetimes.Hour, getlatetimes.Minute, getlatetimes.Second, am_pm2);
            outtimebell.SetTime(getouttimes.Hour, getouttimes.Minute, getouttimes.Second, am_pm3);
            starttime.SetTime(getlunchstarts.Hour, getlunchstarts.Minute, getlunchstarts.Second, am_pm4);
            endtime.SetTime(getlunchend.Hour, getlunchend.Minute, getlunchend.Second, am_pm5);
            txtnooflate.Text = nooflateallowed;
            permission.SetTime(getper.Hour, getper.Minute, getper.Second, am_pm6);
            exgracetime.SetTime(getexnd.Hour, getexnd.Minute, getexnd.Second, am_pm7);
            outtimebell.SetTime(getmrngOut.Hour, getmrngOut.Minute, getmrngOut.Second, am_pm8);
            txtnoofper.Text = totalnoper;
            if (ismannsetting == "1")
            {
                cbmanuelsetting.Checked = true;
                ddl1.Enabled = true;
                ddl2.Enabled = true;
                ddl3.Enabled = true;
                ddl4.Enabled = true;
                if (morlatetext.Contains('-'))
                {
                    string[] splitmornlate = morlatetext.Split('-');
                    ddl1.SelectedItem.Text = splitmornlate[0];
                    ddl2.SelectedItem.Text = splitmornlate[1];

                }
                if (morpertext.Contains('-'))
                {
                    string[] splitmornper = morpertext.Split('-');
                    ddl3.SelectedItem.Text = splitmornper[0];
                    ddl4.SelectedItem.Text = splitmornper[1];

                }
                if (isunreggflagcheck == "1")
                {
                    cbunregisteredstaff.Checked = true;
                    bindleav();
                    ddlunregistered.SelectedItem.Text = unreggglagg;

                }
                else
                {
                    cbunregisteredstaff.Checked = false;
                }


            }
            else
            {
                cbmanuelsetting.Checked = false;
                ddl1.Enabled = false;
                ddl2.Enabled = false;
                ddl3.Enabled = false;
                ddl4.Enabled = false;
            
            }

        }
        catch (Exception ex)
        {


        }
    }
    protected void SelectedIndexChangedgrdhost(Object sender, EventArgs e)//delsijuu
    {
        try
        {
            btn_hostelupdate.Visible = true;
            btn_hostelsave.Visible = false;
            bindhostel();

            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string hostelname = grdhost.Rows[rowIndex].Cells[1].Text;
            string getouttime = grdhost.Rows[rowIndex].Cells[2].Text;
            string getmrngLatetime = grdhost.Rows[rowIndex].Cells[3].Text;
            string getIntime = grdhost.Rows[rowIndex].Cells[4].Text;
            string getlatetimes = grdhost.Rows[rowIndex].Cells[5].Text;
            string gettotlateallowed = grdhost.Rows[rowIndex].Cells[6].Text;

            DateTime hosouTtime = new DateTime();
            DateTime hosmrngLtime = new DateTime();
            DateTime hosIntime = new DateTime();
            DateTime hoslate = new DateTime();

            hosouTtime = Convert.ToDateTime(getouttime);
            hosmrngLtime = Convert.ToDateTime(getmrngLatetime);
            hosIntime = Convert.ToDateTime(getIntime);
            hoslate = Convert.ToDateTime(getlatetimes);
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm;

            MKB.TimePicker.TimeSelector.AmPmSpec am_pm1;
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm2;
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm3;
            if (hosouTtime.ToString("tt") == "AM")
            {
                am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            if (hosmrngLtime.ToString("tt") == "AM")
            {
                am_pm1 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm1 = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            if (hosIntime.ToString("tt") == "AM")
            {

                am_pm2 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm2 = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            if (hoslate.ToString("tt") == "AM")
            {
                am_pm3 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm3 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            hosteltoouttime.SetTime(hosouTtime.Hour, hosouTtime.Minute, hosouTtime.Second, am_pm);
            hostelmrnglatetime.SetTime(hosmrngLtime.Hour, hosmrngLtime.Minute, hosmrngLtime.Second, am_pm1);
            hosteltointime.SetTime(hosIntime.Hour, hosIntime.Minute, hosIntime.Second, am_pm2);
            hosteleveninglate.SetTime(hoslate.Hour, hoslate.Minute, hoslate.Second, am_pm3);
            txthostellateallowed.Text = gettotlateallowed;
            poppe2.Visible = true;

        }
        catch (Exception ex)
        { }
    }

    protected void SelectedIndexChangedgrdstud(Object sender, EventArgs e)//delsijuu
    {
        try
        {
            btnstudupdate.Visible = true;
            bthndeletestud.Visible = true;
            btnstudsave.Visible = false;
            bindstudyear();
            bindcoursestud();
            bindsem();
            bindperiod();
            var grid = (GridView)sender;
            string sem = string.Empty;
            string batchyr = string.Empty;
            string dcode = string.Empty;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string period = grdstud.Rows[rowIndex].Cells[1].Text;
            string starttime = grdstud.Rows[rowIndex].Cells[2].Text;
            string endtime = grdstud.Rows[rowIndex].Cells[3].Text;
            string descp = grdstud.Rows[rowIndex].Cells[4].Text;
            string getsembatchdeg = grdstud.Rows[rowIndex].Cells[5].Text;
            if (getsembatchdeg.Contains('-'))
            {
                string[] splitval = getsembatchdeg.Split('-');
                if (splitval.Length > 0)
                {
                    sem = Convert.ToString(splitval[0]);
                    batchyr = Convert.ToString(splitval[1]);
                    dcode = Convert.ToString(splitval[2]);
                }

            }
            if (descp == "&nbsp;")
            {
                descp = "";
            }

            DateTime stustarttimes = new DateTime();
            DateTime stuendtimes = new DateTime();
            stustarttimes = Convert.ToDateTime(starttime);
            stuendtimes = Convert.ToDateTime(endtime);
            MKB.TimePicker.TimeSelector.AmPmSpec am_pm;

            MKB.TimePicker.TimeSelector.AmPmSpec am_pm1;
            if (stustarttimes.ToString("tt") == "AM")
            {
                am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            if (stuendtimes.ToString("tt") == "AM")
            {
                am_pm1 = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
            }
            else
            {
                am_pm1 = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
            }
            stustarttime.SetTime(stustarttimes.Hour, stustarttimes.Minute, stustarttimes.Second, am_pm);
            stuendtime.SetTime(stuendtimes.Hour, stuendtimes.Minute, stuendtimes.Second, am_pm1);
            string getdegree = Convert.ToString(ddlstudegreecode.SelectedItem.Text);
            string getdeg = string.Empty;
            if (getdegree.Contains('-'))
            {
                string[] splitvals = getdegree.Split('-');
                getdeg = Convert.ToString(splitvals[0]);
            }
            for (int va = 0; va < cbl_degree.Items.Count; va++)
            {
                if (cbl_degree.Items[va].Selected == true)
                {
                    string getdegval = Convert.ToString(cbl_degree.Items[va].Text);
                    if (getdegval.Trim().ToString() == getdeg.Trim().ToString())
                    {
                        cbl_degree.Items[va].Selected = true;
                        cb_degree.Checked = false;
                        txt_degree.Text = "Degree(1)";

                    }
                }
            }
            for (int val = 0; val < cbl_batch.Items.Count; val++)
            {
                if (cbl_batch.Items[val].Selected == true)
                {
                    string getbtchyr = Convert.ToString(cbl_batch.Items[val].Value);
                    if (getbtchyr == batchyr)
                    {
                        cbl_batch.Items[val].Selected = true;
                        cb_batch.Checked = false;
                        txt_batch.Text = "BatchYear(1)";
                    }
                    else
                    {
                        cbl_batch.Items[val].Selected = false;

                    }
                }


            }
            for (int val = 0; val < cbl_dept.Items.Count; val++)
            {
                if (cbl_dept.Items[val].Selected == true)
                {
                    string getbtchyr = Convert.ToString(cbl_dept.Items[val].Value);
                    if (getbtchyr == dcode)
                    {
                        cbl_dept.Items[val].Selected = true;
                        cb_dept.Checked = false;
                        txt_dept.Text = "Degree(1)";
                    }
                    else
                    {
                        cbl_dept.Items[val].Selected = false;

                    }
                }


            }
            for (int val = 0; val < cbl_sem.Items.Count; val++)
            {
                if (cbl_sem.Items[val].Selected == true)
                {
                    string semval = Convert.ToString(cbl_sem.Items[val].Value);
                    if (semval == sem)
                    {
                        cbl_sem.Items[val].Selected = true;
                        cb_sem.Checked = false;
                        txt_sem.Text = "Sem(1)";
                    }
                    else
                    {
                        cbl_sem.Items[val].Selected = false;

                    }
                }


            }
            ddlperiod.SelectedItem.Text = period;
            // cbl_batch.SelectedItem.Value = batchyr;
            //cbl_dept.SelectedItem.Value = dcode;
            cbl_sem.SelectedItem.Value = sem;
            //ddlcourse.SelectedItem.Value = dcode;
            //ddlbatchyear.SelectedItem.Text = batchyr;
            // ddlsemyear.SelectedItem.Text = sem;
            txtstuddesc.Text = descp;
            poppe3.Visible = true;


        }
        catch (Exception ex)
        {
        }
    }
    protected void grdstaff_RowDataBound(object sende, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    e.Row.BackColor = Color.FromArgb(12, 166, 202);
                    e.Row.HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Width = 200;
                    e.Row.Font.Bold = true;
                }
                e.Row.Cells[0].Width = 50;
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                if (e.Row.RowIndex != 0)
                {
                }
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[16].Visible = false;
                e.Row.Cells[17].Visible = false;
                e.Row.Cells[18].Visible = false;
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void grdhost_RowDataBound(object sende, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    e.Row.BackColor = Color.FromArgb(12, 166, 202);
                    e.Row.HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Width = 200;
                    e.Row.Font.Bold = true;
                }
                e.Row.Cells[0].Width = 50;
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                if (e.Row.RowIndex != 0)
                {
                }
            }

        }
        catch (Exception ex)
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

    protected void OnRowCreatedgrdhost(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndexhos.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }
    protected void btnupdate_click(object sender, EventArgs e)
    {
        try
        {
            string stafftype = string.Empty;
            string staffcategory = string.Empty;
            DataSet ds = new DataSet();
            int IntEntryType = 0;
            int IntDayType = 0;
            string latevar = string.Empty;
            string pervar = string.Empty;
            int manualsettings = 0;
            if (rdbstaffcategory.Checked == true)
            {
                if (ddlstaffcategorytype.SelectedItem.Text == "")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Choose a category\");", true);
                    return;
                }

            }
            else
            {
                if (ddlstaffcategorytype.SelectedItem.Text == "")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Choose a Staff Type\");", true);
                    return;

                }
            }
            if (ddlshift.SelectedItem.Text.Trim() == "Select")
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Shift\");", true);
                return;
            }
            if (cbunregisteredstaff.Checked == true && ddlunregistered.Text == "")
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Shift\");", true);
                return;
            }
            if (cbcompworking.Checked == true)
            {
                if (rdbselecteddatecomp.Checked == false && rdbselecteddaycomp.Checked == false)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Compulsory working type\");", true);
                    return;
                }


            }


            if (cbmanuelsetting.Checked == true)
            {


                if (ddl1.SelectedItem.Text == "Select" && ddl2.SelectedItem.Text == "Select")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Proper Morning Late Time\");", true);
                    return;

                }
                else if (ddl3.SelectedItem.Text == "Select" && ddl4.SelectedItem.Text == "Select")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Proper Morning Permission Time\");", true);
                    return;

                }
                latevar = ddl1.SelectedItem.Text + "-" + ddl2.SelectedItem.Text;
                pervar = ddl3.SelectedItem.Text + "-" + ddl4.SelectedItem.Text;
                manualsettings = 1;

            }
            if (txtnooflate.Text == "")
            {
                txtnooflate.Text = "0";
            }
            if (txtnoofper.Text == "")
            {
                txtnoofper.Text = "0";

            }
            string catcode = string.Empty;
            if (rdbstaffcategory.Checked == true)
            {
                IntEntryType = 0;
                catcode = Convert.ToString(ddlstaffcategorytype.SelectedItem.Value);
                staffcategory = Convert.ToString(ddlstaffcategorytype.SelectedItem.Text);
                catcode = d2.GetFunction("select category_code from staffcategorizer where category_name='" + staffcategory + "' and college_code='" + Session["collegecode"] + "'");
                //load catcode
            }
            else
            {
                IntEntryType = 1;
                stafftype = Convert.ToString(ddlstaffcategorytype.SelectedItem.Text);

                //load stftype

            }
            if (rdballdays.Checked == true)
            {
                IntDayType = 0;
            }
            else if (rdbselecteddate.Checked == true)
            {
                IntDayType = 1;

            }
            else if (rdbseelctedday.Checked == true)
            {
                IntDayType = 2;
            }
            int val = 0;
            if (chkotherpermission.Checked == true)
            {
                val = 1;
            }
            int checkunregistred = 0;
            string unregisteredleave = string.Empty;
            if (cbunregisteredstaff.Checked == true)
            {
                checkunregistred = 1;
                unregisteredleave = Convert.ToString(ddlunregistered.SelectedItem.Text);
            }
            int checkcompulsarywork = 0;
            int selecteddateorday = 1;
            string compulsarydatetime = string.Empty;
            string compulsaryworkday = string.Empty;
            DateTime comdatetime = new DateTime();

            if (cbcompworking.Checked == true)
            {
                checkcompulsarywork = 1;
                if (rdbselecteddatecomp.Checked == true)
                {
                    selecteddateorday = 0;
                    compulsarydatetime = Txtentryto.Text;
                    if (compulsarydatetime.Contains('/'))
                    {
                        string[] splitdate = compulsarydatetime.Split('/');
                        compulsarydatetime = splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2];
                        comdatetime = Convert.ToDateTime(compulsarydatetime);

                    }


                }
                if (rdbselecteddaycomp.Checked == true)
                {
                    compulsaryworkday = ddlcompworking.SelectedItem.Value;

                }

            }
            else
            {
                compulsarydatetime = Txtentryto.Text;
                if (compulsarydatetime.Contains('/'))
                {
                    string[] splitdate = compulsarydatetime.Split('/');
                    compulsarydatetime = splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2];
                    comdatetime = Convert.ToDateTime(compulsarydatetime);

                }

            }
            int checkcalpayprocessdate = 0;
            if (cbpayprocess.Checked == true)
            {
                checkcalpayprocessdate = 1;
            }
            DateTime getIntime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", intime.Hour, intime.Minute, intime.Second, intime.AmPm));
            DateTime getgracetime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", gracetime.Hour, gracetime.Minute, gracetime.Second, gracetime.AmPm));
            DateTime getLatetime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", latetime.Hour, latetime.Minute, latetime.Second, latetime.AmPm));
            DateTime outtime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", eveningout.Hour, eveningout.Minute, eveningout.Second, eveningout.AmPm));
            DateTime lunchstartT = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", starttime.Hour, starttime.Minute, starttime.Second, starttime.AmPm));
            DateTime lunchEndT = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", endtime.Hour, endtime.Minute, endtime.Second, endtime.AmPm));
            DateTime getperTime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", permission.Hour, permission.Minute, permission.Second, permission.AmPm));
            DateTime getextend_gracetime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", exgracetime.Hour, exgracetime.Minute, exgracetime.Second, exgracetime.AmPm));
            DateTime getmrngouttime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", outtimebell.Hour, outtimebell.Minute, outtimebell.Second, outtimebell.AmPm));

            //Added By Saranyadevi 7.3.2019
            string Nightshift = "";
            if (chk_NightShift.Checked)
            {
                Nightshift = "1";
            }
            else
            {
                Nightshift = "0";
            }

            string qry = string.Empty;//catcode
            int chkupd = 0;
            if (rdballdays.Checked == true)
            {

                IntDayType = 0;
                qry = "update in_out_time set intime='" + getIntime + "',gracetime='" + getgracetime + "',latetime='" + getLatetime + "',outtime='" + outtime + "',lunch_st_time='" + lunchstartT + "',lunch_end_time='" + lunchEndT + "',nooflate='" + txtnooflate.Text + "',permission_time='" + getperTime + "',college_code='" + Session["collegecode"] + "',noofper='" + txtnoofper.Text + "' ,extend_gracetime='" + getextend_gracetime + "',morn_late='" + latevar + "' ,morn_per='" + pervar + "',manual_Settings='" + manualsettings + "',Shift ='" + ddlshift.SelectedItem.Text + "',Other_Per ='" + val + "',EntryType ='" + IntEntryType + "',StfType ='" + stafftype + "',DayType='" + IntDayType + "',ApplyLeave ='" + ddlsingleday.SelectedItem.Text + "',MorningOutTime ='" + getmrngouttime + "',IsUnRegLeave='" + checkunregistred + "',UnRegLeave='" + unregisteredleave + "',IsCompWork='" + checkcompulsarywork + "',CompWorkType='" + selecteddateorday + "',CompWorkDate='" + comdatetime + "',CompWorkDay='" + compulsaryworkday + "',IsCalPayDate='" + checkcalpayprocessdate + "',ShiftType='" + Nightshift + "'  WHERE college_code='" + Session["collegecode"] + "' and Shift ='" + ddlshift.SelectedItem.Text + "' and DayType ='" + IntDayType + "'";
                if (rdbstaffcategory.Checked == true)
                {
                    qry = qry + " AND category_code='" + ddlstaffcategorytype.SelectedItem.Value + "'";

                }
                else
                {
                    qry = qry + " AND StfType='" + ddlstaffcategorytype.SelectedItem.Text + "'";
                }
                chkupd = d2.update_method_wo_parameter(qry, "text");

            }
            else if (rdbselecteddate.Checked == true)
            {
                IntDayType = 1;
                qry = "update in_out_time set intime='" + getIntime + "',gracetime='" + getgracetime + "',latetime='" + getLatetime + "',outtime='" + outtime + "',lunch_st_time='" + lunchstartT + "',lunch_end_time='" + lunchEndT + "',nooflate='" + txtnooflate.Text + "',permission_time='" + getperTime + "',college_code='" + Session["collegecode"] + "',noofper='" + txtnoofper.Text + "' ,extend_gracetime='" + getextend_gracetime + "',morn_late='" + latevar + "' ,morn_per='" + pervar + "',manual_Settings='" + manualsettings + "',Shift ='" + ddlshift.SelectedItem.Text + "',Other_Per ='" + val + "',EntryType ='" + IntEntryType + "',StfType ='" + stafftype + "',DayType='" + IntDayType + "',ApplyLeave ='" + ddlsingleday.SelectedItem.Text + "',MorningOutTime ='" + getmrngouttime + "',IsUnRegLeave='" + checkunregistred + "',UnRegLeave='" + unregisteredleave + "',IsCompWork='" + checkcompulsarywork + "',CompWorkType='" + selecteddateorday + "',CompWorkDate='" + comdatetime + "',CompWorkDay='" + compulsaryworkday + "',IsCalPayDate='" + checkcalpayprocessdate + "',ShiftType='" + Nightshift + "' WHERE college_code='" + Session["collegecode"] + "' and Shift ='" + ddlshift.SelectedItem.Text + "' and DayType ='" + IntDayType + "'";
                if (rdbstaffcategory.Checked == true)
                {
                    qry = qry + " AND category_code='" + ddlstaffcategorytype.SelectedItem.Value + "'";

                }
                else
                {
                    qry = qry + " AND StfType='" + ddlstaffcategorytype.SelectedItem.Text + "'";
                }
                chkupd = d2.update_method_wo_parameter(qry, "text");

            }
            else if (rdbseelctedday.Checked == true)
            {
                IntDayType = 2;
                qry = "update in_out_time set intime='" + getIntime + "',gracetime='" + getgracetime + "',latetime='" + getLatetime + "',outtime='" + outtime + "',lunch_st_time='" + lunchstartT + "',lunch_end_time='" + lunchEndT + "',nooflate='" + txtnooflate.Text + "',permission_time='" + getperTime + "',college_code='" + Session["collegecode"] + "',noofper='" + txtnoofper.Text + "' ,extend_gracetime='" + getextend_gracetime + "',morn_late='" + latevar + "' ,morn_per='" + pervar + "',manual_Settings='" + manualsettings + "',Shift ='" + ddlshift.SelectedItem.Text + "',Other_Per ='" + val + "',EntryType ='" + IntEntryType + "',StfType ='" + stafftype + "',DayType='" + IntDayType + "',ApplyLeave ='" + ddlsingleday.SelectedItem.Text + "',MorningOutTime ='" + getmrngouttime + "',IsUnRegLeave='" + checkunregistred + "',UnRegLeave='" + unregisteredleave + "',IsCompWork='" + checkcompulsarywork + "',CompWorkType='" + selecteddateorday + "',CompWorkDate='" + comdatetime + "',CompWorkDay='" + compulsaryworkday + "',IsCalPayDate='" + checkcalpayprocessdate + "',ShiftType='" + Nightshift + "' WHERE college_code='" + Session["collegecode"] + "' and Shift ='" + ddlshift.SelectedItem.Text + "' and DayType ='" + IntDayType + "'";
                if (rdbstaffcategory.Checked == true)
                {
                    qry = qry + " AND category_code='" + ddlstaffcategorytype.SelectedItem.Value + "'";

                }
                else
                {
                    qry = qry + " AND StfType='" + ddlstaffcategorytype.SelectedItem.Text + "'";
                }
                chkupd = d2.update_method_wo_parameter(qry, "text");


            }
            if (chkupd > 0)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Bell Timing is Updated Successfully\");", true);

            }

        }
        catch (Exception ex)
        {

        }

    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        poppe2.Visible = false;
    }
    protected void btn_hostelsave_click(object sender, EventArgs e)
    {
        try
        {
            if (ddlhostaelname.SelectedItem.Text == "")
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Choose a Hostel\");", true);
                return;
            }
            string latevar = string.Empty;
            string pervar = string.Empty;
            if (cbregisted_single.Checked == true)
            {
                if (ddlmrnglate1.SelectedItem.Text == "Select" || ddlmrnglate2.SelectedItem.Text == "Select")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Proper Morning Late Time\");", true);
                    return;
                }
                else if (ddlper1.SelectedItem.Text == "Select" || ddlper2.SelectedItem.Text == "Select")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Proper Morning Permission Time\");", true);
                    return;
                }
                latevar = ddlper1.SelectedItem.Text + "-" + ddlmrnglate2.SelectedItem.Text;
                pervar = ddlper1.SelectedItem.Text + "-" + ddlper1.SelectedItem.Text;

            }
            if (txthostellateallowed.Text == "")
            {
                txthostellateallowed.Text = "0";
            }
            if (txthostelperallowed.Text == "")
            {
                txthostelperallowed.Text = "0";
            }
            string checkmanuel = string.Empty;
            if (cbhostelManuelsetting.Checked == true)
            {
                checkmanuel = "1";
            }
            else
            {
                checkmanuel = "0";
            }
            string checkregflag = string.Empty;
            if (cbregisted_single.Checked == true)
            {
                checkregflag = "1";
            }
            else
            {
                checkregflag = "0";
            }
            DataSet hostds = new DataSet();
            hostds.Clear();
            string qry = "select * from Hostel_InOut_Time where Hostel_Code='" + ddlhostaelname.SelectedItem.Value + "' and college_code='" + Convert.ToString(ddlcollegename.SelectedValue) + "'";
            hostds = d2.select_method_wo_parameter(qry, "text");

            DateTime getoutT = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hosteltoouttime.Hour, hosteltoouttime.Minute, hosteltoouttime.Second, hosteltoouttime.AmPm));
            DateTime mrnLate = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hostelmrnglatetime.Hour, hostelmrnglatetime.Minute, hostelmrnglatetime.Second, hostelmrnglatetime.AmPm));
            // DateTime getLatetime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", latetime.Hour, latetime.Minute, latetime.Second, latetime.AmPm));


            DateTime intimeval = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hosteltointime.Hour, hosteltointime.Minute, hosteltointime.Second, hosteltointime.AmPm));
            DateTime lateTimes = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hosteleveninglate.Hour, hosteleveninglate.Minute, hosteleveninglate.Second, hosteleveninglate.AmPm));
            DateTime FrmoutTime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hostelfromouttime.Hour, hostelfromouttime.Minute, hostelfromouttime.Second, hostelfromouttime.AmPm));

            DateTime FrmInTime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hosteltointime.Hour, hosteltointime.Minute, hosteltointime.Second, hosteltointime.AmPm));
            int count = 0;
            if (hostds.Tables[0].Rows.Count == 0)//delss
            {
                string insqry = "INSERT INTO Hostel_InOut_Time(Hostel_Code,Out_Time,MorLate_Time,Grace_Time,ExtGrace_Time,In_Time,Permission_Time,Late_Time,Tot_Late,Tot_Per,Manual_Setting,Manual_Late,Manual_Permission,College_Code,RegType,FromOutTime,FromInTime) Values('" + ddlhostaelname.SelectedItem.Value + "','" + getoutT + "','" + mrnLate + "','" + getoutT + "','" + getoutT + "','" + intimeval + "','" + intimeval + "','" + lateTimes + "','" + txthostellateallowed.Text + "','" + txthostelperallowed.Text + "','" + checkmanuel + "','" + latevar + "','" + pervar + "','" + Convert.ToString(ddlcollegename.SelectedValue) + "','" + checkregflag + "','" + FrmoutTime + "','" + FrmInTime + "')";
                count = d2.update_method_wo_parameter(insqry, "Text");
                if (count > 0)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Hostel Category added successfully\");", true);
                }
            }
            else
            {
                string updateqry = "UPDATE Hostel_InOut_Time SET Out_Time ='" + getoutT + "',Grace_Time='" + getoutT + "',MorLate_Time ='" + mrnLate + "',ExtGrace_Time='" + getoutT + "',In_Time='" + intimeval + "',Permission_Time='" + intimeval + "',Late_Time='" + lateTimes + "',Tot_Late='" + txthostellateallowed.Text + "',Tot_Per='" + txthostelperallowed.Text + "',Manual_Setting ='" + checkmanuel + "',Manual_Late='" + latevar + "',Manual_Permission='" + pervar + "',RegType ='" + checkregflag + "',FromOutTime='" + FrmoutTime + "',FromInTime='" + FrmInTime + "' WHERE Hostel_Code ='" + ddlhostaelname.SelectedItem.Value + "' and College_Code ='" + Convert.ToString(ddlcollegename.SelectedValue) + "'";
                count = d2.update_method_wo_parameter(updateqry, "text");
                if (count > 0)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Hostel Category Updated Successfully\");", true);
                }

            }
        }
        catch (Exception ex)
        {

        }


    }
    protected void btn_hostelupdate_click(object sender, EventArgs e)
    {
        try
        {
            if (ddlhostaelname.SelectedItem.Text == "")
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Choose a Hostel\");", true);
                return;
            }
            string latevar = string.Empty;
            string pervar = string.Empty;
            if (cbregisted_single.Checked == true)
            {
                if (ddlmrnglate1.SelectedItem.Text == "Select" || ddlmrnglate2.SelectedItem.Text == "Select")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Proper Morning Late Time\");", true);
                    return;
                }
                else if (ddlper1.SelectedItem.Text == "Select" || ddlper2.SelectedItem.Text == "Select")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Proper Morning Permission Time\");", true);
                    return;
                }
                latevar = ddlper1.SelectedItem.Text + "-" + ddlmrnglate2.SelectedItem.Text;
                pervar = ddlper1.SelectedItem.Text + "-" + ddlper1.SelectedItem.Text;

            }
            if (txthostellateallowed.Text == "")
            {
                txthostellateallowed.Text = "0";
            }
            if (txthostelperallowed.Text == "")
            {
                txthostelperallowed.Text = "0";
            }
            string checkmanuel = string.Empty;
            if (cbhostelManuelsetting.Checked == true)
            {
                checkmanuel = "1";
            }
            else
            {
                checkmanuel = "0";
            }
            string checkregflag = string.Empty;
            if (cbregisted_single.Checked == true)
            {
                checkregflag = "1";
            }
            else
            {
                checkregflag = "0";
            }
            DataSet hostds = new DataSet();
            hostds.Clear();
            string qry = "select * from Hostel_InOut_Time where Hostel_Code='" + ddlhostaelname.SelectedItem.Value + "' and college_code='" + Convert.ToString(ddlcollegename.SelectedValue) + "'";
            hostds = d2.select_method_wo_parameter(qry, "text");

            DateTime getoutT = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hosteltoouttime.Hour, hosteltoouttime.Minute, hosteltoouttime.Second, hosteltoouttime.AmPm));
            DateTime mrnLate = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hostelmrnglatetime.Hour, hostelmrnglatetime.Minute, hostelmrnglatetime.Second, hostelmrnglatetime.AmPm));
            // DateTime getLatetime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", latetime.Hour, latetime.Minute, latetime.Second, latetime.AmPm));


            DateTime intimeval = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hosteltointime.Hour, hosteltointime.Minute, hosteltointime.Second, hosteltointime.AmPm));
            DateTime lateTimes = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hosteleveninglate.Hour, hosteleveninglate.Minute, hosteleveninglate.Second, hosteleveninglate.AmPm));
            DateTime FrmoutTime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hostelfromouttime.Hour, hostelfromouttime.Minute, hostelfromouttime.Second, hostelfromouttime.AmPm));

            DateTime FrmInTime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", hosteltointime.Hour, hosteltointime.Minute, hosteltointime.Second, hosteltointime.AmPm));
            int count = 0;

            string updateqry = "UPDATE Hostel_InOut_Time SET Out_Time ='" + getoutT + "',Grace_Time='" + getoutT + "',MorLate_Time ='" + mrnLate + "',ExtGrace_Time='" + getoutT + "',In_Time='" + intimeval + "',Permission_Time='" + intimeval + "',Late_Time='" + lateTimes + "',Tot_Late='" + txthostellateallowed.Text + "',Tot_Per='" + txthostelperallowed.Text + "',Manual_Setting ='" + checkmanuel + "',Manual_Late='" + latevar + "',Manual_Permission='" + pervar + "',RegType ='" + checkregflag + "',FromOutTime='" + FrmoutTime + "',FromInTime='" + FrmInTime + "' WHERE Hostel_Code ='" + ddlhostaelname.SelectedItem.Value + "' and College_Code ='" + Convert.ToString(ddlcollegename.SelectedValue) + "'";
            count = d2.update_method_wo_parameter(updateqry, "text");
            if (count > 0)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Hostel Category Updated Successfully\");", true);
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void btn_hostelexit_click(object sender, EventArgs e)
    {
        poppe2.Visible = false;
    }
    protected void cbregisted_single_checkedchange(object sender, EventArgs e)
    {

        if (cbregisted_single.Checked == true)
        {
            lbltoouttime.Text = "From Time";
            lbltointime.Text = "To Time";
        }
        else
        {
            lbltoouttime.Text = "Out Time";
            lbltointime.Text = "In Time";

        }

    }
    protected void ddlhostaelname_SelectedIndexChanged(object sender, EventArgs e)//delhi
    {
        try
        {
            ds.Clear();
            string sql = "select * from Hostel_InOut_Time where Hostel_Code='" + ddlhostaelname.SelectedItem.Value + "' and college_code='" + ddlcol.SelectedItem.Value + "'";
            ds = d2.select_method_wo_parameter(sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string regtype = Convert.ToString(ds.Tables[0].Rows[0]["RegType"]);
                if (regtype == "True" || regtype == "1")
                {
                    cbregisted_single.Checked = true;
                }
                else
                {
                    cbregisted_single.Checked = false;
                }
                DateTime outtimehost = Convert.ToDateTime(ds.Tables[0].Rows[0]["Out_Time"]);
                //DateTime gracetimehost = Convert.ToDateTime(ds.Tables[0].Rows[0]["Grace_Time"]);
                // DateTime exendtimehost = Convert.ToDateTime(ds.Tables[0].Rows[0]["ExtGrace_Time"]);//checknull for this
                DateTime intimehost = Convert.ToDateTime(ds.Tables[0].Rows[0]["In_Time"]);
                //  DateTime perhost = Convert.ToDateTime(ds.Tables[0].Rows[0]["Permission_Time"]);
                DateTime latehost = Convert.ToDateTime(ds.Tables[0].Rows[0]["Late_Time"]);
                txthostellateallowed.Text = Convert.ToString(ds.Tables[0].Rows[0]["Tot_Late"]);//Tot_Late
                txthostelperallowed.Text = Convert.ToString(ds.Tables[0].Rows[0]["Tot_Per"]);
                DateTime morninglate = Convert.ToDateTime(ds.Tables[0].Rows[0]["MorLate_Time"]);// check true or false
                DateTime fromouthost = Convert.ToDateTime(ds.Tables[0].Rows[0]["FromOutTime"]);
                DateTime frominhost = Convert.ToDateTime(ds.Tables[0].Rows[0]["FromInTime"]);
                MKB.TimePicker.TimeSelector.AmPmSpec am_pm;

                if (outtimehost.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                hosteltoouttime.SetTime(outtimehost.Hour, outtimehost.Minute, outtimehost.Second, am_pm);
                if (intimehost.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                hosteltointime.SetTime(intimehost.Hour, intimehost.Minute, intimehost.Second, am_pm);
                if (latehost.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                hosteleveninglate.SetTime(latehost.Hour, latehost.Minute, latehost.Second, am_pm);
                if (fromouthost.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                hostelfromouttime.SetTime(fromouthost.Hour, fromouthost.Minute, fromouthost.Second, am_pm);
                if (frominhost.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                hostelfromintime.SetTime(hostelfromintime.Hour, hostelfromintime.Minute, hostelfromintime.Second, am_pm);
                if (morninglate.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                hostelmrnglatetime.SetTime(morninglate.Hour, morninglate.Minute, morninglate.Second, am_pm);

            }
            else
            {

                DateTime FromTime = DateTime.Parse("00:00:00 AM");
                MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                if (FromTime.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                hosteltoouttime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                hosteltointime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                hosteleveninglate.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                hostelfromouttime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                hostelfromintime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
                hostelmrnglatetime.SetTime(FromTime.Hour, FromTime.Minute, FromTime.Second, am_pm);
            }


        }
        catch (Exception ex)
        {

        }


    }
    protected void bindhostel()
    {

        try
        {
            string qry = "SELECT HostelMasterPK,HostelName from HM_HostelMaster";//WHERE CollegeCode='" + ddlcol.SelectedItem.Value + "'
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlhostaelname.DataSource = ds;
                ddlhostaelname.DataTextField = "HostelName";
                ddlhostaelname.DataValueField = "HostelMasterPK";
                ddlhostaelname.DataBind();
                ddlhostel.DataSource = ds;
                ddlhostel.DataTextField = "HostelName";
                ddlhostel.DataValueField = "HostelMasterPK";
                ddlhostel.DataBind();
                ddlhostel.Items.Insert(0, new ListItem("All", "0"));

            }


        }
        catch (Exception ex)
        {


        }


    }
    protected void cbhostelManuelsetting_checkedchange(object sender, EventArgs e)
    {

    }
    protected void Image3_Click(object sender, EventArgs e)
    {
        poppe3.Visible = false;
    }
    protected void Cbperiod_checkedchange(object sender, EventArgs e)
    {
        if (Cbperiod.Checked == true)
        {
            lblnoofbreak.Visible = true;
            txtnoofbreak.Visible = true;
        }
        else
        {
            lblnoofbreak.Visible = false;
            txtnoofbreak.Visible = false;

        }
    }
    protected void btnstudsave_click(object sender, EventArgs e)//delsi1912
    {
        try
        {
            string degree = string.Empty;
            string batchyr = string.Empty;
            string semester = string.Empty;
            string period = string.Empty;
            DataSet studds = new DataSet();
            studds.Clear();
            int cc = 0;
            if (cbl_dept.Items.Count > 0)
            {
                string description = txtstuddesc.Text;
                batchyr = Convert.ToString(getCblSelectedValue(cbl_batch));
                degree = Convert.ToString(getCblSelectedValue(cbl_dept));
                //  degree = Convert.ToString(ddlcourse.SelectedItem.Value);
                //  batchyr = Convert.ToString(ddlbatchyear.SelectedItem.Value);
                int noofbreak = 0;
                if (Cbperiod.Checked == true)
                {
                    if (txtnoofbreak.Text != "")
                    {
                        noofbreak = Convert.ToInt32(txtnoofbreak.Text);

                    }

                }
                if (cbl_sem.Items.Count > 0)
                {
                    semester = Convert.ToString(getCblSelectedValue(cbl_sem)); //Convert.ToString(ddlsemyear.SelectedItem.Value);

                }
                else
                {

                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Semester\");", true);
                    return;

                }
                if (ddlperiod.Items.Count > 0)
                {
                    period = Convert.ToString(ddlperiod.SelectedItem.Value);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Period\");", true);
                    return;

                }

                DateTime getstudstarttime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", stustarttime.Hour, stustarttime.Minute, stustarttime.Second, stustarttime.AmPm));
                DateTime getstudendtime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", stuendtime.Hour, stuendtime.Minute, stuendtime.Second, stuendtime.AmPm));

                string studentselquery = "select * from bellschedule where Period1 in('" + period + "') and semester in('" + semester + "') and batch_year in('" + batchyr + "') and Degree_Code in('" + degree + "')";
                int chkupd = 0;
                studds = d2.select_method_wo_parameter(studentselquery, "text");
                {
                    //if (studds.Tables[0].Rows.Count > 0)
                    //{

                    //    string updatequry = "update bellschedule set start_time='" + getstudstarttime + "',end_time='" + getstudendtime + "',Desc1='" + description + "',no_of_breaks='" + noofbreak + "' where Period1='" + period + "' and semester in('" + semester + "') and batch_year in('" + batchyr + "') and Degree_Code in('" + degree + "')";
                    //    chkupd = d2.update_method_wo_parameter(updatequry, "text");
                    //}
                    //   else
                    // {

                    for (int sel = 0; sel < cbl_batch.Items.Count; sel++)
                    {
                        if (cbl_batch.Items[sel].Selected == true)
                        {
                            string btchyr = Convert.ToString(cbl_batch.Items[sel].Value);
                            for (int k = 0; k < cbl_dept.Items.Count; k++)
                            {
                                if (cbl_dept.Items[k].Selected == true)
                                {
                                    string degreecode = Convert.ToString(cbl_dept.Items[k].Value);
                                    for (int val = 0; val < cbl_sem.Items.Count; val++)
                                    {
                                        if (cbl_sem.Items[val].Selected == true)
                                        {
                                            string getsem = Convert.ToString(cbl_sem.Items[val].Value);
                                            string insqry = "if exists (select * from  bellschedule where Period1='" + period + "' and semester='" + getsem + "' and batch_year ='" + btchyr + "' and Degree_Code ='" + degreecode + "') update bellschedule set start_time='" + getstudstarttime + "',end_time='" + getstudendtime + "',Desc1='" + description + "',no_of_breaks='" + noofbreak + "' where Period1='" + period + "' and semester='" + getsem + "' and batch_year ='" + btchyr + "' and Degree_Code ='" + degreecode + "' else insert into bellschedule(Degree_code,Period1,Desc1,start_time,end_time,semester,batch_year,no_of_breaks) values('" + degreecode + "','" + period + "','" + description + "','" + getstudstarttime + "','" + getstudendtime + "','" + getsem + "','" + btchyr + "','" + noofbreak + "')";

                                            chkupd = d2.update_method_wo_parameter(insqry, "text");
                                            cc++;
                                        }


                                    }
                                }
                            }
                        }
                    }


                    //string insqry = "insert into bellschedule(Degree_code,Period1,Desc1,start_time,end_time,semester,batch_year,no_of_breaks) values('" + degree + "','" + period + "','" + description + "','" + getstudstarttime + "','" + getstudendtime + "','" + semester + "','" + batchyr + "','" + noofbreak + "')";
                    //chkupd = d2.update_method_wo_parameter(insqry, "text");
                    //   }
                    if (chkupd > 0 || cc > 0)
                    {
                        ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
                    }

                }

            }



        }
        catch (Exception ex)
        {

        }

    }
    protected void btnstudexit_click(object sender, EventArgs e)
    {
        poppe3.Visible = false;
    }
    protected void grdstudbell_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Visible = false;

                CheckBox chk1 = e.Row.FindControl("selectchk") as CheckBox;
                chk1.Text = "";

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Visible = false;

                CheckBox chk1 = e.Row.FindControl("selectchk") as CheckBox;
                chk1.Text = "";

            }
        }
        catch (Exception ex)
        {

        }

    }
    protected void grdstudbell_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {


    }
    protected void bindstudyear()
    {
        try
        {
            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    //protected void chkcourse_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkcourse.Checked == true)
    //    {
    //        for (int i = 0; i < chklcourse.Items.Count; i++)
    //        {
    //            chklcourse.Items[i].Selected = true;
    //        }
    //        txtcourse.Text = "Degree(" + chklcourse.Items.Count + ")";
    //    }
    //    else
    //    {
    //        for (int i = 0; i < chklcourse.Items.Count; i++)
    //        {
    //            chklcourse.Items[i].Selected = false;
    //        }
    //        txtcourse.Text = "---Select---";
    //    }
    //}
    //protected void chklcourse_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    txtcourse.Text = "---Select---";
    //    chkcourse.Checked = false;
    //    int copu = 0;
    //    for (int i = 0; i < chklcourse.Items.Count; i++)
    //    {
    //        if (chklcourse.Items[i].Selected == true)
    //        {
    //            copu++;
    //        }
    //    }
    //    if (copu > 0)
    //    {
    //        txtcourse.Text = "Degree(" + copu + ")";
    //        if (copu == chklcourse.Items.Count)
    //        {
    //            chkcourse.Checked = true;
    //        }
    //    }
    //}
    protected void bindcoursestud()
    {
        try
        {
            DataSet studds = new DataSet();
            string stuqry = "select distinct(degree_code),duration,First_Year_Nonsemester,(course_name + ' - ' + dept_name) as course_name from degree,course,department where department.dept_code=degree.dept_code and course.course_id=degree.course_id and degree.college_code='" + Convert.ToString(Session["collegecode"]) + "' order by degree_code";
            studds = d2.select_method_wo_parameter(stuqry, "text");
            if (studds.Tables[0].Rows.Count > 0)
            {
                //ddlcourse.DataSource = studds;
                //ddlcourse.DataTextField = "course_name";
                //ddlcourse.DataValueField = "degree_code";
                //ddlcourse.DataBind();

                ddlstudegreecode.DataSource = studds;
                ddlstudegreecode.DataTextField = "course_name";
                ddlstudegreecode.DataValueField = "degree_code";
                ddlstudegreecode.DataBind();
                //for (int i = 0; i < chklcourse.Items.Count; i++)
                //{
                //    chklcourse.Items[i].Selected = true;

                //}
                //txtcourse.Text = "Course(" + chklcourse.Items.Count + ")";
                //chkcourse.Checked = true;

            }


        }
        catch (Exception ex)
        {


        }
    }
    public void bindsem()
    {
        // DataSet semds = new DataSet();
        // ddlsemyear.Items.Clear();
        // ddlstusem.Items.Clear();
        // semds.Clear();
        // bool first_year;
        // first_year = false;
        // int duration = 0;
        // int i = 0;
        // string degreecode = string.Empty;
        //string  batchyr = Convert.ToString(getCblSelectedValue(cbl_batch));
        //string degreecodes = Convert.ToString(getCblSelectedValue(cbl_dept));

        try
        {
            int semcount = 0;
            string collcode = Convert.ToString(ddl_popclg1.SelectedValue);

            string getcount = d2.GetFunction("select MAX(NDurations) as Sem from Ndegree where college_code='" + collcode + "'");
            if (getcount.Trim() != "" && getcount.Trim() != "0")
            {
                cbl_sem.Items.Clear();
                ddlstusem.Items.Clear();
                Int32.TryParse(getcount, out semcount);
                for (int ik = 1; ik <= semcount; ik++)
                {
                    cbl_sem.Items.Add(new ListItem(Convert.ToString(ik), Convert.ToString(ik)));
                    ddlstusem.Items.Add(new ListItem(Convert.ToString(ik), Convert.ToString(ik)));
                }
                cbl_sem.DataBind();
                if (cbl_sem.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                    }
                    txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
                    cb_sem.Checked = true;
                }
            }
        }
        catch { }

        // string cmd = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code in('" + dd + "') and batch_year='" + batchyr + "' and college_code='" + Convert.ToString(Session["collegecode"]) + "'";

        //semds = d2.select_method_wo_parameter(cmd, "text");
        //if (semds.Tables[0].Rows.Count > 0)
        //{

        //    for (int semdu = 0; semdu < semds.Tables[0].Rows.Count; semdu++)
        //    {
        //        first_year = Convert.ToBoolean(semds.Tables[0].Rows[semdu]["first_year_nonsemester"]);
        //        duration = Convert.ToInt16(semds.Tables[0].Rows[semdu]["ndurations"]);

        //        for (i = 1; i <= duration; i++)
        //        {
        //            if (first_year == false)
        //            {
        //                ddlsemyear.Items.Add(i.ToString());
        //                ddlstusem.Items.Add(i.ToString());
        //            }
        //            else if (first_year == true && i != 2)
        //            {
        //                ddlsemyear.Items.Add(i.ToString());
        //                ddlstusem.Items.Add(i.ToString());
        //            }
        //        }
        //    }
        //}
        //else
        //{

        //    cmd = "select distinct duration,first_year_nonsemester  from degree where degree_code in('" + ddlcourse.SelectedItem.Value + "')  and college_code='" + Convert.ToString(Session["collegecode"]) + "'";
        //    ddlsemyear.Items.Clear();
        //    semds = d2.select_method_wo_parameter(cmd, "text");
        //    if (semds.Tables[0].Rows.Count > 0)
        //    {
        //        for (int semdu = 0; semdu < semds.Tables[0].Rows.Count; semdu++)
        //        {
        //            first_year = Convert.ToBoolean(semds.Tables[0].Rows[semdu]["first_year_nonsemester"]);
        //            duration = Convert.ToInt16(semds.Tables[0].Rows[semdu]["duration"]);
        //            for (i = 1; i <= duration; i++)
        //            {
        //                if (first_year == false)
        //                {
        //                    ddlsemyear.Items.Add(i.ToString());
        //                    ddlstusem.Items.Add(i.ToString());
        //                }
        //                else if (first_year == true && i != 2)
        //                {
        //                    ddlsemyear.Items.Add(i.ToString());
        //                    ddlstusem.Items.Add(i.ToString());
        //                }
        //            }
        //        }
        //    }
        //dr1.Close();
        // }
        //if (ddlsem.Items.Count > 0)
        //{
        //    ddlsem.SelectedIndex = 0;
        //    BindSectionDetail();
        //}
        //con.Close();
    }
    public void bindperiod()
    {
        //ds.Clear();
        //ddlperiod.Items.Clear();

        string sems = Convert.ToString(getCblSelectedValue(cbl_sem));
        string degreecodes = Convert.ToString(getCblSelectedValue(cbl_dept));
        string getperiod = d2.GetFunction("select No_of_hrs_per_day from periodattndschedule where degree_code in('" + degreecodes + "') and semester in('" + sems + "')");
        if (getperiod != "0" || getperiod != "")
        {
            int period = Convert.ToInt32(getperiod);
            for (int val = 1; val <= period; val++)
            {
                ddlperiod.Items.Add(Convert.ToString(val));
            }
        }
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    ddlcourse.DataSource = ds;
        //    ddlcourse.DataTextField = "course_name";
        //    ddlcourse.DataValueField = "degree_code";
        //    ddlcourse.DataBind();

        //}

    }
    protected void ddlcourse_selectedchange(object sender, EventArgs e)
    {
        // bindsem();
        // bindperiod();
    }
    protected void ddlsemyear_selectedchange(object sender, EventArgs e)
    {
        // bindperiod();
    }
    protected void ddlstudegreecode_changed(object sender, EventArgs e)
    {
        // bindsem();
    }

    protected void OnRowCreatedgrdstud(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndexstud.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdstud_RowDataBound(object sende, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    e.Row.BackColor = Color.FromArgb(12, 166, 202);
                    e.Row.HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Width = 200;
                    e.Row.Font.Bold = true;
                }
                e.Row.Cells[0].Width = 50;
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                if (e.Row.RowIndex != 0)
                {
                }
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void btnstudupdate_click(object sender, EventArgs e)
    {
        try
        {
            string degree = string.Empty;
            string batchyr = string.Empty;
            string semester = string.Empty;
            string period = string.Empty;
            DataSet studds = new DataSet();
            int chkupd = 0;
            studds.Clear();
            // if (ddlcourse.Items.Count > 0)
            // {
            string description = txtstuddesc.Text;
            batchyr = Convert.ToString(getCblSelectedValue(cbl_batch));
            degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            semester = Convert.ToString(getCblSelectedValue(cbl_sem));
            // degree = Convert.ToString(ddlcourse.SelectedItem.Value);
            // batchyr = Convert.ToString(ddlbatchyear.SelectedItem.Value);
            int noofbreak = 0;
            if (Cbperiod.Checked == true)
            {
                if (txtnoofbreak.Text != "")
                {
                    noofbreak = Convert.ToInt32(txtnoofbreak.Text);

                }

            }
            //if (ddlsemyear.Items.Count > 0)
            //{
            //    semester = Convert.ToString(ddlsemyear.SelectedItem.Value);

            //}
            //else
            //{

            //    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Semester\");", true);
            //    return;

            //}
            if (ddlperiod.Items.Count > 0)
            {
                period = Convert.ToString(ddlperiod.SelectedItem.Value);
            }
            else
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Period\");", true);
                return;

            }

            DateTime getstudstarttime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", stustarttime.Hour, stustarttime.Minute, stustarttime.Second, stustarttime.AmPm));
            DateTime getstudendtime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", stuendtime.Hour, stuendtime.Minute, stuendtime.Second, stuendtime.AmPm));


            string updatequry = "update bellschedule set start_time='" + getstudstarttime + "',end_time='" + getstudendtime + "',Desc1='" + description + "',no_of_breaks='" + noofbreak + "' where Period1='" + period + "' and semester='" + semester + "' and batch_year='" + batchyr + "' and Degree_Code='" + degree + "'";
            chkupd = d2.update_method_wo_parameter(updatequry, "text");

            if (chkupd > 0)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
            }

            // }

        }
        catch (Exception ex)
        {

        }

    }
    protected void btndel_click(object sender, EventArgs e)
    {
        try
        {
            string stafftype = string.Empty;
            string staffcategory = string.Empty;
            DataSet ds = new DataSet();
            int IntEntryType = 0;
            int IntDayType = 0;
            string latevar = string.Empty;
            string pervar = string.Empty;
            int manualsettings = 0;
            if (rdbstaffcategory.Checked == true)
            {
                if (ddlstaffcategorytype.SelectedItem.Text == "")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Choose a category\");", true);
                    return;
                }

            }
            else
            {
                if (ddlstaffcategorytype.SelectedItem.Text == "")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Choose a Staff Type\");", true);
                    return;

                }
            }
            if (ddlshift.SelectedItem.Text.Trim() == "Select")
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Shift\");", true);
                return;
            }
            if (cbunregisteredstaff.Checked == true && ddlunregistered.Text == "")
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Shift\");", true);
                return;
            }
            if (cbcompworking.Checked == true)
            {
                if (rdbselecteddatecomp.Checked == false && rdbselecteddaycomp.Checked == false)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Compulsory working type\");", true);
                    return;
                }


            }


            if (cbmanuelsetting.Checked == true)
            {


                if (ddl1.SelectedItem.Text == "Select" && ddl2.SelectedItem.Text == "Select")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Select Proper Morning Late Time\");", true);
                    return;

                }
                else if (ddl3.SelectedItem.Text == "Select" && ddl4.SelectedItem.Text == "Select")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Proper Morning Permission Time\");", true);
                    return;

                }
                latevar = ddl1.SelectedItem.Text + "-" + ddl2.SelectedItem.Text;
                pervar = ddl3.SelectedItem.Text + "-" + ddl4.SelectedItem.Text;
                manualsettings = 1;

            }
            if (txtnooflate.Text == "")
            {
                txtnooflate.Text = "0";
            }
            if (txtnoofper.Text == "")
            {
                txtnoofper.Text = "0";

            }
            string catcode = string.Empty;
            if (rdbstaffcategory.Checked == true)
            {
                IntEntryType = 0;
                catcode = Convert.ToString(ddlstaffcategorytype.SelectedItem.Value);
                staffcategory = Convert.ToString(ddlstaffcategorytype.SelectedItem.Text);
                //load catcode
            }
            else
            {
                IntEntryType = 1;
                stafftype = Convert.ToString(ddlstaffcategorytype.SelectedItem.Text);

                //load stftype

            }
            if (rdballdays.Checked == true)
            {
                IntDayType = 0;
            }
            else if (rdbselecteddate.Checked == true)
            {
                IntDayType = 1;

            }
            else if (rdbseelctedday.Checked == true)
            {
                IntDayType = 2;
            }
            int val = 0;
            if (chkotherpermission.Checked == true)
            {
                val = 1;
            }
            int checkunregistred = 0;
            string unregisteredleave = string.Empty;
            if (cbunregisteredstaff.Checked == true)
            {
                checkunregistred = 1;
                unregisteredleave = Convert.ToString(ddlunregistered.SelectedItem.Text);
            }
            int checkcompulsarywork = 0;
            int selecteddateorday = 1;
            string compulsarydatetime = string.Empty;
            string compulsaryworkday = string.Empty;
            DateTime comdatetime = new DateTime();

            if (cbcompworking.Checked == true)
            {
                checkcompulsarywork = 1;
                if (rdbselecteddatecomp.Checked == true)
                {
                    selecteddateorday = 0;
                    compulsarydatetime = Txtentryto.Text;
                    if (compulsarydatetime.Contains('/'))
                    {
                        string[] splitdate = compulsarydatetime.Split('/');
                        compulsarydatetime = splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2];
                        comdatetime = Convert.ToDateTime(compulsarydatetime);

                    }


                }
                if (rdbselecteddaycomp.Checked == true)
                {
                    compulsaryworkday = ddlcompworking.SelectedItem.Value;

                }

            }
            else
            {
                compulsarydatetime = Txtentryto.Text;
                if (compulsarydatetime.Contains('/'))
                {
                    string[] splitdate = compulsarydatetime.Split('/');
                    compulsarydatetime = splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2];
                    comdatetime = Convert.ToDateTime(compulsarydatetime);

                }

            }
            int checkcalpayprocessdate = 0;
            if (cbpayprocess.Checked == true)
            {
                checkcalpayprocessdate = 1;
            }
            DateTime getIntime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", intime.Hour, intime.Minute, intime.Second, intime.AmPm));
            DateTime getgracetime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", gracetime.Hour, gracetime.Minute, gracetime.Second, gracetime.AmPm));
            DateTime getLatetime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", latetime.Hour, latetime.Minute, latetime.Second, latetime.AmPm));
            DateTime outtime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", eveningout.Hour, eveningout.Minute, eveningout.Second, eveningout.AmPm));
            DateTime lunchstartT = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", starttime.Hour, starttime.Minute, starttime.Second, starttime.AmPm));
            DateTime lunchEndT = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", endtime.Hour, endtime.Minute, endtime.Second, endtime.AmPm));
            DateTime getperTime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", permission.Hour, permission.Minute, permission.Second, permission.AmPm));
            DateTime getextend_gracetime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", exgracetime.Hour, exgracetime.Minute, exgracetime.Second, exgracetime.AmPm));
            DateTime getmrngouttime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", outtimebell.Hour, outtimebell.Minute, outtimebell.Second, outtimebell.AmPm));
            string qry = string.Empty;
            int chkupd = 0;
            if (rdballdays.Checked == true)
            {

                IntDayType = 0;
                qry = "delete in_out_time  WHERE college_code='" + Session["collegecode"] + "' and Shift ='" + ddlshift.SelectedItem.Text + "' and DayType ='" + IntDayType + "'";
                if (rdbstaffcategory.Checked == true)
                {
                    qry = qry + " AND category_code='" + ddlstaffcategorytype.SelectedItem.Value + "'";

                }
                else
                {
                    qry = qry + " AND StfType='" + ddlstaffcategorytype.SelectedItem.Text + "'";
                }
                chkupd = d2.update_method_wo_parameter(qry, "text");
                if (chkupd > 0)
                {

                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Deleted Successfully\");", true);

                }

            }

        }
        catch (Exception ex)
        {

        }

    }
    protected void btndeletestud_click(object sender, EventArgs e)
    {
        try
        {

            string degree = string.Empty;
            string batchyr = string.Empty;
            string semester = string.Empty;
            string period = string.Empty;
            DataSet studds = new DataSet();
            int chkupd = 0;
            studds.Clear();

            string description = txtstuddesc.Text;
            // degree = Convert.ToString(ddlcourse.SelectedItem.Value);
            // batchyr = Convert.ToString(ddlbatchyear.SelectedItem.Value);
            batchyr = Convert.ToString(getCblSelectedValue(cbl_batch));
            degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            semester = Convert.ToString(getCblSelectedValue(cbl_sem));
            int noofbreak = 0;
            if (Cbperiod.Checked == true)
            {
                if (txtnoofbreak.Text != "")
                {
                    noofbreak = Convert.ToInt32(txtnoofbreak.Text);

                }

            }

            if (ddlperiod.Items.Count > 0)
            {
                period = Convert.ToString(ddlperiod.SelectedItem.Value);
            }
            else
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Period\");", true);
                return;

            }

            DateTime getstudstarttime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", stustarttime.Hour, stustarttime.Minute, stustarttime.Second, stustarttime.AmPm));
            DateTime getstudendtime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", stuendtime.Hour, stuendtime.Minute, stuendtime.Second, stuendtime.AmPm));


            string updatequry = "delete bellschedule where Period1='" + period + "' and semester='" + semester + "' and batch_year='" + batchyr + "' and Degree_Code='" + degree + "'";
            chkupd = d2.update_method_wo_parameter(updatequry, "text");

            if (chkupd > 0)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Deleted Successfully\");", true);
            }


        }
        catch (Exception ex)
        {

        }

    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string batch = "";
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {

                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                    batch = Convert.ToString(cbl_batch.Items[i].Text);
                }
                if (cbl_batch.Items.Count == 1)
                {
                    txt_batch.Text = "" + batch + "";

                }
                else
                {
                    txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
            }
            binddeg();
            binddept();
        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_batch.Checked = false;
            int commcount = 0;
            string batch = "";
            txt_batch.Text = "--Select--";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    batch = Convert.ToString(cbl_batch.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_batch.Text = "" + batch + "";
                }
                else
                {
                    txt_batch.Text = "Batch(" + commcount.ToString() + ")";
                }

            }
            binddeg();
            binddept();
        }
        catch { }
    }



    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string degree = "";
            txt_degree.Text = "--Select--";
            if (cb_degree.Checked == true)
            {

                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                    degree = Convert.ToString(cbl_degree.Items[i].Text);
                }
                if (cbl_degree.Items.Count == 1)
                {
                    txt_degree.Text = "" + degree + "";

                }
                else
                {
                    txt_degree.Text = lbldeg.Text + "(" + (cbl_degree.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
            }
            binddept();
        }
        catch { }
    }

    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string degree = "";
            int i = 0;
            cb_dept.Checked = false;
            int commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    degree = Convert.ToString(cbl_degree.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_degree.Text = "" + degree + "";
                }
                else
                {
                    txt_degree.Text = lbldeg.Text + "(" + commcount.ToString() + ")";
                }

            }
            binddept();
        }
        catch { }
    }
    protected void binddeg()
    {
        try
        {

            string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id   and d.college_code='" + Session["collegecode"] + "'";


            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {

                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "Course_Name";
                cbl_degree.DataValueField = "Course_Id";
                cbl_degree.DataBind();
            }
            for (int j = 0; j < cbl_degree.Items.Count; j++)
            {
                cbl_degree.Items[j].Selected = true;
                cb_degree.Checked = true;
            }
            txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
            binddept();
        }
        catch { }

    }

    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string dept = "";
            int i = 0;
            txt_dept.Text = "--Select--";
            if (cb_dept.Checked == true)
            {

                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                    dept = Convert.ToString(cbl_dept.Items[i].Text);
                }
                if (cbl_dept.Items.Count == 1)
                {
                    txt_dept.Text = "" + dept + "";

                }
                else
                {
                    txt_dept.Text = lbldept.Text + "(" + (cbl_dept.Items.Count) + ")";
                }

            }
            else
            {
                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = false;
                }
            }

            bindsem();
            bindperiod();
        }
        catch { }
    }

    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string dept = "";
            int i = 0;
            cb_dept.Checked = false;
            int commcount = 0;
            txt_dept.Text = "--Select--";
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    dept = Convert.ToString(cbl_dept.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_dept.Items.Count)
                {
                    cb_dept.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_dept.Text = "" + dept + "";
                }
                else
                {
                    txt_dept.Text = lbldept.Text + "(" + commcount.ToString() + ")";
                }

            }

            bindsem();
            bindperiod();
        }
        catch { }
    }
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            string batch2 = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch2 == "")
                    {
                        batch2 = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch2 += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }

            string degree = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }

            if (batch2 != "" && degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = lbldept.Text + "(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }

        }
        catch { }
    }
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sem.Text = "--Select--";
            string sem = "";
            if (cb_sem.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                    sem = Convert.ToString(cbl_sem.Items[i].Text);
                }
                if (cbl_sem.Items.Count == 1)
                {
                    txt_sem.Text = "" + sem + "";
                }
                else
                {
                    txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
            //bindsec();
            bindperiod();
        }
        catch (Exception ex)
        {

        }
    }

    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem.Checked = false;
            int commcount = 0;
            txt_sem.Text = "--Select--";
            string sem = "";

            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sem.Checked = false;
                    sem = Convert.ToString(cbl_sem.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {
                    cb_sem.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_sem.Text = "" + sem + "";
                }
                else
                {
                    txt_sem.Text = "Semester(" + commcount.ToString() + ")";
                }
            }
            bindperiod();
            // bindsec();

        }
        catch (Exception ex)
        {

        }
    }
    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    protected void ddl_popclg1_change(object sender, EventArgs e)
    {
        if (ddl_popclg1.Items.Count > 0)
            popclg1 = Convert.ToString(ddl_popclg1.SelectedValue);
        bindstudyear();
        binddeg();
        binddept();

        bindsem();
        bindcoursestud();
        bindperiod();
    }

}