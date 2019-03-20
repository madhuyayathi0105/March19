using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.IO;
using System.Globalization;
using System.Configuration;


public partial class SMS_ManagerSettings : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode1 = string.Empty;
    string usercode = string.Empty;
    string groupcode = string.Empty;
    string singleuser = string.Empty;
    Hashtable hat = new Hashtable();
    Dictionary<string, string> dictlev = new Dictionary<string, string>();
    Dictionary<string, string> dictstftyp = new Dictionary<string, string>();
    Dictionary<string, string> dictstfcat = new Dictionary<string, string>();
    int i = 0;
    int inscount = 0;

    int sendcount = 0;
    int sendsms = 0;
    string collname = "";
    string user_id = "";
    int Month = DateTime.Now.Month;
    int day = DateTime.Now.Day;
    ReuasableMethods rs = new ReuasableMethods();

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
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            groupcode = Session["group_code"].ToString();

            if (!IsPostBack)
            {

                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
                txtsenddt.Text = DateTime.Now.ToString("dd/MM/yyyy")
                    ;
                txtfrmdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtsenddt.Attributes.Add("readonly", "readonly");
                txtfrmdt.Attributes.Add("readonly", "readonly");
                txttodt.Attributes.Add("readonly", "readonly");
                ddlmin.Items.Clear();
                for (i = 0; i <= 59; i++)
                {
                    if (i == 0)
                    {
                        ddlmin.Items.Add("00");
                    }
                    else
                    {
                        if (Convert.ToString(i).Length == 1)
                        {
                            ddlmin.Items.Add("0" + Convert.ToString(i));
                        }
                        else
                        {
                            ddlmin.Items.Add(Convert.ToString(i));
                        }
                    }
                }

                ddltype.SelectedIndex = 0;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { }
    }

    public void bindcollege()
    {
        try
        {
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
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch { }
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        if (rdb_Sms.Checked)
        {
            ddltype.SelectedIndex = 0;
            btnaddtime.Text = "Add";
            ddltype_change(sender, e);
        }
        else
        {
            ddltype1.SelectedIndex = 0;
            btnaddtime.Text = "Add";
            ddltype1_change(sender, e);

        }
    }

    private DataSet getRecord(string smspurpose, string collcode)
    {
        ds.Clear();
        try
        {
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,Hourdaywise,Send_Period_Session from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + collcode + "'";
            ds = d2.select_method_wo_parameter(selq, "Text");
        }
        catch { }
        return ds;
    }

    protected void ddltype_change(object sender, EventArgs e)
    {
        chksendsms.Visible = true;
        tblemail.Visible = false;
        chkstudent.Enabled = false;
        chkstaff.Enabled = false;
        chkhod.Enabled = false;
        chkhigheroff.Enabled = false;
        chkstudent.Checked = false;
        chkstaff.Checked = false;
        chkhod.Checked = false;
        chkhigheroff.Checked = false;
        chkstudwish.Checked = false;
        chkfatwish.Checked = false;
        chkmotwish.Checked = false;
        ddlhr.SelectedIndex = 0;
        ddlmin.SelectedIndex = 0;
        ddlmer.SelectedIndex = 0;
        ddlsession.SelectedIndex = 0;
        txtsenddt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        //chksendsms.Checked = false;
        tblrow3.Visible = false;
        fldgrphos.Visible = false;
        tblrow4.Visible = false;
        tblrow5.Visible = false;
        txtdaysRemind.Text = "";
        chkinclongabs.Visible = false;
        lblgreater.Visible = false;
        txtgreater.Visible = false;
        txtsenddt.Enabled = false;
        chkgrphos.Checked = false;
        chkinclongabs.Checked = false;
        lblsendwish.Visible = false;
        fldsendwish.Visible = false;
        txtmobilno.Enabled = false;
        lbltestname.Visible = false;
        ddltest.Visible = false;
        lblsession.Visible = false;
        ddlsession.Visible = false;
        fldwithlev.Visible = false;
        divsellst.Visible = false;
        lbldevname.Visible = false;
        upddevname.Visible = false;
        lblhostelname.Visible = false;
        updhosname.Visible = false;
        btnaddtime.Visible = false;
        tblaltrow1.Visible = false;
        tblaltrow2.Visible = false;
        fldfrmtodt.Visible = false;
        fldstftypcat.Visible = false;
        lblcount.Visible = false;
        fldcount.Visible = false;
        tbldays.Visible = false;//added by saranya(31.10.2017)
        tblOld.Visible = true;
        tblMobileno.Visible = true;
        tblSms.Visible = false;
        tdldate.Visible = true;
        tdltime.Visible = true;
        txtsenddt.Visible = true;

        txtgreater.Text = "";
        txtmobilno.Text = "";
        string sendtime = "";
        trstdhrday.Visible = false;
        if (ddltype.SelectedItem.Text == "Receipt Cancel")
        {

            send.Visible = false;
            fldresource.Visible = false;
            chksendsms.Visible = false;
            tdldate.Visible = false;
            tdltime.Visible = false;
            tblemail.Visible = true;
            txtmobilno.Enabled = true;
            txtsendmail.Enabled = true;
            txtsendmail.Text = "";

        }
        else
        {
        }

        //SMS
        #region BirthDay

        if (ddltype.SelectedItem.Value == "0")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "420px");
            chkstudent.Enabled = true;
            chkstaff.Enabled = true;
            chkhod.Enabled = true;
            chkhigheroff.Enabled = true;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["Stud_Wish"]).Trim().ToUpper() == "TRUE")
                {
                    chkstudent.Checked = true;
                    fldsendwish.Visible = true;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["SendToStudMob"]).Trim().ToUpper() == "TRUE")
                        chkstudwish.Checked = true;
                    else
                        chkstudwish.Checked = false;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["SendToFatMob"]).Trim().ToUpper() == "TRUE")
                        chkfatwish.Checked = true;
                    else
                        chkfatwish.Checked = false;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["SendToMotMob"]).Trim().ToUpper() == "TRUE")
                        chkmotwish.Checked = true;
                    else
                        chkmotwish.Checked = false;
                }
                else
                {
                    chkstudent.Checked = false;
                    fldsendwish.Visible = false;
                    chkstudwish.Checked = false;
                    chkfatwish.Checked = false;
                    chkmotwish.Checked = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["Staff_Wish"]).Trim().ToUpper() == "TRUE")
                    chkstaff.Checked = true;
                else
                    chkstaff.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")//added by saranyadevi 1.2.2018
                {
                    chkhigheroff.Checked = true;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOD"]).Trim().ToUpper() == "TRUE")
                {
                    chkhod.Checked = true;
                    fldcount.Visible = true;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["Student_Count"]).Trim().ToUpper() == "TRUE")
                        chkstudcount.Checked = true;
                    else
                        chkstudcount.Checked = false;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["Staff_Count"]).Trim().ToUpper() == "TRUE")
                        chkstafcount.Checked = true;
                    else
                        chkstafcount.Checked = false;

                }
                else
                {
                    chkhod.Checked = false;
                    fldcount.Visible = false;
                    chkstudcount.Checked = false;
                    chkstafcount.Checked = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region Student Attendance

        if (ddltype.SelectedItem.Value == "1")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "490px");
            chkhod.Enabled = true;
            chkhigheroff.Enabled = true;
            tblrow3.Visible = true;
            fldgrphos.Visible = true;
            tblrow4.Visible = true;
            chkinclongabs.Visible = true;
            lblgreater.Visible = true;
            txtgreater.Visible = true;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOD"]).Trim().ToUpper() == "TRUE")
                    chkhod.Checked = true;
                else
                    chkhod.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsStudAttnGroup"]).Trim().ToUpper() == "TRUE")
                    chkgrphos.Checked = true;
                else
                    chkgrphos.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["IncludeLongAbs"]).Trim().ToUpper() == "TRUE")
                {
                    tblrow4.Visible = true;
                    chkinclongabs.Checked = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["IncludeLongAbsCount"])))
                        txtgreater.Text = Convert.ToString(ds.Tables[0].Rows[0]["IncludeLongAbsCount"]);
                    else
                        txtgreater.Text = "";
                }
                else
                {
                    tblrow4.Visible = false;
                    chkinclongabs.Checked = false;
                    txtgreater.Text = "";
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region CAM Marks

        if (ddltype.SelectedItem.Value == "2")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            txtsenddt.Enabled = true;
            lbltestname.Visible = true;
            ddltest.Visible = true;
            bindtestname();
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Test_Name"])))
                    ddltest.SelectedIndex = ddltest.Items.IndexOf(ddltest.Items.FindByText(Convert.ToString(ds.Tables[0].Rows[0]["Test_Name"])));
                else
                    ddltest.SelectedIndex = 0;
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Send_Date"])))
                    txtsenddt.Text = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["Send_Date"])).ToString("dd/MM/yyyy");
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region Staff Attendance

        if (ddltype.SelectedItem.Value == "3")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "440px");
            chkhigheroff.Enabled = true;
            chkstaff.Enabled = true;
            lblsession.Visible = true;
            ddlsession.Visible = true;
            bindnewstafftype();
            newcategory();
            int stfcount = 0;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToStaff"]).Trim().ToUpper() == "TRUE")
                    chkstaff.Checked = true;
                else
                    chkstaff.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    fldwithlev.Visible = true;
                    chkwithlev.Enabled = true;
                    chkgrpby.Enabled = true;
                    chkhigheroff.Checked = true;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    fldwithlev.Visible = false;
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                    ddlsession.SelectedIndex = 1;
                else
                    ddlsession.SelectedIndex = 0;
                if (Convert.ToString(ds.Tables[0].Rows[0]["StfAttnRepWithLeave"]).Trim().ToUpper() == "TRUE")
                    chkwithlev.Checked = true;
                else
                    chkwithlev.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["StfAttnRepGroupStfType"]).Trim().ToUpper() == "TRUE")
                {
                    chkgrpby.Checked = true;
                    divstftyp.Visible = true;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupType"]) == "1")
                    {
                        rdb_stfcat.Checked = false;
                        rdb_stftype.Checked = true;
                    }
                    else
                    {
                        rdb_stfcat.Checked = true;
                        rdb_stftype.Checked = false;
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["StfAttnIsGroupList"]).Trim().ToUpper() == "TRUE")
                    {
                        divsellst.Visible = true;
                        chksellst.Checked = true;
                        if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupList"])))
                        {
                            if (rdb_stftype.Checked == true)
                            {
                                updstftyp.Visible = true;
                                updstfcat.Visible = false;
                                string[] splstftype = Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupList"]).Split(',');
                                if (splstftype.Length > 0)
                                {
                                    for (int ik = 0; ik < cblstftype.Items.Count; ik++)
                                    {
                                        for (int jk = 0; jk < splstftype.Length; jk++)
                                        {
                                            if (cblstftype.Items[ik].Text == splstftype[jk])
                                            {
                                                cblstftype.Items[ik].Selected = true;
                                                stfcount++;
                                            }
                                        }
                                    }
                                    if (cblstftype.Items.Count == stfcount)
                                    {
                                        txtstftype.Text = "StaffType(" + stfcount + ")";
                                        cbstftype.Checked = true;
                                    }
                                    else
                                    {
                                        txtstftype.Text = "StaffType(" + stfcount + ")";
                                        cbstftype.Checked = false;
                                    }
                                }
                            }
                            if (rdb_stfcat.Checked == true)
                            {
                                updstftyp.Visible = false;
                                updstfcat.Visible = true;
                                string[] splstftype = Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupList"]).Split(',');
                                if (splstftype.Length > 0)
                                {
                                    for (int ik = 0; ik < cblstfcat.Items.Count; ik++)
                                    {
                                        for (int jk = 0; jk < splstftype.Length; jk++)
                                        {
                                            if (cblstfcat.Items[ik].Text == splstftype[jk])
                                            {
                                                cblstfcat.Items[ik].Selected = true;
                                            }
                                        }
                                    }
                                    if (cblstfcat.Items.Count == stfcount)
                                    {
                                        txtstfcat.Text = "Category(" + stfcount + ")";
                                        cbstfcat.Checked = true;
                                    }
                                    else
                                    {
                                        txtstfcat.Text = "Category(" + stfcount + ")";
                                        cbstfcat.Checked = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        divsellst.Visible = false;
                        chksellst.Checked = false;
                        updstftyp.Visible = false;
                        updstfcat.Visible = false;
                    }
                }
                else
                {
                    chkgrpby.Checked = false;
                    divstftyp.Visible = false;
                    rdb_stfcat.Checked = false;
                    rdb_stftype.Checked = false;
                    divsellst.Visible = false;
                    chksellst.Checked = false;
                    updstftyp.Visible = false;
                    updstfcat.Visible = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region Block Box

        if (ddltype.SelectedItem.Value == "4")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            chkhod.Enabled = true;
            //bindnewstafftype();
            //newcategory();
            //int stfcount = 0;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOD"]).Trim().ToUpper() == "TRUE")
                    chkhod.Checked = true;
                else
                    chkhod.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    //chkgrpby.Checked = true;
                    //divstftyp.Visible = true;
                    //fldwithlev.Visible = true;
                    chkhigheroff.Checked = true;
                    //chkwithlev.Checked = false;
                    //chkgrpby.Checked = false;
                    //chkwithlev.Enabled = false;
                    //chkgrpby.Enabled = false;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    //chkgrpby.Checked = false;
                    //divstftyp.Visible = false;
                    //fldwithlev.Visible = false;
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }

                //if (Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupType"]) == "1")
                //{
                //    rdb_stfcat.Checked = false;
                //    rdb_stftype.Checked = true;
                //}
                //else
                //{
                //    rdb_stfcat.Checked = true;
                //    rdb_stftype.Checked = false;
                //}
                //if (Convert.ToString(ds.Tables[0].Rows[0]["StfAttnIsGroupList"]).Trim().ToUpper() == "TRUE")
                //{
                //    divsellst.Visible = true;
                //    chksellst.Checked = true;
                //    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupList"])))
                //    {
                //        if (rdb_stftype.Checked == true)
                //        {
                //            updstftyp.Visible = true;
                //            updstfcat.Visible = false;
                //            string[] splstftype = Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupList"]).Split(',');
                //            if (splstftype.Length > 0)
                //            {
                //                for (int ik = 0; ik < cblstftype.Items.Count; ik++)
                //                {
                //                    for (int jk = 0; jk < splstftype.Length; jk++)
                //                    {
                //                        if (cblstftype.Items[ik].Text == splstftype[jk])
                //                        {
                //                            cblstftype.Items[ik].Selected = true;
                //                            stfcount++;
                //                        }
                //                    }
                //                }
                //                if (cblstftype.Items.Count == stfcount)
                //                {
                //                    txtstftype.Text = "StaffType(" + stfcount + ")";
                //                    cbstftype.Checked = true;
                //                }
                //                else
                //                {
                //                    txtstftype.Text = "StaffType(" + stfcount + ")";
                //                    cbstftype.Checked = false;
                //                }
                //            }
                //        }
                //        if (rdb_stfcat.Checked == true)
                //        {
                //            updstftyp.Visible = false;
                //            updstfcat.Visible = true;
                //            string[] splstftype = Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupList"]).Split(',');
                //            if (splstftype.Length > 0)
                //            {
                //                for (int ik = 0; ik < cblstfcat.Items.Count; ik++)
                //                {
                //                    for (int jk = 0; jk < splstftype.Length; jk++)
                //                    {
                //                        if (cblstfcat.Items[ik].Text == splstftype[jk])
                //                        {
                //                            cblstfcat.Items[ik].Selected = true;
                //                        }
                //                    }
                //                }
                //                if (cblstfcat.Items.Count == stfcount)
                //                {
                //                    txtstfcat.Text = "Category(" + stfcount + ")";
                //                    cbstfcat.Checked = true;
                //                }
                //                else
                //                {
                //                    txtstfcat.Text = "Category(" + stfcount + ")";
                //                    cbstfcat.Checked = false;
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    divsellst.Visible = false;
                //    chksellst.Checked = false;
                //    updstftyp.Visible = false;
                //    updstfcat.Visible = false;
                //}
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region Student Attendance Shortage

        if (ddltype.SelectedItem.Value == "5")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            txtsenddt.Enabled = true;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Send_Date"])))
                    txtsenddt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Send_Date"]).ToString("dd/MM/yyyy");
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region Hostel Student Attendance

        if (ddltype.SelectedItem.Value == "6")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = true;
            ddlsession.Visible = true;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                    ddlsession.SelectedIndex = 1;
                else
                    ddlsession.SelectedIndex = 0;
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region Student/Staff Cummulative Attendace

        if (ddltype.SelectedItem.Value == "7")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = true;
            ddlsession.Visible = true;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                    ddlsession.SelectedIndex = 1;
                else
                    ddlsession.SelectedIndex = 0;
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region Automatic Download Attendace

        if (ddltype.SelectedItem.Value == "8")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "585px");
            chkstudent.Enabled = true;
            chkstaff.Enabled = true;
            btnaddtime.Visible = true;
            tblaltrow1.Visible = false;
            tblaltrow2.Visible = false;
            int selcount = 0;
            bindalttime();
            binddevname();
            studHostelbind();
            binddept();
            bindshift();
            bindstafftypedown();
            bindstaffcatdown();
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txtfrmdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                string senttostudent = Convert.ToString(ds.Tables[0].Rows[0]["SendToStud"]);
                string senttostaff = Convert.ToString(ds.Tables[0].Rows[0]["SendToStaff"]);
                string devname = "";
                string hosname = "";
                string[] splmydt = new string[5];

                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }

                TimeSpan time1 = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["alternate_time1"])).TimeOfDay;
                string getTime1 = Convert.ToString(time1);
                TimeSpan time2 = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["alternate_time2"])).TimeOfDay;
                string getTime2 = Convert.ToString(time2);
                string myTime1 = "";
                string myTime2 = "";
                if (getTime1.Trim() == "00:00:00" && getTime2.Trim() == "00:00:00")
                {
                    btnaddtime.Text = "Add";
                    ddlmin1.SelectedIndex = 0;
                    ddlmer1.SelectedIndex = 0;
                    ddlhr1.SelectedIndex = 0;
                    ddlhr2.SelectedIndex = 0;
                    ddlmin2.SelectedIndex = 0;
                    ddlmer2.SelectedIndex = 0;
                }
                else
                {
                    btnaddtime.Text = "Hide";
                    tblaltrow1.Visible = true;
                    tblaltrow2.Visible = true;
                    myTime1 = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["alternate_time1"])).ToString("hh:mm tt");
                    myTime2 = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["alternate_time2"])).ToString("hh:mm tt");
                    splmydt = myTime1.Split(' ');
                    if (splmydt.Length > 0)
                    {
                        ddlhr1.SelectedIndex = ddlhr1.Items.IndexOf(ddlhr1.Items.FindByText(Convert.ToString(splmydt[0].Split(':')[0])));
                        ddlmin1.SelectedIndex = ddlmin1.Items.IndexOf(ddlmin1.Items.FindByText(Convert.ToString(splmydt[0].Split(':')[1])));
                        ddlmer1.SelectedIndex = ddlmer1.Items.IndexOf(ddlmer1.Items.FindByText(Convert.ToString(splmydt[1])));
                    }
                    splmydt = myTime2.Split(' ');
                    if (splmydt.Length > 0)
                    {
                        ddlhr2.SelectedIndex = ddlhr2.Items.IndexOf(ddlhr2.Items.FindByText(Convert.ToString(splmydt[0].Split(':')[0])));
                        ddlmin2.SelectedIndex = ddlmin2.Items.IndexOf(ddlmin2.Items.FindByText(Convert.ToString(splmydt[0].Split(':')[1])));
                        ddlmer2.SelectedIndex = ddlmer2.Items.IndexOf(ddlmer2.Items.FindByText(Convert.ToString(splmydt[1])));
                    }
                }
                if (senttostudent.Trim().ToUpper() == "TRUE" || senttostaff.Trim().ToUpper() == "TRUE")
                {
                    lbldevname.Visible = true;
                    upddevname.Visible = true;
                    tblrow3.Visible = true;
                    fldfrmtodt.Visible = true;
                    devname = Convert.ToString(ds.Tables[0].Rows[0]["DeviceID"]);
                    if (cbldevname.Items.Count > 0 && devname.Trim() != "")
                    {
                        string[] spldev = devname.Split(',');
                        if (spldev.Length > 0)
                        {
                            for (int my = 0; my < cbldevname.Items.Count; my++)
                            {
                                for (int ny = 0; ny < spldev.Length; ny++)
                                {
                                    if (cbldevname.Items[my].Value == spldev[ny])
                                    {
                                        cbldevname.Items[my].Selected = true;
                                        selcount++;
                                    }
                                }
                            }
                            if (selcount == cbldevname.Items.Count)
                            {
                                txtdevname.Text = "DeviceName(" + selcount + ")";
                                cbdevname.Checked = true;
                            }
                            else
                            {
                                txtdevname.Text = "DeviceName(" + selcount + ")";
                                cbdevname.Checked = false;
                            }
                        }
                    }
                    else
                    {
                        txtdevname.Text = "--Select--";
                        cbdevname.Checked = false;
                    }
                    txtfrmdt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["From_Date"]).ToString("dd/MM/yyyy");
                    txttodt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["To_Date"]).ToString("dd/MM/yyyy");
                    if (Convert.ToString(ds.Tables[0].Rows[0]["MorAbs"]).Trim().ToUpper() == "TRUE")
                        chkmorabs.Checked = true;
                    else
                        chkmorabs.Checked = false;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["EveAbs"]).Trim().ToUpper() == "TRUE")
                        chkeveabs.Checked = true;
                    else
                        chkeveabs.Checked = false;
                }
                else
                {
                    lbldevname.Visible = false;
                    upddevname.Visible = false;
                    tblrow3.Visible = false;
                    fldfrmtodt.Visible = false;
                }
                if (senttostudent.Trim().ToUpper() == "TRUE")
                {
                    chkstudent.Checked = true;
                    lblhostelname.Visible = true;
                    updhosname.Visible = true;
                    hosname = Convert.ToString(ds.Tables[0].Rows[0]["hostelmasterpk"]);
                    if (cblhosname.Items.Count > 0 && hosname.Trim() != "")
                    {
                        string[] spldev = hosname.Split(',');
                        if (spldev.Length > 0)
                        {
                            for (int my = 0; my < cblhosname.Items.Count; my++)
                            {
                                for (int ny = 0; ny < spldev.Length; ny++)
                                {
                                    if (cblhosname.Items[my].Value == spldev[ny])
                                    {
                                        cblhosname.Items[my].Selected = true;
                                        selcount++;
                                    }
                                }
                            }
                            if (selcount == cblhosname.Items.Count)
                            {
                                txthosname.Text = "HostelName(" + selcount + ")";
                                cbhosname.Checked = true;
                            }
                            else
                            {
                                txthosname.Text = "HostelName(" + selcount + ")";
                                cbhosname.Checked = false;
                            }
                        }
                    }
                    else
                    {
                        txthosname.Text = "--Select--";
                        cbhosname.Checked = false;
                    }
                }
                if (senttostaff.Trim().ToUpper() == "TRUE")
                {
                    chkstaff.Checked = true;
                    tblrow4.Visible = true;
                    fldstftypcat.Visible = true;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupType"]) == "1")
                    {
                        ddlstftypcat.SelectedIndex = 0;
                        ddlstftypedown.Visible = true;
                        ddlstfcatdown.Visible = false;
                        if (ddlstftypedown.SelectedItem.Text != "Select" && ddlstftypedown.Items.Count > 1)
                            ddlstftypedown.SelectedIndex = ddlstftypedown.Items.IndexOf(ddlstftypedown.Items.FindByText(Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupList"])));
                        else
                            ddlstftypedown.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlstftypcat.SelectedIndex = 1;
                        ddlstfcatdown.Visible = true;
                        ddlstftypedown.Visible = false;
                        if (ddlstfcatdown.SelectedItem.Text != "Select" && ddlstfcatdown.Items.Count > 1)
                            ddlstfcatdown.SelectedIndex = ddlstfcatdown.Items.IndexOf(ddlstfcatdown.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupList"])));
                        else
                            ddlstfcatdown.SelectedIndex = 0;
                    }
                    if (ddldeptdown.SelectedItem.Text != "Select" && ddldeptdown.Items.Count > 1)
                        ddldeptdown.SelectedIndex = ddldeptdown.Items.IndexOf(ddldeptdown.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["dept_code"])));
                    else
                        ddldeptdown.SelectedIndex = 0;
                    if (ddlshiftdown.SelectedItem.Text != "Select" && ddlshiftdown.Items.Count > 1)
                        ddlshiftdown.SelectedIndex = ddlshiftdown.Items.IndexOf(ddlshiftdown.Items.FindByText(Convert.ToString(ds.Tables[0].Rows[0]["shift"])));
                    else
                        ddlshiftdown.SelectedIndex = 0;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region Finance Settings

        if (ddltype.SelectedItem.Value == "9")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "385px");
            txtsenddt.Enabled = true;
            chkhigheroff.Enabled = true;
            chkhod.Enabled = true;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOD"]).Trim().ToUpper() == "TRUE")
                    chkhod.Checked = true;
                else
                    chkhod.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Send_Date"])))
                    txtsenddt.Text = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["Send_Date"])).ToString("dd/MM/yyyy");
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region Driving License Renewal Settings

        if (ddltype.SelectedItem.Value == "10")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            chkstaff.Enabled = true;
            tblrow5.Visible = true;
            txtdaysRemind.Text = "";
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToStaff"]).Trim().ToUpper() == "TRUE")
                    chkstaff.Checked = true;
                else
                    chkstaff.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Days_Remind"])))
                    txtdaysRemind.Text = Convert.ToString(ds.Tables[0].Rows[0]["Days_Remind"]);
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region FC Reminder Settings

        if (ddltype.SelectedItem.Value == "11")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            tblrow5.Visible = true;
            txtdaysRemind.Text = "";
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Days_Remind"])))
                    txtdaysRemind.Text = Convert.ToString(ds.Tables[0].Rows[0]["Days_Remind"]);
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        #region Insurance Renewal Settings

        if (ddltype.SelectedItem.Value == "12")
        {
            send.Visible = true;
            fldresource.Visible = true;
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            tblrow5.Visible = true;
            txtdaysRemind.Text = "";
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtmobilno.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"])))
                        txtmobilno.Text = Convert.ToString(ds.Tables[0].Rows[0]["MobileNos"]);
                    else
                        txtmobilno.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["Days_Remind"])))
                    txtdaysRemind.Text = Convert.ToString(ds.Tables[0].Rows[0]["Days_Remind"]);
                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion
        //added by saranyadevi(31.10.2017)
        #region Student Cumulative Attendance

        if (ddltype.SelectedItem.Value == "13")
        {
            mainfld.Style.Add("Height", "420px");
            tbldays.Visible = true;
            tblOld.Visible = false;
            tblMobileno.Visible = false;
            tblSms.Visible = true;
            tdldate.Visible = false;
            tdltime.Visible = true;
            txtsenddt.Visible = false;

            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToStud"]).Trim().ToUpper() == "TRUE")
                {
                    chkstudsms.Checked = true;

                    if (Convert.ToString(ds.Tables[0].Rows[0]["SendToStudMob"]).Trim().ToUpper() == "TRUE")
                        chkstudsms.Checked = true;
                    else
                        chkstudsms.Checked = false;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["SendToFatMob"]).Trim().ToUpper() == "TRUE")
                        chkfatsms.Checked = true;
                    else
                        chkfatsms.Checked = false;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["SendToMotMob"]).Trim().ToUpper() == "TRUE")
                        chkmotsms.Checked = true;
                    else
                        chkmotsms.Checked = false;
                }
                else
                {
                    chkstudsms.Checked = false;
                    fldsendwish.Visible = false;
                    chkstudsms.Checked = false;
                    chkfatsms.Checked = false;
                    chkmotsms.Checked = false;
                }


                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }

        #endregion

        //added by saranyadevi(11.12.2018)
        #region Student Due Date sms
        if (ddltype.SelectedItem.Value == "15")
        {
            send.Visible = false;
            fldresource.Visible = false;
            mainfld.Style.Add("Height", "420px");
            chkstudent.Enabled = true;
            chkstaff.Enabled = false;
            chkhod.Enabled = false;
            chkhigheroff.Enabled = false;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToStudMob"]).Trim().ToUpper() == "TRUE")
                    chkstudent.Checked = true;
                else
                {
                    chkstudent.Checked = false;
                    fldsendwish.Visible = false;
                    chkstudwish.Checked = false;
                    chkfatwish.Checked = false;
                    chkmotwish.Checked = false;
                }

                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }
        #endregion


        //added by saranyadevi(13.12.2018)
        #region Student Home Work
        if (ddltype.SelectedItem.Value == "16")
        {
            send.Visible = false;
            fldresource.Visible = false;
            mainfld.Style.Add("Height", "420px");
            chkstudent.Enabled = true;
            chkstaff.Enabled = false;
            chkhod.Enabled = false;
            chkhigheroff.Enabled = false;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToStudMob"]).Trim().ToUpper() == "TRUE")
                    chkstudent.Checked = true;
                else
                {
                    chkstudent.Checked = false;
                    fldsendwish.Visible = false;
                    chkstudwish.Checked = false;
                    chkfatwish.Checked = false;
                    chkmotwish.Checked = false;
                }

                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;
            }
        }
        #endregion


        //added by saranyadevi(17.12.2018)
        #region Student Absent
        if (ddltype.SelectedItem.Value == "17")
        {
            cbl_hour.Items.Clear();
            loadperiods();
            trstdhrday.Visible = true;
            tbldays.Visible = false;
            tblOld.Visible = false;
            send.Visible = true;
            tblMobileno.Visible = false;
            fldresource.Visible = true;
            tblSms.Visible = true;
            tdldate.Visible = true;
            tdltime.Visible = true;
            txtsenddt.Enabled = false;
            Checkhrdaysend.Visible = true;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }


                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToStudMob"]).Trim().ToUpper() == "TRUE")
                    chkstudsms.Checked = true;
                else
                    chkstudsms.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToFatMob"]).Trim().ToUpper() == "TRUE")
                    chkfatsms.Checked = true;
                else
                    chkfatsms.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToMotMob"]).Trim().ToUpper() == "TRUE")
                    chkmotsms.Checked = true;
                else
                    chkmotsms.Checked = false;



                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    Checkhrdaysend.Checked = true;
                else
                    Checkhrdaysend.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["Hourdaywise"]).Trim().ToUpper() == "0")
                {
                    rdohour.Checked = true;
                    hourwise.Visible = true;
                    daywise.Visible = false;
                    if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Period_Session"]).Trim().ToUpper() != "")
                    {
                        string hourday = Convert.ToString(ds.Tables[0].Rows[0]["Send_Period_Session"]).Trim().ToUpper();
                        string[] spilt = hourday.Split('&');
                        if (spilt.Length > 0)
                        {
                            int ccount1 = 0;
                            for (int hr = 0; hr < spilt.Length; hr++)
                            {
                                string hour = spilt[hr];
                                for (int sel = 0; sel < cbl_hour.Items.Count; sel++)
                                {
                                    if (hour == cbl_hour.Items[sel].Text)
                                    {
                                        cbl_hour.Items[sel].Selected = true;
                                        ccount1++;
                                    }
                                }
                            }
                            txt_hour.Text = "Hour(" + ccount1.ToString() + ")";
                        }

                    }
                }
                else
                {
                    rdodaily.Checked = true;
                    hourwise.Visible = false;
                    daywise.Visible = true;
                    rbldayType.Items[0].Selected = false;
                    rbldayType.Items[1].Selected = false;
                    rbldayType.Items[2].Selected = false;
                    string day = Convert.ToString(ds.Tables[0].Rows[0]["Send_Period_Session"]).Trim().ToUpper();

                    if (day != "")
                    {
                        if (day == "0")
                            rbldayType.Items[0].Selected = true;

                        if (day == "1")
                            rbldayType.Items[1].Selected = true;

                        if (day == "2")
                            rbldayType.Items[2].Selected = true;

                    }
                }
            }
        }
        #endregion


        //added by saranyadevi(17.12.2018)
        #region Hostel Student Absent
        if (ddltype.SelectedItem.Value == "18")
        {
            cbl_hour.Items.Clear();

            tblMobileno.Visible = true;
            txtmobilno.Enabled = false;
            tdldate.Visible = false;
            tdltime.Visible = false;
            trstdhrday.Visible = false;
            tbldays.Visible = false;
            tblOld.Visible = false;
            send.Visible = true;

            fldresource.Visible = true;
            tblSms.Visible = true;

            txtsenddt.Enabled = false;
            chksendsms.Visible = true;
            ds = getRecord(Convert.ToString(ddltype.SelectedItem.Text), Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToStudMob"]).Trim().ToUpper() == "TRUE")
                    chkstudsms.Checked = true;
                else
                    chkstudsms.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToFatMob"]).Trim().ToUpper() == "TRUE")
                    chkfatsms.Checked = true;
                else
                    chkfatsms.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToMotMob"]).Trim().ToUpper() == "TRUE")
                    chkmotsms.Checked = true;
                else
                    chkmotsms.Checked = false;



                if (Convert.ToString(ds.Tables[0].Rows[0]["IsSend"]).Trim().ToUpper() == "TRUE")
                    chksendsms.Checked = true;
                else
                    chksendsms.Checked = false;

            }
        }
        #endregion


    }



    protected void ddltype1_change(object sender, EventArgs e)
    {
        chksendsms.Visible = true;
        tblemail.Visible = false;
        chkstudent.Enabled = false;
        chkstaff.Enabled = false;
        chkhod.Enabled = false;
        chkhigheroff.Enabled = false;
        chkstudent.Checked = false;
        chkstaff.Checked = false;
        chkhod.Checked = false;
        chkhigheroff.Checked = false;
        chkstudwish.Checked = false;
        chkfatwish.Checked = false;
        chkmotwish.Checked = false;
        ddlhr.SelectedIndex = 0;
        ddlmin.SelectedIndex = 0;
        ddlmer.SelectedIndex = 0;
        ddlsession.SelectedIndex = 0;
        txtsenddt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        chksendsms.Checked = false;
        tblrow3.Visible = false;
        fldgrphos.Visible = false;
        tblrow4.Visible = false;
        tblrow5.Visible = false;
        txtdaysRemind.Text = "";
        chkinclongabs.Visible = false;
        lblgreater.Visible = false;
        txtgreater.Visible = false;
        txtsenddt.Enabled = false;
        chkgrphos.Checked = false;
        chkinclongabs.Checked = false;
        lblsendwish.Visible = false;
        fldsendwish.Visible = false;
        txtmobilno.Enabled = false;
        lbltestname.Visible = false;
        ddltest.Visible = false;
        lblsession.Visible = false;
        ddlsession.Visible = false;
        fldwithlev.Visible = false;
        divsellst.Visible = false;
        lbldevname.Visible = false;
        upddevname.Visible = false;
        lblhostelname.Visible = false;
        updhosname.Visible = false;
        btnaddtime.Visible = false;
        tblaltrow1.Visible = false;
        tblaltrow2.Visible = false;
        fldfrmtodt.Visible = false;
        fldstftypcat.Visible = false;
        lblcount.Visible = false;
        fldcount.Visible = false;
        tbldays.Visible = false;//added by saranya(31.10.2017)
        tblOld.Visible = true;
        tblMobileno.Visible = true;
        tblSms.Visible = false;
        tdldate.Visible = true;
        tdltime.Visible = true;
        txtsenddt.Visible = true;

        txtgreater.Text = "";
        txtmobilno.Text = "";
        string sendtime = "";
        //added by saranyadevi(25.07.2018)
        #region Hostel Student AbsentList

        if (ddltype1.SelectedItem.Value == "0")
        {
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = true;
            ddlsession.Visible = true;
            send.Visible = true;
            fldresource.Visible = true;
            chksendsms.Visible = false;
            tdldate.Visible = true;
            tdltime.Visible = true;
            tblemail.Visible = true;
            tblMobileno.Visible = false;
            txtsendmail.Enabled = false;
            txtsendmail.Text = "";
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,EmailId from Automatic_SMS where sms_purpose='" + Convert.ToString(ddltype1.SelectedItem.Text) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtsendmail.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["EmailId"])))
                        txtsendmail.Text = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    else
                        txtsendmail.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                    ddlsession.SelectedIndex = 1;
                else
                    ddlsession.SelectedIndex = 0;
            }
        }

        #endregion

        #region Attendance Summary Hostel Wise

        if (ddltype1.SelectedItem.Value == "1")
        {
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = true;
            ddlsession.Visible = true;
            send.Visible = true;
            fldresource.Visible = true;
            chksendsms.Visible = false;
            tdldate.Visible = true;
            tdltime.Visible = true;
            tblemail.Visible = true;
            tblMobileno.Visible = false;
            txtsendmail.Enabled = false;
            txtsendmail.Text = "";
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,EmailId from Automatic_SMS where sms_purpose='" + Convert.ToString(ddltype1.SelectedItem.Text) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtsendmail.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["EmailId"])))
                        txtsendmail.Text = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    else
                        txtsendmail.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                    ddlsession.SelectedIndex = 1;
                else
                    ddlsession.SelectedIndex = 0;
            }
        }

        #endregion

        #region Item Stock Report



        if (ddltype1.SelectedItem.Value == "2")
        {
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = false;
            ddlsession.Visible = false;
            send.Visible = true;
            fldresource.Visible = true;
            chksendsms.Visible = false;
            tdldate.Visible = true;
            tdltime.Visible = true;
            tblemail.Visible = true;
            tblMobileno.Visible = false;
            txtsendmail.Enabled = false;
            txtsendmail.Text = "";
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,EmailId from Automatic_SMS where sms_purpose='" + Convert.ToString(ddltype1.SelectedItem.Text) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtsendmail.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["EmailId"])))
                        txtsendmail.Text = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    else
                        txtsendmail.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                //if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                //    ddlsession.SelectedIndex = 1;
                //else
                //    ddlsession.SelectedIndex = 0;
            }
        }


        #endregion

        #region Black Box Report



        if (ddltype1.SelectedItem.Value == "3")
        {
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = false;
            ddlsession.Visible = false;
            send.Visible = true;
            fldresource.Visible = true;
            chksendsms.Visible = false;
            tdldate.Visible = true;
            tdltime.Visible = true;
            tblemail.Visible = true;
            tblMobileno.Visible = false;
            txtsendmail.Enabled = false;
            txtsendmail.Text = "";
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,EmailId from Automatic_SMS where sms_purpose='" + Convert.ToString(ddltype1.SelectedItem.Text) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtsendmail.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["EmailId"])))
                        txtsendmail.Text = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    else
                        txtsendmail.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                //if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                //    ddlsession.SelectedIndex = 1;
                //else
                //    ddlsession.SelectedIndex = 0;
            }
        }


        #endregion

        #region Over All Attendance Report For Particular Day



        if (ddltype1.SelectedItem.Value == "4")
        {
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = false;
            ddlsession.Visible = false;
            send.Visible = true;
            fldresource.Visible = true;
            chksendsms.Visible = false;
            tdldate.Visible = true;
            tdltime.Visible = true;
            tblemail.Visible = true;
            tblMobileno.Visible = false;
            txtsendmail.Enabled = false;
            txtsendmail.Text = "";
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,EmailId from Automatic_SMS where sms_purpose='" + Convert.ToString(ddltype1.SelectedItem.Text) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtsendmail.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["EmailId"])))
                        txtsendmail.Text = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    else
                        txtsendmail.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                //if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                //    ddlsession.SelectedIndex = 1;
                //else
                //    ddlsession.SelectedIndex = 0;
            }
        }


        #endregion


        #region Absentees Report



        if (ddltype1.SelectedItem.Value == "5")
        {
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = false;
            ddlsession.Visible = false;
            send.Visible = true;
            fldresource.Visible = true;
            chksendsms.Visible = false;
            tdldate.Visible = true;
            tdltime.Visible = true;
            tblemail.Visible = true;
            tblMobileno.Visible = false;
            txtsendmail.Enabled = false;
            txtsendmail.Text = "";
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,EmailId from Automatic_SMS where sms_purpose='" + Convert.ToString(ddltype1.SelectedItem.Text) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtsendmail.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["EmailId"])))
                        txtsendmail.Text = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    else
                        txtsendmail.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                //if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                //    ddlsession.SelectedIndex = 1;
                //else
                //    ddlsession.SelectedIndex = 0;
            }
        }


        #endregion


        #region Finance BillNoWise Paid Report



        if (ddltype1.SelectedItem.Value == "6")
        {
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = false;
            ddlsession.Visible = false;
            send.Visible = true;
            fldresource.Visible = true;
            chksendsms.Visible = false;
            tdldate.Visible = true;
            tdltime.Visible = true;
            tblemail.Visible = true;
            tblMobileno.Visible = false;
            txtsendmail.Enabled = false;
            txtsendmail.Text = "";
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,EmailId from Automatic_SMS where sms_purpose='" + Convert.ToString(ddltype1.SelectedItem.Text) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtsendmail.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["EmailId"])))
                        txtsendmail.Text = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    else
                        txtsendmail.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                //if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                //    ddlsession.SelectedIndex = 1;
                //else
                //    ddlsession.SelectedIndex = 0;
            }
        }


        #endregion


        #region Finance InstitutionWise Paid Report



        if (ddltype1.SelectedItem.Value == "7")
        {
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = false;
            ddlsession.Visible = false;
            send.Visible = true;
            fldresource.Visible = true;
            chksendsms.Visible = false;
            tdldate.Visible = true;
            tdltime.Visible = true;
            tblemail.Visible = true;
            tblMobileno.Visible = false;
            txtsendmail.Enabled = false;
            txtsendmail.Text = "";
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,EmailId from Automatic_SMS where sms_purpose='" + Convert.ToString(ddltype1.SelectedItem.Text) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtsendmail.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["EmailId"])))
                        txtsendmail.Text = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    else
                        txtsendmail.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                //if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                //    ddlsession.SelectedIndex = 1;
                //else
                //    ddlsession.SelectedIndex = 0;
            }
        }


        #endregion


        #region Financial Student Paymode Collection Report



        if (ddltype1.SelectedItem.Value == "8")
        {
            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = false;
            ddlsession.Visible = false;
            send.Visible = true;
            fldresource.Visible = true;
            chksendsms.Visible = false;
            tdldate.Visible = true;
            tdltime.Visible = true;
            tblemail.Visible = true;
            tblMobileno.Visible = false;
            txtsendmail.Enabled = false;
            txtsendmail.Text = "";
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,EmailId from Automatic_SMS where sms_purpose='" + Convert.ToString(ddltype1.SelectedItem.Text) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtsendmail.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["EmailId"])))
                        txtsendmail.Text = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    else
                        txtsendmail.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                //if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                //    ddlsession.SelectedIndex = 1;
                //else
                //    ddlsession.SelectedIndex = 0;
            }
        }


        #endregion

        #region Staff Attendance Report

        if (ddltype1.SelectedItem.Value == "9")
        {

            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = true;
            ddlsession.Visible = true;
            send.Visible = true;
            fldresource.Visible = true;
            chksendsms.Visible = false;
            tdldate.Visible = true;
            tdltime.Visible = true;
            tblemail.Visible = true;
            tblMobileno.Visible = false;
            txtsendmail.Enabled = false;
            txtsendmail.Text = "";
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,EmailId from Automatic_SMS where sms_purpose='" + Convert.ToString(ddltype1.SelectedItem.Text) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtsendmail.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["EmailId"])))
                        txtsendmail.Text = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    else
                        txtsendmail.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                    ddlsession.SelectedIndex = 1;
                else
                    ddlsession.SelectedIndex = 0;
            }

        }



        #endregion

        #region Department Wise Attendance Report

        if (ddltype1.SelectedItem.Value == "10")
        {

            mainfld.Style.Add("Height", "385px");
            chkhigheroff.Enabled = true;
            lblsession.Visible = true;
            ddlsession.Visible = true;
            send.Visible = true;
            fldresource.Visible = true;
            chksendsms.Visible = false;
            tdldate.Visible = true;
            tdltime.Visible = true;
            tblemail.Visible = true;
            tblMobileno.Visible = false;
            txtsendmail.Enabled = false;
            txtsendmail.Text = "";
            string selq = "select sending_Time,IsSend,Convert(varchar(10),Send_Date,101) as Send_Date,Stud_Wish,Staff_Wish,SendToStudMob,SendToFatMob,SendToMotMob,SendToStud,SendToStaff,SendToHOD,SendToHOff,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,Test_Name,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date,Student_Count,Staff_Count,EmailId from Automatic_SMS where sms_purpose='" + Convert.ToString(ddltype1.SelectedItem.Text) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])))
                {
                    sendtime = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["sending_Time"])).ToString("hh:mm tt");
                    string[] splmer = sendtime.Split(' ');
                    ddlhr.SelectedIndex = ddlhr.Items.IndexOf(ddlhr.Items.FindByText(Convert.ToString(splmer[0].Split(':')[0])));
                    ddlmin.SelectedIndex = ddlmin.Items.IndexOf(ddlmin.Items.FindByText(Convert.ToString(splmer[0].Split(':')[1])));
                    ddlmer.SelectedIndex = ddlmer.Items.IndexOf(ddlmer.Items.FindByText(Convert.ToString(splmer[1])));
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["SendToHOff"]).Trim().ToUpper() == "TRUE")
                {
                    chkhigheroff.Checked = true;
                    txtsendmail.Enabled = true;
                    if (!String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["EmailId"])))
                        txtsendmail.Text = Convert.ToString(ds.Tables[0].Rows[0]["EmailId"]);
                    else
                        txtsendmail.Text = "";
                }
                else
                {
                    chkhigheroff.Checked = false;
                    txtmobilno.Text = "";
                    txtmobilno.Enabled = false;
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["Send_Session"]) == "1")
                    ddlsession.SelectedIndex = 1;
                else
                    ddlsession.SelectedIndex = 0;
            }

        }



        #endregion
    }

    protected void btnaddtime_click(object sender, EventArgs e)
    {
        if (btnaddtime.Text == "Add")
        {
            string[] splmydt = new string[5];
            btnaddtime.Text = "Hide";
            tblaltrow1.Visible = true;
            tblaltrow2.Visible = true;
            string getTime1 = "";
            string getTime2 = "";
            string myTime1 = "";
            string myTime2 = "";
            bindalttime();
            string alttime1 = d2.GetFunction("select alternate_time1 from automatic_sms where sms_purpose='Automatic Download And Mark Time Attendance Settings' and college_code='" + ddlcollege.SelectedValue + "'");
            string alttime2 = d2.GetFunction("select alternate_time2 from automatic_sms where sms_purpose='Automatic Download And Mark Time Attendance Settings' and college_code='" + ddlcollege.SelectedValue + "'");

            if (!String.IsNullOrEmpty(alttime1) && alttime1 != "0")
            {
                TimeSpan time1 = Convert.ToDateTime(alttime1).TimeOfDay;
                getTime1 = Convert.ToString(time1);
                myTime1 = Convert.ToDateTime(d2.GetFunction("select alternate_time1 from automatic_sms where sms_purpose='Automatic Download And Mark Time Attendance Settings' and college_code='" + ddlcollege.SelectedValue + "'")).ToString("hh:mm tt");
            }
            if (!String.IsNullOrEmpty(alttime2) && alttime2 != "0")
            {
                TimeSpan time2 = Convert.ToDateTime(alttime2).TimeOfDay;
                getTime2 = Convert.ToString(time2);
                myTime2 = Convert.ToDateTime(d2.GetFunction("select alternate_time2 from automatic_sms where sms_purpose='Automatic Download And Mark Time Attendance Settings' and college_code='" + ddlcollege.SelectedValue + "'")).ToString("hh:mm tt");
            }
            if ((getTime1.Trim() == "00:00:00" || String.IsNullOrEmpty(getTime1)) && (getTime2.Trim() == "00:00:00" || String.IsNullOrEmpty(getTime2)))
            {
                ddlmin1.SelectedIndex = 0;
                ddlmer1.SelectedIndex = 0;
                ddlhr1.SelectedIndex = 0;
                ddlhr2.SelectedIndex = 0;
                ddlmin2.SelectedIndex = 0;
                ddlmer2.SelectedIndex = 0;
            }
            else
            {
                splmydt = myTime1.Split(' ');
                if (splmydt.Length > 0)
                {
                    ddlhr1.SelectedIndex = ddlhr1.Items.IndexOf(ddlhr1.Items.FindByText(Convert.ToString(splmydt[0].Split(':')[0])));
                    ddlmin1.SelectedIndex = ddlmin1.Items.IndexOf(ddlmin1.Items.FindByText(Convert.ToString(splmydt[0].Split(':')[1])));
                    ddlmer1.SelectedIndex = ddlmer1.Items.IndexOf(ddlmer1.Items.FindByText(Convert.ToString(splmydt[1])));
                }
                splmydt = myTime2.Split(' ');
                if (splmydt.Length > 0)
                {
                    ddlhr2.SelectedIndex = ddlhr2.Items.IndexOf(ddlhr2.Items.FindByText(Convert.ToString(splmydt[0].Split(':')[0])));
                    ddlmin2.SelectedIndex = ddlmin2.Items.IndexOf(ddlmin2.Items.FindByText(Convert.ToString(splmydt[0].Split(':')[1])));
                    ddlmer2.SelectedIndex = ddlmer2.Items.IndexOf(ddlmer2.Items.FindByText(Convert.ToString(splmydt[1])));
                }
            }
        }
        else
        {
            btnaddtime.Text = "Add";
            tblaltrow1.Visible = false;
            tblaltrow2.Visible = false;
        }
    }

    private void bindalttime()
    {
        try
        {
            ddlmin1.Items.Clear();
            ddlmin2.Items.Clear();
            for (int ik = 0; ik <= 59; ik++)
            {
                if (Convert.ToString(ik).Length == 1)
                {
                    ddlmin1.Items.Add(new ListItem(Convert.ToString("0" + ik), Convert.ToString(ik)));
                    ddlmin2.Items.Add(new ListItem(Convert.ToString("0" + ik), Convert.ToString(ik)));
                }
                else
                {
                    ddlmin1.Items.Add(new ListItem(Convert.ToString(ik), Convert.ToString(ik)));
                    ddlmin2.Items.Add(new ListItem(Convert.ToString(ik), Convert.ToString(ik)));
                }
            }
        }
        catch { }
    }

    protected void ddlstftypcat_change(object sender, EventArgs e)
    {
        if (ddlstftypcat.SelectedIndex == 0)
        {
            ddlstftypedown.Visible = true;
            ddlstfcatdown.Visible = false;
            bindstafftypedown();
        }
        else
        {
            ddlstftypedown.Visible = false;
            ddlstfcatdown.Visible = true;
            bindstaffcatdown();
        }
    }

    protected void chkstudent_change(object sender, EventArgs e)
    {
        if (chkstudent.Checked && ddltype.SelectedIndex != 8)
        {
            lblsendwish.Visible = true;
            fldsendwish.Visible = true;
            lblhostelname.Visible = false;
            updhosname.Visible = false;
            unvisidev();
        }

        if (chkstudent.Checked && chkstaff.Checked && ddltype.SelectedIndex == 8)
        {
            lblhostelname.Visible = true;
            updhosname.Visible = true;
            studHostelbind();
            visidev();
        }
        if (chkstudent.Checked && chkstaff.Checked == false && ddltype.SelectedIndex == 8)
        {
            lblhostelname.Visible = true;
            updhosname.Visible = true;
            studHostelbind();
            visidev();
        }
        if (chkstudent.Checked == false && chkstaff.Checked && ddltype.SelectedIndex == 8)
        {
            lblhostelname.Visible = false;
            updhosname.Visible = false;
            visidev();
        }
        if (chkstudent.Checked == false && chkstaff.Checked == false && ddltype.SelectedIndex != 8)
        {
            lblsendwish.Visible = false;
            fldsendwish.Visible = false;
            lblhostelname.Visible = false;
            updhosname.Visible = false;
        }
        if ((chkstudent.Checked == true && ddltype.SelectedIndex == 15) || (chkstudent.Checked == true && ddltype.SelectedIndex == 16))
        {
            lblsendwish.Visible = false;
            fldsendwish.Visible = false;

        }

    }

    protected void chkstaff_change(object sender, EventArgs e)
    {
        if (chkstaff.Checked && chkstudent.Checked == false && ddltype.SelectedIndex == 8)
        {
            tblrow3.Visible = true;
            fldfrmtodt.Visible = true;
            tblrow4.Visible = true;
            fldstftypcat.Visible = true;
            ddlstftypedown.Visible = true;
            bindstafftypedown();
            binddept();
            bindshift();
            visidev();
        }
        if (chkstaff.Checked == false && chkstudent.Checked && ddltype.SelectedIndex == 8)
        {
            tblrow3.Visible = true;
            fldfrmtodt.Visible = true;
            tblrow4.Visible = false;
            fldstftypcat.Visible = false;
            visidev();
        }
        if (chkstaff.Checked && chkstudent.Checked && ddltype.SelectedIndex == 8)
        {
            tblrow3.Visible = true;
            fldfrmtodt.Visible = true;
            tblrow4.Visible = true;
            fldstftypcat.Visible = true;
            ddlstftypedown.Visible = true;
            bindstafftypedown();
            binddept();
            bindshift();
            visidev();
        }
        if ((chkstaff.Checked == false && chkstudent.Checked == false) || ddltype.SelectedIndex != 8)
        {
            tblrow3.Visible = false;
            fldfrmtodt.Visible = false;
            tblrow4.Visible = false;
            fldstftypcat.Visible = false;
            unvisidev();
        }


    }

    private void visidev()
    {
        lbldevname.Visible = true;
        upddevname.Visible = true;
        binddevname();
    }

    private void unvisidev()
    {
        lbldevname.Visible = false;
        upddevname.Visible = false;
    }

    protected void chkhodoff_Change(object sender, EventArgs e)//added by saranyadevi 30.1.2018
    {
        txtmobilno.Text = "";
        fldwithlev.Visible = false;
        txtmobilno.Enabled = false;
        chkwithlev.Checked = false;
        chkgrpby.Checked = false;
        chkgrpby.Enabled = false;
        chkwithlev.Enabled = false;
        divstftyp.Visible = false;
        divsellst.Visible = false;
        rdb_stftype.Checked = false;
        rdb_stfcat.Checked = false;
        chksellst.Checked = false;
        updstftyp.Visible = false;
        updstfcat.Visible = false;
        lblcount.Visible = false;
        fldcount.Visible = false;


        if (chkhod.Checked && ddltype.SelectedItem.Value == "0")
        {
            lblcount.Visible = true;
            fldcount.Visible = true;
        }

    }

    protected void chkhigheroff_Change(object sender, EventArgs e)
    {
        txtmobilno.Text = "";
        fldwithlev.Visible = false;
        txtmobilno.Enabled = false;
        chkwithlev.Checked = false;
        chkgrpby.Checked = false;
        chkgrpby.Enabled = false;
        chkwithlev.Enabled = false;
        divstftyp.Visible = false;
        divsellst.Visible = false;
        rdb_stftype.Checked = false;
        rdb_stfcat.Checked = false;
        chksellst.Checked = false;
        updstftyp.Visible = false;
        updstfcat.Visible = false;

        if (chkhigheroff.Checked)
            txtmobilno.Enabled = true;
        if (chkhigheroff.Checked && ddltype.SelectedItem.Value == "3")
        {
            fldwithlev.Visible = true;
            chkgrpby.Enabled = true;
            chkwithlev.Enabled = true;
        }

        if (chkhigheroff.Checked && ddltype.SelectedItem.Value == "4")
        {
            //fldwithlev.Visible = true;
            //divstftyp.Visible = true;
            //rdb_stftype.Checked = true;
            //divsellst.Visible = true;
            //chksellst.Checked = true;
            //updstftyp.Visible = true;
            //bindstafftype();
        }
        //added by saranyadevi 25.07.2018
        if (rdb_Mail.Checked)
        {
            if (chkhigheroff.Checked && (ddltype1.SelectedItem.Value == "0" || ddltype1.SelectedItem.Value == "1" || ddltype1.SelectedItem.Value == "2" || ddltype1.SelectedItem.Value == "3" || ddltype1.SelectedItem.Value == "4" || ddltype1.SelectedItem.Value == "5" || ddltype1.SelectedItem.Value == "6" || ddltype1.SelectedItem.Value == "7" || ddltype1.SelectedItem.Value == "8" || ddltype1.SelectedItem.Value == "9" || ddltype1.SelectedItem.Value == "10"))
                txtsendmail.Enabled = true;
            if (chkhigheroff.Checked == false && (ddltype1.SelectedItem.Value == "0" || ddltype1.SelectedItem.Value == "1" || ddltype1.SelectedItem.Value == "2" || ddltype1.SelectedItem.Value == "3" || ddltype1.SelectedItem.Value == "4" || ddltype1.SelectedItem.Value == "5" || ddltype1.SelectedItem.Value == "6" || ddltype1.SelectedItem.Value == "7" || ddltype1.SelectedItem.Value == "8" || ddltype1.SelectedItem.Value == "9" || ddltype1.SelectedItem.Value == "10"))
                txtsendmail.Enabled = false;
        }


    }

    protected void rdb_stftype_change(object sender, EventArgs e)
    {
        updstftyp.Visible = false;
        updstfcat.Visible = false;
        if (chksellst.Checked && rdb_stftype.Checked)
        {
            updstftyp.Visible = true;
            bindstafftype();
        }
    }

    protected void rdb_stfcat_change(object sender, EventArgs e)
    {
        updstftyp.Visible = false;
        updstfcat.Visible = false;
        if (chksellst.Checked && rdb_stfcat.Checked)
        {
            updstfcat.Visible = true;
            category();
        }
    }

    protected void chksellst_change(object sender, EventArgs e)
    {
        updstftyp.Visible = false;
        updstfcat.Visible = false;
        if (chksellst.Checked && rdb_stftype.Checked)
        {
            updstftyp.Visible = true;
            bindstafftype();
        }
        else if (chksellst.Checked && rdb_stfcat.Checked)
        {
            updstfcat.Visible = true;
            category();
        }
    }

    protected void chkgrpby_Change(object sender, EventArgs e)
    {
        divsellst.Visible = false;
        divstftyp.Visible = false;
        updstftyp.Visible = false;
        updstfcat.Visible = false;
        chksellst.Checked = false;
        rdb_stftype.Checked = false;
        rdb_stfcat.Checked = false;

        if (chkgrpby.Checked)
        {
            divsellst.Visible = true;
            divstftyp.Visible = true;
            chksellst.Checked = true;
            updstftyp.Visible = true;
            rdb_stftype.Checked = true;
            bindstafftype();
        }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (rdb_Sms.Checked)
        {
            if (ddltype.SelectedIndex == 0)
                birthday(sender, e);
            else if (ddltype.SelectedIndex == 1)
                studattnd(sender, e);
            else if (ddltype.SelectedIndex == 2)
                cammarks(sender, e);
            else if (ddltype.SelectedIndex == 3)
                staffattnd(sender, e);
            else if (ddltype.SelectedIndex == 4)
                blockbox(sender, e);
            else if (ddltype.SelectedIndex == 5)
                studattndshort(sender, e);
            else if (ddltype.SelectedIndex == 6)
                hostelstudattnd(sender, e);
            else if (ddltype.SelectedIndex == 7)
                studstaffcumattnd(sender, e);
            else if (ddltype.SelectedIndex == 8)
                automaticsms(sender, e);
            else if (ddltype.SelectedIndex == 9)
                FinanceSettings(sender, e);
            else if (ddltype.SelectedIndex == 10)
                DriveLicSettings(sender, e);
            else if (ddltype.SelectedIndex == 11)
                FCRemindSettings(sender, e);
            else if (ddltype.SelectedIndex == 12)
                InsRenewSettings(sender, e);
            else if (ddltype.SelectedIndex == 13)//saranya devi
                studcumattnd(sender, e);
            else if (ddltype.SelectedIndex == 14)//saranya srinivasan
                receiptcancel(sender, e);
            else if (ddltype.SelectedIndex == 15)//saranya devi11.12.2018
                StudentDueDate(sender, e);
            else if (ddltype.SelectedIndex == 16)//saranya devi13.12.2018
                StudentHomeWork(sender, e);
            else if (ddltype.SelectedIndex == 17)//saranya devi17.12.2018
                StudentAbsent(sender, e);
            else if (ddltype.SelectedIndex == 18)//saranya devi28.01.2019
                HostelStudentAbsent(sender, e);
        }
        else
        {
            if (ddltype1.SelectedIndex == 0)//Added by Saranyadevi25.7.2018
                hostelAbsentlist(sender, e);
            else if (ddltype1.SelectedIndex == 1)//Added by Saranyadevi26.7.2018
                AttendanceSummaryHostelWise(sender, e);
            else if (ddltype1.SelectedIndex == 2)//Added by Saranyadevi26.7.2018
                ItemStockReport(sender, e);
            else if (ddltype1.SelectedIndex == 3)//Added by Saranyadevi 6.8.2018
                BlackBoxReport(sender, e);
            else if (ddltype1.SelectedIndex == 4)//Added by Saranyadevi 6.8.2018
                OverAllAttendanceReportForParticularDay(sender, e);
            else if (ddltype1.SelectedIndex == 5)//Added by Saranyadevi 6.8.2018
                AbsenteesReport(sender, e);
            else if (ddltype1.SelectedIndex == 6)//Added by Saranyadevi 6.8.2018
                Institutionwise_Paid_Report(sender, e);
            else if (ddltype1.SelectedIndex == 7)//Added by Saranyadevi 6.8.2018
                Institutionwise_Balance_Report(sender, e);
            else if (ddltype1.SelectedIndex == 8)//Added by Saranyadevi 22.8.2018
                Paymode_Collection_Report(sender, e);
            else if (ddltype1.SelectedIndex == 9)//Added by Saranyadevi 30.8.2018
                Staff_Attendance_Report(sender, e);
            else if (ddltype1.SelectedIndex == 10)//Added by Saranyadevi 30.8.2018
                dept_Wise_Staff_Attendance_Report(sender, e);

        }
    }

    protected void btnexit_click(object sender, EventArgs e)
    {
        if (rdb_Sms.Checked)
        {
            ddltype.SelectedIndex = ddltype.Items.IndexOf(ddltype.Items.FindByText(ddltype.SelectedItem.Text));
            ddltype_change(sender, e);
        }
        else
        {
            ddltype1.SelectedIndex = ddltype1.Items.IndexOf(ddltype1.Items.FindByText(ddltype1.SelectedItem.Text));
            ddltype1_change(sender, e);

        }
    }

    private void birthday(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string sentime = sendtime.ToString("HH:mm:ss");
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string studwish = "";
            string staffwish = "";
            string sendstud = "";
            string sendfat = "";
            string sendmot = "";
            string issend = "";
            string sendtohod = "";
            string sendtohigheroff = "";
            string mobileno = "";
            string studentcount = "";
            string staffcount = "";
            if (chkstudent.Checked)
            {
                studwish = "1";
                if (chkstudwish.Checked == false && chkfatwish.Checked == false && chkmotwish.Checked == false)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Select Whose mobile you want to Send Wishes!";
                    return;
                }
                else
                {
                    if (chkstudwish.Checked)
                        sendstud = "1";
                    if (chkfatwish.Checked)
                        sendfat = "1";
                    if (chkmotwish.Checked)
                        sendmot = "1";
                }
            }
            if (chkstaff.Checked)
                staffwish = "1";
            if (chksendsms.Checked)
                issend = "1";
            if (chkhod.Checked)
            {
                sendtohod = "1";
                if (chkstudcount.Checked == false && chkstafcount.Checked == false)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Select Whose mobile you want to Send Wishes!";
                    return;
                }
                else
                {
                    if (chkstudcount.Checked)
                        studentcount = "1";
                    if (chkstafcount.Checked)
                        staffcount = "1";

                }
            }
            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',Stud_Wish='" + studwish + "',Staff_Wish='" + staffwish + "',SendToHOD='" + sendtohod + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',SendToStudMob='" + sendstud + "',SendToFatMob='" + sendfat + "',SendToMotMob='" + sendmot + "',Student_Count='" + studentcount + "',Staff_Count='" + staffcount + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,Stud_Wish,Staff_Wish,SendToHOD,SendToHOff,IsSend,MobileNos,SendToStudMob,SendToFatMob,SendToMotMob,Student_Count,Staff_Count,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + studwish + "','" + staffwish + "','" + sendtohod + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + sendstud + "','" + sendfat + "','" + sendmot + "','" + studentcount + "','" + staffcount + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 0;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void studattnd(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string issend = "";
            string sendtohod = "";
            string sendtohigheroff = "";
            string mobileno = "";
            string isstudattndgrp = "";
            string isinclongabs = "";
            string inclongabscount = "";

            if (chkhod.Checked)
                sendtohod = "1";
            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            if (chksendsms.Checked)
                issend = "1";
            if (chkgrphos.Checked)
                isstudattndgrp = "1";
            if (chkinclongabs.Checked)
            {
                isinclongabs = "1";
                if (txtgreater.Text.Trim() != "")
                {
                    inclongabscount = Convert.ToString(txtgreater.Text.Trim());
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Long Absent Count!";
                    return;
                }
            }

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOD='" + sendtohod + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',IsStudAttnGroup='" + isstudattndgrp + "',IncludeLongAbs='" + isinclongabs + "',IncludeLongAbsCount='" + inclongabscount + "',user_code='" + usercode + "',SMS_Send_Date='' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOD,SendToHOff,IsSend,MobileNos,IsStudAttnGroup,IncludeLongAbs,IncludeLongAbsCount,user_Code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohod + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + isstudattndgrp + "','" + isinclongabs + "','" + inclongabscount + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 1;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void cammarks(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string[] spldt = Convert.ToString(txtsenddt.Text).Split('/');
            DateTime senddt = Convert.ToDateTime(spldt[1] + "/" + spldt[0] + "/" + spldt[2]);
            string issend = "";
            string sendtohigheroff = "";
            string mobileno = "";
            string testname = "";
            if (Convert.ToString(ddltest.SelectedItem.Text).Trim() != "Select")
                testname = Convert.ToString(ddltest.SelectedItem.Text).Trim();
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select the Test Name!";
                return;
            }
            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            if (chksendsms.Checked)
                issend = "1";
            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',Send_Date='" + senddt.ToString("MM/dd/yyyy") + "',Test_Name='" + testname + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOff,IsSend,MobileNos,Send_Date,Test_Name,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + senddt.ToString("MM/dd/yyyy") + "','" + testname + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 2;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void staffattnd(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string issend = "";
            string sendtostaff = "";
            string sendtohigheroff = "";
            string mobileno = "";
            string session = "";
            string isstfattndlev = "";
            string isstfgrpby = "";
            string stfattndgrptyp = "";
            string isstfgrplst = "";
            string stftype = "";

            if (chkstaff.Checked)
                sendtostaff = "1";
            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            if (chksendsms.Checked)
                issend = "1";
            if (ddlsession.SelectedItem.Text == "Morning")
                session = "1";
            else
                session = "2";
            if (chkwithlev.Checked)
                isstfattndlev = "1";
            if (chkgrpby.Checked)
            {
                isstfgrpby = "1";
                if (rdb_stftype.Checked)
                    stfattndgrptyp = "1";
                else
                    stfattndgrptyp = "2";
                if (chksellst.Checked)
                {
                    isstfgrplst = "1";
                    if (rdb_stftype.Checked)
                    {
                        stftype = GetSelectedItemsTextnew(cblstftype);
                        if (stftype.Trim() == "")
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please Select the Staff Type!";
                            return;
                        }
                    }
                    else
                    {
                        stftype = GetSelectedItemsValueAsStringnew(cblstfcat);
                        if (stftype.Trim() == "")
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please Select the Staff Category!";
                            return;
                        }
                    }
                }
            }
            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToStaff='" + sendtostaff + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',Send_Session='" + session + "',StfAttnRepWithLeave='" + isstfattndlev + "',StfAttnRepGroupStfType='" + isstfgrpby + "',StfAttnGroupType='" + stfattndgrptyp + "',StfAttnIsGroupList='" + isstfgrplst + "',StfAttnGroupList='" + stftype + "',user_code='" + usercode + "', SMS_Send_Date=''  where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToStaff,SendToHOff,IsSend,MobileNos,Send_Session,StfAttnRepWithLeave,StfAttnRepGroupStfType,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_Code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtostaff + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + session + "','" + isstfattndlev + "','" + isstfgrpby + "','" + stfattndgrptyp + "','" + isstfgrplst + "','" + stftype + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 3;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void blockbox(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string issend = "";
            string sendtohod = "";
            string sendtohigheroff = "";
            string mobileno = "";
            string stfattndgrptyp = "";
            string isstfgrplst = "";
            string stftype = "";

            if (chkhod.Checked)
                sendtohod = "1";
            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            if (chksendsms.Checked)
                issend = "1";
            if (rdb_stftype.Checked)
                stfattndgrptyp = "1";
            else
                stfattndgrptyp = "2";
            if (chksellst.Checked)
            {
                isstfgrplst = "1";
                if (rdb_stftype.Checked)
                    stftype = GetSelectedItemsTextnew(cblstftype);
                else
                    stftype = GetSelectedItemsValueAsStringnew(cblstfcat);
            }
            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOD='" + sendtohod + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',StfAttnGroupType='" + stfattndgrptyp + "',StfAttnIsGroupList='" + isstfgrplst + "',StfAttnGroupList='" + stftype + "',user_Code='" + usercode + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOD,SendToHOff,IsSend,MobileNos,StfAttnGroupType,StfAttnIsGroupList,StfAttnGroupList,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohod + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + stfattndgrptyp + "','" + isstfgrplst + "','" + stftype + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 4;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void studattndshort(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string[] spldt = Convert.ToString(txtsenddt.Text).Split('/');
            DateTime senddt = Convert.ToDateTime(spldt[1] + "/" + spldt[0] + "/" + spldt[2]);
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string mobileno = "";

            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            if (chksendsms.Checked)
                issend = "1";

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',Send_Date='" + senddt.ToString("MM/dd/yyyy") + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOff,IsSend,MobileNos,Send_Date,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + senddt.ToString("MM/dd/yyyy") + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 5;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void hostelstudattnd(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string mobileno = "";
            string session = "";

            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            if (chksendsms.Checked)
                issend = "1";
            if (ddlsession.SelectedItem.Text == "Morning")
                session = "1";
            else
                session = "2";

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',Send_Session='" + session + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOff,IsSend,MobileNos,Send_Session,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + session + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 6;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void studstaffcumattnd(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string mobileno = "";
            string session = "";

            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            if (chksendsms.Checked)
                issend = "1";
            if (ddlsession.SelectedItem.Text == "Morning")
                session = "1";
            else
                session = "2";

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',Send_Session='" + session + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOff,IsSend,MobileNos,Send_Session,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + session + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 7;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void automaticsms(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime alttime1 = new DateTime();
            DateTime alttime2 = new DateTime();
            DateTime frmdt = new DateTime();
            DateTime todt = new DateTime();
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            if (btnaddtime.Text == "Hide")
            {
                alttime1 = DateTime.Parse(Convert.ToString(ddlhr1.SelectedItem.Text) + ":" + Convert.ToString(ddlmin1.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer1.SelectedItem.Text));
                alttime2 = DateTime.Parse(Convert.ToString(ddlhr2.SelectedItem.Text) + ":" + Convert.ToString(ddlmin2.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer2.SelectedItem.Text));
                if (sendtime >= alttime1)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Send Time Should be less than Alternate Time1!";
                    return;
                }
                if (alttime1 >= alttime2)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Alternate Time1 Should be less than Alternate Time2!";
                    return;
                }
                if (alttime2 <= sendtime)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Send Time Should be less than Alternate Time2!";
                    return;
                }
            }
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string issend = "";
            string sendtoStudent = "";
            string senttostaff = "";
            string devname = "";
            string hosname = "";
            string morabs = "";
            string eveabs = "";
            string stfgrptype = "";
            string stftype = "";
            string deptcode = "";
            string shift = "";

            if (chkstudent.Checked || chkstaff.Checked)
            {
                devname = GetSelectedItemsValueAsStringnew(cbldevname);
                if (devname.Trim() == "")
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Please Select Device Name!";
                    return;
                }
                frmdt = getdate(txtfrmdt.Text);
                todt = getdate(txttodt.Text);
                if (frmdt > todt)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "From Date Should be less than To Date!";
                    return;
                }
                if (frmdt > currdt || todt > currdt)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Not Allowed For Future Date!";
                    return;
                }
                if (chkmorabs.Checked)
                    morabs = "1";
                if (chkeveabs.Checked)
                    eveabs = "1";
            }
            if (chkstudent.Checked)
            {
                sendtoStudent = "1";
                hosname = GetSelectedItemsValueAsStringnew(cblhosname);
            }
            if (chkstaff.Checked)
            {
                senttostaff = "1";
                if (ddlstftypcat.SelectedIndex == 0)
                {
                    stfgrptype = "1";
                    if (ddlstftypedown.SelectedItem.Text != "Select")
                        stftype = Convert.ToString(ddlstftypedown.SelectedItem.Text);
                }
                else
                {
                    stfgrptype = "2";
                    if (ddlstfcatdown.SelectedItem.Text != "Select")
                    {
                        if (ddlstfcatdown.SelectedItem.Text != "All")
                            stftype = Convert.ToString(ddlstfcatdown.SelectedItem.Value);
                        else
                            stftype = Convert.ToString(ddlstfcatdown.SelectedItem.Text);
                    }
                }
                if (ddldeptdown.SelectedItem.Text != "Select")
                {
                    if (ddldeptdown.SelectedItem.Text != "All")
                        deptcode = Convert.ToString(ddldeptdown.SelectedItem.Value);
                    else
                        deptcode = Convert.ToString(ddldeptdown.SelectedItem.Text);
                }
                if (ddlshiftdown.SelectedItem.Text != "Select")
                    shift = Convert.ToString(ddlshiftdown.SelectedItem.Text);
            }
            if (chksendsms.Checked)
                issend = "1";

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToStud='" + sendtoStudent + "',SendToStaff='" + senttostaff + "',IsSend='" + issend + "',user_code='" + usercode + "',StfAttnGroupType='" + stfgrptype + "',StfAttnGroupList='" + stftype + "',alternate_time1='" + alttime1.ToString("HH:mm:ss") + "',alternate_time2='" + alttime2.ToString("HH:mm:ss") + "',MorAbs='" + morabs + "',EveAbs='" + eveabs + "',DeviceID='" + devname + "',hostelmasterpk='" + hosname + "',dept_code='" + deptcode + "',shift='" + shift + "',From_Date='" + frmdt.ToString("MM/dd/yyyy") + "',To_Date='" + todt.ToString("MM/dd/yyyy") + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToStud,SendToStaff,IsSend,user_code,StfAttnGroupType,StfAttnGroupList,alternate_time1,alternate_time2,MorAbs,EveAbs,DeviceID,hostelmasterpk,dept_code,shift,From_Date,To_Date) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtoStudent + "','" + senttostaff + "','" + issend + "','" + usercode + "','" + stfgrptype + "','" + stftype + "','" + alttime1.ToString("HH:mm:ss") + "','" + alttime2.ToString("HH:mm:ss") + "','" + morabs + "','" + eveabs + "','" + devname + "','" + hosname + "','" + deptcode + "','" + shift + "','" + frmdt.ToString("MM/dd/yyyy") + "','" + todt.ToString("MM/dd/yyyy") + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 8;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void FinanceSettings(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string[] spldt = Convert.ToString(txtsenddt.Text).Split('/');
            DateTime senddt = Convert.ToDateTime(spldt[1] + "/" + spldt[0] + "/" + spldt[2]);
            string issend = "";
            string sendtohod = "";
            string sendtohigheroff = "";
            string mobileno = "";

            if (chkhod.Checked)
                sendtohod = "1";
            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            if (chksendsms.Checked)
                issend = "1";

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOD='" + sendtohod + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',Send_Date='" + senddt.ToString("MM/dd/yyyy") + "',user_Code='" + usercode + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOD,SendToHOff,IsSend,MobileNos,Send_Date,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohod + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + senddt.ToString("MM/dd/yyyy") + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 9;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void DriveLicSettings(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string issend = "";
            string sendtostaff = "";
            string sendtohigheroff = "";
            string mobileno = "";
            int DaysRemind = 0;

            if (chkstaff.Checked)
                sendtostaff = "1";
            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            Int32.TryParse(Convert.ToString(txtdaysRemind.Text), out DaysRemind);
            if (DaysRemind == 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter the Days Before Remind!";
                return;
            }
            if (chksendsms.Checked)
                issend = "1";

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToStaff='" + sendtostaff + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',user_Code='" + usercode + "',Days_Remind='" + DaysRemind + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToStaff,SendToHOff,IsSend,MobileNos,user_code,Days_Remind) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtostaff + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + usercode + "','" + DaysRemind + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 10;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void FCRemindSettings(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string mobileno = "";
            int DaysRemind = 0;

            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            Int32.TryParse(Convert.ToString(txtdaysRemind.Text), out DaysRemind);
            if (DaysRemind == 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter the Days Before Remind!";
                return;
            }
            if (chksendsms.Checked)
                issend = "1";

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',user_Code='" + usercode + "',Days_Remind='" + DaysRemind + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOff,IsSend,MobileNos,user_code,Days_Remind) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + usercode + "','" + DaysRemind + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 11;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void InsRenewSettings(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string mobileno = "";
            int DaysRemind = 0;

            if (chkhigheroff.Checked)
            {
                sendtohigheroff = "1";
                if (txtmobilno.Text.Trim() != "")
                    mobileno = Convert.ToString(txtmobilno.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the Mobile No's!";
                    return;
                }
            }
            Int32.TryParse(Convert.ToString(txtdaysRemind.Text), out DaysRemind);
            if (DaysRemind == 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter the Days Before Remind!";
                return;
            }
            if (chksendsms.Checked)
                issend = "1";

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',IsSend='" + issend + "',MobileNos='" + mobileno + "',user_Code='" + usercode + "',Days_Remind='" + DaysRemind + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOff,IsSend,MobileNos,user_code,Days_Remind) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohigheroff + "','" + issend + "','" + mobileno + "','" + usercode + "','" + DaysRemind + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 12;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void studcumattnd(object sender, EventArgs e)//saranya devi(31.10.2017)
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string sentime = sendtime.ToString("HH:mm:ss");
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);

            string sendstud = "";
            string sendfat = "";
            string sendmot = "";
            string issend = "";
            int DaysInterval = 0;


            if (chkstudsms.Checked == false && chkfatsms.Checked == false && chkmotsms.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose mobile you want to Send Sms!";
                return;
            }
            else
            {
                if (chkstudsms.Checked)
                    sendstud = "1";
                if (chkfatsms.Checked)
                    sendfat = "1";
                if (chkmotsms.Checked)
                    sendmot = "1";
            }

            Int32.TryParse(Convert.ToString(txtdays.Text), out DaysInterval);
            if (DaysInterval == 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Enter the Days!";
                return;
            }

            if (cksendsms.Checked)
                issend = "1";

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',IsSend='" + issend + "',SendToStudMob='" + sendstud + "',SendToFatMob='" + sendfat + "',SendToMotMob='" + sendmot + "',user_code='" + usercode + "',Days_Interval ='" + DaysInterval + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,IsSend,SendToStudMob,SendToFatMob,SendToMotMob,user_code,Days_Interval) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + issend + "','" + sendstud + "','" + sendfat + "','" + sendmot + "','" + usercode + "','" + DaysInterval + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 0;
                ddltype_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }

    //========================added by saranya srinivasan(17-11-2017)==========================/


    private void receiptcancel(object sender, EventArgs e)
    {
        try
        {
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string mobileno = txtmobilno.Text.Trim();
            string email = txtsendmail.Text.Trim();

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(mobileno))
            {
                string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set user_code='" + usercode + "',MobileNos='" + mobileno + "',EmailId='" + email + "' where sms_purpose='receipt cancel' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,user_code,MobileNos,EmailId) values ('" + smspurpose + "','" + usercode + "','" + mobileno + "','" + email + "')";

                inscount = d2.update_method_wo_parameter(insq, "Text");
                if (inscount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Settings Saved Successfully!";
                    ddltype.SelectedIndex = 14;
                    ddltype_change(sender, e);
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Mobile No and Email Id Should not be Empty";
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx");
        }

    }
    //=======================================================================================================================//


    //Student Due Date


    private void StudentDueDate(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string sentime = sendtime.ToString("HH:mm:ss");
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string studwish = "";
            string issend = "";

            if (chkstudent.Checked)
                studwish = "1";
            if (chksendsms.Checked)
                issend = "1";
            if (chkstudent.Checked)
            {
                string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToStudMob='" + studwish + "',IsSend='" + issend + "',user_code='" + usercode + "',SMS_Send_Date='' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,IsSend,SendToStudMob,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + issend + "','" + studwish + "','" + usercode + "')";
                inscount = d2.update_method_wo_parameter(insq, "Text");
                if (inscount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Settings Saved Successfully!";
                    ddltype.SelectedIndex = 0;
                    ddltype_change(sender, e);
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose mobile you want to Send Wishes!";
                return;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }


    private void StudentHomeWork(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string sentime = sendtime.ToString("HH:mm:ss");
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string studwish = "";
            string issend = "";

            if (chkstudent.Checked)
                studwish = "1";
            if (chksendsms.Checked)
                issend = "1";
            if (chkstudent.Checked)
            {
                string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToStudMob='" + studwish + "',IsSend='" + issend + "',user_code='" + usercode + "',SMS_Send_Date='' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,IsSend,SendToStudMob,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + issend + "','" + studwish + "','" + usercode + "')";
                inscount = d2.update_method_wo_parameter(insq, "Text");
                if (inscount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Settings Saved Successfully!";
                    ddltype.SelectedIndex = 0;
                    ddltype_change(sender, e);
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose mobile you want to Send Wishes!";
                return;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }
    }

    private void StudentAbsent(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string sentime = sendtime.ToString("HH:mm:ss");
            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string sendstud = "";
            string sendfat = "";
            string sendmot = "";
            string issend = "";
            string hrday = "";
            string sendperdsess = "";
            if (chkstudsms.Checked == false && chkfatsms.Checked == false && chkmotsms.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose mobile you want to Send Sms!";
                return;
            }
            else
            {
                if (chkstudsms.Checked)
                    sendstud = "1";
                if (chkfatsms.Checked)
                    sendfat = "1";
                if (chkmotsms.Checked)
                    sendmot = "1";
            }
            if (Checkhrdaysend.Checked)
                issend = "1";
            if (rdohour.Checked)
            {
                hrday = "0";
                if (cbl_hour.Items.Count > 0)
                {
                    for (int sel = 0; sel < cbl_hour.Items.Count; sel++)
                    {
                        if (cbl_hour.Items[sel].Selected == true)
                        {
                            if (sendperdsess.Length == 0)
                            {
                                sendperdsess = Convert.ToString(cbl_hour.Items[sel].Value);
                            }
                            else
                            {
                                sendperdsess = sendperdsess + "&" + Convert.ToString(cbl_hour.Items[sel].Value);
                            }
                        }
                    }
                }

            }
            else
            {
                hrday = "1";
                if (rbldayType.SelectedIndex == 0)
                    sendperdsess = "0";
                if (rbldayType.SelectedIndex == 1)
                    sendperdsess = "1";
                if (rbldayType.SelectedIndex == 2)
                    sendperdsess = "2";

            }


            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',IsSend='" + issend + "',SendToStudMob='" + sendstud + "',SendToFatMob='" + sendfat + "',SendToMotMob='" + sendmot + "',user_code='" + usercode + "',Hourdaywise='" + hrday + "',Send_Period_Session='" + sendperdsess + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,IsSend,SendToStudMob,SendToFatMob,SendToMotMob,user_code,Hourdaywise,Send_Period_Session) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + issend + "','" + sendstud + "','" + sendfat + "','" + sendmot + "','" + usercode + "','" + hrday + "','" + sendperdsess + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 17;
                ddltype_change(sender, e);
            }


        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); 
        }
    }


    private void HostelStudentAbsent(object sender, EventArgs e)
    {
        try
        {
            inscount = 0;
            DateTime currdt = DateTime.Now;

            string smspurpose = Convert.ToString(ddltype.SelectedItem.Text);
            string sendstud = "";
            string sendfat = "";
            string sendmot = "";
            string issend = "";
            if (chkstudsms.Checked == false && chkfatsms.Checked == false && chkmotsms.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose mobile you want to Send Sms!";
                return;
            }
            else
            {
                if (chkstudsms.Checked)
                    sendstud = "1";
                if (chkfatsms.Checked)
                    sendfat = "1";
                if (chkmotsms.Checked)
                    sendmot = "1";
            }
            if (chksendsms.Checked)
                issend = "1";


            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set IsSend='" + issend + "',SendToStudMob='" + sendstud + "',SendToFatMob='" + sendfat + "',SendToMotMob='" + sendmot + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,college_code,IsSend,SendToStudMob,SendToFatMob,SendToMotMob,user_code) values ('" + smspurpose + "','" + ddlcollege.SelectedItem.Value + "','" + issend + "','" + sendstud + "','" + sendfat + "','" + sendmot + "','" + usercode + "')";

            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype.SelectedIndex = 18;
                ddltype_change(sender, e);
            }


        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); 
        }
    }

    //End

    #region Window_Service_Automatic_Mail


    private void hostelAbsentlist(object sender, EventArgs e)//Added by SaranyaDevi 25.7.2018
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype1.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string sendmail = "";
            string session = "";

            if (chkhigheroff.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose MailID you want to Send Mail!";
                return;
            }
            else
            {
                sendtohigheroff = "1";
                if (txtsendmail.Text.Trim() != "")
                    sendmail = Convert.ToString(txtsendmail.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the MailID!";
                    return;
                }
            }
            if (ddlsession.SelectedItem.Text == "Morning")
                session = "1";
            else
                session = "2";

            //string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',Send_Session='" + session + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOff,EmailId,Send_Session,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohigheroff + "','" + sendmail + "','" + session + "','" + usercode + "')";
            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',Send_Session='" + session + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "'  else Insert into Automatic_SMS (sms_purpose,sending_Time,SendToHOff,EmailId,Send_Session,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + sendtohigheroff + "','" + sendmail + "','" + session + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype1.SelectedIndex = 0;
                ddltype1_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }


    private void AttendanceSummaryHostelWise(object sender, EventArgs e)//Added by SaranyaDevi 26.7.2018
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype1.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string sendmail = "";
            string session = "";

            if (chkhigheroff.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose MailID you want to Send Mail!";
                return;
            }
            else
            {
                sendtohigheroff = "1";
                if (txtsendmail.Text.Trim() != "")
                    sendmail = Convert.ToString(txtsendmail.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the MailID!";
                    return;
                }
            }
            if (ddlsession.SelectedItem.Text == "Morning")
                session = "1";
            else
                session = "2";

            //string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',Send_Session='" + session + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOff,EmailId,Send_Session,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohigheroff + "','" + sendmail + "','" + session + "','" + usercode + "')";
            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',Send_Session='" + session + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "'  else Insert into Automatic_SMS (sms_purpose,sending_Time,SendToHOff,EmailId,Send_Session,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + sendtohigheroff + "','" + sendmail + "','" + session + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype1.SelectedIndex = 1;
                ddltype1_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }


    private void ItemStockReport(object sender, EventArgs e)//Added by SaranyaDevi 27.7.2018
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype1.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string sendmail = "";


            if (chkhigheroff.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose MailID you want to Send Mail!";
                return;
            }
            else
            {
                sendtohigheroff = "1";
                if (txtsendmail.Text.Trim() != "")
                    sendmail = Convert.ToString(txtsendmail.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the MailID!";
                    return;
                }
            }
            //if (ddlsession.SelectedItem.Text == "Morning")
            //    session = "1";
            //else
            //    session = "2";

            //string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',Send_Session='" + session + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "' and college_code='" + ddlcollege.SelectedItem.Value + "' else Insert into Automatic_SMS (sms_purpose,sending_Time,college_code,SendToHOff,EmailId,Send_Session,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + ddlcollege.SelectedItem.Value + "','" + sendtohigheroff + "','" + sendmail + "','" + session + "','" + usercode + "')";
            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "'  else Insert into Automatic_SMS (sms_purpose,sending_Time,SendToHOff,EmailId,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + sendtohigheroff + "','" + sendmail + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype1.SelectedIndex = 2;
                ddltype1_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }

    private void BlackBoxReport(object sender, EventArgs e)//Added by SaranyaDevi 6.8.2018
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype1.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string sendmail = "";


            if (chkhigheroff.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose MailID you want to Send Mail!";
                return;
            }
            else
            {
                sendtohigheroff = "1";
                if (txtsendmail.Text.Trim() != "")
                    sendmail = Convert.ToString(txtsendmail.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the MailID!";
                    return;
                }
            }
            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "'  else Insert into Automatic_SMS (sms_purpose,sending_Time,SendToHOff,EmailId,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + sendtohigheroff + "','" + sendmail + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype1.SelectedIndex = 3;
                ddltype1_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }


    private void OverAllAttendanceReportForParticularDay(object sender, EventArgs e)//Added by SaranyaDevi 6.8.2018
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype1.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string sendmail = "";


            if (chkhigheroff.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose MailID you want to Send Mail!";
                return;
            }
            else
            {
                sendtohigheroff = "1";
                if (txtsendmail.Text.Trim() != "")
                    sendmail = Convert.ToString(txtsendmail.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the MailID!";
                    return;
                }
            }

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "'  else Insert into Automatic_SMS (sms_purpose,sending_Time,SendToHOff,EmailId,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + sendtohigheroff + "','" + sendmail + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype1.SelectedIndex = 4;
                ddltype1_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }

    private void AbsenteesReport(object sender, EventArgs e)//Added by SaranyaDevi 6.8.2018
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype1.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string sendmail = "";


            if (chkhigheroff.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose MailID you want to Send Mail!";
                return;
            }
            else
            {
                sendtohigheroff = "1";
                if (txtsendmail.Text.Trim() != "")
                    sendmail = Convert.ToString(txtsendmail.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the MailID!";
                    return;
                }
            }

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "'  else Insert into Automatic_SMS (sms_purpose,sending_Time,SendToHOff,EmailId,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + sendtohigheroff + "','" + sendmail + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype1.SelectedIndex = 5;
                ddltype1_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }

    private void Institutionwise_Paid_Report(object sender, EventArgs e)//Added by SaranyaDevi 6.8.2018
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype1.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string sendmail = "";


            if (chkhigheroff.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose MailID you want to Send Mail!";
                return;
            }
            else
            {
                sendtohigheroff = "1";
                if (txtsendmail.Text.Trim() != "")
                    sendmail = Convert.ToString(txtsendmail.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the MailID!";
                    return;
                }
            }

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "'  else Insert into Automatic_SMS (sms_purpose,sending_Time,SendToHOff,EmailId,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + sendtohigheroff + "','" + sendmail + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype1.SelectedIndex = 6;
                ddltype1_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }


    private void Institutionwise_Balance_Report(object sender, EventArgs e)//Added by SaranyaDevi 6.8.2018
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype1.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string sendmail = "";


            if (chkhigheroff.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose MailID you want to Send Mail!";
                return;
            }
            else
            {
                sendtohigheroff = "1";
                if (txtsendmail.Text.Trim() != "")
                    sendmail = Convert.ToString(txtsendmail.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the MailID!";
                    return;
                }
            }

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "'  else Insert into Automatic_SMS (sms_purpose,sending_Time,SendToHOff,EmailId,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + sendtohigheroff + "','" + sendmail + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype1.SelectedIndex = 7;
                ddltype1_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }


    private void Paymode_Collection_Report(object sender, EventArgs e)//Added by SaranyaDevi 22.8.2018
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype1.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string sendmail = "";


            if (chkhigheroff.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose MailID you want to Send Mail!";
                return;
            }
            else
            {
                sendtohigheroff = "1";
                if (txtsendmail.Text.Trim() != "")
                    sendmail = Convert.ToString(txtsendmail.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the MailID!";
                    return;
                }
            }

            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "'  else Insert into Automatic_SMS (sms_purpose,sending_Time,SendToHOff,EmailId,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + sendtohigheroff + "','" + sendmail + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype1.SelectedIndex = 8;
                ddltype1_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }


    private void Staff_Attendance_Report(object sender, EventArgs e)//Added by SaranyaDevi 30.8.2018
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype1.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string sendmail = "";
            string session = "";

            if (chkhigheroff.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose MailID you want to Send Mail!";
                return;
            }
            else
            {
                sendtohigheroff = "1";
                if (txtsendmail.Text.Trim() != "")
                    sendmail = Convert.ToString(txtsendmail.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the MailID!";
                    return;
                }
            }
            if (ddlsession.SelectedItem.Text == "Morning")
                session = "1";
            else
                session = "2";
            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',Send_Session='" + session + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "'  else Insert into Automatic_SMS (sms_purpose,sending_Time,SendToHOff,EmailId,Send_Session,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + sendtohigheroff + "','" + sendmail + "','" + session + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype1.SelectedIndex = 9;
                ddltype1_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }

    private void dept_Wise_Staff_Attendance_Report(object sender, EventArgs e)//Added by SaranyaDevi 14.12.2018
    {
        try
        {

            inscount = 0;
            DateTime currdt = DateTime.Now;
            DateTime sendtime = DateTime.Parse(Convert.ToString(ddlhr.SelectedItem.Text) + ":" + Convert.ToString(ddlmin.SelectedItem.Text) + ":" + Convert.ToString(currdt.Second) + " " + Convert.ToString(ddlmer.SelectedItem.Text));
            string smspurpose = Convert.ToString(ddltype1.SelectedItem.Text);
            string issend = "";
            string sendtohigheroff = "";
            string sendmail = "";
            string session = "";

            if (chkhigheroff.Checked == false)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Whose MailID you want to Send Mail!";
                return;
            }
            else
            {
                sendtohigheroff = "1";
                if (txtsendmail.Text.Trim() != "")
                    sendmail = Convert.ToString(txtsendmail.Text.Trim());
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Enter the MailID!";
                    return;
                }
            }
            if (ddlsession.SelectedItem.Text == "Morning")
                session = "1";
            else
                session = "2";
            string insq = "if exists(select * from Automatic_SMS where sms_purpose='" + smspurpose + "') update Automatic_SMS set sending_Time='" + sendtime.ToString("HH:mm:ss") + "',SendToHOff='" + sendtohigheroff + "',EmailId='" + sendmail + "',Send_Session='" + session + "',user_code='" + usercode + "' where sms_purpose='" + smspurpose + "'  else Insert into Automatic_SMS (sms_purpose,sending_Time,SendToHOff,EmailId,Send_Session,user_code) values ('" + smspurpose + "','" + sendtime.ToString("HH:mm:ss") + "','" + sendtohigheroff + "','" + sendmail + "','" + session + "','" + usercode + "')";
            inscount = d2.update_method_wo_parameter(insq, "Text");
            if (inscount > 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Settings Saved Successfully!";
                ddltype1.SelectedIndex = 9;
                ddltype1_change(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "SMS_ManagerSettings.aspx"); }

    }


    #endregion

    private DateTime getdate(string date)
    {
        DateTime myDt = new DateTime();
        try
        {
            string[] spldt = date.Split('/');
            myDt = Convert.ToDateTime(spldt[1] + "/" + spldt[0] + "/" + spldt[2]);
        }
        catch { }
        return myDt;
    }

    private void bindtestname()
    {
        try
        {
            ddltest.Items.Clear();
            ds.Clear();
            string selq = "select distinct criteria from CriteriaforInternal";
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddltest.DataSource = ds;
                ddltest.DataTextField = "criteria";
                ddltest.DataBind();
                ddltest.Items.Insert(0, "Select");
            }
            else
            {
                ddltest.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    private void bindstafftype()
    {
        try
        {
            ds.Clear();
            cblstftype.Items.Clear();
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + ddlcollege.SelectedItem.Value + "' order by stftype";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblstftype.DataSource = ds;
                cblstftype.DataTextField = "stftype";
                cblstftype.DataBind();
                if (cblstftype.Items.Count > 0)
                {
                    for (int i = 0; i < cblstftype.Items.Count; i++)
                    {
                        cblstftype.Items[i].Selected = true;
                    }
                    txtstftype.Text = "StaffType (" + cblstftype.Items.Count + ")";
                    cbstftype.Checked = true;
                }
            }
            else
            {
                txtstftype.Text = "--Select--";
                cbstftype.Checked = false;
            }
        }
        catch { }
    }

    private void bindnewstafftype()
    {
        try
        {
            ds.Clear();
            cblstftype.Items.Clear();
            string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + ddlcollege.SelectedItem.Value + "' order by stftype";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblstftype.DataSource = ds;
                cblstftype.DataTextField = "stftype";
                cblstftype.DataBind();
                if (cblstftype.Items.Count > 0)
                {
                    txtstftype.Text = "--Select--";
                    cbstftype.Checked = false;
                }
            }
            else
            {
                txtstftype.Text = "--Select--";
                cbstftype.Checked = false;
            }
        }
        catch { }
    }

    protected void category()
    {
        try
        {
            ds.Clear();
            cblstfcat.Items.Clear();
            string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + ddlcollege.SelectedItem.Value + "' order by category_Name";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblstfcat.DataSource = ds;
                cblstfcat.DataTextField = "category_Name";
                cblstfcat.DataValueField = "category_code";
                cblstfcat.DataBind();
                cblstfcat.Visible = true;
                if (cblstfcat.Items.Count > 0)
                {
                    for (int i = 0; i < cblstfcat.Items.Count; i++)
                    {
                        cblstfcat.Items[i].Selected = true;
                    }
                    txtstfcat.Text = "Category(" + cblstfcat.Items.Count + ")";
                    cbstfcat.Checked = true;
                }
            }
            else
            {
                txtstfcat.Text = "--Select--";
                cbstfcat.Checked = false;
            }
        }
        catch { }
    }

    protected void newcategory()
    {
        try
        {
            ds.Clear();
            cblstfcat.Items.Clear();
            string statequery = "select category_code,category_Name from staffcategorizer where college_code = '" + ddlcollege.SelectedItem.Value + "' order by category_Name";
            ds = d2.select_method_wo_parameter(statequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblstfcat.DataSource = ds;
                cblstfcat.DataTextField = "category_Name";
                cblstfcat.DataValueField = "category_code";
                cblstfcat.DataBind();
                cblstfcat.Visible = true;
                if (cblstfcat.Items.Count > 0)
                {
                    txtstfcat.Text = "--Select--";
                    cbstfcat.Checked = false;
                }
            }
            else
            {
                txtstfcat.Text = "--Select--";
                cbstfcat.Checked = false;
            }
        }
        catch { }
    }

    private void binddevname()
    {
        try
        {
            ds.Clear();
            cbldevname.Items.Clear();
            string selq = "select distinct DeviceID,DeviceName from DeviceInfo where DeviceName<>'' and DeviceName is not null and College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbldevname.DataSource = ds;
                cbldevname.DataTextField = "DeviceName";
                cbldevname.DataValueField = "DeviceID";
                cbldevname.DataBind();
                txtdevname.Text = "--Select--";
                cbdevname.Checked = false;
            }
            else
            {
                txtdevname.Text = "--Select--";
                cbdevname.Checked = false;
            }
        }
        catch { }
    }

    private void studHostelbind()
    {
        try
        {
            ds.Clear();
            cblhosname.Items.Clear();
            string selcol = "select hostelmasterpk,hostelname from HM_HostelMaster where collegecode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds = d2.select_method_wo_parameter(selcol, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblhosname.DataSource = ds;
                cblhosname.DataTextField = "hostelname";
                cblhosname.DataValueField = "hostelmasterpk";
                cblhosname.DataBind();
                txthosname.Text = "--Select--";
                cbhosname.Checked = false;
            }
            else
            {
                txthosname.Text = "--Select--";
                cbhosname.Checked = false;
            }
        }
        catch { }
    }

    private void binddept()
    {
        try
        {
            ds.Clear();
            ddldeptdown.Items.Clear();
            string selq = "select distinct dept_code,dept_name from hrdept_master where dept_name<>'' and dept_name is not null and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldeptdown.DataSource = ds;
                ddldeptdown.DataTextField = "dept_name";
                ddldeptdown.DataValueField = "dept_code";
                ddldeptdown.DataBind();
                ddldeptdown.Items.Insert(0, "All");
            }
            else
            {
                ddldeptdown.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    private void bindstafftypedown()
    {
        try
        {
            ds.Clear();
            ddlstftypedown.Items.Clear();
            string selq = "select distinct stftype from stafftrans where stftype<>'' and stftype is not null";
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlstftypedown.DataSource = ds;
                ddlstftypedown.DataTextField = "stftype";
                ddlstftypedown.DataValueField = "stftype";
                ddlstftypedown.DataBind();
                ddlstftypedown.Items.Insert(0, "All");
            }
            else
            {
                ddlstftypedown.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    private void bindstaffcatdown()
    {
        try
        {
            ds.Clear();
            ddlstfcatdown.Items.Clear();
            string selq = "select distinct category_name,category_code from staffcategorizer where category_name<>'' and category_name is not null and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlstfcatdown.DataSource = ds;
                ddlstfcatdown.DataTextField = "category_name";
                ddlstfcatdown.DataValueField = "category_code";
                ddlstfcatdown.DataBind();
                ddlstfcatdown.Items.Insert(0, "All");
            }
            else
            {
                ddlstfcatdown.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    private void bindshift()
    {
        try
        {
            ds.Clear();
            ddlshiftdown.Items.Clear();
            string selq = "select distinct Shift from in_out_time where Shift<>'' and Shift is not null and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlshiftdown.DataSource = ds;
                ddlshiftdown.DataTextField = "Shift";
                ddlshiftdown.DataValueField = "Shift";
                ddlshiftdown.DataBind();
                ddlshiftdown.Items.Insert(0, "All");
            }
            else
            {
                ddlshiftdown.Items.Insert(0, "Select");
            }
        }
        catch { }
    }

    protected void cbstftype_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cbstftype, cblstftype, txtstftype, "StaffType");
    }

    protected void cblstftype_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cbstftype, cblstftype, txtstftype, "StaffType");
    }

    protected void cbstfcat_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cbstfcat, cblstfcat, txtstfcat, "Category");
    }

    protected void cblstfcat_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cbstfcat, cblstfcat, txtstfcat, "Category");
    }

    protected void cbdevname_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cbdevname, cbldevname, txtdevname, "DeviceName");
    }

    protected void cbldevname_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cbdevname, cbldevname, txtdevname, "DeviceName");
    }

    protected void cbhosname_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cbhosname, cblhosname, txthosname, "HostelName");
    }

    protected void cblhosname_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cbhosname, cblhosname, txthosname, "HostelName");
    }

    private string GetSelectedItemsValueAsStringnew(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[j].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private string GetSelectedItemsTextnew(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[j].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                {
                    chkchange.Checked = true;
                }
            }
        }
        catch { }
    }


    protected void rdb_Sms_Check(object sender, EventArgs e)
    {
        try
        {
            rdb_Sms.Checked = true;
            rdb_Mail.Checked = false;
            ddltype.Visible = true;
            ddltype1.Visible = false;
            ddltype.SelectedIndex = 0;
            ddltype_change(sender, e);
        }
        catch
        {
        }


    }

    protected void rdb_Mail_Click(object sender, EventArgs e)
    {
        try
        {
            rdb_Sms.Checked = false;
            rdb_Mail.Checked = true;
            ddltype1.Visible = true;
            ddltype.Visible = false;
            ddltype1.SelectedIndex = 0;
            ddltype1_change(sender, e);
        }
        catch
        {
        }

    }


    protected void rdohour_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            rdodaily.Checked = false;
            hourwise.Visible = true;
            loadperiods();
            daywise.Visible = false;
            Checkhrdaysend.Visible = true;
        }
        catch
        {

        }

    }

    protected void rdodaily_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            hourwise.Visible = false;
            rdohour.Checked = false;
            daywise.Visible = true;
            hourwise.Visible = false;
            Checkhrdaysend.Visible = true;
        }
        catch
        {

        }

    }
    public void loadperiods()
    {
        cbl_hour.Items.Clear();
        txt_hour.Text = "--Select--";
        int hour = int.Parse(d2.GetFunction("select MAX(no_of_hrs_per_day) from PeriodAttndSchedule"));

        if (hour > 0)
        {
            for (int i = 1; i <= hour; i++)
            {
                cbl_hour.Items.Add(i.ToString());
            }

        }


    }

    public void cb_hour_checkedchange(object sender, EventArgs e)
    {
        if (cb_hour.Checked == true)
        {
            for (int i = 0; i < cbl_hour.Items.Count; i++)
            {
                cbl_hour.Items[i].Selected = true;
            }
            txt_hour.Text = "Hour(" + cbl_hour.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hour.Items.Count; i++)
            {
                cbl_hour.Items[i].Selected = false;
            }
            txt_hour.Text = "--Select--";
        }

    }
    public void cbl_hour_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_hour.Text = "--Select--";
        cb_hour.Checked = false;
        int ccount = 0;
        for (int i = 0; i < cbl_hour.Items.Count; i++)
        {
            if (cbl_hour.Items[i].Selected == true)
            {
                ccount = ccount + 1;
                cb_hour.Checked = false;
            }
        }
        if (ccount > 0)
        {
            txt_hour.Text = "Hour(" + ccount.ToString() + ")";
            if (ccount == cbl_hour.Items.Count)
            {
                cb_hour.Checked = true;
            }

        }

    }

    protected void rbldayType_Selected(object sender, EventArgs e)
    {

    }
}

