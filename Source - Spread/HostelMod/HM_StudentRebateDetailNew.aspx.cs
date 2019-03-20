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
using System.Globalization;
using System.Threading;
public partial class HM_StudentRebateDetailNew : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    static DAccess2 daobj;
    static DataSet dsobj;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    public static string statichostelname;
    public static string lochos = "";
    bool check = false;
    string courseid;
    string loccourseid = "";
    string rollno = string.Empty;
    string staffapp = string.Empty;
    string StudentName = string.Empty;
    string hostelname = string.Empty;
    string selectrollno = string.Empty;
    string selectapp = string.Empty;
    string selectStudentName = string.Empty;
    string selecthostelname = string.Empty;
    string date = string.Empty;
    static string rdbchk = string.Empty;
    //static string rdbchk = string.Empty;
    //magesh 16.3.18
    bool flag_true = false;
    int studCount = 0;
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
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lbl_validation.Visible = false;
        txt_fromdate1.Attributes.Add("readonly", "true");
        txt_todate1.Attributes.Add("readonly", "true");
        lbl_validation.Text = "";
        if (rdb_student.Checked == true)
            rdbchk = "Student";
        if (!IsPostBack)
        {
            ViewState["NoOfStudents"] = null;
            ViewState["NoOfapp"] = null;
            bindhostelname();
            bindhostelname1();
            binddegree();
            bindbatch();
            //loaddesc();
            //bindbranch();
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            // lbl_errrepor.Visible = false;
            txt_fromdate.Enabled = false;
            txt_todate.Enabled = false;
            //ddl_batch2.Enabled = false;
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            // btngo_Click(sender, e);
        }
        btn_save.Visible = false;
        Button1.Visible = false;
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    //main page
    public void bindhostelname()
    {
        try
        {
            cbl_hostelname.Items.Clear();
            //string selecthostel = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelName";//where CollegeCode='" + collegecode1 + "' 
            //ds = d2.select_method_wo_parameter(selecthostel, "Text");
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
            }
            else
            {
                cbl_hostelname.Items.Insert(0, "Select");
            }
            if (cbl_hostelname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = true;
                    cb_hostelname.Checked = true;
                }
                txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
            }
        }
        catch
        {
        }
    }
    protected void chkhstlname_checkedchange(object sender, EventArgs e)
    {
        try
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
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void chklsthstlname_Change(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            // txt_hostelname.Text = "--Select--";
            cb_hostelname.Checked = false;
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
        }
        catch (Exception ex)
        {
        }
    }
    protected void chkfrdate_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_fromdate.Checked == true)
        {
            txt_fromdate.Enabled = true;
            txt_todate.Enabled = true;
        }
        if (cb_fromdate.Checked == false)
        {
            txt_fromdate.Enabled = false;
            txt_todate.Enabled = false;
        }
    }
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                //txt_leavedays.Text = "";
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Enter FromDate less than or equal to the ToDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = "";
                    //txt_rebatedays.Text = "";
                }
                else
                {
                }
            }
        }
        catch
        {
        }
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                //txt_leavedays.Text = "";
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Enter ToDate greater than or equal to the FromDate ";
                    txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = "";
                    //txt_rebatedays.Text = "";
                }
                else
                {
                    //txt_leavedays.Text = Convert.ToString(days);
                    //txt_rebatedays.Text = Convert.ToString(days);
                }
                if (studCount > 1)
                {
                    ViewState["NoOfStudents"] = selectrollno;
                  
                    txt_hostelname1.Text = selecthostelname;
                    txt_rollno.Text = "Selected Student(" + studCount + ")";
                    txt_rollno.Visible = false;
                    txt_fromdate1.Visible = false;
                    txt_todate1.Visible = false;
                }
                else
                {
                    txt_rollno.Visible = true;
                    txt_fromdate1.Visible = true;
                    txt_todate1.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter ToDate greater than or equal to the FromDate", cv_fromtodt2);
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string hostelname, lochostelname = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    if (lochostelname == "")
                    {
                        lochostelname = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        lochostelname = lochostelname + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                    }
                }
            }
            Printcontrol.Visible = false;
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string selectqurey = "";
            string desc = "";
            if (lochostelname != "")
            {
                if (ddl_rebatetype.SelectedItem.Text == "Rebate Days")
                {
                    if (Rdbst.Checked==true)
                        //selectqurey = "select distinct h.App_No,r.Stud_Name,d.Degree_Code,dt.Dept_Name, c.Course_Name,RebateType,CONVERT(varchar(10),RebateFromDate,103) as RebateFromDate,CONVERT(varchar(10),RebateToDate,103) as RebateToDate,LeaveDays,RebateDays, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description ,RebateDetailPK,hm.HostelMasterPK,hm.HostelName,r.roll_no,hr.id  from HT_HostelRebateDetail h,Registration r,Degree d,Department dt,Course c,HM_HostelMaster hm,HT_HostelRegistration hr where h.App_No =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id   and hr.APP_No =h.App_No and hr.HostelMasterFK =hm.HostelMasterPK and  h.RebateType='1' and h.MemType='1' ";
                        selectqurey = "select distinct h.App_No,r.Stud_Name,d.Degree_Code,dt.Dept_Name, c.Course_Name,RebateType,CONVERT(varchar(10),RebateFromDate,103) as RebateFromDate,CONVERT(varchar(10),RebateToDate,103) as RebateToDate,LeaveDays,RebateDays, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description ,RebateDetailPK,hm.HostelMasterPK,hm.HostelName,r.roll_no,isnull(hr.id,'') as id,hr.StudMessType  from HT_HostelRebateDetail h,Registration r,Degree d,Department dt,Course c,HM_HostelMaster hm,HT_HostelRegistration hr where h.App_No =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id   and hr.APP_No =h.App_No and hr.HostelMasterFK =hm.HostelMasterPK and  h.RebateType='1' and h.MemType='1'  and hr.HostelMasterFK in('" + lochostelname + "') ";//modified by rajasekar 18/07/2018
                    if (rdbsta.Checked == true)
                        //selectqurey = "select  hsd.APP_No,sm.staff_code as Roll_No,sm.staff_name as Stud_Name,dm.desig_name,h.dept_name as Course_Name,dm.staffcategory,hsd.id,RebateType,CONVERT(varchar(10),RebateFromDate,103) as RebateFromDate,CONVERT(varchar(10),RebateToDate,103) as RebateToDate,LeaveDays,RebateDays, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description ,RebateDetailPK,hd.HostelMasterPK ,hd.HostelName from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st,HT_HostelRebateDetail hsr  where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsr.MemType=2 and dm.collegeCode=sm.college_code    and latestrec=1 and ISNULL(IsVacated,'0')=0 and  hsd.APP_No =hsr.App_No and  hsr.RebateType='1'  ";
                        selectqurey = "select  hsd.APP_No,sm.staff_code as Roll_No,sm.staff_name as Stud_Name,dm.desig_name, RebateAmount as Rebate_Amount,hdm.dept_name as Course_Name,dm.staffcategory,isnull(hsd.id,'') as id,RebateType,CONVERT(varchar(10),RebateFromDate,103) as RebateFromDate,CONVERT(varchar(10),RebateToDate,103) as RebateToDate,LeaveDays,RebateDays, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description ,RebateDetailPK,hd.HostelMasterPK ,hd.HostelName,hsd.StudMessType from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master hdm,staff_appl_master a,stafftrans st,HT_HostelRebateDetail h  where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and hdm.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and h.MemType=2 and dm.collegeCode=sm.college_code    and latestrec=1 and ISNULL(IsVacated,'0')=0 and  hsd.APP_No =h.App_No and  h.RebateType='1' and hsd.HostelMasterFK in('" + lochostelname + "') ";//modified by rajasekar 18/07/2018
                    if (rdbgue.Checked == true)
                        //selectqurey = "select im.VendorContactPK as Roll_No,VenContactName as Stud_Name,h.App_No,gr.id,VendorCompName,RebateType,CONVERT(varchar(10),RebateFromDate,103) as RebateFromDate,CONVERT(varchar(10),RebateToDate,103) as RebateToDate,RebateDays,RebateDetailPK, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description,LeaveDays,hd.HostelMasterPK ,hd.HostelName from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd,HT_HostelRebateDetail h where co.VendorPK=im.VendorFK and h.MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK  and isnull(IsVacated,'0')='0' and gr.APP_No =h.App_No and  h.RebateType='1' ";
                        selectqurey = "select im.VendorContactPK as Roll_No,VenContactName as Stud_Name,h.App_No,isnull(gr.id,'') as id,VendorCompName,RebateType,CONVERT(varchar(10),RebateFromDate,103) as RebateFromDate,CONVERT(varchar(10),RebateToDate,103) as RebateToDate,RebateDays,RebateDetailPK, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description,LeaveDays,hd.HostelMasterPK ,hd.HostelName,gr.StudMessType from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd,HT_HostelRebateDetail h where co.VendorPK=im.VendorFK and h.MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK  and isnull(IsVacated,'0')='0' and gr.APP_No =h.App_No and  h.RebateType='1' and gr.HostelMasterFK in('" + lochostelname + "')";//modified by rajasekar 18/07/2018
                    if (cb_fromdate.Checked == true)
                    {
                        //selectqurey = selectqurey + " and ('" + dt.ToString("MM/dd/yyyy")  + "' between h.RebateFromDate and h.RebateFromDate or '" + dt1.ToString("MM/dd/yyyy") + "' between h.RebateToDate and h.RebateToDate)";
                        selectqurey += " and ('" + dt.ToString("MM/dd/yyyy") + "'<=h.RebateFromDate and h.RebateToDate<='" + dt1.ToString("MM/dd/yyyy") + "')";
                    }
                    //selectqurey = "select r.Roll_No,srd.Roll_Admit,r.Stud_Name,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_Name,convert(varchar,convert(date,srd.From_Date,103),103) as 'From_Date',convert(varchar,convert(date,srd.To_Date,103),103) as 'To_Date',srd.Leave_Days,srd.Rebate_Days,convert(varchar,convert(date,srd.Rebate_Date,103),103) as 'Rebate_Date',srd.Rebate_Amount,srd.Desc_Code from StudentRebate_Details as srd join Hostel_Details as hd on srd.Hostel_Code=hd.Hostel_code join Registration as r on srd.Roll_No=r.Roll_No  join Degree as d on r.degree_code=d.Degree_Code join Course as c on d.Course_Id=c.Course_Id join Department as dt on d.Dept_Code=dt.Dept_Code where srd.Hostel_Code in('" + lochostelname + "') and srd.Rebate_Type='1'";
                    //if (cb_fromdate.Checked == true)
                    //{
                    //    selectqurey = selectqurey + "  and ('" + dt + "' between srd.RebateFromDate and srd.RebateToDate or '" + dt1 + "' between srd.RebateFromDate and srd.RebateToDate) ";
                    //}
                }
                else if (ddl_rebatetype.SelectedItem.Text == "Rebate Amount")
                {
                    //selectqurey = "select r.Roll_No,srd.Roll_Admit,r.Stud_Name,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_Name,convert(varchar,convert(date,srd.From_Date,103),103) as 'From_Date',convert(varchar,convert(date,srd.To_Date,103),103) as 'To_Date',srd.Leave_Days,srd.Rebate_Days,convert(varchar,convert(date,srd.Rebate_Date,103),103) as 'Rebate_Date',srd.Rebate_Amount,srd.Desc_Code from StudentRebate_Details as srd join Hostel_Details as hd on srd.Hostel_Code=hd.Hostel_code join Registration as r on srd.Roll_Admit=r.Roll_Admit join Degree as d on r.degree_code=d.Degree_Code join Course as c on d.Course_Id=c.Course_Id join Department as dt on d.Dept_Code=dt.Dept_Code where srd.Hostel_Code in('" + lochostelname + "') and srd.Rebate_Type='0' and srd.Rebate_Date between '" + dt + "' and '" + dt1 + "'";
                    if (Rdbst.Checked == true)
                        //selectqurey = "select distinct h.App_No,hr.id, r.Stud_Name,d.Degree_Code,dt.Dept_Name, c.Course_Name,RebateType, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description ,RebateDetailPK,hm.HostelMasterPK,hm.HostelName,RebateAmount as Rebate_Amount,r.roll_no from HT_HostelRebateDetail h,Registration r,Degree d,Department dt,Course c,HM_HostelMaster hm,HT_HostelRegistration hr where h.App_No =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id   and hr.APP_No =h.App_No and hr.HostelMasterFK =hm.HostelMasterPK and  h.RebateType='2' and hr.HostelMasterFK in('" + lochostelname + "') and h.MemType='1'";
                        selectqurey = "select distinct h.App_No,isnull(hr.id,'') as id, r.Stud_Name,d.Degree_Code,dt.Dept_Name, c.Course_Name,RebateType, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description ,RebateDetailPK,hm.HostelMasterPK,hm.HostelName,RebateAmount as Rebate_Amount,r.roll_no,hr.StudMessType from HT_HostelRebateDetail h,Registration r,Degree d,Department dt,Course c,HM_HostelMaster hm,HT_HostelRegistration hr where h.App_No =r.App_No and d.Degree_Code =r.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id   and hr.APP_No =h.App_No and hr.HostelMasterFK =hm.HostelMasterPK and  h.RebateType='2' and hr.HostelMasterFK in('" + lochostelname + "') and h.MemType='1'";//modified by rajasekar 18/07/2018
                    if (rdbsta.Checked == true)
                        //selectqurey = "select  hsd.APP_No,sm.staff_code as Roll_No,sm.staff_name as Stud_Name,dm.desig_name, RebateAmount as Rebate_Amount,h.dept_name as Course_Name,dm.staffcategory,hsd.id,RebateType,CONVERT(varchar(10),RebateFromDate,103) as RebateFromDate,CONVERT(varchar(10),RebateToDate,103) as RebateToDate,LeaveDays,RebateDays, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description ,RebateDetailPK,hd.HostelMasterPK ,hd.HostelName from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st,HT_HostelRebateDetail hsr  where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsr.MemType=2 and dm.collegeCode=sm.college_code    and latestrec=1 and ISNULL(IsVacated,'0')=0 and  hsd.APP_No =hsr.App_No and  hsr.RebateType='2'  ";
                        selectqurey = "seselect  hsd.APP_No,sm.staff_code as Roll_No,sm.staff_name as Stud_Name,dm.desig_name, RebateAmount as Rebate_Amount,hdm.dept_name as Course_Name,dm.staffcategory,isnull(hsd.id,'') as id,RebateType,CONVERT(varchar(10),RebateFromDate,103) as RebateFromDate,CONVERT(varchar(10),RebateToDate,103) as RebateToDate,LeaveDays,RebateDays, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description ,RebateDetailPK,hd.HostelMasterPK ,hd.HostelName,hsd.StudMessType from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master hdm,staff_appl_master a,stafftrans st,HT_HostelRebateDetail h  where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and hdm.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and h.MemType=2 and dm.collegeCode=sm.college_code    and latestrec=1 and ISNULL(IsVacated,'0')=0 and  hsd.APP_No =h.App_No and  h.RebateType='2' and  hsd.HostelMasterFK in('" + lochostelname + "') ";//modified by rajasekar 18/07/2018
                    if (rdbgue.Checked == true)
                        //selectqurey = "select im.VendorContactPK as Roll_No, VenContactName as Stud_Name,h.App_No,gr.id,VendorCompName,RebateType,CONVERT(varchar(10),RebateFromDate,103) as RebateFromDate,CONVERT(varchar(10),RebateToDate,103) as RebateToDate,RebateDays, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description,LeaveDays,RebateDetailPK,hd.HostelMasterPK ,hd.HostelName,RebateAmount as Rebate_Amount from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd,HT_HostelRebateDetail h where co.VendorPK=im.VendorFK and h.MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK  and isnull(IsVacated,'0')='0' and gr.APP_No =h.App_No and  h.RebateType='2' ";
                        selectqurey = "select im.VendorContactPK as Roll_No, VenContactName as Stud_Name,h.App_No,isnull(gr.id,'') as id,VendorCompName,RebateType,CONVERT(varchar(10),RebateFromDate,103) as RebateFromDate,CONVERT(varchar(10),RebateToDate,103) as RebateToDate,RebateDays, (select MasterValue from CO_MasterValues where MasterCode = RebateDesc) as Description,LeaveDays,RebateDetailPK,hd.HostelMasterPK ,hd.HostelName,RebateAmount as Rebate_Amount,gr.StudMessType from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd,HT_HostelRebateDetail h where co.VendorPK=im.VendorFK and h.MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK  and isnull(IsVacated,'0')='0' and gr.APP_No =h.App_No and  h.RebateType='2' and  gr.HostelMasterFK in('" + lochostelname + "')  ";//modified by rajasekar 18/07/2018
                    if (cb_fromdate.Checked == true)
                    {
                        selectqurey += " and ('" + dt.ToString("MM/dd/yyyy") + "'<=h.RebateFromDate and h.RebateToDate<='" + dt1.ToString("MM/dd/yyyy") + "')";
                        //selectqurey = selectqurey + " and ('" + dt + "' between h.RebateFromDate and h.RebateToDate or '" + dt1 + "' between h.RebateFromDate and h.RebateToDate)";
                    }
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectqurey, "Text");
            }
            else
            {
                Divspread.Visible = false;
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_errrepor.Visible = true;
                lbl_errrepor.Text = "Please Select Hostel Name";
            }
            int rolcount = 0;
            int sno = 0;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.SaveChanges();
            Fpspread1.SheetCorner.ColumnCount = 0;
            Fpspread1.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
            Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 3);
            Fpspread1.Sheets[0].AutoPostBack = true;
            ds = d2.select_method_wo_parameter(selectqurey, "Text");
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 14;
            if (ddl_rebatetype.SelectedItem.Text == "Rebate Amount")
            {
                Fpspread1.Sheets[0].Columns[6].Visible = false;
                Fpspread1.Sheets[0].Columns[9].Visible = true;
                Fpspread1.Sheets[0].Columns[7].Visible = false;
                Fpspread1.Sheets[0].Columns[8].Visible = false;
                Fpspread1.Sheets[0].Columns[10].Visible = false;
                Fpspread1.Sheets[0].Columns[11].Visible = false;
            }
            else if (ddl_rebatetype.SelectedItem.Text == "Rebate Days")
            {
                Fpspread1.Sheets[0].Columns[6].Visible = false;
                Fpspread1.Sheets[0].Columns[9].Visible = false;
                Fpspread1.Sheets[0].Columns[7].Visible = true;
                Fpspread1.Sheets[0].Columns[8].Visible = true;
                Fpspread1.Sheets[0].Columns[10].Visible = true;
                Fpspread1.Sheets[0].Columns[11].Visible = true;
                Fpspread1.Sheets[0].Columns[12].Visible = true;
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                Divspread.Visible = true;
                Fpspread1.Visible = true;
                rptprint.Visible = true;
                // btn_save2.Visible = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[0].Locked = true;
                Fpspread1.Columns[0].Width = 80;
                if (Rdbst.Checked == true)
                {
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[1].Locked = true;
                Fpspread1.Columns[1].Width = 100;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Id";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[2].Locked = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[3].Locked = true;
                Fpspread1.Columns[3].Width = 480;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[4].Locked = true;
                Fpspread1.Columns[4].Width = 250;
            }
                else if (rdbsta.Checked == true)
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[1].Locked = true;
                    Fpspread1.Columns[1].Width = 100;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Id";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[2].Locked = true;
                    Fpspread1.Columns[2].Width = 100;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[3].Locked = true;
                    Fpspread1.Columns[3].Width = 680;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[4].Locked = true;
                    Fpspread1.Columns[4].Width = 450;
                }
                else
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Guest Code";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[1].Locked = true;
                    Fpspread1.Columns[1].Width = 100;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Guest Id";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[2].Locked = true;
                    Fpspread1.Columns[2].Width = 100;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Guest Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[3].Locked = true;
                    Fpspread1.Columns[3].Width = 680;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Columns[4].Locked = true;
                    Fpspread1.Columns[4].Width = 450;
                    Fpspread1.Columns[4].Visible = false;
                }
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Hostel Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread1.Columns[5].Width = 250;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[5].Locked = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Rebate Date";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread1.Columns[6].Width = 200;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[6].Locked = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "From Date";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                Fpspread1.Columns[7].Width = 200;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[7].Locked = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "To Date";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                Fpspread1.Columns[8].Width = 200;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[8].Locked = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Rebate Amount";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                Fpspread1.Columns[9].Width = 200;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[9].Locked = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Leave Days";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                Fpspread1.Columns[10].Width = 200;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[10].Locked = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Rebate Days";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                Fpspread1.Columns[11].Width = 200;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[11].Locked = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Description";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
                Fpspread1.Columns[12].Width = 200;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[12].Locked = true;
                //added by rajasekar 18/07/2018
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Student Type";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Bold = true;
                Fpspread1.Columns[13].Width = 200;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 13].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 13].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[13].Locked = true;
                //*************************//
                Fpspread1.Width = 700;
               
                for (rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    // Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["App_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["roll_no"]);
                    //roll_no
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["RebateDetailPK"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["id"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["Stud_Name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    if (rdbgue.Checked == false)
                    {
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["Course_Name"]);
                        if (Rdbst.Checked == true)
                        {
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["Degree_Code"]);
                        }
                    }
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["HostelName"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    //  Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["Rebate_Date"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                  
                    if (ddl_rebatetype.SelectedItem.Text == "Rebate Amount")
                    {
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["Rebate_Amount"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                    }
                    else
                    {
                        
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["Rebate_Amount"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["RebateFromDate"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["RebateToDate"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["LeaveDays"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["RebateDays"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                    }
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["Description"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";
                    //added by rajasekar 18/07/2018
                    if (Convert.ToString(ds.Tables[0].Rows[rolcount]["StudMessType"]) == "1")
                    {
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].Text = "Non Veg";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";
                    }
                    else if (Convert.ToString(ds.Tables[0].Rows[rolcount]["StudMessType"]) == "0")
                    {
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].Text = "Veg";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";
                    }
                    //*********************//
                }
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.SaveChanges();
                lbl_errrepor.Visible = false;
            }
            else
            {
                Divspread.Visible = false;
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_errrepor.Visible = true;
                lbl_errrepor.Text = "No Records found";
                //imgdiv2.Visible = true;
                //lbl_erroralert.Text = "No records found";
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void btnaddnew_Click(object sender, EventArgs e)
    {
        txt_rollno.Enabled = true;
        btn_question.Enabled = true;
        popwindow1.Visible = true;
        btn_save1.Visible = true;
        btn_exit1.Visible = true;
        btn_delete.Visible = false;
        btn_update.Visible = false;
        btn_exit_fp.Visible = false;
        txt_degree.Enabled = true;
        lbl_degree.Enabled = true;
        lbl_rollno.Visible = true;
        txt_rollno.Visible = true;
        lbl_name.Enabled = true;
        txt_name.Enabled = true;
        txt_rebatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        clearpopup();
        loaddesc();
        if (rdb_days.Checked == true)
        {
            txt_rebatedate.Enabled = false;
            txt_fromdate1.Enabled = true;
            txt_todate1.Enabled = true;
            txt_rebateamt.Enabled = false;
            txt_rebatedays.Enabled = true;
            txt_leavedays.Text = "";
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string firstdate = Convert.ToString(txt_fromdate1.Text);
            string seconddate = Convert.ToString(txt_todate1.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            TimeSpan ts = dt1 - dt;
            int days = ts.Days + 1;
            txt_leavedays.Text = Convert.ToString(days);
            txt_rebatedays.Text = Convert.ToString(days);
        }
        if (rdb_rebateamt.Checked == true)
        {
            txt_rebatedate.Enabled = true;
            txt_fromdate1.Enabled = false;
            txt_todate1.Enabled = false;
            txt_rebateamt.Enabled = true;
            txt_rebatedays.Enabled = false;
            txt_leavedays.Text = "0";
            txt_rebatedays.Text = "0";
        }
    }
    protected void Cell_Click1(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {
        }
    }
    protected void Fpspread_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                popwindow1.Visible = true;
                btn_save1.Visible = false;
                btn_exit1.Visible = false;
                btn_update.Visible = true;
                btn_delete.Visible = true;
                btn_exit_fp.Visible = true;
                string activerow = "";
                string activecol = "";
                if (Rdbst.Checked == true)
                {
                    rdb_student.Checked = true;
                    rdb_staff.Checked = false;
                    rdb_guest.Checked = false;
                }
                if (rdbsta.Checked == true)
                {
                    rdb_staff.Checked = true;
                    rdb_guest.Checked = false;
                    rdb_student.Checked = false;
                }
                if (rdbgue.Checked == true)
                {
                    rdb_guest.Checked = true;
                    rdb_staff.Checked = false;
                    rdb_student.Checked = false;
                }
                rdb_guest_CheckedChange(sender, e);
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                if (activerow.Trim() != "")
                {
                    string Roll_No = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                    string stuid = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                     string rollnum =string.Empty;
                    if (Rdbst.Checked == true)
                        rollnum = d2.GetFunction("select Roll_No from Registration where app_no='" + Roll_No + "'");
                    else if (rdbgue.Checked == true)
                        rollnum = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    else
                    {
                        rollnum = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                        Label4.Text = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);

                    }
                    string Rebetpk = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    Session["Rebetpk"] = Convert.ToString(Rebetpk);
                    string studname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    string degree = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    string hosname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                    // string hoscode = d2.GetFunction("select Hostel_Code from Hostel_Details where Hostel_Name='" + hosname + "'");
                    string rebatedate = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                    string fromdate = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text);
                    string todate = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text);
                    string rebateamount = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
                    string leavedays = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Text);
                    string rebatedays = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Text);
                    string description = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text);
                    string rebatetype = ddl_rebatetype.SelectedItem.Value.ToString();
                    if (rebatetype == "0")
                    {
                        rdb_rebateamt.Checked = true;
                        rdb_days.Checked = false;
                        rdb_days.Enabled = true;
                        rdb_rebateamt.Enabled = true;
                        txt_rebatedate.Enabled = false;
                        txt_rebatedays.Enabled = false;
                        txt_fromdate1.Enabled = false;
                        txt_todate1.Enabled = false;
                        txt_leavedays.Enabled = false;
                        txt_rebateamt.Enabled = true;
                    }
                    else if (rebatetype == "1")
                    {
                        rdb_rebateamt.Checked = false;
                        rdb_days.Checked = true;
                        rdb_rebateamt.Enabled = true;
                        rdb_days.Enabled = true;
                        txt_rebatedate.Enabled = false;
                        txt_rebatedays.Enabled = true;
                        txt_fromdate1.Enabled = true;
                        txt_todate1.Enabled = true;
                        txt_leavedays.Enabled = false;
                        txt_rebateamt.Enabled = false;
                    }
                    txt_rollno.Text = Convert.ToString(rollnum);
                    Txtid.Text = stuid;
                    txt_name.Text = Convert.ToString(studname);
                    txt_degree.Text = Convert.ToString(degree);
                    txt_hostelname1.Text = Convert.ToString(hosname);
                    txt_rebatedate.Text = Convert.ToString(rebatedate);
                    txt_fromdate1.Text = Convert.ToString(fromdate);
                    txt_todate1.Text = Convert.ToString(todate);
                    txt_rebateamt.Text = Convert.ToString(rebateamount);
                    txt_leavedays.Text = Convert.ToString(leavedays);
                    txt_rebatedays.Text = Convert.ToString(rebatedays);
                    loaddesc();
                    //  string desccode   = d2.GetFunction("select TextCode from TextValTable where TextVal='" + Convert.ToString(description) + "'");
                    //ddl_description.SelectedItem.Value=desccode;
                    ddl_description.SelectedIndex = ddl_description.Items.IndexOf(ddl_description.Items.FindByText(description));
                    txt_rollno.Enabled = false;
                    Txtid.Enabled = false;
                    btn_question.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(Fpspread1, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please enter the report name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Student Rebate Details Report";
            string pagename = "HM_StudentRebateDetailNew.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
    }
    // popupwindow1
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //string query = "select distinct top 10 r.Roll_No from Registration as r join Hostel_StudentDetails as hs on r.Roll_No=hs.Roll_No join Hostel_Details as hd on hs.Hostel_Code=hd.Hostel_Code where r.Delflag=0 and r.cc=0 and r.roll_no like '" + prefixText + "%' order by r.Roll_No desc";
         string query=string.Empty;
        if(rdbchk=="Student")
         query = "select distinct top 10 r.Roll_No from Registration as r join HT_HostelRegistration as hs on r.app_no=hs.APP_No join HM_HostelMaster  as hd on hs.HostelMasterFK=hd.HostelMasterPK where r.Delflag=0 and r.cc=0 and r.roll_no like '" + prefixText + "%' order by r.Roll_No desc";
        else  if (rdbchk == "Staff")
            query = "select staff_code from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0  and s.appl_no = a.appl_no  and a.appl_id in(select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0 )  and staff_code like  '" + prefixText + "%' ";
        else if (rdbchk == "Guest")
            query = "select im.VendorContactPK as GuestCode,VenContactName as Stud_Name,gr.id,VendorCompName,VenContactDesig,VenContactDept,VendorAddress,VendorCity,VendorDist,VendorState,im.VendorMobileNo ,gr.HostelMasterFK,hd.HostelName,BuildingFK,FloorFK,RoomFK,case when IsVacated=0 then 'No' when IsVacated=1 then 'Yes' end IsVacated,CONVERT(varchar(10), VacatedDate,103) as vacate_date,APP_No as Roll_No,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,gr.id,im.VendorContactPK as GuestCode  from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd where co.VendorPK=im.VendorFK and MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK  and isnull(IsVacated,'0')='0' and im.VendorContactPK like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //string query = "select stud_name from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and Stud_Type ='Hostler' and stud_name like '" + prefixText + "%' order by stud_name";
        string query = string.Empty;
        if (rdbchk == "Student")
         query = "select r.Stud_Name from Registration as r join Hostel_StudentDetails as hs on r.Roll_Admit=hs.Roll_Admit  join Hostel_Details as hd on hs.Hostel_Code=hd.Hostel_Code where r.Delflag=0 and r.cc=0 and r.Stud_Type ='Hostler' and r.Delflag=0 and r.cc=0 and  r.Stud_Name like '" + prefixText + "%' order by r.Stud_Name";
        else if (rdbchk == "Staff")
            query = "select staff_name from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0  and s.appl_no = a.appl_no  and a.appl_id in(select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0 and isnull(IsVacated,'0')='0')  and staff_code like  '" + prefixText + "%' ";
        else if (rdbchk == "Guest")
            query = "select VenContactName as Stud_Name,im.VendorContactPK as GuestCode,gr.id,VendorCompName,VenContactDesig,VenContactDept,VendorAddress,VendorCity,VendorDist,VendorState,im.VendorMobileNo ,gr.HostelMasterFK,hd.HostelName,BuildingFK,FloorFK,RoomFK,case when IsVacated=0 then 'No' when IsVacated=1 then 'Yes' end IsVacated,CONVERT(varchar(10), VacatedDate,103) as vacate_date,APP_No as Roll_No,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,gr.id,im.VendorContactPK as GuestCode  from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd where co.VendorPK=im.VendorFK and MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK  and isnull(IsVacated,'0')='0' and im.VenContactName like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    protected void txt_rollno_Changed(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Convert.ToString(ViewState["NoOfStudents"])))
        {
            string name = "";
            string degree = "";
            string hstl_name = "";
            string roll_admit = "";
            string hoscode = "";
            string stuid = string.Empty;
            string roll_no = Convert.ToString(txt_rollno.Text);
            stuid = Convert.ToString(Txtid.Text);
            string query = string.Empty;
            if (rdbchk == "Student")
            {
                if (roll_no != "")
                    query = "select r.Roll_No,r.APP_no,r.Roll_Admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.HostelMasterPK ,hd.HostelName,hs.id  from Registration r,HM_HostelMaster hd,HT_HostelRegistration hs,Degree d,Department dt,Course c where r.App_No =hs.App_No and hs.HostelMasterFK =hd.HostelMasterPK and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No = '" + roll_no + "'";
                if (stuid != "")
                    query = "select r.Roll_No,r.APP_no,r.Roll_Admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.HostelMasterPK ,hd.HostelName,hs.id  from Registration r,HM_HostelMaster hd,HT_HostelRegistration hs,Degree d,Department dt,Course c where r.App_No =hs.App_No and hs.HostelMasterFK =hd.HostelMasterPK and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and hs.id = '" + stuid + "'";
            }
            else if (rdbchk == "Staff")
            {
                if (roll_no != "")
                query = " select  hsd.APP_No,sm.staff_code as Roll_No,sm.staff_name as Stud_Name,dm.desig_name as Roll_Admit,h.dept_name as Degree,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName,hsd.BuildingFK,hd.HostelMasterPK,hsd.FloorFK,hsd.RoomFK,CONVERT(varchar(10), VacatedDate,103) as VacatedDate , CONVERT(varchar(10), DiscontinueDate,103) as DiscontinueDate,hsd.Reason,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,hsd.id from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code   and latestrec=1 and ISNULL(IsVacated,'0')=0 and sm.staff_code='" + roll_no + "'";
                 if (stuid != "")
                     query = "select  hsd.APP_No,sm.staff_code as Roll_No,sm.staff_name as Stud_Name,dm.desig_name as Roll_Admit,h.dept_name as Degree,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName,hsd.BuildingFK,hd.HostelMasterPK,hsd.FloorFK,hsd.RoomFK,CONVERT(varchar(10), VacatedDate,103) as VacatedDate , CONVERT(varchar(10), DiscontinueDate,103) as DiscontinueDate,hsd.Reason,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,hsd.id from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code   and latestrec=1 and ISNULL(IsVacated,'0')=0 and hsd.id='" + stuid + "'";
            }
            else if (rdbchk == "Guest")
            {
                if (roll_no != "")
                    query = "select im.VendorContactPK as Roll_No,VenContactName as Stud_Name,gr.id,VendorCompName as Degree,VenContactDesig as APP_No,VenContactDept,VendorAddress,VendorCity,VendorDist,VendorState,im.VendorMobileNo ,hd.HostelMasterPK,gr.HostelMasterFK,hd.HostelName,BuildingFK as Roll_Admit,FloorFK,RoomFK,case when IsVacated=0 then 'No' when IsVacated=1 then 'Yes' end IsVacated,CONVERT(varchar(10), VacatedDate,103) as vacate_date,APP_No as Roll_No,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,gr.id,im.VendorContactPK as GuestCode  from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd where co.VendorPK=im.VendorFK and MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK  and isnull(IsVacated,'0')='0' and im.VendorContactPK='" + roll_no + "'";
                 if (stuid != "")
                     query = "select im.VendorContactPK as Roll_No,VenContactName as Stud_Name,gr.id,VendorCompName as Degree,VenContactDesig as APP_No,VenContactDept,VendorAddress,VendorCity,VendorDist,VendorState,im.VendorMobileNo ,hd.HostelMasterPK,gr.HostelMasterFK,hd.HostelName,BuildingFK as Roll_Admit,FloorFK,RoomFK,case when IsVacated=0 then 'No' when IsVacated=1 then 'Yes' end IsVacated,CONVERT(varchar(10), VacatedDate,103) as vacate_date,APP_No as Roll_No,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,gr.id,im.VendorContactPK as GuestCode  from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd where co.VendorPK=im.VendorFK and MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK  and isnull(IsVacated,'0')='0' and gr.id='" + stuid + "'";
            }
            //string query = "select r.Roll_No,r.Roll_Admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name  from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_No =hs.Roll_No and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No = '" + roll_no + "'";//and r.Stud_Type ='Hostler' 
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                name = ds.Tables[0].Rows[0]["Stud_Name"].ToString();
                degree = ds.Tables[0].Rows[0]["Degree"].ToString();
                roll_no = ds.Tables[0].Rows[0]["Roll_No"].ToString();
                hstl_name = ds.Tables[0].Rows[0]["HostelName"].ToString();
                roll_admit = ds.Tables[0].Rows[0]["Roll_Admit"].ToString();
                hoscode = ds.Tables[0].Rows[0]["HostelMasterPK"].ToString();
                Session["Roll_Admit"] = roll_admit;
                Session["Hostel_Code"] = hoscode;
                stuid = ds.Tables[0].Rows[0]["id"].ToString();
                txt_name.Text = name;
                txt_degree.Text = degree;
                txt_hostelname1.Text = hstl_name;
                Txtid.Text = stuid;
                txt_rollno.Text = roll_no;
                Label4.Text = ds.Tables[0].Rows[0]["APP_No"].ToString();
            }
            else
            {
                txt_rollno.Text = "";
                txt_name.Text = "";
                txt_degree.Text = "";
                txt_hostelname1.Text = "";
                Txtid.Text = "";
            }
        }
    }
    protected void btnquestion_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = true;
        bindhostelname1();
       // binddegree();
        spreaddiv.Visible = false;
        btnSelectStudent.Visible = false;
        txt_rollnum2.Text = "";
        lbl_count.Visible = false;
        if (rdb_staff.Checked ==true)
        {
            string courseidss="";
            bindbranch(courseidss);
        }
        //bindbranch(courseid);
    }
    protected void txt_name_Changed(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Convert.ToString(ViewState["NoOfStudents"])))
        {
            string roll_no = "";
            string degree = "";
            string hstl_name = "";
            string roll_admit = "";
            string hoscode = "";
            string name = Convert.ToString(txt_name.Text);
            string query=string.Empty;
            //string query = "select r.Roll_No,r.Roll_Admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name  from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_Admit =hs.Roll_Admit and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Stud_Type ='Hostler' and Stud_Name = '" + name + "'";
            if (rdbchk == "Student")
            {
                if (name != "")
                    query = "select r.Roll_No,r.Roll_Admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name  from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_Admit =hs.Roll_Admit and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Stud_Type ='Hostler' and r.Stud_Name = '" + name + "'";
            }
             else if (rdbchk == "Staff")
            {
                if (name != "")
                    query = " select  hsd.APP_No,sm.staff_code as Roll_No,sm.staff_name as Stud_Name,dm.desig_name as Roll_Admit,h.dept_name as Degree,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName as Hostel_Name,hsd.BuildingFK,hd.HostelMasterPK as Hostel_code,hsd.FloorFK,hsd.RoomFK,CONVERT(varchar(10), VacatedDate,103) as VacatedDate , CONVERT(varchar(10), DiscontinueDate,103) as DiscontinueDate,hsd.Reason,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,hsd.id from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code   and latestrec=1 and ISNULL(IsVacated,'0')=0 and sm.staff_name='" + name + "'";
                
            }
            else if (rdbchk == "Guest")
            {
                if (name != "")
                    query = "select im.VendorContactPK as Roll_No,VenContactName as Stud_Name,gr.id,VendorCompName as Degree,VenContactDesig as APP_No,VenContactDept,VendorAddress,VendorCity,VendorDist,VendorState,im.VendorMobileNo ,hd.HostelMasterPK as Hostel_code,gr.HostelMasterFK,hd.HostelName as Hostel_Name,BuildingFK as Roll_Admit,FloorFK,RoomFK,case when IsVacated=0 then 'No' when IsVacated=1 then 'Yes' end IsVacated,CONVERT(varchar(10), VacatedDate,103) as vacate_date,APP_No as Roll_No,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,gr.id,im.VendorContactPK as GuestCode  from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd where co.VendorPK=im.VendorFK and MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK  and isnull(IsVacated,'0')='0' and VenContactName='" + name + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                roll_no = ds.Tables[0].Rows[0]["Roll_No"].ToString();
                degree = ds.Tables[0].Rows[0]["Degree"].ToString();
                hstl_name = ds.Tables[0].Rows[0]["Hostel_Name"].ToString();
                roll_admit = ds.Tables[0].Rows[0]["Roll_Admit"].ToString();
                hoscode = ds.Tables[0].Rows[0]["Hostel_code"].ToString();
                Session["Roll_Admit"] = roll_admit;
                Session["Hostel_Code"] = hoscode;
                txt_rollno.Text = roll_no;
                txt_degree.Text = degree;
                txt_hostelname1.Text = hstl_name;
                Label4.Text = ds.Tables[0].Rows[0]["APP_No"].ToString();
                //}
            }
            else
            {
                txt_rollno.Text = "";
                txt_name.Text = "";
                txt_degree.Text = "";
                txt_hostelname1.Text = "";
                Txtid.Text = "";
            }
        }
    }
    protected void rdbdays_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_days.Checked == true)
        {
            txt_rebatedate.Enabled = false;
            txt_fromdate1.Enabled = true;
            txt_todate1.Enabled = true;
            txt_rebateamt.Enabled = false;
            txt_rebatedays.Enabled = true;
            txt_rebatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
    protected void rdbrebateamt_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_rebateamt.Checked == true)
        {
            txt_rebatedate.Enabled = true;
            txt_fromdate1.Enabled = false;
            txt_todate1.Enabled = false;
            txt_rebateamt.Enabled = true;
            txt_rebatedays.Enabled = false;
            txt_rebatedays.Text = "0";
            txt_leavedays.Text = "0";
            txt_rebatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
    protected void txt_fromdate1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate1.Text != "" && txt_todate1.Text != "")
            {
                txt_leavedays.Text = "";
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate1.Text);
                string seconddate = Convert.ToString(txt_todate1.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days + 1;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Enter FromDate less than or equal to the ToDate";
                    txt_leavedays.Text = "";
                    txt_rebatedays.Text = "";
                    txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = Convert.ToString(days);
                    //txt_rebatedays.Text = Convert.ToString(days);
                }
                else
                {
                    txt_leavedays.Text = Convert.ToString(days);
                    txt_rebatedays.Text = Convert.ToString(days);
                }
            }
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter FromDate less than or equal to the ToDate", cv_fromtodt1);
    }
    protected void txt_todate1_TextChanged1(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate1.Text != "" && txt_todate1.Text != "")
            {
                txt_leavedays.Text = "";
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate1.Text);
                string seconddate = Convert.ToString(txt_todate1.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days + 1;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Enter ToDate greater than or equal to the FromDate ";
                    txt_leavedays.Text = "";
                    txt_rebatedays.Text = "";
                    txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = Convert.ToString(days);
                    //txt_rebatedays.Text = Convert.ToString(days);
                }
                else
                {
                    txt_leavedays.Text = Convert.ToString(days);
                    txt_rebatedays.Text = Convert.ToString(days);
                }
            }
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter ToDate greater than or equal to the FromDate", cv_fromtodt2);
    }
    public void btn_exit_fp_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    /* description */
    public void loaddesc()
    {
        ddl_description.Items.Clear();
        ds.Tables.Clear();
        //string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='RDesc' and college_code ='" + collegecode1 + "'";
        string sql = "select MasterCode,MasterValue from CO_MasterValues where  MasterCriteria ='RebatestudentDesc' and collegecode ='" + collegecode1 + "'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_description.DataSource = ds;
            ddl_description.DataTextField = "MasterValue";
            ddl_description.DataValueField = "MasterCode";
            ddl_description.DataBind();
            ddl_description.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        else
        {
            ddl_description.Items.Insert(0, new ListItem("--Select--", "0"));
        }
    }
    protected void btnplus_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = true;
        panel_description.Visible = true;
    }
    protected void btn_sureyesminus_Click(object sender, EventArgs e)
    {
        deleteminus();
    }
    protected void btn_surenominus_Click(object sender, EventArgs e)
    {
        surediv1.Visible = false;
        imgdiv2.Visible = false;
    }
    public void deleteminus()
    {
        try
        {
            surediv1.Visible = false;
            if (ddl_description.SelectedIndex == -1)
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "No records found";
            }
            else if (ddl_description.SelectedIndex == 0)
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Select any record";
            }
            else if (ddl_description.SelectedIndex != 0)
            {
                //string sql = "delete from textvaltable where TextCode='" + ddl_description.SelectedItem.Value.ToString() + "' and TextCriteria='RDesc' and college_code='" + collegecode1 + "' ";
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_description.SelectedItem.Value.ToString() + "' and MasterCriteria ='RebatestudentDesc' and college_code='" + collegecode1 + "' ";
                int delete = d2.update_method_wo_parameter(sql, "TEXT");
                if (delete != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Deleted Successfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "No records found";
                }
                loaddesc();
            }
            //else if (ddl_description.SelectedIndex == -1)
            //{
            //    imgdiv2.Visible = true;
            //    lbl_erroralert.Text = "No records found";
            //}
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "No records found";
            }
        }
        catch
        {
        }
    }
    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_minus.Text == "-")
            {
                surediv1.Visible = true;
                lbl_sureminus.Text = "Do you want to Delete this Record?";
            }
        }
        catch
        {
        }
    }
    protected void btndescpopexit_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
        panel_description.Visible = false;
    }
    protected void btndescpopadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_description11.Text != "")
            {
                CultureInfo cI = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cI.TextInfo;
                txt_description11.Text = textInfo.ToTitleCase(txt_description11.Text);
                //string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_description11.Text.ToUpperInvariant() + "' and TextCriteria ='RDesc' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txt_description11.Text + "' where TextVal ='" + txt_description11.Text + "' and TextCriteria ='RDesc' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_description11.Text + "','RDesc','" + collegecode1 + "')";
                string sql = " if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text.ToUpperInvariant() + "' and MasterCriteria ='RebatestudentDesc' and collegecode ='" + collegecode1 + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='RebatestudentDesc' and collegecode ='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,collegecode) values ('" + txt_description11.Text + "','RebatestudentDesc','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Saved Successfully";
                    txt_description11.Text = "";
                    imgdiv3.Visible = false;
                    panel_description.Visible = false;
                }
                loaddesc();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Enter the description";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void clearpopup()
    {
        txt_rollno.Text = txt_name.Text = txt_leavedays.Text = txt_rebateamt.Text = txt_rebatedays.Text =Txtid.Text= txt_degree.Text = "";
        //txt_hostelname1.Text
        txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_rebatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        loaddesc();
    }
    protected void btnpopsave_Click(object sender, EventArgs e)
    {
        string app_no = string.Empty;
        string name = string.Empty;
        string degree = string.Empty;
        string hostlname = string.Empty;
        string descrip = string.Empty;
        string date1 = string.Empty;
        string memtyp = string.Empty;
        try
        {
            string sql = "";
            int query = 0;
            btnSelectStudent_Click(sender, e);
            string dtaccessdate = DateTime.Now.ToString("MM/dd/yyyy");
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            //app_no = d2.GetFunction("select app_no from Registration where Roll_No='" + txt_rollno.Text + "'");
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            DateTime rebatedt = new DateTime();
            string firstdate = Convert.ToString(txt_fromdate1.Text);
            string seconddate = Convert.ToString(txt_todate1.Text);
            string rebatedate = Convert.ToString(txt_rebatedate.Text);
            //  string[] split = firstdate.Split(' ');
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = rebatedate.Split('/');
            rebatedt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            //command by kowshi 15.2.2018
            //string roll_admit = Session["Roll_Admit"].ToString();
            //string hostel_code = Session["Hostel_Code"].ToString();
            //kowshika
            if (string.IsNullOrEmpty(Convert.ToString(ViewState["NoOfStudents"])))
            {
                rollno = txt_rollno.Text;
                if (rdb_staff.Checked == true)
                    rollno = Label4.Text;
            }
            else
            {
                rollno = Convert.ToString(ViewState["NoOfStudents"]);
                if (rdb_staff.Checked == true)
                    rollno = Convert.ToString(ViewState["NoOfapp"]);
            }
            name = Convert.ToString(txt_name.Text);
            degree = Convert.ToString(txt_degree.Text);
            hostlname = Convert.ToString(txt_hostelname1.Text);
            descrip = Convert.ToString(ddl_description.SelectedItem.Value);
            string rollnum = string.Empty;
            string[] split1 = rollno.Split(';');
            Label4.Text = "";
            if (split1.Length > 0)
            {
                for (int i = 0; i < split1.Length; i++)
                {
                    rollnum = Convert.ToString(split1[i]);
                    if (rdb_guest.Checked == true)
                    {
                        app_no = rollnum;
                        memtyp = "3";

                    }
                   else if (rdb_staff.Checked == true)
                    {
                        app_no = rollnum;
                        memtyp = "2";
                    }
                    else
                    {
                        app_no = d2.GetFunction("select app_no from Registration where Roll_No='" + rollnum + "'");
                        memtyp = "1";
                    }
                    ds.Clear();
                    if (app_no.Trim() != "0" && app_no.Trim() != "")
                    {
                        if (rdb_days.Checked == true)
                        {
                            if (txt_rebatedays.Text != "" && txt_rebatedays.Text != "0")
                            {
                                if (txt_rebatedays.Text.Trim() == "")
                                    txt_rebatedays.Text = "0";
                                if (txt_leavedays.Text.Trim() == "")
                                    txt_leavedays.Text = "0";
                                sql = "if exists (select * from HT_HostelRebateDetail where App_No='" + app_no + "' and RebateFromDate='" + dt.ToString("MM/dd/yyyy") + "' and RebateToDate='" + dt1.ToString("MM/dd/yyyy") + "'  and RebateDesc ='" + descrip + "') update HT_HostelRebateDetail set MemType='" + memtyp + "',App_No='" + app_no + "',RebateType='1',RebateFromDate='" + dt.ToString("MM/dd/yyyy") + "', RebateToDate='" + dt1.ToString("MM/dd/yyyy") + "',LeaveDays='" + txt_leavedays.Text + "',RebateDays='" + txt_rebatedays.Text + "',RebateDesc='" + descrip + "' where App_No='" + app_no + "' and RebateFromDate='" + dt.ToString("MM/dd/yyyy") + "' and RebateToDate='" + dt1.ToString("MM/dd/yyyy") + "' and RebateDesc ='" + descrip + "' else insert into HT_HostelRebateDetail(MemType,App_No,RebateType,RebateFromDate,RebateToDate,LeaveDays,RebateDays,RebateDesc) values('" + memtyp + "','" + app_no + "','1','" + dt.ToString("MM/dd/yyyy") + "','" + dt1.ToString("MM/dd/yyyy") + "','" + txt_leavedays.Text + "','" + txt_rebatedays.Text + "','" + descrip + "')";
                                //sql = "  insert into HT_HostelRebateDetail(MemType,App_No,RebateType,RebateFromDate,RebateToDate,LeaveDays,RebateDays,RebateDesc) values('1','" + app_no + "','1','" + dt.ToString("MM/dd/yyyy") + "','" + dt1.ToString("MM/dd/yyyy") + "','" + txt_leavedays.Text + "','" + txt_rebatedays.Text + "','" + ddl_description.SelectedItem.Value + "')";
                                //sql = "if exists ( select * from StudentRebate_Details where From_Date='" + dt + "' and To_Date='" + dt1 + "' and Hostel_Code ='" + hostel_code + "' and College_Code ='" + collegecode1 + "' and Rebate_Type ='1' and Roll_No='" + txt_rollno.Text + "') update StudentRebate_Details set Access_Date='" + dtaccessdate + "',Access_Time='" + dtaccesstime + "',Roll_Admit='" + roll_admit + "',Leave_Days='" + txt_leavedays.Text + "',Rebate_Days='" + txt_rebatedays.Text + "',Desc_Code='" + ddl_description.SelectedItem.Value + "' where From_Date='" + dt + "' and To_Date='" + dt1 + "' and Hostel_Code ='" + hostel_code + "' and College_Code ='" + collegecode1 + "' and Rebate_Type ='1' and Roll_No='" + txt_rollno.Text + "' else insert into StudentRebate_Details(Access_Date,Access_Time,Roll_Admit,Roll_No,From_Date,To_Date,Leave_Days,Rebate_Days,Desc_Code,Hostel_Code,College_Code,Rebate_Type) values('" + dtaccessdate + "','" + dtaccesstime + "','" + roll_admit + "','" + txt_rollno.Text + "','" + dt + "','" + dt1 + "','" + txt_leavedays.Text + "','" + txt_rebatedays.Text + "','" + ddl_description.SelectedItem.Value + "','" + hostel_code + "','" + collegecode1 + "','1')";
                                query = d2.update_method_wo_parameter(sql, "TEXT");
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_erroralert.Text = "Please Select Rebate Days";
                            }
                        }
                        if (rdb_rebateamt.Checked == true)
                        {
                            if (txt_rebateamt.Text.Trim() == "")
                                txt_rebateamt.Text = "0";
                            sql = "if exists (select * from HT_HostelRebateDetail where App_No='" + app_no + "' and RebateDesc ='" + descrip + "') update HT_HostelRebateDetail set MemType='" + memtyp + "',App_No='" + app_no + "',RebateType='2',RebateAmount='" + txt_rebateamt.Text + "',RebateDesc='" + descrip + "' where App_No='" + app_no + "' and RebateDesc ='" + descrip + "' else insert into HT_HostelRebateDetail(MemType,App_No,RebateType,RebateAmount,RebateDesc) values('" + memtyp + "','" + app_no + "','2','" + txt_rebateamt.Text + "','" + descrip + "')";
                            //sql = " insert into HT_HostelRebateDetail(MemType,App_No,RebateType,RebateAmount,RebateDesc) values('1','" + app_no + "','2','" + txt_rebateamt.Text + "','" + ddl_description.SelectedItem.Value + "')";
                            //sql = "if exists (select * from StudentRebate_Details where Roll_No='" + txt_rollno.Text + "' and Rebate_Date='" + rebatedt + "' and Hostel_Code='" + hostel_code + "' and College_Code='" + collegecode1 + "' and Rebate_Type='0') update StudentRebate_Details set Access_Date ='" + dtaccessdate + "',Access_Time ='" + dtaccesstime + "', Roll_Admit='" + roll_admit + "',Rebate_Amount='" + txt_rebateamt.Text + "',Leave_Days='0',Desc_Code='" + ddl_description.SelectedItem.Value + "' where Roll_No='" + txt_rollno.Text + "' and Rebate_Date='" + rebatedt + "' and Hostel_Code='" + hostel_code + "' and College_Code='" + collegecode1 + "' and Rebate_Type='0' else  insert into StudentRebate_Details(Access_Date,Access_Time,Roll_Admit,Roll_No,Rebate_Date,Rebate_Amount,Leave_Days,Desc_Code,Hostel_Code,College_Code,Rebate_Type) values('" + dtaccessdate + "','" + dtaccesstime + "','" + roll_admit + "','" + txt_rollno.Text + "','" + rebatedt + "','" + txt_rebateamt.Text + "','0','" + ddl_description.SelectedItem.Value + "','" + hostel_code + "','" + collegecode1 + "','0')";
                            query = d2.update_method_wo_parameter(sql, "TEXT");
                        }
                    }
                }
            }
            if (query != 0)
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Saved Successfully";
                clearpopup();
                ViewState["NoOfStudents"] = null;
                ViewState["NoOfapp"] = null;
            }
        }
        catch (Exception ex) { }
    }//
    protected void btnpopexit_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = "";
            int query = 0;
            string dtaccessdate = DateTime.Now.ToString("MM/dd/yyyy");
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string roll_admit = d2.GetFunction("select Roll_Admit from Registration where Roll_No='" + txt_rollno.Text + "'");
            string hostel_code = d2.GetFunction("select Hostel_Code from Hostel_Details where Hostel_Name='" + txt_hostelname1.Text + "'");
            string memtyp = string.Empty;
            if (rdb_guest.Checked == true)
            {
              
                memtyp = "3";

            }
            else if (rdb_staff.Checked == true)
            {
               
                memtyp = "2";
            }
            else
            {
                
                memtyp = "1";
            }
            if (rdb_days.Checked == true)
            {
                if (txt_rebatedays.Text != "" && txt_rebatedays.Text != "0")
                {
                    DateTime dt = new DateTime();
                    DateTime dt1 = new DateTime();
                    string firstdate = Convert.ToString(txt_fromdate1.Text);
                    string seconddate = Convert.ToString(txt_todate1.Text);
                    string[] split = firstdate.Split('/');
                    dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                    split = seconddate.Split('/');
                     string app_no =string.Empty;
                    dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                    if (rdb_guest.Checked == true)
                        app_no = txt_rollno.Text;
                    else if (rdb_staff.Checked == true)
                        app_no = Label4.Text;
                    else
                     app_no = d2.GetFunction("select app_no from Registration where Roll_No='" + txt_rollno.Text + "'");
                    sql = "update HT_HostelRebateDetail set MemType ='" + memtyp + "', App_No ='" + app_no + "', RebateType='1', RebateFromDate='" + dt.ToString("MM/dd/yyyy") + "',RebateToDate='" + dt1.ToString("MM/dd/yyyy") + "',LeaveDays='" + txt_leavedays.Text + "',RebateDays='" + txt_rebatedays.Text + "',RebateDesc='" + ddl_description.SelectedItem.Value + "' where RebateDetailPK ='" + Convert.ToString(Session["Rebetpk"]) + "'";
                    //Session["Rebetpk"] = Convert.ToString(Rebetpk);
                    //sql = "if exists ( select * from StudentRebate_Details where From_Date='" + dt + "' and To_Date='" + dt1 + "' and Hostel_Code ='" + hostel_code + "' and College_Code ='" + collegecode1 + "' and Rebate_Type ='1' and Roll_No='" + txt_rollno.Text + "') update StudentRebate_Details set Access_Date='" + dtaccessdate + "',Access_Time='" + dtaccesstime + "',Roll_Admit='" + roll_admit + "',Leave_Days='" + txt_leavedays.Text + "',Rebate_Days='" + txt_rebatedays.Text + "',Desc_Code='" + ddl_description.SelectedItem.Value + "' where From_Date='" + dt + "' and To_Date='" + dt1 + "' and Hostel_Code ='" + hostel_code + "' and College_Code ='" + collegecode1 + "' and Rebate_Type ='1' and Roll_No='" + txt_rollno.Text + "' else insert into StudentRebate_Details(Access_Date,Access_Time,Roll_Admit,Roll_No,From_Date,To_Date,Leave_Days,Rebate_Days,Desc_Code,Hostel_Code,College_Code,Rebate_Type) values('" + dtaccessdate + "','" + dtaccesstime + "','" + roll_admit + "','" + txt_rollno.Text + "','" + dt + "','" + dt1 + "','" + txt_leavedays.Text + "','" + txt_rebatedays.Text + "','" + ddl_description.SelectedItem.Value + "','" + hostel_code + "','" + collegecode1 + "','1')";
                    //  RebateDetailPK ='" + Convert.ToString( Session["Rebetpk"]) + "'
                    //sql = "if exists ( select * from StudentRebate_Details where  Hostel_Code ='" + hostel_code + "' and College_Code ='" + collegecode1 + "' and Rebate_Type ='1' and Roll_No='" + txt_rollno.Text + "') update StudentRebate_Details set Access_Date='" + dtaccessdate + "',Access_Time='" + dtaccesstime + "',Roll_Admit='" + roll_admit + "',Leave_Days='" + txt_leavedays.Text + "',Rebate_Days='" + txt_rebatedays.Text + "',Desc_Code='" + ddl_description.SelectedItem.Value + "',From_Date='" + dt + "' , To_Date='" + dt1 + "' where  Hostel_Code ='" + hostel_code + "' and College_Code ='" + collegecode1 + "' and Rebate_Type ='1' and Roll_No='" + txt_rollno.Text + "' else insert into StudentRebate_Details(Access_Date,Access_Time,Roll_Admit,Roll_No,From_Date,To_Date,Leave_Days,Rebate_Days,Desc_Code,Hostel_Code,College_Code,Rebate_Type) values('" + dtaccessdate + "','" + dtaccesstime + "','" + roll_admit + "','" + txt_rollno.Text + "','" + dt + "','" + dt1 + "','" + txt_leavedays.Text + "','" + txt_rebatedays.Text + "','" + ddl_description.SelectedItem.Value + "','" + hostel_code + "','" + collegecode1 + "','1')";
                    query = d2.update_method_wo_parameter(sql, "TEXT");
                    if (query != 0)
                    {
                        imgdiv2.Visible = true;
                        lbl_erroralert.Text = "Updated Successfully";
                        popwindow1.Visible = false;
                        btn_delete.Visible = false;
                        btn_update.Visible = false;
                        btn_exit_fp.Visible = false;
                        clearpopup();
                        btngo_Click(sender, e);
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Please Select Rebate Days";
                }
            }
            if (rdb_rebateamt.Checked == true)
            {
                DateTime rebatedt = new DateTime();
                string rebatedate = Convert.ToString(txt_rebatedate.Text);
              //  string[] split = rebatedate.Split('/');
               // rebatedt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
             
                string app_no = string.Empty;
                if (rdb_guest.Checked == true)
                    app_no = txt_rollno.Text;
                else if (rdb_staff.Checked == true)
                    app_no = Label4.Text;
                else
                    app_no = d2.GetFunction("select app_no from Registration where Roll_No='" + txt_rollno.Text + "'");
                sql = "update HT_HostelRebateDetail set MemType='" + memtyp + "',App_No='" + app_no + "',RebateType='2',RebateAmount='" + txt_rebateamt.Text + "',RebateDesc='" + Convert.ToString(ddl_description.SelectedItem.Value) + "' where App_No='" + app_no + "' and RebateDesc ='" + Convert.ToString(ddl_description.SelectedItem.Value) +"'";
                //sql = "if exists (select * from StudentRebate_Details where Roll_No='" + txt_rollno.Text + "' and Hostel_Code='" + hostel_code + "' and College_Code='" + collegecode1 + "' and Rebate_Type='0') update StudentRebate_Details set Access_Date ='" + dtaccessdate + "',Access_Time ='" + dtaccesstime + "', Roll_Admit='" + roll_admit + "',Rebate_Amount='" + txt_rebateamt.Text + "',Leave_Days='0',Desc_Code='" + ddl_description.SelectedItem.Value + "',Rebate_Date='" + rebatedt + "' where Roll_No='" + txt_rollno.Text + "'  and Hostel_Code='" + hostel_code + "' and College_Code='" + collegecode1 + "' and Rebate_Type='0' else  insert into StudentRebate_Details(Access_Date,Access_Time,Roll_Admit,Roll_No,Rebate_Date,Rebate_Amount,Leave_Days,Desc_Code,Hostel_Code,College_Code,Rebate_Type) values('" + dtaccessdate + "','" + dtaccesstime + "','" + roll_admit + "','" + txt_rollno.Text + "','" + rebatedt + "','" + txt_rebateamt.Text + "','0','" + ddl_description.SelectedItem.Value + "','" + hostel_code + "','" + collegecode1 + "','0')";
                query = d2.update_method_wo_parameter(sql, "TEXT");
                if (query != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Updated Successfully";
                    popwindow1.Visible = false;
                    btn_delete.Visible = false;
                    btn_update.Visible = false;
                    btn_exit_fp.Visible = false;
                    clearpopup();
                    btngo_Click(sender, e);
                }
            }
        }
        catch (Exception ex) { }
    }
    protected void delete()
    {
        try
        {
            surediv.Visible = false;
            string sql = "";
            int query = 0;
            string dtaccessdate = DateTime.Now.ToString("MM/dd/yyyy");
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string roll_admit = d2.GetFunction("select Stud_Name from Registration where Roll_No='" + txt_rollno.Text + "'"); //Session["Roll_Admit"].ToString();
            string hostel_code = d2.GetFunction("select Hostel_Code from Hostel_Details where Hostel_Name='" + txt_hostelname1.Text + "'"); // Session["Hostel_Code"].ToString();
            if (rdb_days.Checked == true)
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate1.Text);
                string seconddate = Convert.ToString(txt_todate1.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                //sql = "delete from StudentRebate_Details where From_Date='" + dt + "' and To_Date='" + dt1 + "' and Hostel_Code ='" + hostel_code + "' and College_Code ='" + collegecode1 + "' and Rebate_Type ='1' and Roll_No='" + txt_rollno.Text + "'";
                sql = "delete from HT_HostelRebateDetail where RebateDetailPK ='" + Convert.ToString(Session["Rebetpk"]) + "'";
                query = d2.update_method_wo_parameter(sql, "TEXT");
                if (query != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Deleted Successfully";
                    popwindow1.Visible = false;
                    btn_delete.Visible = false;
                    btn_update.Visible = false;
                    btn_exit_fp.Visible = false;
                    clearpopup();
                    btngo_Click(sender, e);
                }
            }
            else if (rdb_rebateamt.Checked == true)
            {
                DateTime rebatedt = new DateTime();
                string rebatedate = Convert.ToString(txt_rebatedate.Text);
                string[] split = rebatedate.Split('/');
                rebatedt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                sql = "delete from StudentRebate_Details where Roll_No='" + txt_rollno.Text + "' and Rebate_Date='" + rebatedt + "' and Hostel_Code='" + hostel_code + "' and College_Code='" + collegecode1 + "' and Rebate_Type='0'";
                // Session["Rebetpk"]
                //"if exists (select * from StudentRebate_Details where Roll_No='" + txt_rollno.Text + "' and Rebate_Date='" + rebatedt + "' and Hostel_Code='" + hostel_code + "' and College_Code='" + collegecode1 + "' and Rebate_Type='0') update StudentRebate_Details set Access_Date ='" + dtaccessdate + "',Access_Time ='" + dtaccesstime + "', Roll_Admit='" + roll_admit + "',Rebate_Amount='" + txt_rebateamt + "',Leave_Days='0',Desc_Code='" + ddl_description.SelectedItem.Value + "' where Roll_No='" + txt_rollno.Text + "' and Rebate_Date='" + rebatedt + "' and Hostel_Code='" + hostel_code + "' and College_Code='" + collegecode1 + "' and Rebate_Type='0' else  insert into StudentRebate_Details(Access_Date,Access_Time,Roll_Admit,Roll_No,Rebate_Date,Rebate_Amount,Leave_Days,Desc_Code,Hostel_Code,College_Code,Rebate_Type) values('" + dtaccessdate + "','" + dtaccesstime + "','" + roll_admit + "','" + txt_rollno.Text + "','" + rebatedt + "','" + txt_rebateamt.Text + "','0','" + ddl_description.SelectedItem.Value + "','" + hostel_code + "','" + collegecode1 + "','0')";
                query = d2.update_method_wo_parameter(sql, "TEXT");
                if (query != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Deleted Successfully";
                    popwindow1.Visible = false;
                    btn_delete.Visible = false;
                    btn_update.Visible = false;
                    btn_exit_fp.Visible = false;
                    clearpopup();
                    btngo_Click(sender, e);
                }
            }
        }
        catch (Exception ex) { }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to delete this record?";
            }
        }
        catch
        {
        }
    }
    //popupselectstd
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
        clearpopup();
    }
    public void bindhostelname1()
    {
        try
        {
            ds.Clear();
            cbl_hostelname2.Items.Clear();
            //string selecthostel = "select HostelMasterPK,HostelName from HM_HostelMaster  order by HostelMasterPK";//where CollegeCode='" + collegecode1 + "'
            //ds = d2.select_method_wo_parameter(selecthostel, "Text");
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname2.DataSource = ds;
                cbl_hostelname2.DataTextField = "HostelName";
                cbl_hostelname2.DataValueField = "HostelMasterPK";
                cbl_hostelname2.DataBind();
            }
            else
            {
                cbl_hostelname2.Items.Insert(0, "Select");
            }
            if (cbl_hostelname2.Items.Count > 0)
            {
                for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
                {
                    cbl_hostelname2.Items[i].Selected = true;
                    cb_hostelname2.Checked = true;
                }
                txt_hostelname2.Text = "Hostel Name(" + cbl_hostelname2.Items.Count + ")";
            }
            for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
            {
                if (cbl_hostelname2.Items[i].Selected == true)
                {
                    statichostelname = cbl_hostelname2.Items[i].Value.ToString();
                    if (lochos == "")
                    {
                        lochos = statichostelname;
                    }
                    else
                    {
                        lochos = lochos + "'" + "," + "'" + statichostelname + "";
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void chkhostlnm_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname2.Checked == true)
            {
                for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
                {
                    cbl_hostelname2.Items[i].Selected = true;
                }
                txt_hostelname2.Text = "Hostel Name(" + (cbl_hostelname2.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
                {
                    cbl_hostelname2.Items[i].Selected = false;
                }
                txt_hostelname2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void chklsthostlnm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_hostelname2.Text = "--Select--";
            cb_hostelname2.Checked = false;
            for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
            {
                if (cbl_hostelname2.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    statichostelname = cbl_hostelname2.Items[i].Value.ToString();
                    lochos = "";
                    if (lochos == "")
                    {
                        lochos = statichostelname;
                    }
                    else
                    {
                        lochos = lochos + "'" + "," + "'" + statichostelname + "";
                    }
                }
            }
            if (commcount > 0)
            {
                txt_hostelname2.Text = "Hostel Name(" + commcount.ToString() + ")";
                if (commcount == cbl_hostelname2.Items.Count)
                {
                    cb_hostelname2.Checked = true;
                }
            }
            for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
            {
                if (cbl_hostelname2.Items[i].Selected == true)
                {
                    statichostelname = cbl_hostelname2.Items[i].Value.ToString();
                    if (lochos == "")
                    {
                        lochos = statichostelname;
                    }
                    else
                    {
                        lochos = lochos + "'" + "," + "'" + statichostelname + "";
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void bindbatch()
    {
        try
        {
            ddl_batch2.Items.Clear();
            ds.Clear();
            // ds = d2.BindBatch();
            ds = d2.select_method_wo_parameter("select distinct batch_year from applyn where batch_year<>'-1' and batch_year<>''order by batch_year desc", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch2.DataSource = ds;
                ddl_batch2.DataTextField = "batch_year";
                ddl_batch2.DataValueField = "batch_year";
                ddl_batch2.DataBind();
            }
        }
        catch
        {
        }
    }
    public void binddegree()
    {
        try
        {
            ds.Clear();
            ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree2.DataSource = ds;
                cbl_degree2.DataTextField = "course_name";
                cbl_degree2.DataValueField = "course_id";
                cbl_degree2.DataBind();
            }
            if (cbl_degree2.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    cbl_degree2.Items[i].Selected = true;
                    cb_degree2.Checked = true;
                }
                txt_degree2.Text = "Degree(" + cbl_degree2.Items.Count + ")";
            }
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    courseid = cbl_degree2.Items[i].Value.ToString();
                    if (loccourseid == "")
                    {
                        loccourseid = courseid;
                    }
                    else
                    {
                        loccourseid = loccourseid + "," + courseid;
                    }
                }
            }
            bindbranch(loccourseid);
        }
        catch
        {
        }
    }
    //select the student
    protected void btnSelectStudent_Click(object sender, EventArgs e)
    {
        try
        {
            //Boolean value = false;
            Fpspread2.SaveChanges();
            ArrayList hostelnameArr = new ArrayList();
            string degree = string.Empty;
            string id = string.Empty;
            for (int row = 0; row < Fpspread2.Sheets[0].RowCount; row++)
            {
                int selected = 0;
                int.TryParse(Convert.ToString(Fpspread2.Sheets[0].Cells[row, 6].Value), out selected);
                if (selected == 1)
                {
                    if (rdb_staff.Checked==true)
                        staffapp = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 1].Tag).Trim();
                    rollno = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 1].Text).Trim();
                     id = Convert.ToString(Fpspread2.Sheets[0].Cells[row,2].Text).Trim();
                    StudentName = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 4].Text).Trim();
                 degree = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 1].Tag).Trim();
                    hostelname = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 5].Text).Trim();
                    if (String.IsNullOrEmpty(selectrollno))
                    {
                        selectrollno = rollno;
                        selectapp = staffapp;
                    }
                    else
                    {
                        selectrollno += ";" + rollno;
                        selectapp += ";" + staffapp;
                    }
                    if (String.IsNullOrEmpty(selectStudentName))
                        selectStudentName = StudentName;
                    else
                        selectStudentName += ";" + StudentName;
                    if (String.IsNullOrEmpty(selecthostelname))
                    {
                        selecthostelname = hostelname;
                        hostelnameArr.Add(hostelname);
                    }
                    else
                        if (!hostelnameArr.Contains(hostelname))
                        {
                            selecthostelname += ";" + hostelname;
                            hostelnameArr.Add(hostelname);
                        }

                    
                  
                    studCount++;
                }
            }
            if (studCount == 0)
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Please Select Any Student";
                return;
            }
            if (studCount > 1)
            {
                ViewState["NoOfStudents"] = selectrollno;
                ViewState["NoOfapp"] = selectapp;
                txt_hostelname1.Text = selecthostelname;
                txt_rollno.Text = "Selected Student(" + studCount + ")";
                Txtid.Text = "Selected Student(" + studCount + ")";
                
                //ViewState[""]
                //lblstudent.Visible = true;
                //lblstudentcount.Visible = true;
                txt_degree.Enabled = false;
                lbl_degree.Enabled = false;
                lbl_rollno.Visible = true;
                txt_rollno.Visible = true;
                lbl_name.Enabled = false;
                txt_name.Enabled = false;
                popupselectstd.Visible = false;
            }
            else if (studCount ==1)
            {
                txt_rollno.Text = rollno;
                Label4.Text = selectapp;
                Txtid.Text = id;
                txt_name.Text = StudentName;
                txt_hostelname1.Text = hostelname;
                   txt_degree.Text=degree;
                   popupselectstd.Visible = false;
            }
            else
            {
                //lblstudent.Visible = false;
                //lblstudentcount.Visible = false;
                txt_degree.Enabled = true;
                lbl_degree.Enabled = true;
                lbl_rollno.Visible = true;
                txt_rollno.Visible = true;
                popupselectstd.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void chkdeg_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_degree2.Checked == true)
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    cbl_degree2.Items[i].Selected = true;
                }
                txt_degree2.Text = "Degree(" + (cbl_degree2.Items.Count) + ")";
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    if (cbl_degree2.Items[i].Selected == true)
                    {
                        courseid = cbl_degree2.Items[i].Value.ToString();
                        if (loccourseid == "")
                        {
                            loccourseid = courseid;
                        }
                        else
                        {
                            loccourseid = loccourseid + "," + courseid;
                        }
                    }
                }
                bindbranch(loccourseid);
            }
            else
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    cbl_degree2.Items[i].Selected = false;
                }
                txt_degree2.Text = "--Select--";
                cb_branch2.Checked = false;
                cbl_branch2.Items.Clear();
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void chklstdeg_Change(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            //txt_degree2.Text = "--Select--";
            //cb_degree2.Checked = false;
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_degree2.Text = "Degree(" + commcount.ToString() + ")";
                if (commcount == cbl_degree2.Items.Count)
                {
                    cb_degree2.Checked = true;
                }
            }
            else
            {
                txt_degree2.Text = "--Select--";
            }
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    courseid = cbl_degree2.Items[i].Value.ToString();
                    if (loccourseid == "")
                    {
                        loccourseid = courseid;
                    }
                    else
                    {
                        loccourseid = loccourseid + "," + courseid;
                    }
                }
                else
                {
                    cb_degree2.Checked = false;
                    cb_branch2.Checked = false;
                    //txt_degree2.Text = "--Select--";
                    cbl_branch2.Items.Clear();
                    txt_branch2.Text = "--Select--";
                }
            }
            bindbranch(loccourseid);
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch(string courseid)
    {
        try
        {
            string commname = "";
            if (rdb_staff.Checked == false)
            {
                if (courseid != "")
                {
                    commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + courseid + ") and deptprivilages.Degree_code=degree.Degree_code ";
                }
                else
                {
                    commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
                }
                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch2.DataSource = ds;
                    cbl_branch2.DataTextField = "dept_name";
                    cbl_branch2.DataValueField = "degree_code";
                    cbl_branch2.DataBind();
                }
                else
                {
                    cbl_branch2.Items.Clear();
                    txt_branch2.Text = "--Select--";
                }
            }
            else
            {
                commname = "SELECT h.Dept_Code,h.Dept_Name FROM HRDept_Master h,department d WHERE h.dept_code=d.dept_code and d.College_Code=h.College_Code  order by h.Dept_Name";
                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch2.DataSource = ds;
                    cbl_branch2.DataTextField = "dept_name";
                    cbl_branch2.DataValueField = "Dept_Code";
                    cbl_branch2.DataBind();
                }
                else
                {
                    cbl_branch2.Items.Clear();
                    txt_branch2.Text = "--Select--";
                }
            }
           
            if (cbl_branch2.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch2.Items.Count; i++)
                {
                    cbl_branch2.Items[i].Selected = true;
                    cb_branch2.Checked = true;
                }
                txt_branch2.Text = "Branch(" + cbl_branch2.Items.Count + ")";
            }
        }
        catch
        {
        }
    }
    protected void chkbnch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch2.Checked == true)
            {
                for (int i = 0; i < cbl_branch2.Items.Count; i++)
                {
                    cbl_branch2.Items[i].Selected = true;
                }
                txt_branch2.Text = "Branch(" + (cbl_branch2.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch2.Items.Count; i++)
                {
                    cbl_branch2.Items[i].Selected = false;
                }
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void chklstbnch_Change(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_branch2.Checked = false;
            for (int i = 0; i < cbl_branch2.Items.Count; i++)
            {
                if (cbl_branch2.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_branch2.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cbl_branch2.Items.Count)
                {
                    cb_branch2.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getroll(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query =string.Empty;
         if(rdbchk=="Student")
             query = "select distinct top 10 r.Roll_No from Registration as r join HT_HostelRegistration as hs on r.app_no=hs.APP_No join HM_HostelMaster  as hd on hs.HostelMasterFK=hd.HostelMasterPK where r.Delflag=0 and r.cc=0 and r.roll_no like '" + prefixText + "%' and HostelMasterPK in('" + lochos + "') order by r.Roll_No desc";
        //string query = "select distinct r.Roll_No from Registration as r join Hostel_StudentDetails as hs on r.Roll_No=hs.Roll_No join Hostel_Details as hd on hs.Hostel_Code=hd.Hostel_Code where r.Delflag=0 and r.cc=0 and r.roll_no like '" + prefixText + "%' and hd.Hostel_Code in('" + lochos + "') order by r.Roll_No desc"; Session["Hostel_Code"] = hoscode;
         else if (rdbchk == "Staff")
             query = "select staff_code from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0  and s.appl_no = a.appl_no  and a.appl_id in(select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0 and isnull(IsVacated,'0')='0')  and staff_code like  '" + prefixText + "%' ";
         else if (rdbchk == "Guest")
             query = "select VenContactName as Stud_Name,im.VendorContactPK as GuestCode,gr.id,VendorCompName,VenContactDesig,VenContactDept,VendorAddress,VendorCity,VendorDist,VendorState,im.VendorMobileNo ,gr.HostelMasterFK,hd.HostelName,BuildingFK,FloorFK,RoomFK,case when IsVacated=0 then 'No' when IsVacated=1 then 'Yes' end IsVacated,CONVERT(varchar(10), VacatedDate,103) as vacate_date,APP_No as Roll_No,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,gr.id,im.VendorContactPK as GuestCode  from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd where co.VendorPK=im.VendorFK and MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK  and isnull(IsVacated,'0')='0' and im.VenContactName like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    protected void btnpopgo_Click(object sender, EventArgs e)//
    {
        try
        {
            spreaddiv.Visible = true;
            btnSelectStudent.Visible = true;
            Fpspread2.Visible = true;
            string hostelname = "";
            string branchname = "";
            string degreename = "";
            int sno = 0;
            for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
            {
                if (cbl_hostelname2.Items[i].Selected == true)
                {
                    if (hostelname == "")
                    {
                        hostelname = "" + cbl_hostelname2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostelname = hostelname + "'" + "," + "'" + cbl_hostelname2.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (int i = 0; i < cbl_branch2.Items.Count; i++)
            {
                if (cbl_branch2.Items[i].Selected == true)
                {
                    if (branchname == "")
                    {
                        branchname = "" + cbl_branch2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        branchname = branchname + "'" + "," + "'" + cbl_branch2.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    if (degreename == "")
                    {
                        degreename = "" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degreename = degreename + "'" + "," + "'" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                }
            }
            //if (itemheadercode.Trim() != "" && itemheadercode1.Trim()!="")
            //{
            string selectqurey = "";
            if (rdb_student.Checked == true)
            {
                if (txt_rollnum2.Text.Trim() != "")
                {
                    selectqurey = "select r.APP_No,hs.id, r.Roll_No,r.Roll_admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.HostelMasterPK  ,hd.HostelName  from Registration r,HM_HostelMaster hd,HT_HostelRegistration hs,Degree d,Department dt,Course c where r.App_No =hs.APP_No and hs.HostelMasterfK =hd.HostelMasterPK and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and r.Roll_No like  '" + txt_rollnum2.Text + "'and ISNULL(IsVacated,0)=0  order by r.Roll_No ";//magesh 17.3.18 add and IsVacated='0'
                }
                else
                {
                    selectqurey = " select r.APP_No, hs.id, r.Roll_No,r.Roll_admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.HostelMasterPK  ,hd.HostelName  from Registration r,HM_HostelMaster hd,HT_HostelRegistration hs,Degree d,Department dt,Course c where r.App_No =hs.APP_No and hs.HostelMasterfK =hd.HostelMasterPK and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and hd.HostelMasterPK in ('" + hostelname + "') and d.Course_Id in ('" + degreename + "') and d.Degree_Code in ('" + branchname + "') and r.Batch_Year='" + ddl_batch2.SelectedItem.Value.ToString() + "' and ISNULL(IsVacated,0)=0  order by r.Roll_No";//magesh 17.3.18 add and IsVacated='0'
                }
            }
            else    if (rdb_guest.Checked == true)
            {
                if (txt_rollnum2.Text.Trim() != "")
                {
                    selectqurey = " select distinct convert(varchar(10),HostelAdmDate ,103)as Admission_Date,VenContactName as Stud_Name,gr.id,VendorCompName,VenContactDesig,VenContactDept,VendorAddress,VendorCity,VendorDist,VendorState,im.VendorMobileNo ,gr.HostelMasterFK,hd.HostelName,BuildingFK,FloorFK,RoomFK,case when IsVacated=0 then 'No' when IsVacated=1 then 'Yes' end IsVacated,CONVERT(varchar(10), VacatedDate,103) as vacate_date,APP_No as Roll_No,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,gr.id  from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd where co.VendorPK=im.VendorFK and MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK and  gr.HostelMasterFK in('" + hostelname + "') and isnull(IsVacated,'0')='0' and VenContactName like  '" + txt_rollnum2.Text + "'";
                }
                else
                selectqurey = " select distinct convert(varchar(10),HostelAdmDate ,103)as Admission_Date,VenContactName as Stud_Name,gr.id,VendorCompName,VenContactDesig,VenContactDept,VendorAddress,VendorCity,VendorDist,VendorState,im.VendorMobileNo ,gr.HostelMasterFK,hd.HostelName,BuildingFK,FloorFK,RoomFK,case when IsVacated=0 then 'No' when IsVacated=1 then 'Yes' end IsVacated,CONVERT(varchar(10), VacatedDate,103) as vacate_date,APP_No as Roll_No,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,gr.id  from HT_HostelRegistration gr,CO_VendorMaster co,IM_VendorContactMaster im,HM_HostelMaster hd where co.VendorPK=im.VendorFK and MemType='3' and  gr.GuestVendorFK=im.VendorFK  and gr.GuestVendorFK=co.VendorPK and gr.APP_No=im.VendorContactPK AND Hd.HostelMasterPK=gr.HostelMasterFK and  gr.HostelMasterFK in('" + hostelname + "') and isnull(IsVacated,'0')='0' order by  Admission_Date";
            }
            else
            {
                if (txt_rollnum2.Text.Trim() != "")
                {
                    selectqurey = "select  hsd.APP_No,sm.staff_code as Roll_No,sm.staff_name as Stud_Name,dm.desig_name,h.dept_name,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName,hsd.BuildingFK,hsd.FloorFK,hsd.RoomFK,CONVERT(varchar(10), VacatedDate,103) as VacatedDate , CONVERT(varchar(10), DiscontinueDate,103) as DiscontinueDate,hsd.Reason,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,hsd.id from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code   and hsd.HostelMasterFK in('" + hostelname + "') and dm.desig_code  in('" + degreename + "') and h.dept_code in('" + branchname + "')  and latestrec=1 and ISNULL(IsVacated,'0')=0 and sm.staff_code like  '" + txt_rollnum2.Text + "'";
                }
                else
                selectqurey = "select  hsd.APP_No,sm.staff_code as Roll_No,sm.staff_name as Stud_Name,dm.desig_name,h.dept_name,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName,hsd.BuildingFK,hsd.FloorFK,hsd.RoomFK,CONVERT(varchar(10), VacatedDate,103) as VacatedDate , CONVERT(varchar(10), DiscontinueDate,103) as DiscontinueDate,hsd.Reason,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,hsd.id from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code   and hsd.HostelMasterFK in('" + hostelname + "') and dm.desig_code  in('" + degreename + "') and h.dept_code in('" + branchname + "')  and latestrec=1 and ISNULL(IsVacated,'0')=0";
            }
            ds.Clear();
            ds = da.select_method_wo_parameter(selectqurey, "Text");
            if (hostelname.Trim() != "" && branchname.Trim() != "" && degreename.Trim() != "")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    spreaddiv.Visible = true;
                    Fpspread2.Sheets[0].RowCount = 0;
                    Fpspread2.Sheets[0].ColumnCount = 0;
                    Fpspread2.CommandBar.Visible = false;
                   
;
                    Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread2.Sheets[0].RowHeader.Visible = false;
                    Fpspread2.Sheets[0].ColumnCount = 7;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                   
                    darkstyle.ForeColor = Color.White;
                    Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    //Fpspread2.Sheets[0].AutoPostBack = true;
                    //Fpspread2.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    if (rdb_student.Checked == true)
                    {
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Id";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Columns[3].Visible = false;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else if (rdb_guest.Checked == true)
                    {
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Guest Reg";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Guest Id";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                        //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Columns[3].Visible = false;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Guest Name";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else
                    {
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Id";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                        //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Columns[3].Visible = false;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Staff Name";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    }
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Hostel Name";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Select";
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            
                    //Fpspread2.Sheets[0].Cells[0, 5].CellType = chk;
                    Fpspread2.Sheets[0].Columns[0].Width = 50;
                    Fpspread2.Sheets[0].Columns[1].Width = 100;
                    Fpspread2.Sheets[0].Columns[3].Width = 100;
                    Fpspread2.Sheets[0].Columns[4].Width = 250;
                    Fpspread2.Sheets[0].Columns[5].Width = 250;
                    Fpspread2.Sheets[0].Columns[6].Width = 50;
                    FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                   // Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].CellType = chk1;
                    chk1.AutoPostBack = true;
                    Fpspread2.Sheets[0].AutoPostBack = false;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].RowCount++;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].CellType = chk1;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    
                        sno = 0;
                        int studcount = 0;
                        if (rdb_student.Checked == true)
                        {
                        for (int row1 = 0; row1 < cbl_branch2.Items.Count; row1++)
                        {
                            if (cbl_branch2.Items[row1].Selected)
                            {
                                ds.Tables[0].DefaultView.RowFilter = "Degree_Code='" + Convert.ToSingle(cbl_branch2.Items[row1].Value) + "'";
                                DataView dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    Fpspread2.Sheets[0].RowCount = Fpspread2.Sheets[0].RowCount + 1;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv[0]["Degree"]);
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].AddSpanCell(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 6);

                                    sno++;
                                    for (int row = 0; row < dv.Count; row++)
                                    {
                                        studcount++;
                                        Fpspread2.Sheets[0].RowCount++;
                                        //Fpspread2.Sheets[0].RowCount = Fpspread2.Sheets[0].RowCount + 1;
                                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();                               
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[row]["Roll_No"]);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dv[0]["Degree"]);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[row]["id"]);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[row]["Roll_Admit"]);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[row]["Stud_Name"]);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[row]["HostelName"]);
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].CellType = chk;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                            }
                        }
                    }
                        else
                        {
                            Fpspread2.Sheets[0].RowCount = 0;
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                for(int i=0;i<ds.Tables[0].Rows.Count;i++)
                                {
                                    sno++;
                                    studcount++;
                                    Fpspread2.Sheets[0].RowCount++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text =Convert.ToString(sno); 
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                               

                               
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                if (rdb_staff.Checked==true)
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["APP_No"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["id"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["HostelName"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].CellType = chk;
                                      
                                }
                            }
                        }
                    Fpspread2.Visible = true;
                    btnSelectStudent.Visible = true;
                    // rptprint.Visible = true;
                    // div1.Visible = true;
                    // errorlable.Visible = false;
                    lbl_count.Visible = true;
                    lbl_count.Text = "No of Student :" + studcount.ToString();
                    Fpspread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);//
                    Fpspread2.Columns[1].VerticalAlign = VerticalAlign.Middle;
                    //Fpspread2.Sheets[0].SetRowMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    //Fpspread2.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                    lbl_error.Visible = false;
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Width = 750;
                }
                else
                {
                    Fpspread2.Visible = false;
                    //imgdiv2.Visible = true;
                    //theivamani 14.11.15
                    lbl_count.Visible = false;
                    spreaddiv.Visible = false;
                    btnSelectStudent.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                    //rptprint.Visible = false;
                    //div1.Visible = false;
                    //errorlable.Visible = true;
                    //errorlable.Text = "No Records Found";
                }
                //}
                //else
                //{
                //    //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Fill all the Values\");", true);
                //    Fpspread2.Visible = false;
                //    //rptprint.Visible = false;
                //    //div1.Visible = false;
                //    //errorlable.Visible = true;
                //    //errorlable.Text = "No Records Found";
                //}
            }
            else
            {
                Fpspread2.Visible = false;
                lbl_count.Visible = false;
                spreaddiv.Visible = false;
                btnSelectStudent.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select all field";
            }
        }
        catch
        {
        }
    }
    //magesh 16.3.18

    protected void Fpspread2_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.Fpspread2, "Select$" + e.Row.RowIndex);
            }
        }
        catch { }
    }
    protected void Fpspread2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {

            }

        }
        catch
        {

        }
    }

    protected void Fpspread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = Convert.ToString(e.SheetView.ActiveRow).Trim();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(Fpspread2.Sheets[0].RowCount); j++)
            {
                string actcol = Convert.ToString(e.SheetView.ActiveColumn).Trim();
                string seltext = Convert.ToString(e.EditValues[Convert.ToInt16(actcol)]).Trim();
                if (seltext != "System.Object")
                    Fpspread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();
            }
            flag_true = true;
        }
    }
    public void loadpopupdef()
    {
        rdb_days.Enabled = true;
        rdb_rebateamt.Enabled = true;
        rdb_days.Checked = true;
        //rdb_rebateamt.Checked = false;
        txt_fromdate1.Enabled = true;
        txt_todate1.Enabled = true;
        txt_rebatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_fromdate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_leavedays.Text = "0";
        txt_rebatedays.Text = "0";
        txt_rebateamt.Text = "";
    }
    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {
        }
    }
    protected void Fpspread2_render(object sender,EventArgs e)
    {
        try
        {
            string activerow1 = "";
            string activecol1 = "";
           // string seltext = "";
           
            activerow1 = Convert.ToString(Fpspread2.ActiveSheetView.ActiveRow).Trim();
            activecol1 = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
            int m = 0;
            int.TryParse(activerow1, out m);

            if (flag_true == false && activerow1 == "-1" && activecol1 == "5")
            {
               
                string actcol = Convert.ToString(Fpspread2.ActiveSheetView.ActiveColumn).Trim();
                //string seltext = Convert.ToString(CommandEventArgs.ReferenceEquals(0, Convert.ToInt16(actcol)).ToString()).Trim();
                string seltext = Convert.ToString(CommandEventArgs.ReferenceEquals(-1, Convert.ToInt16(actcol)).ToString()).Trim();
                 //seltext = Convert.ToString(EditCommandColumn.Equals(0, Convert.ToInt16(actcol)).ToString()).Trim();
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();
                int checkval = Convert.ToInt32(Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Value);
                for (int j = 1; j < Convert.ToInt16(Fpspread2.Sheets[0].RowCount); j++)
                {
                      
                    actcol = Convert.ToString(Fpspread2.ActiveSheetView.ActiveColumn).Trim();
                   // Fpspread2.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();

                     //if(Fpspread2.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(actcol)].Text=="true")
                   // Convert.ToString(e.EditValues[Convert.ToInt16(actcol)]).Trim();
                     //seltext = Convert.ToString(EditCommandColumn.Equals(j,Convert.ToInt16(actcol)).ToString()).Trim();
                    //if (seltext != "true")
                    //    seltext = "true";
                    // string seltext = Convert.ToString(e.EditValues[Convert.ToInt16(actcol)]).Trim();
                    seltext = "true";
                  
                   
                    //EditCommand[Convert.ToInt16(actcol)]).Trim();
                    if (seltext != "System.Object")
                    {
                        //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                        Fpspread2.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();
                        Fpspread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();
                        //seltext = "false";
                    }
                        
                    else
                    {

                        
                        //EditCommand[Convert.ToInt16(actcol)]).Trim();
                        if (seltext != "System.Object")
                        {
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();
                            Fpspread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();
                        }

                    }
                }
                flag_true = true;
                m++;
            }
            //else
            //{
            //    for (int j = 1; j < Convert.ToInt16(Fpspread2.Sheets[0].RowCount); j++)
            //    {
            //        m++;
            //        string actcol = Convert.ToString(Fpspread2.ActiveSheetView.ActiveColumn).Trim();
            //        seltext = "false";
            //        //EditCommand[Convert.ToInt16(actcol)]).Trim();
            //        if (seltext != "System.Object")
            //            Fpspread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();

            //    }
            //    flag_true = true;
            //}

            if (check == true)
            {
                popwindow1.Visible = true;
                btn_save1.Visible = true;
                btn_exit1.Visible = true;
                btn_delete.Visible = false;
                btn_update.Visible = false;
                btn_exit_fp.Visible = false;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
                if (activerow.Trim() != "")
                {
                    string rollnum = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string roll_admit = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string studname = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    string degree = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    string hosname = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    string hoscode = d2.GetFunction("select Hostel_Code from Hostel_Details where Hostel_Name='" + hosname + "'");
                    //string selectquery = "select r.Roll_No,r.Roll_Admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name  from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_Admit =hs.Roll_Admit and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Stud_Type ='Hostler' and r.Roll_No like '" + rollnum + "'";
                    //ds.Clear();
                    //ds = da.select_method_wo_parameter(selectquery, "Text");
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    txt_rollno.Text = Convert.ToString(rollnum);
                    //    txt_name.Text = Convert.ToString(studname);
                    //    txt_degree.Text = Convert.ToString(degree);
                    //    txt_hostelname1.Text = Convert.ToString(hosname);
                    //    Session["Roll_Admit"] = roll_admit;
                    //    Session["Hostel_Code"] = hoscode;
                    //    popupselectstd.Visible = false;
                    //}
                    Session["Roll_Admit"] = roll_admit;
                    Session["Hostel_Code"] = hoscode;
                    txt_rollno.Text = rollnum;
                    txt_name.Text = studname;
                    txt_degree.Text = degree;
                    txt_hostelname1.Text = hosname;
                    Session["Roll_Admit"] = roll_admit;
                    Session["Hostel_Code"] = hoscode;
                    popupselectstd.Visible = false;
                }
                loadpopupdef();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void buttonsv_Click(object sender, EventArgs e)
    {
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        //surediv.Visible = false;
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        popwindow1.Visible = true;
    }
    public object sender { get; set; }
    public EventArgs e { get; set; }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //string query = "select distinct top 10 r.Roll_No from Registration as r join Hostel_StudentDetails as hs on r.Roll_No=hs.Roll_No join Hostel_Details as hd on hs.Hostel_Code=hd.Hostel_Code where r.Delflag=0 and r.cc=0 and r.roll_no like '" + prefixText + "%' order by r.Roll_No desc";
        string query=string.Empty;
         if(rdbchk=="Student")
             query = "select  hs.id from Registration as r join HT_HostelRegistration as hs on r.app_no=hs.APP_No join HM_HostelMaster  as hd on hs.HostelMasterFK=hd.HostelMasterPK where r.Delflag=0 and r.cc=0 and MemType='1' and  hs.id like '" + prefixText + "%' order by hs.id desc";
         else if (rdbchk == "Staff")
             query = "select  hs.id from HT_HostelRegistration hs where ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0  and MemType='2' and  hs.id like '" + prefixText + "%' order by hs.id desc";
         else if (rdbchk == "Guest")
             query = "select  hs.id from HT_HostelRegistration hs where ISNULL(IsVacated,'0') =0 and ISNULL(IsDiscontinued,'0')=0  and MemType='3' and  hs.id like '" + prefixText + "%' order by hs.id desc";
        name = ws.Getname(query);
        return name;
    }

    protected void rdb_guest_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdb_guest.Checked == true)
            {
                ddl_batch2.Enabled = false;
                txt_degree2.Enabled = false;
                txt_branch2.Enabled = false;
                lbl_rollnum2.Text = "Guest Name";
                lbl_rollno.Text = "Guest No";
                txt_degree.Visible = false;
                lbl_degree.Visible = false;
                Label3.Text = "Guest Id";
                txt_degree.Text = "";
                rdbchk = "Guest";
                txt_rollno.Text = "";
                txt_name.Text = "";
                txt_degree.Text = "";
                txt_hostelname1.Text = "";
                Txtid.Text = "";
            }
            else
            {
                ddl_batch2.Enabled = true;
                txt_degree2.Enabled = true;
                txt_branch2.Enabled = true;
                txt_degree.Visible = true;
                lbl_degree.Visible = true;
            }
            if (rdb_student.Checked == true)
            {
                lbl_rollnum2.Text = "Roll No";
                lbl_rollno.Text = "Roll No";
                rdbchk = "Student";
                Label3.Text = "Student Id";
                lbl_degree2.Text = "Degree";
                binddegree();
                txt_rollno.Text = "";
                txt_name.Text = "";
                txt_degree.Text = "";
                txt_hostelname1.Text = "";
                Txtid.Text = "";
            }
            if (rdb_staff.Checked == true)
            {
                lbl_rollnum2.Text = "Staff Code";
                lbl_rollno.Text = "Staff Code";
                Label3.Text = "Staff Id";
                rdbchk = "Staff";
                lbl_degree2.Text = "Destination";
                binddesignation();
                ddl_batch2.Enabled = false;
                txt_branch2.Enabled = true;
                txt_rollno.Text = "";
                txt_name.Text = "";
                txt_degree.Text = "";
                txt_hostelname1.Text = "";
                Txtid.Text = "";
            }

        }
        catch
        {
        }
    }

    public void binddesignation()
    {
        try
        {
            ds.Clear();
            string clgcode = "";
            //if (ddl_collegename.Items.Count > 0)
            //{
            //    clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            //}
            string sqlcmddesignation = "SELECT Desig_Code,Desig_Name FROM Desig_Master  order by Desig_Name";
            ds = d2.select_method_wo_parameter(sqlcmddesignation,"text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree2.DataSource = ds;
                cbl_degree2.DataTextField = "desig_name";
                cbl_degree2.DataValueField = "desig_code";
                cbl_degree2.DataBind();
                if (cbl_degree2.Items.Count > 0)
                {
                    for (int ro = 0; ro < cbl_degree2.Items.Count; ro++)
                    {
                        cbl_degree2.Items[ro].Selected = true;
                        cb_degree2.Checked = true;
                    }
                    txt_degree2.Text = "Designation(" + cbl_degree2.Items.Count + ")";
                }
            }
        }
        catch
        {

        }
    }
}
