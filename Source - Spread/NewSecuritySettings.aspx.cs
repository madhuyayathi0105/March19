using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using wc = System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;
using System.Data.SqlClient;

public partial class NewSecuritySettings : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    Hashtable ht = new Hashtable();
    InsproDirectAccess dir = new InsproDirectAccess();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods rs1 = new ReuasableMethods();
    DataSet ds = new DataSet();
    DAccess2 dacc = new DAccess2();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string Hostelcode = "";
    int selected_usercode = 0;
    string usergroup = string.Empty;
    Dictionary<int, string> diclib = new Dictionary<int, string>();
    string struser_gruop = string.Empty;
    string user_gropcode = string.Empty;

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex)
        {
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                DataSet dsload = d2.select_method_wo_parameter("select distinct coll_acronymn,college_code from collinfo order by college_code", "Text");
                if (dsload.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.Visible = true;
                    Label3.Visible = true;
                    ddlcollege.DataSource = dsload;
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataTextField = "coll_acronymn";
                    ddlcollege.DataBind();
                }
                else
                {
                    ddlcollege.Visible = false;
                    Label3.Visible = false;
                    ddlcollege.SelectedIndex = 0;
                }
                string staval = string.Empty;

                if (rdb_ind.Checked == true)
                {

                    staval = "select distinct user_code,user_id from usermaster where college_code=" + ddlcollege.Items[0].Value.ToString() + "  and group_code in('0','-1','')  order by user_id";
                }
                else if (rdb_grp.Checked == true)
                {

                    staval = "select distinct distinct group_code as user_code,groupname as user_code from groupmaster  order by groupname";
                }
                DataSet dsuse = d2.select_method_wo_parameter(staval, "Text");
                if (dsuse.Tables[0].Rows.Count > 0)
                {
                    ddluser.DataSource = dsuse;
                    ddluser.DataValueField = "user_code";
                    ddluser.DataTextField = "user_id";
                    ddluser.DataBind();
                }
                binddegree();
                bindbranch();
                bindbatch();
                bindsemester();
                bindCollegeACR();
                bindEduLevel();
                bindBatchYear();
                bindCollegeACR1();
                bindEduLevel1();
                examMntyr();
                bindCollegeACR2();
                bindEduLevel2();
                bindBatchsetting();
                examMntyr2();
                RadioButton1.Checked = true;
                btnAttendance_1_Click(sender, e);
                FpSpread1.Visible = false;
                btnSaveNew.Visible = false;
                txtdop.Text = DateTime.Now.ToString("dd/MM/yyyy");
                #region loadhostel
                int dropname = 0;
                bindhostel();
                bindcollege();
                switch (dropname)
                {
                    default:
                        dropname = 1;
                        loadHostelHeader(dropname);
                        loadHostelLedger(dropname);
                        dropname = 2;
                        loadHostelHeader(dropname);
                        loadHostelLedger(dropname);
                        dropname = 3;
                        loadHostelHeader(dropname);
                        loadHostelLedger(dropname);
                        dropname = 4;
                        loadHostelHeader(dropname);
                        loadHostelLedger(dropname);
                        dropname = 5;
                        loadHostelHeader(dropname);
                        loadHostelLedger(dropname);
                        dropname = 6;
                        loadHostelHeader(dropname);
                        loadHostelLedger(dropname);
                        dropname = 7;
                        loadHostelHeader(dropname);
                        loadHostelLedger(dropname);
                        break;

                }
                #endregion

                #region Attendance
                string stvfa = d2.GetFunctionv("select value from Master_Settings where settings = 'Include Redo student in Attendance'");
                if (stvfa.Trim() == "1")
                {
                    chkRedo.Checked = true;
                }
                else
                {
                    chkRedo.Checked = false;
                }

                //added by Mullai
                string stuotp = d2.GetFunctionv("select LinkValue from New_InsSettings where LinkName = 'student login otp'");
                if (stuotp.Trim() == "1")
                {
                    chkotp.Checked = true;
                }
                else
                {
                    chkotp.Checked = false;
                }


                #endregion



                #region Coe
                string val = d2.GetFunctionv("select value from Master_Settings where settings = 'ExcludeUnpaidStudents'");
                if (val.Trim() == "1")
                {
                    ChkDispMarks.Checked = true;
                }
                else
                {
                    ChkDispMarks.Checked = false;
                }
                string Selectstr = d2.GetFunction("select template from Master_Settings where settings = 'resultText'");
                if (!string.IsNullOrEmpty(Selectstr))
                {
                    txtResultNote.Text = Selectstr;
                }

                //added by Mullai
                string printlock = d2.GetFunctionv("select LinkValue from New_InsSettings where LinkName = 'MarkSheet Printlock'");
                if (printlock.Trim() == "1")
                {
                    chkprintlock.Checked = true;
                }
                else
                {
                    chkprintlock.Checked = false;
                }
                //***
                #endregion

                #region application
                //krishhna kumar.r
                string appliction1 = d2.GetFunctionv("select value from Master_Settings where settings = 'Include Eligibility MarkSetting'");
                if (appliction1.Trim() == "1")
                {
                    chkapplication.Checked = true;
                }
                else
                {
                    chkapplication.Checked = false;
                }
                string applicationSelectstr = d2.GetFunction("select template from Master_Settings where settings = 'rInclude Eligibility MarkSetting'");
                if (!string.IsNullOrEmpty(applicationSelectstr))
                {
                    txtResultNote.Text = applicationSelectstr;
                }
                #endregion

                #region Library

                loadLibraryHeader();
                loadLibraryLedger();
                loadLibraryLostHeader();
                loadLibraryLostLedger();

                #endregion


                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedValue);
                }
                else
                {
                    collegecode = "13";
                }
                if (singleuser.ToLower() == "true")
                {
                    rdb_ind.Checked = true;
                }
                else if (group_user.ToLower() == "true")
                {
                    rdb_grp.Checked = true;
                }

            }

        }
        catch (Exception ex)
        {
        }
        if (ddlcollege.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddlcollege.SelectedValue);
        }
        else
        {
            collegecode = "13";
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strcoll = string.Empty;
        if (rdb_ind.Checked == true)
        {

            strcoll = "select distinct user_code,user_id from usermaster where college_code=" + ddlcollege.SelectedItem.Value.ToString() + "  and group_code in('0','-1','')  order by user_id";
        }
        else if (rdb_grp.Checked == true)
        {

            strcoll = "select distinct group_code as user_code,groupname as user_id from groupmaster  order by groupname";
        }
        DataSet dsufse = d2.select_method_wo_parameter(strcoll, "Text");
        if (dsufse.Tables[0].Rows.Count > 0)
        {
            ddluser.DataSource = dsufse;
            ddluser.DataValueField = "user_code";
            ddluser.DataTextField = "user_id";
            ddluser.DataBind();
        }
    }

    protected void txtuser_TextChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
        }
    }

    protected void chk_alluser_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_alluser.Checked == true)
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddluser.Items)
                {
                    li.Selected = true;
                    txtuser.Text = "User(" + (ddluser.Items.Count) + ")";
                }
            }
            else
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddluser.Items)
                {
                    li.Selected = false;
                    txtuser.Text = "- - Select - -";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddluser_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            int count = 0;
            for (int i = 0; i < ddluser.Items.Count; i++)
            {
                if (ddluser.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                chk_alluser.Checked = false;
            }
            else if (count == ddluser.Items.Count)
            {
                chk_alluser.Checked = true;
                txtuser.Text = "User(" + (ddluser.Items.Count) + ")";
            }
            else
            {
                chk_alluser.Checked = false;
                txtuser.Text = "User(" + count + ")";
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void rdb_ind_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ddlcollege_SelectedIndexChanged(sender, e);

        }
        catch (Exception ex)
        {
        }
    }

    protected void rdb_grp_CheckedChanged(object sender, EventArgs e)
    {

        try
        {
            ddlcollege_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
        }
    }

    public void clearDivisions()
    {
        divAttendance.Visible = true;
        btnAttendance_1.BackColor = ColorTranslator.FromHtml("#A537D1");
        divCOE.Visible = false;
        btnCOE_2.BackColor = ColorTranslator.FromHtml("#A537D1");
        divFinance.Visible = false;
        btnFinancePrint_3.BackColor = ColorTranslator.FromHtml("#A537D1");
        divHR.Visible = false;
        btnHR_4.BackColor = ColorTranslator.FromHtml("#A537D1");
        divTransport.Visible = false;
        btnTransRemind_5.BackColor = ColorTranslator.FromHtml("#A537D1");
        divHostel.Visible = false;
        btnHostel_6.BackColor = ColorTranslator.FromHtml("#A537D1");
        divapplication.Visible = false;
        btnadmesion_7.BackColor = ColorTranslator.FromHtml("#A537D1");
        divinventory.Visible = false;
        Inventory.BackColor = ColorTranslator.FromHtml("#A537D1");//added by abarna on 3.05.2018
        divLibrary.Visible = false;
        Library.BackColor = ColorTranslator.FromHtml("#A537D1");
        divMblApp.Visible = false;//Deepali 16.7.18
        btnMblApp_10.BackColor = ColorTranslator.FromHtml("#A537D1");
        switch (TabContainer1.SelectedIndex)
        {

            case 0:

            case 1:
                divAttendance.Visible = true;
                btnAttendance_1.BackColor = ColorTranslator.FromHtml("#EB162C");
                break;
            case 2:
                divCOE.Visible = true;
                btnCOE_2.BackColor = ColorTranslator.FromHtml("#EB162C");
                break;
            case 3:
                divFinance.Visible = true;
                btnFinancePrint_3.BackColor = ColorTranslator.FromHtml("#EB162C");
                break;
            case 4:
                divHR.Visible = true;
                btnHR_4.BackColor = ColorTranslator.FromHtml("#EB162C");
                break;
            case 5:
                divTransport.Visible = true;
                btnTransRemind_5.BackColor = ColorTranslator.FromHtml("#EB162C");
                break;
            case 6:
                divHostel.Visible = true;
                btnHostel_6.BackColor = ColorTranslator.FromHtml("#EB162C");
                break;
            case 7:
                divapplication.Visible = true;
                btnadmesion_7.BackColor = ColorTranslator.FromHtml("#EB162C");
                break;//added by Kowshi
            case 8:
                divinventory.Visible = true;
                Inventory.BackColor = ColorTranslator.FromHtml("#EB162C");
                break;//added by abarna on 3.05.2018
            case 9:
                divLibrary.Visible = true;
                Library.BackColor = ColorTranslator.FromHtml("#EB162C");
                break;//added by saranya on 13.06.2018

        }
    }

    protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
    {
        clearDivisions();
        try
        {
            btnsave_coe.Visible = false;
            btnAttendance.Visible = false;
            btnFinance.Visible = false;
            btnHR.Visible = false;
            btnTransport.Visible = false;
            btnHostel.Visible = false;
            btnapplication.Visible = false;
            divCOE.Visible = false;
            divAttendance.Visible = false;
            divFinance.Visible = false;
            divHR.Visible = false;
            divTransport.Visible = false;
            divHostel.Visible = false;

            //krishhna kumar.r
            divapplication.Visible = false;
            divinventory.Visible = false;//added by abarna on 3.05.2018
            btninventory.Visible = false;
            divLibrary.Visible = false;
            btnLibrary.Visible = false;
            btnMblAppSave.Visible = false;//Deepali 16.7.18
            if (TabContainer1.SelectedIndex == 1)
            {
                btnAttendance.Visible = true;
                divAttendance.Visible = true;
            }
            if (TabContainer1.SelectedIndex == 2)
            {
                divCOE.Visible = true;
                btnsave_coe.Visible = true;
            }
            if (TabContainer1.SelectedIndex == 3)
            {
                btnFinance.Visible = true;
                divFinance.Visible = true;
            }
            if (TabContainer1.SelectedIndex == 4)
            {
                divHR.Visible = true;
                btnHR.Visible = true;
                ReturnSettingDetails();//SD_23/2019
            }
            if (TabContainer1.SelectedIndex == 5)
            {
                divTransport.Visible = true;
                btnTransport.Visible = true;
            }
            if (TabContainer1.SelectedIndex == 6)
            {
                divHostel.Visible = true;
                btnHostel.Visible = true;
            }
            if (TabContainer1.SelectedIndex == 7)
            {
                divapplication.Visible = true;
                btnapplication.Visible = true;
            }
            if (TabContainer1.SelectedIndex == 8)
            {
                divinventory.Visible = true;
                btninventory.Visible = true;
                bindGrid();
            }//added by abarna on 3.05.2018
            //added by saranya on 13/06/2018
            if (TabContainer1.SelectedIndex == 9)
            {
                divLibrary.Visible = true;
                btnLibrary.Visible = true;
            }
            if (TabContainer1.SelectedIndex == 10)//Deepali 16.7.18
            {
                divMblApp.Visible = true;
                btnMblAppSave.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {

        if (rdb_ind.Checked == true)
        {
            Session["single_user"] = "true";
            Session["group_code"] = "false";
        }
        else if (rdb_grp.Checked == true)
        {
            Session["group_code"] = "true";
            Session["single_user"] = "false";
        }
        string usrcd = string.Empty;
        for (int i = 0; i < ddluser.Items.Count; i++)
        {
            if (ddluser.Items[i].Selected == true)
            {
                selected_usercode = Convert.ToInt16(ddluser.Items[i].Value);
                if (rdb_ind.Checked == true)
                {
                    usergroup = "  usercode=" + selected_usercode.ToString() + "";
                    usrcd = "  usercode=" + selected_usercode.ToString() + "";
                }
                else if (rdb_grp.Checked == true)
                {
                    usergroup = "  group_code=" + selected_usercode.ToString() + "";
                    usrcd = "  usercode=" + selected_usercode.ToString() + "";
                }

                txtOdlock.Text = string.Empty;
                string getOdlockva = d2.GetFunctionv("select value from Master_Settings where settings='OD Lock Days' and " + usergroup + "");
                if (getOdlockva.Trim().ToLower() != "")
                {
                    txtOdlock.Text = getOdlockva;
                }
                string val1 = d2.GetFunctionv("select value from Master_Settings where settings = 'include gpa for fail student' and " + usergroup + "");
                if (val1.Trim() == "1")
                {
                    chkfailGpa.Checked = true;
                }
                else
                {
                    chkfailGpa.Checked = false;
                }

                string atndbatyr = d2.GetFunctionv("select value from Master_Settings where settings='Attendance lock with batch year' and " + usrcd + "");
                if (atndbatyr.Trim() == "1")
                {
                    txtbatyr.Enabled = true;
                    cbatndbatyr.Checked = true;
                    string bat = d2.GetFunctionv("select template from Master_Settings where settings='Attendance lock with batch year' and " + usrcd + "");
                    string[] splt = bat.Split(',');
                    cbbatyr.Checked = false;
                    cblbatyr.ClearSelection();
                    for (int j1 = 0; j1 < splt.Length; j1++)
                    {
                        string yr = splt[j1].ToString();

                        cblbatyr.Items.FindByText(yr).Selected = true;


                    }
                    txtbatyr.Text = "batch(" + splt.Length + ")";
                }
                else
                {
                    txtbatyr.Enabled = false;
                    cbatndbatyr.Checked = false;
                }
            }

        }
        //inventory();

        #region Library

        string editQ = "select * from lib_user_perm where user_code = '" + ddluser.SelectedValue + "'";
        DataSet edit = new DataSet();

        edit = dacc.select_method_wo_parameter(editQ, "Text");

        if (edit.Tables[0].Rows.Count > 0)
        {
            string opacprint = Convert.ToString(edit.Tables[0].Rows[0]["mas_print"]);
            if (!string.IsNullOrEmpty(opacprint))
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["mas_print"]) == 1)
                    cb_opacprint.Checked = true;
                else
                    cb_opacprint.Checked = false;
            }
            string spIssue = Convert.ToString(edit.Tables[0].Rows[0]["sp_issue"]);
            if (!string.IsNullOrEmpty(spIssue))
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["sp_issue"]) == 1)
                    cb_specialissue.Checked = true;
                else
                    cb_specialissue.Checked = false;
            }
            string spReturn = Convert.ToString(edit.Tables[0].Rows[0]["sp_return"]);
            if (!string.IsNullOrEmpty(spReturn))
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["sp_return"]) == 1)
                    cb_specialreturn.Checked = true;
                else
                    cb_specialreturn.Checked = false;
            }
            string ResPrint = Convert.ToString(edit.Tables[0].Rows[0]["res_print"]);
            if (!string.IsNullOrEmpty(ResPrint))
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["res_print"]) == 1)
                    cb_reservation_print.Checked = true;
                else
                    cb_reservation_print.Checked = false;
            }
            string req_print = Convert.ToString(edit.Tables[0].Rows[0]["req_print"]);
            if (!string.IsNullOrEmpty(req_print))
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["req_print"]) == 1)
                    cb_newrequest_print.Checked = true;
                else
                    cb_newrequest_print.Checked = false;
            }
            string res_dele = Convert.ToString(edit.Tables[0].Rows[0]["res_dele"]);
            if (!string.IsNullOrEmpty(res_dele))
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["res_dele"]) == 1)
                    cb_reservation_delete.Checked = true;
                else
                    cb_reservation_delete.Checked = false;
            }
            string req_dele = Convert.ToString(edit.Tables[0].Rows[0]["req_dele"]);
            if (!string.IsNullOrEmpty(req_dele))
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["req_dele"]) == 1)
                    cb_newrequest_delete.Checked = true;
                else
                    cb_newrequest_delete.Checked = false;
            }
            string call_entry = Convert.ToString(edit.Tables[0].Rows[0]["call_entry"]);
            if (!string.IsNullOrEmpty(call_entry))
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["call_entry"]) == 1)
                    cb_manualcallnoentry.Checked = true;
                else
                    cb_manualcallnoentry.Checked = false;
            }
            string sp_fine = Convert.ToString(edit.Tables[0].Rows[0]["sp_fine"]);
            if (!string.IsNullOrEmpty(sp_fine))
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["sp_fine"]) == 1)
                    cb_editfine.Checked = true;
                else
                    cb_editfine.Checked = false;
            }
            string BarCodeTrans = Convert.ToString(edit.Tables[0].Rows[0]["BarCodeTrans"]);
            if (!string.IsNullOrEmpty(BarCodeTrans))
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["BarCodeTrans"]) == 1)
                    cb_transactionwithbarcode.Checked = true;
                else
                    cb_transactionwithbarcode.Checked = false;
            }
            string Cancel_Fine = Convert.ToString(edit.Tables[0].Rows[0]["Cancel_Fine"]);
            if (!string.IsNullOrEmpty(Cancel_Fine))
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["Cancel_Fine"]) == 1)
                    cb_editcancelfine.Checked = true;
                else
                    cb_editcancelfine.Checked = false;
            }
            //Added By Saranyadevi27.8.2018
            string ODStudSMS = Convert.ToString(edit.Tables[0].Rows[0]["ODStudSMS"]);
            if (ODStudSMS != "")
            {
                if (Convert.ToInt32(edit.Tables[0].Rows[0]["ODStudSMS"]) == 1)
                    cb_sendsmstostudentsforoverdue.Checked = true;
                else
                    cb_sendsmstostudentsforoverdue.Checked = false;
            }

            string ODStaffSMS = Convert.ToString(edit.Tables[0].Rows[0]["ODStaffSMS"]);
            {
                if (ODStaffSMS != "")
                    if (Convert.ToInt32(edit.Tables[0].Rows[0]["ODStaffSMS"]) == 1)
                        cb_sendsmstostaffsforoverdue.Checked = true;
                    else
                        cb_sendsmstostaffsforoverdue.Checked = false;
            }
        }
        #endregion

        DataSet ds = new DataSet();
        ds.Clear();
        string query = string.Empty;

        query = "select * from New_InsSettings where LinkName='Hostel Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'";

        ds = d2.select_method_wo_parameter(query, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string gethostalval = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
            string hostalval = string.Empty;

            if (gethostalval != "" || gethostalval != "0")
            {
                if (gethostalval.Contains(','))
                {
                    int count = 0;
                    string[] splival = gethostalval.Split(',');
                    if (splival.Length > 0)
                    {
                        for (int i = 0; i < splival.Count(); i++)
                        {
                            string gerhostal = Convert.ToString(splival[i]);
                            for (int j = 0; j < cbl_hos.Items.Count; j++)
                            {
                                string getcbhostalval = Convert.ToString(cbl_hos.Items[j].Value);
                                if (gerhostal == getcbhostalval)
                                {
                                    cbl_hos.Items[j].Selected = true;
                                    count++;
                                    if (hostalval == "")
                                    {
                                        hostalval = getcbhostalval;
                                    }
                                    else
                                    {
                                        hostalval = hostalval + "','" + getcbhostalval;
                                    }
                                }
                            }
                        }
                        if (count > 0)
                        {
                            txt_messname.Text = "Hostel(" + count + ")";
                        }
                    }
                }
                else
                {
                    int count = 0;
                    for (int j = 0; j < cbl_hos.Items.Count; j++)
                    {
                        string getcbhostalval = Convert.ToString(cbl_hos.Items[j].Value);
                        if (gethostalval == getcbhostalval)
                        {
                            cbl_hos.Items[j].Selected = true;
                            count++;
                            hostalval = gethostalval;
                        }
                    }
                    if (count > 0)
                    {
                        txt_messname.Text = "Hostel(" + count + ")";
                    }
                }
            }
            clgbuild(hostalval);
        }

        ds.Clear();
        ds.Reset();
        query = "select * from New_InsSettings where LinkName='Building Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'";

        ds = d2.select_method_wo_parameter(query, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string getbuildval = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
            string buildingvalval = string.Empty;
            if (getbuildval != "" || getbuildval != "0")
            {
                if (getbuildval.Contains(','))
                {
                    int count = 0;
                    string[] splival = getbuildval.Split(',');
                    if (splival.Length > 0)
                    {
                        for (int i = 0; i < splival.Count(); i++)
                        {
                            string gerhostal = Convert.ToString(splival[i]);
                            for (int j = 0; j < cbl_buildname.Items.Count; j++)
                            {
                                string getcbhostalval = Convert.ToString(cbl_buildname.Items[j].Value);
                                string build1 = cbl_buildname.Items[j].Text.ToString();
                                if (gerhostal == getcbhostalval)
                                {
                                    cbl_buildname.Items[j].Selected = true;
                                    count++;
                                    if (buildingvalval == "")
                                    {
                                        buildingvalval = build1;
                                    }
                                    else
                                    {
                                        buildingvalval = buildingvalval + "','" + build1;
                                    }
                                }
                            }
                        }
                        if (count > 0)
                        {
                            txt_buildingname.Text = "Building(" + count + ")";
                        }
                    }
                }
                else
                {
                    int count = 0;
                    for (int j = 0; j < cbl_buildname.Items.Count; j++)
                    {
                        string getcbhostalval = Convert.ToString(cbl_buildname.Items[j].Value);
                        string build1 = cbl_buildname.Items[j].Text.ToString();
                        if (getbuildval == getcbhostalval)
                        {
                            cbl_buildname.Items[j].Selected = true;
                            count++;
                            buildingvalval = build1;
                        }
                    }
                    if (count > 0)
                    {
                        txt_buildingname.Text = "Building(" + count + ")";
                    }
                }
            }
            clgfloor(buildingvalval);
        }
        ds.Clear();
        ds.Reset();
        query = "select * from New_InsSettings where LinkName='Floor Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'";

        ds = d2.select_method_wo_parameter(query, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string getbuildval = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
            string floorval = string.Empty;

            if (getbuildval != "" || getbuildval != "0")
            {
                if (getbuildval.Contains(','))
                {
                    int count = 0;
                    string[] splival = getbuildval.Split(',');
                    if (splival.Length > 0)
                    {
                        for (int i = 0; i < splival.Count(); i++)
                        {
                            string gerhostal = Convert.ToString(splival[i]);
                            for (int j = 0; j < cbl_floorname.Items.Count; j++)
                            {
                                string getcbhostalval = Convert.ToString(cbl_floorname.Items[j].Value);
                                string floor = cbl_floorname.Items[j].Text.ToString();
                                if (gerhostal == getcbhostalval)
                                {
                                    cbl_floorname.Items[j].Selected = true;
                                    count++;
                                    if (floorval == "")
                                    {
                                        floorval = floor;
                                    }
                                    else
                                    {
                                        floorval = floorval + "','" + floor;
                                    }
                                }
                            }
                        }
                        if (count > 0)
                        {
                            txt_floorname.Text = "Floor(" + count + ")";
                        }
                    }
                }
                else
                {
                    int count = 0;
                    for (int j = 0; j < cbl_floorname.Items.Count; j++)
                    {
                        string getcbhostalval = Convert.ToString(cbl_floorname.Items[j].Value);
                        string floor = cbl_floorname.Items[j].Text.ToString();
                        if (getbuildval == getcbhostalval)
                        {
                            cbl_floorname.Items[j].Selected = true;
                            count++;
                            floorval = floor;
                        }
                    }
                    if (count > 0)
                    {
                        txt_floorname.Text = "Floor(" + count + ")";
                    }
                }
            }
            string building = string.Empty;
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    string build = string.Empty;
                    build = cbl_buildname.Items[i].Text.ToString();
                    if (building == "")
                    {
                        building = build;
                    }
                    else
                    {
                        building = building + "'" + "," + "'" + build;
                    }
                }
            }
            clgroom(floorval, building);
        }
        ds.Clear();
        ds.Reset();
        query = "select * from New_InsSettings where LinkName='Room Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'";

        ds = d2.select_method_wo_parameter(query, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string getbuildval = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
            string roomval = string.Empty;

            if (getbuildval != "" || getbuildval != "0")
            {
                if (getbuildval.Contains(','))
                {
                    int count = 0;
                    string[] splival = getbuildval.Split(',');
                    if (splival.Length > 0)
                    {
                        for (int i = 0; i < splival.Count(); i++)
                        {
                            string gerhostal = Convert.ToString(splival[i]);
                            for (int j = 0; j < cbl_roomname.Items.Count; j++)
                            {
                                string getcbhostalval = Convert.ToString(cbl_roomname.Items[j].Value);
                                string rooms = cbl_roomname.Items[j].Text.ToString();
                                if (gerhostal == getcbhostalval)
                                {
                                    cbl_roomname.Items[j].Selected = true;
                                    count++;
                                    if (roomval == "")
                                    {
                                        roomval = rooms;
                                    }
                                    else
                                    {
                                        roomval = roomval + "','" + rooms;
                                    }
                                }
                            }
                        }
                        if (count > 0)
                        {
                            txt_roomname.Text = "Room(" + count + ")";
                        }
                    }
                }
                else
                {
                    int count = 0;
                    for (int j = 0; j < cbl_roomname.Items.Count; j++)
                    {
                        string getcbhostalval = Convert.ToString(cbl_roomname.Items[j].Value);
                        string rooms = cbl_roomname.Items[j].Text.ToString();
                        if (getbuildval == getcbhostalval)
                        {
                            cbl_roomname.Items[j].Selected = true;
                            count++;
                            roomval = rooms;
                        }
                    }
                    if (count > 0)
                    {
                        txt_roomname.Text = "Room(" + count + ")";
                    }
                }
            }
        }

        #region gatepass
        if (ddl_Hostel.Items.Count > 0)
        {
            int count = 0;
            string hoste = Convert.ToString(ddl_Hostel.SelectedValue);
            string cun = d2.GetFunction("select HostelGatePassPerCount from  HM_HostelMaster  where HostelMasterPK='" + Convert.ToString(ddl_Hostel.SelectedValue) + "'");
            txt_count.Text = cun;
        }
        if (ddlcollegeco.Items.Count > 0)
        {
            int count = 0;
            string hoste = Convert.ToString(ddlcollegeco.SelectedValue);
            string cun = d2.GetFunction("select leavecount from gatepasscount where college_code='" + Convert.ToString(ddlcollegeco.SelectedValue) + "'");
            Txtcol_count.Text = cun;
        }
        #endregion

        //Added By Saranyadevi4.3.2019
        #region HostelVacateRights

        if (ddlcollege.SelectedItem.Value.ToString() != "" && selected_usercode.ToString() != "")
        {
            string LinkValue = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Vacate Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ");
            if (LinkValue != "")
            {
                if (LinkValue == "0")
                    RadioButtonHostel.Items[0].Selected = true;
                else
                    RadioButtonHostel.Items[1].Selected = true;

            }

        }
        #endregion

        string insqry1 = "select LinkValue from New_InsSettings where LinkName='IncludeShiftName' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ";
        string fees = d2.GetFunction(insqry1).Trim();
        if (fees == "1")
            rb_WithFees.Checked = true;
        else
            rb_WithoutFees.Checked = true;

        #region Mobile App
        //Deepali 16.7.18
        loadExistingMblAppTabRights();
        loadExistingStudentMblAppTabRights();
        bind_FN_AN_Hour();
        loadExisting_FN_AN();
        loadExisting_DueFee();
        #endregion

        #region financetab
        #region year
        string usercode = ddluser.SelectedValue;
        if (d2.GetFunction("select LinkValue from New_InsSettings where LinkName='YearwiseSetting' and user_code ='" + usercode + "' and college_code ='" + Convert.ToString(ddlcollege.SelectedValue) + "'") == "0")
        {
            Year.Checked = false;
        }
        else
        {
            Year.Checked = true;
        }
        #endregion



        //added by abarna
        string print = d2.GetFunctionv("select LinkValue from New_InsSettings where LinkName = 'PrintBasedUser' and  user_code ='" + usercode + "' and college_code ='" + Convert.ToString(ddlcollege.SelectedValue) + "' ");
        if (print.Trim() == "1")
        {
            chkprint.Checked = true;
        }
        else
        {
            chkprint.Checked = false;
        }
        //***









        #region semester
        usercode = ddluser.SelectedValue;
        if (d2.GetFunction("select LinkValue from New_InsSettings where LinkName='SemesterWiseSetting' and user_code ='" + usercode + "' and college_code ='" + Convert.ToString(ddlcollege.SelectedValue) + "'") == "0")
        {
            Semester.Checked = false;
        }
        else
        {
            Semester.Checked = true;
        }
        #endregion
        #endregion

        #region
        DataTable gdvheaders = new DataTable();
        gdvheaders.Columns.Add("S.No");
        gdvheaders.Columns.Add("actualgrade");
        gdvheaders.Columns.Add("result");
        gdvheaders.Columns.Add("Grade");

        DataRow dr = null;



        Hashtable hs = new Hashtable();
        hs.Add(1, "SA");
        hs.Add(2, "RA");
        hs.Add(3, "W");
        hs.Add(4, "UA");
        hs.Add(5, "U");

        string gradesett = "select * from gradesettings where college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "'";
        DataSet ds1 = d2.select_method_wo_parameter(gradesett, "text");
        if (ds1.Tables[0].Rows.Count > 0)
        {
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int cun = 0;
                for (int m = 0; m < ds1.Tables[0].Rows.Count; m++)
                {
                    if (!hs.ContainsValue(ds1.Tables[0].Rows[m]["ActualGrade"]))
                    {
                        cun++;
                        if (cun == 1)
                            cun += 5;
                        else
                            cun++;


                        hs.Add(cun, ds1.Tables[0].Rows[m]["ActualGrade"]);
                    }
                }
            }
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < hs.Count; i++)
                {

                    dr = gdvheaders.NewRow();
                    dr["actualgrade"] = hs[i + 1];

                    //  string actgrade = dayy.Text;
                    ds1.Tables[0].DefaultView.RowFilter = "ActualGrade='" + hs[i + 1] + "'";

                    DataView dvholiday = ds1.Tables[0].DefaultView;
                    if (dvholiday.Count > 0)
                    {
                        dr["Grade"] = Convert.ToString(dvholiday[0]["grade"]);
                        dr["result"] = Convert.ToString(dvholiday[0]["Result"]);
                    }
                    gdvheaders.Rows.Add(dr);

                }
                gridView2.Visible = true;
                ViewState["CurrentTable"] = gdvheaders;
                gridView2.DataSource = gdvheaders;
                gridView2.DataBind();
                for (int gr = 0; gr < gridView2.Rows.Count; gr++)
                {
                    TextBox dayy = (TextBox)gridView2.Rows[gr].FindControl("lblActual");
                    dayy.Enabled = false;
                }
            }
        }
        else if (ds1.Tables[0].Rows.Count == 0)
        {

            for (int i = 0; i < hs.Count; i++)
            {

                dr = gdvheaders.NewRow();
                dr["actualgrade"] = hs[i + 1];
                gdvheaders.Rows.Add(dr);
            }
            gridView2.Visible = true;
            ViewState["CurrentTable"] = gdvheaders;
            gridView2.DataSource = gdvheaders;
            gridView2.DataBind();

        }
        #endregion

        #region Invigilator_Travel_Allowance
        DataSet dsInvig = new DataSet();
        string Invigamt = "select Min_Kilometer,min_Amount,Per_Kilometer,Per_Amount from Invigilator_Travel_setting where college_code='" + ddlcollege.SelectedItem.Value + "'";
        dsInvig.Clear();
        dsInvig = d2.select_method_wo_parameter(Invigamt, "Text");
        if (dsInvig.Tables.Count > 0 && dsInvig.Tables[0].Rows.Count > 0)
        {
            Text_minkm.Text = Convert.ToString(dsInvig.Tables[0].Rows[0]["Min_Kilometer"]);
            Text_minAmt.Text = Convert.ToString(dsInvig.Tables[0].Rows[0]["min_Amount"]);
            Text_Perkm.Text = Convert.ToString(dsInvig.Tables[0].Rows[0]["Per_Kilometer"]);
            Text_peramt.Text = Convert.ToString(dsInvig.Tables[0].Rows[0]["Per_Amount"]);

        }
        #endregion
    }
    protected void Addnew_Click(object sender, EventArgs e)
    {
        try
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {

                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {

                        //extract the TextBox values

                        TextBox box1 = (TextBox)gridView2.Rows[rowIndex].Cells[1].FindControl("lblActual");

                        TextBox box2 = (TextBox)gridView2.Rows[rowIndex].Cells[2].FindControl("txtgrade");

                        TextBox box3 = (TextBox)gridView2.Rows[rowIndex].Cells[3].FindControl("txtResult");
                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows[i - 1]["actualgrade"] = box1.Text;

                        dtCurrentTable.Rows[i - 1]["grade"] = box2.Text;

                        dtCurrentTable.Rows[i - 1]["result"] = box3.Text;



                        rowIndex++;

                    }

                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable"] = dtCurrentTable;



                    gridView2.DataSource = dtCurrentTable;

                    gridView2.DataBind();
                    for (int gr = 0; gr < gridView2.Rows.Count - 1; gr++)
                    {
                        TextBox dayy = (TextBox)gridView2.Rows[gr].FindControl("lblActual");
                        dayy.Enabled = false;
                    }

                }
            }

        }
        catch
        {

        }
    }
    protected void Btngradesave_Click(object sender, EventArgs e)
    {
        try
        {
            for (int gr = 0; gr < gridView2.Rows.Count; gr++)
            {
                TextBox dayy = (TextBox)gridView2.Rows[gr].FindControl("lblActual");
                string actgrade = dayy.Text;

                TextBox grad = (TextBox)gridView2.Rows[gr].FindControl("txtgrade");
                string grade = grad.Text;
                TextBox resul = (TextBox)gridView2.Rows[gr].FindControl("txtResult");
                string result = resul.Text;
                string ins = "if exists(select * from gradesettings where ActualGrade='" + actgrade + "' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "') update gradesettings set grade='" + grade + "', Result='" + result + "' where ActualGrade='" + actgrade + "' and college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' else  insert into gradesettings (ActualGrade,grade,Result,college_code) values ('" + actgrade + "','" + grade + "','" + result + "','" + Convert.ToString(ddlcollege.SelectedValue) + "')";
                int retu = d2.update_method_wo_parameter(ins, "Text");
                if (retu == 1)
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            }
        }
        catch
        {

        }
    }
    protected void btnAttendance_1_Click(object sender, EventArgs e)
    {
        TabContainer1.SelectedIndex = 1;
        TabContainer1_ActiveTabChanged(sender, e);
    }

    protected void btnCOE_2_Click(object sender, EventArgs e)
    {
        TabContainer1.SelectedIndex = 2;
        TabContainer1_ActiveTabChanged(sender, e);
    }

    protected void btnFinancePrint_3_Click(object sender, EventArgs e)
    {
        TabContainer1.SelectedIndex = 3;
        TabContainer1_ActiveTabChanged(sender, e);
        loadcollege();
        loadheaderandledger();
        ledgerload();
        //loadfinanceyear();
        loadsem();
        loadOnlineHeaders();
        loadOnlineLedgers();
    }

    protected void btnHR_4_Click(object sender, EventArgs e)
    {
        TabContainer1.SelectedIndex = 4;
        TabContainer1_ActiveTabChanged(sender, e);
    }

    protected void btnTransRemind_5_Click(object sender, EventArgs e)
    {
        TabContainer1.SelectedIndex = 5;
        TabContainer1_ActiveTabChanged(sender, e);
    }

    protected void btnHostel_6_Click(object sender, EventArgs e)
    {
        try
        {
            TabContainer1.SelectedIndex = 6;
            TabContainer1_ActiveTabChanged(sender, e);
        }
        catch (Exception ex)
        {

        }
    }

    //krishhna kumar.r
    protected void btnadmesion_7_Click(object sender, EventArgs e)
    {
        TabContainer1.SelectedIndex = 7;
        TabContainer1_ActiveTabChanged(sender, e);

    }

    protected void Inventory_Click(object sender, EventArgs e)
    {
        TabContainer1.SelectedIndex = 8;
        TabContainer1_ActiveTabChanged(sender, e);
    }

    protected void Library_Click(object sender, EventArgs e)
    {
        TabContainer1.SelectedIndex = 9;
        TabContainer1_ActiveTabChanged(sender, e);
        loadLibraryHeader();
        loadLibraryLedger();
        librights();
        //kowshi
    }
    //Deepali 16.7.18============================
    protected void btnMblApp_Click(object sender, EventArgs e)
    {
        TabContainer1.SelectedIndex = 10;
        TabContainer1_ActiveTabChanged(sender, e);
    }
    //Deepali 16.7.18============================
    protected void btnsave_Attendance_Click(object sender, EventArgs e)
    {
        try
        {
            string usercode = ddluser.SelectedValue;
            int value = 0;
            if (chkRedo.Checked)
                value = 1;
            else
                value = 0;
            string sql = "if exists(select * from Master_Settings where settings = 'Include Redo student in Attendance') update Master_Settings set value='" + value + "' where settings = 'Include Redo student in Attendance' else insert into Master_Settings (usercode,settings,value) values ('" + usercode + "','Include Redo student in Attendance','" + value + "')";
            int status = dacc.update_method_wo_parameter(sql, "text");

            //=====================Added by saranya on 09/04/2018=====================//
            value = 0;
            string lockdays = txtOdlock.Text.ToString();

            if (lockdays.Trim() != "")
            {
                if (Convert.ToInt32(lockdays) > 0)
                {
                    value = Convert.ToInt32(lockdays);
                }
            }
            if (rdb_ind.Checked == true)
            {
                sql = "if not exists ( select * from Master_Settings where usercode='" + usercode + "' and settings='OD Lock Days') insert into Master_Settings (usercode,settings,value) values ('" + usercode + "','OD Lock Days','" + value + "') else update Master_Settings set value ='" + value + "' where usercode='" + usercode + "' and settings='OD Lock Days'";
            }
            if (rdb_grp.Checked == true)
            {
                sql = "if not exists ( select * from Master_Settings where group_code='" + usercode + "' and settings='OD Lock Days') insert into Master_Settings (group_code,settings,value) values ('" + usercode + "','OD Lock Days','" + value + "') else update Master_Settings set value ='" + value + "' where group_code='" + usercode + "' and settings='OD Lock Days'";
            }
            status = dacc.update_method_wo_parameter(sql, "Text");

            //***Added by Mullai

            if (chkotp.Checked == true)
            {
                value = 1;
            }
            else
            {
                value = 0;
            }

            sql = "if exists (select * from New_InsSettings where LinkName='student login otp' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + value + "' where LinkName='student login otp' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('student login otp','" + value + "','" + usercode + "','" + ddlcollege.SelectedItem.Value + "')";
            status = dacc.update_method_wo_parameter(sql, "Text");
            //***


            if (cbatndbatyr.Checked == true)
            {
                value = 1;
            }
            else
            {
                value = 0;
            }
            string batvalue = string.Empty;

            for (int j1 = 0; j1 < cblbatyr.Items.Count; j1++)
            {
                if (cblbatyr.Items[j1].Selected == true)
                {
                    if (string.IsNullOrEmpty(batvalue))
                        batvalue = "'" + cblbatyr.Items[j1].Text;
                    else
                        batvalue = batvalue + "," + cblbatyr.Items[j1].Text;
                }
            }


            sql = "if not exists ( select * from Master_Settings where usercode='" + usercode + "' and settings='Attendance lock with batch year') insert into Master_Settings (usercode,settings,value,template) values ('" + usercode + "','Attendance lock with batch year','" + value + "'," + batvalue + "') else update Master_Settings set value ='" + value + "' , template = " + batvalue + "' where usercode='" + usercode + "' and settings='Attendance lock with batch year'";
            int status1 = dacc.update_method_wo_parameter(sql, "text");


            //=======================================================================//

            imgAlert.Visible = true;
            lbl_alert.Text = "Saved successfully";

        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");

        }
    }

    protected void btnsave_coe_Click(object sender, EventArgs e)
    {
        try
        {
            string usercode = ddluser.SelectedValue;
            int value = 0;
            string selected_userid = string.Empty;
            int selected_usercode;
            string userorgropcode = string.Empty;
            string user_gropcode = string.Empty;
            string usergroup = string.Empty;
            string strusergruop = string.Empty;
            string struser_gruop = string.Empty;
            string user_code = string.Empty;
            if (ChkDispMarks.Checked)
                value = 1;
            else
                value = 0;
            string sql = "if exists(select * from Master_Settings where settings = 'ExcludeUnpaidStudents') update Master_Settings set value='" + value + "' where settings = 'ExcludeUnpaidStudents' else insert into Master_Settings (usercode,settings,value) values ('" + usercode + "','ExcludeUnpaidStudents','" + value + "')";
            int status = dacc.update_method_wo_parameter(sql, "text");

            string str = txtResultNote.Text;
            if (!string.IsNullOrEmpty(str))
            {
                string strresultNote = "if exists(select * from Master_Settings where settings = 'resultText') update Master_Settings set template='" + str + "' where settings = 'resultText' else insert into Master_Settings (usercode,settings,template) values ('" + usercode + "','resultText','" + str + "')";
                int result = dacc.update_method_wo_parameter(strresultNote, "text");
            }


            if (!string.IsNullOrEmpty(DropDownList1.SelectedValue) && !string.IsNullOrEmpty(DropDownList2.SelectedValue) && !string.IsNullOrEmpty(DropDownList3.SelectedValue) && !string.IsNullOrEmpty(DropDownList4.SelectedValue) && Convert.ToString(DropDownList3.SelectedValue) != "0" && Convert.ToString(DropDownList4.SelectedValue) != "0")
            {
                str = string.Empty;
                str = Convert.ToString(DropDownList1.SelectedValue) + "-" + Convert.ToString(DropDownList2.SelectedValue);

                string str1 = Convert.ToString(DropDownList3.SelectedValue) + "-" + Convert.ToString(DropDownList4.SelectedValue);
                string val = "0";
                if (CheckBox1.Checked)
                    val = "1";
                else
                    val = "0";

                string resultHold = "if exists(select * from Master_Settings where settings = 'result hold-" + str + "') update Master_Settings set template='" + str1 + "',value='" + val + "' where settings = 'result hold-" + str + "' else insert into Master_Settings (settings,template,value) values ('result hold-" + str + "','" + str1 + "','" + val + "')";
                int result = dacc.update_method_wo_parameter(resultHold, "text");
            }
            string batc = string.Empty;
            if (CheckBoxList1.Items.Count > 0)
                batc = rs1.getCblSelectedValue(CheckBoxList1);

            if (!string.IsNullOrEmpty(DropDownList5.SelectedValue) && !string.IsNullOrEmpty(DropDownList6.SelectedValue) && !string.IsNullOrEmpty(DropDownList7.SelectedValue) && !string.IsNullOrEmpty(DropDownList8.SelectedValue) && !string.IsNullOrEmpty(batc) && Convert.ToString(DropDownList8.SelectedValue) != "0" && Convert.ToString(DropDownList7.SelectedValue) != "0")
            {
                string va = string.Empty;
                if (RadioButton1.Checked)
                    va = "1";
                else if (RadioButton2.Checked)
                    va = "0";
                str = string.Empty;
                str = Convert.ToString(DropDownList5.SelectedValue) + "-" + Convert.ToString(DropDownList6.SelectedValue);
                for (int ji = 0; ji < CheckBoxList1.Items.Count; ji++)
                {
                    if (CheckBoxList1.Items[ji].Selected)
                    {
                        string b = Convert.ToString(CheckBoxList1.Items[ji]);

                        string resultHold = "if exists(select * from Master_Settings where settings = 'application hold-" + str + "' and template='" + b + "') update Master_Settings set value='" + va + "' where settings = 'application hold-" + str + "' and template='" + b + "' else insert into Master_Settings (settings,template,value) values ('application hold-" + str + "','" + b + "','" + va + "')";
                        int result = dacc.update_method_wo_parameter(resultHold, "text");
                    }
                }
            }

            //added by Mullai
            if (chkprintlock.Checked == true)
            {
                value = 1;
            }
            else
            {
                value = 0;
            }

            string sqlll = "if exists (select * from New_InsSettings where LinkName='MarkSheet Printlock' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + value + "' where LinkName='MarkSheet Printlock' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('MarkSheet Printlock','" + value + "','" + usercode + "','" + ddlcollege.SelectedItem.Value + "')";
            int status3 = dacc.update_method_wo_parameter(sqlll, "Text");
            //***



            for (int i = 0; i < ddluser.Items.Count; i++)
            {
                if (ddluser.Items[i].Selected == true)
                {
                    selected_userid = Convert.ToString(ddluser.Items[i].Text);
                    selected_usercode = Convert.ToInt16(ddluser.Items[i].Value);
                    //bindMulTerm(Convert.ToString(selected_usercode));
                    if (rdb_ind.Checked == true)
                    {
                        userorgropcode = " and usercode='" + selected_usercode.ToString() + "'";
                        user_gropcode = " and user_code='" + selected_usercode.ToString() + "'";
                        usergroup = "  usercode='" + selected_usercode.ToString() + "'";
                        strusergruop = "usercode";
                        struser_gruop = "user_code";
                    }
                    else if (rdb_grp.Checked == true)
                    {
                        userorgropcode = " and group_code='" + selected_usercode.ToString() + "'";
                        user_gropcode = " and group_code='" + selected_usercode.ToString() + "'";
                        usergroup = "  group_code='" + selected_usercode.ToString() + "'";
                        strusergruop = "group_code";
                        struser_gruop = "group_code";
                    }
                    user_code = selected_usercode.ToString();

                    if (rdbval1.Checked == true)
                    {
                        string savecopy = "if exists(select * from Master_Settings where settings='Valuation Settings' " + userorgropcode + ") update Master_Settings set value='1' where settings='Valuation Settings' " + userorgropcode + " else insert into Master_Settings (" + strusergruop + ",settings,value) values(" + selected_usercode.ToString() + ",'Valuation Settings','1')";
                        int copy = dacc.update_method_wo_parameter(savecopy, "text");
                    }
                    else if (rdbval2.Checked == true)
                    {
                        string savecopy = "if exists(select * from Master_Settings where settings='Valuation Settings' " + userorgropcode + " )update Master_Settings set value='2' where settings='Valuation Settings' " + userorgropcode + " else insert into Master_Settings (" + strusergruop + ",settings,value) values(" + selected_usercode.ToString() + ",'Valuation Settings','2')";
                        int copy = dacc.update_method_wo_parameter(savecopy, "text");
                    }
                    else if (rbdAll.Checked == true)
                    {
                        string savecopy = "if exists(select * from Master_Settings where settings='Valuation Settings' " + userorgropcode + " )update Master_Settings set value='3' where settings='Valuation Settings' " + userorgropcode + " else insert into Master_Settings (" + strusergruop + ",settings,value) values(" + selected_usercode.ToString() + ",'Valuation Settings','3')";
                        int copy = dacc.update_method_wo_parameter(savecopy, "text");
                    }
                    if (chkfailGpa.Checked)
                        value = 1;
                    else
                        value = 0;
                    string sqlQry = "if exists(select * from Master_Settings where settings = 'include gpa for fail student' " + userorgropcode + ") update Master_Settings set value='" + value + "' where settings = 'include gpa for fail student' " + userorgropcode + "  else insert into Master_Settings (" + strusergruop + ",settings,value) values (" + selected_usercode.ToString() + ",'include gpa for fail student','" + value + "')";
                    int status1 = dacc.update_method_wo_parameter(sqlQry, "text");
                }
            }


            imgAlert.Visible = true;
            lbl_alert.Text = "Saved successfully";

        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");

        }
    }

    protected void btnsave_Finance_Click(object sender, EventArgs e)
    {
        try
        {

            string ledgerid = string.Empty;
            double amount = 0;
            string headerid = string.Empty;
            string setting = string.Empty;
            string clgcode = string.Empty;
            string linkName = string.Empty;


            if (ddlledger.Items.Count > 0)
            {
                ledgerid = Convert.ToString(ddlledger.SelectedItem.Value);
                //headerid = d2.GetFunction("select headerfk from FM_LedgerMaster where LedgerPK='" + ledgerid + "' ");
            }
            if (ddlheader.Items.Count > 0)
            {
                headerid = Convert.ToString(ddlheader.SelectedItem.Value);
            }
            double.TryParse(Convert.ToString(txtamt.Text.Trim()), out amount);

            linkName = "Graduation Application Fees";
            clgcode = ddlcollegename.SelectedItem.Value;

            if (clgcode != "" && ledgerid != "" && headerid != "" && amount != 0)
            {
                string value = headerid + ";" + ledgerid + ";" + amount;

                // string insertQ = "insert into Master_Settings (settings,value) values('" + setting + "','" + value + "')";
                string insertQ = "if exists (select * from New_InsSettings where LinkName='" + linkName + "' and college_code ='" + clgcode + "' and LinkValue ='" + value + "') update New_InsSettings set LinkValue ='" + value + "' where LinkName='" + linkName + "'  and college_code ='" + clgcode + "' else insert into New_InsSettings(LinkName,LinkValue,college_code) values ('" + linkName + "','" + value + "','" + clgcode + "')";
                int update = d2.update_method_wo_parameter(insertQ, "Text");
                if (update > 0)
                {
                    //lbloutput.Text = "Saved Successfully";
                    //lbloutput.Visible = true;
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved successfully')", true);
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Saved successfully";
                }
                else
                {
                    //lbloutput.Text = "Not Saved";
                    //lbloutput.Visible = true;
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Not Saved')", true);
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Not Saved ";
                }
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Not Saved')", true);
                imgAlert.Visible = true;
                lbl_alert.Text = "Not Saved";
            }
            string refernumber = txtReference.Text;

            string Q1 = " if exists(select * from co_mastervalues where MasterCriteria='ReferenceNumber' and collegecode in(" + clgcode + ") and mastercriteria1='" + selected_usercode + "') update co_mastervalues set mastervalue='" + refernumber + "' where MasterCriteria='ReferenceNumber' and collegecode in(" + clgcode + ") and mastercriteria1='" + selected_usercode + "' and collegecode in(" + clgcode + ") else insert into co_mastervalues(mastervalue,MasterCriteria,mastercriteria1,collegecode) values ('" + refernumber + "','ReferenceNumber','" + selected_usercode + "'," + clgcode + ")";
            d2.update_method_wo_parameter(Q1, "text");
        }

        catch (Exception ex)
        {
        }

        #region print Setting
        int val = 0;
        if (chkprint.Checked == true)
        {
            val = 1;
        }
        else
        {
            val = 0;
        }

        string sqlll = "if exists (select * from New_InsSettings where LinkName='PrintBasedUser' and user_code ='" + ddluser.SelectedValue + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + val + "' where LinkName='PrintBasedUser' and user_code ='" + ddluser.SelectedValue + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('PrintBasedUser','" + val + "','" + ddluser.SelectedValue + "','" + ddlcollege.SelectedItem.Value + "')";
        int save2 = d2.update_method_wo_parameter(sqlll, "Text");
        if (save2 == 1)
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Saved successfully";
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Not Saved";
        }
        #endregion



        if (rb_WithFees.Checked == true)
        {
            int storevalue1 = 1;
            string savecopy = "if exists (select * from New_InsSettings where LinkName='graduationfees' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='graduationfees' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('graduationfees','" + storevalue1 + "','" + selected_usercode + "','" + ddlcollege.SelectedItem.Value + "')";
            int save1 = d2.update_method_wo_parameter(savecopy, "Text");
            if (save1 == 1)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Saved successfully";
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Not Saved";
            }

        }
        else if (rb_WithoutFees.Checked == true)
        {
            int storevalue1 = 0;
            string savecopy = "if exists (select * from New_InsSettings where LinkName='graduationfees' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='graduationfees' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('graduationfees','" + storevalue1 + "','" + selected_usercode + "','" + ddlcollege.SelectedItem.Value + "')";
            int save1 = d2.update_method_wo_parameter(savecopy, "Text");
            if (save1 == 1)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Saved successfully";
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Not Saved";
            }
        }
    }

    protected void btnsave_HR_Click(object sender, EventArgs e)
    {
        try
        {
            //SD_23/2019

            if (txtExtentPermission.Text != "")
            {
                string sql = "if exists (select * from New_InsSettings where LinkName='staff Permission' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + txtExtentPermission.Text + "' where LinkName='staff Permission'  and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,college_code) values ('staff Permission','" + txtExtentPermission.Text + "','" + ddlcollege.SelectedItem.Value + "')";
                int status = dacc.update_method_wo_parameter(sql, "Text");

                if (status > 0)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Saved successfully";
                }

            }
        }
        catch
        {

        }
    }

    protected void btnsave_Trans_Click(object sender, EventArgs e)
    {
    }
    //magesh 5.3.18
    protected void btnsave_Hostel_Click(object sender, EventArgs e)
    {
        try
        {
            string coll_code = Convert.ToString(ddlcollege.SelectedItem.Value);
            int savebreak = 0;
            int Health = 0;
            int Gym = 0;
            int Regular = 0;
            int Guest = 0;
            int Others = 0;
            int Staff = 0;
            int save1 = 0;
            int hsattn = 0;
            int messattn = 0;
            int hsmessattn = 0;
            #region breakage
            if (ddl_BreakageHeader.Items.Count > 0 && ddl_BreakageLedger.Items.Count > 0)
            {
                string head = Convert.ToString(ddl_BreakageHeader.SelectedItem.Value);
                string led = Convert.ToString(ddl_BreakageLedger.SelectedItem.Value);
                int valtype = 0;
                if (rdb_inmess.Checked == true)
                {
                    valtype = 1;
                }
                if (rdb_exmess.Checked == true)
                {
                    valtype = 0;
                }
                string saveBreakage = "if exists(select * from HM_Feessetting where Type='Breakage' and collegecode=" + coll_code + " )update HM_Feessetting set header=" + head + " , ledger=" + led + ",Text_value=" + valtype + " where Type='Breakage' and collegecode=" + coll_code + "   else insert into HM_Feessetting (collegecode,Type,header,ledger,Text_value) values(" + coll_code + ",'Breakage'," + head + "," + led + "," + valtype + ")";
                savebreak = dacc.update_method_wo_parameter(saveBreakage, "text");
            }
            #endregion
            #region Gym
            if (ddl_GymHeader.Items.Count > 0 && ddl_GymLedger.Items.Count > 0)
            {
                string head = Convert.ToString(ddl_GymHeader.SelectedItem.Value);
                string led = Convert.ToString(ddl_GymLedger.SelectedItem.Value);
                int valtype = 0;
                if (rdb_Gymin.Checked == true)
                {
                    valtype = 1;
                }
                if (rdb_Gymex.Checked == true)
                {
                    valtype = 0;
                }
                string saveGym = "if exists(select * from HM_Feessetting where Type='Gym' and collegecode=" + coll_code + " )update HM_Feessetting set header=" + head + " , ledger=" + led + ",Text_value=" + valtype + " where Type='Gym' and collegecode=" + coll_code + "   else insert into HM_Feessetting (collegecode,Type,header,ledger,Text_value) values(" + coll_code + ",'Gym'," + head + "," + led + "," + valtype + ")";
                Gym = dacc.update_method_wo_parameter(saveGym, "text");
            }
            #endregion
            #region Health
            if (ddl_HealthHeader.Items.Count > 0 && ddl_HealthLedger.Items.Count > 0)
            {
                string head = Convert.ToString(ddl_HealthHeader.SelectedItem.Value);
                string led = Convert.ToString(ddl_HealthLedger.SelectedItem.Value);
                int valtype = 0;
                if (rdb_healthin.Checked == true)
                {
                    valtype = 1;
                }
                if (rdb_healthex.Checked == true)
                {
                    valtype = 0;
                }
                string savehealth = "if exists(select * from HM_Feessetting where Type='Health' and collegecode=" + coll_code + " )update HM_Feessetting set header=" + head + " , ledger=" + led + ",Text_value=" + valtype + " where Type='Health' and collegecode=" + coll_code + "   else insert into HM_Feessetting (collegecode,Type,header,ledger,Text_value) values(" + coll_code + ",'Health'," + head + "," + led + "," + valtype + ")";
                Health = dacc.update_method_wo_parameter(savehealth, "text");
            }
            #endregion

            #region Regular
            if (ddl_RegularHeader.Items.Count > 0 && ddl_RegularLedger.Items.Count > 0)
            {
                string head = Convert.ToString(ddl_RegularHeader.SelectedItem.Value);
                string led = Convert.ToString(ddl_RegularLedger.SelectedItem.Value);
                string saveRegular = "if exists(select * from HM_Feessetting where Type='Regular' and collegecode=" + coll_code + " )update HM_Feessetting set header=" + head + " , ledger=" + led + ",Text_value='1' where Type='Regular' and collegecode=" + coll_code + "   else insert into HM_Feessetting (collegecode,Type,header,ledger,Text_value) values(" + coll_code + ",'Regular'," + head + "," + led + ",'1')";
                Regular = dacc.update_method_wo_parameter(saveRegular, "text");
            }
            #endregion

            #region Guest
            if (ddl_GustHeader.Items.Count > 0 && ddl_GustLedger.Items.Count > 0)
            {
                string head = Convert.ToString(ddl_GustHeader.SelectedItem.Value);
                string led = Convert.ToString(ddl_GustLedger.SelectedItem.Value);
                string saveGuest = "if exists(select * from HM_Feessetting where Type='Guest' and collegecode=" + coll_code + " )update HM_Feessetting set header=" + head + " , ledger=" + led + ",Text_value='1' where Type='Guest' and collegecode=" + coll_code + "   else insert into HM_Feessetting (collegecode,Type,header,ledger,Text_value) values(" + coll_code + ",'Guest'," + head + "," + led + ",'1')";
                Guest = dacc.update_method_wo_parameter(saveGuest, "text");
            }
            #endregion

            #region Staff
            if (ddl_StaffHeader.Items.Count > 0 && ddl_StaffLedger.Items.Count > 0)
            {
                string head = Convert.ToString(ddl_StaffHeader.SelectedItem.Value);
                string led = Convert.ToString(ddl_StaffLedger.SelectedItem.Value);
                string saveStaff = "if exists(select * from HM_Feessetting where Type='Staff' and collegecode=" + coll_code + " )update HM_Feessetting set header=" + head + " , ledger=" + led + ",Text_value='1' where Type='Staff' and collegecode=" + coll_code + "   else insert into HM_Feessetting (collegecode,Type,header,ledger,Text_value) values(" + coll_code + ",'Staff'," + head + "," + led + ",'1')";
                Staff = dacc.update_method_wo_parameter(saveStaff, "text");
            }
            #endregion

            #region Others
            if (ddl_OthersHeader.Items.Count > 0 && ddl_OthersLedger.Items.Count > 0)
            {
                string head = Convert.ToString(ddl_OthersHeader.SelectedItem.Value);
                string led = Convert.ToString(ddl_OthersLedger.SelectedItem.Value);
                string saveOthers = "if exists(select * from HM_Feessetting where Type='Others' and collegecode=" + coll_code + " )update HM_Feessetting set header=" + head + " , ledger=" + led + ",Text_value='1' where Type='Others' and collegecode=" + coll_code + "   else insert into HM_Feessetting (collegecode,Type,header,ledger,Text_value) values(" + coll_code + ",'Others'," + head + "," + led + ",'1')";
                Others = dacc.update_method_wo_parameter(saveOthers, "text");
            }

            if (Others > 0 || savebreak > 0 || Health > 0 || Gym > 0 || Regular > 0 || Guest > 0 || Staff > 0)
            {
                imagalt.Visible = true;
                lbl_aler.Text = "Saved successfully";
            }
            //else
            //{
            //    imagalt.Visible = true;
            //    lbl_aler.Text = "Not Saved!";
            //}
            #endregion

            #region attenrights
            for (int i = 0; i < ddluser.Items.Count; i++)
            {
                if (ddluser.Items[i].Selected == true)
                {
                    selected_usercode = Convert.ToInt16(ddluser.Items[i].Value);
                    if (rdb_ind.Checked == true)
                    {
                        //if (usergroup == "")
                        usergroup = selected_usercode.ToString();
                        // else
                        //usergroup = usergroup + ',' + selected_usercode.ToString();
                    }
                    else if (rdb_grp.Checked == true)
                    {
                        usergroup = selected_usercode.ToString();
                    }


                    if (rdb_ind.Checked == true)
                    {
                        if (rdb_hostelattn.Checked == true)
                        {
                            hsattn = 1;
                            string insqry1 = "if exists (select * from New_InsSettings where LinkName='Hostel Attendance' and  user_code ='" + usergroup + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + hsattn + "' where LinkName='Hostel Attendance'   and user_code ='" + usergroup + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Hostel Attendance','" + hsattn + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");
                        }
                        if (rdb_messattn.Checked == true)
                        {
                            messattn = 2;
                            string insqry1 = "if exists (select * from New_InsSettings where LinkName='Hostel Attendance' and user_code in('" + usergroup + "') and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + messattn + "' where LinkName='Hostel Attendance' and user_code in('" + usergroup + "') and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Hostel Attendance','" + messattn + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");
                        }
                        if (rdb_bothattn.Checked == true)
                        {
                            hsmessattn = 3;
                            string insqry1 = "if exists (select * from New_InsSettings where LinkName='Hostel Attendance' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + hsmessattn + "' where LinkName='Hostel Attendance' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Hostel Attendance','" + hsmessattn + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");
                        }
                    }
                    if (rdb_grp.Checked == true)
                    {
                        if (rdb_hostelattn.Checked == true)
                        {
                            hsattn = 1;
                            string insqry1 = "if exists (select * from New_InsSettings where LinkName='Hostel Attendance' and  group_code ='" + usergroup + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + hsattn + "' where LinkName='Hostel Attendance'   and group_code ='" + usergroup + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,group_code,college_code) values ('Hostel Attendance','" + hsattn + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");
                        }
                        if (rdb_messattn.Checked == true)
                        {
                            messattn = 2;
                            string insqry1 = "if exists (select * from New_InsSettings where LinkName='Hostel Attendance' and group_code in('" + usergroup + "') and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + messattn + "' where LinkName='Hostel Attendance' and group_code in('" + usergroup + "') and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,group_code,college_code) values ('Hostel Attendance','" + messattn + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");
                        }
                        if (rdb_bothattn.Checked == true)
                        {
                            hsmessattn = 3;
                            string insqry1 = "if exists (select * from New_InsSettings where LinkName='Hostel Attendance' and group_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + hsmessattn + "' where LinkName='Hostel Attendance' and group_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,group_code,college_code) values ('Hostel Attendance','" + hsmessattn + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");
                        }
                    }
                }
            }
            #endregion

            # region gatepass
            if (ddl_Hostel.Items.Count > 0)
            {
                int count = 0;
                string hoste = Convert.ToString(ddl_Hostel.SelectedValue);
                // string led = Convert.ToString(ddl_BreakageLedger.SelectedItem.Value);
                if (txt_count.Text != "")
                    int.TryParse(txt_count.Text, out count);
                else
                    count = 0;
                string cun = "update HM_HostelMaster set HostelGatePassPerCount='" + count + "' where HostelMasterPK='" + Convert.ToString(ddl_Hostel.SelectedValue) + "'";
                int up = d2.update_method_wo_parameter(cun, "Text");
            }
            if (ddlcollegeco.Items.Count > 0)
            {
                int count = 0;
                string hoste = Convert.ToString(ddlcollegeco.SelectedValue);
                // string led = Convert.ToString(ddl_BreakageLedger.SelectedItem.Value);
                if (Txtcol_count.Text != "")
                    int.TryParse(Txtcol_count.Text, out count);
                else
                    count = 0;
                string cun = "if exists(select * from gatepasscount where college_code='" + Convert.ToString(ddlcollegeco.SelectedValue) + "') update gatepasscount set  leavecount='" + count + "' where college_code='" + Convert.ToString(ddlcollegeco.SelectedValue) + "' else insert into gatepasscount (leavecount,college_code) values('" + count + "','" + Convert.ToString(ddlcollegeco.SelectedValue) + "')";
                int up = d2.update_method_wo_parameter(cun, "Text");
            }
            int bio = 0;
            if (rdbboi.Checked == true)
            {
                bio = 0;

                string cun1 = "if exists (select * from New_InsSettings where LinkName='gatepass biobased' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + bio + "' where LinkName='gatepass biobased' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('gatepass biobased','" + bio + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                save1 = d2.update_method_wo_parameter(cun1, "Text");
            }
            if (rdbnonboi.Checked == true)
            {
                bio = 1;
                string cun1 = "if exists (select * from New_InsSettings where LinkName='gatepass biobased' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + bio + "' where LinkName='gatepass biobased' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('gatepass biobased','" + bio + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                save1 = d2.update_method_wo_parameter(cun1, "Text");
            }

            #endregion

            #region id generation
            if (rdbin.Checked == true)
            {
                int id = 0;
                string insqry1 = "if exists (select * from New_InsSettings where LinkName='hostelid generation' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + id + "' where LinkName='hostelid generation' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('hostelid generation','" + id + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                save1 = d2.update_method_wo_parameter(insqry1, "Text");
            }
            if (rdbhos.Checked == true)
            {
                int id = 1;
                string insqry1 = "if exists (select * from New_InsSettings where LinkName='hostelid generation' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + id + "' where LinkName='hostelid generation' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('hostelid generation','" + id + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                save1 = d2.update_method_wo_parameter(insqry1, "Text");
            }
            #endregion

            #region hostel rights
            string messmasterfk = string.Empty;
            string Build = string.Empty;
            string Floor = string.Empty;
            string Room = string.Empty;
            for (int k = 0; k < cbl_hos.Items.Count; k++)
            {
                if (cbl_hos.Items[k].Selected == true)
                {
                    if (messmasterfk == "")
                    {
                        messmasterfk = "" + cbl_hos.Items[k].Value.ToString() + "";
                    }
                    else
                    {
                        messmasterfk = messmasterfk + "," + cbl_hos.Items[k].Value.ToString() + "";
                    }
                }
            }
            for (int k = 0; k < cbl_buildname.Items.Count; k++)
            {
                if (cbl_buildname.Items[k].Selected == true)
                {
                    if (Build == "")
                    {
                        Build = "" + cbl_buildname.Items[k].Value.ToString() + "";
                    }
                    else
                    {
                        Build = Build + "," + cbl_buildname.Items[k].Value.ToString() + "";
                    }
                }
            }

            for (int k = 0; k < cbl_floorname.Items.Count; k++)
            {
                if (cbl_floorname.Items[k].Selected == true)
                {
                    if (Floor == "")
                    {
                        Floor = "" + cbl_floorname.Items[k].Value.ToString() + "";
                    }
                    else
                    {
                        Floor = Floor + "," + cbl_floorname.Items[k].Value.ToString() + "";
                    }
                }
            }

            for (int k = 0; k < cbl_roomname.Items.Count; k++)
            {
                if (cbl_roomname.Items[k].Selected == true)
                {
                    if (Room == "")
                    {
                        Room = "" + cbl_roomname.Items[k].Value.ToString() + "";
                    }
                    else
                    {
                        Room = Room + "," + cbl_roomname.Items[k].Value.ToString() + "";
                    }
                }
            }



            if (messmasterfk != "" && txt_messname.Text != "--Select--")
            {
                int id = 0;
                string insqry1 = "if exists (select * from New_InsSettings where LinkName='Hostel Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + messmasterfk + "' where LinkName='Hostel Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Hostel Rights','" + messmasterfk + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                save1 = d2.update_method_wo_parameter(insqry1, "Text");
                txt_messname.Text = "--Select--";
            }
            if (Build != "" && txt_buildingname.Text != "--Select--")
            {
                int id1 = 0;
                string insqry1 = "if exists (select * from New_InsSettings where LinkName='Building Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + Build + "' where LinkName='Building Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Building Rights','" + Build + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                save1 = d2.update_method_wo_parameter(insqry1, "Text");
                txt_buildingname.Text = "--Select--";
            }

            if (Floor != "" && txt_floorname.Text != "--Select--")
            {
                // int id1 = 0;
                string insqry1 = "if exists (select * from New_InsSettings where LinkName='Floor Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + Floor + "' where LinkName='Floor Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Floor Rights','" + Floor + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                save1 = d2.update_method_wo_parameter(insqry1, "Text");
                txt_floorname.Text = "--Select--";
            }
            if (Room != "" && txt_roomname.Text != "--Select--")
            {
                // int id1 = 0;
                string insqry1 = "if exists (select * from New_InsSettings where LinkName='Room Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + Room + "' where LinkName='Room Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Room Rights','" + Room + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                save1 = d2.update_method_wo_parameter(insqry1, "Text");
                txt_roomname.Text = "--Select--";
            }

            #endregion

            //Added By Saranyadevi4.3.2019
            #region Hostel Vacate
            string Vacate = "";
            if (RadioButtonHostel.SelectedIndex == 0)
                Vacate = "0";
            if (RadioButtonHostel.SelectedIndex == 1)
                Vacate = "1";
            if (coll_code != "" && selected_usercode.ToString() != "")
            {
                string VacateRights = "if exists (select * from New_InsSettings where LinkName='Hostel Vacate Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + Vacate + "' where LinkName='Hostel Vacate Rights' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Hostel Vacate Rights','" + Vacate + "','" + usergroup + "','" + ddlcollege.SelectedItem.Value + "')";
                save1 = d2.update_method_wo_parameter(VacateRights, "Text");
            }
            #endregion
            imagalt.Visible = true;
            lbl_aler.Text = "Saved successfully";
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }

    }

    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        try
        {
            imgAlert.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_alertclose_Click1(object sender, EventArgs e)
    {
        try
        {
            imagalt.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    public void binddegree()
    {
        try
        {
            ds.Clear();
            txtDegree.Text = "---Select---";
            string batchCode = string.Empty;
            chkDegree.Checked = false;
            cblDegree.Items.Clear();
            //userCode = Session["usercode"].ToString();
            //singleUser = Session["single_user"].ToString();
            //groupUserCode = Session["group_code"].ToString();
            string collegeCode = string.Empty;
            if (ddlcollege.Items.Count > 0)
                collegeCode = ddlcollege.SelectedValue.ToString().Trim();


            string valBatch = Convert.ToString(ddlbatch.SelectedValue);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "')  ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = d2.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblDegree.DataSource = ds;
                cblDegree.DataTextField = "course_name";
                cblDegree.DataValueField = "course_id";
                cblDegree.DataBind();
                checkBoxListselectOrDeselect(cblDegree, true);
                CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
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
            string degreecode = string.Empty;
            //collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            txtBranch.Text = "---Select---";
            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            ds.Clear();

            string collegeCode = string.Empty;
            if (ddlcollege.Items.Count > 0)
                collegeCode = ddlcollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;

            string valBatch = Convert.ToString(ddlbatch.SelectedValue);// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);

            if (cblDegree.Items.Count > 0)
                valDegree = rs1.GetSelectedItemsValueAsString(cblDegree);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "')  ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = d2.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblBranch.DataSource = ds;
                cblBranch.DataTextField = "dept_name";
                cblBranch.DataValueField = "degree_code";
                cblBranch.DataBind();
                checkBoxListselectOrDeselect(cblBranch, true);
                CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            }

        }
        catch (Exception ex)
        {

        }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            cblbatyr.Items.Clear();
            txtbatyr.Text = "---Select---";
            ds = d2.BindBatch();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();

                cblbatyr.DataSource = ds;
                cblbatyr.DataTextField = "batch_year";
                cblbatyr.DataValueField = "batch_year";
                cblbatyr.DataBind();
            }
            if (cblbatyr.Items.Count > 0)
            {
                for (int i = 0; i < cblbatyr.Items.Count; i++)
                {
                    cblbatyr.Items[i].Selected = true;
                }
                txtbatyr.Text = "Batch(" + cblbatyr.Items.Count + ")";
                cbbatyr.Checked = true;
            }
        }
        catch (Exception ex)
        {
            //lblerror.Text = ex.ToString();
            //lblerror.Visible = true;
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    public void bindsemester()
    {
        try
        {
            string branch = string.Empty;
            if (cblBranch.Items.Count > 0)
                branch = rs1.getCblSelectedValue(cblBranch);
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            string strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + ddlcollege.SelectedValue.ToString() + " order by duration desc";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }

        }
        catch (Exception ex)
        {
            string collegecode1 = Session["collegecode"].ToString();
            d2.sendErrorMail(ex, collegecode1, "LetterInwardReport");
        }
    }

    public void bindCollegeACR()
    {
        try
        {

            string SelectQ = "select college_code,Coll_acronymn from collinfo";
            DataTable dtacr = dir.selectDataTable(SelectQ);
            if (dtacr.Rows.Count > 0)
            {
                ddlCollegeAcr.DataSource = dtacr;
                ddlCollegeAcr.DataTextField = "Coll_acronymn";
                ddlCollegeAcr.DataValueField = "college_code";
                ddlCollegeAcr.DataBind();

            }
        }
        catch
        {

        }
    }

    public void bindEduLevel()
    {
        try
        {
            string colCode = Convert.ToString(ddlCollegeAcr.SelectedValue);
            string SelectQ = "select distinct Edu_Level from course where college_code='" + colCode + "'";
            DataTable dtacr = dir.selectDataTable(SelectQ);
            if (dtacr.Rows.Count > 0)
            {
                ddlEduLevel.DataSource = dtacr;
                ddlEduLevel.DataTextField = "Edu_Level";
                ddlEduLevel.DataValueField = "Edu_Level";
                ddlEduLevel.DataBind();
            }
        }
        catch
        {

        }
    }

    public void bindBatchYear()
    {
        string SelectQ = "select distinct r.Batch_Year from Registration r, course c,Degree d where r.college_code=c.college_code  and r.degree_code=d.Degree_Code and c.Course_Id=d.Course_Id and c.Edu_Level='" + Convert.ToString(ddlEduLevel.SelectedValue) + "' and CC=0 and  r.college_code='" + Convert.ToString(ddlCollegeAcr.SelectedValue) + "'";
        DataTable dtbatch = dir.selectDataTable(SelectQ);
        if (dtbatch.Rows.Count > 0)
        {
            ddlBatchYear.DataSource = dtbatch;
            ddlBatchYear.DataTextField = "Batch_Year";
            ddlBatchYear.DataValueField = "Batch_Year";
            ddlBatchYear.DataBind();
        }
    }

    public void bindCollegeACR1()
    {
        try
        {

            string SelectQ = "select college_code,Coll_acronymn from collinfo";
            DataTable dtacr = dir.selectDataTable(SelectQ);
            if (dtacr.Rows.Count > 0)
            {
                DropDownList1.DataSource = dtacr;
                DropDownList1.DataTextField = "Coll_acronymn";
                DropDownList1.DataValueField = "college_code";
                DropDownList1.DataBind();

            }
        }
        catch
        {

        }
    }

    public void bindEduLevel1()
    {
        try
        {
            string colCode = Convert.ToString(DropDownList1.SelectedValue);
            string SelectQ = "select distinct Edu_Level from course where college_code='" + colCode + "'";
            DataTable dtacr = dir.selectDataTable(SelectQ);
            if (dtacr.Rows.Count > 0)
            {
                DropDownList2.DataSource = dtacr;
                DropDownList2.DataTextField = "Edu_Level";
                DropDownList2.DataValueField = "Edu_Level";
                DropDownList2.DataBind();
            }
        }
        catch
        {

        }
    }

    public void examMntyr()
    {
        try
        {
            DropDownList4.Items.Clear();
            DropDownList4.Items.Insert(0, new ListItem("  ", "0"));
            DropDownList4.Items.Insert(1, new ListItem("Jan", "1"));
            DropDownList4.Items.Insert(2, new ListItem("Feb", "2"));
            DropDownList4.Items.Insert(3, new ListItem("Mar", "3"));
            DropDownList4.Items.Insert(4, new ListItem("Apr", "4"));
            DropDownList4.Items.Insert(5, new ListItem("May", "5"));
            DropDownList4.Items.Insert(6, new ListItem("Jun", "6"));
            DropDownList4.Items.Insert(7, new ListItem("Jul", "7"));
            DropDownList4.Items.Insert(8, new ListItem("Aug", "8"));
            DropDownList4.Items.Insert(9, new ListItem("Sep", "9"));
            DropDownList4.Items.Insert(10, new ListItem("Oct", "10"));
            DropDownList4.Items.Insert(11, new ListItem("Nov", "11"));
            DropDownList4.Items.Insert(12, new ListItem("Dec", "12"));
            int year = 0;
            year = Convert.ToInt16(DateTime.Today.Year);
            DropDownList3.Items.Clear();
            for (int l = 0; l <= 5; l++)
            {
                DropDownList3.Items.Add(Convert.ToString(year - l));
            }
            DropDownList3.Items.Insert(0, new ListItem("  ", "0"));
        }
        catch
        {

        }
    }

    public void bindCollegeACR2()
    {
        try
        {

            string SelectQ = "select college_code,Coll_acronymn from collinfo";
            DataTable dtacr = dir.selectDataTable(SelectQ);
            if (dtacr.Rows.Count > 0)
            {
                DropDownList5.DataSource = dtacr;
                DropDownList5.DataTextField = "Coll_acronymn";
                DropDownList5.DataValueField = "college_code";
                DropDownList5.DataBind();

            }
        }
        catch
        {

        }
    }

    public void bindEduLevel2()
    {
        try
        {
            string colCode = Convert.ToString(DropDownList1.SelectedValue);
            string SelectQ = "select distinct Edu_Level from course where college_code='" + colCode + "'";
            DataTable dtacr = dir.selectDataTable(SelectQ);
            if (dtacr.Rows.Count > 0)
            {
                DropDownList6.DataSource = dtacr;
                DropDownList6.DataTextField = "Edu_Level";
                DropDownList6.DataValueField = "Edu_Level";
                DropDownList6.DataBind();
            }
        }
        catch
        {

        }
    }

    public void examMntyr2()
    {
        try
        {
            DropDownList8.Items.Clear();
            DropDownList8.Items.Insert(0, new ListItem("  ", "0"));
            DropDownList8.Items.Insert(1, new ListItem("Jan", "1"));
            DropDownList8.Items.Insert(2, new ListItem("Feb", "2"));
            DropDownList8.Items.Insert(3, new ListItem("Mar", "3"));
            DropDownList8.Items.Insert(4, new ListItem("Apr", "4"));
            DropDownList8.Items.Insert(5, new ListItem("May", "5"));
            DropDownList8.Items.Insert(6, new ListItem("Jun", "6"));
            DropDownList8.Items.Insert(7, new ListItem("Jul", "7"));
            DropDownList8.Items.Insert(8, new ListItem("Aug", "8"));
            DropDownList8.Items.Insert(9, new ListItem("Sep", "9"));
            DropDownList8.Items.Insert(10, new ListItem("Oct", "10"));
            DropDownList8.Items.Insert(11, new ListItem("Nov", "11"));
            DropDownList8.Items.Insert(12, new ListItem("Dec", "12"));
            int year = 0;
            year = Convert.ToInt16(DateTime.Today.Year);
            DropDownList7.Items.Clear();
            for (int l = 0; l <= 5; l++)
            {
                DropDownList7.Items.Add(Convert.ToString(year - l));
            }
            DropDownList7.Items.Insert(0, new ListItem("  ", "0"));
        }
        catch
        {

        }
    }

    public void bindBatchsetting()
    {
        try
        {
            string edu = Convert.ToString(DropDownList6.SelectedValue);
            CheckBoxList1.Items.Clear();
            string selectQ = "select distinct Batch_Year from Registration r,course c,Degree d where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and c.Edu_Level='" + edu.Trim() + "'  order by Batch_Year desc";//and CC<>1 and DelFlag<>1 
            DataTable dtbatch = dir.selectDataTable(selectQ);
            if (dtbatch.Rows.Count > 0)
            {

                CheckBoxList1.DataSource = dtbatch;
                CheckBoxList1.DataTextField = "Batch_Year";
                CheckBoxList1.DataValueField = "Batch_Year";
                CheckBoxList1.DataBind();
                checkBoxListselectOrDeselect(CheckBoxList1, true);
                CallCheckboxListChange(CheckBox2, CheckBoxList1, TextBox1, Label17.Text, "--Select--");
            }
        }
        catch
        {

        }

    }

    protected void DropDownList5_IndexChange(object sender, EventArgs e)
    {
        try
        {
            bindEduLevel2();
            examMntyr2();
        }
        catch
        {
        }
    }

    protected void DropDownList6_IndexChange(object sender, EventArgs e)
    {
        try
        {
            bindBatchsetting();
            examMntyr2();
        }
        catch
        {
        }
    }

    protected void DropDownList7_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }

    protected void DropDownList8_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }

    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            // CallCheckboxListChange(CheckBox2, CheckBoxList1, TextBox1, Label17.Text, "--Select--");

            CallCheckboxChange(CheckBox2, CheckBoxList1, TextBox1, Label17.Text, "--Select--");
            bindbranch();

        }
        catch (Exception ex)
        {
        }
    }

    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(CheckBox2, CheckBoxList1, TextBox1, Label17.Text, "--Select--");
            bindbranch();

        }
        catch (Exception ex)
        {
        }
    }

    protected void DropDownList1_IndexChange(object sender, EventArgs e)
    {
        try
        {
            bindEduLevel1();
            examMntyr();
        }
        catch
        {
        }
    }

    protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }

    protected void DropDownList4_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }

    protected void ddlCollegeAcr_IndexChange(object sender, EventArgs e)
    {
        bindEduLevel();
        bindBatchYear();

    }

    protected void ddlEduLevel_IndexChange(object sender, EventArgs e)
    {
        bindBatchYear();

    }

    protected void ddlBatchYear_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {

        binddegree();
        bindbranch();
        bindsemester();
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();

        }
        catch (Exception ex)
        {
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            bindbranch();

        }
        catch (Exception ex)
        {
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            bindsemester();
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            bindsemester();

        }
        catch (Exception ex)
        {
        }
    }

    protected void btnSaveSem_Click(object sender, EventArgs e)
    {
        try
        {
            string collCode = Convert.ToString(ddlcollege.SelectedValue);
            string batch = Convert.ToString(ddlbatch.SelectedValue);
            string sem = Convert.ToString(ddlsem.SelectedValue);
            int count = 0;
            string setting = "CBCSsem" + batch;
            if (!string.IsNullOrEmpty(collCode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(sem))
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    if (cblBranch.Items[i].Selected == true)
                    {
                        string degCode = Convert.ToString(cblBranch.Items[i].Value);
                        string insertQ = "if Exists (select * from master_settings where settings='" + setting + "' and value='" + degCode + "') Update master_settings set template='" + sem + "' where settings='" + setting + "' and value='" + degCode + "' else insert into master_settings(settings,value,template) values ('" + setting + "','" + degCode + "','" + sem + "')";
                        count = dir.updateData(insertQ);
                    }
                }
                if (count != 0)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Saved successfully";
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Not Saved";
            }
        }
        catch
        {
        }

    }

    protected void btnGo1_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            FpSpread1.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            string colCode = Convert.ToString(ddlCollegeAcr.SelectedValue);
            string edulevel = Convert.ToString(ddlEduLevel.SelectedValue);
            string batch = Convert.ToString(ddlBatchYear.SelectedValue);

            string SelectQ = "select EntryCode,LeaveCode,DispText from AttMasterSetting where CollegeCode='" + colCode + "'";
            DataTable dtval = dir.selectDataTable("select * from  leaveMaster where collegeCode='" + colCode + "' and batchyear='" + batch + "' and eduLevel='" + edulevel + "'");
            //and semester='" + sem + "'
            string maxSem = d2.GetFunction("select MAX(Duration) from Degree d,course c where d.Course_Id=c.Course_Id and c.Edu_Level='" + edulevel + "'");
            int semVal = 0;
            int.TryParse(maxSem, out semVal);
            DataTable dtAttSettings = dir.selectDataTable(SelectQ);
            int wid = 70;
            //FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Semester";
            FpSpread1.Sheets[0].Columns[0].Width = 70;
            if (dtAttSettings.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dt in dtAttSettings.Rows)
                {
                    string distxt = Convert.ToString(dt["DispText"]);
                    string EntryCode = Convert.ToString(dt["EntryCode"]);
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, i].Text = distxt;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, i].Note = EntryCode;
                    FpSpread1.Sheets[0].Columns[i].Width = 70;
                    i = i + 1;
                    wid = wid + 70;
                }
                for (int se = 1; se <= semVal; se++)
                {
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = se.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                }
                FpSpread1.SaveChanges();

                for (int j = 0; j < FpSpread1.Sheets[0].Rows.Count; j++)
                {
                    string sems = FpSpread1.Sheets[0].Cells[j, 0].Text;
                    for (int col = 1; col < FpSpread1.Sheets[0].ColumnCount; col++)
                    {
                        string entCode = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Note);
                        if (dtval.Rows.Count > 0)
                        {
                            dtval.DefaultView.RowFilter = "EntryCode='" + entCode + "' and semester='" + sems + "'";
                            DataTable dvVal = dtval.DefaultView.ToTable();
                            if (dvVal.Rows.Count > 0)
                            {
                                string val = Convert.ToString(dvVal.Rows[0]["Maxval"]);
                                FpSpread1.Sheets[0].Cells[j, col].Text = val;
                            }
                        }
                    }
                }
                FpSpread1.Visible = true;
                btnSaveNew.Visible = true;
                FpSpread1.Width = wid;
                FpSpread1.Height = 200;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();
            }
        }
        catch
        {

        }

    }

    protected void btnSaveNew_Click(object sender, EventArgs e)
    {
        try
        {
            string collegeCodeNew = Convert.ToString(ddlCollegeAcr.SelectedValue);
            string edulevel = Convert.ToString(ddlEduLevel.SelectedValue);
            string batch = Convert.ToString(ddlBatchYear.SelectedValue);
            //string sem = Convert.ToString(ddlS.SelectedValue);
            FpSpread1.SaveChanges();
            int count = 0;
            //if (FpSpread1.Sheets[0].Rows.Count > 0 && FpSpread1.Sheets[0].ColumnCount > 0)
            //{
            //    for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
            //    {
            //        string sem = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Text);
            //        string entryval = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, i].Note);
            //        string maxVal = Convert.ToString(FpSpread1.Sheets[0].Cells[i, i].Text);
            //        string insertQ = "if exists(select * from  leaveMaster where collegeCode='" + collegeCodeNew + "' and batchyear='" + batch + "' and eduLevel='" + edulevel + "' and semester='" + sem + "' and EntryCode='" + entryval + "') update leaveMaster SET Maxval='" + maxVal + "' where collegeCode='" + collegeCodeNew + "' and batchyear='" + batch + "' and eduLevel='" + edulevel + "' and semester='" + sem + "' and EntryCode='" + entryval + "' else insert into leaveMaster(collegeCode,batchyear,eduLevel,semester,EntryCode,Maxval) values('" + collegeCodeNew + "','" + batch + "','" + edulevel + "','" + sem + "','" + entryval + "','" + maxVal + "')";
            //        count = d2.update_method_wo_parameter(insertQ, "text");

            //    }
            //}
            if (FpSpread1.Sheets[0].Rows.Count > 0 && FpSpread1.Sheets[0].ColumnCount > 0)
            {
                for (int j = 0; j < FpSpread1.Sheets[0].Rows.Count; j++)
                {
                    string sems = FpSpread1.Sheets[0].Cells[j, 0].Text;
                    for (int col = 1; col < FpSpread1.Sheets[0].ColumnCount; col++)
                    {
                        string entCode = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Note);
                        string maxVal = Convert.ToString(FpSpread1.Sheets[0].Cells[j, col].Text);
                        if (!string.IsNullOrEmpty(maxVal))
                        {
                            string insertQ = "if exists(select * from  leaveMaster where collegeCode='" + collegeCodeNew + "' and batchyear='" + batch + "' and eduLevel='" + edulevel + "' and semester='" + sems + "' and EntryCode='" + entCode + "') update leaveMaster SET Maxval='" + maxVal + "' where collegeCode='" + collegeCodeNew + "' and batchyear='" + batch + "' and eduLevel='" + edulevel + "' and semester='" + sems + "' and EntryCode='" + entCode + "' else insert into leaveMaster(collegeCode,batchyear,eduLevel,semester,EntryCode,Maxval) values('" + collegeCodeNew + "','" + batch + "','" + edulevel + "','" + sems + "','" + entCode + "','" + maxVal + "')";
                            count = d2.update_method_wo_parameter(insertQ, "text");
                        }

                    }
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Values are Not Found!";
            }
            if (count != 0)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Saved successfully!";
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Not Saved!";
            }
        }
        catch
        {

        }
    }

    #region hostel
    #region header
    public void loadHostelHeader(int dropname)
    {
        try
        {
            string usercode = Convert.ToString(Session["usercode"]);
            string collegecodeNew = string.Empty;
            if (ddlcollege.Items.Count > 0)
            {
                collegecodeNew = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            switch (dropname)
            {
                case 1:
                    ddl_BreakageHeader.Items.Clear();
                    break;
                case 2:
                    ddl_GymHeader.Items.Clear();
                    break;
                case 3:
                    ddl_HealthHeader.Items.Clear();
                    break;
                case 4:
                    ddl_RegularHeader.Items.Clear();
                    break;
                case 5:
                    ddl_GustHeader.Items.Clear();
                    break;
                case 6:
                    ddl_StaffHeader.Items.Clear();
                    break;
                case 7:
                    ddl_OthersHeader.Items.Clear();
                    break;
            }
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H WHERE CollegeCode = " + collegecodeNew + "";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                switch (dropname)
                {
                    case 1:
                        ddl_BreakageHeader.DataSource = ds;
                        ddl_BreakageHeader.DataTextField = "HeaderName";
                        ddl_BreakageHeader.DataValueField = "HeaderPK";
                        ddl_BreakageHeader.DataBind();
                        break;
                    case 2:
                        ddl_GymHeader.DataSource = ds;
                        ddl_GymHeader.DataTextField = "HeaderName";
                        ddl_GymHeader.DataValueField = "HeaderPK";
                        ddl_GymHeader.DataBind();
                        break;
                    case 3:
                        ddl_HealthHeader.DataSource = ds;
                        ddl_HealthHeader.DataTextField = "HeaderName";
                        ddl_HealthHeader.DataValueField = "HeaderPK";
                        ddl_HealthHeader.DataBind();
                        loadHostelLedger(dropname);
                        break;
                    case 4:
                        ddl_RegularHeader.DataSource = ds;
                        ddl_RegularHeader.DataTextField = "HeaderName";
                        ddl_RegularHeader.DataValueField = "HeaderPK";
                        ddl_RegularHeader.DataBind();
                        break;
                    case 5:
                        ddl_GustHeader.DataSource = ds;
                        ddl_GustHeader.DataTextField = "HeaderName";
                        ddl_GustHeader.DataValueField = "HeaderPK";
                        ddl_GustHeader.DataBind();
                        break;
                    case 6:
                        ddl_StaffHeader.DataSource = ds;
                        ddl_StaffHeader.DataTextField = "HeaderName";
                        ddl_StaffHeader.DataValueField = "HeaderPK";
                        ddl_StaffHeader.DataBind();
                        break;
                    case 7:
                        ddl_OthersHeader.DataSource = ds;
                        ddl_OthersHeader.DataTextField = "HeaderName";
                        ddl_OthersHeader.DataValueField = "HeaderPK";
                        ddl_OthersHeader.DataBind();
                        break;


                }
                loadHostelLedger(dropname);
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }
    #endregion
    #region ledger
    public void loadHostelLedger(int dropname)
    {
        try
        {
            string usercode = Convert.ToString(Session["usercode"]);
            string collegecodeNew = string.Empty;
            int cun = 0;
            string drop_selected_value = string.Empty;
            if (ddlcollege.Items.Count > 0)
            {
                collegecodeNew = Convert.ToString(ddlcollege.SelectedItem.Value);
                switch (dropname)
                {
                    case 1:
                        ddl_BreakageLedger.Items.Clear();
                        cun = ddl_BreakageHeader.Items.Count;
                        drop_selected_value = Convert.ToString(ddl_BreakageHeader.SelectedItem.Value).Trim();
                        break;
                    case 2:
                        ddl_GymLedger.Items.Clear();
                        cun = ddl_GymHeader.Items.Count;
                        drop_selected_value = Convert.ToString(ddl_GymHeader.SelectedItem.Value).Trim();
                        break;
                    case 3:
                        ddl_HealthLedger.Items.Clear();
                        cun = ddl_HealthHeader.Items.Count;
                        drop_selected_value = Convert.ToString(ddl_HealthHeader.SelectedItem.Value).Trim();
                        break;
                    case 4:
                        ddl_RegularLedger.Items.Clear();
                        cun = ddl_RegularHeader.Items.Count;
                        drop_selected_value = Convert.ToString(ddl_RegularHeader.SelectedItem.Value).Trim();
                        break;
                    case 5:
                        ddl_GustLedger.Items.Clear();
                        cun = ddl_GustHeader.Items.Count;
                        drop_selected_value = Convert.ToString(ddl_GustHeader.SelectedItem.Value).Trim();
                        break;
                    case 6:
                        ddl_StaffLedger.Items.Clear();
                        cun = ddl_StaffHeader.Items.Count;
                        drop_selected_value = Convert.ToString(ddl_StaffHeader.SelectedItem.Value).Trim();
                        break;
                    case 7:
                        ddl_OthersLedger.Items.Clear();
                        cun = ddl_OthersHeader.Items.Count;
                        drop_selected_value = Convert.ToString(ddl_OthersHeader.SelectedItem.Value).Trim();
                        break;
                }
            }
            if (cun > 0)
            {
                string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L WHERE  l.LedgerMode=0   AND L.CollegeCode = " + collegecodeNew + " and L.HeaderFK in (" + drop_selected_value + ")";
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    switch (dropname)
                    {
                        case 1:
                            ddl_BreakageLedger.DataSource = ds;
                            ddl_BreakageLedger.DataTextField = "LedgerName";
                            ddl_BreakageLedger.DataValueField = "LedgerPK";
                            ddl_BreakageLedger.DataBind();
                            break;
                        case 2:
                            ddl_GymLedger.DataSource = ds;
                            ddl_GymLedger.DataTextField = "LedgerName";
                            ddl_GymLedger.DataValueField = "LedgerPK";
                            ddl_GymLedger.DataBind();

                            break;
                        case 3:
                            ddl_HealthLedger.DataSource = ds;
                            ddl_HealthLedger.DataTextField = "LedgerName";
                            ddl_HealthLedger.DataValueField = "LedgerPK";
                            ddl_HealthLedger.DataBind();
                            break;
                        case 4:
                            ddl_RegularLedger.DataSource = ds;
                            ddl_RegularLedger.DataTextField = "LedgerName";
                            ddl_RegularLedger.DataValueField = "LedgerPK";
                            ddl_RegularLedger.DataBind();
                            break;
                        case 5:
                            ddl_GustLedger.DataSource = ds;
                            ddl_GustLedger.DataTextField = "LedgerName";
                            ddl_GustLedger.DataValueField = "LedgerPK";
                            ddl_GustLedger.DataBind();
                            break;
                        case 6:
                            ddl_StaffLedger.DataSource = ds;
                            ddl_StaffLedger.DataTextField = "LedgerName";
                            ddl_StaffLedger.DataValueField = "LedgerPK";
                            ddl_StaffLedger.DataBind();
                            break;
                        case 7:
                            ddl_OthersLedger.DataSource = ds;
                            ddl_OthersLedger.DataTextField = "LedgerName";
                            ddl_OthersLedger.DataValueField = "LedgerPK";
                            ddl_OthersLedger.DataBind();
                            break;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }
    #endregion

    #region hostel
    public void bindhostel()
    {
        try
        {
            ds.Clear();

            string itemname = "select HostelMasterPK,HostelName  from HM_HostelMaster ";// where CollegeCode in ('" + ddl_college.SelectedItem.Value + "') order by HostelMasterPK ";
            ds = d2.select_method_wo_parameter(itemname, "Text");
            ddl_Hostel.Items.Clear();
            // ds = d2.BindHostel(ddl_college.SelectedItem.Value);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Hostel.DataSource = ds;
                ddl_Hostel.DataTextField = "HostelName";
                ddl_Hostel.DataValueField = "HostelMasterPK";
                ddl_Hostel.DataBind();

                cbl_hos.DataSource = ds;
                cbl_hos.DataTextField = "HostelName";
                cbl_hos.DataValueField = "HostelMasterPK";
                cbl_hos.DataBind();

            }

        }
        catch
        {

        }
    }
    public void bindcollege()
    {
        try
        {
            ds.Clear();
            //ddl_college.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ddl_college.DataSource = ds;
                //ddl_college.DataTextField = "collname";
                //ddl_college.DataValueField = "college_code";
                //ddl_college.DataBind();
                ddlcollegeco.DataSource = ds;
                ddlcollegeco.DataTextField = "collname";
                ddlcollegeco.DataValueField = "college_code";
                ddlcollegeco.DataBind();
            }
        }
        catch
        {
        }
    }
    #endregion

    #region build


    public void bindfloor()
    {
        try
        {
            string hostel = "";

            for (int i = 0; i < cbl_hos.Items.Count; i++)
            {
                if (cbl_hos.Items[i].Selected == true)
                {
                    if (hostel == "")
                    {
                        hostel = "" + cbl_hos.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostel = hostel + "'" + "," + "'" + cbl_hos.Items[i].Value.ToString() + "";
                    }
                }
            }

            string build = d2.GetBuildingCode_inv(hostel);
            char[] delimiterChars = { ',' };
            string[] build1 = build.Split(delimiterChars);
            string build2 = "";

            foreach (string b in build1)
            {
                if (build2 == "")
                {
                    build2 = "" + b + "";
                }
                else
                {
                    build2 = build2 + "'" + "," + "'" + b + "";
                }
            }

            DataSet ds1 = new DataSet();
            string floor = "select code,Building_Name from Building_Master where code in ('" + build2 + "')";
            ds1 = d2.select_method_wo_parameter(floor, "Text");
            string w = "";
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string q1 = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    string q = Convert.ToString(ds1.Tables[0].Rows[i][1]);
                    if (w == "")
                    {
                        w = "" + q + "";
                    }
                    else
                    {
                        w = w + "'" + "," + "'" + q + "";
                    }
                    cbl_buildname.DataSource = ds1;
                    cbl_buildname.DataTextField = "Building_Name";
                    cbl_buildname.DataValueField = "code";
                    cbl_buildname.DataBind();
                    if (cbl_buildname.Items.Count > 0)
                    {
                        for (int j = 0; j < cbl_buildname.Items.Count; j++)
                        {

                            // cbl_buildname.Items[j].Selected = true;
                        }

                        txt_buildingname.Text = "Building Name(" + cbl_buildname.Items.Count + ")";
                    }

                    else
                    {
                        //ddl_floorname.Items.Insert(0, "Select");
                        txt_buildingname.Text = "--Select--";
                    }



                }
            }
            ds.Clear();
            ds = d2.BindFloor_new(w);

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "FloorPK";
                cbl_floorname.DataBind();





                if (cbl_floorname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_floorname.Items.Count; i++)
                    {

                        // cbl_floorname.Items[i].Selected = true;
                    }

                    txt_floorname.Text = "Floor Name(" + cbl_floorname.Items.Count + ")";
                }
            }
            else
            {
                //ddl_floorname.Items.Insert(0, "Select");
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
            cbl_roomname.Items.Clear();
            txt_roomname.Text = "---Select---";
            cb_roomname.Checked = false;
            string query = "";
            query = "select distinct rd.Roompk,rd.Room_Name from Room_Detail rd,Floor_Master hd where rd.Floor_Name=hd.Floor_Name and hd.FloorPK in('" + floor + "')  order by Roompk";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomname.DataSource = ds;
                cbl_roomname.DataTextField = "Room_Name";
                cbl_roomname.DataValueField = "Roompk";
                cbl_roomname.DataBind();

                if (cbl_roomname.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_roomname.Items.Count; row++)
                    {
                        // cbl_roomname.Items[row].Selected = true;
                    }
                    txt_roomname.Text = "Room (" + cbl_roomname.Items.Count + ")";
                    cb_roomname.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_mess_CheckedChange(object sender, EventArgs e)
    {
        if (cb_hos.Checked == true)
        {
            for (int i = 0; i < cbl_hos.Items.Count; i++)
            {
                cbl_hos.Items[i].Selected = true;
            }
            txt_messname.Text = "Hostel Name(" + (cbl_hos.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hos.Items.Count; i++)
            {
                cbl_hos.Items[i].Selected = false;
            }
            txt_messname.Text = "--Select--";
            txt_floorname.Text = "--Select--";
        }
        bindfloor();
        bindroom();
        //cb_floorname_CheckedChange(sender, e);
        //cbl_floorname_SelectedIndexChanged(sender, e);

    }
    public void cbl_mess_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_hos.Checked = false;
        int commcount = 0;
        string buildvalue = "";
        string build = "";
        txt_messname.Text = "--Select--";
        for (i = 0; i < cbl_hos.Items.Count; i++)
        {
            if (cbl_hos.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_hos.Checked = false;
                ///new 22/08/15
                build = cbl_hos.Items[i].Value.ToString();
                if (buildvalue == "")
                {
                    buildvalue = build;
                }
                else
                {
                    buildvalue = buildvalue + "'" + "," + "'" + build;
                }
                clgbuild(buildvalue);
                Hostelcode = buildvalue;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_hos.Items.Count)
            {
                cb_hos.Checked = true;
            }
            txt_messname.Text = "Hostel Name(" + commcount.ToString() + ")";
        }
    }

    protected void cb_room_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_roomname.Text = "--Select--";
            if (cb_roomname.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = true;
                }
                txt_roomname.Text = "Room (" + (cbl_roomname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = false;
                }
                txt_roomname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_room_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_roomname.Checked = false;
        int commcount = 0;

        txt_roomname.Text = "--Select--";

        for (int i = 0; i < cbl_roomname.Items.Count; i++)
        {
            if (cbl_roomname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_roomname.Items.Count)
            {
                cb_roomname.Checked = true;
            }
            txt_roomname.Text = "Room (" + commcount.ToString() + ")";
        }
    }

    public void clgbuild(string hostelname)
    {
        try
        {
            cbl_buildname.Items.Clear();
            string bul = "";
            bul = d2.GetBuildingCode_inv(hostelname);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_buildname.DataSource = ds;
                cbl_buildname.DataTextField = "Building_Name";
                cbl_buildname.DataValueField = "code";
                cbl_buildname.DataBind();
            }
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                // cbl_buildname.Items[i].Selected = true;
                txt_buildingname.Text = "Building(" + (cbl_buildname.Items.Count) + ")";
                // cb_buildname.Checked = true;
            }
            string locbuild = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    string builname = cbl_buildname.Items[i].Text;
                    if (locbuild == "")
                    {
                        locbuild = builname;
                    }
                    else
                    {
                        locbuild = locbuild + "'" + "," + "'" + builname;
                    }
                }
            }
            clgfloor(locbuild);
        }
        catch (Exception ex)
        {
        }
    }

    public void clgfloor(string buildname)
    {
        try
        {
            cbl_floorname.Items.Clear();
            //ds = d2.BindFloor_new(buildname);
            string itemname = "select distinct Floor_Name,FloorPK from Floor_Master where Building_Name in('" + buildname + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "FloorPK";
                cbl_floorname.DataBind();
            }
            else
            {
                txt_floorname.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                // cbl_floorname.Items[i].Selected = true;
                //  cb_floorname.Checked = true;
            }
            string locfloor = "";
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                    string flrname = cbl_floorname.Items[i].Text; //cbl_floorname.SelectedItem.Text; 
                    if (locfloor == "")
                    {
                        locfloor = flrname;
                    }
                    else
                    {
                        locfloor = locfloor + "'" + "," + "'" + flrname;
                    }
                }
            }
            clgroom(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }

    public void clgroom(string floorname, string buildname)
    {
        try
        {
            cbl_roomname.Items.Clear();
            //ds = d2.BindRoom(floorname, buildname);changed at sairam 29.09.16//11.04.17 barath
            string itemname = "select Room_Name,Roompk from Room_Detail where Building_Name in('" + buildname + "') and floor_name in('" + floorname + "') order by (len(Room_Name)) asc,Room_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomname.DataSource = ds;
                cbl_roomname.DataTextField = "Room_Name";
                cbl_roomname.DataValueField = "Roompk";
                cbl_roomname.DataBind();
            }
            else
            {
                txt_roomname.Text = "--Select--";
            }
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                // cbl_roomname.Items[i].Selected = true;
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
                // cb_roomname.Checked = true;
            }
            string room = "";
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    string flrname = cbl_roomname.Items[i].Text;
                    if (room == "")
                    {
                        room = flrname;
                    }
                    else
                    {
                        room = room + "'" + "," + "'" + flrname;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void cb_buildname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_buildname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_buildname.Items.Count; i++)
                {
                    if (cb_buildname.Checked == true)
                    {
                        cbl_buildname.Items[i].Selected = true;
                        txt_buildingname.Text = "Building(" + (cbl_buildname.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_buildname.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                clgfloor(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_buildname.Items.Count; i++)
                {
                    cbl_buildname.Items[i].Selected = false;
                    txt_buildingname.Text = "--Select--";
                    cbl_floorname.Items.Clear();
                    cb_floorname.Checked = false;
                    txt_floorname.Text = "--Select--";
                    txt_roomname.Text = "--Select--";
                    cb_roomname.Checked = false;
                    cbl_roomname.Items.Clear();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_buildname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_buildname.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    // txt_floorname.Text = "--Select--";
                    cb_floorname.Checked = true;
                    build = cbl_buildname.Items[i].Text.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            clgfloor(buildvalue);
            if (seatcount == cbl_buildname.Items.Count)
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
                cb_buildname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_buildingname.Text = "--Select--";
            }
            else
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void cb_floorname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_floorname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";
                if (cb_buildname.Checked == true)
                {
                    for (int i = 0; i < cbl_buildname.Items.Count; i++)
                    {
                        build1 = cbl_buildname.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                if (cb_floorname.Checked == true)
                {
                    for (int j = 0; j < cbl_floorname.Items.Count; j++)
                    {
                        cbl_floorname.Items[j].Selected = true;
                        txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                        build2 = cbl_floorname.Items[j].Text.ToString();
                        if (buildvalue2 == "")
                        {
                            buildvalue2 = build2;
                        }
                        else
                        {
                            buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                        }
                    }
                }
                clgroom(buildvalue2, buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = false;
                    txt_floorname.Text = "--Select--";
                }
                cb_roomname.Checked = false;
                cbl_roomname.Items.Clear();
                txt_roomname.Text = "--Select--";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_floorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floorname.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    build1 = cbl_buildname.Items[i].Text.ToString();
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                    }
                }
            }
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    build2 = cbl_floorname.Items[i].Text.ToString();
                    if (buildvalue2 == "")
                    {
                        buildvalue2 = build2;
                    }
                    else
                    {
                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                    }
                }
            }
            clgroom(buildvalue2, buildvalue1);
            if (seatcount == cbl_floorname.Items.Count)
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
                cb_floorname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_floorname.Text = "--Select--";
            }
            else
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion
    //protected void cb_mess_CheckedChange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int cout = 0;
    //        txt_messname.Text = "--Select--";
    //        if (cb_hos.Checked == true)
    //        {
    //            cout++;
    //            for (int i = 0; i < cbl_hos.Items.Count; i++)
    //            {
    //                cbl_hos.Items[i].Selected = true;
    //            }
    //            txt_messname.Text = "Mess Name(" + (cbl_hos.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cbl_hos.Items.Count; i++)
    //            {
    //                cbl_hos.Items[i].Selected = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //protected void cbl_mess_SelectedIndexChange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int i = 0;
    //        cb_hos.Checked = false;
    //        int commcount = 0;
    //        txt_messname.Text = "--Select--";
    //        for (i = 0; i < cbl_hos.Items.Count; i++)
    //        {
    //            if (cbl_hos.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //                cb_hos.Checked = false;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            if (commcount == cbl_hos.Items.Count)
    //            {
    //                cb_hos.Checked = true;
    //            }
    //            txt_messname.Text = "Mess Name(" + commcount.ToString() + ")";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    protected void ddl_BreakageHeader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int dropname = 1;
            loadHostelLedger(dropname);

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }
    protected void ddl_GymHeader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int dropname = 2;
            loadHostelLedger(dropname);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }
    protected void ddl_HealthHeader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int dropname = 3;
            loadHostelLedger(dropname);

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }

    protected void ddl_RegularHeader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int dropname = 4;
            loadHostelLedger(dropname);

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }

    protected void ddl_GustHeader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int dropname = 5;
            loadHostelLedger(dropname);

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }

    protected void ddl_StaffHeader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int dropname = 6;
            loadHostelLedger(dropname);

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }

    protected void ddl_OthersHeader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int dropname = 7;
            loadHostelLedger(dropname);

        }
        catch (Exception ex)
        {
        }
    }
    protected void rdb_inmess_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }

    }
    protected void rdb_exmess_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }

    }
    protected void rdb_Gymin_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }

    }
    protected void rdb_Gymex_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }

    }
    protected void rdb_healthin_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }

    }
    protected void rdb_healthex_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }
    }
    protected void rdb_hostelattn_CheckedChanged(object sender, EventArgs e)
    {
    }
    protected void rdb_messattn_CheckedChanged(object sender, EventArgs e)
    {
    }
    protected void rdb_bothattn_CheckedChanged(object sender, EventArgs e)
    {
    }
    protected void ddl_Hostel_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_Hostel.Items.Count > 0)
            {
                int count = 0;
                string hoste = Convert.ToString(ddl_Hostel.SelectedValue);
                string cun = d2.GetFunction("select HostelGatePassPerCount from  HM_HostelMaster  where HostelMasterPK='" + Convert.ToString(ddl_Hostel.SelectedValue) + "'");
                txt_count.Text = cun;
            }

        }
        catch
        {
        }
    }
    protected void ddlcollegeco_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlcollegeco.Items.Count > 0)
            {
                int count = 0;
                Txtcol_count.Text = "";
                string hoste = Convert.ToString(ddlcollegeco.SelectedValue);
                string cun = d2.GetFunction("select leavecount from gatepasscount where college_code='" + Convert.ToString(ddlcollegeco.SelectedValue) + "'");
                Txtcol_count.Text = cun;
            }
        }
        catch
        {
        }
    }
    #endregion

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        StringBuilder selectedvalue = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        StringBuilder selectedText = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedText.ToString();
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    private bool getSelectedCheckBoxListCount(CheckBoxList cbl, out int selectedCount)
    {
        selectedCount = 0;
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                if (li.Selected)
                {
                    selectedCount++;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="c">Only Data Bound Controls eg.DropDownList,RadioButtonList,CheckBoxList </param>
    /// <param name="selectedValue"></param>
    /// <param name="selectedText"></param>
    /// <param name="type">0 - Index; 1 - Text; 2 - Value;</param>
    private void SelectDataBound(Control c, string selectedValue, string selectedText)
    {
        try
        {
            bool isDataBoundControl = false;
            if (c is DataBoundControl)
            {
                if (c is CheckBoxList || c is DropDownList || c is RadioButtonList)
                {
                    isDataBoundControl = true;
                }
                if (isDataBoundControl)
                {
                    ListControl lstControls = (ListControl)c;
                    if (lstControls.Items.Count > 0)
                    {
                        ListItem[] listItem = new ListItem[lstControls.Items.Count];
                        lstControls.Items.CopyTo(listItem, 0);
                        if (listItem.Contains(new ListItem(selectedText, selectedValue)))
                        {
                            lstControls.SelectedValue = selectedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region application
    //krishhna kumar.r

    protected void btnapplication_Click(object sender, EventArgs e)
    {
        try
        {
            string usercode = ddluser.SelectedValue;
            int value = 0;
            if (chkapplication.Checked)
                value = 1;
            else
                value = 0;
            string sql = "if exists(select * from Master_Settings where settings = 'Include Eligibility MarkSetting') update Master_Settings set value='" + value + "' where settings = 'Include Eligibility MarkSetting' else insert into Master_Settings (usercode,settings,value) values ('" + usercode + "','Include Eligibility MarkSetting','" + value + "')";
            int status = dacc.update_method_wo_parameter(sql, "text");
            imgAlert.Visible = true;
            lbl_alert.Text = "Saved successfully";

        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");

        }
    }
    #endregion

    #region Inventory


    protected void btninventory_Click(object sender, EventArgs e)
    {
        divinventory.Visible = true;
        bindGrid();
        // inventory();
    }
    protected void gridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[1].Attributes["Onchange"] = Page.ClientScript.GetPostBackEventReference(gridView1, "Select$" + e.Row.RowIndex);
                if (rbl_Com_ind.SelectedIndex == 0)
                {
                    Panel Kit = (Panel)e.Row.Cells[1].FindControl("pan_kit");
                    Kit.Visible = true;
                }
                else
                {
                    DropDownList ind = (DropDownList)e.Row.Cells[1].FindControl("ddl_Kitname");
                    ind.Visible = true;
                }
            }
        }
        catch
        {

        }
    }
    protected void gridView1_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        lbl_er.Visible = false;
        string strdname = "";
        string collegecode = string.Empty;
        string usercode = ddluser.SelectedValue;
        if (ddlcollege.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
        int n = Convert.ToInt32(e.CommandArgument);
        DropDownList strhdname = (DropDownList)gridView1.Rows[n].FindControl("ddl_headername");
        (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Clear();
        // string englisquery = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode='" + collegecode1 + "' and LedgerMode=1 and HeaderFK ='" + strhdname.SelectedItem.Value + "' order by isnull(priority,1000),ledgerName asc";

        string englisquery = "SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode + "    order by isnull(l.priority,1000), l.ledgerName asc";
        ds.Clear();
        ds = d2.select_method_wo_parameter(englisquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {

            (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataSource = ds;
            (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataTextField = "LedgerName";
            (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataValueField = "LedgerPK";
            (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataBind();

            (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");
            lbl_er.Visible = false;
        }
        else
        {
            (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");
        }
        //try
        //{

        //    if (gridView1.Rows.Count > 0)
        //    {

        //        for (int a = 0; a < gridView1.Rows.Count; a++)
        //        {
        //            //academic year

        //            (gridView1.Rows[a].FindControl("ddl_kitname") as DropDownList).Items.Clear();

        //            (gridView1.Rows[a].FindControl("ddl_kitname") as DropDownList).DataSource = dtYear;
        //            (gridView1.Rows[a].FindControl("ddl_kitname") as DropDownList).DataTextField = "Academic_Year";
        //            (gridView1.Rows[a].FindControl("ddl_kitname") as DropDownList).DataValueField = "Academic_Year";
        //            (gridView1.Rows[a].FindControl("ddl_kitname") as DropDownList).DataBind();

        //            //(gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).Items.Insert(0, "Select");
        //            //batch year
        //            (gridView1.Rows[a].FindControl("ddl_headername") as DropDownList).Items.Clear();

        //            (gridView1.Rows[a].FindControl("ddl_headername") as DropDownList).DataSource = dsBatch;
        //            (gridView1.Rows[a].FindControl("ddl_headername") as DropDownList).DataTextField = "Batch_year";
        //            (gridView1.Rows[a].FindControl("ddl_headername") as DropDownList).DataValueField = "Batch_year";
        //            (gridView1.Rows[a].FindControl("ddl_headername") as DropDownList).DataBind();

        //            // (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).Items.Insert(0, "Select");
        //            //feecategory 
        //            (gridView1.Rows[a].FindControl("ddl_ledgername") as CheckBoxList).Items.Clear();


        //            (gridView1.Rows[a].FindControl("ddl_ledgername") as CheckBoxList).DataSource = dsTemp;
        //            (gridView1.Rows[a].FindControl("ddl_ledgername") as CheckBoxList).DataTextField = "TextVal";
        //            (gridView1.Rows[a].FindControl("ddl_ledgername") as CheckBoxList).DataValueField = "TextVal";
        //            (gridView1.Rows[a].FindControl("ddl_ledgername") as CheckBoxList).DataBind();

        //            // (gdSetting.Rows[a].FindControl("ddlFeecat") as DropDownList).Items.Insert(0, "Select");
        //        }
        //    }
        //}
        //catch
        //{ }
    }
    public void bindGrid()
    {
        ArrayList addnew = new ArrayList();
        addnew.Add("1");
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("Kit Name");
        dt.Columns.Add("Header Name");
        dt.Columns.Add("Ledger Name");
        dt.Columns.Add("Amount");
        DataRow dr;
        for (int row = 0; row < addnew.Count; row++)
        {
            dr = dt.NewRow();
            dr[0] = addnew[row].ToString();
            dt.Rows.Add(dr);
        }
        if (dt.Rows.Count > 0)
        {
            ViewState["CurrentTable"] = dt;
            gridView1.DataSource = dt;
            //gridView2.DataSource = dt;
            //gridView2.DataBind();
            gridView1.DataBind();
        }
    }
    protected void Marksgrid_pg_DataBound(object sender, EventArgs e)
    {
        try
        {
            string collegecode = string.Empty;
            string usercode = ddluser.SelectedValue;
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            (gridView1.Rows[0].FindControl("Kit_name") as CheckBoxList).Items.Clear();
            (gridView1.Rows[0].FindControl("ddl_Kitname") as DropDownList).Items.Clear();
            (gridView1.Rows[0].FindControl("ddl_headername") as DropDownList).Items.Clear();
            (gridView1.Rows[0].FindControl("ddl_ledgername") as DropDownList).Items.Clear();
            if (gridView1.Rows.Count > 0)
            {
                // lbl_er.Visible = false;
                for (int a = 0; a < gridView1.Rows.Count; a++)
                {
                    // string englisquery = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "'";

                    string englisquery = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode + "  ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(englisquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        (gridView1.Rows[a].FindControl("ddl_headername") as DropDownList).DataSource = ds;
                        (gridView1.Rows[a].FindControl("ddl_headername") as DropDownList).DataTextField = "HeaderName";
                        (gridView1.Rows[a].FindControl("ddl_headername") as DropDownList).DataValueField = "HeaderPK";
                        (gridView1.Rows[a].FindControl("ddl_headername") as DropDownList).DataBind();
                    }
                    (gridView1.Rows[a].FindControl("ddl_headername") as DropDownList).Items.Insert(0, "Select");

                    englisquery = "select * from CO_MasterValues where MasterCriteria='kit' and CollegeCode='" + collegecode + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(englisquery, "Text");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (rbl_Com_ind.SelectedIndex == 0)
                        {
                            (gridView1.Rows[a].FindControl("Kit_name") as CheckBoxList).DataSource = ds;
                            (gridView1.Rows[a].FindControl("Kit_name") as CheckBoxList).DataTextField = "MasterValue";
                            (gridView1.Rows[a].FindControl("Kit_name") as CheckBoxList).DataValueField = "MasterCode";
                            (gridView1.Rows[a].FindControl("Kit_name") as CheckBoxList).DataBind();
                        }
                        else
                        {
                            (gridView1.Rows[a].FindControl("ddl_Kitname") as DropDownList).DataSource = ds;
                            (gridView1.Rows[a].FindControl("ddl_Kitname") as DropDownList).DataTextField = "MasterValue";
                            (gridView1.Rows[a].FindControl("ddl_Kitname") as DropDownList).DataValueField = "MasterCode";
                            (gridView1.Rows[a].FindControl("ddl_Kitname") as DropDownList).DataBind();

                        }
                    }
                    else
                    {
                        if (rbl_Com_ind.SelectedIndex == 0)
                            (gridView1.Rows[a].FindControl("Kit_name") as CheckBoxList).Items.Insert(0, "Select");
                        else
                            (gridView1.Rows[a].FindControl("ddl_Kitname") as DropDownList).Items.Insert(0, "Select");
                    }
                    //  lbl_er.Visible = false;
                    //  englisquery = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode='" + collegecode1 + "' and LedgerMode=1 order by isnull(priority,1000), ledgerName asc";
                    //int n = Convert.ToInt32(e.CommandArgument);
                    //DropDownList strhdname = (DropDownList)gridView1.Rows[n].FindControl("ddl_headername");
                    englisquery = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode + "    order by isnull(l.priority,1000), l.ledgerName asc ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(englisquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        (gridView1.Rows[a].FindControl("ddl_ledgername") as DropDownList).DataSource = ds;
                        (gridView1.Rows[a].FindControl("ddl_ledgername") as DropDownList).DataTextField = "LedgerName";
                        (gridView1.Rows[a].FindControl("ddl_ledgername") as DropDownList).DataValueField = "LedgerPK";
                        (gridView1.Rows[a].FindControl("ddl_ledgername") as DropDownList).DataBind();
                    }
                    (gridView1.Rows[a].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");

                }

                //Div5.Visible = true;
            }
        }
        catch
        {

        }


    }
    public void btnaddgrid_Click(object sender, EventArgs e)
    {
        try
        {
            bool emptyflage = false;
            if (gridView1.Rows.Count > 0)
            {
                CheckBoxList box = new CheckBoxList();
                DropDownList box1 = new DropDownList();
                DropDownList box2 = new DropDownList();
                DropDownList box4 = new DropDownList();
                TextBox box3 = new TextBox();
                if (rbl_Com_ind.SelectedIndex == 0)
                {
                    for (int i = 0; i < gridView1.Rows.Count; i++)
                    {
                        box = (CheckBoxList)gridView1.Rows[i].Cells[1].FindControl("Kit_name");
                        box1 = (DropDownList)gridView1.Rows[i].Cells[2].FindControl("ddl_headername");
                        box2 = (DropDownList)gridView1.Rows[i].Cells[3].FindControl("ddl_ledgername");
                        box3 = (TextBox)gridView1.Rows[i].Cells[4].FindControl("txtpaymt");

                        if (box.Text != "Select" && box1.Text != "Select" && box2.Text != "Select" && box3.Text.Trim() != "")
                        {
                            if (box.Text != "" && box1.Text != "" && box2.Text != "" && box3.Text.Trim() != "")
                                emptyflage = false;
                            else
                                emptyflage = true;
                        }
                        else
                            emptyflage = true;
                    }
                }
                else
                {
                    for (int i = 0; i < gridView1.Rows.Count; i++)
                    {
                        box4 = (DropDownList)gridView1.Rows[i].Cells[1].FindControl("ddl_Kitname");
                        box1 = (DropDownList)gridView1.Rows[i].Cells[2].FindControl("ddl_headername");
                        box2 = (DropDownList)gridView1.Rows[i].Cells[3].FindControl("ddl_ledgername");
                        box3 = (TextBox)gridView1.Rows[i].Cells[4].FindControl("txtpaymt");

                        if (box4.Text != "Select" && box1.Text != "Select" && box2.Text != "Select" && box3.Text.Trim() != "")
                        {
                            if (box4.Text != "" && box1.Text != "" && box2.Text != "" && box3.Text.Trim() != "")
                                emptyflage = false;
                            else
                                emptyflage = true;
                        }
                        else
                            emptyflage = true;
                    }

                }
            }
            //.style.borderColor = 'Red'
            if (emptyflage == true)
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Fill All The Fields\");", true);
            else
            {
                AddNewRowToGrid();
                Marksgrid_pg_DataBound(sender, e);
                // gridView1_OnRowCommand(sender, e);                
                SetPreviousData1();
            }
        }
        catch
        {
        }
    }
    public void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            CheckBoxList box = new CheckBoxList();
            DropDownList box1 = new DropDownList();
            DropDownList box2 = new DropDownList();
            DropDownList box4 = new DropDownList();
            TextBox box3 = new TextBox();


            if (dtCurrentTable.Rows.Count > 0)
            {
                if (rbl_Com_ind.SelectedIndex == 0)
                {
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        box = (CheckBoxList)gridView1.Rows[i].Cells[1].FindControl("Kit_name");
                        box1 = (DropDownList)gridView1.Rows[i].Cells[2].FindControl("ddl_headername");
                        box2 = (DropDownList)gridView1.Rows[i].Cells[3].FindControl("ddl_ledgername");
                        box3 = (TextBox)gridView1.Rows[i].Cells[4].FindControl("txtpaymt");
                        //  drCurrentRow["RowNumber"] = i + 1;
                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows[i][0] = Convert.ToString(i + 1);
                        dtCurrentTable.Rows[i][1] = box.Text;
                        dtCurrentTable.Rows[i][2] = box1.Text;
                        dtCurrentTable.Rows[i][3] = box2.Text;
                        dtCurrentTable.Rows[i][4] = box3.Text;

                        rowIndex++;
                    }
                }
                else
                {
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        box4 = (DropDownList)gridView1.Rows[i].Cells[1].FindControl("ddl_Kitname");
                        box1 = (DropDownList)gridView1.Rows[i].Cells[2].FindControl("ddl_headername");
                        box2 = (DropDownList)gridView1.Rows[i].Cells[3].FindControl("ddl_ledgername");
                        box3 = (TextBox)gridView1.Rows[i].Cells[4].FindControl("txtpaymt");
                        //  drCurrentRow["RowNumber"] = i + 1;
                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows[i][0] = Convert.ToString(i + 1);
                        dtCurrentTable.Rows[i][1] = box4.Text;
                        dtCurrentTable.Rows[i][2] = box1.Text;
                        dtCurrentTable.Rows[i][3] = box2.Text;
                        dtCurrentTable.Rows[i][4] = box3.Text;

                        rowIndex++;
                    }

                }

                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                gridView1.DataSource = dtCurrentTable;
                gridView1.DataBind();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"View State Null\");", true);
            // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('View State Null')", true);
        }
        //
        // SetPreviousData1();
    }
    public void SetPreviousData1()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            Hashtable hashlist = new Hashtable();
            if (dt.Rows.Count > 0)
            {
                CheckBoxList box = new CheckBoxList();
                DropDownList box1 = new DropDownList();
                DropDownList box2 = new DropDownList();
                DropDownList box4 = new DropDownList();
                TextBox box3 = new TextBox();
                Label lbl = new Label();

                hashlist.Add(0, "Sno");
                hashlist.Add(1, "Kit Name");
                hashlist.Add(2, "Header Name");
                hashlist.Add(3, "Ledger Name");
                hashlist.Add(4, "Amount");

                DataRow dr;
                if (rbl_Com_ind.SelectedIndex == 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        box = (CheckBoxList)gridView1.Rows[i].Cells[1].FindControl("Kit_name");
                        box1 = (DropDownList)gridView1.Rows[i].Cells[2].FindControl("ddl_headername");
                        box2 = (DropDownList)gridView1.Rows[i].Cells[3].FindControl("ddl_ledgername");
                        box3 = (TextBox)gridView1.Rows[i].Cells[4].FindControl("txtpaymt");
                        lbl = (Label)gridView1.Rows[i].Cells[4].FindControl("lbl_rs");
                        string val_file = Convert.ToString(hashlist[i]);
                        lbl.Text = Convert.ToString(i + 1);
                        //  ddlBatch_year.SelectedIndex = ddlBatch_year.Items.IndexOf(ddlBatch_year.Items.FindByText(Convert.ToString(Batch_year)));
                        string kit = dt.Rows[i][1].ToString();
                        string hedid = dt.Rows[i][2].ToString();
                        string ledgid = dt.Rows[i][3].ToString();
                        box.SelectedIndex = box.Items.IndexOf(box.Items.FindByValue(Convert.ToString(dt.Rows[i][1])));
                        box1.SelectedIndex = box1.Items.IndexOf(box1.Items.FindByValue(Convert.ToString(dt.Rows[i][2])));
                        gridledgerload(hedid, i);
                        box2.SelectedIndex = box2.Items.IndexOf(box2.Items.FindByValue(Convert.ToString(dt.Rows[i][3])));
                        // box1.Text = dt.Rows[i][1].ToString();
                        //  box2.Text = dt.Rows[i][2].ToString();
                        box3.Text = dt.Rows[i][4].ToString();

                        rowIndex++;
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        box4 = (DropDownList)gridView1.Rows[i].Cells[1].FindControl("ddl_Kitname");
                        box1 = (DropDownList)gridView1.Rows[i].Cells[2].FindControl("ddl_headername");
                        box2 = (DropDownList)gridView1.Rows[i].Cells[3].FindControl("ddl_ledgername");
                        box3 = (TextBox)gridView1.Rows[i].Cells[4].FindControl("txtpaymt");
                        lbl = (Label)gridView1.Rows[i].Cells[4].FindControl("lbl_rs");
                        string val_file = Convert.ToString(hashlist[i]);
                        lbl.Text = Convert.ToString(i + 1);
                        //  ddlBatch_year.SelectedIndex = ddlBatch_year.Items.IndexOf(ddlBatch_year.Items.FindByText(Convert.ToString(Batch_year)));
                        string kit = dt.Rows[i][1].ToString();
                        string hedid = dt.Rows[i][2].ToString();
                        string ledgid = dt.Rows[i][3].ToString();
                        box4.SelectedIndex = box4.Items.IndexOf(box.Items.FindByValue(Convert.ToString(dt.Rows[i][1])));
                        box1.SelectedIndex = box1.Items.IndexOf(box1.Items.FindByValue(Convert.ToString(dt.Rows[i][2])));
                        gridledgerload(hedid, i);
                        box2.SelectedIndex = box2.Items.IndexOf(box2.Items.FindByValue(Convert.ToString(dt.Rows[i][3])));
                        // box1.Text = dt.Rows[i][1].ToString();
                        //  box2.Text = dt.Rows[i][2].ToString();
                        box3.Text = dt.Rows[i][4].ToString();

                        rowIndex++;
                    }

                }
            }
        }
    }
    protected void gridledgerload(string hedid, int n)
    {
        try
        {
            lbl_er.Visible = false;
            string strdname = "";
            string collegecode = string.Empty;
            string usercode = ddluser.SelectedValue;
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            string englisquery = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + "   and L.CollegeCode = " + collegecode + "  and LedgerMode=1 and l.HeaderFK ='" + hedid + "'   order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(englisquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataSource = ds;
                (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataTextField = "LedgerName";
                (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataValueField = "LedgerPK";
                (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataBind();

                (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");
                lbl_er.Visible = false;
            }
            else
            {
                (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");
            }
        }
        catch { }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int save = 0;
        collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        string usercode = ddluser.SelectedValue;
        if (rbl_Com_ind.SelectedIndex == 0)
        {
            foreach (GridViewRow gdRow in gridView1.Rows)
            {
                CheckBoxList kit_name = (CheckBoxList)gdRow.FindControl("kit_name");
                DropDownList headername = (DropDownList)gdRow.FindControl("ddl_headername");
                DropDownList ledgername = (DropDownList)gdRow.FindControl("ddl_ledgername");
                TextBox Amount = (TextBox)gdRow.FindControl("txtpaymt");
                if (kit_name.Items.Count > 0 && headername.Items.Count > 0 && ledgername.Items.Count > 0)
                {
                    for (int row = 0; row < kit_name.Items.Count; row++)
                    {
                        if (!kit_name.Items[row].Selected)
                            continue;
                        string kit = Convert.ToString(kit_name.Items[row].Value);
                        string header = Convert.ToString(headername.SelectedItem.Value);
                        string ledger = Convert.ToString(ledgername.SelectedItem.Value);
                        string amt = Amount.Text;
                        string sql = string.Empty;
                        //sql = "insert into inventorykit(kitid,headerid,ledgerid,amt,collegecode,usercode,CommonOrIndividual)values('" + kit + "','" + header + "','" + ledger + "','" + amt + "','" + collegecode + "','" + usercode + "','0')";
                        sql = "if exists(select * from inventorykit where kitid='" + kit + "' and headerid='" + header + "' and ledgerid='" + ledger + "' and collegecode='" + collegecode + "' and usercode='" + usercode + "' and CommonOrIndividual='0')update inventorykit set amt='" + amt + "' where kitid='" + kit + "' and headerid='" + header + "' and ledgerid='" + ledger + "' and collegecode='" + collegecode + "' and usercode='" + usercode + "' and CommonOrIndividual='0' else insert into inventorykit (kitid,headerid,ledgerid,amt,collegecode,usercode,CommonOrIndividual)values('" + kit + "','" + header + "','" + ledger + "','" + amt + "','" + collegecode + "','" + usercode + "','0')";
                        save = d2.update_method_wo_parameter(sql, "Text");
                    }

                }
            }
        }
        else
        {
            foreach (GridViewRow gdRow in gridView1.Rows)
            {
                DropDownList kitname = (DropDownList)gdRow.FindControl("ddl_Kitname");
                DropDownList headername = (DropDownList)gdRow.FindControl("ddl_headername");
                DropDownList ledgername = (DropDownList)gdRow.FindControl("ddl_ledgername");
                TextBox Amount = (TextBox)gdRow.FindControl("txtpaymt");
                if (kitname.Items.Count > 0 && headername.Items.Count > 0 && ledgername.Items.Count > 0)
                {
                    string kit = Convert.ToString(kitname.SelectedItem.Value);
                    string header = Convert.ToString(headername.SelectedItem.Value);
                    string ledger = Convert.ToString(ledgername.SelectedItem.Value);
                    string amt = Amount.Text;
                    string sql = string.Empty;
                    //sql = "insert into inventorykit(kitid,headerid,ledgerid,amt,collegecode,usercode,CommonOrIndividual)values('" + kit + "','" + header + "','" + ledger + "','" + amt + "','" + collegecode + "','" + usercode + "','1')";
                    sql = "if exists(select * from inventorykit where kitid='" + kit + "' and headerid='" + header + "' and ledgerid='" + ledger + "' and collegecode='" + collegecode + "' and usercode='" + usercode + "' and CommonOrIndividual='1')update inventorykit set amt='" + amt + "' where kitid='" + kit + "' and headerid='" + header + "' and ledgerid='" + ledger + "' and collegecode='" + collegecode + "' and usercode='" + usercode + "' and CommonOrIndividual='1' else insert into inventorykit (kitid,headerid,ledgerid,amt,collegecode,usercode,CommonOrIndividual)values('" + kit + "','" + header + "','" + ledger + "','" + amt + "','" + collegecode + "','" + usercode + "','1')";
                    save = d2.update_method_wo_parameter(sql, "Text");

                }
            }
        }
        if (save == 1)
        {
            imgAlert.Visible = true;

            lbl_alert.Text = "Saved Successfully";
        }
        else
        {
            imgAlert.Visible = true;

            lbl_alert.Text = "Not Saved";
        }

    }

    #endregion

    #region inventory go

    public void inventory()
    {
        gdReport.Visible = true;
        DataRow drReport;
        DataTable dtReport = new DataTable();
        dtReport.Columns.Add("Sno");
        dtReport.Columns.Add("kitname");
        dtReport.Columns.Add("headername");
        //dtReport.Columns.Add("headerid");
        dtReport.Columns.Add("ledgername");
        //dtReport.Columns.Add("ledgerid");

        // dtReport.Columns.Add("kitid");
        dtReport.Columns.Add("amt");
        collegecode = ddlcollege.SelectedItem.Value;
        string usercode = ddluser.SelectedValue;
        string sql = "select MasterValue,headername,ledgername,amt,i.collegecode,i.usercode from inventorykit i,FM_HeaderMaster h ,FM_LedgerMaster l,CO_MasterValues  c where i.kitid =c.MasterCode  and h.HeaderPK =i.headerid and l.LedgerPK =i.ledgerid and i.collegecode='" + collegecode + "' and usercode='" + usercode + "'";
        DataSet ds = new DataSet();
        ds = d2.select_method_wo_parameter(sql, "text");


        int Sno = 0;
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            gridView1.DataSource = ds.Tables[0];


            DataTable dtkit = ds.Tables[0].DefaultView.ToTable(true, "MasterValue", "headername", "ledgername", "collegecode", "usercode", "amt");
            if (dtkit.Rows.Count > 0)
            {
                if (rbl_Com_ind.SelectedIndex == 0)
                {
                    for (int row = 0; row < dtkit.Rows.Count; row++)
                    {
                        Sno++;
                        drReport = dtReport.NewRow();
                        drReport["Sno"] = Convert.ToString(Sno);
                        drReport["kitname"] = Convert.ToString(dtkit.Rows[row]["MasterValue"]);
                        drReport["headername"] = Convert.ToString(dtkit.Rows[row]["headername"]);
                        drReport["ledgername"] = Convert.ToString(dtkit.Rows[row]["ledgername"]);
                        drReport["amt"] = Convert.ToString(dtkit.Rows[row]["amt"]);
                        dtReport.Rows.Add(drReport);
                        string kitname = Convert.ToString(dtkit.Rows[row]["MasterValue"]);
                        string headername = Convert.ToString(dtkit.Rows[row]["headername"]);
                        string ledgername = Convert.ToString(dtkit.Rows[row]["ledgername"]);
                        string amount = Convert.ToString(dtkit.Rows[row]["amt"]);
                        CheckBoxList kit_name = (CheckBoxList)gridView1.Rows[row].FindControl("kit_name");
                        DropDownList header = (DropDownList)gridView1.Rows[row].FindControl("ddl_headername");
                        DropDownList ledger = (DropDownList)gridView1.Rows[row].FindControl("ddl_ledgername");
                        TextBox txtamtvalue = (TextBox)gridView1.Rows[row].FindControl("txtpaymt");
                        txtamtvalue.Text = amount;
                        kit_name.SelectedItem.Text = kitname;
                        header.SelectedItem.Text = headername;
                        ledger.SelectedItem.Text = ledgername;

                    }
                }
                else
                {
                    for (int row = 0; row < dtkit.Rows.Count; row++)
                    {
                        Sno++;
                        drReport = dtReport.NewRow();
                        drReport["Sno"] = Convert.ToString(Sno);
                        drReport["kitname"] = Convert.ToString(dtkit.Rows[row]["MasterValue"]);
                        drReport["headername"] = Convert.ToString(dtkit.Rows[row]["headername"]);
                        drReport["ledgername"] = Convert.ToString(dtkit.Rows[row]["ledgername"]);
                        drReport["amt"] = Convert.ToString(dtkit.Rows[row]["amt"]);
                        dtReport.Rows.Add(drReport);
                        string kitname = Convert.ToString(dtkit.Rows[row]["MasterValue"]);
                        string headername = Convert.ToString(dtkit.Rows[row]["headername"]);
                        string ledgername = Convert.ToString(dtkit.Rows[row]["ledgername"]);
                        string amount = Convert.ToString(dtkit.Rows[row]["amt"]);
                        DropDownList kit_name = (DropDownList)gridView1.Rows[row].FindControl("ddl_Kitname");
                        DropDownList header = (DropDownList)gridView1.Rows[row].FindControl("ddl_headername");
                        DropDownList ledger = (DropDownList)gridView1.Rows[row].FindControl("ddl_ledgername");
                        TextBox txtamtvalue = (TextBox)gridView1.Rows[row].FindControl("txtpaymt");
                        txtamtvalue.Text = amount;
                        kit_name.SelectedItem.Text = kitname;
                        header.SelectedItem.Text = headername;
                        ledger.SelectedItem.Text = ledgername;

                    }

                }

            }
        }
    }

    //Added By Saranyadevi 18.5.2018

    protected void rbl_Com_ind_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindGrid();
            //if (rbl_Com_ind.SelectedIndex == 0)
            //{
            //    bindGrid();
            //    Panel Kit = (Panel)gridView1.SelectedRow.FindControl("pan_kit");
            //    //CheckBoxList kit = (CheckBoxList)gridView1.SelectedRow.FindControl("kit_name");
            //    Kit.Visible = true;
            //}
            //else
            //{
            //    bindGrid();
            //    DropDownList ind = (DropDownList)gridView1.SelectedRow.FindControl("ddl_Kitname");
            //    ind.Visible = true;
            //}
        }
        catch
        {
        }


    }


    #endregion

    #region library

    public void loadLibraryHeader()
    {
        try
        {
            string collegecodeNew = Convert.ToString(ddlcollege.SelectedItem.Value);
            if (ddlcollege.Items.Count > 0)
            {
                collegecodeNew = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            ddlLibFineHeader.Items.Clear();
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H WHERE CollegeCode = " + collegecodeNew + "";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlLibFineHeader.DataSource = ds;
                ddlLibFineHeader.DataTextField = "HeaderName";
                ddlLibFineHeader.DataValueField = "HeaderPK";
                ddlLibFineHeader.DataBind();
            }
            loadLibraryLedger();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }

    public void loadLibraryLedger()
    {
        try
        {
            string usercode = Convert.ToString(Session["usercode"]);
            string collegecodeNew = Convert.ToString(ddlcollege.SelectedItem.Value);
            if (ddlcollege.Items.Count > 0)
            {
                collegecodeNew = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            ddlLibFineLedger.Items.Clear();

            string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L WHERE  L.CollegeCode = " + collegecodeNew + " and L.HeaderFK in (" + Convert.ToString(ddlLibFineHeader.SelectedItem.Value) + ")";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlLibFineLedger.DataSource = ds;
                ddlLibFineLedger.DataTextField = "LedgerName";
                ddlLibFineLedger.DataValueField = "LedgerPK";
                ddlLibFineLedger.DataBind();
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }

    public void loadLibraryLostHeader()
    {
        try
        {
            string collegecodeNew = Convert.ToString(ddlcollege.SelectedItem.Value);
            if (ddlcollege.Items.Count > 0)
            {
                collegecodeNew = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            ddlLibFineHeader.Items.Clear();
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H WHERE CollegeCode = " + collegecodeNew + "";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlLibLostFineHeader.DataSource = ds;
                ddlLibLostFineHeader.DataTextField = "HeaderName";
                ddlLibLostFineHeader.DataValueField = "HeaderPK";
                ddlLibLostFineHeader.DataBind();

            }
            loadLibraryLostLedger();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }

    public void loadLibraryLostLedger()
    {
        try
        {
            string usercode = Convert.ToString(Session["usercode"]);
            string collegecodeNew = Convert.ToString(ddlcollege.SelectedItem.Value);
            if (ddlcollege.Items.Count > 0)
            {
                collegecodeNew = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            ddlLibFineLedger.Items.Clear();

            string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L WHERE  L.CollegeCode = " + collegecodeNew + " and L.HeaderFK in (" + Convert.ToString(ddlLibLostFineHeader.SelectedItem.Value) + ")";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlLibLostFineLedger.DataSource = ds;
                ddlLibLostFineLedger.DataTextField = "LedgerName";
                ddlLibLostFineLedger.DataValueField = "LedgerPK";
                ddlLibLostFineLedger.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }

    public void libset()
    {
        try
        {
            diclib.Add(0, "Edit Issue Date");
            diclib.Add(1, "Edit Due Date");
            diclib.Add(2, "Edit Return Date");
            diclib.Add(3, "Display Return Message");
            diclib.Add(4, "Display Issue Message");
            diclib.Add(5, "Display Fine Message");
            diclib.Add(6, "Display Toay Fine & Due Books in Issue Return");
            diclib.Add(7, "Allow Book Transaction Only if Geate In Entry");
            diclib.Add(8, "Calculate Fine in Library Holidays");
            diclib.Add(9, "Reservation Link in OPAC");
            diclib.Add(10, "Display Book Status in Trans");
            diclib.Add(11, "Use Client System Date for User In Out");
            diclib.Add(12, "Library Reservation Due");
            diclib.Add(13, "Staff Code Generation");
            diclib.Add(14, "Application Exit With Password");
            diclib.Add(15, "Automatic Card Lock");
            diclib.Add(16, "Callno Auto Increment");
            diclib.Add(17, "Fine Calculation Exclude Holidays");
            diclib.Add(18, "Due date exclude holidays");
            diclib.Add(19, "Multiple Renewal Days");
            diclib.Add(20, "Renewal Permission");
        }
        catch
        {
        }
    }

    protected void btnSaveLibrary_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet libsave = new DataSet();
            string usercode = ddluser.SelectedValue;
            string selected_userid = string.Empty;
            int selected_usercode;
            string userorgropcode = string.Empty;
            string user_gropcode = string.Empty;
            string usergroup = string.Empty;
            string strusergruop = string.Empty;
            string user_code = string.Empty;
            string activerow = "";
            string activecol = "";
            string Sql = string.Empty;
            int LibraryDetSave = 0;
            string collegeCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            for (int i = 0; i < ddluser.Items.Count; i++)
            {
                if (ddluser.Items[i].Selected == true)
                {
                    selected_userid = Convert.ToString(ddluser.Items[i].Text);
                    selected_usercode = Convert.ToInt16(ddluser.Items[i].Value);
                    if (rdb_ind.Checked == true)
                    {
                        userorgropcode = " and usercode='" + selected_usercode.ToString() + "'";
                        user_gropcode = " and user_code='" + selected_usercode.ToString() + "'";
                        usergroup = "  usercode='" + selected_usercode.ToString() + "'";
                        strusergruop = "usercode";
                        struser_gruop = "user_code";
                    }
                    else if (rdb_grp.Checked == true)
                    {
                        userorgropcode = " and group_code='" + selected_usercode.ToString() + "'";
                        user_gropcode = " and group_code='" + selected_usercode.ToString() + "'";
                        usergroup = "  group_code='" + selected_usercode.ToString() + "'";
                        strusergruop = "group_code";
                        struser_gruop = "group_code";
                    }
                    string coll_code = Convert.ToString(ddlcollege.SelectedItem.Value);
                    int LibFine = 0;
                    //Over Due Fine
                    if (ddlLibFineHeader.Items.Count > 0 && ddlLibFineLedger.Items.Count > 0)
                    {
                        string head = Convert.ToString(ddlLibFineHeader.SelectedItem.Value);
                        string led = Convert.ToString(ddlLibFineLedger.SelectedItem.Value);
                        string selValue = head + ',' + led;
                        string LibPrivDel = "delete from New_InsSettings where LinkName='LibraryFine'";
                        LibraryDetSave = dacc.update_method_wo_parameter(LibPrivDel, "text");

                        string SaveFine = "if exists (select * from New_InsSettings where LinkName='LibraryFine' " + user_gropcode + " and college_code ='" + collegeCode + "' and  LinkValue='" + selValue + "') update New_InsSettings set LinkValue ='" + selValue + "' where LinkName='LibraryFine' " + user_gropcode + "  and college_code ='" + collegeCode + "' and  LinkValue='" + selValue + "' else  insert into New_InsSettings(LinkName,LinkValue," + struser_gruop + ",college_code) values ('LibraryFine','" + selValue + "'," + selected_usercode.ToString() + ",'" + collegeCode + "')";
                        LibFine = dacc.update_method_wo_parameter(SaveFine, "text");
                    }
                    //Lost Book Fine
                    if (ddlLibFineHeader.Items.Count > 0 && ddlLibFineLedger.Items.Count > 0)
                    {
                        string Losthead = Convert.ToString(ddlLibLostFineHeader.SelectedItem.Value);
                        string Lostled = Convert.ToString(ddlLibLostFineLedger.SelectedItem.Value);
                        string selValue = Losthead + ',' + Lostled;
                        string LibPrivDel = "delete from New_InsSettings where LinkName='LostBookFine'";
                        LibraryDetSave = dacc.update_method_wo_parameter(LibPrivDel, "text");

                        string SaveFine = "if exists (select * from New_InsSettings where LinkName='LostBookFine' " + user_gropcode + " and college_code ='" + collegeCode + "' and  LinkValue='" + selValue + "') update New_InsSettings set LinkValue ='" + selValue + "' where LinkName='LostBookFine' " + user_gropcode + "  and college_code ='" + collegeCode + "' and  LinkValue='" + selValue + "' else  insert into New_InsSettings(LinkName,LinkValue," + struser_gruop + ",college_code) values ('LostBookFine','" + selValue + "'," + selected_usercode.ToString() + ",'" + collegeCode + "')";
                        LibFine = dacc.update_method_wo_parameter(SaveFine, "text");
                    }
                    if (Fpload1.Sheets[0].Rows.Count == 0)
                    {
                        lbl_aler.Text = "There is no Library Information";
                    }
                    activerow = Fpload1.ActiveSheetView.ActiveRow.ToString();
                    activecol = Fpload1.ActiveSheetView.ActiveColumn.ToString();
                    string libname = "";
                    string libcode = "";
                    for (int row = 0; row < Fpload1.Sheets[0].RowCount; row++)
                    {
                        int checkval1 = Convert.ToInt32(Fpload1.Sheets[0].Cells[row, 2].Value);
                        libname = Convert.ToString(Fpload1.Sheets[0].Cells[row, 1].Text);
                        libcode = d2.GetFunction("select lib_code  from library where lib_name='" + libname + "'");
                        if (checkval1 == 1)
                        {
                            string savelibname = "if exists (select * from lib_privileges where lib_code='" + libcode + "' " + user_gropcode + " and college_code ='" + collegeCode + "') update lib_privileges set lib_code='" + libcode + "' where college_code ='" + collegeCode + "' " + user_gropcode + "  else insert into lib_privileges(lib_code,college_code,user_code) values ('" + libcode + "','" + ddlcollege.SelectedItem.Value + "','" + selected_usercode.ToString() + "')";
                            LibraryDetSave = dacc.update_method_wo_parameter(savelibname, "text");
                        }
                        else
                        {
                            string LibPrivDel = "delete from lib_privileges where lib_code='" + libcode + "' " + user_gropcode + " and college_code ='" + ddlcollege.SelectedItem.Value + "'";
                            LibraryDetSave = dacc.update_method_wo_parameter(LibPrivDel, "text");
                        }
                    }
                    int valtype = 0;
                    if (chkeditissue.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Edit Issue Date') update inssettings set LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Edit Issue Date'  else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Edit Issue Date','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkeditdue.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Edit Due Date') update inssettings set LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Edit Due Date'  else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Edit Due Date','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkreturn.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Edit Return Date') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Edit Return Date'  else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Edit Return Date','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkdisretn.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Display Return Message') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Display Return Message'   else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Display Return Message','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkdisiss.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Display Issue Message') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Display Issue Message' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Display Issue Message','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkdisfine.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Display Fine Message') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Display Fine Message'  else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Display Fine Message','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkdistofinebok.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }

                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Display Toay Fine & Due Books in Issue Return') update inssettings set LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Display Toay Fine & Due Books in Issue Return' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Display Toay Fine & Due Books in Issue Return','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkboktran.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Allow Book Transaction Only if Geate In Entry') update inssettings set LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Allow Book Transaction Only if Geate In Entry' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Allow Book Transaction Only if Geate In Entry','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (ckcalculatefine.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Calculate Fine in Library Holidays') update inssettings set LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Calculate Fine in Library Holidays' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Calculate Fine in Library Holidays','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (reslink.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Reservation Link in OPAC') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Reservation Link in OPAC' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Reservation Link in OPAC','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (disallboksta.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Display Book Status in Trans') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Display Book Status in Trans' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Display Book Status in Trans','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkclientsys.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Use Client System Date for User In Out') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Use Client System Date for User In Out' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Use Client System Date for User In Out','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (resvadue.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Library Reservation Due') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Library Reservation Due' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Library Reservation Due','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkautmatic.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Staff Code Generation') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Staff Code Generation' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Staff Code Generation','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkpass.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Application Exit With Password') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Application Exit With Password' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Application Exit With Password','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkautomaticcardlock.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Automatic Card Lock') update inssettings set LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Automatic Card Lock' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Automatic Card Lock','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkcallno.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }

                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Callno Auto Increment') update inssettings set LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Callno Auto Increment' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Callno Auto Increment','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkfinecal.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Fine Calculation Exclude Holidays' ) update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Fine Calculation Exclude Holidays' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Fine Calculation Exclude Holidays','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkduedate.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Due date exclude holidays') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Due date exclude holidays' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Due date exclude holidays','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    int renewPermission = 0;
                    string PermissionVal = "";
                    if (chkrenewal.Checked == true)
                    {
                        PermissionVal = txtrenewal.Text;
                        if (!string.IsNullOrEmpty(PermissionVal))
                            renewPermission = Convert.ToInt32(PermissionVal);
                        valtype = 1;
                    }
                    else
                    {
                        PermissionVal = txtrenewal.Text;
                        if (!string.IsNullOrEmpty(PermissionVal))
                            renewPermission = Convert.ToInt32(PermissionVal);
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Renewal Permission') update inssettings set  LinkValue='" + valtype + "/" + renewPermission + "' where college_code ='" + collegeCode + "' and LinkName ='Renewal Permission' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Renewal Permission','" + valtype + "/" + renewPermission + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (chkmultiple.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from inssettings where college_code='" + collegeCode + "' and LinkName ='Multiple Renewal Days') update inssettings set  LinkValue='" + valtype + "' where college_code ='" + collegeCode + "' and LinkName ='Multiple Renewal Days' else insert into inssettings(college_code,LinkName,LinkValue) values('" + collegeCode + "' ,'Multiple Renewal Days','" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (cb_opacprint.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from lib_user_perm where user_code = '" + selected_usercode.ToString() + "') update lib_user_perm set mas_print='" + valtype + "' where user_code= '" + selected_usercode.ToString() + "' else insert into lib_user_perm(mas_print) values('" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (cb_specialissue.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from lib_user_perm where user_code = '" + selected_usercode.ToString() + "') update lib_user_perm set sp_issue='" + valtype + "' where user_code= '" + selected_usercode.ToString() + "' else insert into lib_user_perm(sp_issue) values('" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (cb_specialreturn.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from lib_user_perm where user_code = '" + selected_usercode.ToString() + "') update lib_user_perm set sp_return='" + valtype + "' where user_code= '" + selected_usercode.ToString() + "' else insert into lib_user_perm(sp_return) values('" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (cb_reservation_print.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from lib_user_perm where user_code = '" + selected_usercode.ToString() + "') update lib_user_perm set res_print='" + valtype + "' where user_code= '" + selected_usercode.ToString() + "' else insert into lib_user_perm(res_print) values('" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (cb_newrequest_print.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from lib_user_perm where user_code = '" + selected_usercode.ToString() + "') update lib_user_perm set req_print='" + valtype + "' where user_code= '" + selected_usercode.ToString() + "' else insert into lib_user_perm(req_print) values('" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (cb_reservation_delete.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from lib_user_perm where user_code = '" + selected_usercode.ToString() + "') update lib_user_perm set res_dele='" + valtype + "' where user_code= '" + selected_usercode.ToString() + "' else insert into lib_user_perm(res_dele) values('" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (cb_newrequest_delete.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from lib_user_perm where user_code = '" + selected_usercode.ToString() + "') update lib_user_perm set req_dele='" + valtype + "' where user_code= '" + selected_usercode.ToString() + "' else insert into lib_user_perm(req_dele) values('" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (cb_manualcallnoentry.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from lib_user_perm where user_code = '" + selected_usercode.ToString() + "') update lib_user_perm set call_entry='" + valtype + "' where user_code= '" + selected_usercode.ToString() + "' else insert into lib_user_perm(call_entry) values('" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (cb_editfine.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from lib_user_perm where user_code = '" + selected_usercode.ToString() + "') update lib_user_perm set sp_fine='" + valtype + "' where user_code= '" + selected_usercode.ToString() + "' else insert into lib_user_perm(sp_fine) values('" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (cb_transactionwithbarcode.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from lib_user_perm where user_code = '" + selected_usercode.ToString() + "') update lib_user_perm set BarCodeTrans='" + valtype + "' where user_code= '" + selected_usercode.ToString() + "' else insert into lib_user_perm(BarCodeTrans) values('" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");

                    if (cb_editcancelfine.Checked == true)
                    {
                        valtype = 1;
                    }
                    else
                    {
                        valtype = 0;
                    }
                    Sql = "if exists (select * from lib_user_perm where user_code = '" + selected_usercode.ToString() + "') update lib_user_perm set Cancel_Fine='" + valtype + "' where user_code= '" + selected_usercode.ToString() + "' else insert into lib_user_perm(Cancel_Fine) values('" + valtype + "')";
                    LibraryDetSave = dacc.update_method_wo_parameter(Sql, "text");
                }
            }
            if (LibraryDetSave > 0)
            {
                imagalt.Visible = true;
                lbl_aler.Text = "Saved successfully";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }

    protected void ddlLibFineHeader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadLibraryLedger();

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }

    protected void ddlLibLostFineHeader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadLibraryLostLedger();
            loadLibraryLedger();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "NewSecuritySettings");
        }
    }

    protected void librights()
    {
        DataSet rigbok = new DataSet();
        string lib = string.Empty;
        string permission = string.Empty;
        int sno = 0;
        string usergroup = "";
        string collegecode = ddlcollege.SelectedItem.Value;
        try
        {
            divtable.Visible = true;
            for (int i = 0; i < ddluser.Items.Count; i++)
            {
                if (ddluser.Items[i].Selected == true)
                {
                    selected_usercode = Convert.ToInt16(ddluser.Items[i].Value);
                    usergroup = "user_code='" + selected_usercode.ToString() + "'";
                }
            }
            string Sql = "select lib_name,lib_code from library where college_code='" + collegecode + "'";
            rigbok.Clear();
            rigbok = d2.select_method_wo_parameter(Sql, "Text");
            libset();
            loadspreadlib(ds);
            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            chk.AutoPostBack = true;
            DataSet dsLib = new DataSet();
            string LibValue = "";
            //OverDueFine
            Sql = "select * from New_InsSettings where LinkName='LibraryFine' and college_code='" + ddlcollege.SelectedItem.Value + "' ";
            DataSet dsprint = d2.select_method_wo_parameter(Sql, "text");
            string lid = string.Empty;
            string hid = string.Empty;
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                string[] linkval = Convert.ToString(dsprint.Tables[0].Rows[0]["LinkValue"]).Split(',');
                hid = linkval[0];
                ddlLibFineHeader.ClearSelection();
                ddlLibFineHeader.Items.FindByValue(hid.Trim()).Selected = true;
                loadLibraryLedger();
            }
            //LostBookFine
            Sql = "select * from New_InsSettings where LinkName='LostBookFine' and college_code='" + ddlcollege.SelectedItem.Value + "' ";
            DataSet dsLostBook = d2.select_method_wo_parameter(Sql, "text");
            if (dsLostBook.Tables[0].Rows.Count > 0)
            {
                string[] linkval = Convert.ToString(dsLostBook.Tables[0].Rows[0]["LinkValue"]).Split(',');
                hid = linkval[0];
                ddlLibLostFineHeader.ClearSelection();
                ddlLibLostFineHeader.Items.FindByValue(hid.Trim()).Selected = true;
                loadLibraryLostLedger();
                loadLibraryLedger();
            }

            if (rigbok.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < rigbok.Tables[0].Rows.Count; j++)
                {
                    Fpload1.Sheets[0].RowCount++;
                    sno++;
                    lib = Convert.ToString(rigbok.Tables[0].Rows[j]["lib_name"]).Trim();
                    string licode = Convert.ToString(rigbok.Tables[0].Rows[j]["lib_code"]).Trim();

                    string libexit = "select lib_code from lib_privileges where  " + usergroup + " and lib_code='" + licode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'";
                    dsLib = d2.select_method_wo_parameter(libexit, "text");

                    if (dsLib.Tables[0].Rows.Count > 0)
                    {
                        LibValue = Convert.ToString(dsLib.Tables[0].Rows[0]["lib_code"]);
                    }
                    else
                    {
                        LibValue = "0";
                    }
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].CellType = chk;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].Text = lib;
                    if (string.IsNullOrEmpty(LibValue) || LibValue == "0")
                    {
                        Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].Text = permission;
                        Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].Value = 0;
                    }
                    else
                    {
                        Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].Text = permission;
                        Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].Value = 1;
                    }
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Columns[0].Width = 70;
                    Fpload1.Sheets[0].Columns[1].Width = 280;
                    Fpload1.Sheets[0].Columns[2].Width = 250;
                }
                Fpload1.Sheets[0].PageSize = Fpload1.Sheets[0].RowCount;
                Fpload1.Width = 620;
                Fpload1.Height = 200;
                Fpload1.SaveChanges();
            }
            string renewalPer = "";
            if (diclib.Values.Count > 0)
            {
                for (int k = 0; k < diclib.Values.Count; k++)
                {
                    string libraryma = diclib[k];
                    string libmaste = "select LinkValue from inssettings where college_code='" + collegecode + "' and LinkName ='" + libraryma + "' ";
                    dsLib = d2.select_method_wo_parameter(libmaste, "text");
                    if (dsLib.Tables[0].Rows.Count > 0)
                    {
                        LibValue = Convert.ToString(dsLib.Tables[0].Rows[0]["LinkValue"]);
                        if (LibValue.Contains('/'))
                        {
                            string[] SpLibVal = LibValue.Split('/');
                            LibValue = SpLibVal[0];
                            renewalPer = SpLibVal[1];

                        }
                    }
                    if (LibValue == "1")
                    {
                        if (k == 0)
                        {
                            chkeditissue.Checked = true;
                        }
                        else if (k == 1)
                        {
                            chkeditdue.Checked = true;
                        }
                        else if (k == 2)
                        {
                            chkreturn.Checked = true;
                        }
                        else if (k == 3)
                        {
                            chkdisretn.Checked = true;
                        }
                        else if (k == 4)
                        {
                            chkdisiss.Checked = true;
                        }
                        else if (k == 5)
                        {
                            chkdisfine.Checked = true;
                        }
                        else if (k == 6)
                        {
                            chkdistofinebok.Checked = true;
                        }
                        else if (k == 7)
                        {
                            chkboktran.Checked = true;
                        }
                        else if (k == 8)
                        {
                            ckcalculatefine.Checked = true;
                        }
                        else if (k == 9)
                        {
                            reslink.Checked = true;
                        }
                        else if (k == 10)
                        {
                            disallboksta.Checked = true;
                        }
                        else if (k == 11)
                        {
                            chkclientsys.Checked = true;
                        }
                        else if (k == 12)
                        {
                            resvadue.Checked = true;
                        }
                        else if (k == 13)
                        {
                            chkautmatic.Checked = true;
                        }
                        else if (k == 14)
                        {
                            chkpass.Checked = true;
                        }
                        else if (k == 15)
                        {
                            chkautomaticcardlock.Checked = true;
                        }
                        else if (k == 16)
                        {
                            chkcallno.Checked = true;
                        }
                        else if (k == 17)
                        {
                            chkfinecal.Checked = true;
                        }
                        else if (k == 18)
                        {
                            chkduedate.Checked = true;
                        }
                        else if (k == 19)
                        {
                            chkmultiple.Checked = true;
                        }
                        else if (k == 20)
                        {
                            chkrenewal.Checked = true;
                            txtrenewal.Visible = true;
                            txtrenewal.Text = renewalPer;
                        }
                    }
                    else
                    {
                        if (k == 0)
                        {
                            chkeditissue.Checked = false;
                        }
                        else if (k == 1)
                        {
                            chkeditdue.Checked = false;
                        }
                        else if (k == 2)
                        {
                            chkreturn.Checked = false;
                        }
                        else if (k == 3)
                        {
                            chkdisretn.Checked = false;
                        }
                        else if (k == 4)
                        {
                            chkdisiss.Checked = false;
                        }
                        else if (k == 5)
                        {
                            chkdisfine.Checked = false;
                        }
                        else if (k == 6)
                        {
                            chkdistofinebok.Checked = false;
                        }
                        else if (k == 7)
                        {
                            chkboktran.Checked = false;
                        }
                        else if (k == 8)
                        {
                            ckcalculatefine.Checked = false;
                        }
                        else if (k == 9)
                        {
                            reslink.Checked = false;
                        }
                        else if (k == 10)
                        {
                            disallboksta.Checked = false;
                        }
                        else if (k == 11)
                        {
                            chkclientsys.Checked = false;
                        }
                        else if (k == 12)
                        {
                            resvadue.Checked = false;
                        }
                        else if (k == 13)
                        {
                            chkautmatic.Checked = false;
                        }
                        else if (k == 14)
                        {
                            chkpass.Checked = false;
                        }
                        else if (k == 15)
                        {
                            chkautomaticcardlock.Checked = false;
                        }
                        else if (k == 16)
                        {
                            chkcallno.Checked = false;
                        }
                        else if (k == 17)
                        {
                            chkfinecal.Checked = false;
                        }
                        else if (k == 18)
                        {
                            chkduedate.Checked = false;
                        }
                        else if (k == 19)
                        {
                            chkmultiple.Checked = false;
                        }
                        else if (k == 20)
                        {
                            chkrenewal.Checked = false;
                            txtrenewal.Visible = false;
                        }
                    }
                }
            }
            string selQry = "select linkvalue from inssettings where college_code='" + collegecode + "' and LinkName='Renewal Permission'";
            dsLib = d2.select_method_wo_parameter(selQry, "text");
            if (dsLib.Tables[0].Rows.Count > 0)
            {
                string Link = Convert.ToString(dsLib.Tables[0].Rows[0]["linkvalue"]);
                string[] linVal = Link.Split('/');
                txtrenewal.Text = linVal[1];
            }
        }
        catch
        {
        }
    }

    private void loadspreadlib(DataSet ds)
    {
        try
        {
            Fpload1.Visible = true;
            divtable.Visible = true;
            Fpload1.Sheets[0].RowCount = 0;
            Fpload1.Sheets[0].ColumnCount = 3;
            Fpload1.CommandBar.Visible = false;
            Fpload1.Sheets[0].AutoPostBack = false;
            Fpload1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpload1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Fpload1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = false;
            Fpload1.Sheets[0].Columns[0].Width = 70;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Library";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = false;
            Fpload1.Sheets[0].Columns[1].Width = 280;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Permission";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].Locked = false;
            Fpload1.Sheets[0].Columns[2].Width = 250;
        }
        catch (Exception ex)
        { }
    }

    protected void chkrenewal_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkrenewal.Checked == true)
        {
            txtrenewal.Visible = true;
        }
        else
            txtrenewal.Visible = false;
    }

    #endregion

    #region Mobile App
    //Deepali 16.7.18  Staff App
    protected void loadExistingMblAppTabRights()
    {
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
            string usercode = ddluser.SelectedValue;
            string linkValue = "";
            if (usercode != "")
            {
                //string linkValue = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Mobile_App_Tab_Rights' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'");

                string sqlstr = "select LinkValue from New_InsSettings where LinkName='Mobile_App_Tab_Rights' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'";
                con.Close();
                con.Open();
                SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
                SqlDataReader drnew;
                SqlCommand funcmd = new SqlCommand(sqlstr);
                funcmd.Connection = con;
                drnew = funcmd.ExecuteReader();
                drnew.Read();
                if (drnew.HasRows == true)
                {
                    linkValue = drnew[0].ToString();
                }
                else
                {
                    linkValue = "";
                }

                string[] arr = linkValue.Split(',');

                if (arr.Length == 31)
                {
                    for (int i = 0; i < cbl_appTab.Items.Count; i++)
                        cbl_appTab.Items[i].Selected = true;
                    cb_appTab.Checked = true;
                }
                else
                {
                    if (linkValue != "")
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            int val = Convert.ToInt32(arr[i]);
                            cbl_appTab.Items[val].Selected = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < cbl_appTab.Items.Count; i++)
                            cbl_appTab.Items[i].Selected = false;
                        cb_appTab.Checked = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_appTab.Items.Count; i++)
                    cbl_appTab.Items[i].Selected = false;
                cb_appTab.Checked = false;
            }
        }
        catch { }
    }
    protected void cbl_appTab_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_appTab, cbl_appTab, sampleTxt, "Tab", "--Select--");
    }
    protected void btnMblAppSave_Click(object sender, EventArgs e)
    {
        try
        {
            int ii = 0;
            for (int i = 0; i < ddluser.Items.Count; i++)
            {
                if (ddluser.Items[i].Selected == true)
                {
                    string usercode = Convert.ToString(ddluser.Items[i].Value);
                    string linkValue = "";
                    //Staff App tab rights
                    if (cb_appTab.Checked)
                        linkValue = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30";
                    else
                    {
                        for (int ia = 0; ia < cbl_appTab.Items.Count; ia++)
                        {
                            if (cbl_appTab.Items[ia].Selected)
                            {
                                if (linkValue == "")
                                    linkValue = Convert.ToString(ia);
                                else
                                    linkValue = linkValue + "," + Convert.ToString(ia);
                            }
                        }
                    }

                    string sql = "if exists (select * from New_InsSettings where LinkName='Mobile_App_Tab_Rights' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + linkValue + "' where LinkName='Mobile_App_Tab_Rights' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Mobile_App_Tab_Rights','" + linkValue + "','" + usercode + "','" + ddlcollege.SelectedItem.Value + "')";
                    int status = dacc.update_method_wo_parameter(sql, "Text");
                    if (status > 0)
                        ii = ii + 1;
                    //Student App tab rights
                    linkValue = "";
                    if (cb_Student_AppTab.Checked)
                        linkValue = "0,1,2,3,4,5,6,7,8,9,10,11,12";
                    else
                    {
                        for (int ib = 0; ib < cbl_Student_AppTab.Items.Count; ib++)
                        {
                            if (cbl_Student_AppTab.Items[ib].Selected)
                            {
                                if (linkValue == "")
                                    linkValue = Convert.ToString(ib);
                                else
                                    linkValue = linkValue + "," + Convert.ToString(ib);
                            }
                        }
                    }

                    sql = "if exists (select * from New_InsSettings where LinkName='Student_Mobile_App_Tab_Rights' and user_code =30 and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + linkValue + "' where LinkName='Student_Mobile_App_Tab_Rights' and user_code =30 and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Student_Mobile_App_Tab_Rights','" + linkValue + "','30','" + ddlcollege.SelectedItem.Value + "')";
                    status = dacc.update_method_wo_parameter(sql, "Text");
                    if (status > 0)
                        ii = ii + 1;
                }
            }
            if (ii > 0)
                lbl_alert.Text = "Saved Successfully";
            else
                lbl_alert.Text = "Saving Failed";
            imgAlert.Visible = true;
        }
        catch
        {

        }
    }

    //Deepali 28.8.18  Student App
    protected void loadExistingStudentMblAppTabRights()
    {
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
            string usercode = ddluser.SelectedValue;
            string linkValue = "";
            if (usercode != "")
            {
                string sqlstr = "select LinkValue from New_InsSettings where LinkName='Student_Mobile_App_Tab_Rights' and user_code ='30' and college_code ='" + ddlcollege.SelectedItem.Value + "'";
                con.Close();
                con.Open();
                SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
                SqlDataReader drnew;
                SqlCommand funcmd = new SqlCommand(sqlstr);
                funcmd.Connection = con;
                drnew = funcmd.ExecuteReader();
                drnew.Read();
                if (drnew.HasRows == true)
                {
                    linkValue = drnew[0].ToString();
                }
                else
                {
                    linkValue = "";
                }

                string[] arr = linkValue.Split(',');

                if (arr.Length == 14)
                {
                    for (int i = 0; i < cbl_Student_AppTab.Items.Count; i++)
                        cbl_Student_AppTab.Items[i].Selected = true;
                    cb_Student_AppTab.Checked = true;
                }
                else
                {
                    if (linkValue != "")
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            int val = Convert.ToInt32(arr[i]);
                            cbl_Student_AppTab.Items[val].Selected = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < cbl_Student_AppTab.Items.Count; i++)
                            cbl_Student_AppTab.Items[i].Selected = false;
                        cb_Student_AppTab.Checked = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_Student_AppTab.Items.Count; i++)
                    cbl_Student_AppTab.Items[i].Selected = false;
                cb_Student_AppTab.Checked = false;
            }
        }
        catch { }
    }
    protected void cbl_Student_AppTab_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_Student_AppTab, cbl_Student_AppTab, sampleTxt, "Tab", "--Select--");
    }

    protected void loadExisting_FN_AN()
    {
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
            string usercode = ddluser.SelectedValue;
            string linkValue = "";
            if (usercode != "")
            {

                string sqlstr = "select LinkValue from New_InsSettings where LinkName='Student_App_Att_SelHour' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'";
                con.Close();
                con.Open();
                SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
                SqlDataReader drnew;
                SqlCommand funcmd = new SqlCommand(sqlstr);
                funcmd.Connection = con;
                drnew = funcmd.ExecuteReader();
                drnew.Read();
                if (drnew.HasRows == true)
                {
                    linkValue = drnew[0].ToString();
                }
                else
                {
                    linkValue = "";
                }

                string[] arr = linkValue.Split(',');
                int k = 0;
                if (linkValue != "")
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        k++;
                        int val = Convert.ToInt32(arr[i]);
                        cbl_FN_AN.Items[val - 1].Selected = true;
                    }
                    if (k == cbl_FN_AN.Items.Count)
                        cb_FN_AN.Checked = true;
                }
                else
                {
                    for (int i = 0; i < cbl_FN_AN.Items.Count; i++)
                        cbl_FN_AN.Items[i].Selected = false;
                    cb_FN_AN.Checked = false;
                }

            }
            else
            {
                for (int i = 0; i < cbl_FN_AN.Items.Count; i++)
                    cbl_FN_AN.Items[i].Selected = false;
                cb_FN_AN.Checked = false;
            }
        }
        catch { }
    }

    public void bind_FN_AN_Hour()
    {
        try
        {
            txt_FN_AN.Text = "---Select---";
            cb_FN_AN.Checked = false;
            cbl_FN_AN.Items.Clear();

            string noOfHrs = d2.GetFunction("select max (No_of_hrs_per_day)No_of_hrs_per_day from PeriodAttndSchedule");
            int totHrs = 0;
            int.TryParse(noOfHrs, out totHrs);
            for (int i = 1; i <= totHrs; i++)
            {
                cbl_FN_AN.Items.Add(i.ToString());
            }

            CallCheckboxListChange(cb_FN_AN, cbl_FN_AN, txt_FN_AN, "Hour", "--Select--");
        }
        catch (Exception ex)
        {

        }


    }

    protected void cbl_FN_AN_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_FN_AN, cbl_FN_AN, txt_FN_AN, "Hour", "--Select--");
    }

    protected void btnStu_app_Click(object sender, EventArgs e)
    {
        string usercode = ddluser.SelectedValue;
        try
        {
            string linkValue = "";

            for (int i = 0; i < cbl_FN_AN.Items.Count; i++)
            {
                if (cbl_FN_AN.Items[i].Selected)
                {
                    if (linkValue == "")
                        linkValue = Convert.ToString(i + 1);
                    else
                        linkValue = linkValue + "," + Convert.ToString(i + 1);
                }

            }
            string sql = "if exists (select * from New_InsSettings where LinkName='Student_App_Att_SelHour' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + linkValue + "' where LinkName='Student_App_Att_SelHour' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Student_App_Att_SelHour','" + linkValue + "','" + usercode + "','" + ddlcollege.SelectedItem.Value + "')";
            int status = dacc.update_method_wo_parameter(sql, "Text");


            //Show fee due setting
            linkValue = "";

            if (rbList_DueFeeMode.SelectedIndex == 0)
                linkValue = "0";//Header
            else if (rbList_DueFeeMode.SelectedIndex == 1)
                linkValue = "1";//Ledger
            if (rbList_DueFeeMode.SelectedIndex == 2)
                linkValue = "2";//Both

            sql = "if exists (select * from New_InsSettings where LinkName='Student_App_DueFeeBy' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + linkValue + "' where LinkName='Student_App_DueFeeBy' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Student_App_DueFeeBy','" + linkValue + "','" + usercode + "','" + ddlcollege.SelectedItem.Value + "')";
            status = dacc.update_method_wo_parameter(sql, "Text");
            //==============

            if (status > 0)
                lbl_alert.Text = "Saved Successfully";
            else
                lbl_alert.Text = "Saving Failed";
            imgAlert.Visible = true;
        }
        catch
        {
            if (usercode == "")
            {
                lbl_alert.Text = "Select User";
                imgAlert.Visible = true;
            }
        }
    }

    protected void loadExisting_DueFee()
    {
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
            string usercode = ddluser.SelectedValue;
            string linkValue = "";
            if (usercode != "")
            {

                string sqlstr = "select LinkValue from New_InsSettings where LinkName='Student_App_DueFeeBy' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'";
                con.Close();
                con.Open();
                SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
                SqlDataReader drnew;
                SqlCommand funcmd = new SqlCommand(sqlstr);
                funcmd.Connection = con;
                drnew = funcmd.ExecuteReader();
                drnew.Read();
                if (drnew.HasRows == true)
                {
                    linkValue = drnew[0].ToString();
                }
                else
                {
                    linkValue = "";
                }

                if (linkValue != "")
                {
                    rbList_DueFeeMode.SelectedValue = linkValue;
                }
                else
                {
                    rbList_DueFeeMode.SelectedValue = "2";
                }

            }
            else
            {
                rbList_DueFeeMode.SelectedValue = "2";
            }
        }
        catch { }
    }
    #endregion

    #region finance
    public void loadheaderandledger()
    {
        try
        {
            #region CheckBox List Load

            #endregion

            #region single selection header
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            string usercode = ddluser.SelectedValue;
            ddlheader.Items.Clear();
            ds.Clear();
            string Query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + clgvalue + "  ";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlheader.DataSource = ds;
                ddlheader.DataTextField = "HeaderName";
                ddlheader.DataValueField = "HeaderPK";
                ddlheader.DataBind();
            }
            #endregion
        }
        catch
        {
        }
    }
    public void loadcollege()
    {
        try
        {
            ddlcollegename.Items.Clear();
            ds.Clear();
            string Query = "select cp.college_code,cf.Coll_acronymn from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollegename.DataSource = ds;
                ddlcollegename.DataTextField = "Coll_acronymn";
                ddlcollegename.DataValueField = "college_code";
                ddlcollegename.DataBind();
            }
        }
        catch
        { }
    }
    #endregion

    //SD_23/2019
    #region HR
    public void ReturnSettingDetails()
    {
        try
        {
            string LinkValue = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='staff Permission' and college_code ='" + ddlcollege.SelectedItem.Value + "'");
            if (LinkValue != "" && LinkValue != "0")
                txtExtentPermission.Text = LinkValue;

        }
        catch
        {

        }

    }
    #endregion

    public void ledgerload()
    {
        try
        {
            #region single selection header
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            string hed = ddlheader.SelectedItem.Value.ToString();
            string usercode = ddluser.SelectedValue;
            ddlledger.Items.Clear();
            ds.Clear();
            string Query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + clgvalue + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlledger.DataSource = ds;
                ddlledger.DataTextField = "LedgerName";
                ddlledger.DataValueField = "LedgerPK";
                ddlledger.DataBind();
            }
            #endregion
        }
        catch
        {
        }
    }

    protected void ddlcollegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadheaderandledger();
        ledgerload();
    }

    protected void ddl_header_SelectedIndexChanged(object sender, EventArgs e)
    {
        string header = string.Empty;
        if (ddlheader.Items.Count > 0)
        {
            header = Convert.ToString(ddlheader.SelectedItem.Value);
        }

        ledgerload();

    }

    protected void btnChlSave_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            string chlDate = Convert.ToString(txtdop.Text);
            string collCode = Convert.ToString(ddlcollege.SelectedValue);
            if (!string.IsNullOrEmpty(chlDate) && !string.IsNullOrEmpty(collCode))
            {

                string insertQ = "if Exists (select * from master_settings where settings='challan last date' and template='" + collCode + "') Update master_settings set value='" + chlDate + "' where settings='challan last date' and template='" + collCode + "' else insert into master_settings(settings,value,template) values ('challan last date','" + chlDate + "','" + collCode + "')";
                count = dir.updateData(insertQ);

            }
            if (count != 0)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Saved successfully";
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Not Saved";
            }
        }
        catch
        {
        }
    }

    protected void btnSaveSet_Click(object sender, EventArgs e)
    {
        int checkvalue = 0;
        int save1 = 0;
        if (Year.Checked == true)
        {
            checkvalue = 1;
            string usercode = ddluser.SelectedValue;
            string insqry1 = "if exists (select * from New_InsSettings where LinkName='YearwiseSetting' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + checkvalue + "' where LinkName='YearwiseSetting' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('YearwiseSetting','" + checkvalue + "','" + usercode + "','" + ddlcollege.SelectedItem.Value + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");

        }
        if (Semester.Checked == true)
        {
            checkvalue = 1;
            string usercode = ddluser.SelectedValue;
            string insqry1 = "if exists (select * from New_InsSettings where LinkName='SemesterWiseSetting' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + checkvalue + "' where LinkName='SemesterWiseSetting' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('SemesterWiseSetting','" + checkvalue + "','" + usercode + "','" + ddlcollege.SelectedItem.Value + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");

        }
        if (save1 == 1)
        {
            imgAlert.Visible = true;

            lbl_alert.Text = "Saved Successfully";
        }
        else
        {
            imgAlert.Visible = true;

            lbl_alert.Text = "Not Saved";
        }

    }

    #region finance online additional fees setting added by abarna
    protected void cbhdOnline_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbhdOnline, cblhdOnline, txthdOnline, "Header", "--Select--");
        loadOnlineLedgers();
    }

    protected void cblhdOnline_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbhdOnline, cblhdOnline, txthdOnline, "Header", "--Select--");
        loadOnlineLedgers();
    }

    protected void cbedgOnline_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbedgOnline, cbledgOnline, txtldOnline, "Ledger", "--Select--");
    }

    protected void cbledgOnline_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbedgOnline, cbledgOnline, txtldOnline, "Ledger", "--Select--");
    }


    protected void btnsaveOnline_Click(object sender, EventArgs e)
    {
        try
        {
            bool boolFalse = false;
            //string finYear = Convert.ToString(ddlfinOnline.SelectedValue);
            string collegeCode = ddlcollege.Items.Count > 0 ? ddlcollege.SelectedValue : "13";
            //if (!string.IsNullOrEmpty(finYear))
            //{
            Hashtable htHeader = htheader(collegeCode);
            for (int hdrI = 0; hdrI < cblhdOnline.Items.Count; hdrI++)
            {
                if (!cblhdOnline.Items[hdrI].Selected)
                    continue;
                string hdFK = Convert.ToString(cblhdOnline.Items[hdrI].Value);
                for (int hdriI = 0; hdriI < cbledgOnline.Items.Count; hdriI++)
                {
                    if (!cbledgOnline.Items[hdriI].Selected)
                        continue;
                    string ldFK = Convert.ToString(cbledgOnline.Items[hdriI].Value);
                    hdFK = Convert.ToString(htHeader[ldFK]);
                    for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                    {
                        if (!cbl_sem.Items[sem].Selected)
                            continue;
                        string semster = Convert.ToString(cbl_sem.Items[sem].Value);

                        string insQ = " if exists(select * from tbl_OnlineSettingsaddfees where onlineSetting_CollegeCode='" + collegeCode + "' and onlineSetting_Semester='" + semster + "' and onlineSetting_HeaderFK='" + hdFK + "' and onlineSetting_LedgerFK='" + ldFK + "') delete from  tbl_OnlineSettingsaddfees where onlineSetting_CollegeCode='" + collegeCode + "' and onlineSetting_Semester='" + semster + "' and onlineSetting_HeaderFK='" + hdFK + "' and onlineSetting_LedgerFK='" + ldFK + "' insert into tbl_OnlineSettingsaddfees(onlineSetting_Semester,onlineSetting_HeaderFK,onlineSetting_LedgerFK,onlineSetting_CollegeCode) values('" + semster + "','" + hdFK + "','" + ldFK + "','" + collegeCode + "')";
                        //update tbl_OnlineSettings set onlineSetting_FinYearFk='" + finYear + "',onlineSetting_HeaderFK='" + hdFK + "',onlineSetting_LedgerFK='" + ldFK + "' where onlineSetting_CollegeCode='" + collegeCode + "'
                        int upd = d2.update_method_wo_parameter(insQ, "Text");
                        boolFalse = true;
                    }
                }
            }
            //}
            if (boolFalse)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            }
        }
        catch { }
    }

    protected void cb_sem_CheckedChanged(object sender, EventArgs e)
    {

        //CallCheckBoxChangedEvent(cbl_sem, cb_sem, txt_sem, name);
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
    }
    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
        //CallCheckBoxListChangedEvent(cbl_sem, cb_sem, txt_sem, name);
    }
    protected void loadsem()
    {
        try
        {
            // string featDegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            cbl_sem.Items.Clear();
            txt_sem.Text = "--Select--";
            cb_sem.Checked = false;
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            string usercode = ddluser.SelectedValue;
            //  d2.featDegreeCode = featDegcode;
            ds = d2.loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "TextVal";
                cbl_sem.DataValueField = "TextCode";
                cbl_sem.DataBind();

                if (cbl_sem.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                        cbltext = Convert.ToString(cbl_sem.Items[i].Text);
                    }
                    if (cbl_sem.Items.Count == 1)
                        txt_sem.Text = "" + linkName + "(" + cbltext + ")";
                    else
                        txt_sem.Text = "" + linkName + "(" + cbl_sem.Items.Count + ")";
                    cb_sem.Checked = true;
                }
            }
        }
        catch { }
    }
    protected Hashtable htheader(string collegecode)
    {
        Hashtable htDept = new Hashtable();
        try
        {
            string selQ = " select headerpk,ledgerpk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and h.collegecode='" + collegecode + "'";
            DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    string deptcode = Convert.ToString(dsVal.Tables[0].Rows[row]["ledgerpk"]);
                    string courseId = Convert.ToString(dsVal.Tables[0].Rows[row]["headerpk"]);
                    if (!htDept.ContainsKey(deptcode))
                    {
                        htDept.Add(deptcode, courseId);
                    }
                }
            }
        }
        catch { }
        return htDept;
    }

    #region bind values



    private void loadOnlineHeaders()
    {
        try
        {
            string collegeCode = ddlcollege.Items.Count > 0 ? ddlcollege.SelectedValue : "13";
            cbhdOnline.Checked = false;
            cblhdOnline.Items.Clear();
            txthdOnline.Text = "--Select--";
            DataSet dsHdrs = d2.select_method_wo_parameter("select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegeCode + "'", "TEXT");
            if (dsHdrs.Tables.Count > 0 && dsHdrs.Tables[0].Rows.Count > 0)
            {
                cblhdOnline.DataSource = dsHdrs.Tables[0];
                cblhdOnline.DataTextField = "HeaderName";
                cblhdOnline.DataValueField = "HeaderPK";
                cblhdOnline.DataBind();
                for (int hdrI = 0; hdrI < cblhdOnline.Items.Count; hdrI++)
                {
                    cblhdOnline.Items[hdrI].Selected = true;
                }
                cbhdOnline.Checked = true;
                txthdOnline.Text = "Header(" + cblhdOnline.Items.Count + ")";
                loadOnlineLedgers();
            }
        }
        catch { }
    }

    private void loadOnlineLedgers()
    {
        try
        {
            cbedgOnline.Checked = false;
            cbledgOnline.Items.Clear();
            txtldOnline.Text = "--Select--";

            string collegeCode = ddlcollege.Items.Count > 0 ? ddlcollege.SelectedValue : "13";
            StringBuilder headerCodes = new StringBuilder();
            for (int hdrI = 0; hdrI < cblhdOnline.Items.Count; hdrI++)
            {
                if (cblhdOnline.Items[hdrI].Selected)
                {
                    headerCodes.Append(cblhdOnline.Items[hdrI].Value + ",");
                }
            }
            if (headerCodes.Length > 1)
            {
                headerCodes.Remove(headerCodes.Length - 1, 1);

                DataSet dsLgrs = d2.select_method_wo_parameter("select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode ='" + collegeCode + "' and HeaderFk in (" + headerCodes.ToString() + ")", "TEXT");
                if (dsLgrs.Tables.Count > 0 && dsLgrs.Tables[0].Rows.Count > 0)
                {
                    cbledgOnline.DataSource = dsLgrs.Tables[0];
                    cbledgOnline.DataTextField = "LedgerName";
                    cbledgOnline.DataValueField = "LedgerPK";
                    cbledgOnline.DataBind();
                    for (int hdrI = 0; hdrI < cbledgOnline.Items.Count; hdrI++)
                    {
                        cbledgOnline.Items[hdrI].Selected = true;
                    }
                    cbedgOnline.Checked = true;
                    txtldOnline.Text = "Ledger(" + cbledgOnline.Items.Count + ")";
                }
            }
        }
        catch { }
    }

    #endregion
    #endregion

    private void cellchkchange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }

    private void cellcbchange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {

            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    protected void cblbatyr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            cellcbchange(cbbatyr, cblbatyr, txtbatyr, cbatndbatyr.Text, "---Select---");

        }
        catch
        {
        }
    }

    protected void cbbatyr_CheckedChanged(object sender, EventArgs e)
    {
        try
        {


            cellchkchange(cbbatyr, cblbatyr, txtbatyr, cbatndbatyr.Text, "---Select---");

        }
        catch
        {
        }
    }

    protected void cbatndbatyr_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbatndbatyr.Checked == true)
        {
            txtbatyr.Enabled = true;

        }
        else
        {
            txtbatyr.Enabled = false;
        }

    }

    //Added By Saranyadevi 21.12.2018

    protected void btnsave_InvigilatorTravel_Click(object sender, EventArgs e)
    {
        try
        {
            string minkm = Text_minkm.Text;
            string minamt = Text_minAmt.Text;
            string perkm = Text_Perkm.Text;
            string peramt = Text_peramt.Text;
            string userorgropcode = string.Empty;
            string selected_userid = string.Empty;
            string strusergruop = string.Empty;



            if (minkm == "")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Enter Minimun Km!";
                return;
            }
            if (minamt == "")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Enter Minimun Km Amount!";
                return;
            }
            if (perkm == "")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Enter Per Km!";
                return;
            }
            if (peramt == "")
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Enter Per Km Amount!";
                return;
            }
            string collcode = ddlcollege.SelectedItem.Value;
            int insset = 0;
            if (collcode != "")
            {
                string insqry = "if not exists(select * from Invigilator_Travel_setting where college_code='" + collcode + "') insert into Invigilator_Travel_setting(college_code,Min_Kilometer,min_Amount,Per_Kilometer,Per_Amount) Values('" + collcode + "','" + minkm + "','" + minamt + "','" + perkm + "','" + peramt + "') else update Invigilator_Travel_setting set Min_Kilometer='" + minkm + "',min_Amount='" + minamt + "',Per_Kilometer='" + perkm + "',Per_Amount='" + peramt + "' where college_code='" + collcode + "'";
                insset = d2.update_method_wo_parameter(insqry, "Text");

                if (insset > 0)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Saved successfully";
                }
            }

        }
        catch
        {


        }

    }
}