#region CheckWinService

//public void getcheck()
//{
//    try
//    {
//        getdivi();
//        return;
//        DataSet ds = new DataSet();
//        int i = 0;
//        string studwish = "";
//        string staffwish = "";
//        string studMob = "";
//        string FatMob = "";
//        string MotMob = "";
//        string getval = "";
//        string collcode = "";
//        string smspurpose = "";
//        string usercode = "";
//        string grpbyhos = "";
//        string inclongabs = "";
//        string inclongabscount = "";
//        string session = "";
//        string withlev = "";
//        string withgrpby = "";
//        string grptype = "";
//        string isgrplst = "";
//        string grplst = "";
//        string strgetmsg = "";
//        string senttohod = "";
//        string senttohoff = "";
//        string mobileno = "";
//        string testname = "";
//        DateTime scheduletime = new DateTime();
//        DateTime senddate = new DateTime();
//        string newscheduletime = "";
//        string smssenddt = "";
//        string senttostaff = "";
//        string senttostudent = "";
//        string devname = "";
//        string hosname = "";
//        string dept = "";
//        string shift = "";
//        string morabs = "";
//        string eveabs = "";
//        DateTime fromdate = new DateTime();
//        DateTime todate = new DateTime();
//        List<string> lstdt = new List<string>();
//        Dictionary<string, string> dict = new Dictionary<string, string>();

