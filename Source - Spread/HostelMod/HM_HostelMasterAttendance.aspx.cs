using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Drawing;

public partial class HM_HostelMasterAttendance : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    bool check = false;
    bool checkdate = false;
    string q = "";
    string fromdate = "";
    string todate = "";
    int i = 0;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        caltodate.EndDate = DateTime.Now;
        calfromdate.EndDate = DateTime.Now;
        usercode = Session["usercode"].ToString();
        lblvalidation1.Text = "";
        if (!IsPostBack)
        {
            bindhostelname();
            bindbatch();
            cb_floorname.Checked = true;
            cb_floorname_CheckedChange(sender, e);
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Attributes.Add("readonly", "readonly");
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Visible = false;
            cbboth.Checked = true;
             
        }
       
    }
    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_hostelname.Checked == true)
        {
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = true;
            }
            txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = false;
            }
            txt_hostelname.Text = "--Select--";
            txt_floorname.Text = "--Select--";
        }
        bindbuilding();
        bindfloor();
        bindroom();
        cb_floorname_CheckedChange(sender, e);
        cbl_floorname_SelectedIndexChanged(sender, e);

    }
    protected void cbl_hostelname_SelectIndexChange(object sender, EventArgs e)
    {
        txt_hostelname.Text = "--Select--";
        cb_hostelname.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_hostelname.Items.Count; i++)
        {
            if (cbl_hostelname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
            if (commcount == cbl_hostelname.Items.Count)
            {
                cb_hostelname.Checked = true;
            }
        }
        bindbuilding();
        bindfloor();
        bindroom();
        cb_floorname_CheckedChange(sender, e);
        cbl_floorname_SelectedIndexChanged(sender, e);
    }
    public void bindhostelname()
    {
        try
        {
            ds.Clear();
            cbl_hostelname.Items.Clear();
            //string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            //ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            string MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + usercode + "'");
         
            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster where  HostelMasterPK in (" + MessmasterFK + ") order by hostelname ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "text");
          
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();

                if (cbl_hostelname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                    }

                    txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
                }
                bindbuilding();
                bindfloor();
                bindroom();

            }
            else
            {
                txt_hostelname.Text = "--Select--";
                txt_floorname.Text = "--Select--";
                cbl_floorname.Items.Clear();
                cb_floorname.Checked = false;

            }
        }
        catch
        {
        }
    }
    protected void txt_fromdate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            lbl_error1.Visible = false;
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                if (from > to)
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Please Enter To Date Greater Than From Date";
                    FpSpread1.Visible = false;
                    dat.Visible = false;
                    rptprint.Visible = false;
                    lbl_errorsearch1.Visible = false;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    lbl_error.Visible = false;
                    btn_save.Visible = false;
                    btn_update.Visible = false;
                    btn_reset.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_error1.Visible = true;
            lbl_error1.Text = ex.ToString();
        }
    }
    protected void txt_todate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            lbl_error1.Visible = false;
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            if (fromdate != "" && fromdate != null && todate != "" && todate != null)
            {
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);

                string todate1 = DateTime.Now.ToString("dd/MM/yyyy");
                string[] todate2 = todate1.Split('/');
                DateTime todate3 = Convert.ToDateTime(todate2[1] + '/' + todate2[0] + '/' + todate2[2]);


                if (from > to)// && to <= todate3
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Please Enter To Date Grater Than From Date";
                    FpSpread1.Visible = false;
                    dat.Visible = false;
                    rptprint.Visible = false;
                    lbl_errorsearch1.Visible = false;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    lbl_error.Visible = false;
                    btn_save.Visible = false;
                    btn_update.Visible = false;
                    btn_reset.Visible = false;
                }
                if (to > todate3)
                {
                    lbl_error1.Visible = true;
                    lbl_errorsearch1.Visible = false;
                    lbl_error1.Text = "Don't Enter Future Date";
                    FpSpread1.Visible = false;
                    dat.Visible = false;
                    rptprint.Visible = false;
                    lbl_errorsearch1.Visible = false;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    lbl_error.Visible = false;
                    btn_save.Visible = false;
                    btn_update.Visible = false;
                    btn_reset.Visible = false;
                    rptprint.Visible = false;

                }
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }
    protected void ddl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindfloor();
        txt_floorname.Text = "Floor Name (" + cbl_floorname.Items.Count + ")";
        cb_floorname.Checked = true;
        cb_floorname_CheckedChange(sender, e);
        if (ddl_hostelname.Text == "select")
        {
            txt_floorname.Text = "--Select--";
        }
        fpspreadvisiblefalse();
        rptprint.Visible = false;

    }
    public void fpspreadvisiblefalse()
    {
        FpSpread1.Visible = false;
        dat.Visible = false;
        btn_save.Visible = false;
        btn_update.Visible = false;
        btn_reset.Visible = false;

    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void cbl_floorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_floorname.Checked = false;
            txt_floorname.Text = "--Select--";

            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_floorname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_floorname.Items.Count)
                {
                    cb_floorname.Checked = true;
                }
                txt_floorname.Text = "Floor Name(" + commcount.ToString() + ")";
            }
            bindroom();
        }
        catch { }
    }
    protected void cb_floorname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            int c = 0;
            if (cb_floorname.Checked == true)
            {
                c++;
                for (i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = true;
                }
                txt_floorname.Text = "Floor Name(" + (cbl_floorname.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = false;
                }
                txt_floorname.Text = "--Select--";
            }
            bindroom();
        }
        catch { }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
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
            string degreedetails = "Hostel Student Attendance Report";
            string pagename = "HM_HostelMasterAttendance.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            bool saveflage = false;
            if (txt_floorname.Text.Trim() != "--Select--")
            {
                FpSpread1.SaveChanges();
                string[] spiltfrom = fromdate.Split('/');
                string[] spitto = todate.Split('/');
                DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
                DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);
                string attnday = spiltfrom[0];
                attnday = attnday.TrimStart('0');
                string attnmonth = spiltfrom[1];
                attnmonth = attnmonth.TrimStart('0');
                string attnyear = spiltfrom[2];
                string Attendance = ""; string AttendanceE = "";
                string rollno = "";
                string insertquery = "";
                string columngetvalue = "";
                string AttndDayvalue = "";
                string AttndEven = "";

                if (from > to)
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Please Enter To Date Grater Than From Date";
                }
                else
                {
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        int colnewvlaue = Convert.ToInt32(ViewState["colcountnewvalue"]);
                        if (cbboth.Checked == true)
                            colnewvlaue = colnewvlaue;
                        else
                            colnewvlaue = colnewvlaue - 1;
                        for (int col = colnewvlaue; col < FpSpread1.Sheets[0].ColumnCount; col += 4)
                        {
                            columngetvalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col - 1].Text);
                            spiltfrom = columngetvalue.Split('/');
                            AttndDayvalue = Convert.ToString(spiltfrom[0]);
                            AttndDayvalue = AttndDayvalue.TrimStart('0');
                            attnday = AttndDayvalue;
                            AttndDayvalue = "[D" + AttndDayvalue + "]";

                            AttndEven = "[D" + attnday + "E]";

                            attnmonth = spiltfrom[1];
                            attnmonth = attnmonth.TrimStart('0');
                            attnyear = spiltfrom[2];


                            string hostelcode = ""; // Convert.ToString(ddl_hostelname.SelectedItem.Value);

                            for (int j = 1; j < FpSpread1.Sheets[0].RowCount; j++)
                            {
                                hostelcode = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 4].Tag);

                                Attendance = "0";
                                AttendanceE = "0";
                                rollno = Convert.ToString(FpSpread1.Sheets[0].Cells[j, 1].Tag);
                                if (rollno.Trim() != "")
                                {
                                    string app_no = d2.GetFunction("select App_No from Registration where Roll_No='" + rollno.Trim() + "' ");//and college_code='" + collegecode1 + "'
                                    if (cbboth.Checked)
                                    {
                                        int checkvalue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j, col - 1].Value);
                                        if (checkvalue == 1)
                                        {
                                            Attendance = "1";
                                        }

                                        int checkvalue1 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j, col].Value);
                                        if (checkvalue1 == 1)
                                        {
                                            Attendance = "2";
                                        }

                                        int checkvalue2 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j, col + 1].Value);
                                        if (checkvalue2 == 1)
                                        {
                                            Attendance = "3";
                                        }
                                        int checkvalueE = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j + 1, col - 1].Value);
                                        if (checkvalueE == 1)
                                        {
                                            AttendanceE = "1";
                                        }

                                        int checkvalueE1 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j + 1, col].Value);
                                        if (checkvalueE1 == 1)
                                        {
                                            AttendanceE = "2";
                                        }

                                        int checkvalueE2 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j + 1, col + 1].Value);
                                        if (checkvalueE2 == 1)
                                        {
                                            AttendanceE = "3";
                                        }
                                    }
                                    else if (cbmor.Checked == true)
                                    {
                                        int checkvalue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j, col - 1].Value);
                                        if (checkvalue == 1)
                                        {
                                            Attendance = "1";
                                        }

                                        int checkvalue1 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j, col].Value);
                                        if (checkvalue1 == 1)
                                        {
                                            Attendance = "2";
                                        }

                                        int checkvalue2 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j, col + 1].Value);
                                        if (checkvalue2 == 1)
                                        {
                                            Attendance = "3";
                                        }
                                    }
                                    else
                                    {
                                        int checkvalueE = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j, col - 1].Value);
                                        if (checkvalueE == 1)
                                        {
                                            AttendanceE = "1";
                                        }

                                        int checkvalueE1 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j, col].Value);
                                        if (checkvalueE1 == 1)
                                        {
                                            AttendanceE = "2";
                                        }

                                        int checkvalueE2 = Convert.ToInt32(FpSpread1.Sheets[0].Cells[j, col + 1].Value);
                                        if (checkvalueE2 == 1)
                                        {
                                            AttendanceE = "3";
                                        }
                                    }

                                    FpSpread1.SaveChanges();
                                    if (cbboth.Checked)
                                    {
                                        insertquery = "if exists (select * from HT_Attendance where App_No ='" + app_no.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "') update HT_Attendance set " + AttndDayvalue.Trim() + "=" + Attendance.Trim() + "," + AttndEven.Trim() + "=" + AttendanceE.Trim() + " where App_No ='" + app_no.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "' else insert into HT_Attendance(App_No,AttnMonth,AttnYear," + AttndDayvalue.Trim() + "," + AttndEven.Trim() + ") values ('" + app_no.Trim() + "','" + attnmonth.Trim() + "','" + attnyear.Trim() + "','" + Attendance.Trim() + "','" + AttendanceE.Trim() + "')";

                                    }
                                    else if (cbmor.Checked == true)
                                    {

                                        insertquery = "if exists (select * from HT_Attendance where App_No ='" + app_no.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "') update HT_Attendance set " + AttndDayvalue.Trim() + "=" + Attendance.Trim() + " where App_No ='" + app_no.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "' else insert into HT_Attendance(App_No,AttnMonth,AttnYear," + AttndDayvalue.Trim() + ") values ('" + app_no.Trim() + "','" + attnmonth.Trim() + "','" + attnyear.Trim() + "','" + Attendance.Trim() + "')";

                                    }
                                    else
                                    {
                                        insertquery = "if exists (select * from HT_Attendance where App_No ='" + app_no.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "') update HT_Attendance set " + AttndEven.Trim() + "=" + AttendanceE.Trim() + " where App_No ='" + app_no.Trim() + "' and AttnMonth='" + attnmonth.Trim() + "' and AttnYear='" + attnyear.Trim() + "' else insert into HT_Attendance(App_No,AttnMonth,AttnYear," + AttndEven.Trim() + ") values ('" + app_no.Trim() + "','" + attnmonth.Trim() + "','" + attnyear.Trim() + "','" + AttendanceE.Trim() + "')";
                                    }
                                    int retu = d2.update_method_wo_parameter(insertquery, "Text");
                                    if (retu != 0)
                                    {
                                        saveflage = true;
                                    }
                                }
                            }
                        }
                    }
                    if (saveflage == true)
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Saved Successfully";
                        alertmessage.Visible = true;
                    }
                    else
                    {
                        lbl_alerterror.Visible = true;
                        lbl_alerterror.Text = "Please Update Attendance";
                        alertmessage.Visible = true;
                    }
                }
            }
        }
        catch { }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertmessage.Visible = false;
    }
    public void bindbuilding()
    {
        try
        {
            string hostel = "";

            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (hostel == "")
                    {
                        hostel = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostel = hostel + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }
           
            string building = string.Empty;
            string build ="select HostelBuildingFK From  HM_HostelMaster where HostelMasterPK IN ('" + hostel + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(build, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                   
                        if (building == "")
                        {
                            building = "" + Convert.ToString(ds.Tables[0].Rows[i]["HostelBuildingFK"] )+ "";
                        }
                        else
                        {
                            building = building + "" + "," + "" + Convert.ToString(ds.Tables[0].Rows[i]["HostelBuildingFK"]) + "";
                        }
                    }
                }
            

            string itemname = "select * from  Building_Master where code in(" + building + ")";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                drbbuilding.DataSource = ds;
                drbbuilding.DataTextField = "Building_name";
                drbbuilding.DataValueField = "code";
                drbbuilding.DataBind();
            }
          
        }
        catch
        {
        }
    }
    public void bindfloor()
    {
        try
        {
            //string hostel = "";

            //for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            //{
            //    if (cbl_hostelname.Items[i].Selected == true)
            //    {
            //        if (hostel == "")
            //        {
            //            hostel = "" + cbl_hostelname.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            hostel = hostel + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}

            //string build = d2.GetBuildingCode_inv(hostel);
            //char[] delimiterChars = { ',' };
            //string[] build1 = build.Split(delimiterChars);
            //string build2 = "";

            //foreach (string b in build1)
            //{
            //    if (build2 == "")
            //    {
            //        build2 = "" + b + "";
            //    }
            //    else
            //    {
            //        build2 = build2 + "'" + "," + "'" + b + "";
            //    }
            //}

            //ds1.Clear();
            //string floor = "select code,Building_Name from Building_Master where code in ('" + build2 + "')";
            //ds1 = d2.select_method_wo_parameter(floor, "Text");
            //string w = "";
            //if (ds1.Tables[0].Rows.Count > 0)
            //{
            //    string q1 = Convert.ToString(ds1.Tables[0].Rows[0][1]);
            //    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            //    {
            //        string q = Convert.ToString(ds1.Tables[0].Rows[i][1]);
            //        if (w == "")
            //        {
            //            w = "" + q + "";
            //        }
            //        else
            //        {
            //            w = w + "'" + "," + "'" + q + "";
            //        }
            //    }
            //}
            ds.Clear();
           // ds = d2.BindFloor_new(w);
            string itemname = "select * from Floor_Master f,Building_Master b where b.Building_Name=f.Building_Name and b.code  ='" + Convert.ToString(drbbuilding.SelectedValue) + "'";
           
            ds = d2.select_method_wo_parameter(itemname, "Text");
            ddl_floorname.Items.Clear();

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "FloorPK";
                cbl_floorname.DataBind();


                ddl_floorname.DataSource = ds;
                ddl_floorname.DataTextField = "Floor_Name";
                ddl_floorname.DataValueField = "FloorPK";
                ddl_floorname.DataBind();


                if (cbl_floorname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_floorname.Items.Count; i++)
                    {

                        cbl_floorname.Items[i].Selected = true;
                    }

                    txt_floorname.Text = "Floor Name(" + cbl_floorname.Items.Count + ")";
                }
            }
            else
            {
                ddl_floorname.Items.Insert(0, "Select");
                txt_floorname.Text = "--Select--";
            }
            bindroom();
        }
        catch (Exception ex)
        {
        }
    }

    public void bindroom()
    {
        try
        {
            string floor = "";

            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    if (floor == "")
                    {
                        floor = "" + cbl_floorname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        floor = floor + "'" + "," + "'" + cbl_floorname.Items[i].Value.ToString() + "";
                    }
                }
            }
            cbl_room.Items.Clear();
            txt_room.Text = "---Select---";
            cb_room.Checked = false;
            string query = "";
            query = "select Room_Name,Roompk from Floor_Master f,Building_Master b,Room_Detail r where b.Building_Name=f.Building_Name and  r.Floor_Name=f.Floor_Name and r.Building_Name=f.Building_Name and f.Floorpk in('" + floor + "') and b.Code='" + Convert.ToString(drbbuilding.SelectedValue) + "' order by Roompk";
          //  query = "select distinct rd.Roompk,rd.Room_Name from Room_Detail rd,Floor_Master hd where rd.Floor_Name=hd.Floor_Name and hd.FloorPK in('" + floor + "')  order by Roompk";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_room.DataSource = ds;
                cbl_room.DataTextField = "Room_Name";
                cbl_room.DataValueField = "Roompk";
                cbl_room.DataBind();

                if (cbl_room.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_room.Items.Count; row++)
                    {
                        cbl_room.Items[row].Selected = true;
                    }
                    txt_room.Text = "Room (" + cbl_room.Items.Count + ")";
                    cb_room.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_room_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_room.Text = "--Select--";
            if (cb_room.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_room.Items.Count; i++)
                {
                    cbl_room.Items[i].Selected = true;
                }
                txt_room.Text = "Room (" + (cbl_room.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_room.Items.Count; i++)
                {
                    cbl_room.Items[i].Selected = false;
                }
                txt_room.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_room_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_room.Checked = false;
        int commcount = 0;

        txt_room.Text = "--Select--";

        for (int i = 0; i < cbl_room.Items.Count; i++)
        {
            if (cbl_room.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_room.Items.Count)
            {
                cb_room.Checked = true;
            }
            txt_room.Text = "Room (" + commcount.ToString() + ")";
        }
    }
    protected void imgbtn_presentclick(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();

        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            imgbtnclear_presentclick(sender, e);
            int startcol = Convert.ToInt32(ViewState["Columnheadercount"]) + 1;

            if (cbboth.Checked == true)
            {
                if (cb_shwtim.Checked == true)
                {
                    for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 4)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                        if (s == "P")
                        {
                            for (i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                        if (s == "P")
                        {
                            for (i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                            }
                        }
                    }
                }
            }
            if (cbeve.Checked == true)
            {
                if (cb_shwtim.Checked == true)
                {
                    for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 4)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                        if (s == "P")
                        {
                            for (i = 2; i < FpSpread1.Sheets[0].RowCount; i += 2)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (cbboth.Checked == true)
                    {
                        for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                            if (s == "P")
                            {
                                for (i = 2; i < FpSpread1.Sheets[0].RowCount; i += 2)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                            if (s == "P")
                            {
                                for (i = 1; i < FpSpread1.Sheets[0].RowCount; i ++)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                                }
                            }
                        }
                    }
                }
            }
            if (cbmor.Checked == true)
            {
                if (cb_shwtim.Checked == true)
                {
                    for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 4)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                        if (s == "P")
                        {
                            for (i = 1; i < FpSpread1.Sheets[0].RowCount; i += 2)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (cbboth.Checked == true)
                    {
                        for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                            if (s == "P")
                            {
                                for (i = 1; i < FpSpread1.Sheets[0].RowCount; i += 2)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                            if (s == "P")
                            {
                                for (i = 1; i < FpSpread1.Sheets[0].RowCount; i ++)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    protected void imgbtnclear_presentclick(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges(); int startcol = Convert.ToInt32(ViewState["Columnheadercount"]) + 1;
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            if (cb_shwtim.Checked == true)
            {
                for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 4)
                {
                    string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                    if (s == "P")
                    {
                        for (i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {

                            FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                            FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                            FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                        }
                    }
                }
            }
            else
            {
                if (cbboth.Checked == true)
                {
                    for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                        if (s == "P")
                        {
                            for (i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col - 1].Text);
                        if (s == "P")
                        {
                            for (i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    protected void imgbtn_abstclick(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            imgbtnclear_presentclick(sender, e);
            int startcol = Convert.ToInt32(ViewState["Columnheadercount"]) + 1;
            if (cbmor.Checked == true)
            {
                if (cb_shwtim.Checked == true)
                {
                    for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 4)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                        if (s == "A")
                        {
                            for (i = 1; i < FpSpread1.Sheets[0].RowCount; i += 2)
                            {
                                FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (cbboth.Checked == true)
                    {
                        for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                            if (s == "A")
                            {
                                for (i = 1; i < FpSpread1.Sheets[0].RowCount; i += 2)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                            if (s == "A")
                            {
                                for (i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                                }
                            }
                        }
                    }
                }
            }
            if (cbeve.Checked == true)
            {
                if (cb_shwtim.Checked == true)
                {
                    for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 4)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                        if (s == "A")
                        {
                            for (i = 2; i < FpSpread1.Sheets[0].RowCount; i += 2)
                            {
                                FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (cbboth.Checked == true)
                    {
                        for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                            if (s == "A")
                            {
                                for (i = 2; i < FpSpread1.Sheets[0].RowCount; i += 2)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                            if (s == "A")
                            {
                                for (i = 1; i < FpSpread1.Sheets[0].RowCount; i ++)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                                }
                            }
                        }
                    }
                }
            }
            if (cbboth.Checked == true)
            {
                if (cb_shwtim.Checked == true)
                {
                    for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 4)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                        if (s == "A")
                        {
                            for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                        if (s == "A")
                        {
                            for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                            }
                        }
                    }
                }
            }
        }
    }
    protected void imgbtnclear_abstclick(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            int startcol = Convert.ToInt32(ViewState["Columnheadercount"]) + 1;
            if (cb_shwtim.Checked == true)
            {
                for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                {
                    string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                    if (s == "A")
                    {
                        for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                        {

                            FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                            FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                            FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;

                        }
                    }
                }
            }
            else
            {
                for (int col = startcol; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                {
                    string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                    if (s == "A")
                    {
                        for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                            FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                            FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                        }
                    }
                }
            }
        }
    }

    protected void chk_od_Checkedchange(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges(); imgbtnclear_presentclick(sender, e);
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            int startcol = Convert.ToInt32(ViewState["Columnheadercount"]) + 1;
            if (cbmor.Checked == true)
            {
                if (cb_shwtim.Checked == true)
                {
                    for (int col = startcol + 1; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                        if (s == "OD")
                        {
                            for (i = 1; i < FpSpread1.Sheets[0].RowCount; i += 2)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                FpSpread1.Sheets[0].Cells[i, col - 2].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (cbboth.Checked == true)
                    {
                        for (int col = startcol + 1; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                            if (s == "OD")
                            {
                                for (i = 1; i < FpSpread1.Sheets[0].RowCount; i += 2)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col - 2].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int col = startcol + 1; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                            if (s == "OD")
                            {
                                for (i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col - 2].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                }
                            }
                        }
                    }
                }
            }
            if (cbeve.Checked == true)
            {
                if (cb_shwtim.Checked == true)
                {
                    for (int col = startcol + 1; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                        if (s == "OD")
                        {
                            for (i = 2; i < FpSpread1.Sheets[0].RowCount; i += 2)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (cbboth.Checked == true)
                    {

                        for (int col = startcol + 1; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                            if (s == "OD")
                            {
                                for (i = 2; i < FpSpread1.Sheets[0].RowCount; i += 2)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col - 2].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 1;

                                }
                            }
                        }
                    }
                    else
                    {
                        for (int col = startcol + 1; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                        {
                            string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                            if (s == "OD")
                            {
                                for (i = 1; i < FpSpread1.Sheets[0].RowCount; i ++)
                                {
                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col - 2].Value = 0;
                                    FpSpread1.Sheets[0].Cells[i, col].Value = 1;

                                }
                            }
                        }
                    }

                }
            }
            if (cbboth.Checked == true)
            {
                if (cb_shwtim.Checked == true)
                {
                    for (int col = startcol + 1; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                        if (s == "OD")
                        {
                            for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;

                            }
                        }
                    }
                }
                else
                {

                    for (int col = startcol + 1; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                    {
                        string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                        if (s == "OD")
                        {
                            for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                            {
                                FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col - 2].Value = 0;
                                FpSpread1.Sheets[0].Cells[i, col].Value = 1;

                            }
                        }
                    }
                }
            }
        }
    }

    protected void chk_odclear_Checkedchange(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges(); imgbtnclear_presentclick(sender, e);
        if (FpSpread1.Sheets[0].RowCount > 0)
        {
            int startcol = Convert.ToInt32(ViewState["Columnheadercount"]) + 1;
            if (cb_shwtim.Checked == true)
            {
                for (int col = startcol + 1; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                {
                    string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                    if (s == "OD")
                    {
                        for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                            FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                            FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;

                        }
                    }
                }
            }
            else
            {

                for (int col = startcol + 1; col < FpSpread1.Sheets[0].ColumnCount; col += 2)
                {
                    string s = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text);
                    if (s == "OD")
                    {
                        for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                            FpSpread1.Sheets[0].Cells[i, col - 2].Value = 0;
                            FpSpread1.Sheets[0].Cells[i, col].Value = 0;

                        }
                    }
                }
            }
        }
    }

    protected void btn_update_Click(object sender, EventArgs e)
    {

    }
    protected void btn_reset_Click(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        imgbtnclear_abstclick(sender, e);
        imgbtnclear_presentclick(sender, e);
        chk_odclear_Checkedchange(sender, e);
    }
    protected void FpSpread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "DisplayLoadingDiv();", true);
            string r = FpSpread1.Sheets[0].ActiveRow.ToString();
            string j = FpSpread1.Sheets[0].ActiveColumn.ToString();
            int k = Convert.ToInt32(j);
            int a = Convert.ToInt32(r);
            int b = Convert.ToInt32(j);
            if (r.Trim() != "")
            {
                if (Convert.ToInt32(r) == 0)
                {
                    if (r.Trim() != "" && j.Trim() != "")
                    {
                        if (FpSpread1.Sheets[0].RowCount > 0)
                        {
                            int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[a, b].Value);
                            if (checkval == 0)
                            {
                                string headervalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, b].Text);
                                int m = 0;
                                if (cbmor.Checked == true)
                                    m = 1;
                                if (cbeve.Checked == true)
                                    m = 1;
                                   // m = 2;
                                if (cbeve.Checked == true || cbmor.Checked == true)
                                {
                                    for (int i = m; i < FpSpread1.Sheets[0].RowCount; i ++)
                                    {
                                        if (headervalue.Trim() == "P")
                                        {
                                            FpSpread1.Sheets[0].Cells[i, b].Value = 1;
                                            FpSpread1.Sheets[0].Cells[i, b + 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[i, b + 2].Value = 0;

                                            FpSpread1.Sheets[0].Cells[a, b + 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[a, b + 2].Value = 0;
                                        }
                                        if (headervalue.Trim() == "A")
                                        {
                                            FpSpread1.Sheets[0].Cells[i, b].Value = 1;
                                            FpSpread1.Sheets[0].Cells[i, b - 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[i, b + 1].Value = 0;

                                            FpSpread1.Sheets[0].Cells[a, b - 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[a, b + 1].Value = 0;

                                        }
                                        if (headervalue.Trim() == "OD")
                                        {
                                            FpSpread1.Sheets[0].Cells[i, b].Value = 1;
                                            FpSpread1.Sheets[0].Cells[i, b - 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[i, b - 2].Value = 0;

                                            FpSpread1.Sheets[0].Cells[a, b - 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[a, b - 2].Value = 0;
                                        }
                                    }
                                }
                                if (cbboth.Checked == true)
                                {
                                    for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                                    {
                                        if (headervalue.Trim() == "P")
                                        {
                                            FpSpread1.Sheets[0].Cells[i, b].Value = 1;
                                            FpSpread1.Sheets[0].Cells[i, b + 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[i, b + 2].Value = 0;

                                            FpSpread1.Sheets[0].Cells[a, b + 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[a, b + 2].Value = 0;
                                        }
                                        if (headervalue.Trim() == "A")
                                        {
                                            FpSpread1.Sheets[0].Cells[i, b].Value = 1;
                                            FpSpread1.Sheets[0].Cells[i, b - 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[i, b + 1].Value = 0;

                                            FpSpread1.Sheets[0].Cells[a, b - 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[a, b + 1].Value = 0;

                                        }
                                        if (headervalue.Trim() == "OD")
                                        {
                                            FpSpread1.Sheets[0].Cells[i, b].Value = 1;
                                            FpSpread1.Sheets[0].Cells[i, b - 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[i, b - 2].Value = 0;

                                            FpSpread1.Sheets[0].Cells[a, b - 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[a, b - 2].Value = 0;
                                        }
                                    }
                                }
                            }
                            if (checkval == 1)
                            {
                                for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                                {
                                    FpSpread1.Sheets[0].Cells[i, b].Value = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    string headervalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, Convert.ToInt32(j)].Text);

                    if (headervalue.Trim() == "P")
                    {
                        FpSpread1.Sheets[0].Cells[a, b + 1].Value = 0;
                        FpSpread1.Sheets[0].Cells[a, b + 2].Value = 0;
                    }
                    if (headervalue.Trim() == "A")
                    {
                        FpSpread1.Sheets[0].Cells[a, b - 1].Value = 0;
                        FpSpread1.Sheets[0].Cells[a, b + 1].Value = 0;

                    }

                    if (headervalue.Trim() == "OD")
                    {
                        FpSpread1.Sheets[0].Cells[a, b - 1].Value = 0;
                        FpSpread1.Sheets[0].Cells[a, b - 2].Value = 0;
                    }
                }
            }
        }
        catch
        {

        }
        finally
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "HideLoadingDiv();", false);
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            //FpSpread1.SaveChanges();
            string date = "";
            string hostel = "";
            string room = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (hostel == "")
                    {
                        hostel = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostel = hostel + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }


            string batch = string.Empty;
            for (int i = 0; i < Cblbatch.Items.Count; i++)
            {
                if (Cblbatch.Items[i].Selected == true)
                {
                    if (hostel == "")
                    {
                        batch = "" + Cblbatch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        batch = batch + "'" + "," + "'" + Cblbatch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string floor = "";
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    if (floor == "")
                    {
                        floor = "" + cbl_floorname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        floor = floor + "'" + "," + "'" + cbl_floorname.Items[i].Value.ToString() + "";
                    }
                }
            }

           // floor = Convert.ToString(ddl_floorname.SelectedItem.Value);

            for (int i = 0; i < cbl_room.Items.Count; i++)
            {
                if (cbl_room.Items[i].Selected == true)
                {
                    if (room == "")
                    {
                        room = "" + cbl_room.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        room = room + "'" + "," + "'" + cbl_room.Items[i].Value.ToString() + "";
                    }
                }
            }

            string fromdate = Convert.ToString(txt_fromdate.Text);
            DateTime dt = new DateTime();
            string[] split = fromdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string todate = Convert.ToString(txt_todate.Text);
            DateTime dt1 = new DateTime();
            string[] split1 = todate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            string build = Convert.ToString(drbbuilding.SelectedValue);

            string hoidaydate = "select CONVERT(varchar(10),  HolidayDate,103) as HolidayDate from HT_Holidays  where HolidayType =1  and HolidayDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'and HolidayForDayscholar='1' and HolidayForHostler ='1' and HolidayForStaff ='1'";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(hoidaydate, "Text");
            ArrayList newarray = new ArrayList();

            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    newarray.Add(Convert.ToString(ds1.Tables[0].Rows[i]["HolidayDate"]));
                }
            }
            if (hostel.Trim() != "" && floor.Trim() != "" && room.Trim() != "" && batch.Trim() != "" && build!="")
            {
                string q = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,hs.id,hs.HostelRegistrationPK,hs.HostelMasterFK,Dt.Dept_Name,C.Course_Name ,r.Current_Semester,r.Sections,(select b.Building_Name from Building_Master b where Code=hs.BuildingFK) as Building_Name,(select f.Floor_Name from Floor_Master f where f.FloorPK=hs.FloorFK) as Floor_Name,(select r.Room_Name from Room_Detail r where r.Roompk=hs.RoomFK) as Room_Name,h.HostelName as Hostel_Name from HM_HostelMaster h,HT_HostelRegistration hs,Registration r,Degree d,Department dt,Course c  where h.HostelMasterPK =hs.HostelMasterFK and hs.APP_No  =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and ISNULL(IsSuspend,'0')=0  and ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0 and h.HostelMasterPK in('" + hostel + "') and FloorFK in ('" + floor + "') and RoomFK in('" + room + "') and r.Batch_Year in ('" + batch + "')  ";//order by r.batch_year desc, r.degree_code asc,r.roll_no asc,hs.roomfk asc and r.Batch_Year in ('" + Convert.ToString(ddlBatchyear.SelectedItem.Value) + "')
                if (rdbrollno.Checked == true)
                   q = q + " order by r.roll_no asc";
                else
                    q = q + " order by hs.RoomFK asc";
                ds = d2.select_method_wo_parameter(q, "Text");
                lbl_errorsearch1.Text = "No of Students :" + ds.Tables[0].Rows.Count.ToString();

                string current = DateTime.Now.ToString("dd/MM/yyyy");

                string[] split2 = current.Split('/');
                DateTime dt3 = Convert.ToDateTime(split2[1] + "/" + split2[0] + "/" + split2[2]);

                if (dt <= dt1 && dt1 <= dt3)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 1;
                        FpSpread1.Sheets[0].AutoPostBack = false;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        FpSpread1.Columns[0].Width = 50;
                        FpSpread1.Columns[0].Locked = true;

                        Hashtable columnhash = new Hashtable();
                        columnhash.Add("Roll_No", "Roll No");
                        columnhash.Add("Reg_No", "Reg No");
                        columnhash.Add("Stud_Name", "Student Name");
                        columnhash.Add("Stud_Type", "Student Type");
                        //columnhash.Add("Session", "Session");
                        columnhash.Add("Course_Name", "Degree");
                        columnhash.Add("Dept_Name", "Department");
                        columnhash.Add("Current_Semester", "Semester");
                        columnhash.Add("Sections", "Section");
                        columnhash.Add("Hostel_Name", "Hostel Name");
                        columnhash.Add("Building_Name", "Building Name");
                        columnhash.Add("Floor_Name", "Floor Name");
                        columnhash.Add("Room_Name", "Room");
                        columnhash.Add("id", "Student Id");

                        if (ItemList.Count != 0)
                        {
                            FpSpread1.Sheets[0].SpanModel.Add(0, 0, 1, ItemList.Count);
                        }
                        else if (ItemList.Count == 0)
                        {
                            ItemList.Add("Roll_No");
                            ItemList.Add("Reg_No");
                            ItemList.Add("Stud_Name");
                            ItemList.Add("Stud_Type");
                            ItemList.Add("id");
                        }
                        for (int ks = 0; ks < ItemList.Count; ks++)
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        }
                        for (int jk = 0; jk < ds.Tables[0].Columns.Count; jk++)
                        {
                            string colno = Convert.ToString(ds.Tables[0].Columns[jk]);
                            if (ItemList.Contains(Convert.ToString(colno)))
                            {
                                int index = ItemList.IndexOf(Convert.ToString(colno));
                                //FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Text = Convert.ToString(columnhash[colno]);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, index + 1].HorizontalAlign = HorizontalAlign.Center;
                                // FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                            }
                        }
                        //02.05.16
                        if (cbboth.Checked == true)
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Session";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                           
                        }
                        ViewState["Columnheadercount"] = FpSpread1.Sheets[0].ColumnCount;
                        while (dt <= dt1)
                        {
                            if (!newarray.Contains(dt.ToString("dd/MM/yyyy")))
                            {
                                checkdate = true;

                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dt.ToString("dd/MM/yyyy"));
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "P";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "A";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "OD";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 3, 1, 3);

                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Time";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                                if (cb_shwtim.Checked == true)
                                {
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                                    FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                                }
                            }
                            else
                            {
                                if (date == "")
                                {
                                    date = "" + Convert.ToString(dt.ToString("dd/MM/yyyy")) + "";
                                }
                                else
                                {
                                    date = date + "," + Convert.ToString(dt.ToString("dd/MM/yyyy")) + "";
                                }
                            }
                            dt = dt.AddDays(1);
                        }
                        if (checkdate == true)
                        {

                            FpSpread1.Sheets[0].RowCount++;
                            if (txt_fromdate.Text.Trim() != "" && txt_todate.Text.Trim() != "")//&& ddl_hostelname.Text.Trim() != "Select"
                            {
                                FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                    // FpSpread1.Sheets[0].SpanModel.Add(1, 0, 2, 1);
                                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                    {
                                        if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                                        {
                                            int index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                            FpSpread1.Sheets[0].Columns[index + 1].Width = 150;
                                            FpSpread1.Sheets[0].Columns[index + 1].Locked = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, index + 1].CellType = txtreg;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, index + 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, index + 1].Font.Size = FontUnit.Medium;
                                        }
                                        string Hostel_Code1 = Convert.ToString(ds.Tables[0].Columns[j]);
                                        string Hostel_Code = "HostelMasterFK";
                                        if (Hostel_Code1 == Hostel_Code)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["HostelMasterFK"]);
                                        }
                                        if (Convert.ToString(ds.Tables[0].Columns["Roll_No"]).Trim() == "Roll_No")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]).Trim();
                                        }
                                    }
                                    if(cbboth.Checked==true)
                                    FpSpread1.Sheets[0].RowCount++;
                                }

                                FarPoint.Web.Spread.CheckBoxCellType chkdate = new FarPoint.Web.Spread.CheckBoxCellType();
                                chkdate.AutoPostBack = true;
                                chkdate.Text = " ";
                                FarPoint.Web.Spread.CheckBoxCellType chkdate1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                chkdate1.AutoPostBack = true;
                                chkdate1.Text = " ";

                                chkdate1.Text = " ";
                                string[] spiltfrom;
                                string Attendance = "";
                                string rollno = "";
                                string insertquery = "";
                                string columngetvalue = "";
                                string AttndDayvalue = "";
                                string AttnEven = "";
                                string attnmonth = "";
                                string attnyear = "";
                                string attnday = ""; string mornA = ""; string evenA = "";
                                ViewState["colcountnewvalue"] = ItemList.Count + 3; int k = 0; int cun = 0;
                                if (cbboth.Checked == true)
                                    cun = ItemList.Count + 3;
                                else
                                    cun = ItemList.Count + 3 - 1;
                                for (int col = cun; col < FpSpread1.Sheets[0].ColumnCount; col += 4)
                                {
                                    FpSpread1.Sheets[0].Cells[0, col - 1].CellType = chkdate1;
                                    FpSpread1.Sheets[0].Cells[0, col - 1].HorizontalAlign = HorizontalAlign.Center;

                                    FpSpread1.Sheets[0].Cells[0, (col - 1) + 1].CellType = chkdate1;
                                    FpSpread1.Sheets[0].Cells[0, (col - 1) + 1].HorizontalAlign = HorizontalAlign.Center;

                                    FpSpread1.Sheets[0].Cells[0, (col - 1) + 2].CellType = chkdate1;
                                    FpSpread1.Sheets[0].Cells[0, (col - 1) + 2].HorizontalAlign = HorizontalAlign.Center;

                                    columngetvalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col - 1].Text);
                                    spiltfrom = columngetvalue.Split('/');
                                    AttndDayvalue = Convert.ToString(spiltfrom[0]);
                                    AttndDayvalue = AttndDayvalue.TrimStart('0');
                                    attnday = AttndDayvalue;
                                    AttndDayvalue = "[D" + AttndDayvalue + "]";
                                    AttnEven = "[D" + attnday + "E]";

                                    mornA = "D" + attnday;
                                    evenA = "D" + attnday + "E";
                                    attnmonth = spiltfrom[1];
                                    attnmonth = attnmonth.TrimStart('0');
                                    attnyear = spiltfrom[2];

                                    for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                                    {
                                        FpSpread1.Sheets[0].Cells[i, col - 1].CellType = chkdate;
                                        FpSpread1.Sheets[0].Cells[i, col - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Columns[col - 1].Width = 30;

                                        FpSpread1.Sheets[0].Cells[i, (col - 1) + 1].CellType = chkdate;
                                        FpSpread1.Sheets[0].Cells[i, (col - 1) + 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Columns[(col - 1) + 1].Width = 30;

                                        FpSpread1.Sheets[0].Cells[i, (col - 1) + 2].CellType = chkdate;
                                        FpSpread1.Sheets[0].Cells[i, (col - 1) + 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Columns[(col - 1) + 2].Width = 30;

                                        rollno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                        if (rollno.Trim() != "")
                                        {
                                            string app_no = d2.GetFunction("select App_No from Registration where Roll_No='" + rollno.Trim() + "'");// and college_code='" + collegecode1 + "'
                                            string getvalue = "select " + AttndDayvalue + "," + AttnEven + " from HT_Attendance where App_No ='" + app_no.Trim() + "' and AttnMonth='" + attnmonth + "' and AttnYear='" + attnyear + "'";
                                            ds.Clear();
                                            ds = d2.select_method_wo_parameter(getvalue, "Text"); string dayvalue = "";
                                            string evenvalue = "";
                                            if (ds.Tables[0].Rows.Count > 0)//if (getvalue != "" && getvalue != "0")
                                            {
                                                dayvalue = Convert.ToString(ds.Tables[0].Rows[0][mornA]);
                                                evenvalue = Convert.ToString(ds.Tables[0].Rows[0][evenA]);
                                                if (cbboth.Checked == true)
                                                {
                                                    if (dayvalue.Trim() == "1")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                                    }
                                                    else if (dayvalue.Trim() == "2")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                                    }
                                                    else if (dayvalue.Trim() == "3")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i, col + 1].Value = 1;
                                                    }

                                                    if (evenvalue.Trim() == "1")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i + 1, col - 1].Value = 1;
                                                    }
                                                    else if (evenvalue.Trim() == "2")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i + 1, col].Value = 1;
                                                    }
                                                    else if (evenvalue.Trim() == "3")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i + 1, col + 1].Value = 1;
                                                    }
                                                }
                                                else if (cbmor.Checked == true)
                                                {
                                                    if (dayvalue.Trim() == "1")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                                    }
                                                    else if (dayvalue.Trim() == "2")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                                    }
                                                    else if (dayvalue.Trim() == "3")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i, col + 1].Value = 1;
                                                    }

                                                }
                                                else
                                                {
                                                    if (evenvalue.Trim() == "1")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i, col - 1].Value = 1;
                                                    }
                                                    else if (evenvalue.Trim() == "2")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i, col].Value = 1;
                                                    }
                                                    else if (evenvalue.Trim() == "3")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[i, col + 1].Value = 1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (cbboth.Checked == true)
                                                {
                                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                                    FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                                                    FpSpread1.Sheets[0].Cells[i, col].Value = 0;

                                                    FpSpread1.Sheets[0].Cells[i + 1, col - 1].Value = 0;
                                                    FpSpread1.Sheets[0].Cells[i + 1, col].Value = 0;
                                                    FpSpread1.Sheets[0].Cells[i + 1, col + 1].Value = 0;
                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].Cells[i, col - 1].Value = 0;
                                                    FpSpread1.Sheets[0].Cells[i, col + 1].Value = 0;
                                                    FpSpread1.Sheets[0].Cells[i, col].Value = 0;
                                                }
                                            }
                                        }
                                        if (cbboth.Checked == true)
                                        {
                                            for (int c = 0; c < ItemList.Count + 1; c++)
                                            {
                                                FpSpread1.Sheets[0].SpanModel.Add(i, c, 2, 1);
                                            }
                                        }
                                    }
                                }

                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                // lbl_errorsearch1.Text = "No of Students :" + FpSpread1.Sheets[0].RowCount.ToString();
                                lbl_errorsearch1.Visible = true;
                                FpSpread1.Sheets[0].FrozenRowCount = 1;
                                FpSpread1.Sheets[0].FrozenColumnCount = 4;
                                FpSpread1.SaveChanges();
                                FpSpread1.Visible = true;
                                btn_update.Visible = false;
                                btn_reset.Visible = true;
                                rptprint.Visible = true;
                                dat.Visible = true;
                                btn_save.Visible = true;
                                pheaderfilter.Visible = true;
                                pcolumnorder.Visible = true;
                                if (date != "")
                                {
                                    lbl_error.Visible = true;
                                    lbl_error.Text = date + "---Holiday";
                                }
                                else
                                {
                                    lbl_error.Visible = false;
                                }
                                for (int col = ItemList.Count; col < ItemList.Count + 1; col++)
                                {
                                    for (int i = 1; i < (Convert.ToDouble(FpSpread1.Sheets[0].RowCount) / 2); i++)
                                    {
                                        if (cbboth.Checked == true)
                                        {
                                            k++;

                                            FpSpread1.Sheets[0].Cells[k, col + 1].Text = "Morning";
                                            k++;
                                            FpSpread1.Sheets[0].Cells[k, col + 1].Text = "Evening";
                                        }

                                    }
                                }
                            }
                            else
                            {
                                fpspreadvisiblefalse();
                                rptprint.Visible = false;
                                pcolumnorder.Visible = false;
                                lbl_errorsearch1.Visible = false;
                                lbl_error.Visible = true;
                                lbl_error.Text = "Please Select All Field";
                            }
                        }
                        else
                        {
                            btn_save.Visible = false;
                            btn_update.Visible = false;
                            btn_reset.Visible = false;
                            dat.Visible = false;
                            rptprint.Visible = false;
                            lbl_error.Visible = true;
                            pheaderfilter.Visible = false;
                            pcolumnorder.Visible = false;
                            lbl_errorsearch1.Visible = false;
                            lbl_error.Text = "Selected Date Is Holiday";
                        }
                    }
                    else
                    {
                        fpspreadvisiblefalse();
                        rptprint.Visible = false;
                        lbl_errorsearch1.Visible = false;
                        pcolumnorder.Visible = false;
                        lbl_errorsearch1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Found";
                    }
                }
            }
            else
            {
                fpspreadvisiblefalse();
                rptprint.Visible = false;
                lbl_errorsearch1.Visible = false;
                pcolumnorder.Visible = false;
                lbl_errorsearch1.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All Field";
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }
    public void cb_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string si = "";
            int j = 0;
            if (cb_column.Checked == true)
            {
                ItemList.Clear();
                for (i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                for (i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    tborder.Text = tborder.Text + ItemList[i].ToString();
                    tborder.Text = tborder.Text + "(" + (j).ToString() + ")  ";
                }
            }
            else
            {
                for (i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    cblcolumnorder.Items[0].Enabled = false;
                }
                tborder.Text = "";
                tborder.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void lb_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            cb_column.Checked = false;
            lnk_columnorder.Visible = false;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_columnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index;
            string value = "";
            string result = "";
            string sindex = "";
            cb_column.Checked = false;
            cblcolumnorder.Items[0].Selected = true;
          //  cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            for (i = 0; i < ItemList.Count; i++)
            {
                tborder.Text = tborder.Text + ItemList[i].ToString();

                tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";

            }
            if (ItemList.Count == 22)
            {
                cb_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddl_floorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindroom();
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbatch()
    {
        try
        {
            ddlBatchyear.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {

                Cblbatch.DataSource = ds;
                Cblbatch.DataTextField = "batch_year";
                Cblbatch.DataValueField = "batch_year";
                Cblbatch.DataBind();
                ddlBatchyear.DataSource = ds;
                ddlBatchyear.DataTextField = "batch_year";
                ddlBatchyear.DataValueField = "batch_year";
                ddlBatchyear.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void ddlBatchyear_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void Chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkbatch.Checked == true)
        {
            for (int i = 0; i < Cblbatch.Items.Count; i++)
            {
                Cblbatch.Items[i].Selected = true;
            }
            Txtbatch.Text = "Batch(" + (Cblbatch.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < Cblbatch.Items.Count; i++)
            {
                Cblbatch.Items[i].Selected = false;
            }
          
        }
      

    }
    protected void Cblbatch_SelectIndexChange(object sender, EventArgs e)
    {
        Txtbatch.Text = "--Select--";
        Chkbatch.Checked = false;
        int commcount = 0;
        for (int i = 0; i < Cblbatch.Items.Count; i++)
        {
            if (Cblbatch.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            Txtbatch.Text = "Batch(" + commcount.ToString() + ")";
            if (commcount == Cblbatch.Items.Count)
            {
                Chkbatch.Checked = true;
            }
        }
      
    }
    protected void drbbuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindfloor();
        }
        catch
        {
        }
    }
}