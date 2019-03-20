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

public partial class OfficeMOD_CodeSetting : System.Web.UI.Page
{

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DateTime dt;
    string collegecode = string.Empty;
    string usercode = string.Empty;
    int row;
    int i;
    string[] split;
    string dep = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    bool fromDropDown = false;
    string dept = string.Empty;
    string sql1 = string.Empty;
    string Acroynm = string.Empty;
    string startno = string.Empty;
    string size = string.Empty;
    DAccess2 da = new DAccess2();
    string select = string.Empty;
    string dep1 = string.Empty;
    string PageLogOut = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //string sess = Convert.ToString(Session["IsLogin"]);
        //PageLogOut = Convert.ToString(Session["PageLogout"]);
        //if (sess == "")
        //{
        //}
        //else
        //{
        //    if (!Request.FilePath.Contains("OffM"))
        //    {
        //        string strPreviousPage = "";
        //        if (Request.UrlReferrer != null)
        //        {
        //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //        }
        //        if (strPreviousPage == "")
        //        {
        //            string redrURI = ConfigurationManager.AppSettings["Office"].Trim();
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

        if (!Request.FilePath.Contains("OfficeHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/OfficeMOD/OfficeHome.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        // usercode = Session["usercode"].ToString();
        if (!IsPostBack)
        {
            //setLabelText();
            bindcollege();
            if (ddlcol.Items.Count > 0)
            {
                collegecode = ddlcol.SelectedItem.Value;
            }
            else collegecode = "0";
            binddept();
            //bindheadername();
            //bindheadernamePrev();
            loadsetting();
        }
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
        txtdateerr.Visible = false;
        if (ddlcol.Items.Count > 0)
        {
            collegecode = ddlcol.SelectedItem.Value;
        }
        else collegecode = "0";
    }

    protected void bindcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddlcol.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcol.DataSource = ds;
                ddlcol.DataTextField = "collname";
                ddlcol.DataValueField = "college_code";
                ddlcol.DataBind();
            }
        }
        catch
        {

        }
    }

    protected void loadsetting()
    {
        try
        {
            dt = new DateTime();
            string selquery = "select top 1 * from InwardCodeSettings where CollegeCode='" + collegecode + "' order by FromDate desc";
            if (fromDropDown)
            {
                split = ddl_PrevDate.SelectedItem.Text.Split('/');
                dt = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]);
                TimeSpan time = dt.TimeOfDay;
                selquery = "select * from InwardCodeSettings where FromDate='" + dt.ToString("MM/dd/yyyy") + "' and CollegeCode='" + collegecode + "' order by FromDate desc";
            }
            else
            {
                string selectq = "select distinct CONVERT(varchar(10), FromDate,103) as newdate,FromDate from InwardCodeSettings where CollegeCode='" + collegecode + "' order by FromDate desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_PrevDate.DataSource = ds;
                    ddl_PrevDate.DataTextField = "newdate";
                    ddl_PrevDate.DataBind();
                }
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selquery, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        TextBox Itemcode = (TextBox)ds.Tables[0].Rows[i].FindControl("txtacronym");
            //        TextBox Itemvalue = (TextBox)ds.Tables[0].Rows[i].FindControl("txtstartno");
            //        TextBox Itemsize = (TextBox)old_grid.Rows[i].FindControl("txt_size");

            //        switch (i)
            //        {
            //            case 0:
            //                Itemcode.Text = ds.Tables[0].Rows[0]["RcptAcr"].ToString();
            //                Itemvalue.Text = ds.Tables[0].Rows[0]["RcptStNo"].ToString();
            //                Itemsize.Text = ds.Tables[0].Rows[0]["RcptSize"].ToString();
            //                break;
            //            case 1:
            //                Itemcode.Text = ds.Tables[0].Rows[0]["VouchAcr"].ToString();
            //                Itemvalue.Text = ds.Tables[0].Rows[0]["VouchStNo"].ToString();
            //                Itemsize.Text = ds.Tables[0].Rows[0]["VouchSize"].ToString();
            //                break;
            //            case 2:
            //                Itemcode.Text = ds.Tables[0].Rows[0]["DupRcptAcr"].ToString();
            //                Itemvalue.Text = ds.Tables[0].Rows[0]["DupRcptStNo"].ToString();
            //                Itemsize.Text = ds.Tables[0].Rows[0]["DupRcptSize"].ToString();
            //                break;
            //            case 3:
            //                Itemcode.Text = ds.Tables[0].Rows[0]["DataImportAcr"].ToString();
            //                Itemvalue.Text = ds.Tables[0].Rows[0]["DataImportStNo"].ToString();
            //                Itemsize.Text = ds.Tables[0].Rows[0]["DataImportSize"].ToString();
            //                break;
            //            case 4:
            //                Itemcode.Text = ds.Tables[0].Rows[0]["ChallanAcr"].ToString();
            //                Itemvalue.Text = ds.Tables[0].Rows[0]["ChallanStNo"].ToString();
            //                Itemsize.Text = ds.Tables[0].Rows[0]["ChallanSize"].ToString();
            //                break;
            //            case 5:
            //                Itemcode.Text = ds.Tables[0].Rows[0]["JournalAcr"].ToString();
            //                Itemvalue.Text = ds.Tables[0].Rows[0]["JournalStNo"].ToString();
            //                Itemsize.Text = ds.Tables[0].Rows[0]["JournalSize"].ToString();
            //                break;
            //            case 6:
            //                Itemcode.Text = ds.Tables[0].Rows[0]["ScholarshipAcr"].ToString();
            //                Itemvalue.Text = ds.Tables[0].Rows[0]["ScholarshipStNo"].ToString();
            //                Itemsize.Text = ds.Tables[0].Rows[0]["ScholarshipSize"].ToString();
            //                break;
            //        }
            //    }
            //    string acchead = ds.Tables[0].Rows[0]["IsHeader"].ToString();
            //    string headid = ds.Tables[0].Rows[0]["HeaderFK"].ToString();
            //    int count = 0;
            //    split = ddl_PrevDate.SelectedItem.Text.Split('/');
            //    dt = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]);
            //    string selacc = "select distinct a.HeaderPK,a.HeaderName,f.HeaderFK from FM_HeaderMaster a,FM_FinCodeSettings f where HeaderName is not null and a.HeaderPK=f.HeaderFK and f.FromDate='" + dt.ToString("MM/dd/yyyy") + "' and f.IsHeader='" + acchead + "' and CollegeCode='" + collegecode + "'";
            //    ds.Clear();
            //    ds = d2.select_method_wo_parameter(selacc, "Text");
            //    if (acchead.Trim() != null)
            //    {
            //        //cb_forheader.Enabled = true;
            //        //cb_forheader.Checked = false;

            //        // txt_select.Enabled = true;
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            for (int j = 0; j < cbl_header.Items.Count; j++)
            //            {
            //                cbl_header.Items[j].Selected = false;
            //            }
            //            for (int j = 0; j < cbl_header.Items.Count; j++)
            //            {
            //                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            //                {
            //                    if (Convert.ToString(cbl_header.Items[j].Value) == Convert.ToString(ds.Tables[0].Rows[k]["HeaderPK"]))
            //                    {
            //                        cbl_header.Items[j].Selected = true;
            //                        count = count + 1;
            //                    }
            //                }
            //            }
            //            if (cbl_header.Items.Count == ds.Tables[0].Rows.Count)
            //            {
            //                txt_select.Text = "Header Name(" + count + ")";
            //            }
            //            }
            //            else
            //            {
            //                //cb_forheader.Enabled = true;
            //                //cb_forheader.Checked = false;
            //                //txt_select.Text = "--Select--";
            //                //txt_select.Enabled = false;
            //                //btnGo.Enabled = false;
            //                //btnSaveHeader.Enabled = false;
            //            }
            //        }
            //        else
            //        {
            //            //cb_forheader.Enabled = true;
            //            //cb_forheader.Checked = false;
            //            //txt_select.Text = "--Select--";
            //            //txt_select.Enabled = false;
            //            //btnGo.Enabled = false;
            //            //btnSaveHeader.Enabled = false;
            //        }
            //    }
            //}
        }
        catch
        {

        }
    }

    protected void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            ds.Clear();
            string group_user = "";
            string cmd = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                cmd = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";
            }
            ds = d2.select_method_wo_parameter(cmd, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds.Tables[0];
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "dept_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = "Department(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
                else
                {
                    txt_dept.Text = "--Select--";
                    cb_dept.Checked = false;
                }
            }
        }
        catch { }
    }

    protected void ddlcol_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadsetting();
        }
        catch
        {

        }
    }

    protected void txt_frmdate_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            txtdateerr.Visible = false;
            string dateTime = txt_frmdate.Text.Split('/')[1] + "/" + txt_frmdate.Text.Split('/')[0] + "/" + txt_frmdate.Text.Split('/')[2];
            DateTime dt = new DateTime();
            dt = DateTime.Now.Date;
            DateTime dt2 = Convert.ToDateTime(dateTime);


            if (dt2 < dt)
            {
                txtdateerr.Visible = true;
                txtdateerr.Text = "Date Must be Current Date";
            }
            else if (dt2 > dt)
            {
                txtdateerr.Visible = true;
                txtdateerr.Text = "Date Must be Current Date";
            }
            else
            {
                txtdateerr.Visible = false;
                //Mainpage.Visible = true;
                //btn_save.Visible = true;
                //btn_reset.Visible = true;
                //btn_exit.Visible = true;
                //ug_grid.Visible = true;
                //old_grid.Visible = true;
                //div1.Visible = true;
            }
        }
        catch
        {

        }
    }

    protected void ddl_PrevDate_OnSelectedIndexChange(object sender, EventArgs e)
    {
        fromDropDown = true;
        loadsetting();
    }

    protected void cb_dept_CheckedChanged(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
    }

    protected void cbl_dept_selectedchanged(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
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
    private void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }
    private void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                {
                    chkchange.Checked = true;
                }
            }
        }
        catch { }
    }

    #endregion

    #region go
    protected void btnGo_OnClick(object sender, EventArgs e)
    {
        DataSet costmaster = new DataSet();
        loadspreaddetails(ds);
        costmaster = codesetting();
        if (costmaster.Tables.Count > 0 && costmaster.Tables[0].Rows.Count > 0)
        {
            loadspreaddetails(ds);

        }
        //else
        //{
        //    alertpopwindow.Visible = true;
        //    lblalerterr.Text = "No Record Found!";
        //}

    }
    #endregion

    protected void Fpload1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //Fpspread2.Visible = true;
        try
        {
            string actrow = Fpload1.Sheets[0].ActiveRow.ToString();
            string actcol = Fpload1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpload1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpload1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpload1.Sheets[0].RowCount; i++)
                        {
                            Fpload1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpload1.Sheets[0].RowCount; i++)
                        {
                            Fpload1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "Individual_StudentFeeStatus"); 
        }
    }

    #region save,delete,update
    protected void btnSave_Click(object sender, EventArgs e)
    {

        int query = 0;


        string sqls = string.Empty;
        string acr = string.Empty;
        string stno = string.Empty;
        string siz = string.Empty;
        try
        {
            Fpload1.SaveChanges();

            string firstdate = Convert.ToString(txt_frmdate.Text);
            dt = new DateTime();
            split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]);
            DateTime date = dt.Date;
            DateTime currdate = DateTime.Now.Date;
            string currtime = DateTime.Now.ToLongTimeString();
            bool check = false;
            int inscount = 0;

            if (Fpload1.Rows.Count > 0)
            {
                Fpload1.SaveChanges();
                for (int row = 0; row < Fpload1.Sheets[0].RowCount; row++)
                {
                    int checkval = Convert.ToInt32(Fpload1.Sheets[0].Cells[row, 1].Value);
                    if (checkval == 1)
                    {
                        dep = Convert.ToString(Fpload1.Sheets[0].Cells[row, 2].Tag);
                        acr = Convert.ToString(Fpload1.Sheets[0].Cells[row, 3].Text);
                        stno = Convert.ToString(Fpload1.Sheets[0].Cells[row, 4].Text);
                        siz = Convert.ToString(Fpload1.Sheets[0].Cells[row, 5].Text);


                        if (acr != "" && stno != "" && siz != "" && dep != "")
                        {

                            sqls = "if exists (select * from InwardCodeSettings where InwardAcr='" + acr + "' and InwardSize='" + siz + "' and  InwardStNo='" + stno + "' and CollegeCode='" + Convert.ToString(collegecode) + "' and  FromTime='" + currtime + "' and FromDate='" + date.ToString("MM/dd/yyyy") + "' and DeptCode='" + dep + "') update InwardCodeSettings set InwardAcr='" + acr + "' , InwardSize='" + siz + "' ,  InwardStNo='" + stno + "' , CollegeCode='" + Convert.ToString(collegecode) + "' ,  FromTime='" + currtime + "' , FromDate='" + date.ToString("MM/dd/yyyy") + "' , DeptCode='" + dep + "'  where InwardAcr='" + acr + "' and InwardSize='" + siz + "' and  InwardStNo='" + stno + "' and CollegeCode='" + Convert.ToString(collegecode) + "' and  FromTime='" + currtime + "' and FromDate='" + date.ToString("MM/dd/yyyy") + "' and DeptCode='" + dep + "' else insert into InwardCodeSettings (InwardAcr,InwardSize,InwardStNo,CollegeCode,FromTime,FromDate,DeptCode) values('" + acr + "','" + siz + "','" + stno + "','" + Convert.ToString(collegecode) + "','" + currtime + "','" + date.ToString("MM/dd/yyyy") + "','" + dep + "')";

                            inscount = d2.update_method_wo_parameter(sqls, "Text");

                        }
                    }
                }
                if (inscount != 0)
                {
                    Div1.Visible = true;
                    Label3.Visible = true;
                    Label3.Text = "Saved Successfully";

                }
                else
                {
                    Label3.Text = "Not Saved";
                }
            }
        }


        catch
        {
        }
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        int query = 0;
        try
        {
            string sqlu = string.Empty;
            string acr = string.Empty;
            string stno = string.Empty;
            string siz = string.Empty;

            Fpload1.SaveChanges();
            int activerow = Fpload1.ActiveSheetView.ActiveRow;
            int activecol = Fpload1.ActiveSheetView.ActiveColumn;
            dep = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 2].Tag);
            acr = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 3].Text);
            stno = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 4].Text);
            siz = Convert.ToString(Fpload1.Sheets[0].Cells[activerow, 5].Text);

            string firstdate = Convert.ToString(txt_frmdate.Text);
            dt = new DateTime();
            split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]);
            DateTime date = dt.Date;
            DateTime currdate = DateTime.Now.Date;
            string currtime = DateTime.Now.ToLongTimeString();


            sqlu = "update InwardCodeSettings set InwardAcr='" + acr + "' , InwardSize='" + siz + "' ,  InwardStNo='" + stno + "' ,  FromTime='" + currtime + "' , FromDate='" + date.ToString("MM/dd/yyyy") + "'  where  DeptCode='" + dep + "' and  CollegeCode='" + Convert.ToString(collegecode) + "'";

            int qry = d2.update_method_wo_parameter(sqlu, "Text");
            if (qry != 0)
            {
                Div1.Visible = true;
                Label3.Visible = true;
                Label3.Text = "Updated Successfully";

            }
            else
            {

                Div1.Visible = false;
                lblalerterr.Text = "No Record Found!";
            }
        }
        catch
        {
        }
    }

    protected void delete()
    {
        try
        {
            string sqld = string.Empty;
            string acr = string.Empty;
            string stno = string.Empty;
            string siz = string.Empty;

            int query = 0;
            Fpload1.SaveChanges();
            string firstdate = Convert.ToString(txt_frmdate.Text);
            dt = new DateTime();
            split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]);
            DateTime date = dt.Date;
            DateTime currdate = DateTime.Now.Date;
            string currtime = DateTime.Now.ToLongTimeString();
            if (Fpload1.Rows.Count > 0)
            {
                Fpload1.SaveChanges();
                for (int row = 0; row < Fpload1.Sheets[0].RowCount; row++)
                {
                    int checkval = Convert.ToInt32(Fpload1.Sheets[0].Cells[row, 1].Value);
                    if (checkval == 1)
                    {
                        dep = Convert.ToString(Fpload1.Sheets[0].Cells[row, 2].Tag);
                        acr = Convert.ToString(Fpload1.Sheets[0].Cells[row, 3].Text);
                        stno = Convert.ToString(Fpload1.Sheets[0].Cells[row, 4].Text);
                        siz = Convert.ToString(Fpload1.Sheets[0].Cells[row, 5].Text);
                        if (acr != "" && stno != "" && siz != "" && dep != "")
                        {
                            sqld = "delete from InwardCodeSettings where CollegeCode='" + Convert.ToString(collegecode) + "' and FromDate='" + date.ToString("MM/dd/yyyy") + "' and DeptCode='" + dep + "'";
                            query = d2.update_method_wo_parameter(sqld, "Text");
                        }
                    }
                }
                if (query != 0)
                {
                    Divdelete.Visible = true;
                    Label3.Visible = true;
                    Label3.Text = "Deleted Successfully";
                    Div3.Visible = false;
                    surediv.Visible = false;
                }
                else
                {
                    Label3.Text = "Not Saved";
                }

            }

        }
        catch (Exception ex) { }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            surediv.Visible = true;
            lbl_sure.Text = "Do you want to delete this record?";

        }
        catch
        {
        }
    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        Div3.Visible = false;
    }

    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;

        lbl_sure.Text = "";

    }
    # endregion

    #region spread
    private DataSet codesetting()
    {
        DataSet dscode = new DataSet();
        string selq = string.Empty;
        try
        {


            selq = "select i.InwardAcr,InwardSize,InwardStNo,CollegeCode,FromTime,FromDate,hr.dept_name from InwardCodeSettings i ,hrdept_master hr";

            dscode.Clear();
            dscode = da.select_method_wo_parameter(selq, "Text");


            //dscode.Tables[0].DefaultView.RowFilter = " dep='" + dep + "' ";
            //DataTable dtroom = dscode.Tables[0].DefaultView.ToTable();

        }
        catch { }
        return dscode;

    }

    private void loadspreaddetails(DataSet ds)
    {
        DataSet dscostm = new DataSet();
        try
        {

            Fpload1.Visible = true;
            divtable.Visible = true;
            Fpload1.Sheets[0].RowCount = 1;
            Fpload1.Sheets[0].ColumnCount = 6;
            Fpload1.CommandBar.Visible = false;
            //Fpload1.Sheets[0].AutoPostBack = true;
            Fpload1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpload1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            Fpload1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            int sno = 0;

            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chk.AutoPostBack = true;
            chkall.AutoPostBack = false;
            Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].CellType = chk;
            Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "SNo";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = false;
            Fpload1.Sheets[0].Columns[0].Width = 40;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = false;

            Fpload1.Sheets[0].Columns[1].Width = 40;


            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 2].Locked = false;
            Fpload1.Sheets[0].Columns[2].Width = 150;


            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Acronym";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 3].Locked = false;
            Fpload1.Sheets[0].Columns[3].Width = 150;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "StartNo";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 4].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 4].Locked = false;
            Fpload1.Sheets[0].Columns[4].Width = 150;

            Fpload1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Size";
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 5].VerticalAlign = VerticalAlign.Bottom;
            Fpload1.Sheets[0].ColumnHeader.Cells[0, 5].Locked = false;
            Fpload1.Sheets[0].Columns[5].Width = 150;

            Fpload1.Height = 900;
            Fpload1.Width = 700;

            txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_prvdate.Text = DateTime.Now.ToString("dd/MM/yyyy");


            for (int deptname = 0; deptname < cbl_dept.Items.Count; deptname++)
            {
                if (cbl_dept.Items[deptname].Selected)
                {
                    dep = Convert.ToString(cbl_dept.Items[deptname].Text);
                    dep1 = Convert.ToString(cbl_dept.Items[deptname].Value);
                    Fpload1.Sheets[0].RowCount++;
                    sno++;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].Text = select;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].Text = dep;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].Tag= Convert.ToString(cbl_dept.Items[deptname].Value);
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].CellType = chk;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 5].CellType = txtCell;


                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;


                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                    Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;


                    if (dep1 != "")
                    {
                        string selQ1 = "select InwardAcr,InwardSize,InwardStNo from InwardCodeSettings where DeptCode='" + dep1 + "'";
                        dscostm.Clear();
                        dscostm = d2.select_method_wo_parameter(selQ1, "Text");
                        if (dscostm.Tables.Count > 0 && dscostm.Tables[0].Rows.Count > 0)
                        {

                            Acroynm = Convert.ToString(dscostm.Tables[0].Rows[0]["InwardAcr"]).Trim();
                            startno = Convert.ToString(dscostm.Tables[0].Rows[0]["InwardStNo"]).Trim();
                            size = Convert.ToString(dscostm.Tables[0].Rows[0]["InwardSize"]).Trim();



                            Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].Text = Acroynm;
                            Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 4].Text = startno;
                            Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 5].Text = size;
                            Fpload1.Sheets[0].Cells[deptname + 1, 1].Value = 1;
                        }
                        else
                        {

                            Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 3].Text = "";
                            Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 4].Text = "";
                            Fpload1.Sheets[0].Cells[Fpload1.Sheets[0].RowCount - 1, 5].Text = "";
                            // Fpload1.Sheets[0].Cells[deptname + 1, 1].Value = 0;
                        }
                    }


                }

            }


            Fpload1.Sheets[0].PageSize = Fpload1.Sheets[0].RowCount;
            Fpload1.SaveChanges();
            Fpload1.Visible = true;
            btnSave.Visible = true;
            btn_update.Visible = true;
            btn_delete.Visible = true;

        }
        catch
        {
        }
    }
    #endregion

    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
        }
        catch { }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        Div1.Visible = false;

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        // popwindow1.Visible = false;
        Divdelete.Visible = false;

        Div3.Visible = false;
        surediv.Visible = false;

    }
    #endregion


}