//        lstdt.Clear();
//        dict.Clear();
//        lstdt = getdtlst();
//        dict = getdicval();
//        string[] splval = new string[2];
//        string syscurrtime = string.Format("{0:t}", DateTime.Now);
//        DateTime sysnewcurrtime = DateTime.Now;

//        for (i = 0; i < lstdt.Count; i++)
//        {
//            strgetmsg = "";
//            ds.Clear();
//            if (dict.ContainsKey(lstdt[i]))
//            {
//                getval = Convert.ToString(dict[lstdt[i]]);
//                splval = getval.Split('-');
//                if (splval.Length >= 3)
//                {
//                    scheduletime = Convert.ToDateTime(splval[0]);
//                    newscheduletime = string.Format("{0:t}", scheduletime);
//                    collcode = Convert.ToString(splval[1]);
//                    smspurpose = Convert.ToString(splval[2]);

//                    if (smspurpose == "Automatic Download And Mark Time Attendance Settings")
//                    {
//                        ds = getRecord(smspurpose, collcode);
//                        senttostudent = Convert.ToString(ds.Tables[0].Rows[0]["SendToStud"]);
//                        senttostaff = Convert.ToString(ds.Tables[0].Rows[0]["SendToStaff"]);
//                        devname = Convert.ToString(ds.Tables[0].Rows[0]["DeviceID"]);
//                        hosname = Convert.ToString(ds.Tables[0].Rows[0]["hostelmasterpk"]);
//                        grptype = Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupType"]);
//                        grplst = Convert.ToString(ds.Tables[0].Rows[0]["StfAttnGroupList"]);
//                        dept = Convert.ToString(ds.Tables[0].Rows[0]["dept_code"]);
//                        shift = Convert.ToString(ds.Tables[0].Rows[0]["shift"]);
//                        morabs = Convert.ToString(ds.Tables[0].Rows[0]["MorAbs"]);
//                        eveabs = Convert.ToString(ds.Tables[0].Rows[0]["EveAbs"]);
//                        fromdate = Convert.ToDateTime(ds.Tables[0].Rows[0]["From_Date"]);
//                        todate = Convert.ToDateTime(ds.Tables[0].Rows[0]["To_Date"]);
//                        usercode = Convert.ToString(ds.Tables[0].Rows[0]["user_code"]);
//                        if (senttostaff.Trim().ToUpper() == "TRUE" || senttostudent.Trim().ToUpper() == "TRUE")
//                        {
//                            if (devname.Trim() != "")
//                            {
//                                devname = "'" + devname + "'";
//                                try
//                                {
//                                    downloadlogs(devname, fromdate, todate, collcode);
//                                }
//                                catch { }
//                            }
//                        }
//                        if (senttostaff.Trim().ToUpper() == "TRUE")
//                        {
//                            try
//                            {
//                                markattdstaff(grptype, grplst, morabs, eveabs, dept, shift, fromdate, todate, collcode);
//                            }
//                            catch (Exception ex)
//                            {
//                                alertpopwindow.Visible = true;
//                                lblalerterr.Visible = true;
//                                lblalerterr.Text = ex.StackTrace;
//                            }
//                        }
//                        //if (senttostudent.Trim().ToUpper() == "TRUE")
//                        //{
//                        //    if (hosname.Trim() != "")
//                        //    {
//                        //        hosname = "'" + hosname + "'";
//                        //        try
//                        //        {
//                        //            markattdstudent(morabs, eveabs, hosname, fromdate, todate, collcode);
//                        //        }
//                        //        catch (Exception ex)
//                        //        {
//                        //            alertpopwindow.Visible = true;
//                        //            lblalerterr.Visible = true;
//                        //            lblalerterr.Text = ex.StackTrace;
//                        //        }
//                        //    }
//                        //    else
//                        //    {
//                        //        alertpopwindow.Visible = true;
//                        //        lblalerterr.Visible = true;
//                        //        lblalerterr.Text = "Please Select Any Hostel Name";
//                        //    }
//                        //}
//                    }
//                }
//            }
//        }
//    }
//    catch { }
//}

