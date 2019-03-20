using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

//Last Modified By Jeyaprakash on Jan 23rd,2017 Add Scheme by Admission Number

public partial class SchemeAdmission : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string clgcode = string.Empty;
    static string btyr = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods reuse = new ReuasableMethods();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                bindcollege();
                radApplNo.Checked = true;
                radAdmNo.Checked = false;
                lblappno.Text = "Application No";
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                    clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
                }
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                clgcode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }

    protected void bindcollege()
    {
        ddlcollege.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollege);
    }
    #region ddl reason
    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnplus_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Scheme";
        lblerror.Visible = false;
    }
    protected void btnminus_Click(object sender, EventArgs e)
    {
        if (ddl_reason.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
        else if (ddl_reason.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select any record";
        }
        else if (ddl_reason.SelectedIndex != 0)
        {
            btn_del.Visible = true;
            btn_ok.Visible = true;
            lbl_del.Text = "Do You Want Delete The Record";
            alertdel.Visible = true;
            lbl_del.Visible = true;
            btn_ok.Text = "Cancel";
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
    }
    public void btn_del_Click(object sender, EventArgs e)
    {
        if (ddl_reason.SelectedIndex != 0)
        {
            string sql = "delete from textvaltable where TextCode='" + ddl_reason.SelectedItem.Value.ToString() + "' and TextCriteria='Schm' and college_code='" + collegecode1 + "' ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {

                alertdel.Visible = true;
                btn_del.Visible = false;
                btn_ok.Visible = true;
                lbl_del.Text = "Deleted Sucessfully";
                lbl_del.Visible = true;
                btn_ok.Text = "OK";
            }
            else
            {
                alertdel.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "No records found";
            }

            loaddesc();
        }

    }
    public void btn_ok_Click(object sender, EventArgs e)
    {
        alertdel.Visible = false;
        lbl_del.Visible = false;
        btn_ok.Visible = false;

    }
    public void loaddesc()
    {
        ddl_reason.Items.Clear();
        ds.Tables.Clear();
        string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='Schm' and college_code ='" + collegecode1 + "'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_reason.DataSource = ds;
            ddl_reason.DataTextField = "TextVal";
            ddl_reason.DataValueField = "TextCode";
            ddl_reason.DataBind();
            ddl_reason.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        else
        {
            ddl_reason.Items.Insert(0, new ListItem("--Select--", "0"));
        }

    }
    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_addgroup.Text != "")
            {
                string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_addgroup.Text + "' and TextCriteria ='Schm' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txt_addgroup.Text + "' where TextVal ='" + txt_addgroup.Text + "' and TextCriteria ='Schm' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_addgroup.Text + "','Schm','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved sucessfully";
                    txt_addgroup.Text = "";
                    plusdiv.Visible = false;
                    panel_addgroup.Visible = false;
                }
                loaddesc();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Enter the description";
            }
        }

        catch
        {
        }
    }
    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }
    #endregion

    protected void txtappl_Changed(object sender, EventArgs e)
    {
        try
        {
            string applno = string.Empty;
            string SelQ = string.Empty;
            if (radApplNo.Checked == true)
                applno = Convert.ToString(txtappl.Text);
            else if (radAdmNo.Checked == true)
                applno = Convert.ToString(txtappl.Text);
            if (!string.IsNullOrEmpty(applno))
            {
                if (radApplNo.Checked == true)
                {
                    SelQ = " select app_no,app_formno,stud_name,a.degree_code,batch_year,a.college_code,c.Course_Name,dt.Dept_Name from applyn a,Degree d,Department dt,Course c where a.degree_code=d.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and app_formno='" + applno + "' and isnull(is_enroll,'0')<>'2' and a.college_code='" + collegecode1 + "' ";
                }
                else if (radAdmNo.Checked == true)
                {
                    SelQ = " select r.app_no,app_formno,r.stud_name,a.degree_code,r.batch_year,a.college_code,c.Course_Name,dt.Dept_Name from applyn a,Degree d,Department dt,Course c,Registration r where a.degree_code=d.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No=a.app_no and CC=0 and DelFlag=0 and r.Exam_Flag<>'Debar' and r.Roll_Admit='" + applno + "' and a.college_code='" + collegecode1 + "'";
                }
                //and isnull(Admission_Status,'0')<>'1' and isnull(selection_status,'0')<>'1'
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelQ, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lbstudname.Text = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]);
                    lbappno.Text = Convert.ToString(ds.Tables[0].Rows[0]["app_no"]);
                    lbscltype.Text = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);
                    lbstand.Text = Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]);
                    lbldegree.Text = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]);
                    lbyear.Text = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                    btyr = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                    lblclgcode.Text = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                    collegecode1 = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                    // Session["collegecode"] = collegecode1;
                    txtamount.Text = string.Empty;
                    tddet.Visible = true;
                    if (radAdmNo.Checked == true)
                        trAdm.Visible = false;
                    else if (radApplNo.Checked == true)
                        trAdm.Visible = true;
                    loaddesc();
                }
                else
                {
                    tddet.Visible = false;
                    txtappl.Text = string.Empty;
                    imgdiv2.Visible = true;
                    if (radApplNo.Checked == true)
                        lbl_alert.Text = "Please Enter Valid Application No";
                    else if (radAdmNo.Checked == true)
                        lbl_alert.Text = "Please Enter Valid Admission No";
                }
            }
            else
            {
                tddet.Visible = false;
                txtappl.Text = string.Empty;
                imgdiv2.Visible = true;
                if (radApplNo.Checked == true)
                    lbl_alert.Text = "Please Enter Valid Application No";
                else if (radAdmNo.Checked == true)
                    lbl_alert.Text = "Please Enter Valid Admission No";
            }
        }
        catch { }
    }

    protected void radApplNo_Change(object sender, EventArgs e)
    {
        bindcollege();
        lblappno.Text = "Application No";
        tddet.Visible = false;
        txtappl.Text = "";
    }

    protected void radAdmNo_Change(object sender, EventArgs e)
    {
        bindcollege();
        lblappno.Text = "Admission No";
        tddet.Visible = false;
        txtappl.Text = "";
    }

    protected void btnadmit_Click(object sender, EventArgs e)
    {
        if (radApplNo.Checked == true)
        {
            admitDetails();
        }
        else if (radAdmNo.Checked == true)
        {
            RollAdmitDetails();
        }
    }

    protected void admitDetails()
    {
        bool save = false;
        string applno = Convert.ToString(txtappl.Text);
        string appNo = Convert.ToString(lbappno.Text);
        string degCode = Convert.ToString(lbldegree.Text);
        string Year = Convert.ToString(lbyear.Text);
        string collegecode = Convert.ToString(lblclgcode.Text);
        string Scheme = string.Empty;
        if (ddl_reason.Items.Count > 0 && ddl_reason.SelectedItem.Text != "--Select--")
            Scheme = Convert.ToString(ddl_reason.SelectedItem.Value);
        double Amount = 0;
        double.TryParse(Convert.ToString(txtamount.Text), out Amount);
        string admissionNo = string.Empty;
        if (cbincadmis.Checked)
            admissionNo = Convert.ToString(txtadmno.Text);

        if (validation(applno, degCode, Year, Scheme, Amount, admissionNo))
        {
            save = admitToRegistration(appNo, collegecode, degCode, Scheme, Amount, admissionNo);
            if (save == true)
            {
                Clear();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Admitted Successfully";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Fill The All Field";
        }

    }

    private void RollAdmitDetails()
    {
        try
        {
            string admissionNo = Convert.ToString(txtappl.Text);
            string appNo = Convert.ToString(lbappno.Text);
            string degCode = Convert.ToString(lbldegree.Text);
            string Year = Convert.ToString(lbyear.Text);
            string collegecode = Convert.ToString(lblclgcode.Text);
            string Scheme = string.Empty;
            if (ddl_reason.Items.Count > 0 && ddl_reason.SelectedItem.Text != "--Select--")
                Scheme = Convert.ToString(ddl_reason.SelectedItem.Value);
            double Amount = 0;
            double.TryParse(Convert.ToString(txtamount.Text), out Amount);

            if (!string.IsNullOrEmpty(admissionNo) && !string.IsNullOrEmpty(degCode) && !string.IsNullOrEmpty(Year) && !string.IsNullOrEmpty(Scheme) && Amount != 0)
            {
                string InsQ = "update Registration set IsSchemeAdmission='1',IsSchemeCode='" + Scheme + "',IsSchemeAmount='" + Amount + "' where App_No='" + appNo + "' and Roll_Admit='" + admissionNo + "' and college_code='" + collegecode + "'";
                int s = d2.update_method_wo_parameter(InsQ, "Text");
                if (s > 0)
                {
                    Clear();
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Scheme Details Updated Successfully";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Fill The All Field";
            }
        }
        catch { }
    }

    private bool admitToRegistration(string appNo, string collegecode, string degCode, string Scheme, double Amount, string rolladmit)
    {
        bool blAppNo = false;
        try
        {
            #region registration
            if (true)
            {
                //Admit               
                string stud_name = string.Empty;
                string app_fromno = string.Empty;
                string batchYr = string.Empty;
                string Mode = string.Empty;
                string seattype = string.Empty;
                string cursem = string.Empty;
                string selQ = "select seattype,stud_name,app_formno,batch_year,mode,current_semester from applyn where app_no ='" + appNo + "'";
                DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                {
                    seattype = Convert.ToString(dsval.Tables[0].Rows[0]["seattype"]);
                    stud_name = Convert.ToString(dsval.Tables[0].Rows[0]["stud_name"]);
                    app_fromno = Convert.ToString(dsval.Tables[0].Rows[0]["app_formno"]);
                    batchYr = Convert.ToString(dsval.Tables[0].Rows[0]["batch_year"]);
                    Mode = Convert.ToString(dsval.Tables[0].Rows[0]["mode"]);
                    cursem = Convert.ToString(dsval.Tables[0].Rows[0]["current_semester"]);
                }
                if (string.IsNullOrEmpty(Mode))
                    Mode = "1";

                string approve = " update applyn set Admission_Status='1',selection_status='1',is_enroll='2',seattype='" + seattype + "',AdmitedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no ='" + Convert.ToString(appNo) + "'";
                int a = d2.update_method_wo_parameter(approve, "text");

                if (!cbincadmis.Checked)
                {
                    if (admissionNoGeneration() == 1)
                        rolladmit = generateAdmissionNo(collegecode, degCode, batchYr);
                    else
                        rolladmit = app_fromno;
                }
                if (rolladmit.Trim() == "0" || string.IsNullOrEmpty(rolladmit))
                    rolladmit = app_fromno;

                string regEntry = "  if exists(select * from Registration where App_No='" + appNo + "' and isnull(IsSchemeAdmission,'0')='1')  delete from Registration where App_No='" + appNo + "' and isnull(IsSchemeAdmission,'0')='1' insert into Registration (App_No,Adm_Date,Roll_Admit,Roll_No,RollNo_Flag,Reg_No,Stud_Name,Batch_Year,degree_code,college_code,CC,DelFlag,Exam_Flag,Current_Semester,mode,IsSchemeAdmission,IsSchemeCode,IsSchemeAmount)values('" + appNo + "','" + System.DateTime.Now.ToString("yyy/MM/dd") + "','" + rolladmit + "','" + app_fromno + "','1','" + app_fromno + "','" + stud_name + "','" + batchYr + "','" + degCode + "','" + collegecode + "','0','0','OK','" + cursem + "','" + Mode + "','1','" + Scheme + "','" + Amount + "')";
                int s = d2.update_method_wo_parameter(regEntry, "Text");
                blAppNo = true;
            }
            #endregion
        }
        catch { }
        return blAppNo;
    }

    protected int admissionNoGeneration()
    {
        int admitValue = 0;
        int.TryParse(Convert.ToString(d2.GetFunction("select value from Master_Settings where settings ='Admission No Rights' and usercode ='" + usercode + "'")), out admitValue);
        return admitValue;
    }

    private bool validation(string applno, string degCode, string Year, string Scheme, double Amount, string admissionNo)
    {
        bool check = false;
        if (!string.IsNullOrEmpty(applno) && !string.IsNullOrEmpty(degCode) && !string.IsNullOrEmpty(Year) && !string.IsNullOrEmpty(Scheme) && Amount != 0)
        {
            if (cbincadmis.Checked)
            {
                if (!string.IsNullOrEmpty(admissionNo))
                    check = true;
                else
                    check = false;
            }
            else
                check = true;
        }
        return check;

    }

    #region admission no generation
    protected double collegewiseapplicationRights(string collegecode)
    {
        double RightsCode = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select linkvalue from New_InsSettings where LinkName='CollegewiseAdmissionNoRights' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'")), out RightsCode);
        return RightsCode;
    }

    private string generateAdmissionNo(string collegecode, string degreecode, string ddl_batch)
    {
        string orginalapplication_number = "";
        try
        {
            Hashtable hat = new Hashtable();
            hat.Add(1, "0");
            hat.Add(2, "00");
            hat.Add(3, "000");
            hat.Add(4, "0000");
            hat.Add(5, "00000");
            hat.Add(6, "000000");
            hat.Add(7, "0000000");
            hat.Add(8, "00000000");
            hat.Add(9, "000000000");
            hat.Add(10, "0000000000");
            bool check = false;
            int application_No = 0;
            string appCodetemp = string.Empty;
            string selectquery = string.Empty;
            if (collegewiseapplicationRights(collegecode) == 1)
            {
                appCodetemp = d2.GetFunction("select appcode from code_generation where  batch_year='" + ddl_batch + "' and college_code='" + collegecode + "' and app_code_flag ='1' and isnull(iscollege,'0')='1'");
                selectquery = "select top 1 r.roll_admit  from Registration r,applyn a where  a.app_no=r.App_No and isnull(a.is_enroll,0) = '2' and  r.roll_admit <>'' and r.roll_admit like '%" + appCodetemp + "%' and r.batch_year='" + ddl_batch + "'  and r.college_code='" + collegecode + "' order by r.roll_admit desc";

                selectquery = selectquery + " select appcode,app_startwith,app_serial from code_generation where  batch_year='" + ddl_batch + "' and college_code='" + collegecode + "'  and app_code_flag ='1' and isnull(iscollege,'0')='1'";
            }
            else
            {
                appCodetemp = d2.GetFunction("select appcode from code_generation where  batch_year='" + ddl_batch + "' and degree_code='" + degreecode + "' and college_code='" + collegecode + "' and app_code_flag ='1'");
                selectquery = "select top 1 r.roll_admit  from Registration r,applyn a where  a.app_no=r.App_No and isnull(a.is_enroll,0) = '2' and  r.roll_admit <>'' and r.roll_admit like '%" + appCodetemp + "%' and r.batch_year='" + ddl_batch + "' and r.degree_code='" + degreecode + "' and r.college_code='" + collegecode + "' order by r.roll_admit desc";
                selectquery = selectquery + " select appcode,app_startwith,app_serial from code_generation where  batch_year='" + ddl_batch + "' and degree_code='" + degreecode + "' and college_code='" + collegecode + "'  and app_code_flag ='1'";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                // application_No = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                string applno = Convert.ToString(ds.Tables[0].Rows[0][0]);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    string appcode = Convert.ToString(ds.Tables[1].Rows[0]["appcode"]);
                    string appsiz2 = Convert.ToString(ds.Tables[1].Rows[0]["app_serial"]);
                    int len = appcode.Length;
                    applno = applno.Remove(0, len);
                    string newnumber = Convert.ToString((Convert.ToInt32(applno) + 1));
                    int val = newnumber.Length;
                    if (val == Convert.ToInt32(appsiz2))
                        orginalapplication_number = appcode + "" + newnumber;
                    else
                    {
                        int remain = Convert.ToInt32(appsiz2) - val;
                        string addnumber = Convert.ToString(hat[remain]);
                        addnumber = addnumber + "" + newnumber;
                        orginalapplication_number = appcode + "" + addnumber;
                    }
                }
                else
                    check = true;
            }
            else
                check = true;

            if (check && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                string appcode = Convert.ToString(ds.Tables[1].Rows[0]["appcode"]);
                string appsiz2 = Convert.ToString(ds.Tables[1].Rows[0]["app_startwith"]);
                int len = appsiz2.Length;
                if (len == Convert.ToInt32(ds.Tables[1].Rows[0]["app_serial"]))
                {
                    orginalapplication_number = appcode + "" + appsiz2;
                }
                else
                {
                    int remain = Convert.ToInt32(ds.Tables[1].Rows[0]["app_serial"]) - len;
                    string addnumber = Convert.ToString(hat[remain]);
                    addnumber = addnumber + "" + appsiz2;
                    orginalapplication_number = appcode + "" + addnumber;
                }
            }
        }
        catch { }
        return orginalapplication_number;
    }
    #endregion

    protected void btnclear_Click(object sender, EventArgs e)
    {
        Clear();
    }
    private void Clear()
    {
        txtappl.Text = string.Empty;
        lbstudname.Text = string.Empty;
        lbappno.Text = string.Empty;
        lbscltype.Text = string.Empty;
        lbstand.Text = string.Empty;
        lbldegree.Text = string.Empty;
        lbyear.Text = string.Empty;
        lblclgcode.Text = string.Empty;
        ddl_reason.SelectedIndex = 0;
        txtamount.Text = string.Empty;
        txtadmno.Text = string.Empty;
        btyr = string.Empty;
        tddet.Visible = false;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    //added by sudhagar 27.12.2016
    protected void cbincamdis_Changed(object sender, EventArgs e)
    {
        if (cbincadmis.Checked)
        {
            txtadmno.Enabled = true;
            txtadmno.Text = string.Empty;
        }
        else
        {
            txtadmno.Enabled = false;
            txtadmno.Text = string.Empty;
        }
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lblclg);
        fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    [WebMethod]
    public static string applicationNo(string applno)
    {
        string Value = "1";
        try
        {
            if (!string.IsNullOrEmpty(applno) && !string.IsNullOrEmpty(btyr))
            {
                DAccess2 da = new DAccess2();
                string applValue = da.GetFunction(" select top 1 r.roll_admit  from Registration r,applyn a where  a.app_no=r.App_No and isnull(a.is_enroll,0) = '2' and  r.roll_admit <>'' and r.roll_admit like '%" + applno + "%' and r.batch_year='" + btyr + "'  and r.college_code='" + clgcode + "' order by r.roll_admit desc");
                if (string.IsNullOrEmpty(applValue) || applValue == "0" || applValue == "-1")
                    Value = "0";
            }
            else
                Value = "2";
        }
        catch (SqlException ex) { Value = "error" + ex.ToString(); }
        return Value;
    }
}