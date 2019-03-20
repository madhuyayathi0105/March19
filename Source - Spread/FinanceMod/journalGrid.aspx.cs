using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Text;
using InsproDataAccess;
using System.Configuration;

public partial class journalGrid : System.Web.UI.Page
{
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    Hashtable ht = new Hashtable();
    int count = 0;
    bool spreadstud_click = false;
    bool spreadstud2_click = false;
    bool spreadstud3_click = false;
    static byte roll = 0;
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static Hashtable ledgertotal = new Hashtable();
    static Hashtable headertotal = new Hashtable();
    static Hashtable Grandtotal = new Hashtable();
    static Hashtable hsgetpay = new Hashtable();
    static ArrayList columnind = new ArrayList();
    ReuasableMethods reuse = new ReuasableMethods();
    InsproDirectAccess dirAccess = new InsproDirectAccess();
    string columnname = "";
    static int personmode = 0;
    static int chosedmode = 0;
    static int applied = 0;
    static int distcon = 0;
    static int compl = 0;
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["dtGrid"] != null)
            {
                Session.Remove("dtGrid");
            }
            if (Session["arrColHdrNames2"] != null)
            {
                Session.Remove("arrColHdrNames2");
            }
        }
        callGridBind();
    }
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

            sessstream = Convert.ToString(Session["streamcode"]);
            lbl_stream.Text = sessstream;

            plusdiv.Visible = false;
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
            //this.Form.DefaultButton = "btn_go";

            if (!IsPostBack)
            {
                setLabelText();
                bindcollege();
                if (ddl_college.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
                    collegecode = Convert.ToString(ddl_college.SelectedItem.Value);
                }
                loadstream();
                loadedulevel();
                BindBatch();
                Bindcourse();
                binddept();
                loadseat();
                loadType();

                loadsem();
                headerbind();
                loadledger();
                bindaddreason();
                loadreligion();
                loadcommunity();
                loaddesc1();

                ledgertotal.Clear();
                headertotal.Clear();
                Grandtotal.Clear();
                hsgetpay.Clear();
                lnkview.Visible = false;
                txt_roll.Visible = false;
                rbl_rollno.Visible = false;
                lblNameSrc.Visible = false;
                txt_name.Visible = false;
                tborder.Visible = false;
                txt_due.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_due.Attributes.Add("readonly", "readonly");
                txtreeadddt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtreeadddt.Attributes.Add("readonly", "readonly");
                getDiscontinue();
                loadDisable();
            }

            if (ddl_college.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
                collegecode = Convert.ToString(ddl_college.SelectedItem.Value);
            }
            if (isStreamEnabled())
            {
                txt_stream.Enabled = true;
            }
            else
            {
                txt_stream.Enabled = false;
            }
            //string uid = this.Page.Request.Params.Get("__EVENTTARGET");
            //if (uid != null && !uid.Contains("gridLedgeDetails"))
            //{
            //callGridBind();
            //}
        }
        catch (Exception ex) { }
    }
    protected void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddl_college.SelectedIndex = ddl_college.Items.IndexOf(ddl_college.Items.FindByValue(ddl_college.SelectedItem.Value));
            if (ddl_type.SelectedItem.Text == "General")
            {
                lnkview.Visible = false;
            }
            if (ddl_type.SelectedItem.Text == "Individual(Admitted)")
            {
                lnkview.Visible = true;
            }
            loadstream();
            loadedulevel();
            BindBatch();
            Bindcourse();
            binddept();
            loadseat();
            //loadtype();
            loadstutype();
            loadsem();
            headerbind();
            loadledger();
            bindaddreason();
            loaddesc1();
            loadcommunity();
            loadreligion();
            ledgertotal.Clear();
            headertotal.Clear();
            Grandtotal.Clear();
            loadDisable();
        }
        catch { }
    }
    protected void lb2_Click(object sender, EventArgs e)
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
    protected void bindcollege()
    {
        ddl_college.Items.Clear();
        // reuse.bindCollegeToDropDown(usercode, ddl_college);
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
    }
    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static List<string> Getroll(string prefixText)
    //{
    //    WebService ws = new WebService();
    //    List<string> roll = new List<string>();
    //    // if (Hostelcode.Trim() != "")
    //    //{
    //    //  query = "select R.Roll_No from Registration r,Hostel_StudentDetails h where r.Roll_Admit =h.Roll_Admit and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Hostel_Code in ('" + Hostelcode + "') and R.roll_no like '" + prefixText + "%' ";
    //    string query = "select distinct top (10) Reg_No from Registration where college_code ='" + collegecode1 + "' and Reg_No like '" + prefixText + "%'";
    //    //}

    //    // string query = "select distinct Store_Name from StoreMaster WHERE Store_Name like '" + prefixText + "%'";
    //    roll = ws.Getname(query);
    //    return roll;
    //}
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getroll(string prefixText)
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
                    query = "select top 100 Roll_No from Registration where college_code ='" + collegecode1 + "' and Roll_No like '" + prefixText + "%' order by Roll_No asc";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration where college_code ='" + collegecode1 + "' and Reg_No like '" + prefixText + "%' order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where college_code ='" + collegecode1 + "' and Roll_admit like '" + prefixText + "%' order by Roll_admit asc";
                }
                else
                {
                    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' order by app_formno asc";
                }
            }


            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {

        }
    }
    protected void btnexit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default_login.aspx");
    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        //  popper1.Visible = false;
    }

    protected void Fpspreadfine_Command(object sender, EventArgs e)
    {

    }
    protected void Fpspreadfine_render(object sender, EventArgs e)
    {
        try
        {
            string actrow = Convert.ToString(Fpspreadfine.ActiveSheetView.ActiveRow);
            string actcol = Convert.ToString(Fpspreadfine.ActiveSheetView.ActiveColumn);
        }
        catch
        {

        }
    }

    //fine settings
    protected void lnkfine_click(object sender, EventArgs e)
    {
        try
        {
            popfine.Visible = true;
            rb_common.Checked = true;
            rb_common_OnCheckedChanged(sender, e);
            bindheader();
            bindledger();
            rbfine.Checked = true;
            rbreadd.Checked = false;
            rbfine_Changed(sender, e);
        }
        catch { }
    }
    protected void rb_common_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            btnWeekFindDel.Visible = false;
            txt_fine.Visible = true;
            int headcount = 0;
            int ledgercount = 0;

            string selhead = "select distinct HeaderFK,LedgerFK from FM_FineMaster where FineType='1' and CollegeCode='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selhead, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbheadfine.Checked = false;
                    cblheadfine.ClearSelection();
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        for (int i = 0; i < cblheadfine.Items.Count; i++)
                        {
                            if (Convert.ToString(cblheadfine.Items[i].Value) == Convert.ToString(ds.Tables[0].Rows[j]["HeaderFK"]))
                            {
                                cblheadfine.Items[i].Selected = true;
                                headcount = headcount + 1;
                            }
                        }
                    }
                    txtheadfine.Text = "Header(" + headcount + ")";

                    cbledgefine.Checked = false;
                    cblledgefine.ClearSelection();
                    for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                    {
                        for (int k = 0; k < cblledgefine.Items.Count; k++)
                        {
                            if (Convert.ToString(cblledgefine.Items[k].Value) == Convert.ToString(ds.Tables[0].Rows[l]["LedgerFK"]))
                            {
                                cblledgefine.Items[k].Selected = true;
                                ledgercount = ledgercount + 1;
                            }
                        }
                    }
                    txtledgerfine.Text = "Ledger(" + ledgercount + ")";
                }
            }

            string selcom = "select distinct FineAmount,convert(varchar(10),DueDate,103) as DueDate,HeaderFK,LedgerFK from FM_FineMaster where FineType='1' and CollegeCode='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selcom, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txt_fine.Text = Convert.ToString(ds.Tables[0].Rows[0]["FineAmount"]);
                    txt_due.Text = Convert.ToString(ds.Tables[0].Rows[0]["DueDate"]);
                }
                else
                {
                    txt_fine.Text = "";
                    txt_due.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    bindheader();
                    bindledger();
                }
            }
            else
            {
                txt_fine.Text = "";
                txt_due.Text = DateTime.Now.ToString("dd/MM/yyyy");
                bindheader();
                bindledger();
            }
            Fpspreadfine.Visible = false;
            field.Visible = false;
        }
        catch
        {

        }
    }
    protected void rb_perday_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            btnWeekFindDel.Visible = false;
            txt_fine.Visible = true;
            int headcount = 0;
            int ledgercount = 0;

            string selhead = "select distinct HeaderFK,LedgerFK from FM_FineMaster where FineType='2' and CollegeCode='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selhead, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbheadfine.Checked = false;
                    cblheadfine.ClearSelection();
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        for (int i = 0; i < cblheadfine.Items.Count; i++)
                        {
                            if (Convert.ToString(cblheadfine.Items[i].Value) == Convert.ToString(ds.Tables[0].Rows[j]["HeaderFK"]))
                            {
                                cblheadfine.Items[i].Selected = true;
                                headcount = headcount + 1;
                            }
                        }
                    }
                    txtheadfine.Text = "Header(" + headcount + ")";

                    cbledgefine.Checked = false;
                    cblledgefine.ClearSelection();
                    for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                    {
                        for (int k = 0; k < cblledgefine.Items.Count; k++)
                        {
                            if (Convert.ToString(cblledgefine.Items[k].Value) == Convert.ToString(ds.Tables[0].Rows[l]["LedgerFK"]))
                            {
                                cblledgefine.Items[k].Selected = true;
                                ledgercount = ledgercount + 1;
                            }
                        }
                    }
                    txtledgerfine.Text = "Ledger(" + ledgercount + ")";
                }
            }

            string selperday = "select distinct FineAmount,convert(varchar(10),DueDate,103) as DueDate,HeaderFK,LedgerFK from FM_FineMaster where FineType='2' and CollegeCode='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selperday, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txt_fine.Text = Convert.ToString(ds.Tables[0].Rows[0]["FineAmount"]);
                    txt_due.Text = Convert.ToString(ds.Tables[0].Rows[0]["DueDate"]);
                }
                else
                {
                    txt_fine.Text = "";
                    txt_due.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    bindheader();
                    bindledger();
                }
            }
            else
            {
                txt_fine.Text = "";
                txt_due.Text = DateTime.Now.ToString("dd/MM/yyyy");
                bindheader();
                bindledger();
            }
            Fpspreadfine.Visible = false;
            field.Visible = false;
        }
        catch
        {

        }
    }
    protected void rb_perweek_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            txt_fine.Visible = false;
            field.Visible = true;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.Font.Size = 13;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Bold = true;
            Fpspreadfine.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Fpspreadfine.Sheets[0].RowHeader.Visible = false;
            Fpspreadfine.CommandBar.Visible = false;
            Fpspreadfine.Sheets[0].AutoPostBack = false;

            Fpspreadfine.Sheets[0].ColumnCount = 4;
            FarPoint.Web.Spread.DoubleCellType doubl = new FarPoint.Web.Spread.DoubleCellType();
            doubl.ErrorMessage = "Allow only Numerics";

            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadfine.Columns[0].Width = 50;
            //Fpspreadfine.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;

            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Days From";
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadfine.Columns[1].Width = 125;

            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Days To";
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadfine.Columns[2].Width = 125;

            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Fine Amount(Rs.)";
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspreadfine.Columns[3].Width = 200;

            Fpspreadfine.Sheets[0].RemoveRows(0, Fpspreadfine.Sheets[0].RowCount);
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            string feecat = Convert.ToString(getCblSelectedValue(cbl_sem));
            string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            string hdFK = Convert.ToString(getCblSelectedValue(cblheadfine));
            string ldFK = Convert.ToString(getCblSelectedValue(cblledgefine));
            int headcount = 0;
            int ledgercount = 0;
            string selhead = "select distinct HeaderFK,LedgerFK from FM_FineMaster where FineType='3' and CollegeCode='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selhead, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbheadfine.Checked = false;
                    cblheadfine.ClearSelection();
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        for (int i = 0; i < cblheadfine.Items.Count; i++)
                        {
                            if (Convert.ToString(cblheadfine.Items[i].Value) == Convert.ToString(ds.Tables[0].Rows[j]["HeaderFK"]))
                            {
                                cblheadfine.Items[i].Selected = true;
                                headcount = headcount + 1;
                            }
                        }
                    }
                    txtheadfine.Text = "Header(" + headcount + ")";

                    cbledgefine.Checked = false;
                    cblledgefine.ClearSelection();
                    for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                    {
                        for (int k = 0; k < cblledgefine.Items.Count; k++)
                        {
                            if (Convert.ToString(cblledgefine.Items[k].Value) == Convert.ToString(ds.Tables[0].Rows[l]["LedgerFK"]))
                            {
                                cblledgefine.Items[k].Selected = true;
                                ledgercount = ledgercount + 1;
                            }
                        }
                    }
                    txtledgerfine.Text = "Ledger(" + ledgercount + ")";
                }
            }
            string selq = "select distinct FromDay,ToDay,FineAmount,DueDate from FM_FineMaster where FineType='3' and CollegeCode='" + collegecode1 + "' and headerfk in('" + hdFK + "') and ledgerfk in('" + ldFK + "') and feecatgory in('" + feecat + "') and degreecode in('" + degree + "') and batchyear in('" + batch + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string date = Convert.ToString(ds.Tables[0].Rows[0]["DueDate"]);
                    string[] spldt = date.Split('/');
                    DateTime dtad = Convert.ToDateTime(spldt[0] + "/" + spldt[1] + "/" + spldt[2]);
                    txt_due.Text = dtad.ToString("dd/MM/yyyy");

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspreadfine.Sheets[0].RowCount++;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 1].CellType = doubl;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["FromDay"]);
                        //Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["FineMasterPK"]);
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 2].CellType = doubl;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["ToDay"]);
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 3].CellType = doubl;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["FineAmount"]);
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    }
                    Fpspreadfine.Visible = true;
                    Fpspreadfine.Sheets[0].PageSize = Fpspreadfine.Sheets[0].RowCount;
                    Fpspreadfine.Width = 500;
                    Fpspreadfine.Height = 300;
                    ds.Clear();
                    tblperweek.Visible = true;
                    btnWeekFindDel.Visible = true;
                }
                else
                {
                    Fpspreadfine.Sheets[0].RowCount = 1;
                    Fpspreadfine.Sheets[0].Cells[0, 0].Text = "1";
                    txt_due.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspreadfine.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspreadfine.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspreadfine.Sheets[0].PageSize = Fpspreadfine.Sheets[0].RowCount;
                    Fpspreadfine.Visible = true;
                    Fpspreadfine.Width = 500;
                    Fpspreadfine.Height = 300;
                    tblperweek.Visible = true;
                    btnWeekFindDel.Visible = false;
                }
            }
        }
        catch
        {

        }
    }
    protected void btnaddrow_click(object sender, EventArgs e)
    {
        try
        {
            int no = Fpspreadfine.Sheets[0].RowCount;
            Fpspreadfine.Sheets[0].RowCount++;
            Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(no + 1);
            Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            Fpspreadfine.Sheets[0].Cells[Fpspreadfine.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

            Fpspreadfine.Sheets[0].PageSize = Fpspreadfine.Sheets[0].RowCount;
            Fpspreadfine.Visible = true;
            Fpspreadfine.Width = 500;
            Fpspreadfine.Height = 300;
        }
        catch
        {

        }
    }

    protected void btnsavefine_click(object sender, EventArgs e)
    {
        try
        {
            string finetype = "";
            string frmday = "";
            string today = "";
            string fneweek = "";

            if (rbfine.Checked == true)
            {
                if (rb_common.Checked == true)
                    finetype = "1";

                if (rb_perday.Checked == true)
                    finetype = "2";

                if (rb_perweek.Checked == true)
                    finetype = "3";
            }
            else
                finetype = "1";


            string fineamnt = Convert.ToString(txt_fine.Text);
            string duedate = Convert.ToString(txt_due.Text);
            string[] split = new string[2];
            split = duedate.Split('/');
            DateTime dut = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

            //readmission
            string refineamt = Convert.ToString(txtreeaddAmt.Text);
            string reduedate = Convert.ToString(txtreeadddt.Text);
            string[] splits = new string[2];
            splits = reduedate.Split('/');
            DateTime dutreadd = Convert.ToDateTime(splits[1] + "/" + splits[0] + "/" + splits[2]);

            Fpspreadfine.SaveChanges();
            string actrow = Convert.ToString(Fpspreadfine.ActiveSheetView.ActiveRow);
            string actcol = Convert.ToString(Fpspreadfine.ActiveSheetView.ActiveColumn);

            if (actrow.Trim() != "" && actrow.Trim() != "-1")
            {
                frmday = Convert.ToString(Fpspreadfine.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Text);
                today = Convert.ToString(Fpspreadfine.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
                fneweek = Convert.ToString(Fpspreadfine.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Text);
            }

            string insquery = "";
            int inscount = 0;
            bool boolbtn = false;
            btnWeekFindDel.Visible = false;
            for (int j = 0; j < cblledgefine.Items.Count; j++)
            {
                if (cblledgefine.Items[j].Selected == true)
                {
                    for (int bt = 0; bt < cbl_batch.Items.Count; bt++)
                    {
                        if (cbl_batch.Items[bt].Selected)
                        {
                            for (int k = 0; k < cbl_dept.Items.Count; k++)
                            {
                                if (cbl_dept.Items[k].Selected == true)
                                {
                                    for (int l = 0; l < cbl_sem.Items.Count; l++)
                                    {
                                        if (cbl_sem.Items[l].Selected == true)
                                        {
                                            string getheaderid = d2.GetFunction("select headerfk from fm_ledgermaster where ledgerpk='" + Convert.ToString(cblledgefine.Items[j].Value) + "'");
                                            if (rbfine.Checked == true)
                                            {
                                                //normal fine settings
                                                #region
                                                if (rb_common.Checked == true)
                                                {

                                                    if (fineamnt != "")
                                                    {
                                                        insquery = "if exists (select * from FM_FineMaster where BatchYear='" + cbl_batch.Items[bt].Value + "' and FeeCatgory ='" + cbl_sem.Items[l].Value + "' and DegreeCode='" + cbl_dept.Items[k].Value + "' and CollegeCode='" + collegecode1 + "' and HeaderFK='" + getheaderid + "' and LedgerFK='" + cblledgefine.Items[j].Value + "' and FineType='" + finetype + "' and FineSettingType='0') update FM_FineMaster set FineType='" + finetype + "',FineAmount='" + fineamnt + "',DueDate='" + dut.ToString("MM/dd/yyyy") + "' where BatchYear='" + cbl_batch.Items[bt].Value + "' and FeeCatgory ='" + cbl_sem.Items[l].Value + "' and DegreeCode='" + cbl_dept.Items[k].Value + "' and CollegeCode='" + collegecode1 + "' and HeaderFK='" + getheaderid + "' and LedgerFK='" + cblledgefine.Items[j].Value + "' and FineType='" + finetype + "' and FineSettingType='0'  else Insert into FM_FineMaster (FineType,FineAmount,DueDate,HeaderFK,LedgerFK,FeeCatgory,DegreeCode,CollegeCode,FineSettingType,BatchYear) values ('" + finetype + "','" + fineamnt + "','" + dut.ToString("MM/dd/yyyy") + "','" + getheaderid + "','" + cblledgefine.Items[j].Value + "','" + cbl_sem.Items[l].Value + "','" + cbl_dept.Items[k].Value + "','" + collegecode1 + "','0','" + cbl_batch.Items[bt].Value + "')";
                                                        inscount = d2.update_method_wo_parameter(insquery, "Text");
                                                    }
                                                }
                                                if (rb_perday.Checked == true)
                                                {
                                                    if (fineamnt != "")
                                                    {
                                                        insquery = "if exists (select * from FM_FineMaster where BatchYear='" + cbl_batch.Items[bt].Value + "' and FeeCatgory ='" + cbl_sem.Items[l].Value + "' and DegreeCode='" + cbl_dept.Items[k].Value + "' and CollegeCode='" + collegecode1 + "' and HeaderFK='" + getheaderid + "' and LedgerFK='" + cblledgefine.Items[j].Value + "' and FineType='" + finetype + "' and FineSettingType='0' ) update FM_FineMaster set FineType='" + finetype + "',FineAmount='" + fineamnt + "',DueDate='" + dut.ToString("MM/dd/yyyy") + "' where BatchYear='" + cbl_batch.Items[bt].Value + "' and FeeCatgory ='" + cbl_sem.Items[l].Value + "' and DegreeCode='" + cbl_dept.Items[k].Value + "' and CollegeCode='" + collegecode1 + "' and HeaderFK='" + getheaderid + "' and LedgerFK='" + cblledgefine.Items[j].Value + "' and FineType='" + finetype + "' and FineSettingType='0'  else Insert into FM_FineMaster (FineType,FineAmount,DueDate,HeaderFK,LedgerFK,FeeCatgory,DegreeCode,CollegeCode,FineSettingType,BatchYear) values ('" + finetype + "','" + fineamnt + "','" + dut.ToString("MM/dd/yyyy") + "','" + getheaderid + "','" + cblledgefine.Items[j].Value + "','" + cbl_sem.Items[l].Value + "','" + cbl_dept.Items[k].Value + "','" + collegecode1 + "','0','" + cbl_batch.Items[bt].Value + "')";
                                                        inscount = d2.update_method_wo_parameter(insquery, "Text");
                                                    }
                                                }
                                                if (rb_perweek.Checked == true)
                                                {
                                                    for (int fp = 0; fp < Fpspreadfine.Sheets[0].Rows.Count; fp++)
                                                    {
                                                        frmday = Convert.ToString(Fpspreadfine.Sheets[0].Cells[fp, 1].Text);
                                                        today = Convert.ToString(Fpspreadfine.Sheets[0].Cells[fp, 2].Text);
                                                        fneweek = Convert.ToString(Fpspreadfine.Sheets[0].Cells[fp, 3].Text);

                                                        if (!string.IsNullOrEmpty(frmday) && !string.IsNullOrEmpty(today) && !string.IsNullOrEmpty(fneweek))
                                                        {
                                                            insquery = "if exists (select * from FM_FineMaster where BatchYear='" + cbl_batch.Items[bt].Value + "' and FeeCatgory='" + cbl_sem.Items[l].Value + "' and DegreeCode='" + cbl_dept.Items[k].Value + "' and CollegeCode='" + collegecode1 + "' and HeaderFK='" + getheaderid + "' and LedgerFK='" + cblledgefine.Items[j].Value + "' and FromDay='" + frmday + "' and ToDay='" + today + "' and FineType='" + finetype + "' and FineSettingType='0') update FM_FineMaster set FineType='" + finetype + "',FineAmount='" + fneweek + "',DueDate='" + dut.ToString("MM/dd/yyyy") + "' where BatchYear='" + cbl_batch.Items[bt].Value + "' and FeeCatgory='" + cbl_sem.Items[l].Value + "' and DegreeCode='" + cbl_dept.Items[k].Value + "' and CollegeCode='" + collegecode1 + "' and HeaderFK='" + getheaderid + "' and LedgerFK='" + cblledgefine.Items[j].Value + "' and FromDay='" + frmday + "' and ToDay='" + today + "' and FineType='" + finetype + "' and FineSettingType='0' else Insert into FM_FineMaster (FineType,FromDay,ToDay,FineAmount,DueDate,HeaderFK,LedgerFK,FeeCatgory,DegreeCode,CollegeCode,FineSettingType,BatchYear) values ('" + finetype + "','" + frmday + "','" + today + "','" + fneweek + "','" + dut.ToString("MM/dd/yyyy") + "','" + getheaderid + "','" + cblledgefine.Items[j].Value + "','" + cbl_sem.Items[l].Value + "','" + cbl_dept.Items[k].Value + "','" + collegecode1 + "','0','" + cbl_batch.Items[bt].Value + "')";
                                                            inscount = d2.update_method_wo_parameter(insquery, "Text");
                                                            boolbtn = true;
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                //re admission fees settings
                                                insquery = "if exists (select * from FM_FineMaster where BatchYear='" + cbl_batch.Items[bt].Value + "' and  FeeCatgory ='" + cbl_sem.Items[l].Value + "' and DegreeCode='" + cbl_dept.Items[k].Value + "' and CollegeCode='" + collegecode1 + "' and HeaderFK='" + getheaderid + "' and LedgerFK='" + cblledgefine.Items[j].Value + "' and FineType='" + finetype + "' and FineSettingType='1') update FM_FineMaster set FineType='" + finetype + "',FineAmount='" + refineamt + "',DueDate='" + dutreadd.ToString("MM/dd/yyyy") + "' where BatchYear='" + cbl_batch.Items[bt].Value + "' and FeeCatgory ='" + cbl_sem.Items[l].Value + "' and DegreeCode='" + cbl_dept.Items[k].Value + "' and CollegeCode='" + collegecode1 + "' and HeaderFK='" + getheaderid + "' and LedgerFK='" + cblledgefine.Items[j].Value + "' and FineType='" + finetype + "' and FineSettingType='1'  else Insert into FM_FineMaster (FineType,FineAmount,DueDate,HeaderFK,LedgerFK,FeeCatgory,DegreeCode,CollegeCode,FineSettingType,BatchYear) values ('" + finetype + "','" + refineamt + "','" + dutreadd.ToString("MM/dd/yyyy") + "','" + getheaderid + "','" + cblledgefine.Items[j].Value + "','" + cbl_sem.Items[l].Value + "','" + cbl_dept.Items[k].Value + "','" + collegecode1 + "','1','" + cbl_batch.Items[bt].Value + "')";
                                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (inscount > 0)
            {
                if (boolbtn)
                    btnWeekFindDel.Visible = true;

                alertfine.Visible = true;
                lblfine.Visible = true;
                lblfine.Text = "Saved Successfully";
            }
        }
        catch
        {

        }
    }
    protected void btnfineclose_click(object sender, EventArgs e)
    {
        alertfine.Visible = false;
    }
    protected void btnexitfine_click(object sender, EventArgs e)
    {
        popfine.Visible = false;
    }
    protected void btnWeekFindDel_click(object sender, EventArgs e)
    {
        try
        {
            if (rb_perweek.Checked)
            {
                bool boolUpd = false;
                for (int hd = 0; hd < cblheadfine.Items.Count; hd++)
                {
                    if (cblheadfine.Items[hd].Selected)
                    {
                        for (int ld = 0; ld < cblledgefine.Items.Count; ld++)
                        {
                            if (cblledgefine.Items[ld].Selected)
                            {
                                for (int bt = 0; bt < cbl_batch.Items.Count; bt++)
                                {
                                    if (cbl_batch.Items[bt].Selected)
                                    {
                                        for (int dpt = 0; dpt < cbl_dept.Items.Count; dpt++)
                                        {
                                            if (cbl_dept.Items[dpt].Selected)
                                            {
                                                for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                                                {
                                                    if (cbl_sem.Items[sem].Selected)
                                                    {
                                                        string hdFK = Convert.ToString(cblheadfine.Items[hd].Value);
                                                        string delQ = "    delete from FM_FineMaster where BatchYear='" + cbl_batch.Items[bt].Value + "' and FeeCatgory='" + cbl_sem.Items[sem].Value + "' and DegreeCode='" + cbl_dept.Items[dpt].Value + "' and CollegeCode='" + collegecode1 + "' and HeaderFK='" + hdFK + "' and LedgerFK='" + cblledgefine.Items[ld].Value + "'  and FineType='3' and FineSettingType='0' ";
                                                        int inscount = d2.update_method_wo_parameter(delQ, "Text");
                                                        if (inscount > 0)
                                                            boolUpd = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (boolUpd)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                    rb_perweek_OnCheckedChanged(sender, e);
                    btnWeekFindDel.Visible = false;
                }
            }
        }
        catch { }
    }

    protected void imagepopclose_click(object sender, EventArgs e)
    {
        popfine.Visible = false;
    }
    protected void cbheadfine_CheckedChanged(object sender, EventArgs e)
    {
        if (cbheadfine.Checked == true)
        {
            for (int i = 0; i < cblheadfine.Items.Count; i++)
            {
                cblheadfine.Items[i].Selected = true;
            }
            txtheadfine.Text = "Header (" + (cblheadfine.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblheadfine.Items.Count; i++)
            {
                cblheadfine.Items[i].Selected = false;
            }
            txtheadfine.Text = "--Select--";
        }
        bindledger();
    }
    protected void cblheadfine_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtheadfine.Text = "--Select--";
        cbheadfine.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cblheadfine.Items.Count; i++)
        {
            if (cblheadfine.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtheadfine.Text = "Header(" + commcount.ToString() + ")";
            if (commcount == cblheadfine.Items.Count)
            {
                cbheadfine.Checked = true;
            }
        }
        bindledger();
    }
    protected void cbledgefine_CheckedChanged(object sender, EventArgs e)
    {
        if (cbledgefine.Checked == true)
        {
            for (int i = 0; i < cblledgefine.Items.Count; i++)
            {
                cblledgefine.Items[i].Selected = true;
            }
            txtledgerfine.Text = "Ledger(" + (cblledgefine.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblledgefine.Items.Count; i++)
            {
                cblledgefine.Items[i].Selected = false;
            }
            txtledgerfine.Text = "--Select--";
        }
    }
    protected void cblledgefine_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtledgerfine.Text = "--Select--";
        cbledgefine.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cblledgefine.Items.Count; i++)
        {
            if (cblledgefine.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtledgerfine.Text = "Ledger(" + commcount.ToString() + ")";
            if (commcount == cblledgefine.Items.Count)
            {
                cbledgefine.Checked = true;
            }
        }
    }
    public void bindheader()
    {
        try
        {
            ds.Clear();
            cblheadfine.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  order by len(isnull(hd_priority,10000)),hd_priority asc";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cblheadfine.DataSource = ds;
                cblheadfine.DataTextField = "HeaderName";
                cblheadfine.DataValueField = "HeaderPK";
                cblheadfine.DataBind();


                if (cblheadfine.Items.Count > 0)
                {
                    for (int i = 0; i < cblheadfine.Items.Count; i++)
                    {
                        cblheadfine.Items[i].Selected = true;
                    }
                    txtheadfine.Text = "Header(" + cblheadfine.Items.Count + ")";
                    cbheadfine.Checked = true;
                }
            }
            else
            {
                txtheadfine.Text = "--Select--";

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
            ds.Clear();
            cblledgefine.Items.Clear();

            string itemheader = "";
            for (int i = 0; i < cblheadfine.Items.Count; i++)
            {
                if (cblheadfine.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cblheadfine.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cblheadfine.Items[i].Value.ToString() + "";
                    }
                }
            }

            if (itemheader.Trim() != "")
            {
                string deptquery = "select distinct LedgerName,LedgerPK from FM_LedgerMaster f,FM_HeaderMaster h where h.HeaderPK=f.HeaderFK and f.HeaderFK in('" + itemheader + "') and f.LedgerMode='0' and f.CollegeCode =" + collegecode1 + "";
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblledgefine.DataSource = ds;
                    cblledgefine.DataTextField = "LedgerName";
                    cblledgefine.DataValueField = "LedgerPK";
                    cblledgefine.DataBind();
                    if (cblledgefine.Items.Count > 0)
                    {
                        for (int i = 0; i < cblledgefine.Items.Count; i++)
                        {
                            cblledgefine.Items[i].Selected = true;
                        }
                        txtledgerfine.Text = "Ledger(" + cblledgefine.Items.Count + ")";
                        cbledgefine.Checked = true;
                    }
                }
                else
                {
                    txtledgerfine.Text = "--Select--";
                    cbledgefine.Checked = false;
                }
            }
            else
            {
                txtledgerfine.Text = "--Select--";
                cbledgefine.Checked = false;
            }
        }
        catch
        {
        }
    }

    //addd by sudhagar 28-09-2016
    #region readmission fees settings
    protected void rbfine_Changed(object sender, EventArgs e)
    {
        tblfine.Visible = true;
        tblreadd.Visible = false;
        rb_perday.Checked = false;
        rb_perweek.Checked = false;
        rb_common.Checked = true;
        btnWeekFindDel.Visible = false;

    }
    protected void rbreadd_Changed(object sender, EventArgs e)
    {
        tblfine.Visible = false;
        tblperweek.Visible = false;
        tblreadd.Visible = true;
        btnWeekFindDel.Visible = false;
    }
    #endregion




    protected void view_click(object sender, EventArgs e)
    {
        try
        {
            //  btn_go_click(sender, e);
            //popper1.Visible = true;
            if (ddl_type.SelectedItem.Text == "Individual(Admitted)")
            {
                if (lnkview.Text == "Hide Details")
                {
                    lnkview.Text = "View Details";
                    FpSpreadstud.Visible = false;

                    Div3.Visible = false;
                    lblerr1.Visible = false;
                }
                else
                {
                    if (FpSpreadstud.Sheets[0].RowCount >= 2)
                    {
                        Div3.Visible = true;
                        // lnkview.Text = "Hide Details";
                        FpSpreadstud.Visible = true;
                        FpSpreadstud.Width = 800;
                        FpSpreadstud.Height = 400;
                        divview.Visible = true;

                        if (dirAccess.selectScalarInt("select LinkValue from New_InsSettings where LinkName='StudentsDisplayPositioninJournal' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ") == 1)
                        {
                            //New Position
                            divview.Attributes.Add("style", "height: 400px; z-index: 1000;width: 320px; background-color: rgba(54, 25, 25, .2); position: absolute; top: 300; left: 700px;");
                            div1.Attributes.Add("style", "height: 380px;width: 290px; overflow:auto; position:relative;");
                            studentdetail.Attributes.Add("style", "width: 270px; height: 350px;");
                            //imgbtn3.Attributes.Add("style", "height: 30px; width: 30px; position: absolute; margin-top: -22px; margin-left: 130px; z-index:100;");
                            imgbtn3.Visible = false;
                            divGridI.Attributes.Add("style", "width: 650px;position:absolute;");
                            divGridII.Attributes.Add("style", "width: 630px; height: 500px; overflow: auto;");
                            //gridLedgeDetails.Width = 650;
                            //FpSpread1.Width = 650;
                            FpSpreadstud.Width = 250;
                            //New Position end
                        }

                        studentdetail.Visible = true;
                        //  FpSpread1.Width = 600;
                        FpSpreadstud2.Visible = false;
                        FpSpreadstud3.Visible = false;
                    }
                    else if (FpSpreadstud.Sheets[0].RowCount == 1)
                    {
                        Div3.Visible = false;
                        lnkview.Text = "View Details";
                        lblerr1.Visible = true;
                        lblerr1.Text = "There are no students available!";
                    }
                    else
                    {
                        Div3.Visible = false;
                        lnkview.Text = "View Details";
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Click the go Button & then Proceed!";
                    }
                }
            }
            else if (ddl_type.SelectedItem.Text == "Individual(Applied)")
            {
                //FpSpreadstud2.Visible = true;
                //FpSpreadstud.Visible = false;
                //FpSpreadstud3.Visible = false;
                Div3.Visible = true;
                // lnkview.Text = "Hide Details";
                FpSpreadstud.Visible = true;
                FpSpreadstud.Width = 800;
                FpSpreadstud.Height = 400;
                divview.Visible = true;
                studentdetail.Visible = true;
                //  FpSpread1.Width = 600;
                FpSpreadstud2.Visible = false;
                FpSpreadstud3.Visible = false;
            }
            else if (ddl_type.SelectedItem.Text == "Individual(Both)")
            {
                FpSpreadstud3.Visible = true;
                FpSpreadstud.Visible = false;
                FpSpreadstud2.Visible = false;
            }


            //    FarPoint.Web.Spread.StyleInfo darknewstyle = new FarPoint.Web.Spread.StyleInfo();
            //    darknewstyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
            //    darknewstyle.ForeColor = System.Drawing.Color.Black;
            //    darknewstyle.HorizontalAlign = HorizontalAlign.Center;
            //    FpSpreadstud.ActiveSheetView.ColumnHeader.DefaultStyle = darknewstyle;
            //    FpSpreadstud.Sheets[0].ColumnCount = 4;
            //    FpSpreadstud.Sheets[0].RowCount = 0;
            //    FpSpreadstud.Sheets[0].RowHeader.Visible = false;
            //    FpSpreadstud.CommandBar.Visible = false;
            //    FpSpreadstud.Sheets[0].AutoPostBack = true;
            //    int i;
            //    string type = "";
            //    string edulevel = "";
            //    string mode = "";
            //    string stutype = "";
            //    string seattype = "";
            //    string course = "";
            //    string degreecode = "";
            //    string batchyear = "";

            //    for (i = 0; i < cbl_stream.Items.Count; i++)
            //    {
            //        if (cbl_stream.Items[i].Selected == true)
            //        {
            //            if (type == "")
            //            {
            //                type = "" + cbl_stream.Items[i].Value.ToString();
            //            }
            //            else
            //            {
            //                type += "','" + cbl_stream.Items[i].Value.ToString() + "";
            //            }
            //        }
            //    }

            //    for (i = 0; i < cbl_edulevel.Items.Count; i++)
            //    {
            //        if (cbl_edulevel.Items[i].Selected == true)
            //        {
            //            if (edulevel == "")
            //            {
            //                edulevel = "" + cbl_edulevel.Items[i].Value.ToString();
            //            }
            //            else
            //            {
            //                edulevel += "','" + cbl_edulevel.Items[i].Value.ToString() + "";
            //            }
            //        }
            //    }

            //    for (i = 0; i < cbl_type.Items.Count; i++)
            //    {
            //        if (cbl_type.Items[i].Selected == true)
            //        {
            //            if (mode == "")
            //            {
            //                mode = "" + cbl_type.Items[i].Value.ToString();
            //            }
            //            else
            //            {
            //                mode += "','" + cbl_type.Items[i].Value.ToString() + "";
            //            }
            //        }
            //    }

            //    for (i = 0; i < cbl_stutype.Items.Count; i++)
            //    {
            //        if (cbl_stutype.Items[i].Selected == true)
            //        {
            //            if (stutype == "")
            //            {
            //                stutype = "" + cbl_stutype.Items[i].Value.ToString();
            //            }
            //            else
            //            {
            //                stutype += "','" + cbl_stutype.Items[i].Value.ToString() + ""; ;
            //            }
            //        }
            //    }

            //    for (i = 0; i < cbl_dept.Items.Count; i++)
            //    {
            //        if (cbl_dept.Items[i].Selected)
            //        {
            //            if (degreecode == "")
            //            {
            //                degreecode = "" + cbl_dept.Items[i].Value.ToString();
            //            }
            //            else
            //            {
            //                degreecode += "','" + cbl_dept.Items[i].Value.ToString() + "";
            //            }
            //        }
            //    }
            //    for (i = 0; i < cbl_course.Items.Count; i++)
            //    {
            //        if (cbl_course.Items[i].Selected)
            //        {
            //            if (course == "")
            //            {
            //                course = "" + cbl_course.Items[i].Value.ToString();
            //            }
            //            else
            //            {
            //                course += "','" + cbl_course.Items[i].Value.ToString() + "";
            //            }
            //        }
            //    }

            //    for (i = 0; i < cbl_seat.Items.Count; i++)
            //    {
            //        if (cbl_seat.Items[i].Selected)
            //        {
            //            if (seattype == "")
            //            {
            //                seattype = "" + cbl_seat.Items[i].Value.ToString();
            //            }
            //            else
            //            {
            //                seattype += "','" + cbl_seat.Items[i].Value.ToString() + "";
            //            }
            //        }
            //    }

            //    for (i = 0; i < cbl_batch.Items.Count; i++)
            //    {
            //        if (cbl_batch.Items[i].Selected)
            //        {
            //            if (batchyear == "")
            //            {
            //                batchyear = "" + cbl_batch.Items[i].Text.ToString();
            //            }
            //            else
            //            {
            //                batchyear += "','" + cbl_batch.Items[i].Text.ToString() + "";
            //            }
            //        }
            //    }

            //    string selquery = "";
            //    selquery = "SELECT r.App_NO,Roll_No,R.Stud_Name,(C.Course_Name+'-'+D.Dept_Name) as Department  FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE A.app_no = r.App_No and R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and type in('" + type + "') and Edu_Level in('" + edulevel + "') and R.mode in('" + mode + "') and r.Stud_Type in('" + stutype + "') and a.seattype in('" + seattype + "') and r.batch_year in('" + batchyear + "') and g.Course_Id in('" + course + "')  and g.Degree_Code in('" + degreecode + "')";
            //    ds.Clear();
            //    ds = d2.select_method_wo_parameter(selquery, "Text");
            //    if (ds.Tables.Count > 0)
            //    {
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            FarPoint.Web.Spread.StyleInfo style5 = new FarPoint.Web.Spread.StyleInfo();
            //            style5.Font.Size = 13;
            //            style5.Font.Name = "Book Antiqua";
            //            style5.Font.Bold = true;
            //            style5.HorizontalAlign = HorizontalAlign.Center;
            //            style5.ForeColor = System.Drawing.Color.Black;
            //            style5.BackColor = System.Drawing.Color.AliceBlue;

            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            //            FpSpreadstud.Columns[0].Width = 50;


            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll Number";
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            //            FpSpreadstud.Columns[1].Width = 175;

            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            //            FpSpreadstud.Columns[2].Width = 200;

            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            //            FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            //            FpSpreadstud.Columns[3].Width = 150;

            //            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            //            {
            //                FpSpreadstud.Sheets[0].RowCount++;
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_NO"]);
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            //                FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
            //            }
            //            //FpSpread1.Visible = true;
            //            popper1.Visible = true;
            //            FpSpreadstud.Visible = true;
            //            FpSpreadstud.Sheets[0].PageSize = FpSpreadstud.Sheets[0].RowCount;
            //        }
            //    }
        }
        catch
        {

        }
    }


    static bool save = true;
    protected void FpSpreadstud_CellClick(object sender, EventArgs e)
    {
        try
        {
            // FpSpread1.SaveChanges();
            // FpSpreadstud.SaveChanges();
            // FpSpreadstud_SelectedIndexChanged(sender, e);
            spreadstud_click = true;
        }
        catch
        {

        }
    }
    protected void FpSpreadstud2_CellClick(object sender, EventArgs e)
    {
        try
        {
            // FpSpread1.SaveChanges();
            //FpSpreadstud2.SaveChanges();
            //FpSpreadstud2_SelectedIndexChanged(sender, e);
            spreadstud2_click = true;
        }
        catch { }
    }
    protected void FpSpreadstud3_CellClick(object sender, EventArgs e)
    {
        try
        {
            //FpSpread1.SaveChanges();
            //FpSpreadstud3.SaveChanges();
            // FpSpreadstud3_SelectedIndexChanged(sender, e);
            spreadstud3_click = true;
        }
        catch { }
    }
    //protected void FpSpreadstud_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{

    //}

    //protected void FpSpreadstud2_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{

    //}

    //protected void FpSpreadstud3_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{

    //}
    protected void FpSpreadstud_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch
        {

        }
    }
    protected void FpSpreadstud2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (spreadstud2_click == true)
            {
                // FpSpread1.SaveChanges();
                // FpSpreadstud2.SaveChanges();
                DataView dvnew = new DataView();
                DataView dv1 = new DataView();
                int i;
                int col;
                string startdate = "";
                string duedate = "";
                string day1 = "";
                string mon1 = "";
                string year1 = "";
                string day2 = "";
                string mon2 = "";
                string year2 = "";
                string feecategory = "";
                string header = "";
                string ledger = "";
                string[] split;
                //Grandtotal.Clear();
                //ledgertotal.Clear();
                //headertotal.Clear();

                string[] dtday = new string[31];
                for (int id = 1; id <= 31; id++)
                {
                    if (id.ToString().Length == 1)
                    {
                        dtday[id - 1] = "0" + Convert.ToString(id);
                    }
                    else
                    {
                        dtday[id - 1] = Convert.ToString(id);
                    }
                }
                string[] dtmon = new string[12] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
                string[] droparray = new string[2];
                string[] droparray1 = new string[ddl_detre.Items.Count];
                droparray[0] = "Regular";
                droparray[1] = "Monthly";
                string[] loadyear = new string[10];

                DateTime currdt = DateTime.Now;
                int curryear = currdt.Year;

                for (int ij = 0; ij < 10; ij++)
                {
                    loadyear[ij] = Convert.ToString(curryear - ij);
                }


                if (ddl_detre.Items.Count > 0)
                {
                    for (int re = 0; re < ddl_detre.Items.Count; re++)
                    {
                        if (re == 0)
                        {
                            droparray1[re] = " ";
                        }
                        else
                        {
                            droparray1[re] = Convert.ToString(ddl_detre.Items[re].Text);
                        }
                    }
                }

                //if (ddlyear1.Items.Count > 0)
                //{
                //    for (int yr = 0; yr < ddlyear1.Items.Count; yr++)
                //    {
                //        loadyear[yr] = Convert.ToString(ddlyear1.Items[yr].Text);
                //    }
                //}

                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected)
                    {
                        if (feecategory == "")
                        {
                            feecategory = "" + cbl_sem.Items[i].Value.ToString();
                        }
                        else
                        {
                            feecategory += "','" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                    }
                }

                for (i = 0; i < cbl_ledger.Items.Count; i++)
                {
                    if (cbl_ledger.Items[i].Selected)
                    {
                        if (ledger == "")
                        {
                            ledger = "" + cbl_ledger.Items[i].Value.ToString();
                        }
                        else
                        {
                            ledger += "','" + cbl_ledger.Items[i].Value.ToString() + "";
                        }
                    }
                }

                for (i = 0; i < cbl_header.Items.Count; i++)
                {
                    if (cbl_header.Items[i].Selected)
                    {
                        if (header == "")
                        {
                            header = "" + cbl_header.Items[i].Value.ToString();
                        }
                        else
                        {
                            header += "','" + cbl_header.Items[i].Value.ToString() + "";
                        }
                    }
                }

                FarPoint.Web.Spread.ComboBoxCellType cb = new FarPoint.Web.Spread.ComboBoxCellType(droparray);
                cb.UseValue = true;
                cb.AutoPostBack = true;
                cb.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cb1 = new FarPoint.Web.Spread.ComboBoxCellType(droparray1);
                cb1.UseValue = true;
                cb1.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbday1 = new FarPoint.Web.Spread.ComboBoxCellType(dtday);
                cbday1.UseValue = true;
                cbday1.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbmon1 = new FarPoint.Web.Spread.ComboBoxCellType(dtmon);
                cbmon1.UseValue = true;
                cbmon1.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbyear1 = new FarPoint.Web.Spread.ComboBoxCellType(loadyear);
                cbyear1.UseValue = true;
                cbyear1.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbday2 = new FarPoint.Web.Spread.ComboBoxCellType(dtday);
                cbday2.UseValue = true;
                cbday2.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbmon2 = new FarPoint.Web.Spread.ComboBoxCellType(dtmon);
                cbmon2.UseValue = true;
                cbmon2.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbyear2 = new FarPoint.Web.Spread.ComboBoxCellType(loadyear);
                cbyear2.UseValue = true;
                cbyear2.ShowButton = true;

                FarPoint.Web.Spread.DoubleCellType doubl = new FarPoint.Web.Spread.DoubleCellType();
                doubl.ErrorMessage = "Allow Numerics";

                Hashtable hatnew = new Hashtable();
                hatnew = (Hashtable)ViewState["colcountnew"];
                if (spreadstud2_click == true)
                {
                    FpSpreadstud2.Visible = false;

                    //popper1.Visible = false;

                    Hashtable tothash = new Hashtable();
                    Hashtable dedhash = new Hashtable();
                    Hashtable coltothash = new Hashtable();
                    Hashtable refhash = new Hashtable();
                    Hashtable frmhash = new Hashtable();
                    Hashtable finehash = new Hashtable();

                    Hashtable Grandtothash = new Hashtable();
                    Hashtable Granddedhash = new Hashtable();
                    Hashtable Grandcoltothash = new Hashtable();
                    Hashtable Grandrefhash = new Hashtable();
                    Hashtable Grandfrmhash = new Hashtable();
                    Hashtable Grandfinehash = new Hashtable();
                    int colindex;
                    int dedindex;
                    int coltotindex;
                    int refindex;
                    int frmindex;
                    int fineindex;
                    double totgrand = 0;
                    double dedgrand = 0;
                    double grandtot = 0;
                    double refundtot = 0;
                    double frmgovttot = 0;
                    double finetotal = 0;



                    string actrow = FpSpreadstud2.ActiveSheetView.ActiveRow.ToString();
                    string actcol = FpSpreadstud2.ActiveSheetView.ActiveColumn.ToString();

                    string app_no = Convert.ToString(FpSpreadstud2.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Tag);
                    Session["appform_no"] = app_no;
                    string selquery = "";

                    selquery = "select headerpk,headername,ledgerpk,ledgername from FM_HeaderMaster m,FM_LedgerMaster l where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure')  and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "')";

                    selquery = selquery + "select HeaderPK,HeaderName,LedgerPK,LedgerName,LedgerFK,f.HeaderFK,AllotDate,FeeCategory,PayMode,FeeAmount,DeductAmout,DeductReason,TotalAmount,RefundAmount,FromGovtAmt,convert(varchar(10),DueDate,103) as DueDate,FineAmount,convert(varchar(10),PayStartDate,103) as  PayStartDate from FM_HeaderMaster m,FM_LedgerMaster l,FT_FeeAllot f where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure') and m.HeaderPK=f.HeaderFK and l.LedgerPK=f.LedgerFK and App_No='" + app_no + "' and f.FeeCategory in('" + feecategory + "') and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "')";

                    selquery = selquery + " select distinct headerpk,headername from FM_HeaderMaster m,FM_LedgerMaster l where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure')  and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "')";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selquery, "Text");

                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                    {
                        int sno = 0;
                        for (int ik = 0; ik < ds.Tables[2].Rows.Count; ik++)
                        {
                            tothash.Clear();
                            dedhash.Clear();
                            coltothash.Clear();
                            refhash.Clear();
                            frmhash.Clear();
                            finehash.Clear();

                            double total = 0;
                            double dedtot = 0;
                            double coltot = 0;
                            double reftot = 0;
                            double frmtot = 0;
                            double finetot = 0;
                            //FpSpread1.Sheets[0].RowCount++;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[2].Rows[ik]["HeaderName"]);
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Locked = true;
                            //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);

                            ds.Tables[0].DefaultView.RowFilter = "HeaderPK='" + Convert.ToString(ds.Tables[2].Rows[ik]["HeaderPK"]) + "'";
                            dvnew = ds.Tables[0].DefaultView;
                            for (i = 0; i < dvnew.Count; i++)
                            {
                                //FpSpread1.Sheets[0].RowCount++;
                                //sno++;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[2].Rows[ik]["HeaderPK"]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvnew[i]["LedgerName"]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dvnew[i]["LedgerPK"]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                for (int cn = 0; cn < cbl_sem.Items.Count; cn++)
                                {
                                    col = 1;
                                    if (cbl_sem.Items[cn].Selected == true)
                                    {
                                        col = Convert.ToInt32(hatnew[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                        //dt = dv.ToTable();
                                        //dt1 = dv1.ToTable();
                                        //dv1 = new DataView(dt1);
                                        ds.Tables[1].DefaultView.RowFilter = "FeeCategory='" + cbl_sem.Items[cn].Value + "' and HeaderFK ='" + Convert.ToString(ds.Tables[2].Rows[ik]["HeaderPK"]) + "' and LedgerFK='" + Convert.ToString(dvnew[i]["LedgerPK"]) + "'";
                                        dv1 = ds.Tables[1].DefaultView;

                                        if (dv1.Count > 0)
                                        {
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cb;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Tag = Convert.ToString(dv1[0]["LedgerFK"]);
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["FeeAmount"]);
                                            if (Convert.ToString(dv1[0]["FeeAmount"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["FeeAmount"]);
                                                if (tothash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    total = Convert.ToInt32(tothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    total += getvalue;
                                                    tothash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    tothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(total));
                                                }
                                                else
                                                {
                                                    tothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            totgrand += total;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = doubl;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["DeductAmout"]);
                                            if (Convert.ToString(dv1[0]["DeductAmout"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["DeductAmout"]);
                                                if (dedhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    dedtot = Convert.ToInt32(dedhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    dedtot += getvalue;
                                                    dedhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    dedhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(dedtot));
                                                }
                                                else
                                                {
                                                    dedhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            dedgrand += dedtot;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = doubl;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cb1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["DeductReason"]);
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;


                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["TotalAmount"]);
                                            if (Convert.ToString(dv1[0]["TotalAmount"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["TotalAmount"]);
                                                if (coltothash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    coltot = Convert.ToInt32(coltothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    coltot += getvalue;
                                                    coltothash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    coltothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(coltot));
                                                }
                                                else
                                                {
                                                    coltothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            grandtot += coltot;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;


                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["RefundAmount"]);
                                            if (Convert.ToString(dv1[0]["RefundAmount"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["RefundAmount"]);
                                                if (refhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    reftot = Convert.ToInt32(refhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    reftot += getvalue;
                                                    refhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    refhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(reftot));
                                                }
                                                else
                                                {
                                                    refhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            refundtot += reftot;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;


                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["FromGovtAmt"]);
                                            if (Convert.ToString(dv1[0]["FromGovtAmt"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["FromGovtAmt"]);
                                                if (frmhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    frmtot = Convert.ToInt32(frmhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    frmtot += getvalue;
                                                    frmhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    frmhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(frmtot));
                                                }
                                                else
                                                {
                                                    frmhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            frmgovttot += frmtot;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Tag = Convert.ToString(dv1[0]["PayStartDate"]);
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //startdate = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag);
                                            if (startdate.Trim() != "01/01/1900")
                                            {
                                                split = startdate.Split('/');
                                                day2 = split[0];
                                                mon2 = split[1];
                                                year2 = split[2];
                                            }
                                            else
                                            {
                                                day2 = "";
                                                mon2 = "";
                                                year2 = "";
                                            }

                                            col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbday2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = day2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbmon2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = mon2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbyear2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = year2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["FineAmount"]);
                                            if (Convert.ToString(dv1[0]["FineAmount"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["FineAmount"]);
                                                if (finehash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    finetot = Convert.ToInt32(finehash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    finetot += getvalue;
                                                    finehash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    finehash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(finetot));
                                                }
                                                else
                                                {
                                                    finehash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            finetotal += finetot;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dv1[0]["DueDate"]);
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //duedate = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Tag);
                                            if (duedate.Trim() != "01/01/1900")
                                            {
                                                split = duedate.Split('/');
                                                day1 = split[0];
                                                mon1 = split[1];
                                                year1 = split[2];
                                            }
                                            else
                                            {
                                                day1 = "";
                                                mon1 = "";
                                                year1 = "";
                                            }
                                            col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbday1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = day1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbmon1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = mon1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbyear1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = year1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                        }
                                        else
                                        {
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cb;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = doubl;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = doubl;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cb1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbday2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbmon2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbyear2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbday1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbmon1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbyear1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                        }
                                    }
                                }
                            }
                            //FpSpread1.Sheets[0].RowCount++;
                            //if (!headertotal.ContainsKey(Convert.ToString(ds.Tables[2].Rows[ik]["HeaderPK"])))
                            //{
                            //    headertotal.Add(Convert.ToString(ds.Tables[2].Rows[ik]["HeaderPK"]), FpSpread1.Sheets[0].RowCount - 1);
                            //    ViewState["headertotal"] = headertotal;
                            //}
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                            //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Locked = true;
                            //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            for (int cn = 0; cn < cbl_sem.Items.Count; cn++)
                            {
                                col = 1;
                                if (cbl_sem.Items[cn].Selected == true)
                                {
                                    col = Convert.ToInt32(hatnew[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    col++;
                                    colindex = col;
                                    dedindex = col + 1;
                                    coltotindex = col + 3;
                                    refindex = col + 4;
                                    frmindex = col + 5;
                                    fineindex = col + 9;

                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colindex].Text = Convert.ToString(tothash[Convert.ToString(cbl_sem.Items[cn].Value)]);

                                    string headertotalGrand = Convert.ToString(tothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(headertotalGrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(headertotalGrand);
                                        if (Grandtothash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            totgrand = Convert.ToInt32(Grandtothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            totgrand += getvalue;
                                            Grandtothash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Grandtothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(totgrand));
                                        }
                                        else
                                        {
                                            Grandtothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }


                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dedindex].Text = Convert.ToString(dedhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    string dedtotalgrand = Convert.ToString(dedhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(dedtotalgrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(dedtotalgrand);
                                        if (Granddedhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            dedgrand = Convert.ToInt32(Granddedhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            dedgrand += getvalue;
                                            Granddedhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Granddedhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(dedgrand));
                                        }
                                        else
                                        {
                                            Granddedhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }


                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, coltotindex].Text = Convert.ToString(coltothash[Convert.ToString(cbl_sem.Items[cn].Value)]);

                                    string tottotalgrand = Convert.ToString(coltothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(tottotalgrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(tottotalgrand);
                                        if (Grandcoltothash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            grandtot = Convert.ToInt32(Grandcoltothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            grandtot += getvalue;
                                            Grandcoltothash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Grandcoltothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(grandtot));
                                        }
                                        else
                                        {
                                            Grandcoltothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }


                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, refindex].Text = Convert.ToString(refhash[Convert.ToString(cbl_sem.Items[cn].Value)]);

                                    string reftotalgrand = Convert.ToString(refhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(reftotalgrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(reftotalgrand);
                                        if (Grandrefhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            refundtot = Convert.ToInt32(Grandrefhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            refundtot += getvalue;
                                            Grandrefhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Grandrefhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(refundtot));
                                        }
                                        else
                                        {
                                            Grandrefhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }


                                    //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, frmindex].Text = Convert.ToString(frmhash[Convert.ToString(cbl_sem.Items[cn].Value)]);

                                    string frmtotalgrand = Convert.ToString(frmhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(frmtotalgrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(frmtotalgrand);
                                        if (Grandfrmhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            frmgovttot = Convert.ToInt32(Grandfrmhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            frmgovttot += getvalue;
                                            Grandfrmhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Grandfrmhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(frmgovttot));
                                        }
                                        else
                                        {
                                            Grandfrmhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }


                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, fineindex].Text = Convert.ToString(finehash[Convert.ToString(cbl_sem.Items[cn].Value)]);

                                    string finetotalgrand = Convert.ToString(finehash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(finetotalgrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(finetotalgrand);
                                        if (Grandfinehash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            finetotal = Convert.ToInt32(Grandfinehash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            finetotal += getvalue;
                                            Grandfinehash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Grandfinehash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(finetotal));
                                        }
                                        else
                                        {
                                            Grandfinehash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }

                                }
                            }
                            //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        }

                        //FpSpread1.Sheets[0].RowCount++;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                        //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Locked = true;
                        //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                        for (int cn = 0; cn < cbl_sem.Items.Count; cn++)
                        {
                            col = 1;
                            if (cbl_sem.Items[cn].Selected == true)
                            {
                                col = Convert.ToInt32(hatnew[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                col++;
                                colindex = col;
                                dedindex = col + 1;
                                coltotindex = col + 3;
                                refindex = col + 4;
                                frmindex = col + 5;
                                fineindex = col + 9;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colindex].Text = Convert.ToString(Grandtothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dedindex].Text = Convert.ToString(Granddedhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, coltotindex].Text = Convert.ToString(Grandcoltothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, refindex].Text = Convert.ToString(Grandrefhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, frmindex].Text = Convert.ToString(Grandfrmhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, fineindex].Text = Convert.ToString(Grandfinehash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                            }
                        }
                        //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        //Grandtotal.Add(Convert.ToString("GrandTotal"), FpSpread1.Sheets[0].RowCount - 1);
                        //ViewState["Grandtotal"] = Grandtotal;
                    }

                }
            }
        }
        catch
        {

        }
    }
    protected void FpSpreadstud3_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (spreadstud3_click == true)
            {
                // FpSpread1.SaveChanges();
                // FpSpreadstud3.SaveChanges();
                DataView dvnew = new DataView();
                DataView dv1 = new DataView();
                int i;
                int col;
                string startdate = "";
                string duedate = "";
                string day1 = "";
                string mon1 = "";
                string year1 = "";
                string day2 = "";
                string mon2 = "";
                string year2 = "";
                string feecategory = "";
                string header = "";
                string ledger = "";
                string[] split;
                //Grandtotal.Clear();
                //ledgertotal.Clear();
                //headertotal.Clear();

                string[] dtday = new string[31];
                for (int id = 1; id <= 31; id++)
                {
                    if (Convert.ToString(id).Length == 1)
                    {
                        dtday[id - 1] = "0" + Convert.ToString(id);
                    }
                    else
                    {
                        dtday[id - 1] = Convert.ToString(id);
                    }
                }
                string[] dtmon = new string[12] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
                string[] droparray = new string[2];
                string[] droparray1 = new string[ddl_detre.Items.Count];
                droparray[0] = "Regular";
                droparray[1] = "Monthly";
                string[] loadyear = new string[10];

                DateTime currdt = DateTime.Now;
                int curryear = currdt.Year;

                for (int ij = 0; ij < 10; ij++)
                {
                    loadyear[ij] = Convert.ToString(curryear - ij);
                }


                if (ddl_detre.Items.Count > 0)
                {
                    for (int re = 0; re < ddl_detre.Items.Count; re++)
                    {
                        if (re == 0)
                        {
                            droparray1[re] = " ";
                        }
                        else
                        {
                            droparray1[re] = Convert.ToString(ddl_detre.Items[re].Text);
                        }
                    }
                }

                //if (ddlyear1.Items.Count > 0)
                //{
                //    for (int yr = 0; yr < ddlyear1.Items.Count; yr++)
                //    {
                //        loadyear[yr] = Convert.ToString(ddlyear1.Items[yr].Text);
                //    }
                //}

                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected)
                    {
                        if (feecategory == "")
                        {
                            feecategory = "" + cbl_sem.Items[i].Value.ToString();
                        }
                        else
                        {
                            feecategory += "','" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                    }
                }

                for (i = 0; i < cbl_ledger.Items.Count; i++)
                {
                    if (cbl_ledger.Items[i].Selected)
                    {
                        if (ledger == "")
                        {
                            ledger = "" + cbl_ledger.Items[i].Value.ToString();
                        }
                        else
                        {
                            ledger += "','" + cbl_ledger.Items[i].Value.ToString() + "";
                        }
                    }
                }

                for (i = 0; i < cbl_header.Items.Count; i++)
                {
                    if (cbl_header.Items[i].Selected)
                    {
                        if (header == "")
                        {
                            header = "" + cbl_header.Items[i].Value.ToString();
                        }
                        else
                        {
                            header += "','" + cbl_header.Items[i].Value.ToString() + "";
                        }
                    }
                }

                FarPoint.Web.Spread.ComboBoxCellType cb = new FarPoint.Web.Spread.ComboBoxCellType(droparray);
                cb.UseValue = true;
                cb.AutoPostBack = true;
                cb.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cb1 = new FarPoint.Web.Spread.ComboBoxCellType(droparray1);
                cb1.UseValue = true;
                cb1.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbday1 = new FarPoint.Web.Spread.ComboBoxCellType(dtday);
                cbday1.UseValue = true;
                cbday1.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbmon1 = new FarPoint.Web.Spread.ComboBoxCellType(dtmon);
                cbmon1.UseValue = true;
                cbmon1.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbyear1 = new FarPoint.Web.Spread.ComboBoxCellType(loadyear);
                cbyear1.UseValue = true;
                cbyear1.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbday2 = new FarPoint.Web.Spread.ComboBoxCellType(dtday);
                cbday2.UseValue = true;
                cbday2.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbmon2 = new FarPoint.Web.Spread.ComboBoxCellType(dtmon);
                cbmon2.UseValue = true;
                cbmon2.ShowButton = true;

                FarPoint.Web.Spread.ComboBoxCellType cbyear2 = new FarPoint.Web.Spread.ComboBoxCellType(loadyear);
                cbyear2.UseValue = true;
                cbyear2.ShowButton = true;

                FarPoint.Web.Spread.DoubleCellType doubl = new FarPoint.Web.Spread.DoubleCellType();
                doubl.ErrorMessage = "Allow Numerics";

                Hashtable hatnew = new Hashtable();
                hatnew = (Hashtable)ViewState["colcountnew"];
                if (spreadstud3_click == true)
                {
                    FpSpreadstud3.Visible = false;

                    //popper1.Visible = false;

                    Hashtable tothash = new Hashtable();
                    Hashtable dedhash = new Hashtable();
                    Hashtable coltothash = new Hashtable();
                    Hashtable refhash = new Hashtable();
                    Hashtable frmhash = new Hashtable();
                    Hashtable finehash = new Hashtable();

                    Hashtable Grandtothash = new Hashtable();
                    Hashtable Granddedhash = new Hashtable();
                    Hashtable Grandcoltothash = new Hashtable();
                    Hashtable Grandrefhash = new Hashtable();
                    Hashtable Grandfrmhash = new Hashtable();
                    Hashtable Grandfinehash = new Hashtable();
                    int colindex;
                    int dedindex;
                    int coltotindex;
                    int refindex;
                    int frmindex;
                    int fineindex;
                    double totgrand = 0;
                    double dedgrand = 0;
                    double grandtot = 0;
                    double refundtot = 0;
                    double frmgovttot = 0;
                    double finetotal = 0;



                    string actrow = FpSpreadstud3.ActiveSheetView.ActiveRow.ToString();
                    string actcol = FpSpreadstud3.ActiveSheetView.ActiveColumn.ToString();

                    string app_no = Convert.ToString(FpSpreadstud3.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Tag);
                    Session["appformnew_no"] = app_no;
                    string selquery = "";

                    selquery = "select headerpk,headername,ledgerpk,ledgername from FM_HeaderMaster m,FM_LedgerMaster l where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure')  and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "')";

                    selquery = selquery + "select HeaderPK,HeaderName,LedgerPK,LedgerName,LedgerFK,f.HeaderFK,AllotDate,FeeCategory,PayMode,FeeAmount,DeductAmout,DeductReason,TotalAmount,RefundAmount,FromGovtAmt,convert(varchar(10),DueDate,103) as DueDate,FineAmount,convert(varchar(10),PayStartDate,103) as  PayStartDate from FM_HeaderMaster m,FM_LedgerMaster l,FT_FeeAllot f where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure') and m.HeaderPK=f.HeaderFK and l.LedgerPK=f.LedgerFK and App_No='" + app_no + "' and f.FeeCategory in('" + feecategory + "') and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "')";

                    selquery = selquery + " select distinct headerpk,headername from FM_HeaderMaster m,FM_LedgerMaster l where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure')  and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "')";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selquery, "Text");

                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                    {
                        int sno = 0;
                        for (int ik = 0; ik < ds.Tables[2].Rows.Count; ik++)
                        {
                            tothash.Clear();
                            dedhash.Clear();
                            coltothash.Clear();
                            refhash.Clear();
                            frmhash.Clear();
                            finehash.Clear();

                            double total = 0;
                            double dedtot = 0;
                            double coltot = 0;
                            double reftot = 0;
                            double frmtot = 0;
                            double finetot = 0;
                            //FpSpread1.Sheets[0].RowCount++;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[2].Rows[ik]["HeaderName"]);
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Locked = true;
                            //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);

                            ds.Tables[0].DefaultView.RowFilter = "HeaderPK='" + Convert.ToString(ds.Tables[2].Rows[ik]["HeaderPK"]) + "'";
                            dvnew = ds.Tables[0].DefaultView;
                            for (i = 0; i < dvnew.Count; i++)
                            {
                                //FpSpread1.Sheets[0].RowCount++;
                                //sno++;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[2].Rows[ik]["HeaderPK"]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvnew[i]["LedgerName"]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dvnew[i]["LedgerPK"]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                for (int cn = 0; cn < cbl_sem.Items.Count; cn++)
                                {
                                    col = 1;
                                    if (cbl_sem.Items[cn].Selected == true)
                                    {
                                        col = Convert.ToInt32(hatnew[Convert.ToString(cbl_sem.Items[cn].Value)]);

                                        ds.Tables[1].DefaultView.RowFilter = "FeeCategory='" + cbl_sem.Items[cn].Value + "' and HeaderFK ='" + Convert.ToString(ds.Tables[2].Rows[ik]["HeaderPK"]) + "' and LedgerFK='" + Convert.ToString(dvnew[i]["LedgerPK"]) + "'";
                                        dv1 = ds.Tables[1].DefaultView;

                                        if (dv1.Count > 0)
                                        {
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cb;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Tag = Convert.ToString(dv1[0]["LedgerFK"]);
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["FeeAmount"]);
                                            if (Convert.ToString(dv1[0]["FeeAmount"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["FeeAmount"]);
                                                if (tothash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    total = Convert.ToInt32(tothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    total += getvalue;
                                                    tothash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    tothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(total));
                                                }
                                                else
                                                {
                                                    tothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            totgrand += total;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = doubl;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["DeductAmout"]);
                                            if (Convert.ToString(dv1[0]["DeductAmout"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["DeductAmout"]);
                                                if (dedhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    dedtot = Convert.ToInt32(dedhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    dedtot += getvalue;
                                                    dedhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    dedhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(dedtot));
                                                }
                                                else
                                                {
                                                    dedhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            dedgrand += dedtot;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = doubl;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cb1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["DeductReason"]);
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;


                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["TotalAmount"]);
                                            if (Convert.ToString(dv1[0]["TotalAmount"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["TotalAmount"]);
                                                if (coltothash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    coltot = Convert.ToInt32(coltothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    coltot += getvalue;
                                                    coltothash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    coltothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(coltot));
                                                }
                                                else
                                                {
                                                    coltothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            grandtot += coltot;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;


                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["RefundAmount"]);
                                            if (Convert.ToString(dv1[0]["RefundAmount"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["RefundAmount"]);
                                                if (refhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    reftot = Convert.ToInt32(refhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    reftot += getvalue;
                                                    refhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    refhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(reftot));
                                                }
                                                else
                                                {
                                                    refhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            refundtot += reftot;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;


                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["FromGovtAmt"]);
                                            if (Convert.ToString(dv1[0]["FromGovtAmt"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["FromGovtAmt"]);
                                                if (frmhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    frmtot = Convert.ToInt32(frmhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    frmtot += getvalue;
                                                    frmhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    frmhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(frmtot));
                                                }
                                                else
                                                {
                                                    frmhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            frmgovttot += frmtot;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Tag = Convert.ToString(dv1[0]["DueDate"]);
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //duedate = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Tag);
                                            if (duedate.Trim() != "01/01/1900")
                                            {
                                                split = duedate.Split('/');
                                                day1 = split[0];
                                                mon1 = split[1];
                                                year1 = split[2];
                                            }
                                            else
                                            {
                                                day1 = "";
                                                mon1 = "";
                                                year1 = "";
                                            }

                                            col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbday1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = day1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbmon1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = mon1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbyear1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = year1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["FineAmount"]);
                                            if (Convert.ToString(dv1[0]["FineAmount"]).Trim() != "")
                                            {
                                                double getvalue = Convert.ToDouble(dv1[0]["FineAmount"]);
                                                if (finehash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                                {
                                                    finetot = Convert.ToInt32(finehash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                                    finetot += getvalue;
                                                    finehash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                                    finehash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(finetot));
                                                }
                                                else
                                                {
                                                    finehash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                                }
                                            }
                                            finetotal += finetot;

                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dv1[0]["PayStartDate"]);
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //startdate = Convert.ToString(FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag);
                                            if (startdate.Trim() != "01/01/1900")
                                            {
                                                split = startdate.Split('/');
                                                day2 = split[0];
                                                mon2 = split[1];
                                                year2 = split[2];
                                            }
                                            else
                                            {
                                                day2 = "";
                                                mon2 = "";
                                                year2 = "";
                                            }
                                            col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbday2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = day2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbmon2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = mon2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbyear2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = year2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                        }
                                        else
                                        {
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cb;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = doubl;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = doubl;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cb1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbday1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbmon1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbyear1;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbday2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbmon2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;

                                            //col++;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cbyear2;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                        }
                                    }
                                }
                            }
                            //FpSpread1.Sheets[0].RowCount++;
                            //if (!headertotal.ContainsKey(Convert.ToString(ds.Tables[2].Rows[ik]["HeaderPK"])))
                            //{
                            //    headertotal.Add(Convert.ToString(ds.Tables[2].Rows[ik]["HeaderPK"]), FpSpread1.Sheets[0].RowCount - 1);
                            //    ViewState["headertotal"] = headertotal;
                            //}
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                            //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Locked = true;
                            //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            for (int cn = 0; cn < cbl_sem.Items.Count; cn++)
                            {
                                col = 1;
                                if (cbl_sem.Items[cn].Selected == true)
                                {
                                    col = Convert.ToInt32(hatnew[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    col++;
                                    colindex = col;
                                    dedindex = col + 1;
                                    coltotindex = col + 3;
                                    refindex = col + 4;
                                    frmindex = col + 5;
                                    fineindex = col + 9;

                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colindex].Text = Convert.ToString(tothash[Convert.ToString(cbl_sem.Items[cn].Value)]);

                                    string headertotalGrand = Convert.ToString(tothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(headertotalGrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(headertotalGrand);
                                        if (Grandtothash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            totgrand = Convert.ToInt32(Grandtothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            totgrand += getvalue;
                                            Grandtothash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Grandtothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(totgrand));
                                        }
                                        else
                                        {
                                            Grandtothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }

                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dedindex].Text = Convert.ToString(dedhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    string dedtotalgrand = Convert.ToString(dedhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(dedtotalgrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(dedtotalgrand);
                                        if (Granddedhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            dedgrand = Convert.ToInt32(Granddedhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            dedgrand += getvalue;
                                            Granddedhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Granddedhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(dedgrand));
                                        }
                                        else
                                        {
                                            Granddedhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }

                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, coltotindex].Text = Convert.ToString(coltothash[Convert.ToString(cbl_sem.Items[cn].Value)]);

                                    string tottotalgrand = Convert.ToString(coltothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(tottotalgrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(tottotalgrand);
                                        if (Grandcoltothash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            grandtot = Convert.ToInt32(Grandcoltothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            grandtot += getvalue;
                                            Grandcoltothash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Grandcoltothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(grandtot));
                                        }
                                        else
                                        {
                                            Grandcoltothash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }


                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, refindex].Text = Convert.ToString(refhash[Convert.ToString(cbl_sem.Items[cn].Value)]);

                                    string reftotalgrand = Convert.ToString(refhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(reftotalgrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(reftotalgrand);
                                        if (Grandrefhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            refundtot = Convert.ToInt32(Grandrefhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            refundtot += getvalue;
                                            Grandrefhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Grandrefhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(refundtot));
                                        }
                                        else
                                        {
                                            Grandrefhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }


                                    //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, frmindex].Text = Convert.ToString(frmhash[Convert.ToString(cbl_sem.Items[cn].Value)]);

                                    string frmtotalgrand = Convert.ToString(frmhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(frmtotalgrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(frmtotalgrand);
                                        if (Grandfrmhash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            frmgovttot = Convert.ToInt32(Grandfrmhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            frmgovttot += getvalue;
                                            Grandfrmhash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Grandfrmhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(frmgovttot));
                                        }
                                        else
                                        {
                                            Grandfrmhash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }


                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, fineindex].Text = Convert.ToString(finehash[Convert.ToString(cbl_sem.Items[cn].Value)]);

                                    string finetotalgrand = Convert.ToString(finehash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                    if (Convert.ToString(finetotalgrand).Trim() != "")
                                    {
                                        double getvalue = Convert.ToDouble(finetotalgrand);
                                        if (Grandfinehash.ContainsKey(Convert.ToString(cbl_sem.Items[cn].Value)))
                                        {
                                            finetotal = Convert.ToInt32(Grandfinehash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                            finetotal += getvalue;
                                            Grandfinehash.Remove(Convert.ToString(cbl_sem.Items[cn].Value));
                                            Grandfinehash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(finetotal));
                                        }
                                        else
                                        {
                                            Grandfinehash.Add(Convert.ToString(cbl_sem.Items[cn].Value), Convert.ToString(getvalue));
                                        }
                                    }

                                }
                            }
                            //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        }

                        //FpSpread1.Sheets[0].RowCount++;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                        //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Locked = true;
                        //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                        for (int cn = 0; cn < cbl_sem.Items.Count; cn++)
                        {
                            col = 1;
                            if (cbl_sem.Items[cn].Selected == true)
                            {
                                col = Convert.ToInt32(hatnew[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                col++;
                                colindex = col;
                                dedindex = col + 1;
                                coltotindex = col + 3;
                                refindex = col + 4;
                                frmindex = col + 5;
                                fineindex = col + 9;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colindex].Text = Convert.ToString(Grandtothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dedindex].Text = Convert.ToString(Granddedhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, coltotindex].Text = Convert.ToString(Grandcoltothash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, refindex].Text = Convert.ToString(Grandrefhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, frmindex].Text = Convert.ToString(Grandfrmhash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, fineindex].Text = Convert.ToString(Grandfinehash[Convert.ToString(cbl_sem.Items[cn].Value)]);
                            }
                        }
                        //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        //Grandtotal.Add(Convert.ToString("GrandTotal"), FpSpread1.Sheets[0].RowCount - 1);
                        //ViewState["Grandtotal"] = Grandtotal;
                    }

                }
            }
        }
        catch
        {

        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void btn_exitaddreason_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addreason.Visible = false;
        txt_addreason.Text = "";

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
            if (lbl_stream.Text == "Stream")
            {
                if (cbl_stream.Items.Count == 1)
                {
                    txt_stream.Text = "" + stream + "";
                }
                else
                {
                    txt_stream.Text = lbl_stream.Text + "(" + (cbl_stream.Items.Count) + ")";
                }
            }
            if (lbl_stream.Text == "Shift")
            {
                if (cbl_stream.Items.Count == 1)
                {
                    txt_stream.Text = "" + stream + "";
                }
                else
                {
                    txt_stream.Text = lbl_stream.Text + "(" + (cbl_stream.Items.Count) + ")";
                }
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
        loadedulevel();
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
            if (lbl_stream.Text == "Shift")
            {
                if (commcount == 1)
                {
                    txt_stream.Text = "" + stream + "";
                }
                else
                {
                    txt_stream.Text = lbl_stream.Text + "(" + commcount.ToString() + ")";
                }
            }
            if (lbl_stream.Text == "Stream")
            {
                if (commcount == 1)
                {
                    txt_stream.Text = "" + stream + "";
                }
                else
                {
                    txt_stream.Text = lbl_stream.Text + "(" + commcount.ToString() + ")";
                }
            }
            if (commcount == cbl_stream.Items.Count)
            {
                cb_stream.Checked = true;
            }
        }
        loadedulevel();
        Bindcourse();
        binddept();
    }
    public void loadstream()
    {
        try
        {
            string stream = "";
            cbl_stream.Items.Clear();
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + collegecode1 + "')";
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
                        stream = Convert.ToString(cbl_stream.Items[i].Text);
                    }
                    if (lbl_stream.Text == "Stream")
                    {
                        if (cbl_stream.Items.Count == 1)
                        {
                            txt_stream.Text = "Stream(" + stream + ")";
                        }
                        else
                        {
                            txt_stream.Text = "Stream(" + cbl_stream.Items.Count + ")";
                        }
                    }
                    if (lbl_stream.Text == "Shift")
                    {
                        if (cbl_stream.Items.Count == 1)
                        {
                            txt_stream.Text = "Shift(" + stream + ")";
                        }
                        else
                        {
                            txt_stream.Text = "Shift(" + cbl_stream.Items.Count + ")";
                        }
                    }
                    cb_stream.Checked = true;
                    if (isStreamEnabled())
                        txt_stream.Enabled = true;
                    else
                        txt_stream.Enabled = false;
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
                txt_edulevel.Text = "Edu Level(" + (cbl_edulevel.Items.Count) + ")";
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
                txt_edulevel.Text = "Edu Level(" + commcount.ToString() + ")";
            }
        }
        Bindcourse();
        binddept();
    }
    public void loadedulevel()
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
            if (itemheader.Trim() != "" && txt_stream.Enabled)
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
                        txt_edulevel.Text = "Edu Level(" + edulevel + ")";
                    }
                    else
                    {
                        txt_edulevel.Text = "Edu Level(" + cbl_edulevel.Items.Count + ")";
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
    public void loadreligion()
    {
        try
        {
            string religion = "";
            cbl_religion.Items.Clear();
            string reliquery = "SELECT Distinct religion,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.religion AND R.college_code ='" + ddl_college.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(reliquery, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_religion.DataSource = ds;
                    cbl_religion.DataTextField = "TextVal";
                    cbl_religion.DataValueField = "religion";
                    cbl_religion.DataBind();
                    if (cbl_religion.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_religion.Items.Count; i++)
                        {
                            cbl_religion.Items[i].Selected = true;
                            religion = Convert.ToString(cbl_religion.Items[i].Text);
                        }
                        if (cbl_religion.Items.Count == 1)
                        {
                            txt_religion.Text = "" + religion + "";
                        }
                        else
                        {
                            txt_religion.Text = "Religion(" + cbl_religion.Items.Count + ")";
                        }
                        cb_religion.Checked = true;
                    }
                }
            }
            else
            {
                txt_religion.Text = "--Select--";
                cb_religion.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void cb_religion_CheckedChanged(object seender, EventArgs e)
    {
        try
        {
            string religion = "";
            if (cb_religion.Checked == true)
            {
                for (int i = 0; i < cbl_religion.Items.Count; i++)
                {
                    cbl_religion.Items[i].Selected = true;
                    religion = Convert.ToString(cbl_religion.Items[i].Text);
                }
                if (cbl_religion.Items.Count == 1)
                {
                    txt_religion.Text = "" + religion + "";
                }
                else
                {
                    txt_religion.Text = "Religion(" + (cbl_religion.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbl_religion.Items.Count; i++)
                {
                    cbl_religion.Items[i].Selected = false;
                }
                txt_religion.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void cbl_religion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_religion.Text = "--Select--";
            string religion = "";
            cb_religion.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_religion.Items.Count; i++)
            {
                if (cbl_religion.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    religion = Convert.ToString(cbl_religion.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_religion.Items.Count)
                {
                    cb_religion.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_religion.Text = "" + religion + "";
                }
                else
                {
                    txt_religion.Text = "Religion(" + commcount.ToString() + ")";
                }
            }
        }
        catch
        {

        }
    }
    public void loadcommunity()
    {
        try
        {
            string comm = "";
            string selq = "SELECT Distinct community,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.community  AND TextVal<>''AND R.college_code ='" + ddl_college.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_community.DataSource = ds;
                    cbl_community.DataTextField = "TextVal";
                    cbl_community.DataValueField = "community";
                    cbl_community.DataBind();
                    if (cbl_community.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_community.Items.Count; i++)
                        {
                            cbl_community.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_community.Items[i].Text);
                        }
                        if (cbl_community.Items.Count == 1)
                        {
                            txt_community.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_community.Text = "Community(" + cbl_community.Items.Count + ")";
                        }
                        cb_community.Checked = true;
                    }
                }
            }
            else
            {
                txt_community.Text = "--Select--";
                cb_community.Checked = false;
            }
        }
        catch
        {

        }
    }
    protected void cb_community_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string comm = "";
            if (cb_community.Checked == true)
            {
                for (int i = 0; i < cbl_community.Items.Count; i++)
                {
                    cbl_community.Items[i].Selected = true;
                    comm = Convert.ToString(cbl_community.Items[i].Text);
                }
                if (cbl_community.Items.Count == 1)
                {
                    txt_community.Text = "" + comm + "";
                }
                else
                {
                    txt_community.Text = "" + (cbl_community.Items.Count) + "";
                }
            }
            else
            {
                for (int i = 0; i < cbl_community.Items.Count; i++)
                {
                    cbl_community.Items[i].Selected = false;
                }
                txt_community.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void cbl_community_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_community.Text = "--Select--";
            cb_community.Checked = false;
            string comm = "";
            int commcount = 0;
            for (int i = 0; i < cbl_community.Items.Count; i++)
            {
                if (cbl_community.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    comm = Convert.ToString(cbl_community.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_community.Items.Count)
                {
                    cb_community.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_community.Text = "" + comm + "";
                }
                else
                {
                    txt_community.Text = "Community(" + commcount.ToString() + ")";
                }
            }
        }
        catch
        {

        }
    }
    public void cb_batch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            string batch = "";
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                    batch = Convert.ToString(cbl_batch.Items[i].Text);
                }
                if (cbl_batch.Items.Count == 1)
                {
                    txt_batch.Text = "" + batch + "";
                }
                else
                {
                    txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
            }
            //BindDegree();
            //bindbranch();
            //bindsem();
            //bindsec();

        }
        catch (Exception ex)
        {

        }

    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string buildvalue = "";
            string build = "";
            string batch = "";
            cb_batch.Checked = false;
            txt_batch.Text = "--Select--";


            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    batch = Convert.ToString(cbl_batch.Items[i].Text);
                    //cb_batch.Checked = false;
                    build = cbl_batch.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }

                }

            }


            if (commcount > 0)
            {
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_batch.Text = "" + batch + "";
                }
                else
                {
                    txt_batch.Text = "Batch(" + commcount.ToString() + ")";
                }
            }
            //BindDegree();
            //bindbranch();
            //bindsem();
            //bindsec();

        }
        catch (Exception ex)
        {

        }
    }
    public void BindBatch()
    {
        try
        {
            string batch = "";
            cbl_batch.Items.Clear();
            // hat.Clear();
            ds = d2.BindBatch();

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
            }
            if (cbl_batch.Items.Count > 0)
            {
                for (int row = 0; row < cbl_batch.Items.Count; row++)
                {
                    cbl_batch.Items[row].Selected = true;
                    cb_batch.Checked = true;
                    batch = Convert.ToString(cbl_batch.Items[row].Text);
                }
                if (cbl_batch.Items.Count == 1)
                {
                    txt_batch.Text = "Batch(" + batch + ")";
                }
                else
                {
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                }
            }

            else
            {

                txt_batch.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cb_course_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_course.Text = "--Select--";
            string course = "";
            if (cb_course.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_course.Items.Count; i++)
                {

                    cbl_course.Items[i].Selected = true;
                    course = Convert.ToString(cbl_course.Items[i].Text);
                }
                if (cbl_course.Items.Count == 1)
                {
                    txt_course.Text = "" + course + "";
                }
                else
                {
                    txt_course.Text = lbl_course.Text + "(" + (cbl_course.Items.Count) + ")";
                }
                //txt_course.Text = "Course(" + (cbl_course.Items.Count) + ")";

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
            int i = 0;
            int commcount = 0;
            cb_course.Checked = false;
            string course = "";
            txt_course.Text = "--Select--";
            for (i = 0; i < cbl_course.Items.Count; i++)
            {
                if (cbl_course.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    course = Convert.ToString(cbl_course.Items[i].Text);
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
                    txt_course.Text = "" + course + "";
                }
                else
                {
                    txt_course.Text = lbl_course.Text + "(" + commcount.ToString() + ")";
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
                //  string deptquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + collegecode1 + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "') and type in ('" + build1 + "')";
                string deptquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + collegecode1 + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
                if (build1.Trim() != "" && txt_stream.Enabled)
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
                        txt_course.Text = lbl_course.Text + "(" + cbl_course.Items.Count + ")";
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
                    txt_dept.Text = lbl_dept.Text + "(" + (cbl_dept.Items.Count) + ")";
                }
                // txt_dept.Text = "Department(" + (cbl_dept.Items.Count) + ")";
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
            // cbl_sem.Items.Clear();

            int commcount = 0;
            cb_dept.Checked = false;
            txt_dept.Text = "--Select--";
            //int commcount1 = 0;
            string deptname = "";
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
                    txt_dept.Text = lbl_dept.Text + "(" + commcount.ToString() + ")";
                }
                //  txt_dept.Text = "Department(" + commcount.ToString() + ")";

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
                            // build2 = build2 + "'" + "," + "'" + Convert.ToString(cbl_course.Items[i].Value);
                            build2 += "," + Convert.ToString(cbl_course.Items[i].Value);
                        }
                    }
                }
            }
            if (build2 != "")
            {
                // string deptquery = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + build2 + "') and degree.college_code in ('" + collegecode1 + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')and type in ('" + build1 + "')";
                //string deptquery = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and  department .dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + build2 + "') and degree.college_code in ('" + collegecode1 + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
                //ds.Clear();
                //ds = d2.select_method_wo_parameter(deptquery, "Text");
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
                        txt_dept.Text = lbl_dept.Text + "(" + cbl_dept.Items.Count + ")";
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
    protected void cb_seat_CheckedChanged(object sender, EventArgs e)
    {
        string seat = "";
        if (cb_seat.Checked == true)
        {
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                cbl_seat.Items[i].Selected = true;
                seat = Convert.ToString(cbl_seat.Items[i].Text);
            }
            if (cbl_seat.Items.Count == 1)
            {
                txt_seat.Text = "" + seat + "";
            }
            else
            {
                txt_seat.Text = "Seat(" + (cbl_seat.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                cbl_seat.Items[i].Selected = false;
            }
            txt_seat.Text = "--Select--";
        }

    }
    protected void cbl_seat_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_seat.Text = "--Select--";
        string seat = "";
        cb_seat.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_seat.Items.Count; i++)
        {
            if (cbl_seat.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                seat = Convert.ToString(cbl_seat.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_seat.Items.Count)
            {
                cb_seat.Checked = true;
            }
            if (commcount == 1)
            {
                txt_seat.Text = "" + seat + "";
            }
            else
            {
                txt_seat.Text = "Seat(" + commcount.ToString() + ")";
            }
        }

    }
    public void loadseat()
    {

        try
        {

            cbl_seat.Items.Clear();
            txt_seat.Text = "--Select--";
            cb_seat.Checked = false;
            string seat = "";
            string deptquery = "select distinct TextCode,TextVal from TextValTable  where TextCriteria='seat' and college_code='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_seat.DataSource = ds;
                cbl_seat.DataTextField = "TextVal";
                cbl_seat.DataValueField = "TextCode";
                cbl_seat.DataBind();
                if (ddl_type.SelectedIndex == 0)
                {
                    if (cbl_seat.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_seat.Items.Count; i++)
                        {
                            cbl_seat.Items[i].Selected = true;
                            seat = Convert.ToString(cbl_seat.Items[i].Text);
                        }
                        if (cbl_seat.Items.Count == 1)
                        {
                            txt_seat.Text = "Seat(" + seat + ")";
                        }
                        else
                        {
                            txt_seat.Text = "Seat(" + cbl_seat.Items.Count + ")";
                        }
                        cb_seat.Checked = true;
                    }
                }
            }
            else
            {
                txt_seat.Text = "--Select--";

            }
        }
        catch
        {
        }

    }
    protected void cb_type_CheckedChanged(object sender, EventArgs e)
    {
        string type = "";
        if (cb_type.Checked == true)
        {
            for (int i = 0; i < cbl_type.Items.Count; i++)
            {
                cbl_type.Items[i].Selected = true;
                type = Convert.ToString(cbl_type.Items[i].Text);
            }
            if (cbl_type.Items.Count == 1)
            {
                txt_type.Text = "" + type + "";
            }
            else
            {
                txt_type.Text = "Type(" + (cbl_type.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbl_type.Items.Count; i++)
            {
                cbl_type.Items[i].Selected = false;
            }
            txt_type.Text = "--Select--";
        }

    }
    protected void cbl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_type.Text = "--Select--";
        string type = "";
        cb_type.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_type.Items.Count; i++)
        {
            if (cbl_type.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                type = Convert.ToString(cbl_type.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_type.Items.Count)
            {
                cb_type.Checked = true;
            }
            if (commcount == 1)
            {
                txt_type.Text = "" + type + "";
            }
            else
            {
                txt_type.Text = "Type(" + commcount.ToString() + ")";
            }
        }

    }
    public void loadtype()
    {

        try
        {

            cbl_type.Items.Clear();

            string type = "";
            string deptquery = "select distinct case when mode =1 then 'Regular' when mode =3 then 'Lateral'  when mode =2 then 'Transfer' end as Modename,mode from Registration r,Degree g where r.degree_code = g.Degree_Code and g.college_code in('" + collegecode1 + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_type.DataSource = ds;
                cbl_type.DataTextField = "Modename";
                cbl_type.DataValueField = "mode";
                cbl_type.DataBind();

                if (cbl_type.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_type.Items.Count; i++)
                    {
                        cbl_type.Items[i].Selected = true;
                        type = Convert.ToString(cbl_type.Items[i].Text);
                    }
                    if (cbl_type.Items.Count == 1)
                    {
                        txt_type.Text = "Type(" + type + ")";
                    }
                    else
                    {
                        txt_type.Text = "Type(" + cbl_type.Items.Count + ")";
                    }
                    cb_type.Checked = true;
                }
            }
            else
            {
                txt_type.Text = "--Select--";

            }
        }
        catch
        {
        }

    }
    protected void cb_stutype_CheckedChanged(object sender, EventArgs e)
    {
        string studtype = "";
        if (cb_stutype.Checked == true)
        {
            for (int i = 0; i < cbl_stutype.Items.Count; i++)
            {
                cbl_stutype.Items[i].Selected = true;
                studtype = Convert.ToString(cbl_stutype.Items[i].Text);
            }
            if (cbl_stutype.Items.Count == 1)
            {
                txt_stutype.Text = "" + studtype + "";
            }
            else
            {
                txt_stutype.Text = "Type (" + (cbl_stutype.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbl_stutype.Items.Count; i++)
            {
                cbl_stutype.Items[i].Selected = false;
            }
            txt_stutype.Text = "--Select--";
        }

    }
    protected void cbl_stutype_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_stutype.Text = "--Select--";
        string studtype = "";
        cb_stutype.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_stutype.Items.Count; i++)
        {
            if (cbl_stutype.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                studtype = Convert.ToString(cbl_stutype.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_stutype.Items.Count)
            {
                cb_stutype.Checked = true;
            }
            if (commcount == 1)
            {
                txt_stutype.Text = "" + studtype + "";
            }
            else
            {
                txt_stutype.Text = "Type(" + commcount.ToString() + ")";
            }
        }

    }
    public void loadstutype()
    {

        try
        {

            cbl_stutype.Items.Clear();

            string studtype = "";
            string deptquery = "select distinct Stud_Type from Registration where college_code in('" + collegecode1 + "') and Stud_Type is not null and Stud_Type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stutype.DataSource = ds;
                cbl_stutype.DataTextField = "Stud_Type";
                // cbl_stutype.DataValueField = "mode";
                cbl_stutype.DataBind();

                if (cbl_stutype.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stutype.Items.Count; i++)
                    {
                        cbl_stutype.Items[i].Selected = true;
                        studtype = Convert.ToString(cbl_stutype.Items[i].Text);
                    }
                    if (cbl_stutype.Items.Count == 1)
                    {
                        txt_stutype.Text = "Type(" + studtype + ")";
                    }
                    else
                    {
                        txt_stutype.Text = "Type(" + cbl_stutype.Items.Count + ")";
                    }
                    cb_stutype.Checked = true;
                }
            }
            else
            {
                txt_stutype.Text = "--Select--";

            }
        }
        catch
        {
        }

    }
    protected void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        string sem = "";
        if (cb_sem.Checked == true)
        {
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                cbl_sem.Items[i].Selected = true;
                sem = Convert.ToString(cbl_sem.Items[i].Text);
            }
            if (lbl_sem.Text == "Semester")
            {
                if (cbl_sem.Items.Count == 1)
                {
                    txt_sem.Text = "" + sem + "";
                }
                else
                {
                    txt_sem.Text = "Sem(" + (cbl_sem.Items.Count) + ")";
                }
            }
            if (lbl_sem.Text == "Year")
            {
                if (cbl_sem.Items.Count == 1)
                {
                    txt_sem.Text = "" + sem + "";
                }
                else
                {
                    txt_sem.Text = "Year(" + (cbl_sem.Items.Count) + ")";
                }
            }
        }
        else
        {
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                cbl_sem.Items[i].Selected = false;
            }
            txt_sem.Text = "--Select--";
        }

    }
    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_sem.Text = "--Select--";
        cb_sem.Checked = false;
        string sem = "";
        int commcount = 0;
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
            if (lbl_sem.Text == "Semester")
            {
                if (commcount == 1)
                {
                    txt_sem.Text = "" + sem + "";
                }
                else
                {
                    txt_sem.Text = "Sem(" + commcount.ToString() + ")";
                }
            }
            if (lbl_sem.Text == "Year")
            {
                if (commcount == 1)
                {
                    txt_sem.Text = "" + sem + "";
                }
                else
                {
                    txt_sem.Text = "Year(" + commcount.ToString() + ")";
                }
            }
            if (commcount == cbl_sem.Items.Count)
            {
                cb_sem.Checked = true;
            }
        }
    }

    protected void loadsem()
    {
        try
        {
            string featDegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            cbl_sem.Items.Clear();
            txt_sem.Text = "--Select--";
            cb_sem.Checked = false;
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            d2.featDegreeCode = featDegcode;
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
    //protected void loadsem()
    //{
    //    try
    //    {
    //        string sem = "";
    //        //  string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
    //        string semyear = "select * from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
    //        DataSet dsset = new DataSet();
    //        dsset.Clear();
    //        dsset = d2.select_method_wo_parameter(semyear, "Text");
    //        if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
    //        {
    //            string value = Convert.ToString(dsset.Tables[0].Rows[0]["LinkValue"]);
    //            if (value == "1")
    //            {
    //                string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + collegecode1 + "' order by len (textval) ,textval asc";
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
    //                string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(settingquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //                    if (linkvalue == "0")
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + collegecode1 + "' order by len (textval) ,textval asc";
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
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + collegecode1 + "' order by len (textval) ,textval asc";
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
    protected void cb_header_CheckedChanged(object sender, EventArgs e)
    {
        string headername = "";
        if (cb_header.Checked == true)
        {
            for (int i = 0; i < cbl_header.Items.Count; i++)
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
                txt_header.Text = "Header (" + (cbl_header.Items.Count) + ")";
            }
            // txt_header.Text = "Header (" + (cbl_header.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_header.Items.Count; i++)
            {
                cbl_header.Items[i].Selected = false;
            }
            txt_header.Text = "--Select--";
        }
        loadledger();
    }
    protected void cbl_header_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_header.Text = "--Select--";
        cb_header.Checked = false;
        string headername = "";
        int commcount = 0;
        for (int i = 0; i < cbl_header.Items.Count; i++)
        {
            if (cbl_header.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                headername = Convert.ToString(cbl_header.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            // txt_header.Text = "Header(" + commcount.ToString() + ")";
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
                txt_header.Text = "Header (" + commcount.ToString() + ")";
            }
        }
        loadledger();
    }
    public void headerbind()
    {
        try
        {

            cbl_header.Items.Clear();

            //  string query = "select distinct HeaderName,HeaderPK from FM_HeaderMaster where CollegeCode='" + collegecode1 + "'";
            string query = " SELECT HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  order by len(isnull(hd_priority,10000)),hd_priority asc";
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
                    txt_header.Text = "Header(" + cbl_header.Items.Count + ")";
                    cb_header.Checked = true;
                }
            }
            else
            {
                txt_header.Text = "--Select--";

            }
        }

        catch
        {
        }
    }
    protected void cb_ledger_CheckedChanged(object sender, EventArgs e)
    {
        string ledgername = "";
        if (cb_ledger.Checked == true)
        {
            for (int i = 0; i < cbl_ledger.Items.Count; i++)
            {
                cbl_ledger.Items[i].Selected = true;
                ledgername = Convert.ToString(cbl_ledger.Items[i].Text);
            }
            if (cbl_ledger.Items.Count == 1)
            {
                txt_ledger.Text = "" + ledgername + "";
            }
            else
            {
                txt_ledger.Text = "Ledger (" + (cbl_ledger.Items.Count) + ")";
            }
            // txt_ledger.Text = "Ledger(" + (cbl_ledger.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_ledger.Items.Count; i++)
            {
                cbl_ledger.Items[i].Selected = false;
            }
            txt_ledger.Text = "--Select--";
        }

    }
    protected void cbl_ledger_SelectedIndexChanged(object sender, EventArgs e)
    {
        string ledgername = "";
        txt_ledger.Text = "--Select--";
        cb_ledger.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbl_ledger.Items.Count; i++)
        {
            if (cbl_ledger.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                ledgername = Convert.ToString(cbl_ledger.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            //   txt_ledger.Text = "Ledger(" + commcount.ToString() + ")";
            if (commcount == cbl_ledger.Items.Count)
            {
                cb_ledger.Checked = true;
            }
            if (commcount == 1)
            {
                txt_ledger.Text = "" + ledgername + "";
            }
            else
            {
                txt_ledger.Text = "Ledger (" + commcount.ToString() + ")";
            }
        }

    }
    public void loadledger()
    {
        try
        {
            ds.Clear();
            cbl_ledger.Items.Clear();

            string itemheader = "";
            for (int i = 0; i < cbl_header.Items.Count; i++)
            {
                if (cbl_header.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_header.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_header.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                // string deptquery = "select distinct LedgerName,LedgerPK from FM_LedgerMaster l,FM_HeaderMaster h where l.HeaderFK = h.HeaderPK and l.HeaderFK in('" + itemheader + "') and l.LedgerMode='0' and l.CollegeCode =" + collegecode1 + "";
                string deptquery = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode1 + "  and L.HeaderFK in('" + itemheader + "') order by len(isnull(l.priority,1000)) , l.priority asc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_ledger.DataSource = ds;
                    cbl_ledger.DataTextField = "LedgerName";
                    cbl_ledger.DataValueField = "LedgerPK";
                    cbl_ledger.DataBind();
                    if (cbl_ledger.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_ledger.Items.Count; i++)
                        {
                            cbl_ledger.Items[i].Selected = true;
                        }
                        txt_ledger.Text = "Ledger(" + cbl_ledger.Items.Count + ")";
                        cb_ledger.Checked = true;
                    }
                }
                else
                {
                    txt_ledger.Text = "--Select--";
                    cb_ledger.Checked = false;
                }
            }
            else
            {
                txt_ledger.Text = "--Select--";
                cb_ledger.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void btn_plus_detre_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addreason.Visible = true;
        lbl_addreason.Text = "Add Reason";
        lblerror.Visible = false;
    }
    protected void bindaddreason()
    {
        try
        {
            ddl_detre.Items.Clear();
            ds.Clear();
            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_detre.DataSource = ds;
                ddl_detre.DataTextField = "TextVal";
                ddl_detre.DataValueField = "TextCode";
                ddl_detre.DataBind();
                ddl_detre.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddl_detre.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        { }
    }
    protected void btn_minus_detre_Click(object sender, EventArgs e)
    {
        try
        {
            imgDiv1.Visible = true;
            lblconfirm.Visible = true;
            lblconfirm.Text = "Do you want to delete this Record?";
        }
        catch { }
    }
    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_detre.SelectedIndex != 0)
            {
                string sql = "delete from TextValTable where TextCode='" + ddl_detre.SelectedItem.Value.ToString() + "' and TextCriteria='DedRe' and college_code='" + collegecode1 + "' ";
                int delete = d2.update_method_wo_parameter(sql, "Text");
                if (delete != 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Deleted Successfully";
                    imgDiv1.Visible = false;
                    lblconfirm.Visible = false;
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Selected";
                    imgDiv1.Visible = false;
                    lblconfirm.Visible = false;
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Selected";
                imgDiv1.Visible = false;
                lblconfirm.Visible = false;
            }
            bindaddreason();
        }
        catch
        {

        }
    }
    protected void btnno_Click(object sender, EventArgs e)
    {
        imgDiv1.Visible = false;
        lblconfirm.Visible = false;
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void btn_addreason_Click(object sender, EventArgs e)
    {
        try
        {
            if (lbl_addreason.Text == "Add Reason")
            {
                if (txt_addreason.Text != "")
                {
                    string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_addreason.Text + "' and TextCriteria ='DedRe' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txt_addreason.Text + "' where TextVal ='" + txt_addreason.Text + "' and TextCriteria ='DedRe' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_addreason.Text + "','DedRe','" + collegecode1 + "')";
                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Saved Successfully";
                        txt_addreason.Text = "";
                        plusdiv.Visible = false;
                        panel_addreason.Visible = false;
                    }
                    bindaddreason();
                    txt_addreason.Text = "";
                }
                else
                {
                    //alertpopwindow.Visible = true;
                    plusdiv.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = "Enter the Reason";
                }
            }
        }
        catch
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
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    //if (tborder.Text == "")
                    //{
                    //    ItemList.Add("Company Code");
                    //}
                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    // if (cblcolumnorder.Items[index].Text.ToString().Trim().ToLower() == "scholarship")


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
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder.Items[0].Selected = true;
                //    cblcolumnorder.Items[1].Selected = true;
                //    cblcolumnorder.Items[2].Selected = true;
                //}
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Remove(sindex);
                }
            }

            lnk_columnorder.Visible = true;
            tborder.Visible = false;
            tborder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                //tborder.Text = tborder.Text + ItemList[i].ToString();

                //tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";

            }
            tborder.Text = colname12;
            if (ItemList.Count == 14)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
            tborder.Visible = false;
            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }

    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                tborder.Text = "";
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
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";

                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    // tborder.Text = tborder.Text + ItemList[i].ToString();



                }
                tborder.Text = colname12;

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
                    //cblcolumnorder.Items[0].Selected = true;
                }

                tborder.Text = "";
                tborder.Visible = false;

            }
            tborder.Visible = false;
            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    public void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public string newvalue(int index)
    {
        string valu = "";
        if (index == 4)
        {
            valu = "D";
        }
        return valu;
    }
    public static string getcolidx(int index)
    {
        int quotient = (index) / 26;

        if (quotient > 0)
        {
            return getcolidx(quotient - 1) + (char)((index % 26) + 65);
        }
        else
        {
            return "" + (char)((index % 26) + 65);
        }
    }
    public string getidx(int value)
    {
        columnname = getcolidx(value);
        return columnname;
    }
    protected void FpSpreadstud_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpreadstud.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpreadstud.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpreadstud.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpreadstud.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpreadstud.Sheets[0].RowCount; i++)
                        {
                            FpSpreadstud.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpreadstud.Sheets[0].RowCount; i++)
                        {
                            FpSpreadstud.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch { }
    }
    protected void btnexitstud_Click(object sender, EventArgs e)
    {
        lnkview.Text = "View Details";
        FpSpreadstud.Visible = false;
        divview.Visible = false;
        Div3.Visible = false;
        lblerr1.Visible = false;

    }
    public void loadsetting()
    {
        try
        {
            ListItem list1 = new ListItem("Roll No", "0");
            ListItem list2 = new ListItem("Reg No", "1");
            ListItem list3 = new ListItem("Admission No", "2");
            ListItem list4 = new ListItem("App No", "3");

            if (ddl_type.SelectedItem.Text.Trim() == "Individual(Admitted)")
            {
                rbl_rollno.Items.Clear();
                string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";

                int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

                if (save1 == 1)
                {
                    rbl_rollno.Items.Add(list1);
                }


                insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                if (save1 == 1)
                {
                    rbl_rollno.Items.Add(list2);
                }

                insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                if (save1 == 1)
                {
                    rbl_rollno.Items.Add(list3);
                }

                insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
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
                }
            }
            else if (ddl_type.SelectedItem.Text.Trim() == "Individual(Applied)")
            {
                rbl_rollno.Items.Clear();
                rbl_rollno.Items.Add(list4);
                txt_roll.Attributes.Add("placeholder", "App No");
                chosedmode = 3;
            }

        }
        catch { }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_roll.Text = "";
            txt_name.Text = "";
            if (ddl_type.SelectedItem.Text.Trim() == "Individual(Admitted)")
            {
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
                        chosedmode = 2;
                        break;
                }
            }
            else if (ddl_type.SelectedItem.Text.Trim() == "Individual(Applied)")
            {
                txt_roll.Attributes.Add("Placeholder", "App No");
                chosedmode = 2;
            }
        }
        catch { }
    }
    public void imagebtnorder_Click(object sender, EventArgs e)
    {
        divview.Visible = false;
    }
    //Code added by Mohamed Idhris 04-03-2016
    //Add and Remove Reasons for Scholarship
    protected void btnplusMulSclReason_OnClick(object sender, EventArgs e)
    {
        imgdiv3.Visible = true;
        panel_description.Visible = true;
    }
    protected void btnminusMulSclReason_OnClick(object sender, EventArgs e)
    {
        if (ddl_MulSclReason.Items.Count > 0)
        {
            surediv.Visible = true;
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_erroralert.Text = "No Scholarship Type Selected";
        }
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        try
        {
            surediv.Visible = false;
            if (ddl_MulSclReason.Items.Count > 0)
            {

                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_MulSclReason.SelectedItem.Value.ToString() + "' and MasterCriteria ='SchlolarshipReason' and CollegeCode='" + collegecode1 + "' ";
                int delete = d2.update_method_wo_parameter(sql, "TEXT");
                if (delete != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Deleted Sucessfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Not deleted";
                }
                loaddesc1();
            }

            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "No Scholarship Type Selected";
            }
        }
        catch { }
    }
    protected void btndescpopadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_description11.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode1 + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','SchlolarshipReason','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Saved sucessfully";
                    txt_description11.Text = "";
                    imgdiv3.Visible = false;
                    panel_description.Visible = false;
                }
                loaddesc1();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Enter the description";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btndescpopexit_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
        panel_description.Visible = false;
        loaddesc1();
    }
    public void loaddesc1()
    {
        try
        {
            ddl_MulSclReason.Items.Clear();
            string query = " select Distinct MasterValue,MasterCode from CO_MasterValues where MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode1 + "' order by MasterValue asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_MulSclReason.DataSource = ds;
                    ddl_MulSclReason.DataTextField = "MasterValue";
                    ddl_MulSclReason.DataValueField = "MasterCode";
                    ddl_MulSclReason.DataBind();
                }
            }
        }
        catch { }
    }
    protected void btnerrexit_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    //Multiple Scholarship Popup
    private void LoadMulScholarship(string appno)
    {
        try
        {
            string query = " select Distinct MasterValue,MasterCode from CO_MasterValues where MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode1 + "' order by MasterValue asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");

            string app_no = "0";
            if (txt_roll.Text.Trim() != "")
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    app_no = d2.GetFunction(" select App_No from Registration where Roll_No='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    app_no = d2.GetFunction(" select App_No from Registration where reg_no='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    app_no = d2.GetFunction(" select App_No from Registration where Roll_admit='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                {
                    app_no = d2.GetFunction(" select app_no from applyn where app_formno='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
            }
            else
            {
                if (appno != "")
                    app_no = appno;
            }
            string feecat = "0";
            string ledger = "0";

            int actrow = 0;//Convert.ToInt32(FpSpread1.Sheets[0].ActiveRow.ToString());
            int actcol = 0;//Convert.ToInt32(FpSpread1.Sheets[0].ActiveColumn.ToString());
            if (actrow > 0 && actcol > 0)
            {
                ledger = "0";// FpSpread1.Sheets[0].Cells[actrow, 1].Tag.ToString();
                feecat = "0";//FpSpread1.Sheets[0].Cells[actrow, actcol].Tag.ToString();
            }

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                FpSchloar.Sheets[0].RowHeader.Visible = false;
                FpSchloar.CommandBar.Visible = false;
                FpSchloar.Sheets[0].AutoPostBack = false;
                FpSchloar.Sheets[0].RowCount = 0;
                FpSchloar.Sheets[0].ColumnCount = 3;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 0].Column.Width = 50;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Scholarship";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 1].Column.Width = 100;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;

                FpSchloar.Columns[0].Locked = true;
                FpSchloar.Columns[1].Locked = true;

                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Amount";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 2].Column.Width = 80;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FarPoint.Web.Spread.DoubleCellType intgrcell = new FarPoint.Web.Spread.DoubleCellType();
                intgrcell.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();

                intgrcell.MinimumValue = 0;
                intgrcell.ErrorMessage = "Enter valid Number";
                FpSchloar.Sheets[0].Columns[2].CellType = intgrcell;
                FpSchloar.Sheets[0].Columns[2].Font.Bold = false;
                FpSchloar.Sheets[0].Columns[2].Font.Name = "Book Antiqua";

                FarPoint.Web.Spread.IntegerCellType integer = new FarPoint.Web.Spread.IntegerCellType();
                FpSchloar.Sheets[0].Columns[2].CellType = integer;
                double totOvall = 0;
                for (int scl = 0; scl < ds.Tables[0].Rows.Count; scl++)
                {
                    FpSchloar.Sheets[0].RowCount++;
                    FpSchloar.Sheets[0].Cells[scl, 0].Font.Bold = false;
                    FpSchloar.Sheets[0].Cells[scl, 0].Text = (scl + 1).ToString();
                    FpSchloar.Sheets[0].Cells[scl, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSchloar.Sheets[0].Cells[scl, 0].Font.Name = "Book Antiqua";

                    FpSchloar.Sheets[0].Cells[scl, 1].Font.Bold = false;
                    FpSchloar.Sheets[0].Cells[scl, 1].Text = Convert.ToString(ds.Tables[0].Rows[scl]["MasterValue"]);
                    FpSchloar.Sheets[0].Cells[scl, 1].Tag = Convert.ToString(ds.Tables[0].Rows[scl]["MasterCode"]);
                    FpSchloar.Sheets[0].Cells[scl, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSchloar.Sheets[0].Cells[scl, 1].Font.Name = "Book Antiqua";
                    FpSchloar.Sheets[0].Cells[scl, 1].Font.Size = FontUnit.Medium;

                    FpSchloar.Sheets[0].Cells[scl, 2].CellType = intgrcell;
                    #region old
                    //if (txt_roll.Text.Trim() != "")
                    //{
                    //    double dbValue = 0;
                    //    double.TryParse(d2.GetFunction("select isnull(TotalAmount,0) as schl from FT_FinScholarship where LedgerFK=" + ledger + "  and CollegeCode=" + collegecode1 + " and Feecategory=" + feecat + " and App_no=" + app_no + " and ReasonCode=" + ds.Tables[0].Rows[scl]["MasterCode"] + ""), out dbValue);
                    //    FpSchloar.Sheets[0].Cells[scl, 2].Text = dbValue.ToString();
                    //    totOvall += dbValue;
                    //}
                    //else
                    //{
                    //    FpSchloar.Sheets[0].Cells[scl, 2].Text = "";
                    //}
                    #endregion

                    if (app_no != "")
                    {
                        double dbValue = 0;
                        double.TryParse(d2.GetFunction("select isnull(TotalAmount,0) as schl from FT_FinScholarship where LedgerFK=" + ledger + "  and CollegeCode=" + collegecode1 + " and Feecategory=" + feecat + " and App_no=" + app_no + " and ReasonCode=" + ds.Tables[0].Rows[scl]["MasterCode"] + ""), out dbValue);
                        FpSchloar.Sheets[0].Cells[scl, 2].Text = dbValue.ToString();
                        totOvall += dbValue;
                    }
                    else
                    {
                        FpSchloar.Sheets[0].Cells[scl, 2].Text = "";
                    }
                }

                FpSchloar.Sheets[0].RowCount++;
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 0].Text = "Total Amount";
                FpSchloar.Sheets[0].SpanModel.Add(FpSchloar.Sheets[0].RowCount - 1, 0, 1, 2);
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 2].Text = totOvall.ToString();
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 2].Locked = true;
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                FpSchloar.Height = 350;

                FpSchloar.Sheets[0].PageSize = FpSchloar.Sheets[0].RowCount;
                FpSchloar.SaveChanges();
            }
            else
            {
                divMulSchlolar.Visible = false;
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "No Scholarship Type Available";
            }
        }
        catch { }
    }
    protected void FpSchloar_Commandold(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            int col = (FpSchloar.Sheets[0].RowCount) - 1;
            FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 2].Formula = "SUM(C1:C" + col + ")";
        }
        catch { }
    }
    protected void btnExitScholar_Clickold(object sender, EventArgs e)
    {
        ReplaceScholarshipamount(true);
    }
    private void ReplaceScholarshipamount(bool fromExit)
    {
        try
        {
            string mulScholar = "";
            string monthamount = "";

            string actrow = "0";//FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = "0";//FpSpread1.Sheets[0].ActiveColumn.ToString();
            int col = Convert.ToInt32(actcol);
            int colindex = col + 1;

            if (!fromExit)
            {
                monthamount = "0";
                mulScholar = "";
            }
            else
            {

                FpSchloar.SaveChanges();
                for (int i = 0; i < FpSchloar.Sheets[0].RowCount - 1; i++)
                {
                    if (FpSchloar.Sheets[0].Cells[i, 2].Text.Trim() != "")
                    {
                        if (mulScholar == "")
                        {
                            mulScholar = "" + FpSchloar.Sheets[0].Cells[i, 1].Tag + ":" + FpSchloar.Sheets[0].Cells[i, 2].Text + "";
                        }
                        else
                        {
                            mulScholar = mulScholar + "," + FpSchloar.Sheets[0].Cells[i, 1].Tag + ":" + FpSchloar.Sheets[0].Cells[i, 2].Text + "";
                        }
                    }
                }
                monthamount = FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 2].Text;
                divMulSchlolar.Visible = false;
            }

            //FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt32(colindex)].Text = monthamount;
            //FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt32(colindex)].Tag = mulScholar;
            //FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt32(colindex)].Font.Size = FontUnit.Medium;
            //FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt32(colindex)].HorizontalAlign = HorizontalAlign.Right;
            //FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt32(colindex)].Locked = true;

        }
        catch { }
    }
    private string GetSelectedItemsValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
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
    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    //Month wise Fees Allocation
    //Name Search
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetName(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            WebService ws = new WebService();
            string qAdd = string.Empty;
            if (chosedmode == 0)
            {
                qAdd = "r.Roll_No,r.Roll_No ";
            }
            else if (chosedmode == 1)
            {
                qAdd = "r.Reg_No,r.Reg_No ";
            }
            else if (chosedmode == 2)
            {
                qAdd = "r.Roll_admit,r.Roll_admit ";
            }
            else
            {
                qAdd = "a.app_formno,a.app_formno ";
            }
            string query = "";
            if (applied == 0)
            {
                query = "select  top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+" + qAdd + " from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%' and r.college_code=" + collegecode1 + "";
                if (distcon == 1)
                    query += " and r.DelFlag =0";
                if (compl == 1)
                    query += " and r.CC=0";
            }
            else
            {
                query = " select top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+" + qAdd + " from applyn a,Course C,Degree d,Department dt where a.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and a.college_code=" + collegecode1 + " and a.stud_name like '" + prefixText + "%'";
            }

            Hashtable studhash = ws.Getnamevalue(query);
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
        catch { return name; }
    }
    protected void getDiscontinue()
    {
        try
        {
            distcon = 0;
            compl = 0;
            if (dirAccess.selectScalarInt("select LinkValue from New_InsSettings where LinkName='IncludeDiscontinuedInJournal' and user_code ='" + usercode + "'  ") == 0)//and college_code ='" + collegecode1 + "'
                distcon = 1;
            if (dirAccess.selectScalarInt("select LinkValue from New_InsSettings where LinkName='IncludeCompletedInJournal' and user_code ='" + usercode + "'  ") == 0)//and college_code ='" + collegecode1 + "'
                compl = 1;
        }
        catch { }
    }

    protected void txt_name_Changed(object sender, EventArgs e)
    {
        try
        {
            string roll_no = Convert.ToString(txt_name.Text);

            if (roll_no != "")
            {
                try
                {
                    string rollno = roll_no.Split('-')[4];
                    roll_no = rollno;
                }
                catch { roll_no = ""; }
            }
            txt_roll.Text = roll_no;
        }
        catch { }
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
    private byte StudentAppliedShorlistAdmit()
    {

        string Q = "select LinkValue from New_InsSettings where LinkName='StudentAppliedShorlistAdmit' and user_code ='" + usercode + "'";
        byte moveVal = 0;
        byte.TryParse(d2.GetFunction(Q.Trim()), out moveVal);
        return moveVal;
    }
    //Code ended by Mohamed Idhris -- Last Modified - 11-06-2016

    //Code for Gridview Added by Idhris 13-08-2016
    //Load grid and search
    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadseat();
            txt_roll.Text = "";
            txt_name.Text = "";
            FpSpreadstud.Rows.Count = 0;
            FpSpreadstud.SaveChanges();
            if (ddl_type.SelectedItem.Text.Trim() == "Individual(Admitted)")
            {
                applied = 0;
                getDiscontinue();
            }
            else if (ddl_type.SelectedItem.Text.Trim() == "Individual(Applied)")
            {
                applied = 1;
            }
            if (ddl_type.SelectedItem.Text == "General")
            {
                lnkview.Visible = false;
                FpSpreadstud.Visible = false;
                Div3.Visible = false;
                txt_roll.Visible = false;
                rbl_rollno.Visible = false;
                lblNameSrc.Visible = false;
                txt_name.Visible = false;
                int indexvalue = 1;
                Hashtable check = (Hashtable)ViewState["colcountnew"];

            }
            else
            {
                lnkview.Visible = true;
                lnkview.Text = "View Details";
                loadsetting();
                txt_roll.Visible = true;
                rbl_rollno.Visible = true;
                lblNameSrc.Visible = true;
                txt_name.Visible = true;
                txt_roll.Text = "";
                txt_name.Text = "";

                int indexvalue = 1;
                Hashtable check = (Hashtable)ViewState["colcountnew"];
            }
        }
        catch { }
    }
    protected void btnok1_Click(object sender, EventArgs e)
    {
        if (dirAccess.selectScalarInt("select LinkValue from New_InsSettings where LinkName='StudentsDisplayPositioninJournal' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ") == 0)
        {
            divview.Visible = false;
        }
        try
        {
            List<string> appnoList = new List<string>();
            FpSpreadstud.SaveChanges();
            for (int sprStu = 1; sprStu < FpSpreadstud.Sheets[0].Rows.Count; sprStu++)
            {
                byte value = Convert.ToByte(FpSpreadstud.Sheets[0].Cells[sprStu, 1].Value);
                if (value == 1)
                {
                    appnoList.Add(Convert.ToString(FpSpreadstud.Sheets[0].Cells[sprStu, 0].Tag));
                }
            }
            if (appnoList.Count > 0)
            {
                ViewState["appNoList"] = appnoList;
                appnoList = (List<string>)ViewState["appNoList"];
                loadStudentDetails();
            }
            else
            {
                if (ViewState["appNoList"] != null)
                    ViewState.Remove("appNoList");
            }
        }
        catch { }
    }
    public void loadBaseGrid(DataSet dnewset)
    {
        try
        {
            #region DataTable for Grid
            DataTable dtGrid = new DataTable();

            ArrayList arrColHdrNames = new ArrayList();
            ArrayList arrColHdrNames2 = new ArrayList();
            arrColHdrNames.Add("S.No");
            arrColHdrNames.Add("Fee Type");
            arrColHdrNames2.Add("S.No");
            arrColHdrNames2.Add("Fee Type");
            dtGrid.Columns.Add("col0");
            dtGrid.Columns.Add("col1");
            int colHdrIndx = 2;
            for (int semLs = 0; semLs < cbl_sem.Items.Count; semLs++)
            {
                if (cbl_sem.Items[semLs].Selected)
                {
                    for (int colOrd = 0; colOrd < cblcolumnorder.Items.Count; colOrd++)
                    {
                        if (cblcolumnorder.Items[colOrd].Selected)
                        {
                            arrColHdrNames.Add(cbl_sem.Items[semLs].Value + "$" + cbl_sem.Items[semLs].Text);
                            arrColHdrNames2.Add(cblcolumnorder.Items[colOrd].Text);
                            dtGrid.Columns.Add("col" + colHdrIndx);
                            colHdrIndx++;

                            if (cblcolumnorder.Items[colOrd].Text.Trim().ToLower() == "scholarship")
                            {
                                arrColHdrNames.Add(cbl_sem.Items[semLs].Value + "$" + cbl_sem.Items[semLs].Text);
                                arrColHdrNames2.Add("Scholarship Type");
                                dtGrid.Columns.Add("col" + colHdrIndx);
                                colHdrIndx++;
                            }
                        }
                    }
                }
            }
            DataRow drHdr1 = dtGrid.NewRow();
            DataRow drHdr2 = dtGrid.NewRow();
            for (int grCol = 0; grCol < dtGrid.Columns.Count; grCol++)
            {
                drHdr1["col" + grCol] = grCol > 1 ? arrColHdrNames[grCol].ToString().Split('$')[1] : arrColHdrNames[grCol];
                drHdr2["col" + grCol] = arrColHdrNames2[grCol];
                //if (arrColHdrNames2[grCol].ToString().ToUpper() == "MODE" || arrColHdrNames2[grCol].ToString().ToUpper() == "DEDUCTION REASON" || arrColHdrNames2[grCol].ToString().ToUpper() == "Scholar")
                //{
                //    BoundField bfield = new BoundField();
                //    bfield.HeaderText = "col" + grCol;
                //    bfield.DataField = "col" + grCol;
                //    gridLedgeDetails.Columns.Add(bfield);
                //}
                //else
                //{
                //    TemplateField tfield = new TemplateField();
                //    tfield.HeaderText = "col" + grCol;
                //    gridLedgeDetails.Columns.Add(tfield);
                //}
            }
            //gridLedgeDetails.AutoGenerateColumns = false;
            dtGrid.Rows.Add(drHdr1);
            dtGrid.Rows.Add(drHdr2);
            Session["arrColHdrNames2"] = arrColHdrNames2;
            #endregion
            #region load Basic Grid
            if (dnewset.Tables.Count > 1 && dnewset.Tables[1].Rows.Count > 0)
            {
                int sno = 0;
                for (int ik = 0; ik < dnewset.Tables[1].Rows.Count; ik++)
                {
                    dnewset.Tables[0].DefaultView.RowFilter = "HeaderPK='" + Convert.ToString(dnewset.Tables[1].Rows[ik]["HeaderPK"]) + "'";
                    DataView dv = dnewset.Tables[0].DefaultView;
                    ///////
                    DataRow drHdr = dtGrid.NewRow();
                    drHdr[1] = Convert.ToString(dnewset.Tables[1].Rows[ik]["HeaderName"]);
                    dtGrid.Rows.Add(drHdr);
                    ///////
                    for (int i = 0; i < dv.Count; i++)
                    {
                        sno++;
                        ////////
                        DataRow drLdr = dtGrid.NewRow();
                        drLdr[0] = Convert.ToString(sno);
                        drLdr[1] = Convert.ToString(dv[i]["LedgerName"]) + "#" + Convert.ToString(dv[i]["LedgerPK"]);
                        dtGrid.Rows.Add(drLdr);
                        ////////
                    }
                }

                ///////
                DataRow drTot = dtGrid.NewRow();
                drTot[1] = "TOTAL";
                dtGrid.Rows.Add(drTot);
                Session["dtGrid"] = dtGrid;
                gridLedgeDetails.DataSource = dtGrid;
                gridLedgeDetails.DataBind();
                gridLedgeDetails.HeaderRow.Visible = false;
                ///////
            }
            #endregion
        }
        catch { }
    }
    protected void btn_go_click(object sender, EventArgs e)
    {
        try
        {
            #region Common Details

            lnkview.Text = "View Details";
            ledgertotal.Clear();
            headertotal.Clear();
            Grandtotal.Clear();
            hsgetpay.Clear();
            lblerr.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;

            ViewState["colcountnew"] = null;
            Session["appform_no"] = null;
            Session["app_no"] = null;
            Session["appformnew_no"] = null;

            string[] dtday = new string[31];
            for (int id = 1; id <= 31; id++)
            {
                if (Convert.ToString(id).Length == 1)
                {
                    dtday[id - 1] = Convert.ToString("0" + id);
                }
                else
                {
                    dtday[id - 1] = Convert.ToString(id);
                }
            }
            string[] dtmon = new string[12] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            string[] droparray = new string[2];
            string[] droparray1 = new string[ddl_detre.Items.Count];
            string[] droparray2 = new string[ddl_detre.Items.Count];
            droparray[0] = "Regular";
            droparray[1] = "Monthwise";
            string[] loadyear = new string[10];

            DateTime currdt = DateTime.Now;
            int curryear = currdt.Year;

            for (int ij = 0; ij < 10; ij++)
            {
                loadyear[ij] = Convert.ToString(curryear - ij);
            }

            if (ddl_detre.Items.Count > 0)
            {
                for (int re = 0; re < ddl_detre.Items.Count; re++)
                {
                    if (re == 0)
                    {
                        droparray1[re] = "Select";
                    }
                    else
                    {
                        droparray1[re] = Convert.ToString(ddl_detre.Items[re].Text);
                    }
                }
            }

            if (ddl_detre.Items.Count > 0)
            {
                for (int re = 0; re < ddl_detre.Items.Count; re++)
                {
                    if (re == 0)
                    {
                        droparray2[re] = " ";
                    }
                    else
                    {
                        droparray2[re] = Convert.ToString(ddl_detre.Items[re].Value);
                    }
                }
            }

            //if (ddlyear1.Items.Count > 0)
            //{
            //    for (int yr = 0; yr < ddlyear1.Items.Count; yr++)
            //    {
            //        loadyear[yr] = Convert.ToString(ddlyear1.Items[yr].Text);
            //    }
            //}


            string ledger = "";
            string header = "";
            string degreecode = "";
            string batchyear = "";
            string seattype = "";
            string feecategory = "";
            string type = "";
            string edulevel = "";
            string mode = "";
            string stutype = "";
            string course = "";
            string community = "";
            string religion = "";
            string reasoncode = "";
            int col = 1;
            int paycol = 0;

            FarPoint.Web.Spread.ComboBoxCellType cb = new FarPoint.Web.Spread.ComboBoxCellType(droparray);
            cb.UseValue = true;
            cb.AutoPostBack = true;
            cb.ShowButton = true;

            FarPoint.Web.Spread.ComboBoxCellType cb1 = new FarPoint.Web.Spread.ComboBoxCellType(droparray1, droparray2);
            //string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + collegecode1 + "'";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(sql, "Text");
            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        cb1.DataSource = ds;
            //        cb1.DataTextField = "TextVal";
            //        cb1.DataValueField = "TextCode";
            //    }
            //}
            cb1.UseValue = true;
            cb1.ShowButton = true;
            cb1.AutoPostBack = true;

            FarPoint.Web.Spread.ComboBoxCellType cbcomday = new FarPoint.Web.Spread.ComboBoxCellType(dtday);
            cbcomday.UseValue = true;
            cbcomday.ShowButton = true;
            cbcomday.AutoPostBack = true;

            FarPoint.Web.Spread.ComboBoxCellType cbcommon = new FarPoint.Web.Spread.ComboBoxCellType(dtmon);
            cbcommon.UseValue = true;
            cbcommon.ShowButton = true;
            cbcommon.AutoPostBack = true;

            FarPoint.Web.Spread.ComboBoxCellType cbcomyear = new FarPoint.Web.Spread.ComboBoxCellType(loadyear);
            cbcomyear.UseValue = true;
            cbcomyear.ShowButton = true;
            cbcomyear.AutoPostBack = true;

            FarPoint.Web.Spread.ComboBoxCellType cbday1 = new FarPoint.Web.Spread.ComboBoxCellType(dtday);
            cbday1.UseValue = true;
            cbday1.ShowButton = true;

            FarPoint.Web.Spread.ComboBoxCellType cbmon1 = new FarPoint.Web.Spread.ComboBoxCellType(dtmon);
            cbmon1.UseValue = true;
            cbmon1.ShowButton = true;

            FarPoint.Web.Spread.ComboBoxCellType cbyear1 = new FarPoint.Web.Spread.ComboBoxCellType(loadyear);
            cbyear1.UseValue = true;
            cbyear1.ShowButton = true;

            FarPoint.Web.Spread.ComboBoxCellType cbday2 = new FarPoint.Web.Spread.ComboBoxCellType(dtday);
            cbday2.UseValue = true;
            cbday2.ShowButton = true;

            FarPoint.Web.Spread.ComboBoxCellType cbmon2 = new FarPoint.Web.Spread.ComboBoxCellType(dtmon);
            cbmon2.UseValue = true;
            cbmon2.ShowButton = true;

            FarPoint.Web.Spread.ComboBoxCellType cbyear2 = new FarPoint.Web.Spread.ComboBoxCellType(loadyear);
            cbyear2.UseValue = true;
            cbyear2.ShowButton = true;

            string[] schlType = { " ", "Type" };
            FarPoint.Web.Spread.ComboBoxCellType cbSchType = new FarPoint.Web.Spread.ComboBoxCellType(schlType);
            cbSchType.UseValue = true;
            cbSchType.ShowButton = true;
            cbSchType.AutoPostBack = true;

            FarPoint.Web.Spread.DoubleCellType doubl = new FarPoint.Web.Spread.DoubleCellType();
            doubl.ErrorMessage = "Allow Numerics";
            int i = 0;
            #region Added by Idhris -- 08-03-2016

            ledger = GetSelectedItemsValueAsString(cbl_ledger);

            header = GetSelectedItemsValueAsString(cbl_header);

            degreecode = GetSelectedItemsValueAsString(cbl_dept);

            course = GetSelectedItemsValueAsString(cbl_course);

            batchyear = GetSelectedItemsValueAsString(cbl_batch);

            seattype = GetSelectedItemsValueAsString(cbl_seat);

            feecategory = GetSelectedItemsValueAsString(cbl_sem);

            type = GetSelectedItemsValueAsString(cbl_stream);

            edulevel = GetSelectedItemsValueAsString(cbl_edulevel);

            mode = GetSelectedItemsValueAsString(cbl_type);

            stutype = GetSelectedItemsValueAsString(cbl_stutype);

            community = GetSelectedItemsValueAsString(cbl_community);

            religion = GetSelectedItemsValueAsString(cbl_religion);

            #endregion

            string strorderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = "";
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY r.Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = "ORDER BY r.Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY r.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Stud_Name";
                }
                else if (strorderby == "0,1")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Reg_No";
                }
                else if (strorderby == "0,1,2")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else
                {
                    strorderby = "";
                }
            }

            List<string> ls = new List<string>();

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.Black;
            style2.BackColor = System.Drawing.Color.AliceBlue;
            Hashtable hatnew = new Hashtable();

            DataView dv1 = new DataView();
            DataView dv = new DataView();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();


            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "S.No";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
            //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;

            //FpSpread1.Sheets[0].ColumnCount++;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Fee Type";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
            //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 150;
            int count = 0;
            int semcount = 0;
            for (i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected)
                {
                    count = 0;
                    semcount++;
                    #region old
                    //FpSpread1.Sheets[0].ColumnCount++;
                    //hatnew.Add(Convert.ToString(cbl_sem.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Mode";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //if (!ItemList.Contains("Mode"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Fee Amount";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    //if (!ItemList.Contains("Fee Amount"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Deduction";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    //columnind.Add(FpSpread1.Sheets[0].ColumnCount - 1);
                    //if (!ItemList.Contains("Deduction"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Deduction Reason";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //if (!ItemList.Contains("Deduction Reason"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //ledgertotal.Add(Convert.ToString(cbl_sem.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 3);
                    //ViewState["ledgertotal"] = ledgertotal;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    //if (!ItemList.Contains("Total"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Refund";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    //columnind.Add(FpSpread1.Sheets[0].ColumnCount - 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //if (!ItemList.Contains("Refund"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Scholarship Type";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    //columnind.Add(FpSpread1.Sheets[0].ColumnCount - 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Scholarship";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    //columnind.Add(FpSpread1.Sheets[0].ColumnCount - 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //if (!ItemList.Contains("Scholarship"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].Visible = false;
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Pay Start Date";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //if (!hsgetpay.ContainsKey(Convert.ToString(cbl_sem.Items[i].Value)))
                    //{
                    //    hsgetpay.Add(Convert.ToString(cbl_sem.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                    //    ViewState["hsgetcol"] = hsgetpay;
                    //}
                    //if (!ItemList.Contains("Pay Start Date"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Date";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
                    //if (!ItemList.Contains("Pay Start Date"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Month";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
                    //if (!ItemList.Contains("Pay Start Date"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Year";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
                    //if (!ItemList.Contains("Pay Start Date"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 3, 1, 3);

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Fine";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //columnind.Add(FpSpread1.Sheets[0].ColumnCount - 1);
                    //if (!ItemList.Contains("Fine"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Due Date";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //if (!ItemList.Contains("Due Date"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Date";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
                    //if (!ItemList.Contains("Due Date"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Month";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;
                    //if (!ItemList.Contains("Due Date"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //count++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Year";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 50;

                    //if (!ItemList.Contains("Due Date"))
                    //{
                    //    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
                    //}
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 3, 1, 3);
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);

                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - count].Text = Convert.ToString(cbl_sem.Items[i].Text);
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - count].Tag = Convert.ToString(cbl_sem.Items[i].Value);
                    //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - count, 1, count);
                    #endregion
                }
            }
            if (semcount == 0)
            {
                //FpSpread1.Visible = false;
                //upd.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                if (lbl_sem.Text == "Semester")
                {
                    lblalerterr.Text = "Please select any one semester!";
                }
                if (lbl_sem.Text == "Year")
                {
                    lblalerterr.Text = "Please select any one year!";
                }
            }
            else
            {
                //upd.Visible = true;
                //FpSpread1.Visible = true;
            }
            ViewState["colcountnew"] = hatnew;
            ds.Clear();
            string selquery = "";

            string finyearid = d2.getCurrentFinanceYear(usercode, collegecode1);

            #endregion

            loadcolumns();
            if (ViewState["RetriveTable"] != null)
                ViewState.Remove("RetriveTable");

            #region General

            if (ddl_type.SelectedItem.Text == "General")
            {
                selquery = "select headerpk,headername,ledgerpk,ledgername from FM_HeaderMaster m,FM_LedgerMaster l where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure') and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "')";
                selquery = selquery + " select distinct headerpk,headername from FM_HeaderMaster m,FM_LedgerMaster l where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure') and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "')";
                selquery = selquery + "select FeeCategory,FeeAmount,LedgerFK,HeaderFK,DeductAmout,PayMode,DeductReason,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,convert(varchar(10),PayStartDate,103) as PayStartDate,convert(varchar(10),DueDate,103) as DueDate,FineAmount,'0' as FromGovtAmt from FT_FeeAllotDegree A where HeaderFK in('" + header + "') and LedgerFK in('" + ledger + "') and DegreeCode in ('" + degreecode + "') and batchyear in ('" + batchyear + "') and SeatType in ('" + seattype + "') and  FinYearFK ='" + finyearid + "' and FeeCategory in('" + feecategory + "')";

                DataSet dnewset = new DataSet();
                dnewset.Clear();
                dnewset = d2.select_method_wo_parameter(selquery, "Text");
                if (dnewset.Tables.Count > 1 && dnewset.Tables[1].Rows.Count > 0)
                {
                    studentdetail.Visible = false;
                    lblerr1.Visible = false;

                    loadBaseGrid(dnewset);
                    loadGeneralDetails(dnewset);
                }
            }

            #endregion

            #region Individual Admitted and Applied

            if (ddl_type.SelectedItem.Text == "Individual(Admitted)" || ddl_type.SelectedItem.Text == "Individual(Applied)")
            {
                FarPoint.Web.Spread.StyleInfo darknewstyle = new FarPoint.Web.Spread.StyleInfo();
                darknewstyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                darknewstyle.ForeColor = System.Drawing.Color.Black;
                darknewstyle.HorizontalAlign = HorizontalAlign.Center;
                FpSpreadstud.ActiveSheetView.ColumnHeader.DefaultStyle = darknewstyle;
                FpSpreadstud.Sheets[0].ColumnCount = 12;
                FpSpreadstud.Sheets[0].RowCount = 1;
                FpSpreadstud.Sheets[0].RowHeader.Visible = false;
                FpSpreadstud.CommandBar.Visible = false;
                FpSpreadstud.Sheets[0].AutoPostBack = false;

                FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                cball.AutoPostBack = true;

                selquery = "select headerpk,headername,ledgerpk,ledgername from FM_HeaderMaster m,FM_LedgerMaster l where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure') and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "') order by len(isnull(l.priority,1000)) , l.priority asc";
                selquery = selquery + " select distinct headerpk,headername from FM_HeaderMaster m,FM_LedgerMaster l where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure') and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "')";

                DataSet dnewset = new DataSet();
                dnewset.Clear();
                dnewset = d2.select_method_wo_parameter(selquery, "Text");
                if (dnewset.Tables.Count > 1 && dnewset.Tables[1].Rows.Count > 0)
                {
                    loadBaseGrid(dnewset);
                }

                if (dnewset.Tables.Count > 1 && dnewset.Tables[1].Rows.Count > 0)
                {
                    studentdetail.Visible = false;
                    lblerr1.Visible = false;

                    #region Students Section
                    if (txt_roll.Text.Trim() != "")
                    {
                        lnkview.Visible = false;
                        loadStudentDetails();
                    }
                    else
                    {
                        #region Load Students Spread
                        lnkview.Visible = true;
                        if (ddl_type.SelectedItem.Text == "Individual(Admitted)")
                        {
                            selquery = "SELECT r.App_NO,Roll_No,r.roll_admit,R.Stud_Name,(C.Course_Name+'-'+D.Dept_Name) as Department,Reg_no, A.parent_name, R.Current_Semester,(select TextVal from TextValTable where TextCode=A.seattype and TextCriteria='seat' ) as StType, R.Stud_Type,r.CC,r.DelFlag  FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE A.app_no = r.App_No and R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code ";

                            if (type.Trim() != "" && txt_stream.Enabled)
                            {
                                selquery = selquery + " and type in('" + type + "')";
                            }

                            if (edulevel.Trim() != "")
                            {
                                selquery = selquery + " and Edu_Level in('" + edulevel + "')";
                            }

                            if (mode.Trim() != "")
                            {
                                selquery = selquery + " and R.mode in('" + mode + "')";
                            }

                            if (stutype.Trim() != "")
                            {
                                selquery = selquery + " and r.Stud_Type in('" + stutype + "')";
                            }

                            if (seattype.Trim() != "")
                            {
                                selquery = selquery + " and a.seattype in('" + seattype + "')";
                            }

                            if (batchyear.Trim() != "")
                            {
                                selquery = selquery + " and r.batch_year in('" + batchyear + "')";
                            }

                            if (course.Trim() != "")
                            {
                                selquery = selquery + " and g.Course_Id in('" + course + "')";
                            }

                            if (degreecode.Trim() != "")
                            {
                                selquery = selquery + " and g.Degree_Code in('" + degreecode + "')";
                            }

                            selquery = selquery + " and isconfirm = 1";
                            if (ddlsearch.SelectedIndex == 1)
                            {
                                selquery = selquery + " and A.first_graduate=1 ";
                            }
                            if (ddlsearch.SelectedIndex == 2)
                            {
                                selquery = selquery + " and A.tutionfee_waiver=1";
                            }
                            if (ddlsearch.SelectedIndex == 3)
                            {
                                selquery = selquery + " and isnull(r.Post_Matric_Scholarship,'0')='1'";
                            }

                            selquery = selquery + " and admission_status = 1and Exam_Flag <>'DEBAR' ";


                            if (dirAccess.selectScalarInt("select LinkValue from New_InsSettings where LinkName='IncludeDiscontinuedInJournal' and user_code ='" + usercode + "'  ") == 0)//and college_code ='" + collegecode1 + "'
                            {
                                selquery += "  and DelFlag =0 ";
                            }

                            if (dirAccess.selectScalarInt("select LinkValue from New_InsSettings where LinkName='IncludeCompletedInJournal' and user_code ='" + usercode + "'  ") == 0)//and college_code ='" + collegecode1 + "'
                            {
                                selquery += "  and CC=0 ";
                            }

                            if (community.Trim() != "")
                            {
                                selquery = selquery + "  and A.community in('" + community + "')";
                            }
                            if (religion.Trim() != "")
                            {
                                selquery = selquery + "  and A.religion in('" + religion + "')";
                            }
                            if (getDisable() != string.Empty)
                                selquery += getDisable();
                            selquery += strorderby;
                        }
                        else if (ddl_type.SelectedItem.Text == "Individual(Applied)")
                        {
                            byte studAppSHrtAdm = StudentAppliedShorlistAdmit();
                            string admStudFilter = "";
                            switch (studAppSHrtAdm)
                            {
                                case 0:
                                    admStudFilter = " and a.isconfirm=1  and isnull(a.selection_status,'0')='0' and isnull(admission_status,'0')='0'  and a.app_no not in (select app_no from registration where Degree_Code in('" + degreecode + "')  and batch_year in('" + batchyear + "'))";
                                    break;
                                case 1:
                                    admStudFilter = " and a.isconfirm=1 and isnull(a.selection_status,'0')='1' and isnull(admission_status,'0')='0'  and a.app_no not in (select app_no from registration where Degree_Code in('" + degreecode + "')  and batch_year in('" + batchyear + "'))";
                                    break;
                                case 2:
                                    admStudFilter = " and a.isconfirm=1 and isnull(a.selection_status,'0')='1' and isnull(admission_status,'0')='1' and a.app_no not in (select app_no from registration where Degree_Code in('" + degreecode + "')  and batch_year in('" + batchyear + "'))";
                                    break;
                            }
                            selquery = "SELECT a.App_NO,a.app_formno,a.Stud_Name,'' Roll_No,(C.Course_Name+'-'+D.Dept_Name) as Department,A.parent_name,a.current_semester,(select TextVal from TextValTable where TextCode=A.seattype and TextCriteria='seat' ) as StType, a.Stud_Type,'' CC,'' DelFlag FROM Applyn A,Degree G,Course C,Department D WHERE A.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code " + admStudFilter + "";
                            if (txt_stream.Enabled && type.Trim() != "")
                            {
                                selquery += "  and type in('" + type + "') ";
                            }

                            if (edulevel.Trim() != "")
                            {
                                selquery = selquery + " and Edu_Level in('" + edulevel + "')";
                            }

                            if (mode.Trim() != "")
                            {
                                selquery = selquery + " and a.mode in('" + mode + "')";
                            }

                            if (stutype.Trim() != "")
                            {
                                selquery = selquery + " and a.Stud_Type in('" + stutype + "')";
                            }

                            if (seattype.Trim() != "")
                            {
                                selquery = selquery + " and a.seattype in('" + seattype + "')";
                            }

                            if (batchyear.Trim() != "")
                            {
                                selquery = selquery + " and a.batch_year in('" + batchyear + "')";
                            }

                            if (course.Trim() != "")
                            {
                                selquery = selquery + " and g.Course_Id in('" + course + "')";
                            }

                            if (degreecode.Trim() != "")
                            {
                                selquery = selquery + " and g.Degree_Code in('" + degreecode + "')";
                            }

                            selquery = selquery + " and isconfirm = 1";
                            if (ddlsearch.SelectedIndex == 1)
                            {
                                selquery = selquery + " and A.first_graduate=1 ";
                            }
                            if (ddlsearch.SelectedIndex == 2)
                            {
                                selquery = selquery + " and A.tutionfee_waiver=1";
                            }
                            //if (ddlsearch.SelectedIndex == 3)
                            //{
                            //    selquery = selquery + " and R.isnull(Post_Matric_Scholarship,'0')='1'";
                            //}
                            if (community.Trim() != "")
                            {
                                selquery = selquery + "  and A.community in('" + community + "')";
                            }
                            if (religion.Trim() != "")
                            {
                                selquery = selquery + "  and A.religion in('" + religion + "')";
                            }
                            if (getDisable() != string.Empty)
                                selquery += getDisable();
                            selquery += " ORDER BY a.app_formno";
                        }

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selquery, "Text");
                        double divheight = 0;
                        if (ds.Tables.Count > 0)
                        {
                            #region old

                            //if (ds.Tables[0].Rows.Count > 0)
                            //{
                            //    FarPoint.Web.Spread.StyleInfo style5 = new FarPoint.Web.Spread.StyleInfo();
                            //    style5.Font.Size = 13;
                            //    style5.Font.Name = "Book Antiqua";
                            //    style5.Font.Bold = true;
                            //    style5.HorizontalAlign = HorizontalAlign.Center;
                            //    style5.ForeColor = System.Drawing.Color.Black;
                            //    style5.BackColor = System.Drawing.Color.AliceBlue;

                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Columns[0].Width = 50;
                            //    FpSpreadstud.Columns[0].Locked = true;

                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";

                            //    FpSpreadstud.Sheets[0].Cells[0, 1].CellType = cball;
                            //    FpSpreadstud.Sheets[0].Cells[0, 1].Value = 0;
                            //    FpSpreadstud.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Columns[1].Width = 50;

                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll Number";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Columns[2].Width = 100;
                            //    FpSpreadstud.Columns[2].Locked = true;

                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg Number";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Columns[3].Width = 100;
                            //    FpSpreadstud.Columns[3].Locked = true;

                            //    if (rbl_rollno.SelectedItem.Text == "Roll No" && rbl_rollno.SelectedIndex == 0)
                            //    {
                            //        FpSpreadstud.Columns[2].Visible = true;
                            //        FpSpreadstud.Columns[3].Visible = false;
                            //    }
                            //    if (rbl_rollno.SelectedItem.Text == "Reg No" && rbl_rollno.SelectedIndex == 1)
                            //    {
                            //        FpSpreadstud.Columns[2].Visible = false;
                            //        FpSpreadstud.Columns[3].Visible = true;
                            //    }

                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Columns[4].Width = 150;
                            //    FpSpreadstud.Columns[4].Locked = true;

                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Department";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Columns[5].Width = 200;
                            //    FpSpreadstud.Columns[5].Locked = true;

                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Semester";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Columns[6].Width = 80;
                            //    FpSpreadstud.Columns[6].Locked = true;

                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Year";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Columns[7].Width = 50;
                            //    FpSpreadstud.Columns[7].Locked = true;

                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 8].Text = "FatherName";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Columns[8].Width = 200;
                            //    FpSpreadstud.Columns[8].Locked = true;

                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 9].Text = "SeatType";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Columns[9].Width = 80;
                            //    FpSpreadstud.Columns[9].Locked = true;

                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Belongs";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                            //    FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpreadstud.Columns[10].Width = 100;
                            //    FpSpreadstud.Columns[10].Locked = true;


                            //    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                            //    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            //    {
                            //        FpSpreadstud.Sheets[0].RowCount++;
                            //        divheight += 7;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_NO"]);
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Small;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            //        FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                            //        check.AutoPostBack = false;

                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].CellType = check;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Small;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].CellType = txt;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_NO"]);
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Small;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_no"]);
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Small;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Small;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Small;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]);
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Small;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 7].Text = reuse.returnYearforSem(Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]));
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Small;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["parent_name"]);
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Small;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[row]["StType"]);
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Small;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[row]["stud_type"]);
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Small;
                            //        FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                            //    }

                            //    lblerr1.Visible = false;
                            //    Div3.Visible = true;
                            //    btnok1.Visible = true;
                            //    FpSpreadstud.Visible = true;
                            //    FpSpreadstud.Sheets[0].PageSize = FpSpreadstud.Sheets[0].RowCount;
                            //    FpSpreadstud.Width = 800;
                            //    FpSpreadstud.Height = 400;
                            //    FpSpreadstud.Sheets[0].SpanModel.Add(0, 2, 1, 4);
                            //    FpSpreadstud.Sheets[0].FrozenRowCount = 1;
                            //    studentdetail.Visible = true;
                            //}
                            #endregion

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                RollAndRegSettings();
                                FarPoint.Web.Spread.StyleInfo style5 = new FarPoint.Web.Spread.StyleInfo();
                                style5.Font.Size = 13;
                                style5.Font.Name = "Book Antiqua";
                                style5.Font.Bold = true;
                                style5.HorizontalAlign = HorizontalAlign.Center;
                                style5.ForeColor = System.Drawing.Color.Black;
                                style5.BackColor = System.Drawing.Color.AliceBlue;

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[0].Width = 50;
                                FpSpreadstud.Columns[0].Locked = true;

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";

                                FpSpreadstud.Sheets[0].Cells[0, 1].CellType = cball;
                                FpSpreadstud.Sheets[0].Cells[0, 1].Value = 0;
                                FpSpreadstud.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[1].Width = 50;

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll Number";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[2].Width = 100;
                                FpSpreadstud.Columns[2].Locked = true;

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg Number";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[3].Width = 100;
                                FpSpreadstud.Columns[3].Locked = true;

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admission No";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[4].Width = 100;
                                FpSpreadstud.Columns[4].Locked = true;

                                //if (rbl_rollno.SelectedItem.Text == "Roll No" && rbl_rollno.SelectedIndex == 0)
                                //{
                                //    FpSpreadstud.Columns[2].Visible = true;
                                //    FpSpreadstud.Columns[3].Visible = false;
                                //}
                                //if (rbl_rollno.SelectedItem.Text == "Reg No" && rbl_rollno.SelectedIndex == 1)
                                //{
                                //    FpSpreadstud.Columns[2].Visible = false;
                                //    FpSpreadstud.Columns[3].Visible = true;
                                //}

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Name";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[5].Width = 150;
                                FpSpreadstud.Columns[5].Locked = true;

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Department";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[6].Width = 200;
                                FpSpreadstud.Columns[6].Locked = true;

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Semester";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[7].Width = 80;
                                FpSpreadstud.Columns[7].Locked = true;

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Year";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[8].Width = 50;
                                FpSpreadstud.Columns[8].Locked = true;

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 9].Text = "FatherName";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[9].Width = 200;
                                FpSpreadstud.Columns[9].Locked = true;

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 10].Text = "SeatType";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[10].Width = 80;
                                FpSpreadstud.Columns[10].Locked = true;

                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Belongs";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                                FpSpreadstud.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                                FpSpreadstud.Columns[11].Width = 100;
                                FpSpreadstud.Columns[11].Locked = true;

                                spreadColumnVisible();
                                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    FpSpreadstud.Sheets[0].RowCount++;
                                    divheight += 7;

                                    string ccValue = Convert.ToString(ds.Tables[0].Rows[row]["CC"]).Trim();//Course Completed
                                    string delflagValue = Convert.ToString(ds.Tables[0].Rows[row]["DelFlag"]).Trim();//discontinue

                                    if (delflagValue == "1" || delflagValue.ToLower() == "true")
                                    {
                                        FpSpreadstud.Sheets[0].Rows[FpSpreadstud.Sheets[0].RowCount - 1].BackColor = Color.Pink;
                                    }
                                    else if (ccValue == "1" || ccValue.ToLower() == "true")
                                    {
                                        FpSpreadstud.Sheets[0].Rows[FpSpreadstud.Sheets[0].RowCount - 1].BackColor = Color.Wheat;
                                    }
                                    else
                                    {
                                        FpSpreadstud.Sheets[0].Rows[FpSpreadstud.Sheets[0].RowCount - 1].BackColor = Color.WhiteSmoke;
                                    }
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_NO"]);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                                    check.AutoPostBack = false;

                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].CellType = check;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].CellType = txt;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_NO"]);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_no"]);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].CellType = txt;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 4].CellType = txt;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";



                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 8].Text = reuse.returnYearforSem(Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]));
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[row]["parent_name"]);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[row]["StType"]);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(ds.Tables[0].Rows[row]["stud_type"]);
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Small;
                                    FpSpreadstud.Sheets[0].Cells[FpSpreadstud.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";

                                }
                                // FpSpread1.Visible = false;
                                //popper1.Visible = false;
                                //div2.Visible = true;
                                lblerr1.Visible = false;
                                Div3.Visible = true;
                                btnok1.Visible = true;
                                FpSpreadstud.Visible = true;
                                FpSpreadstud.Sheets[0].PageSize = FpSpreadstud.Sheets[0].RowCount;
                                FpSpreadstud.Width = 800;
                                FpSpreadstud.Height = 400;
                                FpSpreadstud.Sheets[0].SpanModel.Add(0, 2, 1, 4);
                                FpSpreadstud.Sheets[0].FrozenRowCount = 1;
                                FpSpreadstud.SaveChanges();
                                studentdetail.Visible = true;
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    FpSpreadstud.Visible = false;
                    lblerr1.Visible = true;
                    lblerr1.Text = "There are no students available!";
                    Div3.Visible = false;
                    btnok1.Visible = false;
                    studentdetail.Visible = true;
                }

            }
            #endregion

            lnk_columnorder.Visible = false;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode1, "journal.aspx");
        }
    }
    public void loadGeneralDetails(DataSet dnewset)
    {
        if (ViewState["RetriveTable"] != null)
            ViewState.Remove("RetriveTable");

        if (dnewset.Tables.Count > 2)
        {
            ViewState["RetriveTable"] = dnewset.Tables[2];
            loadBaseGrid(dnewset);
        }
    }
    public void loadStudentDetails()
    {
        if (ViewState["RetriveTable"] != null)
            ViewState.Remove("RetriveTable");
        string app_no = string.Empty;
        #region Student To Load
        bool studOk = false;
        if (txt_roll.Text.Trim() != "")
        {
            lnkview.Visible = false;

            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
            {
                app_no = d2.GetFunction(" select App_No from Registration where Roll_No='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
            }
            else
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    app_no = d2.GetFunction(" select App_No from Registration where reg_no='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
                else
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        app_no = d2.GetFunction(" select App_No from Registration where Roll_admit='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                    }
                    else
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                        {
                            app_no = d2.GetFunction(" select app_no from applyn where app_formno='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                        }
            studOk = true;
        }
        else
        {
            lnkview.Visible = true;
            List<string> appnoList = new List<string>();
            if (ViewState["appNoList"] != null)
            {
                appnoList = (List<string>)ViewState["appNoList"];
                app_no = appnoList[0];
            }

            if (appnoList.Count > 0 || txt_roll.Text.Trim() != "")
            {
                studOk = true;
            }
        }
        #endregion

        if (studOk && app_no != string.Empty)
        {
            string feecategory = GetSelectedItemsValueAsString(cbl_sem);

            string ledger = GetSelectedItemsValueAsString(cbl_ledger);

            string header = GetSelectedItemsValueAsString(cbl_header);

            string selquery = "";

            string rghtval = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Show All Ledger' and user_code='" + usercode + "'");
            if (rghtval == "0")
            {
                selquery = " select distinct headerpk,headername,ledgerpk,ledgername from FM_HeaderMaster m,FM_LedgerMaster l,FT_FeeAllot F where m.HeaderPK = l.HeaderFK and f.LedgerFK =l.LedgerPK and l.HeaderFK =f.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure')  and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "') and f.App_No ='" + app_no + "' and f.FeeCategory in('" + feecategory + "') ";

                selquery = selquery + " select distinct m.headerpk,m.headername from FM_HeaderMaster m,FT_FeeAllot F where f.HeaderFK=m.headerPK and HeaderpK in('" + header + "') and LedgerfK in('" + ledger + "') and f.App_No ='" + app_no + "' and f.FeeCategory in('" + feecategory + "') ";

                selquery = selquery + " select HeaderPK,HeaderName,LedgerPK,LedgerName,LedgerFK,f.HeaderFK,AllotDate,FeeCategory,PayMode,FeeAmount,FeeAmountMonthly,DeductAmout,DeductReason,TotalAmount,RefundAmount,FromGovtAmt,convert(varchar(10),DueDate,103) as DueDate,FineAmount,convert(varchar(10),PayStartDate,103) as  PayStartDate,f.FeeAllotPK from FM_HeaderMaster m,FM_LedgerMaster l,FT_FeeAllot f where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure') and m.HeaderPK=f.HeaderFK and l.LedgerPK=f.LedgerFK and App_No ='" + app_no + "' and f.FeeCategory in('" + feecategory + "') and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "') ";

            }
            else
            {
                selquery = " select distinct headerpk,headername,ledgerpk,ledgername from FM_HeaderMaster m,FM_LedgerMaster l where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure')  and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "') ";

                selquery = selquery + " select distinct headerpk,headername from FM_HeaderMaster m,FM_LedgerMaster l where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure')  and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "') ";

                selquery = selquery + " select HeaderPK,HeaderName,LedgerPK,LedgerName,LedgerFK,f.HeaderFK,AllotDate,FeeCategory,PayMode,FeeAmount,FeeAmountMonthly,DeductAmout,DeductReason,TotalAmount,RefundAmount,FromGovtAmt,convert(varchar(10),DueDate,103) as DueDate,FineAmount,convert(varchar(10),PayStartDate,103) as  PayStartDate,f.FeeAllotPK from FM_HeaderMaster m,FM_LedgerMaster l,FT_FeeAllot f where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure') and m.HeaderPK=f.HeaderFK and l.LedgerPK=f.LedgerFK and App_No in('" + app_no + "') and f.FeeCategory in('" + feecategory + "') and HeaderpK in('" + header + "') and LedgerpK in('" + ledger + "') ";

            }
            DataSet dnewset = d2.select_method_wo_parameter(selquery, "Text");
            if (dnewset.Tables.Count > 2)
            {
                ViewState["RetriveTable"] = dnewset.Tables[2];
                loadBaseGrid(dnewset);
            }
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Please Select A Student";
        }
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                e.Row.BackColor = Color.FromArgb(12, 166, 202);
                e.Row.Cells[0].RowSpan = 2;
                e.Row.Cells[1].RowSpan = 2;

                for (int cCnt = 2; cCnt < e.Row.Cells.Count; )
                {
                    if (cCnt < e.Row.Cells.Count - 1 && e.Row.Cells[cCnt].Text.Trim() == e.Row.Cells[cCnt + 1].Text.Trim())
                    {
                        e.Row.Cells[cCnt].ColumnSpan += 1;
                        e.Row.Cells.RemoveAt(cCnt + 1);
                    }
                    else
                    {
                        e.Row.Cells[cCnt].ColumnSpan += 1;
                        cCnt++;
                    }
                }
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.Font.Bold = true;
            }
            else if (e.Row.RowIndex == 1)
            {
                e.Row.BackColor = Color.FromArgb(12, 166, 202);
                e.Row.Cells.RemoveAt(0);
                e.Row.Cells.RemoveAt(0);
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.Font.Bold = true;
            }
            else
            {
                string curText = e.Row.Cells[0].Text.Trim().Replace("&nbsp;", "");
                bool breakthis = false;
                if (curText == string.Empty)
                {
                    breakthis = true;
                    e.Row.ForeColor = Color.White;
                    e.Row.BackColor = Color.FromArgb(153, 0, 153);
                    e.Row.Font.Bold = true;
                }
                else
                {
                    e.Row.Cells[0].BackColor = Color.FromArgb(12, 202, 166);
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].BackColor = Color.FromArgb(12, 202, 166);
                }
                ArrayList arrColHdrNames2 = (ArrayList)Session["arrColHdrNames2"];
                if (!breakthis)
                {
                    string[] ldgrNameVal = e.Row.Cells[1].Text.Trim().Split('#');
                    Label lbl_lgrId = new Label();
                    lbl_lgrId.Text = ldgrNameVal[1];
                    lbl_lgrId.Visible = false;
                    lbl_lgrId.ID = "lbl_lgrId";

                    e.Row.Cells[1].Controls.Add(lbl_lgrId);

                    Label lbl_lgrNm = new Label();
                    lbl_lgrNm.Width = 250;
                    lbl_lgrNm.Style.Add("font-weight", "bold");
                    lbl_lgrNm.Text = ldgrNameVal[0];
                    lbl_lgrNm.ID = "lbl_lgrNm";
                    e.Row.Cells[1].Controls.Add(lbl_lgrNm);


                    for (int colInd = 2; colInd < e.Row.Cells.Count; colInd++)
                    {
                        string hdrName = arrColHdrNames2[colInd].ToString().ToLower();

                        if (hdrName == "mode")
                        {
                            DropDownList ddlMode = new DropDownList();
                            ddlMode.Width = 90;
                            ddlMode.Attributes.Add("style", "background-color:#ff6600;");
                            ddlMode.Items.Add("Regular");
                            ddlMode.Items.Add("Monthwise");
                            ddlMode.SelectedIndexChanged += new EventHandler(ddlMode_Indexchanged);
                            ddlMode.AutoPostBack = true;
                            ddlMode.ID = "ddlMode_" + colInd;

                            TextBox lblMode = new TextBox();
                            // lblMode.Visible = false;
                            lblMode.ID = "lblMode_" + colInd;
                            lblMode.Attributes.Add("style", "display:none;");

                            e.Row.Cells[colInd].Controls.Add(lblMode);
                            e.Row.Cells[colInd].Controls.Add(ddlMode);

                        }
                        else if (hdrName == "deduction reason")
                        {
                            DropDownList ddlDedReas = new DropDownList();
                            ddlDedReas.Attributes.Add("style", "background-color:#b3b300;");
                            ddlDedReas.Width = 140;
                            ddlDedReas.Items.Add("Regular");
                            ddlDedReas.Items.Add("Monthwise");
                            ddlDedReas.ID = "ddlDedReas_" + colInd;
                            e.Row.Cells[colInd].Controls.Add(ddlDedReas);
                            bindDedReason(ddlDedReas);
                        }
                        else if (hdrName == "scholarship type")
                        {
                            DropDownList ddlSchl = new DropDownList();
                            ddlSchl.Attributes.Add("style", "background-color:#3399ff;");
                            ddlSchl.Width = 120;
                            ddlSchl.Items.Add(" ");
                            ddlSchl.Items.Add("Type");
                            ddlSchl.AutoPostBack = true;
                            ddlSchl.ID = "ddlSchl_" + colInd;
                            ddlSchl.SelectedIndexChanged += new EventHandler(ddlSchl_IndexChanged);
                            e.Row.Cells[colInd].Controls.Add(ddlSchl);
                        }
                        else if (hdrName == "fee amount")
                        {
                            TextBox txtFeeamt = new TextBox();
                            txtFeeamt.Width = 90;
                            txtFeeamt.Attributes.Add("style", "text-align:right;background-color:#2eb82e;");
                            txtFeeamt.ID = "txtFeeamt_" + colInd;
                            txtFeeamt.Text = "0.00";


                            AjaxControlToolkit.FilteredTextBoxExtender afte = new AjaxControlToolkit.FilteredTextBoxExtender();
                            afte.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                            afte.ValidChars = ".0123456789";
                            afte.TargetControlID = txtFeeamt.ID;

                            e.Row.Cells[colInd].Controls.Add(afte);
                            e.Row.Cells[colInd].Controls.Add(txtFeeamt);
                        }
                        else if (hdrName == "deduction")
                        {
                            TextBox txtDed = new TextBox();
                            txtDed.Width = 80;
                            txtDed.Attributes.Add("style", "text-align:right;background-color:#b3b300;");
                            txtDed.ID = "txtDed_" + colInd;
                            txtDed.Text = "0.00";


                            AjaxControlToolkit.FilteredTextBoxExtender afte = new AjaxControlToolkit.FilteredTextBoxExtender();
                            afte.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                            afte.ValidChars = ".0123456789";
                            afte.TargetControlID = txtDed.ID;

                            e.Row.Cells[colInd].Controls.Add(afte);
                            e.Row.Cells[colInd].Controls.Add(txtDed);
                        }
                        else if (hdrName == "total")
                        {
                            TextBox txttotal = new TextBox();
                            txttotal.Width = 80;
                            txttotal.Attributes.Add("style", "text-align:right;background-color:#2eb82e;");
                            txttotal.Attributes.Add("readonly", "readonly");
                            txttotal.ID = "txttotal_" + colInd;
                            txttotal.Text = "0.00";
                            txttotal.Attributes.Add("onchange", "if(this.value=='') this.value='0.00';");

                            e.Row.Cells[colInd].Controls.Add(txttotal);
                        }
                        else if (hdrName == "refund")
                        {
                            TextBox txtrefund = new TextBox();
                            txtrefund.Width = 60;
                            txtrefund.Attributes.Add("style", "text-align:right;background-color:#ff6699;");
                            txtrefund.ID = "txtrefund_" + colInd;
                            txtrefund.Text = "0.00";
                            txtrefund.Attributes.Add("onchange", "if(this.value=='') this.value='0.00';");

                            AjaxControlToolkit.FilteredTextBoxExtender afte = new AjaxControlToolkit.FilteredTextBoxExtender();
                            afte.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                            afte.ValidChars = ".0123456789";
                            afte.TargetControlID = txtrefund.ID;

                            e.Row.Cells[colInd].Controls.Add(afte);
                            e.Row.Cells[colInd].Controls.Add(txtrefund);
                        }
                        else if (hdrName == "scholarship")
                        {
                            TextBox txtscholarship = new TextBox();
                            txtscholarship.Width = 80;
                            txtscholarship.Attributes.Add("style", "text-align:right;background-color:#3399ff;");
                            txtscholarship.ID = "txtscholarship_" + colInd;
                            txtscholarship.Text = "0.00";
                            txtscholarship.Attributes.Add("readonly", "readonly");
                            txtscholarship.Attributes.Add("onchange", "if(this.value=='') this.value='0.00';");

                            AjaxControlToolkit.FilteredTextBoxExtender afte = new AjaxControlToolkit.FilteredTextBoxExtender();
                            afte.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                            afte.ValidChars = ".0123456789";
                            afte.TargetControlID = txtscholarship.ID;

                            TextBox lblSchl = new TextBox();
                            lblSchl.Attributes.Add("style", "display:none;");
                            lblSchl.ID = "lblSchl_" + colInd;

                            e.Row.Cells[colInd].Controls.Add(lblSchl);
                            e.Row.Cells[colInd].Controls.Add(txtscholarship);
                            e.Row.Cells[colInd].Controls.Add(afte);
                        }
                        else if (hdrName == "pay start date")
                        {
                            TextBox txtpay = new TextBox();
                            txtpay.Width = 100;
                            txtpay.Attributes.Add("readonly", "readonly");
                            txtpay.Attributes.Add("style", "text-align:center;background-color:#ffb3ff;");
                            txtpay.ID = "txtpay_" + colInd;
                            txtpay.Text = DateTime.Now.ToString("dd/MM/yyyy");

                            AjaxControlToolkit.CalendarExtender ajCl = new AjaxControlToolkit.CalendarExtender();
                            ajCl.TargetControlID = txtpay.ID;
                            ajCl.Format = "dd/MM/yyyy";

                            e.Row.Cells[colInd].Controls.Add(txtpay);
                            e.Row.Cells[colInd].Controls.Add(ajCl);
                        }
                    }
                }
                else if (e.Row.Cells[1].Text.Trim() == "TOTAL")
                {
                    for (int colInd = 2; colInd < e.Row.Cells.Count; colInd++)
                    {
                        string hdrName = arrColHdrNames2[colInd].ToString().ToLower();
                        if (hdrName == "fee amount")
                        {
                            TextBox txtFeeamt = new TextBox();
                            txtFeeamt.Width = 90;
                            txtFeeamt.Attributes.Add("readonly", "readonly");
                            txtFeeamt.Attributes.Add("style", "text-align:right;background-color:#2eb82e;");
                            txtFeeamt.ID = "txtFeeamtT_" + colInd;
                            txtFeeamt.Text = "0.00";


                            AjaxControlToolkit.FilteredTextBoxExtender afte = new AjaxControlToolkit.FilteredTextBoxExtender();
                            afte.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                            afte.ValidChars = ".0123456789";
                            afte.TargetControlID = txtFeeamt.ID;

                            e.Row.Cells[colInd].Controls.Add(afte);
                            e.Row.Cells[colInd].Controls.Add(txtFeeamt);
                        }
                        else if (hdrName == "deduction")
                        {
                            TextBox txtDed = new TextBox();
                            txtDed.Width = 80;
                            txtDed.Attributes.Add("readonly", "readonly");
                            txtDed.Attributes.Add("style", "text-align:right;background-color:#b3b300;");
                            txtDed.ID = "txtDedT_" + colInd;
                            txtDed.Text = "0.00";


                            AjaxControlToolkit.FilteredTextBoxExtender afte = new AjaxControlToolkit.FilteredTextBoxExtender();
                            afte.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                            afte.ValidChars = ".0123456789";
                            afte.TargetControlID = txtDed.ID;

                            e.Row.Cells[colInd].Controls.Add(afte);
                            e.Row.Cells[colInd].Controls.Add(txtDed);
                        }
                        else if (hdrName == "total")
                        {
                            TextBox txttotal = new TextBox();
                            txttotal.Width = 80;
                            txttotal.Attributes.Add("style", "text-align:right;background-color:#2eb82e;");
                            txttotal.Attributes.Add("readonly", "readonly");
                            txttotal.ID = "txttotalT_" + colInd;
                            txttotal.Text = "0.00";
                            txttotal.Attributes.Add("onchange", "if(this.value=='') this.value='0.00';");

                            e.Row.Cells[colInd].Controls.Add(txttotal);
                        }
                        else if (hdrName == "refund")
                        {
                            TextBox txtrefund = new TextBox();
                            txtrefund.Width = 60;
                            txtrefund.Attributes.Add("readonly", "readonly");
                            txtrefund.Attributes.Add("style", "text-align:right;background-color:#ff6699;");
                            txtrefund.ID = "txtrefundT_" + colInd;
                            txtrefund.Text = "0.00";
                            txtrefund.Attributes.Add("onchange", "if(this.value=='') this.value='0.00';");

                            AjaxControlToolkit.FilteredTextBoxExtender afte = new AjaxControlToolkit.FilteredTextBoxExtender();
                            afte.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                            afte.ValidChars = ".0123456789";
                            afte.TargetControlID = txtrefund.ID;

                            e.Row.Cells[colInd].Controls.Add(afte);
                            e.Row.Cells[colInd].Controls.Add(txtrefund);
                        }
                        else if (hdrName == "scholarship")
                        {
                            TextBox txtscholarship = new TextBox();
                            txtscholarship.Width = 80;
                            txtscholarship.Attributes.Add("readonly", "readonly");
                            txtscholarship.Attributes.Add("style", "text-align:right;background-color:#3399ff;");
                            txtscholarship.ID = "txtscholarshipT_" + colInd;
                            txtscholarship.Text = "0.00";
                            txtscholarship.Attributes.Add("readonly", "readonly");
                            txtscholarship.Attributes.Add("onchange", "if(this.value=='') this.value='0.00';");

                            AjaxControlToolkit.FilteredTextBoxExtender afte = new AjaxControlToolkit.FilteredTextBoxExtender();
                            afte.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                            afte.ValidChars = ".0123456789";
                            afte.TargetControlID = txtscholarship.ID;

                            TextBox lblSchl = new TextBox();
                            lblSchl.Attributes.Add("style", "display:none;");
                            lblSchl.ID = "lblSchl_" + colInd;

                            e.Row.Cells[colInd].Controls.Add(lblSchl);
                            e.Row.Cells[colInd].Controls.Add(txtscholarship);
                            e.Row.Cells[colInd].Controls.Add(afte);
                        }
                    }
                }
                else
                {
                    for (int colInd = 2; colInd < e.Row.Cells.Count; colInd++)
                    {
                        string hdrName = arrColHdrNames2[colInd].ToString().ToLower();
                        if (hdrName == "pay start date")
                        {
                            TextBox txtpay = new TextBox();
                            txtpay.Width = 100;
                            txtpay.Attributes.Add("readonly", "readonly");
                            txtpay.Attributes.Add("style", "text-align:center;background-color:#ffb3ff;");
                            txtpay.ID = "txtpayH_" + colInd;
                            txtpay.Text = DateTime.Now.ToString("dd/MM/yyyy");

                            AjaxControlToolkit.CalendarExtender ajCl = new AjaxControlToolkit.CalendarExtender();
                            ajCl.TargetControlID = txtpay.ID;
                            ajCl.Format = "dd/MM/yyyy";

                            e.Row.Cells[colInd].Controls.Add(txtpay);
                            e.Row.Cells[colInd].Controls.Add(ajCl);
                        }
                    }
                    #region Column Span
                    //for (int cCnt = 1; cCnt < e.Row.Cells.Count; )
                    //{
                    //    if (cCnt < e.Row.Cells.Count - 1)
                    //    {
                    //        e.Row.Cells[cCnt].ColumnSpan += 1;
                    //        e.Row.Cells.RemoveAt(cCnt + 1);
                    //    }
                    //    else
                    //    {
                    //        e.Row.Cells[cCnt].ColumnSpan += 1;
                    //        cCnt++;
                    //    }
                    //}
                    #endregion
                }
            }
        }
    }
    protected void gridLedgeDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            DataTable prevTable = new DataTable();
            if (ViewState["RetriveTable"] != null)
            {
                prevTable = (DataTable)ViewState["RetriveTable"];
                //ViewState.Remove("RetriveTable");
            }

            #region for Scholarship
            string query = " select Distinct MasterValue,MasterCode from CO_MasterValues where MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode1 + "' order by MasterValue asc";
            DataSet dsSchl = d2.select_method_wo_parameter(query, "Text");

            string app_no = string.Empty;
            if (txt_roll.Text.Trim() != "")
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    app_no = d2.GetFunction(" select App_No from Registration where Roll_No='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
                else
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        app_no = d2.GetFunction(" select App_No from Registration where reg_no='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                    }
                    else
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        {
                            app_no = d2.GetFunction(" select App_No from Registration where Roll_admit='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                        }
                        else
                            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                            {
                                app_no = d2.GetFunction(" select app_no from applyn where app_formno='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                            }

            }
            #endregion

            ArrayList semValue = new ArrayList();
            int semCnt = 0;
            for (int chk = 0; chk < cbl_sem.Items.Count; chk++)
            {
                if (cbl_sem.Items[chk].Selected)
                {
                    semCnt++;
                    semValue.Add(cbl_sem.Items[chk].Value);
                }
            }
            ArrayList arrColOrder = new ArrayList();
            for (int chk = 0; chk < cblcolumnorder.Items.Count; chk++)
            {
                if (cblcolumnorder.Items[chk].Selected)
                {
                    arrColOrder.Add(cblcolumnorder.Items[chk].Text);
                    if (cblcolumnorder.Items[chk].Text == "Scholarship") arrColOrder.Add("Scholarship Type");
                }
            }

            int rowIndx = 0;
            Hashtable htTotal = new Hashtable();
            foreach (GridViewRow row in gridLedgeDetails.Rows)
            {
                int modeIndx = -1, feeamtIndx = -1, dedIndx = -1, dedReIndx = -1, totIndx = -1, refIndx = -1, schIndx = -1, schlTypeIndx = -1, payIndx = -1;
                #region colIndex for selected order
                if (arrColOrder.Contains("Mode"))
                {
                    modeIndx = arrColOrder.IndexOf("Mode");
                }
                if (arrColOrder.Contains("Fee Amount"))
                {
                    feeamtIndx = arrColOrder.IndexOf("Fee Amount");
                }
                if (arrColOrder.Contains("Deduction"))
                {
                    dedIndx = arrColOrder.IndexOf("Deduction");
                }
                if (arrColOrder.Contains("Deduction Reason"))
                {
                    dedReIndx = arrColOrder.IndexOf("Deduction Reason");
                }
                if (arrColOrder.Contains("Total"))
                {
                    totIndx = arrColOrder.IndexOf("Total");
                }
                if (arrColOrder.Contains("Refund"))
                {
                    refIndx = arrColOrder.IndexOf("Refund");
                }
                if (arrColOrder.Contains("Pay Start Date"))
                {
                    payIndx = arrColOrder.IndexOf("Pay Start Date");
                }
                if (arrColOrder.Contains("Scholarship"))
                {
                    schIndx = arrColOrder.IndexOf("Scholarship");
                    schlTypeIndx = schIndx + 1;
                }
                #endregion
                for (int colindx = 2, semcnt = 1; semcnt <= semCnt; semcnt++, colindx += arrColOrder.Count)
                {
                    Label lbl_lgrId = (Label)row.FindControl("lbl_lgrId");
                    string ledgerFk = lbl_lgrId != null ? lbl_lgrId.Text : "0";
                    string feeCategory = semValue[(semcnt - 1)].ToString();

                    DropDownList ddlMode_ = (DropDownList)row.FindControl("ddlMode_" + (modeIndx + colindx));
                    TextBox lblMode = (TextBox)row.FindControl("lblMode_" + (modeIndx + colindx));
                    TextBox txtFeeamt_ = (TextBox)row.FindControl("txtFeeamt_" + (feeamtIndx + colindx));
                    TextBox txtDed_ = (TextBox)row.FindControl("txtDed_" + (dedIndx + colindx));
                    DropDownList ddlDedReas_ = (DropDownList)row.FindControl("ddlDedReas_" + (dedReIndx + colindx));
                    TextBox txttotal_ = (TextBox)row.FindControl("txttotal_" + (totIndx + colindx));
                    TextBox txtrefund_ = (TextBox)row.FindControl("txtrefund_" + (refIndx + colindx));
                    TextBox txtpay_ = (TextBox)row.FindControl("txtpay_" + (payIndx + colindx));
                    TextBox txtscholarship_ = (TextBox)row.FindControl("txtscholarship_" + (schIndx + colindx));
                    DropDownList ddlSchl_ = (DropDownList)row.FindControl("ddlSchl_" + (schlTypeIndx + colindx));
                    TextBox lblSchl_ = (TextBox)row.FindControl("lblSchl_" + (schIndx + colindx));

                    #region Paystartdate Fullchange
                    TextBox txtpayH_ = (TextBox)row.FindControl("txtpayH_" + (payIndx + colindx));

                    if (txtpayH_ != null)
                    {
                        StringBuilder sbPaDate = new StringBuilder();
                        bool okOne = true;
                        for (int gRow = rowIndx; gRow < gridLedgeDetails.Rows.Count; gRow++)
                        {
                            TextBox txtpayH = (TextBox)gridLedgeDetails.Rows[gRow].FindControl("txtpayH_" + (payIndx + colindx));
                            if (txtpayH == null)
                            {
                                TextBox txtpay = (TextBox)gridLedgeDetails.Rows[gRow].FindControl("txtpay_" + (payIndx + colindx));
                                if (txtpay != null)
                                {
                                    sbPaDate.Append("document.getElementById('MainContent_gridLedgeDetails_txtpay_" + (payIndx + colindx) + "_" + gRow + "').value=document.getElementById('MainContent_gridLedgeDetails_txtpayH_" + (payIndx + colindx) + "_" + rowIndx + "').value;");
                                }
                            }
                            else
                            {
                                if (!okOne)
                                {
                                    break;
                                }
                                okOne = false;
                            }
                        }
                        txtpayH_.Attributes.Add("onchange", sbPaDate.ToString());
                    }
                    #endregion

                    #region Total Change Events
                    StringBuilder sbRef = new StringBuilder();
                    StringBuilder sbFee = new StringBuilder();
                    StringBuilder sbTot = new StringBuilder();
                    StringBuilder sbDed = new StringBuilder();
                    for (int gRow = 2; gRow < gridLedgeDetails.Rows.Count - 1; gRow++)
                    {
                        #region Refund Change
                        TextBox txtrefund = (TextBox)gridLedgeDetails.Rows[gRow].FindControl("txtrefund_" + (refIndx + colindx));
                        if (txtrefund != null)
                        {
                            sbRef.Append("parseFloat(document.getElementById('MainContent_gridLedgeDetails_txtrefund_" + (refIndx + colindx) + "_" + gRow + "').value)+");
                        }
                        #endregion
                        #region Feeamount Change

                        TextBox txtFeeamt = (TextBox)gridLedgeDetails.Rows[gRow].FindControl("txtFeeamt_" + (feeamtIndx + colindx));
                        if (txtFeeamt != null)
                        {
                            sbFee.Append("parseFloat(document.getElementById('MainContent_gridLedgeDetails_txtFeeamt_" + (feeamtIndx + colindx) + "_" + gRow + "').value)+");
                        }
                        #endregion
                        #region Total Change

                        TextBox txttotal = (TextBox)gridLedgeDetails.Rows[gRow].FindControl("txttotal_" + (totIndx + colindx));
                        if (txttotal != null)
                        {
                            sbTot.Append("parseFloat(document.getElementById('MainContent_gridLedgeDetails_txttotal_" + (totIndx + colindx) + "_" + gRow + "').value)+");
                        }
                        #endregion
                        #region Deduction Change

                        TextBox txtDed = (TextBox)gridLedgeDetails.Rows[gRow].FindControl("txtDed_" + (dedIndx + colindx));
                        if (txtDed != null)
                        {
                            sbDed.Append("parseFloat(document.getElementById('MainContent_gridLedgeDetails_txtDed_" + (dedIndx + colindx) + "_" + gRow + "').value)+");
                        }
                        #endregion
                    }
                    if (sbRef.Length > 0)
                    {
                        sbRef.Remove(sbRef.Length - 1, 1);
                    }
                    string scriptRef = "document.getElementById('MainContent_gridLedgeDetails_txtrefundT_" + (refIndx + colindx) + "_" + (gridLedgeDetails.Rows.Count - 1) + "').value=(" + sbRef + ");";
                    if (txtrefund_ != null)
                        txtrefund_.Attributes.Add("onchange", scriptRef);

                    if (sbFee.Length > 0)
                    {
                        sbFee.Remove(sbFee.Length - 1, 1);
                    }
                    string scriptFee = "document.getElementById('MainContent_gridLedgeDetails_txtFeeamtT_" + (feeamtIndx + colindx) + "_" + (gridLedgeDetails.Rows.Count - 1) + "').value=(" + sbFee + ");";

                    if (sbTot.Length > 0)
                    {
                        sbTot.Remove(sbTot.Length - 1, 1);
                    }
                    string scriptTot = "document.getElementById('MainContent_gridLedgeDetails_txttotalT_" + (totIndx + colindx) + "_" + (gridLedgeDetails.Rows.Count - 1) + "').value=(" + sbTot + ");";

                    if (sbDed.Length > 0)
                    {
                        sbDed.Remove(sbDed.Length - 1, 1);
                    }
                    string scriptDed = "document.getElementById('MainContent_gridLedgeDetails_txtDedT_" + (dedIndx + colindx) + "_" + (gridLedgeDetails.Rows.Count - 1) + "').value=(" + sbDed + ");";

                    #endregion

                    if (txttotal_ != null)
                    {
                        if (txtDed_ != null)
                        {
                            if (txtFeeamt_ != null)
                            {
                                txtDed_.Attributes.Add("onchange", "if(this.value=='') this.value='0.00';var feeamt=parseFloat(document.getElementById('MainContent_gridLedgeDetails_txtFeeamt_" + (feeamtIndx + colindx) + "_" + rowIndx + "').value);var dedamt =parseFloat(document.getElementById('MainContent_gridLedgeDetails_txtDed_" + (dedIndx + colindx) + "_" + rowIndx + "').value); if(dedamt>feeamt){dedamt=0; document.getElementById('MainContent_gridLedgeDetails_txtDed_" + (dedIndx + colindx) + "_" + rowIndx + "').value='0.00';}document.getElementById('MainContent_gridLedgeDetails_txttotal_" + (totIndx + colindx) + "_" + rowIndx + "').value=feeamt-dedamt;" + scriptDed + scriptTot);
                            }
                        }
                        if (txtFeeamt_ != null)
                        {
                            txtFeeamt_.Attributes.Add("onchange", "if(this.value=='') this.value='0.00';var feeamt=parseFloat(document.getElementById('MainContent_gridLedgeDetails_txtFeeamt_" + (feeamtIndx + colindx) + "_" + rowIndx + "').value);var dedamt =0; if(document.getElementById('MainContent_gridLedgeDetails_txtDed_" + (dedIndx + colindx) + "_" + rowIndx + "') !=null)dedamt= parseFloat(document.getElementById('MainContent_gridLedgeDetails_txtDed_" + (dedIndx + colindx) + "_" + rowIndx + "').value); if(dedamt>feeamt){dedamt=0; document.getElementById('MainContent_gridLedgeDetails_txtDed_" + (dedIndx + colindx) + "_" + rowIndx + "').value='0.00';}document.getElementById('MainContent_gridLedgeDetails_txttotal_" + (totIndx + colindx) + "_" + rowIndx + "').value=feeamt-dedamt;" + scriptFee + scriptTot);
                        }
                    }
                    if (prevTable.Rows.Count > 0)
                    {
                        prevTable.DefaultView.RowFilter = " ledgerFk='" + ledgerFk + "' and FeeCategory='" + feeCategory + "'";
                        DataView dvRec = prevTable.DefaultView;
                        if (dvRec.Count > 0)
                        {
                            if (txtFeeamt_ != null)
                            {
                                txtFeeamt_.Text = Convert.ToString(dvRec[0]["FeeAmount"]);
                                if (htTotal.Contains((feeamtIndx + colindx)))
                                {
                                    double val = 0.00;
                                    double val2 = 0.00;
                                    double.TryParse(htTotal[(feeamtIndx + colindx)].ToString(), out val);
                                    double.TryParse(txtFeeamt_.Text, out val2);
                                    val += val2;
                                    htTotal.Remove((feeamtIndx + colindx));
                                    htTotal.Add((feeamtIndx + colindx), val);
                                }
                                else
                                {
                                    double val = 0.00;
                                    double.TryParse(txtFeeamt_.Text, out val);
                                    htTotal.Add((feeamtIndx + colindx), val);
                                }
                            }
                            if (txtDed_ != null)
                            {
                                txtDed_.Text = Convert.ToString(dvRec[0]["DeductAmout"]);
                                if (htTotal.Contains((dedIndx + colindx)))
                                {
                                    double val = 0.00;
                                    double val2 = 0.00;
                                    double.TryParse(htTotal[(dedIndx + colindx)].ToString(), out val);
                                    double.TryParse(txtDed_.Text, out val2);
                                    val += val2;
                                    htTotal.Remove((dedIndx + colindx));
                                    htTotal.Add((dedIndx + colindx), val);
                                }
                                else
                                {
                                    double val = 0.00;
                                    double.TryParse(txtDed_.Text, out val);
                                    htTotal.Add((dedIndx + colindx), val);
                                }
                            }
                            if (txttotal_ != null)
                            {
                                txttotal_.Text = Convert.ToString(dvRec[0]["TotalAmount"]);
                                if (htTotal.Contains((totIndx + colindx)))
                                {
                                    double val = 0.00;
                                    double val2 = 0.00;
                                    double.TryParse(htTotal[(totIndx + colindx)].ToString(), out val);
                                    double.TryParse(txttotal_.Text, out val2);
                                    val += val2;
                                    htTotal.Remove((totIndx + colindx));
                                    htTotal.Add((totIndx + colindx), val);
                                }
                                else
                                {
                                    double val = 0.00;
                                    double.TryParse(txttotal_.Text, out val);
                                    htTotal.Add((totIndx + colindx), val);
                                }
                            }
                            if (txtrefund_ != null)
                            {
                                txtrefund_.Text = Convert.ToString(dvRec[0]["RefundAmount"]);
                                int ind = refIndx + colindx;
                                if (htTotal.Contains(ind))
                                {
                                    double val = 0.00;
                                    double val2 = 0.00;
                                    double.TryParse(htTotal[(refIndx + colindx)].ToString(), out val);
                                    double.TryParse(txtrefund_.Text, out val2);
                                    val += val2;
                                    htTotal.Remove((refIndx + colindx));
                                    htTotal.Add((refIndx + colindx), val);
                                }
                                else
                                {
                                    double val = 0.00;
                                    double.TryParse(txtrefund_.Text, out val);
                                    htTotal.Add((refIndx + colindx), val);
                                }
                            }
                            if (ddlDedReas_ != null)
                            {
                                ddlDedReas_.SelectedIndex = ddlDedReas_.Items.IndexOf(ddlDedReas_.Items.FindByValue(Convert.ToString(dvRec[0]["DeductReason"])));
                            }
                            if (ddlMode_ != null)
                            {
                                ddlMode_.SelectedIndex = (Convert.ToString(dvRec[0]["PayMode"]).Trim() == "2") ? 1 : 0;
                                if (ddlMode_.SelectedIndex == 1)
                                {
                                    if (lblMode != null)
                                    {
                                        lblMode.Text = Convert.ToString(dvRec[0]["FeeAmountMonthly"]);
                                    }
                                }
                            }

                            if (txtpay_ != null)
                            {
                                //string[] dt = Convert.ToString(dvRec[0]["PayStartDate"]).Split('/');
                                //if(dt.Length==3)
                                //    txtpay_.Text = dt[1] + "/" + dt[0] + "/" + dt[2];
                                txtpay_.Text = Convert.ToString(dvRec[0]["PayStartDate"]);
                            }
                            if (txtscholarship_ != null)
                            {
                                txtscholarship_.Text = Convert.ToString(dvRec[0]["FromGovtAmt"]);
                                if (app_no != string.Empty)
                                {
                                    string mulScholar = string.Empty;
                                    double schlAmt = 0;
                                    if (dsSchl.Tables.Count > 0)
                                    {
                                        for (int scl = 0; scl < dsSchl.Tables[0].Rows.Count; scl++)
                                        {
                                            double dbValue = 0;
                                            double.TryParse(d2.GetFunction("select isnull(TotalAmount,0) as schl from FT_FinScholarship where LedgerFK=" + ledgerFk + "  and CollegeCode=" + collegecode1 + " and Feecategory=" + feeCategory + " and App_no=" + app_no + " and ReasonCode=" + dsSchl.Tables[0].Rows[scl]["MasterCode"] + ""), out dbValue);
                                            schlAmt += dbValue;
                                            if (dbValue > 0)
                                            {
                                                if (mulScholar == "")
                                                {
                                                    mulScholar = "" + dsSchl.Tables[0].Rows[scl]["MasterCode"] + ":" + dbValue + "";
                                                }
                                                else
                                                {
                                                    mulScholar = mulScholar + "," + dsSchl.Tables[0].Rows[scl]["MasterCode"] + ":" + dbValue + "";
                                                }
                                            }
                                        }
                                    }
                                    if (schlAmt > 0)
                                    {
                                        txtscholarship_.Text = schlAmt.ToString();
                                        if (ddlSchl_ != null)
                                        {
                                            ddlSchl_.SelectedIndex = 1;
                                            if (lblSchl_ != null)
                                            {
                                                lblSchl_.Text = mulScholar;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #region Last Row
                        TextBox txtFeeamtT_ = (TextBox)row.FindControl("txtFeeamtT_" + (feeamtIndx + colindx));
                        TextBox txtDedT_ = (TextBox)row.FindControl("txtDedT_" + (dedIndx + colindx));
                        TextBox txttotalT_ = (TextBox)row.FindControl("txttotalT_" + (totIndx + colindx));
                        TextBox txtrefundT_ = (TextBox)row.FindControl("txtrefundT_" + (refIndx + colindx));
                        if (txtFeeamtT_ != null && htTotal.Contains(feeamtIndx + colindx))
                        {
                            txtFeeamtT_.Text = Convert.ToString(htTotal[(feeamtIndx + colindx)]);
                        }
                        if (txtDedT_ != null && htTotal.Contains(dedIndx + colindx))
                        {
                            txtDedT_.Text = Convert.ToString(htTotal[(dedIndx + colindx)]);
                        }
                        if (txttotalT_ != null && htTotal.Contains(totIndx + colindx))
                        {
                            txttotalT_.Text = Convert.ToString(htTotal[(totIndx + colindx)]);
                        }
                        if (txtrefundT_ != null && htTotal.Contains(refIndx + colindx))
                        {
                            txtrefundT_.Text = Convert.ToString(htTotal[(refIndx + colindx)]);
                        }
                        #endregion
                    }

                }
                rowIndx++;
            }
        }
        catch { }
    }
    //Month wise Fees Allocation
    protected void ddlMode_Indexchanged(object sender, EventArgs e)
    {
        string uid = this.Page.Request.Params.Get("__EVENTTARGET");
        if (uid != null && uid.Contains("ddlMode_"))
        {
            string[] values = uid.Split('$');
            string row = values[3].Replace("ctl", "");
            string col = values[4].Replace("ddlMode_", "");
            Control ctrl = Page.FindControl(uid);
            DropDownList ddl = (DropDownList)ctrl;
            if (ddl.SelectedIndex == 1)
            {
                monthwise(col, row);
                pnlupdate.Visible = true;
            }
        }
    }
    public void monthwise(string actcol, string actrow)
    {
        try
        {
            #region Montwise Retrieve

            string[] prevYear = new string[13];
            string[] prevAmt = new string[13];
            double totamt = 0;
            try
            {
                string monvwiseDet = string.Empty;// Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt32(actcol) + 1].Tag);
                int rowind = Convert.ToInt32(actrow) - 2;
                int col = Convert.ToInt32(actcol);
                TextBox lblmode = (TextBox)gridLedgeDetails.Rows[rowind].FindControl("lblMode_" + col);
                if (lblmode != null)
                {
                    monvwiseDet = lblmode.Text;
                }
                string[] monsplit = monvwiseDet.Split(',');

                if (monsplit.Length > 0)
                {
                    foreach (string mondet in monsplit)
                    {
                        string[] detSplit = mondet.Split(':');
                        if (detSplit.Length == 3)
                        {
                            prevYear[Convert.ToInt32(detSplit[0])] = detSplit[1];
                            prevAmt[Convert.ToInt32(detSplit[0])] = detSplit[2];
                            double totTempamt = 0;
                            double.TryParse(detSplit[2], out totTempamt);
                            totamt += totTempamt;
                        }
                    }
                }
            }
            catch { }
            #endregion

            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.CommandBar.Visible = false;
            FpSpread3.Sheets[0].AutoPostBack = false;
            FpSpread3.Sheets[0].RowCount = 14;
            FpSpread3.Sheets[0].ColumnCount = 4;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Column.Width = 50;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Month";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Column.Width = 100;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;

            ArrayList array = new ArrayList();
            for (int s = Convert.ToInt32(DateTime.Now.Date.Year); s > 2000; s--)
            {
                array.Add(s);
            }
            string[] droparray = new string[array.Count];
            for (int yea = 0; yea < array.Count; yea++)
            {
                droparray[yea] = array[yea].ToString();
            }
            FarPoint.Web.Spread.ComboBoxCellType cbYear = new FarPoint.Web.Spread.ComboBoxCellType(droparray);
            cbYear.UseValue = true;
            cbYear.ShowButton = true;



            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Year";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Column.Width = 80;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Amount";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Column.Width = 80;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FarPoint.Web.Spread.DoubleCellType intgrcell = new FarPoint.Web.Spread.DoubleCellType();
            intgrcell.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();
            //  intgrcell.MaximumValue = Convert.ToInt32(100);
            intgrcell.MinimumValue = 0;
            intgrcell.ErrorMessage = "Enter valid Number";
            FpSpread3.Sheets[0].Columns[2].CellType = intgrcell;
            FpSpread3.Sheets[0].Columns[2].Font.Bold = false;
            FpSpread3.Sheets[0].Columns[2].Font.Name = "Book Antiqua";

            FpSpread3.Sheets[0].Cells[0, 0].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[0, 0].Text = "1";
            FpSpread3.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[0, 0].Font.Name = "Book Antiqua";

            FpSpread3.Sheets[0].Cells[0, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[0, 1].Text = "January";
            FpSpread3.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[0, 1].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].Cells[0, 2].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[0, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[0, 2].Text = Convert.ToString(prevYear[1]);
            FpSpread3.Sheets[0].Cells[0, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[0, 2].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].Cells[1, 0].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[1, 0].Text = "2";
            FpSpread3.Sheets[0].Cells[1, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[1, 0].Font.Name = "Book Antiqua";

            FpSpread3.Sheets[0].Cells[1, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[1, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[1, 1].Text = "Febraury";
            FpSpread3.Sheets[0].Cells[1, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[1, 1].Font.Name = "Book Antiqua";

            FpSpread3.Sheets[0].Cells[1, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[1, 2].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[1, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[1, 2].Text = Convert.ToString(prevYear[2]);
            FpSpread3.Sheets[0].Cells[1, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[1, 2].Font.Name = "Book Antiqua";

            FpSpread3.Sheets[0].Cells[1, 0].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[2, 0].Text = "3";
            FpSpread3.Sheets[0].Cells[2, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[2, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[2, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[2, 1].Text = "March";
            FpSpread3.Sheets[0].Cells[2, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[2, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[2, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[2, 0].Font.Bold = false;

            FpSpread3.Sheets[0].Cells[2, 2].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[2, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[2, 2].Text = Convert.ToString(prevYear[3]);
            FpSpread3.Sheets[0].Cells[2, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[2, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[2, 2].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].Cells[3, 0].Text = "4";
            FpSpread3.Sheets[0].Cells[3, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[3, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[3, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[3, 1].Text = "April";
            FpSpread3.Sheets[0].Cells[3, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[3, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[3, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[3, 0].Font.Bold = false;

            FpSpread3.Sheets[0].Cells[3, 2].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[3, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[3, 2].Text = Convert.ToString(prevYear[4]);
            FpSpread3.Sheets[0].Cells[3, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[3, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[3, 2].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].Cells[4, 0].Text = "5";
            FpSpread3.Sheets[0].Cells[4, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[4, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[4, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[4, 1].Text = "May";
            FpSpread3.Sheets[0].Cells[4, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[4, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[4, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[4, 0].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[4, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[4, 2].Text = Convert.ToString(prevYear[5]);
            FpSpread3.Sheets[0].Cells[4, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[4, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[4, 2].Font.Size = FontUnit.Medium;

            FpSpread3.Sheets[0].Cells[5, 0].Text = "6";
            FpSpread3.Sheets[0].Cells[5, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[5, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[5, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[5, 1].Text = "June";
            FpSpread3.Sheets[0].Cells[5, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[5, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[5, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[5, 0].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[5, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[5, 2].Text = Convert.ToString(prevYear[6]);
            FpSpread3.Sheets[0].Cells[5, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[5, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[5, 2].Font.Size = FontUnit.Medium;


            FpSpread3.Sheets[0].Cells[6, 0].Text = "7";
            FpSpread3.Sheets[0].Cells[6, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[6, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[6, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[6, 1].Text = "July";
            FpSpread3.Sheets[0].Cells[6, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[6, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[6, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[6, 0].Font.Bold = false;

            FpSpread3.Sheets[0].Cells[6, 2].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[6, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[6, 2].Text = Convert.ToString(prevYear[7]);
            FpSpread3.Sheets[0].Cells[6, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[6, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[6, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[7, 0].Text = "8";
            FpSpread3.Sheets[0].Cells[7, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[7, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[7, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[7, 1].Text = "August";
            FpSpread3.Sheets[0].Cells[7, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[7, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[7, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[7, 2].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[7, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[7, 2].Text = Convert.ToString(prevYear[8]);
            FpSpread3.Sheets[0].Cells[7, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[7, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[7, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[7, 0].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[8, 0].Text = "9";
            FpSpread3.Sheets[0].Cells[8, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[8, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[8, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[8, 1].Text = "September";
            FpSpread3.Sheets[0].Cells[8, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[8, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[8, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[8, 2].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[8, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[8, 2].Text = Convert.ToString(prevYear[9]);
            FpSpread3.Sheets[0].Cells[8, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[8, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[8, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[9, 0].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[9, 0].Text = "10";
            FpSpread3.Sheets[0].Cells[9, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[9, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[9, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[9, 1].Text = "October";
            FpSpread3.Sheets[0].Cells[9, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[9, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[9, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[9, 2].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[9, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[9, 2].Text = Convert.ToString(prevYear[10]);
            FpSpread3.Sheets[0].Cells[9, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[9, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[9, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[10, 0].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[10, 0].Text = "11";
            FpSpread3.Sheets[0].Cells[10, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[10, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[10, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[10, 1].Text = "November";
            FpSpread3.Sheets[0].Cells[10, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[10, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[10, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[10, 2].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[10, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[10, 2].Text = Convert.ToString(prevYear[11]);
            FpSpread3.Sheets[0].Cells[10, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[10, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[10, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[11, 0].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[11, 0].Text = "12";
            FpSpread3.Sheets[0].Cells[11, 0].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[11, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[11, 1].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[11, 1].Text = "December";
            FpSpread3.Sheets[0].Cells[11, 1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[11, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[11, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[11, 2].Font.Bold = false;
            FpSpread3.Sheets[0].Cells[11, 2].CellType = cbYear;
            FpSpread3.Sheets[0].Cells[11, 2].Text = Convert.ToString(prevYear[12]);
            FpSpread3.Sheets[0].Cells[11, 2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread3.Sheets[0].Cells[11, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[11, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Columns[0].Locked = true;
            FpSpread3.Columns[1].Locked = true;

            FarPoint.Web.Spread.IntegerCellType integer = new FarPoint.Web.Spread.IntegerCellType();
            FpSpread3.Sheets[0].Columns[3].CellType = integer;
            FpSpread3.Sheets[0].Cells[0, 3].Text = Convert.ToString(prevAmt[1]);
            FpSpread3.Sheets[0].Cells[1, 3].Text = Convert.ToString(prevAmt[2]);
            FpSpread3.Sheets[0].Cells[2, 3].Text = Convert.ToString(prevAmt[3]);
            FpSpread3.Sheets[0].Cells[3, 3].Text = Convert.ToString(prevAmt[4]);
            FpSpread3.Sheets[0].Cells[4, 3].Text = Convert.ToString(prevAmt[5]);
            FpSpread3.Sheets[0].Cells[5, 3].Text = Convert.ToString(prevAmt[6]);
            FpSpread3.Sheets[0].Cells[6, 3].Text = Convert.ToString(prevAmt[7]);
            FpSpread3.Sheets[0].Cells[7, 3].Text = Convert.ToString(prevAmt[8]);
            FpSpread3.Sheets[0].Cells[8, 3].Text = Convert.ToString(prevAmt[9]);
            FpSpread3.Sheets[0].Cells[9, 3].Text = Convert.ToString(prevAmt[10]);
            FpSpread3.Sheets[0].Cells[10, 3].Text = Convert.ToString(prevAmt[11]);
            FpSpread3.Sheets[0].Cells[11, 3].Text = Convert.ToString(prevAmt[12]);
            FpSpread3.Sheets[0].Cells[13, 3].Text = Convert.ToString(totamt);
            FpSpread3.Sheets[0].SpanModel.Add(12, 0, 1, 3);
            FpSpread3.Sheets[0].SpanModel.Add(13, 0, 1, 2);
            FpSpread3.Sheets[0].Cells[13, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].Cells[13, 0].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Cells[13, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].Cells[13, 0].Text = "Total Amount";
            FpSpread3.Sheets[0].Cells[13, 2].Tag = actcol + "," + actrow;
            FpSpread3.Sheets[0].Cells[13, 2].Locked = true;
            FpSpread3.Sheets[0].Cells[13, 3].Locked = true;

            FpSpread3.Height = 350;
            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].PageSize = 14;
        }
        catch (Exception ex)
        {

        }
    }
    protected void FpSpread3_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            int a1 = (FpSpread3.Sheets[0].RowCount) - 2;
            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Formula = "SUM(D1:D" + a1 + ")";
        }
        catch (Exception ex)
        {

        }
    }
    protected void btnexi_Click(object sender, EventArgs e)
    {
        try
        {
            string[] rowcol = Convert.ToString(FpSpread3.Sheets[0].Cells[13, 2].Tag).Split(',');
            string monthwise = "";
            FpSpread3.SaveChanges();

            int col = Convert.ToInt32(rowcol[0]);

            int colindex = col + 1;

            for (int i = 0; i < 12; i++)
            {
                if (FpSpread3.Sheets[0].Cells[i, 3].Text.Trim() != "")
                {
                    if (monthwise == "")
                    {
                        monthwise = "" + FpSpread3.Sheets[0].Cells[i, 0].Text + ":" + FpSpread3.Sheets[0].Cells[i, 2].Text + ":" + FpSpread3.Sheets[0].Cells[i, 3].Text + "";
                    }
                    else
                    {
                        monthwise = monthwise + "," + FpSpread3.Sheets[0].Cells[i, 0].Text + ":" + FpSpread3.Sheets[0].Cells[i, 2].Text + ":" + FpSpread3.Sheets[0].Cells[i, 3].Text + "";
                    }
                }
            }

            string monthamount = FpSpread3.Sheets[0].Cells[13, 3].Text;
            double feeamt = 0;
            double dedamt = 0;

            int rowind = Convert.ToInt32(rowcol[1]) - 2;
            TextBox lblmode = (TextBox)gridLedgeDetails.Rows[rowind].FindControl("lblMode_" + col);
            if (lblmode != null)
            {
                lblmode.Text = monthwise;
            }
            TextBox txtFeeamt = (TextBox)gridLedgeDetails.Rows[rowind].FindControl("txtFeeamt_" + colindex);
            if (txtFeeamt != null)
            {
                txtFeeamt.Text = monthamount;
                double.TryParse(monthamount, out feeamt);
                txtFeeamt.Attributes.Add("readonly", "readonly");
            }
            if (cblcolumnorder.Items[2].Selected) colindex++;
            TextBox txtDedamt = (TextBox)gridLedgeDetails.Rows[rowind].FindControl("txtDed_" + colindex);
            if (txtDedamt != null)
            {
                double.TryParse(txtDedamt.Text, out dedamt);
            }
            if (cblcolumnorder.Items[3].Selected) colindex++;
            TextBox txtTotamt = (TextBox)gridLedgeDetails.Rows[rowind].FindControl("txttotal_" + ++colindex);

            if (txtTotamt != null)
            {
                txtTotamt.Text = (feeamt - dedamt).ToString();
            }
            pnlupdate.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }
    protected void bindDedReason(DropDownList ddlReas)
    {
        try
        {
            ddlReas.Items.Clear();
            DataSet dsRes = new DataSet();
            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + collegecode1 + "'";
            dsRes = d2.select_method_wo_parameter(sql, "TEXT");
            if (dsRes.Tables.Count > 0 && dsRes.Tables[0].Rows.Count > 0)
            {
                ddlReas.DataSource = dsRes;
                ddlReas.DataTextField = "TextVal";
                ddlReas.DataValueField = "TextCode";
                ddlReas.DataBind();
                ddlReas.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlReas.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch
        { }
    }
    public void callGridBind()
    {
        //string uid = this.Page.Request.Params.Get("__EVENTTARGET");
        //if (uid != null && uid.Contains("gridLedgeDetails"))
        //{
        if (Session["dtGrid"] != null)
        {
            DataTable dtGrid = (DataTable)Session["dtGrid"];
            gridLedgeDetails.DataSource = dtGrid;
            gridLedgeDetails.DataBind();
            gridLedgeDetails.HeaderRow.Visible = false;
        }
        else
        {
            gridLedgeDetails.DataSource = null;
            gridLedgeDetails.DataBind();
        }

        //}
    }
    //Multiple Scholarship
    protected void ddlSchl_IndexChanged(object sender, EventArgs e)
    {
        string uid = this.Page.Request.Params.Get("__EVENTTARGET");
        if (uid != null && uid.Contains("ddlSchl_"))
        {
            string[] values = uid.Split('$');
            string row = values[3].Replace("ctl", "");
            string col = values[4].Replace("ddlSchl_", "");
            try
            {
                Control ctrl = Page.FindControl(uid);
                DropDownList ddl = (DropDownList)ctrl;
                if (ddl.SelectedIndex == 0)
                {
                    ReplaceScholarshipamount(false, row, col);
                }
                else
                {
                    string appno = string.Empty;
                    if (txt_roll.Text.Trim() != "")
                    {
                        LoadMulScholarship(appno, row, col);
                        divMulSchlolar.Visible = true;
                    }
                }
            }
            catch { divMulSchlolar.Visible = false; }
        }
    }
    private void LoadMulScholarship(string appno, string row, string col)
    {
        try
        {
            string query = " select Distinct MasterValue,MasterCode from CO_MasterValues where MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode1 + "' order by MasterValue asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");

            string app_no = "0";
            if (txt_roll.Text.Trim() != "")
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    app_no = d2.GetFunction(" select App_No from Registration where Roll_No='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    app_no = d2.GetFunction(" select App_No from Registration where reg_no='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    app_no = d2.GetFunction(" select App_No from Registration where Roll_admit='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                {
                    app_no = d2.GetFunction(" select app_no from applyn where app_formno='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
            }
            else
            {
                if (appno != "")
                    app_no = appno;
            }
            string feecat = "0";
            string ledger = "0";

            int actrow = Convert.ToInt32(row) - 2;
            int actcol = Convert.ToInt32(col);
            if (actrow > 0 && actcol > 0)
            {
                Label lbl_lgrId = (Label)gridLedgeDetails.Rows[actrow].FindControl("lbl_lgrId");
                if (lbl_lgrId != null)
                {
                    ledger = lbl_lgrId.Text;
                }

                int semCnt = 0;
                ArrayList arrSemIndx = new ArrayList();
                for (int chk = 0; chk < cbl_sem.Items.Count; chk++)
                {
                    if (cbl_sem.Items[chk].Selected) { semCnt++; arrSemIndx.Add(cbl_sem.Items[chk].Value); }
                }
                ArrayList arrColOrder = new ArrayList();
                for (int chk = 0; chk < cblcolumnorder.Items.Count; chk++)
                {
                    if (cblcolumnorder.Items[chk].Selected)
                    {
                        arrColOrder.Add(cblcolumnorder.Items[chk].Text);
                        if (cblcolumnorder.Items[chk].Text == "Scholarship") arrColOrder.Add("Scholarship Type");
                    }
                }

                int feecatind = (actcol - 2) / arrColOrder.Count;

                feecat = arrSemIndx[feecatind].ToString();
            }

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                FpSchloar.Sheets[0].RowHeader.Visible = false;
                FpSchloar.CommandBar.Visible = false;
                FpSchloar.Sheets[0].AutoPostBack = false;
                FpSchloar.Sheets[0].RowCount = 0;
                FpSchloar.Sheets[0].ColumnCount = 3;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 0].Column.Width = 50;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;

                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Scholarship";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 1].Column.Width = 100;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;

                FpSchloar.Columns[0].Locked = true;
                FpSchloar.Columns[1].Locked = true;

                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Amount";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 2].Column.Width = 80;
                FpSchloar.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FarPoint.Web.Spread.DoubleCellType intgrcell = new FarPoint.Web.Spread.DoubleCellType();
                intgrcell.FormatString = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals.ToString();

                intgrcell.MinimumValue = 0;
                intgrcell.ErrorMessage = "Enter valid Number";
                FpSchloar.Sheets[0].Columns[2].CellType = intgrcell;
                FpSchloar.Sheets[0].Columns[2].Font.Bold = false;
                FpSchloar.Sheets[0].Columns[2].Font.Name = "Book Antiqua";

                FarPoint.Web.Spread.IntegerCellType integer = new FarPoint.Web.Spread.IntegerCellType();
                FpSchloar.Sheets[0].Columns[2].CellType = integer;
                double totOvall = 0;
                for (int scl = 0; scl < ds.Tables[0].Rows.Count; scl++)
                {
                    FpSchloar.Sheets[0].RowCount++;
                    FpSchloar.Sheets[0].Cells[scl, 0].Font.Bold = false;
                    FpSchloar.Sheets[0].Cells[scl, 0].Text = (scl + 1).ToString();
                    FpSchloar.Sheets[0].Cells[scl, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSchloar.Sheets[0].Cells[scl, 0].Font.Name = "Book Antiqua";

                    FpSchloar.Sheets[0].Cells[scl, 1].Font.Bold = false;
                    FpSchloar.Sheets[0].Cells[scl, 1].Text = Convert.ToString(ds.Tables[0].Rows[scl]["MasterValue"]);
                    FpSchloar.Sheets[0].Cells[scl, 1].Tag = Convert.ToString(ds.Tables[0].Rows[scl]["MasterCode"]);
                    FpSchloar.Sheets[0].Cells[scl, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSchloar.Sheets[0].Cells[scl, 1].Font.Name = "Book Antiqua";
                    FpSchloar.Sheets[0].Cells[scl, 1].Font.Size = FontUnit.Medium;

                    FpSchloar.Sheets[0].Cells[scl, 2].CellType = intgrcell;

                    if (app_no != "")
                    {
                        double dbValue = 0;
                        double.TryParse(d2.GetFunction("select isnull(TotalAmount,0) as schl from FT_FinScholarship where LedgerFK=" + ledger + "  and CollegeCode=" + collegecode1 + " and Feecategory=" + feecat + " and App_no=" + app_no + " and ReasonCode=" + ds.Tables[0].Rows[scl]["MasterCode"] + ""), out dbValue);
                        FpSchloar.Sheets[0].Cells[scl, 2].Text = dbValue.ToString();
                        totOvall += dbValue;
                    }
                    else
                    {
                        FpSchloar.Sheets[0].Cells[scl, 2].Text = "";
                    }
                }

                FpSchloar.Sheets[0].RowCount++;
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 0].Text = "Total Amount";
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 0].Tag = row + "," + col;
                FpSchloar.Sheets[0].SpanModel.Add(FpSchloar.Sheets[0].RowCount - 1, 0, 1, 2);
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 2].Text = totOvall.ToString();
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 2].Locked = true;
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                FpSchloar.Height = 350;

                FpSchloar.Sheets[0].PageSize = FpSchloar.Sheets[0].RowCount;
                FpSchloar.SaveChanges();
            }
            else
            {
                divMulSchlolar.Visible = false;
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "No Scholarship Type Available";
            }
        }
        catch { }
    }
    protected void FpSchloar_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            int col = (FpSchloar.Sheets[0].RowCount) - 1;
            FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 2].Formula = "SUM(C1:C" + col + ")";
        }
        catch { }
    }
    protected void btnExitScholar_Click(object sender, EventArgs e)
    {
        try
        {
            string[] rowcol = Convert.ToString(FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 0].Tag).Trim().Split(',');
            if (rowcol.Length == 2)
            {
                ReplaceScholarshipamount(true, rowcol[0], rowcol[1]);
            }
            else { divMulSchlolar.Visible = false; }
        }
        catch { divMulSchlolar.Visible = false; }
    }
    private void ReplaceScholarshipamount(bool fromExit, string row, string col1)
    {
        try
        {
            string mulScholar = "";
            string monthamount = "";

            int actrow = Convert.ToInt32(row) - 2;
            int actcol = Convert.ToInt32(col1);
            int col = Convert.ToInt32(actcol);
            int colindex = col - 1;

            if (!fromExit)
            {
                monthamount = "0";
                mulScholar = "";
            }
            else
            {

                FpSchloar.SaveChanges();
                for (int i = 0; i < FpSchloar.Sheets[0].RowCount - 1; i++)
                {
                    if (FpSchloar.Sheets[0].Cells[i, 2].Text.Trim() != "")
                    {
                        if (mulScholar == "")
                        {
                            mulScholar = "" + FpSchloar.Sheets[0].Cells[i, 1].Tag + ":" + FpSchloar.Sheets[0].Cells[i, 2].Text + "";
                        }
                        else
                        {
                            mulScholar = mulScholar + "," + FpSchloar.Sheets[0].Cells[i, 1].Tag + ":" + FpSchloar.Sheets[0].Cells[i, 2].Text + "";
                        }
                    }
                }
                monthamount = FpSchloar.Sheets[0].Cells[FpSchloar.Sheets[0].RowCount - 1, 2].Text;
                divMulSchlolar.Visible = false;
            }

            TextBox txtscholarship_ = (TextBox)gridLedgeDetails.Rows[actrow].FindControl("txtscholarship_" + (actcol - 1));
            if (txtscholarship_ != null)
            {
                txtscholarship_.Text = monthamount;
            }
            TextBox lblSchl_ = (TextBox)gridLedgeDetails.Rows[actrow].FindControl("lblSchl_" + (actcol - 1));
            if (lblSchl_ != null)
            {
                lblSchl_.Text = mulScholar;
            }
        }
        catch { }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {

            string getfinid = d2.getCurrentFinanceYear(usercode, collegecode1);
            if (getfinid != "0" && getfinid != "" && getfinid != null)
            {
                if (ddl_type.SelectedItem.Text == "General")
                {
                    bool inscheck = false;
                    #region General Allocation

                    string memtype = "1";

                    int semCnt = 0;
                    ArrayList arrSemIndx = new ArrayList();
                    for (int chk = 0; chk < cbl_sem.Items.Count; chk++)
                    {
                        if (cbl_sem.Items[chk].Selected) { semCnt++; arrSemIndx.Add(cbl_sem.Items[chk].Value); }
                    }
                    ArrayList arrColOrder = new ArrayList();
                    for (int chk = 0; chk < cblcolumnorder.Items.Count; chk++)
                    {
                        if (cblcolumnorder.Items[chk].Selected)
                        {
                            arrColOrder.Add(cblcolumnorder.Items[chk].Text);
                            if (cblcolumnorder.Items[chk].Text == "Scholarship") arrColOrder.Add("Scholarship Type");
                        }
                    }

                    #region colIndex for selected order

                    int modeIndx = -1, feeamtIndx = -1, dedIndx = -1, dedReIndx = -1, totIndx = -1, refIndx = -1, schIndx = -1, schlTypeIndx = -1, payIndx = -1;
                    if (arrColOrder.Contains("Mode"))
                    {
                        modeIndx = arrColOrder.IndexOf("Mode");
                    }
                    if (arrColOrder.Contains("Fee Amount"))
                    {
                        feeamtIndx = arrColOrder.IndexOf("Fee Amount");
                    }
                    if (arrColOrder.Contains("Deduction"))
                    {
                        dedIndx = arrColOrder.IndexOf("Deduction");
                    }
                    if (arrColOrder.Contains("Deduction Reason"))
                    {
                        dedReIndx = arrColOrder.IndexOf("Deduction Reason");
                    }
                    if (arrColOrder.Contains("Total"))
                    {
                        totIndx = arrColOrder.IndexOf("Total");
                    }
                    if (arrColOrder.Contains("Refund"))
                    {
                        refIndx = arrColOrder.IndexOf("Refund");
                    }
                    if (arrColOrder.Contains("Pay Start Date"))
                    {
                        payIndx = arrColOrder.IndexOf("Pay Start Date");
                    }
                    if (arrColOrder.Contains("Scholarship"))
                    {
                        schIndx = arrColOrder.IndexOf("Scholarship");
                        schlTypeIndx = schIndx + 1;
                    }
                    #endregion

                    bool batchOk = false;
                    bool deptOk = false;
                    bool seatOk = false;

                    int rowIndx = 0;
                    foreach (GridViewRow row in gridLedgeDetails.Rows)
                    {
                        string ledgerId = string.Empty;
                        string headerid = string.Empty;

                        Label lblLegrId = (Label)row.FindControl("lbl_lgrId");
                        if (lblLegrId != null)
                        {
                            ledgerId = lblLegrId.Text.Trim();
                            headerid = d2.GetFunction("select headerfk from fm_ledgermaster where ledgerpk='" + ledgerId + "'").Trim();
                        }

                        if (!string.IsNullOrEmpty(ledgerId))
                        {
                            for (int colindx = 2, semcnt = 1; semcnt <= semCnt; semcnt++, colindx += arrColOrder.Count)
                            {
                                string mulScholarVal = string.Empty;
                                string feecateg = arrSemIndx[(semcnt - 1)].ToString();
                                string paymode = "1";
                                string feeamountmonthly = string.Empty;
                                string deductreason = "0";
                                string paidAmt = "PaidAmount";
                                string duedt = string.Empty;
                                string startdt = string.Empty;
                                string frm = "0";
                                string isfeedeposit = "0";
                                string fineamnt = "0";

                                double feeamt = 0;
                                double dedAmt = 0;
                                double totalAmt = 0;
                                double refAmt = 0;
                                double schAmt = 0;

                                DropDownList ddlMode = (DropDownList)row.FindControl("ddlMode_" + (modeIndx + colindx));
                                TextBox lblMode = (TextBox)row.FindControl("lblMode_" + (modeIndx + colindx));
                                TextBox txtFeeamt_ = (TextBox)row.FindControl("txtFeeamt_" + (feeamtIndx + colindx));
                                TextBox txtDed_ = (TextBox)row.FindControl("txtDed_" + (dedIndx + colindx));
                                DropDownList ddlDed = (DropDownList)row.FindControl("ddlDedReas_" + (dedReIndx + colindx));
                                TextBox txttotal_ = (TextBox)row.FindControl("txttotal_" + (totIndx + colindx));
                                TextBox txtrefund_ = (TextBox)row.FindControl("txtrefund_" + (refIndx + colindx));
                                TextBox txtpay_ = (TextBox)row.FindControl("txtpay_" + (payIndx + colindx));
                                TextBox txtscholarship_ = (TextBox)row.FindControl("txtscholarship_" + (schIndx + colindx));
                                DropDownList ddlSchl_ = (DropDownList)row.FindControl("ddlSchl_" + (schlTypeIndx + colindx));
                                TextBox lblSchl_ = (TextBox)row.FindControl("lblSchl_" + (schIndx + colindx));

                                if (ddlMode != null)
                                {
                                    if (ddlMode.SelectedIndex == 0)
                                    {
                                        paymode = "1";
                                        feeamountmonthly = string.Empty;
                                    }
                                    else
                                    {
                                        paymode = "2";
                                        if (lblMode != null)
                                            feeamountmonthly = lblMode.Text;
                                    }
                                }

                                if (txtDed_ != null)
                                {
                                    dedAmt = Convert.ToDouble(txtDed_.Text);
                                }
                                if (txtFeeamt_ != null)
                                {
                                    feeamt = Convert.ToDouble(txtFeeamt_.Text);
                                }
                                if (txttotal_ != null)
                                {
                                    totalAmt = Convert.ToDouble(txttotal_.Text);
                                }
                                if (feeamt <= dedAmt)
                                {
                                    paidAmt = "0";
                                }
                                if (txtrefund_ != null)
                                {
                                    refAmt = Convert.ToDouble(txtrefund_.Text);
                                }

                                if (txtscholarship_ != null)
                                {
                                    schAmt = Convert.ToDouble(txtscholarship_.Text);
                                    frm = schAmt.ToString();
                                    if (ddlSchl_ != null)
                                    {
                                        if (ddlSchl_.SelectedIndex == 1)
                                        {
                                            if (lblSchl_ != null)
                                            {
                                                mulScholarVal = lblSchl_.Text;
                                            }
                                        }
                                    }
                                }
                                if (refAmt > 0)
                                {
                                    isfeedeposit = "1";
                                }

                                if (ddlDed != null)
                                {
                                    if (ddlDed.SelectedIndex != 0)
                                    {
                                        deductreason = ddlDed.SelectedValue;
                                    }
                                }
                                if (txtpay_ != null)
                                {
                                    duedt = txtpay_.Text.Split('/')[1] + "/" + txtpay_.Text.Split('/')[0] + "/" + txtpay_.Text.Split('/')[2];
                                }

                                #region Save in FeeAllotDegree

                                for (int i = 0; i < cbl_batch.Items.Count; i++)
                                {
                                    if (cbl_batch.Items[i].Selected == true)
                                    {
                                        batchOk = true;

                                        for (int j = 0; j < cbl_dept.Items.Count; j++)
                                        {
                                            if (cbl_dept.Items[j].Selected == true)
                                            {
                                                deptOk = true;

                                                for (int k = 0; k < cbl_seat.Items.Count; k++)
                                                {
                                                    if (cbl_seat.Items[k].Selected == true)
                                                    {
                                                        seatOk = true;
                                                        if (feeamt > 0 && (totalAmt > 0 || (feeamt > dedAmt)))
                                                        {
                                                            string insupdquery = "if exists (select * from FT_FeeAllotDegree where DegreeCode ='" + cbl_dept.Items[j].Value + "' and BatchYear ='" + cbl_batch.Items[i].Value + "' and FeeCategory ='" + feecateg + "' and SeatType='" + cbl_seat.Items[k].Value + "' and HeaderFK ='" + headerid + "' and LedgerFK ='" + ledgerId + "' ) update FT_FeeAllotDegree set  AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',FeeAmount='" + feeamt + "',DeductAmout='" + dedAmt + "',DeductReason='" + deductreason + "',TotalAmount='" + totalAmt + "',RefundAmount='" + refAmt + "',IsFeeDeposit='" + isfeedeposit + "',FeeAmountMonthly='" + feeamountmonthly + "',PayMode='" + paymode + "',PayStartDate='" + duedt.ToString() + "',DueDate='" + startdt.ToString() + "',FineAmount='" + fineamnt + "' where DegreeCode ='" + cbl_dept.Items[j].Value + "' and BatchYear ='" + cbl_batch.Items[i].Value + "' and FeeCategory ='" + feecateg + "' and SeatType='" + cbl_seat.Items[k].Value + "' and HeaderFK ='" + headerid + "' and LedgerFK ='" + ledgerId + "'  else INSERT INTO FT_FeeAllotDegree(AllotDate,BatchYear,DegreeCode,SeatType,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,DueDate,FineAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + cbl_batch.Items[i].Value + "','" + cbl_dept.Items[j].Value + "','" + cbl_seat.Items[k].Value + "','" + ledgerId + "','" + headerid + "','" + feeamt + "','" + dedAmt + "','" + deductreason + "','" + totalAmt + "','" + refAmt + "','" + isfeedeposit + "','" + feeamountmonthly + "','" + paymode + "','" + feecateg + "','" + duedt.ToString() + "','" + startdt.ToString() + "','" + fineamnt + "','" + getfinid + "')";//and FinYearFK='" + getfinid + "',and FinYearFK='" + getfinid + "'

                                                            int inscount = d2.update_method_wo_parameter(insupdquery, "Text");
                                                            if (inscount > 0)
                                                            {
                                                                inscheck = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string delquery = "delete FT_FeeAllotDegree where LedgerFK in('" + ledgerId + "') and HeaderFK in('" + headerid + "') and FeeCategory in('" + feecateg + "') and BatchYear ='" + cbl_batch.Items[i].Value + "' and DegreeCode ='" + cbl_dept.Items[j].Value + "' and SeatType='" + cbl_seat.Items[k].Value + "'";
                                                            int delcount = d2.update_method_wo_parameter(delquery, "Text");
                                                            if (delcount > 0)
                                                            {
                                                                inscheck = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        rowIndx++;
                    }

                    if (inscheck == true)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Saved Successfully";

                        if (ViewState["appNoList"] != null)
                            ViewState.Remove("appNoList");
                    }
                    else
                    {

                        if (!deptOk)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Select " + lbl_dept.Text;
                        }
                        else if (!batchOk)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Select Batch";
                        }
                        else if (!seatOk)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Select Seat Type";
                        }
                    }
                    #endregion
                }
                else
                {
                    bool studOk = false;
                    List<string> appnoList = new List<string>();
                    if (ViewState["appNoList"] != null)
                    {
                        appnoList = (List<string>)ViewState["appNoList"];
                    }

                    if (appnoList.Count > 0 || txt_roll.Text.Trim() != "")
                    {
                        studOk = true;
                    }
                    if (studOk)
                    {
                        #region Save for Individual Admitted and Applied
                        int refundcheck = 0;
                        if (ddl_type.SelectedItem.Text == "Individual(Admitted)" || ddl_type.SelectedItem.Text == "Individual(Applied)")
                        {
                            bool inscheck = false;
                            if (txt_roll.Text.Trim() != "")
                            {
                                #region textbox not equal to empty

                                string memtype = "1";
                                string appl_no = "";
                                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                                {
                                    appl_no = d2.GetFunction(" select App_No from Registration where Roll_No='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                                }
                                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                                {
                                    appl_no = d2.GetFunction(" select App_No from Registration where reg_no='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                                }
                                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                                {
                                    appl_no = d2.GetFunction(" select App_No from Registration where Roll_admit='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                                }
                                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                                {
                                    appl_no = d2.GetFunction(" select app_no from applyn where app_formno='" + txt_roll.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                                }

                                int semCnt = 0;
                                ArrayList arrSemIndx = new ArrayList();
                                for (int chk = 0; chk < cbl_sem.Items.Count; chk++)
                                {
                                    if (cbl_sem.Items[chk].Selected) { semCnt++; arrSemIndx.Add(cbl_sem.Items[chk].Value); }
                                }
                                ArrayList arrColOrder = new ArrayList();
                                for (int chk = 0; chk < cblcolumnorder.Items.Count; chk++)
                                {
                                    if (cblcolumnorder.Items[chk].Selected)
                                    {
                                        arrColOrder.Add(cblcolumnorder.Items[chk].Text);
                                        if (cblcolumnorder.Items[chk].Text == "Scholarship") arrColOrder.Add("Scholarship Type");
                                    }
                                }

                                #region colIndex for selected order

                                int modeIndx = -1, feeamtIndx = -1, dedIndx = -1, dedReIndx = -1, totIndx = -1, refIndx = -1, schIndx = -1, schlTypeIndx = -1, payIndx = -1;
                                if (arrColOrder.Contains("Mode"))
                                {
                                    modeIndx = arrColOrder.IndexOf("Mode");
                                }
                                if (arrColOrder.Contains("Fee Amount"))
                                {
                                    feeamtIndx = arrColOrder.IndexOf("Fee Amount");
                                }
                                if (arrColOrder.Contains("Deduction"))
                                {
                                    dedIndx = arrColOrder.IndexOf("Deduction");
                                }
                                if (arrColOrder.Contains("Deduction Reason"))
                                {
                                    dedReIndx = arrColOrder.IndexOf("Deduction Reason");
                                }
                                if (arrColOrder.Contains("Total"))
                                {
                                    totIndx = arrColOrder.IndexOf("Total");
                                }
                                if (arrColOrder.Contains("Refund"))
                                {
                                    refIndx = arrColOrder.IndexOf("Refund");
                                }
                                if (arrColOrder.Contains("Pay Start Date"))
                                {
                                    payIndx = arrColOrder.IndexOf("Pay Start Date");
                                }
                                if (arrColOrder.Contains("Scholarship"))
                                {
                                    schIndx = arrColOrder.IndexOf("Scholarship");
                                    schlTypeIndx = schIndx + 1;
                                }
                                #endregion

                                int rowIndx = 0;
                                foreach (GridViewRow row in gridLedgeDetails.Rows)
                                {
                                    for (int colindx = 2, semcnt = 1; semcnt <= semCnt; semcnt++, colindx += arrColOrder.Count)
                                    {
                                        string mulScholarVal = string.Empty;
                                        string feecateg = arrSemIndx[(semcnt - 1)].ToString();
                                        string ledgerId = string.Empty;
                                        string headerid = string.Empty;
                                        string paymode = "1";
                                        string feeamountmonthly = string.Empty;
                                        string deductreason = "0";
                                        string paidAmt = "PaidAmount";
                                        string duedt = string.Empty;
                                        string startdt = string.Empty;
                                        string frm = "0";
                                        string isfeedeposit = "0";
                                        string fineamnt = "0";

                                        double feeamt = 0;
                                        double dedAmt = 0;
                                        double totalAmt = 0;
                                        double refAmt = 0;
                                        double schAmt = 0;

                                        Label lblLegrId = (Label)row.FindControl("lbl_lgrId");
                                        if (lblLegrId != null)
                                        {
                                            ledgerId = lblLegrId.Text.Trim();
                                            headerid = d2.GetFunction("select headerfk from fm_ledgermaster where ledgerpk='" + ledgerId + "'").Trim();
                                        }

                                        DropDownList ddlMode = (DropDownList)row.FindControl("ddlMode_" + (modeIndx + colindx));
                                        TextBox lblMode = (TextBox)row.FindControl("lblMode_" + (modeIndx + colindx));
                                        TextBox txtFeeamt_ = (TextBox)row.FindControl("txtFeeamt_" + (feeamtIndx + colindx));
                                        TextBox txtDed_ = (TextBox)row.FindControl("txtDed_" + (dedIndx + colindx));
                                        DropDownList ddlDed = (DropDownList)row.FindControl("ddlDedReas_" + (dedReIndx + colindx));
                                        TextBox txttotal_ = (TextBox)row.FindControl("txttotal_" + (totIndx + colindx));
                                        TextBox txtrefund_ = (TextBox)row.FindControl("txtrefund_" + (refIndx + colindx));
                                        TextBox txtpay_ = (TextBox)row.FindControl("txtpay_" + (payIndx + colindx));
                                        TextBox txtscholarship_ = (TextBox)row.FindControl("txtscholarship_" + (schIndx + colindx));
                                        DropDownList ddlSchl_ = (DropDownList)row.FindControl("ddlSchl_" + (schlTypeIndx + colindx));
                                        TextBox lblSchl_ = (TextBox)row.FindControl("lblSchl_" + (schIndx + colindx));

                                        if (ddlMode != null)
                                        {
                                            if (ddlMode.SelectedIndex == 0)
                                            {
                                                paymode = "1";
                                                feeamountmonthly = string.Empty;
                                            }
                                            else
                                            {
                                                paymode = "2";
                                                if (lblMode != null)
                                                    feeamountmonthly = lblMode.Text;
                                            }
                                        }

                                        if (txtDed_ != null)
                                        {
                                            dedAmt = Convert.ToDouble(txtDed_.Text);
                                        }
                                        if (txtFeeamt_ != null)
                                        {
                                            feeamt = Convert.ToDouble(txtFeeamt_.Text);
                                        }
                                        if (txttotal_ != null)
                                        {
                                            totalAmt = Convert.ToDouble(txttotal_.Text);
                                        }
                                        if (feeamt <= dedAmt)
                                        {
                                            paidAmt = "0";
                                        }
                                        if (txtrefund_ != null)
                                        {
                                            refAmt = Convert.ToDouble(txtrefund_.Text);
                                        }

                                        if (txtscholarship_ != null)
                                        {
                                            schAmt = Convert.ToDouble(txtscholarship_.Text);
                                            frm = schAmt.ToString();
                                            if (ddlSchl_ != null)
                                            {
                                                if (ddlSchl_.SelectedIndex == 1)
                                                {
                                                    if (lblSchl_ != null)
                                                    {
                                                        mulScholarVal = lblSchl_.Text;
                                                    }
                                                }
                                            }
                                        }
                                        if (refAmt > 0)
                                        {
                                            isfeedeposit = "1";
                                        }

                                        if (ddlDed != null)
                                        {
                                            if (ddlDed.SelectedIndex != 0)
                                            {
                                                deductreason = ddlDed.SelectedValue;
                                            }
                                        }
                                        if (txtpay_ != null)
                                        {
                                            duedt = txtpay_.Text.Split('/')[1] + "/" + txtpay_.Text.Split('/')[0] + "/" + txtpay_.Text.Split('/')[2];
                                        }

                                        #region Save in FeeAllot
                                        if (feeamt > 0 && (totalAmt > 0 || (feeamt > dedAmt) || (feeamt == dedAmt)))
                                        {
                                            string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ledgerId + "') and HeaderFK in('" + headerid + "') and FeeCategory in('" + feecateg + "')  and App_No in('" + appl_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='" + memtype + "',FeeAmount='" + feeamt + "',PaidAmount=" + paidAmt + ",DeductAmout='" + dedAmt + "',DeductReason='" + deductreason + "',FromGovtAmt='" + frm + "',TotalAmount='" + totalAmt + "',RefundAmount='" + refAmt + "',IsFeeDeposit='" + isfeedeposit + "',FeeAmountMonthly='" + feeamountmonthly + "',PayMode='" + paymode + "',PayStartDate='" + duedt.ToString() + "',PaidStatus='0',DueDate='" + startdt.ToString() + "',DueAmount='0',FineAmount='" + fineamnt + "',BalAmount=" + totalAmt + " - isnull(PaidAmount,0) where LedgerFK in('" + ledgerId + "') and HeaderFK in('" + headerid + "') and FeeCategory in('" + feecateg + "') and App_No in('" + appl_no + "') and isnull(PaidAmount,0) <=" + totalAmt + " else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "'," + memtype + "," + appl_no + ",'" + ledgerId + "','" + headerid + "','" + feeamt + "','" + dedAmt + "'," + deductreason + ",'" + frm + "','" + totalAmt + "','" + refAmt + "','" + isfeedeposit + "','" + feeamountmonthly + "','" + paymode + "','" + feecateg + "','" + duedt.ToString() + "','0','" + startdt.ToString() + "','0','" + fineamnt + "','" + totalAmt + "','" + getfinid + "')";

                                            try
                                            {
                                                string[] reasonsWtValue = mulScholarVal.Split(',');
                                                if (reasonsWtValue.Length > 0)
                                                {
                                                    for (int reas = 0; reas < reasonsWtValue.Length; reas++)
                                                    {
                                                        string[] reasonAdValues = reasonsWtValue[reas].Split(':');
                                                        if (reasonAdValues.Length == 2)
                                                        {
                                                            string insUpQuery = "if exists (select * from FT_FinScholarship where App_no=" + appl_no + " and LedgerFK=" + ledgerId + " and HeaderFk=" + headerid + " and Feecategory=" + feecateg + " and ReasonCode=" + reasonAdValues[0] + " and CollegeCode=" + collegecode1 + " and FinyearFk=" + getfinid + ") update Ft_FinScholarship set AlloteDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',TotalAmount=" + reasonAdValues[1] + ",AdjusAmount=0 where App_no=" + appl_no + " and LedgerFK=" + ledgerId + " and HeaderFk=" + headerid + " and Feecategory=" + feecateg + " and ReasonCode=" + reasonAdValues[0] + " and CollegeCode=" + collegecode1 + "  and FinyearFk=" + getfinid + " else INSERT INTO FT_FINSCHOLARSHIP (App_no, LedgerFK, HeaderFk, ReasonCode, Feecategory, AlloteDate, TotalAmount, AdjusAmount, FinyearFK, CollegeCode) VALUES(" + appl_no + ", " + ledgerId + ", " + headerid + ", " + reasonAdValues[0] + ", " + feecateg + ", '" + DateTime.Now.ToString("MM/dd/yyyy") + "', " + reasonAdValues[1] + ", 0, " + getfinid + ", " + collegecode1 + ")";
                                                            d2.update_method_wo_parameter(insUpQuery, "Text");
                                                        }
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                            }
                                            int inscount = d2.update_method_wo_parameter(insupdquery, "Text");
                                            if (inscount > 0)
                                            {
                                                if (paymode.Trim() == "2")
                                                {
                                                    double feeallotpk = 0;
                                                    double.TryParse(d2.GetFunction("select feeallotpk from FT_FeeAllot where App_No=" + appl_no + " and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "' and FeeCategory=" + feecateg + "   ").Trim(), out feeallotpk);
                                                    if (feeallotpk > 0)
                                                    {
                                                        string delQ = "if exists (select feeallotpk from ft_feeallotmonthly where feeallotpk=" + feeallotpk + ") delete from FT_FeeallotMonthly where FeeAllotPK=" + feeallotpk + " and  Isnull(PaidAmount,0) =0  ";
                                                        d2.update_method_wo_parameter(delQ, "Text");

                                                        string[] months = feeamountmonthly.Split(',');
                                                        foreach (string month in months)
                                                        {
                                                            string[] resultval = month.Split(':');
                                                            if (resultval.Length == 3)
                                                            {
                                                                string insMonwiseQ = "if exists (select * from ft_feeallotmonthly where FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + resultval[0] + " ) update ft_feeallotmonthly set  AllotAmount= " + resultval[2] + ", BalAmount=(" + resultval[2] + "-isnull(PaidAMount,0)), FinYearFK=" + getfinid + ",AllotYear=" + resultval[1] + " where  FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + resultval[0] + "   else INSERT INTO ft_feeallotmonthly (FeeAllotPK, AllotMonth, AllotYear, AllotAmount, BalAmount, FinYearFK) VALUES (" + feeallotpk + ", " + resultval[0] + ", " + resultval[1] + ", " + resultval[2] + ", " + resultval[2] + ", " + getfinid + ")";
                                                                d2.update_method_wo_parameter(insMonwiseQ, "Text");
                                                            }
                                                        }
                                                    }
                                                }

                                                inscheck = true;
                                            }
                                            //refund amount
                                            if (refAmt > 0)
                                            {
                                                double Refndamt = 0;
                                                double Exledgamt = 0;
                                                string RefInsQ = "";
                                                string FTPK = "";
                                                string ExLEINQ = "";
                                                double.TryParse(refAmt.ToString(), out Refndamt);
                                                string SelQExPK = d2.GetFunction(" select ExcessDetPK from FT_ExcessDet where ExcessType='3' and App_No='" + appl_no + "' and FeeCategory='" + feecateg + "'  and FinYearFK='" + getfinid + "'");
                                                if (SelQExPK != "" && SelQExPK != "0")
                                                {
                                                    string SelEx = d2.GetFunction(" select SUM(ISNULL(AdjAmt,'0')) as Paid from FT_ExcessLedgerDet where FeeCategory='" + feecateg + "' and ExcessDetFK='" + SelQExPK + "' and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "'  and FinYearFK='" + getfinid + "'");
                                                    double.TryParse(SelEx, out Exledgamt);
                                                    if (Refndamt >= Exledgamt)
                                                    {
                                                        refundcheck++;
                                                        if (refundcheck == 1)
                                                        {
                                                            string update = " update FT_ExcessDet set ExcessAmt=0,BalanceAmt=0 where ExcessDetPK='" + SelQExPK + "' and App_No='" + appl_no + "' and MemType='1' and FeeCategory='" + feecateg + "' and ExcessType='3'";
                                                            int updrefnd = d2.update_method_wo_parameter(update, "Text");
                                                        }
                                                        RefInsQ = "if exists (select * from FT_ExcessDet where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "'  and FinYearFK='" + getfinid + "')update FT_ExcessDet set ExcessAmt=ISNULL(ExcessAmt,'0')+'" + refAmt + "',BalanceAmt=ISNULL(BalanceAmt,'0')+'" + refAmt + "' where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "' and excessType='3' and FinYearFK='" + getfinid + "' else insert into FT_ExcessDet (ExcessTransDate,TransTime,app_no,memtype,excessType,Excessamt,balanceamt,feecategory,FinYearFK) values('" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToLongTimeString() + "','" + appl_no + "','1','3','" + refAmt + "','" + refAmt + "','" + feecateg + "','" + getfinid + "')";
                                                        d2.update_method_wo_parameter(RefInsQ, "Text");
                                                        FTPK = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "'  and FinYearFK='" + getfinid + "'");
                                                        if (FTPK != "" && FTPK != "0")
                                                        {
                                                            ExLEINQ = " if exists (select * from FT_ExcessLedgerDet where ExcessDetFK='" + FTPK + "' and FeeCategory='" + feecateg + "' and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "' and FinYearFK='" + getfinid + "')update FT_ExcessLedgerDet set ExcessAmt='" + refAmt + "',BalanceAmt='" + refAmt + "' where ExcessDetFK='" + FTPK + "' and FeeCategory='" + feecateg + "' and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "' and FinYearFK='" + getfinid + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,FeeCategory,FinYearFK) values('" + headerid + "','" + ledgerId + "','" + refAmt + "','" + refAmt + "','" + FTPK + "','" + feecateg + "','" + getfinid + "')";
                                                            d2.update_method_wo_parameter(ExLEINQ, "Text");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    refundcheck++;
                                                    RefInsQ = "if exists (select * from FT_ExcessDet where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "'  and FinYearFK='" + getfinid + "')update FT_ExcessDet set ExcessAmt='" + refAmt + "',BalanceAmt='" + refAmt + "' where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "' and excessType='3' and FinYearFK='" + getfinid + "' else insert into FT_ExcessDet (ExcessTransDate,TransTime,app_no,memtype,excessType,Excessamt,balanceamt,feecategory,FinYearFK) values('" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToLongTimeString() + "','" + appl_no + "','1','3','" + refAmt + "','" + refAmt + "','" + feecateg + "','" + getfinid + "')";
                                                    d2.update_method_wo_parameter(RefInsQ, "Text");
                                                    FTPK = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "'  and FinYearFK='" + getfinid + "'");
                                                    if (FTPK != "" && FTPK != "0")
                                                    {
                                                        ExLEINQ = " if exists (select * from FT_ExcessLedgerDet where ExcessDetFK='" + FTPK + "' and FeeCategory='" + feecateg + "' and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "' and FinYearFK='" + getfinid + "')update FT_ExcessLedgerDet set ExcessAmt='" + refAmt + "',BalanceAmt='" + refAmt + "' where ExcessDetFK='" + FTPK + "' and FeeCategory='" + feecateg + "' and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "' and FinYearFK='" + getfinid + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,FeeCategory,FinYearFK) values('" + headerid + "','" + ledgerId + "','" + refAmt + "','" + refAmt + "','" + FTPK + "','" + feecateg + "','" + getfinid + "')";
                                                        d2.update_method_wo_parameter(ExLEINQ, "Text");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string delquery = "delete FT_FeeAllot where LedgerFK in('" + ledgerId + "') and HeaderFK in('" + headerid + "') and FeeCategory in('" + feecateg + "') and App_No in('" + appl_no + "')";
                                            int delcount = d2.update_method_wo_parameter(delquery, "Text");
                                            if (delcount > 0)
                                            {
                                                inscheck = true;
                                            }
                                        }
                                        #endregion
                                    }
                                    rowIndx++;
                                }

                                if (inscheck == true)
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Visible = true;
                                    lblalerterr.Text = "Saved Successfully";
                                }
                                #endregion
                            }
                            else
                            {
                                foreach (string appl_no in appnoList)
                                {
                                    #region multiple students

                                    string memtype = "1";

                                    int semCnt = 0;
                                    ArrayList arrSemIndx = new ArrayList();
                                    for (int chk = 0; chk < cbl_sem.Items.Count; chk++)
                                    {
                                        if (cbl_sem.Items[chk].Selected) { semCnt++; arrSemIndx.Add(cbl_sem.Items[chk].Value); }
                                    }
                                    ArrayList arrColOrder = new ArrayList();
                                    for (int chk = 0; chk < cblcolumnorder.Items.Count; chk++)
                                    {
                                        if (cblcolumnorder.Items[chk].Selected)
                                        {
                                            arrColOrder.Add(cblcolumnorder.Items[chk].Text);
                                            if (cblcolumnorder.Items[chk].Text == "Scholarship") arrColOrder.Add("Scholarship Type");
                                        }
                                    }

                                    #region colIndex for selected order

                                    int modeIndx = -1, feeamtIndx = -1, dedIndx = -1, dedReIndx = -1, totIndx = -1, refIndx = -1, schIndx = -1, schlTypeIndx = -1, payIndx = -1;
                                    if (arrColOrder.Contains("Mode"))
                                    {
                                        modeIndx = arrColOrder.IndexOf("Mode");
                                    }
                                    if (arrColOrder.Contains("Fee Amount"))
                                    {
                                        feeamtIndx = arrColOrder.IndexOf("Fee Amount");
                                    }
                                    if (arrColOrder.Contains("Deduction"))
                                    {
                                        dedIndx = arrColOrder.IndexOf("Deduction");
                                    }
                                    if (arrColOrder.Contains("Deduction Reason"))
                                    {
                                        dedReIndx = arrColOrder.IndexOf("Deduction Reason");
                                    }
                                    if (arrColOrder.Contains("Total"))
                                    {
                                        totIndx = arrColOrder.IndexOf("Total");
                                    }
                                    if (arrColOrder.Contains("Refund"))
                                    {
                                        refIndx = arrColOrder.IndexOf("Refund");
                                    }
                                    if (arrColOrder.Contains("Pay Start Date"))
                                    {
                                        payIndx = arrColOrder.IndexOf("Pay Start Date");
                                    }
                                    if (arrColOrder.Contains("Scholarship"))
                                    {
                                        schIndx = arrColOrder.IndexOf("Scholarship");
                                        schlTypeIndx = schIndx + 1;
                                    }
                                    #endregion

                                    int rowIndx = 0;
                                    foreach (GridViewRow row in gridLedgeDetails.Rows)
                                    {
                                        string ledgerId = string.Empty;
                                        string headerid = string.Empty;

                                        Label lblLegrId = (Label)row.FindControl("lbl_lgrId");
                                        if (lblLegrId != null)
                                        {
                                            ledgerId = lblLegrId.Text.Trim();
                                            headerid = d2.GetFunction("select headerfk from fm_ledgermaster where ledgerpk='" + ledgerId + "'").Trim();
                                        }

                                        if (!string.IsNullOrEmpty(ledgerId))
                                        {
                                            for (int colindx = 2, semcnt = 1; semcnt <= semCnt; semcnt++, colindx += arrColOrder.Count)
                                            {
                                                string mulScholarVal = string.Empty;
                                                string feecateg = arrSemIndx[(semcnt - 1)].ToString();
                                                string paymode = "1";
                                                string feeamountmonthly = string.Empty;
                                                string deductreason = "0";
                                                string paidAmt = "PaidAmount";
                                                string duedt = string.Empty;
                                                string startdt = string.Empty;
                                                string frm = "0";
                                                string isfeedeposit = "0";
                                                string fineamnt = "0";

                                                double feeamt = 0;
                                                double dedAmt = 0;
                                                double totalAmt = 0;
                                                double refAmt = 0;
                                                double schAmt = 0;

                                                DropDownList ddlMode = (DropDownList)row.FindControl("ddlMode_" + (modeIndx + colindx));
                                                TextBox lblMode = (TextBox)row.FindControl("lblMode_" + (modeIndx + colindx));
                                                TextBox txtFeeamt_ = (TextBox)row.FindControl("txtFeeamt_" + (feeamtIndx + colindx));
                                                TextBox txtDed_ = (TextBox)row.FindControl("txtDed_" + (dedIndx + colindx));
                                                DropDownList ddlDed = (DropDownList)row.FindControl("ddlDedReas_" + (dedReIndx + colindx));
                                                TextBox txttotal_ = (TextBox)row.FindControl("txttotal_" + (totIndx + colindx));
                                                TextBox txtrefund_ = (TextBox)row.FindControl("txtrefund_" + (refIndx + colindx));
                                                TextBox txtpay_ = (TextBox)row.FindControl("txtpay_" + (payIndx + colindx));
                                                TextBox txtscholarship_ = (TextBox)row.FindControl("txtscholarship_" + (schIndx + colindx));
                                                DropDownList ddlSchl_ = (DropDownList)row.FindControl("ddlSchl_" + (schlTypeIndx + colindx));
                                                TextBox lblSchl_ = (TextBox)row.FindControl("lblSchl_" + (schIndx + colindx));

                                                if (ddlMode != null)
                                                {
                                                    if (ddlMode.SelectedIndex == 0)
                                                    {
                                                        paymode = "1";
                                                        feeamountmonthly = string.Empty;
                                                    }
                                                    else
                                                    {
                                                        paymode = "2";
                                                        if (lblMode != null)
                                                            feeamountmonthly = lblMode.Text;
                                                    }
                                                }

                                                if (txtDed_ != null)
                                                {
                                                    dedAmt = Convert.ToDouble(txtDed_.Text);
                                                }
                                                if (txtFeeamt_ != null)
                                                {
                                                    feeamt = Convert.ToDouble(txtFeeamt_.Text);
                                                }
                                                if (txttotal_ != null)
                                                {
                                                    totalAmt = Convert.ToDouble(txttotal_.Text);
                                                }
                                                if (feeamt <= dedAmt)
                                                {
                                                    paidAmt = "0";
                                                }
                                                if (txtrefund_ != null)
                                                {
                                                    refAmt = Convert.ToDouble(txtrefund_.Text);
                                                }

                                                if (txtscholarship_ != null)
                                                {
                                                    schAmt = Convert.ToDouble(txtscholarship_.Text);
                                                    frm = schAmt.ToString();
                                                    if (ddlSchl_ != null)
                                                    {
                                                        if (ddlSchl_.SelectedIndex == 1)
                                                        {
                                                            if (lblSchl_ != null)
                                                            {
                                                                mulScholarVal = lblSchl_.Text;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (refAmt > 0)
                                                {
                                                    isfeedeposit = "1";
                                                }

                                                if (ddlDed != null)
                                                {
                                                    if (ddlDed.SelectedIndex != 0)
                                                    {
                                                        deductreason = ddlDed.SelectedValue;
                                                    }
                                                }
                                                if (txtpay_ != null)
                                                {
                                                    duedt = txtpay_.Text.Split('/')[1] + "/" + txtpay_.Text.Split('/')[0] + "/" + txtpay_.Text.Split('/')[2];
                                                }

                                                #region Save in FeeAllot
                                                if (feeamt > 0 && (totalAmt > 0 || (feeamt > dedAmt)))
                                                {
                                                    string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ledgerId + "') and HeaderFK in('" + headerid + "') and FeeCategory in('" + feecateg + "')  and App_No in('" + appl_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='" + memtype + "',FeeAmount='" + feeamt + "',PaidAmount=" + paidAmt + ",DeductAmout='" + dedAmt + "',DeductReason='" + deductreason + "',FromGovtAmt='" + frm + "',TotalAmount='" + totalAmt + "',RefundAmount='" + refAmt + "',IsFeeDeposit='" + isfeedeposit + "',FeeAmountMonthly='" + feeamountmonthly + "',PayMode='" + paymode + "',PayStartDate='" + duedt.ToString() + "',PaidStatus='0',DueDate='" + startdt.ToString() + "',DueAmount='0',FineAmount='" + fineamnt + "',BalAmount=" + totalAmt + " - isnull(PaidAmount,0) where LedgerFK in('" + ledgerId + "') and HeaderFK in('" + headerid + "') and FeeCategory in('" + feecateg + "') and App_No in('" + appl_no + "') and isnull(PaidAmount,0) <=" + totalAmt + " else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "'," + memtype + "," + appl_no + ",'" + ledgerId + "','" + headerid + "','" + feeamt + "','" + dedAmt + "'," + deductreason + ",'" + frm + "','" + totalAmt + "','" + refAmt + "','" + isfeedeposit + "','" + feeamountmonthly + "','" + paymode + "','" + feecateg + "','" + duedt.ToString() + "','0','" + startdt.ToString() + "','0','" + fineamnt + "','" + totalAmt + "','" + getfinid + "')";

                                                    try
                                                    {
                                                        string[] reasonsWtValue = mulScholarVal.Split(',');
                                                        if (reasonsWtValue.Length > 0)
                                                        {
                                                            for (int reas = 0; reas < reasonsWtValue.Length; reas++)
                                                            {
                                                                string[] reasonAdValues = reasonsWtValue[reas].Split(':');
                                                                if (reasonAdValues.Length == 2)
                                                                {
                                                                    string insUpQuery = "if exists (select * from FT_FinScholarship where App_no=" + appl_no + " and LedgerFK=" + ledgerId + " and HeaderFk=" + headerid + " and Feecategory=" + feecateg + " and ReasonCode=" + reasonAdValues[0] + " and CollegeCode=" + collegecode1 + " and FinyearFk=" + getfinid + ") update Ft_FinScholarship set AlloteDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',TotalAmount=" + reasonAdValues[1] + ",AdjusAmount=0 where App_no=" + appl_no + " and LedgerFK=" + ledgerId + " and HeaderFk=" + headerid + " and Feecategory=" + feecateg + " and ReasonCode=" + reasonAdValues[0] + " and CollegeCode=" + collegecode1 + "  and FinyearFk=" + getfinid + " else INSERT INTO FT_FINSCHOLARSHIP (App_no, LedgerFK, HeaderFk, ReasonCode, Feecategory, AlloteDate, TotalAmount, AdjusAmount, FinyearFK, CollegeCode) VALUES(" + appl_no + ", " + ledgerId + ", " + headerid + ", " + reasonAdValues[0] + ", " + feecateg + ", '" + DateTime.Now.ToString("MM/dd/yyyy") + "', " + reasonAdValues[1] + ", 0, " + getfinid + ", " + collegecode1 + ")";
                                                                    d2.update_method_wo_parameter(insUpQuery, "Text");
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch
                                                    {
                                                    }
                                                    int inscount = d2.update_method_wo_parameter(insupdquery, "Text");
                                                    if (inscount > 0)
                                                    {
                                                        if (paymode.Trim() == "2")
                                                        {
                                                            double feeallotpk = 0;
                                                            double.TryParse(d2.GetFunction("select feeallotpk from FT_FeeAllot where App_No=" + appl_no + " and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "' and FeeCategory=" + feecateg + "   ").Trim(), out feeallotpk);
                                                            if (feeallotpk > 0)
                                                            {
                                                                string delQ = "if exists (select feeallotpk from ft_feeallotmonthly where feeallotpk=" + feeallotpk + ") delete from FT_FeeallotMonthly where FeeAllotPK=" + feeallotpk + " and  Isnull(PaidAmount,0) =0  ";
                                                                d2.update_method_wo_parameter(delQ, "Text");

                                                                string[] months = feeamountmonthly.Split(',');
                                                                foreach (string month in months)
                                                                {
                                                                    string[] resultval = month.Split(':');
                                                                    if (resultval.Length == 3)
                                                                    {
                                                                        string insMonwiseQ = "if exists (select * from ft_feeallotmonthly where FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + resultval[0] + " ) update ft_feeallotmonthly set  AllotAmount= " + resultval[2] + ", BalAmount=(" + resultval[2] + "-isnull(PaidAMount,0)), FinYearFK=" + getfinid + ",AllotYear=" + resultval[1] + " where  FeeAllotPK=" + feeallotpk + " and  AllotMonth=" + resultval[0] + "   else INSERT INTO ft_feeallotmonthly (FeeAllotPK, AllotMonth, AllotYear, AllotAmount, BalAmount, FinYearFK) VALUES (" + feeallotpk + ", " + resultval[0] + ", " + resultval[1] + ", " + resultval[2] + ", " + resultval[2] + ", " + getfinid + ")";
                                                                        d2.update_method_wo_parameter(insMonwiseQ, "Text");
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        inscheck = true;
                                                    }
                                                    //refund amount
                                                    if (refAmt > 0)
                                                    {
                                                        double Refndamt = 0;
                                                        double Exledgamt = 0;
                                                        string RefInsQ = "";
                                                        string FTPK = "";
                                                        string ExLEINQ = "";
                                                        double.TryParse(refAmt.ToString(), out Refndamt);
                                                        string SelQExPK = d2.GetFunction(" select ExcessDetPK from FT_ExcessDet where ExcessType='3' and App_No='" + appl_no + "' and FeeCategory='" + feecateg + "'  and FinYearFK='" + getfinid + "'");
                                                        if (SelQExPK != "" && SelQExPK != "0")
                                                        {
                                                            string SelEx = d2.GetFunction(" select SUM(ISNULL(AdjAmt,'0')) as Paid from FT_ExcessLedgerDet where FeeCategory='" + feecateg + "' and ExcessDetFK='" + SelQExPK + "' and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "'  and FinYearFK='" + getfinid + "'");
                                                            double.TryParse(SelEx, out Exledgamt);
                                                            if (Refndamt >= Exledgamt)
                                                            {
                                                                refundcheck++;
                                                                if (refundcheck == 1)
                                                                {
                                                                    string update = " update FT_ExcessDet set ExcessAmt=0,BalanceAmt=0 where ExcessDetPK='" + SelQExPK + "' and App_No='" + appl_no + "' and MemType='1' and FeeCategory='" + feecateg + "' and ExcessType='3'";
                                                                    int updrefnd = d2.update_method_wo_parameter(update, "Text");
                                                                }
                                                                RefInsQ = "if exists (select * from FT_ExcessDet where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "'  and FinYearFK='" + getfinid + "')update FT_ExcessDet set ExcessAmt=ISNULL(ExcessAmt,'0')+'" + refAmt + "',BalanceAmt=ISNULL(BalanceAmt,'0')+'" + refAmt + "' where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "' and excessType='3' and FinYearFK='" + getfinid + "' else insert into FT_ExcessDet (ExcessTransDate,TransTime,app_no,memtype,excessType,Excessamt,balanceamt,feecategory,FinYearFK) values('" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToLongTimeString() + "','" + appl_no + "','1','3','" + refAmt + "','" + refAmt + "','" + feecateg + "','" + getfinid + "')";
                                                                d2.update_method_wo_parameter(RefInsQ, "Text");
                                                                FTPK = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "'  and FinYearFK='" + getfinid + "'");
                                                                if (FTPK != "" && FTPK != "0")
                                                                {
                                                                    ExLEINQ = " if exists (select * from FT_ExcessLedgerDet where ExcessDetFK='" + FTPK + "' and FeeCategory='" + feecateg + "' and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "' and FinYearFK='" + getfinid + "')update FT_ExcessLedgerDet set ExcessAmt='" + refAmt + "',BalanceAmt='" + refAmt + "' where ExcessDetFK='" + FTPK + "' and FeeCategory='" + feecateg + "' and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "' and FinYearFK='" + getfinid + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,FeeCategory,FinYearFK) values('" + headerid + "','" + ledgerId + "','" + refAmt + "','" + refAmt + "','" + FTPK + "','" + feecateg + "','" + getfinid + "')";
                                                                    d2.update_method_wo_parameter(ExLEINQ, "Text");
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            refundcheck++;
                                                            RefInsQ = "if exists (select * from FT_ExcessDet where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "'  and FinYearFK='" + getfinid + "')update FT_ExcessDet set ExcessAmt='" + refAmt + "',BalanceAmt='" + refAmt + "' where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "' and excessType='3' and FinYearFK='" + getfinid + "' else insert into FT_ExcessDet (ExcessTransDate,TransTime,app_no,memtype,excessType,Excessamt,balanceamt,feecategory,FinYearFK) values('" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToLongTimeString() + "','" + appl_no + "','1','3','" + refAmt + "','" + refAmt + "','" + feecateg + "','" + getfinid + "')";
                                                            d2.update_method_wo_parameter(RefInsQ, "Text");
                                                            FTPK = d2.GetFunction("select ExcessDetPK from FT_ExcessDet where App_No='" + appl_no + "' and FeeCategory='" + feecateg + "'  and FinYearFK='" + getfinid + "'");
                                                            if (FTPK != "" && FTPK != "0")
                                                            {
                                                                ExLEINQ = " if exists (select * from FT_ExcessLedgerDet where ExcessDetFK='" + FTPK + "' and FeeCategory='" + feecateg + "' and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "' and FinYearFK='" + getfinid + "')update FT_ExcessLedgerDet set ExcessAmt='" + refAmt + "',BalanceAmt='" + refAmt + "' where ExcessDetFK='" + FTPK + "' and FeeCategory='" + feecateg + "' and LedgerFK='" + ledgerId + "' and HeaderFK='" + headerid + "' and FinYearFK='" + getfinid + "' else insert into FT_ExcessLedgerDet (HeaderFK,LedgerFK,ExcessAmt,BalanceAmt,ExcessDetFK,FeeCategory,FinYearFK) values('" + headerid + "','" + ledgerId + "','" + refAmt + "','" + refAmt + "','" + FTPK + "','" + feecateg + "','" + getfinid + "')";
                                                                d2.update_method_wo_parameter(ExLEINQ, "Text");
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    string delquery = "delete FT_FeeAllot where LedgerFK in('" + ledgerId + "') and HeaderFK in('" + headerid + "') and FeeCategory in('" + feecateg + "') and App_No in('" + appl_no + "')";
                                                    int delcount = d2.update_method_wo_parameter(delquery, "Text");
                                                    if (delcount > 0)
                                                    {
                                                        inscheck = true;
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        rowIndx++;
                                    }

                                    if (inscheck == true)
                                    {
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Saved Successfully";

                                        if (ViewState["appNoList"] != null)
                                            ViewState.Remove("appNoList");
                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Student Selected";
                    }
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Finance Year Not Set";
            }
        }
        catch (Exception exc)
        {
            d2.sendErrorMail(exc, collegecode1, "JournalGrid.aspx");
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Please Try Later...";
        }
    }

    //Code Last modified by Idhris 10-12-2016
    #region Rollno and name


    protected void txt_roll_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string rollno = Convert.ToString(txt_roll.Text.Trim());
            if (rollno != "")
            {
                if (ddl_type.SelectedItem.Text.Trim() == "Individual(Admitted)")
                {
                    string query = "select   a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name as Name from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and r.Exam_Flag <>'DEBAR'  and r.college_code=" + collegecode1 + "";
                    //and r.CC=0 and r.DelFlag =0
                    if (dirAccess.selectScalarInt("select LinkValue from New_InsSettings where LinkName='IncludeDiscontinuedInJournal' and user_code ='" + usercode + "'  ") == 0)//and college_code ='" + collegecode1 + "'
                    {
                        query += "  and r.DelFlag =0 ";
                    }

                    if (dirAccess.selectScalarInt("select LinkValue from New_InsSettings where LinkName='IncludeCompletedInJournal' and user_code ='" + usercode + "'  ") == 0)//and college_code ='" + collegecode1 + "'
                    {
                        query += "  and r.CC=0 ";
                    }

                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        query = query + " and r.Roll_no='" + rollno + "' ";
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        query = query + " and r.Reg_No='" + rollno + "'";
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        query = query + " and r.Roll_Admit='" + rollno + "' ";
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                    {
                        query = query + " and a.app_formno='" + rollno + "' ";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(query, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        txt_name.Text = Convert.ToString(ds.Tables[0].Rows[0]["Name"]);
                    }
                    else
                    {
                        txt_name.Text = "";
                    }
                }
                else if (ddl_type.SelectedItem.Text.Trim() == "Individual(Applied)")
                {
                    string query = "select a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name as Name from applyn a,Course C,Degree d,Department dt where a.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and a.college_code=" + collegecode1 + " and a.app_formno='" + rollno + "'";
                    // query = query + " and a.app_formno='" + rollno + "' ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(query, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        txt_name.Text = Convert.ToString(ds.Tables[0].Rows[0]["Name"]);
                    }
                    else
                    {
                        txt_name.Text = "";
                    }
                }
            }
        }
        catch { }
    }

    #endregion
    protected void showLedgerSetting()
    {
        try
        {
            string rghtval = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Show All Ledger' and college_code='" + ddl_college.SelectedItem.Value + "' and user_code='" + usercode + "'");
            if (rghtval == "1")
            {

            }
            else
            {

            }
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
        lbl.Add(lbl_collegename);
        //lbl.Add(lbl_stream);
        lbl.Add(lbl_course);
        lbl.Add(lbl_dept);
        lbl.Add(lbl_sem);
        fields.Add(0);
        // fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    //added by sudhagar 
    private double checkSchoolSetting()
    {
        double getVal = 0;
        try
        {
            double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);

        }
        catch { }
        return getVal;
    }

    protected void loadType()
    {
        try
        {
            cbl_type.Items.Clear();
            if (checkSchoolSetting() == 0)
            {
                cbl_type.Items.Add(new ListItem("Old Studnent", "1"));
                cbl_type.Items.Add(new ListItem("New Student", "3"));
                cbl_type.Items.Add(new ListItem("Transfer", "2"));
            }
            else
            {
                cbl_type.Items.Add(new ListItem("Regular", "1"));
                cbl_type.Items.Add(new ListItem("Lateral", "3"));
                cbl_type.Items.Add(new ListItem("Transfer", "2"));
                cbl_type.Items.Add(new ListItem("IrRegular", "4"));
            }
        }
        catch { }
    }

    //roll,reg,admis no setting added by sudhagar
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
                FpSpreadstud.Columns[2].Visible = true;
                FpSpreadstud.Columns[3].Visible = true;
                FpSpreadstud.Columns[4].Visible = true;
            }
            else if (roll == 1)
            {
                FpSpreadstud.Columns[2].Visible = true;
                FpSpreadstud.Columns[3].Visible = true;
                FpSpreadstud.Columns[4].Visible = true;
            }
            else if (roll == 2)
            {
                FpSpreadstud.Columns[2].Visible = true;
                FpSpreadstud.Columns[3].Visible = false;
                FpSpreadstud.Columns[4].Visible = false;

            }
            else if (roll == 3)
            {
                FpSpreadstud.Columns[2].Visible = false;
                FpSpreadstud.Columns[3].Visible = true;
                FpSpreadstud.Columns[4].Visible = false;
            }
            else if (roll == 4)
            {
                FpSpreadstud.Columns[2].Visible = false;
                FpSpreadstud.Columns[3].Visible = false;
                FpSpreadstud.Columns[4].Visible = true;
            }
            else if (roll == 5)
            {
                FpSpreadstud.Columns[2].Visible = true;
                FpSpreadstud.Columns[3].Visible = true;
                FpSpreadstud.Columns[4].Visible = false;
            }
            else if (roll == 6)
            {
                FpSpreadstud.Columns[2].Visible = false;
                FpSpreadstud.Columns[3].Visible = true;
                FpSpreadstud.Columns[4].Visible = true;
            }
            else if (roll == 7)
            {
                FpSpreadstud.Columns[2].Visible = true;
                FpSpreadstud.Columns[3].Visible = false;
                FpSpreadstud.Columns[4].Visible = true;
            }
        }
        catch { }
    }

    #endregion

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
            string linkname = "journalGrid column order settings";
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
                    if (text.Trim() == "Mode" || text.Trim() == "Fee Amount" || text.Trim() == "Total")
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
    //discontinue setting query collegecode removed  11.03.2017 sudhagar

    //added by sudhagar disability option 11.03.2017
    protected void cbdisa_CheckedChanged(object sender, EventArgs e)
    {
        if (cbdisa.Checked == true)
        {
            for (int i = 0; i < cbldisa.Items.Count; i++)
            {
                cbldisa.Items[i].Selected = true;
            }
            txtdisa.Text = "Disability(" + (cbldisa.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbldisa.Items.Count; i++)
            {
                cbldisa.Items[i].Selected = false;
            }
            txtdisa.Text = "--Select--";
        }
    }
    protected void cbldisa_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtdisa.Text = "--Select--";
        cbdisa.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cbldisa.Items.Count; i++)
        {
            if (cbldisa.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtdisa.Text = "Disability(" + commcount.ToString() + ")";
            if (commcount == cbldisa.Items.Count)
            {
                cbdisa.Checked = true;
            }
        }
    }

    protected void loadDisable()
    {
        try
        {
            cbldisa.Items.Clear();
            cbldisa.Items.Add(new ListItem("Isdisable", "1"));
            cbldisa.Items.Add(new ListItem("Handy", "1"));
            cbldisa.Items.Add(new ListItem("Visualhandy", "1"));
            cbldisa.Items.Add(new ListItem("Islearningdis", "1"));
            cbldisa.Items.Add(new ListItem("Isdisabledisc", "1"));
        }
        catch { }
    }
    protected string getDisable()
    {
        string strDisable = string.Empty;
        try
        {
            for (int row = 0; row < cbldisa.Items.Count; row++)
            {
                if (cbldisa.Items[row].Selected)
                {
                    if (cbldisa.Items[row].Text == "Isdisable")
                    {
                        strDisable = " and( Isdisable='1'";
                    }
                    if (cbldisa.Items[row].Text == "Handy")
                    {
                        if (strDisable == string.Empty)
                            strDisable += " and( Handy='1'";
                        else
                            strDisable += " or Handy='1'";
                    }
                    if (cbldisa.Items[row].Text == "Visualhandy")
                    {
                        if (strDisable == string.Empty)
                            strDisable += " and( Visualhandy='1'";
                        else
                            strDisable += " or Visualhandy='1'";
                    }
                    if (cbldisa.Items[row].Text == "Islearningdis")
                    {
                        if (strDisable == string.Empty)
                            strDisable += " and( Islearningdis='1'";
                        else
                            strDisable += " or Islearningdis='1'";
                    }
                    if (cbldisa.Items[row].Text == "Isdisabledisc")
                    {
                        if (strDisable == string.Empty)
                            strDisable += " and( Isdisabledisc='1'";
                        else
                            strDisable += " or Isdisabledisc='1'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(strDisable))
                strDisable += " )";
        }
        catch { strDisable = string.Empty; }
        return strDisable;
    }
}