//public void getdivi()
//{
//    try
//    {
//        int i = 1;
//        int k = 0;
//        int j = Convert.ToInt32(i / k);
//    }
//    catch (Exception ex)
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = ex.Message;
//    }
//}

//public void markattdstaff(string grptype, string grplst, string morabs, string eveabs, string deptcode, string shift, DateTime frmdt, DateTime todt, string collcode)
//{
//    int savecount = 0;
//    int mysavecount = 0;
//    string selq = "";
//    DataSet ds1 = new DataSet();
//    DataSet ds2 = new DataSet();
//    DataView dv = new DataView();

//    string currtime = DateTime.Now.ToString("MM/dd/yyyy");
//    DateTime currdt = Convert.ToDateTime(currtime);
//    if (frmdt > todt)
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = "From Date Should be less than To Date!";
//        return;
//    }
//    if (frmdt > currdt || todt > currdt)
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = "Not Allowed For Future Date!";
//        return;
//    }
//    selq = "select s.staff_code,t.category_code,stftype,Shift,Fingerprint1 from staffmaster s,stafftrans t where t.staff_code =s.staff_code and s.settled=0 and s.resign=0 and ISNULL(Discontinue,'0')='0' and t.latestrec='1' and s.college_code='" + Convert.ToString(collcode) + "'";

