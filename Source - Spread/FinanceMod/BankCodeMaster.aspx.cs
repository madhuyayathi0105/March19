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

public partial class BankCodeMaster : System.Web.UI.Page
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

    bool fromDropDown = false;

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
            if (!IsPostBack)
            {
                setLabelText();
                bindcollege();
                if (ddlcol.Items.Count > 0)
                {
                    collegecode = ddlcol.SelectedItem.Value;
                }
                else collegecode = "0";
                BindGridview();
                bindheadername();
                bindheadernamePrev();
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
        catch (Exception ex) { }
    }

    protected void loadsetting()
    {
        try
        {
            if (rblCommonHeader.SelectedIndex == 0)
            {
                dt = new DateTime();
                string selquery = "";
                string selectquery = "";
                selquery = "select top 1 * from FM_FinCodeSettings where CollegeCode='" + collegecode + "' and isnull(onlineRec,'')<>'1' order by FromDate desc";//ddl_PrevDate

                selectquery = "select  top 1 * from FM_FinCodeSettings where CollegeCode='" + collegecode + "' and onlineRec='1' order by FromDate desc";//FromDate='" + dt.ToString("MM/dd/yyyy") + "'
                if (fromDropDown)
                {
                    split = ddl_PrevDate.SelectedItem.Text.Split('/');
                    dt = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]);
                    TimeSpan time = dt.TimeOfDay;
                    selquery = "select * from FM_FinCodeSettings where FromDate='" + dt.ToString("MM/dd/yyyy") + "' and CollegeCode='" + collegecode + "' and isnull(onlineRec,'')<>'1' order by FromDate desc";
                    selectquery = "select  top 1 * from FM_FinCodeSettings where  FromDate='" + dt.ToString("MM/dd/yyyy") + "' and CollegeCode='" + collegecode + "' and onlineRec='1' order by FromDate desc";
                }
                else
                {
                    string selectq = "select distinct CONVERT(varchar(10), FromDate,103) as newdate,FromDate from FM_FinCodeSettings where CollegeCode='" + collegecode + "'  order by FromDate desc";//and isnull(onlineRec,'')<>'1'
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

                DataSet dsOnline = new DataSet();
                dsOnline = d2.select_method_wo_parameter(selectquery, "Text");
                string onlineRecpAcr = "";
                string onlineRecpStNo = "";
                string onlineRecpSize = "";
                string GeneralRecpAcr = "";
                string GeneralRecpStNo = "";
                string GeneralRecpSize = "";
                string VouchAcr = "";
                string VouchStNo = "";
                string VouchSize = "";
                string DupRcptAcr = "";
                string DupRcptStNo = "";
                string DupRcptSize = "";
                string DataImportAcr = "";
                string DataImportStNo = "";
                string DataImportSize = "";
                string ChallanAcr = "";
                string ChallanStNo = "";
                string ChallanSize = "";
                string JournalAcr = "";
                string JournalStNo = "";
                string JournalSize = "";
                string ScholarshipAcr = "";
                string ScholarshipStNo = "";
                string ScholarshipSize = "";
                if (dsOnline.Tables[0].Rows.Count > 0)
                {
                    onlineRecpAcr = Convert.ToString(dsOnline.Tables[0].Rows[0]["RcptAcr"].ToString());
                    onlineRecpStNo = Convert.ToString(dsOnline.Tables[0].Rows[0]["RcptStNo"].ToString());
                    onlineRecpSize = Convert.ToString(dsOnline.Tables[0].Rows[0]["RcptSize"].ToString());

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GeneralRecpAcr = ds.Tables[0].Rows[0]["RcptAcr"].ToString();
                        GeneralRecpStNo = ds.Tables[0].Rows[0]["RcptStNo"].ToString();
                        GeneralRecpSize = ds.Tables[0].Rows[0]["RcptSize"].ToString();
                        VouchAcr = ds.Tables[0].Rows[0]["VouchAcr"].ToString();
                        VouchStNo = ds.Tables[0].Rows[0]["VouchStNo"].ToString();
                        VouchSize = ds.Tables[0].Rows[0]["VouchSize"].ToString();
                        DupRcptAcr = ds.Tables[0].Rows[0]["DupRcptAcr"].ToString();
                        DupRcptStNo = ds.Tables[0].Rows[0]["DupRcptStNo"].ToString();
                        DupRcptSize = ds.Tables[0].Rows[0]["DupRcptSize"].ToString();
                        DataImportAcr = ds.Tables[0].Rows[0]["DataImportAcr"].ToString();
                        DataImportStNo = ds.Tables[0].Rows[0]["DataImportStNo"].ToString();
                        DataImportSize = ds.Tables[0].Rows[0]["DataImportSize"].ToString();
                        ChallanAcr = ds.Tables[0].Rows[0]["ChallanAcr"].ToString();
                        ChallanStNo = ds.Tables[0].Rows[0]["ChallanStNo"].ToString();
                        ChallanSize = ds.Tables[0].Rows[0]["ChallanSize"].ToString();
                        JournalAcr = ds.Tables[0].Rows[0]["JournalAcr"].ToString();
                        JournalStNo = ds.Tables[0].Rows[0]["JournalStNo"].ToString();
                        JournalSize = ds.Tables[0].Rows[0]["JournalSize"].ToString();
                        ScholarshipAcr = ds.Tables[0].Rows[0]["ScholarshipAcr"].ToString();
                        ScholarshipStNo = ds.Tables[0].Rows[0]["ScholarshipStNo"].ToString();
                        ScholarshipSize = ds.Tables[0].Rows[0]["ScholarshipSize"].ToString();
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GeneralRecpAcr = ds.Tables[0].Rows[0]["RcptAcr"].ToString();
                    GeneralRecpStNo = ds.Tables[0].Rows[0]["RcptStNo"].ToString();
                    GeneralRecpSize = ds.Tables[0].Rows[0]["RcptSize"].ToString();
                    VouchAcr = ds.Tables[0].Rows[0]["VouchAcr"].ToString();
                    VouchStNo = ds.Tables[0].Rows[0]["VouchStNo"].ToString();
                    VouchSize = ds.Tables[0].Rows[0]["VouchSize"].ToString();
                    DupRcptAcr = ds.Tables[0].Rows[0]["DupRcptAcr"].ToString();
                    DupRcptStNo = ds.Tables[0].Rows[0]["DupRcptStNo"].ToString();
                    DupRcptSize = ds.Tables[0].Rows[0]["DupRcptSize"].ToString();
                    DataImportAcr = ds.Tables[0].Rows[0]["DataImportAcr"].ToString();
                    DataImportStNo = ds.Tables[0].Rows[0]["DataImportStNo"].ToString();
                    DataImportSize = ds.Tables[0].Rows[0]["DataImportSize"].ToString();
                    ChallanAcr = ds.Tables[0].Rows[0]["ChallanAcr"].ToString();
                    ChallanStNo = ds.Tables[0].Rows[0]["ChallanStNo"].ToString();
                    ChallanSize = ds.Tables[0].Rows[0]["ChallanSize"].ToString();
                    JournalAcr = ds.Tables[0].Rows[0]["JournalAcr"].ToString();
                    JournalStNo = ds.Tables[0].Rows[0]["JournalStNo"].ToString();
                    JournalSize = ds.Tables[0].Rows[0]["JournalSize"].ToString();
                    ScholarshipAcr = ds.Tables[0].Rows[0]["ScholarshipAcr"].ToString();
                    ScholarshipStNo = ds.Tables[0].Rows[0]["ScholarshipStNo"].ToString();
                    ScholarshipSize = ds.Tables[0].Rows[0]["ScholarshipSize"].ToString();
                }

                if (ds.Tables[0].Rows.Count > 0 || dsOnline.Tables[0].Rows.Count > 0)
                {
                    for (i = 0; i < old_grid.Rows.Count; i++)
                    {
                        TextBox Itemcode = (TextBox)old_grid.Rows[i].FindControl("txtacronym");
                        TextBox Itemvalue = (TextBox)old_grid.Rows[i].FindControl("txtstartno");
                        TextBox Itemsize = (TextBox)old_grid.Rows[i].FindControl("txt_size");

                        switch (i)
                        {
                            case 0:
                                Itemcode.Text = GeneralRecpAcr;
                                Itemvalue.Text = GeneralRecpStNo;
                                Itemsize.Text = GeneralRecpSize;
                                break;
                            case 1:
                                Itemcode.Text = VouchAcr;
                                Itemvalue.Text = VouchStNo;
                                Itemsize.Text = VouchSize;
                                break;
                            case 2:
                                Itemcode.Text = DupRcptAcr;
                                Itemvalue.Text = DupRcptStNo;
                                Itemsize.Text = DupRcptSize;
                                break;
                            case 3:
                                Itemcode.Text = DataImportAcr;
                                Itemvalue.Text = DataImportStNo;
                                Itemsize.Text = DataImportSize;
                                break;
                            case 4:
                                Itemcode.Text = ChallanAcr;
                                Itemvalue.Text = ChallanStNo;
                                Itemsize.Text = ChallanSize;
                                break;
                            case 5:
                                Itemcode.Text = JournalAcr;
                                Itemvalue.Text = JournalStNo;
                                Itemsize.Text = JournalSize;
                                break;
                            case 6:
                                Itemcode.Text = ScholarshipAcr;
                                Itemvalue.Text = ScholarshipStNo;
                                Itemsize.Text = ScholarshipSize;
                                break;
                            case 7:
                                Itemcode.Text = onlineRecpAcr.ToString();
                                Itemvalue.Text = onlineRecpStNo.ToString();
                                Itemsize.Text = onlineRecpSize.ToString();
                                break;
                        }
                    }
                    string acchead = ds.Tables[0].Rows[0]["IsHeader"].ToString();
                    string headid = ds.Tables[0].Rows[0]["HeaderFK"].ToString();
                    int count = 0;
                    split = ddl_PrevDate.SelectedItem.Text.Split('/');
                    dt = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]);
                    string selacc = "select distinct a.HeaderPK,a.HeaderName,f.HeaderFK from FM_HeaderMaster a,FM_FinCodeSettings f where HeaderName is not null and a.HeaderPK=f.HeaderFK and f.FromDate='" + dt.ToString("MM/dd/yyyy") + "' and f.IsHeader='" + acchead + "' and f.CollegeCode='" + collegecode + "' and isnull(onlineRec,'')<>'1'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selacc, "Text");
                    if (acchead.Trim() != null)
                    {
                        //cb_forheader.Enabled = true;
                        //cb_forheader.Checked = false;
                        bindaccheader();
                        // txt_select.Enabled = true;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < cbl_header.Items.Count; j++)
                            {
                                cbl_header.Items[j].Selected = false;
                            }
                            for (int j = 0; j < cbl_header.Items.Count; j++)
                            {
                                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                {
                                    if (Convert.ToString(cbl_header.Items[j].Value) == Convert.ToString(ds.Tables[0].Rows[k]["HeaderPK"]))
                                    {
                                        cbl_header.Items[j].Selected = true;
                                        count = count + 1;
                                    }
                                }
                            }
                            if (cbl_header.Items.Count == ds.Tables[0].Rows.Count)
                            {
                                txt_select.Text = "Header Name(" + count + ")";
                            }
                        }
                        else
                        {
                            //cb_forheader.Enabled = true;
                            //cb_forheader.Checked = false;
                            //txt_select.Text = "--Select--";
                            //txt_select.Enabled = false;
                            //btnGo.Enabled = false;
                            //btnSaveHeader.Enabled = false;
                        }
                    }
                    else
                    {
                        //cb_forheader.Enabled = true;
                        //cb_forheader.Checked = false;
                        //txt_select.Text = "--Select--";
                        //txt_select.Enabled = false;
                        //btnGo.Enabled = false;
                        //btnSaveHeader.Enabled = false;
                    }
                }
            }

        }
        catch
        {

        }
    }

    public void BindGridview()
    {
        ArrayList addnew = new ArrayList();
        DataTable dt = new DataTable();
        DataRow dr;
        if (rblCommonHeader.SelectedIndex == 0)
        {
            addnew.Add("Receipt Code");
            addnew.Add("Voucher Code");
            addnew.Add("Duplicate Receipt Code");
            addnew.Add("Adjustment Receipt Code");
            addnew.Add("Challan No");
            addnew.Add("Journal No");
            addnew.Add("Scholarship No");
            addnew.Add("Online Receipt Code");
            ug_grid.Visible = true;
            old_grid.Visible = true;
            dt.Columns.Add("Dummy");
            dt.Columns.Add("Dummy1");
            dt.Columns.Add("Dummy2");
            dt.Columns.Add("Dummy3");
            dt.Columns.Add("Dummy4");
            dt.Columns.Add("Dummay5");
            dt.Columns.Add("Dummay6");
            dt.Columns.Add("Dummay7");
            dt.Columns.Add("Dummay8");
            for (row = 0; row < addnew.Count; row++)
            {
                dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = Convert.ToString(addnew[row]);
                dr[2] = "";
                dr[3] = "";
                dr[4] = "";
                dr[5] = "";
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count > 0)
            {
                ug_grid.DataSource = dt;
                ug_grid.DataBind();
                old_grid.DataSource = dt;
                old_grid.DataBind();
            }
            txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_prvdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    protected void ddl_PrevDate_OnSelectedIndexChange(object sender, EventArgs e)
    {
        fromDropDown = true;
        loadsetting();
    }

    public void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string link = "select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code='" + collegecode + "' and Finusercode ='" + usercode + "' and Linkvalue in (select finyearpk from FM_FinYearMaster)";
            string getfinid = d2.GetFunction(link);
            if (getfinid.Trim() != "" && getfinid.Trim() != "0")
            {
                string[] nReceiptcode = new string[3];
                string[] nVoucher = new string[3];
                string[] nDuprecpt = new string[3];
                string[] nAdrecpt = new string[3];
                string[] nChlanno = new string[3];
                string[] nJournalno = new string[3];
                string[] nScholarno = new string[3];
                string[] nOnlineRecno = new string[3];
                string isacc = "";

                string firstdate = Convert.ToString(txt_frmdate.Text);
                dt = new DateTime();
                split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]);
                DateTime date = dt.Date;
                DateTime currdate = DateTime.Now.Date;
                string currtime = DateTime.Now.ToLongTimeString();

                if (currdate < dt)
                {
                    txtdateerr.Visible = true;
                    Mainpage.Visible = false;
                    btn_save.Visible = false;
                    btn_reset.Visible = false;
                    btn_exit.Visible = false;
                    ug_grid.Visible = false;
                    old_grid.Visible = false;
                    div1.Visible = false;
                    txtdateerr.Text = "Date Must be Current Date";
                }
                else if (currdate > dt)
                {
                    txtdateerr.Visible = true;
                    Mainpage.Visible = false;
                    btn_save.Visible = false;
                    btn_reset.Visible = false;
                    btn_exit.Visible = false;
                    ug_grid.Visible = false;
                    old_grid.Visible = false;
                    div1.Visible = false;
                    txtdateerr.Text = "Date Must be Current Date";
                }
                else
                {
                    txtdateerr.Visible = false;
                    Mainpage.Visible = true;
                    btn_save.Visible = true;
                    btn_reset.Visible = true;
                    btn_exit.Visible = true;
                    ug_grid.Visible = true;
                    old_grid.Visible = true;

                    div1.Visible = true;
                    for (i = 0; i < ug_grid.Rows.Count; i++)
                    {
                        TextBox txtItem = (TextBox)ug_grid.Rows[i].FindControl("txt_acronym");
                        TextBox txtItem1 = (TextBox)ug_grid.Rows[i].FindControl("txt_startno");
                        TextBox txtItem2 = (TextBox)ug_grid.Rows[i].FindControl("txt_size");

                        switch (i)
                        {
                            case 0:
                                nReceiptcode[0] = txtItem.Text.ToUpper();
                                nReceiptcode[1] = txtItem1.Text.ToUpper();
                                nReceiptcode[2] = txtItem2.Text.ToUpper();

                                txtItem.Text = "";
                                txtItem1.Text = "";
                                txtItem2.Text = "";
                                break;
                            case 1:
                                nVoucher[0] = txtItem.Text.ToUpper();
                                nVoucher[1] = txtItem1.Text.ToUpper();
                                nVoucher[2] = txtItem2.Text.ToUpper();

                                txtItem.Text = "";
                                txtItem1.Text = "";
                                txtItem2.Text = "";
                                break;
                            case 2:
                                nDuprecpt[0] = txtItem.Text.ToUpper();
                                nDuprecpt[1] = txtItem1.Text.ToUpper();
                                nDuprecpt[2] = txtItem2.Text.ToUpper();

                                txtItem.Text = "";
                                txtItem1.Text = "";
                                txtItem2.Text = "";
                                break;
                            case 3:
                                nAdrecpt[0] = txtItem.Text.ToUpper();
                                nAdrecpt[1] = txtItem1.Text.ToUpper();
                                nAdrecpt[2] = txtItem2.Text.ToUpper();

                                txtItem.Text = "";
                                txtItem1.Text = "";
                                txtItem2.Text = "";
                                break;
                            case 4:
                                nChlanno[0] = txtItem.Text.ToUpper();
                                nChlanno[1] = txtItem1.Text.ToUpper();
                                nChlanno[2] = txtItem2.Text.ToUpper();

                                txtItem.Text = "";
                                txtItem1.Text = "";
                                txtItem2.Text = "";
                                break;
                            case 5:
                                nJournalno[0] = txtItem.Text.ToUpper();
                                nJournalno[1] = txtItem1.Text.ToUpper();
                                nJournalno[2] = txtItem2.Text.ToUpper();

                                txtItem.Text = "";
                                txtItem1.Text = "";
                                txtItem2.Text = "";
                                break;
                            case 6:
                                nScholarno[0] = txtItem.Text.ToUpper();
                                nScholarno[1] = txtItem1.Text.ToUpper();
                                nScholarno[2] = txtItem2.Text.ToUpper();

                                txtItem.Text = "";
                                txtItem1.Text = "";
                                txtItem2.Text = "";
                                break;
                            case 7://Added by saranya
                                nOnlineRecno[0] = txtItem.Text.ToUpper();
                                nOnlineRecno[1] = txtItem1.Text.ToUpper();
                                nOnlineRecno[2] = txtItem2.Text.ToUpper();

                                txtItem.Text = "";
                                txtItem1.Text = "";
                                txtItem2.Text = "";
                                break;
                        }
                    }
                    string onlineRecpt = nOnlineRecno[0].ToString();
                    bool check = false;
                    if (rblCommonHeader.SelectedIndex == 1)
                    {
                        for (int i = 0; i < cbl_header.Items.Count; i++)
                        {
                            if (cbl_header.Items[i].Selected == true)
                            {
                                isacc = "1";
                                string insquery = "insert into FM_FinCodeSettings(RcptAcr,RcptStNo,RcptSize,VouchAcr,VouchStNo,VouchSize,DupRcptAcr,DupRcptStNo,DupRcptSize,DataImportAcr,DataImportStNo,DataImportSize,ChallanAcr,ChallanStNo,ChallanSize,FromDate,FromTime,IsHeader,HeaderFK,CollegeCode,FinyearFk) values ('" + nReceiptcode[0].ToUpper() + "','" + Convert.ToString(nReceiptcode[1]) + "','" + Convert.ToString(nReceiptcode[2]) + "','" + nVoucher[0].ToUpper() + "','" + Convert.ToString(nVoucher[1]) + "','" + Convert.ToString(nVoucher[2]) + "','" + nDuprecpt[0].ToUpper() + "','" + Convert.ToString(nDuprecpt[1]) + "','" + Convert.ToString(nDuprecpt[2]) + "','" + nAdrecpt[0].ToUpper() + "','" + Convert.ToString(nAdrecpt[1]) + "','" + Convert.ToString(nAdrecpt[2]) + "','" + nChlanno[0].ToUpper() + "','" + Convert.ToString(nChlanno[1]) + "','" + Convert.ToString(nChlanno[2]) + "','" + date.ToString("MM/dd/yyyy") + "','" + currtime + "','" + isacc + "','" + cbl_header.Items[i].Value + "','" + Convert.ToString(collegecode) + "','" + getfinid + "')";
                                int inscount = d2.update_method_wo_parameter(insquery, "Text");
                                if (inscount > 0)
                                {
                                    check = true;
                                }
                            }
                        }
                        if (check == true)
                        {
                            loadsetting();
                            imgdiv2.Visible = true;
                            lbl_alerterr.Text = "Saved Successfully";
                            //cb_forheader.Checked = false;
                            //txt_select.Enabled = false;
                            //txt_select.Text = "--Select--";
                        }
                    }
                    else if (rblCommonHeader.SelectedIndex == 0)
                    {
                        string insquery1 = "";
                        isacc = "0";
                        if (!string.IsNullOrEmpty(onlineRecpt))
                        {
                            isacc = "0";
                            //string insquery1 = "insert into FM_FinCodeSettings(RcptAcr,RcptStNo,RcptSize,VouchAcr,VouchStNo,VouchSize,DupRcptAcr,DupRcptStNo,DupRcptSize,DataImportAcr,DataImportStNo,DataImportSize,ChallanAcr,ChallanStNo,ChallanSize,FromDate,FromTime,IsHeader,CollegeCode,FinyearFk,JournalAcr,JournalStNo,JournalSize,ScholarshipAcr,ScholarshipStNo,ScholarshipSize,onlineRec) values ('" + nReceiptcode[0].ToUpper() + "','" + nReceiptcode[1] + "','" + nReceiptcode[2] + "','','','','','','','','','','','','','" + date.ToString("MM/dd/yyyy") + "','" + currtime + "','" + isacc + "','" + Convert.ToString(collegecode) + "','" + getfinid + "','','','','','','','1')";
                            insquery1 = "insert into FM_FinCodeSettings(RcptAcr,RcptStNo,RcptSize,VouchAcr,VouchStNo,VouchSize,DupRcptAcr,DupRcptStNo,DupRcptSize,DataImportAcr,DataImportStNo,DataImportSize,ChallanAcr,ChallanStNo,ChallanSize,FromDate,FromTime,IsHeader,CollegeCode,FinyearFk,JournalAcr,JournalStNo,JournalSize,ScholarshipAcr,ScholarshipStNo,ScholarshipSize,onlineRec) values ('" + nOnlineRecno[0].ToUpper() + "','" + nOnlineRecno[1] + "','" + nOnlineRecno[2] + "','" + nVoucher[0].ToUpper() + "','" + nVoucher[1] + "','" + nVoucher[2] + "','" + nDuprecpt[0].ToUpper() + "','" + nDuprecpt[1] + "','" + nDuprecpt[2] + "','" + nAdrecpt[0].ToUpper() + "','" + nAdrecpt[1] + "','" + nAdrecpt[2] + "','" + nChlanno[0].ToUpper() + "','" + nChlanno[1] + "','" + nChlanno[2] + "','" + date.ToString("MM/dd/yyyy") + "','" + currtime + "','" + isacc + "','" + Convert.ToString(collegecode) + "','" + getfinid + "','" + Convert.ToString(nJournalno[0]) + "','" + Convert.ToString(nJournalno[1]) + "','" + Convert.ToString(nJournalno[2]) + "','" + Convert.ToString(nScholarno[0]) + "','" + Convert.ToString(nScholarno[1]) + "','" + Convert.ToString(nScholarno[2]) + "','1')";
                            int inscount = d2.update_method_wo_parameter(insquery1, "Text");
                            if (inscount != 0)
                            {
                                loadsetting();
                                imgdiv2.Visible = true;
                                lbl_alerterr.Text = "Saved Successfully";
                                //cb_forheader.Checked = false;
                                //txt_select.Enabled = false;
                                //txt_select.Text = "--Select--";
                            }
                        }
                        else
                        {
                            insquery1 = "insert into FM_FinCodeSettings(RcptAcr,RcptStNo,RcptSize,VouchAcr,VouchStNo,VouchSize,DupRcptAcr,DupRcptStNo,DupRcptSize,DataImportAcr,DataImportStNo,DataImportSize,ChallanAcr,ChallanStNo,ChallanSize,FromDate,FromTime,IsHeader,CollegeCode,FinyearFk,JournalAcr,JournalStNo,JournalSize,ScholarshipAcr,ScholarshipStNo,ScholarshipSize) values ('" + nReceiptcode[0].ToUpper() + "','" + nReceiptcode[1] + "','" + nReceiptcode[2] + "','" + nVoucher[0].ToUpper() + "','" + nVoucher[1] + "','" + nVoucher[2] + "','" + nDuprecpt[0].ToUpper() + "','" + nDuprecpt[1] + "','" + nDuprecpt[2] + "','" + nAdrecpt[0].ToUpper() + "','" + nAdrecpt[1] + "','" + nAdrecpt[2] + "','" + nChlanno[0].ToUpper() + "','" + nChlanno[1] + "','" + nChlanno[2] + "','" + date.ToString("MM/dd/yyyy") + "','" + currtime + "','" + isacc + "','" + Convert.ToString(collegecode) + "','" + getfinid + "','" + Convert.ToString(nJournalno[0]) + "','" + Convert.ToString(nJournalno[1]) + "','" + Convert.ToString(nJournalno[2]) + "','" + Convert.ToString(nScholarno[0]) + "','" + Convert.ToString(nScholarno[1]) + "','" + Convert.ToString(nScholarno[2]) + "')";
                            int inscount = d2.update_method_wo_parameter(insquery1, "Text");
                            if (inscount != 0)
                            {
                                loadsetting();
                                imgdiv2.Visible = true;
                                lbl_alerterr.Text = "Saved Successfully";
                                //cb_forheader.Checked = false;
                                //txt_select.Enabled = false;
                                //txt_select.Text = "--Select--";
                            }
                        }
                    }

                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Finance Year Not Set";
            }
        }
        catch
        {

        }
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Finance.aspx");
    }

    protected void btn_reset_Click(object sender, EventArgs e)
    {
        clearGridview();
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
                Mainpage.Visible = true;
                btn_save.Visible = true;
                btn_reset.Visible = true;
                btn_exit.Visible = true;
                ug_grid.Visible = true;
                old_grid.Visible = true;
                div1.Visible = true;
            }
        }
        catch
        {

        }
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

    protected void cb_header_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_header, cb_header, txt_select, "Header");
        BindHeaderGridview();
    }

    protected void cbl_header_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_header, cb_header, txt_select, "Header");
        BindHeaderGridview();
    }

    public void bindaccheader()
    {
        try
        {
            split = ddl_PrevDate.SelectedItem.Text.Split('/');
            dt = Convert.ToDateTime(split[1] + '/' + split[0] + '/' + split[2]);
            //string selquery = "select distinct a.HeaderPK,a.HeaderName,f.HeaderFK from FM_HeaderMaster a,FM_FinCodeSettings f where HeaderName is not null and a.HeaderPK=f.HeaderFK and f.FromDate='" + dt.ToString("MM/dd/yyyy") + "' and f.CollegeCode='" + collegecode + "' ";
            string selquery = "select distinct a.HeaderPK,a.HeaderName,f.HeaderFK from FM_HeaderMaster a,FM_FinCodeSettings f where HeaderName is not null and a.HeaderPK=f.HeaderFK and f.FromDate='" + dt.ToString("MM/dd/yyyy") + "' and f.CollegeCode='" + collegecode + "' and a.HeaderPK not in (select distinct Headerfk from FM_HeaderFinCodeSettingsDet)";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_header.DataSource = ds;
                cbl_header.DataTextField = "HeaderName";
                cbl_header.DataValueField = "HeaderPK";
                cbl_header.DataBind();
            }
        }
        catch
        {

        }
    }

    public void bindheadername()
    {
        try
        {
            if (rblCommonHeader.SelectedIndex == 1)//if (cb_forheader.Checked == true)
            {
                txt_select.Enabled = true;

                string getfinid = d2.getCurrentFinanceYear(usercode, collegecode);
                if (getfinid.Trim() != "" && getfinid.Trim() != "0")
                {
                    string query = "select distinct HeaderPK,HeaderName from FM_HeaderMaster H,FS_HeaderPrivilage P where  H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " and HeaderName is not null and h.CollegeCode='" + collegecode + "'  and HeaderPK not in (select HeaderFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and CollegeCode=" + collegecode + " and FinyearFK=" + getfinid + ")";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(query, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_header.DataSource = ds;
                        cbl_header.DataTextField = "HeaderName";
                        cbl_header.DataValueField = "HeaderPK";
                        cbl_header.DataBind();

                        if (cbl_header.Items.Count > 0)
                        {
                            for (int i = 0; i < cbl_header.Items.Count; i++)
                            {
                                cbl_header.Items[i].Selected = true;
                            }
                            cb_header.Checked = true;
                            txt_select.Text = "Header Name(" + cbl_header.Items.Count + ")";
                        }
                    }
                    else
                    {
                        txt_select.Text = "--Select--";
                    }
                }
            }
            else
            {
                txt_select.Enabled = false;
            }
        }
        catch
        {

        }
    }

    public void bindheadernamePrev()
    {
        try
        {
            gridHeaderPrev.DataSource = null;
            gridHeaderPrev.DataBind();
            gridHeaderPrev.Visible = false;
            trHeaderSetPrev.Visible = false;
            if (rblCommonHeader.SelectedIndex == 1)//if (cb_forheader.Checked == true)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("HeaderSettingPk");
                dt.Columns.Add("hdrCode");
                dt.Columns.Add("hdrNames");
                dt.Columns.Add("rcptAcr");
                dt.Columns.Add("rcptStno");
                dt.Columns.Add("rcptSize");
                dt.Columns.Add("chlnAcr");
                dt.Columns.Add("chlnStno");
                dt.Columns.Add("chlnSize");
                dt.Columns.Add("voucAcr");
                dt.Columns.Add("voucStno");
                dt.Columns.Add("voucSize");

                string getfinid = d2.getCurrentFinanceYear(usercode, collegecode);
                if (getfinid.Trim() != "" && getfinid.Trim() != "0")
                {
                    string query = "select distinct HeaderPK,HeaderName from FM_HeaderMaster H,FS_HeaderPrivilage P where  H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " and HeaderName is not null and h.CollegeCode='" + collegecode + "'  and HeaderPK in (select HeaderFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and CollegeCode=" + collegecode + " and FinyearFK=" + getfinid + ")";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(query, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string uHdr = string.Empty;
                        for (int uhdr = 0; uhdr < ds.Tables[0].Rows.Count; uhdr++)
                        {
                            if (uHdr == string.Empty)
                            {
                                uHdr = Convert.ToString(ds.Tables[0].Rows[uhdr][0]);
                            }
                            else
                            {
                                uHdr += "," + Convert.ToString(ds.Tables[0].Rows[uhdr][0]);
                            }
                        }
                        string Q = "select distinct Headersettingfk from fm_headerfincodesettingsdet where HeaderFk in (" + uHdr + ") ;select  Headersettingfk, headerfk, headername, Rcptacr, RcptStno, Rcptsize, ChallanAcr, ChallanStNo, ChallanSize, VouchAcr, VouchStNo, VouchSize from fm_headerfincodesettings f, fm_headerfincodesettingsdet hf,Fm_HeaderMaster h where hf.headerfk=h.headerpk and f.headersettingpk=hf.headersettingfk and hf.HeaderFk in (" + uHdr + ");";
                        DataSet dsHdrs = d2.select_method_wo_parameter(Q, "Text");
                        if (dsHdrs.Tables.Count > 1 && dsHdrs.Tables[0].Rows.Count > 0 && dsHdrs.Tables[1].Rows.Count > 0)
                        {
                            for (int hd = 0; hd < dsHdrs.Tables[0].Rows.Count; hd++)
                            {
                                dsHdrs.Tables[1].DefaultView.RowFilter = "Headersettingfk='" + Convert.ToString(dsHdrs.Tables[0].Rows[hd]["Headersettingfk"]) + "'";
                                DataView dv = dsHdrs.Tables[1].DefaultView;
                                if (dv.Count > 0)
                                {
                                    string hdrCode = string.Empty;
                                    string hdrNames = string.Empty;
                                    for (int dvCnt = 0; dvCnt < dv.Count; dvCnt++)
                                    {
                                        if (hdrCode == string.Empty)
                                        {
                                            hdrCode = Convert.ToString(dv[dvCnt]["headerfk"]);
                                            hdrNames = Convert.ToString(dv[dvCnt]["headername"]);
                                        }
                                        else
                                        {
                                            hdrCode += "," + Convert.ToString(dv[dvCnt]["headerfk"]);
                                            hdrNames += "," + Convert.ToString(dv[dvCnt]["headername"]);
                                        }
                                    }
                                    DataRow dr = dt.NewRow();
                                    dr["HeaderSettingPk"] = Convert.ToString(dv[0]["HeaderSettingFk"]);
                                    dr["hdrCode"] = hdrCode;
                                    dr["hdrNames"] = hdrNames;
                                    dr["rcptAcr"] = Convert.ToString(dv[0]["rcptAcr"]);
                                    dr["rcptStNo"] = Convert.ToString(dv[0]["rcptStNo"]);
                                    dr["rcptSize"] = Convert.ToString(dv[0]["rcptSize"]);
                                    dr["chlnAcr"] = Convert.ToString(dv[0]["challanAcr"]);
                                    dr["chlnStNo"] = Convert.ToString(dv[0]["challanStNo"]);
                                    dr["chlnSize"] = Convert.ToString(dv[0]["challanSize"]);
                                    dr["voucAcr"] = Convert.ToString(dv[0]["vouchAcr"]);
                                    dr["voucStNo"] = Convert.ToString(dv[0]["vouchStNo"]);
                                    dr["voucSize"] = Convert.ToString(dv[0]["vouchSize"]);
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        gridHeaderPrev.DataSource = dt;
                        gridHeaderPrev.DataBind();
                        gridHeaderPrev.Visible = true;
                        trHeaderSetPrev.Visible = true;
                    }
                }
            }
        }
        catch
        {

        }
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

    public void clearGridview()
    {
        ArrayList addnew = new ArrayList();
        DataTable dt = new DataTable();
        DataRow dr;
        if (rblCommonHeader.SelectedIndex == 0)
        {
            addnew.Add("Receipt Code");
            addnew.Add("Voucher Code");
            addnew.Add("Duplicate Receipt Code");
            addnew.Add("Adjustment Receipt Code");
            addnew.Add("Challan No");
            addnew.Add("Journal No");
            addnew.Add("Online Receipt Code");
            ug_grid.Visible = true;

            dt.Columns.Add("Dummy");
            dt.Columns.Add("Dummy1");
            dt.Columns.Add("Dummy2");
            dt.Columns.Add("Dummy3");
            dt.Columns.Add("Dummy4");
            dt.Columns.Add("Dummay5");
            dt.Columns.Add("Dummay6");
            dt.Columns.Add("Dummay7");

            for (row = 0; row < addnew.Count; row++)
            {
                dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = Convert.ToString(addnew[row]);
                dr[2] = "";
                dr[3] = "";
                dr[4] = "";
                dr[5] = "";
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count > 0)
            {
                ug_grid.DataSource = dt;
                ug_grid.DataBind();
            }

            txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_prvdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        if (rblCommonHeader.SelectedIndex == 2)
        {
            addnew.Add("Receipt Code");
            //addnew.Add("Voucher Code");
            //addnew.Add("Duplicate Receipt Code");
            //addnew.Add("Adjustment Receipt Code");
            //addnew.Add("Challan No");
            //addnew.Add("Journal No");
            ug_grid.Visible = true;

            dt.Columns.Add("Dummy");
            dt.Columns.Add("Dummy1");
            //dt.Columns.Add("Dummy2");
            //dt.Columns.Add("Dummy3");
            //dt.Columns.Add("Dummy4");
            //dt.Columns.Add("Dummay5");
            //dt.Columns.Add("Dummay6");

            for (row = 0; row < addnew.Count; row++)
            {
                dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = Convert.ToString(addnew[row]);
                //dr[2] = "";
                //dr[3] = "";
                //dr[4] = "";
                //dr[5] = "";
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count > 0)
            {
                ug_grid.DataSource = dt;
                ug_grid.DataBind();
            }

            txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_prvdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        loadsetting();
        //cb_forheader.Checked = false;
        //txt_select.Enabled = false;
        //txt_select.Text = "--Select--";
        //cb_header.Checked = false;
        //cbl_header.Items.Clear();
        btnGo.Enabled = false;
        btnSaveHeader.Enabled = false;
        BindHeaderGridview();
    }

    protected void btn_errclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        loadsetting();
        BindHeaderGridview();
    }

    //Code Added by Idhris -- 14-03-2016

    protected void rblCommonHeader_Changed(object sender, EventArgs e)
    {
        trHeaderSet.Visible = false;
        gridHeaderPrev.Visible = false;
        trCommonSet.Visible = false;
        tdBtns.Visible = false;
        tdHdrBtns.Visible = false;
        if (rblCommonHeader.SelectedIndex == 0)
        {
            trCommonSet.Visible = true;
            tdBtns.Visible = true;
        }
        else if (rblCommonHeader.SelectedIndex == 1)
        {
            trHeaderSet.Visible = true;
            gridHeaderPrev.Visible = true;
            tdHdrBtns.Visible = true;
        }
        grid_header.DataSource = null;
        grid_header.DataBind();
        grid_header.Visible = false;
        if (rblCommonHeader.SelectedIndex == 1)
        {
            bindheadername();
            bindheadernamePrev();
            BindHeaderGridview();
        }
        else
        {
            bindheadername();
            bindheadernamePrev();
            BindHeaderGridview();
            loadsetting();
        }
    }

    protected void btnGo_OnClick(object sender, EventArgs e)
    {
        BindHeaderGridview();
    }

    protected void btnSaveHeader_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = true;
        try
        {
            string getfinid = d2.getCurrentFinanceYear(usercode, collegecode);

            if (getfinid.Trim() != "" && getfinid.Trim() != "0")
            {
                string[] rcpt = new string[3];
                string[] vouc = new string[3];
                string[] chal = new string[3];
                string[] onlineRcpt = new string[3];
                for (int i = 0; i < grid_header.Rows.Count; i++)
                {
                    TextBox txtAcr = (TextBox)grid_header.Rows[i].FindControl("txt_acronym");
                    TextBox txtStno = (TextBox)grid_header.Rows[i].FindControl("txt_startno");
                    TextBox txtSize = (TextBox)grid_header.Rows[i].FindControl("txt_size");

                    switch (i)
                    {
                        case 0:
                            rcpt[0] = txtAcr.Text.ToUpper();
                            rcpt[1] = txtStno.Text.ToUpper();
                            rcpt[2] = txtSize.Text.ToUpper();
                            break;
                        case 1:
                            vouc[0] = txtAcr.Text.ToUpper();
                            vouc[1] = txtStno.Text.ToUpper();
                            vouc[2] = txtSize.Text.ToUpper();
                            break;
                        case 2:
                            chal[0] = txtAcr.Text.ToUpper();
                            chal[1] = txtStno.Text.ToUpper();
                            chal[2] = txtSize.Text.ToUpper();
                            break;
                        case 3:
                            onlineRcpt[0] = txtAcr.Text.ToUpper();
                            onlineRcpt[1] = txtStno.Text.ToUpper();
                            onlineRcpt[2] = txtSize.Text.ToUpper();
                            break;
                    }
                }
                List<string> lstHeaders = GetSelectedItemsValueList(cbl_header);
                string insUpQ = "";
                string hdrSettingPk = "";
                string insUpHQ = "";
                string OnlineRecptCode = onlineRcpt[0].ToString();

                if (lstHeaders.Count > 0)
                {
                    if (!string.IsNullOrEmpty(OnlineRecptCode))//Added by saranya 
                    {
                        insUpQ = "IF NOT EXISTS (SElECT * FROM FM_HeaderFinCodeSettings WHERE  FINYEARFK='" + getfinid + "' AND COLLEGECODE='" + collegecode + "' and RecpSettingsDate='" + DateTime.Now.Date.ToString() + "' and RcptAcr='" + onlineRcpt[0] + "' and  RcptSize='" + onlineRcpt[2] + "' and RcptStNo='" + onlineRcpt[1] + "'  and VouchAcr='" + vouc[0] + "' and  VouchSize='" + vouc[2] + "' and VouchStNo='" + vouc[1] + "'  and ChallanAcr='" + chal[0] + "' and  ChallanSize='" + chal[2] + "' and ChallanStNo='" + chal[1] + "' and onlineRec='1')  INSERT INTO FM_HeaderFinCodeSettings( RcptAcr, RcptStNo, RcptSize,VouchAcr, VouchStNo, VouchSize,ChallanAcr, ChallanStNo, ChallanSize, RecpSettingsDate, FinyearFK, CollegeCode,onlineRec) VALUES ( '" + onlineRcpt[0] + "', '" + onlineRcpt[1] + "', '" + onlineRcpt[2] + "','" + vouc[0] + "', '" + vouc[1] + "', '" + vouc[2] + "','" + chal[0] + "', '" + chal[1] + "', '" + chal[2] + "', '" + DateTime.Now.Date.ToString() + "', " + getfinid + ", " + collegecode + ",'1')";
                        int upIns = d2.update_method_wo_parameter(insUpQ, "Text");
                        if (upIns > 0)
                        {
                            hdrSettingPk = d2.GetFunction("SElECT HeaderSettingPK FROM FM_HeaderFinCodeSettings WHERE  FINYEARFK='" + getfinid + "' AND COLLEGECODE='" + collegecode + "' and RecpSettingsDate='" + DateTime.Now.Date.ToString() + "' and RcptAcr='" + onlineRcpt[0] + "' and RcptSize=" + onlineRcpt[2] + " and RcptStNo=" + onlineRcpt[1] + "  and VouchAcr='" + vouc[0] + "' and  VouchSize='" + vouc[2] + "' and VouchStNo='" + vouc[1] + "'  and ChallanAcr='" + chal[0] + "' and  ChallanSize='" + chal[2] + "' and ChallanStNo='" + chal[1] + "' and onlineRec='1'").Trim();
                            if (hdrSettingPk != "0")
                            {
                                foreach (string header in lstHeaders)
                                {
                                    insUpHQ = "IF EXISTS(SELECT * FROM FM_HeaderFinCodeSettingsDet WHERE HeaderFk=" + header + " and HeaderSettingFK=" + hdrSettingPk + ") UPDATE FM_HeaderFinCodeSettingsDet SET HeaderFk=" + header + "  WHERE HeaderSettingFK=" + hdrSettingPk + " ELSE INSERT INTO FM_HeaderFinCodeSettingsDet(HeaderFK,HeaderSettingFK) VALUES(" + header + "," + hdrSettingPk + ")";
                                    d2.update_method_wo_parameter(insUpHQ, "Text");
                                }
                                lbl_alerterr.Text = "Saved Successfully";
                                bindheadername();
                                bindheadernamePrev();
                            }
                            else
                            {
                                lbl_alerterr.Text = "Not Saved";
                            }
                        }
                        else
                        {
                            lbl_alerterr.Text = "Acronym Already Exists";
                        }
                    }
                    else
                    {
                        insUpQ = "IF NOT EXISTS (SElECT * FROM FM_HeaderFinCodeSettings WHERE  FINYEARFK='" + getfinid + "' AND COLLEGECODE='" + collegecode + "' and RecpSettingsDate='" + DateTime.Now.Date.ToString() + "' and RcptAcr='" + rcpt[0] + "' and  RcptSize=" + rcpt[2] + " and RcptStNo=" + rcpt[1] + "  and VouchAcr='" + vouc[0] + "' and  VouchSize=" + vouc[2] + " and VouchStNo=" + vouc[1] + "  and ChallanAcr='" + chal[0] + "' and  ChallanSize=" + chal[2] + " and ChallanStNo=" + chal[1] + " )  INSERT INTO FM_HeaderFinCodeSettings( RcptAcr, RcptStNo, RcptSize,VouchAcr, VouchStNo, VouchSize,ChallanAcr, ChallanStNo, ChallanSize, RecpSettingsDate, FinyearFK, CollegeCode) VALUES ( '" + rcpt[0] + "', " + rcpt[1] + ", " + rcpt[2] + ",'" + vouc[0] + "', " + vouc[1] + ", " + vouc[2] + ",'" + chal[0] + "', " + chal[1] + ", " + chal[2] + ", '" + DateTime.Now.Date.ToString() + "', " + getfinid + ", " + collegecode + ")";
                        int upIns = d2.update_method_wo_parameter(insUpQ, "Text");
                        if (upIns > 0)
                        {
                            hdrSettingPk = d2.GetFunction("SElECT HeaderSettingPK FROM FM_HeaderFinCodeSettings WHERE  FINYEARFK='" + getfinid + "' AND COLLEGECODE='" + collegecode + "' and RecpSettingsDate='" + DateTime.Now.Date.ToString() + "' and RcptAcr='" + rcpt[0] + "' and RcptSize=" + rcpt[2] + " and RcptStNo=" + rcpt[1] + "  and VouchAcr='" + vouc[0] + "' and  VouchSize=" + vouc[2] + " and VouchStNo=" + vouc[1] + "  and ChallanAcr='" + chal[0] + "' and  ChallanSize=" + chal[2] + " and ChallanStNo=" + chal[1] + "").Trim();
                            if (hdrSettingPk != "0")
                            {
                                foreach (string header in lstHeaders)
                                {
                                    insUpHQ = "IF EXISTS(SELECT * FROM FM_HeaderFinCodeSettingsDet WHERE HeaderFk=" + header + " and HeaderSettingFK=" + hdrSettingPk + ") UPDATE FM_HeaderFinCodeSettingsDet SET HeaderFk=" + header + "  WHERE HeaderSettingFK=" + hdrSettingPk + " ELSE INSERT INTO FM_HeaderFinCodeSettingsDet(HeaderFK,HeaderSettingFK) VALUES(" + header + "," + hdrSettingPk + ")";
                                    d2.update_method_wo_parameter(insUpHQ, "Text");
                                }
                                lbl_alerterr.Text = "Saved Successfully";
                                bindheadername();
                                bindheadernamePrev();
                            }
                            else
                            {
                                lbl_alerterr.Text = "Not Saved";
                            }
                        }
                        else
                        {
                            lbl_alerterr.Text = "Acronym Already Exists";
                        }
                    }
                }
                else
                {
                    lbl_alerterr.Text = "Please Select Headers";
                }
            }
            else
            {
                lbl_alerterr.Text = "Financial Year Not Set";
            }
        }
        catch { lbl_alerterr.Text = "Please Try Later"; }
    }

    public void BindHeaderGridview()
    {
        grid_header.DataSource = null;
        grid_header.DataBind();
        grid_header.Visible = false;

        string getfinid = d2.getCurrentFinanceYear(usercode, collegecode);
        if (getfinid.Trim() != "" && getfinid.Trim() != "0")
        {
            //fsHSetting.Visible = true;

            DataTable dt = new DataTable();

            dt.Columns.Add("Dummy1");
            dt.Columns.Add("Dummy2");
            dt.Columns.Add("Dummy3");
            dt.Columns.Add("Dummy4");

            DataRow dr = dt.NewRow(); ;
            dr["Dummy1"] = "Receipt Code";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow(); ;
            dr1["Dummy1"] = "Challan No";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow(); ;
            dr2["Dummy1"] = "Voucher Code";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow(); ;
            dr3["Dummy1"] = "Online Receipt Code";
            dt.Rows.Add(dr3);

            grid_header.DataSource = dt;
            grid_header.DataBind();
            grid_header.Visible = true;
        }
    }

    private List<string> GetSelectedItemsList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Text);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }

    private List<string> GetSelectedItemsValueList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Value);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }

    private List<string> GetItemsValueList(CheckBoxList cblItems)
    {
        System.Collections.Generic.List<string> lsItems = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblItems.Items.Count; list++)
            {
                lsItems.Add(cblItems.Items[list].Value);
            }
        }
        catch { lsItems.Clear(); }
        return lsItems;
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

    public void BindHeaderGridviewPrev()
    {
        string getfinid = d2.getCurrentFinanceYear(usercode, collegecode);
        if (getfinid.Trim() != "" && getfinid.Trim() != "0")
        {
            //fsHSetting.Visible = true;

            DataTable dt = new DataTable();
            dt.Columns.Add("Dummy1");
            dt.Columns.Add("Dummy2");
            dt.Columns.Add("Dummy3");
            dt.Columns.Add("Dummy4");

            DataRow dr = dt.NewRow(); ;
            dr["Dummy1"] = "Receipt Code";
            dt.Rows.Add(dr);
            DataRow dr1 = dt.NewRow(); ;
            dr1["Dummy1"] = "Challan No";
            dt.Rows.Add(dr1);
            DataRow dr2 = dt.NewRow(); ;
            dr2["Dummy1"] = "Voucher Code";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow(); ;
            dr3["Dummy1"] = "Online Receipt Code";
            dt.Rows.Add(dr3);

            gridHeaderPrev.DataSource = dt;
            gridHeaderPrev.DataBind();
            gridHeaderPrev.Visible = true;
        }
    }

    protected void btnDelHeader_OnCLick(object sender, EventArgs e)
    {
        suredivDelete.Visible = true;
    }

    protected void btn_sureyesDel_Click(object sender, EventArgs e)
    {
        suredivDelete.Visible = false;
        List<string> HeadSetFk = new List<string>();
        if (selectedHeaders(out HeadSetFk))
        {
            try
            {
                foreach (string HeaderSetFk in HeadSetFk)
                {
                    d2.update_method_wo_parameter("delete from fm_headerfincodesettingsdet where headersettingFk = " + HeaderSetFk + "", "Text");
                    d2.update_method_wo_parameter("delete from fm_headerfincodesettings	where headersettingPk = " + HeaderSetFk + "", "Text");
                }
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Deleted Sucessfully";
            }
            catch
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Not Deleted";
            }
        }
        bindheadername();
        bindheadernamePrev();

    }

    protected void btn_surenoDel_Click(object sender, EventArgs e)
    {
        suredivDelete.Visible = false;
    }

    private bool selectedHeaders(out List<string> HeadSetFk)
    {
        bool retVal = false;
        HeadSetFk = new List<string>();
        foreach (GridViewRow gRow in gridHeaderPrev.Rows)
        {
            CheckBox chk = (CheckBox)gRow.FindControl("cb_Select");
            if (chk.Checked)
            {
                Label lblHeadSetFk = (Label)gRow.FindControl("lbl_HeaderSettingPk");
                HeadSetFk.Add(lblHeadSetFk.Text);
                retVal = true;
            }
        }
        return retVal;
    }

    protected void btnUpHeader_OnCLick(object sender, EventArgs e)
    {
        suredivUpdate.Visible = true;
    }

    protected void btn_sureyesUpd_Click(object sender, EventArgs e)
    {
        suredivUpdate.Visible = false;
        try
        {
            foreach (GridViewRow gRow in gridHeaderPrev.Rows)
            {
                CheckBox chk = (CheckBox)gRow.FindControl("cb_Select");
                if (chk.Checked)
                {
                    Label lblHeadSetFk = (Label)gRow.FindControl("lbl_HeaderSettingPk");
                    TextBox txtRcptAcr = (TextBox)gRow.FindControl("txt_acronym0");
                    TextBox txtRcptStNo = (TextBox)gRow.FindControl("txt_startno0");
                    TextBox txtRcptSize = (TextBox)gRow.FindControl("txt_size0");
                    TextBox txtChlnAcr = (TextBox)gRow.FindControl("txt_acronym1");
                    TextBox txtChlnStNo = (TextBox)gRow.FindControl("txt_startno1");
                    TextBox txtChlnSize = (TextBox)gRow.FindControl("txt_size1");
                    TextBox txtVoucAcr = (TextBox)gRow.FindControl("txt_acronym2");
                    TextBox txtVoucStNo = (TextBox)gRow.FindControl("txt_startno2");
                    TextBox txtVoucSize = (TextBox)gRow.FindControl("txt_size2");

                    string Q = " update fm_headerfincodesettings set RcptAcr='" + txtRcptAcr.Text + "', RcptStNo='" + txtRcptStNo.Text + "', RcptSize='" + txtRcptSize.Text + "', VouchAcr='" + txtVoucAcr.Text + "', VOuchStNo='" + txtVoucStNo.Text + "', VouchSize='" + txtVoucSize.Text + "', ChallanAcr='" + txtChlnAcr.Text + "', ChallanStNo='" + txtChlnStNo.Text + "', ChallanSize ='" + txtChlnSize.Text + "', RecpSettingsDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "'	where headersettingPk = " + lblHeadSetFk.Text + "";
                    d2.update_method_wo_parameter(Q, "Text");

                }
            }
            imgdiv2.Visible = true;
            lbl_alerterr.Text = "Updated Successfully";
        }
        catch
        {
            imgdiv2.Visible = true;
            lbl_alerterr.Text = "Not Updated";
        }
        bindheadername();
        bindheadernamePrev();
    }

    protected void btn_surenoUpd_Click(object sender, EventArgs e)
    {
        suredivUpdate.Visible = false;
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
        lbl.Add(lblcol);
        fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    //Last modified -- 04-10-2016
}