using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;

public partial class CancelReceiptReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    static byte roll = 0;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    static DAccess2 d22 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    DAccess2 queryObject = new DAccess2();
    ReuasableMethods reUse = new ReuasableMethods();
    DAccess2 da = new DAccess2();
    DataSet sdn = new DataSet();
    static string usercodestat = string.Empty;
    static string collegecodestat = string.Empty;
    static int chosedmode = 0;
    static byte BalanceType = 0;

    string batch = "";
    string degree = "";
    string exammonth = "";
    string examyear = "";
    string colg = "";
    string dept = "";
    int commcount;
    int i;
    int cout;
    int row;
    string college = "";
    bool check = false;
    static Hashtable studhash = new Hashtable();

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
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();

            if (!IsPostBack)
            {
                bindclg();
                if (ddl_college.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
                }
                createLogo(collegecode1);
                LoadFromSettings();
                bindBtch();
                binddeg();
                binddept();
                bindheader();
                txt_regno.Attributes.Add("placeholder", "Roll No");
                chosedmode = 0;
                //txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txt_date.Attributes.Add("readonly", "readonly");
                txt_chaln.Attributes.Add("readonly", "readonly");

                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");

                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
            }
            if (ddl_college.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                txt_chaln.Text = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
            }
            collegecodestat = collegecode1;
            usercodestat = usercode;
            loadCancelDuplicateButtons(usercode, collegecode1);
        }
        catch (Exception ex) { }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadFromSettings();
        bindBtch();
        binddeg();
        binddept();
        bindheader();
        txt_regno.Text = "";
        txt_name.Text = "";
        txt_chno.Text = "";
        btn_go_Click(sender, e);
        createLogo(ddl_college.SelectedValue);
    }
    public void LoadFromSettings()
    {
        try
        {
            ListItem lst1 = new ListItem("Roll No", "0");
            ListItem lst2 = new ListItem("Reg No", "1");
            ListItem lst3 = new ListItem("Admission No", "2");
            ListItem lst4 = new ListItem("App No", "3");
            ListItem lst5 = new ListItem("Smartcard No", "4");
            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rollno.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //App Form Number - Application Number
                rbl_rollno.Items.Add(lst4);

            }
            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptSmartNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Smartcard Number - smart_serial_no
                rbl_rollno.Items.Add(lst5);

            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(lst1);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ShowBalanceType' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                BalanceType = 1;

            }
            else
            {
                BalanceType = 0;
            }

            //Receipt or Challan or Both
            //save1 = 0;
            //insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanOrReceiptOrBoth' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            //save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            //rdo_receipt.Enabled = false;
            //rdo_challan.Enabled = false;
            //if (save1 == 1)
            //{
            //    //Receipt
            //    rdo_receipt.Enabled = true;
            //}
            //if (save1 == 2)
            //{
            //    //Challan
            //    rdo_challan.Enabled = true;

            //}
            //if (save1 == 3)
            //{
            //    //Receipt and Challan
            //    rdo_receipt.Enabled = true;
            //    rdo_challan.Enabled = true;
            //}

            //Headerwise or Group Header wise or Ledger wise
            //save1 = 0;
            //insqry1 = "select LinkValue from New_InsSettings where LinkName='GroupHeaderOrHeaderOrLedger' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            //save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            //rbl_headerselect.Items[0].Enabled = false;
            //rbl_headerselect.Items[1].Enabled = false;
            //rbl_headerselect.Items[2].Enabled = false;
            //if (save1 == 1)
            //{
            //    //Group Header
            //    rbl_headerselect.Items[0].Enabled = true;
            //}
            //if (save1 == 2)
            //{
            //    //Header
            //    rbl_headerselect.Items[1].Enabled = true;
            //}
            //if (save1 == 3)
            //{
            //    //Ledger
            //    rbl_headerselect.Items[2].Enabled = true;

            //}
            //if (save1 == 4)
            //{
            //    //All

            //    rbl_headerselect.Items[0].Enabled = true;
            //    rbl_headerselect.Items[1].Enabled = true;
            //    rbl_headerselect.Items[2].Enabled = true;
            //}
            //if (save1 == 5)
            //{
            //    //GH and Ledger
            //    rbl_headerselect.Items[0].Enabled = true;
            //    rbl_headerselect.Items[2].Enabled = true;
            //}
            //if (save1 == 6)
            //{
            //    //Ledger and Header                        
            //    rbl_headerselect.Items[1].Enabled = true;
            //    rbl_headerselect.Items[2].Enabled = true;
            //}
            //if (save1 == 7)
            //{
            //    //GHeader and Ledger
            //    rbl_headerselect.Items[0].Enabled = true;
            //    rbl_headerselect.Items[2].Enabled = true;
            //}

            //rbl_headerselect.SelectedIndex = -1;
            //if (save1 == 5 || save1 == 1 || save1 == 7 || save1 == 4)
            //{
            //    rbl_headerselect.SelectedIndex = 0;
            //}
            //else if (save1 == 2 || save1 == 6)
            //{
            //    rbl_headerselect.SelectedIndex = 1;
            //}
            //else if (save1 == 3)
            //{
            //    rbl_headerselect.SelectedIndex = 2;
            //}

            ////Is Challan or Receipt Header wise ?


            //insqry1 = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + selected_usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ";
            //save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            //if (save1 == 2)
            //{
            //    //If Challan Selected
            //    cbl_HdrwiseChlnRcpt.Items[0].Selected = true;
            //}
            //if (save1 == 1)
            //{
            //    //If Receipt Selected
            //    cbl_HdrwiseChlnRcpt.Items[1].Selected = true;
            //}
            //if (save1 == 3)
            //{

            //    cbl_HdrwiseChlnRcpt.Items[0].Selected = true;
            //    cbl_HdrwiseChlnRcpt.Items[1].Selected = true;
            //}

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void bindBtch()
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
                    for (i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {

                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
            }
            binddeg();
            binddept();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_batch.Checked = false;
            commcount = 0;
            txt_batch.Text = "--Select--";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
            }
            binddeg();
            binddept();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            batch = "";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }
            if (batch != "")
            {
                ds.Clear();
                ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_degree.DataSource = ds;
                    cbl_degree.DataTextField = "course_name";
                    cbl_degree.DataValueField = "course_id";
                    cbl_degree.DataBind();
                    if (cbl_degree.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_degree.Items.Count; i++)
                        {
                            cbl_degree.Items[i].Selected = true;
                        }
                        txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                        cb_degree.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_degree.Text = "--Select--";
            if (cb_degree.Checked == true)
            {

                for (i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
            }
            binddept();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_dept.Checked = false;
            commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
                txt_degree.Text = "Degree(" + commcount.ToString() + ")";
            }
            binddept();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            batch = "";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }
            }
            degree = "";
            for (i = 0; i < cbl_degree.Items.Count; i++)
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

            if (batch != "" && degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = "Department(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_dept.Text = "--Select--";
            if (cb_dept.Checked == true)
            {

                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                }
                txt_dept.Text = "Department(" + (cbl_dept.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_dept.Checked = false;
            commcount = 0;
            txt_dept.Text = "--Select--";
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_dept.Items.Count)
                {
                    cb_dept.Checked = true;
                }
                txt_dept.Text = "Department(" + commcount.ToString() + ")";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void cb_header_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_header.Text = "--Select--";
            if (cb_header.Checked == true)
            {

                for (i = 0; i < cbl_header.Items.Count; i++)
                {
                    cbl_header.Items[i].Selected = true;
                }
                txt_header.Text = "Header(" + (cbl_header.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_header.Items.Count; i++)
                {
                    cbl_header.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void cbl_header_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_header.Checked = false;
            commcount = 0;
            txt_header.Text = "--Select--";
            for (i = 0; i < cbl_header.Items.Count; i++)
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
                txt_header.Text = "Header(" + commcount.ToString() + ")";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    public void bindheader()
    {
        try
        {
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";

            DataSet dsHeader = d2.select_method_wo_parameter(query, "Text");
            if (dsHeader.Tables[0].Rows.Count > 0)
            {
                cbl_header.DataSource = dsHeader;
                cbl_header.DataTextField = "HeaderName";
                cbl_header.DataValueField = "HeaderPK";
                cbl_header.DataBind();
                for (int i = 0; i < cbl_header.Items.Count; i++)
                {
                    cbl_header.Items[i].Selected = true;
                }
                txt_header.Text = "Header (" + cbl_header.Items.Count + ")";
                cb_header.Checked = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void checkDate(object sender, EventArgs e)
    {
        try
        {
            DateTime fromdate = Convert.ToDateTime(txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2]);
            DateTime todate = Convert.ToDateTime(txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2]);

            if (fromdate <= todate)
            {
            }
            else
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                imgAlert.Visible = true;
                lbl_alert.Text = "From Date Should Not Exceed To Date";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            #region Basic Data
            string selectQuery = string.Empty;
            string dispRoll = string.Empty;
            string chlnNo = txt_chno.Text.Trim();
            string chlnAcr = txt_chaln.Text.Trim();
            string chlnCode = chlnAcr + chlnNo;
            //string confDate = txt_date.Text.Trim();
            string name = txt_name.Text.Trim();
            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string rollno = string.Empty;

            rptprint.Visible = false;

            StringBuilder hdrid = new StringBuilder();
            StringBuilder btch = new StringBuilder();
            StringBuilder dept = new StringBuilder();
            if (Convert.ToByte(rbl_Memtype.SelectedValue) == 1)
            {
                if (txt_regno.Text.Trim() == "")
                {
                    if (name != "")
                    {
                        try
                        {
                            rollno = name.Split('-')[4];
                            //rbl_rollno.SelectedIndex = 0;
                        }
                        catch { }
                    }
                    else
                    {
                        rollno = string.Empty;
                    }
                }
                else
                {
                    rollno = txt_regno.Text.Trim();
                    txt_name.Text = "";
                }

                for (int i = 0; i < cbl_header.Items.Count; i++)
                {
                    if (cbl_header.Items[i].Selected)
                    {
                        if (hdrid.Length == 0)
                        {
                            hdrid.Append(Convert.ToString(cbl_header.Items[i].Value));
                        }
                        else
                        {
                            hdrid.Append("," + Convert.ToString(cbl_header.Items[i].Value));
                        }
                    }
                }

                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected)
                    {
                        if (btch.Length == 0)
                        {
                            btch.Append(Convert.ToString(cbl_batch.Items[i].Value));
                        }
                        else
                        {
                            btch.Append("," + Convert.ToString(cbl_batch.Items[i].Value));
                        }
                    }
                }

                for (int i = 0; i < cbl_dept.Items.Count; i++)
                {
                    if (cbl_dept.Items[i].Selected)
                    {
                        if (dept.Length == 0)
                        {
                            dept.Append(Convert.ToString(cbl_dept.Items[i].Value));
                        }
                        else
                        {
                            dept.Append("," + Convert.ToString(cbl_dept.Items[i].Value));
                        }
                    }
                }
            }
            #endregion
            #region Query Section

            string fromdate = txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2];
            string todate = txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2];
            string paymode = "1,2,3";
            if (AllowBankImpDup())
            {
                paymode = "1,2,3,5";
            }

            if (Convert.ToByte(rbl_Memtype.SelectedValue) == 1)
            {
                #region For Students
                FpSpread1.Width = 800;
                if (ddlBefAfteAdm.SelectedIndex == 0)
                {
                    //After Admission
                    selectQuery = "SELECT TransCode as ChallanNo,convert(varchar(10), TransDate,103) as ChallanDate,app_formno,r.smart_serial_no,R.Reg_No,R.Roll_Admit,R.Roll_No,R.App_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,SUM(debit) as TakenAmt, isnull(c.Iscanceled,0) as Iscanceled,c.canceledRcptRemarks  FROM FT_FinDailyTransaction C,applyn A,Registration R,Degree G,Course U,Department D WHERE C.App_No = A.app_no AND a.app_no = r.App_No and A.degree_code = G.Degree_Code AND G.Course_Id = u.Course_Id and g.college_code = u.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and isnull(c.Transtype,0)=1 and c.Transcode<>'' and c.Paymode in(" + paymode + ")  and TransDate between '" + fromdate + "' and '" + todate + "'   and r.college_code=" + collegecode1 + " and  isnull(c.Iscanceled,0) ='1' ";

                    if (chlnNo != "")
                    {
                        selectQuery += " and TransCode = '" + chlnCode + "' ";
                    }
                    else
                    {
                        if (rollno != "")
                        {
                            if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                            {
                                selectQuery += " and r.roll_No = '" + rollno + "' ";
                            }
                            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                            {
                                selectQuery += " and r.reg_no = '" + rollno + "' ";
                            }
                            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                            {
                                selectQuery += " and r.roll_admit = '" + rollno + "' ";
                            }
                            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                            {
                                selectQuery += " and r.smart_serial_no = '" + rollno + "' ";
                            }
                            else
                            {
                                selectQuery += " and a.app_formno = '" + rollno + "'  and a.college_code=" + collegecode1 + "";
                            }
                        }

                        if (hdrid.Length > 0)
                        {
                            selectQuery += " and  C.headerfk in(" + hdrid + " ) ";
                        }
                        if (btch.Length > 0)
                        {
                            selectQuery += " and a.batch_year in(" + btch + " ) ";
                        }
                        if (dept.Length > 0)
                        {
                            selectQuery += " and a.degree_code in(" + dept + " ) ";
                        }
                    }

                    selectQuery += " GROUP BY TransCode,TransDate,app_formno,R.Stud_Name, Course_Name, Dept_Name, R.Reg_No, R.Roll_Admit, R.Roll_No,R.App_No,c.Iscanceled,r.smart_serial_no,c.canceledRcptRemarks order by TransDate desc";
                }
                else
                {
                    byte studAppSHrtAdm = StudentAppliedShorlistAdmit();
                    string admStudFilter = "";
                    switch (studAppSHrtAdm)
                    {
                        case 0:
                            admStudFilter = " and a.isconfirm=1  and isnull(a.selection_status,'0')='0' and isnull(a.admission_status,'0')='0'  and a.app_no not in (select app_no from registration where Degree_Code in(" + dept + ")  and batch_year in(" + btch + "))";
                            break;
                        case 1:
                            admStudFilter = " and a.isconfirm=1 and isnull(a.selection_status,'0')='1' and isnull(a.admission_status,'0')='0'  and a.app_no not in (select app_no from registration where Degree_Code in(" + dept + ")  and batch_year in(" + btch + "))";
                            break;
                        case 2:
                            admStudFilter = " and a.isconfirm=1 and isnull(a.selection_status,'0')='1' and isnull(a.admission_status,'0')='1' and a.app_no not in (select app_no from registration where Degree_Code in(" + dept + ")  and batch_year in(" + btch + "))";
                            break;
                    }

                    selectQuery = "SELECT TransCode as ChallanNo,convert(varchar(10), TransDate,103) as ChallanDate,app_formno,app_formno as smart_serial_no,app_formno as Reg_No,app_formno as Roll_Admit,app_formno as Roll_No,A.App_No,A.Stud_Name,Course_Name+'-'+Dept_Name Degree,SUM(debit) as TakenAmt, isnull(c.Iscanceled,0) as Iscanceled,c.canceledRcptRemarks FROM FT_FinDailyTransaction C,applyn A,Degree G,Course U,Department D WHERE C.App_No = A.app_no and A.degree_code = G.Degree_Code AND G.Course_Id = u.Course_Id and g.college_code = u.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and isnull(c.Transtype,0)=1 and c.Transcode<>'' and c.Paymode in(" + paymode + ")  and TransDate between '" + fromdate + "' and '" + todate + "'  and  isnull(c.Iscanceled,0) ='1'  ";
                    if (chlnNo != "")
                    {
                        selectQuery += " and TransCode = '" + chlnCode + "' ";
                    }
                    else
                    {
                        if (rollno != "")
                        {
                            //if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                            //{
                            //    selectQuery += " and r.roll_No = '" + rollno + "' ";
                            //}
                            //else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                            //{
                            //    selectQuery += " and r.reg_no = '" + rollno + "' ";
                            //}
                            //else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                            //{
                            //    selectQuery += " and r.roll_admit = '" + rollno + "' ";
                            //}
                            //else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                            //{
                            //    selectQuery += " and r.smart_serial_no = '" + rollno + "' ";
                            //}
                            //else
                            //{
                            selectQuery += " and a.app_formno = '" + rollno + "'  and a.college_code=" + collegecode1 + " ";
                            //}
                        }

                        if (hdrid.Length > 0)
                        {
                            selectQuery += " and  C.headerfk in(" + hdrid + " ) ";
                        }
                        if (btch.Length > 0)
                        {
                            selectQuery += " and a.batch_year in(" + btch + " ) ";
                        }
                        if (dept.Length > 0)
                        {
                            selectQuery += " and a.degree_code in(" + dept + " ) ";
                        }
                    }

                    selectQuery += admStudFilter;
                    selectQuery += " GROUP BY TransCode,TransDate,app_formno,A.Stud_Name, Course_Name, Dept_Name,A.App_No,c.Iscanceled,c.canceledRcptRemarks order by TransDate desc";
                }
                #endregion
            }
            else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 2)
            {
                //For Staff
                selectQuery = "select TransCode as ChallanNo,convert(varchar(10), TransDate,103) as ChallanDate,s.staff_code as app_formno,s.staff_code as smart_serial_no,s.staff_code as Reg_No,s.staff_code as Roll_Admit,'' Roll_No,App_No,s.staff_name as Stud_Name,h.dept_name as  Degree,SUM(debit) as TakenAmt, isnull(Iscanceled,0) as Iscanceled,f.canceledRcptRemarks  from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,stafftrans t,hrdept_master h,desig_master d where sa.appl_id =f.App_No and s.staff_code =t.staff_code and t.desig_code =d.desig_code and h.dept_code =t.dept_code and latestrec ='1' and s.college_code =d.collegeCode and h.college_code =s.college_code and sa.appl_no =s.appl_no  and f.MemType ='2'  and isnull(Transtype,0)=1 and Transcode<>'' and f.Paymode in(" + paymode + ")  and TransDate between '" + fromdate + "' and '" + todate + "' and  isnull(f.Iscanceled,0) ='1'   ";
                if (chlnNo != "")
                {
                    selectQuery += " and TransCode = '" + chlnCode + "' ";
                }

                selectQuery += " GROUP BY TransCode,TransDate,s.staff_code,  h.Dept_Name, App_No,s.staff_name,Iscanceled,f.canceledRcptRemarks order by TransDate desc";
            }
            else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 3)
            {
                //For Vendor
                selectQuery = "select TransCode as ChallanNo,convert(varchar(10), TransDate,103) as ChallanDate,'' app_formno,'' smart_serial_no,'' Reg_No,'' Roll_Admit,'' Roll_No,App_No, VenContactName as Stud_Name,'' Degree,SUM(debit) as TakenAmt, isnull(Iscanceled,0) as Iscanceled,f.canceledRcptRemarks from FT_FinDailyTransaction f,CO_VendorMaster v,IM_VendorContactMaster vc where f.App_No =vc.VendorContactPK and v.VendorPK =vc.VendorFK and f.MemType ='3'  and isnull(Transtype,0)=1 and Transcode<>'' and Paymode in(" + paymode + ")  and TransDate between '" + fromdate + "' and '" + todate + "' and  isnull(f.Iscanceled,0) ='1' ";

                if (chlnNo != "")
                {
                    selectQuery += " and TransCode = '" + chlnCode + "' ";
                }

                selectQuery += " GROUP BY TransCode,TransDate,App_No,Iscanceled,VenContactName,f.canceledRcptRemarks order by TransDate desc";
            }
            else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 4)
            {
                //For Others
                selectQuery = " select  TransCode as ChallanNo,convert(varchar(10), TransDate,103) as ChallanDate,'' app_formno,'' smart_serial_no,'' Reg_No,'' Roll_Admit,'' Roll_No,App_No,VENDORNAME as Stud_Name,'' Degree,SUM(debit) as TakenAmt, isnull(Iscanceled,0) as Iscanceled,f.canceledRcptRemarks   from FT_FinDailyTransaction f,CO_VendorMaster v where f.App_No =v.VendorPK and f.MemType ='4'  and isnull(Transtype,0)=1 and Transcode<>'' and Paymode in(" + paymode + ")  and TransDate between '" + fromdate + "' and '" + todate + "' and  isnull(f.Iscanceled,0) ='1' ";

                if (chlnNo != "")
                {
                    selectQuery += " and TransCode = '" + chlnCode + "' ";
                }
                selectQuery += " GROUP BY TransCode,TransDate,App_No,VENDORNAME,Iscanceled,f.canceledRcptRemarks order by TransDate desc ";
            }
            else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 5)
            {
                #region both


                selectQuery = " SELECT TransCode as ChallanNo,convert(varchar(10), TransDate,103) as ChallanDate,app_formno,r.smart_serial_no,R.Reg_No,R.Roll_Admit,R.Roll_No,R.App_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,SUM(debit) as TakenAmt, isnull(c.Iscanceled,0) as Iscanceled,c.canceledRcptRemarks  FROM FT_FinDailyTransaction C,applyn A,Registration R,Degree G,Course U,Department D WHERE C.App_No = A.app_no AND a.app_no = r.App_No and A.degree_code = G.Degree_Code AND G.Course_Id = u.Course_Id and g.college_code = u.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and isnull(c.Transtype,0)=1 and c.Transcode<>'' and c.Paymode in(" + paymode + ")  and TransDate between '" + fromdate + "' and '" + todate + "'   and r.college_code=" + collegecode1 + " and  isnull(c.Iscanceled,0) ='1' ";

                if (chlnNo != "")
                {
                    selectQuery += " and TransCode = '" + chlnCode + "' ";
                }
                else
                {
                    if (rollno != "")
                    {
                        if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                        {
                            selectQuery += " and r.roll_No = '" + rollno + "' ";
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                        {
                            selectQuery += " and r.reg_no = '" + rollno + "' ";
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                        {
                            selectQuery += " and r.roll_admit = '" + rollno + "' ";
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                        {
                            selectQuery += " and r.smart_serial_no = '" + rollno + "' ";
                        }
                        else
                        {
                            selectQuery += " and a.app_formno = '" + rollno + "'  and a.college_code=" + collegecode1 + "";
                        }
                    }

                    if (hdrid.Length > 0)
                    {
                        selectQuery += " and  C.headerfk in(" + hdrid + " ) ";
                    }
                    if (btch.Length > 0)
                    {
                        selectQuery += " and a.batch_year in(" + btch + " ) ";
                    }
                    if (dept.Length > 0)
                    {
                        selectQuery += " and a.degree_code in(" + dept + " ) ";
                    }
                }

                selectQuery += " GROUP BY TransCode,TransDate,app_formno,R.Stud_Name, Course_Name, Dept_Name, R.Reg_No, R.Roll_Admit, R.Roll_No,R.App_No,c.Iscanceled,r.smart_serial_no,c.canceledRcptRemarks order by TransDate desc";


                selectQuery += " select TransCode as ChallanNo,convert(varchar(10), TransDate,103) as ChallanDate,s.staff_code as app_formno,s.staff_code as smart_serial_no,s.staff_code as Reg_No,s.staff_code as Roll_Admit,'' Roll_No,App_No,s.staff_name as Stud_Name,h.dept_name as  Degree,SUM(debit) as TakenAmt, isnull(Iscanceled,0) as Iscanceled,f.canceledRcptRemarks  from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,stafftrans t,hrdept_master h,desig_master d where sa.appl_id =f.App_No and s.staff_code =t.staff_code and t.desig_code =d.desig_code and h.dept_code =t.dept_code and latestrec ='1' and s.college_code =d.collegeCode and h.college_code =s.college_code and sa.appl_no =s.appl_no  and f.MemType ='2'  and isnull(Transtype,0)=1 and Transcode<>'' and f.Paymode in(" + paymode + ")  and TransDate between '" + fromdate + "' and '" + todate + "' and  isnull(f.Iscanceled,0) ='1'   ";
                if (chlnNo != "")
                {
                    selectQuery += " and TransCode = '" + chlnCode + "' ";
                }

                selectQuery += " GROUP BY TransCode,TransDate,s.staff_code,  h.Dept_Name, App_No,s.staff_name,Iscanceled,f.canceledRcptRemarks order by TransDate desc";

                selectQuery += " select TransCode as ChallanNo,convert(varchar(10), TransDate,103) as ChallanDate,'' app_formno,'' smart_serial_no,'' Reg_No,'' Roll_Admit,'' Roll_No,App_No, VenContactName as Stud_Name,'' Degree,SUM(debit) as TakenAmt, isnull(Iscanceled,0) as Iscanceled,f.canceledRcptRemarks from FT_FinDailyTransaction f,CO_VendorMaster v,IM_VendorContactMaster vc where f.App_No =vc.VendorContactPK and v.VendorPK =vc.VendorFK and f.MemType ='3'  and isnull(Transtype,0)=1 and Transcode<>'' and Paymode in(" + paymode + ")  and TransDate between '" + fromdate + "' and '" + todate + "' and  isnull(f.Iscanceled,0) ='1' ";

                if (chlnNo != "")
                {
                    selectQuery += " and TransCode = '" + chlnCode + "' ";
                }

                selectQuery += " GROUP BY TransCode,TransDate,App_No,Iscanceled,VenContactName,f.canceledRcptRemarks order by TransDate desc";


                selectQuery += " select  TransCode as ChallanNo,convert(varchar(10), TransDate,103) as ChallanDate,'' app_formno,'' smart_serial_no,'' Reg_No,'' Roll_Admit,'' Roll_No,App_No,VENDORNAME as Stud_Name,'' Degree,SUM(debit) as TakenAmt, isnull(Iscanceled,0) as Iscanceled,f.canceledRcptRemarks   from FT_FinDailyTransaction f,CO_VendorMaster v where f.App_No =v.VendorPK and f.MemType ='4'  and isnull(Transtype,0)=1 and Transcode<>'' and Paymode in(" + paymode + ")  and TransDate between '" + fromdate + "' and '" + todate + "' and  isnull(f.Iscanceled,0) ='1' ";

                if (chlnNo != "")
                {
                    selectQuery += " and TransCode = '" + chlnCode + "' ";
                }
                selectQuery += " GROUP BY TransCode,TransDate,App_No,VENDORNAME,Iscanceled,f.canceledRcptRemarks order by TransDate desc ";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQuery, "Text");

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 9;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread1.Columns[0].Width = 50;
                FpSpread1.Columns[1].Width = 100;
                FpSpread1.Columns[2].Width = 100;
                FpSpread1.Columns[3].Width = 100;
                FpSpread1.Columns[4].Width = 130;
                FpSpread1.Columns[5].Width = 150;
                FpSpread1.Columns[6].Width = 150;
                FpSpread1.Columns[7].Width = 80;
                FpSpread1.Columns[8].Width = 150;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Receipt No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Receipt Date";

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reference No";//Convert.ToString(rbl_rollno.SelectedItem.Text);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Department";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Reason";

                if (Convert.ToByte(rbl_Memtype.SelectedValue) == 4)
                {
                    FpSpread1.Sheets[0].Columns[4].Visible = false;
                    FpSpread1.Sheets[0].Columns[6].Visible = false;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Name";
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[4].Visible = true;
                    FpSpread1.Sheets[0].Columns[6].Visible = true;
                }

                if (Convert.ToByte(rbl_Memtype.SelectedValue) == 2)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Staff Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Staff Name";
                }
                else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 3)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Vendor Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Vendor Name";
                }
                FpSpread1.Sheets[0].Columns[1].Visible = false;

                for (int j = 0; j < FpSpread1.Sheets[0].Columns.Count; j++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[j].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[j].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[j].Font.Name = "Arial";
                    FpSpread1.Sheets[0].ColumnHeader.Columns[j].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[j].Font.Bold = true;

                }
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].RowCount++;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Ivory;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.HotPink;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "STUDENT";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 9);
                    both(0);
                    rptprint.Visible = true;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Ivory;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.HotPink;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "STAFF";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 9);
                    both(1);
                    rptprint.Visible = true;
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Ivory;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.HotPink;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "VENDOR";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 9);
                    both(2);
                    rptprint.Visible = true;
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Ivory;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.HotPink;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "OTHERS";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 2, 1, 9);
                    both(3);
                    rptprint.Visible = true;
                }
                FpSpread1.Width = 790;
                if (FpSpread1.Sheets[0].RowCount < 1)
                {
                    FpSpread1.Visible = false;
                    //Divspread.Visible = false;
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    tblBtns.Visible = false; rptprint.Visible = false;
                    imgAlert.Visible = true;
                    if (chlnNo != "")
                    {
                        lbl_alert.Text = "Invalid Receipt Number";
                    }
                    else
                    {
                        lbl_alert.Text = "Please Generate Receipt To Process";
                    }
                }
                return;
                #endregion
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQuery, "Text");

            #endregion

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                #region spread load
                RollAndRegSettings();
                //Divspread.Visible = true;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 11;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                chk.AutoPostBack = false;

                FpSpread1.Columns[0].Width = 50;
                FpSpread1.Columns[1].Width = 100;
                FpSpread1.Columns[2].Width = 100;
                FpSpread1.Columns[3].Width = 100;
                FpSpread1.Columns[4].Width = 130;
                FpSpread1.Columns[5].Width = 130;
                FpSpread1.Columns[6].Width = 130;
                FpSpread1.Columns[7].Width = 150;
                FpSpread1.Columns[8].Width = 150;
                FpSpread1.Columns[9].Width = 80;
                FpSpread1.Columns[10].Width = 150;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Receipt No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Receipt Date";

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Admission No";

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Department";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Total";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Reason";

                if (Convert.ToByte(rbl_Memtype.SelectedValue) == 4)
                {
                    FpSpread1.Sheets[0].Columns[4].Visible = false;
                    FpSpread1.Sheets[0].Columns[8].Visible = false;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Name";
                }
                else
                {
                    FpSpread1.Sheets[0].Columns[4].Visible = true;
                    FpSpread1.Sheets[0].Columns[8].Visible = true;
                }

                if (Convert.ToByte(rbl_Memtype.SelectedValue) == 2)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Staff Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Staff Name";
                }
                else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 3)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Vendor Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Vendor Name";
                }


                for (int j = 0; j < FpSpread1.Sheets[0].Columns.Count; j++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Columns[j].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[j].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[j].Font.Name = "Arial";
                    FpSpread1.Sheets[0].ColumnHeader.Columns[j].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[j].Font.Bold = true;

                }
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].RowCount++;
                FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                check.AutoPostBack = true;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                FpSpread1.Sheets[0].Cells[0, 1].CellType = check;
                for (int i = 2; i < FpSpread1.Sheets[0].Columns.Count; i++)
                {
                    FpSpread1.Sheets[0].Columns[i].Locked = true;
                }
                spreadColumnVisible();
                FarPoint.Web.Spread.TextCellType txtReceiptno = new FarPoint.Web.Spread.TextCellType();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chk;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txtReceiptno;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["ChallanNo"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["ChallanDate"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt;
                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["roll_No"]);
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["reg_no"]);
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_Admit"]);
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["smart_serial_no"]);
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["app_formno"]);
                    }

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Arial";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["roll_admit"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Arial";


                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Arial";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["Degree"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Arial";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["TakenAmt"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Arial";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["canceledRcptRemarks"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Arial";
                }

                #endregion
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true; rptprint.Visible = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].Rows.Count;
                tblBtns.Visible = false;
            }
            else
            {
                FpSpread1.Visible = false;
                //Divspread.Visible = false;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                rptprint.Visible = false;
                tblBtns.Visible = false;
                imgAlert.Visible = true;
                if (chlnNo != "")
                {
                    lbl_alert.Text = "Invalid Receipt Number";
                }
                else
                {
                    lbl_alert.Text = "Please Generate Receipt To Process";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Cancel Receipt Report";
            string pagename = "CancelReceiptReport.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }
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
    public void both(int tablecount)
    {
        #region spread load
        //Divspread.Visible = true;
        FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
        check.AutoPostBack = true;
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        chk.AutoPostBack = false;
        FpSpread1.Sheets[0].Cells[0, 1].CellType = check;
        for (int i = 2; i < FpSpread1.Sheets[0].Columns.Count; i++)
        {
            FpSpread1.Sheets[0].Columns[i].Locked = true;
        }
        FarPoint.Web.Spread.TextCellType txtReceiptno = new FarPoint.Web.Spread.TextCellType();
        for (int i = 0; i < ds.Tables[tablecount].Rows.Count; i++)
        {
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Arial";

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chk;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Value = 0;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txtReceiptno;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[tablecount].Rows[i]["ChallanNo"]);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Arial";

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[tablecount].Rows[i]["ChallanDate"]);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Arial";

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt;
            if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
            {
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[tablecount].Rows[i]["roll_No"]);
            }
            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
            {
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[tablecount].Rows[i]["reg_no"]);
            }
            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
            {
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[tablecount].Rows[i]["Roll_Admit"]);
            }
            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
            {
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[tablecount].Rows[i]["smart_serial_no"]);
            }
            else
            {
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[tablecount].Rows[i]["app_formno"]);
            }

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[tablecount].Rows[i]["app_no"]);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Arial";

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[tablecount].Rows[i]["Stud_Name"]);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Arial";

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[tablecount].Rows[i]["Degree"]);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Arial";

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[tablecount].Rows[i]["TakenAmt"]);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Arial";

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[tablecount].Rows[i]["canceledRcptRemarks"]);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Arial";
        }
        //for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
        //{
        //    for (int k = 0; k < FpSpread1.Columns.Count; k++)
        //    {
        //        FpSpread1.Sheets[0].Cells[i, k].BackColor = ColorTranslator.FromHtml("#FF7777");
        //    }
        //}
        #endregion
        FpSpread1.SaveChanges();
        FpSpread1.Visible = true;
        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].Rows.Count;
        tblBtns.Visible = false;
    }
    protected void rbl_Memtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_name.Text = string.Empty;
        txt_regno.Text = string.Empty;
        txt_name.Enabled = false;
        txt_regno.Enabled = false;
        txt_batch.Enabled = false;
        txt_degree.Enabled = false;
        txt_dept.Enabled = false;
        txt_header.Enabled = false;
        rbl_rollno.Enabled = false;
        // ddlBefAfteAdm.Enabled = false;

        txt_chno.Text = string.Empty;
        txt_fromdate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
        txt_todate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
        // txt_date.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");

        if (rbl_Memtype.SelectedIndex == 0)
        {
            txt_name.Enabled = true;
            txt_regno.Enabled = true;
            txt_batch.Enabled = true;
            txt_degree.Enabled = true;
            txt_dept.Enabled = true;
            txt_header.Enabled = true;
            rbl_rollno.Enabled = true;
            //ddlBefAfteAdm.Enabled = true;
        }

        btn_go_Click(sender, e);
        imgAlert.Visible = false;
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

    }
    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }
    protected void btnChlnCancel_Click(object sender, EventArgs e)
    {
        if (checkedOK())
        {
            surediv.Visible = true;
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Receipt";
        }

    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        try
        {
            string alertmsg = "";
            surediv.Visible = false;
            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

            string ScholarTypeQ = "select LinkValue from New_InsSettings where LinkName='ScholarshipType' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            byte ScholarTypeValue = 0;
            byte.TryParse(Convert.ToString(d2.GetFunction(ScholarTypeQ)), out ScholarTypeValue);

            for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
            {
                byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                if (check == 1)
                {
                    string chlnNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    string chlnDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);
                    //string AppFormNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                    string studname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Text);
                    string dept = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                    string total = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);

                    string trasdate = txt_date.Text.Trim();
                    DateTime canceldate = Convert.ToDateTime(txt_date.Text.Split('/')[1] + "/" + txt_date.Text.Split('/')[0] + "/" + txt_date.Text.Split('/')[2]);
                    string transtime = DateTime.Now.ToLongTimeString();
                    string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);


                    //string confirmChk = d2.GetFunction(" select ChallanNo from FT_ChallanDet WHERE ChallanNo = '" + chlnNo.Trim() + "' AND App_No = " + AppNo + " and IsConfirmed = '0'");
                    string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + chlnNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                    if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                    {
                        byte excessType = 0;
                        string isUsedExcess = d2.GetFunction("select excesstype from ft_excessreceiptdet where receiptno='" + chlnNo.Trim() + "'").Trim();
                        byte.TryParse(isUsedExcess, out excessType);

                        double adjAmt = 0;
                        double.TryParse(d2.GetFunction("select adjamt from ft_excessdet where app_no='" + AppNo + "'").Trim(), out adjAmt);
                        byte.TryParse(isUsedExcess, out excessType);

                        if (excessType != 1 || (!(adjAmt > 0) && excessType == 1))
                        {
                            //string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,isnull(TakenAmt,0) as TakenAmt  from FT_ChallanDet where challanNo='" + chlnNo + "'";
                            string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,isnull(debit,0) as TakenAmt,DailyTransPk from FT_FinDailyTransaction where TransCode='" + chlnNo + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0";
                            DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                            if (dsDet.Tables.Count > 0)
                            {
                                if (dsDet.Tables[0].Rows.Count > 0)
                                {
                                    #region cancel excess amount

                                    double usedExcess = 0;
                                    double.TryParse(d2.GetFunction("select sum(isnull(amount,0)) from ft_excessreceiptdet where receiptno='" + chlnNo.Trim() + "'"), out usedExcess);
                                    if (usedExcess > 0)
                                    {
                                        if (excessType != 1)
                                        {
                                            string updExcessDet = "update ft_excessdet set AdjAmt=Adjamt-" + usedExcess + ",BalanceAmt=BalanceAmt+" + usedExcess + " where app_no='" + AppNo + "'";
                                            d2.update_method_wo_parameter(updExcessDet, "Text");
                                        }
                                        else
                                        {
                                            string updExcessDet = "update ft_excessdet set ExcessAmt=ExcessAmt-" + usedExcess + ",BalanceAmt=BalanceAmt-" + usedExcess + " where app_no='" + AppNo + "'";
                                            d2.update_method_wo_parameter(updExcessDet, "Text");
                                        }

                                        string ledExcQ = "select isnull(el.AdjAmt,0) as AdjAmt,isnull(el.excessamt,0) as excessamt,el.HeaderFk,el.LedgerFk,el.Feecategory,el.FinYearFk,el.ExcessDetFk from FT_ExcessLedgerDet el,FT_ExcessDet e where  ExcessDetFk=ExcessDetpk and app_no='" + AppNo + "'";
                                        DataSet dsExLedge = new DataSet();
                                        dsExLedge = d2.select_method_wo_parameter(ledExcQ, "Text");
                                        if (dsExLedge.Tables.Count > 0)
                                        {
                                            for (int exc = 0; exc < dsExLedge.Tables[0].Rows.Count; exc++)
                                            {
                                                double curAdj = 0;
                                                string curHdr = Convert.ToString(dsExLedge.Tables[0].Rows[exc]["HeaderFk"]);
                                                string curlgr = Convert.ToString(dsExLedge.Tables[0].Rows[exc]["LedgerFk"]);
                                                string excessFeecat = Convert.ToString(dsExLedge.Tables[0].Rows[exc]["Feecategory"]);
                                                string excessFin = Convert.ToString(dsExLedge.Tables[0].Rows[exc]["FinYearFk"]);
                                                string excessPk = Convert.ToString(dsExLedge.Tables[0].Rows[exc]["ExcessDetFk"]);
                                                if (excessType != 1)
                                                {
                                                    double.TryParse(Convert.ToString(dsExLedge.Tables[0].Rows[exc]["AdjAmt"]), out curAdj);
                                                }
                                                else
                                                {
                                                    double.TryParse(Convert.ToString(dsExLedge.Tables[0].Rows[exc]["ExcessAmt"]), out curAdj);
                                                }
                                                double curAdjamt = 0;
                                                if (usedExcess <= curAdj)
                                                {
                                                    curAdjamt = usedExcess;
                                                    usedExcess = 0;
                                                }
                                                else
                                                {
                                                    curAdjamt = curAdj;
                                                    usedExcess -= curAdjamt;
                                                }
                                                if (curAdjamt > 0)
                                                {
                                                    if (excessType != 1)
                                                    {
                                                        string ledUpQ = "update FT_ExcessLedgerDet set AdjAmt=AdjAmt-" + curAdjamt + ", BalanceAmt=BalanceAmt+" + curAdjamt + " where HeaderFK ='" + curHdr + "' and LedgerFK ='" + curlgr + "' and Feecategory=" + excessFeecat + " and Finyearfk=" + excessFin + " and excessdetfk=" + excessPk + "";
                                                        d2.update_method_wo_parameter(ledUpQ, "Text");
                                                    }
                                                    else
                                                    {
                                                        string ledUpQ = "update FT_ExcessLedgerDet set ExcessAmt=ExcessAmt-" + curAdjamt + ", BalanceAmt=BalanceAmt-" + curAdjamt + " where HeaderFK ='" + curHdr + "' and LedgerFK ='" + curlgr + "' and Feecategory=" + excessFeecat + " and Finyearfk=" + excessFin + " and excessdetfk=" + excessPk + "";
                                                        d2.update_method_wo_parameter(ledUpQ, "Text");
                                                    }
                                                }
                                            }
                                        }
                                        string delQ = "delete from ft_excessreceiptdet where receiptno='" + chlnNo.Trim() + "'";
                                        d2.update_method_wo_parameter(delQ, "Text");
                                    }

                                    #endregion

                                    #region Bounce Cheque Or DD
                                    string deStr = d2.GetFunction("select isnull(isdeposited,0) as Deposited from ft_finbanktransaction where dailytransId = '" + chlnNo + "' ");
                                    if (deStr.Trim() != string.Empty)
                                    {
                                        if (deStr.Trim().ToUpper() == "TRUE" || deStr.Trim().ToUpper() == "1")
                                        {
                                            d2.update_method_wo_parameter(" update ft_finbanktransaction set IsBounced = '1'  where  dailytransId = '" + chlnNo + "' ", "Text");
                                            string[] bnDa = txt_date.Text.Trim().Split('/');
                                            d2.update_method_wo_parameter(" update FT_FinDailyTransaction set IsBounced = '1',BouncedDate='" + (bnDa[1] + "/" + bnDa[0] + "/" + bnDa[2]) + "'  where  transcode = '" + chlnNo + "' ", "Text");
                                        }
                                    }
                                    #endregion

                                    for (int n = 0; n < dsDet.Tables[0].Rows.Count; n++)
                                    {
                                        string ledger = Convert.ToString(dsDet.Tables[0].Rows[n]["LedgerFK"]);
                                        string header = Convert.ToString(dsDet.Tables[0].Rows[n]["HeaderFk"]);
                                        string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[n]["FeeCategory"]);
                                        string creditamt = Convert.ToString(dsDet.Tables[0].Rows[n]["TakenAmt"]);
                                        string DailyTransPk = Convert.ToString(dsDet.Tables[0].Rows[n]["DailyTransPk"]);

                                        string upTrans = "UPDATE FT_FinDailyTransaction SET IsCanceled =1,CancelledDate = '" + canceldate.Date + "',CancelUserCode = 30 WHERE TransCode = '" + chlnNo + "' AND App_No = " + AppNo + " AND FeeCategory = " + FeeCategory + " and HeaderFk=" + header + " and LedgerFk=" + ledger + " and DailyTransPK=" + DailyTransPk + "";

                                        #region Monthwise cancel

                                        string selFeeAllotPkQ = "select FeeAllotPk from ft_feeallot  where FeeCategory ='" + FeeCategory + "' and HeaderFK ='" + header + "' and LedgerFK ='" + ledger + "' and App_No='" + AppNo + "' ";
                                        Int64 FeeAllotPk = 0;
                                        Int64.TryParse(d2.GetFunction(selFeeAllotPkQ).Trim(), out FeeAllotPk);

                                        DataSet dsMonWiseDet = new DataSet();
                                        string monWiseDetQ = "select Monthvalue,Yearvalue,Debit from FT_FinDailyTransactionDetailMonthWise where Dailytransfk=" + DailyTransPk + " and isCancel='0'";
                                        dsMonWiseDet = d2.select_method_wo_parameter(monWiseDetQ, "Text");
                                        if (dsMonWiseDet.Tables.Count > 0 && dsMonWiseDet.Tables[0].Rows.Count > 0)
                                        {
                                            int monWisemon = 0;
                                            int monWiseyea = 0;
                                            double debAmt = 0;
                                            int.TryParse(Convert.ToString(dsMonWiseDet.Tables[0].Rows[0]["Monthvalue"]), out monWisemon);
                                            int.TryParse(Convert.ToString(dsMonWiseDet.Tables[0].Rows[0]["Yearvalue"]), out monWiseyea);
                                            double.TryParse(Convert.ToString(dsMonWiseDet.Tables[0].Rows[0]["Debit"]), out debAmt);
                                            if (monWisemon > 0 && monWiseyea > 0)
                                            {
                                                string upDailyTransMonwiseQ = "update FT_FinDailyTransactionDetailMonthWise set isCancel='1' where DailyTransFK=" + DailyTransPk + "";

                                                string upFeeAllotMonwiseQ = "update FT_FeeallotMonthly set  paidamount=ISNULL(paidamount,0)-" + debAmt + ",BalAmount=ISNULL(BalAmount,0)+" + debAmt + " where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseyea + "";

                                                d2.update_method_wo_parameter(upDailyTransMonwiseQ, "Text");
                                                d2.update_method_wo_parameter(upFeeAllotMonwiseQ, "Text");
                                            }
                                        }

                                        #endregion


                                        string updateCHlTkn = " UPDATE FT_FeeAllot SET PaidStatus = 0,PaidAmount = PaidAmount - " + creditamt + ",BalAmount = BalAmount + " + creditamt + "  where FeeCategory ='" + FeeCategory + "' and HeaderFK ='" + header + "' and LedgerFK ='" + ledger + "' and App_No='" + AppNo + "' ";

                                        #region Scholaship Cancel
                                        DataSet dtSchlAmt = new DataSet();
                                        string strinSchlQ = "select ISNULL(Adjusamount,0) as Amt,LedgerFk,HeaderFk,Feecategory,Reasoncode from FT_FinScholarshipAdjusDet  WHERE  App_No=" + AppNo + " and collegecode=" + collegecode1 + " and Feecategory=" + FeeCategory + " and Ledgerfk=" + ledger + " and headerfk=" + header + " and transcode='" + chlnNo.Trim() + "' and Isnull(iscancelled,0)=0 ";
                                        dtSchlAmt = d2.select_method_wo_parameter(strinSchlQ, "Text");
                                        if (dtSchlAmt.Tables.Count > 0 && dtSchlAmt.Tables[0].Rows.Count > 0)
                                        {
                                            for (int rea = 0; rea < dtSchlAmt.Tables[0].Rows.Count; rea++)
                                            {
                                                string reasoncode = Convert.ToString(dtSchlAmt.Tables[0].Rows[rea]["Reasoncode"]);
                                                double amt = 0;
                                                double.TryParse(Convert.ToString(dtSchlAmt.Tables[0].Rows[rea]["Amt"]), out amt);
                                                if (amt > 0)
                                                {
                                                    if (ScholarTypeValue == 1)
                                                    {
                                                        //Ledgerwise
                                                        string updateGovt = " UPDATE FT_FinScholarship SET AdjusAmount=isnull(AdjusAmount,0.00)-" + amt + " WHERE  App_No=" + AppNo + " and ledgerfk=" + ledger + " and Headerfk=" + header + " and FeeCategory=" + FeeCategory + " and collegecode=" + collegecode1 + " and Reasoncode=" + reasoncode + "";
                                                        d2.update_method_wo_parameter(updateGovt, "Text");

                                                        string ledUpdQ = "UPDATE FT_FinScholarshipAdjusDet SET IsCancelled='1',Adjusdate='" + canceldate.Date + "'  WHERE  App_No=" + AppNo + " and collegecode=" + collegecode1 + " and Feecategory=" + FeeCategory + " and Ledgerfk=" + ledger + " and headerfk=" + header + " and transcode='" + chlnNo.Trim() + "' and Isnull(iscancelled,0)=0 and Reasoncode=" + reasoncode + "";
                                                        d2.update_method_wo_parameter(ledUpdQ, "Text");
                                                    }
                                                    else
                                                    {
                                                        //common
                                                        string ledUpdQ = "UPDATE FT_FinScholarshipAdjusDet SET IsCancelled='1',Adjusdate='" + canceldate.Date + "'  WHERE  App_No=" + AppNo + " and collegecode=" + collegecode1 + " and Feecategory=" + FeeCategory + " and Ledgerfk=" + ledger + " and headerfk=" + header + " and transcode='" + chlnNo.Trim() + "' and Isnull(iscancelled,0)=0 and Reasoncode=" + reasoncode + "";
                                                        d2.update_method_wo_parameter(ledUpdQ, "Text");

                                                        string selDistLedge = " select isnull(adjusamount,0) as adjamt,Feecategory,Reasoncode, Headerfk,Ledgerfk from FT_FinScholarship where App_no=" + AppNo + " and collegecode=" + collegecode1 + " and isnull(adjusamount,0)>0";
                                                        DataSet dsDistLedge = new DataSet();
                                                        dsDistLedge = d2.select_method_wo_parameter(selDistLedge, "Text");
                                                        if (dsDistLedge.Tables.Count > 0 && dsDistLedge.Tables[0].Rows.Count > 0)
                                                        {
                                                            for (int dsled = 0; dsled < dsDistLedge.Tables[0].Rows.Count; dsled++)
                                                            {
                                                                double upamt = 0;
                                                                double curAmt = 0;
                                                                double.TryParse(Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["adjamt"]), out curAmt);
                                                                string ledg = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Ledgerfk"]);
                                                                string heade = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Headerfk"]);
                                                                string feec = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Feecategory"]);
                                                                string reasc = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Reasoncode"]);
                                                                if (amt <= curAmt)
                                                                {
                                                                    upamt = amt;
                                                                    amt = 0;
                                                                }
                                                                else
                                                                {
                                                                    upamt = curAmt;
                                                                    amt -= curAmt;
                                                                }
                                                                if (upamt > 0)
                                                                {
                                                                    string updateGovt = " UPDATE FT_FinScholarship SET AdjusAmount=isnull(AdjusAmount,0.00)-" + upamt + " WHERE  App_No=" + AppNo + " and ledgerfk=" + ledg + " and Headerfk=" + heade + " and FeeCategory=" + feec + " and collegecode=" + collegecode1 + " and Reasoncode=" + reasc + "";
                                                                    d2.update_method_wo_parameter(updateGovt, "Text");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region update CashTransaction

                                        string upCashTrans = "  if exists (select * from FT_FinCashTransaction where TransDate ='" + canceldate.Date + "' and FinYearFK ='" + finYearid + "') update FT_FinCashTransaction set TransTime ='" + DateTime.Now.ToLongTimeString() + "', Debit =isnull(Debit,0) -" + creditamt + " where FinYearFK ='" + finYearid + "' and TransDate ='" + canceldate.Date + "' ";

                                        #endregion

                                        int up2OK = d2.update_method_wo_parameter(updateCHlTkn, "Text");

                                        int up1OK = d2.update_method_wo_parameter(upTrans, "Text");

                                        int up3OK = d2.update_method_wo_parameter(upCashTrans, "Text");

                                        imgAlert.Visible = true;

                                        if (up1OK > 0 && up2OK > 0 && up3OK > 0)
                                        {
                                            alertmsg = "Cancelled Sucessfully";
                                        }
                                        else
                                        {
                                            alertmsg = "Please Cancel The Challan To Cancel";
                                        }
                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    alertmsg = "Not Ledgers Found";
                                }
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                alertmsg = "Not Ledgers Found";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            alertmsg = "Cannot Delete Until Used Excess Receipt Cancelled";
                        }
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        alertmsg = "Already Cancelled";
                    }

                }
            }
            btn_go_Click(sender, e);
            lbl_alert.Text = alertmsg;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate");
            imgAlert.Visible = true;
            lbl_alert.Text = "Not Deleted";
        }
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
    }
    protected void btnChlnDelete_Click(object sender, EventArgs e)
    {
        if (checkedOK())
        {
            suredivDelete.Visible = true;
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Receipt";
        }

    }
    protected void btn_sureyesDel_Click(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        try
        {
            string alertmsg = "";
            suredivDelete.Visible = false;
            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

            string ScholarTypeQ = "select LinkValue from New_InsSettings where LinkName='ScholarshipType' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            byte ScholarTypeValue = 0;
            byte.TryParse(Convert.ToString(d2.GetFunction(ScholarTypeQ)), out ScholarTypeValue);

            for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
            {
                byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                if (check == 1)
                {
                    string chlnNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    string chlnDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);
                    //string AppFormNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                    string studname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Text);
                    string dept = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                    string total = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);

                    string trasdate = txt_date.Text.Trim();
                    DateTime canceldate = Convert.ToDateTime(txt_date.Text.Split('/')[1] + "/" + txt_date.Text.Split('/')[0] + "/" + txt_date.Text.Split('/')[2]);
                    string transtime = DateTime.Now.ToLongTimeString();
                    string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);


                    //string confirmChk = d2.GetFunction(" select ChallanNo from FT_ChallanDet WHERE ChallanNo = '" + chlnNo.Trim() + "' AND App_No = " + AppNo + " and IsConfirmed = '0'");
                    string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + chlnNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                    if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                    {
                        byte excessType = 0;
                        string isUsedExcess = d2.GetFunction("select excesstype from ft_excessreceiptdet where receiptno='" + chlnNo.Trim() + "'").Trim();
                        byte.TryParse(isUsedExcess, out excessType);

                        double adjAmt = 0;
                        double.TryParse(d2.GetFunction("select adjamt from ft_excessdet where app_no='" + AppNo + "'").Trim(), out adjAmt);
                        byte.TryParse(isUsedExcess, out excessType);

                        if (excessType != 1 || (!(adjAmt > 0) && excessType == 1))
                        {
                            //string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,isnull(TakenAmt,0) as TakenAmt  from FT_ChallanDet where challanNo='" + chlnNo + "'";
                            string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,isnull(debit,0) as TakenAmt,DailyTransPk from FT_FinDailyTransaction where TransCode='" + chlnNo + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0";
                            DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                            if (dsDet.Tables.Count > 0)
                            {
                                if (dsDet.Tables[0].Rows.Count > 0)
                                {
                                    #region cancel excess amount

                                    double usedExcess = 0;
                                    double.TryParse(d2.GetFunction("select sum(isnull(amount,0)) from ft_excessreceiptdet where receiptno='" + chlnNo.Trim() + "'"), out usedExcess);
                                    if (usedExcess > 0)
                                    {
                                        if (excessType != 1)
                                        {
                                            string updExcessDet = "update ft_excessdet set AdjAmt=Adjamt-" + usedExcess + ",BalanceAmt=BalanceAmt+" + usedExcess + " where app_no='" + AppNo + "'";
                                            d2.update_method_wo_parameter(updExcessDet, "Text");
                                        }
                                        else
                                        {
                                            string updExcessDet = "update ft_excessdet set ExcessAmt=ExcessAmt-" + usedExcess + ",BalanceAmt=BalanceAmt-" + usedExcess + " where app_no='" + AppNo + "'";
                                            d2.update_method_wo_parameter(updExcessDet, "Text");
                                        }

                                        string ledExcQ = "select isnull(el.AdjAmt,0) as AdjAmt,isnull(el.excessamt,0) as excessamt,el.HeaderFk,el.LedgerFk,el.Feecategory,el.FinYearFk,el.ExcessDetFk from FT_ExcessLedgerDet el,FT_ExcessDet e where  ExcessDetFk=ExcessDetpk and app_no='" + AppNo + "'";
                                        DataSet dsExLedge = new DataSet();
                                        dsExLedge = d2.select_method_wo_parameter(ledExcQ, "Text");
                                        if (dsExLedge.Tables.Count > 0)
                                        {
                                            for (int exc = 0; exc < dsExLedge.Tables[0].Rows.Count; exc++)
                                            {
                                                double curAdj = 0;
                                                string curHdr = Convert.ToString(dsExLedge.Tables[0].Rows[exc]["HeaderFk"]);
                                                string curlgr = Convert.ToString(dsExLedge.Tables[0].Rows[exc]["LedgerFk"]);
                                                string excessFeecat = Convert.ToString(dsExLedge.Tables[0].Rows[exc]["Feecategory"]);
                                                string excessFin = Convert.ToString(dsExLedge.Tables[0].Rows[exc]["FinYearFk"]);
                                                string excessPk = Convert.ToString(dsExLedge.Tables[0].Rows[exc]["ExcessDetFk"]);
                                                if (excessType != 1)
                                                {
                                                    double.TryParse(Convert.ToString(dsExLedge.Tables[0].Rows[exc]["AdjAmt"]), out curAdj);
                                                }
                                                else
                                                {
                                                    double.TryParse(Convert.ToString(dsExLedge.Tables[0].Rows[exc]["ExcessAmt"]), out curAdj);
                                                }
                                                double curAdjamt = 0;
                                                if (usedExcess <= curAdj)
                                                {
                                                    curAdjamt = usedExcess;
                                                    usedExcess = 0;
                                                }
                                                else
                                                {
                                                    curAdjamt = curAdj;
                                                    usedExcess -= curAdjamt;
                                                }
                                                if (curAdjamt > 0)
                                                {
                                                    if (excessType != 1)
                                                    {
                                                        string ledUpQ = "update FT_ExcessLedgerDet set AdjAmt=AdjAmt-" + curAdjamt + ", BalanceAmt=BalanceAmt+" + curAdjamt + " where HeaderFK ='" + curHdr + "' and LedgerFK ='" + curlgr + "' and Feecategory=" + excessFeecat + " and Finyearfk=" + excessFin + " and excessdetfk=" + excessPk + "";
                                                        d2.update_method_wo_parameter(ledUpQ, "Text");
                                                    }
                                                    else
                                                    {
                                                        string ledUpQ = "update FT_ExcessLedgerDet set ExcessAmt=ExcessAmt-" + curAdjamt + ", BalanceAmt=BalanceAmt-" + curAdjamt + " where HeaderFK ='" + curHdr + "' and LedgerFK ='" + curlgr + "' and Feecategory=" + excessFeecat + " and Finyearfk=" + excessFin + " and excessdetfk=" + excessPk + "";
                                                        d2.update_method_wo_parameter(ledUpQ, "Text");
                                                    }
                                                }
                                            }
                                        }
                                        string delQ = "delete from ft_excessreceiptdet where receiptno='" + chlnNo.Trim() + "'";
                                        d2.update_method_wo_parameter(delQ, "Text");
                                    }

                                    #endregion
                                    #region Bounce Cheque Or DD
                                    string deStr = d2.GetFunction("select isnull(isdeposited,0) as Deposited from ft_finbanktransaction where dailytransId = '" + chlnNo + "' ");
                                    if (deStr.Trim() != string.Empty)
                                    {
                                        if (deStr.Trim().ToUpper() == "TRUE" || deStr.Trim().ToUpper() == "1")
                                        {
                                            d2.update_method_wo_parameter(" update ft_finbanktransaction set IsBounced = '1'  where  dailytransId = '" + chlnNo + "' ", "Text");
                                            string[] bnDa = txt_date.Text.Trim().Split('/');
                                            d2.update_method_wo_parameter(" update FT_FinDailyTransaction set IsBounced = '1',BouncedDate='" + (bnDa[1] + "/" + bnDa[0] + "/" + bnDa[2]) + "'  where  transcode = '" + chlnNo + "' ", "Text");
                                        }
                                    }
                                    #endregion
                                    for (int n = 0; n < dsDet.Tables[0].Rows.Count; n++)
                                    {
                                        string ledger = Convert.ToString(dsDet.Tables[0].Rows[n]["LedgerFK"]);
                                        string header = Convert.ToString(dsDet.Tables[0].Rows[n]["HeaderFk"]);
                                        string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[n]["FeeCategory"]);
                                        string creditamt = Convert.ToString(dsDet.Tables[0].Rows[n]["TakenAmt"]);
                                        string DailyTransPk = Convert.ToString(dsDet.Tables[0].Rows[n]["DailyTransPk"]);

                                        string upTrans = "UPDATE FT_FinDailyTransaction SET IsCanceled =2,CancelledDate = '" + canceldate.Date + "',CancelUserCode = 30 WHERE TransCode = '" + chlnNo + "' AND App_No = " + AppNo + " AND FeeCategory = " + FeeCategory + " and HeaderFk=" + header + " and LedgerFk=" + ledger + " and DailyTransPK=" + DailyTransPk + "";

                                        #region Monthwise cancel

                                        //string selFeeAllotPkQ = "select FeeAllotPk from ft_feeallot  where FeeCategory ='" + FeeCategory + "' and HeaderFK ='" + header + "' and LedgerFK ='" + ledger + "' and App_No='" + AppNo + "' ";
                                        //Int64 FeeAllotPk = 0;
                                        //Int64.TryParse(d2.GetFunction(selFeeAllotPkQ).Trim(), out FeeAllotPk);

                                        //DataSet dsMonWiseDet = new DataSet();
                                        //string monWiseDetQ = "select Monthvalue,Yearvalue,Debit from FT_FinDailyTransactionDetailMonthWise where Dailytransfk=" + DailyTransPk + " and isCancel='0'";
                                        //dsMonWiseDet = d2.select_method_wo_parameter(monWiseDetQ, "Text");
                                        //if (dsMonWiseDet.Tables.Count > 0 && dsMonWiseDet.Tables[0].Rows.Count > 0)
                                        //{
                                        //    int monWisemon = 0;
                                        //    int monWiseyea = 0;
                                        //    double debAmt = 0;
                                        //    int.TryParse(Convert.ToString(dsMonWiseDet.Tables[0].Rows[0]["Monthvalue"]), out monWisemon);
                                        //    int.TryParse(Convert.ToString(dsMonWiseDet.Tables[0].Rows[0]["Yearvalue"]), out monWiseyea);
                                        //    double.TryParse(Convert.ToString(dsMonWiseDet.Tables[0].Rows[0]["Debit"]), out debAmt);
                                        //    if (monWisemon > 0 && monWiseyea > 0)
                                        //    {
                                        //        string upDailyTransMonwiseQ = "update FT_FinDailyTransactionDetailMonthWise set isCancel='1' where DailyTransFK=" + DailyTransPk + "";

                                        //        string upFeeAllotMonwiseQ = "update FT_FeeallotMonthly set  paidamount=ISNULL(paidamount,0)-" + debAmt + ",BalAmount=ISNULL(BalAmount,0)+" + debAmt + " where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseyea + "";

                                        //        d2.update_method_wo_parameter(upDailyTransMonwiseQ, "Text");
                                        //        d2.update_method_wo_parameter(upFeeAllotMonwiseQ, "Text");
                                        //    }
                                        //}

                                        #endregion


                                        string updateCHlTkn = " UPDATE FT_FeeAllot SET PaidStatus = 0,PaidAmount = PaidAmount - " + creditamt + ",FeeAmount = FeeAmount - " + creditamt + ",TotalAmount = TotalAmount - " + creditamt + "  where FeeCategory ='" + FeeCategory + "' and HeaderFK ='" + header + "' and LedgerFK ='" + ledger + "' and App_No='" + AppNo + "' ";

                                        #region Scholaship Cancel
                                        //DataSet dtSchlAmt = new DataSet();
                                        //string strinSchlQ = "select ISNULL(Adjusamount,0) as Amt,LedgerFk,HeaderFk,Feecategory,Reasoncode from FT_FinScholarshipAdjusDet  WHERE  App_No=" + AppNo + " and collegecode=" + collegecode1 + " and Feecategory=" + FeeCategory + " and Ledgerfk=" + ledger + " and headerfk=" + header + " and transcode='" + chlnNo.Trim() + "' and Isnull(iscancelled,0)=0 ";
                                        //dtSchlAmt = d2.select_method_wo_parameter(strinSchlQ, "Text");
                                        //if (dtSchlAmt.Tables.Count > 0 && dtSchlAmt.Tables[0].Rows.Count > 0)
                                        //{
                                        //    for (int rea = 0; rea < dtSchlAmt.Tables[0].Rows.Count; rea++)
                                        //    {
                                        //        string reasoncode = Convert.ToString(dtSchlAmt.Tables[0].Rows[rea]["Reasoncode"]);
                                        //        double amt = 0;
                                        //        double.TryParse(Convert.ToString(dtSchlAmt.Tables[0].Rows[rea]["Amt"]), out amt);
                                        //        if (amt > 0)
                                        //        {
                                        //            if (ScholarTypeValue == 1)
                                        //            {
                                        //                //Ledgerwise
                                        //                string updateGovt = " UPDATE FT_FinScholarship SET AdjusAmount=isnull(AdjusAmount,0.00)-" + amt + " WHERE  App_No=" + AppNo + " and ledgerfk=" + ledger + " and Headerfk=" + header + " and FeeCategory=" + FeeCategory + " and collegecode=" + collegecode1 + " and Reasoncode=" + reasoncode + "";
                                        //                d2.update_method_wo_parameter(updateGovt, "Text");

                                        //                string ledUpdQ = "UPDATE FT_FinScholarshipAdjusDet SET IsCancelled='1',Adjusdate='" + canceldate.Date + "'  WHERE  App_No=" + AppNo + " and collegecode=" + collegecode1 + " and Feecategory=" + FeeCategory + " and Ledgerfk=" + ledger + " and headerfk=" + header + " and transcode='" + chlnNo.Trim() + "' and Isnull(iscancelled,0)=0 and Reasoncode=" + reasoncode + "";
                                        //                d2.update_method_wo_parameter(ledUpdQ, "Text");
                                        //            }
                                        //            else
                                        //            {
                                        //                //common
                                        //                string ledUpdQ = "UPDATE FT_FinScholarshipAdjusDet SET IsCancelled='1',Adjusdate='" + canceldate.Date + "'  WHERE  App_No=" + AppNo + " and collegecode=" + collegecode1 + " and Feecategory=" + FeeCategory + " and Ledgerfk=" + ledger + " and headerfk=" + header + " and transcode='" + chlnNo.Trim() + "' and Isnull(iscancelled,0)=0 and Reasoncode=" + reasoncode + "";
                                        //                d2.update_method_wo_parameter(ledUpdQ, "Text");

                                        //                string selDistLedge = " select isnull(adjusamount,0) as adjamt,Feecategory,Reasoncode, Headerfk,Ledgerfk from FT_FinScholarship where App_no=" + AppNo + " and collegecode=" + collegecode1 + " and isnull(adjusamount,0)>0";
                                        //                DataSet dsDistLedge = new DataSet();
                                        //                dsDistLedge = d2.select_method_wo_parameter(selDistLedge, "Text");
                                        //                if (dsDistLedge.Tables.Count > 0 && dsDistLedge.Tables[0].Rows.Count > 0)
                                        //                {
                                        //                    for (int dsled = 0; dsled < dsDistLedge.Tables[0].Rows.Count; dsled++)
                                        //                    {
                                        //                        double upamt = 0;
                                        //                        double curAmt = 0;
                                        //                        double.TryParse(Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["adjamt"]), out curAmt);
                                        //                        string ledg = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Ledgerfk"]);
                                        //                        string heade = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Headerfk"]);
                                        //                        string feec = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Feecategory"]);
                                        //                        string reasc = Convert.ToString(dsDistLedge.Tables[0].Rows[dsled]["Reasoncode"]);
                                        //                        if (amt <= curAmt)
                                        //                        {
                                        //                            upamt = amt;
                                        //                            amt = 0;
                                        //                        }
                                        //                        else
                                        //                        {
                                        //                            upamt = curAmt;
                                        //                            amt -= curAmt;
                                        //                        }
                                        //                        if (upamt > 0)
                                        //                        {
                                        //                            string updateGovt = " UPDATE FT_FinScholarship SET AdjusAmount=isnull(AdjusAmount,0.00)-" + upamt + " WHERE  App_No=" + AppNo + " and ledgerfk=" + ledg + " and Headerfk=" + heade + " and FeeCategory=" + feec + " and collegecode=" + collegecode1 + " and Reasoncode=" + reasc + "";
                                        //                            d2.update_method_wo_parameter(updateGovt, "Text");
                                        //                        }
                                        //                    }
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        #endregion

                                        #region update CashTransaction

                                        string upCashTrans = "  if exists (select * from FT_FinCashTransaction where TransDate ='" + canceldate.Date + "' and FinYearFK ='" + finYearid + "') update FT_FinCashTransaction set TransTime ='" + DateTime.Now.ToLongTimeString() + "', Debit =isnull(Debit,0) -" + creditamt + " where FinYearFK ='" + finYearid + "' and TransDate ='" + canceldate.Date + "' ";

                                        #endregion

                                        int up2OK = d2.update_method_wo_parameter(updateCHlTkn, "Text");

                                        int up1OK = d2.update_method_wo_parameter(upTrans, "Text");

                                        int up3OK = d2.update_method_wo_parameter(upCashTrans, "Text");

                                        imgAlert.Visible = true;

                                        if (up1OK > 0 && up2OK > 0 && up3OK > 0)
                                        {
                                            alertmsg = "Deleted Sucessfully";
                                        }
                                        else
                                        {
                                            alertmsg = "Please Cancel The Challan To Delete";
                                        }
                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    alertmsg = "Not Ledgers Found";
                                }
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                alertmsg = "Not Ledgers Found";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            alertmsg = "Cannot Delete Until Used Excess Receipt Cancelled";
                        }
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        alertmsg = "Already Deleted";
                    }

                }
            }
            btn_go_Click(sender, e);
            lbl_alert.Text = alertmsg;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate");
            imgAlert.Visible = true;
            lbl_alert.Text = "Not Deleted";
        }
    }
    protected void btn_surenoDel_Click(object sender, EventArgs e)
    {
        suredivDelete.Visible = false;
    }
    protected void btnChlnDuplicate_Click(object sender, EventArgs e)
    {
        Control ctrlid = GetPostBackControl(this.Page);
        string btnid = Convert.ToString(ctrlid.UniqueID);
        string dupReceipt = " DUPLICATE";
        if (btnid == "btnChlnOriginal")
        {
            dupReceipt = string.Empty;
        }

        try
        {
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 0)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Add Receipt Format Settings";
            }
            else
            {
                if (save1 == 1)
                {
                    //Mcc and Common
                    btnChlnDuplicate0_Click(dupReceipt);
                }
                else if (save1 == 2)
                {
                    btnChlnDuplicate1_Click();
                }
                else if (save1 == 4)
                {
                    //For Vellamal
                    btnChlnDuplicate3_Click();
                }
                else if (save1 == 5)
                {
                    //Jeppiar
                    btnChlnDuplicate4_Click(dupReceipt);
                }
                else if (save1 == 6)
                {
                    //PMC
                    btnChlnDuplicate5_Click();
                }
                else if (save1 == 10)
                {
                    //Christopher
                    btnChlnChristopher_Click(dupReceipt);
                }
                else if (save1 == 11)
                {
                    //San academy
                    btnChlnsanAcademy_Click(dupReceipt);
                }
                else
                {
                    #region Others Receipt

                    if (checkedOK())
                    {
                        FpSpread1.SaveChanges();
                        try
                        {
                            string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
                            DataSet dsPri = new DataSet();
                            dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
                            if (dsPri.Tables.Count > 0)
                            {
                                if (dsPri.Tables[0].Rows.Count > 0)
                                {
                                    string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

                                    //Document Settings
                                    bool createPDFOK = false;

                                    contentDiv.InnerHtml = "";
                                    StringBuilder sbHtml = new StringBuilder();

                                    for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                                    {
                                        sbHtml.Clear();
                                        byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                                        if (check == 1)
                                        {
                                            string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                                            string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

                                            string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                                            if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                                            {
                                                string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype,DDNo,Convert(varchar(10),DDDate,103) as DDDate,(select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as DDBankCode,DDBankBranch from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                                                DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                                                if (dsDet.Tables.Count > 0)
                                                {
                                                    if (dsDet.Tables[0].Rows.Count > 0)
                                                    {
                                                        string rollno = string.Empty;
                                                        string studname = string.Empty;
                                                        string receiptno = string.Empty;
                                                        string name = string.Empty;

                                                        string app_formno = string.Empty;
                                                        string appnoNew = string.Empty;
                                                        string Regno = string.Empty;

                                                        string batchYrSem = string.Empty;

                                                        string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                                                        string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                                                        string mode = string.Empty;
                                                        string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                                                        string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                                                        string ddNo = Convert.ToString(dsDet.Tables[0].Rows[0]["ddNo"]);
                                                        string dddate = Convert.ToString(dsDet.Tables[0].Rows[0]["DDDate"]);
                                                        string ddBank = Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankCode"]);
                                                        string ddbranch = Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankBranch"]);
                                                        switch (paymode)
                                                        {
                                                            case "1":
                                                                mode = "Cash";
                                                                break;
                                                            case "2":
                                                                mode = "Cheque";
                                                                break;
                                                            case "3":
                                                                mode = "DD";
                                                                break;
                                                            case "4":
                                                                mode = "Challan";
                                                                break;
                                                            case "5":
                                                                mode = "Online";
                                                                break;
                                                            default:
                                                                mode = "Others";
                                                                break;
                                                        }


                                                        //string queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                                        string queryRollApp = string.Empty;

                                                        if (Convert.ToByte(rbl_Memtype.SelectedValue) == 1)
                                                        {
                                                            if (ddlBefAfteAdm.SelectedIndex == 0)
                                                            {
                                                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                                            }
                                                            else
                                                            {
                                                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name  from applyn where app_no='" + AppNo + "'";
                                                            }
                                                        }
                                                        else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 2)
                                                        {
                                                            queryRollApp = "select s.staff_name  as Stud_Name,s.staff_code as app_formno,s.staff_code as Roll_No,s.staff_code as Reg_No,appl_id as app_no   from staffmaster s,staff_appl_master a where s.appl_no =a.appl_no  and appl_id ='" + AppNo + "'";
                                                        }
                                                        else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 3)
                                                        {
                                                            queryRollApp = " select VenContactName  as Stud_Name,vendorcode as app_formno,vendorcode as Roll_No,vendorcode as Reg_No,vendorcontactpk as app_no from IM_VendorContactMaster c,CO_VendorMaster v where VendorFK =vendorpk and VendorType<>-5  and vendorcontactpk='" + AppNo + "'";
                                                        }
                                                        else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 4)
                                                        {
                                                            queryRollApp = "select vendorName as Stud_Name, '' Roll_No, '' app_formno,vendorpk as app_no, '' Reg_No,vendorCode from co_vendormaster where  VendorType=-5 and vendorpk='" + AppNo + "'";
                                                        }
                                                        DataSet dsRollApp = new DataSet();
                                                        dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                                                        if (dsRollApp.Tables.Count > 0)
                                                        {
                                                            if (dsRollApp.Tables[0].Rows.Count > 0)
                                                            {
                                                                rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                                                app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                                                appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                                                Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                                                studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                                            }
                                                        }
                                                        //if (Convert.ToByte(rbl_Memtype.SelectedValue) != 1)
                                                        //{
                                                        //    appnoNew = AppNo;
                                                        //}
                                                        name = rollno + "-" + studname;
                                                        name = name.Trim('-');
                                                        string remarks = string.Empty;
                                                        //Print Region
                                                        #region Print Option For Receipt
                                                        try
                                                        {
                                                            #region Settings Input
                                                            //Header Div Values
                                                            byte collegeid = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                                                            byte address1 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd1"]);
                                                            byte address2 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd2"]);
                                                            byte address3 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd3"]);
                                                            byte city = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeDist"]);
                                                            byte state = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeState"]);

                                                            byte university = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeUniversity"]);
                                                            byte rightLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRightLogo"]);
                                                            byte leftLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsLeftLogo"]);
                                                            byte time;
                                                            if (Convert.ToBoolean(Convert.ToString(dsPri.Tables[0].Rows[0]["IsTime"])))
                                                            {
                                                                time = 1;
                                                            }
                                                            else
                                                            {
                                                                time = 0;
                                                            }
                                                            byte degACR = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsDegreeAcr"]);
                                                            byte degNam = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsDegreeName"]);
                                                            byte studnam = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudName"]);
                                                            byte year = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsYear"]);
                                                            byte semester = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSemester"]);
                                                            byte regno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRegNo"]);
                                                            byte rolno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRollNo"]);
                                                            byte admno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAdminNo"]);

                                                            byte fathername = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFatherName"]);
                                                            byte seattype = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSeatType"]);
                                                            //byte setRollAsAdmin = Convert.ToByte(dsPri.Tables[0].Rows[0]["rollas_adm"]);
                                                            byte boarding = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsBoarding"]);
                                                            byte mothername = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsMontherName"]);
                                                            string recptValid = Convert.ToString(dsPri.Tables[0].Rows[0]["ValidDate"]);


                                                            //Body Div Values
                                                            //byte showAllFees = Convert.ToByte(dsPri.Tables[0].Rows[0]["showallfee"]);
                                                            byte allotedAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAllotedAmt"]);
                                                            byte fineAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFineAmt"]);
                                                            byte balAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsBalanceAmt"]);
                                                            byte semOrYear = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSemYear"]);
                                                            byte prevPaidAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsPrevPaid"]);
                                                            byte excessAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsExcessAmt"]);
                                                            // byte totDetails = Convert.ToByte(dsPri.Tables[0].Rows[0]["Total_Details"]);
                                                            byte fineInRow = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFineinRow"]);
                                                            //byte totWTselectCol = Convert.ToByte(dsPri.Tables[0].Rows[0]["TotalSelCol"]);
                                                            byte concession = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsConcession"]);
                                                            string concessionValue = string.Empty;
                                                            if (concession != 0)
                                                            {
                                                                concessionValue = Convert.ToString(dsPri.Tables[0].Rows[0]["ConcessionName"]);
                                                            }


                                                            //Footer Div Values

                                                            byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                                                            byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                                                            byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);
                                                            byte narration = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsNarration"]);
                                                            byte deduction = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTotConcession"]);
                                                            byte forclgName = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsForCollegeName"]);
                                                            byte authSign = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAuthSign"]);
                                                            byte validDate = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsValidUpto"]);
                                                            string authSignValue = string.Empty;
                                                            if (authSign != 0)
                                                            {
                                                                authSignValue = Convert.ToString(dsPri.Tables[0].Rows[0]["AuthName"]);

                                                            }

                                                            byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                                                            // byte dispModeWTcash = Convert.ToByte(dsPri.Tables[0].Rows[0]["DisModeWithCash"]);
                                                            byte signFile = Convert.ToByte(dsPri.Tables[0].Rows[0]["cashier_sign"]);

                                                            //if (signFile != 0)
                                                            //{
                                                            //if (FileUpload1.HasFile)
                                                            //{

                                                            //}                                                    
                                                            //}


                                                            #endregion

                                                            #region Students Input
                                                            //string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL(type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.app_no='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
                                                            string colquery = "";
                                                            if (Convert.ToByte(rbl_Memtype.SelectedValue) == 1)
                                                            {
                                                                if (ddlBefAfteAdm.SelectedIndex == 0)
                                                                {
                                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                                }
                                                                else
                                                                {
                                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                                }
                                                            }
                                                            else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 2)
                                                            {
                                                                colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + "  select appl_id ,h.dept_name,h.dept_acronym,h.dept_code,s.staff_name,s.staff_code,a.father_name,t.stftype as staff_type  from staffmaster s,staff_appl_master a,hrdept_master h,stafftrans t,desig_master d where s.appl_no =a.appl_no and s.staff_code =t.staff_code and t.dept_code =h.dept_code and d.desig_code =t.desig_code and s.college_code =h.college_code and d.collegeCode =s.college_code and latestrec ='1' and appl_id ='" + appnoNew + "' and s.college_Code=" + collegecode1 + "  ";
                                                            }
                                                            else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 3)
                                                            {
                                                                colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + "  SELECT VendorContactPK, VenContactType, VenContactName, VenContactDesig, VenContactDept, VendorPhoneNo, VendorExtNo, VendorMobileNo, VendorEmail, VendorFK FROM      IM_VendorContactMaster WHERE VendorContactPK = '" + appnoNew + "' ";
                                                            }
                                                            else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 4)
                                                            {
                                                                colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " SELECT VendorCode,vendorname,VendorMobileNo,VendorAddress,VendorCity,VendorCompName,VendorType  from co_vendormaster  WHERE VendorPK = '" + appnoNew + "'";
                                                            }
                                                            string collegename = "";
                                                            string add1 = "";
                                                            string add2 = "";
                                                            string add3 = "";
                                                            string univ = "";
                                                            string deg = "";
                                                            string degAcr = "";
                                                            string cursem = "";
                                                            string batyr = "";
                                                            string seatty = "";
                                                            string board = "";
                                                            string mothe = "";
                                                            string fathe = "";
                                                            string stream = "";
                                                            double deductionamt = 0;
                                                            ds.Clear();
                                                            ds = d2.select_method_wo_parameter(colquery, "Text");
                                                            if (ds.Tables.Count > 0)
                                                            {
                                                                if (ds.Tables[0].Rows.Count > 0)
                                                                {
                                                                    collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                                                    add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                                                    add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                                                    add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                                                                    univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                                                }
                                                                if (ds.Tables[1].Rows.Count > 0)
                                                                {
                                                                    if (Convert.ToByte(rbl_Memtype.SelectedValue) == 1)
                                                                    {
                                                                        //if (degACR == 0)
                                                                        //{
                                                                        deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        degAcr = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                                        //}
                                                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                                                        board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                                        mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                                                        //sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                                                                        stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                                                    }
                                                                    else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 2)
                                                                    {
                                                                        //if (degACR == 0)
                                                                        //{
                                                                        deg = " -" + Convert.ToString(ds.Tables[1].Rows[0]["dept_name"]);
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        degAcr = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                                        //}
                                                                        //cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                                        //batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["staff_type"]);
                                                                        //board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                                        //mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["father_name"]);
                                                                        //sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                                                                    }
                                                                    else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 3)
                                                                    {
                                                                        deg = " - ";
                                                                    }
                                                                    else if (Convert.ToByte(rbl_Memtype.SelectedValue) == 4)
                                                                    {
                                                                        deg = " - ";
                                                                    }
                                                                }

                                                            }
                                                            string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                                            try
                                                            {
                                                                acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                                            }
                                                            catch { }
                                                            if (deg.Split('-').Length < 2)
                                                            {
                                                                deg = " - ";
                                                            }
                                                            #endregion

                                                            #region Receipt Narration
                                                            string modeMulti = string.Empty;
                                                            bool multiCash = false;
                                                            bool multiChk = false;
                                                            bool multiDD = false;

                                                            DataSet dtMulBnkDetails = new DataSet();
                                                            dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  else 'DD' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

                                                            string ddnar = string.Empty;
                                                            string ddnew = string.Empty;
                                                            if (dtMulBnkDetails.Tables.Count > 0)
                                                            {
                                                                int sn = 1;
                                                                for (int z = 0; z < dtMulBnkDetails.Tables[0].Rows.Count; z++)
                                                                {
                                                                    if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CASH")
                                                                    {
                                                                        multiCash = true;
                                                                        continue;
                                                                    }
                                                                    else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CHEQUE")
                                                                    {
                                                                        multiChk = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        multiDD = true;
                                                                    }

                                                                    ddnar += "\nNo : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                                    sn++;
                                                                }
                                                            }
                                                            if (narration == 1)
                                                            {
                                                                remarks = d2.GetFunction("select narration from ft_findailytransaction where TransCode='" + recptNo.Trim() + "' and app_no='" + appnoNew + "' and isnull(iscanceled,0)=0");
                                                                if (remarks.Trim() == "0" || remarks.Trim() == string.Empty)
                                                                    remarks = string.Empty;
                                                                else
                                                                {
                                                                    remarks = "\n Narration : " + remarks;
                                                                }
                                                            }

                                                            if (multiCash)
                                                            {
                                                                modeMulti += "Cash,";
                                                            }
                                                            if (multiChk)
                                                            {
                                                                modeMulti += "Cheque,";
                                                            }
                                                            if (multiDD)
                                                            {
                                                                modeMulti += "DD";
                                                            }
                                                            modeMulti = modeMulti.TrimEnd(',');
                                                            if (modeMulti != "")
                                                            {
                                                                mode = modeMulti;
                                                            }
                                                            else
                                                            {
                                                                if (paymode == "3")
                                                                {
                                                                    ddnar = "\nDDNo : " + ddNo + " Bank : " + ddBank + "\nBranch :" + ddbranch + " Date  : " + dddate;
                                                                    ddnew = "&nbsp;&nbsp;&nbsp;\nDDNo : " + ddNo + "&nbsp;&nbsp;&nbsp;Bank : " + ddBank + "&nbsp;&nbsp;&nbsp;\nBranch :" + ddbranch + "&nbsp;&nbsp;&nbsp;Date  : " + dddate;
                                                                }
                                                                else if (paymode == "2")
                                                                {
                                                                    ddnar = "\nChequeNo : " + ddNo + " Bank : " + ddBank + "\nBranch :" + ddbranch + " Date  : " + dddate;
                                                                    ddnew = "&nbsp;&nbsp;&nbsp;\nChequeNo : " + ddNo + "&nbsp;&nbsp;&nbsp;Bank : " + ddBank + "&nbsp;&nbsp;&nbsp;\nBranch :" + ddbranch + "&nbsp;&nbsp;&nbsp;Date  : " + dddate;
                                                                }
                                                                else if (paymode == "1")
                                                                {// ddnar = "\nCash"; 
                                                                }
                                                            }
                                                            // ddnew = ddnar;
                                                            ddnar += remarks;
                                                            #endregion

                                                            if (save1 == 3)
                                                            {
                                                                //For Hiet
                                                                HietChallanReceipt rcptObj = new HietChallanReceipt(AppNo, rollno, collegecode1, usercode, Convert.ToByte(rcptType), studname, recptDt, rcptTime, recptNo, cursem, deg.Split('-')[0].ToUpper(), deg.Split('-')[1].ToUpper(), ddnar);//(modeMulti + ddnar)
                                                                contentDiv.InnerHtml += ((StringBuilder)(rcptObj.returnHtmlString(out createPDFOK))).ToString();
                                                            }
                                                            else if (save1 == 7)
                                                            {
                                                                //For Gnanamani
                                                                GnanamaniChallanReceipt rcptObj = new GnanamaniChallanReceipt(AppNo, rollno, collegecode1, usercode, Convert.ToByte(rcptType), studname, recptDt, rcptTime, recptNo, cursem, deg.Split('-')[0].ToUpper(), deg.Split('-')[1].ToUpper());
                                                                contentDiv.InnerHtml += ((StringBuilder)(rcptObj.returnHtmlString(out createPDFOK))).ToString();
                                                            }
                                                            else if (save1 == 8)
                                                            {
                                                                //For VRS
                                                                VRSChallanReceipt rcptObj = new VRSChallanReceipt(AppNo, rollno, collegecode1, usercode, Convert.ToByte(rcptType), studname, recptDt, rcptTime, recptNo, cursem, deg.Split('-')[0].ToUpper(), deg.Split('-')[1].ToUpper(), degAcr.Split('-')[1].ToUpper(), (modeMulti + ddnar));
                                                                contentDiv.InnerHtml += ((StringBuilder)(rcptObj.returnHtmlString(out createPDFOK))).ToString();
                                                            }
                                                            else if (save1 == 9)
                                                            {
                                                                #region  Print Format 1 in Html
                                                                if (ds.Tables.Count > 0)
                                                                {
                                                                    //if (ds.Tables[0].Rows.Count > 0)
                                                                    //{
                                                                    //    collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                                                    //    add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                                                    //    add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                                                    //    add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                                                                    //    univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                                                    //}
                                                                    //if (ds.Tables[1].Rows.Count > 0)
                                                                    //{
                                                                    //    if (degACR == 0)
                                                                    //    {
                                                                    //        deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                                                    //    }
                                                                    //    else
                                                                    //    {
                                                                    //        deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                                    //    }
                                                                    //cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                                    //batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                                    //seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                                                    //board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                                    //mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                                    //fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                                                    //stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                                                    //}

                                                                    int rectHeight = 920;
                                                                    if (studOffiCopy == 1)
                                                                    {
                                                                        rectHeight = 475;
                                                                    }

                                                                    #region Receipt Header

                                                                    //Header Images
                                                                    //Line1
                                                                    string Hllogo = string.Empty;
                                                                    if (leftLogo != 0)
                                                                    {
                                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                                                                        {
                                                                            Hllogo = "<img src='" + "college/Left_Logo.jpeg?" + DateTime.Now.Ticks.ToString() + "' style='height:80px; width:80px;'/>";
                                                                        }
                                                                    }
                                                                    string Hcol = string.Empty;
                                                                    if (collegeid != 0)
                                                                    {
                                                                        Hcol = collegename;
                                                                    }
                                                                    string Hrlogo = string.Empty;
                                                                    //if (rightLogo != 0)
                                                                    //{
                                                                    //    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                                                                    //    {
                                                                    //        Hrlogo = "<img src='" + "college/Right_Logo.jpeg?" + DateTime.Now.Ticks.ToString() + "' style='height:80px; width:80px;'/>";
                                                                    //    }
                                                                    //}
                                                                    Hrlogo = "<div style='height:80px; width:100px; border:1px solid black;'><div style='margin-top:30px;font-size:20px;'><b>" + Regex.Replace(recptNo, @"[\d-]", string.Empty).ToUpper() + "</b></div></div>";
                                                                    //Line2
                                                                    string Huniv = string.Empty;
                                                                    if (university != 0)
                                                                    {
                                                                        Huniv = univ;
                                                                    }
                                                                    //Line3
                                                                    string Hadd1add2 = string.Empty;
                                                                    if (address1 != 0 || address2 != 0)
                                                                    {
                                                                        if (address2 != 0)
                                                                        {
                                                                            add1 += " " + add2;
                                                                        }
                                                                        Hadd1add2 = add1;

                                                                    }
                                                                    //Line4
                                                                    string Hadd3 = string.Empty;
                                                                    if (address3 != 0)
                                                                    {
                                                                        //Hadd3 = add3;
                                                                        Hadd1add2 = Hadd1add2.TrimEnd('.', ',') + "," + add3;
                                                                    }

                                                                    sbHtml.Append("<center><table cellpadding='0' cellspacing='0' style='text-align:center; width: 585px; ' class='classBold10'><tr><td rowspan='5'>" + Hllogo + "</td><td style='text-align:center; font-weight:bold; font-size:14px;'>" + Hcol + "</td><td  rowspan='5' >" + Hrlogo + "</td></tr><tr><td  style='text-align:center;'>" + Huniv + "</td></tr><tr><td  style='text-align:center;'>" + Hadd1add2 + Hadd3 + "</td></tr><tr><td style='text-align:center; font-weight:bold; font-size:14px;'><u>RECEIPT" + dupReceipt + "</u></td></tr></table></center>");
                                                                    #endregion

                                                                    #region Table 1

                                                                    //Table1 Data
                                                                    string Htime1 = string.Empty;
                                                                    string Htime2 = string.Empty;
                                                                    //Line 1
                                                                    if (time != 0)
                                                                    {
                                                                        Htime1 = "Time";
                                                                        Htime2 = ": " + rcptTime;
                                                                    }
                                                                    //Line2
                                                                    ArrayList arr = new ArrayList();
                                                                    string Hsname1 = string.Empty;
                                                                    if (studnam != 0)
                                                                    {
                                                                        // Hsname1 = "<td colspan='2'>Name: " + studname + "</td>";
                                                                        //arr.Add(Hsname1);
                                                                        Htime1 = "Name";
                                                                        Htime2 = ": " + studname;
                                                                    }
                                                                    string Hregno1 = string.Empty;
                                                                    if (regno != 0)
                                                                    {
                                                                        Hregno1 = "<td colspan='2'>RegNo: " + Regno + "</td>";
                                                                        arr.Add(Hregno1);
                                                                    }

                                                                    string HrolNo1 = string.Empty;
                                                                    if (rolno != 0)
                                                                    {
                                                                        HrolNo1 = "<td colspan='2'>RollNo: " + rollno + "</td>";
                                                                        arr.Add(HrolNo1);
                                                                    }
                                                                    string HadmNo1 = string.Empty;
                                                                    if (admno != 0)
                                                                    {
                                                                        HadmNo1 = "<td colspan='2'>AdmissionNo: " + app_formno + "</td>";
                                                                        arr.Add(HadmNo1);
                                                                    }

                                                                    string Hfname1 = string.Empty;
                                                                    if (fathername != 0)
                                                                    {
                                                                        Hfname1 = "<td colspan='2'>Father's Name: " + fathe + "</td>";
                                                                        arr.Add(Hfname1);
                                                                    }
                                                                    string HMother1 = string.Empty;
                                                                    if (mothername != 0)
                                                                    {
                                                                        HMother1 = "<td colspan='2'>Mother's Name: " + mothe + "</td>";
                                                                        arr.Add(HMother1);
                                                                    }

                                                                    //Line 3
                                                                    string batYrSemHead = string.Empty;
                                                                    string batYrSemCont = string.Empty;
                                                                    if (degACR != 0)
                                                                    {
                                                                        batYrSemHead = "Degree/";
                                                                        batYrSemCont = deg + "/";
                                                                    }
                                                                    if (year != 0)
                                                                    {
                                                                        //batYrSemCont += " " + romanLetter(returnYearforSem(cursem)) + "/";
                                                                        int yr = 0;
                                                                        int.TryParse(reUse.GetFunction("select batch_year from applyn where app_no =" + appnoNew + "").Trim(), out yr);
                                                                        if (yr != 0)
                                                                        {
                                                                            batYrSemHead += "Yr/";
                                                                            batYrSemCont += " " + yr + "/";
                                                                        }
                                                                    }
                                                                    if (semester != 0)
                                                                    {
                                                                        batYrSemHead += "Sem";
                                                                        batYrSemCont += " " + romanLetter(cursem);
                                                                    }
                                                                    batYrSemHead = batYrSemHead.TrimEnd('/');
                                                                    batYrSemCont = batYrSemCont.TrimEnd('/');

                                                                    string HbatYrSem1 = string.Empty;
                                                                    if (batYrSemHead != "")
                                                                    {
                                                                        HbatYrSem1 = "<td colspan='2'>" + batYrSemHead + ": " + batYrSemCont + "</td>";
                                                                        arr.Add(HbatYrSem1);
                                                                    }
                                                                    string HseatType1 = string.Empty;
                                                                    if (seattype != 0)
                                                                    {
                                                                        HseatType1 = "<td colspan='2'>Seat Type: " + seatty + "</td>";
                                                                        arr.Add(HseatType1);
                                                                    }
                                                                    string Hboard1 = string.Empty;
                                                                    if (boarding != 0)
                                                                    {
                                                                        Hboard1 = "<td colspan='2'>Boarding: " + board + "</td>";
                                                                        arr.Add(Hboard1);
                                                                    }

                                                                    sbHtml.Append("<br><center><table cellpadding='0' cellspacing='0' style='text-align:left; width: 585px; padding-left:5px; ' class='classBold10'><tr><td>Receipt No</td><td>" + ": " + recptNo + "</td><td>" + Htime1 + "</td><td>" + Htime2 + "</td><td>Date</td><td>" + ": " + recptDt + "</td></tr>");
                                                                    int lasti = -1;
                                                                    for (int ar = 0; ar < arr.Count; ar++)
                                                                    {
                                                                        if (ar == 0 || ar == 3 || ar == 6)
                                                                        {
                                                                            sbHtml.Append("<tr>");
                                                                        }
                                                                        sbHtml.Append(arr[ar]);
                                                                        if (ar == 2 || ar == 5 || ar == 8)
                                                                        {
                                                                            sbHtml.Append("</tr>");
                                                                        }
                                                                        lasti = ar;
                                                                    }
                                                                    if (lasti != 2 && lasti != 5 && lasti != 8)
                                                                    {
                                                                        sbHtml.Append("</tr>");
                                                                    }
                                                                    sbHtml.Append("</table></center>");

                                                                    #endregion

                                                                    #region Table2 Format

                                                                    int rows = 1;
                                                                    selectQuery = "";
                                                                    Hashtable htIndex = new Hashtable();
                                                                    int hInsdx = 3;

                                                                    //Table2 Header

                                                                    if (semOrYear != 0)
                                                                    {

                                                                        htIndex.Add("semOrYear", hInsdx);
                                                                        hInsdx++;
                                                                    }

                                                                    if (allotedAmt != 0)
                                                                    {

                                                                        htIndex.Add("allotedAmt", hInsdx);
                                                                        hInsdx++;
                                                                    }

                                                                    if (balAmt != 0)
                                                                    {

                                                                        htIndex.Add("balAmt", hInsdx);
                                                                        hInsdx++;
                                                                    }
                                                                    if (prevPaidAmt != 0)
                                                                    {

                                                                        htIndex.Add("prevPaidAmt", hInsdx);
                                                                        hInsdx++;
                                                                    }

                                                                    if (concession != 0)
                                                                    {

                                                                        htIndex.Add("concession", hInsdx);
                                                                        hInsdx++;
                                                                    }

                                                                    //Table2 Data

                                                                    int sno = 0;
                                                                    int indx = 0;
                                                                    double totalamt = 0;
                                                                    double balanamt = 0;
                                                                    double curpaid = 0;
                                                                    // double paidamount = 0;


                                                                    string selHeadersQ = string.Empty;
                                                                    DataSet dsHeaders = new DataSet();

                                                                    if (rcptType == "1" || rcptType == "2")
                                                                    {
                                                                        string StudStream = string.Empty;

                                                                        DataSet dsStr = new DataSet();
                                                                        dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                                                        if (dsStr.Tables.Count > 0)
                                                                        {
                                                                            if (dsStr.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                                                            }
                                                                        }

                                                                        selHeadersQ = " select sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d ,FS_ChlGroupHeaderSettings f,FT_FeeAllot A,FM_LedgerMaster l where d.HeaderFK =f.HeaderFK and D.LedgerFK=l.LedgerPK  and  d.LedgerFK=a.LedgerFK and d.App_No=a.App_No and A.FeeCategory =D.FeeCategory  and    transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'  ";
                                                                        if (StudStream != "")
                                                                        {
                                                                            selHeadersQ += " and f.stream='" + StudStream + "' ";
                                                                        }
                                                                        selHeadersQ += "   group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                                                    }
                                                                    else if (rcptType == "3")
                                                                    {
                                                                        selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                                                    }
                                                                    else if (rcptType == "4")
                                                                    {
                                                                        selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.LedgerFK=a.LedgerFK  and d.FeeCategory =A.FeeCategory and  d.App_No=a.App_No and transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";
                                                                    }
                                                                    #endregion

                                                                    if (selHeadersQ != string.Empty)
                                                                    {
                                                                        string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                                                        dsHeaders.Clear();
                                                                        dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                                                        if (dsHeaders.Tables.Count > 0 && dsHeaders.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            #region Table 2
                                                                            rows += dsHeaders.Tables[0].Rows.Count;

                                                                            sbHtml.Append("<br><center><table cellpadding='0' cellspacing='0' style='text-align:left; width: 560px;  border:1px solid; ' border='1' class='classBold10'><tr><td style='text-align:center;'>S.No</td><td style='text-align:center;'>Description</td><td style='text-align:center;'>Amount</td>");

                                                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                                                            {
                                                                                StringBuilder tempHtml = new StringBuilder();
                                                                                string disphdr = string.Empty;
                                                                                double allotamt0 = 0;
                                                                                double deductAmt0 = 0;
                                                                                double totalAmt0 = 0;
                                                                                double paidAmt0 = 0;
                                                                                double balAmt0 = 0;
                                                                                double creditAmt0 = 0;

                                                                                creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);

                                                                                totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                                                //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);


                                                                                //paidAmt0 = totalAmt0 - balAmt0;
                                                                                deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                                                disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                                                string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                                                string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                                                string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);
                                                                                string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                                                paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                                                #region Monthwise
                                                                                string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                                                string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                                                                int monWisemon = 0;
                                                                                int monWiseYea = 0;
                                                                                string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                                string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                                int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                                                                int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                                                                if (monWisemon > 0 && monWiseYea > 0)
                                                                                {
                                                                                    string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                                                    DataSet dsMonwise = new DataSet();
                                                                                    dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                                                    if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                                                    {
                                                                                        totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                                                        paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                                                        disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                                                        balAmt0 = totalAmt0 - paidAmt0;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                                }
                                                                                #endregion

                                                                                //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                                feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                                                                sno++;
                                                                                indx++;
                                                                                totalamt += Convert.ToDouble(totalAmt0);
                                                                                balanamt += Convert.ToDouble(balAmt0);
                                                                                curpaid += Convert.ToDouble(creditAmt0);

                                                                                deductionamt += Convert.ToDouble(deductAmt0);
                                                                                tempHtml.Append("<tr><td style='text-align:center;'>" + sno + "</td><td style='text-align:left;'>" + disphdr + "</td><td style='text-align:right;'>" + creditAmt0 + "</td>");
                                                                                if (semOrYear != 0)
                                                                                {
                                                                                    if (htIndex.Contains("semOrYear"))
                                                                                    {
                                                                                        tempHtml.Append("<td style='text-align:Center;'>" + feecatcode + "</td>");
                                                                                        if (indx == 1)
                                                                                        {
                                                                                            sbHtml.Append("<td style='text-align:center;'>Category</td>");
                                                                                        }
                                                                                    }

                                                                                    // htIndex.Add("semOrYear", hInsdx);
                                                                                }


                                                                                if (allotedAmt != 0)
                                                                                {
                                                                                    if (htIndex.Contains("allotedAmt"))
                                                                                    {
                                                                                        tempHtml.Append("<td style='text-align:right;'>" + totalAmt0 + "</td>");
                                                                                        if (indx == 1)
                                                                                        {
                                                                                            sbHtml.Append("<td style='text-align:center;'>Fixed Fee Rs</td>");
                                                                                        }
                                                                                    }
                                                                                }

                                                                                if (balAmt != 0)
                                                                                {
                                                                                    if (htIndex.Contains("balAmt"))
                                                                                    {
                                                                                        tempHtml.Append("<td style='text-align:right;'>" + balAmt0 + "</td>");
                                                                                        if (indx == 1)
                                                                                        {
                                                                                            sbHtml.Append("<td style='text-align:center;'>Balance Rs</td>");
                                                                                        }
                                                                                    }
                                                                                }
                                                                                if (prevPaidAmt != 0)
                                                                                {
                                                                                    if (htIndex.Contains("prevPaidAmt"))
                                                                                    {
                                                                                        tempHtml.Append("<td style='text-align:right;'>" + paidAmt0 + "</td>");
                                                                                        if (indx == 1)
                                                                                        {
                                                                                            sbHtml.Append("<td style='text-align:center;'>Already Paid Rs</td>");
                                                                                        }
                                                                                    }

                                                                                }

                                                                                if (concession != 0)
                                                                                {
                                                                                    if (htIndex.Contains("concession"))
                                                                                    {
                                                                                        tempHtml.Append("<td style='text-align:right;'>" + deductAmt0 + "</td>");
                                                                                        if (indx == 1)
                                                                                        {
                                                                                            sbHtml.Append("<td style='text-align:center;'>Deduction Rs</td>");
                                                                                        }
                                                                                    }

                                                                                }
                                                                                sbHtml.Append("</tr>");
                                                                                tempHtml.Append("</tr>");
                                                                                sbHtml.Append(tempHtml.ToString());
                                                                            }
                                                                            sbHtml.Append("</table></center>");
                                                                            createPDFOK = true;

                                                                            //curY += 5 + (int)addtabletopage1.Area.Height;
                                                                            #endregion

                                                                            #region Table 3
                                                                            if (BalanceType == 1)
                                                                            {
                                                                                balanamt = retBalance(appnoNew);
                                                                            }
                                                                            decimal totalamount = (decimal)curpaid;

                                                                            //curY += 5 + (int)addtabletopage2.Area.Height;
                                                                            //sbHtml.Append("<center><table cellpadding='0' cellspacing='0' style='text-align:left; width: 560px;  border:1px solid;padding-top:0px; ' border='1' class='classBold10'><tr><td style='text-align:center;' colspan='4'> " + DecimalToWords(totalamount).ToString() + "Rupees Only</td><td style='text-align:center;'>Total</td><td style='text-align:center;' colspan='3'>Rs. " + curpaid + "/-</td><tr></table></center>");
                                                                            sbHtml.Append("<center><table cellpadding='0' cellspacing='0' style='text-align:left; width: 560px;  border:0px solid;padding-top:0px; ' class='classBold10'><tr><td style='text-align:center;' colspan='4'> ( " + DecimalToWords(totalamount).ToString() + "Rupees Only )</td><td style='text-align:center;' colspan='3'>Total - Rs. " + curpaid + "/-</td><tr></table></center>");
                                                                            #endregion

                                                                            #region Receipt Footer

                                                                            sbHtml.Append("<center><table cellpadding='0' cellspacing='0' style='text-align:left; width: 560px; padding-top:5px; ' class='classBold10'><tr><td style='text-align:left;'>Mode of Pay : " + mode + " " + ddnew + "</td></tr><tr><td style='text-align:left;'>" + remarks + " </td>");

                                                                            if (deduction != 0)
                                                                            {
                                                                                sbHtml.Append("<td style='text-align:center;'>Deduction Amount Rs. : " + deductionamt + "</td>");
                                                                            }
                                                                            if (excessAmt != 0)
                                                                            {
                                                                                sbHtml.Append("<td style='text-align:center;'>Excess Amount Rs. : " + excessRemaining(appnoNew) + "</td>");
                                                                            }
                                                                            if (validDate != 0)
                                                                            {
                                                                                sbHtml.Append("<td style='text-align:center;'>Valid upto : " + "(" + recptValid + ")</td>");
                                                                            }

                                                                            sbHtml.Append("<tr></table></center>");


                                                                            sbHtml.Append("<center><table cellpadding='0' cellspacing='0' style='text-align:right; width: 560px;  padding-top:5px; ' class='classBold10'><tr><td style='text-align:right;'>");
                                                                            //Authorizer
                                                                            if (forclgName != 0)
                                                                            {
                                                                                sbHtml.Append("For " + collegename + "");
                                                                            }



                                                                            sbHtml.Append("</td><tr></table></center>");

                                                                            int marginTop = 240 - (rows * 15);
                                                                            if (authSignValue.Trim() != "")
                                                                            {
                                                                                sbHtml.Append("<br><br><div style='margin-top:" + marginTop + "px;margin-left:20px; text-align:left;float:left;'>#copy#<span style='padding-left:330px;'> " + authSignValue + "</span></div> ");
                                                                            }
                                                                            else
                                                                            {
                                                                                sbHtml.Append("<br><br><div style='margin-top:" + marginTop + "px;margin-left:20px; text-align:left;float:left;'>#copy#<span style='padding-left:330px;'>Authorised Signature</span></div> ");
                                                                            }

                                                                            if (studCopy != 0 || studOffiCopy == 1)
                                                                            {
                                                                                if (studOffiCopy != 1)
                                                                                {
                                                                                    StringBuilder sbFinal = new StringBuilder();
                                                                                    sbFinal.Append("<div style='padding-left:5px;height: 920px; width:595px;'>");
                                                                                    sbFinal.Append("<div style='width:585px; height:" + rectHeight + "px;padding-top:5px; border:1px solid;text-align:right; '  class='classBold10'>" + sbHtml.ToString() + "</div>");
                                                                                    sbFinal.Append("</div><br>");
                                                                                    sbFinal.Replace("#copy#", "Original Copy");
                                                                                    contentDiv.InnerHtml += sbFinal.ToString();
                                                                                }
                                                                                else
                                                                                {
                                                                                    StringBuilder sbFinal1 = new StringBuilder();
                                                                                    sbFinal1.Append("<div style='width:585px; height:" + rectHeight + "px;padding-top:5px; border:1px solid;text-align:right;'  class='classBold10'>" + sbHtml.ToString() + "</div><br>");
                                                                                    sbFinal1.Replace("#copy#", "Original Copy");

                                                                                    StringBuilder sbFinal = new StringBuilder();
                                                                                    sbFinal.Append("<div style='padding-left:5px;height: 920px; width:595px;'>");
                                                                                    sbFinal.Append(sbFinal1.ToString());
                                                                                    sbFinal.Append("<div style='width:585px; height:" + rectHeight + "px;padding-top:5px; border:1px solid;text-align:right;'  class='classBold10'>" + sbHtml.ToString() + "</div>");
                                                                                    sbFinal.Append("</div><br>");
                                                                                    sbFinal.Replace("#copy#", "Duplicate Copy");
                                                                                    contentDiv.InnerHtml += sbFinal.ToString();
                                                                                }
                                                                            }

                                                                            if (officopy != 0 && studOffiCopy != 1)
                                                                            {
                                                                                StringBuilder sbFinal = new StringBuilder();
                                                                                sbFinal.Append("<div style='padding-left:5px;height: 920px; width:595px;'>");
                                                                                sbFinal.Append("<div style='width:585px; height:" + rectHeight + "px;padding-top:5px; border:1px solid;text-align:right;'  class='classBold10'>" + sbHtml.ToString() + "</div> ");
                                                                                sbFinal.Append("</div><br>");
                                                                                sbFinal.Replace("#copy#", "Office Copy");
                                                                                contentDiv.InnerHtml += sbFinal.ToString();
                                                                            }

                                                                            if (transCopy != 0)
                                                                            {
                                                                                StringBuilder sbFinal = new StringBuilder();
                                                                                sbFinal.Append("<div style='padding-left:5px;height: 920px; width:595px;'>");
                                                                                sbFinal.Append("<div style='width:585px; height:" + rectHeight + "px;padding-top:5px; border:1px solid;text-align:right;'  class='classBold10'>" + sbHtml.ToString() + "</div>");
                                                                                sbFinal.Append("</div><br>");
                                                                                sbFinal.Replace("#copy#", "Transport Copy");
                                                                                contentDiv.InnerHtml += sbFinal.ToString();
                                                                            }

                                                                            #endregion
                                                                        }
                                                                    }

                                                                }
                                                                #endregion
                                                            }
                                                            else if (save1 == 12)
                                                            {
                                                                //For KCG
                                                                contentDiv.InnerHtml += ((StringBuilder)(returnHtmlStringKCG(out createPDFOK, AppNo, rollno, collegecode1, usercode, Convert.ToByte(rcptType), studname, recptDt, rcptTime, recptNo, cursem, deg.Split('-')[0].ToUpper(), deg.Split('-')[1].ToUpper(), ddnar))).ToString();
                                                            }
                                                        }
                                                        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); sbHtml.Clear(); }
                                                        finally
                                                        {
                                                        }
                                                        createPDFOK = true;
                                                    }
                                                    else
                                                    {
                                                        imgAlert.Visible = true;
                                                        lbl_alert.Text = "No Records Found";
                                                    }
                                                }
                                                else
                                                {
                                                    imgAlert.Visible = true;
                                                    lbl_alert.Text = "No Records Found";
                                                }
                                            }
                                        }
                                    }
                                                        #endregion
                                    #region To print the Receipt
                                    if (createPDFOK)
                                    {

                                        #region New Print
                                        //contentDiv.InnerHtml += sbHtml.ToString();
                                        contentDiv.Visible = true;
                                        ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
                                        #endregion
                                    }
                                    else
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "Receipt Cannot Be Generated";
                                    }
                                    #endregion
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "Please Add Print Settings";
                                }
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Please Add Print Settings";
                            }
                        }
                        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Select A Receipt";
                    }
                    #endregion
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    public void btnChlnDuplicate0_Click(string dupReceipt)
    {
        if (checkedOK())
        {

            FpSpread1.SaveChanges();
            try
            {
                string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
                DataSet dsPri = new DataSet();
                dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
                if (dsPri.Tables.Count > 0)
                {
                    if (dsPri.Tables[0].Rows.Count > 0)
                    {
                        string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

                        //Document Settings
                        PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.A4);

                        Font FontboldheadC = new Font("Arial", 15, FontStyle.Bold);
                        Font FontboldheadC1 = new Font("Arial", 14, FontStyle.Bold);
                        Font Fontboldhead = new Font("Arial", 12, FontStyle.Bold);
                        Font FontTableHead = new Font("Arial", 8, FontStyle.Bold);
                        Font FontTablebody = new Font("Arial", 8, FontStyle.Regular);
                        Font FontTable = new Font("Arial", 8, FontStyle.Bold);
                        Font tamilFont = new Font("AMUDHAM.TTF", 10, FontStyle.Regular);

                        bool createPDFOK = false;

                        #region For Every selected Receipt
                        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                            if (check == 1)
                            {
                                string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                                string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

                                string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                                if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                                {
                                    string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype,DDNo,convert(varchar(10),DDDate,103) as DDDate,(select textval from textvaltable where textcode =DDBankCode) as Bank,DDBankBranch from ft_findailytransaction  where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                                    DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                                    if (dsDet.Tables.Count > 0)
                                    {
                                        if (dsDet.Tables[0].Rows.Count > 0)
                                        {
                                            string rollno = string.Empty;
                                            string studname = string.Empty;
                                            string receiptno = string.Empty;
                                            string name = string.Empty;

                                            string app_formno = string.Empty;
                                            string appnoNew = string.Empty;
                                            string Regno = string.Empty;

                                            string batchYrSem = string.Empty;

                                            string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                                            string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                                            string mode = string.Empty;
                                            string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                                            string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                                            string modePaySng = string.Empty;
                                            string dddates = Convert.ToString(dsDet.Tables[0].Rows[0]["DDDate"]);
                                            string ddnos = Convert.ToString(dsDet.Tables[0].Rows[0]["DDNo"]);
                                            string ddBanks = Convert.ToString(dsDet.Tables[0].Rows[0]["Bank"]);
                                            string ddBrans = Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankBranch"]);
                                            switch (paymode)
                                            {
                                                case "1":
                                                    mode = "Cash";
                                                    break;
                                                case "2":
                                                    mode = "Cheque";
                                                    modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                                    // mode += modePaySng;
                                                    break;
                                                case "3":
                                                    mode = "DD";
                                                    modePaySng = "\n\nDDNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                                    // mode += modePaySng;
                                                    break;
                                                case "4":
                                                    mode = "Challan";
                                                    break;
                                                case "5":
                                                    mode = "Online";
                                                    break;
                                                default:
                                                    mode = "Others";
                                                    break;
                                            }


                                            string queryRollApp;

                                            if (ddlBefAfteAdm.SelectedIndex == 0)
                                            {
                                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            }
                                            else
                                            {
                                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name  from applyn where app_no='" + AppNo + "'";
                                            }
                                            DataSet dsRollApp = new DataSet();
                                            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                                            if (dsRollApp.Tables.Count > 0)
                                            {
                                                if (dsRollApp.Tables[0].Rows.Count > 0)
                                                {
                                                    rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                                    app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                                    Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                                    studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                                }
                                            }
                                            name = rollno + "-" + studname;

                                            //Print Region

                                            try
                                            {
                                                #region Print Option For Receipt

                                                PdfPage rcptpage = recptDoc.NewPage();
                                                //Fields to print

                                                #region Settings Input
                                                //Header Div Values
                                                byte collegeid = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                                                byte address1 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd1"]);
                                                byte address2 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd2"]);
                                                byte address3 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd3"]);
                                                byte city = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeDist"]);
                                                byte state = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeState"]);

                                                byte university = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeUniversity"]);
                                                byte rightLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRightLogo"]);
                                                byte leftLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsLeftLogo"]);
                                                byte time;
                                                if (Convert.ToBoolean(Convert.ToString(dsPri.Tables[0].Rows[0]["IsTime"])))
                                                {
                                                    time = 1;
                                                }
                                                else
                                                {
                                                    time = 0;
                                                }
                                                byte degACR = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsDegreeAcr"]);
                                                byte degNam = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsDegreeName"]);
                                                byte studnam = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudName"]);
                                                byte year = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsYear"]);
                                                byte semester = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSemester"]);
                                                byte regno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRegNo"]);
                                                byte rolno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRollNo"]);
                                                byte admno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAdminNo"]);

                                                byte fathername = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFatherName"]);
                                                byte seattype = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSeatType"]);
                                                //byte setRollAsAdmin = Convert.ToByte(dsPri.Tables[0].Rows[0]["rollas_adm"]);
                                                byte boarding = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsBoarding"]);
                                                byte mothername = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsMontherName"]);
                                                string recptValid = Convert.ToString(dsPri.Tables[0].Rows[0]["ValidDate"]);


                                                //Body Div Values
                                                //byte showAllFees = Convert.ToByte(dsPri.Tables[0].Rows[0]["showallfee"]);
                                                byte allotedAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAllotedAmt"]);
                                                byte fineAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFineAmt"]);
                                                byte balAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsBalanceAmt"]);
                                                byte semOrYear = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSemYear"]);
                                                byte prevPaidAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsPrevPaid"]);
                                                byte excessAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsExcessAmt"]);
                                                // byte totDetails = Convert.ToByte(dsPri.Tables[0].Rows[0]["Total_Details"]);
                                                byte fineInRow = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFineinRow"]);
                                                //byte totWTselectCol = Convert.ToByte(dsPri.Tables[0].Rows[0]["TotalSelCol"]);
                                                byte concession = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsConcession"]);
                                                string concessionValue = string.Empty;
                                                if (concession != 0)
                                                {
                                                    concessionValue = Convert.ToString(dsPri.Tables[0].Rows[0]["ConcessionName"]);
                                                }


                                                //Footer Div Values

                                                byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                                                byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                                                byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);
                                                byte narration = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsNarration"]);
                                                if (narration != 0)
                                                {
                                                    mode += modePaySng;
                                                }
                                                byte deduction = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTotConcession"]);
                                                byte forclgName = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsForCollegeName"]);
                                                byte authSign = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAuthSign"]);
                                                byte validDate = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsValidUpto"]);
                                                string authSignValue = string.Empty;
                                                if (authSign != 0)
                                                {
                                                    authSignValue = Convert.ToString(dsPri.Tables[0].Rows[0]["AuthName"]);

                                                }

                                                byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                                                // byte dispModeWTcash = Convert.ToByte(dsPri.Tables[0].Rows[0]["DisModeWithCash"]);
                                                byte signFile = Convert.ToByte(dsPri.Tables[0].Rows[0]["cashier_sign"]);

                                                //if (signFile != 0)
                                                //{
                                                //if (FileUpload1.HasFile)
                                                //{

                                                //}                                                    
                                                //}


                                                #endregion

                                                #region Students Input
                                                //string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL(type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.app_no='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
                                                string colquery = "";
                                                if (ddlBefAfteAdm.SelectedIndex == 0)
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,isnull(r.sections,'') as sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                else
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,'' Sections from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                string collegename = "";
                                                string add1 = "";
                                                string add2 = "";
                                                string add3 = "";
                                                string univ = "";
                                                string deg = "";
                                                string cursem = "";
                                                string batyr = "";
                                                string seatty = "";
                                                string board = "";
                                                string mothe = "";
                                                string fathe = "";
                                                string stream = "";
                                                string section = "";
                                                double deductionamt = 0;
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(colquery, "Text");
                                                if (ds.Tables.Count > 0)
                                                {
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                                                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                                    }
                                                    if (ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        if (degACR == 0)
                                                        {
                                                            deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                                        }
                                                        else
                                                        {
                                                            deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                        }
                                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                                        board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                        mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                                        stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                                        section = Convert.ToString(ds.Tables[1].Rows[0]["sections"]);
                                                    }
                                                }
                                                #endregion

                                                int pagelength = 1;
                                                int rectHeight = 800;
                                                if (studOffiCopy == 1)
                                                {
                                                    pagelength = 2;
                                                    rectHeight = 380;
                                                }
                                                PdfPage rcptpageTran = recptDoc.NewPage();
                                                int curY = 10;
                                                int curX = 30;
                                                for (int pl = 1; pl <= pagelength; pl++)
                                                {

                                                    if (pl == 2)
                                                    {
                                                        curY = 420;
                                                    }
                                                    #region Receipt Header


                                                    //Rectangle Border
                                                    PdfArea rectArea = new PdfArea(recptDoc, 10, curY, 570, rectHeight);
                                                    PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
                                                    rcptpage.Add(rectSpace);

                                                    //Header Images
                                                    //Line1
                                                    if (leftLogo != 0)
                                                    {
                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                                                        {
                                                            PdfImage LogoImage = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg"));
                                                            rcptpage.Add(LogoImage, curX, curY, 450);
                                                        }
                                                    }
                                                    if (collegeid != 0)
                                                    {
                                                        curX = 80;
                                                        PdfTextArea clgText = new PdfTextArea(FontboldheadC, Color.Black, new PdfArea(recptDoc, curX, curY + 5, 450, 20), ContentAlignment.MiddleCenter, collegename);
                                                        rcptpage.Add(clgText);
                                                    }
                                                    if (rightLogo != 0)
                                                    {
                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                                                        {
                                                            curX = 500;
                                                            PdfImage LogoImage1 = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg"));
                                                            rcptpage.Add(LogoImage1, curX, curY, 450);
                                                        }
                                                    }
                                                    //Line2
                                                    if (university != 0)
                                                    {
                                                        curY += 20;
                                                        curX = 120;
                                                        PdfTextArea uniText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, univ);
                                                        rcptpage.Add(uniText);
                                                    }
                                                    //Line3
                                                    string jaiadd1 = "";
                                                    if (address1 != 0 || address2 != 0)
                                                    {
                                                        curX = 120;
                                                        curY += 15;
                                                        if (address2 != 0)
                                                        {
                                                            jaiadd1 = add1 + " " + add2;
                                                        }
                                                        PdfTextArea addText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, jaiadd1);
                                                        rcptpage.Add(addText);
                                                    }
                                                    //Line4
                                                    if (address3 != 0)
                                                    {
                                                        curX = 120;
                                                        curY += 15;
                                                        PdfTextArea cityText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, add3);
                                                        rcptpage.Add(cityText);
                                                    }

                                                    curX = 280;
                                                    curY += 35;
                                                    //Text Area For Receipt
                                                    PdfTextArea headingText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX - 100, curY, 200, 30), ContentAlignment.MiddleCenter, "RECEIPT" + dupReceipt);
                                                    rcptpage.Add(headingText);
                                                    int curX1 = 265;
                                                    int curX2 = 315;
                                                    curY += 21;
                                                    //PdfLine underLineRecpt = new PdfLine(recptDoc, new Point(curX1, curY), new Point(curX2, curY), Color.Black, 1);
                                                    //rcptpage.Add(underLineRecpt);

                                                    #endregion

                                                    #region Table 1

                                                    int rowIn = 0;
                                                    int colIn = 0;
                                                    //Table1 Format 
                                                    PdfTable tableparts = recptDoc.NewTable(FontTableHead, 7, 7, 5);
                                                    tableparts.VisibleHeaders = false;

                                                    //Table1 Data
                                                    //Line 1
                                                    tableparts.Cell(rowIn, colIn).SetContent("Receipt No");
                                                    tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                    tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    colIn++;

                                                    tableparts.Cell(rowIn, colIn).SetContent(": " + recptNo);
                                                    tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                    tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    colIn++;
                                                    if (colIn == 6)
                                                    {
                                                        colIn = 0;
                                                        rowIn++;
                                                    }

                                                    if (studnam != 0)
                                                    {
                                                        tableparts.Cell(rowIn, colIn).SetContent("Name : " + studname);
                                                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                        colIn++;
                                                        //tableparts.Cell(rowIn, colIn).SetContent(" : " + studname);
                                                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        colIn++;

                                                    }
                                                    if (colIn == 6)
                                                    {
                                                        colIn = 0;
                                                        rowIn++;
                                                    }

                                                    tableparts.Cell(rowIn, colIn).SetContent("Date");
                                                    tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                    tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    colIn++;

                                                    tableparts.Cell(rowIn, colIn).SetContent(": " + recptDt);
                                                    tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                    tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    colIn++;
                                                    //tableparts.Cell(0, 5).ColSpan = 2;

                                                    //Line2

                                                    if (colIn == 6)
                                                    {
                                                        colIn = 0;
                                                        rowIn++;
                                                    }


                                                    if (regno != 0)
                                                    {
                                                        tableparts.Cell(rowIn, colIn).SetContent("RegNo : " + Regno);
                                                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                        colIn++;
                                                        //tableparts.Cell(rowIn, colIn).SetContent(": " + Regno);
                                                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        //tableparts.Cell(rowIn, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        colIn++;
                                                        // tableparts.Cell(rowIn, colIn).ColSpan = 2;

                                                    }
                                                    if (colIn == 6)
                                                    {
                                                        colIn = 0;
                                                        rowIn++;
                                                    }

                                                    if (rolno != 0)
                                                    {
                                                        tableparts.Cell(rowIn, colIn).SetContent("RollNo : " + rollno);
                                                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                        colIn++;
                                                        //tableparts.Cell(rowIn, colIn).SetContent(": " + rollno);
                                                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        colIn++;
                                                        //  tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                    }
                                                    if (colIn == 6)
                                                    {
                                                        colIn = 0;
                                                        rowIn++;
                                                    }

                                                    if (admno != 0)
                                                    {
                                                        tableparts.Cell(rowIn, colIn).SetContent("AdmissionNo : " + app_formno);
                                                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                        colIn++;
                                                        //tableparts.Cell(rowIn, colIn).SetContent(": " + app_formno);
                                                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        colIn++;
                                                        //tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                    }
                                                    if (colIn == 6)
                                                    {
                                                        colIn = 0;
                                                        rowIn++;
                                                    }

                                                    if (fathername != 0)
                                                    {
                                                        tableparts.Cell(rowIn, colIn).SetContent("Father's Name : " + fathe);
                                                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                        colIn++;
                                                        //tableparts.Cell(rowIn, colIn).SetContent(" : " + fathe);
                                                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        colIn++;
                                                        //tableparts.Cell(1, 4).ColSpan = 2;
                                                    }
                                                    if (colIn == 6)
                                                    {
                                                        colIn = 0;
                                                        rowIn++;
                                                    }

                                                    if (mothername != 0)
                                                    {
                                                        tableparts.Cell(rowIn, colIn).SetContent("Mother's Name : " + mothe);
                                                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                        colIn++;
                                                        //tableparts.Cell(rowIn, colIn).SetContent(": " + mothe);
                                                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        colIn++;
                                                        // tableparts.Cell(1, colIn).ColSpan = 2;
                                                    }

                                                    //Line 3
                                                    if (colIn == 6)
                                                    {
                                                        colIn = 0;
                                                        rowIn++;
                                                    }

                                                    string batYrSemHead = string.Empty;
                                                    string batYrSemCont = string.Empty;
                                                    if (degACR != 0)
                                                    {
                                                        batYrSemHead = "Degree/";
                                                        batYrSemCont = deg + "/";
                                                    }
                                                    if (year != 0)
                                                    {
                                                        batYrSemHead += "Yr/";
                                                        batYrSemCont += " " + romanLetter(returnYearforSem(cursem)) + "/";

                                                    }
                                                    if (semester != 0)
                                                    {
                                                        batYrSemHead += "Sem";
                                                        batYrSemCont += " " + romanLetter(cursem);
                                                        if (section.Trim() != string.Empty)
                                                        {
                                                            batYrSemCont += "-" + section;
                                                        }
                                                    }
                                                    batYrSemHead = batYrSemHead.TrimEnd('/');
                                                    batYrSemCont = batYrSemCont.TrimEnd('/');

                                                    if (batYrSemHead != "")
                                                    {
                                                        tableparts.Cell(rowIn, colIn).SetContent(batYrSemHead + " : " + batYrSemCont);
                                                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                        colIn++;
                                                        //tableparts.Cell(rowIn, colIn).SetContent(": " + batYrSemCont);
                                                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        colIn++;
                                                        // tableparts.Cell(2, colIn).ColSpan = 2;
                                                    }
                                                    if (colIn == 6)
                                                    {
                                                        colIn = 0;
                                                        rowIn++;
                                                    }

                                                    if (seattype != 0)
                                                    {
                                                        tableparts.Cell(rowIn, colIn).SetContent("Seat Type : " + seatty);
                                                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                        colIn++;
                                                        //tableparts.Cell(rowIn, colIn).SetContent(": " + seatty);
                                                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        colIn++;
                                                        //tableparts.Cell(rowIn, 4).ColSpan = 2;
                                                    }
                                                    if (colIn == 6)
                                                    {
                                                        colIn = 0;
                                                        rowIn++;
                                                    }

                                                    if (boarding != 0)
                                                    {
                                                        tableparts.Cell(rowIn, colIn).SetContent("Boarding : " + board);
                                                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                        colIn++;
                                                        //tableparts.Cell(rowIn, colIn).SetContent(": " + board);
                                                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                                                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        colIn++;
                                                        // tableparts.Cell(rowIn, colIn).ColSpan = 2;
                                                    }

                                                    curX = 15;
                                                    curY += 1;
                                                    PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, curX, curY, 600, 200));
                                                    rcptpage.Add(addtabletopage1);

                                                    #endregion

                                                    #region Table 2
                                                    //Table2 Format

                                                    int rows = 1;

                                                    selectQuery = "";

                                                    Hashtable htIndex = new Hashtable();
                                                    int hInsdx = 3;

                                                    //Table2 Header

                                                    int descWidth = 440;

                                                    if (semOrYear != 0)
                                                    {

                                                        htIndex.Add("semOrYear", hInsdx);
                                                        hInsdx++;
                                                        descWidth -= 70;
                                                    }


                                                    if (allotedAmt != 0)
                                                    {

                                                        htIndex.Add("allotedAmt", hInsdx);
                                                        hInsdx++;
                                                        descWidth -= 70;
                                                    }

                                                    if (balAmt != 0)
                                                    {

                                                        htIndex.Add("balAmt", hInsdx);
                                                        hInsdx++;
                                                        descWidth -= 70;
                                                    }
                                                    if (prevPaidAmt != 0)
                                                    {

                                                        htIndex.Add("prevPaidAmt", hInsdx);
                                                        hInsdx++;
                                                        descWidth -= 80;
                                                    }

                                                    if (concession != 0)
                                                    {

                                                        htIndex.Add("concession", hInsdx);
                                                        hInsdx++;
                                                        descWidth -= 70;
                                                    }




                                                    //Table2 Data

                                                    int sno = 0;
                                                    int indx = 0;
                                                    double totalamt = 0;
                                                    double balanamt = 0;
                                                    double curpaid = 0;
                                                    // double paidamount = 0;


                                                    string selHeadersQ = string.Empty;
                                                    DataSet dsHeaders = new DataSet();

                                                    if (rcptType == "1" || rcptType == "2")
                                                    {
                                                        string StudStream = string.Empty;

                                                        DataSet dsStr = new DataSet();
                                                        dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                                        if (dsStr.Tables.Count > 0)
                                                        {
                                                            if (dsStr.Tables[0].Rows.Count > 0)
                                                            {
                                                                StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                                            }
                                                        }

                                                        selHeadersQ = " select sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d ,FS_ChlGroupHeaderSettings f,FT_FeeAllot A,FM_LedgerMaster l where d.HeaderFK =f.HeaderFK and D.LedgerFK=l.LedgerPK  and  d.LedgerFK=a.LedgerFK and d.App_No=a.App_No and A.FeeCategory =D.FeeCategory  and    transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'  ";
                                                        if (StudStream != "")
                                                        {
                                                            selHeadersQ += " and f.stream='" + StudStream + "' ";
                                                        }
                                                        selHeadersQ += "   group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                                    }
                                                    else if (rcptType == "3")
                                                    {
                                                        selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                                    }
                                                    else if (rcptType == "4")
                                                    {
                                                        selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.LedgerFK=a.LedgerFK  and d.FeeCategory =A.FeeCategory and  d.App_No=a.App_No and transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";
                                                    }

                                                    if (selHeadersQ != string.Empty)
                                                    {


                                                        string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                                        dsHeaders.Clear();
                                                        dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                                        if (dsHeaders.Tables.Count > 0)
                                                        {
                                                            if (dsHeaders.Tables[0].Rows.Count > 0)
                                                            {
                                                                rows += dsHeaders.Tables[0].Rows.Count;
                                                                PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, htIndex.Count + 3, 5);
                                                                tableparts1.VisibleHeaders = false;
                                                                tableparts1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                                                tableparts1.Cell(0, 0).SetContent("S.No");
                                                                tableparts1.Cell(0, 0).SetFont(FontTableHead);
                                                                tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tableparts1.Columns[0].SetWidth(20);



                                                                tableparts1.Cell(0, 1).SetContent("Description");
                                                                tableparts1.Cell(0, 1).SetFont(FontTableHead);
                                                                tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tableparts1.Columns[1].SetWidth(descWidth);

                                                                tableparts1.Cell(0, 2).SetContent("Paid Rs");
                                                                tableparts1.Cell(0, 2).SetFont(FontTableHead);
                                                                tableparts1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tableparts1.Columns[2].SetWidth(40);

                                                                for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                                                {
                                                                    string disphdr = string.Empty;
                                                                    double allotamt0 = 0;
                                                                    double deductAmt0 = 0;
                                                                    double totalAmt0 = 0;
                                                                    double paidAmt0 = 0;
                                                                    double balAmt0 = 0;
                                                                    double creditAmt0 = 0;

                                                                    creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);

                                                                    totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                                    //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);


                                                                    //paidAmt0 = totalAmt0 - balAmt0;
                                                                    deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                                    disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                                    string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                                    string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                                    string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);
                                                                    string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' --and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                                    paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                                    #region Monthwise
                                                                    string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                                    string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                                                    int monWisemon = 0;
                                                                    int monWiseYea = 0;
                                                                    string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                    string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                    int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                                                    int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                                                    if (monWisemon > 0 && monWiseYea > 0)
                                                                    {
                                                                        string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                                        DataSet dsMonwise = new DataSet();
                                                                        dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                                        if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                                            paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                                            disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                                            balAmt0 = totalAmt0 - paidAmt0;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                    }
                                                                    #endregion


                                                                    feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                                                    sno++;
                                                                    indx++;
                                                                    totalamt += Convert.ToDouble(totalAmt0);
                                                                    balanamt += Convert.ToDouble(balAmt0);
                                                                    curpaid += Convert.ToDouble(creditAmt0);

                                                                    deductionamt += Convert.ToDouble(deductAmt0);

                                                                    tableparts1.Cell(indx, 0).SetContent(sno);
                                                                    tableparts1.Cell(indx, 0).SetFont(FontTablebody);
                                                                    tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                                                                    tableparts1.Cell(indx, 1).SetContent(disphdr);
                                                                    tableparts1.Cell(indx, 1).SetFont(FontTablebody);
                                                                    tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                    tableparts1.Cell(indx, 2).SetContent(creditAmt0);
                                                                    tableparts1.Cell(indx, 2).SetFont(FontTablebody);
                                                                    tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);

                                                                    if (semOrYear != 0)
                                                                    {
                                                                        if (htIndex.Contains("semOrYear"))
                                                                        {
                                                                            int ind = Convert.ToInt32(htIndex["semOrYear"]);
                                                                            tableparts1.Cell(indx, ind).SetContent(Convert.ToString(feecatcode));
                                                                            tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                                                                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                            if (indx == 1)
                                                                            {
                                                                                tableparts1.Cell(0, ind).SetContent("Category");
                                                                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                tableparts1.Columns[ind].SetWidth(60);
                                                                            }
                                                                        }

                                                                        // htIndex.Add("semOrYear", hInsdx);
                                                                    }


                                                                    if (allotedAmt != 0)
                                                                    {
                                                                        if (htIndex.Contains("allotedAmt"))
                                                                        {
                                                                            int ind = Convert.ToInt32(htIndex["allotedAmt"]);
                                                                            tableparts1.Cell(indx, ind).SetContent(totalAmt0);
                                                                            tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                                                                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                            if (indx == 1)
                                                                            {
                                                                                tableparts1.Cell(0, ind).SetContent("Fixed Fee Rs");
                                                                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                tableparts1.Columns[ind].SetWidth(60);
                                                                            }
                                                                        }
                                                                    }

                                                                    if (balAmt != 0)
                                                                    {
                                                                        if (htIndex.Contains("balAmt"))
                                                                        {
                                                                            int ind = Convert.ToInt32(htIndex["balAmt"]);
                                                                            tableparts1.Cell(indx, ind).SetContent(balAmt0);
                                                                            tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                                                                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                            if (indx == 1)
                                                                            {
                                                                                tableparts1.Cell(0, ind).SetContent("Balance Rs");
                                                                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                tableparts1.Columns[ind].SetWidth(60);
                                                                            }
                                                                        }
                                                                    }
                                                                    if (prevPaidAmt != 0)
                                                                    {
                                                                        if (htIndex.Contains("prevPaidAmt"))
                                                                        {
                                                                            int ind = Convert.ToInt32(htIndex["prevPaidAmt"]);
                                                                            tableparts1.Cell(indx, ind).SetContent(paidAmt0);
                                                                            tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                                                                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                            if (indx == 1)
                                                                            {
                                                                                tableparts1.Cell(0, ind).SetContent("Already Paid Rs");
                                                                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                tableparts1.Columns[ind].SetWidth(70);
                                                                            }
                                                                        }

                                                                    }

                                                                    if (concession != 0)
                                                                    {
                                                                        if (htIndex.Contains("concession"))
                                                                        {
                                                                            int ind = Convert.ToInt32(htIndex["concession"]);
                                                                            tableparts1.Cell(indx, ind).SetContent(deductAmt0);
                                                                            tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                                                                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                            if (indx == 1)
                                                                            {
                                                                                tableparts1.Cell(0, ind).SetContent("Deduction Rs");
                                                                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                tableparts1.Columns[ind].SetWidth(60);
                                                                            }
                                                                        }

                                                                    }
                                                                }
                                                                createPDFOK = true;

                                                                curY += 5 + (int)addtabletopage1.Area.Height;
                                                                PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 30, curY, 520, 600));
                                                                rcptpage.Add(addtabletopage2);

                                                                #region Table 3
                                                                //Table3 Format
                                                                PdfTable tableparts2 = recptDoc.NewTable(FontTableHead, 1, 8, 5);
                                                                tableparts2.VisibleHeaders = false;
                                                                tableparts2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                                                //Table3 Header              
                                                                decimal totalamount = (decimal)curpaid;

                                                                tableparts2.Cell(0, 0).SetContent("Received " + DecimalToWords(totalamount).ToString() + " Rupees Only");
                                                                tableparts2.Cell(0, 0).SetFont(FontTableHead);
                                                                tableparts2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tableparts2.Cell(0, 0).ColSpan = 4;

                                                                tableparts2.Cell(0, 4).SetContent("Total");
                                                                tableparts2.Cell(0, 4).SetFont(FontTableHead);
                                                                tableparts2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                                tableparts2.Cell(0, 5).SetContent("Rs. " + curpaid + "/-");
                                                                tableparts2.Cell(0, 5).SetFont(FontTableHead);
                                                                tableparts2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                                tableparts2.Cell(0, 6).SetContent("Balance");
                                                                tableparts2.Cell(0, 6).SetFont(FontTableHead);
                                                                tableparts2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                if (BalanceType == 1)
                                                                {
                                                                    balanamt = retBalance(appnoNew);
                                                                }
                                                                tableparts2.Cell(0, 7).SetContent("Rs. " + balanamt + "/-");
                                                                tableparts2.Cell(0, 7).SetFont(FontTableHead);
                                                                tableparts2.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);


                                                                curY += (int)addtabletopage2.Area.Height + 5;
                                                                PdfTablePage addtabletopage3 = tableparts2.CreateTablePage(new PdfArea(recptDoc, 30, curY, 520, 50));
                                                                rcptpage.Add(addtabletopage3);
                                                                #endregion

                                                                #region Receipt Footer
                                                                string modeMulti = string.Empty;
                                                                bool multiCash = false;
                                                                bool multiChk = false;
                                                                bool multiDD = false;

                                                                DataSet dtMulBnkDetails = new DataSet();
                                                                dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  else 'DD' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

                                                                string ddnar = string.Empty;
                                                                string remarks = string.Empty;
                                                                double modeht = 40;
                                                                if (narration != 0)
                                                                {
                                                                    if (dtMulBnkDetails.Tables.Count > 0)
                                                                    {
                                                                        int sn = 1;
                                                                        for (int z = 0; z < dtMulBnkDetails.Tables[0].Rows.Count; z++)
                                                                        {
                                                                            if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CASH")
                                                                            {
                                                                                multiCash = true;
                                                                                continue;
                                                                            }
                                                                            else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CHEQUE")
                                                                            {
                                                                                multiChk = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                multiDD = true;
                                                                            }

                                                                            ddnar += "\n\n" + sn.ToString() + ")No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                                            sn++;
                                                                        }
                                                                        modeht = dtMulBnkDetails.Tables[0].Rows.Count * 15;
                                                                        modeht += 20;

                                                                    }
                                                                    if (narration == 1)
                                                                    {
                                                                        remarks = d2.GetFunction("select narration from ft_findailytransaction where TransCode='" + recptNo.Trim() + "' and app_no='" + appnoNew + "' and isnull(iscanceled,0)=0");
                                                                        if (remarks.Trim() == "0")
                                                                            remarks = string.Empty;
                                                                        else
                                                                        {
                                                                            remarks = "\n\n" + remarks;
                                                                        }
                                                                    }
                                                                }

                                                                if (multiCash)
                                                                {
                                                                    modeMulti += "Cash,";
                                                                }
                                                                if (multiChk)
                                                                {
                                                                    modeMulti += "Cheque,";
                                                                }
                                                                if (multiDD)
                                                                {
                                                                    modeMulti += "DD";
                                                                }
                                                                modeMulti = modeMulti.TrimEnd(',');
                                                                if (modeMulti != "")
                                                                {
                                                                    mode = modeMulti;
                                                                }
                                                                ddnar += remarks;
                                                                //Mode of Pay

                                                                curY += 5 + (int)addtabletopage3.Area.Height;
                                                                if (ddnar.Trim() != "")
                                                                {
                                                                    curY += 5;
                                                                }
                                                                PdfTextArea modeofpayText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 30, curY, 250, modeht), ContentAlignment.MiddleLeft, "Mode of Pay : " + mode + ddnar);
                                                                rcptpage.Add(modeofpayText);

                                                                if (deduction != 0)
                                                                {
                                                                    PdfTextArea deducText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 130, curY, 200, 20), ContentAlignment.MiddleCenter, "Deduction Amount Rs. : " + deductionamt);
                                                                    rcptpage.Add(deducText);
                                                                }
                                                                if (excessAmt != 0)
                                                                {
                                                                    PdfTextArea exText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 240, curY, 200, 20), ContentAlignment.MiddleCenter, "Excess Amount Rs. : " + excessRemaining(appnoNew));
                                                                    rcptpage.Add(exText);
                                                                }
                                                                if (validDate != 0)
                                                                {
                                                                    PdfTextArea valdtText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 370, curY, 200, 20), ContentAlignment.MiddleCenter, "Valid upto : " + "(" + recptValid + ")");
                                                                    rcptpage.Add(valdtText);
                                                                }



                                                                //Authorizer
                                                                if (forclgName != 0)
                                                                {
                                                                    curY += 15;
                                                                    PdfTextArea authorizeText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 350, curY, 250, 20), ContentAlignment.MiddleCenter, "For " + collegename);
                                                                    rcptpage.Add(authorizeText);
                                                                }

                                                                if (authSignValue.Trim() != "")
                                                                {
                                                                    curY += 15;
                                                                    PdfTextArea authorizeSignText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, authSignValue);
                                                                    rcptpage.Add(authorizeSignText);
                                                                }
                                                                else
                                                                {
                                                                    curY += 15;
                                                                    PdfTextArea authorizeSignText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, "Authorised Signature");
                                                                    rcptpage.Add(authorizeSignText);
                                                                }

                                                                PdfPage rcptpageOf = rcptpage.CreateCopy();


                                                                if (transCopy != 0 && pl == 1)
                                                                {
                                                                    int cuyy = curY;
                                                                    //if (authSign == 0)
                                                                    //{
                                                                    cuyy += 10;
                                                                    //}
                                                                    rcptpageTran = rcptpage.CreateCopy();
                                                                    PdfTextArea transCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, cuyy, 150, 20), ContentAlignment.MiddleCenter, "Transport Copy ");
                                                                    rcptpageTran.Add(transCopyText);


                                                                }


                                                                if (studCopy != 0 || studOffiCopy == 1)
                                                                {
                                                                    //if (authSign == 0)
                                                                    //{
                                                                    curY += 10;
                                                                    //}
                                                                    string copy = "Student Copy ";
                                                                    if (pl == 2)
                                                                        copy = "Office Copy ";
                                                                    PdfTextArea studCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, copy);
                                                                    rcptpage.Add(studCopyText);
                                                                    if (pl == pagelength)
                                                                    {
                                                                        rcptpage.SaveToDocument();
                                                                    }
                                                                }

                                                                //save changes

                                                                if (pl == pagelength)
                                                                {
                                                                    if (officopy != 0 && studOffiCopy != 1)
                                                                    {
                                                                        PdfTextArea offCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, "Office Copy ");
                                                                        rcptpageOf.Add(offCopyText);
                                                                        rcptpageOf.SaveToDocument();

                                                                    }

                                                                }

                                                                if (transCopy != 0 && pl == pagelength)
                                                                {
                                                                    rcptpageTran.SaveToDocument();
                                                                }

                                                                curY += 10;

                                                                #endregion
                                                            }
                                                        }
                                                    }

                                                    #endregion


                                                }

                                                #endregion
                                            }
                                            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
                                            finally
                                            {

                                            }

                                            createPDFOK = true;
                                        }
                                        else
                                        {
                                            imgAlert.Visible = true;
                                            lbl_alert.Text = "No Records Found";
                                        }
                                    }
                                    else
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "No Records Found";
                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "Receipt Cancelled";
                                }

                            }
                        }
                        #endregion

                        #region To print the Receipt
                        if (createPDFOK)
                        {
                            //Response Write
                            string appPath = HttpContext.Current.Server.MapPath("~");
                            if (appPath != "")
                            {
                                string szPath = appPath + "/Report/";
                                string szFile = "Receipt" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                Response.Buffer = true;
                                Response.Clear();
                                recptDoc.SaveToFile(szPath + szFile);
                                //Response.ClearHeaders();
                                //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                //Response.ContentType = "application/pdf";
                                //Response.WriteFile(szPath + szFile);

                                Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Receipt Generated";
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Server Path Not Found";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Receipt Cannot Be Generated";
                        }
                        #endregion
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Add Print Settings";
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Add Print Settings";
                }
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Receipt";
        }
    }
    public void btnChlnDuplicate1_Click()
    {
        if (checkedOK())
        {
            FpSpread1.SaveChanges();
            try
            {
                string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
                DataSet dsPri = new DataSet();
                dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
                if (dsPri.Tables.Count > 0)
                {
                    if (dsPri.Tables[0].Rows.Count > 0)
                    {
                        string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

                        //Document Settings
                        PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.A4);

                        Font Fontboldhead = new Font("Arial", 10, FontStyle.Bold);
                        Font FontTableHead = new Font("Arial", 7, FontStyle.Bold);
                        Font FontTable = new Font("Arial", 7, FontStyle.Regular);
                        Font tamilFont = new Font("AMUDHAM.TTF", 10, FontStyle.Regular);

                        bool createPDFOK = false;

                        #region For Every selected Receipt
                        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                            if (check == 1)
                            {
                                string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                                string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);
                                string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                                if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                                {

                                    string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                                    DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                                    if (dsDet.Tables.Count > 0)
                                    {
                                        if (dsDet.Tables[0].Rows.Count > 0)
                                        {
                                            string rollno = string.Empty;
                                            string studname = string.Empty;
                                            string receiptno = string.Empty;
                                            string name = string.Empty;

                                            string app_formno = string.Empty;
                                            string appnoNew = string.Empty;
                                            string Regno = string.Empty;

                                            string batchYrSem = string.Empty;

                                            string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                                            string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                                            string mode = string.Empty;
                                            string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                                            string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                                            switch (paymode)
                                            {
                                                case "1":
                                                    mode = "Cash";
                                                    break;
                                                case "2":
                                                    mode = "Cheque";
                                                    break;
                                                case "3":
                                                    mode = "DD";
                                                    break;
                                                case "4":
                                                    mode = "Challan";
                                                    break;
                                                case "5":
                                                    mode = "Online";
                                                    break;
                                                default:
                                                    mode = "Others";
                                                    break;
                                            }


                                            //string queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            string queryRollApp;

                                            if (ddlBefAfteAdm.SelectedIndex == 0)
                                            {
                                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            }
                                            else
                                            {
                                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name  from applyn where app_no='" + AppNo + "'";
                                            }
                                            DataSet dsRollApp = new DataSet();
                                            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                                            if (dsRollApp.Tables.Count > 0)
                                            {
                                                if (dsRollApp.Tables[0].Rows.Count > 0)
                                                {
                                                    rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                                    app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                                    Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                                    studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                                }
                                            }
                                            name = rollno + "-" + studname;

                                            //Print Region

                                            try
                                            {

                                                #region Print Option For Receipt

                                                PdfPage rcptpage = recptDoc.NewPage();
                                                //Fields to print

                                                #region Settings Input
                                                //Header Div Values

                                                byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                                                byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                                                byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);

                                                #endregion

                                                #region Students Input
                                                //string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL(type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.app_no='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
                                                string colquery = "";
                                                if (ddlBefAfteAdm.SelectedIndex == 0)
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                else
                                                {
                                                    colquery = "select collname,universityaddress1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                string collegename = "";
                                                string add1 = "";
                                                string add2 = "";
                                                string add3 = "";
                                                string univ = "";
                                                string deg = "";
                                                string cursem = "";
                                                string batyr = "";
                                                string seatty = "";
                                                string board = "";
                                                string mothe = "";
                                                string fathe = "";
                                                string stream = "";
                                                double deductionamt = 0;
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(colquery, "Text");
                                                if (ds.Tables.Count > 0)
                                                {
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                                                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                                    }
                                                    if (ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        //if (degACR == 0)
                                                        //{
                                                        //    deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                                        //}
                                                        //else
                                                        //{
                                                        deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                        //}
                                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                                        board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                        mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                                        stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                                    }
                                                }
                                                string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                                try
                                                {
                                                    acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                                }
                                                catch { }
                                                #endregion

                                                #region Receipt Header
                                                int curY = 130;
                                                int curX = 30;

                                                //Rectangle Border
                                                PdfArea rectArea = new PdfArea(recptDoc, 10, 10, 570, 800);
                                                PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
                                                rcptpage.Add(rectSpace);

                                                #endregion

                                                #region Table 1
                                                //Table1 Format 
                                                PdfTable tableparts = recptDoc.NewTable(FontTableHead, 5, 6, 5);
                                                tableparts.VisibleHeaders = false;


                                                //Table1 Data
                                                //Line 1
                                                tableparts.Cell(0, 0).SetContent("Receipt No");
                                                tableparts.Cell(0, 0).SetFont(FontTableHead);
                                                tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(0, 1).SetContent(": " + recptNo);
                                                tableparts.Cell(0, 1).SetFont(FontTableHead);
                                                tableparts.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(0, 4).SetContent("Date");
                                                tableparts.Cell(0, 4).SetFont(FontTableHead);
                                                tableparts.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(0, 5).SetContent(": " + recptDt);
                                                tableparts.Cell(0, 5).SetFont(FontTableHead);
                                                tableparts.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                //Line2
                                                tableparts.Cell(1, 0).SetContent("Batch");
                                                tableparts.Cell(1, 0).SetFont(FontTableHead);
                                                tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(1, 1).SetContent(": " + batyr);
                                                tableparts.Cell(1, 1).SetFont(FontTableHead);
                                                tableparts.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(1, 2).SetContent("Degree / Branch");
                                                tableparts.Cell(1, 2).SetFont(FontTableHead);
                                                tableparts.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(1, 3).SetContent(": " + deg.ToUpper());
                                                tableparts.Cell(1, 3).SetFont(FontTableHead);
                                                tableparts.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                //Line3
                                                tableparts.Cell(2, 0).SetContent("RollNo");
                                                tableparts.Cell(2, 0).SetFont(FontTableHead);
                                                tableparts.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(2, 1).SetContent(": " + rollno);
                                                tableparts.Cell(2, 1).SetFont(FontTableHead);
                                                tableparts.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                tableparts.Cell(2, 2).SetContent("Name");
                                                tableparts.Cell(2, 2).SetFont(FontTableHead);
                                                tableparts.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(2, 3).SetContent(": " + studname.ToUpper());
                                                tableparts.Cell(2, 3).SetFont(FontTableHead);
                                                tableparts.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(2, 3).ColSpan = 3;

                                                //Line4
                                                tableparts.Cell(3, 0).SetContent("Fee For Academic Year");
                                                tableparts.Cell(3, 0).SetFont(FontTableHead);
                                                tableparts.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(3, 0).ColSpan = 2;

                                                tableparts.Cell(3, 2).SetContent(acaYear);
                                                tableparts.Cell(3, 2).SetFont(FontTableHead);
                                                tableparts.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                tableparts.Cell(3, 4).SetContent("Type");
                                                tableparts.Cell(3, 4).SetFont(FontTableHead);
                                                tableparts.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                tableparts.Cell(3, 5).SetContent(": " + mode);
                                                tableparts.Cell(3, 5).SetFont(FontTableHead);
                                                tableparts.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, curX, curY, 500, 200));
                                                rcptpage.Add(addtabletopage1);

                                                #endregion

                                                #region Table 2
                                                //Table2 Format

                                                int rows = 1;
                                                selectQuery = "";

                                                //Table2 Data

                                                int sno = 0;
                                                int indx = 0;
                                                double totalamt = 0;
                                                double balanamt = 0;
                                                double curpaid = 0;
                                                // double paidamount = 0;


                                                string selHeadersQ = string.Empty;
                                                DataSet dsHeaders = new DataSet();

                                                if (rcptType == "1" || rcptType == "2")
                                                {
                                                    string StudStream = string.Empty;

                                                    DataSet dsStr = new DataSet();
                                                    dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                                    if (dsStr.Tables.Count > 0)
                                                    {
                                                        if (dsStr.Tables[0].Rows.Count > 0)
                                                        {
                                                            StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                                        }
                                                    }

                                                    selHeadersQ = " select sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d ,FS_ChlGroupHeaderSettings f,FT_FeeAllot A,FM_LedgerMaster l where d.HeaderFK =f.HeaderFK and D.LedgerFK=l.LedgerPK  and  d.LedgerFK=a.LedgerFK and d.App_No=a.App_No and A.FeeCategory =D.FeeCategory  and    transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'  ";
                                                    if (StudStream != "")
                                                    {
                                                        selHeadersQ += " and f.stream='" + StudStream + "' ";
                                                    }
                                                    selHeadersQ += "   group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory ,D.DailyTransPk,A.Feeallotpk ";
                                                }
                                                else if (rcptType == "3")
                                                {
                                                    selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory ,D.DailyTransPk,A.Feeallotpk";
                                                }
                                                else if (rcptType == "4")
                                                {
                                                    selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.LedgerFK=a.LedgerFK  and d.FeeCategory =A.FeeCategory and  d.App_No=a.App_No and transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";
                                                }

                                                if (selHeadersQ != string.Empty)
                                                {


                                                    string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                                    dsHeaders.Clear();
                                                    dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                                    if (dsHeaders.Tables.Count > 0)
                                                    {
                                                        if (dsHeaders.Tables[0].Rows.Count > 0)
                                                        {
                                                            rows += dsHeaders.Tables[0].Rows.Count;
                                                            PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, 6, 5);
                                                            tableparts1.VisibleHeaders = false;
                                                            //tableparts1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                                            tableparts1.Cell(0, 0).SetContent("S.No");
                                                            tableparts1.Cell(0, 0).SetFont(FontTableHead);
                                                            tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                                                            tableparts1.Cell(0, 1).SetContent("Particulars");
                                                            tableparts1.Cell(0, 1).SetFont(FontTableHead);
                                                            tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            tableparts1.Cell(0, 1).ColSpan = 4;

                                                            tableparts1.Cell(0, 5).SetContent("Amount (Rs)");
                                                            tableparts1.Cell(0, 5).SetFont(FontTableHead);
                                                            tableparts1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);


                                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                                            {
                                                                string disphdr = string.Empty;
                                                                double allotamt0 = 0;
                                                                double deductAmt0 = 0;
                                                                double totalAmt0 = 0;
                                                                double paidAmt0 = 0;
                                                                double balAmt0 = 0;
                                                                double creditAmt0 = 0;

                                                                creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);

                                                                totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                                //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);


                                                                //paidAmt0 = totalAmt0 - balAmt0;
                                                                deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                                disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                                string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                                string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                                string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);
                                                                string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                                paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                                #region Monthwise
                                                                string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                                string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                                                int monWisemon = 0;
                                                                int monWiseYea = 0;
                                                                string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                                                int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                                                if (monWisemon > 0 && monWiseYea > 0)
                                                                {
                                                                    string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                                    DataSet dsMonwise = new DataSet();
                                                                    dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                                    if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                                        paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                                        disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                                        balAmt0 = totalAmt0 - paidAmt0;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                }
                                                                #endregion

                                                                //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                                                sno++;
                                                                indx++;
                                                                totalamt += Convert.ToDouble(totalAmt0);
                                                                balanamt += Convert.ToDouble(balAmt0);
                                                                curpaid += Convert.ToDouble(creditAmt0);

                                                                deductionamt += Convert.ToDouble(deductAmt0);

                                                                tableparts1.Cell(indx, 0).SetContent(sno);
                                                                tableparts1.Cell(indx, 0).SetFont(FontTable);
                                                                tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                                                                tableparts1.Cell(indx, 1).SetContent(disphdr);
                                                                tableparts1.Cell(indx, 1).SetFont(FontTable);
                                                                tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                tableparts1.Cell(indx, 1).ColSpan = 4;

                                                                tableparts1.Cell(indx, 5).SetContent(creditAmt0);
                                                                tableparts1.Cell(indx, 5).SetFont(FontTable);
                                                                tableparts1.Cell(indx, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                                createPDFOK = true;
                                                            }


                                                            curY += 5 + (int)addtabletopage1.Area.Height;

                                                            PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 40, curY, 500, 600));
                                                            rcptpage.Add(addtabletopage2);

                                                            #region Table 3
                                                            //Table3 Format
                                                            PdfTable tableparts2 = recptDoc.NewTable(FontTableHead, 4, 6, 5);
                                                            tableparts2.VisibleHeaders = false;
                                                            // tableparts2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                                            //Table3 Header              
                                                            decimal totalamount = (decimal)curpaid;

                                                            tableparts2.Cell(0, 0).SetContent("Received " + DecimalToWords(totalamount).ToString() + " Rupees Only");
                                                            tableparts2.Cell(0, 0).SetFont(FontTableHead);
                                                            tableparts2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            tableparts2.Cell(0, 0).ColSpan = 4;

                                                            tableparts2.Cell(0, 4).SetContent("Total Amount");
                                                            tableparts2.Cell(0, 4).SetFont(FontTableHead);
                                                            tableparts2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                            tableparts2.Cell(0, 5).SetContent(curpaid);
                                                            tableparts2.Cell(0, 5).SetFont(FontTableHead);
                                                            tableparts2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                            tableparts2.Cell(3, 3).SetContent("CASHIER / ACCOUNTANT");
                                                            tableparts2.Cell(3, 3).SetFont(FontTableHead);
                                                            tableparts2.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                                                            tableparts2.Cell(3, 3).ColSpan = 3;


                                                            curY += 5 + (int)addtabletopage2.Area.Height;
                                                            PdfTablePage addtabletopage3 = tableparts2.CreateTablePage(new PdfArea(recptDoc, 40, curY, 500, 100));
                                                            rcptpage.Add(addtabletopage3);
                                                            #endregion

                                                            rcptpage.SaveToDocument();

                                                            //save changes
                                                            PdfPage rcptpageOf = rcptpage.CreateCopy();
                                                            PdfPage rcptpageTran = rcptpage.CreateCopy();
                                                            if (officopy != 0)
                                                            {
                                                                rcptpageOf.SaveToDocument();
                                                            }

                                                            if (transCopy != 0)
                                                            {
                                                                rcptpageTran.SaveToDocument();
                                                            }


                                                        }
                                                    }

                                                }
                                                #endregion
                                                #endregion
                                            }
                                            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
                                            finally
                                            {

                                            }

                                            createPDFOK = true;
                                        }
                                        else
                                        {
                                            imgAlert.Visible = true;
                                            lbl_alert.Text = "No Records Found";
                                        }
                                    }
                                    else
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "No Records Found";
                                    }
                                }

                            }
                        }
                        #endregion

                        #region To print the Receipt
                        if (createPDFOK)
                        {
                            //Response Write
                            string appPath = HttpContext.Current.Server.MapPath("~");
                            if (appPath != "")
                            {
                                string szPath = appPath + "/Report/";
                                string szFile = "Receipt" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                Response.Buffer = true;
                                Response.Clear();
                                recptDoc.SaveToFile(szPath + szFile);
                                //Response.ClearHeaders();
                                //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                //Response.ContentType = "application/pdf";
                                //Response.WriteFile(szPath + szFile);

                                Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Receipt Generated";
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Server Path Not Found";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Receipt Cannot Be Generated";
                        }
                        #endregion
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Add Print Settings";
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Add Print Settings";
                }
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Receipt";
        }
    }
    public void btnChlnDuplicate3_Click()
    {
        //Vellammal
        if (checkedOK())
        {
            FpSpread1.SaveChanges();
            try
            {
                string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
                DataSet dsPri = new DataSet();
                dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
                if (dsPri.Tables.Count > 0)
                {
                    if (dsPri.Tables[0].Rows.Count > 0)
                    {
                        string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

                        //Document Settings
                        PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.InCentimeters(18, 15.2));

                        Font Fontboldhead = new Font("Arial", 10, FontStyle.Bold);
                        Font FontTableHead = new Font("Arial", 10, FontStyle.Bold);
                        Font FontTable = new Font("Arial", 10, FontStyle.Bold);
                        Font tamilFont = new Font("AMUDHAM.TTF", 10, FontStyle.Regular);

                        bool createPDFOK = false;


                        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                            if (check == 1)
                            {
                                string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                                string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

                                string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                                if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                                {
                                    string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype,DDno,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,DDBankCode from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                                    DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                                    if (dsDet.Tables.Count > 0)
                                    {
                                        if (dsDet.Tables[0].Rows.Count > 0)
                                        {
                                            string rollno = string.Empty;
                                            string studname = string.Empty;
                                            string receiptno = string.Empty;
                                            string name = string.Empty;

                                            string app_formno = string.Empty;
                                            string appnoNew = string.Empty;
                                            string Regno = string.Empty;

                                            string batchYrSem = string.Empty;

                                            string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                                            string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                                            string mode = string.Empty;
                                            string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                                            string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                                            string DDDate = Convert.ToString(dsDet.Tables[0].Rows[0]["DDDate"]);
                                            string DDBranch = Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankBranch"]);
                                            string DDNo = Convert.ToString(dsDet.Tables[0].Rows[0]["DDno"]);
                                            string DDBankName = d2.GetFunction("select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode='" + Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankCode"]) + "' ");

                                            switch (paymode)
                                            {
                                                case "1":
                                                    mode = "Cash";
                                                    break;
                                                case "2":
                                                    mode = "Cheque";
                                                    break;
                                                case "3":
                                                    mode = "DD";
                                                    break;
                                                case "4":
                                                    mode = "Challan";
                                                    break;
                                                case "5":
                                                    mode = "Online";
                                                    break;
                                                default:
                                                    mode = "Others";
                                                    break;
                                            }


                                            //string queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            string queryRollApp;

                                            if (ddlBefAfteAdm.SelectedIndex == 0)
                                            {
                                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            }
                                            else
                                            {
                                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name  from applyn where app_no='" + AppNo + "'";
                                            }
                                            DataSet dsRollApp = new DataSet();
                                            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                                            if (dsRollApp.Tables.Count > 0)
                                            {
                                                if (dsRollApp.Tables[0].Rows.Count > 0)
                                                {
                                                    rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                                    app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                                    Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                                    studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                                }
                                            }
                                            name = rollno + "-" + studname;

                                            //Print Region
                                            #region Print Option For Receipt
                                            try
                                            {
                                                PdfPage rcptpage = recptDoc.NewPage();
                                                //Fields to print

                                                #region Settings Input
                                                //Header Div Values

                                                byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                                                byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                                                byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);

                                                #endregion

                                                #region Students Input
                                                //string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL(type,'') as type,r.Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.app_no='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
                                                string colquery = "";
                                                if (ddlBefAfteAdm.SelectedIndex == 0)
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,isnull(r.sections,'') as Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                else
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,'' Sections from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                string collegename = "";
                                                string add1 = "";
                                                string add2 = "";
                                                string add3 = "";
                                                string univ = "";
                                                string deg = "";
                                                string cursem = "";
                                                string batyr = "";
                                                string seatty = "";
                                                string board = "";
                                                string mothe = "";
                                                string fathe = "";
                                                string stream = "";
                                                double deductionamt = 0;
                                                string fgraduate = d2.GetFunction("select isnull(first_graduate,0) as first_graduate  from applyn where app_no='" + appnoNew + "'");
                                                if (fgraduate == "0")
                                                {
                                                    fgraduate = string.Empty;
                                                }
                                                else
                                                {
                                                    fgraduate = " FG ";
                                                }
                                                string sec = string.Empty;
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(colquery, "Text");
                                                if (ds.Tables.Count > 0)
                                                {
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                                                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                                    }
                                                    if (ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        //if (degACR == 0)
                                                        //{
                                                        //deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                                        //}
                                                        //else
                                                        //{
                                                        deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                        //}
                                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                                        board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                        mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                                        stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                                        sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                                                    }
                                                }
                                                string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                                try
                                                {
                                                    acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                                }
                                                catch { }
                                                #endregion

                                                #region Table 1
                                                //Table1 Format 

                                                PdfTable tableparts = recptDoc.NewTable(FontTableHead, 5, 6, 2);
                                                tableparts.VisibleHeaders = false;

                                                tableparts.Rows[0].SetRowHeight(10);
                                                tableparts.Rows[1].SetRowHeight(30);
                                                tableparts.Rows[2].SetRowHeight(20);
                                                tableparts.Rows[3].SetRowHeight(20);
                                                tableparts.Rows[4].SetRowHeight(10);
                                                tableparts.Rows[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Rows[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Rows[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Rows[3].SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Rows[4].SetContentAlignment(ContentAlignment.MiddleLeft);
                                                //Table1 Data
                                                //Line 1
                                                int rowindextbl1 = 0;

                                                tableparts.Cell(rowindextbl1, 0).SetContent("");
                                                tableparts.Cell(rowindextbl1, 0).SetFont(FontTable);
                                                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tableparts.Rows[rowindextbl1].SetRowHeight(10);
                                                rowindextbl1++;

                                                tableparts.Cell(rowindextbl1, 0).SetContent("Roll No");
                                                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 1).SetContent(": " + rollno);
                                                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 1).ColSpan = 2;

                                                tableparts.Cell(rowindextbl1, 4).SetContent("Receipt No");
                                                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 5).SetContent(": " + recptNo);
                                                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                //Line2
                                                rowindextbl1++;
                                                tableparts.Cell(rowindextbl1, 0).SetContent("Name");
                                                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(rowindextbl1, 1).SetContent(": " + studname.ToUpper());
                                                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 1).ColSpan = 3;

                                                tableparts.Cell(rowindextbl1, 4).SetContent("Date");
                                                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(rowindextbl1, 5).SetContent(": " + recptDt);
                                                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                //Line3
                                                rowindextbl1++;

                                                tableparts.Cell(rowindextbl1, 0).SetContent("Year/ Major");
                                                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                tableparts.Cell(rowindextbl1, 1).SetContent(": " + romanLetter(returnYearforSem(cursem)) + " / " + deg.Split('-')[1].ToUpper() + sec.ToUpper() + fgraduate.ToUpper());
                                                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 1).ColSpan = 3;

                                                tableparts.Cell(rowindextbl1, 4).SetContent("Term");
                                                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 5).SetContent(": " + acaYear);
                                                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                rowindextbl1++;
                                                tableparts.Cell(rowindextbl1, 0).SetContent("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                                tableparts.Cell(rowindextbl1, 0).SetFont(FontTable);
                                                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tableparts.Rows[rowindextbl1].SetRowHeight(10);


                                                PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, 10, 10, 480, 150));
                                                rcptpage.Add(addtabletopage1);

                                                #endregion

                                                #region Table 2
                                                //Table2 Format

                                                int rows = 0;
                                                selectQuery = "";

                                                //Table2 Data

                                                int sno = 0;
                                                int indx = 1;
                                                double totalamt = 0;
                                                double balanamt = 0;
                                                double curpaid = 0;
                                                // double paidamount = 0;

                                                string selHeadersQ = string.Empty;
                                                DataSet dsHeaders = new DataSet();

                                                if (rcptType == "1" || rcptType == "2")
                                                {
                                                    string StudStream = string.Empty;

                                                    DataSet dsStr = new DataSet();
                                                    dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                                    if (dsStr.Tables.Count > 0)
                                                    {
                                                        if (dsStr.Tables[0].Rows.Count > 0)
                                                        {
                                                            StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                                        }
                                                    }

                                                    selHeadersQ = " select sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d ,FS_ChlGroupHeaderSettings f,FT_FeeAllot A,FM_LedgerMaster l where d.HeaderFK =f.HeaderFK and D.LedgerFK=l.LedgerPK  and  d.LedgerFK=a.LedgerFK and d.App_No=a.App_No and A.FeeCategory =D.FeeCategory  and    transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'  ";
                                                    if (StudStream != "")
                                                    {
                                                        selHeadersQ += " and f.stream='" + StudStream + "' ";
                                                    }
                                                    selHeadersQ += "   group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory ,D.DailyTransPk,A.Feeallotpk ";
                                                }
                                                else if (rcptType == "3")
                                                {
                                                    selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory  and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                                }
                                                else if (rcptType == "4")
                                                {
                                                    selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.FeeCategory =A.FeeCategory and d.LedgerFK=a.LedgerFK and  d.App_No=a.App_No and transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";
                                                }

                                                if (selHeadersQ != string.Empty)
                                                {


                                                    string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                                    dsHeaders.Clear();
                                                    dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                                    if (dsHeaders.Tables.Count > 0)
                                                    {
                                                        if (dsHeaders.Tables[0].Rows.Count > 0)
                                                        {
                                                            rows += dsHeaders.Tables[0].Rows.Count;
                                                            PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows + 5, 4, 1);
                                                            // tableparts1.SetBorders(Color.Black, 1, BorderType.Rows);
                                                            tableparts1.VisibleHeaders = false;
                                                            tableparts1.Columns[0].SetWidth(57);
                                                            tableparts1.Columns[1].SetWidth(340);
                                                            tableparts1.Columns[2].SetWidth(85);
                                                            tableparts1.Columns[3].SetWidth(28);

                                                            tableparts1.Cell(0, 0).SetContent("S.No");
                                                            tableparts1.Cell(0, 0).SetFont(FontTable);
                                                            tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);



                                                            tableparts1.Cell(0, 1).SetContent("Particulars");
                                                            tableparts1.Cell(0, 1).SetFont(FontTable);
                                                            tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            //tableparts1.Cell(indx, 1).ColSpan = 4;

                                                            tableparts1.Cell(0, 2).SetContent("Rs.");
                                                            tableparts1.Cell(0, 2).SetFont(FontTable);
                                                            tableparts1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                                            tableparts1.Cell(0, 3).SetContent("Ps.");
                                                            tableparts1.Cell(0, 3).SetFont(FontTable);
                                                            tableparts1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            tableparts1.Rows[0].SetRowHeight(20);
                                                            tableparts1.Rows[1].SetRowHeight(10);

                                                            foreach (PdfCell pr in tableparts1.CellRange(indx, 0, indx, 0).Cells)
                                                            {
                                                                pr.ColSpan = 4;
                                                            }

                                                            tableparts1.Cell(indx, 0).SetContent("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                                                            tableparts1.Cell(indx, 0).SetFont(FontTable);
                                                            tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            tableparts1.Rows[indx].SetRowHeight(10);
                                                            indx++;

                                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                                            {
                                                                string disphdr = string.Empty;
                                                                double allotamt0 = 0;
                                                                double deductAmt0 = 0;
                                                                double totalAmt0 = 0;
                                                                double paidAmt0 = 0;
                                                                double balAmt0 = 0;
                                                                double creditAmt0 = 0;

                                                                creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);

                                                                totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                                //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);


                                                                //paidAmt0 = totalAmt0 - balAmt0;
                                                                deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                                disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                                string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                                string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                                string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);
                                                                string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                                paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                                #region Monthwise
                                                                string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                                string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                                                int monWisemon = 0;
                                                                int monWiseYea = 0;
                                                                string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                                                int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                                                if (monWisemon > 0 && monWiseYea > 0)
                                                                {
                                                                    string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                                    DataSet dsMonwise = new DataSet();
                                                                    dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                                    if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                                        paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                                        disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                                        balAmt0 = totalAmt0 - paidAmt0;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                }
                                                                #endregion

                                                                //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                                                sno++;

                                                                totalamt += Convert.ToDouble(totalAmt0);
                                                                balanamt += Convert.ToDouble(balAmt0);
                                                                curpaid += Convert.ToDouble(creditAmt0);

                                                                deductionamt += Convert.ToDouble(deductAmt0);

                                                                tableparts1.Cell(indx, 0).SetContent(sno);
                                                                tableparts1.Cell(indx, 0).SetFont(FontTable);
                                                                tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                                                                tableparts1.Cell(indx, 1).SetContent(disphdr);
                                                                tableparts1.Cell(indx, 1).SetFont(FontTable);
                                                                tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                                //tableparts1.Cell(indx, 1).ColSpan = 4;

                                                                tableparts1.Cell(indx, 2).SetContent(returnIntegerPart(creditAmt0));
                                                                tableparts1.Cell(indx, 2).SetFont(FontTable);
                                                                tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                tableparts1.Cell(indx, 3).SetContent(returnDecimalPart(creditAmt0));
                                                                tableparts1.Cell(indx, 3).SetFont(FontTable);
                                                                tableparts1.Cell(indx, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                indx++;
                                                                createPDFOK = true;
                                                            }

                                                            foreach (PdfCell pr in tableparts1.CellRange(indx, 0, indx, 0).Cells)
                                                            {
                                                                pr.ColSpan = 4;
                                                            }

                                                            tableparts1.Cell(indx, 0).SetContent("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                                                            tableparts1.Cell(indx, 0).SetFont(FontTable);
                                                            tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            tableparts1.Rows[indx].SetRowHeight(10);
                                                            indx++;
                                                            decimal totalamount = (decimal)curpaid;
                                                            tableparts1.Cell(indx, 1).SetContent("Total");
                                                            tableparts1.Cell(indx, 1).SetFont(FontTable);
                                                            tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            tableparts1.Cell(indx, 2).SetContent("" + returnIntegerPart((double)totalamount));
                                                            tableparts1.Cell(indx, 2).SetFont(FontTable);
                                                            tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                                            tableparts1.Cell(indx, 3).SetContent(returnDecimalPart((double)totalamount));
                                                            tableparts1.Cell(indx, 3).SetFont(FontTable);
                                                            tableparts1.Cell(indx, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            string endstatement = "\n" + DecimalToWords(totalamount) + " Rupees Only." + "\n\nPaid by " + mode + " Rs." + totalamount.ToString() + "/-.";
                                                            string finalstrig = "";
                                                            string examt = d2.GetFunction("   select isnull(ExcessAmt,0) from FT_ExcessDet where Dailytranscode = '" + recptNo + "'");
                                                            if (examt.Trim() != "" && examt.Trim() != "0")
                                                            {
                                                                finalstrig = "\nExcess Amount  : " + examt;
                                                            }
                                                            if (paymode == "2")
                                                            {
                                                                finalstrig = finalstrig + "\nCheque : " + DDNo + "         Date  : " + DDDate;
                                                                finalstrig = finalstrig + "\nBank Name  : " + DDBankName;
                                                            }
                                                            if (paymode == "3")
                                                            {
                                                                finalstrig = finalstrig + "\nDD : " + DDNo + "         Date  : " + DDDate;
                                                                finalstrig = finalstrig + "\nBank Name  : " + DDBankName;
                                                            }
                                                            endstatement = endstatement + finalstrig;

                                                            tableparts1.Cell(indx + 1, 0).SetContent(endstatement);
                                                            tableparts1.Cell(indx + 1, 0).SetFont(FontTable);
                                                            tableparts1.Cell(indx + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            tableparts1.Cell(indx + 1, 0).ColSpan = 3;


                                                            PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 10, 80, 480, 500));
                                                            rcptpage.Add(addtabletopage2);

                                                            rcptpage.SaveToDocument();

                                                            //save changes
                                                            PdfPage rcptpageOf = rcptpage.CreateCopy();
                                                            PdfPage rcptpageTran = rcptpage.CreateCopy();
                                                            if (officopy != 0)
                                                            {
                                                                rcptpageOf.SaveToDocument();
                                                            }

                                                            if (transCopy != 0)
                                                            {
                                                                rcptpageTran.SaveToDocument();
                                                            }


                                                        }
                                                    }

                                                }


                                                #endregion
                                            }
                                            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
                                            finally
                                            {

                                            }
                                            createPDFOK = true;
                                        }
                                        else
                                        {
                                            imgAlert.Visible = true;
                                            lbl_alert.Text = "No Records Found";
                                        }
                                    }
                                    else
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "No Records Found";
                                    }
                                }

                            }
                        }
                                            #endregion
                        #region To print the Receipt
                        if (createPDFOK)
                        {
                            //Response Write
                            string appPath = HttpContext.Current.Server.MapPath("~");
                            if (appPath != "")
                            {
                                string szPath = appPath + "/Report/";
                                string szFile = "Receipt" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                Response.Buffer = true;
                                Response.Clear();
                                recptDoc.SaveToFile(szPath + szFile);
                                //Response.ClearHeaders();
                                //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                //Response.ContentType = "application/pdf";
                                //Response.WriteFile(szPath + szFile);

                                Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Receipt Generated";
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Server Path Not Found";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Receipt Cannot Be Generated";
                        }
                        #endregion
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Add Print Settings";
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Add Print Settings";
                }
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Receipt";
        }
    }
    public void btnChlnDuplicate4_Click(string dupReceipt)
    {
        //Jeppiar
        if (checkedOK())
        {
            FpSpread1.SaveChanges();
            try
            {
                contentDiv.InnerHtml = "";
                StringBuilder sbHtml = new StringBuilder();
                string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
                DataSet dsPri = new DataSet();
                dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
                if (dsPri.Tables.Count > 0)
                {
                    if (dsPri.Tables[0].Rows.Count > 0)
                    {
                        string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

                        //Document Settings

                        bool createPDFOK = false;


                        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            sbHtml.Clear();
                            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                            if (check == 1)
                            {
                                string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                                string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

                                string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                                if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                                {
                                    string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype,DDno,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,DDBankCode from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                                    DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                                    if (dsDet.Tables.Count > 0)
                                    {
                                        if (dsDet.Tables[0].Rows.Count > 0)
                                        {
                                            string rollno = string.Empty;
                                            string studname = string.Empty;
                                            string receiptno = string.Empty;
                                            string name = string.Empty;

                                            string app_formno = string.Empty;
                                            string appnoNew = string.Empty;
                                            string Regno = string.Empty;

                                            string batchYrSem = string.Empty;

                                            string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                                            string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                                            string mode = string.Empty;
                                            string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                                            string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                                            string DDDate = Convert.ToString(dsDet.Tables[0].Rows[0]["DDDate"]);
                                            string DDBranch = Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankBranch"]);
                                            string DDNo = Convert.ToString(dsDet.Tables[0].Rows[0]["DDno"]);
                                            string DDBankName = d2.GetFunction("select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode='" + Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankCode"]) + "' ");

                                            switch (paymode)
                                            {
                                                case "1":
                                                    mode = "Cash";
                                                    break;
                                                case "2":
                                                    mode = "Cheque";
                                                    break;
                                                case "3":
                                                    mode = "DD";
                                                    break;
                                                case "4":
                                                    mode = "Challan";
                                                    break;
                                                case "5":
                                                    mode = "Online";
                                                    break;
                                                default:
                                                    mode = "Others";
                                                    break;
                                            }


                                            //string queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            string queryRollApp;

                                            if (ddlBefAfteAdm.SelectedIndex == 0)
                                            {
                                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            }
                                            else
                                            {
                                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name  from applyn where app_no='" + AppNo + "'";
                                            }
                                            DataSet dsRollApp = new DataSet();
                                            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                                            if (dsRollApp.Tables.Count > 0)
                                            {
                                                if (dsRollApp.Tables[0].Rows.Count > 0)
                                                {
                                                    rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                                    app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                                    Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                                    studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                                }
                                            }
                                            name = rollno + "-" + studname;

                                            //Print Region
                                            #region Print Option For Receipt
                                            try
                                            {
                                                sbHtml.Append("<div style='padding-left:5px;height: 900px; width:595px;'><center><div style='padding-left:130px;height: 763.5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 574.5px; ' class='classBold12'><tr><td>");
                                                //Fields to print

                                                #region Settings Input
                                                //Header Div Values

                                                byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                                                byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                                                byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);

                                                #endregion

                                                #region Students Input
                                                //string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL(type,'') as type,r.Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.app_no='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
                                                string colquery = "";
                                                if (ddlBefAfteAdm.SelectedIndex == 0)
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                else
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                string collegename = "";
                                                string add1 = "";
                                                string add2 = "";
                                                string add3 = "";
                                                string univ = "";
                                                string deg = "";
                                                string cursem = "";
                                                string batyr = "";
                                                string seatty = "";
                                                string board = "";
                                                string mothe = "";
                                                string fathe = "";
                                                string stream = "";
                                                string curYr = "";
                                                double deductionamt = 0;
                                                string fgraduate = d2.GetFunction("select isnull(first_graduate,0) as first_graduate  from applyn where app_no='" + appnoNew + "'");
                                                if (fgraduate == "0")
                                                {
                                                    fgraduate = string.Empty;
                                                }
                                                else
                                                {
                                                    fgraduate = " FG ";
                                                }
                                                string sec = string.Empty;
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(colquery, "Text");
                                                if (ds.Tables.Count > 0)
                                                {
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                                                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                                    }
                                                    if (ds.Tables[1].Rows.Count > 1)
                                                    {
                                                        //if (degACR == 0)
                                                        //{
                                                        //deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                                        //}
                                                        //else
                                                        //{
                                                        deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                        //}
                                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                                        board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                        mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                                        stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                                        sec = " ";// +Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);

                                                        curYr = romanLetter(returnYearforSem(cursem)) + " Year ";
                                                        //deg = curYr + deg;
                                                    }
                                                }
                                                string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                                try
                                                {
                                                    acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                                }
                                                catch { }
                                                #endregion



                                                #region Table 2
                                                //Table2 Format

                                                int rows = 0;
                                                selectQuery = "";

                                                //Table2 Data

                                                int sno = 0;
                                                int indx = 1;
                                                double totalamt = 0;
                                                double balanamt = 0;
                                                double curpaid = 0;
                                                // double paidamount = 0;

                                                string selHeadersQ = string.Empty;
                                                DataSet dsHeaders = new DataSet();

                                                if (rcptType == "1" || rcptType == "2")
                                                {
                                                    string StudStream = string.Empty;

                                                    DataSet dsStr = new DataSet();
                                                    dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                                    if (dsStr.Tables.Count > 0)
                                                    {
                                                        if (dsStr.Tables[0].Rows.Count > 0)
                                                        {
                                                            StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                                        }
                                                    }

                                                    selHeadersQ = " select sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d ,FS_ChlGroupHeaderSettings f,FT_FeeAllot A,FM_LedgerMaster l where d.HeaderFK =f.HeaderFK and D.LedgerFK=l.LedgerPK  and  d.LedgerFK=a.LedgerFK and d.App_No=a.App_No and A.FeeCategory =D.FeeCategory  and    transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.Iscanceled,0)=0  ";
                                                    if (StudStream != "")
                                                    {
                                                        selHeadersQ += " and f.stream='" + StudStream + "' ";
                                                    }
                                                    selHeadersQ += "   group by l.LedgerName, D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk, A.Feeallotpk ";
                                                }
                                                else if (rcptType == "3")
                                                {
                                                    selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.App_No=a.App_No  and d.FeeCategory =A.FeeCategory  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'  and isnull(d.Iscanceled,0)=0  group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                                }
                                                else if (rcptType == "4")
                                                {
                                                    selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.LedgerFK=a.LedgerFK and  d.App_No=a.App_No  and d.FeeCategory =A.FeeCategory and transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'  and isnull(d.Iscanceled,0)=0  group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";
                                                }

                                                if (selHeadersQ != string.Empty)
                                                {


                                                    string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                                    dsHeaders.Clear();
                                                    dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                                    if (dsHeaders.Tables.Count > 0)
                                                    {
                                                        if (dsHeaders.Tables[0].Rows.Count > 0)
                                                        {
                                                            string feecatName = d2.GetFunction("select textval from TextValTable where TextCode=" + Convert.ToString(dsHeaders.Tables[0].Rows[0]["Feecategory"]) + " and college_code=" + collegecode1 + "").Trim();
                                                            try
                                                            {
                                                                deg = romanLetter(feecatName.Split(' ')[0]) + " " + feecatName.Split(' ')[1] + " " + deg;
                                                            }
                                                            catch { deg = curYr + deg; }

                                                            sbHtml.Append("<table class='classBold12' style='width:460px; height:60px;' cellpadding='7'><tr><td style='padding-left:260px; padding-top:70px; text-align:right;'>" + dupReceipt + "<BR>" + recptDt + "</td></tr><tr><td style='padding-left:0px;'>Receipt No. " + recptNo + "</td></tr><tr><td style='padding-left:150px;padding-top:-650px;'>" + studname.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + rollno.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + deg.ToUpper() + "</td></tr></table>");

                                                            rows += dsHeaders.Tables[0].Rows.Count;

                                                            sbHtml.Append("<div style='width:380px; height:215px; padding-left:80px; padding-top:80px;'><table class='classBold12' cellpadding='4' >");

                                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                                            {
                                                                string disphdr = string.Empty;
                                                                double allotamt0 = 0;
                                                                double deductAmt0 = 0;
                                                                double totalAmt0 = 0;
                                                                double paidAmt0 = 0;
                                                                double balAmt0 = 0;
                                                                double creditAmt0 = 0;

                                                                creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);

                                                                totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                                //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);


                                                                //paidAmt0 = totalAmt0 - balAmt0;
                                                                deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                                disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                                string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                                string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                                string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);
                                                                string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                                paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                                #region Monthwise
                                                                string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                                string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                                                int monWisemon = 0;
                                                                int monWiseYea = 0;
                                                                string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                                                int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                                                if (monWisemon > 0 && monWiseYea > 0)
                                                                {
                                                                    string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                                    DataSet dsMonwise = new DataSet();
                                                                    dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                                    if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                                        paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                                        disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                                        balAmt0 = totalAmt0 - paidAmt0;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                }
                                                                #endregion

                                                                //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                                                sno++;

                                                                totalamt += Convert.ToDouble(totalAmt0);
                                                                balanamt += Convert.ToDouble(balAmt0);
                                                                curpaid += Convert.ToDouble(creditAmt0);

                                                                deductionamt += Convert.ToDouble(deductAmt0);

                                                                indx++;
                                                                //235
                                                                //290
                                                                sbHtml.Append("<tr><td style='padding-left:290px; text-align:right; width:60px;'>" + returnIntegerPart(creditAmt0) + "</td><td style=' text-align:right; width:30px;'>&nbsp;&nbsp;&nbsp;&nbsp;" + returnDecimalPart(creditAmt0) + "</td></tr>");

                                                                createPDFOK = true;
                                                            }

                                                            sbHtml.Append("</table></div>");
                                                            sbHtml.Append("<div style='height:60px;' ><table class='classBold12' style='width:380px; padding-left:50px; padding-top:10px;' cellpadding='5'><tr><td style='padding-left:5px;padding-right:70px;'>" + DecimalToWords((decimal)curpaid) + " Rupees Only.</td></tr><tr><td style='padding-left:280px;text-align:right;'><span style='padding-left:50px;text-align:right;width:60px;'>" + returnIntegerPart(curpaid) + "</span></td><td style=' text-align:right; width:30px;padding-left:25px;'>&nbsp;&nbsp;" + returnDecimalPart(curpaid) + "</td></tr></table></div>");
                                                            sbHtml.Append("</td></tr></table></div></center></div>");
                                                            contentDiv.InnerHtml += sbHtml.ToString();
                                                        }
                                                    }

                                                }


                                                #endregion

                                            }
                                            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
                                            finally
                                            {

                                            }
                                            createPDFOK = true;
                                        }
                                        else
                                        {
                                            imgAlert.Visible = true;
                                            lbl_alert.Text = "No Records Found";
                                        }
                                    }
                                    else
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "No Records Found";
                                    }
                                }

                            }
                        }
                                            #endregion
                        #region To print the Receipt
                        if (createPDFOK)
                        {
                            #region New Print
                            //contentDiv.InnerHtml += sbHtml.ToString();
                            contentDiv.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
                            #endregion

                            ////Response Write
                            //string appPath = HttpContext.Current.Server.MapPath("~");
                            //if (appPath != "")
                            //{
                            //    string szPath = appPath + "/Report/";
                            //    string szFile = "Receipt" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                            //    Response.Buffer = true;
                            //    Response.Clear();
                            //    recptDoc.SaveToFile(szPath + szFile);
                            //    //Response.ClearHeaders();
                            //    //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                            //    //Response.ContentType = "application/pdf";
                            //    //Response.WriteFile(szPath + szFile);

                            //    Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");
                            //    imgAlert.Visible = true;
                            //    lbl_alert.Text = "Receipt Generated";
                            //}
                            //else
                            //{
                            //    imgAlert.Visible = true;
                            //    lbl_alert.Text = "Server Path Not Found";
                            //}
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Receipt Cannot Be Generated";
                        }
                        #endregion
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Add Print Settings";
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Add Print Settings";
                }
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Receipt";
        }
    }
    public void btnChlnDuplicate5_Click()
    {
        //PMC
        if (checkedOK())
        {
            FpSpread1.SaveChanges();
            try
            {
                string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
                DataSet dsPri = new DataSet();
                dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
                if (dsPri.Tables.Count > 0 && dsPri.Tables[0].Rows.Count > 0)
                {
                    string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

                    //Document Settings

                    bool createPDFOK = false;

                    contentDiv.InnerHtml = "";
                    StringBuilder sbHtml = new StringBuilder();

                    for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                    {
                        sbHtml.Clear();
                        byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                        if (check == 1)
                        {
                            string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                            string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

                            string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                            if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                            {
                                string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype,DDNo,convert(varchar(10),DDDate,103) as DDDate,(select textval from textvaltable where textcode =DDBankCode) as Bank,DDBankBranch  from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                                DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                                if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                                {
                                    string rollno = string.Empty;
                                    string studname = string.Empty;
                                    string receiptno = string.Empty;
                                    string name = string.Empty;

                                    string app_formno = string.Empty;
                                    string appnoNew = string.Empty;
                                    string Regno = string.Empty;

                                    string batchYrSem = string.Empty;

                                    string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                                    string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                                    string mode = string.Empty;
                                    string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                                    string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                                    string ddNo = Convert.ToString(dsDet.Tables[0].Rows[0]["ddNo"]).Trim();
                                    string modePaySng = string.Empty;
                                    string dddates = Convert.ToString(dsDet.Tables[0].Rows[0]["DDDate"]);
                                    string ddnos = Convert.ToString(dsDet.Tables[0].Rows[0]["DDNo"]);
                                    string ddBanks = Convert.ToString(dsDet.Tables[0].Rows[0]["Bank"]);
                                    string ddBrans = Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankBranch"]);

                                    switch (paymode)
                                    {
                                        case "1":
                                            mode = "Cash";
                                            break;
                                        case "2":
                                            mode = "Cheque - No:" + ddNo;
                                            modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                            mode += modePaySng;
                                            break;
                                        case "3":
                                            mode = "DD - No:" + ddNo;
                                            modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                            mode += modePaySng;
                                            break;
                                        case "4":
                                            mode = "Challan";
                                            break;
                                        case "5":
                                            mode = "Online";
                                            break;
                                        default:
                                            mode = "Others";
                                            break;
                                    }


                                    //string queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                    string queryRollApp;

                                    if (ddlBefAfteAdm.SelectedIndex == 0)
                                    {
                                        queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                    }
                                    else
                                    {
                                        queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name  from applyn where app_no='" + AppNo + "'";
                                    }
                                    DataSet dsRollApp = new DataSet();
                                    dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                                    if (dsRollApp.Tables.Count > 0)
                                    {
                                        if (dsRollApp.Tables[0].Rows.Count > 0)
                                        {
                                            rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                            app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                            appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                            Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                            studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                        }
                                    }
                                    name = rollno + "-" + studname;

                                    //Print Region
                                    #region Print Option For Receipt
                                    try
                                    {
                                        //Fields to print

                                        #region Settings Input
                                        //Header Div Values
                                        byte narration = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsNarration"]);

                                        byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                                        byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                                        byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);

                                        #endregion

                                        #region Students Input

                                        //string colquery = "";
                                        //if (ddlBefAfteAdm.SelectedIndex == 0)
                                        //{
                                        //    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                        //}
                                        //else
                                        //{
                                        //    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                        //}
                                        string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " ";
                                        if (rbl_Memtype.SelectedIndex == 0)
                                        {
                                            if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 3 || ddlBefAfteAdm.SelectedIndex == 1)
                                            {
                                                colquery += " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,'' Sections  from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                            }
                                            else
                                            {
                                                colquery += " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,isnull(r.Sections,'') as Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + " ";
                                            }
                                        }
                                        else if (rbl_Memtype.SelectedIndex == 1)
                                        {
                                            colquery += "  select appl_id ,h.dept_name,h.dept_acronym,h.dept_code,s.staff_name,s.staff_code,a.father_name,t.stftype as staff_type  from staffmaster s,staff_appl_master a,hrdept_master h,stafftrans t,desig_master d where s.appl_no =a.appl_no and s.staff_code =t.staff_code and t.dept_code =h.dept_code and d.desig_code =t.desig_code and s.college_code =h.college_code and d.collegeCode =s.college_code and latestrec ='1' and appl_id ='" + appnoNew + "' and s.college_Code=" + collegecode1 + "  ";
                                        }
                                        else if (rbl_Memtype.SelectedIndex == 2)
                                        {
                                            colquery += " SELECT VendorContactPK, VenContactType, VenContactName, VenContactDesig, VenContactDept, VendorPhoneNo, VendorExtNo, VendorMobileNo, VendorEmail, VendorFK FROM      IM_VendorContactMaster WHERE VendorContactPK = '" + appnoNew + "' ";
                                        }
                                        else if (rbl_Memtype.SelectedIndex == 3)
                                        {
                                            colquery += " SELECT VendorCode,vendorname,VendorMobileNo,VendorAddress,VendorCity,VendorCompName,VendorType  from co_vendormaster  WHERE VendorPK = '" + appnoNew + "' ";

                                        }
                                        string collegename = "";
                                        string add1 = "";
                                        string add2 = "";
                                        string add3 = "";
                                        string univ = "";
                                        string deg = "";
                                        string cursem = "";
                                        string batyr = "";
                                        string seatty = "";
                                        string board = "";
                                        string mothe = "";
                                        string fathe = "";
                                        string stream = "";
                                        double deductionamt = 0;
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(colquery, "Text");
                                        if (ds.Tables.Count > 0)
                                        {
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                                add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                                                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                            }
                                            //if (ds.Tables[1].Rows.Count > 0)
                                            //{
                                            //    //if (degACR == 0)
                                            //    //{
                                            //    //deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                            //    //}
                                            //    //else
                                            //    //{
                                            //    deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                            //    //}
                                            //    cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                            //    batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                            //    seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                            //    board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                            //    mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                            //    fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                            //    stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                            //}
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                if (rbl_Memtype.SelectedIndex == 0)
                                                {
                                                    //if (degACR == 0)
                                                    //{
                                                    // deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                                    //}
                                                    //else
                                                    //{
                                                    deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                    //}
                                                    cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                    batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                    seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                                    board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                    mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                    fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                                    //sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                                                }
                                                else if (rbl_Memtype.SelectedIndex == 1)
                                                {
                                                    //if (degACR == 0)
                                                    //{
                                                    // deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_name"]);
                                                    //}
                                                    //else
                                                    //{
                                                    deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                    //}
                                                    //cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                    //batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                    seatty = Convert.ToString(ds.Tables[1].Rows[0]["staff_type"]);
                                                    //board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                    //mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                    fathe = Convert.ToString(ds.Tables[1].Rows[0]["father_name"]);
                                                    //sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                                                }
                                                else if (rbl_Memtype.SelectedIndex == 2)
                                                {
                                                    deg = " - ";
                                                }
                                                else if (rbl_Memtype.SelectedIndex == 3)
                                                {
                                                    deg = " - ";
                                                }
                                            }
                                        }
                                        string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                        try
                                        {
                                            acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                        }
                                        catch { }
                                        #endregion
                                        string degString = string.Empty;
                                        //Line3
                                        if (rbl_Memtype.SelectedIndex == 0)
                                        {
                                            degString = deg;//.Split('-')[0].ToUpper();
                                        }
                                        else if (rbl_Memtype.SelectedIndex == 1)
                                        {
                                            degString = deg;
                                        }
                                        #region Receipt Header

                                        sbHtml.Append("<div style='height: 536px;width:455px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 450px; ' class='classBold10'><tr><td>");

                                        sbHtml.Append("<div style='text-align:right;padding-top:23px; padding-right:10px; width:450px; height:150px;'><img src='college/right_Logo.jpeg?" + DateTime.Now.Ticks.ToString() + "' height='60px' width='60px'/></div>");

                                        sbHtml.Append("<table style='width:450px; height:102px; ' class='classBold10'><tr><td style='width:150px; text-align:right;'>" + recptNo + "</td><td style='width:150px; text-align:right;'>" + recptDt + "</td><td style='width:150px; text-align:right;'>" + Regno + "</td></tr><tr><td style='width:450px; text-align:left;' colspan='3'><span style='padding-left:90px;'>" + studname.ToUpper() + "</span></td></tr><tr><td style='width:150px; text-align:right;' >" + degString + "</td><td style='width:300px; text-align:right;' colspan='2'>" + romanLetter(returnYearforSem(cursem)) + " Year </td></tr></table>");

                                        #endregion

                                        #region Receipt Body

                                        sbHtml.Append("<div style='width:450px; height:90px; padding-left:2px;padding-top:30px; '><table  class='classBold10'>");
                                        selectQuery = "";

                                        int sno = 0;
                                        int indx = 0;
                                        double totalamt = 0;
                                        double balanamt = 0;
                                        double curpaid = 0;
                                        // double paidamount = 0;


                                        string selHeadersQ = string.Empty;
                                        DataSet dsHeaders = new DataSet();

                                        if (rcptType == "1" || rcptType == "2")
                                        {
                                            string StudStream = string.Empty;

                                            DataSet dsStr = new DataSet();
                                            dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                            if (dsStr.Tables.Count > 0)
                                            {
                                                if (dsStr.Tables[0].Rows.Count > 0)
                                                {
                                                    StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                                }
                                            }

                                            selHeadersQ = " select sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d ,FS_ChlGroupHeaderSettings f,FT_FeeAllot A,FM_LedgerMaster l where d.HeaderFK =f.HeaderFK and D.LedgerFK=l.LedgerPK  and  d.LedgerFK=a.LedgerFK and d.App_No=a.App_No and A.FeeCategory =D.FeeCategory  and    transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'  ";
                                            if (StudStream != "")
                                            {
                                                selHeadersQ += " and f.stream='" + StudStream + "' ";
                                            }
                                            selHeadersQ += "   group by l.LedgerName,D.LedgerFK,d.HeaderFK, D.FeeCategory, A.Feeallotpk, D.DailyTransPk ";
                                        }
                                        else if (rcptType == "3")
                                        {
                                            selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                        }
                                        else if (rcptType == "4")
                                        {
                                            selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.LedgerFK=a.LedgerFK  and d.FeeCategory =A.FeeCategory and  d.App_No=a.App_No and transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";
                                        }

                                        if (selHeadersQ != string.Empty)
                                        {
                                            string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                            dsHeaders.Clear();
                                            dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                            if (dsHeaders.Tables.Count > 0)
                                            {
                                                if (dsHeaders.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                                    {
                                                        string disphdr = string.Empty;
                                                        double allotamt0 = 0;
                                                        double deductAmt0 = 0;
                                                        double totalAmt0 = 0;
                                                        double paidAmt0 = 0;
                                                        double balAmt0 = 0;
                                                        double creditAmt0 = 0;

                                                        creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                                        totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                        //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);

                                                        //paidAmt0 = totalAmt0 - balAmt0;
                                                        deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                        disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                        string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                        string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                        string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);
                                                        string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                        paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                        #region Monthwise
                                                        string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                        string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                                        int monWisemon = 0;
                                                        int monWiseYea = 0;
                                                        string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                        string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                        int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                                        int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                                        if (monWisemon > 0 && monWiseYea > 0)
                                                        {
                                                            string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                            DataSet dsMonwise = new DataSet();
                                                            dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                            if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                            {
                                                                totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                                paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                                disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                                balAmt0 = totalAmt0 - paidAmt0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                        }
                                                        #endregion

                                                        //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                        feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                                        sno++;

                                                        totalamt += Convert.ToDouble(totalAmt0);
                                                        balanamt += Convert.ToDouble(balAmt0);
                                                        curpaid += Convert.ToDouble(creditAmt0);

                                                        deductionamt += Convert.ToDouble(deductAmt0);

                                                        indx++;
                                                        createPDFOK = true;
                                                        sbHtml.Append("<tr><td style='width:340px;'>" + disphdr + "</td><td style='width:110px;text-align:right;'>" + creditAmt0 + "</td><td style='text-align:right;'></td></tr>");
                                                    }
                                                    if (BalanceType == 1)
                                                    {
                                                        balanamt = retBalance(appnoNew);
                                                    }
                                                    sbHtml.Append("</table></div>");
                                                    #region DD Narration
                                                    string modeMulti = string.Empty;
                                                    bool multiCash = false;
                                                    bool multiChk = false;
                                                    bool multiDD = false;

                                                    DataSet dtMulBnkDetails = new DataSet();
                                                    dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  else 'DD' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

                                                    string ddnar = string.Empty;
                                                    string remarks = string.Empty;
                                                    //double modeht = 40;
                                                    if (narration != 0)
                                                    {
                                                        if (dtMulBnkDetails.Tables.Count > 0)
                                                        {
                                                            int sn = 1;
                                                            for (int z = 0; z < dtMulBnkDetails.Tables[0].Rows.Count; z++)
                                                            {
                                                                if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CASH")
                                                                {
                                                                    multiCash = true;
                                                                    continue;
                                                                }
                                                                else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CHEQUE")
                                                                {
                                                                    multiChk = true;
                                                                }
                                                                else
                                                                {
                                                                    multiDD = true;
                                                                }

                                                                ddnar += "\n" + sn.ToString() + ")No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                                sn++;
                                                            }
                                                            //modeht = dtMulBnkDetails.Tables[0].Rows.Count * 15;
                                                            //modeht += 20;

                                                        }
                                                        remarks = d2.GetFunction("select narration from ft_findailytransaction where TransCode='" + recptNo.Trim() + "' and app_no='" + appnoNew + "' and isnull(iscanceled,0)=0");
                                                        if (remarks.Trim() == "0")
                                                            remarks = string.Empty;
                                                        else
                                                        {
                                                            remarks = "\n" + remarks;
                                                        }
                                                    }

                                                    if (multiCash)
                                                    {
                                                        modeMulti += "Cash,";
                                                    }
                                                    if (multiChk)
                                                    {
                                                        modeMulti += "Cheque,";
                                                    }
                                                    if (multiDD)
                                                    {
                                                        modeMulti += "DD";
                                                    }
                                                    modeMulti = modeMulti.TrimEnd(',');
                                                    if (modeMulti != "")
                                                    {
                                                        mode = modeMulti;
                                                    }
                                                    //ddnar += remarks;
                                                    #endregion
                                                    //Mode of Pay
                                                    sbHtml.Append("<table><tr><td style='width:215px; height:75px;padding-left:10px;' class='classBold10'><center>" + mode.ToUpper() + ddnar + "</center></td><td style='width:125px; height:75px;padding-left:10px;' class='classBold10'><center>" + balanamt + "</center></td></tr>");


                                                    double totalamount = curpaid;

                                                    sbHtml.Append("<table style='width:450px; height:75px;padding-left:2px;' class='classBold10'><tr><td style='width:340px;'></td><td style='width:110px;text-align:right;'>" + totalamount + "</td><td style=''></td></tr><tr><td><span style='padding-top:3px;'>Received Rupees " + DecimalToWords((decimal)totalamount) + " Only.</span></td><td colspan='2'></td></tr></table>");
                                                }
                                            }
                                        }
                                        sbHtml.Append("</td></tr></table></div>");
                                        #endregion

                                        contentDiv.InnerHtml += sbHtml.ToString();
                                    }
                                    catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
                                    finally
                                    {
                                    }
                                    createPDFOK = true;
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "No Records Found";
                                }
                            }
                        }
                    }
                                    #endregion
                    #region To print the Receipt
                    if (createPDFOK)
                    {
                        #region New Print
                        //contentDiv.InnerHtml += sbHtml.ToString();
                        contentDiv.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
                        #endregion
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Receipt Cannot Be Generated";
                    }
                    #endregion
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Add Print Settings";
                }
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Receipt";
        }
    }
    public void btnChlnDuplicate6_Click()
    {
        if (checkedOK())
        {

            FpSpread1.SaveChanges();
            try
            {
                string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
                DataSet dsPri = new DataSet();
                dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
                if (dsPri.Tables.Count > 0)
                {
                    if (dsPri.Tables[0].Rows.Count > 0)
                    {
                        string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

                        //Document Settings
                        PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.A4);

                        Font Fontboldhead = new Font("Arial", 10, FontStyle.Bold);
                        Font FontTableHead = new Font("Arial", 7, FontStyle.Bold);
                        Font FontTable = new Font("Arial", 7, FontStyle.Regular);
                        Font tamilFont = new Font("AMUDHAM.TTF", 10, FontStyle.Regular);

                        bool createPDFOK = false;

                        #region For Every selected Receipt
                        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                            if (check == 1)
                            {
                                string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                                string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

                                string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                                if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                                {
                                    string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                                    DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                                    if (dsDet.Tables.Count > 0)
                                    {
                                        if (dsDet.Tables[0].Rows.Count > 0)
                                        {
                                            string rollno = string.Empty;
                                            string studname = string.Empty;
                                            string receiptno = string.Empty;
                                            string name = string.Empty;

                                            string app_formno = string.Empty;
                                            string appnoNew = string.Empty;
                                            string Regno = string.Empty;

                                            string batchYrSem = string.Empty;

                                            string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                                            string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                                            string mode = string.Empty;
                                            string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                                            string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                                            switch (paymode)
                                            {
                                                case "1":
                                                    mode = "Cash";
                                                    break;
                                                case "2":
                                                    mode = "Cheque";
                                                    break;
                                                case "3":
                                                    mode = "DD";
                                                    break;
                                                case "4":
                                                    mode = "Challan";
                                                    break;
                                                case "5":
                                                    mode = "Online";
                                                    break;
                                                default:
                                                    mode = "Others";
                                                    break;
                                            }


                                            //string queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            string queryRollApp;

                                            if (ddlBefAfteAdm.SelectedIndex == 0)
                                            {
                                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            }
                                            else
                                            {
                                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name  from applyn where app_no='" + AppNo + "'";
                                            }
                                            DataSet dsRollApp = new DataSet();
                                            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                                            if (dsRollApp.Tables.Count > 0)
                                            {
                                                if (dsRollApp.Tables[0].Rows.Count > 0)
                                                {
                                                    rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                                    app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                                    Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                                    studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                                }
                                            }
                                            name = rollno + "-" + studname;

                                            //Print Region

                                            try
                                            {

                                                #region Print Option For Receipt

                                                PdfPage rcptpage = recptDoc.NewPage();
                                                //Fields to print

                                                #region Settings Input
                                                //Header Div Values
                                                byte collegeid = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                                                byte address1 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd1"]);
                                                byte address2 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd2"]);
                                                byte address3 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd3"]);
                                                byte city = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeDist"]);
                                                byte state = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeState"]);

                                                byte university = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeUniversity"]);
                                                byte rightLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRightLogo"]);
                                                byte leftLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsLeftLogo"]);
                                                byte time;
                                                if (Convert.ToBoolean(Convert.ToString(dsPri.Tables[0].Rows[0]["IsTime"])))
                                                {
                                                    time = 1;
                                                }
                                                else
                                                {
                                                    time = 0;
                                                }
                                                byte degACR = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsDegreeAcr"]);
                                                byte degNam = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsDegreeName"]);
                                                byte studnam = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudName"]);
                                                byte year = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsYear"]);
                                                byte semester = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSemester"]);
                                                byte regno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRegNo"]);
                                                byte rolno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRollNo"]);
                                                byte admno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAdminNo"]);

                                                byte fathername = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFatherName"]);
                                                byte seattype = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSeatType"]);
                                                //byte setRollAsAdmin = Convert.ToByte(dsPri.Tables[0].Rows[0]["rollas_adm"]);
                                                byte boarding = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsBoarding"]);
                                                byte mothername = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsMontherName"]);
                                                string recptValid = Convert.ToString(dsPri.Tables[0].Rows[0]["ValidDate"]);


                                                //Body Div Values
                                                //byte showAllFees = Convert.ToByte(dsPri.Tables[0].Rows[0]["showallfee"]);
                                                byte allotedAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAllotedAmt"]);
                                                byte fineAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFineAmt"]);
                                                byte balAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsBalanceAmt"]);
                                                byte semOrYear = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSemYear"]);
                                                byte prevPaidAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsPrevPaid"]);
                                                byte excessAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsExcessAmt"]);
                                                // byte totDetails = Convert.ToByte(dsPri.Tables[0].Rows[0]["Total_Details"]);
                                                byte fineInRow = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFineinRow"]);
                                                //byte totWTselectCol = Convert.ToByte(dsPri.Tables[0].Rows[0]["TotalSelCol"]);
                                                byte concession = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsConcession"]);
                                                string concessionValue = string.Empty;
                                                if (concession != 0)
                                                {
                                                    concessionValue = Convert.ToString(dsPri.Tables[0].Rows[0]["ConcessionName"]);
                                                }


                                                //Footer Div Values

                                                byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                                                byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                                                byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);
                                                byte narration = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsNarration"]);
                                                byte deduction = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTotConcession"]);
                                                byte forclgName = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsForCollegeName"]);
                                                byte authSign = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAuthSign"]);
                                                byte validDate = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsValidUpto"]);
                                                string authSignValue = string.Empty;
                                                if (authSign != 0)
                                                {
                                                    authSignValue = Convert.ToString(dsPri.Tables[0].Rows[0]["AuthName"]);

                                                }

                                                byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                                                // byte dispModeWTcash = Convert.ToByte(dsPri.Tables[0].Rows[0]["DisModeWithCash"]);
                                                byte signFile = Convert.ToByte(dsPri.Tables[0].Rows[0]["cashier_sign"]);

                                                //if (signFile != 0)
                                                //{
                                                //if (FileUpload1.HasFile)
                                                //{

                                                //}                                                    
                                                //}


                                                #endregion

                                                #region Students Input
                                                //string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL(type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.app_no='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
                                                string colquery = "";
                                                if (ddlBefAfteAdm.SelectedIndex == 0)
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                else
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                string collegename = "";
                                                string add1 = "";
                                                string add2 = "";
                                                string add3 = "";
                                                string univ = "";
                                                string deg = "";
                                                string cursem = "";
                                                string batyr = "";
                                                string seatty = "";
                                                string board = "";
                                                string mothe = "";
                                                string fathe = "";
                                                string stream = "";
                                                double deductionamt = 0;
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(colquery, "Text");
                                                if (ds.Tables.Count > 0)
                                                {
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                                                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                                    }
                                                    if (ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        if (degACR == 0)
                                                        {
                                                            deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                                        }
                                                        else
                                                        {
                                                            deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                        }
                                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                                        board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                        mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                                        stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                                    }
                                                }
                                                #endregion

                                                int pagelength = 1;
                                                int rectHeight = 800;
                                                if (studOffiCopy == 1)
                                                {
                                                    pagelength = 2;
                                                    rectHeight = 400;
                                                }
                                                PdfPage rcptpageTran = recptDoc.NewPage();
                                                int curY = 10;
                                                int curX = 30;
                                                for (int pl = 1; pl <= pagelength; pl++)
                                                {

                                                    if (pl == 2)
                                                    {
                                                        curY = 420;
                                                    }
                                                    #region Receipt Header


                                                    //Rectangle Border
                                                    PdfArea rectArea = new PdfArea(recptDoc, 10, curY, 570, rectHeight);
                                                    PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
                                                    rcptpage.Add(rectSpace);

                                                    //Header Images
                                                    //Line1
                                                    if (leftLogo != 0)
                                                    {
                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                                                        {
                                                            PdfImage LogoImage = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg"));
                                                            rcptpage.Add(LogoImage, curX, curY, 450);
                                                        }
                                                    }
                                                    if (collegeid != 0)
                                                    {
                                                        curX = 120;
                                                        PdfTextArea clgText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, collegename);
                                                        rcptpage.Add(clgText);
                                                    }
                                                    if (rightLogo != 0)
                                                    {
                                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                                                        {
                                                            curX = 500;
                                                            PdfImage LogoImage1 = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg"));
                                                            rcptpage.Add(LogoImage1, curX, curY, 450);
                                                        }
                                                    }
                                                    //Line2
                                                    if (university != 0)
                                                    {
                                                        curY += 15;
                                                        curX = 120;
                                                        PdfTextArea uniText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, univ);
                                                        rcptpage.Add(uniText);
                                                    }
                                                    //Line3
                                                    if (address1 != 0 || address2 != 0)
                                                    {
                                                        curX = 120;
                                                        curY += 10;
                                                        if (address2 != 0)
                                                        {
                                                            add1 += " " + add2;
                                                        }
                                                        PdfTextArea addText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, add1);
                                                        rcptpage.Add(addText);
                                                    }
                                                    //Line4
                                                    if (address3 != 0)
                                                    {
                                                        curX = 120;
                                                        curY += 10;
                                                        PdfTextArea cityText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, add3);
                                                        rcptpage.Add(cityText);
                                                    }

                                                    curX = 280;
                                                    curY += 35;
                                                    //Text Area For Receipt
                                                    PdfTextArea headingText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX, curY, 20, 30), ContentAlignment.MiddleCenter, "RECEIPT");
                                                    rcptpage.Add(headingText);
                                                    int curX1 = 265;
                                                    int curX2 = 315;
                                                    curY += 21;
                                                    PdfLine underLineRecpt = new PdfLine(recptDoc, new Point(curX1, curY), new Point(curX2, curY), Color.Black, 1);
                                                    rcptpage.Add(underLineRecpt);

                                                    #endregion

                                                    #region Table 1
                                                    //Table1 Format 
                                                    PdfTable tableparts = recptDoc.NewTable(FontTableHead, 4, 9, 5);
                                                    tableparts.VisibleHeaders = false;


                                                    //Table1 Data
                                                    //Line 1
                                                    tableparts.Cell(0, 0).SetContent("Receipt No");
                                                    tableparts.Cell(0, 0).SetFont(FontTableHead);
                                                    tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                    tableparts.Cell(0, 1).SetContent(": " + recptNo);
                                                    tableparts.Cell(0, 1).SetFont(FontTableHead);
                                                    tableparts.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                    if (time != 0)
                                                    {
                                                        tableparts.Cell(0, 3).SetContent("Time");
                                                        tableparts.Cell(0, 3).SetFont(FontTableHead);
                                                        tableparts.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                        tableparts.Cell(0, 4).SetContent(": " + rcptTime);
                                                        tableparts.Cell(0, 4).SetFont(FontTableHead);
                                                        tableparts.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(0, 4).ColSpan = 2;
                                                    }

                                                    tableparts.Cell(0, 6).SetContent("Date");
                                                    tableparts.Cell(0, 6).SetFont(FontTableHead);
                                                    tableparts.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                    tableparts.Cell(0, 7).SetContent(": " + recptDt);
                                                    tableparts.Cell(0, 7).SetFont(FontTableHead);
                                                    tableparts.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    tableparts.Cell(0, 7).ColSpan = 2;

                                                    //Line2
                                                    if (regno != 0)
                                                    {
                                                        tableparts.Cell(1, 0).SetContent("RegNo\n\nName");
                                                        tableparts.Cell(1, 0).SetFont(FontTableHead);
                                                        tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                        tableparts.Cell(1, 1).SetContent(": " + Regno + "\n\n: " + studname);
                                                        tableparts.Cell(1, 1).SetFont(FontTableHead);
                                                        tableparts.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(1, 1).ColSpan = 2;

                                                    }
                                                    else if (rolno != 0)
                                                    {
                                                        tableparts.Cell(1, 0).SetContent("RollNo\n\nName");
                                                        tableparts.Cell(1, 0).SetFont(FontTableHead);
                                                        tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                        tableparts.Cell(1, 1).SetContent(": " + rollno + "\n\n: " + studname);
                                                        tableparts.Cell(1, 1).SetFont(FontTableHead);
                                                        tableparts.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(1, 1).ColSpan = 2;
                                                    }
                                                    else if (admno != 0)
                                                    {
                                                        tableparts.Cell(1, 0).SetContent("AdmissionNo\n\nName");
                                                        tableparts.Cell(1, 0).SetFont(FontTableHead);
                                                        tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                        tableparts.Cell(1, 1).SetContent(": " + app_formno + "\n\n: " + studname);
                                                        tableparts.Cell(1, 1).SetFont(FontTableHead);
                                                        tableparts.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(1, 1).ColSpan = 2;
                                                    }

                                                    if (fathername != 0)
                                                    {
                                                        tableparts.Cell(1, 3).SetContent("Father's Name");
                                                        tableparts.Cell(1, 3).SetFont(FontTableHead);
                                                        tableparts.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                        tableparts.Cell(1, 4).SetContent(": " + fathe);
                                                        tableparts.Cell(1, 4).SetFont(FontTableHead);
                                                        tableparts.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(1, 4).ColSpan = 2;
                                                    }

                                                    if (mothername != 0)
                                                    {
                                                        tableparts.Cell(1, 6).SetContent("Mother's Name");
                                                        tableparts.Cell(1, 6).SetFont(FontTableHead);
                                                        tableparts.Cell(1, 6).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                        tableparts.Cell(1, 7).SetContent(": " + mothe);
                                                        tableparts.Cell(1, 7).SetFont(FontTableHead);
                                                        tableparts.Cell(1, 7).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(1, 7).ColSpan = 2;
                                                    }

                                                    //Line 3
                                                    string batYrSemHead = string.Empty;
                                                    string batYrSemCont = string.Empty;
                                                    if (degACR != 0)
                                                    {
                                                        batYrSemHead = "Degree/";
                                                        batYrSemCont = deg + "/";
                                                    }
                                                    if (year != 0)
                                                    {
                                                        batYrSemHead += "Yr/";
                                                        batYrSemCont += " " + romanLetter(returnYearforSem(cursem)) + "/";

                                                    }
                                                    if (semester != 0)
                                                    {
                                                        batYrSemHead += "Sem";
                                                        batYrSemCont += " " + romanLetter(cursem);
                                                    }
                                                    batYrSemHead = batYrSemHead.TrimEnd('/');
                                                    batYrSemCont = batYrSemCont.TrimEnd('/');

                                                    if (batYrSemHead != "")
                                                    {
                                                        tableparts.Cell(2, 0).SetContent(batYrSemHead);
                                                        tableparts.Cell(2, 0).SetFont(FontTableHead);
                                                        tableparts.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                        tableparts.Cell(2, 1).SetContent(": " + batYrSemCont);
                                                        tableparts.Cell(2, 1).SetFont(FontTableHead);
                                                        tableparts.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(2, 1).ColSpan = 2;
                                                    }

                                                    if (seattype != 0)
                                                    {
                                                        tableparts.Cell(2, 3).SetContent("Seat Type");
                                                        tableparts.Cell(2, 3).SetFont(FontTableHead);
                                                        tableparts.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                        tableparts.Cell(2, 4).SetContent(": " + seatty);
                                                        tableparts.Cell(2, 4).SetFont(FontTableHead);
                                                        tableparts.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(2, 4).ColSpan = 2;
                                                    }
                                                    if (boarding != 0)
                                                    {
                                                        tableparts.Cell(2, 6).SetContent("Boarding");
                                                        tableparts.Cell(2, 6).SetFont(FontTableHead);
                                                        tableparts.Cell(2, 6).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                        tableparts.Cell(2, 7).SetContent(": " + board);
                                                        tableparts.Cell(2, 7).SetFont(FontTableHead);
                                                        tableparts.Cell(2, 7).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts.Cell(2, 7).ColSpan = 2;
                                                    }

                                                    curX = 15;
                                                    curY += 1;
                                                    PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, curX, curY, 600, 200));
                                                    rcptpage.Add(addtabletopage1);

                                                    #endregion

                                                    #region Table 2
                                                    //Table2 Format

                                                    int rows = 1;

                                                    selectQuery = "";

                                                    Hashtable htIndex = new Hashtable();
                                                    int hInsdx = 3;

                                                    //Table2 Header


                                                    if (semOrYear != 0)
                                                    {

                                                        htIndex.Add("semOrYear", hInsdx);
                                                        hInsdx++;
                                                    }


                                                    if (allotedAmt != 0)
                                                    {

                                                        htIndex.Add("allotedAmt", hInsdx);
                                                        hInsdx++;
                                                    }

                                                    if (balAmt != 0)
                                                    {

                                                        htIndex.Add("balAmt", hInsdx);
                                                        hInsdx++;
                                                    }
                                                    if (prevPaidAmt != 0)
                                                    {

                                                        htIndex.Add("prevPaidAmt", hInsdx);
                                                        hInsdx++;

                                                    }

                                                    if (concession != 0)
                                                    {

                                                        htIndex.Add("concession", hInsdx);
                                                        hInsdx++;

                                                    }



                                                    //Table2 Data

                                                    int sno = 0;
                                                    int indx = 0;
                                                    double totalamt = 0;
                                                    double balanamt = 0;
                                                    double curpaid = 0;
                                                    // double paidamount = 0;


                                                    string selHeadersQ = string.Empty;
                                                    DataSet dsHeaders = new DataSet();

                                                    if (rcptType == "1" || rcptType == "2")
                                                    {
                                                        string StudStream = string.Empty;

                                                        DataSet dsStr = new DataSet();
                                                        dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                                        if (dsStr.Tables.Count > 0)
                                                        {
                                                            if (dsStr.Tables[0].Rows.Count > 0)
                                                            {
                                                                StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                                            }
                                                        }

                                                        selHeadersQ = " select sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d ,FS_ChlGroupHeaderSettings f,FT_FeeAllot A,FM_LedgerMaster l where d.HeaderFK =f.HeaderFK and D.LedgerFK=l.LedgerPK  and  d.LedgerFK=a.LedgerFK and d.App_No=a.App_No and A.FeeCategory =D.FeeCategory  and    transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'  ";
                                                        if (StudStream != "")
                                                        {
                                                            selHeadersQ += " and f.stream='" + StudStream + "' ";
                                                        }
                                                        selHeadersQ += "   group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                                    }
                                                    else if (rcptType == "3")
                                                    {
                                                        selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                                    }
                                                    else if (rcptType == "4")
                                                    {
                                                        selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.LedgerFK=a.LedgerFK  and d.FeeCategory =A.FeeCategory and  d.App_No=a.App_No and transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";
                                                    }

                                                    if (selHeadersQ != string.Empty)
                                                    {


                                                        string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                                        dsHeaders.Clear();
                                                        dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                                        if (dsHeaders.Tables.Count > 0)
                                                        {
                                                            if (dsHeaders.Tables[0].Rows.Count > 0)
                                                            {
                                                                rows += dsHeaders.Tables[0].Rows.Count;
                                                                PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, htIndex.Count + 3, 5);
                                                                tableparts1.VisibleHeaders = false;
                                                                tableparts1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                                                tableparts1.Cell(0, 0).SetContent("S.No");
                                                                tableparts1.Cell(0, 0).SetFont(FontTableHead);
                                                                tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                                                                tableparts1.Cell(0, 1).SetContent("Description");
                                                                tableparts1.Cell(0, 1).SetFont(FontTableHead);
                                                                tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                                tableparts1.Cell(0, 2).SetContent("Paid Rs");
                                                                tableparts1.Cell(0, 2).SetFont(FontTableHead);
                                                                tableparts1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                                for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                                                {
                                                                    string disphdr = string.Empty;
                                                                    double allotamt0 = 0;
                                                                    double deductAmt0 = 0;
                                                                    double totalAmt0 = 0;
                                                                    double paidAmt0 = 0;
                                                                    double balAmt0 = 0;
                                                                    double creditAmt0 = 0;

                                                                    creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);

                                                                    totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                                    //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);


                                                                    //paidAmt0 = totalAmt0 - balAmt0;
                                                                    deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                                    disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                                    string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                                    string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                                    string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);
                                                                    string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                                    paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                                    #region Monthwise
                                                                    string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                                    string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                                                    int monWisemon = 0;
                                                                    int monWiseYea = 0;
                                                                    string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                    string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                    int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                                                    int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                                                    if (monWisemon > 0 && monWiseYea > 0)
                                                                    {
                                                                        string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                                        DataSet dsMonwise = new DataSet();
                                                                        dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                                        if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                                            paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                                            disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                                            balAmt0 = totalAmt0 - paidAmt0;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                    }
                                                                    #endregion

                                                                    //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                    feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                                                    sno++;
                                                                    indx++;
                                                                    totalamt += Convert.ToDouble(totalAmt0);
                                                                    balanamt += Convert.ToDouble(balAmt0);
                                                                    curpaid += Convert.ToDouble(creditAmt0);

                                                                    deductionamt += Convert.ToDouble(deductAmt0);

                                                                    tableparts1.Cell(indx, 0).SetContent(sno);
                                                                    tableparts1.Cell(indx, 0).SetFont(FontTable);
                                                                    tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                                                                    tableparts1.Cell(indx, 1).SetContent(disphdr);
                                                                    tableparts1.Cell(indx, 1).SetFont(FontTable);
                                                                    tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                                    tableparts1.Cell(indx, 2).SetContent(creditAmt0);
                                                                    tableparts1.Cell(indx, 2).SetFont(FontTable);
                                                                    tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);

                                                                    if (semOrYear != 0)
                                                                    {
                                                                        if (htIndex.Contains("semOrYear"))
                                                                        {
                                                                            int ind = Convert.ToInt32(htIndex["semOrYear"]);
                                                                            tableparts1.Cell(indx, ind).SetContent(Convert.ToString(feecatcode));
                                                                            tableparts1.Cell(indx, ind).SetFont(FontTable);
                                                                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                            if (indx == 1)
                                                                            {
                                                                                tableparts1.Cell(0, ind).SetContent("Category");
                                                                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                            }
                                                                        }

                                                                        // htIndex.Add("semOrYear", hInsdx);
                                                                    }


                                                                    if (allotedAmt != 0)
                                                                    {
                                                                        if (htIndex.Contains("allotedAmt"))
                                                                        {
                                                                            int ind = Convert.ToInt32(htIndex["allotedAmt"]);
                                                                            tableparts1.Cell(indx, ind).SetContent(totalAmt0);
                                                                            tableparts1.Cell(indx, ind).SetFont(FontTable);
                                                                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                            if (indx == 1)
                                                                            {
                                                                                tableparts1.Cell(0, ind).SetContent("Fixed Fee Rs");
                                                                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                            }
                                                                        }
                                                                    }

                                                                    if (balAmt != 0)
                                                                    {
                                                                        if (htIndex.Contains("balAmt"))
                                                                        {
                                                                            int ind = Convert.ToInt32(htIndex["balAmt"]);
                                                                            tableparts1.Cell(indx, ind).SetContent(balAmt0);
                                                                            tableparts1.Cell(indx, ind).SetFont(FontTable);
                                                                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                            if (indx == 1)
                                                                            {
                                                                                tableparts1.Cell(0, ind).SetContent("Balance Rs");
                                                                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                            }
                                                                        }
                                                                    }
                                                                    if (prevPaidAmt != 0)
                                                                    {
                                                                        if (htIndex.Contains("prevPaidAmt"))
                                                                        {
                                                                            int ind = Convert.ToInt32(htIndex["prevPaidAmt"]);
                                                                            tableparts1.Cell(indx, ind).SetContent(paidAmt0);
                                                                            tableparts1.Cell(indx, ind).SetFont(FontTable);
                                                                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                            if (indx == 1)
                                                                            {
                                                                                tableparts1.Cell(0, ind).SetContent("Already Paid Rs");
                                                                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                            }
                                                                        }

                                                                    }

                                                                    if (concession != 0)
                                                                    {
                                                                        if (htIndex.Contains("concession"))
                                                                        {
                                                                            int ind = Convert.ToInt32(htIndex["concession"]);
                                                                            tableparts1.Cell(indx, ind).SetContent(deductAmt0);
                                                                            tableparts1.Cell(indx, ind).SetFont(FontTable);
                                                                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                                                            if (indx == 1)
                                                                            {
                                                                                tableparts1.Cell(0, ind).SetContent("Deduction Rs");
                                                                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                                                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                            }
                                                                        }

                                                                    }
                                                                }
                                                                createPDFOK = true;

                                                                curY += 5 + (int)addtabletopage1.Area.Height;

                                                                PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 40, curY, 500, 600));
                                                                rcptpage.Add(addtabletopage2);

                                                                #region Table 3
                                                                //Table3 Format
                                                                PdfTable tableparts2 = recptDoc.NewTable(FontTableHead, 1, 8, 5);
                                                                tableparts2.VisibleHeaders = false;
                                                                tableparts2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                                                //Table3 Header              
                                                                decimal totalamount = (decimal)curpaid;

                                                                tableparts2.Cell(0, 0).SetContent("Received " + DecimalToWords(totalamount).ToString() + " Rupees Only");
                                                                tableparts2.Cell(0, 0).SetFont(FontTableHead);
                                                                tableparts2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                tableparts2.Cell(0, 0).ColSpan = 4;

                                                                tableparts2.Cell(0, 4).SetContent("Total");
                                                                tableparts2.Cell(0, 4).SetFont(FontTableHead);
                                                                tableparts2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                                tableparts2.Cell(0, 5).SetContent("Rs. " + curpaid + "/-");
                                                                tableparts2.Cell(0, 5).SetFont(FontTableHead);
                                                                tableparts2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                                tableparts2.Cell(0, 6).SetContent("Balance");
                                                                tableparts2.Cell(0, 6).SetFont(FontTableHead);
                                                                tableparts2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                if (BalanceType == 1)
                                                                {
                                                                    balanamt = retBalance(appnoNew);
                                                                }
                                                                tableparts2.Cell(0, 7).SetContent("Rs. " + balanamt + "/-");
                                                                tableparts2.Cell(0, 7).SetFont(FontTableHead);
                                                                tableparts2.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);


                                                                curY += 5 + (int)addtabletopage2.Area.Height;
                                                                PdfTablePage addtabletopage3 = tableparts2.CreateTablePage(new PdfArea(recptDoc, 40, curY, 500, 50));
                                                                rcptpage.Add(addtabletopage3);
                                                                #endregion

                                                                #region Receipt Footer
                                                                string modeMulti = string.Empty;
                                                                bool multiCash = false;
                                                                bool multiChk = false;
                                                                bool multiDD = false;

                                                                DataSet dtMulBnkDetails = new DataSet();
                                                                dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  else 'DD' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

                                                                string ddnar = string.Empty;
                                                                double modeht = 40;
                                                                if (narration != 0)
                                                                {
                                                                    if (dtMulBnkDetails.Tables.Count > 0)
                                                                    {
                                                                        int sn = 1;
                                                                        for (int z = 0; z < dtMulBnkDetails.Tables[0].Rows.Count; z++)
                                                                        {
                                                                            if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CASH")
                                                                            {
                                                                                multiCash = true;
                                                                                continue;
                                                                            }
                                                                            else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CHEQUE")
                                                                            {
                                                                                multiChk = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                multiDD = true;
                                                                            }

                                                                            ddnar += "\n" + sn.ToString() + ")No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                                            sn++;
                                                                        }
                                                                        modeht = dtMulBnkDetails.Tables[0].Rows.Count * 15;
                                                                        modeht += 20;
                                                                    }
                                                                }

                                                                if (multiCash)
                                                                {
                                                                    modeMulti += "Cash,";
                                                                }
                                                                if (multiChk)
                                                                {
                                                                    modeMulti += "Cheque,";
                                                                }
                                                                if (multiDD)
                                                                {
                                                                    modeMulti += "DD";
                                                                }
                                                                modeMulti = modeMulti.TrimEnd(',');

                                                                //Mode of Pay

                                                                curY += 5 + (int)addtabletopage3.Area.Height;
                                                                PdfTextArea modeofpayText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 30, curY, 250, modeht), ContentAlignment.MiddleLeft, "Mode of Pay : " + modeMulti + ddnar);
                                                                rcptpage.Add(modeofpayText);

                                                                if (deduction != 0)
                                                                {
                                                                    PdfTextArea deducText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 130, curY, 200, 20), ContentAlignment.MiddleCenter, "Deduction Amount Rs. : " + deductionamt);
                                                                    rcptpage.Add(deducText);
                                                                }
                                                                if (excessAmt != 0)
                                                                {
                                                                    PdfTextArea exText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 240, curY, 200, 20), ContentAlignment.MiddleCenter, "Excess Amount Rs. : " + excessRemaining(appnoNew));
                                                                    rcptpage.Add(exText);
                                                                }
                                                                if (validDate != 0)
                                                                {
                                                                    PdfTextArea valdtText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 370, curY, 200, 20), ContentAlignment.MiddleCenter, "Valid upto : " + "(" + recptValid + ")");
                                                                    rcptpage.Add(valdtText);
                                                                }



                                                                //Authorizer
                                                                if (forclgName != 0)
                                                                {
                                                                    curY += 20;
                                                                    PdfTextArea authorizeText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 320, curY, 250, 20), ContentAlignment.MiddleCenter, "For " + collegename);
                                                                    rcptpage.Add(authorizeText);
                                                                }

                                                                if (authSignValue.Trim() != "")
                                                                {
                                                                    curY += 20;
                                                                    PdfTextArea authorizeSignText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 350, curY, 150, 20), ContentAlignment.MiddleCenter, authSignValue);
                                                                    rcptpage.Add(authorizeSignText);
                                                                }
                                                                else
                                                                {
                                                                    curY += 20;
                                                                    PdfTextArea authorizeSignText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 350, curY, 150, 20), ContentAlignment.MiddleCenter, "Authorised Signature");
                                                                    rcptpage.Add(authorizeSignText);
                                                                }

                                                                PdfPage rcptpageOf = rcptpage.CreateCopy();


                                                                if (transCopy != 0 && pl == 1)
                                                                {
                                                                    int cuyy = curY;
                                                                    //if (authSign == 0)
                                                                    //{
                                                                    cuyy += 10;
                                                                    //}
                                                                    rcptpageTran = rcptpage.CreateCopy();
                                                                    PdfTextArea transCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 50, cuyy + modeht, 150, 20), ContentAlignment.MiddleCenter, "Transport Copy ");
                                                                    rcptpageTran.Add(transCopyText);


                                                                }


                                                                if (studCopy != 0 || studOffiCopy == 1)
                                                                {
                                                                    //if (authSign == 0)
                                                                    //{
                                                                    curY += 10;
                                                                    //}
                                                                    string copy = "Student Copy ";
                                                                    if (pl == 2)
                                                                        copy = "Office Copy ";
                                                                    PdfTextArea studCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 50, curY + modeht, 150, 20), ContentAlignment.MiddleCenter, copy);
                                                                    rcptpage.Add(studCopyText);
                                                                    if (pl == pagelength)
                                                                    {
                                                                        rcptpage.SaveToDocument();
                                                                    }
                                                                }

                                                                //save changes

                                                                if (pl == pagelength)
                                                                {
                                                                    if (officopy != 0 && studOffiCopy != 1)
                                                                    {
                                                                        PdfTextArea offCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 50, curY + modeht, 150, 20), ContentAlignment.MiddleCenter, "Office Copy ");
                                                                        rcptpageOf.Add(offCopyText);
                                                                        rcptpageOf.SaveToDocument();

                                                                    }

                                                                }

                                                                if (transCopy != 0 && pl == pagelength)
                                                                {
                                                                    rcptpageTran.SaveToDocument();
                                                                }

                                                                curY += 10;

                                                                #endregion
                                                            }
                                                        }
                                                    }

                                                    #endregion


                                                }

                                                #endregion

                                            }
                                            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
                                            finally
                                            {

                                            }

                                            createPDFOK = true;
                                        }
                                        else
                                        {
                                            imgAlert.Visible = true;
                                            lbl_alert.Text = "No Records Found";
                                        }
                                    }
                                    else
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "No Records Found";
                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "Receipt Cancelled";
                                }

                            }
                        }
                        #endregion

                        #region To print the Receipt
                        if (createPDFOK)
                        {
                            //Response Write
                            string appPath = HttpContext.Current.Server.MapPath("~");
                            if (appPath != "")
                            {
                                string szPath = appPath + "/Report/";
                                string szFile = "Receipt" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                Response.Buffer = true;
                                Response.Clear();
                                recptDoc.SaveToFile(szPath + szFile);
                                //Response.ClearHeaders();
                                //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                //Response.ContentType = "application/pdf";
                                //Response.WriteFile(szPath + szFile);

                                Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Receipt Generated";
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Server Path Not Found";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Receipt Cannot Be Generated";
                        }
                        #endregion
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Add Print Settings";
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Add Print Settings";
                }
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Receipt";
        }
    }
    public void btnChlnChristopher_Click(string dupReceipt)
    {
        if (checkedOK())
        {
            //Christopher
            FpSpread1.SaveChanges();
            try
            {
                string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
                DataSet dsPri = new DataSet();
                dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
                if (dsPri.Tables.Count > 0)
                {
                    if (dsPri.Tables[0].Rows.Count > 0)
                    {
                        string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

                        //Document Settings
                        PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.A4);

                        Font Fontboldhead = new Font("Arial", 10, FontStyle.Bold);
                        Font FontTableHead = new Font("Arial", 9, FontStyle.Bold);
                        Font FontTable = new Font("Arial", 9, FontStyle.Regular);
                        Font tamilFont = new Font("AMUDHAM.TTF", 10, FontStyle.Regular);

                        bool createPDFOK = false;

                        #region For Every selected Receipt
                        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                            if (check == 1)
                            {
                                string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                                string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

                                string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                                if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                                {
                                    string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                                    DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                                    if (dsDet.Tables.Count > 0)
                                    {
                                        if (dsDet.Tables[0].Rows.Count > 0)
                                        {
                                            string rollno = string.Empty;
                                            string studname = string.Empty;
                                            string receiptno = string.Empty;
                                            string name = string.Empty;

                                            string app_formno = string.Empty;
                                            string appnoNew = string.Empty;
                                            string Regno = string.Empty;

                                            string batchYrSem = string.Empty;

                                            string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                                            string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                                            string mode = string.Empty;
                                            string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                                            string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                                            switch (paymode)
                                            {
                                                case "1":
                                                    mode = "Cash";
                                                    break;
                                                case "2":
                                                    mode = "Cheque";
                                                    break;
                                                case "3":
                                                    mode = "DD";
                                                    break;
                                                case "4":
                                                    mode = "Challan";
                                                    break;
                                                case "5":
                                                    mode = "Online";
                                                    break;
                                                default:
                                                    mode = "Others";
                                                    break;
                                            }


                                            string queryRollApp;

                                            if (ddlBefAfteAdm.SelectedIndex == 0)
                                            {
                                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            }
                                            else
                                            {
                                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name  from applyn where app_no='" + AppNo + "'";
                                            }
                                            DataSet dsRollApp = new DataSet();
                                            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                                            if (dsRollApp.Tables.Count > 0)
                                            {
                                                if (dsRollApp.Tables[0].Rows.Count > 0)
                                                {
                                                    rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                                    app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                                    Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                                    studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                                }
                                            }
                                            name = rollno + "-" + studname;

                                            //Print Region

                                            try
                                            {
                                                #region Print Option For Receipt

                                                PdfPage rcptpage = recptDoc.NewPage();
                                                //Fields to print

                                                #region Settings Input
                                                //Header Div Values
                                                byte collegeid = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                                                byte address1 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd1"]);
                                                byte address2 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd2"]);
                                                byte address3 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd3"]);
                                                byte city = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeDist"]);
                                                byte state = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeState"]);

                                                byte university = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeUniversity"]);
                                                byte rightLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRightLogo"]);
                                                byte leftLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsLeftLogo"]);
                                                byte time;
                                                if (Convert.ToBoolean(Convert.ToString(dsPri.Tables[0].Rows[0]["IsTime"])))
                                                {
                                                    time = 1;
                                                }
                                                else
                                                {
                                                    time = 0;
                                                }
                                                byte degACR = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsDegreeAcr"]);
                                                byte degNam = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsDegreeName"]);
                                                byte studnam = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudName"]);
                                                byte year = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsYear"]);
                                                byte semester = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSemester"]);
                                                byte regno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRegNo"]);
                                                byte rolno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRollNo"]);
                                                byte admno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAdminNo"]);

                                                byte fathername = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFatherName"]);
                                                byte seattype = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSeatType"]);
                                                //byte setRollAsAdmin = Convert.ToByte(dsPri.Tables[0].Rows[0]["rollas_adm"]);
                                                byte boarding = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsBoarding"]);
                                                byte mothername = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsMontherName"]);
                                                string recptValid = Convert.ToString(dsPri.Tables[0].Rows[0]["ValidDate"]);


                                                //Body Div Values
                                                //byte showAllFees = Convert.ToByte(dsPri.Tables[0].Rows[0]["showallfee"]);
                                                byte allotedAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAllotedAmt"]);
                                                byte fineAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFineAmt"]);
                                                byte balAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsBalanceAmt"]);
                                                byte semOrYear = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSemYear"]);
                                                byte prevPaidAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsPrevPaid"]);
                                                byte excessAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsExcessAmt"]);
                                                // byte totDetails = Convert.ToByte(dsPri.Tables[0].Rows[0]["Total_Details"]);
                                                byte fineInRow = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFineinRow"]);
                                                //byte totWTselectCol = Convert.ToByte(dsPri.Tables[0].Rows[0]["TotalSelCol"]);
                                                byte concession = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsConcession"]);
                                                string concessionValue = string.Empty;
                                                if (concession != 0)
                                                {
                                                    concessionValue = Convert.ToString(dsPri.Tables[0].Rows[0]["ConcessionName"]);
                                                }


                                                //Footer Div Values

                                                byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                                                byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                                                byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);
                                                byte narration = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsNarration"]);
                                                byte deduction = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTotConcession"]);
                                                byte forclgName = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsForCollegeName"]);
                                                byte authSign = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAuthSign"]);
                                                byte validDate = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsValidUpto"]);
                                                string authSignValue = string.Empty;
                                                if (authSign != 0)
                                                {
                                                    authSignValue = Convert.ToString(dsPri.Tables[0].Rows[0]["AuthName"]);

                                                }

                                                byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                                                // byte dispModeWTcash = Convert.ToByte(dsPri.Tables[0].Rows[0]["DisModeWithCash"]);
                                                byte signFile = Convert.ToByte(dsPri.Tables[0].Rows[0]["cashier_sign"]);

                                                //if (signFile != 0)
                                                //{
                                                //if (FileUpload1.HasFile)
                                                //{

                                                //}                                                    
                                                //}


                                                #endregion

                                                #region Students Input
                                                //string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL(type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.app_no='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
                                                string colquery = "";
                                                if (ddlBefAfteAdm.SelectedIndex == 0)
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                else
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                string collegename = "";
                                                string add1 = "";
                                                string add2 = "";
                                                string add3 = "";
                                                string univ = "";
                                                string deg = "";
                                                string cursem = "";
                                                string batyr = "";
                                                string seatty = "";
                                                string board = "";
                                                string mothe = "";
                                                string fathe = "";
                                                string stream = "";
                                                double deductionamt = 0;
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(colquery, "Text");
                                                if (ds.Tables.Count > 0)
                                                {
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                                                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                                    }
                                                    if (ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        if (degACR == 0)
                                                        {
                                                            deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                                        }
                                                        else
                                                        {
                                                            deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                        }
                                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                                        board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                        mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                                        stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                                    }
                                                }
                                                #endregion


                                                int rectHeight = 380;

                                                int curY = 10;
                                                int curX = 30;


                                                #region Receipt Header

                                                //Rectangle Border
                                                PdfArea rectArea = new PdfArea(recptDoc, 10, curY, 570, rectHeight);
                                                PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
                                                rcptpage.Add(rectSpace);

                                                PdfArea rectAreaOff = new PdfArea(recptDoc, 10, curY + 430, 570, rectHeight);
                                                PdfRectangle rectSpaceOff = new PdfRectangle(recptDoc, rectAreaOff, Color.Black);
                                                //Header Images
                                                //Line1
                                                PdfImage LogoImageOff;
                                                int lefty = 0;
                                                if (leftLogo != 0)
                                                {
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                                                    {
                                                        PdfImage LogoImage = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg"));
                                                        rcptpage.Add(LogoImage, curX, curY + 5, 600);
                                                        lefty = curY + 5 + 430;

                                                    }
                                                }

                                                if (collegeid != 0)
                                                {
                                                    curX = 120;
                                                    PdfTextArea clgText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, collegename);
                                                    rcptpage.Add(clgText);

                                                }
                                                PdfTextArea clgOffText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX, curY + 430, 350, 20), ContentAlignment.MiddleCenter, collegename);

                                                PdfImage LogoImageOff1;
                                                int righty = 0;
                                                if (rightLogo != 0)
                                                {
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                                                    {
                                                        curX = 500;
                                                        PdfImage LogoImage1 = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg"));
                                                        rcptpage.Add(LogoImage1, curX, curY + 5, 600);
                                                        righty = curY + 5 + 430;
                                                    }
                                                }

                                                //Line2
                                                curY += 15;
                                                if (address1 != 0 || address2 != 0)
                                                {
                                                    curX = 120;

                                                    if (address2 != 0)
                                                    {
                                                        add1 += " " + add2;
                                                    }
                                                    PdfTextArea addText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, add1);
                                                    rcptpage.Add(addText);

                                                }
                                                PdfTextArea addOffText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY + 430, 350, 20), ContentAlignment.MiddleCenter, add1);
                                                // curY += 20;
                                                //if (university != 0)
                                                //{

                                                //    curX = 120;
                                                //    PdfTextArea uniText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, univ);
                                                //    rcptpage.Add(uniText);

                                                //}
                                                //PdfTextArea uniOffText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY + 430, 350, 20), ContentAlignment.MiddleCenter, univ);

                                                //Line3




                                                //Line4
                                                curY += 8;
                                                //if (address3 != 0)
                                                //{

                                                //    curX = 120;

                                                //    PdfTextArea cityText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, add3);
                                                //    rcptpage.Add(cityText);

                                                //}
                                                //PdfTextArea cityOffText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY + 430, 350, 20), ContentAlignment.MiddleCenter, add3);
                                                PdfTextArea headingText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX - 50, curY, 450, 30), ContentAlignment.MiddleCenter, "FEE RECEIPT " + dupReceipt);
                                                rcptpage.Add(headingText);

                                                //curX = 280;
                                                curY += 12;
                                                //Text Area For Receipt
                                                PdfTextArea headingText1 = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX - 50, curY, 450, 30), ContentAlignment.MiddleCenter, recptHeader(recptNo));
                                                rcptpage.Add(headingText1);

                                                PdfTextArea headingOffText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX - 50, curY + 418, 450, 30), ContentAlignment.MiddleCenter, "FEE RECEIPT " + dupReceipt);

                                                PdfTextArea headingOffText1 = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX - 50, curY + 430, 450, 30), ContentAlignment.MiddleCenter, recptHeader(recptNo));


                                                curY += 15;

                                                #endregion

                                                #region Table 1
                                                //Table1 Format 
                                                PdfTable tableparts = recptDoc.NewTable(FontTableHead, 2, 3, 5);
                                                tableparts.VisibleHeaders = false;
                                                tableparts.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                                if (regno != 0)
                                                {
                                                    tableparts.Cell(0, 0).SetContent("RegNo : " + Regno);
                                                    tableparts.Cell(0, 0).SetFont(FontTableHead);
                                                    tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                }

                                                if (rolno != 0)
                                                {
                                                    tableparts.Cell(0, 0).SetContent("RollNo : " + rollno);
                                                    tableparts.Cell(0, 0).SetFont(FontTableHead);
                                                    tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                }

                                                if (admno != 0)
                                                {
                                                    tableparts.Cell(0, 0).SetContent("AdmissionNo : " + app_formno);
                                                    tableparts.Cell(0, 0).SetFont(FontTableHead);
                                                    tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                }

                                                tableparts.Cell(0, 1).SetContent("Programme : " + deg);
                                                tableparts.Cell(0, 1).SetFont(FontTableHead);
                                                tableparts.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(0, 2).SetContent(termDisplay(cursem));
                                                tableparts.Cell(0, 2).SetFont(FontTableHead);
                                                tableparts.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                tableparts.Cell(1, 0).SetContent("Name : " + studname.ToUpper());
                                                tableparts.Cell(1, 0).SetFont(FontTableHead);
                                                tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                tableparts.Cell(1, 1).SetContent("Receipt No : " + recptNo);
                                                tableparts.Cell(1, 1).SetFont(FontTableHead);
                                                tableparts.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(1, 2).SetContent("Date : " + recptDt);
                                                tableparts.Cell(1, 2).SetFont(FontTableHead);
                                                tableparts.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                //Off
                                                PdfTable tablepartsOff = recptDoc.NewTable(FontTableHead, 2, 3, 5);
                                                tablepartsOff.VisibleHeaders = false;
                                                tablepartsOff.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                                if (regno != 0)
                                                {
                                                    tablepartsOff.Cell(0, 0).SetContent("RegNo : " + Regno);
                                                    tablepartsOff.Cell(0, 0).SetFont(FontTableHead);
                                                    tablepartsOff.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                }

                                                if (rolno != 0)
                                                {
                                                    tablepartsOff.Cell(0, 0).SetContent("RollNo : " + rollno);
                                                    tablepartsOff.Cell(0, 0).SetFont(FontTableHead);
                                                    tablepartsOff.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                }

                                                if (admno != 0)
                                                {
                                                    tablepartsOff.Cell(0, 0).SetContent("AdmissionNo : " + app_formno);
                                                    tablepartsOff.Cell(0, 0).SetFont(FontTableHead);
                                                    tablepartsOff.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                }

                                                tablepartsOff.Cell(0, 1).SetContent("Programme : " + deg);
                                                tablepartsOff.Cell(0, 1).SetFont(FontTableHead);
                                                tablepartsOff.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tablepartsOff.Cell(0, 2).SetContent(termDisplay(cursem));
                                                tablepartsOff.Cell(0, 2).SetFont(FontTableHead);
                                                tablepartsOff.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                tablepartsOff.Cell(1, 0).SetContent("Name : " + studname.ToUpper());
                                                tablepartsOff.Cell(1, 0).SetFont(FontTableHead);
                                                tablepartsOff.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                tablepartsOff.Cell(1, 1).SetContent("Receipt No : " + recptNo);
                                                tablepartsOff.Cell(1, 1).SetFont(FontTableHead);
                                                tablepartsOff.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tablepartsOff.Cell(1, 2).SetContent("Date : " + recptDt);
                                                tablepartsOff.Cell(1, 2).SetFont(FontTableHead);
                                                tablepartsOff.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                curX = 10;
                                                curY += 10;
                                                PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, curX, curY, 570, 200));
                                                rcptpage.Add(addtabletopage1);

                                                PdfTablePage addtabletopageOff1 = tablepartsOff.CreateTablePage(new PdfArea(recptDoc, curX, curY + 430, 570, 200));

                                                #endregion

                                                #region Table 2
                                                //Table2 Format

                                                int rows = 1;

                                                selectQuery = "";


                                                int sno = 0;
                                                int indx = 0;
                                                double totalamt = 0;
                                                double balanamt = 0;
                                                double curpaid = 0;
                                                // double paidamount = 0;


                                                string selHeadersQ = string.Empty;
                                                DataSet dsHeaders = new DataSet();

                                                if (rcptType == "1" || rcptType == "2")
                                                {
                                                    string StudStream = string.Empty;

                                                    DataSet dsStr = new DataSet();
                                                    dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                                    if (dsStr.Tables.Count > 0)
                                                    {
                                                        if (dsStr.Tables[0].Rows.Count > 0)
                                                        {
                                                            StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                                        }
                                                    }

                                                    selHeadersQ = " select sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d ,FS_ChlGroupHeaderSettings f,FT_FeeAllot A,FM_LedgerMaster l where d.HeaderFK =f.HeaderFK and D.LedgerFK=l.LedgerPK  and  d.LedgerFK=a.LedgerFK and d.App_No=a.App_No and A.FeeCategory =D.FeeCategory  and    transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'  ";
                                                    if (StudStream != "")
                                                    {
                                                        selHeadersQ += " and f.stream='" + StudStream + "' ";
                                                    }
                                                    selHeadersQ += "   group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                                }
                                                else if (rcptType == "3")
                                                {
                                                    selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                                }
                                                else if (rcptType == "4")
                                                {
                                                    selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.LedgerFK=a.LedgerFK  and d.FeeCategory =A.FeeCategory and  d.App_No=a.App_No and transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";
                                                }

                                                if (selHeadersQ != string.Empty)
                                                {
                                                    string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                                    dsHeaders.Clear();
                                                    dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                                    if (dsHeaders.Tables.Count > 0)
                                                    {
                                                        if (dsHeaders.Tables[0].Rows.Count > 0)
                                                        {
                                                            rows += dsHeaders.Tables[0].Rows.Count;
                                                            int ledgeCol = 2;
                                                            int ledgeWidt = 570;

                                                            if (rows > 0)
                                                            {
                                                                ledgeCol = 2;
                                                                ledgeWidt = 570;
                                                                int wdth = ledgeWidt - 50;
                                                                ledgeWidt = wdth;
                                                            }
                                                            if (rows > 10)
                                                            {
                                                                ledgeCol += 2;
                                                                ledgeWidt = 570;
                                                                int wdth = ledgeWidt - 100;
                                                                ledgeWidt = (int)(wdth / 2);
                                                            }
                                                            if (rows > 20)
                                                            {
                                                                ledgeCol += 2;
                                                                ledgeWidt = 570;
                                                                int wdth = ledgeWidt - 150;
                                                                ledgeWidt = (int)(wdth / 3);
                                                            }
                                                            if (rows > 30)
                                                            {
                                                                ledgeCol += 2;
                                                                ledgeWidt = 570;
                                                                int wdth = ledgeWidt - 200;
                                                                ledgeWidt = (int)(wdth / 4);
                                                            }
                                                            PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, ledgeCol, 5);
                                                            tableparts1.VisibleHeaders = false;
                                                            tableparts1.SetBorders(Color.Black, 1, BorderType.Columns);
                                                            for (int colval = 0; colval < tableparts1.Columns.Length; colval++)
                                                            {
                                                                if (colval % 2 == 1)
                                                                {
                                                                    tableparts1.Columns[colval].SetWidth(50);
                                                                }
                                                                else
                                                                {
                                                                    tableparts1.Columns[colval].SetWidth(ledgeWidt);
                                                                }
                                                            }


                                                            PdfTable tablepartsOff1 = recptDoc.NewTable(FontTable, rows, ledgeCol, 5);
                                                            tablepartsOff1.VisibleHeaders = false;
                                                            tablepartsOff1.SetBorders(Color.Black, 1, BorderType.Columns);
                                                            for (int colval = 0; colval < tablepartsOff1.Columns.Length; colval++)
                                                            {
                                                                if (colval % 2 == 1)
                                                                {
                                                                    tablepartsOff1.Columns[colval].SetWidth(50);
                                                                }
                                                                else
                                                                {
                                                                    tablepartsOff1.Columns[colval].SetWidth(ledgeWidt);
                                                                }
                                                            }

                                                            int initLegCol = 0;
                                                            int initValCol = 1;
                                                            int initRow = 0;

                                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                                            {
                                                                string disphdr = string.Empty;
                                                                double allotamt0 = 0;
                                                                double deductAmt0 = 0;
                                                                double totalAmt0 = 0;
                                                                double paidAmt0 = 0;
                                                                double balAmt0 = 0;
                                                                double creditAmt0 = 0;

                                                                creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);

                                                                totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                                //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);

                                                                //paidAmt0 = totalAmt0 - balAmt0;
                                                                deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                                disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                                string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                                string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                                string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);
                                                                string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                                paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                                #region Monthwise
                                                                string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                                string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                                                int monWisemon = 0;
                                                                int monWiseYea = 0;
                                                                string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                                int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                                                int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                                                if (monWisemon > 0 && monWiseYea > 0)
                                                                {
                                                                    string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                                    DataSet dsMonwise = new DataSet();
                                                                    dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                                    if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                                        paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                                        disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                                        balAmt0 = totalAmt0 - paidAmt0;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                                }
                                                                #endregion


                                                                feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                                                sno++;
                                                                indx++;
                                                                totalamt += Convert.ToDouble(totalAmt0);
                                                                balanamt += Convert.ToDouble(balAmt0);
                                                                curpaid += Convert.ToDouble(creditAmt0);

                                                                deductionamt += Convert.ToDouble(deductAmt0);

                                                                if (initRow == 10)
                                                                {
                                                                    initRow = 0;
                                                                    initLegCol += 2;
                                                                    initValCol += 2;
                                                                }

                                                                tableparts1.Cell(initRow, initLegCol).SetContent(sno + ") " + disphdr);
                                                                tableparts1.Cell(initRow, initLegCol).SetFont(FontTable);
                                                                tableparts1.Cell(initRow, initLegCol).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                                tableparts1.Cell(initRow, initValCol).SetContent(returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0));
                                                                tableparts1.Cell(initRow, initValCol).SetFont(FontTable);
                                                                tableparts1.Cell(initRow, initValCol).SetContentAlignment(ContentAlignment.MiddleRight);

                                                                tablepartsOff1.Cell(initRow, initLegCol).SetContent(sno + ") " + disphdr);
                                                                tablepartsOff1.Cell(initRow, initLegCol).SetFont(FontTable);
                                                                tablepartsOff1.Cell(initRow, initLegCol).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                                tablepartsOff1.Cell(initRow, initValCol).SetContent(returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0));
                                                                tablepartsOff1.Cell(initRow, initValCol).SetFont(FontTable);
                                                                tablepartsOff1.Cell(initRow, initValCol).SetContentAlignment(ContentAlignment.MiddleRight);

                                                                initRow++;

                                                            }
                                                            createPDFOK = true;

                                                            curY += (int)addtabletopage1.Area.Height;
                                                            PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 10, curY, 570, 220));
                                                            rcptpage.Add(addtabletopage2);
                                                            PdfTablePage addtabletopageOff2 = tablepartsOff1.CreateTablePage(new PdfArea(recptDoc, 10, curY + 430, 570, 220));

                                                            #region Table 3
                                                            PdfTable tableparts2 = recptDoc.NewTable(FontTableHead, 1, 6, 5);
                                                            tableparts2.VisibleHeaders = false;
                                                            tableparts2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                                                            PdfTable tablepartsOff2 = recptDoc.NewTable(FontTableHead, 1, 6, 5);
                                                            tablepartsOff2.VisibleHeaders = false;
                                                            tablepartsOff2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);


                                                            decimal totalamount = (decimal)curpaid;

                                                            tableparts2.Cell(0, 0).SetContent("Collected By :");
                                                            tableparts2.Cell(0, 0).SetFont(FontTableHead);
                                                            tableparts2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            tableparts2.Cell(0, 0).ColSpan = 2;

                                                            tableparts2.Cell(0, 2).SetContent("(" + DecimalToWords((decimal)curpaid) + " Rupees Only)");
                                                            tableparts2.Cell(0, 2).SetFont(FontTableHead);
                                                            tableparts2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            tableparts2.Cell(0, 2).ColSpan = 2;

                                                            tableparts2.Cell(0, 4).SetContent("Total Fees");
                                                            tableparts2.Cell(0, 4).SetFont(FontTableHead);
                                                            tableparts2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleRight);

                                                            tableparts2.Cell(0, 5).SetContent(returnIntegerPart(curpaid) + "." + returnDecimalPart(curpaid));
                                                            tableparts2.Cell(0, 5).SetFont(FontTableHead);
                                                            tableparts2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                            tablepartsOff2.Cell(0, 0).SetContent("Collected By :");
                                                            tablepartsOff2.Cell(0, 0).SetFont(FontTableHead);
                                                            tablepartsOff2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            tablepartsOff2.Cell(0, 0).ColSpan = 2;

                                                            tablepartsOff2.Cell(0, 2).SetContent("(" + DecimalToWords((decimal)curpaid) + " Rupees Only)");
                                                            tablepartsOff2.Cell(0, 2).SetFont(FontTableHead);
                                                            tablepartsOff2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            tablepartsOff2.Cell(0, 2).ColSpan = 2;

                                                            tablepartsOff2.Cell(0, 4).SetContent("Total Fees");
                                                            tablepartsOff2.Cell(0, 4).SetFont(FontTableHead);
                                                            tablepartsOff2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleRight);

                                                            tablepartsOff2.Cell(0, 5).SetContent(returnIntegerPart(curpaid) + "." + returnDecimalPart(curpaid));
                                                            tablepartsOff2.Cell(0, 5).SetFont(FontTableHead);
                                                            tablepartsOff2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                            curY += (int)addtabletopage2.Area.Height;
                                                            PdfTablePage addtabletopage3 = tableparts2.CreateTablePage(new PdfArea(recptDoc, 10, curY, 570, 50));
                                                            rcptpage.Add(addtabletopage3);

                                                            PdfTablePage addtabletopageOff3 = tablepartsOff2.CreateTablePage(new PdfArea(recptDoc, 10, curY + 430, 570, 50));
                                                            #endregion

                                                            #region Receipt Footer
                                                            PdfPage rcptpageTran = rcptpage.CreateCopy();

                                                            curY += 24;

                                                            if (officopy != 0 && studOffiCopy != 0)
                                                            {
                                                                //visible Office copy
                                                                rcptpage.Add(rectSpaceOff);
                                                                if (leftLogo != 0)
                                                                {
                                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                                                                    {
                                                                        LogoImageOff = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg"));
                                                                        rcptpage.Add(LogoImageOff, 30, lefty, 600);
                                                                    }
                                                                }

                                                                if (rightLogo != 0)
                                                                {
                                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                                                                    {
                                                                        LogoImageOff1 = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg"));
                                                                        rcptpage.Add(LogoImageOff1, 500, righty, 600);
                                                                    }
                                                                }
                                                                if (collegeid != 0)
                                                                {
                                                                    rcptpage.Add(clgOffText);
                                                                }
                                                                if (university != 0)
                                                                {
                                                                    //rcptpage.Add(uniOffText);
                                                                }

                                                                if (address1 != 0 || address2 != 0)
                                                                {
                                                                    rcptpage.Add(addOffText);
                                                                }

                                                                if (address3 != 0)
                                                                {
                                                                    // rcptpage.Add(cityOffText);
                                                                }
                                                                rcptpage.Add(headingOffText);
                                                                rcptpage.Add(headingOffText1);
                                                                rcptpage.Add(addtabletopageOff1);
                                                                rcptpage.Add(addtabletopageOff2);
                                                                rcptpage.Add(addtabletopageOff3);

                                                                string copy = "Office Copy ";

                                                                PdfTextArea studCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 50, curY + 430, 150, 20), ContentAlignment.MiddleCenter, copy);
                                                                rcptpage.Add(studCopyText);

                                                            }

                                                            if (studCopy != 0 || studOffiCopy == 1)
                                                            {

                                                                string copy = "Student Copy ";

                                                                PdfTextArea studCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 50, curY, 150, 20), ContentAlignment.MiddleCenter, copy);
                                                                rcptpage.Add(studCopyText);

                                                                rcptpage.SaveToDocument();
                                                            }

                                                            if (transCopy != 0)
                                                            {
                                                                string copy = "Transport Copy ";

                                                                PdfTextArea studCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 50, curY, 150, 20), ContentAlignment.MiddleCenter, copy);
                                                                rcptpageTran.Add(studCopyText);
                                                                rcptpageTran.SaveToDocument();
                                                            }

                                                            #endregion
                                                        }
                                                    }
                                                }

                                                #endregion

                                                #endregion
                                            }
                                            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
                                            finally
                                            {

                                            }

                                            createPDFOK = true;
                                        }
                                        else
                                        {
                                            imgAlert.Visible = true;
                                            lbl_alert.Text = "No Records Found";
                                        }
                                    }
                                    else
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "No Records Found";
                                    }
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "Receipt Cancelled";
                                }

                            }
                        }
                        #endregion

                        #region To print the Receipt
                        if (createPDFOK)
                        {
                            //Response Write
                            string appPath = HttpContext.Current.Server.MapPath("~");
                            if (appPath != "")
                            {
                                string szPath = appPath + "/Report/";
                                string szFile = "Receipt" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                                Response.Buffer = true;
                                Response.Clear();
                                recptDoc.SaveToFile(szPath + szFile);
                                //Response.ClearHeaders();
                                //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                //Response.ContentType = "application/pdf";
                                //Response.WriteFile(szPath + szFile);

                                Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Receipt Generated";
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Server Path Not Found";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Receipt Cannot Be Generated";
                        }
                        #endregion
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Add Print Settings";
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Add Print Settings";
                }
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Receipt";
        }
    }
    public void btnChlnsanAcademy_Click(string dupReceipt)
    {
        //San Academy
        if (checkedOK())
        {
            FpSpread1.SaveChanges();
            try
            {
                contentDiv.InnerHtml = "";
                StringBuilder sbHtml = new StringBuilder();
                string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
                DataSet dsPri = new DataSet();
                dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
                if (dsPri.Tables.Count > 0)
                {
                    if (dsPri.Tables[0].Rows.Count > 0)
                    {
                        string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

                        //Document Settings
                        PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.InCentimeters(18, 15.2));


                        Font FontboldheadC = new Font("Old English Text MT", 15, FontStyle.Bold);
                        Font Fontboldhead = new Font("Arial", 10, FontStyle.Bold);
                        Font FontTableHead = new Font("Arial", 10, FontStyle.Bold);
                        Font FontTable = new Font("Arial", 10, FontStyle.Bold);
                        Font tamilFont = new Font("AMUDHAM.TTF", 10, FontStyle.Regular);

                        bool createPDFOK = false;


                        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                            if (check == 1)
                            {
                                sbHtml.Clear();
                                string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                                string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

                                string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                                if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                                {
                                    string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype,DDno,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,DDBankCode from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                                    DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                                    if (dsDet.Tables.Count > 0)
                                    {
                                        if (dsDet.Tables[0].Rows.Count > 0)
                                        {
                                            string rollno = string.Empty;
                                            string studname = string.Empty;
                                            string receiptno = string.Empty;
                                            string name = string.Empty;

                                            string app_formno = string.Empty;
                                            string appnoNew = string.Empty;
                                            string Regno = string.Empty;

                                            string batchYrSem = string.Empty;

                                            string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                                            string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                                            string mode = string.Empty;
                                            string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                                            string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                                            string DDDate = Convert.ToString(dsDet.Tables[0].Rows[0]["DDDate"]);
                                            string DDBranch = Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankBranch"]);
                                            string DDNo = Convert.ToString(dsDet.Tables[0].Rows[0]["DDno"]);
                                            string DDBankName = d2.GetFunction("select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode='" + Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankCode"]) + "' ");

                                            switch (paymode)
                                            {
                                                case "1":
                                                    mode = "Cash";
                                                    break;
                                                case "2":
                                                    mode = "Cheque";
                                                    break;
                                                case "3":
                                                    mode = "DD";
                                                    break;
                                                case "4":
                                                    mode = "Challan";
                                                    break;
                                                case "5":
                                                    mode = "Online";
                                                    break;
                                                default:
                                                    mode = "Others";
                                                    break;
                                            }


                                            //string queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            string queryRollApp;

                                            if (ddlBefAfteAdm.SelectedIndex == 0)
                                            {
                                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                            }
                                            else
                                            {
                                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name  from applyn where app_no='" + AppNo + "'";
                                            }
                                            DataSet dsRollApp = new DataSet();
                                            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                                            if (dsRollApp.Tables.Count > 0)
                                            {
                                                if (dsRollApp.Tables[0].Rows.Count > 0)
                                                {
                                                    rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                                    app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                                    Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                                    studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                                }
                                            }
                                            name = rollno + "-" + studname;

                                            //Print Region
                                            #region Print Option For Receipt
                                            try
                                            {
                                                PdfPage rcptpage = recptDoc.NewPage();
                                                //Fields to print

                                                #region Settings Input
                                                //Header Div Values
                                                //Header Div Values
                                                byte collegeid = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                                                byte address1 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd1"]);
                                                byte address2 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd2"]);
                                                byte address3 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd3"]);
                                                byte city = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeDist"]);
                                                byte state = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeState"]);

                                                byte university = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeUniversity"]);
                                                byte rightLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRightLogo"]);
                                                byte leftLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsLeftLogo"]);

                                                byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                                                byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                                                byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);

                                                #endregion

                                                #region Students Input
                                                //string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL(type,'') as type,r.Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.app_no='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
                                                string colquery = "";
                                                if (ddlBefAfteAdm.SelectedIndex == 0)
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3,phoneno from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,isnull(r.sections,'') as Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                else
                                                {
                                                    colquery = "select collname,university,address1,address2,address3+' - '+pincode as address3,phoneno from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,'' Sections from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                                }
                                                string collegename = "";
                                                string add1 = "";
                                                string add2 = "";
                                                string add3 = "";
                                                string univ = "";
                                                string phone = "";
                                                string deg = "";
                                                string cursem = "";
                                                string batyr = "";
                                                string seatty = "";
                                                string board = "";
                                                string mothe = "";
                                                string fathe = "";
                                                string stream = "";
                                                double deductionamt = 0;
                                                string fgraduate = d2.GetFunction("select isnull(first_graduate,0) as first_graduate  from applyn where app_no='" + appnoNew + "'");
                                                if (fgraduate == "0")
                                                {
                                                    fgraduate = string.Empty;
                                                }
                                                else
                                                {
                                                    fgraduate = " FG ";
                                                }
                                                string sec = string.Empty;
                                                ds.Clear();
                                                ds = d2.select_method_wo_parameter(colquery, "Text");
                                                if (ds.Tables.Count > 0)
                                                {
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                                                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                                        phone = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                                                        if (phone.Trim() != "")
                                                        {
                                                            phone = "Phone : " + phone;
                                                        }
                                                    }
                                                    if (ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        //if (degACR == 0)
                                                        //{
                                                        //deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                                        //}
                                                        //else
                                                        //{
                                                        deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                                        //}
                                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                                        board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                                        mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                                        stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                                        sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                                                    }
                                                }
                                                string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                                try
                                                {
                                                    acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                                }
                                                catch { }
                                                #endregion
                                                PdfArea rectArea = new PdfArea(recptDoc, 118, 35, 963, 570);
                                                PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
                                                rcptpage.Add(rectSpace);

                                                //Header Images
                                                //Line1
                                                string leftImg = "";
                                                if (leftLogo != 0)
                                                {
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                                                    {
                                                        PdfImage LogoImage = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg"));
                                                        rcptpage.Add(LogoImage, 125, 40, 450);
                                                        leftImg = "<img src='" + "college/left_Logo.jpeg?" + DateTime.Now.Ticks.ToString() + "" + "' style='height:80px; width:80px;'/>";
                                                    }
                                                }
                                                if (collegeid != 0)
                                                {
                                                    PdfTextArea clgText = new PdfTextArea(FontboldheadC, Color.Black, new PdfArea(recptDoc, 245, 45, 350, 20), ContentAlignment.MiddleCenter, collegename);
                                                    rcptpage.Add(clgText);
                                                }
                                                string rghtimg = "";
                                                if (rightLogo != 0)
                                                {
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                                                    {

                                                        PdfImage LogoImage1 = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg"));
                                                        rcptpage.Add(LogoImage1, 550, 40, 450);
                                                        rghtimg = "<img src='" + "college/right_Logo.jpeg?" + DateTime.Now.Ticks.ToString() + "" + "' style='height:80px; width:80px;'/>";
                                                    }
                                                }
                                                //Line2
                                                if (university != 0)
                                                {
                                                    PdfTextArea uniText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 245, 65, 350, 20), ContentAlignment.MiddleCenter, univ);
                                                    rcptpage.Add(uniText);
                                                }
                                                //Line3
                                                string jaiadd1 = "";
                                                if (address1 != 0 || address2 != 0)
                                                {
                                                    if (address2 != 0)
                                                    {
                                                        jaiadd1 = add1 + " " + add2;
                                                    }
                                                    PdfTextArea addText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 245, 80, 350, 20), ContentAlignment.MiddleCenter, add1);
                                                    rcptpage.Add(addText);
                                                }
                                                //Line4
                                                if (address3 != 0)
                                                {
                                                    PdfTextArea cityText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 245, 95, 350, 20), ContentAlignment.MiddleCenter, add3);
                                                    rcptpage.Add(cityText);
                                                }


                                                #region Table 1
                                                //Table1 Format 
                                                PdfTable tableparts = recptDoc.NewTable(FontTableHead, 5, 6, 2);
                                                tableparts.VisibleHeaders = false;

                                                tableparts.Rows[0].SetRowHeight(10);
                                                tableparts.Rows[1].SetRowHeight(30);
                                                tableparts.Rows[2].SetRowHeight(20);
                                                tableparts.Rows[3].SetRowHeight(20);
                                                tableparts.Rows[4].SetRowHeight(10);
                                                tableparts.Rows[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Rows[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Rows[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Rows[3].SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Rows[4].SetContentAlignment(ContentAlignment.MiddleLeft);
                                                //Table1 Data
                                                //Line 1
                                                int rowindextbl1 = 0;

                                                tableparts.Cell(rowindextbl1, 0).SetContent("");
                                                tableparts.Cell(rowindextbl1, 0).SetFont(FontTable);
                                                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tableparts.Rows[rowindextbl1].SetRowHeight(10);
                                                rowindextbl1++;

                                                tableparts.Cell(rowindextbl1, 0).SetContent("Roll No");
                                                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 1).SetContent(": " + rollno);
                                                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 1).ColSpan = 2;

                                                tableparts.Cell(rowindextbl1, 4).SetContent("Receipt No");
                                                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 5).SetContent(": " + recptNo);
                                                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                //Line2
                                                rowindextbl1++;
                                                tableparts.Cell(rowindextbl1, 0).SetContent("Name");
                                                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(rowindextbl1, 1).SetContent(": " + studname.ToUpper());
                                                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 1).ColSpan = 3;

                                                tableparts.Cell(rowindextbl1, 4).SetContent("Date");
                                                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts.Cell(rowindextbl1, 5).SetContent(": " + recptDt);
                                                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                //Line3
                                                rowindextbl1++;

                                                tableparts.Cell(rowindextbl1, 0).SetContent("Year/ Major");
                                                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);


                                                tableparts.Cell(rowindextbl1, 1).SetContent(": " + romanLetter(returnYearforSem(cursem)) + " / " + deg.Split('-')[1].ToUpper() + sec.ToUpper() + fgraduate.ToUpper());
                                                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 1).ColSpan = 3;

                                                tableparts.Cell(rowindextbl1, 4).SetContent("Term");
                                                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                tableparts.Cell(rowindextbl1, 5).SetContent(": " + acaYear);
                                                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                                                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                rowindextbl1++;
                                                tableparts.Cell(rowindextbl1, 0).SetContent("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                                                tableparts.Cell(rowindextbl1, 0).SetFont(FontTable);
                                                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                tableparts.Rows[rowindextbl1].SetRowHeight(10);


                                                PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, 10, 10 + 150, 480, 150));
                                                rcptpage.Add(addtabletopage1);

                                                #endregion


                                                if (leftImg != "" && rghtimg == "")
                                                {
                                                    rghtimg = "<div style='width:80px;height:80px;'></div>";
                                                }
                                                sbHtml.Append("<div style='height: 575px;width:963px;padding-left:114px;'>");
                                                sbHtml.Append("<table cellpadding='0' cellspacing='0' style='text-align:center; width: 745px;font-size:Arial; ' class='classBold10'><tr><td rowspan='4'>" + leftImg + "</td><td colspan='7' style='text-align:center; font-family:Old English Text MT; font-weight:bold; font-size:18px;'>" + collegename + "</td><td rowspan='4'>" + rghtimg + "</td></tr><tr><td colspan='7' style='text-align:center;font-weight:bold; font-size:12px;'>" + jaiadd1 + "</td></tr><tr><td colspan='7' style='text-align:center;font-weight:bold; font-size:12px;'>" + add3 + "</td></tr><tr><td colspan='7' style='text-align:center;font-weight:bold; font-size:12px;'>" + phone + "</td></tr></table>");
                                                sbHtml.Append("<br><table cellpadding='2' cellspacing='0' style='text-align:center;padding-top:5px; width: 745px;font-size:Arial;font-weight:bold; font-size:12px;text-align:left; border-width:1px;border-style:solid;' class='classBold10'><tr><td>Roll No</td><td colspan='2'>:&nbsp;" + rollno + "</td><td></td><td>Receipt No</td><td>:&nbsp;" + recptNo + "</td></tr><tr><td>Name</td><td colspan='3'>:&nbsp;" + studname.ToUpper() + "</td><td>Date</td><td>:&nbsp;" + recptDt + "</td><td></td></tr><tr><td>Year/ Major</td><td colspan='3'>:&nbsp;" + romanLetter(returnYearforSem(cursem)) + "</td><td>Term</td><td>:&nbsp;" + acaYear + "</td><td></td></tr></table><br>");

                                                #region Table 2
                                                //Table2 Format

                                                int rows = 0;
                                                selectQuery = "";

                                                //Table2 Data

                                                int sno = 0;
                                                int indx = 1;
                                                double totalamt = 0;
                                                double balanamt = 0;
                                                double curpaid = 0;
                                                // double paidamount = 0;

                                                string selHeadersQ = string.Empty;
                                                DataSet dsHeaders = new DataSet();

                                                if (rcptType == "1" || rcptType == "2")
                                                {
                                                    string StudStream = string.Empty;

                                                    DataSet dsStr = new DataSet();
                                                    dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                                    if (dsStr.Tables.Count > 0)
                                                    {
                                                        if (dsStr.Tables[0].Rows.Count > 0)
                                                        {
                                                            StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                                        }
                                                    }

                                                    selHeadersQ = " select sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d ,FS_ChlGroupHeaderSettings f,FT_FeeAllot A,FM_LedgerMaster l where d.HeaderFK =f.HeaderFK and D.LedgerFK=l.LedgerPK  and  d.LedgerFK=a.LedgerFK and d.App_No=a.App_No and A.FeeCategory =D.FeeCategory  and    transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'  ";
                                                    if (StudStream != "")
                                                    {
                                                        selHeadersQ += " and f.stream='" + StudStream + "' ";
                                                    }
                                                    selHeadersQ += "   group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory ,D.DailyTransPk,A.Feeallotpk ";
                                                }
                                                else if (rcptType == "3")
                                                {
                                                    selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory  and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
                                                }
                                                else if (rcptType == "4")
                                                {
                                                    selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.FeeCategory =A.FeeCategory and d.LedgerFK=a.LedgerFK and  d.App_No=a.App_No and transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";
                                                }

                                                if (selHeadersQ != string.Empty)
                                                {


                                                    string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                                    dsHeaders.Clear();
                                                    dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                                    if (dsHeaders.Tables.Count > 0 && dsHeaders.Tables[0].Rows.Count > 0)
                                                    {
                                                        sbHtml.Append("<table Rules='Rows' cellpadding='2' cellspacing='0' style='text-align:center;padding-top:5px; width: 745px;font-size:Arial;font-weight:bold; font-size:12px;text-align:left; border-width:1px;border-style:solid;' class='classBold10'><tr><td style='text-align:center;'>S.No</td><td colspan='5'  style='text-align:left;'>Particulars</td><td  style='text-align:right;'>Rs.</td><td  style='text-align:right;'>Ps.</td></tr>");


                                                        rows += dsHeaders.Tables[0].Rows.Count;
                                                        PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows + 5, 4, 1);
                                                        // tableparts1.SetBorders(Color.Black, 1, BorderType.Rows);
                                                        tableparts1.VisibleHeaders = false;
                                                        tableparts1.Columns[0].SetWidth(57);
                                                        tableparts1.Columns[1].SetWidth(340);
                                                        tableparts1.Columns[2].SetWidth(85);
                                                        tableparts1.Columns[3].SetWidth(28);

                                                        tableparts1.Cell(0, 0).SetContent("S.No");
                                                        tableparts1.Cell(0, 0).SetFont(FontTable);
                                                        tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);



                                                        tableparts1.Cell(0, 1).SetContent("Particulars");
                                                        tableparts1.Cell(0, 1).SetFont(FontTable);
                                                        tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        //tableparts1.Cell(indx, 1).ColSpan = 4;

                                                        tableparts1.Cell(0, 2).SetContent("Rs.");
                                                        tableparts1.Cell(0, 2).SetFont(FontTable);
                                                        tableparts1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                                        tableparts1.Cell(0, 3).SetContent("Ps.");
                                                        tableparts1.Cell(0, 3).SetFont(FontTable);
                                                        tableparts1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tableparts1.Rows[0].SetRowHeight(20);
                                                        tableparts1.Rows[1].SetRowHeight(10);

                                                        foreach (PdfCell pr in tableparts1.CellRange(indx, 0, indx, 0).Cells)
                                                        {
                                                            pr.ColSpan = 4;
                                                        }

                                                        tableparts1.Cell(indx, 0).SetContent("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                                                        tableparts1.Cell(indx, 0).SetFont(FontTable);
                                                        tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tableparts1.Rows[indx].SetRowHeight(10);
                                                        indx++;

                                                        for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                                        {
                                                            string disphdr = string.Empty;
                                                            double allotamt0 = 0;
                                                            double deductAmt0 = 0;
                                                            double totalAmt0 = 0;
                                                            double paidAmt0 = 0;
                                                            double balAmt0 = 0;
                                                            double creditAmt0 = 0;

                                                            creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);

                                                            totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                            //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);


                                                            //paidAmt0 = totalAmt0 - balAmt0;
                                                            deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                            disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                            string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                            string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                            string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);
                                                            string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                            paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                            #region Monthwise
                                                            string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                            string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                                            int monWisemon = 0;
                                                            int monWiseYea = 0;
                                                            string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                            string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                            int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                                            int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                                            if (monWisemon > 0 && monWiseYea > 0)
                                                            {
                                                                string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                                DataSet dsMonwise = new DataSet();
                                                                dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                                if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                                {
                                                                    totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                                    paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                                    disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                                    balAmt0 = totalAmt0 - paidAmt0;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                            }
                                                            #endregion

                                                            //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                            feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                                            sno++;

                                                            totalamt += Convert.ToDouble(totalAmt0);
                                                            balanamt += Convert.ToDouble(balAmt0);
                                                            curpaid += Convert.ToDouble(creditAmt0);

                                                            deductionamt += Convert.ToDouble(deductAmt0);

                                                            tableparts1.Cell(indx, 0).SetContent(sno);
                                                            tableparts1.Cell(indx, 0).SetFont(FontTable);
                                                            tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                                                            tableparts1.Cell(indx, 1).SetContent(disphdr);
                                                            tableparts1.Cell(indx, 1).SetFont(FontTable);
                                                            tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                            //tableparts1.Cell(indx, 1).ColSpan = 4;

                                                            tableparts1.Cell(indx, 2).SetContent(returnIntegerPart(creditAmt0));
                                                            tableparts1.Cell(indx, 2).SetFont(FontTable);
                                                            tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                                            tableparts1.Cell(indx, 3).SetContent(returnDecimalPart(creditAmt0));
                                                            tableparts1.Cell(indx, 3).SetFont(FontTable);
                                                            tableparts1.Cell(indx, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            indx++;
                                                            createPDFOK = true;

                                                            sbHtml.Append("<tr><td style='text-align:center;'>" + sno + "</td><td colspan='5'  style='text-align:left;'>" + disphdr + "</td><td  style='text-align:right;'>" + returnIntegerPart(creditAmt0) + "</td><td  style='text-align:right;'>" + returnDecimalPart(creditAmt0) + "</td></tr>");
                                                        }

                                                        foreach (PdfCell pr in tableparts1.CellRange(indx, 0, indx, 0).Cells)
                                                        {
                                                            pr.ColSpan = 4;
                                                        }

                                                        tableparts1.Cell(indx, 0).SetContent("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                                                        tableparts1.Cell(indx, 0).SetFont(FontTable);
                                                        tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        tableparts1.Rows[indx].SetRowHeight(10);
                                                        indx++;
                                                        decimal totalamount = (decimal)curpaid;
                                                        tableparts1.Cell(indx, 1).SetContent("Total");
                                                        tableparts1.Cell(indx, 1).SetFont(FontTable);
                                                        tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts1.Cell(indx, 2).SetContent("" + returnIntegerPart((double)totalamount));
                                                        tableparts1.Cell(indx, 2).SetFont(FontTable);
                                                        tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                                        tableparts1.Cell(indx, 3).SetContent(returnDecimalPart((double)totalamount));
                                                        tableparts1.Cell(indx, 3).SetFont(FontTable);
                                                        tableparts1.Cell(indx, 3).SetContentAlignment
(ContentAlignment.MiddleCenter);



                                                        string endstatement = "<br>" + DecimalToWords(totalamount) + " Rupees Only." + "<br>Paid by " + mode + " Rs." + totalamount.ToString() + "/-.";
                                                        string finalstrig = "";
                                                        string examt = d2.GetFunction("   select isnull(ExcessAmt,0) from FT_ExcessDet where Dailytranscode = '" + recptNo + "'");
                                                        if (examt.Trim() != "" && examt.Trim() != "0")
                                                        {
                                                            finalstrig = "<br>Excess Amount  : " + examt;
                                                        }
                                                        if (paymode == "2")
                                                        {
                                                            finalstrig = finalstrig + "<br>Cheque : " + DDNo + "         Date  : " + DDDate;
                                                            finalstrig = finalstrig + "<br>Bank Name  : " + DDBankName;
                                                        }
                                                        if (paymode == "3")
                                                        {
                                                            finalstrig = finalstrig + "<br>DD : " + DDNo + "         Date  : " + DDDate;
                                                            finalstrig = finalstrig + "<br>Bank Name  : " + DDBankName;
                                                        }
                                                        string remarks = d2.GetFunction("select narration from ft_findailytransaction where TransCode='" + recptNo.Trim() + "' and app_no='" + appnoNew + "' and isnull(iscanceled,0)=0").Trim();
                                                        if (remarks.Trim() == "0")
                                                            remarks = string.Empty;

                                                        if (remarks != string.Empty)
                                                        {
                                                            finalstrig = finalstrig + "<br>Remarks : " + remarks;
                                                        }
                                                        endstatement = endstatement + finalstrig;

                                                        tableparts1.Cell(indx + 1, 0).SetContent(endstatement);
                                                        tableparts1.Cell(indx + 1, 0).SetFont(FontTable);
                                                        tableparts1.Cell(indx + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                        tableparts1.Cell(indx + 1, 0).ColSpan = 3;


                                                        PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 10, 80 + 150, 480, 500));
                                                        rcptpage.Add(addtabletopage2);

                                                        rcptpage.SaveToDocument();


                                                        sbHtml.Append("<tr><td colspan='6'  style='text-align:center;'>Total</td><td  style='text-align:right;'>" + returnIntegerPart((double)totalamount) + "</td><td  style='text-align:right;'>" + returnDecimalPart((double)totalamount) + "</td></tr><tr><td colspan='8'  style='text-align:left;'>" + endstatement + "</td></tr>");

                                                        //save changes
                                                        PdfPage rcptpageOf = rcptpage.CreateCopy();
                                                        PdfPage rcptpageTran = rcptpage.CreateCopy();
                                                        StringBuilder sboffCopy = new StringBuilder();
                                                        StringBuilder sbtranCopy = new StringBuilder();

                                                        if (officopy != 0)
                                                        {
                                                            sboffCopy.Append(sbHtml.ToString() + "<tr><td colspan='8'  style='text-align:left;'>Office Copy</td></tr></table></div><br>");
                                                            rcptpageOf.SaveToDocument();
                                                        }

                                                        if (transCopy != 0)
                                                        {
                                                            sbtranCopy.Append(sbHtml.ToString() + "<tr><td colspan='8'  style='text-align:left;'>Transport Copy</td></tr></table></div><br>");
                                                            rcptpageTran.SaveToDocument();
                                                        }
                                                        sbHtml.Append("<tr><td colspan='8'  style='text-align:left;'>Student Copy</td></tr></table></div><br>");
                                                        sbHtml.Append(sboffCopy.ToString() + sbtranCopy.ToString());
                                                        contentDiv.InnerHtml += sbHtml.ToString();
                                                        sbHtml.Clear();
                                                    }
                                                }
                                                #endregion
                                            }
                                            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
                                            finally
                                            {

                                            }
                                            createPDFOK = true;
                                        }
                                        else
                                        {
                                            imgAlert.Visible = true;
                                            lbl_alert.Text = "No Records Found";
                                        }
                                    }
                                    else
                                    {
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "No Records Found";
                                    }
                                }

                            }
                        }
                                            #endregion
                        #region To print the Receipt
                        if (createPDFOK)
                        {
                            #region Old
                            ////Response Write
                            //string appPath = HttpContext.Current.Server.MapPath("~");
                            //if (appPath != "")
                            //{
                            //    string szPath = appPath + "/Report/";
                            //    string szFile = "Receipt" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                            //    Response.Buffer = true;
                            //    Response.Clear();
                            //    recptDoc.SaveToFile(szPath + szFile);
                            //    //Response.ClearHeaders();
                            //    //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                            //    //Response.ContentType = "application/pdf";
                            //    //Response.WriteFile(szPath + szFile);

                            //    Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");
                            //    imgAlert.Visible = true;
                            //    lbl_alert.Text = "Receipt Generated";

                            //}
                            //else
                            //{
                            //    imgAlert.Visible = true;
                            //    lbl_alert.Text = "Server Path Not Found";
                            //}
                            #endregion
                            contentDiv.Visible = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            lbl_alert.Text = "Receipt Cannot Be Generated";
                        }
                        #endregion
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        lbl_alert.Text = "Please Add Print Settings";
                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Add Print Settings";
                }
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Receipt";
        }
    }
    public StringBuilder returnHtmlStringKCG(out bool createPDFOK, string Appno, string Rollno, string Collegecode, string Usercode, byte GpHdrType, string Studname, string RecptDt, string RcptTime, string RecptNo, string Cursem, string DegString, string DeptString, string Narration)
    {
        string appno = string.Empty;
        string rollno = string.Empty;
        string collegecode = string.Empty;
        string usercode = string.Empty;
        byte rcptType = 0;
        string studname = string.Empty;
        string recptDt = string.Empty;
        string rcptTime = string.Empty;
        string recptNo = string.Empty;
        string cursem = string.Empty;
        string degString = string.Empty;
        string deptString = string.Empty;
        string narration = string.Empty;

        appno = Appno;
        rollno = Rollno;
        collegecode = Collegecode;
        usercode = Usercode;
        rcptType = GpHdrType;
        studname = Studname;
        recptDt = RecptDt;
        recptNo = RecptNo;
        cursem = Cursem;
        degString = DegString;
        deptString = DeptString;
        rcptTime = RcptTime;
        narration = Narration;


        createPDFOK = false;
        StringBuilder sbHtml = new StringBuilder();
        try
        {
            #region Receipt Header

            sbHtml.Append("<div style='padding-left:5px;height:500px;'><table cellpadding='0' cellspacing='0' text-align:center; width: 700px;' class='classBold10'><tr><td>");

            sbHtml.Append("<table style='width:700px; height:20px; padding-left:5px;padding-top:60px; ' class='classReg12'><tr><td colspan='3' style='text-align:right;'>" + recptNo + "</td></tr></table><table style='width:700px; height:160px; padding-left:5px;padding-top:40px; ' class='classReg12'><tr><td colspan='3'><br><center>" + studname.ToUpper() + "</center><br/></td></tr><tr><td style='width:300px; text-align:right; '>" + "" + rollno + "</td><td style='width:300px;text-align:right;'>" + "" + romanLetter(returnYearforSem(cursem)) + " Year" + "</td><td style='width:300px; text-align:right;'>" + "" + recptDt + "</td></tr><tr><td style='width:200px; text-align:center;'><br>" + degString + "</td><td style='width:500px;margin-left:10px; text-align:center;' colspan='2'><br>" + deptString + "</td></tr></table>");

            #endregion

            #region Receipt Body

            sbHtml.Append("<div style='width:700px; height:218px; padding-top:30px;padding-left:-50px; '><table  class='classReg12'>");
            int rows = 0;

            int sno = 0;
            int indx = 0;
            double totalamt = 0;
            double balanamt = 0;
            double curpaid = 0;
            double deductionamt = 0;
            // double paidamount = 0;

            string selHeadersQ = string.Empty;
            DataSet dsHeaders = new DataSet();

            if (rcptType == 1 || rcptType == 2)
            {
                string StudStream = string.Empty;

                DataSet dsStr = new DataSet();
                dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + appno + "", "Text");
                if (dsStr.Tables.Count > 0)
                {
                    if (dsStr.Tables[0].Rows.Count > 0)
                    {
                        StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                    }
                }

                selHeadersQ = " select sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d ,FS_ChlGroupHeaderSettings f,FT_FeeAllot A,FM_LedgerMaster l where d.HeaderFK =f.HeaderFK and D.LedgerFK=l.LedgerPK  and  d.LedgerFK=a.LedgerFK and d.App_No=a.App_No and A.FeeCategory =D.FeeCategory  and    transcode='" + recptNo + "' and d.App_No ='" + appno + "'  ";
                if (StudStream != "")
                {
                    selHeadersQ += " and f.stream='" + StudStream + "' ";
                }
                selHeadersQ += "   group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
            }
            else if (rcptType == 3)
            {
                selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + appno + "' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk ";
            }
            else if (rcptType == 4)
            {
                selHeadersQ = " select D.LedgerFK,d.HeaderFK,D.FeeCategory,sum(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName ,D.DailyTransPk,A.Feeallotpk from FT_FinDailyTransaction d,FM_LedgerMaster l ,FT_FeeAllot A where d.LedgerFK =l.LedgerPK  and d.LedgerFK=a.LedgerFK  and d.FeeCategory =A.FeeCategory and  d.App_No=a.App_No and transcode='" + recptNo + "' and d.App_No ='" + appno + "' group by   l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk";
            }

            if (selHeadersQ != string.Empty)
            {
                string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                dsHeaders.Clear();
                dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                if (dsHeaders.Tables.Count > 0)
                {
                    if (dsHeaders.Tables[0].Rows.Count > 0)
                    {
                        rows += dsHeaders.Tables[0].Rows.Count;

                        for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                        {
                            string disphdr = string.Empty;
                            double allotamt0 = 0;
                            double deductAmt0 = 0;
                            double totalAmt0 = 0;
                            double paidAmt0 = 0;
                            double balAmt0 = 0;
                            double creditAmt0 = 0;

                            creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);

                            totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                            //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);


                            //paidAmt0 = totalAmt0 - balAmt0;
                            deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                            disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                            string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                            string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                            string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);
                            string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appno + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                            paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));


                            #region Monthwise
                            string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                            string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                            int monWisemon = 0;
                            int monWiseYea = 0;
                            string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                            string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                            int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                            int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                            if (monWisemon > 0 && monWiseYea > 0)
                            {
                                string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                DataSet dsMonwise = new DataSet();
                                dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                {
                                    totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                    disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                }
                            }
                            #endregion

                            balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                            feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode + "");
                            sno++;

                            totalamt += Convert.ToDouble(totalAmt0);
                            balanamt += Convert.ToDouble(balAmt0);
                            curpaid += Convert.ToDouble(creditAmt0);

                            deductionamt += Convert.ToDouble(deductAmt0);

                            indx++;
                            createPDFOK = true;

                            sbHtml.Append("<tr><td style='width:30px;text-align:left;'>" + sno + "</td><td style='width:470px;text-indent:40px;'>" + disphdr + "</td><td style='width:150px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "</td><td style='width:50px;text-align:right;'>" + returnDecimalPart(creditAmt0) + "</td></tr>");

                            if (head == (dsHeaders.Tables[0].Rows.Count - 1))
                            {
                                string exAmt = d2.GetFunction("select sum(isnull(amount,0)) as amount from ft_excessReceiptdet where app_no='" + appno + "' and receiptno='" + recptNo + "'").Trim();
                                double exAmtD = 0; double.TryParse(exAmt, out exAmtD);

                                if (exAmtD > 0)
                                {
                                    ++sno;
                                    sbHtml.Append("<tr><td style='width:30px;text-align:left;'>" + sno + "</td><td style='width:470px;text-indent:40px;'>Excess Amount </td><td style='width:150px;text-align:right;'>" + returnIntegerPart(exAmtD) + "</td><td style='width:50px;text-align:right;'>" + returnDecimalPart(exAmtD) + "</td></tr>");
                                    curpaid += exAmtD;
                                }
                            }
                        }

                        sbHtml.Append("</table></div>");

                        double totalamount = curpaid;

                        sbHtml.Append("<div><table  class='classReg12'><tr><td style='width:30px;text-align:right;'></td><td style='width:700px;text-indent:20;'>" + narration + "</td><td style='width:150px;text-align:right;'></td><td style='width:50px;text-align:right;'></td></tr><tr><td style='width:30px;text-align:right;'>&nbsp;</td><td style='width:470px; font-size:12px;'>(" + DecimalToWords((decimal)totalamount) + " Rupees Only.)</td></table><br/><br/><table  class='classReg12'></tr><tr><td style='width:150px;text-align:right;'></td><td style='width:50px;text-align:right;'></td></tr><tr><td style='width:30px;text-align:right;'>&nbsp;</td><td style='width:270px;'>&nbsp;</td><td style='width:210px;text-align:right;'>" + returnIntegerPart(totalamount) + "</td><td style='width:60px;text-align:right;'>" + returnDecimalPart(totalamount) + "</td></tr></table></div>");

                    }
                }
            }

            #endregion

            sbHtml.Append("</td></tr></table></div>");
        }
        catch { sbHtml.Clear(); }
        return sbHtml;
    }
    private string termDisplay(string cursem)
    {
        string Termdisp = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayTermForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ").Trim();

        string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
        if (linkvalue.Trim() == "1")
        {
            if (Termdisp == "1")
            {
                try
                {
                    double cursemester = Convert.ToDouble(cursem);

                    if (cursemester % 2 == 1)
                    {
                        cursem = romanLetter(cursemester.ToString()) + " & " + romanLetter((cursemester + 1).ToString());
                    }
                    else
                    {
                        cursem = romanLetter((cursemester - 1).ToString()) + " & " + romanLetter(cursemester.ToString());
                    }
                }
                catch { }
                cursem = "Term : " + cursem;
            }
            else
            {
                cursem = "Year : " + romanLetter(returnYearforSem(cursem));
            }
        }
        else
        {
            if (Termdisp == "1")
            {
                cursem = "Term : " + romanLetter(cursem);
            }
            else
            {
                cursem = "Semester : " + romanLetter(cursem);
            }
        }
        return cursem;
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {


        switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
        {
            case 0:
                txt_regno.Attributes.Add("placeholder", "Roll No");

                chosedmode = 0;
                break;
            case 1:
                txt_regno.Attributes.Add("placeholder", "Reg No");

                chosedmode = 1;
                break;
            case 2:
                txt_regno.Attributes.Add("placeholder", "Admin No");

                chosedmode = 2;
                break;
            case 3:
                txt_regno.Attributes.Add("placeholder", "App No");

                chosedmode = 3;
                break;
            case 4:
                txt_regno.Attributes.Add("placeholder", "Smartcard No");

                chosedmode = 4;
                break;
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select top 10 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%' and a.App_No in (select distinct App_No from FT_FinDailyTransaction where PayMode in (1,2,3) )";

        studhash = ws.Getnamevalue(query);
        if (studhash.Count > 0)
        {
            foreach (DictionaryEntry p in studhash)
            {
                string studname = Convert.ToString(p.Key);
                name.Add(studname);
            }
        }
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();
            //student query
            if (chosedmode == 0)
            {
                query = "select top 10 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecodestat + "  and App_No in (select distinct App_No from FT_FinDailyTransaction where PayMode in (1,2,3) ) order by Roll_No asc";
            }
            else if (chosedmode == 1)
            {
                query = "select  top 10 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code=" + collegecodestat + "  and App_No in (select distinct App_No from FT_FinDailyTransaction where PayMode in (1,2,3) )  order by Reg_No asc";
            }
            else if (chosedmode == 2)
            {
                query = "select  top 10 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecodestat + "  and App_No in (select distinct App_No from FT_FinDailyTransaction where PayMode in (1,2,3) )  order by Roll_admit asc";
            }
            else
            {
                byte studAppSHrtAdm = statStudentAppliedShorlistAdmit();
                string admStudFilter = "";
                switch (studAppSHrtAdm)
                {
                    case 0:
                        admStudFilter = " and isconfirm=1 ";
                        break;
                    case 1:
                        admStudFilter = " and isconfirm=1 and selection_status=1 ";
                        break;
                    case 2:
                        admStudFilter = " and isconfirm=1 and selection_status=1 and admission_status=1 ";
                        break;
                }
                query = "  select  top 10 app_formno from applyn where  app_formno like '" + prefixText + "%' and college_code=" + collegecodestat + "  and App_No in (select distinct App_No from FT_FinDailyTransaction where PayMode in (1,2,3) ) " + admStudFilter + "  order by app_formno asc";
            }


            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    private void loadCancelDuplicateButtons(string usercode, string collegecode)
    {
        try
        {
            string duplicate = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Duplicate Receipt For Student' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ").Trim();

            if (duplicate == "0")
            {
                btnChlnDuplicate.Visible = false;
                btnChlnOriginal.Visible = false;
            }
            else
            {
                btnChlnDuplicate.Visible = true;
                btnChlnOriginal.Visible = true;
            }

            string cancel = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Cancel Receipt' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ").Trim();

            if (cancel == "0")
            {
                btnChlnCancel.Visible = false;
            }
            else
            {
                btnChlnCancel.Visible = true;
            }

            string delete = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Delete Receipt' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ").Trim();

            if (delete == "0")
            {
                btnChlnDelete.Visible = false;
            }
            else
            {
                btnChlnDelete.Visible = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    //Reusable Methods
    private double excessRemaining(string appnoNew)
    {
        string excessamtQ = d2.GetFunction("select sum(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) as BalanceAmt from FT_ExcessDet WHERE  App_No=" + appnoNew + " ");

        double excessamtValue = 0;
        double.TryParse(excessamtQ, out excessamtValue);
        return excessamtValue;
    }
    private double retBalance(string appNo)
    {
        double ovBalAMt = 0;
        if (BalanceType == 1)
        {
            double.TryParse(d2.GetFunction(" select sum(isnull(totalAmount,0)-isnull(paidAmount,0)) as BalanceAmt from ft_feeallot where app_no =" + appNo + ""), out ovBalAMt);
        }
        return ovBalAMt;
    }
    public void isContainsDecimal(double myValue)
    {
        bool hasFractionalPart = (myValue - Math.Round(myValue) != 0);
    }
    public string returnIntegerPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 0)
        {
            strVal = strvalArr[0];
        }
        return strVal;
    }
    public string returnDecimalPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 1)
        {
            strVal = strvalArr[1];
            if (strVal.Length >= 2)
            {
                strVal = strVal.Substring(0, 2);
            }
            else
            {
                while (2 != strVal.Length)
                {
                    strVal = strVal + "0";
                }
            }
        }
        else
        {
            strVal = "00";
        }
        return strVal;
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
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " Lakhs";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + "  Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    public string DecimalToWords(decimal number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + DecimalToWords(Math.Abs(number));

        string words = "";

        int intPortion = (int)number;
        decimal fraction = (number - intPortion) * 100;
        int decPortion = (int)fraction;

        words = ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
        if (decPortion > 0)
        {
            words += " And ";
            words += ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
            words += " Paise ";
        }
        return words;
    }
    public string NumberToWords(int number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " Million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }

        return words;
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
    public bool checkedOK()
    {
        bool Ok = false;
        FpSpread1.SaveChanges();
        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
            {
                Ok = true;
            }
        }
        return Ok;
    }
    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }
    private void createLogo(string collCode)
    {
        try
        {
            string logoQ = "select logo1,logo2 from collinfo where college_code=" + collCode + "";
            DataSet dsLogo = d2.select_method_wo_parameter(logoQ, "Text");
            if (dsLogo.Tables.Count > 0 && dsLogo.Tables[0].Rows.Count > 0)
            {
                string logoname = Server.MapPath("~/FinanceLogo/Left_Logo" + collCode + ".jpeg");
                if (File.Exists(logoname))
                {
                    File.Delete(logoname);
                }
                if (!File.Exists(logoname))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    byte[] file = (byte[])dsLogo.Tables[0].Rows[0]["logo1"];
                    memoryStream.Write(file, 0, file.Length);
                    if (file.Length > 0)
                    {
                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                        thumb.Save(logoname, System.Drawing.Imaging.ImageFormat.Jpeg);

                    }
                    memoryStream.Dispose();
                    memoryStream.Close();
                }
                logoname = Server.MapPath("~/FinanceLogo/Right_Logo" + collCode + ".jpeg");
                if (File.Exists(logoname))
                {
                    File.Delete(logoname);
                }
                if (!File.Exists(logoname))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    byte[] file = (byte[])dsLogo.Tables[0].Rows[0]["logo2"];
                    memoryStream.Write(file, 0, file.Length);
                    if (file.Length > 0)
                    {
                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                        thumb.Save(logoname, System.Drawing.Imaging.ImageFormat.Jpeg);

                    }
                    memoryStream.Dispose();
                    memoryStream.Close();
                }
            }
        }
        catch (Exception ex) { }
    }
    protected void ddlBefAfteAdm_Indexchange(object sender, EventArgs e)
    {
        try
        {
            if (ddlBefAfteAdm.SelectedIndex == 0)
            {

            }
            else
            {
                rbl_rollno.SelectedIndex = rbl_rollno.Items.IndexOf(rbl_rollno.Items.FindByText("App No"));
                rbl_rollno_OnSelectedIndexChanged(sender, e);
                btn_go_Click(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate"); }
    }
    private bool AllowBankImpDup()
    {
        bool Move = false;
        string Q = "select LinkValue from New_InsSettings where LinkName='AllowDupReceiptBankimport' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
        int moveVal = 0;
        int.TryParse(d2.GetFunction(Q.Trim()), out moveVal);
        if (moveVal > 0)
        {
            Move = true;
        }
        return Move;
    }
    private byte StudentAppliedShorlistAdmit()
    {

        string Q = "select LinkValue from New_InsSettings where LinkName='StudentAppliedShorlistAdmit' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
        byte moveVal = 0;
        byte.TryParse(d2.GetFunction(Q.Trim()), out moveVal);
        return moveVal;
    }
    private static byte statStudentAppliedShorlistAdmit()
    {

        string Q = "select LinkValue from New_InsSettings where LinkName='StudentAppliedShorlistAdmit' and user_code ='" + usercodestat + "' --and college_code ='" + collegecodestat + "'";
        byte moveVal = 0;
        byte.TryParse(d22.GetFunction(Q.Trim()), out moveVal);
        return moveVal;
    }
    private string recptHeader(string transcode)
    {
        string recptHeaders = string.Empty;
        try
        {
            DataSet dsHeaders = new DataSet();
            dsHeaders = d2.select_method_wo_parameter("select distinct HeaderName from fm_headermaster h,ft_findailytransaction d where h.headerpk=d.headerfk and d.TransCode='" + transcode + "'", "Text");
            if (dsHeaders.Tables.Count > 0 && dsHeaders.Tables[0].Rows.Count > 0)
            {
                StringBuilder sbHead = new StringBuilder();
                for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                {
                    if (sbHead.Length == 0)
                    {
                        sbHead.Append(Convert.ToString(dsHeaders.Tables[0].Rows[head][0]));
                    }
                    else
                    {
                        sbHead.Append("," + Convert.ToString(dsHeaders.Tables[0].Rows[head][0]));
                    }
                }
                recptHeaders = sbHead.ToString();
            }
        }
        catch { recptHeaders = string.Empty; }
        return recptHeaders;
    }
    //Last modified by Idhris -- 25-01-2017

    #region roll,reg,admission setting
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }

    protected void spreadColumnVisible()
    {
        try
        {
            if (roll == 0)
            {
                FpSpread1.Columns[4].Visible = true;
                FpSpread1.Columns[5].Visible = true;
                FpSpread1.Columns[6].Visible = true;
            }
            else if (roll == 1)
            {
                FpSpread1.Columns[4].Visible = true;
                FpSpread1.Columns[5].Visible = true;
                FpSpread1.Columns[6].Visible = true;
            }
            else if (roll == 2)
            {
                FpSpread1.Columns[4].Visible = true;
                FpSpread1.Columns[5].Visible = false;
                FpSpread1.Columns[6].Visible = false;

            }
            else if (roll == 3)
            {
                FpSpread1.Columns[4].Visible = false;
                FpSpread1.Columns[5].Visible = true;
                FpSpread1.Columns[6].Visible = false;
            }
            else if (roll == 4)
            {
                FpSpread1.Columns[4].Visible = false;
                FpSpread1.Columns[5].Visible = false;
                FpSpread1.Columns[6].Visible = true;
            }
            else if (roll == 5)
            {
                FpSpread1.Columns[4].Visible = true;
                FpSpread1.Columns[5].Visible = true;
                FpSpread1.Columns[6].Visible = false;
            }
            else if (roll == 6)
            {
                FpSpread1.Columns[4].Visible = false;
                FpSpread1.Columns[5].Visible = true;
                FpSpread1.Columns[6].Visible = true;
            }
            else if (roll == 7)
            {
                FpSpread1.Columns[4].Visible = true;
                FpSpread1.Columns[5].Visible = false;
                FpSpread1.Columns[6].Visible = true;
            }
        }
        catch { }
    }

    #endregion
}