//    if (!String.IsNullOrEmpty(deptcode) && deptcode.Trim() != "All")
//        selq = selq + " and t.dept_code='" + Convert.ToString(deptcode) + "'";

//    if (grptype.Trim() == "2" && !String.IsNullOrEmpty(grplst) && grplst.Trim() != "All")
//        selq = selq + " and t.category_code='" + Convert.ToString(grplst) + "'";

//    if (grptype.Trim() == "1" && !String.IsNullOrEmpty(grplst) && grplst.Trim() != "All")
//        selq = selq + " and t.stftype='" + Convert.ToString(grplst) + "'";

//    if (!String.IsNullOrEmpty(shift) && shift.Trim() != "All")
//        selq = selq + " and t.Shift='" + Convert.ToString(shift) + "'";

//    selq = selq + "  order by s.staff_code";

//    selq = selq + " select Convert(varchar(10),Log_Date,101) as Log_Date,InTime,outtime,FingerID from attn_logs where Log_date>='" + frmdt.ToString("MM/dd/yyyy") + "' and Log_date<='" + todt.ToString("MM/dd/yyyy") + "'  order by Log_date,FingerID,intime";

//    ds1.Clear();
//    ds1 = d2.select_method_wo_parameter(selq, "Text");
//    if (ds1.Tables.Count > 0 && ds1.Tables[1].Rows.Count > 0)
//    {
//        for (int jk = 0; jk < ds1.Tables[1].Rows.Count; jk++)
//        {
//            ds1.Tables[0].DefaultView.RowFilter = " Fingerprint1 like '" + Convert.ToString(ds1.Tables[1].Rows[jk]["FingerID"]) + "'";
//            dv = ds1.Tables[0].DefaultView;
//            if (dv.Count > 0)
//            {
//                SaveAttendance(Convert.ToString(dv[0]["staff_code"]), Convert.ToString(dv[0]["category_code"]), Convert.ToString(dv[0]["stftype"]), Convert.ToString(dv[0]["Shift"]), Convert.ToDateTime(ds1.Tables[1].Rows[jk]["Log_date"]).ToString("MM/dd/yyyy"), Convert.ToString(ds1.Tables[1].Rows[jk]["InTime"]), Convert.ToString(ds1.Tables[1].Rows[jk]["outtime"]), Convert.ToString(ds1.Tables[1].Rows[jk]["FingerID"]), collcode, grptype, morabs, eveabs, out savecount);
//                mysavecount = mysavecount + savecount;
//            }
//        }
//    }
//    if (mysavecount > 0)
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = "Download and Mark the Attendance for Staff Successfully!";
//    }
//    else
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = "Download UnSuccessfully,Please Try Again!";
//    }
//}

//public void SaveAttendance(string staffcode, string category, string stftype, string shift, string logdate, string intime, string outtime, string FingerID, string collcode, string grptype, string morabs, string eveabs, out int scount)
//{
//    scount = 0;
//    string selq = "";
//    string attnd = "";
//    string getsetstr1 = "";
//    string getsetstr2 = "";
//    string latefrom = "";
//    string lateto = "";
//    string perfrom = "";
//    string perto = "";
//    string inTime = "";
//    string outTime = "";
//    string[] splattnd = new string[2];
//    DataSet dsinout = new DataSet();
//    DataSet dscheck = new DataSet();
//    DateTime dtsample = new DateTime();
//    TimeSpan InTime = new TimeSpan();
//    TimeSpan TempInTime = new TimeSpan();
//    TimeSpan GraceTime = new TimeSpan();
//    TimeSpan PerFrom = new TimeSpan();
//    TimeSpan PerTo = new TimeSpan();
//    TimeSpan LateFrom = new TimeSpan();
//    TimeSpan LateTo = new TimeSpan();
//    TimeSpan OutTime = new TimeSpan();
//    TimeSpan TempOutTime = new TimeSpan();
//    TimeSpan mornouttime = new TimeSpan();
//    TimeSpan extendgracetime = new TimeSpan();
//    TimeSpan LateTime = new TimeSpan();
//    TimeSpan LunchEndTime = new TimeSpan();
//    TimeSpan dsOutTime = new TimeSpan();
//    TimeSpan perTime = new TimeSpan();
//    DataSet dscnt = new DataSet();
//    string mornval = "";
//    string evenval = "";
//    int hour = 0;
//    int minute = 0;
//    int paytype = 0;
//    int latecnt = 0;
//    int mypercnt = 0;
//    int nooflate = 0;
//    int intLATak = 0;
//    int intPERTak = 0;
//    int noofper = 0;
//    string apyleve = "";

//    int dtd = Convert.ToDateTime(logdate).Day;
//    int dtm = Convert.ToDateTime(logdate).Month;
//    int dty = Convert.ToDateTime(logdate).Year;
//    string monandYear = dtm + "/" + dty;

//    if (category.Trim() != "")
//    {
//        selq = "select convert(varchar(10),time_in,101) as time_in,convert(varchar(10),time_out,101) as time_out from  bio_attendance where roll_no='" + staffcode + "' and access_date='" + logdate + "' and latestrec=1";
//        dscheck.Clear();
//        dscheck = d2.select_method_wo_parameter(selq, "text");

//        if (dscheck.Tables[0].Rows.Count == 0)
//        {
//            if (intime == outtime)
//            {
//                attnd = "";
//                if (category.Trim() != "" || stftype.Trim() != "")
//                {
//                    selq = "";
//                    //Bell_Date check
//                    if (grptype == "2")
//                        selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and Bell_Date='" + logdate + "'";
//                    else
//                        selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and Bell_Date='" + logdate + "'";
//                    dsinout.Clear();
//                    dsinout = d2.select_method_wo_parameter(selq, "Text");
//                    if (dsinout.Tables[0].Rows.Count == 0)
//                    {
//                        //Bell_Day check
//                        if (grptype == "2")
//                            selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and upper(Bell_Day) ='" + Convert.ToDateTime(logdate).ToString("dddd") + "'";
//                        else
//                            selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and upper(Bell_Day) ='" + Convert.ToDateTime(logdate).ToString("dddd") + "'";
//                        dsinout.Clear();
//                        dsinout = d2.select_method_wo_parameter(selq, "Text");
//                        if (dsinout.Tables[0].Rows.Count == 0)
//                        {
//                            if (grptype == "2")
//                                selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND DayType = 0 AND College_Code =" + Convert.ToString(collcode) + "";
//                            else
//                                selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND DayType = 0 AND College_Code =" + Convert.ToString(collcode) + "";

//                            dsinout.Clear();
//                            dsinout = d2.select_method_wo_parameter(selq, "Text");
//                        }
//                    }
//                    if (dsinout.Tables.Count > 0 && dsinout.Tables[0].Rows.Count > 0)
//                    {
//                        #region

//                        if (Convert.ToString(dsinout.Tables[0].Rows[0]["manual_Settings"]) == "1" && Convert.ToString(dsinout.Tables[0].Rows[0]["morn_late"]) != "" && Convert.ToString(dsinout.Tables[0].Rows[0]["morn_per"]) != "")
//                        {
//                            #region Morning late values

