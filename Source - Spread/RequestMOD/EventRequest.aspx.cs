using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.IO;
using System.Threading;


public partial class EventRequest : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable hat = new Hashtable();
    Hashtable ht = new Hashtable();
    DataTable dt = new DataTable();
    DataTable dt2 = new DataTable();
    DAccess2 da = new DAccess2();

    static string particpentstaff = "";
    static string persentedstaff = "";
    static string particpentstud = "";
    static string persentedstud = "";
    static string particpentindi = "";
    static string particpentcomp = "";
    static string persentedindi = "";
    static string persentedcomp = "";
    static string prscompany = "";
    static string partcompany = "";

    static DataSet dsnew = new DataSet();
    string college = "";
    static Hashtable newparticipant = new Hashtable();
    static Hashtable newpresented = new Hashtable();
    static Hashtable eventhash = new Hashtable();
    static Hashtable actionhash = new Hashtable();
    static Hashtable singlepresentindi = new Hashtable();
    static Hashtable singlepresentcomp = new Hashtable();
    static Hashtable singleparticindi = new Hashtable();
    static Hashtable singleparticcomp = new Hashtable();
    static int ii;
    static int jj;
    static string pri_txt = "";
    static string con_txt = "";
    static string checknew = "";
    static string sms_req = "";
    static string sms_app = "";
    static string sms_exit = "";
    static string event_requestcode;
    static string sms_mom = "";
    static string sms_dad = "";
    static string sms_stud = "";
    string rollflag1 = string.Empty;
    string rq_fk1 = "";
    static Hashtable depthash = new Hashtable();
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    static string previousCat = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.MaintainScrollPositionOnPostBack = true;
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("ReqM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Request"].Trim();
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

            if (!Request.FilePath.Contains("RequestHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/RequestMOD/RequestHome.aspx");
                    return;
                }
            }

            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                ItemReqNo();
                txt_min_location.Enabled = false;
                div_Published.Visible = true;
                rdo_single.Checked = true;
                rdo_commpati.Checked = true;
                pop_minute.Visible = true;
                div_orgindoor.Visible = true;
                BindCollege();
                newbindbatch();
                ItemReqNo();
                loadaction();
                bindstaffname();
                loadhour();
                loadminits();
                timevalue();
                loadeventtype();
                loaddesc();
                loadtour();
                loadgame();
                loadseminar();
                loadaward();
                loadtitle();
                loadexpn();
                loadnewaction();
                outinstitution();
                outorganizer();
                res();
                res_semi();
                res_awd();
                res_game();
                res_title();
                res_expnc();
                res_tour();
                res_insititution();
                res_organizer();
                res_new();
                itemheader();
                loadsubheadername();
                itemmaster1();
                degree();
                bindbranch1(college);
                bindbranch2(college);
                bindsem();
                clgbuild();
                loadstaffdep1(collegecode);
                loadstaffdept1(collegecode);
                bind_stafType1();
                bind_stafTypenew();
                bind_design1();
                ViewState["CurrentTable"] = null;
                ViewState["CurrentTable1"] = null;
                ViewState["CurrentTablenew"] = null;

                if (rdo_single.Checked == true)
                {
                    lnk_min_loc.Visible = true;
                }
                else
                {
                    lnk_min_loc.Visible = false;
                }
                bindbatch_Present();
                binddegree_present();
                bindbranch1(college);
                bindbranch2(college);
                txtfd.Enabled = true;
                txttd.Enabled = false;
                txt_min_enddate.Visible = false;
                lbl_min_enddate.Visible = false;
                TextBox6.Text = DateTime.Now.ToString("dd/MM/yyyy");
                TextBox6.Attributes.Add("readonly", "readonly");
                txtfd.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtfd.Attributes.Add("readonly", "readonly");
                txttd.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttd.Attributes.Add("readonly", "readonly");
                txt_min_startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_min_startdate.Attributes.Add("readonly", "readonly");
                txt_min_enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_min_enddate.Attributes.Add("readonly", "readonly");
                txt_patenappdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_patenappdate.Attributes.Add("readonly", "readonly");
                txt_pre_Startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_pre_Startdate.Attributes.Add("readonly", "readonly");
                txt_pre_enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_pre_enddate.Attributes.Add("readonly", "readonly");
                txt_mat_expect.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_mat_expect.Attributes.Add("readonly", "readonly");
                txtdays.Text = "1";
                txt_min_enddate.Enabled = false;
                div_prsnt_staff.Visible = true;
                popup_selectstaff.Visible = true;
                loadfsstaff1();
                btn_go_prsntclik.Visible = true;
                txt_studenorgsearch.Visible = true;
                UpdatePanel1.Visible = true;
                lblissueperson.Visible = true;
                lbl_org_studnamee.Visible = true;
                bindstudentname();
                previousCat = null;
                rdbinst.Checked = true;
                gv33div.Visible = true;
            }
            if (fileupload.HasFile)
            {
            }
        }
        catch (Exception ex) { }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }
    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
        pop_radiodiv.Visible = true;
        div_Published.Visible = true;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = false;
        div_rdo_others.Visible = false;
       // event_clearall();
        rdb_nat_int.Visible = true;
        DDDD.Visible = true;
        lbl_heading.Text = "Papers Published";
        txtothers.Text = "Papers Published";
        div_seminar.Visible = false;
        div_workshop.Visible = false;
        div_gustt.Visible = false;

    }
    protected void chkaward_CheckedChanged(object sender, EventArgs e)
    {
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = true;
        div_rdo_others.Visible = false;
       // event_clearall();
        rdb_nat_int.Visible = true;
        DDDD.Visible = true;
        lbl_heading.Text = "Patents";
        txtothers.Text = "Patents";
        div_seminar.Visible = false;
        div_workshop.Visible = false;
        div_gustt.Visible = false;

    }

    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {
        pop_radiodiv.Visible = true;
        div_seminar.Visible = true;
        div_workshop.Visible = false;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = false;
      //  event_clearall();
        rdb_nat_int.Visible = true;
        DDDD.Visible = true;
        lbl_heading.Text = "Conference";
        txtothers.Text = "Conference";
        div_rdo_others.Visible = false;
        div_gustt.Visible = false;
    }

    public void rdb_seminor_CheckedChanged(object sender, EventArgs e)
    {
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = true;
        div_seminar.Visible = false;
        div_workshop.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = false;
      //  event_clearall();
        rdb_nat_int.Visible = true;
        DDDD.Visible = true;
        lbl_heading.Text = "Seminar";
        txtothers.Text = "Seminar";
        div_rdo_others.Visible = false;
        div_gustt.Visible = false;
    }
    public void rdb_workshop_CheckedChanged(object sender, EventArgs e)
    {
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_seminar.Visible = false;
        div_workshop.Visible = true;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = false;
       // event_clearall();
        rdb_nat_int.Visible = true;
        DDDD.Visible = true;
        lbl_heading.Text = "WorkShop";
        txtothers.Text = "WorkShop";
        div_rdo_others.Visible = false;
        div_gustt.Visible = false;
    }
    public void cb_pap_prsnt_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            pop_radiodiv.Visible = true;
            div_Published.Visible = false;
            div_present.Visible = true;
            div_Tournament.Visible = false;
            div_dis_vist.Visible = false;
            div_Membership.Visible = false;
            div_research.Visible = false;
            div_Conference.Visible = false;
            div_Award.Visible = false;
            div_intership.Visible = false;
            div_Patents.Visible = false;
            lbl_heading.Text = "Papers Presented";
            txtothers.Text = "Papers Presented";
            div_rdo_others.Visible = false;
            rdb_nat_int.Visible = true;
            DDDD.Visible = true;
            div_seminar.Visible = false;
            div_workshop.Visible = false;
            div_gustt.Visible = false;
        }
        catch
        {
        }
    }
    public void rdb_Award_CheckedChanged(object sender, EventArgs e)
    {
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_Award.Visible = true;
        div_intership.Visible = false;
        div_Patents.Visible = false;
        lbl_heading.Text = "Award Details";
        div_rdo_others.Visible = false;
        txtothers.Text = "Award Details";
        div_rdo_others.Visible = false;
        rdb_nat_int.Visible = true;
        DDDD.Visible = true;
        div_seminar.Visible = false;
        div_workshop.Visible = false;
        div_gustt.Visible = false;
    }
    public void rdb_student_CheckedChanged(object sender, EventArgs e)
    {
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = true;
        div_Patents.Visible = false;
        lbl_heading.Text = "Student Intership";
        txtothers.Text = "Student Intership";
        div_rdo_others.Visible = false;
        div_seminar.Visible = false;
        div_workshop.Visible = false;
        rdb_nat_int.Visible = true;
        DDDD.Visible = true;
        div_gustt.Visible = false;
    }
    public void rdb_ReSearch_CheckedChanged(object sender, EventArgs e)
    {
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = true;
        div_Conference.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = false;
        lbl_heading.Text = "Research";
        txtothers.Text = "Research";
        div_rdo_others.Visible = false;
        rdb_nat_int.Visible = true;
        DDDD.Visible = true;
        div_seminar.Visible = false;
        div_workshop.Visible = false;
        div_gustt.Visible = false;
    }
    public void rdb_Membership_CheckedChanged(object sender, EventArgs e)
    {
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = true;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = false;
        lbl_heading.Text = "Membership";
        txtothers.Text = "Membership";
        div_rdo_others.Visible = false;
        rdb_nat_int.Visible = true;
        DDDD.Visible = true;
        div_seminar.Visible = false;
        div_workshop.Visible = false;
        div_gustt.Visible = false;
    }
    public void rdb_Distinguished_CheckedChanged(object sender, EventArgs e)
    {
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = true;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = false;
        lbl_heading.Text = "Visitors";
        txtothers.Text = "Visitors";
        div_rdo_others.Visible = false;
        div_seminar.Visible = false;
        div_workshop.Visible = false;
        div_gustt.Visible = false;
    }
    public void rdb_Tournamentk_CheckedChanged(object sender, EventArgs e)
    {
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = true;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = false;
        lbl_heading.Text = "Tournament";
        txtothers.Text = "Tournament";
        div_rdo_others.Visible = false;
        rdb_nat_int.Visible = true;
        DDDD.Visible = true;
        div_seminar.Visible = false;
        div_workshop.Visible = false;
        div_gustt.Visible = false;
    }

    public void rdb_Symposium_CheckedChanged(object sender, EventArgs e)
    {
        lbl_heading.Text = "Symposium";
        txtothers.Text = "Symposium";
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = false;
        div_rdo_others.Visible = false;
        rdb_nat_int.Visible = true;
        DDDD.Visible = true;
        div_gustt.Visible = false;
    }
    public void rdb_gustCheckedChanged(object sender, EventArgs e)
    {
        lbl_heading.Text = "Guest Lectures";
        txtothers.Text = "Guest Lectures";
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = false;
        DDDD.Visible = false;
        div_gustt.Visible = true;
        div_rdo_others.Visible = false;
        div_seminar.Visible = false;
        div_workshop.Visible = false;
        rdb_nat_int.Visible = false;
    }
    public void RDB_OTHERS_CheckedChanged(object sender, EventArgs e)
    {
        lbl_heading.Text = "Others";
        if (ddl_actname.SelectedItem.Text == "Select")
        {
            txtothers.Text = "";
        }
        else
        {
            txtothers.Text = Convert.ToString(ddl_actname.SelectedItem.Text);
        }
        pop_radiodiv.Visible = true;
        div_Published.Visible = false;
        div_present.Visible = false;
        div_Tournament.Visible = false;
        div_dis_vist.Visible = false;
        div_Membership.Visible = false;
        div_research.Visible = false;
        div_Conference.Visible = false;
        div_Award.Visible = false;
        div_intership.Visible = false;
        div_Patents.Visible = false;
        DDDD.Visible = false;
        div_rdo_others.Visible = true;
        rdb_nat_int.Visible = false;
        div_seminar.Visible = false;
        div_workshop.Visible = false;
        div_gustt.Visible = false;
    }
    public void btn_rdogo_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {

        }
    }
    protected void rdb1_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb1.Checked == true)
        {
            div_orgindoor.Visible = true;
            div_orgoutdoor.Visible = false;
            lnk_min_loc.Visible = true;
            txt_min_location.ReadOnly = true;
            lnk_locationmul.Visible = true;
            txt_min_location.Enabled = false;
        }
        else
        {
            lnk_min_loc.Visible = false;
            txt_min_location.ReadOnly = false;
            div_orgindoor.Visible = false;
        }

    }

    protected void rdb2_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb2.Checked == true)
        {
            div_orgoutdoor.Visible = true;
            div_orgindoor.Visible = false;
            lnk_min_loc.Visible = false;
            lnk_locationmul.Visible = false;
            txt_min_location.Enabled = true;
        }
        else
        {
            div_orgoutdoor.Visible = false;
            lnk_min_loc.Visible = true;
            lnk_locationmul.Visible = true;
        }

    }
    public void rdo_single_CheckedChanged(object sender, EventArgs e)
    {
        if (rdo_single.Checked == true)
        {
            pop_minute.Visible = true;
            lnk_min_loc.Visible = true;
            lnk_locationmul.Visible = false;
            //txtfd.Enabled = false;
            txttd.Enabled = false;
            pop_Gv1_div.Visible = false;
            txtfd.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttd.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtdays.Text = "1";
            txt_min_enddate.Visible = false;
            lbl_min_enddate.Visible = false;
            txt_min_startdate.Enabled = true;
        }
        event_clearall();

    }
    public void rdo_multipl_CheckedChanged(object sender, EventArgs e)
    {
        if (rdo_multipl.Checked == true)
        {
            pop_minute.Visible = false;
            lnk_min_loc.Visible = false;

            txt_min_enddate.Visible = false;
            lbl_min_enddate.Visible = false;
            txtfd.Enabled = true;
            txttd.Enabled = true;
            //pop_minute.Visible = false;
            rdb_Papers.Checked = false;
            rdb_Paper.Checked = false;
            rdb_Patents.Checked = false;
            rdb_Conference.Checked = false;
            rdb_seminor.Checked = false;
            rdb_workshop.Checked = false;
            rdb_Award.Checked = false;
            rdb_student.Checked = false;
            rdb_ReSearch.Checked = false;
            rdb_Distinguished.Checked = false;
            rdb_Tournamentk.Checked = false;
            rdb_Symposium.Checked = false;
            rdb_gust.Checked = false;
            RDB_OTHERS.Checked = false;
            pop_radiodiv.Visible = false;
            txt_min_startdate.Enabled = false;
        }
        event_clearall();

    }
    protected void txtfd_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdo_single.Checked == true)
                txttd.Text = txtfd.Text;
            string enddt=string.Empty;
            string dt = txtfd.Text;
            string[] Split = dt.Split('/');
            DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
              if (rdo_single.Checked == true)
                  enddt = txttd.Text;
              else
                      enddt = DateTime.Now.ToString("dd/MM/yyyy");

            Split = enddt.Split('/');
            DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            TimeSpan days = fromdate - todate;
            string ndate = Convert.ToString(days);
            Split = ndate.Split('.');
            string getdate = Split[0];
            int finaldate = Convert.ToInt32(getdate);
          
            if (fromdate < todate)
            {
                spanerr.Visible = false;

                alertpopwindow.Visible = false;
                spandays.Visible = true;
                txtdays.Text = Convert.ToString(finaldate + 1);
            }
            else
            {

                spanerr.Visible = true;
                spandays.Visible = false;
                alertpopwindow.Visible = true;
                txtfd.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttd.Text = DateTime.Now.ToString("dd/MM/yyyy");

            }
            
        }
        catch
        {

            spanerr.Visible = false;
            alertpopwindow.Visible = false;
            spandays.Visible = true;
            txtdays.Text = "1";
        }
    }
    protected void txttd_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string dt = txtfd.Text;
            string[] Split = dt.Split('/');
            DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            string enddt = txttd.Text;
            Split = enddt.Split('/');
            DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            TimeSpan days = fromdate - todate;
            string ndate = Convert.ToString(days);
            Split = ndate.Split('.');
            string getdate = Split[0];
            int finaldate = Convert.ToInt32(getdate);
            if (fromdate < todate)
            {

                spanerr.Visible = true;
                alertpopwindow.Visible = true;
                spandays.Visible = false;
            }

            else if (fromdate == todate)
            {
                txtdays.Text = "1";
                add_details();
            }
            else
            {
                txtdays.Text = Convert.ToString(finaldate + 1);
                if (rdo_single.Checked == true)
                {
                    pop_minute.Visible = true;
                    txt_min_enddate.Visible = true;
                    lbl_min_enddate.Visible = true;
                }
                else if (rdo_multipl.Checked == true)
                {
                    pop_minute.Visible = false;
                    add_details();
                    txt_min_enddate.Visible = false;
                    lbl_min_enddate.Visible = false;

                }
            }
        }
        catch
        {
            spanerr.Visible = false;
            alertpopwindow.Visible = false;
            spandays.Visible = true;
            txtdays.Text = "1";
        }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
        spandays.Visible = true;
        txtdays.Text = "";
    }

    public void btn_go_event_Click(object sender, EventArgs e)
    {
        add_details();
    }
    public void add_details()
    {
        if (txtdays.Text != "")
        {
            pop_Gv1_div.Visible = true;
            datecal();


        }
        //else if (ddlname.SelectedItem.Text == "Select" && txtothers.Text == "")
        //{
        //    imgdiv2.Visible = true;
        //    lbl_alert.Text = "Kindly Select the Event Name";
        //    txttd.Text = DateTime.Now.ToString("dd/MM/yyyy");
        //    txtdays.Text = "1";
        //}

        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Kindly Select the Date";
        }


    }
    public void datecal()
    {
        try
        {
            int finaldate = 0;
            string dt = txtfd.Text;
            string[] Split = dt.Split('/');
            DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            string enddt = txttd.Text;
            Split = enddt.Split('/');
            DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            TimeSpan days = fromdate - todate;
            string ndate = Convert.ToString(days);
            Split = ndate.Split('.');
            string getdate = Split[0];
            if (fromdate != todate)
            {
                finaldate = Convert.ToInt32(getdate);
            }

            if (fromdate == todate)
            {
                BindGridview();
            }
            else if (fromdate > todate)
            {
                if (txtdays.Text != "")
                {
                    spanerr.Visible = false;
                    alertpopwindow.Visible = false;

                    txtdays.Text = Convert.ToString(finaldate + 1);
                    ///gvdiv.Visible = true;
                    BindGridview();
                }
            }

            else
            {
                //txtdays.Text = "";
                spanerr.Visible = true;
                alertpopwindow.Visible = true;
                spandays.Visible = false;

            }
        }
        catch
        {
            spanerr.Visible = false;
            alertpopwindow.Visible = false;
            spandays.Visible = true;
            txtdays.Text = "1";
        }
    }
    public void ddl_org_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        bindstudentname();
    }
    public void bindsem()
    {
        cbl_or_sem.Items.Clear();
        txt_org_sem.Text = "--Select--";
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        ds.Clear();
        string branch = "";
        string build = "";
        string batch1 = "";
        if (cbl_branch.Items.Count > 0)
        {
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {

                if (cbl_branch.Items[i].Selected == true)
                {
                    build = cbl_branch.Items[i].Value.ToString();
                    if (branch == "")
                    {
                        branch = build;
                    }
                    else
                    {
                        branch = branch + "," + build;

                    }
                }
            }
        }
        batch1 = Convert.ToString(ddl_org_batch.SelectedItem.Text);

        //batch = build;

        if (branch.Trim() != "" && batch1.Trim() != "")
        {
            ds = d2.BindSem(branch, batch1, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string dur = Convert.ToString(ds.Tables[0].Rows[i][0]);
                    if (dur.Trim() != "")
                    {
                        if (duration < Convert.ToInt32(dur))
                        {
                            duration = Convert.ToInt32(dur);
                        }
                    }
                }
            }
            if (duration != 0)
            {
                for (i = 1; i <= duration; i++)
                {
                    cbl_or_sem.Items.Add(Convert.ToString(i));
                }
                if (cbl_or_sem.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_or_sem.Items.Count; row++)
                    {
                        cbl_or_sem.Items[row].Selected = true;
                        cb_or_sem.Checked = true;
                    }
                    txt_org_sem.Text = "Sem(" + cbl_or_sem.Items.Count + ")";
                }
            }


        }

    }
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";

            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {

                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
                        build1 = cbl_degree.Items[i].Value.ToString();
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
                bindbranch(buildvalue1);
                bindstudentname();
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    cbl_branch.ClearSelection();
                    cb_branch.Checked = false;
                }
            }
            // Button2.Focus();
            bindsem();
            All_dropdownchange();

        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;

            cb_degree.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_branch.Text = "--Select--";
                    build = cbl_degree.Items[i].Value.ToString();
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
            bindbranch(buildvalue);
            bindstudentname();
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
            }
            bindsem();
            All_dropdownchange();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch(string branch)
    {
        try
        {
            cbl_branch.Items.Clear();
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            {
                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch.Items.Count; i++)
                        {
                            cbl_branch.Items[i].Selected = true;
                        }
                        txt_branch.Text = "Branch(" + cbl_branch.Items.Count + ")";
                    }

                }
            }
            if (rdo_org_staff.Checked == true)
                bindstaffname();
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
            bindsem();
            bindstaffname();
            All_dropdownchange();
            bindstudentname();
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
            if (commcount == cbl_branch.Items.Count)
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
            }
            // Button2.Focus();
            bindsem();
            bindstaffname();
            cb_staff_name.Checked = false;
            All_dropdownchange();
            bindstudentname();
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_or_sem_checkedchange(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_org_sem.Text = "--Select--";
            if (cb_or_sem.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_or_sem.Items.Count; i++)
                {
                    cbl_or_sem.Items[i].Selected = true;
                }
                txt_org_sem.Text = "Semester(" + (cbl_or_sem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_or_sem.Items.Count; i++)
                {
                    cbl_or_sem.Items[i].Selected = false;
                }
            }
            All_dropdownchange();
            bindstudentname();
        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_or_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_or_sem.Checked = false;
            int commcount = 0;
            txt_org_sem.Text = "--Select--";

            for (int i = 0; i < cbl_or_sem.Items.Count; i++)
            {
                if (cbl_or_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_or_sem.Checked = false;

                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_or_sem.Items.Count)
                {

                    cb_or_sem.Checked = true;
                }
                txt_org_sem.Text = "Semester(" + commcount.ToString() + ")";

            }

            All_dropdownchange();
            bindstudentname();
        }
        catch (Exception ex)
        {

        }
    }
    public void cb_staff_name1_CheckedChange(object sender, EventArgs e)
    {
        try
        {

            if (cb_staff_name.Checked == true)
            {
                for (int i = 0; i < cbl_staff_name.Items.Count; i++)
                {
                    cbl_staff_name.Items[i].Selected = true;
                }
                txt_staffnamemul.Text = "Sfaff Name(" + (cbl_staff_name.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_staff_name.Items.Count; i++)
                {
                    cbl_staff_name.Items[i].Selected = false;
                }
                txt_staffnamemul.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void cb_staff_name1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_staffnamemul.Text = "--Select--";
            cb_staff_name.Checked = false;
            for (int i = 0; i < cbl_staff_name.Items.Count; i++)
            {
                if (cbl_staff_name.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_staffnamemul.Text = "Staff Name(" + commcount.ToString() + ")";
                if (commcount == cbl_staff_name.Items.Count)
                {
                    cb_staff_name.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindstaffname()
    {
        try
        {
            string dept = "";

            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (dept == "")
                    {
                        dept = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        dept = dept + "'" + "," + "" + "'" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string srisql = "select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and  s.college_code =  h.college_code and s.college_code = d.collegecode and h.dept_code in ( '" + dept + "')   and s.college_code='" + collegecode1 + "' and resign = 0 and settled = 0 and latestrec=1";

            // string srisql = "select distinct s.staff_name,s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 ";
            ds.Clear();
            ds = da.select_method_wo_parameter(srisql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staff_name.DataSource = ds;
                cbl_staff_name.DataTextField = "staff_name";
                cbl_staff_name.DataValueField = "staff_code";
                cbl_staff_name.DataBind();
                //cb_staff_name.Checked = true;
                //if (cbl_staff_name.Items.Count > 0)
                //{
                //    for (int i = 0; i < cbl_staff_name.Items.Count; i++)
                //    {
                //        cbl_staff_name.Items[i].Selected = true;
                //    }
                //    txt_staffnamemul.Text = "Staff Name(" + cbl_staff_name.Items.Count + ")";
                //}
            }

        }
        catch
        {
        }
    }
    protected void GV1_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellvalue = e.Row.Cells.Count;
                e.Row.Cells[cellvalue - 1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(GV1, "Select$" + e.Row.RowIndex);
                //  e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(GV1, "instruction$" + e.Row.RowIndex);

            }
        }
        catch
        {
        }
    }
    protected void grid_edulevel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ii = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Select")
            {
                // btnadd(row);
                minute_event(ii);
                savetemp(ii);
            }
            foreach (GridViewRow row1 in GV1.Rows)
            {
                if (ii == row1.DataItemIndex)
                {
                    // row1.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                }
                else
                {
                    //row1.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                }
            }

            timevalue();
            rdo_commpati.Checked = true;
        }
        catch
        {
        }
    }
    public void txt_min_enddate_Changed(object sender, EventArgs e)
    {
        try
        {
            string dt = txt_min_startdate.Text;
            string[] Split = dt.Split('/');
            DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            string enddt = txt_min_enddate.Text;
            Split = enddt.Split('/');
            DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            TimeSpan days = fromdate - todate;
            string ndate = Convert.ToString(days);
            Split = ndate.Split('.');
            string getdate = Split[0];
            int finaldate = Convert.ToInt32(getdate);
            if (fromdate < todate)
            {

                spanerr.Visible = true;
                alertpopwindow.Visible = true;
                spandays.Visible = false;

            }
            else
            {
                //txt_min_action.Text = Convert.ToString(finaldate + 1);
                //btnadd();
            }
        }
        catch
        {
            spanerr.Visible = false;
            alertpopwindow.Visible = false;
            spandays.Visible = true;
            txt_min_action.Text = "1";
        }
    }
    public void lnk_min_loc_Click(object sender, EventArgs e)
    {
        pop_bul_flr_room.Visible = true;
    }
    public void txt_min_action_Changed(object sender, EventArgs e)
    {

    }
    public void btn_min_Action_Click(object sender, EventArgs e)
    {

        string starthr = Convert.ToString(ddl_hour1.SelectedItem.Text);
        string startmin = Convert.ToString(ddl_minits1.Text);
        string startday = Convert.ToString(ddl_timeformate1.Text);
        string endhr = Convert.ToString(ddl_endhour1.SelectedItem.Text);
        string endmin = Convert.ToString(ddl_endminit1.Text);
        string endday = Convert.ToString(ddl_endformate1.Text);
        string location = "";
        int starthrvalue = 0;
        int endhrvalue = 0;
        starthrvalue = Convert.ToInt32(starthr);
        endhrvalue = Convert.ToInt32(endhr);
        if (txt_min_startperiod.Text != "" && txt_min_endperiod.Text != "")
        {
            if ((starthr == endhr && startmin != endmin) || (starthr != endhr && startmin == endmin) || (starthr != endhr && startmin != endmin) || (starthr == endhr && startmin == endmin && startday != endday))
            {

                if (startday == "PM" && endday == "AM")
                {
                    imgdiv2.Visible = true;
                    pnl2.Visible = true;
                    lbl_alert.Text = "Kindly Select The Valid Time";
                    return;
                }
                else
                {
                    string starttime = starthr + ":" + startmin + ":" + startday;
                    string endtime = endhr + ":" + endmin + ":" + endday;
                    if (rdb1.Checked == true)
                    {
                        location = "Indoor";
                    }
                    else
                    {
                        location = "Outdoor";
                    }

                    string start_prd = Convert.ToString(txt_min_startperiod.Text);
                    string end_prd = Convert.ToString(txt_min_endperiod.Text);
                    string noofact = Convert.ToString(txt_min_action.Text);
                    foreach (GridViewRow row1 in GV1.Rows)
                    {
                        if (ii == row1.DataItemIndex)
                        {
                            TextBox txtsttime = (TextBox)GV1.Rows[ii].FindControl("txt_start");
                            txtsttime.Text = starttime;

                            TextBox txtendtime = (TextBox)GV1.Rows[ii].FindControl("txt_end");
                            txtendtime.Text = endtime;

                            TextBox txtstprd = (TextBox)GV1.Rows[ii].FindControl("txt_st_prd");
                            txtstprd.Text = start_prd;

                            TextBox txtendprd = (TextBox)GV1.Rows[ii].FindControl("txt_end_prd");
                            txtendprd.Text = end_prd;

                            TextBox txtloc = (TextBox)GV1.Rows[ii].FindControl("txt");
                            txtloc.Text = location;
                        }
                    }
                    BindGridviewadd();

                    if (rdo_commpati.Checked == true)
                    {
                        lnk_patici.Visible = false;
                        lnk_com_particate.Visible = true;
                    }
                    else if (rdo_indivparti.Checked == true)
                    {
                        lnk_patici.Visible = true;
                        lnk_com_particate.Visible = false;
                    }
                    if (rdo_single.Checked == true)
                    {
                        lnk_locationmul.Visible = false;
                    }
                    else if (rdo_multipl.Checked == true)
                    {
                        lnk_locationmul.Visible = true;
                    }
                    //}
                    ////else
                    //{
                    //    imgdiv2.Visible = true;
                    //    lbl_alert.Text = "Kindly Select The Valid Time";
                    //}
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Kindly Select The Valid Time";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Kindly Fill All The Fields";
        }

        All_dropdownchange();
    }
    public void lnk_com_particate_Click(object sender, EventArgs e)
    {
        MergeCells();
        parti_studbind();
        pop_add_staff_stud_othr.Visible = true;
        pop_add_staff_stud_othr1.Visible = false;
        pop_bul_flr_room.Visible = false;

    }
    protected void gridadd_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellvalue = e.Row.Cells.Count;
                e.Row.Cells[cellvalue - 2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gridadd, "Select$" + e.Row.RowIndex);
                //  e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(GV1, "instruction$" + e.Row.RowIndex);

            }
        }
        catch
        {
        }
    }
    protected void gridadd_edulevel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            jj = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Select")
            {

                adddetailsdiv(jj);
            }
        }
        catch
        {
        }
    }

    public void adddetailsdiv(int index)
    {
        try
        {
            string STARTTIME = "";
            string ENDTIME = "";
            string lENDTIME = "";
            int l_hr = 0;
            int l_min = 0;
            string ar_ap = "";
            string firstampm = "";
            string beforehr = "";
            string beforemin = "";
            string beforeampm = "";
            string laststarttime = "";
            string firsttime = "";
            if (rdo_commpati.Checked == true)
            {
                lnk_patici.Visible = false;
                lnk_com_particate.Visible = true;
            }
            else if (rdo_indivparti.Checked == true)
            {
                lnk_patici.Visible = true;
                lnk_com_particate.Visible = false;
            }
            if (rdo_single.Checked == true)
            {
                lnk_locationmul.Visible = false;
            }
            else if (rdo_multipl.Checked == true)
            {
                lnk_locationmul.Visible = true;
            }
            int count = 0;
            for (int i = 0; i < gridadd.Rows.Count; i++)
            {
                count++;
                if (i == gridadd.Rows.Count - 1)
                {
                    TextBox txt_etime = (TextBox)gridadd.Rows[i].FindControl("txt_end");
                    lENDTIME = Convert.ToString(txt_etime.Text);
                    string[] ar = lENDTIME.Split(':');
                    string ar_hr = ar[0];
                    string ar_min = ar[1];
                    ar_ap = ar[2];
                    l_hr = Convert.ToInt32(ar_hr);
                    l_min = Convert.ToInt32(ar_min);

                }
                if (i == 0)
                {
                    TextBox txt_firsttime = (TextBox)gridadd.Rows[0].FindControl("txt_start");
                    firsttime = Convert.ToString(txt_firsttime.Text);
                    string[] ar = firsttime.Split(':');
                    firstampm = ar[2];

                }
            }
            //Validation for AM PM
            if (ar_ap != firstampm)
            {
                if (ar_ap == "AM" && firstampm == "PM")
                {
                    poprdoview.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Kindly Fill The Valid Time";
                    for (int i = 0; i < gridadd.Rows.Count; i++)
                    {
                        TextBox txt_etime = (TextBox)gridadd.Rows[i].FindControl("txt_end");
                        txt_etime.Text = "";
                        TextBox txt_firsttime = (TextBox)gridadd.Rows[i].FindControl("txt_start");
                        txt_firsttime.Text = "";
                    }
                    return;
                }
            }
            foreach (GridViewRow row1 in gridadd.Rows)
            {
                if (jj == row1.DataItemIndex)
                {

                    TextBox txtactionn = (TextBox)gridadd.Rows[jj].FindControl("txtactname");
                    TextBox txt_descri = (TextBox)gridadd.Rows[jj].FindControl("txt_descri");
                    TextBox txt_stime = (TextBox)gridadd.Rows[jj].FindControl("txt_start");
                    STARTTIME = Convert.ToString(txt_stime.Text);
                    TextBox txt_etime = (TextBox)gridadd.Rows[jj].FindControl("txt_end");
                    ENDTIME = Convert.ToString(txt_etime.Text);

                    if (txtactionn.Text != "")
                    {
                        ddl_act_namenew.SelectedItem.Text = txtactionn.Text;
                        txt_act_description.Text = txt_descri.Text;
                    }
                    else
                    {
                        ddl_act_namenew.SelectedItem.Text = "Select";
                        txt_act_description.Text = "";
                    }
                    if (txt_stime.Text != "" && txt_etime.Text != "")
                    {
                        string[] arr = STARTTIME.Split(':');
                        string shr = arr[0];
                        string smin = arr[1];
                        string sap = arr[2];
                        string[] arr1 = ENDTIME.Split(':');
                        string ehr = arr1[0];
                        string emin = arr1[1];
                        string eap = arr1[2];
                        int s_hr = Convert.ToInt32(shr);
                        int s_min = Convert.ToInt32(smin);
                        int e_hr = Convert.ToInt32(ehr);
                        int e_min = Convert.ToInt32(emin);
                        int val = Convert.ToInt32(jj);

                        if (val > 0)
                        {
                            int rowcount = val - 1;
                            TextBox txt1time = (TextBox)gridadd.Rows[rowcount].FindControl("txt_end");
                            string brforetime = Convert.ToString(txt1time.Text);
                            string[] bt = brforetime.Split(':');
                            beforehr = bt[0];
                            beforemin = bt[1];
                            beforeampm = bt[2];
                            // before-row-endtime--on row start time
                            if (Convert.ToInt32(beforehr) >= s_hr && beforeampm == sap)
                            {
                                if (Convert.ToInt32(beforemin) > s_min)
                                {
                                    poprdoview.Visible = false;
                                    imgdiv2.Visible = true;
                                    lbl_alert.Text = "Kindly Fill The Valid Time";
                                    txt_stime.Text = "";
                                    return;
                                }

                            }
                            if (ar_ap == firstampm)
                            {
                                if (beforeampm != ar_ap)
                                {
                                    poprdoview.Visible = false;
                                    imgdiv2.Visible = true;
                                    lbl_alert.Text = "Kindly Fill The Valid Time";
                                    txt_stime.Text = "";
                                    return;
                                }

                            }
                            if (ar_ap != firstampm)
                            {
                                if (firstampm != beforeampm && beforeampm == "AM")
                                {
                                    poprdoview.Visible = false;
                                    imgdiv2.Visible = true;
                                    lbl_alert.Text = "Kindly Fill The Valid Time";
                                    txt_stime.Text = "";
                                    return;
                                }

                            }
                            // last row end time---on row start time
                            if (l_hr < s_hr && sap == ar_ap)
                            {
                                poprdoview.Visible = false;
                                imgdiv2.Visible = true;
                                lbl_alert.Text = "Kindly Fill The Valid Time";
                                txt_stime.Text = "";
                                return;
                            }
                        }

                        int val1 = val + 1;
                        if (count < val1)
                        {
                            TextBox txt_stlastime = (TextBox)gridadd.Rows[val1].FindControl("txt_start");
                            laststarttime = Convert.ToString(txt_stlastime.Text);
                            if (laststarttime != "")
                            {
                                string[] last = laststarttime.Split(':');
                                beforehr = last[0];
                                beforemin = last[1];
                                beforeampm = last[2];
                                //next row start time-- lastrow end time
                                if (Convert.ToInt32(beforehr) <= e_hr && beforeampm == eap)
                                {
                                    if (Convert.ToInt32(beforemin) < e_min)
                                    {
                                        poprdoview.Visible = false;
                                        imgdiv2.Visible = true;
                                        pnl2.Visible = true;
                                        lbl_alert.Text = "Kindly Fill The Valid Time";
                                        txt_etime.Text = "";
                                        return;
                                    }
                                }
                            }
                        }
                        //on row start nd end time
                        if (s_hr >= e_hr && sap == eap)
                        {
                            if (s_min > e_min)
                            {
                                poprdoview.Visible = false;
                                imgdiv2.Visible = true;
                                pnl2.Visible = true;
                                lbl_alert.Text = "Kindly Fill The Valid Time";
                                txt_etime.Text = "";
                                return;
                            }
                        }
                        //on row endtime --- last row endtime
                        if (e_hr > l_hr && ar_ap == eap)
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "Kindly Fill The Valid Time";
                            pnl2.Visible = true;
                            poprdoview.Visible = false;
                            txt_etime.Text = "";
                            return;

                        }
                        if (ar_ap == firstampm)
                        {
                            if (eap != ar_ap && eap != firstampm)
                            {
                                imgdiv2.Visible = true;
                                lbl_alert.Text = "Kindly Fill The Valid Time";
                                pnl2.Visible = true;
                                poprdoview.Visible = false;
                                txt_etime.Text = "";
                                return;


                            }
                            if (ar_ap != firstampm)
                            {
                                if (eap != firstampm && eap == "AM")
                                {
                                    imgdiv2.Visible = true;
                                    lbl_alert.Text = "Kindly Fill The Valid Time";
                                    pnl2.Visible = true;
                                    poprdoview.Visible = false;
                                    txt_etime.Text = "";
                                    return;


                                }
                            }
                        }



                        poprdoview.Visible = true;

                    }
                    else
                    {
                        poprdoview.Visible = false;
                        imgdiv2.Visible = true;
                        pnl2.Visible = true;
                        lbl_alert.Text = "Kindly Fill The Time";
                    }
                }
            }
            if (rdo_indivparti.Checked == true)
            {

            }
        }
        catch
        {
        }
    }
    public void pop_bul_flr_roomclose_Click(object sender, EventArgs e)
    {
        pop_bul_flr_room.Visible = false;

    }
    public void clgbuild()
    {
        try
        {
            cbl_buildname.Items.Clear();
            string bul = "select code,Building_Name from Building_Master";


            ds = d2.select_method_wo_parameter(bul, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_buildname.DataSource = ds;
                cbl_buildname.DataTextField = "Building_Name";
                cbl_buildname.DataValueField = "code";
                cbl_buildname.DataBind();
            }

            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                cbl_buildname.Items[i].Selected = true;
                txt_buildingname.Text = "Building(" + (cbl_buildname.Items.Count) + ")";
                cb_buildname.Checked = true;
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
            //  Button2.Focus();

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
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void clgfloor(string buildname)
    {
        try
        {
            //chklstfloorpo3.Items.Clear();
            cbl_floorname.Items.Clear();
            ds = d2.BindFloor(buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "Floor_Name";
                cbl_floorname.DataBind();
            }
            else
            {
                txt_floorname.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                cbl_floorname.Items[i].Selected = true;
                cb_floorname.Checked = true;
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
            //   Button2.Focus();
            //  clgroom(buildvalue1, buildvalue2);
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
            ds = d2.BindRoom(floorname, buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomname.DataSource = ds;
                cbl_roomname.DataTextField = "Room_Name";
                cbl_roomname.DataValueField = "Room_Name";
                cbl_roomname.DataBind();
            }
            else
            {
                txt_roomname.Text = "--Select--";
            }

            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                cbl_roomname.Items[i].Selected = true;
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
                cb_roomname.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_roomname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_roomname.Checked == true)
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = true;
                }
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
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
        catch (Exception ex)
        {
        }
    }
    protected void cblroomname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_roomname.Checked = false;
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            if (seatcount == cbl_roomname.Items.Count)
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
                cb_roomname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_roomname.Text = "--Select--";
            }
            else
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_bul_flr_select_Click(object sender, EventArgs e)
    {
        try
        {
            string bulname = "";
            string floorname = "";
            string roomname = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    if (bulname == "")
                    {
                        bulname = "" + cbl_buildname.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        bulname = bulname + "" + "," + "" + cbl_buildname.Items[i].Text.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    if (floorname == "")
                    {
                        floorname = "" + cbl_floorname.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        floorname = floorname + "" + "," + "" + cbl_floorname.Items[i].Text.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    if (roomname == "")
                    {
                        roomname = "" + cbl_roomname.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        roomname = roomname + "" + "," + "" + cbl_roomname.Items[i].Text.ToString() + "";
                    }
                }
            }

            string con = bulname + "-" + floorname + "-" + roomname;
            if (lnk_min_loc.Visible == true)
            {
                txt_min_location.Text = con;
            }
            else
            {
                txt_min_location.Text = con;
            }
            pop_bul_flr_room.Visible = false;
        }
        catch
        {
        }

    }
    public void lnk_patici_Click(object sender, EventArgs e)
    {
        loadfsstaff1();
        loaddesc();
        pop_add_staff_stud_othr.Visible = true;
        pop_add_staff_stud_othr1.Visible = false;
        pop_bul_flr_room.Visible = false;
    }
    public void lnk_prest_Click(object sender, EventArgs e)
    {

        loadfsstaff2();
        prsnt_studbind();
        GridView13.Visible = true;
        GridView12.Visible = true;
        pop_add_staff_stud_othr1.Visible = true;
        pop_add_staff_stud_othr.Visible = false;
        pop_bul_flr_room.Visible = false;

    }
    public void lnk_locationmul_Click(object sender, EventArgs e)
    {
        pop_bul_flr_room.Visible = true;
    }
    public void btn_go_addddnew_Click(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["CurrentTable"] != null)
            {
                newparticipant = (Hashtable)ViewState["CurrentTable"];
            }
            if (ViewState["CurrentTable1"] != null)
            {
                newpresented = (Hashtable)ViewState["CurrentTable1"];
            }
            if (ViewState["CurrentTablenew"] != null)
            {
                eventhash = (Hashtable)ViewState["CurrentTablenew"];
            }
            if (ViewState["CurrentTablenewaction"] != null)
            {
                actionhash = (Hashtable)ViewState["CurrentTablenewaction"];
            }
            if (ViewState["CurrentTableparticcomp"] != null)
            {
                singleparticcomp = (Hashtable)ViewState["CurrentTableparticcomp"];
            }
            if (ViewState["CurrentTableparticindi"] != null)
            {
                singleparticindi = (Hashtable)ViewState["CurrentTableparticindi"];
            }
            if (ViewState["CurrentTablesingleindi"] != null)
            {
                singlepresentindi = (Hashtable)ViewState["CurrentTablesingleindi"];
            }
            if (ViewState["CurrentTablesinglecomp"] != null)
            {
                singlepresentcomp = (Hashtable)ViewState["CurrentTablesinglecomp"];
            }

            string actionvalue = "";
            string checkvalue = "";
            int count = 0;
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;
            int count5 = 0;
            int count6 = 0;
            int count7 = 0;
            string val = "";
            string actionname = "";
            string act = "";
            string description = "";
            string saveaction = "";
            string location = "";
            if (ddl_act_namenew.SelectedItem.Value != "Select")
            {
                if (ddl_act_namenew.SelectedItem.Value != "Others")
                {
                    actionvalue = Convert.ToString(ddl_act_namenew.SelectedItem.Text);
                }
                else
                {
                    actionvalue = Convert.ToString(txt_act_namenew.Text);
                    saveaction = subjectcodenew("Action", actionvalue);
                }
            }

            if (ddl_act_namenew.SelectedItem.Text != "Select" && ddl_act_namenew.SelectedItem.Text != "Others")
            {
                act = Convert.ToString(ddl_act_namenew.SelectedItem.Text);
            }
            else if (ddl_act_namenew.SelectedItem.Text == "Others")
            {
                act = Convert.ToString(txt_act_namenew.Text);
            }
            else
            {
                act = "";
            }
            if (rdo_single.Checked == true)
            {
                TextBox txtact = (TextBox)gridadd.Rows[jj].FindControl("txtactname");
                txtact.Text = act;
                actionname = act;

                description = Convert.ToString(txt_act_description.Text);
                location = Convert.ToString(txt_min_location.Text);
                string value = "";


                for (int i = 0; i < GridView2.Rows.Count; i++)
                {
                    CheckBox checkvalue1 = (CheckBox)GridView2.Rows[i].FindControl("chkup3"); ;
                    if (checkvalue1.Checked == true)
                    {
                        count++;
                        Label stud_appno = (Label)GridView2.Rows[i].FindControl("lblappno");
                        val = Convert.ToString(stud_appno.Text);

                        if (rdo_indivparti.Checked == true)
                        {
                            if (!newparticipant.Contains(Convert.ToString(actionname)))
                            {

                                newparticipant.Add(Convert.ToString(actionname), Convert.ToString(val));
                            }
                            else
                            {
                                string getvalue = Convert.ToString(newparticipant[Convert.ToString(actionname)]);
                                if (getvalue.Trim() != "")
                                {
                                    getvalue = getvalue + "," + val;
                                    newparticipant.Remove(Convert.ToString(actionname));
                                    if (getvalue.Trim() != "")
                                    {
                                        newparticipant.Add(Convert.ToString(actionname), Convert.ToString(getvalue));
                                    }
                                }

                            }
                        }
                    }
                }


                for (int i = 0; i < GridView1.Rows.Count; i++)
                {

                    CheckBox checkvalue1 = (CheckBox)GridView1.Rows[i].FindControl("chkup3");

                    if (checkvalue1.Checked == true)
                    {
                        count1++;

                        Label stud_appno = (Label)GridView1.Rows[i].FindControl("lblappno");
                        val = Convert.ToString(stud_appno.Text);

                        if (rdo_indivparti.Checked == true)
                        {
                            if (!newparticipant.Contains(Convert.ToString(actionname)))
                            {

                                newparticipant.Add(Convert.ToString(actionname), Convert.ToString(val));
                            }
                            else
                            {
                                string getvalue = Convert.ToString(newparticipant[Convert.ToString(actionname)]);
                                if (getvalue.Trim() != "")
                                {
                                    getvalue = getvalue + "," + val;
                                    newparticipant.Remove(Convert.ToString(actionname));
                                    if (getvalue.Trim() != "")
                                    {
                                        newparticipant.Add(Convert.ToString(actionname), Convert.ToString(getvalue));
                                    }
                                }

                            }
                        }
                    }


                }
                ViewState["CurrentTable"] = newparticipant;

                // PRESENTED PERSON COUNT
                string val1 = "";
                string val2 = "";
                for (int i = 0; i < GridView12.Rows.Count; i++)
                {

                    CheckBox checkvalue1 = (CheckBox)GridView12.Rows[i].FindControl("chkup3");
                    if (checkvalue1.Checked == true)
                    {
                        count2++;
                        Label stud_appno = (Label)GridView12.Rows[i].FindControl("lblappno");
                        val1 = Convert.ToString(stud_appno.Text);

                        DropDownList ddl = (DropDownList)GridView12.Rows[i].FindControl("ddl_categofstaff");
                        val2 = Convert.ToString(ddl.Text);

                        val = val1 + "-" + val2;
                        if (!newpresented.Contains(Convert.ToString(actionname)))
                        {

                            newpresented.Add(Convert.ToString(actionname), Convert.ToString(val));
                        }
                        else
                        {
                            string getvalue = Convert.ToString(newpresented[Convert.ToString(actionname)]);
                            if (getvalue.Trim() != "")
                            {
                                getvalue = getvalue + "," + val;
                                newpresented.Remove(Convert.ToString(actionname));
                                if (getvalue.Trim() != "")
                                {
                                    newpresented.Add(Convert.ToString(actionname), Convert.ToString(getvalue));
                                }
                            }

                        }
                    }
                }

                for (int i = 0; i < GridView13.Rows.Count; i++)
                {

                    CheckBox checkvalue1 = (CheckBox)GridView13.Rows[i].FindControl("chkup3");
                    if (checkvalue1.Checked == true)
                    {

                        count3++;
                        Label stud_appno = (Label)GridView13.Rows[i].FindControl("lblappno");
                        val1 = Convert.ToString(stud_appno.Text);

                        DropDownList ddl = (DropDownList)GridView13.Rows[i].FindControl("ddl_categofstaff");
                        val2 = Convert.ToString(ddl.Text);
                        val = val1 + "-" + val2;
                        if (!newpresented.Contains(Convert.ToString(actionname)))
                        {

                            newpresented.Add(Convert.ToString(actionname), Convert.ToString(val));
                        }
                        {
                            string getvalue = Convert.ToString(newpresented[Convert.ToString(actionname)]);
                            if (getvalue.Trim() != "")
                            {
                                getvalue = getvalue + "," + val;
                                newpresented.Remove(Convert.ToString(actionname));
                                if (getvalue.Trim() != "")
                                {
                                    newpresented.Add(Convert.ToString(actionname), Convert.ToString(getvalue));
                                }
                            }

                        }
                    }
                }
                ViewState["CurrentTable1"] = newpresented;

                string cname = "";
                string pname = "";
                string addr = "";
                string street = "";
                string city = "";
                string pin = "";
                string country = "";
                string state = "";
                string phn = "";
                string mail = "";
                string attch = "";
                string doc = "";

                for (int k = 0; k < GridView8.Rows.Count; k++)
                {
                    count4++;
                    TextBox txtamt = (TextBox)GridView8.Rows[k].FindControl("txtactname");
                    cname = Convert.ToString(txtamt.Text);
                    TextBox txtpanam = (TextBox)GridView8.Rows[k].FindControl("txt_per");
                    pname = Convert.ToString(txtpanam.Text);
                    TextBox txtadd = (TextBox)GridView8.Rows[k].FindControl("txt_add");
                    addr = Convert.ToString(txtadd.Text);
                    TextBox txtst = (TextBox)GridView8.Rows[k].FindControl("txt_st");
                    street = Convert.ToString(txtst.Text);
                    TextBox txtcity = (TextBox)GridView8.Rows[k].FindControl("txt_city");
                    city = Convert.ToString(txtcity.Text);
                    TextBox txtpin = (TextBox)GridView8.Rows[k].FindControl("txt_pin");
                    pin = Convert.ToString(txtpin.Text);
                    TextBox txtcou = (TextBox)GridView8.Rows[k].FindControl("txt_country");
                    country = Convert.ToString(txtcou.Text);
                    TextBox txtstate = (TextBox)GridView8.Rows[k].FindControl("txt_state");
                    state = Convert.ToString(txtstate.Text);
                    TextBox txtphn = (TextBox)GridView8.Rows[k].FindControl("txt_phn");
                    phn = Convert.ToString(txtphn.Text);
                    TextBox txtmail = (TextBox)GridView8.Rows[k].FindControl("txt_mail");
                    mail = Convert.ToString(txtmail.Text);
                    TextBox txtattch = (TextBox)GridView8.Rows[k].FindControl("txt_attch");
                    attch = Convert.ToString(txtattch.Text);
                    TextBox txtat = (TextBox)GridView8.Rows[k].FindControl("txt_e");
                    int at = Convert.ToInt32(txtat.Text);
                    TextBox txtdoc = (TextBox)GridView8.Rows[k].FindControl("txt_dt");
                    doc = Convert.ToString(txtdoc.Text);
                    val = cname + "-" + pname + "-" + addr + "-" + street + "-" + city + "-" + pin + "-" + country + "-" + state + "-" + phn + "-" + mail + "-" + attch + "-" + at + "-" + doc;
                    if (!singleparticcomp.Contains(Convert.ToString(actionname)))
                    {

                        singleparticcomp.Add(Convert.ToString(actionname), Convert.ToString(val));
                    }
                    else
                    {
                        string getvalue = Convert.ToString(singleparticcomp[Convert.ToString(actionname)]);
                        if (getvalue.Trim() != "")
                        {
                            getvalue = getvalue + "," + val;
                            singleparticcomp.Remove(Convert.ToString(actionname));
                            if (getvalue.Trim() != "")
                            {
                                singleparticcomp.Add(Convert.ToString(actionname), Convert.ToString(getvalue));
                            }
                        }

                    }
                }
                ViewState["CurrentTableparticcomp"] = singleparticcomp;

                for (int i = 0; i < GridView9.Rows.Count; i++)
                {
                    count5++;
                    TextBox txtamt = (TextBox)GridView9.Rows[i].FindControl("txtactname");
                    cname = Convert.ToString(txtamt.Text);
                    TextBox txtpanam = (TextBox)GridView9.Rows[i].FindControl("txt_per");
                    pname = Convert.ToString(txtpanam.Text);
                    TextBox txtadd = (TextBox)GridView9.Rows[i].FindControl("txt_add");
                    addr = Convert.ToString(txtadd.Text);
                    TextBox txtst = (TextBox)GridView9.Rows[i].FindControl("txt_st");
                    street = Convert.ToString(txtst.Text);
                    TextBox txtcity = (TextBox)GridView9.Rows[i].FindControl("txt_city");
                    city = Convert.ToString(txtcity.Text);
                    TextBox txtpin = (TextBox)GridView9.Rows[i].FindControl("txt_pin");
                    pin = Convert.ToString(txtpin.Text);
                    TextBox txtcou = (TextBox)GridView9.Rows[i].FindControl("txt_country");
                    country = Convert.ToString(txtcou.Text);
                    TextBox txtstate = (TextBox)GridView9.Rows[i].FindControl("txt_state");
                    state = Convert.ToString(txtstate.Text);
                    TextBox txtphn = (TextBox)GridView9.Rows[i].FindControl("txt_phn");
                    phn = Convert.ToString(txtphn.Text);
                    TextBox txtmail = (TextBox)GridView9.Rows[i].FindControl("txt_mail");
                    mail = Convert.ToString(txtmail.Text);
                    TextBox txtattch = (TextBox)GridView9.Rows[i].FindControl("txt_attch");
                    attch = Convert.ToString(txtattch.Text);
                    TextBox txtat = (TextBox)GridView9.Rows[i].FindControl("txt_e");
                    int at = Convert.ToInt32(txtat.Text);
                    TextBox txtdoc = (TextBox)GridView9.Rows[i].FindControl("txt_dt");
                    doc = Convert.ToString(txtdoc.Text);
                    val = cname + "-" + pname + "-" + addr + "-" + street + "-" + city + "-" + pin + "-" + country + "-" + state + "-" + phn + "-" + mail + "-" + attch + "-" + at + "-" + doc;
                    if (!singleparticindi.Contains(Convert.ToString(actionname)))
                    {

                        singleparticindi.Add(Convert.ToString(actionname), Convert.ToString(val));
                    }
                    else
                    {
                        string getvalue = Convert.ToString(singleparticindi[Convert.ToString(actionname)]);
                        if (getvalue.Trim() != "")
                        {
                            getvalue = getvalue + "," + val;
                            singleparticindi.Remove(Convert.ToString(actionname));
                            if (getvalue.Trim() != "")
                            {
                                singleparticindi.Add(Convert.ToString(actionname), Convert.ToString(getvalue));
                            }
                        }

                    }
                }
                ViewState["CurrentTableparticindi"] = singleparticindi;

                for (int k = 0; k < GridView10.Rows.Count; k++)
                {
                    count6++;

                    TextBox txtamtindi = (TextBox)GridView10.Rows[k].FindControl("txtactname");
                    cname = Convert.ToString(txtamtindi.Text);
                    TextBox txtpanam = (TextBox)GridView10.Rows[k].FindControl("txt_per");
                    pname = Convert.ToString(txtpanam.Text);
                    TextBox txtadd = (TextBox)GridView10.Rows[k].FindControl("txt_add");
                    addr = Convert.ToString(txtadd.Text);
                    TextBox txtst = (TextBox)GridView10.Rows[k].FindControl("txt_st");
                    street = Convert.ToString(txtst.Text);
                    TextBox txtcity = (TextBox)GridView10.Rows[k].FindControl("txt_city");
                    city = Convert.ToString(txtcity.Text);
                    TextBox txtpin = (TextBox)GridView10.Rows[k].FindControl("txt_pin");
                    pin = Convert.ToString(txtpin.Text);
                    TextBox txtcou = (TextBox)GridView10.Rows[k].FindControl("txt_country");
                    country = Convert.ToString(txtcou.Text);
                    TextBox txtstate = (TextBox)GridView10.Rows[k].FindControl("txt_state");
                    state = Convert.ToString(txtstate.Text);
                    TextBox txtphn = (TextBox)GridView10.Rows[k].FindControl("txt_phn");
                    phn = Convert.ToString(txtphn.Text);
                    TextBox txtmail = (TextBox)GridView10.Rows[k].FindControl("txt_mail");
                    mail = Convert.ToString(txtmail.Text);
                    TextBox txtattch = (TextBox)GridView10.Rows[k].FindControl("txtattch");
                    attch = Convert.ToString(txtattch.Text);
                    TextBox txtat = (TextBox)GridView10.Rows[k].FindControl("txt_e");
                    int at = Convert.ToInt32(txtat.Text);
                    TextBox txtdoc = (TextBox)GridView10.Rows[k].FindControl("txt_dt");
                    doc = Convert.ToString(txtdoc.Text);

                    val = cname + "-" + pname + "-" + addr + "-" + street + "-" + city + "-" + pin + "-" + country + "-" + state + "-" + phn + "-" + mail + "-" + attch + "-" + at + "-" + doc;
                    if (!singlepresentindi.Contains(Convert.ToString(actionname)))
                    {

                        singlepresentindi.Add(Convert.ToString(actionname), Convert.ToString(val));
                    }
                    else
                    {
                        string getvalue = Convert.ToString(singlepresentindi[Convert.ToString(actionname)]);
                        if (getvalue.Trim() != "")
                        {
                            getvalue = getvalue + "," + val;
                            singlepresentindi.Remove(Convert.ToString(actionname));
                            if (getvalue.Trim() != "")
                            {
                                singlepresentindi.Add(Convert.ToString(actionname), Convert.ToString(getvalue));
                            }
                        }

                    }

                }
                ViewState["CurrentTablesingleindi"] = singlepresentindi;

                for (int k = 0; k < GridView11.Rows.Count; k++)
                {
                    TextBox txtamt = (TextBox)GridView11.Rows[k].FindControl("txtactname");
                    cname = Convert.ToString(txtamt.Text);
                    TextBox txtpanam = (TextBox)GridView11.Rows[k].FindControl("txt_per");
                    pname = Convert.ToString(txtpanam.Text);
                    TextBox txtadd = (TextBox)GridView11.Rows[k].FindControl("txt_add");
                    addr = Convert.ToString(txtadd.Text);
                    TextBox txtst = (TextBox)GridView11.Rows[k].FindControl("txt_st");
                    street = Convert.ToString(txtst.Text);
                    TextBox txtcity = (TextBox)GridView11.Rows[k].FindControl("txt_city");
                    city = Convert.ToString(txtcity.Text);
                    TextBox txtpin = (TextBox)GridView11.Rows[k].FindControl("txt_pin");
                    pin = Convert.ToString(txtpin.Text);
                    TextBox txtcou = (TextBox)GridView11.Rows[k].FindControl("txt_country");
                    country = Convert.ToString(txtcou.Text);
                    TextBox txtstate = (TextBox)GridView11.Rows[k].FindControl("txt_state");
                    state = Convert.ToString(txtstate.Text);
                    TextBox txtphn = (TextBox)GridView11.Rows[k].FindControl("txt_phn");
                    phn = Convert.ToString(txtphn.Text);
                    TextBox txtmail = (TextBox)GridView11.Rows[k].FindControl("txt_mail");
                    mail = Convert.ToString(txtmail.Text);
                    TextBox txtattch = (TextBox)GridView11.Rows[k].FindControl("txt_attch");
                    attch = Convert.ToString(txtattch.Text);
                    TextBox txtat = (TextBox)GridView11.Rows[k].FindControl("txt_e");
                    int at = Convert.ToInt32(txtat.Text);
                    TextBox txtdoc = (TextBox)GridView11.Rows[k].FindControl("txt_dt");
                    doc = Convert.ToString(txtdoc.Text);
                    count7++;
                    val = cname + "-" + pname + "-" + addr + "-" + street + "-" + city + "-" + pin + "-" + country + "-" + state + "-" + phn + "-" + mail + "-" + attch + "-" + at + "-" + doc;
                    if (!singlepresentcomp.Contains(Convert.ToString(actionname)))
                    {

                        singlepresentcomp.Add(Convert.ToString(actionname), Convert.ToString(val));
                    }
                    else
                    {
                        string getvalue = Convert.ToString(singlepresentcomp[Convert.ToString(actionname)]);
                        if (getvalue.Trim() != "")
                        {
                            getvalue = getvalue + "," + val;
                            singlepresentcomp.Remove(Convert.ToString(actionname));
                            if (getvalue.Trim() != "")
                            {
                                singlepresentcomp.Add(Convert.ToString(actionname), Convert.ToString(getvalue));
                            }
                        }

                    }
                }
                ViewState["CurrentTablesinglecomp"] = singlepresentcomp;
                if (rdo_single.Checked == true)
                {
                    int totcount = count + count1 + count4 + count5;
                    int totcountprs = count2 + count3 + count6 + count7;
                    foreach (GridViewRow row1 in gridadd.Rows)
                    {
                        if (jj == row1.DataItemIndex)
                        {
                            if (ddl_act_namenew.SelectedItem.Value != "Select")
                            {
                                if (ddl_act_namenew.SelectedItem.Value != "Others")
                                {
                                    actionvalue = Convert.ToString(ddl_act_namenew.SelectedItem.Text);
                                }
                                else
                                {
                                    actionvalue = Convert.ToString(txt_act_namenew.Text);

                                }
                            }


                            TextBox txtactionn = (TextBox)gridadd.Rows[jj].FindControl("txtactname");
                            txtactionn.Text = actionvalue;
                            TextBox txtdes = (TextBox)gridadd.Rows[jj].FindControl("txt_descri");
                            txtdes.Text = description;
                            TextBox txtloc = (TextBox)gridadd.Rows[jj].FindControl("txt_loc");
                            txtloc.Text = location;
                            TextBox txtpart = (TextBox)gridadd.Rows[jj].FindControl("txt_noact");
                            txtpart.Text = Convert.ToString(totcount);
                            TextBox txtperst = (TextBox)gridadd.Rows[jj].FindControl("txt_noconper");
                            txtperst.Text = Convert.ToString(totcountprs);

                        }
                    }


                }
            }

            else
            {
                addevent();
            }

            spd_clear();
            gridclear_presented();
            txt_act_description.Text = "";

            loadnewaction();

            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                cbl_buildname.Items[i].Selected = false;
                txt_buildingname.Text = "--Select--";
                cb_buildname.Checked = false;
            }
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                cbl_floorname.Items[i].Selected = false;
                txt_floorname.Text = "--Select--";
                cb_floorname.Checked = false;
            }

            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                cbl_roomname.Items[i].Selected = false;
                txt_roomname.Text = "--Select--";
                cb_roomname.Checked = false;
            }
            if (rdo_indivparti.Checked == true)
            {
                GridView9.Visible = false;
                GridView8.Visible = false;
                GridView9.DataSource = null;
                GridView9.DataBind();
                GridView8.DataSource = null;
                GridView8.DataBind();
                ViewState["CurrentTable22"] = null;
                ViewState["CurrentTable11"] = null;
            }

            ViewState["CurrentTable33"] = null;
            ViewState["CurrentTable44"] = null;

            //ViewState["CurrentTablesingleindi"] = null;
            //ViewState["CurrentTablesinglecomp"] = null;
            GridView10.DataSource = null;
            GridView11.DataBind();
            GridView11.DataSource = null;
            GridView11.DataBind();
            poprdoview.Visible = false;
        }
        catch
        {
        }
    }
    public void txt_min_startdate_changed(object sender, EventArgs e)
    {
        string dt = txt_min_startdate.Text;
        string[] Split = dt.Split('/');
        DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        string enddt = DateTime.Now.ToString("dd/MM/yyyy");
        Split = enddt.Split('/');
        DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        if (fromdate > todate)
        {

            txt_min_startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            imgdiv2.Visible = true;
            lbl_alert.Text = "Kindly Select Valid Date";

        }
        else
        {
            if (rdo_single.Checked == true)
            {
                txt_pre_Startdate.Text = Convert.ToString(txt_min_startdate.Text);
                txt_pre_enddate.Text = Convert.ToString(txt_min_startdate.Text);
                txt_mat_expect.Text = Convert.ToString(txt_min_startdate.Text);
                txt_pre_Startdate.Enabled = false;
                txt_pre_enddate.Enabled = false;
                txt_mat_expect.Enabled = false;
            }
        }
    }
    public void btn_prerequest_addnew_Click(object sender, EventArgs e)
    {
        if (rdo_single.Checked == true)
        {
            txt_pre_Startdate.Text = Convert.ToString(txt_min_startdate.Text);
            txt_pre_enddate.Text = Convert.ToString(txt_min_startdate.Text);
            txt_pre_Startdate.Enabled = false;
            txt_pre_enddate.Enabled = false;
        }
        else
        {

        }
        divprte.Visible = true;
        reloadothers();
    }
    public void txt_pre_Startdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string dt = txt_pre_Startdate.Text;
            string[] Split = dt.Split('/');
            DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            string enddt = DateTime.Now.ToString("dd/MM/yyyy");
            Split = enddt.Split('/');
            DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);

            if (fromdate > todate)
            {

                txt_pre_Startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            }
        }
        catch (Exception ex)
        {

        }
    }
    public void txt_pre_enddate_TextChanged(object sender, EventArgs e)
    {
        string dt = txt_pre_Startdate.Text;
        string[] Split = dt.Split('/');
        DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        string enddt = txt_pre_enddate.Text;
        Split = enddt.Split('/');

        DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        TimeSpan days = fromdate - todate;
        string ndate = Convert.ToString(days);
        Split = ndate.Split('.');
        string getdate = Split[0];
        int finaldate = Convert.ToInt32(getdate);
        if (fromdate > todate)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Kindly Select Valid Date";
        }
    }
    public void rdo_pre_staff_CheckedChanged(object sender, EventArgs e)
    {
        if (rdo_pre_staff.Checked == true)
        {
            txt_pre_repby.Visible = true;
            txt_pre_repbystud.Visible = false;
        }
        else
        {
            txt_pre_repby.Visible = false;
            txt_pre_repbystud.Visible = true;
        }
    }

    public void rdo_pre_stud_CheckedChanged(object sender, EventArgs e)
    {
        if (rdo_pre_stud.Checked == true)
        {
            txt_pre_repby.Visible = false;
            txt_pre_repbystud.Visible = true;
        }
        else
        {
            txt_pre_repby.Visible = true;
            txt_pre_repbystud.Visible = false;
        }
    }
    public void btn_pre_add_Click(object sender, EventArgs e)
    {
        int rowIndex = 0;
        string act = Convert.ToString(txt_pre_action.Text);
        string start = Convert.ToString(txt_pre_Startdate.Text);
        string end = Convert.ToString(txt_pre_enddate.Text);
        string actionname = Convert.ToString(txt_pre_ctname.Text);
        string rep = "";
        if (rdo_pre_stud.Checked == true)
        {
            string[] ar = txt_pre_repbystud.Text.Split('-');
            rep = ar[0];
        }
        else
        {
            string[] ar = txt_pre_repby.Text.Split('-');
            rep = ar[0];
        }
        if (rdo_pre_staff.Checked == true)
        {
            if (txt_pre_action.Text == "" || txt_pre_ctname.Text == "" || txt_pre_repby.Text == "")
            {
                divdown.Visible = true;

                lbl_divdown1.Text = "Fill All The Data";
                return;
            }
        }
        else
        {
            if (txt_pre_action.Text == "" || txt_pre_ctname.Text == "" || txt_pre_repbystud.Text == "")
            {
                divdown.Visible = true;

                lbl_divdown1.Text = "Fill All The Data";
                return;
            }
        }

        if (ViewState["CurrentTable111"] != null)
        {

            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable111"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                TextBox box1 = new TextBox();
                TextBox box2 = new TextBox();
                TextBox box3 = new TextBox();
                TextBox box4 = new TextBox();
                TextBox box5 = new TextBox();

                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {

                    box1 = (TextBox)GridView4.Rows[i].Cells[1].FindControl("txtact");
                    box2 = (TextBox)GridView4.Rows[i].Cells[2].FindControl("txt_startdate");
                    box3 = (TextBox)GridView4.Rows[i].Cells[3].FindControl("txt_enddate");
                    box4 = (TextBox)GridView4.Rows[i].Cells[4].FindControl("txt_actname");
                    box5 = (TextBox)GridView4.Rows[i].Cells[5].FindControl("txt_repsen");

                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i][0] = box1.Text;
                    dtCurrentTable.Rows[i][1] = box2.Text;
                    dtCurrentTable.Rows[i][2] = box3.Text;
                    dtCurrentTable.Rows[i][3] = box4.Text;
                    dtCurrentTable.Rows[i][4] = box5.Text;

                    rowIndex++;

                }
                drCurrentRow[0] = act;
                drCurrentRow[1] = start;
                drCurrentRow[2] = end;
                drCurrentRow[3] = actionname;
                drCurrentRow[4] = rep;
                dtCurrentTable.Rows.Add(drCurrentRow);

                ViewState["CurrentTable111"] = dtCurrentTable;

                GridView4.DataSource = dtCurrentTable;
                GridView4.DataBind();
            }
        }
        else
        {
            bindgridview4();
        }

        reloadothers();
        txt_pre_action.Text = "";
        txt_pre_ctname.Text = "";
        txt_pre_repby.Text = "";
        txt_pre_Startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_pre_enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");

    }
    public void bindgridview4()
    {
        GridView4_div.Visible = true;
        GridView4.Visible = true;
        string act = Convert.ToString(txt_pre_action.Text);
        string start = Convert.ToString(txt_pre_Startdate.Text);
        string end = Convert.ToString(txt_pre_enddate.Text);
        string actionname = Convert.ToString(txt_pre_ctname.Text);
        string rep = "";
        if (rdo_pre_stud.Checked == true)
        {
            string[] ar = txt_pre_repbystud.Text.Split('-');
            rep = ar[0];
        }
        else
        {
            string[] ar = txt_pre_repby.Text.Split('-');
            rep = ar[0];
        }
        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");
        dt.Columns.Add("Dummy4");

        DataRow dr;

        dr = dt.NewRow();
        dr[0] = act;
        dr[1] = start;
        dr[2] = end;
        dr[3] = actionname;
        dr[4] = rep;

        dt.Rows.Add(dr);
        ViewState["CurrentTable111"] = dt;
        if (dt.Rows.Count > 0)
        {
            GridView4.DataSource = dt;
            GridView4.DataBind();
        }


    }
    public void btn_mat_go_Click(object sender, EventArgs e)
    {
        string itemname = Convert.ToString(txt_mat_itemname.Text);
        string quantity = Convert.ToString(txt_mat_qunty.Text);
        string expect = Convert.ToString(txt_mat_expect.Text);
        string ststus = "";
        if (rdo_usepur.Checked == true)
        {
            ststus = "Use Existing";
        }
        else if (rdo_tobepur.Checked == true)
        {
            ststus = "To Be Purchase";
        }
        int rowIndex = 0;
        if (txt_mat_itemname.Text != "" && txt_mat_qunty.Text != "")
        {
            if (ViewState["CurrentTable222"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable222"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    TextBox box1 = new TextBox();
                    TextBox box2 = new TextBox();
                    TextBox box3 = new TextBox();
                    TextBox box4 = new TextBox();
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {

                        box1 = (TextBox)GridView5.Rows[i].Cells[1].FindControl("txt_name");
                        box2 = (TextBox)GridView5.Rows[i].Cells[2].FindControl("txt_qty");
                        box3 = (TextBox)GridView5.Rows[i].Cells[3].FindControl("txt_exp");
                        box4 = (TextBox)GridView5.Rows[i].Cells[4].FindControl("tx_inmax");
                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i][0] = box1.Text;
                        dtCurrentTable.Rows[i][1] = box2.Text;
                        dtCurrentTable.Rows[i][2] = box3.Text;
                        dtCurrentTable.Rows[i][3] = box4.Text;

                        rowIndex++;

                    }
                    drCurrentRow[0] = itemname;
                    drCurrentRow[1] = quantity;
                    drCurrentRow[2] = expect;
                    drCurrentRow[3] = ststus;

                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable222"] = dtCurrentTable;

                    GridView5.DataSource = dtCurrentTable;
                    GridView5.DataBind();
                }
            }
            else
            {
                bindgridview5();
            }
        }
        else
        {
            divdown.Visible = true;
            lbl_divdown1.Text = "Fill All The Column";
        }
        reloadothers();

        txt_mat_itemname.Text = "";
        txt_mat_qunty.Text = "";
        txt_mat_expect.Text = DateTime.Now.ToString("dd/MM/yyyy");
        rdo_tobepur.Checked = true;
    }

    public void bindgridview5()
    {
        GridView5_div.Visible = true;
        GridView5.Visible = true;
        string itemname = Convert.ToString(txt_mat_itemname.Text);
        string quantity = Convert.ToString(txt_mat_qunty.Text);
        string expect = Convert.ToString(txt_mat_expect.Text);
        string ststus = "";
        if (rdo_usepur.Checked == true)
        {
            ststus = "Use Existing";
        }
        else if (rdo_tobepur.Checked == true)
        {
            ststus = "To Be Purchase";
        }

        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");

        DataRow dr;

        dr = dt.NewRow();
        dr[0] = itemname;
        dr[1] = quantity;
        dr[2] = expect;
        dr[3] = ststus;


        dt.Rows.Add(dr);
        ViewState["CurrentTable222"] = dt;
        if (dt.Rows.Count > 0)
        {
            GridView5.DataSource = dt;
            GridView5.DataBind();
        }


    }

    public void rdbcompany_CheckedChanged(object sender, EventArgs e)
    {
        gv33div.Visible = false;
        POP_GV3_DIV.Visible = false;
        POP_GV4_DIV.Visible = false;
        POP_GV6_DIV.Visible = true;
    }
    public void btn_ex_ad_Click(object sender, EventArgs e)
    {
        ex_new_div.Visible = true;
        reloadothers();
    }
    public void btn_sp_add_Click(object sender, EventArgs e)
    {
        int rowIndex = 0;
        string name = Convert.ToString(txt_spn_cmpy.Text);
        string res = Convert.ToString(txt_sp_cont.Text);
        string amt = Convert.ToString(txt_sp_amt.Text);
        if (ViewState["CurrentTable777"] != null)
        {

            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable777"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                TextBox box1 = new TextBox();
                TextBox box2 = new TextBox();
                TextBox box3 = new TextBox();


                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {

                    box1 = (TextBox)GridView6.Rows[i].Cells[1].FindControl("txtcname");
                    box2 = (TextBox)GridView6.Rows[i].Cells[2].FindControl("txtcnt");
                    box3 = (TextBox)GridView6.Rows[i].Cells[3].FindControl("txtamt1");

                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i][0] = box1.Text;
                    dtCurrentTable.Rows[i][1] = box2.Text;
                    dtCurrentTable.Rows[i][2] = box3.Text;


                    rowIndex++;

                }
                drCurrentRow[0] = name;
                drCurrentRow[1] = res;
                drCurrentRow[2] = amt;

                dtCurrentTable.Rows.Add(drCurrentRow);

                ViewState["CurrentTable777"] = dtCurrentTable;

                GridView6.DataSource = dtCurrentTable;
                GridView6.DataBind();
            }
        }
        else
        {
            BindGridview6();
        }
        txt_spn_cmpy.Text = "";
        txt_sp_cont.Text = "";
        txt_sp_amt.Text = "";
        All_dropdownchange();
    }
    public void BindGridview6()
    {
        GridView6.Visible = true;
        string name = Convert.ToString(txt_spn_cmpy.Text);
        string res = Convert.ToString(txt_sp_cont.Text);
        string amt = Convert.ToString(txt_sp_amt.Text);
        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");

        DataRow dr;

        dr = dt.NewRow();
        dr[0] = name;
        dr[1] = res;
        dr[2] = amt;


        dt.Rows.Add(dr);
        ViewState["CurrentTable777"] = dt;

        GridView6.DataSource = dt;
        GridView6.DataBind();
    }


    public void btn_itemlkup_Click(object sender, EventArgs e)
    {
        itemnamediv.Visible = true;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getitemname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();

        string query = "select distinct ItemName from IM_ItemMaster WHERE ItemName like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["ItemName"].ToString());
            }
        }
        return name;
    }

    protected void cb_itemname_CheckedChange(object sender, EventArgs e)
    {
        try
        {

            if (cb_itemname.Checked == true)
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = true;
                }
                txt_itemname.Text = "Items(" + (cbl_itemname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = false;
                }
                txt_itemname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_itemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_itemname.Text = "--Select--";
            cb_itemname.Checked = false;
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_itemname.Text = "Items(" + commcount.ToString() + ")";
                if (commcount == cbl_itemname.Items.Count)
                {
                    cb_branch1.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ViewState["selecteditems"] != null)
            //{
            //    DataTable dnew = (DataTable)ViewState["selecteditems"];
            //    ViewState["sb"] = dnew;
            //    checknew = "s";
            //}

            string itemheadercode = "";
            for (int i = 0; i < cbl_itm_hdrname.Items.Count; i++)
            {
                if (cbl_itm_hdrname.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_itm_hdrname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_itm_hdrname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemheadercode1 = "";
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    if (itemheadercode1 == "")
                    {
                        itemheadercode1 = "" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode1 = itemheadercode1 + "'" + "," + "'" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string selectquery = "";
            if (txt_itemsearch.Text.Trim() != "")
            {
                //selectquery = "select itemheader_name,itemheader_code,item_code,item_name ,model_name,Size_name ,item_unit,description ,special_instru from Item_Master where item_name='" + txt_searchby.Text + "' order by item_code";
                selectquery = "select ItemHeaderName,ItemHeaderCode,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit  from IM_ItemMaster where ItemName='" + txt_itemsearch.Text + "' order by ItemCode";
            }


            else if (itemheadercode.Trim() != "" && itemheadercode1.Trim() != "")
            {
                //selectquery = "select distinct  item_code ,item_name , itemheader_code,itemheader_name,item_unit from item_master where itemheader_code in ('" + itemheadercode + "') and item_code in ('" + itemheadercode1 + "') order by item_code ";
                selectquery = "select ItemHeaderName,ItemHeaderCode,ItemCode,ItemName ,ItemModel,ItemSize ,ItemUnit  from IM_ItemMaster where ItemHeaderCode in ('" + itemheadercode + "') and ItemCode in ('" + itemheadercode1 + "') order by ItemCode ";
            }

            if (txt_itn_hdr.Text.Trim() != "--Select--" && txt_itemname.Text.Trim() != "--Select--")
            {
                if (selectquery.Trim() != "")
                {
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataList1.DataSource = ds.Tables[0];
                        DataList1.DataBind();
                        DataList1.Visible = true;
                        div4.Visible = true;
                        btn_ok1.Visible = true;
                        btn_exit3.Visible = true;
                    }
                }
            }
            else
            {
                Label12.Visible = true;
                Label12.Text = "Please select all fields";
                div4.Visible = false;
                btn_ok1.Visible = false;
                btn_exit3.Visible = false;

            }
            txt_itemsearch.Text = "";
            All_dropdownchange();
        }
        catch
        {

        }

    }
    public void ImageButton5_Click(object sender, EventArgs e)
    {
        itemnamediv.Visible = false;
    }
    public void btn_ok1_Click(object sender, EventArgs e)
    {
        string name = "";
        string itemname = "";
        string itemcode = "";
        int count = 0;
        foreach (DataListItem gvrow in DataList1.Items)
        {
            CheckBox chkSelect = (gvrow.FindControl("CheckBox2") as CheckBox);
            if (chkSelect.Checked == true)
            {
                // chkSelect.Enabled = false;
                count++;

                Label lbl_itemname = (Label)gvrow.FindControl("lbl_itemname");
                itemname = lbl_itemname.Text;
                if (name == "")
                {
                    name = itemname;
                }
                else
                {
                    name = name + "-" + itemname;

                }
                txt_mat_itemname.Text = name;
            }
        }
        itemnamediv.Visible = false;

    }
    public void btn_exit3_Click(object sender, EventArgs e)
    {
        itemnamediv.Visible = false;
    }


    public void cb_itm_hdrname_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_itn_hdr.Text = "--Select--";
        if (cbb_itm_hdrname.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_itm_hdrname.Items.Count; i++)
            {
                cbl_itm_hdrname.Items[i].Selected = true;
            }
            txt_itn_hdr.Text = "Item Header(" + (cbl_itm_hdrname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_itm_hdrname.Items.Count; i++)
            {
                cbl_itm_hdrname.Items[i].Selected = false;
            }
        }

        itemmaster1();
    }
    public void cb_itm_hdrname_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;

        int commcount = 0;
        txt_itn_hdr.Text = "--Select--";
        for (i = 0; i < cbl_itm_hdrname.Items.Count; i++)
        {
            if (cbl_itm_hdrname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cbb_itm_hdrname.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_itm_hdrname.Items.Count)
            {
                cbb_itm_hdrname.Checked = true;
            }
            txt_itn_hdr.Text = "Item Header(" + commcount.ToString() + ")";
        }

        itemmaster1();
    }
    public void cb_item_subhdr_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_item_subhdr.Checked == true)
            {
                for (int i = 0; i < cbl_item_subhdr.Items.Count; i++)
                {
                    cbl_item_subhdr.Items[i].Selected = true;
                }
                txt_subhdrname.Text = "Sub Header Name(" + (cbl_item_subhdr.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_item_subhdr.Items.Count; i++)
                {
                    cbl_item_subhdr.Items[i].Selected = false;
                }
                txt_subhdrname.Text = "--Select--";
            }
            // loadsubheadername();
            itemmaster1();

        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_item_subhdr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_subhdrname.Text = "--Select--";
            cb_item_subhdr.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_item_subhdr.Items.Count; i++)
            {
                if (cbl_item_subhdr.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_subhdrname.Text = "Sub Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_item_subhdr.Items.Count)
                {
                    cb_item_subhdr.Checked = true;
                }
            }
            itemmaster1();
        }
        catch (Exception ex)
        {
        }
    }
    public void itemmaster1()
    {
        cbl_itemname.Items.Clear();
        string itemheadercode = "";
        string subheader = "";
        for (int i = 0; i < cbl_itm_hdrname.Items.Count; i++)
        {
            if (cbl_itm_hdrname.Items[i].Selected == true)
            {
                if (itemheadercode == "")
                {
                    itemheadercode = "" + cbl_itm_hdrname.Items[i].Value.ToString() + "";
                }
                else
                {
                    itemheadercode = itemheadercode + "'" + "," + "'" + cbl_itm_hdrname.Items[i].Value.ToString() + "";
                }
            }
        }
        for (int i = 0; i < cbl_item_subhdr.Items.Count; i++)
        {
            if (cbl_item_subhdr.Items[i].Selected == true)
            {
                if (subheader == "")
                {
                    subheader = "" + cbl_item_subhdr.Items[i].Value.ToString() + "";
                }
                else
                {
                    subheader = subheader + "'" + "," + "" + "'" + cbl_item_subhdr.Items[i].Value.ToString() + "";
                }
            }
        }
        if (itemheadercode.Trim() != "" && subheader.Trim() != "")
        {
            // ds.Clear();
            //  ds = d2.BindItemCodewithsubheader(itemheadercode, subheader);
            string query = "";
            query = "select distinct ItemCode,ItemName  from IM_ItemMaster where ItemHeaderCode in ('" + itemheadercode + "') and subheader_code in ('" + subheader + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");

            //if (itemheadercode.Trim() != "")
            //{
            //    ds.Clear();
            //    ds = d2.BindItemCode(itemheadercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_itemname.DataSource = ds;
                cbl_itemname.DataTextField = "ItemName";
                cbl_itemname.DataValueField = "ItemCode";
                cbl_itemname.DataBind();

                if (cbl_itemname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_itemname.Items.Count; i++)
                    {

                        cbl_itemname.Items[i].Selected = true;
                    }
                    txt_itemname.Text = "Item Name(" + cbl_itemname.Items.Count + ")";
                }
            }
            else
            {
                txt_itemname.Text = "--Select--";
            }
        }
        else
        {
            txt_itemname.Text = "--Select--";
        }
    }

    public void bindgridview7()
    {

        GridView7.Visible = true;
        string itemname = "";
        if (ddl_expnc_name.SelectedItem.Text != "Select" && ddl_expnc_name.SelectedItem.Text == "Others")
        {
            itemname = Convert.ToString(txt_expn_name.Text);
        }
        else if (ddl_expnc_name.SelectedItem.Text != "Select" && ddl_expnc_name.SelectedItem.Text != "Others")
        {
            itemname = Convert.ToString(ddl_expnc_name.SelectedItem.Text);
        }
        else
        {
            itemname = "";
        }
        string quantity = Convert.ToString(txt_expnc_descrp.Text);
        string expect = Convert.ToString(txt_expnce_amt.Text);


        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");

        DataRow dr;

        dr = dt.NewRow();
        dr[0] = itemname;
        dr[1] = quantity;
        dr[2] = expect;



        dt.Rows.Add(dr);
        ViewState["CurrentTable333"] = dt;
        if (dt.Rows.Count > 0)
        {
            GridView7.DataSource = dt;
            GridView7.DataBind();
        }


    }
    public void bt_exp_addnew_Click(object sender, EventArgs e)
    {
        int rowIndex = 0;
        string itemname = "";
        if (ddl_expnc_name.SelectedItem.Text != "Select" && ddl_expnc_name.SelectedItem.Text == "Others")
        {
            itemname = Convert.ToString(txt_expn_name.Text);
        }
        else if (ddl_expnc_name.SelectedItem.Text != "Select" && ddl_expnc_name.SelectedItem.Text != "Others")
        {
            itemname = Convert.ToString(ddl_expnc_name.SelectedItem.Text);
        }
        else
        {
            itemname = "";
        }
        string quantity = Convert.ToString(txt_expnc_descrp.Text);
        string expect = Convert.ToString(txt_expnce_amt.Text);
        if (ddl_expnc_name.SelectedItem.Text != "Select" && txt_expnc_descrp.Text != "" && txt_expnce_amt.Text != "")
        {
            if (ViewState["CurrentTable333"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable333"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    TextBox box1 = new TextBox();
                    TextBox box2 = new TextBox();
                    TextBox box3 = new TextBox();

                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {

                        box1 = (TextBox)GridView7.Rows[i].Cells[1].FindControl("txtcname");
                        box2 = (TextBox)GridView7.Rows[i].Cells[2].FindControl("txtcnt");
                        box3 = (TextBox)GridView7.Rows[i].Cells[3].FindControl("txtamt1");

                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i][0] = box1.Text;
                        dtCurrentTable.Rows[i][1] = box2.Text;
                        dtCurrentTable.Rows[i][2] = box3.Text;

                        rowIndex++;

                    }
                    drCurrentRow[0] = itemname;
                    drCurrentRow[1] = quantity;
                    drCurrentRow[2] = expect;

                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable333"] = dtCurrentTable;

                    GridView7.DataSource = dtCurrentTable;
                    GridView7.DataBind();
                }
            }
            else
            {
                bindgridview7();
            }
        }
        else
        {

            divdown.Visible = true;
            lbl_divdown1.Text = "Fill All The Column";
        }
        txt_expnc_descrp.Text = "";
        txt_expnce_amt.Text = "";
        txt_expn_name.Text = "";
        All_dropdownchange();
        loadexpn();
        res();
        //ddl_expnc_name.SelectedItem.Text = "Select";
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getcompname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorCompName from CO_VendorMaster where VendorType=4 and VendorCompName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getindiname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorCompName from CO_VendorMaster where VendorType=5 and VendorCompName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getotherpername(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (persentedindi == "")
        {
            persentedindi = "0";
        }
        string query = "select VendorName from CO_VendorMaster where VendorType=7 and VendorName like '" + prefixText + "%' and  VendorName not in('" + persentedindi + "')";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getotherpername1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (particpentindi == "")
        {
            particpentindi = "0";
        }
        string query = "select VendorName from CO_VendorMaster where VendorType=7 and VendorName like '" + prefixText + "%' and VendorName not in('" + particpentindi + "')";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getassocname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorName from CO_VendorMaster where VendorType=8 and VendorName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    public void btn_go_staff_Click(object sender, EventArgs e)
    {
        pop_add_staff_stud_othr.Visible = false;
        string val = "";
        for (int i = 0; i < GridView2.Rows.Count; i++)
        {
            CheckBox checkvalue1 = (CheckBox)GridView2.Rows[i].FindControl("chkup3"); ;
            if (checkvalue1.Checked == true)
            {
                Label stud_appno = (Label)GridView2.Rows[i].FindControl("lblappno");
                val = Convert.ToString(stud_appno.Text);
                if (particpentstaff == "")
                {
                    particpentstaff = val;
                }
                else
                {
                    particpentstaff = particpentstaff + "'" + "," + "'" + val;
                }
            }
        }
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {

            CheckBox checkvalue1 = (CheckBox)GridView1.Rows[i].FindControl("chkup3");

            if (checkvalue1.Checked == true)
            {
                Label stud_appno = (Label)GridView1.Rows[i].FindControl("lblappno");
                val = Convert.ToString(stud_appno.Text);
                if (particpentstud == "")
                {
                    particpentstud = val;
                }
                else
                {
                    particpentstud = particpentstud + "'" + "," + "'" + val;
                }
            }

        }
        All_dropdownchange();
        indi_comp();
    }
    public void btn_go_prsntclik_Click(object sender, EventArgs e)
    {
        pop_add_staff_stud_othr1.Visible = false;
        string val1 = "";
        for (int i = 0; i < GridView12.Rows.Count; i++)
        {
            CheckBox checkvalue1 = (CheckBox)GridView12.Rows[i].FindControl("chkup3");
            if (checkvalue1.Checked == true)
            {
                Label stud_appno = (Label)GridView12.Rows[i].FindControl("lblappno");
                val1 = Convert.ToString(stud_appno.Text);
                if (persentedstaff == "")
                {
                    persentedstaff = val1;
                }
                else
                {
                    persentedstaff = persentedstaff + "'" + "," + "'" + val1;
                }
            }
        }

        for (int i = 0; i < GridView13.Rows.Count; i++)
        {
            CheckBox checkvalue1 = (CheckBox)GridView13.Rows[i].FindControl("chkup3");
            if (checkvalue1.Checked == true)
            {
                Label stud_appno = (Label)GridView13.Rows[i].FindControl("lblappno");
                val1 = Convert.ToString(stud_appno.Text);

                if (persentedstud == "")
                {
                    persentedstud = val1;
                }
                else
                {
                    persentedstud = persentedstud + "'" + "," + "'" + val1;
                }
            }
        }
        All_dropdownchange();
        indi_comp();
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getcompname1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorCompName from CO_VendorMaster where VendorType=4 and VendorCompName like '" + prefixText + "%' and VendorCompName not in('" + persentedcomp + "')";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getcompnameprst(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorCompName from CO_VendorMaster where VendorType=4 and VendorCompName like '" + prefixText + "%' and VendorCompName not in('" + particpentcomp + "')";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getcompnameper(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //string query = "select VenContactName from IM_VendorContactMaster where VenContactName like '" + prefixText + "%'";

        string query = "select VenContactName from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK AND VendorType='4' and VenContactName like '" + prefixText + "%'";

        name = ws.Getname(query);
        return name;
    }




    public void btn_addcompanydetails_Click(object sender, EventArgs e)
    {
        int rowIndex = 0;
        string name = Convert.ToString(TextBox1.Text);
        string per = Convert.ToString(TextBox2.Text);
        string add = Convert.ToString(TextBox3.Text);
        string street = Convert.ToString(TextBox4.Text);
        string city = Convert.ToString(TextBox8.Text);
        string pin = Convert.ToString(TextBox9.Text);
        string state = Convert.ToString(TextBox10.Text);
        string country = Convert.ToString(TextBox11.Text);
        string phnno = Convert.ToString(TextBox12.Text);
        string mail = Convert.ToString(TextBox13.Text);
        string fileName = "";
        int fileSize = 0;
        string documentType = string.Empty;
        if (FileUpload_part_comp_attch.HasFile)
        {
            if (FileUpload_part_comp_attch.FileName.EndsWith(".jpg") || FileUpload_part_comp_attch.FileName.EndsWith(".gif") || FileUpload_part_comp_attch.FileName.EndsWith(".png") || FileUpload_part_comp_attch.FileName.EndsWith(".txt") || FileUpload_part_comp_attch.FileName.EndsWith(".doc") || FileUpload_part_comp_attch.FileName.EndsWith(".xls") || FileUpload_part_comp_attch.FileName.EndsWith(".docx") || FileUpload_part_comp_attch.FileName.EndsWith(".txt") || FileUpload_part_comp_attch.FileName.EndsWith(".document") || FileUpload_part_comp_attch.FileName.EndsWith(".xls") || FileUpload_part_comp_attch.FileName.EndsWith(".xlsx") || FileUpload_part_comp_attch.FileName.EndsWith(".pdf") || FileUpload_part_comp_attch.FileName.EndsWith(".ppt") || FileUpload_part_comp_attch.FileName.EndsWith(".pptx"))
            {
                fileName = Path.GetFileName(FileUpload_part_comp_attch.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload_part_comp_attch.PostedFile.FileName);

                switch (fileExtension)
                {
                    case ".pdf":
                        documentType = "application/pdf";
                        break;

                    case ".xls":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".xlsx":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".doc":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".docx":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".gif":
                        documentType = "image/gif";
                        break;

                    case ".png":
                        documentType = "image/png";
                        break;

                    case ".jpg":
                        documentType = "image/jpg";
                        break;

                    case ".ppt":
                        documentType = "application/vnd.ms-ppt";
                        break;

                    case ".pptx":
                        documentType = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType = "application/txt";
                        break;
                }

                fileSize = FileUpload_part_comp_attch.PostedFile.ContentLength;

                byte[] documentBinary = new byte[fileSize];
                FileUpload_part_comp_attch.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
            }
        }
        if (TextBox1.Text != "" && TextBox2.Text != "")
        {
            lbl_emptyerror2.Visible = false;
            if (ViewState["CurrentTable11"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable11"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    TextBox box1 = new TextBox();
                    TextBox box2 = new TextBox();
                    TextBox box3 = new TextBox();
                    TextBox box4 = new TextBox();
                    TextBox box5 = new TextBox();
                    TextBox box6 = new TextBox();
                    TextBox box7 = new TextBox();
                    TextBox box8 = new TextBox();
                    TextBox box9 = new TextBox();
                    TextBox box10 = new TextBox();
                    TextBox box11 = new TextBox();
                    TextBox box12 = new TextBox();
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {

                        box1 = (TextBox)GridView8.Rows[i].Cells[1].FindControl("txtactname");
                        box2 = (TextBox)GridView8.Rows[i].Cells[2].FindControl("txt_per");
                        box3 = (TextBox)GridView8.Rows[i].Cells[3].FindControl("txt_add");
                        box4 = (TextBox)GridView8.Rows[i].Cells[4].FindControl("txt_st");
                        box5 = (TextBox)GridView8.Rows[i].Cells[5].FindControl("txt_city");
                        box6 = (TextBox)GridView8.Rows[i].Cells[6].FindControl("txt_pin");
                        box7 = (TextBox)GridView8.Rows[i].Cells[7].FindControl("txt_state");
                        box8 = (TextBox)GridView8.Rows[i].Cells[8].FindControl("txt_country");
                        box9 = (TextBox)GridView8.Rows[i].Cells[9].FindControl("txt_phn");
                        box10 = (TextBox)GridView8.Rows[i].Cells[10].FindControl("txt_mail");
                        box11 = (TextBox)GridView8.Rows[i].Cells[11].FindControl("txt_attch");
                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i][0] = box1.Text;
                        dtCurrentTable.Rows[i][1] = box2.Text;


                        dtCurrentTable.Rows[i][2] = box3.Text;

                        dtCurrentTable.Rows[i][3] = box4.Text;
                        dtCurrentTable.Rows[i][4] = box5.Text;
                        dtCurrentTable.Rows[i][5] = box6.Text;
                        dtCurrentTable.Rows[i][6] = box7.Text;
                        dtCurrentTable.Rows[i][7] = box8.Text;
                        dtCurrentTable.Rows[i][8] = box9.Text;
                        dtCurrentTable.Rows[i][9] = box10.Text;
                        dtCurrentTable.Rows[i][10] = box11.Text;
                        rowIndex++;
                    }

                    drCurrentRow[0] = name;
                    drCurrentRow[1] = per;
                    drCurrentRow[2] = add;
                    drCurrentRow[3] = street;
                    drCurrentRow[4] = city;
                    drCurrentRow[5] = pin;
                    drCurrentRow[6] = state;
                    drCurrentRow[7] = country;
                    drCurrentRow[8] = phnno;
                    drCurrentRow[9] = mail;
                    drCurrentRow[10] = fileName;
                    drCurrentRow[11] = fileSize;
                    drCurrentRow[12] = documentType;

                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable11"] = dtCurrentTable;

                    GridView8.DataSource = dtCurrentTable;
                    GridView8.DataBind();
                }
            }


            else
            {
                BindGridview8();
            }
        }
        else
        {
            lbl_emptyerror2.Visible = true;
            lbl_emptyerror2.Text = "Must Fill Company & Person Name";
        }
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";
        TextBox8.Text = "";
        TextBox9.Text = "";
        TextBox10.Text = "";
        TextBox11.Text = "";
        TextBox12.Text = "";
        TextBox12.Text = "";
        TextBox13.Text = "";
    }
    public void BindGridview8()
    {
        int rowIndex = 0;
        GridView8.Visible = true;

        DataTable dt = new DataTable();
        DataRow dr;
        int fileSize = 0;
        string documentType = string.Empty;
        string fileName = "";
        string name = Convert.ToString(TextBox1.Text);
        string per = Convert.ToString(TextBox2.Text);
        string add = Convert.ToString(TextBox3.Text);
        string street = Convert.ToString(TextBox4.Text);
        string city = Convert.ToString(TextBox8.Text);
        string pin = Convert.ToString(TextBox9.Text);
        string state = Convert.ToString(TextBox10.Text);
        string country = Convert.ToString(TextBox11.Text);
        string phnno = Convert.ToString(TextBox12.Text);
        string mail = Convert.ToString(TextBox13.Text);
        if (FileUpload_part_comp_attch.HasFile)
        {
            if (FileUpload_part_comp_attch.FileName.EndsWith(".jpg") || FileUpload_part_comp_attch.FileName.EndsWith(".gif") || FileUpload_part_comp_attch.FileName.EndsWith(".png") || FileUpload_part_comp_attch.FileName.EndsWith(".txt") || FileUpload_part_comp_attch.FileName.EndsWith(".doc") || FileUpload_part_comp_attch.FileName.EndsWith(".xls") || FileUpload_part_comp_attch.FileName.EndsWith(".docx") || FileUpload_part_comp_attch.FileName.EndsWith(".txt") || FileUpload_part_comp_attch.FileName.EndsWith(".document") || FileUpload_part_comp_attch.FileName.EndsWith(".xls") || FileUpload_part_comp_attch.FileName.EndsWith(".xlsx") || FileUpload_part_comp_attch.FileName.EndsWith(".pdf") || FileUpload_part_comp_attch.FileName.EndsWith(".ppt") || FileUpload_part_comp_attch.FileName.EndsWith(".pptx"))
            {
                fileName = Path.GetFileName(FileUpload_part_comp_attch.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload_part_comp_attch.PostedFile.FileName);

                switch (fileExtension)
                {
                    case ".pdf":
                        documentType = "application/pdf";
                        break;

                    case ".xls":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".xlsx":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".doc":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".docx":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".gif":
                        documentType = "image/gif";
                        break;

                    case ".png":
                        documentType = "image/png";
                        break;

                    case ".jpg":
                        documentType = "image/jpg";
                        break;

                    case ".ppt":
                        documentType = "application/vnd.ms-ppt";
                        break;

                    case ".pptx":
                        documentType = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType = "application/txt";
                        break;
                }

                fileSize = FileUpload_part_comp_attch.PostedFile.ContentLength;

                byte[] documentBinary = new byte[fileSize];
                FileUpload_part_comp_attch.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
            }
        }

        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");
        dt.Columns.Add("Dummy4");
        dt.Columns.Add("Dummy5");
        dt.Columns.Add("Dummy6");
        dt.Columns.Add("Dummy7");
        dt.Columns.Add("Dummy8");
        dt.Columns.Add("Dummy9");
        dt.Columns.Add("Dummy10");
        dt.Columns.Add("Dummy11");
        dt.Columns.Add("Dummy12");

        dr = dt.NewRow();
        dr[0] = name;
        dr[1] = per;
        dr[2] = add;
        dr[3] = street;
        dr[4] = city;
        dr[5] = pin;
        dr[6] = state;
        dr[7] = country;
        dr[8] = phnno;
        dr[9] = mail;
        dr[10] = fileName;
        dr[11] = fileSize;
        dr[12] = documentType;
        dt.Rows.Add(dr);
        rowIndex++;

        ViewState["CurrentTable11"] = dt;

        GridView8.DataSource = dt;
        GridView8.DataBind();
    }

    public void btn_add_indiadd_Click(object sender, EventArgs e)
    {

        int rowIndex = 0;
        string name = Convert.ToString(txt_cmpname1.Text);
        string per = Convert.ToString(txt_othr_pname1.Text);
        string add = Convert.ToString(tx_compadd.Text);
        string street = Convert.ToString(tx_comstr.Text);
        string city = Convert.ToString(txt_cmcity.Text);
        string pin = Convert.ToString(txt_cmpin.Text);
        string state = Convert.ToString(txt_cmstste.Text);
        string country = Convert.ToString(txt_cmcountry.Text);
        string phnno = Convert.ToString(txt_cmpho.Text);
        string mail = Convert.ToString(txt_cmmail.Text);
        string fileName = "";

        int fileSize = 0;
        string documentType = string.Empty;
        if (FileUpload1_part_indi_attch.HasFile)
        {
            if (FileUpload1_part_indi_attch.FileName.EndsWith(".jpg") || FileUpload1_part_indi_attch.FileName.EndsWith(".gif") || FileUpload1_part_indi_attch.FileName.EndsWith(".png") || FileUpload1_part_indi_attch.FileName.EndsWith(".txt") || FileUpload1_part_indi_attch.FileName.EndsWith(".doc") || FileUpload1_part_indi_attch.FileName.EndsWith(".xls") || FileUpload1_part_indi_attch.FileName.EndsWith(".docx") || FileUpload1_part_indi_attch.FileName.EndsWith(".txt") || FileUpload1_part_indi_attch.FileName.EndsWith(".document") || FileUpload1_part_indi_attch.FileName.EndsWith(".xls") || FileUpload1_part_indi_attch.FileName.EndsWith(".xlsx") || FileUpload1_part_indi_attch.FileName.EndsWith(".pdf") || FileUpload1_part_indi_attch.FileName.EndsWith(".ppt") || FileUpload1_part_indi_attch.FileName.EndsWith(".pptx"))
            {
                fileName = Path.GetFileName(FileUpload1_part_indi_attch.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload1_part_indi_attch.PostedFile.FileName);
                //string documentType = string.Empty;
                switch (fileExtension)
                {
                    case ".pdf":
                        documentType = "application/pdf";
                        break;

                    case ".xls":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".xlsx":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".doc":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".docx":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".gif":
                        documentType = "image/gif";
                        break;

                    case ".png":
                        documentType = "image/png";
                        break;

                    case ".jpg":
                        documentType = "image/jpg";
                        break;

                    case ".ppt":
                        documentType = "application/vnd.ms-ppt";
                        break;

                    case ".pptx":
                        documentType = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType = "application/txt";
                        break;
                }

                fileSize = FileUpload1_part_indi_attch.PostedFile.ContentLength;
                byte[] documentBinary = new byte[fileSize];
                FileUpload1_part_indi_attch.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

            }
        }
        if (txt_othr_pname1.Text != "" && txt_cmpname1.Text != "")
        {
            lbl_emptyerror1.Visible = false;
            if (ViewState["CurrentTable22"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable22"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    TextBox box1 = new TextBox();
                    TextBox box2 = new TextBox();
                    TextBox box3 = new TextBox();
                    TextBox box4 = new TextBox();
                    TextBox box5 = new TextBox();
                    TextBox box6 = new TextBox();
                    TextBox box7 = new TextBox();
                    TextBox box8 = new TextBox();
                    TextBox box9 = new TextBox();
                    TextBox box10 = new TextBox();
                    TextBox box11 = new TextBox();
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {

                        box1 = (TextBox)GridView9.Rows[i].Cells[1].FindControl("txtactname");
                        box2 = (TextBox)GridView9.Rows[i].Cells[2].FindControl("txt_per");
                        box3 = (TextBox)GridView9.Rows[i].Cells[3].FindControl("txt_add");
                        box4 = (TextBox)GridView9.Rows[i].Cells[4].FindControl("txt_st");
                        box5 = (TextBox)GridView9.Rows[i].Cells[5].FindControl("txt_city");
                        box6 = (TextBox)GridView9.Rows[i].Cells[6].FindControl("txt_pin");
                        box7 = (TextBox)GridView9.Rows[i].Cells[7].FindControl("txt_state");
                        box8 = (TextBox)GridView9.Rows[i].Cells[8].FindControl("txt_country");
                        box9 = (TextBox)GridView9.Rows[i].Cells[9].FindControl("txt_phn");
                        box10 = (TextBox)GridView9.Rows[i].Cells[10].FindControl("txt_mail");
                        box11 = (TextBox)GridView9.Rows[i].Cells[11].FindControl("txt_attch");
                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i][0] = box1.Text;
                        dtCurrentTable.Rows[i][1] = box2.Text;


                        dtCurrentTable.Rows[i][2] = box3.Text;

                        dtCurrentTable.Rows[i][3] = box4.Text;
                        dtCurrentTable.Rows[i][4] = box5.Text;
                        dtCurrentTable.Rows[i][5] = box6.Text;
                        dtCurrentTable.Rows[i][6] = box7.Text;
                        dtCurrentTable.Rows[i][7] = box8.Text;
                        dtCurrentTable.Rows[i][8] = box9.Text;
                        dtCurrentTable.Rows[i][9] = box10.Text;
                        dtCurrentTable.Rows[i][10] = box11.Text;
                        rowIndex++;
                    }

                    drCurrentRow[0] = name;
                    drCurrentRow[1] = per;
                    drCurrentRow[2] = add;
                    drCurrentRow[3] = street;
                    drCurrentRow[4] = city;
                    drCurrentRow[5] = pin;
                    drCurrentRow[6] = state;
                    drCurrentRow[7] = country;
                    drCurrentRow[8] = phnno;
                    drCurrentRow[9] = mail;
                    drCurrentRow[10] = fileName;
                    drCurrentRow[11] = fileSize;
                    drCurrentRow[12] = documentType;
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable22"] = dtCurrentTable;

                    GridView9.DataSource = dtCurrentTable;
                    GridView9.DataBind();
                }
            }
            else
            {

                BindGridview9();
            }

        }
        else
        {

            lbl_emptyerror1.Visible = true;
            lbl_emptyerror1.Text = "Must Fill Person and Institution Name";
        }
        txt_othr_pname1.Text = "";
        txt_cmpname1.Text = "";
        tx_compadd.Text = "";
        tx_comstr.Text = "";
        txt_cmcity.Text = "";
        txt_cmpin.Text = "";
        txt_cmstste.Text = "";
        txt_cmcountry.Text = "";
        txt_cmpho.Text = "";
        txt_cmmail.Text = "";
    }

    public void BindGridview9()
    {

        GridView9.Visible = true;
        string name = Convert.ToString(txt_cmpname1.Text);
        string per = Convert.ToString(txt_othr_pname1.Text);
        string add = Convert.ToString(tx_compadd.Text);
        string street = Convert.ToString(tx_comstr.Text);
        string city = Convert.ToString(txt_cmcity.Text);
        string pin = Convert.ToString(txt_cmpin.Text);
        string state = Convert.ToString(txt_cmstste.Text);
        string country = Convert.ToString(txt_cmcountry.Text);
        string phnno = Convert.ToString(txt_cmpho.Text);
        string mail = Convert.ToString(txt_cmmail.Text);
        string fileName = "";
        int fileSize = 0;
        string documentType = string.Empty;
        if (FileUpload1_part_indi_attch.HasFile)
        {
            if (FileUpload1_part_indi_attch.FileName.EndsWith(".jpg") || FileUpload1_part_indi_attch.FileName.EndsWith(".gif") || FileUpload1_part_indi_attch.FileName.EndsWith(".png") || FileUpload1_part_indi_attch.FileName.EndsWith(".txt") || FileUpload1_part_indi_attch.FileName.EndsWith(".doc") || FileUpload1_part_indi_attch.FileName.EndsWith(".xls") || FileUpload1_part_indi_attch.FileName.EndsWith(".docx") || FileUpload1_part_indi_attch.FileName.EndsWith(".txt") || FileUpload1_part_indi_attch.FileName.EndsWith(".document") || FileUpload1_part_indi_attch.FileName.EndsWith(".xls") || FileUpload1_part_indi_attch.FileName.EndsWith(".xlsx") || FileUpload1_part_indi_attch.FileName.EndsWith(".pdf") || FileUpload1_part_indi_attch.FileName.EndsWith(".ppt") || FileUpload1_part_indi_attch.FileName.EndsWith(".pptx"))
            {
                fileName = Path.GetFileName(FileUpload1_part_indi_attch.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload1_part_indi_attch.PostedFile.FileName);
                //string documentType = string.Empty;
                switch (fileExtension)
                {
                    case ".pdf":
                        documentType = "application/pdf";
                        break;

                    case ".xls":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".xlsx":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".doc":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".docx":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".gif":
                        documentType = "image/gif";
                        break;

                    case ".png":
                        documentType = "image/png";
                        break;

                    case ".jpg":
                        documentType = "image/jpg";
                        break;

                    case ".ppt":
                        documentType = "application/vnd.ms-ppt";
                        break;

                    case ".pptx":
                        documentType = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType = "application/txt";
                        break;
                }

                fileSize = FileUpload1_part_indi_attch.PostedFile.ContentLength;
                byte[] documentBinary = new byte[fileSize];
                FileUpload1_part_indi_attch.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

            }
        }
        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");
        dt.Columns.Add("Dummy4");
        dt.Columns.Add("Dummy5");
        dt.Columns.Add("Dummy6");
        dt.Columns.Add("Dummy7");
        dt.Columns.Add("Dummy8");
        dt.Columns.Add("Dummy9");
        dt.Columns.Add("Dummy10");
        dt.Columns.Add("Dummy11");
        dt.Columns.Add("Dummy12");
        DataRow dr;

        dr = dt.NewRow();
        dr[0] = name;
        dr[1] = per;
        dr[2] = add;
        dr[3] = street;
        dr[4] = city;
        dr[5] = pin;
        dr[6] = state;
        dr[7] = country;
        dr[8] = phnno;
        dr[9] = mail;
        dr[10] = fileName;
        dr[11] = fileSize;
        dr[12] = documentType;
        dt.Rows.Add(dr);
        ViewState["CurrentTable22"] = dt;
        GridView9.DataSource = dt;
        GridView9.DataBind();
    }

    public void btn_add_prsn_indi_Click(object sender, EventArgs e)
    {
        int rowIndex = 0;
        string name = Convert.ToString(txt_othr_pname.Text);
        string per = Convert.ToString(txt_othr_pname.Text);
        string add = Convert.ToString(txt_othr_add.Text);
        string street = Convert.ToString(txt_othr_str.Text);
        string city = Convert.ToString(txt_othr_city.Text);
        string pin = Convert.ToString(txt_othr_pin.Text);
        string state = Convert.ToString(txt_othr_state.Text);
        string country = Convert.ToString(txt_othr_county.Text);
        string phnno = Convert.ToString(txt_othr_ph.Text);
        string mail = Convert.ToString(txt_othr_mail.Text);
        int fileSize = 0;
        string fileName = "";
        string documentType = string.Empty;
        if (FileUpload_prsnt_ind_atch.HasFile)
        {
            if (FileUpload_prsnt_ind_atch.FileName.EndsWith(".jpg") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".gif") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".png") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".txt") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".doc") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".xls") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".docx") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".txt") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".document") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".xls") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".xlsx") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".pdf") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".ppt") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".pptx"))
            {
                fileName = Path.GetFileName(FileUpload_prsnt_ind_atch.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload_prsnt_ind_atch.PostedFile.FileName);

                switch (fileExtension)
                {
                    case ".pdf":
                        documentType = "application/pdf";
                        break;

                    case ".xls":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".xlsx":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".doc":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".docx":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".gif":
                        documentType = "image/gif";
                        break;

                    case ".png":
                        documentType = "image/png";
                        break;

                    case ".jpg":
                        documentType = "image/jpg";
                        break;

                    case ".ppt":
                        documentType = "application/vnd.ms-ppt";
                        break;

                    case ".pptx":
                        documentType = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType = "application/txt";
                        break;
                }

                fileSize = FileUpload_prsnt_ind_atch.PostedFile.ContentLength;

                byte[] documentBinary = new byte[fileSize];
                FileUpload_prsnt_ind_atch.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

            }
        }
        if (txt_othr_pname.Text != "" && txt_othr_name.Text != "")
        {
            lbl_emptyerror3.Visible = false;
            if (ViewState["CurrentTable33"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable33"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    TextBox box1 = new TextBox();
                    TextBox box2 = new TextBox();
                    TextBox box3 = new TextBox();
                    TextBox box4 = new TextBox();
                    TextBox box5 = new TextBox();
                    TextBox box6 = new TextBox();
                    TextBox box7 = new TextBox();
                    TextBox box8 = new TextBox();
                    TextBox box9 = new TextBox();
                    TextBox box10 = new TextBox();
                    TextBox box11 = new TextBox();
                    TextBox box12 = new TextBox();
                    TextBox box13 = new TextBox();

                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {

                        box1 = (TextBox)GridView10.Rows[i].Cells[1].FindControl("txtactname");
                        box2 = (TextBox)GridView10.Rows[i].Cells[2].FindControl("txt_per");
                        box3 = (TextBox)GridView10.Rows[i].Cells[3].FindControl("txt_add");
                        box4 = (TextBox)GridView10.Rows[i].Cells[4].FindControl("txt_st");
                        box5 = (TextBox)GridView10.Rows[i].Cells[5].FindControl("txt_city");
                        box6 = (TextBox)GridView10.Rows[i].Cells[6].FindControl("txt_pin");
                        box7 = (TextBox)GridView10.Rows[i].Cells[7].FindControl("txt_state");
                        box8 = (TextBox)GridView10.Rows[i].Cells[8].FindControl("txt_country");
                        box9 = (TextBox)GridView10.Rows[i].Cells[9].FindControl("txt_phn");
                        box10 = (TextBox)GridView10.Rows[i].Cells[10].FindControl("txt_mail");
                        box11 = (TextBox)GridView10.Rows[i].Cells[11].FindControl("txtattch");

                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i][0] = box1.Text;
                        dtCurrentTable.Rows[i][1] = box2.Text;


                        dtCurrentTable.Rows[i][2] = box3.Text;

                        dtCurrentTable.Rows[i][3] = box4.Text;
                        dtCurrentTable.Rows[i][4] = box5.Text;
                        dtCurrentTable.Rows[i][5] = box6.Text;
                        dtCurrentTable.Rows[i][6] = box7.Text;
                        dtCurrentTable.Rows[i][7] = box8.Text;
                        dtCurrentTable.Rows[i][8] = box9.Text;
                        dtCurrentTable.Rows[i][9] = box10.Text;
                        dtCurrentTable.Rows[i][10] = box11.Text;

                        rowIndex++;
                    }

                    drCurrentRow[0] = name;
                    drCurrentRow[1] = per;
                    drCurrentRow[2] = add;
                    drCurrentRow[3] = street;
                    drCurrentRow[4] = city;
                    drCurrentRow[5] = pin;
                    drCurrentRow[6] = state;
                    drCurrentRow[7] = country;
                    drCurrentRow[8] = phnno;
                    drCurrentRow[9] = mail;
                    drCurrentRow[10] = fileName;
                    drCurrentRow[11] = fileSize;
                    drCurrentRow[12] = documentType;


                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable33"] = dtCurrentTable;

                    GridView10.DataSource = dtCurrentTable;
                    GridView10.DataBind();
                }
            }
            else
            {
                BindGridview10();
            }
        }
        else
        {
            lbl_emptyerror3.Visible = true;
            lbl_emptyerror3.Text = "Must Fill Person & Institution Name";
        }
        txt_othr_pname.Text = "";
        txt_othr_name.Text = "";
        txt_othr_add.Text = "";
        txt_othr_str.Text = "";
        txt_othr_city.Text = "";
        txt_othr_pin.Text = "";
        txt_othr_county.Text = "";
        txt_othr_state.Text = "";
        txt_othr_ph.Text = "";
        txt_othr_mail.Text = "";
    }
    public void BindGridview10()
    {

        GridView10.Visible = true;
        string name = Convert.ToString(txt_othr_pname.Text);
        string per = Convert.ToString(txt_othr_pname.Text);
        string add = Convert.ToString(txt_othr_add.Text);
        string street = Convert.ToString(txt_othr_str.Text);
        string city = Convert.ToString(txt_othr_city.Text);
        string pin = Convert.ToString(txt_othr_pin.Text);
        string state = Convert.ToString(txt_othr_state.Text);
        string country = Convert.ToString(txt_othr_county.Text);
        string phnno = Convert.ToString(txt_othr_ph.Text);
        string mail = Convert.ToString(txt_othr_mail.Text);
        int fileSize = 0;
        string fileName = "";
        string documentType = string.Empty;
        if (FileUpload_prsnt_ind_atch.HasFile)
        {
            if (FileUpload_prsnt_ind_atch.FileName.EndsWith(".jpg") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".gif") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".png") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".txt") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".doc") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".xls") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".docx") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".txt") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".document") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".xls") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".xlsx") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".pdf") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".ppt") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".pptx"))
            {
                fileName = Path.GetFileName(FileUpload_prsnt_ind_atch.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload_prsnt_ind_atch.PostedFile.FileName);

                switch (fileExtension)
                {
                    case ".pdf":
                        documentType = "application/pdf";
                        break;

                    case ".xls":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".xlsx":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".doc":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".docx":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".gif":
                        documentType = "image/gif";
                        break;

                    case ".png":
                        documentType = "image/png";
                        break;

                    case ".jpg":
                        documentType = "image/jpg";
                        break;

                    case ".ppt":
                        documentType = "application/vnd.ms-ppt";
                        break;

                    case ".pptx":
                        documentType = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType = "application/txt";
                        break;
                }

                fileSize = FileUpload_prsnt_ind_atch.PostedFile.ContentLength;

                byte[] documentBinary = new byte[fileSize];
                FileUpload_prsnt_ind_atch.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

            }
        }
        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");
        dt.Columns.Add("Dummy4");
        dt.Columns.Add("Dummy5");
        dt.Columns.Add("Dummy6");
        dt.Columns.Add("Dummy7");
        dt.Columns.Add("Dummy8");
        dt.Columns.Add("Dummy9");
        dt.Columns.Add("Dummy10");
        dt.Columns.Add("Dummy11");
        dt.Columns.Add("Dummy12");
        DataRow dr;

        dr = dt.NewRow();
        dr[0] = name;
        dr[1] = per;
        dr[2] = add;
        dr[3] = street;
        dr[4] = city;
        dr[5] = pin;
        dr[6] = state;
        dr[7] = country;
        dr[8] = phnno;
        dr[9] = mail;
        dr[10] = fileName;
        dr[11] = fileSize;
        dr[12] = documentType;
        dt.Rows.Add(dr);
        ViewState["CurrentTable33"] = dt;
        GridView10.DataSource = dt;
        GridView10.DataBind();
    }

    public void btn_add_prstcomp_Click(object sender, EventArgs e)
    {
        int rowIndex = 0;
        string name = Convert.ToString(TextBox15.Text);
        string per = Convert.ToString(TextBox14.Text);
        string add = Convert.ToString(TextBox16.Text);
        string street = Convert.ToString(TextBox17.Text);
        string city = Convert.ToString(TextBox18.Text);
        string pin = Convert.ToString(TextBox19.Text);
        string state = Convert.ToString(TextBox20.Text);
        string country = Convert.ToString(TextBox21.Text);
        string phnno = Convert.ToString(TextBox22.Text);
        string mail = Convert.ToString(TextBox23.Text);
        int fileSize = 0;
        string fileName = "";
        string documentType = string.Empty;
        if (FileUpload1.HasFile)
        {
            if (FileUpload1.FileName.EndsWith(".jpg") || FileUpload1.FileName.EndsWith(".gif") || FileUpload1.FileName.EndsWith(".png") || FileUpload1.FileName.EndsWith(".txt") || FileUpload1.FileName.EndsWith(".doc") || FileUpload1.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".docx") || FileUpload1.FileName.EndsWith(".txt") || FileUpload1.FileName.EndsWith(".document") || FileUpload1.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".xlsx") || FileUpload1.FileName.EndsWith(".pdf") || FileUpload1.FileName.EndsWith(".ppt") || FileUpload1.FileName.EndsWith(".pptx"))
            {
                fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);

                switch (fileExtension)
                {
                    case ".pdf":
                        documentType = "application/pdf";
                        break;

                    case ".xls":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".xlsx":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".doc":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".docx":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".gif":
                        documentType = "image/gif";
                        break;

                    case ".png":
                        documentType = "image/png";
                        break;

                    case ".jpg":
                        documentType = "image/jpg";
                        break;

                    case ".ppt":
                        documentType = "application/vnd.ms-ppt";
                        break;

                    case ".pptx":
                        documentType = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType = "application/txt";
                        break;
                }

                fileSize = FileUpload1.PostedFile.ContentLength;

                byte[] documentBinary = new byte[fileSize];
                FileUpload1.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

            }
        }
        if (TextBox15.Text != "" && TextBox14.Text != "")
        {
            lbl_emptyerror4.Visible = false;
            if (ViewState["CurrentTable44"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable44"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    TextBox box1 = new TextBox();
                    TextBox box2 = new TextBox();
                    TextBox box3 = new TextBox();
                    TextBox box4 = new TextBox();
                    TextBox box5 = new TextBox();
                    TextBox box6 = new TextBox();
                    TextBox box7 = new TextBox();
                    TextBox box8 = new TextBox();
                    TextBox box9 = new TextBox();
                    TextBox box10 = new TextBox();
                    TextBox box11 = new TextBox();
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {

                        box1 = (TextBox)GridView11.Rows[i].Cells[1].FindControl("txtactname");
                        box2 = (TextBox)GridView11.Rows[i].Cells[2].FindControl("txt_per");
                        box3 = (TextBox)GridView11.Rows[i].Cells[3].FindControl("txt_add");
                        box4 = (TextBox)GridView11.Rows[i].Cells[4].FindControl("txt_st");
                        box5 = (TextBox)GridView11.Rows[i].Cells[5].FindControl("txt_city");
                        box6 = (TextBox)GridView11.Rows[i].Cells[6].FindControl("txt_pin");
                        box7 = (TextBox)GridView11.Rows[i].Cells[7].FindControl("txt_state");
                        box8 = (TextBox)GridView11.Rows[i].Cells[8].FindControl("txt_country");
                        box9 = (TextBox)GridView11.Rows[i].Cells[9].FindControl("txt_phn");
                        box10 = (TextBox)GridView11.Rows[i].Cells[10].FindControl("txt_mail");
                        box11 = (TextBox)GridView11.Rows[i].Cells[11].FindControl("txt_attch");
                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i][0] = box1.Text;
                        dtCurrentTable.Rows[i][1] = box2.Text;


                        dtCurrentTable.Rows[i][2] = box3.Text;

                        dtCurrentTable.Rows[i][3] = box4.Text;
                        dtCurrentTable.Rows[i][4] = box5.Text;
                        dtCurrentTable.Rows[i][5] = box6.Text;
                        dtCurrentTable.Rows[i][6] = box7.Text;
                        dtCurrentTable.Rows[i][7] = box8.Text;
                        dtCurrentTable.Rows[i][8] = box9.Text;
                        dtCurrentTable.Rows[i][9] = box10.Text;
                        dtCurrentTable.Rows[i][10] = box11.Text;
                        rowIndex++;
                    }

                    drCurrentRow[0] = name;
                    drCurrentRow[1] = per;
                    drCurrentRow[2] = add;
                    drCurrentRow[3] = street;
                    drCurrentRow[4] = city;
                    drCurrentRow[5] = pin;
                    drCurrentRow[6] = state;
                    drCurrentRow[7] = country;
                    drCurrentRow[8] = phnno;
                    drCurrentRow[9] = mail;
                    drCurrentRow[10] = fileName;
                    drCurrentRow[11] = fileSize;
                    drCurrentRow[12] = documentType;
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable44"] = dtCurrentTable;

                    GridView11.DataSource = dtCurrentTable;
                    GridView11.DataBind();
                }
            }
            else
            {
                BindGridview11();
            }
        }
        else
        {
            lbl_emptyerror4.Visible = true;
            lbl_emptyerror4.Text = "Must Fill Company & Person Name";
        }
        TextBox15.Text = "";
        TextBox14.Text = "";
        TextBox16.Text = "";
        TextBox17.Text = "";
        TextBox18.Text = "";
        TextBox19.Text = "";
        TextBox20.Text = "";
        TextBox21.Text = "";
        TextBox22.Text = "";
        TextBox23.Text = "";
    }
    public void BindGridview11()
    {

        GridView11.Visible = true;
        string name = Convert.ToString(TextBox15.Text);
        string per = Convert.ToString(TextBox14.Text);
        string add = Convert.ToString(TextBox16.Text);
        string street = Convert.ToString(TextBox17.Text);
        string city = Convert.ToString(TextBox18.Text);
        string pin = Convert.ToString(TextBox19.Text);
        string state = Convert.ToString(TextBox20.Text);
        string country = Convert.ToString(TextBox21.Text);
        string phnno = Convert.ToString(TextBox22.Text);
        string mail = Convert.ToString(TextBox23.Text);
        string fileName = "";
        int fileSize = 0;
        string documentType = string.Empty;
        if (FileUpload1.HasFile)
        {
            if (FileUpload1.FileName.EndsWith(".jpg") || FileUpload1.FileName.EndsWith(".gif") || FileUpload1.FileName.EndsWith(".png") || FileUpload1.FileName.EndsWith(".txt") || FileUpload1.FileName.EndsWith(".doc") || FileUpload1.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".docx") || FileUpload1.FileName.EndsWith(".txt") || FileUpload1.FileName.EndsWith(".document") || FileUpload1.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".xlsx") || FileUpload1.FileName.EndsWith(".pdf") || FileUpload1.FileName.EndsWith(".ppt") || FileUpload1.FileName.EndsWith(".pptx"))
            {
                fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);

                switch (fileExtension)
                {
                    case ".pdf":
                        documentType = "application/pdf";
                        break;

                    case ".xls":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".xlsx":
                        documentType = "application/vnd.ms-excel";
                        break;

                    case ".doc":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".docx":
                        documentType = "application/vnd.ms-word";
                        break;

                    case ".gif":
                        documentType = "image/gif";
                        break;

                    case ".png":
                        documentType = "image/png";
                        break;

                    case ".jpg":
                        documentType = "image/jpg";
                        break;

                    case ".ppt":
                        documentType = "application/vnd.ms-ppt";
                        break;

                    case ".pptx":
                        documentType = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType = "application/txt";
                        break;
                }

                fileSize = FileUpload1.PostedFile.ContentLength;
                //Create array and read the file into it
                byte[] documentBinary = new byte[fileSize];
                FileUpload1.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
            }
        }
        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");
        dt.Columns.Add("Dummy4");
        dt.Columns.Add("Dummy5");
        dt.Columns.Add("Dummy6");
        dt.Columns.Add("Dummy7");
        dt.Columns.Add("Dummy8");
        dt.Columns.Add("Dummy9");
        dt.Columns.Add("Dummy10");
        dt.Columns.Add("Dummy11");
        dt.Columns.Add("Dummy12");
        DataRow dr;

        dr = dt.NewRow();
        dr[0] = name;
        dr[1] = per;
        dr[2] = add;
        dr[3] = street;
        dr[4] = city;
        dr[5] = pin;
        dr[6] = state;
        dr[7] = country;
        dr[8] = phnno;
        dr[9] = mail;
        dr[10] = fileName;
        dr[11] = fileSize;
        dr[12] = documentType;

        dt.Rows.Add(dr);
        ViewState["CurrentTable44"] = dt;
        GridView11.DataSource = dt;
        GridView11.DataBind();
    }

    public void btn_attach_Click(object sender, EventArgs e)
    {
        div_attch.Visible = true;
        reloadothers();
    }

    public void upload()
    {
        try
        {
            bool savnotsflag = false;

            if (fileupload.HasFile)
            {
                if (fileupload.FileName.EndsWith(".jpg") || fileupload.FileName.EndsWith(".gif") || fileupload.FileName.EndsWith(".png") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".doc") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".docx") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".document") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".xlsx") || fileupload.FileName.EndsWith(".pdf") || fileupload.FileName.EndsWith(".ppt") || fileupload.FileName.EndsWith(".pptx"))
                {
                    string fileName = Path.GetFileName(fileupload.PostedFile.FileName);
                    string fileExtension = Path.GetExtension(fileupload.PostedFile.FileName);
                    string documentType = string.Empty;
                    switch (fileExtension)
                    {
                        case ".pdf":
                            documentType = "application/pdf";
                            break;

                        case ".xls":
                            documentType = "application/vnd.ms-excel";
                            break;

                        case ".xlsx":
                            documentType = "application/vnd.ms-excel";
                            break;

                        case ".doc":
                            documentType = "application/vnd.ms-word";
                            break;

                        case ".docx":
                            documentType = "application/vnd.ms-word";
                            break;

                        case ".gif":
                            documentType = "image/gif";
                            break;

                        case ".png":
                            documentType = "image/png";
                            break;

                        case ".jpg":
                            documentType = "image/jpg";
                            break;

                        case ".ppt":
                            documentType = "application/vnd.ms-ppt";
                            break;

                        case ".pptx":
                            documentType = "application/vnd.ms-pptx";
                            break;
                        case ".txt":
                            documentType = "application/txt";
                            break;
                    }

                    int fileSize = fileupload.PostedFile.ContentLength;
                    //Create array and read the file into it
                    byte[] documentBinary = new byte[fileSize];
                    fileupload.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

                    string date = DateTime.Now.ToString("MM/dd/yyyy");
                    SqlCommand cmdnotes = new SqlCommand();

                    string rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");
                    //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                    cmdnotes.CommandText = " update RQ_RequisitionDet set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where RequisitionFK='" + rq_fk + "'";
                    cmdnotes.CommandType = CommandType.Text;
                    cmdnotes.Connection = ssql;

                    SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                    DocName.Value = fileName.ToString();
                    cmdnotes.Parameters.Add(DocName);

                    SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                    Type.Value = documentType.ToString();
                    cmdnotes.Parameters.Add(Type);

                    SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                    uploadedDocument.Value = documentBinary;
                    cmdnotes.Parameters.Add(uploadedDocument);


                    ssql.Close();
                    ssql.Open();
                    int result = cmdnotes.ExecuteNonQuery();
                    if (result > 0)
                    {
                        savnotsflag = true;
                    }

                }

            }

        }
        catch (Exception ex)
        {
        }
    }
    public void upload1()
    {
        try
        {
            bool savnotsflag = false;
            if (FileUpload1_part_indi_attch.HasFile)
            {
                if (FileUpload1_part_indi_attch.FileName.EndsWith(".jpg") || FileUpload1_part_indi_attch.FileName.EndsWith(".gif") || FileUpload1_part_indi_attch.FileName.EndsWith(".png") || FileUpload1_part_indi_attch.FileName.EndsWith(".txt") || FileUpload1_part_indi_attch.FileName.EndsWith(".doc") || FileUpload1_part_indi_attch.FileName.EndsWith(".xls") || FileUpload1_part_indi_attch.FileName.EndsWith(".docx") || FileUpload1_part_indi_attch.FileName.EndsWith(".txt") || FileUpload1_part_indi_attch.FileName.EndsWith(".document") || FileUpload1_part_indi_attch.FileName.EndsWith(".xls") || FileUpload1_part_indi_attch.FileName.EndsWith(".xlsx") || FileUpload1_part_indi_attch.FileName.EndsWith(".pdf") || FileUpload1_part_indi_attch.FileName.EndsWith(".ppt") || FileUpload1_part_indi_attch.FileName.EndsWith(".pptx"))
                {
                    string fileName = Path.GetFileName(FileUpload1_part_indi_attch.PostedFile.FileName);
                    string fileExtension = Path.GetExtension(FileUpload1_part_indi_attch.PostedFile.FileName);
                    string documentType = string.Empty;
                    switch (fileExtension)
                    {
                        case ".pdf":
                            documentType = "application/pdf";
                            break;

                        case ".xls":
                            documentType = "application/vnd.ms-excel";
                            break;

                        case ".xlsx":
                            documentType = "application/vnd.ms-excel";
                            break;

                        case ".doc":
                            documentType = "application/vnd.ms-word";
                            break;

                        case ".docx":
                            documentType = "application/vnd.ms-word";
                            break;

                        case ".gif":
                            documentType = "image/gif";
                            break;

                        case ".png":
                            documentType = "image/png";
                            break;

                        case ".jpg":
                            documentType = "image/jpg";
                            break;

                        case ".ppt":
                            documentType = "application/vnd.ms-ppt";
                            break;

                        case ".pptx":
                            documentType = "application/vnd.ms-pptx";
                            break;
                        case ".txt":
                            documentType = "application/txt";
                            break;
                    }

                    int fileSize = FileUpload1_part_indi_attch.PostedFile.ContentLength;
                    //Create array and read the file into it
                    byte[] documentBinary = new byte[fileSize];
                    FileUpload1_part_indi_attch.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

                    string date = DateTime.Now.ToString("MM/dd/yyyy");
                    SqlCommand cmdnotes = new SqlCommand();

                    string rq_fk = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                    //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                    cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + rq_fk + "'";
                    cmdnotes.CommandType = CommandType.Text;
                    cmdnotes.Connection = ssql;

                    SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                    DocName.Value = fileName.ToString();
                    cmdnotes.Parameters.Add(DocName);

                    SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                    Type.Value = documentType.ToString();
                    cmdnotes.Parameters.Add(Type);

                    SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                    uploadedDocument.Value = documentBinary;
                    cmdnotes.Parameters.Add(uploadedDocument);


                    ssql.Close();
                    ssql.Open();
                    int result = cmdnotes.ExecuteNonQuery();
                    if (result > 0)
                    {
                        savnotsflag = true;
                    }

                }

            }

        }
        catch (Exception ex)
        {
        }
    }
    public void upload2()
    {
        try
        {
            bool savnotsflag = false;
            if (FileUpload_part_comp_attch.HasFile)
            {
                if (FileUpload_part_comp_attch.FileName.EndsWith(".jpg") || FileUpload_part_comp_attch.FileName.EndsWith(".gif") || FileUpload_part_comp_attch.FileName.EndsWith(".png") || FileUpload_part_comp_attch.FileName.EndsWith(".txt") || FileUpload_part_comp_attch.FileName.EndsWith(".doc") || FileUpload_part_comp_attch.FileName.EndsWith(".xls") || FileUpload_part_comp_attch.FileName.EndsWith(".docx") || FileUpload_part_comp_attch.FileName.EndsWith(".txt") || FileUpload_part_comp_attch.FileName.EndsWith(".document") || FileUpload_part_comp_attch.FileName.EndsWith(".xls") || FileUpload_part_comp_attch.FileName.EndsWith(".xlsx") || FileUpload_part_comp_attch.FileName.EndsWith(".pdf") || FileUpload_part_comp_attch.FileName.EndsWith(".ppt") || FileUpload_part_comp_attch.FileName.EndsWith(".pptx"))
                {
                    string fileName = Path.GetFileName(FileUpload_part_comp_attch.PostedFile.FileName);
                    string fileExtension = Path.GetExtension(FileUpload_part_comp_attch.PostedFile.FileName);
                    string documentType = string.Empty;
                    switch (fileExtension)
                    {
                        case ".pdf":
                            documentType = "application/pdf";
                            break;

                        case ".xls":
                            documentType = "application/vnd.ms-excel";
                            break;

                        case ".xlsx":
                            documentType = "application/vnd.ms-excel";
                            break;

                        case ".doc":
                            documentType = "application/vnd.ms-word";
                            break;

                        case ".docx":
                            documentType = "application/vnd.ms-word";
                            break;

                        case ".gif":
                            documentType = "image/gif";
                            break;

                        case ".png":
                            documentType = "image/png";
                            break;

                        case ".jpg":
                            documentType = "image/jpg";
                            break;

                        case ".ppt":
                            documentType = "application/vnd.ms-ppt";
                            break;

                        case ".pptx":
                            documentType = "application/vnd.ms-pptx";
                            break;
                        case ".txt":
                            documentType = "application/txt";
                            break;
                    }

                    int fileSize = FileUpload_part_comp_attch.PostedFile.ContentLength;
                    //Create array and read the file into it
                    byte[] documentBinary = new byte[fileSize];
                    FileUpload_part_comp_attch.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

                    string date = DateTime.Now.ToString("MM/dd/yyyy");
                    SqlCommand cmdnotes = new SqlCommand();

                    string rq_fk = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                    //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                    cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + rq_fk + "'";
                    cmdnotes.CommandType = CommandType.Text;
                    cmdnotes.Connection = ssql;

                    SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                    DocName.Value = fileName.ToString();
                    cmdnotes.Parameters.Add(DocName);

                    SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                    Type.Value = documentType.ToString();
                    cmdnotes.Parameters.Add(Type);

                    SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                    uploadedDocument.Value = documentBinary;
                    cmdnotes.Parameters.Add(uploadedDocument);


                    ssql.Close();
                    ssql.Open();
                    int result = cmdnotes.ExecuteNonQuery();
                    if (result > 0)
                    {
                        savnotsflag = true;
                    }

                }

            }

        }
        catch (Exception ex)
        {
        }
    }
    public void upload3()
    {
        try
        {
            bool savnotsflag = false;
            if (FileUpload_prsnt_ind_atch.HasFile)
            {
                if (FileUpload_prsnt_ind_atch.FileName.EndsWith(".jpg") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".gif") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".png") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".txt") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".doc") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".xls") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".docx") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".txt") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".document") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".xls") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".xlsx") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".pdf") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".ppt") || FileUpload_prsnt_ind_atch.FileName.EndsWith(".pptx"))
                {
                    string fileName = Path.GetFileName(FileUpload_prsnt_ind_atch.PostedFile.FileName);
                    string fileExtension = Path.GetExtension(FileUpload_prsnt_ind_atch.PostedFile.FileName);
                    string documentType = string.Empty;
                    switch (fileExtension)
                    {
                        case ".pdf":
                            documentType = "application/pdf";
                            break;

                        case ".xls":
                            documentType = "application/vnd.ms-excel";
                            break;

                        case ".xlsx":
                            documentType = "application/vnd.ms-excel";
                            break;

                        case ".doc":
                            documentType = "application/vnd.ms-word";
                            break;

                        case ".docx":
                            documentType = "application/vnd.ms-word";
                            break;

                        case ".gif":
                            documentType = "image/gif";
                            break;

                        case ".png":
                            documentType = "image/png";
                            break;

                        case ".jpg":
                            documentType = "image/jpg";
                            break;

                        case ".ppt":
                            documentType = "application/vnd.ms-ppt";
                            break;

                        case ".pptx":
                            documentType = "application/vnd.ms-pptx";
                            break;
                        case ".txt":
                            documentType = "application/txt";
                            break;
                    }

                    int fileSize = FileUpload_prsnt_ind_atch.PostedFile.ContentLength;
                    //Create array and read the file into it
                    byte[] documentBinary = new byte[fileSize];
                    FileUpload_prsnt_ind_atch.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

                    string date = DateTime.Now.ToString("MM/dd/yyyy");
                    SqlCommand cmdnotes = new SqlCommand();

                    string rq_fk = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                    //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                    cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + rq_fk + "'";
                    cmdnotes.CommandType = CommandType.Text;
                    cmdnotes.Connection = ssql;

                    SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                    DocName.Value = fileName.ToString();
                    cmdnotes.Parameters.Add(DocName);

                    SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                    Type.Value = documentType.ToString();
                    cmdnotes.Parameters.Add(Type);

                    SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                    uploadedDocument.Value = documentBinary;
                    cmdnotes.Parameters.Add(uploadedDocument);


                    ssql.Close();
                    ssql.Open();
                    int result = cmdnotes.ExecuteNonQuery();
                    if (result > 0)
                    {
                        savnotsflag = true;
                    }

                }

            }

        }
        catch (Exception ex)
        {
        }
    }
    public void upload4()
    {
        try
        {
            bool savnotsflag = false;
            if (FileUpload1.HasFile)
            {
                if (FileUpload1.FileName.EndsWith(".jpg") || FileUpload1.FileName.EndsWith(".gif") || FileUpload1.FileName.EndsWith(".png") || FileUpload1.FileName.EndsWith(".txt") || FileUpload1.FileName.EndsWith(".doc") || FileUpload1.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".docx") || FileUpload1.FileName.EndsWith(".txt") || FileUpload1.FileName.EndsWith(".document") || FileUpload1.FileName.EndsWith(".xls") || FileUpload1.FileName.EndsWith(".xlsx") || FileUpload1.FileName.EndsWith(".pdf") || FileUpload1.FileName.EndsWith(".ppt") || FileUpload1.FileName.EndsWith(".pptx"))
                {
                    string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                    string documentType = string.Empty;
                    switch (fileExtension)
                    {
                        case ".pdf":
                            documentType = "application/pdf";
                            break;

                        case ".xls":
                            documentType = "application/vnd.ms-excel";
                            break;

                        case ".xlsx":
                            documentType = "application/vnd.ms-excel";
                            break;

                        case ".doc":
                            documentType = "application/vnd.ms-word";
                            break;

                        case ".docx":
                            documentType = "application/vnd.ms-word";
                            break;

                        case ".gif":
                            documentType = "image/gif";
                            break;

                        case ".png":
                            documentType = "image/png";
                            break;

                        case ".jpg":
                            documentType = "image/jpg";
                            break;

                        case ".ppt":
                            documentType = "application/vnd.ms-ppt";
                            break;

                        case ".pptx":
                            documentType = "application/vnd.ms-pptx";
                            break;
                        case ".txt":
                            documentType = "application/txt";
                            break;
                    }

                    int fileSize = FileUpload1.PostedFile.ContentLength;
                    //Create array and read the file into it
                    byte[] documentBinary = new byte[fileSize];
                    FileUpload1.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

                    string date = DateTime.Now.ToString("MM/dd/yyyy");
                    SqlCommand cmdnotes = new SqlCommand();

                    string rq_fk = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                    //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                    cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + rq_fk + "'";
                    cmdnotes.CommandType = CommandType.Text;
                    cmdnotes.Connection = ssql;

                    SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                    DocName.Value = fileName.ToString();
                    cmdnotes.Parameters.Add(DocName);

                    SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                    Type.Value = documentType.ToString();
                    cmdnotes.Parameters.Add(Type);

                    SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                    uploadedDocument.Value = documentBinary;
                    cmdnotes.Parameters.Add(uploadedDocument);


                    ssql.Close();
                    ssql.Open();
                    int result = cmdnotes.ExecuteNonQuery();
                    if (result > 0)
                    {
                        savnotsflag = true;
                    }

                }

            }

        }
        catch (Exception ex)
        {
        }
    }

    public void access()
    {
        try
        {
            string query = "";
            string Master1 = "";
            string stud = "";
            string values = "";
            string sms = "";
            string sms1 = "";
            string sms2 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='SMS Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='SMS Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    string[] split = val.Split(',');
                    if (split.Length == 1)
                    {
                        sms = split[0];
                        if (sms == "1")
                        {
                            sms_req = sms;
                        }
                    }
                    else if (split.Length == 2)
                    {
                        sms = split[0];
                        sms1 = split[1];
                        if (sms == "1")
                        {
                            sms_req = sms;
                        }
                        else if (sms == "2")
                        {
                            sms_app = sms;
                        }
                        else if (sms == "3")
                        {
                            sms_exit = sms;
                        }
                        if (sms1 == "1")
                        {
                            sms_req = sms1;
                        }
                        else if (sms1 == "2")
                        {
                            sms_app = sms1;
                        }
                        else if (sms1 == "3")
                        {
                            sms_exit = sms1;
                        }
                    }
                    else
                    {
                        sms = split[0];
                        sms1 = split[1];
                        sms2 = split[2];

                        sms_req = "1";
                        sms_app = "2";
                        sms_exit = "3";

                    }

                }
            }

        }
        catch
        {
        }
    }
    public void access1()
    {
        try
        {
            string query = "";
            string Master1 = "";
            string stud = "";
            string values = "";
            string sms = "";
            string sms1 = "";
            string sms2 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='SMS Mobile Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='SMS Mobile Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    string[] split = val.Split(',');
                    if (split.Length == 1)
                    {
                        sms = split[0];
                        if (sms == "1")
                        {
                            sms_mom = sms;
                        }
                        else if (sms == "2")
                        {
                            sms_dad = sms;
                        }
                        else if (sms == "3")
                        {
                            sms_stud = sms;
                        }
                    }
                    else if (split.Length == 2)
                    {
                        sms = split[0];
                        sms1 = split[1];
                        if (sms == "1")
                        {
                            sms_mom = sms;
                        }
                        else if (sms == "2")
                        {
                            sms_dad = sms;
                        }
                        else if (sms == "3")
                        {
                            sms_stud = sms;
                        }
                        if (sms1 == "1")
                        {
                            sms_mom = sms1;
                        }
                        else if (sms1 == "2")
                        {
                            sms_dad = sms1;
                        }
                        else if (sms1 == "3")
                        {
                            sms_stud = sms1;
                        }
                    }
                    else
                    {
                        sms = split[0];
                        sms1 = split[1];
                        sms2 = split[2];

                        sms_mom = "1";
                        sms_dad = "2";
                        sms_stud = "3";

                    }

                }
            }

        }
        catch
        {
        }
    }


    public void reloadothers()
    {
        //if (ddl_popuptitle.SelectedItem.Text == "Others")
        //{
        //    txt_poprd_title.Attributes.Add("style", "display:block");
        //}
        //else
        //{
        //    txt_poprd_title.Attributes.Add("style", "display:none");
        //}
    }
    public void btn_event_appclear_Click(object sender, EventArgs e)
    {
        reload();
        reloadothers();
        txtothers.Text = "";
        txtfd.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txttd.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_min_startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        timevalue();
        txt_min_startperiod.Text = "";
        txt_min_endperiod.Text = "";
        txt_min_location.Text = "";
        txt_min_action.Text = "";
        rdo_commpati.Checked = true;
        txt_pre_action.Text = "";
        txt_pre_ctname.Text = "";
        txt_pre_repby.Text = "";
        txt_mat_itemname.Text = "";
        txt_mat_qunty.Text = "";
        txt_mat_expect.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_inst_name.Text = "";
        txt_ins_resource.Text = "";
        txt_ins_amount.Text = "";
        txt_departmentname.Text = "";
        txt_dept_resource.Text = "";
        txt_dept_amt.Text = "";
        txt_sponscmp_name.Text = "";
        txt_sponc_contact.Text = "";
        txt_sponc_amount.Text = "";
        txt_spn_cmpy.Text = "";
        txt_sp_cont.Text = "";
        txt_sp_amt.Text = "";
        txt_expnc_descrp.Text = "";
        txt_expnce_amt.Text = "";
        txt_expn_name.Text = "";
        ddl_expnc_name.Text = "Select";

    }



    public void TextBox1_TextChanged(object sender, EventArgs e)
    {

        string name = Convert.ToString(TextBox1.Text);
        string vendorpk = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCompName like '" + name + "%'");

        string query = "select v.VendorCode,v.VendorCompName,v.VendorCountry,v.VendorAddress,v.VendorCity,v.VendorStreet,v.VendorDist,v.VendorState,v.VendorPin,vc.VenContactName,vc.VenContactDept,vc.VenContactDesig,vc.VendorPhoneNo,vc.VendorExtNo,vc.VendorMobileNo,vc.VendorEmail from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK and v.VendorPK='" + vendorpk + "'";
        ds1 = d2.select_method_wo_parameter(query, "Text");


        if (ds1.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {

                string statecode = ds1.Tables[0].Rows[i]["VendorState"].ToString();
                string countrycode = ds1.Tables[0].Rows[i]["VendorCountry"].ToString();
                string statename = d2.GetFunction("select MasterValue from CO_MasterValues where  MasterCriteria='state' and MasterCode='" + statecode + "' ");
                string countryname = d2.GetFunction("select MasterValue from CO_MasterValues where  MasterCriteria='coun' and MasterCode='" + countrycode + "' ");

                TextBox1.Text = ds1.Tables[0].Rows[i]["VendorCompName"].ToString();

                TextBox2.Text = ds1.Tables[0].Rows[i]["VenContactName"].ToString();
                TextBox3.Text = ds1.Tables[0].Rows[i]["VenContactDesig"].ToString();
                TextBox4.Text = ds1.Tables[0].Rows[i]["VendorStreet"].ToString();
                TextBox8.Text = ds1.Tables[0].Rows[i]["VendorCity"].ToString();
                TextBox3.Text = ds1.Tables[0].Rows[i]["VendorAddress"].ToString();
                TextBox10.Text = statename;
                TextBox11.Text = countryname;
                TextBox9.Text = ds1.Tables[0].Rows[i]["VendorPin"].ToString();
                TextBox12.Text = ds1.Tables[0].Rows[i]["VendorPhoneNo"].ToString();
                TextBox13.Text = ds1.Tables[0].Rows[i]["VendorEmail"].ToString();

            }
        }
        All_dropdownchange();

    }

    public void TextBox15_TextChanged(object sender, EventArgs e)
    {
        string name = Convert.ToString(TextBox15.Text);
        string vendorpk = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCompName like '" + name + "%'");

        string query = "select v.VendorCode,v.VendorCompName,v.VendorCountry,v.VendorAddress,v.VendorCity,v.VendorStreet,v.VendorDist,v.VendorState,v.VendorPin,vc.VenContactName,vc.VenContactDept,vc.VenContactDesig,vc.VendorPhoneNo,vc.VendorExtNo,vc.VendorMobileNo,vc.VendorEmail from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK and v.VendorPK='" + vendorpk + "'";
        ds1 = d2.select_method_wo_parameter(query, "Text");


        if (ds1.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {

                string statecode = ds1.Tables[0].Rows[i]["VendorState"].ToString();
                string countrycode = ds1.Tables[0].Rows[i]["VendorCountry"].ToString();
                string statename = d2.GetFunction("select MasterValue from CO_MasterValues where  MasterCriteria='state' and MasterCode='" + statecode + "' ");
                string countryname = d2.GetFunction("select MasterValue from CO_MasterValues where  MasterCriteria='coun' and MasterCode='" + countrycode + "' ");
                TextBox15.Text = ds1.Tables[0].Rows[i]["VendorCompName"].ToString();

                TextBox14.Text = ds1.Tables[0].Rows[i]["VenContactName"].ToString();
                // TextBox3.Text = ds1.Tables[0].Rows[i]["VenContactDesig"].ToString();
                TextBox17.Text = ds1.Tables[0].Rows[i]["VendorStreet"].ToString();
                TextBox18.Text = ds1.Tables[0].Rows[i]["VendorCity"].ToString();
                TextBox16.Text = ds1.Tables[0].Rows[i]["VendorAddress"].ToString();
                TextBox20.Text = statename;
                TextBox21.Text = countryname;
                TextBox19.Text = ds1.Tables[0].Rows[i]["VendorPin"].ToString();
                TextBox22.Text = ds1.Tables[0].Rows[i]["VendorPhoneNo"].ToString();
                TextBox23.Text = ds1.Tables[0].Rows[i]["VendorEmail"].ToString();

            }
        }
        All_dropdownchange();

    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getothername(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorCompName from CO_VendorMaster where VendorType=7 and VendorCompName like '" + prefixText + "%' and VendorName not in('" + particpentindi + "')";
        name = ws.Getname(query);
        return name;

    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getothernameprst(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorCompName from CO_VendorMaster where VendorType=7 and VendorCompName like '" + prefixText + "%' and VendorName not in('" + persentedindi + "')";
        name = ws.Getname(query);
        return name;

    }
    public void txt_othr_name_TextChanged(object sender, EventArgs e)
    {
        string name = Convert.ToString(txt_othr_name.Text);
        string vendorpk = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCompName like '" + name + "%'");

        string query = "select v.VendorCode,v.VendorCompName,v.VendorCountry,v.VendorName,v.VendorAddress,v.VendorCity,v.VendorStreet,v.VendorDist,v.VendorState,v.VendorPin,v.VendorEmailID,v.VendorCity,v.VendorPhoneNo from CO_VendorMaster v where v.VendorPK='" + vendorpk + "'";

        ds1 = d2.select_method_wo_parameter(query, "Text");


        if (ds1.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {

                string statecode = ds1.Tables[0].Rows[i]["VendorState"].ToString();
                string countrycode = ds1.Tables[0].Rows[i]["VendorCountry"].ToString();
                string statename = d2.GetFunction("select MasterValue from CO_MasterValues where  MasterCriteria='state' and MasterCode='" + statecode + "' ");
                string countryname = d2.GetFunction("select MasterValue from CO_MasterValues where  MasterCriteria='coun' and MasterCode='" + countrycode + "' ");
                txt_othr_name.Text = ds1.Tables[0].Rows[i]["VendorCompName"].ToString();

                txt_othr_pname.Text = ds1.Tables[0].Rows[i]["VendorName"].ToString();
                // TextBox3.Text = ds1.Tables[0].Rows[i]["VenContactDesig"].ToString();
                txt_othr_str.Text = ds1.Tables[0].Rows[i]["VendorStreet"].ToString();
                txt_othr_city.Text = ds1.Tables[0].Rows[i]["VendorCity"].ToString();
                txt_othr_add.Text = ds1.Tables[0].Rows[i]["VendorAddress"].ToString();
                txt_othr_state.Text = statename;
                txt_othr_county.Text = countryname;
                txt_othr_pin.Text = ds1.Tables[0].Rows[i]["VendorPin"].ToString();
                txt_othr_ph.Text = ds1.Tables[0].Rows[i]["VendorPhoneNo"].ToString();
                txt_othr_mail.Text = ds1.Tables[0].Rows[i]["VendorEmailID"].ToString();

            }
        }
        All_dropdownchange();
    }
    public void txt_cmpname1_TextChanged(object sender, EventArgs e)
    {
        string pname = Convert.ToString(txt_othr_pname1.Text);
        string name = Convert.ToString(txt_cmpname1.Text);
        string vendorpk = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCompName like '" + name + "%'");

        string query = "select v.VendorCode,v.VendorCompName,v.VendorCountry,v.VendorName,v.VendorAddress,v.VendorCity,v.VendorStreet,v.VendorDist,v.VendorState,v.VendorPin,v.VendorEmailID,v.VendorCity,v.VendorPhoneNo from CO_VendorMaster v where v.VendorPK='" + vendorpk + "'";

        ds1 = d2.select_method_wo_parameter(query, "Text");


        if (ds1.Tables[0].Rows.Count > 0)
        {

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {

                string statecode = ds1.Tables[0].Rows[i]["VendorState"].ToString();
                string countrycode = ds1.Tables[0].Rows[i]["VendorCountry"].ToString();
                string statename = d2.GetFunction("select MasterValue from CO_MasterValues where  MasterCriteria='state' and MasterCode='" + statecode + "' ");
                string countryname = d2.GetFunction("select MasterValue from CO_MasterValues where  MasterCriteria='coun' and MasterCode='" + countrycode + "' ");
                txt_cmpname1.Text = ds1.Tables[0].Rows[i]["VendorCompName"].ToString();

                txt_othr_pname1.Text = ds1.Tables[0].Rows[i]["VendorName"].ToString();
                // TextBox3.Text = ds1.Tables[0].Rows[i]["VenContactDesig"].ToString();
                tx_comstr.Text = ds1.Tables[0].Rows[i]["VendorStreet"].ToString();
                txt_cmcity.Text = ds1.Tables[0].Rows[i]["VendorCity"].ToString();
                tx_compadd.Text = ds1.Tables[0].Rows[i]["VendorAddress"].ToString();
                txt_cmstste.Text = statename;
                txt_cmcountry.Text = countryname;
                txt_cmpin.Text = ds1.Tables[0].Rows[i]["VendorPin"].ToString();
                txt_cmpho.Text = ds1.Tables[0].Rows[i]["VendorPhoneNo"].ToString();
                txt_cmmail.Text = ds1.Tables[0].Rows[i]["VendorEmailID"].ToString();

            }
        }
        All_dropdownchange();
    }

    [WebMethod]
    public static string Checkdept(string Roll_No)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Roll_No != "")
            {
                string query = "";

                query = "select Dept_Name as DeptName,Dept_Code from Department where Dept_Name = '" + Roll_No + "'";


                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";

                }
                else
                {
                    returnValue = "1";

                }

            }
            else
            {
                returnValue = "0";
            }


        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    [WebMethod]
    public static string Checkindividual(string Roll_No)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Roll_No != "")
            {
                string[] split1 = Roll_No.Split('-');
                string namesplit1 = split1[0];
                string query = "";

                query = "select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and s.staff_name='" + namesplit1 + "'";


                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";

                }
                else
                {
                    returnValue = "1";

                }

            }
            else
            {
                returnValue = "0";
            }


        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    public void reload()
    {
        //    loadeventtype();

        //    //loadeventLOC();
        //    loadaward();
        //    loadgame();
        //    loadseminar();
        //    loadtitle();
        //    loadtour();
        //    loadexpn();
        //    loadnewaction();
        //    res();

    }
    public void loadeventtype()
    {

        ds.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='EventName'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlname.DataSource = ds;
            ddlname.DataTextField = "MasterValue";
            ddlname.DataValueField = "MasterCode";
            ddlname.DataBind();
            ddlname.Items.Insert(0, new ListItem("Select", "0"));

        }
        else
        {
            ddlname.Items.Insert(0, new ListItem("Select", "0"));


        }

    }

    public void loadtour()
    {

        ds.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='EventTourType'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_Tournament.DataSource = ds;
            ddl_Tournament.DataTextField = "MasterValue";
            ddl_Tournament.DataValueField = "MasterCode";
            ddl_Tournament.DataBind();
            ddl_Tournament.Items.Insert(0, new ListItem("Select", "0"));

        }
        else
        {
            ddl_Tournament.Items.Insert(0, new ListItem("Select", "0"));


        }

    }

    public void loadgame()
    {

        ds.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='EventGame'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_game.DataSource = ds;
            ddl_game.DataTextField = "MasterValue";
            ddl_game.DataValueField = "MasterCode";
            ddl_game.DataBind();
            ddl_game.Items.Insert(0, new ListItem("Select", "0"));

        }
        else
        {
            ddl_game.Items.Insert(0, new ListItem("Select", "0"));


        }

    }
    public void loadseminar()
    {

        ds.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='EventSemType'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_seminar.DataSource = ds;
            ddl_seminar.DataTextField = "MasterValue";
            ddl_seminar.DataValueField = "MasterCode";
            ddl_seminar.DataBind();
            ddl_seminar.Items.Insert(0, new ListItem("Select", "0"));

        }
        else
        {
            ddl_seminar.Items.Insert(0, new ListItem("Select", "0"));


        }

    }
    public void loadaward()
    {

        ds.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='EventAward'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_awdcat.DataSource = ds;
            ddl_awdcat.DataTextField = "MasterValue";
            ddl_awdcat.DataValueField = "MasterCode";
            ddl_awdcat.DataBind();
            ddl_awdcat.Items.Insert(0, new ListItem("Select", "0"));

        }
        else
        {
            ddl_awdcat.Items.Insert(0, new ListItem("Select", "0"));


        }

    }
    public void loadtitle()
    {

        ds.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='EventTitle'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_popuptitle.DataSource = ds;
            ddl_popuptitle.DataTextField = "MasterValue";
            ddl_popuptitle.DataValueField = "MasterCode";
            ddl_popuptitle.DataBind();
            ddl_popuptitle.Items.Insert(0, new ListItem("Select", "0"));

        }
        else
        {
            ddl_popuptitle.Items.Insert(0, new ListItem("Select", "0"));


        }

    }
    public void loadexpn()
    {

        ds.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='Expense'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_expnc_name.DataSource = ds;
            ddl_expnc_name.DataTextField = "MasterValue";
            ddl_expnc_name.DataValueField = "MasterCode";
            ddl_expnc_name.DataBind();
            ddl_expnc_name.Items.Insert(0, new ListItem("Select", "0"));

        }
        else
        {
            ddl_expnc_name.Items.Insert(0, new ListItem("Select", "0"));


        }

    }
    public void loadnewaction()
    {

        ds.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='Action'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_act_namenew.DataSource = ds;
            ddl_act_namenew.DataTextField = "MasterValue";
            ddl_act_namenew.DataValueField = "MasterCode";
            ddl_act_namenew.DataBind();
            ddl_act_namenew.Items.Insert(0, new ListItem("Select", "0"));
            ddl_act_namenew.Items.Insert(ddl_act_namenew.Items.Count, "Others");

        }
        else
        {
            ddl_act_namenew.Items.Insert(0, new ListItem("Select", "0"));
            ddl_act_namenew.Items.Insert(ddl_act_namenew.Items.Count, "Others");

        }

    }

    public void txt_mat_expect_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string dt = txt_mat_expect.Text;
            string[] Split = dt.Split('/');
            DateTime todate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            string enddt = DateTime.Now.ToString("dd/MM/yyyy");
            Split = enddt.Split('/');
            DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);

            if (fromdate > todate)
            {

                txt_mat_expect.Text = DateTime.Now.ToString("dd/MM/yyyy");

            }
        }
        catch (Exception ex)
        {

        }
        All_dropdownchange();
    }

    public void event_clearall()
    {
        txtothers.Text = "";
        txtfd.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txttd.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_min_startdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        timevalue();
        txt_min_startperiod.Text = "";
        txt_min_endperiod.Text = "";
        txt_min_location.Text = "";
        txt_min_action.Text = "";
        rdo_commpati.Checked = true;
        txt_pre_action.Text = "";
        txt_pre_ctname.Text = "";
        txt_pre_repby.Text = "";
        txt_mat_itemname.Text = "";
        txt_mat_qunty.Text = "";
        txt_mat_expect.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_inst_name.Text = "";
        txt_ins_resource.Text = "";
        txt_ins_amount.Text = "";
        txt_departmentname.Text = "";
        txt_dept_resource.Text = "";
        txt_dept_amt.Text = "";
        txt_sponscmp_name.Text = "";
        txt_sponc_contact.Text = "";
        txt_sponc_amount.Text = "";
        txt_spn_cmpy.Text = "";
        txt_sp_cont.Text = "";
        txt_sp_amt.Text = "";
        txt_expnc_descrp.Text = "";
        txt_expnce_amt.Text = "";
        txt_expn_name.Text = "";
        gridadd.Visible = false;
        poprdoview.Visible = false;
        txt_act_description.Text = "";
        txt_confn.Text = "";
        txt_jour.Text = "";
        txt_impact.Text = "";
        txt_patentnumb.Text = "";
        txt_patentappno.Text = "";
        txt_patentappstatus.Text = "";
        //pop_radiodiv.Visible = false;
        //rdb_Papers.Checked = false;
        //rdb_Paper.Checked = false;
        //rdb_Patents.Checked = false;
        //rdb_Conference.Checked = false;
        //rdb_Award.Checked = false;
        //rdb_student.Checked = false;
        //rdb_ReSearch.Checked = false;
        //rdb_Membership.Checked = false;
        //rdb_Distinguished.Checked = false;
        //rdb_Tournamentk.Checked = false;
        //rdb_Symposium.Checked = false;
        //RDB_OTHERS.Checked = false;

        if (cbl_degree.Items.Count > 0)
        {
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                cbl_degree.Items[i].Selected = false;
            }
            txt_degree.Text = "--Select--";
        }
        cb_degree.Checked = false;


        if (cbl_branch.Items.Count > 0)
        {
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                cbl_branch.Items[i].Selected = false;
            }
            txt_branch.Text = "--Select--";
        }
        cb_branch.Checked = false;

        if (cbl_or_sem.Items.Count > 0)
        {
            for (int i = 0; i < cbl_or_sem.Items.Count; i++)
            {
                cbl_or_sem.Items[i].Selected = false;
            }
            txt_org_sem.Text = "--Select--";
        }
        cb_or_sem.Checked = false;

        cb_staff_name.Checked = false;
        if (cbl_staff_name.Items.Count > 0)
        {
            for (int i = 0; i < cbl_staff_name.Items.Count; i++)
            {
                cbl_staff_name.Items[i].Selected = false;
            }
            txt_staffnamemul.Text = "--Select--";
        }
    }

    public void event_gridclear()
    {
        GridView4.Visible = false;
        GridView4.DataSource = null;
        GridView4.DataBind();
        GridView5.Visible = false;
        GridView5.DataSource = null;
        GridView5.DataBind();
        GridView7.Visible = false;
        GridView7.DataSource = null;
        GridView7.DataBind();
        GridView3.Visible = false;
        GridView3.DataSource = null;
        GridView3.DataBind();
        GV3.Visible = false;
        GV3.DataSource = null;
        GV3.DataBind();
        GV4.Visible = false;
        GV4.DataSource = null;
        GV4.DataBind();
        GridView6.Visible = false;
        GridView6.DataSource = null;
        GridView6.DataBind();
        GridView7.Visible = false;
        GridView7.DataSource = null;
        GridView7.DataBind();
        divprte.Visible = false;
        div_expence.Visible = false;
        divmr.Visible = false;
    }

    protected void OnRowDeleting_GridView4(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["CurrentTable111"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable111"] = dt;


            GridView4.DataSource = dt;
            GridView4.DataBind();
        }
        else
        {
            GridView4.Visible = false;
            GridView4.DataSource = null;
            GridView4.DataBind();
            ViewState["CurrentTable111"] = null;
        }
    }

    protected void grid4_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int a = Convert.ToInt32(e.CommandArgument);
        }
        catch
        {
        }

    }
    protected void OnRowDeleting_GridView5(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["CurrentTable222"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable222"] = dt;
            GridView5.DataSource = dt;
            GridView5.DataBind();
        }
        else
        {
            GridView5.Visible = false;
            GridView5.DataSource = null;
            GridView5.DataBind();
            ViewState["CurrentTable222"] = null;
        }
    }

    protected void grid5_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int a = Convert.ToInt32(e.CommandArgument);
        }
        catch
        {
        }

    }

    protected void OnRowDeleting_grid7(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["CurrentTable333"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable333"] = dt;
            GridView7.DataSource = dt;
            GridView7.DataBind();
        }
        else
        {
            GridView7.Visible = false;
            GridView7.DataSource = null;
            GridView7.DataBind();
            ViewState["CurrentTable333"] = null;
        }

    }
    protected void grid7_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int a = Convert.ToInt32(e.CommandArgument);
        }
        catch
        {
        }

    }
    protected void OnRowDeleting_GridView3(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["CurrentTable444"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable444"] = dt;
            GridView3.DataSource = dt;
            GridView3.DataBind();
        }
        else
        {
            GridView3.Visible = false;
            GridView3.DataSource = null;
            GridView3.DataBind();
            ViewState["CurrentTable444"] = null;
        }

    }
    protected void grid3_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int a = Convert.ToInt32(e.CommandArgument);
        }
        catch
        {
        }

    }
    protected void OnRowDeleting_gv3(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["CurrentTable888"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable888"] = dt;
            GV3.DataSource = dt;
            GV3.DataBind();
        }
        else
        {
            GV3.Visible = false;
            GV3.DataSource = null;
            GV3.DataBind();
            ViewState["CurrentTable888"] = null;
        }

    }
    protected void gv3_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int a = Convert.ToInt32(e.CommandArgument);
        }
        catch
        {
        }

    }

    protected void OnRowDeleting_GV4(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["CurrentTable666"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable666"] = dt;
            GV4.DataSource = dt;
            GV4.DataBind();
        }
        else
        {
            GV4.Visible = false;
            GV4.DataSource = null;
            GV4.DataBind();
            ViewState["CurrentTable666"] = null;
        }

    }
    protected void GV4_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int a = Convert.ToInt32(e.CommandArgument);
        }
        catch
        {
        }

    }

    protected void OnRowDeleting_GridView6(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["CurrentTable777"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable777"] = dt;
            GridView6.DataSource = dt;
            GridView6.DataBind();
        }
        else
        {
            GridView6.Visible = false;
            GridView6.DataSource = null;
            GridView6.DataBind();
            ViewState["CurrentTable777"] = null;
        }

    }
    protected void grid6_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int a = Convert.ToInt32(e.CommandArgument);
        }
        catch
        {
        }

    }

    protected void OnRowDeleting_GridView9(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["CurrentTable22"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable22"] = dt;
            GridView9.DataSource = dt;
            GridView9.DataBind();
        }
        else
        {
            GridView9.Visible = false;
            GridView9.DataSource = null;
            GridView9.DataBind();
            ViewState["CurrentTable22"] = null;
        }

    }
    protected void grid9_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int a = Convert.ToInt32(e.CommandArgument);
        }
        catch
        {
        }

    }

    protected void OnRowDeleting_GridView10(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["CurrentTable33"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable33"] = dt;
            GridView10.DataSource = dt;
            GridView10.DataBind();
        }
        else
        {
            GridView10.Visible = false;
            GridView10.DataSource = null;
            GridView10.DataBind();
            ViewState["CurrentTable33"] = null;
        }

    }
    protected void grid10_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int a = Convert.ToInt32(e.CommandArgument);
        }
        catch
        {
        }

    }

    protected void OnRowDeleting_GridView8(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["CurrentTable11"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable11"] = dt;
            GridView8.DataSource = dt;
            GridView8.DataBind();
        }
        else
        {
            GridView8.Visible = false;
            GridView8.DataSource = null;
            GridView8.DataBind();
            ViewState["CurrentTable11"] = null;
        }

    }
    protected void grid8_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int a = Convert.ToInt32(e.CommandArgument);
        }
        catch
        {
        }

    }

    protected void OnRowDeleting_GridView11(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["CurrentTable44"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable44"] = dt;
            GridView11.DataSource = dt;
            GridView11.DataBind();
        }
        else
        {
            GridView11.Visible = false;
            GridView11.DataSource = null;
            GridView11.DataBind();
            ViewState["CurrentTable44"] = null;
        }

    }
    protected void grid11_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int a = Convert.ToInt32(e.CommandArgument);
        }
        catch
        {
        }

    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstudname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select a.stud_name,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    public void BindGridview()
    {
        ArrayList addnew = new ArrayList();
        DateTime fromdate = new DateTime();
        fromdate = TextToDate(txtfd);
        DateTime todate = new DateTime();
        todate = TextToDate(txttd);
        GV1.Visible = true;
        TimeSpan c = fromdate - todate;
        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");
        dt.Columns.Add("Dummy4");
        dt.Columns.Add("Dummay5");
        dt.Columns.Add("Dummay6");
        dt.Columns.Add("Dummay7");
        DataRow dr;
        if (fromdate != todate)
        {
            for (; fromdate <= todate; )
            {
                string to = Convert.ToString(txttd.Text);
                string from = Convert.ToString(txtfd.Text);
                dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = fromdate.ToString("dd/MM/yyyy");
                dr[2] = "";
                dr[3] = "";
                dr[4] = "";
                dr[5] = "";
                dr[6] = "";
                dt.Rows.Add(dr);
                fromdate = fromdate.AddDays(1);
            }

        }
        else
        {
            dr = dt.NewRow();
            dr[0] = "1";

            dr[1] = fromdate.ToString("dd/MM/yyyy");

            dr[2] = "";
            dr[3] = "";
            dr[4] = "";
            dr[5] = "";
            dr[6] = "";
            dt.Rows.Add(dr);

            fromdate = fromdate.AddDays(1);
        }

        if (dt.Rows.Count > 0)
        {
            GV1.DataSource = dt;
            GV1.DataBind();

        }
    }
    public void minute_event(int index)
    {
        try
        {
            string date = "";
            foreach (GridViewRow row1 in GV1.Rows)
            {
                if (ii == row1.DataItemIndex)
                {
                    TextBox txtsttime = (TextBox)GV1.Rows[ii].FindControl("txtdate");
                    date = Convert.ToString(txtsttime.Text);
                }
            }
            txt_min_startdate.Text = date;
            txt_min_enddate.Text = date;
            txt_min_startperiod.Text = "";
            txt_min_endperiod.Text = "";
            txt_min_location.Text = "";
            txt_min_action.Text = "";
            gridadd.Visible = false;
            poprdoview.Visible = false;
            txt_act_description.Text = "";
            loadaction();

            pop_minute.Visible = true;
        }
        catch
        {
        }
    }
    public void savetemp(int index)
    {
        try
        {

            //string req_no = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequestCode='" + event_requestcode + "'");
            ArrayList addnew = new ArrayList();
            DateTime fromdate = new DateTime();
            fromdate = TextToDate(txtfd);
            DateTime todate = new DateTime();
            todate = TextToDate(txttd);
            TimeSpan c = fromdate - todate;
            string dateval;

            foreach (GridViewRow row1 in GV1.Rows)
            {
                if (ii == row1.DataItemIndex)
                {
                    TextBox txtdate = (TextBox)GV1.Rows[ii].FindControl("txtdate");
                    dateval = Convert.ToString(txtdate.Text);
                }
            }
            //for (; fromdate <= todate; )
            //{

            //}



        }
        catch
        {
        }
    }
    public void BindGridviewadd()
    {
        string starthr = Convert.ToString(ddl_hour1.SelectedItem.Text);
        string startmm = Convert.ToString(ddl_minits1.SelectedItem.Text);
        string startday = Convert.ToString(ddl_timeformate1.SelectedItem.Text);
        string endhr = Convert.ToString(ddl_endhour1.SelectedItem.Text);
        string endmm = Convert.ToString(ddl_endminit1.SelectedItem.Text);
        string endday = Convert.ToString(ddl_endformate1.SelectedItem.Text);

        string starttime = starthr + ":" + startmm + ":" + startday;
        string endtime = endhr + ":" + endmm + ":" + endday;

        gridadd.Visible = true;
        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");
        dt.Columns.Add("Dummy4");
        dt.Columns.Add("Dummay5");
        dt.Columns.Add("Dummay6");
        dt.Columns.Add("Dummay7");
        DataRow dr;

        string value = Convert.ToString(txt_min_action.Text);
        if (value != "")
        {
            int val = Convert.ToInt32(value);

            for (int i = 0; i < val; i++)
            {
                dr = dt.NewRow();

                if (i == 0)
                {
                    dr[3] = starttime;
                }

                if (i == (val - 1))
                {
                    dr[4] = endtime;
                }
                dr[0] = "1";
                dr[1] = "";
                dr[2] = "";
                dr[5] = "";
                dr[6] = "";
                dt.Rows.Add(dr);
            }

        }
        ViewState["gridadd"] = dt;
        if (dt.Rows.Count > 0)
        {
            gridadd.DataSource = dt;
            gridadd.DataBind();

        }
        reloadothers();

    }


    public void timevalue()
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        string hrr = "";
        //string time =Convert.ToString(txt_viewtime.Text);
        string[] ay = time.Split(':');
        string val_hr = ay[0].ToString();
        int hr = Convert.ToInt16(val_hr);
        if (val_hr == "01")
        {
            hrr = "1";
        }
        else if (val_hr == "02")
        {
            hrr = "2";
        }
        else if (val_hr == "03")
        {
            hrr = "3";
        }

        else if (val_hr == "04")
        {
            hrr = "4";
        }
        else if (val_hr == "05")
        {
            hrr = "5";
        }

        else if (val_hr == "06")
        {
            hrr = "6";
        }
        else if (val_hr == "07")
        {
            hrr = "7";
        }

        else if (val_hr == "08")
        {
            hrr = "8";
        }
        else if (val_hr == "09")
        {
            hrr = "9";
        }
        else if (val_hr == "13")
        {
            hrr = "1";
        }
        else if (val_hr == "14")
        {
            hrr = "2";
        }
        else if (val_hr == "15")
        {
            hrr = "3";
        }

        else if (val_hr == "16")
        {
            hrr = "4";
        }
        else if (val_hr == "17")
        {
            hrr = "5";
        }

        else if (val_hr == "18")
        {
            hrr = "6";
        }
        else if (val_hr == "19")
        {
            hrr = "7";
        }

        else if (val_hr == "20")
        {
            hrr = "8";
        }
        else if (val_hr == "21")
        {
            hrr = "9";
        }

        else if (val_hr == "22")
        {
            hrr = "10";
        }
        else if (val_hr == "23")
        {
            hrr = "11";
        }
        else if (val_hr == "24")
        {
            hrr = "12";
        }
        if (val_hr == "10" || val_hr == "11" || val_hr == "12")
        {

            ddl_hour1.Text = val_hr;
            ddl_minits1.Text = ay[1].ToString();

            ddl_endhour1.Text = val_hr;
            ddl_endminit1.Text = ay[1].ToString();
        }
        else
        {

            ddl_hour1.Text = hrr;
            ddl_minits1.Text = ay[1].ToString();

            ddl_endhour1.Text = hrr;
            ddl_endminit1.Text = ay[1].ToString();
        }

        if (val_hr == "12" || val_hr == "13" || val_hr == "14" || val_hr == "15" || val_hr == "16" || val_hr == "17" || val_hr == "18" || val_hr == "19" || val_hr == "20" || val_hr == "21" || val_hr == "22" || val_hr == "23" || val_hr == "24")
        {


            ddl_timeformate1.Text = "PM";
            ddl_endformate1.Text = "PM";
        }
        else
        {

            ddl_timeformate1.Text = "AM";
            ddl_endformate1.Text = "AM";
        }


    }
    public DateTime TextToDate(TextBox txt)
    {
        DateTime dt = new DateTime();
        string firstdate = Convert.ToString(txt.Text);

        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        return dt;
    }
    public void btn_actadd_Click(object sender, EventArgs e)
    {
        imgdiv33.Visible = true;
        panel_description11.Visible = true;
    }
    public void pop_add_staff_stud_othrclose1_Click(object sender, EventArgs e)
    {
        string val1 = "";
        for (int i = 0; i < GridView12.Rows.Count; i++)
        {
            CheckBox checkvalue1 = (CheckBox)GridView12.Rows[i].FindControl("chkup3");
            if (checkvalue1.Checked == true)
            {
                Label stud_appno = (Label)GridView12.Rows[i].FindControl("lblappno");
                val1 = Convert.ToString(stud_appno.Text);
                if (persentedstaff == "")
                {
                    persentedstaff = val1;
                }
                else
                {
                    persentedstaff = persentedstaff + "'" + "," + "'" + val1;
                }
            }
        }

        for (int i = 0; i < GridView13.Rows.Count; i++)
        {
            CheckBox checkvalue1 = (CheckBox)GridView13.Rows[i].FindControl("chkup3");
            if (checkvalue1.Checked == true)
            {
                Label stud_appno = (Label)GridView13.Rows[i].FindControl("lblappno");
                val1 = Convert.ToString(stud_appno.Text);

                if (persentedstud == "")
                {
                    persentedstud = val1;
                }
                else
                {
                    persentedstud = persentedstud + "'" + "," + "'" + val1;
                }
            }
        }
        indi_comp();
        pop_add_staff_stud_othr1.Visible = false;
    }
    public void rdp_prsnt_staff_CheckedChanged(object sender, EventArgs e)
    {
        div_prsnt_stud.Visible = false;
        div_prsnt_otherss.Visible = false;
        div_prsnt_staff.Visible = true;
        div_pesnt_com.Visible = false;
    }
    public void rdo_prsnt_stud_CheckedChanged(object sender, EventArgs e)
    {
        div_prsnt_stud.Visible = true;
        div_prsnt_otherss.Visible = false;
        div_prsnt_staff.Visible = false;
        div_pesnt_com.Visible = false;
    }
    public void rdo_prsnt_othr_CheckedChanged(object sender, EventArgs e)
    {
        div_prsnt_stud.Visible = false;
        div_prsnt_otherss.Visible = true;
        div_prsnt_staff.Visible = false;
        div_pesnt_com.Visible = false;
        btn_go_prsntclik.Visible = true;
    }
    public void rdo_prsnt_com_CheckedChanged(object sender, EventArgs e)
    {
        div_prsnt_stud.Visible = false;
        div_prsnt_otherss.Visible = false;
        div_prsnt_staff.Visible = false;
        div_pesnt_com.Visible = true;
        btn_go_prsntclik.Visible = true;
    }
    public void cb_prsnt_degreeChekedChange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";
            if (cb_prsnt_degree.Checked == true)
            {
                for (int i = 0; i < cbl_prsnt_degree.Items.Count; i++)
                {
                    if (cb_prsnt_degree.Checked == true)
                    {
                        cbl_prsnt_degree.Items[i].Selected = true;
                        txt_prsnt_degree.Text = "Degree(" + (cbl_prsnt_degree.Items.Count) + ")";
                        build1 = cbl_prsnt_degree.Items[i].Value.ToString();
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
                bindbranch2(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_prsnt_degree.Items.Count; i++)
                {
                    cbl_prsnt_degree.Items[i].Selected = false;
                    txt_prsnt_degree.Text = "--Select--";
                    txt_prsnt_branch.Text = "--Select--";
                    cbl_prsnt_branch.ClearSelection();
                    cb_presnt_branch.Checked = false;
                }
            }
            bindbranch2(college);
            // Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_prsnt_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_prsnt_degree.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_prsnt_degree.Items.Count; i++)
            {
                if (cbl_prsnt_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    //  txt_branch.Text = "--Select--";
                    build = cbl_prsnt_degree.Items[i].Value.ToString();
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
            bindbranch2(buildvalue);
            if (seatcount == cbl_prsnt_degree.Items.Count)
            {
                txt_prsnt_degree.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree2.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_prsnt_degree.Text = "--Select--";
                txt_prsnt_degree.Text = "--Select--";
            }
            else
            {
                txt_prsnt_degree.Text = "Degree(" + seatcount.ToString() + ")";
            }
            // bindbranch(college);
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch2(string branch)
    {
        try
        {
            cbl_prsnt_branch.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < cbl_prsnt_degree.Items.Count; i++)
            {
                if (cbl_prsnt_degree.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_prsnt_degree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_prsnt_degree.Items[i].Value.ToString() + "";
                    }
                }
            }
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            if (itemheader.Trim() != "")
            {
                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_prsnt_branch.DataSource = ds;
                    cbl_prsnt_branch.DataTextField = "dept_name";
                    cbl_prsnt_branch.DataValueField = "degree_code";
                    cbl_prsnt_branch.DataBind();



                    if (cbl_prsnt_branch.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_prsnt_branch.Items.Count; i++)
                        {
                            cbl_prsnt_branch.Items[i].Selected = true;
                        }
                        txt_prsnt_branch.Text = "Branch(" + cbl_prsnt_branch.Items.Count + ")";
                    }
                }
                else
                {
                    txt_prsnt_branch.Text = "--Select--";
                }
            }
            else
            {
                txt_prsnt_branch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_prsnt_branch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_presnt_branch.Checked == true)
            {
                for (int i = 0; i < cbl_prsnt_branch.Items.Count; i++)
                {
                    cbl_prsnt_branch.Items[i].Selected = true;
                }
                txt_prsnt_branch.Text = "Branch(" + (cbl_prsnt_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_prsnt_branch.Items.Count; i++)
                {
                    cbl_prsnt_branch.Items[i].Selected = false;
                }
                txt_prsnt_branch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_prsnt_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_prsnt_branch.Text = "--Select--";
            cb_presnt_branch.Checked = false;
            for (int i = 0; i < cbl_prsnt_branch.Items.Count; i++)
            {
                if (cbl_prsnt_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_prsnt_branch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cbl_prsnt_branch.Items.Count)
                {
                    cb_presnt_branch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_prsnt_studgo_Click(object sender, EventArgs e)
    {
        prsnt_studbind();
    }
    public void prsnt_studbind()
    {
        try
        {
            string selectquery = "";
            int sno = 0;

            string itemheader = "";

            for (int i = 0; i < cbl_prsnt_branch.Items.Count; i++)
            {
                if (cbl_prsnt_branch.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_prsnt_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_prsnt_branch.Items[i].Value.ToString() + "";
                    }
                }
            }

            string batch_year = Convert.ToString(ddl_prsnt_batch.SelectedItem.Text);

            if (itemheader.Trim() != "" && batch_year.Trim() != "")
            {

                if (txt_rollno1.Text == "")
                {
                    //selectquery = "select Roll_No,Roll_Admit,Stud_Name,App_No,d.Degree_Code ,(C.Course_Name +' - '+ dt.dept_acronym) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "')   ";
                    selectquery = "select Roll_No,Stud_Name,App_No,dt.Dept_Name  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "') order by dt.Dept_Code   ";
                    //and r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' ) 
                }
                else
                {
                    //selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.Dept_Name) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "')  order by Roll_No,d.Degree_Code ";
                    selectquery = "select Roll_No,Stud_Name,App_No  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and  r.Roll_No ='" + txt_rollno1.Text + "'";
                    // r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' ) and
                }


                ds1 = d2.select_method_wo_parameter(selectquery, "Text");

                for (int row1 = 0; row1 < ds1.Tables[0].Rows.Count; row1++)
                {

                    GridView13.DataSource = ds1;
                    GridView13.DataBind();

                }
                GridView13.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void Fpspread3_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }
    protected void loadfsstaff2()
    {
        try
        {
            Label41.Visible = false;
            string sql = "";

            string bindspread = sql;

            string design_name = string.Empty;
            string dept_all = string.Empty;
            string design_all = string.Empty;
            string itemheader = "";
            string designation = "";
            string dept = "";
            for (int i = 0; i < cbl_stafftype.Items.Count; i++)
            {
                if (cbl_stafftype.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_stafftype.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_stafftype.Items[i].Value.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_staff_desng.Items.Count; i++)
            {
                if (cbl_staff_desng.Items[i].Selected == true)
                {
                    if (designation == "")
                    {
                        designation = "" + cbl_staff_desng.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        designation = designation + "'" + "," + "" + "'" + cbl_staff_desng.Items[i].Value.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_staffdeprt.Items.Count; i++)
            {
                if (cbl_staffdeprt.Items[i].Selected == true)
                {
                    if (dept == "")
                    {
                        dept = "" + cbl_staffdeprt.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        dept = dept + "'" + "," + "" + "'" + cbl_staffdeprt.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (particpentstaff == "")
            {
                particpentstaff = "0";
            }
            string Sql_Query = "select distinct s.staff_code,s.staff_name,sm.appl_id,h.dept_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and s.appl_no=sm.appl_no  and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode and h.dept_code in ( '" + dept + "')  and d.desig_name in ('" + designation + "') and s.college_code='" + ddl_prnst_staff_clg.SelectedValue.ToString() + "'   and stftype in('" + itemheader + "') and resign = 0 and settled = 0 and latestrec=1 and sm.appl_id  not in('" + particpentstaff + "')";
            DataSet dsbindspread = new DataSet();

            dsbindspread = da.select_method_wo_parameter(Sql_Query, "Text");
            if (dsbindspread.Tables[0].Rows.Count > 0)
            {


                GridView12.DataSource = dsbindspread;
                GridView12.DataBind();


            }
            else
            {
                GridView12.Visible = false;

            }
        }
        catch
        {
        }
    }
    public void btn_actmin_Click(object sender, EventArgs e)
    {
        if (ddl_actname.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
        else if (ddl_actname.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select any record";
        }
        else if (ddl_actname.SelectedIndex != 0)
        {
            string sql = "delete from CO_MasterValues where MasterCode='" + ddl_actname.SelectedItem.Value.ToString() + "' and MasterCriteria='EventAction' and CollegeCode='" + collegecode1 + "' ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Sucessfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No records found";
            }
            loadaction();
        }

        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
    }

    public void loadaction()
    {
        ddl_actname.Items.Clear();
        ds.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='EventAction' and CollegeCode ='" + collegecode1 + "'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_actname.DataSource = ds;
            ddl_actname.DataTextField = "MasterValue";
            ddl_actname.DataValueField = "MasterCode";
            ddl_actname.DataBind();
            ddl_actname.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_actname.Items.Insert(0, new ListItem("Select", "0"));
        }

    }
    public void btn_matrlrequest_Click(object sender, EventArgs e)
    {
        divmr.Visible = true;
        reloadothers();
    }
    public void btn_expense_Click(object sender, EventArgs e)
    {
        div_expence.Visible = true;
        reloadothers();
    }
    public void btn_spncmp_go_Click(object sender, EventArgs e)
    {

        int rowIndex = 0;
        string name = Convert.ToString(txt_sponscmp_name.Text);
        string res = Convert.ToString(txt_sponc_contact.Text);
        string amt = Convert.ToString(txt_sponc_amount.Text);
        if (txt_sponscmp_name.Text != "" && txt_sponc_contact.Text != "" && txt_sponc_amount.Text != "")
        {
            if (ViewState["CurrentTable666"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable666"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    TextBox box1 = new TextBox();
                    TextBox box2 = new TextBox();
                    TextBox box3 = new TextBox();


                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {

                        box1 = (TextBox)GV4.Rows[i].Cells[1].FindControl("txtcname");
                        box2 = (TextBox)GV4.Rows[i].Cells[2].FindControl("txtcnt");
                        box3 = (TextBox)GV4.Rows[i].Cells[3].FindControl("txtamt1");


                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i][0] = box1.Text;
                        dtCurrentTable.Rows[i][1] = box2.Text;
                        dtCurrentTable.Rows[i][2] = box3.Text;


                        rowIndex++;

                    }
                    drCurrentRow[0] = name;
                    drCurrentRow[1] = res;
                    drCurrentRow[2] = amt;

                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable666"] = dtCurrentTable;

                    GV4.DataSource = dtCurrentTable;
                    GV4.DataBind();
                }
            }
            else
            {
                BindGridview4();
            }
        }
        else
        {

            divdown.Visible = true;
            lbl_divdown1.Text = "Fill All The Column";
        }
        txt_sponscmp_name.Text = "";
        txt_sponc_contact.Text = "";
        txt_sponc_amount.Text = "";
    }
    public void BindGridview4()
    {
        GV4.Visible = true;
        string name = Convert.ToString(txt_sponscmp_name.Text);
        string res = Convert.ToString(txt_sponc_contact.Text);
        string amt = Convert.ToString(txt_sponc_amount.Text);
        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");

        DataRow dr;

        dr = dt.NewRow();
        dr[0] = name;
        dr[1] = res;
        dr[2] = amt;


        dt.Rows.Add(dr);
        ViewState["CurrentTable666"] = dt;
        GV4.DataSource = dt;
        GV4.DataBind();
    }
    public void cb_staffdeprt_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_staffdeprt.Text = "--Select--";
        if (cb_staffdeprt.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_staffdeprt.Items.Count; i++)
            {
                cbl_staffdeprt.Items[i].Selected = true;
            }
            txt_staffdeprt.Text = "Dept(" + (cbl_staffdeprt.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_staffdeprt.Items.Count; i++)
            {
                cbl_staffdeprt.Items[i].Selected = false;
            }
        }

    }
    public void cbl_staffdeprt_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;

        int commcount = 0;
        txt_staffdeprt.Text = "--Select--";
        for (i = 0; i < cbl_staffdeprt.Items.Count; i++)
        {
            if (cbl_staffdeprt.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_staffdeprt.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_staffdeprt.Items.Count)
            {
                cb_staffdeprt.Checked = true;
            }
            txt_staffdeprt.Text = "Dept(" + commcount.ToString() + ")";
        }

    }
    public void cb_stafftype_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_staff_type.Text = "--Select--";
        if (cb_stafftype.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_stafftype.Items.Count; i++)
            {
                cbl_stafftype.Items[i].Selected = true;
            }
            txt_staff_type.Text = "Staff Type(" + (cbl_stafftype.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_stafftype.Items.Count; i++)
            {
                cbl_stafftype.Items[i].Selected = false;
            }
        }
        bind_design2();
    }
    public void cbl_stafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;

        int commcount = 0;
        txt_staff_type.Text = "--Select--";
        for (i = 0; i < cbl_stafftype.Items.Count; i++)
        {
            if (cbl_stafftype.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_stafftype.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_stafftype.Items.Count)
            {
                cb_stafftype.Checked = true;
            }
            txt_staff_type.Text = "Staff Type(" + commcount.ToString() + ")";
        }
        bind_design2();
    }
    public void cb_staff_desng_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_staff_desgn.Text = "--Select--";
        if (cb_staff_desng.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_staff_desng.Items.Count; i++)
            {
                cbl_staff_desng.Items[i].Selected = true;
            }
            txt_staff_desgn.Text = "Designation(" + (cbl_staff_desng.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_staff_desng.Items.Count; i++)
            {
                cbl_staff_desng.Items[i].Selected = false;
            }
        }

    }
    public void cbl_staff_desng_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;

        int commcount = 0;
        txt_staff_desgn.Text = "--Select--";
        for (i = 0; i < cbl_staff_desng.Items.Count; i++)
        {
            if (cbl_staff_desng.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_staff_desng.Checked = false;
            }
        }

        if (commcount > 0)
        {
            if (commcount == cbl_staff_desng.Items.Count)
            {
                cb_staff_desng.Checked = true;
            }
            txt_staff_desgn.Text = "Designation(" + commcount.ToString() + ")";
        }
    }
    public void bind_design2()
    {
        try
        {
            string sql = string.Empty;

            string itemheader = "";
            for (int i = 0; i < cbl_stafftype.Items.Count; i++)
            {
                if (cbl_stafftype.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_stafftype.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_stafftype.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (txt_staff_type.Text == "Select")
            {
                sql = "SELECT distinct Desig_Name FROM StaffTrans T,staffmaster m,Desig_Master G WHERE t.staff_code = m.staff_code and T.Desig_Code = G.Desig_Code AND Latestrec = 1 and G.collegecode=" + Session["collegecode"] + "";
            }
            else
            {
                sql = "SELECT distinct Desig_Name FROM StaffTrans T,staffmaster m,Desig_Master G WHERE t.staff_code = m.staff_code and T.Desig_Code = G.Desig_Code AND Latestrec = 1 and G.collegecode=" + Session["collegecode"] + " and stftype in('" + itemheader + "')";
            }

            ds.Clear();
            ds = da.select_method_wo_parameter(sql, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {

                cbl_staff_desng.DataSource = ds;
                cbl_staff_desng.DataTextField = "Desig_Name";
                cbl_staff_desng.DataValueField = "Desig_Name";
                cbl_staff_desng.DataBind();

                if (cbl_staff_desng.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staff_desng.Items.Count; i++)
                    {
                        cbl_staff_desng.Items[i].Selected = true;
                    }
                    txt_staff_desgn.Text = "Designation(" + cbl_staff_desng.Items.Count + ")";
                }

            }
        }
        catch
        {
        }
    }
    public void bind_design1()
    {
        try
        {
            string sql = string.Empty;

            string itemheader = "";
            for (int i = 0; i < cbl_staff_type111.Items.Count; i++)
            {
                if (cbl_staff_type111.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_staff_type111.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_staff_type111.Items[i].Value.ToString() + "";
                    }
                }
            }

            sql = "SELECT distinct Desig_Name FROM StaffTrans T,staffmaster m,Desig_Master G WHERE t.staff_code = m.staff_code and T.Desig_Code = G.Desig_Code AND Latestrec = 1 and G.collegecode=" + Session["collegecode"] + " and stftype in('" + itemheader + "')";


            ds.Clear();
            ds = da.select_method_wo_parameter(sql, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staff_desn11.DataSource = ds;
                cbl_staff_desn11.DataTextField = "Desig_Name";
                cbl_staff_desn11.DataValueField = "Desig_Name";
                cbl_staff_desn11.DataBind();


                if (cbl_staff_desn11.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staff_desn11.Items.Count; i++)
                    {
                        cbl_staff_desn11.Items[i].Selected = true;
                    }
                    txt_staff_desg111.Text = "Designation(" + cbl_staff_desn11.Items.Count + ")";
                }

            }
        }
        catch
        {
        }
    }
    public void rdbinst_CheckedChanged(object sender, EventArgs e)
    {
        gv33div.Visible = true;
        GridView3.Visible = false;

        POP_GV3_DIV.Visible = false;
        POP_GV4_DIV.Visible = false;
        POP_GV6_DIV.Visible = false;

    }
    protected void rdbdept_CheckedChanged(object sender, EventArgs e)
    {
        gv33div.Visible = false;
        POP_GV3_DIV.Visible = true;

        POP_GV4_DIV.Visible = false;
        POP_GV6_DIV.Visible = false;

    }
    protected void rdbsponser_CheckedChanged(object sender, EventArgs e)
    {
        POP_GV3_DIV.Visible = false;
        POP_GV4_DIV.Visible = true;
        POP_GV6_DIV.Visible = false;
        gv33div.Visible = false;


    }
    public void ImageButton5popclose1_Click(object sender, EventArgs e)
    {
        pop_others.Visible = false;
    }
    public void btn_ins_go_Click(object sender, EventArgs e)
    {

        int rowIndex = 0;
        string name = Convert.ToString(txt_inst_name.Text);
        string res = Convert.ToString(txt_ins_resource.Text);
        string amt = Convert.ToString(txt_ins_amount.Text);
        if (txt_inst_name.Text != "" && txt_ins_resource.Text != "" && txt_ins_amount.Text != "")
        {
            if (ViewState["CurrentTable444"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable444"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    TextBox box1 = new TextBox();
                    TextBox box2 = new TextBox();
                    TextBox box3 = new TextBox();

                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {

                        box1 = (TextBox)GridView3.Rows[i].Cells[1].FindControl("txtdname");
                        box2 = (TextBox)GridView3.Rows[i].Cells[2].FindControl("txtresource");
                        box3 = (TextBox)GridView3.Rows[i].Cells[3].FindControl("txtamt");

                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i][0] = box1.Text;
                        dtCurrentTable.Rows[i][1] = box2.Text;
                        dtCurrentTable.Rows[i][2] = box3.Text;

                        rowIndex++;

                    }
                    drCurrentRow[0] = name;
                    drCurrentRow[1] = res;
                    drCurrentRow[2] = amt;

                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable444"] = dtCurrentTable;

                    GridView3.DataSource = dtCurrentTable;
                    GridView3.DataBind();
                }
            }
            else
            {
                BindGridview33();
            }
        }
        else
        {
            divdown.Visible = true;
            lbl_divdown1.Text = "Fill All The Columns";
        }
        reloadothers();
        txt_inst_name.Text = "";
        txt_ins_resource.Text = "";
        txt_ins_amount.Text = "";
    }
    public void BindGridview33()
    {

        GridView3.Visible = true;
        string name = Convert.ToString(txt_inst_name.Text);
        string res = Convert.ToString(txt_ins_resource.Text);
        string amt = Convert.ToString(txt_ins_amount.Text);
        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");

        DataRow dr;

        dr = dt.NewRow();
        dr[0] = name;
        dr[1] = res;
        dr[2] = amt;


        dt.Rows.Add(dr);
        ViewState["CurrentTable444"] = dt;
        if (dt.Rows.Count > 0)
        {
            GridView3.DataSource = dt;
            GridView3.DataBind();
        }


    }
    public void btn_dept_go_Click(object sender, EventArgs e)
    {

        int rowIndex = 0;
        string name = Convert.ToString(txt_departmentname.Text);
        string res = Convert.ToString(txt_dept_resource.Text);
        string amt = Convert.ToString(txt_dept_amt.Text);
        if (txt_departmentname.Text != "" && txt_dept_resource.Text != "" && txt_dept_amt.Text != "")
        {
            if (ViewState["CurrentTable888"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable888"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    TextBox box1 = new TextBox();
                    TextBox box2 = new TextBox();
                    TextBox box3 = new TextBox();

                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {

                        box1 = (TextBox)GV3.Rows[i].Cells[1].FindControl("txtdname");
                        box2 = (TextBox)GV3.Rows[i].Cells[2].FindControl("txtresource");
                        box3 = (TextBox)GV3.Rows[i].Cells[3].FindControl("txtamt");

                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i][0] = box1.Text;
                        dtCurrentTable.Rows[i][1] = box2.Text;
                        dtCurrentTable.Rows[i][2] = box3.Text;

                        rowIndex++;

                    }
                    drCurrentRow[0] = name;
                    drCurrentRow[1] = res;
                    drCurrentRow[2] = amt;

                    dtCurrentTable.Rows.Add(drCurrentRow);

                    ViewState["CurrentTable888"] = dtCurrentTable;

                    GV3.DataSource = dtCurrentTable;
                    GV3.DataBind();
                }
            }
            else
            {
                BindGridview3();
            }
        }
        else
        {

            divdown.Visible = true;
            lbl_divdown1.Text = "Fill All The Column";
        }
        reloadothers();
        txt_departmentname.Text = "";
        txt_dept_resource.Text = "";
        txt_dept_amt.Text = "";
    }
    public void BindGridview3()
    {

        GV3.Visible = true;
        string name = Convert.ToString(txt_departmentname.Text);
        string res = Convert.ToString(txt_dept_resource.Text);
        string amt = Convert.ToString(txt_dept_amt.Text);
        DataTable dt = new DataTable();
        dt.Columns.Add("Dummy");
        dt.Columns.Add("Dummy1");
        dt.Columns.Add("Dummy2");
        dt.Columns.Add("Dummy3");

        DataRow dr;

        dr = dt.NewRow();
        dr[0] = name;
        dr[1] = res;
        dr[2] = amt;


        dt.Rows.Add(dr);
        ViewState["CurrentTable888"] = dt;
        GV3.DataSource = dt;
        GV3.DataBind();
    }
    public void btnerrclose1_Click(object sender, EventArgs e)
    {
        divclosess.Visible = false;

        pop_add_staff_stud_othr.Visible = true;
    }
    public void btn_event_app_Click(object sender, EventArgs e)
    {
        try
        {
            string actionvalue = "";
            string description = "";
            string location = "";
            string RequestCode = "";
            string query = "";
            string stime = "";
            string etime = "";
            Int64 ReqStaffAppNo = 0;
            Int64 ReqStaffDeptFK = 0;
            bool Is_Staff;
            string loctype = "";
            string common = "";
            string eventtype = "";
            if (rdb1.Checked == true)
            {
                loctype = "0";
            }
            else if (rdb2.Checked == true)
            {
                loctype = "1";
            }
            if (rdo_commpati.Checked == true)
            {
                common = "0";
            }
            else if (rdo_indivparti.Checked == true)
            {
                common = "1";
            }
            if (rdo_single.Checked == true)
            {
                eventtype = "0";
            }
            else if (rdo_multipl.Checked == true)
            {
                eventtype = "1";
            }
            dept();
            if (txtothers.Text.Trim() == "")
            {
                imgdiv2.Visible = true;
                pnl2.Visible = true;
                lbl_alert.Text = "Kindly Select The Event Name";
                return;
            }
            Is_Staff = Convert.ToBoolean(d2.GetFunction("select Is_Staff from UserMaster where User_Code='" + usercode + "' and college_code='" + collegecode1 + "'"));
            if (Is_Staff == true)
            {
                string staffcode = d2.GetFunction("select staff_code  from UserMaster where User_Code='" + usercode + "'");
                if (staffcode.Trim() != "")
                {
                    ReqStaffAppNo = Convert.ToInt64(d2.GetFunction("select appl_id  from staff_appl_master a, staffmaster s where a.appl_no=s.appl_no and staff_code='" + staffcode + "'"));
                }
            }
            else if (Is_Staff == false)
            {
                ReqStaffAppNo = Convert.ToInt64(usercode);
            }
            ReqStaffDeptFK = Convert.ToInt64(d2.GetFunction("select distinct hr.dept_code as DeptCode,hr.Dept_Name as DeptName from hrdept_master hr,UserMaster um,staffmaster sm,staff_appl_master sa where um.staff_code=sm.staff_code and sm.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and um.is_staff=1 and um.staff_code='" + usercode + "' and hr.college_code ='" + collegecode1 + "'"));

            int RequestType = 7;

            DateTime RequestDate = new DateTime();
            RequestDate = TextToDate(TextBox6);
            DateTime RequesfromDate = new DateTime();
            RequesfromDate = TextToDate(txtfd);
            DateTime RequestoDate = new DateTime();
            RequestoDate = TextToDate(txttd);
            RequestCode = rqustn_no_event.Text;
            DateTime eventdate = new DateTime();
            eventdate = TextToDate(txt_min_startdate);
            string startprd = Convert.ToString(txt_min_startperiod.Text);
            string endprd = Convert.ToString(txt_min_endperiod.Text);
            string delivery_mode1 = Convert.ToString(txtothers.Text);
            string eventname = subjectcodenew("EventName", delivery_mode1);
            string noofact = Convert.ToString(txt_min_action.Text);
            event_requestcode = RequestCode;
            string rq_fk = "";
            if (txtothers.Text.Trim() != "")
            {
                //...........RQ_Requisition save..........
                query = "insert into RQ_Requisition(RequestType,RequestCode,RequestDate,ReqAppNo,ReqApproveStage,MemType,ReqFromDate,ReqToDate,ReqLocType,IsCommonParticipate,EventType,ReqEventName,college_code) values('" + RequestType + "','" + RequestCode + "','" + RequestDate + "','" + ReqStaffAppNo + "','0','2','" + RequesfromDate + "','" + RequestoDate + "','" + loctype + "','" + common + "','" + eventtype + "','" + eventname + "','" + collegecode1 + "')";
                d2.update_method_wo_parameter(query, "Text");
                //...........................RQ_ReqEventDet SAVE.................................................

                save2();
                if (rdo_single.Checked == true)
                {

                    //....................RQ_RequisitionDet save................
                    string locationvalue = Convert.ToString(txt_min_location.Text);
                    string bul = "";
                    string flr = "";
                    string rm = "";
                    if (rdb2.Checked == false)
                    {

                        string[] split = locationvalue.Split('-');
                        if (split.Length == 1)
                        {

                        }
                        else
                        {

                            bul = split[0];
                            flr = split[1];
                            rm = split[2];
                        }
                    }
                    string locationvalue1 = "";
                    string outdoorloc = "";
                    if (rdb2.Checked == true)
                    {
                        locationvalue1 = Convert.ToString(txt_min_location.Text);
                    }
                    else
                    {
                        locationvalue1 = "";
                    }

                    string starthr = Convert.ToString(ddl_hour1.SelectedItem.Text);
                    string startmin = Convert.ToString(ddl_minits1.Text);
                    string startday = Convert.ToString(ddl_timeformate1.Text);
                    string endhr = Convert.ToString(ddl_endhour1.SelectedItem.Text);
                    string endmin = Convert.ToString(ddl_endminit1.Text);
                    string endday = Convert.ToString(ddl_endformate1.Text);
                    string starttime = starthr + ":" + startmin + ":" + startday;
                    string endtime = endhr + ":" + endmin + ":" + endday;
                    rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");
                    query = "insert into RQ_RequisitionDet(RequisitionFK,EventDate,EventName,StartPeriod,EndPeriod,EventLocation,StartTime,EndTime,LocationType,BuildCode,OutdoorLoc,FloorNo,RoomNo,NoOfAction)values('" + rq_fk + "','" + eventdate + "','" + eventname + "','" + startprd + "','" + endprd + "','" + locationvalue1 + "','" + starttime + "','" + endtime + "','" + loctype + "','" + bul + "','" + locationvalue + "','" + flr + "','" + rm + "','" + noofact + "')";
                    d2.update_method_wo_parameter(query, "Text");
                    upload();

                    //.....................RQ_ReqActionDet save......................


                    for (int ii = 0; ii < gridadd.Rows.Count; ii++)
                    {
                        //if (ii == row1.DataItemIndex)
                        //{
                        TextBox txtact = (TextBox)gridadd.Rows[ii].FindControl("txtactname");
                        actionvalue = txtact.Text;

                        TextBox txtdes = (TextBox)gridadd.Rows[ii].FindControl("txt_descri");
                        description = txtdes.Text;

                        TextBox txtloc = (TextBox)gridadd.Rows[ii].FindControl("txt_loc");
                        location = txtloc.Text;

                        TextBox txtstart = (TextBox)gridadd.Rows[ii].FindControl("txt_start");
                        stime = txtstart.Text;
                        TextBox txtend = (TextBox)gridadd.Rows[ii].FindControl("txt_end");
                        etime = txtend.Text;
                        if (rdb1.Checked == true)
                        {

                        }
                        else
                        {
                            outdoorloc = Convert.ToString(txt_min_location.Text);
                        }

                        string getact = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + actionvalue + "'"); ;
                        string querynew = "insert into RQ_ReqActionDet(ActionName,ACtionDesc,StartTime,EndTime,LocationType,OutdoorLoc,BuildCode,FloorNo,RoomNo,RequisitionFK,EventDate)values('" + getact + "','" + description + "','" + stime + "','" + etime + "','" + loctype + "','" + outdoorloc + "','" + bul + "','" + flr + "','" + rm + "','" + rq_fk + "','" + eventdate + "')";
                        d2.update_method_wo_parameter(querynew, "Text");

                    }

                    ////....................RQ_EventMemberDet save.................................

                    save1();
                }
                else
                {
                    multiple_event_save();
                }

                //.....................RQ_EventMaterialReq save.................


                string pur_status = "";
                string edate = "";
                string item_qty = "";
                string p_status = "";
                string itemname = "";
                for (int i = 0; i < GridView5.Rows.Count; i++)
                {
                    TextBox txtitemname = (TextBox)GridView5.Rows[i].FindControl("txt_name");
                    itemname = Convert.ToString(txtitemname.Text);

                    TextBox txtname = (TextBox)GridView5.Rows[i].FindControl("txt_qty");
                    item_qty = Convert.ToString(txtname.Text);
                    TextBox status = (TextBox)GridView5.Rows[i].FindControl("tx_inmax");
                    pur_status = Convert.ToString(status.Text);
                    if (pur_status.Trim() == "To Be Purchase")
                    {
                        p_status = "0";
                    }
                    else
                    {
                        p_status = "1";
                    }
                    TextBox date = (TextBox)GridView5.Rows[i].FindControl("txt_exp");
                    DateTime expdate = new DateTime();
                    expdate = TextToDate(date);
                    rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");
                    string itempk = d2.GetFunction("select ItemPK from IM_ItemMaster WHERE ItemName='" + itemname + "'");
                    query = "insert into RQ_EventMaterialReq(ItemFK,ReqQty,ExpectedDate,PurchaseStatus,RequisitionFK)values('" + itempk + "','" + item_qty + "','" + expdate + "','" + p_status + "','" + rq_fk + "')";
                    d2.update_method_wo_parameter(query, "Text");
                }
                //.................RQ_EventExpensesDet save..................

                string itemamt = "";
                string itemdes = "";
                string expnc = "";
                if (ddl_expnc_name.SelectedItem.Value != "Select")
                {
                    if (ddl_expnc_name.SelectedItem.Value != "Others")
                    {
                        expnc = Convert.ToString(ddl_expnc_name.SelectedItem.Value);
                    }
                    else
                    {
                        string doc_prty1 = Convert.ToString(txt_expn_name.Text);
                        expnc = subjectcodenew("Expense", doc_prty1);
                    }
                }
                for (int i = 0; i < GridView7.Rows.Count; i++)
                {
                    TextBox txtname = (TextBox)GridView7.Rows[i].FindControl("txtcname");
                    expnc = Convert.ToString(txtname.Text);
                    expnc = subjectcodenew("Expense", expnc);
                    TextBox txtdes = (TextBox)GridView7.Rows[i].FindControl("txtcnt");
                    itemdes = Convert.ToString(txtdes.Text);
                    TextBox txtamt = (TextBox)GridView7.Rows[i].FindControl("txtamt1");
                    itemamt = Convert.ToString(txtamt.Text);
                    // string expnc_val = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Expense' and MasterValue='" + expnc + "'");
                    query = "insert into RQ_EventExpensesDet(ExpensesName,ExpensesDesc,ExpAmount,RequisitionFK)values('" + expnc + "','" + itemdes + "','" + itemamt + "','" + rq_fk + "')";
                    d2.update_method_wo_parameter(query, "Text");
                }

                string venfk = "";
                string vencontackfk = "";
                string vendorname = "";
                //........................RQ_EventSponserDet save..................
                string SponserType = "";
                string SponserAmount = "";
                string Sponseresource = "";
                Sponser_save();
                string spnr_query = "";
                rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");
                if (rdbinst.Checked == true)
                {
                    SponserType = "1";
                    for (int i = 0; i < GridView3.Rows.Count; i++)
                    {
                        TextBox txtspresource = (TextBox)GridView3.Rows[i].FindControl("txtresource");
                        Sponseresource = Convert.ToString(txtspresource.Text);
                        TextBox txtamtval = (TextBox)GridView3.Rows[i].FindControl("txtamt");
                        SponserAmount = Convert.ToString(txtamtval.Text);
                        TextBox tctname = (TextBox)GridView3.Rows[i].FindControl("txtdname");
                        vendorname = Convert.ToString(tctname.Text);
                        venfk = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorType='5' and VendorCompName='" + vendorname + "'");
                        vencontackfk = d2.GetFunction("select VendorContactPK from IM_VendorContactMaster where VendorFK='" + venfk + "'");
                        spnr_query = "insert into RQ_EventSponserDet(SponserType,VendorFK,VendorContactFK,SponserAmount,RequisitionFK,resources) values('" + SponserType + "','" + venfk + "','" + vencontackfk + "','" + SponserAmount + "','" + rq_fk + "','" + Sponseresource + "')";
                        d2.update_method_wo_parameter(spnr_query, "Text");
                    }

                }
                else if (rdbdept.Checked == true)
                {
                    SponserType = "2";

                    for (int i = 0; i < GV3.Rows.Count; i++)
                    {
                        TextBox txtspresource = (TextBox)GV3.Rows[i].FindControl("txtresource");
                        Sponseresource = Convert.ToString(txtspresource.Text);
                        TextBox txtamtval = (TextBox)GV3.Rows[i].FindControl("txtamt");
                        SponserAmount = Convert.ToString(txtamtval.Text);
                        TextBox tctname = (TextBox)GV3.Rows[i].FindControl("txtdname");
                        vendorname = Convert.ToString(tctname.Text);
                        venfk = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorType='8' and VendorCompName='" + vendorname + "'");
                        vencontackfk = d2.GetFunction("select VendorContactPK from IM_VendorContactMaster where VendorFK='" + venfk + "'");
                        spnr_query = "insert into RQ_EventSponserDet(SponserType,VendorFK,VendorContactFK,SponserAmount,RequisitionFK,resources) values('" + SponserType + "','" + venfk + "','" + vencontackfk + "','" + SponserAmount + "','" + rq_fk + "','" + Sponseresource + "')";
                        d2.update_method_wo_parameter(spnr_query, "Text");
                    }
                }
                else if (rdbsponser.Checked == true)
                {
                    SponserType = "3";
                    for (int i = 0; i < GV4.Rows.Count; i++)
                    {
                        TextBox txtspresource = (TextBox)GV4.Rows[i].FindControl("txtcnt");
                        Sponseresource = Convert.ToString(txtspresource.Text);
                        TextBox txtamtval = (TextBox)GV4.Rows[i].FindControl("txtamt1");
                        SponserAmount = Convert.ToString(txtamtval.Text);
                        TextBox tctname = (TextBox)GV4.Rows[i].FindControl("txtcname");
                        vendorname = Convert.ToString(tctname.Text);

                        venfk = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorType='6' and VendorCompName='" + vendorname + "'");
                        vencontackfk = d2.GetFunction("select VendorContactPK from IM_VendorContactMaster where VendorFK='" + venfk + "'");
                        spnr_query = "insert into RQ_EventSponserDet(SponserType,VendorFK,VendorContactFK,SponserAmount,RequisitionFK,resources) values('" + SponserType + "','" + venfk + "','" + vencontackfk + "','" + SponserAmount + "','" + rq_fk + "','" + Sponseresource + "')";
                        d2.update_method_wo_parameter(spnr_query, "Text");
                    }
                }
                else if (rdbcompany.Checked == true)
                {
                    SponserType = "4";
                    for (int i = 0; i < GridView6.Rows.Count; i++)
                    {
                        TextBox txtspresource = (TextBox)GridView6.Rows[i].FindControl("txtcnt");
                        Sponseresource = Convert.ToString(txtspresource.Text);
                        TextBox txtamtval = (TextBox)GridView6.Rows[i].FindControl("txtamt1");
                        SponserAmount = Convert.ToString(txtamtval.Text);
                        TextBox tctname = (TextBox)GridView6.Rows[i].FindControl("txtcname");
                        vendorname = Convert.ToString(tctname.Text);
                        venfk = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorType='4' and VendorCompName='" + vendorname + "'");
                        vencontackfk = d2.GetFunction("select VendorContactPK from IM_VendorContactMaster where VendorFK='" + venfk + "'");
                        spnr_query = "insert into RQ_EventSponserDet(SponserType,VendorFK,VendorContactFK,SponserAmount,RequisitionFK,resources) values('" + SponserType + "','" + venfk + "','" + vencontackfk + "','" + SponserAmount + "','" + rq_fk + "','" + Sponseresource + "')";
                        d2.update_method_wo_parameter(spnr_query, "Text");
                    }
                }



                //////.............................RQ_EventPreRequest save.....................
                string actname = "";
                string actdes = "";
                string repby = "";
                rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");
                for (int i = 0; i < GridView4.Rows.Count; i++)
                {
                    TextBox txtname = (TextBox)GridView4.Rows[i].FindControl("txtact");
                    actname = Convert.ToString(txtname.Text);
                    TextBox txtdes = (TextBox)GridView4.Rows[i].FindControl("txt_actname");
                    actdes = Convert.ToString(txtdes.Text);

                    TextBox txtrep = (TextBox)GridView4.Rows[i].FindControl("txt_repsen");
                    repby = Convert.ToString(txtrep.Text);

                    TextBox sdate = (TextBox)GridView4.Rows[i].FindControl("txt_startdate");
                    DateTime st_date1 = new DateTime();
                    st_date1 = TextToDate(sdate);

                    TextBox txtend = (TextBox)GridView4.Rows[i].FindControl("txt_enddate");
                    DateTime enddate = new DateTime();
                    enddate = TextToDate(txtend);
                    string reprsnt_by = d2.GetFunction("select distinct sa.appl_id from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name='" + repby + "'");
                    if (reprsnt_by == "0" || reprsnt_by == "")
                    {
                        reprsnt_by = d2.GetFunction("select app_no from applyn where stud_name='" + repby + "'");
                    }
                    query = "insert into RQ_EventPreRequest(ActivityName,ActivityDesc,StartDate,EndDate,RepresentApllNo,RequisitionFK)values('" + actname + "','" + actdes + "','" + st_date1 + "','" + enddate + "','" + reprsnt_by + "','" + rq_fk + "')";
                    d2.update_method_wo_parameter(query, "Text");
                }

                //....................batch,deg,branch,sem.............................
                if (rdb1.Checked == true)
                {
                    string staffname = "";
                    string degree = "";
                    string sem = "";
                    string batch = Convert.ToString(ddl_org_batch.SelectedItem.Value);
                    if (rdo_orgstudent.Checked == true)
                    {
                        for (int i = 0; i < cbl_branch.Items.Count; i++)
                        {
                            if (cbl_branch.Items[i].Selected == true)
                            {
                                if (degree == "")
                                {
                                    degree = "" + cbl_branch.Items[i].Value.ToString() + "";
                                }
                                for (int k = 0; k < cbl_or_sem.Items.Count; k++)
                                {
                                    if (cbl_or_sem.Items[k].Selected == true)
                                    {
                                        if (sem == "")
                                        {
                                            sem = "" + cbl_or_sem.Items[k].Text.ToString() + "";
                                        }
                                    }

                                }
                                rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");
                                query = "insert into RQ_EventMemberDet(BatchYear,DegreeCode,Semester,RequisitionFK)values('" + batch + "','" + degree + "','" + sem + "','" + rq_fk + "')";
                                d2.update_method_wo_parameter(query, "Text");

                            }
                        }
                    }

                    ///..............................org by..............................
                    ///
                    rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");
                    if (rdo_org_staff.Checked == true)
                    {
                        string staffapp = "";
                        if (txtissueper.Text == "")
                        {
                            for (int i = 0; i < cbl_staff_name.Items.Count; i++)
                            {
                                if (cbl_staff_name.Items[i].Selected == true)
                                {

                                    staffname = "" + cbl_staff_name.Items[i].Value.ToString() + "";
                                    staffapp = d2.GetFunction("select distinct sa.appl_id from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + staffname + "' ");
                                    string DegreeCode = d2.GetFunction("select distinct degree.degree_code from degree,department,course,deptprivilages,staff_appl_master  where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and appl_id='" + staffapp + "' ");
                                    query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionType,DegreeCode)values('" + staffapp + "','" + rq_fk + "','3','" + DegreeCode + "')";
                                    d2.update_method_wo_parameter(query, "Text");

                                }
                            }
                        }
                        else
                        {
                            string[] ast = txtissueper.Text.Split('-');
                            string txtstaffname = ast[0];
                            staffapp = d2.GetFunction("select distinct sa.appl_id from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and sa.appl_name='" + txtstaffname + "' ");
                            query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionType)values('" + staffapp + "','" + rq_fk + "','3')";
                            d2.update_method_wo_parameter(query, "Text");
                        }
                    }
                    else
                    {
                        string staffapp = "";
                        if (txt_studenorgsearch.Text == "")
                        {
                            for (int i = 0; i < cb1_studentorgby.Items.Count; i++)
                            {
                                if (cb1_studentorgby.Items[i].Selected == true)
                                {

                                    staffname = "" + cb1_studentorgby.Items[i].Value.ToString() + "";
                                    staffapp = d2.GetFunction("select app_no from Registration where Roll_No='" + staffname + "' ");
                                    query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionType)values('" + staffapp + "','" + rq_fk + "','3')";
                                    d2.update_method_wo_parameter(query, "Text");

                                }
                            }
                        }
                        else
                        {
                            string[] ast = txt_studenorgsearch.Text.Split('-');
                            string txtstaffname = ast[0];
                            staffapp = d2.GetFunction("select app_no from Registration where Stud_Name='" + txtstaffname + "' ");
                            query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionType)values('" + staffapp + "','" + rq_fk + "','3')";
                            d2.update_method_wo_parameter(query, "Text");
                        }
                    }
                }
                else
                {
                    rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");
                    string institutionname = "";
                    if (ddl_outinstitution.SelectedItem.Value != "Select")
                    {
                        if (ddl_outinstitution.SelectedItem.Value != "Others")
                        {
                            institutionname = Convert.ToString(ddl_outinstitution.SelectedItem.Value);
                        }
                        else
                        {
                            string doc_prty1 = Convert.ToString(txt_outinstitution.Text);
                            institutionname = subjectcodenew("Institution", doc_prty1);
                        }
                    }

                    string outquery = "insert into RQ_EventMemberDet(RequisitionFK,Institution)values('" + rq_fk + "','" + institutionname + "')";
                    d2.update_method_wo_parameter(outquery, "Text");

                    string outorganizername = "";
                    if (ddl_outorganiser.SelectedItem.Value != "Select")
                    {
                        if (ddl_outorganiser.SelectedItem.Value != "Others")
                        {
                            outorganizername = Convert.ToString(ddl_outorganiser.SelectedItem.Value);
                        }
                        else
                        {
                            string doc_prty1 = Convert.ToString(txt_outorganz.Text);
                            outorganizername = subjectcodenew("organizer", doc_prty1);
                        }
                    }

                    string outquery1 = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,MemType)values('" + outorganizername + "','" + rq_fk + "','3')";
                    d2.update_method_wo_parameter(outquery1, "Text");

                }


                lbl_alert.Visible = true;

                imgdiv2.Visible = true;
                pnl2.Visible = true;
                lbl_alert.Text = "Saved Successfully";

                event_gridclear();
                event_clearall();
                pop_radiodiv.Visible = false;
                pop_radiodiv.Visible = false;
                rdb_Papers.Checked = false;
                rdb_Paper.Checked = false;
                rdb_Patents.Checked = false;
                rdb_Conference.Checked = false;
                rdb_Award.Checked = false;
                rdb_student.Checked = false;
                rdb_ReSearch.Checked = false;
                rdb_Membership.Checked = false;
                rdb_Distinguished.Checked = false;
                rdb_Tournamentk.Checked = false;
                rdb_Symposium.Checked = false;
                RDB_OTHERS.Checked = false;
                clear_multipeve();
                txtfd.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtfd.Attributes.Add("readonly", "readonly");
                txttd.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttd.Attributes.Add("readonly", "readonly");
                txtfd.Enabled = false;
                txttd.Enabled = false;
                txtdays.Text = "1";
                rdo_single.Checked = true;
                imgdiv2.Visible = true;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Select The EventName";
            }
        }
        catch
        {
        }
    }
    public void save1()
    {
        string cname = "";
        string pname = "";
        string addr = "";
        string street = "";
        string city = "";
        string pin = "";
        string country = "";
        string state = "";
        string phn = "";
        string mail = "";
        string attch = "";
        string doc = "";
        int atd = 0;
        string VendorCode = "";
        string query = "";
        string appno;
        string memtype = "";
        string actionname = "";
        string staffname = "";

        string batch = Convert.ToString(ddl_org_batch.SelectedItem.Value);
        string degree = "";
        string sem = "";
        string actionfk = "";
        string getapp = "";
        string checkvalue = "";
        string val = "";
        string rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");


        for (int ii = 0; ii < gridadd.Rows.Count; ii++)
        {
            TextBox txtact = (TextBox)gridadd.Rows[ii].FindControl("txtactname");
            actionname = txtact.Text;

            //.........................................................................................
            if (rdo_commpati.Checked == true)
            {
                for (int i = 0; i < GridView2.Rows.Count; i++)
                {
                    CheckBox chkItemHeader = (CheckBox)GridView2.Rows[i].FindControl("chkup3");

                    if (chkItemHeader.Checked == true)
                    {
                        Label stud_appno = (Label)GridView2.Rows[i].FindControl("lblappno");
                        val = Convert.ToString(stud_appno.Text);

                        string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + actionname + "'");
                        actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where RequisitionFK='" + rq_fk + "' and ActionName='" + app + "'))");
                        query = "insert into RQ_EventMemberDet(MemType,ApplNo,RequisitionFK,ActionFK,ActionType)values('2','" + val + "','" + rq_fk + "','" + actionfk + "','1')";
                        d2.update_method_wo_parameter(query, "Text");
                    }
                }



                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    CheckBox chkItemHeader = (CheckBox)GridView1.Rows[i].FindControl("chkup3");

                    if (chkItemHeader.Checked == true)
                    {

                        Label stud_appno = (Label)GridView1.Rows[i].FindControl("lblappno");
                        val = Convert.ToString(stud_appno.Text);

                        string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + actionname + "'");
                        actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where RequisitionFK='" + rq_fk + "' and ActionName='" + app + "'))");
                        query = "insert into RQ_EventMemberDet(MemType,ApplNo,RequisitionFK,ActionFK,ActionType)values('1','" + val + "','" + rq_fk + "','" + actionfk + "','1')";
                        d2.update_method_wo_parameter(query, "Text");
                    }
                }

                for (int k = 0; k < GridView9.Rows.Count; k++)
                {
                    TextBox txtamt = (TextBox)GridView9.Rows[k].FindControl("txtactname");
                    cname = Convert.ToString(txtamt.Text);
                    TextBox txtpanam = (TextBox)GridView9.Rows[k].FindControl("txt_per");
                    pname = Convert.ToString(txtpanam.Text);
                    TextBox txtadd = (TextBox)GridView9.Rows[k].FindControl("txt_add");
                    addr = Convert.ToString(txtadd.Text);
                    TextBox txtst = (TextBox)GridView9.Rows[k].FindControl("txt_st");
                    street = Convert.ToString(txtst.Text);
                    TextBox txtcity = (TextBox)GridView9.Rows[k].FindControl("txt_city");
                    city = Convert.ToString(txtcity.Text);
                    TextBox txtpin = (TextBox)GridView9.Rows[k].FindControl("txt_pin");
                    pin = Convert.ToString(txtpin.Text);
                    TextBox txtcou = (TextBox)GridView9.Rows[k].FindControl("txt_country");
                    country = Convert.ToString(txtcou.Text);
                    string country1 = subjectcodenew("coun", country);
                    TextBox txtstate = (TextBox)GridView9.Rows[k].FindControl("txt_state");
                    state = Convert.ToString(txtstate.Text);
                    string state1 = subjectcodenew("state", state);
                    TextBox txtphn = (TextBox)GridView9.Rows[k].FindControl("txt_phn");
                    phn = Convert.ToString(txtphn.Text);
                    TextBox txtmail = (TextBox)GridView9.Rows[k].FindControl("txt_mail");
                    mail = Convert.ToString(txtmail.Text);
                    TextBox txtattch = (TextBox)GridView9.Rows[k].FindControl("txt_attch");
                    attch = Convert.ToString(txtattch.Text);
                    TextBox txtat = (TextBox)GridView9.Rows[k].FindControl("txt_e");
                    int at = Convert.ToInt32(txtat.Text);
                    TextBox txtdoc = (TextBox)GridView9.Rows[k].FindControl("txt_dt");
                    doc = Convert.ToString(txtdoc.Text);
                    string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=7 and VendorCompName like '" + cname + "%'");

                    if (VenCode != "" && VenCode != null && VenCode != "0")
                    {
                        VendorCode = VenCode;
                    }
                    else
                    {
                        VendorCodeGen();
                        VendorCode = Session["VendorCode"].ToString();
                        string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + cname + "' and VendorType='7') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + cname + "',VendorType='7',VendorAddress='" + addr + "',VendorStreet='" + street + "',VendorCity='" + city + "',VendorPin='" + pin + "',VendorName='" + pname + "',VendorPhoneNo='" + phn + "',VendorEmailID='" + mail + "',VendorCountry='" + country1 + "',VendorState='" + state1 + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + cname + "' and VendorType='7' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry,VendorState) values('" + VendorCode + "','" + cname + "','7','" + addr + "','" + street + "','" + city + "','" + pin + "','" + pname + "','" + phn + "','" + mail + "','" + country1 + "','" + state1 + "')";
                        int vc = d2.update_method_wo_parameter(venmst, "TEXT");
                    }
                    Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
                    string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + pname + "',VendorMobileNo='" + phn + "',VendorPhoneNo='" + phn + "',VendorEmail='" + mail + "' where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorMobileNo,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + pname + "','" + phn + "','" + phn + "','" + mail + "')";
                    int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");


                    bool savnotsflag = false;
                    if (attch != "" && at != 0)
                    {

                        string fileName = attch;

                        int fileSize = at;

                        byte[] documentBinary = new byte[fileSize];


                        string date = DateTime.Now.ToString("MM/dd/yyyy");
                        SqlCommand cmdnotes = new SqlCommand();

                        rq_fk1 = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                        //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                        cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + VendorFK + "'";
                        cmdnotes.CommandType = CommandType.Text;
                        cmdnotes.Connection = ssql;

                        SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                        DocName.Value = fileName.ToString();
                        cmdnotes.Parameters.Add(DocName);

                        SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                        Type.Value = doc.ToString();
                        cmdnotes.Parameters.Add(Type);

                        SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                        uploadedDocument.Value = documentBinary;
                        cmdnotes.Parameters.Add(uploadedDocument);


                        ssql.Close();
                        ssql.Open();
                        int result = cmdnotes.ExecuteNonQuery();
                        if (result > 0)
                        {
                            savnotsflag = true;
                        }

                    }

                    string vk = d2.GetFunction("select VendorContactPK from CO_VendorMaster co,IM_VendorContactMaster im where co.VendorCode='" + VendorCode + "' and VendorType=7 and im.VenContactName='" + pname + "'");
                    string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + actionname + "'");
                    actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where RequisitionFK='" + rq_fk + "' and ActionName='" + app + "'))");
                    query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionFK,ActionType,MemType)values('" + vk + "','" + rq_fk + "','" + actionfk + "','1','3')";
                    d2.update_method_wo_parameter(query, "Text");

                }
                for (int k = 0; k < GridView8.Rows.Count; k++)
                {
                    TextBox txtamt = (TextBox)GridView8.Rows[k].FindControl("txtactname");
                    cname = Convert.ToString(txtamt.Text);
                    TextBox txtpanam = (TextBox)GridView8.Rows[k].FindControl("txt_per");
                    pname = Convert.ToString(txtpanam.Text);
                    TextBox txtadd = (TextBox)GridView8.Rows[k].FindControl("txt_add");
                    addr = Convert.ToString(txtadd.Text);
                    TextBox txtst = (TextBox)GridView8.Rows[k].FindControl("txt_st");
                    street = Convert.ToString(txtst.Text);
                    TextBox txtcity = (TextBox)GridView8.Rows[k].FindControl("txt_city");
                    city = Convert.ToString(txtcity.Text);
                    TextBox txtpin = (TextBox)GridView8.Rows[k].FindControl("txt_pin");
                    pin = Convert.ToString(txtpin.Text);
                    TextBox txtcou = (TextBox)GridView8.Rows[k].FindControl("txt_country");
                    country = Convert.ToString(txtcou.Text);
                    string country1 = subjectcodenew("coun", country);
                    TextBox txtstate = (TextBox)GridView8.Rows[k].FindControl("txt_state");
                    state = Convert.ToString(txtstate.Text);
                    string state1 = subjectcodenew("state", state);
                    TextBox txtphn = (TextBox)GridView8.Rows[k].FindControl("txt_phn");
                    phn = Convert.ToString(txtphn.Text);
                    TextBox txtmail = (TextBox)GridView8.Rows[k].FindControl("txt_mail");
                    mail = Convert.ToString(txtmail.Text);
                    TextBox txtattch = (TextBox)GridView8.Rows[k].FindControl("txt_attch");
                    attch = Convert.ToString(txtattch.Text);
                    TextBox txtat = (TextBox)GridView8.Rows[k].FindControl("txt_e");
                    int at = Convert.ToInt32(txtat.Text);
                    TextBox txtdoc = (TextBox)GridView8.Rows[k].FindControl("txt_dt");
                    doc = Convert.ToString(txtdoc.Text);
                    string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=4 and VendorCompName like '" + cname + "%'");

                    if (VenCode != "" && VenCode != null && VenCode != "0")
                    {
                        VendorCode = VenCode;
                    }
                    else
                    {
                        VendorCodeGen();
                        VendorCode = Session["VendorCode"].ToString();
                        string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + cname + "' and VendorType='4') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + cname + "',VendorType='4',VendorAddress='" + addr + "',VendorStreet='" + street + "',VendorCity='" + city + "',VendorPin='" + pin + "',VendorName='" + pname + "',VendorPhoneNo='" + phn + "',VendorEmailID='" + mail + "',VendorCountry='" + country1 + "',VendorState='" + state1 + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + cname + "' and VendorType='4' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry,VendorState) values('" + VendorCode + "','" + cname + "','4','" + addr + "','" + street + "','" + city + "','" + pin + "','" + pname + "','" + phn + "','" + mail + "','" + country1 + "','" + state1 + "')";
                        int vc = d2.update_method_wo_parameter(venmst, "TEXT");
                    }
                    Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
                    string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + pname + "',VendorMobileNo='" + phn + "',VendorPhoneNo='" + phn + "',VendorEmail='" + mail + "' where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorMobileNo,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + pname + "','" + phn + "','" + phn + "','" + mail + "')";
                    int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");


                    bool savnotsflag = false;
                    if (attch != "" && at != 0)
                    {

                        string fileName = attch;

                        int fileSize = at;

                        byte[] documentBinary = new byte[fileSize];


                        string date = DateTime.Now.ToString("MM/dd/yyyy");
                        SqlCommand cmdnotes = new SqlCommand();

                        rq_fk1 = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                        //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                        cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + VendorFK + "'";
                        cmdnotes.CommandType = CommandType.Text;
                        cmdnotes.Connection = ssql;

                        SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                        DocName.Value = fileName.ToString();
                        cmdnotes.Parameters.Add(DocName);

                        SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                        Type.Value = doc.ToString();
                        cmdnotes.Parameters.Add(Type);

                        SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                        uploadedDocument.Value = documentBinary;
                        cmdnotes.Parameters.Add(uploadedDocument);


                        ssql.Close();
                        ssql.Open();
                        int result = cmdnotes.ExecuteNonQuery();
                        if (result > 0)
                        {
                            savnotsflag = true;
                        }


                    }


                    string vk = d2.GetFunction("select VendorContactPK from CO_VendorMaster co,IM_VendorContactMaster im where co.VendorCode='" + VendorCode + "' and VendorType=4 and im.VenContactName='" + pname + "'");
                    string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + actionname + "'");
                    actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where RequisitionFK='" + rq_fk + "' and ActionName='" + app + "'))");
                    query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionFK,ActionType,MemType)values('" + vk + "','" + rq_fk + "','" + actionfk + "','1','4')";
                    d2.update_method_wo_parameter(query, "Text");
                }
            }

            //.................................................................................................
            if (rdo_indivparti.Checked == true)
            {
                if (newparticipant.Count > 0)
                {
                    appno = Convert.ToString(newparticipant[actionname]);

                    string[] array = appno.Split(',');

                    for (int j = 0; j < array.Length; j++)
                    {
                        string mem = d2.GetFunction("select appl_id from staff_appl_master where appl_id IN ('" + array[j] + "')");
                        if (mem != "0")
                        {
                            memtype = "2";

                        }
                        else
                        {
                            string mem1 = d2.GetFunction("select App_No from Registration where App_No IN('" + array[j] + "')");

                            memtype = "1";

                        }

                        string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + actionname + "'");
                        actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where ActionName='" + app + "'))");
                        query = "insert into RQ_EventMemberDet(MemType,BatchYear,DegreeCode,Semester,ApplNo,RequisitionFK,ActionFK,ActionType)values('" + memtype + "','','','','" + array[j] + "','" + rq_fk + "','" + actionfk + "','1')";
                        d2.update_method_wo_parameter(query, "Text");
                    }

                }

            }
            if (rdo_indivparti.Checked == true)
            {
                if (singleparticcomp.Count > 0)
                {
                    appno = Convert.ToString(singleparticcomp[actionname]);
                    if (appno != "")
                    {
                        string[] array = appno.Split(',');

                        for (int j = 0; j < array.Length; j++)
                        {
                            string totindi = array[j];

                            string[] arr = totindi.Split('-');

                            cname = arr[0];
                            pname = arr[1];
                            addr = arr[2];
                            street = arr[3];
                            city = arr[4];
                            pin = arr[5];
                            country = arr[6];
                            string country1 = subjectcodenew("coun", country);
                            state = arr[7];
                            string state1 = subjectcodenew("state", state);
                            phn = arr[8];
                            mail = arr[9];
                            attch = arr[10];
                            string atttt = arr[11];
                            doc = arr[12];
                            atd = Convert.ToInt32(atttt);

                            string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=4 and VendorCompName like '" + cname + "%'");

                            if (VenCode != "" && VenCode != null && VenCode != "0")
                            {
                                VendorCode = VenCode;
                            }
                            else
                            {
                                VendorCodeGen();
                                VendorCode = Session["VendorCode"].ToString();
                                string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + cname + "' and VendorType='4') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + cname + "',VendorType='4',VendorAddress='" + addr + "',VendorStreet='" + street + "',VendorCity='" + city + "',VendorPin='" + pin + "',VendorName='" + pname + "',VendorPhoneNo='" + phn + "',VendorEmailID='" + mail + "',VendorCountry='" + country1 + "',VendorState='" + state1 + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + cname + "' and VendorType='4' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry,VendorState) values('" + VendorCode + "','" + cname + "','4','" + addr + "','" + street + "','" + city + "','" + pin + "','" + pname + "','" + phn + "','" + mail + "','" + country1 + "','" + state1 + "')";
                                int vc = d2.update_method_wo_parameter(venmst, "TEXT");
                            }
                            Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
                            string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + pname + "',VendorMobileNo='" + phn + "',VendorPhoneNo='" + phn + "',VendorEmail='" + mail + "' where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorMobileNo,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + pname + "','" + phn + "','" + phn + "','" + mail + "')";
                            int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");

                            bool savnotsflag = false;
                            if (attch != "" && atd != 0)
                            {

                                string fileName = attch;

                                int fileSize = atd;

                                byte[] documentBinary = new byte[fileSize];


                                string date = DateTime.Now.ToString("MM/dd/yyyy");
                                SqlCommand cmdnotes = new SqlCommand();

                                rq_fk1 = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                                //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                                cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + VendorFK + "'";
                                cmdnotes.CommandType = CommandType.Text;
                                cmdnotes.Connection = ssql;

                                SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                                DocName.Value = fileName.ToString();
                                cmdnotes.Parameters.Add(DocName);

                                SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                                Type.Value = doc.ToString();
                                cmdnotes.Parameters.Add(Type);

                                SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                                uploadedDocument.Value = documentBinary;
                                cmdnotes.Parameters.Add(uploadedDocument);


                                ssql.Close();
                                ssql.Open();
                                int result = cmdnotes.ExecuteNonQuery();
                                if (result > 0)
                                {
                                    savnotsflag = true;
                                }

                            }

                            string vk = d2.GetFunction("select VendorContactPK from CO_VendorMaster co,IM_VendorContactMaster im where co.VendorCode='" + VendorCode + "' and VendorType=4 and im.VenContactName='" + pname + "'");
                            string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + actionname + "'");
                            actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where RequisitionFK='" + rq_fk + "' and ActionName='" + app + "'))");
                            query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionFK,ActionType,MemType)values('" + vk + "','" + rq_fk + "','" + actionfk + "','1','4')";
                            d2.update_method_wo_parameter(query, "Text");
                        }
                    }
                }

                if (singleparticindi.Count > 0)
                {
                    appno = Convert.ToString(singleparticindi[actionname]);
                    if (appno != "")
                    {
                        string[] array = appno.Split(',');

                        for (int j = 0; j < array.Length; j++)
                        {
                            string totindi = array[j];

                            string[] arr = totindi.Split('-');

                            cname = arr[0];
                            pname = arr[1];
                            addr = arr[2];
                            street = arr[3];
                            city = arr[4];
                            pin = arr[5];
                            country = arr[6];
                            string country1 = subjectcodenew("coun", country);
                            state = arr[7];
                            string state1 = subjectcodenew("state", state);
                            phn = arr[8];
                            mail = arr[9];
                            attch = arr[10];
                            string atttt = arr[11];
                            doc = arr[12];
                            atd = Convert.ToInt32(atttt);

                            string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=7 and VendorCompName like '" + cname + "%'");

                            if (VenCode != "" && VenCode != null && VenCode != "0")
                            {
                                VendorCode = VenCode;
                            }
                            else
                            {
                                VendorCodeGen();
                                VendorCode = Session["VendorCode"].ToString();
                                string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + cname + "' and VendorType='7') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + cname + "',VendorType='7',VendorAddress='" + addr + "',VendorStreet='" + street + "',VendorCity='" + city + "',VendorPin='" + pin + "',VendorName='" + pname + "',VendorPhoneNo='" + phn + "',VendorEmailID='" + mail + "',VendorCountry='" + country1 + "',VendorState='" + state1 + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + cname + "' and VendorType='7' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry,VendorState) values('" + VendorCode + "','" + cname + "','7','" + addr + "','" + street + "','" + city + "','" + pin + "','" + pname + "','" + phn + "','" + mail + "','" + country1 + "','" + state1 + "')";
                                int vc = d2.update_method_wo_parameter(venmst, "TEXT");
                            }
                            Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
                            string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + pname + "',VendorMobileNo='" + phn + "',VendorPhoneNo='" + phn + "',VendorEmail='" + mail + "' where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorMobileNo,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + pname + "','" + phn + "','" + phn + "','" + mail + "')";
                            int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");

                            bool savnotsflag = false;
                            if (attch != "" && atd != 0)
                            {

                                string fileName = attch;

                                int fileSize = atd;

                                byte[] documentBinary = new byte[fileSize];


                                string date = DateTime.Now.ToString("MM/dd/yyyy");
                                SqlCommand cmdnotes = new SqlCommand();

                                rq_fk1 = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                                //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                                cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + VendorFK + "'";
                                cmdnotes.CommandType = CommandType.Text;
                                cmdnotes.Connection = ssql;

                                SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                                DocName.Value = fileName.ToString();
                                cmdnotes.Parameters.Add(DocName);

                                SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                                Type.Value = doc.ToString();
                                cmdnotes.Parameters.Add(Type);

                                SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                                uploadedDocument.Value = documentBinary;
                                cmdnotes.Parameters.Add(uploadedDocument);


                                ssql.Close();
                                ssql.Open();
                                int result = cmdnotes.ExecuteNonQuery();
                                if (result > 0)
                                {
                                    savnotsflag = true;
                                }

                            }

                            string vk = d2.GetFunction("select VendorContactPK from CO_VendorMaster co,IM_VendorContactMaster im where co.VendorCode='" + VendorCode + "' and VendorType=7 and im.VenContactName='" + pname + "'");
                            string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + actionname + "'");
                            actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where RequisitionFK='" + rq_fk + "' and ActionName='" + app + "'))");
                            query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionFK,ActionType,MemType)values('" + vk + "','" + rq_fk + "','" + actionfk + "','1','3')";
                            d2.update_method_wo_parameter(query, "Text");
                        }
                    }
                }
            }


            ////........................ presented person save..................
            if (newpresented.Count > 0)
            {
                appno = Convert.ToString(newpresented[actionname]);
                if (appno != "")
                {
                    string[] array = appno.Split(',');

                    for (int j = 0; j < array.Length; j++)
                    {
                        string arr = array[j];
                        string[] array1 = arr.Split('-');
                        string array_staff = array1[0];
                        string array_catg = array1[1];
                        string array_catg_code = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='EventCategory' and MasterValue='" + array_catg + "' ");

                        string mem = d2.GetFunction("select distinct s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code ='" + array_staff + "'");
                        if (mem != "0")
                        {
                            memtype = "1";

                        }
                        else
                        {
                            string mem1 = d2.GetFunction("select App_No from Registration where App_No='" + array_staff + "'");

                            memtype = "2";


                        }
                        string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + actionname + "'");
                        actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where ActionName='" + app + "'))");
                        query = "insert into RQ_EventMemberDet(MemType,ApplNo,MemberAction,RequisitionFK,ActionFK,ActionType)values('" + memtype + "','" + array_staff + "','" + array_catg + "','" + rq_fk + "','" + actionfk + "','2')";
                        d2.update_method_wo_parameter(query, "Text");
                    }
                    ///....................................................................
                }
            }



            if (singlepresentindi.Count > 0)
            {
                appno = Convert.ToString(singlepresentindi[actionname]);
                if (appno != "")
                {
                    string[] array = appno.Split(',');

                    for (int j = 0; j < array.Length; j++)
                    {
                        string totindi = array[j];

                        string[] arr = totindi.Split('-');

                        cname = arr[0];
                        pname = arr[1];
                        addr = arr[2];
                        street = arr[3];
                        city = arr[4];
                        pin = arr[5];
                        country = arr[6];
                        string country1 = subjectcodenew("coun", country);
                        state = arr[7];
                        string state1 = subjectcodenew("state", state);
                        phn = arr[8];
                        mail = arr[9];
                        attch = arr[10];
                        string atttt = arr[11];
                        doc = arr[12];
                        atd = Convert.ToInt32(atttt);

                        string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=7 and VendorCompName like '" + cname + "%'");

                        if (VenCode != "" && VenCode != null && VenCode != "0")
                        {
                            VendorCode = VenCode;
                        }
                        else
                        {
                            VendorCodeGen();
                            VendorCode = Session["VendorCode"].ToString();
                            string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + cname + "' and VendorType='7') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + cname + "',VendorType='7',VendorAddress='" + addr + "',VendorStreet='" + street + "',VendorCity='" + city + "',VendorPin='" + pin + "',VendorName='" + pname + "',VendorPhoneNo='" + phn + "',VendorEmailID='" + mail + "',VendorCountry='" + country1 + "',VendorState='" + state1 + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + cname + "' and VendorType='7' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry,VendorState) values('" + VendorCode + "','" + cname + "','7','" + addr + "','" + street + "','" + city + "','" + pin + "','" + pname + "','" + phn + "','" + mail + "','" + country1 + "','" + state1 + "')";
                            int vc = d2.update_method_wo_parameter(venmst, "TEXT");
                        }
                        Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
                        string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + pname + "',VendorMobileNo='" + phn + "',VendorPhoneNo='" + phn + "',VendorEmail='" + mail + "' where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorMobileNo,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + pname + "','" + phn + "','" + phn + "','" + mail + "')";
                        int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");

                        bool savnotsflag = false;
                        if (attch != "" && atd != 0)
                        {

                            string fileName = attch;

                            int fileSize = atd;

                            byte[] documentBinary = new byte[fileSize];


                            string date = DateTime.Now.ToString("MM/dd/yyyy");
                            SqlCommand cmdnotes = new SqlCommand();

                            rq_fk1 = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                            //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                            cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + VendorFK + "'";
                            cmdnotes.CommandType = CommandType.Text;
                            cmdnotes.Connection = ssql;

                            SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                            DocName.Value = fileName.ToString();
                            cmdnotes.Parameters.Add(DocName);

                            SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                            Type.Value = doc.ToString();
                            cmdnotes.Parameters.Add(Type);

                            SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                            uploadedDocument.Value = documentBinary;
                            cmdnotes.Parameters.Add(uploadedDocument);


                            ssql.Close();
                            ssql.Open();
                            int result = cmdnotes.ExecuteNonQuery();
                            if (result > 0)
                            {
                                savnotsflag = true;
                            }



                        }


                        string vk = d2.GetFunction("select VendorContactPK from CO_VendorMaster co,IM_VendorContactMaster im where co.VendorCode='" + VendorCode + "' and VendorType=7 and im.VenContactName='" + pname + "'");
                        string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + actionname + "'");
                        actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where RequisitionFK='" + rq_fk + "' and ActionName='" + app + "'))");
                        query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionFK,ActionType,MemType)values('" + vk + "','" + rq_fk + "','" + actionfk + "','2','3')";
                        d2.update_method_wo_parameter(query, "Text");
                    }
                }
            }


            if (singlepresentcomp.Count > 0)
            {
                appno = Convert.ToString(singlepresentcomp[actionname]);

                if (appno != "")
                {
                    string[] array = appno.Split(',');

                    for (int j = 0; j < array.Length; j++)
                    {
                        string totindi = array[j];

                        string[] arr = totindi.Split('-');

                        cname = arr[0];
                        pname = arr[1];
                        addr = arr[2];
                        street = arr[3];
                        city = arr[4];
                        pin = arr[5];
                        country = arr[6];
                        string country1 = subjectcodenew("coun", country);
                        state = arr[7];
                        string state1 = subjectcodenew("state", state);
                        phn = arr[8];
                        mail = arr[9];
                        attch = arr[10];
                        string atttt = arr[11];
                        doc = arr[12];
                        atd = Convert.ToInt32(atttt);
                        string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=4 and VendorCompName like '" + cname + "%'");

                        if (VenCode != "" && VenCode != null && VenCode != "0")
                        {
                            VendorCode = VenCode;
                        }
                        else
                        {
                            VendorCodeGen();
                            VendorCode = Session["VendorCode"].ToString();
                            string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + cname + "' and VendorType='4') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + cname + "',VendorType='4',VendorAddress='" + addr + "',VendorStreet='" + street + "',VendorCity='" + city + "',VendorPin='" + pin + "',VendorName='" + pname + "',VendorPhoneNo='" + phn + "',VendorEmailID='" + mail + "',VendorCountry='" + country1 + "',VendorState='" + state1 + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + cname + "' and VendorType='4' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry,VendorState) values('" + VendorCode + "','" + cname + "','4','" + addr + "','" + street + "','" + city + "','" + pin + "','" + pname + "','" + phn + "','" + mail + "','" + country1 + "','" + state1 + "')";
                            int vc = d2.update_method_wo_parameter(venmst, "TEXT");
                        }
                        Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
                        string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + pname + "',VendorMobileNo='" + phn + "',VendorPhoneNo='" + phn + "',VendorEmail='" + mail + "' where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorMobileNo,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + pname + "','" + phn + "','" + phn + "','" + mail + "')";
                        int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");

                        bool savnotsflag = false;
                        if (attch != "" && atd != 0)
                        {

                            string fileName = attch;

                            int fileSize = atd;

                            byte[] documentBinary = new byte[fileSize];


                            string date = DateTime.Now.ToString("MM/dd/yyyy");
                            SqlCommand cmdnotes = new SqlCommand();

                            rq_fk1 = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                            //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                            cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + VendorFK + "'";
                            cmdnotes.CommandType = CommandType.Text;
                            cmdnotes.Connection = ssql;

                            SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                            DocName.Value = fileName.ToString();
                            cmdnotes.Parameters.Add(DocName);

                            SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                            Type.Value = doc.ToString();
                            cmdnotes.Parameters.Add(Type);

                            SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                            uploadedDocument.Value = documentBinary;
                            cmdnotes.Parameters.Add(uploadedDocument);


                            ssql.Close();
                            ssql.Open();
                            int result = cmdnotes.ExecuteNonQuery();
                            if (result > 0)
                            {
                                savnotsflag = true;
                            }

                        }


                        string vk = d2.GetFunction("select VendorContactPK from CO_VendorMaster co,IM_VendorContactMaster im where co.VendorCode='" + VendorCode + "' and VendorType=4 and im.VenContactName='" + pname + "'");
                        string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + actionname + "'");
                        actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where RequisitionFK='" + rq_fk + "' and ActionName='" + app + "'))");
                        query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionFK,ActionType,MemType)values('" + vk + "','" + rq_fk + "','" + actionfk + "','2','4')";
                        d2.update_method_wo_parameter(query, "Text");
                    }
                }
            }
        }
    }


    public void save2()
    {
        try
        {
            string isnational = "";
            string query = "";
            string journal = Convert.ToString(txt_jour.Text);
            string impact = Convert.ToString(txt_impact.Text);
            string confnc = Convert.ToString(txt_confn.Text);
            string patenno = Convert.ToString(txt_patentnumb.Text);
            string appno = Convert.ToString(txt_patentappno.Text);
            string appstatus = Convert.ToString(txt_patentappstatus.Text);
            string prz = Convert.ToString(txt_won.Text);
            string dur = Convert.ToString(txt_dur.Text);
            string work = Convert.ToString(txt_work.Text);
            string schlr = Convert.ToString(txt_shlr.Text);
            string prg = Convert.ToString(txt_nameofprg.Text);
            string Mainsup = Convert.ToString(txt_supervisormain.Text);
            string cosup = Convert.ToString(txt_co_sup.Text);
            string Society = Convert.ToString(txt_society.Text);
            string mem_det = Convert.ToString(txt_membership.Text);
            string visit = Convert.ToString(txt_namevist.Text);
            string org = Convert.ToString(txt_org.Text);
            string pur = Convert.ToString(txt_pur_vist.Text);
            string tour = Convert.ToString(txt_nametour.Text);
            string gst = Convert.ToString(txt_gsttit.Text);
            string wkshop = Convert.ToString(txt_workshop.Text);
            string cnfrnc = Convert.ToString(txt_seminartit.Text);
            string title = "";
            string seminartype = "";
            string awdcat = "";
            string toutype = "";
            string gametype = "";
            string actionname = "";
            DateTime appdate = new DateTime();
            appdate = TextToDate(txt_patenappdate);
            if (RDB_OTHERS.Checked == true)
            {
                actionname = Convert.ToString(ddl_actname.SelectedItem.Value);
            }
            else
            {
                actionname = "";
            }
            if (ddl_popuptitle.SelectedItem.Value != "Select")
            {
                if (ddl_popuptitle.SelectedItem.Value != "Others")
                {
                    title = Convert.ToString(ddl_popuptitle.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_poprd_title.Text);
                    title = subjectcodenew("EventTitle", doc_prty1);
                }
            }
            if (ddl_seminar.SelectedItem.Value != "Select")
            {
                if (ddl_seminar.SelectedItem.Value != "Others")
                {
                    seminartype = Convert.ToString(ddl_seminar.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_seminar.Text);
                    seminartype = subjectcodenew("EventSemType", doc_prty1);
                }
            }

            if (ddl_awdcat.SelectedItem.Value != "Select")
            {
                if (ddl_awdcat.SelectedItem.Value != "Others")
                {
                    awdcat = Convert.ToString(ddl_awdcat.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_awdcat.Text);
                    awdcat = subjectcodenew("EventAward", doc_prty1);
                }
            }
            if (ddl_Tournament.SelectedItem.Value != "Select")
            {
                if (ddl_Tournament.SelectedItem.Value != "Others")
                {
                    toutype = Convert.ToString(ddl_Tournament.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_Tournament.Text);
                    toutype = subjectcodenew("EventTourType", doc_prty1);
                }
            }

            if (ddl_game.SelectedItem.Value != "Select")
            {
                if (ddl_game.SelectedItem.Value != "Others")
                {
                    gametype = Convert.ToString(ddl_game.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_game.Text);
                    gametype = subjectcodenew("EventGame", doc_prty1);
                }
            }

            string rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");
            if (rdb_popnational.Checked == true)
            {
                isnational = "0";
            }
            else
            {
                isnational = "1";
            }
            if (rdb_Papers.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,Journal,Im_Factor)values('" + rq_fk + "','" + title + "','" + isnational + "','" + journal + "','" + impact + "')";
            }
            else if (rdb_Paper.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,ConferenceProceed)values('" + rq_fk + "','" + title + "','" + isnational + "','" + confnc + "')";
            }
            else if (rdb_Patents.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,PatentNo,ApplicationNo,AppDate,AppStatus)values('" + rq_fk + "','" + title + "','" + isnational + "','" + patenno + "','" + appno + "','" + appdate + "','" + appstatus + "')";
            }
            else if (rdb_Conference.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,Conference)values('" + rq_fk + "','" + title + "','" + isnational + "','" + cnfrnc + "')";
            }
            else if (rdb_workshop.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,Workshoptype)values('" + rq_fk + "','" + title + "','" + isnational + "','" + wkshop + "')";
            }
            else if (rdb_seminor.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,SeminarType)values('" + rq_fk + "','" + title + "','" + isnational + "','" + seminartype + "')";
            }
            else if (rdb_Award.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,AwardCategory,PrizeWon)values('" + rq_fk + "','" + title + "','" + isnational + "','" + awdcat + "','" + prz + "')";
            }
            else if (rdb_student.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,Duration,Work)values('" + rq_fk + "','" + title + "','" + isnational + "','" + dur + "','" + work + "')";
            }
            else if (rdb_ReSearch.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,Scholar,ProgName,MainSupervisor,CoSupervisor)values('" + rq_fk + "','" + title + "','" + isnational + "','" + schlr + "','" + prg + "','" + Mainsup + "','" + cosup + "')";
            }
            else if (rdb_Membership.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,Society,MembershipDet)values('" + rq_fk + "','" + title + "','" + isnational + "','" + Society + "','" + mem_det + "')";
            }
            else if (rdb_Distinguished.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,Visitor,Organization,Purpose)values('" + rq_fk + "','" + title + "','" + isnational + "','" + visit + "','" + org + "','" + pur + "')";
            }
            else if (rdb_Tournamentk.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,TourType,Game,Tournament)values('" + rq_fk + "','" + title + "','" + isnational + "','" + toutype + "','" + gametype + "','" + tour + "')";
            }
            else if (rdb_gust.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,GL_title)values('" + rq_fk + "','" + gst + "')";
            }
            else if (RDB_OTHERS.Checked == true)
            {
                query = "insert into RQ_ReqEventDet(RequisitionFK,title,isnational,ActionName)values('" + rq_fk + "','" + title + "','" + isnational + "','" + actionname + "')";
            }
            d2.update_method_wo_parameter(query, "Text");
        }
        catch
        {
        }
    }

    public void dept()
    {
        string query = "select Dept_Name as DeptName,Dept_Code from Department ";
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string dd = Convert.ToString(ds.Tables[0].Rows[i]["DeptName"]);
                string ddd = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Code"]);
                if (!depthash.Contains(Convert.ToString(dd)))
                {

                    depthash.Add(Convert.ToString(dd), Convert.ToString(ddd));
                }

            }
        }


    }
    protected void bindpop2degree()
    {
        try
        {
            ds.Clear();
            string query = "";
            if (usercode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + "";
            }

            ds = d2.select_method_wo_parameter(query, "Text");
            cbl_degree.DataSource = ds;
            cbl_degree.DataTextField = "course_name";
            cbl_degree.DataValueField = "course_id";
            cbl_degree.DataBind();
            //if (cbl_degree.Items.Count > 0)
            //{
            //    for (int i = 0; i < cbl_degree.Items.Count; i++)
            //    {
            //        cbl_degree.Items[i].Selected = true;
            //    }
            //    txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
            //}
            //cb_degree.Checked = true;
        }
        catch
        {
        }
    }

    public void btnaddrows_Click(object sender, EventArgs e)
    {


        if (GridView3.Rows.Count > 0)
        {
            TextBox box1 = new TextBox();
            TextBox box2 = new TextBox();
            TextBox box3 = new TextBox();

            for (int i = 0; i < GridView3.Rows.Count; i++)
            {
                box1 = (TextBox)GridView3.Rows[i].Cells[1].FindControl("txtdname");
                box2 = (TextBox)GridView3.Rows[i].Cells[2].FindControl("txtresource");
                box3 = (TextBox)GridView3.Rows[i].Cells[3].FindControl("txtamt");
            }

            AddNewRowToGrid();



        }
    }
    public void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTablenew"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTablenew"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                TextBox box1 = new TextBox();
                TextBox box2 = new TextBox();

                TextBox box3 = new TextBox();


                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {

                    box1 = (TextBox)GridView3.Rows[i].Cells[1].FindControl("txtdname");
                    box2 = (TextBox)GridView3.Rows[i].Cells[2].FindControl("txtresource");

                    box3 = (TextBox)GridView3.Rows[i].Cells[3].FindControl("txtamt");

                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i][0] = box1.Text;
                    dtCurrentTable.Rows[i][1] = box2.Text;
                    dtCurrentTable.Rows[i][2] = box3.Text;
                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTablenew"] = dtCurrentTable;

                GridView3.DataSource = dtCurrentTable;
                GridView3.DataBind();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"View State Null\");", true);

        }



    }

    public void btnaddrows1_Click(object sender, EventArgs e)
    {
        if (GV3.Rows.Count > 0)
        {
            TextBox box1 = new TextBox();
            TextBox box2 = new TextBox();
            TextBox box3 = new TextBox();

            for (int i = 0; i < GV3.Rows.Count; i++)
            {
                box1 = (TextBox)GV3.Rows[i].Cells[1].FindControl("txtdname");
                box2 = (TextBox)GV3.Rows[i].Cells[2].FindControl("txtresource");
                box3 = (TextBox)GV3.Rows[i].Cells[3].FindControl("txtamt");
            }

            AddNewRowToGrid1();

        }
    }
    public void AddNewRowToGrid1()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable2"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable2"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                TextBox box1 = new TextBox();
                TextBox box2 = new TextBox();

                TextBox box3 = new TextBox();


                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {

                    box1 = (TextBox)GV3.Rows[i].Cells[1].FindControl("txtdname");
                    box2 = (TextBox)GV3.Rows[i].Cells[2].FindControl("txtresource");

                    box3 = (TextBox)GV3.Rows[i].Cells[3].FindControl("txtamt");

                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i][0] = box1.Text;
                    dtCurrentTable.Rows[i][1] = box2.Text;
                    dtCurrentTable.Rows[i][2] = box3.Text;
                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable2"] = dtCurrentTable;

                GV3.DataSource = dtCurrentTable;
                GV3.DataBind();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"View State Null\");", true);

        }



    }

    public void btnaddrows2_Click(object sender, EventArgs e)
    {
        if (GV4.Rows.Count > 0)
        {
            TextBox box1 = new TextBox();
            TextBox box2 = new TextBox();
            TextBox box3 = new TextBox();

            for (int i = 0; i < GV4.Rows.Count; i++)
            {
                box1 = (TextBox)GV4.Rows[i].Cells[1].FindControl("txtcname");
                box2 = (TextBox)GV4.Rows[i].Cells[2].FindControl("txtcnt");
                box3 = (TextBox)GV4.Rows[i].Cells[3].FindControl("txtamt1");
            }

            AddNewRowToGrid2();

        }
    }
    public void AddNewRowToGrid2()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable3"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable3"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                TextBox box1 = new TextBox();
                TextBox box2 = new TextBox();

                TextBox box3 = new TextBox();


                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {

                    box1 = (TextBox)GV4.Rows[i].Cells[1].FindControl("txtcname");
                    box2 = (TextBox)GV4.Rows[i].Cells[2].FindControl("txtcnt");

                    box3 = (TextBox)GV4.Rows[i].Cells[3].FindControl("txtamt1");

                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i][0] = box1.Text;
                    dtCurrentTable.Rows[i][1] = box2.Text;
                    dtCurrentTable.Rows[i][2] = box3.Text;
                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable3"] = dtCurrentTable;

                GV4.DataSource = dtCurrentTable;
                GV4.DataBind();
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"View State Null\");", true);

        }



    }

    public string subjectcodenew(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {

        }
        return subjec_no;
    }
    public void newbindbatch()
    {
        ds = d2.BindBatch();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_org_batch.DataSource = ds;
            ddl_org_batch.DataTextField = "batch_year";
            ddl_org_batch.DataValueField = "batch_year";
            ddl_org_batch.DataBind();
            // ddl_org_batch.SelectedIndex = 3;
        }
    }

    public void cb_degree2_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";
            if (cb_degree2.Checked == true)
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    if (cb_degree2.Checked == true)
                    {
                        cbl_degree2.Items[i].Selected = true;
                        txt_degree2.Text = "Degree(" + (cbl_degree2.Items.Count) + ")";
                        build1 = cbl_degree2.Items[i].Value.ToString();
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
                bindbranch1(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    cbl_degree2.Items[i].Selected = false;
                    txt_degree2.Text = "--Select--";
                    txt_branch2.Text = "--Select--";
                    cbl_branch1.ClearSelection();
                    cb_branch1.Checked = false;
                    cb_degree2.Checked = false;
                }
            }
            bindbranch1(college);
            // Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree2.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    //  txt_branch.Text = "--Select--";
                    build = cbl_degree2.Items[i].Value.ToString();
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
            bindbranch1(buildvalue);
            if (seatcount == cbl_degree2.Items.Count)
            {
                txt_degree2.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree2.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree2.Text = "--Select--";
                txt_degree2.Text = "--Select--";
            }
            else
            {
                txt_degree2.Text = "Degree(" + seatcount.ToString() + ")";
            }
            // bindbranch(college);
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch1(string branch)
    {
        try
        {
            cbl_branch1.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                }
            }
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            if (itemheader.Trim() != "")
            {
                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch1.DataSource = ds;
                    cbl_branch1.DataTextField = "dept_name";
                    cbl_branch1.DataValueField = "degree_code";
                    cbl_branch1.DataBind();



                    if (cbl_branch1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch1.Items.Count; i++)
                        {
                            cbl_branch1.Items[i].Selected = true;
                        }
                        txt_branch2.Text = "Branch(" + cbl_branch1.Items.Count + ")";
                    }
                }
                else
                {
                    txt_branch2.Text = "--Select--";
                }
            }
            else
            {
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_branch1_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch1.Checked == true)
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = true;
                }
                txt_branch2.Text = "Branch(" + (cbl_branch1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = false;
                }
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_branch2.Text = "--Select--";
            cb_branch1.Checked = false;
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_branch2.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cbl_branch1.Items.Count)
                {
                    cb_branch1.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }


    protected void buttonok_Click(object sender, EventArgs e)
    {


    }
    protected void btn_exit2_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
    }

    protected void btn_go1stud_Click(object sender, EventArgs e)
    {
        parti_studbind();
    }

    public void parti_studbind()
    {
        try
        {
            string selectquery = "";
            int sno = 0;

            string itemheader = "";
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_branch1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_branch1.Items[i].Value.ToString() + "";
                    }
                }
            }

            string batch_year = Convert.ToString(ddl_batch1.SelectedItem.Text);

            if (itemheader.Trim() != "" && batch_year.Trim() != "")
            {
                if (txt_rollno1.Text == "")
                {

                    selectquery = "select Roll_No,Stud_Name,App_No,dt.Dept_Name  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "') order by dt.Dept_Code   ";
                    //and r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' ) 
                }
                else
                {
                    //selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.Dept_Name) as Department  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "')  order by Roll_No,d.Degree_Code ";
                    selectquery = "select Roll_No,Stud_Name,App_No,dt.Dept_Name  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and  r.Roll_No ='" + txt_rollno1.Text + "'";
                    // r.roll_no not in (select Roll_No  from DayScholourStaffAdd where ISNULL(Roll_No,'') <>'' ) and
                }


                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                //DataTable dt = new DataTable();
                //DataRow dr = null;
                //dt.Columns.Add("department");
                //dt.Columns.Add("Header");

                //dt.Columns.Add("ItemCode");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GridView1.DataSource = ds;
                    GridView1.DataBind();
                    MergeCells();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void pop_Gv1_div_Click(object sender, EventArgs e)
    {
        pop_Gv1_div.Visible = false;
    }

    public void pop_minuteclose_Click(object sender, EventArgs e)
    {
        pop_minute.Visible = false;
    }
    public void VendorCodeGen()
    {
        try
        {
            string newitemcode = "";
            string VendorCode = "";
            string selectquery = "select VenAcr,VenStNo,VenSize from IM_CodeSettings  order by StartDate desc";
            //select Requisition_Acr ,Requisition_Size,Requisition_StNo  from InvCode_Settings where Latestrec =1";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["VenAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["VenStNo"]);
                if (itemacronym.Trim() != "" && itemstarno.Trim() != "")
                {
                    selectquery = "select distinct top (1)  VendorCode  from CO_VendorMaster where VendorCode like '" + Convert.ToString(itemacronym) + "%' order by VendorCode desc";

                    //select distinct top (1)  RequestCode  from RQ_Requisition where RequestCode like '" + Convert.ToString(itemacronym) + "%' order by RequestCode desc";
                    //select distinct top (1) item_code  from item_master where item_code like '" + Convert.ToString(itemacronym) + "%' order by item_code desc";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["VendorCode"]);
                        string itemacr = Convert.ToString(itemacronym);
                        int len = itemacr.Length;
                        itemcode = itemcode.Remove(0, len);
                        int len1 = Convert.ToString(itemcode).Length;
                        string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                        len = Convert.ToString(newnumber).Length;
                        len1 = len1 - len;
                        if (len1 == 2)
                        {
                            newitemcode = "00" + newnumber;
                        }
                        else if (len1 == 1)
                        {
                            newitemcode = "0" + newnumber;
                        }
                        else if (len1 == 4)
                        {
                            newitemcode = "0000" + newnumber;
                        }
                        else if (len1 == 3)
                        {
                            newitemcode = "000" + newnumber;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(newnumber);
                        }
                        if (newitemcode.Trim() != "")
                        {
                            newitemcode = itemacr + "" + newitemcode;
                        }
                    }
                    else
                    {
                        newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(itemstarno);
                    }

                    Session["VendorCode"] = newitemcode;


                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void pop_add_staff_stud_othrclose_Click(object sender, EventArgs e)
    {
        //Div1.EnableViewState = false;
        //fsstaff.Visible = true;
        string val = "";
        pop_add_staff_stud_othr.Visible = false;
        for (int i = 0; i < GridView2.Rows.Count; i++)
        {
            CheckBox checkvalue1 = (CheckBox)GridView2.Rows[i].FindControl("chkup3"); ;
            if (checkvalue1.Checked == true)
            {
                Label stud_appno = (Label)GridView2.Rows[i].FindControl("lblappno");
                val = Convert.ToString(stud_appno.Text);
                if (particpentstaff == "")
                {
                    particpentstaff = val;
                }
                else
                {
                    particpentstaff = particpentstaff + "'" + "," + "'" + val;
                }
            }
        }
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {

            CheckBox checkvalue1 = (CheckBox)GridView1.Rows[i].FindControl("chkup3");

            if (checkvalue1.Checked == true)
            {
                Label stud_appno = (Label)GridView1.Rows[i].FindControl("lblappno");
                val = Convert.ToString(stud_appno.Text);
                if (particpentstud == "")
                {
                    particpentstud = val;
                }
                else
                {
                    particpentstud = particpentstud + "'" + "," + "'" + val;
                }
            }

        }
        indi_comp();

    }
    public void loadstaffdep1(string collegecode)
    {
        try
        {

            string srisql = "select distinct dept_name,dept_code from hrdept_master where college_code=" + Session["collegecode"] + "";

            ds.Clear();
            ds = da.select_method_wo_parameter(srisql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staff_dept11.DataSource = ds;
                cbl_staff_dept11.DataTextField = "dept_name";
                cbl_staff_dept11.DataValueField = "dept_code";
                cbl_staff_dept11.DataBind();
                if (cbl_staff_dept11.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staff_dept11.Items.Count; i++)
                    {
                        cbl_staff_dept11.Items[i].Selected = true;
                    }
                    txt_staff_dept11.Text = "Dept(" + cbl_staff_dept11.Items.Count + ")";
                }

            }
        }
        catch
        {
        }
    }
    public void loadstaffdept1(string collegecode)
    {
        try
        {

            string srisql = "select distinct dept_name,dept_code from hrdept_master where college_code=" + Session["collegecode"] + "";

            ds.Clear();
            ds = da.select_method_wo_parameter(srisql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staffdeprt.DataSource = ds;
                cbl_staffdeprt.DataTextField = "dept_name";
                cbl_staffdeprt.DataValueField = "dept_code";
                cbl_staffdeprt.DataBind();
                if (cbl_staff_dept11.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staffdeprt.Items.Count; i++)
                    {
                        cbl_staffdeprt.Items[i].Selected = true;
                    }
                    txt_staffdeprt.Text = "Dept(" + cbl_staffdeprt.Items.Count + ")";
                }
            }
        }
        catch
        {
        }
    }
    void bind_stafType1()
    {
        try
        {
            string srisql = "SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 and d.college_code=" + Session["collegecode"] + "";
            ds.Clear();
            ds = da.select_method_wo_parameter(srisql, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staff_type111.DataSource = ds;
                cbl_staff_type111.DataTextField = "StfType";
                cbl_staff_type111.DataValueField = "StfType";
                cbl_staff_type111.DataBind();
                if (cbl_staff_type111.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staff_type111.Items.Count; i++)
                    {
                        cbl_staff_type111.Items[i].Selected = true;
                    }
                    txt_staff_type11.Text = "Staff Type(" + cbl_staff_type111.Items.Count + ")";
                }

            }
        }
        catch
        {
        }


    }
    void bind_stafTypenew()
    {
        try
        {
            string srisql = "SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 and d.college_code=" + Session["collegecode"] + "";
            ds.Clear();
            ds = da.select_method_wo_parameter(srisql, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stafftype.DataSource = ds;
                cbl_stafftype.DataTextField = "StfType";
                cbl_stafftype.DataValueField = "StfType";
                cbl_stafftype.DataBind();
                cb_stafftype.Checked = true;
                if (cbl_stafftype.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                    {
                        cbl_stafftype.Items[i].Selected = true;
                    }
                    txt_staff_type.Text = "Staff Type(" + cbl_stafftype.Items.Count + ")";
                }

            }
            bind_design2();
        }
        catch
        {
        }
    }
    protected void loadfsstaff1()
    {
        try
        {

            string sql = "";
            string bindspread = sql;
            string design_name = string.Empty;
            string dept_all = string.Empty;
            string design_all = string.Empty;
            string itemheader = "";
            string designation = "";
            string dept = "";
            for (int i = 0; i < cbl_staff_dept11.Items.Count; i++)
            {
                if (cbl_staff_dept11.Items[i].Selected == true)
                {
                    if (dept == "")
                    {
                        dept = "" + cbl_staff_dept11.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        dept = dept + "'" + "," + "" + "'" + cbl_staff_dept11.Items[i].Value.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_staff_type111.Items.Count; i++)
            {
                if (cbl_staff_type111.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_staff_type111.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_staff_type111.Items[i].Value.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_staff_desn11.Items.Count; i++)
            {
                if (cbl_staff_desn11.Items[i].Selected == true)
                {
                    if (designation == "")
                    {
                        designation = "" + cbl_staff_desn11.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        designation = designation + "'" + "," + "" + "'" + cbl_staff_desn11.Items[i].Value.ToString() + "";
                    }
                }
            }
            string Sql_Query = "";
            if (dept != "")
            {
                if (itemheader != "")
                {
                    if (designation != "")
                    {
                        if (persentedstaff == "")
                        {
                            persentedstaff = "0";
                        }
                        Sql_Query = "select distinct s.staff_code,s.staff_name,appl_id,h.dept_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st,staff_appl_master sm where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code =  h.college_code and s.college_code = d.collegecode  and s.appl_no=sm.appl_no and h.dept_code in ( '" + dept + "')  and d.desig_name in ('" + designation + "') and s.college_code='" + ddl_poupcollege.SelectedValue.ToString() + "'   and stftype in('" + itemheader + "') and resign = 0 and settled = 0 and latestrec=1 and appl_id not in('" + persentedstaff + "')";
                    }
                    else
                    {
                        GridView2.Visible = false;
                        Label33.Visible = true;
                        Label33.Text = "Select Any Designation";
                    }
                }
                else
                {
                    GridView2.Visible = false;
                    Label33.Visible = true;
                    Label33.Text = "Select Any Staff Type";
                }
            }
            else
            {
                GridView2.Visible = false;
                Label33.Visible = true;
                Label33.Text = "Select Any Department";
            }
            DataSet dsbindspread = new DataSet();
            dsbindspread.Clear();
            dsbindspread = da.select_method_wo_parameter(Sql_Query, "Text");
            FarPoint.Web.Spread.CheckBoxCellType ccb = new FarPoint.Web.Spread.CheckBoxCellType();
            ccb.AutoPostBack = true;
            if (dsbindspread.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
                {

                    GridView2.DataSource = dsbindspread;
                    GridView2.DataBind();
                }

            }
            else
            {
                GridView2.Visible = false;
                Label33.Visible = true;
                Label33.Text = "No Records Found";

            }
        }
        catch
        {
        }
    }
    public void outinstitution()
    {
        ds.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='Institution'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_outinstitution.DataSource = ds;
            ddl_outinstitution.DataTextField = "MasterValue";
            ddl_outinstitution.DataValueField = "MasterCode";
            ddl_outinstitution.DataBind();
            ddl_outinstitution.Items.Insert(0, new ListItem("Select", "0"));


        }
        else
        {
            ddl_outinstitution.Items.Insert(0, new ListItem("Select", "0"));

            //ddl_outinstitution.Items.Insert(ddl_act_namenew.Items.Count, "Others");
        }


    }

    public void outorganizer()
    {
        ds.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='organizer'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_outorganiser.DataSource = ds;
            ddl_outorganiser.DataTextField = "MasterValue";
            ddl_outorganiser.DataValueField = "MasterCode";
            ddl_outorganiser.DataBind();
            ddl_outorganiser.Items.Insert(0, new ListItem("Select", "0"));
            // ddl_act_namenew.Items.Insert(ddl_act_namenew.Items.Count, "Others");

        }
        else
        {
            ddl_outorganiser.Items.Insert(0, new ListItem("Select", "0"));


        }
    }

    public void res()
    {
        try
        {
            ddlname.Items.Insert(ddlname.Items.Count, "Others");

        }
        catch
        {
        }

    }
    public void res_semi()
    {
        ddl_seminar.Items.Insert(ddl_seminar.Items.Count, "Others");
    }
    public void res_awd()
    {
        ddl_awdcat.Items.Insert(ddl_awdcat.Items.Count, "Others");
    }
    public void res_game()
    {
        ddl_game.Items.Insert(ddl_game.Items.Count, "Others");
    }
    public void res_title()
    {
        ddl_popuptitle.Items.Insert(ddl_popuptitle.Items.Count, "Others");
    }
    public void res_expnc()
    {
        ddl_expnc_name.Items.Insert(ddl_expnc_name.Items.Count, "Others");
    }
    public void res_tour()
    {
        ddl_Tournament.Items.Insert(ddl_Tournament.Items.Count, "Others");
    }
    public void res_new()
    {
        //ddl_act_namenew.Items.Insert(ddl_act_namenew.Items.Count, "Others");
    }
    public void res_insititution()
    {
        ddl_outinstitution.Items.Insert(ddl_outinstitution.Items.Count, "Others");
    }
    public void res_organizer()
    {
        ddl_outorganiser.Items.Insert(ddl_outorganiser.Items.Count, "Others");
    }
    public void loadhour()
    {
        try
        {

            ddl_hour1.Items.Clear();
            ddl_endhour1.Items.Clear();

            for (int i = 1; i <= 12; i++)
            {

                ddl_hour1.Items.Add(Convert.ToString(i));
                ddl_endhour1.Items.Add(Convert.ToString(i));
                ddl_hour1.SelectedIndex = ddl_hour1.Items.Count - 1;
                ddl_endhour1.SelectedIndex = ddl_endhour1.Items.Count - 1;

            }

        }
        catch
        {
        }
    }

    public void loadminits()
    {

        ddl_minits1.Items.Clear();
        ddl_endminit1.Items.Clear();

        for (int i = 0; i <= 59; i++)
        {
            string value = Convert.ToString(i);
            if (value.Length == 1)
            {
                value = "0" + "" + value;
            }



            ddl_minits1.Items.Add(Convert.ToString(value));
            ddl_endminit1.Items.Add(Convert.ToString(value));


        }
    }
    public void SetPreviousData3()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable2"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable2"];
            if (dt.Rows.Count > 0)
            {
                TextBox box1 = new TextBox();
                TextBox box2 = new TextBox();
                TextBox box3 = new TextBox();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    box1 = (TextBox)GV3.Rows[i].Cells[1].FindControl("txtdname");
                    box2 = (TextBox)GV3.Rows[i].Cells[2].FindControl("txtresource");
                    box3 = (TextBox)GV3.Rows[i].Cells[3].FindControl("txtamt");




                    rowIndex++;
                }
            }
        }
    }
    public void SetPreviousData4()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable3"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable3"];
            if (dt.Rows.Count > 0)
            {
                TextBox box1 = new TextBox();
                TextBox box2 = new TextBox();
                TextBox box3 = new TextBox();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    box1 = (TextBox)GV4.Rows[i].Cells[1].FindControl("txtcname");
                    box2 = (TextBox)GV4.Rows[i].Cells[2].FindControl("txtcnt");
                    box3 = (TextBox)GV4.Rows[i].Cells[3].FindControl("txtamt1");

                    rowIndex++;
                }
            }
        }
    }


    public void ImageButton6_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
    }

    public void bindbatch_Present()
    {
        try
        {
            ddl_batch1.Items.Clear();
            hat.Clear();

            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch1.DataSource = ds;
                ddl_batch1.DataTextField = "batch_year";
                ddl_batch1.DataValueField = "batch_year";
                ddl_batch1.DataBind();
                ddl_batch1.SelectedIndex = 3;
                ddl_prsnt_batch.DataSource = ds;
                ddl_prsnt_batch.DataTextField = "batch_year";
                ddl_prsnt_batch.DataValueField = "batch_year";
                ddl_prsnt_batch.DataBind();
                ddl_prsnt_batch.SelectedIndex = 3;

            }
        }
        catch
        {
        }
    }

    protected void binddegree_present()
    {
        try
        {
            ds.Clear();
            string query = "";
            if (usercode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + "";
            }

            ds = d2.select_method_wo_parameter(query, "Text");
            cbl_degree2.DataSource = ds;
            cbl_degree2.DataTextField = "course_name";
            cbl_degree2.DataValueField = "course_id";
            cbl_degree2.DataBind();

            cbl_prsnt_degree.DataSource = ds;
            cbl_prsnt_degree.DataTextField = "course_name";
            cbl_prsnt_degree.DataValueField = "course_id";
            cbl_prsnt_degree.DataBind();

            if (cbl_degree2.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    cbl_degree2.Items[i].Selected = true;
                }
                txt_degree2.Text = "Degree(" + cbl_degree2.Items.Count + ")";
            }
            else
            {
                txt_degree2.Text = "--Select--";
            }

            if (cbl_prsnt_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_prsnt_degree.Items.Count; i++)
                {
                    cbl_prsnt_degree.Items[i].Selected = true;
                }
                txt_prsnt_degree.Text = "Degree(" + cbl_prsnt_degree.Items.Count + ")";
            }
            else
            {
                txt_prsnt_degree.Text = "--Select--";
            }


        }
        catch
        {
        }
    }
    public void loaddesc()
    {
        ddl_viewdetails1.Items.Clear();
        dsnew.Tables.Clear();

        string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='EventCategory' and CollegeCode ='" + collegecode1 + "'";
        dsnew = d2.select_method_wo_parameter(sql, "TEXT");
        if (dsnew.Tables[0].Rows.Count > 0)
        {
            ddl_viewdetails1.DataSource = dsnew;
            ddl_viewdetails1.DataTextField = "MasterValue";
            ddl_viewdetails1.DataValueField = "MasterCode";
            ddl_viewdetails1.DataBind();
            ddl_viewdetails1.Items.Insert(0, new ListItem("Select", "0"));


        }
        else
        {
            ddl_viewdetails1.Items.Insert(0, new ListItem("Select", "0"));
        }

    }
    public void rdb_addstaff_CheckedChanged(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
        pop_others.Visible = false;
        popup_selectstaff.Visible = true;
        popcm.Visible = false;
    }
    public void rdb_addstudent_CheckedChanged(object sender, EventArgs e)
    {
        popupselectstd.Visible = true;
        pop_others.Visible = false;
        popup_selectstaff.Visible = false;
        popcm.Visible = false;
    }
    public void rdb_addothers_CheckedChanged(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
        pop_others.Visible = true;
        popup_selectstaff.Visible = false;
        btn_go_staff.Visible = true;
        popcm.Visible = false;
    }

    public void rdo_addcomp_CheckedChanged(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
        pop_others.Visible = false;
        popup_selectstaff.Visible = false;
        btn_go_staff.Visible = true;
        popcm.Visible = true;
    }
    public void ddl_popupdent_SelectedIndexChanged(object sender, EventArgs e)
    {

        loadfsstaff1();
    }
    public void ddl_popstafftype_SelectedIndexChanged(object sender, EventArgs e)
    {


        loadfsstaff1();
    }
    public void ddl_popdesign_SelectedIndexChanged(object sender, EventArgs e)
    {


        loadfsstaff1();
    }
    public void ddl_popstaffby_SelectedIndexChanged(object sender, EventArgs e)
    {


        loadfsstaff1();
    }
    public void FpSpread1_CellClick(object sender, EventArgs e)
    {

        BindCollege();
        loadstaffdep1(collegecode);
        bind_stafType1();
        bind_design1();
        loadfsstaff1();
    }


    public void ddl_prsnt_staff_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff2();
    }
    public void ddl_prsnt_stafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff2();
    }
    public void ddl_prsnt_staff_design_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfsstaff2();
    }
    public void btndesc2popadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_description22.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description22.Text + "' and MasterCriteria ='EventCategory' and CollegeCode ='" + collegecode1 + "') update CO_MasterValues set MasterValue ='" + txt_description22.Text + "' where MasterValue ='" + txt_description22.Text + "' and MasterCriteria ='EventCategory' and CollegeCode ='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description22.Text + "','EventCategory','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    pop_add_staff_stud_othr1.Visible = true;
                    //imgdiv2.Visible = true;
                    //pnl2.Visible = true;
                    //lbl_alert.Text = "Saved sucessfully";
                    txt_description22.Text = "";
                    imgdiv44.Visible = false;
                    txt_description22.Visible = false;

                    loaddesc();
                }

            }
            else
            {
                //imgdiv2.Visible = true;
                //pnl2.Visible = true;
                //lbl_alert.Text = "Enter the description";
            }
        }
        catch
        {
        }
    }
    public void btndesc2popexit_Click(object sender, EventArgs e)
    {
        imgdiv44.Visible = false;
        panel_description22.Visible = false;
    }
    public void btn_staffgo_Click(object sender, EventArgs e)
    {
        loadfsstaff2();
        btn_go_prsntclik.Visible = true;

    }
    public void cb_staff_dept11_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_staff_dept11.Text = "--Select--";
        if (cb_staff_dept11.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_staff_dept11.Items.Count; i++)
            {
                cbl_staff_dept11.Items[i].Selected = true;
            }
            txt_staff_dept11.Text = "Dept(" + (cbl_staff_dept11.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_staff_dept11.Items.Count; i++)
            {
                cbl_staff_dept11.Items[i].Selected = false;
            }
        }
    }
    public void cbl_staff_dept11_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;

        int commcount = 0;
        txt_staff_dept11.Text = "--Select--";
        for (i = 0; i < cbl_staff_dept11.Items.Count; i++)
        {
            if (cbl_staff_dept11.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_staff_dept11.Checked = false;
            }
        }

        if (commcount > 0)
        {
            if (commcount == cbl_staff_dept11.Items.Count)
            {
                cb_staff_dept11.Checked = true;
            }
            txt_staff_dept11.Text = "Dept(" + commcount.ToString() + ")";
        }
    }
    public void cb_staff_type111_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        txt_staff_type11.Text = "--Select--";
        if (cb_staff_type111.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_staff_type111.Items.Count; i++)
            {
                cbl_staff_type111.Items[i].Selected = true;
            }
            txt_staff_type11.Text = "Staff Type(" + (cbl_staff_type111.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_staff_type111.Items.Count; i++)
            {
                cbl_staff_type111.Items[i].Selected = false;
            }
        }
        bind_design1();
    }
    public void cb_staff_type111_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;

        int commcount = 0;
        txt_staff_type11.Text = "--Select--";
        for (i = 0; i < cbl_staff_type111.Items.Count; i++)
        {
            if (cbl_staff_type111.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_staff_type111.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_staff_type111.Items.Count)
            {
                cb_staff_type111.Checked = true;
            }
            txt_staff_type11.Text = "Staff Type(" + commcount.ToString() + ")";
        }
        bind_design1();
    }
    public void cb_staff_desn11_CheckedChanged(object sender, EventArgs e)
    {

        int cout = 0;
        txt_staff_desg111.Text = "--Select--";
        if (cb_staff_desn11.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_staff_desn11.Items.Count; i++)
            {
                cbl_staff_desn11.Items[i].Selected = true;
            }
            txt_staff_desg111.Text = "Designation(" + (cbl_staff_desn11.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_staff_desn11.Items.Count; i++)
            {
                cbl_staff_desn11.Items[i].Selected = false;
            }
        }
    }
    public void cbl_staff_desn11_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;

        int commcount = 0;
        txt_staff_desg111.Text = "--Select--";
        for (i = 0; i < cbl_staff_desn11.Items.Count; i++)
        {
            if (cbl_staff_desn11.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_staff_desn11.Checked = false;
            }
        }

        if (commcount > 0)
        {
            if (commcount == cbl_staff_desn11.Items.Count)
            {
                cb_staff_desn11.Checked = true;
            }
            txt_staff_desg111.Text = "Designation(" + commcount.ToString() + ")";
        }
    }
    public void btn_staff_go11_Click(object sender, EventArgs e)
    {
        loadfsstaff1();
    }
    public void txt_pop_search_TextChanged(object sender, EventArgs e)
    {
    }
    public void btn_addddd_Click(object sender, EventArgs e)
    {
        divactionadddetails.Visible = true;
        actadd.Visible = true;
    }
    public void FpSpread4_CellClick(object sender, EventArgs e)
    {
    }
    public void ItemReqNo()
    {
        try
        {
            string newitemcode = "";
            string selectquery = "select ReqAcr,ReqSize,ReqStNo  from IM_CodeSettings order by StartDate desc";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["ReqAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["ReqStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["ReqSize"]);
                selectquery = "select distinct top (1)  RequestCode  from RQ_Requisition where RequestCode like '" + Convert.ToString(itemacronym) + "%' order by RequestCode desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["RequestCode"]);
                    string itemacr = Convert.ToString(itemacronym);
                    int len = itemacr.Length;
                    itemcode = itemcode.Remove(0, len);
                    int len1 = Convert.ToString(itemcode).Length;
                    string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                    len = Convert.ToString(newnumber).Length;
                    len1 = Convert.ToInt32(itemsize) - len;
                    if (len1 == 2)
                    {
                        newitemcode = "00" + newnumber;
                    }
                    else if (len1 == 1)
                    {
                        newitemcode = "0" + newnumber;
                    }
                    else if (len1 == 4)
                    {
                        newitemcode = "0000" + newnumber;
                    }
                    else if (len1 == 3)
                    {
                        newitemcode = "000" + newnumber;
                    }
                    else if (len1 == 5)
                    {
                        newitemcode = "00000" + newnumber;
                    }
                    else if (len1 == 6)
                    {
                        newitemcode = "000000" + newnumber;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(newnumber);
                    }
                    if (newitemcode.Trim() != "")
                    {
                        newitemcode = itemacr + "" + newitemcode;
                    }
                }
                else
                {
                    string itemacr = Convert.ToString(itemstarno);
                    int len = itemacr.Length;

                    string items = Convert.ToString(itemsize);
                    int len1 = Convert.ToInt32(items);
                    int size = len1 - len;
                    if (size == 2)
                    {
                        newitemcode = "00" + itemstarno;
                    }
                    else if (size == 1)
                    {
                        newitemcode = "0" + itemstarno;
                    }
                    else if (size == 4)
                    {
                        newitemcode = "0000" + itemstarno;
                    }
                    else if (size == 3)
                    {
                        newitemcode = "000" + itemstarno;
                    }
                    else if (len1 == 5)
                    {
                        newitemcode = "00000" + itemstarno;
                    }
                    else if (len1 == 6)
                    {
                        newitemcode = "000000" + itemstarno;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(itemstarno);
                    }
                    newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                }
                Session["requestcode"] = Convert.ToString(newitemcode);
                rqustn_no_event.Text = Convert.ToString(newitemcode);
                #region new code
                #endregion
            }
        }
        catch
        { }
    }
    public void itemheader()
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
                columnfield = " and usercode='" + Session["usercode"] + "'";
            }
            string maninvalue = "";
            string selectnewquery = d2.GetFunction("select value from Master_Settings where settings='ItemHeaderRights' " + columnfield + "");
            if (selectnewquery.Trim() != "" && selectnewquery.Trim() != "0")
            {
                string[] splitnew = selectnewquery.Split(',');
                if (splitnew.Length > 0)
                {
                    for (int row = 0; row <= splitnew.GetUpperBound(0); row++)
                    {
                        if (maninvalue == "")
                        {
                            maninvalue = Convert.ToString(splitnew[row]);
                        }
                        else
                        {
                            maninvalue = maninvalue + "'" + "," + "'" + Convert.ToString(splitnew[row]);
                        }
                    }
                }
            }


            ds.Clear();

            string query = "";
            query = "select distinct ItemHeaderCode,ItemHeaderName  from IM_ItemMaster ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {


                cbl_itm_hdrname.DataSource = ds;
                cbl_itm_hdrname.DataTextField = "ItemHeaderName";
                cbl_itm_hdrname.DataValueField = "ItemHeaderCode";
                cbl_itm_hdrname.DataBind();


                if (cbl_itm_hdrname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_itm_hdrname.Items.Count; i++)
                    {

                        cbl_itm_hdrname.Items[i].Selected = true;
                    }

                    txt_itn_hdr.Text = "Item Header(" + cbl_itm_hdrname.Items.Count + ")";
                }
            }

        }
        catch
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getnamemm(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct ItemName from IM_ItemMaster WHERE ItemName like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["ItemName"].ToString());
            }
        }
        return name;
    }
    public void loadsubheadername()
    {
        try
        {
            cbl_item_subhdr.Items.Clear();
            string itemheader = "";
            for (int i = 0; i < cbl_itm_hdrname.Items.Count; i++)
            {
                if (cbl_itm_hdrname.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_itm_hdrname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_itm_hdrname.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                string query = "";
                query = "select distinct t.MasterCode,t.MasterValue from CO_MasterValues  t,IM_ItemMaster i where t.MasterCode=i.subheader_code and ItemHeaderCode in ('" + itemheader + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                // ds.Clear();
                // ds = d2.BindItemCodeAll(itemheader);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_item_subhdr.DataSource = ds;
                    cbl_item_subhdr.DataTextField = "MasterValue";
                    cbl_item_subhdr.DataValueField = "MasterCode";
                    cbl_item_subhdr.DataBind();
                    if (cbl_item_subhdr.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_item_subhdr.Items.Count; i++)
                        {
                            cbl_item_subhdr.Items[i].Selected = true;
                        }
                        txt_subhdrname.Text = "Sub Header Name(" + cbl_item_subhdr.Items.Count + ")";
                    }
                    if (cbl_item_subhdr.Items.Count > 5)
                    {

                    }
                }
                else
                {
                    cbl_item_subhdr.Text = "--Select--";
                }
            }
            else
            {
                cbl_item_subhdr.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaffname1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct s.staff_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        // string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";


        name = ws.Getname(query);

        return name;
    }
    public void degree()
    {
        try
        {
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            string query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id";
            ds = d2.select_method_wo_parameter(query, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();

                //if (cbl_degree.Items.Count > 0)
                //{
                //    for (int i = 0; i < cbl_degree.Items.Count; i++)
                //    {
                //        cbl_degree.Items[i].Selected = true;
                //    }
                //    txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                //}
            }
        }
        catch (Exception ex)
        {
        }
    }

    void BindCollege()
    {
        try
        {
            string srisql = "select collname,college_code from collinfo";
            ds.Clear();
            ds = da.select_method_wo_parameter(srisql, "Text");


            ddl_poupcollege.DataSource = ds;
            ddl_poupcollege.DataTextField = "collname";
            ddl_poupcollege.DataValueField = "college_code";
            ddl_poupcollege.DataBind();

            ddl_prnst_staff_clg.DataSource = ds;
            ddl_prnst_staff_clg.DataTextField = "collname";
            ddl_prnst_staff_clg.DataValueField = "college_code";
            ddl_prnst_staff_clg.DataBind();

        }
        catch
        {
        }
    }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> Getroll(string prefixText)
    {
        DataSet dt = new DataSet();
        DAccess2 dsa = new DAccess2();

        List<string> CityNames = new List<string>();
        string strsql = "select distinct r.Roll_No,r.Reg_No,r.stud_name,r.Stud_Type,hd.Hostel_Name,hd.Hostel_code,hs.Floor_Name,hs.Room_Name  from Hostel_StudentDetails hs,Registration r,Hostel_Details hd where hd.Hostel_code=hs.Hostel_Code and hs.Roll_Admit=r.Roll_Admit and r.roll_no like '" + prefixText + "%'";
        dt = dsa.select_method_wo_parameter(strsql, "text");


        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
        {
            CityNames.Add(dt.Tables[0].Rows[i]["roll_no"].ToString());
        }


        return CityNames;


    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        pnl2.Visible = false;
    }


    public void btndescpopadd_Click(object sender, EventArgs e)
    {

        try
        {
            if (txt_description11.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='Action' and CollegeCode ='" + collegecode1 + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='EventAction' and CollegeCode ='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','EventAction','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;

                    lbl_alert.Text = "Added sucessfully";
                    txt_description11.Text = "";

                    //imgdiv33.Visible = false;           
                }

            }
            else
            {
                imgdiv2.Visible = true;
                pnl2.Visible = true;
                lbl_alert.Text = "Enter the description";
            }
            loadaction();
        }
        catch (Exception ex)
        {
        }
        //pop_add_staff_stud_othr.Visible = true;
        //imgdiv33.Visible = false;
        //panel_description11.Visible = false;
    }
    public void btndescpopexit_Click(object sender, EventArgs e)
    {
        imgdiv33.Visible = false;
        panel_description11.Visible = false;
    }
    public void btnaddactiondescrip_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_actiondescription.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_actiondescription.Text + "' and MasterCriteria ='EventCategory' and CollegeCode ='" + collegecode1 + "') update CO_MasterValues set MasterValue ='" + txt_actiondescription.Text + "' where MasterValue ='" + txt_actiondescription.Text + "' and MasterCriteria ='EventCategory' and CollegeCode ='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_actiondescription.Text + "','EventCategory','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    divactdeletedetais.Visible = true;

                    lbl_actdeletedetail.Text = "Added sucessfully";
                    txt_actiondescription.Text = "";
                    divactionadddetails.Visible = false;
                    actadd.Visible = false;
                }

            }
            else
            {
                divactdeletedetais.Visible = true;
                //pnl2.Visible = true;
                lbl_actdeletedetail.Text = "Enter the description";
                divactionadddetails.Visible = false;
                actadd.Visible = false;
            }
            loaddesc();
        }
        catch (Exception ex)
        {
        }
    }
    public void btnexiteactiondescrip_Click(object sender, EventArgs e)
    {
        divactionadddetails.Visible = false;
        actadd.Visible = false;
    }
    public void btactdeletexit_Click(object sender, EventArgs e)
    {
        divactdeletedetais.Visible = false;
    }
    public void btn_min_Click(object sender, EventArgs e)
    {
        if (ddl_viewdetails1.SelectedIndex == -1)
        {
            pop_add_staff_stud_othr.Visible = false;
            divactdeletedetais.Visible = true;
            lbl_actdeletedetail.Text = "No records found";
        }
        else if (ddl_viewdetails1.SelectedIndex == 0)
        {
            pop_add_staff_stud_othr.Visible = false;
            divactdeletedetais.Visible = true;
            lbl_actdeletedetail.Text = "Select any record";
        }
        else if (ddl_viewdetails1.SelectedIndex != 0)
        {
            string sql = "delete from CO_MasterValues where MasterCode='" + ddl_viewdetails1.SelectedItem.Value.ToString() + "' and MasterCriteria='EventCategory' and CollegeCode='" + collegecode1 + "' ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {

                divactdeletedetais.Visible = true;
                lbl_actdeletedetail.Text = "Deleted Sucessfully";
                pop_add_staff_stud_othr.Visible = false;
            }
            else
            {

                divactdeletedetais.Visible = true;
                lbl_actdeletedetail.Text = "No records found";
                pop_add_staff_stud_othr.Visible = false;
            }
            loaddesc();
        }

        else
        {
            pop_add_staff_stud_othr.Visible = false;
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
    }

    public void multiple_event_save()
    {
        string st_time = "";
        string en_time = "";
        string stprd = "";
        string ed_prd = "";
        string loc = "";
        string eve_date = "";
        for (int i = 0; i < GV1.Rows.Count; i++)
        {
            TextBox txtstime = (TextBox)GV1.Rows[i].FindControl("txt_start");
            st_time = Convert.ToString(txtstime.Text);
            TextBox txtetime = (TextBox)GV1.Rows[i].FindControl("txt_end");
            en_time = Convert.ToString(txtetime.Text);
            TextBox txtsprd = (TextBox)GV1.Rows[i].FindControl("txt_st_prd");
            stprd = Convert.ToString(txtsprd.Text);
            TextBox txteprd = (TextBox)GV1.Rows[i].FindControl("txt_end_prd");
            ed_prd = Convert.ToString(txteprd.Text);
            TextBox txtloc = (TextBox)GV1.Rows[i].FindControl("txt");
            loc = Convert.ToString(txtloc.Text);
            TextBox txtdate = (TextBox)GV1.Rows[i].FindControl("txtdate");
            eve_date = Convert.ToString(txtdate.Text);

            string bul = "";
            string flr = "";
            string rm = "";
            string loctype = "";
            string locationvalue1 = "";
            string[] split = loc.Split('-');
            if (split.Length == 1)
            {

            }
            else
            {

                bul = split[0];
                flr = split[1];
                rm = split[2];

            }
            if (rdb1.Checked == true)
            {
                loctype = "0";
                locationvalue1 = "";
            }
            else
            {
                loctype = "1";
                locationvalue1 = loc;
            }
            string eve_name = Convert.ToString(txtothers.Text);
            string eventname = subjectcodenew("EventName", eve_name);
            string noofact = Convert.ToString(txt_min_action.Text);
            string[] ay = eve_date.Split('/');
            string date = ay[1].ToString() + "/" + ay[0].ToString() + "/" + ay[2].ToString();
            string rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");
            string query = "insert into RQ_RequisitionDet(RequisitionFK,EventDate,EventName,StartPeriod,EndPeriod,EventLocation,StartTime,EndTime,LocationType,BuildCode,OutdoorLoc,FloorNo,RoomNo,NoOfAction)values('" + rq_fk + "','" + date + "','" + eventname + "','" + stprd + "','" + ed_prd + "','" + locationvalue1 + "','" + st_time + "','" + en_time + "','" + loctype + "','" + bul + "','" + locationvalue1 + "','" + flr + "','" + rm + "','" + noofact + "')";
            d2.update_method_wo_parameter(query, "Text");
        }
        savemultievent();
    }

    public void addevent()
    {
        string val = "";
        val = "";
        string eve_date = "";
        string description = "";
        string location = "";
        string checkvalue = "";
        int count = 0;
        int count1 = 0;
        int count3 = 0;
        int count2 = 0;
        int count4 = 0;
        int count5 = 0;
        int count6 = 0;
        int count7 = 0;
        string sttime = "";
        string edtime = "";
        string actdes = "";
        string acloc = "";
        string actname = "";

        foreach (GridViewRow row1 in GV1.Rows)
        {
            if (ii == row1.DataItemIndex)
            {
                TextBox txtdate = (TextBox)GV1.Rows[ii].FindControl("txtdate");
                eve_date = Convert.ToString(txtdate.Text);


                foreach (GridViewRow row11 in gridadd.Rows)
                {
                    if (jj == row11.DataItemIndex)
                    {

                        actname = Convert.ToString(ddl_act_namenew.SelectedItem.Text);

                        actdes = Convert.ToString(txt_act_description.Text);

                        TextBox txtstart = (TextBox)gridadd.Rows[jj].FindControl("txt_start");
                        sttime = Convert.ToString(txtstart.Text);

                        TextBox txtend = (TextBox)gridadd.Rows[jj].FindControl("txt_end");
                        edtime = Convert.ToString(txtend.Text);

                        TextBox txtloc = (TextBox)gridadd.Rows[jj].FindControl("txt_loc");
                        acloc = Convert.ToString(txt_min_location.Text);

                        string addaction = actname + "-" + actdes + "-" + sttime + "-" + edtime + "-" + acloc;

                        if (!eventhash.Contains(Convert.ToString(eve_date)))
                        {

                            eventhash.Add(Convert.ToString(eve_date), Convert.ToString(actname));
                        }
                        else
                        {
                            string getvalue = Convert.ToString(eventhash[Convert.ToString(eve_date)]);
                            if (getvalue.Trim() != "")
                            {
                                getvalue = getvalue + "," + actname;
                                eventhash.Remove(Convert.ToString(eve_date));
                                if (getvalue.Trim() != "")
                                {
                                    eventhash.Add(Convert.ToString(eve_date), Convert.ToString(getvalue));
                                }
                            }

                        }

                        if (!actionhash.Contains(Convert.ToString(eve_date)))
                        {

                            actionhash.Add(Convert.ToString(eve_date), Convert.ToString(addaction));
                        }
                        else
                        {
                            string getvalue = Convert.ToString(actionhash[Convert.ToString(eve_date)]);
                            if (getvalue.Trim() != "")
                            {
                                getvalue = getvalue + "/" + addaction;
                                actionhash.Remove(Convert.ToString(eve_date));
                                if (getvalue.Trim() != "")
                                {
                                    actionhash.Add(Convert.ToString(eve_date), Convert.ToString(getvalue));
                                }
                            }

                        }

                        for (int i = 0; i < GridView2.Rows.Count; i++)
                        {
                            CheckBox chkItemHeader = (CheckBox)GridView2.Rows[i].FindControl("chkup3");
                            if (chkItemHeader.Checked == true)
                            {

                                count++;
                                Label stud_appno = (Label)GridView2.Rows[i].FindControl("lblappno");
                                val = Convert.ToString(stud_appno.Text);

                                if (!newparticipant.Contains(Convert.ToString(actname)))
                                {

                                    newparticipant.Add(Convert.ToString(actname), Convert.ToString(val));
                                }
                                else
                                {
                                    string getvalue = Convert.ToString(newparticipant[Convert.ToString(actname)]);
                                    if (getvalue.Trim() != "")
                                    {
                                        getvalue = getvalue + "," + val;
                                        newparticipant.Remove(Convert.ToString(actname));
                                        if (getvalue.Trim() != "")
                                        {
                                            newparticipant.Add(Convert.ToString(actname), Convert.ToString(getvalue));
                                        }
                                    }

                                }
                            }
                        }


                        for (int i = 0; i < GridView1.Rows.Count; i++)
                        {
                            CheckBox chkItemHeader = (CheckBox)GridView1.Rows[i].FindControl("chkup3");
                            if (chkItemHeader.Checked == true)
                            {
                                count1++;
                                Label stud_appno = (Label)GridView1.Rows[i].FindControl("lblappno");
                                val = Convert.ToString(stud_appno.Text);

                                if (!newparticipant.Contains(Convert.ToString(actname)))
                                {

                                    newparticipant.Add(Convert.ToString(actname), Convert.ToString(val));
                                }
                                {
                                    string getvalue = Convert.ToString(newparticipant[Convert.ToString(actname)]);
                                    if (getvalue.Trim() != "")
                                    {
                                        getvalue = getvalue + "," + val;
                                        newparticipant.Remove(Convert.ToString(actname));
                                        if (getvalue.Trim() != "")
                                        {
                                            newparticipant.Add(Convert.ToString(actname), Convert.ToString(getvalue));
                                        }
                                    }

                                }
                            }
                        }
                        ViewState["CurrentTable"] = newparticipant;

                        // PRESENTED PERSON COUNT
                        string val1 = "";
                        string val2 = "";
                        for (int i = 0; i < GridView12.Rows.Count; i++)
                        {
                            CheckBox chkItemHeader = (CheckBox)GridView12.Rows[i].FindControl("chkup3");
                            if (chkItemHeader.Checked == true)
                            {
                                count2++;
                                Label stud_appno = (Label)GridView12.Rows[i].FindControl("lblappno");
                                val1 = Convert.ToString(stud_appno.Text);
                                DropDownList ddl = (DropDownList)GridView12.Rows[i].FindControl("ddl_categofstaff");
                                val2 = Convert.ToString(ddl.Text);

                                val = val1 + "-" + val2;
                                if (!newpresented.Contains(Convert.ToString(actname)))
                                {

                                    newpresented.Add(Convert.ToString(actname), Convert.ToString(val));
                                }
                                else
                                {
                                    string getvalue = Convert.ToString(newpresented[Convert.ToString(actname)]);
                                    if (getvalue.Trim() != "")
                                    {
                                        getvalue = getvalue + "," + val;
                                        newpresented.Remove(Convert.ToString(actname));
                                        if (getvalue.Trim() != "")
                                        {
                                            newpresented.Add(Convert.ToString(actname), Convert.ToString(getvalue));
                                        }
                                    }

                                }
                            }
                        }

                        for (int i = 0; i < GridView13.Rows.Count; i++)
                        {
                            CheckBox chkItemHeader = (CheckBox)GridView13.Rows[i].FindControl("chkup3");
                            if (chkItemHeader.Checked == true)
                            {
                                count3++;
                                Label stud_appno = (Label)GridView12.Rows[i].FindControl("lblappno");
                                val1 = Convert.ToString(stud_appno.Text);
                                DropDownList ddl = (DropDownList)GridView12.Rows[i].FindControl("ddl_categofstaff");
                                val2 = Convert.ToString(ddl.Text);
                                val = val1 + "-" + val2;
                                if (!newpresented.Contains(Convert.ToString(actname)))
                                {

                                    newpresented.Add(Convert.ToString(actname), Convert.ToString(val));
                                }
                                {
                                    string getvalue = Convert.ToString(newpresented[Convert.ToString(actname)]);
                                    if (getvalue.Trim() != "")
                                    {
                                        getvalue = getvalue + "," + val;
                                        newpresented.Remove(Convert.ToString(actname));
                                        if (getvalue.Trim() != "")
                                        {
                                            newpresented.Add(Convert.ToString(actname), Convert.ToString(getvalue));
                                        }
                                    }

                                }
                            }
                        }
                        ViewState["CurrentTable1"] = newpresented;
                        string cname = "";
                        string pname = "";
                        string addr = "";
                        string street = "";
                        string city = "";
                        string pin = "";
                        string country = "";
                        string state = "";
                        string phn = "";
                        string mail = "";
                        string attch = "";
                        string doc = "";
                        for (int k = 0; k < GridView8.Rows.Count; k++)
                        {
                            count4++;
                            TextBox txtamt = (TextBox)GridView8.Rows[k].FindControl("txtactname");
                            cname = Convert.ToString(txtamt.Text);
                            TextBox txtpanam = (TextBox)GridView8.Rows[k].FindControl("txt_per");
                            pname = Convert.ToString(txtpanam.Text);
                            TextBox txtadd = (TextBox)GridView8.Rows[k].FindControl("txt_add");
                            addr = Convert.ToString(txtadd.Text);
                            TextBox txtst = (TextBox)GridView8.Rows[k].FindControl("txt_st");
                            street = Convert.ToString(txtst.Text);
                            TextBox txtcity = (TextBox)GridView8.Rows[k].FindControl("txt_city");
                            city = Convert.ToString(txtcity.Text);
                            TextBox txtpin = (TextBox)GridView8.Rows[k].FindControl("txt_pin");
                            pin = Convert.ToString(txtpin.Text);
                            TextBox txtcou = (TextBox)GridView8.Rows[k].FindControl("txt_country");
                            country = Convert.ToString(txtcou.Text);
                            TextBox txtstate = (TextBox)GridView8.Rows[k].FindControl("txt_state");
                            state = Convert.ToString(txtstate.Text);
                            TextBox txtphn = (TextBox)GridView8.Rows[k].FindControl("txt_phn");
                            phn = Convert.ToString(txtphn.Text);
                            TextBox txtmail = (TextBox)GridView8.Rows[k].FindControl("txt_mail");
                            mail = Convert.ToString(txtmail.Text);
                            TextBox txtattch = (TextBox)GridView8.Rows[k].FindControl("txt_attch");
                            attch = Convert.ToString(txtattch.Text);
                            TextBox txtat = (TextBox)GridView8.Rows[k].FindControl("txt_e");
                            int at = Convert.ToInt32(txtat.Text);
                            TextBox txtdoc = (TextBox)GridView8.Rows[k].FindControl("txt_dt");
                            doc = Convert.ToString(txtdoc.Text);

                            val = cname + "-" + pname + "-" + addr + "-" + street + "-" + city + "-" + pin + "-" + country + "-" + state + "-" + phn + "-" + mail + "-" + attch + "-" + at + "-" + doc;
                            if (!singleparticcomp.Contains(Convert.ToString(actname)))
                            {

                                singleparticcomp.Add(Convert.ToString(actname), Convert.ToString(val));
                            }
                            else
                            {
                                string getvalue = Convert.ToString(singleparticcomp[Convert.ToString(actname)]);
                                if (getvalue.Trim() != "")
                                {
                                    getvalue = getvalue + "," + val;
                                    singleparticcomp.Remove(Convert.ToString(actname));
                                    if (getvalue.Trim() != "")
                                    {
                                        singleparticcomp.Add(Convert.ToString(actname), Convert.ToString(getvalue));
                                    }
                                }

                            }
                        }
                        ViewState["CurrentTableparticcomp"] = singleparticcomp;

                        for (int i = 0; i < GridView9.Rows.Count; i++)
                        {
                            count5++;
                            TextBox txtamt = (TextBox)GridView9.Rows[i].FindControl("txtactname");
                            cname = Convert.ToString(txtamt.Text);
                            TextBox txtpanam = (TextBox)GridView9.Rows[i].FindControl("txt_per");
                            pname = Convert.ToString(txtpanam.Text);
                            TextBox txtadd = (TextBox)GridView9.Rows[i].FindControl("txt_add");
                            addr = Convert.ToString(txtadd.Text);
                            TextBox txtst = (TextBox)GridView9.Rows[i].FindControl("txt_st");
                            street = Convert.ToString(txtst.Text);
                            TextBox txtcity = (TextBox)GridView9.Rows[i].FindControl("txt_city");
                            city = Convert.ToString(txtcity.Text);
                            TextBox txtpin = (TextBox)GridView9.Rows[i].FindControl("txt_pin");
                            pin = Convert.ToString(txtpin.Text);
                            TextBox txtcou = (TextBox)GridView9.Rows[i].FindControl("txt_country");
                            country = Convert.ToString(txtcou.Text);
                            TextBox txtstate = (TextBox)GridView9.Rows[i].FindControl("txt_state");
                            state = Convert.ToString(txtstate.Text);
                            TextBox txtphn = (TextBox)GridView9.Rows[i].FindControl("txt_phn");
                            phn = Convert.ToString(txtphn.Text);
                            TextBox txtmail = (TextBox)GridView9.Rows[i].FindControl("txt_mail");
                            mail = Convert.ToString(txtmail.Text);
                            TextBox txtattch = (TextBox)GridView9.Rows[i].FindControl("txt_attch");
                            attch = Convert.ToString(txtattch.Text);
                            TextBox txtat = (TextBox)GridView9.Rows[i].FindControl("txt_e");
                            int at = Convert.ToInt32(txtat.Text);
                            TextBox txtdoc = (TextBox)GridView9.Rows[i].FindControl("txt_dt");
                            doc = Convert.ToString(txtdoc.Text);
                            val = cname + "-" + pname + "-" + addr + "-" + street + "-" + city + "-" + pin + "-" + country + "-" + state + "-" + phn + "-" + mail + "-" + attch + "-" + at + "-" + doc;
                            if (!singleparticindi.Contains(Convert.ToString(actname)))
                            {

                                singleparticindi.Add(Convert.ToString(actname), Convert.ToString(val));
                            }
                            else
                            {
                                string getvalue = Convert.ToString(singleparticindi[Convert.ToString(actname)]);
                                if (getvalue.Trim() != "")
                                {
                                    getvalue = getvalue + "," + val;
                                    singleparticindi.Remove(Convert.ToString(actname));
                                    if (getvalue.Trim() != "")
                                    {
                                        singleparticindi.Add(Convert.ToString(actname), Convert.ToString(getvalue));
                                    }
                                }

                            }
                        }
                        ViewState["CurrentTableparticindi"] = singleparticindi;


                        for (int k = 0; k < GridView10.Rows.Count; k++)
                        {
                            count6++;

                            TextBox txtamtindi = (TextBox)GridView10.Rows[k].FindControl("txtactname");
                            cname = Convert.ToString(txtamtindi.Text);
                            TextBox txtpanam = (TextBox)GridView10.Rows[k].FindControl("txt_per");
                            pname = Convert.ToString(txtpanam.Text);
                            TextBox txtadd = (TextBox)GridView10.Rows[k].FindControl("txt_add");
                            addr = Convert.ToString(txtadd.Text);
                            TextBox txtst = (TextBox)GridView10.Rows[k].FindControl("txt_st");
                            street = Convert.ToString(txtst.Text);
                            TextBox txtcity = (TextBox)GridView10.Rows[k].FindControl("txt_city");
                            city = Convert.ToString(txtcity.Text);
                            TextBox txtpin = (TextBox)GridView10.Rows[k].FindControl("txt_pin");
                            pin = Convert.ToString(txtpin.Text);
                            TextBox txtcou = (TextBox)GridView10.Rows[k].FindControl("txt_country");
                            country = Convert.ToString(txtcou.Text);
                            TextBox txtstate = (TextBox)GridView10.Rows[k].FindControl("txt_state");
                            state = Convert.ToString(txtstate.Text);
                            TextBox txtphn = (TextBox)GridView10.Rows[k].FindControl("txt_phn");
                            phn = Convert.ToString(txtphn.Text);
                            TextBox txtmail = (TextBox)GridView10.Rows[k].FindControl("txt_mail");
                            mail = Convert.ToString(txtmail.Text);
                            TextBox txtattch = (TextBox)GridView10.Rows[k].FindControl("txtattch");
                            attch = Convert.ToString(txtattch.Text);
                            TextBox txtat = (TextBox)GridView10.Rows[k].FindControl("txt_e");
                            int at = Convert.ToInt32(txtat.Text);
                            TextBox txtdoc = (TextBox)GridView10.Rows[k].FindControl("txt_dt");
                            doc = Convert.ToString(txtdoc.Text);

                            val = cname + "-" + pname + "-" + addr + "-" + street + "-" + city + "-" + pin + "-" + country + "-" + state + "-" + phn + "-" + mail + "-" + attch + "-" + at + "-" + doc;
                            if (!singlepresentindi.Contains(Convert.ToString(actname)))
                            {

                                singlepresentindi.Add(Convert.ToString(actname), Convert.ToString(val));
                            }
                            else
                            {
                                string getvalue = Convert.ToString(singlepresentindi[Convert.ToString(actname)]);
                                if (getvalue.Trim() != "")
                                {
                                    getvalue = getvalue + "," + val;
                                    singlepresentindi.Remove(Convert.ToString(actname));
                                    if (getvalue.Trim() != "")
                                    {
                                        singlepresentindi.Add(Convert.ToString(actname), Convert.ToString(getvalue));
                                    }
                                }

                            }

                        }
                        ViewState["CurrentTablesingleindi"] = singlepresentindi;

                        for (int k = 0; k < GridView11.Rows.Count; k++)
                        {
                            TextBox txtamt = (TextBox)GridView11.Rows[k].FindControl("txtactname");
                            cname = Convert.ToString(txtamt.Text);
                            TextBox txtpanam = (TextBox)GridView11.Rows[k].FindControl("txt_per");
                            pname = Convert.ToString(txtpanam.Text);
                            TextBox txtadd = (TextBox)GridView11.Rows[k].FindControl("txt_add");
                            addr = Convert.ToString(txtadd.Text);
                            TextBox txtst = (TextBox)GridView11.Rows[k].FindControl("txt_st");
                            street = Convert.ToString(txtst.Text);
                            TextBox txtcity = (TextBox)GridView11.Rows[k].FindControl("txt_city");
                            city = Convert.ToString(txtcity.Text);
                            TextBox txtpin = (TextBox)GridView11.Rows[k].FindControl("txt_pin");
                            pin = Convert.ToString(txtpin.Text);
                            TextBox txtcou = (TextBox)GridView11.Rows[k].FindControl("txt_country");
                            country = Convert.ToString(txtcou.Text);
                            TextBox txtstate = (TextBox)GridView11.Rows[k].FindControl("txt_state");
                            state = Convert.ToString(txtstate.Text);
                            TextBox txtphn = (TextBox)GridView11.Rows[k].FindControl("txt_phn");
                            phn = Convert.ToString(txtphn.Text);
                            TextBox txtmail = (TextBox)GridView11.Rows[k].FindControl("txt_mail");
                            mail = Convert.ToString(txtmail.Text);
                            TextBox txtattch = (TextBox)GridView11.Rows[k].FindControl("txt_attch");
                            attch = Convert.ToString(txtattch.Text);
                            TextBox txtat = (TextBox)GridView11.Rows[k].FindControl("txt_e");
                            int at = Convert.ToInt32(txtat.Text);
                            TextBox txtdoc = (TextBox)GridView11.Rows[k].FindControl("txt_dt");
                            doc = Convert.ToString(txtdoc.Text);
                            count7++;
                            val = cname + "-" + pname + "-" + addr + "-" + street + "-" + city + "-" + pin + "-" + country + "-" + state + "-" + phn + "-" + mail + "-" + attch + "-" + at + "-" + doc;
                            if (!singlepresentcomp.Contains(Convert.ToString(actname)))
                            {

                                singlepresentcomp.Add(Convert.ToString(actname), Convert.ToString(val));
                            }
                            else
                            {
                                string getvalue = Convert.ToString(singlepresentcomp[Convert.ToString(actname)]);
                                if (getvalue.Trim() != "")
                                {
                                    getvalue = getvalue + "," + val;
                                    singlepresentcomp.Remove(Convert.ToString(actname));
                                    if (getvalue.Trim() != "")
                                    {
                                        singlepresentcomp.Add(Convert.ToString(actname), Convert.ToString(getvalue));
                                    }
                                }

                            }
                        }
                        ViewState["CurrentTablesinglecomp"] = singlepresentcomp;


                        //......................................................................................................................
                        int totcount = count + count1 + count4 + count5;
                        int totcountprs = count2 + count3 + count6 + count7;

                        description = Convert.ToString(txt_act_description.Text);
                        location = Convert.ToString(txt_min_location.Text);
                        foreach (GridViewRow row in gridadd.Rows)
                        {
                            if (jj == row.DataItemIndex)
                            {
                                TextBox txtaname = (TextBox)gridadd.Rows[jj].FindControl("txtactname");
                                txtaname.Text = actname;

                                TextBox txtdes1 = (TextBox)gridadd.Rows[jj].FindControl("txt_descri");
                                txtdes1.Text = description;

                                TextBox txtloc1 = (TextBox)gridadd.Rows[jj].FindControl("txt_loc");
                                txtloc1.Text = location;

                                TextBox txtpart = (TextBox)gridadd.Rows[jj].FindControl("txt_noact");
                                txtpart.Text = Convert.ToString(totcount);
                                TextBox txtperst = (TextBox)gridadd.Rows[jj].FindControl("txt_noconper");
                                txtperst.Text = Convert.ToString(totcountprs);
                            }
                        }
                    }
                }
            }
        }
        ViewState["CurrentTablenew"] = eventhash;
        ViewState["CurrentTablenewaction"] = actionhash;

    }

    public void savemultievent()
    {
        try
        {
            string cname = "";
            string pname = "";
            string addr = "";
            string street = "";
            string city = "";
            string pin = "";
            string country = "";
            string state = "";
            string phn = "";
            string mail = "";
            string attch = "";
            string doc = "";
            int atd = 0;
            string VendorCode = "";
            string query = "";
            string appno;
            string memtype = "";
            string actionname = "";
            string staffname = "";
            string eventdatee = "";
            string batch = Convert.ToString(ddl_org_batch.SelectedItem.Value);
            string degree = "";
            string sem = "";
            string actionfk = "";
            string eventdate = "";
            string eventactionname = "";
            string eventtactnname = "";
            string loctype = "";
            string outdoorloc = "";
            string bul = "";
            string flr = "";
            string rm = "";
            if (rdb1.Checked == true)
            {
                loctype = "0";
            }
            else
            {
                loctype = "1";
                outdoorloc = Convert.ToString(txt_min_location.Text);
            }

            string rq_fk = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))");
            for (int i = 0; i < GV1.Rows.Count; i++)
            {
                TextBox txteventDATE = (TextBox)GV1.Rows[i].FindControl("txtdate");
                eventdate = txteventDATE.Text;

                DateTime reqactdate = new DateTime();
                reqactdate = TextToDate(txteventDATE);
                if (eventhash.Count > 0)
                {
                    eventactionname = Convert.ToString(eventhash[eventdate]);

                    if (actionhash.Count > 0)
                    {
                        eventtactnname = Convert.ToString(actionhash[eventdate]);

                        string[] arraysplit = eventtactnname.Split('/');
                        for (int k = 0; k < arraysplit.Length; k++)
                        {
                            string actarrayval = arraysplit[k];

                            string[] split = actarrayval.Split('-');

                            string Aname = split[0];
                            string dname = split[1];
                            string stime = split[2];
                            string etime = split[3];
                            bul = split[4];
                            flr = split[5];
                            rm = split[6];


                            string getact = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + Aname + "'"); ;
                            query = "insert into RQ_ReqActionDet(ActionName,ACtionDesc,StartTime,EndTime,LocationType,OutdoorLoc,BuildCode,FloorNo,RoomNo,RequisitionFK,EventDate)values('" + getact + "','" + dname + "','" + stime + "','" + etime + "','" + loctype + "','" + outdoorloc + "','" + bul + "','" + flr + "','" + rm + "','" + rq_fk + "','" + reqactdate + "')";
                            d2.update_method_wo_parameter(query, "Text");

                            if (newparticipant.Count > 0)
                            {
                                appno = Convert.ToString(newparticipant[Aname]);

                                string[] array = appno.Split(',');
                                for (int j = 0; j < array.Length; j++)
                                {
                                    string mem = d2.GetFunction("select distinct s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and sa.appl_id = '" + array[j] + "'");
                                    if (mem != "0")
                                    {
                                        memtype = "1";

                                    }
                                    else
                                    {
                                        string mem1 = d2.GetFunction("select App_No from Registration where App_No ='" + array[j] + "'");

                                        memtype = "2";

                                    }
                                    string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + Aname + "'");
                                    actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where ActionName='" + app + "'))");
                                    query = "insert into RQ_EventMemberDet(MemType,BatchYear,DegreeCode,Semester,ApplNo,RequisitionFK,ActionFK,ActionType)values('" + memtype + "','','','','" + array[j] + "','" + rq_fk + "','" + actionfk + "','1')";
                                    d2.update_method_wo_parameter(query, "Text");
                                }

                            }
                            string array_staff = "";
                            string array_catg = "";
                            ////........................ presented person save..................
                            if (newpresented.Count > 0)
                            {
                                appno = Convert.ToString(newpresented[Aname]);
                                string[] array = appno.Split(',');

                                for (int j = 0; j < array.Length; j++)
                                {
                                    string arr = array[j];
                                    if (arr.Length > 1)
                                    {
                                        string[] array1 = arr.Split('-');
                                        array_staff = array1[0];
                                        array_catg = array1[1];
                                    }
                                    string array_catg_code = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='EventCategory' and MasterValue='" + array_catg + "' ");

                                    string mem = d2.GetFunction("select distinct s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and sa.appl_id = '" + array_staff + "'");
                                    if (mem != "0")
                                    {
                                        memtype = "1";

                                    }
                                    else
                                    {
                                        string mem1 = d2.GetFunction("select App_No from Registration where App_No IN ='" + array_staff + "'");

                                        memtype = "2";

                                    }
                                    string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + Aname + "'");
                                    actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where ActionName='" + app + "'))");
                                    query = "insert into RQ_EventMemberDet(MemType,BatchYear,DegreeCode,Semester,ApplNo,MemberAction,RequisitionFK,ActionFK,ActionType)values('" + memtype + "','','','','" + array_staff + "','" + array_catg_code + "','" + rq_fk + "','" + actionfk + "','2')";
                                    d2.update_method_wo_parameter(query, "Text");
                                }

                            }

                            ///.......................................

                            if (singlepresentindi.Count > 0)
                            {
                                appno = Convert.ToString(singlepresentindi[Aname]);
                                if (appno != "")
                                {
                                    string[] array = appno.Split(',');

                                    for (int j = 0; j < array.Length; j++)
                                    {
                                        string totindi = array[j];

                                        string[] arr = totindi.Split('-');

                                        cname = arr[0];
                                        pname = arr[1];
                                        addr = arr[2];
                                        street = arr[3];
                                        city = arr[4];
                                        pin = arr[5];
                                        country = arr[6];
                                        string country1 = subjectcodenew("coun", country);
                                        state = arr[7];
                                        string state1 = subjectcodenew("state", state);
                                        phn = arr[8];
                                        mail = arr[9];
                                        attch = arr[10];
                                        string atttt = arr[11];
                                        doc = arr[12];
                                        atd = Convert.ToInt32(atttt);

                                        string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=7 and VendorCompName like '" + cname + "%'");

                                        if (VenCode != "" && VenCode != null && VenCode != "0")
                                        {
                                            VendorCode = VenCode;
                                        }
                                        else
                                        {
                                            VendorCodeGen();
                                            VendorCode = Session["VendorCode"].ToString();
                                            string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + cname + "' and VendorType='7') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + cname + "',VendorType='7',VendorAddress='" + addr + "',VendorStreet='" + street + "',VendorCity='" + city + "',VendorPin='" + pin + "',VendorName='" + pname + "',VendorPhoneNo='" + phn + "',VendorEmailID='" + mail + "',VendorCountry='" + country1 + "',VendorState='" + state1 + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + cname + "' and VendorType='7' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry,VendorState) values('" + VendorCode + "','" + cname + "','7','" + addr + "','" + street + "','" + city + "','" + pin + "','" + pname + "','" + phn + "','" + mail + "'," + country1 + "','" + state1 + "')";
                                            int vc = d2.update_method_wo_parameter(venmst, "TEXT");
                                        }
                                        Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
                                        string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + pname + "',VendorMobileNo='" + phn + "',VendorPhoneNo='" + phn + "',VendorEmail='" + mail + "' where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorMobileNo,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + pname + "','" + phn + "','" + phn + "','" + mail + "')";
                                        int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");

                                        bool savnotsflag = false;
                                        if (attch != "" && atd != 0)
                                        {

                                            string fileName = attch;

                                            int fileSize = atd;

                                            byte[] documentBinary = new byte[fileSize];


                                            string date = DateTime.Now.ToString("MM/dd/yyyy");
                                            SqlCommand cmdnotes = new SqlCommand();

                                            rq_fk1 = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                                            //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                                            cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + VendorFK + "'";
                                            cmdnotes.CommandType = CommandType.Text;
                                            cmdnotes.Connection = ssql;

                                            SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                                            DocName.Value = fileName.ToString();
                                            cmdnotes.Parameters.Add(DocName);

                                            SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                                            Type.Value = doc.ToString();
                                            cmdnotes.Parameters.Add(Type);

                                            SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                                            uploadedDocument.Value = documentBinary;
                                            cmdnotes.Parameters.Add(uploadedDocument);


                                            ssql.Close();
                                            ssql.Open();
                                            int result = cmdnotes.ExecuteNonQuery();
                                            if (result > 0)
                                            {
                                                savnotsflag = true;
                                            }

                                        }

                                        string vk = d2.GetFunction("select VendorContactPK from CO_VendorMaster co,IM_VendorContactMaster im where co.VendorCode='" + VendorCode + "' and VendorType=7 and im.VenContactName='" + pname + "'");
                                        string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + Aname + "'");
                                        actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where ActionName='" + app + "'))");
                                        query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionFK,ActionType,MemType)values('" + vk + "','" + rq_fk + "','" + actionfk + "','2','3')";
                                        d2.update_method_wo_parameter(query, "Text");
                                    }
                                }
                            }



                            if (singlepresentcomp.Count > 0)
                            {
                                appno = Convert.ToString(singlepresentcomp[Aname]);
                                if (appno != "")
                                {
                                    string[] array = appno.Split(',');

                                    for (int j = 0; j < array.Length; j++)
                                    {
                                        string totindi = array[j];

                                        string[] arr = totindi.Split('-');

                                        cname = arr[0];
                                        pname = arr[1];
                                        addr = arr[2];
                                        street = arr[3];
                                        city = arr[4];
                                        pin = arr[5];
                                        country = arr[6];
                                        string country1 = subjectcodenew("coun", country);
                                        state = arr[7];
                                        string state1 = subjectcodenew("state", state);
                                        phn = arr[8];
                                        mail = arr[9];
                                        attch = arr[10];
                                        string atttt = arr[11];
                                        doc = arr[12];
                                        atd = Convert.ToInt32(atttt);
                                        string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=4 and VendorCompName like '" + cname + "%'");

                                        if (VenCode != "" && VenCode != null && VenCode != "0")
                                        {
                                            VendorCode = VenCode;
                                        }
                                        else
                                        {
                                            VendorCodeGen();
                                            VendorCode = Session["VendorCode"].ToString();
                                            string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + cname + "' and VendorType='4') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + cname + "',VendorType='4',VendorAddress='" + addr + "',VendorStreet='" + street + "',VendorCity='" + city + "',VendorPin='" + pin + "',VendorName='" + pname + "',VendorPhoneNo='" + phn + "',VendorEmailID='" + mail + "',VendorCountry='" + country1 + "',VendorState='" + state1 + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + cname + "' and VendorType='4' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry,VendorState) values('" + VendorCode + "','" + cname + "','4','" + addr + "','" + street + "','" + city + "','" + pin + "','" + pname + "','" + phn + "','" + mail + "','" + country1 + "','" + state1 + "')";
                                            int vc = d2.update_method_wo_parameter(venmst, "TEXT");
                                        }
                                        Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
                                        string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + pname + "',VendorMobileNo='" + phn + "',VendorPhoneNo='" + phn + "',VendorEmail='" + mail + "' where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorMobileNo,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + pname + "','" + phn + "','" + phn + "','" + mail + "')";
                                        int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");

                                        bool savnotsflag = false;
                                        if (attch != "" && atd != 0)
                                        {

                                            string fileName = attch;

                                            int fileSize = atd;

                                            byte[] documentBinary = new byte[fileSize];


                                            string date = DateTime.Now.ToString("MM/dd/yyyy");
                                            SqlCommand cmdnotes = new SqlCommand();

                                            rq_fk1 = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                                            //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                                            cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + VendorFK + "'";
                                            cmdnotes.CommandType = CommandType.Text;
                                            cmdnotes.Connection = ssql;

                                            SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                                            DocName.Value = fileName.ToString();
                                            cmdnotes.Parameters.Add(DocName);

                                            SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                                            Type.Value = doc.ToString();
                                            cmdnotes.Parameters.Add(Type);

                                            SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                                            uploadedDocument.Value = documentBinary;
                                            cmdnotes.Parameters.Add(uploadedDocument);


                                            ssql.Close();
                                            ssql.Open();
                                            int result = cmdnotes.ExecuteNonQuery();
                                            if (result > 0)
                                            {
                                                savnotsflag = true;
                                            }

                                        }

                                        string vk = d2.GetFunction("select VendorContactPK from CO_VendorMaster co,IM_VendorContactMaster im where co.VendorCode='" + VendorCode + "' and VendorType=4 and im.VenContactName='" + pname + "'");
                                        string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + Aname + "'");
                                        actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where ActionName='" + app + "'))");
                                        query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionFK,ActionType,MemType)values('" + vk + "','" + rq_fk + "','" + actionfk + "','2','4')";
                                        d2.update_method_wo_parameter(query, "Text");
                                    }
                                }
                            }
                            if (singleparticcomp.Count > 0)
                            {
                                appno = Convert.ToString(singleparticcomp[eventactionname]);
                                if (appno != "")
                                {
                                    string[] array = appno.Split(',');

                                    for (int j = 0; j < array.Length; j++)
                                    {
                                        string totindi = array[j];

                                        string[] arr = totindi.Split('-');

                                        cname = arr[0];
                                        pname = arr[1];
                                        addr = arr[2];
                                        street = arr[3];
                                        city = arr[4];
                                        pin = arr[5];
                                        country = arr[6];
                                        string country1 = subjectcodenew("coun", country);
                                        state = arr[7];
                                        string state1 = subjectcodenew("state", state);
                                        phn = arr[8];
                                        mail = arr[9];
                                        attch = arr[10];
                                        string atttt = arr[11];
                                        doc = arr[12];
                                        atd = Convert.ToInt32(atttt);

                                        string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=4 and VendorCompName like '" + cname + "%'");

                                        if (VenCode != "" && VenCode != null && VenCode != "0")
                                        {
                                            VendorCode = VenCode;
                                        }
                                        else
                                        {
                                            VendorCodeGen();
                                            VendorCode = Session["VendorCode"].ToString();
                                            string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + cname + "' and VendorType='4') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + cname + "',VendorType='4',VendorAddress='" + addr + "',VendorStreet='" + street + "',VendorCity='" + city + "',VendorPin='" + pin + "',VendorName='" + pname + "',VendorPhoneNo='" + phn + "',VendorEmailID='" + mail + "',VendorCountry='" + country1 + "',VendorState='" + state1 + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + cname + "' and VendorType='4' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry,VendorState) values('" + VendorCode + "','" + cname + "','4','" + addr + "','" + street + "','" + city + "','" + pin + "','" + pname + "','" + phn + "','" + mail + "'," + country1 + "','" + state1 + "')";
                                            int vc = d2.update_method_wo_parameter(venmst, "TEXT");
                                        }
                                        Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
                                        string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + pname + "',VendorMobileNo='" + phn + "',VendorPhoneNo='" + phn + "',VendorEmail='" + mail + "' where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorMobileNo,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + pname + "','" + phn + "','" + phn + "','" + mail + "')";
                                        int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");

                                        bool savnotsflag = false;
                                        if (attch != "" && atd != 0)
                                        {

                                            string fileName = attch;

                                            int fileSize = atd;

                                            byte[] documentBinary = new byte[fileSize];


                                            string date = DateTime.Now.ToString("MM/dd/yyyy");
                                            SqlCommand cmdnotes = new SqlCommand();

                                            rq_fk1 = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                                            //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                                            cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + VendorFK + "'";
                                            cmdnotes.CommandType = CommandType.Text;
                                            cmdnotes.Connection = ssql;

                                            SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                                            DocName.Value = fileName.ToString();
                                            cmdnotes.Parameters.Add(DocName);

                                            SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                                            Type.Value = doc.ToString();
                                            cmdnotes.Parameters.Add(Type);

                                            SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                                            uploadedDocument.Value = documentBinary;
                                            cmdnotes.Parameters.Add(uploadedDocument);


                                            ssql.Close();
                                            ssql.Open();
                                            int result = cmdnotes.ExecuteNonQuery();
                                            if (result > 0)
                                            {
                                                savnotsflag = true;
                                            }

                                        }

                                        string vk = d2.GetFunction("select VendorContactPK from CO_VendorMaster co,IM_VendorContactMaster im where co.VendorCode='" + VendorCode + "' and VendorType=4 and im.VenContactName='" + pname + "'");
                                        string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + Aname + "'");
                                        actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where RequisitionFK='" + rq_fk + "' and ActionName='" + app + "'))");
                                        query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionFK,ActionType,MemType)values('" + vk + "','" + rq_fk + "','" + actionfk + "','1','4')";
                                        d2.update_method_wo_parameter(query, "Text");
                                    }
                                }
                            }

                            if (singleparticindi.Count > 0)
                            {
                                appno = Convert.ToString(singleparticindi[eventactionname]);
                                if (appno != "")
                                {
                                    string[] array = appno.Split(',');

                                    for (int j = 0; j < array.Length; j++)
                                    {
                                        string totindi = array[j];

                                        string[] arr = totindi.Split('-');

                                        cname = arr[0];
                                        pname = arr[1];
                                        addr = arr[2];
                                        street = arr[3];
                                        city = arr[4];
                                        pin = arr[5];
                                        country = arr[6];
                                        string country1 = subjectcodenew("coun", country);
                                        state = arr[7];
                                        string state1 = subjectcodenew("state", state);
                                        phn = arr[8];
                                        mail = arr[9];
                                        attch = arr[10];
                                        string atttt = arr[11];
                                        doc = arr[12];
                                        atd = Convert.ToInt32(atttt);

                                        string VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=7 and VendorCompName like '" + cname + "%'");

                                        if (VenCode != "" && VenCode != null && VenCode != "0")
                                        {
                                            VendorCode = VenCode;
                                        }
                                        else
                                        {
                                            VendorCodeGen();
                                            VendorCode = Session["VendorCode"].ToString();
                                            string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + cname + "' and VendorType='7') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + cname + "',VendorType='7',VendorAddress='" + addr + "',VendorStreet='" + street + "',VendorCity='" + city + "',VendorPin='" + pin + "',VendorName='" + pname + "',VendorPhoneNo='" + phn + "',VendorEmailID='" + mail + "',VendorCountry='" + country1 + "',VendorState='" + state1 + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + cname + "' and VendorType='7' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType,VendorAddress,VendorStreet,VendorCity,VendorPin,VendorName,VendorPhoneNo,VendorEmailID,VendorCountry,VendorState) values('" + VendorCode + "','" + cname + "','7','" + addr + "','" + street + "','" + city + "','" + pin + "','" + pname + "','" + phn + "','" + mail + "','" + country1 + "','" + state1 + "')";
                                            int vc = d2.update_method_wo_parameter(venmst, "TEXT");
                                        }
                                        Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
                                        string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "',VenContactName='" + pname + "',VendorMobileNo='" + phn + "',VendorPhoneNo='" + phn + "',VendorEmail='" + mail + "' where VendorFK='" + VendorFK + "' and VenContactName='" + pname + "' and VendorMobileNo='" + phn + "'  else insert into IM_VendorContactMaster(VendorFK,VenContactName,VendorMobileNo,VendorPhoneNo,VendorEmail) values('" + VendorFK + "','" + pname + "','" + phn + "','" + phn + "','" + mail + "')";
                                        int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");

                                        bool savnotsflag = false;
                                        if (attch != "" && atd != 0)
                                        {

                                            string fileName = attch;

                                            int fileSize = atd;

                                            byte[] documentBinary = new byte[fileSize];


                                            string date = DateTime.Now.ToString("MM/dd/yyyy");
                                            SqlCommand cmdnotes = new SqlCommand();

                                            rq_fk1 = d2.GetFunction("select VendorFK from im_VendorContactMaster where VendorFK=((select max(VendorFK) from im_VendorContactMaster))");
                                            //  cmdnotes.CommandText = "insert into lettertbl(filename,filetype,filedata)" + " VALUES (@DocName,@Type,@DocData)";
                                            cmdnotes.CommandText = " update im_VendorContactMaster set FileName=@DocName, AttachDoc=@DocData, Filetype=@Type where VendorFK='" + VendorFK + "'";
                                            cmdnotes.CommandType = CommandType.Text;
                                            cmdnotes.Connection = ssql;

                                            SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 100);
                                            DocName.Value = fileName.ToString();
                                            cmdnotes.Parameters.Add(DocName);

                                            SqlParameter Type = new SqlParameter("@Type", SqlDbType.NVarChar, 100);
                                            Type.Value = doc.ToString();
                                            cmdnotes.Parameters.Add(Type);

                                            SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                                            uploadedDocument.Value = documentBinary;
                                            cmdnotes.Parameters.Add(uploadedDocument);


                                            ssql.Close();
                                            ssql.Open();
                                            int result = cmdnotes.ExecuteNonQuery();
                                            if (result > 0)
                                            {
                                                savnotsflag = true;
                                            }

                                        }

                                        string vk = d2.GetFunction("select VendorContactPK from CO_VendorMaster co,IM_VendorContactMaster im where co.VendorCode='" + VendorCode + "' and VendorType=7 and im.VenContactName='" + pname + "'");
                                        string app = d2.GetFunction("select MasterCode from CO_MasterValues where MasterCriteria='Action' and MasterValue='" + Aname + "'");
                                        actionfk = d2.GetFunction("select ActionPK from RQ_ReqActionDet where ActionPK=((select max(ActionPK) from RQ_ReqActionDet where RequisitionFK='" + rq_fk + "' and ActionName='" + app + "'))");
                                        query = "insert into RQ_EventMemberDet(ApplNo,RequisitionFK,ActionFK,ActionType,MemType)values('" + vk + "','" + rq_fk + "','" + actionfk + "','1','3')";
                                        d2.update_method_wo_parameter(query, "Text");
                                    }
                                }
                            }

                            //..................................................................
                        }
                    }
                }
            }

        }
        catch
        {
        }


    }

    public void spd_clear()
    {
        loadstaffdep1(college);
        loadstaffdept1(college);
        bind_stafType1();
        bind_stafTypenew();
        bind_design1();
        bindbranch1(college);
        txt_rollno1.Text = "";
        bind_design2();
        bindbranch2(college);
        txt_prsnt_roll.Text = "";
        Label32.Visible = false;
        loadfsstaff2();
        prsnt_studbind();

        GridView13.Visible = true;
        GridView12.Visible = true;
        if (rdo_indivparti.Checked == true)
        {

        }
    }

    public void gridclear_presented()
    {
        if (rdo_indivparti.Checked == true)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView2.DataSource = null;
            GridView2.DataBind();
        }
        GridView12.DataSource = null;
        GridView12.DataBind();
        GridView13.DataSource = null;
        GridView13.DataBind();
        GridView13.Visible = true;
        GridView12.Visible = true;
        GridView10.Visible = false;
        GridView10.DataSource = null;
        GridView10.DataBind();
        GridView11.Visible = false;
        GridView11.DataSource = null;
        GridView11.DataBind();
    }
    protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkBoxHeader = (CheckBox)GridView1.HeaderRow.FindControl("chkboxSelectAll");
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkup3");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }
        catch
        {
        }
    }

    protected void chkboxSelectAllSTAFF_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkBoxHeader = (CheckBox)GridView2.HeaderRow.FindControl("chkboxSelectAll");
            foreach (GridViewRow row in GridView2.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkup3");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }
        catch
        {
        }
    }

    protected void chkboxSelectAllstud2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkBoxHeader = (CheckBox)GridView13.HeaderRow.FindControl("chkboxSelectAll");
            foreach (GridViewRow row in GridView13.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkup3");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }
        catch
        {
        }
    }
    protected void chkboxSelectAllstaff2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ChkBoxHeader = (CheckBox)GridView12.HeaderRow.FindControl("chkboxSelectAll");
            foreach (GridViewRow row in GridView12.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkup3");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
        }
        catch
        {
        }
    }

    public void All_dropdownchange()
    {
        if (ddl_popuptitle.SelectedItem.Text == "Others")
        {
            txt_poprd_title.Attributes.Add("style", "display:block");

        }
        else
        {
            txt_poprd_title.Attributes.Add("style", "display:none");
        }

        if (ddl_seminar.SelectedItem.Text == "Others")
        {
            txt_seminar.Attributes.Add("style", "display:block");

        }
        else
        {
            txt_seminar.Attributes.Add("style", "display:none");
        }

        if (ddl_awdcat.SelectedItem.Text == "Others")
        {
            txt_awdcat.Attributes.Add("style", "display:block");
        }
        else
        {
            txt_awdcat.Attributes.Add("style", "display:none");
        }

        if (ddl_Tournament.SelectedItem.Text == "Others")
        {
            txt_Tournament.Attributes.Add("style", "display:block");
        }
        else
        {
            txt_Tournament.Attributes.Add("style", "display:none");
        }

        if (ddl_game.SelectedItem.Text == "Others")
        {
            txt_game.Attributes.Add("style", "display:block");
        }
        else
        {
            txt_game.Attributes.Add("style", "display:none");
        }
    }
    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView2.PageIndex = e.NewPageIndex;
        loadfsstaff1();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        btn_go1stud_Click(sender, e);
    }
    protected void GridView12_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView12.PageIndex = e.NewPageIndex;
        loadfsstaff2();
    }
    protected void GridView13_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView13.PageIndex = e.NewPageIndex;
        btn_prsnt_studgo_Click(sender, e);
    }
    protected void GridView12_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var ddl = (DropDownList)e.Row.FindControl("ddl_categofstaff");
            //string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='EventCategory' and CollegeCode ='" + collegecode1 + "'";
            //ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (dsnew.Tables[0].Rows.Count > 0)
            {
                ddl.DataSource = dsnew;
                ddl.DataTextField = "MasterValue";
                ddl.DataValueField = "MasterCode";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
    }

    protected void GridView13_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var ddl = (DropDownList)e.Row.FindControl("ddl_categofstaff");
            string sql = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='EventCategory' and CollegeCode ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl.DataSource = ds;
                ddl.DataTextField = "MasterValue";
                ddl.DataValueField = "MasterCode";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
    }
    public void clear_multipeve()
    {
        GV1.Visible = false;
        GV1.DataSource = null;
        GV1.DataBind();
        pop_Gv1_div.Visible = false;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getstate(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct top (50)TEXTVAL from TextValTable where TextCriteria ='state' and college_code=13 and TEXTVAL like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getcountry(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select TEXTVAL from TextValTable where TextCriteria ='coun' and TEXTVAL like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;

    }

    public void btn_divdown_Click(object sender, EventArgs e)
    {
        divdown.Visible = false;
    }

    public void Sponser_save()
    {
        string VendorCode = "";
        string VenCode = "";
        string vendortype = "";
        string companyname = "";
        if (rdbcompany.Checked == true)
        {
            VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=4 and VendorCompName like '" + txt_spn_cmpy.Text + "%'");
            vendortype = "4";
            companyname = Convert.ToString(txt_spn_cmpy.Text);
        }
        else if (rdbsponser.Checked == true)
        {
            VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=6 and VendorCompName like '" + txt_sponscmp_name.Text + "%'");
            vendortype = "6";
            companyname = Convert.ToString(txt_sponscmp_name.Text);
        }
        else if (rdbdept.Checked == true)
        {
            VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=8 and VendorCompName like '" + txt_departmentname.Text + "%'");
            vendortype = "8";
            companyname = Convert.ToString(txt_departmentname.Text);
        }
        else if (rdbinst.Checked == true)
        {
            VenCode = d2.GetFunction("select VendorCode from CO_VendorMaster where VendorType=5 and VendorCompName like '" + txt_inst_name.Text + "%'");
            vendortype = "5";
            companyname = Convert.ToString(txt_inst_name.Text);
        }
        if (rdbinst.Checked == true || rdbdept.Checked == true || rdbsponser.Checked == true || rdbcompany.Checked == true)
        {
            if (VenCode != "" && VenCode != null && VenCode != "0")
            {
                VendorCode = VenCode;
            }
            else
            {
                VendorCodeGen();
                VendorCode = Session["VendorCode"].ToString();
                string venmst = "if exists (select * from CO_VendorMaster where VendorCode='" + VendorCode + "' and VendorCompName='" + companyname + "' and VendorType='" + vendortype + "') update CO_VendorMaster set VendorCode='" + VendorCode + "', VendorCompName='" + companyname + "',VendorType='" + vendortype + "' where  VendorCode='" + VendorCode + "' and  VendorCompName='" + companyname + "' and VendorType='" + vendortype + "' else insert into CO_VendorMaster(VendorCode,VendorCompName,VendorType) values('" + VendorCode + "','" + companyname + "','" + vendortype + "')";
                int vc = d2.update_method_wo_parameter(venmst, "TEXT");
            }
            Int64 VendorFK = Convert.ToInt64(d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + VendorCode + "'"));
            string vencmst = "if exists(select * from IM_VendorContactMaster where VendorFK='" + VendorFK + "') update IM_VendorContactMaster set VendorFK='" + VendorFK + "' where VendorFK='" + VendorFK + "' else insert into IM_VendorContactMaster(VendorFK,VendorPhoneNo) values('" + VendorFK + "','" + companyname + "')";
            int vcm = d2.update_method_wo_parameter(vencmst, "TEXT");
        }

    }

    protected void OnRowDeleting_gridadd(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        if (index != 0)
        {
            DataTable dt = ViewState["gridadd"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["gridadd"] = dt;
            gridadd.DataSource = dt;
            gridadd.DataBind();
        }
        else
        {
            gridadd.Visible = false;
            gridadd.DataSource = null;
            gridadd.DataBind();
            ViewState["gridadd"] = null;
        }
    }
    public void ddl_actname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_actname.SelectedItem.Text != "Select")
        {
            txtothers.Text = Convert.ToString(ddl_actname.SelectedItem.Text);
        }
        else
        {
            txtothers.Text = "";
        }
    }
    public void indi_comp()
    {
        string cname = "";
        string pname = "";

        for (int i = 0; i < GridView9.Rows.Count; i++)
        {
            TextBox txtamt = (TextBox)GridView9.Rows[i].FindControl("txtactname");
            cname = Convert.ToString(txtamt.Text);
            TextBox txtpanam = (TextBox)GridView9.Rows[i].FindControl("txt_per");
            pname = Convert.ToString(txtpanam.Text);

            particpentindi = pname;

        }
        for (int i = 0; i < GridView8.Rows.Count; i++)
        {
            TextBox txtamt = (TextBox)GridView8.Rows[i].FindControl("txtactname");
            cname = Convert.ToString(txtamt.Text);
            TextBox txtpanam = (TextBox)GridView8.Rows[i].FindControl("txt_per");
            pname = Convert.ToString(txtpanam.Text);

            particpentcomp = cname;
        }
        for (int i = 0; i < GridView10.Rows.Count; i++)
        {
            TextBox txtamtindi = (TextBox)GridView10.Rows[i].FindControl("txtactname");
            cname = Convert.ToString(txtamtindi.Text);
            TextBox txtpanam = (TextBox)GridView10.Rows[i].FindControl("txt_per");
            pname = Convert.ToString(txtpanam.Text);
            persentedindi = pname;
        }
        for (int i = 0; i < GridView11.Rows.Count; i++)
        {
            TextBox txtamt = (TextBox)GridView11.Rows[i].FindControl("txtactname");
            cname = Convert.ToString(txtamt.Text);
            TextBox txtpanam = (TextBox)GridView11.Rows[i].FindControl("txt_per");
            pname = Convert.ToString(txtpanam.Text);
            persentedcomp = cname;
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaffname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name+'-'+ s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        // string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";


        name = ws.Getname(query);

        return name;
    }

    public void rdo_orgstudent_Checkedchange(object sender, EventArgs e)
    {
        UpdatePanel1.Visible = true;
        lbl_orgstudentname.Visible = true;
        txt_studenorgsearch.Visible = true;
        lbl_org_studnamee.Visible = true;
        lb_org_staffname.Visible = false;
        txtissueper.Visible = false;
        UpdatePanel25.Visible = false;
        lblissueperson.Visible = false;
        ddl_org_batch.Enabled = true;
        txt_degree.Enabled = true;
        txt_org_sem.Enabled = true;
    }
    public void rdo_org_staff_Checkedchange(object sender, EventArgs e)
    {
        txtissueper.Visible = true;
        UpdatePanel25.Visible = true;
        lblissueperson.Visible = true;
        lb_org_staffname.Visible = true;
        lbl_org_studnamee.Visible = false;
        UpdatePanel1.Visible = false;
        lbl_orgstudentname.Visible = false;
        txt_studenorgsearch.Visible = false;
        ddl_org_batch.Enabled = false;
        txt_degree.Enabled = false;
        txt_org_sem.Enabled = false;
       string branch=string.Empty;
       bindbranch(branch);
    }
    public void cb_studentorgby_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_studentorgby.Text = "--Select--";
            if (cb_studentorgby.Checked == true)
            {
                cout++;
                for (int i = 0; i < cb1_studentorgby.Items.Count; i++)
                {
                    cb1_studentorgby.Items[i].Selected = true;
                }
                txt_studentorgby.Text = "Student(" + (cb1_studentorgby.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cb1_studentorgby.Items.Count; i++)
                {
                    cb1_studentorgby.Items[i].Selected = false;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void cb1_studentorgby_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_studentorgby.Checked = false;
        int commcount = 0;
        txt_studentorgby.Text = "--Select--";

        for (int i = 0; i < cb1_studentorgby.Items.Count; i++)
        {
            if (cb1_studentorgby.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_studentorgby.Checked = false;

            }
        }
        if (commcount > 0)
        {
            if (commcount == cb1_studentorgby.Items.Count)
            {

                cb_studentorgby.Checked = true;
            }
            txt_studentorgby.Text = "Student(" + commcount.ToString() + ")";

        }


    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    public void bindstudentname()
    {
        string branch = "";
        string dept = "";
        string sem = "";
        string batch_year = Convert.ToString(ddl_org_batch.SelectedItem.Value);
        for (int i = 0; i < cbl_branch.Items.Count; i++)
        {
            if (cbl_branch.Items[i].Selected == true)
            {
                if (branch == "")
                {
                    branch = "" + cbl_branch.Items[i].Value.ToString() + "";
                }
                else
                {
                    branch = branch + "'" + "," + "" + "'" + cbl_branch.Items[i].Value.ToString() + "";
                }
            }
        }
        for (int i = 0; i < cbl_branch.Items.Count; i++)
        {
            if (cbl_branch.Items[i].Selected == true)
            {
                if (branch == "")
                {
                    branch = "" + cbl_branch.Items[i].Value.ToString() + "";
                }
                else
                {
                    branch = branch + "'" + "," + "" + "'" + cbl_branch.Items[i].Value.ToString() + "";
                }
            }
        }
        for (int i = 0; i < cbl_or_sem.Items.Count; i++)
        {
            if (cbl_or_sem.Items[i].Selected == true)
            {
                if (sem == "")
                {
                    sem = "" + cbl_or_sem.Items[i].Value.ToString() + "";
                }
                else
                {
                    sem = sem + "'" + "," + "" + "'" + cbl_or_sem.Items[i].Value.ToString() + "";
                }
            }
        }
        string selectquery = "select Roll_No,Stud_Name from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year='" + batch_year + "' ";
        if (branch.Trim() != "")
        {
            selectquery = selectquery + " and r.degree_code in('" + branch + "')";
        }
        if (sem.Trim() != "")
        {
            selectquery = selectquery + " and r.Current_Semester in('" + sem + "') ";
        }
        ds.Clear();
        ds = da.select_method_wo_parameter(selectquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cb1_studentorgby.DataSource = ds;
            cb1_studentorgby.DataTextField = "Stud_Name";
            cb1_studentorgby.DataValueField = "Roll_No";
            cb1_studentorgby.DataBind();
            //cb_staff_name.Checked = true;
            //if (cbl_staff_name.Items.Count > 0)
            //{
            //    for (int i = 0; i < cbl_staff_name.Items.Count; i++)
            //    {
            //        cbl_staff_name.Items[i].Selected = true;
            //    }
            //    txt_staffnamemul.Text = "Staff Name(" + cbl_staff_name.Items.Count + ")";
            //}
        }
    }
    private void MergeCells()
    {
        //int i = GridView1.Rows.Count - 2;
        //while (i >= 0)
        //{
        //    GridViewRow curRow = GridView1.Rows[i];
        //    GridViewRow preRow = GridView1.Rows[i + 1];
        //    Label txtsttime = (Label)GridView1.Rows[i].FindControl("lbl_dept");
        //    string dept = Convert.ToString(txtsttime.Text);

        //    Label pre = (Label)GridView1.Rows[i+1].FindControl("lbl_dept");
        //    string dept1 = Convert.ToString(pre.Text);

        //    int j = 0;
        //    while (j < curRow.Cells.Count)
        //    {
        //        if (dept == dept1)
        //        {

        //            curRow.Cells[j].RowSpan = 2;
        //            preRow.Cells[j].Visible = false;

        //            curRow.Cells[j].RowSpan = preRow.Cells[j].RowSpan + 1;
        //            preRow.Cells[j].Visible = false;

        //        }
        //        j++;
        //    }
        //    i--;
        //}
    }
    protected void GridView1_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            Label lbldept = (Label)GridView1.Rows[0].Cells[2].FindControl("lbl_dept");
            string curName = lbldept.Text;
            string temName = curName;
            int curindx = 0;
            int curLength = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                Label lbldept1 = (Label)GridView1.Rows[i].Cells[2].FindControl("lbl_dept");
                temName = lbldept1.Text;
                if (temName == curName)
                {
                    if (curLength != 0)
                    {
                        GridView1.Rows[i].Cells[2].Visible = false;
                        if (i == GridView1.Rows.Count - 1)
                        {
                            GridView1.Rows[curindx].Cells[2].RowSpan = curLength + 1;
                        }
                    }
                    curLength++;
                }
                else
                {
                    curName = temName;
                    GridView1.Rows[curindx].Cells[2].RowSpan = curLength;
                    curindx = curindx + curLength - 1;
                    curLength = 0;
                }
            }
        }
        catch
        {

        }
    }
    protected void GridView13_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            Label lbldept = (Label)GridView13.Rows[0].Cells[2].FindControl("lbldept");
            string curName = lbldept.Text;
            string temName = curName;
            int curindx = 0;
            int curLength = 0;
            for (int i = 0; i < GridView13.Rows.Count; i++)
            {
                Label lbldept1 = (Label)GridView13.Rows[i].Cells[2].FindControl("lbldept");
                temName = lbldept1.Text;
                if (temName == curName)
                {
                    if (curLength != 0)
                    {
                        GridView13.Rows[i].Cells[2].Visible = false;
                        if (i == GridView13.Rows.Count - 1)
                        {
                            GridView13.Rows[curindx].Cells[2].RowSpan = curLength + 1;
                        }
                    }
                    curLength++;
                }
                else
                {
                    curName = temName;
                    GridView13.Rows[curindx].Cells[2].RowSpan = curLength;
                    curindx = curindx + curLength - 1;
                    curLength = 0;
                }
            }
        }
        catch
        {

        }
    }

}