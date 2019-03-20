using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using System.Configuration;



public partial class ConsessionMaster : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;

    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    Boolean Cellclick = false;
    bool check = false;
    Boolean flag_true = false;
    int commcount;
    int i;
    int cout;
    int row;

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
            //  collegecode1 = Session["collegecode"].ToString();
            sessstream = Convert.ToString(Session["streamcode"]);
            lbl_str.Text = sessstream;

            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddl_collegename.Items.Count > 0)
                {
                    collegecode1 = ddl_collegename.SelectedItem.Value.ToString();
                    collegecode = ddl_collegename.SelectedItem.Value.ToString();
                }
                bindstream();
                bindedu_lvl();
                Bindcourse();
                binddept();
                LoadYearSemester();
                bindheader();
                ledgerbind();
                loaddesc();
                rb_concession.Checked = true;
                rb_amt.Checked = true;
                chkdept_OnCheckedChanged(sender, e);
                btn_go_Click(sender, e);
                loadfinanceyear();
            }
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = ddl_collegename.SelectedItem.Value.ToString();
                collegecode = ddl_collegename.SelectedItem.Value.ToString();
            }
        }
        catch (Exception ex) { }
    }

    public void loadcollege()
    {
        try
        {
            ddl_collegename.Items.Clear();
            ds.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
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
        {
        }
    }
    public void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_collegename.Items.Count > 0)
        {
            collegecode1 = ddl_collegename.SelectedItem.Value.ToString();
        }
        bindstream();
        bindedu_lvl();
        LoadYearSemester();
        bindheader();
        ledgerbind();
        loaddesc();
        loadfinanceyear();
    }
    public void loadreason()
    {
        try
        {
            ddl_enroll.Items.Clear();
            ddl_enroll.Items.Add(new ListItem("Enrolled", "1"));
            ddl_enroll.Items.Add(new ListItem("Not Enrolled", "2"));
        }
        catch
        {
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
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(FpSpread1, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
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

        }
        catch
        {

        }

    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void rb_concession_CheckedChanged(object sender, EventArgs e)
    {
        bindstream();
        bindheader();
        ledgerbind();
        btn_minus.Visible = true;
        btn_plus.Visible = true;
        loaddesc();
        bindedu_lvl();
        LoadYearSemester();
        btn_save.Visible = false;
        btn_reset.Visible = false;
        FpSpread1.Visible = false;
        rb_amt.Checked = true;
        lbl_reas.Visible = false;
        btn_minus.Visible = false;
        ddl_reason.Visible = false;
        btn_plus.Visible = false;
        btn_go_Click(sender, e);
    }
    protected void rb_refund_CheckedChanged(object sender, EventArgs e)
    {
        bindstream();
        bindheader();
        ledgerbind();
        loadreason();
        bindedu_lvl();
        LoadYearSemester();
        btn_reset.Visible = false;
        Divspread.Visible = false;
        FpSpread1.Visible = false;
        btn_save.Visible = false;
        rb_amt.Checked = true;
        lbl_reas.Visible = false;
        btn_minus.Visible = false;
        ddl_reason.Visible = false;
        btn_plus.Visible = false;
        btn_go_Click(sender, e);
    }

    #region stream
    public void bindstream()
    {
        try
        {
            cbl_stream.Items.Clear();
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + ddl_collegename.SelectedItem.Value + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stream.DataSource = ds;
                cbl_stream.DataTextField = "type";
                cbl_stream.DataBind();

                if (cbl_stream.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stream.Items.Count; i++)
                    {
                        cbl_stream.Items[i].Selected = true;
                    }
                    if (lbl_str.Text == "Stream")
                    {
                        txt_stream.Text = "Stream(" + cbl_stream.Items.Count + ")";
                    }
                    if (lbl_str.Text == "Shift")
                    {
                        txt_stream.Text = "Shift(" + cbl_stream.Items.Count + ")";
                    }
                    cb_stream.Checked = true;
                    txt_stream.Enabled = true;
                }
            }
            else
            {
                txt_stream.Text = "--Select--";
                txt_stream.Enabled = false;

            }
        }
        catch
        {
        }

    }
    protected void cb_stream_CheckedChanged(object sender, EventArgs e)
    {
        string stream = "";

        if (cb_stream.Checked == true)
        {
            for (int i = 0; i < cbl_stream.Items.Count; i++)
            {
                cbl_stream.Items[i].Selected = true;
                stream = Convert.ToString(cbl_stream.Items[i].Text);
            }
            if (lbl_str.Text == "Stream")
            {
                if (cbl_stream.Items.Count == 1)
                {
                    txt_stream.Text = "" + stream + "";
                }
                else
                {
                    txt_stream.Text = "Stream(" + (cbl_stream.Items.Count) + ")";
                }
            }

            if (lbl_str.Text == "Shift")
            {
                if (cbl_stream.Items.Count == 1)
                {
                    txt_stream.Text = "" + stream + "";
                }
                else
                {
                    txt_stream.Text = "Shift(" + (cbl_stream.Items.Count) + ")";
                }
                // txt_stream.Text = "Shift(" + (cbl_stream.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbl_stream.Items.Count; i++)
            {
                cbl_stream.Items[i].Selected = false;
            }
            txt_stream.Text = "--Select--";
        }
        bindedu_lvl();
        Bindcourse();
        binddept();
    }
    protected void cbl_stream_SelectedIndexChanged(object sender, EventArgs e)
    {
        string stream = "";
        txt_stream.Text = "--Select--";
        cb_stream.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_stream.Items.Count; i++)
        {
            if (cbl_stream.Items[i].Selected == true)
            {

                commcount = commcount + 1;
                stream = Convert.ToString(cbl_stream.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            if (lbl_str.Text == "Shift")
            {
                if (commcount == 1)
                {
                    txt_stream.Text = "" + stream + "";
                }
                else
                {
                    txt_stream.Text = "Shift(" + commcount.ToString() + ")";
                }

                // txt_stream.Text = "Shift(" + commcount.ToString() + ")";
            }
            if (lbl_str.Text == "Stream")
            {
                if (commcount == 1)
                {
                    txt_stream.Text = "" + stream + "";
                }
                else
                {
                    txt_stream.Text = "Stream(" + commcount.ToString() + ")";
                }
                // txt_stream.Text = "Stream(" + commcount.ToString() + ")";
            }
            if (commcount == cbl_stream.Items.Count)
            {
                cb_stream.Checked = true;
            }

        }
        bindedu_lvl();
        Bindcourse();
        binddept();
    }
    #endregion

    #region ddl reason
    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnplus_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Reasons";
        lblerror.Visible = false;
    }
    protected void btnminus_Click(object sender, EventArgs e)
    {
        if (ddl_reason.SelectedIndex == -1)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "No records found";
        }
        else if (ddl_reason.SelectedIndex == 0)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Select any record";
        }
        else if (ddl_reason.SelectedIndex != 0)
        {
            alertdel.Visible = true;
            btn_del.Visible = true;
            lbl_del.Text = "Do You Want Delete The Record";

            //string sql = "delete from textvaltable where TextCode='" + ddl_reason.SelectedItem.Value.ToString() + "' and TextCriteria='DedRe' and college_code='" + collegecode1 + "' ";
            //int delete = d2.update_method_wo_parameter(sql, "TEXT");
            //if (delete != 0)
            //{
            //    alertpopwindow.Visible = true;
            //    lblalerterr.Text = "Deleted Sucessfully";
            //}
            //else
            //{
            //    alertpopwindow.Visible = true;
            //    lblalerterr.Text = "No records found";
            //}
            //loaddesc();
        }

        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "No records found";
        }
    }
    public void btn_del_Click(object sender, EventArgs e)
    {
        if (ddl_reason.SelectedIndex != 0)
        {
            string sql = "delete from textvaltable where TextCode='" + ddl_reason.SelectedItem.Value.ToString() + "' and TextCriteria='DedRe' and college_code='" + collegecode1 + "' ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {

                alertdel.Visible = true;
                btn_del.Visible = false;
                btn_ok.Visible = true;
                lbl_del.Text = "Deleted Sucessfully";


            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No records found";
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
        string college = ddl_collegename.SelectedItem.Value.ToString();
        string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + college + "'";
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
                string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_addgroup.Text + "' and TextCriteria ='DedRe' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txt_addgroup.Text + "' where TextVal ='" + txt_addgroup.Text + "' and TextCriteria ='DedRe' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_addgroup.Text + "','DedRe','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Saved sucessfully";
                    txt_addgroup.Text = "";
                    plusdiv.Visible = false;
                    panel_addgroup.Visible = false;
                }
                loaddesc();
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Enter the description";
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

    #region header and ledger

    public void bindheader()
    {
        try
        {
            cbl_header.Items.Clear();
            string college = ddl_collegename.SelectedItem.Value.ToString();
            string query = "SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + college + "   ";
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
                txt_header.Text = "Header(" + cbl_header.Items.Count + ")";
                cb_header.Checked = true;
            }


        }
        catch { }
    }
    public void ledgerbind()
    {
        try
        {
            cbl_ledger.Items.Clear();
            txt_ledger.Text = "---Select---";
            cb_ledger.Checked = false;
            string itemheadercode = "";
            for (int i = 0; i < cbl_header.Items.Count; i++)
            {
                if (cbl_header.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_header.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "" + "," + "" + cbl_header.Items[i].Value.ToString() + "";
                    }
                }
            }
            string college = ddl_collegename.SelectedItem.Value.ToString();

            string query = "SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND L.CollegeCode = " + college + " and L.HeaderFK in (" + itemheadercode + ") and LedgerMode=0  order by isnull(l.priority,1000), l.ledgerName asc ";

            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_ledger.DataSource = ds;
                cbl_ledger.DataTextField = "LedgerName";
                cbl_ledger.DataValueField = "LedgerPK";
                cbl_ledger.DataBind();
                for (int i = 0; i < cbl_ledger.Items.Count; i++)
                {
                    cbl_ledger.Items[i].Selected = true;
                }
                txt_ledger.Text = "Ledger(" + cbl_ledger.Items.Count + ")";
                cb_ledger.Checked = true;
            }
        }
        catch
        {
        }
    }
    protected void cb_header_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string headername = "";
            txt_header.Text = "--Select--";
            if (cb_header.Checked == true)
            {

                for (i = 0; i < cbl_header.Items.Count; i++)
                {
                    cbl_header.Items[i].Selected = true;
                    headername = Convert.ToString(cbl_header.Items[i].Text);
                }
                if (cbl_header.Items.Count == 1)
                {
                    txt_header.Text = "" + headername + "";
                }
                else
                {
                    txt_header.Text = "Header(" + (cbl_header.Items.Count) + ")";
                }
                //  txt_header.Text = "Header(" + (cbl_header.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_header.Items.Count; i++)
                {
                    cbl_header.Items[i].Selected = false;
                }
            }
            ledgerbind();
        }
        catch { }
    }
    protected void cbl_header_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string headername = "";
            i = 0;
            cb_header.Checked = false;
            commcount = 0;
            txt_header.Text = "--Select--";
            for (i = 0; i < cbl_header.Items.Count; i++)
            {
                if (cbl_header.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    headername = Convert.ToString(cbl_header.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_header.Items.Count)
                {
                    cb_header.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_header.Text = "" + headername + "";
                }
                else
                {
                    txt_header.Text = "Header(" + commcount.ToString() + ")";
                }
                // txt_header.Text = "Header(" + commcount.ToString() + ")";
            }
            ledgerbind();
        }
        catch { }
    }
    protected void cb_ledger_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string ledger = "";
            txt_ledger.Text = "--Select--";
            if (cb_ledger.Checked == true)
            {

                for (i = 0; i < cbl_ledger.Items.Count; i++)
                {
                    cbl_ledger.Items[i].Selected = true;
                    ledger = Convert.ToString(cbl_header.Items[i].Text);
                }
                if (cbl_ledger.Items.Count == 1)
                {
                    txt_ledger.Text = "" + ledger + "";
                }
                else
                {
                    txt_ledger.Text = "Ledger(" + (cbl_ledger.Items.Count) + ")";
                }
                //  txt_ledger.Text = "Ledger(" + (cbl_ledger.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_ledger.Items.Count; i++)
                {
                    cbl_ledger.Items[i].Selected = false;
                }
            }

        }
        catch { }
    }
    protected void cbl_ledger_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string ledger = "";
            i = 0;
            cb_ledger.Checked = false;
            commcount = 0;
            txt_ledger.Text = "--Select--";
            for (i = 0; i < cbl_ledger.Items.Count; i++)
            {
                if (cbl_ledger.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    ledger = Convert.ToString(cbl_ledger.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_ledger.Items.Count)
                {
                    cb_ledger.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_ledger.Text = "" + ledger + "";
                }
                else
                {
                    txt_ledger.Text = "Ledger(" + commcount.ToString() + ")";
                }
                //txt_ledger.Text = "Ledger(" + commcount.ToString() + ")";
            }

        }
        catch { }

    }

    #endregion

    #region bind education level
    public void bindedu_lvl()
    {
        try
        {
            ds.Clear();
            cbl_edulevel.Items.Clear();
            string edulevel = "";

            string itemheader = "";
            for (int i = 0; i < cbl_stream.Items.Count; i++)
            {
                if (cbl_stream.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_stream.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_stream.Items[i].Value.ToString() + "";
                    }
                }
            }
            string deptquery = "";
            if (itemheader.Trim() != "")
            {
                deptquery = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and type in ('" + itemheader + "') and college_code in ('" + collegecode1 + "')";
            }
            else
            {
                deptquery = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and college_code in ('" + collegecode1 + "')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_edulevel.DataSource = ds;
                cbl_edulevel.DataTextField = "Edu_Level";
                cbl_edulevel.DataBind();
                if (cbl_edulevel.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_edulevel.Items.Count; i++)
                    {
                        cbl_edulevel.Items[i].Selected = true;
                        edulevel = Convert.ToString(cbl_edulevel.Items[i].Text);
                    }
                    if (cbl_edulevel.Items.Count == 1)
                    {
                        txt_edulevel.Text = "" + edulevel + "";
                    }
                    else
                    {
                        txt_edulevel.Text = "Education Level(" + cbl_edulevel.Items.Count + ")";
                    }
                    cb_edulevel.Checked = true;
                }
            }
            else
            {
                txt_edulevel.Text = "--Select--";
                cb_edulevel.Checked = false;
            }

            //}
            //else
            //{
            //    txt_edulevel.Text = "--Select--";
            //    cb_edulevel.Checked = false;
            //}
        }
        catch
        {
        }
    }

    protected void cb_edulevel_CheckedChanged(object sender, EventArgs e)
    {
        string edulevel = "";
        if (cb_edulevel.Checked == true)
        {
            for (int i = 0; i < cbl_edulevel.Items.Count; i++)
            {
                cbl_edulevel.Items[i].Selected = true;
                edulevel = Convert.ToString(cbl_edulevel.Items[i].Text);
            }
            if (cbl_edulevel.Items.Count == 1)
            {
                txt_edulevel.Text = "" + edulevel + "";
            }
            else
            {
                txt_edulevel.Text = "Education Level(" + (cbl_edulevel.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbl_edulevel.Items.Count; i++)
            {
                cbl_edulevel.Items[i].Selected = false;
            }
            txt_edulevel.Text = "--Select--";
        }
        Bindcourse();
        binddept();
    }

    protected void cbl_edulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_edulevel.Text = "--Select--";
        cb_edulevel.Checked = false;
        string edulevel = "";
        int commcount = 0;
        for (int i = 0; i < cbl_edulevel.Items.Count; i++)
        {
            if (cbl_edulevel.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                edulevel = Convert.ToString(cbl_edulevel.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_edulevel.Items.Count)
            {
                cb_edulevel.Checked = true;
            }
            if (commcount == 1)
            {
                txt_edulevel.Text = "" + edulevel + "";
            }
            else
            {
                txt_edulevel.Text = "Education Level(" + commcount.ToString() + ")";
            }
        }
        Bindcourse();
        binddept();
    }
    #endregion

    #region course
    public void cb_course_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_course.Text = "--Select--";
            string coursenmae = "";
            if (cb_course.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_course.Items.Count; i++)
                {
                    cbl_course.Items[i].Selected = true;
                    coursenmae = Convert.ToString(cbl_course.Items[i].Text);
                }
                if (cbl_course.Items.Count == 1)
                {
                    txt_course.Text = "" + coursenmae + "";
                }
                else
                {
                    txt_course.Text = lbldeg.Text + "(" + (cbl_course.Items.Count) + ")";
                }
                // txt_course.Text = "Course(" + (cbl_course.Items.Count) + ")";

            }


            else
            {
                for (int i = 0; i < cbl_course.Items.Count; i++)
                {
                    cbl_course.Items[i].Selected = false;

                }
                txt_course.Text = "--Select--";
            }


            binddept();

        }
        catch (Exception ex)
        {
        }

    }

    public void cbl_course_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string coursename = "";
            int i = 0;
            int commcount = 0;
            cb_course.Checked = false;
            txt_course.Text = "--Select--";
            for (i = 0; i < cbl_course.Items.Count; i++)
            {
                if (cbl_course.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    coursename = Convert.ToString(cbl_course.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_course.Items.Count)
                {
                    cb_course.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_course.Text = "" + coursename + "";
                }
                else
                {
                    txt_course.Text = lbldeg.Text + "(" + commcount.ToString() + ")";
                }
                //txt_course.Text = "Course (" + commcount.ToString() + ")";
            }
            binddept();
        }
        catch (Exception ex)
        {

        }
    }

    public void Bindcourse()
    {
        try
        {
            cbl_course.Items.Clear();
            string build = "";
            string build1 = "";
            if (cbl_stream.Items.Count > 0)
            {
                for (int i = 0; i < cbl_stream.Items.Count; i++)
                {
                    if (cbl_stream.Items[i].Selected == true)
                    {
                        if (build1 == "")
                        {
                            build1 = Convert.ToString(cbl_stream.Items[i].Value);
                        }
                        else
                        {
                            build1 = build1 + "'" + "," + "'" + Convert.ToString(cbl_stream.Items[i].Value);
                        }
                    }
                }
            }
            if (cbl_edulevel.Items.Count > 0)
            {
                for (int i = 0; i < cbl_edulevel.Items.Count; i++)
                {
                    if (cbl_edulevel.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_edulevel.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_edulevel.Items[i].Value);
                        }
                    }
                }
            }
            if (build != "")
            {
                string deptquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + collegecode1 + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
                if (build1.Trim() != "")
                {
                    deptquery = deptquery + " and type in ('" + build1 + "')";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_course.DataSource = ds;
                    cbl_course.DataTextField = "course_name";
                    cbl_course.DataValueField = "course_id";
                    cbl_course.DataBind();
                    if (cbl_course.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_course.Items.Count; row++)
                        {
                            cbl_course.Items[row].Selected = true;
                        }
                        cb_course.Checked = true;
                        txt_course.Text = lbldeg.Text + "(" + cbl_course.Items.Count + ")";
                    }

                }
            }
            else
            {
                cb_course.Checked = false;
                txt_course.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    #region dept
    public void cb_dept_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string deptname = "";
            int cout = 0;
            txt_dept.Text = "--Select--";
            if (cb_dept.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                    deptname = Convert.ToString(cbl_dept.Items[i].Text);
                }
                if (cbl_dept.Items.Count == 1)
                {
                    txt_dept.Text = "" + deptname + "";
                }
                else
                {
                    txt_dept.Text = lbldept.Text + "(" + (cbl_dept.Items.Count) + ")";
                }
                //  txt_dept.Text = "Department(" + (cbl_dept.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = false;
                }
                txt_dept.Text = "--Select--";
            }



        }
        catch (Exception ex)
        {

        }
    }

    public void cbl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string deptname = "";
            int commcount = 0;
            cb_dept.Checked = false;
            txt_dept.Text = "--Select--";

            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    deptname = Convert.ToString(cbl_dept.Items[i].Text);

                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_dept.Items.Count)
                {

                    cb_dept.Checked = true;

                }
                if (commcount == 1)
                {
                    txt_dept.Text = "" + deptname + "";
                }
                else
                {
                    txt_dept.Text = lbldept.Text + "(" + commcount.ToString() + ")";
                }
                // txt_dept.Text = "Department(" + commcount.ToString() + ")";

            }

        }
        catch (Exception ex)
        {

        }
    }

    public void binddept()
    {
        try
        {

            cbl_dept.Items.Clear();
            string build = "";
            string build1 = "";
            string build2 = "";
            if (cbl_stream.Items.Count > 0)
            {
                for (int i = 0; i < cbl_stream.Items.Count; i++)
                {
                    if (cbl_stream.Items[i].Selected == true)
                    {
                        if (build1 == "")
                        {
                            build1 = Convert.ToString(cbl_stream.Items[i].Value);
                        }
                        else
                        {
                            build1 = build1 + "'" + "," + "'" + Convert.ToString(cbl_stream.Items[i].Value);
                        }
                    }
                }
            }
            if (cbl_edulevel.Items.Count > 0)
            {
                for (int i = 0; i < cbl_edulevel.Items.Count; i++)
                {
                    if (cbl_edulevel.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_edulevel.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_edulevel.Items[i].Value);
                        }
                    }
                }
            }
            if (cbl_course.Items.Count > 0)
            {
                for (int i = 0; i < cbl_course.Items.Count; i++)
                {
                    if (cbl_course.Items[i].Selected == true)
                    {
                        if (build2 == "")
                        {
                            build2 = Convert.ToString(cbl_course.Items[i].Value);
                        }
                        else
                        {
                            //build2 = build2 + "'" + "," + "'" + Convert.ToString(cbl_course.Items[i].Value);
                            build2 += "," + Convert.ToString(cbl_course.Items[i].Value);
                        }
                    }
                }
            }
            if (build != "" && build2 != "")
            {
                //  string deptquery = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and  department .dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + build2 + "') and degree.college_code in ('" + collegecode1 + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
                //  ds.Clear();
                //  ds = d2.select_method_wo_parameter(deptquery, "Text");
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, build2, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_dept.Items.Count; row++)
                        {
                            cbl_dept.Items[row].Selected = true;
                        }
                        cb_dept.Checked = true;
                        txt_dept.Text = lbldept.Text + "(" + cbl_dept.Items.Count + ")";
                    }

                }
            }
            else
            {
                cb_dept.Checked = false;
                txt_dept.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }
    }
    #endregion

    #region semester
    public void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            string sem = "";
            if (cb_sem.Checked == true)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                    sem = Convert.ToString(cbl_sem.Items[i].Text);
                }
                if (cbl_sem.Items.Count == 1)
                {
                    txt_sem.Text = "" + sem + "";
                }
                else
                {
                    txt_sem.Text = lblsem.Text + "(" + (cbl_sem.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
                txt_sem.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_sem.Text = "--Select--";
            string sem = "";
            cb_sem.Checked = false;
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    sem = Convert.ToString(cbl_sem.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {
                    cb_sem.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_sem.Text = "" + sem + "";
                }
                else
                {
                    txt_sem.Text =lblsem.Text+ "(" + commcount.ToString() + ")";
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void LoadYearSemester()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = d2.loadFeecategory(Convert.ToString(ddl_collegename.SelectedItem.Value), usercode, ref linkName);
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


    //protected void LoadYearSemester()
    //{
    //    try
    //    {
    //        string sem = "";
    //        string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
    //        string semyear = "select * from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "'";
    //        DataSet dsset = new DataSet();
    //        dsset.Clear();
    //        dsset = d2.select_method_wo_parameter(semyear, "Text");
    //        if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
    //        {
    //            string value = Convert.ToString(dsset.Tables[0].Rows[0]["LinkValue"]);
    //            if (value == "1")
    //            {
    //                string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(SelectQ, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                    cbl_sem.DataSource = ds;
    //                    cbl_sem.DataTextField = "TextVal";
    //                    cbl_sem.DataValueField = "TextCode";
    //                    cbl_sem.DataBind();
    //                }
    //                if (cbl_sem.Items.Count > 0)
    //                {
    //                    for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                    {
    //                        cbl_sem.Items[i].Selected = true;
    //                        sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                    }
    //                    if (cbl_sem.Items.Count == 1)
    //                    {
    //                        txt_sem.Text = "SemesterandYear(" + sem + ")";
    //                    }
    //                    else
    //                    {
    //                        txt_sem.Text = "SemesterandYear(" + cbl_sem.Items.Count + ")";
    //                    }
    //                    cb_sem.Checked = true;
    //                }

    //            }
    //            else
    //            {
    //                cbl_sem.Items.Clear();
    //                string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "'";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(settingquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //                    if (linkvalue == "0")
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            cbl_sem.DataSource = ds;
    //                            cbl_sem.DataTextField = "TextVal";
    //                            cbl_sem.DataValueField = "TextCode";
    //                            cbl_sem.DataBind();
    //                        }
    //                        if (cbl_sem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                            {
    //                                cbl_sem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                            }
    //                            if (cbl_sem.Items.Count == 1)
    //                            {
    //                                txt_sem.Text = "Semester(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
    //                            }
    //                            cb_sem.Checked = true;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            // text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            cbl_sem.DataSource = ds;
    //                            cbl_sem.DataTextField = "TextVal";
    //                            cbl_sem.DataValueField = "TextCode";
    //                            cbl_sem.DataBind();
    //                        }
    //                        if (cbl_sem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                            {
    //                                cbl_sem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                            }
    //                            if (cbl_sem.Items.Count == 1)
    //                            {
    //                                txt_sem.Text = "Year(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txt_sem.Text = "Year(" + cbl_sem.Items.Count + ")";
    //                            }
    //                            cb_sem.Checked = true;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //}
    #endregion

    #region button go
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            #region getvalue
            string itemheadercode = "";
            string itemledgercode = "";
            string streamcode = "";
            string edulvl = "";
            string sem = "";
            double divheight = 0;

            for (int i = 0; i < cbl_header.Items.Count; i++)
            {
                if (cbl_header.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_header.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_header.Items[i].Value.ToString() + "";
                    }
                }
            }

            for (int i = 0; i < cbl_ledger.Items.Count; i++)
            {
                if (cbl_ledger.Items[i].Selected == true)
                {
                    if (itemledgercode == "")
                    {
                        itemledgercode = "" + cbl_ledger.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemledgercode = itemledgercode + "'" + "," + "'" + cbl_ledger.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (int j = 0; j < cbl_stream.Items.Count; j++)
            {
                if (cbl_stream.Items[j].Selected == true)
                {
                    if (streamcode == "")
                    {
                        streamcode = "" + cbl_stream.Items[j].Value.ToString() + "";
                    }
                    else
                    {
                        streamcode = streamcode + "'" + "," + "'" + cbl_stream.Items[j].Value.ToString() + "";
                    }
                }
            }

            for (int j = 0; j < cbl_edulevel.Items.Count; j++)
            {
                if (cbl_edulevel.Items[j].Selected == true)
                {
                    if (edulvl == "")
                    {
                        edulvl = "" + cbl_edulevel.Items[j].Value.ToString() + "";
                    }
                    else
                    {
                        edulvl = edulvl + "'" + "," + "'" + cbl_edulevel.Items[j].Value.ToString() + "";
                    }
                }
            }

            for (int jk = 0; jk < cbl_sem.Items.Count; jk++)
            {
                if (cbl_sem.Items[jk].Selected == true)
                {
                    if (sem == "")
                    {
                        sem = "" + cbl_sem.Items[jk].Value.ToString() + "";
                    }
                    else
                    {
                        sem = sem + "'" + "," + "'" + cbl_sem.Items[jk].Value.ToString() + "";
                    }
                }
            }
            string reason = ddl_reason.SelectedItem.Value.ToString();
            string finyearfk = Convert.ToString(ddlfinyear.SelectedValue);
            #endregion

            string query = "  select HeaderName,HeaderPK,LedgerPK ,LedgerName from  FM_LedgerMaster L,FM_HeaderMaster H WHERE  h.HeaderPK =l.HeaderFK and  HeaderPK in('" + itemheadercode + "') and LedgerPK in('" + itemledgercode + "') order by isnull(l.priority,1000), l.ledgerName asc";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int refmode = 0;
                if (rb_concession.Checked == true)
                {
                    refmode = 1;
                }
                else
                {
                    refmode = 2;
                }
                if (rb_concession.Checked == true)
                {
                    #region consession

                    #region design
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 4;

                    FpSpread1.Sheets[0].AutoPostBack = false;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Header";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Columns[1].Width = 280;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Ledger";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Columns[2].Width = 320;

                    if (rb_per.Checked == true)
                    {
                        FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                        db.ErrorMessage = "Enter only Valid Prercentage";
                        db.MinimumValue = 1;
                        db.MaximumValue = 100;
                        FpSpread1.Columns[3].CellType = db;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Deduction(%)";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[3].Width = 194;
                    }
                    if (rb_amt.Checked == true)
                    {
                        FarPoint.Web.Spread.DoubleCellType db1 = new FarPoint.Web.Spread.DoubleCellType();
                        db1.ErrorMessage = "Enter only Numbers";
                        db1.MinimumValue = 1;
                        // db1.MaximumValue = 1;
                        FpSpread1.Columns[3].CellType = db1;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Deduction Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[3].Width = 194;
                    }
                    #endregion

                    #region value bind
                    bool Amt = false;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string header = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                        string header_code = Convert.ToString(ds.Tables[0].Rows[i]["HeaderPK"]);

                        string led = Convert.ToString(ds.Tables[0].Rows[i]["LedgerName"]);
                        string led_code = Convert.ToString(ds.Tables[0].Rows[i]["LedgerPK"]);
                        string tot = header + '-' + led;
                        string val = header_code + ',' + led_code;
                        FpSpread1.Sheets[0].RowCount++;
                        divheight += 30;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = header;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = val;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = led;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Column.Width = 300;
                        DataSet dscon = new DataSet();
                        double conperc = 0;
                        double conamt = 0;
                        string stream = string.Empty;
                        if (!string.IsNullOrEmpty(streamcode))
                            stream = " and Stream in ('" + streamcode + "')";
                        string selqry = "  select consper,consamt from FM_ConcessionRefundSettings where HeaderfK in('" + header_code + "') and LedgerfK in('" + led_code + "') " + stream + " and Edu_Level in('" + edulvl + "')  and Fee_Category in ('" + sem + "') and RefMode ='" + refmode + "' and ConsDesc in ('" + reason + "') and finyearfk in('" + finyearfk + "') ";
                        //and ConsType='" + type + "'";
                        dscon.Clear();
                        dscon = d2.select_method_wo_parameter(selqry, "Text");
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            for (int s = 0; s < dscon.Tables[0].Rows.Count; s++)
                            {
                                double.TryParse(Convert.ToString(dscon.Tables[0].Rows[s]["ConsPer"]), out conperc);
                                double.TryParse(Convert.ToString(dscon.Tables[0].Rows[s]["ConsAmt"]), out conamt);
                                if (rb_amt.Checked == true)
                                    Amt = true;
                                else
                                    Amt = false;
                                loadspread(Amt, FpSpread1, Convert.ToInt32(FpSpread1.Sheets[0].RowCount - 1), conamt, conperc);
                            }
                        }
                        if (rb_amt.Checked == true)
                            Amt = true;
                        else
                            Amt = false;
                        loadspread(Amt, FpSpread1, Convert.ToInt32(FpSpread1.Sheets[0].RowCount - 1), conamt, conperc);
                    }
                    #endregion

                    #region visible
                    for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].Cells[i, 0].Locked = true;
                        FpSpread1.Sheets[0].Cells[i, 1].Locked = true;

                    }
                    for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Columns[i].ForeColor = Color.Black;
                    }
                    FpSpread1.ShowHeaderSelection = false;
                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                    lbl_reas.Visible = true;
                    btn_minus.Visible = true;
                    ddl_reason.Visible = true;
                    btn_plus.Visible = true;
                    btn_save.Visible = true;
                    btn_reset.Visible = true;
                    Divspread.Visible = true;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Width = 900;
                    if (150 > divheight)
                        divheight += 90;
                    FpSpread1.Height = Convert.ToInt32(divheight);
                    FpSpread1.SaveChanges();
                    FpSpread1.Visible = true;
                    #endregion

                    #endregion
                }
                else
                {
                    #region refund

                    #region design
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 6;

                    FpSpread1.Sheets[0].AutoPostBack = false;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Header";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Columns[1].Width = 300;


                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Ledger";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Columns[2].Width = 400;

                    if (rb_per.Checked == true)
                    {
                        FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                        db.ErrorMessage = "Enter only Valid Percentage";
                        db.MinimumValue = 1;
                        db.MaximumValue = 100;
                        FpSpread1.Columns[3].CellType = db;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Refund(%)";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[3].Width = 235;
                    }
                    if (rb_amt.Checked == true)
                    {
                        FarPoint.Web.Spread.DoubleCellType db1 = new FarPoint.Web.Spread.DoubleCellType();
                        db1.ErrorMessage = "Enter only Numbers";
                        db1.MinimumValue = 1;
                        //db1.MaximumValue = 6;
                        FpSpread1.Columns[3].CellType = db1;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Refund Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[3].Width = 235;
                    }

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Priority";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Priority";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    #endregion

                    #region value bind
                    bool Amt = false;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        string header = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                        string header_code = Convert.ToString(ds.Tables[0].Rows[i]["HeaderPK"]);

                        string led = Convert.ToString(ds.Tables[0].Rows[i]["LedgerName"]);
                        string led_code = Convert.ToString(ds.Tables[0].Rows[i]["LedgerPK"]);
                        string tot = header + '-' + led;
                        string val = header_code + ',' + led_code;
                        FpSpread1.Sheets[0].RowCount++;
                        divheight += 30;
                        DataSet dsview = new DataSet();



                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = header;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = val;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Column.Width = 383;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = led;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                        double dedpercent = 0;
                        double dedamt = 0;
                        double priority = 0;
                        string stream = string.Empty;
                        if (!string.IsNullOrEmpty(streamcode))
                            stream = " and Stream in ('" + streamcode + "')";
                        string selqry = "  select consper,consamt,ledpriority,RefMode from FM_ConcessionRefundSettings where HeaderfK in('" + header_code + "') and LedgerfK in('" + led_code + "') " + stream + " and Edu_Level in('" + edulvl + "')  and Fee_Category in ('" + sem + "') and RefMode ='" + refmode + "' and finyearfk in('" + finyearfk + "')";
                        //and ConsType='" + type + "'";
                        if (reason.Trim() != "0")
                        {
                            selqry = selqry + "  and ConsDesc ='" + reason + "'";
                        }
                        dsview.Clear();
                        dsview = d2.select_method_wo_parameter(selqry, "Text");
                        if (dsview.Tables[0].Rows.Count > 0)
                        {
                            for (int s = 0; s < dsview.Tables[0].Rows.Count; s++)
                            {
                                double.TryParse(Convert.ToString(dsview.Tables[0].Rows[s]["consper"]), out dedpercent);
                                double.TryParse(Convert.ToString(dsview.Tables[0].Rows[s]["consamt"]), out dedamt);
                                double.TryParse(Convert.ToString(dsview.Tables[0].Rows[s]["ledpriority"]), out priority);
                                // dedpercent = Convert.ToString(dsview.Tables[0].Rows[s]["consper"]);
                                //  dedamt = Convert.ToString(dsview.Tables[0].Rows[s]["consamt"]);
                                // priority = Convert.ToString(dsview.Tables[0].Rows[s]["ledpriority"]);
                            }
                        }

                        if (rb_amt.Checked == true)
                            Amt = true;
                        else
                            Amt = false;

                        refundloadspread(Amt, FpSpread1, FpSpread1.Sheets[0].RowCount - 1, dedamt, dedpercent, priority);
                        #region old
                        //if (rb_per.Checked == true)
                        //{
                        //    if (dedpercent != "")
                        //    {
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dedpercent;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = false;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Column.Width = 100;
                        //    }
                        //    else
                        //    {
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Column.Width = 100;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = false;
                        //    }
                        //    if (priority != "" && priority != "0")
                        //    {
                        //        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        //        cb.AutoPostBack = true;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = cb;
                        //        if (dedpercent == "0.00" && priority != "" && priority != "0")
                        //        {
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                        //        }
                        //        else
                        //        {
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Value = 1;
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                        //        }

                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        //    }
                        //    else
                        //    {
                        //        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        //        cb.AutoPostBack = true;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = cb;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = false;
                        //    }
                        //    if (priority != "" && dedpercent != "0.00")
                        //    {
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Value = priority;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                        //    }
                        //    else
                        //    {
                        //        if (priority != "" && priority != "0" && dedpercent == "0.00")
                        //        {
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                        //        }
                        //        else
                        //        {
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        //        }

                        //    }
                        //}
                        //if (rb_amt.Checked == true)
                        //{
                        //    if (dedamt != "")
                        //    {
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dedamt;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = false;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Column.Width = 100;
                        //    }
                        //    else
                        //    {
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Column.Width = 100;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = false;
                        //    }
                        //    if (priority != "" && priority != "0")
                        //    {
                        //        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        //        cb.AutoPostBack = true;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = cb;
                        //        if (dedamt == "0.00" && priority != "" && priority != "0")
                        //        {
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                        //        }
                        //        else
                        //        {
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Value = 1;
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                        //        }
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        //    }
                        //    else
                        //    {
                        //        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        //        cb.AutoPostBack = true;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = cb;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = false;
                        //    }
                        //    if (priority != "" && dedamt != "0.00")
                        //    {
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Value = priority;
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                        //    }
                        //    else
                        //    {
                        //        if (priority != "" && priority != "0" && dedamt == "0.00")
                        //        {
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                        //        }
                        //        else
                        //        {
                        //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        //        }

                        //    }
                        //}
                        #endregion
                    }
                    #endregion

                    #region visible
                    for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].Cells[i, 0].Locked = true;
                        FpSpread1.Sheets[0].Cells[i, 1].Locked = true;
                    }
                    for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Columns[i].ForeColor = Color.Black;
                    }
                    FpSpread1.ShowHeaderSelection = false;
                    lbl_reas.Visible = false;
                    Label1.Visible = true;
                    btn_minus.Visible = false;
                    ddl_enroll.Visible = true;
                    btn_plus.Visible = false;
                    btn_save.Visible = true;
                    btn_reset.Visible = true;
                    Divspread.Visible = true;
                    FpSpread1.Width = 770;
                    if (150 > divheight)
                        divheight += 90;
                    FpSpread1.Height = Convert.ToInt32(divheight);
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    FpSpread1.Visible = true;
                    #endregion

                    #endregion
                }
            }
        }
        catch
        { }
    }
    #endregion

    protected void loadspread(bool Amt, FarPoint.Web.Spread.FpSpread FpSpread1, int colcnt, double conamt, double conperc)
    {
        if (Amt)
        {
            #region amount
            if (conamt != 0 && conperc == 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Text = Convert.ToString(conamt);
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Column.Width = 100;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = false;
            }
            else if (conamt == 0 && conperc == 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].Text = "";
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Locked = false;

            }
            else if (conamt == 0 && conperc != 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Locked = true;
            }
            #endregion
        }
        else
        {
            #region
            if (conamt == 0 && conperc != 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Text = Convert.ToString(conperc);
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Column.Width = 100;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Locked = false;
            }
            else if (conamt == 0 && conperc == 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Text = "";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Locked = false;

            }
            else if (conamt != 0 && conperc == 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Locked = true;
            }
            #endregion
        }
        FpSpread1.SaveChanges();
    }

    protected void refundloadspread(bool Amt, FarPoint.Web.Spread.FpSpread FpSpread1, int colcnt, double dedamt, double dedpercent, double priority)
    {
        if (Amt)
        {
            #region amount
            if (dedamt != 0 && dedpercent == 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Text = Convert.ToString(dedamt);
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Column.Width = 100;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = false;
            }
            else if (dedamt == 0 && dedpercent == 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].Text = "";
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Locked = false;

            }
            else if (dedamt == 0 && dedpercent != 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Locked = true;
            }
            //priority
            if (priority != 0)
            {
                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                cb.AutoPostBack = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = cb;
                if (dedamt == 0)
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                else
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Value = 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                cb.AutoPostBack = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = cb;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = false;
            }
            if (priority != 0 && dedamt != 0)
            {
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Value = priority;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;
            }
            else
            {
                if (priority != 0 && dedamt == 0)
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;
                }
                else
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
            }

            #endregion
        }
        else
        {
            #region
            if (dedamt == 0 && dedpercent != 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Text = Convert.ToString(dedpercent);
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Column.Width = 100;
                FpSpread1.Sheets[0].Cells[colcnt, 2].Locked = false;
            }
            else if (dedamt == 0 && dedpercent == 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].Text = "";
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Locked = false;

            }
            else if (dedamt != 0 && dedpercent == 0)
            {
                FpSpread1.Sheets[0].Cells[colcnt, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[colcnt, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[colcnt, 3].Locked = true;
            }
            if (priority != 0)
            {
                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                cb.AutoPostBack = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = cb;
                if (dedpercent == 0)
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                else
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Value = 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                cb.AutoPostBack = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = cb;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = false;
            }
            if (priority != 0 && dedpercent != 0)
            {
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Value = priority;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;
            }
            else
            {
                if (priority != 0 && dedpercent == 0)
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;
                }
                else
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
            }
            #endregion
        }
        FpSpread1.SaveChanges();
    }

    #region button save
    public void btn_save_click(object sender, EventArgs e)
    {
        try
        {
            #region get values
            bool saveflage = false;
            bool feeallot = true;
            string headercode = "";
            string ledgercode = "";
            double dect_amt = 0;
            double dect_per = 0;
            int refmode;
            string priority = "";
            string streamcode = "";
            string edulvl = "";
            string sem = "";
            string check = "";
            string stream = "";
            string educlevel = "";
            string semaster = "";
            string type = "";
            string deptcode = "";
            if (rb_concession.Checked == true)
                refmode = 1;
            else
                refmode = 2;
            //string finYearFk = d2.getCurrentFinanceYear(usercode, Convert.ToString(ddl_collegename.SelectedItem.Value));
            for (int s = 0; s < cbl_stream.Items.Count; s++)
            {
                if (cbl_stream.Items[s].Selected == true)
                {
                    if (stream == "")
                    {
                        stream = cbl_stream.Items[s].Value.ToString();
                    }
                    else
                    {
                        stream = stream + "','" + "" + cbl_stream.Items[s].Value.ToString() + "";
                    }
                }
            }

            for (int y = 0; y < cbl_edulevel.Items.Count; y++)
            {
                if (cbl_edulevel.Items[y].Selected == true)
                {
                    if (educlevel == "")
                    {
                        educlevel = cbl_edulevel.Items[y].Value.ToString();
                    }
                    else
                    {
                        educlevel = educlevel + "','" + "" + cbl_edulevel.Items[y].Value.ToString() + "";
                    }
                }
            }

            for (int z = 0; z < cbl_sem.Items.Count; z++)
            {
                if (cbl_sem.Items[z].Selected == true)
                {
                    if (semaster == "")
                    {
                        semaster = cbl_sem.Items[z].Value.ToString();
                    }
                    else
                    {
                        semaster = semaster + "','" + "" + cbl_sem.Items[z].Value.ToString() + "";
                    }
                }
            }
            string finYearFk = Convert.ToString(ddlfinyear.SelectedValue);
            #endregion

            FpSpread1.SaveChanges();
            string reason = Convert.ToString(ddl_reason.SelectedItem.Value);
            if (rb_concession.Checked == true)
            {
                if (stream != "")
                {
                    if (chkdept.Checked == true)
                    {
                        #region consession dept wise
                        if (reason.Trim() != "0")
                        {
                            for (int j = 0; j < cbl_stream.Items.Count; j++)
                            {
                                if (cbl_stream.Items[j].Selected == true)
                                {
                                    for (int k = 0; k < cbl_edulevel.Items.Count; k++)
                                    {
                                        if (cbl_edulevel.Items[k].Selected == true)
                                        {
                                            for (int b = 0; b < cbl_dept.Items.Count; b++)
                                            {
                                                if (cbl_dept.Items[b].Selected == true)
                                                {
                                                    #region
                                                    for (int s = 0; s < cbl_sem.Items.Count; s++)
                                                    {
                                                        if (cbl_sem.Items[s].Selected == true)
                                                        {
                                                            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                                                            {
                                                                streamcode = "" + cbl_stream.Items[j].Value.ToString() + "";
                                                                edulvl = "" + cbl_edulevel.Items[k].Value.ToString() + "";
                                                                deptcode = "" + cbl_dept.Items[b].Value.ToString() + "";
                                                                sem = "" + cbl_sem.Items[s].Value.ToString() + "";
                                                                string headerledcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                                                if (headerledcode.Contains(','))
                                                                {
                                                                    string[] split = headerledcode.Split(',');
                                                                    headercode = split[0];
                                                                    ledgercode = split[1];
                                                                }
                                                                if (rb_per.Checked == true)
                                                                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_per);
                                                                else
                                                                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_amt);
                                                                if (dect_per != 0 || dect_amt != 0 && !string.IsNullOrEmpty(headercode) && !string.IsNullOrEmpty(ledgercode))
                                                                {
                                                                    //general feeallot check
                                                                    bool saveval = false;
                                                                    double feeAmount = 0;
                                                                    double totAmount = 0;
                                                                    if (dect_amt != 0)
                                                                    {
                                                                        string SelQ = " select feeamount,totalamount from ft_feeallotdegree where degreecode in('" + deptcode + "') and headerfk in('" + headercode + "') and ledgerfk in('" + ledgercode + "') and feecategory in('" + sem + "') and finyearfk in('" + finYearFk + "')";
                                                                        DataSet dsfee = d2.select_method_wo_parameter(SelQ, "Text");
                                                                        if (dsfee.Tables.Count > 0 && dsfee.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            double.TryParse(Convert.ToString(dsfee.Tables[0].Rows[0]["feeamount"]), out feeAmount);
                                                                            double.TryParse(Convert.ToString(dsfee.Tables[0].Rows[0]["totalamount"]), out totAmount);
                                                                            if (totAmount >= dect_amt)
                                                                                saveval = true;
                                                                            else
                                                                                feeallot = false;
                                                                        }
                                                                    }
                                                                    else
                                                                        saveval = true;
                                                                    string insert_query = "";
                                                                    if (saveval)
                                                                    {
                                                                        string delqry = " delete FM_ConcessionRefundSettings where  Stream in ('" + streamcode + "') and Edu_Level in ('" + edulvl + "') and Degree_Code in('" + deptcode + "')  and Fee_Category in ('" + sem + "') and RefMode ='" + refmode + "' and ConsDesc='" + reason + "' and HeaderFK in ('" + headercode + "') and LedgerFK in('" + ledgercode + "') and finyearfk in('" + finYearFk + "')";
                                                                        int del = d2.update_method_wo_parameter(delqry, "Text");

                                                                        insert_query = "INSERT INTO FM_ConcessionRefundSettings(Stream,Edu_Level,Degree_Code,HeaderFK,LedgerFK,ConsDesc,ConsAmt,ConsType,RefMode,Fee_Category,ConsPer,finyearfk) VALUES('" + streamcode + "','" + edulvl + "','" + deptcode + "','" + headercode + "','" + ledgercode + "','" + reason + "','" + dect_amt + "','" + type + "','" + refmode + "','" + sem + "','" + dect_per + "','" + finYearFk + "')";
                                                                        int ins = d2.update_method_wo_parameter(insert_query, "Text");
                                                                        saveflage = true;
                                                                        feeallot = true;
                                                                    }
                                                                }

                                                            }

                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (!feeallot)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please Enter Equal Amout to General Fee Allot";
                            }
                            else if (saveflage == true)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please enter deduction amount or Percentage";
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please Select Any One Reason";
                        }
                        #endregion
                    }
                    else
                    {
                        #region consession without dept
                        if (reason.Trim() != "0")
                        {
                            for (int j = 0; j < cbl_stream.Items.Count; j++)
                            {
                                if (cbl_stream.Items[j].Selected == true)
                                {
                                    streamcode = "" + cbl_stream.Items[j].Value.ToString() + "";

                                    for (int k = 0; k < cbl_edulevel.Items.Count; k++)
                                    {
                                        if (cbl_edulevel.Items[k].Selected == true)
                                        {
                                            edulvl = "" + cbl_edulevel.Items[k].Value.ToString() + "";
                                            for (int s = 0; s < cbl_sem.Items.Count; s++)
                                            {
                                                if (cbl_sem.Items[s].Selected == true)
                                                {
                                                    sem = "" + cbl_sem.Items[s].Value.ToString() + "";

                                                    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                                                    {

                                                        string headerledcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                                        if (headerledcode.Contains(','))
                                                        {
                                                            string[] split = headerledcode.Split(',');
                                                            headercode = split[0];
                                                            ledgercode = split[1];
                                                        }
                                                        if (rb_per.Checked == true)
                                                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_per);
                                                        else
                                                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_amt);

                                                        string insert_query = "";
                                                        if (dect_per != 0 || dect_amt != 0)
                                                        {
                                                            string delqry = " delete FM_ConcessionRefundSettings where Stream in ('" + streamcode + "') and Edu_Level in ('" + edulvl + "') and Fee_Category in ('" + sem + "') and RefMode ='" + refmode + "' and ConsDesc='" + reason + "' and HeaderFK in ('" + headercode + "') and LedgerFK in('" + ledgercode + "') and finyearfk in('" + finYearFk + "')";
                                                            int del = d2.update_method_wo_parameter(delqry, "Text");

                                                            insert_query = "INSERT INTO FM_ConcessionRefundSettings(Stream,Edu_Level,HeaderFK,LedgerFK,ConsDesc,ConsAmt,ConsType,RefMode,Fee_Category,ConsPer,finyearfk) VALUES('" + streamcode + "','" + edulvl + "','" + headercode + "','" + ledgercode + "','" + reason + "','" + dect_amt + "','" + type + "','" + refmode + "','" + sem + "','" + dect_per + "','" + finYearFk + "')";
                                                            int ins = d2.update_method_wo_parameter(insert_query, "Text");
                                                            saveflage = true;
                                                        }

                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (saveflage == true)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please enter deduction amount or Percentage";
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please Select Any One Reason";
                        }
                        #endregion
                    }
                }
                else
                {
                    if (chkdept.Checked == true)
                    {
                        #region consession dept wise
                        if (reason.Trim() != "0")
                        {
                            for (int k = 0; k < cbl_edulevel.Items.Count; k++)
                            {
                                if (cbl_edulevel.Items[k].Selected == true)
                                {
                                    for (int b = 0; b < cbl_dept.Items.Count; b++)
                                    {
                                        if (cbl_dept.Items[b].Selected == true)
                                        {
                                            #region
                                            for (int s = 0; s < cbl_sem.Items.Count; s++)
                                            {
                                                if (cbl_sem.Items[s].Selected == true)
                                                {
                                                    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                                                    {
                                                        edulvl = "" + cbl_edulevel.Items[k].Value.ToString() + "";
                                                        deptcode = "" + cbl_dept.Items[b].Value.ToString() + "";
                                                        sem = "" + cbl_sem.Items[s].Value.ToString() + "";
                                                        string headerledcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                                        if (headerledcode.Contains(','))
                                                        {
                                                            string[] split = headerledcode.Split(',');
                                                            headercode = split[0];
                                                            ledgercode = split[1];
                                                        }
                                                        if (rb_per.Checked == true)
                                                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_per);
                                                        else
                                                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_amt);
                                                        string insert_query = "";
                                                        if (dect_per != 0 || dect_amt != 0 && !string.IsNullOrEmpty(headercode) && !string.IsNullOrEmpty(ledgercode))
                                                        {
                                                            //general feeallot check
                                                            bool saveval = false;
                                                            double feeAmount = 0;
                                                            double totAmount = 0;
                                                            if (dect_amt != 0)
                                                            {
                                                                string SelQ = " select feeamount,totalamount from ft_feeallotdegree where degreecode in('" + deptcode + "') and headerfk in('" + headercode + "') and ledgerfk in('" + ledgercode + "') and feecategory in('" + sem + "') and finyearfk in('" + finYearFk + "')";
                                                                DataSet dsfee = d2.select_method_wo_parameter(SelQ, "Text");
                                                                if (dsfee.Tables.Count > 0 && dsfee.Tables[0].Rows.Count > 0)
                                                                {
                                                                    double.TryParse(Convert.ToString(dsfee.Tables[0].Rows[0]["feeamount"]), out feeAmount);
                                                                    double.TryParse(Convert.ToString(dsfee.Tables[0].Rows[0]["totalamount"]), out totAmount);
                                                                    if (totAmount >= dect_amt)
                                                                        saveval = true;
                                                                    else
                                                                        feeallot = false;
                                                                }
                                                            }
                                                            else
                                                                saveval = true;
                                                            if (saveval)
                                                            {
                                                                string delqry = " delete FM_ConcessionRefundSettings where  Edu_Level in ('" + edulvl + "') and Degree_Code in('" + deptcode + "')  and Fee_Category in ('" + sem + "') and RefMode ='" + refmode + "' and ConsDesc='" + reason + "' and HeaderFK in ('" + headercode + "') and LedgerFK in('" + ledgercode + "') and finyearfk in('" + finYearFk + "')";
                                                                int del = d2.update_method_wo_parameter(delqry, "Text");

                                                                insert_query = "INSERT INTO FM_ConcessionRefundSettings(Edu_Level,Degree_Code,HeaderFK,LedgerFK,ConsDesc,ConsAmt,ConsType,RefMode,Fee_Category,ConsPer,finyearfk) VALUES('" + edulvl + "','" + deptcode + "','" + headercode + "','" + ledgercode + "','" + reason + "','" + dect_amt + "','" + type + "','" + refmode + "','" + sem + "','" + dect_per + "','" + finYearFk + "')";
                                                                int ins = d2.update_method_wo_parameter(insert_query, "Text");
                                                                saveflage = true;
                                                                feeallot = true;
                                                            }
                                                        }

                                                    }

                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            if (!feeallot)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please Enter Equal Amout to General Fee Allot";
                            }
                            else if (saveflage == true)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please enter deduction amount or Percentage";
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please Select Any One Reason";
                        }
                        #endregion
                    }
                    else
                    {
                        #region consession without dept
                        if (reason.Trim() != "0")
                        {
                            //for (int j = 0; j < cbl_stream.Items.Count; j++)
                            //{
                            //    if (cbl_stream.Items[j].Selected == true)
                            //    {
                            //        streamcode = "" + cbl_stream.Items[j].Value.ToString() + "";

                            for (int k = 0; k < cbl_edulevel.Items.Count; k++)
                            {
                                if (cbl_edulevel.Items[k].Selected == true)
                                {
                                    edulvl = "" + cbl_edulevel.Items[k].Value.ToString() + "";
                                    for (int s = 0; s < cbl_sem.Items.Count; s++)
                                    {
                                        if (cbl_sem.Items[s].Selected == true)
                                        {
                                            sem = "" + cbl_sem.Items[s].Value.ToString() + "";

                                            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                                            {

                                                string headerledcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                                string[] split = headerledcode.Split(',');
                                                headercode = split[0];
                                                ledgercode = split[1];
                                                if (rb_per.Checked == true)
                                                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_per);
                                                else
                                                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_amt);
                                                string insert_query = "";
                                                if (dect_per != 0 || dect_amt != 0)
                                                {
                                                    string delqry = " delete FM_ConcessionRefundSettings where  Edu_Level in ('" + edulvl + "') and Fee_Category in ('" + sem + "') and RefMode ='" + refmode + "' and ConsDesc='" + reason + "' and HeaderFK in ('" + headercode + "') and LedgerFK in('" + ledgercode + "') and finyearfk in('" + finYearFk + "')";
                                                    int del = d2.update_method_wo_parameter(delqry, "Text");

                                                    insert_query = "INSERT INTO FM_ConcessionRefundSettings(Edu_Level,HeaderFK,LedgerFK,ConsDesc,ConsAmt,ConsType,RefMode,Fee_Category,ConsPer,finyearfk) VALUES('" + edulvl + "','" + headercode + "','" + ledgercode + "','" + reason + "','" + dect_amt + "','" + type + "','" + refmode + "','" + sem + "','" + dect_per + "','" + finYearFk + "')";
                                                    int ins = d2.update_method_wo_parameter(insert_query, "Text");
                                                    saveflage = true;
                                                }

                                            }

                                        }
                                    }
                                }
                            }
                            //    }
                            //}
                            if (saveflage == true)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please enter deduction amount or Percentage";
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Please Select Any One Reason";
                        }
                        #endregion
                    }
                }
            }
            else
            {
                if (stream != "")
                {
                    if (chkdept.Checked == true)
                    {
                        #region refund dept wise
                        if (reason.Trim() != "0" || reason.Trim() == "0")
                        {
                            type = Convert.ToString(ddl_enroll.SelectedItem.Value);

                            for (int j = 0; j < cbl_stream.Items.Count; j++)
                            {
                                if (cbl_stream.Items[j].Selected == true)
                                {
                                    streamcode = "" + cbl_stream.Items[j].Value.ToString() + "";

                                    for (int k = 0; k < cbl_edulevel.Items.Count; k++)
                                    {
                                        if (cbl_edulevel.Items[k].Selected == true)
                                        {
                                            edulvl = "" + cbl_edulevel.Items[k].Value.ToString() + "";

                                            for (int b = 0; b < cbl_dept.Items.Count; b++)
                                            {
                                                if (cbl_dept.Items[b].Selected == true)
                                                {
                                                    deptcode = "" + cbl_dept.Items[b].Value.ToString() + "";
                                                    #region
                                                    for (int s = 0; s < cbl_sem.Items.Count; s++)
                                                    {
                                                        if (cbl_sem.Items[s].Selected == true)
                                                        {
                                                            sem = "" + cbl_sem.Items[s].Value.ToString() + "";

                                                            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                                                            {

                                                                string headerledcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                                                string[] split = headerledcode.Split(',');
                                                                headercode = split[0];
                                                                ledgercode = split[1];

                                                                if (rb_per.Checked == true)
                                                                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_per);
                                                                else
                                                                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_amt);

                                                                check = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Value);
                                                                priority = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Text);
                                                                refmode = 2;
                                                                string insert_query = "";
                                                                if (dect_per != 0 || dect_amt != 0)
                                                                {
                                                                    string delqry = " delete FM_ConcessionRefundSettings where Stream in('" + streamcode + "') and Edu_Level in('" + edulvl + "') and Degree_Code in('" + deptcode + "')  and Fee_Category in ('" + sem + "') and RefMode ='" + refmode + "' and HeaderFK in('" + headercode + "') and LedgerFK in('" + ledgercode + "') and finyearfk in('" + finYearFk + "')";
                                                                    if (reason.Trim() != "0")
                                                                    {
                                                                        delqry = delqry + " and ConsDesc='" + reason + "'";
                                                                    }
                                                                    int del = d2.update_method_wo_parameter(delqry, "Text");


                                                                    insert_query = "INSERT INTO FM_ConcessionRefundSettings(Stream,Edu_Level,Degree_Code,HeaderFK,LedgerFK,ConsDesc,ConsAmt,ConsType,RefMode,LedPriority,Fee_Category,ConsPer,finyearfk) VALUES('" + streamcode + "','" + edulvl + "','" + deptcode + "','" + headercode + "','" + ledgercode + "','" + reason + "','" + dect_amt + "','" + type + "','" + refmode + "','" + priority + "','" + sem + "','" + dect_per + "','" + finYearFk + "')";
                                                                    int ins = d2.update_method_wo_parameter(insert_query, "Text");
                                                                    saveflage = true;
                                                                }
                                                            }

                                                        }
                                                    }
                                                    #endregion
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                            if (saveflage == true)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please enter deduction amount or Percentage";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region refund without dept
                        if (reason.Trim() != "0" || reason.Trim() == "0")
                        {
                            type = Convert.ToString(ddl_enroll.SelectedItem.Value);

                            for (int j = 0; j < cbl_stream.Items.Count; j++)
                            {
                                if (cbl_stream.Items[j].Selected == true)
                                {
                                    streamcode = "" + cbl_stream.Items[j].Value.ToString() + "";

                                    for (int k = 0; k < cbl_edulevel.Items.Count; k++)
                                    {
                                        if (cbl_edulevel.Items[k].Selected == true)
                                        {
                                            edulvl = "" + cbl_edulevel.Items[k].Value.ToString() + "";
                                            for (int s = 0; s < cbl_sem.Items.Count; s++)
                                            {
                                                if (cbl_sem.Items[s].Selected == true)
                                                {
                                                    sem = "" + cbl_sem.Items[s].Value.ToString() + "";

                                                    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                                                    {

                                                        string headerledcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                                        string[] split = headerledcode.Split(',');
                                                        headercode = split[0];
                                                        ledgercode = split[1];
                                                        if (rb_per.Checked == true)
                                                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_per);
                                                        else
                                                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_amt);

                                                        check = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Value);
                                                        priority = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Text);
                                                        refmode = 2;
                                                        string insert_query = "";
                                                        if (dect_per != 0 || dect_amt != 0)
                                                        {
                                                            string delqry = " delete FM_ConcessionRefundSettings where Stream in('" + streamcode + "') and Edu_Level in('" + edulvl + "') and Fee_Category in ('" + sem + "') and RefMode ='" + refmode + "' and HeaderFK in('" + headercode + "') and LedgerFK in('" + ledgercode + "') and finyearfk in('" + finYearFk + "')";
                                                            if (reason.Trim() != "0")
                                                            {
                                                                delqry = delqry + " and ConsDesc='" + reason + "'";
                                                            }
                                                            int del = d2.update_method_wo_parameter(delqry, "Text");


                                                            insert_query = "INSERT INTO FM_ConcessionRefundSettings(Stream,Edu_Level,HeaderFK,LedgerFK,ConsDesc,ConsAmt,ConsType,RefMode,LedPriority,Fee_Category,ConsPer,finyearfk) VALUES('" + streamcode + "','" + edulvl + "','" + headercode + "','" + ledgercode + "','" + reason + "','" + dect_amt + "','" + type + "','" + refmode + "','" + priority + "','" + sem + "','" + dect_per + "','" + finYearFk + "')";
                                                            int ins = d2.update_method_wo_parameter(insert_query, "Text");
                                                            saveflage = true;
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (saveflage == true)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please enter deduction amount or Percentage";
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    if (chkdept.Checked == true)
                    {
                        #region refund dept wise
                        if (reason.Trim() != "0" || reason.Trim() == "0")
                        {
                            type = Convert.ToString(ddl_enroll.SelectedItem.Value);

                            //for (int j = 0; j < cbl_stream.Items.Count; j++)
                            //{
                            //    if (cbl_stream.Items[j].Selected == true)
                            //    {
                            //        streamcode = "" + cbl_stream.Items[j].Value.ToString() + "";

                            for (int k = 0; k < cbl_edulevel.Items.Count; k++)
                            {
                                if (cbl_edulevel.Items[k].Selected == true)
                                {
                                    edulvl = "" + cbl_edulevel.Items[k].Value.ToString() + "";

                                    for (int b = 0; b < cbl_dept.Items.Count; b++)
                                    {
                                        if (cbl_dept.Items[b].Selected == true)
                                        {
                                            deptcode = "" + cbl_dept.Items[b].Value.ToString() + "";
                                            #region
                                            for (int s = 0; s < cbl_sem.Items.Count; s++)
                                            {
                                                if (cbl_sem.Items[s].Selected == true)
                                                {
                                                    sem = "" + cbl_sem.Items[s].Value.ToString() + "";

                                                    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                                                    {

                                                        string headerledcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                                        string[] split = headerledcode.Split(',');
                                                        headercode = split[0];
                                                        ledgercode = split[1];

                                                        if (rb_per.Checked == true)
                                                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_per);
                                                        else
                                                            double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_amt);

                                                        check = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Value);
                                                        priority = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Text);
                                                        refmode = 2;
                                                        string insert_query = "";
                                                        if (dect_per != 0 || dect_amt != 0)
                                                        {
                                                            string delqry = " delete FM_ConcessionRefundSettings where Edu_Level in('" + edulvl + "') and Degree_Code in('" + deptcode + "')  and Fee_Category in ('" + sem + "') and RefMode ='" + refmode + "' and HeaderFK in('" + headercode + "') and LedgerFK in('" + ledgercode + "') and finyearfk in('" + finYearFk + "')";
                                                            if (reason.Trim() != "0")
                                                            {
                                                                delqry = delqry + " and ConsDesc='" + reason + "'";
                                                            }
                                                            int del = d2.update_method_wo_parameter(delqry, "Text");


                                                            insert_query = "INSERT INTO FM_ConcessionRefundSettings(Edu_Level,Degree_Code,HeaderFK,LedgerFK,ConsDesc,ConsAmt,ConsType,RefMode,LedPriority,Fee_Category,ConsPer,finyearfk) VALUES('" + edulvl + "','" + deptcode + "','" + headercode + "','" + ledgercode + "','" + reason + "','" + dect_amt + "','" + type + "','" + refmode + "','" + priority + "','" + sem + "','" + dect_per + "','" + finYearFk + "')";
                                                            int ins = d2.update_method_wo_parameter(insert_query, "Text");
                                                            saveflage = true;
                                                        }
                                                    }

                                                }
                                            }
                                            #endregion
                                        }

                                    }
                                }
                            }
                            //    }
                            //}
                            if (saveflage == true)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please enter deduction amount or Percentage";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region refund without dept
                        if (reason.Trim() != "0" || reason.Trim() == "0")
                        {
                            type = Convert.ToString(ddl_enroll.SelectedItem.Value);

                            //for (int j = 0; j < cbl_stream.Items.Count; j++)
                            //{
                            //    if (cbl_stream.Items[j].Selected == true)
                            //    {
                            //        streamcode = "" + cbl_stream.Items[j].Value.ToString() + "";

                            for (int k = 0; k < cbl_edulevel.Items.Count; k++)
                            {
                                if (cbl_edulevel.Items[k].Selected == true)
                                {
                                    edulvl = "" + cbl_edulevel.Items[k].Value.ToString() + "";
                                    for (int s = 0; s < cbl_sem.Items.Count; s++)
                                    {
                                        if (cbl_sem.Items[s].Selected == true)
                                        {
                                            sem = "" + cbl_sem.Items[s].Value.ToString() + "";

                                            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                                            {

                                                string headerledcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                                                string[] split = headerledcode.Split(',');
                                                headercode = split[0];
                                                ledgercode = split[1];

                                                if (rb_per.Checked == true)
                                                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_per);
                                                else
                                                    double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text), out dect_amt);

                                                check = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 4].Value);
                                                priority = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Text);
                                                refmode = 2;
                                                string insert_query = "";
                                                if (dect_per != 0 || dect_amt != 0)
                                                {
                                                    string delqry = " delete FM_ConcessionRefundSettings where Edu_Level in('" + edulvl + "') and Fee_Category in ('" + sem + "') and RefMode ='" + refmode + "' and HeaderFK in('" + headercode + "') and LedgerFK in('" + ledgercode + "') and finyearfk in('" + finYearFk + "')";
                                                    if (reason.Trim() != "0")
                                                    {
                                                        delqry = delqry + " and ConsDesc='" + reason + "'";
                                                    }
                                                    int del = d2.update_method_wo_parameter(delqry, "Text");


                                                    insert_query = "INSERT INTO FM_ConcessionRefundSettings(Edu_Level,HeaderFK,LedgerFK,ConsDesc,ConsAmt,ConsType,RefMode,LedPriority,Fee_Category,ConsPer,finyearfk) VALUES('" + headercode + "','" + ledgercode + "','" + reason + "','" + dect_amt + "','" + type + "','" + refmode + "','" + priority + "','" + sem + "','" + dect_per + "','" + finYearFk + "')";
                                                    int ins = d2.update_method_wo_parameter(insert_query, "Text");
                                                    saveflage = true;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            //    }
                            //}
                            if (saveflage == true)
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Text = "Please enter deduction amount or Percentage";
                            }
                        }
                        #endregion
                    }
                }
            }

        }
        catch
        {
        }
    }
    #endregion


    #region reset
    protected void btn_reset_Onclick(object sender, EventArgs e)
    {
        try
        {

            Boolean delflag = false;
            int refmode = 0;
            string stream = "";
            string educlevel = "";
            string semaster = "";
            if (rb_concession.Checked == true)
            {
                refmode = 1;
            }
            else
            {
                refmode = 2;
            }

            for (int s = 0; s < cbl_stream.Items.Count; s++)
            {
                if (cbl_stream.Items[s].Selected == true)
                {
                    if (stream == "")
                    {
                        stream = cbl_stream.Items[s].Value.ToString();
                    }
                    else
                    {
                        stream = stream + "','" + "" + cbl_stream.Items[s].Value.ToString() + "";
                    }
                }
            }

            for (int y = 0; y < cbl_edulevel.Items.Count; y++)
            {
                if (cbl_edulevel.Items[y].Selected == true)
                {
                    if (educlevel == "")
                    {
                        educlevel = cbl_edulevel.Items[y].Value.ToString();
                    }
                    else
                    {
                        educlevel = educlevel + "','" + "" + cbl_edulevel.Items[y].Value.ToString() + "";
                    }
                }
            }

            for (int z = 0; z < cbl_sem.Items.Count; z++)
            {
                if (cbl_sem.Items[z].Selected == true)
                {
                    if (semaster == "")
                    {
                        semaster = cbl_sem.Items[z].Value.ToString();
                    }
                    else
                    {
                        semaster = semaster + "','" + "" + cbl_sem.Items[z].Value.ToString() + "";
                    }
                }
            }
            string reason = Convert.ToString(ddl_reason.SelectedItem.Value);
            FpSpread1.SaveChanges();

            if (rb_concession.Checked == true)
            {
                if (stream.Trim() != "" && educlevel.Trim() != "" && semaster.Trim() != "" && reason.Trim() != "")
                {
                    string delqry = " delete FM_ConcessionRefundSettings where Stream in ('" + stream + "') and Edu_Level in ('" + educlevel + "') and Fee_Category in ('" + semaster + "') and RefMode ='" + refmode + "' and ConsDesc='" + reason + "'";
                    int delupd = d2.update_method_wo_parameter(delqry, "Text");
                    delflag = true;
                }

            }
            else
            {
                if (stream.Trim() != "" && educlevel.Trim() != "" && semaster.Trim() != "")
                {
                    string delqry = " delete FM_ConcessionRefundSettings where Stream in ('" + stream + "') and Edu_Level in ('" + educlevel + "') and Fee_Category in ('" + semaster + "') and RefMode ='" + refmode + "'";
                    if (reason.Trim() != "0")
                    {
                        delqry = delqry + " and ConsDesc='" + reason + "'";
                    }
                    int delupd = d2.update_method_wo_parameter(delqry, "Text");
                    delflag = true;
                }

            }
            if (delflag == true)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Reset Successfully";
                // FpSpread1.SaveChanges();
                btn_go_Click(sender, e);
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Please Select Corresponding Values";
            }
        }
        catch
        {
        }
    }
    #endregion

    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {

    }
    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (flag_true == true)
        {
            FpSpread1.SaveChanges();
            string activrow = "";
            activrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string activecol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            int actcol = Convert.ToInt16(activecol);
            int hy_order = 0;
            for (int i = 0; i <= Convert.ToInt16(FpSpread1.Sheets[0].RowCount) - 1; i++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, actcol].Value);
                if (isval == 1)
                {

                    hy_order++;
                    FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Locked = true;
                }
            }
            FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].Text = hy_order.ToString();
        }
    }

    protected void FpSpread1_ButtonCommand(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();


        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        if (activecol == "3")
        {

            int act1 = Convert.ToInt32(activerow);
            int act2 = Convert.ToInt16(activecol);

            if (FpSpread1.Sheets[0].Cells[act1, act2].Value.ToString() == "1")
            {
                flag_true = true;
                FpSpread1.Sheets[0].Cells[act1, act2 + 1].Text = "";
            }
            else
            {
                flag_true = false;
            }
        }
        FpSpread1.SaveChanges();

    }


    protected void rb_amt_OnCheckedChanged(object sender, EventArgs e)
    {
        //Divspread.Visible = false;
        //FpSpread1.Visible = false;
        //btn_save.Visible = false;
        //btn_reset.Visible = false;
        // btn_go_Click(sender, e);
    }
    protected void rb_per_OnCheckedChanged(object sender, EventArgs e)
    {
        //Divspread.Visible = false;
        //FpSpread1.Visible = false;
        //btn_save.Visible = false;
        //btn_reset.Visible = false;
        // btn_go_Click(sender, e);
    }
    protected void chkdept_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkdept.Checked == true)
        {
            txt_course.Enabled = true;
            txt_dept.Enabled = true;
        }
        else
        {
            txt_course.Enabled = false;
            txt_dept.Enabled = false;
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

        lbl.Add(lbl_collegename);
        lbl.Add(lbl_str);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    // last modified 22.06.2017 sudhagar

    #region financial year
    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + collegecode + "'  order by FinYearPK desc";
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
            }
        }
        catch (Exception ex)
        {

        }
    }
    #endregion
}