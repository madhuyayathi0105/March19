using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Web.UI;
using InsproDataAccess;
using System.Net;
using System.Configuration;

public partial class ChallanConfirm : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    static DAccess2 d22 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    ReuasableMethods reuse = new ReuasableMethods();
    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet sdn = new DataSet();
    InsproDirectAccess dirAccess = new InsproDirectAccess();
    static string searchFltValues = string.Empty;
    static string usercodestat = string.Empty;
    static string collegecodestat = string.Empty;
    static string streamStat = string.Empty;
    static int chosedmode;
    static int isHeaderwise = 0;

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
    bool fromChlnNo = false;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
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
            lbl_validation.Visible = false;
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
                //CheckBox_column.Checked = true;
                // CheckBox_column_CheckedChanged(sender, e);
                // td_challanOption.Visible = false;
                ddl_ChallanOption.Enabled = false;
                txt_fromdate.Enabled = false;
                txt_todate.Enabled = false;

                string StreamShift = string.Empty;
                try
                {
                    StreamShift = Convert.ToString(Session["streamcode"]);
                    if (StreamShift.Trim() == "")
                    {
                        StreamShift = "Stream";
                    }
                }
                catch { StreamShift = "Stream"; }
                lbl_stream.Text = StreamShift;

                // cb_batchDeg.Checked = true;
                Session["dt"] = null;
                bindclg();
                if (ddl_college.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
                }

                ddl_strm.Enabled = false;
                txt_stream.Enabled = false;
                txt_degree.Enabled = false;
                txt_dept.Enabled = false;
                txt_batch.Enabled = false;
                txt_sem.Enabled = false;

                chosedmode = 0;
                LoadFromSettings();
                bindBtch();
                bindstream();
                binddeg();
                binddept();
                LoadYearSemester();
                bindheader();
                loadfinanceyear();
                //txt_regno.Attributes.Add("placeholder", "Roll No");           

                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_date.Attributes.Add("readonly", "readonly");
                txt_chaln.Attributes.Add("readonly", "readonly");

                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");

                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");


            }
            if (ddl_college.Items.Count > 0)
            {
                loadChlAcronym();
            }
            collegecodestat = collegecode1;
            usercodestat = usercode;
            if (ddl_strm.Items.Count > 0)
            {
                if (ddl_strm.SelectedItem.ToString().ToUpper() == "ALL")
                {
                    streamStat = string.Empty;
                    for (int i = 0; i < ddl_strm.Items.Count; i++)
                    {
                        if (ddl_strm.Items[i].ToString().ToUpper() != "ALL")
                        {
                            if (string.IsNullOrEmpty(streamStat))
                            {
                                streamStat = "'" + ddl_strm.Items[i].ToString() + "'";
                            }
                            else
                            {
                                streamStat += ",'" + ddl_strm.Items[i].ToString() + "'";
                            }
                        }
                    }
                    //selectQuery += ddl_strm.Items.Count > 0 ? " and U.type in (" + strmNew + ") " : string.Empty;
                }
                else
                {
                    streamStat = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : string.Empty;
                }
            }
            //streamStat = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : string.Empty;

            fromChlnNo = false;
        }
        catch (Exception ex) { }
    }
    protected void loadChlAcronym()
    {
        collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
        // string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
        string finYearid = Convert.ToString(ddlfinyear.SelectedValue);
        txt_chaln.Text = d2.GetFunction("SELECT ChallanAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    public void bindclg()
    {
        ddl_college.Items.Clear();
        string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
        ds.Clear();
        ddl_college.Items.Clear();
        string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_college.DataSource = ds;
            ddl_college.DataTextField = "collname";
            ddl_college.DataValueField = "college_code";
            ddl_college.DataBind();
        }
        //reuse.bindCollegeToDropDown(usercode, ddl_college);
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
            switch (Convert.ToInt32(rbl_rollno.SelectedItem.Value))
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void txt_chnoreg_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_regno.Text.Trim() != "")
            {
                fromChlnNo = true;
                btn_go_Click(sender, e);
                txt_regno.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void txt_chno_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_chno.Text.Trim() != "")
            {
                fromChlnNo = true;
                btn_go_Click(sender, e);
                txt_chno.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void txt_chnoName_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string txt = Convert.ToString(txt_name.Text);
            if (txt.Trim() != "")
            {
                if (txt.Contains('-') == true)
                {
                    fromChlnNo = true;
                    btn_go_Click(sender, e);
                    txt_name.Focus();
                }
                else
                {
                    txt_name.Text = "";
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Please Select Valid Name";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }

    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadFromSettings();
        bindBtch();
        bindstream();
        binddeg();
        binddept();
        bindheader();
        txt_regno.Text = "";
        txt_name.Text = "";
        txt_chno.Text = "";
        LoadYearSemester();
        loadfinanceyear();
        // btn_go_Click(sender, e);

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
            txt_batch.Text = "Batch";
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_batch.Text = "Batch";
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
            //binddeg();
            //binddept();
            getfltrValues();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_batch.Checked = false;
            commcount = 0;
            txt_batch.Text = "Batch";
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
            //binddeg();
            //binddept();
            getfltrValues();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    public void bindstream()
    {
        try
        {
            string stream = "";
            ddl_strm.Items.Clear();
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + collegecode1 + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm.DataSource = ds;
                ddl_strm.DataTextField = "type";
                ddl_strm.DataValueField = "type";
                ddl_strm.DataBind();
                if (ddl_strm.Items.Count > 0)
                {
                    if (isStreamEnabled())
                    {
                        ddl_strm.Enabled = true;
                        ddl_strm.Items.Add("All");
                    }
                    else
                        ddl_strm.Enabled = false;
                }
            }
        }
        catch
        {
        }

    }
    private bool isStreamEnabled()
    {
        bool enabled = false;
        string chkQ = "select LinkValue from New_InsSettings where LinkName='JournalEnableStreamShift' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
        byte val = 0;
        byte.TryParse(d2.GetFunction(chkQ), out val);
        if (val == 1)
            enabled = true;
        return enabled;

    }
    public void bindstreams()
    {
        try
        {
            ddl_strm.Items.Clear();
            reuse.bindStreamToDropDown(ddl_strm, Convert.ToString(ddl_college.SelectedItem.Value));
            if (ddl_strm.Items.Count > 0)
                ddl_strm.Enabled = true;
            else
                ddl_strm.Enabled = false;
            //cbl_stream.Items.Clear();
            //cb_stream.Checked = false;
            //txt_stream.Text = lbl_stream.Text;
            //string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type<>''  and r.college_code='" + collegecode1 + "'  order by type asc";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(query, "Text");
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    //cbl_stream.DataSource = ds;
            //    //cbl_stream.DataTextField = "type";
            //    //cbl_stream.DataValueField = "type";
            //    //cbl_stream.DataBind();
            //    //if (cbl_stream.Items.Count > 0)
            //    //{
            //    //    for (int i = 0; i < cbl_stream.Items.Count; i++)
            //    //    {
            //    //        cbl_stream.Items[i].Selected = true;
            //    //    }
            //    //    txt_stream.Text = lbl_stream.Text+"(" + cbl_stream.Items.Count + ")";
            //    //    cb_stream.Checked = true;
            //    //}
            //    //txt_stream.Enabled = true;
            //    ddl_strm.DataSource = ds;
            //    ddl_strm.DataTextField = "type";
            //    ddl_strm.DataValueField = "type";
            //    ddl_strm.DataBind();
            //    ddl_strm.Enabled = true;
            //    streamStat = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : string.Empty;
            //}
            //else
            //{
            //    //txt_stream.Enabled = false;
            //    ddl_strm.Enabled = false;
            //}
        }
        catch (Exception ex) { }
    }
    protected void ddl_strm_OnIndexChange(object sender, EventArgs e)
    {
        if (ddl_strm.Items.Count > 0)
        {

            if (ddl_strm.SelectedItem.ToString().ToUpper() == "ALL")
            {
                streamStat = string.Empty;
                for (int i = 0; i < ddl_strm.Items.Count; i++)
                {
                    if (ddl_strm.Items[i].ToString().ToUpper() != "ALL")
                    {
                        if (string.IsNullOrEmpty(streamStat))
                        {
                            streamStat = "'" + ddl_strm.Items[i].ToString() + "'";
                        }
                        else
                        {
                            streamStat += ",'" + ddl_strm.Items[i].ToString() + "'";
                        }
                    }
                }
                //selectQuery += ddl_strm.Items.Count > 0 ? " and U.type in (" + strmNew + ") " : string.Empty;
            }
            else
            {
                streamStat = ddl_strm.Items.Count > 0 ? "'" + ddl_strm.SelectedValue + "'" : "'" + string.Empty + "'";
            }
        }
        // streamStat = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : string.Empty;
        if (Session["dt"] != null)
            Session.Remove("dt");
        binddeg();
        binddept();
    }
    protected void cb_stream_OnCheckedChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    txt_stream.Text = lbl_stream.Text;
        //    if (cb_stream.Checked == true)
        //    {

        //        for (i = 0; i < cbl_stream.Items.Count; i++)
        //        {
        //            cbl_stream.Items[i].Selected = true;
        //        }
        //        txt_stream.Text = lbl_stream.Text+"(" + (cbl_stream.Items.Count) + ")";
        //    }
        //    else
        //    {
        //        for (i = 0; i < cbl_stream.Items.Count; i++)
        //        {
        //            cbl_stream.Items[i].Selected = false;
        //        }
        //    }
        //    binddeg();
        //    binddept();
        //}
        //catch (Exception ex) { }
    }
    protected void cbl_stream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    i = 0;
        //    commcount = 0;
        //    cb_stream.Checked = false;
        //    txt_stream.Text = lbl_stream.Text;
        //    for (i = 0; i < cbl_stream.Items.Count; i++)
        //    {
        //        if (cbl_stream.Items[i].Selected == true)
        //        {
        //            commcount = commcount + 1;
        //        }
        //    }
        //    if (commcount > 0)
        //    {
        //        if (commcount == cbl_stream.Items.Count)
        //        {
        //            cb_stream.Checked = true;
        //        }
        //        txt_stream.Text = lbl_stream.Text+"(" + commcount.ToString() + ")";
        //    }
        //    binddeg();
        //    binddept();
        //}
        //catch (Exception ex) {  }
    }
    public void binddeg()
    {
        try
        {

            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "Degree";
            //batch = "";
            //for (i = 0; i < cbl_batch.Items.Count; i++)
            //{
            //    if (cbl_batch.Items[i].Selected == true)
            //    {
            //        if (batch == "")
            //        {
            //            batch = Convert.ToString(cbl_batch.Items[i].Text);
            //        }
            //        else
            //        {
            //            batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
            //        }
            //    }

            //}
            string stream = string.Empty;
            if (ddl_strm.Enabled)
            {
                //for (int str = 0; str < cbl_stream.Items.Count; str++)
                //{
                //    if (cbl_stream.Items[str].Selected)
                //    {
                //        if (stream == "")
                //        {
                //            stream = Convert.ToString(cbl_stream.Items[str].Text);
                //        }
                //        else
                //        {
                //            stream += "','" + Convert.ToString(cbl_stream.Items[str].Text);
                //        }
                //    }
                //}
                stream = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : "";
                stream = " and course.type in ('" + stream + "')";
            }
            //if (batch != "")
            //{
            //ds.Clear();
            //ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
            string query = "select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegecode1 + ") ";
            query += stream;
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
            //}
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_degree.Text = "Degree";
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
            getfltrValues();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_dept.Checked = false;
            commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "Degree";
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
            getfltrValues();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "Department";
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
                        txt_dept.Text = "Dept(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_dept.Text = "Department";
            if (cb_dept.Checked == true)
            {

                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                }
                txt_dept.Text = "Dept(" + (cbl_dept.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = false;
                }
            }
            getfltrValues();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_dept.Checked = false;
            commcount = 0;
            txt_dept.Text = "Department";
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
                txt_dept.Text = "Dept(" + commcount.ToString() + ")";
            }
            getfltrValues();

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    public void LoadYearSemester()
    {
        try
        {
            cbl_sem.Items.Clear();
            txt_sem.Text = "--Select--";
            cb_sem.Checked = false;
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = d2.loadFeecategory(Convert.ToString(ddl_college.SelectedItem.Value), usercode, ref linkName);
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
    //public void LoadYearSemester()
    //{
    //    try
    //    {
    //        cbl_sem.Items.Clear();
    //        cb_sem.Checked = false;

    //        string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");

    //        if (linkvalue != "")
    //        {
    //            DataSet dsSemYear = new DataSet();
    //            string query = "";
    //            string semyear = "select Linkvalue from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";

    //            if (d2.GetFunction(semyear).Trim() == "1")
    //            {
    //                query = "selECT	* from textvaltable where TextCriteria ='FEECA' and (textval like '%Semester' or textval like '%Year')  and college_code=" + collegecode1 + " order by len(textval),textval asc";
    //            }
    //            else
    //            {
    //                if (linkvalue == "0")
    //                {
    //                    query = "selECT	* from textvaltable where TextCriteria ='FEECA' and textval like '%semester' and college_code=" + collegecode1 + " order by len(textval),textval asc";
    //                }
    //                else
    //                {
    //                    query = " selECT	* from textvaltable where TextCriteria ='FEECA' and textval like '%Year' and college_code=" + collegecode1 + " order by len(textval),textval asc";
    //                }
    //            }
    //            dsSemYear = d2.select_method_wo_parameter(query, "Text");
    //            if (dsSemYear.Tables.Count > 0)
    //            {
    //                if (dsSemYear.Tables[0].Rows.Count > 0)
    //                {
    //                    cbl_sem.DataSource = dsSemYear;
    //                    cbl_sem.DataTextField = "TextVal";
    //                    cbl_sem.DataValueField = "TextCode";
    //                    cbl_sem.DataBind();

    //                    for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                    {
    //                        cbl_sem.Items[i].Selected = true;
    //                    }
    //                    txt_sem.Text = "Semester/Year(" + cbl_sem.Items.Count + ")";
    //                    cb_sem.Checked = true;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    //}
    protected void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_sem.Text = "Semester/Year";
            if (cb_sem.Checked)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester/Year(" + cbl_sem.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem.Checked = false;
            txt_sem.Text = "Semester/Year";
            int cnt = 0;
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    cnt++;
                }
            }
            txt_sem.Text = "Semester/Year(" + cnt + ")";
            if (cnt == cbl_sem.Items.Count)
            {
                cb_sem.Checked = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void cb_header_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_header.Text = "Header";
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void cbl_header_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_header.Checked = false;
            commcount = 0;
            txt_header.Text = "Header";
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    public void bindheader()
    {
        try
        {
            string query = "SELECT  HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  order by len(isnull(hd_priority,10000)),hd_priority asc";

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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void cb_datewise_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_datewise.Checked)
            {
                td_challanOption.Visible = true;
                ddl_ChallanOption.Enabled = true;
                txt_fromdate.Enabled = true;
                txt_todate.Enabled = true;

            }
            else
            {
                td_challanOption.Visible = true;
                ddl_ChallanOption.Enabled = false;
                txt_fromdate.Enabled = false;
                txt_todate.Enabled = false;
            }
            //btn_go_Click(sender, e);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {

        Printcontrol.Visible = false;

        //if (ddl_befAftAdmis.SelectedIndex == 1)
        //{
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Visible = false;
        FpSpread1.SaveChanges();
        tblBtns.Visible = false;
        rptprint.Visible = false;
        bool save = false;
        try
        {
            loadcolumns();
            #region Basic Data
            string selectQuery;
            string dispRoll = string.Empty;
            string chlnNo = txt_chno.Text.Trim();
            string chlnAcr = txt_chaln.Text.Trim();
            string chlnCode = chlnAcr + chlnNo;
            string confDate = txt_date.Text.Trim();
            string name = txt_name.Text.Trim();
            string finYearid = Convert.ToString(ddlfinyear.SelectedValue);
            // string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
            string rollno = string.Empty;
            bool admission = false;
            StringBuilder hdrid = new StringBuilder();
            StringBuilder btch = new StringBuilder();
            StringBuilder dept = new StringBuilder();
            StringBuilder semCode = new StringBuilder();

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
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected)
                {
                    if (semCode.Length == 0)
                    {
                        semCode.Append(Convert.ToString(cbl_sem.Items[i].Value));
                    }
                    else
                    {
                        semCode.Append("," + Convert.ToString(cbl_sem.Items[i].Value));
                    }
                }
            }

            bool checkvalue = false;

            if (!fromChlnNo)
            {
                Session["dt"] = null;
            }

            if (txt_regno.Text.Trim() == "" && txt_name.Text.Trim() == "" && txt_chno.Text.Trim() == "" && txt_Name_Search.Text.Trim() == "" && cb_batchDeg.Checked == false && cb_datewise.Checked == false)
            {
                checkvalue = true;
            }
            else if (semCode.Length == 0 && dept.Length == 0 && btch.Length == 0 && hdrid.Length == 0)
            {
                checkvalue = true;
            }
            if (checkvalue == false)
            {

                if (txt_regno.Text.Trim() == "")
                {
                    if (name != "" && name.Contains('-') == true)
                    {
                        try
                        {
                            rollno = name.Split('-')[4];
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
            #endregion

                //if (ddl_befAftAdmis.SelectedIndex == 0)
                //{
                //    admission = false;
                //}
                //else
                //{
                //    //admission = true;
                //}

                //added by sudhagar 16-05-2016 friday
                bool isValid = false;
                if (!checkRegistration(out isValid))
                    admission = false;
                else
                    admission = true;

                if (cb_batchDeg.Checked)
                {
                    isValid = true;
                    if (Convert.ToInt32(rbl_rollno.SelectedValue.Trim()) == 3)
                    {
                        admission = false;
                    }
                    else
                    {
                        admission = true;
                    }
                }

                ds.Clear();
                if (isValid)
                {
                    string actualFinyearFk = string.Empty;
                    if (checkSchoolSetting() == 0)
                    {
                        actualFinyearFk = ",chl_actualfinyearfk";
                    }
                    else
                    {
                        actualFinyearFk = ",''chl_actualfinyearfk";
                    }
                    #region Query Section

                    string fromdate = txt_fromdate.Text.Split('/')[1] + "/" + txt_fromdate.Text.Split('/')[0] + "/" + txt_fromdate.Text.Split('/')[2];
                    string todate = txt_todate.Text.Split('/')[1] + "/" + txt_todate.Text.Split('/')[0] + "/" + txt_todate.Text.Split('/')[2];

                    if (!admission)
                    {
                        #region Befor Admission

                        //byte studAppSHrtAdm = StudentAppliedShorlistAdmit();
                        string admStudFilter = "";
                        //switch (studAppSHrtAdm)
                        //{
                        //case 0:
                        //admStudFilter = " and a.isconfirm=1 and a.app_no not in (select app_no from registration where Degree_Code in(" + dept + ")  and batch_year in(" + btch + "))";
                        //        break;
                        //    case 1:
                        //        admStudFilter = " and a.isconfirm=1 and isnull(a.selection_status,'0')='1' and isnull(a.admission_status,'0')='0'  and a.app_no not in (select app_no from registration where Degree_Code in(" + dept + ")  and batch_year in(" + btch + "))";
                        //        break;
                        //    case 2:
                        //        admStudFilter = " and a.isconfirm=1 and a.selection_status=1 and a.admission_status=1 and a.app_no not in (select app_no from registration where Degree_Code in(" + dept + ")  and batch_year in(" + btch + "))";
                        //        break;
                        ////}

                        selectQuery = "SELECT ChallanNo,convert(varchar(10), ChallanDate,103) as ChallanDate,app_formno,'' smart_serial_no,''Reg_No,''Roll_Admit,''Roll_No,Stud_Name,Course_Name+'-'+Dept_Name Degree,Course_Name+'-'+dept_acronym DegreeAcr,SUM(TakenAmt) as TakenAmt,ChallanDate  as cldate,A.App_No" + actualFinyearFk + " FROM FT_ChallanDet C,applyn A,Degree G,Course U,Department D WHERE C.App_No = A.app_no AND A.degree_code = G.Degree_Code AND G.Course_Id = u.Course_Id and g.college_code = u.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and a.college_code='" + collegecode1 + "'  ";

                        string strmNew = string.Empty;

                        if (ddl_strm.SelectedItem.ToString().ToUpper() == "ALL")
                        {
                            for (int i = 0; i < ddl_strm.Items.Count; i++)
                            {
                                streamStat = string.Empty;
                                if (ddl_strm.Items[i].ToString().ToUpper() != "ALL")
                                {
                                    if (string.IsNullOrEmpty(strmNew))
                                    {
                                        strmNew = "'" + ddl_strm.Items[i].ToString() + "'";
                                    }
                                    else
                                    {
                                        strmNew += ",'" + ddl_strm.Items[i].ToString() + "'";
                                    }
                                }
                            }
                            selectQuery += ddl_strm.Items.Count > 0 ? " and U.type in (" + strmNew + ") " : string.Empty;
                        }
                        else
                        {
                            selectQuery += ddl_strm.Items.Count > 0 ? " and U.type in ('" + ddl_strm.SelectedValue + "') " : string.Empty;
                        }

                        if (chlnNo != "")
                        {
                            selectQuery += " and ChallanNo = '" + chlnCode + "' ";
                        }
                        else
                        {
                            if (rollno != "")
                            {
                                selectQuery += " and app_formno = '" + rollno + "' ";
                            }
                            else
                            {
                                if (cb_batchDeg.Checked)
                                {
                                    if (btch.Length > 0)
                                    {
                                        selectQuery += " and a.batch_year in(" + btch + " ) ";
                                    }
                                    if (dept.Length > 0)
                                    {
                                        selectQuery += " and a.degree_code in(" + dept + " ) ";
                                    }
                                    if (semCode.Length > 0)
                                    {
                                        selectQuery += "  and C.FeeCategory in (" + semCode + " ) ";
                                    }
                                }
                            }
                            if (hdrid.Length > 0)
                            {
                                selectQuery += " and  C.headerfk in(" + hdrid + " ) ";
                            }
                            if (txt_Name_Search.Text.Trim() != "")
                            {
                                selectQuery += " and a.stud_name like '%" + Convert.ToString(txt_Name_Search.Text) + "%'";
                            }
                            if (cb_datewise.Checked)
                            {
                                if (cb_fromToDate.Checked)
                                {
                                    if (ddl_ChallanOption.SelectedIndex == 0 || ddl_ChallanOption.SelectedIndex == 2)
                                    {
                                        selectQuery += "  and RcptTransDate between '" + fromdate + "' and '" + todate + "'   ";
                                    }
                                    if (ddl_ChallanOption.SelectedIndex == 1)
                                    {
                                        selectQuery += "  and ChallanDate between '" + fromdate + "' and '" + todate + "'   ";
                                    }
                                }
                                if (ddl_ChallanOption.SelectedIndex == 0)
                                {
                                    selectQuery += " and  isnull( IsConfirmed,0)=1 ";
                                }
                                else if (ddl_ChallanOption.SelectedIndex == 1)
                                {
                                    selectQuery += " and  isnull( IsConfirmed,0)=0 ";
                                }
                                else if (ddl_ChallanOption.SelectedIndex == 2)
                                {
                                    selectQuery += " and  isnull( IsConfirmed,0)=2 ";
                                }
                            }
                            //else
                            //{
                            //    selectQuery += " and  isnull( IsConfirmed,0)=0 ";
                            //}
                        }
                        if (checkSchoolSetting() != 0)
                            actualFinyearFk = "";
                        selectQuery += admStudFilter;
                        selectQuery += " GROUP BY ChallanNo, ChallanDate, app_formno, Stud_Name, Course_Name, Dept_Name,ChallanDate,a.App_No,dept_acronym" + actualFinyearFk + " order by  cldate";
                        #endregion
                    }
                    else
                    {
                        //last modified by sudhagar 16-09-2016
                        #region after admission

                        selectQuery = "SELECT ChallanNo,convert(varchar(10), ChallanDate,103) as ChallanDate,app_formno,R.Reg_No,R.Roll_Admit,r.smart_serial_no,R.Roll_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,Course_Name+'-'+dept_acronym DegreeAcr,SUM(TakenAmt) as TakenAmt,ChallanDate  as cldate,r.App_No" + actualFinyearFk + " FROM FT_ChallanDet C,applyn A,Registration R,Degree G,Course U,Department D WHERE C.App_No = A.app_no AND a.app_no = r.App_No and A.degree_code = G.Degree_Code AND G.Course_Id = u.Course_Id and g.college_code = u.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and r.college_code='" + collegecode1 + "'  ";
                        //                        selectQuery += ddl_strm.Items.Count > 0 ? " and U.type in ('" + ddl_strm.SelectedValue + "') " : string.Empty;

                        string strmNew = string.Empty;

                        if (ddl_strm.SelectedItem.ToString().ToUpper() == "ALL")
                        {
                            streamStat = string.Empty;
                            for (int i = 0; i < ddl_strm.Items.Count; i++)
                            {
                                if (ddl_strm.Items[i].ToString().ToUpper() != "ALL")
                                {
                                    if (string.IsNullOrEmpty(strmNew))
                                    {
                                        strmNew = "'" + ddl_strm.Items[i].ToString() + "'";
                                    }
                                    else
                                    {
                                        strmNew += ",'" + ddl_strm.Items[i].ToString() + "'";
                                    }
                                }
                            }
                            selectQuery += ddl_strm.Items.Count > 0 ? " and U.type in (" + strmNew + ") " : string.Empty;
                        }
                        else
                        {
                            selectQuery += ddl_strm.Items.Count > 0 ? " and U.type in ('" + ddl_strm.SelectedValue + "') " : string.Empty;
                        }


                        if (txt_Name_Search.Text.Trim() == "")
                        {
                            if (chlnNo != "")
                            {
                                selectQuery += " and ChallanNo = '" + chlnCode + "' ";
                            }
                            else
                            {
                                if (rollno != "")
                                {
                                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
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
                                        selectQuery += " and a.app_formno = '" + rollno + "' ";
                                    }
                                }
                                else
                                {
                                    if (cb_batchDeg.Checked)
                                    {
                                        if (btch.Length > 0)
                                        {
                                            selectQuery += " and a.batch_year in(" + btch + " ) ";
                                        }
                                        if (dept.Length > 0)
                                        {
                                            selectQuery += " and a.degree_code in(" + dept + " ) ";
                                        }
                                        if (semCode.Length > 0)
                                        {
                                            selectQuery += "  and C.FeeCategory in (" + semCode + " ) ";
                                        }
                                    }
                                }

                                if (hdrid.Length > 0)
                                {
                                    selectQuery += " and  C.headerfk in(" + hdrid + " ) ";
                                }
                                if (txt_Name_Search.Text.Trim() != "")
                                {
                                    selectQuery += " and r.stud_name like '%" + Convert.ToString(txt_Name_Search.Text) + "%'";
                                }
                                if (cb_datewise.Checked)
                                {
                                    if (cb_fromToDate.Checked)
                                    {
                                        if (ddl_ChallanOption.SelectedIndex == 0 || ddl_ChallanOption.SelectedIndex == 2)
                                        {
                                            selectQuery += "  and RcptTransDate between '" + fromdate + "' and '" + todate + "'   ";
                                        }
                                        if (ddl_ChallanOption.SelectedIndex == 1)
                                        {
                                            selectQuery += "  and ChallanDate between '" + fromdate + "' and '" + todate + "'   ";
                                        }
                                    }
                                    if (ddl_ChallanOption.SelectedIndex == 0)
                                    {
                                        selectQuery += " and  isnull( IsConfirmed,0)=1 ";
                                    }
                                    else if (ddl_ChallanOption.SelectedIndex == 1)
                                    {
                                        selectQuery += " and  isnull( IsConfirmed,0)=0 ";
                                    }
                                    else if (ddl_ChallanOption.SelectedIndex == 2)
                                    {
                                        selectQuery += " and  isnull( IsConfirmed,0)=2 ";
                                    }
                                }
                                //else
                                //{
                                //    selectQuery += " and  isnull( IsConfirmed,0)=0 ";
                                //}
                            }
                        }
                        else
                        {
                            selectQuery = selectQuery + " and R.Stud_Name like '%" + Convert.ToString(txt_Name_Search.Text) + "%'";
                            if (hdrid.Length > 0)
                            {
                                selectQuery += " and  C.headerfk in(" + hdrid + " ) ";
                            }

                            if (cb_datewise.Checked)
                            {
                                if (cb_fromToDate.Checked)
                                {
                                    if (ddl_ChallanOption.SelectedIndex == 0 || ddl_ChallanOption.SelectedIndex == 2)
                                    {
                                        selectQuery += "  and RcptTransDate between '" + fromdate + "' and '" + todate + "'   ";
                                    }
                                    if (ddl_ChallanOption.SelectedIndex == 1)
                                    {
                                        selectQuery += "  and ChallanDate between '" + fromdate + "' and '" + todate + "'   ";
                                    }
                                }
                                if (ddl_ChallanOption.SelectedIndex == 0)
                                {
                                    selectQuery += " and  isnull( IsConfirmed,0)=1 ";
                                }
                                else if (ddl_ChallanOption.SelectedIndex == 1)
                                {
                                    selectQuery += " and  isnull( IsConfirmed,0)=0 ";
                                }
                                else if (ddl_ChallanOption.SelectedIndex == 2)
                                {
                                    selectQuery += " and  isnull( IsConfirmed,0)=2 ";
                                }

                            }
                            //else
                            //{
                            //    selectQuery += " and  isnull( IsConfirmed,0)=0 ";
                            //}
                        }
                        if (checkSchoolSetting() != 0)
                            actualFinyearFk = "";
                        selectQuery += " GROUP BY ChallanNo,ChallanDate,app_formno,R.Stud_Name,Course_Name,Dept_Name,R.Reg_No,R.Roll_Admit,R.Roll_No, ChallanDate,r.App_No,r.smart_serial_no,dept_acronym" + actualFinyearFk + " order by   cldate";
                        #endregion
                    }


                    ds = d2.select_method_wo_parameter(selectQuery, "Text");

                    #endregion
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0 || Session["dt"] != null)
                    {
                        if (ds.Tables[0].Rows.Count == 0)
                            save = true;
                        #region spread load
                        //Divspread.Visible = true;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = false;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 12;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.Black;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                        chk.AutoPostBack = false;
                        //for (int k = 0; k < FpSpread1.Sheets[0].Columns.Count; k++)
                        //{
                        //    FpSpread1.Sheets[0].ColumnHeader.Columns[k].HorizontalAlign = HorizontalAlign.Center;
                        //    FpSpread1.Sheets[0].ColumnHeader.Columns[k].Font.Name = "Book Antiqua";
                        //    FpSpread1.Sheets[0].ColumnHeader.Columns[k].Font.Bold = true;
                        //    FpSpread1.Sheets[0].ColumnHeader.Columns[k].Font.Size = FontUnit.Medium;

                        //}

                        FpSpread1.Columns[0].Width = 50;
                        FpSpread1.Columns[1].Width = 60;
                        FpSpread1.Columns[2].Width = 140;
                        FpSpread1.Columns[3].Width = 100;
                        FpSpread1.Columns[4].Width = 100;
                        FpSpread1.Columns[5].Width = 100;
                        FpSpread1.Columns[6].Width = 250;
                        FpSpread1.Columns[7].Width = 350;
                        FpSpread1.Columns[8].Width = 120;
                        FpSpread1.Columns[9].Width = 110;
                        FpSpread1.Columns[10].Width = 50;
                        FpSpread1.Columns[11].Width = 80;

                        #region columnorder visibility
                        if (cblcolumnorder.Items[1].Selected)
                        {
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Columns[3].Visible = false;
                        }

                        if (cblcolumnorder.Items[2].Selected)
                        {
                            FpSpread1.Sheets[0].Columns[4].Visible = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Columns[4].Visible = false;
                        }

                        if (cblcolumnorder.Items[3].Selected)
                        {
                            FpSpread1.Sheets[0].Columns[5].Visible = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Columns[5].Visible = false;
                        }

                        if (cblcolumnorder.Items[4].Selected)
                        {
                            FpSpread1.Sheets[0].Columns[6].Visible = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Columns[6].Visible = false;
                        }

                        if (cblcolumnorder.Items[5].Selected)
                        {
                            FpSpread1.Sheets[0].Columns[7].Visible = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Columns[7].Visible = false;
                        }

                        if (cblcolumnorder.Items[6].Selected)
                        {
                            FpSpread1.Sheets[0].Columns[8].Visible = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Columns[8].Visible = false;
                        }

                        if (cblcolumnorder.Items[7].Selected)
                        {
                            FpSpread1.Sheets[0].Columns[9].Visible = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Columns[9].Visible = false;
                        }

                        if (cblcolumnorder.Items[8].Selected)
                        {
                            FpSpread1.Sheets[0].Columns[10].Visible = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Columns[10].Visible = false;
                        }

                        if (cblcolumnorder.Items[9].Selected)
                        {
                            FpSpread1.Sheets[0].Columns[11].Visible = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Columns[11].Visible = false;
                        }

                        #endregion

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Challan No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Challan Date";

                        if (rbl_rollno.SelectedItem.Text == "App No" || ddl_befAftAdmis.SelectedIndex == 0)
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "App No";
                        }
                        else
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Roll No";
                        }
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Reg No";

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Department";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "ReceiptNo";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "ConfirmDate";
                        if (linkvalue.Trim() == "0")
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Sem";
                        }
                        else
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Year";
                        }

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Total";

                        //if (cb_datewise.Checked && ddl_ChallanOption.SelectedIndex == 0)
                        //{
                        //    FpSpread1.Sheets[0].ColumnHeader.Columns[8].Visible = true;
                        //    FpSpread1.Sheets[0].ColumnHeader.Columns[9].Visible = true;
                        //}
                        //else
                        //{
                        //    FpSpread1.Sheets[0].ColumnHeader.Columns[8].Visible = false;
                        //    FpSpread1.Sheets[0].ColumnHeader.Columns[9].Visible = false;
                        //}

                        for (int j = 0; j < FpSpread1.Sheets[0].Columns.Count; j++)
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Columns[j].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[j].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Columns[j].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Columns[j].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Columns[j].Font.Bold = true;

                        }
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].RowCount++;
                        FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.TextCellType txt1 = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType txt2 = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType txt3 = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType txt4 = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType txt5 = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType txt6 = new FarPoint.Web.Spread.TextCellType();
                        check.AutoPostBack = true;
                        FpSpread1.Sheets[0].Cells[0, 1].CellType = check;
                        FpSpread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[0, 1].Font.Name = "Book Antiqua";
                        for (int i = 2; i < FpSpread1.Sheets[0].Columns.Count; i++)
                        {
                            FpSpread1.Sheets[0].Columns[i].Locked = true;
                        }
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(ds.Tables[0].Rows[i]["chl_actualfinyearfk"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chk;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Value = 0;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt1;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["ChallanNo"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt2;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["ChallanDate"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt3;
                            if (rbl_rollno.SelectedItem.Text == "App No" || ddl_befAftAdmis.SelectedIndex == 0)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["app_formno"]);
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                            }

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = txt4;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";


                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["Degree"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[i]["DegreeAcr"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";



                            string transcode = d2.GetFunction("select Transcode from FT_FinDailyTransaction where PayMode=4 and DDno='" + ds.Tables[0].Rows[i]["ChallanNo"] + "' and App_No='" + ds.Tables[0].Rows[i]["App_No"] + "' and isnull(iscanceled,0)=0");
                            string transdate = d2.GetFunction("select Convert(varchar(10),Transdate,103) as Transdate from FT_FinDailyTransaction where PayMode=4 and DDno='" + ds.Tables[0].Rows[i]["ChallanNo"] + "' and App_No='" + ds.Tables[0].Rows[i]["App_No"] + "'  and isnull(iscanceled,0)=0");
                            string feecatcode = d2.GetFunction("select FeeCategory from FT_ChallanDet where  ChallanNo='" + ds.Tables[0].Rows[i]["ChallanNo"] + "' and App_No='" + ds.Tables[0].Rows[i]["App_No"] + "'");
                            string FeeCat = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                            try
                            {
                                FeeCat = FeeCat.Substring(0, 2);
                            }
                            catch { }
                            if (linkvalue.Trim() == "1")
                            {
                                FeeCat = returnYearforSem(FeeCat.Trim());
                            }
                            if (transcode.Trim() == "0")
                            {
                                transcode = "";
                            }
                            if (transdate.Trim() == "0")
                            {
                                transdate = "";
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = txt5;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = transcode;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].CellType = txt6;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = transdate;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = FeeCat;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Tag = Convert.ToString(ds.Tables[0].Rows[i]["TakenAmt"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(ds.Tables[0].Rows[i]["TakenAmt"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";


                        }

                        #region Add Data From Session

                        if ((fromChlnNo && !cb_datewise.Checked) || (cb_datewise.Checked && ddl_ChallanOption.SelectedIndex == 0))
                        {
                            int indx = FpSpread1.Sheets[0].RowCount - 1;

                            if (Session["dt"] != null)
                            {

                                DataTable d1 = new DataTable();
                                d1 = (DataTable)Session["dt"];
                                if (d1.Rows.Count > 0)
                                {
                                    for (int r = 0; r < d1.Rows.Count; r++)
                                    {
                                        bool Alreadypresent = false;
                                        for (int sp = 1; sp < FpSpread1.Sheets[0].RowCount; sp++)
                                        {
                                            if (Convert.ToString(d1.Rows[r]["ChallanNo"]).Trim() == Convert.ToString(FpSpread1.Sheets[0].Cells[sp, 2].Text).Trim())
                                            {
                                                Alreadypresent = true;
                                            }
                                        }
                                        if (!Alreadypresent)
                                        {
                                            indx++;
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(indx);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(d1.Rows[r]["actualFinyearFk"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chk;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Value = 0;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(d1.Rows[r]["ChallanNo"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(d1.Rows[r]["AppNo1"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(d1.Rows[r]["ChallanDate"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(d1.Rows[r]["AppNo"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = txt4;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(d1.Rows[r]["RegNo"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";



                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(d1.Rows[r]["StudentName"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(d1.Rows[r]["Department"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(d1.Rows[r]["Transcode"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(d1.Rows[r]["Transdate"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(d1.Rows[r]["Feecat"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Tag = Convert.ToString(d1.Rows[r]["TakenAmt"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(d1.Rows[r]["TakenAmt"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";

                                        }
                                    }
                                }
                                //  fromChlnNo = false;
                            }

                        }

                        if (fromChlnNo)
                        {
                            if (FpSpread1.Sheets[0].Rows.Count > 0)
                            {
                                DataTable dt = new DataTable();
                                DataRow dr;
                                dt.Columns.Add("ChallanNo");
                                dt.Columns.Add("ChallanDate");
                                dt.Columns.Add("AppNo");
                                dt.Columns.Add("RegNo");
                                dt.Columns.Add("StudentName");
                                dt.Columns.Add("Department");
                                dt.Columns.Add("TakenAmt");
                                dt.Columns.Add("Transcode");
                                dt.Columns.Add("Transdate");
                                dt.Columns.Add("AppNo1");
                                dt.Columns.Add("Feecat");
                                dt.Columns.Add("actualFinyearFk");

                                for (int fp = 1; fp < FpSpread1.Sheets[0].Rows.Count; fp++)
                                {
                                    dr = dt.NewRow();
                                    dr["ChallanNo"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 2].Text);
                                    dr["ChallanDate"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 3].Text);
                                    dr["AppNo"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 4].Text);
                                    dr["RegNo"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 5].Text);
                                    dr["StudentName"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 6].Text);
                                    dr["Department"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 7].Text);
                                    dr["Transcode"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 8].Text);
                                    dr["Transdate"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 9].Text);
                                    dr["TakenAmt"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 10].Tag);
                                    dr["AppNo1"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 2].Tag);
                                    dr["Feecat"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 10].Text);
                                    dr["actualFinyearFk"] = Convert.ToString(FpSpread1.Sheets[0].Cells[fp, 0].Note);
                                    dt.Rows.Add(dr);
                                }

                                Session["dt"] = dt;

                            }
                            else
                            {
                                Session["dt"] = null;
                            }
                        }
                        #endregion

                        #endregion
                        getDuplicateVisible();
                        FpSpread1.SaveChanges();
                        FpSpread1.Visible = true;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].Rows.Count;
                        tblBtns.Visible = true;
                        rptprint.Visible = true;
                        //}
                        if (cb_datewise.Checked)
                        {
                            if (ddl_ChallanOption.SelectedIndex == 0)
                            {
                                btnChlnCancel.Visible = true;
                                btnChlnConfirm.Visible = false;
                                btnChlnDelete.Visible = false;
                                //btnchangeconfirm.Visible = true;
                            }
                            else if (ddl_ChallanOption.SelectedIndex == 1)
                            {
                                btnChlnCancel.Visible = false;
                                btnChlnConfirm.Visible = true;
                                btnChlnDelete.Visible = true;
                                //btnchangeconfirm.Visible = false;
                            }
                            else
                            {
                                tblBtns.Visible = false;
                            }
                        }
                        else
                        {
                            btnChlnCancel.Visible = true;
                            btnChlnConfirm.Visible = true;
                            btnChlnDelete.Visible = true;
                            //btnchangeconfirm.Visible = false;
                        }
                        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                        {
                            string chlnNum = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                            string AppFormNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                            string AppNo = "";
                            AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                            // AppNo = d2.GetFunction("select app_no from Registration where Roll_No ='" + AppFormNo + "'");
                            if (chlnNum != "" && AppNo != "")
                            {
                                string value = d2.GetFunction(" select isnull( IsConfirmed,'0') from FT_ChallanDet WHERE ChallanNo = '" + chlnNum.Trim() + "' AND App_No = " + AppNo + "");
                                if (value.Trim() == "1" || value.Trim().ToUpper() == "TRUE")
                                {
                                    //for (int k = 0; k < FpSpread1.Columns.Count; k++)
                                    //{
                                    //FpSpread1.Sheets[0].Cells[i, k].BackColor = ColorTranslator.FromHtml("#74F7A4");
                                    FpSpread1.Rows[i].BackColor = Color.FromArgb(32, 178, 153);
                                    //}

                                }
                                else if (value.Trim() == "2")
                                {
                                    //Online
                                    Color clr = Color.FromArgb(255, 77, 77);
                                    FpSpread1.Rows[i].BackColor = clr;
                                    string selectquyery = "select RcptTransCode,convert(varchar(10), RcptTransDate,103)as RcptTransDate from FT_ChallanDet WHERE ChallanNo = '" + chlnNum.Trim() + "' AND App_No = " + AppNo + "";
                                    DataSet dnewset = d2.select_method_wo_parameter(selectquyery, "Text");
                                    if (dnewset.Tables.Count > 0 && dnewset.Tables[0].Rows.Count > 0)
                                    {
                                        FpSpread1.Sheets[0].Cells[i, 8].CellType = txt5;
                                        FpSpread1.Sheets[0].Cells[i, 8].Text = Convert.ToString(dnewset.Tables[0].Rows[0]["RcptTransCode"]);
                                        FpSpread1.Sheets[0].Cells[i, 8].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[i, 8].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[i, 8].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[i, 2].CellType = txt6;
                                        FpSpread1.Sheets[0].Cells[i, 9].Text = Convert.ToString(dnewset.Tables[0].Rows[0]["RcptTransDate"]);
                                        FpSpread1.Sheets[0].Cells[i, 9].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }

                            }
                        }
                    }
                    else
                    {

                        //if (Session["dt"] == null)
                        //{
                        if (Session["dt"] == null)
                        {
                            FpSpread1.Visible = false;
                            //Divspread.Visible = false;
                            FpSpread1.Sheets[0].RowCount = 0;
                            FpSpread1.Sheets[0].ColumnCount = 0;
                            tblBtns.Visible = false;
                            rptprint.Visible = false;

                        }
                        else if (FpSpread1.Visible)
                        {
                            tblBtns.Visible = true;
                            //if (cb_datewise.Checked)
                            //{
                            rptprint.Visible = true;
                            //}
                        }
                        imgAlert.Visible = true;
                        if (chlnNo != "")
                        {
                            if (!fromChlnNo)
                            {
                                lbl_alert.Text = "Invalid Challan Number";
                                // Session["dt"] = null;
                                FpSpread1.Visible = false;
                                //Divspread.Visible = false;
                                FpSpread1.Sheets[0].RowCount = 0;
                                FpSpread1.Sheets[0].ColumnCount = 0;
                                tblBtns.Visible = false;
                                rptprint.Visible = false;
                            }
                            else
                            {
                                lbl_alert.Text = "Invalid Challan Number";
                            }
                        }
                        else
                        {
                            if (!fromChlnNo)
                            {
                                lbl_alert.Text = "No Records Found";
                                // Session["dt"] = null;
                                FpSpread1.Visible = false;
                                //Divspread.Visible = false;
                                FpSpread1.Sheets[0].RowCount = 0;
                                FpSpread1.Sheets[0].ColumnCount = 0;
                                tblBtns.Visible = false;
                                rptprint.Visible = false;
                            }
                            else
                            {
                                lbl_alert.Text = "Please Generate Challan To Process";
                            }
                        }
                        //}
                        //else
                        //{
                        //    //FpSpread1.SaveChanges();
                        //    FpSpread1.Visible = true;
                        //    // FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].Rows.Count;
                        //    tblBtns.Visible = true;
                        //    rptprint.Visible = true;
                        //    lbl_alert.Text = "Please Enter Correct Number";
                        //}
                    }
                    if (save == true)
                    {
                        lbl_alert.Text = "Invalid Number";
                        imgAlert.Visible = true;
                    }
                }
                else
                {
                    if (Session["dt"] == null)
                    {
                        FpSpread1.Visible = false;
                        //Divspread.Visible = false;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        tblBtns.Visible = false;
                        rptprint.Visible = false;

                    }
                    else
                    {

                        tblBtns.Visible = true;
                        //if (cb_datewise.Checked)
                        //{
                        rptprint.Visible = true;
                        //}
                    }
                    //FpSpread1.Visible = false;
                    ////Divspread.Visible = false;
                    //FpSpread1.Sheets[0].RowCount = 0;
                    //FpSpread1.Sheets[0].ColumnCount = 0;

                    imgAlert.Visible = true;
                    if (chlnNo != "")
                    {
                        lbl_alert.Text = "Invalid Challan Number";
                        //Session["dt"] = null;
                    }
                    else if (!isValid)
                    {
                        FpSpread1.Visible = false;
                        //Divspread.Visible = false;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        tblBtns.Visible = false;
                        rptprint.Visible = false;
                        lbl_alert.Text = "Invalid Number";
                    }
                    else
                    {
                        lbl_alert.Text = "Please Generate Challan To Process";
                        // Session["dt"] = null;
                    }

                }
                txt_name.Text = "";
                txt_chno.Text = "";
                txt_regno.Text = "";
                txt_Name_Search.Text = "";
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Any One Field";
                FpSpread1.Visible = false;
                //Divspread.Visible = false;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                tblBtns.Visible = false;
                rptprint.Visible = false;
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode1, "ChallanConfirm");
            Session["dt"] = null;
            FpSpread1.Visible = false;
            //Divspread.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            tblBtns.Visible = false;
            rptprint.Visible = false;
        }
        if (FpSpread1.Rows.Count > 0)
        {
            rptprint.Visible = true;
        }
        else
        {
            if (cb_datewise.Checked)
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "No Records Found";
            }
        }

        //}
        //else
        //{
        //    lblappNo.Visible = true;
        //    rbl_rollno.Visible = false;
        //    FpSpread1.Rows.Count = 0;
        //    FpSpread1.SaveChanges();
        //    FpSpread1.Visible = false;
        //    tblBtns.Visible = false;
        //    rptprint.Visible = false;
        //    imgAlert.Visible = true;
        //    lbl_alert.Text = "Please Admit The Student";
        //    Session["dt"] = null;
        //}

    }

    protected bool checkRegistration(out bool isValid)
    {
        isValid = true;
        bool status = true;
        string studentid = "";
        string studname = "";
        string chalacr = Convert.ToString(txt_chaln.Text);
        string appno = "";
        try
        {
            string chlno = "";
            // string srname = "";

            if (txt_regno.Text.Trim() == "")
            {
                if (txt_name.Text.Trim() == "")
                {
                    if (txt_chno.Text.Trim() == "")
                    {
                        if (txt_Name_Search.Text.Trim() != "")
                            studname = Convert.ToString(txt_Name_Search.Text);
                    }
                    else
                        chlno = Convert.ToString(txt_chno.Text);
                }
                else
                {
                    string name = Convert.ToString(txt_name.Text);
                    if (name != "" && name.Contains('-') == true)
                    {
                        try
                        {
                            studentid = name.Split('-')[4];
                        }
                        catch { }
                    }
                }
            }
            else
                studentid = Convert.ToString(txt_regno.Text);

            if (!string.IsNullOrEmpty(studentid))
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    appno = d2.GetFunction("select App_No  from Registration where Roll_No='" + studentid + "'");

                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                    appno = d2.GetFunction("select App_No  from Registration where reg_no='" + studentid + "'");

                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                    appno = d2.GetFunction("select App_No  from Registration where Roll_Admit='" + studentid + "'");

                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                    appno = d2.GetFunction(" select app_no from applyn where smart_serial_no='" + studentid + "'");
                else
                {
                    appno = d2.GetFunction(" select app_no from applyn where app_formno='" + studentid + "'");
                    status = false;
                }
            }
            else if (!string.IsNullOrEmpty(chlno))
            {
                //chlno
                string roll = "";
                appno = d2.GetFunction("select distinct app_no from ft_challandet where challanno='" + chalacr + chlno + "'");
                if (appno != "0")
                    roll = d2.GetFunction("select app_no from registration where app_no='" + appno + "'");
                if (roll == "0")
                {
                    roll = d2.GetFunction("select app_no from applyn where app_no='" + appno + "'");
                    status = false;
                }
            }
            else if (studname.Trim() != string.Empty)
            {
                appno = d2.GetFunction("select app_no from registration where Stud_Name like '%" + studname + "%'");
                if (appno == "0")
                {
                    appno = d2.GetFunction("select app_no from applyn where Stud_Name like '%" + studname + "%'");
                    status = false;
                }
            }
        }
        catch { }
        if (appno.Trim() == "0" || string.IsNullOrEmpty(appno.Trim()))
        {
            isValid = false;
        }
        return status;
    }
    protected void ddl_befAftAdmis_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Session["dt"] != null)
                Session.Remove("dt");
            FpSpread1.Rows.Count = 0;
            FpSpread1.SaveChanges();
            FpSpread1.Visible = false;
            tblBtns.Visible = false;
            rptprint.Visible = false;

            if (ddl_befAftAdmis.SelectedIndex == 1)
            {
                lblappNo.Visible = false;
                rbl_rollno.Visible = true;
            }
            else
            {
                lblappNo.Visible = true;
                rbl_rollno.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void Cell_Click1(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }
    protected void btnChlnConfirm_Click(object sender, EventArgs e)
    {
        string alertmsg = "";
        if (checkedOK())
        {
            try
            {
                FpSpread1.SaveChanges();
                string fineHdrId = string.Empty;
                string fineLgrId = string.Empty;
                string fineHdrIdRe = string.Empty;
                string fineLgrIdRe = string.Empty;
                string finYearid = Convert.ToString(ddlfinyear.SelectedValue);
                // string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                //string accountid = d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
                string acronym = string.Empty;//d2.GetFunction("SELECT  RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where FinYearFK=" + finYearid + ")");
                string fineLegHedQ = d2.GetFunction(" select Linkvalue from New_InsSettings where LinkName='FineLedgerValue' and user_code ='" + usercode + "' and college_code  in (" + ddl_college.SelectedItem.Value + ")");
                if (fineLegHedQ != "0")
                {
                    fineHdrId = fineLegHedQ.Split(',')[0];
                    fineLgrId = fineLegHedQ.Split(',')[1];
                }

                string fineLegHedQRe = d2.GetFunction(" select Linkvalue from New_InsSettings where LinkName='ReAdmissionFessSettings' and user_code ='" + usercode + "' and college_code  in (" + ddl_college.SelectedItem.Value + ")");
                if (fineLegHedQRe != "0")
                {
                    fineHdrIdRe = fineLegHedQRe.Split(',')[0];
                    fineLgrIdRe = fineLegHedQRe.Split(',')[1];
                }

                string hdrSetPK = string.Empty;
                bool confvalue = false;
                string actualSelect = string.Empty;
                string actualInsert = string.Empty;
                string actualInsertVal = string.Empty;
                for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                    if (check == 1)
                    {
                        string chlnNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                        string chlnDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);
                        string AppFormNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                        string studname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                        string dept = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);
                        string total = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Tag);
                        string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                        string actualFinyearFk = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Note);
                        if (!string.IsNullOrEmpty(actualFinyearFk))
                        {
                            actualSelect = " and finyearfk='" + actualFinyearFk + "'";
                            actualInsert = ",actualfinyearfk";
                            actualInsertVal = ",'" + actualFinyearFk + "'";
                        }
                        string trasdate = txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2];
                        chlnDt = chlnDt.Split('/')[1] + "/" + chlnDt.Split('/')[0] + "/" + chlnDt.Split('/')[2];
                        string transtime = DateTime.Now.ToLongTimeString();


                        sdn.Clear();
                        sdn = d2.select_method_wo_parameter(" select ChallanNo from FT_ChallanDet WHERE ChallanNo = '" + chlnNo.Trim() + "' AND App_No = " + AppNo + " and isnull( IsConfirmed,'0') = '0'", "Text");

                        if (sdn.Tables.Count > 0 && sdn.Tables[0].Rows.Count > 0)
                        {

                            string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,bankFK,TakenAmt,FInyearFk from FT_ChallanDet where challanNo='" + chlnNo + "' AND App_No = " + AppNo + " and isnull( IsConfirmed,'0') = '0' select distinct HeaderFk from FT_ChallanDet where challanNo='" + chlnNo + "' AND App_No = " + AppNo + " and isnull( IsConfirmed,'0') = '0'";
                            DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                            bool challanOk = true;
                            if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsDet.Tables[0].Rows.Count; j++)
                                {
                                    string ledger = Convert.ToString(dsDet.Tables[0].Rows[j]["LedgerFK"]);
                                    string header = Convert.ToString(dsDet.Tables[0].Rows[j]["HeaderFk"]);
                                    string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[j]["FeeCategory"]);
                                    string finFk = Convert.ToString(dsDet.Tables[0].Rows[j]["FInyearFk"]);
                                    string taknAmt = Convert.ToString(dsDet.Tables[0].Rows[j]["TakenAmt"]);
                                    double amount = 0;
                                    double.TryParse(taknAmt, out amount);

                                    double balamount = 0;
                                    string balAmtStr = d2.GetFunction("select ISNULL(totalamount,0)-ISNULL(paidamount,0) as balamount from FT_FeeAllot where LedgerFK=" + ledger + " and HeaderFK=" + header + " and FeeCategory=" + FeeCategory + "  and App_No=" + AppNo + " " + actualSelect + ""); //and FinYearFK=" + finFk + "
                                    double.TryParse(balAmtStr, out balamount);
                                    if (balamount < amount)
                                    {
                                        if (fineLgrId != ledger && fineLgrIdRe != ledger)
                                        {
                                            challanOk = false;
                                        }
                                    }
                                }
                            }
                            if (challanOk)
                            {
                                if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                                {
                                    string hdrs = string.Empty;
                                    for (int hdr = 0; hdr < dsDet.Tables[1].Rows.Count; hdr++)
                                    {
                                        if (hdrs == string.Empty)
                                        {
                                            hdrs = Convert.ToString(dsDet.Tables[1].Rows[hdr][0]);
                                        }
                                        else
                                        {
                                            hdrs += "," + Convert.ToString(dsDet.Tables[1].Rows[hdr][0]);
                                        }
                                    }
                                    int save1 = 0;
                                    try
                                    {
                                        string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and college_code ='" + collegecode1 + "' -- and user_code ='" + usercode + "' ";
                                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));

                                    }
                                    catch { save1 = 0; }

                                    string transcode = generateReceiptNo(out acronym, out hdrSetPK, hdrs);
                                    if (save1 == 5 || (transcode != "" && (hdrSetPK != "" || (isHeaderwise == 0))))
                                    {
                                        int insOk = 0;

                                        for (int j = 0; j < dsDet.Tables[0].Rows.Count; j++)
                                        {
                                            string ledger = Convert.ToString(dsDet.Tables[0].Rows[j]["LedgerFK"]);
                                            string header = Convert.ToString(dsDet.Tables[0].Rows[j]["HeaderFk"]);
                                            string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[j]["FeeCategory"]);
                                            string bankPk = Convert.ToString(dsDet.Tables[0].Rows[j]["bankFk"]);
                                            string taknAmt = Convert.ToString(dsDet.Tables[0].Rows[j]["TakenAmt"]);

                                            string bankDet = "SELECT DISTINCT BankCode,City FROM FM_FinBankMaster  where CollegeCode=" + collegecode1 + " and BankPk=" + bankPk + "";
                                            DataSet dsBnk = d2.select_method_wo_parameter(bankDet, "Text");

                                            if (dsBnk.Tables.Count > 0)
                                            {
                                                if (dsBnk.Tables[0].Rows.Count > 0)
                                                {
                                                    string iscollected = "0";
                                                    string collecteddate = "";

                                                    iscollected = "1";
                                                    collecteddate = (Convert.ToDateTime(trasdate).ToString("MM/dd/yyyy")).ToString();
                                                    string bnkCode = Convert.ToString(dsBnk.Tables[0].Rows[0]["BankCode"]);
                                                    string bnkCity = Convert.ToString(dsBnk.Tables[0].Rows[0]["City"]);

                                                    string insQuery = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,IsCollected,CollectedDate" + actualInsert + ") VALUES('" + Convert.ToDateTime(trasdate).ToString("MM/dd/yyyy") + "','" + transtime + "','" + transcode + "', 1, " + AppNo + ", " + ledger + ", " + header + ", " + FeeCategory + ", 0, " + taknAmt + ", 4, '" + chlnNo + "', '" + Convert.ToDateTime(chlnDt).ToString("MM/dd/yyyy") + "', " + bankPk + ",'" + bnkCity + "', 1, '0', 0, '', '0', '0', '0', 0, " + usercode + ", " + finYearid + ",'" + iscollected + "','" + collecteddate + "'" + actualInsertVal + ")";

                                                    insOk = d2.update_method_wo_parameter(insQuery, "Text");

                                                    string getChlTakenAllot = " select chltaken from FT_FeeAllot where HeaderFk='" + header + "' and ledgerfk='" + ledger + "' and feecategory='" + FeeCategory + "' and App_No='" + AppNo + "'";
                                                    double allotChlTaken = 0;
                                                    double.TryParse(Convert.ToString(d2.GetFunction(getChlTakenAllot)), out allotChlTaken);
                                                    string updateCHlTkn = string.Empty;
                                                    if (allotChlTaken < 0)
                                                        updateCHlTkn = " ,ChlTaken = '0'";
                                                    else
                                                        updateCHlTkn = " ,ChlTaken = isnull(ChlTaken,'0')-  " + taknAmt + "";

                                                    string updateFee = "UPDATE FT_FeeAllot SET PaidAmount = isnull(PaidAmount,0) + " + taknAmt + ",BalAmount =isnull(BalAmount,'0')-  " + taknAmt + "" + updateCHlTkn + " WHERE App_No = " + AppNo + " AND FeeCategory = " + FeeCategory + " AND LedgerFK = " + ledger + " and HeaderFk=" + header + " " + actualSelect + "";
                                                    d2.update_method_wo_parameter(updateFee, "Text");

                                                }
                                            }
                                        }


                                        if (insOk > 0)
                                        {
                                            #region Update  Challan
                                            string updateChln = "UPDATE FT_ChallanDet SET RcptTransCode= '" + transcode + "',RcptTransDate= '" + trasdate + "',IsConfirmed = '1' WHERE ChallanNo = '" + chlnNo + "' AND App_No = " + AppNo + "";
                                            d2.update_method_wo_parameter(updateChln, "Text");



                                            #endregion

                                            #region Update Receipt No
                                            transcode = transcode.Remove(0, acronym.Length);

                                            if (save1 != 5)
                                            {
                                                string updateRecpt = string.Empty;
                                                if (isHeaderwise == 0)
                                                {
                                                    updateRecpt = " update FM_FinCodeSettings set RcptStNo=(" + transcode + "+1) where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                                                }
                                                else
                                                {
                                                    updateRecpt = "update FM_HeaderFinCodeSettings set RcptStNo=(" + transcode + "+1) where HeaderSettingPK=" + hdrSetPK + " and FinyearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1'";
                                                }
                                                d2.update_method_wo_parameter(updateRecpt, "Text");
                                            }
                                            #endregion

                                            if (ddl_befAftAdmis.SelectedIndex == 0)
                                            {
                                                #region Move to Registration

                                                string Criteria_code = d2.GetFunction("select criteria_Code  from selectcriteria where app_no ='" + AppNo + "'").Trim();
                                                string degreecode = d2.GetFunction("select degree_code from applyn where app_No=" + AppNo + "").Trim();
                                                string regInsQ = "update applyn set admission_status ='1',selection_status ='1' where app_no  ='" + AppNo + "'";
                                                regInsQ = regInsQ + " update admitcolumnset set allot =allot+1 where column_name ='" + Criteria_code + "' and setcolumn ='" + degreecode + "'";
                                                d2.update_method_wo_parameter(regInsQ, "Text");

                                                if (IsMoveToReg())
                                                {
                                                    string regvalQ = "select * from applyn where app_no=" + AppNo + "";
                                                    DataSet dsREgVal = new DataSet();
                                                    dsREgVal = d2.select_method_wo_parameter(regvalQ, "Text");
                                                    if (dsREgVal.Tables.Count > 0 && dsREgVal.Tables[0].Rows.Count > 0)
                                                    {

                                                        string regisInsQ = "  if exists(select * from Registration where App_No='" + AppNo + "' )  delete from Registration where App_No='" + AppNo + "' insert into Registration    (App_No, Adm_Date, Roll_Admit, Roll_No, RollNo_Flag, Reg_No, Stud_Name, Batch_Year, degree_code, college_code, CC, DelFlag, Exam_Flag, Current_Semester,mode,Stud_Type) values ('" + AppNo + "','" + DateTime.Now.Date + "','" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["App_formno"]) + "','" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["app_formno"]) + "','1','" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["app_formno"]) + "','" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["stud_name"]) + "','" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["batch_year"]) + "','" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["degree_code"]) + "','" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["college_code"]) + "','0','0','OK','1',1,'Day Scholar')";//From select -- and Adm_Date='" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["date_applied"]) + "' and Stud_Name='" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["stud_name"]) + "' and Batch_Year='" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["batch_year"]) + "' and   degree_code='" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["degree_code"]) + "' and  college_code='" + Convert.ToString(dsREgVal.Tables[0].Rows[0]["college_code"]) + "' 
                                                        d2.update_method_wo_parameter(regisInsQ, "Text");
                                                    }
                                                }
                                                #endregion
                                            }
                                            confvalue = true;
                                            imgAlert.Visible = true;
                                            alertmsg = "Confirmed Sucessfully";
                                            FpSpread1.Rows[i].BackColor = Color.LightGreen;
                                            FpSpread1.Sheets[0].Cells[i, 1].Value = 0;

                                            //==================Added by saranya on 11/04/2018=================//
                                            int savevalue = 1;
                                            string entrycode = Session["Entry_Code"].ToString();
                                            string formname = "ChallanConfirm";
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
                                            string details = "RollNO - " + AppFormNo + " :ChallanNo - " + chlnNo + " : ChallanDate - " + chlnDt + " : Date - " + toa + "";
                                            string ctsname = "";
                                            if (savevalue == 1)
                                            {
                                                ctsname = "ChallanConfirm";
                                            }
                                            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                                            d2.insertEinanceUserActionLog(entrycode, formname, 1, toa, doa, details, ctsname, localip);
                                            //==============================================================//
                                        }
                                        else
                                        {
                                            imgAlert.Visible = true;
                                            alertmsg = "Not Saved";
                                        }
                                    }
                                    else
                                    {
                                        imgAlert.Visible = true;
                                        alertmsg = "Please Select Particular Header";// "Receipt No Not Assigned For Selected Headers";
                                    }

                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    alertmsg = "Not Saved";
                                }
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                alertmsg = "Challan Cannot Be Confirmed. Balance Not Available";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            alertmsg = "Challan Already Confirmed";
                        }
                    }
                }
                // btn_go_Click(sender, e);
                FpSpread1.SaveChanges();
                FpSpread1.Sheets[0].Cells[0, 1].Value = 0;

                if (confvalue)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Confirmed Sucessfully";
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = alertmsg;
                }

            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Challan";
        }
    }
    private bool IsMoveToReg()
    {
        bool Move = false;
        string Q = "select LinkValue from New_InsSettings where LinkName='MoveFromChallanToReg' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
        int moveVal = 0;
        int.TryParse(d2.GetFunction(Q.Trim()), out moveVal);
        if (moveVal > 0)
        {
            Move = true;
        }
        return Move;
    }
    protected void btnChlnCancel_Click(object sender, EventArgs e)
    {
        string alertmsg = "";
        if (checkedOK())
        {
            FpSpread1.SaveChanges();
            try
            {
                //string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                string finYearid = Convert.ToString(ddlfinyear.SelectedValue);
                bool cancvalue = false;
                string actualSelect = string.Empty;
                string actualInsert = string.Empty;
                string actualInsertVal = string.Empty;
                string AppFormNo=string.Empty;
                for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                    if (check == 1)
                    {
                        string chlnNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                        string chlnDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);
                        AppFormNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                        string studname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                        string dept = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);
                        string total = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Tag);
                        string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                        string actualFinyearFk = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Note);
                        if (!string.IsNullOrEmpty(actualFinyearFk))
                        {
                            actualSelect = " and finyearfk='" + actualFinyearFk + "'";
                            actualInsert = ",actualfinyearfk";
                            actualInsertVal = ",'" + actualFinyearFk + "'";
                        }
                        string trasdate = txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2];
                        string transtime = DateTime.Now.ToLongTimeString();
                        // string AppNo = d2.GetFunction("select app_no from applyn where app_formno='" + AppFormNo + "'");

                        sdn.Clear();
                        sdn = d2.select_method_wo_parameter(" select ChallanNo from FT_ChallanDet WHERE ChallanNo = '" + chlnNo.Trim() + "' AND App_No = " + AppNo + " and  isnull(IsConfirmed,0)= '1'", "Text");

                        if (sdn.Tables[0].Rows.Count > 0)
                        {
                            string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,TakenAmt from FT_ChallanDet where challanNo='" + chlnNo + "' AND App_No = " + AppNo + "  and  isnull(IsConfirmed,0)= '1'";
                            DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                            if (dsDet.Tables.Count > 0)
                            {
                                if (dsDet.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < dsDet.Tables[0].Rows.Count; j++)
                                    {
                                        string ledger = Convert.ToString(dsDet.Tables[0].Rows[j]["LedgerFK"]);
                                        string header = Convert.ToString(dsDet.Tables[0].Rows[j]["HeaderFk"]);
                                        string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[j]["FeeCategory"]);
                                        string taknAmt = Convert.ToString(dsDet.Tables[0].Rows[j]["TakenAmt"]);

                                        string delQuery = "UPDATE FT_FinDailyTransaction set iscanceled=1 , cancelleddate='" + DateTime.Now.Date + "' , cancelusercode=" + usercode + " where ddno= '" + chlnNo + "' and app_no = " + AppNo + "";
                                        int delOK = d2.update_method_wo_parameter(delQuery, "Text");

                                        string updateFee = "UPDATE FT_FeeAllot SET PaidAmount =isnull(PaidAmount,'0') - " + taknAmt + ",BalAmount =isnull(BalAmount,'0')+  " + taknAmt + ",ChlTaken =isnull(ChlTaken,'0')-" + taknAmt + " WHERE App_No = " + AppNo + " AND FeeCategory = " + FeeCategory + " AND LedgerFK = " + ledger + " and   HeaderFk=" + header + " " + actualSelect + "";
                                        d2.update_method_wo_parameter(updateFee, "Text");
                                    }

                                    imgAlert.Visible = true;

                                    #region Update Challan

                                    //string updateChln = "UPDATE FT_ChallanDet SET RcptTransCode= '" + transcode + "',RcptTransDate= '" + Convert.ToDateTime(trasdate).ToString("MM/dd/yyyy") + "',IsConfirmed = '0' WHERE ChallanNo = '" + chlnNo + "' AND App_No = " + AppNo + "  AND FeeCategory = " + FeeCategory + " ";
                                    string updateChln = "UPDATE FT_ChallanDet SET IsConfirmed = '0' WHERE ChallanNo = '" + chlnNo + "' AND App_No = " + AppNo + "  ";
                                    d2.update_method_wo_parameter(updateChln, "Text");


                                    #endregion

                                    #region Update Receipt No
                                    //transcode = transcode.Remove(0, acronym.Length);
                                    //string updateRecpt = " update account_info set receipt=" + transcode + " where acct_id =" + accountid + "";
                                    //d2.update_method_wo_parameter(updateRecpt, "Text");

                                    #endregion

                                    if (ddl_befAftAdmis.SelectedIndex == 0)
                                    {
                                        #region Move to Registration
                                        //if (IsMoveToReg())
                                        //{
                                        //    string regInsQ = "delete from registration  where app_no  ='" + AppNo + "'";
                                        //    d2.update_method_wo_parameter(regInsQ, "Text");
                                        //}
                                        //else
                                        //{
                                        //    //string Criteria_code = d2.GetFunction("select criteria_Code  from selectcriteria where app_no ='" + AppNo + "'").Trim();
                                        //    //string degreecode = d2.GetFunction("select degree_code from applyn where app_No=" + AppNo + "").Trim();
                                        //    string regInsQ = "update applyn set admission_status ='0',selection_status ='1' where app_no  ='" + AppNo + "'";
                                        //   // regInsQ = regInsQ + " update admitcolumnset set allot =allot+1 where column_name ='" + Criteria_code + "' and setcolumn ='" + degreecode + "'";
                                        //    d2.update_method_wo_parameter(regInsQ, "Text");
                                        //}
                                        #endregion
                                    }

                                    cancvalue = true;
                                    alertmsg = "Cancelled Sucessfully";
                                    FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                                    FpSpread1.Rows[i].BackColor = Color.White;

                                    //==================Added by saranya on 11/04/2018=================//
                                    int savevalue = 1;
                                    string entrycode = Session["Entry_Code"].ToString();
                                    string formname = "Challan";
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
                                    string details = "RollNO - " + AppFormNo + " :ChallanNo - " + chlnNo + " : ChallanDate - " + chlnDt + " : Date - " + toa + "";
                                    string ctsname = "";
                                    if (savevalue == 1)
                                    {
                                        ctsname = "ChallanCancel";
                                    }
                                    string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                                    d2.insertEinanceUserActionLog(entrycode, formname, 1, toa, doa, details, ctsname, localip);
                                    //==============================================================//
                                }
                                else
                                {
                                    imgAlert.Visible = true;
                                    alertmsg = "Not Cancelled";
                                }
                            }
                            else
                            {
                                imgAlert.Visible = true;
                                alertmsg = "Not Cancelled";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            alertmsg = "Challan Already Cancelled";
                        }
                    }
                }
                //btn_go_Click(sender, e);
                // FpSpread1.SaveChanges();
                FpSpread1.Sheets[0].Cells[0, 1].Value = 0;
                if (cancvalue)
                {
                    lbl_alert.Text = "Cancelled Sucessfully";
                }
                else
                {
                    lbl_alert.Text = alertmsg;
                }
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Challan";
        }
    }
    protected void btnChlnDelete_Click(object sender, EventArgs e)
    {
        if (checkedOK())
        {
            surediv.Visible = true;
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Challan";
        }

    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        try
        {
            string alertmsg = "";
            surediv.Visible = false;
            bool delValue = false;
            string actualSelect = string.Empty;
            string actualInsert = string.Empty;
            string actualInsertVal = string.Empty;
            for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
            {
                byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                if (check == 1)
                {
                    string chlnNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    string chlnDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);
                    string AppFormNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                    string studname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                    string dept = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);
                    string total = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Tag);
                    string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                    string actualFinyearFk = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Note);
                    if (!string.IsNullOrEmpty(actualFinyearFk))
                    {
                        actualSelect = " and finyearfk='" + actualFinyearFk + "'";
                        actualInsert = ",actualfinyearfk";
                        actualInsertVal = ",'" + actualFinyearFk + "'";
                    }
                    string trasdate = txt_date.Text.Trim();
                    string transtime = DateTime.Now.ToLongTimeString();
                    // string AppNo = d2.GetFunction("select app_no from applyn where app_formno='" + AppFormNo + "'");
                    // string transcode = generateReceiptNo();

                    string confirmChk = d2.GetFunction(" select ChallanNo from FT_ChallanDet WHERE ChallanNo = '" + chlnNo.Trim() + "' AND App_No = " + AppNo + " and isnull(IsConfirmed,0) = '0'");
                    if (confirmChk != null && confirmChk != "")
                    {
                        string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,isnull(TakenAmt,0) as TakenAmt  from FT_ChallanDet where challanNo='" + chlnNo + "'  AND App_No = " + AppNo + " and isnull(IsConfirmed,0) = '0'";
                        DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                        if (dsDet.Tables.Count > 0)
                        {
                            if (dsDet.Tables[0].Rows.Count > 0)
                            {
                                for (int n = 0; n < dsDet.Tables[0].Rows.Count; n++)
                                {

                                    string ledger = Convert.ToString(dsDet.Tables[0].Rows[n]["LedgerFK"]);
                                    string header = Convert.ToString(dsDet.Tables[0].Rows[n]["HeaderFk"]);
                                    string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[n]["FeeCategory"]);
                                    string creditamt = Convert.ToString(dsDet.Tables[0].Rows[n]["TakenAmt"]);

                                    string delQuery = "delete from FT_ChallanDet WHERE ChallanNo = '" + chlnNo + "' AND App_No = " + AppNo + " AND FeeCategory = " + FeeCategory + " and HeaderFk=" + header + " and LedgerFk=" + ledger + " and (IsConfirmed = '0' or IsConfirmed is Null)";
                                    double allotChlTaken = 0;
                                    double allotTotAmt = 0;
                                    string getChlTakenAllot = " select chltaken,totalamount from FT_FeeAllot where HeaderFk='" + header + "' and ledgerfk='" + ledger + "' and feecategory='" + FeeCategory + "' and App_No='" + AppNo + "'";
                                    DataSet dsTemp = d2.select_method_wo_parameter(getChlTakenAllot, "Text");
                                    if (dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dsTemp.Tables[0].Rows[0]["chltaken"]), out allotChlTaken);
                                        double.TryParse(Convert.ToString(dsTemp.Tables[0].Rows[0]["totalamount"]), out allotTotAmt);
                                    }
                                    // double.TryParse(Convert.ToString(d2.GetFunction(getChlTakenAllot)), out allotChlTaken);
                                    string updateCHlTkn = string.Empty;
                                    if (allotChlTaken < 0 || allotTotAmt < allotChlTaken)//if it's come minus value then chlken will be update 0
                                    {
                                        updateCHlTkn = " update FT_FeeAllot set ChlTaken ='0'  where FeeCategory ='" + FeeCategory + "' and HeaderFK ='" + header + "' and LedgerFK ='" + ledger + "' and App_No='" + AppNo + "' " + actualSelect + "";
                                    }
                                    else
                                    {
                                        updateCHlTkn = " update FT_FeeAllot set ChlTaken =ISNULL( ChlTaken,0)-" + creditamt + "  where FeeCategory ='" + FeeCategory + "' and HeaderFK ='" + header + "' and LedgerFK ='" + ledger + "' and App_No='" + AppNo + "' " + actualSelect + "";
                                    }
                                    d2.update_method_wo_parameter(updateCHlTkn, "Text");

                                    int delOK = d2.update_method_wo_parameter(delQuery, "Text");

                                    imgAlert.Visible = true;

                                    if (delOK > 0)
                                    {
                                        delValue = true;
                                        alertmsg = "Deleted Sucessfully";

                                        //==================Added by saranya on 11/04/2018=================//
                                        int savevalue = 1;
                                        string entrycode = Session["Entry_Code"].ToString();
                                        string formname = "ChallanDelete";
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
                                        string details = "RollNO - " + AppFormNo + " :ChallanNo - " + chlnNo + " : ChallanDate - " + chlnDt + " : Date - " + toa + "";
                                        string ctsname = "";
                                        if (savevalue == 1)
                                        {
                                            ctsname = "Challan Delete";
                                        }
                                        string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                                        d2.insertEinanceUserActionLog(entrycode, formname, 1, toa, doa, details, ctsname, localip);
                                        //==============================================================//
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
                                alertmsg = "Not Deleted";
                            }
                        }
                        else
                        {
                            imgAlert.Visible = true;
                            alertmsg = "Not Deleted";
                        }
                    }
                    else
                    {
                        imgAlert.Visible = true;
                        alertmsg = "Please Cancel The Challan To Delete";
                    }

                }
            }
            //btn_go_Click(sender, e);
            if (delValue)
            {
                lbl_alert.Text = "Deleted Sucessfully";
            }
            else
            {
                lbl_alert.Text = alertmsg;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "ChallanConfirm");
            imgAlert.Visible = true;
            lbl_alert.Text = "Not Deleted";
        }
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
    }
    protected void btnChlnDuplicate_Click(object sender, EventArgs e)
    {
        try
        {
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            int format = Convert.ToInt32(d2.GetFunction(insqry1));
            if (format == 0 || format == 1)
            {
                //FOr MCC and Others
                Duplicate();
            }
            else if (format == 2)
            {
                //For NEC College
                Duplicate1();
            }
            else if (format == 3)
            {
                //For UIT
                Duplicate2();
            }
            else if (format == 4)
            {
                //For New College
                Duplicate3();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    public void Duplicate()
    {
        //Last modified by Idhris  01-03-2017
        if (checkedOK())
        {
            FpSpread1.SaveChanges();
            try
            {
                bool createPDFOK = false;

                Font Fontbold = new Font("Arial", 8, FontStyle.Bold);
                Font Fontsmall = new Font("Arial", 8, FontStyle.Bold);
                Font Fontsmall1 = new Font("Arial", 10, FontStyle.Bold);
                Font Fontbold1 = new Font("Arial", 10, FontStyle.Bold);

                Gios.Pdf.PdfDocument mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
                mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(13.8, 8.5));

                string useIFSC = d2.GetFunction("select LinkValue from New_InsSettings where LinkName= 'DisplayIFSCForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ").Trim();


                string bursarSchool = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanOfficeFooter' and user_code ='" + usercode + "' and college_code =" + collegecode1 + "").Trim();
                if (bursarSchool == "0" || bursarSchool == "")
                    bursarSchool = "Bursar's";

                #region For Every selected Challan
                for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                    if (check == 1)
                    {
                        #region base data
                        int challanType = 1;
                        string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                        string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);
                        string app_formno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                        string studname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                        string deg = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);
                        string total = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Tag);
                        string accNo = string.Empty;

                        //string trasdate = txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2];
                        // string transtime = DateTime.Now.ToLongTimeString();

                        //ListItem lst1 = new ListItem("Roll No", "0");
                        //ListItem lst2 = new ListItem("Reg No", "1");
                        //ListItem lst3 = new ListItem("Admission No", "2");
                        //ListItem lst4 = new ListItem("App No", "3");
                        string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);

                        //AppNo = d2.GetFunction("select app_no from Registration where Roll_No ='" + app_formno + "'");

                        //string transcode = generateReceiptNo();

                        string regno = string.Empty;
                        string rollno = string.Empty;
                        string appnoNew = string.Empty;
                        string roll_admit = string.Empty;
                        string smartno = string.Empty;
                        string queryRollApp = "select r.smart_serial_no,r.Roll_No,a.app_formno,a.app_no,r.Reg_No,r.Roll_Admit  from Registration r,applyn a where r.App_No=a.app_no and r.App_No='" + AppNo + "'";
                        DataSet dsRollApp = new DataSet();
                        dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                        if (dsRollApp.Tables.Count > 0)
                        {
                            if (dsRollApp.Tables[0].Rows.Count > 0)
                            {
                                rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                roll_admit = Convert.ToString(dsRollApp.Tables[0].Rows[0]["roll_admit"]);
                                smartno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["smart_serial_no"]);
                            }
                        }
                        string rolldisplay = "Reg No :";
                        string rollvalue = regno;

                        if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                        {
                            rolldisplay = "Roll No :";
                            rollvalue = rollno;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                        {
                            rolldisplay = "Reg No :";
                            rollvalue = regno;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                        {
                            rolldisplay = "Admission No :";
                            rollvalue = roll_admit;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                        {
                            rolldisplay = "Smartcard No :";
                            rollvalue = smartno;
                        }
                        else
                        {
                            rolldisplay = "App No :";
                            appnoNew = AppNo;
                            app_formno = d2.GetFunction("select app_formno from applyn where app_no='" + AppNo + "'").Trim();
                            rollvalue = app_formno;
                        }

                        if (ddl_befAftAdmis.SelectedIndex == 0)
                        {
                            rolldisplay = "App No :";
                            appnoNew = AppNo;
                            app_formno = d2.GetFunction("select app_formno from applyn where app_no='" + AppNo + "'").Trim();

                            rollvalue = app_formno;
                        }
                        //string colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + AppNo + "' and d.college_code=" + collegecode1 + "";
                        string colquery = "";
                        if (rolldisplay != "App No :")
                        {
                            colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                        }
                        else
                        {
                            colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                        }

                        string collegename = string.Empty;
                        string add1 = string.Empty;
                        string add2 = string.Empty;
                        string univ = string.Empty;
                        string degreeCode = string.Empty;
                        string stream = string.Empty;
                        string cursem = string.Empty;
                        string batyr = string.Empty;

                        string bankName = string.Empty;
                        string bankCity = string.Empty;

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(colquery, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                degreeCode = Convert.ToString(ds.Tables[1].Rows[0]["Degree_code"]);
                                cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                            }
                        }

                        #endregion

                        #region Hide Institution Name in Challan Header -- For MCC School
                        string hideCollege = string.Empty;
                        string groupHeaders = dirAccess.selectScalarString(" select f.ChlGroupHeader  as DispName from FT_ChallanDet d ,FS_ChlGroupHeaderSettings f where d.HeaderFK =f.HeaderFK and   challanNo='" + recptNo + "' and App_No ='" + AppNo + "' ").Trim();
                        if (!string.IsNullOrEmpty(groupHeaders))
                        {
                            string pageCode = dirAccess.selectScalarString(" select PageCode from FM_ChlBankPrintSettings where CollegeCode='" + collegecode1 + "' and SettingType='1' and (ChlGroupHeader like '%," + groupHeaders + "'  or ChlGroupHeader like '" + groupHeaders + ",%' or ChlGroupHeader like '%," + groupHeaders + ",%'  or ChlGroupHeader ='" + groupHeaders + "')").Trim();
                            int iPageCode = 0;
                            if (!string.IsNullOrEmpty(pageCode) && int.TryParse(pageCode, out iPageCode))
                            {
                                hideCollege = dirAccess.selectScalarString("select LinkValue from New_InsSettings where LinkName='HideInstituteAddressInChallan" + iPageCode + "' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ").Trim();
                            }
                        }
                        if (!string.IsNullOrEmpty(hideCollege))
                        {
                            collegename = hideCollege;
                            add1 = string.Empty;
                            add2 = string.Empty;
                        }
                        #endregion

                        string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,TakenAmt,BankFk,FinYearFk,challanType from FT_ChallanDet where challanNo='" + recptNo + "' and App_No ='" + AppNo + "'";
                        DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                        if (dsDet.Tables.Count > 0)
                        {
                            if (dsDet.Tables[0].Rows.Count > 0)
                            {
                                challanType = Convert.ToInt32(Convert.ToString(dsDet.Tables[0].Rows[0]["challanType"]));

                                string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[0]["FeeCategory"]);
                                cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + FeeCategory + " and college_code=" + collegecode1 + "");
                                cursem = cursem.Split(' ')[1] + " : " + romanLetter(cursem.Split(' ')[0]);

                                string bnkFk = Convert.ToString(dsDet.Tables[0].Rows[0]["BankFk"]);

                                string bnkDetQ = "select BankName,City,BankCode,AccNo,Upper(BankBranch) as BankBranch from FM_FinBankMaster where BankPK=" + bnkFk + "";
                                DataSet dsBnkDet = d2.select_method_wo_parameter(bnkDetQ, "Text");
                                if (dsBnkDet.Tables.Count > 0)
                                {
                                    if (dsBnkDet.Tables[0].Rows.Count > 0)
                                    {
                                        bankName = Convert.ToString(dsBnkDet.Tables[0].Rows[0]["BankName"]);
                                        bankCity = "(" + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["City"]) + ")";
                                        accNo = "A/c No " + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["AccNo"]);
                                    }
                                }

                                createPDFOK = true;

                                #region Challan Top portion

                                int y = 0;

                                Gios.Pdf.PdfPage myprov_pdfpage = mychallan.NewPage();

                                PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                PdfTextArea ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL");
                                PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 70, 40, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                //PdfTextArea FC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                //                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                     new PdfArea(mychallan, 30, 68, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, 80, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                //PdfTextArea FC33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                //                                                   new PdfArea(mychallan, 70, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                  new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + recptDt);

                                PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);

                                PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 20, 145, 240, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 20, 160, 240, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 20, 175, 240, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                myprov_pdfpage.Add(FC17);
                                string text = "";

                                //First Ends

                                PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                      new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                PdfTextArea UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE");
                                PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 400, 40, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);

                                PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                     new PdfArea(mychallan, 360, 68, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 80, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                  new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + recptDt);


                                PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);

                                PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 350, 145, 240, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 350, 160, 240, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 350, 175, 240, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                //second End
                                y = 0;


                                PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                      new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                PdfTextArea TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE");
                                PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 720, 40, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);

                                PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                     new PdfArea(mychallan, 680, 68, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 670, 80, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                  new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + recptDt);

                                PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);

                                PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                             new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                y = 0;

                                PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                      new PdfArea(mychallan, 70, 50, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                      new PdfArea(mychallan, 400, 50, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                      new PdfArea(mychallan, 720, 50, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);

                                myprov_pdfpage.Add(FC4);
                                myprov_pdfpage.Add(UC4);
                                myprov_pdfpage.Add(TC4);
                                PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                          new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No.:" + recptNo);
                                PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                   new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No.:" + recptNo);
                                PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                   new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No.:" + recptNo);

                                //PdfImage FC08 = mychallan.NewImage(generateBarcode(recptNo));

                                //myprov_pdfpage.Add(FC08, 250, 125);
                                //myprov_pdfpage.Add(FC08, 550, 125);
                                //myprov_pdfpage.Add(FC08, 900, 125);mychallan, 240, 110, 85, 20

                                PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                myprov_pdfpage.Add(FC10);
                                myprov_pdfpage.Add(UC10);
                                myprov_pdfpage.Add(TC10);

                                #endregion

                                #region Challan Middle Portion

                                //for (int j = 0; j < dsDet.Tables[0].Rows.Count; j++)
                                //{
                                //    string ledger = Convert.ToString(dsDet.Tables[0].Rows[j]["LedgerFK"]);
                                //    string header = Convert.ToString(dsDet.Tables[0].Rows[j]["HeaderFk"]);
                                //    string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[j]["FeeCategory"]);
                                //    string taknAmt = Convert.ToString(dsDet.Tables[0].Rows[j]["TakenAmt"]);
                                //}

                                string selHeadersQ = string.Empty;
                                DataSet dsHeaders = new DataSet();

                                if (challanType == 1 || challanType == 2)
                                {
                                    string StudStream = string.Empty;

                                    DataSet dsStr = new DataSet();
                                    if (ddl_befAftAdmis.SelectedIndex != 0) // Added by jairam 09-08-2016
                                    {
                                        dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                        if (dsStr.Tables.Count > 0)
                                        {
                                            if (dsStr.Tables[0].Rows.Count > 0)
                                            {
                                                StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                            }
                                        }
                                    }
                                    if (ddl_befAftAdmis.SelectedIndex == 0)
                                    {
                                        dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from applyn r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                        if (dsStr.Tables.Count > 0)
                                        {
                                            if (dsStr.Tables[0].Rows.Count > 0)
                                            {
                                                StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                            }
                                        }
                                    }

                                    selHeadersQ = " select sum(TakenAmt) as TakenAmt,f.ChlGroupHeader  as DispName from FT_ChallanDet d ,FS_ChlGroupHeaderSettings f where d.HeaderFK =f.HeaderFK and   challanNo='" + recptNo + "' and App_No ='" + AppNo + "'  ";
                                    if (StudStream != "")
                                    {
                                        selHeadersQ += " and f.stream='" + StudStream + "' ";
                                    }
                                    selHeadersQ += "   group by ChlGroupHeader ";
                                }
                                else if (challanType == 3)
                                {
                                    selHeadersQ = " select HeaderFk,SUM(TakenAmt) as TakenAmt,h.HeaderName  as DispName  from FT_ChallanDet d,FM_HeaderMaster h  where d.HeaderFK =h.HeaderPK and challanNo='" + recptNo + "' and d.App_No ='" + AppNo + "' group by HeaderFk,h.HeaderName ";
                                }
                                else if (challanType == 4)
                                {
                                    selHeadersQ = " select LedgerFK,sum(TakenAmt) as TakenAmt,l.LedgerName as DispName  from FT_ChallanDet d,FM_LedgerMaster l where d.LedgerFK =l.LedgerPK and challanNo='" + recptNo + "' and d.App_No ='" + AppNo + "' group by LedgerFK,l.LedgerName ";
                                }

                                if (selHeadersQ != string.Empty)
                                {
                                    dsHeaders.Clear();
                                    dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                    if (dsHeaders.Tables.Count > 0)
                                    {
                                        if (dsHeaders.Tables[0].Rows.Count > 0)
                                        {
                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                            {
                                                string dispHdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                //if (challanType > 2)
                                                //{
                                                accNo = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                //}
                                                //else
                                                //{
                                                //    bnkFk = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]) + "' AND BankPK = '" + bankPK + "'");
                                                //}



                                                dispHdr += " (" + accNo + ")";

                                                string totalAmt = Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                                PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                        new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, totalAmt);
                                                PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, y + 190, 240, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(FC18);
                                                myprov_pdfpage.Add(FC171);
                                                myprov_pdfpage.Add(FC19);


                                                PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, totalAmt);
                                                PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 350, y + 190, 240, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(UC18);
                                                myprov_pdfpage.Add(UC19);
                                                myprov_pdfpage.Add(UC171);

                                                PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, totalAmt);
                                                PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 680, y + 190, 240, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(TC18);
                                                myprov_pdfpage.Add(TC19);
                                                myprov_pdfpage.Add(TC171);
                                                y = y + 15;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region Bottom Portion of Challan

                                text = "(" + DecimalToWords((decimal)Convert.ToDouble(total)) + " Rupees Only)";

                                PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 20, y + 195, 240, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                             new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                PdfTextArea FC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 25, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the " + bursarSchool + " Office");
                                PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 300);
                                PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                myprov_pdfpage.Add(pr1);

                                PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 300);
                                PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                myprov_pdfpage.Add(pr2);

                                PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 300);
                                PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                myprov_pdfpage.Add(pr3);

                                Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                table.VisibleHeaders = false;
                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table.Columns[0].SetWidth(100);
                                table.Columns[1].SetWidth(60);
                                table.Columns[2].SetWidth(60);

                                table.Cell(0, 0).SetContent("Cheque/DD No");
                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 0).SetFont(Fontbold1);
                                table.Cell(0, 1).SetContent("Date");
                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 1).SetFont(Fontbold1);
                                table.Cell(0, 2).SetContent("Amount");
                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 2).SetFont(Fontbold1);
                                table.Cell(1, 0).SetContent("\n");
                                table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 0).SetFont(Fontbold1);
                                table.Cell(1, 1).SetContent("\n");
                                table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 1).SetFont(Fontbold1);
                                table.Cell(1, 2).SetContent("\n");
                                table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 2).SetFont(Fontbold1);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 330, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable);

                                Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 10, 2, 3);
                                table1.VisibleHeaders = false;
                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table1.Columns[0].SetWidth(100);
                                table1.Columns[1].SetWidth(60);
                                table1.Cell(0, 0).SetContent("2000x");
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(0, 0).SetFont(Fontbold1);
                                table1.Cell(1, 0).SetContent("500x");
                                table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(1, 0).SetFont(Fontbold1);
                                table1.Cell(2, 0).SetContent("200x");
                                table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(2, 0).SetFont(Fontbold1);
                                table1.Cell(3, 0).SetContent("100x");
                                table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(3, 0).SetFont(Fontbold1);
                                table1.Cell(4, 0).SetContent("50x");
                                table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(4, 0).SetFont(Fontbold1);
                                table1.Cell(5, 0).SetContent("20x");
                                table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(5, 0).SetFont(Fontbold1);
                                table1.Cell(6, 0).SetContent("10x");
                                table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(6, 0).SetFont(Fontbold1);
                                table1.Cell(7, 0).SetContent("5x");
                                table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(7, 0).SetFont(Fontbold1);
                                table1.Cell(8, 0).SetContent("Coinsx");
                                table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(8, 0).SetFont(Fontbold1);
                                table1.Cell(9, 0).SetContent("Total");
                                table1.Cell(9, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(9, 0).SetFont(Fontbold1);



                                Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable1);

                                myprov_pdfpage.Add(FC);
                                myprov_pdfpage.Add(ORGI);
                                myprov_pdfpage.Add(IOB);
                                //myprov_pdfpage.Add(FC4);
                                myprov_pdfpage.Add(FC5);
                                myprov_pdfpage.Add(FC6);
                                myprov_pdfpage.Add(FC7);
                                myprov_pdfpage.Add(FC8);
                                myprov_pdfpage.Add(FC9);

                                myprov_pdfpage.Add(FC11);
                                myprov_pdfpage.Add(FC12);
                                myprov_pdfpage.Add(FC13);
                                myprov_pdfpage.Add(FC14);
                                myprov_pdfpage.Add(FC15);
                                myprov_pdfpage.Add(FC16);

                                myprov_pdfpage.Add(FC24);
                                myprov_pdfpage.Add(FC25);
                                myprov_pdfpage.Add(FC26);
                                myprov_pdfpage.Add(FC27);
                                myprov_pdfpage.Add(FC28);
                                myprov_pdfpage.Add(FC29);
                                myprov_pdfpage.Add(FC30);
                                myprov_pdfpage.Add(FC31);

                                myprov_pdfpage.Add(FC32);
                                //myprov_pdfpage.Add(FC33);

                                //First End
                                myprov_pdfpage.Add(UC17);

                                PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                             new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                PdfTextArea UC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 355, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the " + bursarSchool + " Office");


                                Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                table3.VisibleHeaders = false;
                                table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table3.Columns[0].SetWidth(100);
                                table3.Columns[1].SetWidth(60);
                                table3.Columns[2].SetWidth(60);

                                table3.Cell(0, 0).SetContent("Cheque/DD No");
                                table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 0).SetFont(Fontbold1);
                                table3.Cell(0, 1).SetContent("Date");
                                table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 1).SetFont(Fontbold1);
                                table3.Cell(0, 2).SetContent("Amount");
                                table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 2).SetFont(Fontbold1);
                                table3.Cell(1, 0).SetContent("\n");
                                table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 0).SetFont(Fontbold1);
                                table3.Cell(1, 1).SetContent("\n");
                                table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 1).SetFont(Fontbold1);
                                table3.Cell(1, 2).SetContent("\n");
                                table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 2).SetFont(Fontbold1);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 330, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable3);

                                Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 10, 2, 3);
                                table14.VisibleHeaders = false;
                                table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table14.Columns[0].SetWidth(100);
                                table14.Columns[1].SetWidth(60);
                                table14.Cell(0, 0).SetContent("2000x");
                                table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(0, 0).SetFont(Fontbold1);
                                table14.Cell(1, 0).SetContent("500x");
                                table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(1, 0).SetFont(Fontbold1);
                                table14.Cell(2, 0).SetContent("200x");
                                table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(2, 0).SetFont(Fontbold1);
                                table14.Cell(3, 0).SetContent("100x");
                                table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(3, 0).SetFont(Fontbold1);
                                table14.Cell(4, 0).SetContent("50x");
                                table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(4, 0).SetFont(Fontbold1);
                                table14.Cell(5, 0).SetContent("20x");
                                table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(5, 0).SetFont(Fontbold1);
                                table14.Cell(6, 0).SetContent("10x");
                                table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(6, 0).SetFont(Fontbold1);
                                table14.Cell(7, 0).SetContent("5x");
                                table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(7, 0).SetFont(Fontbold1);
                                table14.Cell(8, 0).SetContent("Coinsx");
                                table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(8, 0).SetFont(Fontbold1);
                                table14.Cell(9, 0).SetContent("Total");
                                table14.Cell(9, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(9, 0).SetFont(Fontbold1);

                                Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable4);

                                myprov_pdfpage.Add(UC);
                                myprov_pdfpage.Add(UC1);
                                myprov_pdfpage.Add(UC2);
                                //myprov_pdfpage.Add(UC4);
                                myprov_pdfpage.Add(UC5);
                                myprov_pdfpage.Add(UC6);
                                myprov_pdfpage.Add(UC7);
                                myprov_pdfpage.Add(UC8);
                                myprov_pdfpage.Add(UC9);

                                myprov_pdfpage.Add(UC11);
                                myprov_pdfpage.Add(UC12);
                                myprov_pdfpage.Add(UC13);
                                myprov_pdfpage.Add(UC14);
                                myprov_pdfpage.Add(UC15);
                                myprov_pdfpage.Add(UC16);


                                myprov_pdfpage.Add(UC24);
                                myprov_pdfpage.Add(UC25);
                                myprov_pdfpage.Add(UC26);
                                myprov_pdfpage.Add(UC27);
                                myprov_pdfpage.Add(UC28);
                                myprov_pdfpage.Add(UC29);
                                myprov_pdfpage.Add(UC30);
                                myprov_pdfpage.Add(UC31);
                                myprov_pdfpage.Add(UC32);
                                //second End


                                myprov_pdfpage.Add(TC17);

                                PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mychallan, 685, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                             new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                PdfTextArea TC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 685, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the " + bursarSchool + " Office");


                                Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                table5.VisibleHeaders = false;
                                table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table5.Columns[0].SetWidth(100);
                                table5.Columns[1].SetWidth(60);
                                table5.Columns[2].SetWidth(60);

                                table5.Cell(0, 0).SetContent("Cheque/DD No");
                                table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 0).SetFont(Fontbold1);
                                table5.Cell(0, 1).SetContent("Date");
                                table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 1).SetFont(Fontbold1);
                                table5.Cell(0, 2).SetContent("Amount");
                                table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 2).SetFont(Fontbold1);
                                table5.Cell(1, 0).SetContent("\n");
                                table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 0).SetFont(Fontbold1);
                                table5.Cell(1, 1).SetContent("\n");
                                table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 1).SetFont(Fontbold1);
                                table5.Cell(1, 2).SetContent("\n");
                                table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 2).SetFont(Fontbold1);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 330, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable31);

                                Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 10, 2, 3);
                                table15.VisibleHeaders = false;
                                table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table15.Columns[0].SetWidth(100);
                                table15.Columns[1].SetWidth(60);
                                table15.Cell(0, 0).SetContent("2000x");
                                table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(0, 0).SetFont(Fontbold1);
                                table15.Cell(1, 0).SetContent("500x");
                                table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(1, 0).SetFont(Fontbold1);
                                table15.Cell(2, 0).SetContent("200x");
                                table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(2, 0).SetFont(Fontbold1);
                                table15.Cell(3, 0).SetContent("100x");
                                table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(3, 0).SetFont(Fontbold1);
                                table15.Cell(4, 0).SetContent("50x");
                                table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(4, 0).SetFont(Fontbold1);
                                table15.Cell(5, 0).SetContent("20x");
                                table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(5, 0).SetFont(Fontbold1);
                                table15.Cell(6, 0).SetContent("10x");
                                table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(6, 0).SetFont(Fontbold1);
                                table15.Cell(7, 0).SetContent("5x");
                                table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(7, 0).SetFont(Fontbold1);
                                table15.Cell(8, 0).SetContent("Coinsx");
                                table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(8, 0).SetFont(Fontbold1);
                                table15.Cell(9, 0).SetContent("Total");
                                table15.Cell(9, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(9, 0).SetFont(Fontbold1);

                                Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable5);

                                myprov_pdfpage.Add(TC);
                                myprov_pdfpage.Add(TC1);
                                myprov_pdfpage.Add(TC2);
                                //myprov_pdfpage.Add(TC4);
                                myprov_pdfpage.Add(TC5);
                                myprov_pdfpage.Add(TC6);
                                myprov_pdfpage.Add(TC7);
                                myprov_pdfpage.Add(TC8);
                                myprov_pdfpage.Add(TC9);

                                myprov_pdfpage.Add(TC11);
                                myprov_pdfpage.Add(TC12);
                                myprov_pdfpage.Add(TC13);
                                myprov_pdfpage.Add(TC14);
                                myprov_pdfpage.Add(TC15);
                                myprov_pdfpage.Add(TC16);
                                myprov_pdfpage.Add(TC17);
                                myprov_pdfpage.Add(TC24);
                                myprov_pdfpage.Add(TC25);
                                myprov_pdfpage.Add(TC26);
                                myprov_pdfpage.Add(TC27);
                                myprov_pdfpage.Add(TC28);
                                myprov_pdfpage.Add(TC29);
                                myprov_pdfpage.Add(TC30);
                                myprov_pdfpage.Add(TC31);
                                myprov_pdfpage.Add(TC32);

                                myprov_pdfpage.SaveToDocument();
                                #endregion
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
                #endregion

                #region To print the challan
                if (createPDFOK)
                {
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";

                        string szFile = "Challan" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                        mychallan.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                        //Response.AddHeader("Refresh", "1; url=receiptPrint.aspx");
                        //Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");

                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Challan Cannot Be Generated";
                }
                #endregion
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Challan";
        }
    }
    private bool showLedgerFees()
    {
        bool showFees = false;
        string Q = "select LinkValue from New_InsSettings where LinkName='ShowLedgerwiseFeesinChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
        if (d2.GetFunction(Q).Trim() == "1")
            showFees = true;
        return showFees;
    }
    public void Duplicate1()
    {
        if (checkedOK())
        {
            FpSpread1.SaveChanges();
            try
            {
                bool createPDFOK = false;

                Font Fontbold = new Font("Arial", 8, FontStyle.Bold);
                Font Fontsmall = new Font("Arial", 8, FontStyle.Regular);
                Font Fontsmall1 = new Font("Arial", 8, FontStyle.Regular);
                Font Fontbold1 = new Font("Arial", 8, FontStyle.Bold);
                Font Fontboldled = new Font("Arial", 7, FontStyle.Regular);
                Font FontboldBig = new Font("Arial", 12, FontStyle.Bold);
                Font FontboldBig1 = new Font("Arial", 10, FontStyle.Bold);

                Gios.Pdf.PdfDocument mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
                // mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(13.8, 8.5));
                mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(14.2, 8.5));//14.0 previous
                #region For Every selected Challan
                contentDiv.InnerHtml = "";
                StringBuilder sbHtml = new StringBuilder();
                for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    sbHtml.Clear();
                    byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                    if (check == 1)
                    {
                        string shift = "";
                        string acaYear = System.DateTime.Now.Year.ToString();
                        shift = d2.GetFunction("select UPPER(type) from course where college_code=" + collegecode1 + "").Trim();
                        if (shift == "0" || shift == "")
                        {
                            shift = "";
                        }
                        else
                        {
                            shift = "(" + shift + ")";
                        }
                        string counterName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName= 'ChallanFeeCounterValue' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ").Trim();
                        if (counterName == "0")
                            counterName = string.Empty;

                        string colName = string.Empty;
                        colName = d2.GetFunction("select collname from collinfo where college_code=" + collegecode1 + "").Trim();
                        if (colName == "0" || colName == "")
                            colName = string.Empty;
                        //if (colName != string.Empty)
                        //{
                        //    string tempCName = colName.ToUpper().Replace(" ", "");
                        //    if (tempCName.Contains("NEWCOLLEGE"))
                        //    {
                        //        colName = "THE NEW COLLEGE (AUTONOMOUS) CH-14";
                        //    }
                        //}

                        string parName = string.Empty;
                        parName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanParticular' and user_code ='" + usercode + "' and college_code =" + collegecode1 + "").Trim();
                        if (parName == "0" || parName == "")
                            parName = "Particulars";
                        else
                            parName = "Particulars - " + parName;

                        string useIFSC = d2.GetFunction("select LinkValue from New_InsSettings where LinkName= 'DisplayIFSCForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ").Trim();

                        string useDegAcr = d2.GetFunction("select LinkValue from New_InsSettings where LinkName= 'DisplayAcrForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ").Trim();

                        int useDenom = Convert.ToInt32(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayDenominationChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'").Trim());

                        #region base data
                        int challanType = 1;
                        string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                        string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);
                        string app_formno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                        string studname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                        string deg = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);
                        if (useDegAcr == "1")
                        {
                            deg = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Tag);
                        }
                        string total = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Tag);
                        string accNo = string.Empty;

                        //string trasdate = txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2];
                        // string transtime = DateTime.Now.ToLongTimeString();
                        string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);

                        //AppNo = d2.GetFunction("select app_no from Registration where Roll_No ='" + app_formno + "'");
                        //string transcode = generateReceiptNo();

                        string regno = string.Empty;
                        string rollno = string.Empty;
                        string appnoNew = string.Empty;
                        string roll_admit = string.Empty;
                        string smartno = string.Empty;

                        string queryRollApp = "select r.smart_serial_no,r.Roll_No,a.app_formno,a.app_no,r.Reg_No,r.Roll_Admit  from Registration r,applyn a where r.App_No=a.app_no and r.App_No='" + AppNo + "'";
                        DataSet dsRollApp = new DataSet();
                        dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                        if (dsRollApp.Tables.Count > 0)
                        {
                            if (dsRollApp.Tables[0].Rows.Count > 0)
                            {
                                rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                roll_admit = Convert.ToString(dsRollApp.Tables[0].Rows[0]["roll_admit"]);
                                smartno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["smart_serial_no"]);
                            }
                        }
                        string rolldisplay = "Reg No :";
                        string rollvalue = regno;
                        if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                        {
                            rolldisplay = "Roll No :";
                            rollvalue = rollno;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                        {
                            rolldisplay = "Reg No :";
                            rollvalue = regno;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                        {
                            rolldisplay = "Admission No :";
                            rollvalue = roll_admit;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                        {
                            rolldisplay = "Smartcard No :";
                            rollvalue = smartno;
                        }
                        else
                        {
                            rolldisplay = "App No :";
                            appnoNew = AppNo;
                            app_formno = d2.GetFunction("select app_formno from applyn where app_no='" + AppNo + "'").Trim();
                            rollvalue = app_formno;
                        }

                        if (ddl_befAftAdmis.SelectedIndex == 0)
                        {
                            rolldisplay = "App No :";
                            appnoNew = AppNo;
                            app_formno = d2.GetFunction("select app_formno from applyn where app_no='" + AppNo + "'").Trim();

                            rollvalue = app_formno;
                        }

                        //string colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + AppNo + "' and d.college_code=" + collegecode1 + "";
                        string colquery = "";
                        if (rolldisplay != "App No :")
                        {
                            colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                        }
                        else
                        {
                            colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                        }

                        string collegename = string.Empty;
                        string add1 = string.Empty;
                        string add2 = string.Empty;
                        string univ = string.Empty;
                        string degreeCode = string.Empty;
                        string stream = string.Empty;
                        string cursem = string.Empty;
                        string batyr = string.Empty;

                        string bankName = string.Empty;
                        string bankCity = string.Empty;

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(colquery, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                degreeCode = Convert.ToString(ds.Tables[1].Rows[0]["Degree_code"]);
                                cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);


                                acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                try
                                {
                                    acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                }
                                catch { }

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
                            }
                        }

                        #endregion


                        string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,TakenAmt,BankFk,FinYearFk,challanType from FT_ChallanDet where challanNo='" + recptNo + "' and App_No ='" + AppNo + "' order by LedgerFK asc";
                        DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                        if (dsDet.Tables.Count > 0)
                        {
                            if (dsDet.Tables[0].Rows.Count > 0)
                            {
                                challanType = Convert.ToInt32(Convert.ToString(dsDet.Tables[0].Rows[0]["challanType"]));

                                string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[0]["FeeCategory"]);


                                string bnkFk = Convert.ToString(dsDet.Tables[0].Rows[0]["BankFk"]);

                                string bnkDetQ1 = "select BankName,City,BankCode,AccNo,Upper(BankBranch) as BankBranch from FM_FinBankMaster where BankPK=" + bnkFk + "";
                                string bankAddress = string.Empty;
                                DataSet dsBnkDet1 = d2.select_method_wo_parameter(bnkDetQ1, "Text");
                                if (dsBnkDet1.Tables.Count > 0)
                                {
                                    if (dsBnkDet1.Tables[0].Rows.Count > 0)
                                    {
                                        bankName = Convert.ToString(dsBnkDet1.Tables[0].Rows[0]["BankName"]);
                                        bankCity = Convert.ToString(dsBnkDet1.Tables[0].Rows[0]["BankBranch"]) + " Branch";
                                        accNo = "A/c No " + Convert.ToString(dsBnkDet1.Tables[0].Rows[0]["AccNo"]);
                                        bankAddress = d2.GetFunction("select Street+', '+(select MasterValue from CO_MasterValues where MasterCode=District)+'-'+PinCode as address from FM_FinBankMaster where BankPK=" + bnkFk + "");
                                        bankAddress = "(" + bankAddress + ")";
                                    }
                                }

                                createPDFOK = true;

                                #region HTML Generation
                                //<div style='height: 710px;width:380px;border:1px solid black;float:left;'></div><div style='margin-left:10px;height: 710px;width:380px;border:1px solid black;float:left;'></div><div style='margin-left:10px;height: 710px;width:380px;border:1px solid black;float:left;'></div><br>
                                sbHtml.Append("<div style='padding-left:50px;height: 710px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 1056px; ' class='classRegular'>");

                                sbHtml.Append("<tr class='classBold10'><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td  style='font-size:16px;'><center><b>" + colName + "</b></center></td></tr><tr><td style='font-size:15px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td><center>COLLEGE FEES CHALLAN " + shift + "</center></td></tr></table></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td  style='font-size:16px;'><center><b>" + colName + "</b></center></td></tr><tr><td  style='font-size:15px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td><center>COLLEGE FEES CHALLAN " + shift + "</center></td></tr></table></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td  style='font-size:16px;'><center><b>" + colName + "</b></center></td></tr><tr><td  style='font-size:15px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td><center>COLLEGE FEES CHALLAN " + shift + "</center></td></tr></table></td></tr>");

                                sbHtml.Append("<tr class='classBold10' style='text-align:center;'><td ><table  class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>ChallanNo.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + recptDt + "</td><tr></table></td><td></td><td><table  class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>ChallanNo.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + recptDt + "</td><tr></table ></td><td></td><td><table class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>ChallanNo.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + recptDt + "</td><tr></table></td></tr>");
                                if (checkSchoolSetting() == 0)
                                {
                                    sbHtml.Append("<tr class='classBold10'><td ><table  Rules='Rows' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td  style='border: 1px solid black;' colspan='2'>ORIGINAL - SCHOOL <span style='padding-left:130px;'>By D/D or Cash</span></td></tr><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr  style='border: 1px solid black;'><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td  style='border: 1px solid black;' COLSPAN='2'>DUPLICATE - BANK <span style='padding-left:140px;'>By D/D or Cash</span></td></tr><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td COLSPAN='2'  style='border: 1px solid black;'>TRIPLICATE - STUDENT <span style='padding-left:130px;'>By D/D or Cash</span></td></tr><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td></tr>");
                                }
                                else
                                {
                                    sbHtml.Append("<tr class='classBold10'><td ><table  Rules='Rows' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td  style='border: 1px solid black;' colspan='2'>ORIGINAL - COLLEGE <span style='padding-left:130px;'>By D/D or Cash</span></td></tr><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr  style='border: 1px solid black;'><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td  style='border: 1px solid black;' COLSPAN='2'>DUPLICATE - BANK <span style='padding-left:140px;'>By D/D or Cash</span></td></tr><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td COLSPAN='2'  style='border: 1px solid black;'>TRIPLICATE - STUDENT <span style='padding-left:130px;'>By D/D or Cash</span></td></tr><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td></tr>");
                                }
                                #endregion

                                #region Challan Top portion

                                int y = 0;

                                Gios.Pdf.PdfPage myprov_pdfpage = mychallan.NewPage();




                                PdfTextArea IOB = new PdfTextArea(FontboldBig, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 70, 5, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);

                                PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 20, 15, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, bankAddress);

                                PdfTextArea FC32 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, 90, 380, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL FOR THE COLLEGE                                                  By D/D or Cash");
                                PdfTextArea FC011 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 20, 80, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(FC011);
                                PdfTextArea FC012 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 20, 92, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(FC012);
                                PdfTextArea UC011 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 80, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(UC011);
                                PdfTextArea UC012 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 350, 92, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(UC012);
                                PdfTextArea TC011 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 690, 80, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(TC011);
                                PdfTextArea TC012 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 690, 92, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(TC012);
                                PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 105, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);

                                PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, 110, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, 120, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 100, 105, 220, 20), System.Drawing.ContentAlignment.MiddleRight, "Class :  " + deg);
                                PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, 125, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, 140, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS - Arts & Science Major"));
                                PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 280, 140, 130, 20), System.Drawing.ContentAlignment.MiddleLeft, "Amount Rs.");
                                PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                myprov_pdfpage.Add(FC17);
                                string text = "";

                                //First Ends

                                PdfTextArea UC2 = new PdfTextArea(FontboldBig, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 400, 5, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);

                                PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 15, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, bankAddress);


                                PdfTextArea UC32 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, 90, 380, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE FOR THE BANK                                                      By D/D or Cash");

                                PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 105, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 110, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, 120, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 430, 105, 220, 20), System.Drawing.ContentAlignment.MiddleRight, "Class :  " + deg);
                                PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 125, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, 140, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS - Arts & Science Major"));
                                PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 610, 140, 130, 20), System.Drawing.ContentAlignment.MiddleLeft, "Amount Rs.");
                                PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                //second End
                                y = 0;

                                PdfTextArea TC2 = new PdfTextArea(FontboldBig, System.Drawing.Color.Black,
                                                                                                                           new PdfArea(mychallan, 740, 5, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 690, 15, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, bankAddress);


                                PdfTextArea TC9 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 695, 90, 380, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE FOR THE STUDENT                                                By D/D or Cash");
                                PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 695, 105, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);

                                PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 690, 110, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 695, 120, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 780, 105, 210, 20), System.Drawing.ContentAlignment.MiddleRight, "Class :  " + deg);
                                PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 690, 125, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 695, 140, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS - Arts & Science Major"));
                                PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 950, 140, 130, 20), System.Drawing.ContentAlignment.MiddleLeft, "Amount Rs.");
                                PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 690, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mychallan, 250, 120, 70, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 580, 120, 70, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 920, 120, 70, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                myprov_pdfpage.Add(FC10);
                                myprov_pdfpage.Add(UC10);
                                myprov_pdfpage.Add(TC10);

                                y = -30;

                                PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                      new PdfArea(mychallan, 70, 33, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                      new PdfArea(mychallan, 400, 33, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                      new PdfArea(mychallan, 730, 33, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);

                                myprov_pdfpage.Add(FC4);
                                myprov_pdfpage.Add(UC4);
                                myprov_pdfpage.Add(TC4);

                                myprov_pdfpage.Add(FC10);
                                myprov_pdfpage.Add(UC10);
                                myprov_pdfpage.Add(TC10);

                                #endregion

                                #region Challan Middle Portion

                                //for (int j = 0; j < dsDet.Tables[0].Rows.Count; j++)
                                //{
                                //    string ledger = Convert.ToString(dsDet.Tables[0].Rows[j]["LedgerFK"]);
                                //    string header = Convert.ToString(dsDet.Tables[0].Rows[j]["HeaderFk"]);
                                //    string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[j]["FeeCategory"]);
                                //    string taknAmt = Convert.ToString(dsDet.Tables[0].Rows[j]["TakenAmt"]);
                                //}

                                string selHeadersQ = string.Empty;
                                DataSet dsHeaders = new DataSet();

                                //if (challanType == 1 || challanType == 2)
                                //{
                                //    string StudStream = string.Empty;

                                //    DataSet dsStr = new DataSet();
                                //    dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                //    if (dsStr.Tables.Count > 0)
                                //    {
                                //        if (dsStr.Tables[0].Rows.Count > 0)
                                //        {
                                //            StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                //        }
                                //    }

                                //    selHeadersQ = " select sum(TakenAmt) as TakenAmt,f.ChlGroupHeader  as DispName from FT_ChallanDet d ,FS_ChlGroupHeaderSettings f where d.HeaderFK =f.HeaderFK and   challanNo='" + recptNo + "' and App_No ='" + AppNo + "'  ";
                                //    if (StudStream != "")
                                //    {
                                //        selHeadersQ += " and f.stream='" + StudStream + "' ";
                                //    }
                                //    selHeadersQ += "   group by ChlGroupHeader ";
                                //}
                                //else if (challanType == 3)
                                //{
                                selHeadersQ = " select HeaderFk,SUM(TakenAmt) as TakenAmt,h.HeaderName  as DispName  from FT_ChallanDet d,FM_HeaderMaster h  where d.HeaderFK =h.HeaderPK and challanNo='" + recptNo + "' and d.App_No ='" + AppNo + "' group by HeaderFk,h.HeaderName ";
                                //}
                                //else if (challanType == 4)
                                //{
                                //    selHeadersQ = " select LedgerFK,sum(TakenAmt) as TakenAmt,l.LedgerName as DispName  from FT_ChallanDet d,FM_LedgerMaster l where d.LedgerFK =l.LedgerPK and challanNo='" + recptNo + "' and d.App_No ='" + AppNo + "' group by LedgerFK,l.LedgerName ";
                                //}
                                int heght = 380;


                                if (selHeadersQ != string.Empty)
                                {
                                    dsHeaders.Clear();
                                    dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                    if (dsHeaders.Tables.Count > 0)
                                    {
                                        if (dsHeaders.Tables[0].Rows.Count > 0)
                                        {
                                            int hdrsno = 0;

                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                            {
                                                StringBuilder tempHtml = new StringBuilder();
                                                StringBuilder tempHtmlAmt = new StringBuilder();

                                                hdrsno++;
                                                string dispHdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);

                                                //if (challanType > 2)
                                                //{

                                                //    if (useIFSC == "0")
                                                //        bnkFk = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                //    else
                                                //        bnkFk = "A/c No " + d2.GetFunction("SELECT AccNo+'-IFSC '+IFSCCode FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                //}
                                                //else
                                                //{
                                                //    bnkFk = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]) + "' AND BankPK = '" + bankPK + "'");
                                                //}

                                                string bnkDetQ = "select BankName,City,BankCode,AccNo,Upper(BankBranch) as BankBranch from FM_FinBankMaster where BankPK=" + bnkFk + "";
                                                DataSet dsBnkDet = d2.select_method_wo_parameter(bnkDetQ, "Text");
                                                if (dsBnkDet.Tables.Count > 0)
                                                {
                                                    if (dsBnkDet.Tables[0].Rows.Count > 0)
                                                    {
                                                        bankName = Convert.ToString(dsBnkDet.Tables[0].Rows[0]["BankName"]);
                                                        bankCity = "(" + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["BankBranch"]) + ")";
                                                        accNo = "A/c No " + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["AccNo"]);
                                                    }
                                                }
                                                string bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                if (useIFSC == "1")
                                                {
                                                    bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo+'-IFSC '+IFSCCode FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                }
                                                dispHdr += " (" + bnkAcc + ")";

                                                string totalAmt = Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                                PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                             new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, hdrsno + "." + dispHdr);

                                                myprov_pdfpage.Add(FC18);

                                                PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                             new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, hdrsno + "." + dispHdr);
                                                myprov_pdfpage.Add(UC18);

                                                PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 695, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, hdrsno + "." + dispHdr);
                                                myprov_pdfpage.Add(TC18);

                                                PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]));
                                                myprov_pdfpage.Add(FC19);
                                                PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                  new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]));
                                                myprov_pdfpage.Add(UC19);
                                                PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 940, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]));
                                                myprov_pdfpage.Add(TC19);
                                                y = y + 5;

                                                string tkn = Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);


                                                //For ledgerwise Details
                                                selHeadersQ = " select LedgerFK,sum(TakenAmt) as TakenAmt,l.LedgerName as DispName  from FT_ChallanDet d,FM_LedgerMaster l where d.LedgerFK =l.LedgerPK and challanNo='" + recptNo + "' and d.App_No ='" + AppNo + "' and d.HeaderFK=" + Convert.ToString(dsHeaders.Tables[0].Rows[head]["HeaderFk"]) + " group by LedgerFK,l.LedgerName ";
                                                DataSet dsLedge = new DataSet();
                                                dsLedge = d2.select_method_wo_parameter(selHeadersQ, "Text");
                                                if (dsLedge.Tables.Count > 0)
                                                {
                                                    if (dsLedge.Tables[0].Rows.Count > 0)
                                                    {
                                                        int ledsno = 0;
                                                        for (int ldr = 0; ldr < dsLedge.Tables[0].Rows.Count; ldr++)
                                                        {
                                                            ledsno++;
                                                            y = y + 7;
                                                            PdfTextArea FC018 = new PdfTextArea(Fontboldled, System.Drawing.Color.Black,
                                                                                                             new PdfArea(mychallan, 25, y + 185, 250, 10), System.Drawing.ContentAlignment.MiddleLeft, "  " + ledsno + "." + Convert.ToString(dsLedge.Tables[0].Rows[ldr]["DispName"]));
                                                            myprov_pdfpage.Add(FC018);
                                                            PdfTextArea UC018 = new PdfTextArea(Fontboldled, System.Drawing.Color.Black,
                                                                                                             new PdfArea(mychallan, 355, y + 185, 250, 10), System.Drawing.ContentAlignment.MiddleLeft, "  " + ledsno + "." + Convert.ToString(dsLedge.Tables[0].Rows[ldr]["DispName"]));
                                                            myprov_pdfpage.Add(UC018);

                                                            PdfTextArea TC018 = new PdfTextArea(Fontboldled, System.Drawing.Color.Black,
                                                                                                                        new PdfArea(mychallan, 695, y + 185, 250, 10), System.Drawing.ContentAlignment.MiddleLeft, "  " + ledsno + "." + Convert.ToString(dsLedge.Tables[0].Rows[ldr]["DispName"]));
                                                            myprov_pdfpage.Add(TC018);
                                                            //PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            //                                    new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsLedge.Tables[0].Rows[ldr]["TakenAmt"]));
                                                            //myprov_pdfpage.Add(FC19);
                                                            //PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            //                                  new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsLedge.Tables[0].Rows[ldr]["TakenAmt"]));
                                                            //myprov_pdfpage.Add(UC19);
                                                            //PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            //                                    new PdfArea(mychallan, 940, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsLedge.Tables[0].Rows[ldr]["TakenAmt"]));
                                                            //myprov_pdfpage.Add(TC19);

                                                            tempHtml.Append("<br><span class='classRegular' style='font-size:11px; width:320px;PADDING-LEFT:10PX;'>" + ledsno + "." + Convert.ToString(dsLedge.Tables[0].Rows[ldr]["DispName"]) + "</span>");
                                                            tempHtmlAmt.Append("<br><span class='classRegular' style='font-size:11px; '>" + returnIntegerPart(Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[ldr]["TakenAmt"]))) + "." + returnDecimalPart(Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[ldr]["TakenAmt"]))) + "</span>");

                                                            heght -= 12;
                                                        }
                                                    }
                                                }


                                                //Ledgerwise Details End
                                                PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, y + 190, 300, 10), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                                myprov_pdfpage.Add(FC171);

                                                PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 350, y + 190, 300, 10), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                                myprov_pdfpage.Add(UC171);



                                                PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 690, y + 190, 300, 10), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                                myprov_pdfpage.Add(TC171);
                                                y = y + 15;

                                                string amtDisp = showLedgerFees() ? tempHtmlAmt.ToString() : tkn;

                                                sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:320px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;'>" + amtDisp + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'   BORDER=1><tr><td style='width:320px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;'>" + amtDisp + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:320px;font-size:12px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;'>" + amtDisp + "</td></tr></table></td></tr>");
                                                heght -= 13;
                                            }


                                        }
                                    }
                                }

                                #region Denomionation and Particulars

                                sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:320px;'>Total</td><td style='text-align:right;'>" + returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)) + "</td></tr><tr><td colspan='2'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(total)) + " Rupees Only)" + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'   BORDER=1><tr><td style='width:320px;'>Total</td><td style='text-align:right;'>" + returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)) + "</td></tr><tr><td colspan='2'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(total)) + " Rupees Only)" + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:320px;'>Total</td><td style='text-align:right;'>" + returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)) + "</td></tr><tr><td colspan='2'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(total)) + " Rupees Only)" + "</td></tr></table></td></tr>");

                                sbHtml.Append("<tr><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 120) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0'  RULES='ROWS'><tr><td><br/>College Cashier<br/></td><td style='text-align:right;'><br>Signature of Remitter</td></tr><tr><td><br/>Bank Clerk</td><td style='text-align:right;'><br/>Bank Manager</td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 120) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td><br/>College Cashier<br/></td><td style='text-align:right;'><br>Signature of Remitter</td></tr><tr><td><br/>Bank Clerk</td><td style='text-align:right;'><br/>Bank Manager</td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 120) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td><br/>College Cashier<br/></td><td style='text-align:right;'><br>Signature of Remitter</td></tr><tr><td><br/>Bank Clerk</td><td style='text-align:right;'><br/>Bank Manager</td></tr></table></td></tr>");

                                if (useDenom == 1)
                                {
                                    //College
                                    sbHtml.Append("<tr><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td></tr>");

                                    sbHtml.Append("<tr><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td><td></td><td></td></tr>");
                                }
                                if (useDenom == 2)
                                {
                                    //Bank
                                    sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td><td></td><td></td></tr>");

                                    sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td><td></td><td></td><td></td><td></td></tr>");
                                }
                                if (useDenom == 3)
                                {
                                    //Student
                                    sbHtml.Append("<tr><td></td><td></td><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                    sbHtml.Append("<tr><td></td><td></td><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td></tr>");

                                }
                                if (useDenom == 4)
                                {
                                    //All

                                    sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                    sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td></tr>");
                                }
                                if (useDenom == 5)
                                {
                                    //College and Bank
                                    sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td></tr>");

                                    sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td><td></td><td></td></tr>");

                                }
                                if (useDenom == 6)
                                {
                                    //Student and Bank     
                                    sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                    sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td><td></td><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td></tr>");

                                }
                                if (useDenom == 7)
                                {
                                    //College and Student

                                    sbHtml.Append("<tr><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                    sbHtml.Append("<tr><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' border='1' ><tr><td>1000x</td><td>500x</td><td>100x</td><td>50x</td></tr><tr><td>20x</td><td>10x</td><td>5x</td><td>Coins</td></tr><tr><td colspan='4'>Total</td></tr></table></td></tr>");
                                }

                                #endregion

                                #endregion

                                #region Bottom Portion of Challan

                                PdfTextArea FC04 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                              new PdfArea(mychallan, 70, 25, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "New College Fee Collection Counter");
                                myprov_pdfpage.Add(FC04);
                                PdfTextArea UC04 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 400, 25, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "New College Fee Collection Counter");
                                myprov_pdfpage.Add(UC04);
                                PdfTextArea TC04 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 740, 25, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "New College Fee Collection Counter");
                                myprov_pdfpage.Add(TC04);

                                Gios.Pdf.PdfTable tableHr1 = mychallan.NewTable(Fontsmall, 2, 3, 1);
                                tableHr1.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);
                                tableHr1.VisibleHeaders = false;
                                tableHr1.Columns[0].SetWidth(100);
                                tableHr1.Columns[1].SetWidth(120);
                                tableHr1.Columns[2].SetWidth(80);

                                tableHr1.Cell(0, 1).SetContent("Receipt No:\n(Office Use Only) ");
                                tableHr1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr1.Cell(0, 1).SetFont(Fontsmall);

                                tableHr1.Cell(0, 0).SetContent("ChallanNo.:" + recptNo);
                                tableHr1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr1.Cell(0, 0).SetFont(Fontbold);

                                tableHr1.Cell(0, 2).SetContent("Date:" + recptDt);
                                tableHr1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                tableHr1.Cell(0, 2).SetFont(Fontsmall);

                                Gios.Pdf.PdfTablePage myprov_pdfpagetableHR1 = tableHr1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 25, 68, 300, 20));

                                myprov_pdfpage.Add(myprov_pdfpagetableHR1);

                                Gios.Pdf.PdfTable tableHr2 = mychallan.NewTable(Fontsmall, 2, 3, 1);
                                tableHr2.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);
                                tableHr2.VisibleHeaders = false;
                                tableHr2.Columns[0].SetWidth(100);
                                tableHr2.Columns[1].SetWidth(120);
                                tableHr2.Columns[2].SetWidth(80);

                                tableHr2.Cell(0, 1).SetContent("Receipt No:\n(Office Use Only) ");
                                tableHr2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr2.Cell(0, 1).SetFont(Fontsmall);

                                tableHr2.Cell(0, 0).SetContent("ChallanNo.:" + recptNo);
                                tableHr2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr2.Cell(0, 0).SetFont(Fontbold);

                                tableHr2.Cell(0, 2).SetContent("Date:" + recptDt);
                                tableHr2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                tableHr2.Cell(0, 2).SetFont(Fontsmall);

                                Gios.Pdf.PdfTablePage myprov_pdfpagetableHR2 = tableHr2.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 355, 68, 300, 20));

                                myprov_pdfpage.Add(myprov_pdfpagetableHR2);

                                Gios.Pdf.PdfTable tableHr3 = mychallan.NewTable(Fontsmall, 2, 3, 1);
                                tableHr3.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);
                                tableHr3.VisibleHeaders = false;
                                tableHr3.Columns[0].SetWidth(100);
                                tableHr3.Columns[1].SetWidth(120);
                                tableHr3.Columns[2].SetWidth(80);

                                tableHr3.Cell(0, 1).SetContent("Receipt No:\n(Office Use Only) ");
                                tableHr3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr3.Cell(0, 1).SetFont(Fontsmall);

                                tableHr3.Cell(0, 0).SetContent("ChallanNo.:" + recptNo);
                                tableHr3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr3.Cell(0, 0).SetFont(Fontbold);

                                tableHr3.Cell(0, 2).SetContent("Date:" + recptDt);
                                tableHr3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                tableHr3.Cell(0, 2).SetFont(Fontsmall);

                                Gios.Pdf.PdfTablePage myprov_pdfpagetableHR3 = tableHr3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 695, 68, 300, 20));

                                myprov_pdfpage.Add(myprov_pdfpagetableHR3);



                                PdfTextArea FC001 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                  new PdfArea(mychallan, 25, 55, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "COLLEGE FEES CHALLAN (" + shift + ")");
                                myprov_pdfpage.Add(FC001);
                                PdfTextArea UC001 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                              new PdfArea(mychallan, 350, 55, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "COLLEGE FEES CHALLAN (" + shift + ")");
                                myprov_pdfpage.Add(UC001);
                                PdfTextArea TC001 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                             new PdfArea(mychallan, 700, 55, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "COLLEGE FEES CHALLAN (" + shift + ")");
                                myprov_pdfpage.Add(TC001);
                                PdfTextArea FC0001 = new PdfTextArea(FontboldBig1, System.Drawing.Color.Black,
                                                                                  new PdfArea(mychallan, 25, 43, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "THE NEW COLLEGE (AUTONOMOUS CH-14)");
                                myprov_pdfpage.Add(FC0001);
                                PdfTextArea UC0001 = new PdfTextArea(FontboldBig1, System.Drawing.Color.Black,
                                                                              new PdfArea(mychallan, 350, 43, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "THE NEW COLLEGE (AUTONOMOUS CH-14)");
                                myprov_pdfpage.Add(UC0001);
                                PdfTextArea TC0001 = new PdfTextArea(FontboldBig1, System.Drawing.Color.Black,
                                                                             new PdfArea(mychallan, 700, 43, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "THE NEW COLLEGE (AUTONOMOUS CH-14)");
                                myprov_pdfpage.Add(TC0001);



                                text = "(" + DecimalToWords((decimal)Convert.ToDouble(total)) + " Rupees Only)";

                                PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Total");
                                PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Cashier");
                                PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 250, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Manager / Acct.");

                                PdfArea tete = new PdfArea(mychallan, 20, 5, 310, y + 255);
                                PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                myprov_pdfpage.Add(pr1);

                                PdfArea tete2 = new PdfArea(mychallan, 350, 5, 310, y + 255);
                                PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                myprov_pdfpage.Add(pr2);

                                PdfArea tete3 = new PdfArea(mychallan, 690, 5, 310, y + 255);
                                PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                myprov_pdfpage.Add(pr3);

                                PdfTextArea FC0015 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 25, y + 257, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS OF DEMAND DRAFT AND DENOMINATION"));
                                myprov_pdfpage.Add(FC0015);
                                PdfTextArea UC0015 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 355, y + 257, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS OF DEMAND DRAFT AND DENOMINATION"));
                                myprov_pdfpage.Add(UC0015);
                                PdfTextArea TC0015 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 695, y + 257, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS OF DEMAND DRAFT AND DENOMINATION"));
                                myprov_pdfpage.Add(TC0015);

                                Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 5, 5);
                                table.VisibleHeaders = false;
                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table.Columns[0].SetWidth(60);
                                table.Columns[1].SetWidth(60);
                                table.Columns[2].SetWidth(60);
                                table.Columns[3].SetWidth(60);
                                table.Columns[4].SetWidth(60);

                                table.Cell(0, 0).SetContent("Name of Bank");
                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 0).SetFont(Fontsmall);
                                table.Cell(0, 1).SetContent("Place of Bank");
                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 1).SetFont(Fontsmall);
                                table.Cell(0, 2).SetContent("Draft Number");
                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 2).SetFont(Fontsmall);
                                table.Cell(0, 3).SetContent("Date");
                                table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 3).SetFont(Fontsmall);
                                table.Cell(0, 4).SetContent("Amount");
                                table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 4).SetFont(Fontsmall);

                                table.Cell(1, 0).SetContent("\n");
                                table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 0).SetFont(Fontsmall);
                                table.Cell(1, 1).SetContent("\n");
                                table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 1).SetFont(Fontsmall);
                                table.Cell(1, 2).SetContent("\n");
                                table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 2).SetFont(Fontsmall);
                                table.Cell(1, 3).SetContent("\n");
                                table.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 3).SetFont(Fontsmall);
                                table.Cell(1, 4).SetContent("\n");
                                table.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 4).SetFont(Fontsmall);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 270, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable);

                                Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 5, 4, 3);
                                table1.VisibleHeaders = false;
                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                //table1.Columns[0].SetWidth(100);
                                //table1.Columns[1].SetWidth(60);
                                table1.Cell(0, 0).SetContent("1000  x");
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(0, 0).SetFont(Fontbold1);
                                table1.Cell(1, 0).SetContent("500   x");
                                table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(1, 0).SetFont(Fontbold1);
                                table1.Cell(0, 2).SetContent("20    x");
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(0, 2).SetFont(Fontbold1);
                                table1.Cell(1, 2).SetContent("10    x");
                                table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(1, 2).SetFont(Fontbold1);

                                table1.Cell(2, 0).SetContent("100   x");
                                table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(2, 0).SetFont(Fontbold1);
                                table1.Cell(3, 0).SetContent("50    x");
                                table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(3, 0).SetFont(Fontbold1);
                                table1.Cell(2, 2).SetContent("5     x");
                                table1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(2, 2).SetFont(Fontbold1);
                                table1.Cell(3, 2).SetContent("Coins x");
                                table1.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(3, 2).SetFont(Fontbold1);
                                table1.Cell(4, 0).SetContent("Total");
                                table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(4, 0).SetFont(Fontbold1);
                                table1.Cell(4, 1).ColSpan = 3;

                                Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 310, 310, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable1);


                                myprov_pdfpage.Add(IOB);
                                //myprov_pdfpage.Add(FC4);

                                myprov_pdfpage.Add(FC6);
                                myprov_pdfpage.Add(FC9);

                                myprov_pdfpage.Add(FC11);
                                myprov_pdfpage.Add(FC12);
                                myprov_pdfpage.Add(FC13);
                                myprov_pdfpage.Add(FC14);
                                myprov_pdfpage.Add(FC15);
                                myprov_pdfpage.Add(FC16);

                                myprov_pdfpage.Add(FC24);
                                myprov_pdfpage.Add(FC25);
                                myprov_pdfpage.Add(FC26);
                                myprov_pdfpage.Add(FC27);
                                myprov_pdfpage.Add(FC28);
                                myprov_pdfpage.Add(FC29);
                                myprov_pdfpage.Add(FC30);

                                myprov_pdfpage.Add(FC32);


                                //First End
                                myprov_pdfpage.Add(UC17);

                                PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Total");
                                PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Cashier");
                                PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 580, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Manager / Acct.");

                                Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 5, 5);
                                table3.VisibleHeaders = false;
                                table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table3.Columns[0].SetWidth(60);
                                table3.Columns[1].SetWidth(60);
                                table3.Columns[2].SetWidth(60);
                                table3.Columns[3].SetWidth(60);
                                table3.Columns[4].SetWidth(60);

                                table3.Cell(0, 0).SetContent("Name of Bank");
                                table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 0).SetFont(Fontsmall);
                                table3.Cell(0, 1).SetContent("Place of Bank");
                                table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 1).SetFont(Fontsmall);
                                table3.Cell(0, 2).SetContent("Draft Number");
                                table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 2).SetFont(Fontsmall);
                                table3.Cell(0, 3).SetContent("Date");
                                table3.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 3).SetFont(Fontsmall);
                                table3.Cell(0, 4).SetContent("Amount");
                                table3.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 4).SetFont(Fontsmall);

                                table3.Cell(1, 0).SetContent("\n");
                                table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 0).SetFont(Fontsmall);
                                table3.Cell(1, 1).SetContent("\n");
                                table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 1).SetFont(Fontsmall);
                                table3.Cell(1, 2).SetContent("\n");
                                table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 2).SetFont(Fontsmall);
                                table3.Cell(1, 3).SetContent("\n");
                                table3.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 3).SetFont(Fontsmall);
                                table3.Cell(1, 4).SetContent("\n");
                                table3.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 4).SetFont(Fontsmall);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 270, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable3);

                                Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 5, 4, 3);
                                table14.VisibleHeaders = false;
                                table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                //table14.Columns[0].SetWidth(100);
                                //table14.Columns[1].SetWidth(60);
                                table14.Cell(0, 0).SetContent("1000  x");
                                table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(0, 0).SetFont(Fontbold1);
                                table14.Cell(1, 0).SetContent("500   x");
                                table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(1, 0).SetFont(Fontbold1);
                                table14.Cell(0, 2).SetContent("20    x");
                                table14.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(0, 2).SetFont(Fontbold1);
                                table14.Cell(1, 2).SetContent("10    x");
                                table14.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(1, 2).SetFont(Fontbold1);

                                table14.Cell(2, 0).SetContent("100   x");
                                table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(2, 0).SetFont(Fontbold1);
                                table14.Cell(3, 0).SetContent("50    x");
                                table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(3, 0).SetFont(Fontbold1);
                                table14.Cell(2, 2).SetContent("5     x");
                                table14.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(2, 2).SetFont(Fontbold1);
                                table14.Cell(3, 2).SetContent("Coins x");
                                table14.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(3, 2).SetFont(Fontbold1);
                                table14.Cell(4, 0).SetContent("Total");
                                table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(4, 0).SetFont(Fontbold1);
                                table14.Cell(4, 1).ColSpan = 3;

                                Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 310, 310, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable4);


                                myprov_pdfpage.Add(UC2);
                                myprov_pdfpage.Add(UC6);

                                myprov_pdfpage.Add(UC9);

                                myprov_pdfpage.Add(UC11);
                                myprov_pdfpage.Add(UC12);
                                myprov_pdfpage.Add(UC13);
                                myprov_pdfpage.Add(UC14);
                                myprov_pdfpage.Add(UC15);
                                myprov_pdfpage.Add(UC16);


                                myprov_pdfpage.Add(UC24);
                                myprov_pdfpage.Add(UC25);
                                myprov_pdfpage.Add(UC26);
                                myprov_pdfpage.Add(UC27);
                                myprov_pdfpage.Add(UC28);
                                myprov_pdfpage.Add(UC29);
                                myprov_pdfpage.Add(UC30);
                                myprov_pdfpage.Add(UC32);
                                //second End


                                myprov_pdfpage.Add(TC17);

                                PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 695, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Total");
                                PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 940, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 690, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 695, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 695, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 695, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Cashier");
                                PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 920, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Manager / Acct.");


                                Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 5, 5);
                                table5.VisibleHeaders = false;
                                table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table5.Columns[0].SetWidth(60);
                                table5.Columns[1].SetWidth(60);
                                table5.Columns[2].SetWidth(60);
                                table5.Columns[3].SetWidth(60);
                                table5.Columns[4].SetWidth(60);

                                table5.Cell(0, 0).SetContent("Name of Bank");
                                table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 0).SetFont(Fontsmall);
                                table5.Cell(0, 1).SetContent("Place of Bank");
                                table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 1).SetFont(Fontsmall);
                                table5.Cell(0, 2).SetContent("Draft Number");
                                table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 2).SetFont(Fontsmall);
                                table5.Cell(0, 3).SetContent("Date");
                                table5.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 3).SetFont(Fontsmall);
                                table5.Cell(0, 4).SetContent("Amount");
                                table5.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 4).SetFont(Fontsmall);

                                table5.Cell(1, 0).SetContent("\n");
                                table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 0).SetFont(Fontsmall);
                                table5.Cell(1, 1).SetContent("\n");
                                table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 1).SetFont(Fontsmall);
                                table5.Cell(1, 2).SetContent("\n");
                                table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 2).SetFont(Fontsmall);
                                table5.Cell(1, 3).SetContent("\n");
                                table5.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 3).SetFont(Fontsmall);
                                table5.Cell(1, 4).SetContent("\n");
                                table5.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 4).SetFont(Fontsmall);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 690, y + 270, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable31);

                                Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 5, 4, 3);
                                table15.VisibleHeaders = false;
                                table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                //table15.Columns[0].SetWidth(100);
                                //table15.Columns[1].SetWidth(60);
                                table15.Cell(0, 0).SetContent("1000  x");
                                table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(0, 0).SetFont(Fontbold1);
                                table15.Cell(1, 0).SetContent("500   x");
                                table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(1, 0).SetFont(Fontbold1);
                                table15.Cell(0, 2).SetContent("20    x");
                                table15.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(0, 2).SetFont(Fontbold1);
                                table15.Cell(1, 2).SetContent("10    x");
                                table15.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(1, 2).SetFont(Fontbold1);

                                table15.Cell(2, 0).SetContent("100   x");
                                table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(2, 0).SetFont(Fontbold1);
                                table15.Cell(3, 0).SetContent("50    x");
                                table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(3, 0).SetFont(Fontbold1);
                                table15.Cell(2, 2).SetContent("5     x");
                                table15.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(2, 2).SetFont(Fontbold1);
                                table15.Cell(3, 2).SetContent("Coins x");
                                table15.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(3, 2).SetFont(Fontbold1);
                                table15.Cell(4, 0).SetContent("Total");
                                table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(4, 0).SetFont(Fontbold1);
                                table15.Cell(4, 1).ColSpan = 3;

                                Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 690, y + 310, 310, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable5);

                                myprov_pdfpage.Add(TC2);
                                myprov_pdfpage.Add(TC6);

                                myprov_pdfpage.Add(TC9);

                                myprov_pdfpage.Add(TC11);
                                myprov_pdfpage.Add(TC12);
                                myprov_pdfpage.Add(TC13);
                                myprov_pdfpage.Add(TC14);
                                myprov_pdfpage.Add(TC15);
                                myprov_pdfpage.Add(TC16);
                                myprov_pdfpage.Add(TC17);
                                myprov_pdfpage.Add(TC24);
                                myprov_pdfpage.Add(TC25);
                                myprov_pdfpage.Add(TC26);
                                myprov_pdfpage.Add(TC27);
                                myprov_pdfpage.Add(TC28);
                                myprov_pdfpage.Add(TC29);
                                myprov_pdfpage.Add(TC30);
                                myprov_pdfpage.Add(TC32);

                                myprov_pdfpage.SaveToDocument();
                                #endregion

                                sbHtml.Append("<tr>");


                                sbHtml.Append("</tr>");
                                sbHtml.Append("</table></div>");
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
                        contentDiv.InnerHtml += sbHtml.ToString();
                    }
                }
                #endregion

                #region To print the challan
                if (createPDFOK)
                {
                    #region New Print
                    //contentDiv.InnerHtml += sbHtml.ToString();
                    contentDiv.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
                    #endregion

                    //string appPath = HttpContext.Current.Server.MapPath("~");
                    //if (appPath != "")
                    //{
                    //    string szPath = appPath + "/Report/";

                    //    string szFile = "Challan" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                    //    mychallan.SaveToFile(szPath + szFile);
                    //    Response.ClearHeaders();
                    //    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    //    Response.ContentType = "application/pdf";
                    //    //mychallan.SaveToStream(Response.OutputStream);
                    //    Response.WriteFile(szPath + szFile);
                    //    //Response.AddHeader("Refresh", "1; url=receiptPrint.aspx");
                    //    //Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");

                    //}
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Challan Cannot Be Generated";
                }
                #endregion
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Challan";
        }
    }
    public void Duplicate2()
    {
        if (checkedOK())
        {
            FpSpread1.SaveChanges();
            try
            {
                bool createPDFOK = false;

                Font Fontbold = new Font("Arial", 8, FontStyle.Bold);
                Font Fontsmall = new Font("Arial", 8, FontStyle.Bold);
                Font Fontsmall1 = new Font("Arial", 10, FontStyle.Bold);
                Font Fontbold1 = new Font("Arial", 10, FontStyle.Bold);

                Gios.Pdf.PdfDocument mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
                // mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(14.0, 8.5));
                mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(13.8, 8.5));

                #region For Every selected Challan
                for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                    if (check == 1)
                    {
                        #region base data
                        int challanType = 1;
                        string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                        string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);
                        string app_formno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                        string studname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                        string deg = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);
                        string total = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Tag);
                        string accNo = string.Empty;

                        //string trasdate = txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2];
                        // string transtime = DateTime.Now.ToLongTimeString();
                        string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);//d2.GetFunction("select app_no from Registration where Roll_No ='" + app_formno + "'");
                        //string transcode = generateReceiptNo();

                        string regno = string.Empty;
                        string rollno = string.Empty;
                        string appnoNew = string.Empty;
                        string roll_admit = string.Empty;
                        string smartno = string.Empty;
                        string queryRollApp = "select r.smart_serial_no,r.Roll_No,a.app_formno,a.app_no,r.Reg_No,r.Roll_Admit  from Registration r,applyn a where r.App_No=a.app_no and r.App_No='" + AppNo + "'";
                        DataSet dsRollApp = new DataSet();
                        dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                        if (dsRollApp.Tables.Count > 0)
                        {
                            if (dsRollApp.Tables[0].Rows.Count > 0)
                            {
                                rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                roll_admit = Convert.ToString(dsRollApp.Tables[0].Rows[0]["roll_admit"]);
                                smartno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["smart_serial_no"]);
                            }
                        }
                        string rolldisplay = "Reg No :";
                        string rollvalue = regno;
                        if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                        {
                            rolldisplay = "Roll No :";
                            rollvalue = rollno;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                        {
                            rolldisplay = "Reg No :";
                            rollvalue = regno;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                        {
                            rolldisplay = "Admission No :";
                            rollvalue = roll_admit;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                        {
                            rolldisplay = "Smartcard No :";
                            rollvalue = smartno;
                        }
                        else
                        {
                            rolldisplay = "App No :";
                            appnoNew = AppNo;
                            app_formno = d2.GetFunction("select app_formno from applyn where app_no='" + AppNo + "'").Trim();
                            rollvalue = app_formno;
                        }

                        if (ddl_befAftAdmis.SelectedIndex == 0)
                        {
                            rolldisplay = "App No :";
                            appnoNew = AppNo;
                            app_formno = d2.GetFunction("select app_formno from applyn where app_no='" + AppNo + "'").Trim();

                            rollvalue = app_formno;
                        }
                        //string colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + AppNo + "' and d.college_code=" + collegecode1 + "";
                        string colquery = "";
                        if (rolldisplay != "App No :")
                        {
                            colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                        }
                        else
                        {
                            colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                        }

                        string collegename = string.Empty;
                        string add1 = string.Empty;
                        string add2 = string.Empty;
                        string univ = string.Empty;
                        string degreeCode = string.Empty;
                        string stream = string.Empty;
                        string cursem = string.Empty;
                        string batyr = string.Empty;

                        string bankName = string.Empty;
                        string bankCity = string.Empty;

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(colquery, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                degreeCode = Convert.ToString(ds.Tables[1].Rows[0]["Degree_code"]);
                                cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                            }
                        }

                        #endregion


                        string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,TakenAmt,BankFk,FinYearFk,challanType from FT_ChallanDet where challanNo='" + recptNo + "' and App_No ='" + AppNo + "'";
                        DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                        if (dsDet.Tables.Count > 0)
                        {
                            if (dsDet.Tables[0].Rows.Count > 0)
                            {
                                challanType = Convert.ToInt32(Convert.ToString(dsDet.Tables[0].Rows[0]["challanType"]));

                                string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[0]["FeeCategory"]);
                                cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + FeeCategory + " and college_code=" + collegecode1 + "");
                                cursem = cursem.Split(' ')[1] + " : " + romanLetter(cursem.Split(' ')[0]);

                                string bnkFk = Convert.ToString(dsDet.Tables[0].Rows[0]["BankFk"]);

                                //string bnkDetQ = "select BankName,City,BankCode,AccNo from FM_FinBankMaster where BankPK=" + bnkFk + "";
                                //DataSet dsBnkDet = d2.select_method_wo_parameter(bnkDetQ, "Text");
                                //if (dsBnkDet.Tables.Count > 0)
                                //{
                                //    if (dsBnkDet.Tables[0].Rows.Count > 0)
                                //    {
                                //        bankName = Convert.ToString(dsBnkDet.Tables[0].Rows[0]["BankName"]);
                                //        //bankCity = "(" + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["City"]) + ")";
                                //        accNo = "A/c No " + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["AccNo"]);
                                //        string ifsc = d2.GetFunction("SELECT DISTINCT IFSCCode FROM FM_FinBankMaster  where BankPk=" + bnkFk + " and CollegeCode=" + collegecode1 + "");
                                //        bankName = "Bank : " + bankName;
                                //        bankCity = String.Format("A/c No. " + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["AccNo"]) + "\n IFSC Code : "+ifsc);
                                //    }
                                //}

                                createPDFOK = true;

                                #region Challan Top portion

                                int y = 0;
                                Gios.Pdf.PdfPage myprov_pdfpage = mychallan.NewPage();

                                PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                PdfTextArea ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Bank Copy");
                                PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 70, 75, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);

                                PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 30, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, 60, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);

                                PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                myprov_pdfpage.Add(FC17);
                                string text = "";

                                //First Ends

                                PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                PdfTextArea UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Office Copy");
                                PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 400, 75, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                //PdfTextArea UC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                //                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 360, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 60, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);

                                PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                //PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                //                                                 new PdfArea(mychallan, 550, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 350, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                //second End
                                y = 0;

                                PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                                           new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                PdfTextArea TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student Copy");
                                PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 720, 75, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                //PdfTextArea TC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                //                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 680, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 670, 60, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                //PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                //                                                 new PdfArea(mychallan, 900, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                y = 0;

                                //PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                //                                                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                //PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                //                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                //PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                //                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);

                                //myprov_pdfpage.Add(FC4);
                                //myprov_pdfpage.Add(UC4);
                                //myprov_pdfpage.Add(TC4);
                                //PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                //                                          new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No.:" + recptNo);
                                //PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                //                                                   new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No.:" + recptNo);
                                //PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                //                                                   new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No.:" + recptNo);

                                PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                          new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, "Due Date :            ");
                                PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, "Due Date :            ");
                                PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, "Due Date :            ");
                                myprov_pdfpage.Add(FC10);
                                myprov_pdfpage.Add(UC10);
                                myprov_pdfpage.Add(TC10);

                                #endregion

                                #region Challan Middle Portion

                                //for (int j = 0; j < dsDet.Tables[0].Rows.Count; j++)
                                //{
                                //    string ledger = Convert.ToString(dsDet.Tables[0].Rows[j]["LedgerFK"]);
                                //    string header = Convert.ToString(dsDet.Tables[0].Rows[j]["HeaderFk"]);
                                //    string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[j]["FeeCategory"]);
                                //    string taknAmt = Convert.ToString(dsDet.Tables[0].Rows[j]["TakenAmt"]);
                                //}

                                string selHeadersQ = string.Empty;
                                DataSet dsHeaders = new DataSet();

                                if (challanType == 1 || challanType == 2)
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

                                    selHeadersQ = " select sum(TakenAmt) as TakenAmt,f.ChlGroupHeader  as DispName from FT_ChallanDet d ,FS_ChlGroupHeaderSettings f where d.HeaderFK =f.HeaderFK and   challanNo='" + recptNo + "' and App_No ='" + AppNo + "'  ";
                                    if (StudStream != "")
                                    {
                                        selHeadersQ += " and f.stream='" + StudStream + "' ";
                                    }
                                    selHeadersQ += "   group by ChlGroupHeader ";
                                }
                                else if (challanType == 3)
                                {
                                    selHeadersQ = " select HeaderFk,SUM(TakenAmt) as TakenAmt,h.HeaderName  as DispName  from FT_ChallanDet d,FM_HeaderMaster h  where d.HeaderFK =h.HeaderPK and challanNo='" + recptNo + "' and d.App_No ='" + AppNo + "' group by HeaderFk,h.HeaderName ";
                                }
                                else if (challanType == 4)
                                {
                                    selHeadersQ = " select LedgerFK,sum(TakenAmt) as TakenAmt,l.LedgerName as DispName  from FT_ChallanDet d,FM_LedgerMaster l where d.LedgerFK =l.LedgerPK and challanNo='" + recptNo + "' and d.App_No ='" + AppNo + "' group by LedgerFK,l.LedgerName ";
                                }

                                if (selHeadersQ != string.Empty)
                                {
                                    dsHeaders.Clear();
                                    dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                    if (dsHeaders.Tables.Count > 0)
                                    {
                                        if (dsHeaders.Tables[0].Rows.Count > 0)
                                        {
                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                            {
                                                string dispHdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                if (challanType > 2)
                                                {
                                                    bnkFk = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                }
                                                //else
                                                //{
                                                //    bnkFk = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]) + "' AND BankPK = '" + bankPK + "'");
                                                //}

                                                string bnkDetQ = "select BankName,Upper(BankBranch) as city,BankCode,AccNo from FM_FinBankMaster where BankPK=" + bnkFk + "";
                                                DataSet dsBnkDet = d2.select_method_wo_parameter(bnkDetQ, "Text");
                                                if (dsBnkDet.Tables.Count > 0)
                                                {
                                                    if (dsBnkDet.Tables[0].Rows.Count > 0)
                                                    {
                                                        bankName = Convert.ToString(dsBnkDet.Tables[0].Rows[0]["BankName"]);
                                                        //bankCity = "(" + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["City"]) + ")";
                                                        accNo = "A/c No " + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["AccNo"]);
                                                        string ifsc = d2.GetFunction("SELECT DISTINCT IFSCCode FROM FM_FinBankMaster  where BankPk=" + bnkFk + " and CollegeCode=" + collegecode1 + "");
                                                        bankName = "Bank : " + bankName;
                                                        bankCity = String.Format("A/c No. " + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["AccNo"]) + "\n IFSC Code : " + ifsc);
                                                    }
                                                }


                                                dispHdr += " (" + accNo + ")";
                                                string totalAmt = Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                                PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, totalAmt);
                                                PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(FC18);
                                                myprov_pdfpage.Add(FC171);
                                                myprov_pdfpage.Add(FC19);


                                                PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                           new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, totalAmt);
                                                PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(UC18);
                                                myprov_pdfpage.Add(UC19);
                                                myprov_pdfpage.Add(UC171);

                                                PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, totalAmt);
                                                PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");



                                                myprov_pdfpage.Add(TC18);
                                                myprov_pdfpage.Add(TC19);
                                                myprov_pdfpage.Add(TC171);
                                                y = y + 15;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region Bottom Portion of Challan

                                text = "(" + DecimalToWords((decimal)Convert.ToDouble(total)) + " Rupees Only)";
                                PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                                                    new PdfArea(mychallan, 70, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                      new PdfArea(mychallan, 400, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                      new PdfArea(mychallan, 720, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);

                                PdfArea rectBnk = new PdfArea(mychallan, 68, 78, 210, 33);
                                PdfRectangle prRect = new PdfRectangle(mychallan, rectBnk, Color.Black);
                                myprov_pdfpage.Add(prRect);

                                PdfArea rectBnk2 = new PdfArea(mychallan, 398, 78, 210, 33);
                                PdfRectangle prRect2 = new PdfRectangle(mychallan, rectBnk2, Color.Black);
                                myprov_pdfpage.Add(prRect2);

                                PdfArea rectBnk3 = new PdfArea(mychallan, 718, 78, 210, 33);
                                PdfRectangle prRect3 = new PdfRectangle(mychallan, rectBnk3, Color.Black);
                                myprov_pdfpage.Add(prRect3);

                                myprov_pdfpage.Add(FC4);
                                myprov_pdfpage.Add(UC4);
                                myprov_pdfpage.Add(TC4);
                                PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                             new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);


                                PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, y + 235, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");

                                PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 260);
                                PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                myprov_pdfpage.Add(pr1);

                                PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 260);
                                PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                myprov_pdfpage.Add(pr2);

                                PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 260);
                                PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                myprov_pdfpage.Add(pr3);

                                Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                table.VisibleHeaders = false;
                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table.Columns[0].SetWidth(100);
                                table.Columns[1].SetWidth(60);
                                table.Columns[2].SetWidth(60);

                                table.Cell(0, 0).SetContent("Cheque/DD No");
                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 0).SetFont(Fontbold1);
                                table.Cell(0, 1).SetContent("Date");
                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 1).SetFont(Fontbold1);
                                table.Cell(0, 2).SetContent("Amount");
                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 2).SetFont(Fontbold1);
                                table.Cell(1, 0).SetContent("\n");
                                table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 0).SetFont(Fontbold1);
                                table.Cell(1, 1).SetContent("\n");
                                table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 1).SetFont(Fontbold1);
                                table.Cell(1, 2).SetContent("\n");
                                table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 2).SetFont(Fontbold1);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 290, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable);

                                Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                table1.VisibleHeaders = false;
                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table1.Columns[0].SetWidth(100);
                                table1.Columns[1].SetWidth(60);
                                table1.Cell(0, 0).SetContent("1000x");
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(0, 0).SetFont(Fontbold1);
                                table1.Cell(1, 0).SetContent("500x");
                                table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(1, 0).SetFont(Fontbold1);
                                table1.Cell(2, 0).SetContent("100x");
                                table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(2, 0).SetFont(Fontbold1);
                                table1.Cell(3, 0).SetContent("50x");
                                table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(3, 0).SetFont(Fontbold1);
                                table1.Cell(4, 0).SetContent("20x");
                                table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(4, 0).SetFont(Fontbold1);
                                table1.Cell(5, 0).SetContent("10x");
                                table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(5, 0).SetFont(Fontbold1);
                                table1.Cell(6, 0).SetContent("5x");
                                table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(6, 0).SetFont(Fontbold1);
                                table1.Cell(7, 0).SetContent("Coinsx");
                                table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(7, 0).SetFont(Fontbold1);
                                table1.Cell(8, 0).SetContent("Total");
                                table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(8, 0).SetFont(Fontbold1);



                                Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable1);

                                myprov_pdfpage.Add(FC);
                                myprov_pdfpage.Add(ORGI);
                                myprov_pdfpage.Add(IOB);
                                //myprov_pdfpage.Add(FC4);
                                myprov_pdfpage.Add(FC5);
                                myprov_pdfpage.Add(FC6);
                                myprov_pdfpage.Add(FC7);
                                myprov_pdfpage.Add(FC8);
                                myprov_pdfpage.Add(FC9);
                                //myprov_pdfpage.Add(FC10);
                                myprov_pdfpage.Add(FC11);
                                myprov_pdfpage.Add(FC12);
                                myprov_pdfpage.Add(FC13);
                                myprov_pdfpage.Add(FC14);
                                myprov_pdfpage.Add(FC15);
                                myprov_pdfpage.Add(FC16);

                                myprov_pdfpage.Add(FC24);
                                myprov_pdfpage.Add(FC25);
                                myprov_pdfpage.Add(FC26);
                                myprov_pdfpage.Add(FC27);
                                myprov_pdfpage.Add(FC28);
                                myprov_pdfpage.Add(FC29);
                                myprov_pdfpage.Add(FC30);


                                myprov_pdfpage.Add(FC32);
                                //myprov_pdfpage.Add(FC33);

                                //First End
                                myprov_pdfpage.Add(UC17);

                                PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, y + 235, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");


                                Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                table3.VisibleHeaders = false;
                                table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table3.Columns[0].SetWidth(100);
                                table3.Columns[1].SetWidth(60);
                                table3.Columns[2].SetWidth(60);

                                table3.Cell(0, 0).SetContent("Cheque/DD No");
                                table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 0).SetFont(Fontbold1);
                                table3.Cell(0, 1).SetContent("Date");
                                table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 1).SetFont(Fontbold1);
                                table3.Cell(0, 2).SetContent("Amount");
                                table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 2).SetFont(Fontbold1);
                                table3.Cell(1, 0).SetContent("\n");
                                table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 0).SetFont(Fontbold1);
                                table3.Cell(1, 1).SetContent("\n");
                                table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 1).SetFont(Fontbold1);
                                table3.Cell(1, 2).SetContent("\n");
                                table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 2).SetFont(Fontbold1);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 290, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable3);

                                Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                table14.VisibleHeaders = false;
                                table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table14.Columns[0].SetWidth(100);
                                table14.Columns[1].SetWidth(60);
                                table14.Cell(0, 0).SetContent("1000x");
                                table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(0, 0).SetFont(Fontbold1);
                                table14.Cell(1, 0).SetContent("500x");
                                table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(1, 0).SetFont(Fontbold1);
                                table14.Cell(2, 0).SetContent("100x");
                                table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(2, 0).SetFont(Fontbold1);
                                table14.Cell(3, 0).SetContent("50x");
                                table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(3, 0).SetFont(Fontbold1);
                                table14.Cell(4, 0).SetContent("20x");
                                table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(4, 0).SetFont(Fontbold1);
                                table14.Cell(5, 0).SetContent("10x");
                                table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(5, 0).SetFont(Fontbold1);
                                table14.Cell(6, 0).SetContent("5x");
                                table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(6, 0).SetFont(Fontbold1);
                                table14.Cell(7, 0).SetContent("Coinsx");
                                table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(7, 0).SetFont(Fontbold1);
                                table14.Cell(8, 0).SetContent("Total");
                                table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(8, 0).SetFont(Fontbold1);

                                Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable4);

                                myprov_pdfpage.Add(UC);
                                myprov_pdfpage.Add(UC1);
                                myprov_pdfpage.Add(UC2);
                                //myprov_pdfpage.Add(UC4);
                                myprov_pdfpage.Add(UC5);
                                myprov_pdfpage.Add(UC6);
                                myprov_pdfpage.Add(UC7);
                                myprov_pdfpage.Add(UC8);
                                myprov_pdfpage.Add(UC9);
                                //myprov_pdfpage.Add(UC10);
                                myprov_pdfpage.Add(UC11);
                                myprov_pdfpage.Add(UC12);
                                myprov_pdfpage.Add(UC13);
                                myprov_pdfpage.Add(UC14);
                                myprov_pdfpage.Add(UC15);
                                myprov_pdfpage.Add(UC16);


                                myprov_pdfpage.Add(UC24);
                                myprov_pdfpage.Add(UC25);
                                myprov_pdfpage.Add(UC26);
                                myprov_pdfpage.Add(UC27);
                                myprov_pdfpage.Add(UC28);
                                myprov_pdfpage.Add(UC29);
                                myprov_pdfpage.Add(UC30);

                                myprov_pdfpage.Add(UC32);
                                //second End


                                myprov_pdfpage.Add(TC17);

                                PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 685, y + 235, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");

                                PdfTextArea FC0027 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 25, y + 330, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, String.Format(" For Bank use only\n " + text.ToString() + "\n\n\n Date :                                                    Signature of the Bank Official with seal"));
                                myprov_pdfpage.Add(FC0027);
                                PdfTextArea UC0027 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                              new PdfArea(mychallan, 355, y + 330, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, String.Format(" For Bank use only\n " + text.ToString() + "\n\n\n Date :                                                    Signature of the Bank Official with seal"));
                                myprov_pdfpage.Add(UC0027);
                                PdfTextArea TC0027 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                               new PdfArea(mychallan, 685, y + 330, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, String.Format(" For Bank use only\n " + text.ToString() + "\n\n\n Date :                                                    Signature of the Bank Official with seal"));
                                myprov_pdfpage.Add(TC0027);

                                Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                table5.VisibleHeaders = false;
                                table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table5.Columns[0].SetWidth(100);
                                table5.Columns[1].SetWidth(60);
                                table5.Columns[2].SetWidth(60);

                                table5.Cell(0, 0).SetContent("Cheque/DD No");
                                table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 0).SetFont(Fontbold1);
                                table5.Cell(0, 1).SetContent("Date");
                                table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 1).SetFont(Fontbold1);
                                table5.Cell(0, 2).SetContent("Amount");
                                table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 2).SetFont(Fontbold1);
                                table5.Cell(1, 0).SetContent("\n");
                                table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 0).SetFont(Fontbold1);
                                table5.Cell(1, 1).SetContent("\n");
                                table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 1).SetFont(Fontbold1);
                                table5.Cell(1, 2).SetContent("\n");
                                table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 2).SetFont(Fontbold1);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 290, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable31);

                                Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                table15.VisibleHeaders = false;
                                table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table15.Columns[0].SetWidth(100);
                                table15.Columns[1].SetWidth(60);
                                table15.Cell(0, 0).SetContent("1000x");
                                table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(0, 0).SetFont(Fontbold1);
                                table15.Cell(1, 0).SetContent("500x");
                                table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(1, 0).SetFont(Fontbold1);
                                table15.Cell(2, 0).SetContent("100x");
                                table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(2, 0).SetFont(Fontbold1);
                                table15.Cell(3, 0).SetContent("50x");
                                table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(3, 0).SetFont(Fontbold1);
                                table15.Cell(4, 0).SetContent("20x");
                                table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(4, 0).SetFont(Fontbold1);
                                table15.Cell(5, 0).SetContent("10x");
                                table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(5, 0).SetFont(Fontbold1);
                                table15.Cell(6, 0).SetContent("5x");
                                table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(6, 0).SetFont(Fontbold1);
                                table15.Cell(7, 0).SetContent("Coinsx");
                                table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(7, 0).SetFont(Fontbold1);
                                table15.Cell(8, 0).SetContent("Total");
                                table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(8, 0).SetFont(Fontbold1);

                                Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable5);

                                myprov_pdfpage.Add(TC);
                                myprov_pdfpage.Add(TC1);
                                myprov_pdfpage.Add(TC2);
                                //myprov_pdfpage.Add(TC4);
                                myprov_pdfpage.Add(TC5);
                                myprov_pdfpage.Add(TC6);
                                myprov_pdfpage.Add(TC7);
                                myprov_pdfpage.Add(TC8);
                                myprov_pdfpage.Add(TC9);
                                //myprov_pdfpage.Add(TC10);
                                myprov_pdfpage.Add(TC11);
                                myprov_pdfpage.Add(TC12);
                                myprov_pdfpage.Add(TC13);
                                myprov_pdfpage.Add(TC14);
                                myprov_pdfpage.Add(TC15);
                                myprov_pdfpage.Add(TC16);
                                myprov_pdfpage.Add(TC17);
                                myprov_pdfpage.Add(TC24);
                                myprov_pdfpage.Add(TC25);
                                myprov_pdfpage.Add(TC26);
                                myprov_pdfpage.Add(TC27);
                                myprov_pdfpage.Add(TC28);
                                myprov_pdfpage.Add(TC29);
                                myprov_pdfpage.Add(TC30);

                                myprov_pdfpage.Add(TC32);

                                myprov_pdfpage.SaveToDocument();
                                #endregion
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
                #endregion

                #region To print the challan
                if (createPDFOK)
                {
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";

                        string szFile = "Challan" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                        mychallan.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                        //Response.AddHeader("Refresh", "1; url=receiptPrint.aspx");
                        //Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");

                    }
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Challan Cannot Be Generated";
                }
                #endregion
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Challan";
        }
    }
    public void Duplicate3()
    {
        //New College
        if (checkedOK())
        {
            FpSpread1.SaveChanges();
            try
            {
                bool createPDFOK = false;

                Font Fontbold = new Font("Arial", 8, FontStyle.Bold);
                Font Fontsmall = new Font("Arial", 8, FontStyle.Regular);
                Font Fontsmall1 = new Font("Arial", 8, FontStyle.Regular);
                Font Fontbold1 = new Font("Arial", 8, FontStyle.Bold);
                Font Fontboldled = new Font("Arial", 7, FontStyle.Regular);
                Font FontboldBig = new Font("Arial", 12, FontStyle.Bold);
                Font FontboldBig1 = new Font("Arial", 10, FontStyle.Bold);

                Gios.Pdf.PdfDocument mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
                // mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(13.8, 8.5));
                mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(14.2, 8.5));//14.0 previous
                #region For Every selected Challan
                contentDiv.InnerHtml = "";
                StringBuilder sbHtml = new StringBuilder();
                for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    sbHtml.Clear();
                    byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                    if (check == 1)
                    {
                        string shift = "";
                        string acaYear = System.DateTime.Now.Year.ToString();
                        shift = d2.GetFunction("select UPPER(type) from course where college_code=" + collegecode1 + "").Trim();
                        if (shift == "0" || shift == "")
                        {
                            shift = "";
                        }
                        else
                        {
                            shift = "(" + shift + ")";
                        }
                        string counterName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName= 'ChallanFeeCounterValue' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ").Trim();
                        if (counterName == "0")
                            counterName = string.Empty;


                        //added by sudhagar 29.03.2017
                        string hstlName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IncludeHostelName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ");

                        string incShift = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IncludeShiftName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ");
                        string hstincName = string.Empty;
                        string hstlfees = string.Empty;
                        string colName = string.Empty;
                        colName = d2.GetFunction("select collname from collinfo where college_code=" + collegecode1 + "").Trim();
                        if (colName == "0" || colName == "")
                            colName = string.Empty;
                        if (colName != string.Empty)
                        {
                            string tempCName = colName.ToUpper().Replace(" ", "");
                            if (tempCName.Contains("NEWCOLLEGE"))
                            {
                                if (!string.IsNullOrEmpty(incShift) && incShift != "0")
                                    shift = "";
                                if (!string.IsNullOrEmpty(hstlName) && hstlName != "0")
                                {
                                    hstincName = hstlName;
                                    hstlfees = hstlName;
                                }
                                else
                                {
                                    hstincName = "AUTONOMOUS";
                                    hstlfees = "COLLEGE";
                                }
                                colName = "THE NEW COLLEGE (" + hstincName + ") CH-14";
                            }
                        }

                        string parName = string.Empty;
                        parName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanParticular' and user_code ='" + usercode + "' and college_code =" + collegecode1 + "").Trim();
                        if (parName == "0" || parName == "")
                            parName = "Particulars";
                        else
                            parName = "Particulars - " + parName;

                        string useIFSC = d2.GetFunction("select LinkValue from New_InsSettings where LinkName= 'DisplayIFSCForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ").Trim();

                        string useDegAcr = d2.GetFunction("select LinkValue from New_InsSettings where LinkName= 'DisplayAcrForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ").Trim();

                        int useDenom = Convert.ToInt32(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayDenominationChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'").Trim());

                        #region base data
                        int challanType = 1;
                        string recptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                        string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);
                        string app_formno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                        string studname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                        string deg = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);
                        if (useDegAcr == "1")
                        {
                            deg = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Tag);
                        }
                        string total = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Tag);
                        string accNo = string.Empty;

                        //string trasdate = txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2];
                        // string transtime = DateTime.Now.ToLongTimeString();
                        string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);

                        //AppNo = d2.GetFunction("select app_no from Registration where Roll_No ='" + app_formno + "'");
                        //string transcode = generateReceiptNo();

                        string regno = string.Empty;
                        string rollno = string.Empty;
                        string appnoNew = string.Empty;
                        string roll_admit = string.Empty;
                        string smartno = string.Empty;

                        string queryRollApp = "select r.smart_serial_no,r.Roll_No,a.app_formno,a.app_no,r.Reg_No,r.Roll_Admit  from Registration r,applyn a where r.App_No=a.app_no and r.App_No='" + AppNo + "'";
                        DataSet dsRollApp = new DataSet();
                        dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                        if (dsRollApp.Tables.Count > 0)
                        {
                            if (dsRollApp.Tables[0].Rows.Count > 0)
                            {
                                rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                roll_admit = Convert.ToString(dsRollApp.Tables[0].Rows[0]["roll_admit"]);
                                smartno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["smart_serial_no"]);
                            }
                        }
                        string rolldisplay = "Reg No :";
                        string rollvalue = regno;
                        if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                        {
                            rolldisplay = "Roll No :";
                            rollvalue = rollno;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                        {
                            rolldisplay = "Reg No :";
                            rollvalue = regno;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                        {
                            rolldisplay = "Admission No :";
                            rollvalue = roll_admit;
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                        {
                            rolldisplay = "Smartcard No :";
                            rollvalue = smartno;
                        }
                        else
                        {
                            rolldisplay = "App No :";
                            appnoNew = AppNo;
                            app_formno = d2.GetFunction("select app_formno from applyn where app_no='" + AppNo + "'").Trim();

                            rollvalue = app_formno;
                        }

                        //if (ddl_befAftAdmis.SelectedIndex == 0)
                        //{
                        //    rolldisplay = "App No :";
                        //    appnoNew = AppNo;
                        //    app_formno = d2.GetFunction("select app_formno from applyn where app_no='" + AppNo + "'").Trim();

                        //    rollvalue = app_formno;
                        //}



                        //string colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + AppNo + "' and d.college_code=" + collegecode1 + "";
                        string colquery = "";
                        if (rolldisplay != "App No :")
                        {
                            colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                        }
                        else
                        {
                            colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                        }

                        string collegename = string.Empty;
                        string add1 = string.Empty;
                        string add2 = string.Empty;
                        string univ = string.Empty;
                        string degreeCode = string.Empty;
                        string stream = string.Empty;
                        string cursem = string.Empty;
                        string cursemCSe = string.Empty;
                        string batyr = string.Empty;

                        string bankName = string.Empty;
                        string bankCity = string.Empty;

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(colquery, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                degreeCode = Convert.ToString(ds.Tables[1].Rows[0]["Degree_code"]);
                                cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                cursemCSe = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);


                                acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                try
                                {
                                    acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                }
                                catch { }

                                string Termdisp = d2.GetFunction("select UPPER(type) from course where college_code=" + collegecode1 + "").Trim();

                                string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                if (Termdisp.Trim() == "SHIFT I")
                                {
                                    string deptName = d2.GetFunction("select distinct dt.dept_name from degree d,course c,department dt,registration r where r.degree_code=d.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code='" + collegecode1 + "' and app_no='" + appnoNew + "'");
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
                                    if (deptName.ToUpper() == "COMPUTER SCIENCE" || deptName.ToLower() == "computer science" | deptName.ToLower() == "Computer Science")
                                    {
                                        cursem = "Term : " + romanLetter(cursemCSe);
                                    }

                                }
                                else
                                {
                                    cursem = "Term : " + romanLetter(cursem);
                                }
                            }
                        }

                        #endregion


                        string chlnDet = "select HeaderFk,LedgerFK,FeeCategory,TakenAmt,BankFk,FinYearFk,challanType from FT_ChallanDet where challanNo='" + recptNo + "' and App_No ='" + AppNo + "' order by LedgerFK asc";
                        DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                        if (dsDet.Tables.Count > 0)
                        {
                            if (dsDet.Tables[0].Rows.Count > 0)
                            {
                                challanType = Convert.ToInt32(Convert.ToString(dsDet.Tables[0].Rows[0]["challanType"]));

                                string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[0]["FeeCategory"]);


                                string bnkFk = Convert.ToString(dsDet.Tables[0].Rows[0]["BankFk"]);

                                string bnkDetQ1 = "select BankName,City,BankCode,AccNo,Upper(BankBranch) as BankBranch from FM_FinBankMaster where BankPK=" + bnkFk + "";
                                string bankAddress = string.Empty;
                                DataSet dsBnkDet1 = d2.select_method_wo_parameter(bnkDetQ1, "Text");
                                if (dsBnkDet1.Tables.Count > 0)
                                {
                                    if (dsBnkDet1.Tables[0].Rows.Count > 0)
                                    {
                                        bankName = Convert.ToString(dsBnkDet1.Tables[0].Rows[0]["BankName"]);
                                        bankCity = Convert.ToString(dsBnkDet1.Tables[0].Rows[0]["BankBranch"]) + " Branch";
                                        accNo = "A/c No " + Convert.ToString(dsBnkDet1.Tables[0].Rows[0]["AccNo"]);
                                        bankAddress = d2.GetFunction("select Street+', '+(select MasterValue from CO_MasterValues where MasterCode=District)+'-'+PinCode as address from FM_FinBankMaster where BankPK=" + bnkFk + "");
                                        bankAddress = "(" + bankAddress + ")";
                                    }
                                }

                                createPDFOK = true;

                                #region HTML Generation
                                //<div style='height: 710px;width:380px;border:1px solid black;float:left;'></div><div style='margin-left:10px;height: 710px;width:380px;border:1px solid black;float:left;'></div><div style='margin-left:10px;height: 710px;width:380px;border:1px solid black;float:left;'></div><br>
                                sbHtml.Append("<div style='padding-left:50px;height: 780px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 1056px; ' class='classRegular'>");

                                sbHtml.Append("<tr><td  style='font-size:16px;text-align:center;font-weight:bold;'>BANK COPY</td><td></td><td  style='font-size:16px;text-align:center;font-weight:bold;'>COLLEGE COPY</td><td></td><td  style='font-size:16px;text-align:center;font-weight:bold;'>STUDENT COPY</td></tr>");

                                sbHtml.Append("<tr class='classBold10'><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td  style='font-size:15px;'><center ><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td  style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td style='font-size:15px;'><center><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td  style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td  style='font-size:15px;'><center><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td></tr>");

                                sbHtml.Append("<tr class='classBold10' style='text-align:center;font-size:12px;'><td ><table  class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + recptDt + "&nbsp;&nbsp;</td><tr></table></td><td></td><td><table  class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + recptDt + "&nbsp;&nbsp;</td><tr></table ></td><td></td><td><table class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + recptDt + "&nbsp;&nbsp;</td><tr></table></td></tr>");

                                sbHtml.Append("<tr class='classBold10'><td ><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr  style='border: 1px solid black;'><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td></tr>");
                                #endregion

                                #region Challan Top portion

                                int y = 0;

                                Gios.Pdf.PdfPage myprov_pdfpage = mychallan.NewPage();

                                PdfTextArea IOB = new PdfTextArea(FontboldBig, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 70, 5, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);

                                PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 20, 15, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, bankAddress);

                                PdfTextArea FC32 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, 90, 380, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL FOR THE COLLEGE                                                  By D/D or Cash");
                                PdfTextArea FC011 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 20, 80, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(FC011);
                                PdfTextArea FC012 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 20, 92, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(FC012);
                                PdfTextArea UC011 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 80, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(UC011);
                                PdfTextArea UC012 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 350, 92, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(UC012);
                                PdfTextArea TC011 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 690, 80, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(TC011);
                                PdfTextArea TC012 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 690, 92, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                myprov_pdfpage.Add(TC012);
                                PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 105, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);

                                PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, 110, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, 120, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 100, 105, 220, 20), System.Drawing.ContentAlignment.MiddleRight, "Class :  " + deg);
                                PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, 125, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, 140, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS - Arts & Science Major"));
                                PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 280, 140, 130, 20), System.Drawing.ContentAlignment.MiddleLeft, "Amount Rs.");
                                PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                myprov_pdfpage.Add(FC17);
                                string text = "";

                                //First Ends

                                PdfTextArea UC2 = new PdfTextArea(FontboldBig, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 400, 5, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);

                                PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 15, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, bankAddress);


                                PdfTextArea UC32 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, 90, 380, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE FOR THE BANK                                                      By D/D or Cash");

                                PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 105, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 110, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, 120, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 430, 105, 220, 20), System.Drawing.ContentAlignment.MiddleRight, "Class :  " + deg);
                                PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 125, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, 140, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS - Arts & Science Major"));
                                PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 610, 140, 130, 20), System.Drawing.ContentAlignment.MiddleLeft, "Amount Rs.");
                                PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                //second End
                                y = 0;

                                PdfTextArea TC2 = new PdfTextArea(FontboldBig, System.Drawing.Color.Black,
                                                                                                                           new PdfArea(mychallan, 740, 5, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 690, 15, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, bankAddress);


                                PdfTextArea TC9 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 695, 90, 380, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE FOR THE STUDENT                                                By D/D or Cash");
                                PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 695, 105, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);

                                PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 690, 110, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 695, 120, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                PdfTextArea TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 780, 105, 210, 20), System.Drawing.ContentAlignment.MiddleRight, "Class :  " + deg);
                                PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 690, 125, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 695, 140, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS - Arts & Science Major"));
                                PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 950, 140, 130, 20), System.Drawing.ContentAlignment.MiddleLeft, "Amount Rs.");
                                PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 690, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mychallan, 250, 120, 70, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 580, 120, 70, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 920, 120, 70, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                myprov_pdfpage.Add(FC10);
                                myprov_pdfpage.Add(UC10);
                                myprov_pdfpage.Add(TC10);

                                y = -30;

                                PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                      new PdfArea(mychallan, 70, 33, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                      new PdfArea(mychallan, 400, 33, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                      new PdfArea(mychallan, 730, 33, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);

                                myprov_pdfpage.Add(FC4);
                                myprov_pdfpage.Add(UC4);
                                myprov_pdfpage.Add(TC4);

                                myprov_pdfpage.Add(FC10);
                                myprov_pdfpage.Add(UC10);
                                myprov_pdfpage.Add(TC10);

                                #endregion

                                #region Challan Middle Portion

                                //for (int j = 0; j < dsDet.Tables[0].Rows.Count; j++)
                                //{
                                //    string ledger = Convert.ToString(dsDet.Tables[0].Rows[j]["LedgerFK"]);
                                //    string header = Convert.ToString(dsDet.Tables[0].Rows[j]["HeaderFk"]);
                                //    string FeeCategory = Convert.ToString(dsDet.Tables[0].Rows[j]["FeeCategory"]);
                                //    string taknAmt = Convert.ToString(dsDet.Tables[0].Rows[j]["TakenAmt"]);
                                //}

                                string selHeadersQ = string.Empty;
                                DataSet dsHeaders = new DataSet();

                                //if (challanType == 1 || challanType == 2)
                                //{
                                //    string StudStream = string.Empty;

                                //    DataSet dsStr = new DataSet();
                                //    dsStr = d2.select_method_wo_parameter("select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and R.App_No=" + AppNo + "", "Text");
                                //    if (dsStr.Tables.Count > 0)
                                //    {
                                //        if (dsStr.Tables[0].Rows.Count > 0)
                                //        {
                                //            StudStream = Convert.ToString(dsStr.Tables[0].Rows[0][0]);
                                //        }
                                //    }

                                //    selHeadersQ = " select sum(TakenAmt) as TakenAmt,f.ChlGroupHeader  as DispName from FT_ChallanDet d ,FS_ChlGroupHeaderSettings f where d.HeaderFK =f.HeaderFK and   challanNo='" + recptNo + "' and App_No ='" + AppNo + "'  ";
                                //    if (StudStream != "")
                                //    {
                                //        selHeadersQ += " and f.stream='" + StudStream + "' ";
                                //    }
                                //    selHeadersQ += "   group by ChlGroupHeader ";
                                //}
                                //else if (challanType == 3)
                                //{
                                selHeadersQ = " select HeaderFk,SUM(TakenAmt) as TakenAmt,h.HeaderName  as DispName  from FT_ChallanDet d,FM_HeaderMaster h  where d.HeaderFK =h.HeaderPK and challanNo='" + recptNo + "' and d.App_No ='" + AppNo + "' group by HeaderFk,h.HeaderName ";
                                //}
                                //else if (challanType == 4)
                                //{
                                //    selHeadersQ = " select LedgerFK,sum(TakenAmt) as TakenAmt,l.LedgerName as DispName  from FT_ChallanDet d,FM_LedgerMaster l where d.LedgerFK =l.LedgerPK and challanNo='" + recptNo + "' and d.App_No ='" + AppNo + "' group by LedgerFK,l.LedgerName ";
                                //}
                                int heght = 380;


                                if (selHeadersQ != string.Empty)
                                {
                                    dsHeaders.Clear();
                                    dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                    if (dsHeaders.Tables.Count > 0)
                                    {
                                        if (dsHeaders.Tables[0].Rows.Count > 0)
                                        {
                                            int hdrsno = 0;

                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                            {
                                                StringBuilder tempHtml = new StringBuilder();

                                                hdrsno++;
                                                string dispHdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);

                                                if (challanType > 2)
                                                {

                                                    //if (useIFSC == "0")
                                                    //    bnkFk = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                    //else
                                                    //    bnkFk = "A/c No " + d2.GetFunction("SELECT AccNo+'-IFSC '+IFSCCode FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                }
                                                //else
                                                //{
                                                //    bnkFk = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]) + "' AND BankPK = '" + bankPK + "'");
                                                //}

                                                string bnkDetQ = "select BankName,City,BankCode,AccNo,Upper(BankBranch) as BankBranch from FM_FinBankMaster where BankPK=" + bnkFk + "";
                                                DataSet dsBnkDet = d2.select_method_wo_parameter(bnkDetQ, "Text");
                                                if (dsBnkDet.Tables.Count > 0)
                                                {
                                                    if (dsBnkDet.Tables[0].Rows.Count > 0)
                                                    {
                                                        bankName = Convert.ToString(dsBnkDet.Tables[0].Rows[0]["BankName"]);
                                                        bankCity = "(" + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["BankBranch"]) + ")";
                                                        accNo = "A/c No " + Convert.ToString(dsBnkDet.Tables[0].Rows[0]["AccNo"]);
                                                    }
                                                }
                                                string bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                if (useIFSC == "1")
                                                {
                                                    bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo+'-IFSC '+IFSCCode FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                }
                                                dispHdr += " <i><strong>(" + bnkAcc + ")</strong></i>";

                                                string totalAmt = Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                                PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                             new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, hdrsno + "." + dispHdr);

                                                myprov_pdfpage.Add(FC18);

                                                PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                             new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, hdrsno + "." + dispHdr);
                                                myprov_pdfpage.Add(UC18);

                                                PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 695, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, hdrsno + "." + dispHdr);
                                                myprov_pdfpage.Add(TC18);

                                                PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]));
                                                myprov_pdfpage.Add(FC19);
                                                PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                  new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]));
                                                myprov_pdfpage.Add(UC19);
                                                PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 940, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]));
                                                myprov_pdfpage.Add(TC19);
                                                y = y + 5;

                                                string tkn = Convert.ToString(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                                try { tkn = tkn.Split('.')[0]; }
                                                catch { }


                                                //For ledgerwise Details
                                                selHeadersQ = " select LedgerFK,sum(TakenAmt) as TakenAmt,l.LedgerName as DispName  from FT_ChallanDet d,FM_LedgerMaster l where d.LedgerFK =l.LedgerPK and challanNo='" + recptNo + "' and d.App_No ='" + AppNo + "' and d.HeaderFK=" + Convert.ToString(dsHeaders.Tables[0].Rows[head]["HeaderFk"]) + " group by LedgerFK,l.LedgerName ";
                                                DataSet dsLedge = new DataSet();
                                                dsLedge = d2.select_method_wo_parameter(selHeadersQ, "Text");
                                                if (dsLedge.Tables.Count > 0)
                                                {
                                                    if (dsLedge.Tables[0].Rows.Count > 0)
                                                    {
                                                        int ledsno = 0;
                                                        for (int ldr = 0; ldr < dsLedge.Tables[0].Rows.Count; ldr++)
                                                        {
                                                            ledsno++;
                                                            y = y + 7;
                                                            PdfTextArea FC018 = new PdfTextArea(Fontboldled, System.Drawing.Color.Black,
                                                                                                             new PdfArea(mychallan, 25, y + 185, 250, 10), System.Drawing.ContentAlignment.MiddleLeft, "  " + ledsno + "." + Convert.ToString(dsLedge.Tables[0].Rows[ldr]["DispName"]));
                                                            myprov_pdfpage.Add(FC018);
                                                            PdfTextArea UC018 = new PdfTextArea(Fontboldled, System.Drawing.Color.Black,
                                                                                                             new PdfArea(mychallan, 355, y + 185, 250, 10), System.Drawing.ContentAlignment.MiddleLeft, "  " + ledsno + "." + Convert.ToString(dsLedge.Tables[0].Rows[ldr]["DispName"]));
                                                            myprov_pdfpage.Add(UC018);

                                                            PdfTextArea TC018 = new PdfTextArea(Fontboldled, System.Drawing.Color.Black,
                                                                                                                        new PdfArea(mychallan, 695, y + 185, 250, 10), System.Drawing.ContentAlignment.MiddleLeft, "  " + ledsno + "." + Convert.ToString(dsLedge.Tables[0].Rows[ldr]["DispName"]));
                                                            myprov_pdfpage.Add(TC018);
                                                            //PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            //                                    new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsLedge.Tables[0].Rows[ldr]["TakenAmt"]));
                                                            //myprov_pdfpage.Add(FC19);
                                                            //PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            //                                  new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsLedge.Tables[0].Rows[ldr]["TakenAmt"]));
                                                            //myprov_pdfpage.Add(UC19);
                                                            //PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            //                                    new PdfArea(mychallan, 940, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, Convert.ToString(dsLedge.Tables[0].Rows[ldr]["TakenAmt"]));
                                                            //myprov_pdfpage.Add(TC19);

                                                            tempHtml.Append("<br><span class='classRegular' style='font-size:9px; width:320px;PADDING-LEFT:10PX;'>" + ledsno + "." + Convert.ToString(dsLedge.Tables[0].Rows[ldr]["DispName"]) + "</span>");
                                                            heght -= 10;

                                                        }
                                                    }
                                                }


                                                //Ledgerwise Details End
                                                PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, y + 190, 300, 10), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                                myprov_pdfpage.Add(FC171);

                                                PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 350, y + 190, 300, 10), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                                myprov_pdfpage.Add(UC171);



                                                PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 690, y + 190, 300, 10), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                                myprov_pdfpage.Add(TC171);
                                                y = y + 15;

                                                sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(Convert.ToDouble(tkn)) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(Convert.ToDouble(tkn)) + "</tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'   BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(Convert.ToDouble(tkn)) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(Convert.ToDouble(tkn)) + "</tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(Convert.ToDouble(tkn)) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(Convert.ToDouble(tkn)) + "</tr></table></td></tr>");
                                                heght -= 13;
                                            }
                                        }
                                    }
                                }

                                #region Denomionation and Particulars

                                sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(total), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(total)) + " Rupees Only)" + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'   BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(total), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(total)) + " Rupees Only)" + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(total), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(total)) + " Rupees Only)" + "</td></tr></table></td></tr>");

                                sbHtml.Append("<tr><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td></tr>");
                                sbHtml.Append("<tr><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td></tr>");
                                //sbHtml.Append("<tr><td style='border:none;'>&nbsp;</td><tr>");
                                if (useDenom == 1)
                                {
                                    //College
                                    sbHtml.Append("<tr><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td></tr>");

                                    sbHtml.Append("<tr><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td></tr>");
                                }
                                if (useDenom == 2)
                                {
                                    //Bank
                                    sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td><td></td><td></td></tr>");

                                    sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td><td></td><td></td></tr>");
                                }
                                if (useDenom == 3)
                                {
                                    //Student
                                    sbHtml.Append("<tr><td></td><td></td><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                    sbHtml.Append("<tr><td></td><td></td><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td></tr>");

                                }
                                if (useDenom == 4)
                                {
                                    //All

                                    sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                    sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td></tr>");
                                }
                                if (useDenom == 5)
                                {
                                    //College and Bank
                                    sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td></tr>");

                                    sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td><td></td><td></td></tr>");

                                }
                                if (useDenom == 6)
                                {
                                    //Student and Bank     
                                    sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                    sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td></tr>");

                                }
                                if (useDenom == 7)
                                {
                                    //College and Student
                                    sbHtml.Append("<tr><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                    sbHtml.Append("<tr><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td></tr>");

                                }

                                #endregion

                                #endregion

                                #region Bottom Portion of Challan

                                PdfTextArea FC04 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                              new PdfArea(mychallan, 70, 25, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "New College Fee Collection Counter");
                                myprov_pdfpage.Add(FC04);
                                PdfTextArea UC04 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 400, 25, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "New College Fee Collection Counter");
                                myprov_pdfpage.Add(UC04);
                                PdfTextArea TC04 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 740, 25, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "New College Fee Collection Counter");
                                myprov_pdfpage.Add(TC04);

                                Gios.Pdf.PdfTable tableHr1 = mychallan.NewTable(Fontsmall, 2, 3, 1);
                                tableHr1.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);
                                tableHr1.VisibleHeaders = false;
                                tableHr1.Columns[0].SetWidth(100);
                                tableHr1.Columns[1].SetWidth(120);
                                tableHr1.Columns[2].SetWidth(80);

                                tableHr1.Cell(0, 1).SetContent("Receipt No:\n(Office Use Only) ");
                                tableHr1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr1.Cell(0, 1).SetFont(Fontsmall);

                                tableHr1.Cell(0, 0).SetContent("ChallanNo.:" + recptNo);
                                tableHr1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr1.Cell(0, 0).SetFont(Fontbold);

                                tableHr1.Cell(0, 2).SetContent("Date:" + recptDt);
                                tableHr1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                tableHr1.Cell(0, 2).SetFont(Fontsmall);

                                Gios.Pdf.PdfTablePage myprov_pdfpagetableHR1 = tableHr1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 25, 68, 300, 20));

                                myprov_pdfpage.Add(myprov_pdfpagetableHR1);

                                Gios.Pdf.PdfTable tableHr2 = mychallan.NewTable(Fontsmall, 2, 3, 1);
                                tableHr2.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);
                                tableHr2.VisibleHeaders = false;
                                tableHr2.Columns[0].SetWidth(100);
                                tableHr2.Columns[1].SetWidth(120);
                                tableHr2.Columns[2].SetWidth(80);

                                tableHr2.Cell(0, 1).SetContent("Receipt No:\n(Office Use Only) ");
                                tableHr2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr2.Cell(0, 1).SetFont(Fontsmall);

                                tableHr2.Cell(0, 0).SetContent("ChallanNo.:" + recptNo);
                                tableHr2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr2.Cell(0, 0).SetFont(Fontbold);

                                tableHr2.Cell(0, 2).SetContent("Date:" + recptDt);
                                tableHr2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                tableHr2.Cell(0, 2).SetFont(Fontsmall);

                                Gios.Pdf.PdfTablePage myprov_pdfpagetableHR2 = tableHr2.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 355, 68, 300, 20));

                                myprov_pdfpage.Add(myprov_pdfpagetableHR2);

                                Gios.Pdf.PdfTable tableHr3 = mychallan.NewTable(Fontsmall, 2, 3, 1);
                                tableHr3.SetBorders(Color.Black, 1, BorderType.ColumnsAndBounds);
                                tableHr3.VisibleHeaders = false;
                                tableHr3.Columns[0].SetWidth(100);
                                tableHr3.Columns[1].SetWidth(120);
                                tableHr3.Columns[2].SetWidth(80);

                                tableHr3.Cell(0, 1).SetContent("Receipt No:\n(Office Use Only) ");
                                tableHr3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr3.Cell(0, 1).SetFont(Fontsmall);

                                tableHr3.Cell(0, 0).SetContent("ChallanNo.:" + recptNo);
                                tableHr3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                tableHr3.Cell(0, 0).SetFont(Fontbold);

                                tableHr3.Cell(0, 2).SetContent("Date:" + recptDt);
                                tableHr3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                tableHr3.Cell(0, 2).SetFont(Fontsmall);

                                Gios.Pdf.PdfTablePage myprov_pdfpagetableHR3 = tableHr3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 695, 68, 300, 20));

                                myprov_pdfpage.Add(myprov_pdfpagetableHR3);



                                PdfTextArea FC001 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                  new PdfArea(mychallan, 25, 55, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "COLLEGE FEES CHALLAN (" + shift + ")");
                                myprov_pdfpage.Add(FC001);
                                PdfTextArea UC001 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                              new PdfArea(mychallan, 350, 55, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "COLLEGE FEES CHALLAN (" + shift + ")");
                                myprov_pdfpage.Add(UC001);
                                PdfTextArea TC001 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                             new PdfArea(mychallan, 700, 55, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "COLLEGE FEES CHALLAN (" + shift + ")");
                                myprov_pdfpage.Add(TC001);
                                PdfTextArea FC0001 = new PdfTextArea(FontboldBig1, System.Drawing.Color.Black,
                                                                                  new PdfArea(mychallan, 25, 43, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "THE NEW COLLEGE (AUTONOMOUS CH-14)");
                                myprov_pdfpage.Add(FC0001);
                                PdfTextArea UC0001 = new PdfTextArea(FontboldBig1, System.Drawing.Color.Black,
                                                                              new PdfArea(mychallan, 350, 43, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "THE NEW COLLEGE (AUTONOMOUS CH-14)");
                                myprov_pdfpage.Add(UC0001);
                                PdfTextArea TC0001 = new PdfTextArea(FontboldBig1, System.Drawing.Color.Black,
                                                                             new PdfArea(mychallan, 700, 43, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, "THE NEW COLLEGE (AUTONOMOUS CH-14)");
                                myprov_pdfpage.Add(TC0001);



                                text = "(" + DecimalToWords((decimal)Convert.ToDouble(total)) + " Rupees Only)";

                                PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Total");
                                PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 25, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Cashier");
                                PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 250, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Manager / Acct.");

                                PdfArea tete = new PdfArea(mychallan, 20, 5, 310, y + 255);
                                PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                myprov_pdfpage.Add(pr1);

                                PdfArea tete2 = new PdfArea(mychallan, 350, 5, 310, y + 255);
                                PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                myprov_pdfpage.Add(pr2);

                                PdfArea tete3 = new PdfArea(mychallan, 690, 5, 310, y + 255);
                                PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                myprov_pdfpage.Add(pr3);

                                PdfTextArea FC0015 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                       new PdfArea(mychallan, 25, y + 257, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS OF DEMAND DRAFT AND DENOMINATION"));
                                myprov_pdfpage.Add(FC0015);
                                PdfTextArea UC0015 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 355, y + 257, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS OF DEMAND DRAFT AND DENOMINATION"));
                                myprov_pdfpage.Add(UC0015);
                                PdfTextArea TC0015 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                 new PdfArea(mychallan, 695, y + 257, 250, 20), System.Drawing.ContentAlignment.MiddleCenter, String.Format("PARTICULARS OF DEMAND DRAFT AND DENOMINATION"));
                                myprov_pdfpage.Add(TC0015);

                                Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 5, 5);
                                table.VisibleHeaders = false;
                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table.Columns[0].SetWidth(60);
                                table.Columns[1].SetWidth(60);
                                table.Columns[2].SetWidth(60);
                                table.Columns[3].SetWidth(60);
                                table.Columns[4].SetWidth(60);

                                table.Cell(0, 0).SetContent("Name of Bank");
                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 0).SetFont(Fontsmall);
                                table.Cell(0, 1).SetContent("Place of Bank");
                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 1).SetFont(Fontsmall);
                                table.Cell(0, 2).SetContent("Draft Number");
                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 2).SetFont(Fontsmall);
                                table.Cell(0, 3).SetContent("Date");
                                table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 3).SetFont(Fontsmall);
                                table.Cell(0, 4).SetContent("Amount");
                                table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(0, 4).SetFont(Fontsmall);

                                table.Cell(1, 0).SetContent("\n");
                                table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 0).SetFont(Fontsmall);
                                table.Cell(1, 1).SetContent("\n");
                                table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 1).SetFont(Fontsmall);
                                table.Cell(1, 2).SetContent("\n");
                                table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 2).SetFont(Fontsmall);
                                table.Cell(1, 3).SetContent("\n");
                                table.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 3).SetFont(Fontsmall);
                                table.Cell(1, 4).SetContent("\n");
                                table.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(1, 4).SetFont(Fontsmall);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 270, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable);

                                Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 5, 4, 3);
                                table1.VisibleHeaders = false;
                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                //table1.Columns[0].SetWidth(100);
                                //table1.Columns[1].SetWidth(60);
                                table1.Cell(0, 0).SetContent("1000  x");
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(0, 0).SetFont(Fontbold1);
                                table1.Cell(1, 0).SetContent("500   x");
                                table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(1, 0).SetFont(Fontbold1);
                                table1.Cell(0, 2).SetContent("20    x");
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(0, 2).SetFont(Fontbold1);
                                table1.Cell(1, 2).SetContent("10    x");
                                table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(1, 2).SetFont(Fontbold1);

                                table1.Cell(2, 0).SetContent("100   x");
                                table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(2, 0).SetFont(Fontbold1);
                                table1.Cell(3, 0).SetContent("50    x");
                                table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(3, 0).SetFont(Fontbold1);
                                table1.Cell(2, 2).SetContent("5     x");
                                table1.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(2, 2).SetFont(Fontbold1);
                                table1.Cell(3, 2).SetContent("Coins x");
                                table1.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(3, 2).SetFont(Fontbold1);
                                table1.Cell(4, 0).SetContent("Total");
                                table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(4, 0).SetFont(Fontbold1);
                                table1.Cell(4, 1).ColSpan = 3;

                                Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 310, 310, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable1);


                                myprov_pdfpage.Add(IOB);
                                //myprov_pdfpage.Add(FC4);

                                myprov_pdfpage.Add(FC6);
                                myprov_pdfpage.Add(FC9);

                                myprov_pdfpage.Add(FC11);
                                myprov_pdfpage.Add(FC12);
                                myprov_pdfpage.Add(FC13);
                                myprov_pdfpage.Add(FC14);
                                myprov_pdfpage.Add(FC15);
                                myprov_pdfpage.Add(FC16);

                                myprov_pdfpage.Add(FC24);
                                myprov_pdfpage.Add(FC25);
                                myprov_pdfpage.Add(FC26);
                                myprov_pdfpage.Add(FC27);
                                myprov_pdfpage.Add(FC28);
                                myprov_pdfpage.Add(FC29);
                                myprov_pdfpage.Add(FC30);

                                myprov_pdfpage.Add(FC32);


                                //First End
                                myprov_pdfpage.Add(UC17);

                                PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Total");
                                PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 355, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Cashier");
                                PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 580, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Manager / Acct.");

                                Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 5, 5);
                                table3.VisibleHeaders = false;
                                table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table3.Columns[0].SetWidth(60);
                                table3.Columns[1].SetWidth(60);
                                table3.Columns[2].SetWidth(60);
                                table3.Columns[3].SetWidth(60);
                                table3.Columns[4].SetWidth(60);

                                table3.Cell(0, 0).SetContent("Name of Bank");
                                table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 0).SetFont(Fontsmall);
                                table3.Cell(0, 1).SetContent("Place of Bank");
                                table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 1).SetFont(Fontsmall);
                                table3.Cell(0, 2).SetContent("Draft Number");
                                table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 2).SetFont(Fontsmall);
                                table3.Cell(0, 3).SetContent("Date");
                                table3.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 3).SetFont(Fontsmall);
                                table3.Cell(0, 4).SetContent("Amount");
                                table3.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 4).SetFont(Fontsmall);

                                table3.Cell(1, 0).SetContent("\n");
                                table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 0).SetFont(Fontsmall);
                                table3.Cell(1, 1).SetContent("\n");
                                table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 1).SetFont(Fontsmall);
                                table3.Cell(1, 2).SetContent("\n");
                                table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 2).SetFont(Fontsmall);
                                table3.Cell(1, 3).SetContent("\n");
                                table3.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 3).SetFont(Fontsmall);
                                table3.Cell(1, 4).SetContent("\n");
                                table3.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(1, 4).SetFont(Fontsmall);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 270, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable3);

                                Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 5, 4, 3);
                                table14.VisibleHeaders = false;
                                table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                //table14.Columns[0].SetWidth(100);
                                //table14.Columns[1].SetWidth(60);
                                table14.Cell(0, 0).SetContent("1000  x");
                                table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(0, 0).SetFont(Fontbold1);
                                table14.Cell(1, 0).SetContent("500   x");
                                table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(1, 0).SetFont(Fontbold1);
                                table14.Cell(0, 2).SetContent("20    x");
                                table14.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(0, 2).SetFont(Fontbold1);
                                table14.Cell(1, 2).SetContent("10    x");
                                table14.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(1, 2).SetFont(Fontbold1);

                                table14.Cell(2, 0).SetContent("100   x");
                                table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(2, 0).SetFont(Fontbold1);
                                table14.Cell(3, 0).SetContent("50    x");
                                table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(3, 0).SetFont(Fontbold1);
                                table14.Cell(2, 2).SetContent("5     x");
                                table14.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(2, 2).SetFont(Fontbold1);
                                table14.Cell(3, 2).SetContent("Coins x");
                                table14.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(3, 2).SetFont(Fontbold1);
                                table14.Cell(4, 0).SetContent("Total");
                                table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table14.Cell(4, 0).SetFont(Fontbold1);
                                table14.Cell(4, 1).ColSpan = 3;

                                Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 310, 310, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable4);


                                myprov_pdfpage.Add(UC2);
                                myprov_pdfpage.Add(UC6);

                                myprov_pdfpage.Add(UC9);

                                myprov_pdfpage.Add(UC11);
                                myprov_pdfpage.Add(UC12);
                                myprov_pdfpage.Add(UC13);
                                myprov_pdfpage.Add(UC14);
                                myprov_pdfpage.Add(UC15);
                                myprov_pdfpage.Add(UC16);


                                myprov_pdfpage.Add(UC24);
                                myprov_pdfpage.Add(UC25);
                                myprov_pdfpage.Add(UC26);
                                myprov_pdfpage.Add(UC27);
                                myprov_pdfpage.Add(UC28);
                                myprov_pdfpage.Add(UC29);
                                myprov_pdfpage.Add(UC30);
                                myprov_pdfpage.Add(UC32);
                                //second End


                                myprov_pdfpage.Add(TC17);

                                PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 695, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Total");
                                PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 940, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(Convert.ToDouble(total)) + "." + returnDecimalPart(Convert.ToDouble(total)));
                                PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 690, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 695, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 695, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                new PdfArea(mychallan, 695, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Cashier");
                                PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                            new PdfArea(mychallan, 920, y + 245, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Manager / Acct.");


                                Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 5, 5);
                                table5.VisibleHeaders = false;
                                table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table5.Columns[0].SetWidth(60);
                                table5.Columns[1].SetWidth(60);
                                table5.Columns[2].SetWidth(60);
                                table5.Columns[3].SetWidth(60);
                                table5.Columns[4].SetWidth(60);

                                table5.Cell(0, 0).SetContent("Name of Bank");
                                table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 0).SetFont(Fontsmall);
                                table5.Cell(0, 1).SetContent("Place of Bank");
                                table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 1).SetFont(Fontsmall);
                                table5.Cell(0, 2).SetContent("Draft Number");
                                table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 2).SetFont(Fontsmall);
                                table5.Cell(0, 3).SetContent("Date");
                                table5.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 3).SetFont(Fontsmall);
                                table5.Cell(0, 4).SetContent("Amount");
                                table5.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(0, 4).SetFont(Fontsmall);

                                table5.Cell(1, 0).SetContent("\n");
                                table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 0).SetFont(Fontsmall);
                                table5.Cell(1, 1).SetContent("\n");
                                table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 1).SetFont(Fontsmall);
                                table5.Cell(1, 2).SetContent("\n");
                                table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 2).SetFont(Fontsmall);
                                table5.Cell(1, 3).SetContent("\n");
                                table5.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 3).SetFont(Fontsmall);
                                table5.Cell(1, 4).SetContent("\n");
                                table5.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table5.Cell(1, 4).SetFont(Fontsmall);
                                Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 690, y + 270, 310, 250));
                                myprov_pdfpage.Add(myprov_pdfpagetable31);

                                Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 5, 4, 3);
                                table15.VisibleHeaders = false;
                                table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                //table15.Columns[0].SetWidth(100);
                                //table15.Columns[1].SetWidth(60);
                                table15.Cell(0, 0).SetContent("1000  x");
                                table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(0, 0).SetFont(Fontbold1);
                                table15.Cell(1, 0).SetContent("500   x");
                                table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(1, 0).SetFont(Fontbold1);
                                table15.Cell(0, 2).SetContent("20    x");
                                table15.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(0, 2).SetFont(Fontbold1);
                                table15.Cell(1, 2).SetContent("10    x");
                                table15.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(1, 2).SetFont(Fontbold1);

                                table15.Cell(2, 0).SetContent("100   x");
                                table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(2, 0).SetFont(Fontbold1);
                                table15.Cell(3, 0).SetContent("50    x");
                                table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(3, 0).SetFont(Fontbold1);
                                table15.Cell(2, 2).SetContent("5     x");
                                table15.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(2, 2).SetFont(Fontbold1);
                                table15.Cell(3, 2).SetContent("Coins x");
                                table15.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(3, 2).SetFont(Fontbold1);
                                table15.Cell(4, 0).SetContent("Total");
                                table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table15.Cell(4, 0).SetFont(Fontbold1);
                                table15.Cell(4, 1).ColSpan = 3;

                                Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 690, y + 310, 310, 500));
                                myprov_pdfpage.Add(myprov_pdfpagetable5);

                                myprov_pdfpage.Add(TC2);
                                myprov_pdfpage.Add(TC6);

                                myprov_pdfpage.Add(TC9);

                                myprov_pdfpage.Add(TC11);
                                myprov_pdfpage.Add(TC12);
                                myprov_pdfpage.Add(TC13);
                                myprov_pdfpage.Add(TC14);
                                myprov_pdfpage.Add(TC15);
                                myprov_pdfpage.Add(TC16);
                                myprov_pdfpage.Add(TC17);
                                myprov_pdfpage.Add(TC24);
                                myprov_pdfpage.Add(TC25);
                                myprov_pdfpage.Add(TC26);
                                myprov_pdfpage.Add(TC27);
                                myprov_pdfpage.Add(TC28);
                                myprov_pdfpage.Add(TC29);
                                myprov_pdfpage.Add(TC30);
                                myprov_pdfpage.Add(TC32);

                                myprov_pdfpage.SaveToDocument();
                                #endregion

                                sbHtml.Append("<tr>");


                                sbHtml.Append("</tr>");
                                sbHtml.Append("</table></div><br>");
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
                        contentDiv.InnerHtml += sbHtml.ToString();
                    }
                }
                #endregion

                #region To print the challan
                if (createPDFOK)
                {
                    #region New Print
                    contentDiv.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
                    #endregion
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Challan Cannot Be Generated";
                }
                #endregion
            }
            catch (Exception ex)
            {
                // d2.sendErrorMail(ex, collegecode1, "ChallanConfirm");
            }
        }
        else
        {
            imgAlert.Visible = true;
            lbl_alert.Text = "Please Select A Challan";
        }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        switch (Convert.ToInt32(rbl_rollno.SelectedItem.Value))
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
        string type = string.Empty;
        if (streamStat != string.Empty)
        {
            type = " and c.type in ('" + streamStat + "') ";
        }

        string batchDeg = string.Empty;
        if (!string.IsNullOrEmpty(searchFltValues))
            batchDeg = searchFltValues;

        string query = "select top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and (r.CC=0 or r.cc=1) and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%' and a.App_No in (select distinct App_No from FT_ChallanDet )" + type + batchDeg;
        if (chosedmode == 1)
        {
            query = "select top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Reg_No,r.Reg_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and (r.CC=0 or r.cc=1) and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%' and a.App_No in (select distinct App_No from FT_ChallanDet )" + type + batchDeg;
        } if (chosedmode == 2)
        {
            query = "select top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_Admit,r.Roll_Admit from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and (r.CC=0 or r.cc=1) and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%' and a.App_No in (select distinct App_No from FT_ChallanDet )" + type + batchDeg;
        } if (chosedmode == 3)
        {
            query = "select top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+a.app_formno,a.app_formno from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and (r.CC=0 or r.cc=1) and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%' and a.App_No in (select distinct App_No from FT_ChallanDet )" + type + batchDeg;
        }
        if (chosedmode == 4)
        {
            query = "select top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.smart_serial_no,r.smart_serial_no from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and (r.CC=0 or r.cc=1) and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%' and a.App_No in (select distinct App_No from FT_ChallanDet )" + type + batchDeg;
        }

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

            string type = string.Empty;
            if (streamStat != string.Empty)
            {
                type = " and c.type in ( " + streamStat + ")";
            }
            //student query
            if (chosedmode == 0)
            {
                query = "select top 100 Roll_No from Registration r,degree d,Course c where r.degree_code=d.degree_code and c.course_id=d.course_id and  (CC=0 or cc=1) and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and r.college_code=" + collegecodestat + type + "  and App_No in (select distinct App_No from FT_ChallanDet ) order by Roll_No asc";

                // query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecodestat + type + "  and App_No in (select distinct App_No from FT_ChallanDet ) order by Roll_No asc";
            }
            else if (chosedmode == 1)
            {
                query = "select top 100 Reg_No from Registration r,degree d,Course c where r.degree_code=d.degree_code and c.course_id=d.course_id and  (CC=0 or cc=1) and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and r.college_code=" + collegecodestat + type + "  and App_No in (select distinct App_No from FT_ChallanDet ) order by Reg_No asc";
                //query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code=" + collegecodestat +type+ "  and App_No in (select distinct App_No from FT_ChallanDet )  order by Reg_No asc";
            }
            else if (chosedmode == 2)
            {
                query = "select top 100 Roll_admit from Registration r,degree d,Course c where r.degree_code=d.degree_code and c.course_id=d.course_id and  (CC=0 or cc=1) and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and r.college_code=" + collegecodestat + type + "  and App_No in (select distinct App_No from FT_ChallanDet ) order by Roll_admit asc";
                //query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecodestat +type+ "  and App_No in (select distinct App_No from FT_ChallanDet )  order by Roll_admit asc";
            }
            else if (chosedmode == 4)
            {
                query = "select top 100 smart_serial_no from Registration r,degree d,Course c where r.degree_code=d.degree_code and c.course_id=d.course_id and  (CC=0 or cc=1) and DelFlag =0 and Exam_Flag <>'DEBAR' and smart_serial_no like '" + prefixText + "%' and r.college_code=" + collegecodestat + type + "  and App_No in (select distinct App_No from FT_ChallanDet ) order by smart_serial_no asc";
                //query = "select  top 100 smart_serial_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and smart_serial_no like '" + prefixText + "%' and college_code=" + collegecodestat +type+ "  and App_No in (select distinct App_No from FT_ChallanDet )  order by smart_serial_no asc";
            }
            else
            {
                byte studAppSHrtAdm = statStudentAppliedShorlistAdmit();
                string admStudFilter = "";
                switch (studAppSHrtAdm)
                {
                    case 0:
                        admStudFilter = " and r.isconfirm=1 ";
                        break;
                    case 1:
                        admStudFilter = " and r.isconfirm=1 and r.selection_status=1 ";
                        break;
                    case 2:
                        admStudFilter = " and r.isconfirm=1 and r.selection_status=1 and r.admission_status=1 ";
                        break;
                }
                query = "select top 100 app_formno from applyn r,degree d,Course c where r.degree_code=d.degree_code and c.course_id=d.course_id  and app_formno like '" + prefixText + "%' and r.college_code=" + collegecodestat + type + "  and App_No in (select distinct App_No from FT_ChallanDet ) " + admStudFilter + " order by app_formno asc";
                //query = "  select  top 100 app_formno from applyn where    app_formno like '" + prefixText + "%' and college_code=" + collegecodestat +type+ "  and App_No in (select distinct App_No from FT_ChallanDet ) " + admStudFilter + "  order by app_formno asc ";
            }


            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }


    public string generateReceiptNo(out string rcpracr, out string hdrSetPK, string hdrs)
    {
        hdrSetPK = string.Empty;
        rcpracr = string.Empty;
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
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
        if (isHeaderwise == 0)
        {
            return getCommonReceiptNo(out rcpracr, out hdrSetPK);
        }
        else
        {
            return getHeaderwiseReceiptNo(out rcpracr, out hdrSetPK, hdrs);
        }
    }
    private string getCommonReceiptNo(out string rcpracr, out string hdrSetPK)
    {
        hdrSetPK = string.Empty;
        rcpracr = string.Empty;
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            // string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string finYearid = Convert.ToString(ddlfinyear.SelectedValue);
            string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
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

                string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;
                rcpracr = recacr;

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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); return recno; }
    }

    private string getHeaderwiseReceiptNo(out string rcpracr, out string hdrSetPK, string hdrs)
    {
        hdrSetPK = string.Empty;
        rcpracr = string.Empty;
        string recno = string.Empty;

        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;
            string isheaderFk = hdrs;

            // string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string finYearid = Convert.ToString(ddlfinyear.SelectedValue);
            DataSet dsFinHedDet = d2.select_method_wo_parameter("select distinct HeaderSettingFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and HeaderFK in (" + isheaderFk + ") and CollegeCode=" + collegecode1 + " and FinyearFK=" + finYearid + " and isnull(onlineRec,'')<>'1'", "Text");

            if (dsFinHedDet.Tables.Count > 0 && dsFinHedDet.Tables[0].Rows.Count == 1)
            {
                hdrSetPK = Convert.ToString(dsFinHedDet.Tables[0].Rows[0][0]);
                string secondreciptqurey = "select * from FM_HeaderFinCodeSettings where HeaderSettingPK =" + hdrSetPK + " and FinyearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1'";
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
                    rcpracr = recacr;
                }
            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }
    //Reusable Methods
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
            byte check = 0;
            byte.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Value), out check);
            if (check == 1)
            {
                Ok = true;
            }
        }
        return Ok;
    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Challan Datewise Report";
            string pagename = "ChallanConfirm.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }

    }
    protected void cb_batchDeg_Change(object sender, EventArgs e)
    {
        try
        {
            if (cb_batchDeg.Checked == false)
            {
                //ddl_strm.Enabled = false;
                //txt_stream.Enabled = false;
                txt_degree.Enabled = false;
                txt_dept.Enabled = false;
                txt_batch.Enabled = false;
                txt_sem.Enabled = false;
                searchFltValues = getfltrValues();
            }
            else
            {
                //if (ddl_strm.Items.Count > 0)
                //{
                //    ddl_strm.Enabled = true;
                //    txt_stream.Enabled = true;
                //}
                txt_degree.Enabled = true;
                txt_dept.Enabled = true;
                txt_batch.Enabled = true;
                txt_sem.Enabled = true;
                searchFltValues = string.Empty;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void btnchangeconfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (checkedOK())
            {
                lblconfirmdate.Text = "Do You Want To Change The Challan Confirm Date ?";
                Div1.Visible = true;
            }
            else
            {
                lbl_alert.Text = "Please Select Any One Record";
                imgAlert.Visible = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void btn_confirmchange_yes_Change(object sender, EventArgs e)
    {
        try
        {
            if (checkedOK())
            {
                bool checkfla = false;
                Div1.Visible = false;
                string actualSelect = string.Empty;
                string actualInsert = string.Empty;
                string actualInsertVal = string.Empty;
                string AppFormNo = string.Empty;
                string chlnNo = string.Empty;
                string chlnDt = string.Empty;
                string trasdate = string.Empty;
                for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                    if (check == 1)
                    {
                        chlnNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                        chlnDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);
                        AppFormNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Text);
                        string studname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 6].Text);
                        string dept = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 7].Text);
                        string total = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Tag);
                        string AppNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                        string actualFinyearFk = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Note);
                        if (!string.IsNullOrEmpty(actualFinyearFk))
                        {
                            actualSelect = " and finyearfk='" + actualFinyearFk + "'";
                            actualInsert = ",actualfinyearfk";
                            actualInsertVal = ",'" + actualFinyearFk + "'";
                        }
                        trasdate = txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2];
                        string rcptNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 8].Text).Trim();
                        string rcptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 9].Text).Trim();
                        if (rcptNo != string.Empty && rcptDt != string.Empty)
                        {
                            // chlnDt = chlnDt.Split('/')[1] + "/" + chlnDt.Split('/')[0] + "/" + chlnDt.Split('/')[2];
                            string query = "";
                            query = "update FT_FinDailyTransaction set TransDate ='" + trasdate + "' where App_No =" + AppNo + " and DDNo ='" + chlnNo + "'";
                            query = query + " update FT_ChallanDet set RcptTransDate ='" + trasdate + "' where App_No =" + AppNo + " and ChallanNo ='" + chlnNo + "'";

                            int upd = d2.update_method_wo_parameter(query, "Text");
                            if (upd != 0)
                            {
                                checkfla = true;
                                FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                            }
                        }
                    }
                }
                if (checkfla == true)
                {
                    FpSpread1.SaveChanges();
                    FpSpread1.Sheets[0].Cells[0, 1].Value = 0;
                    // btn_go_Click(sender, e);
                    lbl_alert.Text = "Updated Successfully";
                    imgAlert.Visible = true;

                    //==================Added by saranya on 11/04/2018=================//
                    int savevalue = 1;
                    string entrycode = Session["Entry_Code"].ToString();
                    string formname = "ChallanDateUpdate";
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
                    string details = "RollNO - " + AppFormNo + " :ChallanNo - " + chlnNo + " : ChallanDate - " + trasdate + " : Date - " + toa + "";
                    string ctsname = "";
                    if (savevalue == 1)
                    {
                        ctsname = "ChallanDateUpdate";
                    }
                    string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                    d2.insertEinanceUserActionLog(entrycode, formname, 2, toa, doa, details, ctsname, localip);
                    //==============================================================//
                }
                else
                {
                    lbl_alert.Text = "Not Updated";
                    imgAlert.Visible = true;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
    }
    protected void btn_confirmchange_no_Change(object sender, EventArgs e)
    {
        try
        {
            Div1.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanConfirm"); }
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
    //public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CheckBox_column.Checked = false;
    //        string value = "";
    //        int index;
    //        cblcolumnorder.Items[0].Selected = true;

    //        value = string.Empty;
    //        string result = Request.Form["__EVENTTARGET"];
    //        string[] checkedBox = result.Split('$');
    //        index = int.Parse(checkedBox[checkedBox.Length - 1]);
    //        string sindex = Convert.ToString(index);

    //        int a = 0;
    //        for (int i = 0; i < cblcolumnorder.Items.Count; i++)
    //        {
    //            if (cblcolumnorder.Items[i].Selected) a++;
    //        }
    //        if (a == 10)
    //        {
    //            CheckBox_column.Checked = true;
    //        }
    //        if (a == 0)
    //        {
    //            lnk_columnorder.Visible = false;
    //        }

    //    }
    //    catch
    //    {

    //    }
    //}
    //public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (CheckBox_column.Checked == true)
    //        {
    //            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
    //            {
    //                string si = Convert.ToString(i);
    //                cblcolumnorder.Items[i].Selected = true;
    //                lnk_columnorder.Visible = true;
    //            }
    //            lnk_columnorder.Visible = true;
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
    //            {
    //                cblcolumnorder.Items[i].Selected = false;
    //                lnk_columnorder.Visible = false;
    //            }
    //        }
    //        cblcolumnorder.Items[0].Selected = true;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;

        }
        catch (Exception ex)
        {
        }
    }

    //column order setting added by sudhagar
    public bool columncount()
    {
        bool colorder = false;
        try
        {
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == true)
                {
                    colorder = true;
                }
            }
        }
        catch { }
        return colorder;
    }
    public void loadcolumns()
    {
        try
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string linkname = "ChallanConfirm column order settings";
            string columnvalue = "";
            int clsupdate = 0;
            collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "' ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (columncount() == true)
            {
                if (cblcolumnorder.Items.Count > 0)
                {
                    // colord.Clear();
                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                    {
                        if (cblcolumnorder.Items[i].Selected == true)
                        {
                            // colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                            if (columnvalue == "")
                                columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                            else
                                columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0 && dscol.Tables[0].Rows.Count > 0)
            {
                for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                {
                    string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                    string[] valuesplit = value.Split(',');
                    if (valuesplit.Length > 0)
                    {
                        for (int k = 0; k < valuesplit.Length; k++)
                        {
                            //  colord.Add(Convert.ToString(valuesplit[k]));
                            if (columnvalue == "")
                                columnvalue = Convert.ToString(valuesplit[k]);
                            else
                                columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string text = Convert.ToString(cblcolumnorder.Items[i].Text);
                    if (text.Trim() == "Challan No" || text.Trim() == "Challan Date" || text.Trim() == "Total")
                    {
                        cblcolumnorder.Items[i].Selected = true;
                        if (columnvalue == "")
                            columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                        else
                            columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                    }
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and college_code ='" + collegecode1 + "' and user_code='" + usercode + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode1 + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "' ";
                DataSet dscolor = new DataSet();
                dscolor.Clear();
                dscolor = d2.select_method_wo_parameter(sel, "Text");
                //  ItemList.Clear();
                if (dscolor.Tables.Count > 0 && dscolor.Tables[0].Rows.Count > 0)
                {
                    int count = 0;
                    string value = Convert.ToString(dscolor.Tables[0].Rows[0]["LinkValue"]);
                    string[] value1 = value.Split(',');
                    if (value1.Length > 0)
                    {
                        for (int i = 0; i < value1.Length; i++)
                        {
                            string val = value1[i].ToString();
                            for (int k = 0; k < cblcolumnorder.Items.Count; k++)
                            {
                                if (val == cblcolumnorder.Items[k].Value)
                                {
                                    cblcolumnorder.Items[k].Selected = true;
                                    if (!ItemList.Contains(cblcolumnorder.Items[k].Text))
                                    {
                                        ItemList.Add(cblcolumnorder.Items[k].Text);
                                    }
                                    count++;
                                }
                            }
                            if (count == cblcolumnorder.Items.Count)
                                CheckBox_column.Checked = true;
                            else
                                CheckBox_column.Checked = false;
                        }
                    }
                }
            }
        }
        catch { }
    }

    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    else
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                }
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    if (cblcolumnorder.Items[i].Enabled == true)
                        cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(cblcolumnorder.Items[index].Text.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Text.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnorder.Visible = true;
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                else
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
            }
            if (ItemList.Count == 14)
                CheckBox_column.Checked = true;
            if (ItemList.Count == 0)
                lnk_columnorder.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    //added by sudhagar 1703.2017
    protected bool getCount()
    {
        bool check = false;
        try
        {
            int cnt = 0;
            FpSpread1.SaveChanges();
            for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
            {
                int value = 0;
                int.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Value), out value);
                if (value == 1)
                    cnt++;
            }
            if (cnt == 1)
                check = true;
        }
        catch { }
        return check;
    }
    protected void btnrcptdupl_Click(object sender, EventArgs e)
    {
        try
        {
            if (getCount())
            {
                bool check = false;
                FpSpread1.SaveChanges();
                for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    int value = 0;
                    int.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Value), out value);
                    if (value == 1)
                    {
                        double paidAmount = 0;
                        string appno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                        string challNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                        //  string feecat = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 10].Text);
                        if (!string.IsNullOrEmpty(appno) && !string.IsNullOrEmpty(challNo))
                        {
                            string selQ = " select sum(debit) as debit,convert(varchar(10),transdate,103) as transdate,transdate,transcode,app_no,ddno,dddate,feecategory from ft_findailytransaction where app_no='" + appno + "' and paymode='4' and ddno='" + challNo + "' and isnull(ddno,'')<>'' group by transdate,transcode,app_no,ddno,dddate,feecategory";
                            selQ += " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode1 + "'";
                            DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                            {
                                string rcptno = Convert.ToString(dsval.Tables[0].Rows[0]["transcode"]);
                                string rcptDt = Convert.ToString(dsval.Tables[0].Rows[0]["transdate"]);
                                string feecat = Convert.ToString(dsval.Tables[0].Rows[0]["feecategory"]);
                                string TextName = string.Empty;
                                if (dsval.Tables[1].Rows.Count > 0)
                                {
                                    dsval.Tables[1].DefaultView.RowFilter = "TextCode='" + feecat + "'";
                                    DataView Dview = dsval.Tables[1].DefaultView;
                                    if (Dview.Count > 0)
                                        TextName = Convert.ToString(Dview[0]["TextVal"]);
                                }
                                double.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["debit"]), out paidAmount);
                                getDuplicateReceipt(rcptno, rcptDt, appno, paidAmount, TextName);
                            }
                            else
                                check = true;
                        }
                    }
                }
                if (check)
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "Confirm Challan Only Allows to Take Duplicate!";
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "Please Select Any One";
            }
        }
        catch { }
    }
    protected void getDuplicateReceipt(string receiptno, string rcptDt, string appno, double paidAmount, string feecat)
    {
        try
        {
            PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.InCentimeters(15.2, 20.2));
            Font Fontboldhead = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font FontNorm = new Font("Book Antiqua", 12, FontStyle.Regular);
            Font FontTableHead = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font FontTable = new Font("Book Antiqua", 12, FontStyle.Regular);
            bool createPDF = false;

            contentDiv.InnerHtml = "";
            StringBuilder sbHtml = new StringBuilder();

            string studname = string.Empty;
            string rollno = string.Empty;
            string deg = string.Empty;
            string curYr = string.Empty;
            string rcptacr = string.Empty;
            // string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string finYearid = Convert.ToString(ddlfinyear.SelectedValue);
            try
            {
                string query = "select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.dept_acronym, dt.Dept_Name,C.type,a.app_no,r.Current_Semester   from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code and r.app_no='" + appno + "'";
                ds1 = d2.select_method_wo_parameter(query, "Text");
                string app_no = string.Empty;
                if (ds1.Tables.Count > 0)
                {
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        studname = Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]);
                        rollno = Convert.ToString(ds1.Tables[0].Rows[0]["Roll_no"]);
                        deg = Convert.ToString(ds1.Tables[0].Rows[0]["Course_Name"]) + "-" + Convert.ToString(ds1.Tables[0].Rows[0]["dept_acronym"]);
                        app_no = Convert.ToString(ds1.Tables[0].Rows[0]["app_no"]).Trim();
                        curYr = romanLetter(returnYearforSem(Convert.ToString(ds1.Tables[0].Rows[0]["Current_Semester"]))) + " Year ";
                    }
                }
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "challanreceiptduplicate"); }
            PdfPage rcptpage = recptDoc.NewPage();
            sbHtml.Append("<div style='padding-left:5px;height: 900px; width:595px;'><center><div style='padding-left:130px;height: 763.5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 574.5px; ' class='classBold12'><tr><td>");
            PdfTextArea dateText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 350, 110, 50, 20), ContentAlignment.MiddleLeft, rcptDt);
            rcptpage.Add(dateText);
            PdfTextArea rcptNoText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 55, 120, 200, 20), ContentAlignment.MiddleLeft, "Receipt No. " + receiptno);
            rcptpage.Add(rcptNoText);


            PdfTable tableparts = recptDoc.NewTable(FontTableHead, 3, 1, 7);
            tableparts.VisibleHeaders = false;
            tableparts.Cell(0, 0).SetContent(studname.ToUpper());
            tableparts.Cell(0, 0).SetFont(FontTableHead);
            tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

            tableparts.Cell(1, 0).SetContent(rollno.ToUpper());
            tableparts.Cell(1, 0).SetFont(FontTableHead);
            tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

            tableparts.Cell(2, 0).SetContent(deg.ToUpper());
            tableparts.Cell(2, 0).SetFont(FontTableHead);
            tableparts.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

            PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, 150, 135, 300, 200));
            rcptpage.Add(addtabletopage1);
            double total = 0;
            int rows = 0;
            bool AddYearOk = true;
            double paidAmt = paidAmount;
            if (paidAmt > 0)
            {
                if (AddYearOk)
                {
                    try
                    {
                        deg = romanLetter(feecat.Split(' ')[0]) + " " + feecat.Split(' ')[1] + " " + deg;
                    }
                    catch { deg = curYr + deg; }
                    AddYearOk = false;
                }
                rows++;
            }
            sbHtml.Append("<table class='classBold12' style='width:460px; height:60px;' cellpadding='7'><tr><td style='padding-left:260px; padding-top:70px; text-align:right;'><BR>" + rcptDt + "</td></tr><tr><td style='padding-left:0px;'>Receipt No. " + receiptno + "</td></tr><tr><td style='padding-left:150px;padding-top:-650px;'>" + studname.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + rollno.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + deg.ToUpper() + "</td></tr></table>");

            PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, 3, 5);
            tableparts1.VisibleHeaders = false;
            tableparts1.Columns[0].SetWidth(204);
            tableparts1.Columns[1].SetWidth(62);
            tableparts1.Columns[2].SetWidth(28);
            sbHtml.Append("<div style='width:380px; height:215px; padding-left:80px; padding-top:80px;'><table class='classBold12' cellpadding='4' >");
            int indx = 0;
            total += paidAmt;
            if (paidAmt > 0)
            {
                createPDF = true;
                tableparts1.Cell(indx, 1).SetContent(returnIntegerPart(paidAmt));
                tableparts1.Cell(indx, 1).SetFont(FontTable);
                tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                tableparts1.Cell(indx, 2).SetContent(returnDecimalPart(paidAmt));
                tableparts1.Cell(indx, 2).SetFont(FontTable);
                tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                indx++;
                sbHtml.Append("<tr><td style='padding-left:290px; text-align:right; width:60px;'>" + returnIntegerPart(paidAmt) + "</td><td style=' text-align:right; width:30px;'>&nbsp;&nbsp;&nbsp;&nbsp;" + returnDecimalPart(paidAmt) + "</td></tr>");

                #region update ReceiptNumber
                // string upQ = " update ft_findailytransaction set TransCode='" + receiptno + "' where app_no='" + appno + "' and ledgerfk='" + lbllgrid.Text + "' and Headerfk='" + lblhdrid.Text + "' and Feecategory='" + lblfeecat.Text + "'   and isnull(Iscanceled,0)=0";
                //  d2.update_method_wo_parameter(upQ, "Text");
                #endregion
            }
            sbHtml.Append("</table></div>");
            sbHtml.Append("<div style='height:60px;' ><table class='classBold12' style='width:380px; padding-left:50px; padding-top:10px;' cellpadding='5'><tr><td style='padding-left:5px;padding-right:70px;'>" + DecimalToWords((decimal)total) + " Rupees Only.</td></tr><tr><td style='padding-left:280px;text-align:right;'><span style='padding-left:50px;text-align:right;width:60px;'>" + returnIntegerPart(total) + "</span></td><td style=' text-align:right; width:30px;padding-left:25px;'>&nbsp;&nbsp;" + returnDecimalPart(total) + "</td></tr></table></div>");


            PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 45, 232, 346, 218));
            rcptpage.Add(addtabletopage2);

            PdfTextArea amtWords = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 50, 400, 250, 100), ContentAlignment.MiddleLeft, DecimalToWords((decimal)total) + " Rupees Only.");
            rcptpage.Add(amtWords);

            PdfTable tableparts3 = recptDoc.NewTable(FontTable, 1, 3, 5);
            tableparts3.VisibleHeaders = false;

            tableparts3.Columns[0].SetWidth(204);
            tableparts3.Columns[1].SetWidth(62);
            tableparts3.Columns[2].SetWidth(28);

            tableparts3.Cell(0, 0).SetContent(" ");

            tableparts3.Cell(0, 1).SetContent(returnIntegerPart(total));
            tableparts3.Cell(0, 1).SetFont(FontTable);
            tableparts3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

            tableparts3.Cell(0, 2).SetContent(returnDecimalPart(total));
            tableparts3.Cell(0, 2).SetFont(FontTable);
            tableparts3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

            PdfTablePage addtabletopage3 = tableparts3.CreateTablePage(new PdfArea(recptDoc, 45, 450, 346, 28));
            rcptpage.Add(addtabletopage3);


            //sbHtml.Append("</td></tr></table></div>");
            sbHtml.Append("</td></tr></table></div></center></div>");
            contentDiv.InnerHtml += sbHtml.ToString();
            rcptpage.SaveToDocument();
            if (createPDF)
            {
                #region New Print
                contentDiv.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
                #endregion
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "No Ledgers Available To Print";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "challanreceiptduplicate");
        }
    }
    protected void getDuplicateVisible()
    {
        double linkVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction(" select linkvalue from New_InsSettings where LinkName='ChallanReceipt' and user_code ='" + usercode + "' and college_code ='" + ddl_college.SelectedValue + "'")), out linkVal);
        if (linkVal == 1)
        {
            btnrcptdupl.Visible = true;
            tblBtns.Visible = true;
        }
        else
        {
            btnrcptdupl.Visible = false;
            tblBtns.Visible = false;
        }

    }
    //added by sudhagar school challan format 28.06.2017
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }

    protected string getfltrValues()
    {
        string getValues = string.Empty;
        try
        {
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            if (!string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(degree))
            {
                getValues = " and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "')";
            }
        }
        catch { getValues = string.Empty; }
        return getValues;
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

    #region financial year
    public void loadfinanceyear()
    {
        try
        {
            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + ddl_college.SelectedValue + "'  order by FinYearPK desc";
            ds.Dispose();
            ds.Reset();
            ddlfinyear.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
                    string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    ddlfinyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
                }
                if (!string.IsNullOrEmpty(finYearid))
                {
                    ddlfinyear.SelectedIndex = ddlfinyear.Items.IndexOf(ddlfinyear.Items.FindByValue(finYearid));
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlfinyear_Selected(object sender, EventArgs e)
    {
        loadChlAcronym();
    }
    #endregion
}