//                            //intime and out time- in_out_table
//                            string mornintme = Convert.ToString(dsinout.Tables[0].Rows[0]["intime"]);
//                            if (!string.IsNullOrEmpty(mornintme))
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["intime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["intime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                inTime = dtsample.ToString("hh:mm tt");
//                            }

//                            //out time
//                            string eventime = Convert.ToString(dsinout.Tables[0].Rows[0]["outtime"]);
//                            if (!string.IsNullOrEmpty(eventime))
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["outtime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["outtime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                outTime = dtsample.ToString("hh:mm tt");
//                            }

//                            splattnd = Convert.ToString(dsinout.Tables[0].Rows[0]["morn_late"]).Split('-');
//                            getsetstr1 = "";
//                            getsetstr1 = splattnd[0];
//                            getsetstr2 = "";
//                            getsetstr2 = splattnd[1];
//                            latefrom = "";
//                            hour = 0;
//                            minute = 0;
//                            if (getsetstr1 == "Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                latefrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Extend Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                latefrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Late Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                latefrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Lunch Start Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                latefrom = dtsample.ToString("hh:mm tt");
//                            }
//                            lateto = "";
//                            hour = 0;
//                            minute = 0;
//                            if (getsetstr2 == "Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                lateto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Extend Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                lateto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Late Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                lateto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Lunch Start Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                lateto = dtsample.ToString("hh:mm tt");
//                            }

//                            #endregion

//                            #region Morning Permission

//                            splattnd = Convert.ToString(dsinout.Tables[0].Rows[0]["morn_per"]).Split('-');
//                            getsetstr1 = "";
//                            getsetstr1 = splattnd[0];
//                            getsetstr2 = "";
//                            getsetstr2 = splattnd[1];
//                            perfrom = "";
//                            hour = 0;
//                            minute = 0;
//                            if (getsetstr1 == "Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perfrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Extend Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perfrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Late Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perfrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Lunch Start Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perfrom = dtsample.ToString("hh:mm tt");
//                            }
//                            perto = "";
//                            hour = 0;
//                            minute = 0;
//                            if (getsetstr2 == "Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Extend Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Late Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Lunch Start Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perto = dtsample.ToString("hh:mm tt");
//                            }

//                            #endregion

//                            //intime
//                            InTime = Convert.ToDateTime(intime).TimeOfDay;

//                            TempInTime = Convert.ToDateTime(inTime).TimeOfDay;
//                            GraceTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"])).TimeOfDay;
//                            PerFrom = Convert.ToDateTime(perfrom).TimeOfDay;
//                            PerTo = Convert.ToDateTime(perto).TimeOfDay;
//                            LateFrom = Convert.ToDateTime(latefrom).TimeOfDay;
//                            LateTo = Convert.ToDateTime(lateto).TimeOfDay;
//                            //out time
//                            if (outtime != "")
//                                OutTime = Convert.ToDateTime(outtime).TimeOfDay;

//                            TempOutTime = Convert.ToDateTime(outTime).TimeOfDay;

//                            if (InTime <= GraceTime)
//                            {
//                                if (eveabs.Trim().ToUpper() == "TRUE")
//                                    attnd = "P-A";
//                                else
//                                    attnd = "P-";
//                            }
//                            else if (PerTo > InTime && InTime > PerFrom)
//                            {
//                                LateSettings(staffcode, logdate, intime, latefrom, lateto, category, collcode);
//                                if (Convert.ToString(dsinout.Tables[0].Rows[0]["noofper"]) == "0" || String.IsNullOrEmpty(Convert.ToString(dsinout.Tables[0].Rows[0]["noofper"])))
//                                {
//                                    Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["IsCalPayDate"]), out paytype);
//                                    Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["nooflate"]), out nooflate);
//                                    Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["noofper"]), out noofper);
//                                    apyleve = Convert.ToString(dsinout.Tables[0].Rows[0]["ApplyLeave"]);

//                                    if (InTime > LateFrom && InTime <= LateTo)
//                                    {
//                                        if (Convert.ToString(dsinout.Tables[0].Rows[0]["nooflate"]) == "0" || String.IsNullOrEmpty(Convert.ToString(dsinout.Tables[0].Rows[0]["nooflate"])))
//                                        {
//                                            if (eveabs.Trim().ToUpper() == "TRUE")
//                                                attnd = "A-A";
//                                            else
//                                                attnd = "A-";
//                                        }
//                                        else if (GetLateCount(staffcode, logdate, paytype, out intLATak, collcode) >= nooflate)
//                                        {
//                                            if (apyleve == "A")
//                                            {
//                                                if (eveabs.Trim().ToUpper() == "TRUE")
//                                                    attnd = "A-A";
//                                                else
//                                                    attnd = "A-";
//                                            }
//                                            else
//                                            {
//                                                if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                                {
//                                                    if (eveabs.Trim().ToUpper() == "TRUE")
//                                                        attnd = "A-A";
//                                                    else
//                                                        attnd = "A-";
//                                                }
//                                                else
//                                                {
//                                                    if (eveabs.Trim().ToUpper() == "TRUE")
//                                                        attnd = apyleve + "-" + "A";
//                                                    else
//                                                        attnd = apyleve + "-";
//                                                }
//                                            }
//                                        }
//                                        else
//                                        {
//                                            if (eveabs.Trim().ToUpper() == "TRUE")
//                                                attnd = "LA-A";
//                                            else
//                                                attnd = "LA-";
//                                            latecnt = 1;
//                                        }
//                                    }
//                                    else if (GetPermissionCount(staffcode, logdate, paytype, out intPERTak, collcode) >= noofper)
//                                    {
//                                        if (apyleve == "A")
//                                        {
//                                            if (eveabs.Trim().ToUpper() == "TRUE")
//                                                attnd = "A-A";
//                                            else
//                                                attnd = "A-";
//                                        }
//                                        else
//                                        {
//                                            if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                            {
//                                                if (eveabs.Trim().ToUpper() == "TRUE")
//                                                    attnd = "A-A";
//                                                else
//                                                    attnd = "A-";
//                                            }
//                                            else
//                                            {
//                                                if (eveabs.Trim().ToUpper() == "TRUE")
//                                                    attnd = apyleve + "-" + "A";
//                                                else
//                                                    attnd = apyleve + "-";
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        if (eveabs.Trim().ToUpper() == "TRUE")
//                                            attnd = "PER-A";
//                                        else
//                                            attnd = "PER-";
//                                        mypercnt = 1;
//                                    }
//                                }
//                            }
//                            else if (InTime >= Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"])).TimeOfDay)
//                            {
//                                if (eveabs.Trim().ToUpper() == "TRUE")
//                                    attnd = "A-A";
//                                else
//                                    attnd = "A-";

//                            }
//                        }

//                        //attendance save
//                        if (!string.IsNullOrEmpty(attnd))
//                        {
//                            string Query = " if exists(select * from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monandYear + "' ) update staff_attnd set [" + dtd + "]='" + attnd + "',latecount='" + latecnt + "',permissioncount='" + mypercnt + "' where staff_code='" + staffcode + "' and mon_year='" + monandYear + "' else insert into staff_attnd(staff_code,mon_year,[" + dtd + "]) values('" + staffcode + "','" + monandYear + "','" + attnd + "')";
//                            Query += " if exists(select * from Bio_Attendance where roll_no='" + staffcode + "' and is_staff='1' and access_date='" + logdate + "') update  Bio_Attendance set time_in='" + InTime + "',time_out='" + OutTime + "',att='" + attnd + "' where roll_no='" + staffcode + "' and is_staff='1' and access_date='" + logdate + "'  else insert into Bio_Attendance(roll_no,time_in,time_out,is_staff,access_date,latestrec,att,mark_time) values('" + staffcode + "','" + InTime + "','" + OutTime + "','1','" + logdate + "','1','" + attnd + "','" + DateTime.Now.ToString("hh:mm tt") + "')";

//                            int upd = d2.update_method_wo_parameter(Query, "Text");
//                            if (upd > 0)
//                            {
//                                scount++;
//                            }
//                        }
//                        #endregion
//                    }
//                }
//            }
//            else if (intime != outtime)
//            {
//                mornval = "";
//                attnd = "";
//                if (category.Trim() != "" || stftype.Trim() != "")
//                {
//                    selq = "";
//                    //Bell_Date check
//                    if (grptype.Trim() == "2")
//                        selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and Bell_Date='" + logdate + "'";
//                    else
//                        selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and Bell_Date='" + logdate + "'";
//                    dsinout.Clear();
//                    dsinout = d2.select_method_wo_parameter(selq, "Text");
//                    if (dsinout.Tables[0].Rows.Count == 0)
//                    {
//                        //Bell_Day check
//                        if (grptype.Trim() == "2")
//                            selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and upper(Bell_Day) ='" + Convert.ToDateTime(logdate).ToString("dddd") + "'";
//                        else
//                            selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and upper(Bell_Day) ='" + Convert.ToDateTime(logdate).ToString("dddd") + "'";
//                        dsinout.Clear();
//                        dsinout = d2.select_method_wo_parameter(selq, "Text");
//                        if (dsinout.Tables[0].Rows.Count == 0)
//                        {
//                            if (grptype.Trim() == "2")
//                                selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND DayType = 0 AND College_Code =" + Convert.ToString(collcode) + "";
//                            else
//                                selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND DayType = 0 AND College_Code =" + Convert.ToString(collcode) + "";

//                            dsinout.Clear();
//                            dsinout = d2.select_method_wo_parameter(selq, "Text");
//                        }
//                    }
//                    if (dsinout.Tables.Count > 0 && dsinout.Tables[0].Rows.Count > 0)
//                    {
//                        mornouttime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["MorningOutTime"])).TimeOfDay;
//                        GraceTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"])).TimeOfDay;
//                        extendgracetime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"])).TimeOfDay;
//                        LateTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"])).TimeOfDay;
//                        InTime = Convert.ToDateTime(intime).TimeOfDay;
//                        OutTime = Convert.ToDateTime(outtime).TimeOfDay;
//                        Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["noofper"]), out noofper);
//                        Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["nooflate"]), out nooflate);
//                        apyleve = Convert.ToString(dsinout.Tables[0].Rows[0]["ApplyLeave"]);
//                        Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["IsCalPayDate"]), out paytype);

//                        string[] mval = Convert.ToString(d2.GetFunction("select [" + dtd + "] from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monandYear + "'")).Split('-');
//                        if (mval.Length == 2)
//                        {
//                            mornval = mval[0] + "-";
//                            evenval = mval[1];
//                        }
//                        else
//                        {
//                            if (mornval == "")
//                                mornval = "-";
//                        }
//                        if (String.IsNullOrEmpty(Convert.ToString(dsinout.Tables[0].Rows[0]["manual_Settings"])) || Convert.ToString(dsinout.Tables[0].Rows[0]["manual_Settings"]) != "1")
//                        {
//                            if (OutTime < mornouttime)
//                            {
//                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                    attnd = "A-";
//                                else
//                                    attnd = mornval;
//                            }
//                            else
//                            {
//                                if (InTime <= GraceTime)
//                                {
//                                    if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                        attnd = "P-";
//                                    else
//                                        attnd = mornval;
//                                    if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                    {

//                                    }
//                                    else
//                                        attnd = attnd + evenval;
//                                }
//                                else if (InTime > GraceTime && InTime <= extendgracetime)
//                                {
//                                    if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                        attnd = "OOD-";
//                                    else
//                                        attnd = mornval;
//                                    if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                    {

//                                    }
//                                    else
//                                        attnd = attnd + evenval;
//                                }
//                                else if (LateTime > InTime && InTime > extendgracetime)
//                                {
//                                    if (noofper == 0)
//                                    {
//                                        if (apyleve == "A")
//                                        {
//                                            if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                attnd = "A-";
//                                            else
//                                                attnd = mornval;
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            {

//                                            }
//                                            else
//                                                attnd = attnd + evenval;
//                                        }
//                                        else
//                                        {
//                                            if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = "A-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                            else
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = apyleve + "-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                        }
//                                        if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                            attnd = "A-";
//                                        else
//                                            attnd = mornval;
//                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                        {

//                                        }
//                                        else
//                                            attnd = attnd + evenval;
//                                    }
//                                    else if (GetPermissionCount(staffcode, logdate, paytype, out intPERTak, collcode) >= noofper)
//                                    {
//                                        if (apyleve == "A")
//                                        {
//                                            if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                attnd = "A-";
//                                            else
//                                                attnd = mornval;
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            {

//                                            }
//                                            else
//                                                attnd = attnd + evenval;
//                                        }
//                                        else
//                                        {
//                                            if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = "A-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                            else
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = apyleve + "-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                            attnd = "PER-";
//                                        else
//                                            attnd = mornval;
//                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                        {

//                                        }
//                                        else
//                                            attnd = attnd + evenval;
//                                        mypercnt = 1;
//                                    }
//                                }
//                                else if (InTime >= LateTime)
//                                {
//                                    if (nooflate == 0)
//                                    {
//                                        if (apyleve == "A")
//                                        {
//                                            if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                attnd = "A-";
//                                            else
//                                                attnd = mornval;
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            {

//                                            }
//                                            else
//                                                attnd = attnd + evenval;
//                                        }
//                                        else
//                                        {
//                                            if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = "A-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                            else
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = apyleve + "-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                        }
//                                        if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                            attnd = "A-";
//                                        else
//                                            attnd = mornval;
//                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                        {

//                                        }
//                                        else
//                                            attnd = attnd + evenval;
//                                    }
//                                    else if (GetLateCount(staffcode, logdate, paytype, out intLATak, collcode) >= nooflate)
//                                    {
//                                        if (apyleve == "A")
//                                        {
//                                            if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                attnd = "A-";
//                                            else
//                                                attnd = mornval;
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            {

//                                            }
//                                            else
//                                                attnd = attnd + evenval;
//                                        }
//                                        else
//                                        {
//                                            if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = "A-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                            else
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = apyleve + "-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                            attnd = "LA-";
//                                        else
//                                            attnd = mornval;
//                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                        {

//                                        }
//                                        else
//                                            attnd = attnd + evenval;
//                                        latecnt = 1;
//                                    }
//                                }
//                                else if (InTime >= OutTime)
//                                {
//                                    attnd = mornval + "P";
//                                }
//                            }
//                        }
//                        else if (Convert.ToString(dsinout.Tables[0].Rows[0]["manual_Settings"]) == "1" && Convert.ToString(dsinout.Tables[0].Rows[0]["morn_late"]) != "" && Convert.ToString(dsinout.Tables[0].Rows[0]["morn_per"]) != "")
//                        {
//                            string mornintme = Convert.ToString(dsinout.Tables[0].Rows[0]["intime"]);
//                            if (!string.IsNullOrEmpty(mornintme))
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["intime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["intime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                inTime = dtsample.ToString("hh:mm tt");
//                            }

//                            //out time
//                            string eventime = Convert.ToString(dsinout.Tables[0].Rows[0]["outtime"]);
//                            if (!string.IsNullOrEmpty(eventime))
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["outtime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["outtime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                outTime = dtsample.ToString("hh:mm tt");
//                            }

//                            splattnd = Convert.ToString(dsinout.Tables[0].Rows[0]["morn_late"]).Split('-');
//                            getsetstr1 = "";
//                            getsetstr1 = splattnd[0];
//                            getsetstr2 = "";
//                            getsetstr2 = splattnd[1];
//                            latefrom = "";
//                            hour = 0;
//                            minute = 0;
//                            if (getsetstr1 == "Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                latefrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Extend Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                latefrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Late Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                latefrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Lunch Start Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                latefrom = dtsample.ToString("hh:mm tt");
//                            }
//                            lateto = "";
//                            hour = 0;
//                            minute = 0;
//                            if (getsetstr2 == "Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                lateto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Extend Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                lateto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Late Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                lateto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Lunch Start Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                lateto = dtsample.ToString("hh:mm tt");
//                            }

//                            #region Morning Permission

//                            splattnd = Convert.ToString(dsinout.Tables[0].Rows[0]["morn_per"]).Split('-');
//                            getsetstr1 = "";
//                            getsetstr1 = splattnd[0];
//                            getsetstr2 = "";
//                            getsetstr2 = splattnd[1];
//                            perfrom = "";
//                            hour = 0;
//                            minute = 0;
//                            if (getsetstr1 == "Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perfrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Extend Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perfrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Late Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perfrom = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr1 == "Lunch Start Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perfrom = dtsample.ToString("hh:mm tt");
//                            }
//                            perto = "";
//                            hour = 0;
//                            minute = 0;
//                            if (getsetstr2 == "Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Extend Grace Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Late Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perto = dtsample.ToString("hh:mm tt");
//                            }
//                            else if (getsetstr2 == "Lunch Start Time")
//                            {
//                                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["lunch_st_time"]));
//                                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                                perto = dtsample.ToString("hh:mm tt");
//                            }

//                            #endregion

//                            //intime
//                            TempInTime = Convert.ToDateTime(inTime).TimeOfDay;
//                            PerFrom = Convert.ToDateTime(perfrom).TimeOfDay;
//                            PerTo = Convert.ToDateTime(perto).TimeOfDay;
//                            LateFrom = Convert.ToDateTime(latefrom).TimeOfDay;
//                            LateTo = Convert.ToDateTime(lateto).TimeOfDay;
//                            //out time
//                            TempOutTime = Convert.ToDateTime(outTime).TimeOfDay;

//                            if (OutTime < mornouttime)
//                                attnd = "A-";
//                            else
//                            {
//                                if (InTime <= GraceTime)
//                                {
//                                    if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                        attnd = "P-";
//                                    else
//                                        attnd = mornval;
//                                    if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                    {

//                                    }
//                                    else
//                                        attnd = attnd + evenval;
//                                }
//                                else if (PerTo > InTime && InTime > PerFrom)
//                                {
//                                    if (noofper == 0)
//                                    {
//                                        if (InTime > LateFrom && InTime <= LateTo)
//                                        {
//                                            LateSettings(staffcode, logdate, intime, latefrom, lateto, category, collcode);
//                                            if (nooflate == 0)
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = "A-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                            else if (GetLateCount(staffcode, logdate, paytype, out intLATak, collcode) >= nooflate)
//                                            {
//                                                if (apyleve == "A")
//                                                {
//                                                    if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                        attnd = "A-";
//                                                    else
//                                                        attnd = mornval;
//                                                    if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                    {

//                                                    }
//                                                    else
//                                                        attnd = attnd + evenval;
//                                                }
//                                                else
//                                                {
//                                                    if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                                    {
//                                                        if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                            attnd = "A-";
//                                                        else
//                                                            attnd = mornval;
//                                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                        {

//                                                        }
//                                                        else
//                                                            attnd = attnd + evenval;
//                                                    }
//                                                    else
//                                                    {
//                                                        if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                            attnd = apyleve + "-";
//                                                        else
//                                                            attnd = mornval;
//                                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                        {

//                                                        }
//                                                        else
//                                                            attnd = attnd + evenval;
//                                                    }
//                                                }
//                                            }
//                                            else
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = "LA-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                                latecnt = 1;
//                                            }
//                                        }
//                                        else if (InTime >= LateTime)
//                                        {
//                                            if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                attnd = "A-";
//                                            else
//                                                attnd = mornval;
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            {

//                                            }
//                                            else
//                                                attnd = attnd + evenval;
//                                        }
//                                        else
//                                        {
//                                            if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                attnd = "A-";
//                                            else
//                                                attnd = mornval;
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            {

//                                            }
//                                            else
//                                                attnd = attnd + evenval;
//                                        }
//                                    }
//                                    else
//                                    {
//                                        if (GetPermissionCount(staffcode, logdate, paytype, out intPERTak, collcode) >= noofper)
//                                        {
//                                            if (apyleve == "A")
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = "A-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                            else
//                                            {
//                                                if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                                {
//                                                    if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                        attnd = "A-";
//                                                    else
//                                                        attnd = mornval;
//                                                    if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                    {

//                                                    }
//                                                    else
//                                                        attnd = attnd + evenval;
//                                                }
//                                                else
//                                                {
//                                                    if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                        attnd = apyleve + "-";
//                                                    else
//                                                        attnd = mornval;
//                                                    if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                    {

//                                                    }
//                                                    else
//                                                        attnd = attnd + evenval;
//                                                }
//                                            }
//                                        }
//                                        else
//                                        {
//                                            if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                attnd = "PER-";
//                                            else
//                                                attnd = mornval;
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            {

//                                            }
//                                            else
//                                                attnd = attnd + evenval;
//                                            mypercnt = 1;
//                                        }
//                                    }
//                                }
//                                else if (InTime > LateFrom && InTime <= LateTo)
//                                {
//                                    LateSettings(staffcode, logdate, intime, latefrom, lateto, category, collcode);
//                                    if (nooflate == 0)
//                                    {
//                                        if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                            attnd = "A-";
//                                        else
//                                            attnd = mornval;
//                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                        {

//                                        }
//                                        else
//                                            attnd = attnd + evenval;
//                                    }
//                                    else if (GetLateCount(staffcode, logdate, paytype, out intLATak, collcode) >= nooflate)
//                                    {
//                                        if (apyleve == "A")
//                                        {
//                                            if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                attnd = "A-";
//                                            else
//                                                attnd = mornval;
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            {

//                                            }
//                                            else
//                                                attnd = attnd + evenval;
//                                        }
//                                        else
//                                        {
//                                            if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = "A-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                            else
//                                            {
//                                                if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                                    attnd = apyleve + "-";
//                                                else
//                                                    attnd = mornval;
//                                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                {

//                                                }
//                                                else
//                                                    attnd = attnd + evenval;
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                            attnd = "LA-";
//                                        else
//                                            attnd = mornval;
//                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                        {

//                                        }
//                                        else
//                                            attnd = attnd + evenval;
//                                        latecnt = 1;
//                                    }
//                                }
//                                else if (InTime >= LateTime)
//                                {
//                                    if (mornval.Trim() == "" || mornval.Trim() == "-" || mornval.Trim() == "P-" || mornval.Trim() == "A-" || mornval.Trim() == "PER-" || mornval.Trim() == "LA-")
//                                        attnd = "A-";
//                                    else
//                                        attnd = mornval;
//                                    if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                    {

//                                    }
//                                    else
//                                        attnd = attnd + evenval;
//                                }
//                            }
//                        }
//                        if (attnd.Trim() != "")
//                        {
//                            string Query = " if exists(select * from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monandYear + "' ) update staff_attnd set [" + dtd + "]='" + attnd + "',latecount='" + latecnt + "',permissioncount='" + mypercnt + "' where staff_code='" + staffcode + "' and mon_year='" + monandYear + "' else insert into staff_attnd(staff_code,mon_year,[" + dtd + "]) values('" + staffcode + "','" + monandYear + "','" + attnd + "')";
//                            Query += " if exists(select * from Bio_Attendance where roll_no='" + staffcode + "' and is_staff='1' and access_date='" + logdate + "') update  Bio_Attendance set time_in='" + InTime + "',time_out='" + OutTime + "',att='" + attnd + "' where roll_no='" + staffcode + "' and is_staff='1' and access_date='" + logdate + "'  else insert into Bio_Attendance(roll_no,time_in,time_out,is_staff,access_date,latestrec,att,mark_time) values('" + staffcode + "','" + InTime + "','" + OutTime + "','1','" + logdate + "','1','" + attnd + "','" + DateTime.Now.ToString("hh:mm tt") + "')";

//                            int upd = d2.update_method_wo_parameter(Query, "Text");
//                            if (upd > 0)
//                            {
//                                scount++;
//                            }
//                        }
//                    }
//                }
//                if (outtime != "")
//                {
//                    latecnt = 0;
//                    mypercnt = 0;
//                    attnd = "";
//                    mornval = "";
//                    evenval = "";

//                    if (category.Trim() != "" || stftype.Trim() != "")
//                    {
//                        selq = "";
//                        //Bell_Date check
//                        if (grptype.Trim() == "2")
//                            selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and Bell_Date='" + logdate + "'";
//                        else
//                            selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and Bell_Date='" + logdate + "'";
//                        dsinout.Clear();
//                        dsinout = d2.select_method_wo_parameter(selq, "Text");
//                        if (dsinout.Tables[0].Rows.Count == 0)
//                        {
//                            //Bell_Day check
//                            if (grptype.Trim() == "2")
//                                selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and upper(Bell_Day) ='" + Convert.ToDateTime(logdate).ToString("dddd") + "'";
//                            else
//                                selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and upper(Bell_Day) ='" + Convert.ToDateTime(logdate).ToString("dddd") + "'";
//                            dsinout.Clear();
//                            dsinout = d2.select_method_wo_parameter(selq, "Text");
//                            if (dsinout.Tables[0].Rows.Count == 0)
//                            {
//                                if (grptype.Trim() == "2")
//                                    selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND DayType = 0 AND College_Code =" + Convert.ToString(collcode) + "";
//                                else
//                                    selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND DayType = 0 AND College_Code =" + Convert.ToString(collcode) + "";

//                                dsinout.Clear();
//                                dsinout = d2.select_method_wo_parameter(selq, "Text");
//                            }
//                        }
//                        if (dsinout.Tables.Count > 0 && dsinout.Tables[0].Rows.Count > 0)
//                        {
//                            mornouttime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["MorningOutTime"])).TimeOfDay;
//                            GraceTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"])).TimeOfDay;
//                            extendgracetime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"])).TimeOfDay;
//                            LateTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"])).TimeOfDay;
//                            LunchEndTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["Lunch_End_Time"])).TimeOfDay;
//                            dsOutTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["outtime"])).TimeOfDay;
//                            perTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["permission_time"])).TimeOfDay;

//                            InTime = Convert.ToDateTime(intime).TimeOfDay;
//                            OutTime = Convert.ToDateTime(outtime).TimeOfDay;
//                            Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["noofper"]), out noofper);
//                            Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["nooflate"]), out nooflate);
//                            apyleve = Convert.ToString(dsinout.Tables[0].Rows[0]["ApplyLeave"]);
//                            Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["IsCalPayDate"]), out paytype);

//                            string[] mval = Convert.ToString(d2.GetFunction("select [" + dtd + "] from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monandYear + "'")).Split('-');
//                            if (mval.Length == 2)
//                            {
//                                mornval = mval[0] + "-";
//                                evenval = mval[1];
//                            }
//                            else
//                            {
//                                if (mornval == "")
//                                    mornval = "-";
//                                if (evenval == "")
//                                    evenval = "-";
//                            }
//                            if (InTime > LunchEndTime)
//                            {
//                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                    attnd = mornval + "A";
//                                else
//                                    attnd = mornval + evenval;
//                            }
//                            else if (OutTime >= dsOutTime)
//                            {
//                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                    attnd = mornval + "P";
//                                else
//                                    attnd = mornval + evenval;
//                            }
//                            else if (OutTime >= perTime && OutTime <= dsOutTime)
//                            {
//                                if (noofper == 0)
//                                {
//                                    if (apyleve == "A")
//                                    {
//                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            attnd = mornval + "A";
//                                        else
//                                            attnd = mornval + evenval;
//                                    }
//                                    else
//                                    {
//                                        if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                        {
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                attnd = mornval + "A";
//                                            else
//                                                attnd = mornval + evenval;
//                                        }
//                                        else
//                                        {
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                attnd = mornval + apyleve;
//                                            else
//                                                attnd = mornval + evenval;
//                                        }
//                                    }
//                                    if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                        attnd = mornval + "A";
//                                    else
//                                        attnd = mornval + evenval;
//                                }
//                                else if (GetPermissionCount(staffcode, logdate, paytype, out intPERTak, collcode) >= noofper)
//                                {
//                                    if (apyleve == "A")
//                                    {
//                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            attnd = mornval + "A";
//                                        else
//                                            attnd = mornval + evenval;
//                                    }
//                                    else
//                                    {
//                                        if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                        {
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                                attnd = mornval + "A";
//                                            else
//                                                attnd = mornval + evenval;
//                                        }
//                                        else
//                                        {
//                                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            {
//                                                if (apyleve == "LA")
//                                                    attnd = mornval + "PER";
//                                                else
//                                                    attnd = mornval + apyleve;
//                                            }
//                                            else
//                                                attnd = mornval + evenval;
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                        attnd = mornval + "PER";
//                                    else
//                                        attnd = mornval + evenval;
//                                    mypercnt = 1;
//                                }
//                            }
//                            else if (OutTime < dsOutTime)
//                                attnd = mornval + "A";
//                            else
//                            {
//                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                {
//                                    if (OutTime < mornouttime)
//                                    {
//                                        if (eveabs.Trim().ToUpper() == "TRUE")
//                                            attnd = mornval + "PER";
//                                    }
//                                }
//                                else
//                                    attnd = mornval + evenval;
//                            }
//                            if (attnd.Trim() != "")
//                            {
//                                string Query = " if exists(select * from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monandYear + "' ) update staff_attnd set [" + dtd + "]='" + attnd + "',latecount='" + latecnt + "',permissioncount='" + mypercnt + "' where staff_code='" + staffcode + "' and mon_year='" + monandYear + "' else insert into staff_attnd(staff_code,mon_year,[" + dtd + "]) values ('" + staffcode + "','" + monandYear + "','" + attnd + "')";
//                                Query += " if exists(select * from Bio_Attendance where roll_no='" + staffcode + "' and is_staff='1' and access_date='" + logdate + "') update  Bio_Attendance set time_in='" + InTime + "',time_out='" + OutTime + "',att='" + attnd + "' where roll_no='" + staffcode + "' and is_staff='1' and access_date='" + logdate + "'  else insert into Bio_Attendance(roll_no,time_in,time_out,is_staff,access_date,latestrec,att,mark_time) values('" + staffcode + "','" + InTime + "','" + OutTime + "','1','" + logdate + "','1','" + attnd + "','" + DateTime.Now.ToString("hh:mm tt") + "')";

//                                int upd = d2.update_method_wo_parameter(Query, "Text");
//                                if (upd > 0)
//                                {
//                                    scount++;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }
//        else
//        {
//            if (outtime != "")
//            {
//                latecnt = 0;
//                mypercnt = 0;
//                attnd = "";
//                mornval = "";
//                evenval = "";

//                if (category.Trim() != "" || stftype.Trim() != "")
//                {
//                    selq = "";
//                    //Bell_Date check
//                    if (grptype.Trim() == "2")
//                        selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and Bell_Date='" + logdate + "'";
//                    else
//                        selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and Bell_Date='" + logdate + "'";
//                    dsinout.Clear();
//                    dsinout = d2.select_method_wo_parameter(selq, "Text");
//                    if (dsinout.Tables[0].Rows.Count == 0)
//                    {
//                        //Bell_Day check
//                        if (grptype.Trim() == "2")
//                            selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and upper(Bell_Day) ='" + Convert.ToDateTime(logdate).ToString("dddd") + "'";
//                        else
//                            selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND College_Code =" + Convert.ToString(collcode) + " and upper(Bell_Day) ='" + Convert.ToDateTime(logdate).ToString("dddd") + "'";
//                        dsinout.Clear();
//                        dsinout = d2.select_method_wo_parameter(selq, "Text");
//                        if (dsinout.Tables[0].Rows.Count == 0)
//                        {
//                            if (grptype.Trim() == "2")
//                                selq = "select * from in_out_time where category_code='" + category + "' and shift ='" + shift + "' AND DayType = 0 AND College_Code =" + Convert.ToString(collcode) + "";
//                            else
//                                selq = "select * from in_out_time where StfType='" + stftype + "' and shift ='" + shift + "' AND DayType = 0 AND College_Code =" + Convert.ToString(collcode) + "";

//                            dsinout.Clear();
//                            dsinout = d2.select_method_wo_parameter(selq, "Text");
//                        }
//                    }
//                    if (dsinout.Tables.Count > 0 && dsinout.Tables[0].Rows.Count > 0)
//                    {
//                        mornouttime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["MorningOutTime"])).TimeOfDay;
//                        GraceTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["gracetime"])).TimeOfDay;
//                        extendgracetime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["extend_gracetime"])).TimeOfDay;
//                        LateTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["latetime"])).TimeOfDay;
//                        LunchEndTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["Lunch_End_Time"])).TimeOfDay;
//                        dsOutTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["outtime"])).TimeOfDay;
//                        perTime = Convert.ToDateTime(Convert.ToString(dsinout.Tables[0].Rows[0]["permission_time"])).TimeOfDay;

//                        InTime = Convert.ToDateTime(intime).TimeOfDay;
//                        OutTime = Convert.ToDateTime(outtime).TimeOfDay;
//                        Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["noofper"]), out noofper);
//                        Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["nooflate"]), out nooflate);
//                        apyleve = Convert.ToString(dsinout.Tables[0].Rows[0]["ApplyLeave"]);
//                        Int32.TryParse(Convert.ToString(dsinout.Tables[0].Rows[0]["IsCalPayDate"]), out paytype);

//                        string[] mval = Convert.ToString(d2.GetFunction("select [" + dtd + "] from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monandYear + "'")).Split('-');
//                        if (mval.Length == 2)
//                        {
//                            mornval = mval[0] + "-";
//                            evenval = mval[1];
//                        }
//                        else
//                        {
//                            if (mornval == "")
//                                mornval = "-";
//                            if (evenval == "")
//                                evenval = "-";
//                        }
//                        if (OutTime >= dsOutTime)
//                        {
//                            if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                attnd = mornval + "P";
//                        }
//                        else if (OutTime >= perTime && OutTime < dsOutTime)
//                        {
//                            if (noofper == 0)
//                            {
//                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                    attnd = mornval + "A";
//                            }
//                            else if (GetPermissionCount(staffcode, logdate, paytype, out intPERTak, collcode) >= noofper)
//                            {
//                                if (apyleve == "A")
//                                {
//                                    if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                        attnd = mornval + "A";
//                                }
//                                else
//                                {
//                                    if (LeaveChk(staffcode, apyleve, logdate, paytype, collcode) == 0)
//                                    {
//                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            attnd = mornval + "A";
//                                    }
//                                    else
//                                    {
//                                        if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                            attnd = mornval + apyleve;
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                if (evenval.Trim() == "" || evenval.Trim() == "-" || evenval.Trim() == "P" || evenval.Trim() == "A" || evenval.Trim() == "PER" || evenval.Trim() == "LA")
//                                    attnd = mornval + "PER";
//                                mypercnt = 1;
//                            }
//                        }
//                        if (attnd.Trim() != "")
//                        {
//                            string Query = " if exists(select * from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + monandYear + "' ) update staff_attnd set [" + dtd + "]='" + attnd + "',latecount='" + latecnt + "',permissioncount='" + mypercnt + "' where staff_code='" + staffcode + "' and mon_year='" + monandYear + "' else insert into staff_attnd(staff_code,mon_year,[" + dtd + "]) values ('" + staffcode + "','" + monandYear + "','" + attnd + "')";
//                            Query += " if exists(select * from Bio_Attendance where roll_no='" + staffcode + "' and is_staff='1' and access_date='" + logdate + "') update  Bio_Attendance set time_in='" + InTime + "',time_out='" + OutTime + "',att='" + attnd + "' where roll_no='" + staffcode + "' and is_staff='1' and access_date='" + logdate + "'  else insert into Bio_Attendance(roll_no,time_in,time_out,is_staff,access_date,latestrec,att,mark_time) values('" + staffcode + "','" + InTime + "','" + OutTime + "','1','" + logdate + "','1','" + attnd + "','" + DateTime.Now.ToString("hh:mm tt") + "')";

//                            int upd = d2.update_method_wo_parameter(Query, "Text");
//                            if (upd > 0)
//                            {
//                                scount++;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//}

//public int getHour(string date)
//{
//    int hour = 0;
//    DateTime dt = Convert.ToDateTime(date);
//    hour = dt.Hour;
//    return hour;
//}

//public int getMinute(string date)
//{
//    int minute = 0;
//    DateTime dt = Convert.ToDateTime(date);
//    minute = dt.Minute;
//    return minute;
//}

//public void LateSettings(string rollorstafcode, string indate, string intime, string latefrom, string lateto, string catcode, string collcode)
//{
//    string selq = "";
//    DataSet dsgethryear = new DataSet();
//    DataSet dsgetlate = new DataSet();
//    string[] spllev = new string[10];
//    string[] spllev1 = new string[10];
//    string[] splattn = new string[2];
//    string MonAtt = "";
//    string EveAtt = "";
//    string StrStaffLeave = "";
//    string StrYearLeave = "";
//    string StrMonthLeave = "";
//    string StrCarry = "";
//    string StrLeaveName = "";
//    int strmaxday = 0;
//    string StrSaveAttn = "";
//    double strLeaveTak = 0;
//    int strLATak = 0;
//    DateTime StrFromDate;
//    DateTime StrToDate;
//    string strmonyear = "";
//    string StrPrevAtt = "";
//    string StrLAAbsCount = "";
//    string strpaytype = "";
//    int inpaytype = 0;
//    double leavedays = 0;
//    double yearlev = 0;
//    double monlev = 0;
//    int upcount = 0;
//    int intLATak;

//    strpaytype = d2.GetFunction("select isnull(IsCalPayDate,'0') from in_out_time where category_code = '" + catcode + "'");
//    Int32.TryParse(strpaytype, out inpaytype);
//    selq = "SELECT * FROM HrPayMonths WHERE '" + indate + "' between from_date and to_date ";
//    dsgethryear.Clear();
//    dsgethryear = d2.select_method_wo_parameter(selq, "Text");
//    if (dsgethryear.Tables.Count > 0 && dsgethryear.Tables[0].Rows.Count > 0)
//    {
//        StrFromDate = Convert.ToDateTime(dsgethryear.Tables[0].Rows[0]["from_date"]);
//        StrToDate = Convert.ToDateTime(dsgethryear.Tables[0].Rows[0]["to_date"]);
//    }
//    else
//    {
//        StrFromDate = Convert.ToDateTime(Convert.ToDateTime(indate).Month + "/" + "1" + "/" + Convert.ToDateTime(indate).Year);
//        StrToDate = Convert.ToDateTime(Convert.ToDateTime(indate).Month + "/" + DateTime.DaysInMonth(Convert.ToDateTime(indate).Year, Convert.ToDateTime(indate).Month) + "/" + Convert.ToDateTime(indate).Year);
//    }

//    if (StrFromDate.Month == StrToDate.Month)
//    {
//        strmonyear = Convert.ToString(Convert.ToDateTime(indate).Month + "/" + Convert.ToDateTime(indate).Year).Trim();
//        strLATak = GetLateCount(rollorstafcode, indate, inpaytype, out intLATak, collcode);
//        StrLAAbsCount = d2.GetFunction("SELECT SUM(LateCount) FROM Staff_Attnd WHERE Staff_Code ='" + rollorstafcode + "' AND Mon_Year ='" + strmonyear + "'");
//        intLATak = strLATak + Convert.ToInt32(StrLAAbsCount) + 1;

//        selq = "SELECT * FROM StaffLateSettings WHERE " + intLATak + " BETWEEN FromLA AND ToLA AND CollegeCode =" + collcode + " AND Category_Code ='" + catcode + "'";
//        dsgetlate.Clear();
//        dsgetlate = d2.select_method_wo_parameter(selq, "Text");
//        if (dsgetlate.Tables.Count > 0 && dsgetlate.Tables[0].Rows.Count > 0)
//        {
//            StrLeaveName = d2.GetFunction("select category from leave_category where shortname ='" + Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]) + "'");
//            if (StrLeaveName.Trim() != "" && StrLeaveName.Trim() != "0")
//            {
//                StrStaffLeave = d2.GetFunction("SELECT LeaveType FROM Individual_Leave_Type where Staff_Code ='" + rollorstafcode + "'");
//                if (StrStaffLeave.Trim() != "" && StrStaffLeave.Trim() != "0")
//                {
//                    spllev = StrStaffLeave.Split('/');
//                    if (spllev.Length > 0)
//                    {
//                        for (int s = 0; s < spllev.Length; s++)
//                        {
//                            spllev1 = spllev[s].Split(';');
//                            if (spllev1.Length > 0)
//                            {
//                                if (StrLeaveName.Trim().ToUpper() == spllev1[0].Trim().ToUpper())
//                                {
//                                    MonAtt = "";
//                                    EveAtt = "";
//                                    if (spllev1.Length >= 2)
//                                    {
//                                        StrYearLeave = spllev1[1];
//                                    }
//                                    if (spllev1.Length >= 3)
//                                    {
//                                        StrMonthLeave = spllev1[2];
//                                    }
//                                    if (spllev1.Length >= 4)
//                                    {
//                                        StrCarry = spllev1[3];
//                                    }

//                                    strLeaveTak = GetLeaveCount(rollorstafcode, indate, Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]), 0, collcode);
//                                    StrPrevAtt = d2.GetFunction("select [" + Convert.ToDateTime(indate).Day + "] from staff_attnd where staff_code ='" + rollorstafcode + "' and mon_year ='" + strmonyear + "'");
//                                    if (StrPrevAtt.Trim() != "" && StrPrevAtt.Trim() != "0")
//                                    {
//                                        splattn = StrPrevAtt.Split('-');
//                                        if (splattn.Length > 0)
//                                        {
//                                            MonAtt = splattn[0];
//                                            EveAtt = splattn[1];
//                                        }
//                                    }
//                                    Double.TryParse(Convert.ToString(dsgetlate.Tables[0].Rows[0]["LeaveDays"]), out leavedays);
//                                    Double.TryParse(StrYearLeave, out yearlev);
//                                    Double.TryParse(StrMonthLeave, out monlev);
//                                    if ((strLeaveTak + leavedays) <= yearlev && (strLeaveTak + leavedays) <= monlev)
//                                    {
//                                        if (leavedays == 0.5)
//                                        {
//                                            StrSaveAttn = Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]) + "-" + EveAtt;
//                                        }
//                                        else
//                                        {
//                                            StrSaveAttn = Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]) + "-" + Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]);
//                                        }
//                                        upcount = d2.update_method_wo_parameter("update staff_attnd set [" + Convert.ToDateTime(indate).Day + "] ='" + StrSaveAttn + "',latecount = latecount+1  where staff_code ='" + rollorstafcode + "' and mon_year ='" + strmonyear + "'", "Text");
//                                    }
//                                    else
//                                    {
//                                        if (leavedays == 0.5)
//                                        {
//                                            StrSaveAttn = "LOP" + "-" + EveAtt;
//                                        }
//                                        else
//                                        {
//                                            StrSaveAttn = "LOP" + "-" + "LOP";
//                                        }
//                                        upcount = d2.update_method_wo_parameter("update staff_attnd set [" + Convert.ToDateTime(indate).Day + "] ='" + StrSaveAttn + "',latecount = latecount+1  where staff_code ='" + rollorstafcode + "' and mon_year ='" + strmonyear + "'", "Text");
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//    else
//    {
//        strmaxday = DateTime.DaysInMonth(Convert.ToDateTime(indate).Year, Convert.ToDateTime(indate).Month);
//        for (int k = StrFromDate.Day; k < strmaxday; k++)
//        {
//            strmonyear = Convert.ToString(Convert.ToDateTime(StrFromDate).Month + "/" + Convert.ToDateTime(StrFromDate).Year).Trim();
//            intLATak = 0;
//            strLATak = GetLateCount(rollorstafcode, indate, inpaytype, out intLATak, collcode);
//            StrLAAbsCount = d2.GetFunction("SELECT SUM(LateCount) FROM Staff_Attnd WHERE Staff_Code ='" + rollorstafcode + "' AND Mon_Year ='" + strmonyear + "'");
//            intLATak = strLATak + Convert.ToInt32(StrLAAbsCount) + 1;

//            selq = "SELECT * FROM StaffLateSettings WHERE " + intLATak + " BETWEEN FromLA AND ToLA AND CollegeCode =" + collcode + " AND Category_Code ='" + catcode + "'";
//            dsgetlate.Clear();
//            dsgetlate = d2.select_method_wo_parameter(selq, "Text");
//            if (dsgetlate.Tables.Count > 0 && dsgetlate.Tables[0].Rows.Count > 0)
//            {
//                StrLeaveName = d2.GetFunction("select category from leave_category where shortname ='" + Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]) + "'");
//                if (StrLeaveName.Trim() != "" && StrLeaveName.Trim() != "0")
//                {
//                    StrStaffLeave = d2.GetFunction("SELECT LeaveType FROM Individual_Leave_Type where Staff_Code ='" + rollorstafcode + "'");
//                    if (StrStaffLeave.Trim() != "" && StrStaffLeave.Trim() != "0")
//                    {
//                        spllev = StrStaffLeave.Split('/');
//                        if (spllev.Length > 0)
//                        {
//                            for (int s = 0; s < spllev.Length; s++)
//                            {
//                                spllev1 = spllev[s].Split(';');
//                                if (spllev1.Length > 0)
//                                {
//                                    if (StrLeaveName.Trim().ToUpper() == spllev1[0].Trim().ToUpper())
//                                    {
//                                        MonAtt = "";
//                                        EveAtt = "";
//                                        if (spllev1.Length >= 2)
//                                        {
//                                            StrYearLeave = spllev1[1];
//                                        }
//                                        if (spllev1.Length >= 3)
//                                        {
//                                            StrMonthLeave = spllev1[2];
//                                        }
//                                        if (spllev1.Length >= 4)
//                                        {
//                                            StrCarry = spllev1[3];
//                                        }

//                                        strLeaveTak = GetLeaveCount(rollorstafcode, indate, Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]), 0, collcode);
//                                        StrPrevAtt = d2.GetFunction("select [" + Convert.ToDateTime(indate).Day + "] from staff_attnd where staff_code ='" + rollorstafcode + "' and mon_year ='" + strmonyear + "'");
//                                        if (StrPrevAtt.Trim() != "" && StrPrevAtt.Trim() != "0")
//                                        {
//                                            splattn = StrPrevAtt.Split('-');
//                                            if (splattn.Length > 0)
//                                            {
//                                                MonAtt = splattn[0];
//                                                EveAtt = splattn[1];
//                                            }
//                                        }
//                                        Double.TryParse(Convert.ToString(dsgetlate.Tables[0].Rows[0]["LeaveDays"]), out leavedays);
//                                        Double.TryParse(StrYearLeave, out yearlev);
//                                        Double.TryParse(StrMonthLeave, out monlev);
//                                        if ((strLeaveTak + leavedays) <= yearlev && (strLeaveTak + leavedays) <= monlev)
//                                        {
//                                            if (leavedays == 0.5)
//                                            {
//                                                StrSaveAttn = Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]) + "-" + EveAtt;
//                                            }
//                                            else
//                                            {
//                                                StrSaveAttn = Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]) + "-" + Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]);
//                                            }
//                                            upcount = d2.update_method_wo_parameter("update staff_attnd set [" + Convert.ToDateTime(indate).Day + "] ='" + StrSaveAttn + "',latecount = latecount+1  where staff_code ='" + rollorstafcode + "' and mon_year ='" + strmonyear + "'", "Text");
//                                        }
//                                        else
//                                        {
//                                            if (leavedays == 0.5)
//                                            {
//                                                StrSaveAttn = "LOP" + "-" + EveAtt;
//                                            }
//                                            else
//                                            {
//                                                StrSaveAttn = "LOP" + "-" + "LOP";
//                                            }
//                                            upcount = d2.update_method_wo_parameter("update staff_attnd set [" + Convert.ToDateTime(indate).Day + "] ='" + StrSaveAttn + "',latecount = latecount+1  where staff_code ='" + rollorstafcode + "' and mon_year ='" + strmonyear + "'", "Text");
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        for (int m = 1; m < StrToDate.Day; m++)
//        {
//            strmonyear = Convert.ToString(StrToDate.Month + "/" + StrToDate.Year).Trim();
//            intLATak = 0;
//            strLATak = GetLateCount(rollorstafcode, indate, inpaytype, out intLATak, collcode);
//            StrLAAbsCount = d2.GetFunction("SELECT SUM(LateCount) FROM Staff_Attnd WHERE Staff_Code ='" + rollorstafcode + "' AND Mon_Year ='" + strmonyear + "'");
//            intLATak = strLATak + Convert.ToInt32(StrLAAbsCount) + 1;

//            selq = "SELECT * FROM StaffLateSettings WHERE " + intLATak + " BETWEEN FromLA AND ToLA AND CollegeCode =" + collcode + " AND Category_Code ='" + catcode + "'";
//            dsgetlate.Clear();
//            dsgetlate = d2.select_method_wo_parameter(selq, "Text");
//            if (dsgetlate.Tables.Count > 0 && dsgetlate.Tables[0].Rows.Count > 0)
//            {
//                StrLeaveName = d2.GetFunction("select category from leave_category where shortname ='" + Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]) + "'");
//                if (StrLeaveName.Trim() != "" && StrLeaveName.Trim() != "0")
//                {
//                    StrStaffLeave = d2.GetFunction("SELECT LeaveType FROM Individual_Leave_Type where Staff_Code ='" + rollorstafcode + "'");
//                    if (StrStaffLeave.Trim() != "" && StrStaffLeave.Trim() != "0")
//                    {
//                        spllev = StrStaffLeave.Split('/');
//                        if (spllev.Length > 0)
//                        {
//                            for (int s = 0; s < spllev.Length; s++)
//                            {
//                                spllev1 = spllev[s].Split(';');
//                                if (spllev1.Length > 0)
//                                {
//                                    if (StrLeaveName.Trim().ToUpper() == spllev1[0].Trim().ToUpper())
//                                    {
//                                        MonAtt = "";
//                                        EveAtt = "";
//                                        if (spllev1.Length >= 2)
//                                        {
//                                            StrYearLeave = spllev1[1];
//                                        }
//                                        if (spllev1.Length >= 3)
//                                        {
//                                            StrMonthLeave = spllev1[2];
//                                        }
//                                        if (spllev1.Length >= 4)
//                                        {
//                                            StrCarry = spllev1[3];
//                                        }

//                                        strLeaveTak = GetLeaveCount(rollorstafcode, indate, Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]), 0, collcode);
//                                        StrPrevAtt = d2.GetFunction("select [" + Convert.ToDateTime(indate).Day + "] from staff_attnd where staff_code ='" + rollorstafcode + "' and mon_year ='" + strmonyear + "'");
//                                        if (StrPrevAtt.Trim() != "" && StrPrevAtt.Trim() != "0")
//                                        {
//                                            splattn = StrPrevAtt.Split('-');
//                                            if (splattn.Length > 0)
//                                            {
//                                                MonAtt = splattn[0];
//                                                EveAtt = splattn[1];
//                                            }
//                                        }
//                                        Double.TryParse(Convert.ToString(dsgetlate.Tables[0].Rows[0]["LeaveDays"]), out leavedays);
//                                        Double.TryParse(StrYearLeave, out yearlev);
//                                        Double.TryParse(StrMonthLeave, out monlev);
//                                        if ((strLeaveTak + leavedays) <= yearlev && (strLeaveTak + leavedays) <= monlev)
//                                        {
//                                            if (leavedays == 0.5)
//                                            {
//                                                StrSaveAttn = Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]) + "-" + EveAtt;
//                                            }
//                                            else
//                                            {
//                                                StrSaveAttn = Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]) + "-" + Convert.ToString(dsgetlate.Tables[0].Rows[0]["ShortName"]);
//                                            }
//                                            upcount = d2.update_method_wo_parameter("update staff_attnd set [" + Convert.ToDateTime(indate).Day + "] ='" + StrSaveAttn + "',latecount = latecount+1  where staff_code ='" + rollorstafcode + "' and mon_year ='" + strmonyear + "'", "Text");
//                                        }
//                                        else
//                                        {
//                                            if (leavedays == 0.5)
//                                            {
//                                                StrSaveAttn = "LOP" + "-" + EveAtt;
//                                            }
//                                            else
//                                            {
//                                                StrSaveAttn = "LOP" + "-" + "LOP";
//                                            }
//                                            upcount = d2.update_method_wo_parameter("update staff_attnd set [" + Convert.ToDateTime(indate).Day + "] ='" + StrSaveAttn + "',latecount = latecount+1  where staff_code ='" + rollorstafcode + "' and mon_year ='" + strmonyear + "'", "Text");
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//}

//public double GetLeaveCount(string StaffCode, string attdate, string leavetype, int pintPayDate, string collcode)
//{
//    string selq = "";
//    string selval = "";
//    string[] splval = new string[2];
//    DataSet dslev = new DataSet();
//    DataSet dslev1 = new DataSet();
//    Double LeaveCount = 0;
//    Double GetLeaveCount = 0;
//    int i = 0;

//    if (pintPayDate == 0)
//    {
//        i = Convert.ToDateTime(attdate).Day;
//        selq = "select * from staff_attnd where staff_code='" + StaffCode + "' and mon_year='" + Convert.ToDateTime(attdate).Month + "/" + Convert.ToDateTime(attdate).Year + "'";
//        dslev.Clear();
//        dslev = d2.select_method_wo_parameter(selq, "Text");
//        if (dslev.Tables.Count > 0 && dslev.Tables[0].Rows.Count > 0)
//        {
//            for (int k = 1; k < i; k++)
//            {
//                selval = d2.GetFunction("select [" + k + "] from staff_attnd where staff_code='" + StaffCode + "' and mon_year='" + Convert.ToDateTime(attdate).Month + "/" + Convert.ToDateTime(attdate).Year + "'");
//                if (selval.Trim() != "" && selval.Trim() != "0")
//                {
//                    splval = selval.Split('-');
//                    if (splval.Length > 0)
//                    {
//                        if (splval[0] == leavetype)
//                        {
//                            LeaveCount = LeaveCount + 1;
//                        }
//                        if (splval[1] == leavetype)
//                        {
//                            LeaveCount = LeaveCount + 1;
//                        }
//                    }
//                }
//            }
//        }
//        if (LeaveCount == 0.5)
//        {
//            GetLeaveCount = 0.5;
//        }
//        else
//        {
//            GetLeaveCount = LeaveCount / 2;
//        }
//    }
//    else
//    {
//        selq = "SELECT * FROM HrPayMonths WHERE '" + attdate + "' BETWEEN From_Date AND To_Date AND College_Code =" + collcode + " AND ISNULL(SelStatus,0) = 1 ORDER BY From_Date ";
//        dslev1.Clear();
//        dslev1 = d2.select_method_wo_parameter(selq, "Text");
//        if (dslev1.Tables.Count > 0 && dslev1.Tables[0].Rows.Count > 0)
//        {
//            i = DateTime.DaysInMonth(Convert.ToDateTime(Convert.ToString(dslev1.Tables[0].Rows[0]["From_Date"])).Year, Convert.ToDateTime(Convert.ToString(dslev1.Tables[0].Rows[0]["From_Date"])).Month);
//            if (Convert.ToDateTime(Convert.ToString(dslev1.Tables[0].Rows[0]["From_Date"])).Month != Convert.ToDateTime(Convert.ToString(dslev1.Tables[0].Rows[0]["To_Date"])).Month)
//            {
//                selq = "select * from staff_attnd where staff_code='" + StaffCode + "' and mon_year='" + Convert.ToDateTime(Convert.ToString(dslev1.Tables[0].Rows[0]["From_Date"])).Month + "/" + Convert.ToDateTime(Convert.ToString(dslev1.Tables[0].Rows[0]["From_Date"])).Year + "'";
//                dslev.Clear();
//                dslev = d2.select_method_wo_parameter(selq, "Text");
//                if (dslev.Tables.Count > 0 && dslev.Tables[0].Rows.Count > 0)
//                {
//                    for (int k = Convert.ToDateTime(Convert.ToString(dslev.Tables[0].Rows[0]["From_Date"])).Day; k < i; k++)
//                    {
//                        selval = d2.GetFunction("select [" + k + "] from staff_attnd where staff_code='" + StaffCode + "' and mon_year='" + Convert.ToDateTime(Convert.ToString(dslev.Tables[0].Rows[0]["From_Date"])).Month + "/" + Convert.ToDateTime(Convert.ToString(dslev.Tables[0].Rows[0]["From_Date"])).Year + "'");
//                        if (selval.Trim() != "" && selval.Trim() != "0")
//                        {
//                            splval = selval.Split('-');
//                            if (splval.Length > 0)
//                            {
//                                if (splval[0] == leavetype)
//                                {
//                                    LeaveCount = LeaveCount + 1;
//                                }
//                                if (splval[1] == leavetype)
//                                {
//                                    LeaveCount = LeaveCount + 1;
//                                }
//                            }
//                        }
//                    }
//                }
//                selq = "select * from staff_attnd where staff_code='" + StaffCode + "' and mon_year='" + Convert.ToDateTime(Convert.ToString(dslev1.Tables[0].Rows[0]["To_Date"])).Month + "/" + Convert.ToDateTime(Convert.ToString(dslev1.Tables[0].Rows[0]["To_Date"])).Year + "'";
//                dslev.Clear();
//                dslev = d2.select_method_wo_parameter(selq, "Text");
//                if (dslev.Tables.Count > 0 && dslev.Tables[0].Rows.Count > 0)
//                {
//                    for (int k = 1; k < Convert.ToDateTime(Convert.ToString(dslev1.Tables[0].Rows[0]["To_Date"])).Day; k++)
//                    {
//                        selval = d2.GetFunction("select [" + k + "] from staff_attnd where staff_code='" + StaffCode + "' and mon_year='" + Convert.ToDateTime(Convert.ToString(dslev.Tables[0].Rows[0]["To_Date"])).Month + "/" + Convert.ToDateTime(Convert.ToString(dslev.Tables[0].Rows[0]["To_Date"])).Year + "'");
//                        if (selval.Trim() != "" && selval.Trim() != "0")
//                        {
//                            splval = selval.Split('-');
//                            if (splval.Length > 0)
//                            {
//                                if (splval[0] == leavetype)
//                                {
//                                    LeaveCount = LeaveCount + 1;
//                                }
//                                if (splval[1] == leavetype)
//                                {
//                                    LeaveCount = LeaveCount + 1;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//    if (LeaveCount == 0.5)
//    {
//        GetLeaveCount = 0.5;
//    }
//    else
//    {
//        GetLeaveCount = LeaveCount / 2;
//    }
//    return GetLeaveCount;
//}

//public int GetLateCount(string staffcode, string indate, int pinpaytype, out int pintLATAK, string collcode)
//{
//    string selq = "";
//    DataSet dspay = new DataSet();
//    DataSet dzpay = new DataSet();
//    string selval = "";
//    string[] splval = new string[2];
//    int latecount = 0;
//    int j = 0;

//    if (pinpaytype == 0)
//    {
//        j = Convert.ToDateTime(indate).Day;
//        selq = "select * from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + Convert.ToDateTime(indate).Month + "/" + Convert.ToDateTime(indate).Year + "'";
//        dspay.Clear();
//        dspay = d2.select_method_wo_parameter(selq, "Text");
//        if (dspay.Tables.Count > 0 && dspay.Tables[0].Rows.Count > 0)
//        {
//            for (int i = 0; i < dspay.Tables[0].Rows.Count; i++)
//            {
//                for (int k = 1; k < j; k++)
//                {
//                    selval = d2.GetFunction("select [" + k + "] from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + Convert.ToDateTime(indate).Month + "/" + Convert.ToDateTime(indate).Year + "'");
//                    if (selval.Trim() != "" && selval.Trim() != "0")
//                    {
//                        splval = selval.Split('-');
//                        if (splval.Length > 0)
//                        {
//                            if (splval[0] == "LA")
//                            {
//                                latecount = latecount + 1;
//                            }
//                            if (splval[1] == "LA")
//                            {
//                                latecount = latecount + 1;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//    else
//    {
//        selq = "SELECT * FROM HrPayMonths WHERE '" + indate + "' BETWEEN From_Date AND To_Date AND College_Code =" + collcode + " AND ISNULL(SelStatus,0) = 1 ORDER BY From_Date ";
//        dspay.Clear();
//        dspay = d2.select_method_wo_parameter(selq, "Text");
//        if (dspay.Tables.Count > 0 && dspay.Tables[0].Rows.Count > 0)
//        {
//            j = DateTime.DaysInMonth(Convert.ToDateTime(Convert.ToString(dspay.Tables[0].Rows[0]["From_Date"])).Year, Convert.ToDateTime(Convert.ToString(dspay.Tables[0].Rows[0]["From_Date"])).Month);
//            if (Convert.ToDateTime(Convert.ToString(dspay.Tables[0].Rows[0]["From_Date"])).Month != Convert.ToDateTime(Convert.ToString(dspay.Tables[0].Rows[0]["To_Date"])).Month)
//            {
//                selq = "select * from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + Convert.ToDateTime(Convert.ToString(dspay.Tables[0].Rows[0]["From_Date"])).Month + "/" + Convert.ToDateTime(Convert.ToString(dspay.Tables[0].Rows[0]["From_Date"])).Year + "'";
//                dzpay.Clear();
//                dzpay = d2.select_method_wo_parameter(selq, "Text");
//                if (dzpay.Tables.Count > 0 && dzpay.Tables[0].Rows.Count > 0)
//                {
//                    for (int k = Convert.ToDateTime(Convert.ToString(dzpay.Tables[0].Rows[0]["From_Date"])).Day; k < j; k++)
//                    {
//                        selval = d2.GetFunction("select [" + k + "] from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + Convert.ToDateTime(Convert.ToString(dzpay.Tables[0].Rows[0]["From_Date"])).Month + "/" + Convert.ToDateTime(Convert.ToString(dzpay.Tables[0].Rows[0]["From_Date"])).Year + "'");
//                        if (selval.Trim() != "" && selval.Trim() != "0")
//                        {
//                            splval = selval.Split('-');
//                            if (splval.Length > 0)
//                            {
//                                if (splval[0] == "LA")
//                                {
//                                    latecount = latecount + 1;
//                                }
//                                if (splval[1] == "LA")
//                                {
//                                    latecount = latecount + 1;
//                                }
//                            }
//                        }
//                    }
//                }
//                selq = "select * from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + Convert.ToDateTime(Convert.ToString(dspay.Tables[0].Rows[0]["To_Date"])).Month + "/" + Convert.ToDateTime(Convert.ToString(dspay.Tables[0].Rows[0]["To_Date"])).Year + "'";
//                dzpay.Clear();
//                dzpay = d2.select_method_wo_parameter(selq, "Text");
//                if (dzpay.Tables.Count > 0 && dzpay.Tables[0].Rows.Count > 0)
//                {
//                    for (int k = 1; k < Convert.ToDateTime(Convert.ToString(dspay.Tables[0].Rows[0]["To_Date"])).Day; k++)
//                    {
//                        selval = "";
//                        selval = d2.GetFunction("select [" + k + "] from staff_attnd where staff_code='" + staffcode + "' and mon_year='" + Convert.ToDateTime(Convert.ToString(dzpay.Tables[0].Rows[0]["To_Date"])).Month + "/" + Convert.ToDateTime(Convert.ToString(dzpay.Tables[0].Rows[0]["To_Date"])).Year + "'");
//                        if (selval.Trim() != "" && selval.Trim() != "0")
//                        {
//                            splval = selval.Split('-');
//                            if (splval.Length > 0)
//                            {
//                                if (splval[0] == "LA")
//                                {
//                                    latecount = latecount + 1;
//                                }
//                                if (splval[1] == "LA")
//                                {
//                                    latecount = latecount + 1;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//    pintLATAK = latecount;
//    return latecount;
//}

//public Double LeaveChk(string strScode, string StrSLeave, string indate, int pinpaytype, string collcode)
//{
//    Double GetLeavechk = 0;
//    string StrLeaveDesc = "";
//    string getleavedesc = "";
//    string leavetype = "";
//    Double IntAvl = 0;
//    Double IntLTak = 0;
//    Double StfLeave = 0;

//    getleavedesc = d2.GetFunction("SELECT Category FROM Leave_Category WHERE ShortName ='" + StrSLeave + "' AND College_Code ='" + Convert.ToString(collcode) + "'");
//    if (!String.IsNullOrEmpty(getleavedesc.Trim()) && getleavedesc.Trim() != "0")
//        StrLeaveDesc = getleavedesc;
//    else
//        StrLeaveDesc = "";
//    IntLTak = GetLeaveCount(strScode, indate, StrSLeave, pinpaytype, collcode);
//    leavetype = d2.GetFunction("SELECT ISNULL(LeaveType,'') LeaveType FROM individual_leave_type WHERE Staff_Code ='" + strScode + "' AND College_Code ='" + Convert.ToString(collcode) + "'");

//    if (!String.IsNullOrEmpty(leavetype.Trim()) && leavetype.Trim() != "0")
//    {
//        string[] strleavetype = leavetype.Trim().Split('\\');
//        if (strleavetype.Length > 0)
//        {
//            for (int s = 0; s < strleavetype.Length; s++)
//            {
//                string[] strleave = strleavetype[s].Split(';');
//                if (strleave.Length > 0)
//                {
//                    if (strleave[0] == StrLeaveDesc)
//                    {
//                        Double.TryParse(Convert.ToString(strleave[2]), out StfLeave);
//                        IntAvl = StfLeave - IntLTak;
//                        if (IntAvl <= 0)
//                            GetLeavechk = 0;
//                        else
//                            GetLeavechk = IntAvl;
//                    }
//                }
//            }
//        }
//    }
//    return GetLeavechk;
//}

//public int GetPermissionCount(string StaffCode, string attdate, int pintPayDate, out int intPERTak, string collcode)
//{
//    intPERTak = 0;
//    int Permissioncount = 0;
//    string strmonyear = "";
//    int day = 0;
//    DataSet dsPay = new DataSet();
//    DataSet dzPay = new DataSet();
//    Permissioncount = 0;
//    string selq = "";

//    if (pintPayDate == 0)
//    {
//        day = Convert.ToDateTime(attdate).Day;
//        strmonyear = Convert.ToString(Convert.ToDateTime(attdate).Month + "/" + Convert.ToDateTime(attdate).Year);
//        selq = "select * from staff_attnd where staff_code='" + StaffCode + "' and mon_year='" + strmonyear + "'";
//        dzPay.Clear();
//        dzPay = d2.select_method_wo_parameter(selq, "Text");
//        if (dzPay.Tables.Count > 0 && dzPay.Tables[0].Rows.Count > 0)
//        {
//            for (int att = 1; att <= day; att++)
//            {
//                if (!String.IsNullOrEmpty(Convert.ToString(dzPay.Tables[0].Rows[0]["" + att + ""])))
//                {
//                    string[] mval = Convert.ToString(dzPay.Tables[0].Rows[0]["" + att + ""]).Split('-');
//                    if (mval.Length > 0)
//                    {
//                        if (String.Compare(mval[0], "PER") == 0)
//                            Permissioncount = Permissioncount + 1;
//                        if (String.Compare(mval[1], "PER") == 0)
//                            Permissioncount = Permissioncount + 1;
//                    }
//                }
//            }
//        }
//    }
//    else
//    {
//        selq = "SELECT Convert(varchar(10),From_Date,101) as From_Date,Convert(varchar(10),To_Date,101) as To_Date FROM HrPayMonths WHERE '" + Convert.ToDateTime(attdate).ToString("MM/dd/yyyy") + "' BETWEEN From_Date AND To_Date AND College_Code ='" + Convert.ToString(collcode) + "' AND ISNULL(SelStatus,0) = 1 ORDER BY From_Date ";
//        dsPay.Clear();
//        dsPay = d2.select_method_wo_parameter(selq, "Text");
//        if (dsPay.Tables.Count > 0 && dsPay.Tables[0].Rows.Count > 0)
//        {
//            day = DateTime.DaysInMonth(Convert.ToDateTime(Convert.ToString(dsPay.Tables[0].Rows[0]["From_Date"])).Year, Convert.ToDateTime(Convert.ToString(dsPay.Tables[0].Rows[0]["From_Date"])).Month);
//            if (Convert.ToDateTime(Convert.ToString(dsPay.Tables[0].Rows[0]["From_Date"])).Month != Convert.ToDateTime(Convert.ToString(dsPay.Tables[0].Rows[0]["To_Date"])).Month)
//            {
//                strmonyear = Convert.ToString(Convert.ToDateTime(dsPay.Tables[0].Rows[0]["From_Date"]).Month + "/" + Convert.ToString(Convert.ToDateTime(dsPay.Tables[0].Rows[0]["From_Date"]).Year));
//                selq = "select * from staff_attnd where staff_code='" + StaffCode + "' and mon_year='" + strmonyear + "'";
//                dzPay.Clear();
//                dzPay = d2.select_method_wo_parameter(selq, "Text");
//                if (dzPay.Tables.Count > 0 && dzPay.Tables[0].Rows.Count > 0)
//                {
//                    for (int att = Convert.ToDateTime(dsPay.Tables[0].Rows[0]["From_Date"]).Day; att <= day; att++)
//                    {
//                        if (!String.IsNullOrEmpty(Convert.ToString(dzPay.Tables[0].Rows[0]["" + att + ""])))
//                        {
//                            string[] mval = Convert.ToString(dzPay.Tables[0].Rows[0]["" + att + ""]).Split('-');
//                            if (mval.Length > 0)
//                            {
//                                if (String.Compare(mval[0], "PER") == 0)
//                                    Permissioncount = Permissioncount + 1;
//                                if (String.Compare(mval[1], "PER") == 0)
//                                    Permissioncount = Permissioncount + 1;
//                            }
//                        }
//                    }
//                }
//                strmonyear = Convert.ToString(Convert.ToDateTime(dsPay.Tables[0].Rows[0]["To_Date"]).Month + "/" + Convert.ToString(Convert.ToDateTime(dsPay.Tables[0].Rows[0]["To_Date"]).Year));
//                selq = "select * from staff_attnd where staff_code='" + StaffCode + "' and mon_year='" + strmonyear + "'";
//                dzPay.Clear();
//                dzPay = d2.select_method_wo_parameter(selq, "Text");
//                if (dzPay.Tables.Count > 0 && dzPay.Tables[0].Rows.Count > 0)
//                {
//                    for (int att = Convert.ToDateTime(dsPay.Tables[0].Rows[0]["To_Date"]).Day; att <= day; att++)
//                    {
//                        if (!String.IsNullOrEmpty(Convert.ToString(dzPay.Tables[0].Rows[0]["" + att + ""])))
//                        {
//                            string[] mval = Convert.ToString(dzPay.Tables[0].Rows[0]["" + att + ""]).Split('-');
//                            if (mval.Length > 0)
//                            {
//                                if (String.Compare(mval[0], "PER") == 0)
//                                    Permissioncount = Permissioncount + 1;
//                                if (String.Compare(mval[1], "PER") == 0)
//                                    Permissioncount = Permissioncount + 1;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//    intPERTak = Permissioncount;
//    return Permissioncount;
//}

//public void markattdstudent(string morabs, string eveabs, string hosname, DateTime frmdt, DateTime todt, string collcode)
//{
//    int savcount = 0;
//    int mystudcount = 0;
//    string selq = "";
//    DataSet ds1 = new DataSet();
//    DataView dv = new DataView();
//    string currtime = DateTime.Now.ToString("MM/dd/yyyy");
//    DateTime currdttime = Convert.ToDateTime(currtime);

//    if (frmdt > todt)
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = "From Date Should be less than To Date!";
//        return;
//    }
//    if (frmdt >= currdttime || todt >= currdttime)
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = "From and To Date Should be less than Today!";
//        return;
//    }

//    string cblhtlvalue = "";
//    cblhtlvalue = hosname.Replace(",", "','");

//    if (cblhtlvalue.Trim() == "")
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = "Please Select the Hostel Name!";
//        return;
//    }
//    selq = " select Roll_No,finger_id,h.APP_No,HostelMasterFK from HT_HostelRegistration h,Registration r where h.APP_No = r.App_No and isnull(IsVacated,0) = 0 and isnull(IsDiscontinued,0) = 0  and isnull(issuspend,0)=0 and hostelmasterfk in(" + cblhtlvalue + ") and h.collegecode='" + Convert.ToString(collcode) + "'";

//    selq = selq + " select Convert(varchar(10),Log_Date,101) as Log_Date,InTime,outtime,FingerID from attn_logs where Log_date>='" + frmdt.ToString("MM/dd/yyyy") + "' and Log_date<='" + todt.ToString("MM/dd/yyyy") + "'  order by Log_date,FingerID,intime";
//    ds1.Clear();
//    ds1 = d2.select_method_wo_parameter(selq, "Text");
//    if (ds1.Tables.Count > 0 && ds1.Tables[1].Rows.Count > 0)
//    {
//        for (int ik = 0; ik < ds1.Tables[1].Rows.Count; ik++)
//        {
//            ds1.Tables[0].DefaultView.RowFilter = " finger_id='" + Convert.ToString(ds1.Tables[1].Rows[ik]["FingerID"]) + "'";
//            dv = ds1.Tables[0].DefaultView;
//            if (dv.Count > 0)
//            {
//                SaveStudAttendance(Convert.ToString(dv[0]["app_no"]), Convert.ToString(dv[0]["Roll_no"]), Convert.ToDateTime(ds1.Tables[1].Rows[ik]["Log_date"]).ToString("MM/dd/yyyy"), Convert.ToString(ds1.Tables[1].Rows[ik]["InTime"]), Convert.ToString(ds1.Tables[1].Rows[ik]["outtime"]), Convert.ToString(ds1.Tables[1].Rows[ik]["FingerID"]), Convert.ToString(ds1.Tables[1].Rows[ik]["hostelmasterFK"]), collcode, morabs, eveabs, out savcount);
//                mystudcount = mystudcount + savcount;
//            }
//        }
//    }
//    if (mystudcount > 0)
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = "Download and Mark the Attendance for Student Successfully!";
//    }
//    else
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = "Download UnSuccessfully,Please Try Again!";
//    }
//}

//private void SaveStudAttendance(string app_no, string roll_no, string logdate, string intime, string outtime, string FingerID, string hostelcode, string collcode, string morabs, string eveabs, out int stucount)
//{
//    stucount = 0;
//    string selq = "";
//    string attnd = "";
//    string eveattnd = "";
//    string gractTme = "";
//    string extGracttme = "";
//    string inTime = "";
//    string outTime = "";
//    string[] splattnd = new string[2];
//    DataSet dsinout = new DataSet();
//    DateTime dtsample = new DateTime();
//    DateTime gractme = new DateTime();
//    DateTime extgractme = new DateTime();
//    TimeSpan InTime = new TimeSpan();
//    TimeSpan TempInTime = new TimeSpan();
//    TimeSpan GraceTime = new TimeSpan();
//    TimeSpan OutTime = new TimeSpan();
//    TimeSpan TempOutTime = new TimeSpan();
//    TimeSpan ExtGrace = new TimeSpan();
//    DataSet dscnt = new DataSet();
//    int hour = 0;
//    int minute = 0;

//    if (hostelcode.Trim() != "" && app_no.Trim() != "")
//    {
//        selq = "select * from Hostel_InOut_Time where hostel_code='" + hostelcode + "' AND College_Code =" + Convert.ToString(collcode) + "";

//        dsinout.Clear();
//        dsinout = d2.select_method_wo_parameter(selq, "Text");
//        if (dsinout.Tables.Count > 0 && dsinout.Tables[0].Rows.Count > 0)
//        {
//            #region

//            int dtd = Convert.ToDateTime(logdate).Day;
//            int dtm = Convert.ToDateTime(logdate).Month;
//            int dty = Convert.ToDateTime(logdate).Year;
//            string monandYear = dtm + "/" + dty;

//            #region Morning late values

//            //intime and out time- in_out_table
//            string mornintme = Convert.ToString(dsinout.Tables[0].Rows[0]["in_time"]);
//            if (!string.IsNullOrEmpty(mornintme))
//            {
//                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["in_time"]));
//                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["in_time"]));
//                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                inTime = dtsample.ToString("hh:mm tt");
//            }

//            //out time
//            string eventime = Convert.ToString(dsinout.Tables[0].Rows[0]["out_time"]);
//            if (!string.IsNullOrEmpty(eventime))
//            {
//                hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["out_time"]));
//                minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["out_time"]));
//                dtsample = Convert.ToDateTime(hour + ":" + minute);
//                outTime = dtsample.ToString("hh:mm tt");
//            }
//            hour = 0;
//            minute = 0;

//            hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["grace_time"]));
//            minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["grace_time"]));
//            gractme = Convert.ToDateTime(hour + ":" + minute);
//            gractTme = dtsample.ToString("hh:mm tt");

//            hour = getHour(Convert.ToString(dsinout.Tables[0].Rows[0]["extgrace_time"]));
//            minute = getMinute(Convert.ToString(dsinout.Tables[0].Rows[0]["extgrace_time"]));
//            extgractme = Convert.ToDateTime(hour + ":" + minute);
//            extGracttme = dtsample.ToString("hh:mm tt");
//            #endregion
//            //intime
//            InTime = Convert.ToDateTime(intime).TimeOfDay;
//            TempInTime = Convert.ToDateTime(inTime).TimeOfDay;
//            GraceTime = Convert.ToDateTime(gractTme).TimeOfDay;
//            ExtGrace = Convert.ToDateTime(extGracttme).TimeOfDay;
//            //out time
//            if (outtime != "")
//                OutTime = Convert.ToDateTime(outtime).TimeOfDay;

//            TempOutTime = Convert.ToDateTime(outTime).TimeOfDay;

//            #region without morning and evening
//            //payprocess date calculate     
//            if (InTime == OutTime)
//            {
//                if (TempOutTime <= OutTime)
//                {
//                    if (morabs.Trim().ToUpper() == "TRUE")
//                    {
//                        attnd = "2";
//                        eveattnd = "1";
//                    }
//                    else
//                    {
//                        attnd = "";
//                        eveattnd = "1";
//                    }
//                }
//                else
//                {
//                    //intime calculation
//                    if (TempInTime >= InTime || TempInTime <= InTime || InTime <= GraceTime || InTime <= ExtGrace)
//                        attnd = "1";
//                    else
//                        attnd = "2";
//                    //out time calculation
//                    if (TempOutTime <= OutTime)
//                        eveattnd = "1";
//                    else
//                    {
//                        if (eveabs.Trim().ToUpper() == "TRUE")
//                            eveattnd = "2";
//                        else
//                            eveattnd = "";
//                    }
//                }
//            }
//            else
//            {
//                //intime calculation
//                if (TempInTime >= InTime || TempInTime <= InTime || InTime <= GraceTime || InTime <= ExtGrace)
//                    attnd = "1";
//                else
//                    attnd = "2";

//                //out time calculation
//                if (TempOutTime <= OutTime)
//                    eveattnd = "1";
//                else
//                    eveattnd = "2";
//            }
//            #endregion

//            //attendance save
//            if (!string.IsNullOrEmpty(attnd) || !string.IsNullOrEmpty(eveattnd))
//            {
//                string FnlAttnd = attnd + "-" + eveattnd;
//                string Query = " if exists(select * from HT_Attendance where attnmonth='" + dtm + "' and attnyear='" + dty + "' and app_no='" + app_no + "')update HT_Attendance set [D" + dtd + "]='" + attnd + "',[D" + dtd + "E]='" + eveattnd + "' where attnmonth='" + dtm + "' and attnyear='" + dty + "' and app_no='" + app_no + "' else insert into (attnmonth,attnyear,app_no,[D" + dtd + "],[D" + dtd + "E])values('" + dtm + "','" + dty + "','" + app_no + "','" + attnd + "','" + eveattnd + "')";

//                Query += " if exists(select * from Bio_Attendance where roll_no='" + roll_no + "' and is_staff='0' and access_date='" + logdate + "' and hostel_code='" + hostelcode + "') update  Bio_Attendance set time_in='" + InTime + "',time_out='" + OutTime + "',att='" + FnlAttnd + "' where roll_no='" + roll_no + "' and is_staff='0' and access_date='" + logdate + "' and hostel_code='" + hostelcode + "' else insert into Bio_Attendance(roll_no,time_in,time_out,is_staff,access_date,latestrec,hostel_code,att,mark_time) values('" + roll_no + "','" + InTime + "','" + OutTime + "','0','" + logdate + "','1','" + hostelcode + "','" + FnlAttnd + "','" + DateTime.Now + "')";
//                int upd = d2.update_method_wo_parameter(Query, "Text");
//                if (upd > 0)
//                {
//                    stucount++;
//                }
//            }
//            #endregion
//        }
//    }
//}

//public List<string> getdtlst()
//{
//    string alttime1 = "";
//    string alttime2 = "";
//    int i;
//    DataSet ds = new DataSet();
//    ds.Clear();
//    string selq = " select Convert(varchar(15),sending_Time,108) as sending_Time,Convert(varchar(15),alternate_time1,108) as alternate_time1,Convert(varchar(15),alternate_time2,108) as alternate_time2,college_code,sms_purpose from Automatic_SMS where IsSend='1' order by sending_Time";
//    ds = d2.select_method_wo_parameter(selq, "Text");
//    List<string> mylst = new List<string>();
//    mylst.Clear();
//    try
//    {
//        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
//        {
//            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
//            {
//                if (Convert.ToString(ds.Tables[0].Rows[i]["sms_purpose"]) == "Automatic Download And Mark Time Attendance Settings")
//                {
//                    TimeSpan time1 = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[i]["alternate_time1"])).TimeOfDay;
//                    string getTime1 = Convert.ToString(time1);
//                    TimeSpan time2 = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[i]["alternate_time2"])).TimeOfDay;
//                    string getTime2 = Convert.ToString(time2);
//                    if (getTime1.Trim() != "00:00:00" && getTime2.Trim() != "00:00:00")
//                    {
//                        alttime1 = Convert.ToString(ds.Tables[0].Rows[i]["alternate_time1"]);
//                        alttime2 = Convert.ToString(ds.Tables[0].Rows[i]["alternate_time2"]);
//                        mylst.Add(Convert.ToString(DateTime.Parse(Convert.ToString(ds.Tables[0].Rows[i]["sending_Time"])) + "-" + ds.Tables[0].Rows[i]["college_code"] + "-" + ds.Tables[0].Rows[i]["sms_purpose"]));
//                        mylst.Add(Convert.ToString(DateTime.Parse(Convert.ToString(ds.Tables[0].Rows[i]["alternate_time1"])) + "-" + ds.Tables[0].Rows[i]["college_code"] + "-" + ds.Tables[0].Rows[i]["sms_purpose"] + "-1"));
//                        mylst.Add(Convert.ToString(DateTime.Parse(Convert.ToString(ds.Tables[0].Rows[i]["alternate_time2"])) + "-" + ds.Tables[0].Rows[i]["college_code"] + "-" + ds.Tables[0].Rows[i]["sms_purpose"] + "-2"));
//                    }
//                    else
//                    {
//                        mylst.Add(Convert.ToString(DateTime.Parse(Convert.ToString(ds.Tables[0].Rows[i]["sending_Time"])) + "-" + ds.Tables[0].Rows[i]["college_code"] + "-" + ds.Tables[0].Rows[i]["sms_purpose"]));
//                    }
//                }
//                else
//                {
//                    mylst.Add(Convert.ToString(DateTime.Parse(Convert.ToString(ds.Tables[0].Rows[i]["sending_Time"])) + "-" + ds.Tables[0].Rows[i]["college_code"] + "-" + ds.Tables[0].Rows[i]["sms_purpose"]));
//                }
//            }
//        }
//    }
//    catch { }
//    return mylst;
//}

//public Dictionary<string, string> getdicval()
//{
//    Dictionary<string, string> dict = new Dictionary<string, string>();
//    dict.Clear();
//    try
//    {
//        int i;
//        string collcode = "";
//        string birthwishtime = "";
//        string studattndtime = "";
//        string cammarkstime = "";
//        string staffattndtime = "";
//        string blockboxtime = "";
//        string studatnshttime = "";
//        string hosstuattntime = "";
//        string studstaffattntime = "";
//        string alttime1 = "";
//        string alttime2 = "";

//        DataSet ds = new DataSet();
//        ds.Clear();
//        ds = getSMSSettings();
//        if (ds.Tables.Count > 0)
//        {
//            if (ds.Tables[0].Rows.Count > 0)
//            {
//                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
//                {
//                    birthwishtime = Convert.ToString(ds.Tables[0].Rows[i]["sending_Time"]);
//                    collcode = Convert.ToString(ds.Tables[0].Rows[i]["college_code"]);
//                    if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(birthwishtime) + "-" + collcode + "-Birthday Wishes")))
//                        dict.Add(Convert.ToString(DateTime.Parse(birthwishtime) + "-" + collcode + "-Birthday Wishes"), Convert.ToString(DateTime.Parse(birthwishtime) + "-" + collcode + "-Birthday Wishes"));
//                }
//            }
//            if (ds.Tables[1].Rows.Count > 0)
//            {
//                for (i = 0; i < ds.Tables[1].Rows.Count; i++)
//                {
//                    studattndtime = Convert.ToString(ds.Tables[1].Rows[i]["sending_Time"]);
//                    collcode = Convert.ToString(ds.Tables[1].Rows[i]["college_code"]);
//                    if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(studattndtime) + "-" + collcode + "-Student Attendance")))
//                        dict.Add(Convert.ToString(DateTime.Parse(studattndtime) + "-" + collcode + "-Student Attendance"), Convert.ToString(DateTime.Parse(studattndtime) + "-" + collcode + "-Student Attendance"));
//                }
//            }
//            if (ds.Tables[2].Rows.Count > 0)
//            {
//                for (i = 0; i < ds.Tables[2].Rows.Count; i++)
//                {
//                    cammarkstime = Convert.ToString(ds.Tables[2].Rows[i]["sending_Time"]);
//                    collcode = Convert.ToString(ds.Tables[2].Rows[i]["college_code"]);
//                    if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(cammarkstime) + "-" + collcode + "-CAM Marks")))
//                        dict.Add(Convert.ToString(DateTime.Parse(cammarkstime) + "-" + collcode + "-CAM Marks"), Convert.ToString(DateTime.Parse(cammarkstime) + "-" + collcode + "-CAM Marks"));
//                }
//            }
//            if (ds.Tables[3].Rows.Count > 0)
//            {
//                for (i = 0; i < ds.Tables[3].Rows.Count; i++)
//                {
//                    staffattndtime = Convert.ToString(ds.Tables[3].Rows[i]["sending_Time"]);
//                    collcode = Convert.ToString(ds.Tables[3].Rows[i]["college_code"]);
//                    if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(staffattndtime) + "-" + collcode + "-Staff Attendance")))
//                        dict.Add(Convert.ToString(DateTime.Parse(staffattndtime) + "-" + collcode + "-Staff Attendance"), Convert.ToString(DateTime.Parse(staffattndtime) + "-" + collcode + "-Staff Attendance"));
//                }
//            }
//            if (ds.Tables[4].Rows.Count > 0)
//            {
//                for (i = 0; i < ds.Tables[4].Rows.Count; i++)
//                {
//                    blockboxtime = Convert.ToString(ds.Tables[4].Rows[i]["sending_Time"]);
//                    collcode = Convert.ToString(ds.Tables[4].Rows[i]["college_code"]);
//                    if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(blockboxtime) + "-" + collcode + "-Block Box")))
//                        dict.Add(Convert.ToString(DateTime.Parse(blockboxtime) + "-" + collcode + "-Block Box"), Convert.ToString(DateTime.Parse(blockboxtime) + "-" + collcode + "-Block Box"));
//                }
//            }
//            if (ds.Tables[5].Rows.Count > 0)
//            {
//                for (i = 0; i < ds.Tables[5].Rows.Count; i++)
//                {
//                    studatnshttime = Convert.ToString(ds.Tables[5].Rows[i]["sending_Time"]);
//                    collcode = Convert.ToString(ds.Tables[5].Rows[i]["college_code"]);
//                    if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(studatnshttime) + "-" + collcode + "-Student Attendance Shortage")))
//                        dict.Add(Convert.ToString(DateTime.Parse(studatnshttime) + "-" + collcode + "-Student Attendance Shortage"), Convert.ToString(DateTime.Parse(studatnshttime) + "-" + collcode + "-Student Attendance Shortage"));
//                }
//            }
//            if (ds.Tables[6].Rows.Count > 0)
//            {
//                for (i = 0; i < ds.Tables[6].Rows.Count; i++)
//                {
//                    hosstuattntime = Convert.ToString(ds.Tables[6].Rows[i]["sending_Time"]);
//                    collcode = Convert.ToString(ds.Tables[6].Rows[i]["college_code"]);
//                    if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(hosstuattntime) + "-" + collcode + "-Hostel Student Attendance")))
//                        dict.Add(Convert.ToString(DateTime.Parse(hosstuattntime) + "-" + collcode + "-Hostel Student Attendance"), Convert.ToString(DateTime.Parse(hosstuattntime) + "-" + collcode + "-Hostel Student Attendance"));
//                }
//            }
//            if (ds.Tables[7].Rows.Count > 0)
//            {
//                for (i = 0; i < ds.Tables[7].Rows.Count; i++)
//                {
//                    studstaffattntime = Convert.ToString(ds.Tables[7].Rows[i]["sending_Time"]);
//                    collcode = Convert.ToString(ds.Tables[7].Rows[i]["college_code"]);
//                    if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(studstaffattntime) + "-" + collcode + "-Student/Staff Cumulative Attendance")))
//                        dict.Add(Convert.ToString(DateTime.Parse(studstaffattntime) + "-" + collcode + "-Student/Staff Cumulative Attendance"), Convert.ToString(DateTime.Parse(studstaffattntime) + "-" + collcode + "-Student/Staff Cumulative Attendance"));
//                }
//            }
//            if (ds.Tables[8].Rows.Count > 0)
//            {
//                for (i = 0; i < ds.Tables[8].Rows.Count; i++)
//                {
//                    collcode = Convert.ToString(ds.Tables[8].Rows[i]["college_code"]);
//                    studstaffattntime = Convert.ToString(ds.Tables[8].Rows[i]["sending_Time"]);
//                    TimeSpan time1 = Convert.ToDateTime(Convert.ToString(ds.Tables[8].Rows[i]["alternate_time1"])).TimeOfDay;
//                    string getTime1 = Convert.ToString(time1);
//                    TimeSpan time2 = Convert.ToDateTime(Convert.ToString(ds.Tables[8].Rows[i]["alternate_time2"])).TimeOfDay;
//                    string getTime2 = Convert.ToString(time2);
//                    if (getTime1.Trim() != "00:00:00" && getTime2.Trim() != "00:00:00")
//                    {
//                        alttime1 = Convert.ToString(ds.Tables[8].Rows[i]["alternate_time1"]);
//                        alttime2 = Convert.ToString(ds.Tables[8].Rows[i]["alternate_time2"]);
//                        if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(studstaffattntime) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings")))
//                            dict.Add(Convert.ToString(DateTime.Parse(studstaffattntime) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings"), Convert.ToString(DateTime.Parse(studstaffattntime) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings"));
//                        if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(alttime1) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings-1")))
//                            dict.Add(Convert.ToString(DateTime.Parse(alttime1) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings-1"), Convert.ToString(DateTime.Parse(alttime1) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings-1"));
//                        if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(alttime2) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings-2")))
//                            dict.Add(Convert.ToString(DateTime.Parse(alttime2) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings-2"), Convert.ToString(DateTime.Parse(alttime2) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings-2"));
//                    }
//                    else
//                    {
//                        if (!dict.ContainsKey(Convert.ToString(DateTime.Parse(studstaffattntime) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings")))
//                            dict.Add(Convert.ToString(DateTime.Parse(studstaffattntime) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings"), Convert.ToString(DateTime.Parse(studstaffattntime) + "-" + collcode + "-Automatic Download And Mark Time Attendance Settings"));
//                    }
//                }
//            }
//        }
//    }
//    catch { }
//    return dict;
//}

//public DataSet getSMSSettings()
//{
//    DataSet ds = new DataSet();
//    ds.Clear();
//    try
//    {
//        string selq = "select Convert(varchar(15),sending_Time,108) as sending_Time,college_code from Automatic_SMS where sms_purpose='Birthday Wishes' and IsSend='1'";
//        selq = selq + " select Convert(varchar(15),sending_Time,108) as sending_Time,college_code from Automatic_SMS where sms_purpose='Student Attendance' and IsSend='1'";
//        selq = selq + " select Convert(varchar(15),sending_Time,108) as sending_Time,college_code from Automatic_SMS where sms_purpose='CAM Marks' and IsSend='1'";
//        selq = selq + " select Convert(varchar(15),sending_Time,108) as sending_Time,college_code from Automatic_SMS where sms_purpose='Staff Attendance' and IsSend='1'";
//        selq = selq + " select Convert(varchar(15),sending_Time,108) as sending_Time,college_code from Automatic_SMS where sms_purpose='Block Box' and IsSend='1'";
//        selq = selq + " select Convert(varchar(15),sending_Time,108) as sending_Time,college_code from Automatic_SMS where sms_purpose='Student Attendance Shortage' and IsSend='1'";
//        selq = selq + " select Convert(varchar(15),sending_Time,108) as sending_Time,college_code from Automatic_SMS where sms_purpose='Hostel Student Attendance' and IsSend='1'";
//        selq = selq + " select Convert(varchar(15),sending_Time,108) as sending_Time,college_code from Automatic_SMS where sms_purpose='Student/Staff Cumulative Attendance' and IsSend='1'";
//        selq = selq + " select Convert(varchar(15),sending_Time,108) as sending_Time,Convert(varchar(15),alternate_time1,108) as alternate_time1,Convert(varchar(15),alternate_time2,108) as alternate_time2,college_code from Automatic_SMS where sms_purpose='Automatic Download And Mark Time Attendance Settings' and IsSend='1'";
//        ds = d2.select_method_wo_parameter(selq, "Text");
//    }
//    catch { }
//    return ds;
//}

//public void downloadlogs(string devname, DateTime frmdt, DateTime todt, string collcode)
//{
//    string devicename = "";
//    devicename = devname.Replace(",", "','");
//    string delq = "";
//    int delcount = 0;
//    string selq = "";
//    DataSet dsdown = new DataSet();
//    dsdown.Clear();
//    int bwcount = 0;
//    int colcount = 0;
//    int mybwcount = 0;
//    int mycolcount = 0;
//    int downcount = 0;

//    if (devicename.Trim() != "")
//    {
//        if (frmdt <= todt)
//        {
//            delq = delq + " DELETE Daily_Logs FROM DeviceInfo D WHERE Daily_Logs.MachineNo = D.MachineNo AND Log_Date BETWEEN '" + frmdt + "' and '" + todt + "'";
//            if (devicename.Trim() != "")
//            {
//                delq = delq + " and DeviceID in(" + devicename + ")";
//            }
//            delcount = d2.update_method_wo_parameter(delq, "Text");
//            selq = "Select * from DeviceInfo where College_Code='" + Convert.ToString(collcode) + "'";
//            if (devicename.Trim() != "")
//            {
//                selq = selq + " and DeviceID in(" + devicename + ")";
//            }
//            dsdown.Clear();
//            dsdown = d2.select_method_wo_parameter(selq, "Text");
//            if (dsdown.Tables.Count > 0 && dsdown.Tables[0].Rows.Count > 0)
//            {
//                for (int i = 0; i < dsdown.Tables[0].Rows.Count; i++)
//                {
//                    string ipno = Convert.ToString(dsdown.Tables[0].Rows[i]["IPAdd"]);
//                    int portno = 0;
//                    int newmacno = 0;
//                    Int32.TryParse(Convert.ToString(dsdown.Tables[0].Rows[i]["PortNo"]), out portno);
//                    Int32.TryParse(Convert.ToString(dsdown.Tables[0].Rows[i]["MachineNo"]), out newmacno);
//                   // CZKEM czkm = new CZKEM();
//                    ConnectDevice(ipno, portno, newmacno, czkm);
//                    if (Convert.ToString(dsdown.Tables[0].Rows[i]["DeviceType"]) == "0")
//                    {
//                        DownloadlogBW(newmacno, ipno, portno, czkm, frmdt, todt, out bwcount);
//                        mybwcount = mybwcount + bwcount;
//                    }
//                    else if (Convert.ToString(dsdown.Tables[0].Rows[i]["DeviceType"]) == "1")
//                    {
//                        Downloadlogcolor(newmacno, ipno, portno, czkm, frmdt, todt, out colcount);
//                        mycolcount = mycolcount + colcount;
//                    }
//                }
//                if (mybwcount > 0 || mycolcount > 0)
//                {
//                    download_attnlogs(frmdt, todt, out downcount);
//                    if (downcount > 0)
//                    {
//                        alertpopwindow.Visible = true;
//                        lblalerterr.Visible = true;
//                        lblalerterr.Text = "Downloaded Successfully!";
//                    }
//                    else
//                    {
//                        alertpopwindow.Visible = true;
//                        lblalerterr.Visible = true;
//                        lblalerterr.Text = "Download Failed,Please Try Again!";
//                    }
//                }
//                else
//                {
//                    alertpopwindow.Visible = true;
//                    lblalerterr.Visible = true;
//                    lblalerterr.Text = "No Records Found From Device!";
//                }
//            }
//            else
//            {
//                alertpopwindow.Visible = true;
//                lblalerterr.Visible = true;
//                lblalerterr.Text = "No Such Devices Found!";
//            }
//        }
//        else
//        {
//            alertpopwindow.Visible = true;
//            lblalerterr.Visible = true;
//            lblalerterr.Text = "Please Select a Valid Date!";
//        }
//    }
//    else
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = "Please Select Device Name!";
//    }
//}

//public void ConnectDevice(string IPNo, int PortNo, int macno, object czkm)
//{
//    try
//    {
//        CZKEM czk = (CZKEM)czkm;
//        string ver = "";
//        if (czk.Connect_Net(IPNo, PortNo))
//        {
//            if (czk.GetFirmwareVersion(macno, ref ver))
//            {
//                if (czk.GetDeviceIP(macno, IPNo))
//                {
//                    czk.RegEvent(macno, 32767);
//                }
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = ex.StackTrace;
//    }
//}

//public void DownloadlogBW(int macno, string IpNo, int portno, object czkm, DateTime frmdt, DateTime todt, out int bcount)
//{
//    bcount = 0;
//    try
//    {
//        CZKEM czk = (CZKEM)czkm;
//        int enrollno = 0;
//        int dwVerifyMode = 0;
//        int dwInOutMode = 0;
//        int dwYear = 0;
//        int dwMonth = 0;
//        int dwDay = 0;
//        int dwHour = 0;
//        int dwMinute = 0;
//        DateTime StrCDate = new DateTime();
//        string StrLogDate = "";
//        string StrLogTime = "";
//        DateTime dtsample = new DateTime();
//        string insq = "";
//        int inscount = 0;

//        StrCDate = frmdt;

//        if (czk.ReadAllGLogData(macno))
//        {
//            while (StrCDate <= todt)
//            {
//                while (czk.GetAllGLogData(macno, ref macno, ref enrollno, ref macno, ref dwVerifyMode, ref dwInOutMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute))
//                {
//                    if (dwYear == StrCDate.Year && dwMonth == StrCDate.Month && dwDay == StrCDate.Day)
//                    {
//                        StrLogDate = dwMonth + "/" + dwDay + "/" + dwYear;
//                        StrLogTime = dwHour + ":" + dwMinute;
//                        dtsample = Convert.ToDateTime(StrLogTime);
//                        StrLogTime = dtsample.ToString("hh:mm tt");
//                        insq = "insert into Daily_Logs(Log_Date,FingerID,MachineNo,LogTime) Values ('" + StrLogDate + "','" + enrollno + "','" + macno + "','" + StrLogTime + "')";
//                        inscount = d2.update_method_wo_parameter(insq, "Text");
//                        if (inscount > 0)
//                        {
//                            bcount++;
//                        }
//                    }
//                }
//                StrCDate = StrCDate.AddDays(1);
//                ConnectDevice(IpNo, portno, macno, czk);
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = ex.StackTrace;
//    }
//}

//public void Downloadlogcolor(int macno, string IpNo, int portno, object czkm, DateTime frmdt, DateTime todt, out int ccount)
//{
//    ccount = 0;
//    try
//    {
//        CZKEM czk = (CZKEM)czkm;
//        string enrollno = "0";
//        int dwVerifyMode = 0;
//        int dwInOutMode = 0;
//        int dwYear = 0;
//        int dwMonth = 0;
//        int dwDay = 0;
//        int dwHour = 0;
//        int dwMinute = 0;
//        int dwSecond = 0;
//        int dwWorkCode = 0;
//        DateTime StrCDate = new DateTime();
//        string StrLogDate = "";
//        string StrLogTime = "";
//        DateTime dtsample = new DateTime();
//        string insq = "";
//        int inscount = 0;

//        StrCDate = frmdt;

//        if (czk.ReadAllGLogData(macno))
//        {
//            while (StrCDate <= todt)
//            {
//                enrollno = "";
//                dwVerifyMode = 0;
//                dwInOutMode = 0;
//                while (czk.SSR_GetGeneralLogData(macno, out enrollno, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
//                {
//                    if (dwYear == StrCDate.Year && dwMonth == StrCDate.Month && dwDay == StrCDate.Day)
//                    {
//                        StrLogDate = dwMonth + "/" + dwDay + "/" + dwYear;
//                        StrLogTime = dwHour + ":" + dwMinute;
//                        dtsample = Convert.ToDateTime(StrLogTime);
//                        StrLogTime = dtsample.ToString("hh:mm tt");
//                        insq = "insert into Daily_Logs(Log_Date,FingerID,MachineNo,LogTime) Values ('" + StrLogDate + "','" + enrollno + "','" + macno + "','" + StrLogTime + "')";
//                        inscount = d2.update_method_wo_parameter(insq, "Text");
//                        if (inscount > 0)
//                        {
//                            ccount++;
//                        }
//                    }
//                }
//                StrCDate = StrCDate.AddDays(1);
//                ConnectDevice(IpNo, portno, macno, czk);
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        alertpopwindow.Visible = true;
//        lblalerterr.Visible = true;
//        lblalerterr.Text = ex.StackTrace;
//    }
//}

//public void download_attnlogs(DateTime frmdt, DateTime todt, out int dcount)
//{
//    dcount = 0;
//    DataSet dsfir = new DataSet();
//    DataSet dssec = new DataSet();

//    string insq = "";
//    string delq = "";
//    string selq = "";
//    int inscount = 0;
//    int delcount = 0;
//    delq = " DELETE FROM Attn_Logs WHERE Log_Date BETWEEN '" + frmdt + "' AND '" + todt + "'";
//    delcount = d2.update_method_wo_parameter(delq, "Text");

//    selq = "SELECT DISTINCT Log_Date,FingerID,LEN(FingerID) FROM Daily_Logs WHERE Log_Date BETWEEN '" + frmdt + "' AND '" + todt + "' Order By Log_Date,LEN(FingerID),FingerID ";
//    dsfir.Clear();
//    dsfir = d2.select_method_wo_parameter(selq, "Text");
//    if (dsfir.Tables.Count > 0 && dsfir.Tables[0].Rows.Count > 0)
//    {
//        for (int ik = 0; ik < dsfir.Tables[0].Rows.Count; ik++)
//        {
//            selq = "SELECT * FROM Daily_Logs WHERE Log_Date ='" + Convert.ToString(dsfir.Tables[0].Rows[ik]["Log_Date"]) + "' AND FingerID ='" + Convert.ToString(dsfir.Tables[0].Rows[ik]["FingerID"]) + "' Order By FingerID,Cast(LogTime as datetime) ";
//            dssec.Clear();
//            dssec = d2.select_method_wo_parameter(selq, "Text");
//            if (dssec.Tables.Count > 0 && dssec.Tables[0].Rows.Count > 0)
//            {
//                insq = "INSERT INTO Attn_Logs(Log_Date,FingerID,MachineNo,InTime,OutTime) Values ('" + Convert.ToString(dssec.Tables[0].Rows[0]["Log_Date"]) + "','" + Convert.ToString(dssec.Tables[0].Rows[0]["FingerID"]) + "','" + Convert.ToString(dssec.Tables[0].Rows[0]["MachineNo"]) + "','" + Convert.ToString(dssec.Tables[0].Rows[0]["LogTime"]) + "','" + Convert.ToString(dssec.Tables[0].Rows[dssec.Tables[0].Rows.Count - 1]["LogTime"]) + "')";
//                inscount = d2.update_method_wo_parameter(insq, "Text");
//                if (inscount > 0)
//                {
//                    dcount++;
//                }
//            }
//        }
//    }
//}

#endregion