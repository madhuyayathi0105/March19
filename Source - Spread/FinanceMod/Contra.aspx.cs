using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;


public partial class Contra : System.Web.UI.Page
{
    static string clgcode = "";
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    //  string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Boolean cellclick = false;
    static byte roll = 0;
    bool usBasedRights = false;
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
            // collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            //   college_code = Session["collegecode"].ToString();
            // clgcode = collegecode1;
            // college_code = Session["collegecode"].ToString(); 

            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddlclg.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddlclg.SelectedItem.Value);
                    clgcode = Convert.ToString(ddlclg.SelectedItem.Value);
                }
                // bindGrid();
                bindcollege();
                binddepartment();
                headerbind();
                ledgerbind();
                typeofintv();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                Txt_Todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Txt_Todate.Attributes.Add("readonly", "readonly");
                //  Txtamount.Attributes.Add("readonly", "readonly");
                // Txt_depositAmount.Attributes.Add("readonly", "readonly");
                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_date.Attributes.Add("readonly", "readonly");
                txtbankamtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtbankamtdate.Attributes.Add("readonly", "readonly");
                txtchedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtchedate.Attributes.Add("readonly", "readonly");
                txtaval.Attributes.Add("readonly", "readonly");
                txtbankaval.Attributes.Add("readonly", "readonly");
                txt_deptname.Attributes.Add("readonly", "readonly");
                txt_cat.Attributes.Add("readonly", "readonly");
                txt_desig.Attributes.Add("readonly", "readonly");
                txt_dptbank.Attributes.Add("readonly", "readonly");
                txt_dptdesg.Attributes.Add("readonly", "readonly");
                txt_dptcat.Attributes.Add("readonly", "readonly");
                txtptydt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtbkdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtptydt.Attributes.Add("readonly", "readonly");
                txtbkdt.Attributes.Add("readonly", "readonly");
                rb_entry.Checked = true;
                rb_petty.Checked = false;
                rb_dept.Checked = true;
                rb_with.Checked = false;
                rb_pety.Checked = true;
                RollAndRegSettings();
                SettingType();
                ddltype_indexChanged(sender, e);
                UserbasedRights();
            }
            if (ddlclg.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlclg.SelectedItem.Value);
                clgcode = Convert.ToString(ddlclg.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }
    protected void SettingType()
    {
        try
        {
            ddltype.Items.Clear();
            string rghtval = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Cash Deposit Cumulative' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'");
            if (rghtval == "0")
            {
                ddltype.Items.Add(new ListItem("Detail", "2"));
                // ddltype.SelectedItem.Value = "2";
            }
            else
            {
                ddltype.Items.Add(new ListItem("Cumulative", "1"));
                // ddltype.SelectedItem.Value = "1";
            }
        }
        catch { }
    }

    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsset = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsset = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            if (dsset.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsset.Tables[0].Rows.Count; hf++)
                {
                    if (dsset.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsset.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsset.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsset.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0")
                {
                    roll = 0;
                }
                else if (rollval == "1" && regval == "1")
                {
                    roll = 1;
                }
                else if (rollval == "1" || regval == "0")
                {
                    roll = 2;
                }
                else if (rollval == "0" || regval == "1")
                {
                    roll = 3;
                }
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }

    protected void ddlclg_indexChanged(object sender, EventArgs e)
    {
        if (ddlclg.Items.Count > 0)
        {
            collegecode1 = Convert.ToString(ddlclg.SelectedItem.Value);
            clgcode = Convert.ToString(ddlclg.SelectedItem.Value);
            SettingType();
            ddltype_indexChanged(sender, e);
        }
    }
    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ddlclg.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = ds;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
            }
        }
        catch
        { }
    }

    //public void fairpoint()
    //{
    //    FpSpreaddp.Sheets[0].RowCount = 0;
    //    FpSpreaddp.Sheets[0].ColumnCount = 0;
    //    FpSpreaddp.CommandBar.Visible = false;
    //    // FpSpread1.Sheets[0].AutoPostBack = false;

    //    FpSpreaddp.Sheets[0].ColumnHeader.RowCount = 1;
    //    FpSpreaddp.Sheets[0].RowHeader.Visible = false;
    //    FpSpreaddp.Sheets[0].ColumnCount = 3;
    //    FpSpreaddp.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //    FpSpreaddp.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
    //    FpSpreaddp.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Deposit";


    //    for (int i = 0; i < FpSpreaddp.Sheets[0].Columns.Count; i++)
    //    {
    //        FpSpreaddp.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
    //        // FpSpread1.Sheets[0].ColumnHeader.Columns[i].VerticalAlign = VerticalAlign.Middle;
    //        FpSpreaddp.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
    //        FpSpreaddp.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
    //        FpSpreaddp.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
    //        FpSpreaddp.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;

    //    }

    //    //if (ds.Tables.Count > 0)
    //    //{
    //    //    if (ds.Tables[0].Rows.Count > 0)
    //    //    {
    //    //        FpSpreaddp.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
    //    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //    //        {

    //    //            FpSpreaddp.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
    //    //            FpSpreaddp.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["cal_date"].ToString();
    //    //            FpSpreaddp.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["Credit"].ToString();
    //    //            FpSpreaddp.Sheets[0].Cells[i, 5].Text = ds.Tables[0].Rows[i]["Credit"].ToString();
    //    //           lbl_alert1.Visible = false;

    //    //        }

    //    //    }
    //    //    else
    //    //    {

    //    //        lbl_alert1.Text = "No Records Found";
    //    //        FpSpread1.Visible = false;

    //    //    }
    //    //}
    //    //else
    //    //{

    //    //    lbl_alert1.Text = "No Records Found";

    //    //    FpSpreaddp.Visible = false;

    //    //}



    //}

    public void typeofintv()
    {
        try
        {

            ddl_narrotion.Items.Clear();
            string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'invtp' and college_code = '" + Session["collegecode"].ToString() + "'";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_narrotion.DataSource = ds;
                ddl_narrotion.DataTextField = "TextVal";
                ddl_narrotion.DataValueField = "TextCode";
                ddl_narrotion.DataBind();
                // ddl_narrotion.Items.Insert(0, "---Select Narrotion---");

            }
            else
            {
                ddl_narrotion.Items.Add("Select");
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void bindcollege()
    {
        try
        {
            string clgname = "";
            ds.Clear();
            ddl_college2.Items.Clear();
            clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college2.DataSource = ds;
                ddl_college2.DataTextField = "collname";
                ddl_college2.DataValueField = "college_code";
                ddl_college2.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static List<string> GetWithdraw(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct top(10) (s.staff_name+'-'+ s.staff_code) as Staffname , s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    protected void binddepartment()
    {
        ds.Clear();
        //query = "";
        //query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + ddl_college2.SelectedValue.ToString() + "'";
        ds = d2.loaddepartment(ddl_college2.SelectedValue.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_department3.DataSource = ds;
            ddl_department3.DataTextField = "Dept_Name";
            ddl_department3.DataValueField = "Dept_Code";
            ddl_department3.DataBind();
            //ddl_department3.Items.Insert(0, "All");
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static List<string> Getresponsename(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct top(10) (s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name+'-'+ s.staff_code) as Staffname , s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    public void Txt_respnspersorns_OnTextChanged(object sender, EventArgs e)
    {
        string staffname = Txt_respnspersorns.Text.ToString();
        string staff_name = "";
        string prof = "";
        string dept = "";
        string pin = "";
        string[] strstaffname = staffname.Split('-');
        if (strstaffname.Length == 4)
        {
            staff_name = strstaffname[0].ToString();
            prof = strstaffname[1].ToString();
            dept = strstaffname[2].ToString();
            pin = strstaffname[3].ToString();
        }
        getstaffname(staff_name);

    }
    public void getstaffname(string staff_name)
    {
        try
        {
            string name = "";
            string deptname = "";
            string desgname = "";
            string catagname = "";
            string stafcode = "";
            //string query = "select s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode,CONVERT(varchar(10), s.join_date,103) as join_date,st.stftype  from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm,stafftrans st where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and st.staff_code =s.staff_code and latestrec =1 and s.staff_name='" + staff_name + "'";
            string query = "select distinct s.staff_code,s.staff_name ,d.staffcategory ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_name='" + staff_name + "' order by s.staff_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    name = ds.Tables[0].Rows[i]["staff_name"].ToString();
                    stafcode = ds.Tables[0].Rows[i]["staff_code"].ToString();
                    Txt_respnspersorns.Text = name + "-" + stafcode;
                    deptname = ds.Tables[0].Rows[i]["dept_name"].ToString();
                    txt_deptname.Text = deptname;
                    desgname = ds.Tables[0].Rows[i]["desig_name"].ToString();
                    txt_desig.Text = desgname;
                    catagname = ds.Tables[0].Rows[i]["staffcategory"].ToString();
                    txt_cat.Text = catagname;


                }
            }
            else
            {
                Txt_respnspersorns.Text = "";
                txt_deptname.Text = "";
                txt_desig.Text = "";
                txt_cat.Text = "";
            }

        }
        catch
        {

        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static List<string> Getdepositename(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct top(10) s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name+'-'+ s.staff_code, s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static List<string> Getwithbank(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct top(10) (s.staff_name+'-'+ s.staff_code) as Staffname , s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    public void Txt_respnspersornbank_OnTextChanged(object sender, EventArgs e)
    {
        string staffname = Txt_respnspersornbank.Text.ToString();
        string staff_name = "";
        string prof = "";
        string dept = "";
        string pin = "";
        string[] strstaffname = staffname.Split('-');
        if (strstaffname.Length == 4)
        {
            staff_name = strstaffname[0].ToString();
            prof = strstaffname[1].ToString();
            dept = strstaffname[2].ToString();
            pin = strstaffname[3].ToString();
        }
        getdepositestaffname(staff_name);

    }
    public void getdepositestaffname(string staff_name)
    {
        try
        {
            string name = "";
            string deptname = "";
            string desgname = "";
            string catagname = "";
            string stafcode = "";
            //string query = "select s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode,CONVERT(varchar(10), s.join_date,103) as join_date,st.stftype  from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm,stafftrans st where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and st.staff_code =s.staff_code and latestrec =1 and s.staff_name='" + staff_name + "'";
            string query = "select distinct s.staff_code,s.staff_name ,d.staffcategory ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_name='" + staff_name + "' order by s.staff_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    name = ds.Tables[0].Rows[i]["staff_name"].ToString();
                    stafcode = ds.Tables[0].Rows[i]["staff_code"].ToString();
                    Txt_respnspersornbank.Text = name + "-" + stafcode;
                    deptname = ds.Tables[0].Rows[i]["dept_name"].ToString();
                    txt_dptbank.Text = deptname;
                    desgname = ds.Tables[0].Rows[i]["desig_name"].ToString();
                    txt_dptdesg.Text = desgname;
                    catagname = ds.Tables[0].Rows[i]["staffcategory"].ToString();
                    txt_dptcat.Text = catagname;

                }
            }
            else
            {
                Txt_respnspersornbank.Text = "";
                txt_dptbank.Text = "";
                txt_dptdesg.Text = "";
                txt_dptcat.Text = "";
            }

        }
        catch
        {

        }
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
        catch
        {
        }
    }


    public void bankbind()
    {
        try
        {
            ddlbank.Items.Clear();
            string query = "select (accno+'-'+BankName) as BankName,BankCode,BankPK from FM_FinBankMaster where CollegeCode='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbank.DataSource = ds;
                ddlbank.DataTextField = "BankName";
                ddlbank.DataValueField = "BankPK";
                ddlbank.DataBind();
            }

        }
        catch
        {
        }
    }
    public void headerbind()
    {
        try
        {
            ddl_header.Items.Clear();
            // string query = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "' ORDER BY HeaderName";
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_header.DataSource = ds;
                ddl_header.DataTextField = "HeaderName";
                ddl_header.DataValueField = "HeaderPK";
                ddl_header.DataBind();
            }

        }
        catch
        {
        }
    }
    public void ledgerbind()
    {
        try
        {
            ddlledger.Items.Clear();
            string HeaderPK = ddl_header.SelectedItem.Value;
            // string query = "select LedgerPK,LedgerName from FM_LedgerMaster  where CollegeCode='" + collegecode1 + "' and HeaderFK='" + HeaderPK + "'  order by isnull(priority,1000), ledgerName asc ";


            //added by sudhagar 09-05-2016
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " and l.headerfk in('" + HeaderPK + "') AND  Ledgermode='1' and L.CollegeCode = " + collegecode1 + " order by isnull(l.priority,1000), l.ledgerName asc ";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlledger.DataSource = ds;
                ddlledger.DataTextField = "LedgerName";
                ddlledger.DataValueField = "LedgerPK";
                ddlledger.DataBind();
            }
        }
        catch
        {
        }
    }

    public void headwithbind()
    {
        try
        {
            ddlhead.Items.Clear();
            // string query = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "' ORDER BY HeaderName";

            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlhead.DataSource = ds;
                ddlhead.DataTextField = "HeaderName";
                ddlhead.DataValueField = "HeaderPK";
                ddlhead.DataBind();
            }

        }
        catch
        {
        }
    }
    public void ledgwithbind()
    {
        try
        {
            ddlledg.Items.Clear();
            string HeaderPK = ddlhead.SelectedItem.Value;
            // string query = "select LedgerPK,LedgerName from FM_LedgerMaster  where CollegeCode='" + collegecode1 + "' and HeaderFK='" + HeaderPK + "'  order by isnull(priority,1000), ledgerName asc ";
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='1' and l.headerfk in('" + HeaderPK + "') and L.CollegeCode = " + collegecode1 + " order by isnull(l.priority,1000), l.ledgerName asc ";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlledg.DataSource = ds;
                ddlledg.DataTextField = "LedgerName";
                ddlledg.DataValueField = "LedgerPK";
                ddlledg.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void txtfrom_TextChanged(object sender, EventArgs e)
    {
        //try
        //{

        //    lbl_alert.Visible = false;
        //    string fromdate = txt_fromdate.Text;
        //    string todate = Txt_Todate.Text;
        //    if (fromdate != "" && fromdate != null && todate != "" && todate != null)
        //    {
        //        string[] spiltfrom = fromdate.Split('/');
        //        string[] spitto = todate.Split('/');
        //        DateTime from = Convert.ToDateTime(spiltfrom[1] + '/' + spiltfrom[0] + '/' + spiltfrom[2]);
        //        DateTime to = Convert.ToDateTime(spitto[1] + '/' + spitto[0] + '/' + spitto[2]);

        //        if (from > to)
        //        {
        //            imgdiv2.Visible = true;
        //            lbl_alert.Visible = true;
        //            lbl_alert.Text = "Please Enter To Date Greater Than From Date";
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    lblerror.Visible = true;
        //    lblerror.Text = ex.ToString();
        //}

        //string firstdate = Convert.ToString(txt_fromdate.Text);
        //string seconddate = Convert.ToString(Txt_Todate.Text);

        //DateTime dt = new DateTime();
        //DateTime dt1 = new DateTime();
        //string[] split = firstdate.Split('/');
        //dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        //split = seconddate.Split('/');
        //dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        //if (dt > dt1)
        //{
        //    lblerror.Visible = true;
        //    lblerror.Text = "Please Enter To Date Greater Than From Date";
        //    imgdiv2.Visible = true;
        //    lbl_alert.Text = "Please Enter To Date Greater Than From Date";
        //    lbl_alert.ForeColor = Color.Red;
        //}


    }
    //protected void Txt_Todate_TextChanged(object sender, EventArgs e)
    //{
    //    string firstdate = txt_fromdate.Text;
    //    string todate = Txt_Todate.Text;
    //    string[] split = firstdate.Split('/');
    //    split = todate.Split('/');

    //    DateTime dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
    //    DateTime dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
    //    if (dt > dt1)
    //    {
    //        lblerror.Visible = true;
    //        lblerror.Text = "please Enter to Date High";
    //    }

    //}

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            UserbasedRights();
            lblvalidation1.Text = "";
            string firstdate = txt_fromdate.Text;
            string seconddate = Txt_Todate.Text;
            string hedname = "";
            string ledname = "";
            string petyamt = "";
            string bankname = "";
            string bankamt = "";

            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            if (ddlclg.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlclg.SelectedItem.Value);
            }
            string userCode = "";
            if (usBasedRights == true)
                userCode = " and EntryUserCode in('" + usercode + "')";
            string typedt = Convert.ToString(ddltype.SelectedItem.Value);
            string fincyr = d2.getCurrentFinanceYear(usercode, collegecode1);

            if (rb_dept.Checked == true)
            {
                int heigh = 0;
                if (rb_entry.Checked == true)
                {
                    if (typedt == "1")
                    {
                        string selqry = "select CONVERT(varchar(10), TransDate,103) as TransDate ,Credit,Debit,Debit-ISNULL( Credit,0) as received,ToPetty_amount,ToBank_amount from FT_FinCashTransaction where TransDate between  '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "' and FinYearFK='" + fincyr + "' " + userCode + " order by cast(transdate as datetime)";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selqry, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            #region entry
                            FpSpread1.Sheets[0].RowCount = 0;
                            FpSpread1.Sheets[0].ColumnCount = 0;
                            FpSpread1.CommandBar.Visible = false;
                            FpSpread1.Sheets[0].AutoPostBack = false;
                            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                            FpSpread1.Sheets[0].RowHeader.Visible = false;
                            FpSpread1.Sheets[0].ColumnCount = 11;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.White;
                            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                            FarPoint.Web.Spread.TextCellType debit = new FarPoint.Web.Spread.TextCellType();
                            FarPoint.Web.Spread.TextCellType credit = new FarPoint.Web.Spread.TextCellType();
                            FarPoint.Web.Spread.TextCellType bal = new FarPoint.Web.Spread.TextCellType();
                            chk.AutoPostBack = false;
                            FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                            cball.AutoPostBack = true;

                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 3);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                            FpSpread1.Sheets[0].Columns[1].Visible = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Date";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Received Amount";
                            FpSpread1.Columns[3].CellType = debit;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Deposit";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Balance Amount";
                            FpSpread1.Columns[5].CellType = credit;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Cash Type";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Deposit";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Status";



                            FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 7].Text = "Petty";
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 8, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 8].Text = "Bank";
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 9, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 9].Text = "Hand";
                            FpSpread1.Columns[9].CellType = bal;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, 10, 2, 1);
                            FpSpread1.Sheets[0].Rows.Count++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = cball;
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    FpSpread1.Sheets[0].Rows.Count++;
                                    heigh += 20;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(i + 1);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = chk;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = ds.Tables[0].Rows[i]["TransDate"].ToString();

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Text = ds.Tables[0].Rows[i]["Debit"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Text = ds.Tables[0].Rows[i]["Credit"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 5].Text = ds.Tables[0].Rows[i]["received"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 7].Text = ds.Tables[0].Rows[i]["ToPetty_amount"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 8].Text = ds.Tables[0].Rows[i]["ToBank_amount"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 9].Text = ds.Tables[0].Rows[i]["received"].ToString();

                                    lbl_alert1.Visible = false;
                                }
                                FpSpread1.Sheets[0].Columns[0].Width = 60;
                                FpSpread1.Sheets[0].Columns[1].Width = 100;
                                FpSpread1.Sheets[0].Columns[2].Width = 80;
                                FpSpread1.Sheets[0].Columns[3].Width = 100;
                                FpSpread1.Sheets[0].Columns[4].Width = 100;
                                FpSpread1.Sheets[0].Columns[5].Width = 100;
                                FpSpread1.Sheets[0].Columns[6].Width = 100;
                                FpSpread1.Sheets[0].Columns[7].Width = 100;
                                FpSpread1.Sheets[0].Columns[8].Width = 100;
                                FpSpread1.Sheets[0].Columns[9].Width = 100;
                                FpSpread1.Sheets[0].Columns[10].Width = 100;

                                for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
                                {
                                    FpSpread1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
                                }
                                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Right;
                                for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
                                {

                                    FpSpread1.Sheets[0].Columns[i].Locked = true;
                                }
                                FpSpread1.Sheets[0].Columns[1].Locked = false;
                                FpSpread1.Sheets[0].Columns[6].Visible = false;
                            }
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].Rows.Count;
                            FpSpread1.ShowHeaderSelection = false;
                            FpSpread1.SaveChanges();
                            btnDeposit.Visible = true;
                            divfp.Visible = true;
                            FpSpread1.Visible = true;
                            heigh += 100;
                            FpSpread1.Height = heigh;
                            print.Visible = false;
                            #endregion
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "No Records Found";
                            lbl_alert.Visible = true;
                            FpSpread1.Visible = false;
                            btnDeposit.Visible = false;
                        }
                    }
                    else
                    {
                        if (rb_entry.Checked == true)
                        {
                            if (cbinclude.Checked == false)
                            {
                                #region without deposit
                                string deptfull = "";
                                string strdt = "";
                                deptfull = " and isnull(f.IsDepositedFully,'0')='0'";
                                strdt = " and f.TransDate between  '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "'";

                                string selqry = "select SUM(debit)as debit,TransCode,CONVERT(varchar(10), TransDate,103) as TransDate,r.Stud_Name,r.Roll_No,r.Reg_No,r.Sections,case when f.PayMode=1 then 'cash'  end paymode,r.degree_code,f.app_no,f.IsDepositedFully from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,course c where f.App_No =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  " + strdt + " and FinYearFK='" + fincyr + "' and r.college_code='" + collegecode1 + "' " + deptfull + " " + userCode + " ";
                                //and f.PayMode='1'
                                selqry += "  group by  TransCode,r.Stud_Name,r.Roll_No,r.Reg_No,r.Sections,f.PayMode,TransDate,r.degree_code,f.app_no,f.IsDepositedFully order by cast(TransDate as datetime)";
                                selqry = selqry + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode1 + "'";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selqry, "Text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    #region design
                                    RollAndRegSettings();
                                    FpSpread1.Sheets[0].RowCount = 0;
                                    FpSpread1.Sheets[0].ColumnCount = 0;
                                    FpSpread1.CommandBar.Visible = false;
                                    FpSpread1.Sheets[0].AutoPostBack = false;
                                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                                    FpSpread1.Sheets[0].ColumnCount = 11;
                                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                    darkstyle.ForeColor = Color.White;
                                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                                    FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                                    FarPoint.Web.Spread.TextCellType debit = new FarPoint.Web.Spread.TextCellType();
                                    FarPoint.Web.Spread.TextCellType credit = new FarPoint.Web.Spread.TextCellType();
                                    FarPoint.Web.Spread.TextCellType bal = new FarPoint.Web.Spread.TextCellType();
                                    chk.AutoPostBack = false;
                                    FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                                    cball.AutoPostBack = true;


                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;


                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;

                                    // FpSpread1.Columns[3].CellType = debit;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = lbldept.Text;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Section";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

                                    // FpSpread1.Columns[5].CellType = credit;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Date";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Transcode";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Paymode";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Left;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Amount";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;


                                    if (roll == 0)
                                    {
                                        FpSpread1.Sheets[0].Columns[3].Visible = true;
                                        FpSpread1.Sheets[0].Columns[4].Visible = true;
                                    }
                                    else if (roll == 1)
                                    {
                                        FpSpread1.Sheets[0].Columns[3].Visible = true;
                                        FpSpread1.Sheets[0].Columns[4].Visible = true;
                                    }
                                    else if (roll == 2)
                                    {
                                        FpSpread1.Sheets[0].Columns[3].Visible = true;
                                        FpSpread1.Sheets[0].Columns[4].Visible = false;
                                    }
                                    else if (roll == 3)
                                    {
                                        FpSpread1.Sheets[0].Columns[3].Visible = false;
                                        FpSpread1.Sheets[0].Columns[4].Visible = true;
                                    }

                                    FpSpread1.Sheets[0].Columns[0].Width = 60;
                                    FpSpread1.Sheets[0].Columns[1].Width = 80;
                                    FpSpread1.Sheets[0].Columns[2].Width = 180;
                                    FpSpread1.Sheets[0].Columns[3].Width = 100;
                                    FpSpread1.Sheets[0].Columns[4].Width = 100;
                                    FpSpread1.Sheets[0].Columns[5].Width = 250;
                                    FpSpread1.Sheets[0].Columns[6].Width = 100;
                                    FpSpread1.Sheets[0].Columns[7].Width = 100;
                                    FpSpread1.Sheets[0].Columns[8].Width = 100;
                                    FpSpread1.Sheets[0].Columns[9].Width = 100;
                                    FpSpread1.Sheets[0].Columns[10].Width = 100;
                                    #endregion
                                    string cbtype = "";
                                    double grandtot = 0;
                                    double gdtot = 0;
                                    bool type = false;

                                    FpSpread1.Sheets[0].Rows.Count++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = cball;
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        FpSpread1.Sheets[0].Rows.Count++;
                                        heigh += 20;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(i + 1);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = chk;
                                        if (cbinclude.Checked == true)
                                        {
                                            cbtype = Convert.ToString(ds.Tables[0].Rows[i]["IsDepositedFully"]);
                                            if (cbtype != "")
                                                type = Convert.ToBoolean(cbtype);

                                            if (type == true)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Locked = true;
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                                                type = false;
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Locked = false;
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.White;
                                            }
                                        }

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].CellType = chk;


                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["App_no"]);

                                        DataView dnew = new DataView();
                                        string degreename = "";
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["Degree_code"]) + "' ";
                                            dnew = ds.Tables[1].DefaultView;
                                            if (dnew.Count > 0)
                                                degreename = Convert.ToString(dnew[0]["degreename"]);
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 5].Text = degreename;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Sections"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["paymode"]);
                                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["debit"]), out gdtot);
                                        grandtot += gdtot;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["debit"]);

                                    }
                                    #region grand total

                                    for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
                                    {

                                        FpSpread1.Sheets[0].Columns[i].Locked = true;
                                    }
                                    FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                                    FpSpread1.Sheets[0].Rows.Count++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 8);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(grandtot);
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");

                                    #endregion

                                    grandtot = 0;
                                    FpSpread1.Sheets[0].Columns[1].Locked = false;
                                    FpSpread1.ShowHeaderSelection = false;
                                    FpSpread1.Sheets[0].FrozenColumnCount = 3;
                                    FpSpread1.SaveChanges();
                                    btnDeposit.Visible = true;
                                    divfp.Visible = true;
                                    FpSpread1.Visible = true;
                                    heigh += 100;
                                    FpSpread1.Height = heigh;
                                    if (cbinclude.Checked == true)
                                    {
                                        btnDeposit.Visible = false;
                                        print.Visible = true;
                                        FpSpread1.Sheets[0].Columns[1].Visible = false;
                                    }
                                    else
                                    {
                                        btnDeposit.Visible = true;
                                        print.Visible = false;
                                        FpSpread1.Sheets[0].Columns[1].Visible = true;
                                    }

                                }
                                else
                                {
                                    print.Visible = false;
                                    imgdiv2.Visible = true;
                                    lbl_alert.Text = "No Records Found";
                                    lbl_alert.Visible = true;
                                    FpSpread1.Visible = false;
                                    btnDeposit.Visible = false;
                                }
                                #endregion
                            }
                            else
                            {
                                #region withdate

                                string deptfull = "";
                                string strdt = "";
                                deptfull = " and isnull(f.IsDepositedFully,'0')='1'";
                                strdt = " and f.DepositedDate between  '" + dt.ToString("MM/dd/yyyy") + "' AND '" + dt1.ToString("MM/dd/yyyy") + "'";
                                string selqry = "select SUM(debit)as debit,TransCode,CONVERT(varchar(10), TransDate,103) as TransDate,r.Stud_Name,r.Roll_No,r.Reg_No,r.Sections,case when f.PayMode=1 then 'cash'  end paymode,r.degree_code,f.app_no,f.IsDepositedFully from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,course c where f.App_No =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and f.PayMode='1' " + strdt + " and FinYearFK='" + fincyr + "' and r.college_code='" + collegecode1 + "' " + deptfull + " " + userCode + " ";
                                selqry += "  group by  TransCode,r.Stud_Name,r.Roll_No,r.Reg_No,r.Sections,f.PayMode,TransDate,r.degree_code,f.app_no,f.IsDepositedFully order by cast(TransDate as datetime)";
                                selqry = selqry + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode1 + "'";
                                selqry += "select distinct CONVERT(varchar(10), TransDate,103) as TransDate from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,course c where f.App_No =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and f.PayMode='1' " + strdt + " and FinYearFK='" + fincyr + "' and r.college_code='" + collegecode1 + "' " + deptfull + " " + userCode + " ";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selqry, "Text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    #region design
                                    RollAndRegSettings();
                                    FpSpread1.Sheets[0].RowCount = 0;
                                    FpSpread1.Sheets[0].ColumnCount = 0;
                                    FpSpread1.CommandBar.Visible = false;
                                    FpSpread1.Sheets[0].AutoPostBack = false;
                                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                                    FpSpread1.Sheets[0].ColumnCount = 9;
                                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                    darkstyle.ForeColor = Color.White;
                                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                                    FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                                    FarPoint.Web.Spread.TextCellType debit = new FarPoint.Web.Spread.TextCellType();
                                    FarPoint.Web.Spread.TextCellType credit = new FarPoint.Web.Spread.TextCellType();
                                    FarPoint.Web.Spread.TextCellType bal = new FarPoint.Web.Spread.TextCellType();
                                    chk.AutoPostBack = false;
                                    FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                                    cball.AutoPostBack = true;


                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[0].Locked = true;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Transcode";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[1].Locked = true;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[2].Locked = true;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Columns[3].Locked = true;

                                    // FpSpread1.Columns[3].CellType = debit;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[4].Locked = true;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = lbldept.Text;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[5].Locked = true;

                                    // FpSpread1.Columns[5].CellType = credit;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Section";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[6].Locked = true;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Paymode";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[7].Locked = true;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Amount";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[8].Locked = true;


                                    if (roll == 0)
                                    {
                                        FpSpread1.Sheets[0].Columns[3].Visible = true;
                                        FpSpread1.Sheets[0].Columns[4].Visible = true;
                                    }
                                    else if (roll == 1)
                                    {
                                        FpSpread1.Sheets[0].Columns[3].Visible = true;
                                        FpSpread1.Sheets[0].Columns[4].Visible = true;
                                    }
                                    else if (roll == 2)
                                    {
                                        FpSpread1.Sheets[0].Columns[3].Visible = true;
                                        FpSpread1.Sheets[0].Columns[4].Visible = false;
                                    }
                                    else if (roll == 3)
                                    {
                                        FpSpread1.Sheets[0].Columns[3].Visible = false;
                                        FpSpread1.Sheets[0].Columns[4].Visible = true;
                                    }

                                    FpSpread1.Sheets[0].Columns[0].Width = 60;
                                    FpSpread1.Sheets[0].Columns[1].Width = 120;
                                    FpSpread1.Sheets[0].Columns[2].Width = 180;
                                    FpSpread1.Sheets[0].Columns[3].Width = 100;
                                    FpSpread1.Sheets[0].Columns[4].Width = 100;
                                    FpSpread1.Sheets[0].Columns[5].Width = 250;
                                    FpSpread1.Sheets[0].Columns[6].Width = 100;
                                    FpSpread1.Sheets[0].Columns[7].Width = 100;
                                    FpSpread1.Sheets[0].Columns[8].Width = 100;
                                    #endregion

                                    #region value

                                    double grandtot = 0;
                                    double gdtot = 0;
                                    DataView dvdt = new DataView();
                                    Hashtable httot = new Hashtable();
                                    DataSet dsload = new DataSet();
                                    ArrayList ardt = new ArrayList();
                                    int rolcnt = 0;
                                    for (int sel = 0; sel < ds.Tables[2].Rows.Count; sel++)
                                    {
                                        string gnrldt = Convert.ToString(ds.Tables[2].Rows[sel]["Transdate"]);
                                        ds.Tables[0].DefaultView.RowFilter = "Transdate='" + gnrldt + "'";
                                        dvdt = ds.Tables[0].DefaultView;
                                        DataTable dttbl = new DataTable();
                                        if (dvdt.Count > 0)
                                        {
                                            // dttbl = dvdt.ToTable();
                                            // dsload.Clear();
                                            // dsload.Tables.Add(new DataTable[]{dttbl.Copy()});
                                            //  if(sel!=0)
                                            // dsload.Tables.Remove(dttbl.Copy());

                                            //dsload.Tables.Add(dttbl.Copy());
                                            //if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
                                            //{
                                            // for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                                            for (int i = 0; i < dvdt.Count; i++)
                                            {
                                                string trdt = Convert.ToString(dvdt[i]["Transdate"]);
                                                if (!ardt.Contains(trdt))
                                                {
                                                    ardt.Add(trdt);
                                                    FpSpread1.Sheets[0].Rows.Count++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = trdt;
                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                                }
                                                FpSpread1.Sheets[0].Rows.Count++;
                                                rolcnt++;
                                                heigh += 30;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(rolcnt);
                                                //   FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(dvdt[i]["TransCode"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = Convert.ToString(dvdt[i]["Stud_Name"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Text = Convert.ToString(dvdt[i]["Roll_No"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Tag = Convert.ToString(dvdt[i]["TransCode"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(dvdt[i]["Reg_No"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Tag = Convert.ToString(dvdt[i]["App_no"]);

                                                DataView dnew = new DataView();
                                                string degreename = "";
                                                if (ds.Tables[1].Rows.Count > 0)
                                                {
                                                    ds.Tables[1].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dvdt[i]["Degree_code"]) + "' ";
                                                    dnew = ds.Tables[1].DefaultView;
                                                    if (dnew.Count > 0)
                                                        degreename = Convert.ToString(dnew[0]["degreename"]);
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 5].Text = degreename;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 6].Text = Convert.ToString(dvdt[i]["Sections"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Tag = Convert.ToString(dvdt[i]["TransDate"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 7].Text = Convert.ToString(dvdt[i]["paymode"]);
                                                double.TryParse(Convert.ToString(dvdt[i]["debit"]), out gdtot);
                                                grandtot += gdtot;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 8].Text = Convert.ToString(dvdt[i]["debit"]);
                                            }

                                            #region total

                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(grandtot);
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.RoyalBlue;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                                            if (!httot.ContainsKey(gnrldt))
                                            {
                                                httot.Add(gnrldt, Convert.ToString(grandtot));
                                            }
                                            grandtot = 0;
                                            #endregion
                                        }
                                    }
                                    #endregion

                                    #region grand total

                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Gold;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    if (httot.Count > 0)
                                    {
                                        double gdAmt = 0;
                                        double fnlgdAmt = 0;
                                        foreach (DictionaryEntry grdamout in httot)
                                        {
                                            double.TryParse(Convert.ToString(grdamout.Value), out gdAmt);
                                            fnlgdAmt += gdAmt;
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(fnlgdAmt);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Bold = true;
                                    }
                                    #endregion

                                    FpSpread1.ShowHeaderSelection = false;
                                    FpSpread1.Sheets[0].FrozenColumnCount = 3;
                                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                    FpSpread1.SaveChanges();
                                    btnDeposit.Visible = true;
                                    divfp.Visible = true;
                                    FpSpread1.Visible = true;
                                    heigh += 200;
                                    FpSpread1.Height = heigh;
                                    btnDeposit.Visible = false;
                                    print.Visible = true;

                                }
                                else
                                {
                                    print.Visible = false;
                                    imgdiv2.Visible = true;
                                    lbl_alert.Text = "No Records Found";
                                    lbl_alert.Visible = true;
                                    FpSpread1.Visible = false;
                                    btnDeposit.Visible = false;
                                }

                                #endregion
                            }

                        }
                    }
                }
                else if (rb_report.Checked == true)
                {
                    #region report
                    string selqry = "SELECT CONVERT (varchar (20),TransDate) as TransDate,SUM(Debit) as amount FROM FT_FinCashContraDet Where TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and FinYearFK='" + fincyr + "'  GROUP BY TransDate  order by cast(TransDate as datetime)";
                    selqry = selqry + " SELECT CONVERT(varchar(10), TransDate,103) as TransDate,HeaderName,LedgerName,SUM(ISNULL(Debit,0)-ISNULL(credit,0)) as petyamount,l.priority FROM FT_FinCashContraDet T,FM_HeaderMaster H,FM_LedgerMaster L WHERE T.HeaderFK = H.HeaderPK AND T.LedgerFK = L.LedgerPK ANd IsPetty = 1 AND TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and FinYearFK='" + fincyr + "'  GROUP BY TransDate,HeaderName,LedgerName,l.priority   order by cast(TransDate as datetime),isnull(l.priority,1000), l.ledgerName asc";
                    // order by isnull(priority,1000), ledgerName asc 
                    selqry = selqry + " SELECT CONVERT(varchar(10), TransDate,103) as TransDate,BankName,SUM(ISNULL(Debit,0)-ISNULL(credit,0)) as bankamount FROM FT_FinCashContraDet T,FM_FinBankMaster B WHERE T.BankFK = B.BankPK  ANd IsBank  = 1 AND TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and FinYearFK='" + fincyr + "' GROUP BY TransDate,BankName order by cast(TransDate as datetime)";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selqry, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        #region design
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 7;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.TextCellType depot = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType pyamt = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType bkamt = new FarPoint.Web.Spread.TextCellType();
                        chk.AutoPostBack = false;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 3);

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Date";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Received Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Deposit";

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Balance Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Pety";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, 3);

                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Header";
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Ledger";
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Amount";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 3, 1, 3);

                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Bank";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 1, 2);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Bank Name";
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Amount";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 1, 2);

                        #endregion
                        DataView dv = new DataView();
                        DataView dv1 = new DataView();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chk;
                            string dtt = "";
                            dtt = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"].ToString());
                            DateTime dttt = new DateTime();
                            dttt = Convert.ToDateTime(dtt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dttt.ToString("MM/dd/yyyy");
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["amount"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = depot;
                            int check = 0;

                            int index = 0;
                            ds.Tables[1].DefaultView.RowFilter = "TransDate='" + dttt.ToString("dd/MM/yyyy") + "'";
                            dv = ds.Tables[1].DefaultView;
                            ds.Tables[2].DefaultView.RowFilter = "TransDate='" + dttt.ToString("dd/MM/yyyy") + "'";
                            dv1 = ds.Tables[2].DefaultView;
                            #region old
                            //if (ds.Tables[1].Rows.Count > 0)
                            //{
                            //    for (int ii = 0; ii < ds.Tables[1].Rows.Count; ii++)
                            //    {
                            //        check++;
                            //        if (ii != 0)
                            //        {

                            //            FpSpread1.Sheets[0].RowCount++;
                            //        }
                            //        else
                            //        {
                            //            index = FpSpread1.Sheets[0].RowCount - 1;
                            //        }
                            //        hedname = Convert.ToString(ds.Tables[1].Rows[ii]["HeaderName"]);
                            //        if (hedname != "")
                            //        {
                            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = hedname;
                            //        }
                            //        else
                            //        {
                            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "-";
                            //        }
                            //        ledname = Convert.ToString(ds.Tables[1].Rows[ii]["LedgerName"]);
                            //        if (ledname != "")
                            //        {
                            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ledname;
                            //        }
                            //        else
                            //        {
                            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = "-";
                            //        }
                            //        petyamt = Convert.ToString(ds.Tables[1].Rows[ii]["petyamount"]);
                            //        if (petyamt != "")
                            //        {
                            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = petyamt;
                            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = pyamt;
                            //        }
                            //        else
                            //        {
                            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "-";
                            //        }

                            //    }
                            //}
                            #endregion
                            if (dv.Count > 0)
                            {
                                for (int ii = 0; ii < dv.Count; ii++)
                                {
                                    check++;
                                    if (ii != 0)
                                    {

                                        FpSpread1.Sheets[0].RowCount++;
                                    }
                                    else
                                    {
                                        index = FpSpread1.Sheets[0].RowCount - 1;
                                    }
                                    hedname = Convert.ToString(dv[ii]["HeaderName"]);
                                    if (hedname != "")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = hedname;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "-";
                                    }
                                    ledname = Convert.ToString(dv[ii]["LedgerName"]);
                                    if (ledname != "")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ledname;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = "-";
                                    }
                                    petyamt = Convert.ToString(dv[ii]["petyamount"]);
                                    if (petyamt != "")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = petyamt;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = pyamt;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "-";
                                    }

                                }
                            }
                            #region old
                            //if (ds.Tables[2].Rows.Count > 0)
                            //{
                            //    for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
                            //    {
                            //        if (index >= check)
                            //        {
                            //            index = FpSpread1.Sheets[0].RowCount++;
                            //        }
                            //        else
                            //        {
                            //            index++;
                            //        }
                            //        bankname = Convert.ToString(ds.Tables[2].Rows[j]["BankName"]);
                            //        if (bankname != "")
                            //        {
                            //            FpSpread1.Sheets[0].Cells[index - 1, 9].Text = bankname;
                            //        }
                            //        else
                            //        {
                            //            FpSpread1.Sheets[0].Cells[index - 1, 9].Text = "-";
                            //        }
                            //        bankamt = Convert.ToString(ds.Tables[2].Rows[j]["bankamount"]);
                            //        if (bankamt != "")
                            //        {
                            //            FpSpread1.Sheets[0].Cells[index - 1, 10].Text = bankamt;
                            //            FpSpread1.Sheets[0].Cells[index - 1, 10].CellType = bkamt;
                            //        }
                            //        else
                            //        {
                            //            FpSpread1.Sheets[0].Cells[index - 1, 10].Text = "-";
                            //        }
                            //    }
                            //}
                            #endregion
                            if (dv1.Count > 0)
                            {
                                for (int j = 0; j < dv1.Count; j++)
                                {
                                    if (index >= check)
                                    {
                                        index = FpSpread1.Sheets[0].RowCount++;
                                    }
                                    else
                                    {
                                        index++;
                                    }
                                    bankname = Convert.ToString(dv1[j]["BankName"]);
                                    if (bankname != "")
                                    {
                                        FpSpread1.Sheets[0].Cells[index - 1, 9].Text = bankname;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[index - 1, 9].Text = "-";
                                    }
                                    bankamt = Convert.ToString(dv1[j]["bankamount"]);
                                    if (bankamt != "")
                                    {
                                        FpSpread1.Sheets[0].Cells[index - 1, 10].Text = bankamt;
                                        FpSpread1.Sheets[0].Cells[index - 1, 10].CellType = bkamt;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[index - 1, 10].Text = "-";
                                    }
                                }
                            }
                        }
                        #region grandtotal
                        FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 4);
                        double hedval = 0;
                        double fnl = 0;
                        double hedval1 = 0;
                        double fnl1 = 0;
                        double hedval2 = 0;
                        double fnl2 = 0;
                        for (int i = 0; i < FpSpread1.Rows.Count - 1; i++)
                        {
                            if (Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 4].Text) == "-")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString("-");
                            }
                            else if (Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 4].Value) != 0)
                            {
                                hedval = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 4].Value);
                                fnl += hedval;
                                // hedval = 0;
                            }
                            if (Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 8].Text) == "-")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 8].Text = Convert.ToString("-");
                            }
                            else if (Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 8].Value) != 0)
                            {
                                hedval1 = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 8].Value);
                                fnl1 += hedval1;
                            }
                            if (Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 10].Text) == "-")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 10].Text = Convert.ToString("-");
                            }
                            else if (Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 10].Value) != 0)
                            {
                                hedval2 = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 10].Value);
                                fnl2 += hedval2;

                            }

                            // FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 5, 1, 1);
                            // FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 2, 0, 1, 4);

                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(fnl);
                        fnl = 0;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 8].Text = Convert.ToString(fnl1);
                        fnl1 = 0;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 10].Text = Convert.ToString(fnl2);
                        fnl2 = 0;
                        FpSpread1.Sheets[0].Columns[1].Visible = false;
                        FpSpread1.Sheets[0].Columns[3].Visible = false;
                        FpSpread1.Sheets[0].Columns[5].Visible = false;
                        for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
                        }
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.ShowHeaderSelection = false;
                        FpSpread1.SaveChanges();
                        lbl_alert1.Visible = false;
                        btnDeposit.Visible = false;
                        divfp.Visible = true;
                        FpSpread1.Visible = true;
                        FpSpread1.Height = 280;
                        print.Visible = false;
                        #endregion

                    }
                    else
                    {
                        print.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "No Records Found";
                        lbl_alert.Visible = true;
                        FpSpread1.Visible = false;
                        btnDeposit.Visible = false;
                    }
                    #endregion
                }
            }
            if (rb_with.Checked == true)
            {

                DataView dvhead = new DataView();
                DataView dv = new DataView();
                if (rb_report.Checked == true)
                {
                    #region withdraw report

                    string selqry = "select SUM(DEBIT) as totalamt, TransDate  FROM FT_FinContraWithDrawDet WHERE  TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' group by TransDate order by TransDate select CONVERT(varchar(20), TransDate,103) as TransDate,SUM(DEBIT) as petyamt,HeaderName,LedgerName,l.priority   FROM FT_FinContraWithDrawDet t,FM_HeaderMaster H,FM_LedgerMaster L WHERE T.HeaderfK = H.HeaderPK AND T.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND  TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'  GROUP BY transdate,BankFK,HeaderName,LedgerName,l.priority order by isnull(l.priority,1000), l.ledgerName asc select CONVERT(varchar(20), TransDate,103) as TransDate,SUM(DEBIT) as bankamt,BankName,AccNo  FROM FT_FinContraWithDrawDet t,FM_FinBankMaster B WHERE T.BankFK = B.BankPK AND  TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'  GROUP BY transdate,BankFK,BankName,AccNo order by cast(TransDate as datetime) ";
                    //isnull(l.priority,1000), l.ledgerName asc
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selqry, "Text");
                    //  and FinYearFK='" + fincyr + "'
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        #region design
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 4;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.TextCellType txtAccno = new FarPoint.Web.Spread.TextCellType();
                        chk.AutoPostBack = false;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 3);

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "TotalAmount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Pety";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, 3);

                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Header";
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Ledger";
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Amount";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 3, 1, 3);

                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Bank";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 1, 2);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Bank Name";
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Account No";
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Amount";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 1, 2);
                        #endregion
                        string accnum = "";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (i == 0)
                                FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            string dtt = "";
                            dtt = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"].ToString());
                            DateTime dttt = new DateTime();
                            dttt = Convert.ToDateTime(dtt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dttt.ToString("MM/dd/yyyy");
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["totalamt"]);

                            int check = 0;
                            int index = 0;
                            ds.Tables[1].DefaultView.RowFilter = "TransDate='" + dttt.ToString("dd/MM/yyyy") + "'";
                            dvhead = ds.Tables[1].DefaultView;
                            ds.Tables[2].DefaultView.RowFilter = "TransDate='" + dttt.ToString("dd/MM/yyyy") + "'";
                            dv = ds.Tables[2].DefaultView;

                            if (dvhead.Count > 0)
                            {
                                for (int ii = 0; ii < dvhead.Count; ii++)
                                {
                                    check++;
                                    if (ii != 0)
                                        FpSpread1.Sheets[0].RowCount++;
                                    else
                                        index = FpSpread1.Sheets[0].RowCount - 1;

                                    hedname = Convert.ToString(dvhead[ii]["HeaderName"]);
                                    if (hedname != "")
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = hedname;
                                    else
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "-";

                                    ledname = Convert.ToString(dvhead[ii]["LedgerName"]);
                                    if (ledname != "")
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ledname;
                                    else
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "-";

                                    petyamt = Convert.ToString(dvhead[ii]["petyamt"]);
                                    if (petyamt != "")
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = petyamt;
                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = pyamt;
                                    else
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "-";

                                }
                            }

                            if (dv.Count > 0)
                            {
                                for (int j = 0; j < dv.Count; j++)
                                {
                                    if (index >= check)
                                        index = FpSpread1.Sheets[0].RowCount++;
                                    else
                                        index++;

                                    bankname = Convert.ToString(dv[j]["BankName"]);
                                    if (bankname != "")
                                        FpSpread1.Sheets[0].Cells[index - 1, 6].Text = bankname;
                                    else
                                        FpSpread1.Sheets[0].Cells[index - 1, 6].Text = "-";

                                    accnum = Convert.ToString(dv[j]["AccNo"]);
                                    if (accnum != "")
                                    {
                                        FpSpread1.Sheets[0].Cells[index - 1, 7].Text = accnum;
                                        FpSpread1.Sheets[0].Cells[index - 1, 7].CellType = txtAccno;
                                    }
                                    else
                                        FpSpread1.Sheets[0].Cells[index - 1, 7].Text = "-";

                                    bankamt = Convert.ToString(dv[j]["bankamt"]);
                                    if (bankamt != "")
                                        FpSpread1.Sheets[0].Cells[index - 1, 8].Text = bankamt;
                                    // FpSpread1.Sheets[0].Cells[index - 1, 8].CellType = bkamt;
                                    else
                                        FpSpread1.Sheets[0].Cells[index - 1, 8].Text = "-";

                                }
                            }
                        }
                        // FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        #region grandtotal
                        FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                        double hedval = 0;
                        double fnl = 0;
                        double hedval1 = 0;
                        double fnl1 = 0;
                        double hedval2 = 0;
                        double fnl2 = 0;
                        for (int i = 0; i < FpSpread1.Rows.Count - 1; i++)
                        {
                            if (Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 2].Text) == "-")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = Convert.ToString("-");
                            }
                            else if (Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 2].Value) != 0)
                            {
                                hedval = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 2].Value);
                                fnl += hedval;
                            }
                            if (Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 5].Text) == "-")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 5].Text = Convert.ToString("-");
                            }
                            else if (Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 5].Value) != 0)
                            {
                                hedval1 = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 5].Value);
                                fnl1 += hedval1;
                            }
                            if (Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 8].Text) == "-")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 8].Text = Convert.ToString("-");
                            }
                            else if (Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 8].Value) != 0)
                            {
                                hedval2 = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 8].Value);
                                fnl2 += hedval2;
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = Convert.ToString(fnl);
                        fnl = 0;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 5].Text = Convert.ToString(fnl1);
                        fnl = 0;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 8].Text = Convert.ToString(fnl2);
                        fnl2 = 0;
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");
                        #endregion
                        for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
                        {
                            FpSpread1.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[i].Font.Name = "Book Antiqua";
                            // FpSpread1.Sheets[0].Columns[i].Font.Bold = true;
                            FpSpread1.Sheets[0].Columns[i].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
                        }
                        for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
                        {
                            FpSpread1.Sheets[0].Columns[i].Locked = true;
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        btnDeposit.Visible = false;
                        FpSpread1.Visible = true;
                        FpSpread1.Height = 280;
                        divfp.Visible = true;
                        FpSpread1.ShowHeaderSelection = false;
                        FpSpread1.SaveChanges();
                        print.Visible = false;
                    }

                    else
                    {
                        print.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "No Records Found";
                        lbl_alert.Visible = true;
                        FpSpread1.Visible = false;

                    }
                    #endregion
                }
            }
        }
        catch
        {
        }
    }


    public void bindGrid()
    {
        FpSpread1.SaveChanges();
        DataTable dt = new DataTable();
        Hashtable htdt = new Hashtable();
        double tempamt = 0;
        dt.Columns.Add("Sno");
        dt.Columns.Add("Date");
        dt.Columns.Add("Deposit");
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        double totamt = 0;
        string typedt = Convert.ToString(ddltype.SelectedItem.Value);
        if (typedt == "1")
        {
            for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
            {
                int valu = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value);
                if (valu == 1)
                {
                    DataRow dr;
                    dr = dt.NewRow();
                    dr[1] = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    dr[2] = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Text);
                    totamt += Convert.ToDouble(FpSpread1.Sheets[0].Cells[i, 5].Text);
                    dt.Rows.Add(dr);
                }
            }
        }
        else
        {
            for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
            {
                int valu = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value);
                if (valu == 1)
                {
                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Text), out tempamt);
                    if (!htdt.ContainsKey(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text)))
                        htdt.Add(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text), tempamt);
                    else
                    {
                        double total = 0;
                        double.TryParse(Convert.ToString(htdt[Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text)]), out total);
                        total += tempamt;
                        htdt.Remove(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text));
                        htdt.Add(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text), Convert.ToString(total));

                    }
                    totamt += Convert.ToDouble(FpSpread1.Sheets[0].Cells[i, 10].Text);
                }
            }
            foreach (DictionaryEntry value in htdt)
            {
                DataRow dr;
                dr = dt.NewRow();
                dr[1] = Convert.ToString(value.Key);
                dr[2] = Convert.ToString(value.Value);
                dt.Rows.Add(dr);
            }
        }
        lblamount.Text = Convert.ToString(totamt);
        ViewState["amount"] = Convert.ToString(totamt);

        if (dt.Rows.Count > 0)
        {
            gridView1.DataSource = dt;
            gridView1.DataBind();
        }
    }
    protected void btnDeposit_Click(object sender, EventArgs e)
    {
        try
        {
            Txt_accno.Text = "";
            Txt_accname.Text = "";
            Txt_bankname.Text = "";
            Txt_branch.Text = "";
            Txt_respnspersornbank.Text = "";
            txt_dptbank.Text = "";
            txt_dptdesg.Text = "";
            txt_dptcat.Text = "";
            Txt_respnspersorns.Text = "";
            txt_deptname.Text = "";
            txt_desig.Text = "";
            txt_cat.Text = "";
            rb_petty.Checked = true;
            rb_petty_CheckedChanged(sender, e);
            headerbind();
            ledgerbind();
            bindGrid();
            Grid.Visible = true;
            gridView1.Visible = true;
            typeofintv();

            string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            popwindow.Visible = true;
            double value = 0;
            int check = 0;
            string typedt = Convert.ToString(ddltype.SelectedItem.Value);
            if (typedt == "1")
            {
                for (int i = 1; i < FpSpread1.Rows.Count; i++)
                {
                    if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 1].Value) == 1)
                    {
                        if (value == 0)
                        {
                            value = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 5].Value);
                            check = Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 1].Value);
                        }
                        else
                        {
                            value = value + Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 5].Value);
                            check = Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 1].Value);
                        }

                    }
                }
                Txtamount.Enabled = true;
            }
            else
            {
                for (int i = 1; i < FpSpread1.Rows.Count - 1; i++)
                {
                    if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 1].Value) == 1)
                    {
                        if (value == 0)
                        {
                            value = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 10].Value);
                            check = Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 1].Value);
                        }
                        else
                        {
                            value = value + Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 10].Value);
                            check = Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), 1].Value);
                        }

                    }
                }
                Txtamount.Enabled = false;
            }
            if (check == 1)
            {
                if (value == 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Balance Amount is 0   Pleace Select again";
                    lbl_alert.ForeColor = Color.Red;
                    popwindow.Visible = false;
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any One";
                lbl_alert.ForeColor = Color.Red;
                popwindow.Visible = false;
            }
            Txtamount.Text = Convert.ToString(value);
            Txt_depositAmount.Text = Convert.ToString(value);
            //  Txt_depositAmount.Attributes.Add("readonly","readonly");

            // btnSearch_Click(sender, e);
            rb_petty.Checked = true;
        }
        catch
        {
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {

    }
    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        cellclick = true;

    }
    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void FpSpread1_Command(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            string value = "";
            if (actrow != "")
            {
                int arow = Convert.ToInt32(actrow);
                if (arow == 0)
                {
                    value = Convert.ToString(FpSpread1.Sheets[0].Cells[0, 1].Value);
                    if (value == "1")
                    {
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            if (FpSpread1.Sheets[0].Cells[i, 1].Locked == false)
                            {
                                FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            if (FpSpread1.Sheets[0].Cells[i, 1].Locked == false)
                            {
                                FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }

    protected void btnplus_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = true;
        panel_description.Visible = true;

    }

    protected void btnnarrotion_Click(object sender, EventArgs e)
    {

        try
        {
            if (txt_narrotion.Text.Trim() != "")
            {
                string add = " insert into textvaltable (TextVal,TextCriteria,college_code) values ('" + txt_narrotion.Text + "', 'invtp','" + Session["collegecode"].ToString() + "')";
                int a = d2.update_method_wo_parameter(add, "Text");

                //imgdiv2.Visible = true;
                //lbl_alert.Visible = true;
                //lbl_alert.Text = "Type of Narrotion Added Successfully";
                //lbl_alert.ForeColor = Color.Green;
                typeofintv();
                txt_narrotion.Text = "";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Enter Type of Narrotion";
                lbl_alert.ForeColor = Color.Red;
            }

        }
        catch (Exception ex)
        {

        }
        imgdiv3.Visible = false;


    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }

    //public void loaddesc()
    //{
    //    ddl_group.Items.Clear();
    //    ds.Tables.Clear();

    //    string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='HCGrp' and college_code ='" + collegecode1 + "'";
    //    //ds = d2.select_method_wo_parameter(sql, "TEXT");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        ddl_group.DataSource = ds;
    //        ddl_group.DataTextField = "TextVal";
    //        ddl_group.DataValueField = "TextCode";
    //        ddl_group.DataBind();
    //        // ddl_group.Items.Insert(0, new ListItem("--Select--", "0"));
    //    }
    //    else
    //    {
    //        //ddl_group.Items.Insert(0, new ListItem("--Select--", "0"));
    //    }

    //}
    protected void btndescpopexit_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;


    }
    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {
            string narrotion = ddl_narrotion.SelectedItem.Value.ToString();
            string add = " delete from textvaltable where TextCode='" + narrotion + "' and TextCriteria='invtp' and college_code='" + Session["collegecode"].ToString() + "' ";
            int a = d2.update_method_wo_parameter(add, "Text");
            imgdiv2.Visible = true;
            pnl2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = "Narrotion Deleted Successfully";
            lbl_alert.ForeColor = Color.Green;
            if (narrotion == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No records found";

            }

            typeofintv();
            txt_narrotion.Text = "";
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        //imgdiv2.Visible = false;
    }

    protected void btnresponspersorn_Click(object sender, EventArgs e)
    {
        popupsscode1.Visible = true;
        Fpstaff.Visible = false;
        btn_save1.Visible = false;
        btn_exit2.Visible = false;
        Txt_respnspersorns.Text = "";
        txt_deptname.Text = "";
        txt_desig.Text = "";
        txt_cat.Text = "";
        binddepartment();
        txt_searchby.Text = "";
        Txt_respnspersornbank.Text = "";
        txt_dptbank.Text = "";
        txt_dptdesg.Text = "";
        txt_dptcat.Text = "";
        //  ddl_searchby.SelectedIndex = 0;


    }
    protected void btnsave_Click(object sender, EventArgs e)
    {

    }
    protected void btnsave1_Click(object sender, EventArgs e)
    {

    }
    protected void btnsavebank_Click(object sender, EventArgs e)
    {
        try
        {
            string typedt = Convert.ToString(ddltype.SelectedItem.Value);
            string date1 = "";
            double amount = 0;
            bool save = false;
            string ResponseStaff = Txt_respnspersorns.Text;
            string code = Txt_respnspersornbank.Text.ToString();
            string Staffcode = "";
            if (code.Length > 0)
            {
                string[] splcode = code.Split('-');
                string codest = splcode[1].ToString();
                Staffcode = d2.GetFunction("select appl_id from staffmaster s,staff_appl_master h where  s.appl_no =h.appl_no and s.staff_code='" + codest + "'");
            }
            string Debit = Txt_depositAmount.Text;

            string date = "";
            string curdt = DateTime.Now.ToString("MM/dd/yyyy");
            string datee = Convert.ToString(txtbkdt.Text);
            string[] spldt = datee.Split('/');
            if (spldt.Length > 0)
                date = spldt[1] + "/" + spldt[0] + "/" + spldt[2];

            DateTime dtcr = Convert.ToDateTime(curdt);
            DateTime dtdept = Convert.ToDateTime(date);
            bool dtval = false;
            while (dtcr >= dtdept)
            {
                dtval = true;
                break;
            }
            if (dtval == true)
            {
                string Currenttime = Convert.ToString(DateTime.Now.ToLongTimeString());
                string Financialyear = d2.getCurrentFinanceYear(usercode, collegecode1);

                string bankcode = d2.GetFunction(" select BankPK  from FM_FinBankMaster where AccNo='" + Txt_accno.Text + "'");
                bankcode = Convert.ToString(bankcode);
                if (bankcode != "" && Debit != "")
                {
                    string insert = "if exists (select * from FT_FinCashContraDet where BankFK='" + bankcode + "' and TransDate ='" + date + "' and FinYearFK='" + Financialyear + "' ) update FT_FinCashContraDet set TransTime='" + Currenttime + "',Credit='0',Debit=Debit+'" + Debit + "',IsHand='0',IsPetty='0',IsBank='1'  where BankFK='" + bankcode + "' and TransDate ='" + date + "' and FinYearFK='" + Financialyear + "'  else insert into FT_FinCashContraDet (TransDate,TransTime,Credit,Debit,IsHand,IsPetty,IsBank,FinYearFK,BankFK)values ('" + date + "','" + Currenttime + "','0','" + Debit + "','0','0','1','" + Financialyear + "','" + bankcode + "')";
                    int insertvalue1 = d2.update_method_wo_parameter(insert, "Text");
                    save = true;
                }
                //,ResponsePerson='" + Staffcode + "'
                if (bankcode != "" && Debit != "")
                {
                    //and EntryUserCode='" + usercode + "'
                    string insert2 = "insert into FT_FinBankTransaction (TransDate,TransTime,BankFK,PayMode,DailyTransID,IsDeposited,IsCleared,IsBounced,Credit,Debit,FinYearFK,EntryUserCode) values ('" + date + "','" + Currenttime + "','" + bankcode + "','1','0','1','1','0','0','" + Debit + "','" + Financialyear + "','" + usercode + "')";
                    int insertvalue2 = d2.update_method_wo_parameter(insert2, "Text");
                    save = true;
                }

                if (gridView1.Rows.Count > 0)
                {
                    for (int i = 0; i < gridView1.Rows.Count; i++)
                    {
                        TextBox txtdeposit1 = (TextBox)gridView1.Rows[i].FindControl("txtdeposit");

                        if (txtdeposit1.Text.Trim() != "")
                        {
                            double fnlamt = 0;
                            //   date1 += = Convert.ToInt32(GridView1.Rows[i].Cells[0].Text); 
                            date1 = Convert.ToString((gridView1.Rows[i].FindControl("lbl_date") as Label).Text);
                            string[] split = date1.Split('/');
                            DateTime dtnew = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                            amount = Convert.ToDouble(txtdeposit1.Text.ToString());
                            if (Convert.ToDouble(Debit) <= Convert.ToDouble(amount))
                            {
                                fnlamt = Convert.ToDouble(Debit);
                                Debit = "0";
                            }
                            else
                            {
                                Debit = Convert.ToString(Convert.ToDouble(Debit) - Convert.ToDouble(amount));
                                fnlamt = Convert.ToDouble(amount);
                            }
                            string insqry = "  UPDATE FT_FinCashTransaction SET Credit=ISNULL( Credit,0) + " + fnlamt + ",ToBank='1',ToBank_amount=ISNULL(ToBank_amount,0)+" + fnlamt + " WHERE TransDate='" + dtnew.ToString("MM/dd/yyyy") + "' and EntryUserCode='" + usercode + "'";
                            int update = d2.update_method_wo_parameter(insqry, "Text");
                            save = true;
                        }
                    }
                }
                double colval = 0;
                if (typedt == "2")
                {
                    for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
                    {
                        double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Value), out colval);
                        if (colval == 1)
                        {
                            string transcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Tag);
                            string appno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);
                            string Selq = "update FT_FinDailyTransaction set IsDepositedFully='1' where TransCode='" + transcode + "' and PayMode='1' and App_no='" + appno + "'";
                            int upd = d2.update_method_wo_parameter(Selq, "Text");
                        }
                    }
                }

                if (save == true)
                {
                    btnSearch_Click(sender, e);
                    imgdiv2.Visible = true;
                    pnl2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = " Saved Sucessfully ";
                    popwindow.Visible = false;
                    Txt_accno.Text = "";
                    Txt_accname.Text = "";
                    Txt_bankname.Text = "";
                    Txt_branch.Text = "";
                    Txt_depositAmount.Text = "";
                    Txt_respnspersornbank.Text = "";
                }
                else
                {
                    lbl_alert.Text = "Please Enter The Correct Values";
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;
                }
            }
            else
            {
                lbl_alert.Text = "Deposit Date Must Be Current Date Or Past Date";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
            }
        }
        catch
        {
        }

    }
    protected void btnsavepetty_Click(object sender, EventArgs e)
    {
        try
        {
            string typedt = Convert.ToString(ddltype.SelectedItem.Value);
            bool save = false;
            string Narration = ddl_narrotion.SelectedItem.Value;
            string ResponseStaff = Txt_respnspersorns.Text;
            string code = Txt_respnspersorns.Text.ToString();
            string Staffcode = "";
            if (code.Length > 0)
            {
                string[] splcode = code.Split('-');
                string codest = splcode[1].ToString();
                Staffcode = d2.GetFunction("select appl_id from staffmaster s,staff_appl_master h where  s.appl_no =h.appl_no and s.staff_code='" + codest + "'");
            }
            string Debit = Convert.ToString(Txtamount.Text);
            string date1 = "";
            double amount = 0;
            int value = 0;
            string curdt = DateTime.Now.ToString("MM/dd/yyyy");
            string date = "";
            string datee = Convert.ToString(txtptydt.Text);
            string[] spldt = datee.Split('/');
            if (spldt.Length > 0)
                date = spldt[1] + "/" + spldt[0] + "/" + spldt[2];

            DateTime dtcr = Convert.ToDateTime(curdt);
            DateTime dtdept = Convert.ToDateTime(date);
            bool dtval = false;
            while (dtcr >= dtdept)
            {
                dtval = true;
                break;
            }
            if (dtval == true)
            {
                string Currenttime = Convert.ToString(DateTime.Now.ToLongTimeString());
                string Financialyear = d2.getCurrentFinanceYear(usercode, collegecode1);

                string headname = ddl_header.SelectedItem.Value.ToString();
                string ledname = ddlledger.SelectedItem.Value.ToString();
                if (headname != "" && ledname != "" && Debit != "")
                {
                    //if()
                    string insert = "if exists (select * from FT_FinCashContraDet where HeaderFK ='" + headname + "' and LedgerFK ='" + ledname + "' and TransDate ='" + date + "' and FinYearFK='" + Financialyear + "'  ) update FT_FinCashContraDet set TransTime='" + Currenttime + "',Credit='0',Debit=Debit+'" + Debit + "',IsHand='0',IsPetty='1',IsBank='0',Narration='" + Narration + "',ResponsePerson='" + Staffcode + "' where HeaderFK ='" + headname + "' and LedgerFK ='" + ledname + "' and TransDate ='" + date + "' and FinYearFK='" + Financialyear + "'  else  insert into FT_FinCashContraDet (TransDate,TransTime,Credit,Debit,IsHand,IsPetty,IsBank,Narration,ResponsePerson,FinYearFK,HeaderFK,LedgerFK)values ('" + date + "','" + Currenttime + "','0','" + Debit + "','0','1','0','" + Narration + "','" + Staffcode + "','" + Financialyear + "','" + headname + "','" + ledname + "')";
                    int insertvalue = d2.update_method_wo_parameter(insert, "Text");
                    save = true;


                    if (gridView1.Rows.Count > 0)
                    {
                        for (int i = 0; i < gridView1.Rows.Count; i++)
                        {
                            TextBox txtdeposit1 = (TextBox)gridView1.Rows[i].FindControl("txtdeposit");

                            if (txtdeposit1.Text.Trim() != "")
                            {
                                double bal = 0;
                                double fnlamt = 0;
                                date1 = Convert.ToString((gridView1.Rows[i].FindControl("lbl_date") as Label).Text);
                                DateTime dtnew = new DateTime();
                                string[] split = date1.Split('/');
                                if (split.Length > 0)
                                {
                                    dtnew = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                                }
                                amount = Convert.ToDouble(txtdeposit1.Text.ToString());
                                if (Convert.ToDouble(Debit) <= Convert.ToDouble(amount))
                                {
                                    fnlamt = Convert.ToDouble(Debit);
                                    Debit = "0";
                                }
                                else
                                {
                                    Debit = Convert.ToString(Convert.ToDouble(Debit) - Convert.ToDouble(amount));
                                    fnlamt = Convert.ToDouble(amount);

                                }

                                string insqry = "  UPDATE FT_FinCashTransaction SET Credit=ISNULL( Credit,0) + " + fnlamt + ",ToPetty='1',ToPetty_amount=ISNULL(ToPetty_amount,0)+" + fnlamt + " WHERE TransDate='" + dtnew.ToString("MM/dd/yyyy") + "' and EntryUserCode='" + usercode + "'";
                                int update = d2.update_method_wo_parameter(insqry, "Text");
                                save = true;
                            }
                        }
                    }
                    double colval = 0;
                    if (typedt == "2")
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
                        {
                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Value), out colval);
                            if (colval == 1)
                            {
                                string transcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Tag);
                                string appno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);
                                string Selq = "update FT_FinDailyTransaction set IsDepositedFully='1' where TransCode='" + transcode + "' and PayMode='1' and App_no='" + appno + "'";
                                int upd = d2.update_method_wo_parameter(Selq, "Text");
                            }
                        }
                    }
                }
                if (save == true)
                {
                    btnSearch_Click(sender, e);
                    imgdiv2.Visible = true;
                    pnl2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = " Saved Sucessfully ";
                    popwindow.Visible = false;
                    Txt_respnspersorns.Text = "";
                }
                else
                {
                    lbl_alert.Text = "Please Enter The Correct Values";
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;
                }
            }
            else
            {
                lbl_alert.Text = "Deposit Date Must Be Current Date Or Past Date";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
            }
        }
        catch
        {
        }
    }
    protected void btnplus1_Click(object sender, EventArgs e)
    {

    }
    protected void btnminus1_Click(object sender, EventArgs e)
    {

    }

    protected void ddl_header_SelectedIndexChanged(object sender, EventArgs e)
    {
        ledgerbind();
    }
    protected void ddl_Ledger_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void rb_petty_CheckedChanged(object sender, EventArgs e)
    {
        rb_petty.Checked = true;
        rb_bank.Checked = false;
        Petty.Visible = true;
        Grid.Visible = true;
        Bank.Visible = false;
        Txt_respnspersorns.Text = "";
        txt_deptname.Text = "";
        txt_desig.Text = "";
        txt_cat.Text = "";
        binddepartment();
        txt_searchby.Text = "";
        Txt_respnspersornbank.Text = "";
        txt_dptbank.Text = "";
        txt_dptdesg.Text = "";
        txt_dptcat.Text = "";
        Txt_accno.Text = "";
        Txt_accname.Text = "";
        Txt_bankname.Text = "";
        Txt_branch.Text = "";
        headerbind();
        ledgerbind();
        //spreadselect.Visible = true;
    }
    protected void rb_bank_CheckedChanged(object sender, EventArgs e)
    {
        rb_bank.Checked = true;
        rb_petty.Checked = false;
        Petty.Visible = false;
        Grid.Visible = true;
        Bank.Visible = true;
        Txt_respnspersorns.Text = "";
        txt_deptname.Text = "";
        txt_desig.Text = "";
        txt_cat.Text = "";
        binddepartment();
        txt_searchby.Text = "";
        Txt_respnspersornbank.Text = "";
        txt_dptbank.Text = "";
        txt_dptdesg.Text = "";
        txt_dptcat.Text = "";
        Txt_accno.Text = "";
        Txt_accname.Text = "";
        Txt_bankname.Text = "";
        Txt_branch.Text = "";
        if (ddltype.SelectedItem.Value == "2")
            Txt_depositAmount.Enabled = false;
        else
            Txt_depositAmount.Enabled = true;
        //spreadselect.Visible = true;
    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        // popwindow.Visible = false;
        popupsscode1.Visible = false;
    }
    protected void btn_go2_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = "";
            int rolcount = 0;
            int rowcount = 0;
            int sno = 0;
            if (txt_searchby.Text != "")
            {
                if (ddl_searchby.SelectedIndex == 0)
                {
                    sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name,d.staffcategory from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.Staff_name ='" + Convert.ToString(txt_searchby.Text) + "' order by s.staff_code";
                }
            }
            else if (txt_wardencode.Text.Trim() != "")
            {
                if (ddl_searchby.SelectedIndex == 1)
                {
                    sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name,d.staffcategory  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_code ='" + Convert.ToString(txt_wardencode.Text) + "' order by s.staff_code";
                }
            }
            else
            {
                sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name,d.staffcategory  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and h.dept_code in ('" + ddl_department3.SelectedItem.Value + "') order by s.staff_code";
            }
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.SaveChanges();
            Fpstaff.SheetCorner.ColumnCount = 0;

            Fpstaff.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpstaff.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
            Fpstaff.Sheets[0].SpanModel.Add(Fpstaff.Sheets[0].RowCount - 1, 0, 1, 3);
            Fpstaff.Sheets[0].AutoPostBack = false;
            ds = d2.select_method_wo_parameter(sql, "Text");
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.Sheets[0].ColumnCount = 5;

            if (ds.Tables[0].Rows.Count > 0)
            {

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[0].Locked = true;
                Fpstaff.Columns[0].Width = 80;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[1].Locked = true;
                Fpstaff.Columns[1].Width = 100;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[2].Locked = true;
                Fpstaff.Columns[2].Width = 200;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[3].Locked = true;
                Fpstaff.Columns[3].Width = 250;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpstaff.Columns[4].Width = 200;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[4].Locked = true;
                Fpstaff.Width = 700;

                for (rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["staffcategory"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_code"]);
                }
                Fpstaff.Visible = true;
                btn_save1.Visible = true;
                btn_exit2.Visible = true;

                lbl_errorsearch.Visible = true;
                lbl_errorsearch.Text = "No Records Found";
                lbl_errorsearch.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                Fpstaff.Height = 370;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();
                err.Visible = false;
            }
            else
            {
                Fpstaff.Visible = false;
                lbl_errorsearch.Visible = false;
                btn_save1.Visible = false;
                btn_exit2.Visible = false;
                err.Visible = true;
                err.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_save1_Click(object sender, EventArgs e)
    {
        try
        {

            string name = "";
            string code = "";
            string deptname = "";
            string desgname = "";
            string catagname = "";

            string activerow = "";
            string activecol = "";
            if (Fpstaff.Sheets[0].RowCount != 0)
            {
                activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();
                if (activerow != Convert.ToString(-1))
                {

                    if (txt_searchby.Text == "" || txt_searchby.Text != "")
                    {
                        if (rb_dept.Checked == true)
                        {
                            if (rb_entry.Checked == true)
                            {
                                if (rb_petty.Checked == true)
                                {
                                    name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                                    code = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                                    ViewState["code"] = Convert.ToString(code);
                                    Txt_respnspersorns.Text = name + "-" + Convert.ToString(code);
                                    deptname = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                                    txt_deptname.Text = deptname;
                                    desgname = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                                    txt_desig.Text = desgname;
                                    catagname = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                                    txt_cat.Text = catagname;

                                }
                                else
                                {
                                    name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                                    deptname = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                                    txt_dptbank.Text = deptname;
                                    desgname = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                                    txt_dptdesg.Text = desgname;
                                    catagname = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                                    txt_dptcat.Text = catagname;
                                    code = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                                    ViewState["code"] = Convert.ToString(code);
                                    Txt_respnspersornbank.Text = name + "-" + Convert.ToString(code);
                                }
                            }
                            popwindow.Visible = true;
                        }
                        else if (rb_with.Checked == true)
                        {
                            if (rb_entry.Checked == true)
                            {
                                if (rb_pety.Checked == true)
                                {
                                    name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                                    code = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                                    txtwith.Text = name + "-" + Convert.ToString(code);
                                }
                                else if (rb_banks.Checked == true)
                                {
                                    name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                                    code = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                                    txtbankwith.Text = name + "-" + Convert.ToString(code);
                                }
                            }
                        }
                    }

                    popupsscode1.Visible = false;



                }
                else
                {
                    lbl_errorsearch.Visible = true;
                    lbl_errorsearch.Text = "Please Select Any One Staff";
                }
            }
            else
            {
                lbl_errorsearch1.Visible = true;
                lbl_errorsearch1.Text = "No Records Found";
                Fpstaff.Visible = false;
            }
        }

        catch (Exception ex)
        {
        }
    }
    protected void btn_exit2_Click(object sender, EventArgs e)
    {
        popupsscode1.Visible = false;
        //Petty.Visible = true;

    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getstaffcode(string prefixText)
    {

        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code,staff_name from staffmaster where resign =0 and settled =0 and staff_code like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Accountno(string prefixText)
    {

        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct (AccNo+'-'+AccHolderName) as accname from FM_FinBankMaster where CollegeCode ='" + collegecode1 + "' and AccNo like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;

    }
    protected void btnresponspersornbank_Click(object sender, EventArgs e)
    {
        popupsscode1.Visible = true;
        Fpstaff.Visible = false;
        btn_save1.Visible = false;
        btn_exit2.Visible = false;

    }

    public void Txt_accno_OnTextChanged(object sender, EventArgs e)
    {
        string accountno = Txt_accno.Text.ToString();

        string[] accno = accountno.Split('-');
        if (accno.Length > 0)
        {
            accountno = accno[0].ToString();
        }
        // getaccountnum(accountno);
        string selqry = "select BankPK,BankCode,BankName,AccHolderName,City,AccNo,AccType from FM_FinBankMaster  where AccNo='" + accountno + "' and collegecode='" + collegecode1 + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selqry, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Txt_accno.Text = ds.Tables[0].Rows[i]["AccNo"].ToString();
                Txt_accname.Text = ds.Tables[0].Rows[i]["AccHolderName"].ToString();
                Txt_bankname.Text = ds.Tables[0].Rows[i]["BankName"].ToString();
                Txt_branch.Text = ds.Tables[0].Rows[i]["City"].ToString();
            }
        }
        else
        {
            Txt_accno.Text = "";
            Txt_accname.Text = "";
            Txt_accname.Text = "";
            Txt_bankname.Text = "";
            Txt_branch.Text = "";
        }
    }
    //public void getaccountnum(string accountno)
    //{
    //    string selqry = "select bankname,actname from bank_master1 where accountno='" + accountno + "' and college_code='"+collegecode1+"'";
    //    ds.Clear();
    //    ds = d2.select_method_wo_parameter(selqry, "Text");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {
    //            //Txt_accno.Text = ds.Tables[0].Rows[i]["accountno"].ToString();
    //            Txt_accname.Text = ds.Tables[0].Rows[i]["actname"].ToString();
    //            Txt_bankname.Text = ds.Tables[0].Rows[i]["bankname"].ToString();
    //        }
    //    }

    //}

    protected void txt_AccName_textChanged(object sender, EventArgs e)
    {


    }

    protected void Txtbankname_TextChanged(object sender, EventArgs e)
    {

    }
    protected void bankgrid_pg_DataBound(object sender, EventArgs e)
    {

    }

    protected void ddl_searchby_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_searchby.SelectedValue == "0")
        {
            txt_searchby.Visible = true;
            txt_wardencode.Visible = false;
            txt_wardencode.Text = "";
        }
        else if (ddl_searchby.SelectedValue == "1")
        {
            txt_searchby.Visible = false;
            txt_searchby.Text = "";
            txt_wardencode.Visible = true;
        }
        btn_go2_Click(sender, e);
    }


    public void deposit()
    {
        if (rb_bank.Checked == true)
        {
            Bank.Visible = true;
        }
        else
        {
            Petty.Visible = true;

        }
    }
    protected void rb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        rb_entry.Visible = true;
        rb_report.Visible = true;
        rb_pety.Visible = false;
        rb_banks.Visible = false;
        divwith.Visible = false;
        divpety.Visible = false;
        divbank.Visible = false;
        txtamt.Text = "";
        txtwith.Text = "";
        txtnarr.Text = "";
        headwithbind();
        ledgwithbind();
        bankbind();
        txtbankamt.Text = "";
        // txtbankamtdate.Text = "";
        txtche.Text = "";
        txtbankwith.Text = "";
        txtbanknarr.Text = "";
        divfp.Visible = false;
        FpSpread1.Visible = false;
        txtaval.Text = "";
        txtbankaval.Text = "";
        //rbcuml.Checked = true;
        //rbdetail.Enabled = true;
        //rbcuml_OnCheckedChanged(sender, e);
        ddltype.Enabled = true;
        cbinclude.Enabled = true;
        cbinclude.Checked = false;
        // cbinclude.Visible = false;
        SettingType();

    }
    protected void rb_with_OnCheckedChanged(object sender, EventArgs e)
    {

        rb_entry.Visible = true;
        rb_report.Visible = true;
        divwith.Visible = true;
        rb_pety.Visible = true;
        rb_pety.Checked = true;
        rb_banks.Visible = true;
        rb_banks.Checked = false;
        divpety.Visible = true;
        divbank.Visible = false;
        divfp.Visible = false;
        FpSpread1.Visible = false;
        txtamt.Text = "";
        txtwith.Text = "";
        txtnarr.Text = "";
        headwithbind();
        ledgwithbind();
        bankbind();
        txtbankamt.Text = "";
        //  txtbankamtdate.Text = "";
        txtche.Text = "";
        txtbankwith.Text = "";
        txtbanknarr.Text = "";
        btnDeposit.Visible = false;
        txtaval.Text = "";
        txtbankaval.Text = "";
        petyamount();
        bankamount();
        if (rb_report.Checked == true)
        {
            divpety.Visible = false;
            divbank.Visible = false;
            divwith.Visible = false;
            txtaval.Text = "";
            txtbankaval.Text = "";
        }
        if (rb_entry.Checked == true)
        {
            rb_pety_OnCheckedChanged(sender, e);
        }
        //rbdetail.Enabled = false;
        //cbinclude.Visible = false;
        ddltype.Enabled = false;
        cbinclude.Enabled = false;
        cbinclude.Checked = false;

    }
    public void rb_entry_OnCheckedChanged(object sender, EventArgs e)
    {
        txtaval.Text = "";
        txtwith.Text = "";
        txtnarr.Text = "";
        if (rb_dept.Checked == true)
        {
            btnDeposit.Visible = false;
            divfp.Visible = false;
            FpSpread1.Visible = false;
            lbl_alert1.Visible = false;
            Txt_respnspersorns.Text = "";
            txt_deptname.Text = "";
            txt_desig.Text = "";
            txt_cat.Text = "";
            binddepartment();
            txt_searchby.Text = "";
            Txt_respnspersornbank.Text = "";
            txt_dptbank.Text = "";
            txt_dptdesg.Text = "";
            txt_dptcat.Text = "";
            Txt_accno.Text = "";
            Txt_accname.Text = "";
            Txt_accname.Text = "";
            Txt_bankname.Text = "";
            Txt_branch.Text = "";
            divpety.Visible = false;
            divbank.Visible = false;
            txtaval.Text = "";
            txtbankaval.Text = "";
        }
        else if (rb_with.Checked == true)
        {
            txtaval.Text = "";
            txtwith.Text = "";
            txtnarr.Text = "";
            txtaval.Text = "";
            txtbankaval.Text = "";
            rb_pety.Visible = true;
            rb_pety.Checked = true;
            rb_bank.Visible = true;
            rb_bank.Checked = false;

            if (rb_pety.Checked == true)
            {
                rb_pety.Checked = true;
                rb_bank.Checked = false;
                divwith.Visible = true;
                divpety.Visible = true;
                divbank.Visible = false;
                txtamt.Text = "";
                txtwith.Text = "";
                txtnarr.Text = "";
                headwithbind();
                ledgwithbind();
                bankbind();
                txtbankamt.Text = "";
                //  txtbankamtdate.Text = "";
                txtche.Text = "";
                txtbankwith.Text = "";
                txtbanknarr.Text = "";
                divfp.Visible = false;
                FpSpread1.Visible = false;
                txtaval.Text = "";
                txtbankaval.Text = "";
                petyamount();
            }
            else if (rb_banks.Checked == true)
            {
                rb_bank.Checked = true;
                rb_pety.Checked = false;
                txtaval.Text = "";
                txtwith.Text = "";
                txtnarr.Text = "";
                divwith.Visible = false;
                divpety.Visible = false;
                divbank.Visible = true;
                txtamt.Text = "";
                txtwith.Text = "";
                txtnarr.Text = "";
                headwithbind();
                ledgwithbind();
                bankbind();
                txtbankamt.Text = "";
                // txtbankamtdate.Text = "";
                txtche.Text = "";
                txtbankwith.Text = "";
                txtbanknarr.Text = "";
                divfp.Visible = false;
                FpSpread1.Visible = false;
                txtaval.Text = "";
                txtbankaval.Text = "";
                bankamount();
            }
        }
    }
    public void rb_report_OnCheckedChanged(object sender, EventArgs e)
    {
        txtaval.Text = "";
        txtwith.Text = "";
        txtnarr.Text = "";
        if (rb_dept.Checked == true)
        {
            btnDeposit.Visible = false;
            divfp.Visible = false;
            FpSpread1.Visible = false;
            lbl_alert1.Visible = false;
            Txt_respnspersorns.Text = "";
            txt_deptname.Text = "";
            txt_desig.Text = "";
            txt_cat.Text = "";
            binddepartment();
            txt_searchby.Text = "";
            Txt_respnspersornbank.Text = "";
            txt_dptbank.Text = "";
            txt_dptdesg.Text = "";
            txt_dptcat.Text = "";
            Txt_accno.Text = "";
            Txt_accname.Text = "";
            Txt_bankname.Text = "";
            Txt_branch.Text = "";
            txtaval.Text = "";
            txtbankaval.Text = "";
        }
        else if (rb_with.Checked == true)
        {
            rb_pety.Checked = true;
            divwith.Visible = false;
            divpety.Visible = false;
            divbank.Visible = false;
            btnDeposit.Visible = false;
            txtaval.Text = "";
            txtwith.Text = "";
            txtnarr.Text = "";
            txtaval.Text = "";
            txtbankaval.Text = "";

        }
    }
    protected void rb_pety_OnCheckedChanged(object sender, EventArgs e)
    {
        divpety.Visible = true;
        divbank.Visible = false;
        txtamt.Text = "";
        txtwith.Text = "";
        txtnarr.Text = "";
        txtaval.Text = "";
        headwithbind();
        ledgwithbind();
        txtbankaval.Text = "";
        petyamount();
        if (ddltype.SelectedItem.Value == "2")
        {
            Txtamount.Enabled = false;
        }
        else
        {
            Txtamount.Enabled = true;
        }
    }
    protected void rb_banks_OnCheckedChanged(object sender, EventArgs e)
    {
        divpety.Visible = false;
        divbank.Visible = true;
        bankbind();
        txtbankamt.Text = "";
        txtaval.Text = "";
        //  txtbankamtdate.Text = "";
        txtche.Text = "";
        txtbankwith.Text = "";
        txtbanknarr.Text = "";
        txtbankaval.Text = "";
        bankamount();
        txtvochno.Text = generateReceiptNo();
        if (ddltype.SelectedItem.Value == "2")
        {
            Txt_depositAmount.Enabled = false;
        }
        else
        {
            Txt_depositAmount.Enabled = true;
        }
    }
    protected void ddlhead_SelectedIndexChanged(object sender, EventArgs e)
    {
        ledgwithbind();
        if (rb_with.Checked == true)
        {
            if (rb_pety.Checked == true)
            {
                petyamount();
            }
        }

    }
    protected void ddlledg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtaval.Text = "";
            txtamt.Text = "";
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtwith.Text = "";
            txtnarr.Text = "";
            petyamount();
        }
        catch { }
    }
    public void petyamount()
    {
        try
        {
            string debit = "";
            double totamt = 0;
            string seldt = "";
            string header = ddlhead.SelectedItem.Value.ToString();
            string ledger = ddlledg.SelectedItem.Value.ToString();
            if (header != "" & ledger != "")
            {
                seldt = "select (ISNULL(Debit,0)-ISNULL(credit,0)) as debit  from FT_FinCashContraDet where HeaderFK='" + header + "' and LedgerFK='" + ledger + "' and IsPetty='1'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(seldt, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            debit = Convert.ToString(ds.Tables[0].Rows[i]["debit"]);
                            totamt += Convert.ToDouble(debit);
                        }
                    }
                    if (totamt != null && totamt != 0 && totamt != 0.00)
                    {
                        txtaval.Text = Convert.ToString(totamt);
                        txtaval.Attributes.Add("readonly", "readonly");
                        lblavaler.Visible = false;
                        lblavaler.Text = "";
                    }
                    else
                    {
                        txtaval.Text = "";
                        lblavaler.Text = "There Is No Available Credit Amount";
                        lblavaler.Visible = true;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_with_Click(object sender, EventArgs e)
    {
        popupsscode1.Visible = true;
        lbl_errorsearch.Text = "";
        Fpstaff.Visible = false;
        btn_save1.Visible = false;
        btn_exit2.Visible = false;
    }
    protected void btnsavewith_Click(object sender, EventArgs e)
    {
        try
        {
            string header = "";
            string ledger = "";
            int amount = 0;
            string date = "";
            string wthdrw = "";
            string narr = "";
            string debit = "";
            int save = 0;
            string seldt = "";
            header = ddlhead.SelectedItem.Value.ToString();
            ledger = ddlledg.SelectedItem.Value.ToString();
            amount = Convert.ToInt32(txtamt.Text);
            date = txt_date.Text.ToString();
            string[] splitdate = date.Split('/');
            if (splitdate.Length > 0)
            {
                date = splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString();
            }
            wthdrw = Convert.ToString(txtwith.Text);
            if (wthdrw != "")
            {
                string[] splitstr = wthdrw.Split('-');
                if (splitstr.Length > 0)
                {
                    string codest = splitstr[1].ToString();
                    wthdrw = d2.GetFunction("select appl_id from staffmaster s,staff_appl_master h where  s.appl_no =h.appl_no and s.staff_code='" + codest + "'");
                }
            }
            narr = Convert.ToString(txtnarr.Text);
            string Financialyear = d2.getCurrentFinanceYear(usercode, collegecode1);
            string Currenttime = Convert.ToString(DateTime.Now.ToLongTimeString());


            if (header != "" && ledger != "" && amount != 0 && wthdrw != "")
            {
                string insqry = "if exists (select * from FT_FinCashContraDet where HeaderFK ='" + header + "' and LedgerFK ='" + ledger + "'  and FinYearFK='" + Financialyear + "' and IsPetty='1' ) update FT_FinCashContraDet set TransTime='" + Currenttime + "',Credit=Credit+'" + amount + "',IsHand='0',IsPetty='1',IsBank='0',Narration='" + narr + "',ResponsePerson='" + wthdrw + "' where HeaderFK ='" + header + "' and LedgerFK ='" + ledger + "'  and FinYearFK='" + Financialyear + "'  ";
                int insert = d2.update_method_wo_parameter(insqry, "Text");

                string contrafk = d2.GetFunction("select FinCashContraDetPK from  FT_FinCashContraDet where HeaderFK='" + header + "' and LedgerFK='" + ledger + "'");

                string insertqry = "if exists(select * from FT_FinContraWithDrawDet where TransDate='" + date + "' and HeaderfK='" + header + "' and LedgerFK='" + ledger + "' )update FT_FinContraWithDrawDet set  TransTime='" + Currenttime + "', debit=debit+'" + amount + "',StaffApplNo='" + wthdrw + "',FinCashContraDetFK='" + contrafk + "' where  TransDate='" + date + "' and HeaderfK='" + header + "' and LedgerFK='" + ledger + "'  else insert into FT_FinContraWithDrawDet (TransDate,TransTime,Credit,Debit,StaffApplNo,HeaderfK,LedgerFK,FinCashContraDetFK,Remarks)values('" + date + "','" + Currenttime + "','" + 0 + "','" + amount + "','" + wthdrw + "','" + header + "','" + ledger + "','" + contrafk + "','" + narr + "')";
                int inserq = d2.update_method_wo_parameter(insertqry, "Text");
                string updatqry = "  if exists(select * from FT_FinCashTransaction where TransDate='" + date + "' and FinYearFK='" + Financialyear + "' and ToPetty='1' and EntryUserCode='" + usercode + "')update FT_FinCashTransaction set Debit=Debit+'" + amount + "' where TransDate='" + date + "' and FinYearFK='" + Financialyear + "' and ToPetty='1' and EntryUserCode='" + usercode + "' else insert into FT_FinCashTransaction(TransDate,TransTime,Debit,FinYearFK,ToPetty,EntryUserCode) values('" + date + "','" + Currenttime + "','" + amount + "','" + Financialyear + "','1','" + usercode + "')";
                int update = d2.update_method_wo_parameter(updatqry, "Text");

                save++;
                if (save > 0)
                {
                    cleartext();
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Sucessfully";
                    lbl_alert.Visible = true;
                }


            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Enter The Correct Details";
                lbl_alert.Visible = true;
            }
        }
        catch
        {
        }
    }
    public void cleartext()
    {
        try
        {
            ///////////pety///////////
            txtaval.Text = "";
            txtamt.Text = "";
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtwith.Text = "";
            txtnarr.Text = "";
            headwithbind();
            ledgwithbind();
            //////////////bank/////////////////
            bankbind();
            txtbankaval.Text = "";
            txtbankamt.Text = "";
            txtche.Text = "";
            txtbankamtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtchedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtbankwith.Text = "";
            txtbanknarr.Text = "";
        }
        catch { }
    }
    protected void ddlbank_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtbankaval.Text = "";
            txtbankamt.Text = "";
            txtche.Text = "";
            txtbankamtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtchedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtbankwith.Text = "";
            txtbanknarr.Text = "";
            bankamount();
        }
        catch { }
    }
    public void bankamount()
    {
        try
        {
            double totamt = 0;
            string debit = "";
            string bankfk = ddlbank.SelectedItem.Value.ToString();
            if (bankfk != "")
            {

                string seldt = "select (ISNULL(Debit,0)-ISNULL(credit,0)) as debit  from FT_FinCashContraDet where BankFK='" + bankfk + "' and IsBank='1'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(seldt, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            debit = Convert.ToString(ds.Tables[0].Rows[i]["debit"]);
                            totamt += Convert.ToDouble(debit);
                        }
                    }
                    if (totamt != null && totamt != 0.00 && totamt != 0)
                    {
                        txtbankaval.Text = Convert.ToString(totamt);
                        txtbankaval.Attributes.Add("readonly", "readonly");
                        lblbankaver.Visible = false;
                        lblbankaver.Text = "";
                    }
                    else
                    {
                        lblbankaver.Text = "There Is No Available Credit amount";
                        lblbankaver.Visible = true;
                        txtbankaval.Text = "";
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_save_bank_Click(object sender, EventArgs e)
    {
        try
        {
            string bankfk = "";
            int amount = 0;
            string amtdate = "";
            string cheque = "";
            string cheqdate = "";
            string wthdrw = "";
            string narr = "";
            string debit = "";
            int save = 0;
            bankfk = ddlbank.SelectedItem.Value.ToString();
            amount = Convert.ToInt32(txtbankamt.Text);
            amtdate = txtbankamtdate.Text;
            string[] splitdate = amtdate.Split('/');
            if (splitdate.Length > 0)
            {
                amtdate = splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString();
            }
            cheque = Convert.ToString(txtche.Text);
            cheqdate = txtchedate.Text;
            string[] splitdate1 = cheqdate.Split('/');
            if (splitdate1.Length > 0)
            {
                cheqdate = splitdate1[1].ToString() + "/" + splitdate1[0].ToString() + "/" + splitdate1[2].ToString();
            }
            wthdrw = Convert.ToString(txtbankwith.Text);
            if (wthdrw != "")
            {
                string[] splitstr = wthdrw.Split('-');
                if (splitstr.Length > 0)
                {
                    string codest = splitstr[1].ToString();
                    wthdrw = d2.GetFunction("select appl_id from staffmaster s,staff_appl_master h where  s.appl_no =h.appl_no and s.staff_code='" + codest + "'");
                }
            }
            narr = Convert.ToString(txtbanknarr.Text);
            string voucherno = Convert.ToString(txtvochno.Text);
            string Financialyear = d2.getCurrentFinanceYear(usercode, collegecode1);
            string Currenttime = Convert.ToString(DateTime.Now.ToLongTimeString());
            if (bankfk != "" && amount != 0 && cheque != "" && wthdrw != "")
            {
                string insertqry = "";
                string insqry = "if exists (select * from FT_FinCashContraDet where BankFK='" + bankfk + "'  and FinYearFK='" + Financialyear + "' and IsBank='1' ) update FT_FinCashContraDet set TransTime='" + Currenttime + "',Credit=Credit+'" + amount + "',IsHand='0',IsPetty='0',IsBank='1',Narration='" + narr + "',ResponsePerson='" + wthdrw + "' where  BankFK='" + bankfk + "'  and FinYearFK='" + Financialyear + "' ";
                int insert = d2.update_method_wo_parameter(insqry, "Text");

                string contrafk = d2.GetFunction("select FinCashContraDetPK from  FT_FinCashContraDet where BankFK='" + bankfk + "'");

                string insertquery = "if exists(select * from FT_FinContraWithDrawDet where TransDate='" + amtdate + "'   and BankFK='" + bankfk + "' )update FT_FinContraWithDrawDet set  TransTime='" + Currenttime + "', debit=debit+'" + amount + "',StaffApplNo='" + wthdrw + "',FinCashContraDetFK='" + contrafk + "'where TransDate='" + amtdate + "'   and BankFK='" + bankfk + "'  else insert into FT_FinContraWithDrawDet (TransDate,TransTime,Credit,Debit,StaffApplNo,ChequeNo,ChequeDate,BankFK,FinCashContraDetFK,Remarks)values('" + amtdate + "','" + Currenttime + "','" + 0 + "','" + amount + "','" + wthdrw + "','" + cheque + "','" + cheqdate + "','" + bankfk + "','" + contrafk + "','" + narr + "')";
                int inserq = d2.update_method_wo_parameter(insertquery, "Text");

                string updatqry = "  if exists(select * from FT_FinCashTransaction where TransDate='" + amtdate + "' and FinYearFK='" + Financialyear + "' and ToBank='1' and EntryUserCode='" + usercode + "')update FT_FinCashTransaction set Debit=Debit+'" + amount + "' where TransDate='" + amtdate + "' and FinYearFK='" + Financialyear + "' and ToBank='1' and EntryUserCode='" + usercode + "' else insert into FT_FinCashTransaction(TransDate,TransTime,Debit,FinYearFK,ToBank,EntryUserCode) values('" + amtdate + "','" + Currenttime + "','" + amount + "','" + Financialyear + "','1','" + usercode + "')";
                int update = d2.update_method_wo_parameter(updatqry, "Text");

                insqry = "insert into FT_FinBankTransaction (TransDate,TransTime,BankFK,PayMode,DailyTransID,IsDeposited,IsCleared,IsBounced,Credit,Debit,FinYearFK,EntryUserCode) values ('" + amtdate + "','" + Currenttime + "','" + bankfk + "','2','" + voucherno + "','1','1','0','" + amount + "','0','" + Financialyear + "','" + usercode + "')";
                int upd = d2.update_method_wo_parameter(insqry, "Text");
                save++;
                if (save > 0)
                {
                    string uprec = "update FM_FinCodeSettings set VouchStNo=" + ViewState["receno"] + "+1 where IsHeader=0 and FinYearFK='" + Financialyear + "' and collegecode ='" + ddlclg.SelectedItem.Value + "' and FromDate = (select MAX(FromDate) from FM_FinCodeSettings and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                    int uprecno = d2.update_method_wo_parameter(uprec, "Text");
                    txtvochno.Text = generateReceiptNo();
                    cleartext();
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Sucessfully";
                    lbl_alert.Visible = true;
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Enter The Corresponding Values";
                lbl_alert.Visible = true;
            }
        }
        catch
        {
        }

    }
    protected void btnbankwith_Click(object sender, EventArgs e)
    {
        popupsscode1.Visible = true;
        Fpstaff.Visible = false;
        btn_save1.Visible = false;
        btn_exit2.Visible = false;
    }

    //added by sudhagar 02-06
    #region old
    //protected void rbcuml_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    rb_entry.Visible = true;
    //    rb_report.Visible = true;
    //    rb_pety.Visible = false;
    //    rb_banks.Visible = false;
    //    divwith.Visible = false;
    //    divpety.Visible = false;
    //    divbank.Visible = false;
    //    txtamt.Text = "";
    //    txtwith.Text = "";
    //    txtnarr.Text = "";
    //    headwithbind();
    //    ledgwithbind();
    //    bankbind();
    //    txtbankamt.Text = "";
    //    // txtbankamtdate.Text = "";
    //    txtche.Text = "";
    //    txtbankwith.Text = "";
    //    txtbanknarr.Text = "";
    //    divfp.Visible = false;
    //    FpSpread1.Visible = false;
    //    txtaval.Text = "";
    //    txtbankaval.Text = "";
    //    rbcuml.Checked = true;
    //    btnDeposit.Visible = false;
    //    // rbdetail.Enabled = true;
    //    //rbcuml_Changed(sender, e);
    //    cbinclude.Visible = false;
    //    rb_report.Enabled = true;
    //}
    //protected void rbdetail_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    rb_entry.Visible = true;
    //    rb_report.Visible = true;
    //    rb_pety.Visible = false;
    //    rb_banks.Visible = false;
    //    divwith.Visible = false;
    //    divpety.Visible = false;
    //    divbank.Visible = false;
    //    txtamt.Text = "";
    //    txtwith.Text = "";
    //    txtnarr.Text = "";
    //    headwithbind();
    //    ledgwithbind();
    //    bankbind();
    //    txtbankamt.Text = "";
    //    // txtbankamtdate.Text = "";
    //    txtche.Text = "";
    //    txtbankwith.Text = "";
    //    txtbanknarr.Text = "";
    //    divfp.Visible = false;
    //    FpSpread1.Visible = false;
    //    txtaval.Text = "";
    //    txtbankaval.Text = "";
    //    rbcuml.Checked = true;
    //    btnDeposit.Visible = false;
    //    cbinclude.Visible = true;
    //    rb_report.Enabled = false;
    //}

    #endregion
    //06-07-2016 added by sudhagar

    protected void ddltype_indexChanged(object sender, EventArgs e)
    {
        try
        {
            string type = Convert.ToString(ddltype.SelectedItem.Value);
            if (type == "2")
            {
                cbinclude.Visible = true;
            }
            else
            {
                cbinclude.Visible = false;
            }
        }
        catch { }
    }



    #region print
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
                if (cbinclude.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Deposit Report Name";
                }
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Contra Deposit Report";
            pagename = "Contra.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    #endregion

    #region withdraw voucher no generate

    public string generateReceiptNo()
    {
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string fincyr = d2.getCurrentFinanceYear(usercode, ddlclg.SelectedItem.Value);  
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            // lblaccid.Text = accountid;
            //string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
            string secondreciptqurey = "SELECT VouchStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddlclg.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }

                //string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)");
                string acronymquery = d2.GetFunction("SELECT VouchAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddlclg.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;


                //int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)"));

                int size = Convert.ToInt32(d2.GetFunction("SELECT  VouchSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddlclg.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
                ViewState["receno"] = Convert.ToString(recenoString);
                //lstrcpt.Text = Convert.ToString(receno);
            }

            return recno;
        }
        catch { return recno; }
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
        lbl.Add(lbldept);
        fields.Add(3);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    // last modified 04-10-2016 sudhagar
}
