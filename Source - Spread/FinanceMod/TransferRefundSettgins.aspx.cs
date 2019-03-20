using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Collections;
using System.Text;
using System.IO;
using System.Drawing;
using System.Net;
using System.Configuration;
using System.Web;

public partial class TransferRefundSettgins : System.Web.UI.Page
{

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string stcollegecode = string.Empty;
    static string appliedClgCode = string.Empty;
    static string collegecodestat = string.Empty;
    static string admisionvalue = string.Empty;
    static int admis = 0;
    int userCode = 0;
    static int chosedmode = 0;
    static int personmode = 0;
    static string vencontcode = "-1";
    static int settType = 0;
    static byte BalanceType = 0;
    static int SelectedCnt = 0;
    DataSet ds = new DataSet();
    InsproDirectAccess DirAccess = new InsproDirectAccess();
    AdmissionNumberAndApplicationNumberGeneration autoGenDS = new AdmissionNumberAndApplicationNumberGeneration();
    ReuasableMethods reUse = new ReuasableMethods();
    DAccess2 d2 = new DAccess2();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //****************************************************//
                //string url = HttpContext.Current.Request.Url.AbsoluteUri;
                //if (url.Contains("?"))
                //{
                //    string[] splitTransval = url.Split('?');
                //    string transVal = splitTransval[1];
                //    string[] sp = transVal.Split('*');
                //    string val = sp[1];
                //    string spval = val.Trim();
                //    string decQString = Decryptdata(spval);
                //    string[] resultArray = decQString.Split('&');
                //    foreach (string qstring in resultArray)
                //    {
                //        string[] fnlRes = qstring.Split('$');
                //        string[] valu = fnlRes[0].Split('$');
                //        if (valu.Length > 0)
                //        {
                //            switch (valu[0])
                //            {
                //                case "C":
                //                    Session["collegecode"] = fnlRes[1];
                //                    break;
                //                case "U":
                //                    Session["UserCode"] = fnlRes[1];
                //                    break;
                //                case "G":
                //                    Session["group_code"] = fnlRes[1];
                //                    break;
                //                case "S":
                //                    Session["Single_User"] = fnlRes[1];
                //                    break;
                //                case "P":
                //                    Session["column_header_row_count"] = fnlRes[1];
                //                    break;
                //                case "V":
                //                    Session["Version"] = fnlRes[1];
                //                    break;
                //                case "SC":
                //                    Session["Staff_Code"] = fnlRes[1];
                //                    break;
                //                case "UN":
                //                    Session["UserName"] = fnlRes[1];
                //                    break;
                //                case "E":
                //                    Session["Entry_Code"] = fnlRes[1];
                //                    break;
                //                case "L":
                //                    Session["IsLogin"] = fnlRes[1];
                //                    break;
                //                case "Cu":
                //                    Session["current_college_code"] = fnlRes[1];
                //                    break;
                //                case "IC":
                //                    Session["InternalCollegeCode"] = fnlRes[1];
                //                    break;
                //                case "Pa":
                //                    Session["password"] = fnlRes[1];
                //                    break;
                //                case "PV":
                //                    Session["prntvissble"] = fnlRes[1];
                //                    break;
                //                case "D":
                //                    Session["dashprod"] = fnlRes[1];
                //                    break;
                //                case "M":
                //                    Session["MID"] = fnlRes[1];
                //                    break;
                //                case "SB":
                //                    Session["backbutton_value"] = fnlRes[1];
                //                    break;
                //            }
                //        }
                //    }
                //}
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
            }
            usercode = Session["usercode"].ToString();
            userCode = Convert.ToInt32(Convert.ToString(Session["usercode"]));
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            //sessstream = Convert.ToString(Session["streamcode"]);
            //lbl_str1.Text = sessstream;
            //lbl_str2.Text = sessstream;
            //lbl_str3.Text = sessstream;
            //lbl_str4.Text = sessstream;

            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                usercode = Session["group_code"].ToString();
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                usercode = Session["usercode"].ToString();
            }
            if (!IsPostBack)
            {

                ViewState["PreviousPage"] = Request.UrlReferrer;
                setLabelText();
                Sourcecollege();
                loadfromsetting();

                rightsettings(sender, e);
                ddladmis_Selected(sender, e);
                //added by abarna 14.12.2017
                getTabRights(sender, e);
            }
        }
        catch (Exception ex) { }
    }

    private string Decryptdata(string encryptpwd)
    {
        string decryptpwd = string.Empty;
        UTF8Encoding encodepwd = new UTF8Encoding();
        Decoder Decode = encodepwd.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
        int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        char[] decoded_char = new char[charCount];
        Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        decryptpwd = new String(decoded_char);
        return decryptpwd;
    }

    public void rightsettings(object sender, EventArgs e)
    {
        rb_transfer.Visible = false;
        rb_discont.Visible = false;
        rb_refund.Visible = false;
        rb_Journal.Visible = false;
        rb_ProlongAbsent.Visible = false;
        fileterVisible();
        string Qry = " select LinkValue from New_InsSettings where LinkName='Transfer and Refund Settings'  and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "'";

        string settingtransferrefund = Convert.ToString(d2.GetFunction(Qry));
        if (settingtransferrefund.Contains("1"))
        {
            rb_transfer.Visible = true;
            rb_transfer_Change(sender, e);
        }
        if (settingtransferrefund.Contains("2"))
        {
            rb_discont.Visible = true;
            SelectedCnt++;
            rb_refund_Change(sender, e);
        }
        if (settingtransferrefund.Contains("3"))
        {
            rb_refund.Visible = true;
            rb_discont_Change(sender, e);
        }
        if (settingtransferrefund.Contains("4"))
        {
            rb_Journal.Visible = true;
            rb_Journal_Change(sender, e);
        }
        if (settingtransferrefund.Contains("5"))
        {
            rb_ProlongAbsent.Visible = true;
            rb_ProlongAbsent_Change(sender, e);
        }
    }
    protected void fileterVisible()
    {
        lnkindivmap.Visible = false;
        fldapplied.Visible = false;
        fldrefund.Visible = false;
        cbdisWithoutFees.Visible = false;
        fldadm.Visible = false;
        divTransfer.Visible = false;
        div_refund.Visible = false;
    }
    protected void getEvent(int totCnt, int tranS, int disCont, int reFund, int journaL)
    {
        //switch()
    }


    public void Sourcecollege()
    {
        ddlcollege.Items.Clear();
        // reUse.bindCollegeToDropDown(usercode, ddlcollege);
        string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
        ds.Clear();
        string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
        if (ddlcollege.Items.Count > 0)
            stcollegecode = Convert.ToString(ddlcollege.SelectedValue);
    }
    protected void ddlcollege_indexChanged(object sender, EventArgs e)
    {
        if (ddlcollege.Items.Count > 0)
            stcollegecode = Convert.ToString(ddlcollege.SelectedValue);
        if (!rb_transfer.Checked)
        {
            disTransClear();
        }
        else
        {
            transFromClear();
            transToClear();
        }

        if (rb_Journal.Checked)//modified
        {
            rightsettings(sender, e);
            getTabRights(sender, e);
        }

    }

    #region applied
    public void bindappliedclg()
    {
        ddlclgapplied.Items.Clear();
        // reUse.bindCollegeToDropDown(usercode, ddlclgapplied);
        string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
        ds.Clear();
        string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlclgapplied.DataSource = ds;
            ddlclgapplied.DataTextField = "collname";
            ddlclgapplied.DataValueField = "college_code";
            ddlclgapplied.DataBind();
        }
        if (ddlclgapplied.Items.Count > 0)
            appliedClgCode = Convert.ToString(ddlclgapplied.SelectedValue);

    }
    protected void ddlclgapplied_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlclgapplied.Items.Count > 0)
            appliedClgCode = Convert.ToString(ddlclgapplied.SelectedValue);
        transToClear();
    }
    #endregion

    #region not applied filter values
    public void bindnotappliedclg()
    {
        ddl_colg.Items.Clear();
        // reUse.bindCollegeToDropDown(usercode, ddlclgapplied);
        string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
        ds.Clear();
        string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_colg.DataSource = ds;
            ddl_colg.DataTextField = "collname";
            ddl_colg.DataValueField = "college_code";
            ddl_colg.DataBind();
        }
        //if (ddlclgapplied.Items.Count > 0)
        // ddl_colg = Convert.ToString(ddlclgapplied.SelectedValue);

    }
    public void bindclg()
    {
        try
        {
            ddl_colg.Items.Clear();
            bindnotappliedclg();
            //   reUse.bindCollegeToDropDown(usercode, ddl_colg);
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindsect();
            bindstream();
            bindSeat();
        }
        catch (Exception ex) { }
    }
    public void bindBtch()
    {
        try
        {
            ddl_batch.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
            binddeg();
            binddept();
        }
        catch (Exception ex) { }
    }
    public void binddeg()
    {
        try
        {
            ddl_degree.Items.Clear();

            string batch = "";
            if (ddl_colg.Items.Count > 0 && ddl_batch.Items.Count > 0)
            {
                batch = Convert.ToString(ddl_batch.SelectedValue.ToString());
                string stream = "";
                stream = Convert.ToString(ddl_strm.SelectedValue.ToString());
                if (batch != "")
                {
                    ds.Clear();

                    string sel = " select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + Convert.ToString(ddl_colg.SelectedValue) + "')  ";
                    if (stream != "")
                    {
                        sel = sel + "  and type in ('" + stream + "')";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sel, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_degree.DataSource = ds;
                        ddl_degree.DataTextField = "course_name";
                        ddl_degree.DataValueField = "course_id";
                        ddl_degree.DataBind();
                    }
                }
            }
        }
        catch (Exception ex) { }
    }
    public void binddept()
    {
        try
        {
            ddl_dept.Items.Clear();
            string degree = "";
            if (ddl_degree.Items.Count > 0 && ddl_colg.Items.Count > 0)
            {
                degree = Convert.ToString(ddl_degree.SelectedValue.ToString());

                if (degree != "")
                {
                    //ds.Clear();
                    //ds = d2.BindBranchMultiple(singleuser, group_user, degree, ddl_colg.SelectedItem.Value, usercode);
                    string sel = " select dt.Dept_Name,d.degree_code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.Course_Id in('" + degree + "') and d.college_code in('" + ddl_colg.SelectedItem.Value + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sel, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_dept.DataSource = ds;
                        ddl_dept.DataTextField = "dept_name";
                        ddl_dept.DataValueField = "degree_code";
                        ddl_dept.DataBind();

                    }
                }
            }
        }
        catch (Exception ex) { }
    }
    public void bindsem()
    {
        try
        {
            if (ddl_colg.Items.Count > 0 && ddl_batch.Items.Count > 0 && ddl_dept.Items.Count > 0)
            {
                DataSet ds3 = new DataSet();
                ddl_sem.Items.Clear();
                Boolean first_year;
                first_year = false;
                int duration = 0;
                int i = 0;


                string sqluery = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code= (" + ddl_dept.SelectedValue.ToString() + ") and batch_year  = (" + ddl_batch.SelectedValue.ToString() + ") and college_code=" + ddl_colg.SelectedValue.ToString() + "";

                ds3 = d2.select_method_wo_parameter(sqluery, "text");
                if (ds3.Tables.Count > 0)
                {
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
                        duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["ndurations"]);
                        for (i = 1; i <= duration; i++)
                        {
                            if (first_year == false)
                            {
                                ddl_sem.Items.Add(i.ToString());
                            }
                            else if (first_year == true && i != 2)
                            {
                                ddl_sem.Items.Add(i.ToString());
                            }

                        }
                    }
                    else
                    {
                        sqluery = "select distinct duration,first_year_nonsemester  from degree where degree_code in (" + ddl_dept.SelectedValue.ToString() + ") and college_code=" + ddl_colg.SelectedValue.ToString() + "";
                        ddl_sem.Items.Clear();
                        ds3 = d2.select_method_wo_parameter(sqluery, "text");
                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
                            duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["duration"]);
                            for (i = 1; i <= duration; i++)
                            {
                                if (first_year == false)
                                {

                                    ddl_sem.Items.Add(i.ToString());
                                }
                                else if (first_year == true && i != 2)
                                {

                                    ddl_sem.Items.Add(i.ToString());
                                }
                            }
                        }
                    }
                }
            }

        }
        catch (Exception ex) { }
    }
    public void bindSeat()
    {
        ddl_seattype.Items.Clear();
        try
        {
            if (ddl_colg.Items.Count > 0)
            {
                DataSet dsSeat = new DataSet();
                dsSeat = d2.select_method_wo_parameter("select TextVal,Textcode from TextValTable where textcriteria='seat' and college_code='" + ddl_colg.SelectedValue + "' order by Textval asc", "Text");
                if (dsSeat.Tables.Count > 0 && dsSeat.Tables[0].Rows.Count > 0)
                {
                    ddl_seattype.DataSource = dsSeat;
                    ddl_seattype.DataTextField = "TextVal";
                    ddl_seattype.DataValueField = "Textcode";
                    ddl_seattype.DataBind();
                }
            }
        }
        catch (Exception ex) { }
    }
    public string bindstudsem(int semester, string college)
    {
        string semesterquery = "";

        string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + college + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(settingquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
            if (linkvalue == "0")
            {
                semesterquery = d2.GetFunction("select * from textvaltable where TextCriteria = 'FEECA'and textval like '" + semester + " Semester' and textval not like '-1%' and college_code ='" + college + "'");

            }
            else
            {
                semesterquery = d2.GetFunction("select * from textvaltable where TextCriteria = 'FEECA'and textval like '" + semester + " Year' and textval not like '-1%' and college_code ='" + college + "'");

            }
        }

        return semesterquery;
    }
    public void bindsect()
    {
        try
        {
            ddl_sec.Items.Clear();
            if (ddl_colg.Items.Count > 0 && ddl_batch.Items.Count > 0 && ddl_dept.Items.Count > 0 && ddl_sem.Items.Count > 0)
            {

                string branch = ddl_dept.SelectedValue.ToString();
                string batch = ddl_batch.SelectedValue.ToString();
                ListItem item = new ListItem("Empty", " ");
                string sqlquery = "select distinct sections from registration where batch_year=" + batch + " and degree_code=" + branch + " and college_code=" + ddl_colg.SelectedValue.ToString() + " and Current_Semester=" + ddl_sem.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";

                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(sqlquery, "text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddl_sec.DataSource = ds;
                        ddl_sec.DataTextField = "sections";
                        ddl_sec.DataValueField = "sections";
                        ddl_sec.DataBind();
                        ddl_sec.Enabled = true;

                    }
                    else
                    {
                        ddl_sec.Enabled = false;
                    }
                }
                else
                {
                    ddl_sec.Enabled = false;
                }
                // ddl_sec.Items.Add(item);
            }

        }
        catch (Exception ex) { }
    }
    public void bindstream()
    {
        try
        {
            ddl_strm.Items.Clear();

            // string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.college_code=" + ddl_colg.SelectedItem.Value + "  and type<>'' order by type asc";
            string query = " select distinct type  from Course where college_code ='" + ddl_colg.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm.DataSource = ds;
                ddl_strm.DataTextField = "type";
                ddl_strm.DataValueField = "type";
                ddl_strm.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    protected void ddl_colg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindstream();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindSeat();
            bindsect();
            // getAdmissionNo();

        }
        catch (Exception ex) { }

    }
    protected void ddl_strm_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddeg();
        binddept();
        bindsem();
        bindsect();
        // getAdmissionNo();
    }
    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddeg();
            binddept();
            bindsem();
            bindsect();
            // getAdmissionNo();
        }
        catch (Exception ex) { }
    }
    protected void ddl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddept();
            bindsem();
            bindsect();
            // getAdmissionNo();
        }
        catch (Exception ex) { }
    }
    protected void ddl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        bindsect();

        //  getAdmissionNo();
    }
    protected void ddl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsect();
        //  getAdmissionNo();

    }
    protected void ddl_seattype_SelectedIndexChanged(object sender, EventArgs e)
    {

        // getAdmissionNo();
    }
    protected void ddl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void txt_roll_noNotApp_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_roll_no.Text.Trim() != "")
            {
                string rollNo = d2.GetFunction("select roll_no from Registration where roll_no='" + txt_roll_no.Text.Trim() + "'").Trim();
                if (rollNo != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Roll No Already Exists')", true);
                    txt_roll_no.Text = "";
                }
            }
        }
        catch { }
    }
    #endregion

    #region trans,refund,discontinue rb button
    protected void rb_transfer_Change(object sender, EventArgs e)
    {
        div_refundStudent.Visible = true;
        btn_cancel.Visible = false;
        otherdiv.Visible = false;
        ftype.Visible = false;
        tdJournalType.Visible = false;
        settType = 0;
        lnkindivmap.Visible = true;
        fldapplied.Visible = true;
        fldrefund.Visible = false;
        cbdisWithoutFees.Visible = false;
        fldadm.Visible = true;
        divTransfer.Visible = true;
        div_refund.Visible = false;
        rbl_AdmitTransfer.SelectedIndex = 0;
        rbl_AdmitTransfer_OnSelectedIndexChanged(sender, e);
        loadfromsetting();
        lbladvance.Visible = false;
        tbltrans.Visible = false;
        tbljournal.Visible = false;
        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_date.Attributes.Add("readonly", "readonly");
        btnsavePro.Visible = false;
        ddlJournalType.Visible = false;
        btnsavePro.Visible = false;
        tdRefund.Visible = false;
        DiscontinueReason.Visible = false;
        reasondis.Visible = false;
        refundStudOrStaff.Visible = false;//Added by saranya on 05April2018
        Rerollno.Visible = true;
        ddl_AmtPerc.Visible = false;
        lbladvance.Visible = false;
        LblRefund_staffid.Visible = false;
        txtRefund_staffid.Visible = false;
        LblRefund_staffName.Visible = false;
        txtRefund_staffName.Visible = false;
        LblRefund_staffCode.Visible = false;
        txtRefund_staffDept.Visible = false;

    }
    protected void rb_refund_Change(object sender, EventArgs e)
    {
        if (rbl_rollnoNewForRefund.Text == "Student")
        {
            otherdiv.Visible = false;
            ftype.Visible = false;
            tdJournalType.Visible = false;
            div_refundStudent.Visible = true;
            settType = 1;
            lnkindivmap.Visible = false;
            fldapplied.Visible = false;
            fldrefund.Visible = true;
            cbdisWithoutFees.Visible = false;
            fldadm.Visible = false;
            divTransfer.Visible = false;
            div_refund.Visible = true;

            refundStudOrStaff.Visible = true;//Added by saranya on 05April2018
            hostels.Visible = false;
            transport.Visible = false;
            loadrefundsetting();
            btn_refund.Text = "Refund";
            disTransClear();
            lbladvance.Visible = false;
            divref.Attributes.Add("Style", "border-radius: 10px; border: 1px solid Gray; width: 900px; height: 200px; overflow: auto;");
            tbltrans.Visible = true;
            tbljournal.Visible = false;
            txt_rdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_rdate.Attributes.Add("readonly", "readonly");
            btnsavePro.Visible = false;
            ddlJournalType.Visible = false;
            btnsavePro.Visible = false;
            tdRefund.Visible = false;
            btn_cancel.Visible = false;
            DiscontinueReason.Visible = false;
            reasondis.Visible = false;
            rcptSngleStaff.Visible = false;
            LblRefund_staffid.Visible = false;
            txtRefund_staffid.Visible = false;
            LblRefund_staffName.Visible = false;
            txtRefund_staffName.Visible = false;
            LblRefund_staffCode.Visible = false;
            txtRefund_staffDept.Visible = false;

            Rerollno.Visible = true;
            LblDate.Visible = true;
            txt_date.Visible = true;
            lbladvance.Visible = false;
        }

        if (rb_canceltranshostel.Checked == true)
        {
            div_refundStudent.Visible = true;
            hostels.Visible = true;
            transport.Visible = true;
            btn_cancel.Visible = true;
            DiscontinueReason.Visible = false;
            reasondis.Visible = false;
            refundStudOrStaff.Visible = false;//Added by saranya on 05April2018
            LblRefund_staffid.Visible = false;
            txtRefund_staffid.Visible = false;
            LblRefund_staffName.Visible = false;
            txtRefund_staffName.Visible = false;
            LblRefund_staffCode.Visible = false;
            txtRefund_staffDept.Visible = false;

            Rerollno.Visible = true;
            LblDate.Visible = true;
            txt_date.Visible = true;
            lbladvance.Visible = false;
        }

    }
    protected void rb_discont_Change(object sender, EventArgs e)
    {
        refundStudOrStaff.Visible = false;//Added by saranya on 05April2018
        btn_cancel.Visible = false;
        otherdiv.Visible = false;
        transport.Visible = false;
        hostels.Visible = false;
        ftype.Visible = false;
        tdJournalType.Visible = false;
        settType = 2;
        lnkindivmap.Visible = false;
        fldapplied.Visible = false;
        fldrefund.Visible = false;
        cbdisWithoutFees.Visible = true;
        fldadm.Visible = false;
        divTransfer.Visible = false;
        div_refund.Visible = true;
        loadrefundsetting();
        btn_refund.Text = "Discontinue";
        disTransClear();
        lbladvance.Visible = false;
        divref.Attributes.Add("Style", "border-radius: 10px; border: 1px solid Gray; width: 900px; height: 200px; overflow: auto;");
        tbltrans.Visible = true;
        tbljournal.Visible = false;
        txt_rdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_rdate.Attributes.Add("readonly", "readonly");
        btnsavePro.Visible = false;
        ddlJournalType.Visible = false;
        btnsavePro.Visible = false;
        tdRefund.Visible = false;
        DiscontinueReason.Visible = true;
        reasondis.Visible = true;
        div_refundStudent.Visible = true;
        Rerollno.Visible = true;
        LblDate.Visible = true;
        txt_date.Visible = true;
        lbladvance.Visible = false;
        ddl_AmtPerc.Visible = false;
        LblRefund_staffid.Visible = false;
        txtRefund_staffid.Visible = false;
        LblRefund_staffName.Visible = false;
        txtRefund_staffName.Visible = false;
        LblRefund_staffCode.Visible = false;
        txtRefund_staffDept.Visible = false;
    }
    protected void rb_Journal_Change(object sender, EventArgs e)
    {
        // settType = 2;
        btn_cancel.Visible = false;
        otherdiv.Visible = false;
        ftype.Visible = true;
        div_refundStudent.Visible = true;
        lnkindivmap.Visible = false;
        hostels.Visible = false;
        transport.Visible = false;
        fldapplied.Visible = false;
        fldrefund.Visible = false;
        cbdisWithoutFees.Visible = false;
        fldadm.Visible = false;
        divTransfer.Visible = false;
        div_refund.Visible = true;
        tdJournalType.Visible = true;
        tbltrans.Visible = true;
        loadrefundsetting();
        btn_refund.Text = "Journal Save";
        disTransClear();
        lbladvance.Visible = false;
        ddl_AmtPerc.Visible = false;
        ddlJournalType.Visible = true;
        gridView3.DataSource = null;
        gridView3.DataBind();
        divref.Attributes.Add("Style", "border-radius: 10px; border: 0px solid Gray; width: 900px; height: 200px; overflow: auto;");
        tbljournal.Visible = false;
        txt_rdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_rdate.Attributes.Add("readonly", "readonly");
        btnsavePro.Visible = false;
        btnsavePro.Visible = false;
        tdRefund.Visible = false;
        div_rcptSngleStaff.Visible = true;
        DiscontinueReason.Visible = false;
        reasondis.Visible = false;
        refundStudOrStaff.Visible = false;//Added by saranya on 05April2018
        Rerollno.Visible = true;
        LblDate.Visible = true;
        txt_date.Visible = true;
        txt_AmtPerc.Visible = false;
        lbladvance.Visible = false;
        ddl_AmtPerc.Visible = false;
        LblRefund_staffid.Visible = false;
        txtRefund_staffid.Visible = false;
        LblRefund_staffName.Visible = false;
        txtRefund_staffName.Visible = false;
        LblRefund_staffCode.Visible = false;
        txtRefund_staffDept.Visible = false;
    }
    //rb_ProlongAbsent_Change add by poomalar
    protected void rb_ProlongAbsent_Change(object sender, EventArgs e)
    {
        btn_cancel.Visible = false;
        otherdiv.Visible = false;
        hostels.Visible = false;
        transport.Visible = false;
        ftype.Visible = false;
        btnsavePro.Visible = true;
        tdJournalType.Visible = false;
        tbltrans.Visible = false;
        // settType = 2;
        lnkindivmap.Visible = false;
        fldapplied.Visible = false;
        fldrefund.Visible = false;
        cbdisWithoutFees.Visible = false;
        fldadm.Visible = false;
        divTransfer.Visible = false;
        div_refund.Visible = true;
        loadrefundsetting();
        btn_refund.Text = "Save";
        disTransClear();
        lbladvance.Visible = false;
        ddl_AmtPerc.Visible = false;
        ddlJournalType.Visible = false;
        gridView3.Visible = false;
        gridView3.DataSource = null;
        txt_AmtPerc.Visible = false;
        divref.Attributes.Add("Style", "border-radius: 10px; border: 0px solid Gray; width: 900px; height: 200px; overflow: auto;");
        tbljournal.Visible = false;
        txt_rdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_rdate.Attributes.Add("readonly", "readonly");
        btnsavePro.Visible = false;
        DiscontinueReason.Visible = false;
        reasondis.Visible = false;
        refundStudOrStaff.Visible = false;//Added by saranya on 05April2018
        LblRefund_staffid.Visible = false;
        txtRefund_staffid.Visible = false;
        LblRefund_staffName.Visible = false;
        txtRefund_staffName.Visible = false;
        LblRefund_staffCode.Visible = false;
        txtRefund_staffDept.Visible = false;
    }
    #endregion

    //applied and not applied
    protected void rbl_AdmitTransfer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //transFromClear();
        //transToClear();
        mapplingVisibile();
        if (rbl_AdmitTransfer.SelectedIndex == 0)
        {
            todivAdmit.Visible = true;
            todivnotAdmit.Visible = false;
            bindappliedclg();
        }
        else
        {
            todivAdmit.Visible = false;
            todivnotAdmit.Visible = true;
            bindclg();
        }
        // getAdmissionNo();
    }
    //enroll and not enroll
    protected void rbl_EnrollRefund_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //txt_AmtPerc.Text = "";
        //if (rbl_EnrollRefund.SelectedItem.Text.Trim() == "Enrolled")
        //{
        //    admis = 2;
        //    loadrefundsetting();
        //}
        //else
        //{
        //  //  loadEnorllapp();
        //}
    }
    //before and after admission
    protected void ddladmis_Selected(object sender, EventArgs e)
    {
        transFromClear();
        transToClear();
        if (ddladmis.SelectedIndex == 0)
        {
            rbl_rollno.Items.Clear();
            ListItem lst = new ListItem("App No", "3");
            rbl_rollno.Items.Add(lst);
            txt_roll.Attributes.Add("placeholder", "App No");
            chosedmode = 3;
            admis = 1;
        }
        else
        {
            chosedmode = 3;
            admis = 2;
            loadfromsetting();
        }
    }


    //roll,reg,admission no  source college
    public void loadfromsetting()
    {
        try
        {
            ListItem lst1 = new ListItem("Roll No", "0");
            ListItem lst2 = new ListItem("Reg No", "1");
            ListItem lst3 = new ListItem("Admission No", "2");
            ListItem lst4 = new ListItem("App No", "3");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rollno.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(lst4);
            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(lst1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    // txt_roll.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    // txt_roll.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    // txt_roll.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    // txt_roll.Text = "App No";
                    chosedmode = 3;
                    break;
            }

        }
        catch (Exception ex) { }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            transFromClear();
            transToClear();
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    //  rbl_rollno.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    // rbl_rollno.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    // rbl_rollno.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    // rbl_rollno.Text = "App No";
                    chosedmode = 3;
                    break;
            }
        }
        catch (Exception ex) { }
    }
    public void txt_roll_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string cursem = "";
            string studMode = string.Empty;
            bool boolClear = false;
            string rollno = Convert.ToString(txt_roll.Text);
            if (!string.IsNullOrEmpty(rollno))
            {
                string query = "";
                if (ddladmis.SelectedItem.Text.Trim() != "Before Admission")
                {
                    query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,seattype,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type,r.degree_code ,r.mode  from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "'";
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                    {
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                            query = query + "and r.Roll_no='" + rollno + "' ";
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                            query = query + "and r.Reg_No='" + rollno + "' ";
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                            query = query + "and r.Roll_Admit='" + rollno + "' ";
                    }
                    else
                    {
                        query = "select a.batch_year,a.Current_Semester,a.parent_name,r.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type,co.collname,co.college_code,seattype,''Sections,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type,r.degree_code,r.mode  from applyn a,Registration r,Degree d,Department dt,Course c,collinfo co where a.app_no=r.App_No and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and  a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code and admission_status =0   and isconfirm ='1' and app_formno = '" + rollno + "' and d.college_code='" + ddlcollege.SelectedItem.Value + "'";
                    }
                }
                else
                {
                    query = "select a.batch_year,a.Current_Semester,a.parent_name,a.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type,co.collname,co.college_code,seattype,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type,''Sections,a.degree_code,a.mode from applyn a,Degree d,Department dt,Course c,collinfo co where   a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code   and isconfirm ='1' and app_formno = '" + rollno + "' and d.college_code='" + ddlcollege.SelectedItem.Value + "'";
                }
                DataSet ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        txt_name.Text = ds1.Tables[0].Rows[i]["stud_name"].ToString();
                        txt_batch.Text = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_degree.Text = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_dept.Text = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_sec.Text = ds1.Tables[0].Rows[i]["Sections"].ToString();
                        txt_seattype.Text = ds1.Tables[0].Rows[i]["Seat_Type"].ToString();
                        txt_sem.Text = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        cursem = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        txt_colg.Text = ds1.Tables[0].Rows[i]["collname"].ToString();
                        txt_strm.Text = ds1.Tables[0].Rows[i]["type"].ToString();
                        string seatype = ds1.Tables[0].Rows[i]["seattype"].ToString();
                        Session["seatype"] = seatype;
                        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        //                       
                        lbltempfstclg.Text = ds1.Tables[0].Rows[i]["college_code"].ToString();
                        lbltempfstdeg.Text = ds1.Tables[0].Rows[i]["degree_code"].ToString();
                        studMode = Convert.ToString(ds1.Tables[0].Rows[i]["mode"]);
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        rollno = d2.GetFunction("select roll_no from registration where reg_no='" + rollno + "' and college_code='" + ddlcollege.SelectedItem.Value + "'");
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        rollno = d2.GetFunction("select roll_no from registration where Roll_admit='" + rollno + "' and college_code='" + ddlcollege.SelectedItem.Value + "'");
                    image2.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + rollno;
                    mapplingVisibile();
                    if (!string.IsNullOrEmpty(cursem) && (cursem == "1" || studMode == "3"))
                        boolClear = true;
                }
            }
            if (!boolClear)
                transFromClear();
        }
        catch (Exception ex) { }
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
            if (settType == 0)
            {
                #region transfer
                if (personmode == 0)
                {
                    if (chosedmode == 0)
                    {
                        query = "select top 100 Roll_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_No like '" + prefixText + "%' and r.college_code='" + stcollegecode + "'  order by  Roll_No asc";
                    }
                    else if (chosedmode == 1)
                    {
                        query = "select  top 100 Reg_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Reg_No like '" + prefixText + "%' and r.college_code='" + stcollegecode + "'  order by  Reg_No asc";
                    }
                    else if (chosedmode == 2)
                    {
                        query = "select  top 100 Roll_admit from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_admit like '" + prefixText + "%' and r.college_code='" + stcollegecode + "'  order by  Roll_admit asc";
                    }
                    else
                    {
                        if (admis == 2)
                        {
                            query = "  select  top 100 app_formno from applyn a ,Registration r where a.app_no=r.App_No and admission_status =1 and selection_status=1 and isconfirm ='1' and DelFlag =0 and app_formno like '" + prefixText + "%' and r.college_code='" + stcollegecode + "'  order by  app_formno asc";
                        }
                        else
                        {
                            query = "  select  top 100 app_formno from applyn where isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code='" + stcollegecode + "'  order by  app_formno asc";
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region refund,discontinue
                if (personmode == 0)
                {
                    if (chosedmode == 0)
                    {
                        query = "select top 100 Roll_No from Registration r where (r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_No like '" + prefixText + "%' and r.college_code='" + stcollegecode + "' order by  Roll_No asc";
                    }
                    else if (chosedmode == 1)
                    {
                        query = "select  top 100 Reg_No from Registration r where (r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Reg_No like '" + prefixText + "%' and r.college_code='" + stcollegecode + "' order by  Reg_No asc";
                    }
                    else if (chosedmode == 2)
                    {
                        query = "select  top 100 Roll_admit from Registration r where (r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_admit like '" + prefixText + "%' and r.college_code='" + stcollegecode + "' order by  Roll_admit asc";
                    }
                    else
                    {
                        if (admis == 2)
                        {
                            query = "  select  top 100 app_formno from applyn a ,Registration r where a.app_no=r.App_No and admission_status =1 and selection_status=1 and isconfirm ='1' and DelFlag =0 and app_formno like '" + prefixText + "%' and r.college_code='" + stcollegecode + "' order by  app_formno asc";
                        }
                        else
                        {
                            query = "  select  top 100 app_formno from applyn where isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code='" + stcollegecode + "' order by  app_formno asc";
                        }
                    }
                }
                #endregion
            }
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    //applied college application no generation
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetAppFormno(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select top 100 app_formno,app_no from applyn where  app_formno like '" + prefixText + "%' and  isconfirm='1' and isnull(admission_status,'0')='0' and college_code ='" + appliedClgCode + "'";
        name = ws.Getname(query);
        return name;
    }


    //added by abarna 17.01.2018
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getReceiptno(string prefixText)
    {
        WebService ws = new WebService();
        List<string> reciptno = new List<string>();
        string query = "select top 100 transcode from FT_FinDailyTransaction where MemType='4'";
        reciptno = ws.Getname(query);
        return reciptno;
    }
    public void txt_roll1_TextChanged(object sender, EventArgs e)
    {
        try
        {
            bool boolClear = false;
            string feecatagory = "";
            string rollno = Convert.ToString(txt_roll1.Text);
            if (!string.IsNullOrEmpty(rollno))
            {
                string appno = "";
                string query = "select a.parent_name,a.stud_name, a.Stud_Type,c.Course_Name,dt.Dept_Name,a.degree_code,a.Current_Semester  ,a.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,a.app_no,a.seattype,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type   from applyn a ,Degree d,course c,Department dt,collinfo co where  a.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code  and a.app_formno='" + rollno + "' and d.college_code='" + ddlclgapplied.SelectedValue + "'";
                DataSet ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        txt_batch1.Text = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_degree1.Text = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_dept1.Text = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_sec1.Text = "";// ds1.Tables[0].Rows[i]["Sections"].ToString();
                        txt_sem1.Text = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        txt_seat_type1.Text = ds1.Tables[0].Rows[i]["Seat_Type"].ToString();
                        int fee = Convert.ToInt32(ds1.Tables[0].Rows[i]["Current_Semester"]);
                        string clgName = Convert.ToString(ds1.Tables[0].Rows[i]["collname"]);
                        string clgcode = Convert.ToString(ds1.Tables[0].Rows[i]["college_code"]);
                        txt_colg1.Text = ds1.Tables[0].Rows[i]["collname"].ToString();
                        txt_strm1.Text = ds1.Tables[0].Rows[i]["type"].ToString();
                        lblDegCode.Text = ds1.Tables[0].Rows[i]["degree_code"].ToString();
                        appno = ds1.Tables[0].Rows[i]["app_no"].ToString();
                        feecatagory = bindstudsem(fee, clgcode);
                        string seatype = ds1.Tables[0].Rows[i]["seattype"].ToString();
                        Session["seatype"] = seatype;                                                //
                        lbltempsndclg.Text = clgcode;
                        lbltempsnddeg.Text = ds1.Tables[0].Rows[i]["degree_code"].ToString();
                        string applycurrentdate = DateTime.Now.ToString("MM/dd/yyyy");
                        boolClear = true;
                        //bool blAppNo = true;
                        //string cursem = Convert.ToString(ds1.Tables[0].Rows[i]["Current_Semester"]);
                        mapplingVisibile();
                        //collegecode = ddlclgapplied.SelectedValue;
                        // getAdmissionNo();
                        // admissionNumGeneration(appno, seatype, lblDegCode.Text, txt_batch1.Text, cursem, ref blAppNo, Convert.ToDateTime(applycurrentdate), collegecode);

                    }
                }
            }
            if (!boolClear)
            {
                transToClear();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void txt_roll_noApp_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_roll_no1.Text.Trim() != "")
            {
                string rollNo = d2.GetFunction("select roll_no from Registration where roll_no='" + txt_roll_no1.Text.Trim() + "'").Trim();
                if (rollNo != "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Roll No Already Exists')", true);
                    txt_roll_no1.Text = "";
                }
            }
        }
        catch { }
    }

    //addmission no generation
    private string generateApplNo(string collegecode, int degreecode, string edulevel, string mode, string seattype, string batchyear, out int format)
    {
        string applNo = string.Empty;
        format = 0;
        try
        {
            ApplicationNumberGeneration appGen = new ApplicationNumberGeneration();
            int codeCheck = 0;
            string query = "select LinkValue from New_InsSettings where LinkName='CollegewiseAdmissionNoRights' and user_code ='" + usercode + "' "; //and college_code ='" + collegecode + "'
            codeCheck = DirAccess.selectScalarInt(query);
            if (codeCheck > 0)
            {
                applNo = appGen.getApplicationNumber(collegecode, batchyear, 1);
                format = 1;
            }
            else
            {
                query = "select LinkValue from New_InsSettings where LinkName='EdulevelAdmissionNoRights' and user_code ='" + usercode + "' ";//and college_code ='" + collegecode + "'
                codeCheck = DirAccess.selectScalarInt(query);

                if (codeCheck > 0)
                {
                    applNo = appGen.getApplicationNumber(collegecode, edulevel, batchyear, 1);
                    format = 2;
                }
                else
                {
                    query = "select LinkValue from New_InsSettings where LinkName='DegreeSeatModewiseAdmissionNoRights' and user_code ='" + usercode + "' ";//and college_code ='" + collegecode + "'
                    codeCheck = DirAccess.selectScalarInt(query);
                    if (codeCheck > 0)
                    {
                        applNo = appGen.getApplicationNumber(collegecode, batchyear, degreecode.ToString(), mode, seattype, 1);
                        format = 3;
                    }
                    else
                    {
                        applNo = appGen.getApplicationNumber(collegecode, batchyear, degreecode, 1);
                        format = 0;
                    }
                }
            }
        }
        catch { applNo = string.Empty; }
        return applNo;
    }
    private bool UpdateApplNo(string collegecode, int degreecode, string edulevel, string mode, string seattype, string batchyear, int format)
    {
        bool update = false;

        try
        {
            ApplicationNumberGeneration appGen = new ApplicationNumberGeneration();
            int codeCheck = 0;
            string query = "select LinkValue from New_InsSettings where LinkName='CollegewiseAdmissionNoRights' and user_code ='" + usercode + "' "; //and college_code ='" + collegecode + "'
            codeCheck = DirAccess.selectScalarInt(query);
            if (codeCheck > 0)
            {
                update = appGen.updateApplicationNumber(collegecode, batchyear, 1);

            }
            else
            {
                query = "select LinkValue from New_InsSettings where LinkName='EdulevelAdmissionNoRights' and user_code ='" + usercode + "'"; // and college_code ='" + collegecode + "'
                codeCheck = DirAccess.selectScalarInt(query);

                if (codeCheck > 0)
                {
                    update = appGen.updateApplicationNumber(collegecode, edulevel, batchyear, 1);

                }
                else
                {
                    query = "select LinkValue from New_InsSettings where LinkName='DegreeSeatModewiseAdmissionNoRights' and user_code ='" + usercode + "' "; //and college_code ='" + collegecode + "'
                    codeCheck = DirAccess.selectScalarInt(query);
                    if (codeCheck > 0)
                    {
                        update = appGen.updateApplicationNumber(collegecode, batchyear, degreecode.ToString(), mode, seattype, 1);

                    }
                    else
                    {
                        update = appGen.updateApplicationNumber(collegecode, batchyear, degreecode, 1);

                    }
                }
            }
        }
        catch { update = false; }
        return update;
    }

    protected void getAdmissionNo()
    {
        try
        {
            txt_roll_no.Text = string.Empty;
            string sndClgcode = string.Empty;
            string sndBatchYr = string.Empty;
            string sndDegreecode = string.Empty;
            string sndSection = string.Empty;
            string sndSeat = string.Empty;
            string collegecodeTemp = string.Empty;
            string semester = string.Empty;
            admissionNoGeneration();
            if (rbl_AdmitTransfer.SelectedIndex == 0)
                collegecodeTemp = Convert.ToString(ddlclgapplied.SelectedValue);
            else
                collegecodeTemp = Convert.ToString(ddl_colg.SelectedValue);
            if (rbl_AdmitTransfer.SelectedIndex == 0)
            {
                sndClgcode = Convert.ToString(lbltempsndclg.Text);
                sndBatchYr = Convert.ToString(txt_batch1.Text);
                sndDegreecode = Convert.ToString(lbltempsnddeg.Text);
                sndSection = Convert.ToString(txt_sec1.Text);
                sndSeat = Convert.ToString(txt_seat_type1.Text);
                sndSeat = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + collegecodeTemp + "'  and textval='" + sndSeat.Trim() + "'"));
            }
            else
            {
                if (ddl_colg.Items.Count > 0)
                    sndClgcode = Convert.ToString(ddl_colg.SelectedItem.Value);
                if (ddl_batch.Items.Count > 0)
                    sndBatchYr = Convert.ToString(ddl_batch.SelectedItem.Value);
                if (ddl_dept.Items.Count > 0)
                    sndDegreecode = Convert.ToString(ddl_dept.SelectedItem.Value);
                if (ddl_sec.Items.Count > 0)
                    sndSection = Convert.ToString(ddl_sec.SelectedItem.Value);
                if (ddl_seattype.Items.Count > 0)
                    sndSeat = Convert.ToString(ddl_seattype.SelectedItem.Value);
                if (ddl_sem.Items.Count > 0)//abarna
                    semester = Convert.ToString(ddl_sem.SelectedItem.Value);
            }
            if (validateAdmissionCheck())//admission no check
            {
                int format = 0;
                string eduleve = Convert.ToString(d2.GetFunction(" select distinct edu_level,degree_code from degree d,course  c where d.course_id=c.course_id and d.college_code='" + collegecodeTemp + "' and d.degree_code='" + sndDegreecode + "'"));
                string Mode = string.Empty;
                string appNo = getappNo();
                if (appNo != "0" && !string.IsNullOrEmpty(appNo))
                    Mode = Convert.ToString(d2.GetFunction(" select mode from applyn where app_no='" + appNo + "' and college_code='" + ddlcollege.SelectedValue + "'"));
                string paavaiNewApplcationNO = string.Empty;
                string admNo = string.Empty;
                if (admisionvalue == "1")//abarna add 24.03.18
                {
                    paavaiNewApplcationNO = d2.GetFunction("select linkvalue from New_InsSettings where LinkName='Common Application Number Settings' and  college_code  in('" + ddlcollege.SelectedValue + "')");//abarna 24.03.18//user_code ='" + usercode + "' and  abarna
                    if (string.IsNullOrEmpty(paavaiNewApplcationNO) || paavaiNewApplcationNO == "0")
                        admNo = generateApplNo(collegecodeTemp, Convert.ToInt32(sndDegreecode), eduleve, Mode, sndSeat, sndBatchYr, out format);//genearateAdmissionNo(collegeCode, degreecode, batchYr);
                    else
                        admNo = autoGenDS.AdmissionNoAndApplicationNumberGeneration(0, appno: appNo, Mode: Mode, DegreeCode: sndDegreecode, CollegeCode: collegecodeTemp, SeatType: sndSeat, BatchYear: sndBatchYr, Semester: semester);//abarna 24.03.18
                }
                //else
                //    admNo = app_fromno;

                //  string admNo = generateApplNo(collegecodeTemp, Convert.ToInt32(sndDegreecode), eduleve, Mode, sndSeat, sndBatchYr, out format);
                if (rbl_AdmitTransfer.SelectedIndex == 0)
                    txt_roll_no1.Text = admNo;
                else
                    txt_roll_no.Text = admNo;
            }
        }
        catch { }
    }

    protected void UpdateAdmissionNo(string appNo)
    {
        try
        {
            string sndClgcode = string.Empty;
            string sndBatchYr = string.Empty;
            string sndDegreecode = string.Empty;
            string sndSection = string.Empty;
            string sndSeat = string.Empty;
            string collegecodeTemp = string.Empty;
            if (rbl_AdmitTransfer.SelectedIndex == 0)
                collegecodeTemp = Convert.ToString(ddlclgapplied.SelectedValue);
            else
                collegecodeTemp = Convert.ToString(ddl_colg.SelectedValue);
            if (rbl_AdmitTransfer.SelectedIndex == 0)
            {
                sndClgcode = Convert.ToString(lbltempsndclg.Text);
                sndBatchYr = Convert.ToString(txt_batch1.Text);
                sndDegreecode = Convert.ToString(lbltempsnddeg.Text);
                sndSection = Convert.ToString(txt_sec1.Text);
                sndSeat = Convert.ToString(txt_seat_type1.Text);
                sndSeat = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + collegecodeTemp + "'  and textval='" + sndSeat.Trim() + "'"));
            }
            else
            {
                if (ddl_colg.Items.Count > 0)
                    sndClgcode = Convert.ToString(ddl_colg.SelectedItem.Value);
                if (ddl_batch.Items.Count > 0)
                    sndBatchYr = Convert.ToString(ddl_batch.SelectedItem.Value);
                if (ddl_dept.Items.Count > 0)
                    sndDegreecode = Convert.ToString(ddl_dept.SelectedItem.Value);
                if (ddl_sec.Items.Count > 0)
                    sndSection = Convert.ToString(ddl_sec.SelectedItem.Value);
                if (ddl_seattype.Items.Count > 0)
                    sndSeat = Convert.ToString(ddl_seattype.SelectedItem.Value);
            }
            string rollNo = string.Empty;
            if (rbl_AdmitTransfer.SelectedIndex == 0)
                rollNo = txt_roll_no1.Text.Trim();
            else
                rollNo = txt_roll_no.Text.Trim();
            if (!string.IsNullOrEmpty(rollNo) && rollNo != "0")
            {
                int format = 0;
                string eduleve = Convert.ToString(d2.GetFunction(" select distinct edu_level,degree_code from degree d,course  c where d.course_id=c.course_id and d.college_code='" + collegecodeTemp + "' and d.degree_code='" + sndDegreecode + "'"));
                string Mode = string.Empty;
                if (appNo != "0" && !string.IsNullOrEmpty(appNo))
                    Mode = Convert.ToString(d2.GetFunction(" select mode from applyn where app_no='" + appNo + "' "));//and college_code='" + ddlcollege.SelectedValue + "'
                UpdateApplNo(collegecodeTemp, Convert.ToInt32(sndDegreecode), eduleve, Mode, sndSeat, sndBatchYr, format);
            }
        }
        catch { }
    }
    protected bool validateAdmissionCheck()
    {
        bool check = false;
        try
        {
            string fstClgcode = string.Empty;
            string fstBatchYr = string.Empty;
            string fstDegreecode = string.Empty;
            string fstSection = string.Empty;
            string fstSeat = string.Empty;

            string sndClgcode = string.Empty;
            string sndBatchYr = string.Empty;
            string sndDegreecode = string.Empty;
            string sndSection = string.Empty;
            string sndSeat = string.Empty;

            fstClgcode = Convert.ToString(lbltempfstclg.Text);
            fstBatchYr = Convert.ToString(txt_batch.Text);
            fstDegreecode = Convert.ToString(lbltempfstdeg.Text);
            fstSection = Convert.ToString(txt_sec.Text);
            fstSeat = Convert.ToString(txt_seattype.Text);

            string applno = string.Empty;
            if (rbl_AdmitTransfer.SelectedIndex == 0)
            {
                sndClgcode = Convert.ToString(lbltempsndclg.Text);
                sndBatchYr = Convert.ToString(txt_batch1.Text);
                sndDegreecode = Convert.ToString(lbltempsnddeg.Text);
                sndSection = Convert.ToString(txt_sec1.Text);
                sndSeat = Convert.ToString(txt_seat_type1.Text);
            }
            else
            {
                if (ddl_colg.Items.Count > 0)
                    sndClgcode = Convert.ToString(ddl_colg.SelectedItem.Value);
                if (ddl_batch.Items.Count > 0)
                    sndBatchYr = Convert.ToString(ddl_batch.SelectedItem.Value);
                if (ddl_dept.Items.Count > 0)
                    sndDegreecode = Convert.ToString(ddl_dept.SelectedItem.Value);
                if (ddl_sec.Items.Count > 0)
                    sndSection = Convert.ToString(ddl_sec.SelectedItem.Value);
                if (ddl_seattype.Items.Count > 0)
                    sndSeat = Convert.ToString(ddl_seattype.SelectedItem.Text);

            }

            if (!string.IsNullOrEmpty(fstClgcode) && !string.IsNullOrEmpty(fstBatchYr) && !string.IsNullOrEmpty(fstDegreecode) && !string.IsNullOrEmpty(sndClgcode)
&& !string.IsNullOrEmpty(sndBatchYr) && !string.IsNullOrEmpty(sndDegreecode))
            {
                if (fstClgcode == sndClgcode)
                {
                    if (fstDegreecode == sndDegreecode)
                    {
                        if (fstSeat == sndSeat)
                            check = false;
                        else
                            check = true;
                    }
                    else
                        check = true;
                }
                else
                    check = true;
            }
        }
        catch { }
        return check;

    }
    protected string getappNo()
    {
        string appNo = string.Empty;
        try
        {
            string roll = Convert.ToString(txt_roll.Text);
            string selQ = " select app_no from registration where roll_no='" + roll + "' and college_code='" + ddlcollege.SelectedValue + "'";
            appNo = Convert.ToString(d2.GetFunction(selQ));
            if (appNo == "0")
            {
                selQ = " select app_no from registration where reg_no='" + roll + "' and college_code='" + ddlcollege.SelectedValue + "'";
                appNo = Convert.ToString(d2.GetFunction(selQ));
            }
            if (appNo == "0")
            {
                selQ = " select app_no from registration where roll_admit='" + roll + "' and college_code='" + ddlcollege.SelectedValue + "'";
                appNo = Convert.ToString(d2.GetFunction(selQ));
            }
            if (appNo == "0")
            {
                selQ = " select app_no from applyn where app_formno='" + roll + "' and college_code='" + ddlcollege.SelectedValue + "'";
                appNo = Convert.ToString(d2.GetFunction(selQ));
            }

        }
        catch { appNo = "0"; }
        return appNo;
    }
    //new admission number generation 
    protected void admissionNumGeneration(string app_no, string seattype, string degreecode, string batchyear, string cursem, ref bool blAppNo, DateTime applycurrentdate, string collegeCode)
    {
        try
        {
            //Admit
            string rolladmit = "";
            string approve = "";
            string stud_name = string.Empty;
            string app_fromno = string.Empty;
            string batchYr = string.Empty;
            string Mode = string.Empty;
            string eduleve = string.Empty;
            admissionNoGeneration();
            string selQ = "select seattype,stud_name,app_formno,batch_year,mode,(select Edu_Level from course c,Degree d where d.Course_Id=c.Course_Id and a.degree_code=d.Degree_Code) as Edulevel from applyn a where app_no ='" + app_no + "'";
            DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                seattype = Convert.ToString(dsval.Tables[0].Rows[0]["seattype"]);
                stud_name = Convert.ToString(dsval.Tables[0].Rows[0]["stud_name"]);
                app_fromno = Convert.ToString(dsval.Tables[0].Rows[0]["app_formno"]);
                batchYr = Convert.ToString(dsval.Tables[0].Rows[0]["batch_year"]);
                Mode = Convert.ToString(dsval.Tables[0].Rows[0]["mode"]);
                eduleve = Convert.ToString(dsval.Tables[0].Rows[0]["Edulevel"]);
            }
            if (string.IsNullOrEmpty(Mode))
                Mode = "1";
            int format = 0;
            string paavaiNewApplcationNO = string.Empty;
            if (admisionvalue == "1")//Barath add 09.01.18
            {
                paavaiNewApplcationNO = d2.GetFunction("select linkvalue from New_InsSettings where LinkName='Common Application Number Settings' and  college_code  in('" + collegeCode + "') ");//barath 10.01.18//user_code ='" + usercode + "' and 01/02/2018 barath
                if (string.IsNullOrEmpty(paavaiNewApplcationNO) || paavaiNewApplcationNO == "0")
                    rolladmit = generateApplNo(collegeCode, Convert.ToInt32(degreecode), eduleve, Mode, seattype, batchYr, out format);//genearateAdmissionNo(collegeCode, degreecode, batchYr);
                else
                    rolladmit = autoGenDS.AdmissionNoAndApplicationNumberGeneration(0, appno: app_no, Mode: Mode, DegreeCode: degreecode, CollegeCode: collegeCode, SeatType: seattype);//barath 24.01.18
            }
            else
                rolladmit = app_fromno;
            if (rolladmit.Trim() == "0" || string.IsNullOrEmpty(rolladmit))
                rolladmit = app_fromno;
            //string regEntry = "  if exists(select * from Registration where App_No='" + app_no + "')  delete from Registration where App_No='" + app_no + "' insert into Registration (App_No,Adm_Date,Roll_Admit,Roll_No,RollNo_Flag,Reg_No,Stud_Name,Batch_Year,degree_code,college_code,CC,DelFlag,Exam_Flag,Current_Semester,mode)values('" + app_no + "','" + System.DateTime.Now.ToString("yyy/MM/dd") + "','" + rolladmit + "','" + rolladmit + "','1','" + rolladmit + "','" + stud_name + "','" + batchyear + "','" + degreecode + "','" + collegeCode + "','0','0','OK','" + cursem + "','" + Mode + "')";
            //int s = d2.update_method_wo_parameter(regEntry, "Text");

            if (string.IsNullOrEmpty(paavaiNewApplcationNO) || paavaiNewApplcationNO == "0")
                UpdateApplNo(collegeCode, Convert.ToInt32(degreecode), eduleve, Mode, seattype, batchYr, format);//Barath add 09.01.18
            blAppNo = true;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, Convert.ToString(collegeCode), "studAdmissionSelection"); }
    }
    protected void admissionNoGeneration()
    {
        try
        {
            string value = d2.GetFunction("select value from Master_Settings where settings ='Admission No Rights' and usercode ='" + usercode + "'");
            if (value == "1")
                admisionvalue = "1";
            else
                admisionvalue = "0";
        }
        catch { }
    }
    //refund settings
    public void loadrefundsetting()
    {
        try
        {
            ListItem list1 = new ListItem("Roll No", "0");
            ListItem list2 = new ListItem("Reg No", "1");
            ListItem list3 = new ListItem("Admission No", "2");
            ListItem list4 = new ListItem("App No", "3");
            ListItem lst5 = new ListItem("Smartcard No", "4");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rerollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rerollno.Items.Add(list1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rerollno.Items.Add(list2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rerollno.Items.Add(list3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code ='" + ddlcollege.SelectedValue + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rerollno.Items.Add(list4);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptSmartNo' and user_code ='" + usercode + "' --and college_code in(" + ddlcollege.SelectedValue + ") ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Smartcard No - smart_serial_no
                rbl_rollno.Items.Add(lst5);
            }
            int smartDisp = Convert.ToInt32(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' --and college_code in (" + ddlcollege.SelectedValue + ")").Trim());

            if (rbl_rerollno.Items.Count == 0)
            {
                rbl_rerollno.Items.Add(list1);
            }
            switch (Convert.ToUInt32(rbl_rerollno.SelectedItem.Value))
            {
                case 0:
                case1:
                    txt_rerollno.Attributes.Add("placeholder", "Roll No");
                    // txt_roll.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                case2:
                    txt_rerollno.Attributes.Add("placeholder", "Reg No");
                    // txt_roll.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                case3:
                    txt_rerollno.Attributes.Add("placeholder", "Admin No");
                    // txt_roll.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                case4:
                    txt_rerollno.Attributes.Add("placeholder", "App No");
                    // txt_roll.Text = "App No";
                    chosedmode = 3;
                    break;
                case 4:
                    txt_rerollno.Attributes.Add("placeholder", "Smartcard No");
                    //txt_roll.Text = "SmartCard No";
                    chosedmode = 4;
                    switch (smartDisp)
                    {
                        case 0:
                            goto case1;
                        case 1:
                            goto case2;
                        case 2:
                            goto case3;
                        case 3:
                            goto case4;
                    }
                    break;
            }

        }
        catch (Exception ex) { }
    }
    protected void rbl_rerollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        disTransClear();
        switch (Convert.ToUInt32(rbl_rerollno.SelectedItem.Value))
        {
            case 0:
                txt_rerollno.Attributes.Add("Placeholder", "Roll No");
                chosedmode = 0;
                break;
            case 1:
                txt_rerollno.Attributes.Add("Placeholder", "Reg No");
                chosedmode = 1;
                break;
            case 2:
                txt_rerollno.Attributes.Add("Placeholder", "Admin No");
                chosedmode = 2;
                break;
            case 3:
                txt_rerollno.Attributes.Add("Placeholder", "App No");
                chosedmode = 3;
                break;
        }
    }
    public void txt_rerollno_TextChanged(object sender, EventArgs e)
    {
        //modified by sudhagar 29.08.2017-journal already paid amount adjust settings
        if (rb_Journal.Checked)
        {
            getJournalChange();
        }
        else if (rb_ProlongAbsent.Checked)
        {
            btnsavePro.Visible = true;
            getJournalChange();
            lbladvance.Visible = false;
            ddl_AmtPerc.Visible = false;
            ddlJournalType.Visible = false;
            gridView3.Visible = false;
            lnkJournal.Visible = false;
            tblgrid3.Visible = false;
            Fieldset1.Visible = false;
            btnsavePro.Visible = true;

            //rb_ProlongAbsent_Change(sender, e);
        }
        else//refund and discontinue
        {
            getRefundDistChange();
        }

        //else//refund and discontinue
        //{
        //    btnsavePro.Visible = false;
        //    getRefundDistChange();
        //}
    }


    protected void getRefundDistChange()
    {
        try
        {
            txt_reamt.Text = "";
            string rollno = Convert.ToString(txt_rerollno.Text);
            string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type   from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code and r.college_code='" + ddlcollege.SelectedValue + "' ";
            //and r.Roll_no='" + rollno + "'";
            if (!string.IsNullOrEmpty(rollno))
            {
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                {
                    if (rb_refund.Checked == false)
                    {
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                            query = query + "and r.Roll_no='" + rollno + "'  and DelFlag =0 ";
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                            query = query + "and r.Reg_No='" + rollno + "' and  DelFlag =0";
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                            query = query + "and r.Roll_Admit='" + rollno + "' and DelFlag =0 ";

                    }
                    if (rb_refund.Checked == true)
                    {
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                            query = query + "and r.Roll_no='" + rollno + "'  --and DelFlag =0 ";
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                            query = query + "and r.Reg_No='" + rollno + "' --and  DelFlag =0";
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                            query = query + "and r.Roll_Admit='" + rollno + "' --and DelFlag =0 ";
                    }
                }
                else
                {
                    if (rb_refund.Checked == true)
                    {
                        if (rbl_EnrollRefund.SelectedItem.Text == "Enrolled")
                        {
                            query = "select a.batch_year,a.Current_Semester,a.parent_name,r.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type,co.collname,co.college_code from applyn a,Registration r,Degree d,Department dt,Course c,collinfo co where a.app_no=r.App_No  and  a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code and admission_status =1 and selection_status=1 and isconfirm ='1' and app_formno = '" + rollno + "' and r.college_code='" + ddlcollege.SelectedValue + "'";
                        }
                        else
                        {
                            query = "  select a.batch_year,a.Current_Semester,a.parent_name,a.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type,co.collname,co.college_code from applyn a,Degree d,Department dt,Course c,collinfo co where  a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code and  isconfirm ='1' and app_formno = '" + rollno + "' and a.college_code='" + ddlcollege.SelectedValue + "'";
                        }
                    }
                    else
                    {
                        query = "select a.batch_year,a.Current_Semester,a.parent_name,r.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type,co.collname,co.college_code from applyn a,Registration r,Degree d,Department dt,Course c,collinfo co where a.app_no=r.App_No  and  a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code and admission_status =1 and selection_status=1 and isconfirm ='1' and app_formno = '" + rollno + "' and r.college_code='" + ddlcollege.SelectedValue + "'";
                    }

                }
                if (rb_canceltranshostel.Checked == true)
                {
                    query = "select  distinct a.batch_year,r.Current_Semester,a.parent_name,r.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL(c.type,'') as type,co.collname,co.college_code,s.stage_name,rm.Route_id,v.veh_id,hn.hostelname,bl.building_name,ro.room_name from applyn a,Degree d,Department dt,Course c,collinfo co,Registration r left join ht_hostelregistration hr on r.app_no=hr.app_no left join  hm_hostelmaster hn on hr.hostelmasterfk=hn.hostelmasterpK left join building_master bl on convert(varchar(10),hr.buildingfk)=bl.code left join room_detail ro on hr.roomfk=ro.roompk left join stage_master s on r.boarding=convert(varchar(10),s.stage_id) left join routemaster rm on rm.route_id=r.bus_RouteID left join vehicle_master v on v.veh_id=r.vehid  where a.app_no=r.App_No  and  a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code and  selection_status=1 and isconfirm ='1' and roll_no = '" + rollno + "' and r.college_code='" + ddlcollege.SelectedValue + "'";//admission_status =1 and
                }
                DataSet ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        // txt_rerollno.Text = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                        txt_rename.Text = ds1.Tables[0].Rows[i]["stud_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_rebatch.Text = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_redegree.Text = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_redept.Text = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        //  txt_resec.Text = ds1.Tables[0].Rows[i]["Sections"].ToString();
                        txt_resem.Text = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        txt_recolg.Text = ds1.Tables[0].Rows[i]["collname"].ToString();
                        txt_restrm.Text = ds1.Tables[0].Rows[i]["type"].ToString(); // jairam
                        Session["clgcode"] = Convert.ToString(ds1.Tables[0].Rows[i]["college_code"]);
                        if (rb_canceltranshostel.Checked == true)
                        {
                            routetxt.Text = ds1.Tables[0].Rows[i]["route_id"].ToString();
                            vehicletxt.Text = ds1.Tables[0].Rows[i]["veh_id"].ToString();
                            stagetxt.Text = ds1.Tables[0].Rows[i]["stage_name"].ToString();
                            txt_hostel.Text = ds1.Tables[0].Rows[i]["hostelname"].ToString();
                            txt_build.Text = ds1.Tables[0].Rows[i]["building_name"].ToString();
                            txt_roomname.Text = ds1.Tables[0].Rows[i]["room_name"].ToString();
                        }
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                        rollno = d2.GetFunction("select roll_no from registration where reg_no='" + rollno + "' and college_code='" + ddlcollege.SelectedValue + "' ");
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                        rollno = d2.GetFunction("select roll_no from registration where Roll_admit='" + rollno + "' and college_code='" + ddlcollege.SelectedValue + "'");
                    image3.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + rollno;
                    txt_AmtPerc.Text = "";
                    if (rb_discont.Checked)
                    {
                        tblgrid3.Visible = true;
                        bindRefund();
                        txt_reamt.Enabled = true;
                        ddl_AmtPerc.Enabled = true;
                        ddl_AmtPerc.Visible = true;
                        txt_AmtPerc.Enabled = true;
                        txt_AmtPerc.Visible = true;
                        tdRefund.Visible = true;
                    }
                    else
                    {
                        tblgrid3.Visible = true;
                        bindRefund();
                        txt_reamt.Enabled = true;
                        ddl_AmtPerc.Enabled = true;
                        txt_AmtPerc.Enabled = true;
                        ddl_AmtPerc.Visible = true;
                        txt_AmtPerc.Visible = true;
                        tdRefund.Visible = true;
                    }
                }
                else
                    disTransClear();
            }
            else
                disTransClear();
        }
        catch (Exception ex) { }
    }
    protected void ddl_AmtPerc_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_AmtPerc.Text = "";
        bindRefund();
    }
    protected void chk_refCommon_OnCheckedChanged(object sender, EventArgs e)
    {
        #region commoncheck
        try
        {
            Hashtable refundsetting = new Hashtable();
            if (txt_rerollno.Text.Trim() != "")
            {
                string stream = txt_restrm.Text.Trim();
                string edulevel = "";
                string sem = "";
                string semesterquery = "";
                string selqyery = "select r.Current_Semester,C.type,c.Edu_Level   from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code  and r.Roll_no='" + txt_rerollno.Text.Trim() + "' and d.college_code=" + ddlcollege.SelectedValue + "";
                DataSet dss = new DataSet();
                dss = d2.select_method_wo_parameter(selqyery, "Text");
                if (dss.Tables.Count > 0)
                {
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        edulevel = Convert.ToString(dss.Tables[0].Rows[0]["Edu_Level"]);
                        sem = Convert.ToString(dss.Tables[0].Rows[0]["Current_Semester"]);
                    }
                }
                if (chk_refCommon.Checked == true)
                {

                    string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(settingquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                        if (linkvalue == "0")
                        {
                            semesterquery = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and textval like '" + sem + " Semester' and college_code='" + ddlcollege.SelectedValue + "' and textval not like '-1%'");
                        }
                        else
                        {
                            semesterquery = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and textval like '" + sem + " Year' and college_code='" + ddlcollege.SelectedValue + "' and textval not like '-1%'");
                        }
                    }

                    string selectquery = "select HeaderFK,LedgerFK,ConsPer,ConsAmt from FM_ConcessionRefundSettings where RefMode =2 and  Stream in ('" + stream + "') and Edu_Level in('" + edulevel + "')   and Fee_Category in(" + semesterquery + ")";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[row]["ConsAmt"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[row]["ConsAmt"]).Trim() != "0.00")
                            {
                                refundsetting.Add(Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]), Convert.ToString(ds.Tables[0].Rows[row]["ConsAmt"]) + "-1");
                            }
                            if (Convert.ToString(ds.Tables[0].Rows[row]["ConsPer"]).Trim() != "" && Convert.ToString(ds.Tables[0].Rows[row]["ConsPer"]).Trim() != "0.00")
                            {
                                refundsetting.Add(Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]), Convert.ToString(ds.Tables[0].Rows[row]["ConsPer"]) + "-2");
                            }
                        }
                        double totrefundvalue = 0;
                        double totrefunretun = 0;
                        string RefundAmount = string.Empty;
                        double refundAmountVal = 0;
                        if (refundsetting.Count > 0)
                        {
                            if (gridView3.Rows.Count > 0)
                            {
                                for (int ro = 0; ro < gridView3.Rows.Count; ro++)
                                {
                                    string getvalue = Convert.ToString((gridView3.Rows[ro].FindControl("lbl_lgrid") as Label).Text);
                                    string gettotamt = Convert.ToString((gridView3.Rows[ro].FindControl("lbl_paid") as Label).Text);
                                    if (getvalue.Trim() != "")
                                    {
                                        double finamt = 0;
                                        double finper = 0;
                                        if (refundsetting.ContainsKey(Convert.ToString(getvalue)))
                                        {
                                            string getamount = Convert.ToString(refundsetting[Convert.ToString(getvalue)]);
                                            string[] split = getamount.Split('-');
                                            string secondvalue = Convert.ToString(split[1]);
                                            if (Convert.ToString(split[1]) == "1")
                                            {

                                                string amonut = Convert.ToString(split[0]);
                                                //(gridView3.Rows[ro].FindControl("txt_refund") as TextBox).Text = amonut; //Modified by saranya on 17/04/2018
                                                (gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = amonut;
                                                if (amonut != "" && gettotamt != "")
                                                {
                                                    if (Convert.ToDouble(gettotamt) >= Convert.ToDouble(amonut))
                                                    {
                                                        finamt = Convert.ToDouble(gettotamt) - Convert.ToDouble(amonut);
                                                        //(gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = Convert.ToString(finamt); //Modified by saranya on 17/04/2018
                                                        (gridView3.Rows[ro].FindControl("txt_refund") as TextBox).Text = Convert.ToString(finamt);
                                                    }
                                                    else
                                                    {
                                                        finamt = 0;
                                                        //(gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = Convert.ToString(finamt);//Modified by saranya on 17/04/2018
                                                        (gridView3.Rows[ro].FindControl("txt_refund") as TextBox).Text = Convert.ToString(finamt);
                                                    }
                                                }

                                            }
                                            else if (Convert.ToString(split[1]) == "2")
                                            {
                                                double refunvalue = 0;
                                                double percent = Convert.ToDouble(split[0]);
                                                //(gridView3.Rows[ro].FindControl("txt_refund") as TextBox).Text = Convert.ToString(percent);//Modified by saranya on 17/04/2018
                                                (gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = Convert.ToString(percent);
                                                if (Convert.ToString(percent) != "" && getamount != "")
                                                {
                                                    if (Convert.ToDouble(gettotamt) >= Convert.ToDouble(percent))
                                                    {
                                                        finper = Convert.ToDouble(gettotamt) * Convert.ToDouble(percent) / Convert.ToDouble(100);
                                                        refunvalue = Convert.ToDouble(gettotamt) - Convert.ToDouble(finper);
                                                        //(gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = Convert.ToString(refunvalue); //Modified by saranya on 17/04/2018
                                                        (gridView3.Rows[ro].FindControl("txt_refund") as TextBox).Text = Convert.ToString(refunvalue);
                                                    }
                                                    else
                                                    {
                                                        finper = 0;
                                                        (gridView3.Rows[ro].FindControl("txt_refund") as TextBox).Text = Convert.ToString(finper);
                                                        //(gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = Convert.ToString(finper);//Modified by saranya on 17/04/2018
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //(gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text = Convert.ToString(gettotamt);//Modified by saranya on 17/04/2018
                                            (gridView3.Rows[ro].FindControl("txt_refund") as TextBox).Text = Convert.ToString(gettotamt);
                                        }
                                    }
                                    //totrefundvalue = Convert.ToDouble((gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text);//Modified by saranya on 17/04/2018
                                    totrefundvalue = Convert.ToDouble((gridView3.Rows[ro].FindControl("txt_refund") as TextBox).Text);
                                    RefundAmount = (gridView3.Rows[ro].FindControl("txt_refundbal") as TextBox).Text;//Added by saranya on 17/04/2018
                                    if (totrefundvalue > 0)
                                    {
                                        totrefunretun += Convert.ToDouble(totrefundvalue);
                                    }
                                    //added by saranya on 17/04/2018
                                    if (RefundAmount != "")
                                    {
                                        refundAmountVal += Convert.ToDouble(RefundAmount);
                                    }
                                }
                                txt_reamt.Text = Convert.ToString(refundAmountVal);
                                txt_AmtPerc.Text = Convert.ToString(totrefunretun);
                            }
                        }
                    }
                }
                else
                {
                    //  bindGrid2();
                    txt_reamt.Text = "";
                    txt_AmtPerc.Text = "";
                }
            }
        }
        catch (Exception ex) { }
        #endregion
    }
    public void bindRefund()
    {
        if (rbl_rollnoNewForRefund.Text == "Student")
        {
            string app_no = "";
            DataTable dt = new DataTable();
            dt.Columns.Add("Sno");
            dt.Columns.Add("YearSem");
            dt.Columns.Add("Header");
            dt.Columns.Add("HeaderFk");
            dt.Columns.Add("Ledger");
            dt.Columns.Add("LedgerFk");
            dt.Columns.Add("FeeCategory");
            dt.Columns.Add("Concession");
            dt.Columns.Add("Paid");
            dt.Columns.Add("Balance");
            dt.Columns.Add("Total");
            dt.Columns.Add("finyear");//abarna
            dt.Columns.Add("FeeAmt");
            dt.Columns.Add("RefundAmt");
            DataRow dr;
            double total = 0;
            double balance = 0;
            double paid = 0;
            if (txt_rerollno.Text.Trim() != "")
            {
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                {
                    app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
                }
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                {
                    app_no = d2.GetFunction("select app_no from Registration where Reg_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
                }
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                {
                    app_no = d2.GetFunction("select app_no from Registration where Roll_admit='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
                }
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
                {
                    app_no = d2.GetFunction("select app_no from applyn where app_formno='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
                }
            }
            if (app_no != "")
            {
                string selectQ = "";
                if (rb_refund.Checked == true)
                {
                    if (rbl_EnrollRefund.SelectedItem.Text == "Enrolled")
                    {
                        //Commented by saranya on 19/04/2018
                        //selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,(isnull(F.PaidAmount,0)-isnull(f.refundamount,0)) as paidamount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(refundamount,0) as refundamount   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0 and (isnull(PaidAmount,0)- isnull(refundamount,0)>0 )   and r.App_No=" + app_no + " and isnull(IsTransfer,'0')='0' order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";
                        // order by F.FeeCategory
                        selectQ = "   select distinct f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,(isnull(F.PaidAmount,0)-isnull(f.refundamount,0)) as paidamount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(refundamount,0) as refundamount,hp.UserCode,finyearfk from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L,FS_HeaderPrivilage hp,FS_LedgerPrivilage lp where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0 and (isnull(PaidAmount,0)- isnull(refundamount,0)>0 )   and r.App_No=" + app_no + " and isnull(IsTransfer,'0')='0' and hp.HeaderFK=H.HeaderPK and hp.UserCode='" + usercode + "' and hp.UserCode=lp.UserCode and hp.HeaderFK=lp.HeaderFK order by f.HeaderFK,F.FeeCategory,l.ledgerName asc";//Added by saranya on 19/04/2018 finyearfk added abarna
                    }
                    else
                    {
                        //Commented by saranya on 19/04/2018
                        //selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,(isnull(F.PaidAmount,0)-isnull(f.refundamount,0)) as paidamount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(refundamount,0) as refundamount   from FT_FeeAllot f,applyn R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0 and (isnull(PaidAmount,0)- isnull(refundamount,0)>0 )   and r.App_No=" + app_no + " and isnull(IsTransfer,'0')='0' order by isnull(l.priority,1000), l.ledgerName asc, F.FeeCategory";

                        selectQ = " select distinct f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,(isnull(F.PaidAmount,0)-isnull(f.refundamount,0)) as paidamount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(refundamount,0) as refundamount,finyearfk   from FT_FeeAllot f,applyn R,FM_HeaderMaster H,FM_LedgerMaster L,FS_HeaderPrivilage hp,FS_LedgerPrivilage lp where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0 and (isnull(PaidAmount,0)- isnull(refundamount,0)>0 )   and r.App_No=" + app_no + " and isnull(IsTransfer,'0')='0' and hp.HeaderFK=H.HeaderPK and hp.usercode='" + usercode + "' and hp.UserCode=lp.UserCode and hp.HeaderFK=lp.HeaderFK order by f.HeaderFK,F.FeeCategory,l.ledgerName asc";//Added by saranya on 19/04/2018
                    }
                }
                else
                {
                    selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,(isnull(F.PaidAmount,0)-isnull(f.refundamount,0)) as paidamount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(refundamount,0) as refundamount,finyearfk   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0 and (isnull(PaidAmount,0)- isnull(refundamount,0)>0 ) and isnull(IsTransfer,'0')='0'  and r.App_No=" + app_no + " order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";
                }
                if (rb_canceltranshostel.Checked == true)//added by abarna
                {
                    string hdFK = string.Empty;
                    string ldFK = string.Empty;
                    string selQ = string.Empty;
                    string save0 = string.Empty;
                    string HeaderFK = string.Empty;
                    string LedgerFK = string.Empty;
                    if (txt_hostel.Text != "")
                    {
                        selQ = " select LinkValue,college_code from New_InsSettings where LinkName='Hostel_Admission_Form_Fee'  and user_code='" + usercode + "' and college_code in('" + ddlcollege.SelectedValue + "')";
                        DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                        if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                        {
                            for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                            {
                                save0 = Convert.ToString(d2.GetFunction(selQ));
                                if (save0.Trim() != "")
                                {
                                    string[] admissionformfee = save0.Split('$');//1$9,10$500
                                    if (admissionformfee.Length > 1)
                                    {
                                        if (Convert.ToString(admissionformfee[0]) == "1")
                                        {
                                            //cb_hosteladmissionformfee.Checked = true;
                                            if (Convert.ToString(admissionformfee[1]).Trim() != "")
                                            {
                                                string[] headled = Convert.ToString(admissionformfee[1]).Split(',');
                                                if (headled.Length > 1)
                                                {
                                                    hdFK = Convert.ToString(headled[0]);
                                                    HeaderFK += "'" + "," + "'" + hdFK;
                                                    ldFK = Convert.ToString(headled[1]);
                                                    LedgerFK += "'" + "," + "'" + ldFK;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,(isnull(F.PaidAmount,0)-isnull(f.refundamount,0)) as paidamount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(refundamount,0) as refundamount,finyearfk   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0 and (isnull(PaidAmount,0)- isnull(refundamount,0)>0 )   and r.App_No=" + app_no + " and f.headerfk in('" + HeaderFK + "') and isnull(IsTransfer,'0')='0' order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";//and f.ledgerfk in('" + LedgerFK + "') 
                    }
                    else
                    {
                        selQ = " select LinkValue,college_code from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code in('" + ddlcollege.SelectedValue + "')";
                        DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                        if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                        {
                            for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                            {
                                string linkValue = Convert.ToString(dsVal.Tables[0].Rows[row]["LinkValue"]);
                                string clgcode = Convert.ToString(dsVal.Tables[0].Rows[row]["college_code"]);
                                string[] leng = linkValue.Split(',');
                                if (leng.Length == 2)
                                {
                                    hdFK = Convert.ToString(leng[0]);
                                    HeaderFK += "'" + "," + "'" + hdFK;
                                    ldFK = Convert.ToString(leng[1]);
                                    LedgerFK += "'" + "," + "'" + ldFK;
                                }
                            }
                        }
                        selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,(isnull(F.PaidAmount,0)-isnull(f.refundamount,0)) as paidamount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(refundamount,0) as refundamount,finyearfk   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0 and (isnull(PaidAmount,0)- isnull(refundamount,0)>0 )   and r.App_No=" + app_no + " and f.headerfk in('" + HeaderFK + "')  and f.ledgerfk in('" + LedgerFK + "') and  isnull(IsTransfer,'0')='0' order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";//
                    }
                }

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQ, "Text");
                if (ds.Tables.Count > 0)
                {
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                        string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + ddlcollege.SelectedValue + "");
                        dr = dt.NewRow();
                        dr["Sno"] = row + 1;
                        dr["YearSem"] = cursem;
                        dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                        dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                        dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                        dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                        dr["FeeCategory"] = feecat;
                        dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                        dr["Paid"] = Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]);
                        dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                        dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                        dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                        dr["RefundAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["refundamount"]);

                        dr["finyear"] = Convert.ToString(ds.Tables[0].Rows[row]["finyearfk"]);//abarna
                        dt.Rows.Add(dr);

                        total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);
                        balance += Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]);
                        paid += Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                gridView3.DataSource = dt;
                gridView3.DataBind();
                lbl_grid3_bal.Text = "Rs." + balance.ToString();
                lbl_grid3_paid.Text = "Rs." + paid.ToString();
                lbl_grid3_tot.Text = "Rs." + total.ToString();
                tblgrid3.Visible = true;
                gridView3.Visible = true;
            }
            else
            {
                gridView3.DataSource = null;
                gridView3.DataBind();
                tblgrid3.Visible = false;
                gridView3.Visible = false;
            }
            if (gridView3.Rows.Count > 0)
            {
                foreach (GridViewRow rows in gridView3.Rows)
                {
                    TextBox txtAmt = (TextBox)rows.Cells[9].FindControl("txt_refund");
                    if (ddl_AmtPerc.SelectedIndex == 0)
                    {
                        txtAmt.ReadOnly = false;
                    }
                    else
                    {
                        txtAmt.ReadOnly = false;
                    }
                    if (rb_refund.Checked == true && rbl_rollnoNewForRefund.Text == "Student")
                        chk_refCommon.Visible = true;//Added by saranya on 17/04/2018
                    if (rb_refund.Checked == true && rbl_rollnoNewForRefund.Text == "Staff")
                        chk_refCommon.Visible = false;//Added by saranya on 17/04/2018
                }
            }
        }

        #region StaffRefund Added by saranya on 6/04/2018
        if (rbl_rollnoNewForRefund.Text == "Staff")
        {
            string app_no = "";
            DataTable dt = new DataTable();
            dt.Columns.Add("Sno");
            dt.Columns.Add("YearSem");
            dt.Columns.Add("Header");
            dt.Columns.Add("HeaderFk");
            dt.Columns.Add("Ledger");
            dt.Columns.Add("LedgerFk");
            dt.Columns.Add("FeeCategory");
            dt.Columns.Add("Concession");
            dt.Columns.Add("Paid");
            dt.Columns.Add("Balance");
            dt.Columns.Add("Total");
            dt.Columns.Add("FeeAmt");
            dt.Columns.Add("RefundAmt");
            dt.Columns.Add("finyear");
            DataRow dr;
            double total = 0;
            double balance = 0;
            double paid = 0;

            if (txtRefund_staffid.Text.Trim() != "")
            {

                app_no = d2.GetFunction("select appl_id from staff_appl_master Sa,staffmaster Sm where sm.appl_no=sa.appl_no and staff_code='" + txtRefund_staffid.Text.Trim() + "' and sa.college_code='" + ddlcollege.SelectedValue + "'");

            }
            if (app_no != "")
            {
                string selectQ = "";

                selectQ = "  select distinct f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,isnull(F.DeductAmout,0) as DeductAmout,(isnull(F.PaidAmount,0)-isnull(f.refundamount,0)) as paidamount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(refundamount,0) as refundamount   from FT_FeeAllot f,staffmaster Sm,staff_appl_master Sa,FM_HeaderMaster H,FM_LedgerMaster L,FS_HeaderPrivilage hp,FS_LedgerPrivilage lp where  sa.appl_id=f.App_No and sa.appl_no=sm.appl_no and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and  isnull(PaidAmount,0)<>0 and (isnull(PaidAmount,0)- isnull(refundamount,0)>0 ) and f.App_No='" + app_no + "' and hp.HeaderFK=H.HeaderPK and hp.usercode='" + usercode + "' and hp.UserCode=lp.UserCode and hp.HeaderFK=lp.HeaderFK order by f.HeaderFK,l.ledgerName asc";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQ, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {

                        dr = dt.NewRow();
                        dr["Sno"] = row + 1;

                        dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                        dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                        dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                        dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                        dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                        dr["Paid"] = Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]);
                        dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                        dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                        dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                        dr["RefundAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["refundamount"]);
                        dt.Rows.Add(dr);
                        total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);
                        balance += Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]);
                        paid += Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                gridView3.DataSource = dt;
                gridView3.DataBind();
                lbl_grid3_bal.Text = "Rs." + balance.ToString();
                lbl_grid3_paid.Text = "Rs." + paid.ToString();
                lbl_grid3_tot.Text = "Rs." + total.ToString();
                tblgrid3.Visible = true;
                gridView3.Visible = true;
            }
            else
            {
                gridView3.DataSource = null;
                gridView3.DataBind();
                tblgrid3.Visible = false;
                gridView3.Visible = false;
            }
            if (gridView3.Rows.Count > 0)
            {
                foreach (GridViewRow rows in gridView3.Rows)
                {
                    TextBox txtAmt = (TextBox)rows.Cells[9].FindControl("txt_refund");
                    if (ddl_AmtPerc.SelectedIndex == 0)
                    {
                        txtAmt.ReadOnly = false;
                    }
                    else
                    {
                        txtAmt.ReadOnly = false;
                    }
                    if (rb_refund.Checked == true && rbl_rollnoNewForRefund.Text == "Staff")
                        chk_refCommon.Visible = false;//Added by saranya on 17/04/2018
                }
            }
        }
        #endregion
    }
    protected void gridView3_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Text = lblsem.Text;
        }
    }

    protected void btn_refund_Click(object sender, EventArgs e)
    {
        if (txt_rerollno.Text.Trim() != "")
        {
            if (rb_refund.Checked == true)
                CurrentrefundMethod();  // refundMethod();
            else if (rb_discont.Checked == true)
                divReuseRoll.Visible = true;
            else if (rb_canceltranshostel.Checked == true)
                CurrentrefundMethod();
        }
        if (rbl_rollnoNewForRefund.Text == "Staff")
        {
            if (txtRefund_staffid.Text.Trim() != "")
            {
                if (rb_refund.Checked == true)
                    CurrentrefundMethod();  // refundMethod();
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Enter Roll Number')", true);
        }
    }
    private void refundMethod()
    {
        try
        {
            int check = 0;
            double refuntaken = 0;
            bool Amt = false;
            foreach (GridViewRow grid in gridView3.Rows)
            {
                TextBox refuntakens = (TextBox)grid.FindControl("txt_refund");
                double.TryParse(Convert.ToString(refuntakens.Text), out refuntaken);
                if (refuntaken != 0)
                    Amt = true;
            }

            string appno = "";
            string rollno = txt_rerollno.Text.Trim();
            if (Amt == true)
            {
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                    {
                        appno = d2.GetFunction("select app_no from Registration where roll_no='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'");//and cc<>'1' and DelFlag<>1
                        check = 0;
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                    {
                        appno = d2.GetFunction("select app_no from Registration where reg_no='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'");//and cc<>'1' and DelFlag<>1
                        check = 0;
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                    {
                        appno = d2.GetFunction("select app_no from Registration where Roll_admit='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'");//and cc<>'1' and DelFlag<>1
                        check = 0;
                    }
                }
                //  if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
                else
                {
                    appno = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "' and Admission_Status=1 and selection_status=1 and IsConfirm=1 and college_code='" + ddlcollege.SelectedValue + "'");
                    check = 1;
                    // appno = "  and a.app_formno  = '" + rollno + "' ";
                }
                string finYearid = d2.getCurrentFinanceYear(usercode, ddlcollege.SelectedValue);
                string[] dtsplit = txt_rdate.Text.Split('/');
                DateTime dtdate = Convert.ToDateTime(dtsplit[1] + "/" + dtsplit[0] + "/" + dtsplit[2]);
                if (appno != "" && appno != "0")
                {
                    if (txt_reamt.Text.Trim() == "")
                        txt_reamt.Text = "0";
                    string upExcess = " if exists (select * from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2' ) update FT_ExcessDet set ExcessAmt =ExcessAmt +'" + txt_reamt.Text + "',BalanceAmt =BalanceAmt +'" + txt_reamt.Text + "' where App_No ='" + appno + "' and ExcessType ='2'  else  INSERT INTO FT_ExcessDet (ExcessTransDate,TransTime ,DailyTransCode,App_No ,MemType ,ExcessType ,ExcessAmt,AdjAmt,BalanceAmt,FinYearFK ) VALUES ('" + dtdate + "','" + DateTime.Now.ToLongTimeString() + "' ,'', " + appno + " , 1 , 2 ," + txt_reamt.Text + ",0," + txt_reamt.Text + "," + finYearid + ")";
                    d2.update_method_wo_parameter(upExcess, "Text");

                    string getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2'");
                    ArrayList arSem = new ArrayList();
                    foreach (GridViewRow rows in gridView3.Rows)
                    {
                        Label hdrid = (Label)rows.Cells[10].FindControl("lbl_hdrid");
                        Label lgrid = (Label)rows.Cells[10].FindControl("lbl_lgrid");
                        Label feecatid = (Label)rows.Cells[10].FindControl("lbl_feecat");
                        Label paidamt = (Label)rows.Cells[10].FindControl("lbl_paid");
                        TextBox txtAmt = (TextBox)rows.Cells[10].FindControl("txt_refund");
                        TextBox txtrefundAmt = (TextBox)rows.Cells[10].FindControl("txt_refundbal");
                        if (!arSem.Contains(feecatid.Text))
                        {

                        }
                        if (txtrefundAmt.Text != "" && txtrefundAmt.Text != "0")
                        {
                            string upRefundQ = " update FT_FeeAllot set RefundAmount= ISNull(RefundAmount,0)+ " + txtrefundAmt.Text + ", IsRefund='1' where app_no=" + appno + " and HeaderFK=" + hdrid.Text + " and Ledgerfk=" + lgrid.Text + " and FeeCategory=" + feecatid.Text + " and isnull(IsTransfer,'0')='0'='0'";
                            d2.update_method_wo_parameter(upRefundQ, "Text");

                            upExcess = "if exists ( select * from FT_ExcessLedgerDet where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "')update FT_ExcessLedgerDet set ExcessAmt  =ExcessAmt +'" + txtrefundAmt.Text + "',BalanceAmt =BalanceAmt +'" + txtrefundAmt.Text + "' where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK) values ('" + hdrid.Text + "','" + lgrid.Text + "','" + txtrefundAmt.Text + "','" + txtrefundAmt.Text + "','" + getvalue + "')";
                            d2.update_method_wo_parameter(upExcess, "Text");
                        }
                    }
                    disTransClear();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Refunded Sucessfully')", true);
                    Div8.Visible = true;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Already Refunded')", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Enter The Refund Amount')", true);
        }
        catch (Exception ex) { }
    }
    private void CurrentrefundMethod()
    {
        try
        {
            string refundamount = string.Empty;
            if (rbl_rollnoNewForRefund.Text == "Student")
            {
                int check = 0;
                double refuntaken = 0;
                double refund = 0;
                bool Amt = false;
                foreach (GridViewRow grid in gridView3.Rows)
                {
                    TextBox refuntakens = (TextBox)grid.FindControl("txt_refund");
                    TextBox txtRefund = (TextBox)grid.FindControl("txt_refundbal");
                    double.TryParse(Convert.ToString(refuntakens.Text), out refuntaken);
                    double.TryParse(Convert.ToString(txtRefund.Text), out refund);
                    if (refuntaken != 0)
                        Amt = true;
                    if (refuntaken == 0 && refund != 0)
                        Amt = true;
                }

                string appno = "";
                string rollno = txt_rerollno.Text.Trim();
                if (Amt == true)
                {
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                    {
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                        {
                            appno = d2.GetFunction("select app_no from Registration where roll_no='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'");//and cc<>'1' and DelFlag<>1
                            check = 0;
                        }
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                        {
                            appno = d2.GetFunction("select app_no from Registration where reg_no='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'");//and cc<>'1' and DelFlag<>1
                            check = 0;
                        }
                        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                        {
                            appno = d2.GetFunction("select app_no from Registration where Roll_admit='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'");//and cc<>'1' and DelFlag<>1
                            check = 0;
                        }
                    }
                    //  if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
                    else
                    {
                        appno = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "' and Admission_Status=1 and selection_status=1 and IsConfirm=1 and college_code='" + ddlcollege.SelectedValue + "'");
                        check = 1;
                        // appno = "  and a.app_formno  = '" + rollno + "' ";
                    }
                    string finYearid = d2.getCurrentFinanceYear(usercode, ddlcollege.SelectedValue);
                    string[] dtsplit = txt_rdate.Text.Split('/');
                    DateTime dtdate = Convert.ToDateTime(dtsplit[1] + "/" + dtsplit[0] + "/" + dtsplit[2]);
                    Dictionary<string, string> getSem = getFeeWise();
                    if (appno != "" && appno != "0")
                    {
                        //if (txt_reamt.Text.Trim() == "")
                        //    txt_reamt.Text = "0";
                        string getvalue = string.Empty;
                        foreach (GridViewRow rows in gridView3.Rows)
                        {
                            Label hdrid = (Label)rows.Cells[10].FindControl("lbl_hdrid");
                            Label lgrid = (Label)rows.Cells[10].FindControl("lbl_lgrid");
                            Label feecatid = (Label)rows.Cells[10].FindControl("lbl_feecat");
                            Label paidamt = (Label)rows.Cells[10].FindControl("lbl_paid");
                            TextBox txtAmt = (TextBox)rows.Cells[10].FindControl("txt_refund");
                            TextBox txtrefundAmt = (TextBox)rows.Cells[10].FindControl("txt_refundbal");

                            int RowCnt = Convert.ToInt32(rows.RowIndex);
                            Label finyear = (Label)gridView3.Rows[RowCnt].FindControl("lbl_finyear");




                            double refundAmt = 0;
                            double.TryParse(Convert.ToString(txtrefundAmt.Text), out refundAmt);
                            if (getSem.Count > 0 && getSem.ContainsKey(feecatid.Text))
                            {
                                string amt = Convert.ToString(getSem[feecatid.Text]);
                                if (checkSchoolSetting() == 0)
                                {
                                    string upExcess = " if exists (select * from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2' and ex_journalentry='0' and feecategory='" + feecatid.Text + "' and finyearfk='" + finYearid + "' and actualfinyearfk='" + finyear.Text + "' ) update FT_ExcessDet set ExcessAmt =ExcessAmt +'" + amt + "',BalanceAmt =BalanceAmt +'" + amt + "' where App_No ='" + appno + "' and ExcessType ='2' and ex_journalentry='0' and feecategory='" + feecatid.Text + "' and finyearfk='" + finYearid + "' and actualfinyearfk='" + finyear.Text + "' else  INSERT INTO FT_ExcessDet (ExcessTransDate,TransTime ,DailyTransCode,App_No ,MemType ,ExcessType ,ExcessAmt,AdjAmt,BalanceAmt,FinYearFK,ex_journalentry,feecategory,actualfinyearfk) VALUES ('" + dtdate + "','" + DateTime.Now.ToLongTimeString() + "' ,'', " + appno + " , 1 , 2 ," + amt + ",0," + amt + "," + finYearid + ",'0','" + feecatid.Text + "','" + finyear.Text + "')";
                                    d2.update_method_wo_parameter(upExcess, "Text");
                                }
                                else
                                {
                                    string upExcess = " if exists (select * from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2' and ex_journalentry='0' and feecategory='" + feecatid.Text + "' ) update FT_ExcessDet set ExcessAmt =ExcessAmt +'" + amt + "',BalanceAmt =BalanceAmt +'" + amt + "' where App_No ='" + appno + "' and ExcessType ='2' and ex_journalentry='0' and feecategory='" + feecatid.Text + "'  else  INSERT INTO FT_ExcessDet (ExcessTransDate,TransTime ,DailyTransCode,App_No ,MemType ,ExcessType ,ExcessAmt,AdjAmt,BalanceAmt,FinYearFK,ex_journalentry,feecategory ) VALUES ('" + dtdate + "','" + DateTime.Now.ToLongTimeString() + "' ,'', " + appno + " , 1 , 2 ," + amt + ",0," + amt + "," + finYearid + ",'0','" + feecatid.Text + "')";
                                    d2.update_method_wo_parameter(upExcess, "Text");
                                }
                                getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2' and ex_journalentry='0' and feecategory='" + feecatid.Text + "'");
                                //getSem.Remove(feecatid.Text);
                            }
                            //ArrayList arSem = new ArrayList();
                            double semTotAmt = 0;

                            semTotAmt += refundAmt;
                            if (!string.IsNullOrEmpty(getvalue) && getvalue != "0" && txtrefundAmt.Text != "" && txtrefundAmt.Text != "0")
                            {
                                if (checkSchoolSetting() == 0)
                                {
                                    string upRefundQ = " update FT_FeeAllot set RefundAmount= ISNull(RefundAmount,0)+ " + txtrefundAmt.Text + ", IsRefund='1' where app_no=" + appno + " and HeaderFK=" + hdrid.Text + " and Ledgerfk=" + lgrid.Text + " and FeeCategory=" + feecatid.Text + " and IsTransfer='0' and finyearfk='" + finyear.Text + "'";//abarna
                                    d2.update_method_wo_parameter(upRefundQ, "Text");
                                }
                                else
                                {
                                    string upRefundQ = " update FT_FeeAllot set RefundAmount= ISNull(RefundAmount,0)+ " + txtrefundAmt.Text + ", IsRefund='1' where app_no=" + appno + " and HeaderFK=" + hdrid.Text + " and Ledgerfk=" + lgrid.Text + " and FeeCategory=" + feecatid.Text + " and IsTransfer='0'";
                                    d2.update_method_wo_parameter(upRefundQ, "Text");
                                }
                                if (checkSchoolSetting() == 0)
                                {
                                    string upExcess = "if exists ( select * from FT_ExcessLedgerDet where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "' and feecategory='" + feecatid.Text + "' and finyearfk='" + finyear.Text + "')update FT_ExcessLedgerDet set ExcessAmt  =ExcessAmt +'" + txtrefundAmt.Text + "',BalanceAmt =BalanceAmt +'" + txtrefundAmt.Text + "'  where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "' and feecategory='" + feecatid.Text + "' and  finyearfk='" + finyear.Text + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,feecategory,finyearfk) values ('" + hdrid.Text + "','" + lgrid.Text + "','" + txtrefundAmt.Text + "','" + txtrefundAmt.Text + "','" + getvalue + "','" + feecatid.Text + "','" + finyear.Text + "')";
                                    d2.update_method_wo_parameter(upExcess, "Text");
                                }
                                else
                                {
                                    string upExcess = "if exists ( select * from FT_ExcessLedgerDet where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "' and feecategory='" + feecatid.Text + "')update FT_ExcessLedgerDet set ExcessAmt  =ExcessAmt +'" + txtrefundAmt.Text + "',BalanceAmt =BalanceAmt +'" + txtrefundAmt.Text + "' where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "' and feecategory='" + feecatid.Text + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,feecategory) values ('" + hdrid.Text + "','" + lgrid.Text + "','" + txtrefundAmt.Text + "','" + txtrefundAmt.Text + "','" + getvalue + "','" + feecatid.Text + "')";
                                    d2.update_method_wo_parameter(upExcess, "Text");
                                }

                                refundamount = txt_reamt.Text;
                            }
                        }
                        disTransClear();
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Refunded Sucessfully')", true);
                        Div8.Visible = true;
                        //==========================Added by Saranya on 10/04/2018=============================//
                        int savevalue = 1;
                        string col_Code = ddlcollege.SelectedItem.Value;
                        string entrycode = Session["Entry_Code"].ToString();
                        string formname = "Refund";
                        string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                        string doa = DateTime.Now.ToString("MM/dd/yyy");
                        IPHostEntry host;
                        string localip = "";
                        host = Dns.GetHostEntry(Dns.GetHostName());
                        foreach (IPAddress ip in host.AddressList)
                        {
                            if (ip.AddressFamily.ToString() == "InterNetwork")
                            {
                                localip = ip.ToString();
                            }
                        }
                        string details = "RollNo - " + rollno + ": CollegeCode - " + col_Code + ": RefundAmount - " + refundamount + " : Date - " + toa + " ";
                        string ctsname = "";
                        if (savevalue == 1)
                        {
                            ctsname = "Amount Refund";
                        }
                        string hostName = Dns.GetHostName();
                        d2.insertEinanceUserActionLog(entrycode, formname, 1, toa, doa, details, ctsname, localip);

                        //============================================================================//
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Already Refunded')", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Enter The Refund Amount')", true);
            }

            if (rbl_rollnoNewForRefund.Text == "Staff")
            {
                int check = 0;
                double refuntaken = 0;
                double refund = 0;
                bool Amt = false;
                foreach (GridViewRow grid in gridView3.Rows)
                {
                    TextBox refuntakens = (TextBox)grid.FindControl("txt_refund");
                    TextBox txtRefund = (TextBox)grid.FindControl("txt_refundbal");
                    double.TryParse(Convert.ToString(refuntakens.Text), out refuntaken);
                    double.TryParse(Convert.ToString(txtRefund.Text), out refund);
                    if (refuntaken != 0)
                        Amt = true;
                    if (refuntaken == 0 && refund != 0)
                        Amt = true;
                }

                string appno = "";
                string staffcode = txtRefund_staffid.Text.Trim();
                if (Amt == true)
                {
                    appno = d2.GetFunction("select appl_id from staff_appl_master Sa,staffmaster Sm where sm.appl_no=sa.appl_no and staff_code='" + staffcode + "' and sa.college_code='" + ddlcollege.SelectedValue + "'");
                    string finYearid = d2.getCurrentFinanceYear(usercode, ddlcollege.SelectedValue);
                    string[] dtsplit = txt_rdate.Text.Split('/');
                    DateTime dtdate = Convert.ToDateTime(dtsplit[1] + "/" + dtsplit[0] + "/" + dtsplit[2]);
                    if (appno != "" && appno != "0")
                    {

                        string getvalue = string.Empty;
                        foreach (GridViewRow rows in gridView3.Rows)
                        {
                            Label hdrid = (Label)rows.Cells[10].FindControl("lbl_hdrid");
                            Label lgrid = (Label)rows.Cells[10].FindControl("lbl_lgrid");
                            Label feecatid = (Label)rows.Cells[10].FindControl("lbl_feecat");
                            Label paidamt = (Label)rows.Cells[10].FindControl("lbl_paid");
                            TextBox txtAmt = (TextBox)rows.Cells[10].FindControl("txt_refund");
                            TextBox txtrefundAmt = (TextBox)rows.Cells[10].FindControl("txt_refundbal");
                            double refundAmt = 0;
                            double.TryParse(Convert.ToString(txtrefundAmt.Text), out refundAmt);

                            if (txtrefundAmt.Text != "" && txtrefundAmt.Text != "0")
                            {
                                string upExcess = " if exists (select * from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2' and ex_journalentry='0' and feecategory='" + feecatid.Text + "' ) update FT_ExcessDet set ExcessAmt =ExcessAmt +'" + txtrefundAmt.Text + "',BalanceAmt =BalanceAmt +'" + txtrefundAmt.Text + "' where App_No ='" + appno + "' and ExcessType ='2' and ex_journalentry='0' and feecategory='" + feecatid.Text + "'  else  INSERT INTO FT_ExcessDet (ExcessTransDate,TransTime ,DailyTransCode,App_No ,MemType ,ExcessType ,ExcessAmt,AdjAmt,BalanceAmt,FinYearFK,ex_journalentry,feecategory ) VALUES ('" + dtdate + "','" + DateTime.Now.ToLongTimeString() + "' ,'', " + appno + " , 1 , 2 ," + txtrefundAmt.Text + ",0," + txtrefundAmt.Text + "," + finYearid + ",'0','" + feecatid.Text + "')";
                                d2.update_method_wo_parameter(upExcess, "Text");

                                getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2' and ex_journalentry='0' and feecategory='" + feecatid.Text + "'");

                                double semTotAmt = 0;

                                semTotAmt += refundAmt;

                                string upStaffRefundQ = " update FT_FeeAllot set RefundAmount= ISNull(RefundAmount,0)+ " + txtrefundAmt.Text + ", IsRefund='1' where app_no=" + appno + " and HeaderFK=" + hdrid.Text + " and Ledgerfk=" + lgrid.Text + "  and IsTransfer='0'";
                                d2.update_method_wo_parameter(upStaffRefundQ, "Text");

                                string upStaffExcess = "if exists ( select * from FT_ExcessLedgerDet where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "' and feecategory='" + feecatid.Text + "')update FT_ExcessLedgerDet set ExcessAmt  =ExcessAmt +'" + txtrefundAmt.Text + "',BalanceAmt =BalanceAmt +'" + txtrefundAmt.Text + "' where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "' and feecategory='" + feecatid.Text + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,feecategory) values ('" + hdrid.Text + "','" + lgrid.Text + "','" + txtrefundAmt.Text + "','" + txtrefundAmt.Text + "','" + getvalue + "','" + feecatid.Text + "')";
                                d2.update_method_wo_parameter(upStaffExcess, "Text");
                                refundamount = txt_reamt.Text;
                            }
                        }
                        disTransClear();
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Refunded Sucessfully')", true);
                        Div8.Visible = true;
                        //==========================Added by Saranya on 10/04/2018=============================//
                        int savevalue = 1;
                        string col_Code = ddlcollege.SelectedItem.Value;
                        string entrycode = Session["Entry_Code"].ToString();
                        string formname = "Refund";
                        string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                        string doa = DateTime.Now.ToString("MM/dd/yyy");
                        IPHostEntry host;
                        string localip = "";
                        host = Dns.GetHostEntry(Dns.GetHostName());
                        foreach (IPAddress ip in host.AddressList)
                        {
                            if (ip.AddressFamily.ToString() == "InterNetwork")
                            {
                                localip = ip.ToString();
                            }
                        }
                        string details = "StaffCode - " + staffcode + ": CollegeCode - " + col_Code + ": RefundAmount - " + refundamount + " : Date - " + toa + " ";
                        string ctsname = "";
                        if (savevalue == 1)
                        {
                            ctsname = "AmountRefund";
                        }
                        string hostName = Dns.GetHostName();
                        d2.insertEinanceUserActionLog(entrycode, formname, 1, toa, doa, details, ctsname, localip);
                        //============================================================================//
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Already Refunded')", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Enter The Refund Amount')", true);
            }
        }
        catch (Exception ex) { }
    }
    protected Dictionary<string, string> getFeeWise()
    {
        Dictionary<string, string> getFee = new Dictionary<string, string>();
        try
        {
            ArrayList arSem = new ArrayList();
            foreach (GridViewRow rows in gridView3.Rows)
            {
                Label hdrid = (Label)rows.Cells[10].FindControl("lbl_hdrid");
                Label lgrid = (Label)rows.Cells[10].FindControl("lbl_lgrid");
                Label feecatid = (Label)rows.Cells[10].FindControl("lbl_feecat");
                Label paidamt = (Label)rows.Cells[10].FindControl("lbl_paid");
                TextBox txtAmt = (TextBox)rows.Cells[10].FindControl("txt_refund");
                TextBox txtrefundAmt = (TextBox)rows.Cells[10].FindControl("txt_refundbal");
                double excessAmt = 0;
                //double paidAmt = 0;
                //double balAmt = 0;
                //double tobePaid = 0;
                double.TryParse(Convert.ToString(txtrefundAmt.Text), out excessAmt);
                if (excessAmt != 0)
                {
                    // Label finyear = (Label)rows.Cells[3].FindControl("lbl_finyear");
                    if (!getFee.ContainsKey(feecatid.Text))
                        getFee.Add(feecatid.Text, Convert.ToString(excessAmt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(getFee[feecatid.Text]), out amount);
                        amount += excessAmt;
                        getFee.Remove(feecatid.Text);
                        getFee.Add(feecatid.Text, Convert.ToString(amount));
                    }
                }
            }
        }
        catch { }
        return getFee;
    }
    protected void btnReuseYes_Click(object sender, EventArgs e)
    {
        divReuseRoll.Visible = false;
        if (cbdisWithoutFees.Checked)
            getDiscontinue(true);
        else
            discontinueMethod(true);
    }
    protected void btnReuseNo_Click(object sender, EventArgs e)//saranya(31.10.2017)
    {
        divReuseRoll.Visible = false;
        //discontinueMethod(false);
        divReuseRoll.Visible = false;
        if (cbdisWithoutFees.Checked)
            getDiscontinue(false);
        else
            discontinueMethod(false);
    }
    protected void btcancel_Click(object sender, EventArgs e)//saranya(31.10.2017)
    {
        divReuseRoll.Visible = false;

    }
    private void discontinueMethod(bool reuseRollNo)
    {
        try
        {
            int check = 0;
            string appno = "";
            double refuntaken = 0;
            bool Amt = false;
            string dis_Date = Convert.ToString(txt_rdate.Text);
            string[] frdate = dis_Date.Split('/');
            if (frdate.Length == 3)
                dis_Date = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            foreach (GridViewRow grid in gridView3.Rows)
            {
                TextBox refuntakens = (TextBox)grid.FindControl("txt_refund");
                double.TryParse(Convert.ToString(refuntakens.Text), out refuntaken);
                if (refuntaken != 0)
                    Amt = true;
            }
            string rollno = txt_rerollno.Text.Trim();
            if (Amt == true)
            {
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                    {
                        appno = d2.GetFunction("select app_no from Registration where roll_no='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'");//and cc<>'1' and DelFlag<>1
                        check = 0;
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                    {
                        appno = d2.GetFunction("select app_no from Registration where reg_no='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'");//and cc<>'1' and DelFlag<>1
                        check = 0;
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                    {
                        appno = d2.GetFunction("select app_no from Registration where Roll_admit='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'");//and cc<>'1' and DelFlag<>1
                        check = 0;
                    }
                }
                //  if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
                else
                {
                    appno = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "' and Admission_Status=1 and selection_status=1 and IsConfirm=1 and college_code='" + ddlcollege.SelectedValue + "'");
                    check = 1;
                    // appno = "  and a.app_formno  = '" + rollno + "' ";
                }
                string finYearid = d2.getCurrentFinanceYear(usercode, ddlcollege.SelectedValue);
                string[] dtsplit = txt_rdate.Text.Split('/');
                DateTime dtdate = Convert.ToDateTime(dtsplit[1] + "/" + dtsplit[0] + "/" + dtsplit[2]);
                if (appno != "" && appno != "0")
                {
                    //update registration                   
                    //if (value == 1)
                    //{
                    string reason = string.Empty;
                    reason = reasondis.Text;//added by abarna 22.02.2018

                    string critcode = d2.GetFunction("select criteria_Code  from selectcriteria where app_no='" + appno + "'");
                    string degreecode = d2.GetFunction("select degree_Code  from applyn where app_no='" + appno + "'");
                    //string upRegQ = " update applyn set Admission_Status=0,selection_status=0 where app_no='" + appno + "'";
                    //d2.update_method_wo_parameter(upRegQ, "Text");
                    string upq = " update registration set  DelFlag=1 where app_no='" + appno + "' ";
                    upq = upq + " update applyn set Admission_Status='2',AdmitedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no='" + appno + "' ";
                    upq = upq + " update Room_Detail set Avl_Student= Avl_Student - 1 where Roompk in(select RoomFk from HT_HostelRegistration where app_no='" + appno + "' and  isnull(IsDiscontinued,'0')='0' and isnull(isvacated,'0')='0')"; //magesh 13.10.18
                    upq = upq + "if exists ( select app_no from HT_HostelRegistration where app_no='" + appno + "') update ht_hostelregistration set IsDiscontinued='1' where app_no='" + appno + "'";//added by abarna 22.02.2018
                    string qry = d2.GetFunction("select app_no from HT_HostelRegistration where app_no='" + appno + "'");
                    if (qry != "")
                    {
                        string paid = "select paidamount from ft_feeallot where app_no='" + appno + "'";
                        DataSet ds = new DataSet();
                        ds = d2.select_method_wo_parameter(paid, "text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    double amt = Convert.ToDouble(ds.Tables[0].Rows[i]["paidamount"]);
                                    if (amt == 0.00)
                                    {
                                        qry = "delete from ft_Feeallot where app_no='" + appno + "' and paidamount='0'";
                                        d2.update_method_wo_parameter(qry, "Text");
                                    }
                                }
                            }
                        }

                    }

                    if (reuseRollNo)
                    {
                        //upq = upq + " update registration set roll_no=roll_admit where app_no='" + appno + "' ";
                        //barath 09.12.17
                        DataSet getRollandAdmitNoDS = new DataSet();
                        getRollandAdmitNoDS = d2.select_method_wo_parameter("select Roll_No,Roll_Admit from registration where App_No='" + appno + "'", "text");
                        if (getRollandAdmitNoDS.Tables[0].Rows.Count > 0)
                        {
                            string Oldrollno = Convert.ToString(getRollandAdmitNoDS.Tables[0].Rows[0]["Roll_No"]);
                            string Newrollno = Convert.ToString(getRollandAdmitNoDS.Tables[0].Rows[0]["Roll_Admit"]);
                            string ACR = string.Empty;
                            if (rb_discont.Checked)
                                ACR = "DIS";
                            if (rb_ProlongAbsent.Checked)
                                ACR = "PROL";
                            Hashtable hat = new Hashtable();
                            hat.Add("@OldRollNo", Oldrollno);
                            hat.Add("@NewRollNo", Oldrollno);
                            d2.update_method_with_parameter("StudentRollNoUpdate", hat, "sp");
                        }
                    }
                    d2.update_method_wo_parameter(upq, "Text");

                    //insert into discontinue table
                    string insQ = "insert into discontinue(app_no,discontinue_date,coll_transfer,letter_date,Reason) values('" + appno + "','" + dis_Date + "','0','" + dis_Date + "','" + reason + "')";//reason added by abarna 22.02.2018//dis_Date modified by saranya on 17/04/2018
                    d2.update_method_wo_parameter(insQ, "Text");
                    // Criteria code update
                    string CrUpd = "update selectcriteria set admit_confirm='0' where app_no='" + appno + "'";
                    int crup = d2.update_method_wo_parameter(CrUpd, "Text");
                    //admitcolumn update
                    string Adupd = "update admitcolumnset set allot_Confirm =allot_Confirm -1 where setcolumn ='" + degreecode + "' and column_name ='" + critcode + "'";
                    int admit = d2.update_method_wo_parameter(Adupd, "Text");
                    //}
                    if (txt_reamt.Text.Trim() == "")
                        txt_reamt.Text = "0";
                    string upExcess = " if exists (select * from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2' ) update FT_ExcessDet set ExcessAmt =ExcessAmt +'" + txt_reamt.Text + "',BalanceAmt =BalanceAmt +'" + txt_reamt.Text + "' where App_No ='" + appno + "' and ExcessType ='2'  else  INSERT INTO FT_ExcessDet (ExcessTransDate,TransTime ,DailyTransCode,App_No ,MemType ,ExcessType ,ExcessAmt,AdjAmt,BalanceAmt,FinYearFK ) VALUES ('" + dtdate + "','" + DateTime.Now.ToLongTimeString() + "' ,'', " + appno + " , 1 , 2 ," + txt_reamt.Text + ",0," + txt_reamt.Text + "," + finYearid + ")";
                    d2.update_method_wo_parameter(upExcess, "Text");

                    string getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='2'");
                    foreach (GridViewRow rows in gridView3.Rows)
                    {
                        Label hdrid = (Label)rows.Cells[10].FindControl("lbl_hdrid");
                        Label lgrid = (Label)rows.Cells[10].FindControl("lbl_lgrid");
                        Label feecatid = (Label)rows.Cells[10].FindControl("lbl_feecat");
                        Label paidamt = (Label)rows.Cells[10].FindControl("lbl_paid");
                        TextBox txtAmt = (TextBox)rows.Cells[10].FindControl("txt_refund");
                        TextBox txtrefundAmt = (TextBox)rows.Cells[10].FindControl("txt_refundbal");

                        if (txtrefundAmt.Text != "" && txtrefundAmt.Text != "0")
                        {
                            string upRefundQ = " update FT_FeeAllot set RefundAmount= ISNull(RefundAmount,0)+ " + txtrefundAmt.Text + ", IsRefund='1' where app_no=" + appno + " and HeaderFK=" + hdrid.Text + " and Ledgerfk=" + lgrid.Text + " and FeeCategory=" + feecatid.Text + " and IsTransfer='0'";
                            d2.update_method_wo_parameter(upRefundQ, "Text");

                            upExcess = "if exists ( select * from FT_ExcessLedgerDet where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "')update FT_ExcessLedgerDet set ExcessAmt  =ExcessAmt +'" + txtrefundAmt.Text + "',BalanceAmt =BalanceAmt +'" + txtrefundAmt.Text + "' where ExcessDetFK ='" + getvalue + "' and HeaderFK ='" + hdrid.Text + "' and LedgerFK ='" + lgrid.Text + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK) values ('" + hdrid.Text + "','" + lgrid.Text + "','" + txtrefundAmt.Text + "','" + txtrefundAmt.Text + "','" + getvalue + "')";
                            d2.update_method_wo_parameter(upExcess, "Text");
                        }
                    }
                    disTransClear();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Discontinued Sucessfully')", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Already Discontinued')", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Enter The Amount')", true);

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "TransferRefundSettings");
        }
    }
    protected void getDiscontinue(bool reuseRollNo)
    {
        try
        {
            bool boolCheck = false;
            string appno = string.Empty;
            string rollno = Convert.ToString(txt_rerollno.Text);
            string dis_Date = Convert.ToString(txt_rdate.Text);
            string[] frdate = dis_Date.Split('/');
            if (frdate.Length == 3)
                dis_Date = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();

            if (!string.IsNullOrEmpty(rollno))
            {
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                    {
                        appno = d2.GetFunction("select app_no from Registration where roll_no='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'"); //and cc<>'1' and DelFlag<>1
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                    {
                        appno = d2.GetFunction("select app_no from Registration where reg_no='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'");//and cc<>'1' and DelFlag<>1
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                    {
                        appno = d2.GetFunction("select app_no from Registration where Roll_admit='" + rollno + "'  and college_code='" + ddlcollege.SelectedValue + "'");//and cc<>'1' and DelFlag<>1
                    }
                }
                //  if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
                else
                {
                    appno = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "' and Admission_Status=1 and selection_status=1 and IsConfirm=1 and college_code='" + ddlcollege.SelectedValue + "'");
                }
                if (!string.IsNullOrEmpty(appno) && appno != "0")
                {
                    string reason = string.Empty;
                    string Catogery = string.Empty;
                    string critcode = d2.GetFunction("select criteria_Code  from selectcriteria where app_no='" + appno + "'");
                    string degreecode = d2.GetFunction("select degree_Code  from applyn where app_no='" + appno + "'");
                    reason = reasondis.Text;
                    string upq = "";
                    if (rb_discont.Checked == true)
                    {
                        Catogery = "0";
                        upq = " update registration set  DelFlag=1 where app_no='" + appno + "' ";
                        upq = upq + " update applyn set Admission_Status='2',AdmitedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no='" + appno + "' ";
                        upq = upq + " update Room_Detail set Avl_Student= Avl_Student - 1 where Roompk in(select RoomFk from HT_HostelRegistration where app_no='" + appno + "' and  isnull(IsDiscontinued,'0')='0' and isnull(isvacated,'0')='0')"; //magesh 13.10.18
                        upq = upq + "if exists ( select app_no from HT_HostelRegistration where app_no='" + appno + "') update ht_hostelregistration set IsDiscontinued='1' where app_no='" + appno + "'";//added by abarna 22.02.2018
                        //upq = upq + " update registration set roll_no=roll_admit where app_no='" + appno + "' ";//barath 09.12.17
                    }
                    else if (rb_ProlongAbsent.Checked)
                    {
                        Catogery = "1";
                        string[] dtsplit = txt_rdate.Text.Split('/');
                        DateTime dtdate = Convert.ToDateTime(dtsplit[1] + "/" + dtsplit[0] + "/" + dtsplit[2]);
                        upq = " update registration set ProLongAbsent=1,delflag=1 where app_no='" + appno + "' ";
                        upq = upq + " update applyn set Admission_Status='2',AdmitedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no='" + appno + "' ";
                        //upq = upq + " update registration set roll_no=roll_admit where app_no='" + appno + "' ";//barath 09.12.17
                        //Rajkumar
                        upq = upq + "update registration set ProlongDate='" + dtdate + "' where app_no='" + appno + "' ";
                        //
                    }
                    d2.update_method_wo_parameter(upq, "Text");
                    //barath 09.12.17
                    if (reuseRollNo)
                    {
                        if (rb_discont.Checked == true || rb_ProlongAbsent.Checked)
                        {
                            DataSet getRollandAdmitNoDS = new DataSet();
                            getRollandAdmitNoDS = d2.select_method_wo_parameter("select Roll_No,Roll_Admit from registration where App_No='" + appno + "'", "text");
                            if (getRollandAdmitNoDS.Tables[0].Rows.Count > 0)
                            {
                                string Oldrollno = Convert.ToString(getRollandAdmitNoDS.Tables[0].Rows[0]["Roll_No"]);
                                string Newrollno = Convert.ToString(getRollandAdmitNoDS.Tables[0].Rows[0]["Roll_Admit"]);
                                string ACR = string.Empty;
                                if (rb_discont.Checked)
                                    ACR = "DIS";
                                if (rb_ProlongAbsent.Checked)
                                    ACR = "PROL";
                                Hashtable hat = new Hashtable();
                                hat.Add("@OldRollNo", Oldrollno);
                                hat.Add("@NewRollNo", Oldrollno);
                                d2.update_method_with_parameter("StudentRollNoUpdate", hat, "sp");
                            }
                        }
                    }

                    // Criteria code update
                    string CrUpd = "update selectcriteria set admit_confirm='0' where app_no='" + appno + "'";
                    int crup = d2.update_method_wo_parameter(CrUpd, "Text");

                    //insert into discontinue table
                    string insQ = "insert into discontinue(app_no,discontinue_date,coll_transfer,letter_date,Catogery,Reason) values('" + appno + "','" + dis_Date + "','0','" + dis_Date + "','" + Catogery + "','" + reason + "')";//reason added by abarna 22.02.2018//dis_Date modified by saranya on 17/04/2018
                    d2.update_method_wo_parameter(insQ, "Text");
                    boolCheck = true;
                    //admitcolumn update
                    string Adupd = "update admitcolumnset set allot_Confirm =allot_Confirm -1 where setcolumn ='" + degreecode + "' and column_name ='" + critcode + "'";
                    int admit = d2.update_method_wo_parameter(Adupd, "Text");


                }
                if (boolCheck)
                {
                    disTransClear();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Discontinued Sucessfully')", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Discontinued Failed')", true);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "TransferRefundSettings"); }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        getDiscontinue(true);

    }
    protected void btnHistory_Click(object sender, EventArgs e)
    {

        //div_History.Visible = true;
        //headerbind1();
        //ledgerbind1();

        //gridHist.DataSource = null;
        //gridHist.DataBind();
        //// btnhisgo.Visible = false;
        //btnhisgo_Click(sender, e);
        //imgAlert.Visible = false;
    }
    protected void ddl_refheader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // bindLedgerRe();
    }

    protected void disTransClear()
    {
        txt_rerollno.Text = "";
        txt_rebatch.Text = "";
        txt_redegree.Text = "";
        txt_redept.Text = "";
        txt_resec.Text = "";
        txt_resem.Text = "";
        txt_recolg.Text = "";
        txt_restrm.Text = "";
        txt_rename.Text = "";
        txt_AmtPerc.Text = "";
        txt_reamt.Text = "";
        routetxt.Text = "";
        vehicletxt.Text = "";
        stagetxt.Text = "";
        txt_hostel.Text = "";
        txt_build.Text = "";
        txt_roomname.Text = "";
        image3.ImageUrl = "";
        txtroll_staff.Text = "";
        txtname_staff.Text = "";
        txtDept_staff.Text = "";
        tblgrid3.Visible = false;
        tbljournal.Visible = false;
        gridView3.DataSource = null;
        gridView3.DataBind();
        txtRefund_staffid.Text = "";
        txtRefund_staffName.Text = "";
        txtRefund_staffDept.Text = "";

    }

    protected void transFromClear()
    {
        txt_roll.Text = "";
        txt_batch.Text = "";
        txt_degree.Text = "";
        txt_dept.Text = "";
        txt_sec.Text = "";
        txt_seattype.Text = "";
        txt_sem.Text = "";
        txt_colg.Text = "";
        txt_strm.Text = "";
        txt_name.Text = "";
        image3.ImageUrl = "";
        image2.ImageUrl = "";
        gridView3.DataSource = null;
        gridView3.DataBind();
        //
        lbltempfstclg.Text = string.Empty;
        lbltempfstdeg.Text = string.Empty;
        //  Div3.Visible = false;
        lnkindivmap.Enabled = false;
        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_date.Attributes.Add("readonly", "readonly");
    }
    protected void transToClear()
    {
        txt_roll1.Text = "";
        txt_batch1.Text = "";
        txt_degree1.Text = "";
        txt_dept1.Text = "";
        txt_sec1.Text = "";
        txt_seat_type1.Text = "";
        txt_sem1.Text = "";
        txt_colg1.Text = "";
        txt_strm1.Text = "";
        lblDegCode.Text = "";
        txt_roll_no.Text = "";
        txt_roll_no1.Text = "";
        //
        lbltempsndclg.Text = string.Empty;
        lbltempsnddeg.Text = string.Empty;
        // Div3.Visible = false;
        lnkindivmap.Enabled = false;
    }

    protected bool mapplingVisibile()
    {
        bool boolCheck = false;
        if (txt_roll.Text.Trim() != "")
        {
            if (rbl_AdmitTransfer.SelectedIndex == 0)
            {
                if (txt_roll1.Text.Trim() != "")
                    boolCheck = true;
                else
                    boolCheck = false;
            }
            else
                boolCheck = true;

        }
        if (boolCheck)
            lnkindivmap.Enabled = true;
        else
            lnkindivmap.Enabled = false;
        return boolCheck;
    }


    //transfer mapping     
    protected void lnkindivmap_Click(object sender, EventArgs e)
    {
        divindi.Visible = true;
        divind.Visible = true;
        incPaid.Checked = false;
        incSem.Checked = false;
        incSem_Changed(sender, e);
        bindGridInd();
        bindGrid5Ind();
        txtamtind.Text = "";
        // btntransind.Enabled = false;
        inclAddAmt.Checked = false;
        inclAddAmt_Changed(sender, e);
        getJournalSettings();
        btntransind.Text = "Transfer";
        tblSem.Visible = true;
        btnadjust.Visible = false;
        tblJournalSet.Visible = true;
        lbladvancetxt.Text = "Excess/Advance :";
        staffadd.Visible = false;
        div4.Visible = false;
        savebutton.Visible = false;
        btnupdatestaff.Visible = false;
    }

    public void bindHeaderind()
    {
        try
        {
            string colgCode = string.Empty;
            if (rb_transfer.Checked)
            {
                if (rbl_AdmitTransfer.SelectedIndex == 0)
                    colgCode = Convert.ToString(ddlclgapplied.SelectedItem.Value);
                else
                    colgCode = Convert.ToString(ddl_colg.SelectedItem.Value);
            }
            else
                colgCode = Convert.ToString(ddlcollege.SelectedItem.Value);

            ddlhedind.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster L,FS_HeaderPrivilage P WHERE L.HeaderPK = P.HeaderFK   AND P.CollegeCode = L.CollegeCode  AND P. UserCode = " + usercode + " AND L.CollegeCode = " + colgCode + " ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlhedind.DataSource = ds;
                    ddlhedind.DataTextField = "HeaderName";
                    ddlhedind.DataValueField = "HeaderPK";
                    ddlhedind.DataBind();

                }
            }
        }
        catch (Exception ex) { }
    }

    public void bindLedgerind()
    {
        try
        {
            string colgCode = string.Empty;
            if (rb_transfer.Checked)
            {
                if (rbl_AdmitTransfer.SelectedIndex == 0)
                    colgCode = Convert.ToString(ddlclgapplied.SelectedItem.Value);
                else
                    colgCode = Convert.ToString(ddl_colg.SelectedItem.Value);
            }
            else
                colgCode = Convert.ToString(ddlcollege.SelectedItem.Value);
            ddlledind.Items.Clear();
            string headerfk = "-1";
            if (ddlhedind.Items.Count > 0)
            {
                headerfk = Convert.ToString(ddlhedind.SelectedItem.Value);
            }
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode and l.HeaderFK=" + headerfk + " AND P. UserCode = " + usercode + " AND L.CollegeCode = " + colgCode + " order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlledind.DataSource = ds;
                    ddlledind.DataTextField = "LedgerName";
                    ddlledind.DataValueField = "LedgerPK";
                    ddlledind.DataBind();

                }
            }
        }
        catch (Exception ex) { }
    }
    public void bindGridInd()
    {
        string app_no = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("YearSem");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("FeeCategory");
        dt.Columns.Add("Concession");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("Total");
        dt.Columns.Add("FeeAmt");
        dt.Columns.Add("paymode");
        dt.Columns.Add("narration");

        DataRow dr;
        double total = 0;
        double balance = 0;
        double paid = 0;
        string paidVal = string.Empty;
        if (incPaid.Checked)
            paidVal = " and isnull(f.paidamount,'0')<>0";
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
        {
            app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
        {
            app_no = d2.GetFunction("select app_no from Registration where Reg_no='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
        {
            app_no = d2.GetFunction("select app_no from Registration where Roll_admit='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }
        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
        {
            app_no = d2.GetFunction("select app_no from applyn where app_formno='" + txt_roll.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }

        if (app_no != "")
        {
            string selectQ = "";
            if (ddladmis.SelectedItem.Text.Trim() != "Before Admission")
            {
                selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,f.paymode   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(IsTransfer,'0')='0'  and r.App_No=" + app_no + " " + paidVal + " and isnull(IsTransfer,'0')='0' order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";
                // order by F.FeeCategory,f.HeaderFK,f.LedgerFK
            }
            else
            {
                selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,f.paymode   from FT_FeeAllot f,applyn R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(IsTransfer,'0')='0'  and r.App_No=" + app_no + " " + paidVal + " and isnull(IsTransfer,'0')='0' order by isnull(l.priority,1000), l.ledgerName asc,F.FeeCategory";
                //order by F.FeeCategory,f.HeaderFK,f.LedgerFK
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQ, "Text");
            if (ds.Tables.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                    string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + ddlcollege.SelectedValue + "");
                    dr = dt.NewRow();
                    dr["Sno"] = row + 1;
                    dr["YearSem"] = cursem;
                    dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                    dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                    dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                    dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                    dr["FeeCategory"] = feecat;
                    dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                    dr["Paid"] = Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]);
                    dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                    dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                    dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                    dr["paymode"] = Convert.ToString(ds.Tables[0].Rows[row]["paymode"]);
                    //dr["narration"] = Convert.ToString(ds.Tables[0].Rows[row]["narration"]);
                    dt.Rows.Add(dr);

                    total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);
                    balance += Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]);
                    paid += Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]);//modified by abarna

                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            gridView4.DataSource = dt;
            gridView4.DataBind();
            Label4.Text = "Rs." + balance.ToString();
            Label3.Text = "Rs." + paid.ToString();
            Label2.Text = "Rs." + total.ToString();
            Table1.Visible = true;
        }
        else
        {
            gridView4.DataSource = null;
            gridView4.DataBind();
            Table1.Visible = false;
        }
    }
    protected void ddlhedind_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindLedgerind();
    }

    public void bindGrid5Ind()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("YearSem");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("FeeCategory");
        dt.Columns.Add("Concession");
        dt.Columns.Add("Paid");
        dt.Columns.Add("hiddenTempAmt");
        dt.Columns.Add("tobePaid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("Total");
        dt.Columns.Add("FeeAmt");

        DataRow dr;
        double total = 0;
        double balance = 0;
        double paid = 0;
        string clgcode = "";

        string selectQ = "";
        string stream = "";
        string batch = "";
        string degreeCode = "";
        string dept = "";
        string feecategory = "";
        string section = "";
        string seatype = "";
        string IncfeeCat = string.Empty;
        if (incSem.Checked)
            IncfeeCat = " and f.FeeCategory in('" + Convert.ToString(getCblSelectedValue(cbl_sem)) + "')";
        if (rb_transfer.Checked)
        {
            if (rbl_AdmitTransfer.SelectedIndex == 0)
            {
                if (txt_roll1.Text.Trim() != "")
                {
                    stream = txt_strm1.Text.Trim();
                    batch = txt_batch1.Text.Trim();
                    degreeCode = lblDegCode.Text;
                    dept = "";
                    feecategory = "";
                    section = "";
                    clgcode = Convert.ToString(ddlclgapplied.SelectedItem.Value);
                    string fstSeatCode = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + ddlclgapplied.SelectedValue + "'  and textval='" + txt_seat_type1.Text.Trim() + "'"));

                    selectQ = "  select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(TotalAmount,'0') as balamount  from FT_FeeAllotDegree f,FM_HeaderMaster H,FM_LedgerMaster L where  F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and  L.CollegeCode=H.CollegeCode and L.CollegeCode=" + ddlclgapplied.SelectedValue + " and F.BatchYear=" + batch + " and isnull(f.TotalAmount,'0')<>'0' and isnull(ishostelfees,'0')='0' and F.DegreeCode=" + degreeCode + " " + IncfeeCat + " ";
                    if (fstSeatCode != "0")
                        selectQ += "   and seattype='" + fstSeatCode + "' ";
                    if (stream != "")
                    {
                        selectQ += " ";
                    }
                    selectQ += " order by F.FeeCategory, isnull(l.priority,1000), l.ledgerName asc";
                }
            }
            else
            {
                if (ddl_colg.Items.Count > 0)
                {
                    // collegecode1 = Convert.ToString(ddl_colg.SelectedItem.Value);
                    clgcode = Convert.ToString(ddl_colg.SelectedItem.Value);
                    if (ddl_strm.Items.Count > 0)
                        stream = Convert.ToString(ddl_strm.SelectedItem.Value);
                    if (ddl_batch.Items.Count > 0)
                        batch = Convert.ToString(ddl_batch.SelectedItem.Value);
                    if (ddl_degree.Items.Count > 0)
                        degreeCode = Convert.ToString(ddl_degree.SelectedItem.Value);
                    if (ddl_dept.Items.Count > 0)
                        dept = Convert.ToString(ddl_dept.SelectedItem.Value);
                    if (ddl_sem.Items.Count > 0)
                        feecategory = Convert.ToString(ddl_sem.SelectedItem.Value);
                    if (ddl_sec.Items.Count > 0)
                        section = Convert.ToString(ddl_sec.SelectedItem.Value);
                    if (ddl_seattype.Items.Count > 0)
                        seatype = Convert.ToString(ddl_seattype.SelectedValue);


                    selectQ = "  select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,isnull(F.TotalAmount,0) as balamount   from FT_FeeAllotDegree f,FM_HeaderMaster H,FM_LedgerMaster L where  F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and  L.CollegeCode=H.CollegeCode and L.CollegeCode=" + ddl_colg.SelectedItem.Value + " and F.BatchYear=" + batch + " and F.DegreeCode=" + dept + " and isnull(f.TotalAmount,'0')<>'0' and isnull(ishostelfees,'0')='0' and seattype='" + seatype + "' " + IncfeeCat + "";
                    selectQ += " order by F.FeeCategory,  isnull(l.priority,1000), l.ledgerName asc";
                }
            }
        }
        if (selectQ != "")
        {
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQ, "Text");
            if (ds.Tables.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    if (feecategory == "3")
                    {
                        string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                        string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + clgcode + "");
                        if (cursem.Contains("1 SEMESTER") || cursem.Contains("2 SEMESTER"))
                        {
                        }
                        else
                        {
                            dr = dt.NewRow();
                            dr["Sno"] = row + 1;
                            dr["YearSem"] = cursem;
                            dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                            dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                            dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                            dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                            dr["FeeCategory"] = feecat;
                            dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                            dr["Paid"] = "0";
                            dr["hiddenTempAmt"] = "0";
                            dr["tobePaid"] = "0";
                            dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                            dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                            dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                            dt.Rows.Add(dr);

                            total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);
                        }
                    }
                    else
                    {
                        string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                        string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + clgcode + "");
                        dr = dt.NewRow();
                        dr["Sno"] = row + 1;
                        dr["YearSem"] = cursem;
                        dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                        dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                        dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                        dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                        dr["FeeCategory"] = feecat;
                        dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                        dr["Paid"] = "0";
                        dr["hiddenTempAmt"] = "0";
                        dr["tobePaid"] = "0";
                        dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                        dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                        dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                        dt.Rows.Add(dr);

                        total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);
                    }
                    //balance += Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]);
                    // paid += Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]);
                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            gridView5.DataSource = dt;
            gridView5.DataBind();
            Label8.Text = "Rs." + balance.ToString();
            Label6.Text = "Rs." + paid.ToString();
            Label5.Text = "Rs." + total.ToString();
            Label9.Text = "Rs.";
            // Label10.Text = "RS.";
            Table2.Visible = true;
        }
        else
        {
            gridView5.DataSource = null;
            gridView5.DataBind();
            Table2.Visible = false;
        }


    }

    protected void gridView4_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Text = lblsem.Text;
            e.Row.Cells[7].Visible = true;
            e.Row.Cells[8].Visible = true;
            if (rb_Journal.Checked)
            {
                e.Row.Cells[5].Text = "Receipt No";
                e.Row.Cells[6].Text = "Receipt Date";
                if (ddlJournalType.SelectedIndex == 0)
                {
                    e.Row.Cells[7].Text = "Total Amount";
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                }
                else if (ddlJournalType.SelectedIndex == 1)
                {
                    e.Row.Cells[7].Text = "Total Amount";
                    e.Row.Cells[7].Visible = true;
                    e.Row.Cells[8].Visible = true;
                }
                else
                {
                    e.Row.Cells[7].Text = "Total Amount";
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[7].Visible = true;
            e.Row.Cells[8].Visible = true;
            if (rb_Journal.Checked)
            {
                //e.Row.Cells[7].Visible = false;
                //e.Row.Cells[8].Visible = false;
                if (ddlJournalType.SelectedIndex == 0)
                {

                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                }
                else if (ddlJournalType.SelectedIndex == 1)
                {

                    e.Row.Cells[7].Visible = true;
                    e.Row.Cells[8].Visible = true;
                }
                else
                {

                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                }
            }
        }
    }
    protected void gridView5_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Text = lblsem.Text;
            e.Row.Cells[1].Visible = true;
            // e.Row.Cells[9].Visible = false;
            e.Row.Cells[9].Visible = true;
            e.Row.Cells[11].Visible = true;
            if (rb_Journal.Checked)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[9].Visible = true;
                // e.Row.Cells[11].Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Visible = true;
            // e.Row.Cells[9].Visible = false;
            e.Row.Cells[9].Visible = true;
            e.Row.Cells[11].Visible = true;
            if (rb_Journal.Checked)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[9].Visible = true;
                // e.Row.Cells[11].Visible = false;
            }
            TextBox txtPaid = (TextBox)e.Row.FindControl("txt_paid");
            txtPaid.Attributes.Add("readonly", "readonly");
        }
    }

    //paid amount mapping
    protected void btnadjust_Click(object sender, EventArgs e)
    {
        if (gridFirstCheck())
        {
            if (gridSecondCheck())
            {
                if (!rb_Journal.Checked)
                {
                    AdjustLedgerDetails();
                }
                else
                {
                    AdjustLedgerJournalDetails();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Select Any One Ledger')", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Select Any One Ledger')", true);
        }

    }
    protected void btnmapreset_Click(object sender, EventArgs e)
    {
        Label8.Text = "";
        Label6.Text = "";
        Label5.Text = "";
        Label3.Text = "";
        Label2.Text = "";
        Label1.Text = "";
        incPaid.Checked = false;
        incSem.Checked = false;
        cbl_sem.Items.Clear();
        bindHeaderind();
        bindLedgerind();
        if (!rb_Journal.Checked)
        {
            bindGridInd();
            bindGrid5Ind();
        }
        else
        {
            bindGridJournalAdvance();
            bindGridAllotJournal();
        }
        txtamtind.Text = "";
        //   btntransind.Enabled = false;

    }
    //gridrow checkbox checked
    protected bool gridFirstCheck()
    {
        bool checkval = false;
        foreach (GridViewRow gdview in gridView4.Rows)
        {
            CheckBox cb = (CheckBox)gdview.FindControl("cbsel");
            if (cb.Checked)
                checkval = true;
        }
        return checkval;
    }
    protected bool gridSecondCheck()
    {
        bool checkval = false;
        foreach (GridViewRow gdview in gridView5.Rows)
        {
            CheckBox cb = (CheckBox)gdview.FindControl("cblsell");
            if (cb.Checked)
                checkval = true;
        }
        return checkval;
    }
    protected void AdjustLedgerDetails()
    {
        try
        {
            bool save = false;
            double totAmt = 0;
            double paidAmt = 0;
            double paidOvall = 0;
            double FnlPaidamt = 0;
            double Fnltotalamt = 0;
            double temptotamt = 0;
            double excessAmt = 0;
            string disAmt = "";
            foreach (GridViewRow grow in gridView4.Rows)
            {
                CheckBox cb1 = (CheckBox)grow.FindControl("cbsel");
                Label totamount = (Label)grow.FindControl("lbl_totamt");
                if (cb1.Checked)
                {
                    Label paid1 = (Label)grow.FindControl("lbl_paid");
                    double.TryParse(Convert.ToString(paid1.Text), out  paidAmt);
                    FnlPaidamt += paidAmt;
                }
                double.TryParse(Convert.ToString(totamount.Text), out  temptotamt);
                Fnltotalamt += temptotamt;
            }
            if (FnlPaidamt != 0)
            {
                disAmt = Convert.ToString(FnlPaidamt);
                int index = -1;
                int indextwo = 0;
                foreach (GridViewRow gdsndrow in gridView5.Rows)
                {
                    CheckBox cb2 = (CheckBox)gdsndrow.FindControl("cblsell");
                    if (cb2.Checked)
                    {
                        if (index == -1)
                            index = indextwo;

                        Label hdrid = (Label)gdsndrow.FindControl("lbl_hdrid");
                        Label lgrid = (Label)gdsndrow.FindControl("lbl_lgrid");
                        Label feecat = (Label)gdsndrow.FindControl("lbl_feecat");
                        Label feeamt = (Label)gdsndrow.FindControl("lbl_feeamt");
                        Label totamt = (Label)gdsndrow.FindControl("lbl_totamt");
                        Label concession = (Label)gdsndrow.FindControl("lbl_Concess");
                        TextBox txtpaid = (TextBox)gdsndrow.FindControl("txt_paid");
                        TextBox txtbalance = (TextBox)gdsndrow.FindControl("txt_bal");
                        TextBox excess = (TextBox)gdsndrow.FindControl("txt_exGrid2");
                        excess.Text = "";
                        double.TryParse(Convert.ToString(totamt.Text), out  totAmt);
                        if (totAmt >= FnlPaidamt)
                        {
                            txtpaid.Text = Convert.ToString(FnlPaidamt);
                            paidOvall += FnlPaidamt;
                            FnlPaidamt = 0;
                        }
                        else
                        {
                            txtpaid.Text = Convert.ToString(totAmt);
                            FnlPaidamt = FnlPaidamt - totAmt;
                            paidOvall += totAmt;
                        }
                        txtbalance.Text = (totAmt - Convert.ToDouble(txtpaid.Text)).ToString();
                        save = true;
                    }
                    indextwo++;
                }
                // FnlPaidamt = 4500;
                excessAmt += FnlPaidamt;
                if (excessAmt > 0)
                {
                    TextBox excess = (TextBox)gridView5.Rows[index].FindControl("txt_exGrid2");
                    excess.Text = Convert.ToString(excessAmt);
                }
                Label6.Text = "Rs." + Convert.ToString(paidOvall);
                string totPaid = Label5.Text;
                string paid = totPaid.Split('.')[1];
                double fnltot = 0;
                double.TryParse(Convert.ToString(paid), out fnltot);
                Label8.Text = "Rs." + Convert.ToString(fnltot - paidOvall);
                Label24ex.Text = "Rs." + Convert.ToString(excessAmt);
                Label9.Text = "Rs." + Convert.ToString(excessAmt);
                btntransind.Enabled = true;
                if (save == true)
                {
                    Label12.Text = disAmt;
                    div7.Visible = true;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Do Not Have Paid Amount')", true);
            }
        }
        catch { }
    }

    protected void AdjustLedgerJournalDetails()
    {
        try
        {
            bool save = false;
            double totAmt = 0;
            double paidAmt = 0;
            double paidOvall = 0;
            double FnlPaidamt = 0;
            double Fnltotalamt = 0;
            double temptotamt = 0;
            double excessAmt = 0;
            string disAmt = "";
            foreach (GridViewRow grow in gridView4.Rows)
            {
                CheckBox cb1 = (CheckBox)grow.FindControl("cbsel");
                Label totamount = (Label)grow.FindControl("lbl_totamt");
                if (cb1.Checked)
                {
                    Label paid1 = (Label)grow.FindControl("lbl_bal");
                    double.TryParse(Convert.ToString(paid1.Text), out  paidAmt);
                    FnlPaidamt += paidAmt;
                }
                double.TryParse(Convert.ToString(totamount.Text), out  temptotamt);
                Fnltotalamt += temptotamt;
            }
            if (FnlPaidamt != 0)
            {
                disAmt = Convert.ToString(FnlPaidamt);
                int index = -1;
                int indextwo = 0;
                foreach (GridViewRow gdsndrow in gridView5.Rows)
                {
                    CheckBox cb2 = (CheckBox)gdsndrow.FindControl("cblsell");
                    if (cb2.Checked)
                    {
                        if (index == -1)
                            index = indextwo;

                        Label hdrid = (Label)gdsndrow.FindControl("lbl_hdrid");
                        Label lgrid = (Label)gdsndrow.FindControl("lbl_lgrid");
                        Label feecat = (Label)gdsndrow.FindControl("lbl_feecat");
                        Label feeamt = (Label)gdsndrow.FindControl("lbl_feeamt");
                        Label totamt = (Label)gdsndrow.FindControl("lbl_totamt");
                        Label concession = (Label)gdsndrow.FindControl("lbl_Concess");
                        TextBox txtpaid = (TextBox)gdsndrow.FindControl("txt_paid");
                        TextBox txtbalance = (TextBox)gdsndrow.FindControl("txt_bal");
                        TextBox excess = (TextBox)gdsndrow.FindControl("txt_exGrid2");
                        excess.Text = "";
                        double.TryParse(Convert.ToString(totamt.Text), out  totAmt);
                        if (totAmt >= FnlPaidamt)
                        {
                            txtpaid.Text = Convert.ToString(FnlPaidamt);
                            paidOvall += FnlPaidamt;
                            FnlPaidamt = 0;
                        }
                        else
                        {
                            txtpaid.Text = Convert.ToString(totAmt);
                            FnlPaidamt = FnlPaidamt - totAmt;
                            paidOvall += totAmt;
                        }
                        txtbalance.Text = (totAmt - Convert.ToDouble(txtpaid.Text)).ToString();
                        save = true;
                    }
                    indextwo++;
                }
                // FnlPaidamt = 4500;
                excessAmt += FnlPaidamt;
                if (excessAmt > 0)
                {
                    TextBox excess = (TextBox)gridView5.Rows[index].FindControl("txt_exGrid2");
                    excess.Text = Convert.ToString(excessAmt);
                }
                Label6.Text = "Rs." + Convert.ToString(paidOvall);
                string totPaid = Label5.Text;
                string paid = totPaid.Split('.')[1];
                double fnltot = 0;
                double.TryParse(Convert.ToString(paid), out fnltot);
                Label8.Text = "Rs." + Convert.ToString(fnltot - paidOvall);
                Label24ex.Text = "Rs." + Convert.ToString(excessAmt);
                Label9.Text = "Rs." + Convert.ToString(excessAmt);
                btntransind.Enabled = true;
                if (save == true)
                {
                    Label12.Text = disAmt;
                    div7.Visible = true;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Do Not Have Balance Amount')", true);
            }
        }
        catch { }
    }

    protected void buttonok_Click(object sender, EventArgs e)
    {
        div7.Visible = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Mapping Successfully')", true);
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        div7.Visible = false;
        btnmapreset_Click(sender, e);
    }
    protected void ImageButton2_Click(object sender, EventArgs e)
    {
        divindi.Visible = false;
    }

    protected void btntransind_Click(object sender, EventArgs e)
    {

        if (rb_Journal.Checked) //journal 
        {
            if (ddlJournalType.SelectedIndex == 1 && getValidate())// if (checkPaidamount())              
            {
                JournalPaidAmount();
            }
            else if (ddlJournalType.SelectedIndex == 0 || ddlJournalType.SelectedIndex == 2 || ddlJournalType.SelectedIndex == 3)
            {
                JournalPaidAmount();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select Any One Ledger')", true);
        }
        else //transfer
        {
            bool boolnotRcptNo = false;
            bool confirmAlert = true;
            if (checkPaidamount(ref boolnotRcptNo, ref confirmAlert))
            {
                insertPaidAmount(ref boolnotRcptNo);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Make Ledger Mapping')", true);
        }
    }

    protected bool checkPaidamount(ref bool boolnotRcptNo, ref bool confirmAlert)
    {
        bool boolCheck = false;
        try
        {
            bool boolOld = false;
            bool boolNew = false;
            double oldPaidAmt = 0;
            double newPaidAmt = 0;
            foreach (GridViewRow gdrow in gridView4.Rows)
            {
                CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
                Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                double tempOldPaidAmt = 0;
                double.TryParse(Convert.ToString(lblpaid.Text), out tempOldPaidAmt);
                oldPaidAmt += tempOldPaidAmt;
            }
            foreach (GridViewRow row in gridView5.Rows)
            {
                CheckBox cbsel = (CheckBox)row.FindControl("cblsell");
                TextBox paid = (TextBox)row.FindControl("txt_paid");
                double tempNewPaidAmt = 0;
                double.TryParse(Convert.ToString(paid.Text), out tempNewPaidAmt);
                newPaidAmt += tempNewPaidAmt;
            }
            if (oldPaidAmt == 0 && newPaidAmt == 0)
            {
                boolCheck = true;
                boolnotRcptNo = true;
            }
            else if (oldPaidAmt != 0 && newPaidAmt != 0)
            {
                boolCheck = true;
            }
            else
            {
                boolCheck = true;
                boolnotRcptNo = true;
                confirmAlert = false;
            }
        }
        catch { }
        return boolCheck;
    }

    //show paid detials only
    protected void incPaid_Changed(object sender, EventArgs e)
    {
        if (rb_Journal.Checked)
        {
            bindGridJournalAdvance();
        }
        else
        {
            bindGridInd();
        }
    }
    //semester filter
    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txtsem, lbldept.Text, "--Select--");
        if (rb_Journal.Checked)
        {
            bindGridAllotJournal();
        }
        else
        {
            bindGrid5Ind();
        }
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txtsem, lbldept.Text, "--Select--");
        if (rb_Journal.Checked)
        {
            bindGridAllotJournal();
        }
        else
        {
            bindGrid5Ind();
        }
    }
    protected void bindTransfersem()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txtsem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            string clgcode = string.Empty;
            if (!rb_Journal.Checked)
            {
                if (rbl_AdmitTransfer.SelectedIndex == 0)
                    clgcode = Convert.ToString(ddlclgapplied.SelectedValue);
                else
                    clgcode = Convert.ToString(ddl_colg.SelectedValue);
            }
            else
            {
                clgcode = Convert.ToString(ddlcollege.SelectedValue);
            }

            ds = d2.loadFeecategory(clgcode, usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "TextVal";
                cbl_sem.DataValueField = "TextCode";
                cbl_sem.DataBind();

                ddlsem.DataSource = ds;
                ddlsem.DataTextField = "TextVal";
                ddlsem.DataValueField = "TextCode";
                ddlsem.DataBind();

                if (cbl_sem.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                        cbltext = Convert.ToString(cbl_sem.Items[i].Text);
                    }
                    if (cbl_sem.Items.Count == 1)
                        txtsem.Text = "" + linkName + "(" + cbltext + ")";
                    else
                        txtsem.Text = "" + linkName + "(" + cbl_sem.Items.Count + ")";
                    cb_sem.Checked = true;
                }
            }
        }
        catch { }
    }

    protected void bindAddsem()
    {
        try
        {
            ddlsem.Items.Clear();
            string linkName = string.Empty;
            string clgcode = string.Empty;
            if (!rb_Journal.Checked)
            {
                if (rbl_AdmitTransfer.SelectedIndex == 0)
                    clgcode = Convert.ToString(ddlclgapplied.SelectedValue);
                else
                    clgcode = Convert.ToString(ddl_colg.SelectedValue);
            }
            else
            {
                clgcode = Convert.ToString(ddlcollege.SelectedValue);
            }
            ds.Clear();
            ds = d2.loadFeecategory(clgcode, usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                ddlsem.DataSource = ds;
                ddlsem.DataTextField = "TextVal";
                ddlsem.DataValueField = "TextCode";
                ddlsem.DataBind();
            }
        }
        catch { }
    }
    #endregion
    protected void incSem_Changed(object sender, EventArgs e)
    {
        if (incSem.Checked)
        {
            txtsem.Enabled = true;
            bindTransfersem();
        }
        else
        {
            cbl_sem.Items.Clear();
            txtsem.Enabled = false;
        }
    }

    //add additional amount include to destination college
    protected void inclAddAmt_Changed(object sender, EventArgs e)
    {
        divtblOne.Attributes.Add("style", "float: left; top: 482px; position: absolute;");
        if (rb_Journal.Checked)
            divtblOne.Attributes.Add("style", "float: left; top: 462px; position: absolute;");

        divind.Attributes.Add("Style", "background-color: White; height: 614px; width: 1000px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 5px;border-radius: 10px;");
        if (inclAddAmt.Checked)
        {
            divtblOne.Attributes.Add("style", "float: left; top: 518px; position: absolute;");
            divind.Attributes.Add("Style", "background-color: White; height: 655px; width: 1000px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 5px;border-radius: 10px;");
            tdaddamt.Visible = true;
            txtamtind.Text = string.Empty;
            bindHeaderind();
            bindLedgerind();
            bindAddsem();
        }
        else
            tdaddamt.Visible = false;
    }

    #region Common Checkbox and Checkboxlist Event

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
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
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

    #endregion

    //allot insert
    protected bool insertAllotAmount(string appno, DateTime transdate, string finYearid)
    {
        bool updateOK = false;
        try
        {
            // feeallot insert record
            #region Allot insert
            foreach (GridViewRow gdrow in gridView4.Rows)
            {
                Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                Label lblfeecat = (Label)gdrow.FindControl("lbl_feecat");
                Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                try
                {
                    string delQ = " update ft_feeallot set IsTransfer='1' where app_no='" + appno + "' and feecategory='" + lblfeecat.Text + "' and headerfk='" + lblhedg.Text + "' and ledgerfk='" + lblledg.Text + "' and IsTransfer='0'";
                    delQ += " update ft_findailytransaction set paid_Istransfer='1' where app_no='" + appno + "' and feecategory='" + lblfeecat.Text + "' and headerfk='" + lblhedg.Text + "' and ledgerfk='" + lblledg.Text + "' and isnull(paid_Istransfer,'0')='0'";
                    d2.update_method_wo_parameter(delQ, "Text");
                }
                catch { }
            }

            foreach (GridViewRow row in gridView5.Rows)
            {
                Label hdrid = (Label)row.Cells[1].FindControl("lbl_hdrid");
                Label lgrid = (Label)row.Cells[1].FindControl("lbl_lgrid");
                Label feecat = (Label)row.Cells[1].FindControl("lbl_feecat");
                Label feeamt = (Label)row.Cells[1].FindControl("lbl_feeamt");
                Label totamt = (Label)row.Cells[1].FindControl("lbl_totamt");
                Label concession = (Label)row.Cells[1].FindControl("lbl_Concess");
                TextBox paid = (TextBox)row.Cells[1].FindControl("txt_paid");
                TextBox balance = (TextBox)row.Cells[1].FindControl("txt_bal");
                TextBox excess = (TextBox)row.Cells[1].FindControl("txt_exGrid2");
                TextBox topaid = (TextBox)row.Cells[1].FindControl("txt_tobePaid");
                double temptot = 0;
                double tempPaid = 0;
                double.TryParse(Convert.ToString(totamt.Text), out temptot);
                double.TryParse(Convert.ToString(topaid.Text), out tempPaid);
                if (feeamt.Text == "")
                    feeamt.Text = "0";
                if (totamt.Text == "")
                    totamt.Text = "0";
                if (concession.Text == "")
                    concession.Text = "0";
                if (paid.Text == "")
                    paid.Text = "0";
                if (balance.Text != "" && paid.Text != "0")
                    balance.Text = Convert.ToString(temptot - tempPaid);
                if (excess.Text == "")
                    excess.Text = "0";
                if (!rb_Journal.Checked)
                {
                    #region transfer
                    string updateFeeallot = "if exists (select * from FT_FeeAllot where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "')  and App_No in('" + appno + "') and isnull(IsTransfer,'0')='0') update FT_FeeAllot set AllotDate='" + transdate.ToString("MM/dd/yyyy") + "',FeeAmount='" + feeamt.Text + "',DeductAmout='" + concession.Text + "',DeductReason='0',FromGovtAmt='0',TotalAmount='" + totamt.Text + "',RefundAmount='0',IsFeeDeposit='1',PayMode='1',FeeCategory='" + feecat.Text + "',PaidStatus='0',DueAmount='0',FineAmount='0',BalAmount='" + balance.Text + "',paidamount='" + paid.Text + "' where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "')  and App_No in('" + appno + "') and isnull(IsTransfer,'0')='0' else   INSERT INTO FT_FeeAllot (AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK,paidamount,IsTransfer) VALUES('" + transdate.ToString("MM/dd/yyyy") + "',1," + appno + ", " + lgrid.Text + "," + hdrid.Text + ",'" + feeamt.Text + "','0','0','0','" + totamt.Text + "','0','1','','1','" + feecat.Text + "','','0','','0','0','" + balance.Text + "'," + finYearid + ",'" + paid.Text + "','0')";//and  FinYearFK='" + finYearid + "' and  FinYearFK='" + finYearid + "'modified
                    d2.update_method_wo_parameter(updateFeeallot, "Text");
                    updateOK = true;
                    #endregion
                }
                else
                {
                    #region journal
                    //string updateFeeallot = "if exists (select * from FT_FeeAllot where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "')  and App_No in('" + appno + "')) update FT_FeeAllot set FeeAmount='" + feeamt.Text + "',TotalAmount='" + totamt.Text + "', BalAmount='" + balance.Text + "',paidamount=isnull(paidamount,'0')+'" + paid.Text + "' where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "')  and App_No in('" + appno + "') else   INSERT INTO FT_FeeAllot (AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK,paidamount) VALUES('" + transdate.ToString("MM/dd/yyyy") + "',1," + appno + ", " + lgrid.Text + "," + hdrid.Text + ",'" + feeamt.Text + "','0','0','0','" + totamt.Text + "','0','1','','1','" + feecat.Text + "','','0','','0','0','" + balance.Text + "'," + finYearid + ",'" + paid.Text + "')";//and  FinYearFK='" + finYearid + "'  and  FinYearFK='" + finYearid + "'
                    //d2.update_method_wo_parameter(updateFeeallot, "Text");

                    //if (row.RowIndex == gridView5.Rows.Count - 1 && inclAddAmt.Checked)
                    //{
                    //    double amt = 0;
                    //    string ddlhdr = Convert.ToString(ddlhedind.SelectedItem.Value);
                    //    string ddllgr = Convert.ToString(ddlledind.SelectedItem.Value);
                    //    double.TryParse(Convert.ToString(txtamtind.Text), out amt);

                    //    if (!string.IsNullOrEmpty(ddlhdr) && !string.IsNullOrEmpty(ddllgr) && amt != 0)
                    //    {
                    //        string updateTransfer = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ddllgr + "') and HeaderFK in('" + ddlhdr + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "')) update FT_FeeAllot set AllotDate='" + transdate.ToString("MM/dd/yyyy") + "',FeeAmount=ISNULL(FeeAmount,'0')+'" + amt + "',DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount=ISNULL(TotalAmount,'0')+'" + amt + "',RefundAmount='0',IsFeeDeposit='1',PayMode='1',FeeCategory='" + feecat.Text + "',PaidStatus='0',DueAmount='0',FineAmount='0',BalAmount=ISNULL(BalAmount,'0')+'" + amt + "' where LedgerFK in('" + ddllgr + "') and HeaderFK in('" + ddlhdr + "') and FeeCategory in('" + feecat.Text + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "') else   INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + transdate.ToString("MM/dd/yyyy") + "',1," + appno + ", " + ddllgr + "," + ddlhdr + ",'" + amt + "','0','0','0','" + amt + "','0','1','','1','" + feecat.Text + "','','0','','0','0','" + amt + "'," + finYearid + ")";
                    //        d2.update_method_wo_parameter(updateTransfer, "Text");
                    //    }
                    //}
                    updateOK = true;
                    #endregion
                }

            }

            #endregion
        }
        catch { }
        return updateOK;
    }
    //student detials and paidamount insert
    protected bool insertPaidAmount(ref bool boolnotRcptNo)
    {
        bool updateOK = false;
        try
        {
            bool checkReceiptNo = false;
            bool boolApplNot = false;
            ArrayList htCheckVal = new ArrayList();
            ArrayList NewhtCheckVal = new ArrayList();
            StringBuilder sbOldRecptDate = new StringBuilder();
            StringBuilder sbOldRecptCode = new StringBuilder();
            double oldAmt = 0;
            StringBuilder sbNewRecptDate = new StringBuilder();
            StringBuilder sbNewRecptCode = new StringBuilder();
            double newAmt = 0;
            double newExcessAmt = 0;
            string oldRoll = string.Empty;
            string oldReg = string.Empty;
            string oldRollAdmit = string.Empty;
            string studAdmDate = string.Empty;
            string fstClgcode = string.Empty;
            string fstBatchYr = string.Empty;
            string fstDegreecode = string.Empty;
            string fstSection = string.Empty;
            string fstSeat = string.Empty;
            string fstSeatCode = string.Empty;
            string finYearid = string.Empty;
            string entryUserCode = string.Empty;
            DateTime transdate = Convert.ToDateTime(txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2]);

            string batch = string.Empty;
            string sec = string.Empty;
            string sem = string.Empty;
            string colCode = string.Empty;
            string degcode = string.Empty;
            string seatype = string.Empty;
            string Rcptno = string.Empty;
            string appno = string.Empty;
            Dictionary<string, string> dtReceipt = new Dictionary<string, string>();
            Dictionary<string, string> arRcptfk = new Dictionary<string, string>();
            string rollno = string.Empty;
            string hedgid = ledgermappingheaderValue();
            if (!incJournal.Checked)
            {
                if (hedgid != "")
                    Rcptno = generateReceiptNo(hedgid, ref dtReceipt, ref arRcptfk);
            }
            else
            {
                //if (hedgid != "")
                Rcptno = generateJournalNo(hedgid, colCode);
            }
            if (boolnotRcptNo)
                Rcptno = "NotPaidAmount";

            if (!string.IsNullOrEmpty(Rcptno) && Rcptno != "0")
            {
                if (checkAdvanceAmt())//if excess amount available hedader and ledger must be available
                {
                    #region student details get
                    if (rbl_AdmitTransfer.SelectedIndex == 0)
                    {
                        #region applied
                        if (txt_roll1.Text.Trim() != "" && txt_roll.Text.Trim() != "")
                        {
                            rollno = Convert.ToString(txt_roll1.Text);
                            string newrollNo = txt_roll_no1.Text.Trim();
                            if (string.IsNullOrEmpty(newrollNo))
                                newrollNo = rollno;

                            string degree = "";
                            string dept = "";
                            string stream = "";
                            string name = "";
                            string query = "select a.parent_name,a.stud_name, a.Stud_Type,c.Course_Name,dt.Dept_Name,a.degree_code,a.Current_Semester  ,a.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,a.app_no,a.seattype  from applyn a ,Degree d,course c,Department dt,collinfo co where  a.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code  and a.app_formno='" + rollno + "' and d.college_code='" + ddlclgapplied.SelectedValue + "'";
                            DataSet ds1 = d2.select_method_wo_parameter(query, "Text");
                            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                            {
                                name = Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]);
                                batch = Convert.ToString(ds1.Tables[0].Rows[0]["Batch_Year"]);
                                degree = Convert.ToString(ds1.Tables[0].Rows[0]["Course_Name"]);
                                dept = Convert.ToString(ds1.Tables[0].Rows[0]["Dept_Name"]);
                                // sec = Convert.ToString( ds1.Tables[0].Rows[0]["Sections"]);
                                sec = txt_sec1.Text.Trim();
                                sem = Convert.ToString(ds1.Tables[0].Rows[0]["Current_Semester"]);
                                colCode = Convert.ToString(ds1.Tables[0].Rows[0]["college_code"]);
                                stream = Convert.ToString(ds1.Tables[0].Rows[0]["type"]);
                                degcode = Convert.ToString(ds1.Tables[0].Rows[0]["degree_code"]);
                                appno = Convert.ToString(ds1.Tables[0].Rows[0]["app_no"]);
                                string sndSeat = Convert.ToString(ds1.Tables[0].Rows[0]["seattype"]);

                                finYearid = d2.getCurrentFinanceYear(usercode, ddlclgapplied.SelectedValue);

                                //Update Registration table
                                string updateApp = "update applyn set admission_status =1 where app_no ='" + appno + "'";
                                d2.update_method_wo_parameter(updateApp, "Text");

                                if (ddladmis.SelectedItem.Text.Trim() == "After Admission")
                                {
                                    string tempAppno = getappNo();
                                    string selQReg = " select roll_no,reg_no,roll_admit,adm_date from registration where app_no='" + tempAppno + "'";
                                    DataSet dsReg = d2.select_method_wo_parameter(selQReg, "Text");
                                    if (dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0)
                                    {
                                        oldRoll = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_no"]);
                                        oldReg = Convert.ToString(dsReg.Tables[0].Rows[0]["reg_no"]);
                                        oldRollAdmit = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_admit"]);
                                        studAdmDate = Convert.ToString(dsReg.Tables[0].Rows[0]["adm_date"]);
                                    }
                                    string updateReg = "update Registration set DelFlag=1 where app_no='" + tempAppno + "'";
                                    d2.update_method_wo_parameter(updateReg, "Text");

                                    string insReg = "  insert into Registration (App_No,Adm_Date,Roll_Admit,Roll_No,RollNo_Flag,Reg_No,Stud_Name,Batch_Year,degree_code,college_code,CC,DelFlag,Exam_Elg,mode,Current_Semester,Sections)values ('" + appno + "','" + transdate.ToString("MM/dd/yyyy") + "','" + rollno + "','" + newrollNo + "','1','" + rollno + "','" + name + "','" + batch + "','" + degcode + "','" + colCode + "','0','0','OK',3,1,'" + sec + "')";
                                    d2.update_method_wo_parameter(insReg, "Text");
                                }
                                //new insert to studentransfer table
                                fstClgcode = Convert.ToString(lbltempfstclg.Text);
                                fstBatchYr = Convert.ToString(txt_batch.Text);
                                fstDegreecode = Convert.ToString(lbltempfstdeg.Text);
                                fstSection = Convert.ToString(txt_sec.Text);
                                fstSeat = Convert.ToString(txt_seattype.Text);
                                fstSeatCode = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + ddlcollege.SelectedValue + "'  and textval='" + fstSeat.Trim() + "'"));
                                boolApplNot = true;
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        #region not Applied
                        rollno = Convert.ToString(txt_roll.Text.Trim());
                        string newrollNo = txt_roll_no.Text.Trim();
                        if (string.IsNullOrEmpty(newrollNo))
                            newrollNo = rollno;
                        appno = getappNo();
                        if (appno != "0")
                        {
                            string query = " select app_no,Stud_Name,Batch_Year,degree_code,college_code,Current_Semester,Sections from Registration where college_code='" + ddlcollege.SelectedValue + "'";
                            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                                query = query + " and  app_no='" + appno + "'";
                            else
                                query = " select app_no,Stud_Name,Batch_Year,degree_code,college_code,Current_Semester,'' Sections from applyn where app_formno='" + rollno + "' and college_code='" + ddlcollege.SelectedValue + "'";
                            DataSet ds1 = d2.select_method_wo_parameter(query, "Text");
                            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                            {
                                if (ddl_batch.Items.Count > 0)
                                    batch = Convert.ToString(ddl_batch.SelectedItem.Value);
                                if (ddl_sec.Items.Count > 0)
                                    sec = Convert.ToString(ddl_sec.SelectedItem.Value);
                                if (ddl_sem.Items.Count > 0)
                                    sem = Convert.ToString(ddl_sem.SelectedItem.Value);
                                if (ddl_colg.Items.Count > 0)
                                    colCode = Convert.ToString(ddl_colg.SelectedItem.Value);
                                if (ddl_dept.Items.Count > 0)
                                    degcode = Convert.ToString(ddl_dept.SelectedItem.Value);
                                if (ddl_seattype.Items.Count > 0)
                                    seatype = Convert.ToString(ddl_seattype.SelectedItem.Value);
                                appno = Convert.ToString(ds1.Tables[0].Rows[0]["app_no"]);
                                finYearid = d2.getCurrentFinanceYear(usercode, colCode);
                                string curtime = DateTime.Now.ToShortTimeString();
                                if (colCode != "" && batch != "" && degcode != "" && appno != "")
                                {
                                    //applyn update
                                    string AppUpd = " update applyn set degree_code='" + degcode + "',seattype='" + seatype + "',college_code='" + ddl_colg.SelectedItem.Value + "' where app_no='" + appno + "'";
                                    int Aup = d2.update_method_wo_parameter(AppUpd, "Text");
                                    //Update Registration table
                                    if (ddladmis.SelectedItem.Text.Trim() == "After Admission")
                                    {
                                        string selQReg = " select roll_no,reg_no,roll_admit,adm_date from registration where app_no='" + appno + "'";
                                        DataSet dsReg = d2.select_method_wo_parameter(selQReg, "Text");
                                        if (dsReg.Tables.Count > 0 && dsReg.Tables[0].Rows.Count > 0)
                                        {
                                            oldRoll = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_no"]);
                                            oldReg = Convert.ToString(dsReg.Tables[0].Rows[0]["reg_no"]);
                                            oldRollAdmit = Convert.ToString(dsReg.Tables[0].Rows[0]["roll_admit"]);
                                            studAdmDate = Convert.ToString(dsReg.Tables[0].Rows[0]["adm_date"]);
                                        }
                                        string upReg = " update Registration set degree_code='" + degcode + "', college_code=" + ddl_colg.SelectedItem.Value + ", batch_year=" + batch + ",Current_Semester='" + sem + "',Sections='" + sec + "',Roll_No='" + newrollNo + "',Adm_Date='" + transdate.ToString("MM/dd/yyyy") + "' where App_No=" + appno + "  ";
                                        d2.update_method_wo_parameter(upReg, "Text");
                                    }
                                    //new insert to studentransfer table
                                    fstClgcode = Convert.ToString(lbltempfstclg.Text);
                                    fstBatchYr = Convert.ToString(txt_batch.Text);
                                    fstDegreecode = Convert.ToString(lbltempfstdeg.Text);
                                    fstSection = Convert.ToString(txt_sec.Text);
                                    fstSeat = Convert.ToString(txt_seattype.Text);
                                    fstSeatCode = Convert.ToString(d2.GetFunction("select Textcode from TextValTable where textcriteria='seat' and college_code='" + ddlcollege.SelectedValue + "'  and textval='" + fstSeat.Trim() + "'"));
                                    boolApplNot = true;
                                    // UpdateAdmissionNo(appno);
                                    //  admNo = autoGenDS.AdmissionNoAndApplicationNumberGeneration(0, appno: appNo, Mode: Mode, DegreeCode: sndDegreecode, CollegeCode: collegecodeTemp, SeatType: sndSeat);

                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                    // bool boolchecks = false;
                    if (boolApplNot)
                    {
                        bool inSertAlot = insertAllotAmount(appno, transdate, finYearid);
                        if (inSertAlot)
                        {
                            bool boolOld = false;
                            bool boolNew = false;
                            #region old Paidamount

                            foreach (GridViewRow gdrow in gridView4.Rows)
                            {
                                CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
                                if (cb.Checked)
                                {
                                    Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                                    Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                                    Label lblfeecat = (Label)gdrow.FindControl("lbl_feecat");
                                    Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                                    Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                                    if (lblpaid.Text != "" && lblpaid.Text != "0")
                                    {

                                        string selOldQ = " select distinct convert(varchar(10),transdate,103) as transdate,transcode,debit from FT_FinDailyTransaction  where App_No='" + appno + "' and headerfk='" + lblhedg.Text + "' and ledgerfk='" + lblledg.Text + "'  and FeeCategory='" + lblfeecat.Text + "' and isnull(paid_Istransfer,'0')='1'";
                                        DataSet dsOld = d2.select_method_wo_parameter(selOldQ, "Text");
                                        if (dsOld.Tables.Count > 0 && dsOld.Tables[0].Rows.Count > 0)
                                        {
                                            for (int old = 0; old < dsOld.Tables[0].Rows.Count; old++)
                                            {
                                                if (!htCheckVal.Contains(Convert.ToString(dsOld.Tables[0].Rows[old]["transdate"])))
                                                {
                                                    sbOldRecptDate.Append(Convert.ToString(dsOld.Tables[0].Rows[old]["transdate"]) + ",");
                                                    htCheckVal.Add(Convert.ToString(dsOld.Tables[0].Rows[old]["transdate"]));
                                                }
                                                if (!htCheckVal.Contains(Convert.ToString(dsOld.Tables[0].Rows[old]["transcode"])))
                                                {
                                                    sbOldRecptCode.Append(Convert.ToString(dsOld.Tables[0].Rows[old]["transcode"]) + ",");
                                                    htCheckVal.Add(Convert.ToString(dsOld.Tables[0].Rows[old]["transcode"]));
                                                }
                                                double tempPaidAmt = 0;
                                                double.TryParse(Convert.ToString(dsOld.Tables[0].Rows[old]["debit"]), out tempPaidAmt);
                                                oldAmt += tempPaidAmt;
                                            }
                                        }
                                        getOldPayment(appno, lblhedg.Text, lblledg.Text, lblfeecat.Text, Rcptno, lblpaid.Text, transdate.ToString("MM/dd/yyyy"));
                                        boolOld = true;
                                    }
                                }
                            }
                            #endregion

                            #region Dailytransaction insert
                            //entryUserCode = d2.GetFunction(" select distinct entryusercode from FT_FinDailyTransaction where app_no='" + appno + "'");//commented by saranya on 28/12/2017
                            foreach (GridViewRow row in gridView5.Rows)
                            {
                                CheckBox cbsel = (CheckBox)row.FindControl("cblsell");
                                TextBox toBepaid = (TextBox)row.FindControl("txt_tobePaid");
                                double toBePaid = 0;
                                double.TryParse(Convert.ToString(toBepaid.Text), out toBePaid);
                                if (cbsel.Checked || toBePaid != 0)
                                {
                                    Label hdrid = (Label)row.FindControl("lbl_hdrid");
                                    Label lgrid = (Label)row.FindControl("lbl_lgrid");
                                    Label feecat = (Label)row.FindControl("lbl_feecat");
                                    Label feeamt = (Label)row.FindControl("lbl_feeamt");
                                    Label totamt = (Label)row.FindControl("lbl_totamt");
                                    Label concession = (Label)row.FindControl("lbl_Concess");
                                    TextBox paid = (TextBox)row.FindControl("txt_paid");
                                    TextBox balance = (TextBox)row.FindControl("txt_bal");
                                    TextBox excess = (TextBox)row.FindControl("txt_exGrid2");

                                    if (feeamt.Text == "")
                                        feeamt.Text = "0";
                                    if (totamt.Text == "")
                                        totamt.Text = "0";
                                    if (concession.Text == "")
                                        concession.Text = "0";
                                    if (paid.Text == "")
                                        paid.Text = "0";
                                    if (balance.Text == "")
                                        balance.Text = "0";
                                    if (excess.Text == "")
                                        excess.Text = "0";
                                    if (paid.Text != "0")
                                    {
                                        string selQy = "select distinct paymode from ft_findailytransaction where app_no='" + appno + "' and isnull(iscanceled,'0')='1' and debit='" + paid.Text + "'";
                                        string payMode = d2.GetFunction(selQy);
                                        payMode = "1";
                                        if (payMode != "0")
                                        {
                                            //daily transaction
                                            //if exists(select * from FT_FinDailyTransaction where HeaderFK='" + hdrid.Text + "' and LedgerFK='" + lgrid.Text + "' and FeeCategory='" + feecat.Text + "' and TransCode='" + Rcptno + "' and App_No='" + appno + "' and FinYearFK='" + finYearid + "')update FT_FinDailyTransaction set Debit='" + paid.Text + "',TransDate='" + transdate.ToString("MM/dd/yyyy") + "',TransTime='" + DateTime.Now.ToShortTimeString() + "',IsCanceled='0',IsCollected='1',paymode ='" + payMode + "', narration='" + txtnaration.Text.Trim() + "' where HeaderFK='" + hdrid.Text + "' and LedgerFK='" + lgrid.Text + "' and FeeCategory='" + feecat.Text + "' and TransCode='" + Rcptno + "' and App_No='" + appno + "'  and FinYearFK='" + finYearid + "' else   
                                            string INSdaily = " insert into FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,LedgerFK,HeaderFK,FeeCategory,Debit,FinYearFK,App_No,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype,narration) values('" + transdate.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToShortTimeString() + "','" + Rcptno + "','1','" + lgrid.Text + "','" + hdrid.Text + "','" + feecat.Text + "','" + paid.Text + "','" + finYearid + "','" + appno + "','0','1','" + payMode + "','1','" + userCode + "','3','" + txtnaration.Text.Trim() + "')";//entryUserCode is modified as usercode by saranya
                                            boolNew = true;
                                            d2.update_method_wo_parameter(INSdaily, "Text");
                                            checkReceiptNo = true;
                                            if (!NewhtCheckVal.Contains(transdate))
                                            {
                                                sbNewRecptDate.Append(transdate + ",");
                                                NewhtCheckVal.Add(transdate);
                                            }
                                            if (!NewhtCheckVal.Contains(Rcptno))
                                            {
                                                sbNewRecptCode.Append(Rcptno + ",");
                                                NewhtCheckVal.Add(Rcptno);
                                            }
                                            double tempNewPaidAmt = 0;
                                            double.TryParse(Convert.ToString(paid.Text), out tempNewPaidAmt);
                                            newAmt += tempNewPaidAmt;
                                            if (gridView5.Rows.Count > 0)
                                            {
                                                string excessval = string.Empty;
                                                try
                                                {
                                                    if (Label9.Text != "" || hiddnewPaid.Value != "")
                                                    {
                                                        excessval = Convert.ToString(Label9.Text).Split('.')[1];
                                                        if (excessval == "" || excessval == "0")
                                                            excessval = Convert.ToString(hiddnewPaid.Value);
                                                    }
                                                }
                                                catch { }
                                                if (excessval == "" || excessval == "0")
                                                    excessval = "0";
                                                if (excessval != "0")
                                                {
                                                    // excessval = allotExcessAmt(appno, feecat.Text, excessval, finYearid, transdate.ToString("MM/dd/yyyy"), Rcptno);
                                                    double tempextraAmt = 0;
                                                    double.TryParse(excessval, out tempextraAmt);
                                                    newExcessAmt += tempextraAmt;
                                                    excessval = allotAdvanceAmt(appno, feecat.Text, excessval, finYearid, transdate.ToString("MM/dd/yyyy"), Rcptno);
                                                    Label9.Text = "";
                                                    hiddnewPaid.Value = "0";
                                                }
                                            }
                                        }
                                    }
                                    updateOK = true;
                                }
                            }

                            #endregion
                            //updateOK && 
                            if (((boolOld && boolNew) || (!boolOld && !boolNew)))
                            {
                                #region update receipt,new insert to transfer table and print
                                if (checkReceiptNo)
                                    updateReceiptNo(Rcptno, finYearid);
                                //new entry to transfer table
                                transfer(appno, fstDegreecode, ddl_dept.SelectedItem.Value, fstSection, sec, fstClgcode, ddl_colg.SelectedItem.Value, ddl_batch.SelectedItem.Value, fstSeatCode, seatype, sbOldRecptDate, sbOldRecptCode, oldAmt, sbNewRecptDate, sbNewRecptCode, newAmt, newExcessAmt, oldRoll, oldReg, oldRollAdmit, studAdmDate);
                                string tempCollegecode = string.Empty;
                                if (rbl_AdmitTransfer.SelectedIndex == 0)
                                {
                                    tempCollegecode = ddlclgapplied.SelectedValue;
                                }
                                else
                                {
                                    tempCollegecode = ddl_colg.SelectedValue;
                                }
                                // divindi.Visible = false;
                                transFromClear();
                                transToClear();
                                divindi.Visible = false;
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Transfered Successfully')", true);

                                //==========================Added by Saranya on 10/04/2018=============================//
                                string entrycode = Session["Entry_Code"].ToString();
                                string formname = "Journal";
                                int savevalue = 1;
                                string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                                string doa = DateTime.Now.ToString("MM/dd/yyy");
                                IPHostEntry host;
                                string localip = "";
                                host = Dns.GetHostEntry(Dns.GetHostName());
                                foreach (IPAddress ip in host.AddressList)
                                {
                                    if (ip.AddressFamily.ToString() == "InterNetwork")
                                    {
                                        localip = ip.ToString();
                                    }
                                }
                                string details = "OldRollNo - " + oldRoll + " : CollegeCode - " + colCode + " : OldReceiptNO -" + sbOldRecptCode + " : OldAmount -" + oldAmt + " : NewRollNo - " + txt_roll_no1.Text.Trim() + " : NewReceiptNO -" + sbNewRecptCode + " : NewAmount -" + newAmt + " : Date - " + toa + "";
                                string ctsname = "";
                                if (savevalue == 1)
                                {
                                    ctsname = "Journal Transfer";
                                    string hostName = Dns.GetHostName();
                                    d2.insertEinanceUserActionLog(entrycode, formname, 1, toa, doa, details, ctsname, localip);
                                }

                                //============================================================================//

                                if (incJournal.Checked && checkReceiptNo)
                                {
                                    transferReceipt("Journal", appno, tempCollegecode, transdate.ToString("MM/dd/yyyy"), Convert.ToString(sbNewRecptCode));
                                }
                                else
                                {
                                    divindi.Visible = false;
                                    transFromClear();
                                    transToClear();
                                }
                                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Transfered Successfully')", true);
                                #endregion
                            }
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Not Alloted Amount')", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Student Detail Not Updated')", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Set Advance Header and Ledger')", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Receipt/Journal No Not Generated')", true);
        }
        catch { }
        return updateOK;
    }

    protected void updateReceiptNo(string Rcptno, string finYearid)
    {
        try
        {
            #region Update Receipt No

            if (Convert.ToInt32(Session["save1"]) != 5)
            {
                string collegecode1 = string.Empty;
                if (!rb_Journal.Checked)
                {
                    if (rbl_AdmitTransfer.SelectedIndex == 0)
                        collegecode1 = Convert.ToString(ddlclgapplied.SelectedValue);
                    else
                        collegecode1 = Convert.ToString(ddl_colg.SelectedValue);
                }
                else
                {
                    collegecode1 = Convert.ToString(ddlcollege.SelectedValue);
                }
                string updateRecpt = string.Empty;
                if (Convert.ToInt32(Session["isHeaderwise"]) == 0 || Convert.ToInt32(Session["isHeaderwise"]) == 2)
                {
                    if (!incJournal.Checked)
                    {
                        Rcptno = Rcptno.Remove(0, Convert.ToString(Session["acronym"]).Length);
                        updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + Rcptno + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + "  and isnull(onlineRec,'')<>'1')  and isnull(onlineRec,'')<>'1'";
                        d2.update_method_wo_parameter(updateRecpt, "Text");
                    }
                    else//journal no
                    {
                        Rcptno = Rcptno.Remove(0, Convert.ToString(Session["acronym"]).Length);
                        updateRecpt = " update FM_FinCodeSettings set JournalStNo=" + Rcptno + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + "  and isnull(onlineRec,'')<>'1')  and isnull(onlineRec,'')<>'1'";
                        d2.update_method_wo_parameter(updateRecpt, "Text");
                    }
                }
                else
                {
                    #region old
                    //ArrayList arrcpt = new ArrayList();
                    //foreach (KeyValuePair<string, string> reptUpdate in dtReceipt)
                    //{
                    //    string headerfk = reptUpdate.Key.ToString();
                    //    Rcptno = reptUpdate.Value.ToString();
                    //    if (!arrcpt.Contains(Rcptno))
                    //    {
                    //        string hdFkval = string.Empty;
                    //        if (arRcptfk.ContainsKey(Rcptno))
                    //        {
                    //            hdFkval = arRcptfk[Rcptno].ToString();
                    //            arrcpt.Add(Rcptno);
                    //            Rcptno = Rcptno.Remove(0, Convert.ToString(hdFkval.Split('-')[1]).Length);
                    //            updateRecpt = "update FM_HeaderFinCodeSettings set RcptStNo=" + Rcptno + "+1 where HeaderSettingPK=" + hdFkval.Split('-')[0] + " and FinyearFK=" + finYearid + " and CollegeCode=" + ddl_colg.SelectedItem.Value + "";
                    //            d2.update_method_wo_parameter(updateRecpt, "Text");

                    //        }
                    //    }
                    //}
                    #endregion
                }

            }

            #endregion
        }
        catch { }
    }

    protected string ledgermappingheaderValue()
    {
        string hedgid = string.Empty;
        ArrayList arrcpt = new ArrayList();
        foreach (GridViewRow hdrow in gridView5.Rows)
        {
            //CheckBox cbsnd = (CheckBox)hdrow.FindControl("cblsell");
            //if (cbsnd.Checked)
            //{
            Label hdrid = (Label)hdrow.Cells[1].FindControl("lbl_hdrid");
            if (hedgid == "")
            {
                hedgid = Convert.ToString(hdrid.Text);
                arrcpt.Add(Convert.ToString(hdrid.Text));
            }
            else
            {
                if (!arrcpt.Contains(hdrid.Text))
                {
                    hedgid = hedgid + "'" + "," + "'" + Convert.ToString(hdrid.Text);
                    arrcpt.Add(Convert.ToString(hdrid.Text));
                }
            }
            // }
        }
        return hedgid;
    }

    protected void getOldPayment(string appNo, string hdFK, string ldFK, string feeCat, string receiptno, string amt, string dtrcpt)
    {
        string journalType = string.Empty;
        if (ddlJournalType.SelectedIndex == 1)
            journalType = " and transcode in('" + receiptno + "') and isnull(paid_Istransfer,'0')='0'";
        else if (rb_transfer.Checked)
            journalType = " and isnull(paid_Istransfer,'0')='1'";
        else if (ddlJournalType.SelectedIndex == 0 || ddlJournalType.SelectedIndex == 2)
            journalType = " and isnull(paid_Istransfer,'0')='0'";

        string selQ = " select distinct memtype,paymode,ddno,dddate,ddbankcode,ddbankbranch,isdeposited,depositeddate,iscollected,collecteddate,entryusercode,finyearfk,receipttype,actualfinyearfk,deposite_bankfk,narration from ft_findailytransaction  where App_No='" + appNo + "' and headerfk='" + hdFK + "' and ledgerfk='" + ldFK + "'  and FeeCategory='" + feeCat + "' " + journalType + "";
        DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
        if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
        {
            //for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
            //{
            string insertDebit = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected,CollectedDate,IsDeposited,DepositedDate,ActualFinYearFK,deposite_bankfk) VALUES('" + dtrcpt + "','" + DateTime.Now.ToLongTimeString() + "','" + receiptno + "', " + Convert.ToString(dsVal.Tables[0].Rows[0]["memtype"]) + ", " + appNo + ", " + ldFK + ", " + hdFK + ", " + feeCat + ", '" + amt + "','0', " + Convert.ToString(dsVal.Tables[0].Rows[0]["paymode"]) + ", '" + Convert.ToString(dsVal.Tables[0].Rows[0]["ddno"]) + "', '" + Convert.ToString(dsVal.Tables[0].Rows[0]["dddate"]) + "', '" + Convert.ToString(dsVal.Tables[0].Rows[0]["ddbankcode"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[0]["ddbankbranch"]) + "', 3, '0', 0, '" + Convert.ToString(dsVal.Tables[0].Rows[0]["narration"]) + "', '0', '0', '0', 0, " + Convert.ToString(dsVal.Tables[0].Rows[0]["entryusercode"]) + ", " + Convert.ToString(dsVal.Tables[0].Rows[0]["finyearfk"]) + ",'" + Convert.ToString(dsVal.Tables[0].Rows[0]["receipttype"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[0]["isdeposited"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[0]["depositeddate"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[0]["iscollected"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[0]["collecteddate"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[0]["actualfinyearfk"]) + "','" + Convert.ToString(dsVal.Tables[0].Rows[0]["deposite_bankfk"]) + "')";

            d2.update_method_wo_parameter(insertDebit, "Text");
            // }
        }
    }
    protected string allotExcessAmt(string appno, string feecat, string excessval, string finYearid, string transdate, string transcode)
    {
        try
        {
            #region excess amount

            if (excessval != "0")
            {
                string select = "if exists(select * from FT_ExcessDet where App_No='" + appno + "' and ExcessType='1' and FinYearFK='" + finYearid + "' and FeeCategory='" + feecat + "')update FT_ExcessDet set ExcessAmt=isnull(ExcessAmt,'0')+'" + excessval + "',BalanceAmt=isnull(BalanceAmt,'0')+'" + excessval + "' where App_No='" + appno + "' and ExcessType='1' and FinYearFK='" + finYearid + "' and FeeCategory='" + feecat + "' else insert into FT_ExcessDet (ExcessTransDate,dailytranscode,TransTime,MemType,App_No ,ExcessType,ExcessAmt,BalanceAmt,FinYearFK , FeeCategory) values('" + transdate + "','" + transcode + "','" + DateTime.Now.ToLongTimeString() + "','1','" + appno + "','1','" + excessval + "','" + excessval + "','" + finYearid + "','" + feecat + "')";
                int exCal = d2.update_method_wo_parameter(select, "Text");
                if (exCal > 0)
                {
                    string getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='1'");
                    for (int i = 0; i < gridView5.Rows.Count; i++)
                    {
                        Label header = (Label)gridView5.Rows[i].FindControl("lbl_hdrid");
                        Label ledger = (Label)gridView5.Rows[i].FindControl("lbl_lgrid");
                        Label feecatg = (Label)gridView5.Rows[i].FindControl("lbl_yearsem");
                        Label totalamt = (Label)gridView5.Rows[i].FindControl("lbl_totamt");
                        TextBox excessamt = (TextBox)gridView5.Rows[i].FindControl("txt_exGrid2");
                        double tempExcess = 0;
                        double.TryParse(Convert.ToString(excessamt.Text), out tempExcess);
                        if (tempExcess != 0)
                        {
                            string selqry = "select * from FT_ExcessLedgerDet if  exists(select * from FT_ExcessLedgerDet where  ExcessDetFK='" + getvalue + "' and HeaderFK='" + header.Text + "' and LedgerFK='" + ledger.Text + "' and FinYearFK='" + finYearid + "' and FeeCategory in('" + feecat + "') )update FT_ExcessLedgerDet set ExcessAmt=isnull(ExcessAmt,'0')+'" + tempExcess + "',BalanceAmt=isnull(BalanceAmt,'0')+'" + tempExcess + "',HeaderFK ='" + header.Text + "',LedgerFK='" + ledger.Text + "' where ExcessDetFK='" + getvalue + "' and HeaderFK ='" + header.Text + "' and LedgerFK='" + ledger.Text + "' and FinYearFK='" + finYearid + "' and FeeCategory in('" + feecat + "') else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,FinYearFK,FeeCategory) values('" + header.Text + "','" + ledger.Text + "','" + tempExcess + "','" + tempExcess + "','" + getvalue + "','" + finYearid + "','" + feecat + "')";
                            d2.update_method_wo_parameter(selqry, "Text");
                            excessval = "0";
                        }
                    }
                }
            }

            #endregion
        }
        catch { }
        return excessval;
    }

    protected string allotAdvanceAmt(string appno, string feecat, string excessval, string finYearid, string transdate, string transcode)
    {
        try
        {
            #region excess amount

            if (excessval != "0")
            {
                //and FinYearFK='" + finYearid + "' and FeeCategory='" + feecat + "' and ExcessTransDate='" + transdate + "' and dailytranscode='" + transcode + "'
                string select = "if exists(select * from FT_ExcessDet where App_No='" + appno + "' and ExcessType='1'  and Ex_JournalEntry='1')update FT_ExcessDet set ExcessAmt=isnull(ExcessAmt,'0')+'" + excessval + "',BalanceAmt=isnull(BalanceAmt,'0')+'" + excessval + "' where App_No='" + appno + "' and ExcessType='1' and Ex_JournalEntry='1' else insert into FT_ExcessDet (ExcessTransDate,dailytranscode,TransTime,MemType,App_No ,ExcessType,ExcessAmt,BalanceAmt,FinYearFK , FeeCategory,Ex_JournalEntry) values('" + transdate + "','" + transcode + "','" + DateTime.Now.ToLongTimeString() + "','1','" + appno + "','1','" + excessval + "','" + excessval + "','" + finYearid + "','" + feecat + "','1')";
                int exCal = d2.update_method_wo_parameter(select, "Text");
                if (exCal > 0)
                {
                    string getvalue = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "' and ExcessType ='1'  and Ex_JournalEntry='1'");
                    //and ExcessTransDate='" + transdate + "' and dailytranscode='" + transcode + "'
                    if (getvalue != "0")
                    {
                        string header = Convert.ToString(ddlMainJrHed.SelectedValue);
                        string ledger = Convert.ToString(ddlMainJrLed.SelectedValue);
                        double tempExcess = 0;
                        double.TryParse(excessval, out tempExcess);
                        if (tempExcess != 0)
                        {
                            string selqry = "select * from FT_ExcessLedgerDet if  exists(select * from FT_ExcessLedgerDet where  ExcessDetFK='" + getvalue + "' and HeaderFK='" + header + "' and LedgerFK='" + ledger + "'  and FeeCategory in('" + feecat + "') )update FT_ExcessLedgerDet set ExcessAmt=isnull(ExcessAmt,'0')+'" + tempExcess + "',BalanceAmt=isnull(BalanceAmt,'0')+'" + tempExcess + "' where ExcessDetFK='" + getvalue + "' and HeaderFK ='" + header + "' and LedgerFK='" + ledger + "'  and FeeCategory in('" + feecat + "') else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,FinYearFK,FeeCategory) values('" + header + "','" + ledger + "','" + tempExcess + "','" + tempExcess + "','" + getvalue + "','" + finYearid + "','" + feecat + "')";
                            d2.update_method_wo_parameter(selqry, "Text");
                            excessval = "0";
                            //and FinYearFK='" + finYearid + "'
                        }
                    }

                }
            }

            #endregion
        }
        catch { }
        return excessval;
    }

    protected bool transfer(string app_no, string olddeg, string deptcode, string oldsec, string sec, string oldcolg, string chngeClgCode, string batch, string fstSeat, string sndSeat, StringBuilder sbOldRecptDate, StringBuilder sbOldRecptCode, double oldAmt, StringBuilder sbNewRecptDate, StringBuilder sbNewRecptCode, double newAmt, double newExcessAmt, string oldRoll, string oldReg, string oldRollAdmit, string studAdmDate)
    {
        bool save = false;
        try
        {
            if (sbOldRecptDate.Length > 0)
                sbOldRecptDate.Remove(sbOldRecptDate.Length - 1, 1);
            if (sbOldRecptCode.Length > 0)
                sbOldRecptCode.Remove(sbOldRecptCode.Length - 1, 1);

            if (sbNewRecptDate.Length > 0)
                sbNewRecptDate.Remove(sbNewRecptDate.Length - 1, 1);
            if (sbNewRecptCode.Length > 0)
                sbNewRecptCode.Remove(sbNewRecptCode.Length - 1, 1);
            string transferDate = Convert.ToString(txt_date.Text.Split('/')[1] + "/" + txt_date.Text.Split('/')[0] + "/" + txt_date.Text.Split('/')[2]);
            string insQ = "  insert into ST_Student_Transfer(AppNo,TransferDate,TransferTime,FromDegree,Todegree,FromSection,ToSection,FromCollege,Tocollege,FromSeatType,ToSeatType) values('" + app_no + "','" + transferDate + "','" + DateTime.Now.ToShortTimeString() + "','" + olddeg + "','" + deptcode + "','" + oldsec + "','" + sec + "','" + oldcolg + "','" + chngeClgCode + "','" + fstSeat + "','" + sndSeat + "')";
            int ins = d2.update_method_wo_parameter(insQ, "Text");
            if (ins > 0)
            {
                string StudPK = d2.GetFunction("select studentTransferPK from ST_Student_Transfer where AppNo='" + app_no + "' and TransferDate='" + transferDate + "' and FromDegree='" + olddeg + "' and FromSection='" + oldsec + "' and FromCollege='" + oldcolg + "' and FromSeatType='" + fstSeat + "'");
                if (StudPK != "0")
                {
                    string insStudDetails = " insert into st_student_transfer_details(studentTransferfK,old_rollno,Old_RegNo,Old_RollAdmit,stud_admDate,Old_ReceiptNo,Old_ReceiptDate,Old_Amt,New_ReceiptNo,New_ReceiptDate,New_Amt,New_ExcessAmt) values('" + StudPK + "','" + oldRoll + "','" + oldReg + "','" + oldRollAdmit + "','" + studAdmDate + "','" + Convert.ToString(sbOldRecptCode) + "','" + Convert.ToString(sbOldRecptDate) + "','" + oldAmt + "','" + Convert.ToString(sbNewRecptCode) + "','" + Convert.ToString(sbNewRecptDate) + "','" + newAmt + "','" + newExcessAmt + "')";
                    int inss = d2.update_method_wo_parameter(insStudDetails, "Text");
                    save = true;
                }
            }
        }
        catch { }
        return save;
    }

    protected bool checkAdvanceAmt()
    {
        bool boolCheck = false;
        try
        {
            string getExcess = Label9.Text.Trim().Split('.')[1];
            double excesAmt = 0; double.TryParse(getExcess, out excesAmt);
            if (excesAmt != 0)
            {
                string header = string.Empty;
                string ledger = string.Empty;
                if (ddlMainJrHed.Items.Count > 0)
                    header = Convert.ToString(ddlMainJrHed.SelectedValue);
                if (ddlMainJrLed.Items.Count > 0)
                    ledger = Convert.ToString(ddlMainJrLed.SelectedValue);
                if (!string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(ledger))
                    boolCheck = true;
            }
            else
                boolCheck = true;
        }
        catch { }
        return boolCheck;
    }


    //include journal
    public void transferReceipt(string dupReceipt, string AppNo, string collegecode1, string recptDt, string recptNo)
    {
        //PAVAI College and School

        // FpSpread1.SaveChanges();
        try
        {

            string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
            DataSet dsPri = new DataSet();
            dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
            if (dsPri.Tables.Count > 0 && dsPri.Tables[0].Rows.Count > 0)
            {
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                //  finYearid = Convert.ToString(ddlfinyear.SelectedItem.Value);
                byte ColName = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                //Document Settings

                bool createPDFOK = false;

                Div3.InnerHtml = "";
                StringBuilder sbHtml = new StringBuilder();
                //  string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + usercode + "'").Trim();
                string collname = d2.GetFunction("select collname from collinfo where college_code ='" + collegecode1 + "'");
                int heightvar = 0;
                //for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                //{
                sbHtml.Clear();
                //byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                //if (check == 1)
                //{

                int officeCopyHeight = 0;
                //if (heightvar != 0)
                //{
                //    officeCopyHeight = heightvar+250;
                //}
                StringBuilder sbHtmlCopy = new StringBuilder();
                //string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                //string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

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
                        string batch_year = string.Empty;

                        string app_formno = string.Empty;
                        string appnoNew = string.Empty;
                        string Regno = string.Empty;
                        string Roll_admit = string.Empty;
                        string section = string.Empty;
                        string currentSem = string.Empty;

                        string batchYrSem = string.Empty;

                        string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                        //string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

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

                        if (!rb_Journal.Checked)
                        {
                            if (ddladmis.SelectedIndex == 1)
                            {
                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name,r.Roll_admit,r.sections,r.batch_year,r.current_semester  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                            }
                            else
                            {
                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name,app_formno as Roll_admit,'' sections,batch_year,current_Semester  from applyn where app_no='" + AppNo + "'";
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                            {
                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name,r.Roll_admit,r.sections,r.batch_year,r.current_semester  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                            }
                            else
                            {
                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name,app_formno as Roll_admit,'' sections,batch_year,current_Semester  from applyn where app_no='" + AppNo + "'";
                            }
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
                            if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 3 || ddladmis.SelectedIndex == 1)
                            {
                                colquery += " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,'' Sections  from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                            }
                            else
                            {
                                colquery += " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,isnull(r.Sections,'') as Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + " ";
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
                                    if (checkSchoolSetting() == 0)
                                    {
                                        classdisplay = "Class Name ";
                                        TermOrSem = "Term";
                                    }
                                    else
                                    {
                                        classdisplay = "Dept Name ";
                                        TermOrSem = "Semester";
                                    }
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
                                    if (checkSchoolSetting() == 0)
                                    {
                                        strMem = "Admission No";
                                    }
                                    else
                                    {
                                        strMem = rbl_rerollno.SelectedItem.Text.Trim();
                                        if (Convert.ToInt32(rbl_rerollno.SelectedValue) == 0)
                                        {
                                            Roll_admit = rollno;
                                        }
                                        else if (Convert.ToInt32(rbl_rerollno.SelectedValue) == 1)
                                        {
                                            Roll_admit = Regno;
                                        }
                                        else if (Convert.ToInt32(rbl_rerollno.SelectedValue) == 2)
                                        {
                                            //Roll_admit = Roll_admit;
                                        }
                                        else if (Convert.ToInt32(rbl_rerollno.SelectedValue) == 3)
                                        {
                                            Roll_admit = app_formno;
                                        }
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

                            degString = deg;//.Split('-')[0].ToUpper();


                            string[] className = degString.Split('-');
                            if (className.Length > 1)
                            {
                                degString = className[1];
                            }
                            //string entryUserCode = d2.GetFunction("select distinct entryusercode from ft_findailytransaction where app_no='" + AppNo + "'");//commented by saranya on 28/12/2017
                            string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + userCode + "'").Trim();//entryUserCode is modified as usercode
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
                            sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + "  </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + " </td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>:" + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermOrSem + " </td><td style='width:160px; '>: " + currentSem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");


                            sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + " </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermOrSem + " </td><td style='width:160px; '>: " + currentSem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                            #endregion

                            #region Receipt Body

                            sbHtml.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                            sbHtmlCopy.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                            string selectQuery = "";

                            int sno = 0;
                            int indx = 0;
                            double totalamt = 0;
                            double balanamt = 0;
                            double curpaid = 0;
                            // double paidamount = 0;


                            string selHeadersQ = string.Empty;
                            DataSet dsHeaders = new DataSet();


                            //New
                            if (!rb_Journal.Checked)//changed by sudhagar 12.08.2017 for transfer and journal receipt same process
                            {
                                selHeadersQ = " select SUM(Credit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3' and istransfer='1' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(Credit,'0'))>0 and sum(isnull(debit,'0'))=0 ";//,A.Feeallotpk
                            }
                            else
                            {
                                selHeadersQ = " select SUM(Credit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3' and isnull(IsTransfer,'0')='0' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(Credit,'0'))>0 and sum(isnull(debit,'0'))=0 ";//,A.Feeallotpk
                            }

                            //else
                            // {
                            // selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk,H.HeaderName having sum(isnull(debit,'0'))>0 and sum(isnull(credit,'0'))=0";
                            // }

                            selHeadersQ += "  select distinct r.college_code,f.feecategory,r.degree_code,isnull(monthcode,'0')as monthcode,MonthYear from Fee_degree_match fd,registration r,FT_FinDailyTransaction f  where fd.college_code=r.college_code and f.app_no=r.app_no and f.feecategory=fd.feecategory and r.degree_code=fd.degree_code and r.college_code='" + collegecode1 + "' and r.App_No ='" + AppNo + "'";

                            //fine amount added by sudhagar 31.01.2017
                            selHeadersQ += " select SUM(debit) as TakenAmt,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,h.headername  from FT_FinDailyTransaction d,fm_headermaster h  where d.headerfk=h.headerpk and  d.transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and finefeecategory='-1'  group by D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk ,h.headername";
                            //New End
                            if (!rb_Journal.Checked || rb_Journal.Checked)
                            {
                                selHeadersQ += " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3' and isnull(IsTransfer,'0')='0' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(debit,'0'))>0 and sum(isnull(credit,'0'))=0";//,A.Feeallotpk
                            }
                            DataView dv = new DataView();
                            if (selHeadersQ != string.Empty)
                            {
                                string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                dsHeaders.Clear();
                                dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");
                                string modeMulti = string.Empty;
                                bool multiCash = false;
                                bool multiChk = false;
                                bool multiDD = false;
                                bool multiCard = false;

                                if (dsHeaders.Tables.Count > 0)
                                {
                                    if (dsHeaders.Tables[0].Rows.Count > 0)
                                    {
                                        Hashtable htHdrAmt = new Hashtable();
                                        Hashtable htHdrName = new Hashtable();
                                        // Hashtable htfeecat = new Hashtable();
                                        int ledgCnt = 0;
                                        Dictionary<string, string> htfeecat = new Dictionary<string, string>();
                                        Dictionary<string, double> htfeeAmt = new Dictionary<string, double>();
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
                                            string feecode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                            string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                            string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);

                                            string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                            paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                            #region Monthwise
                                            string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                            string FeeAllotPk = string.Empty;
                                            //string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
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
                                            if (!rb_Journal.Checked || rb_Journal.Checked)
                                            {
                                                if (disphdr != "")
                                                    disphdr += "-" + "(DR_J)";
                                            }
                                            else
                                            {
                                                if (disphdr != "")
                                                    disphdr += "-" + "(CR_J)";
                                            }
                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                            //officeCopyHeight -= 20;
                                            ledgCnt++;
                                        }

                                        if (BalanceType == 1)
                                        {
                                            balanamt = retBalance(appnoNew);
                                        }

                                        #region DD Narration


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
                                                        ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + "\nCard :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                        sn++;
                                                        continue;
                                                    }

                                                    ddnar += "\n" + strMode + " No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
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
                                        sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                        //  sbHtml.Append("</table></div><br>");

                                        sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                        //debit
                                        curpaid = 0;
                                        try
                                        {
                                            if (dsHeaders.Tables[3].Rows.Count > 0)
                                            {
                                                for (int head = 0; head < dsHeaders.Tables[3].Rows.Count; head++)
                                                {
                                                    string disphdr = string.Empty;
                                                    double allotamt0 = 0;
                                                    double deductAmt0 = 0;
                                                    double totalAmt0 = 0;
                                                    double paidAmt0 = 0;
                                                    double balAmt0 = 0;
                                                    double creditAmt0 = 0;

                                                    creditAmt0 = Convert.ToDouble(dsHeaders.Tables[3].Rows[head]["TakenAmt"]);
                                                    totalAmt0 = Convert.ToDouble(dsHeaders.Tables[3].Rows[head]["TotalAmount"]);
                                                    //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);

                                                    //paidAmt0 = totalAmt0 - balAmt0;
                                                    deductAmt0 = Convert.ToDouble(dsHeaders.Tables[3].Rows[head]["DeductAmout"]);
                                                    disphdr = Convert.ToString(dsHeaders.Tables[3].Rows[head]["DispName"]);
                                                    string feecatcode = Convert.ToString(dsHeaders.Tables[3].Rows[head]["FeeCategory"]);
                                                    string feecode = Convert.ToString(dsHeaders.Tables[3].Rows[head]["FeeCategory"]);
                                                    string ledgFK = Convert.ToString(dsHeaders.Tables[3].Rows[head]["LedgerFK"]);
                                                    string hdrFK = Convert.ToString(dsHeaders.Tables[3].Rows[head]["headerfk"]);

                                                    string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                    paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                    #region Monthwise
                                                    string DailyTransPk = Convert.ToString(dsHeaders.Tables[3].Rows[head]["DailyTransPk"]);
                                                    string FeeAllotPk = string.Empty;
                                                    // string FeeAllotPk = Convert.ToString(dsHeaders.Tables[3].Rows[head]["FeeAllotPk"]);
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
                                                    if (disphdr != "")
                                                        disphdr += "-" + "(CR_J)";
                                                    sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                    sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                                    //officeCopyHeight -= 20;
                                                    // ledgCnt++;
                                                }
                                            }
                                        }
                                        catch { }

                                        if (curpaid != 0)
                                        {
                                            if (BalanceType == 1)
                                            {
                                                balanamt = retBalance(appnoNew);
                                            }

                                            #region DD Narration
                                            modeMulti = string.Empty;
                                            multiCash = false;
                                            multiChk = false;
                                            multiDD = false;
                                            multiCard = false;

                                            dtMulBnkDetails = new DataSet();
                                            dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD'  else 'Card' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3,6) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

                                            ddnar = string.Empty;
                                            remarks = string.Empty;
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
                                                            ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + "\nCard :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                            sn++;
                                                            continue;
                                                        }

                                                        ddnar += "\n" + strMode + " No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
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


                                            totalamount = curpaid;
                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:55px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;' colspan='3'>Verified by <span style='padding-left:200px;'>Student copy</span><span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                                        }

                                        sbHtml.Append("</table></div><br>");

                                        if (curpaid != 0)
                                        {
                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                                        }


                                        //debit amount


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
                                    }
                                }
                            }
                            sbHtml.Append((studOffiCopy == 1 ? sbHtmlCopy.ToString() : string.Empty) + "</td></tr></table></div>");
                            #endregion

                            Div3.InnerHtml += sbHtml.ToString();

                        }
                        catch (Exception ex)
                        {
                            d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate");
                        }
                        finally
                        {
                        }
                        createPDFOK = true;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Records Found')", true);
                    }
                }
                //    }
                //}
                        #endregion
                #region To print the Receipt
                if (createPDFOK)
                {
                    #region New Print
                    //Div3.InnerHtml += sbHtml.ToString();
                    Div3.Visible = true;

                    ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);

                    #endregion
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Receipt Cannot Be Generated')", true);
                }
                #endregion
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Add Print Settings')", true);
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate");
        }

    }

    public void transferReceiptJournal(string dupReceipt, string AppNo, string collegecode1, string recptDt, string recptNo)
    {
        //PAVAI College and School
        int isMemType = 0;
        // FpSpread1.SaveChanges();
        try
        {

            string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
            DataSet dsPri = new DataSet();
            dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
            if (dsPri.Tables.Count > 0 && dsPri.Tables[0].Rows.Count > 0)
            {
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                //  finYearid = Convert.ToString(ddlfinyear.SelectedItem.Value);
                byte ColName = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                //Document Settings

                bool createPDFOK = false;

                Div3.InnerHtml = "";
                StringBuilder sbHtml = new StringBuilder();
                //  string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + usercode + "'").Trim();
                string collname = d2.GetFunction("select collname from collinfo where college_code ='" + collegecode1 + "'");
                int heightvar = 0;

                //for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                //{
                sbHtml.Clear();
                //byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                //if (check == 1)
                //{

                int officeCopyHeight = 0;
                //if (heightvar != 0)
                //{
                //    officeCopyHeight = heightvar+250;
                //}
                StringBuilder sbHtmlCopy = new StringBuilder();
                //string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                //string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Tag);

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
                        string batch_year = string.Empty;

                        string app_formno = string.Empty;
                        string appnoNew = string.Empty;
                        string Regno = string.Empty;
                        string Roll_admit = string.Empty;
                        string section = string.Empty;
                        string currentSem = string.Empty;
                        string batchYrSem = string.Empty;

                        //Staff
                        string staffcode = string.Empty;
                        string staffname = string.Empty;
                        string deptname = string.Empty;
                        string deptcode = string.Empty;
                        string coll_name = string.Empty;

                        //Others
                        string Vendor_Code = string.Empty;
                        string Vendor_Name = string.Empty;
                        string Vendor_Address = string.Empty;
                        string Vendor_MobileNo = string.Empty;
                        string Vendor_CompName = string.Empty;


                        string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                        //string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

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

                        string queryRollApp = string.Empty;


                        if (!rb_Journal.Checked)
                        {
                            if (ddladmis.SelectedIndex == 1)
                            {
                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name,r.Roll_admit,r.sections,r.batch_year,r.current_semester  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                            }
                            else
                            {
                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name,app_formno as Roll_admit,'' sections,batch_year,current_Semester  from applyn where app_no='" + AppNo + "'";
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                            {
                                //Added by saranya on 08/12/2017
                                //isMemType=1(Student) && isMemType=2(Staff) && isMemType=3(Vendor) && isMemType=4(others)

                                if (rbl_rollnoNew.Text == "Student")
                                {
                                    queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name,r.Roll_admit,r.sections,r.batch_year,r.current_semester  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                                    isMemType = 1;
                                }
                                if (rbl_rollnoNew.Text == "Staff")
                                {
                                    queryRollApp = " select s.staff_code,s.staff_name,h.dept_name,h.dept_code,c.collname  from collinfo c,staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code and a.appl_id ='" + AppNo + "' ";
                                    isMemType = 2;

                                }
                                if (rbl_rollnoNew.Text == "Vendor")
                                {
                                    queryRollApp = " select VendorCode,VendorName,VendorAddress,VendorMobileNo,VendorCompName from co_vendormaster where VendorPk='" + AppNo + "' and VendorType=1 ";
                                    isMemType = 3;
                                }
                                if (rbl_rollnoNew.Text == "Others")
                                {
                                    queryRollApp = " select VendorCode,VendorName,VendorAddress,VendorMobileNo,VendorCompName from co_vendormaster where VendorPk='" + AppNo + "' and VendorType=-5 ";
                                    isMemType = 4;
                                }
                            }
                            else
                            {
                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name,app_formno as Roll_admit,'' sections,batch_year,current_Semester  from applyn where app_no='" + AppNo + "'";
                            }
                        }
                        DataSet dsRollApp = new DataSet();
                        dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");

                        if (dsRollApp.Tables.Count > 0)
                        {
                            if (dsRollApp.Tables[0].Rows.Count > 0)
                            {
                                if (isMemType == 1)
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
                                //================Added By Saranya(09Dec2017)=========================//
                                if (isMemType == 2)
                                {
                                    staffcode = Convert.ToString(dsRollApp.Tables[0].Rows[0]["staff_code"]);
                                    staffname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["staff_name"]);
                                    deptname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["dept_name"]);
                                    deptcode = Convert.ToString(dsRollApp.Tables[0].Rows[0]["dept_code"]);
                                    coll_name = Convert.ToString(dsRollApp.Tables[0].Rows[0]["collname"]);
                                }
                                if (isMemType == 3)
                                {
                                    Vendor_Code = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorCode"]);
                                    Vendor_Name = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorName"]);
                                    Vendor_Address = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorAddress"]);
                                    Vendor_MobileNo = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorMobileNo"]);
                                    Vendor_CompName = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorCompName"]);
                                }
                                if (isMemType == 4)
                                {
                                    Vendor_Code = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorCode"]);
                                    Vendor_Name = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorName"]);
                                    Vendor_Address = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorAddress"]);
                                    Vendor_MobileNo = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorMobileNo"]);
                                    Vendor_CompName = Convert.ToString(dsRollApp.Tables[0].Rows[0]["VendorCompName"]);
                                }
                                //===================================================//

                            }
                            else
                                appnoNew = AppNo;
                        }
                        else
                            appnoNew = AppNo;
                        //name = rollno + "-" + studname;

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

                            string colquery = string.Empty;
                            if (isMemType == 1)
                            {
                                colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " ";
                                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 3 || ddladmis.SelectedIndex == 1)
                                {
                                    colquery += " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,'' Sections  from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                }
                                else
                                {
                                    colquery += " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,isnull(r.Sections,'') as Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + " ";
                                }
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
                            string classdisplay = string.Empty;//"Class Name "
                            string rollDisplay = string.Empty;

                            if (isMemType == 1)
                            {
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
                                        if (checkSchoolSetting() == 0)
                                        {
                                            classdisplay = "Class Name ";
                                            TermOrSem = "Term";
                                        }
                                        else
                                        {
                                            classdisplay = "Dept Name ";
                                            TermOrSem = "Semester";
                                        }
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
                                        if (checkSchoolSetting() == 0)
                                        {
                                            strMem = "Admission No";
                                        }
                                        else
                                        {
                                            strMem = rbl_rerollno.SelectedItem.Text.Trim();

                                            if (Convert.ToInt32(rbl_rerollno.SelectedValue) == 0)
                                            {
                                                Roll_admit = rollno;
                                            }
                                            else if (Convert.ToInt32(rbl_rerollno.SelectedValue) == 1)
                                            {
                                                Roll_admit = Regno;
                                            }
                                            else if (Convert.ToInt32(rbl_rerollno.SelectedValue) == 2)
                                            {
                                                //Roll_admit = Roll_admit;
                                            }
                                            else if (Convert.ToInt32(rbl_rerollno.SelectedValue) == 3)
                                            {
                                                Roll_admit = app_formno;
                                            }
                                        }
                                    }
                                }
                            }

                            //===========================Added By Saranya(09Dec2017)====================//
                            if (isMemType == 2)
                            {
                                if (checkSchoolSetting() == 0)
                                {
                                    classdisplay = "Class Name ";
                                    TermOrSem = "Term";
                                }
                                else
                                {
                                    classdisplay = "Dept Name ";
                                    TermOrSem = "Semester";
                                }
                                if (checkSchoolSetting() == 0)
                                {
                                    strMem = "Admission No";
                                }
                                else
                                {
                                    strMem = "Staff Code";
                                    Roll_admit = staffcode;

                                }
                            }
                            if (isMemType == 3)
                            {
                                if (checkSchoolSetting() == 0)
                                {
                                    classdisplay = "Class Name ";
                                    TermOrSem = "Term";
                                }

                                if (checkSchoolSetting() == 0)
                                {
                                    strMem = "Admission No";
                                }
                                else
                                {
                                    strMem = "Vendor Code";
                                    Roll_admit = Vendor_Code;

                                }
                            }
                            if (isMemType == 4)
                            {
                                if (checkSchoolSetting() == 0)
                                {
                                    classdisplay = "Class Name ";
                                    TermOrSem = "Term";
                                }

                                if (checkSchoolSetting() == 0)
                                {
                                    strMem = "Admission No";
                                }
                                else
                                {
                                    strMem = "Vendor Code";
                                    Roll_admit = Vendor_Code;

                                }
                            }
                            //=====================================================//

                            string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                            try
                            {
                                acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                            }
                            catch { }

                            #endregion

                            string degString = string.Empty;
                            //Line3
                            degString = deg;//.Split('-')[0].ToUpper();

                            string[] className = degString.Split('-');
                            if (className.Length > 1)
                            {
                                degString = className[1];
                            }
                            //string entryUserCode = d2.GetFunction("select distinct entryusercode from ft_findailytransaction where app_no='" + AppNo + "'");//commented by Saranya on 28/12/2014
                            string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + userCode + "'").Trim();//entryUserCode is modified to userCode
                            #region Receipt Header

                            //  sbHtml.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");

                            sbHtml.Append("<div style='height: 560px;width:795px; border:0px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");
                            //sbHtml.Append("<div style=' width:790px; height:30px;border:0px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");

                            sbHtmlCopy.Append("<div style=' width:790px; height:0px;border:0px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");
                            sbHtmlCopy.Append("<div style=' width:790px; height:#officeCopyHeight#px;'></div>");
                            //sbHtmlCopy.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");
                            //sbHtmlCopy.Append("<div style='height:#officeCopyHeight#px; width:790px;'></div>");
                            if (ColName == 1)
                            {
                                sbHtml.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                                sbHtml.Append("<br/>");

                                sbHtmlCopy.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                                sbHtmlCopy.Append("<br/>");
                            }

                            if (isMemType == 1)
                            {
                                sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + "  </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + " </td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>:" + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermOrSem + " </td><td style='width:160px; '>: " + currentSem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");
                                sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + " </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermOrSem + " </td><td style='width:160px; '>: " + currentSem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");
                            }
                            //============================Added By Saranya(09Dec2017)=====================//
                            if (isMemType == 2)
                            {

                                sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + "  </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + " </td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>:" + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + staffname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + deptname + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                                sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + " </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + staffname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + deptname + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");
                            }
                            if (isMemType == 3)
                            {

                                sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + "  </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + " </td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>:" + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + Vendor_Name.ToUpper() + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                                sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + " </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + Vendor_Name.ToUpper() + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");
                            }
                            if (isMemType == 4)
                            {

                                sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + "  </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + " </td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>:" + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + Vendor_CompName.ToUpper() + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                                sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + " </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + Vendor_CompName.ToUpper() + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");
                                //===========================================//
                            }
                            #endregion

                            #region Receipt Body

                            sbHtml.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                            sbHtmlCopy.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");



                            string selectQuery = "";

                            int sno = 0;
                            int indx = 0;
                            double totalamt = 0;
                            double balanamt = 0;
                            double curpaid = 0;
                            double amount = 0;
                            // double paidamount = 0;


                            string selHeadersQ = string.Empty;
                            DataSet dsHeaders = new DataSet();


                            //New

                            if (rb_Journal.Checked && ddlJournalType.SelectedIndex == 1)//changed by sudhagar 12.08.2017 for transfer and journal receipt same process
                            {
                                if (isMemType == 1)
                                {
                                    selHeadersQ = " select SUM(Credit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3'  group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(Credit,'0'))>0 and sum(isnull(debit,'0'))=0 ";//,A.Feeallotpk
                                }
                                //=============Added By Saranya(09Dec2017) for Staff journal print===============//
                                if (isMemType == 2)
                                {
                                    selHeadersQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,f.FeeCategory,''DeductAmout,isnull(f.debit,0) as Amount,isnull(f.debit,0) as BalAmount,(select distinct totalamount from ft_feeallot fa where fa.headerfk=f.headerfk and fa.ledgerfk=f.ledgerfk and fa.feecategory=f.feecategory and fa.App_No=" + AppNo + " and isnull(IsTransfer,'0')='0') as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),transdate,103) as excesstransdate,transcode as dailytranscode,narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,staff_appl_master s where s.appl_id=f.App_No  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK  and memtype='2' and transcode='" + recptNo + "' and  s.appl_id='" + AppNo + "'  and isnull(f.debit,'0')>0  and isnull(paid_Istransfer,'0')='0'  order by isnull(l.priority,1000), l.ledgerName asc,f.FeeCategory";//" + paidVal + " " + Transrcpt + " 
                                }
                                //=============================//

                            }
                            if (ddlJournalType.SelectedIndex == 3)
                            {
                                foreach (GridViewRow gdrow in othervendor.Rows)
                                {
                                    CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
                                    if (cb.Checked)
                                    {
                                        TextBox code = (TextBox)gdrow.FindControl("txt_vendorcode");
                                        string vendorappno = d2.GetFunction("select vendorpk from co_vendormaster where vendorcode='" + code.Text + "'");
                                        selHeadersQ = "select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,f.FeeCategory,''DeductAmout,isnull(f.Credit,0) as amount,isnull(f.debit,0) as BalAmount,(select distinct totalamount from ft_feeallot fa where fa.headerfk=f.headerfk and fa.ledgerfk=f.ledgerfk and fa.feecategory=f.feecategory and fa.App_No=" + AppNo + " and isnull(IsTransfer,'0')='0') as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),transdate,103) as excesstransdate,transcode as dailytranscode,narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,co_vendormaster co where co.VendorPK=f.App_No  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK  and memtype='4' and app_no='" + vendorappno + "'";
                                    }

                                }


                            }
                            //if (ddlJournalType.SelectedIndex == 3)
                            //{
                            //    selHeadersQ = "select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,''DeductAmout,isnull(f.debit,0) as Amount,isnull(f.debit,0) as BalAmount,f.transcode as dailytranscode, narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,co_vendormaster V where v.vendorpk=f.app_no  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK and f.memtype='4' and f.transcode='" + recptNo + "' and  f.app_no='" + AppNo + "'  and isnull(f.debit,'0')>0 order by isnull(l.priority,1000), l.ledgerName";
                            //}

                            if (ddlJournalType.SelectedIndex == 0 || ddlJournalType.SelectedIndex == 2)//advance and excess amount
                            {

                                selHeadersQ = " select distinct SUM(Credit) as TakenAmt,SUM(exl.excessamt) as FeeAmount, isnull(sum(exl.excessamt),0)-isnull(sum(exl.adjamt),0) as BalAmount,'0' as DeductAmout,isnull(sum(exl.excessamt),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,ft_excessdet ex,ft_excessledgerdet exl  where d.HeaderFK =h.HeaderPK   and D.LedgerFK=l.LedgerPK and ex.app_no=d.app_no  and exl.headerfk=d.headerfk and exl.ledgerfk=d.ledgerfk and exl.feecategory=d.feecategory and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk  and ex.feecategory=exl.feecategory and ex.feecategory = d.feecategory and ex.excessdetpk=exl.excessdetfk and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(Credit,'0'))>0 and sum(isnull(debit,'0'))=0";//and ex_journalentry='1' Modified by saranya on 17/04/2018

                                //  selHeadersQ = " select SUM(Credit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3' and istransfer='0' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(Credit,'0'))>0 and sum(isnull(debit,'0'))=0 ";//,A.Feeallotpk
                            }

                            //else
                            // {
                            // selHeadersQ = " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,A.Feeallotpk,H.HeaderName having sum(isnull(debit,'0'))>0 and sum(isnull(credit,'0'))=0";
                            // }

                            //=============Added By Saranya (09Dec2017) for vendor and others journal print========================//

                            if (rb_Journal.Checked)
                            {
                                if (isMemType == 3)
                                {
                                    selHeadersQ = "select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,''DeductAmout,isnull(f.credit,0) as Amount,isnull(f.credit,0) as BalAmount,f.transcode as dailytranscode, narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,co_vendormaster V where v.vendorpk=f.app_no  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK and f.memtype='3' and f.transcode='" + recptNo + "' and  f.app_no='" + AppNo + "'  and isnull(f.credit,'0')>0 order by isnull(l.priority,1000), l.ledgerName";

                                }
                                if (isMemType == 4)
                                {
                                    selHeadersQ = "select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,''DeductAmout,isnull(f.debit,0) as Amount,isnull(f.debit,0) as BalAmount,f.transcode as dailytranscode, narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,co_vendormaster V where v.vendorpk=f.app_no  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK and f.memtype='4' and f.transcode='" + recptNo + "' and  f.app_no='" + AppNo + "'  and isnull(f.debit,'0')>0 order by isnull(l.priority,1000), l.ledgerName";

                                }
                            }
                            //=========================================================//

                            //student

                            selHeadersQ += "  select distinct r.college_code,f.feecategory,r.degree_code,isnull(monthcode,'0')as monthcode,MonthYear from Fee_degree_match fd,registration r,FT_FinDailyTransaction f  where fd.college_code=r.college_code and f.app_no=r.app_no and f.feecategory=fd.feecategory and r.degree_code=fd.degree_code and r.college_code='" + collegecode1 + "' and r.App_No ='" + AppNo + "'";

                            //fine amount added by sudhagar 31.01.2017
                            selHeadersQ += " select SUM(debit) as TakenAmt,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,h.headername  from FT_FinDailyTransaction d,fm_headermaster h  where d.headerfk=h.headerpk and  d.transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and finefeecategory='-1'  group by D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk ,h.headername";
                            //New End
                            if (!rb_Journal.Checked || rb_Journal.Checked)
                            {
                                selHeadersQ += " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3' and isnull(IsTransfer,'0')='0' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(debit,'0'))>0 and sum(isnull(credit,'0'))=0";//,A.Feeallotpk

                            }
                            DataView dv = new DataView();
                            if (selHeadersQ != string.Empty)
                            {
                                string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                dsHeaders.Clear();
                                dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                if (dsHeaders.Tables.Count > 0)
                                {
                                    if (dsHeaders.Tables[0].Rows.Count > 0)
                                    {

                                        Hashtable htHdrAmt = new Hashtable();
                                        Hashtable htHdrName = new Hashtable();
                                        // Hashtable htfeecat = new Hashtable();
                                        int ledgCnt = 0;
                                        Dictionary<string, string> htfeecat = new Dictionary<string, string>();
                                        Dictionary<string, double> htfeeAmt = new Dictionary<string, double>();

                                        #region Student Journal Print

                                        if (isMemType == 1)
                                        {
                                            string modeMulti = string.Empty;
                                            bool multiCash = false;
                                            bool multiChk = false;
                                            bool multiDD = false;
                                            bool multiCard = false;
                                            DataSet dtMulBnkDetails = new DataSet();
                                            string ddnar = string.Empty;
                                            string remarks = string.Empty;
                                            double totalamount = 0;
                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                            {


                                                string disphdr = string.Empty;
                                                double allotamt0 = 0;
                                                double deductAmt0 = 0;
                                                double totalAmt0 = 0;
                                                double paidAmt0 = 0;
                                                double balAmt0 = 0;
                                                double amount0 = 0;
                                                double creditAmt0 = 0;
                                                string feecatcode = string.Empty;
                                                string feecode = string.Empty;
                                                string ledgFK = string.Empty;
                                                string hdrFK = string.Empty;

                                                //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);
                                                if (ddlJournalType.SelectedIndex == 1 || ddlJournalType.SelectedIndex == 2 || ddlJournalType.SelectedIndex == 0)
                                                {
                                                    //paidAmt0 = totalAmt0 - balAmt0;
                                                    creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                                    totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                    deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                    disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                    feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                    feecode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                    ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                    hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);



                                                    string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                    paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));


                                                    #region Monthwise
                                                    string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                    string FeeAllotPk = string.Empty;
                                                    //string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
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

                                                    if (!rb_Journal.Checked || rb_Journal.Checked)
                                                    {
                                                        if (disphdr != "")
                                                            disphdr += "-" + "(DR_J)";
                                                    }
                                                    else
                                                    {
                                                        if (disphdr != "")
                                                            disphdr += "-" + "(CR_J)";
                                                    }
                                                    sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                    sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                                    //officeCopyHeight -= 20;
                                                    ledgCnt++;


                                                    if (BalanceType == 1)
                                                    {
                                                        balanamt = retBalance(appnoNew);
                                                    }

                                                    #region DD Narration



                                                    dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD'  else 'Card' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3,6) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

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
                                                                    ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + "\nCard :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                                    sn++;
                                                                    continue;
                                                                }

                                                                ddnar += "\n" + strMode + " No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
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
                                                            ddnar += " Excess/Advance Amount Rs. : " + excessRemaining(appnoNew);

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

                                                    totalamount = curpaid;
                                                    sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                    //  sbHtml.Append("</table></div><br>");

                                                    sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                }

                                                else
                                                {
                                                    balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);
                                                    amount0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["Amount"]);
                                                    disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["HeaderName"]);
                                                    ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                    hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);

                                                    string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + AppNo + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                    paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));
                                                    sno++;
                                                    amount += Convert.ToDouble(amount0);
                                                    indx++;
                                                    createPDFOK = true;
                                                    if (!rb_Journal.Checked || rb_Journal.Checked)
                                                    {
                                                        if (disphdr != "")
                                                            disphdr += "-" + "(DR_J)";
                                                    }
                                                    else
                                                    {
                                                        if (disphdr != "")
                                                            disphdr += "-" + "(CR_J)";
                                                    }
                                                    sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(balAmt0) + "." + returnDecimalPart(balAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                    sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(balAmt0) + "." + returnDecimalPart(balAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                                    officeCopyHeight -= 20;
                                                    ledgCnt++;


                                                    curpaid += Convert.ToDouble(balAmt0);
                                                    totalamount = curpaid;

                                                    //sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(balAmt0) + "." + returnDecimalPart(balAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                    ////  sbHtml.Append("</table></div><br>");

                                                    //sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(balAmt0) + "." + returnDecimalPart(balAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                    //sbHtml.Append("<tr><td colspan='3'></td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:55px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;' colspan='3'>Verified by <span style='padding-left:200px;'>Vendor copy</span><span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                                                    //sbHtmlCopy.Append("<tr><td colspan='3'></td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                                                }

                                            }

                                            //debit
                                            curpaid = 0;
                                            try
                                            {
                                                if (dsHeaders.Tables[3].Rows.Count > 0)
                                                {
                                                    for (int head = 0; head < dsHeaders.Tables[3].Rows.Count; head++)
                                                    {
                                                        string disphdr = string.Empty;
                                                        double allotamt0 = 0;
                                                        double deductAmt0 = 0;
                                                        double totalAmt0 = 0;
                                                        double paidAmt0 = 0;
                                                        double balAmt0 = 0;
                                                        double creditAmt0 = 0;

                                                        creditAmt0 = Convert.ToDouble(dsHeaders.Tables[3].Rows[head]["TakenAmt"]);
                                                        totalAmt0 = Convert.ToDouble(dsHeaders.Tables[3].Rows[head]["TotalAmount"]);
                                                        //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);

                                                        //paidAmt0 = totalAmt0 - balAmt0;
                                                        deductAmt0 = Convert.ToDouble(dsHeaders.Tables[3].Rows[head]["DeductAmout"]);
                                                        disphdr = Convert.ToString(dsHeaders.Tables[3].Rows[head]["DispName"]);
                                                        string feecatcode = Convert.ToString(dsHeaders.Tables[3].Rows[head]["FeeCategory"]);
                                                        string feecode = Convert.ToString(dsHeaders.Tables[3].Rows[head]["FeeCategory"]);
                                                        string ledgFK = Convert.ToString(dsHeaders.Tables[3].Rows[head]["LedgerFK"]);
                                                        string hdrFK = Convert.ToString(dsHeaders.Tables[3].Rows[head]["headerfk"]);

                                                        string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                        paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                        #region Monthwise
                                                        string DailyTransPk = Convert.ToString(dsHeaders.Tables[3].Rows[head]["DailyTransPk"]);
                                                        string FeeAllotPk = string.Empty;
                                                        // string FeeAllotPk = Convert.ToString(dsHeaders.Tables[3].Rows[head]["FeeAllotPk"]);
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
                                                        if (disphdr != "")
                                                            disphdr += "-" + "(CR_J)";
                                                        sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                        sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                                        //officeCopyHeight -= 20;
                                                        ledgCnt++;
                                                    }
                                                }
                                            }
                                            catch { }

                                            if (curpaid != 0)
                                            {
                                                if (BalanceType == 1)
                                                {
                                                    balanamt = retBalance(appnoNew);
                                                }

                                                #region DD Narration
                                                modeMulti = string.Empty;
                                                multiCash = false;
                                                multiChk = false;
                                                multiDD = false;
                                                multiCard = false;

                                                dtMulBnkDetails = new DataSet();
                                                dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD'  else 'Card' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3,6) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

                                                ddnar = string.Empty;
                                                remarks = string.Empty;
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
                                                                ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + "\nCard :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                                sn++;
                                                                continue;
                                                            }

                                                            ddnar += "\n" + strMode + " No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
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
                                                        ddnar += " Excess/Advance Amount Rs. : " + excessRemaining(appnoNew);

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


                                                totalamount = curpaid;
                                                sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:55px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;' colspan='3'>Verified by <span style='padding-left:200px;'>Student copy</span><span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                                            }

                                            sbHtml.Append("</table></div><br>");

                                            if (curpaid != 0)
                                            {
                                                sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                                            }


                                            //debit amount

                                        }



                                        #endregion

                                        #region Staff Journal Print Added By Saranya On 09Dec2017

                                        else if (isMemType == 2)
                                        {
                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                            {
                                                string disphdr = string.Empty;
                                                //double allotamt0 = 0;
                                                double deductAmt0 = 0;
                                                double totalAmt0 = 0;
                                                double paidAmt0 = 0;
                                                double amount0 = 0;
                                                double balAmt0 = 0;
                                                double creditAmt0 = 0;

                                                //creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                                //totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);
                                                amount0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["Amount"]);
                                                //paidAmt0 = totalAmt0 - balAmt0;
                                                //deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["HeaderName"]);
                                                //string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                //string feecode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);

                                                string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + AppNo + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));
                                                sno++;
                                                amount += Convert.ToDouble(amount0);
                                                indx++;
                                                createPDFOK = true;
                                                if (!rb_Journal.Checked || rb_Journal.Checked)
                                                {
                                                    if (disphdr != "")
                                                        disphdr += "-" + "(DR_J)";
                                                }
                                                else
                                                {
                                                    if (disphdr != "")
                                                        disphdr += "-" + "(CR_J)";
                                                }
                                                sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(amount0) + "." + returnDecimalPart(amount0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(amount0) + "." + returnDecimalPart(amount0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                                //officeCopyHeight -= 20;
                                                //ledgCnt++;
                                            }

                                            curpaid += Convert.ToDouble(amount);
                                            double totalamount = curpaid;

                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                            //  sbHtml.Append("</table></div><br>");

                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                            sbHtml.Append("<tr><td colspan='3'></td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:55px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;' colspan='3'>Verified by <span style='padding-left:200px;'>Staff copy</span><span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                                            sbHtmlCopy.Append("<tr><td colspan='3'></td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");


                                        }
                                        #endregion


                                        #region Vendor and Others Journal Print Added By Saranya On 09Dec2017

                                        else if (isMemType == 3 || isMemType == 4)
                                        {
                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                            {
                                                string disphdr = string.Empty;
                                                //double allotamt0 = 0;
                                                double deductAmt0 = 0;
                                                double totalAmt0 = 0;
                                                double paidAmt0 = 0;
                                                double amount0 = 0;
                                                double balAmt0 = 0;
                                                double creditAmt0 = 0;


                                                balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);
                                                amount0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["Amount"]);
                                                disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["HeaderName"]);
                                                string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);

                                                string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + AppNo + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));
                                                sno++;
                                                amount += Convert.ToDouble(amount0);
                                                indx++;
                                                createPDFOK = true;
                                                if (!rb_Journal.Checked || rb_Journal.Checked)
                                                {
                                                    if (disphdr != "")
                                                        disphdr += "-" + "(DR_J)";
                                                }
                                                else
                                                {
                                                    if (disphdr != "")
                                                        disphdr += "-" + "(CR_J)";
                                                }
                                                sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(amount0) + "." + returnDecimalPart(amount0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(amount0) + "." + returnDecimalPart(amount0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                                //officeCopyHeight -= 20;
                                                //ledgCnt++;
                                            }

                                            curpaid += Convert.ToDouble(amount);
                                            double totalamount = curpaid;

                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                            //  sbHtml.Append("</table></div><br>");

                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                            sbHtml.Append("<tr><td colspan='3'></td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:55px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;' colspan='3'>Verified by <span style='padding-left:200px;'>Vendor copy</span><span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                                            sbHtmlCopy.Append("<tr><td colspan='3'></td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                                        }
                                        #endregion




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

                                    }
                                }
                            }
                            sbHtml.Append((studOffiCopy == 1 ? sbHtmlCopy.ToString() : string.Empty) + "</td></tr></table></div>");
                            #endregion

                            Div3.InnerHtml += sbHtml.ToString();
                        #endregion //123

                        }
                        catch (Exception ex)
                        {
                            //d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate");
                        }
                        finally
                        {
                        }
                        createPDFOK = true;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Records Found')", true);
                    }
                }
                //    }
                //}

                #region To print the Receipt
                if (createPDFOK)
                {
                    #region New Print
                    //Div3.InnerHtml += sbHtml.ToString();
                    Div3.Visible = true;
                    //===========Added by Saranya(09Dec2017)=================//  
                    rcptSngleStaff.Visible = false;
                    staffjournal.Visible = false;
                    rcptSngleVendor.Visible = false;
                    lnkothersjournalmap.Visible = false;
                    rcptSngleOthers.Visible = false;
                    lnkvendorjournalmap.Visible = false;
                    //======================================================//
                    ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
                    #endregion
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Receipt Cannot Be Generated')", true);
                }
                #endregion
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Add Print Settings')", true);
            }


        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate");
        }

    }

    //Reusable Methods
    private double retBalance(string appNo)
    {
        double ovBalAMt = 0;
        if (BalanceType == 1)
        {
            double.TryParse(d2.GetFunction(" select sum(isnull(totalAmount,0)-isnull(paidAmount,0)) as BalanceAmt from ft_feeallot where app_no =" + appNo + " and isnull(IsTransfer,'0')='0'"), out ovBalAMt);
        }
        return ovBalAMt;
    }
    private double excessRemaining(string appnoNew)
    {
        string excessamtQ = d2.GetFunction("select sum(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) as BalanceAmt from FT_ExcessDet WHERE  App_No=" + appnoNew + " ");

        double excessamtValue = 0;
        double.TryParse(excessamtQ, out excessamtValue);
        return excessamtValue;
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
            words += " and ";
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

    public string generateBarcode(string barCode)
    {
        string urlImg = Server.MapPath("~/BarCode/" + "barcodeimg" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".Jpeg");
        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
        using (Bitmap bitMap = new Bitmap(barCode.Length * 10, 20))
        {
            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                Font oFont = new Font("IDAutomationHC39M", 16);
                PointF point = new PointF(2f, 2f);
                SolidBrush blackBrush = new SolidBrush(Color.Black);
                SolidBrush whiteBrush = new SolidBrush(Color.White);
                graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                graphics.DrawString("*" + barCode + "*", oFont, blackBrush, point);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                //bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //byte[] byteImage = ms.ToArray();

                //Convert.ToBase64String(byteImage);
                //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);


                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitMap.Save(urlImg, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return urlImg;
        }

    }
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }

    #region RecieptNo Generate

    public string generateReceiptNo(string hdrs, ref Dictionary<string, string> dtrcpt, ref Dictionary<string, string> arRcptfk)
    {
        string collegecode1 = string.Empty;
        if (!rb_Journal.Checked)
        {
            if (rbl_AdmitTransfer.SelectedIndex == 0)
                collegecode1 = Convert.ToString(ddlclgapplied.SelectedValue);
            else
                collegecode1 = Convert.ToString(ddl_colg.SelectedValue);
        }
        else
        {
            collegecode1 = Convert.ToString(ddlcollege.SelectedValue);
        }
        int isHeaderwise = 0;
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
            Session["isHeaderwise"] = isHeaderwise;
        }
        catch { isHeaderwise = 0; }
        try
        {
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 5)
                return string.Empty;
        }
        catch { return string.Empty; }
        if (isHeaderwise == 0 || isHeaderwise == 2)
        {
            return getCommonReceiptNo(collegecode1);
        }
        else
        {
            return getHeaderwiseReceiptNo(hdrs, ref dtrcpt, ref arRcptfk, collegecode1);
        }
    }
    private string getCommonReceiptNo(string collegecode1)
    {
        string recno = string.Empty;
        //lblaccid.Text = "";
        //lstrcpt.Text = "";
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            //   lblaccid.Text = accountid;
            string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + "  and isnull(onlineRec,'')<>'1')  and isnull(onlineRec,'')<>'1'";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables.Count > 0 && dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }
                string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;
                Session["acronym"] = recacr;
                int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));
                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }
    private string getHeaderwiseReceiptNo(string hdrs, ref Dictionary<string, string> dtrcpt, ref Dictionary<string, string> arRcptfk, string collegecode1)
    {
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;
            string hdrSetPK = string.Empty;
            DataSet dsFinHedDet = new DataSet();
            DataView dvcode = new DataView();
            string isheaderFk = hdrs;
            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string selQ = string.Empty;
            selQ = "select distinct HeaderSettingFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and HeaderFK in (" + isheaderFk + ") and CollegeCode=" + collegecode1 + " and FinyearFK=" + finYearid + " and isnull(onlineRec,'')<>'1'";
            selQ += "select distinct HeaderSettingFk, headerfk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and HeaderFK in (" + isheaderFk + ") and CollegeCode=" + collegecode1 + " and FinyearFK=" + finYearid + " and isnull(onlineRec,'')<>'1'";
            dsFinHedDet = d2.select_method_wo_parameter(selQ, "Text");
            if (dsFinHedDet.Tables.Count > 0 && dsFinHedDet.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsFinHedDet.Tables[0].Rows.Count; i++)
                {
                    hdrSetPK = Convert.ToString(dsFinHedDet.Tables[0].Rows[i][0]).Trim();
                    string secondreciptqurey = "select * from FM_HeaderFinCodeSettings where HeaderSettingPK =" + Convert.ToString(hdrSetPK) + " and FinyearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1'";
                    DataSet dsrecYr = new DataSet();
                    dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
                    if (dsrecYr.Tables.Count > 0 && dsrecYr.Tables[0].Rows.Count > 0)
                    {
                        recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0]["RcptStNo"]);
                        if (recnoprev != "")
                        {
                            int recno_cur = Convert.ToInt32(recnoprev);
                            receno = recno_cur;
                        }
                        recacr = Convert.ToString(dsrecYr.Tables[0].Rows[0]["RcptAcr"]);

                        int size = Convert.ToInt32(dsrecYr.Tables[0].Rows[0]["Rcptsize"]);

                        string recenoString = receno.ToString();

                        if (size != recenoString.Length && size > recenoString.Length)
                        {
                            while (size != recenoString.Length)
                            {
                                recenoString = "0" + recenoString;
                            }
                        }
                        recno = recacr + recenoString;
                        if (!string.IsNullOrEmpty(recno))
                        {
                            dsFinHedDet.Tables[1].DefaultView.RowFilter = "HeaderSettingFK='" + hdrSetPK + "'";
                            dvcode = dsFinHedDet.Tables[1].DefaultView;
                            if (dvcode.Count > 0)
                            {
                                for (int row = 0; row < dvcode.Count; row++)
                                {
                                    if (!dtrcpt.ContainsKey(Convert.ToString(dvcode[row]["headerfk"])))
                                        dtrcpt.Add(Convert.ToString(dvcode[row]["headerfk"]), Convert.ToString(recno));
                                }
                            }
                        }
                        if (!arRcptfk.ContainsKey(recno))
                            arRcptfk.Add(recno, hdrSetPK + "-" + recacr);
                    }
                }
            }
            if (dtrcpt.Count > 0)
                recno = string.Empty;
            return recno;
        }
        catch (Exception ex) { return recno; }
    }


    #endregion

    #region journal no generate

    public string generateJournalNo(string hdrs, string collegecode1)
    {
        if (!rb_Journal.Checked)
        {
            if (rbl_AdmitTransfer.SelectedIndex == 0)
                collegecode1 = Convert.ToString(ddlclgapplied.SelectedValue);
            else
                collegecode1 = Convert.ToString(ddl_colg.SelectedValue);
        }
        else
        {
            collegecode1 = Convert.ToString(ddlcollege.SelectedValue);
        }
        int isHeaderwise = 0;
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
            Session["isHeaderwise"] = isHeaderwise;
        }
        catch { isHeaderwise = 0; }
        try
        {
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 5)
                return string.Empty;
        }
        catch { return string.Empty; }
        return getCommonJournalNo(collegecode1);
    }
    private string getCommonJournalNo(string collegecode1)
    {
        string recno = string.Empty;
        //lblaccid.Text = "";
        //lstrcpt.Text = "";
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            //   lblaccid.Text = accountid;
            string secondreciptqurey = "SELECT JournalStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables.Count > 0 && dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }
                string acronymquery = d2.GetFunction("SELECT JournalAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + "  and isnull(onlineRec,'')<>'1')  and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;
                Session["acronym"] = recacr;
                int size = Convert.ToInt32(d2.GetFunction("SELECT  JournalSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));
                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }
    #endregion

    //label text changed
    private void setLabelText()
    {
        //string grouporusercode = string.Empty;
        //if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        //{
        //    grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        //}
        //else if (Session["usercode"] != null)
        //{
        //    grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        //}
        //List<Label> lbl = new List<Label>();
        //List<byte> fields = new List<byte>();

        //lbl.Add(lblclg);
        //lbl.Add(lbldeg);
        //lbl.Add(lbldept);
        //lbl.Add(lblsem);
        //fields.Add(0);
        //fields.Add(2);
        //fields.Add(3);
        //fields.Add(4);

        ////
        //lbl.Add(lblclgs);
        //lbl.Add(lbl_str2);
        //lbl.Add(lbldegs);
        //lbl.Add(lbldepts);
        //lbl.Add(lblsems);
        //fields.Add(0);
        //fields.Add(1);
        //fields.Add(2);
        //fields.Add(3);
        //fields.Add(4);
        ////
        //lbl.Add(lblclgss);
        //lbl.Add(lbl_str3);
        //lbl.Add(lbldegss);
        //lbl.Add(lbldeptss);
        //lbl.Add(lblsemss);
        //fields.Add(0);
        //fields.Add(1);
        //fields.Add(2);
        //fields.Add(3);
        //fields.Add(4);

        ////
        //lbl.Add(lblCollege1);
        //lbl.Add(lbl_stream1);
        //lbl.Add(lbl_degree1);
        //lbl.Add(lbl_branch1);
        //lbl.Add(lbl_sem1);
        //fields.Add(0);
        //fields.Add(1);
        //fields.Add(2);
        //fields.Add(3);
        //fields.Add(4);

        ////
        //lbl.Add(lblCollege);
        //lbl.Add(lbl_stream);
        //lbl.Add(lbl_degree);
        //lbl.Add(lbl_branch);
        //lbl.Add(lbl_Sem);
        //fields.Add(0);
        //fields.Add(1);
        //fields.Add(2);
        //fields.Add(3);
        //fields.Add(4);

        ////
        //lbl.Add(lblcoll);
        //lbl.Add(lbl_str4);
        //lbl.Add(lbldegre);
        //lbl.Add(lbldeptms);
        //lbl.Add(lblsemests);
        //fields.Add(0);
        //fields.Add(1);
        //fields.Add(2);
        //fields.Add(3);
        //fields.Add(4);

        //new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    //journal fees setting header and ledger load
    protected void getJournalSettings()
    {
        try
        {
            ddlMainJrHed.Items.Clear();
            ddlMainJrLed.Items.Clear();
            string collgCode = string.Empty;
            if (!rb_Journal.Checked)
            {
                if (rbl_AdmitTransfer.SelectedIndex == 0)
                    collgCode = Convert.ToString(ddlclgapplied.SelectedValue);
                else
                    collgCode = Convert.ToString(ddl_colg.SelectedValue);
            }
            else
            {
                collgCode = Convert.ToString(ddlcollege.SelectedValue);
            }
            string SelQ = " select LinkValue from New_InsSettings where LinkName='JournalFessSettings'  and user_code ='" + usercode + "' and college_code ='" + collgCode + "'";
            string sav01 = Convert.ToString(d2.GetFunction(SelQ));
            if (sav01 != "0")
            {
                loadMainJrHed(collgCode);
                if (ddlMainJrHed.Items.Count > 0)
                {
                    for (int hdri = 0; hdri < ddlMainJrHed.Items.Count; hdri++)
                    {
                        if (Convert.ToString(ddlMainJrHed.Items[hdri].Value) == sav01.Split(',')[0])
                            ddlMainJrHed.SelectedIndex = hdri;
                    }
                    if (ddlMainJrLed.Items.Count > 0)
                    {
                        for (int lgri = 0; lgri < ddlMainJrLed.Items.Count; lgri++)
                        {
                            if (Convert.ToString(ddlMainJrLed.Items[lgri].Value) == sav01.Split(',')[1])
                                ddlMainJrLed.SelectedIndex = lgri;
                        }
                    }
                }
            }
        }
        catch { }
    }
    public void loadMainJrHed(string collgCode)
    {
        try
        {
            ddlMainJrHed.Items.Clear();
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H WHERE CollegeCode = " + collgCode + "";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlMainJrHed.DataSource = ds;
                ddlMainJrHed.DataTextField = "HeaderName";
                ddlMainJrHed.DataValueField = "HeaderPK";
                ddlMainJrHed.DataBind();
                loadMainJrLed(collgCode);
            }
        }
        catch { }
    }
    public void loadMainJrLed(string collgCode)
    {
        try
        {
            ddlMainJrLed.Items.Clear();
            if (ddlMainJrHed.Items.Count > 0)
            {
                string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L WHERE  l.LedgerMode=0   AND L.CollegeCode = " + collgCode + " ";//and L.HeaderFK in (" + Convert.ToString(ddlMainJrHed.SelectedItem.Value) + ")
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlMainJrLed.DataSource = ds;
                    ddlMainJrLed.DataTextField = "LedgerName";
                    ddlMainJrLed.DataValueField = "LedgerPK";
                    ddlMainJrLed.DataBind();
                }
            }
        }
        catch { }
    }

    protected void getJournalChange()
    {
        try
        {
            txt_reamt.Text = "";
            string rollno = Convert.ToString(txt_rerollno.Text);
            string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,r.app_no  from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code and r.college_code='" + ddlcollege.SelectedValue + "' ";
            //and r.Roll_no='" + rollno + "'";
            if (!string.IsNullOrEmpty(rollno))
            {
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                        query = query + "and r.Roll_no='" + rollno + "'  and DelFlag =0 ";
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                        query = query + "and r.Reg_No='" + rollno + "' and  DelFlag =0";
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                        query = query + "and r.Roll_Admit='" + rollno + "' and DelFlag =0 ";

                }
                else
                {
                    query = "select a.batch_year,a.Current_Semester,a.parent_name,a.stud_name,c.type,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type,co.collname,co.college_code,a.app_no from applyn a,Degree d,Department dt,Course c,collinfo co where  a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and co.college_code =d.college_code  and isconfirm ='1' and app_formno = '" + rollno + "' and a.college_code='" + ddlcollege.SelectedValue + "'";//and admission_status =1 and selection_status=1
                }
                DataSet ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    string appNo = string.Empty;
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        // txt_rerollno.Text = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                        txt_rename.Text = ds1.Tables[0].Rows[i]["stud_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_rebatch.Text = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_redegree.Text = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_redept.Text = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        //  txt_resec.Text = ds1.Tables[0].Rows[i]["Sections"].ToString();
                        txt_resem.Text = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        txt_recolg.Text = ds1.Tables[0].Rows[i]["collname"].ToString();
                        txt_restrm.Text = ds1.Tables[0].Rows[i]["type"].ToString(); // jairam
                        Session["clgcode"] = Convert.ToString(ds1.Tables[0].Rows[i]["college_code"]);
                        appNo = Convert.ToString(ds1.Tables[0].Rows[i]["app_no"]);
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                        rollno = d2.GetFunction("select roll_no from registration where reg_no='" + rollno + "' and college_code='" + ddlcollege.SelectedValue + "' ");
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                        rollno = d2.GetFunction("select roll_no from registration where Roll_admit='" + rollno + "' and college_code='" + ddlcollege.SelectedValue + "'");
                    image3.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + rollno;
                    txt_AmtPerc.Text = "";
                    lbladvance.Visible = false;
                    txt_AmtPerc.Enabled = false;
                    tbljournal.Visible = true;
                    string Transrcpt = string.Empty;
                    string getApp = d2.GetFunction("select appno from st_student_transfer s,st_student_transfer_details sd where s.studenttransferpk=sd.studenttransferfk and appno='" + appNo + "'");
                    if (getApp != "0")
                    {
                        //Transrcpt = " and transdate>= ( select transferdate from st_student_transfer s,st_student_transfer_details sd where s.studenttransferpk=sd.studenttransferfk and appno='" + appNo + "') ";//and isnull(transtype,'0')<>'1'
                    }
                    string getAmt = string.Empty;
                    string jourNalType = string.Empty;
                    if (ddlJournalType.SelectedIndex == 0)
                        jourNalType = " and Ex_JournalEntry='1' and excesstype='1'";
                    else
                        jourNalType = " and isnull(Ex_JournalEntry,'0')='0' and excesstype='1'";//modified by saranya on 08/01/2018( Ex_JournalEntry=0 as isnull(Ex_JournalEntry,'0')='0')

                    if (ddlJournalType.SelectedIndex == 0 || ddlJournalType.SelectedIndex == 2)
                    {
                        string excessfk = string.Empty;
                        string excessamtQ = "select excessdetpk from FT_ExcessDet WHERE  App_No=" + appNo + " " + jourNalType + "";
                        DataSet ds = d2.select_method_wo_parameter(excessamtQ, "Text");
                        for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                        {
                            string detfk = Convert.ToString(ds.Tables[0].Rows[s]["excessdetpk"]);
                            if (excessfk == "")
                                excessfk = detfk;
                            else

                                excessfk += "'" + "," + "'" + detfk;
                        }
                        getAmt = "select sum(BalanceAmt ) as BalanceAmt from FT_ExcessLedgerDet   WHERE  ExcessDetFK in('" + excessfk + "')";
                        //getAmt = " select sum(isnull(excessamt,'0'))-sum(isnull(adjamt,'0')) as bal from FT_ExcessDet where App_No='" + appNo + "' " + jourNalType + "";
                    }
                    else
                    {
                        getAmt = " select isnull(sum(debit),'0')-isnull(sum(credit),'0') from ft_findailytransaction  where App_No='" + appNo + "' and isnull(iscanceled,'0')='0' " + Transrcpt + " and isnull(paid_istransfer,'0')='0' and isnull(transtype,'0')='1' ";//and isnull(transtype,'0')='1'  modified by abarna
                    }
                    double getAdvanceAmt = 0;
                    double.TryParse(Convert.ToString(d2.GetFunction(getAmt)), out getAdvanceAmt);
                    txt_AmtPerc.Text = Convert.ToString(getAdvanceAmt);
                    ddl_AmtPerc.Visible = true;
                    txt_AmtPerc.Visible = true;
                    if (rb_ProlongAbsent.Checked)
                        txt_AmtPerc.Visible = false;
                }
                else
                    disTransClear();
            }
            else
                disTransClear();
        }
        catch (Exception ex) { }
    }


    //journal mapping
    protected void lnkJournal_Click(object sender, EventArgs e)
    {
        if (txt_AmtPerc.Text.Trim() != "0" && txt_AmtPerc.Text.Trim() != "")
        {
            divindi.Visible = true;
            btnupdatestaff.Visible = false;
            divind.Visible = true;
            incPaid.Checked = false;
            incSem.Checked = false;
            incSem_Changed(sender, e);
            if (ddlJournalType.SelectedIndex == 0 || ddlJournalType.SelectedIndex == 1 || ddlJournalType.SelectedIndex == 2)
            {
                bindGridJournalAdvance();
            }

            bindGridAllotJournal();
            vendorothersave.Visible = false;
            txtamtind.Text = "";
            // btntransind.Enabled = false;
            inclAddAmt.Checked = false;
            staffadd.Visible = false;
            gd5.Visible = true;
            Label25.Visible = false;
            transCodetext.Visible = false;
            btnAddRow.Visible = false;
            gridView5.Visible = true;
            savebutton.Visible = true;
            inclAddAmt_Changed(sender, e);
            getJournalSettings();
            btntransind.Text = "Save";

            tblSem.Visible = false;
            btnadjust.Visible = false;
            tblJournalSet.Visible = false;
            lbladvancetxt.Text = "New Paid:";
            othervendor.Visible = false;
            div4.Visible = false;
            div5.Visible = true;
            gridView4.Visible = true;

            divtblOne.Visible = true;
            if (ddlJournalType.SelectedIndex == 3)//created by abarna 10.1.2018
            {
                #region Adjust Scholarship Amount
                bindgridscholarship();
                divtblOne.Visible = true;
                othervendor.Visible = true;
                div4.Visible = true;
                gridView4.Visible = false;
                div5.Visible = false;
                Label25.Visible = true;
                transCodetext.Visible = true;
                #endregion

            }

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Advance Amount Not Available')", true);
        }

    }


    public void bindgridjournalother()//added by abarna
    {
        string vendor = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("Vendorname");

        dt.Columns.Add("VendorCode");
        dt.Columns.Add("Companyname");
        dt.Columns.Add("mobile");
        dt.Columns.Add("address1");
        //  dt.Columns.Add("address2");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("FeeAmt");
        dt.Columns.Add("Total");
        dt.Columns.Add("Narration");
        dt.Columns.Add("Debit");
        dt.Columns.Add("paymode");
        string sql = "";
        string transcode = txt_transcode.Text;
        DataRow dr;
        string appno = "";

        if (rbl_rollnoNew.Text == "Others")
        {
            vendor = d2.GetFunction(" select vendorcode from co_vendormaster where vendorname like '" + txtroll_other.Text.Trim() + "%' and VendorType=-5");
            appno = d2.GetFunction(" select VendorPK from co_vendormaster where vendorname like '" + txtroll_other.Text.Trim() + "%' and VendorType=-5");
        }
        if (rbl_rollnoNew.Text == "Vendor")
        {
            vendor = d2.GetFunction(" select vendorcode from co_vendormaster where vendorcompname like '" + txtroll_vendor.Text.Trim() + "%' and VendorType=1");
            appno = d2.GetFunction(" select VendorPK from co_vendormaster where vendorcompname like '" + txtroll_vendor.Text.Trim() + "%' and VendorType=1");
        }
        if (vendor != "")
        {
            if (rbl_rollnoNew.Text == "Others")
            {
                sql = "select VendorName,VendorCode,VendorCompName,VendorAddress,VendorMobileNo from co_vendormaster where vendorname='" + txtroll_other.Text.Trim() + "' and VendorType=-5 and VendorCode='" + vendor + "' ";
                if (transcode == "")
                {

                    sql += "       select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,f.FeeCategory,''DeductAmout,isnull(f.Credit,0) as amount,isnull(f.debit,0) as BalAmount,(select distinct totalamount from ft_feeallot fa where fa.headerfk=f.headerfk and fa.ledgerfk=f.ledgerfk and fa.feecategory=f.feecategory and fa.App_No=" + appno + " and isnull(IsTransfer,'0')='0') as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),transdate,103) as excesstransdate,transcode as dailytranscode,narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,co_vendormaster co where co.VendorPK=f.App_No  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK  and memtype='4'  and  co.VendorPK=" + appno + "";
                }
                else
                {
                    sql += "       select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,f.FeeCategory,''DeductAmout,isnull(f.Credit,0) as amount,isnull(f.debit,0) as BalAmount,(select distinct totalamount from ft_feeallot fa where fa.headerfk=f.headerfk and fa.ledgerfk=f.ledgerfk and fa.feecategory=f.feecategory and fa.App_No=" + appno + " and isnull(IsTransfer,'0')='0') as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),transdate,103) as excesstransdate,transcode as dailytranscode,narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,co_vendormaster co where co.VendorPK=f.App_No  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK  and memtype='4'  and  co.VendorPK=" + appno + " and transcode='" + transcode + "'";
                }

            }
            if (rbl_rollnoNew.Text == "Vendor")
            {
                sql = "select VendorName,VendorCode,VendorCompName,VendorAddress,VendorMobileNo from co_vendormaster where vendorcompname='" + txtroll_vendor.Text.Trim() + "' and VendorType=1 and VendorCode='" + vendor + "'";
                sql += "       select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,f.FeeCategory,''DeductAmout,isnull(f.Credit,0) as amount,isnull(f.debit,0) as BalAmount,(select distinct totalamount from ft_feeallot fa where fa.headerfk=f.headerfk and fa.ledgerfk=f.ledgerfk and fa.feecategory=f.feecategory and fa.App_No=" + appno + " and isnull(IsTransfer,'0')='0') as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),transdate,103) as excesstransdate,transcode as dailytranscode,narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,co_vendormaster co where co.VendorPK=f.App_No  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK  and memtype='3'  and  co.VendorPK=" + appno + "";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                {
                    dr = dt.NewRow();
                    //modified ds.Tables[0].Rows[row] to ds.Tables[0].Rows[0]
                    dr["Sno"] = row + 1;
                    dr["Vendorname"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorName"]);
                    dr["VendorCode"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorCode"]);
                    dr["Companyname"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorCompName"]);
                    dr["mobile"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorMobileNo"]);
                    dr["address1"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorAddress"]);
                    dr["Header"] = Convert.ToString(ds.Tables[1].Rows[row]["HeaderName"]);
                    dr["HeaderFk"] = Convert.ToString(ds.Tables[1].Rows[row]["HeaderFK"]);
                    dr["Ledger"] = Convert.ToString(ds.Tables[1].Rows[row]["LedgerName"]);
                    dr["LedgerFk"] = Convert.ToString(ds.Tables[1].Rows[row]["LedgerFK"]);

                    dr["FeeAmt"] = Convert.ToString(ds.Tables[1].Rows[row]["excesstransdate"]);
                    transcode = Convert.ToString(ds.Tables[1].Rows[row]["dailytranscode"]);
                    dr["Total"] = transcode;
                    dr["Debit"] = Convert.ToString(ds.Tables[1].Rows[row]["amount"]);
                    // dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                    //dr["Paid"] = Convert.ToString(paidAmount - tempPaid);
                    dr["Balance"] = Convert.ToString(ds.Tables[1].Rows[row]["BalAmount"]);

                    dr["Narration"] = Convert.ToString(ds.Tables[1].Rows[row]["Narration"]);
                    dt.Rows.Add(dr);


                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            othervendor.DataSource = dt;
            othervendor.DataBind();

            // Table1.Visible = true;
        }
        else
        {
            othervendor.DataSource = null;
            othervendor.DataBind();
            //Table1.Visible = false;
        }

    }
    protected void txt_recch_Changed(object sender, EventArgs e)
    {
        string vendor = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("Vendorname");

        dt.Columns.Add("VendorCode");
        dt.Columns.Add("Companyname");
        dt.Columns.Add("mobile");
        dt.Columns.Add("address1");
        //  dt.Columns.Add("address2");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("FeeAmt");
        dt.Columns.Add("Total");
        dt.Columns.Add("Narration");
        dt.Columns.Add("Debit");
        dt.Columns.Add("paymode");
        string sql = "";
        double total = 0;
        //double balance = 0;
        //double paid = 0;
        string transcode = "";
        //txt_transcode.Text;

        DataRow dr;
        string appno = "";
        string code = "";
        string name = "";
        vendor = "select VendorPK,vendorcode,VendorName from co_vendormaster where VendorType=-5";
        DataSet dset = new DataSet();
        dset = d2.select_method_wo_parameter(vendor, "Text");
        if (dset.Tables.Count > 0 && dset.Tables[0].Rows.Count > 0)
        {
            for (int r = 0; r < dset.Tables[0].Rows.Count; r++)
            {

                if (vendor != "")
                {
                    appno = Convert.ToString(dset.Tables[0].Rows[r]["VendorPK"]);
                    code = Convert.ToString(dset.Tables[0].Rows[r]["vendorcode"]);
                    name = Convert.ToString(dset.Tables[0].Rows[r]["VendorName"]);

                    sql = "select VendorName,VendorCode,VendorCompName,VendorAddress,VendorMobileNo from co_vendormaster where vendorname='" + name + "' and VendorType=-5 and VendorCode='" + code + "' ";
                    transcode = transCodetext.Text;

                    sql += "       select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,f.FeeCategory,''DeductAmout,isnull(f.Credit,0) as amount,isnull(f.debit,0) as BalAmount,(select distinct totalamount from ft_feeallot fa where fa.headerfk=f.headerfk and fa.ledgerfk=f.ledgerfk and fa.feecategory=f.feecategory and fa.App_No=" + appno + " and isnull(IsTransfer,'0')='0') as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),transdate,103) as excesstransdate,transcode as dailytranscode,narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,co_vendormaster co where co.VendorPK=f.App_No  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK  and memtype='4'  and  co.VendorPK=" + appno + " and transcode='" + transcode + "'";

                }
                ds = d2.select_method_wo_parameter(sql, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                    {
                        dr = dt.NewRow();
                        //modified ds.Tables[0].Rows[row] to ds.Tables[0].Rows[0]
                        dr["Sno"] = row + 1;
                        dr["Vendorname"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorName"]);
                        dr["VendorCode"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorCode"]);
                        dr["Companyname"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorCompName"]);
                        dr["mobile"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorMobileNo"]);
                        dr["address1"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorAddress"]);
                        dr["Header"] = Convert.ToString(ds.Tables[1].Rows[row]["HeaderName"]);
                        dr["HeaderFk"] = Convert.ToString(ds.Tables[1].Rows[row]["HeaderFK"]);
                        dr["Ledger"] = Convert.ToString(ds.Tables[1].Rows[row]["LedgerName"]);
                        dr["LedgerFk"] = Convert.ToString(ds.Tables[1].Rows[row]["LedgerFK"]);

                        dr["FeeAmt"] = Convert.ToString(ds.Tables[1].Rows[row]["excesstransdate"]);
                        transcode = Convert.ToString(ds.Tables[1].Rows[row]["dailytranscode"]);
                        dr["Total"] = transcode;
                        dr["Debit"] = Convert.ToString(ds.Tables[1].Rows[row]["amount"]);
                        // dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                        //dr["Paid"] = Convert.ToString(paidAmount - tempPaid);
                        dr["Balance"] = Convert.ToString(ds.Tables[1].Rows[row]["BalAmount"]);

                        dr["Narration"] = Convert.ToString(ds.Tables[1].Rows[row]["Narration"]);
                        dt.Rows.Add(dr);



                        //dt.Rows.Add(dr);

                        double tempTotal = 0;
                        //double tempBal = 0;
                        //double temppaid = 0;
                        double.TryParse(Convert.ToString(ds.Tables[1].Rows[row]["BalAmount"]), out tempTotal);
                        //double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]), out tempBal);
                        //double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]), out temppaid);

                        total += tempTotal;
                        //balance += tempBal;
                        //paid += temppaid;
                    }
                }
            }
        }

        if (dt.Rows.Count > 0)
        {
            othervendor.DataSource = dt;
            othervendor.DataBind();
            //Label4.Text = "Rs." + balance.ToString();
            //Label3.Text = "Rs." + paid.ToString();
            Label3.Text = "Rs." + total.ToString();
            Table1.Visible = true;
            // Table1.Visible = true;
        }
        else
        {
            othervendor.DataSource = null;
            othervendor.DataBind();
            Table1.Visible = false;
        }

    }

    public void bindgridscholarship()//added by abarna 10.1.2018
    {
        string vendor = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("Vendorname");

        dt.Columns.Add("VendorCode");
        dt.Columns.Add("Companyname");
        dt.Columns.Add("mobile");
        dt.Columns.Add("address1");
        //  dt.Columns.Add("address2");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("FeeAmt");
        dt.Columns.Add("Total");
        dt.Columns.Add("Narration");
        dt.Columns.Add("Debit");
        dt.Columns.Add("paymode");
        string sql = "";
        double total = 0;
        //double balance = 0;
        //double paid = 0;
        string transcode = "";
        //txt_transcode.Text;

        DataRow dr;
        string appno = "";
        string code = "";
        string name = "";
        vendor = "select VendorPK,vendorcode,VendorName from co_vendormaster where VendorType=-5";
        DataSet dset = new DataSet();
        dset = d2.select_method_wo_parameter(vendor, "Text");
        if (dset.Tables.Count > 0 && dset.Tables[0].Rows.Count > 0)
        {
            for (int r = 0; r < dset.Tables[0].Rows.Count; r++)
            {

                if (vendor != "")
                {
                    appno = Convert.ToString(dset.Tables[0].Rows[r]["VendorPK"]);
                    code = Convert.ToString(dset.Tables[0].Rows[r]["vendorcode"]);
                    name = Convert.ToString(dset.Tables[0].Rows[r]["VendorName"]);

                    sql = "select VendorName,VendorCode,VendorCompName,VendorAddress,VendorMobileNo from co_vendormaster where vendorname='" + name + "' and VendorType=-5 and VendorCode='" + code + "' ";
                    //if (transcode == "")
                    //{

                    sql += "       select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,f.FeeCategory,''DeductAmout,isnull(f.Credit,0) as amount,isnull(f.debit,0) as BalAmount,(select distinct totalamount from ft_feeallot fa where fa.headerfk=f.headerfk and fa.ledgerfk=f.ledgerfk and fa.feecategory=f.feecategory and fa.App_No=" + appno + " and isnull(IsTransfer,'0')='0') as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),transdate,103) as excesstransdate,transcode as dailytranscode,narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,co_vendormaster co where co.VendorPK=f.App_No  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK  and memtype='4'  and  co.VendorPK=" + appno + "";
                    //}
                    //else
                    //{
                    //    sql += "       select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,f.FeeCategory,''DeductAmout,isnull(f.Credit,0) as amount,isnull(f.debit,0) as BalAmount,(select distinct totalamount from ft_feeallot fa where fa.headerfk=f.headerfk and fa.ledgerfk=f.ledgerfk and fa.feecategory=f.feecategory and fa.App_No=" + appno + " and istransfer='0') as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),transdate,103) as excesstransdate,transcode as dailytranscode,narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,co_vendormaster co where co.VendorPK=f.App_No  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK  and memtype='4'  and  co.VendorPK=" + appno + " and transcode='" + transcode + "'";
                    //}


                }

                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                    {
                        dr = dt.NewRow();
                        //modified ds.Tables[0].Rows[row] to ds.Tables[0].Rows[0]
                        dr["Sno"] = row + 1;
                        dr["Vendorname"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorName"]);
                        dr["VendorCode"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorCode"]);
                        dr["Companyname"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorCompName"]);
                        dr["mobile"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorMobileNo"]);
                        dr["address1"] = Convert.ToString(ds.Tables[0].Rows[0]["VendorAddress"]);
                        dr["Header"] = Convert.ToString(ds.Tables[1].Rows[row]["HeaderName"]);
                        dr["HeaderFk"] = Convert.ToString(ds.Tables[1].Rows[row]["HeaderFK"]);
                        dr["Ledger"] = Convert.ToString(ds.Tables[1].Rows[row]["LedgerName"]);
                        dr["LedgerFk"] = Convert.ToString(ds.Tables[1].Rows[row]["LedgerFK"]);

                        dr["FeeAmt"] = Convert.ToString(ds.Tables[1].Rows[row]["excesstransdate"]);
                        transcode = Convert.ToString(ds.Tables[1].Rows[row]["dailytranscode"]);
                        dr["Total"] = transcode;
                        dr["Debit"] = Convert.ToString(ds.Tables[1].Rows[row]["amount"]);
                        // dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                        //dr["Paid"] = Convert.ToString(paidAmount - tempPaid);
                        dr["Balance"] = Convert.ToString(ds.Tables[1].Rows[row]["BalAmount"]);

                        dr["Narration"] = Convert.ToString(ds.Tables[1].Rows[row]["Narration"]);
                        dt.Rows.Add(dr);



                        //dt.Rows.Add(dr);

                        double tempTotal = 0;
                        //double tempBal = 0;
                        //double temppaid = 0;
                        double.TryParse(Convert.ToString(ds.Tables[1].Rows[row]["BalAmount"]), out tempTotal);
                        //double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]), out tempBal);
                        //double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]), out temppaid);

                        total += tempTotal;
                        //balance += tempBal;
                        //paid += temppaid;
                    }
                }
            }
        }

        if (dt.Rows.Count > 0)
        {
            othervendor.DataSource = dt;
            othervendor.DataBind();
            //Label4.Text = "Rs." + balance.ToString();
            //Label3.Text = "Rs." + paid.ToString();
            Label3.Text = "Rs." + total.ToString();
            Table1.Visible = true;
            // Table1.Visible = true;
        }
        else
        {
            othervendor.DataSource = null;
            othervendor.DataBind();
            Table1.Visible = false;
        }

    }

    public void bindGridJournalAdvance()
    {
        string app_no = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("YearSem");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("FeeCategory");
        dt.Columns.Add("Concession");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("Total");
        dt.Columns.Add("FeeAmt");
        dt.Columns.Add("paymode");
        dt.Columns.Add("Narration");

        DataRow dr;
        double total = 0;
        double balance = 0;
        double paid = 0;
        string paidVal = string.Empty;
        if (incPaid.Checked)
            paidVal = " and isnull(lex.Adjamt,'0')<>0";




        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
        {
            app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }
        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
        {
            app_no = d2.GetFunction("select app_no from Registration where Reg_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }
        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
        {
            app_no = d2.GetFunction("select app_no from Registration where Roll_admit='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }
        if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
        {
            app_no = d2.GetFunction("select app_no from applyn where app_formno='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
        }

        if (rbl_rollnoNew.Text == "Staff")
        {
            app_no = d2.GetFunction(" select appl_id from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code and s.staff_code ='" + txtroll_staff.Text.Trim() + "' and s.college_Code in('" + ddlcollege.SelectedValue + "')");
        }


        if (app_no != "")
        {
            string selectQ = "";
            if (ddlJournalType.SelectedIndex == 0 || ddlJournalType.SelectedIndex == 2)
            {
                #region journal
                string jourNalType = string.Empty;
                if (ddlJournalType.SelectedIndex == 0)
                    jourNalType = " and Ex_JournalEntry='1' and excesstype='1'";
                else
                    jourNalType = " and isnull(Ex_JournalEntry,'0')='0' and excesstype='1'";
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                {
                    selectQ = "       select lex.HeaderFK,h.HeaderName,lex.LedgerFK,l.LedgerName,lex.FeeCategory,''DeductAmout,isnull(lex.Adjamt,0) as PaidAmount,isnull(lex.excessamt,0)-isnull(lex.adjamt,0) as BalAmount,isnull(lex.excessamt,0) as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),excesstransdate,103) as excesstransdate,dailytranscode from ft_excessdet ex,ft_excessledgerdet lex,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=ex.App_No and lex.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=lex.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and ex.excessdetpk=lex.excessdetfk " + jourNalType + " and memtype='1'  and r.App_No=" + app_no + " " + paidVal + "  and isnull(lex.adjamt,'0')<>isnull(lex.excessamt,'0') and isnull(lex.balanceamt,'0')<>'0'   order by isnull(l.priority,1000), l.ledgerName asc,ex.FeeCategory";
                }
                else
                {
                    selectQ = "       select lex.HeaderFK,h.HeaderName,lex.LedgerFK,l.LedgerName,lex.FeeCategory,''DeductAmout,isnull(lex.Adjamt,0) as PaidAmount,isnull(lex.excessamt,0)-isnull(lex.adjamt,0) as BalAmount,isnull(lex.excessamt,0) as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),excesstransdate,103) as excesstransdate,dailytranscode from ft_excessdet ex,ft_excessledgerdet lex,applyn R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=ex.App_No and lex.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=lex.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and ex.excessdetpk=lex.excessdetfk " + jourNalType + " and memtype='1'  and r.App_No=" + app_no + " " + paidVal + "  and isnull(lex.adjamt,'0')<>isnull(lex.excessamt,'0') and isnull(lex.balanceamt,'0')<>'0'  order by isnull(l.priority,1000), l.ledgerName asc,ex.FeeCategory";
                }
                #endregion
            }

            else
            {
                #region already paid amount
                string Transrcpt = string.Empty;
                if (rbl_rollnoNew.Text == "Student")
                {
                    string getApp = d2.GetFunction("select appno from st_student_transfer s,st_student_transfer_details sd where s.studenttransferpk=sd.studenttransferfk and appno='" + app_no + "'");

                    if (getApp != "0")
                    {
                        Transrcpt = " and transdate>= (  select  top(1) transferdate from st_student_transfer s,st_student_transfer_details sd where s.studenttransferpk=sd.studenttransferfk and  appno='" + app_no + "' order by transferdate desc)";
                    }
                    if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
                    {
                        selectQ = "       select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,f.FeeCategory,''DeductAmout,isnull(f.debit,0) as PaidAmount,isnull(f.debit,0) as BalAmount,(select distinct totalamount from ft_feeallot fa where fa.headerfk=f.headerfk and fa.ledgerfk=f.ledgerfk and fa.feecategory=f.feecategory and fa.App_No=" + app_no + " and isnull(IsTransfer,'0')='0') as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),transdate,103) as excesstransdate,transcode as dailytranscode,narration   from ft_findailytransaction f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and memtype='1'  and r.App_No=" + app_no + " " + paidVal + " " + Transrcpt + "  and isnull(f.debit,'0')>0  and isnull(paid_Istransfer,'0')='0'  order by isnull(l.priority,1000), l.ledgerName asc,f.FeeCategory";
                    }
                    else
                    {
                        selectQ = "       select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,f.FeeCategory,''DeductAmout,isnull(f.debit,0) as PaidAmount,isnull(f.debit,0) as BalAmount,(select distinct totalamount from ft_feeallot fa where fa.headerfk=f.headerfk and fa.ledgerfk=f.ledgerfk and fa.feecategory=f.feecategory and r.App_No=" + app_no + " and isnull(IsTransfer,'0')='0') as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),transdate,103) as excesstransdate,transcode  as dailytranscode,narration  from ft_findailytransaction f,applyn R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and memtype='1'  and r.App_No=" + app_no + " " + paidVal + " " + Transrcpt + "  and isnull(f.debit,'0')>0 and isnull(paid_Istransfer,'0')='0'   order by isnull(l.priority,1000), l.ledgerName asc,f.FeeCategory";
                    }
                }
                if (rbl_rollnoNew.Text == "Staff")
                {




                    selectQ = "       select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,f.FeeCategory,''DeductAmout,isnull(f.debit,0) as PaidAmount,isnull(f.debit,0) as BalAmount,(select distinct totalamount from ft_feeallot fa where fa.headerfk=f.headerfk and fa.ledgerfk=f.ledgerfk and fa.feecategory=f.feecategory and fa.App_No=" + app_no + " and isnull(IsTransfer,'0')='0') as TotalAmount,''FeeAmount,''paymode,convert(varchar(10),transdate,103) as excesstransdate,transcode as dailytranscode,narration   from ft_findailytransaction f,FM_HeaderMaster H,FM_LedgerMaster L,staff_appl_master s where s.appl_id=f.App_No  and f.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=f.LedgerFK  and memtype='2'  and  s.appl_id=" + app_no + " " + paidVal + " " + Transrcpt + "  and isnull(f.debit,'0')>0  and isnull(paid_Istransfer,'0')='0'  order by isnull(l.priority,1000), l.ledgerName asc,f.FeeCategory";
                }


                #endregion
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                    string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + ddlcollege.SelectedValue + "");
                    double tempPaid = 0;
                    double tempVal = 0;
                    double paidAmount = 0;
                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]), out paidAmount);
                    if (ddlJournalType.SelectedIndex == 1)
                    {
                        string selQ = "select sum(credit) from ft_findailytransaction where headerfk='" + Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]) + "' and ledgerfk='" + Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]) + "' and feecategory='" + feecat + "' and  App_No=" + app_no + "";//and transcode='" + transcode + "'
                        double.TryParse(Convert.ToString(d2.GetFunction(selQ)), out tempPaid);
                        tempVal = paidAmount - tempPaid;
                    }
                    else
                        tempVal = 1;
                    if (tempVal != 0)
                    {
                        dr = dt.NewRow();
                        dr["Sno"] = row + 1;
                        dr["YearSem"] = cursem;
                        dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                        dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                        dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                        dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                        dr["FeeCategory"] = feecat;
                        dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["excesstransdate"]);
                        string transcode = Convert.ToString(ds.Tables[0].Rows[row]["dailytranscode"]);
                        dr["Total"] = transcode;
                        dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);

                        dr["Paid"] = Convert.ToString(paidAmount - tempPaid);
                        dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                        dr["paymode"] = Convert.ToString(ds.Tables[0].Rows[row]["paymode"]);
                        if (ddlJournalType.SelectedIndex == 1)
                        {
                            dr["Narration"] = Convert.ToString(ds.Tables[0].Rows[row]["Narration"]);
                        }
                        dt.Rows.Add(dr);

                        double tempTotal = 0;
                        double tempBal = 0;
                        double temppaid = 0;
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]), out tempTotal);
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]), out tempBal);
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]), out temppaid);

                        total += tempTotal;
                        balance += tempBal;
                        paid += temppaid;
                    }

                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            gridView4.DataSource = dt;
            gridView4.DataBind();
            Label4.Text = "Rs." + balance.ToString();
            if (ddlJournalType.SelectedIndex == 0)
            {
                Label3.Text = "Rs." + total.ToString();
            }
            else
            {
                Label3.Text = "Rs." + paid.ToString();
            }
            Label2.Text = "Rs." + total.ToString();
            Table1.Visible = true;
        }
        else
        {
            gridView4.DataSource = null;
            gridView4.DataBind();
            Table1.Visible = false;
        }
    }

    public void bindGridAllotJournal()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("YearSem");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("FeeCategory");
        dt.Columns.Add("Concession");
        dt.Columns.Add("tobePaid");
        dt.Columns.Add("hiddenTempAmt");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("Total");
        dt.Columns.Add("FeeAmt");

        DataRow dr;
        double total = 0;
        double balance = 0;
        double paid = 0;


        string selectQ = "";
        string app_no = string.Empty;
        string IncfeeCat = string.Empty;
        if (incSem.Checked)
            IncfeeCat = " and f.FeeCategory in('" + Convert.ToString(getCblSelectedValue(cbl_sem)) + "')";
        if (rbl_rollnoNew.Text == "Student")
        {
            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
            {
                app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
            }
            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
            {
                app_no = d2.GetFunction("select app_no from Registration where Reg_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
            }
            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
            {
                app_no = d2.GetFunction("select app_no from Registration where Roll_admit='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
            }
            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
            {
                app_no = d2.GetFunction("select app_no from applyn where app_formno='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
            }
        }
        //if (rbl_rollnoNew.Text == "Staff")
        //{
        //    app_no = d2.GetFunction(" select appl_id from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code and s.staff_code ='" + txtroll_staff.Text.Trim() + "' and s.college_Code in('" + ddlcollege.SelectedValue + "')");
        //}


        if (app_no != "0")
        {
            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) != 3)
            {
                selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,f.paymode   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code  and r.App_No=" + app_no + " " + IncfeeCat + " and isnull(f.balamount,'0')<>'0' and isnull(paidamount,'0')<>isnull(totalamount,'0') and isnull(IsTransfer,'0')='0'   order by F.FeeCategory";//isnull(l.priority,1000), l.ledgerName asc,
                // order by F.FeeCategory,f.HeaderFK,f.LedgerFK and isnull(paidamount,'0')='0' 
            }
            else
            {
                selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,f.paymode   from FT_FeeAllot f,applyn R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code  and r.App_No=" + app_no + " " + IncfeeCat + " and isnull(f.balamount,'0')<>'0'  and isnull(paidamount,'0')<>isnull(totalamount,'0') and isnull(IsTransfer,'0')='0'  order by F.FeeCategory";//isnull(l.priority,1000), l.ledgerName asc,
                //order by F.FeeCategory,f.HeaderFK,f.LedgerFK and isnull(paidamount,'0')='0'
            }
            //if (rbl_rollnoNew.Text == "Staff")
            //{
            //    selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,f.paymode   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code  and memtype='2' and r.App_No=" + app_no + " " + IncfeeCat + " and isnull(f.balamount,'0')<>'0' and isnull(paidamount,'0')<>isnull(totalamount,'0') and IsTransfer='0'   order by F.FeeCategory";
            //}
            if (rbl_rollnoNew.Text == "Staff")
            {
                selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.TotalAmount,0)-isnull(F.PaidAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount,f.paymode   from FT_FeeAllot f,FM_HeaderMaster H,FM_LedgerMaster L,staff_appl_master s where s.appl_id=f.App_No   and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and memtype='2'  and  s.appl_id=" + app_no + " " + IncfeeCat + " and isnull(f.balamount,'0')<>'0' and isnull(paidamount,'0')<>isnull(totalamount,'0') and isnull(IsTransfer,'0')='0'   order by F.FeeCategory";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                    string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + ddlcollege.SelectedValue + "");
                    dr = dt.NewRow();
                    dr["Sno"] = row + 1;
                    dr["YearSem"] = cursem;
                    dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                    dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                    dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                    dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                    dr["FeeCategory"] = feecat;
                    dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                    dr["tobePaid"] = "0";//added by abarna
                    dr["hiddenTempAmt"] = "0";
                    dr["Paid"] = Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]);
                    dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                    dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                    dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                    dt.Rows.Add(dr);

                    total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);
                    balance += Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]);
                    paid += Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]);
                }
            }
        }

        if (dt.Rows.Count > 0)
        {
            gridView5.DataSource = dt;
            gridView5.DataBind();
            Label8.Text = "Rs." + balance.ToString();
            Label6.Text = "Rs." + paid.ToString();
            Label5.Text = "Rs." + total.ToString();
            Label9.Text = "Rs.";
            // Label10.Text = "RS.";
            Table2.Visible = true;
        }
        else
        {
            gridView5.DataSource = null;
            gridView5.DataBind();
            Table2.Visible = false;
        }
    }

    protected bool JournalPaidAmount()
    {
        bool updateOK = false;
        try
        {

            string finYearid = string.Empty;
            string entryUserCode = string.Empty;
            DateTime transdate = Convert.ToDateTime(txt_rdate.Text.Trim().Split('/')[1] + "/" + txt_rdate.Text.Trim().Split('/')[0] + "/" + txt_rdate.Text.Trim().Split('/')[2]);
            string batch = string.Empty;
            string sec = string.Empty;
            string sem = string.Empty;
            string colCode = string.Empty;
            string degcode = string.Empty;
            string seatype = string.Empty;
            string Rcptno = string.Empty;
            string appno = string.Empty;
            Dictionary<string, string> dtReceipt = new Dictionary<string, string>();
            Dictionary<string, string> arRcptfk = new Dictionary<string, string>();
            string hedgid = ledgermappingheaderValue();
            finYearid = d2.getCurrentFinanceYear(usercode, ddlcollege.SelectedValue);
            colCode = ddlcollege.SelectedValue;
            string rollno = txt_rerollno.Text.Trim();

            //======================Modified by saranya on 07Dec2017================================//

            if (rbl_rollnoNew.Text == "Student")
            {
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
                {
                    appno = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
                }
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
                {
                    appno = d2.GetFunction("select app_no from Registration where Reg_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
                }
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 2)
                {
                    appno = d2.GetFunction("select app_no from Registration where Roll_admit='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
                }
                if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 3)
                {
                    appno = d2.GetFunction("select app_no from applyn where app_formno='" + txt_rerollno.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
                }
            }
            if (rbl_rollnoNew.Text == "Staff")
            {
                appno = d2.GetFunction("select a.appl_id from staffmaster s,staff_appl_master a where s.appl_no =a.appl_no and staff_code='" + txtroll_staff.Text.Trim() + "' and college_code='" + ddlcollege.SelectedValue + "'");
            }
            //if (Convert.ToInt32(rbl_rollnoNew.SelectedItem.Value) == 2)
            //{
            //    appno = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" +txtroll_vendor.Text.Trim() + "' and vendortype='1'");
            //}
            //if (Convert.ToInt32(rbl_rollnoNew.SelectedItem.Value) == 3)
            //{
            //    appno = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCode='" + txtroll_other.Text.Trim() + "' and vendortype='-5'");
            //}
            //==========================================================================================================================//

            if (!incJournal.Checked)
            {
                //  if (hedgid != "")
                Rcptno = generateReceiptNo(hedgid, ref dtReceipt, ref arRcptfk);
            }
            else
            {
                // if (hedgid != "")
                Rcptno = generateJournalNo(hedgid, colCode);
            }
            if (!string.IsNullOrEmpty(Rcptno) && Rcptno != "0" && finYearid != "0" && appno != "0")
            {
                if (checkAdvanceAmt())//if excess amount available hedader and ledger must be available
                {

                    bool inSertAlot = JournalinsertAllotAmount(appno, transdate, finYearid);
                    if (inSertAlot)
                    {
                        #region advance amt Update
                        double totalPaidAmt = 0;
                        //double.TryParse(Convert.ToString(Label9.Text.Trim().Split('.')[1]), out totalPaidAmt);
                        double.TryParse(Convert.ToString(hiddnewPaid.Value), out totalPaidAmt);
                        foreach (GridViewRow gdrow in gridView4.Rows)
                        {
                            double balAmt = 0;
                            CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
                            if (cb.Checked)
                            {
                                Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                                Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                                Label lblfeecat = (Label)gdrow.FindControl("lbl_feecat");
                                Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                                Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                                Label lblbal = (Label)gdrow.FindControl("lbl_bal");
                                Label transcode = (Label)gdrow.FindControl("lbl_feeamt");
                                Label transdt = (Label)gdrow.FindControl("lbl_totamt");
                                string preTransdt = transdt.Text.Split('/')[1] + "/" + transdt.Text.Split('/')[0] + "/" + transdt.Text.Split('/')[2];
                                double tempAdvAMt = 0;
                                double.TryParse(Convert.ToString(lblbal.Text), out  balAmt);
                                if (ddlJournalType.SelectedIndex == 0 || ddlJournalType.SelectedIndex == 2)//only journal adjustement
                                {
                                    string jourNalType = string.Empty;
                                    if (ddlJournalType.SelectedIndex == 0)
                                        jourNalType = " and ExcessType='1' and Ex_JournalEntry='1'";
                                    if (balAmt >= totalPaidAmt)
                                        tempAdvAMt = totalPaidAmt;
                                    else
                                        tempAdvAMt = balAmt;
                                    totalPaidAmt = (totalPaidAmt - balAmt);
                                    if (tempAdvAMt != 0)
                                    {
                                        getJournalAmountUpdate(tempAdvAMt, appno, preTransdt, transcode.Text, lblfeecat.Text, Convert.ToString(transdate), lblledg.Text, jourNalType);
                                    }
                                }
                                else if (ddlJournalType.SelectedIndex == 1)
                                {
                                    // getOldPayment(appno, lblhedg.Text, lblledg.Text, lblfeecat.Text, transcode.Text, Convert.ToString(tempAdvAMt), preTransdt);
                                    //string updQ = " update ft_feeallot set paidamount=isnull(paidamount,'0')-'" + tempAdvAMt + "',balamount=isnull(balamount,'0')+'" + tempAdvAMt + "' where app_no='" + appno + "' and headerfk='" + lblhedg.Text + "' and ledgerfk='" + lblledg.Text + "' and feecategory='" + lblfeecat.Text + "'";
                                    //int inst = d2.update_method_wo_parameter(updQ, "Text");
                                }
                            }
                        }
                        if (ddlJournalType.SelectedIndex == 3)
                        {

                            foreach (GridViewRow gdrow in othervendor.Rows)
                            {
                                double balAmt = 0;
                                CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
                                if (cb.Checked)
                                {
                                    Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                                    Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                                    Label lblfeecat = (Label)gdrow.FindControl("lbl_feecat");
                                    Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                                    Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                                    Label lblbal = (Label)gdrow.FindControl("lbl_bal");
                                    Label transcode = (Label)gdrow.FindControl("lbl_feeamt");
                                    Label transdt = (Label)gdrow.FindControl("lbl_totamt");
                                    string preTransdt = transdt.Text.Split('/')[1] + "/" + transdt.Text.Split('/')[0] + "/" + transdt.Text.Split('/')[2];
                                    double tempAdvAMt = 0;
                                    double.TryParse(Convert.ToString(lblbal.Text), out  balAmt);



                                    if (ddlJournalType.SelectedIndex == 3)//only journal adjustement
                                    {
                                        string jourNalType = string.Empty;
                                        if (ddlJournalType.SelectedIndex == 0)
                                            jourNalType = " and ExcessType='1' and Ex_JournalEntry='1'";
                                        if (balAmt >= totalPaidAmt)
                                            tempAdvAMt = totalPaidAmt;
                                        else
                                            tempAdvAMt = balAmt;
                                        totalPaidAmt = (totalPaidAmt - balAmt);
                                        if (tempAdvAMt != 0)
                                        {
                                            // getJournalAmountUpdate(tempAdvAMt, appno, preTransdt, transcode.Text, lblfeecat.Text, Convert.ToString(transdate), lblledg.Text, jourNalType);
                                        }
                                    }
                                    else if (ddlJournalType.SelectedIndex == 1)
                                    {
                                        // getOldPayment(appno, lblhedg.Text, lblledg.Text, lblfeecat.Text, transcode.Text, Convert.ToString(tempAdvAMt), preTransdt);
                                        //string updQ = " update ft_feeallot set paidamount=isnull(paidamount,'0')-'" + tempAdvAMt + "',balamount=isnull(balamount,'0')+'" + tempAdvAMt + "' where app_no='" + appno + "' and headerfk='" + lblhedg.Text + "' and ledgerfk='" + lblledg.Text + "' and feecategory='" + lblfeecat.Text + "'";
                                        //int inst = d2.update_method_wo_parameter(updQ, "Text");
                                    }


                                }
                            }

                        }
                        #endregion
                        double toBePaidAmt = 0;

                        #region Dailytransaction insert
                        //entryUserCode = d2.GetFunction(" select distinct entryusercode from FT_FinDailyTransaction where app_no='" + appno + "'");//commented by saranya on 28/12/2017
                        foreach (GridViewRow row in gridView5.Rows)
                        {
                            //CheckBox cbsel = (CheckBox)row.FindControl("cblsell");
                            //if (cbsel.Checked)
                            //{
                            Label hdrid = (Label)row.FindControl("lbl_hdrid");
                            Label lgrid = (Label)row.FindControl("lbl_lgrid");
                            Label feecat = (Label)row.FindControl("lbl_feecat");
                            Label feeamt = (Label)row.FindControl("lbl_feeamt");
                            Label totamt = (Label)row.FindControl("lbl_totamt");
                            Label concession = (Label)row.FindControl("lbl_Concess");
                            TextBox paid = (TextBox)row.FindControl("txt_paid");
                            TextBox txtToBePaid = (TextBox)row.FindControl("txt_tobePaid");
                            TextBox balance = (TextBox)row.FindControl("txt_bal");
                            TextBox excess = (TextBox)row.FindControl("txt_exGrid2");

                            double feeAmt = 0;
                            double totalAmt = 0;
                            double concsAmt = 0;
                            double paidAmt = 0;

                            double excessAmt = 0;
                            double balAmt = 0;

                            double.TryParse(Convert.ToString(feeamt.Text), out  feeAmt);
                            double.TryParse(Convert.ToString(totamt.Text), out  totalAmt);
                            double.TryParse(Convert.ToString(concession.Text), out  concsAmt);
                            double.TryParse(Convert.ToString(paid.Text), out  paidAmt);
                            double.TryParse(Convert.ToString(txtToBePaid.Text), out  toBePaidAmt);
                            double.TryParse(Convert.ToString(excess.Text), out  excessAmt);
                            double.TryParse(Convert.ToString(balance.Text), out  balAmt);
                            if (toBePaidAmt == 0)
                                continue;
                            toBePaidAmt = toBePaidAmt - excessAmt;
                            //    balAmt = totalAmt - paidAmt;
                            if (toBePaidAmt != 0)
                            {
                                //credit entry 
                                string INSdaily = string.Empty;
                                if (ddlJournalType.SelectedIndex == 0 || ddlJournalType.SelectedIndex == 1 || ddlJournalType.SelectedIndex == 2)//only journal adjustement
                                {
                                    //INSdaily = "insert into FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,LedgerFK,HeaderFK,FeeCategory,credit,FinYearFK,App_No,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype,narration) values('" + transdate.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToShortTimeString() + "','" + Rcptno + "','1','" + lgrid.Text + "','" + hdrid.Text + "','" + feecat.Text + "','" + toBePaidAmt + "','" + finYearid + "','" + appno + "','0','1','1','1','" + entryUserCode + "','3','" + txtnaration.Text.Trim() + "')";
                                    //d2.update_method_wo_parameter(INSdaily, "Text");
                                    //}
                                    //else
                                    //{
                                    #region
                                    foreach (GridViewRow gdrow in gridView4.Rows)
                                    {
                                        // double balAmt = 0;
                                        CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
                                        if (cb.Checked)
                                        {
                                            Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                                            Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                                            Label lblfeecat = (Label)gdrow.FindControl("lbl_feecat");
                                            Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                                            Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                                            Label lblbal = (Label)gdrow.FindControl("lbl_bal");
                                            Label transcode = (Label)gdrow.FindControl("lbl_feeamt");
                                            Label transdt = (Label)gdrow.FindControl("lbl_totamt");
                                            string preTransdt = transdt.Text.Split('/')[1] + "/" + transdt.Text.Split('/')[0] + "/" + transdt.Text.Split('/')[2];
                                            INSdaily = "insert into FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,LedgerFK,HeaderFK,FeeCategory,credit,FinYearFK,App_No,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype,narration,receipttype) values('" + transdate.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToShortTimeString() + "','" + Rcptno + "','1','" + lblledg.Text + "','" + lblhedg.Text + "','" + lblfeecat.Text + "','" + toBePaidAmt + "','" + finYearid + "','" + appno + "','0','1','1','1','" + userCode + "','3','" + txtnaration.Text.Trim() + "','6')";//entryUserCode is modifified to userCode by saranya on 28/12/2017
                                            d2.update_method_wo_parameter(INSdaily, "Text");
                                            int savevalue = 1;

                                            if (ddlJournalType.SelectedIndex == 1)
                                            {
                                                string updQ = " update ft_feeallot set paidamount=isnull(paidamount,'0')-'" + toBePaidAmt + "',balamount=isnull(balamount,'0')+'" + toBePaidAmt + "' where app_no='" + appno + "' and headerfk='" + lblhedg.Text + "' and ledgerfk='" + lblledg.Text + "' and feecategory='" + lblfeecat.Text + "' and istransfer='0'";
                                                int inst = d2.update_method_wo_parameter(updQ, "Text");

                                            }
                                            //==========================Added by Saranya on 10/04/2018=============================//
                                            string entrycode = Session["Entry_Code"].ToString();
                                            string formname = "Journal";
                                            string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                                            string doa = DateTime.Now.ToString("MM/dd/yyy");

                                            IPHostEntry host;
                                            string localip = "";
                                            host = Dns.GetHostEntry(Dns.GetHostName());
                                            foreach (IPAddress ip in host.AddressList)
                                            {
                                                if (ip.AddressFamily.ToString() == "InterNetwork")
                                                {
                                                    localip = ip.ToString();
                                                }
                                            }
                                            string details = "RollNo - " + rollno + ": CollegeCode - " + colCode + ": Semester - " + lblfeecat.Text + ": ReceiptNO -" + Rcptno + " : Amount -" + toBePaidAmt + " : Date - " + toa + "";
                                            string ctsname = "";
                                            if (savevalue == 1)
                                            {
                                                ctsname = "Journal Adjustment";
                                                string hostName = Dns.GetHostName();
                                                d2.insertEinanceUserActionLog(entrycode, formname, 1, toa, doa, details, ctsname, localip);
                                            }

                                            //============================================================================//

                                        }

                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region
                                    foreach (GridViewRow gdrow in othervendor.Rows)
                                    {
                                        // double balAmt = 0;
                                        CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
                                        if (cb.Checked)
                                        {
                                            Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                                            Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                                            Label lblfeecat = (Label)gdrow.FindControl("lbl_feecat");
                                            Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                                            Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                                            Label lblbal = (Label)gdrow.FindControl("lbl_bal");
                                            Label transcode = (Label)gdrow.FindControl("lbl_feeamt");
                                            Label transdt = (Label)gdrow.FindControl("lbl_totamt");
                                            string preTransdt = transdt.Text.Split('/')[1] + "/" + transdt.Text.Split('/')[0] + "/" + transdt.Text.Split('/')[2];
                                            INSdaily = "insert into FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,LedgerFK,HeaderFK,credit,FinYearFK,App_No,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype,narration,receipttype) values('" + transdate.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToShortTimeString() + "','" + Rcptno + "','4','" + lblledg.Text + "','" + lblhedg.Text + "','" + toBePaidAmt + "','" + finYearid + "','" + appno + "','0','1','1','1','" + userCode + "','3','" + txtnaration.Text.Trim() + "','6')";//entryUserCode is modifified to userCode by saranya on 28/12/2017
                                            d2.update_method_wo_parameter(INSdaily, "Text");

                                            if (ddlJournalType.SelectedIndex == 1)
                                            {
                                                string updQ = " update ft_feeallot set paidamount=isnull(paidamount,'0')-'" + toBePaidAmt + "',balamount=isnull(balamount,'0')+'" + toBePaidAmt + "' where app_no='" + appno + "' and headerfk='" + lblhedg.Text + "' and ledgerfk='" + lblledg.Text + "' and feecategory='" + lblfeecat.Text + "' and istransfer='0'";
                                                int inst = d2.update_method_wo_parameter(updQ, "Text");

                                            }
                                        }

                                    }
                                    #endregion
                                }
                                //debit entry
                                INSdaily = "insert into FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,LedgerFK,HeaderFK,FeeCategory,Debit,FinYearFK,App_No,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype,narration) values('" + transdate.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToShortTimeString() + "','" + Rcptno + "','1','" + lgrid.Text + "','" + hdrid.Text + "','" + feecat.Text + "','" + toBePaidAmt + "','" + finYearid + "','" + appno + "','0','1','1','1','" + userCode + "','3','" + txtnaration.Text.Trim() + "')";//entryUserCode is modified as userCode by saranya on 28/12/2017
                                d2.update_method_wo_parameter(INSdaily, "Text");
                            }
                            updateOK = true;
                            //}
                        }

                        #endregion

                        if (updateOK)
                        {
                            #region update receipt,new insert to transfer table and print
                            updateReceiptNo(Rcptno, finYearid);
                            string tempCollegecode = string.Empty;
                            tempCollegecode = Convert.ToString(ddlcollege.SelectedValue);
                            if (incJournal.Checked)
                            {
                                // divindi.Visible = false;
                                disTransClear();
                                divindi.Visible = false;
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                                transferReceiptJournal("Journal", appno, tempCollegecode, transdate.ToString("MM/dd/yyyy"), Rcptno);
                                // transferReceipt("Journal", appno, tempCollegecode, transdate.ToString("MM/dd/yyyy"), Convert.ToString(sbNewRecptCode));



                            }
                            else
                            {
                                divindi.Visible = false;
                                transFromClear();
                                transToClear();
                            }
                            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Transfered Successfully')", true);
                            #endregion
                        }
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Not Alloted Amount')", true);
                }
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Receipt/Journal No Not Generated')", true);
        }
        catch { }
        return updateOK;
    }
    protected void getJournalAmountUpdate(double tempAdvAMt, string appno, string preTransdt, string transcode, string lblfeecat, string transdate, string lblledg, string JournalType)
    {
        try
        {
            string upDQry = " update FT_ExcessDet set Adjamt=isnull(Adjamt,'0')+'" + tempAdvAMt + "',BalanceAmt=isnull(BalanceAmt,'0')-'" + tempAdvAMt + "' where App_No='" + appno + "' " + JournalType + "  and FeeCategory='" + lblfeecat + "' and ExcessTransDate='" + preTransdt + "' and dailytranscode='" + transcode + "' ";//and FinYearFK='" + finYearid + "'
            int updT = d2.update_method_wo_parameter(upDQry, "Text");
            if (updT > 0)
            {
                string excessPK = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No ='" + appno + "'  and ExcessTransDate='" + transdate + "' and dailytranscode='" + transcode + "' " + JournalType + "");
                string update = " update FT_ExcessledgerDet set Adjamt=isnull(Adjamt,'0')+'" + tempAdvAMt + "',BalanceAmt=isnull(BalanceAmt,'0')-'" + tempAdvAMt + "' where ExcessDetfK='" + excessPK + "'  and FeeCategory='" + lblfeecat + "' and  ledgerfk='" + lblledg + "' ";//and FinYearFK='" + finYearid + "'
                int updTs = d2.update_method_wo_parameter(update, "Text");

                //===Added by saranya on 08/01/2018 for individual student feereport===//
                string ExcessRcptAmt = d2.GetFunction("select amount from ft_excessReceiptdet where app_no='" + appno + "' and excesstype='1' and receiptno='" + transcode + "' ");
                tempAdvAMt = Convert.ToInt32(ExcessRcptAmt) - tempAdvAMt;
                //=====================================================================//
                string insExcessRcpt = "insert into ft_excessReceiptdet (app_no , amount , receiptno ,rcptdate,ledgerfk,excesstype ) values ('" + appno + "', '" + tempAdvAMt + "', '" + transcode + "','" + preTransdt + "','0','2')";

                int updTss = d2.update_method_wo_parameter(insExcessRcpt, "Text");
            }
        }
        catch { }
    }
    protected bool JournalinsertAllotAmount(string appno, DateTime transdate, string finYearid)
    {
        bool updateOK = false;
        try
        {
            // feeallot insert record
            #region Allot insert
            foreach (GridViewRow row in gridView5.Rows)
            {
                Label hdrid = (Label)row.Cells[1].FindControl("lbl_hdrid");
                Label lgrid = (Label)row.Cells[1].FindControl("lbl_lgrid");
                Label feecat = (Label)row.Cells[1].FindControl("lbl_feecat");
                Label feeamt = (Label)row.Cells[1].FindControl("lbl_feeamt");
                Label totamt = (Label)row.Cells[1].FindControl("lbl_totamt");
                Label concession = (Label)row.Cells[1].FindControl("lbl_Concess");
                TextBox paid = (TextBox)row.Cells[1].FindControl("txt_paid");
                TextBox txtToBePaid = (TextBox)row.Cells[1].FindControl("txt_tobePaid");
                TextBox balance = (TextBox)row.Cells[1].FindControl("txt_bal");
                TextBox excess = (TextBox)row.Cells[1].FindControl("txt_exGrid2");

                double feeAmt = 0;
                double totalAmt = 0;
                double concsAmt = 0;
                double paidAmt = 0;
                double toBePaidAmt = 0;
                double excessAmt = 0;
                double balAmt = 0;

                double.TryParse(Convert.ToString(feeamt.Text), out  feeAmt);
                double.TryParse(Convert.ToString(totamt.Text), out  totalAmt);
                double.TryParse(Convert.ToString(concession.Text), out  concsAmt);
                double.TryParse(Convert.ToString(paid.Text), out  paidAmt);
                double.TryParse(Convert.ToString(txtToBePaid.Text), out  toBePaidAmt);
                double.TryParse(Convert.ToString(excess.Text), out  excessAmt);
                double.TryParse(Convert.ToString(balance.Text), out  balAmt);
                if (toBePaidAmt == 0)
                    continue;
                balAmt = totalAmt - paidAmt;
                #region journal old
                //else
                //{

                //    string updateFeeallot = "if exists (select * from FT_FeeAllot where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "')  and App_No in('" + appno + "')) update FT_FeeAllot set FeeAmount='" + feeamt.Text + "',TotalAmount='" + totamt.Text + "', BalAmount='" + balance.Text + "',paidamount=isnull(paidamount,'0')+'" + paid.Text + "' where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "')  and App_No in('" + appno + "') else   INSERT INTO FT_FeeAllot (AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK,paidamount) VALUES('" + transdate.ToString("MM/dd/yyyy") + "',1," + appno + ", " + lgrid.Text + "," + hdrid.Text + ",'" + feeamt.Text + "','0','0','0','" + totamt.Text + "','0','1','','1','" + feecat.Text + "','','0','','0','0','" + balance.Text + "'," + finYearid + ",'" + paid.Text + "')";//and  FinYearFK='" + finYearid + "'  and  FinYearFK='" + finYearid + "'
                //    d2.update_method_wo_parameter(updateFeeallot, "Text");

                //    updateOK = true;

                //}
                #endregion
                if (paidAmt != 0)
                {
                    #region journal
                    string updateFeeallot = "if exists (select * from FT_FeeAllot where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "')  and App_No in('" + appno + "') and isnull(IsTransfer,'0')='0') update FT_FeeAllot set FeeAmount='" + feeAmt + "',TotalAmount='" + totalAmt + "', BalAmount='" + balAmt + "',paidamount='" + paidAmt + "' where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "')  and App_No in('" + appno + "') and isnull(IsTransfer,'0')='0' else   INSERT INTO FT_FeeAllot (AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK,paidamount) VALUES('" + transdate.ToString("MM/dd/yyyy") + "',1," + appno + ", " + lgrid.Text + "," + hdrid.Text + ",'" + feeAmt + "','0','0','0','" + totalAmt + "','0','1','','1','" + feecat.Text + "','','0','','0','0','" + balAmt + "'," + finYearid + ",'" + paidAmt + "')";//and  FinYearFK='" + finYearid + "'  and  FinYearFK='" + finYearid + "'
                    string feeallot = d2.GetFunction("select feeallotpk from FT_FeeAllot where LedgerFK in('" + lgrid.Text + "') and HeaderFK in('" + hdrid.Text + "') and FeeCategory in('" + feecat.Text + "')  and App_No in('" + appno + "') and isnull(IsTransfer,'0')='0'");//added by abarna
                    if (Convert.ToInt64(feeallot) > 0)
                    {
                        string monthWiseQ = " select isnull(AllotAmount,0) as AllotAmt,isnull(PaidAmount,0) as PaidAmount,isnull(AllotAmount,0)-isnull(PaidAmount,0) as BalAmount,AllotMonth,AllotYear,(select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend))) as finyearfk FROM fm_finyearmaster fm where a.finyearfk=fm.finyearpk )as finyear,a.finyearfk from FT_FeeallotMonthly a where FeeAllotPK=" + feeallot + " and balamount>0";
                        DataSet dsMonWiseDet = d2.select_method_wo_parameter(monthWiseQ, "Text");
                        if (dsMonWiseDet.Tables.Count > 0 && dsMonWiseDet.Tables[0].Rows.Count > 0)
                        {
                            for (int mon = 0; mon < dsMonWiseDet.Tables[0].Rows.Count; mon++)
                            {

                                updateFeeallot += "update ft_feeallotmonthly set paidamount='" + toBePaidAmt + "',balamount=allotamount-'" + toBePaidAmt + "' where feeallotpk='" + feeallot + "' and  balamount>0";
                                //Added on 04-06-2016

                            }

                        }
                    }
                    d2.update_method_wo_parameter(updateFeeallot, "Text");


                    updateOK = true;
                    #endregion
                }
            }

            #endregion
        }
        catch { }
        return updateOK;
    }

    //add fees 
    protected void btnAddFees_Click(object sender, EventArgs e)
    {
        addFees();
    }
    protected void addFees()
    {
        try
        {
            double total = 0;
            double balance = 0;
            double paid = 0;
            string hdFK = Convert.ToString(ddlhedind.SelectedValue);
            string hdFKText = Convert.ToString(ddlhedind.SelectedItem.Text);
            string ldFK = Convert.ToString(ddlledind.SelectedValue);
            string ldFKText = Convert.ToString(ddlledind.SelectedItem.Text);
            string feecat = Convert.ToString(ddlsem.SelectedValue);
            string feecatText = Convert.ToString(ddlsem.SelectedItem.Text);
            double payAmount = 0;
            double paidAmount = 0;
            double balamount = 0;
            double.TryParse(Convert.ToString(txtamtind.Text), out payAmount);
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(hdFK) && !string.IsNullOrEmpty(ldFK) && !string.IsNullOrEmpty(feecat) && payAmount != 0 && gridView5.Rows.Count > 0)
            {
                dt.Columns.Add("Sno");
                dt.Columns.Add("YearSem");
                dt.Columns.Add("Header");
                dt.Columns.Add("HeaderFk");
                dt.Columns.Add("Ledger");
                dt.Columns.Add("LedgerFk");
                dt.Columns.Add("FeeCategory");
                dt.Columns.Add("Concession");
                dt.Columns.Add("Paid");
                dt.Columns.Add("hiddenTempAmt");
                dt.Columns.Add("tobePaid");
                dt.Columns.Add("Balance");
                dt.Columns.Add("Total");
                dt.Columns.Add("FeeAmt");
                DataRow dr;
                //additional fees added here

                int rowCnt = 1;
                foreach (GridViewRow gdRow in gridView5.Rows)//old record added here
                {
                    Label lblSemText = (Label)gdRow.FindControl("lbl_yearsem");
                    Label lblSemValue = (Label)gdRow.FindControl("lbl_feecat");
                    Label lblHd = (Label)gdRow.FindControl("lbl_hdr");
                    Label lblHdFk = (Label)gdRow.FindControl("lbl_hdrid");
                    Label lblld = (Label)gdRow.FindControl("lbl_lgr");
                    Label lblldFK = (Label)gdRow.FindControl("lbl_lgrid");
                    Label lblFeeAmt = (Label)gdRow.FindControl("lbl_feeamt");
                    Label lblTotAmt = (Label)gdRow.FindControl("lbl_totamt");
                    Label lblConsAmt = (Label)gdRow.FindControl("lbl_Concess");
                    TextBox lblPaidAmt = (TextBox)gdRow.FindControl("txt_paid");
                    TextBox lblBalAmt = (TextBox)gdRow.FindControl("txt_bal");
                    TextBox lblExcessAmt = (TextBox)gdRow.FindControl("txt_exGrid2");
                    double feeamt = 0;
                    double totalAmt = 0;
                    double paidAMt = 0;
                    double balAMt = 0;
                    double.TryParse(Convert.ToString(lblFeeAmt.Text), out feeamt);
                    double.TryParse(Convert.ToString(lblTotAmt.Text), out totalAmt);
                    double.TryParse(Convert.ToString(lblPaidAmt.Text), out paidAMt);
                    double.TryParse(Convert.ToString(lblBalAmt.Text), out balAMt);
                    paid += paidAMt;
                    balance += balAMt;
                    if (hdFK == lblHdFk.Text && ldFK == lblldFK.Text && feecat == lblSemValue.Text && payAmount != 0)
                    {
                        feeamt += payAmount;
                        totalAmt += payAmount;
                        payAmount = 0;
                    }

                    dr = dt.NewRow();
                    dr["Sno"] = ++rowCnt;
                    dr["YearSem"] = Convert.ToString(lblSemText.Text);
                    dr["FeeCategory"] = Convert.ToString(lblSemValue.Text);
                    dr["Header"] = Convert.ToString(lblHd.Text);
                    dr["HeaderFk"] = Convert.ToString(lblHdFk.Text);
                    dr["Ledger"] = Convert.ToString(lblld.Text);
                    dr["LedgerFk"] = Convert.ToString(lblldFK.Text);
                    dr["FeeAmt"] = Convert.ToString(feeamt);
                    dr["Total"] = Convert.ToString(totalAmt);
                    dr["Concession"] = Convert.ToString(lblConsAmt.Text);
                    dr["Paid"] = Convert.ToString(paidAMt);
                    dr["Balance"] = Convert.ToString(balAMt);
                    dr["hiddenTempAmt"] = "0";
                    dr["tobePaid"] = "0";

                    dt.Rows.Add(dr);
                    double tempTot = 0;
                    double.TryParse(Convert.ToString(totalAmt), out tempTot);
                    total += tempTot;
                }
                if (payAmount != 0)
                {
                    dr = dt.NewRow();
                    dr["Sno"] = 1;
                    dr["YearSem"] = feecatText;
                    dr["FeeCategory"] = feecat;
                    dr["Header"] = hdFKText;
                    dr["HeaderFk"] = hdFK;
                    dr["Ledger"] = ldFKText;
                    dr["LedgerFk"] = ldFK;
                    dr["FeeAmt"] = Convert.ToString(payAmount);
                    dr["Total"] = Convert.ToString(payAmount);
                    dr["Concession"] = "0.00";
                    dr["Paid"] = "0";
                    dr["Balance"] = "0";
                    dr["hiddenTempAmt"] = "0";
                    dr["tobePaid"] = "0";
                    dt.Rows.InsertAt(dr, 0);
                    total += payAmount;
                }
            }
            if (dt.Rows.Count > 0)
            {
                gridView5.DataSource = dt;
                gridView5.DataBind();
                Label8.Text = "Rs." + balance.ToString();
                Label6.Text = "Rs." + paid.ToString();
                Label5.Text = "Rs." + total.ToString();
                Label9.Text = "Rs.";
                // Label10.Text = "RS.";
                Table2.Visible = true;
            }
            else
            {
                gridView5.DataSource = null;
                gridView5.DataBind();
                Table2.Visible = false;
            }
        }
        catch { }
    }

    //added by sudhagar 29.08.2017
    protected void ddlJournalType_indexChanged(object sender, EventArgs e)
    {
        txt_rerollno_TextChanged(sender, e);

    }

    protected bool getValidate()
    {
        bool boolCheck = false;
        ArrayList arValidate = new ArrayList();
        foreach (GridViewRow gdrow in gridView4.Rows)
        {
            double balAmt = 0;
            CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
            if (cb.Checked)
            {
                Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                Label lblfeecat = (Label)gdrow.FindControl("lbl_feecat");
                Label transcode = (Label)gdrow.FindControl("lbl_feeamt");
                string allValue = lblhedg.Text + "-" + lblledg.Text + "-" + lblfeecat.Text + "-" + transcode.Text;
                if (!arValidate.Contains(allValue))
                {
                    arValidate.Add(allValue);
                }
            }
        }
        if (arValidate.Count == 1)
            boolCheck = true;
        return boolCheck;
    }
    //added by abarna 17.11.2017
    protected void rbl_rollnoNew_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbl_rollnoNew.Text == "Student")
        {
            div_refund.Visible = true;
            otherdiv.Visible = false;
            rcptSngleStaff.Visible = false;
            rcptSngleOthers.Visible = false;
            tbljournalStaff.Visible = false;
            lnkvendorjournalmaptable.Visible = false;
            lnkothersjournalmaptable.Visible = false;
            rcptSngleVendor.Visible = false;
            tbljournal.Visible = true;
            gridView5.Visible = true;
            btnAddRow.Visible = true;
            tdJournalType.Visible = true;
            ftype.Visible = true;
            LblRefund_staffid.Visible = false;
            txtRefund_staffid.Visible = false;
            LblRefund_staffName.Visible = false;
            txtRefund_staffName.Visible = false;
            LblRefund_staffCode.Visible = false;
            txtRefund_staffDept.Visible = false;
        }

        if (rbl_rollnoNew.Text == "Staff")
        {
            div_refund.Visible = false;
            rcptSngleStaff.Visible = false;
            rcptSngleOthers.Visible = false;
            personmode = 1;
            txtroll_staff_Changed(sender, e);
            tbljournalStaff.Visible = true;
            otherdiv.Visible = false;
            rcptSngleVendor.Visible = false;
            tbljournal.Visible = false;
            lnkvendorjournalmaptable.Visible = false;
            lnkothersjournalmaptable.Visible = false;
            ftype.Visible = true;
            tdJournalType.Visible = true;

        }
        if (rbl_rollnoNew.Text == "Others")
        {
            div_refund.Visible = false;
            otherdiv.Visible = true;
            rcptSngleOthers.Visible = true;
            rcptSngleStaff.Visible = false;
            rcptSngleVendor.Visible = false;
            personmode = 3;
            txtroll_other_Changed(sender, e);
            //  tbljournalStaff.Visible = true;
            tbljournalStaff.Visible = false;
            // bindgridjournalother();
            tbljournal.Visible = false;
            lnkvendorjournalmaptable.Visible = false;
            lnkothersjournalmaptable.Visible = true;
            othervendor.Visible = true;
            tdJournalType.Visible = false;
            ftype.Visible = true;

        }
        if (rbl_rollnoNew.Text == "Vendor")
        {
            div_refund.Visible = false;
            otherdiv.Visible = false;
            rcptSngleOthers.Visible = false;
            rcptSngleStaff.Visible = false;
            rcptSngleVendor.Visible = true;
            personmode = 3;
            txtroll_vendor_Changed(sender, e);
            //  tbljournalStaff.Visible = true;
            tbljournalStaff.Visible = false;
            //bindgridjournalother();
            tbljournal.Visible = false;
            lnkvendorjournalmaptable.Visible = true;
            lnkothersjournalmaptable.Visible = false;
            //tdJournalType.Visible = false;
            tdJournalType.Visible = false;
            ftype.Visible = true;

        }


    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetVendorno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            //staff query
            query = " select VendorCompName,VendorPK  from CO_VendorMaster where VendorType =1";//+'-'+VendorCode+'-'+Convert(varchar(10),vendorpk)

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetVendorName(string prefixText)
    {
        WebService ws = new WebService();
        string query = " select (VenContactName+'-'+VenContactDesig+'-'+ CONVERT(varchar(10), VendorContactPK)) as contactname from IM_VendorContactMaster where VendorFK ='" + vencontcode + "' ";
        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetVendorno1(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            //staff query
            query = " select VendorCode  from CO_VendorMaster where VendorType =1  order by VendorCode asc";

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetVendorName1(string prefixText)
    {
        WebService ws = new WebService();
        string query = " select VendorCompName  from CO_VendorMaster where VendorType =1 order by VendorCompName asc";
        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }
    //Staff Division
    protected void txtroll_staff_Changed(object sender, EventArgs e)
    {
        string name = string.Empty;
        string degree = string.Empty;
        string college = string.Empty;
        string staffId = Convert.ToString(txtroll_staff.Text.Trim());
        //img_stud.ImageUrl = "";
        //img_stud.Visible = false;

        if (staffId != "")
        {
            if (rbl_rollnoNew.Text == "Staff")
            {
                //string name = string.Empty;
                //string degree = string.Empty;

                // string query = "select staff_name,appl_no,ISNULL( Stream,'') as type from staffmaster where resign<>1 and college_code="+collegecode1+" and staff_code='" + staffId + "'";

                string query = " select appl_id ,h.dept_name,h.dept_code,s.staff_name,s.staff_code,c.collname  from collinfo c,staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code and s.staff_code ='" + staffId + "' and s.college_Code in('" + stcollegecode + "') ";

                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");


                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            name = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                            degree = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                            //lbltype.Text = Convert.ToString(ds.Tables[0].Rows[i]["type"]);      
                            college = Convert.ToString(ds.Tables[0].Rows[i]["collname"]);
                        }
                    }
                }

                txtname_staff.Text = name;
                txtDept_staff.Text = degree;

                //img_stud.ImageUrl = "~/Handler/ghStaffFoto.ashx?QSstaff_id=" + staffId;
                //img_stud.Visible = true;
            }
        }


    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            //staff query
            query = " select staff_code from staffmaster where resign<>1 and staff_code like '" + prefixText + "%' and college_code='" + stcollegecode + "' order by staff_code asc";


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
        string query = " select top 100 staff_name+'-'+staff_code from staffmaster where resign<>1 and staff_name like '" + prefixText + "%' and college_code='" + stcollegecode + "'  order by staff_name asc";
        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }
    protected void staffjournal_Click(object sender, EventArgs e)
    {
        divindi.Visible = true;
        btnupdatestaff.Visible = true;
        div_refund.Visible = false;
        inclAddAmt.Visible = false;
        staffadd.Visible = true;
        bindGridJournalAdvance();
        //bindGridAllotJournal();
        gridView5.Visible = false;
        gd5.Visible = false;
        div5.Visible = true;
        divtblOne.Visible = true;
        incJournal.Visible = true;
        btntransind.Visible = false;
        gridView1.Visible = true;
        btnAddRow.Visible = true;
        bindGrid();
        savebutton.Visible = true;
        othervendor.Visible = false;
        div4.Visible = false;
        vendorothersave.Visible = false;
        Label25.Visible = false;

        transCodetext.Visible = false;
        //  divtblOne.Visible = true;
        // savebutton.Visible = true;
    }
    protected void Othersjournal_Click(object sender, EventArgs e)
    {
        divindi.Visible = true;
        div_refund.Visible = false;
        inclAddAmt.Visible = false;
        staffadd.Visible = true;
        //bindGridJournalAdvance();
        //bindGridAllotJournal();

        bindgridjournalother();
        gridView1.Visible = true;
        incJournal.Visible = true;
        btntransind.Visible = false;

        bindGrid();
        gridView4.Visible = false;
        gridView5.Visible = false;
        gd5.Visible = false;
        othervendor.Visible = true;
        div4.Visible = true;
        div5.Visible = false;
        savebutton.Visible = true;
        divtblOne.Visible = false;
        Label25.Visible = false;

        transCodetext.Visible = false;
        Table3.Visible = true;
        vendorothersave.Visible = true;
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
    protected void gridView1_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        // lbl_er.Visible = false;
        string strdname = "";
        //if (ddl_collegename.Items.Count > 0)
        //{
        //    collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
        //}
        int n = Convert.ToInt32(e.CommandArgument);
        //DropDownList strhdname = (DropDownList)gridView1.Rows[n].FindControl("ddl_headername");
        //(gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Clear();
        //// string englisquery = "select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode='" + collegecode1 + "' and LedgerMode=1 and HeaderFK ='" + strhdname.SelectedItem.Value + "' order by isnull(priority,1000),ledgerName asc";

        //string englisquery = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + "   and L.CollegeCode = " + stcollegecode + "  and LedgerMode=1 and l.HeaderFK ='" + strhdname.SelectedItem.Value + "'   order by isnull(l.priority,1000), l.ledgerName asc ";
        //ds.Clear();
        //ds = d2.select_method_wo_parameter(englisquery, "Text");
        //if (ds.Tables[0].Rows.Count > 0)
        //{

        //    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataSource = ds;
        //    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataTextField = "LedgerName";
        //    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataValueField = "LedgerPK";
        //    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataBind();

        //    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");
        //    // lbl_er.Visible = false;
        //}
        //else
        //{
        //    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");
        //}
    }
    protected void Marksgrid_pg_DataBound(object sender, EventArgs e)
    {
        string linkName = string.Empty;
        try
        {
            //if (ddl_collegename.Items.Count > 0)
            //{
            //    collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            //}
            (gridView1.Rows[0].FindControl("ddl_headername") as DropDownList).Items.Clear();
            (gridView1.Rows[0].FindControl("ddl_ledgername") as DropDownList).Items.Clear();
            if (gridView1.Rows.Count > 0)
            {
                // lbl_er.Visible = false;
                for (int a = 0; a < gridView1.Rows.Count; a++)
                {
                    // string englisquery = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "'";

                    string englisquery = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + stcollegecode + "  ";
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

                    englisquery = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + stcollegecode + "  order by isnull(l.priority,1000), l.ledgerName asc ";
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
                    //DataSet dsFee = d2.loadFeecategory(stcollegecode, usercode, ref linkName);
                    //if (dsFee.Tables.Count > 0 && dsFee.Tables[0].Rows.Count > 0)
                    //{
                    //    (gridView1.Rows[a].FindControl("ddlFeecat") as DropDownList).DataSource = dsFee;
                    //    (gridView1.Rows[a].FindControl("ddlFeecat") as DropDownList).DataTextField = "TextVal";
                    //    (gridView1.Rows[a].FindControl("ddlFeecat") as DropDownList).DataValueField = "TextCode";
                    //    (gridView1.Rows[a].FindControl("ddlFeecat") as DropDownList).DataBind();
                    //}
                    //(gridView1.Rows[a].FindControl("ddlFeecat") as DropDownList).Items.Insert(0, "Select");

                }
                //  div_cash.Visible = true;
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
                // DropDownList box3 = new DropDownList();
                TextBox box4 = new TextBox();
                for (int i = 0; i < gridView1.Rows.Count; i++)
                {
                    box1 = (DropDownList)gridView1.Rows[i].FindControl("ddl_headername");
                    box2 = (DropDownList)gridView1.Rows[i].FindControl("ddl_ledgername");
                    //box3 = (DropDownList)gridView1.Rows[i].FindControl("ddlFeecat");
                    box4 = (TextBox)gridView1.Rows[i].FindControl("txtAmt");
                    string dfg = box4.Text;
                    if (box1.Items.Count > 0 && box2.Items.Count > 0)
                    {
                        if (box1.SelectedItem.ToString() != "" && box2.SelectedItem.ToString() != "" && box4.Text != null)
                        {
                            if (box1.Text != "" && box2.Text != "" && !string.IsNullOrEmpty(box4.Text))
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
            DropDownList box1 = new DropDownList();
            DropDownList box2 = new DropDownList();
            //DropDownList box3 = new DropDownList();
            TextBox box4 = new TextBox();


            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    box1 = (DropDownList)gridView1.Rows[i].Cells[1].FindControl("ddl_headername");
                    box2 = (DropDownList)gridView1.Rows[i].Cells[2].FindControl("ddl_ledgername");
                    //box3 = (DropDownList)gridView1.Rows[i].Cells[3].FindControl("ddlFeecat");
                    box4 = (TextBox)gridView1.Rows[i].Cells[3].FindControl("txtAmt");
                    //  drCurrentRow["RowNumber"] = i + 1;
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows[i][0] = Convert.ToString(i + 1);
                    dtCurrentTable.Rows[i][1] = box1.Text;
                    dtCurrentTable.Rows[i][2] = box2.Text;
                    //dtCurrentTable.Rows[i][3] = box3.Text;
                    dtCurrentTable.Rows[i][3] = box4.Text;
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
                // DropDownList box3 = new DropDownList();

                TextBox box4 = new TextBox();
                Label lbl = new Label();

                hashlist.Add(0, "Sno");
                hashlist.Add(1, "Header Name");
                hashlist.Add(2, "Ledger Name");
                // hashlist.Add(3, "Feecategory");
                hashlist.Add(4, "Amount");

                DataRow dr;

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    box1 = (DropDownList)gridView1.Rows[i].Cells[1].FindControl("ddl_headername");
                    box2 = (DropDownList)gridView1.Rows[i].Cells[2].FindControl("ddl_ledgername");
                    //box3 = (DropDownList)gridView1.Rows[i].Cells[3].FindControl("ddlFeecat");
                    box4 = (TextBox)gridView1.Rows[i].Cells[3].FindControl("txtAmt");
                    //lbl = (Label)gridView1.Rows[i].Cells[4].FindControl("lbl_rs");
                    string val_file = Convert.ToString(hashlist[i]);
                    lbl.Text = Convert.ToString(i + 1);
                    //  ddlBatch_year.SelectedIndex = ddlBatch_year.Items.IndexOf(ddlBatch_year.Items.FindByText(Convert.ToString(Batch_year)));
                    string hedid = dt.Rows[i][1].ToString();
                    string ledgid = dt.Rows[i][2].ToString();
                    //string feecat = dt.Rows[i][3].ToString();
                    box1.SelectedIndex = box1.Items.IndexOf(box1.Items.FindByValue(Convert.ToString(dt.Rows[i][1])));
                    gridledgerload(hedid, i);
                    box2.SelectedIndex = box2.Items.IndexOf(box2.Items.FindByValue(Convert.ToString(dt.Rows[i][2])));
                    //box3.SelectedIndex = box3.Items.IndexOf(box3.Items.FindByValue(Convert.ToString(dt.Rows[i][3])));
                    // box1.Text = dt.Rows[i][1].ToString();
                    //  box2.Text = dt.Rows[i][2].ToString();
                    box4.Text = dt.Rows[i][3].ToString();

                    rowIndex++;
                }
            }
        }
    }
    protected void gridledgerload(string hedid, int n)
    {
        try
        {
            //lbl_er.Visible = false;
            string strdname = "";
            //if (ddl_collegename.Items.Count > 0)
            //{
            //    collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            //}
            string englisquery = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + "   and L.CollegeCode = " + stcollegecode + "  and l.HeaderFK ='" + hedid + "'   order by isnull(l.priority,1000), l.ledgerName asc ";

            ds.Clear();
            ds = d2.select_method_wo_parameter(englisquery, "Text");
            if (gridView1.Rows.Count > n)
            {
                (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {

                    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataSource = ds;
                    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataTextField = "LedgerName";
                    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataValueField = "LedgerPK";
                    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).DataBind();
                    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");
                    //lbl_er.Visible = false;
                }
                else
                {
                    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Clear();
                    (gridView1.Rows[n].FindControl("ddl_ledgername") as DropDownList).Items.Insert(0, "Select");
                }
            }
        }
        catch { }
    }
    public void bindGrid()
    {
        ArrayList addnew = new ArrayList();
        addnew.Add("1");
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("Header Name");
        dt.Columns.Add("Ledger Name");
        dt.Columns.Add("FeeCategory");
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

    public int rowIndxClicked()
    {
        int rownumber = -1;
        try
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[1].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }
        return rownumber;
    }

    protected void ddl_headername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int idx = row.RowIndex;
            if (gridView1.Rows.Count > idx)
            {
                if ((gridView1.Rows[idx].FindControl("ddl_headername") as DropDownList).Items.Count > 0)
                {
                    string headerId = ((gridView1.Rows[idx].FindControl("ddl_headername") as DropDownList).SelectedValue);
                    gridledgerload(headerId, idx);
                }
            }

        }
        catch
        {
        }

    }
    protected void txtroll_other_Changed(object sender, EventArgs e)
    {
        // btn_print.Visible = false;
        //try
        //{
        //txt_otherMobile.Text = "";//    txt_otherMobile.Text = txtroll_other.Text.Split('-')[1].Trim();
        //    txtroll_other.Text = txtroll_other.Text.Split('-')[0].Trim();
        //}
        //catch { }

        string staffId = Convert.ToString(txtroll_other.Text.Trim());
        // string staffMob = Convert.ToString(txt_otherMobile.Text.Trim());
        //  img_stud.ImageUrl = "";
        // img_stud.Visible = false;

        if (staffId != "")//&& staffMob != ""
        {
            //string ifAlreadyExist = d2.GetFunction("select VendorCode from co_vendormaster where vendorname='" + staffId + "' and VendorMobileNo='" + staffMob + "'  and VendorType=-5").Trim();

            string name = string.Empty;
            string compname = string.Empty;
            string Add1 = string.Empty;
            string Add2 = string.Empty;
            string mobiNo = string.Empty;
            //if (ifAlreadyExist == "0")
            //{
            string query = " select VendorName,VendorMobileNo,VendorCode,VendorAddress+'-'+VendorStreet as Add1,VendorCity,VendorCompName from co_vendormaster where vendorname='" + staffId + "'  and VendorType=-5";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                name = Convert.ToString(ds.Tables[0].Rows[0]["VendorName"]);
                compname = Convert.ToString(ds.Tables[0].Rows[0]["VendorCompName"]);
                Add1 = Convert.ToString(ds.Tables[0].Rows[0]["Add1"]);
                Add2 = Convert.ToString(ds.Tables[0].Rows[0]["VendorCity"]);
                mobiNo = Convert.ToString(ds.Tables[0].Rows[0]["VendorMobileNo"]);
            }

            txtname_other.Text = compname;
            txtAdd1_Other.Text = Add1;
            txtAdd2_Other.Text = Add2;
            txt_otherMobile.Text = mobiNo;
            //}

            //   img_stud.ImageUrl = "~/Handler/ghStaffFoto.ashx?QSstaff_id=" + staffId;
            // img_stud.Visible = true;

            // LoadYearSemester();
            //  Txt_amt.Text = "0.00";
            // loadGridOthers();
            //bindGrid();

        }

    }

    protected void Vendor_Click(object sender, EventArgs e)
    {
        divindi.Visible = true;
        div4.Visible = false;
        div_refund.Visible = false;
        inclAddAmt.Visible = false;
        staffadd.Visible = true;
        //  bindGridJournalAdvance();
        //bindGridAllotJournal();
        bindgridjournalother();
        gridView5.Visible = false;
        gd5.Visible = false;
        gridView4.Visible = true;
        div5.Visible = false;
        incJournal.Visible = true;
        btntransind.Visible = false;
        gridView1.Visible = true;
        //  gridView1.Visible = true;
        savebutton.Visible = true;
        bindGrid();
        vendorothersave.Visible = true;
        othervendor.Visible = true;
        div4.Visible = true;
        Label25.Visible = false;

        transCodetext.Visible = false;
    }
    protected void txtroll_vendor_Changed(object sender, EventArgs e)
    {
        if (txtroll_vendor.Text.Trim() != "")
        {
            // string staffid = Convert.ToString(txtname_staff.Text);

            // if (staffid != "")
            // {
            //     try
            //     {
            //         staffid = staffid.Split('-')[1];
            //     }
            //     catch { staffid = ""; }
            // }
            //// txtroll_staff.Text = staffid;

            txtname_vendor_Changed(sender, e);
            //imgAlert.Visible = false;
        }



    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetOthername(string prefixText)
    {
        WebService ws = new WebService();
        //string query = " select vendorname+'-'+VendorMobileNo from co_vendormaster where vendorname like '" + prefixText + "%' and VendorType=-5";
        string query = " select vendorname from co_vendormaster where vendorname like '" + prefixText + "%' and VendorType=-5";
        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }
    protected void txtname_vendor_Changed(object sender, EventArgs e)
    {
        //btn_print.Visible = false;
        if (txtroll_vendor.Text.Trim() == "")
        {
            //txtname_vendor.Text = "";
        }
        string staffId = Convert.ToString(txtroll_vendor.Text.Trim());
        //try
        //{

        //    staffId = staffId.Split('-')[2];
        //}
        //catch { staffId = ""; }
        //img_stud.ImageUrl = "";
        //img_stud.Visible = false;

        if (staffId != "")
        {
            if (rbl_rollnoNew.Text == "Vendor")
            {

                string name = string.Empty;
                string degree = string.Empty;
                string vendorpk = d2.GetFunction(" select VendorPK  from CO_VendorMaster where VendorType =1 and VendorCompName='" + txtroll_vendor.Text.Trim() + "'");
                string query = " SELECT VendorContactPK, VenContactType, VenContactName, VenContactDesig, VenContactDept, VendorPhoneNo, VendorExtNo, VendorMobileNo, VendorEmail,VendorFK FROM IM_VendorContactMaster WHERE    VendorContactPK = '" + vendorpk + "' ";
                //string query=" select * from IM_VendorContactMaster
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");


                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            name = Convert.ToString(ds.Tables[0].Rows[i]["VenContactName"]);
                            degree = Convert.ToString(ds.Tables[0].Rows[i]["VenContactDesig"]);
                            //lbltype.Text = Convert.ToString(ds.Tables[0].Rows[i]["type"]);                            
                        }
                    }
                }

                // txtname_staff.Text = name;
                txtDept_vendor.Text = degree;
                txtname_vendor.Text = name;
                //img_stud.ImageUrl = "~/Handler/ghStaffFoto.ashx?QSstaff_id=" + staffId;
                //img_stud.Visible = true;

                //LoadYearSemester();
                //Txt_amt.Text = "0.00";
                //loadGridVendor();
                //bindGrid();
            }

        }


    }

    protected void btnupdatestaff_Click(object sender, EventArgs e)
    {
        string Amount = string.Empty;
        if (rbl_rollnoNew.Text == "Staff")
        {
            string Rcptno = string.Empty;
            string finYearid = string.Empty;
            string entryUserCode = string.Empty;
            string app_no = "";
            int inst = 0;
            string INSdaily = string.Empty;
            finYearid = d2.getCurrentFinanceYear(usercode, ddlcollege.SelectedValue);


            string tempCollegecode = string.Empty;
            tempCollegecode = Convert.ToString(ddlcollege.SelectedValue);
            DateTime transdate = Convert.ToDateTime(txt_rdate.Text.Trim().Split('/')[1] + "/" + txt_rdate.Text.Trim().Split('/')[0] + "/" + txt_rdate.Text.Trim().Split('/')[2]);
            foreach (GridViewRow gdrow in gridView4.Rows)
            {
                CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
                if (cb.Checked)
                {
                    Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                    Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                    // Label lblfeecat = (Label)gdrow.FindControl("lbl_feecat");
                    Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                    Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                    Label lblbal = (Label)gdrow.FindControl("lbl_bal");
                    Label transcode = (Label)gdrow.FindControl("lbl_feeamt");
                    Label transdt = (Label)gdrow.FindControl("lbl_totamt");
                    TextBox Narration = (TextBox)gdrow.FindControl("txt_Narration");

                    string preTransdt = transdt.Text.Split('/')[1] + "/" + transdt.Text.Split('/')[0] + "/" + transdt.Text.Split('/')[2];
                    //string INSdaily = "insert into FT_FinDailyTransaction (TransCode,MemType,LedgerFK,HeaderFK,FeeCategory,entryusercode,Transtype,narration,receipttype) values('" + transcode + "','1','" + lblledg.Text + "','" + lblhedg.Text + "','" + lblfeecat.Text + "','0','1','1','1','3','" + txtnaration.Text.Trim() + "','6')";
                    INSdaily = "update FT_FinDailyTransaction set narration='" + Narration.Text.Trim() + "' where HeaderFK='" + lblhedg.Text + "'and LedgerFK='" + lblledg.Text + "' and TransCode='" + transcode.Text + "'";
                    inst = d2.update_method_wo_parameter(INSdaily, "Text");

                    string deleteqry = "delete from FT_FinDailyTransaction where TransCode='" + transcode.Text + "' and debit='" + lblpaid.Text + "' and HeaderFK='" + lblhedg.Text + "'and LedgerFK='" + lblledg.Text + "'";// and debit='"++"'

                    int dltres = d2.update_method_wo_parameter(deleteqry, "Text");

                    foreach (GridViewRow gdrow1 in gridView1.Rows)
                    {
                        DropDownList box1 = (DropDownList)gdrow1.FindControl("ddl_headername");
                        DropDownList box2 = (DropDownList)gdrow1.FindControl("ddl_ledgername");

                        TextBox box4 = (TextBox)gdrow1.FindControl("txtAmt");

                        app_no = d2.GetFunction(" select appl_id from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code and s.staff_code ='" + txtroll_staff.Text.Trim() + "' and s.college_Code in('" + ddlcollege.SelectedValue + "')");

                        string insertqry = "insert into  FT_FinDailyTransaction (HeaderFK,LedgerFK,debit,app_no,TransCode,MemType,TransDate,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype,FinYearFK) values(" + box1.Text + "," + box2.Text + "," + box4.Text + "," + app_no + ",'" + transcode.Text + "','2','" + transdate + "','0','1','1','1','" + usercode + "','3','" + finYearid + "') ";
                        //insertqry += "insert into  FT_FinDailyTransaction (HeaderFK,LedgerFK,credit,app_no,TransCode,MemType,TransDate,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype,FinYearFK) values(" + box1.Text + "," + box2.Text + "," + box4.Text + "," + app_no + ",'" + transcode.Text + "','2','" + transdate + "','0','1','1','1','" + usercode + "','3','" + finYearid + "') ";
                        insertqry += "update ft_feeallot set paidamount=" + box4.Text + ",balamount=0,feeamount=" + box4.Text + ",totalamount=" + box4.Text + " where app_no='" + app_no + "'";
                        inst = d2.update_method_wo_parameter(insertqry, "Text");


                        //transferReceiptJournal("Journal", app_no, tempCollegecode, transdate.ToString("MM/dd/yyyy"), Rcptno);
                        Amount = box4.Text;
                    }
                    //disTransClear();
                    //divindi.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Update Successfully')", true);
                    Rcptno = transcode.Text;
                    transferReceiptJournal("Journal", app_no, tempCollegecode, transdate.ToString("MM/dd/yyyy"), Rcptno);

                    //StaffClear();

                    //==========================Added by Saranya on 10/04/2018=============================//
                    int savevalue = 2;
                    string entrycode = Session["Entry_Code"].ToString();
                    string formname = "Journal";
                    string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string doa = DateTime.Now.ToString("MM/dd/yyy");
                    IPHostEntry host;
                    string localip = "";
                    host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (IPAddress ip in host.AddressList)
                    {
                        if (ip.AddressFamily.ToString() == "InterNetwork")
                        {
                            localip = ip.ToString();
                        }
                    }
                    string details = "Staff code -" + txt_rerollno.Text + " : ReceiptNO -" + Rcptno + " :Amount -" + Amount + " : Date - " + toa + " ";//:Allot Ledger-" +ledgername + ":Allot Amt-" +allotamt +"";
                    string modules = "0";
                    string act_diff = " ";
                    string ctsname = "";
                    if (savevalue == 2)
                    {
                        ctsname = "Journal Update";
                    }
                    string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                    // Console.WriteLine(hostName);
                    // Get the IP  
                    string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                    int a = d2.update_method_wo_parameter(strlogdetails, "Text");
                    //============================================================================//
                }
            }



        }
        if (rbl_rollnoNew.Text == "Others")
        {
            string Rcptno = string.Empty;
            string finYearid = string.Empty;
            string app_no = "";
            int inst = 0;
            string INSdaily = string.Empty;
            finYearid = d2.getCurrentFinanceYear(usercode, ddlcollege.SelectedValue);

            string tempCollegecode = string.Empty;
            tempCollegecode = Convert.ToString(ddlcollege.SelectedValue);
            DateTime transdate = Convert.ToDateTime(txt_rdate.Text.Trim().Split('/')[1] + "/" + txt_rdate.Text.Trim().Split('/')[0] + "/" + txt_rdate.Text.Trim().Split('/')[2]);
            foreach (GridViewRow gdrow in othervendor.Rows)
            {
                CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");

                if (cb.Checked)
                {
                    Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                    Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                    Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                    Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                    Label lblbal = (Label)gdrow.FindControl("lbl_bal");
                    Label transcode = (Label)gdrow.FindControl("lbl_feeamt");
                    Label transdt = (Label)gdrow.FindControl("lbl_totamt");
                    TextBox Narration = (TextBox)gdrow.FindControl("txt_Narration");
                    TextBox vendorcode = (TextBox)gdrow.FindControl("txt_vendorcode");
                    TextBox vendorname = (TextBox)gdrow.FindControl("txt_vendorname");

                    string preTransdt = transdt.Text.Split('/')[1] + "/" + transdt.Text.Split('/')[0] + "/" + transdt.Text.Split('/')[2];

                    INSdaily = "update FT_FinDailyTransaction set narration='" + Narration.Text.Trim() + "' where HeaderFK='" + lblhedg.Text + "'and LedgerFK='" + lblledg.Text + "' and TransCode='" + transcode.Text + "'";
                    inst = d2.update_method_wo_parameter(INSdaily, "Text");

                    //string deleteqry = "delete from FT_FinDailyTransaction where TransCode='" + transcode.Text + "' and debit='" + lblpaid.Text + "' and HeaderFK='" + lblhedg.Text + "'and LedgerFK='" + lblledg.Text + "' ";
                    //int dltres = d2.update_method_wo_parameter(deleteqry, "Text");

                    foreach (GridViewRow gdrow1 in gridView1.Rows)
                    {
                        DropDownList box1 = (DropDownList)gdrow1.FindControl("ddl_headername");
                        DropDownList box2 = (DropDownList)gdrow1.FindControl("ddl_ledgername");
                        TextBox box4 = (TextBox)gdrow1.FindControl("txtAmt");
                        app_no = d2.GetFunction(" select VendorPK from co_vendormaster where VendorCode= '" + vendorcode.Text + "' and VendorType=-5");
                        // int inst = d2.update_method_wo_parameter(INSdaily, "Text");
                        //INSdaily += "update FT_FinDailyTransaction set HeaderFK='" + box1.Text + "',LedgerFK='" + box2.Text + "',debit='" + box4.Text.Trim() + "' where app_no='" + app_no + "' and TransCode='" + transcode.Text + "'";
                        //inst = d2.update_method_wo_parameter(INSdaily, "Text");

                        string insertqry = "insert into  FT_FinDailyTransaction (HeaderFK,LedgerFK,debit,app_no,TransCode,MemType,TransDate,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype,FinYearFK) values(" + box1.Text + "," + box2.Text + "," + box4.Text + "," + app_no + ",'" + transcode.Text + "','4','" + transdate + "','0','1','1','1','" + usercode + "','3','" + finYearid + "') ";
                        insertqry += "insert into  FT_FinDailyTransaction (HeaderFK,LedgerFK,credit,app_no,TransCode,MemType,TransDate,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype,FinYearFK) values(" + box1.Text + "," + box2.Text + "," + box4.Text + "," + app_no + ",'" + transcode.Text + "','4','" + transdate + "','0','1','1','1','" + usercode + "','3','" + finYearid + "') ";
                        inst = d2.update_method_wo_parameter(insertqry, "Text");
                        Amount = box4.Text;
                    }
                    //disTransClear();
                    //divindi.Visible = false;
                    //
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);

                    Rcptno = transcode.Text;
                    transferReceiptJournal("Journal", app_no, tempCollegecode, transdate.ToString("MM/dd/yyyy"), Rcptno);
                    StaffClear();
                    //divind.Visible = false;

                    //==========================Added by Saranya on 10/04/2018=============================//
                    int savevalue = 2;
                    string entrycode = Session["Entry_Code"].ToString();
                    string formname = "Journal";
                    string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string doa = DateTime.Now.ToString("MM/dd/yyy");
                    IPHostEntry host;
                    string localip = "";
                    host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (IPAddress ip in host.AddressList)
                    {
                        if (ip.AddressFamily.ToString() == "InterNetwork")
                        {
                            localip = ip.ToString();
                        }
                    }
                    string details = "vendor Code=" + txt_rerollno.Text + " : ReceiptNO -" + Rcptno + " : Amount -" + Amount + " : Date - " + toa + "";//:Allot Ledger-" +ledgername + ":Allot Amt-" +allotamt +"";
                    string modules = "0";
                    string act_diff = " ";
                    string ctsname = "";
                    if (savevalue == 2)
                    {
                        ctsname = "Journal Update";
                    }
                    string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                    // Console.WriteLine(hostName);
                    // Get the IP  
                    string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                    int a = d2.update_method_wo_parameter(strlogdetails, "Text");
                    //============================================================================//

                }
            }
        }
        if (rbl_rollnoNew.Text == "Vendor")
        {
            string Rcptno = string.Empty;
            string finYearid = string.Empty;
            string app_no = "";
            int inst = 0;
            string INSdaily = string.Empty;
            finYearid = d2.getCurrentFinanceYear(usercode, ddlcollege.SelectedValue);

            string tempCollegecode = string.Empty;
            tempCollegecode = Convert.ToString(ddlcollege.SelectedValue);
            DateTime transdate = Convert.ToDateTime(txt_rdate.Text.Trim().Split('/')[1] + "/" + txt_rdate.Text.Trim().Split('/')[0] + "/" + txt_rdate.Text.Trim().Split('/')[2]);
            foreach (GridViewRow gdrow in othervendor.Rows)
            {

                CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");

                if (cb.Checked)
                {
                    Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                    Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                    Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                    Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                    Label lblbal = (Label)gdrow.FindControl("lbl_bal");
                    Label transcode = (Label)gdrow.FindControl("lbl_feeamt");
                    Label transdt = (Label)gdrow.FindControl("lbl_totamt");
                    TextBox Narration = (TextBox)gdrow.FindControl("txt_Narration");
                    TextBox vendorcode = (TextBox)gdrow.FindControl("txt_vendorcode");
                    TextBox vendorname = (TextBox)gdrow.FindControl("txt_vendorname");

                    string preTransdt = transdt.Text.Split('/')[1] + "/" + transdt.Text.Split('/')[0] + "/" + transdt.Text.Split('/')[2];

                    INSdaily = "update FT_FinDailyTransaction set narration='" + Narration.Text.Trim() + "' where HeaderFK='" + lblhedg.Text + "'and LedgerFK='" + lblledg.Text + "' and TransCode='" + transcode.Text + "'";
                    inst = d2.update_method_wo_parameter(INSdaily, "Text");
                    string deleteqry = "delete from FT_FinDailyTransaction where TransCode='" + transcode.Text + "' and credit='" + lblpaid.Text + "' and HeaderFK='" + lblhedg.Text + "'and LedgerFK='" + lblledg.Text + "' ";
                    int dltres = d2.update_method_wo_parameter(deleteqry, "Text");

                    foreach (GridViewRow gdrow1 in gridView1.Rows)
                    {
                        DropDownList box1 = (DropDownList)gdrow1.FindControl("ddl_headername");
                        DropDownList box2 = (DropDownList)gdrow1.FindControl("ddl_ledgername");
                        TextBox box4 = (TextBox)gdrow1.FindControl("txtAmt");
                        app_no = d2.GetFunction(" select VendorPK from co_vendormaster where VendorCode= '" + vendorcode.Text + "' and VendorType=1");

                        // int inst = d2.update_method_wo_parameter(INSdaily, "Text");
                        //INSdaily += "update FT_FinDailyTransaction set HeaderFK='" + box1.Text + "',LedgerFK='" + box2.Text + "',debit='" + box4.Text.Trim() + "' where app_no='" + app_no + "' and TransCode='" + transcode.Text + "'";
                        //inst = d2.update_method_wo_parameter(INSdaily, "Text");
                        string insertqry = "insert into  FT_FinDailyTransaction(HeaderFK,LedgerFK,credit,app_no,TransCode,MemType,TransDate,IsCanceled,IsCollected,paymode,isdeposited,entryusercode,Transtype,FinYearFK) values(" + box1.Text + "," + box2.Text + "," + box4.Text + "," + app_no + ",'" + transcode.Text + "','3','" + transdate + "','0','1','1','1','" + usercode + "','3','" + finYearid + "') ";

                        inst = d2.update_method_wo_parameter(insertqry, "Text");
                        Amount = box4.Text;
                        //transferReceiptJournal("Journal", app_no, tempCollegecode, transdate.ToString("MM/dd/yyyy"), Rcptno);
                    }
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
                    Rcptno = transcode.Text;
                    transferReceiptJournal("Journal", app_no, tempCollegecode, transdate.ToString("MM/dd/yyyy"), Rcptno);
                    //disTransClear();
                    //divind.Visible = false;

                    //==========================Added by Saranya on 10/04/2018=============================//
                    int savevalue = 2;
                    string entrycode = Session["Entry_Code"].ToString();
                    string formname = "Journal";
                    string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string doa = DateTime.Now.ToString("MM/dd/yyy");
                    IPHostEntry host;
                    string localip = "";
                    host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (IPAddress ip in host.AddressList)
                    {
                        if (ip.AddressFamily.ToString() == "InterNetwork")
                        {
                            localip = ip.ToString();
                        }
                    }
                    string details = "Vendor code -" + txt_rerollno.Text + " : ReceiptNO -" + Rcptno + " : Amount -" + Amount + " : Date - " + toa + "";//:Allot Ledger-" +ledgername + ":Allot Amt-" +allotamt +"";
                    string modules = "0";
                    string act_diff = " ";
                    string ctsname = "";
                    if (savevalue == 2)
                    {
                        ctsname = "Journal Update";
                    }
                    string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                    // Console.WriteLine(hostName);
                    // Get the IP  
                    string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                    int a = d2.update_method_wo_parameter(strlogdetails, "Text");
                    //============================================================================//
                }
            }
        }


    }

    protected void StaffClear()
    {
        txtroll_staff.Text = "";
        txtname_staff.Text = "";
        txtDept_staff.Text = "";
        //image3.ImageUrl = "";
        tbljournalStaff.Visible = true;
        rcptSngleStaff.Visible = true;

    }

    protected void btnSaveStud_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gdrow in gridView4.Rows)
        {
            // double balAmt = 0;
            CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
            if (cb.Checked)
            {
                Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                Label lblfeecat = (Label)gdrow.FindControl("lbl_feecat");
                Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                Label lblbal = (Label)gdrow.FindControl("lbl_bal");
                Label transcode = (Label)gdrow.FindControl("lbl_feeamt");
                Label transdt = (Label)gdrow.FindControl("lbl_totamt");
                TextBox Narration = (TextBox)gdrow.FindControl("txt_Narration");

                //   string s = Narration.Text;


                string preTransdt = transdt.Text.Split('/')[1] + "/" + transdt.Text.Split('/')[0] + "/" + transdt.Text.Split('/')[2];
                //string INSdaily = "insert into FT_FinDailyTransaction (TransCode,MemType,LedgerFK,HeaderFK,FeeCategory,entryusercode,Transtype,narration,receipttype) values('" + transcode + "','1','" + lblledg.Text + "','" + lblhedg.Text + "','" + lblfeecat.Text + "','0','1','1','1','3','" + txtnaration.Text.Trim() + "','6')";
                string INSdaily = "update FT_FinDailyTransaction set narration='" + Narration.Text.Trim() + "' where HeaderFK='" + lblhedg.Text + "'and LedgerFK='" + lblledg.Text + "' and TransCode='" + transcode.Text + "'";
                int inst = d2.update_method_wo_parameter(INSdaily, "Text");

                if (inst == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Update Successfully')", true);
                }

            }
        }
    }
    protected void vendorothersave_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gdrow in othervendor.Rows)
        {
            // double balAmt = 0;
            CheckBox cb = (CheckBox)gdrow.FindControl("cbsel");
            if (cb.Checked)
            {
                Label lblhedg = (Label)gdrow.FindControl("lbl_hdrid");
                Label lblledg = (Label)gdrow.FindControl("lbl_lgrid");
                TextBox vendorname = (TextBox)gdrow.FindControl("txt_vendorname");
                TextBox vendorcode = (TextBox)gdrow.FindControl("txt_vendorcode");
                TextBox compname = (TextBox)gdrow.FindControl("txt_companyname");
                TextBox mobilenum = (TextBox)gdrow.FindControl("txt_mobno");
                TextBox address = (TextBox)gdrow.FindControl("txt_addresss");

                //Label lblpay = (Label)gdrow.FindControl("lblpaymode");
                //Label lblpaid = (Label)gdrow.FindControl("lbl_paid");
                //Label lblbal = (Label)gdrow.FindControl("lbl_bal");
                Label transcode = (Label)gdrow.FindControl("lbl_feeamt");
                Label transdt = (Label)gdrow.FindControl("lbl_totamt");
                Label paid = (Label)gdrow.FindControl("lbl_bal");
                string preTransdt = transdt.Text.Split('/')[1] + "/" + transdt.Text.Split('/')[0] + "/" + transdt.Text.Split('/')[2];

                TextBox Narration = (TextBox)gdrow.FindControl("txt_Narration");
                // string vendorpk1 = d2.GetFunction("select vendorpk from co_vendormaster where vendorcode='" + vendorcode.Text + "'");
                // string appno = d2.GetFunction(" select VendorPK from co_vendormaster where VendorCode= '" + vendorcode.Text + "' and VendorType=-5");
                string appno = d2.GetFunction(" select app_no from FT_FinDailyTransaction where memtype='4' and HeaderFK='" + lblhedg.Text + "'and LedgerFK='" + lblledg.Text + "' and  Transcode='" + transcode.Text.Trim() + "' and transdate='" + preTransdt + "'");
                string newVenCode = generateVendorCode().Trim();

                string INSdaily = "update FT_FinDailyTransaction set narration='" + Narration.Text.Trim() + "' where HeaderFK='" + lblhedg.Text + "'and LedgerFK='" + lblledg.Text + "' and App_No='" + appno + "' and Transcode='" + transcode.Text.Trim() + "' and transdate='" + preTransdt + "'";
                //INSdaily += "update co_vendormaster set  VendorName='" + vendorname.Text.Trim() + "',VendorCompName='" + compname.Text.Trim() + "',VendorMobileNo='" + mobilenum.Text.Trim() + "',VendorAddress='" + address.Text.Trim() + "'  where  VendorCode='" + vendorcode.Text + "' and vendorpk='" + appno + "'";
                INSdaily += "IF EXISTS(select vendorname from co_vendormaster where vendorname= '" + vendorname.Text + "')update co_vendormaster set  VendorCompName='" + compname.Text.Trim() + "',VendorMobileNo='" + mobilenum.Text.Trim() + "',VendorAddress='" + address.Text.Trim() + "',vendortype='-5'  where  VendorCode='" + vendorcode.Text + "' and vendorpk='" + appno + "' else insert into co_vendormaster(vendorname,VendorCompName,VendorAddress,VendorMobileNo,VendorCode,vendortype)values('" + vendorname.Text.Trim() + "','" + compname.Text.Trim() + "','" + address.Text.Trim() + "','" + mobilenum.Text.Trim() + "','" + newVenCode + "','-5')";
                int inst = d2.update_method_wo_parameter(INSdaily, "Text");
                string vendorpk = d2.GetFunction("select VendorPK from co_vendormaster where vendorname='" + vendorname.Text.Trim() + "'");

                string qry = "update FT_FinDailyTransaction set app_no='" + vendorpk + "' where HeaderFK='" + lblhedg.Text + "'and LedgerFK='" + lblledg.Text + "' and Transcode='" + transcode.Text.Trim() + "' and transdate='" + preTransdt + "' and memtype=4 ";

                int upd = d2.update_method_wo_parameter(qry, "Text");
                string qry1 = d2.GetFunction("select app_no from ft_feeallot where HeaderFK='" + lblhedg.Text + "'and LedgerFK='" + lblledg.Text + "' and memtype=4 and app_no='" + vendorpk + "'");

                if (!String.IsNullOrEmpty(qry1) || qry1 == " ")//exist app_no
                {
                    double total = 0;
                    double feeamount = 0;
                    double paidamount = 0;

                    string query = "select feeamount,paidamount,totalamount from ft_feeallot where app_no='" + vendorpk + "' and memtype=4";
                    DataTable dt = DirAccess.selectDataTable(query);
                    if (dt.Rows.Count > 0)
                    {
                        total = Convert.ToDouble(dt.Rows[0]["totalamount"]);
                        feeamount = Convert.ToDouble(dt.Rows[0]["feeamount"]);
                        paidamount = Convert.ToDouble(dt.Rows[0]["paidamount"]);
                        total = Convert.ToDouble(paid.Text) - total;
                        feeamount = Convert.ToDouble(paid.Text) - feeamount;
                        paidamount = Convert.ToDouble(paid.Text) - paidamount;
                        string updateqry = "update ft_feeallot set feeamount='" + feeamount + "',totalamount='" + total + "',paidamount='" + paidamount + "' where app_no='" + vendorpk + "' and memtype=4";
                        int updfee = d2.update_method_wo_parameter(updateqry, "Text");
                    }
                    else
                    {
                        string selq = d2.GetFunction("select paymode from ft_findailytransaction where app_no='" + vendorpk + "'");
                        string fin = d2.GetFunction("select finyearfk from ft_findailytransaction where app_no='" + vendorpk + "'");
                        string insqry = "insert into ft_feeallot(paymode,memtype,allotdate,app_no,ledgerfk,headerfk,feeamount,totalamount,paidamount,Feecategory,balamount,finyearfk)values('" + selq + "','4','" + preTransdt + "','" + vendorpk + "','" + lblledg.Text + "','" + lblhedg.Text + "','" + paid.Text + "','" + paid.Text + "','" + paid.Text + "',0,0,'" + fin + "')";
                        int s = d2.update_method_wo_parameter(insqry, "Text");

                    }


                }

                // int upd1 = d2.update_method_wo_parameter(qry1, "Text");


                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Update Successfully')", true);

            }
        }
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
                if (itemacronym.Trim() != "" && itemstarno.Trim() != "") // Added by jairam
                {
                    selectquery = " select distinct top (1) VendorCode,vendorPK  from CO_VendorMaster where VendorCode like '" + Convert.ToString(itemacronym) + "%' order by VendorCode desc";
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
        catch (Exception ex) { newitemcode = string.Empty; }
        return newitemcode;
    }
    protected void getTabRights(object sender, EventArgs e)
    {

        // rbl_rollnoNew.Items.Clear();
        // rbl_rollnoNew.Enabled = true;
        string selQ = "select LinkValue from New_InsSettings where LinkName='FinanceReceiptTabRights' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedItem.Value + "' ";
        string strVal = Convert.ToString(d2.GetFunction(selQ));
        rbl_rollnoNew.Items.Clear();
        //ftype.Visible = false;
        journal.Visible = false;
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
                            journal.Visible = true;
                            rbl_rollnoNew.Items.Add("Student");
                            rbl_rollnoNew.Items[rcpt].Enabled = true;

                            //rbl_rollnoNew.SelectedIndex = 0;
                            rbl_rollnoNew_OnSelectedIndexChanged(sender, e);
                            break;
                        case 2:
                            journal.Visible = true;
                            rbl_rollnoNew.Items.Add("Staff");
                            //rbl_rollnoNew.Items[rcpt].Enabled = true;
                            //rbl_rollnoNew.SelectedIndex = 1;
                            rbl_rollnoNew_OnSelectedIndexChanged(sender, e);
                            break;
                        case 3:
                            journal.Visible = true;
                            rbl_rollnoNew.Items.Add("Vendor");
                            //rbl_rollnoNew.Items[rcpt].Enabled = true;
                            //rbl_rollnoNew.SelectedIndex = 2;
                            rbl_rollnoNew_OnSelectedIndexChanged(sender, e);
                            break;
                        case 4:
                            journal.Visible = true;
                            rbl_rollnoNew.Items.Add("Others");
                            //rbl_rollnoNew.Items[rcpt].Enabled = true;
                            //rbl_rollnoNew.SelectedIndex = 3;
                            rbl_rollnoNew_OnSelectedIndexChanged(sender, e);
                            break;
                    }
                }
                //rb_cash_Change(sender, e);
            }
        }
    }
    //----------------------------------------------Added by abarna----------------------------------------
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        try
        {
            bool boolCheck = false;
            string hdFK = string.Empty;
            string ldFK = string.Empty;
            string selQ = string.Empty;
            string save0 = string.Empty;
            string HeaderFK = string.Empty;
            string LedgerFK = string.Empty;
            string rollno = string.Empty;
            rollno = txt_rerollno.Text;
            string qry = string.Empty;
            //string disdate = txt_rdate.Text;
            string dis_Date = Convert.ToString(txt_rdate.Text);
            string[] frdate = dis_Date.Split('/');
            if (frdate.Length == 3)
                dis_Date = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string routeid = routetxt.Text;
            string vehid = vehicletxt.Text;
            string stageid = stagetxt.Text;
            string hostelname = txt_hostel.Text;
            string buildname = txt_build.Text;
            string roomname = txt_roomname.Text;


            if (txt_hostel.Text != "")
            {

                selQ = " select LinkValue,college_code from New_InsSettings where LinkName='Hostel_Admission_Form_Fee'  and user_code='" + usercode + "' and college_code in('" + ddlcollege.SelectedValue + "')";
                DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                    {
                        save0 = Convert.ToString(d2.GetFunction(selQ));
                        if (save0.Trim() != "")
                        {
                            string[] admissionformfee = save0.Split('$');//1$9,10$500
                            if (admissionformfee.Length > 1)
                            {
                                if (Convert.ToString(admissionformfee[0]) == "1")
                                {
                                    //cb_hosteladmissionformfee.Checked = true;
                                    if (Convert.ToString(admissionformfee[1]).Trim() != "")
                                    {
                                        string[] headled = Convert.ToString(admissionformfee[1]).Split(',');
                                        if (headled.Length > 1)
                                        {
                                            hdFK = Convert.ToString(headled[0]);
                                            HeaderFK += "'" + "," + "'" + hdFK;
                                            ldFK = Convert.ToString(headled[1]);
                                            LedgerFK += "'" + "," + "'" + ldFK;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }
            else
            {
                selQ = " select LinkValue,college_code from New_InsSettings where LinkName='TransportLedgerValue'  and user_code='" + usercode + "' and college_code in('" + ddlcollege.SelectedValue + "')";


                DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                    {
                        string linkValue = Convert.ToString(dsVal.Tables[0].Rows[row]["LinkValue"]);
                        string clgcode = Convert.ToString(dsVal.Tables[0].Rows[row]["college_code"]);
                        string[] leng = linkValue.Split(',');
                        if (leng.Length == 2)
                        {
                            hdFK = Convert.ToString(leng[0]);
                            HeaderFK += "'" + "," + "'" + hdFK;
                            ldFK = Convert.ToString(leng[1]);
                            LedgerFK += "'" + "," + "'" + ldFK;

                        }
                    }

                }


            }
            if (txt_hostel.Text != "")
            {
                string appno = d2.GetFunction("select app_no from registration where roll_no='" + rollno + "'");
                qry = " update Room_Detail set Avl_Student= Avl_Student - 1 where Roompk in(select RoomFk from HT_HostelRegistration where app_no='" + appno + "' and  isnull(IsDiscontinued,'0')='0' and isnull(isvacated,'0')='0')"; //magesh 13.10.18
                qry = qry + "update ht_hostelregistration set isvacated='1' where app_no='" + appno + "'";
                d2.update_method_wo_parameter(qry, "Text");
                boolCheck = true;
                string paid = "select paidamount from ft_feeallot where app_no='" + appno + "'";
                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(paid, "text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            double amt = Convert.ToDouble(ds.Tables[0].Rows[i]["paidamount"]);
                            if (amt == 0.00)
                            {
                                qry = "delete from ft_Feeallot where app_no='" + appno + "' and paidamount='0' and headerfk in('" + HeaderFK + "') and ledgerfk in('" + LedgerFK + "')";
                                d2.update_method_wo_parameter(qry, "Text");
                            }
                        }
                    }
                }


                qry = "insert into discontinue(app_no,Discontinue_Date,Catogery,boarding,bus_routeid,vehid,buildingname,hostelname,roomname)values('" + appno + "','" + dis_Date + "','3','" + stageid + "','" + routeid + "','" + vehid + "','" + buildname + "','" + hostelname + "','" + roomname + "')";
                d2.update_method_wo_parameter(qry, "Text");


            }
            else
            {
                qry = "update registration set Boarding='',Bus_RouteID='', vehid='' where roll_no='" + rollno + "'";
                d2.update_method_wo_parameter(qry, "Text");
                boolCheck = true;
                string appno = d2.GetFunction("select app_no from registration where roll_no='" + rollno + "'");
                string paid = "select paidamount from ft_feeallot where app_no='" + appno + "'";
                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(paid, "text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            double amt = Convert.ToDouble(ds.Tables[0].Rows[i]["paidamount"]);
                            if (amt == 0.00)
                            {
                                qry = "delete from ft_Feeallot where app_no='" + appno + "' and paidamount='0' and headerfk in('" + HeaderFK + "') and ledgerfk in('" + LedgerFK + "')";
                                d2.update_method_wo_parameter(qry, "Text");
                            }
                        }
                    }
                }
                qry = "insert into discontinue(app_no,Discontinue_Date,Catogery,boarding,bus_routeid,vehid,buildingname,hostelname,roomname)values('" + appno + "','" + dis_Date + "','4','" + stageid + "','" + routeid + "','" + vehid + "','" + buildname + "','" + hostelname + "','" + roomname + "')";
                d2.update_method_wo_parameter(qry, "Text");


            }
            if (boolCheck)
            {

                imgAlert.Visible = true;
                lbl_alert.Text = "Cancel Successfully";
                disTransClear();
            }
        }
        catch
        {
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

    #region Added by saranya for name search option
    protected void txt_name_Changed(object sender, EventArgs e)
    {
        try
        {
            string roll_no = Convert.ToString(txt_name.Text);

            if (roll_no != "")
            {
                if (rb_transfer.Checked)
                {
                    try
                    {
                        string rollno = roll_no.Split('-')[4];
                        roll_no = rollno;
                    }
                    catch { roll_no = ""; }

                }
                else if (rb_discont.Checked)
                {
                    try
                    {
                        string rollno = roll_no.Split('-')[4];
                        roll_no = rollno;
                    }
                    catch { roll_no = ""; }
                }
                else if (rb_Journal.Checked)
                {
                    try
                    {
                        string rollno = roll_no.Split('-')[4];
                        roll_no = rollno;
                    }
                    catch { roll_no = ""; }
                }
                else if (rb_refund.Checked)
                {
                    try
                    {
                        string rollno = roll_no.Split('-')[4];
                        roll_no = rollno;
                    }
                    catch { roll_no = ""; }
                }
                else if (rb_ProlongAbsent.Checked)
                {
                    try
                    {
                        string rollno = roll_no.Split('-')[4];
                        roll_no = rollno;
                    }
                    catch { roll_no = ""; }
                }
                else if (rb_canceltranshostel.Checked)
                {
                    try
                    {
                        string rollno = roll_no.Split('-')[4];
                        roll_no = rollno;
                    }
                    catch { roll_no = ""; }
                }
            }

            txt_roll.Text = roll_no;
            rbl_rollno.SelectedIndex = 0;
            //txt_rollno.TextMode = TextBoxMode.SingleLine;
            txt_roll_TextChanged(sender, e);

        }
        catch (Exception ex) { }
    }

    protected void txt_rename_Changed(object sender, EventArgs e)
    {
        try
        {
            string roll_no = Convert.ToString(txt_rename.Text);

            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 0)
            {
                if (roll_no != "")
                {
                    if (rb_transfer.Checked || rb_discont.Checked || rb_Journal.Checked || rb_ProlongAbsent.Checked || rb_refund.Checked || rb_canceltranshostel.Checked)
                    {
                        try
                        {
                            string rollno = roll_no.Split('-')[4];
                            roll_no = rollno;
                        }
                        catch { roll_no = ""; }
                    }

                }
            }
            if (Convert.ToInt32(rbl_rerollno.SelectedItem.Value) == 1)
            {
                if (roll_no != "")
                {
                    if (rb_transfer.Checked || rb_discont.Checked || rb_Journal.Checked || rb_ProlongAbsent.Checked || rb_refund.Checked || rb_canceltranshostel.Checked)
                    {
                        try
                        {
                            string rollno = roll_no.Split('-')[5];
                            roll_no = rollno;
                        }
                        catch { roll_no = ""; }
                    }

                }
            }
            txt_rerollno.Text = roll_no;
            txt_roll.Text = roll_no;
            rbl_rollno.SelectedIndex = 0;
            //txt_rollno.TextMode = TextBoxMode.SingleLine;
            txt_rerollno_TextChanged(sender, e);

        }
        catch (Exception ex) { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetName(string prefixText)
    {

        WebService ws = new WebService();
        List<string> name = new List<string>();
        Hashtable studhash = new Hashtable();

        if (prefixText.Length > 0)
        {
            string[] nameval = prefixText.Split(' ');
            string query = string.Empty;
            string name_VAL = string.Empty;
            for (int i = 0; i < nameval.Length; i++)
            {
                name_VAL += "%" + nameval[i] + "%";
            }

            if (nameval.Length > 0)
            {
                query = "select  top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No+'-'+r.Reg_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + name_VAL + "'  and r.college_code='" + stcollegecode + "'";
            }
            else
            {
                query = "select  top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No+'-'+r.Reg_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '%" + prefixText + "%' and r.college_code='" + stcollegecode + "'";
            }
            studhash = ws.GetNameSearch(query);
            if (studhash.Count > 0)
            {
                foreach (DictionaryEntry p in studhash)
                {
                    string studname = Convert.ToString(p.Key);
                    name.Add(studname);
                }
            }
        }
        return name;
    }
    #endregion

    protected void rbl_rollNoForRefund_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbl_rollnoNewForRefund.Text == "Student")
        {
            otherdiv.Visible = false;
            ftype.Visible = false;
            tdJournalType.Visible = false;
            settType = 1;
            lnkindivmap.Visible = false;
            fldapplied.Visible = false;
            fldrefund.Visible = true;
            cbdisWithoutFees.Visible = false;
            fldadm.Visible = false;
            divTransfer.Visible = false;
            div_refund.Visible = true;
            refundStudOrStaff.Visible = true;//Added by saranya on 05April2018
            hostels.Visible = false;
            transport.Visible = false;
            loadrefundsetting();
            btn_refund.Text = "Refund";
            disTransClear();
            lbladvance.Visible = false;
            divref.Attributes.Add("Style", "border-radius: 10px; border: 1px solid Gray; width: 900px; height: 200px; overflow: auto;");
            tbltrans.Visible = true;
            tbljournal.Visible = false;
            txt_rdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_rdate.Attributes.Add("readonly", "readonly");
            btnsavePro.Visible = false;
            ddlJournalType.Visible = false;
            btnsavePro.Visible = false;
            tdRefund.Visible = false;
            btn_cancel.Visible = false;
            DiscontinueReason.Visible = false;
            reasondis.Visible = false;
            rcptSngleStaff.Visible = false;

            div_refundStudent.Visible = true;

            Rerollno.Visible = true;
            txt_rename.Visible = true;
            rbl_rerollno.Visible = true;

            LblReName.Visible = true;
            lblcoll.Visible = true;
            txt_recolg.Visible = true;
            LblRebatch.Visible = true;
            txt_rebatch.Visible = true;
            lbldegre.Visible = true;
            txt_redegree.Visible = true;
            lbldeptms.Visible = true;
            txt_redept.Visible = true;
            lblsemests.Visible = true;
            txt_resem.Visible = true;
            txt_restrm.Visible = true;
            lblReSection.Visible = true;
            txt_resec.Visible = true;
            image3.Visible = true;
            LblDate.Visible = true;
            txt_date.Visible = true;
            LblRefund_staffid.Visible = false;
            txtRefund_staffid.Visible = false;
            LblRefund_staffName.Visible = false;
            txtRefund_staffName.Visible = false;
            LblRefund_staffCode.Visible = false;
            txtRefund_staffDept.Visible = false;

        }
        if (rbl_rollnoNewForRefund.Text == "Staff")
        {
            LblRefund_staffid.Visible = true;
            txtRefund_staffid.Visible = true;
            LblRefund_staffName.Visible = true;
            txtRefund_staffName.Visible = true;
            LblRefund_staffCode.Visible = true;
            txtRefund_staffDept.Visible = true;

            div_refundStudent.Visible = true;
            txtRefund_staffid_Changed(sender, e);
            otherdiv.Visible = false;
            ftype.Visible = false;
            tdJournalType.Visible = false;
            settType = 1;
            lnkindivmap.Visible = false;
            fldapplied.Visible = false;
            fldrefund.Visible = false;
            cbdisWithoutFees.Visible = false;
            fldadm.Visible = false;
            divTransfer.Visible = false;
            div_refund.Visible = true;
            refundStudOrStaff.Visible = true;//Added by saranya on 05April2018

            hostels.Visible = false;
            transport.Visible = false;
            //loadrefundsetting();
            btn_refund.Text = "Refund";
            disTransClear();
            lbladvance.Visible = false;
            divref.Attributes.Add("Style", "border-radius: 10px; border: 1px solid Gray; width: 900px; height: 200px; overflow: auto;");
            tbltrans.Visible = true;
            tbljournal.Visible = false;
            txt_rdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_rdate.Attributes.Add("readonly", "readonly");
            btnsavePro.Visible = false;
            ddlJournalType.Visible = false;
            btnsavePro.Visible = false;
            tdRefund.Visible = false;
            btn_cancel.Visible = false;
            DiscontinueReason.Visible = false;
            reasondis.Visible = false;
            Rerollno.Visible = false;
            txt_rename.Visible = false;
            rbl_rerollno.Visible = false;

            LblReName.Visible = false;
            lblcoll.Visible = false;
            txt_recolg.Visible = false;
            LblRebatch.Visible = false;
            txt_rebatch.Visible = false;
            lbldegre.Visible = false;
            txt_redegree.Visible = false;
            lbldeptms.Visible = false;
            txt_redept.Visible = false;
            lblsemests.Visible = false;
            txt_resem.Visible = false;
            txt_restrm.Visible = false;
            lblReSection.Visible = false;
            txt_resec.Visible = false;
            image3.Visible = false;
            LblDate.Visible = false;
            txt_rdate.Visible = false;
            ddl_AmtPerc.Visible = false;
            txt_AmtPerc.Visible = false;
            chk_refCommon.Visible = false;
        }
    }
    protected void txtRefund_staffid_Changed(object sender, EventArgs e)
    {
        string name = string.Empty;
        string degree = string.Empty;
        string college = string.Empty;
        string staffId = Convert.ToString(txtRefund_staffid.Text.Trim());
        //img_stud.ImageUrl = "";
        //img_stud.Visible = false;

        if (staffId != "")
        {
            if (rbl_rollnoNewForRefund.Text == "Staff")
            {

                string query = " select appl_id ,h.dept_name,h.dept_code,s.staff_name,s.staff_code,c.collname  from collinfo c,staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code and s.staff_code ='" + staffId + "' and s.college_Code in('" + stcollegecode + "') ";

                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");


                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            name = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                            degree = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                            //lbltype.Text = Convert.ToString(ds.Tables[0].Rows[i]["type"]);      
                            college = Convert.ToString(ds.Tables[0].Rows[i]["collname"]);
                        }
                    }
                }

                txtRefund_staffName.Text = name;
                txtRefund_staffDept.Text = degree;

                //img_stud.ImageUrl = "~/Handler/ghStaffFoto.ashx?QSstaff_id=" + staffId;
                //img_stud.Visible = true;
                getRefundStaff();
            }
        }
    }
    protected void getRefundStaff()
    {
        try
        {
            txt_reamt.Text = "";
            string StaffId = Convert.ToString(txtRefund_staffid.Text);

            //and r.Roll_no='" + rollno + "'";
            if (!string.IsNullOrEmpty(StaffId))
            {

                string query = " select staff_name,staff_code,sa.dept_name,sa.college_code  from staff_appl_master Sa,staffmaster Sm where sa.appl_no=Sm.appl_no and Sa.college_code='" + stcollegecode + "' and Sm.staff_code='" + StaffId + "' ";
                DataSet ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {

                        txtRefund_staffName.Text = ds1.Tables[0].Rows[i]["staff_name"].ToString();
                        txtRefund_staffid.Text = ds1.Tables[0].Rows[i]["staff_code"].ToString();
                        txtRefund_staffDept.Text = ds1.Tables[0].Rows[i]["dept_name"].ToString();

                        Session["clgcode"] = Convert.ToString(ds1.Tables[0].Rows[i]["college_code"]);

                    }

                    txt_AmtPerc.Text = "";

                    tblgrid3.Visible = true;
                    bindRefund();
                    txt_reamt.Enabled = true;
                    ddl_AmtPerc.Enabled = true;
                    txt_AmtPerc.Enabled = true;
                    ddl_AmtPerc.Visible = true;
                    txt_AmtPerc.Visible = true;
                    tdRefund.Visible = true;

                }
                else
                    disTransClear();
            }
            else
                disTransClear();
        }
        catch (Exception ex) { }
    }
    protected void btn_generate(object sender, EventArgs e)
    {
        getAdmissionNo();

    }
    protected void btn_sureyesRcpt_Click(object sender, EventArgs e)
    {
        Response.Redirect("studentpayment.aspx");
    }
    protected void btn_surenoRcpt_Click(object sender, EventArgs e)
    {
        Div8.Visible = false;
    }
}