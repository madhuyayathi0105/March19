using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using Gios.Pdf;

using System.IO;
using System.Configuration;

public partial class Individual_StudentFeeStatus : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    static string collegecode1 = string.Empty;
    string usercode = string.Empty;

    static int personmode = 0;
    static int chosedmode = 0;
    bool usBasedRights = false;
    DataTable dtIndividualReport = new DataTable();
    DataRow drowInst;
    ArrayList arrColHdrNames = new ArrayList();
    static Hashtable StudwiseRowCnt = new Hashtable();
    Dictionary<string, string> dicColSpan = new Dictionary<string, string>();
    Dictionary<int, string> dicColSpanFormat1 = new Dictionary<int, string>();
    static Dictionary<int, string> dicColvisible = new Dictionary<int, string>();
    static Dictionary<int, string> dicColAlignment = new Dictionary<int, string>();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            collegecode = Convert.ToString(Session["collegecode"]);
            // collegecode1 = Convert.ToString(Session["collegecode"]);
            usercode = Convert.ToString(Session["usercode"]);

            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddl_collegename.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
                }
                bindgrouphdr();
                bindheader();
                bindsem();
                LoadFromSettings();
                ddl_hdr_OnSelectedIndexChanged(sender, e);

                ddltype.SelectedIndex = 2;
                lbl_hdr.Text = "Group Header";
                rprint.Visible = false;
                UserbasedRights();
                rbstudtype_Selected(sender, e);
                chklsfyear.Items.Clear();
                tdlblfnl.Visible = false;
                tdfnl.Visible = false;
                if (checkSchoolSetting() == 0)
                {
                    loadfinanceyear();
                }
            }
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
        }
        catch (Exception) { }
    }

    #region college
    public void loadcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ddl_collegename.Items.Clear();
            ds.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        { }
    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_collegename.Items.Count > 0)
        {
            collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
        }
        bindgrouphdr();
        bindheader();
        bindsem();
        LoadFromSettings();
        ddl_hdr_OnSelectedIndexChanged(sender, e);
    }
    #endregion

    #region financial year Added by saranya on 08/02/2018

    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            //string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate from FM_FinYearMaster where CollegeCode in('" + collegecode + "')";
            ds.Dispose();
            ds.Reset();
            chkfyear.Checked = false;
            chklsfyear.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
                    // string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    chklsfyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, fdatye));
                }
                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = true;
                    fnalyr = Convert.ToString(chklsfyear.Items[i].Text);
                }
                if (chklsfyear.Items.Count == 1)
                    txtfyear.Text = "" + fnalyr + "";
                else
                    txtfyear.Text = "Finance Year(" + (chklsfyear.Items.Count) + ")";
                // txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
                chkfyear.Checked = true;
                tdlblfnl.Visible = true;
                tdfnl.Visible = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chklsfyear_selected(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");

    }

    protected void chkfyear_changed(object sender, EventArgs e)
    {
        CallCheckboxChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
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
            string name = "";
            cb.Checked = false;
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

    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }

    #endregion

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();
            if (personmode == 0)
            {
                //student query
                if (chosedmode == 0)
                {
                    //query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecode1 + " order by Roll_No asc";

                    query = "select top 100 Roll_No from Registration where Roll_No like '" + prefixText + "%' and college_code=" + collegecode1 + " order by Roll_No asc";

                }
                else if (chosedmode == 1)
                {
                    //query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Reg_No asc";
                    query = "select  top 100 Reg_No from Registration where Reg_No like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where Roll_admit like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Roll_admit asc";

                    //query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Roll_admit asc";

                }
                else if (chosedmode == 4)
                {
                    query = "select  top 100 Stud_Name+'-'+Roll_No+'-'+(select c.Course_Name+'-'+dept_name from Department dt,Degree d,course c where c.Course_Id=d.Course_Id and dt.Dept_Code =d.Dept_Code and d.Degree_Code=r.degree_code) as Roll_admit from Registration r where Stud_Name like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Roll_admit asc";

                    //query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by Roll_admit asc";
                }
                else
                {
                    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by app_formno asc";
                }
            }
            else if (personmode == 1)
            {
                query = " select top 100 staff_code from staffmaster where resign<>1 and staff_code like '" + prefixText + "%' and college_code=" + collegecode1 + " order by staff_code asc";

                //staff query

            }
            else if (personmode == 2)
            {
                //Vendor query
            }
            else
            {
                //Others query
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    protected void Logout_btn_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    protected void cb_header_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            txtheader.Text = "--Select--";
            if (cb_header.Checked == true)
            {
                for (int i = 0; i < cbl_header.Items.Count; i++)
                {
                    cbl_header.Items[i].Selected = true;
                }
                if (lbl_hdr.Text == "Group Header")
                {
                    txtheader.Text = "Group Header(" + cbl_header.Items.Count + ")";
                }
                if (lbl_hdr.Text == "Header")
                {
                    txtheader.Text = "Header(" + cbl_header.Items.Count + ")";
                }
                if (lbl_hdr.Text == "Ledger")
                {
                    txtheader.Text = "Ledger(" + cbl_header.Items.Count + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbl_header.Items.Count; i++)
                {
                    cbl_header.Items[i].Selected = false;
                }
                txtheader.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void cbl_header_selectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txtheader.Text = "--Select--";
            cb_header.Checked = false;

            for (int i = 0; i < cbl_header.Items.Count; i++)
            {
                if (cbl_header.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_header.Items.Count)
                {
                    cb_header.Checked = true;
                }
                if (lbl_hdr.Text == "Group Header")
                {
                    txtheader.Text = "Header(" + commcount.ToString() + ")";
                }
                if (lbl_hdr.Text == "Header")
                {
                    txtheader.Text = "Header(" + commcount.ToString() + ")";
                }
                if (lbl_hdr.Text == "Ledger")
                {
                    txtheader.Text = "Ledger(" + commcount.ToString() + ")";
                }
            }
        }
        catch
        { }
    }

    protected void cb_fee_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            txtfee.Text = "--Select--";
            if (cb_fee.Checked == true)
            {
                for (int i = 0; i < cbl_fee.Items.Count; i++)
                {
                    cbl_fee.Items[i].Selected = true;
                }
                txtfee.Text = "Category(" + cbl_fee.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl_fee.Items.Count; i++)
                {
                    cbl_fee.Items[i].Selected = false;
                }
                txtfee.Text = "--Select--";
            }
        }
        catch { }
    }

    protected void cbl_fee_selectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txtfee.Text = "--Select--";
            cb_fee.Checked = false;

            for (int i = 0; i < cbl_fee.Items.Count; i++)
            {
                if (cbl_fee.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_fee.Items.Count)
                {
                    cb_fee.Checked = true;
                }
                txtfee.Text = "Category(" + commcount.ToString() + ")";
            }
        }
        catch
        { }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdIndividualReport.Visible = false;
        //btnprintmaster.Visible = false;
        Error.Visible = false;
        txtno.Text = "";
        if (ddltype.SelectedIndex == 0)
        {
            Error.Visible = false;
            lblnum.Text = "Enq No";
            ddladmit.Visible = false;
        }
        else if (ddltype.SelectedIndex == 1)
        {
            Error.Visible = false;
            lblnum.Text = "App No";
            ddladmit.Visible = false;
        }
        else if (ddltype.SelectedIndex == 2)
        {
            Error.Visible = false;
            lblnum.Text = "Roll No";
            ddladmit.Visible = true;
        }
    }

    protected void ddladmit_SelectedIndexChanged(object sender, EventArgs e)
    {
        Error.Visible = false;
        grdIndividualReport.Visible = false;
        //btnprintmaster.Visible = false;
        txtno.Text = "";
        lblnum.Text = ddladmit.SelectedItem.ToString();

        switch (Convert.ToUInt32(ddladmit.SelectedItem.Value))
        {
            case 0:
                txtno.Attributes.Add("placeholder", "Roll No");
                chosedmode = 0;
                break;
            case 1:
                txtno.Attributes.Add("placeholder", "Reg No");
                chosedmode = 1;
                break;
            case 2:
                txtno.Attributes.Add("placeholder", "Admin No");
                chosedmode = 2;
                break;
            case 3:
                txtno.Attributes.Add("placeholder", "App No");
                chosedmode = 3;
                break;
            case 4:
                txtno.Attributes.Add("placeholder", "");
                chosedmode = 4;
                break;
        }


    }

    protected void btngo_click(object sender, EventArgs e)
    {

        if (studstaffid.SelectedIndex == 0)
        {
            if (ddlViewFormat.SelectedIndex == 0)
            {
                if (!cbpaymode.Checked)
                {
                    viewFormat1();
                    btnprint.Visible = false;
                }
                else
                {
                    viewFormatPaymode();
                    btnprint.Visible = false;
                }
            }
            else if (ddlViewFormat.SelectedIndex == 1)
            {
                viewFormat2();
                btnprint.Visible = false;
            }
            else if (ddlViewFormat.SelectedIndex == 2)
            {
                if (checkSchoolSetting() != 0)
                {
                    StudwiseRowCnt.Clear();
                    loadFormatNew3();
                    btnprint.Visible = true;
                }
                //Added by saranya on 09/02/2018 for School setting
                if (checkSchoolSetting() == 0)
                {
                    StudwiseRowCnt.Clear();
                    loadFormatNewSchool();
                }
            }
        }
        else
        {
            NewStaffFormat();
        }
    }

    //added by sudhagar include old student transfer record get  

    protected string getTransferAppNo(string collegcode, string rollNo)
    {
        string appNo = string.Empty;
        try
        {
            DataSet dsload = new DataSet();
            string SeleQ = " select r.app_no,r.stud_name,r.roll_no,r.reg_no,r.roll_admit,r.batch_year,st.FromDegree,st.Todegree,st.FromSection,st.ToSection,st.FromCollege,st.Tocollege,st.fromseattype,st.ToSeatType,convert(varchar(10),st.TransferDate,103)as TransferDate,convert(varchar(10),r.adm_date,103)as adm_date,Old_RollNo,Old_RegNo,Old_RollAdmit,Old_ReceiptNo,convert(varchar(10),Old_ReceiptDate,103)as Old_ReceiptDate,Old_Amt,New_ReceiptNo,convert(varchar(10),New_ReceiptDate,103)as New_ReceiptDate,New_Amt,New_ExcessAmt from registration r,st_student_transfer st,ST_Student_Transfer_Details std where r.app_no=st.appno and st.StudentTransferPK=std.StudentTransferfK and FromCollege in('" + collegcode + "') and Old_RollNo='" + rollNo + "' ";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SeleQ, "Text");
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count == 0)
            {
                SeleQ = " select r.app_no,r.stud_name,r.roll_no,r.reg_no,r.roll_admit,r.batch_year,st.FromDegree,st.Todegree,st.FromSection,st.ToSection,st.FromCollege,st.Tocollege,st.fromseattype,st.ToSeatType,convert(varchar(10),st.TransferDate,103)as TransferDate,convert(varchar(10),r.adm_date,103)as adm_date,Old_RollNo,Old_RegNo,Old_RollAdmit,Old_ReceiptNo,convert(varchar(10),Old_ReceiptDate,103)as Old_ReceiptDate,Old_Amt,New_ReceiptNo,convert(varchar(10),New_ReceiptDate,103)as New_ReceiptDate,New_Amt,New_ExcessAmt from registration r,st_student_transfer st,ST_Student_Transfer_Details std where r.app_no=st.appno and st.StudentTransferPK=std.StudentTransferfK and FromCollege in('" + collegcode + "') and Old_RegNo='" + rollNo + "' ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(SeleQ, "Text");
            }
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count == 0)
            {
                SeleQ = " select r.app_no,r.stud_name,r.roll_no,r.reg_no,r.roll_admit,r.batch_year,st.FromDegree,st.Todegree,st.FromSection,st.ToSection,st.FromCollege,st.Tocollege,st.fromseattype,st.ToSeatType,convert(varchar(10),st.TransferDate,103)as TransferDate,convert(varchar(10),r.adm_date,103)as adm_date,Old_RollNo,Old_RegNo,Old_RollAdmit,Old_ReceiptNo,convert(varchar(10),Old_ReceiptDate,103)as Old_ReceiptDate,Old_Amt,New_ReceiptNo,convert(varchar(10),New_ReceiptDate,103)as New_ReceiptDate,New_Amt,New_ExcessAmt from registration r,st_student_transfer st,ST_Student_Transfer_Details std where r.app_no=st.appno and st.StudentTransferPK=std.StudentTransferfK  and FromCollege in('" + collegcode + "') and Old_RollAdmit='" + rollNo + "' ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(SeleQ, "Text");
            }
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            {
                appNo = Convert.ToString(dsload.Tables[0].Rows[0]["app_no"]);
            }
        }
        catch { }
        return appNo;
    }

    protected Dictionary<int, string> getPaymode(List<string> stduAppLst, string type, string hdFK, string feecat)
    {
        Dictionary<int, string> payMode = new Dictionary<int, string>();
        try
        {
            string appNo = string.Empty;
            StringBuilder studAppNo = new StringBuilder();
            if (stduAppLst.Count > 0)
            {
                foreach (string appLst in stduAppLst)
                {
                    studAppNo.Append(appLst + "','");
                }
                if (studAppNo.Length > 0)
                {
                    studAppNo.Remove(studAppNo.Length - 3, 3);
                    appNo = Convert.ToString(studAppNo);
                }
            }
            string strHeader = string.Empty;

            if (lbl_hdr.Text == "Header")
                strHeader = " and headerfk in('" + hdFK + "')";
            else if (lbl_hdr.Text == "Ledger")
                strHeader = " and ledgerfk in('" + hdFK + "')";

            string selQ = " select distinct case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end paymode,paymode as paymodeval from ft_findailytransaction where app_no in('" + appNo + "') " + strHeader + " and feecategory in('" + feecat + "','0')  order by paymode";
            if (lbl_hdr.Text == "Group Header")
            {
                selQ = " select distinct case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end paymode,paymode as paymodeval from ft_findailytransaction f,FS_ChlGroupHeaderSettings g where f.headerfk=g.headerfk app_no in('" + appNo + "')  and feecategory in('" + feecat + "','0') and g.ChlGroupHeader in('" + hdFK + "')  order by paymode";
            }
            DataSet dsload = d2.select_method_wo_parameter(selQ, "Text");
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsload.Tables[0].Rows.Count; row++)
                {
                    string paymodeStr = Convert.ToString(dsload.Tables[0].Rows[row]["paymode"]);
                    int paymodeVal = 0;
                    Int32.TryParse(Convert.ToString(dsload.Tables[0].Rows[row]["paymodeval"]), out paymodeVal);
                    if (!payMode.ContainsKey(paymodeVal))
                    {
                        payMode.Add(paymodeVal, paymodeStr);
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return payMode;
    }

    private void viewFormatPaymode()
    {
        try
        {
            dicColvisible.Clear();
            dicColAlignment.Clear();
            UserbasedRights();
            Error.Visible = false;
            string headerid = "";
            string semcode = "";
            int headercount = 0;
            double feeamount = 0;
            double allottot = 0.0;
            double paidtot = 0.0;
            double baltot = 0.0;
            double grandfeeamount = 0;
            double grandalltot = 0.0;
            double grandpaidtot = 0.0;
            double grandbaltot = 0.0;
            double DeductTotal = 0.0;
            double grandDeductTotal = 0.0;
            string AppNo = "";
            string type = "";
            int rowCnt = 0;
            int ColumnCnt = 0;
            Hashtable hscol = new Hashtable();
            hscol.Clear();
            Hashtable hshead = new Hashtable();
            hshead.Clear();
            Hashtable hschkcol = new Hashtable();
            hschkcol.Clear();
            List<string> rolllist = new List<string>();
            bool beforeAdm = false;
            string transerText = string.Empty;
            string rollType = string.Empty;
            if (ddl_collegename.Items.Count > 0)
                collegecode1 = Convert.ToString(ddl_collegename.SelectedValue);
            if (!cbTrans.Checked)
            {
                #region without transfer
                transerText = " and istransfer='0'";
                if (rbstudtype.SelectedItem.Value == "1")
                {
                    if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 0)
                    {
                        //roll no
                        AppNo = d2.GetFunction("select App_No from Registration where Roll_No='" + txtno.Text + "' and college_code='" + collegecode1 + "' ");
                        if (AppNo != "0")
                            rolllist.Add(AppNo);
                        rollType = " r.roll_no";
                        //and cc=0 and DelFlag=0 and Exam_Flag<>'debar'
                    }
                    else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 1)
                    {
                        //reg no
                        AppNo = d2.GetFunction("select App_No from Registration where Reg_No='" + txtno.Text + "' and college_code='" + collegecode1 + "'");
                        if (AppNo != "0")
                            rolllist.Add(AppNo);
                        rollType = " r.Reg_No";
                    }
                    else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 2)
                    {
                        //Admin no
                        AppNo = d2.GetFunction("select App_No from Registration where Roll_admit='" + txtno.Text + "' and college_code='" + collegecode1 + "'");
                        if (AppNo != "0")
                            rolllist.Add(AppNo);
                        rollType = " r.Roll_admit";
                    }
                    else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 3)
                    {
                        //Admin no
                        AppNo = d2.GetFunction("select App_No from applyn where app_formno='" + txtno.Text + "' and college_code='" + collegecode1 + "'");
                        if (AppNo != "0")
                            rolllist.Add(AppNo);
                        rollType = " r.app_formno";
                        beforeAdm = true;
                    }
                    else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 4)
                    {
                        //Admin no
                        AppNo = d2.GetFunction("select App_No from Registration where Roll_no='" + txtno.Text.Split('-')[1] + "' and college_code='" + collegecode1 + "'");
                        if (AppNo != "0")
                            rolllist.Add(AppNo);
                    }
                    //  rollType = " r.Roll_admit";
                }
                else
                {
                    string rollMult = Convert.ToString(lblrolldisp.Text);
                    if (rollMult != "")
                    {
                        string[] roll = rollMult.Split(',');
                        if (roll.Length > 0)
                        {
                            for (int i = 0; i < roll.Length; i++)
                            {
                                string app_no = d2.GetFunction("select App_No from Registration where Roll_No='" + Convert.ToString(roll[i]) + "' and college_code='" + collegecode1 + "'");
                                if (app_no != "0")
                                    rolllist.Add(app_no);
                                rollType = " r.Roll_No";
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                transerText = " and istransfer='0'";
                AppNo = getTransferAppNo(collegecode1, txtno.Text.Trim());
                if (AppNo != "0")
                    rolllist.Add(AppNo);
            }

            #region get value

            if (ddl_hdr.SelectedIndex == 1)
            {
                if (cbl_header.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        if (cbl_header.Items[i].Selected == true)
                        {
                            headercount++;
                            if (headerid.Trim() == "")
                                headerid = "" + Convert.ToString(cbl_header.Items[i].Value) + "";
                            else
                                headerid = headerid + "'" + "," + "'" + Convert.ToString(cbl_header.Items[i].Value);
                        }
                    }
                }
            }

            if (ddl_hdr.SelectedIndex == 0)
            {
                if (cbl_header.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        if (cbl_header.Items[i].Selected == true)
                        {
                            if (headerid.Trim() == "")
                                headerid = "" + Convert.ToString(cbl_header.Items[i].Text) + "";
                            else
                                headerid = headerid + "'" + "," + "'" + Convert.ToString(cbl_header.Items[i].Text);
                        }
                    }
                }
            }

            if (ddl_hdr.SelectedIndex == 2)
            {
                if (cbl_header.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        if (cbl_header.Items[i].Selected == true)
                        {
                            if (headerid.Trim() == "")
                                headerid = "" + Convert.ToString(cbl_header.Items[i].Value) + "";
                            else
                                headerid = headerid + "'" + "," + "'" + Convert.ToString(cbl_header.Items[i].Value);
                        }
                    }
                }
            }

            if (cbl_fee.Items.Count > 0)
            {
                for (int i = 0; i < cbl_fee.Items.Count; i++)
                {
                    if (cbl_fee.Items[i].Selected == true)
                    {
                        if (semcode.Trim() == "")
                            semcode = "" + Convert.ToString(cbl_fee.Items[i].Value) + "";
                        else
                            semcode = semcode + "'" + "," + "'" + Convert.ToString(cbl_fee.Items[i].Value);
                    }
                }
            }

            if (headerid.Trim() == "")
            {
                Error.Visible = true;
                Error.Text = "Please select any Item!";
                return;
            }
            if (semcode.Trim() == "")
            {
                Error.Visible = true;
                Error.Text = "Please select any semester!";
                return;
            }

            #endregion

            //if (AppNo.Trim() != "" && AppNo.Trim() != "0")
            if (rolllist.Count > 0)
            {
                #region design
                int cellCount = 0;
                Dictionary<int, string> payMode = getPaymode(rolllist, lbl_hdr.Text, headerid, semcode);
                arrColHdrNames.Add("S.No");
                dtIndividualReport.Columns.Add("S.No");
                arrColHdrNames.Add("Trans Date");
                dtIndividualReport.Columns.Add("Trans Date");
                arrColHdrNames.Add("Trans Code");
                dtIndividualReport.Columns.Add("Trans Code");
                if (lbl_hdr.Text == "Group Header")
                {
                    arrColHdrNames.Add("Group Header");
                    dtIndividualReport.Columns.Add("Group Header");
                }
                if (lbl_hdr.Text == "Header")
                {
                    arrColHdrNames.Add("Header");
                    dtIndividualReport.Columns.Add("Header");
                }
                if (lbl_hdr.Text == "Ledger")
                {
                    arrColHdrNames.Add("Ledger");
                    dtIndividualReport.Columns.Add("Ledger");
                }
                arrColHdrNames.Add("Fee Amount");
                dtIndividualReport.Columns.Add("Fee Amount");
                arrColHdrNames.Add("Allot");
                dtIndividualReport.Columns.Add("Allot");

                Hashtable htColCnt = new Hashtable();
                foreach (KeyValuePair<int, string> payVal in payMode)
                {
                    cellCount = dtIndividualReport.Columns.Count;
                    htColCnt.Add(Convert.ToString(payVal.Key), cellCount);
                    arrColHdrNames.Add(payVal.Value);
                    dtIndividualReport.Columns.Add(payVal.Value);
                    dicColAlignment.Add(cellCount, "ColumnAlign");
                }
                cellCount = dtIndividualReport.Columns.Count;
                arrColHdrNames.Add("Total Paid");
                dtIndividualReport.Columns.Add("Total Paid");
                dicColAlignment.Add(cellCount, "ColumnAlign");
                cellCount = dtIndividualReport.Columns.Count;
                arrColHdrNames.Add("Balance");
                dtIndividualReport.Columns.Add("Balance");
                dicColvisible.Add(cellCount, "Visible");
                cellCount = dtIndividualReport.Columns.Count;
                arrColHdrNames.Add("Refund");
                dtIndividualReport.Columns.Add("Refund");
                if (!cbRefund.Checked == true)
                    dicColvisible.Add(cellCount, "Visible");
                dicColAlignment.Add(cellCount, "ColumnAlign");
                cellCount = dtIndividualReport.Columns.Count;
                arrColHdrNames.Add("Narration");
                dtIndividualReport.Columns.Add("Narration");
                if (!inclnarr.Checked == true)
                    dicColvisible.Add(cellCount, "Visible");
                dicColAlignment.Add(cellCount, "ColumnAlign");
                cellCount = dtIndividualReport.Columns.Count;
                arrColHdrNames.Add("Concession Amt");
                dtIndividualReport.Columns.Add("Concession Amt");
                if (!cbincdedut.Checked == true)
                    dicColvisible.Add(cellCount, "Visible");
                dicColAlignment.Add(cellCount, "ColumnAlign");
                cellCount = dtIndividualReport.Columns.Count;
                arrColHdrNames.Add("Concession Reason");
                dtIndividualReport.Columns.Add("Concession Reason");
                if (!cbincdedut.Checked == true)
                    dicColvisible.Add(cellCount, "Visible");
                // }
                DataRow drHdr1 = dtIndividualReport.NewRow();
                for (int grCol = 0; grCol < dtIndividualReport.Columns.Count; grCol++)
                    drHdr1[grCol] = arrColHdrNames[grCol];
                dtIndividualReport.Rows.Add(drHdr1);

                string userCode = "";  // Modify Jairam 30.08.2016
                //if (usBasedRights == true)
                //    userCode = " and d.EntryUserCode in('" + usercode + "')";

                #endregion

                for (int rol = 0; rol < rolllist.Count; rol++)
                {
                    AppNo = Convert.ToString(rolllist[rol]);
                    type = d2.GetFunction("select c.type from Registration r,Degree d,Course c where r.degree_code =d.Degree_Code and d.Course_Id =c.Course_Id and App_No ='" + AppNo + "' ");
                    #region Query

                    string selq = "";
                    if (ddl_hdr.SelectedIndex == 0)
                    {
                        if (!beforeAdm)
                        {
                            #region group header
                            //allot detail
                            selq = "  select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  r.App_No=f.App_No and ch.HeaderFK=f.HeaderFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  f.HeaderFK=ch.HeaderFK and r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r where  r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK";

                            //paid detail                   
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + userCode + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.Narration,transtype order by cast(transdate as datetime)";

                            //deduction
                            // selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.LedgerFK in('" + headerid + "') and f.feecategory in('" + semcode + "') group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK ";
                            selq += "  select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t,FS_ChlGroupHeaderSettings G where f.App_No=r.App_No and f.headerfk=g.headerfk and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and g.ChlGroupHeader in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),f.HeaderFK ";

                            //paid detail                   
                            selq = selq + " select SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.paymode,ddno,ddbankcode,isnull(transtype,'0') as transtype  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + userCode + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.paymode,ddno,ddbankcode,transtype";

                            //paid detail                   
                            selq = selq + " select SUM(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype,d.paymode,ddno,ddbankcode  from FS_ChlGroupHeaderSettings G,registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + userCode + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0' and isnull(credit,'0')>'0' " + transerText + " group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype,d.paymode,ddno,ddbankcode";
                            #endregion
                        }
                        else
                        {
                            #region group header
                            //allot detail
                            selq = "  select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from applyn r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  r.App_No=f.App_No and ch.HeaderFK=f.HeaderFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  f.HeaderFK=ch.HeaderFK and r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,v r where  r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK";

                            //paid detail                   
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + userCode + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.Narration,transtype order by cast(transdate as datetime)";

                            selq += "  select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue  from FT_FeeAllot f,applyn r,textvaltable t,FS_ChlGroupHeaderSettings G where f.App_No=r.App_No and f.headerfk=g.headerfk and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and g.ChlGroupHeader in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),f.HeaderFK ";

                            //paid detail                   
                            selq = selq + " select SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.paymode,ddno,ddbankcode,isnull(transtype,'0') as transtype  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + userCode + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.paymode,ddno,ddbankcode, transtype";

                            //paid detail                   
                            selq = selq + " select SUM(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype,d.paymode,ddno,ddbankcode  from FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + userCode + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0' and isnull(credit,'0')>'0' " + transerText + " group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype,d.paymode,ddno,ddbankcode";
                            #endregion
                        }
                    }
                    if (ddl_hdr.SelectedIndex == 1)
                    {
                        if (!beforeAdm)
                        {
                            #region header

                            //allot detail query
                            selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from Registration r,TextValTable t,FT_FeeAllot f where    r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                            selq = selq + "  select distinct (r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where   r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + "  select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue,H.HeaderName as CollName from FT_FeeAllot F,Registration r,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK  and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " group by f.FeeCategory,f.App_No,AllotDate,F.HeaderFK,h.HeaderName";

                            //Paid Detail query                  
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,F.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + "  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,d.Narration,transtype order by cast(transdate as datetime)";

                            //deduction 
                            //selq += " select textval,isnull(SUM(DeductAmout),0) from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headerid + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and FinYearFK in(" + actidquery + ")  group by TextCode,textval";                   

                            selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.headerfk in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK ";

                            //paymode query
                            selq = selq + " select SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,F.HeaderFK as CollValue,H.HeaderName as CollName,d.paymode,ddno,ddbankcode,isnull(transtype,'0') as transtype from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + "  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,d.paymode,ddno,ddbankcode, transtype ";

                            selq += "    select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype,d.paymode,ddno,ddbankcode from Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype,d.paymode,ddno,ddbankcode ";

                            #endregion
                        }
                        else
                        {
                            #region header

                            //allot detail query
                            selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from applyn r,TextValTable t,FT_FeeAllot f where    r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                            selq = selq + "  select distinct (r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f where   r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + "  select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue,H.HeaderName as CollName from FT_FeeAllot F,applyn r,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK  and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " group by f.FeeCategory,f.App_No,AllotDate,F.HeaderFK,h.HeaderName";

                            //Paid Detail query                  
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,F.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + "  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,d.Narration,transtype order by cast(transdate as datetime)";

                            selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue  from FT_FeeAllot f,applyn r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.headerfk in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK ";

                            //Paid Detail query                  
                            selq = selq + " select SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,F.HeaderFK as CollValue,H.HeaderName as CollName,d.paymode,ddno,ddbankcode,isnull(transtype,'0') as transtype from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + "  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,d.paymode,ddno,ddbankcode, transtype";


                            selq += "    select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype,d.paymode,ddno,ddbankcode from applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype,d.paymode,ddno,ddbankcode ";
                            #endregion
                        }
                    }
                    if (ddl_hdr.SelectedIndex == 2)
                    {
                        if (!beforeAdm)
                        {
                            #region ledger

                            //allot details query
                            selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from Registration r,TextValTable t,FT_FeeAllot f where r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                            selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.LedgerFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where  r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue,l.LedgerName as CollName,l.priority from FT_FeeAllot F,Registration r,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "')  " + transerText + " group by f.FeeCategory,f.App_No,AllotDate,F.LedgerFK,l.LedgerName,l.priority order by len(isnull(l.priority,1000)) , l.priority asc ";

                            //Paid details query
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),TransDate,103) as TransDate,TransCode,F.LedgerFK as CollValue,l.LedgerName as CollName,d.Narration,l.priority,isnull(transtype,'0') as transtype from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.Narration,l.priority,transtype order by len(isnull(l.priority,1000)) , l.priority asc,cast(transdate as datetime)";

                            //deduction
                            selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.LedgerFK in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.LedgerFK ";

                            //Paid details query
                            selq = selq + " select SUM(Debit) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),TransDate,103) as TransDate,TransCode,F.LedgerFK as CollValue,l.LedgerName as CollName,d.paymode,ddno,ddbankcode,isnull(transtype,'0') as transtype from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.paymode,ddno,ddbankcode,transtype ";

                            selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.ledgerfk as CollValue,H.ledgername as CollName,d.Narration,isnull(transtype,'0') as transtype,d.paymode,ddno,ddbankcode from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype,d.paymode,ddno,ddbankcode ";
                            #endregion
                        }
                        else
                        {
                            #region ledger

                            //allot details query
                            selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from applyn r,TextValTable t,FT_FeeAllot f where r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                            selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.LedgerFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f where  r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue,l.LedgerName as CollName,l.priority from FT_FeeAllot F,applyn r,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + "  group by f.FeeCategory,f.App_No,AllotDate,F.LedgerFK,l.LedgerName,l.priority order by len(isnull(l.priority,1000)) , l.priority asc ";

                            //Paid details query
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),TransDate,103) as TransDate,TransCode,F.LedgerFK as CollValue,l.LedgerName as CollName,d.Narration,l.priority,isnull(transtype,'0') as transtype from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.Narration,l.priority,transtype order by len(isnull(l.priority,1000)) , l.priority asc,cast(transdate as datetime)";

                            //deduction
                            selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue  from FT_FeeAllot f,applyn r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.LedgerFK in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.LedgerFK ";

                            //Paid details query
                            selq = selq + " select SUM(Debit) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),TransDate,103) as TransDate,TransCode,F.LedgerFK as CollValue,l.LedgerName as CollName,d.paymode,ddno,ddbankcode,isnull(transtype,'0') as transtype from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.paymode,ddno,ddbankcode, transtype";

                            selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.ledgerfk as CollValue,H.ledgername as CollName,d.Narration,isnull(transtype,'0') as transtype,d.paymode,ddno,ddbankcode from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype,d.paymode,ddno,ddbankcode ";
                            #endregion
                        }
                    }
                    selq += " select receiptno,Convert(varchar(10),rcptdate,103) as rcptdate,ledgerfk,app_no,amount from ft_excessReceiptdet where app_no='" + AppNo + "' and excesstype='1' ";
                    selq += " select receiptno,Convert(varchar(10),rcptdate,103) as rcptdate,ledgerfk,app_no,amount from ft_excessReceiptdet where app_no='" + AppNo + "' and excesstype='2' ";
                    //selq += "   select (accno+'-'+bankname) as bankname,bankpk from fm_finbankmaster where collegecode='" + collegecode1 + "'";
                    selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";
                    string selQ = " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode1 + "'";
                    #endregion

                    int sno = 0;
                    int serino = 0;
                    int serialno = 0;
                    DataView dvnew1 = new DataView();
                    DataView dvnew2 = new DataView();
                    DataView dvnew3 = new DataView();
                    DataView dvdt = new DataView();
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selq, "Text");
                    DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                    if (rol != 0)
                    {
                        //FpSpread1.Sheets[0].RowCount++;
                        // FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.White;
                    }
                    Hashtable htGrandTot = new Hashtable();
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            string degreecode = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                            string deptname = string.Empty;
                            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                            {
                                dsval.Tables[0].DefaultView.RowFilter = "degree_code='" + degreecode + "'";
                                DataView dv = dsval.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                    deptname = Convert.ToString(dv[0]["degreename"]);
                            }

                            drowInst = dtIndividualReport.NewRow();
                            drowInst[0] = Convert.ToString(ds.Tables[0].Rows[row]["headername"]) + "-" + deptname + "-" + Convert.ToString(ds.Tables[0].Rows[row]["TextVal"]);
                            rowCnt = dtIndividualReport.Rows.Count;
                            dicColSpanFormat1.Add(rowCnt, "Roll No");
                            dtIndividualReport.Rows.Add(drowInst);
                            ds.Tables[1].DefaultView.RowFilter = "FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]) + "'";
                            allottot = 0.0;
                            baltot = 0.0;
                            paidtot = 0.0;
                            DeductTotal = 0;
                            feeamount = 0;
                            dvnew1 = ds.Tables[1].DefaultView;
                            for (int dv1 = 0; dv1 < dvnew1.Count; dv1++)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "CollValue='" + Convert.ToString(dvnew1[dv1]["CollValue"]) + "' and FeeCategory='" + Convert.ToString(dvnew1[dv1]["FeeCategory"]) + "'";
                                dvnew2 = ds.Tables[2].DefaultView;
                                for (int ik = 0; ik < dvnew2.Count; ik++)
                                {
                                    #region allot

                                    drowInst = dtIndividualReport.NewRow();
                                    sno++;
                                    drowInst[0] = Convert.ToString(sno);
                                    drowInst[1] = Convert.ToString(dvnew2[ik]["TransDate"]);
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvnew2[ik]["TransCode"]);
                                    drowInst[3] = Convert.ToString(dvnew2[ik]["CollName"]);
                                    drowInst[4] = Convert.ToString(dvnew2[ik]["feeamount"]);
                                    feeamount = feeamount + Convert.ToDouble(dvnew2[ik]["feeamount"]);
                                    drowInst[5] = Convert.ToString(dvnew2[ik]["total"]);
                                    allottot = allottot + Convert.ToDouble(dvnew2[ik]["total"]);
                                    drowInst[6] = Convert.ToString(0);
                                    dtIndividualReport.Rows.Add(drowInst);
                                    #endregion
                                }
                                double FNlAmt = 0;
                                string DedutRes = "";
                                if (ds.Tables[4].Rows.Count > 0)
                                {
                                    #region deduction

                                    string val = "CollValue='" + Convert.ToString(dvnew1[dv1]["CollValue"]) + "' and FeeCategory='" + Convert.ToString(dvnew1[dv1]["FeeCategory"]) + "'";
                                    ds.Tables[4].DefaultView.RowFilter = "CollValue='" + Convert.ToString(dvnew1[dv1]["CollValue"]) + "' and FeeCategory='" + Convert.ToString(dvnew1[dv1]["FeeCategory"]) + "'";
                                    dvdt = ds.Tables[4].DefaultView;
                                    if (dvdt.Count > 0 && dvdt != null)
                                    {
                                        for (int i = 0; i < dvdt.Count; i++)
                                        {
                                            double AMt = 0;
                                            double.TryParse(Convert.ToString(dvdt[i]["DeductAmout"]), out AMt);
                                            FNlAmt += AMt;
                                            if (DedutRes == "")
                                                DedutRes = Convert.ToString(dvdt[i]["textval"]);
                                            else
                                                DedutRes = DedutRes + "," + Convert.ToString(dvdt[i]["textval"]);
                                        }
                                        ColumnCnt = dtIndividualReport.Columns.Count;
                                        drowInst[ColumnCnt - 2] = Convert.ToString(FNlAmt);
                                        DeductTotal += FNlAmt;
                                        ColumnCnt = dtIndividualReport.Columns.Count;
                                        drowInst[ColumnCnt - 1] = DedutRes;
                                    }

                                    #endregion
                                }
                            }
                            DataView dvpaid = new DataView();
                            ds.Tables[3].DefaultView.RowFilter = " FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]) + "'";
                            //App_no='" + Convert.ToString(dvnew2[0]["App_no"]) + "' and
                            Hashtable htpayTot = new Hashtable();
                            dvpaid = ds.Tables[3].DefaultView;
                            if (dvpaid.Count > 0 && dvpaid != null)
                            {
                                for (int i = 0; i < dvpaid.Count; i++)
                                {
                                    #region Paid

                                    sno++;
                                    drowInst = dtIndividualReport.NewRow();
                                    drowInst[0] = Convert.ToString(sno);
                                    drowInst[1] = Convert.ToString(dvpaid[i]["TransDate"]);
                                    drowInst[2] = Convert.ToString(dvpaid[i]["TransCode"]);
                                    bool boolEx = false;
                                    string strEx = string.Empty;
                                    if (ds.Tables[8].Rows.Count > 0)//if excess used then print the name
                                    {
                                        DataView dvex = new DataView();
                                        try
                                        {
                                            if (ddl_hdr.SelectedItem.Text.Trim() == "Ledger")
                                            {
                                                ds.Tables[8].DefaultView.RowFilter = "rcptdate='" + Convert.ToString(dvpaid[i]["TransDate"]) + "' and receiptno='" + Convert.ToString(dvpaid[i]["TransCode"]) + "' and ledgerfk='0'";
                                            }
                                            else
                                            {
                                                ds.Tables[8].DefaultView.RowFilter = "rcptdate='" + Convert.ToString(dvpaid[i]["TransDate"]) + "' and receiptno='" + Convert.ToString(dvpaid[i]["TransCode"]) + "' and ledgerfk='0'";
                                            }
                                        }
                                        catch { }
                                        dvex = ds.Tables[8].DefaultView;
                                        if (dvex.Count > 0)
                                        {
                                            boolEx = true;
                                            strEx = "-(Used Excess)";
                                        }
                                    }
                                    drowInst[3] = Convert.ToString(dvpaid[i]["CollName"]) + strEx;
                                    drowInst[5] = Convert.ToString(0);
                                    string transtype = Convert.ToString(dvpaid[i]["transtype"]);
                                    //paymode paid
                                    string bankname = string.Empty;
                                    string ddno = string.Empty;
                                    foreach (KeyValuePair<int, string> payVal in payMode)
                                    {
                                        string strVal = "FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]) + "' and transdate='" + Convert.ToString(dvpaid[i]["TransDate"]) + "' and TransCode='" + Convert.ToString(dvpaid[i]["TransCode"]) + "' and CollValue='" + Convert.ToString(dvpaid[i]["CollValue"]) + "' and paymode='" + payVal.Key + "'";
                                        ds.Tables[5].DefaultView.RowFilter = strVal;
                                        DataTable dtPayMode = ds.Tables[5].DefaultView.ToTable();
                                        double paidAmount = 0;
                                        int curColCnt = 0;
                                        int.TryParse(Convert.ToString(htColCnt[Convert.ToString(payVal.Key)]), out curColCnt);
                                        if (dtPayMode.Rows.Count > 0)
                                        {
                                            if (transtype != "3")
                                                double.TryParse(Convert.ToString(dtPayMode.Rows[0]["paid"]), out paidAmount);
                                            if (!htpayTot.ContainsKey(curColCnt))
                                                htpayTot.Add(curColCnt, Convert.ToString(paidAmount));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htpayTot[curColCnt]), out amount);
                                                amount += paidAmount;
                                                htpayTot.Remove(curColCnt);
                                                htpayTot.Add(curColCnt, Convert.ToString(amount));
                                            }
                                            if (payVal.Key == 2 || payVal.Key == 3)
                                            {
                                                string bankfk = Convert.ToString(dtPayMode.Rows[0]["ddbankcode"]);
                                                ddno = Convert.ToString(dtPayMode.Rows[0]["ddno"]);
                                                if (ds.Tables[9].Rows.Count > 0)
                                                {
                                                    ds.Tables[9].DefaultView.RowFilter = "bankpk='" + bankfk + "'";
                                                    DataTable dtbankName = ds.Tables[9].DefaultView.ToTable();
                                                    if (dtbankName.Rows.Count > 0)
                                                    {
                                                        bankname = Convert.ToString(dtbankName.Rows[0]["bankname"]) + "-" + ddno;
                                                    }
                                                }
                                            }
                                        }
                                        drowInst[curColCnt] = Convert.ToString(paidAmount);
                                    }
                                    if (!boolEx && transtype != "3")
                                        paidtot = paidtot + Convert.ToDouble(dvpaid[i]["paid"]);
                                    ColumnCnt = dtIndividualReport.Columns.Count;
                                    drowInst[ColumnCnt - 5] = Convert.ToString(dvpaid[i]["paid"]);

                                    if (ds.Tables[7].Rows.Count > 0)
                                    {
                                        DataView dvex = new DataView();
                                        if (ds.Tables[7].Rows.Count > 0 && ddl_hdr.SelectedItem.Text.Trim() == "Ledger")
                                        {
                                            ds.Tables[7].DefaultView.RowFilter = "rcptdate='" + Convert.ToString(dvpaid[i]["TransDate"]) + "' and receiptno='" + Convert.ToString(dvpaid[i]["TransCode"]) + "' and ledgerfk='" + Convert.ToString(dvpaid[i]["CollValue"]) + "'";
                                        }
                                        else if (ds.Tables[7].Rows.Count > 0)
                                        {
                                            ds.Tables[7].DefaultView.RowFilter = "rcptdate='" + Convert.ToString(dvpaid[i]["TransDate"]) + "' and receiptno='" + Convert.ToString(dvpaid[i]["TransCode"]) + "'";
                                        }
                                        dvex = ds.Tables[7].DefaultView;
                                        if (dvex.Count > 0)
                                        {
                                            for (int k = 0; k < dvex.Count; k++)
                                            {
                                                #region excess Amount
                                                sno++;
                                                drowInst = dtIndividualReport.NewRow();
                                                drowInst[0] = Convert.ToString(sno);
                                                drowInst[1] = Convert.ToString(dvex[k]["rcptdate"]);
                                                drowInst[2] = Convert.ToString(dvex[k]["receiptno"]);
                                                string ledgerfk = Convert.ToString(dvex[k]["ledgerfk"]);
                                                string Name = "";
                                                if (ddl_hdr.SelectedItem.Text.Trim() == "Header")
                                                    Name = d2.GetFunction("select HeaderName from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and l.ledgerpk='" + ledgerfk + "'");

                                                else if (ddl_hdr.SelectedItem.Text.Trim() == "Ledger")
                                                    Name = d2.GetFunction("select LedgerName from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and l.ledgerpk='" + ledgerfk + "'");

                                                else
                                                {
                                                    string hedFK = d2.GetFunction("select Headerfk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and l.ledgerpk='" + ledgerfk + "'");
                                                    string stream = d2.GetFunction("select c.type from registration r,degree d,Course c where r.degree_code=d.degree_code and d.course_id=c.course_id and r.app_no ='" + Convert.ToString(dvex[k]["app_no"]) + "'");

                                                    Name = d2.GetFunction("  select ChlGroupHeader from FS_ChlGroupHeaderSettings where headerfk='" + hedFK + "' and Stream='" + stream + "'");
                                                }

                                                drowInst[3] = Name + "(" + "Excess)";
                                                drowInst[4] = Convert.ToString(0);
                                                drowInst[5] = Convert.ToString(0);
                                                paidtot = paidtot + Convert.ToDouble(dvex[k]["amount"]);
                                                ColumnCnt = dtIndividualReport.Columns.Count;
                                                drowInst[ColumnCnt - 5] = Convert.ToString(dvex[k]["amount"]);
                                                #endregion
                                            }
                                        }
                                    }
                                    string naratn = Convert.ToString(dvpaid[i]["narration"]);
                                    if (!string.IsNullOrEmpty(naratn))
                                        naratn += "-" + bankname;
                                    else
                                        naratn = bankname;
                                    ColumnCnt = dtIndividualReport.Columns.Count;
                                    drowInst[ColumnCnt - 3] = naratn;

                                    #endregion

                                    dtIndividualReport.Rows.Add(drowInst);
                                }
                            }

                            #region total

                            drowInst = dtIndividualReport.NewRow();
                            rowCnt = dtIndividualReport.Rows.Count;
                            drowInst[0] = "Total";
                            dicColSpanFormat1.Add(rowCnt, "Total");
                            drowInst[4] = Convert.ToString(feeamount);
                            drowInst[5] = Convert.ToString(allottot);

                            foreach (DictionaryEntry totAmt in htpayTot)
                            {
                                int tempColCnt = Convert.ToInt32(totAmt.Key);
                                drowInst[tempColCnt] = Convert.ToString(totAmt.Value);
                                if (!htGrandTot.ContainsKey(tempColCnt))
                                    htGrandTot.Add(tempColCnt, Convert.ToString(totAmt.Value));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htGrandTot[tempColCnt]), out amount);
                                    amount += Convert.ToDouble(totAmt.Value);
                                    htGrandTot.Remove(tempColCnt);
                                    htGrandTot.Add(tempColCnt, Convert.ToString(amount));
                                }
                                // grandpaidtot = grandpaidtot + Convert.ToDouble(FpSpread1.Sheets[0].Cells[rowCnt, 6].Text);
                            }
                            int tempCnt = dtIndividualReport.Columns.Count - 5;
                            drowInst[tempCnt] = Convert.ToString(paidtot);


                            //DeductTotal
                            tempCnt = dtIndividualReport.Columns.Count - 2;
                            drowInst[tempCnt] = Convert.ToString(DeductTotal);
                            dtIndividualReport.Rows.Add(drowInst);
                            tempCnt = dtIndividualReport.Columns.Count - 5;
                            grandpaidtot = grandpaidtot + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][tempCnt]);
                            tempCnt = dtIndividualReport.Columns.Count - 2;
                            grandDeductTotal = grandDeductTotal + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][tempCnt]);
                            grandfeeamount = grandfeeamount + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][4]);
                            grandalltot = grandalltot + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][5]);
                            //total balance
                            drowInst = dtIndividualReport.NewRow();
                            drowInst[0] = "Balance";
                            rowCnt = dtIndividualReport.Rows.Count;
                            dicColSpanFormat1.Add(rowCnt, "Balance");

                            //value
                            double balval = 0;
                            if (allottot >= paidtot)
                            {
                                balval = allottot - paidtot;
                                drowInst[5] = Convert.ToString(balval);
                            }
                            else
                            {
                                balval = allottot - paidtot;
                                tempCnt = dtIndividualReport.Columns.Count - 5;
                                drowInst[tempCnt] = Convert.ToString(balval).TrimStart('-');
                            }
                            tempCnt = dtIndividualReport.Columns.Count - 4;
                            drowInst[tempCnt] = Convert.ToString(baltot);
                            dtIndividualReport.Rows.Add(drowInst);
                            grandbaltot = grandbaltot + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][tempCnt]);

                            #endregion
                        }
                        if (cbRefund.Checked && ds.Tables.Count > 0 && ds.Tables[6].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                            {
                                #region refund Paid

                                sno++;
                                drowInst = dtIndividualReport.NewRow();
                                drowInst[0] = Convert.ToString(sno);
                                drowInst[1] = Convert.ToString(ds.Tables[6].Rows[i]["TransDate"]);
                                string strEx = string.Empty;
                                drowInst[2] = Convert.ToString(ds.Tables[6].Rows[i]["TransCode"]);
                                drowInst[3] = Convert.ToString(ds.Tables[6].Rows[i]["CollName"]) + strEx;
                                drowInst[5] = Convert.ToString(0);
                                //foreach (KeyValuePair<int, string> payVal in payMode)
                                //{
                                //    string strVal = " transdate='" + Convert.ToString(ds.Tables[6].Rows[i]["TransDate"]) + "' and TransCode='" + Convert.ToString(ds.Tables[6].Rows[i]["TransCode"]) + "' and CollValue='" + Convert.ToString(ds.Tables[6].Rows[i]["CollValue"]) + "' and paymode='" + payVal.Key + "'";
                                //    ds.Tables[6].DefaultView.RowFilter = strVal;
                                //    DataTable dtPayMode = ds.Tables[6].DefaultView.ToTable();
                                //    double paidAmount = 0;
                                //    if (dtPayMode.Rows.Count > 0)
                                //        double.TryParse(Convert.ToString(dtPayMode.Rows[0]["paid"]), out paidAmount);
                                //    int curColCnt = 0;
                                //    int.TryParse(Convert.ToString(htColCnt[Convert.ToString(payVal.Key)]), out curColCnt);

                                //}
                                ColumnCnt = dtIndividualReport.Columns.Count;
                                drowInst[ColumnCnt - 4] = Convert.ToString(ds.Tables[6].Rows[i]["paid"]);
                                drowInst[ColumnCnt - 3] = Convert.ToString(ds.Tables[6].Rows[i]["narration"]);
                                #endregion

                                dtIndividualReport.Rows.Add(drowInst);
                            }
                        }

                        #region grand total

                        drowInst = dtIndividualReport.NewRow();
                        rowCnt = dtIndividualReport.Rows.Count;
                        drowInst[0] = "Grand Total";
                        dicColSpanFormat1.Add(rowCnt, "Grand Total");
                        drowInst[5] = Convert.ToString(grandalltot);
                        foreach (DictionaryEntry totAmt in htGrandTot)//paymode
                        {
                            int tempColCnt = Convert.ToInt32(totAmt.Key);
                            drowInst[tempColCnt] = Convert.ToString(totAmt.Value);
                        }

                        drowInst[dtIndividualReport.Columns.Count - 5] = Convert.ToString(grandpaidtot);
                        dtIndividualReport.Rows.Add(drowInst);

                        //grand balance
                        drowInst = dtIndividualReport.NewRow();
                        rowCnt = dtIndividualReport.Rows.Count;
                        drowInst[0] = "Grand Balance";
                        dicColSpanFormat1.Add(rowCnt, "Grand Balance");

                        double grandbal = 0;
                        if (grandalltot >= grandpaidtot)
                        {
                            grandbal = grandalltot - grandpaidtot;
                            drowInst[5] = Convert.ToString(grandbal);

                        }
                        else
                        {
                            int colCnt = dtIndividualReport.Columns.Count - 5;
                            grandbal = grandalltot - grandpaidtot;
                            drowInst[colCnt] = Convert.ToString(grandbal);

                        }

                        grandalltot = 0;
                        grandpaidtot = 0;
                        int colCnts = dtIndividualReport.Columns.Count - 4;
                        drowInst[colCnts] = Convert.ToString(grandbaltot);
                        dtIndividualReport.Rows.Add(drowInst);
                        grandbaltot = 0;

                        //grandDeductTotal
                        if (cbincdedut.Checked == true)
                        {
                            drowInst = dtIndividualReport.NewRow();
                            rowCnt = dtIndividualReport.Rows.Count;
                            drowInst[0] = "Grand Deduct Amount";
                            dicColSpanFormat1.Add(rowCnt, "Grand Deduct Amount");
                            drowInst[dtIndividualReport.Columns.Count - 2] = Convert.ToString(grandDeductTotal);
                            dtIndividualReport.Rows.Add(drowInst);
                            grandDeductTotal = 0;
                        }
                        drowInst = dtIndividualReport.NewRow();
                        rowCnt = dtIndividualReport.Rows.Count;
                        dicColSpanFormat1.Add(rowCnt, "Empty");
                        dtIndividualReport.Rows.Add(drowInst);

                        #endregion

                    }
                    else
                    {
                        rprint.Visible = false;
                        grdIndividualReport.Visible = false;
                        //  div1.Visible = false;
                        Error.Visible = true;
                        Error.Text = "No Record Found!";
                    }
                }
                grdIndividualReport.DataSource = dtIndividualReport;
                grdIndividualReport.DataBind();
                grdIndividualReport.Visible = true;

                grdIndividualReport.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grdIndividualReport.Rows[0].Font.Bold = true;
                grdIndividualReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;


                foreach (KeyValuePair<int, string> dr in dicColSpanFormat1)
                {
                    int g = dr.Key;
                    string DicValue = dr.Value;
                    if (DicValue == "Roll No")
                    {
                        int colcount = dtIndividualReport.Columns.Count;
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = colcount;
                        for (int a = 1; a < colcount; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = ColorTranslator.FromHtml("#cc66ff");
                    }
                    if (DicValue == "Total")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = Color.Gray;
                    }
                    if (DicValue == "Balance")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = Color.YellowGreen;
                    }
                    if (DicValue == "Grand Total")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 4;
                        for (int a = 1; a < 4; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = Color.Gold;
                    }
                    if (DicValue == "Grand Balance")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 4;
                        for (int a = 1; a < 4; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = Color.LightSkyBlue;
                    }
                    if (DicValue == "Grand Deduct Amount")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 4;
                        for (int a = 1; a < 4; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = Color.Tomato;
                    }
                    if (DicValue == "Empty")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = dtIndividualReport.Columns.Count;
                        for (int a = 1; a < dtIndividualReport.Columns.Count; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                    }
                }

                // div1.Visible = true;
                Error.Visible = false;
                rprint.Visible = true;
                lblsmserror.Visible = false;
                txtexcel.Text = "";
            }
            else
            {
                rprint.Visible = false;
                grdIndividualReport.Visible = false;
                // div1.Visible = false;
                Error.Visible = true;
                Error.Text = "Please Enter the Roll/Reg No!";
            }
        }
        catch (Exception ex)
        { }
    }

    private void viewFormat1()
    {
        try
        {
            UserbasedRights();
            Error.Visible = false;
            string headerid = "";
            string semcode = "";
            int headercount = 0;
            double feeamount = 0;
            double allottot = 0.0;
            double paidtot = 0.0;
            double baltot = 0.0;
            double grandfeeamount = 0;
            double grandalltot = 0.0;
            double grandpaidtot = 0.0;
            double grandbaltot = 0.0;
            double DeductTotal = 0.0;
            double grandDeductTotal = 0.0;
            string AppNo = "";
            string type = "";

            Hashtable hscol = new Hashtable();
            hscol.Clear();
            Hashtable hshead = new Hashtable();
            hshead.Clear();
            Hashtable hschkcol = new Hashtable();
            hschkcol.Clear();
            List<string> rolllist = new List<string>();
            int rowCnt = 0;
            bool beforeAdm = false;
            string rollType = string.Empty;
            string transerText = string.Empty;
            if (ddl_collegename.Items.Count > 0)
                collegecode1 = Convert.ToString(ddl_collegename.SelectedValue);
            if (!cbTrans.Checked)
            {
                #region without transfer
                //transerText = " and istransfer='0'";
                transerText = " and ISNULL(istransfer,0)='0'";//modified by saranya 04Dec2017
                if (rbstudtype.SelectedItem.Value == "1")
                {
                    if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 0)
                    {
                        //roll no
                        AppNo = d2.GetFunction("select App_No from Registration where Roll_No='" + txtno.Text + "' and college_code='" + collegecode1 + "' ");
                        if (AppNo != "0")
                            rolllist.Add(AppNo);
                        rollType = " r.roll_no";
                        //and cc=0 and DelFlag=0 and Exam_Flag<>'debar'
                    }
                    else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 1)
                    {
                        //reg no
                        AppNo = d2.GetFunction("select App_No from Registration where Reg_No='" + txtno.Text + "' and college_code='" + collegecode1 + "'");
                        if (AppNo != "0")
                            rolllist.Add(AppNo);
                        rollType = " r.Reg_No";
                    }
                    else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 2)
                    {
                        //Admin no
                        AppNo = d2.GetFunction("select App_No from Registration where Roll_admit='" + txtno.Text + "' and college_code='" + collegecode1 + "'");
                        if (AppNo != "0")
                            rolllist.Add(AppNo);
                        rollType = " r.Roll_admit";
                    }
                    else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 3)
                    {
                        //Admin no
                        AppNo = d2.GetFunction("select App_No from applyn where app_formno='" + txtno.Text + "' and college_code='" + collegecode1 + "'");
                        if (AppNo != "0")
                            rolllist.Add(AppNo);
                        rollType = " r.app_formno";
                        beforeAdm = true;
                    }
                    else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 4)
                    {
                        //Admin no
                        AppNo = d2.GetFunction("select App_No from Registration where Roll_no='" + txtno.Text.Split('-')[1] + "' and college_code='" + collegecode1 + "'");
                        if (AppNo != "0")
                            rolllist.Add(AppNo);
                    }
                    //  rollType = " r.Roll_admit";
                }
                else
                {
                    string rollMult = Convert.ToString(lblrolldisp.Text);
                    if (rollMult != "")
                    {
                        string[] roll = rollMult.Split(',');
                        if (roll.Length > 0)
                        {
                            for (int i = 0; i < roll.Length; i++)
                            {
                                string app_no = d2.GetFunction("select App_No from Registration where Roll_No='" + Convert.ToString(roll[i]) + "' and college_code='" + collegecode1 + "'");
                                if (app_no != "0")
                                    rolllist.Add(app_no);
                                rollType = " r.Roll_No";
                            }
                        }
                    }
                }
                #endregion

            }
            else
            {
                AppNo = getTransferAppNo(collegecode1, txtno.Text.Trim());
                if (AppNo != "0")
                {
                    rolllist.Add(AppNo);
                    transerText = " and ISNULL(istransfer,0)='1'";//modified by saranya 04Dec2017
                }
            }

            #region get value

            if (ddl_hdr.SelectedIndex == 1)
            {
                if (cbl_header.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        if (cbl_header.Items[i].Selected == true)
                        {
                            headercount++;
                            if (headerid.Trim() == "")
                            {
                                headerid = "" + Convert.ToString(cbl_header.Items[i].Value) + "";
                            }
                            else
                            {
                                headerid = headerid + "'" + "," + "'" + Convert.ToString(cbl_header.Items[i].Value);
                            }
                        }
                    }
                }
            }

            if (ddl_hdr.SelectedIndex == 0)
            {
                if (cbl_header.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        if (cbl_header.Items[i].Selected == true)
                        {
                            if (headerid.Trim() == "")
                            {
                                headerid = "" + Convert.ToString(cbl_header.Items[i].Text) + "";
                            }
                            else
                            {
                                headerid = headerid + "'" + "," + "'" + Convert.ToString(cbl_header.Items[i].Text);
                            }
                        }
                    }
                }
            }

            if (ddl_hdr.SelectedIndex == 2)
            {
                if (cbl_header.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        if (cbl_header.Items[i].Selected == true)
                        {
                            if (headerid.Trim() == "")
                            {
                                headerid = "" + Convert.ToString(cbl_header.Items[i].Value) + "";
                            }
                            else
                            {
                                headerid = headerid + "'" + "," + "'" + Convert.ToString(cbl_header.Items[i].Value);
                            }
                        }
                    }
                }
            }

            if (cbl_fee.Items.Count > 0)
            {
                for (int i = 0; i < cbl_fee.Items.Count; i++)
                {
                    if (cbl_fee.Items[i].Selected == true)
                    {
                        if (semcode.Trim() == "")
                        {
                            semcode = "" + Convert.ToString(cbl_fee.Items[i].Value) + "";
                        }
                        else
                        {
                            semcode = semcode + "'" + "," + "'" + Convert.ToString(cbl_fee.Items[i].Value);
                        }
                    }
                }
            }

            if (headerid.Trim() == "")
            {
                Error.Visible = true;
                Error.Text = "Please select any Item!";
                return;
            }
            if (semcode.Trim() == "")
            {
                Error.Visible = true;
                Error.Text = "Please select any semester!";
                return;
            }

            #endregion

            //if (AppNo.Trim() != "" && AppNo.Trim() != "0")
            if (rolllist.Count > 0)
            {
                #region design

                ArrayList arColumn = getColumn();
                Hashtable htCol = new Hashtable();

                arrColHdrNames.Add("S.No");
                dtIndividualReport.Columns.Add("S.No");
                arrColHdrNames.Add("Trans Date");
                dtIndividualReport.Columns.Add("Trans Date");
                arrColHdrNames.Add("Trans Code");
                dtIndividualReport.Columns.Add("Trans Code");
                if (lbl_hdr.Text == "Group Header")
                {
                    arrColHdrNames.Add("Group Header");
                    dtIndividualReport.Columns.Add("Group Header");
                }
                if (lbl_hdr.Text == "Header")
                {
                    arrColHdrNames.Add("Header");
                    dtIndividualReport.Columns.Add("Header");
                }
                if (lbl_hdr.Text == "Ledger")
                {
                    arrColHdrNames.Add("Ledger");
                    dtIndividualReport.Columns.Add("Ledger");
                }
                arrColHdrNames.Add("Fee Amount");
                dtIndividualReport.Columns.Add("Fee Amount");
                arrColHdrNames.Add("Allot");
                dtIndividualReport.Columns.Add("Allot");
                arrColHdrNames.Add("Paid");
                dtIndividualReport.Columns.Add("Paid");
                //if (cbRefund.Checked)
                //{
                arrColHdrNames.Add("Refund");
                dtIndividualReport.Columns.Add("Refund");
                // }
                //narration
                //if (inclnarr.Checked == true)
                //{
                arrColHdrNames.Add("Narration");
                dtIndividualReport.Columns.Add("Narration");
                //}
                //deduction
                //if (cbincdedut.Checked == true)
                //{
                arrColHdrNames.Add("Concession Amt");
                dtIndividualReport.Columns.Add("Concession Amt");
                arrColHdrNames.Add("Concession Reason");
                dtIndividualReport.Columns.Add("Concession Reason");
                // }
                DataRow drHdr1 = dtIndividualReport.NewRow();
                for (int grCol = 0; grCol < dtIndividualReport.Columns.Count; grCol++)
                    drHdr1[grCol] = arrColHdrNames[grCol];
                dtIndividualReport.Rows.Add(drHdr1);

                string userCode = "";  // Modify Jairam 30.08.2016
                //if (usBasedRights == true)
                //    userCode = " and d.EntryUserCode in('" + usercode + "')";

                #endregion

                double tempfnlExcess = 0;
                for (int rol = 0; rol < rolllist.Count; rol++)
                {
                    AppNo = Convert.ToString(rolllist[rol]);
                    type = d2.GetFunction("select c.type from Registration r,Degree d,Course c where r.degree_code =d.Degree_Code and d.Course_Id =c.Course_Id and App_No ='" + AppNo + "' ");
                    #region Query

                    string selq = "";
                    if (ddl_hdr.SelectedIndex == 0)
                    {
                        if (!beforeAdm)
                        {
                            #region group header
                            //allot detail
                            selq = "  select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  r.App_No=f.App_No and ch.HeaderFK=f.HeaderFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "'" + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  f.HeaderFK=ch.HeaderFK and r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r where  r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK";

                            //paid detail                   
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + userCode + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.Narration,transtype order by cast(transdate as datetime)";

                            //deduction
                            // selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.LedgerFK in('" + headerid + "') and f.feecategory in('" + semcode + "') group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK ";
                            selq += "  select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t,FS_ChlGroupHeaderSettings G where f.App_No=r.App_No and f.headerfk=g.headerfk and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and g.ChlGroupHeader in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),f.HeaderFK ";

                            //paid detail                   
                            selq = selq + " select SUM(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype  from FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + userCode + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0' and isnull(credit,'0')>'0' " + transerText + " group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype";

                            #endregion
                        }
                        else
                        {
                            #region group header
                            //allot detail
                            selq = "  select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from applyn r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  r.App_No=f.App_No and ch.HeaderFK=f.HeaderFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  f.HeaderFK=ch.HeaderFK and r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,v r where  r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK";

                            //paid detail                   
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration ,isnull(transtype,'0') as transtype from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + userCode + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.Narration,transtype order by cast(transdate as datetime)";

                            selq += "  select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue  from FT_FeeAllot f,applyn r,textvaltable t,FS_ChlGroupHeaderSettings G where f.App_No=r.App_No and f.headerfk=g.headerfk and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and g.ChlGroupHeader in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),f.HeaderFK ";

                            //paid detail                   
                            selq = selq + " select SUM(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype  from FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + userCode + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0' and isnull(credit,'0')>'0' " + transerText + " group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype";

                            #endregion
                        }
                    }
                    if (ddl_hdr.SelectedIndex == 1)
                    {
                        if (!beforeAdm)
                        {
                            #region header

                            //allot detail query
                            selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from Registration r,TextValTable t,FT_FeeAllot f where    r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "')" + transerText + " order by len(t.TextVal),t.TextVal  asc ";

                            selq = selq + "  select distinct (r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where   r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + "  select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue,H.HeaderName as CollName from FT_FeeAllot F,Registration r,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK  and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " group by f.FeeCategory,f.App_No,AllotDate,F.HeaderFK,h.HeaderName";

                            //Paid Detail query                  
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,sum(credit) as credit,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,F.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  " + transerText + "  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,d.Narration,transtype order by cast(transdate as datetime)";//and isnull(credit,'0')='0' abar

                            //deduction 
                            //selq += " select textval,isnull(SUM(DeductAmout),0) from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headerid + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and FinYearFK in(" + actidquery + ")  group by TextCode,textval";                   

                            selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.headerfk in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK ";

                            selq += "    select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype from Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype ";

                            #endregion
                        }
                        else
                        {
                            #region header

                            //allot detail query
                            selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from applyn r,TextValTable t,FT_FeeAllot f where    r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                            selq = selq + "  select distinct (r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f where   r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + "  select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue,H.HeaderName as CollName from FT_FeeAllot F,applyn r,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK  and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " group by f.FeeCategory,f.App_No,AllotDate,F.HeaderFK,h.HeaderName";

                            //Paid Detail query                  
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,F.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0'  " + transerText + " group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,d.Narration,transtype order by cast(transdate as datetime)";

                            selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue  from FT_FeeAllot f,applyn r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.headerfk in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK ";

                            selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype from applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype ";

                            #endregion
                        }
                    }
                    if (ddl_hdr.SelectedIndex == 2)
                    {
                        if (!beforeAdm)
                        {
                            #region ledger

                            //allot details query
                            selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from Registration r,TextValTable t,FT_FeeAllot f where r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                            selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.LedgerFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where  r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue,l.LedgerName as CollName,l.priority from FT_FeeAllot F,Registration r,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + "  group by f.FeeCategory,f.App_No,AllotDate,F.LedgerFK,l.LedgerName,l.priority order by len(isnull(l.priority,1000)) , l.priority asc ";

                            //Paid details query               

                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),TransDate,103) as TransDate,TransCode,F.LedgerFK as CollValue,l.LedgerName as CollName,d.Narration,l.priority,isnull(transtype,'0') as transtype from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.Narration,l.priority,transtype order by len(isnull(l.priority,1000)) , l.priority asc,cast(transdate as datetime)";

                            //deduction
                            selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.LedgerFK in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.LedgerFK ";

                            selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.ledgerfk as CollValue,H.ledgername as CollName,d.Narration,isnull(transtype,'0') as transtype from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype ";
                            #endregion
                        }
                        else
                        {
                            #region ledger

                            //allot details query
                            selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) from applyn r,TextValTable t,FT_FeeAllot f where r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc  ";
                            selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.LedgerFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f where  r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue,l.LedgerName as CollName,l.priority from FT_FeeAllot F,applyn r,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + "  group by f.FeeCategory,f.App_No,AllotDate,F.LedgerFK,l.LedgerName,l.priority order by len(isnull(l.priority,1000)) , l.priority asc ";

                            //Paid details query               

                            selq = selq + " select sum(feeamount) as feeamount, SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),TransDate,103) as TransDate,TransCode,F.LedgerFK as CollValue,l.LedgerName as CollName,d.Narration,l.priority,isnull(transtype,'0') as transtype from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')='0' " + transerText + " group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.Narration,l.priority,transtype order by len(isnull(l.priority,1000)) , l.priority asc,cast(transdate as datetime)";

                            //deduction
                            selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue  from FT_FeeAllot f,applyn r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.LedgerFK in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.LedgerFK ";

                            selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.ledgerfk as CollValue,H.ledgername as CollName,d.Narration,isnull(transtype,'0') as transtype from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype ";
                            #endregion
                        }
                    }
                    // selq += " select receiptno,Convert(varchar(10),rcptdate,103) as rcptdate,ledgerfk,app_no,amount,headerfk from ft_excessReceiptdet er,fm_ledgermaster l where l.ledgerpk=er.ledgerfk and app_no='" + AppNo + "' and excesstype='1' ";
                    string strName = string.Empty;
                    string strGrpBy = string.Empty;
                    if (ddl_hdr.SelectedItem.Text.Trim() == "Ledger")
                    {
                        strName = ",er.ledgerfk as fk,l.ledgername as name";
                        strGrpBy = " ,er.ledgerfk,l.ledgername";
                    }
                    else
                    {
                        strName = ",h.headerpk as fk,h.headername as name";
                        strGrpBy = " ,h.headerpk,h.headername";
                    }
                    selq += " select receiptno,Convert(varchar(10),rcptdate,103) as rcptdate,app_no,sum(amount) as amount" + strName + " from ft_excessReceiptdet er,fm_ledgermaster l,fm_headermaster h where h.headerpk=l.headerfk and l.ledgerpk=er.ledgerfk and er.app_no='" + AppNo + "' and h.collegecode=l.collegecode and excesstype='1' group by receiptno,rcptdate,er.app_no" + strGrpBy + "";

                    selq += " select receiptno,Convert(varchar(10),rcptdate,103) as rcptdate,ledgerfk,app_no,amount from ft_excessReceiptdet where app_no='" + AppNo + "' and excesstype='2' ";

                    string selQ = " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode1 + "'";
                    #endregion

                    int sno = 0;
                    int serino = 0;
                    int serialno = 0;
                    DataView dvnew1 = new DataView();
                    DataView dvnew2 = new DataView();
                    DataView dvnew3 = new DataView();
                    DataView dvdt = new DataView();
                    ArrayList arRecpt = new ArrayList();
                    ArrayList arRcptAmt = new ArrayList();
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selq, "Text");
                    DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                    if (rol != 0)
                    {
                        //FpSpread1.Sheets[0].RowCount++;
                        //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.White;
                    }
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            drowInst = dtIndividualReport.NewRow();
                            string degreecode = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                            string deptname = string.Empty;
                            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                            {
                                dsval.Tables[0].DefaultView.RowFilter = "degree_code='" + degreecode + "'";
                                DataView dv = dsval.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                    deptname = Convert.ToString(dv[0]["degreename"]);
                            }
                            rowCnt = dtIndividualReport.Rows.Count;
                            drowInst[0] = Convert.ToString(ds.Tables[0].Rows[row]["headername"]) + "-" + deptname + "-" + Convert.ToString(ds.Tables[0].Rows[row]["TextVal"]);
                            dicColSpanFormat1.Add(rowCnt, "Roll No");
                            dtIndividualReport.Rows.Add(drowInst);

                            ds.Tables[1].DefaultView.RowFilter = "FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]) + "'";
                            allottot = 0.0;
                            baltot = 0.0;
                            paidtot = 0.0;
                            DeductTotal = 0;
                            feeamount = 0;
                            dvnew1 = ds.Tables[1].DefaultView;
                            for (int dv1 = 0; dv1 < dvnew1.Count; dv1++)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "CollValue='" + Convert.ToString(dvnew1[dv1]["CollValue"]) + "' and FeeCategory='" + Convert.ToString(dvnew1[dv1]["FeeCategory"]) + "'";
                                dvnew2 = ds.Tables[2].DefaultView;
                                for (int ik = 0; ik < dvnew2.Count; ik++)
                                {
                                    #region allot

                                    drowInst = dtIndividualReport.NewRow();
                                    sno++;
                                    drowInst[0] = Convert.ToString(sno);
                                    drowInst[1] = Convert.ToString(dvnew2[ik]["TransDate"]);
                                    drowInst[2] = "";
                                    drowInst[3] = Convert.ToString(dvnew2[ik]["CollName"]);
                                    drowInst[4] = Convert.ToString(dvnew2[ik]["feeamount"]);
                                    feeamount = feeamount + Convert.ToDouble(dvnew2[ik]["feeamount"]);
                                    drowInst[5] = Convert.ToString(dvnew2[ik]["total"]);
                                    allottot = allottot + Convert.ToDouble(dvnew2[ik]["total"]);
                                    drowInst[6] = Convert.ToString(0);

                                    #endregion

                                    dtIndividualReport.Rows.Add(drowInst);
                                }
                                double FNlAmt = 0;

                                string DedutRes = "";
                                if (ds.Tables[4].Rows.Count > 0)
                                {
                                    #region deduction

                                    string val = "CollValue='" + Convert.ToString(dvnew1[dv1]["CollValue"]) + "' and FeeCategory='" + Convert.ToString(dvnew1[dv1]["FeeCategory"]) + "'";
                                    ds.Tables[4].DefaultView.RowFilter = "CollValue='" + Convert.ToString(dvnew1[dv1]["CollValue"]) + "' and FeeCategory='" + Convert.ToString(dvnew1[dv1]["FeeCategory"]) + "'";
                                    dvdt = ds.Tables[4].DefaultView;
                                    if (dvdt.Count > 0 && dvdt != null)
                                    {
                                        for (int i = 0; i < dvdt.Count; i++)
                                        {
                                            double AMt = 0;
                                            double.TryParse(Convert.ToString(dvdt[i]["DeductAmout"]), out AMt);
                                            FNlAmt += AMt;
                                            if (DedutRes == "")
                                                DedutRes = Convert.ToString(dvdt[i]["textval"]);
                                            else
                                                DedutRes = DedutRes + "," + Convert.ToString(dvdt[i]["textval"]);
                                        }
                                        dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][9] = FNlAmt;
                                        DeductTotal += FNlAmt;
                                        dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][10] = Convert.ToString(DedutRes);
                                    }

                                    #endregion
                                }
                            }
                            DataView dvpaids = new DataView();
                            ds.Tables[3].DefaultView.RowFilter = " FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]) + "'";
                            //App_no='" + Convert.ToString(dvnew2[0]["App_no"]) + "' and
                            dvpaids = ds.Tables[3].DefaultView;
                            DataTable dtpaid = dvpaids.ToTable(true, "Transcode", "TransDate");
                            DataTable dtMainPaid = dvpaids.ToTable();
                            if (dvpaids.Count > 0 && dvpaids != null)
                            {
                                for (int rd = 0; rd < dtpaid.Rows.Count; rd++)
                                {
                                    string receiptno = Convert.ToString(dtpaid.Rows[rd]["Transcode"]);
                                    string receiptdt = Convert.ToString(dtpaid.Rows[rd]["TransDate"]);
                                    dtMainPaid.DefaultView.RowFilter = "transcode='" + receiptno + "'";
                                    DataView dvpaid = dtMainPaid.DefaultView;
                                    for (int i = 0; i < dvpaid.Count; i++)
                                    {
                                        #region Paid

                                        drowInst = dtIndividualReport.NewRow();
                                        sno++;
                                        drowInst[0] = Convert.ToString(sno);
                                        drowInst[1] = Convert.ToString(dvpaid[i]["TransDate"]);

                                        bool boolEx = false;
                                        string strEx = string.Empty;
                                        double tempAmt = 0;
                                        double ledger = 0;

                                        if (ds.Tables[7].Rows.Count > 0)//if excess used then print the name
                                        {

                                            DataView dvex = new DataView();
                                            if (ddl_hdr.SelectedItem.Text.Trim() == "Ledger")
                                            {
                                                ds.Tables[7].DefaultView.RowFilter = "rcptdate='" + Convert.ToString(dvpaid[i]["TransDate"]) + "' and receiptno='" + Convert.ToString(dvpaid[i]["TransCode"]) + "' and ledgerfk='0'";
                                            }
                                            else
                                            {
                                                ds.Tables[7].DefaultView.RowFilter = "rcptdate='" + Convert.ToString(dvpaid[i]["TransDate"]) + "' and receiptno='" + Convert.ToString(dvpaid[i]["TransCode"]) + "' and ledgerfk='0'";
                                            }

                                            dvex = ds.Tables[7].DefaultView;
                                            if (dvex.Count > 0)
                                            {
                                                boolEx = true;
                                                strEx = "-(Used Excess)";
                                                double.TryParse(Convert.ToString(dvex[0]["amount"]), out tempAmt);
                                                double.TryParse(Convert.ToString(dvex[0]["ledgerfk"]), out ledger);
                                                if (!arRcptAmt.Contains(Convert.ToString(dvpaid[i]["TransCode"]) + "-" + tempAmt))
                                                {
                                                    if (ledger != 0)//If Condition added by saranya on 8/2/2018 for adjusted excess amount showing in grandBalance
                                                    {
                                                        tempfnlExcess += tempAmt;
                                                    }
                                                    arRcptAmt.Add(Convert.ToString(dvpaid[i]["TransCode"]) + "-" + tempAmt);
                                                }
                                            }
                                        }
                                        drowInst[2] = Convert.ToString(dvpaid[i]["TransCode"]);
                                        drowInst[3] = Convert.ToString(dvpaid[i]["CollName"]) + strEx;
                                        drowInst[5] = Convert.ToString(0);

                                        string transtype = Convert.ToString(dvpaid[i]["transtype"]);
                                        // if (!boolEx)
                                        string paid = string.Empty;
                                        if (transtype != "3")
                                        {
                                            paid = Convert.ToString(dvpaid[i]["paid"]);
                                            if (paid != "")
                                            {


                                                paidtot = paidtot + Convert.ToDouble(dvpaid[i]["paid"]);
                                            }
                                        }
                                        else
                                        {
                                            paid = Convert.ToString(dvpaid[i]["paid"]);
                                            if (paid != "")
                                            {
                                                paidtot = paidtot + Convert.ToDouble(dvpaid[i]["paid"]);//abarna
                                            }
                                        }

                                        // 
                                        //else
                                        //{
                                        //    if (Convert.ToDouble(dvpaid[i]["paid"]) > tempAmt)
                                        //    {
                                        //        paidtot += (Convert.ToDouble(dvpaid[i]["paid"]) - tempAmt);
                                        //    }
                                        //}
                                        drowInst[6] = Convert.ToString(dvpaid[i]["paid"]);
                                        drowInst[8] = Convert.ToString(dvpaid[i]["narration"]);

                                        #endregion

                                        dtIndividualReport.Rows.Add(drowInst);
                                    }

                                    #region excess
                                    try
                                    {
                                        if (ds.Tables[6].Rows.Count > 0)
                                        {
                                            DataTable dvex = new DataTable();
                                            try
                                            {
                                                if (ddl_hdr.SelectedItem.Text.Trim() == "Ledger")
                                                {
                                                    ds.Tables[6].DefaultView.RowFilter = "rcptdate='" + receiptdt + "' and receiptno='" + receiptno + "' ";//and ledgerfk='" + Convert.ToString(dvpaid[i]["CollValue"]) + "'
                                                }
                                                else
                                                {
                                                    ds.Tables[6].DefaultView.RowFilter = "rcptdate='" + receiptdt + "' and receiptno='" + receiptno + "' ";//and headerfk='" + Convert.ToString(dvpaid[i]["CollValue"]) + "'
                                                }
                                            }
                                            catch { }
                                            dvex = ds.Tables[6].DefaultView.ToTable();
                                            if (dvex.Rows.Count > 0)
                                            {
                                                if (!arRecpt.Contains(receiptno))
                                                {
                                                    for (int k = 0; k < dvex.Rows.Count; k++)
                                                    {
                                                        #region excess Amount
                                                        //=============Commented by saranya on 08/01/2018==============//
                                                        //=============Enabled by abarna 30.01.2018 for kcg issue excess amount showing========//

                                                        drowInst = dtIndividualReport.NewRow();
                                                        sno++;
                                                        drowInst[0] = Convert.ToString(sno);
                                                        drowInst[1] = Convert.ToString(dvex.Rows[k]["rcptdate"]);
                                                        drowInst[2] = Convert.ToString(dvex.Rows[k]["receiptno"]);

                                                        string valueFK = Convert.ToString(dvex.Rows[k]["fk"]);
                                                        string Name = Convert.ToString(dvex.Rows[k]["name"]);
                                                        if (ddl_hdr.SelectedItem.Text.Trim() == "Group Header")
                                                        {
                                                            //string hedFK = d2.GetFunction("select Headerfk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and l.ledgerpk='" + ledgerfk + "'");
                                                            string stream = d2.GetFunction("select c.type from registration r,degree d,Course c where r.degree_code=d.degree_code and d.course_id=c.course_id and r.app_no ='" + Convert.ToString(dvex.Rows[k]["app_no"]) + "'");

                                                            Name = d2.GetFunction("  select ChlGroupHeader from FS_ChlGroupHeaderSettings where headerfk='" + valueFK + "' and Stream='" + stream + "'");
                                                        }
                                                        drowInst[3] = Name + "(" + "Excess)";
                                                        drowInst[4] = Convert.ToString(0);
                                                        drowInst[5] = Convert.ToString(0);
                                                        drowInst[6] = Convert.ToString(dvex.Rows[k]["amount"]);
                                                        // paidtot = paidtot + Convert.ToDouble(dvex.Rows[k]["amount"]);//
                                                        #endregion
                                                        dtIndividualReport.Rows.Add(drowInst);
                                                    }
                                                    arRecpt.Add(receiptno);
                                                }
                                            }
                                        }
                                    }
                                    catch { }
                                    #endregion
                                }
                            }

                            #region total
                            drowInst = dtIndividualReport.NewRow();
                            rowCnt = dtIndividualReport.Rows.Count;
                            drowInst[0] = "Total";
                            dicColSpanFormat1.Add(rowCnt, "Total");
                            drowInst[4] = Convert.ToString(feeamount);
                            drowInst[5] = Convert.ToString(allottot);
                            drowInst[6] = Convert.ToString(paidtot);
                            //DeductTotal
                            //drowInst[9] = Convert.ToString(DeductTotal);
                            dtIndividualReport.Rows.Add(drowInst);
                            grandfeeamount = grandfeeamount + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][4]);
                            grandalltot = grandalltot + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][5]);
                            grandpaidtot = grandpaidtot + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][6]);
                            //grandDeductTotal = grandDeductTotal + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][7]);

                            //total balance
                            drowInst = dtIndividualReport.NewRow();
                            rowCnt = dtIndividualReport.Rows.Count;
                            drowInst[0] = "Balance";
                            dicColSpanFormat1.Add(rowCnt, "Balance");

                            //value
                            double balval = 0;
                            if (allottot >= paidtot)
                            {
                                balval = allottot - paidtot;
                                drowInst[5] = Convert.ToString(balval);
                            }
                            else
                            {
                                balval = allottot - paidtot;
                                drowInst[6] = Convert.ToString(balval).TrimStart('-');
                            }
                            //drowInst[7] = Convert.ToString(baltot);
                            dtIndividualReport.Rows.Add(drowInst);
                            //grandbaltot = grandbaltot + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][7]);
                            #endregion
                        }
                        if (cbRefund.Checked && ds.Tables.Count > 0 && ds.Tables[5].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                            {
                                #region refund Paid

                                string strEx = string.Empty;
                                drowInst = dtIndividualReport.NewRow();
                                sno++;
                                drowInst[0] = Convert.ToString(sno);
                                drowInst[1] = Convert.ToString(ds.Tables[5].Rows[i]["TransDate"]);
                                drowInst[2] = Convert.ToString(ds.Tables[5].Rows[i]["TransCode"]);
                                drowInst[3] = Convert.ToString(ds.Tables[5].Rows[i]["CollName"]) + strEx;
                                drowInst[5] = Convert.ToString(0);
                                drowInst[6] = Convert.ToString(ds.Tables[5].Rows[i]["paid"]);
                                drowInst[8] = Convert.ToString(ds.Tables[5].Rows[i]["narration"]);

                                #endregion

                                dtIndividualReport.Rows.Add(drowInst);
                            }
                        }

                        #region grand total

                        drowInst = dtIndividualReport.NewRow();
                        rowCnt = dtIndividualReport.Rows.Count;
                        drowInst[0] = "Grand Total";
                        dicColSpanFormat1.Add(rowCnt, "Grand Total");
                        drowInst[5] = Convert.ToString(grandalltot);
                        drowInst[6] = Convert.ToString(grandpaidtot);
                        dtIndividualReport.Rows.Add(drowInst);

                        //grand balance
                        drowInst = dtIndividualReport.NewRow();
                        rowCnt = dtIndividualReport.Rows.Count;
                        drowInst[0] = "Grand Balance";
                        dicColSpanFormat1.Add(rowCnt, "Grand Balance");

                        double grandbal = 0;
                        if (grandalltot >= grandpaidtot)
                        {
                            grandbal = grandalltot - (grandpaidtot - tempfnlExcess);
                            drowInst[5] = Convert.ToString(grandbal);
                        }
                        else
                        {
                            grandbal = grandalltot - (grandpaidtot - tempfnlExcess);
                            drowInst[6] = Convert.ToString(grandbal);
                        }
                        grandalltot = 0;
                        grandpaidtot = 0;
                        dtIndividualReport.Rows.Add(drowInst);
                        grandbaltot = 0;

                        //grandDeductTotal
                        if (cbincdedut.Checked == true)
                        {
                            drowInst = dtIndividualReport.NewRow();
                            rowCnt = dtIndividualReport.Rows.Count;
                            drowInst[0] = "Grand Deduct Amount";
                            dicColSpanFormat1.Add(rowCnt, "Grand Deduct Amount");
                            drowInst[9] = Convert.ToString(grandDeductTotal);
                            dtIndividualReport.Rows.Add(drowInst);
                            grandDeductTotal = 0;
                        }

                        drowInst = dtIndividualReport.NewRow();
                        rowCnt = dtIndividualReport.Rows.Count;
                        dicColSpanFormat1.Add(rowCnt, "Empty");
                        dtIndividualReport.Rows.Add(drowInst);
                        #endregion


                    }
                    else
                    {
                        rprint.Visible = false;
                        grdIndividualReport.Visible = false;
                        // div1.Visible = false;
                        Error.Visible = true;
                        Error.Text = "No Record Found!";
                    }

                }
                grdIndividualReport.DataSource = dtIndividualReport;
                grdIndividualReport.DataBind();
                grdIndividualReport.Visible = true;

                grdIndividualReport.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grdIndividualReport.Rows[0].Font.Bold = true;
                grdIndividualReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                foreach (KeyValuePair<int, string> dr in dicColSpanFormat1)
                {
                    int g = dr.Key;
                    string DicValue = dr.Value;
                    if (DicValue == "Roll No")
                    {
                        int colcount = dtIndividualReport.Columns.Count;
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = colcount;
                        for (int a = 1; a < colcount; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = ColorTranslator.FromHtml("#cc66ff");
                    }
                    if (DicValue == "Total")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = Color.Gray;
                    }
                    if (DicValue == "Balance")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = Color.YellowGreen;
                    }
                    if (DicValue == "Grand Total")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 4;
                        for (int a = 1; a < 4; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = Color.Gold;
                    }
                    if (DicValue == "Grand Balance")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 4;
                        for (int a = 1; a < 4; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = Color.LightSkyBlue;
                    }
                    if (DicValue == "Grand Deduct Amount")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 4;
                        for (int a = 1; a < 4; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                        grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[g].BackColor = Color.Tomato;
                    }
                    if (DicValue == "Empty")
                    {
                        grdIndividualReport.Rows[g].Cells[0].ColumnSpan = dtIndividualReport.Columns.Count;
                        for (int a = 1; a < dtIndividualReport.Columns.Count; a++)
                            grdIndividualReport.Rows[g].Cells[a].Visible = false;
                    }
                }
                Error.Visible = false;
                rprint.Visible = true;
                lblsmserror.Visible = false;
                txtexcel.Text = "";
            }
            else
            {
                rprint.Visible = false;
                grdIndividualReport.Visible = false;
                //  div1.Visible = false;
                Error.Visible = true;
                Error.Text = "Please Enter the Roll/Reg No!";
            }
        }
        catch
        { }
    }

    //Method added by Idhris - 03-08-2016

    private void viewFormat2()
    {
        try
        {
            dicColSpanFormat1.Clear();
            UserbasedRights();
            ArrayList arrColHdrNames2 = new ArrayList();

            Error.Visible = false;
            string headerid = "";
            string semcode = "";
            int headercount = 0;
            double allottot = 0.0;
            double paidtot = 0.0;
            double baltot = 0.0;
            double grandalltot = 0.0;
            double grandpaidtot = 0.0;
            double grandbaltot = 0.0;
            string AppNo = "";
            string type = "";

            Hashtable hscol = new Hashtable();
            hscol.Clear();
            Hashtable hshead = new Hashtable();
            hshead.Clear();
            Hashtable hschkcol = new Hashtable();
            hschkcol.Clear();

            bool beforeAdm = false;
            string rollType = string.Empty;
            if (ddl_collegename.Items.Count > 0)
                collegecode1 = Convert.ToString(ddl_collegename.SelectedValue);
            string transerText = string.Empty;
            if (!cbTrans.Checked)
            {
                #region without transfer
                transerText = " and istransfer='0'";
                if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 0)
                {
                    //roll no
                    AppNo = d2.GetFunction("select App_No from Registration where Roll_No='" + txtno.Text + "' and college_code='" + collegecode1 + "' ").Trim();
                    //and cc=0 and DelFlag=0 and Exam_Flag<>'debar'
                    rollType = " r.Roll_No";
                }
                else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 1)
                {
                    //reg no
                    AppNo = d2.GetFunction("select App_No from Registration where Reg_No='" + txtno.Text + "' and college_code='" + collegecode1 + "' ").Trim();
                    rollType = " r.Reg_No";
                }
                else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 2)
                {
                    //Admin no
                    AppNo = d2.GetFunction("select App_No from Registration where Roll_admit='" + txtno.Text + "' and college_code='" + collegecode1 + "' ").Trim();
                    rollType = " r.Roll_admit";
                }
                else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 3)
                {
                    //Admin no
                    AppNo = d2.GetFunction("select App_No from applyn where app_formno='" + txtno.Text + "' and college_code='" + collegecode1 + "'");
                    rollType = " r.app_formno";
                    beforeAdm = true;
                }
                else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 4)
                {
                    //Admin no
                    AppNo = d2.GetFunction("select App_No from Registration where Roll_no='" + txtno.Text.Split('-')[1] + "' and college_code='" + collegecode1 + "' ");
                    rollType = " r.Roll_no";

                }
                #endregion
            }
            else
            {
                transerText = " and istransfer='1'";
                AppNo = getTransferAppNo(collegecode1, txtno.Text.Trim());

            }
            type = d2.GetFunction("select c.type from Registration r,Degree d,Course c where r.degree_code =d.Degree_Code and d.Course_Id =c.Course_Id and App_No ='" + AppNo + "' ");

            #region get value
            if (ddl_hdr.SelectedIndex == 1)
            {
                if (cbl_header.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        if (cbl_header.Items[i].Selected == true)
                        {
                            headercount++;
                            if (headerid.Trim() == "")
                            {
                                headerid = "" + Convert.ToString(cbl_header.Items[i].Value) + "";
                            }
                            else
                            {
                                headerid = headerid + "'" + "," + "'" + Convert.ToString(cbl_header.Items[i].Value);
                            }
                        }
                    }
                }
            }

            if (ddl_hdr.SelectedIndex == 0)
            {
                if (cbl_header.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        if (cbl_header.Items[i].Selected == true)
                        {
                            if (headerid.Trim() == "")
                            {
                                headerid = "" + Convert.ToString(cbl_header.Items[i].Text) + "";
                            }
                            else
                            {
                                headerid = headerid + "'" + "," + "'" + Convert.ToString(cbl_header.Items[i].Text);
                            }
                        }
                    }
                }
            }

            if (ddl_hdr.SelectedIndex == 2)
            {
                if (cbl_header.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        if (cbl_header.Items[i].Selected == true)
                        {
                            if (headerid.Trim() == "")
                            {
                                headerid = "" + Convert.ToString(cbl_header.Items[i].Value) + "";
                            }
                            else
                            {
                                headerid = headerid + "'" + "," + "'" + Convert.ToString(cbl_header.Items[i].Value);
                            }
                        }
                    }
                }
            }

            if (cbl_fee.Items.Count > 0)
            {
                for (int i = 0; i < cbl_fee.Items.Count; i++)
                {
                    if (cbl_fee.Items[i].Selected == true)
                    {
                        if (semcode.Trim() == "")
                        {
                            semcode = "" + Convert.ToString(cbl_fee.Items[i].Value) + "";
                        }
                        else
                        {
                            semcode = semcode + "'" + "," + "'" + Convert.ToString(cbl_fee.Items[i].Value);
                        }
                    }
                }
            }

            if (headerid.Trim() == "")
            {
                Error.Visible = true;
                Error.Text = "Please select any Item!";
                return;
            }
            if (semcode.Trim() == "")
            {
                Error.Visible = true;
                Error.Text = "Please select any semester!";
                return;
            }

            #endregion

            if (AppNo != "" && AppNo != "0")
            {
                #region design

                arrColHdrNames.Add("S.No");
                arrColHdrNames2.Add("S.No");
                dtIndividualReport.Columns.Add("S.No");
                arrColHdrNames.Add(lblcate.Text);
                arrColHdrNames2.Add(lblcate.Text);
                dtIndividualReport.Columns.Add(lblcate.Text);
                arrColHdrNames.Add("Allot Date");
                arrColHdrNames2.Add("Allot Date");
                dtIndividualReport.Columns.Add("Allot Date");
                arrColHdrNames.Add("Trans Code");
                arrColHdrNames2.Add("Trans Code");
                dtIndividualReport.Columns.Add("Trans Code");
                if (lbl_hdr.Text == "Group Header")
                {
                    arrColHdrNames.Add("Group Header");
                    arrColHdrNames2.Add("Group Header");
                    dtIndividualReport.Columns.Add("Group Header");
                }
                if (lbl_hdr.Text == "Header")
                {
                    arrColHdrNames.Add("Header");
                    arrColHdrNames2.Add("Header");
                    dtIndividualReport.Columns.Add("Header");
                }
                if (lbl_hdr.Text == "Ledger")
                {
                    arrColHdrNames.Add("Ledger");
                    arrColHdrNames2.Add("Ledger");
                    dtIndividualReport.Columns.Add("Ledger");
                }
                arrColHdrNames.Add("Allot");
                arrColHdrNames2.Add("Allot");
                dtIndividualReport.Columns.Add("Allot");
                arrColHdrNames.Add("Paid");
                arrColHdrNames2.Add("Amount");
                dtIndividualReport.Columns.Add("Amount");
                arrColHdrNames.Add("Paid");
                arrColHdrNames2.Add("Date");
                dtIndividualReport.Columns.Add("Date");
                DataRow drHdr1 = dtIndividualReport.NewRow();
                DataRow drHdr2 = dtIndividualReport.NewRow();
                for (int grCol = 0; grCol < dtIndividualReport.Columns.Count; grCol++)
                {
                    drHdr1[grCol] = arrColHdrNames[grCol];
                    drHdr2[grCol] = arrColHdrNames2[grCol];
                }
                dtIndividualReport.Rows.Add(drHdr1);
                dtIndividualReport.Rows.Add(drHdr2);

                #endregion

                string userCode = "";
                //if (usBasedRights == true)
                //    userCode = " and d.EntryUserCode in('" + usercode + "')";

                #region Query

                string selq = "";
                if (ddl_hdr.SelectedIndex == 0)
                {
                    if (!beforeAdm)
                    {
                        #region gp header
                        //allot detail
                        selq = "  select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,f.FeeCategory,t.TextVal,r.degree_code,len(t.TextVal) from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  r.App_No=f.App_No and ch.HeaderFK=f.HeaderFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  f.HeaderFK=ch.HeaderFK and r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r where  r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK";

                        //paid detail

                        selq = selq + " select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK";
                        #endregion
                    }
                    else
                    {
                        #region gp header
                        //allot detail
                        selq = "  select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,f.FeeCategory,t.TextVal,r.degree_code,len(t.TextVal) from applyn r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  r.App_No=f.App_No and ch.HeaderFK=f.HeaderFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  f.HeaderFK=ch.HeaderFK and r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,applyn r where  r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK";

                        //paid detail

                        selq = selq + " select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,f.HeaderFK as CollValue,G.ChlGroupHeader as CollName  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,AllotDate";
                        #endregion
                    }
                }
                if (ddl_hdr.SelectedIndex == 1)
                {
                    if (!beforeAdm)
                    {
                        #region header
                        //allot detail query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,f.FeeCategory,t.TextVal,r.degree_code,len(t.TextVal) from Registration r,TextValTable t,FT_FeeAllot f where    r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                        selq = selq + "  select distinct (r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where   r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + "  select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue,H.HeaderName as CollName from FT_FeeAllot F,Registration r,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK  and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " group by f.FeeCategory,f.App_No,AllotDate,F.HeaderFK,h.HeaderName";

                        //Paid Detail query

                        selq = selq + " select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,F.HeaderFK as CollValue,H.HeaderName as CollName from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + transerText + "  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,AllotDate";
                        #endregion
                    }
                    else
                    {
                        #region header
                        //allot detail query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,f.FeeCategory,t.TextVal,r.degree_code,len(t.TextVal) from applyn r,TextValTable t,FT_FeeAllot f where    r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  as ";
                        selq = selq + "  select distinct (r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f where   r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  as";
                        selq = selq + "  select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue,H.HeaderName as CollName from FT_FeeAllot F,applyn r,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK  and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " group by f.FeeCategory,f.App_No,AllotDate,F.HeaderFK,h.HeaderName";

                        //Paid Detail query

                        selq = selq + " select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,F.HeaderFK as CollValue,H.HeaderName as CollName from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + transerText + "  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,AllotDate";
                        #endregion
                    }
                }
                if (ddl_hdr.SelectedIndex == 2)
                {
                    if (!beforeAdm)
                    {
                        #region ledger
                        //allot details query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,f.FeeCategory,t.TextVal,r.degree_code,len(t.TextVal) from Registration r,TextValTable t,FT_FeeAllot f where r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                        selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.LedgerFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where  r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue,l.LedgerName as CollName,l.priority from FT_FeeAllot F,Registration r,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + "  group by f.FeeCategory,f.App_No,AllotDate,F.LedgerFK,l.LedgerName,l.priority order by len(isnull(l.priority,1000)) , l.priority asc";

                        //Paid details query

                        selq = selq + " select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),TransDate,103) as TransDate,TransCode,F.LedgerFK as CollValue,l.LedgerName as CollName,l.priority from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + transerText + " group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,l.priority order by len(isnull(l.priority,1000)) , l.priority asc";
                        #endregion
                    }
                    else
                    {
                        #region ledger
                        //allot details query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,f.FeeCategory,t.TextVal,r.degree_code,len(t.TextVal) from applyn r,TextValTable t,FT_FeeAllot f where r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                        selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.LedgerFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f where  r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(paidAmount) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue,l.LedgerName as CollName,l.priority from FT_FeeAllot F,applyn r,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + "  group by f.FeeCategory,f.App_No,AllotDate,F.LedgerFK,l.LedgerName,l.priority order by len(isnull(l.priority,1000)) , l.priority asc";

                        //Paid details query

                        selq = selq + " select SUM(TotalAmount) as total,SUM(BalAmount) as bal,SUM(Debit) as paid,f.FeeCategory ,f.App_No,convert(varchar(10),TransDate,103) as TransDate,TransCode,F.LedgerFK as CollValue,l.LedgerName as CollName,l.priority from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' " + userCode + " and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + transerText + " group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,l.priority order by len(isnull(l.priority,1000)) , l.priority asc";
                        #endregion
                    }
                }

                string selQ = " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode1 + "'";
                DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                #endregion

                int sno = 0;
                int serino = 0;
                int serialno = 0;
                DataView dvnew1 = new DataView();
                DataView dvnew2 = new DataView();
                DataView dvnew3 = new DataView();
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            drowInst = dtIndividualReport.NewRow();
                            string semester = Convert.ToString(ds.Tables[0].Rows[row]["TextVal"]).ToUpper();
                            if (semester.Contains("SEMESTER"))
                            {
                                semester = semester.Replace("SEMESTER", string.Empty).Trim();
                                semester = romanLetter(semester);
                            }
                            else
                            {
                                semester = semester.Replace("YEAR", string.Empty).Trim();
                                semester = romanLetter(returnYearforSem(semester));
                            }

                            string degreecode = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                            string deptname = string.Empty;
                            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                            {
                                dsval.Tables[0].DefaultView.RowFilter = "degree_code='" + degreecode + "'";
                                DataView dv = dsval.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                    deptname = Convert.ToString(dv[0]["degreename"]);
                            }
                            drowInst[0] = Convert.ToString(ds.Tables[0].Rows[row]["headername"]) + "-" + deptname + "-" + Convert.ToString(ds.Tables[0].Rows[row]["TextVal"]);
                            dicColSpanFormat1.Add(dtIndividualReport.Rows.Count, "Roll No");
                            dtIndividualReport.Rows.Add(drowInst);
                            //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[row]["headername"]);
                            //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                            // FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                            ds.Tables[1].DefaultView.RowFilter = "FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]) + "'";
                            allottot = 0.0;
                            baltot = 0.0;
                            paidtot = 0.0;
                            dvnew1 = ds.Tables[1].DefaultView;
                            for (int dv1 = 0; dv1 < dvnew1.Count; dv1++)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "CollValue='" + Convert.ToString(dvnew1[dv1]["CollValue"]) + "' and FeeCategory='" + Convert.ToString(dvnew1[dv1]["FeeCategory"]) + "'";
                                dvnew2 = ds.Tables[2].DefaultView;
                                for (int ik = 0; ik < dvnew2.Count; ik++)
                                {
                                    drowInst = dtIndividualReport.NewRow();
                                    sno++;
                                    drowInst[0] = Convert.ToString(sno);
                                    drowInst[1] = semester;
                                    drowInst[2] = Convert.ToString(dvnew2[ik]["TransDate"]);
                                    drowInst[4] = Convert.ToString(dvnew2[ik]["CollName"]);
                                    drowInst[5] = Convert.ToString(dvnew2[ik]["total"]);
                                    allottot = allottot + Convert.ToDouble(dvnew2[ik]["total"]);
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(0);

                                    //Paid 

                                    DataView dvpaid = new DataView();
                                    ds.Tables[3].DefaultView.RowFilter = " FeeCategory='" + Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]) + "' and CollName='" + Convert.ToString(dvnew2[ik]["CollName"]) + "' and collvalue='" + Convert.ToString(dvnew2[ik]["CollValue"]) + "' and app_no='" + Convert.ToString(dvnew2[ik]["app_no"]) + "'";
                                    //App_no='" + Convert.ToString(dvnew2[0]["App_no"]) + "' and
                                    dvpaid = ds.Tables[3].DefaultView;
                                    if (dvpaid.Count > 0 && dvpaid != null)
                                    {
                                        //sno++;
                                        //if (i != 0)
                                        //FpSpread1.Sheets[0].RowCount++;
                                        drowInst[1] = semester;
                                        paidtot = paidtot + Convert.ToDouble(dvpaid[ik]["paid"]);
                                        drowInst[7] = Convert.ToString(dvpaid[ik]["TransDate"]);
                                        drowInst[6] = Convert.ToString(dvpaid[ik]["paid"]);
                                    }
                                    dtIndividualReport.Rows.Add(drowInst);
                                }
                            }

                            drowInst = dtIndividualReport.NewRow();
                            dicColSpanFormat1.Add(dtIndividualReport.Rows.Count, "Total");
                            drowInst[0] = "Total";
                            drowInst[5] = Convert.ToString(allottot);
                            drowInst[6] = Convert.ToString(paidtot);
                            dtIndividualReport.Rows.Add(drowInst);
                            grandalltot = grandalltot + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][5]);
                            grandpaidtot = grandpaidtot + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][6]);

                            //total balance
                            drowInst = dtIndividualReport.NewRow();
                            dicColSpanFormat1.Add(dtIndividualReport.Rows.Count, "Balance");
                            drowInst[0] = "Balance";

                            //value
                            double balval = allottot - paidtot;
                            drowInst[5] = Convert.ToString(balval);
                            drowInst[6] = Convert.ToString(baltot);
                            dtIndividualReport.Rows.Add(drowInst);
                            grandbaltot = grandbaltot + Convert.ToDouble(dtIndividualReport.Rows[dtIndividualReport.Rows.Count - 1][6]);
                            //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        }
                        drowInst = dtIndividualReport.NewRow();
                        drowInst[0] = "Grand Total";
                        dicColSpanFormat1.Add(dtIndividualReport.Rows.Count, "Grand Total");
                        drowInst[5] = Convert.ToString(grandalltot);
                        drowInst[6] = Convert.ToString(grandpaidtot);
                        dtIndividualReport.Rows.Add(drowInst);
                        //grand balance
                        drowInst = dtIndividualReport.NewRow();
                        drowInst[0] = "Grand Balance";
                        dicColSpanFormat1.Add(dtIndividualReport.Rows.Count, "Grand Balance");
                        double grandbal = grandalltot - grandpaidtot;
                        drowInst[5] = Convert.ToString(grandbal);
                        drowInst[6] = Convert.ToString(grandbaltot);
                        dtIndividualReport.Rows.Add(drowInst);

                        drowInst = dtIndividualReport.NewRow();
                        dicColSpanFormat1.Add(dtIndividualReport.Rows.Count, "Empty");
                        dtIndividualReport.Rows.Add(drowInst);

                        grdIndividualReport.DataSource = dtIndividualReport;
                        grdIndividualReport.DataBind();
                        grdIndividualReport.Visible = true;

                        grdIndividualReport.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grdIndividualReport.Rows[0].Font.Bold = true;
                        grdIndividualReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                        GridViewRow rows = grdIndividualReport.Rows[0];
                        GridViewRow previousRow = grdIndividualReport.Rows[1];

                        for (int i = 0; i < dtIndividualReport.Columns.Count; i++)
                        {
                            if (rows.Cells[i].Text == previousRow.Cells[i].Text)
                            {
                                rows.Cells[i].RowSpan = 2;
                                previousRow.Cells[i].Visible = false;
                            }
                        }
                        //ColumnSpan

                        for (int cell = grdIndividualReport.Rows[0].Cells.Count - 1; cell > 0; cell--)
                        {
                            TableCell colum = grdIndividualReport.Rows[0].Cells[cell];
                            TableCell previouscol = grdIndividualReport.Rows[0].Cells[cell - 1];
                            if (colum.Text == previouscol.Text)
                            {
                                if (previouscol.ColumnSpan == 0)
                                {
                                    if (colum.ColumnSpan == 0)
                                    {
                                        previouscol.ColumnSpan += 2;
                                    }
                                    else
                                    {
                                        previouscol.ColumnSpan += colum.ColumnSpan + 1;
                                    }
                                    colum.Visible = false;
                                }
                            }
                        }
                        foreach (KeyValuePair<int, string> dr in dicColSpanFormat1)
                        {
                            int g = dr.Key;
                            string DicValue = dr.Value;
                            if (DicValue == "Roll No")
                            {
                                int colcount = dtIndividualReport.Columns.Count;
                                grdIndividualReport.Rows[g].Cells[0].ColumnSpan = colcount;
                                for (int a = 1; a < colcount; a++)
                                    grdIndividualReport.Rows[g].Cells[a].Visible = false;
                                grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                grdIndividualReport.Rows[g].BackColor = ColorTranslator.FromHtml("#cc66ff");
                            }
                            if (DicValue == "Total")
                            {
                                grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 2;
                                for (int a = 1; a < 2; a++)
                                    grdIndividualReport.Rows[g].Cells[a].Visible = false;
                                grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                grdIndividualReport.Rows[g].BackColor = Color.Gray;
                            }
                            if (DicValue == "Balance")
                            {
                                grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 2;
                                for (int a = 1; a < 2; a++)
                                    grdIndividualReport.Rows[g].Cells[a].Visible = false;
                                grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                grdIndividualReport.Rows[g].BackColor = Color.YellowGreen;
                            }
                            if (DicValue == "Grand Total")
                            {
                                grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 4;
                                for (int a = 1; a < 4; a++)
                                    grdIndividualReport.Rows[g].Cells[a].Visible = false;
                                grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                grdIndividualReport.Rows[g].BackColor = Color.Gold;
                            }
                            if (DicValue == "Grand Balance")
                            {
                                grdIndividualReport.Rows[g].Cells[0].ColumnSpan = 4;
                                for (int a = 1; a < 4; a++)
                                    grdIndividualReport.Rows[g].Cells[a].Visible = false;
                                grdIndividualReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                grdIndividualReport.Rows[g].BackColor = Color.LightSkyBlue;
                            }
                            if (DicValue == "Empty")
                            {
                                grdIndividualReport.Rows[g].Cells[0].ColumnSpan = dtIndividualReport.Columns.Count;
                                for (int a = 1; a < dtIndividualReport.Columns.Count; a++)
                                    grdIndividualReport.Rows[g].Cells[a].Visible = false;
                            }
                        }
                        // div1.Visible = true;
                        Error.Visible = false;
                        rprint.Visible = true;
                        lblsmserror.Visible = false;
                        txtexcel.Text = "";

                    }
                    else
                    {
                        rprint.Visible = false;
                        grdIndividualReport.Visible = false;
                        //div1.Visible = false;
                        Error.Visible = true;
                        Error.Text = "No Record Found!";
                    }
                }
            }
            else
            {
                rprint.Visible = false;
                grdIndividualReport.Visible = false;
                //  div1.Visible = false;
                Error.Visible = true;
                Error.Text = "Please Enter the Roll/Reg No!";
            }
        }
        catch { }
    }

    public string romanLetter(string numeral)
    {
        string romanLettervalue = String.Empty;
        if (numeral.Trim() != String.Empty)
        {
            switch (numeral)
            {
                case "1":
                    romanLettervalue = "I";
                    break;
                case "2":
                    romanLettervalue = "II";
                    break;
                case "3":
                    romanLettervalue = "III";
                    break;
                case "4":
                    romanLettervalue = "IV";
                    break;
                case "5":
                    romanLettervalue = "V";
                    break;
                case "6":
                    romanLettervalue = "VI";
                    break;
                case "7":
                    romanLettervalue = "VII";
                    break;
                case "8":
                    romanLettervalue = "VIII";
                    break;
                case "9":
                    romanLettervalue = "IX";
                    break;
                case "10":
                    romanLettervalue = "X";
                    break;
            }
        }
        return romanLettervalue;
    }

    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
    }

    //Code Last moidfied by Mohamed Idhris Sheik Dawood - 06-09-2016
    protected void txtno_TextChanged(object sender, EventArgs e)
    {
        btngo_click(sender, e);


    }

    protected void btnback_Click(object sender, EventArgs e)
    {

    }

    #region Print

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdIndividualReport, reportname);
                lblsmserror.Visible = false;
            }
            else
            {
                lblsmserror.Text = "Please Enter Your Report Name";
                lblsmserror.Visible = true;
                txtexcel.Focus();
            }
        }
        catch
        {

        }
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    { }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            // string degreedetails = "Student Fee Status";
            string collegeName = Convert.ToString(ddl_collegename.SelectedItem.Text);
            string degreedetails = collegeName + "\nStudent Fee Status Report" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy");
            string pagename = "Individual_StudentFeeStatus.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(grdIndividualReport, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
            lblsmserror.Visible = false;
        }
        catch
        {

        }
    }

    #endregion

    protected void ddl_hdr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_hdr.SelectedIndex == 0)
            {
                lbl_hdr.Text = "Group Header";
                bindgrouphdr();
            }
            if (ddl_hdr.SelectedIndex == 1)
            {
                lbl_hdr.Text = "Header";
                bindheader();
            }
            if (ddl_hdr.SelectedIndex == 2)
            {
                lbl_hdr.Text = "Ledger";
                bindledger();
            }
        }
        catch
        {

        }
    }

    public void bindgrouphdr()
    {
        try
        {
            cbl_header.Items.Clear();
            string loadgrphdr = " SELECT distinct G.ChlGroupHeader FROM FS_ChlGroupHeaderSettings G,FS_HeaderPrivilage P WHERE G.HeaderFK = P.HeaderFK AND P. UserCode = '" + usercode + "'  AND P.CollegeCode = " + collegecode1 + " ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(loadgrphdr, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_header.DataSource = ds;
                    cbl_header.DataTextField = "ChlGroupHeader";
                    cbl_header.DataValueField = "ChlGroupHeader";
                    cbl_header.DataBind();

                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        cbl_header.Items[i].Selected = true;
                    }
                    txtheader.Text = "Group Header(" + cbl_header.Items.Count + ")";
                    cb_header.Checked = true;
                }
                else
                {
                    txtheader.Text = "--Select--";
                    cb_header.Checked = false;
                }
            }
        }
        catch
        {

        }
    }

    public void bindledger()
    {
        try
        {
            cbl_header.Items.Clear();
            //    string loadledger = "select distinct LedgerPK,isnull(priority,1000), LedgerName from FM_LedgerMaster Where CollegeCode = " + collegecode1 + "  order by isnull(priority,1000), ledgerName asc ";
            string loadledger = "SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " and L.CollegeCode = " + collegecode1 + "   order by len(isnull(l.priority,1000)) , l.priority asc ";//AND  Ledgermode='0' 
            ds.Clear();
            ds = d2.select_method_wo_parameter(loadledger, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_header.DataSource = ds;
                    cbl_header.DataTextField = "LedgerName";
                    cbl_header.DataValueField = "LedgerPK";
                    cbl_header.DataBind();

                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        cbl_header.Items[i].Selected = true;
                    }
                    txtheader.Text = "Ledger(" + cbl_header.Items.Count + ")";
                    cb_header.Checked = true;
                }
                else
                {
                    txtheader.Text = "--Select--";
                    cb_header.Checked = false;
                }
            }
        }
        catch
        {

        }
    }

    public void bindheader()
    {
        try
        {
            cbl_header.Items.Clear();
            //string loadheader = "select distinct HeaderPK,HeaderName from FM_HeaderMaster Where CollegeCode = " + collegecode1 + "";
            string loadheader = " SELECT HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  order by len(isnull(hd_priority,10000)),hd_priority asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(loadheader, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_header.DataSource = ds;
                    cbl_header.DataTextField = "HeaderName";
                    cbl_header.DataValueField = "HeaderPK";
                    cbl_header.DataBind();

                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        cbl_header.Items[i].Selected = true;
                    }
                    txtheader.Text = "Header(" + cbl_header.Items.Count + ")";
                    cb_header.Checked = true;
                }
                else
                {
                    txtheader.Text = "--Select--";
                    cb_header.Checked = false;
                }
            }
        }
        catch
        {

        }
    }

    protected void bindsem()
    {
        try
        {
            cbl_fee.Items.Clear();
            cb_fee.Checked = false;
            txtfee.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = d2.loadFeecategory(Convert.ToString(ddl_collegename.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_fee.DataSource = ds;
                cbl_fee.DataTextField = "TextVal";
                cbl_fee.DataValueField = "TextCode";
                cbl_fee.DataBind();

                if (cbl_fee.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_fee.Items.Count; i++)
                    {
                        cbl_fee.Items[i].Selected = true;
                        cbltext = Convert.ToString(cbl_fee.Items[i].Text);
                    }
                    if (cbl_fee.Items.Count == 1)
                        txtfee.Text = "" + linkName + "(" + cbltext + ")";
                    else
                        txtfee.Text = "" + linkName + "(" + cbl_fee.Items.Count + ")";
                    cb_fee.Checked = true;
                }
            }
        }
        catch { }
    }

    //public void bindsem()
    //{
    //    try
    //    {
    //        //cbl_fee.Items.Clear();
    //        //string loadsem = "select distinct textcode,textval from textvaltable[t] inner join FT_FeeAllot[a] on a.FeeCategory=t.textcode where textcriteria='FEECA' and college_code='" + collegecode1 + "' ";
    //        //ds.Clear();
    //        //ds = d2.select_method_wo_parameter(loadsem, "Text");
    //        //if (ds.Tables.Count > 0)
    //        //{
    //        //    if (ds.Tables[0].Rows.Count > 0)
    //        //    {
    //        //        cbl_fee.DataSource = ds;
    //        //        cbl_fee.DataTextField = "textval";
    //        //        cbl_fee.DataValueField = "textcode";
    //        //        cbl_fee.DataBind();

    //        //        for (int i = 0; i < cbl_fee.Items.Count; i++)
    //        //        {
    //        //            cbl_fee.Items[i].Selected = true;
    //        //        }
    //        //        txtfee.Text = "Category(" + cbl_fee.Items.Count + ")";
    //        //        cb_fee.Checked = true;
    //        //    }
    //        //    else
    //        //    {
    //        //        txtfee.Text = "--Select--";
    //        //        cb_fee.Checked = false;
    //        //    }
    //        //}


    //        string sem = "";
    //        // string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
    //        string semyear = "select * from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "'and college_code ='" + collegecode1 + "'";
    //        DataSet dsset = new DataSet();
    //        dsset.Clear();
    //        dsset = d2.select_method_wo_parameter(semyear, "Text");
    //        if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
    //        {
    //            string value = Convert.ToString(dsset.Tables[0].Rows[0]["LinkValue"]);
    //            if (value == "1")
    //            {
    //                string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%'and college_code ='" + collegecode1 + "' order by len(textval),textval asc";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(SelectQ, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    cbl_fee.DataSource = ds;
    //                    cbl_fee.DataTextField = "textval";
    //                    cbl_fee.DataValueField = "TextCode";
    //                    cbl_fee.DataBind();
    //                }
    //                for (int i = 0; i < cbl_fee.Items.Count; i++)
    //                {
    //                    cbl_fee.Items[i].Selected = true;
    //                }
    //                txtfee.Text = "Category(" + cbl_fee.Items.Count + ")";
    //                cb_fee.Checked = true;

    //            }
    //            else
    //            {
    //                cbl_fee.Items.Clear();
    //                string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(settingquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //                    if (linkvalue == "0")
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + collegecode1 + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            cbl_fee.DataSource = ds;
    //                            cbl_fee.DataTextField = "textval";
    //                            cbl_fee.DataValueField = "TextCode";
    //                            cbl_fee.DataBind();
    //                        }
    //                        for (int i = 0; i < cbl_fee.Items.Count; i++)
    //                        {
    //                            cbl_fee.Items[i].Selected = true;
    //                        }
    //                        txtfee.Text = "Category(" + cbl_fee.Items.Count + ")";
    //                        cb_fee.Checked = true;
    //                    }
    //                    else
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + collegecode1 + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            cbl_fee.DataSource = ds;
    //                            cbl_fee.DataTextField = "textval";
    //                            cbl_fee.DataValueField = "TextCode";
    //                            cbl_fee.DataBind();
    //                        }
    //                        for (int i = 0; i < cbl_fee.Items.Count; i++)
    //                        {
    //                            cbl_fee.Items[i].Selected = true;
    //                        }
    //                        txtfee.Text = "Category(" + cbl_fee.Items.Count + ")";
    //                        cb_fee.Checked = true;
    //                    }
    //                }
    //            }

    //        }
    //    }
    //    catch
    //    {

    //    }
    //}

    public void LoadFromSettings()
    {
        try
        {
            System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("Roll No", "0");
            System.Web.UI.WebControls.ListItem lst2 = new System.Web.UI.WebControls.ListItem("Reg No", "1");
            System.Web.UI.WebControls.ListItem lst3 = new System.Web.UI.WebControls.ListItem("Admission No", "2");
            System.Web.UI.WebControls.ListItem lst4 = new System.Web.UI.WebControls.ListItem("App No", "3");
            System.Web.UI.WebControls.ListItem lst5 = new System.Web.UI.WebControls.ListItem("Name", "4");

            //Roll Number or Reg Number or Admission No or Application Number
            ddladmit.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                ddladmit.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                ddladmit.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                ddladmit.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //App Form Number - Application Number
                ddladmit.Items.Add(lst4);

            }
            if (ddladmit.Items.Count == 0)
            {
                ddladmit.Items.Add(lst1);
            }
            ddladmit.Items.Add(lst5);
            switch (Convert.ToUInt32(ddladmit.SelectedItem.Value))
            {
                case 0:
                    txtno.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txtno.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txtno.Attributes.Add("placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txtno.Attributes.Add("placeholder", "App No");
                    chosedmode = 3;
                    break;
                case 4:
                    txtno.Attributes.Add("placeholder", "");
                    chosedmode = 4;
                    break;
            }
        }
        catch { }
    }

    #region roll no Lookup

    public void bindType()
    {
        try
        {
            cbl_strm.Items.Clear();
            cb_strm.Checked = false;
            txt_strm.Text = "--Select--";
            string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type<>'' order by type asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_strm.DataSource = ds;
                cbl_strm.DataTextField = "type";
                cbl_strm.DataValueField = "type";
                cbl_strm.DataBind();
                if (cbl_strm.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_strm.Items.Count; i++)
                    {
                        cbl_strm.Items[i].Selected = true;
                    }
                    txt_strm.Text = "Stream(" + cbl_strm.Items.Count + ")";
                    cb_strm.Checked = true;
                }
                txt_strm.Enabled = true;
            }
            else
            {
                txt_strm.Enabled = false;
            }
        }
        catch
        {
        }
    }

    public void bindbatch1()
    {
        try
        {
            ddl_batch1.Items.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            ds = d2.select_method_wo_parameter(sqlyear, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch1.DataSource = ds;
                ddl_batch1.DataTextField = "batch_year";
                ddl_batch1.DataValueField = "batch_year";
                ddl_batch1.DataBind();
            }
        }
        catch
        {
        }
    }

    public void binddegree2()
    {
        try
        {
            ds.Clear();
            cbl_degree2.Items.Clear();
            string stream = "";
            if (cbl_strm.Items.Count > 0)
            {
                for (int i = 0; i < cbl_strm.Items.Count; i++)
                {
                    if (cbl_strm.Items[i].Selected == true)
                    {
                        if (stream == "")
                        {
                            stream = Convert.ToString(cbl_strm.Items[i].Value);
                        }
                        else
                        {
                            stream = stream + "'" + "," + "'" + Convert.ToString(cbl_strm.Items[i].Value);
                        }
                    }
                }
            }
            txt_degree2.Text = "--Select--";

            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            string colleges = Convert.ToString(d2.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegecode1;
            }
            string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegecode1 + ") and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + " ";
            if (txt_strm.Enabled)
            {
                query += " and course.type in ('" + stream + "')";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree2.DataSource = ds;
                cbl_degree2.DataTextField = "course_name";
                cbl_degree2.DataValueField = "course_id";
                cbl_degree2.DataBind();
                if (cbl_degree2.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree2.Items.Count; i++)
                    {
                        cbl_degree2.Items[i].Selected = true;
                    }
                    txt_degree2.Text = lbl_degree2.Text + "(" + cbl_degree2.Items.Count + ")";
                    cb_degree2.Checked = true;
                }
                else
                {
                    txt_degree2.Text = "--Select--";
                }
            }
            else
            {
                txt_degree2.Text = "--Select--";
            }

        }
        catch { }
    }

    public void bindbranch1()
    {
        try
        {
            cbl_branch1.Items.Clear();

            string branch = "";
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    if (branch == "")
                    {
                        branch = "" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        branch = branch + "'" + "," + "" + "'" + cbl_degree2.Items[i].Value.ToString() + "";
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
            if (branch.Trim() != "")
            {
                ds = d2.select_method_wo_parameter(commname, "Text");
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
                        txt_branch2.Text = lbl_branch2.Text + "(" + cbl_branch1.Items.Count + ")";
                        cb_branch1.Checked = true;
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

    public void bindsec2()
    {
        try
        {
            cbl_sec2.Items.Clear();
            txt_sec2.Text = "--Select--";
            ListItem item = new ListItem("Empty", " ");
            if (ddl_batch1.Items.Count > 0)
            {
                string strbatch = Convert.ToString(ddl_batch1.SelectedItem.Value);
                string branch = "";
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    if (cbl_branch1.Items[i].Selected == true)
                    {
                        if (branch == "")
                        {
                            branch = "" + cbl_branch1.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            branch = branch + "" + "," + "" + "" + cbl_branch1.Items[i].Value.ToString() + "";
                        }
                    }
                }
                if (branch != "")
                {
                    DataSet dsSec = d2.BindSectionDetail(strbatch, branch);
                    if (dsSec.Tables.Count > 0)
                    {
                        if (dsSec.Tables[0].Rows.Count > 0)
                        {
                            cbl_sec2.DataSource = dsSec;
                            cbl_sec2.DataTextField = "sections";
                            cbl_sec2.DataValueField = "sections";
                            cbl_sec2.DataBind();


                        }
                    }
                    cbl_sec2.Items.Insert(0, item);
                    for (int i = 0; i < cbl_sec2.Items.Count; i++)
                    {
                        cbl_sec2.Items[i].Selected = true;
                    }
                    cb_sec2.Checked = true;
                    txt_sec2.Text = "Section(" + cbl_sec2.Items.Count + ")";

                }
            }


        }
        catch { }
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string selectquery = "";
            string batch_year = Convert.ToString(ddl_batch1.SelectedItem.Text);
            string stream = Convert.ToString(getCblSelectedValue(cbl_strm));
            string degree = Convert.ToString(getCblSelectedValue(cbl_degree2));
            string branch = Convert.ToString(getCblSelectedValue(cbl_branch1));
            string sec = Convert.ToString(getCblSelectedValue(cbl_sec2));
            string PaidFilter = string.Empty;

            #region include distcont

            string cc = "";
            string debar = "";
            string disc = "";
            string commondist = "";
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    if (cblinclude.Items[i].Selected == true)
                    {
                        if (cblinclude.Items[i].Value == "1")
                        {
                            cc = " r.cc=1";
                        }
                        if (cblinclude.Items[i].Value == "2")
                        {
                            debar = "  r.Exam_Flag like '%debar'";
                        }
                        if (cblinclude.Items[i].Value == "3")
                        {
                            disc = " r.DelFlag=1";
                        }
                    }
                }
            }
            if (cc != "" && debar == "" && disc == "")
                commondist = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";

            if (cc == "" && debar != "" && disc == "")
                commondist = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

            if (cc == "" && debar == "" && disc != "")
                commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";

            if (cc != "" && debar != "" && disc == "")
                commondist = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

            if (cc == "" && debar != "" && disc != "")
                commondist = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar')  and (" + disc + " or r.DelFlag=0)";

            if (cc != "" && debar == "" && disc != "")
                commondist = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar'  and (" + disc + " or r.DelFlag=0)";

            else if (cc == "" && debar == "" && disc == "")
                commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";

            if (cc != "" && debar != "" && disc != "")
                commondist = "";

            #endregion

            if (txt_rollno3.Text != "")
            {
                selectquery = "select Roll_No,Roll_Admit,smart_serial_no,Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and r.roll_no ='" + txt_rollno3.Text + "'";
                if (commondist != "")
                    selectquery = selectquery + commondist;
            }
            else
            {
                selectquery = "select Roll_No,Roll_Admit,smart_serial_no,Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and Batch_Year =" + batch_year + " and r.degree_code in ('" + branch + "')  and isnull(r.Sections,'') in ('" + sec + "') ";
                if (commondist != "")
                    selectquery = selectquery + commondist;
            }

            // selectquery = "select Roll_No,Roll_Admit,smart_serial_no,Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' ";         

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtStudent = new DataTable();
                DataRow drowInst;
                ArrayList arrColHdrNames = new ArrayList();
                arrColHdrNames.Add("S.No");
                arrColHdrNames.Add("Roll No");
                arrColHdrNames.Add("Reg No");
                arrColHdrNames.Add("Name");
                arrColHdrNames.Add(lbl_degree2.Text);
                dtStudent.Columns.Add("Sno");
                dtStudent.Columns.Add("Roll No");
                dtStudent.Columns.Add("Reg No");
                dtStudent.Columns.Add("Name");
                dtStudent.Columns.Add(lbl_degree2.Text);
                DataRow drHdr1 = dtStudent.NewRow();
                for (int grCol = 0; grCol < dtStudent.Columns.Count; grCol++)
                    drHdr1[grCol] = arrColHdrNames[grCol];
                dtStudent.Rows.Add(drHdr1);
                #region design
                //Fpspread2.Visible = true;
                //Fpspread2.Sheets[0].RowCount = 1;
                //Fpspread2.Sheets[0].ColumnCount = 0;
                //Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                //Fpspread2.CommandBar.Visible = false;
                //Fpspread2.Sheets[0].ColumnCount = 6;
                //Fpspread2.Sheets[0].RowHeader.Visible = false;
                //FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                //darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                //darkstyle.ForeColor = Color.White;
                //Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].Columns[0].Locked = true;
                //Fpspread2.Columns[0].Width = 50;

                //FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                //chkall.AutoPostBack = true;

                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Columns[1].Width = 80;
                //Fpspread2.Sheets[0].Columns[1].Locked = false;
                //Fpspread2.Sheets[0].Columns[1].Visible = false;
                //Fpspread2.Sheets[0].Cells[0, 1].CellType = chkall;
                //Fpspread2.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].Columns[2].Locked = true;
                //Fpspread2.Columns[2].Width = 100;

                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].Columns[3].Locked = true;
                //Fpspread2.Columns[4].Width = 100;

                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].Columns[4].Locked = true;
                //Fpspread2.Columns[4].Width = 200;

                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = lbl_degree2.Text;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].Columns[5].Locked = true;
                //Fpspread2.Columns[5].Width = 308;


                //FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                //FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                //FarPoint.Web.Spread.TextCellType txtRollAd = new FarPoint.Web.Spread.TextCellType();
                //FarPoint.Web.Spread.TextCellType txtAppno = new FarPoint.Web.Spread.TextCellType();
                //FarPoint.Web.Spread.TextCellType txtSmartno = new FarPoint.Web.Spread.TextCellType();

                //if (rbstudtype.SelectedItem.Value == "2")
                //{
                //    Fpspread2.Sheets[0].Columns[1].Locked = false;
                //    Fpspread2.Sheets[0].Columns[1].Visible = true;
                //    Fpspread2.Sheets[0].AutoPostBack = false;
                //    Fpspread2.Height = 250;
                //}
                //else
                //{
                //    Fpspread2.Sheets[0].Columns[1].Locked = true;
                //    Fpspread2.Sheets[0].Columns[1].Visible = false;
                //    Fpspread2.Sheets[0].AutoPostBack = true;
                //}
                #endregion

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    drowInst = dtStudent.NewRow();
                    drowInst["Sno"] = Convert.ToString(row + 1);
                    drowInst["Roll No"] = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                    drowInst["Reg No"] = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                    drowInst["Name"] = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                    drowInst[lbl_degree2.Text] = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);
                    dtStudent.Rows.Add(drowInst);
                }
                if (rbstudtype.SelectedItem.Value == "1")
                {
                    chkGridSelectAll.Visible = false;
                }
                else
                    chkGridSelectAll.Visible = true;
                GrdStudent.DataSource = dtStudent;
                GrdStudent.DataBind();
                GrdStudent.Visible = true;
                btn_studOK.Visible = true;
                btn_exitstud.Visible = true;
                lbldisp.Visible = false;
                GrdStudent.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                GrdStudent.Rows[0].Font.Bold = true;
                GrdStudent.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                GrdStudent.Visible = false;
                lbl_errormsg.Visible = true;
                lbl_errormsg.Text = "No Records Found";
                btn_studOK.Visible = false;
                btn_exitstud.Visible = false;
                lbldisp.Visible = false;
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void GrdStudent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                e.Row.Cells[1].Text = "Select";
            }
            e.Row.Cells[2].Visible = false;
        }
    }

    public void btn_studOK_Click(object sender, EventArgs e)
    {
        try
        {
            string rollno = "";
            string activerow = "";
            string activecol = "";
            string rollval = "";
            int cnT = 0;
            //if (Fpspread2.Sheets[0].RowCount != 0)
            //{
            //    activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
            //    activecol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
            if (rbstudtype.SelectedItem.Value == "1")
            {
                foreach (GridViewRow gvrow in GrdStudent.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                        rollno = Convert.ToString(GrdStudent.Rows[RowCnt].Cells[3].Text);
                        txtno.Text = Convert.ToString(rollno);
                        popwindow.Visible = false;
                    }
                }
            }
            else
            {
                lblrolldisp.Text = "";
                lbldisp.Text = "";
                foreach (GridViewRow gvrow in GrdStudent.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                        cnT++;
                        if (rollval == "")
                            rollval = Convert.ToString(GrdStudent.Rows[RowCnt].Cells[3].Text);
                        else
                            rollval = rollval + "," + Convert.ToString(GrdStudent.Rows[RowCnt].Cells[3].Text);
                    }
                }
                lblrolldisp.Text = rollval;
                lbldisp.Text = Convert.ToString("You Have Selected " + cnT + " Students");
                lbldisp.Visible = true;
                popwindow.Visible = false;
            }

            //}
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_exitstud_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
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

    protected void cb_strm_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_strm, cb_strm, txt_strm, lbl_stream.Text);
        binddegree2();
        bindbranch1();
        bindsec2();
    }

    protected void cbl_strm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_strm, cb_strm, txt_strm, lbl_stream.Text);
        binddegree2();
        bindbranch1();
        bindsec2();
    }

    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");
        bindsec2();
    }

    protected void cb_branch1_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");
        bindsec2();
    }

    protected void cbl_degree2_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        bindbranch1();
        bindsec2();
    }

    protected void cb_degree2_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        bindbranch1();
        bindsec2();
    }

    protected void cb_sec2_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_sec2, cb_sec2, txt_sec2, "Section");
    }

    protected void cbl_sec2_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_sec2, cb_sec2, txt_sec2, "Section");
    }

    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }

    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }

    #endregion

    #region Include setting
    protected void checkdicon_Changed(object sender, EventArgs e)
    {
        try
        {
            if (checkdicon.Checked == true)
            {
                txtinclude.Enabled = true;
                LoadIncludeSetting();
            }
            else
            {
                txtinclude.Enabled = false;
                cblinclude.Items.Clear();
                // LoadIncludeSetting();
            }
        }
        catch { }
    }

    private void LoadIncludeSetting()
    {
        try
        {
            cblinclude.Items.Clear();
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Course Completed", "1"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Debar", "2"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Discontinue", "3"));
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    cblinclude.Items[i].Selected = true;
                }
                cbinclude.Checked = true;
                txtinclude.Text = "Include Settings(" + cblinclude.Items.Count + ")";
            }
        }
        catch { }
    }


    protected void cbinclude_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxChangedEvent(cblinclude, cbinclude, txtinclude, "Include Setting");
        }
        catch { }
    }
    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxListChangedEvent(cblinclude, cbinclude, txtinclude, "Include Setting");
        }
        catch { }
    }

    #endregion

    protected void UserbasedRights()
    {
        string userrht = d2.GetFunction("select value from Master_Settings where settings='Finance Include User Based Report Settings'  and usercode='" + usercode + "'");
        if (userrht == "1")
            usBasedRights = true;
        else
            usBasedRights = false;

    }

    #region student type

    protected string getClgCode()
    {
        string clgCode = string.Empty;
        try
        {
            StringBuilder sbClg = new StringBuilder();
            for (int row = 0; row < ddl_collegename.Items.Count; row++)
            {
                sbClg.Append(Convert.ToString(ddl_collegename.Items[row].Value) + "','");
            }
            if (sbClg.Length > 0)
            {
                clgCode = Convert.ToString(sbClg.Remove(sbClg.Length - 3, 3));
            }
        }
        catch { }
        return clgCode;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]

    protected void ddlsearch1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtsearch1.Text = "";
        txtsearch1c.Text = "";
        txtsearch1c.Visible = false;
        txtsearch1.Visible = false;
        if (ddlsearch1.SelectedIndex == 0)
        {
            txtsearch1.Visible = true;
            //Label1.Text = "Search By Name";
        }
        else
        {
            txtsearch1c.Visible = true;
            //Label1.Text = "Search By Code";
        }
    }

    protected void rbstudtype_Selected(object sender, EventArgs e)
    {
        if (rbstudtype.SelectedItem.Value == "1")
        {
            txtno.Enabled = true;
            ddladmit.Enabled = true;
            lbldisp.Visible = false;
            lbldisp.Text = "";
            txtno.Text = "";
            grdIndividualReport.Visible = false;
            // div1.Visible = false;
            Error.Visible = false;
            rprint.Visible = false;
            lblsmserror.Visible = false;
            txtexcel.Text = "";
        }
        else
        {
            grdIndividualReport.Visible = false;
            txtno.Enabled = false;
            ddladmit.Enabled = false;
            lbldisp.Visible = false;
            lbldisp.Text = "";
            txtno.Text = "";
            //  div1.Visible = false;
            Error.Visible = false;
            rprint.Visible = false;
            lblsmserror.Visible = false;
            txtexcel.Text = "";
        }
    }

    #endregion

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

        lbl.Add(lbl_collegename);
        lbl.Add(lblcate);
        fields.Add(0);
        fields.Add(4);

        lbl.Add(lbl_stream);
        lbl.Add(lbl_degree2);
        lbl.Add(lbl_branch2);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    protected void ddlViewFormat_Selected(object sender, EventArgs e)
    {
        rbstudtype.Items.Clear();
        if (ddlViewFormat.SelectedIndex == 0)
        {
            rbstudtype.Items.Add(new ListItem("Single", "1"));
            rbstudtype.Items.Add(new ListItem("Multiple", "2"));
            rbstudtype.SelectedIndex = 0;
        }
        else if (ddlViewFormat.SelectedIndex == 1)
        {
            rbstudtype.Items.Add(new ListItem("Single", "1"));
            rbstudtype.SelectedIndex = 0;
        }
        else if (ddlViewFormat.SelectedIndex == 2)
        {
            rbstudtype.Items.Add(new ListItem("Single", "1"));
            rbstudtype.Items.Add(new ListItem("Multiple", "2"));
            rbstudtype.SelectedIndex = 0;
        }
    }

    //added by sudhagar 21.08.2017 paavai  formart 3

    protected DataSet dsFormat3(string AppNo, string rollType, bool beforeAdm)
    {
        DataSet dsLoad = new DataSet();
        try
        {
            string college = string.Empty;
            string rptType = string.Empty;
            string headerid = string.Empty;
            string semcode = string.Empty;
            string transerText = string.Empty;
            // string AppNo = string.Empty;
            // string rollType = string.Empty;
            //  bool beforeAdm = false;
            if (ddl_collegename.Items.Count > 0)
                college = Convert.ToString(ddl_collegename.SelectedValue);
            if (ddl_hdr.SelectedIndex == 0)
                rptType = "Group Header";
            else if (ddl_hdr.SelectedIndex == 1)
                rptType = "Header";
            else if (ddl_hdr.SelectedIndex == 2)
                rptType = "Ledger";
            if (cbl_header.Items.Count > 0)
                headerid = Convert.ToString(getCblSelectedValue(cbl_header));
            if (cbl_fee.Items.Count > 0)
                semcode = Convert.ToString(getCblSelectedValue(cbl_fee));

            #region Query
            string Transrcpt = string.Empty;
            string transType = string.Empty;
            if (checkSchoolSetting() != 0)//school
            {
                if (!cbTrans.Checked)
                {
                    transType = "  and isnull(paid_Istransfer,'0')='0' and( isnull(receipttype,'0')<>'3' and isnull(receipttype,'0')<>'0')";//and isnull(transtype,'0')='1'
                    transerText = " and istransfer='0'";
                    Transrcpt = " and isnull(paid_Istransfer,'0')='0' ";
                    //   string getApp = d2.GetFunction("select appno from st_student_transfer s,st_student_transfer_details sd where s.studenttransferpk=sd.studenttransferfk and appno='" + AppNo + "'");
                    //if (getApp != "0")
                    //{
                    //    transerText = " and istransfer='0'";
                    //    Transrcpt = " and isnull(paid_Istransfer,'0')='0' and( isnull(receipttype,'0')<>'3' and isnull(receipttype,'0')<>'0')";
                    //    //Transrcpt = " and isnull(paid_Istransfer,'0')='0' and transdate>= ( select transferdate from st_student_transfer s,st_student_transfer_details sd where s.studenttransferpk=sd.studenttransferfk and appno='" + AppNo + "')";
                    //}
                }
                else
                {
                    transType = "  and isnull(paid_Istransfer,'0')='0' and isnull(receipttype,'0')<>'6'";//and isnull(transtype,'0')='3'
                    transerText = " and istransfer='1'";
                    Transrcpt = " and isnull(paid_Istransfer,'0')='1' and isnull(transtype,'0')<>'3' ";
                    // Transrcpt = " and isnull(paid_Istransfer,'0')='0' and transdate<= ( select transferdate from st_student_transfer s,st_student_transfer_details sd where s.studenttransferpk=sd.studenttransferfk and appno='" + AppNo + "') and credit is not null  ";
                }
            }
            if (checkSchoolSetting() == 0)//school
            {
                if (!cbTrans.Checked)
                {
                    transType = "  and isnull(paid_Istransfer,'0')='0' and( isnull(receipttype,'0')<>'3')";//and isnull(transtype,'0')='1'//and isnull(receipttype,'0')<>'0' commented by saranya on 13/3/2018
                    transerText = " and istransfer='0'";
                    Transrcpt = " and isnull(paid_Istransfer,'0')='0' ";
                    //   string getApp = d2.GetFunction("select appno from st_student_transfer s,st_student_transfer_details sd where s.studenttransferpk=sd.studenttransferfk and appno='" + AppNo + "'");
                    //if (getApp != "0")
                    //{
                    //    transerText = " and istransfer='0'";
                    //    Transrcpt = " and isnull(paid_Istransfer,'0')='0' and( isnull(receipttype,'0')<>'3' and isnull(receipttype,'0')<>'0')";
                    //    //Transrcpt = " and isnull(paid_Istransfer,'0')='0' and transdate>= ( select transferdate from st_student_transfer s,st_student_transfer_details sd where s.studenttransferpk=sd.studenttransferfk and appno='" + AppNo + "')";
                    //}
                }
                else
                {
                    transType = "  and isnull(paid_Istransfer,'0')='0' and isnull(receipttype,'0')<>'6'";//and isnull(transtype,'0')='3'
                    transerText = " and istransfer='1'";
                    Transrcpt = " and isnull(paid_Istransfer,'0')='1' and isnull(transtype,'0')<>'3' ";
                    // Transrcpt = " and isnull(paid_Istransfer,'0')='0' and transdate<= ( select transferdate from st_student_transfer s,st_student_transfer_details sd where s.studenttransferpk=sd.studenttransferfk and appno='" + AppNo + "') and credit is not null  ";
                }
            }

            string type = d2.GetFunction("select c.type from Registration r,Degree d,Course c where r.degree_code =d.Degree_Code and d.Course_Id =c.Course_Id and App_No ='" + AppNo + "' ");


            string finlYrStr = string.Empty;
            string selFinYra = string.Empty;
            string actfinlYrStr = string.Empty;
            string selFinYr = string.Empty;
            string selFinYrEx = string.Empty;
            string selCol = string.Empty;
            string GrpselCol = string.Empty;

            if (checkSchoolSetting() == 0)//school
            {
                #region
                selFinYr = " ,(select convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)) from fm_finyearmaster where finyearpk=isnull(f.FinYearFK,'0'))as actualfinyearfk";
                selFinYra = " ,(select convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)) from fm_finyearmaster where finyearpk=isnull(d.actualfinyearfk,'0'))as actualfinyearfk";//abarna
                selFinYrEx = " actualfinyearfk";

                StringBuilder sbFinlYr = new StringBuilder();

                Dictionary<string, string> htFinlYR = getFinancialYear();
                if (chklsfyear.Items.Count > 0)
                {
                    for (int fnl = 0; fnl < chklsfyear.Items.Count; fnl++)
                    {
                        if (!chklsfyear.Items[fnl].Selected)
                            continue;
                        for (int clg = 0; clg < ddl_collegename.Items.Count; clg++)
                        {
                            if (!ddl_collegename.Items[clg].Selected)
                                continue;
                            string KeyVal = htFinlYR.Keys.FirstOrDefault(x => htFinlYR[x] == chklsfyear.Items[fnl].Text + "-" + ddl_collegename.Items[clg].Value);//to pass value get key from dictionary  //+ "-"+ cblclg.Items[clg].Value
                            sbFinlYr.Append(KeyVal + "','");
                        }

                    }
                    if (sbFinlYr.Length > 0)
                        sbFinlYr.Remove(sbFinlYr.Length - 3, 3);

                }
                actfinlYrStr = " and d.actualfinyearfk in('" + Convert.ToString(sbFinlYr) + "')";//for school actualfinyearfk
                finlYrStr = " and f.FinYearFK in('" + Convert.ToString(sbFinlYr) + "')";
                //selCol =  + selFinYr + "";
                GrpselCol = ",actualfinyearfk";
                #endregion
            }
            string selq = "";

            if (ddl_hdr.SelectedIndex == 0)
            {
                #region For College

                if (checkSchoolSetting() != 0)
                {
                    if (!beforeAdm)
                    {
                        #region group header
                        //allot detail
                        selq = "  select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  r.App_No=f.App_No and ch.HeaderFK=f.HeaderFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "'" + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        //  selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  f.HeaderFK=ch.HeaderFK and r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],G.ChlGroupHeader as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r where  r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK";
                        //paid detail                   
                        selq = selq + "union select  Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand], SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and  f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "'  and isnull(IsCanceled,'0')='0'  and isnull(debit,'0')>'0'  " + transerText + " " + Transrcpt + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode";//and isnull(credit,'0')='0' and  ISNULL(IsCollected,0)='1'
                        if (cbTrans.Checked || !cbTrans.Checked)
                        {
                            selq = selq + " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,d.Narration,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate  from FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' and isnull(IsCanceled,'0')='0'  and isnull(debit,'0')='0'  and isnull(credit,'0')>'0' " + transType + "    group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory"; //and  ISNULL(IsCollected,0)='1'
                            selq += " order by  TDate,AllotOrPaid,collvalue";
                        }
                        selq += "  select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t,FS_ChlGroupHeaderSettings G where f.App_No=r.App_No and f.headerfk=g.headerfk and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and g.ChlGroupHeader in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),f.HeaderFK ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                        //paid detail                   
                        // selq = selq + " select SUM(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype  from FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "'  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0' and isnull(credit,'0')>'0' " + transerText + " group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype";

                        #endregion
                    }
                    else
                    {
                        #region group header
                        //allot detail
                        selq = "  select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen from applyn r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  r.App_No=f.App_No and ch.HeaderFK=f.HeaderFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        //   selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  f.HeaderFK=ch.HeaderFK and r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],G.ChlGroupHeader as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,v r where  r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK";

                        //paid detail                   
                        selq = selq + "union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "'  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  " + transerText + " " + Transrcpt + " and isnull(debit,'0')>'0' group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode ";//and isnull(credit,'0')='0'
                        if (cbTrans.Checked || !cbTrans.Checked)
                        {
                            selq = selq + " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,d.Narration,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate  from FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0' and isnull(credit,'0')>'0' " + transType + "    group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory";
                            selq += " order by  TDate,AllotOrPaid,collvalue";
                        }

                        selq += "  select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue  from FT_FeeAllot f,applyn r,textvaltable t,FS_ChlGroupHeaderSettings G where f.App_No=r.App_No and f.headerfk=g.headerfk and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and g.ChlGroupHeader in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),f.HeaderFK ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                        //paid detail                   
                        // selq = selq + " select SUM(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype  from FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "'  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0' and isnull(credit,'0')>'0' " + transerText + " group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype";

                        #endregion
                    }
                }
                #endregion

                #region For School setting Added by saranya on 09/02/2018
                if (checkSchoolSetting() == 0)
                {
                    btnprint.Visible = false;
                    if (!beforeAdm)
                    {
                        #region group header
                        //allot detail
                        selq = "  select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen " + selFinYr + " from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and r.App_No=f.App_No and ch.HeaderFK=f.HeaderFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and fn.FinYearPK=f.FinYearFK and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "'" + transerText + " " + finlYrStr + " order by  " + selFinYrEx + " ";
                        //  selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  f.HeaderFK=ch.HeaderFK and r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],G.ChlGroupHeader as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],(TotalAmount) as [Demand],(BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate " + selFinYr + " from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and fn.FinYearPK=f.FinYearFK and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " " + finlYrStr + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK,f.FinYearFK,TotalAmount,BalAmount " + GrpselCol + "  ";
                        //paid detail                   
                        selq = selq + "union select  Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],(debit) as [Receipt],'0' [Demand], (BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + " from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=d.App_No and d.App_No=f.App_No and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "'  and isnull(IsCanceled,'0')='0'  and isnull(debit,'0')>'0'  " + transerText + " " + Transrcpt + " " + actfinlYrStr + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode,f.FinYearFK,debit,BalAmount " + GrpselCol + " ";//and isnull(credit,'0')='0' and  ISNULL(IsCollected,0)='1'
                        if (cbTrans.Checked || !cbTrans.Checked)
                        {
                            selq = selq + " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,d.Narration,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + " from FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d,FM_FinYearMaster fn where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' and isnull(IsCanceled,'0')='0'  and isnull(debit,'0')='0' and fn.FinYearPK=f.FinYearFK  and isnull(credit,'0')>'0' " + transType + " " + actfinlYrStr + "   group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory,f.FinYearFK,credit " + GrpselCol + " "; //and  ISNULL(IsCollected,0)='1'
                            selq += " order by  " + selFinYrEx + " ";
                        }
                        selq += "  select distinct textval,isnull(DeductAmout,0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue " + selFinYr + " from FT_FeeAllot f,Registration r,textvaltable t,FS_ChlGroupHeaderSettings G,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and f.App_No=r.App_No and f.headerfk=g.headerfk and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and fn.FinYearPK=f.FinYearFK and g.ChlGroupHeader in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " " + finlYrStr + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),f.HeaderFK,f.FinYearFK,DeductAmout " + GrpselCol + " ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                        //paid detail                   
                        // selq = selq + " select SUM(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype  from FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "'  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0' and isnull(credit,'0')>'0' " + transerText + " group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype";

                        #endregion
                    }
                    else
                    {
                        #region group header
                        //allot detail
                        selq = "  select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen " + selFinYr + " from applyn r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and fn.FinYearPK=f.FinYearFK and d.App_No=f.App_No and  r.App_No=f.App_No and ch.HeaderFK=f.HeaderFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " " + finlYrStr + " order by  " + selFinYrEx + "";
                        //   selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  f.HeaderFK=ch.HeaderFK and r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],G.ChlGroupHeader as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],(TotalAmount) as [Demand],(BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate  " + selFinYr + " from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and fn.FinYearPK=f.FinYearFK and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " " + finlYrStr + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK,f.FinYearFK,TotalAmount,BalAmount " + GrpselCol + " ";

                        //paid detail                   
                        selq = selq + "union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],(debit) as [Receipt],'0' [Demand],(BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + " from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "'  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  " + transerText + " " + Transrcpt + " and isnull(debit,'0')>'0' " + actfinlYrStr + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode,f.FinYearFK,debit,BalAmount " + GrpselCol + " ";//and isnull(credit,'0')='0'
                        if (cbTrans.Checked || !cbTrans.Checked)
                        {
                            selq = selq + " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,d.Narration,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + " from FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d,FM_FinYearMaster fn where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' and isnull(IsCanceled,'0')='0' and fn.FinYearPK=f.FinYearFK and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0' and isnull(credit,'0')>'0' " + transType + " " + actfinlYrStr + "   group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory,f.FinYearFK,credit " + GrpselCol + " ";
                            selq += " order by  " + selFinYrEx + "";
                        }

                        selq += "  select distinct textval,isnull(DeductAmout,0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue " + selFinYr + " from FT_FeeAllot f,applyn r,textvaltable t,FS_ChlGroupHeaderSettings G,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and f.App_No=r.App_No and f.headerfk=g.headerfk and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and fn.FinYearPK=f.FinYearFK and g.ChlGroupHeader in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " " + finlYrStr + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),f.HeaderFK,f.FinYearFK,DeductAmout " + GrpselCol + " ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                        //paid detail                   
                        // selq = selq + " select SUM(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype  from FS_ChlGroupHeaderSettings G,applyn r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "'  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0' and isnull(credit,'0')>'0' " + transerText + " group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype";

                        #endregion
                    }
                }
                #endregion

            }
            if (ddl_hdr.SelectedIndex == 1)
            {
                #region For College
                if (checkSchoolSetting() != 0)
                {
                    if (!beforeAdm)
                    {
                        #region header

                        //allot detail query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen from Registration r,TextValTable t,FT_FeeAllot f where    r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                        //   selq = selq + "  select distinct (r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where   r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + "  select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],H.HeaderName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate  from FT_FeeAllot F,Registration r,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK  and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + "  group by f.FeeCategory,f.App_No,AllotDate,F.HeaderFK,h.HeaderName  ";

                        //Paid Detail query                  
                        selq = selq + " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate   from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0'  " + transerText + " " + Transrcpt + "  and isnull(debit,'0')>'0'  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode,ddno,ddbankcode,d.paymode ";//and isnull(credit,'0')='0' and  ISNULL(IsCollected,0)='1' 
                        if (!cbTrans.Checked || cbTrans.Checked)
                        {
                            selq += " union  select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate  from Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "') " + transType + "     and isnull(IsCanceled,'0')='0'  and isnull(credit,'0')>0 and isnull(debit,'0')='0'    group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory   ";
                            //and  ISNULL(IsCollected,0)='1'
                            selq += " order by  TDate,AllotOrPaid,collvalue";
                        }

                        //deduction 
                        //selq += " select textval,isnull(SUM(DeductAmout),0) from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headerid + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and FinYearFK in(" + actidquery + ")  group by TextCode,textval";                   

                        selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue from FT_FeeAllot f,Registration r,textvaltable t,FT_FinDailyTransaction d where f.App_No=r.App_No and d.app_no=r.app_no and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.headerfk in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + "   group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK  ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                        //  selq += "    select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype from Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype ";

                        #endregion
                    }
                    else
                    {
                        #region header

                        //allot detail query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen from applyn r,TextValTable t,FT_FeeAllot f where    r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                        //  selq = selq + "  select distinct (r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f where   r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + "  select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],H.HeaderName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate from FT_FeeAllot F,applyn r,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK  and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " group by f.FeeCategory,f.App_No,AllotDate,F.HeaderFK,h.HeaderName";

                        //Paid Detail query                  
                        selq = selq + " union select  Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0'  " + transerText + " " + Transrcpt + " and isnull(debit,'0')>'0' group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode ";
                        //and isnull(credit,'0')='0' and  ISNULL(IsCollected,0)='1'  
                        if (cbTrans.Checked || !cbTrans.Checked)
                        {
                            selq += " union  select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "') " + transType + "   and isnull(IsCanceled,'0')='0' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory ";// and  ISNULL(IsCollected,0)='1'
                            selq += " order by  TDate,AllotOrPaid,collvalue";
                        }
                        selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue  from FT_FeeAllot f,applyn r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.headerfk in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                        //  selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype from applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype ";

                        #endregion
                    }
                }
                #endregion

                #region For School setting Added by saranya on 09/02/2018
                if (checkSchoolSetting() == 0)
                {
                    btnprint.Visible = false;
                    if (!beforeAdm)
                    {
                        #region header

                        //allot detail query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen " + selFinYr + " from Registration r,TextValTable t,FT_FeeAllot f,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and  r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " " + finlYrStr + " and fn.FinYearPK=f.FinYearFK  order by  " + selFinYrEx + " ";
                        //   selq = selq + "  select distinct (r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where   r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + "  select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],H.HeaderName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],(TotalAmount) as [Demand],(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate " + selFinYr + " from FT_FeeAllot F,Registration r,FM_HeaderMaster H,FT_FinDailyTransaction d,FM_FinYearMaster fn where h.HeaderPK =f.HeaderFK  and r.App_No=d.App_No and d.App_No=f.App_No and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " " + finlYrStr + " and fn.FinYearPK=f.FinYearFK group by f.FeeCategory,f.App_No,AllotDate,F.HeaderFK,h.HeaderName,f.FinYearFK,TotalAmount,BalAmount " + GrpselCol + " ";
                        //selq += " order by  " + selFinYrEx + "";

                        //Paid Detail query                  
                        selq = selq + " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand],sum(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + "  from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and f.FinYearFK =d.ActualFinYearFK  and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0'  " + transerText + " " + Transrcpt + "  and isnull(debit,'0')>'0' " + actfinlYrStr + "  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode,ddno,ddbankcode,d.paymode,f.FinYearFK " + GrpselCol + "";//and isnull(credit,'0')='0' and  ISNULL(IsCollected,0)='1' 
                        //selq += " order by  " + selFinYrEx + "";
                        if (!cbTrans.Checked || cbTrans.Checked)
                        {
                            selq += " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],credit as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + " from Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H,FM_FinYearMaster fn,FT_feeallot f where  H.HeaderPK =d.HeaderFK and f.FinYearFK =d.ActualFinYearFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "') " + transType + "     and isnull(IsCanceled,'0')='0'  and isnull(credit,'0')>0  " + actfinlYrStr + " and fn.FinYearPK=f.FinYearFK  group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory,f.FinYearFK,credit " + GrpselCol + "  ";//and isnull(debit,'0')>'0'  f.app_no=r.app_no and
                            //and  ISNULL(IsCollected,0)='1'
                            selq += " order by  " + selFinYrEx + "";
                        }

                        //deduction 
                        //selq += " select textval,isnull(SUM(DeductAmout),0) from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headerid + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and FinYearFK in(" + actidquery + ")  group by TextCode,textval";                   

                        selq += " select distinct textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue " + selFinYr + "  from FT_FeeAllot f,Registration r,textvaltable t,FT_FinDailyTransaction d,FM_FinYearMaster fn where f.App_No=r.App_No and d.app_no=r.app_no and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.headerfk in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " " + finlYrStr + " and fn.FinYearPK=f.FinYearFK group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK,f.FinYearFK " + GrpselCol + " ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                        //  selq += "    select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype from Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype ";

                        #endregion
                    }
                    else
                    {
                        #region header

                        //allot detail query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen " + selFinYr + " from applyn r,TextValTable t,FT_FeeAllot f,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and  r.App_No=f.App_No and fn.FinYearPK=f.FinYearFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " " + finlYrStr + " order by  " + selFinYrEx + " ";
                        //  selq = selq + "  select distinct (r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f where   r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + "  select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],H.HeaderName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],(TotalAmount) as [Demand],(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate    " + selFinYr + " from FT_FeeAllot F,applyn r,FM_HeaderMaster H,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and h.HeaderPK =f.HeaderFK  and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and fn.FinYearPK=f.FinYearFK and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " " + finlYrStr + "  group by f.FeeCategory,f.App_No,AllotDate,F.HeaderFK,h.HeaderName,f.FinYearFK,TotalAmount,BalAmount " + GrpselCol + " ";

                        //Paid Detail query                  
                        selq = selq + " union select  Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],(debit) as [Receipt],'0' [Demand],(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + " from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=d.App_No and d.App_No=f.App_No and f.FinYearFK =d.ActualFinYearFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0'  " + transerText + " " + Transrcpt + " and isnull(debit,'0')>'0' " + actfinlYrStr + "  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode,f.FinYearFK,debit,BalAmount " + GrpselCol + " ";
                        //and isnull(credit,'0')='0' and  ISNULL(IsCollected,0)='1'  
                        if (cbTrans.Checked || !cbTrans.Checked)
                        {
                            selq += " union  select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + " from applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H,FM_FinYearMaster fn where   H.HeaderPK =d.HeaderFK and f.FinYearFK =d.ActualFinYearFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "') " + transType + "" + actfinlYrStr + "  and isnull(IsCanceled,'0')='0' and isnull(credit,'0')>0 and isnull(debit,'0')='0' and fn.FinYearPK=f.FinYearFK  group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory,f.FinYearFK,credit " + GrpselCol + " ";// and  ISNULL(IsCollected,0)='1'
                            selq += " order by  " + selFinYrEx + " ";
                        }
                        selq += " select distinct textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue " + selFinYr + " from FT_FeeAllot f,applyn r,textvaltable t,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and fn.FinYearPK=f.FinYearFK and f.headerfk in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " " + finlYrStr + "  group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK,f.FinYearFK " + GrpselCol + " ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                        //  selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype from applyn r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype ";

                        #endregion
                    }
                }
                #endregion
            }
            if (ddl_hdr.SelectedIndex == 2)
            {
                #region For College

                if (checkSchoolSetting() != 0)
                {
                    if (!beforeAdm)
                    {
                        #region ledger

                        //allot details query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen from Registration r,TextValTable t,FT_FeeAllot f where r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                        //    selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.LedgerFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where  r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],l.LedgerName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.LedgerFK as CollValue,l.priority,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate from FT_FeeAllot F,Registration r,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + "  group by f.FeeCategory,f.App_No,AllotDate,F.LedgerFK,l.LedgerName,l.priority  ";
                        //order by len(isnull(l.priority,1000)) , l.priority asc
                        //Paid details query               

                        selq = selq + " union select  Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],l.LedgerName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.LedgerFK as CollValue,l.priority,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "'  and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0'   " + transerText + " " + Transrcpt + " and isnull(debit,'0')>'0' group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.Narration,l.priority,transtype,ddno,ddbankcode,d.paymode ";//and isnull(credit,'0')='0' and  ISNULL(IsCollected,0)='1'
                        if (cbTrans.Checked || !cbTrans.Checked)
                        {
                            selq += " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],h.LedgerName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.ledgerfk as CollValue,h.priority,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')  " + transType + "   and isnull(IsCanceled,'0')='0'  and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory,h.priority ";//and  ISNULL(IsCollected,0)='1'
                            selq += " order by  TDate,AllotOrPaid,collvalue";
                        }

                        //deduction
                        selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.LedgerFK in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.LedgerFK ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                        //   selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.ledgerfk as CollValue,H.ledgername as CollName,d.Narration,isnull(transtype,'0') as transtype from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype ";
                        #endregion
                    }
                    else
                    {
                        #region ledger

                        //allot details query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen from applyn r,TextValTable t,FT_FeeAllot f where r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc  ";
                        //   selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.LedgerFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f where  r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select  Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],l.LedgerName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.LedgerFK as CollValue,l.priority,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate from FT_FeeAllot F,applyn r,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + "  group by f.FeeCategory,f.App_No,AllotDate,F.LedgerFK,l.LedgerName,l.priority ";
                        //order by len(isnull(l.priority,1000)) , l.priority asc 
                        //Paid details query               

                        selq = selq + " union select  Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],l.LedgerName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.LedgerFK as CollValue,l.priority,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "'  and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' " + transerText + " " + Transrcpt + " and isnull(debit,'0')>'0' group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.Narration,l.priority,transtype,ddno,ddbankcode,d.apymode ";//and isnull(credit,'0')='0'  and  ISNULL(IsCollected,0)='1'  
                        if (cbTrans.Checked || !cbTrans.Checked)
                        {
                            selq += " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],h.LedgerName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.ledgerfk as CollValue,h.priority,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from applyn r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')  " + transType + "   and isnull(IsCanceled,'0')='0' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype ,ddno,ddbankcode,d.paymode,d.feecategory,h.priority";// and  ISNULL(IsCollected,0)='1'
                            selq += " order by  TDate,AllotOrPaid,collvalue";
                        }

                        //deduction
                        selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue  from FT_FeeAllot f,applyn r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.LedgerFK in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.LedgerFK ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";
                        //  selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.ledgerfk as CollValue,H.ledgername as CollName,d.Narration,isnull(transtype,'0') as transtype from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype ";
                        #endregion
                    }
                }
                #endregion

                #region For School Setting Added by saranya on 09/02/2018

                if (checkSchoolSetting() == 0)
                {
                    btnprint.Visible = false;
                    if (!beforeAdm)
                    {
                        #region ledger

                        //allot details query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen " + selFinYr + " from Registration r,TextValTable t,FT_FeeAllot f,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and fn.FinYearPK=f.FinYearFK and f.LedgerFK in('" + headerid + "') " + transerText + " " + finlYrStr + " order by  " + selFinYrEx + " ";
                        //    selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.LedgerFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where  r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],l.LedgerName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],(TotalAmount) as [Demand],(BalAmount) as bal,f.FeeCategory ,f.App_No,F.LedgerFK as CollValue,l.priority,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate " + selFinYr + " from FT_FeeAllot F,Registration r,FM_LedgerMaster L,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and l.LedgerPK =f.LedgerFK and r.App_No=f.App_No and f.App_No ='" + AppNo + "' and fn.FinYearPK=f.FinYearFK and  f.FinYearFK =d.ActualFinYearFK  and  f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " " + finlYrStr + " group by f.FeeCategory,f.App_No,AllotDate,F.LedgerFK,l.LedgerName,l.priority,f.FinYearFK,TotalAmount,BalAmount " + GrpselCol + " ";
                        //order by len(isnull(l.priority,1000)) , l.priority asc
                        //Paid details query               

                        selq = selq + " union select  Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],l.LedgerName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],(debit) as [Receipt],'0' [Demand],(BalAmount) as bal,f.FeeCategory ,f.App_No,F.LedgerFK as CollValue,l.priority,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + " from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and  f.FinYearFK =d.ActualFinYearFK  and  d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + AppNo + "'  and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0'   " + transerText + " " + Transrcpt + " and isnull(debit,'0')>'0'  " + actfinlYrStr + "  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.Narration,l.priority,transtype,ddno,ddbankcode,d.paymode,f.FinYearFK,debit,BalAmount " + GrpselCol + " ";//and isnull(credit,'0')='0' and  ISNULL(IsCollected,0)='1'
                        if (cbTrans.Checked || !cbTrans.Checked)
                        {
                            selq += " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],h.LedgerName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.ledgerfk as CollValue,h.priority,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + " from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H,FM_FinYearMaster fn,Ft_FeeAllot f where f.app_no=r.app_no and H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and fn.FinYearPK=f.FinYearFK and f.FinYearFK =d.ActualFinYearFK and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')  " + transType + "   and isnull(IsCanceled,'0')='0'  and isnull(credit,'0')>0 and isnull(debit,'0')='0'  " + actfinlYrStr + "   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory,h.priority,f.FinYearFK,credit " + GrpselCol + " ";//and  ISNULL(IsCollected,0)='1'
                            selq += " order by  " + selFinYrEx + "";
                        }

                        //deduction
                        selq += " select distinct textval,isnull(DeductAmout,0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue " + selFinYr + " from FT_FeeAllot f,Registration r,textvaltable t,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and f.App_No=r.App_No and fn.FinYearPK=f.FinYearFK and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.LedgerFK in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " " + finlYrStr + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.LedgerFK,f.FinYearFK,DeductAmout " + GrpselCol + " ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                        //   selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.ledgerfk as CollValue,H.ledgername as CollName,d.Narration,isnull(transtype,'0') as transtype from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype ";
                        #endregion
                    }
                    else
                    {
                        #region ledger

                        //allot details query
                        selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen " + selFinYr + " from applyn r,TextValTable t,FT_FeeAllot f,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and fn.FinYearPK=f.FinYearFK and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " " + finlYrStr + "  order by  " + selFinYrEx + " ";
                        //   selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.LedgerFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from applyn r,TextValTable t,FT_FeeAllot f where  r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                        selq = selq + " select  Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],l.LedgerName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],(TotalAmount) as [Demand],(BalAmount) as bal,f.FeeCategory ,f.App_No,F.LedgerFK as CollValue,l.priority,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate " + selFinYr + " from FT_FeeAllot F,applyn r,FM_LedgerMaster L,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and l.LedgerPK =f.LedgerFK and r.App_No=f.App_No and fn.FinYearPK=f.FinYearFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " " + finlYrStr + " group by f.FeeCategory,f.App_No,AllotDate,F.LedgerFK,l.LedgerName,l.priority,f.FinYearFK,TotalAmount,BalAmount " + GrpselCol + "  ";
                        //order by len(isnull(l.priority,1000)) , l.priority asc 
                        //Paid details query               

                        selq = selq + " union select  Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],l.LedgerName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],(debit) as [Receipt],'0' [Demand],(BalAmount) as bal,f.FeeCategory ,f.App_No,F.LedgerFK as CollValue,l.priority,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + " from FT_FeeAllot F,applyn r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and f.FinYearFK =d.ActualFinYearFK and f.App_No ='" + AppNo + "'  and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' " + transerText + " " + Transrcpt + " and isnull(debit,'0')>'0' " + actfinlYrStr + " group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.Narration,l.priority,transtype,ddno,ddbankcode,d.paymode,f.FinYearFK,BalAmount,debit " + GrpselCol + "  ";//and isnull(credit,'0')='0'  and  ISNULL(IsCollected,0)='1'  
                        if (cbTrans.Checked || !cbTrans.Checked)
                        {
                            selq += " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],h.LedgerName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.ledgerfk as CollValue,h.priority,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate " + selFinYra + " from applyn r,FT_FinDailyTransaction d,FM_LedgerMaster H,FM_FinYearMaster fn,ft_feeallot f where f.app_no=r.app_no and  H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and fn.FinYearPK=f.FinYearFK and f.FinYearFK =d.ActualFinYearFK and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')  " + transType + "   and isnull(IsCanceled,'0')='0' and isnull(credit,'0')>0 and isnull(debit,'0')='0' " + actfinlYrStr + "  group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype ,ddno,ddbankcode,d.paymode,d.feecategory,h.priority,f.FinYearFK,credit " + GrpselCol + " ";// and  ISNULL(IsCollected,0)='1'
                            selq += " order by  " + selFinYrEx + "";
                        }

                        //deduction
                        selq += " select distinct textval,isnull(DeductAmout,0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue " + selFinYr + " from FT_FeeAllot f,applyn r,textvaltable t,FT_FinDailyTransaction d,FM_FinYearMaster fn where r.App_No=d.App_No and d.App_No=f.App_No and f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + AppNo + "' and f.LedgerFK in('" + headerid + "') and fn.FinYearPK=f.FinYearFK and f.feecategory in('" + semcode + "') " + transerText + " " + finlYrStr + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.LedgerFK,f.FinYearFK,DeductAmout " + GrpselCol + "  ";
                        selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";
                        //  selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.ledgerfk as CollValue,H.ledgername as CollName,d.Narration,isnull(transtype,'0') as transtype from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype ";
                        #endregion
                    }
                }
                #endregion
            }
            string strName = string.Empty;
            string strGrpBy = string.Empty;
            if (ddl_hdr.SelectedItem.Text.Trim() == "Ledger")
            {
                strName = ",er.ledgerfk as fk,l.ledgername as name";
                strGrpBy = " ,er.ledgerfk,l.ledgername";
            }
            else
            {
                strName = ",h.headerpk as fk,h.headername as name";
                strGrpBy = " ,h.headerpk,h.headername";
            }
            // selq += " select receiptno,Convert(varchar(10),rcptdate,103) as rcptdate,er.app_no,sum(amount) as amount" + strName + ",(case when Ex_Rpt_paymode='1' then 'Cash' when Ex_Rpt_paymode='2' then 'Cheque' when Ex_Rpt_paymode='3' then 'DD' when Ex_Rpt_paymode='4' then 'Challan' when Ex_Rpt_paymode='5' then 'Online' when Ex_Rpt_paymode='6' then 'Card' end) as [Paymode],f.DDNo from ft_excessReceiptdet er,fm_ledgermaster l,fm_headermaster h,FT_FinDailyTransaction f where h.headerpk=l.headerfk and l.ledgerpk=er.ledgerfk and er.app_no='" + AppNo + "' and h.collegecode=l.collegecode and excesstype='1' and er.app_no=f.app_no and er.Ex_Rpt_paymode=f.paymode and er.receiptno=f.transcode group by receiptno,rcptdate,er.app_no,Ex_Rpt_paymode,DDNo" + strGrpBy + "";//added by abarna 20.03.2018
            // select receiptno,Convert(varchar(10),rcptdate,103) as rcptdate,er.app_no,sum(amount) as amount,er.ledgerfk as fk,l.ledgername as name,(case when Ex_Rpt_paymode='1' then 'Cash' when Ex_Rpt_paymode='2' then 'Cheque' when Ex_Rpt_paymode='3' then 'DD' when Ex_Rpt_paymode='4' then 'Challan' when Ex_Rpt_paymode='5' then 'Online' when Ex_Rpt_paymode='6' then 'Card' end) as [Paymode],f.DDNo from ft_excessReceiptdet er,fm_ledgermaster l,fm_headermaster h,FT_FinDailyTransaction f where h.headerpk=l.headerfk and l.ledgerpk=er.ledgerfk and er.app_no='22097' and h.collegecode=l.collegecode and excesstype='1' and er.app_no=f.app_no and er.Ex_Rpt_paymode=f.paymode and er.receiptno=f.transcode   group by receiptno,rcptdate,er.app_no,Ex_Rpt_paymode ,er.ledgerfk,l.ledgername,DDNo
            selq += " select receiptno,Convert(varchar(10),rcptdate,103) as rcptdate,app_no,sum(amount) as amount" + strName + ",(case when Ex_Rpt_paymode='1' then 'Cash' when Ex_Rpt_paymode='2' then 'Cheque' when Ex_Rpt_paymode='3' then 'DD' when Ex_Rpt_paymode='4' then 'Challan' when Ex_Rpt_paymode='5' then 'Online' when Ex_Rpt_paymode='6' then 'Card' end) as [Paymode] from ft_excessReceiptdet er,fm_ledgermaster l,fm_headermaster h where h.headerpk=l.headerfk and l.ledgerpk=er.ledgerfk and er.app_no='" + AppNo + "' and h.collegecode=l.collegecode and excesstype='1' group by receiptno,rcptdate,er.app_no,Ex_Rpt_paymode" + strGrpBy + "";
            dsLoad.Reset();
            dsLoad = d2.select_method_wo_parameter(selq, "Text");
            #endregion

        }
        catch { }
        return dsLoad;
    }

    protected ArrayList getColumn()
    {
        ArrayList arCol = new ArrayList();
        try
        {
            string hdName = string.Empty;
            if (ddl_hdr.SelectedIndex == 0)
                hdName = "Group Header";
            if (ddl_hdr.SelectedIndex == 1)
                hdName = "Header";
            if (ddl_hdr.SelectedIndex == 2)
                hdName = "Ledger";
            arCol.Add("Sno");
            arCol.Add("Date");
            arCol.Add("Receipt No");
            arCol.Add(hdName);
            arCol.Add("Paymode");
            arCol.Add("Cheque/DD/Card No");
            arCol.Add("Bank/Card Name");
            arCol.Add("Narration");
            arCol.Add("Receipt");
            arCol.Add("Demand");
            arCol.Add("Balance");
            arCol.Add("Deduction Amt");
            arCol.Add("Deduction Reason");
        }
        catch { }
        return arCol;
    }

    protected Hashtable getBankName(string collegecode)
    {
        Hashtable htDept = new Hashtable();
        try
        {
            string selQ = "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";
            DataSet dsDept = d2.select_method_wo_parameter(selQ, "Text");
            if (dsDept.Tables.Count > 0 && dsDept.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsDept.Tables[0].Rows.Count; row++)
                {
                    if (!htDept.ContainsKey(Convert.ToString(dsDept.Tables[0].Rows[row]["bankpk"]).Trim()))
                        htDept.Add(Convert.ToString(dsDept.Tables[0].Rows[row]["bankpk"]).Trim(), Convert.ToString(dsDept.Tables[0].Rows[row]["bankname"]));
                }
            }
        }
        catch { }
        return htDept;
    }

    protected Hashtable getCardName(string collegecode)
    {
        Hashtable htDept = new Hashtable();
        try
        {
            string selQ = "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='CardT' and college_code='" + collegecode1 + "'";
            DataSet dsDept = d2.select_method_wo_parameter(selQ, "Text");
            if (dsDept.Tables.Count > 0 && dsDept.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsDept.Tables[0].Rows.Count; row++)
                {
                    if (!htDept.ContainsKey(Convert.ToString(dsDept.Tables[0].Rows[row]["bankpk"]).Trim()))
                        htDept.Add(Convert.ToString(dsDept.Tables[0].Rows[row]["bankpk"]).Trim(), Convert.ToString(dsDept.Tables[0].Rows[row]["bankname"]));
                }
            }
        }
        catch { }
        return htDept;
    }

    protected Hashtable getDepartment(string collegecode)
    {
        Hashtable htDept = new Hashtable();
        try
        {
            string selQ = " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
            DataSet dsDept = d2.select_method_wo_parameter(selQ, "Text");
            if (dsDept.Tables.Count > 0 && dsDept.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsDept.Tables[0].Rows.Count; row++)
                {
                    if (!htDept.ContainsKey(Convert.ToString(dsDept.Tables[0].Rows[row]["Degree_Code"]).Trim()))
                        htDept.Add(Convert.ToString(dsDept.Tables[0].Rows[row]["Degree_Code"]).Trim(), Convert.ToString(dsDept.Tables[0].Rows[row]["degreename"]));
                }
            }
        }
        catch { }
        return htDept;
    }

    protected void loadFormatNew3()
    {
        try
        {
            #region student value get
            string collegecode = Convert.ToString(ddl_collegename.SelectedValue);
            string AppNo = string.Empty;
            string rollType = string.Empty;
            bool beforeAdm = false;
            List<string> rolllist = new List<string>();
            if (!cbTrans.Checked)
            {
                #region without transfer
                if (rbstudtype.SelectedItem.Value == "1")
                {
                    AppNo = getAppNo(txtno.Text.Trim(), collegecode, out rollType, ref beforeAdm);
                    if (AppNo != "0")
                        rolllist.Add(AppNo);
                }
                else
                {
                    string rollMult = Convert.ToString(lblrolldisp.Text);
                    if (rollMult != "")
                    {
                        string[] roll = rollMult.Split(',');
                        if (roll.Length > 0)
                        {
                            for (int i = 0; i < roll.Length; i++)
                            {
                                rollType = " r.Roll_No";
                                AppNo = getAppNo(Convert.ToString(roll[i]), collegecode, out rollType, ref beforeAdm);
                                if (AppNo != "0")
                                    rolllist.Add(AppNo);
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region with transfer
                rollType = " r.Roll_No";
                AppNo = getTransferAppNo(collegecode1, txtno.Text.Trim());
                if (AppNo != "0")
                {

                    rolllist.Add(AppNo);
                }
                #endregion
            }
            #endregion

            bool boolCol = false;
            bool boolCheck = false;
            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();
            string grandCreditORDeb = string.Empty;
            int rowCnt = 1;
            for (int rol = 0; rol < rolllist.Count; rol++)
            {
                DataTable dtInfo = new DataTable();
                int StudRowCnt = 0;
                AppNo = Convert.ToString(rolllist[rol]);
                if (AppNo == "0")
                    continue;
                DataSet ds = dsFormat3(AppNo, rollType, beforeAdm);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                    continue;
                dtInfo = ds.Tables[0].DefaultView.ToTable();
                dtInfo.Clear();
                DataRow drInfo = dtInfo.NewRow();
                for (int rowInfo = ds.Tables[0].Rows.Count - 1; rowInfo < ds.Tables[0].Rows.Count; rowInfo++)
                {
                    drInfo["headername"] = Convert.ToString(ds.Tables[0].Rows[rowInfo]["headername"]).Trim();
                    drInfo["TextVal"] = Convert.ToString("0").Trim();
                    drInfo["FeeCategory"] = Convert.ToString("0").Trim();
                    drInfo["degree_code"] = Convert.ToString(ds.Tables[0].Rows[rowInfo]["degree_code"]).Trim();
                    drInfo["feelen"] = Convert.ToString("0").Trim();
                    dtInfo.Rows.Add(drInfo);
                }
                ds.Tables[0].Merge(dtInfo);
                if (!boolCol)//only once bind colname
                {
                    #region design

                    ArrayList arColumn = getColumn();
                    Hashtable htCol = new Hashtable();
                    int ColValue = 0;
                    foreach (string colName in arColumn)//column header bind
                    {
                        arrColHdrNames.Add(colName);
                        dtIndividualReport.Columns.Add("col" + ColValue);
                        ColValue++;

                        switch (colName)
                        {
                            case "Group Header":
                            case "Header":
                            case "Ledger":
                            case "Deduction Reason":
                                break;
                            case "Receipt":
                            case "Demand":
                            case "Balance":
                            case "Deduction Amt":
                                break;
                        }
                    }
                    DataRow drHdr1 = dtIndividualReport.NewRow();
                    for (int grCol = 0; grCol < dtIndividualReport.Columns.Count; grCol++)
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    dtIndividualReport.Rows.Add(drHdr1);
                    boolCol = true;
                    #endregion
                }
                else
                {
                    //FpSpread1.Sheets[0].RowCount++;
                    //int rowCnt = FpSpread1.Sheets[0].RowCount - 1;
                    //FpSpread1.Sheets[0].Cells[rowCnt, 0].Text = "";
                    //FpSpread1.Sheets[0].SpanModel.Add(rowCnt, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                }

                #region value
                string selQ = "select ('Roll No: '+r.roll_no)as roll_no,r.app_no,('Name: '+stud_name) as stud_name,('Course: '+c.course_name) as course_name,('Department: '+dt.dept_name) as dept_name,('Semester: '+ case when current_semester='1' then '|' when current_semester='2' then '||' when current_semester='3' then '|||' when current_semester='4' then '|V'when current_semester='5' then 'V' when current_semester='6' then 'V|' when current_semester='7' then 'V||' when current_semester='8' then 'V|||' end) as semester from registration r,degree d,course c,department dt where r.degree_code=d.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code=r.college_code and r.app_no='" + AppNo + "'  ";
                DataSet dsStud = d2.select_method_wo_parameter(selQ, "Text");

                if (dsStud.Tables.Count > 0 && dsStud.Tables[0].Rows.Count > 0)
                {
                    drowInst = dtIndividualReport.NewRow();
                    drowInst[0] = Convert.ToString(dsStud.Tables[0].Rows[0]["roll_no"]);
                    dicColSpan.Add(rowCnt + "-" + 0, "Roll No");
                    drowInst[5] = Convert.ToString(dsStud.Tables[0].Rows[0]["stud_name"]);
                    dicColSpan.Add(rowCnt + "-" + 5, "Name");
                    dtIndividualReport.Rows.Add(drowInst);
                    drowInst = dtIndividualReport.NewRow();
                    drowInst[0] = Convert.ToString(dsStud.Tables[0].Rows[0]["course_name"]);
                    rowCnt++;
                    dicColSpan.Add(rowCnt + "-" + 0, "Course");
                    drowInst[5] = Convert.ToString(dsStud.Tables[0].Rows[0]["dept_name"]);
                    dicColSpan.Add(rowCnt + "-" + 5, "Department");
                    dtIndividualReport.Rows.Add(drowInst);
                    drowInst = dtIndividualReport.NewRow();
                    drowInst[0] = Convert.ToString(dsStud.Tables[0].Rows[0]["semester"]);
                    rowCnt++;
                    dicColSpan.Add(rowCnt + "-" + 0, "Semester");
                    dtIndividualReport.Rows.Add(drowInst);
                }
                string appNo = string.Empty;
                Hashtable htDeptName = getDepartment(collegecode);
                Hashtable htBankName = getBankName(collegecode);
                Hashtable htCardName = getCardName(collegecode);
                if (boolCol)
                {
                    int sno = 0;
                    double fnlDebitAmt = 0;
                    ArrayList arRecpt = new ArrayList();

                    for (int dsrow = 0; dsrow < ds.Tables[0].Rows.Count; dsrow++)
                    {
                        bool semCheck = false;
                        string feeCat = Convert.ToString(ds.Tables[0].Rows[dsrow]["FeeCategory"]);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "FeeCategory='" + feeCat + "'";
                            DataTable dtHeader = ds.Tables[1].DefaultView.ToTable();
                            if (dtHeader.Rows.Count > 0)
                            {
                                string totDebOrCredit = string.Empty;
                                for (int hd = 0; hd < dtHeader.Rows.Count; hd++)
                                {
                                    string strFlterval = "CollValue='" + Convert.ToString(dtHeader.Rows[hd]["CollValue"]) + "' and FeeCategory='" + feeCat + "'";

                                    if (!semCheck)
                                    {
                                        StudRowCnt++;
                                        string degreecode = Convert.ToString(ds.Tables[0].Rows[dsrow]["degree_code"]);
                                        string deptname = htDeptName.Count > 0 ? Convert.ToString(htDeptName[degreecode.Trim()]) : "";
                                        string name = Convert.ToString(ds.Tables[0].Rows[dsrow]["headername"]);
                                        drowInst = dtIndividualReport.NewRow();
                                        drowInst[0] = name.Split('-')[1] + "-" + Convert.ToString(ds.Tables[0].Rows[dsrow]["TextVal"]);
                                        rowCnt = dtIndividualReport.Rows.Count;
                                        dicColSpan.Add(rowCnt + "-" + 0, name.Split('-')[1] + "-" + Convert.ToString(ds.Tables[0].Rows[dsrow]["TextVal"]));
                                        dtIndividualReport.Rows.Add(drowInst);
                                        semCheck = true;
                                    }

                                    #region allot
                                    StudRowCnt++;
                                    // string colName = dtHeader.Columns[hd].ColumnName;
                                    string transDate = Convert.ToString(dtHeader.Rows[hd]["Date"]);
                                    string transCode = Convert.ToString(dtHeader.Rows[hd]["Receipt No"]);
                                    transCode = transCode == "0.00" || transCode == "" ? "-" : transCode;
                                    string hdName = Convert.ToString(dtHeader.Rows[hd]["Header"]);
                                    string paymode = Convert.ToString(dtHeader.Rows[hd]["Paymode"]);
                                    paymode = paymode == "" ? "-" : paymode;
                                    string bankCardNo = Convert.ToString(dtHeader.Rows[hd]["Cheque/DD/Card No"]);
                                    bankCardNo = bankCardNo == "" ? "-" : bankCardNo;
                                    string BankCardName = Convert.ToString(dtHeader.Rows[hd]["Bank/Card Name"]);
                                    BankCardName = paymode == "Cheque" || paymode == "DD" ? Convert.ToString(htBankName[BankCardName]) : Convert.ToString(htCardName[BankCardName]);
                                    BankCardName = BankCardName == "" ? "-" : BankCardName;
                                    string narratioN = Convert.ToString(dtHeader.Rows[hd]["Narration"]);
                                    narratioN = narratioN == "" ? "-" : narratioN;
                                    double paidAmt = 0;
                                    double allotAmt = 0;
                                    double balamt = 0;
                                    double.TryParse(Convert.ToString(dtHeader.Rows[hd]["Demand"]), out allotAmt);
                                    double.TryParse(Convert.ToString(dtHeader.Rows[hd]["Receipt"]), out paidAmt);
                                    string allotOrPaid = Convert.ToString(dtHeader.Rows[hd]["AllotOrPaid"]);
                                    //FpSpread1.Sheets[0].RowCount++;
                                    //rowCnt = FpSpread1.Sheets[0].RowCount - 1;
                                    sno++;
                                    drowInst = dtIndividualReport.NewRow();
                                    drowInst[0] = Convert.ToString(sno);
                                    drowInst[1] = transDate;
                                    drowInst[2] = transCode;
                                    drowInst[3] = Convert.ToString(hdName);
                                    drowInst[4] = Convert.ToString(paymode);
                                    drowInst[5] = Convert.ToString(bankCardNo);
                                    drowInst[6] = Convert.ToString(BankCardName);
                                    drowInst[7] = Convert.ToString(narratioN);
                                    drowInst[8] = Convert.ToString(paidAmt);

                                    if (!htTotal.ContainsKey(8))
                                        htTotal.Add(8, Convert.ToString(paidAmt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htTotal[8]), out amount);
                                        amount += paidAmt;
                                        htTotal.Remove(8);
                                        htTotal.Add(8, Convert.ToString(amount));
                                    }
                                    drowInst[9] = Convert.ToString(allotAmt);

                                    if (!htTotal.ContainsKey(9))
                                        htTotal.Add(9, Convert.ToString(allotAmt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htTotal[9]), out amount);
                                        amount += allotAmt;
                                        htTotal.Remove(9);
                                        htTotal.Add(9, Convert.ToString(amount));
                                    }
                                    string balAmount = string.Empty;
                                    if (allotOrPaid == "0")//debit add
                                    {
                                        balamt = allotAmt - paidAmt;
                                        fnlDebitAmt += balamt;
                                        balAmount = Convert.ToString(fnlDebitAmt) + "Dr";
                                    }
                                    else//credit  it means advance amount
                                    {
                                        //string selQAmt = " select (sum(excessamt)-sum(adjamt)) as paid from ft_excessdet ex,ft_excessledgerdet exl where ex.excessdetpk=exl.excessdetfk and app_no='" + AppNo + "' and headerfk='" + Convert.ToString(dtHeader.Rows[hd]["CollValue"]) + "' and ledgerfk='' ";
                                        balamt = paidAmt - allotAmt;
                                        fnlDebitAmt -= balamt;
                                        if (fnlDebitAmt < 0)
                                            balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Cr";
                                        else
                                            balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Dr";
                                        string transtype = Convert.ToString(dtHeader.Rows[hd]["transtype"]);
                                        if (transtype == "3")
                                            drowInst[4] = "Adj/Jl/Voucher";
                                    }
                                    drowInst[10] = Convert.ToString(balAmount);
                                    totDebOrCredit = balAmount;
                                    boolCheck = true;
                                    #endregion

                                    #region deduction
                                    if (ds.Tables[2].Rows.Count > 0 && allotOrPaid == "0")
                                    {
                                        double fnlDeductAmt = 0;
                                        StringBuilder sbDedutReas = new StringBuilder();
                                        ds.Tables[2].DefaultView.RowFilter = strFlterval;
                                        DataTable dtDeduct = ds.Tables[2].DefaultView.ToTable();
                                        if (dtDeduct.Rows.Count == 0)
                                        {
                                            dtIndividualReport.Rows.Add(drowInst);
                                            continue;
                                        }
                                        for (int dedut = 0; dedut < dtDeduct.Rows.Count; dedut++)
                                        {
                                            double deductAmt = 0;
                                            double.TryParse(Convert.ToString(dtDeduct.Rows[dedut]["DeductAmout"]), out deductAmt);
                                            fnlDeductAmt += deductAmt;
                                            sbDedutReas.Append(Convert.ToString(dtDeduct.Rows[dedut]["textval"]) + ",");
                                        }
                                        if (sbDedutReas.Length > 0)
                                            sbDedutReas.Remove(sbDedutReas.Length - 1, 1);
                                        drowInst[11] = Convert.ToString(fnlDeductAmt);
                                        if (!htTotal.ContainsKey(11))
                                            htTotal.Add(11, Convert.ToString(fnlDeductAmt));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htTotal[11]), out amount);
                                            amount += fnlDeductAmt;
                                            htTotal.Remove(11);
                                            htTotal.Add(11, Convert.ToString(amount));
                                        }
                                        //DeductTotal += FNlAmt;
                                        drowInst[12] = Convert.ToString(sbDedutReas);
                                    }
                                    else
                                    {
                                        drowInst[11] = "0";
                                        drowInst[12] = "-";
                                    }
                                    #endregion

                                    dtIndividualReport.Rows.Add(drowInst);

                                    #region excess
                                    try
                                    {
                                        if (allotOrPaid == "1" && ds.Tables[4].Rows.Count > 0)
                                        {
                                            DataTable dvex = new DataTable();
                                            try
                                            {
                                                // transDate = transDate.Split('/')[1] + "/" + transDate.Split('/')[0] + "/" + transDate.Split('/')[2];
                                                if (ddl_hdr.SelectedItem.Text.Trim() == "Ledger")
                                                    ds.Tables[4].DefaultView.RowFilter = "rcptdate='" + transDate + "' and receiptno='" + transCode + "' and paymode='" + paymode + "' ";
                                                else
                                                    ds.Tables[4].DefaultView.RowFilter = "rcptdate='" + transDate + "' and receiptno='" + transCode + "' and paymode='" + paymode + "'";
                                            }
                                            catch { }
                                            dvex = ds.Tables[4].DefaultView.ToTable();
                                            if (dvex.Rows.Count > 0 && !arRecpt.Contains(transCode))
                                            {
                                                for (int k = 0; k < dvex.Rows.Count; k++)
                                                {
                                                    #region excess Amt

                                                    drowInst = dtIndividualReport.NewRow();
                                                    sno++;
                                                    drowInst[0] = Convert.ToString(sno);
                                                    drowInst[1] = Convert.ToString(dvex.Rows[k]["rcptdate"]);
                                                    drowInst[2] = Convert.ToString(dvex.Rows[k]["receiptno"]);
                                                    //string receiptdate=Convert.ToString(dvex.Rows[k]["rcptdate"]);//27.03.2018                                                   
                                                    //string receiptno=Convert.ToString(dvex.Rows[k]["receiptno"]);//27.03.2018
                                                    string valueFK = Convert.ToString(dvex.Rows[k]["fk"]);
                                                    string Name = Convert.ToString(dvex.Rows[k]["name"]);
                                                    if (ddl_hdr.SelectedItem.Text.Trim() == "Group Header")
                                                    {
                                                        //string hedFK = d2.GetFunction("select Headerfk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and l.ledgerpk='" + ledgerfk + "'");
                                                        string stream = d2.GetFunction("select c.type from registration r,degree d,Course c where r.degree_code=d.degree_code and d.course_id=c.course_id and r.app_no ='" + Convert.ToString(dvex.Rows[k]["app_no"]) + "'");

                                                        Name = d2.GetFunction("  select ChlGroupHeader from FS_ChlGroupHeaderSettings where headerfk='" + Convert.ToString(dtHeader.Rows[hd]["CollValue"]) + "' and Stream='" + stream + "'");
                                                    }
                                                    drowInst[3] = Name + "(" + "Excess)";
                                                    drowInst[4] = Convert.ToString(dvex.Rows[k]["paymode"]);
                                                    StudRowCnt++;
                                                    // FpSpread1.Sheets[0].Cells[rowCnt, 5].Text = "-";//27.03.2018
                                                    //string ddno=d2.GetFunction ("select ddno from ft_findailytransaction where transdate='" + receiptdate + "' and transcode='" + receiptdate + "' 
                                                    drowInst[5] = Convert.ToString(bankCardNo);
                                                    drowInst[6] = Convert.ToString(BankCardName);
                                                    drowInst[7] = "-";
                                                    double tempamt = 0;
                                                    double.TryParse(Convert.ToString(dvex.Rows[k]["amount"]), out tempamt);
                                                    drowInst[8] = Convert.ToString(tempamt);
                                                    if (!htTotal.ContainsKey(8))
                                                        htTotal.Add(8, Convert.ToString(tempamt));
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(htTotal[8]), out amount);
                                                        amount += tempamt;
                                                        htTotal.Remove(8);
                                                        htTotal.Add(8, Convert.ToString(amount));
                                                    }
                                                    drowInst[9] = Convert.ToString(allotAmt);
                                                    //if (!htTotal.ContainsKey(9))
                                                    //    htTotal.Add(9, Convert.ToString(0));
                                                    //else
                                                    //{
                                                    //    double amount = 0;
                                                    //    double.TryParse(Convert.ToString(htTotal[9]), out amount);
                                                    //    amount += allotAmt;
                                                    //    htTotal.Remove(9);
                                                    //    htTotal.Add(9, Convert.ToString(amount));
                                                    //}
                                                    //string balAmount = string.Empty;
                                                    if (allotOrPaid == "0")//debit add
                                                    {
                                                        balamt = allotAmt - paidAmt;
                                                        fnlDebitAmt += balamt;
                                                        balAmount = Convert.ToString(fnlDebitAmt) + "Dr";
                                                    }
                                                    else//credit  it means advance amount
                                                    {
                                                        //string selQAmt = " select (sum(excessamt)-sum(adjamt)) as paid from ft_excessdet ex,ft_excessledgerdet exl where ex.excessdetpk=exl.excessdetfk and app_no='" + AppNo + "' and headerfk='" + Convert.ToString(dtHeader.Rows[hd]["CollValue"]) + "' and ledgerfk='' ";
                                                        balamt = tempamt;
                                                        fnlDebitAmt -= balamt;
                                                        if (fnlDebitAmt < 0)
                                                            balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Cr";
                                                        else
                                                            balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Dr";
                                                    }
                                                    drowInst[10] = balAmount;
                                                    totDebOrCredit = balAmount;
                                                    #endregion
                                                    dtIndividualReport.Rows.Add(drowInst);
                                                }
                                                arRecpt.Add(transCode);
                                            }
                                        }
                                    }
                                    catch { }
                                    #endregion

                                }
                                //total
                                if (htTotal.Count > 0)
                                {
                                    #region total
                                    drowInst = dtIndividualReport.NewRow();
                                    drowInst[0] = "Total";
                                    rowCnt = dtIndividualReport.Rows.Count;
                                    dicColSpan.Add(rowCnt + "-" + 0, "Total");
                                    StudRowCnt++;
                                    double grandvalues = 0;
                                    for (int j = 8; j < dtIndividualReport.Columns.Count; j++)
                                    {
                                        if (j == 10)
                                        {
                                            drowInst[10] = totDebOrCredit;
                                            grandCreditORDeb = totDebOrCredit;
                                            continue;
                                        }
                                        if (j == 12)
                                        {
                                            drowInst[12] = "";
                                            continue;
                                        }
                                        double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                                        drowInst[j] = Convert.ToString(grandvalues);

                                        if (!htGrandTotal.ContainsKey(j))
                                            htGrandTotal.Add(j, Convert.ToString(grandvalues));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htGrandTotal[j]), out amount);
                                            amount += grandvalues;
                                            htGrandTotal.Remove(j);
                                            htGrandTotal.Add(j, Convert.ToString(amount));
                                        }
                                    }
                                    dtIndividualReport.Rows.Add(drowInst);
                                    htTotal.Clear();
                                    #endregion
                                }
                            }
                        }
                    }
                    if (htGrandTotal.Count > 0)
                    {
                        #region total
                        drowInst = dtIndividualReport.NewRow();
                        drowInst[0] = "Grand Total";
                        rowCnt = dtIndividualReport.Rows.Count;
                        dicColSpan.Add(rowCnt + "-" + 0, "Grand Total");
                        StudRowCnt++;
                        double grandvalues = 0;
                        for (int j = 8; j < dtIndividualReport.Columns.Count; j++)
                        {
                            if (j == 10)
                            {
                                drowInst[10] = grandCreditORDeb;
                                continue;
                            }
                            if (j == 12)
                            {
                                drowInst[12] = "";
                                continue;
                            }
                            double.TryParse(Convert.ToString(htGrandTotal[j]), out grandvalues);
                            drowInst[j] = Convert.ToString(grandvalues);
                        }
                        dtIndividualReport.Rows.Add(drowInst);
                        rowCnt++;
                        drowInst = dtIndividualReport.NewRow();
                        rowCnt = dtIndividualReport.Rows.Count;
                        dicColSpan.Add(rowCnt + "-" + 0, "Empty");
                        dtIndividualReport.Rows.Add(drowInst);
                        rowCnt++;
                        #endregion
                    }
                    //int row
                }
                #endregion

                StudwiseRowCnt.Add(AppNo, StudRowCnt);
                Session["htStudwiseRowCnt"] = StudwiseRowCnt;
            }
            if (boolCheck)
            {
                grdIndividualReport.DataSource = dtIndividualReport;
                grdIndividualReport.DataBind();
                grdIndividualReport.Visible = true;

                grdIndividualReport.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grdIndividualReport.Rows[0].Font.Bold = true;
                grdIndividualReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                foreach (KeyValuePair<string, string> dr in dicColSpan)
                {
                    string g = dr.Key;
                    string[] rowC = g.Split('-');
                    int RowFinCnt = Convert.ToInt32(rowC[0]);
                    string DicValue = dr.Value;
                    if (DicValue == "Roll No" || DicValue == "Course")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 5;
                        for (int a = 1; a < 5; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (DicValue == "Semester")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 13;
                        for (int a = 1; a < 13; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (DicValue == "Name" || DicValue == "Department")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[5].ColumnSpan = 8;
                        for (int a = 6; a < 13; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[5].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (DicValue.Contains('-'))
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 13;
                        for (int a = 1; a < 13; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[RowFinCnt].BackColor = ColorTranslator.FromHtml("#cc66ff");
                    }
                    if (DicValue == "Total")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[RowFinCnt].BackColor = Color.YellowGreen;
                    }
                    if (DicValue == "Grand Total")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[RowFinCnt].BackColor = Color.Green;
                    }
                    if (DicValue == "Empty")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = dtIndividualReport.Columns.Count;
                        for (int a = 1; a < dtIndividualReport.Columns.Count; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                    }
                }


                // div1.Visible = true;
                Error.Visible = false;
                rprint.Visible = true;
                lblsmserror.Visible = false;
                txtexcel.Text = "";
            }
            else
            {
                rprint.Visible = false;
                grdIndividualReport.Visible = false;
                //  div1.Visible = false;
                Error.Visible = true;
                Error.Text = "Please Enter the Roll/Reg No!";
            }
        }
        catch { }
    }

    protected void grdIndividualReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (studstaffid.SelectedIndex == 0)
        {
            if (ddlViewFormat.SelectedIndex == 0)
            {
                if (!cbpaymode.Checked)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        if (e.Row.RowIndex == 0)
                        {
                            if (cbRefund.Checked == false)
                                e.Row.Cells[7].Visible = false;
                            if (inclnarr.Checked == false)
                                e.Row.Cells[8].Visible = false;
                            if (cbincdedut.Checked == false)
                            {
                                e.Row.Cells[9].Visible = false;
                                e.Row.Cells[10].Visible = false;
                            }
                        }
                        else
                        {
                            e.Row.Font.Size = FontUnit.Medium;
                            e.Row.Font.Bold = true;
                            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                            if (cbRefund.Checked)
                                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                            else
                                e.Row.Cells[7].Visible = false;
                            //narration
                            if (inclnarr.Checked == true)
                                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                            else
                                e.Row.Cells[8].Visible = false;
                            //deduction
                            if (cbincdedut.Checked == true)
                            {
                                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                                e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Left;
                            }
                            else
                            {
                                e.Row.Cells[9].Visible = false;
                                e.Row.Cells[10].Visible = false;
                            }
                        }
                    }
                }
                else
                {
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    foreach (KeyValuePair<int, string> drVisible in dicColvisible)
                    {
                        int g = drVisible.Key;
                        string DicValue = drVisible.Value;
                        if (DicValue == "Visible")
                        {
                            e.Row.Cells[g].Visible = false;
                        }
                    }
                    if (e.Row.RowIndex != 0)
                    {
                        foreach (KeyValuePair<int, string> drVisible in dicColAlignment)
                        {
                            int g = drVisible.Key;
                            string DicValue = drVisible.Value;
                            if (DicValue == "ColumnAlign")
                            {
                                e.Row.Cells[g].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                    }
                }
            }
            else if (ddlViewFormat.SelectedIndex == 1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowIndex == 0)
                    {
                        e.Row.BackColor = Color.FromArgb(12, 166, 202);
                        e.Row.HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Font.Bold = true;
                    }
                    else
                    {
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                    }
                    if (e.Row.RowIndex == 1)
                    {
                        e.Row.BackColor = Color.FromArgb(12, 166, 202);
                        e.Row.HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Font.Bold = true;
                        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            else if (ddlViewFormat.SelectedIndex == 2)
            {
                if (checkSchoolSetting() != 0)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        if (e.Row.RowIndex == 0)
                        {
                        }
                        else
                        {
                            e.Row.Font.Size = FontUnit.Medium;
                            e.Row.Font.Bold = true;
                            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                            e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                            e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Left;
                        }
                    }
                }
                if (checkSchoolSetting() == 0)
                {
                }
            }
        }
        else//staff
        {

        }

    }

    protected string getAppNo(string rollNo, string collegecode, out string rollType, ref bool beforeAdm)
    {
        string AppNo = string.Empty;
        rollType = string.Empty;
        try
        {
            string selQ = string.Empty;
            if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 0)
            {
                selQ = "  select App_No from Registration where Roll_No='" + rollNo + "' and college_code='" + collegecode + "' ";
                rollType = "r.Roll_No";
            }
            else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 1)
            {
                selQ = "select App_No from Registration where Reg_No='" + rollNo + "' and college_code='" + collegecode + "'";
                rollType = "r.Reg_No";
            }
            else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 2)
            {
                selQ = "select App_No from Registration where Roll_admit='" + rollNo + "' and college_code='" + collegecode + "'";
                rollType = "r.Roll_admit";
            }
            else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 3)
            {
                selQ = "select App_No from applyn where app_formno='" + rollNo + "' and college_code='" + collegecode + "'";
                rollType = "r.app_formno";
                beforeAdm = true;
            }

            else if (Convert.ToUInt32(ddladmit.SelectedItem.Value) == 4)
            {
                selQ = "select App_No from Registration where Roll_no='" + rollNo.Split('-')[1] + "' and college_code='" + collegecode + "'";
                rollType = "r.Roll_no";
            }
            if (!string.IsNullOrEmpty(selQ))
                AppNo = d2.GetFunction(selQ);
            if (AppNo == "0")
            {
                selQ = "  select App_No from Registration where Roll_No='" + rollNo + "' and college_code='" + collegecode + "' ";
                AppNo = d2.GetFunction(selQ);
                rollType = "r.Roll_No";
            }
            if (AppNo == "0")
            {
                selQ = "  select App_No from Registration where Reg_No='" + rollNo + "' and college_code='" + collegecode + "' ";
                AppNo = d2.GetFunction(selQ);
                rollType = "r.Reg_No";
            }
            if (AppNo == "0")
            {
                selQ = "  select App_No from Registration where Roll_admit='" + rollNo + "' and college_code='" + collegecode + "' ";
                AppNo = d2.GetFunction(selQ);
                rollType = "r.Roll_admit";
            }
            if (AppNo == "0")
            {
                selQ = "  select App_No from applyn where app_formno='" + rollNo + "' and college_code='" + collegecode + "'";
                AppNo = d2.GetFunction(selQ);
                rollType = "r.app_formno";
                beforeAdm = true;
            }
            if (AppNo == "0" && rollNo.Contains("-"))
            {
                selQ = "select App_No from Registration where Roll_no='" + rollNo.Split('-')[1] + "' and college_code='" + collegecode + "'";
                AppNo = d2.GetFunction(selQ);
                rollType = "r.Roll_no";
            }
        }
        catch { AppNo = "0"; }
        return AppNo;
    }

    #region Added by saranya on 09/02/2018 For School Setting

    protected Dictionary<string, string> getFinancialYear()
    {
        Dictionary<string, string> htfinlYR = new Dictionary<string, string>();
        try
        {
            collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string selQFK = string.Empty;
            selQFK = "  select distinct (convert(nvarchar(15),FinYearStart,103)+'-'+convert(nvarchar(15),FinYearEnd,103)+'-'+convert(varchar(10),collegecode)) as finyear,finyearpk as pk from FM_FinYearMaster where CollegeCode in('" + collegecode + "') ";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!htfinlYR.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        htfinlYR.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["finyear"]));
                }
            }
        }
        catch { htfinlYR.Clear(); }
        return htfinlYR;
    }

    protected ArrayList getSelFinlDate()
    {
        ArrayList arDate = new ArrayList();
        try
        {
            for (int fnl = 0; fnl < chklsfyear.Items.Count; fnl++)
            {
                if (!chklsfyear.Items[fnl].Selected)
                    continue;
                string date = Convert.ToString(chklsfyear.Items[fnl].Text);
                string fnlYr = date.Split('-')[0].Split('/')[2] + "-" + date.Split('-')[1].Split('/')[2];
                if (!arDate.Contains(fnlYr))
                    arDate.Add(fnlYr);
            }
        }
        catch { }
        return arDate;
    }

    protected void loadFormatNewSchool()
    {

        try
        {
            #region student value get
            string collegecode = Convert.ToString(ddl_collegename.SelectedValue);
            string AppNo = string.Empty;
            string rollType = string.Empty;
            bool beforeAdm = false;
            List<string> rolllist = new List<string>();
            if (!cbTrans.Checked)
            {
                #region without transfer
                if (rbstudtype.SelectedItem.Value == "1")
                {
                    AppNo = getAppNo(txtno.Text.Trim(), collegecode, out rollType, ref beforeAdm);
                    if (AppNo != "0")
                        rolllist.Add(AppNo);
                }
                else
                {
                    string rollMult = Convert.ToString(lblrolldisp.Text);
                    if (rollMult != "")
                    {
                        string[] roll = rollMult.Split(',');
                        if (roll.Length > 0)
                        {
                            for (int i = 0; i < roll.Length; i++)
                            {
                                rollType = " r.Roll_No";
                                AppNo = getAppNo(Convert.ToString(roll[i]), collegecode, out rollType, ref beforeAdm);
                                if (AppNo != "0")
                                    rolllist.Add(AppNo);
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region with transfer
                rollType = " r.Roll_No";
                AppNo = getTransferAppNo(collegecode1, txtno.Text.Trim());
                if (AppNo != "0")
                {

                    rolllist.Add(AppNo);
                }
                #endregion
            }
            #endregion

            bool boolCol = false;
            bool boolCheck = false;
            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();
            string grandCreditORDeb = string.Empty;
            for (int rol = 0; rol < rolllist.Count; rol++)
            {
                DataTable dtInfo = new DataTable();

                AppNo = Convert.ToString(rolllist[rol]);
                if (AppNo == "0")
                    continue;
                DataSet ds = dsFormat3(AppNo, rollType, beforeAdm);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                    continue;
                dtInfo = ds.Tables[0].DefaultView.ToTable();
                dtInfo.Clear();
                DataRow drInfo = dtInfo.NewRow();
                for (int rowInfo = ds.Tables[0].Rows.Count - 1; rowInfo < ds.Tables[0].Rows.Count; rowInfo++)
                {
                    drInfo["headername"] = Convert.ToString(ds.Tables[0].Rows[rowInfo]["headername"]).Trim();
                    drInfo["TextVal"] = Convert.ToString("0").Trim();
                    drInfo["FeeCategory"] = Convert.ToString("0").Trim();
                    drInfo["degree_code"] = Convert.ToString(ds.Tables[0].Rows[rowInfo]["degree_code"]).Trim();
                    drInfo["feelen"] = Convert.ToString("0").Trim();
                    dtInfo.Rows.Add(drInfo);
                }
                ds.Tables[0].Merge(dtInfo);
                if (!boolCol)//only once bind colname
                {
                    #region design
                    ArrayList arColumn = getColumn();
                    Hashtable htCol = new Hashtable();
                    int ColValue = 0;
                    foreach (string colName in arColumn)//column header bind
                    {
                        arrColHdrNames.Add(colName);
                        dtIndividualReport.Columns.Add("col" + ColValue);
                        ColValue++;

                        switch (colName)
                        {
                            case "Group Header":
                            case "Header":
                            case "Ledger":
                            case "Deduction Reason":
                                break;
                            case "Receipt":
                            case "Demand":
                            case "Balance":
                            case "Deduction Amt":
                                break;
                        }
                    }
                    DataRow drHdr1 = dtIndividualReport.NewRow();
                    for (int grCol = 0; grCol < dtIndividualReport.Columns.Count; grCol++)
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    dtIndividualReport.Rows.Add(drHdr1);
                    boolCol = true;

                    #endregion
                }
                else
                {
                    //FpSpread1.Sheets[0].RowCount++;
                    //int rowCnt = FpSpread1.Sheets[0].RowCount - 1;
                    //FpSpread1.Sheets[0].Cells[rowCnt, 0].Text = "";
                    //FpSpread1.Sheets[0].SpanModel.Add(rowCnt, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                }

                #region value
                string selQ = "select ('Roll No: '+r.roll_no)as roll_no,r.app_no,('Name: '+stud_name) as stud_name,('Course: '+c.course_name) as course_name,('Department: '+dt.dept_name) as dept_name,('Semester: '+ case when current_semester='1' then '|' when current_semester='2' then '||' when current_semester='3' then '|||' when current_semester='4' then '|V'when current_semester='5' then 'V' when current_semester='6' then 'V|' when current_semester='7' then 'V||' when current_semester='8' then 'V|||' end) as semester from registration r,degree d,course c,department dt where r.degree_code=d.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code=r.college_code and r.app_no='" + AppNo + "'  ";
                DataSet dsStud = d2.select_method_wo_parameter(selQ, "Text");
                int rowCnt = 1;
                if (dsStud.Tables.Count > 0 && dsStud.Tables[0].Rows.Count > 0)
                {
                    drowInst = dtIndividualReport.NewRow();
                    drowInst[0] = Convert.ToString(dsStud.Tables[0].Rows[0]["roll_no"]);
                    dicColSpan.Add(rowCnt + "-" + 0, "Roll No");
                    drowInst[5] = Convert.ToString(dsStud.Tables[0].Rows[0]["stud_name"]);
                    dicColSpan.Add(rowCnt + "-" + 5, "Name");
                    dtIndividualReport.Rows.Add(drowInst);
                    drowInst = dtIndividualReport.NewRow();
                    drowInst[0] = Convert.ToString(dsStud.Tables[0].Rows[0]["course_name"]);
                    rowCnt++;
                    dicColSpan.Add(rowCnt + "-" + 0, "Course");
                    drowInst[5] = Convert.ToString(dsStud.Tables[0].Rows[0]["dept_name"]);
                    dicColSpan.Add(rowCnt + "-" + 5, "Department");
                    dtIndividualReport.Rows.Add(drowInst);
                    drowInst = dtIndividualReport.NewRow();
                    drowInst[0] = Convert.ToString(dsStud.Tables[0].Rows[0]["semester"]);
                    rowCnt++;
                    dicColSpan.Add(rowCnt + "-" + 0, "Semester");
                    dtIndividualReport.Rows.Add(drowInst);
                }
                string appNo = string.Empty;
                Hashtable htDeptName = getDepartment(collegecode);
                Hashtable htBankName = getBankName(collegecode);
                Hashtable htCardName = getCardName(collegecode);
                ArrayList arFnlYear = getSelFinlDate();
                if (boolCol)
                {
                    int sno = 0;
                    double fnlDebitAmt = 0;
                    string feeCat = string.Empty;
                    ArrayList arRecpt = new ArrayList();
                    foreach (string fnlYear in arFnlYear)
                    {
                        //double fnlDebitAmt = 0;
                        ds.Tables[0].DefaultView.RowFilter = "actualfinyearfk='" + fnlYear + "'";
                        DataTable finYear = ds.Tables[0].DefaultView.ToTable();

                        for (int dsrow = 0; dsrow < finYear.Rows.Count; dsrow++)
                        {
                            bool semCheck = false;
                            feeCat = Convert.ToString(ds.Tables[0].Rows[dsrow]["FeeCategory"]);
                            //finYr = Convert.ToString(ds.Tables[0].Rows[dsrow]["actualfinyearfk"]);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ds.Tables[1].DefaultView.RowFilter = "FeeCategory='" + feeCat + "' and actualfinyearfk='" + fnlYear + "' ";
                                DataTable dtHeader = ds.Tables[1].DefaultView.ToTable();
                                if (dtHeader.Rows.Count > 0)
                                {
                                    string totDebOrCredit = string.Empty;
                                    for (int hd = 0; hd < dtHeader.Rows.Count; hd++)
                                    {
                                        string strFlterval = "CollValue='" + Convert.ToString(dtHeader.Rows[hd]["CollValue"]) + "' and FeeCategory='" + feeCat + "'";
                                        if (!semCheck)
                                        {
                                            drowInst = dtIndividualReport.NewRow();

                                            string degreecode = Convert.ToString(ds.Tables[0].Rows[dsrow]["degree_code"]);
                                            string deptname = htDeptName.Count > 0 ? Convert.ToString(htDeptName[degreecode.Trim()]) : "";
                                            string name = Convert.ToString(ds.Tables[0].Rows[dsrow]["headername"]);

                                            //  FpSpread1.Sheets[0].Cells[rowCnt, 0].Text = name.Split('-')[1] + "-" + deptname + "-" + Convert.ToString(ds.Tables[0].Rows[dsrow]["TextVal"]);
                                            drowInst[0] = name.Split('-')[1] + "-" + Convert.ToString(ds.Tables[0].Rows[dsrow]["TextVal"] + "-" + fnlYear);
                                            rowCnt = dtIndividualReport.Rows.Count;
                                            dicColSpan.Add(rowCnt + "-" + 0, name.Split('-')[1] + "-" + Convert.ToString(ds.Tables[0].Rows[dsrow]["TextVal"]));
                                            dtIndividualReport.Rows.Add(drowInst);
                                            semCheck = true;
                                        }
                                        #region allot
                                        // string colName = dtHeader.Columns[hd].ColumnName;
                                        string transDate = Convert.ToString(dtHeader.Rows[hd]["Date"]);
                                        string transCode = Convert.ToString(dtHeader.Rows[hd]["Receipt No"]);
                                        transCode = transCode == "0.00" || transCode == "" ? "-" : transCode;
                                        string hdName = Convert.ToString(dtHeader.Rows[hd]["Header"]);
                                        string paymode = Convert.ToString(dtHeader.Rows[hd]["Paymode"]);
                                        paymode = paymode == "" ? "-" : paymode;
                                        string bankCardNo = Convert.ToString(dtHeader.Rows[hd]["Cheque/DD/Card No"]);
                                        bankCardNo = bankCardNo == "" ? "-" : bankCardNo;
                                        string BankCardName = Convert.ToString(dtHeader.Rows[hd]["Bank/Card Name"]);
                                        BankCardName = paymode == "Cheque" || paymode == "DD" ? Convert.ToString(htBankName[BankCardName]) : Convert.ToString(htCardName[BankCardName]);
                                        BankCardName = BankCardName == "" ? "-" : BankCardName;
                                        string narratioN = Convert.ToString(dtHeader.Rows[hd]["Narration"]);
                                        narratioN = narratioN == "" ? "-" : narratioN;
                                        double paidAmt = 0;
                                        double allotAmt = 0;
                                        double balamt = 0;
                                        double.TryParse(Convert.ToString(dtHeader.Rows[hd]["Demand"]), out allotAmt);
                                        double.TryParse(Convert.ToString(dtHeader.Rows[hd]["Receipt"]), out paidAmt);
                                        string allotOrPaid = Convert.ToString(dtHeader.Rows[hd]["AllotOrPaid"]);
                                        drowInst = dtIndividualReport.NewRow();
                                        sno++;
                                        drowInst[0] = Convert.ToString(sno);
                                        drowInst[1] = transDate;
                                        drowInst[2] = transCode;
                                        drowInst[3] = Convert.ToString(hdName);
                                        drowInst[4] = Convert.ToString(paymode);
                                        drowInst[5] = Convert.ToString(bankCardNo);
                                        drowInst[6] = Convert.ToString(BankCardName);
                                        drowInst[7] = Convert.ToString(narratioN);
                                        drowInst[8] = Convert.ToString(paidAmt);
                                        if (!htTotal.ContainsKey(8))
                                            htTotal.Add(8, Convert.ToString(paidAmt));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htTotal[8]), out amount);
                                            amount += paidAmt;
                                            htTotal.Remove(8);
                                            htTotal.Add(8, Convert.ToString(amount));
                                        }
                                        drowInst[9] = Convert.ToString(allotAmt);

                                        if (!htTotal.ContainsKey(9))
                                            htTotal.Add(9, Convert.ToString(allotAmt));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htTotal[9]), out amount);
                                            amount += allotAmt;
                                            htTotal.Remove(9);
                                            htTotal.Add(9, Convert.ToString(amount));
                                        }
                                        string balAmount = string.Empty;
                                        if (allotOrPaid == "0")//debit add
                                        {
                                            balamt = allotAmt - paidAmt;
                                            fnlDebitAmt += balamt;
                                            balAmount = Convert.ToString(fnlDebitAmt).Trim('-') + "Dr";
                                        }
                                        else//credit  it means advance amount
                                        {
                                            //string selQAmt = " select (sum(excessamt)-sum(adjamt)) as paid from ft_excessdet ex,ft_excessledgerdet exl where ex.excessdetpk=exl.excessdetfk and app_no='" + AppNo + "' and headerfk='" + Convert.ToString(dtHeader.Rows[hd]["CollValue"]) + "' and ledgerfk='' ";
                                            balamt = paidAmt - allotAmt;
                                            fnlDebitAmt -= balamt;
                                            if (fnlDebitAmt < 0)
                                                balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Cr";
                                            else
                                                balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Dr";
                                            string transtype = Convert.ToString(dtHeader.Rows[hd]["transtype"]);
                                            if (transtype == "3")
                                                drowInst[4] = "Adj/Jl/Voucher";
                                        }
                                        drowInst[10] = balAmount;
                                        totDebOrCredit = balAmount;
                                        boolCheck = true;
                                        #endregion

                                        #region deduction
                                        if (ds.Tables[2].Rows.Count > 0 && allotOrPaid == "0")
                                        {
                                            double fnlDeductAmt = 0;
                                            StringBuilder sbDedutReas = new StringBuilder();
                                            ds.Tables[2].DefaultView.RowFilter = strFlterval;
                                            DataTable dtDeduct = ds.Tables[2].DefaultView.ToTable();
                                            if (dtDeduct.Rows.Count == 0)
                                                continue;
                                            for (int dedut = 0; dedut < dtDeduct.Rows.Count; dedut++)
                                            {
                                                double deductAmt = 0;
                                                double.TryParse(Convert.ToString(dtDeduct.Rows[dedut]["DeductAmout"]), out deductAmt);
                                                fnlDeductAmt += deductAmt;
                                                sbDedutReas.Append(Convert.ToString(dtDeduct.Rows[dedut]["textval"]) + ",");
                                            }
                                            if (sbDedutReas.Length > 0)
                                                sbDedutReas.Remove(sbDedutReas.Length - 1, 1);
                                            drowInst[11] = Convert.ToString(fnlDeductAmt);
                                            if (!htTotal.ContainsKey(11))
                                                htTotal.Add(11, Convert.ToString(fnlDeductAmt));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htTotal[11]), out amount);
                                                amount += fnlDeductAmt;
                                                htTotal.Remove(11);
                                                htTotal.Add(11, Convert.ToString(amount));
                                            }
                                            //DeductTotal += FNlAmt;
                                            drowInst[12] = Convert.ToString(sbDedutReas);
                                        }
                                        else
                                        {
                                            drowInst[11] = "0";
                                            drowInst[12] = "-";
                                        }
                                        #endregion

                                        #region excess
                                        try
                                        {
                                            if (allotOrPaid == "1" && ds.Tables[4].Rows.Count > 0)
                                            {
                                                DataTable dvex = new DataTable();
                                                try
                                                {
                                                    // transDate = transDate.Split('/')[1] + "/" + transDate.Split('/')[0] + "/" + transDate.Split('/')[2];
                                                    if (ddl_hdr.SelectedItem.Text.Trim() == "Ledger")
                                                        ds.Tables[4].DefaultView.RowFilter = "rcptdate='" + transDate + "' and receiptno='" + transCode + "' and paymode='" + paymode + "' ";
                                                    else
                                                        ds.Tables[4].DefaultView.RowFilter = "rcptdate='" + transDate + "' and receiptno='" + transCode + "' and paymode='" + paymode + "'";
                                                }
                                                catch { }
                                                dvex = ds.Tables[4].DefaultView.ToTable();
                                                if (dvex.Rows.Count > 0 && !arRecpt.Contains(transCode))
                                                {
                                                    for (int k = 0; k < dvex.Rows.Count; k++)
                                                    {
                                                        #region excess Amt

                                                        drowInst = dtIndividualReport.NewRow();
                                                        sno++;
                                                        drowInst[0] = Convert.ToString(sno);
                                                        drowInst[1] = Convert.ToString(dvex.Rows[k]["rcptdate"]);
                                                        drowInst[2] = Convert.ToString(dvex.Rows[k]["receiptno"]);
                                                        string valueFK = Convert.ToString(dvex.Rows[k]["fk"]);
                                                        string Name = Convert.ToString(dvex.Rows[k]["name"]);
                                                        if (ddl_hdr.SelectedItem.Text.Trim() == "Group Header")
                                                        {
                                                            //string hedFK = d2.GetFunction("select Headerfk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and l.ledgerpk='" + ledgerfk + "'");
                                                            string stream = d2.GetFunction("select c.type from registration r,degree d,Course c where r.degree_code=d.degree_code and d.course_id=c.course_id and r.app_no ='" + Convert.ToString(dvex.Rows[k]["app_no"]) + "'");

                                                            Name = d2.GetFunction("  select ChlGroupHeader from FS_ChlGroupHeaderSettings where headerfk='" + Convert.ToString(dtHeader.Rows[hd]["CollValue"]) + "' and Stream='" + stream + "'");
                                                        }
                                                        drowInst[3] = Name + "(" + "Excess)";
                                                        drowInst[4] = Convert.ToString(dvex.Rows[k]["paymode"]);
                                                        drowInst[5] = "-";
                                                        drowInst[6] = Convert.ToString(BankCardName);
                                                        drowInst[7] = "-";
                                                        double tempamt = 0;
                                                        double.TryParse(Convert.ToString(dvex.Rows[k]["amount"]), out tempamt);
                                                        drowInst[8] = Convert.ToString(tempamt);
                                                        if (!htTotal.ContainsKey(8))
                                                            htTotal.Add(8, Convert.ToString(tempamt));
                                                        else
                                                        {
                                                            double amount = 0;
                                                            double.TryParse(Convert.ToString(htTotal[8]), out amount);
                                                            amount += tempamt;
                                                            htTotal.Remove(8);
                                                            htTotal.Add(8, Convert.ToString(amount));
                                                        }
                                                        drowInst[9] = Convert.ToString(allotAmt);

                                                        //if (!htTotal.ContainsKey(9))
                                                        //    htTotal.Add(9, Convert.ToString(0));
                                                        //else
                                                        //{
                                                        //    double amount = 0;
                                                        //    double.TryParse(Convert.ToString(htTotal[9]), out amount);
                                                        //    amount += allotAmt;
                                                        //    htTotal.Remove(9);
                                                        //    htTotal.Add(9, Convert.ToString(amount));
                                                        //}
                                                        //string balAmount = string.Empty;
                                                        if (allotOrPaid == "0")//debit add
                                                        {
                                                            balamt = allotAmt - paidAmt;
                                                            fnlDebitAmt += balamt;
                                                            balAmount = Convert.ToString(fnlDebitAmt) + "Dr";
                                                        }
                                                        else//credit  it means advance amount
                                                        {
                                                            //string selQAmt = " select (sum(excessamt)-sum(adjamt)) as paid from ft_excessdet ex,ft_excessledgerdet exl where ex.excessdetpk=exl.excessdetfk and app_no='" + AppNo + "' and headerfk='" + Convert.ToString(dtHeader.Rows[hd]["CollValue"]) + "' and ledgerfk='' ";
                                                            balamt = tempamt;
                                                            fnlDebitAmt -= balamt;
                                                            if (fnlDebitAmt < 0)
                                                                balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Cr";
                                                            else
                                                                balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Dr";
                                                        }
                                                        drowInst[10] = balAmount;
                                                        totDebOrCredit = balAmount;
                                                        #endregion
                                                    }
                                                    arRecpt.Add(transCode);
                                                }
                                            }
                                        }
                                        catch { }
                                        #endregion

                                        dtIndividualReport.Rows.Add(drowInst);
                                    }
                                    //total
                                    if (htTotal.Count > 0)
                                    {
                                        #region total
                                        drowInst = dtIndividualReport.NewRow();
                                        drowInst[0] = "Total";
                                        rowCnt = dtIndividualReport.Rows.Count;
                                        dicColSpan.Add(rowCnt + "-" + 0, "Total");
                                        double grandvalues = 0;
                                        for (int j = 8; j < dtIndividualReport.Columns.Count; j++)
                                        {
                                            if (j == 10)
                                            {
                                                drowInst[10] = totDebOrCredit;
                                                grandCreditORDeb = totDebOrCredit;
                                                continue;
                                            }
                                            double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                                            drowInst[j] = Convert.ToString(grandvalues);

                                            if (!htGrandTotal.ContainsKey(j))
                                                htGrandTotal.Add(j, Convert.ToString(grandvalues));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htGrandTotal[j]), out amount);
                                                amount += grandvalues;
                                                htGrandTotal.Remove(j);
                                                htGrandTotal.Add(j, Convert.ToString(amount));
                                            }
                                        }
                                        dtIndividualReport.Rows.Add(drowInst);
                                        htTotal.Clear();
                                        #endregion
                                    }
                                }
                            }
                        }
                        if (htGrandTotal.Count > 0)
                        {
                            #region total
                            drowInst = dtIndividualReport.NewRow();
                            drowInst[0] = "Grand Total";
                            rowCnt = dtIndividualReport.Rows.Count;
                            dicColSpan.Add(rowCnt + "-" + 0, "Grand Total");
                            double grandvalues = 0;
                            for (int j = 8; j < dtIndividualReport.Columns.Count; j++)
                            {
                                if (j == 10)
                                {
                                    drowInst[10] = grandCreditORDeb;
                                    continue;
                                }
                                double.TryParse(Convert.ToString(htGrandTotal[j]), out grandvalues);
                                drowInst[j] = Convert.ToString(grandvalues);
                            }
                            dtIndividualReport.Rows.Add(drowInst);
                            #endregion
                        }

                        htTotal.Clear();
                        htGrandTotal.Clear();
                    }
                }
                #endregion
            }
            if (boolCheck)
            {
                grdIndividualReport.DataSource = dtIndividualReport;
                grdIndividualReport.DataBind();
                grdIndividualReport.Visible = true;

                grdIndividualReport.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grdIndividualReport.Rows[0].Font.Bold = true;
                grdIndividualReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                foreach (KeyValuePair<string, string> dr in dicColSpan)
                {
                    string g = dr.Key;
                    string[] rowC = g.Split('-');
                    int RowFinCnt = Convert.ToInt32(rowC[0]);
                    string DicValue = dr.Value;
                    if (DicValue == "Roll No" || DicValue == "Course")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 5;
                        for (int a = 1; a < 5; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (DicValue == "Semester")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 13;
                        for (int a = 1; a < 13; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (DicValue == "Name" || DicValue == "Department")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[5].ColumnSpan = 8;
                        for (int a = 6; a < 13; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[5].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (DicValue.Contains('-'))
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 13;
                        for (int a = 1; a < 13; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[RowFinCnt].BackColor = ColorTranslator.FromHtml("#cc66ff");
                    }
                    if (DicValue == "Total")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[RowFinCnt].BackColor = Color.YellowGreen;
                    }
                    if (DicValue == "Grand Total")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[RowFinCnt].BackColor = Color.Green;
                    }
                }
                // div1.Visible = true;
                Error.Visible = false;
                rprint.Visible = true;
                lblsmserror.Visible = false;
                txtexcel.Text = "";
            }
            else
            {
                rprint.Visible = false;
                grdIndividualReport.Visible = false;
                //  div1.Visible = false;
                Error.Visible = true;
                Error.Text = "Please Enter the Roll/Reg No!";
            }
        }
        catch { }
    }

    #endregion

    //added by saranya 09/11/2017  staff receipt report  format3

    #region Staff

    protected void rbstudstaffid_Selected(object sender, EventArgs e)
    {
        if (studstaffid.SelectedItem.Value == "1")
        {
            grdIndividualReport.Visible = true;
            LoadFromSettings();
            //txtno_TextChanged(sender,e);
            //ddladmit.Items.Clear();
            //lblnum.Text = "";
            ddladmit_SelectedIndexChanged(sender, e);
            ddladmit.Visible = true;
            personmode = 0;
            lblnum.Text = "Roll No";
            lblcate.Visible = true;
            upsem.Visible = true;
            inclnarr.Visible = true;
            cbincdedut.Visible = true;
            cbpaymode.Visible = true;
            cbTrans.Visible = true;
            cbRefund.Visible = true;
            Lbltype.Visible = true;
            ddltype.Visible = true;
            ddlViewFormat.Visible = true;
            fieldset1.Visible = true;
            rprint.Visible = false;
            lbltype1.Visible = true;
        }
        else
        {
            grdIndividualReport.Visible = false;
            lbltype1.Visible = false;
            ddlViewFormat.Items[0].Enabled = false;
            ddlViewFormat.Items[1].Enabled = false;
            fieldset1.Visible = false;
            lblcate.Visible = false;
            upsem.Visible = false;
            rprint.Visible = false;
            //Lbltype.Visible = false;
            //ddltype.Visible = false;
            lblnum.Text = "Staff Code ";
            ddladmit.Items.Clear();
            ddladmit.Items.Insert(0, "Staff Code");
            txtno.Text = "";
            lblnum.Text = ddladmit.SelectedItem.ToString();
            txtno.Attributes.Add("placeholder", "Staff Code");
            personmode = 1;
        }

    }

    protected void btn_staffOK_Click(object sender, EventArgs e)
    {
        try
        {
            string rollno = "";
            string activerow = "";
            string activecol = "";
            string rollval = "";
            int cnT = 0;
            if (rbstudtype.SelectedItem.Value == "1")
            {
                foreach (GridViewRow gvrow in GrdStaff.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchks");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                        rollno = Convert.ToString(GrdStaff.Rows[RowCnt].Cells[3].Text);
                        txtno.Text = Convert.ToString(rollno);
                        div_staffLook.Visible = false;
                    }
                }
            }
            else
            {
                lblrolldisp.Text = "";
                lbldisp.Text = "";
                foreach (GridViewRow gvrow in GrdStaff.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchks");
                    if (chk.Checked == true)
                    {
                        int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                        cnT++;
                        if (rollval == "")
                            rollval = Convert.ToString(GrdStaff.Rows[RowCnt].Cells[3].Text);
                        else
                            rollval = rollval + "," + Convert.ToString(GrdStaff.Rows[RowCnt].Cells[3].Text);
                    }
                }
                lblrolldisp.Text = rollval;
                lbldisp.Text = Convert.ToString("You Have Selected " + cnT + " Staff");
                lbldisp.Visible = true;
                div_staffLook.Visible = false;
            }


            //string actrow = "";
            //string actcol = "";
            //actrow = spreadStaff.ActiveSheetView.ActiveRow.ToString();
            //actcol = spreadStaff.ActiveSheetView.ActiveColumn.ToString();
            //if (actrow.Trim() != "" && actrow.Trim() != "-1")
            //{
            //    string staff = Convert.ToString(spreadStaff.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Text);
            //    string appno = Convert.ToString(spreadStaff.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Tag);
            //    string staffcode = Convert.ToString(spreadStaff.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
            //    txtroll_staff.Text = staffcode;
            //    txtroll_staff_Changed(sender, e);

            //}
            // div_staffLook.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanReceipt"); }
    }

    protected void btn_go2Staff_Click(object sender, EventArgs e)
    {
        GrdStaff.Visible = true;
        try
        {
            string clgcode = string.Empty;
            clgcode = getClgCode();
            div_staffLook.Visible = true;
            if (collegecode1 != null)
            {
                string selq = "";
                if (txtsearch1.Text.Trim() != "")
                {
                    string sname = string.Empty;
                    try
                    {
                        sname = txtsearch1.Text.Trim().Split('-')[0];
                    }
                    catch { sname = txtsearch1.Text.Trim(); }
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + clgcode + "') and staff_name like '" + Convert.ToString(sname) + "%'";
                }
                else if (txtsearch1c.Text.Trim() != "")
                {
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + clgcode + "') and staff_code='" + Convert.ToString(txtsearch1c.Text) + "'";
                }
                else
                {
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + clgcode + "') order by PrintPriority";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtStaffReport = new DataTable();
                    DataRow drowInst;
                    ArrayList arrColHdrNames = new ArrayList();
                    arrColHdrNames.Add("S.No");
                    arrColHdrNames.Add("Staff Code");
                    arrColHdrNames.Add("ApplId");
                    arrColHdrNames.Add("Staff Name");
                    dtStaffReport.Columns.Add("Sno");
                    dtStaffReport.Columns.Add("Staff Code");
                    dtStaffReport.Columns.Add("ApplId");
                    dtStaffReport.Columns.Add("Staff Name");
                    DataRow drHdr1 = dtStaffReport.NewRow();
                    for (int grCol = 0; grCol < dtStaffReport.Columns.Count; grCol++)
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    dtStaffReport.Rows.Add(drHdr1);
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        drowInst = dtStaffReport.NewRow();
                        drowInst["Sno"] = Convert.ToString(row + 1);
                        drowInst["Staff Code"] = Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]);
                        drowInst["ApplId"] = Convert.ToString(ds.Tables[0].Rows[row]["appl_id"]);
                        drowInst["Staff Name"] = Convert.ToString(ds.Tables[0].Rows[row]["staff_name"]);
                        dtStaffReport.Rows.Add(drowInst);
                    }
                    if (rbstudtype.SelectedItem.Value == "1")
                    {
                        ChkSelectGridStaff.Visible = false;
                    }
                    else
                        ChkSelectGridStaff.Visible = true;
                    GrdStaff.DataSource = dtStaffReport;
                    GrdStaff.DataBind();
                    GrdStaff.Visible = true;

                    // div2.Visible = true;
                    lbl_errormsgstaff.Visible = false;
                    GrdStaff.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    GrdStaff.Rows[0].Font.Bold = true;
                    GrdStaff.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                    if (GrdStaff.Rows.Count > 0)
                    {
                        btn_staffOK.Visible = true;
                        btn_exitstaff.Visible = true;
                    }
                    else
                    {
                        btn_staffOK.Visible = false;
                        btn_exitstaff.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanReceipt"); }
    }

    protected void GrdStaff_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[4].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[4].Visible = false;
            if (e.Row.RowIndex == 0)
            {
                e.Row.Cells[1].Text = "Select";
            }
        }
    }

    protected string getApplNo(string rollno)
    {
        string ApplNo = string.Empty;
        try
        {
            ApplNo = d2.GetFunction("select sa.appl_id  from staffmaster s,staff_appl_master sa where s.appl_no =sa.appl_no and s.staff_code ='" + rollno + "'");
        }
        catch { }
        return ApplNo;
    }

    protected void btn_exitstaff_Click(object sender, EventArgs e)
    {
        div_staffLook.Visible = false;
        GrdStudent.Visible = false;

    }

    protected DataSet dsFormatStaff(string ApplNo)
    {
        DataSet dsLoad = new DataSet();
        try
        {
            string rollType = string.Empty;
            string college = string.Empty;
            string rptType = string.Empty;
            string headerid = string.Empty;
            string semcode = string.Empty;
            string transerText = string.Empty;
            // string AppNo = string.Empty;
            // string rollType = string.Empty;
            //  bool beforeAdm = false;
            if (ddl_collegename.Items.Count > 0)
                college = Convert.ToString(ddl_collegename.SelectedValue);
            if (ddl_hdr.SelectedIndex == 0)
                rptType = "Group Header";
            else if (ddl_hdr.SelectedIndex == 1)
                rptType = "Header";
            else if (ddl_hdr.SelectedIndex == 2)
                rptType = "Ledger";
            if (cbl_header.Items.Count > 0)
                headerid = Convert.ToString(getCblSelectedValue(cbl_header));


            #region Query
            string Transrcpt = string.Empty;
            string transType = string.Empty;
            string selq = "";
            string AppNo = string.Empty;
            string type = string.Empty;
            if (ddl_hdr.SelectedIndex == 0)
            {
                #region group header
                ////allot detail
                //selq = "  select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  r.App_No=f.App_No and ch.HeaderFK=f.HeaderFK and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "'" + transerText + " order by len(t.TextVal),t.TextVal  asc";
                ////  selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  f.HeaderFK=ch.HeaderFK and r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and ch.ChlGroupHeader in('" + headerid + "') and ch.Stream='" + type + "' " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                //selq = selq + " select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],G.ChlGroupHeader as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r where  r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' " + transerText + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK";
                ////paid detail                   
                //selq = selq + "union select  Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand], SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + ApplNo + "' and f.FeeCategory in('" + semcode + "') and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "'  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')>'0'  " + transerText + " " + Transrcpt + " group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode";//and isnull(credit,'0')='0'
                //if (cbTrans.Checked || !cbTrans.Checked)
                //{
                //    selq = selq + " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,d.Narration,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate  from FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + ApplNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0'  and isnull(credit,'0')>'0' " + transType + "    group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory";
                //    selq += " order by  TDate,AllotOrPaid,collvalue";
                //}
                //selq += "  select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t,FS_ChlGroupHeaderSettings G where f.App_No=r.App_No and f.headerfk=g.headerfk and f.DeductReason=t.TextCode and f.app_no='" + ApplNo + "' and g.ChlGroupHeader in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),f.HeaderFK ";
                //selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                ////paid detail                   
                //// selq = selq + " select SUM(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,G.ChlGroupHeader as CollName,d.Narration,isnull(transtype,'0') as transtype  from FS_ChlGroupHeaderSettings G,Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and g.headerfk=d.headerfk  and d.App_No ='" + AppNo + "'  and g.ChlGroupHeader in('" + headerid + "') and Stream ='" + type + "'  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0' and isnull(credit,'0')>'0' " + transerText + " group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype";

                #endregion

                //

                selq += "select distinct (sm.staff_code+'-'+sm.staff_name) as headername, sa.dept_code as degree_code from staffmaster sm,staff_appl_master sa,FT_FeeAllot f,FS_ChlGroupHeaderSettings ch where  sa.appl_id=f.App_No and ch.HeaderFK=f.HeaderFK and f.App_No='" + ApplNo + "' and ch.ChlGroupHeader in('" + headerid + "') ";

                selq += "  select distinct Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],G.ChlGroupHeader as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,staffmaster sm,staff_appl_master sa  where  sm.appl_no=sa.appl_no  and  f.HeaderFK =G.HeaderFK and f.App_No ='" + ApplNo + "'  and g.ChlGroupHeader in('" + headerid + "') and f.MemType='2'   group by G.ChlGroupHeader,f.FeeCategory,f.App_No,AllotDate,f.HeaderFK";

                selq += "  union select  Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand], SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,f.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate  from FT_FeeAllot F,FS_ChlGroupHeaderSettings G,staffmaster sm,staff_appl_master sa,FT_FinDailyTransaction d where d.App_No =sa.appl_id and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and sa.appl_id=f.App_No and  f.HeaderFK =G.HeaderFK and f.App_No ='" + ApplNo + "'  and g.ChlGroupHeader in('" + headerid + "')and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')>'0'    group by G.ChlGroupHeader,f.FeeCategory,f.App_No,TransDate,TransCode,f.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode";

                selq += "  union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],G.ChlGroupHeader as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate  from FS_ChlGroupHeaderSettings G,staffmaster sm,staff_appl_master sa,FT_FinDailyTransaction d where d.App_No =sa.appl_id and g.headerfk=d.headerfk and sa.appl_no=sm.appl_no and d.App_No ='" + ApplNo + "'  and g.ChlGroupHeader in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(debit,'0')='0'  and isnull(credit,'0')>'0' and d.MemType='2'  group by G.ChlGroupHeader,d.App_No,TransDate,TransCode,d.HeaderFK,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory order by  TDate,AllotOrPaid,collvalue";

                selq += "  select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,f.HeaderFK as CollValue  from FT_FeeAllot f,staffmaster sm,staff_appl_master sa,textvaltable t,FS_ChlGroupHeaderSettings G where f.App_No=sa.appl_id and f.headerfk=g.headerfk and f.DeductReason=t.TextCode and f.app_no='" + ApplNo + "' and g.ChlGroupHeader in('" + headerid + "')  group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),f.HeaderFK";

                selq += "  select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='13'";

            }
            if (ddl_hdr.SelectedIndex == 1)
            {
                #region header

                ////allot detail query
                //selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen from Registration r,TextValTable t,FT_FeeAllot f where    r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + ApplNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                ////   selq = selq + "  select distinct (r.Stud_Name+' - '+t.TextVal) as headername,f.HeaderFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where   r.App_No=f.App_No and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                //selq = selq + "  select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],H.HeaderName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate from FT_FeeAllot F,Registration r,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK  and r.App_No=f.App_No and f.App_No ='" + ApplNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') " + transerText + " group by f.FeeCategory,f.App_No,AllotDate,F.HeaderFK,h.HeaderName";
                ////Paid Detail query                  
                //selq = selq + " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + ApplNo + "' and f.FeeCategory in('" + semcode + "') and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  " + transerText + " " + Transrcpt + "  and isnull(debit,'0')>'0'  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.HeaderFK,h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode,ddno,ddbankcode,d.paymode ";//and isnull(credit,'0')='0'
                //if (!cbTrans.Checked || cbTrans.Checked)
                //{
                //    selq += " union  select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + ApplNo + "' and d.headerfk in('" + headerid + "') " + transType + "     and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')>'0'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory ";
                //    selq += " order by  TDate,AllotOrPaid,collvalue";
                //}

                ////deduction 
                ////selq += " select textval,isnull(SUM(DeductAmout),0) from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headerid + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and FinYearFK in(" + actidquery + ")  group by TextCode,textval";                   

                //selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + ApplNo + "' and f.headerfk in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK ";
                //selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                //  selq += "    select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.HeaderFK as CollValue,H.HeaderName as CollName,d.Narration,isnull(transtype,'0') as transtype from Registration r,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.headerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype ";

                #endregion
                selq = " select distinct (sm.staff_code+'-'+sm.staff_name) as headername,sa.dept_code as degree_code from staffmaster sm,staff_appl_master sa,FT_FeeAllot f where sm.appl_no=sa.appl_no and sa.appl_id=f.App_No  and f.HeaderFK in('" + headerid + "') and f.App_No='" + ApplNo + "' ";

                selq += "   select distinct Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],H.HeaderName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate from FT_FeeAllot F,staffmaster sm,staff_appl_master sa,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK  and sm.appl_no=sa.appl_no and sa.appl_id=f.App_No  and f.App_No ='" + ApplNo + "'     and f.HeaderFK in('" + headerid + "') and f.memtype='2' group by f.App_No,AllotDate,F.HeaderFK,h.HeaderName,f.FeeCategory ";

                selq += " union  select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from FT_FeeAllot F,staffmaster sm,staff_appl_master sa,FT_FinDailyTransaction d,FM_HeaderMaster H where h.HeaderPK =f.HeaderFK and H.HeaderPK =d.HeaderFK and sa.appl_no=sm.appl_no and d.App_No =sa.appl_id and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and sa.appl_id=f.App_No  and f.App_No ='" + ApplNo + "' and f.HeaderFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'   and istransfer='0'   and isnull(debit,'0')>'0' and f.memtype='2'  group by h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode,ddno,ddbankcode,d.paymode,Transdate ,Transcode,f.FeeCategory ,f.App_No,f.HeaderFK ";

                selq += " union  select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],H.HeaderName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.HeaderFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from staffmaster sm,staff_appl_master sa,FT_FinDailyTransaction d,FM_HeaderMaster H where   H.HeaderPK =d.HeaderFK  and sa.appl_no=sm.appl_no and d.App_No =sa.appl_id and d.App_No ='" + ApplNo + "' and d.headerfk in('" + headerid + "')   and isnull(paid_Istransfer,'0')='0'  and isnull(receipttype,'0')<>'0'     and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')>'0' and d.memtype='2'   group by d.App_No,TransDate,TransCode,d.HeaderFK,h.HeaderName,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory  order by  TDate,AllotOrPaid,collvalue ";

                selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.HeaderFK as CollValue  from FT_FeeAllot f,staffmaster sm,staff_appl_master sa,textvaltable t where  sa.appl_no=sm.appl_no and f.App_No=sa.appl_id and f.DeductReason=t.TextCode and f.app_no='" + ApplNo + "' and f.headerfk in('" + headerid + "')  and f.memtype='2'  group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.HeaderFK    ";

                selq += "  select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='13'";
            }
            if (ddl_hdr.SelectedIndex == 2)
            {
                #region ledger

                ////allot details query
                //selq = " select distinct (" + rollType + "+'-'+r.Stud_Name) as headername,t.TextVal,f.FeeCategory,r.degree_code,len(t.TextVal) as feelen from Registration r,TextValTable t,FT_FeeAllot f where r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + ApplNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc ";
                ////    selq = selq + " select distinct (" + rollType + "+'-'+r.Stud_Name+' - '+t.TextVal) as headername,f.LedgerFK as CollValue,f.FeeCategory,len(t.TextVal),t.TextVal from Registration r,TextValTable t,FT_FeeAllot f where  r.App_No=f.App_No and f.FeeCategory=t.TextCode and f.FeeCategory=t.TextCode and r.App_No='" + AppNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + " order by len(t.TextVal),t.TextVal  asc";
                //selq = selq + " select Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],l.LedgerName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.LedgerFK as CollValue,l.priority,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate from FT_FeeAllot F,Registration r,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and r.App_No=f.App_No and f.App_No ='" + ApplNo + "' and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') " + transerText + "  group by f.FeeCategory,f.App_No,AllotDate,F.LedgerFK,l.LedgerName,l.priority  ";
                ////order by len(isnull(l.priority,1000)) , l.priority asc
                ////Paid details query               

                //selq = selq + " union select  Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],l.LedgerName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.LedgerFK as CollValue,l.priority,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from FT_FeeAllot F,Registration r,FT_FinDailyTransaction d,FM_LedgerMaster L where l.LedgerPK =f.LedgerFK and l.LedgerPK =d.LedgerFK and d.App_No =r.App_No and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.HeaderFK =d.HeaderFK and f.LedgerFK =d.LedgerFK and r.App_No=f.App_No  and f.App_No ='" + ApplNo + "'  and f.FeeCategory in('" + semcode + "') and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  " + transerText + " " + Transrcpt + " and isnull(debit,'0')>'0' group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.Narration,l.priority,transtype,ddno,ddbankcode,d.paymode ";//and isnull(credit,'0')='0'
                //if (cbTrans.Checked || !cbTrans.Checked)
                //{
                //    selq += " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],h.LedgerName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.ledgerfk as CollValue,h.priority,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + ApplNo + "' and d.ledgerfk in('" + headerid + "')  " + transType + "   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory,h.priority ";
                //    selq += " order by  TDate,AllotOrPaid,collvalue";
                //}

                ////deduction
                //selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue  from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and f.app_no='" + ApplNo + "' and f.LedgerFK in('" + headerid + "') and f.feecategory in('" + semcode + "') " + transerText + " group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.LedgerFK ";
                //selq += "   select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='" + collegecode1 + "'";

                ////   selq += "  select sum(credit) as paid,d.App_No,Convert(varchar(10),TransDate,103) as TransDate,TransCode,d.ledgerfk as CollValue,H.ledgername as CollName,d.Narration,isnull(transtype,'0') as transtype from Registration r,FT_FinDailyTransaction d,FM_LedgerMaster H where   H.HeaderfK =d.HeaderFK and h.ledgerpk=d.ledgerfk and d.App_No =r.App_No and r.App_No ='" + AppNo + "' and d.ledgerfk in('" + headerid + "')   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')='0'   group by d.App_No,TransDate,TransCode,d.ledgerfk,h.ledgername,d.Narration,transtype ";//
                #endregion

                selq = " select distinct (sm.staff_code+'-'+sm.staff_name) as headername,sa.dept_code as degree_code from staffmaster sm,staff_appl_master sa,FT_FeeAllot f where sm.appl_no=sa.appl_no and sa.appl_id=f.App_No  and f.LedgerFK in('" + headerid + "') and f.App_No='" + ApplNo + "' ";

                selq += " select distinct Convert(varchar(10),AllotDate,103) as [Date],''[Receipt No],L.LedgerName as [Header],''[Paymode],''[Cheque/DD/Card No],''[Bank/Card Name],'' [Narration],'0'[Receipt],SUM(TotalAmount) as [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,f.LedgerFK as CollValue,'' as transtype ,'0' as AllotOrPaid ,AllotDate as TDate from FT_FeeAllot F,staffmaster sm,staff_appl_master sa,FM_LedgerMaster L where L.LedgerPK =f.LedgerFK  and sm.appl_no=sa.appl_no and sa.appl_id=f.App_No  and f.App_No ='" + ApplNo + "' and f.LedgerFK in('" + headerid + "') and f.memtype='2' group by f.App_No,AllotDate,F.LedgerFK,L.LedgerName,f.FeeCategory";

                selq += " union  select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],L.LedgerName as [Header],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],sum(debit) as [Receipt],'0' [Demand],SUM(BalAmount) as bal,f.FeeCategory ,f.App_No,F.LedgerFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from FT_FeeAllot F,staffmaster sm,staff_appl_master sa,FT_FinDailyTransaction d,FM_LedgerMaster L where L.LedgerPK =f.LedgerFK and L.LedgerPK =d.LedgerFK and sa.appl_no=sm.appl_no and d.App_No =sa.appl_id and d.App_No =f.App_No and d.FeeCategory =f.FeeCategory and f.LedgerFK =d.LedgerFK and f.headerfk =d.headerfk and sa.appl_id=f.App_No  and d.App_No ='" + ApplNo + "'  and f.LedgerFK in('" + headerid + "') and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'      and isnull(debit,'0')>'0' and f.memtype='2'  group by f.FeeCategory,f.App_No,TransDate,TransCode,F.LedgerFK,l.LedgerName,d.Narration,transtype,ddno,ddbankcode,d.paymode,ddno,ddbankcode,d.paymode";

                selq += " union select Convert(varchar(10),Transdate,103) as [Date],Transcode as [Receipt No],l.LedgerName as [Ledger],(case when d.paymode='1' then 'Cash' when d.paymode='2' then 'Cheque' when d.paymode='3' then 'DD' when d.paymode='4' then 'Challan' when d.paymode='5' then 'Online' when d.paymode='6' then 'Card' end) as [Paymode],ddno as [Cheque/DD/Card No],ddbankcode as [Bank/Card Name],Narration as  [Narration],'0' [Receipt],sum(credit) as [Demand],'0'as bal,d.FeeCategory,d.App_No,d.LedgerFK as CollValue,isnull(transtype,'0') as transtype,'1' as AllotOrPaid,TransDate as TDate from staffmaster sm,staff_appl_master sa,FT_FinDailyTransaction d,FM_LedgerMaster L where   L.LedgerPK =d.LedgerFK and d.App_No =sa.appl_id and d.App_No ='" + ApplNo + "' and d.ledgerfk in('" + headerid + "')   and isnull(receipttype,'0')<>'3' and isnull(receipttype,'0')<>'0'     and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(credit,'0')>0 and isnull(debit,'0')>'0' and d.memtype='2'  group by d.App_No,TransDate,TransCode,d.LedgerFK,l.LedgerName,d.Narration,transtype,ddno,ddbankcode,d.paymode,d.feecategory  order by  TDate,AllotOrPaid,collvalue";

                selq += " select textval,isnull(SUM(DeductAmout),0) as DeductAmout,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103) as TransDate,F.LedgerFK as CollValue  from FT_FeeAllot f,staffmaster sm,staff_appl_master sa,textvaltable t where sa.appl_no=sm.appl_no and f.App_No=sa.appl_id and f.DeductReason=t.TextCode and f.app_no='" + ApplNo + "' and f.LedgerFK in('" + headerid + "') and f.memtype='2' group by Textcode,Textval,f.FeeCategory ,f.App_No,Convert(varchar(10),AllotDate,103),F.LedgerFK ";

                selq += " select textval as bankname,textcode as bankpk from textvaltable where textcriteria='BName' and college_code='13' ";


            }
            string strName = string.Empty;
            string strGrpBy = string.Empty;
            //if (ddl_hdr.SelectedItem.Text.Trim() == "Ledger")
            //{
            //    strName = ",er.ledgerfk as fk,l.ledgername as name";
            //    strGrpBy = " ,er.ledgerfk,l.ledgername";
            //}
            //else
            //{
            //    strName = ",h.headerpk as fk,h.headername as name";
            //    strGrpBy = " ,h.headerpk,h.headername";
            //}
            //selq += " select receiptno,Convert(varchar(10),rcptdate,103) as rcptdate,app_no,sum(amount) as amount" + strName + ",(case when Ex_Rpt_paymode='1' then 'Cash' when Ex_Rpt_paymode='2' then 'Cheque' when Ex_Rpt_paymode='3' then 'DD' when Ex_Rpt_paymode='4' then 'Challan' when Ex_Rpt_paymode='5' then 'Online' when Ex_Rpt_paymode='6' then 'Card' end) as [Paymode] from ft_excessReceiptdet er,fm_ledgermaster l,fm_headermaster h where h.headerpk=l.headerfk and l.ledgerpk=er.ledgerfk and er.app_no='" + ApplNo + "' and h.collegecode=l.collegecode and excesstype='1' group by receiptno,rcptdate,er.app_no,Ex_Rpt_paymode" + strGrpBy + "";
            dsLoad.Reset();
            dsLoad = d2.select_method_wo_parameter(selq, "Text");
            #endregion



        }
        catch { }
        return dsLoad;
    }

    protected void NewStaffFormat()
    {

        try
        {
            #region staff value get
            string collegecode = Convert.ToString(ddl_collegename.SelectedValue);
            string ApplNo = string.Empty;
            string rolltype = string.Empty;
            List<string> rolllist = new List<string>();

            if (rbstudtype.SelectedIndex == 0)
            {
                ApplNo = getApplNo(txtno.Text.Trim());
                if (ApplNo != "0")
                    rolllist.Add(ApplNo);
            }
            else
            {
                string rollMult = Convert.ToString(lblrolldisp.Text);
                if (rollMult != "")
                {
                    string[] roll = rollMult.Split(',');
                    if (roll.Length > 0)
                    {
                        for (int i = 0; i < roll.Length; i++)
                        {
                            //rollType = " r.Roll_No";
                            ApplNo = getApplNo(Convert.ToString(roll[i]));
                            if (ApplNo != "0")
                                rolllist.Add(ApplNo);
                        }
                    }
                }
            }


            #endregion

            bool boolCol = false;
            bool boolCheck = false;
            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();
            string grandCreditORDeb = string.Empty;
            for (int rol = 0; rol < rolllist.Count; rol++)
            {
                DataTable dtInfo = new DataTable();

                ApplNo = Convert.ToString(rolllist[rol]);
                if (ApplNo == "0")
                    continue;
                DataSet ds = dsFormatStaff(ApplNo);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                    continue;
                dtInfo = ds.Tables[0].DefaultView.ToTable();
                dtInfo.Clear();
                DataRow drInfo = dtInfo.NewRow();
                //for (int rowInfo = ds.Tables[0].Rows.Count - 1; rowInfo < ds.Tables[0].Rows.Count; rowInfo++)
                //{
                //    drInfo["headername"] = Convert.ToString(ds.Tables[0].Rows[rowInfo]["headername"]).Trim();
                //    //drInfo["TextVal"] = Convert.ToString("0").Trim();
                //    //drInfo["FeeCategory"] = Convert.ToString("0").Trim();
                //    drInfo["degree_code"] = Convert.ToString(ds.Tables[0].Rows[rowInfo]["degree_code"]).Trim();
                //    // drInfo["feelen"] = Convert.ToString("0").Trim();
                //    dtInfo.Rows.Add(drInfo);
                //}
                //ds.Tables[0].Merge(dtInfo);
                if (!boolCol)//only once bind colname
                {
                    #region design

                    ArrayList arColumn = getColumn();
                    Hashtable htCol = new Hashtable();
                    int ColValue = 0;
                    foreach (string colName in arColumn)//column header bind
                    {
                        arrColHdrNames.Add(colName);
                        dtIndividualReport.Columns.Add("col" + ColValue);
                        ColValue++;

                        switch (colName)
                        {
                            case "Group Header":
                            case "Header":
                            case "Ledger":
                            case "Deduction Reason":
                                break;
                            case "Receipt":
                            case "Demand":
                            case "Balance":
                            case "Deduction Amt":
                                break;
                        }
                    }
                    DataRow drHdr1 = dtIndividualReport.NewRow();
                    for (int grCol = 0; grCol < dtIndividualReport.Columns.Count; grCol++)
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    dtIndividualReport.Rows.Add(drHdr1);
                    boolCol = true;

                    #endregion
                }
                else
                {
                    //FpSpread1.Sheets[0].RowCount++;
                    //int rowCnt = FpSpread1.Sheets[0].RowCount - 1;
                    //FpSpread1.Sheets[0].Cells[rowCnt, 0].Text = "";
                    //FpSpread1.Sheets[0].SpanModel.Add(rowCnt, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                }
                #region value
                string selQ = "select ('Staff Code: '+sm.staff_code)as roll_no,sa.appl_id,('Name: '+sm.staff_name) as stud_name,''as course_name,('Department: '+hr.dept_name) as dept_name,''semester from staffmaster sm,staff_appl_master sa,hrdept_master hr where sm.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and sa.appl_id='" + ApplNo + "'  ";
                DataSet dsStud = d2.select_method_wo_parameter(selQ, "Text");
                int rowCnt = 1;
                if (dsStud.Tables.Count > 0 && dsStud.Tables[0].Rows.Count > 0)
                {
                    drowInst = dtIndividualReport.NewRow();
                    drowInst[0] = Convert.ToString(dsStud.Tables[0].Rows[0]["roll_no"]);
                    dicColSpan.Add(rowCnt + "-" + 0, "Roll No");
                    drowInst[5] = Convert.ToString(dsStud.Tables[0].Rows[0]["stud_name"]);
                    dicColSpan.Add(rowCnt + "-" + 5, "Name");
                    dtIndividualReport.Rows.Add(drowInst);
                    drowInst = dtIndividualReport.NewRow();
                    drowInst[0] = Convert.ToString(dsStud.Tables[0].Rows[0]["course_name"]);
                    rowCnt++;
                    dicColSpan.Add(rowCnt + "-" + 0, "Course");
                    drowInst[5] = Convert.ToString(dsStud.Tables[0].Rows[0]["dept_name"]);
                    dicColSpan.Add(rowCnt + "-" + 5, "Department");
                    dtIndividualReport.Rows.Add(drowInst);
                    drowInst = dtIndividualReport.NewRow();
                    drowInst[0] = Convert.ToString(dsStud.Tables[0].Rows[0]["semester"]);
                    rowCnt++;
                    dicColSpan.Add(rowCnt + "-" + 0, "Semester");
                    dtIndividualReport.Rows.Add(drowInst);
                }
                string appNo = string.Empty;
                Hashtable htDeptName = getStaffDepartment(collegecode);
                Hashtable htBankName = getBankName(collegecode);
                Hashtable htCardName = getCardName(collegecode);
                if (boolCol)
                {
                    int sno = 0;
                    double fnlDebitAmt = 0;
                    //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    //{
                    //}
                    ArrayList arRecpt = new ArrayList();
                    for (int dsrow = 0; dsrow < ds.Tables[0].Rows.Count; dsrow++)
                    {
                        bool semCheck = false;
                        // string feeCat = Convert.ToString(ds.Tables[0].Rows[dsrow]["FeeCategory"]);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //  ds.Tables[1].DefaultView.RowFilter = "FeeCategory='" + feeCat + "'";
                            DataTable dtHeader = ds.Tables[1].DefaultView.ToTable();
                            if (dtHeader.Rows.Count > 0)
                            {
                                string totDebOrCredit = string.Empty;
                                for (int hd = 0; hd < dtHeader.Rows.Count; hd++)
                                {
                                    string strFlterval = "CollValue='" + Convert.ToString(dtHeader.Rows[hd]["CollValue"]) + "' ";
                                    if (!semCheck)
                                    {
                                        string degreecode = Convert.ToString(ds.Tables[0].Rows[dsrow]["degree_code"]);
                                        string deptname = htDeptName.Count > 0 ? Convert.ToString(htDeptName[degreecode.Trim()]) : "";
                                        string name = Convert.ToString(ds.Tables[0].Rows[dsrow]["headername"]);
                                        //  FpSpread1.Sheets[0].Cells[rowCnt, 0].Text = name.Split('-')[1] + "-" + deptname + "-" + Convert.ToString(ds.Tables[0].Rows[dsrow]["TextVal"]);
                                        drowInst = dtIndividualReport.NewRow();
                                        drowInst[0] = name.Split('-')[1];
                                        rowCnt = dtIndividualReport.Rows.Count;
                                        dicColSpan.Add(rowCnt + "-" + 0, "StaffName");
                                        dtIndividualReport.Rows.Add(drowInst);
                                        semCheck = true;
                                    }
                                    #region allot
                                    // string colName = dtHeader.Columns[hd].ColumnName;
                                    string transDate = Convert.ToString(dtHeader.Rows[hd]["Date"]);
                                    string transCode = Convert.ToString(dtHeader.Rows[hd]["Receipt No"]);
                                    transCode = transCode == "0.00" || transCode == "" ? "-" : transCode;
                                    string hdName = Convert.ToString(dtHeader.Rows[hd]["Header"]);
                                    string paymode = Convert.ToString(dtHeader.Rows[hd]["Paymode"]);
                                    paymode = paymode == "" ? "-" : paymode;
                                    string bankCardNo = Convert.ToString(dtHeader.Rows[hd]["Cheque/DD/Card No"]);
                                    bankCardNo = bankCardNo == "" ? "-" : bankCardNo;
                                    string BankCardName = Convert.ToString(dtHeader.Rows[hd]["Bank/Card Name"]);
                                    BankCardName = paymode == "Cheque" || paymode == "DD" ? Convert.ToString(htBankName[BankCardName]) : Convert.ToString(htCardName[BankCardName]);
                                    BankCardName = BankCardName == "" ? "-" : BankCardName;
                                    string narratioN = Convert.ToString(dtHeader.Rows[hd]["Narration"]);
                                    narratioN = narratioN == "" ? "-" : narratioN;
                                    double paidAmt = 0;
                                    double allotAmt = 0;
                                    double balamt = 0;
                                    double.TryParse(Convert.ToString(dtHeader.Rows[hd]["Demand"]), out allotAmt);
                                    double.TryParse(Convert.ToString(dtHeader.Rows[hd]["Receipt"]), out paidAmt);
                                    string allotOrPaid = Convert.ToString(dtHeader.Rows[hd]["AllotOrPaid"]);
                                    drowInst = dtIndividualReport.NewRow();
                                    sno++;
                                    drowInst[0] = Convert.ToString(sno);
                                    drowInst[1] = transDate;
                                    drowInst[2] = transCode;
                                    drowInst[3] = Convert.ToString(hdName);
                                    drowInst[4] = Convert.ToString(paymode);
                                    drowInst[5] = Convert.ToString(bankCardNo);
                                    drowInst[6] = Convert.ToString(BankCardName);
                                    drowInst[7] = Convert.ToString(narratioN);
                                    drowInst[8] = Convert.ToString(paidAmt);
                                    if (!htTotal.ContainsKey(8))
                                        htTotal.Add(8, Convert.ToString(paidAmt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htTotal[8]), out amount);
                                        amount += paidAmt;
                                        htTotal.Remove(8);
                                        htTotal.Add(8, Convert.ToString(amount));
                                    }
                                    drowInst[9] = Convert.ToString(allotAmt);

                                    if (!htTotal.ContainsKey(9))
                                        htTotal.Add(9, Convert.ToString(allotAmt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htTotal[9]), out amount);
                                        amount += allotAmt;
                                        htTotal.Remove(9);
                                        htTotal.Add(9, Convert.ToString(amount));
                                    }
                                    string balAmount = string.Empty;
                                    if (allotOrPaid == "0")//debit add
                                    {
                                        balamt = allotAmt - paidAmt;
                                        fnlDebitAmt += balamt;
                                        balAmount = Convert.ToString(fnlDebitAmt) + "Dr";
                                    }
                                    else//credit  it means advance amount
                                    {
                                        //string selQAmt = " select (sum(excessamt)-sum(adjamt)) as paid from ft_excessdet ex,ft_excessledgerdet exl where ex.excessdetpk=exl.excessdetfk and app_no='" + AppNo + "' and headerfk='" + Convert.ToString(dtHeader.Rows[hd]["CollValue"]) + "' and ledgerfk='' ";
                                        balamt = paidAmt - allotAmt;
                                        fnlDebitAmt -= balamt;
                                        if (fnlDebitAmt < 0)
                                            balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Cr";
                                        else
                                            balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Dr";
                                        string transtype = Convert.ToString(dtHeader.Rows[hd]["transtype"]);
                                        if (transtype == "3")
                                            drowInst[4] = "Adj/Jl/Voucher";
                                    }
                                    drowInst[10] = balAmount;
                                    totDebOrCredit = balAmount;
                                    boolCheck = true;
                                    #endregion

                                    #region deduction
                                    if (ds.Tables[2].Rows.Count > 0 && allotOrPaid == "0")
                                    {
                                        double fnlDeductAmt = 0;
                                        StringBuilder sbDedutReas = new StringBuilder();
                                        ds.Tables[2].DefaultView.RowFilter = strFlterval;
                                        DataTable dtDeduct = ds.Tables[2].DefaultView.ToTable();
                                        if (dtDeduct.Rows.Count == 0)
                                            continue;
                                        for (int dedut = 0; dedut < dtDeduct.Rows.Count; dedut++)
                                        {
                                            double deductAmt = 0;
                                            double.TryParse(Convert.ToString(dtDeduct.Rows[dedut]["DeductAmout"]), out deductAmt);
                                            fnlDeductAmt += deductAmt;
                                            sbDedutReas.Append(Convert.ToString(dtDeduct.Rows[dedut]["textval"]) + ",");
                                        }
                                        if (sbDedutReas.Length > 0)
                                            sbDedutReas.Remove(sbDedutReas.Length - 1, 1);
                                        drowInst[11] = Convert.ToString(fnlDeductAmt);
                                        if (!htTotal.ContainsKey(11))
                                            htTotal.Add(11, Convert.ToString(fnlDeductAmt));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htTotal[11]), out amount);
                                            amount += fnlDeductAmt;
                                            htTotal.Remove(11);
                                            htTotal.Add(11, Convert.ToString(amount));
                                        }
                                        //DeductTotal += FNlAmt;
                                        drowInst[12] = Convert.ToString(sbDedutReas);
                                    }
                                    else
                                    {
                                        drowInst[11] = "0";
                                        drowInst[12] = "-";
                                    }
                                    #endregion

                                    dtIndividualReport.Rows.Add(drowInst);
                                }
                                //total
                                if (htTotal.Count > 0)
                                {
                                    #region total
                                    drowInst = dtIndividualReport.NewRow();
                                    drowInst[0] = "Total";
                                    rowCnt = dtIndividualReport.Rows.Count;
                                    dicColSpan.Add(rowCnt + "-" + 0, "Total");
                                    double grandvalues = 0;
                                    for (int j = 8; j < dtIndividualReport.Columns.Count; j++)
                                    {
                                        if (j == 10)
                                        {
                                            drowInst[10] = totDebOrCredit;
                                            grandCreditORDeb = totDebOrCredit;
                                            continue;
                                        }
                                        double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                                        drowInst[j] = Convert.ToString(grandvalues);

                                        if (!htGrandTotal.ContainsKey(j))
                                            htGrandTotal.Add(j, Convert.ToString(grandvalues));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htGrandTotal[j]), out amount);
                                            amount += grandvalues;
                                            htGrandTotal.Remove(j);
                                            htGrandTotal.Add(j, Convert.ToString(amount));
                                        }
                                    }
                                    dtIndividualReport.Rows.Add(drowInst);
                                    htTotal.Clear();
                                    #endregion
                                }
                            }
                        }
                    }
                    if (htGrandTotal.Count > 0)
                    {
                        #region total
                        drowInst = dtIndividualReport.NewRow();
                        drowInst[0] = "Grand Total";
                        rowCnt = dtIndividualReport.Rows.Count;
                        dicColSpan.Add(rowCnt + "-" + 0, "Grand Total");

                        double grandvalues = 0;
                        for (int j = 8; j < dtIndividualReport.Columns.Count; j++)
                        {
                            if (j == 10)
                            {
                                drowInst[10] = grandCreditORDeb;
                                continue;
                            }
                            double.TryParse(Convert.ToString(htGrandTotal[j]), out grandvalues);
                            drowInst[j] = Convert.ToString(grandvalues);
                        }
                        dtIndividualReport.Rows.Add(drowInst);
                        htGrandTotal.Clear();
                        #endregion
                    }
                }
                #endregion
            }
            if (boolCheck)
            {
                grdIndividualReport.DataSource = dtIndividualReport;
                grdIndividualReport.DataBind();
                grdIndividualReport.Visible = true;

                grdIndividualReport.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grdIndividualReport.Rows[0].Font.Bold = true;
                grdIndividualReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                foreach (KeyValuePair<string, string> dr in dicColSpan)
                {
                    string g = dr.Key;
                    string[] rowC = g.Split('-');
                    int RowFinCnt = Convert.ToInt32(rowC[0]);
                    string DicValue = dr.Value;
                    if (DicValue == "Roll No" || DicValue == "Course")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 5;
                        for (int a = 1; a < 5; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (DicValue == "Semester")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 13;
                        for (int a = 1; a < 13; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (DicValue == "Name" || DicValue == "Department")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[5].ColumnSpan = 8;
                        for (int a = 6; a < 13; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[5].HorizontalAlign = HorizontalAlign.Left;
                    }
                    if (DicValue == "StaffName")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 13;
                        for (int a = 1; a < 13; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[RowFinCnt].BackColor = ColorTranslator.FromHtml("#cc66ff");
                    }
                    if (DicValue == "Total")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[RowFinCnt].BackColor = Color.YellowGreen;
                    }
                    if (DicValue == "Grand Total")
                    {
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].ColumnSpan = 2;
                        for (int a = 1; a < 2; a++)
                            grdIndividualReport.Rows[RowFinCnt].Cells[a].Visible = false;
                        grdIndividualReport.Rows[RowFinCnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdIndividualReport.Rows[RowFinCnt].BackColor = Color.Green;
                    }
                }
                // div1.Visible = true;
                Error.Visible = false;
                rprint.Visible = true;
                lblsmserror.Visible = false;
                txtexcel.Text = "";
            }
            else
            {
                rprint.Visible = false;
                grdIndividualReport.Visible = false;
                //  div1.Visible = false;
                Error.Visible = true;
                Error.Text = "Please Enter the Staff Code!";
            }
        }
        catch { }
    }

    protected Hashtable getStaffDepartment(string collegecode)
    {
        Hashtable htDept = new Hashtable();
        try
        {
            string selQ = " select distinct dept_code as Degree_Code,dept_name as degreename from hrdept_master where college_code='" + collegecode + "'";
            DataSet dsDept = d2.select_method_wo_parameter(selQ, "Text");
            if (dsDept.Tables.Count > 0 && dsDept.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsDept.Tables[0].Rows.Count; row++)
                {
                    if (!htDept.ContainsKey(Convert.ToString(dsDept.Tables[0].Rows[row]["Degree_Code"]).Trim()))
                        htDept.Add(Convert.ToString(dsDept.Tables[0].Rows[row]["Degree_Code"]).Trim(), Convert.ToString(dsDept.Tables[0].Rows[row]["degreename"]));
                }
            }
        }
        catch { }
        return htDept;
    }

    protected void btn_roll_Click(object sender, EventArgs e)
    {

        if (studstaffid.SelectedItem.Value == "1")
        {
            txtno.Text = "";
            popwindow.Visible = true;
            bindType();
            bindbatch1();
            binddegree2();
            bindbranch1();
            bindsec2();
            GrdStudent.Visible = false;
            btn_studOK.Visible = false;
            btn_exitstud.Visible = false;
            checkdicon.Checked = false;
            LoadIncludeSetting();
            lbldisp.Text = "";
            lbldisp.Visible = false;
            checkdicon_Changed(sender, e);
        }
        else
        {
            div_staffLook.Visible = true;
            btn_staffOK.Visible = false;
            btn_exitstaff.Visible = false;
            //spreadStaff.Visible = false;
        }
        if (ddlsearch1.SelectedItem.Value == "0")
        {
            txtsearch1.Visible = true;
        }
        else
        {
            txtsearch1c.Visible = true;
        }




    }

    public static List<string> GetStaffno(string prefixText)
    {


        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            //staff query
            query = " select top 100 staff_code from staffmaster where resign<>1 and staff_code like '" + prefixText + "%' and college_code=" + collegecode1 + " order by staff_code asc";


            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {


        WebService ws = new WebService();
        string query = " select top 100 staff_name+'-'+staff_code from staffmaster where resign<>1 and staff_name like '" + prefixText + "%' and college_code=" + collegecode1 + "  order by staff_name asc";
        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }

    protected void btn_staffLook_Click(object sender, EventArgs e)
    {
        div_staffLook.Visible = true;
        ddlsearch1_OnSelectedIndexChanged(sender, e);
        btn_staffOK.Visible = false;
        btn_exitstaff.Visible = false;
        GrdStudent.Visible = false;
        lbl_errormsgstaff.Visible = false;
    }


    #endregion

    #region Added By Saranya 12Dec2017 For Pdf Print

    protected void btnprint_click(object sender, EventArgs e)
    {
        try
        {
            collegecode = ddl_collegename.SelectedItem.Value;//modified
            //Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            PdfDocument mydoc = new PdfDocument(PdfDocumentFormat.InCentimeters(29, 34.3));
            Gios.Pdf.PdfDocument mypdf = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage deptpdfpage;

            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontbold1 = new Font("Times New Roman", 12, FontStyle.Bold);
            Font Fontbold = new Font("Times New Roman", 20, FontStyle.Bold);
            Font FontHeader = new Font("Times New Roman", 18, FontStyle.Bold);
            Font FontHeaderAf = new Font("Times New Roman", 12, FontStyle.Bold);
            Font FontMedium = new Font("Times New Roman", 12, FontStyle.Regular);
            Font FontText = new Font("Times New Roman", 14, FontStyle.Regular);
            Font Fontbodybold = new Font("Times New Roman", 10, FontStyle.Bold);
            deptpdfpage = mypdf.NewPage();
            Hashtable hschk = new Hashtable();
            Hashtable htBankName = getBankName(collegecode);
            Hashtable htCardName = getCardName(collegecode);
            Hashtable htTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();

            Hashtable htSemTotal = new Hashtable();
            string grandCreditORDeb = string.Empty;
            string grandTotalReceipt = string.Empty;
            string grandTotalDemand = string.Empty;
            string tot = string.Empty;
            double fnlDeductAmt = 0;
            string totDebOrCredit = string.Empty;
            Hashtable htDeptName = getDepartment(collegecode);
            Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
            Gios.Pdf.PdfPage mypdfpage1 = mydoc.NewPage();

            string AppNo = string.Empty;
            string rollType = string.Empty;
            bool beforeAdm = false;
            List<string> rolllist = new List<string>();
            if (!cbTrans.Checked)
            {
                #region without transfer
                if (rbstudtype.SelectedItem.Value == "1")
                {
                    AppNo = getAppNo(txtno.Text.Trim(), collegecode, out rollType, ref beforeAdm);
                    if (AppNo != "0")
                        rolllist.Add(AppNo);
                }
                else
                {
                    string rollMult = Convert.ToString(lblrolldisp.Text);
                    if (rollMult != "")
                    {
                        string[] roll = rollMult.Split(',');
                        if (roll.Length > 0)
                        {
                            for (int i = 0; i < roll.Length; i++)
                            {
                                rollType = " r.Roll_No";
                                AppNo = getAppNo(Convert.ToString(roll[i]), collegecode, out rollType, ref beforeAdm);
                                if (AppNo != "0")
                                    rolllist.Add(AppNo);
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region with transfer
                rollType = " r.Roll_No";
                AppNo = getTransferAppNo(collegecode1, txtno.Text.Trim());
                if (AppNo != "0")
                {

                    rolllist.Add(AppNo);
                }
                #endregion
            }
            int FpCnt = -1;
            for (int rol = 0; rol < rolllist.Count; rol++)
            {
                int RowCnt = 0;
                mypdfpage = mydoc.NewPage();
                string collegename = "";
                string colquery = "select collname from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
                DataSet ds1 = d2.select_method_wo_parameter(colquery, "Text");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    collegename = Convert.ToString(ds1.Tables[0].Rows[0]["collname"]);

                }

                PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, 225, 50, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                mypdfpage.Add(ptc);

                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(13).jpeg")))
                {
                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(13).jpeg"));
                    mypdfpage.Add(LogoImage, 15, 10, 250);
                }
                DataTable dtInfo = new DataTable();

                AppNo = Convert.ToString(rolllist[rol]);
                if (AppNo == "0")
                    continue;
                DataSet ds = dsFormat3(AppNo, rollType, beforeAdm);


                string selQ = "select ('Roll No: '+r.roll_no)as roll_no,r.app_no,('Name: '+stud_name) as stud_name,('Course: '+c.course_name) as course_name,('Department: '+dt.dept_name) as dept_name,('Semester: '+ case when current_semester='1' then '|' when current_semester='2' then '||' when current_semester='3' then '|||' when current_semester='4' then '|V'when current_semester='5' then 'V' when current_semester='6' then 'V|' when current_semester='7' then 'V||' when current_semester='8' then 'V|||' end) as semester from registration r,degree d,course c,department dt where r.degree_code=d.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code=r.college_code and r.app_no='" + AppNo + "'  ";

                DataSet dsStud = d2.select_method_wo_parameter(selQ, "Text");
                if (dsStud.Tables.Count > 0 && dsStud.Tables[0].Rows.Count > 0)
                {
                    PdfTextArea ptc1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 15, 150, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dsStud.Tables[0].Rows[0]["roll_no"]) + "");
                    mypdfpage.Add(ptc1);
                    PdfTextArea ptc2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 250, 150, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dsStud.Tables[0].Rows[0]["stud_name"]) + "");
                    mypdfpage.Add(ptc2);
                    PdfTextArea ptc3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 15, 170, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dsStud.Tables[0].Rows[0]["course_name"]) + "");
                    mypdfpage.Add(ptc3);
                    PdfTextArea ptc4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                           new PdfArea(mydoc, 250, 170, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dsStud.Tables[0].Rows[0]["dept_name"]) + "");
                    mypdfpage.Add(ptc4);
                }
                int Sno = 0;
                double fnlDebitAmt = 0;
                ArrayList arRecpt = new ArrayList();

                //============New code==============//

                //if (StudwiseRowCnt.ContainsKey(AppNo))
                //{
                //    RowCnt = Convert.ToInt32(StudwiseRowCnt[AppNo]);
                //}
                Hashtable a = (Hashtable)Session["htStudwiseRowCnt"];
                if (a.ContainsKey(AppNo))
                {
                    RowCnt = Convert.ToInt32(StudwiseRowCnt[AppNo]);
                }
                //====================================//
                //int TR = 0; int DummyTR = 0;

                Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, 30, 13, 1);
                //mydoc = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 200, 800, 1200));
                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                table.VisibleHeaders = false;
                table.Columns[0].SetWidth(50);
                table.Columns[1].SetWidth(100);
                table.Columns[2].SetWidth(130);
                table.Columns[3].SetWidth(130);
                table.Columns[4].SetWidth(130);
                table.Columns[5].SetWidth(170);
                table.Columns[6].SetWidth(150);
                table.Columns[7].SetWidth(100);
                table.Columns[8].SetWidth(90);
                table.Columns[9].SetWidth(90);
                table.Columns[10].SetWidth(100);
                table.Columns[11].SetWidth(100);
                table.Columns[12].SetWidth(150);

                table.CellRange(0, 0, 0, 12).SetFont(Fontbold1);
                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 0).SetContent("S.No");
                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 1).SetContent("Date");
                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 2).SetContent("Receipt NO");
                table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 3).SetContent("Header");
                table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 4).SetContent("Paymode");
                table.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 5).SetContent("Cheque/DD/Card No");
                table.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 6).SetContent("Bank/Card Name");
                table.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 7).SetContent("Narration");
                table.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 8).SetContent("Receipt");
                table.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 9).SetContent("Demand");
                table.Cell(0, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 10).SetContent("Balance");
                table.Cell(0, 11).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 11).SetContent("Deduction Amt");
                table.Cell(0, 12).SetContentAlignment(ContentAlignment.MiddleCenter);
                table.Cell(0, 12).SetContent("Deduction Reason");
                int TR = 0;

                for (int dsrow = 0; dsrow < RowCnt; dsrow++)
                {
                    TR++;
                    FpCnt++;
                    if (TR < 29)
                    {

                        if (dsrow == 0)
                        {
                            table.Cell(TR, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 0).SetContent(Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[2].Text));
                            table.Cell(TR, 0).ColSpan = 13;
                            TR++;
                            dsrow++;
                            FpCnt++;
                        }

                        string sno = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[0].Text);

                        if (sno != "")
                        {

                            string transDate = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[1].Text);
                            string transCode = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[2].Text);
                            string hdName = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[3].Text);
                            string paymode = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[4].Text);
                            string bankCardNo = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[5].Text);
                            string BankCardName = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[6].Text);
                            string narratioN = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[7].Text);
                            string paid = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[8].Text);
                            string demand = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[9].Text);
                            string balance = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[10].Text);
                            string deductamt = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[11].Text);
                            string deductReason = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[12].Text);


                            // Sno++;
                            table.Cell(TR, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table.Cell(TR, 0).SetContent(Convert.ToString(sno));
                            table.Cell(TR, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 1).SetContent(transDate);
                            table.Cell(TR, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 2).SetContent(transCode);
                            table.Cell(TR, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 3).SetContent(Convert.ToString(hdName));
                            table.Cell(TR, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 4).SetContent(Convert.ToString(paymode));
                            table.Cell(TR, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 5).SetContent(Convert.ToString(bankCardNo));
                            table.Cell(TR, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 6).SetContent(Convert.ToString(BankCardName));
                            table.Cell(TR, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 7).SetContent(Convert.ToString(narratioN));
                            table.Cell(TR, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 8).SetContent(Convert.ToString(paid));
                            table.Cell(TR, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 9).SetContent(Convert.ToString(demand));
                            table.Cell(TR, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 10).SetContent(Convert.ToString(balance));
                            table.Cell(TR, 11).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 11).SetContent(Convert.ToString(deductamt));
                            table.Cell(TR, 12).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 12).SetContent(Convert.ToString(deductReason));
                        }


                        #region excess
                        //try
                        //{
                        //    if (allotOrPaid == "1" && ds.Tables[4].Rows.Count > 0)
                        //    {
                        //        DataTable dvex = new DataTable();
                        //        try
                        //        {
                        //            // transDate = transDate.Split('/')[1] + "/" + transDate.Split('/')[0] + "/" + transDate.Split('/')[2];
                        //            if (ddl_hdr.SelectedItem.Text.Trim() == "Ledger")
                        //                ds.Tables[4].DefaultView.RowFilter = "rcptdate='" + transDate + "' and receiptno='" + transCode + "' and paymode='" + paymode + "' ";
                        //            else
                        //                ds.Tables[4].DefaultView.RowFilter = "rcptdate='" + transDate + "' and receiptno='" + transCode + "' and paymode='" + paymode + "'";
                        //        }
                        //        catch { }
                        //        dvex = ds.Tables[4].DefaultView.ToTable();
                        //        if (dvex.Rows.Count > 0 && !arRecpt.Contains(transCode))
                        //        {
                        //            for (int k = 0; k < dvex.Rows.Count; k++)
                        //            {
                        //                #region excess Amt

                        //                Sno++;
                        //                table.Cell(TR, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        //                table.Cell(TR, 0).SetContent(Convert.ToString(Sno));
                        //                table.Cell(TR, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //                table.Cell(TR, 1).SetContent(Convert.ToString(dvex.Rows[k]["rcptdate"]));
                        //                table.Cell(TR, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //                table.Cell(TR, 2).SetContent(Convert.ToString(dvex.Rows[k]["receiptno"]));

                        //                string valueFK = Convert.ToString(dvex.Rows[k]["fk"]);
                        //                string Name = Convert.ToString(dvex.Rows[k]["name"]);
                        //                if (ddl_hdr.SelectedItem.Text.Trim() == "Group Header")
                        //                {
                        //                    //string hedFK = d2.GetFunction("select Headerfk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and l.ledgerpk='" + ledgerfk + "'");
                        //                    string stream = d2.GetFunction("select c.type from registration r,degree d,Course c where r.degree_code=d.degree_code and d.course_id=c.course_id and r.app_no ='" + Convert.ToString(dvex.Rows[k]["app_no"]) + "'");

                        //                    Name = d2.GetFunction("  select ChlGroupHeader from FS_ChlGroupHeaderSettings where headerfk='" + Convert.ToString(dtHeader.Rows[row]["CollValue"]) + "' and Stream='" + stream + "'");
                        //                }

                        //                table.Cell(TR, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //                table.Cell(TR, 3).SetContent(Name + "(" + "Excess)");
                        //                table.Cell(TR, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //                table.Cell(TR, 4).SetContent(Convert.ToString(dvex.Rows[k]["paymode"]));
                        //                table.Cell(TR, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //                table.Cell(TR, 5).SetContent("-");
                        //                table.Cell(TR, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //                table.Cell(TR, 6).SetContent(Convert.ToString(BankCardName));
                        //                table.Cell(TR, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //                table.Cell(TR, 7).SetContent("-");

                        //                double tempamt = 0;
                        //                double.TryParse(Convert.ToString(dvex.Rows[k]["amount"]), out tempamt);
                        //                table.Cell(TR, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //                table.Cell(TR, 8).SetContent(Convert.ToString(tempamt));
                        //                //FpSpread1.Sheets[0].Cells[rowCnt, 8].Text = Convert.ToString(tempamt);
                        //                if (!htTotal.ContainsKey(8))
                        //                    htTotal.Add(8, Convert.ToString(tempamt));
                        //                else
                        //                {
                        //                    double amount = 0;
                        //                    double.TryParse(Convert.ToString(htTotal[8]), out amount);
                        //                    amount += tempamt;
                        //                    htTotal.Remove(8);
                        //                    htTotal.Add(8, Convert.ToString(amount));
                        //                }
                        //                table.Cell(TR, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //                table.Cell(TR, 9).SetContent(Convert.ToString(allotAmt));

                        //                if (allotOrPaid == "0")//debit add
                        //                {
                        //                    balamt = allotAmt - paidAmt;
                        //                    fnlDebitAmt += balamt;
                        //                    balAmount = Convert.ToString(fnlDebitAmt) + "Dr";

                        //                }
                        //                else//credit  it means advance amount
                        //                {
                        //                    //string selQAmt = " select (sum(excessamt)-sum(adjamt)) as paid from ft_excessdet ex,ft_excessledgerdet exl where ex.excessdetpk=exl.excessdetfk and app_no='" + AppNo + "' and headerfk='" + Convert.ToString(dtHeader.Rows[hd]["CollValue"]) + "' and ledgerfk='' ";
                        //                    balamt = tempamt;
                        //                    fnlDebitAmt -= balamt;
                        //                    if (fnlDebitAmt < 0)
                        //                        balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Cr";
                        //                    else
                        //                        balAmount = Convert.ToString(fnlDebitAmt).TrimStart('-') + "Dr";
                        //                }
                        //                table.Cell(TR, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                        //                table.Cell(TR, 10).SetContent(Convert.ToString(balAmount));

                        //                totDebOrCredit = balAmount;
                        //                #endregion
                        //            }
                        //            arRecpt.Add(transCode);
                        //        }
                        //    }
                        //}
                        //catch
                        //{

                        //}
                        #endregion


                        else
                        {
                            table.Cell(TR, 0).SetContent("Total");
                            table.Cell(TR, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(TR, 0).SetContentAlignment(ContentAlignment.TopLeft);

                            table.Cell(TR, 0).ColSpan = 8;
                            string totPaid = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[8].Text);
                            table.Cell(TR, 8).SetContent(Convert.ToString(totPaid));

                            string totDemand = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[9].Text);
                            table.Cell(TR, 9).SetContent(Convert.ToString(totDemand));

                            string totBalance = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[10].Text);
                            table.Cell(TR, 10).SetContent(Convert.ToString(totBalance));

                            string totDeductamt = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[11].Text);
                            table.Cell(TR, 11).SetContent(Convert.ToString(totDeductamt));

                            string deductreson = Convert.ToString(grdIndividualReport.Rows[FpCnt + 3].Cells[12].Text);
                            table.Cell(TR, 12).SetContent(Convert.ToString(deductreson));


                            if (dsrow != RowCnt - 2)
                            {
                                TR++;
                                table.Cell(TR, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(TR, 0).SetContent(Convert.ToString(grdIndividualReport.Rows[FpCnt + 4].Cells[0].Text));
                                table.Cell(TR, 0).ColSpan = 13;
                                //TR++;
                                FpCnt++;
                                dsrow++;
                            }
                            else
                            {
                                TR++;
                                table.Cell(TR, 0).SetContent("Grand Total");
                                table.Cell(TR, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                //table.Cell(TR, 0).SetContent(Convert.ToString("Grand Total"));
                                table.Cell(TR, 0).SetContentAlignment(ContentAlignment.TopLeft);
                                table.Cell(TR, 0).ColSpan = 8;
                                string gtPaid = Convert.ToString(grdIndividualReport.Rows[FpCnt + 4].Cells[8].Text);
                                table.Cell(TR, 8).SetContent(Convert.ToString(gtPaid));

                                string gtDemand = Convert.ToString(grdIndividualReport.Rows[FpCnt + 4].Cells[9].Text);
                                table.Cell(TR, 9).SetContent(Convert.ToString(gtDemand));

                                string gtBalance = Convert.ToString(grdIndividualReport.Rows[FpCnt + 4].Cells[10].Text);
                                table.Cell(TR, 10).SetContent(Convert.ToString(gtBalance));

                                string gtDeductamt = Convert.ToString(grdIndividualReport.Rows[FpCnt + 4].Cells[11].Text);
                                table.Cell(TR, 11).SetContent(Convert.ToString(gtDeductamt));

                                string gtdeductreson = Convert.ToString(grdIndividualReport.Rows[FpCnt + 4].Cells[12].Text);
                                table.Cell(TR, 12).SetContent(Convert.ToString(gtdeductreson));
                                dsrow++;
                                FpCnt += 5;
                            }
                        }

                    }
                    else
                    {
                        #region Next Page

                        Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 200, 800, 1200));
                        mypdfpage.Add(newpdftabpage);
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydoc.NewPage();

                        int NewtableRow = 0;
                        if (ds.Tables[1].Rows.Count + (ds.Tables[0].Rows.Count * 2) - TR > 20)
                            NewtableRow = 30;
                        else
                        {
                            NewtableRow = ds.Tables[1].Rows.Count + (ds.Tables[0].Rows.Count * 2) - TR + 3;
                            string collegename1 = "";
                            string colquery1 = "select collname from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
                            DataSet ds2 = d2.select_method_wo_parameter(colquery, "Text");
                            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                            {
                                collegename1 = Convert.ToString(ds1.Tables[0].Rows[0]["collname"]);

                            }

                            PdfTextArea ptc1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 225, 50, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename1);
                            mypdfpage.Add(ptc1);

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(13).jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(13).jpeg"));
                                mypdfpage.Add(LogoImage, 15, 10, 250);
                            }
                            AppNo = Convert.ToString(rolllist[rol]);
                            if (AppNo == "0")
                                continue;
                            DataSet ds3 = dsFormat3(AppNo, rollType, beforeAdm);


                            string selQ1 = "select ('Roll No: '+r.roll_no)as roll_no,r.app_no,('Name: '+stud_name) as stud_name,('Course: '+c.course_name) as course_name,('Department: '+dt.dept_name) as dept_name,('Semester: '+ case when current_semester='1' then '|' when current_semester='2' then '||' when current_semester='3' then '|||' when current_semester='4' then '|V'when current_semester='5' then 'V' when current_semester='6' then 'V|' when current_semester='7' then 'V||' when current_semester='8' then 'V|||' end) as semester from registration r,degree d,course c,department dt where r.degree_code=d.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code=r.college_code and r.app_no='" + AppNo + "'  ";

                            DataSet dsStud1 = d2.select_method_wo_parameter(selQ1, "Text");
                            if (dsStud.Tables.Count > 0 && dsStud.Tables[0].Rows.Count > 0)
                            {
                                PdfTextArea ptc3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydoc, 15, 150, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dsStud.Tables[0].Rows[0]["roll_no"]) + "");
                                mypdfpage.Add(ptc3);
                                PdfTextArea ptc2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydoc, 250, 150, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dsStud.Tables[0].Rows[0]["stud_name"]) + "");
                                mypdfpage.Add(ptc2);
                                PdfTextArea ptc4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                   new PdfArea(mydoc, 15, 170, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dsStud.Tables[0].Rows[0]["course_name"]) + "");
                                mypdfpage.Add(ptc4);
                                PdfTextArea ptc5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mydoc, 250, 170, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(dsStud.Tables[0].Rows[0]["dept_name"]) + "");
                                mypdfpage.Add(ptc5);
                            }

                        }
                        //DummyTR = 0; TR = 0;
                        table = mydoc.NewTable(Fontsmall, NewtableRow, 13, 2);
                        table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table.VisibleHeaders = false;
                        table.Columns[0].SetWidth(50);
                        table.Columns[1].SetWidth(100);
                        table.Columns[2].SetWidth(130);
                        table.Columns[3].SetWidth(130);
                        table.Columns[4].SetWidth(130);
                        table.Columns[5].SetWidth(170);
                        table.Columns[6].SetWidth(150);
                        table.Columns[7].SetWidth(100);
                        table.Columns[8].SetWidth(90);
                        table.Columns[9].SetWidth(90);
                        table.Columns[10].SetWidth(100);
                        table.Columns[11].SetWidth(100);
                        table.Columns[12].SetWidth(150);

                        table.CellRange(0, 0, 0, 12).SetFont(Fontbold1);
                        table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 0).SetContent("S.No");
                        table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 1).SetContent("Date");
                        table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 2).SetContent("Receipt NO");
                        table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 3).SetContent("Header");
                        table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 4).SetContent("Paymode");
                        table.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 5).SetContent("Cheque/DD/Card No");
                        table.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 6).SetContent("Bank/Card Name");
                        table.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 7).SetContent("Narration");
                        table.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 8).SetContent("Receipt");
                        table.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 9).SetContent("Demand");
                        table.Cell(0, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 10).SetContent("Balance");
                        table.Cell(0, 11).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 11).SetContent("Deduction Amt");
                        table.Cell(0, 12).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 12).SetContent("Deduction Reason");

                        TR = 0;
                        FpCnt--;
                        dsrow--;
                        #endregion
                    }

                }

                Gios.Pdf.PdfTablePage newpdftabpage1 = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 15, 200, 800, 1200));
                mypdfpage.Add(newpdftabpage1);
                mypdfpage.SaveToDocument();


            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "FeeStatus" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }

        }
        catch
        {

        }
    }

    #endregion

}