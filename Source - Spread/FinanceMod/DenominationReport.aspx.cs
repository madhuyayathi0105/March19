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
using System.Configuration;


public partial class DenominationReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static bool usBasedRights = false;
    static byte roll = 0;
    static int personmode = 0;
    static int chosedmode = 0;
    static string clgcode = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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

            //if (Session["collegecode"] == null)
            //    Response.Redirect("~/Default.aspx");
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindCollege();
                if (cblclg.Items.Count > 0)
                {
                    collegecode = Convert.ToString(getCblSelectedValue(cblclg));
                    clgcode = Convert.ToString(getCblSelectedValue(cblclg));
                }
                bindheader();
                //  loadpaid();
                //  loadfinanceyear();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                getPrintSettings();
                loadsetting();
                loadfinanceUser();
            }
            if (cblclg.Items.Count > 0)
            {
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
                clgcode = Convert.ToString(getCblSelectedValue(cblclg));
            }
        }
        catch
        {
        }
    }
    #region college
    protected void bindCollege()
    {
        cblclg.Items.Clear();
        cbclg.Checked = false;
        txtclg.Text = "--Select--";
        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblclg.DataSource = ds;
            cblclg.DataTextField = "collname";
            cblclg.DataValueField = "college_code";
            cblclg.DataBind();
            if (cblclg.Items.Count > 0)
            {
                for (int row = 0; row < cblclg.Items.Count; row++)
                {
                    cblclg.Items[row].Selected = true;
                }
                cbclg.Checked = true;
                txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
            }
        }
    }
    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        bindheader();
        // loadpaid();
    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        bindheader();
        //loadpaid();
    }
    #endregion
    #region header
    public void bindheader()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            chkl_studhed.Items.Clear();
            txt_studhed.Text = "--Select--";
            chk_studhed.Checked = false;
            // string query = " SELECT distinct HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode in('" + collegecode + "' ) ";
            string query = " SELECT distinct HeaderName FROM FM_HeaderMaster where CollegeCode in('" + collegecode + "' ) ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = ds;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderName";
                chkl_studhed.DataBind();
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = true;
                }
                txt_studhed.Text = lblheader.Text + "(" + chkl_studhed.Items.Count + ")";
                chk_studhed.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void chk_studhed_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
    }
    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
    }
    #endregion

    #region paymentmode
    public void loadpaid()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            chkl_paid.Items.Clear();
            chk_paid.Checked = false;
            txt_paid.Text = "--Select--";
            d2.BindPaymodeToCheckboxList(chkl_paid, usercode, collegecode);
            if (chkl_paid.Items.Count > 0)
            {
                for (int i = 0; i < chkl_paid.Items.Count; i++)
                {
                    chkl_paid.Items[i].Selected = true;
                }
                txt_paid.Text = "Paid(" + chkl_paid.Items.Count + ")";
                chk_paid.Checked = true;
            }
        }
        catch
        {

        }

    }
    public void chk_paid_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }
    public void chkl_paid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }
    #endregion

    #region financial year
    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc";
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
                    string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    chklsfyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
                }

                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = true;
                    fnalyr = Convert.ToString(chklsfyear.Items[i].Text);
                }
                if (chklsfyear.Items.Count == 1)
                {
                    txtfyear.Text = "" + fnalyr + "";
                }
                else
                {
                    txtfyear.Text = "Finance Year(" + (chklsfyear.Items.Count) + ")";
                }
                // txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
                chkfyear.Checked = true;
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
    #endregion
    protected DataSet loadDetails()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            UserbasedRights();
            string hdText = string.Empty;
            string payMode = string.Empty;
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            hdText = Convert.ToString(getCblSelectedText(chkl_studhed));
            string finUser = Convert.ToString(getCblSelectedValue(cbluser));
            // payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string strReg = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";
            string appNo = string.Empty;
            string txtroll = Convert.ToString(txt_roll.Text);
            if (!string.IsNullOrEmpty(txtroll))
                appNo = Convert.ToString(getAppNo(txtroll, collegecode));
            #endregion

            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(hdText))//&& !string.IsNullOrEmpty(payMode)
            {
                #region Query
                string hdFK = getHeaderFK(hdText, collegecode);
                string SelQ = string.Empty;
                SelQ = " select distinct sum(debit) as debit,convert(varchar(10),Transdate,103) as Transdate,Transcode,batch_year,degree_code,f.app_no,r.college_code,r.roll_no,r.reg_no,r.roll_admit,r.stud_name from registration r,ft_findailytransaction f where f.app_no=r.app_no and  r.college_code in('" + collegecode + "') ";
                if (!string.IsNullOrEmpty(appNo))
                    SelQ += " and r.app_no='" + appNo + "'";
                else
                    SelQ += " and f.headerfk in('" + hdFK + "')  and transdate between '" + fromdate + "' and '" + todate + "'    ";//convert(varchar,f.app_no)=d.roll_admit and convert(varchar,r.app_no)=d.roll_admit and and f.TransCode =d.rcpt_no
                SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' " + strReg + " group by Transdate,Transcode,batch_year,degree_code,f.app_no,r.college_code,r.roll_no,r.reg_no,r.roll_admit,r.stud_name order by transcode";//and f.paymode in('" + payMode + "')
                if (returnamt.Checked)
                {
                    SelQ += " select distinct d.roll_admit,rcpt_no,n2000,n1000,n500,n200,n100,n50,n20,n10,n5,n2,n1,c5,c2,c1,Return_amt,user_code from denomination d,ft_findailytransaction f where convert(varchar,f.app_no)=d.roll_admit ";//change by abarna
                    if (!string.IsNullOrEmpty(appNo))
                        SelQ += " and f.app_no='" + appNo + "'";
                    else
                        SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'  ";
                    SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' and f.EntryUserCode =d.user_code and user_code in('" + finUser + "') and  flagsReturnOrCollect='1'";
                }
                else
                {
                    SelQ += " select distinct d.roll_admit,rcpt_no,n2000,n1000,n500,n200,n100,n50,n20,n10,n5,n2,n1,c5,c2,c1,Return_amt,user_code from denomination d,ft_findailytransaction f where convert(varchar,f.app_no)=d.roll_admit ";//change by abarna
                    if (!string.IsNullOrEmpty(appNo))
                        SelQ += " and f.app_no='" + appNo + "'";
                    else
                        SelQ += " and transdate between '" + fromdate + "' and '" + todate + "'  ";
                    SelQ += " and isnull(iscanceled,'0')='0' and transcode<>'' and f.EntryUserCode =d.user_code and user_code in('" + finUser + "') and   flagsReturnOrCollect<>'1'";
                }
                // SelQ = " select headerName,sum(isnull(debit,'0')) as debit,sum(isnull(credit,'0')) as credit,paymode,h.collegecode from ft_findailytransaction f,fm_headermaster h,collinfo cl where h.headerpk=f.headerfk and h.collegecode=cl.college_code and h.collegecode in('" + collegecode + "') and h.headername in('" + hdText + "') and f.paymode in('" + payMode + "') and transdate between '" + fromdate + "' and '" + todate + "' group by headername,paymode,h.collegecode";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(SelQ, "Text");
                #endregion
            }
        }
        catch { }
        return dsload;
    }

    protected string getAppNo(string rollno, string selclgcode)
    {
        string appno = string.Empty;
        try
        {
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
            {
                appno = d2.GetFunction(" select App_No from Registration where Roll_No='" + rollno + "' and college_code in('" + selclgcode + "')");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
            {
                appno = d2.GetFunction(" select App_No from Registration where reg_no='" + rollno + "' and college_code in('" + selclgcode + "')");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
            {
                appno = d2.GetFunction(" select App_No from Registration where Roll_admit='" + rollno + "' and college_code in('" + selclgcode + "')");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
            {
                appno = d2.GetFunction(" select app_no from applyn where app_formno='" + rollno + "' and college_code in('" + selclgcode + "')");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 4)
            {
                appno = d2.GetFunction(" select app_no from registration where roll_no='" + txt_roll.Text.Split('-')[1] + "' and college_code in('" + selclgcode + "')");
            }
        }
        catch { }
        return appno;
    }
    protected void loadSpreadDetails(DataSet ds)
    {
        try
        {
            #region design
            RollAndRegSettings();
            Hashtable htdegName = getDeptName();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 11;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            bool boolSno = false;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].Width = 40;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].Width = 70;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].Width = 70;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[3].Width = 70;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[4].Width = 150;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Department";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[5].Width = 40;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Date";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[6].Width = 40;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Receipt No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[7].Width = 40;


            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Text = "User Name";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Right;
            spreadDet.Sheets[0].Columns[8].Width = 40;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Paid Amount";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Right;
            spreadDet.Sheets[0].Columns[9].Width = 40;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Return Amount";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
            spreadDet.Sheets[0].Columns[10].Width = 40;




            Hashtable htcol = new Hashtable();
            Dictionary<string, string> dtcol = getColumn();
            foreach (KeyValuePair<string, string> dtrow in dtcol)
            {
                spreadDet.Sheets[0].ColumnCount++;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dtrow.Key.Trim());
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                htcol.Add(Convert.ToString(dtrow.Key), spreadDet.Sheets[0].ColumnCount - 1);
            }
            spreadColumnVisible();
            #endregion

            #region value
            int rowCnt = 0;
            double returnAmt = 0;
            string Username = string.Empty;
            Hashtable grandtotal = new Hashtable();

            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                bool boolrow = true;
                string degcode = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                string deptName = Convert.ToString(htdegName[degcode.Trim()]);
                string rollno = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                string regno = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                string admisno = Convert.ToString(ds.Tables[0].Rows[row]["Roll_admit"]);
                string appNo = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                string studName = Convert.ToString(ds.Tables[0].Rows[row]["stud_name"]);
                string rcptNo = Convert.ToString(ds.Tables[0].Rows[row]["Transcode"]);
                string trnsDt = Convert.ToString(ds.Tables[0].Rows[row]["transdate"]);
                double Amt = 0;
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["debit"]), out Amt);

                bool boolCheck = false;
                foreach (KeyValuePair<string, string> dtrow in dtcol)
                {

                    string col = Convert.ToString(dtrow.Key);
                    string colVal = Convert.ToString(dtrow.Value);
                    ds.Tables[1].DefaultView.RowFilter = "roll_admit='" + appNo + "' and rcpt_no='" + rcptNo + "'";
                    DataTable dtstud = ds.Tables[1].DefaultView.ToTable();
                    if (dtstud.Rows.Count > 0)
                    {

                        double.TryParse(Convert.ToString(dtstud.Rows[0]["Return_amt"]), out returnAmt);
                        string usercode = Convert.ToString(dtstud.Rows[0]["user_code"]);
                        Username = d2.GetFunction("select user_id from usermaster where user_code='" + usercode + "'");//User Name Getting
                        if (boolrow)
                            spreadDet.Sheets[0].RowCount++;
                        boolrow = false;
                        double count = 0;
                        int curColCnt = 0;
                        int.TryParse(Convert.ToString(htcol[col.Trim()]), out curColCnt);
                        double.TryParse(Convert.ToString(dtstud.Rows[0][colVal]), out count);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(count);
                        if (!grandtotal.ContainsKey(curColCnt))
                            grandtotal.Add(curColCnt, Convert.ToString(count));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                            amount += count;
                            grandtotal.Remove(curColCnt);
                            grandtotal.Add(curColCnt, Convert.ToString(amount));
                        }
                        boolCheck = true;
                    }
                }
                if (boolCheck)
                {
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++rowCnt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = rollno;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = regno;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = admisno;

                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].CellType = txtroll;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtroll;

                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = studName;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = deptName;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = trnsDt;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = rcptNo;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(Amt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(returnAmt);//Bind Return Amount Added by abarna
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(Username);//bind User Name Added by abarna
                    if (!grandtotal.ContainsKey(9))
                        grandtotal.Add(9, Convert.ToString(Amt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(grandtotal[9]), out amount);
                        amount += Amt;
                        grandtotal.Remove(9);
                        grandtotal.Add(9, Convert.ToString(amount));
                    }
                    if (!grandtotal.ContainsKey(10))
                        grandtotal.Add(10, Convert.ToString(returnAmt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(grandtotal[10]), out amount);
                        amount += returnAmt;
                        grandtotal.Remove(10);
                        grandtotal.Add(10, Convert.ToString(amount));
                    }
                }
            }
            spreadDet.Sheets[0].Rows.Count++;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 3);
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
            double grandvalues = 0;
            for (int j = 9; j < spreadDet.Sheets[0].ColumnCount; j++)
            {
                double.TryParse(Convert.ToString(grandtotal[j]), out grandvalues);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            // lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            txt_roll.Text = string.Empty;
            //spreadDet.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

            //spreadDet.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //spreadDet.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //spreadDet.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //spreadDet.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //spreadDet.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);

            //spreadDet.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //spreadDet.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //spreadDet.Sheets[0].SetColumnMerge(9, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //spreadDet.Sheets[0].SetColumnMerge(10, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //  payModeLabels(htpayMode);
            //  spreadDet.Height = height;
            spreadDet.SaveChanges();
            #endregion
        }
        catch { }
    }

    protected Dictionary<string, string> getColumn()
    {
        Dictionary<string, string> dtcol = new Dictionary<string, string>();
        try
        {
            dtcol.Add("2000(N)", "n2000");
            dtcol.Add("1000(N)", "n1000");
            dtcol.Add("500(N)", "n500");
            dtcol.Add("200(N)", "n200");//added by abarna
            dtcol.Add("100(N)", "n100");
            dtcol.Add("50(N)", "n50");
            dtcol.Add("20(N)", "n20");
            dtcol.Add("10(N)", "n10");
            dtcol.Add("5(N)", "n5");
            dtcol.Add("2(N)", "n2");
            dtcol.Add("1(N)", "n1");
            dtcol.Add("5(C)", "c5");
            dtcol.Add("2(C)", "c2");
            dtcol.Add("1(C)", "c1");
        }
        catch { }
        return dtcol;
    }


    protected void btngo_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = loadDetails();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
        {
            loadSpreadDetails(ds);
        }
        else
        {
            //lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            spreadDet.Visible = false;
            print.Visible = false;
            divlabl.Visible = false;
            txt_roll.Text = string.Empty;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            //lbl_alert.Text = "No Record Found";
            //imgdiv2.Visible = true;
        }
    }

    protected void UserbasedRights()
    {
        string userrht = d2.GetFunction("select value from Master_Settings where settings='Finance Include User Based Report Settings'  and usercode='" + usercode + "'");
        if (userrht == "1")
            usBasedRights = true;
        else
            usBasedRights = false;

    }
    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
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
            // lblvalidation1.Text = "";
            string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));

            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            degreedetails = "Denomination Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "DenominationReport.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    protected string getclgAcr(string collegecode)
    {
        string strAcr = string.Empty;
        try
        {
            StringBuilder clgAcr = new StringBuilder();
            string selQ = " select collname,college_code,acr from collinfo where college_code in('" + collegecode + "')";
            DataSet dsclg = d2.select_method_wo_parameter(selQ, "Text");
            if (dsclg.Tables.Count > 0 && dsclg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsclg.Tables[0].Rows.Count; row++)
                {
                    clgAcr.Append(Convert.ToString(dsclg.Tables[0].Rows[row]["acr"]) + ",");
                }
                if (clgAcr.Length > 0)
                    clgAcr.Remove(clgAcr.Length - 1, 1);
                strAcr = Convert.ToString(clgAcr);
            }
        }
        catch { strAcr = string.Empty; }
        return strAcr;
    }
    #endregion

    #region print settings
    protected void getPrintSettings()
    {
        try
        {
            //barath 15.03.17
            #region Excel print settings
            string usertype = "";
            if (usercode.Trim() != "")
                usertype = " and usercode='" + usercode + "'";
            else if (group_user.Trim() != "")
                usertype = " and group_code='" + group_user + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                }
            }
            #endregion
        }
        catch { }
    }
    #endregion

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

    protected string getHeaderFK(string hdName, string collegecode)
    {
        string hdFK = string.Empty;
        try
        {
            string[] headerFK = new string[0];
            string selQFK = "  select distinct headerpk from fm_headermaster where collegecode in('" + collegecode + "') and headername in('" + hdName + "')";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    Array.Resize(ref headerFK, headerFK.Length + 1);
                    headerFK[headerFK.Length - 1] = Convert.ToString(dsval.Tables[0].Rows[row]["headerpk"]);
                }
                hdFK = string.Join("','", headerFK);
            }
        }
        catch { hdFK = string.Empty; }
        return hdFK;
    }
    protected Hashtable getDeptName()
    {
        Hashtable htdtName = new Hashtable();
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string SelQ = " select distinct d.degree_code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from degree d,department dt,course c where c.course_id=d.course_id and d.dept_code=dt.dept_code and d.college_code in('" + collegecode + "')";
            DataSet dsdeg = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsdeg.Tables.Count > 0 && dsdeg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsdeg.Tables[0].Rows.Count; row++)
                {
                    if (!htdtName.ContainsKey(Convert.ToString(dsdeg.Tables[0].Rows[row]["degree_code"])))
                        htdtName.Add(Convert.ToString(dsdeg.Tables[0].Rows[row]["degree_code"]), Convert.ToString(dsdeg.Tables[0].Rows[row]["degreename"]));
                }
            }
        }
        catch { }
        return htdtName;
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
            #region
            if (roll == 0)
            {
                spreadDet.Columns[1].Visible = true;
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
            }
            else if (roll == 1)
            {
                spreadDet.Columns[1].Visible = true;
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
            }
            else if (roll == 2)
            {
                spreadDet.Columns[1].Visible = true;
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = false;

            }
            else if (roll == 3)
            {
                spreadDet.Columns[1].Visible = false;
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = false;
            }
            else if (roll == 4)
            {
                spreadDet.Columns[1].Visible = false;
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = true;
            }
            else if (roll == 5)
            {
                spreadDet.Columns[1].Visible = true;
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = false;
            }
            else if (roll == 6)
            {
                spreadDet.Columns[1].Visible = false;
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
            }
            else if (roll == 7)
            {
                spreadDet.Columns[1].Visible = true;
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = true;
            }
            #endregion
        }
        catch { }
    }

    #endregion

    //added by sudhagar for student auto search
    public void loadsetting()
    {
        try
        {
            ListItem list1 = new ListItem("Roll No", "0");
            ListItem list2 = new ListItem("Reg No", "1");
            ListItem list3 = new ListItem("Admission No", "2");
            ListItem list4 = new ListItem("App No", "3");
            ListItem list5 = new ListItem("Name", "4");
            string collegecode1 = string.Empty;
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list4);
            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(list1);
            }
            rbl_rollno.Items.Add(list5);
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    chosedmode = 3;
                    break;
                case 4:
                    txt_roll.Attributes.Add("placeholder", "");
                    chosedmode = 4;
                    break;
            }



        }
        catch { }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_roll.Text = "";
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("Placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("Placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("Placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("Placeholder", "App No");
                    chosedmode = 3;
                    break;
                case 4:
                    txt_roll.Attributes.Add("Placeholder", "");
                    chosedmode = 4;
                    break;
            }
        }
        catch { }
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
            if (personmode == 0)
            {
                //student query
                if (chosedmode == 0)
                {
                    query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code in('" + clgcode + "') order by Roll_No asc";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code in('" + clgcode + "') order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code in('" + clgcode + "') order by Roll_admit asc";
                }
                else if (chosedmode == 4)
                {
                    query = "  select  top 100 Stud_Name+'-'+Roll_No+'-'+(select c.Course_Name+'-'+dept_name from Department dt,Degree d,course c where c.Course_Id=d.Course_Id and dt.Dept_Code =d.Dept_Code and d.Degree_Code=r.degree_code) as Roll_admit from Registration r where Stud_Name like '" + prefixText + "%' and college_code in('" + clgcode + "') order by Roll_admit asc";
                }
                else
                {
                    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code in('" + clgcode + "') order by app_formno asc";
                }
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    //added by abarna 25.01.2019
    #region finuser year

    public void loadfinanceUser()
    {
        try
        {
            collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string finUser = "   select user_id,user_code from usermaster where fin_user='1' ";//and college_code in('" + collegecode + "')
            string getfin = d2.GetFunction("select fin_user from usermaster where fin_user='1' and user_code='" + usercode.Trim() + "' ");//and college_code in('" + collegecode + "')
            cbuser.Checked = false;
            cbluser.Items.Clear();
            txtuser.Text = "--Select--";
            ds = d2.select_method_wo_parameter(finUser, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbluser.DataSource = ds;
                cbluser.DataTextField = "user_id";
                cbluser.DataValueField = "user_code";
                cbluser.DataBind();
                int cnt = 0;
                string getSngName = string.Empty;
                if (getfin.Trim() == "1")
                {
                    for (int i = 0; i < cbluser.Items.Count; i++)
                    {
                        //cbluser.Items[i].Selected = true;
                        if (cbluser.Items[i].Value.Trim() == usercode.Trim())
                        {
                            cbluser.Items[i].Selected = true;
                            getSngName = cbluser.Items[i].Text;
                            cnt++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cbluser.Items.Count; i++)
                    {
                        cbluser.Items[i].Selected = true;
                        cnt++;
                    }
                }
                if (cbluser.Items.Count == cnt)
                {
                    txtuser.Text = lbluser.Text + "(" + cbluser.Items.Count + ")";
                    cbuser.Checked = true;
                }
                else
                {
                    if (cnt == 1)
                        txtuser.Text = getSngName;
                    else
                        txtuser.Text = lbluser.Text + "(" + cnt + ")";
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void cbuser_changed(object sender, EventArgs e)
    {
        CallCheckboxChange(cbuser, cbluser, txtuser, lbluser.Text, "--Select--");

    }

    protected void cbluser_selected(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbuser, cbluser, txtuser, lbluser.Text, "--Select--");
    }

    #endregion
}