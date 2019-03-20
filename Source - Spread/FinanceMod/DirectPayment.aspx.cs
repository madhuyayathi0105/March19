using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.Text;
using System.Configuration;



public partial class DirectPayment : System.Web.UI.Page
{
    string collegecode1 = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string collegecodestat = string.Empty;
    string selectQuery = "";
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reUse = new ReuasableMethods();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable hat = new Hashtable();
    DateTime dt;
    int row;
    int i;
    string sql = "";
    int rowcount;
    static string checkvalue = "";
    string name = "";
    string Roll_No = "";
    string stud_name = "";
    string staff_Code = "";
    string staff_name = "";
    string vendor_name = "";
    static string vendor_code = "";
    static string clgcode = "";
    static string vencontcode = "";
    static int personmode = 0;
    static int chosedmode = 0;
    string exapp_no = "";
    static string rightscode = "";
    static byte roll = 0;
    static byte BalanceType = 0;
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
            //  collegecode1 = Session["collegecode"].ToString();
            //   collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            // clgcode = collegecode1;

            if (!IsPostBack)
            {
                setLabelText();
                paymentrights();
                loadcollege();
                // loadledger();
                //loaddebit();
                txt_vdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_cal.Text = DateTime.Now.ToString("dd/MM/yyyy");
                bindGrid();
                bindGrid1();
                bindcollege();
                collegecodestat = Convert.ToString(ddl_collegename.SelectedItem.Value);
                binddepartment();
                popupstud.Visible = false;
                popupstaff.Visible = false;
                Fpsvender.Visible = false;
                divstudtotpay.Visible = true;
                TextBox9.Text = generateReceiptNo();
                txt_vdate.Attributes.Add("readonly", "readonly");
                TextBox9.Attributes.Add("readonly", "readonly");
                txt_lbl_vencurbal.Attributes.Add("readonly", "readonly");
                lbl_er.Text = "";
                vencontcode = "";
                divven.Visible = false;
                chk_insexcess.Checked = false;
                lbl_ventotpayamt.Visible = true;
                txt_ventotpayamt.Visible = true;
                lbl_venpayamt.Visible = true;
                txt_venpayamt.Visible = true;
                lbl_vencurbal.Visible = true;
                txt_lbl_vencurbal.Visible = true;
                loadsetting();
                paymode(sender, e);
                loadpayLedger();
                ddlapyledg_SelectedIndexChanged(sender, e);
                getTabRights(sender, e);//added by sudhagar 10.11.2017
            }
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                collegecodestat = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            rb_percentage_OnSelectedIndexChanged(sender, e);
        }
        catch (Exception ex) { }
    }
    //added by abarna 09.12.2017
    protected void rb_percentage_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        if (rb_percentage.Checked == true)
        {

            txt_dtsamt.Attributes.Add("placeholder", "%");
        }
        if (rb_amount.Checked == true)
        {
            txt_dtsamt.Attributes.Add("placeholder", "Amt");
        }

    }
    //public void loaddebit()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        ddl_debit.Items.Clear();
    //        string ledgername = "select distinct LedgerName,LedgerPK from FM_LedgerMaster WHERE CollegeCode='" + collegecodestat + "'";
    //        ds = d2.select_method_wo_parameter(ledgername, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddl_debit.DataSource = ds;
    //            ddl_debit.DataTextField = "LedgerName";
    //            ddl_debit.DataValueField = "LedgerPK";
    //            ddl_debit.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public void paymentrights()
    {
        try
        {
            string query = "";
            string Master1 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='Payment Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='Payment Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    if (val == "1")
                    {
                        rightscode = "1";
                    }
                    else if (val == "2")
                    {
                        rightscode = "2";
                    }
                    else if (val == "3")
                    {
                        rightscode = "3";
                    }
                    else
                    {
                        rightscode = "0";
                    }
                }
            }
            else
            {
                rightscode = "0";
            }
        }
        catch
        { }
    }
    public void paymode(object sender, EventArgs e)
    {
        try
        {
            if (rightscode == "1")
            {
                rb_cash.Checked = true;
                rb_cash.Enabled = true;
                rb_bank.Enabled = false;
                rb_cash_Change(sender, e);
                //div3.Visible = true;
                div_cash.Visible = true;
                grid1.Visible = true;
                grid1btn.Visible = true;
            }
            if (rightscode == "2")
            {
                rb_cash.Enabled = false;
                rb_bank.Enabled = true;
                rb_cash.Checked = false;
                rb_bank.Checked = true;
                //div3.Visible = true;
                rb_bank_Change(sender, e);
                div_cash.Visible = true;
                grid1.Visible = true;
                grid1btn.Visible = true;
            }
            if (rightscode == "3")
            {
                rb_cash.Checked = true;
                rb_cash.Enabled = true;
                rb_bank.Enabled = true;
                rb_cash_Change(sender, e);
                div_cash.Visible = true;
                //div3.Visible = true;
                grid1.Visible = true;
                grid1btn.Visible = true;
            }
            if (rightscode == "0")
            {
                rb_cash.Checked = true;
                rb_cash.Enabled = true;
                rb_bank.Enabled = true;
                rb_cash_Change(sender, e);
                div_cash.Visible = true;
                //div3.Visible = true;
                grid1.Visible = true;
                grid1btn.Visible = true;
            }
        }
        catch { }
    }
    public void sex()
    {
        try
        {
            ddl_pop2sex.Items.Add(new ListItem("All", "0"));
            ddl_pop2sex.Items.Add(new ListItem("Male", "1"));
            ddl_pop2sex.Items.Add(new ListItem("Female", "2"));
            ddl_pop2sex.Items.Add(new ListItem("Transgender", "3"));
        }
        catch
        { }
    }
    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ddl_collegename.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }

        }
        catch
        {
        }
    }

    public void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_collegename.Items.Count > 0)
        {
            collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            Marksgrid_pg_DataBound(sender, e);
            loadpayLedger();
        }
    }
    public void ddl_pop2collgname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindpop2batchyear();
        loadstream();
        bindpop2degree();
        branch();
        bindsem1();
        BindSectionDetail();
    }

    public void loadstream()
    {
        try
        {
            chkl_str.Items.Clear();
            string college = ddl_pop2collgname.SelectedItem.Value.ToString();
            string selqry = "select distinct type  from Course where college_code ='" + ddl_collegename.SelectedItem.Value + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_str.DataSource = ds;
                chkl_str.DataTextField = "type";
                chkl_str.DataValueField = "type";
                chkl_str.DataBind();
                txt_str.Enabled = true;

            }
            else
            {
                txt_str.Enabled = false;
            }
            for (int i = 0; i < chkl_str.Items.Count; i++)
            {
                chkl_str.Items[i].Selected = true;
            }
            txt_str.Text = "stream(" + chkl_str.Items.Count + ")";
            chk_str.Checked = true;
            bindpop2degree();
        }
        catch
        {

        }
    }
    protected void bindpop2degree()
    {
        try
        {
            ddl_pop2degre.Items.Clear();
            string stream = "";
            for (int i = 0; i < chkl_str.Items.Count; i++)
            {
                if (chkl_str.Items[i].Selected == true)
                {
                    if (stream == "")
                    {
                        stream = chkl_str.Items[i].Value.ToString();
                    }
                    else
                    {
                        stream = stream + "','" + "" + chkl_str.Items[i].Value.ToString() + "";
                    }
                }
            }


            ds.Clear();
            string query = "";
            if (usercode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + ddl_collegename.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
                if (stream != "")
                {
                    query += " and type in ('" + stream + "') ";
                }
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + ddl_collegename.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + " ";
                if (stream != "")
                {
                    query += " and type in ('" + stream + "') ";
                }
            }

            ds = d2.select_method_wo_parameter(query, "Text");
            ddl_pop2degre.DataSource = ds;
            ddl_pop2degre.DataTextField = "course_name";
            ddl_pop2degre.DataValueField = "course_id";
            ddl_pop2degre.DataBind();
            branch();
        }
        catch
        {
        }
    }
    protected void bindpop2batchyear()
    {
        try
        {
            ddl_pop2batchyear.Items.Clear();
            hat.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            ds = d2.select_method(sqlyear, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_pop2batchyear.DataSource = ds;
                ddl_pop2batchyear.DataTextField = "batch_year";
                ddl_pop2batchyear.DataValueField = "batch_year";
                ddl_pop2batchyear.DataBind();
            }
        }
        catch
        {
        }
    }
    public void branch()
    {
        try
        {
            string query1 = "";

            string buildvalue1 = "";
            string build1 = "";
            if (ddl_pop2degre.Items.Count > 0)
            {
                for (int i = 0; i < ddl_pop2degre.Items.Count; i++)
                {
                    build1 = ddl_pop2degre.SelectedValue;
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                }

                string college = ddl_pop2collgname.SelectedItem.Value.ToString();
                query1 = "select distinct degree.degree_code,department.dept_name,degree.Acronym,degree.dept_priority,len(isnull(degree.dept_priority,1000))  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + college + "' and deptprivilages.Degree_code=degree.Degree_code order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc ";
                ds = d2.select_method(query1, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_pop2branch.DataSource = ds;
                    ddl_pop2branch.DataTextField = "dept_name";
                    ddl_pop2branch.DataValueField = "degree_code";
                    ddl_pop2branch.DataBind();
                }
                bindsem1();
            }
            else
            {
                ddl_pop2branch.Items.Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }


    public void bindsem1()
    {
        try
        {
            DataSet ds3 = new DataSet();
            ddlsem1.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string degree_code = ddl_pop2branch.SelectedValue.ToString();
            string batch_year = ddl_pop2batchyear.SelectedValue.ToString();
            string college_code = ddl_pop2collgname.SelectedItem.Value.ToString(); ;

            string sqluery = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code= (" + ddl_pop2branch.SelectedValue + ") and batch_year  = (" + ddl_pop2batchyear.SelectedValue + ") and college_code=" + college_code + " ";
            ds3 = d2.select_method_wo_parameter(sqluery, "text");
            if (ds3.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
                duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["ndurations"]);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem1.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem1.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                ddlsem1.Enabled = false;
            }


        }
        catch
        {
        }
    }
    protected void ddlsem1_selectedchanged(object sender, EventArgs e)
    {
        try
        {
            BindSectionDetail();
        }
        catch
        {
        }
    }
    public void BindSectionDetail()
    {
        try
        {
            cbl_sect.Items.Clear();
            if (ddlsem1.Text != "")
            {
                string branch = ddl_pop2branch.SelectedValue.ToString();
                string batch = ddl_pop2batchyear.SelectedValue.ToString();

                string sqlquery = "select distinct sections from registration where batch_year=" + batch + " and degree_code=" + branch + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";

                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(sqlquery, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sect.DataSource = ds;
                    cbl_sect.DataTextField = "sections";
                    cbl_sect.DataValueField = "sections";
                    cbl_sect.DataBind();
                    //secstatci = ddlsec1.SelectedValue;
                }
                else
                {
                    txt_sect.Enabled = false;
                    cb_sect.Enabled = false;
                    cbl_sect.Enabled = false;
                }
            }
            else
            {
                txt_sect.Enabled = false;
                cb_sect.Enabled = false;
                cbl_sect.Enabled = false;
            }

        }
        catch
        {
        }
    }
    protected void cb_sect_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sect.Text = "--Select--";
            if (cb_sect.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sect.Items.Count; i++)
                {
                    cbl_sect.Items[i].Selected = true;
                }
                txt_sect.Text = "Semester(" + (cbl_sect.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sect.Items.Count; i++)
                {
                    cbl_sect.Items[i].Selected = false;
                }
                txt_sect.Text = "--Select--";
            }

        }


        catch (Exception ex)
        {

        }
    }
    protected void cbl_sect_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_sect.Text = "--Select--";
            cb_sect.Checked = false;

            for (int i = 0; i < cbl_sect.Items.Count; i++)
            {
                if (cbl_sect.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sect.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sect.Items.Count)
                {

                    cb_sect.Checked = true;
                }
                txt_sect.Text = "Section(" + commcount.ToString() + ")";

            }
            //bindhostelname();

        }

        catch (Exception ex)
        {

        }
    }

    public void chk_str_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_str.Checked == true)
            {
                for (int i = 0; i < chkl_str.Items.Count; i++)
                {
                    chkl_str.Items[i].Selected = true;
                }
                txt_str.Text = "Stream(" + (chkl_str.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chkl_str.Items.Count; i++)
                {
                    chkl_str.Items[i].Selected = false;
                }
                txt_str.Text = "---Select---";
            }
            //loadcourse();
            //bindpop2batchyear();
            //branch();
            //bindsem1();
            //BindSectionDetail();

        }
        catch (Exception ex)
        {

        }

    }
    public void chkl_str_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_str.Text = "--Select--";
            chk_str.Checked = false;
            for (int i = 0; i < chkl_str.Items.Count; i++)
            {
                if (chkl_str.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_str.Text = "Stream(" + commcount.ToString() + ")";
                if (commcount == chkl_str.Items.Count)
                {
                    chk_str.Checked = true;
                }
            }

            ddl_pop2degre.Items.Clear();
            string stream = "";
            for (int i = 0; i < chkl_str.Items.Count; i++)
            {
                if (chkl_str.Items[i].Selected == true)
                {
                    if (stream == "")
                    {
                        stream = chkl_str.Items[i].Value.ToString();
                    }
                    else
                    {
                        stream = stream + "','" + "" + chkl_str.Items[i].Value.ToString() + "";
                    }
                }
            }
            //string selqry = "select (c.Course_Name +'-'+ dt.Dept_Name) as Department,degree_code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and type in ('" + stream + "')";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(selqry, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddl_pop2degre.DataSource = ds;
            //    ddl_pop2degre.DataTextField = "Department";
            //    ddl_pop2degre.DataValueField = "degree_code";
            //    ddl_pop2degre.DataBind();
            //}
            bindpop2degree();
            //bindpop2batchyear();
            //branch();
            //bindsem1();
            //BindSectionDetail();

        }
        catch (Exception ex)
        {

        }

    }
    public void loadcourse()
    {

        try
        {
            ddl_pop2degre.Items.Clear();
            string stream = "";
            for (int i = 0; i < chkl_str.Items.Count; i++)
            {
                if (chkl_str.Items[i].Selected == true)
                {
                    if (stream == "")
                    {
                        stream = chkl_str.Items[i].Value.ToString();
                    }
                    else
                    {
                        stream = stream + "','" + "" + chkl_str.Items[i].Value.ToString() + "";
                    }
                }
            }
            string selqry = "select (c.Course_Name +'-'+ dt.Dept_Name) as Department,Degree_Code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and type in ('" + stream + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddl_pop2degre.DataSource = ds;
                ddl_pop2degre.DataTextField = "Department";
                ddl_pop2degre.DataValueField = "Degree_Code";
                ddl_pop2degre.DataBind();
            }
            //for (int j = 0; j < ddl_pop2degre.Items.Count; j++)
            //{
            //    ddl_pop2degre.Items[j].Selected = true;
            //}
            // txt_str.Text = "stream(" + chkl_str.Items.Count + ")";
            // chk_course.Checked = true;
        }
        catch
        {
        }
    }

    //public string generateReceiptNo()
    //{
    //    string recno = string.Empty;

    //    try
    //    {
    //        int receno = 0;
    //        string recacr = string.Empty;
    //        string recnoprev = string.Empty;

    //        string accountid = d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");

    //        string secondreciptqurey = "select receipt,finyear_start  from account_info where acct_id ='" + accountid + "' and (Header_id is null or Header_id ='') order by finyear_start desc";
    //        DataSet dsrecYr = new DataSet();
    //        dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
    //        if (dsrecYr.Tables[0].Rows.Count > 0)
    //        {
    //            recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
    //            if (recnoprev != "")
    //            {
    //                int recno_cur = Convert.ToInt32(recnoprev);
    //                receno = recno_cur + 1;
    //            }

    //            string acronymquery = d2.GetFunction("select rept_acr from Finacode_settings where  college_code =" + collegecode1 + " and user_code =" + usercode + " and (Header_id='' or Header_id is null) order by modifydate desc");
    //            recacr = acronymquery;

    //            recno = recacr + Convert.ToString(receno);
    //            ViewState["receno"] = Convert.ToString(receno);
    //        }

    //        return recno;
    //    }
    //    catch { return recno; }
    //}

    public string generateReceiptNo()
    {
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string fincyr = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code=" + ddl_collegename.SelectedItem.Value + "");
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            // lblaccid.Text = accountid;
            //string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
            string secondreciptqurey = "SELECT VouchStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddl_collegename.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddl_collegename.SelectedItem.Value + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
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
                string acronymquery = d2.GetFunction("SELECT VouchAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddl_collegename.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddl_collegename.SelectedItem.Value + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;


                //int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)"));

                int size = Convert.ToInt32(d2.GetFunction("SELECT  VouchSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddl_collegename.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddl_collegename.SelectedItem.Value + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));

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


    public void loadvencomname()
    {
        chkl_vencomname.Items.Clear();
        string selqry = "select VendorCompName,VendorCode ,VendorPK  from CO_VendorMaster where VendorType =1 ";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selqry, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            chkl_vencomname.DataSource = ds;
            chkl_vencomname.DataTextField = "VendorCompName";
            chkl_vencomname.DataValueField = "VendorCode";
            chkl_vencomname.DataBind();
        }
        for (int i = 0; i < chkl_vencomname.Items.Count; i++)
        {
            chkl_vencomname.Items[i].Selected = true;
        }
        txt_vencomname.Text = "Company Name(" + (chkl_vencomname.Items.Count) + ")";
        chk_vencomname.Checked = true;
    }
    public void chk_vencomname_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_vencomname.Checked == true)
            {
                for (int i = 0; i < chkl_vencomname.Items.Count; i++)
                {
                    chkl_vencomname.Items[i].Selected = true;
                }
                txt_vencomname.Text = "Company Name(" + (chkl_vencomname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chkl_vencomname.Items.Count; i++)
                {
                    chkl_vencomname.Items[i].Selected = false;
                }
                txt_vencomname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void chkl_vencomname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_vencomname.Text = "--Select--";
            chk_vencomname.Checked = false;
            int vencout = 0;
            for (int i = 0; i < chkl_vencomname.Items.Count; i++)
            {
                if (chkl_vencomname.Items[i].Selected == true)
                {
                    vencout = vencout + 1;
                }
            }
            if (vencout > 0)
            {
                txt_vencomname.Text = "Comapany Name(" + vencout.ToString() + ")";
                if (vencout == chkl_vencomname.Items.Count)
                {
                    chk_vencomname.Checked = true;
                }
            }
        }
        catch
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname2(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        // string name = txt_venname.text.tostring();

        string query = "select (VenContactName+'-'+VenContactDesig+'-'+ CONVERT(varchar(10), VendorContactPK)) as contactname from IM_VendorContactMaster where VendorFK ='" + vencontcode + "' and VenContactName <>'' and  VenContactName like '%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["contactname"].ToString());
            }
        }
        return name;
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname3(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select VendorCompName+'-'+VendorCode as vendorcodename ,VendorPK  from CO_VendorMaster where VendorType =1 and VendorCompName like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["vendorcodename"].ToString());
            }
        }
        return name;
    }

    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {

        //if (ddl_type.SelectedValue == "0")
        //{
        //    txt_search.Visible = false;
        //    txt_vendorname2.Visible = true;

        //    txt_vendorname2.Text = "";


        //}

    }
    protected void bindpop1college()
    {
        //try
        //{
        //    ds.Clear();
        //    ddlpop1collegename.Items.Clear();
        //    string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
        //    ds = d2.select_method_wo_parameter(query, "Text");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        ddlpop1collegename.DataSource = ds;
        //        ddlpop1collegename.DataTextField = "collname";
        //        ddlpop1collegename.DataValueField = "college_code";
        //        ddlpop1collegename.DataBind();
        //    }
        //}
        //catch
        //{
        //} {
        try
        {
            ddl_pop2collgname.Items.Clear();
            ds.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddl_pop2collgname.DataSource = ds;
                ddl_pop2collgname.DataTextField = "collname";
                ddl_pop2collgname.DataValueField = "college_code";
                ddl_pop2collgname.DataBind();
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
            ds.Clear();
            ddl_college2.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
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
    //protected void loadledger()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        ddl_credit.Items.Clear();
    //        string ledgername = "select distinct LedgerName,LedgerPK from FM_LedgerMaster WHERE CollegeCode='" + collegecodestat + "'";
    //        ds = d2.select_method_wo_parameter(ledgername, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddl_credit.DataSource = ds;
    //            ddl_credit.DataTextField = "LedgerName";
    //            ddl_credit.DataValueField = "LedgerPK";
    //            ddl_credit.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

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

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }
    protected void btn_add_Click(object sender, EventArgs e)
    {

    }
    public void bindGrid()
    {
        ArrayList addnew = new ArrayList();
        addnew.Add("1");
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
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
    public void bindGrid1()
    {
        ArrayList addnew = new ArrayList();
        addnew.Add("1");
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("Credit ledger");
        dt.Columns.Add("Debit Ledger");
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
            gridView3.DataSource = dt;
            //gridView2.DataSource = dt;
            //gridView2.DataBind();
            gridView3.DataBind();
        }
    }
    public void bindGrid2(object sender, EventArgs e)
    {
        ArrayList addnew = new ArrayList();
        addnew.Add("1");
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("Date");
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
            gridView2.DataSource = dt;
            gridView2.DataBind();
        }
    }
    public void bindgridven()
    {
        ArrayList add = new ArrayList();
        add.Add("1");
        DataTable dtven = new DataTable();
        dtven.Columns.Add("Sno");
        dtven.Columns.Add("Header Name");
        dtven.Columns.Add("Ledger Name");
        dtven.Columns.Add("Total Amount");
        dtven.Columns.Add("Paid amount");
        dtven.Columns.Add("Balance");
        dtven.Columns.Add("TobePaid");
        dtven.Columns.Add("Order Id");
        DataRow drven;
        for (int i = 0; i < add.Count; i++)
        {
            drven = dtven.NewRow();
            drven[0] = add[i].ToString();
            dtven.Rows.Add(drven);
        }
        if (dtven.Rows.Count > 0)
        {
            ViewState["CurrentTable"] = dtven;
            gridven.DataSource = dtven;
            gridven.DataBind();
        }

    }
    protected void rb_cash_Change(object sender, EventArgs e)
    {
        cbledgdet.Checked = false;
        cbledgdet.Visible = true;
        chk_insexcess.Checked = false;
        bank.Visible = false;
        divven.Visible = false;
        btnaddgrid.Visible = true;

        chkinstall.Checked = false;
        lbl_instal.Visible = false;
        TextBox1.Visible = false;
        divbank.Visible = false;

        divstudtotpay.Visible = true;
        instal.Visible = true;
        rb_pety.Checked = true;
        rb_pety.Visible = true;
        //rb_hand.Visible = true;
        rb_pety.Enabled = true;
        rb_hand.Enabled = true;

        lbl_er.Text = "";
        lbl_ventotpayamt.Visible = true;
        txt_ventotpayamt.Visible = true;
        lbl_venpayamt.Visible = true;
        txt_venpayamt.Visible = true;
        lbl_vencurbal.Visible = true;
        txt_lbl_vencurbal.Visible = true;
        if (rb_vendor.Checked == true)
        {
            lbl_ventotpayamt.Visible = false;
            txt_ventotpayamt.Visible = false;
            lbl_venpayamt.Visible = false;
            txt_venpayamt.Visible = false;
            lbl_vencurbal.Visible = false;
            txt_lbl_vencurbal.Visible = false;
            divven.Visible = true;




            divendgrid.Visible = true;
        }
        fldpay.Visible = true;
    }
    protected void rb_bank_Change(object sender, EventArgs e)
    {
        cbledgdet.Checked = false;
        cbledgdet.Visible = false;
        chk_insexcess.Checked = false;
        bank.Visible = true;
        divven.Visible = false;
        bindGrid();
        bindGrid1();
        btnaddgrid.Visible = true;
        rb_pety.Checked = true;
        rb_hand.Checked = false;
        chkinstall.Checked = false;
        lbl_instal.Visible = false;
        TextBox1.Visible = false;
        divbank.Visible = false;
        instal.Visible = true;
        rb_pety.Enabled = false;
        rb_hand.Enabled = false;
        lbl_er.Text = "";
        lbl_ventotpayamt.Visible = true;
        txt_ventotpayamt.Visible = true;
        lbl_venpayamt.Visible = true;
        txt_venpayamt.Visible = true;
        lbl_vencurbal.Visible = true;
        txt_lbl_vencurbal.Visible = true;
        if (rb_vendor.Checked == true)
        {
            lbl_ventotpayamt.Visible = false;
            txt_ventotpayamt.Visible = false;
            lbl_venpayamt.Visible = false;
            txt_venpayamt.Visible = false;
            lbl_vencurbal.Visible = false;
            txt_lbl_vencurbal.Visible = false;
            divven.Visible = true;
            divendgrid.Visible = true;
        }

        fldpay.Visible = false;
    }


    protected void rbonline_Change(object sender, EventArgs e)
    {
        cbStaff.Visible = true;
        cbledgdet.Checked = false;
        cbledgdet.Visible = false;
        chk_insexcess.Checked = false;

        bank.Visible = true;
        divven.Visible = false;
        bindGrid();
        bindGrid1();
        btnaddgrid.Visible = true;
        rb_pety.Checked = true;
        rb_hand.Checked = false;
        chkinstall.Checked = false;
        lbl_instal.Visible = false;
        TextBox1.Visible = false;
        divbank.Visible = false;
        instal.Visible = true;
        rb_pety.Enabled = false;
        rb_hand.Enabled = false;
        lbl_er.Text = "";
        lbl_ventotpayamt.Visible = true;
        txt_ventotpayamt.Visible = true;
        lbl_venpayamt.Visible = true;
        txt_venpayamt.Visible = true;
        lbl_vencurbal.Visible = true;
        txt_lbl_vencurbal.Visible = true;
        if (rb_vendor.Checked == true)
        {
            lbl_ventotpayamt.Visible = false;
            txt_ventotpayamt.Visible = false;
            lbl_venpayamt.Visible = false;
            txt_venpayamt.Visible = false;
            lbl_vencurbal.Visible = false;
            txt_lbl_vencurbal.Visible = false;
            divven.Visible = true;
            divendgrid.Visible = true;
        }

        fldpay.Visible = false;
    }
    #region old
    //protected void rb_stud_Change(object sender, EventArgs e)
    //{
    //    if (rb_cash.Checked == true)
    //    {
    //        if (rb_stud.Checked == true)
    //        {
    //            rb_pety.Enabled = true;
    //            rb_hand.Enabled = true;
    //            chk_insexcess.Checked = false;
    //            stud.Visible = true;
    //            staff.Visible = false;
    //            divvendor.Visible = false;
    //            others.Visible = false;
    //            divstudtotpay.Visible = true;
    //            bank.Visible = false;
    //            div_cash.Visible = true;
    //            grid1btn.Visible = true;
    //            btnaddgrid.Visible = true;
    //            //  txt_lbl_vencurbal.Enabled = false;
    //            instal.Visible = true;
    //            bindGrid();
    //            txt_rollno.Text = "";
    //            txt_name.Text = "";
    //            txt_batch.Text = "";
    //            txt_degr.Text = "";
    //            txt_dept.Text = "";
    //            txt_sem.Text = "";
    //            txt_sec.Text = "";
    //            TextBox1.Text = "";
    //            txt_ventotpayamt.Text = "";
    //            txt_venpayamt.Text = "";
    //            txt_lbl_vencurbal.Text = "";
    //            chkinstall.Checked = false;
    //            lbl_instal.Visible = false;
    //            TextBox1.Visible = false;
    //            divbank.Visible = false;
    //            txt_narr.Text = "";
    //            rb_pety.Checked = true;
    //            rb_hand.Visible = true;
    //            lbl_er.Text = "";
    //            imb_studpht.Visible = false;
    //            txt_stftype.Text = "";
    //            txt_stfcat.Text = "";
    //            txt_stfjn.Text = "";
    //            txt_venname.Text = "";
    //            txt_ccont.Text = "";
    //            txt_cadd.Text = "";
    //            txt_cdesi.Text = "";
    //            txt_othcname.Text = "";
    //            txt_othadd.Text = "";
    //            txt_mblno.Text = "";
    //            TextBox8.Text = "";
    //            divven.Visible = false;
    //            lbl_ventotpayamt.Visible = true;
    //            txt_ventotpayamt.Visible = true;
    //            lbl_venpayamt.Visible = true;
    //            txt_venpayamt.Visible = true;
    //            lbl_vencurbal.Visible = true;
    //            txt_lbl_vencurbal.Visible = true;
    //        }
    //    }
    //    else if (rb_bank.Checked == true)
    //    {
    //        if (rb_stud.Checked == true)
    //        {
    //            chk_insexcess.Checked = false;
    //            bank.Visible = true;
    //            stud.Visible = true;
    //            staff.Visible = false;
    //            divvendor.Visible = false;
    //            others.Visible = false;
    //            divstudtotpay.Visible = true;
    //            grid1btn.Visible = true;
    //            btnaddgrid.Visible = true;
    //            divbank.Visible = true;
    //            div_cash.Visible = true;
    //            rb_pety.Checked = true;
    //            rb_hand.Checked = false;
    //            instal.Visible = true;
    //            bindGrid();
    //            txt_rollno.Text = "";
    //            txt_name.Text = "";
    //            txt_batch.Text = "";
    //            txt_degr.Text = "";
    //            txt_dept.Text = "";
    //            txt_sem.Text = "";
    //            txt_sec.Text = "";
    //            TextBox1.Text = "";
    //            txt_ventotpayamt.Text = "";
    //            txt_venpayamt.Text = "";
    //            txt_lbl_vencurbal.Text = "";
    //            chkinstall.Checked = false;
    //            lbl_instal.Visible = false;
    //            TextBox1.Visible = false;
    //            divbank.Visible = false;
    //            txt_narr.Text = "";
    //            rb_pety.Enabled = false;
    //            rb_hand.Enabled = false;
    //            lbl_er.Text = "";
    //            imb_studpht.Visible = false;
    //            txt_stftype.Text = "";
    //            txt_stfcat.Text = "";
    //            txt_stfjn.Text = "";
    //            txt_venname.Text = "";
    //            txt_ccont.Text = "";
    //            txt_cadd.Text = "";
    //            txt_cdesi.Text = "";

    //            txt_othcname.Text = "";
    //            txt_othadd.Text = "";
    //            txt_mblno.Text = "";
    //            TextBox8.Text = "";
    //            //rb_journel.Visible = false;
    //            //rb_pay.Visible = false;
    //            divven.Visible = false;
    //            lbl_ventotpayamt.Visible = true;
    //            txt_ventotpayamt.Visible = true;
    //            lbl_venpayamt.Visible = true;
    //            txt_venpayamt.Visible = true;
    //            lbl_vencurbal.Visible = true;
    //            txt_lbl_vencurbal.Visible = true;
    //        }
    //    }


    //}
    //protected void rb_staff_Change(object sender, EventArgs e)
    //{
    //    if (rb_cash.Checked == true)
    //    {
    //        if (rb_staff.Checked == true)
    //        {
    //            rb_pety.Enabled = true;
    //            rb_hand.Enabled = true;
    //            chk_insexcess.Checked = false;
    //            stud.Visible = false;
    //            staff.Visible = true;
    //            divvendor.Visible = false;
    //            others.Visible = false;
    //            divstudtotpay.Visible = true;
    //            grid1btn.Visible = true;
    //            btnaddgrid.Visible = true;
    //            // div3.Visible = true;
    //            // div1.Visible = false;
    //            divbank.Visible = true;
    //            //ptyhand.Visible = true;
    //            //  ptyhand1.Visible = false;
    //            instal.Visible = true;
    //            // instal.Visible = true;
    //            // paymentbtn.Visible = true;
    //            bindGrid();
    //            TextBox2.Text = "";
    //            TextBox3.Text = "";
    //            TextBox4.Text = "";
    //            TextBox5.Text = "";
    //            TextBox1.Text = "";
    //            chkinstall.Checked = false;
    //            lbl_instal.Visible = false;
    //            TextBox1.Visible = false;
    //            divbank.Visible = false;
    //            //  rb_petystd.Checked = true;
    //            //  rb_handstd.Checked = false;

    //            txt_othcname.Text = "";
    //            txt_othadd.Text = "";
    //            txt_mblno.Text = "";
    //            TextBox8.Text = "";

    //            txt_acno.Text = "";
    //            txt_acname.Text = "";
    //            txt_bnkname.Text = "";
    //            txt_branch.Text = "";
    //            txt_amt.Text = "";
    //            txt_chqno.Text = "";
    //            TextBox1.Text = "";


    //            txt_ventotpayamt.Text = "";
    //            txt_venpayamt.Text = "";
    //            txt_lbl_vencurbal.Text = "";
    //            txt_narr.Text = "";
    //            rb_pety.Checked = true;
    //            rb_hand.Visible = true;
    //            lbl_er.Text = "";
    //            imb_studpht.Visible = false;
    //            txt_stftype.Text = "";
    //            txt_stfcat.Text = "";
    //            txt_stfjn.Text = "";
    //            txt_venname.Text = "";
    //            txt_ccont.Text = "";
    //            txt_cadd.Text = "";
    //            txt_cdesi.Text = "";
    //            //rb_journel.Visible = false;
    //            //rb_pay.Visible = false;
    //            divven.Visible = false;
    //            lbl_ventotpayamt.Visible = true;
    //            txt_ventotpayamt.Visible = true;
    //            lbl_venpayamt.Visible = true;
    //            txt_venpayamt.Visible = true;
    //            lbl_vencurbal.Visible = true;
    //            txt_lbl_vencurbal.Visible = true;

    //        }
    //    }
    //    else if (rb_bank.Checked == true)
    //    {
    //        if (rb_staff.Checked == true)
    //        {
    //            chk_insexcess.Checked = false;
    //            bank.Visible = true;
    //            stud.Visible = false;
    //            staff.Visible = true;
    //            divvendor.Visible = false;
    //            others.Visible = false;
    //            divstudtotpay.Visible = true;
    //            //  div3.Visible = true;
    //            //  div1.Visible = false;
    //            divbank.Visible = true;
    //            div_cash.Visible = true;
    //            grid1btn.Visible = true;
    //            btnaddgrid.Visible = true;

    //            // ptyhand.Visible = false;
    //            //   ptyhand1.Visible = true;
    //            instal.Visible = true;
    //            // instal.Visible = true;
    //            bindGrid();
    //            // btnaddgrid.Visible = false;
    //            TextBox2.Text = "";
    //            TextBox3.Text = "";
    //            TextBox4.Text = "";
    //            TextBox5.Text = "";

    //            txt_othcname.Text = "";
    //            txt_othadd.Text = "";
    //            txt_mblno.Text = "";
    //            TextBox8.Text = "";
    //            txt_acno.Text = "";
    //            txt_acname.Text = "";
    //            txt_bnkname.Text = "";
    //            txt_branch.Text = "";
    //            txt_amt.Text = "";
    //            txt_chqno.Text = "";
    //            TextBox1.Text = "";


    //            txt_ventotpayamt.Text = "";
    //            txt_venpayamt.Text = "";
    //            txt_lbl_vencurbal.Text = "";
    //            chkinstall.Checked = false;
    //            lbl_instal.Visible = false;
    //            TextBox1.Visible = false;
    //            divbank.Visible = false;
    //            rb_pety.Checked = true;
    //            rb_hand.Checked = false;
    //            txt_narr.Text = "";
    //            rb_pety.Enabled = false;
    //            rb_hand.Enabled = false;
    //            lbl_er.Text = "";
    //            imb_studpht.Visible = false;
    //            txt_stftype.Text = "";
    //            txt_stfcat.Text = "";
    //            txt_stfjn.Text = "";
    //            txt_venname.Text = "";
    //            txt_ccont.Text = "";
    //            txt_cadd.Text = "";
    //            txt_cdesi.Text = "";
    //            //rb_journel.Visible = false;
    //            //rb_pay.Visible = false;
    //            divven.Visible = false;
    //            lbl_ventotpayamt.Visible = true;
    //            txt_ventotpayamt.Visible = true;
    //            lbl_venpayamt.Visible = true;
    //            txt_venpayamt.Visible = true;
    //            lbl_vencurbal.Visible = true;
    //            txt_lbl_vencurbal.Visible = true;

    //        }

    //    }

    //}
    //protected void rb_vendor_Change(object sender, EventArgs e)
    //{
    //    if (rb_cash.Checked == true)
    //    {
    //        if (rb_vendor.Checked == true)
    //        {

    //            rb_pety.Enabled = true;
    //            rb_hand.Enabled = true;
    //            chk_insexcess.Checked = false;
    //            stud.Visible = false;
    //            staff.Visible = false;
    //            divvendor.Visible = true;
    //            others.Visible = false;
    //            divstudtotpay.Visible = true;
    //            divnar.Visible = true;
    //            // div3.Visible = false;
    //            //  div1.Visible = true;
    //            divbank.Visible = true;
    //            div_cash.Visible = false;
    //            grid1btn.Visible = false;
    //            btnaddgrid.Visible = true;
    //            // ptyhand.Visible = true;
    //            //  ptyhand1.Visible = false;
    //            instal.Visible = true;
    //            // instal.Visible = true;
    //            // bindGrid();

    //            //  paymentbtn.Visible = true;
    //            txt_venname.Text = "";
    //            //  txt_vencode.Text = "";
    //            TextBox12.Text = "";
    //            TextBox1.Text = "";
    //            chkinstall.Checked = false;
    //            lbl_instal.Visible = false;
    //            TextBox1.Visible = false;
    //            divbank.Visible = false;
    //            //  rb_petystd.Checked = true;
    //            // rb_handstd.Checked = false;

    //            txt_othcname.Text = "";
    //            txt_othadd.Text = "";
    //            txt_mblno.Text = "";
    //            TextBox8.Text = "";

    //            txt_acno.Text = "";
    //            txt_acname.Text = "";
    //            txt_bnkname.Text = "";
    //            txt_branch.Text = "";
    //            txt_amt.Text = "";
    //            txt_chqno.Text = "";

    //            txt_ventotpayamt.Text = "";
    //            txt_venpayamt.Text = "";
    //            txt_lbl_vencurbal.Text = "";
    //            txt_narr.Text = "";
    //            rb_pety.Checked = true;
    //            rb_hand.Visible = true;
    //            lbl_er.Text = "";
    //            imb_studpht.Visible = false;
    //            txt_stftype.Text = "";
    //            txt_stfcat.Text = "";
    //            txt_stfjn.Text = "";
    //            txt_venname.Text = "";
    //            txt_ccont.Text = "";
    //            txt_cadd.Text = "";
    //            txt_cdesi.Text = "";
    //            //rb_journel.Checked = true;
    //            //rb_journel.Visible = true;
    //            //rb_pay.Visible = true;
    //            seconvdiv.Visible = true;
    //            divven.Visible = true;
    //            bindgridven();
    //            gridven.Visible = true;
    //            lbl_ventotpayamt.Visible = false;
    //            txt_ventotpayamt.Visible = false;
    //            lbl_venpayamt.Visible = false;
    //            txt_venpayamt.Visible = false;
    //            lbl_vencurbal.Visible = false;
    //            txt_lbl_vencurbal.Visible = false;

    //        }
    //    }
    //    else if (rb_bank.Checked == true)
    //    {
    //        if (rb_vendor.Checked == true)
    //        {

    //            chk_insexcess.Checked = false;
    //            bank.Visible = true;
    //            stud.Visible = false;
    //            staff.Visible = false;
    //            divvendor.Visible = true;
    //            others.Visible = false;
    //            divstudtotpay.Visible = true;
    //            divnar.Visible = true;
    //            btnaddgrid.Visible = true;
    //            txt_othcname.Text = "";
    //            txt_othadd.Text = "";
    //            txt_mblno.Text = "";
    //            TextBox8.Text = "";
    //            divbank.Visible = true;
    //            // instal.Visible = true;
    //            // bindGrid();
    //            //   btnaddgrid.Visible = false;
    //            txt_venname.Text = "";
    //            // txt_vencode.Text = "";
    //            TextBox12.Text = "";
    //            TextBox1.Text = "";
    //            chkinstall.Checked = false;
    //            lbl_instal.Visible = false;
    //            TextBox1.Visible = false;
    //            divbank.Visible = false;
    //            rb_pety.Checked = true;
    //            rb_hand.Checked = false;
    //            div_cash.Visible = false;
    //            grid1btn.Visible = false;
    //            instal.Visible = true;
    //            //TextBox8.Text = "";
    //            //PayTextBox9.Text = "";
    //            //TextBox10.Text = "";
    //            //  paymentbtn.Visible = true;
    //            txt_acno.Text = "";
    //            txt_acname.Text = "";
    //            txt_bnkname.Text = "";
    //            txt_branch.Text = "";
    //            txt_amt.Text = "";
    //            txt_chqno.Text = "";


    //            txt_ventotpayamt.Text = "";
    //            txt_venpayamt.Text = "";
    //            txt_lbl_vencurbal.Text = "";
    //            txt_narr.Text = "";
    //            rb_pety.Enabled = false;
    //            rb_hand.Enabled = false;
    //            lbl_er.Text = "";
    //            imb_studpht.Visible = false;
    //            txt_stftype.Text = "";
    //            txt_stfcat.Text = "";
    //            txt_stfjn.Text = "";
    //            txt_venname.Text = "";
    //            txt_ccont.Text = "";
    //            txt_cadd.Text = "";
    //            txt_cdesi.Text = "";
    //            //rb_journel.Checked = true;
    //            //rb_journel.Visible = true;
    //            //rb_pay.Visible = true;
    //            bindgridven();
    //            seconvdiv.Visible = true;
    //            divven.Visible = true;
    //            gridven.Visible = true;
    //            lbl_ventotpayamt.Visible = false;
    //            txt_ventotpayamt.Visible = false;
    //            lbl_venpayamt.Visible = false;
    //            txt_venpayamt.Visible = false;
    //            lbl_vencurbal.Visible = false;
    //            txt_lbl_vencurbal.Visible = false;

    //        }
    //    }

    //}
    //protected void rb_others_Change(object sender, EventArgs e)
    //{
    //    if (rb_cash.Checked == true)
    //    {
    //        if (rb_others.Checked == true)
    //        {
    //            divven.Visible = false;
    //            rb_pety.Enabled = true;
    //            rb_hand.Enabled = true;
    //            chk_insexcess.Checked = false;
    //            stud.Visible = false;
    //            staff.Visible = false;
    //            divvendor.Visible = false;
    //            others.Visible = true;
    //            divstudtotpay.Visible = true;
    //            grid1btn.Visible = true;
    //            btnaddgrid.Visible = true;
    //            // ptyhand.Visible = true;
    //            //   ptyhand1.Visible = false;
    //            instal.Visible = true;
    //            instal.Visible = true;
    //            txt_othcname.Text = "";
    //            txt_othadd.Text = "";
    //            txt_mblno.Text = "";
    //            TextBox8.Text = "";
    //            loaddesc();
    //            TextBox6.Text = "";
    //            TextBox7.Text = "";
    //            //  paymentbtn.Visible = true;
    //            txt_acno.Text = "";
    //            txt_acname.Text = "";
    //            txt_bnkname.Text = "";
    //            txt_branch.Text = "";
    //            txt_amt.Text = "";
    //            txt_chqno.Text = "";
    //            TextBox1.Text = "";
    //            chkinstall.Checked = false;
    //            lbl_instal.Visible = false;
    //            TextBox1.Visible = false;
    //            divbank.Visible = false;
    //            // rb_petystd.Checked = true;
    //            //  rb_handstd.Checked = false;
    //            instal.Visible = true;
    //            div_cash.Visible = true;
    //            txt_narr.Text = "";
    //            rb_pety.Checked = true;
    //            rb_hand.Visible = true;
    //            lbl_er.Text = "";
    //            imb_studpht.Visible = false;
    //            txt_stftype.Text = "";
    //            txt_stfcat.Text = "";
    //            txt_stfjn.Text = "";
    //            txt_venname.Text = "";
    //            txt_ccont.Text = "";
    //            txt_cadd.Text = "";
    //            txt_cdesi.Text = "";
    //            //rb_journel.Visible = false;
    //            //rb_pay.Visible = false;
    //            lbl_ventotpayamt.Visible = true;
    //            txt_ventotpayamt.Visible = true;
    //            lbl_venpayamt.Visible = true;
    //            txt_venpayamt.Visible = true;
    //            lbl_vencurbal.Visible = true;
    //            txt_lbl_vencurbal.Visible = true;
    //        }

    //    }
    //    else if (rb_bank.Checked == true)
    //    {
    //        if (rb_others.Checked == true)
    //        {
    //            divven.Visible = false;
    //            chk_insexcess.Checked = false;
    //            bank.Visible = true;
    //            stud.Visible = false;
    //            staff.Visible = false;
    //            divvendor.Visible = false;
    //            others.Visible = true;
    //            divstudtotpay.Visible = true;
    //            instal.Visible = true;
    //            grid1btn.Visible = true;
    //            btnaddgrid.Visible = true;
    //            loaddesc();
    //            txt_othcname.Text = "";
    //            txt_othadd.Text = "";
    //            txt_mblno.Text = "";
    //            TextBox8.Text = "";
    //            div_cash.Visible = false;
    //            instal.Visible = true;
    //            //  instal.Visible = true;
    //            TextBox6.Text = "";
    //            TextBox7.Text = "";
    //            //  btnaddgrid.Visible = false;
    //            txt_acno.Text = "";
    //            txt_acname.Text = "";
    //            txt_bnkname.Text = "";
    //            txt_branch.Text = "";
    //            txt_amt.Text = "";
    //            txt_chqno.Text = "";
    //            TextBox1.Text = "";
    //            chkinstall.Checked = false;
    //            lbl_instal.Visible = false;
    //            TextBox1.Visible = false;
    //            divbank.Visible = false;
    //            rb_pety.Enabled = false;
    //            rb_hand.Enabled = false;
    //            div_cash.Visible = true;

    //            txt_narr.Text = "";

    //            lbl_er.Text = "";
    //            imb_studpht.Visible = false;
    //            txt_stftype.Text = "";
    //            txt_stfcat.Text = "";
    //            txt_stfjn.Text = "";
    //            txt_venname.Text = "";
    //            txt_ccont.Text = "";
    //            txt_cadd.Text = "";
    //            txt_cdesi.Text = "";
    //            //rb_journel.Visible = false;
    //            //rb_pay.Visible = false;
    //            lbl_ventotpayamt.Visible = true;
    //            txt_ventotpayamt.Visible = true;
    //            lbl_venpayamt.Visible = true;
    //            txt_venpayamt.Visible = true;
    //            lbl_vencurbal.Visible = true;
    //            txt_lbl_vencurbal.Visible = true;
    //        }
    //    }

    //}


    #endregion
    protected void rb_stud_Change(object sender, EventArgs e)
    {
        if (rb_cash.Checked == true)
        {
            if (rb_stud.Checked == true)
            {
                cbStaff.Visible = true;
                rb_pety.Enabled = true;
                rb_hand.Enabled = true;
                chk_insexcess.Checked = false;
                stud.Visible = true;
                staff.Visible = false;
                divvendor.Visible = false;
                others.Visible = false;
                divstudtotpay.Visible = true;
                bank.Visible = false;
                div_cash.Visible = true;
                //Div5.Visible = true;
                grid1btn.Visible = true;
                btnaddgrid.Visible = true;
                divven.Visible = false;

                instal.Visible = true;
                bindGrid();
                bindGrid1();
                txt_rollno.Text = "";
                txt_name.Text = "";
                txt_batch.Text = "";
                txt_degr.Text = "";
                txt_dept.Text = "";
                txt_sem.Text = "";
                txt_sec.Text = "";
                TextBox1.Text = "";
                txt_ventotpayamt.Text = "";
                txt_venpayamt.Text = "";
                txt_lbl_vencurbal.Text = "";
                chkinstall.Checked = false;
                lbl_instal.Visible = false;
                TextBox1.Visible = false;
                divbank.Visible = false;
                txt_narr.Text = "";
                rb_pety.Checked = true;
                //   rb_hand.Visible = true;
                lbl_er.Text = "";
                imb_studpht.Visible = false;
                txt_stftype.Text = "";
                txt_stfcat.Text = "";
                txt_stfjn.Text = "";
                txt_venname.Text = "";
                txt_ccont.Text = "";
                txt_cadd.Text = "";
                txt_cdesi.Text = "";
                txt_othcname.Text = "";
                txt_othadd.Text = "";
                txt_mblno.Text = "";
                TextBox8.Text = "";
                divven.Visible = false;
                lbl_ventotpayamt.Visible = true;
                txt_ventotpayamt.Visible = true;
                lbl_venpayamt.Visible = true;
                txt_venpayamt.Visible = true;
                lbl_vencurbal.Visible = true;
                txt_lbl_vencurbal.Visible = true;

                //txt_rollno.Attributes.Add("readonly", "readonly");
                //txt_name.Attributes.Add("readonly", "readonly");
                txt_batch.Attributes.Add("readonly", "readonly");
                txt_degr.Attributes.Add("readonly", "readonly");
                txt_dept.Attributes.Add("readonly", "readonly");
                txt_sem.Attributes.Add("readonly", "readonly");
                txt_sec.Attributes.Add("readonly", "readonly");
                // txt_narr.Attributes.Add("readonly", "readonly");

            }
        }
        else if (rb_bank.Checked == true)
        {
            if (rb_stud.Checked == true)
            {
                cbStaff.Visible = true;
                chk_insexcess.Checked = false;
                bank.Visible = true;
                stud.Visible = true;
                staff.Visible = false;
                divvendor.Visible = false;
                others.Visible = false;
                divstudtotpay.Visible = true;
                grid1btn.Visible = true;
                btnaddgrid.Visible = true;
                divbank.Visible = true;
                div_cash.Visible = true;
                //Div5.Visible = true;
                // rb_pety.Checked = true;
                //rb_hand.Checked = false;
                instal.Visible = true;
                bindGrid();
                bindGrid1();
                txt_rollno.Text = "";
                txt_name.Text = "";
                txt_batch.Text = "";
                txt_degr.Text = "";
                txt_dept.Text = "";
                txt_sem.Text = "";
                txt_sec.Text = "";
                TextBox1.Text = "";
                txt_ventotpayamt.Text = "";
                txt_venpayamt.Text = "";
                txt_lbl_vencurbal.Text = "";
                chkinstall.Checked = false;
                lbl_instal.Visible = false;
                TextBox1.Visible = false;
                divbank.Visible = false;

                rb_pety.Enabled = false;
                rb_hand.Enabled = false;
                lbl_er.Text = "";
                imb_studpht.Visible = false;
                txt_stftype.Text = "";
                txt_stfcat.Text = "";
                txt_stfjn.Text = "";
                txt_venname.Text = "";
                txt_ccont.Text = "";
                txt_cadd.Text = "";
                txt_cdesi.Text = "";

                txt_othcname.Text = "";
                txt_othadd.Text = "";
                txt_mblno.Text = "";
                TextBox8.Text = "";
                divven.Visible = false;
                lbl_ventotpayamt.Visible = true;
                txt_ventotpayamt.Visible = true;
                lbl_venpayamt.Visible = true;
                txt_venpayamt.Visible = true;
                lbl_vencurbal.Visible = true;
                txt_lbl_vencurbal.Visible = true;
                //txt_rollno.Attributes.Add("readonly", "readonly");
                //txt_name.Attributes.Add("readonly", "readonly");
                txt_batch.Attributes.Add("readonly", "readonly");
                txt_degr.Attributes.Add("readonly", "readonly");
                txt_dept.Attributes.Add("readonly", "readonly");
                txt_sem.Attributes.Add("readonly", "readonly");
                txt_sec.Attributes.Add("readonly", "readonly");
                // txt_narr.Attributes.Add("readonly", "readonly");

                //txt_acno.Attributes.Add("readonly", "readonly");
                txt_acname.Attributes.Add("readonly", "readonly");
                txt_bnkname.Attributes.Add("readonly", "readonly");
                txt_branch.Attributes.Add("readonly", "readonly");
                txt_cal.Attributes.Add("readonly", "readonly");
                //txt_amt.Attributes.Add("readonly", "readonly");
                txtavailAmt.Text = "";
            }
        }

    }
    protected void rb_staff_Change(object sender, EventArgs e)
    {
        if (rb_cash.Checked == true)
        {
            if (rb_staff.Checked == true)
            {
                divven.Visible = false;

                rb_pety.Enabled = true;
                rb_hand.Enabled = true;
                chk_insexcess.Checked = false;
                stud.Visible = false;
                staff.Visible = true;
                divvendor.Visible = false;
                others.Visible = false;
                divstudtotpay.Visible = true;
                div_cash.Visible = true;
                //Div5.Visible = true;
                grid1btn.Visible = true;
                btnaddgrid.Visible = true;
                divbank.Visible = true;
                instal.Visible = true;
                bindGrid();
                bindGrid1();
                TextBox2.Text = "";
                TextBox3.Text = "";
                TextBox4.Text = "";
                TextBox5.Text = "";
                TextBox1.Text = "";
                chkinstall.Checked = false;
                lbl_instal.Visible = false;
                TextBox1.Visible = false;
                divbank.Visible = false;
                txt_othcname.Text = "";
                txt_othadd.Text = "";
                txt_mblno.Text = "";
                TextBox8.Text = "";

                txt_acno.Text = "";
                txt_acname.Text = "";
                txt_bnkname.Text = "";
                txt_branch.Text = "";
                txt_amt.Text = "";
                txt_chqno.Text = "";
                TextBox1.Text = "";


                txt_ventotpayamt.Text = "";
                txt_venpayamt.Text = "";
                txt_lbl_vencurbal.Text = "";
                txt_narr.Text = "";
                rb_pety.Checked = true;
                //  rb_hand.Visible = true;
                lbl_er.Text = "";
                imb_studpht.Visible = false;
                txt_stftype.Text = "";
                txt_stfcat.Text = "";
                txt_stfjn.Text = "";
                txt_venname.Text = "";
                txt_ccont.Text = "";
                txt_cadd.Text = "";
                txt_cdesi.Text = "";
                divven.Visible = false;
                lbl_ventotpayamt.Visible = true;
                txt_ventotpayamt.Visible = true;
                lbl_venpayamt.Visible = true;
                txt_venpayamt.Visible = true;
                lbl_vencurbal.Visible = true;
                txt_lbl_vencurbal.Visible = true;
                //TextBox2.Attributes.Add("readonly", "readonly");
                //TextBox3.Attributes.Add("readonly", "readonly");
                TextBox4.Attributes.Add("readonly", "readonly");
                TextBox5.Attributes.Add("readonly", "readonly");
                txt_stftype.Attributes.Add("readonly", "readonly");
                txt_stfcat.Attributes.Add("readonly", "readonly");
                txt_stfjn.Attributes.Add("readonly", "readonly");
                //txt_narr.Attributes.Add("readonly", "readonly");
                cbStaff.Visible = true;

            }
        }
        else if (rb_bank.Checked == true)
        {
            if (rb_staff.Checked == true)
            {
                chk_insexcess.Checked = false;
                bank.Visible = true;
                stud.Visible = false;
                staff.Visible = true;
                divvendor.Visible = false;
                others.Visible = false;
                divstudtotpay.Visible = true;
                divbank.Visible = true;
                div_cash.Visible = true;
                //Div5.Visible = true;
                grid1btn.Visible = true;
                btnaddgrid.Visible = true;
                instal.Visible = true;
                bindGrid();
                bindGrid1();
                TextBox2.Text = "";
                TextBox3.Text = "";
                TextBox4.Text = "";
                TextBox5.Text = "";

                txt_othcname.Text = "";
                txt_othadd.Text = "";
                txt_mblno.Text = "";
                TextBox8.Text = "";
                txt_acno.Text = "";
                txt_acname.Text = "";
                txt_bnkname.Text = "";
                txt_branch.Text = "";
                txt_amt.Text = "";
                txt_chqno.Text = "";
                TextBox1.Text = "";


                txt_ventotpayamt.Text = "";
                txt_venpayamt.Text = "";
                txt_lbl_vencurbal.Text = "";
                chkinstall.Checked = false;
                lbl_instal.Visible = false;
                TextBox1.Visible = false;
                divbank.Visible = false;
                rb_pety.Checked = true;
                rb_hand.Checked = false;
                txt_narr.Text = "";
                rb_pety.Enabled = false;
                rb_hand.Enabled = false;
                lbl_er.Text = "";
                imb_studpht.Visible = false;
                txt_stftype.Text = "";
                txt_stfcat.Text = "";
                txt_stfjn.Text = "";
                txt_venname.Text = "";
                txt_ccont.Text = "";
                txt_cadd.Text = "";
                txt_cdesi.Text = "";
                //rb_journel.Visible = false;
                //rb_pay.Visible = false;
                divven.Visible = false;
                lbl_ventotpayamt.Visible = true;
                txt_ventotpayamt.Visible = true;
                lbl_venpayamt.Visible = true;
                txt_venpayamt.Visible = true;
                lbl_vencurbal.Visible = true;
                txt_lbl_vencurbal.Visible = true;
                //TextBox2.Attributes.Add("readonly", "readonly");
                //TextBox3.Attributes.Add("readonly", "readonly");
                TextBox4.Attributes.Add("readonly", "readonly");
                TextBox5.Attributes.Add("readonly", "readonly");
                txt_stftype.Attributes.Add("readonly", "readonly");
                txt_stfcat.Attributes.Add("readonly", "readonly");
                txt_stfjn.Attributes.Add("readonly", "readonly");
                // txt_narr.Attributes.Add("readonly", "readonly");

                // txt_acno.Attributes.Add("readonly", "readonly");
                txt_acname.Attributes.Add("readonly", "readonly");
                txt_bnkname.Attributes.Add("readonly", "readonly");
                txt_branch.Attributes.Add("readonly", "readonly");
                txt_cal.Attributes.Add("readonly", "readonly");
                // txt_amt.Attributes.Add("readonly", "readonly");
                txtavailAmt.Text = "";
                cbStaff.Visible = true;
            }

        }

    }
    protected void rb_vendor_Change(object sender, EventArgs e)
    {
        if (rb_cash.Checked == true)
        {
            if (rb_vendor.Checked == true)
            {
                cbStaff.Visible = true;
                rb_pety.Enabled = true;
                // rb_hand.Enabled = true;
                chk_insexcess.Checked = false;
                stud.Visible = false;
                staff.Visible = false;
                divvendor.Visible = true;
                divendgrid.Visible = true;

                divven.Visible = true;
                others.Visible = false;
                divstudtotpay.Visible = true;
                divnar.Visible = true;
                divbank.Visible = true;
                div_cash.Visible = false;
                grid1btn.Visible = false;
                btnaddgrid.Visible = true;
                instal.Visible = true;
                txt_venname.Text = "";
                TextBox12.Text = "";
                TextBox1.Text = "";
                chkinstall.Checked = false;
                lbl_instal.Visible = false;
                TextBox1.Visible = false;
                divbank.Visible = false;
                txt_othcname.Text = "";
                txt_othadd.Text = "";
                txt_mblno.Text = "";
                TextBox8.Text = "";

                txt_acno.Text = "";
                txt_acname.Text = "";
                txt_bnkname.Text = "";
                txt_branch.Text = "";
                txt_amt.Text = "";
                txt_chqno.Text = "";

                txt_ventotpayamt.Text = "";
                txt_venpayamt.Text = "";
                txt_lbl_vencurbal.Text = "";
                txt_narr.Text = "";
                rb_pety.Checked = true;
                //  rb_hand.Visible = true;
                lbl_er.Text = "";
                imb_studpht.Visible = false;
                txt_stftype.Text = "";
                txt_stfcat.Text = "";
                txt_stfjn.Text = "";
                txt_venname.Text = "";
                txt_ccont.Text = "";
                txt_cadd.Text = "";
                txt_cdesi.Text = "";
                //rb_journel.Checked = true;
                //rb_journel.Visible = true;
                //rb_pay.Visible = true;
                seconvdiv.Visible = true;
                divven.Visible = true;
                bindgridven();
                gridven.Visible = true;
                lbl_ventotpayamt.Visible = false;
                txt_ventotpayamt.Visible = false;
                lbl_venpayamt.Visible = false;
                txt_venpayamt.Visible = false;
                lbl_vencurbal.Visible = false;
                txt_lbl_vencurbal.Visible = false;
                // txt_venname.Attributes.Add("readonly", "readonly");
                //  txt_ccont.Attributes.Add("readonly", "readonly");
                txt_cdesi.Attributes.Add("readonly", "readonly");
                txt_cadd.Attributes.Add("readonly", "readonly");
                //TextBox12.Attributes.Add("readonly", "readonly");              

            }
        }
        else if (rb_bank.Checked == true)
        {
            if (rb_vendor.Checked == true)
            {
                cbStaff.Visible = true;
                chk_insexcess.Checked = false;
                bank.Visible = true;
                stud.Visible = false;
                staff.Visible = false;
                divvendor.Visible = true;
                others.Visible = false;
                divstudtotpay.Visible = true;
                divnar.Visible = true;
                btnaddgrid.Visible = true;
                txt_othcname.Text = "";
                txt_othadd.Text = "";
                txt_mblno.Text = "";
                TextBox8.Text = "";
                divbank.Visible = true;
                // instal.Visible = true;
                // bindGrid();
                //   btnaddgrid.Visible = false;
                txt_venname.Text = "";
                // txt_vencode.Text = "";
                TextBox12.Text = "";
                TextBox1.Text = "";
                chkinstall.Checked = false;
                lbl_instal.Visible = false;
                TextBox1.Visible = false;
                divbank.Visible = false;
                rb_pety.Checked = true;
                rb_hand.Checked = false;
                div_cash.Visible = false;
                grid1btn.Visible = false;
                instal.Visible = true;
                //TextBox8.Text = "";
                //PayTextBox9.Text = "";
                //TextBox10.Text = "";
                //  paymentbtn.Visible = true;
                txt_acno.Text = "";
                txt_acname.Text = "";
                txt_bnkname.Text = "";
                txt_branch.Text = "";
                txt_amt.Text = "";
                txt_chqno.Text = "";


                txt_ventotpayamt.Text = "";
                txt_venpayamt.Text = "";
                txt_lbl_vencurbal.Text = "";
                txt_narr.Text = "";
                rb_pety.Enabled = false;
                rb_hand.Enabled = false;
                lbl_er.Text = "";
                imb_studpht.Visible = false;
                txt_stftype.Text = "";
                txt_stfcat.Text = "";
                txt_stfjn.Text = "";
                txt_venname.Text = "";
                txt_ccont.Text = "";
                txt_cadd.Text = "";
                txt_cdesi.Text = "";
                //rb_journel.Checked = true;
                //rb_journel.Visible = true;
                //rb_pay.Visible = true;
                bindgridven();
                seconvdiv.Visible = true;
                divven.Visible = true;
                gridven.Visible = true;
                lbl_ventotpayamt.Visible = false;
                txt_ventotpayamt.Visible = false;
                lbl_venpayamt.Visible = false;
                txt_venpayamt.Visible = false;
                lbl_vencurbal.Visible = false;
                txt_lbl_vencurbal.Visible = false;
                //  txt_venname.Attributes.Add("readonly", "readonly");
                //  txt_ccont.Attributes.Add("readonly", "readonly");
                txt_cdesi.Attributes.Add("readonly", "readonly");
                txt_cadd.Attributes.Add("readonly", "readonly");
                //TextBox12.Attributes.Add("readonly", "readonly");
                //txt_narr.Attributes.Add("readonly", "readonly");
                //  txt_acno.Attributes.Add("readonly", "readonly");
                txt_acname.Attributes.Add("readonly", "readonly");
                txt_bnkname.Attributes.Add("readonly", "readonly");
                txt_branch.Attributes.Add("readonly", "readonly");
                txt_cal.Attributes.Add("readonly", "readonly");
                //  txt_amt.Attributes.Add("readonly", "readonly");
                txtavailAmt.Text = "";
            }
        }

    }
    protected void rb_others_Change(object sender, EventArgs e)
    {
        if (rb_cash.Checked == true)
        {
            if (rb_others.Checked == true)
            {
                divendgrid.Visible = false;

                divven.Visible = false;
                cbStaff.Visible = true;
                gridven.Visible = false;
                rb_pety.Enabled = true;
                rb_hand.Enabled = true;
                chk_insexcess.Checked = false;
                stud.Visible = false;
                staff.Visible = false;
                divvendor.Visible = false;
                others.Visible = true;
                divstudtotpay.Visible = true;
                grid1btn.Visible = true;
                btnaddgrid.Visible = true;
                // ptyhand.Visible = true;
                //   ptyhand1.Visible = false;
                instal.Visible = true;
                instal.Visible = true;
                txt_othcname.Text = "";
                txt_othadd.Text = "";
                txt_mblno.Text = "";
                TextBox8.Text = "";

                TextBox6.Text = "";
                TextBox7.Text = "";
                //  paymentbtn.Visible = true;
                txt_acno.Text = "";
                txt_acname.Text = "";
                txt_bnkname.Text = "";
                txt_branch.Text = "";
                // txt_amt.Text = "";
                txt_chqno.Text = "";
                TextBox1.Text = "";
                chkinstall.Checked = false;
                lbl_instal.Visible = false;
                TextBox1.Visible = false;
                divbank.Visible = false;
                // rb_petystd.Checked = true;
                //  rb_handstd.Checked = false;
                instal.Visible = true;
                div_cash.Visible = true;
                //Div5.Visible = true;
                txt_narr.Text = "";
                rb_pety.Checked = true;
                //  rb_hand.Visible = true;
                lbl_er.Text = "";
                imb_studpht.Visible = false;
                txt_stftype.Text = "";
                txt_stfcat.Text = "";
                txt_stfjn.Text = "";
                txt_venname.Text = "";
                txt_ccont.Text = "";
                txt_cadd.Text = "";
                txt_cdesi.Text = "";
                //rb_journel.Visible = false;
                //rb_pay.Visible = false;
                lbl_ventotpayamt.Visible = true;
                txt_ventotpayamt.Visible = true;
                lbl_venpayamt.Visible = true;
                txt_venpayamt.Visible = true;
                lbl_vencurbal.Visible = true;
                txt_lbl_vencurbal.Visible = true;
                loaddesc();
                //TextBox6.Attributes.Add("readonly", "readonly");
                //txt_othcname.Attributes.Add("readonly", "readonly");
                //txt_othadd.Attributes.Add("readonly", "readonly");
                //txt_mblno.Attributes.Add("readonly", "readonly");
                //TextBox8.Attributes.Add("readonly", "readonly");
                //txt_narr.Attributes.Add("readonly", "readonly");
                txt_ventotpayamt.Text = "";
                txt_venpayamt.Text = "";
                txt_lbl_vencurbal.Text = "";
                divven.Visible = false;
            }

        }
        else if (rb_bank.Checked == true)
        {
            if (rb_others.Checked == true)
            {
                divendgrid.Visible = false;

                divven.Visible = false;
                cbStaff.Visible = true;
                divven.Visible = false;
                chk_insexcess.Checked = false;
                bank.Visible = true;
                stud.Visible = false;
                staff.Visible = false;
                divvendor.Visible = false;
                others.Visible = true;
                divstudtotpay.Visible = true;
                instal.Visible = true;
                grid1btn.Visible = true;
                btnaddgrid.Visible = true;

                txt_othcname.Text = "";
                txt_othadd.Text = "";
                txt_mblno.Text = "";
                TextBox8.Text = "";
                div_cash.Visible = false;
                instal.Visible = true;
                //  instal.Visible = true;
                TextBox6.Text = "";
                TextBox7.Text = "";
                //  btnaddgrid.Visible = false;
                txt_acno.Text = "";
                txt_acname.Text = "";
                txt_bnkname.Text = "";
                txt_branch.Text = "";
                txt_amt.Text = "";
                txt_chqno.Text = "";
                TextBox1.Text = "";
                chkinstall.Checked = false;
                lbl_instal.Visible = false;
                TextBox1.Visible = false;
                divbank.Visible = false;
                rb_pety.Enabled = false;
                rb_hand.Enabled = false;
                div_cash.Visible = true;
                //Div5.Visible = true;
                txt_narr.Text = "";

                lbl_er.Text = "";
                imb_studpht.Visible = false;
                txt_stftype.Text = "";
                txt_stfcat.Text = "";
                txt_stfjn.Text = "";
                txt_venname.Text = "";
                txt_ccont.Text = "";
                txt_cadd.Text = "";
                txt_cdesi.Text = "";
                //rb_journel.Visible = false;
                //rb_pay.Visible = false;
                lbl_ventotpayamt.Visible = true;
                txt_ventotpayamt.Visible = true;
                lbl_venpayamt.Visible = true;
                txt_venpayamt.Visible = true;
                lbl_vencurbal.Visible = true;
                txt_lbl_vencurbal.Visible = true;
                //TextBox6.Attributes.Add("readonly", "readonly");
                //txt_othcname.Attributes.Add("readonly", "readonly");
                //txt_othadd.Attributes.Add("readonly", "readonly");
                //txt_mblno.Attributes.Add("readonly", "readonly");
                //TextBox8.Attributes.Add("readonly", "readonly");
                //txt_narr.Attributes.Add("readonly", "readonly");
                // txt_acno.Attributes.Add("readonly", "readonly");
                txt_acname.Attributes.Add("readonly", "readonly");
                txt_bnkname.Attributes.Add("readonly", "readonly");
                txt_branch.Attributes.Add("readonly", "readonly");
                txt_cal.Attributes.Add("readonly", "readonly");
                //txt_amt.Attributes.Add("readonly", "readonly");
                loaddesc();
                txt_ventotpayamt.Text = "";
                txt_venpayamt.Text = "";
                txt_lbl_vencurbal.Text = "";
                txtavailAmt.Text = "";
            }
        }

    }
    protected void imagebtnpop2close_Click(object sender, EventArgs e)
    {
        popupstud.Visible = false;

    }
    public void btn_popupstud_Click(object sender, EventArgs e)
    {
        loadroll();
    }
    public void loadroll()
    {
        try
        {
            RollAndRegSettings();
            string stream = "";
            string sema = "";
            string sect = "";
            string batch = ddl_pop2batchyear.SelectedItem.Value.ToString();
            for (int i = 0; i < chkl_str.Items.Count; i++)
            {
                if (chkl_str.Items[i].Selected == true)
                {
                    if (stream == "")
                    {
                        stream = chkl_str.Items[i].Value.ToString();
                    }
                    else
                    {
                        stream = stream + "','" + "" + chkl_str.Items[i].Value.ToString() + "";
                    }
                }
            }
            string degree = ddl_pop2degre.SelectedItem.Value.ToString();
            string branch = ddl_pop2branch.SelectedItem.Value.ToString();
            if (ddlsem1.Enabled == true)
            {
                if (ddlsem1.SelectedItem.Selected == true)
                {
                    sema = ddlsem1.SelectedItem.Value.ToString();
                }
                else
                {
                    sema = "";
                }
            }
            else
            {
                sema = "";
            }

            for (int i = 0; i < cbl_sect.Items.Count; i++)
            {
                if (cbl_sect.Items[i].Selected == true)
                {
                    if (sect == "")
                    {
                        sect = cbl_sect.Items[i].Value.ToString();
                    }
                    else
                    {
                        sect = sect + "','" + "" + cbl_sect.Items[i].Value.ToString() + "";
                    }
                }
            }
            string strsex = ddl_pop2sex.SelectedItem.Value.ToString();
            string studtype = ddl_pop2studenttype.SelectedItem.Text.ToString();

            string selqry = "";

            selqry = " select r.app_no,r.Roll_No,r.Reg_No,r.roll_admit,r.Stud_Name,a.app_formno,r.batch_year,r.Current_Semester,r.sections,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree from applyn a,Registration r,Degree d,Department dt,Course c where a.app_no =r.App_No and  r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and CC=0 and DelFlag=0 and Exam_Flag<>'debar'  and d.Degree_Code in ('" + branch + "') and r.Batch_Year='" + ddl_pop2batchyear.SelectedItem.Text + "'  ";
            if (chk_insexcess.Checked == true)
            {
                selqry = selqry + " and r.Current_Semester='" + sema + "'";
            }
            if (sect != "")
            {
                selqry = selqry + " and r.sections in ('" + sect + "')";
            }
            if ((ddl_pop2sex.SelectedItem.Text != "All"))
            {
                selqry = selqry + " and a.sex ='" + strsex + "'";
            }
            if ((ddl_pop2studenttype.SelectedItem.Text != "Both"))
            {
                selqry = selqry + " and r.Stud_Type ='" + studtype + "'";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                fproll.Sheets[0].RowHeader.Visible = false;
                fproll.CommandBar.Visible = false;
                fproll.Sheets[0].RowCount = 0;
                fproll.SheetCorner.ColumnCount = 0;
                fproll.Sheets[0].ColumnHeader.RowCount = 1;

                fproll.Sheets[0].AutoPostBack = true;
                fproll.Sheets[0].ColumnCount = 8;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                fproll.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                fproll.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                fproll.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                fproll.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No ";
                fproll.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No ";
                fproll.Sheets[0].ColumnHeader.Cells[0, 4].Text = "App No ";
                fproll.Sheets[0].Columns[4].Visible = false;
                fproll.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Stud Name";
                fproll.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Degree Code";
                fproll.Sheets[0].Columns[6].Visible = false;
                fproll.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Degree ";
                for (int jj = 0; jj < fproll.Sheets[0].Columns.Count; jj++)
                {
                    fproll.Sheets[0].Columns[jj].HorizontalAlign = HorizontalAlign.Center;
                    fproll.Sheets[0].ColumnHeader.Columns[jj].HorizontalAlign = HorizontalAlign.Center;
                    fproll.Sheets[0].ColumnHeader.Columns[jj].Font.Bold = true;
                    fproll.Sheets[0].ColumnHeader.Columns[jj].Font.Name = "Book Antiqua";
                    fproll.Sheets[0].ColumnHeader.Columns[jj].Font.Size = FontUnit.Medium;
                    fproll.Sheets[0].Columns[jj].Font.Bold = true;
                    fproll.Sheets[0].Columns[jj].Font.Name = "Book Antiqua";
                    fproll.Sheets[0].Columns[jj].Font.Size = FontUnit.Medium;
                }
                fproll.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                fproll.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                {
                    fproll.Sheets[0].Columns[1].Visible = false;
                    fproll.Sheets[0].Columns[2].Visible = false;
                    fproll.Sheets[0].Columns[3].Visible = false;
                    fproll.Sheets[0].Columns[4].Visible = true;
                    fproll.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                }
                spreadColumnVisible();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    fproll.Sheets[0].RowCount++;
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"].ToString());
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"].ToString());
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"].ToString());
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["roll_admit"].ToString());
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["app_formno"].ToString());
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"].ToString());
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["sections"].ToString());
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"].ToString());
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["batch_year"]);
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["Degree"].ToString());
                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"].ToString());

                }
                fproll.Sheets[0].ColumnHeader.Columns[0].Width = 56;
                fproll.Sheets[0].ColumnHeader.Columns[1].Width = 112;
                fproll.Sheets[0].ColumnHeader.Columns[2].Width = 169;
                fproll.Sheets[0].ColumnHeader.Columns[3].Width = 134;
                fproll.Sheets[0].ColumnHeader.Columns[4].Width = 200;
                fproll.Sheets[0].ColumnHeader.Columns[5].Width = 180;
                fproll.Width = 670;


                for (int i = 0; i < fproll.Sheets[0].Columns.Count; i++)
                {
                    fproll.Sheets[0].Columns[i].Locked = true;
                }
                fproll.Sheets[0].PageSize = fproll.Sheets[0].RowCount;
                fproll.SaveChanges();
                fproll.Visible = true;
                btn_popupstud_save.Visible = true;
                btn_popupstud_exit.Visible = true;
                div2.Visible = true;
                lbl_error.Visible = false;

            }
            else
            {
                fproll.Visible = false;
                div2.Visible = false;
                btn_popupstud_save.Visible = false;
                btn_popupstud_exit.Visible = false;
                lbl_alert.Text = "No Record Found";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
            }

        }

        catch
        {

        }


    }

    public void ddl_pop2degre_SelectedIndexChanged(object sender, EventArgs e)
    {
        branch();
        bindsem1();

    }
    public void ddl_pop2branch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem1();
    }
    protected void ddlcreditledger_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public void btn_popupstud_save_Click(object sender, EventArgs e)
    {
        try
        {
            string roll = "";
            string batch = "";
            string deg = "";
            string dept = "";
            string sem = "";
            string sec = "";
            string activerow = "";
            string activecol = "";
            string app_no = "";
            string photo = "";
            if (fproll.Sheets[0].RowCount != 0)
            {
                activerow = fproll.ActiveSheetView.ActiveRow.ToString();
                activecol = fproll.ActiveSheetView.ActiveColumn.ToString();
                if (activerow != Convert.ToString(-1))
                {
                    if (txt_searchby.Text == "" || txt_searchby.Text != "")
                    {
                        photo = fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                        {
                            roll = fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                        }
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        {
                            roll = fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                        }
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        {
                            roll = fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                        }
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                        {
                            roll = fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                        }
                        txt_rollno.Text = roll;
                        name = fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                        txt_name.Text = name;
                        batch = Convert.ToString(fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                        txt_batch.Text = batch;
                        string strdeg = fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text;
                        string deg1 = "";
                        string dept1 = "";
                        string[] strdegsplit = strdeg.Split('-');
                        if (strdegsplit.Length == 2)
                        {
                            deg1 = strdegsplit[0].ToString();
                            dept1 = strdegsplit[1].ToString();
                        }

                        txt_degr.Text = deg1;
                        // dept = fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                        txt_dept.Text = dept1;
                        sem = Convert.ToString(fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);
                        txt_sem.Text = sem;
                        sec = Convert.ToString(fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                        txt_sec.Text = sec;
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                        {
                            imb_studpht.ImageUrl = "~/Handler4.ashx?rollno=" + photo;
                            imb_studpht.Visible = true;
                        }
                        else
                        {
                            imb_studpht.Visible = false;
                        }

                        app_no = Convert.ToString(fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                        payamount(app_no);
                        double paymentvalue = 0;
                        paymentvalue = paymentvalue + payamount(app_no);
                        if (paymentvalue != 0)
                        {
                            lbl_er.Text = "";
                            txt_ventotpayamt.Text = Convert.ToString(paymentvalue);
                            txt_venpayamt.Text = Convert.ToString(paymentvalue);
                            txt_amt.Text = Convert.ToString(paymentvalue);
                            lbl_er.Text = "";
                            lbl_er.Visible = false;
                        }
                        else
                        {
                            txt_ventotpayamt.Text = "";
                            txt_lbl_vencurbal.Text = "";
                            lbl_er.Text = "There Is No Payable Amount";
                            lbl_er.Visible = true;
                        }


                    }
                    popupstud.Visible = false;
                }
                else
                {
                    lbl_alert.Text = "Please Select Any One Staff";
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;

                }

            }
            else
            {
                lbl_alert.Text = "No Record found  Successfully";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                Fpstaff.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public double payamount(string exapp_no)
    {
        string selqrypay = "SELECT SUM(ExcessAmt -AdjAmt) as payamount FROM FT_ExcessDet WHERE App_No = '" + exapp_no + "'";
        if (chk_insexcess.Checked == true)
        {
            selqrypay = selqrypay + " and ExcessType in ('1','2')";
        }
        else
        {
            selqrypay = selqrypay + " and ExcessType in ('2')";
        }
        ds.Clear();
        ds = d2.select_method_wo_parameter(selqrypay, "Text");
        string payamt = Convert.ToString(ds.Tables[0].Rows[0]["payamount"]);
        double totalvalue = 0;
        if (payamt != null && payamt != "")
        {
            totalvalue = Convert.ToDouble(payamt);
        }
        return totalvalue;

    }
    public void btn_popupstud_exit_Click(object sender, EventArgs e)
    {
        popupstud.Visible = false;
    }
    public void btn_stud_Click(object sender, EventArgs e)
    {
        bindpop1college();
        bindpop2batchyear();
        loadstream();
        bindpop2degree();
        branch();
        bindsem1();
        BindSectionDetail();
        sex();
        popupstud.Visible = true; ;
        fproll.Visible = false;
        div2.Visible = false;
        btn_popupstud_save.Visible = false;
        btn_popupstud_exit.Visible = false;
        txt_rollno.Text = "";
        txt_name.Text = "";
        txt_batch.Text = "";
        txt_degr.Text = "";
        txt_dept.Text = "";
        txt_sem.Text = "";
        txt_sec.Text = "";
        imb_studpht.ImageUrl = "";
        imb_studpht.Visible = false;
    }

    public void ddl_pop1hostelname_SelectedIndexchange(object sender, EventArgs e)
    {

    }
    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static List<string> Getrno(string prefixText)
    //{
    //    WebService ws = new WebService();
    //    List<string> name = new List<string>();
    //    string query = "select top (10)Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
    //    name = ws.Getname(query);
    //    return name;
    //}
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
                    query = "select top 100 Roll_No from Registration where   Roll_No like '" + prefixText + "%' and college_code=" + collegecodestat + " order by Roll_No asc  ";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration where   Reg_No like '" + prefixText + "%' and college_code=" + collegecodestat + " order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where   Roll_admit like '" + prefixText + "%' and college_code=" + collegecodestat + " order by Roll_admit asc";
                }
                else
                {
                    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1'  and app_formno like '" + prefixText + "%' and college_code=" + collegecodestat + " order by app_formno asc";
                }
            }
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";

        // studhash = ws.Getnamevalue(query);

        name = ws.Getname(query);
        return name;
    }


    public void btn_staff_Click(object sender, EventArgs e)
    {

        popupstaff.Visible = true;
        Fpstaff.Visible = false;
        btn_popupstaff_save.Visible = false;
        btn_popupstaff_exit.Visible = false;

    }
    public void btn_vendor_Click(object sender, EventArgs e)
    {
        //  bindvendorname();
        loadvencomname();
        popupvender.Visible = true;
        divFpsvender.Visible = false;
        Fpsvender.Visible = false;
        btn_vendor_save.Visible = false;
        btn_vendor_exit.Visible = false;
        txt_venname.Text = "";
        txt_ccont.Text = "";
        txt_cadd.Text = "";
        txt_cdesi.Text = "";

    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaff(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct top(50) s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name+'-'+ s.staff_code, s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";

        name = ws.Getname(query);

        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstaffcode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct top (50) s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code like '" + prefixText + "%'";
        // string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";


        name = ws.Getname(query);

        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select  staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    protected void imagebtnpopsscode_Click(object sender, EventArgs e)
    {
        popupstaff.Visible = false;
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
        //  btn_popupstaff_Click(sender, e);
    }

    protected void btn_popupstaff_Click(object sender, EventArgs e)
    {
        try
        {
            int rolcount = 0;
            int sno = 0;
            if (txt_searchby.Text != "")
            {
                if (ddl_searchby.SelectedIndex == 0)
                {
                    sql = "select distinct s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.Staff_name ='" + Convert.ToString(txt_searchby.Text) + "' order by s.staff_code";
                }

            }
            else if (txt_wardencode.Text != "")
            {
                if (ddl_searchby.SelectedIndex == 1)
                {
                    sql = "select distinct s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_code ='" + Convert.ToString(txt_wardencode.Text) + "' order by s.staff_code";
                }
            }
            else
            {
                sql = "select distinct s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and h.dept_code in ('" + ddl_department3.SelectedItem.Value + "') order by s.staff_code";
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
                    //Fpstaff.Sheets[0].Rows[Fpstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                }
                Fpstaff.Visible = true;
                btn_popupstaff_save.Visible = true;
                btn_popupstaff_exit.Visible = true;

                lbl_errorsearch.Visible = true;
                //lbl_errorsearch.Text = "No Records Found";
                //lbl_errorsearch.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                // Fpstaff.Height = 370;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();
                err.Visible = false;
            }
            else
            {
                Fpstaff.Visible = false;
                lbl_errorsearch.Visible = false;
                btn_popupstaff_save.Visible = false;
                btn_popupstaff_exit.Visible = false;
                err.Visible = true;
                err.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_popupstaff_save_Click(object sender, EventArgs e)
    {
        try
        {

            string dept = "";
            string desg = "";
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
                        name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                        TextBox2.Text = name;
                        dept = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                        TextBox3.Text = dept;
                        desg = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                        TextBox4.Text = desg;
                        desg = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                        TextBox5.Text = desg;
                        // wardencode = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    }
                    popupstaff.Visible = false;
                }
                else
                {
                    lbl_errorsearch.Visible = true;
                    lbl_errorsearch.Text = "Please Select Any One Staff";
                }

            }
            else
            {
                lbl_alert.Text = "No Record found  Successfully";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
                Fpstaff.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_popupstaff_exit_Click(object sender, EventArgs e)
    {
        try
        {
            popupstaff.Visible = false;

        }
        catch
        {
        }
    }
    public void hide()
    {
        popupstud.Visible = false;
        popupstaff.Visible = false;
    }
    public void btn_vendor_go_Click(object sender, EventArgs e)
    {
        try
        {
            string vencomvalue = "";
            for (int i = 0; i < chkl_vencomname.Items.Count; i++)
            {
                if (chkl_vencomname.Items[i].Selected == true)
                {
                    if (vencomvalue == "")
                    {
                        vencomvalue = "" + chkl_vencomname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        vencomvalue = vencomvalue + "'" + "," + "'" + chkl_vencomname.Items[i].Value.ToString() + "";
                    }
                }
            }

            if (vencomvalue != "")
            {
                string selqry = "select VendorCompName,VendorCode ,VendorPK  from CO_VendorMaster where VendorType =1 and VendorCode in ('" + vencomvalue + "')";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selqry, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    Fpsvender.Sheets[0].RowHeader.Visible = false;
                    Fpsvender.CommandBar.Visible = false;
                    Fpsvender.Sheets[0].RowCount = 0;
                    Fpsvender.SheetCorner.ColumnCount = 0;
                    Fpsvender.Sheets[0].ColumnHeader.RowCount = 1;

                    Fpsvender.Sheets[0].AutoPostBack = true;
                    Fpsvender.Sheets[0].ColumnCount = 3;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpsvender.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Company Code";
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Company Name";
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpsvender.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;


                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpsvender.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpsvender.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                            Fpsvender.Sheets[0].Cells[i, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCode"].ToString());
                            Fpsvender.Sheets[0].Cells[i, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["VendorPK"].ToString());
                            Fpsvender.Sheets[0].Cells[i, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"].ToString());
                        }

                        Fpsvender.Sheets[0].ColumnHeader.Columns[0].Width = 100;
                        Fpsvender.Sheets[0].ColumnHeader.Columns[1].Width = 250;
                        Fpsvender.Sheets[0].ColumnHeader.Columns[2].Width = 250;


                        for (int jj = 0; jj < Fpsvender.Sheets[0].Columns.Count; jj++)
                        {
                            Fpsvender.Sheets[0].Columns[jj].HorizontalAlign = HorizontalAlign.Center;
                        }
                        Fpsvender.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                        for (int i = 0; i < Fpsvender.Sheets[0].Columns.Count; i++)
                        {
                            Fpsvender.Sheets[0].Columns[i].Locked = true;
                        }
                        Fpsvender.Sheets[0].PageSize = ds.Tables[0].Rows.Count;

                        divFpsvender.Visible = true;
                        Fpsvender.Visible = true;
                        //  div1.Visible = true;
                        btn_vendor_save.Visible = true;
                        btn_vendor_exit.Visible = true;
                        lbl_error.Visible = false;

                    }
                }
            }
            else
            {
                Fpsvender.Visible = false;
                divFpsvender.Visible = false;
                // div1.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select Any Comapany Name";
            }


        }
        // }
        catch
        {

        }
    }

    protected void ImageButton2_Click(object sender, EventArgs e)
    {
        popupvender.Visible = false;
    }
    public void btnorderr_Click(object sender, EventArgs e)
    {
        popuporderid.Visible = true;
    }
    public void imagebtnorder_Click(object sender, EventArgs e)
    {
        popuporderid.Visible = false;
    }
    protected void btn_go_order_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    int sno = 0;
        //    string code = ""; ;
        //    lbl_error3.Visible = false;
        //    fpbuild.SaveChanges();
        //    fpbuild.SheetCorner.ColumnCount = 0;
        //    fpbuild.Sheets[0].RowCount = 0;
        //    fpbuild.Sheets[0].ColumnCount = 0;
        //    fpbuild.Sheets[0].ColumnCount = 4;
        //    fpbuild.CommandBar.Visible = false;

        //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        //    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        //    darkstyle.ForeColor = Color.White;
        //    fpbuild.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        //    FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
        //    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
        //    fpbuild.Sheets[0].AutoPostBack = false;

        //    sql = "SELECT Code,Building_Name FROM Building_Master where (Building_Name like'" + txt_order.Text + "%')";
        //    ds = d2.select_method_wo_parameter(sql, "Text");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        fpbuild.Visible = true;
        //        btn_ok.Visible = true;
        //        btn_exit3.Visible = true;

        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        //        fpbuild.Sheets[0].Columns[0].Width = 60;
        //        fpbuild.Sheets[0].Columns[0].Locked = true;

        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Order Code";
        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        //        fpbuild.Sheets[0].Columns[2].Locked = true;
        //        fpbuild.Sheets[0].Columns[2].Width = 60;

        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Order Date";
        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        //        fpbuild.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        //        fpbuild.Sheets[0].Columns[3].Locked = true;
        //        fpbuild.Sheets[0].Columns[3].Width = 130;

        //        fpbuild.Width = 360;

        //        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            sno++;
        //            code = ds.Tables[0].Rows[i]["Code"].ToString();
        //            name = ds.Tables[0].Rows[i]["Building_Name"].ToString();

        //            fpbuild.Sheets[0].RowCount = fpbuild.Sheets[0].RowCount + 1;
        //            fpbuild.Sheets[0].Rows[fpbuild.Sheets[0].RowCount - 1].Font.Bold = false;
        //            fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
        //            fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

        //            fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 1].CellType = chkcell1;
        //            fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
        //            fpbuild.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        //            chkcell1.AutoPostBack = false;

        //            fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 2].Text = code;
        //            fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

        //            fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 3].Text = name;
        //            fpbuild.Sheets[0].Cells[fpbuild.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
        //        }
        //        rowcount = fpbuild.Sheets[0].RowCount;
        //        fpbuild.Height = 170;
        //        fpbuild.Sheets[0].PageSize = 15 + (rowcount * 5);
        //        fpbuild.SaveChanges();
        //    }
        //    else
        //    {
        //        fpbuild.Visible = false;
        //        btn_ok.Visible = false;
        //        btn_exit3.Visible = false;
        //        lbl_error3.Visible = true;
        //        lbl_error3.Text = "No Records Found";
        //    }
        //}
        //catch (Exception ex)
        //{
        //}
    }
    protected void btn_order_save_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    y = 0;
        //    int isval = 0;
        //    string value = "";
        //    string builcode = "";
        //    txt_building1.Text = "";
        //    fpbuild.SaveChanges();
        //    for (i = 0; i < fpbuild.Rows.Count; i++)
        //    {
        //        isval = Convert.ToInt32(fpbuild.Sheets[0].Cells[i, 1].Value);
        //        if (isval == 1)
        //        {
        //            if (value == "")
        //            {
        //                value = fpbuild.Sheets[0].Cells[i, 3].Text;
        //                builcode = Convert.ToString(fpbuild.Sheets[0].Cells[i, 2].Text);
        //            }
        //            else
        //            {
        //                value = value + ',' + fpbuild.Sheets[0].Cells[i, 3].Text;
        //                builcode = builcode + "," + Convert.ToString(fpbuild.Sheets[0].Cells[i, 2].Text);
        //            }
        //            y = 1;
        //        }
        //    }
        //    if (y == 1)
        //    {
        //        txt_building.Text = value;
        //        ViewState["BuildingCode"] = Convert.ToString(builcode);
        //        popupbuild1.Visible = false;
        //    }
        //    else
        //    {
        //        lbl_error3.Visible = true;
        //        lbl_error3.Text = "Please Select Any One Building Name";
        //    }
        //}
        //catch (Exception ex)
        //{
        //}
    }
    protected void btn_order_exit_Click(object sender, EventArgs e)
    {
        try
        {
            popuporderid.Visible = false;
        }
        catch
        {
        }
    }
    public void btn_vendor_save_Click(object sender, EventArgs e)
    {
        try
        {
            string dept = "";
            string desg = "";
            string activerow = "";
            string activecol = "";
            if (Fpsvender.Sheets[0].RowCount != 0)
            {
                activerow = Fpsvender.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpsvender.ActiveSheetView.ActiveColumn.ToString();
                if (activerow != Convert.ToString(-1))
                {
                    name = Fpsvender.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                    vencontcode = Convert.ToString(Fpsvender.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    txt_venname.Text = name;
                    vendor_code = Fpsvender.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                    popupvender.Visible = false;

                }
                else
                {
                    lbl_errorsearch.Visible = true;
                    lbl_errorsearch.Text = "Please Select Any One Company Name";
                }

            }
            else
            {
                lbl_errorsearch1.Visible = true;
                lbl_errorsearch1.Text = "No Records Found";
                divFpsvender.Visible = false;
                Fpsvender.Visible = false;

            }
        }

        catch (Exception ex)
        {
        }

    }
    public void btn_vendor_exit_Click(object sender, EventArgs e)
    {
        popupvender.Visible = false;
    }


    public void txt_rollno_OnTextChanged(object sender, EventArgs e)
    {
        //loadledger();
        //loaddebit();
        lbl_er.Text = "";
        lbl_er.Visible = false;
        Roll_No = txt_rollno.Text.ToString();
        if (Roll_No != "" && Roll_No != null)
        {
            getData(Roll_No);
            double paymentvalue = 0;
            paymentvalue = paymentvalue + getAppno(Roll_No);
            paymentvalue = paymentvalue + getpayamount(Roll_No);
            if (paymentvalue != 0)
            {
                lbl_er.Text = "";
                txt_ventotpayamt.Text = Convert.ToString(paymentvalue);
                txt_venpayamt.Text = Convert.ToString(paymentvalue);
                txt_amt.Text = Convert.ToString(paymentvalue);
            }
            else
            {
                txt_ventotpayamt.Text = "";
                txt_venpayamt.Text = "";
                txt_lbl_vencurbal.Text = "";
                lbl_er.Text = "There Is No Payable Amount";
                lbl_er.Visible = true;
            }
        }
        else
        {
            txt_rollno.Text = "";
            txt_name.Text = "";
            txt_batch.Text = "";
            txt_degr.Text = "";
            txt_dept.Text = "";
            txt_sem.Text = "";
            txt_sec.Text = "";
            imb_studpht.Visible = false;
            lbl_er.Text = "";
            txt_ventotpayamt.Text = "";
            txt_venpayamt.Text = "";
            chk_insexcess.Checked = false;
            chk_deposite.Checked = false;
        }
    }
    public void getData(string Roll_No)
    {
        try
        {

            string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code";
            //and r.Roll_no='" + Roll_No + "'";
            if (Roll_No != "" && Roll_No != null)
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        query = query + " and r.Roll_no='" + Roll_No + "' and d.college_code=" + ddl_collegename.SelectedItem.Value + "";
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        query = query + " and r.Reg_No='" + Roll_No + "' and d.college_code=" + ddl_collegename.SelectedItem.Value + "";
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        query = query + " and r.Roll_Admit='" + Roll_No + "' and d.college_code=" + ddl_collegename.SelectedItem.Value + "";
                    }
                }
                else
                {
                    query = "select stud_name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and admission_status =0 and isconfirm ='1' and app_formno = '" + Roll_No + "'";
                }
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        // txt_rollno.Text = ds.Tables[0].Rows[i]["Roll_no"].ToString();
                        txt_name.Text = ds.Tables[0].Rows[i]["stud_name"].ToString();
                        txt_batch.Text = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_degr.Text = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_dept.Text = ds.Tables[0].Rows[i]["Dept_Name"].ToString(); ;
                        txt_sem.Text = ds.Tables[0].Rows[i]["Current_Semester"].ToString();
                        txt_sec.Text = ds.Tables[0].Rows[i]["Sections"].ToString();
                        imb_studpht.Visible = true;
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        {
                            Roll_No = d2.GetFunction("select roll_no from registration where reg_no='" + Roll_No + "'");
                        }
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        {
                            Roll_No = d2.GetFunction("select roll_no from registration where Roll_admit='" + Roll_No + "'");
                        }
                        string photo = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + Roll_No + "')";
                        ds = d2.select_method_wo_parameter(photo, "Text");
                        if (photo != "")
                        {
                            imb_studpht.ImageUrl = "~/Handler4.ashx?rollno=" + Roll_No;
                        }
                        else
                        {
                            imb_studpht.Visible = false;
                        }
                    }
                    popupstud.Visible = false;
                }
            }
            else
            {
                txt_rollno.Text = "";
                txt_name.Text = "";
                txt_batch.Text = "";
                txt_degr.Text = "";
                txt_dept.Text = "";
                txt_sem.Text = "";
                txt_sec.Text = "";
                imb_studpht.Visible = false;
                lbl_er.Text = "";
                txt_venpayamt.Text = "";
                chk_insexcess.Checked = false;
            }
        }
        catch
        {

        }
    }
    public double getAppno(string Roll_No)
    {
        double newtotal = 0;
        try
        {
            string selqry = "";
            string date = "";
            string date1 = "";
            string instaldt = "";
            string instaldate = "";
            string dt = "";
            string mn = "";
            string yr = "";
            string dateonly = "";
            string tme = "";
            string amorpm = "";
            string fndt = "";
            date1 = txt_vdate.Text.ToString();
            string[] strdate1 = date1.Split('/');
            if (strdate1.Length > 0)
            {
                date = strdate1[0].ToString() + "/" + strdate1[1].ToString() + "/" + strdate1[2].ToString();
            }



            // string selqry = d2.GetFunction(" select App_No from Registration where Roll_No='" + Roll_No + "'");
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
            {
                selqry = d2.GetFunction(" select App_No from Registration where Roll_No='" + Roll_No + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
            {
                selqry = d2.GetFunction(" select App_No from Registration where reg_no='" + Roll_No + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
            {
                selqry = d2.GetFunction(" select App_No from Registration where Roll_admit='" + Roll_No + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
            {
                selqry = d2.GetFunction(" select app_no from applyn where app_formno='" + Roll_No + "'");
            }
            string selqryinstdate = "select InstallmentDate,InstallmentAmt from FT_FinInstallmentPay where App_No='" + selqry + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqryinstdate, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    instaldt = ds.Tables[0].Rows[i][0].ToString();
                    string[] insdatetime = instaldt.Split(' ');
                    if (insdatetime.Length > 0)
                    {
                        dateonly = insdatetime[0].ToString();
                        tme = insdatetime[1].ToString();
                        amorpm = insdatetime[2].ToString();
                    }
                    DateTime dtt = new DateTime();
                    string[] instalftdt = dateonly.Split('/');
                    if (instalftdt.Length > 0)
                    {
                        dt = instalftdt[0].ToString();
                        mn = instalftdt[1].ToString();
                        yr = instalftdt[2].ToString();
                        instaldate = dt + "/" + mn + "/" + yr;
                        dtt = Convert.ToDateTime(instaldate);
                        fndt = dtt.ToString("dd/MM/yyyy");
                    }
                    if (date == fndt)
                    {

                        string selqryamt = d2.GetFunction(" select SUM(InstallmentAmt) as amt,App_No from FT_FinInstallmentPay where InstallmentDate='" + dtt.ToString("MM/dd/yyyy") + "' and ISNULL(InstallmentStatus,'0')='0'  group by App_No");
                        if (selqryamt.Trim() != "" && selqryamt.Trim() != "0")
                        {
                            newtotal = Convert.ToDouble(selqryamt);
                        }
                        //txt_ventotpayamt.Text = selqryamt;
                        //txt_venpayamt.Text = selqryamt;
                    }
                }
            }


        }
        catch
        {

        }
        return newtotal;
    }
    public double getpayamount(string Roll_No)
    {
        double totalvalue = 0;
        try
        {
            string selqry = "";
            //string selqry = d2.GetFunction(" select App_No from Registration where Roll_No='" + Roll_No + "'");
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
            {
                selqry = d2.GetFunction(" select App_No from Registration where Roll_No='" + Roll_No + "' and college_code='" + ddl_collegename.SelectedItem.Value + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
            {
                selqry = d2.GetFunction(" select App_No from Registration where reg_no='" + Roll_No + "' and college_code='" + ddl_collegename.SelectedItem.Value + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
            {
                selqry = d2.GetFunction(" select App_No from Registration where Roll_admit='" + Roll_No + "' and college_code='" + ddl_collegename.SelectedItem.Value + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
            {
                selqry = d2.GetFunction(" select app_no from applyn where app_formno='" + Roll_No + "' and college_code='" + ddl_collegename.SelectedItem.Value + "'");
            }
            string selqrypay = "SELECT SUM(ISNULL(ExcessAmt,'0') -ISNULL(AdjAmt,'0')) as payamount FROM FT_ExcessDet WHERE App_No = '" + selqry + "'";
            if (chk_insexcess.Checked == true && chk_deposite.Checked == true)
            {
                selqrypay = selqrypay + " and ExcessType in ('1','2','3')";
            }
            else if (chk_insexcess.Checked == true)
            {
                selqrypay = selqrypay + " and ExcessType in ('1','2')";
            }
            else if (chk_deposite.Checked == true)
            {
                selqrypay = selqrypay + " and ExcessType in ('2','3')";
            }
            else
            {
                selqrypay = selqrypay + " and ExcessType in ('2')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqrypay, "Text");
            string payamt = Convert.ToString(ds.Tables[0].Rows[0]["payamount"]);

            if (payamt != null && payamt != "")
            {
                totalvalue = Convert.ToDouble(payamt);
            }

        }
        catch { }
        return totalvalue;
    }

    protected void chk_insexcess_Changed(object sender, EventArgs e)
    {
        txt_rollno_OnTextChanged(sender, e);
    }
    protected void chk_deposite_Changed(object sender, EventArgs e)
    {
        txt_rollno_OnTextChanged(sender, e);
    }

    public void txt_name_OnTextChanged(object sender, EventArgs e)
    {
        lbl_er.Text = "";
        string name = "";
        string ini = "";
        string deg = "";
        string dept = "";
        string rollno = "";
        stud_name = txt_name.Text.ToString();
        string[] strstudname = stud_name.Split('-');
        if (strstudname.Length == 5)
        {
            stud_name = strstudname[0].ToString();
            ini = strstudname[1].ToString();
            deg = strstudname[2].ToString();
            dept = strstudname[3].ToString();
            Roll_No = strstudname[4].ToString();
        }
        getName(stud_name);
    }
    public void getName(string stud_name)
    {
        try
        {
            string name = txt_name.Text.ToString();
            string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and a.stud_name='" + stud_name + "'";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string roll = "";
                    txt_rollno.Text = ds.Tables[0].Rows[i]["Roll_no"].ToString();
                    roll = txt_rollno.Text.ToString();
                    txt_name.Text = ds.Tables[0].Rows[0]["stud_name"].ToString();
                    txt_batch.Text = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                    txt_degr.Text = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                    txt_dept.Text = ds.Tables[0].Rows[i]["Dept_Name"].ToString(); ;
                    txt_sem.Text = ds.Tables[0].Rows[i]["Current_Semester"].ToString();
                    txt_sec.Text = ds.Tables[0].Rows[i]["Sections"].ToString();
                    imb_studpht.Visible = true;
                    imb_studpht.ImageUrl = "~/Handler4.ashx?rollno=" + roll;
                }
                popupstud.Visible = false;
            }
            else
            {
                txt_rollno.Text = "";
                txt_name.Text = "";
                txt_batch.Text = "";
                txt_degr.Text = "";
                txt_dept.Text = "";
                txt_sem.Text = "";
                txt_sec.Text = "";
                imb_studpht.Visible = false;
                lbl_er.Text = "";
            }
        }
        catch
        {

        }
    }

    //staff 
    public void TextBox2_OnTextChanged(object sender, EventArgs e)
    {
        //div_cash.Visible = false;
        btnaddgrid.Visible = false;
        paidDiv.Visible = false;

        staff_Code = TextBox2.Text.ToString();
        getstaffcode(staff_Code);
        getstaffapplno(staff_Code);
        loadpayLedger();
    }

    public void getstaffapplno(string staff_Code)
    {
        try
        {
            string date = "";
            string date1 = "";
            string instaldt = "";
            string instaldate = "";
            string dt = "";
            string mn = "";
            string yr = "";
            string dateonly = "";
            string tme = "";
            string amorpm = "";
            string fndt = "";
            date1 = txt_vdate.Text.ToString();
            string[] strdate1 = date1.Split('/');
            if (strdate1.Length > 0)
            {
                date = strdate1[0].ToString() + "/" + strdate1[1].ToString() + "/" + strdate1[2].ToString();
            }
            string selqry = d2.GetFunction(" select sa.appl_id  from staffmaster s,staff_appl_master sa where s.appl_no =sa.appl_no and s.staff_code ='" + staff_Code + "'");
            string selqryinstdate = "select InstallmentDate,InstallmentAmt from FT_FinInstallmentPay where App_No='" + selqry + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqryinstdate, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        instaldt = ds.Tables[0].Rows[i][0].ToString();
                        string[] insdatetime = instaldt.Split(' ');
                        if (insdatetime.Length > 0)
                        {
                            dateonly = insdatetime[0].ToString();
                            tme = insdatetime[1].ToString();
                            amorpm = insdatetime[2].ToString();
                        }
                        DateTime dtt = new DateTime();
                        string[] instalftdt = dateonly.Split('/');
                        if (instalftdt.Length > 0)
                        {
                            dt = instalftdt[0].ToString();
                            mn = instalftdt[1].ToString();
                            yr = instalftdt[2].ToString();
                            instaldate = dt + "/" + mn + "/" + yr;
                            dtt = Convert.ToDateTime(instaldate);
                            fndt = dtt.ToString("dd/MM/yyyy");
                        }
                        if (date == fndt)
                        {

                            string selqryamt = d2.GetFunction(" select SUM(InstallmentAmt) as amt,App_No from FT_FinInstallmentPay where InstallmentDate='" + dtt.ToString("MM/dd/yyyy") + "' and ISNULL(InstallmentStatus,'0')='0'  group by App_No");
                            txt_ventotpayamt.Text = selqryamt;
                            txt_venpayamt.Text = selqryamt;
                            txt_amt.Text = selqryamt;
                        }
                    }
                }
            }

        }
        catch
        {

        }
    }

    public void getstaffcode(string staff_Code)
    {
        try
        {
            string query = "select  appl_id,s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode,CONVERT(varchar(10), s.join_date,103) as join_date,st.stftype  from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm,stafftrans st where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and st.staff_code =s.staff_code and latestrec =1 and s.staff_Code='" + staff_Code + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string appNo = string.Empty;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string stafcode = "";
                        TextBox2.Text = ds.Tables[0].Rows[i]["staff_Code"].ToString();
                        stafcode = TextBox2.Text.ToString();
                        TextBox3.Text = ds.Tables[0].Rows[i]["staff_name"].ToString();
                        TextBox4.Text = ds.Tables[0].Rows[i]["dept_name"].ToString();
                        TextBox5.Text = ds.Tables[0].Rows[i]["desig_name"].ToString();
                        txt_stftype.Text = ds.Tables[0].Rows[i]["stftype"].ToString();
                        txt_stfcat.Text = ds.Tables[0].Rows[i]["staffcategory"].ToString();
                        txt_stfjn.Text = ds.Tables[0].Rows[i]["join_date"].ToString();
                        ImageButton3.ImageUrl = "staffphoto.ashx?staff_code=" + stafcode;
                        appNo = Convert.ToString(ds.Tables[0].Rows[i]["appl_id"]);
                        if (ImageButton3.ImageUrl != "")
                        {
                            ImageButton3.Visible = true;
                        }
                        else
                        {
                            ImageButton3.Visible = false;
                        }

                    }
                    if (cbStaff.Checked)
                        getFeesDetails(appNo);
                    else
                        div_cash.Visible = true;
                    //Div5.Visible = true;
                }
            }
            else
            {
                TextBox2.Text = "";
                TextBox3.Text = "";
                TextBox4.Text = "";
                TextBox5.Text = "";
                txt_stftype.Text = "";
                txt_stfcat.Text = "";
                txt_stfjn.Text = "";
                ImageButton3.Visible = false;
            }

        }
        catch
        {

        }
    }

    public void TextBox3_OnTextChanged(object sender, EventArgs e)
    {
        string staffname = TextBox3.Text.ToString();

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
            string query = "select appl_id,s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode,CONVERT(varchar(10), s.join_date,103) as join_date,st.stftype  from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm,stafftrans st where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and st.staff_code =s.staff_code and latestrec =1 and s.staff_name='" + staff_name + "'";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string appNo = string.Empty;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string stafcode = "";
                    TextBox2.Text = ds.Tables[0].Rows[i]["staff_Code"].ToString();
                    stafcode = TextBox2.Text.ToString();
                    TextBox3.Text = ds.Tables[0].Rows[i]["staff_name"].ToString();
                    TextBox4.Text = ds.Tables[0].Rows[i]["dept_name"].ToString();
                    TextBox5.Text = ds.Tables[0].Rows[i]["desig_name"].ToString();
                    txt_stftype.Text = ds.Tables[0].Rows[i]["stftype"].ToString();
                    txt_stfcat.Text = ds.Tables[0].Rows[i]["staffcategory"].ToString();
                    txt_stfjn.Text = ds.Tables[0].Rows[i]["join_date"].ToString();
                    appNo = Convert.ToString(ds.Tables[0].Rows[i]["appl_id"]);
                    ImageButton3.Visible = true;
                    ImageButton3.ImageUrl = "staffphoto.ashx?staff_code=" + stafcode;


                }
                if (cbStaff.Checked)
                    getFeesDetails(appNo);
                else
                    div_cash.Visible = true;
                //Div5.Visible = true;
            }
            else
            {
                TextBox2.Text = "";
                TextBox3.Text = "";
                TextBox4.Text = "";
                TextBox5.Text = "";
                txt_stftype.Text = "";
                txt_stfcat.Text = "";
                txt_stfjn.Text = "";
                ImageButton3.Visible = false;
            }

        }
        catch
        {

        }
    }

    protected DataSet getFeesDetails(string appNo)
    {
        DataSet dsLoad = new DataSet();
        try
        {
            paidDiv.Visible = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("Sno");
            dt.Columns.Add("hdName");
            dt.Columns.Add("hdFK");
            dt.Columns.Add("ldName");
            dt.Columns.Add("ldFK");
            dt.Columns.Add("Amount");
            DataRow dr;
            double totalAmt = 0;
            double paidToAmt = 0;
            string selQ = " select f.headerfk,headername,ledgerfk,ledgername,feecategory,(isnull(totalamount,'0')-isnull(paidamount,'0')) as totalamount,paidamount,balamount from ft_feeallot f,fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and f.headerfk=h.headerpk and f.ledgerfk=l.ledgerpk and f.headerfk=l.headerfk and isnull(isvoucher,'0')='1' and memtype='2' and app_no in('" + appNo + "')";
            DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                int rowCnt = 0;
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    dr = dt.NewRow();
                    double allotAmt = 0;
                    double.TryParse(Convert.ToString(dsVal.Tables[0].Rows[row]["totalamount"]), out allotAmt);
                    double paidAmt = 0;
                    double.TryParse(Convert.ToString(dsVal.Tables[0].Rows[row]["paidamount"]), out paidAmt);
                    dr["Sno"] = Convert.ToString(++rowCnt);
                    dr["hdName"] = Convert.ToString(dsVal.Tables[0].Rows[row]["headername"]);
                    dr["hdFK"] = Convert.ToString(dsVal.Tables[0].Rows[row]["headerfk"]);
                    dr["ldName"] = Convert.ToString(dsVal.Tables[0].Rows[row]["ledgername"]);
                    dr["ldFK"] = Convert.ToString(dsVal.Tables[0].Rows[row]["ledgerfk"]);
                    dr["Amount"] = Convert.ToString(allotAmt);
                    totalAmt += allotAmt;
                    paidToAmt += paidAmt;
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    txt_ventotpayamt.Text = Convert.ToString(totalAmt);
                    txt_venpayamt.Text = Convert.ToString(totalAmt - paidToAmt);
                    double tempPaid = 0;
                    double.TryParse(Convert.ToString(txt_venpayamt.Text), out tempPaid);
                    txtpayamt.Text = Convert.ToString(totalAmt - tempPaid);
                    gdPaid.DataSource = dt;
                    gdPaid.DataBind();
                    div_cash.Visible = false;
                    btnaddgrid.Visible = false;
                    paidDiv.Visible = true;
                }
            }
        }
        catch { }
        return dsLoad;
    }
    protected void cbStaff_Changed(object sender, EventArgs e)
    {
        TextBox2_OnTextChanged(sender, e);
    }
    protected void gdPaid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    TextBox txtAmt = (TextBox)e.Row.Cells[3].FindControl("txtAmt");
        //}
    }


    protected void Marksgrid_pg_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            (gridView1.Rows[0].FindControl("ddl_headername") as DropDownList).Items.Clear();
            (gridView1.Rows[0].FindControl("ddl_ledgername") as DropDownList).Items.Clear();
            if (gridView1.Rows.Count > 0)
            {
                // lbl_er.Visible = false;
                for (int a = 0; a < gridView1.Rows.Count; a++)
                {
                    // string englisquery = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "'";

                    string englisquery = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";
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
                    //  lbl_er.Visible = false;
                    //  englisquery = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode='" + collegecode1 + "' and LedgerMode=1 order by isnull(priority,1000), ledgerName asc";

                    englisquery = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode1 + "  and LedgerMode=1   order by isnull(l.priority,1000), l.ledgerName asc ";
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
                div_cash.Visible = true;
                //Div5.Visible = true;
            }
        }
        catch
        {

        }


    }
    protected void Marksgrid_pg1_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            (gridView3.Rows[0].FindControl("ddl_credit") as DropDownList).Items.Clear();
            (gridView3.Rows[0].FindControl("ddl_debit") as DropDownList).Items.Clear();
            if (gridView3.Rows.Count > 0)
            {
                // lbl_er.Visible = false;
                for (int a = 0; a < gridView3.Rows.Count; a++)
                {
                    // string englisquery = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "'";

                    string englisquery = "select distinct LedgerName,LedgerPK from FM_LedgerMaster WHERE CollegeCode='" + collegecodestat + "' and ledgermode in ('0','2')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(englisquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        (gridView3.Rows[a].FindControl("ddl_credit") as DropDownList).DataSource = ds;
                        (gridView3.Rows[a].FindControl("ddl_credit") as DropDownList).DataTextField = "LedgerName";
                        (gridView3.Rows[a].FindControl("ddl_credit") as DropDownList).DataValueField = "LedgerPK";
                        (gridView3.Rows[a].FindControl("ddl_credit") as DropDownList).DataBind();
                    }
                    //(gridView3.Rows[a].FindControl("ddl_credit") as DropDownList).Items.Insert(0, "Select");
                    //  lbl_er.Visible = false;
                    //  englisquery = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode='" + collegecode1 + "' and LedgerMode=1 order by isnull(priority,1000), ledgerName asc";

                    englisquery = "select distinct LedgerName,LedgerPK from FM_LedgerMaster WHERE CollegeCode='" + collegecodestat + "' and ledgermode in ('1','2')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(englisquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        (gridView3.Rows[a].FindControl("ddl_debit") as DropDownList).DataSource = ds;
                        (gridView3.Rows[a].FindControl("ddl_debit") as DropDownList).DataTextField = "LedgerName";
                        (gridView3.Rows[a].FindControl("ddl_debit") as DropDownList).DataValueField = "LedgerPK";
                        (gridView3.Rows[a].FindControl("ddl_debit") as DropDownList).DataBind();
                    }
                    //(gridView3.Rows[a].FindControl("ddl_debit") as DropDownList).Items.Insert(0, "Select");

                }
                div_cash.Visible = true;
                //Div5.Visible = true;
            }
        }
        catch
        {

        }


    }
    protected void gridven_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            if (gridven.Rows.Count > 0)
            {
                for (int i = 0; i < gridven.Rows.Count; i++)
                {
                    // string selqry = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "'";
                    string selqry = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selqry, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        (gridven.Rows[i].FindControl("ddlheader") as DropDownList).DataSource = ds;
                        (gridven.Rows[i].FindControl("ddlheader") as DropDownList).DataTextField = "HeaderName";
                        (gridven.Rows[i].FindControl("ddlheader") as DropDownList).DataValueField = "HeaderPK";
                        (gridven.Rows[i].FindControl("ddlheader") as DropDownList).DataBind();
                    }
                    (gridven.Rows[i].FindControl("ddlheader") as DropDownList).Items.Insert(0, "Select");

                    // selqry = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode='" + collegecode1 + "' and LedgerMode=1 order by isnull(priority,1000),ledgerName asc";

                    selqry = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode1 + "  and LedgerMode=1   order by isnull(l.priority,1000), l.ledgerName asc ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selqry, "Text");
                    ds = d2.select_method_wo_parameter(selqry, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        (gridven.Rows[i].FindControl("ddlledger") as DropDownList).DataSource = ds;
                        (gridven.Rows[i].FindControl("ddlledger") as DropDownList).DataTextField = "LedgerName";
                        (gridven.Rows[i].FindControl("ddlledger") as DropDownList).DataValueField = "LedgerPK";
                        (gridven.Rows[i].FindControl("ddlledger") as DropDownList).DataBind();
                    }
                    (gridven.Rows[i].FindControl("ddlledger") as DropDownList).Items.Insert(0, "Select");
                }
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
                DropDownList box1 = new DropDownList();
                DropDownList box2 = new DropDownList();
                TextBox box3 = new TextBox();
                for (int i = 0; i < gridView1.Rows.Count; i++)
                {
                    box1 = (DropDownList)gridView1.Rows[i].Cells[1].FindControl("ddl_headername");
                    box2 = (DropDownList)gridView1.Rows[i].Cells[2].FindControl("ddl_ledgername");
                    box3 = (TextBox)gridView1.Rows[i].Cells[3].FindControl("txtpaymt");

                    if (box1.Text != "Select" && box2.Text != "Select" && box3.Text.Trim() != "")
                    {
                        if (box1.Text != "" && box2.Text != "" && box3.Text.Trim() != "")
                            emptyflage = false;
                        else
                            emptyflage = true;
                    }
                    else
                        emptyflage = true;
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



    public void btnaddgriddts_Click(object sender, EventArgs e)
    {
        try
        {
            bool emptyflage = false;
            if (gridView3.Rows.Count > 0)
            {
                DropDownList box1 = new DropDownList();
                DropDownList box2 = new DropDownList();
                TextBox box3 = new TextBox();
                for (int i = 0; i < gridView1.Rows.Count; i++)
                {
                    box1 = (DropDownList)gridView3.Rows[i].Cells[1].FindControl("ddl_credit");
                    box2 = (DropDownList)gridView3.Rows[i].Cells[2].FindControl("ddl_debit");
                    box3 = (TextBox)gridView3.Rows[i].Cells[3].FindControl("txtpaymt");

                    if (box1.Text != "Select" && box2.Text != "Select" && box3.Text.Trim() != "")
                    {
                        if (box1.Text != "" && box2.Text != "" && box3.Text.Trim() != "")
                            emptyflage = false;
                        else
                            emptyflage = true;
                    }
                    else
                        emptyflage = true;
                }
            }
            //.style.borderColor = 'Red'
            if (emptyflage == true)
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Fill All The Fields\");", true);
            else
            {
                AddNewRowToGriddts();
                Marksgrid_pg1_DataBound(sender, e);
                // gridView1_OnRowCommand(sender, e);                
                SetPreviousDatadts();
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
            DropDownList box1 = new DropDownList();
            DropDownList box2 = new DropDownList();
            TextBox box3 = new TextBox();


            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    box1 = (DropDownList)gridView1.Rows[i].Cells[1].FindControl("ddl_headername");
                    box2 = (DropDownList)gridView1.Rows[i].Cells[2].FindControl("ddl_ledgername");
                    box3 = (TextBox)gridView1.Rows[i].Cells[3].FindControl("txtpaymt");
                    //  drCurrentRow["RowNumber"] = i + 1;
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows[i][0] = Convert.ToString(i + 1);
                    dtCurrentTable.Rows[i][1] = box1.Text;
                    dtCurrentTable.Rows[i][2] = box2.Text;
                    dtCurrentTable.Rows[i][3] = box3.Text;

                    rowIndex++;
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
    public void AddNewRowToGriddts()
    {
        int rowIndex = 0;

        if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            DropDownList box1 = new DropDownList();
            DropDownList box2 = new DropDownList();
            TextBox box3 = new TextBox();


            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    box1 = (DropDownList)gridView3.Rows[i].Cells[1].FindControl("ddl_credit");
                    box2 = (DropDownList)gridView3.Rows[i].Cells[2].FindControl("ddl_debit");
                    box3 = (TextBox)gridView3.Rows[i].Cells[3].FindControl("txtpaymt");
                    //  drCurrentRow["RowNumber"] = i + 1;
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows[i][0] = Convert.ToString(i + 1);
                    dtCurrentTable.Rows[i][1] = box1.Text;
                    dtCurrentTable.Rows[i][2] = box2.Text;
                    dtCurrentTable.Rows[i][3] = box3.Text;

                    rowIndex++;
                }

                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                gridView3.DataSource = dtCurrentTable;
                gridView3.DataBind();
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
                DropDownList box1 = new DropDownList();
                DropDownList box2 = new DropDownList();
                TextBox box3 = new TextBox();
                Label lbl = new Label();

                hashlist.Add(0, "Sno");
                hashlist.Add(1, "Header Name");
                hashlist.Add(2, "Ledger Name");
                hashlist.Add(3, "Amount");

                DataRow dr;

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    box1 = (DropDownList)gridView1.Rows[i].Cells[1].FindControl("ddl_headername");
                    box2 = (DropDownList)gridView1.Rows[i].Cells[2].FindControl("ddl_ledgername");
                    box3 = (TextBox)gridView1.Rows[i].Cells[3].FindControl("txtpaymt");
                    lbl = (Label)gridView1.Rows[i].Cells[3].FindControl("lbl_rs");
                    string val_file = Convert.ToString(hashlist[i]);
                    lbl.Text = Convert.ToString(i + 1);
                    //  ddlBatch_year.SelectedIndex = ddlBatch_year.Items.IndexOf(ddlBatch_year.Items.FindByText(Convert.ToString(Batch_year)));
                    string hedid = dt.Rows[i][1].ToString();
                    string ledgid = dt.Rows[i][2].ToString();
                    box1.SelectedIndex = box1.Items.IndexOf(box1.Items.FindByValue(Convert.ToString(dt.Rows[i][1])));
                    gridledgerload(hedid, i);
                    box2.SelectedIndex = box2.Items.IndexOf(box2.Items.FindByValue(Convert.ToString(dt.Rows[i][2])));
                    // box1.Text = dt.Rows[i][1].ToString();
                    //  box2.Text = dt.Rows[i][2].ToString();
                    box3.Text = dt.Rows[i][3].ToString();

                    rowIndex++;
                }
            }
        }
    }
    public void SetPreviousDatadts()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            Hashtable hashlist = new Hashtable();
            if (dt.Rows.Count > 0)
            {
                DropDownList box1 = new DropDownList();
                DropDownList box2 = new DropDownList();
                TextBox box3 = new TextBox();
                Label lbl = new Label();

                hashlist.Add(0, "Sno");
                hashlist.Add(1, "Credit Ledger");
                hashlist.Add(2, "Debit Ledger");
                hashlist.Add(3, "Amount");

                DataRow dr;

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    box1 = (DropDownList)gridView3.Rows[i].Cells[1].FindControl("ddl_credit");
                    box2 = (DropDownList)gridView3.Rows[i].Cells[2].FindControl("ddl_debit");
                    box3 = (TextBox)gridView3.Rows[i].Cells[3].FindControl("txtpaymt");
                    lbl = (Label)gridView3.Rows[i].Cells[3].FindControl("lbl_rs");
                    string val_file = Convert.ToString(hashlist[i]);
                    lbl.Text = Convert.ToString(i + 1);
                    //  ddlBatch_year.SelectedIndex = ddlBatch_year.Items.IndexOf(ddlBatch_year.Items.FindByText(Convert.ToString(Batch_year)));
                    string hedid = dt.Rows[i][1].ToString();
                    string ledgid = dt.Rows[i][2].ToString();
                    box1.SelectedIndex = box1.Items.IndexOf(box1.Items.FindByValue(Convert.ToString(dt.Rows[i][1])));
                    gridledgerloaddts(hedid, i);
                    box2.SelectedIndex = box2.Items.IndexOf(box2.Items.FindByValue(Convert.ToString(dt.Rows[i][2])));
                    // box1.Text = dt.Rows[i][1].ToString();
                    //  box2.Text = dt.Rows[i][2].ToString();
                    box3.Text = dt.Rows[i][3].ToString();

                    rowIndex++;
                }
            }
        }
    }
    public void txt_venname_OnTextChanged(object sender, EventArgs e)
    {
        string vendor_name = txt_venname.Text.ToString();
        string[] sstrspt = vendor_name.Split('-');
        if (sstrspt.Length > 0)
        {
            try
            {
                vendor_name = sstrspt[0].ToString();
                vendor_code = sstrspt[1].ToString();
            }
            catch { }
        }
        getvendorname(vendor_name);
        getvendorpk(vendor_code);
    }
    public void getvendorpk(string vendor_code)
    {
        try
        {
            string date = "";
            string date1 = "";
            string instaldt = "";
            string instaldate = "";
            string dt = "";
            string mn = "";
            string yr = "";
            string dateonly = "";
            string tme = "";
            string amorpm = "";
            string fndt = "";
            date1 = txt_vdate.Text.ToString();
            string[] strdate1 = date1.Split('/');
            if (strdate1.Length > 0)
            {
                date = strdate1[0].ToString() + "/" + strdate1[1].ToString() + "/" + strdate1[2].ToString();
            }

            string selqry = d2.GetFunction(" select VendorPK from CO_VendorMaster where VendorCode ='" + vendor_code + "'");
            string selqryinstdate = "select InstallmentDate,InstallmentAmt from FT_FinInstallmentPay where App_No='" + selqry + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqryinstdate, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        instaldt = ds.Tables[0].Rows[i][0].ToString();
                        string[] insdatetime = instaldt.Split(' ');
                        if (insdatetime.Length > 0)
                        {
                            dateonly = insdatetime[0].ToString();
                            tme = insdatetime[1].ToString();
                            amorpm = insdatetime[2].ToString();
                        }
                        DateTime dtt = new DateTime();
                        string[] instalftdt = dateonly.Split('/');
                        if (instalftdt.Length > 0)
                        {
                            dt = instalftdt[0].ToString();
                            mn = instalftdt[1].ToString();
                            yr = instalftdt[2].ToString();
                            instaldate = dt + "/" + mn + "/" + yr;
                            dtt = Convert.ToDateTime(instaldate);
                            fndt = dtt.ToString("dd/MM/yyyy");
                        }
                        if (date == fndt)
                        {

                            string selqryamt = d2.GetFunction(" select SUM(InstallmentAmt) as amt,App_No from FT_FinInstallmentPay where InstallmentDate='" + dtt.ToString("MM/dd/yyyy") + "' and ISNULL(InstallmentStatus,'0')='0'  group by App_No");
                            txt_ventotpayamt.Text = selqryamt;
                            txt_venpayamt.Text = selqryamt;
                            txt_amt.Text = selqryamt;
                        }
                    }
                }
            }

        }
        catch
        {

        }
    }
    public void getvendorname(string vendor_name)
    {
        string venco = "";
        string selqry = "select VendorCompName,VendorCode ,VendorPK  from CO_VendorMaster where VendorType =1 and  VendorCompName='" + vendor_name + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selqry, "Text");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    txt_venname.Text = ds.Tables[0].Rows[i]["VendorCompName"].ToString();
                    vencontcode = ds.Tables[0].Rows[i]["VendorPK"].ToString();
                    venco = ds.Tables[0].Rows[i]["VendorCode"].ToString();
                }
                ViewState["vendor_code"] = Convert.ToString(venco);
            }
        }
        else
        {
            txt_venname.Text = "";
            txt_cadd.Text = "";
            txt_cdesi.Text = "";
            txt_ccont.Text = "";
            //  txt_vencode.Text = "";
        }

    }
    public void txt_ccont_OnTextChanged(object sender, EventArgs e)
    {
        string vencontperson = txt_ccont.Text.ToString();
        // getvendorcode(vendor_code);
        //  getvendorappno(vendor_code);

        string fnladddesg0 = "";
        string fnladddesg1 = "";
        string fnladddesg2 = "";
        string venaddanddesg = txt_ccont.Text.ToString();
        if (venaddanddesg.Trim() != "")
        {
            string[] strsplit = venaddanddesg.Split('-');
            if (strsplit.Length > 0)
            {
                fnladddesg0 = strsplit[0].ToString();
                fnladddesg1 = strsplit[1].ToString();
                fnladddesg2 = strsplit[2].ToString();
            }

        }
        getadddesg(fnladddesg2);
        getvendorpk(vendor_code);

    }
    public void getadddesg(string fnladddesg2)
    {
        string ordercode = "";
        string narr = "";
        if (fnladddesg2.Trim() != "")
        {
            string selqry = "select VenContactName,VendorContactPK,VenContactDesig,VendorAddress  from IM_VendorContactMaster vc,CO_VendorMaster vm where VendorContactPK ='" + fnladddesg2 + "' and VenContactName <>'' and vc.VendorFK =vm.VendorPK ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txt_ccont.Text = ds.Tables[0].Rows[0]["VenContactName"].ToString();
                    txt_cadd.Text = ds.Tables[0].Rows[0]["VendorAddress"].ToString();
                    txt_cdesi.Text = ds.Tables[0].Rows[0]["VenContactDesig"].ToString();
                    if (ordercode != "" && ordercode != null)
                    {
                        TextBox12.Text = Convert.ToString(ordercode);
                        TextBox12.Attributes.Add("readonly", "readonly");
                    }
                    //narr = ds.Tables[0].Rows[i]["Narration"].ToString();
                    //if (narr != "" && narr != null)
                    //{
                    //    txt_narr.Text = Convert.ToString(narr);
                    //}
                    //else
                    //{
                    //    txt_narr.Text = "";
                    //}
                }
            }
            else
            {
                txt_venname.Text = "";
                txt_ccont.Text = "";
                txt_cadd.Text = "";
                txt_cdesi.Text = "";

            }
        }
    }

    //public void getvendorappno(string vendor_code)
    //{
    //    try
    //    {
    //        string date = "";
    //        string date1 = "";
    //        string instaldt = "";
    //        string instaldate = "";
    //        string dt = "";
    //        string mn = "";
    //        string yr = "";
    //        string dateonly = "";
    //        string tme = "";
    //        string amorpm = "";
    //        string fndt = "";
    //        date1 = txt_vdate.Text.ToString();
    //        string[] strdate1 = date1.Split('/');
    //        if (strdate1.Length > 0)
    //        {
    //            date = strdate1[0].ToString() + "/" + strdate1[1].ToString() + "/" + strdate1[2].ToString();
    //        }



    //        string selqry = d2.GetFunction(" select vendor_code from CO_VendorMaster where vendor_code='" + vendor_code + "'");
    //        string selqryinstdate = "select InstallmentDate,InstallmentAmt from FT_FinInstallmentPay where App_No='" + selqry + "'";
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(selqryinstdate, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                instaldt = ds.Tables[0].Rows[i][0].ToString();
    //                string[] insdatetime = instaldt.Split(' ');
    //                if (insdatetime.Length > 0)
    //                {
    //                    dateonly = insdatetime[0].ToString();
    //                    tme = insdatetime[1].ToString();
    //                    amorpm = insdatetime[2].ToString();
    //                }

    //                string[] instalftdt = dateonly.Split('/');
    //                if (instalftdt.Length > 0)
    //                {
    //                    dt = instalftdt[0].ToString();
    //                    mn = instalftdt[1].ToString();
    //                    yr = instalftdt[2].ToString();
    //                    instaldate = dt + "/" + mn + "/" + yr;
    //                    DateTime dtt = Convert.ToDateTime(instaldate);
    //                    fndt = dtt.ToString("dd/MM/yyyy");
    //                }
    //                if (date == fndt)
    //                {

    //                    string selqryamt = d2.GetFunction(" select SUM(InstallmentAmt) as amt,App_No from FT_FinInstallmentPay where InstallmentDate='" + instaldt + "'  group by InstallmentAmt ,App_No");
    //                    txt_ventotpayamt.Text = selqryamt;
    //                    txt_venpayamt.Text = selqryamt;
    //                }
    //            }
    //        }

    //    }
    //    catch
    //    {

    //    }
    //}
    //public void getvendorcode(string vendor_code)
    //{
    //    string selqry = "select vendor_name,vendor_code from vendor_details where vendor_code='" + vendor_code + "'";
    //    ds.Clear();
    //    ds = d2.select_method_wo_parameter(selqry, "Text");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {
    //            txt_venname.Text = ds.Tables[0].Rows[i]["vendor_name"].ToString();
    //            // txt_vencode.Text = ds.Tables[0].Rows[i]["vendor_code"].ToString();
    //        }
    //    }
    //    else
    //    {
    //        txt_venname.Text = "";
    //        // txt_vencode.Text = "";
    //    }
    //}

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static List<string> Getname4(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string selqry = "select AccNo+'-'+AccHolderName from FM_FinBankMaster where CollegeCode='" + clgcode + "' and AccNo like  '" + prefixText + "%' ";
        name = ws.Getname(selqry);
        return name;
    }
    public void txt_acno_OnTextChanged(object sender, EventArgs e)
    {
        string accno = "";
        string accname = Convert.ToString(txt_acno.Text);
        if (accname != "")
        {
            accno = accname.Split('-')[0];
            if (accno != "")
                getaccountnum(accno);
        }
        else
        {
            txt_acno.Text = "";
            txt_acname.Text = "";
            txt_bnkname.Text = "";
            txt_branch.Text = "";
            txtavailAmt.Text = "";
        }

    }
    public void getaccountnum(string accountno)
    {
        string selqry = "select distinct BankPK,BankCode,BankName,AccHolderName,AccNo,City from FM_FinBankMaster where CollegeCode='" + collegecode1 + "' and AccNo='" + accountno + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selqry, "Text");
        string bankfk = "";
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txt_acno.Text = ds.Tables[0].Rows[0]["AccNo"].ToString();
            txt_acname.Text = ds.Tables[0].Rows[0]["AccHolderName"].ToString();
            txt_bnkname.Text = ds.Tables[0].Rows[0]["BankName"].ToString();
            txt_branch.Text = ds.Tables[0].Rows[0]["City"].ToString();
            bankfk = Convert.ToString(ds.Tables[0].Rows[0]["BankPK"]);
            ViewState["BankPK"] = bankfk;
            double Amt = 0;
            double.TryParse(Convert.ToString(d2.GetFunction("select sum(isnull(debit,0)-isnull(credit,0)) as amount from FT_FinCashContraDet where isbank='1' and BankFK='" + bankfk + "' group by bankfk")), out Amt);
            txtavailAmt.Text = Convert.ToString(Amt);
        }
        else
        {
            txt_acno.Text = "";
            txt_acname.Text = "";
            txt_bnkname.Text = "";
            txt_branch.Text = "";
            txtavailAmt.Text = "";
        }
    }
    public void TextBox1_OnTextChanged(object sender, EventArgs e)
    {
        AddNewRowToGrid1();


    }
    public void bankgrid_pg_DataBound(object sender, EventArgs e)
    {

    }
    public void AddNewRowToGrid1()
    {

        int rowIndex = 0;
        Int32.TryParse(Convert.ToString(TextBox1.Text), out rowIndex);
        if (rowIndex > 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Sno");
            dt.Columns.Add("Date");
            dt.Columns.Add("Amount");
            DataRow dr;
            for (int row = 0; row < rowIndex; row++)
            {
                dr = dt.NewRow();
                dr[0] = Convert.ToString(row + 1);
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count > 0)
            {
                gridView2.DataSource = dt;
                gridView2.DataBind();
                divbank.Visible = true;
            }
            if (gridView2.Rows.Count > 0)
            {
                for (int r = 0; r < gridView2.Rows.Count; r++)
                {
                    (gridView2.Rows[r].FindControl("txtdate") as TextBox).Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtdate.Attributes.Add("readonly", "readonly");
                }
            }
        }
        else
        {
            divbank.Visible = false;
        }

    }
    public void chkinstall_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkinstall.Checked == true)
        {
            lbl_instal.Visible = true;
            TextBox1.Visible = true;
            TextBox1.Text = "";

        }
        else if (chkinstall.Checked == false)
        {
            lbl_instal.Visible = false;
            TextBox1.Visible = false;
            divbank.Visible = false;
        }
    }
    public void rb_petystd_OnCheckedChanged(object sender, EventArgs e)
    {

    }
    public void rb_handstd_OnCheckedChanged(object sendere, EventArgs e)
    {

    }
    public void rb_pety_OnCheckedChanged(object sender, EventArgs e)
    {
        lbl_er.Text = "";
    }
    public void rb_hand_OnCheckedChanged(object sendere, EventArgs e)
    {
        lbl_er.Text = "";
    }

    public void btnpayment_Click(object sender, EventArgs e)
    {
        try
        {
            #region getvalue
            string memTypeNo = string.Empty;
            string studappno = "";
            string name = "";
            string date = "";
            string time = "";
            string tmm = "";
            string fnltm = "";
            string datetime = Convert.ToString(txt_vdate.Text);
            string[] strdt = datetime.Split('/');
            if (strdt.Length > 0)
            {
                date = strdt[1] + "/" + strdt[0] + "/" + strdt[2];
                //time = strdt[1].ToString();
                //tmm = strdt[2].ToString();              
            }
            fnltm = DateTime.Now.ToShortTimeString();
            string TransCode = Convert.ToString(TextBox9.Text.ToString());

            int memtype = 0;
            if (rb_stud.Checked == true)
            {
                memtype = 1;
            }
            else if (rb_staff.Checked == true)
            {
                memtype = 2;
            }
            else if (rb_vendor.Checked == true)
            {
                memtype = 3;
            }
            else if (rb_others.Checked == true)
            {
                memtype = 4;
            }

            string appno = Convert.ToString(txt_rollno.Text.ToString());
            // string studappno = d2.GetFunction("select App_No  from Registration where Roll_No='" + appno + "'");
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
            {
                studappno = d2.GetFunction("select App_No  from Registration where Roll_No='" + appno + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
            {
                studappno = d2.GetFunction("select App_No  from Registration where reg_no='" + appno + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
            {
                studappno = d2.GetFunction("select App_No  from Registration where Roll_Admit='" + appno + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
            {
                studappno = d2.GetFunction(" select app_no from applyn where app_formno='" + appno + "'");
            }

            string applno = Convert.ToString(TextBox2.Text.ToString());
            string staffappno = d2.GetFunction("select sa.appl_id  from staffmaster s,staff_appl_master sa where s.appl_no =sa.appl_no and s.staff_code ='" + applno + "'");

            string vend_code = vendor_code;
            string vendcode = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + vend_code + "'");
            string crepayamt = txt_venpayamt.Text.ToString();

            string memname = "";
            if (rb_stud.Checked == true)
            {
                memname = "";
            }
            else if (rb_staff.Checked == true)
            {
                memname = "";
            }
            else if (rb_vendor.Checked == true)
            {
                memname = "";
            }
            else if (rb_others.Checked == true)
            {
                memname = TextBox6.Text.ToString();
            }


            int paymode = 0;
            if (rb_cash.Checked == true)
            {
                paymode = 1;
            }
            else if (rb_bank.Checked == true)
            {
                paymode = 2;
            }
            else if (rbonline.Checked == true)
            {
                paymode = 5;
            }

            string ddno = "";
            string dddate = "";
            string accno = txt_acno.Text.ToString();
            string bkcode = d2.GetFunction("select BankPK from FM_FinBankMaster where AccNo='" + accno + "'");
            string bankcode = "";
            string bankbranch = "";
            double adjustamt = 0;
            double adjustinstall = 0;
            if (rb_cash.Checked == true)
            {
                ddno = "";
                dddate = "";
                bankcode = Convert.ToString(0);
                bankbranch = "";
            }
            else if (rb_bank.Checked == true || rbonline.Checked == true)
            {
                ddno = Convert.ToString(txt_chqno.Text.ToString());
                string bankdatee = Convert.ToString(txt_cal.Text.ToString());
                string[] strdatet = bankdatee.Split('/');
                if (strdatet.Length > 0)
                {
                    string bkdt = strdatet[0].ToString();
                    string bkmn = strdatet[1].ToString();
                    string bkyr = strdatet[2].ToString();
                    dddate = bkmn + "/" + bkdt + "/" + bkyr;

                }
                bankcode = bkcode;
                bankbranch = Convert.ToString(txt_branch.Text.ToString());
            }
            string fincyr = d2.GetFunction("select LinkValue  from InsSettings where LinkName = 'Current Financial Year' and  college_code=" + ddl_collegename.SelectedItem.Value + "");
            string dt = "";
            string mn = "";
            int yr = 0;
            string curdatetime = DateTime.Now.ToString();
            string[] strdatetime = curdatetime.Split(' ');

            int pety = 0;
            int hand = 0;
            string petyname = "";
            if (rb_cash.Checked == true)
            {
                if (rb_pety.Checked == true)
                {
                    pety = 1;
                    hand = 0;
                    petyname = " ispetty=1";
                }
                else if (rb_hand.Checked == true)
                {
                    hand = 1;
                    pety = 0;
                    petyname = " ishand=1";
                }
            }


            string othname = Convert.ToString(TextBox6.Text);
            string comname = Convert.ToString(txt_othcname.Text);
            // string desi = Convert.ToString(TextBox7.Text);
            string add = Convert.ToString(txt_othadd.Text);
            string mblno = Convert.ToString(txt_mblno.Text);
            string identiy = "";
            for (int i = 0; i < ddl_group.Items.Count; i++)
            {
                if (ddl_group.Items[i].Selected == true)
                {
                    identiy = ddl_group.SelectedItem.Value.ToString();
                }
            }
            DataSet dsex = new DataSet();
            string identyno = TextBox8.Text.ToString();
            string narration = txt_narr.Text.ToString();
            string header = "";
            string ledger = "";
            int credit = 0;
            int debit = 0;
            int val = 0;
            string insqry = "";
            int save = 0;
            int save1 = 0;
            int fdt = 0;
            int fcbt = 0;
            double excessvalue = 0;
            string isdate = txt_vdate.Text.ToString();
            DateTime insdt1 = new DateTime();
            string insfnldate = "";
            string[] strisdate = isdate.Split('/');
            if (strisdate.Length > 0)
            {
                insfnldate = strisdate[1].ToString() + "/" + strisdate[0].ToString() + "/" + strisdate[2].ToString();
                insdt1 = Convert.ToDateTime(insfnldate);

            }

            string headerfk = "";
            string ledgerfk = "";
            double excessamt = 0;
            Boolean voucherinc = false;
            Boolean chequeinc = false;
            Boolean contamt = false;
            Boolean excessandrefund = false;
            string contrafk = "";
            string vendorvoucher = generateVendorCode();
            #endregion
            #region valuecheck
            string accountno = "";
            string accountname = "";
            string bankname = "";
            string branch = "";
            string bankamt = "";
            string checno = "";
            string totpayamt = "";
            string payamt = "";
            string heaname = "";
            string ledgname = "";
            string alltamt = "";
            string padamt = "";
            string baln = "";
            string tobpaid = "";
            string oridvalue = "";
            int gridtotamt = 0;
            string contraamt = "";
            string refundamt = "";
            string oldamt = "";
            Boolean paymentvalue = false;
            DataSet dscontra = new DataSet();
            string hedfk = "";
            string ledfk = "";
            string amount = "";
            string creditamt = "0";
            string vendorPk = "";
            string update = "";
            double fnlAmt = 0;
            double TxtAmt = 0;

            if (rb_cash.Checked == true)
            {
                contraamt = Convert.ToString(txtpayamt.Text);

                #region stud
                if (rb_stud.Checked == true)
                {
                    memTypeNo = studappno;
                    refundamt = d2.GetFunction("SELECT SUM(isnull(ExcessAmt,'0') -isnull(AdjAmt,'0')) as payamount FROM FT_ExcessDet WHERE App_No = '" + studappno + "' and ExcessType in('2','3')");
                    string studroll = Convert.ToString(txt_rollno.Text);
                    string studname = Convert.ToString(txt_name.Text);
                    totpayamt = Convert.ToString(txt_ventotpayamt.Text);
                    payamt = Convert.ToString(txt_venpayamt.Text);
                    if (studroll != "" && studname != "" && totpayamt != "" && payamt != "")
                    {
                        if (contraamt != "" && contraamt != null)
                        {
                            if (Convert.ToDouble(contraamt) >= Convert.ToDouble(payamt))
                            {
                                contamt = true;
                            }
                        }
                        if (gridView1.Rows.Count > 0)
                        {
                            for (int i = 0; i < gridView1.Rows.Count; i++)
                            {
                                DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                {
                                    gridtotamt = gridtotamt + Convert.ToInt32(valtxtpayamt.Text);
                                }
                            }
                            if (Convert.ToInt32(payamt) == gridtotamt)
                            {
                                paymentvalue = true;
                            }
                            else
                            {
                                if (gridView2.Rows.Count > 0)
                                {
                                    for (int sel = 0; sel < gridView2.Rows.Count; sel++)
                                    {
                                        TextBox txtamt = (TextBox)gridView2.Rows[sel].FindControl("txtamt");
                                        double.TryParse(Convert.ToString(txtamt.Text), out TxtAmt);
                                        fnlAmt += TxtAmt;
                                    }
                                    if (Convert.ToInt32(payamt) == Convert.ToInt32(fnlAmt + gridtotamt))
                                        paymentvalue = true;
                                }
                            }
                        }

                    }
                }
                #endregion
                #region staff
                else if (rb_staff.Checked == true)
                {
                    memTypeNo = staffappno;
                    string stafcode = Convert.ToString(TextBox2.Text);
                    string stafname = Convert.ToString(TextBox3.Text);
                    totpayamt = Convert.ToString(txt_ventotpayamt.Text);
                    payamt = Convert.ToString(txt_venpayamt.Text);
                    if (stafname != "" && stafcode != "" && totpayamt != "" && payamt != "")
                    {
                        // contraamt = d2.GetFunction("select sum(isnull(debit,0)-isnull(credit,0)) as  amount from FT_FinCashContraDet where ispetty=1");
                        if (contraamt != "" && contraamt != null)
                        {
                            if (Convert.ToDouble(contraamt) >= Convert.ToDouble(payamt))
                            {
                                contamt = true;
                            }
                        }
                        #region without allot
                        if (!cbStaff.Checked && gridView1.Rows.Count > 0)
                        {
                            for (int i = 0; i < gridView1.Rows.Count; i++)
                            {

                                DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                {
                                    gridtotamt = gridtotamt + Convert.ToInt32(valtxtpayamt.Text);
                                }
                            }
                        }
                        #endregion
                        #region with allot
                        if (cbStaff.Checked && gdPaid.Rows.Count > 0)
                        {
                            for (int i = 0; i < gdPaid.Rows.Count; i++)
                            {
                                Label lblhdStr = (Label)gdPaid.Rows[i].FindControl("lblhdName");
                                Label lblldStr = (Label)gdPaid.Rows[i].FindControl("lblldName");
                                Label lblhd = (Label)gdPaid.Rows[i].FindControl("lblhdFk");
                                Label lblld = (Label)gdPaid.Rows[i].FindControl("lblldFk");
                                TextBox valtxtpayamt = (TextBox)gdPaid.Rows[i].FindControl("txtAmt");
                                if (lblhd.Text.Trim() != "Select" && lblld.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                {
                                    gridtotamt = gridtotamt + Convert.ToInt32(valtxtpayamt.Text);
                                }
                            }
                        }
                        #endregion
                        if (Convert.ToInt32(payamt) == gridtotamt)
                        {
                            paymentvalue = true;
                        }
                        else
                        {
                            if (gridView2.Rows.Count > 0)
                            {
                                for (int sel = 0; sel < gridView2.Rows.Count; sel++)
                                {
                                    TextBox txtamt = (TextBox)gridView2.Rows[sel].FindControl("txtamt");
                                    double.TryParse(Convert.ToString(txtamt.Text), out TxtAmt);
                                    fnlAmt += TxtAmt;
                                }
                                if (Convert.ToInt32(payamt) == Convert.ToInt32(fnlAmt + gridtotamt))
                                    paymentvalue = true;
                            }
                        }

                    }
                }
                #endregion
                #region vendor
                else if (rb_vendor.Checked == true)
                {
                    memTypeNo = vendcode;
                    string compname = Convert.ToString(txt_venname.Text);
                    string contper = Convert.ToString(txt_ccont.Text);
                    if (compname != "" && contper != "")
                    {
                        if (compname != "" && contper != "")
                        {
                            if (gridven.Rows.Count > 0)
                            {
                                for (int i = 0; i < gridven.Rows.Count; i++)
                                {
                                    DropDownList headname = (DropDownList)gridven.Rows[i].FindControl("ddlheader");
                                    DropDownList ledname = (DropDownList)gridven.Rows[i].FindControl("ddlledger");
                                    TextBox allotamt = (TextBox)gridven.Rows[i].FindControl("txttotamt");
                                    TextBox paidamt = (TextBox)gridven.Rows[i].FindControl("txtpaidamt");
                                    TextBox bal = (TextBox)gridven.Rows[i].FindControl("txtbal");
                                    TextBox tobepaid = (TextBox)gridven.Rows[i].FindControl("txttobepaid");
                                    TextBox orderid = (TextBox)gridven.Rows[i].FindControl("txtorderid");
                                    heaname = headname.SelectedItem.Value.ToString();
                                    ledgname = ledname.SelectedItem.Value.ToString();
                                    alltamt = Convert.ToString(allotamt.Text);
                                    padamt = Convert.ToString(paidamt.Text);
                                    baln = Convert.ToString(bal.Text);
                                    tobpaid = Convert.ToString(tobepaid.Text);
                                    payamt = Convert.ToString(tobpaid);
                                    oridvalue = Convert.ToString(orderid.Text);
                                    if (heaname != "" && ledgname != "" && alltamt != "" && baln != "" && tobpaid != "" && oridvalue != "")
                                    {
                                        gridtotamt = gridtotamt + Convert.ToInt32(tobpaid);
                                    }
                                    //  contraamt = d2.GetFunction("select sum(isnull(debit,0)-isnull(credit,0)) as  amount from FT_FinCashContraDet where ispetty=1");

                                    if (contraamt != "" && contraamt != null)
                                    {
                                        if (Convert.ToDouble(contraamt) >= Convert.ToDouble(payamt))
                                        {
                                            contamt = true;
                                        }
                                    }
                                }
                                if (Convert.ToInt32(payamt) == gridtotamt)
                                {
                                    paymentvalue = true;
                                }
                                else
                                {
                                    if (gridView2.Rows.Count > 0)
                                    {
                                        for (int sel = 0; sel < gridView2.Rows.Count; sel++)
                                        {
                                            TextBox txtamt = (TextBox)gridView2.Rows[sel].FindControl("txtamt");
                                            double.TryParse(Convert.ToString(txtamt.Text), out TxtAmt);
                                            fnlAmt += TxtAmt;
                                        }
                                        if (Convert.ToInt32(payamt) == Convert.ToInt32(fnlAmt + gridtotamt))
                                            paymentvalue = true;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region others
                else if (rb_others.Checked == true)
                {
                    string othername = Convert.ToString(TextBox6.Text);
                    string othercomp = Convert.ToString(txt_othcname.Text);
                    totpayamt = Convert.ToString(txt_ventotpayamt.Text);
                    payamt = Convert.ToString(txt_venpayamt.Text);
                    if (othername != "" && othercomp != "" && totpayamt != "" && payamt != "")
                    {
                        //contraamt = d2.GetFunction("select sum(isnull(debit,0)-isnull(credit,0))as  amount from FT_FinCashContraDet where ispetty=1");
                        if (contraamt != "" && contraamt != null)
                        {
                            if (Convert.ToDouble(contraamt) >= Convert.ToDouble(payamt))
                            {
                                contamt = true;
                            }
                        }
                        if (gridView1.Rows.Count > 0)
                        {
                            for (int i = 0; i < gridView1.Rows.Count; i++)
                            {

                                DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                {
                                    gridtotamt = gridtotamt + Convert.ToInt32(valtxtpayamt.Text);
                                }
                            }
                            if (Convert.ToInt32(payamt) == gridtotamt)
                            {
                                paymentvalue = true;
                            }
                            else
                            {
                                if (gridView2.Rows.Count > 0)
                                {
                                    for (int sel = 0; sel < gridView2.Rows.Count; sel++)
                                    {
                                        TextBox txtamt = (TextBox)gridView2.Rows[sel].FindControl("txtamt");
                                        double.TryParse(Convert.ToString(txtamt.Text), out TxtAmt);
                                        fnlAmt += TxtAmt;
                                    }
                                    if (Convert.ToInt32(payamt) == Convert.ToInt32(fnlAmt + gridtotamt))
                                        paymentvalue = true;
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            else if (rb_bank.Checked == true || rbonline.Checked == true)
            {
                //  contraamt = d2.GetFunction("  select sum(isnull(debit,0)-isnull(credit,0)) as amount from FT_FinCashContraDet where isbank='1' and BankFK='" + bkcode + "'  group by bankfk");
                contraamt = Convert.ToString(txtavailAmt.Text);
                #region stud
                if (rb_stud.Checked == true)
                {
                    memTypeNo = studappno;
                    refundamt = d2.GetFunction("SELECT SUM(isnull(ExcessAmt,'0') -isnull(AdjAmt,'0') as payamount FROM FT_ExcessDet WHERE App_No = '" + studappno + "' and ExcessType in('2','3')");
                    string studroll = Convert.ToString(txt_rollno.Text);
                    string studname = Convert.ToString(txt_name.Text);
                    totpayamt = Convert.ToString(txt_ventotpayamt.Text);
                    payamt = Convert.ToString(txt_venpayamt.Text);
                    accountno = Convert.ToString(txt_acno.Text);
                    accountname = Convert.ToString(txt_acname.Text);
                    bankamt = Convert.ToString(txt_amt.Text);
                    checno = Convert.ToString(txt_chqno.Text);

                    if (studroll != "" && studname != "" && accountno != "" && accountname != "" && bankamt != "" && checno != "")
                    {

                        if (contraamt != "" && contraamt != null)
                        {
                            if (Convert.ToDouble(contraamt) >= Convert.ToDouble(payamt))
                            {
                                contamt = true;
                            }
                        }
                        if (gridView1.Rows.Count > 0)
                        {
                            for (int i = 0; i < gridView1.Rows.Count; i++)
                            {

                                DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                {
                                    gridtotamt = gridtotamt + Convert.ToInt32(valtxtpayamt.Text);
                                }
                            }
                            if (Convert.ToInt32(payamt) == gridtotamt)
                            {
                                paymentvalue = true;
                            }
                            else
                            {
                                if (gridView2.Rows.Count > 0)
                                {
                                    for (int sel = 0; sel < gridView2.Rows.Count; sel++)
                                    {
                                        TextBox txtamt = (TextBox)gridView2.Rows[sel].FindControl("txtamt");
                                        double.TryParse(Convert.ToString(txtamt.Text), out TxtAmt);
                                        fnlAmt += TxtAmt;
                                    }
                                    if (Convert.ToInt32(payamt) == Convert.ToInt32(fnlAmt + gridtotamt))
                                        paymentvalue = true;
                                }
                            }
                        }
                    }
                }
                #endregion
                #region staff
                else if (rb_staff.Checked == true)
                {
                    memTypeNo = staffappno;
                    string stafcode = Convert.ToString(TextBox2.Text);
                    string stafname = Convert.ToString(TextBox3.Text);
                    totpayamt = Convert.ToString(txt_ventotpayamt.Text);
                    payamt = Convert.ToString(txt_venpayamt.Text);
                    accountno = Convert.ToString(txt_acno.Text);
                    accountname = Convert.ToString(txt_acname.Text);
                    bankamt = Convert.ToString(txt_amt.Text);
                    checno = Convert.ToString(txt_chqno.Text);

                    if (stafname != "" && stafcode != "" && totpayamt != "" && payamt != "" && accountno != "" && accountname != "" && bankamt != "" && checno != "")
                    {
                        //contraamt = d2.GetFunction("  select sum(isnull(debit,0)-isnull(credit,0)) as amount from FT_FinCashContraDet where isbank='" + bkcode + "'  group by bankfk");
                        if (contraamt != "" && contraamt != null)
                        {
                            if (Convert.ToDouble(contraamt) >= Convert.ToDouble(payamt))
                            {
                                contamt = true;
                            }
                        }
                        #region without allot
                        if (!cbStaff.Checked && gridView1.Rows.Count > 0)
                        {
                            for (int i = 0; i < gridView1.Rows.Count; i++)
                            {

                                DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                {
                                    gridtotamt = gridtotamt + Convert.ToInt32(valtxtpayamt.Text);
                                }
                            }
                        }
                        #endregion
                        #region with allot

                        if (cbStaff.Checked && gdPaid.Rows.Count > 0)
                        {
                            for (int i = 0; i < gdPaid.Rows.Count; i++)
                            {
                                Label lblhdStr = (Label)gdPaid.Rows[i].FindControl("lblhdName");
                                Label lblldStr = (Label)gdPaid.Rows[i].FindControl("lblldName");
                                Label lblhd = (Label)gdPaid.Rows[i].FindControl("lblhdFk");
                                Label lblld = (Label)gdPaid.Rows[i].FindControl("lblldFk");
                                TextBox valtxtpayamt = (TextBox)gdPaid.Rows[i].FindControl("txtAmt");
                                if (lblhd.Text.Trim() != "Select" && lblld.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                {
                                    gridtotamt = gridtotamt + Convert.ToInt32(valtxtpayamt.Text);
                                }
                            }
                        }

                        #endregion


                        if (Convert.ToInt32(payamt) == gridtotamt)
                        {
                            paymentvalue = true;
                        }
                        else
                        {
                            if (gridView2.Rows.Count > 0)
                            {
                                for (int sel = 0; sel < gridView2.Rows.Count; sel++)
                                {
                                    TextBox txtamt = (TextBox)gridView2.Rows[sel].FindControl("txtamt");
                                    double.TryParse(Convert.ToString(txtamt.Text), out TxtAmt);
                                    fnlAmt += TxtAmt;
                                }
                                if (Convert.ToInt32(payamt) == Convert.ToInt32(fnlAmt + gridtotamt))
                                    paymentvalue = true;
                            }
                        }

                    }
                }
                #endregion
                #region vendor
                else if (rb_vendor.Checked == true)
                {
                    memTypeNo = vendcode;
                    string compname = Convert.ToString(txt_venname.Text);
                    string contper = Convert.ToString(txt_ccont.Text);
                    accountno = Convert.ToString(txt_acno.Text);
                    accountname = Convert.ToString(txt_acname.Text);
                    bankamt = Convert.ToString(txt_amt.Text);
                    checno = Convert.ToString(txt_chqno.Text);

                    if (compname != "" && contper != "" && accountno != "" && accountname != "" && bankamt != "" && checno != "")
                    {
                        if (gridven.Rows.Count > 0)
                        {
                            for (int i = 0; i < gridven.Rows.Count; i++)
                            {
                                DropDownList headname = (DropDownList)gridven.Rows[i].FindControl("ddlheader");
                                DropDownList ledname = (DropDownList)gridven.Rows[i].FindControl("ddlledger");
                                TextBox allotamt = (TextBox)gridven.Rows[i].FindControl("txttotamt");
                                TextBox paidamt = (TextBox)gridven.Rows[i].FindControl("txtpaidamt");
                                TextBox bal = (TextBox)gridven.Rows[i].FindControl("txtbal");
                                TextBox tobepaid = (TextBox)gridven.Rows[i].FindControl("txttobepaid");
                                TextBox orderid = (TextBox)gridven.Rows[i].FindControl("txtorderid");
                                heaname = headname.SelectedItem.Value.ToString();
                                ledgname = ledname.SelectedItem.Value.ToString();
                                alltamt = Convert.ToString(allotamt.Text);
                                padamt = Convert.ToString(paidamt.Text);
                                baln = Convert.ToString(bal.Text);
                                tobpaid = Convert.ToString(tobepaid.Text);
                                payamt = Convert.ToString(tobpaid);
                                oridvalue = Convert.ToString(orderid.Text);
                                if (heaname != "" && ledgname != "" && alltamt != "" && baln != "" && tobpaid != "" && oridvalue != "")
                                {
                                    gridtotamt = gridtotamt + Convert.ToInt32(tobpaid);
                                }
                                // contraamt = d2.GetFunction("  select sum(isnull(debit,0)-isnull(credit,0)) as amount from FT_FinCashContraDet where isbank='" + bkcode + "'  group by bankfk");
                                if (contraamt != "" && contraamt != null)
                                {
                                    if (Convert.ToDouble(contraamt) >= Convert.ToDouble(payamt))
                                    {
                                        contamt = true;
                                    }
                                }
                            }
                            if (Convert.ToInt32(payamt) == gridtotamt)
                            {
                                paymentvalue = true;
                            }
                            else
                            {
                                if (gridView2.Rows.Count > 0)
                                {
                                    for (int sel = 0; sel < gridView2.Rows.Count; sel++)
                                    {
                                        TextBox txtamt = (TextBox)gridView2.Rows[sel].FindControl("txtamt");
                                        double.TryParse(Convert.ToString(txtamt.Text), out TxtAmt);
                                        fnlAmt += TxtAmt;
                                    }
                                    if (Convert.ToInt32(payamt) == Convert.ToInt32(fnlAmt + gridtotamt))
                                        paymentvalue = true;
                                }
                            }
                        }
                    }
                }
                #endregion
                #region others
                else if (rb_others.Checked == true)
                {
                    string othername = Convert.ToString(TextBox6.Text);
                    string othercomp = Convert.ToString(txt_othcname.Text);
                    totpayamt = Convert.ToString(txt_ventotpayamt.Text);
                    payamt = Convert.ToString(txt_venpayamt.Text);
                    accountno = Convert.ToString(txt_acno.Text);
                    accountname = Convert.ToString(txt_acname.Text);
                    bankamt = Convert.ToString(txt_amt.Text);
                    checno = Convert.ToString(txt_chqno.Text);
                    if (othername != "" && othercomp != "" && totpayamt != "" && payamt != "" && accountno != "" && accountname != "" && bankamt != "" && checno != "")
                    {
                        // contraamt = d2.GetFunction("  select sum(isnull(debit,0)-isnull(credit,0)) as amount from FT_FinCashContraDet where isbank='" + bkcode + "'  group by bankfk");
                        if (contraamt != "" && contraamt != null)
                        {
                            if (Convert.ToDouble(contraamt) >= Convert.ToDouble(payamt))
                            {
                                contamt = true;
                            }
                        }
                        if (gridView1.Rows.Count > 0)
                        {
                            for (int i = 0; i < gridView1.Rows.Count; i++)
                            {

                                DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                {
                                    gridtotamt = gridtotamt + Convert.ToInt32(valtxtpayamt.Text);

                                }
                            }
                            if (Convert.ToInt32(payamt) == gridtotamt)
                            {
                                paymentvalue = true;
                            }
                            else
                            {
                                if (gridView2.Rows.Count > 0)
                                {
                                    for (int sel = 0; sel < gridView2.Rows.Count; sel++)
                                    {
                                        TextBox txtamt = (TextBox)gridView2.Rows[sel].FindControl("txtamt");
                                        double.TryParse(Convert.ToString(txtamt.Text), out TxtAmt);
                                        fnlAmt += TxtAmt;
                                    }
                                    if (Convert.ToInt32(payamt) == Convert.ToInt32(fnlAmt + gridtotamt))
                                        paymentvalue = true;
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region btnsave
            DataTable dtvocucher = new DataTable();
            dtvocucher.Columns.Add("Sno");
            dtvocucher.Columns.Add("Header");
            dtvocucher.Columns.Add("Ledger");
            dtvocucher.Columns.Add("Amount");
            if (contamt == true)
            {
                if (paymentvalue == true)
                {
                    if (rb_cash.Checked == true)
                    {
                        #region rb_stud
                        if (rb_stud.Checked == true)
                        {
                            name = Convert.ToString(txt_name.Text);
                            DropDownList ddl_credit = new DropDownList();
                            DropDownList ddl_debit = new DropDownList();
                            TextBox txtpaymt = new TextBox();
                            if (gridView1.Rows.Count > 0)
                            {
                                for (int i = 0; i < gridView1.Rows.Count; i++)
                                {

                                    DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                    DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                    TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                    if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                    {
                                        header = strheader.SelectedItem.Value.ToString();
                                        ledger = strledger.SelectedItem.Value.ToString();
                                        credit = Convert.ToInt32(valtxtpayamt.Text.ToString());
                                        adjustamt = adjustamt + credit;
                                        double deductamount;
                                        double balamount = 0;
                                        double.TryParse(Convert.ToString(txt_dtsamt.Text), out deductamount);
                                        if (rb_percentage.Checked == true)
                                        {
                                            deductamount = (credit * deductamount / 100);
                                            balamount = credit - deductamount;
                                        }
                                        if (rb_amount.Checked == true)
                                        {

                                            balamount = credit - deductamount;
                                        }
                                        if (rb_percentage.Checked == true || rb_amount.Checked == true)
                                        {
                                            for (int j = 0; j < gridView3.Rows.Count; j++)
                                            {

                                                ddl_credit = (DropDownList)gridView3.Rows[j].FindControl("ddl_credit");
                                                ddl_debit = (DropDownList)gridView3.Rows[j].FindControl("ddl_debit");
                                                txtpaymt = (TextBox)gridView3.Rows[j].FindControl("txtpaymt");

                                                ledger = ddl_credit.SelectedItem.Value;

                                                insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + 0 + "','" + txtpaymt.Text.Trim() + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                                save = d2.update_method_wo_parameter(insqry, "Text");
                                                ledger = ddl_debit.SelectedItem.Value;
                                                insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + txtpaymt.Text.Trim() + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                                save = d2.update_method_wo_parameter(insqry, "Text");
                                            }
                                        }

                                        ledger = strledger.SelectedItem.Value.ToString();
                                        insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + balamount + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";

                                        save = d2.update_method_wo_parameter(insqry, "Text");
                                        fdt++;
                                        voucherinc = true;

                                        if (voucherinc == true || cbClgFormat.Checked)
                                        {
                                            dtvocucher = bindVoucherLedgerDet(dtvocucher, strheader.SelectedItem.Text, strledger.SelectedItem.Text, credit);
                                        }
                                    }
                                }


                            }
                            if (Convert.ToInt32(payamt) != 0)
                            {
                                contraamt = "select SUM(ISNULL(debit,0)-isnull(credit,0)) as amount,headerfk,ledgerfk,FinCashContraDetPK from FT_FinCashContraDet  where IsPetty=1 group by headerfk,ledgerfk,FinCashContraDetPK";
                                dscontra.Clear();
                                dscontra = d2.select_method_wo_parameter(contraamt, "Text");
                                double payamount = 0;
                                if (dscontra.Tables.Count > 0)
                                {
                                    if (dscontra.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dscontra.Tables[0].Rows.Count; i++)
                                        {
                                            amount = Convert.ToString(dscontra.Tables[0].Rows[i]["amount"]);
                                            hedfk = Convert.ToString(dscontra.Tables[0].Rows[i]["headerfk"]);
                                            ledfk = Convert.ToString(dscontra.Tables[0].Rows[i]["ledgerfk"]);
                                            contrafk = Convert.ToString(dscontra.Tables[0].Rows[i]["FinCashContraDetPK"]);
                                            string ttamt = payamt;
                                            if (Convert.ToDouble(ttamt) <= Convert.ToDouble(amount))
                                            {
                                                creditamt = ttamt;
                                                ttamt = "0";
                                            }
                                            else
                                            {
                                                creditamt = amount;
                                                payamount = Convert.ToDouble(ttamt) - Convert.ToDouble(amount);
                                                ttamt = Convert.ToString(payamount);
                                            }

                                            if (creditamt != "0")
                                            {
                                                string insert = "";
                                                insert = " update FT_FinCashContraDet set Credit=isnull(Credit,0)+'" + creditamt + "',IsHand='" + hand + "',IsPetty='" + pety + "',TransDate='" + date + "',TransTime='" + fnltm + "',Narration='" + narration + "' where  IsPetty='" + pety + "'  and LedgerFK='" + ddlapyledg.SelectedItem.Value + "' and FinYearFK='" + fincyr + "'  and FinCashContraDetPK='" + contrafk + "'";
                                                save1 = d2.update_method_wo_parameter(insert, "Text");
                                                fcbt++;
                                            }
                                        }
                                    }
                                }

                            }
                            if (studappno != "")
                            {
                                string upda = "update FT_FinInstallmentPay set InstallmentStatus='1' where App_No ='" + studappno + "' and InstallmentDate ='" + insdt1.ToString("MM/dd/yyyy") + "'";
                                save1 = d2.update_method_wo_parameter(upda, "Text");
                            }
                            int instalvalue = 0;
                            if (chkinstall.Checked == true)
                            {
                                for (int j = 0; j < gridView2.Rows.Count; j++)
                                {
                                    TextBox txtdate = (TextBox)gridView2.Rows[j].FindControl("txtdate");
                                    TextBox txtamt = (TextBox)gridView2.Rows[j].FindControl("txtamt");
                                    instalvalue = Convert.ToInt32(txtamt.Text.ToString());
                                    adjustinstall = adjustinstall + instalvalue;
                                    if (txtdate.Text.Trim() != "" && txtamt.Text.Trim() != "")
                                    {
                                        string instamt = "";
                                        string insdt = "";
                                        string instdt = "";
                                        string instmn = "";
                                        string instyr = "";
                                        string fninstdt = "";
                                        insdt = txtdate.Text.ToString();
                                        string[] strtxtdt = insdt.Split('/');
                                        if (strtxtdt.Length > 0)
                                        {
                                            instdt = strtxtdt[0].ToString();
                                            instmn = strtxtdt[1].ToString();
                                            instyr = strtxtdt[2].ToString();

                                            fninstdt = instmn + "/" + instdt + "/" + instyr;
                                        }

                                        instamt = txtamt.Text.ToString();
                                        string installqry = "insert into FT_FinInstallmentPay(memtype,App_No,MemName,InstallmentDate,InstallmentAmt,InstallmentStatus)values('" + memtype + "','" + studappno + "','" + memname + "','" + fninstdt + "','" + instamt + "','0')";
                                        int install = d2.update_method_wo_parameter(installqry, "Text");
                                    }
                                }
                            }

                            #region Excess update

                            int updatex = 0;
                            string excessdetpk = "";
                            double oldAmt = 0;

                            if (chk_insexcess.Checked == true)
                            {
                                if (adjustamt > 0 && adjustamt != null)
                                {
                                    excessvalue = adjustamt + adjustinstall;
                                    oldamt = d2.GetFunction("select isnull(ExcessAmt,0)-isnull(AdjAmt,0) from FT_ExcessDet where App_No='" + studappno + "' and ExcessType='1'");
                                    double.TryParse(oldamt, out oldAmt);
                                    if (oldAmt >= excessvalue)
                                    {
                                        #region
                                        update = "update FT_ExcessDet set AdjAmt=ISNULL(AdjAmt,'0') +'" + excessvalue + "' where App_No='" + studappno + "' and ExcessType=1";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");

                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=1");
                                        string select = "select (ISNULL(ExcessAmt,'0')-ISNULL(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt >= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =ISNULL(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        adjustamt = excessvalue;
                                                    }
                                                    if (excessamt <= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =ISNULL(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        adjustamt = excessvalue;
                                                    }
                                                }

                                            }
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region
                                        double balAmt = 0;
                                        balAmt = excessvalue - oldAmt;
                                        adjustamt = balAmt;
                                        balAmt = 0;
                                        update = "update FT_ExcessDet set AdjAmt=ISNULL(AdjAmt,'0') +'" + oldAmt + "' where App_No='" + studappno + "' and ExcessType=1";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");
                                        excessvalue = oldAmt;

                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=1");
                                        string select = "select (ISNULL(ExcessAmt,'0')-ISNULL(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt >= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =ISNULL(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        balAmt = excessvalue;
                                                    }
                                                    if (excessamt <= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =ISNULL(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        balAmt = excessvalue;
                                                    }
                                                }

                                            }
                                            adjustamt += balAmt;
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region Refunds Update

                            #region Type 2
                            if (refundamt != "" && refundamt != null)
                            {
                                if (adjustamt > 0 && adjustamt != null)
                                {
                                    excessvalue = adjustamt + adjustinstall;
                                    oldamt = d2.GetFunction("select isnull(ExcessAmt,0)-isnull(AdjAmt,0) from FT_ExcessDet where App_No='" + studappno + "' and ExcessType='2'");
                                    double.TryParse(oldamt, out oldAmt);
                                    if (oldAmt >= excessvalue)
                                    {
                                        #region
                                        update = "update FT_ExcessDet set AdjAmt=isnull(AdjAmt,'0')+'" + excessvalue + "' where App_No='" + studappno + "' and ExcessType=2";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");

                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=2");
                                        string select = "select (isnull(ExcessAmt,'0')-isnull(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt > excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        adjustamt = 0;
                                                    }
                                                    else if (excessamt < excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        adjustamt = excessvalue;
                                                    }
                                                }

                                            }
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region
                                        double balAmt = 0;
                                        balAmt = excessvalue - oldAmt;
                                        adjustamt = balAmt;
                                        balAmt = 0;
                                        update = "update FT_ExcessDet set AdjAmt=isnull(AdjAmt,'0')+'" + oldAmt + "' where App_No='" + studappno + "' and ExcessType=2";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");
                                        excessvalue = oldAmt;
                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=2");
                                        string select = "select (isnull(ExcessAmt,'0')-isnull(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt > excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        balAmt = excessvalue;
                                                    }
                                                    else if (excessamt < excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        balAmt = excessvalue;
                                                    }
                                                }

                                            }
                                            adjustamt += balAmt;
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }

                                }
                            }
                            #endregion

                            #region Type 3
                            if (chk_deposite.Checked == true && refundamt != "" && refundamt != null)
                            {
                                if (adjustamt > 0 && adjustamt != null)
                                {
                                    excessvalue = adjustamt + adjustinstall;
                                    oldamt = d2.GetFunction("select isnull(ExcessAmt,0)-isnull(AdjAmt,0) from FT_ExcessDet where App_No='" + studappno + "' and ExcessType='3'");
                                    double.TryParse(oldamt, out oldAmt);
                                    if (oldAmt >= excessvalue)
                                    {
                                        #region
                                        update = "update FT_ExcessDet set AdjAmt=isnull(AdjAmt,'0')+'" + excessvalue + "' where App_No='" + studappno + "' and ExcessType=3";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");

                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=3");
                                        string select = "select (isnull(ExcessAmt,'0')-isnull(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt >= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        adjustamt = 0;
                                                    }
                                                    else if (excessamt <= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        adjustamt = excessvalue;
                                                    }
                                                }

                                            }
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region
                                        double balAmt = 0;
                                        balAmt = excessvalue - oldAmt;
                                        adjustamt = balAmt;
                                        balAmt = 0;
                                        update = "update FT_ExcessDet set AdjAmt=isnull(AdjAmt,'0')+'" + oldAmt + "' where App_No='" + studappno + "' and ExcessType=3";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");
                                        excessvalue = oldAmt;
                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=3");
                                        string select = "select (isnull(ExcessAmt,'0')-isnull(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt >= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        balAmt = excessvalue;
                                                    }
                                                    else if (excessamt <= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        balAmt = excessvalue;
                                                    }
                                                }

                                            }
                                            adjustamt += balAmt;
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }

                                }
                            }
                            #endregion

                            #endregion

                        }
                        #endregion
                        #region rb_staff
                        else if (rb_staff.Checked == true)
                        {
                            name = Convert.ToString(TextBox3.Text);
                            DropDownList ddl_credit = new DropDownList();
                            DropDownList ddl_debit = new DropDownList();
                            TextBox txtpaymt = new TextBox();
                            #region without allot
                            if (!cbStaff.Checked && gridView1.Rows.Count > 0)
                            {
                                for (int i = 0; i < gridView1.Rows.Count; i++)
                                {

                                    DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                    DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                    TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                    if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                    {
                                        header = strheader.SelectedItem.Value.ToString();
                                        ledger = strledger.SelectedItem.Value.ToString();
                                        credit = Convert.ToInt32(valtxtpayamt.Text.ToString());
                                        double deductamount;
                                        double balamount = 0;
                                        double.TryParse(Convert.ToString(txt_dtsamt.Text), out deductamount);
                                        if (rb_percentage.Checked == true)
                                        {
                                            deductamount = (credit * deductamount / 100);
                                            balamount = credit - deductamount;
                                        }
                                        if (rb_amount.Checked == true)
                                        {

                                            balamount = credit - deductamount;
                                        }
                                        if (rb_percentage.Checked == true || rb_amount.Checked == true)
                                        {
                                            for (int j = 0; j < gridView3.Rows.Count; j++)
                                            {

                                                ddl_credit = (DropDownList)gridView3.Rows[j].FindControl("ddl_credit");
                                                ddl_debit = (DropDownList)gridView3.Rows[j].FindControl("ddl_debit");
                                                txtpaymt = (TextBox)gridView3.Rows[j].FindControl("txtpaymt");

                                                ledger = ddl_credit.SelectedItem.Value;
                                                insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + staffappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + 0 + "','" + txtpaymt.Text.Trim() + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','','','0','','1','','','0','','','0','','','0','','','','0')";


                                                save = d2.update_method_wo_parameter(insqry, "Text");
                                                ledger = ddl_debit.SelectedItem.Value;
                                                insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + staffappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + txtpaymt.Text.Trim() + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                                save = d2.update_method_wo_parameter(insqry, "Text");
                                            }
                                        }
                                        ledger = strledger.SelectedItem.Value.ToString();
                                        insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + staffappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + balamount + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";

                                        save = d2.update_method_wo_parameter(insqry, "Text");
                                        fdt++;
                                        voucherinc = true;
                                        if (voucherinc == true || cbClgFormat.Checked)
                                        {
                                            dtvocucher = bindVoucherLedgerDet(dtvocucher, strheader.SelectedItem.Text, strledger.SelectedItem.Text, credit);
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region with allot
                            if (cbStaff.Checked && gdPaid.Rows.Count > 0)
                            {
                                for (int i = 0; i < gdPaid.Rows.Count; i++)
                                {

                                    Label lblhdStr = (Label)gdPaid.Rows[i].FindControl("lblhdName");
                                    Label lblldStr = (Label)gdPaid.Rows[i].FindControl("lblldName");
                                    Label lblhd = (Label)gdPaid.Rows[i].FindControl("lblhdFk");
                                    Label lblld = (Label)gdPaid.Rows[i].FindControl("lblldFk");
                                    TextBox valtxtpayamt = (TextBox)gdPaid.Rows[i].FindControl("txtAmt");
                                    if (lblhd.Text.Trim() != "" && lblld.Text.Trim() != "" && valtxtpayamt.Text.Trim() != "")
                                    {
                                        header = lblhd.Text.ToString();
                                        ledger = lblld.Text.ToString();
                                        credit = Convert.ToInt32(valtxtpayamt.Text.ToString());
                                        double deductamount;
                                        double balamount = 0;
                                        double.TryParse(Convert.ToString(txt_dtsamt.Text), out deductamount);
                                        if (rb_percentage.Checked == true)
                                        {
                                            deductamount = (credit * deductamount / 100);
                                            balamount = credit - deductamount;
                                        }
                                        if (rb_amount.Checked == true)
                                        {

                                            balamount = credit - deductamount;
                                        }
                                        if (rb_percentage.Checked == true || rb_amount.Checked == true)
                                        {
                                            for (int j = 0; j < gridView3.Rows.Count; j++)
                                            {

                                                ddl_credit = (DropDownList)gridView3.Rows[j].FindControl("ddl_credit");
                                                ddl_debit = (DropDownList)gridView3.Rows[j].FindControl("ddl_debit");
                                                txtpaymt = (TextBox)gridView3.Rows[j].FindControl("txtpaymt");

                                                ledger = ddl_credit.SelectedItem.Value;
                                                insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + staffappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + 0 + "','" + txtpaymt.Text.Trim() + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                                save = d2.update_method_wo_parameter(insqry, "Text");
                                                ledger = ddl_debit.SelectedItem.Value;
                                                insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + staffappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + txtpaymt.Text.Trim() + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                                save = d2.update_method_wo_parameter(insqry, "Text");
                                            }
                                        }
                                        ledger = lblld.Text.ToString();
                                        insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + staffappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + balamount + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";

                                        save = d2.update_method_wo_parameter(insqry, "Text");

                                        string strAlt = " update ft_feeallot set paidamount=isnull(paidamount,'0')+'" + credit + "',balamount=isnull(balamount,'0')+'" + credit + "' where app_no='" + staffappno + "' and headerfk='" + header + "' and ledgerfk='" + ledger + "' and memtype='2'";
                                        int saves = d2.update_method_wo_parameter(strAlt, "Text");

                                        fdt++;
                                        voucherinc = true;
                                        if (voucherinc == true || cbClgFormat.Checked)
                                        {
                                            dtvocucher = bindVoucherLedgerDet(dtvocucher, lblhdStr.Text, lblldStr.Text, credit);
                                        }
                                    }
                                }
                            }
                            #endregion

                            if (Convert.ToInt32(payamt) != 0)
                            {

                                contraamt = "select SUM(ISNULL(debit,0)-isnull(credit,0)) as amount,headerfk,ledgerfk,FinCashContraDetPK from FT_FinCashContraDet  where IsPetty=1 group by headerfk,ledgerfk,FinCashContraDetPK ";
                                dscontra.Clear();
                                dscontra = d2.select_method_wo_parameter(contraamt, "Text");
                                double payamount = 0;
                                if (dscontra.Tables.Count > 0)
                                {
                                    if (dscontra.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dscontra.Tables[0].Rows.Count; i++)
                                        {
                                            amount = Convert.ToString(dscontra.Tables[0].Rows[i]["amount"]);
                                            hedfk = Convert.ToString(dscontra.Tables[0].Rows[i]["headerfk"]);
                                            ledfk = Convert.ToString(dscontra.Tables[0].Rows[i]["ledgerfk"]);
                                            contrafk = Convert.ToString(dscontra.Tables[0].Rows[i]["FinCashContraDetPK"]);

                                            if (Convert.ToDouble(payamt) <= Convert.ToDouble(amount))
                                            {
                                                creditamt = payamt;
                                                payamt = "0";
                                            }
                                            else
                                            {
                                                creditamt = amount;
                                                payamount = Convert.ToDouble(payamt) - Convert.ToDouble(amount);
                                                payamt = Convert.ToString(payamount);
                                            }

                                            if (creditamt != "0")
                                            {
                                                string insert = " update FT_FinCashContraDet set Credit=isnull(Credit,0)+'" + creditamt + "',IsHand='" + hand + "',IsPetty='" + pety + "',TransDate='" + date + "',TransTime='" + fnltm + "',Narration='" + narration + "' where  IsPetty='" + pety + "'  and LedgerFK='" + ddlapyledg.SelectedItem.Value + "' and FinYearFK='" + fincyr + "'  and FinCashContraDetPK='" + contrafk + "'";
                                                save1 = d2.update_method_wo_parameter(insert, "Text");
                                                fcbt++;
                                            }

                                        }
                                    }
                                }

                            }
                            if (staffappno != "")
                            {
                                string upda = "update FT_FinInstallmentPay set InstallmentStatus='1' where App_No ='" + staffappno + "' and InstallmentDate ='" + insdt1.ToString("MM/dd/yyyy") + "'";
                                save1 = d2.update_method_wo_parameter(upda, "Text");
                            }

                            if (chkinstall.Checked == true)
                            {
                                for (int j = 0; j < gridView2.Rows.Count; j++)
                                {
                                    TextBox txtdate = (TextBox)gridView2.Rows[j].FindControl("txtdate");
                                    TextBox txtamt = (TextBox)gridView2.Rows[j].FindControl("txtamt");
                                    if (txtdate.Text.Trim() != "" && txtamt.Text.Trim() != "")
                                    {
                                        string instamt = "";
                                        string insdt = "";
                                        string instdt = "";
                                        string instmn = "";
                                        string instyr = "";
                                        string fninstdt = "";
                                        insdt = txtdate.Text.ToString();
                                        string[] strtxtdt = insdt.Split('/');
                                        if (strtxtdt.Length > 0)
                                        {
                                            instdt = strtxtdt[0].ToString();
                                            instmn = strtxtdt[1].ToString();
                                            instyr = strtxtdt[2].ToString();

                                            fninstdt = instmn + "/" + instdt + "/" + instyr;
                                        }

                                        instamt = txtamt.Text.ToString();
                                        string installqry = "insert into FT_FinInstallmentPay(memtype,App_No,MemName,InstallmentDate,InstallmentAmt,InstallmentStatus)values('" + memtype + "','" + staffappno + "','" + memname + "','" + fninstdt + "','" + instamt + "','0')";
                                        int install = d2.update_method_wo_parameter(installqry, "Text");
                                    }
                                }
                            }

                        }
                        #endregion
                        #region rb_vendor
                        else if (rb_vendor.Checked == true)
                        {
                            name = Convert.ToString(txt_venname.Text);
                            #region vendor
                            //if (gridView1.Rows.Count > 0)
                            //{
                            //    for (int i = 0; i < gridView1.Rows.Count; i++)
                            //    {

                            //        DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                            //        DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                            //        TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                            //        if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                            //        {
                            //            header = strheader.SelectedItem.Value.ToString();
                            //            ledger = strledger.SelectedItem.Value.ToString();
                            //            credit = Convert.ToInt32(valtxtpayamt.Text.ToString());
                            //            insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + vendcode + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + credit + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','','','0','','','','','0','','','0','','','0','','','','0')";
                            //            save = d2.update_method_wo_parameter(insqry, "Text");
                            //            fdt++;
                            //        }

                            //    }
                            //}
                            #endregion

                            if (gridven.Rows.Count > 0)
                            {
                                for (int i = 0; i < gridven.Rows.Count; i++)
                                {
                                    DropDownList headname = (DropDownList)gridven.Rows[i].FindControl("ddlheader");
                                    DropDownList ledname = (DropDownList)gridven.Rows[i].FindControl("ddlledger");
                                    TextBox allotamt = (TextBox)gridven.Rows[i].FindControl("txttotamt");
                                    TextBox paidamt = (TextBox)gridven.Rows[i].FindControl("txtpaidamt");
                                    TextBox bal = (TextBox)gridven.Rows[i].FindControl("txtbal");
                                    TextBox tobepaid = (TextBox)gridven.Rows[i].FindControl("txttobepaid");
                                    TextBox orderid = (TextBox)gridven.Rows[i].FindControl("txtorderid");
                                    heaname = headname.SelectedItem.Value.ToString();
                                    ledgname = ledname.SelectedItem.Value.ToString();
                                    alltamt = Convert.ToString(allotamt.Text);
                                    padamt = Convert.ToString(paidamt.Text);
                                    baln = Convert.ToString(bal.Text);
                                    tobpaid = Convert.ToString(tobepaid.Text);
                                    oridvalue = Convert.ToString(orderid.Text);
                                    if (heaname != "" && ledgname != "" && alltamt != "" && baln != "" && tobpaid != "" && oridvalue != "")
                                    {
                                        string inseqry = "if exists(select * from FT_FinDailyTransactionDetails where VendorFK='" + vendcode + "' and OrderCode='" + oridvalue + "' and FinYearFK='" + fincyr + "' and MemType='" + memtype + "' and CollegeCode='" + ddl_collegename.SelectedItem.Value + "') update  FT_FinDailyTransactionDetails set Credit=Credit+'" + tobpaid + "' where VendorFK='" + vendcode + "' and OrderCode='" + oridvalue + "' and FinYearFK='" + fincyr + "' and MemType='" + memtype + "' and CollegeCode='" + ddl_collegename.SelectedItem.Value + "' else insert into  FT_FinDailyTransactionDetails(MemType,VendorFK,OrderCode,Credit,Debit,FinYearFK,CollegeCode)values('" + memtype + "','" + vendcode + "','" + oridvalue + "','" + tobpaid + "','" + alltamt + "','" + fincyr + "','" + ddl_collegename.SelectedItem.Value + "')";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(inseqry, "Text");
                                        fdt++;

                                        insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + vendcode + "','" + memname + "','" + heaname + "','" + ledgname + "','" + 0 + "','" + tobpaid + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','','','0','','1','','','0','','','0','','','0','','','','0')";
                                        save = d2.update_method_wo_parameter(insqry, "Text");
                                        fcbt++;
                                        voucherinc = true;
                                        if (voucherinc == true || cbClgFormat.Checked)
                                        {
                                            dtvocucher = bindVoucherLedgerDet(dtvocucher, headname.SelectedItem.Text, ledname.SelectedItem.Text, Convert.ToInt32(tobpaid));
                                        }
                                    }
                                }
                            }

                            if (Convert.ToInt32(tobpaid) != 0)
                            {

                                contraamt = "select SUM(ISNULL(debit,0)-isnull(credit,0)) as amount,headerfk,ledgerfk,FinCashContraDetPK from FT_FinCashContraDet  where IsPetty=1 group by headerfk,ledgerfk,FinCashContraDetPK ";
                                dscontra.Clear();
                                dscontra = d2.select_method_wo_parameter(contraamt, "Text");
                                double payamount = 0;
                                if (dscontra.Tables.Count > 0)
                                {
                                    if (dscontra.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dscontra.Tables[0].Rows.Count; i++)
                                        {
                                            amount = Convert.ToString(dscontra.Tables[0].Rows[i]["amount"]);
                                            hedfk = Convert.ToString(dscontra.Tables[0].Rows[i]["headerfk"]);
                                            ledfk = Convert.ToString(dscontra.Tables[0].Rows[i]["ledgerfk"]);
                                            contrafk = Convert.ToString(dscontra.Tables[0].Rows[i]["FinCashContraDetPK"]);

                                            if (Convert.ToDouble(tobpaid) <= Convert.ToDouble(amount))
                                            {
                                                creditamt = payamt;
                                                payamt = "0";
                                            }
                                            else
                                            {
                                                creditamt = amount;
                                                payamount = Convert.ToDouble(tobpaid) - Convert.ToDouble(amount);
                                                tobpaid = Convert.ToString(payamount);
                                            }
                                            string insert = " update FT_FinCashContraDet set Credit=isnull(Credit,0)+'" + creditamt + "',IsHand='" + hand + "',IsPetty='" + pety + "',TransDate='" + date + "',TransTime='" + fnltm + "',Narration='" + narration + "' where  IsPetty='" + pety + "'  and LedgerFK='" + ddlapyledg.SelectedItem.Value + "' and FinYearFK='" + fincyr + "'  and FinCashContraDetPK='" + contrafk + "'";
                                            save1 = d2.update_method_wo_parameter(insert, "Text");
                                            fcbt++;

                                        }
                                    }
                                }

                            }
                            if (vendcode != "")
                            {
                                string upda = "update FT_FinInstallmentPay set InstallmentStatus='1' where App_No ='" + vendcode + "' and InstallmentDate ='" + insdt1.ToString("MM/dd/yyyy") + "'";
                                save1 = d2.update_method_wo_parameter(upda, "Text");
                            }


                            if (chkinstall.Checked == true)
                            {
                                for (int j = 0; j < gridView2.Rows.Count; j++)
                                {
                                    TextBox txtdate = (TextBox)gridView2.Rows[j].FindControl("txtdate");
                                    TextBox txtamt = (TextBox)gridView2.Rows[j].FindControl("txtamt");
                                    if (txtdate.Text.Trim() != "" && txtamt.Text.Trim() != "")
                                    {
                                        string instamt = "";
                                        string insdt = "";
                                        string instdt = "";
                                        string instmn = "";
                                        string instyr = "";
                                        string fninstdt = "";
                                        insdt = txtdate.Text.ToString();
                                        string[] strtxtdt = insdt.Split('/');
                                        if (strtxtdt.Length > 0)
                                        {
                                            instdt = strtxtdt[0].ToString();
                                            instmn = strtxtdt[1].ToString();
                                            instyr = strtxtdt[2].ToString();

                                            fninstdt = instmn + "/" + instdt + "/" + instyr;
                                        }

                                        instamt = txtamt.Text.ToString();
                                        string installqry = "insert into FT_FinInstallmentPay(memtype,App_No,MemName,InstallmentDate,InstallmentAmt,InstallmentStatus)values('" + memtype + "','" + vendcode + "','" + memname + "','" + fninstdt + "','" + instamt + "','0')";
                                        int install = d2.update_method_wo_parameter(installqry, "Text");
                                    }
                                }
                            }

                        }
                        #endregion
                        #region rb_other
                        else if (rb_others.Checked == true)
                        {
                            name = Convert.ToString(TextBox6.Text);
                            DropDownList ddl_credit = new DropDownList();
                            DropDownList ddl_debit = new DropDownList();
                            TextBox txtpaymt = new TextBox();
                            if (othname != "" && comname != "")
                            {
                                string insertoth = "if exists(select * from CO_VendorMaster where VendorName='" + name + "' and vendorType='-5' and VendorMobileNo='" + mblno + "' )update CO_VendorMaster set VendorCompName='" + comname + "', VendorAddress='" + add + "',IdentityType='" + identiy + "',IdentityNo='" + identyno + "' where   VendorName='" + othname + "' and VendorMobileNo='" + mblno + "' and vendorType='-5' else insert into CO_VendorMaster(VendorName,Vendorcode,VendorCompName,VendorAddress,VendorMobileNo,IdentityType,IdentityNo,vendorType) values ('" + othname + "','" + vendorvoucher + "','" + comname + "','" + add + "','" + mblno + "','" + identiy + "','" + identyno + "','" + -5 + "')";
                                int s1 = d2.update_method_wo_parameter(insertoth, "Text");
                                fdt++;

                                vendorPk = d2.GetFunction("select VendorPK from CO_VendorMaster where  VendorName='" + name + "' and vendorType='-5' and VendorMobileNo='" + mblno + "'");
                            }
                            if (gridView1.Rows.Count > 0)
                            {
                                for (int i = 0; i < gridView1.Rows.Count; i++)
                                {

                                    DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                    DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                    TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                    if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                    {
                                        header = strheader.SelectedItem.Value.ToString();
                                        ledger = strledger.SelectedItem.Value.ToString();
                                        credit = Convert.ToInt32(valtxtpayamt.Text.ToString());
                                        double deductamount;
                                        double balamount = 0;
                                        double.TryParse(Convert.ToString(txt_dtsamt.Text), out deductamount);
                                        if (rb_percentage.Checked == true)
                                        {
                                            deductamount = (credit * deductamount / 100);
                                            balamount = credit - deductamount;
                                        }
                                        if (rb_amount.Checked == true)
                                        {

                                            balamount = credit - deductamount;
                                        }
                                        if (rb_percentage.Checked == true || rb_amount.Checked == true)
                                        {
                                            for (int j = 0; j < gridView3.Rows.Count; j++)
                                            {

                                                ddl_credit = (DropDownList)gridView3.Rows[j].FindControl("ddl_credit");
                                                ddl_debit = (DropDownList)gridView3.Rows[j].FindControl("ddl_debit");
                                                txtpaymt = (TextBox)gridView3.Rows[j].FindControl("txtpaymt");

                                                ledger = ddl_credit.SelectedItem.Value;
                                                insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + vendorPk + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + 0 + "','" + txtpaymt.Text.Trim() + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                                save = d2.update_method_wo_parameter(insqry, "Text");
                                                ledger = ddl_debit.SelectedItem.Value;
                                                insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + vendorPk + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + txtpaymt.Text.Trim() + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                                save = d2.update_method_wo_parameter(insqry, "Text");
                                            }
                                        }

                                        ledger = strledger.SelectedItem.Value.ToString();
                                        insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + vendorPk + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + balamount + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";

                                        save = d2.update_method_wo_parameter(insqry, "Text");
                                        fdt++;
                                        voucherinc = true;
                                        memTypeNo = vendorPk;
                                        if (voucherinc == true || cbClgFormat.Checked)
                                        {
                                            dtvocucher = bindVoucherLedgerDet(dtvocucher, strheader.SelectedItem.Text, strledger.SelectedItem.Text, credit);
                                        }
                                    }

                                }

                            }
                            if (Convert.ToInt32(payamt) != 0)
                            {

                                contraamt = "select SUM(ISNULL(debit,0)-isnull(credit,0)) as amount,headerfk,ledgerfk,FinCashContraDetPK from FT_FinCashContraDet  where IsPetty=1 group by headerfk,ledgerfk,FinCashContraDetPK ";
                                dscontra.Clear();
                                dscontra = d2.select_method_wo_parameter(contraamt, "Text");
                                double payamount = 0;
                                if (dscontra.Tables.Count > 0)
                                {
                                    if (dscontra.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dscontra.Tables[0].Rows.Count; i++)
                                        {
                                            amount = Convert.ToString(dscontra.Tables[0].Rows[i]["amount"]);
                                            hedfk = Convert.ToString(dscontra.Tables[0].Rows[i]["headerfk"]);
                                            ledfk = Convert.ToString(dscontra.Tables[0].Rows[i]["ledgerfk"]);
                                            contrafk = Convert.ToString(dscontra.Tables[0].Rows[i]["FinCashContraDetPK"]);

                                            if (Convert.ToDouble(payamt) <= Convert.ToDouble(amount))
                                            {
                                                creditamt = payamt;
                                                payamt = "0";
                                            }
                                            else
                                            {
                                                creditamt = amount;
                                                payamount = Convert.ToDouble(payamt) - Convert.ToDouble(amount);
                                                payamt = Convert.ToString(payamount);
                                            }
                                            string insert = " update FT_FinCashContraDet set Credit=isnull(Credit,0)+'" + creditamt + "',IsHand='" + hand + "',IsPetty='" + pety + "',TransDate='" + date + "',TransTime='" + fnltm + "',Narration='" + narration + "' where  IsPetty='" + pety + "' and LedgerFK='" + ddlapyledg.SelectedItem.Value + "' and FinYearFK='" + fincyr + "'  and FinCashContraDetPK='" + contrafk + "'";
                                            //if exists(select * from FT_FinCashContraDet where  IsPetty='" + pety + "' and HeaderFK='" + header + "' and LedgerFK='" + ledger + "' and FinYearFK='" + fincyr + "'  and FinCashContraDetPK='" + contrafk + "')
                                            save1 = d2.update_method_wo_parameter(insert, "Text");
                                            fcbt++;

                                        }
                                    }
                                }

                            }
                            if (insdt1 != null)
                            {
                                string upda = "update FT_FinInstallmentPay set InstallmentStatus='1' where App_No ='0' and InstallmentDate ='" + insdt1.ToString("MM/dd/yyyy") + "'";
                                save1 = d2.update_method_wo_parameter(upda, "Text");
                            }

                            if (chkinstall.Checked == true)
                            {
                                for (int j = 0; j < gridView2.Rows.Count; j++)
                                {
                                    TextBox txtdate = (TextBox)gridView2.Rows[j].FindControl("txtdate");
                                    TextBox txtamt = (TextBox)gridView2.Rows[j].FindControl("txtamt");
                                    if (txtdate.Text.Trim() != "" && txtamt.Text.Trim() != "")
                                    {
                                        string instamt = "";
                                        string insdt = "";
                                        string instdt = "";
                                        string instmn = "";
                                        string instyr = "";
                                        string fninstdt = "";
                                        insdt = txtdate.Text.ToString();
                                        string[] strtxtdt = insdt.Split('/');
                                        if (strtxtdt.Length > 0)
                                        {
                                            instdt = strtxtdt[0].ToString();
                                            instmn = strtxtdt[1].ToString();
                                            instyr = strtxtdt[2].ToString();

                                            fninstdt = instmn + "/" + instdt + "/" + instyr;
                                        }

                                        instamt = txtamt.Text.ToString();
                                        string installqry = "insert into FT_FinInstallmentPay(memtype,App_No,MemName,InstallmentDate,InstallmentAmt,InstallmentStatus)values('" + memtype + "','0','" + memname + "','" + fninstdt + "','" + instamt + "','0')";
                                        int install = d2.update_method_wo_parameter(installqry, "Text");
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else if (rb_bank.Checked == true || rbonline.Checked == true)
                    {

                        #region rb_stud
                        int upde = 0;
                        if (rb_stud.Checked == true)
                        {
                            name = Convert.ToString(txt_name.Text);
                            DropDownList ddl_credit = new DropDownList();
                            DropDownList ddl_debit = new DropDownList();
                            TextBox txtpaymt = new TextBox();
                            if (gridView1.Rows.Count > 0)
                            {
                                for (int i = 0; i < gridView1.Rows.Count; i++)
                                {

                                    DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                    DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                    TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                    if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                    {
                                        header = strheader.SelectedItem.Value.ToString();
                                        ledger = strledger.SelectedItem.Value.ToString();
                                        credit = Convert.ToInt32(valtxtpayamt.Text.ToString());
                                        double deductamount;
                                        double balamount = 0;
                                        double.TryParse(Convert.ToString(txt_dtsamt.Text), out deductamount);
                                        if (rb_percentage.Checked == true)
                                        {
                                            deductamount = (credit * deductamount / 100);
                                            balamount = credit - deductamount;
                                        }
                                        if (rb_amount.Checked == true)
                                        {

                                            balamount = credit - deductamount;
                                        }
                                        adjustamt = adjustamt + credit;
                                        if (rb_percentage.Checked == true || rb_amount.Checked == true)
                                        {
                                            for (int j = 0; j < gridView3.Rows.Count; j++)
                                            {

                                                ddl_credit = (DropDownList)gridView3.Rows[j].FindControl("ddl_credit");
                                                ddl_debit = (DropDownList)gridView3.Rows[j].FindControl("ddl_debit");
                                                txtpaymt = (TextBox)gridView3.Rows[j].FindControl("txtpaymt");

                                                ledger = ddl_credit.SelectedItem.Value;
                                                insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + 0 + "','" + txtpaymt.Text.Trim() + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                                save = d2.update_method_wo_parameter(insqry, "Text");
                                                ledger = ddl_debit.SelectedItem.Value;
                                                insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + txtpaymt.Text.Trim() + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                                save = d2.update_method_wo_parameter(insqry, "Text");
                                            }
                                        }

                                        ledger = strledger.SelectedItem.Value.ToString();
                                        insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + balamount + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";

                                        save = d2.update_method_wo_parameter(insqry, "Text");
                                        fdt++;
                                        chequeinc = true;
                                        if (cbClgFormat.Checked)
                                        {
                                            dtvocucher = bindVoucherLedgerDet(dtvocucher, strheader.SelectedItem.Text, strledger.SelectedItem.Text, credit);
                                        }
                                    }

                                }
                                //if (adjustamt > 0 && adjustamt != null)
                                //{
                                //    update = "update FT_ExcessDet set AdjAmt='" + adjustamt + "' where App_No='" + studappno + "'";
                                //    int upadj = d2.update_method_wo_parameter(update, "Text");
                                //}

                            }
                            if (Convert.ToInt32(payamt) != 0)
                            {
                                string bankfk = "";
                                double payamount = 0;
                                string conbank = " select SUM(ISNULL(debit,0) -ISNULL(credit,0)) as amount,BankFK,FinCashContraDetPK from FT_FinCashContraDet where IsBank=1 group by BankFK,FinCashContraDetPK";
                                DataSet dsbank = new DataSet();
                                dsbank.Clear();
                                dsbank = d2.select_method_wo_parameter(conbank, "Text");
                                if (dsbank.Tables.Count > 0)
                                {
                                    if (dsbank.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dsbank.Tables[0].Rows.Count; i++)
                                        {
                                            amount = Convert.ToString(dsbank.Tables[0].Rows[i]["amount"]);
                                            bankfk = Convert.ToString(dsbank.Tables[0].Rows[i]["BankFK"]);
                                            contrafk = Convert.ToString(dsbank.Tables[0].Rows[i]["FinCashContraDetPK"]);
                                            string ttamt = payamt;
                                            if (Convert.ToDouble(ttamt) <= Convert.ToDouble(amount))
                                            {
                                                creditamt = ttamt;
                                                ttamt = "0";
                                            }
                                            else
                                            {
                                                creditamt = amount;
                                                payamount = Convert.ToDouble(ttamt) - Convert.ToDouble(amount);
                                                ttamt = Convert.ToString(payamount);
                                            }
                                            string insert = " update FT_FinCashContraDet set TransDate='" + date + "',TransTime='" + fnltm + "',Credit=isnull(Credit,0)+'" + creditamt + "',IsBank='" + 1 + "',Narration='" + narration + "' where IsBank='1' and BankFK='" + bkcode + "' and FinYearFK='" + fincyr + "' and  FinCashContraDetPK='" + contrafk + "'";
                                            save1 = d2.update_method_wo_parameter(insert, "Text");
                                            fcbt++;
                                        }
                                    }
                                }
                            }
                            if (Convert.ToInt32(payamt) != 0)
                            {
                                // insqry = "insert into FT_FinBankTransaction (TransDate,TransTime,BankFK,PayMode,DailyTransID,IsDeposited,IsCleared,IsBounced,Credit,Debit,FinYearFK) values ('" + date + "','" + fnltm + "','" + bkcode + "','" + paymode + "','" + TransCode + "','1','1','0','" + payamt + "','0','" + fincyr + "')";
                                insqry = "  update FT_FinBankTransaction set TransDate='" + date + "',TransTime='" + fnltm + "',PayMode='" + paymode + "',DailyTransID='" + TransCode + "',IsDeposited='1',IsCleared='1',IsBounced='0',Credit+='" + payamt + "' where BankFK='" + bkcode + "' and FinYearFK='" + fincyr + "'";
                                save = d2.update_method_wo_parameter(insqry, "Text");
                                fcbt++;
                            }
                            if (studappno != "")
                            {
                                string upda = "update FT_FinInstallmentPay set InstallmentStatus='1' where App_No ='" + studappno + "' and InstallmentDate ='" + insdt1.ToString("MM/dd/yyyy") + "'";
                                save1 = d2.update_method_wo_parameter(upda, "Text");
                            }
                            if (chkinstall.Checked == true)
                            {
                                for (int j = 0; j < gridView2.Rows.Count; j++)
                                {
                                    TextBox txtdate = (TextBox)gridView2.Rows[j].FindControl("txtdate");
                                    TextBox txtamt = (TextBox)gridView2.Rows[j].FindControl("txtamt");
                                    if (txtdate.Text.Trim() != "" && txtamt.Text.Trim() != "")
                                    {
                                        string instamt = "";
                                        string insdt = "";
                                        string instdt = "";
                                        string instmn = "";
                                        string instyr = "";
                                        string fninstdt = "";
                                        insdt = txtdate.Text.ToString();
                                        string[] strtxtdt = insdt.Split('/');
                                        if (strtxtdt.Length > 0)
                                        {
                                            instdt = strtxtdt[0].ToString();
                                            instmn = strtxtdt[1].ToString();
                                            instyr = strtxtdt[2].ToString();

                                            fninstdt = instmn + "/" + instdt + "/" + instyr;
                                        }

                                        instamt = txtamt.Text.ToString();
                                        string installqry = "insert into FT_FinInstallmentPay(memtype,App_No,MemName,InstallmentDate,InstallmentAmt,InstallmentStatus)values('" + memtype + "','" + studappno + "','" + memname + "','" + fninstdt + "','" + instamt + "','0')";
                                        int install = d2.update_method_wo_parameter(installqry, "Text");
                                    }
                                }
                            }
                            #region old

                            //#region excessupdate
                            //if (chk_insexcess.Checked == true)
                            //{
                            //    if (adjustamt > 0 && adjustamt != null)
                            //    {
                            //        excessvalue = adjustamt + adjustinstall;
                            //        oldamt = d2.GetFunction("select isnull(ExcessAmt,0)-isnull(AdjAmt,0) from FT_ExcessDet where App_No='" + studappno + "' and ExcessType='1'");
                            //        if (Convert.ToDouble(oldamt) >= Convert.ToDouble(excessvalue))
                            //        {
                            //            string update = "update FT_ExcessDet set AdjAmt=isnull(AdjAmt,0) +'" + excessvalue + "' where App_No='" + studappno + "' and ExcessType='1'";
                            //            int upadj = d2.update_method_wo_parameter(update, "Text");

                            //            string excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and ExcessType='1'");
                            //            string select = "select (isnull(ExcessAmt,'0')-isnull(AdjAmt,0))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                            //            dsex.Clear();
                            //            dsex = d2.select_method_wo_parameter(select, "Text");
                            //            if (dsex.Tables[0].Rows.Count > 0)
                            //            {
                            //                for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                            //                {
                            //                    headerfk = Convert.ToString(dsex.Tables[0].Rows[i]["HeaderFK"]);
                            //                    ledgerfk = Convert.ToString(dsex.Tables[0].Rows[i]["LedgerFK"]);
                            //                    excessamt = Convert.ToString(dsex.Tables[0].Rows[i]["total"]);
                            //                    string updateexcess = "";
                            //                    if (excessvalue > 0 && excessvalue != null && excessamt != "" && excessamt != null)
                            //                    {
                            //                        if (Convert.ToDouble(excessamt) >= excessvalue)
                            //                        {
                            //                            updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,0) +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                            //                            upde = d2.update_method_wo_parameter(updateexcess, "Text");
                            //                            excessvalue = 0;
                            //                            adjustamt = 0;
                            //                        }
                            //                        else if (Convert.ToDouble(excessamt) <= excessvalue)
                            //                        {
                            //                            updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,0) +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                            //                            upde = d2.update_method_wo_parameter(updateexcess, "Text");
                            //                            excessvalue = excessvalue - Convert.ToInt32(excessamt);
                            //                            adjustamt = excessvalue;
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //        }
                            //        excessandrefund = true;
                            //    }
                            //}
                            //#endregion
                            //#region refundupdate
                            //if (refundamt != "" && refundamt != null)
                            //{
                            //    if (adjustamt > 0 && adjustamt != null)
                            //    {
                            //        excessvalue = adjustamt + adjustinstall;
                            //        oldamt = d2.GetFunction("select isnull(ExcessAmt,0)-isnull(AdjAmt,0) from FT_ExcessDet where App_No='" + studappno + "' and ExcessType in('2')");
                            //        if (Convert.ToDouble(oldamt) >= Convert.ToDouble(excessvalue))
                            //        {
                            //            string update = "update FT_ExcessDet set AdjAmt=isnull(AdjAmt,0) +'" + excessvalue + "' where App_No='" + studappno + "' and Excesstype in('2')";
                            //            int upadj = d2.update_method_wo_parameter(update, "Text");

                            //            string excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and ExcessType in('2')");
                            //            string select = "select (isnull(ExcessAmt,'0')-isnull(AdjAmt,0))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                            //            dsex.Clear();
                            //            dsex = d2.select_method_wo_parameter(select, "Text");
                            //            if (dsex.Tables[0].Rows.Count > 0)
                            //            {
                            //                for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                            //                {
                            //                    headerfk = Convert.ToString(dsex.Tables[0].Rows[i]["HeaderFK"]);
                            //                    ledgerfk = Convert.ToString(dsex.Tables[0].Rows[i]["LedgerFK"]);
                            //                    excessamt = Convert.ToString(dsex.Tables[0].Rows[i]["total"]);
                            //                    string updateexcess = "";
                            //                    if (excessvalue > 0 && excessvalue != null && excessamt != "" && excessamt != null)
                            //                    {
                            //                        if (Convert.ToDouble(excessamt) >= excessvalue)
                            //                        {
                            //                            updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,0) +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                            //                            excessvalue = 0;
                            //                            upde = d2.update_method_wo_parameter(updateexcess, "Text");
                            //                            adjustamt = 0;
                            //                        }
                            //                        else if (Convert.ToDouble(excessamt) <= excessvalue)
                            //                        {
                            //                            updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,0) +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                            //                            upde = d2.update_method_wo_parameter(updateexcess, "Text");
                            //                            excessvalue = excessvalue - Convert.ToInt32(excessamt);
                            //                            adjustamt = excessvalue;
                            //                        }
                            //                    }

                            //                }
                            //            }
                            //            excessandrefund = true;
                            //        }
                            //    }
                            //}

                            ////type 3
                            //if (chk_deposite.Checked == true && refundamt != "" && refundamt != null)
                            //{
                            //    if (adjustamt > 0 && adjustamt != null)
                            //    {
                            //        excessvalue = adjustamt + adjustinstall;
                            //        oldamt = d2.GetFunction("select isnull(ExcessAmt,0)-isnull(AdjAmt,0) from FT_ExcessDet where App_No='" + studappno + "' and ExcessType in('3')");
                            //        if (Convert.ToDouble(oldamt) >= Convert.ToDouble(excessvalue))
                            //        {
                            //            string update = "update FT_ExcessDet set AdjAmt=isnull(AdjAmt,0) +'" + excessvalue + "' where App_No='" + studappno + "' and Excesstype in('3')";
                            //            int upadj = d2.update_method_wo_parameter(update, "Text");

                            //            string excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and ExcessType in('3')");
                            //            string select = "select (isnull(ExcessAmt,'0')-isnull(AdjAmt,0))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                            //            dsex.Clear();
                            //            dsex = d2.select_method_wo_parameter(select, "Text");
                            //            if (dsex.Tables[0].Rows.Count > 0)
                            //            {
                            //                for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                            //                {
                            //                    headerfk = Convert.ToString(dsex.Tables[0].Rows[i]["HeaderFK"]);
                            //                    ledgerfk = Convert.ToString(dsex.Tables[0].Rows[i]["LedgerFK"]);
                            //                    excessamt = Convert.ToString(dsex.Tables[0].Rows[i]["total"]);
                            //                    string updateexcess = "";
                            //                    if (excessvalue > 0 && excessvalue != null && excessamt != "" && excessamt != null)
                            //                    {
                            //                        if (Convert.ToDouble(excessamt) >= excessvalue)
                            //                        {
                            //                            updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,0) +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                            //                            upde = d2.update_method_wo_parameter(updateexcess, "Text");
                            //                            adjustamt = 0;
                            //                            excessvalue = 0;
                            //                        }
                            //                        else if (Convert.ToDouble(excessamt) <= excessvalue)
                            //                        {
                            //                            updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,0) +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                            //                            upde = d2.update_method_wo_parameter(updateexcess, "Text");
                            //                            excessvalue = excessvalue - Convert.ToInt32(excessamt);
                            //                            adjustamt = excessvalue;
                            //                        }
                            //                    }

                            //                }
                            //            }
                            //            excessandrefund = true;
                            //        }
                            //    }
                            //}
                            //#endregion
                            #endregion


                            #region Excess update

                            int updatex = 0;
                            string excessdetpk = "";
                            double oldAmt = 0;

                            if (chk_insexcess.Checked == true)
                            {
                                if (adjustamt > 0 && adjustamt != null)
                                {
                                    excessvalue = adjustamt + adjustinstall;
                                    oldamt = d2.GetFunction("select isnull(ExcessAmt,0)-isnull(AdjAmt,0) from FT_ExcessDet where App_No='" + studappno + "' and ExcessType='1'");
                                    double.TryParse(oldamt, out oldAmt);
                                    if (oldAmt >= excessvalue)
                                    {
                                        #region
                                        update = "update FT_ExcessDet set AdjAmt=ISNULL(AdjAmt,'0') +'" + excessvalue + "' where App_No='" + studappno + "' and ExcessType=1";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");

                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=1");
                                        string select = "select (ISNULL(ExcessAmt,'0')-ISNULL(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt >= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =ISNULL(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        adjustamt = excessvalue;
                                                    }
                                                    if (excessamt <= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =ISNULL(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        adjustamt = excessvalue;
                                                    }
                                                }

                                            }
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region
                                        double balAmt = 0;
                                        balAmt = excessvalue - oldAmt;
                                        adjustamt = balAmt;
                                        balAmt = 0;
                                        update = "update FT_ExcessDet set AdjAmt=ISNULL(AdjAmt,'0') +'" + oldAmt + "' where App_No='" + studappno + "' and ExcessType=1";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");
                                        excessvalue = oldAmt;

                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=1");
                                        string select = "select (ISNULL(ExcessAmt,'0')-ISNULL(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt >= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =ISNULL(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        balAmt = excessvalue;
                                                    }
                                                    if (excessamt <= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =ISNULL(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        balAmt = excessvalue;
                                                    }
                                                }

                                            }
                                            adjustamt += balAmt;
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region Refunds Update

                            #region Type 2
                            if (refundamt != "" && refundamt != null)
                            {
                                if (adjustamt > 0 && adjustamt != null)
                                {
                                    excessvalue = adjustamt + adjustinstall;
                                    oldamt = d2.GetFunction("select isnull(ExcessAmt,0)-isnull(AdjAmt,0) from FT_ExcessDet where App_No='" + studappno + "' and ExcessType='2'");
                                    double.TryParse(oldamt, out oldAmt);
                                    if (oldAmt >= excessvalue)
                                    {
                                        #region
                                        update = "update FT_ExcessDet set AdjAmt=isnull(AdjAmt,'0')+'" + excessvalue + "' where App_No='" + studappno + "' and ExcessType=2";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");

                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=2");
                                        string select = "select (isnull(ExcessAmt,'0')-isnull(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt > excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        adjustamt = 0;
                                                    }
                                                    else if (excessamt < excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        adjustamt = excessvalue;
                                                    }
                                                }

                                            }
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region
                                        double balAmt = 0;
                                        balAmt = excessvalue - oldAmt;
                                        adjustamt = balAmt;
                                        balAmt = 0;
                                        update = "update FT_ExcessDet set AdjAmt=isnull(AdjAmt,'0')+'" + oldAmt + "' where App_No='" + studappno + "' and ExcessType=2";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");
                                        excessvalue = oldAmt;
                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=2");
                                        string select = "select (isnull(ExcessAmt,'0')-isnull(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt > excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        balAmt = excessvalue;
                                                    }
                                                    else if (excessamt < excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        balAmt = excessvalue;
                                                    }
                                                }

                                            }
                                            adjustamt += balAmt;
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }

                                }
                            }
                            #endregion

                            #region Type 3
                            if (chk_deposite.Checked == true && refundamt != "" && refundamt != null)
                            {
                                if (adjustamt > 0 && adjustamt != null)
                                {
                                    excessvalue = adjustamt + adjustinstall;
                                    oldamt = d2.GetFunction("select isnull(ExcessAmt,0)-isnull(AdjAmt,0) from FT_ExcessDet where App_No='" + studappno + "' and ExcessType='3'");
                                    double.TryParse(oldamt, out oldAmt);
                                    if (oldAmt >= excessvalue)
                                    {
                                        #region
                                        update = "update FT_ExcessDet set AdjAmt=isnull(AdjAmt,'0')+'" + excessvalue + "' where App_No='" + studappno + "' and ExcessType=3";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");

                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=3");
                                        string select = "select (isnull(ExcessAmt,'0')-isnull(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt >= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        adjustamt = 0;
                                                    }
                                                    else if (excessamt <= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        adjustamt = excessvalue;
                                                    }
                                                }

                                            }
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }
                                    else
                                    {
                                        #region
                                        double balAmt = 0;
                                        balAmt = excessvalue - oldAmt;
                                        adjustamt = balAmt;
                                        balAmt = 0;
                                        update = "update FT_ExcessDet set AdjAmt=isnull(AdjAmt,'0')+'" + oldAmt + "' where App_No='" + studappno + "' and ExcessType=3";
                                        int upadj = d2.update_method_wo_parameter(update, "Text");
                                        excessvalue = oldAmt;
                                        excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + studappno + "' and excessType=3");
                                        string select = "select (isnull(ExcessAmt,'0')-isnull(AdjAmt,'0'))as total ,HeaderFK,LedgerFK  from FT_ExcessLedgerDet where ExcessDetFK ='" + excessdetpk + "'";
                                        dsex.Clear();
                                        dsex = d2.select_method_wo_parameter(select, "Text");
                                        if (dsex.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < dsex.Tables[0].Rows.Count; ii++)
                                            {
                                                headerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["HeaderFK"]);
                                                ledgerfk = Convert.ToString(dsex.Tables[0].Rows[ii]["LedgerFK"]);
                                                double.TryParse(Convert.ToString(dsex.Tables[0].Rows[ii]["total"]), out excessamt);
                                                string updateexcess = "";
                                                if (excessvalue > 0 && excessvalue != null && excessamt != 0)
                                                {
                                                    if (excessamt >= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessvalue + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = 0;
                                                        balAmt = excessvalue;
                                                    }
                                                    else if (excessamt <= excessvalue)
                                                    {
                                                        updateexcess = "update FT_ExcessLedgerDet set AdjAmt =isnull(AdjAmt,'0') +'" + excessamt + "' where ExcessDetFK ='" + excessdetpk + "' and HeaderFK='" + headerfk + "' and LedgerFK ='" + ledgerfk + "'";
                                                        updatex = d2.update_method_wo_parameter(updateexcess, "Text");
                                                        excessvalue = excessvalue - excessamt;
                                                        balAmt = excessvalue;
                                                    }
                                                }

                                            }
                                            adjustamt += balAmt;
                                        }
                                        excessandrefund = true;
                                        #endregion
                                    }

                                }
                            }
                            #endregion

                            #endregion
                        }
                        #endregion
                        #region rb_staff
                        else if (rb_staff.Checked == true)
                        {
                            name = Convert.ToString(TextBox3.Text);
                            DropDownList ddl_credit = new DropDownList();
                            DropDownList ddl_debit = new DropDownList();
                            TextBox txtpaymt = new TextBox();
                            #region without allot
                            if (!cbStaff.Checked && gridView1.Rows.Count > 0)
                            {
                                for (int i = 0; i < gridView1.Rows.Count; i++)
                                {

                                    DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                    DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                    TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                    if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                    {
                                        header = strheader.SelectedItem.Value.ToString();
                                        ledger = strledger.SelectedItem.Value.ToString();
                                        credit = Convert.ToInt32(valtxtpayamt.Text.ToString());
                                        double deductamount;
                                        double balamount = 0;
                                        double.TryParse(Convert.ToString(txt_dtsamt.Text), out deductamount);
                                        if (rb_percentage.Checked == true)
                                        {
                                            deductamount = (credit * deductamount / 100);
                                            balamount = credit - deductamount;
                                        }
                                        if (rb_amount.Checked == true)
                                        {

                                            balamount = credit - deductamount;
                                        }
                                        if (rb_percentage.Checked == true || rb_amount.Checked == true)
                                        {
                                            for (int j = 0; j < gridView3.Rows.Count; j++)
                                            {

                                                ddl_credit = (DropDownList)gridView3.Rows[j].FindControl("ddl_credit");
                                                ddl_debit = (DropDownList)gridView3.Rows[j].FindControl("ddl_debit");
                                                txtpaymt = (TextBox)gridView3.Rows[j].FindControl("txtpaymt");
                                            }
                                            ledger = ddl_credit.SelectedItem.Value;
                                            insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + 0 + "','" + deductamount + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                            save = d2.update_method_wo_parameter(insqry, "Text");
                                            ledger = ddl_debit.SelectedItem.Value;
                                            insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + deductamount + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                            save = d2.update_method_wo_parameter(insqry, "Text");
                                        }

                                        ledger = strledger.SelectedItem.Value.ToString();
                                        insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + balamount + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";

                                        save = d2.update_method_wo_parameter(insqry, "Text");
                                        fdt++;
                                        chequeinc = true;
                                        if (cbClgFormat.Checked)
                                        {
                                            dtvocucher = bindVoucherLedgerDet(dtvocucher, strheader.SelectedItem.Text, strledger.SelectedItem.Text, credit);
                                        }
                                    }

                                }
                            }
                            #endregion

                            #region with allot
                            if (cbStaff.Checked && gdPaid.Rows.Count > 0)
                            {
                                for (int i = 0; i < gdPaid.Rows.Count; i++)
                                {

                                    Label lblhdStr = (Label)gdPaid.Rows[i].FindControl("lblhdName");
                                    Label lblldStr = (Label)gdPaid.Rows[i].FindControl("lblldName");
                                    Label lblhd = (Label)gdPaid.Rows[i].FindControl("lblhdFk");
                                    Label lblld = (Label)gdPaid.Rows[i].FindControl("lblldFk");
                                    TextBox valtxtpayamt = (TextBox)gdPaid.Rows[i].FindControl("txtAmt");
                                    if (lblhd.Text.Trim() != "" && lblld.Text.Trim() != "" && valtxtpayamt.Text.Trim() != "")
                                    {
                                        header = lblhd.Text.ToString();
                                        ledger = lblld.Text.ToString();
                                        credit = Convert.ToInt32(valtxtpayamt.Text.ToString());
                                        double deductamount;
                                        double balamount = 0;
                                        double.TryParse(Convert.ToString(txt_dtsamt.Text), out deductamount);
                                        if (rb_percentage.Checked == true)
                                        {
                                            deductamount = (credit * deductamount / 100);
                                            balamount = credit - deductamount;
                                        }
                                        if (rb_amount.Checked == true)
                                        {

                                            balamount = credit - deductamount;
                                        }

                                        if (rb_percentage.Checked == true || rb_amount.Checked == true)
                                        {
                                            for (int j = 0; j < gridView3.Rows.Count; j++)
                                            {

                                                ddl_credit = (DropDownList)gridView3.Rows[j].FindControl("ddl_credit");
                                                ddl_debit = (DropDownList)gridView3.Rows[j].FindControl("ddl_debit");
                                                txtpaymt = (TextBox)gridView3.Rows[j].FindControl("txtpaymt");
                                            }
                                            ledger = ddl_credit.SelectedItem.Value;
                                            insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + 0 + "','" + deductamount + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                            save = d2.update_method_wo_parameter(insqry, "Text");
                                            ledger = ddl_debit.SelectedItem.Value;
                                            insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + deductamount + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                            save = d2.update_method_wo_parameter(insqry, "Text");
                                        }
                                        ledger = lblld.Text.ToString();
                                        insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + balamount + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";

                                        save = d2.update_method_wo_parameter(insqry, "Text");

                                        string strAlt = " update ft_feeallot set paidamount=isnull(paidamount,'0')+'" + credit + "',balamount=isnull(balamount,'0')+'" + credit + "' where app_no='" + staffappno + "' and headerfk='" + header + "' and ledgerfk='" + ledger + "' and memtype='2'";
                                        int saves = d2.update_method_wo_parameter(strAlt, "Text");
                                        fdt++;
                                        voucherinc = true;
                                        if (voucherinc == true || cbClgFormat.Checked)
                                        {
                                            dtvocucher = bindVoucherLedgerDet(dtvocucher, lblhdStr.Text, lblldStr.Text, credit);
                                        }
                                    }
                                }
                            }
                            #endregion

                            if (Convert.ToInt32(payamt) != 0)
                            {
                                string bankfk = "";
                                double payamount = 0;
                                string conbank = " select SUM(ISNULL(debit,0) -ISNULL(credit,0)) as amount,BankFK,FinCashContraDetPK from FT_FinCashContraDet where IsBank=1 group by BankFK,FinCashContraDetPK";
                                DataSet dsbank = new DataSet();
                                dsbank.Clear();
                                dsbank = d2.select_method_wo_parameter(conbank, "Text");
                                if (dsbank.Tables.Count > 0)
                                {
                                    if (dsbank.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dsbank.Tables[0].Rows.Count; i++)
                                        {
                                            amount = Convert.ToString(dsbank.Tables[0].Rows[i]["amount"]);
                                            bankfk = Convert.ToString(dsbank.Tables[0].Rows[i]["BankFK"]);
                                            contrafk = Convert.ToString(dsbank.Tables[0].Rows[i]["FinCashContraDetPK"]);
                                            if (Convert.ToDouble(payamt) <= Convert.ToDouble(amount))
                                            {
                                                creditamt = payamt;
                                                payamt = "0";
                                            }
                                            else
                                            {
                                                creditamt = amount;
                                                payamount = Convert.ToDouble(payamt) - Convert.ToDouble(amount);
                                                payamt = Convert.ToString(payamount);
                                            }
                                            string insert = " update FT_FinCashContraDet set TransDate='" + date + "',TransTime='" + fnltm + "',Credit=isnull(Credit,0)+'" + creditamt + "',IsBank='" + 1 + "',Narration='" + narration + "' where IsBank='1' and BankFK='" + bkcode + "' and FinYearFK='" + fincyr + "' and  FinCashContraDetPK='" + contrafk + "'";
                                            save1 = d2.update_method_wo_parameter(insert, "Text");
                                            fcbt++;
                                        }
                                    }
                                }
                            }
                            if (Convert.ToInt32(payamt) != 0)
                            {
                                // insqry = "insert into FT_FinBankTransaction (TransDate,TransTime,BankFK,PayMode,DailyTransID,IsDeposited,IsCleared,IsBounced,Credit,Debit,FinYearFK) values ('" + date + "','" + fnltm + "','" + bkcode + "','" + paymode + "','" + TransCode + "','1','1','0','" + payamt + "','0','" + fincyr + "')";
                                insqry = "  update FT_FinBankTransaction set TransDate='" + date + "',TransTime='" + fnltm + "',PayMode='" + paymode + "',DailyTransID='" + TransCode + "',IsDeposited='1',IsCleared='1',IsBounced='0',Credit+='" + payamt + "' where BankFK='" + bkcode + "' and FinYearFK='" + fincyr + "'";
                                save = d2.update_method_wo_parameter(insqry, "Text");
                                fcbt++;
                            }
                            if (staffappno != "")
                            {
                                string upda = "update FT_FinInstallmentPay set InstallmentStatus='1' where App_No ='" + staffappno + "' and InstallmentDate ='" + insdt1.ToString("MM/dd/yyyy") + "'";
                                save1 = d2.update_method_wo_parameter(upda, "Text");
                            }
                            if (chkinstall.Checked == true)
                            {
                                for (int j = 0; j < gridView2.Rows.Count; j++)
                                {
                                    TextBox txtdate = (TextBox)gridView2.Rows[j].FindControl("txtdate");
                                    TextBox txtamt = (TextBox)gridView2.Rows[j].FindControl("txtamt");
                                    if (txtdate.Text.Trim() != "" && txtamt.Text.Trim() != "")
                                    {
                                        string instamt = "";
                                        string insdt = "";
                                        string instdt = "";
                                        string instmn = "";
                                        string instyr = "";
                                        string fninstdt = "";
                                        insdt = txtdate.Text.ToString();
                                        string[] strtxtdt = insdt.Split('/');
                                        if (strtxtdt.Length > 0)
                                        {
                                            instdt = strtxtdt[0].ToString();
                                            instmn = strtxtdt[1].ToString();
                                            instyr = strtxtdt[2].ToString();

                                            fninstdt = instmn + "/" + instdt + "/" + instyr;
                                        }

                                        instamt = txtamt.Text.ToString();
                                        string installqry = "insert into FT_FinInstallmentPay(memtype,App_No,MemName,InstallmentDate,InstallmentAmt,InstallmentStatus)values('" + memtype + "','" + staffappno + "','" + memname + "','" + fninstdt + "','" + instamt + "','0')";
                                        int install = d2.update_method_wo_parameter(installqry, "Text");
                                    }
                                }
                            }
                            // }
                            // #endregion
                        }
                        #endregion
                        #region rb_vendor
                        else if (rb_vendor.Checked == true)
                        {
                            name = Convert.ToString(txt_venname.Text);
                            #region vendor
                            //if (gridView1.Rows.Count > 0)
                            //{
                            //    for (int i = 0; i < gridView1.Rows.Count; i++)
                            //    {

                            //        DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                            //        DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                            //        TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                            //        if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                            //        {
                            //            header = strheader.SelectedItem.Value.ToString();
                            //            ledger = strledger.SelectedItem.Value.ToString();
                            //            credit = Convert.ToInt32(valtxtpayamt.Text.ToString());
                            //            insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + vendcode + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + credit + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','','','0','','','','','0','','','0','','','0','','','','0')";
                            //            save = d2.update_method_wo_parameter(insqry, "Text");
                            //            fdt++;
                            //        }

                            //    }
                            #endregion

                            if (gridven.Rows.Count > 0)
                            {
                                for (int i = 0; i < gridven.Rows.Count; i++)
                                {
                                    DropDownList headname = (DropDownList)gridven.Rows[i].FindControl("ddlheader");
                                    DropDownList ledname = (DropDownList)gridven.Rows[i].FindControl("ddlledger");
                                    TextBox allotamt = (TextBox)gridven.Rows[i].FindControl("txttotamt");
                                    TextBox paidamt = (TextBox)gridven.Rows[i].FindControl("txtpaidamt");
                                    TextBox bal = (TextBox)gridven.Rows[i].FindControl("txtbal");
                                    TextBox tobepaid = (TextBox)gridven.Rows[i].FindControl("txttobepaid");
                                    TextBox orderid = (TextBox)gridven.Rows[i].FindControl("txtorderid");
                                    heaname = headname.SelectedItem.Value.ToString();
                                    ledgname = ledname.SelectedItem.Value.ToString();
                                    alltamt = Convert.ToString(allotamt.Text);
                                    padamt = Convert.ToString(paidamt.Text);
                                    baln = Convert.ToString(bal.Text);
                                    tobpaid = Convert.ToString(tobepaid.Text);
                                    oridvalue = Convert.ToString(orderid.Text);
                                    if (heaname != "" && ledgname != "" && alltamt != "" && baln != "" && tobpaid != "" && oridvalue != "")
                                    {
                                        string inseqry = "if exists(select * from FT_FinDailyTransactionDetails where VendorFK='" + vendcode + "' and OrderCode='" + oridvalue + "' and FinYearFK='" + fincyr + "' and MemType='" + memtype + "' and CollegeCode='" + ddl_collegename.SelectedItem.Value + "') update  FT_FinDailyTransactionDetails set Credit=Credit+'" + tobpaid + "' where VendorFK='" + vendcode + "' and OrderCode='" + oridvalue + "' and FinYearFK='" + fincyr + "' and MemType='" + memtype + "' and CollegeCode='" + ddl_collegename.SelectedItem.Value + "' else insert into  FT_FinDailyTransactionDetails(MemType,VendorFK,OrderCode,Credit,Debit,FinYearFK,CollegeCode)values('" + memtype + "','" + vendcode + "','" + oridvalue + "','" + tobpaid + "','" + alltamt + "','" + fincyr + "','" + ddl_collegename.SelectedItem.Value + "')";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(inseqry, "Text");

                                        insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode,deposite_bankfk) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + vendcode + "','" + memname + "','" + heaname + "','" + ledgname + "','" + 0 + "','" + tobpaid + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','','','0','','1','','','0','','','0','','','0','','','','0','" + bkcode + "')";
                                        save = d2.update_method_wo_parameter(insqry, "Text");
                                        fdt++;
                                        chequeinc = true;
                                        if (cbClgFormat.Checked)
                                        {
                                            dtvocucher = bindVoucherLedgerDet(dtvocucher, headname.SelectedItem.Text, ledname.SelectedItem.Text, credit);
                                        }
                                    }
                                }
                            }
                            if (Convert.ToInt32(payamt) != 0)
                            {
                                string bankfk = "";
                                double payamount = 0;
                                string conbank = " select SUM(ISNULL(debit,0) -ISNULL(credit,0)) as amount,BankFK,FinCashContraDetPK from FT_FinCashContraDet where IsBank=1 group by BankFK,FinCashContraDetPK";
                                DataSet dsbank = new DataSet();
                                dsbank.Clear();
                                dsbank = d2.select_method_wo_parameter(conbank, "Text");
                                if (dsbank.Tables.Count > 0)
                                {
                                    if (dsbank.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dsbank.Tables[0].Rows.Count; i++)
                                        {
                                            amount = Convert.ToString(dsbank.Tables[0].Rows[i]["amount"]);
                                            bankfk = Convert.ToString(dsbank.Tables[0].Rows[i]["BankFK"]);
                                            contrafk = Convert.ToString(dsbank.Tables[0].Rows[i]["FinCashContraDetPK"]);
                                            if (Convert.ToDouble(payamt) <= Convert.ToDouble(amount))
                                            {
                                                creditamt = payamt;
                                                payamt = "0";
                                            }
                                            else
                                            {
                                                creditamt = amount;
                                                payamount = Convert.ToDouble(payamt) - Convert.ToDouble(amount);
                                                payamt = Convert.ToString(payamount);
                                            }
                                            string insert = " update FT_FinCashContraDet set TransDate='" + date + "',TransTime='" + fnltm + "',Credit=isnull(Credit,0)+'" + creditamt + "',IsBank='" + 1 + "',Narration='" + narration + "' where IsBank='1' and BankFK='" + bkcode + "' and FinYearFK='" + fincyr + "' and  FinCashContraDetPK='" + contrafk + "'";
                                            save1 = d2.update_method_wo_parameter(insert, "Text");
                                            fcbt++;
                                        }
                                    }
                                }
                            }
                            if (Convert.ToInt32(tobpaid) != 0)
                            {
                                // insqry = "insert into FT_FinBankTransaction (TransDate,TransTime,BankFK,PayMode,DailyTransID,IsDeposited,IsCleared,IsBounced,Credit,Debit,FinYearFK) values ('" + date + "','" + fnltm + "','" + bkcode + "','" + paymode + "','" + TransCode + "','1','1','0','" + tobpaid + "','0','" + fincyr + "')";
                                insqry = "  update FT_FinBankTransaction set TransDate='" + date + "',TransTime='" + fnltm + "',PayMode='" + paymode + "',DailyTransID='" + TransCode + "',IsDeposited='1',IsCleared='1',IsBounced='0',Credit+='" + tobpaid + "' where BankFK='" + bkcode + "' and FinYearFK='" + fincyr + "'";
                                save = d2.update_method_wo_parameter(insqry, "Text");
                                fcbt++;
                            }
                            if (vendcode != "")
                            {
                                string upda = "update FT_FinInstallmentPay set InstallmentStatus='1' where App_No ='" + vendcode + "' and InstallmentDate ='" + insdt1.ToString("MM/dd/yyyy") + "'";
                                save1 = d2.update_method_wo_parameter(upda, "Text");
                            }

                            if (chkinstall.Checked == true)
                            {
                                for (int j = 0; j < gridView2.Rows.Count; j++)
                                {
                                    TextBox txtdate = (TextBox)gridView2.Rows[j].FindControl("txtdate");
                                    TextBox txtamt = (TextBox)gridView2.Rows[j].FindControl("txtamt");
                                    if (txtdate.Text.Trim() != "" && txtamt.Text.Trim() != "")
                                    {
                                        string instamt = "";
                                        string insdt = "";
                                        string instdt = "";
                                        string instmn = "";
                                        string instyr = "";
                                        string fninstdt = "";
                                        insdt = txtdate.Text.ToString();
                                        string[] strtxtdt = insdt.Split('/');
                                        if (strtxtdt.Length > 0)
                                        {
                                            instdt = strtxtdt[0].ToString();
                                            instmn = strtxtdt[1].ToString();
                                            instyr = strtxtdt[2].ToString();

                                            fninstdt = instmn + "/" + instdt + "/" + instyr;
                                        }

                                        instamt = txtamt.Text.ToString();
                                        string installqry = "insert into FT_FinInstallmentPay(memtype,App_No,MemName,InstallmentDate,InstallmentAmt,InstallmentStatus)values('" + memtype + "','" + vendcode + "','" + memname + "','" + fninstdt + "','" + instamt + "','0')";
                                        int install = d2.update_method_wo_parameter(installqry, "Text");
                                    }
                                }
                            }

                        }
                        #endregion
                        #region rb_others
                        else if (rb_others.Checked == true)
                        {
                            name = Convert.ToString(TextBox6.Text);
                            DropDownList ddl_credit = new DropDownList();
                            DropDownList ddl_debit = new DropDownList();
                            TextBox txtpaymt = new TextBox();
                            if (othname != "" && comname != "")
                            {
                                string insertoth = "if exists(select * from CO_VendorMaster where VendorName='" + name + "' and vendorType='-5' and VendorMobileNo='" + mblno + "' )update CO_VendorMaster set VendorCompName='" + comname + "', VendorAddress='" + add + "',IdentityType='" + identiy + "',IdentityNo='" + identyno + "' where   VendorName='" + othname + "' and VendorMobileNo='" + mblno + "' and vendorType='-5' else insert into CO_VendorMaster(VendorName,Vendorcode,VendorCompName,VendorAddress,VendorMobileNo,IdentityType,IdentityNo,vendorType) values ('" + othname + "','" + vendorvoucher + "','" + comname + "','" + add + "','" + mblno + "','" + identiy + "','" + identyno + "','" + -5 + "')";
                                int s1 = d2.update_method_wo_parameter(insertoth, "Text");
                                fdt++;

                                vendorPk = d2.GetFunction("select VendorPK from CO_VendorMaster where  VendorName='" + name + "' and vendorType='-5' and VendorMobileNo='" + mblno + "'");
                            }
                            if (gridView1.Rows.Count > 0)
                            {
                                for (int i = 0; i < gridView1.Rows.Count; i++)
                                {

                                    DropDownList strheader = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                                    DropDownList strledger = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                                    TextBox valtxtpayamt = (TextBox)gridView1.Rows[i].FindControl("txtpaymt");
                                    if (strheader.SelectedItem.Text.Trim() != "Select" && strledger.SelectedItem.Text.Trim() != "Select" && valtxtpayamt.Text.Trim() != "")
                                    {

                                        header = strheader.SelectedItem.Value.ToString();
                                        ledger = strledger.SelectedItem.Value.ToString();
                                        credit = Convert.ToInt32(valtxtpayamt.Text.ToString());
                                        double deductamount;
                                        double balamount = 0;
                                        double.TryParse(Convert.ToString(txt_dtsamt.Text), out deductamount);
                                        if (rb_percentage.Checked == true)
                                        {
                                            deductamount = (credit * deductamount / 100);
                                            balamount = credit - deductamount;
                                        }
                                        if (rb_amount.Checked == true)
                                        {

                                            balamount = credit - deductamount;
                                        }
                                        if (rb_percentage.Checked == true || rb_amount.Checked == true)
                                        {

                                            for (int j = 0; j < gridView3.Rows.Count; j++)
                                            {

                                                ddl_credit = (DropDownList)gridView3.Rows[j].FindControl("ddl_credit");
                                                ddl_debit = (DropDownList)gridView3.Rows[j].FindControl("ddl_debit");
                                                txtpaymt = (TextBox)gridView3.Rows[j].FindControl("txtpaymt");
                                            }
                                            ledger = ddl_credit.SelectedItem.Value;
                                            insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + 0 + "','" + deductamount + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                            save = d2.update_method_wo_parameter(insqry, "Text");
                                            ledger = ddl_debit.SelectedItem.Value;
                                            insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + deductamount + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";
                                            save = d2.update_method_wo_parameter(insqry, "Text");
                                        }

                                        ledger = strledger.SelectedItem.Value.ToString();
                                        insqry = "INSERT INTO FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,App_No,MemName,HeaderFK,LedgerFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,Narration,EntryUserCode,FinYearFK,MonthlyFeeMonth,MonthlyFeeYear,IsInstallmentPay,InstallmentNo,PayAt,PayThrough,IsExcessAdj,ExcessAdjAmt,IsDeposited,IsDepositedFully,DepositedAmt,DepositedDate,IsCollected,CollectedDate,IsBounced,BountCount,BouncedDate,IsArrearCollect,ArearFinYearFK,IsCanceled,CancelledDate,DepositBankCode,isDataImport,DataImpotNo,DataImportDate,CancelUserCode) values('" + date + "','" + fnltm + "','" + TransCode + "','" + memtype + "','" + studappno + "','" + memname + "','" + header + "','" + ledger + "','" + 0 + "','" + balamount + "','" + 0 + "','" + paymode + "','" + ddno + "','" + dddate + "','" + bkcode + "','" + bankbranch + "','" + 2 + "','" + narration + "','" + usercode + "','" + fincyr + "','0','0','','0','','','','0','1','','0','','1','','','0','','','0','','','0','','','','0')";

                                        save = d2.update_method_wo_parameter(insqry, "Text");
                                        fdt++;
                                        chequeinc = true;
                                        if (cbClgFormat.Checked)
                                        {
                                            dtvocucher = bindVoucherLedgerDet(dtvocucher, strheader.SelectedItem.Text, strledger.SelectedItem.Text, credit);
                                        }
                                    }

                                }

                            }
                            if (Convert.ToInt32(payamt) != 0)
                            {
                                string bankfk = "";
                                double payamount = 0;
                                string conbank = " select SUM(ISNULL(debit,0) -ISNULL(credit,0)) as amount,BankFK,FinCashContraDetPK from FT_FinCashContraDet where IsBank=1 group by BankFK,FinCashContraDetPK";
                                DataSet dsbank = new DataSet();
                                dsbank.Clear();
                                dsbank = d2.select_method_wo_parameter(conbank, "Text");
                                if (dsbank.Tables.Count > 0)
                                {
                                    if (dsbank.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dsbank.Tables[0].Rows.Count; i++)
                                        {
                                            amount = Convert.ToString(dsbank.Tables[0].Rows[i]["amount"]);
                                            bankfk = Convert.ToString(dsbank.Tables[0].Rows[i]["BankFK"]);
                                            contrafk = Convert.ToString(dsbank.Tables[0].Rows[i]["FinCashContraDetPK"]);
                                            if (Convert.ToDouble(payamt) <= Convert.ToDouble(amount))
                                            {
                                                creditamt = payamt;
                                                payamt = "0";
                                            }
                                            else
                                            {
                                                creditamt = amount;
                                                payamount = Convert.ToDouble(payamt) - Convert.ToDouble(amount);
                                                payamt = Convert.ToString(payamount);
                                            }
                                            string insert = " update FT_FinCashContraDet set TransDate='" + date + "',TransTime='" + fnltm + "',Credit=isnull(Credit,0)+'" + creditamt + "',IsBank='" + 1 + "',Narration='" + narration + "' where IsBank='1' and BankFK='" + bkcode + "' and FinYearFK='" + fincyr + "' and  FinCashContraDetPK='" + contrafk + "'";
                                            save1 = d2.update_method_wo_parameter(insert, "Text");
                                            fcbt++;
                                        }
                                    }
                                }
                            }
                            if (Convert.ToInt32(payamt) != 0)
                            {
                                // insqry = "insert into FT_FinBankTransaction (TransDate,TransTime,BankFK,PayMode,DailyTransID,IsDeposited,IsCleared,IsBounced,Credit,Debit,FinYearFK) values ('" + date + "','" + fnltm + "','" + bkcode + "','" + paymode + "','" + TransCode + "','1','1','0','" + payamt + "','0','" + fincyr + "')";
                                insqry = "  update FT_FinBankTransaction set TransDate='" + date + "',TransTime='" + fnltm + "',PayMode='" + paymode + "',DailyTransID='" + TransCode + "',IsDeposited='1',IsCleared='1',IsBounced='0',Credit+='" + payamt + "' where BankFK='" + bkcode + "' and FinYearFK='" + fincyr + "'";
                                save = d2.update_method_wo_parameter(insqry, "Text");
                                fcbt++;
                            }
                            if (insdt1 != null)
                            {
                                string upda = "update FT_FinInstallmentPay set InstallmentStatus='1' where App_No ='0' and InstallmentDate ='" + insdt1.ToString("MM/dd/yyyy") + "'";
                                save1 = d2.update_method_wo_parameter(upda, "Text");
                            }
                            if (chkinstall.Checked == true)
                            {
                                for (int j = 0; j < gridView2.Rows.Count; j++)
                                {

                                    TextBox txtdate = (TextBox)gridView2.Rows[j].FindControl("txtdate");
                                    TextBox txtamt = (TextBox)gridView2.Rows[j].FindControl("txtamt");
                                    if (txtdate.Text.Trim() != "" && txtamt.Text.Trim() != "")
                                    {
                                        string instamt = "";
                                        string insdt = "";
                                        string instdt = "";
                                        string instmn = "";
                                        string instyr = "";
                                        string fninstdt = "";
                                        insdt = txtdate.Text.ToString();
                                        string[] strtxtdt = insdt.Split('/');
                                        if (strtxtdt.Length > 0)
                                        {
                                            instdt = strtxtdt[0].ToString();
                                            instmn = strtxtdt[1].ToString();
                                            instyr = strtxtdt[2].ToString();

                                            fninstdt = instmn + "/" + instdt + "/" + instyr;
                                        }

                                        instamt = txtamt.Text.ToString();
                                        string installqry = "insert into FT_FinInstallmentPay(memtype,App_No,MemName,InstallmentDate,InstallmentAmt,InstallmentStatus)values('" + memtype + "','0','" + memname + "','" + fninstdt + "','" + instamt + "','0')";
                                        int install = d2.update_method_wo_parameter(installqry, "Text");
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    lbl_alert.Text = "Please Fill The Correct Values";
                    lbl_alert.Visible = true;
                    imgdiv2.Visible = true;
                }
            }
            else
            {
                lbl_alert.Text = "You Don't Have Any Amount";
                lbl_alert.Visible = true;
                imgdiv2.Visible = true;
            }
            #endregion

            #region vocuher
            if (rb_cash.Checked == true && !cbClgFormat.Checked)
            {
                if (voucherinc == true)
                {
                    string voucherno = "";
                    string voucherdate = "";
                    string colgcode = "";
                    string colgname = "";
                    string address = "";
                    string address2 = "";
                    string amt = "";
                    string mode = "";
                    string photo = "";
                    string sign = "";
                    if (rb_cash.Checked == true)
                        mode = "cash";
                    else
                        mode = "Bank";


                    voucherdate = Convert.ToString(txt_vdate.Text);
                    voucherno = Convert.ToString(TextBox9.Text);
                    amt = Convert.ToString(txt_venpayamt.Text);
                    if (rb_vendor.Checked == true)
                    {
                        TextBox venamount = (TextBox)gridven.Rows[0].FindControl("txttobepaid");
                        amt = Convert.ToString(venamount.Text);
                    }
                    colgcode = ddl_collegename.SelectedItem.Value.ToString();
                    string clgdetails = "select collname,( address1+','+address2) as Address,( address3+'-'+pincode) as addres from collinfo where college_code ='" + colgcode + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(clgdetails, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        colgname = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                        address = Convert.ToString(ds.Tables[0].Rows[0]["Address"]);
                        address2 = Convert.ToString(ds.Tables[0].Rows[0]["addres"]);
                    }

                    PdfDocument payment = new PdfDocument(PdfDocumentFormat.A4);
                    PdfPage payvoch = payment.NewPage();
                    Font heading = new Font("Book Antiqua", 10, FontStyle.Regular);
                    Font paytext = new Font("Book antiqua", 9, FontStyle.Bold);
                    Font clgtext = new Font("Book Anitqua", 10, FontStyle.Bold);
                    Font addtext = new Font("Book Anitqua", 8, FontStyle.Bold);

                    //   PdfArea headertext = new PdfArea(payment, 50, 50, 100, 100);
                    PdfArea headertext = new PdfArea(payment, 50, 50, 100, 100);
                    PdfTextArea clgname = new PdfTextArea(clgtext, Color.Black, new PdfArea(payment, 170, 30, 260, 60), ContentAlignment.MiddleCenter, colgname);
                    payvoch.Add(clgname);
                    PdfTextArea clgname1 = new PdfTextArea(addtext, Color.Black, new PdfArea(payment, 174, 45, 250, 60), ContentAlignment.MiddleCenter, address);
                    payvoch.Add(clgname1);

                    PdfTextArea add2 = new PdfTextArea(addtext, Color.Black, new PdfArea(payment, 176, 55, 250, 60), ContentAlignment.MiddleCenter, address2);
                    payvoch.Add(add2);

                    PdfTextArea clgname2 = new PdfTextArea(paytext, Color.Black, new PdfArea(payment, 220, 75, 120, 60), ContentAlignment.MiddleCenter, "Payment");
                    payvoch.Add(clgname2);
                    PdfTextArea vouchno = new PdfTextArea(heading, Color.Black, new PdfArea(payment, 50, 90, 60, 100), ContentAlignment.MiddleCenter, "Voucher No:");
                    payvoch.Add(vouchno);
                    if (voucherno != "")
                    {
                        PdfTextArea vohno = new PdfTextArea(heading, Color.Black, new PdfArea(payment, 95, 90, 80, 100), ContentAlignment.MiddleCenter, voucherno);
                        payvoch.Add(vohno);
                    }
                    PdfTextArea vouchdate = new PdfTextArea(heading, Color.Black, new PdfArea(payment, 440, 90, 60, 100), ContentAlignment.MiddleCenter, "Date:");
                    payvoch.Add(vouchdate);
                    if (voucherdate != "")
                    {
                        PdfTextArea vdate = new PdfTextArea(heading, Color.Black, new PdfArea(payment, 480, 90, 60, 100), ContentAlignment.MiddleCenter, voucherdate);
                        payvoch.Add(vdate);
                    }


                    if (cbledgdet.Checked == false)
                    {
                        #region table
                        //    PdfTable tbl = payment.NewTable(heading, 4, 2, 6);
                        //Font heading = new Font("Book Antiqua", 10, FontStyle.Regular);
                        Font tblhd = new Font("Book Antiqua", 6, FontStyle.Regular);
                        PdfTable tbl = payment.NewTable(heading, 5, 2, 6);
                        tbl.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        tbl.VisibleHeaders = false;

                        tbl.Cell(0, 0).SetContent("Paid To Mr/Mrs/Ms");
                        tbl.Cell(0, 0).SetFont(heading);
                        tbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        tbl.Columns[0].SetWidth(20);
                        if (name != "" && name != null)
                        {
                            tbl.Cell(0, 1).SetContent(name);
                            tbl.Cell(0, 1).SetFont(heading);
                            tbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        }
                        double totamt = 0;
                        tbl.Cell(1, 0).SetContent(amt);
                        tbl.Cell(1, 0).SetFont(heading);
                        tbl.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        totamt = Convert.ToDouble(amt);
                        NumToText(totamt);
                        if (amt != "" && amt != null)
                        {

                            tbl.Cell(1, 1).SetContent(NumToText(totamt));
                            tbl.Cell(1, 1).SetFont(heading);
                            tbl.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        }
                        tbl.Cell(2, 0).SetContent("Mode");
                        tbl.Cell(2, 0).SetFont(heading);
                        tbl.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        if (mode != "" && mode != null)
                        {
                            tbl.Cell(2, 1).SetContent(mode);
                            tbl.Cell(2, 1).SetFont(heading);
                            tbl.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        }
                        tbl.Cell(3, 0).SetContent("Narration");
                        tbl.Cell(3, 0).SetFont(heading);
                        tbl.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        if (narration != "" && narration != null)
                        {
                            tbl.Cell(3, 1).SetContent(narration);
                            tbl.Cell(3, 1).SetFont(heading);
                            tbl.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        }

                        string txtsgn = "\n\n\n Prepared by                                       Authorised by                                                                 Received Signature";

                        tbl.Cell(4, 0).SetContent(txtsgn);
                        //  tbl.Cell(4, 1).SetContent("\n\n\nReceived Signature");
                        tbl.Cell(4, 0).SetFont(heading);
                        tbl.Cell(4, 0).SetContentAlignment(ContentAlignment.BottomLeft);

                        //tbl.Cell(4, 0).SetContent("Prepared by");
                        //tbl.Cell(4, 0).SetFont(tblhd);
                        //tbl.Cell(4, 0).SetContentAlignment(ContentAlignment.BottomLeft);

                        //tbl.Cell(4, 1).SetContent("Authorized by");
                        //tbl.Cell(4, 1).SetFont(tblhd);
                        //tbl.Cell(4, 1).SetContentAlignment(ContentAlignment.BottomLeft);
                        //  tbl.Cell(3, 0).SetCellPadding(30);
                        tbl.Cell(4, 0).ColSpan = 2;
                        if (photo != "" && photo != null)
                        {
                            tbl.Cell(4, 1).SetContent(photo);
                            tbl.Cell(4, 1).SetFont(heading);
                            tbl.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleRight);

                        }
                        PdfTablePage vouchtbl = tbl.CreateTablePage(new PdfArea(payment, 30, 150, 530, 400));
                        payvoch.Add(vouchtbl);
                        #endregion
                    }
                    else
                    {
                        #region ledger detail table
                        if (dtvocucher.Rows.Count > 0)
                        {
                            int rowCnt = Convert.ToInt32(dtvocucher.Rows.Count);
                            rowCnt += 6;
                            Font tblhd = new Font("Book Antiqua", 6, FontStyle.Regular);
                            PdfTable tbl = payment.NewTable(heading, rowCnt, 3, 6);
                            tbl.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            tbl.VisibleHeaders = false;

                            tbl.Cell(0, 0).SetContent("Paid To Mr/Mrs/Ms");
                            tbl.Cell(0, 0).SetFont(heading);
                            tbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tbl.Columns[0].SetWidth(20);
                            tbl.Cell(0, 1).ColSpan = 2;
                            if (name != "" && name != null)
                            {
                                tbl.Cell(0, 1).SetContent(name);
                                tbl.Cell(0, 1).SetFont(heading);
                                tbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            }
                            int rowval = 1;
                            int padval = 0;
                            for (int i = 0; i < dtvocucher.Rows.Count; i++)
                            {
                                if (rowCnt >= rowval)
                                {
                                    padval += 15;
                                    tbl.Cell(rowval, 0).SetContent(dtvocucher.Rows[i][1]);
                                    tbl.Cell(rowval, 0).SetFont(heading);
                                    tbl.Cell(rowval, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                    tbl.Cell(rowval, 1).SetContent(dtvocucher.Rows[i][2]);
                                    tbl.Cell(rowval, 1).SetFont(heading);
                                    tbl.Cell(rowval, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                                    tbl.Cell(rowval, 2).SetContent(dtvocucher.Rows[i][3]);
                                    tbl.Cell(rowval, 2).SetFont(heading);
                                    tbl.Cell(rowval, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                    rowval++;
                                }
                            }
                            double totamt = 0;
                            totamt = Convert.ToDouble(amt);
                            //tbl.Cell(rowval, 0).SetContent(NumToText(totamt));
                            tbl.Cell(rowval, 0).SetContent("Total");
                            tbl.Cell(rowval, 0).SetFont(heading);
                            tbl.Cell(rowval, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                            string txtfnt = NumToText(totamt) + " Only";
                            tbl.Cell(rowval, 0).ColSpan = 2;
                            if (amt != "" && amt != null)
                            {
                                tbl.Cell(rowval, 2).SetContent(totamt);
                                tbl.Cell(rowval, 2).SetFont(heading);
                                tbl.Cell(rowval, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                            }
                            rowval++;
                            //totamt = Convert.ToDouble(amt);
                            ////tbl.Cell(rowval, 0).SetContent(NumToText(totamt));
                            //tbl.Cell(rowval, 0).SetContent("Rupees");
                            //tbl.Cell(rowval, 0).SetFont(heading);
                            //tbl.Cell(rowval, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            //tbl.Cell(rowval, 1).ColSpan = 3;
                            if (amt != "" && amt != null)
                            {
                                tbl.Cell(rowval, 0).SetContent(txtfnt);
                                tbl.Cell(rowval, 0).SetFont(heading);
                                tbl.Cell(rowval, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                tbl.Cell(rowval, 0).ColSpan = 3;
                            }


                            rowval++;
                            tbl.Cell(rowval, 0).SetContent("Mode");
                            tbl.Cell(rowval, 0).SetFont(heading);
                            tbl.Cell(rowval, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tbl.Cell(rowval, 1).ColSpan = 2;
                            if (mode != "" && mode != null)
                            {
                                tbl.Cell(rowval, 1).SetContent(mode);
                                tbl.Cell(rowval, 1).SetFont(heading);
                                tbl.Cell(rowval, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            }
                            rowval++;
                            tbl.Cell(rowval, 0).SetContent("Narration");
                            tbl.Cell(rowval, 0).SetFont(heading);
                            tbl.Cell(rowval, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tbl.Cell(rowval, 1).ColSpan = 2;
                            if (narration != "" && narration != null)
                            {
                                tbl.Cell(rowval, 1).SetContent(narration);
                                tbl.Cell(rowval, 1).SetFont(heading);
                                tbl.Cell(rowval, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            }

                            string txtsgn = "\n\n\n Prepared by                                       Authorised by                                                                 Received Signature";
                            rowval++;
                            tbl.Cell(rowval, 0).SetContent(txtsgn);
                            tbl.Cell(rowval, 0).SetFont(heading);
                            tbl.Cell(rowval, 0).SetContentAlignment(ContentAlignment.BottomLeft);
                            tbl.Cell(rowval, 0).ColSpan = 3;
                            if (photo != "" && photo != null)
                            {
                                tbl.Cell(rowval, 2).SetContent(photo);
                                tbl.Cell(rowval, 2).SetFont(heading);
                                tbl.Cell(rowval, 2).SetContentAlignment(ContentAlignment.MiddleRight);

                            }
                            PdfTablePage vouchtbl = tbl.CreateTablePage(new PdfArea(payment, 30, 165, 530, 400));
                            payvoch.Add(vouchtbl);
                        }
                        #endregion
                    }


                    payvoch.SaveToDocument();

                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "Payment" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                        Response.Buffer = true;
                        Response.Clear();
                        payment.SaveToFile(szPath + szFile);

                        Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");

                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Voucher Can not be Generated";
                    }
                }
            }
            #endregion
            #region cheque
            if (rb_bank.Checked == true && !cbClgFormat.Checked)
            {
                if (chequeinc == true)
                {
                    double totamt = 0;
                    string dtt = "";
                    string mnth = "";
                    string yrr = "";
                    string amt = Convert.ToString(txt_venpayamt.Text);
                    if (rb_vendor.Checked == true)
                    {
                        TextBox venamount = (TextBox)gridven.Rows[0].FindControl("txttobepaid");
                        amt = Convert.ToString(venamount.Text);
                    }
                    string cheqdate = Convert.ToString(txt_vdate.Text);
                    string[] dateSplit = cheqdate.Split('/');
                    string day = dateSplit[0].ToString();
                    string mon = dateSplit[1];
                    string year = dateSplit[2];
                    string da1 = "0";
                    string da2 = "";
                    string m1 = "0";
                    string m2 = "";
                    if (day.Length > 1)
                    {
                        da1 = day.Substring(0, 1);
                        da2 = day.Substring(1, 1);
                    }
                    else
                    {
                        da2 = day;
                    }
                    if (mon.Length > 1)
                    {
                        m1 = mon.Substring(0, 1);
                        m2 = mon.Substring(1, 1);
                    }
                    else
                    {
                        m2 = mon;
                    }
                    string y1 = year.Substring(0, 1);
                    string y2 = year.Substring(1, 1);
                    string y3 = year.Substring(2, 1);
                    string y4 = year.Substring(3, 1);

                    PdfDocument paymentcheque = new PdfDocument(PdfDocumentFormat.InCentimeters(20.4, 9.3));
                    PdfPage paychq = paymentcheque.NewPage();
                    Font general = new Font("Book Anitqua", 10, FontStyle.Regular);
                    ///date/////
                    //PdfTextArea date1 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 15.6, 1.3, 16, 0.9), ContentAlignment.TopRight, da1);
                    //paychq.Add(date1);
                    PdfTextArea date1 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 442, 23, 442, 23), ContentAlignment.MiddleLeft, da1);
                    paychq.Add(date1);
                    //PdfTextArea date2 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 16.1, 1, 16.5, 1), ContentAlignment.TopRight, da2);
                    //paychq.Add(date2);
                    PdfTextArea date2 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 456, 23, 468, 23), ContentAlignment.MiddleLeft, da2);
                    paychq.Add(date2);
                    //PdfTextArea mnth1 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 16.6, 1, 17, 1), ContentAlignment.TopRight, m1);
                    //paychq.Add(mnth1);
                    PdfTextArea mnth1 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 471, 23, 482, 23), ContentAlignment.MiddleLeft, m1);
                    paychq.Add(mnth1);

                    //PdfTextArea mnth2 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 17.1, 1, 17.4, 1), ContentAlignment.TopRight, m2);
                    //paychq.Add(mnth2);
                    PdfTextArea mnth2 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 485, 23, 493, 23), ContentAlignment.MiddleLeft, m2);
                    paychq.Add(mnth2);
                    //PdfTextArea yr1 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 17.6, 1, 18, 1), ContentAlignment.TopRight, y1);
                    //paychq.Add(yr1);
                    PdfTextArea yr1 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 499, 23, 510, 23), ContentAlignment.MiddleLeft, y1);
                    paychq.Add(yr1);
                    //PdfTextArea yr2 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 18.1, 1, 18.5, 1), ContentAlignment.TopRight, y2);
                    //paychq.Add(yr2);
                    PdfTextArea yr2 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 513, 23, 524, 23), ContentAlignment.MiddleLeft, y2);
                    paychq.Add(yr2);
                    //PdfTextArea yr3 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 18.6, 1, 19, 1), ContentAlignment.TopRight, y3);
                    //paychq.Add(yr3);
                    PdfTextArea yr3 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 527, 23, 539, 23), ContentAlignment.MiddleLeft, y3);
                    paychq.Add(yr3);
                    //PdfTextArea yr4 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 19.1, 1, 19.5, 1), ContentAlignment.TopRight, y4);
                    //paychq.Add(yr4);
                    PdfTextArea yr4 = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 541, 23, 553, 23), ContentAlignment.MiddleLeft, y4);
                    paychq.Add(yr4);


                    ////////////////pay////////////////
                    //PdfTextArea pay = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 2, 2.5, 17, 3), ContentAlignment.TopRight, name);
                    //paychq.Add(pay);
                    PdfTextArea pay = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 57, 62, 283, 62), ContentAlignment.MiddleLeft, name);
                    paychq.Add(pay);
                    /////////////////////rupees//////////
                    totamt = Convert.ToDouble(amt);
                    NumToText(totamt);
                    //PdfTextArea rupees = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 2.5, 3.5, 14, 3.5), ContentAlignment.TopRight, NumToText(totamt));
                    //paychq.Add(rupees);
                    PdfTextArea rupees = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 71, 91, 283, 91), ContentAlignment.MiddleLeft, NumToText(totamt));
                    paychq.Add(rupees);
                    /////////////////////rs.00000////////////
                    //PdfTextArea rs = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 15.8, 4.3, 19.3, 4.3), ContentAlignment.TopRight, amt);
                    //paychq.Add(rs);
                    PdfTextArea rs = new PdfTextArea(general, Color.Black, new PdfArea(paymentcheque, 445, 119, 510, 119), ContentAlignment.MiddleLeft, amt);
                    paychq.Add(rs);


                    paychq.SaveToDocument();

                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "Payment" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                        Response.Buffer = true;
                        Response.Clear();
                        paymentcheque.SaveToFile(szPath + szFile);

                        Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");

                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Cheque Can not be Generated";
                    }
                }
            }
            #endregion

            #region  paavai fourteen formate
            if (cbClgFormat.Checked)
            {
                paavaiVoucher(dtvocucher, memTypeNo, "Voucher", Convert.ToString(TextBox9.Text), Convert.ToString(txt_vdate.Text));
            }
            #endregion

            if (voucherinc == true || chequeinc == true)
            {
                string uprec = "update FM_FinCodeSettings set VouchStNo=" + ViewState["receno"] + "+1 where IsHeader=0 and FinYearFK='" + fincyr + "' and collegecode ='" + ddl_collegename.SelectedItem.Value + "' and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK='" + fincyr + "' and collegecode ='" + ddl_collegename.SelectedItem.Value + "'  and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                int uprecno = d2.update_method_wo_parameter(uprec, "Text");
                TextBox9.Text = generateReceiptNo();
            }
            if (fdt > 0)
            {
                if (fcbt > 0)
                {
                    if (voucherinc == true || chequeinc == true)
                    {
                        ddlapyledg_SelectedIndexChanged(sender, e);

                        cleartext();
                        lbl_alert.Text = "Saved Successfully";
                        lbl_alert.Visible = true;
                        imgdiv2.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //  d2.sendErrorMail(ex, collegecode1, "DirectPayment.aspx");
        }
    }
    protected DataTable bindVoucherLedgerDet(DataTable dtvocucher, string hedtxt, string ledtxt, int credit)
    {
        DataRow drvoucher;
        try
        {
            if (dtvocucher.Columns.Count > 0)
            {
                drvoucher = dtvocucher.NewRow();
                drvoucher["Sno"] = Convert.ToString(dtvocucher.Rows.Count + 1);
                drvoucher["Header"] = Convert.ToString(hedtxt);
                drvoucher["Ledger"] = Convert.ToString(ledtxt);
                drvoucher["Amount"] = Convert.ToString(credit);
                dtvocucher.Rows.Add(drvoucher);
            }
        }
        catch { }
        return dtvocucher;
    }

    public void cleartext()
    {
        try
        {
            txt_rollno.Text = "";
            txt_name.Text = "";
            txt_batch.Text = "";
            txt_degr.Text = "";
            txt_dept.Text = "";
            txt_sem.Text = "";
            txt_sec.Text = "";

            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox5.Text = "";
            ImageButton3.Visible = false;
            txt_venname.Text = "";
            txt_ccont.Text = "";
            txt_cdesi.Text = "";
            txt_cadd.Text = "";
            //  txt_vencode.Text = "";
            TextBox12.Text = "";
            TextBox7.Text = "";
            bindGrid();
            bindGrid1();
            txt_venpayamt.Text = "";
            txt_narr.Text = "";
            txt_ventotpayamt.Text = "";
            txt_lbl_vencurbal.Text = "";
            txt_acno.Text = "";
            txt_acname.Text = "";
            txt_bnkname.Text = "";
            txt_branch.Text = "";
            txt_amt.Text = "";
            txt_chqno.Text = "";
            lbl_instal.Visible = false;
            TextBox1.Visible = false;
            divbank.Visible = false;
            chkinstall.Checked = false;
            imb_studpht.Visible = false;
            txt_stftype.Text = "";
            txt_stfcat.Text = "";
            txt_stfjn.Text = "";
            lbl_er.Visible = false;

            TextBox6.Text = "";
            txt_othcname.Text = "";
            txt_othadd.Text = "";
            txt_mblno.Text = "";
            TextBox8.Text = "";
            txtavailAmt.Text = "";
            txt_dtsamt.Text = "";

            chk_deposite.Checked = false;
            chk_insexcess.Checked = false;
            cbledgdet.Checked = false;
            paidDiv.Visible = false;
        }
        catch
        { }

    }
    protected string NumToText(double totamt)
    {

        int inputNo = Convert.ToInt32(totamt.ToString());

        if (inputNo == 0)
            return "Zero";

        int[] numbers = new int[4];
        int first = 0;
        int u, h, t;
        System.Text.StringBuilder strResult = new System.Text.StringBuilder();

        if (inputNo < 0)
        {
            strResult.Append("Minus ");
            inputNo = -inputNo;
        }

        string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
        string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
        string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
            "Seventy ","Eighty ", "Ninety "};
        string[] words3 = { "Thousand ", "Lakh ", "Crore " };

        numbers[0] = inputNo % 1000; // units
        numbers[1] = inputNo / 1000;
        numbers[2] = inputNo / 100000;
        numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
        numbers[3] = inputNo / 10000000; // crores
        numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

        for (int i = 3; i > 0; i--)
        {
            if (numbers[i] != 0)
            {
                first = i;
                break;
            }
        }
        for (int i = first; i >= 0; i--)
        {
            if (numbers[i] == 0) continue;
            u = numbers[i] % 10; // ones
            t = numbers[i] / 10;
            h = numbers[i] / 100; // hundreds
            t = t - 10 * h; // tens
            if (h > 0) strResult.Append(words0[h] + "Hundred ");
            if (u > 0 || t > 0)
            {
                if (h > 0 || i == 0) strResult.Append(" and ");
                if (t == 0)
                    strResult.Append(words0[u]);
                else if (t == 1)
                    strResult.Append(words1[u]);
                else
                    strResult.Append(words2[t - 2] + words0[u]);
            }
            if (i != 0) strResult.Append(words3[i - 1]);
        }

        string substring = "";
        substring = strResult.ToString();
        return substring.ToString().TrimEnd();

    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public void ddl_headername_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //string strdname = "";
        //for (int i = 0; i < gridView1.Rows.Count; i++)
        //{
        //    DropDownList strhdname = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
        //    strdname = strhdname.SelectedItem.Value.ToString();
        //}
        //string englisquery = "select LedgerPK,LedgerName from LedgerMasterDet where CollegeCode='" + collegecode1 + "' and HeaderFK ='" + strdname + "'";
        //ds.Clear();
        //ds = d2.select_method_wo_parameter(englisquery, "Text");
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    for (int j = 0; j < gridView1.Rows.Count; j++)
        //    {

        //        (gridView1.Rows[j].FindControl("ddl_ledgername") as DropDownList).DataSource = ds;
        //        (gridView1.Rows[j].FindControl("ddl_ledgername") as DropDownList).DataTextField = "LedgerName";
        //        (gridView1.Rows[j].FindControl("ddl_ledgername") as DropDownList).DataValueField = "LedgerPK";
        //        (gridView1.Rows[j].FindControl("ddl_ledgername") as DropDownList).DataBind();

        //        (gridView1.Rows[j].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");
        //    }
        //}

    }

    protected void gridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[1].Attributes["Onchange"] = Page.ClientScript.GetPostBackEventReference(gridView1, "Select$" + e.Row.RowIndex);


            }
        }
        catch
        {

        }
    }
    protected void gridView3_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[1].Attributes["Onchange"] = Page.ClientScript.GetPostBackEventReference(gridView3, "Select$" + e.Row.RowIndex);


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
        if (ddl_collegename.Items.Count > 0)
        {
            collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
        }
        int n = Convert.ToInt32(e.CommandArgument);
        DropDownList strhdname = (DropDownList)gridView1.Rows[n].FindControl("ddl_headername");
        (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Clear();
        // string englisquery = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode='" + collegecode1 + "' and LedgerMode=1 and HeaderFK ='" + strhdname.SelectedItem.Value + "' order by isnull(priority,1000),ledgerName asc";

        string englisquery = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + "   and L.CollegeCode = " + collegecode1 + "  and LedgerMode=1 and l.HeaderFK ='" + strhdname.SelectedItem.Value + "'   order by isnull(l.priority,1000), l.ledgerName asc ";
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
    protected void gridView3_OnRowDataBound(object sender, GridViewCommandEventArgs e)
    {
        lbl_er.Visible = false;
        string strdname = "";
        if (ddl_collegename.Items.Count > 0)
        {
            collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
        }
        int n = Convert.ToInt32(e.CommandArgument);
        // DropDownList strhdname = (DropDownList)gridView1.Rows[n].FindControl("ddl_headername");
        (gridView3.Rows[n].FindControl("ddl_credit") as DropDownList).Items.Clear();
        // (gridView3.Rows[n].FindControl("ddl_debit") as DropDownList).Items.Clear();
        // string englisquery = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode='" + collegecode1 + "' and LedgerMode=1 and HeaderFK ='" + strhdname.SelectedItem.Value + "' order by isnull(priority,1000),ledgerName asc";

        string englisquery = "select distinct LedgerName,LedgerPK from FM_LedgerMaster WHERE CollegeCode='" + collegecodestat + "' and ledgermode='0'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(englisquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {

            (gridView3.Rows[n].FindControl("ddl_credit") as DropDownList).DataSource = ds;
            (gridView3.Rows[n].FindControl("ddl_credit") as DropDownList).DataTextField = "LedgerName";
            (gridView3.Rows[n].FindControl("ddl_credit") as DropDownList).DataValueField = "LedgerPK";
            (gridView3.Rows[n].FindControl("ddl_credit") as DropDownList).DataBind();


            //   (gridView3.Rows[n].FindControl("ddl_credit") as DropDownList).Items.Insert(0, "Select");
            lbl_er.Visible = false;
        }
        else
        {
            (gridView3.Rows[n].FindControl("ddl_credit") as DropDownList).Items.Insert(0, "Select");
        }
        (gridView3.Rows[n].FindControl("ddl_debit") as DropDownList).Items.Clear();
        string sqlq = "select distinct LedgerName,LedgerPK from FM_LedgerMaster WHERE CollegeCode='" + collegecodestat + "' and ledgermode='1'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(sqlq, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {

            (gridView3.Rows[n].FindControl("ddl_debit") as DropDownList).DataSource = ds;
            (gridView3.Rows[n].FindControl("ddl_debit") as DropDownList).DataTextField = "LedgerName";
            (gridView3.Rows[n].FindControl("ddl_debit") as DropDownList).DataValueField = "LedgerPK";
            (gridView3.Rows[n].FindControl("ddl_debit") as DropDownList).DataBind();

            //  (gridView3.Rows[n].FindControl("ddl_debit") as DropDownList).Items.Insert(0, "Select");
            lbl_er.Visible = false;
        }
        else
        {
            (gridView3.Rows[n].FindControl("ddl_debit") as DropDownList).Items.Insert(0, "Select");
        }
    }
    protected void gridledgerload(string hedid, int n)
    {
        try
        {
            lbl_er.Visible = false;
            string strdname = "";
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string englisquery = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + "   and L.CollegeCode = " + collegecode1 + "  and LedgerMode=1 and l.HeaderFK ='" + hedid + "'   order by isnull(l.priority,1000), l.ledgerName asc ";
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
    protected void gridledgerloaddts(string hedid, int n)
    {
        try
        {
            lbl_er.Visible = false;
            string strdname = "";
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string englisquery = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + "   and L.CollegeCode = " + collegecode1 + "  and LedgerMode=1 and l.HeaderFK ='" + hedid + "'   order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(englisquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                (gridView3.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataSource = ds;
                (gridView3.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataTextField = "LedgerName";
                (gridView3.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataValueField = "LedgerPK";
                (gridView3.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataBind();

                (gridView3.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");
                lbl_er.Visible = false;
            }
            else
            {
                (gridView3.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");
            }
        }
        catch { }
    }
    protected void gridven_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes["Onchange"] = Page.ClientScript.GetPostBackEventReference(gridven, "Select$" + e.Row.RowIndex);
            }
        }
        catch
        {
        }
    }
    protected void gridven_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        lbl_er.Visible = false;
        int m = Convert.ToInt32(e.CommandArgument);
        DropDownList strheader = (DropDownList)gridven.Rows[m].FindControl("ddlheader");
        (gridven.Rows[m].FindControl("ddlledger") as DropDownList).Items.Clear();
        // string selqry = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode='" + collegecode1 + "' and LedgerMode=1 and HeaderFK ='" + strheader.SelectedItem.Value + "' order by isnull(priority,1000),ledgerName asc";


        string selqry = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + "  and L.CollegeCode = " + collegecode1 + "  and LedgerMode=1 and l.HeaderFK ='" + strheader.SelectedItem.Value + "'   order by isnull(l.priority,1000), l.ledgerName asc ";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selqry, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            (gridven.Rows[m].FindControl("ddlledger") as DropDownList).DataSource = ds;
            (gridven.Rows[m].FindControl("ddlledger") as DropDownList).DataTextField = "LedgerName";
            (gridven.Rows[m].FindControl("ddlledger") as DropDownList).DataValueField = "LedgerPK";
            (gridven.Rows[m].FindControl("ddlledger") as DropDownList).DataBind();

            (gridven.Rows[m].FindControl("ddlledger") as DropDownList).Items.Insert(0, "Select");
        }
        else
        {
            (gridven.Rows[m].FindControl("ddlledger") as DropDownList).Items.Insert(0, "Select");
        }
    }

    protected void btnplus_Click(object sender, EventArgs e)
    {
        imgdiv5.Visible = true;
        panel_header.Visible = true;
    }
    protected void btnminus_Click(object sender, EventArgs e)
    {

        if (ddl_group.Items.Count > 0)
        {
            string sql = "delete from textvaltable where TextCode='" + ddl_group.SelectedItem.Value.ToString() + "' and TextCriteria='QHead' and college_code='" + ddl_collegename.SelectedItem.Value + "' ";
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
            loaddesc();
        }

        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
    }
    public void loaddesc()
    {
        ddl_group.Items.Clear();
        ds.Tables.Clear();

        string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='QHead' and college_code ='" + ddl_collegename.SelectedItem.Value + "'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_group.DataSource = ds;
            ddl_group.DataTextField = "TextVal";
            ddl_group.DataValueField = "TextCode";
            ddl_group.DataBind();
            // ddl_group.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        else
        {
            //ddl_group.Items.Insert(0, new ListItem("--Select--", "0"));
        }

    }

    protected void btn_addheader_Click(object sender, EventArgs e)
    {
        try
        {
            //string header = txt_header.Text.ToString();

            txt_header.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt_header.Text);

            if (txt_header.Text != "")
            {
                //Criteria Header - QHead,Range QRang


                string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_header.Text + "' and TextCriteria ='QHead' and college_code ='" + ddl_collegename.SelectedItem.Value + "') update TextValTable set TextVal ='" + txt_header.Text + "' where TextVal ='" + txt_header.Text + "' and TextCriteria ='QHead' and college_code ='" + ddl_collegename.SelectedItem.Value + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_header.Text + "','QHead','" + ddl_collegename.SelectedItem.Value + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved sucessfully";
                    txt_header.Text = "";

                    panel_header.Visible = false;
                    imgdiv5.Visible = false;
                }
                loaddesc();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Enter the HeaderName";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_exitheader_Click(object sender, EventArgs e)
    {
        imgdiv5.Visible = false;
    }
    public void imb_studpht_Click(object sender, EventArgs e)
    {
        //string rollno = txt_rollno.Text.ToString();

        //imb_studpht.ImageUrl = "~/Handler4.ashx?rollno=" + rollno;
    }


    public void btn_history_OnClick(object sender, EventArgs e)
    {
        // history.Visible = true;
        bindhistory();
    }
    public void bindhistory()
    {
        try
        {
            string orderid = "";
            string select = "";
            string selqry = "";
            string memtype = "";
            string vendorfk = "";
            string debit = "";
            string credit = "";
            string date = "";
            DataSet dsht = new DataSet();
            DataTable dt = new DataTable();
            int height = 0;
            if (gridven.Rows.Count > 0)
            {
                for (int i = 0; i < gridven.Rows.Count; i++)
                {
                    TextBox txtorid = (TextBox)gridven.Rows[i].FindControl("txtorderid");
                    orderid = Convert.ToString(txtorid.Text);
                    if (orderid != "")
                    {
                        select = "select memtype,vendorFK,debit,credit from FT_FinDailyTransactionDetails where OrderCode='" + orderid + "' ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(select, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            memtype = Convert.ToString(ds.Tables[0].Rows[0]["memtype"]);
                            vendorfk = Convert.ToString(ds.Tables[0].Rows[0]["vendorFK"]);
                            debit = Convert.ToString(ds.Tables[0].Rows[0]["debit"]);
                            credit = Convert.ToString(ds.Tables[0].Rows[0]["credit"]);

                            selqry = "SELECT VendorCode,VendorCompName,OrderCode,TransDate,TransCode,PayMode,SUM(D.Debit) as debit,SUM(M.Credit) as Credit FROM FT_FinDailyTransaction M,FT_FinDailyTransactionDetails D,CO_VendorMaster V WHERE M.App_No = D.VendorFK and d.VendorFK=v.VendorPK and App_No='" + vendorfk + "' and OrderCode='" + orderid + "' and m.memtype='" + memtype + "' GROUP BY TransDate,VendorCode,VendorCompName,OrderCode,TransCode,PayMode";
                            dsht.Clear();
                            dsht = d2.select_method_wo_parameter(selqry, "Text");


                            dt.Columns.Add("Sno");
                            dt.Columns.Add("Order Id");
                            dt.Columns.Add("Date");
                            dt.Columns.Add("Voucher No");
                            dt.Columns.Add("Mode");
                            dt.Columns.Add("Debit");
                            dt.Columns.Add("Credit");
                            DataRow drht;

                            if (dsht.Tables[0].Rows.Count > 0)
                            {
                                date = Convert.ToString(dsht.Tables[0].Rows[0]["TransDate"]);
                                string[] sptdt = date.Split(' ');
                                if (sptdt.Length > 0)
                                {
                                    date = sptdt[0].ToString();
                                }
                                for (int ii = 0; ii < dsht.Tables[0].Rows.Count; ii++)
                                {
                                    drht = dt.NewRow();
                                    drht["Sno"] = ii + 1;
                                    drht["Order Id"] = Convert.ToString(dsht.Tables[0].Rows[ii]["OrderCode"]);
                                    drht["Date"] = date;
                                    drht["Voucher No"] = Convert.ToString(dsht.Tables[0].Rows[ii]["TransCode"]);
                                    drht["Mode"] = Convert.ToString(dsht.Tables[0].Rows[ii]["PayMode"]);
                                    drht["Debit"] = Convert.ToString(dsht.Tables[0].Rows[ii]["debit"]);
                                    drht["Credit"] = Convert.ToString(dsht.Tables[0].Rows[ii]["Credit"]);
                                    dt.Rows.Add(drht);
                                }
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {

                            gridhty.DataSource = dt;
                            gridhty.DataBind();
                            for (int j = 0; j < gridhty.Rows.Count; j++)
                            {
                                //(gridhty.Rows[j].FindControl("txtorderid") as TextBox).Text = Convert.ToString(j + 1);
                                (gridhty.Rows[j].FindControl("txtorderid") as TextBox).Text = Convert.ToString(dsht.Tables[0].Rows[0]["OrderCode"]);

                                (gridhty.Rows[j].FindControl("txthtydate") as TextBox).Text = date;
                                (gridhty.Rows[j].FindControl("txtvocuherno") as TextBox).Text = Convert.ToString(dsht.Tables[0].Rows[0]["TransCode"]);
                                (gridhty.Rows[j].FindControl("txtmode") as TextBox).Text = Convert.ToString(dsht.Tables[0].Rows[0]["PayMode"]);
                                (gridhty.Rows[j].FindControl("txtdebit") as TextBox).Text = Convert.ToString(dsht.Tables[0].Rows[0]["debit"]);
                                (gridhty.Rows[j].FindControl("txtcredit") as TextBox).Text = Convert.ToString(dsht.Tables[0].Rows[0]["Credit"]);
                                TextBox txtordid = (TextBox)gridhty.Rows[j].FindControl("txtorderid");
                                txtordid.ReadOnly = true;
                                TextBox txthtydate = (TextBox)gridhty.Rows[j].FindControl("txthtydate");
                                txthtydate.ReadOnly = true;
                                TextBox txtvouchno = (TextBox)gridhty.Rows[j].FindControl("txtvocuherno");
                                txtvouchno.ReadOnly = true;
                                TextBox txtmode = (TextBox)gridhty.Rows[j].FindControl("txtmode");
                                txtmode.ReadOnly = true;
                                TextBox txtdebit = (TextBox)gridhty.Rows[j].FindControl("txtdebit");
                                txtdebit.ReadOnly = true;
                                TextBox txtcrdit = (TextBox)gridhty.Rows[j].FindControl("txtcredit");
                                txtcrdit.ReadOnly = true;
                                height += 30;
                            }
                            gridhty.Height = height;
                            gridhty.Visible = true;
                            history.Visible = true;
                        }
                        else
                        {
                            history.Visible = false;
                            gridhty.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "There is No Order Id";
                            lbl_alert.Visible = true;
                        }
                    }
                    else
                    {
                        history.Visible = false;
                        gridhty.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please Fill The Columns";
                        lbl_alert.Visible = true;
                    }
                }
            }
            else
            {
                history.Visible = false;
                gridhty.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Found";
                lbl_alert.Visible = true;
            }
        }
        catch
        {
        }

    }
    public void btnhtok_OnClick(object sender, EventArgs e)
    {
        history.Visible = false;
        gridhty.Visible = false;
    }
    public void TextBox12_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string orderid = "";
            string selqry = "";
            orderid = Convert.ToString(TextBox12.Text);
            string vendorfk = d2.GetFunction("select vendorFK from FT_FinDailyTransactionDetails where OrderCode='" + orderid + "'");
            if (orderid != "")
            {
                selqry = "select Debit,credit,OrderCode,(ISNULL (Debit ,0))-(ISNULL(credit,0)) as balance from FT_FinDailyTransactionDetails where OrderCode='" + orderid + "' and VendorFK='" + vendorfk + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selqry, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < gridven.Rows.Count; j++)
                        {
                            (gridven.Rows[j].FindControl("txttotamt") as TextBox).Text = Convert.ToString(ds.Tables[0].Rows[i]["Debit"]);
                            (gridven.Rows[j].FindControl("txttotamt") as TextBox).ReadOnly = true;
                            (gridven.Rows[j].FindControl("txtpaidamt") as TextBox).Text = Convert.ToString(ds.Tables[0].Rows[i]["credit"]);
                            (gridven.Rows[j].FindControl("txtpaidamt") as TextBox).ReadOnly = true;
                            (gridven.Rows[j].FindControl("txtbal") as TextBox).Text = Convert.ToString(ds.Tables[0].Rows[i]["balance"]);
                            (gridven.Rows[j].FindControl("txtbal") as TextBox).ReadOnly = true;
                            (gridven.Rows[j].FindControl("txtorderid") as TextBox).Text = Convert.ToString(ds.Tables[0].Rows[i]["OrderCode"]);
                            (gridven.Rows[j].FindControl("txtorderid") as TextBox).ReadOnly = true;

                        }
                    }

                }
                else
                {
                    bindgridven();
                }
            }
            else
            {
                bindgridven();
            }
        }
        catch
        {
        }
    }

    public void loadsetting()
    {
        try
        {
            ListItem list1 = new ListItem("Roll No", "0");
            ListItem list2 = new ListItem("Reg No", "1");
            ListItem list3 = new ListItem("Admission No", "2");
            ListItem list4 = new ListItem("App No", "3");

            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + ddl_collegename.SelectedItem.Value + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + ddl_collegename.SelectedItem.Value + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code ='" + ddl_collegename.SelectedItem.Value + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code ='" + ddl_collegename.SelectedItem.Value + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list4);
            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(list1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_rollno.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_rollno.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_rollno.Attributes.Add("placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_rollno.Attributes.Add("placeholder", "App No");
                    chosedmode = 3;
                    break;
            }



        }
        catch { }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //stud
            txt_rollno.Text = "";
            txt_name.Text = "";
            txt_batch.Text = "";
            txt_degr.Text = "";
            txt_dept.Text = "";
            txt_sem.Text = "";
            txt_sec.Text = "";
            imb_studpht.ImageUrl = "";
            imb_studpht.Visible = false;
            //payamount
            txt_ventotpayamt.Text = "";
            txt_venpayamt.Text = "";
            txt_lbl_vencurbal.Text = "";
            txt_narr.Text = "";
            chk_insexcess.Checked = false;
            lbl_er.Text = "";
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_rollno.Attributes.Add("Placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_rollno.Attributes.Add("Placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_rollno.Attributes.Add("Placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_rollno.Attributes.Add("Placeholder", "App No");
                    chosedmode = 2;
                    break;
            }
        }
        catch { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getothername(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();

        string query = "select (VendorName +'-'+ convert (varchar(20),VendorPK)) as VendorName from CO_VendorMaster  where VendorType='-5' and VendorName like '%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["VendorName"].ToString());
            }
        }
        return name;
    }
    public void TextBox6_OnTextChanged(object sender, EventArgs e)
    {
        string othername = "";
        try
        {

            string name = "";
            string vendorpk = "";
            string SelectQ = "";
            string compname = "";
            string address = "";
            string mblno = "";
            string identitytype = "";
            string identityno = "";
            othername = Convert.ToString(TextBox6.Text);
            if (othername != "")
            {
                try
                {
                    string[] others = othername.Split('-');
                    if (othername.Length > 0)
                    {
                        name = Convert.ToString(others[0]);
                        vendorpk = Convert.ToString(others[1]);
                    }
                }
                catch { }
            }
            if (vendorpk != "")
            {
                SelectQ = "select VendorName,VendorCode,VendorCompName,VendorAddress,VendorMobileNo,IdentityType,IdentityNo from CO_VendorMaster where VendorPK='" + vendorpk + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelectQ, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        TextBox6.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorName"]);
                        txt_othcname.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorCompName"]);
                        txt_othadd.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorAddress"]);
                        txt_mblno.Text = Convert.ToString(ds.Tables[0].Rows[0]["VendorMobileNo"]);
                        // ddl_group.Text = Convert.ToString(ds.Tables[0].Rows[0]["IdentityType"]);
                        TextBox8.Text = Convert.ToString(ds.Tables[0].Rows[0]["IdentityNo"]);
                    }
                    else
                    {
                        TextBox6.Text = "";
                        txt_othcname.Text = "";
                        txt_othadd.Text = "";
                        txt_mblno.Text = "";
                        ddl_group.Text = "";
                        TextBox8.Text = "";
                    }

                }
            }
            if (othername != "")
            {
                compname = Convert.ToString(txt_othcname.Text);
                address = Convert.ToString(txt_othadd.Text);
                mblno = Convert.ToString(txt_mblno.Text);
                identitytype = Convert.ToString(ddl_group.SelectedItem.Value);
                identityno = Convert.ToString(TextBox8.Text);

                if (compname != "" && address != "" && identitytype != "" && identityno != "")
                {
                    string InsertQ = "insert into CO_VendorMaster(VendorName,VendorCompName,VendorAddress,VendorMobileNo,IdentityType,IdentityNo)values('" + othername + "','" + compname + "','" + address + "','" + mblno + "','" + identitytype + "','" + identityno + "')";
                    int InsertVal = d2.update_method_wo_parameter(InsertQ, "Text");
                }
            }
            //  }
        }
        catch { }
        getothername(othername);
    }

    public void getothername(string othername)
    {
        try
        {
            string date = "";
            string date1 = "";
            string instaldt = "";
            string instaldate = "";
            string dt = "";
            string mn = "";
            string yr = "";
            string dateonly = "";
            string tme = "";
            string amorpm = "";
            string fndt = "";
            date1 = txt_vdate.Text.ToString();
            string[] strdate1 = date1.Split('/');
            if (strdate1.Length > 0)
            {
                date = strdate1[0].ToString() + "/" + strdate1[1].ToString() + "/" + strdate1[2].ToString();
            }
            string selqry = d2.GetFunction(" select MemName from FT_FinDailyTransaction where MemName='" + othername + "'");
            string selqryinstdate = "select InstallmentDate,InstallmentAmt from FT_FinInstallmentPay where MemName='" + selqry + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqryinstdate, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    instaldt = ds.Tables[0].Rows[i][0].ToString();
                    string[] insdatetime = instaldt.Split(' ');
                    if (insdatetime.Length > 0)
                    {
                        dateonly = insdatetime[0].ToString();
                        tme = insdatetime[1].ToString();
                        amorpm = insdatetime[2].ToString();
                    }
                    DateTime dtt = new DateTime();
                    string[] instalftdt = dateonly.Split('/');
                    if (instalftdt.Length > 0)
                    {
                        dt = instalftdt[0].ToString();
                        mn = instalftdt[1].ToString();
                        yr = instalftdt[2].ToString();
                        instaldate = dt + "/" + mn + "/" + yr;
                        dtt = Convert.ToDateTime(instaldate);
                        fndt = dtt.ToString("dd/MM/yyyy");
                    }
                    if (date == fndt)
                    {

                        string selqryamt = d2.GetFunction(" select SUM(InstallmentAmt) as amt,MemName from FT_FinInstallmentPay where InstallmentDate='" + dtt.ToString("MM/dd/yyyy") + "' and ISNULL(InstallmentStatus,'0')='0' group by MemName");
                        txt_ventotpayamt.Text = selqryamt;
                        txt_venpayamt.Text = selqryamt;
                        txt_amt.Text = selqryamt;
                    }
                }
            }

        }
        catch
        { }
    }
    public string generateVendorCode()
    {
        string newitemcode = string.Empty;
        try
        {
            string selectquery = "select VenAcr,VenStNo,VenSize  from IM_CodeSettings  order by startdate desc";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["VenAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["VenStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["VenSize"]);
                if (itemacronym.Trim() != "" && itemstarno.Trim() != "")
                {
                    selectquery = " select distinct top (1) VendorCode  from CO_VendorMaster where VendorCode like '" + Convert.ToString(itemacronym) + "%' order by VendorCode desc";
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
                        else if (len1 == 3)
                        {
                            newitemcode = "000" + newnumber;
                        }
                        else if (len1 == 4)
                        {
                            newitemcode = "0000" + newnumber;
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
                        else if (size == 3)
                        {
                            newitemcode = "000" + itemstarno;
                        }
                        else if (size == 4)
                        {
                            newitemcode = "0000" + itemstarno;
                        }
                        else if (size == 5)
                        {
                            newitemcode = "00000" + itemstarno;
                        }
                        else if (size == 6)
                        {
                            newitemcode = "000000" + itemstarno;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(itemstarno);
                        }
                        newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                    }
                }
            }
        }
        catch { newitemcode = string.Empty; }
        return newitemcode;
    }

    //added by sudhagar payment ledger 26-09-2016
    public void loadpayLedger()
    {
        try
        {
            ddlapyledg.Items.Clear();
            string selQ = "select l.ledgerPk,l.ledgername FROM FM_LedgerMaster L,FS_LedgerPrivilage P,FT_FinCashContraDet ft WHERE L.LedgerPK = P.LedgerFK  and l.LedgerPK=ft.ledgerfk  AND P.CollegeCode = L.CollegeCode AND P. UserCode = '" + usercode + "' and ispetty='1'  and L.CollegeCode = " + collegecode1 + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selQ, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlapyledg.DataSource = ds;
                ddlapyledg.DataTextField = "ledgername";
                ddlapyledg.DataValueField = "ledgerPk";
                ddlapyledg.DataBind();

                string ledgerfk = "";
                if (ddlapyledg.Items.Count > 0)
                {
                    ledgerfk = Convert.ToString(ddlapyledg.SelectedItem.Value);
                }
                if (!string.IsNullOrEmpty(ledgerfk))
                {
                    double Amt = 0;
                    double.TryParse(Convert.ToString(d2.GetFunction("select sum(isnull(debit,0)-isnull(credit,0)) as  amount from FT_FinCashContraDet where ispetty='1'  and ledgerfk='" + ledgerfk + "'")), out Amt);
                    txtpayamt.Text = Convert.ToString(Amt);
                }
            }
            else
            {
                ddlapyledg.Items.Clear();
                txtpayamt.Text = "";
            }

        }
        catch
        {
        }
    }

    protected void ddlapyledg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string ledgerfk = "";
            if (ddlapyledg.Items.Count > 0)
            {
                ledgerfk = Convert.ToString(ddlapyledg.SelectedItem.Value);
            }
            if (!string.IsNullOrEmpty(ledgerfk))
            {
                double Amt = 0;
                double.TryParse(Convert.ToString(d2.GetFunction("select sum(isnull(debit,0)-isnull(credit,0)) as  amount from FT_FinCashContraDet where ispetty='1'  and ledgerfk='" + ledgerfk + "'")), out Amt);
                txtpayamt.Text = Convert.ToString(Amt);
            }
            else
                txtpayamt.Text = "";
        }
        catch { }
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

        lbl.Add(lbl_clg);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);

        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

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
                fproll.Columns[1].Visible = true;
                fproll.Columns[2].Visible = true;
                fproll.Columns[3].Visible = true;
            }
            else if (roll == 1)
            {
                fproll.Columns[1].Visible = true;
                fproll.Columns[2].Visible = true;
                fproll.Columns[3].Visible = true;
            }
            else if (roll == 2)
            {
                fproll.Columns[1].Visible = true;
                fproll.Columns[2].Visible = false;
                fproll.Columns[3].Visible = false;

            }
            else if (roll == 3)
            {
                fproll.Columns[1].Visible = false;
                fproll.Columns[2].Visible = true;
                fproll.Columns[3].Visible = false;
            }
            else if (roll == 4)
            {
                fproll.Columns[1].Visible = false;
                fproll.Columns[2].Visible = false;
                fproll.Columns[3].Visible = true;
            }
            else if (roll == 5)
            {
                fproll.Columns[1].Visible = true;
                fproll.Columns[2].Visible = true;
                fproll.Columns[3].Visible = false;
            }
            else if (roll == 6)
            {
                fproll.Columns[1].Visible = false;
                fproll.Columns[2].Visible = true;
                fproll.Columns[3].Visible = true;
            }
            else if (roll == 7)
            {
                fproll.Columns[1].Visible = true;
                fproll.Columns[2].Visible = false;
                fproll.Columns[3].Visible = true;
            }
        }
        catch { }
    }

    #endregion

    // last modified 24.08.2017 sudhagar
    public void paavaiVoucher(DataTable dtvocucher, string AppNo, string dupReceipt, string recptNo, string recptDt)
    {
        //PAVAI College and School
        try
        {
            string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
            DataSet dsPri = new DataSet();
            dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
            if (dsPri.Tables.Count > 0 && dsPri.Tables[0].Rows.Count > 0)
            {
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                byte ColName = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                //Document Settings

                bool createPDFOK = false;

                contentDiv.InnerHtml = "";
                StringBuilder sbHtml = new StringBuilder();
                string collname = d2.GetFunction("select collname from collinfo where college_code ='" + collegecode1 + "'");
                int heightvar = 0;
                int transType = 0;

                sbHtml.Clear();
                int officeCopyHeight = 0;
                StringBuilder sbHtmlCopy = new StringBuilder();

                string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                if (confirmChk != null || confirmChk != "" || confirmChk != "0")
                {
                    string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype,DDNo,convert(varchar(10),DDDate,103) as DDDate,(select textval from textvaltable where textcode =DDBankCode) as Bank,DDBankBranch  from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                    DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                    if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                    {
                        string rollno = string.Empty;
                        string studname = string.Empty;
                        string receiptno = string.Empty;
                        string name = string.Empty;
                        string batch_year = string.Empty;

                        string app_formno = string.Empty;
                        string appnoNew = string.Empty;
                        string Regno = string.Empty;
                        string Roll_admit = string.Empty;
                        string section = string.Empty;
                        string currentSem = string.Empty;

                        string batchYrSem = string.Empty;

                        string mode = string.Empty;
                        string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                        string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                        string ddNo = Convert.ToString(dsDet.Tables[0].Rows[0]["ddNo"]).Trim();
                        string modePaySng = string.Empty;
                        string dddates = Convert.ToString(dsDet.Tables[0].Rows[0]["DDDate"]);
                        string ddnos = Convert.ToString(dsDet.Tables[0].Rows[0]["DDNo"]);
                        string ddBanks = Convert.ToString(dsDet.Tables[0].Rows[0]["Bank"]);
                        string ddBrans = Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankBranch"]);

                        DataTable uniqueCols = dsDet.Tables[0].DefaultView.ToTable(true, "PayMode");
                        if (uniqueCols.Rows.Count > 0)
                        {
                            for (int a = 0; a < uniqueCols.Rows.Count; a++)
                            {
                                switch (Convert.ToString(uniqueCols.Rows[a][0]).Trim())
                                {
                                    case "1":
                                        mode += "Cash,";
                                        break;
                                    case "2":
                                        mode += "Cheque,";
                                        break;
                                    case "3":
                                        mode += "DD,";
                                        break;
                                    case "6":
                                        mode += "Card";
                                        break;
                                }
                            }
                            mode = mode.TrimEnd(',');
                        }
                        else
                        {
                            switch (paymode)
                            {
                                case "1":
                                    mode = "Cash";
                                    break;
                                case "2":
                                    mode = "Cheque";
                                    //mode = "Cheque - No:" + ddNo;
                                    modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                    //mode += modePaySng;
                                    break;
                                case "3":
                                    mode = "DD";
                                    //mode = "DD - No:" + ddNo;
                                    modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                    //mode += modePaySng;
                                    break;
                                case "4":
                                    mode = "Challan";
                                    break;
                                case "5":
                                    mode = "Online Payment";
                                    break;
                                case "6":
                                    mode = "Card";
                                    modePaySng = "\n\nCard : " + ddBanks;
                                    break;
                                default:
                                    mode = "Others";
                                    break;
                            }
                        }

                        string queryRollApp;

                        queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name,r.Roll_admit,r.sections,r.batch_year,r.current_semester  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
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
                                Roll_admit = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_admit"]);
                                studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                batch_year = Convert.ToString(dsRollApp.Tables[0].Rows[0]["batch_year"]);
                                section = Convert.ToString(dsRollApp.Tables[0].Rows[0]["sections"]).ToUpper();
                                currentSem = Convert.ToString(dsRollApp.Tables[0].Rows[0]["current_Semester"]).ToUpper();
                            }
                            else
                                appnoNew = AppNo;
                        }
                        else
                            appnoNew = AppNo;
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


                            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " ";
                            if (rb_stud.Checked)
                            {
                                colquery += " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,isnull(r.Sections,'') as Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + " ";

                            }
                            else if (rb_staff.Checked)
                            {
                                colquery += "  select appl_id ,h.dept_name,h.dept_acronym,h.dept_code,s.staff_name,s.staff_code,a.father_name,t.stftype as staff_type  from staffmaster s,staff_appl_master a,hrdept_master h,stafftrans t,desig_master d where s.appl_no =a.appl_no and s.staff_code =t.staff_code and t.dept_code =h.dept_code and d.desig_code =t.desig_code and s.college_code =h.college_code and d.collegeCode =s.college_code and latestrec ='1' and appl_id ='" + AppNo + "' and s.college_Code=" + collegecode1 + "  ";
                            }
                            else if (rb_vendor.Checked)
                            {
                                colquery += " SELECT VendorContactPK, VenContactType, VenContactName, VenContactDesig, VenContactDept, VendorPhoneNo, VendorExtNo, VendorMobileNo, VendorEmail, VendorFK FROM      IM_VendorContactMaster WHERE VendorContactPK = '" + AppNo + "' ";
                            }
                            else if (rb_others.Checked)
                            {
                                colquery += " SELECT VendorCode,vendorname,VendorMobileNo,VendorAddress,VendorCity,VendorCompName,VendorType  from co_vendormaster  WHERE VendorPK = '" + AppNo + "' ";

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
                            string strMem = string.Empty;
                            string TermOrSem = string.Empty;
                            string classdisplay = "Class Name ";
                            string rollDisplay = string.Empty;
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
                                    classdisplay = "Dept Name ";
                                    TermOrSem = "Semester";
                                    if (rb_stud.Checked)
                                    {
                                        deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                        board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                        mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                        strMem = rbl_rollno.SelectedItem.Text.Trim();
                                        if (Convert.ToInt32(rbl_rollno.SelectedValue) == 0)
                                        {
                                            Roll_admit = rollno;
                                        }
                                        else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 1)
                                        {
                                            Roll_admit = Regno;
                                        }
                                        else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 2)
                                        {
                                            //Roll_admit = Roll_admit;
                                        }
                                        else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 3)
                                        {
                                            Roll_admit = app_formno;
                                        }
                                        //}
                                    }
                                    else if (rb_staff.Checked)
                                    {
                                        deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["staff_type"]);
                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["father_name"]);
                                        //sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                                        Roll_admit = Convert.ToString(ds.Tables[1].Rows[0]["staff_code"]);
                                        studname = Convert.ToString(ds.Tables[1].Rows[0]["staff_name"]);
                                        strMem = "Staff Code";
                                    }
                                    else if (rb_vendor.Checked)
                                    {
                                        deg = " - ";
                                        Roll_admit = Convert.ToString(ds.Tables[1].Rows[0]["VendorContactPK"]);
                                        studname = Convert.ToString(ds.Tables[1].Rows[0]["VenContactName"]);
                                        strMem = "Vendor Code";
                                    }
                                    else if (rb_others.Checked)
                                    {
                                        deg = " - ";
                                        Roll_admit = Convert.ToString(ds.Tables[1].Rows[0]["VendorCode"]);
                                        studname = Convert.ToString(ds.Tables[1].Rows[0]["vendorname"]);
                                        strMem = "Other Code";
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
                            if (rb_stud.Checked)
                            {
                                degString = deg;//.Split('-')[0].ToUpper();
                            }
                            else if (rb_staff.Checked)
                            {
                                degString = deg;
                            }
                            string[] className = degString.Split('-');
                            if (className.Length > 1)
                            {
                                // degString = className[1];
                            }
                            string entryUserCode = d2.GetFunction("select distinct entryusercode from ft_findailytransaction where app_no='" + AppNo + "' and TransCode='" + recptNo + "'");
                            string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + entryUserCode + "'").Trim();

                            #region Receipt Header

                            //  sbHtml.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");

                            sbHtml.Append("<div style='height: 560px;width:795px; border:0px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");
                            sbHtml.Append("<div style=' width:790px; height:100px;'></div>");

                            //sbHtmlCopy.Append("<div style=' width:790px; height:#officeCopyHeight#px;'></div>");
                            //sbHtmlCopy.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");
                            sbHtmlCopy.Append("<div style='height:#officeCopyHeight#px; width:790px;'></div>");
                            if (ColName == 1)
                            {
                                sbHtml.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                                sbHtml.Append("<br/>");

                                sbHtmlCopy.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                                sbHtmlCopy.Append("<br/>");
                            }
                            sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + " Receipt </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Voucher No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermOrSem + " </td><td style='width:160px; '>: " + currentSem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                            sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + " Receipt </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Voucher No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermOrSem + " </td><td style='width:160px; '>: " + currentSem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                            #endregion

                            #region Receipt Body

                            sbHtml.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                            sbHtmlCopy.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                            selectQuery = "";

                            int sno = 0;
                            int indx = 0;
                            double totalamt = 0;
                            double balanamt = 0;
                            double curpaid = 0;
                            // double paidamount = 0;



                            string selHeadersQ = string.Empty;
                            DataSet dsHeaders = new DataSet();


                            //New
                            //selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk,H.HeaderName ";

                            //selHeadersQ += "  select distinct r.college_code,f.feecategory,r.degree_code,isnull(monthcode,'0')as monthcode,MonthYear from Fee_degree_match fd,registration r,FT_FinDailyTransaction f  where fd.college_code=r.college_code and f.app_no=r.app_no and f.feecategory=fd.feecategory and r.degree_code=fd.degree_code and r.college_code='" + collegecode1 + "' and r.App_No ='" + AppNo + "'";

                            ////fine amount added by sudhagar 31.01.2017
                            //selHeadersQ += " select SUM(debit) as TakenAmt,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,h.headername  from FT_FinDailyTransaction d,fm_headermaster h  where d.headerfk=h.headerpk and  d.transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and finefeecategory='-1'  group by D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk ,h.headername";
                            //New End


                            DataView dv = new DataView();
                            if (dtvocucher.Rows.Count > 0)
                            {
                                // string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                //dsHeaders.Clear();
                                //dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                //if (dsHeaders.Tables.Count > 0)
                                //{
                                //    if (dsHeaders.Tables[0].Rows.Count > 0)
                                //    {
                                //Hashtable htHdrAmt = new Hashtable();
                                //Hashtable htHdrName = new Hashtable();
                                // Hashtable htfeecat = new Hashtable();
                                int ledgCnt = 0;
                                //Dictionary<string, string> htfeecat = new Dictionary<string, string>();
                                //Dictionary<string, double> htfeeAmt = new Dictionary<string, double>();
                                //for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                //{
                                for (int head = 0; head < dtvocucher.Rows.Count; head++)
                                {
                                    string disphdr = string.Empty;
                                    double allotamt0 = 0;
                                    double deductAmt0 = 0;
                                    double totalAmt0 = 0;
                                    double paidAmt0 = 0;
                                    double balAmt0 = 0;
                                    double creditAmt0 = 0;
                                    double dts = 0;
                                    creditAmt0 = Convert.ToDouble(dtvocucher.Rows[head]["Amount"]);
                                    //totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                    //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);
                                    // creditAmt0 = d2.GetFunction("select paybalamt from ft_findailytransaction where app_no='" + AppNo + "' and TransCode='" + recptNo + "'"); 
                                    //paidAmt0 = totalAmt0 - balAmt0;
                                    //deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                    disphdr = Convert.ToString(dtvocucher.Rows[head]["Header"]);
                                    //    double pay = Convert.ToDouble(dsDet.Tables[0].Rows[0]["paydeductamt"]);
                                    // string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                    // string feecode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                    //string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                    // string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);


                                    //creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                    //totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                    ////balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);

                                    ////paidAmt0 = totalAmt0 - balAmt0;
                                    //deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                    //disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                    //string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                    //string feecode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                    //string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                    //string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);

                                    //string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                    // paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));
                                    double deductamount;
                                    double balamount = 0;
                                    double.TryParse(Convert.ToString(txt_dtsamt.Text), out deductamount);
                                    if (rb_percentage.Checked == true)
                                    {
                                        deductamount = (creditAmt0 * deductamount / 100);
                                        balamount = creditAmt0 - deductamount;
                                    }
                                    if (rb_amount.Checked == true)
                                    {

                                        balamount = creditAmt0 - deductamount;
                                    }


                                    #region Monthwise old
                                    //string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                    //string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                    //int monWisemon = 0;
                                    //int monWiseYea = 0;
                                    //string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                    //string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                    //int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                    //int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                    //if (monWisemon > 0 && monWiseYea > 0)
                                    //{
                                    //    string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                    //    DataSet dsMonwise = new DataSet();
                                    //    dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                    //    if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                    //    {
                                    //        totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                    //        paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                    //        disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                    //        balAmt0 = totalAmt0 - paidAmt0;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                    //}
                                    #endregion

                                    //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                    // feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                    sno++;

                                    totalamt += Convert.ToDouble(totalAmt0);
                                    balanamt += Convert.ToDouble(balAmt0);
                                    curpaid += Convert.ToDouble(balamount);

                                    deductionamt += Convert.ToDouble(deductAmt0);

                                    indx++;
                                    createPDFOK = true;
                                    DropDownList ddl_credit = new DropDownList();
                                    DropDownList ddl_debit = new DropDownList();
                                    TextBox txtpaymt = new TextBox();

                                    if (rb_percentage.Checked == true || rb_amount.Checked == true)
                                    {
                                        for (int j = 0; j < gridView3.Rows.Count; j++)
                                        {

                                            ddl_credit = (DropDownList)gridView3.Rows[j].FindControl("ddl_credit");
                                            ddl_debit = (DropDownList)gridView3.Rows[j].FindControl("ddl_debit");
                                            txtpaymt = (TextBox)gridView3.Rows[j].FindControl("txtpaymt");
                                            double tdsamount = 0;
                                            tdsamount = Convert.ToDouble(txtpaymt.Text.Trim());
                                            //txtpaymt = ((gridView3.Rows[j].FindControl("txtpaymt").);
                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(balamount) + "." + returnDecimalPart(balamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");


                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(balamount) + "." + returnDecimalPart(balamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                            sno++;
                                            disphdr = ddl_credit.SelectedItem.Text;
                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "(CR)" + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(tdsamount) + "." + returnDecimalPart(tdsamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                            disphdr = ddl_credit.SelectedItem.Text;
                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "(CR)" + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(tdsamount) + "." + returnDecimalPart(tdsamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                            sno++;
                                            disphdr = ddl_debit.SelectedItem.Text;
                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "(DR)" + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(tdsamount) + "." + returnDecimalPart(tdsamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                            disphdr = ddl_debit.SelectedItem.Text;
                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "(DR)" + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(tdsamount) + "." + returnDecimalPart(tdsamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                        }
                                    }
                                    else
                                    {
                                        sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");



                                        sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                    }
                                    //officeCopyHeight -= 20;
                                    ledgCnt++;
                                }

                                if (BalanceType == 1)
                                {
                                    balanamt = retBalance(appnoNew);
                                }

                                #region DD Narration
                                string modeMulti = string.Empty;
                                bool multiCash = false;
                                bool multiChk = false;
                                bool multiDD = false;
                                bool multiCard = false;

                                DataSet dtMulBnkDetails = new DataSet();
                                dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD'  else 'Card' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3,6) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

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
                                            string strMode = Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]);
                                            if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CASH")
                                            {
                                                multiCash = true;
                                                continue;
                                            }
                                            else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CHEQUE")
                                            {
                                                multiChk = true;
                                            }
                                            else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "DD")
                                            {
                                                multiDD = true;
                                            }
                                            else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CARD")
                                            {
                                                multiCard = true;
                                                ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + "\nCard :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3];
                                                //+ " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                sn++;
                                                continue;
                                            }

                                            ddnar += "\n" + strMode + " No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3];
                                            //+ " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
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
                                    ddnar += remarks;

                                    if (excessRemaining(appnoNew) > 0)
                                        ddnar += " Excess Amount Rs. : " + excessRemaining(appnoNew);
                                    //added by abarna 11.12.2017
                                    if (rb_percentage.Checked == true)
                                    {
                                        ddnar += "Percentage:" + txt_dtsamt.Text + "%";
                                    }

                                    if (rb_amount.Checked == true)
                                    {
                                        ddnar += "Amount Rs:" + txt_dtsamt.Text;

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
                                    modeMulti += "DD,";
                                }
                                if (multiCard)
                                {
                                    modeMulti += "Card";
                                }
                                modeMulti = modeMulti.TrimEnd(',');
                                if (modeMulti != "")
                                {
                                    mode = modeMulti;
                                }
                                //ddnar += remarks;
                                #endregion


                                double totalamount = curpaid;
                                sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:55px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;' colspan='3'>Verified by <span style='padding-left:200px;'>Student copy</span><span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                                sbHtml.Append("</table></div><br>");

                                sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                                if (ledgCnt == 1)
                                    officeCopyHeight += 290; //270;
                                else if (ledgCnt == 2)
                                    officeCopyHeight += 260; //240;
                                else if (ledgCnt == 3)
                                    officeCopyHeight += 230;//210;
                                else if (ledgCnt == 4)
                                    officeCopyHeight += 200;//180;
                                else if (ledgCnt >= 5)
                                    officeCopyHeight += 155;// 170;// 150;
                                // heightvar += officeCopyHeight;
                                sbHtmlCopy.Append("</table></div><br>");
                                sbHtmlCopy.Replace("#officeCopyHeight#", officeCopyHeight.ToString());
                                //    }
                                //}
                            }
                            sbHtml.Append((studOffiCopy == 1 ? sbHtmlCopy.ToString() : string.Empty) + "</td></tr></table></div>");
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
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found')", true);
                    }
                }
                // }
                // }
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
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found')", true);
                }
                #endregion



            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found')", true);
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate");
        }

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
            words += " and ";
            words += ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
            words += " Paise ";
        }
        return words;
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

    protected void getTabRights(object sender, EventArgs e)
    {
        //rb_stud.Visible = true;
        //rb_staff.Visible = true;
        //rb_vendor.Visible = true;
        //rb_others.Visible = true;
        string selQ = "select LinkValue from New_InsSettings where LinkName='FinancePaymentTabRights' and user_code ='" + usercode + "' and college_code ='" + ddl_collegename.SelectedItem.Value + "' ";
        string strVal = Convert.ToString(d2.GetFunction(selQ));
        if (!string.IsNullOrEmpty(strVal) && strVal != "0")
        {
            string[] strSpltVal = strVal.Split('$');
            if (strSpltVal.Length > 0)
            {
                for (int rcpt = 0; rcpt < strSpltVal.Length; rcpt++)
                {
                    switch (Convert.ToInt32(strSpltVal[rcpt]))
                    {
                        case 1:
                            rb_stud.Visible = true;
                            rb_stud.Checked = true;
                            rb_stud_Change(sender, e);
                            break;
                        case 2:
                            rb_staff.Visible = true;
                            rb_staff.Checked = true;
                            rb_staff_Change(sender, e);
                            break;
                        case 3:
                            rb_vendor.Visible = true;
                            rb_vendor.Checked = true;
                            rb_vendor_Change(sender, e);
                            break;
                        case 4:
                            rb_others.Visible = true;
                            rb_others.Checked = true;
                            rb_others_Change(sender, e);
                            break;
                    }
                }
                rb_cash_Change(sender, e);
            }
        }
    }

}