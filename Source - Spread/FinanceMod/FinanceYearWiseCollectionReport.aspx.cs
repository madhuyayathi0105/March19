using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Configuration;

public partial class FinanceYearWiseCollectionReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    int commcount;
    int i;
    int cout;
    int row;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

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
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
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
                loadcollege();
                if (ddl_collegename.Items.Count > 0)
                    collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                loadcollege();
                loadseat();
                loadtype();
                bindBtch();
                binddeg();
                binddept();
                bindsem();
                loadfinanceyear();
                //getFinyear();
                header1bind();
                ledger1bind();
                //treeledger.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
            }
            if (ddl_collegename.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
        }
        catch (Exception ex) { }
    }
    public void loadcollege()
    {
        try
        {
            ddl_collegename.Items.Clear();
            DataSet dsCol = new DataSet();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            dsCol = d2.select_method_wo_parameter(Query, "Text");
            if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = dsCol;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        { }
    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {

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

    #region batch
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
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = lblbatch.Text + "(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }


    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");

    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");

    }
    #endregion
    #region degree


    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            //string stream = "";
            //if (ddlstream.Items.Count > 0)
            //{
            //    if (ddlstream.SelectedItem.Text != "")
            //    {
            //        stream = ddlstream.SelectedItem.Text.ToString();
            //    }
            //}

            cbl_degree.Items.Clear();
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }

        }
        catch { }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
        binddept();

    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
        binddept();

    }
    #endregion

    #region dept
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            string batch = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    else
                        batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                }
            }
            string degree = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                        degree = Convert.ToString(cbl_degree.Items[i].Value);

                    else
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                }

            }

            string collegecode = ddl_collegename.SelectedItem.Value.ToString();
            if (batch != "" && degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = lbldept.Text + "(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }

        }
        catch { }
    }
    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");

    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");

    }
    #endregion



    #region header and ledger
    public void header1bind()
    {
        try
        {


            ds.Clear();
            cbl_header1.Items.Clear();
            //  string query = "select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "' ORDER BY HeaderName";
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode + "  ";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {


                cbl_header1.DataSource = ds;
                cbl_header1.DataTextField = "HeaderName";
                cbl_header1.DataValueField = "HeaderPK";
                cbl_header1.DataBind();

                cb_header1.Checked = true;

                if (cbl_header1.Items.Count > 0)
                {
                    for (i = 0; i < cbl_header1.Items.Count; i++)
                    {
                        cbl_header1.Items[i].Selected = true;

                    }
                    txt_header1.Text = "Header(" + cbl_header1.Items.Count + ")";
                    cb_header1.Checked = true;
                    // ledgerbind();

                }
            }

        }
        catch { }
    }
    public void ledger1bind()
    {
        try
        {
            string HeaderPK = "";

            for (i = 0; i < cbl_header1.Items.Count; i++)
            {

                if (cbl_header1.Items[i].Selected == true)
                {

                    if (HeaderPK == "")
                    {
                        HeaderPK = cbl_header1.Items[i].Value.ToString();
                    }
                    else
                    {
                        HeaderPK += "','" + cbl_header1.Items[i].Value.ToString();

                    }
                }
            }

            ds.Clear();
            cbl_ledger1.Items.Clear();
            // string query = " select LedgerPK,LedgerName from FM_LedgerMaster  where CollegeCode='" + collegecode1 + "' and HeaderFK IN('" + HeaderPK + "')  order by isnull(priority,1000), ledgerName asc ";
            string query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode + "  and L.HeaderFK in('" + HeaderPK + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            //string query = "SELECT Fee_Code,Fee_Type FROM fee_info I,acctheader H WHERE I.header_id = H.header_id AND I.header_id IN ('" + itemheadercode + "') and  Fee_Type NOT IN ('Cash','Income & Expenditure','Misc','Excess Amount','Fine') AND Fee_Type NOT IN (SELECT BankName FROM Bank_Master1) ORDER BY Fee_Type";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_ledger1.DataSource = ds;
                cbl_ledger1.DataTextField = "LedgerName";
                cbl_ledger1.DataValueField = "LedgerPK";
                cbl_ledger1.DataBind();

                if (cbl_ledger1.Items.Count > 0)
                {

                    for (int i = 0; i < cbl_ledger1.Items.Count; i++)
                    {
                        cbl_ledger1.Items[i].Selected = true;
                    }
                    txt_ledger1.Text = "Ledger(" + cbl_ledger1.Items.Count + ")";
                    cb_ledger1.Checked = true;
                }
            }


        }

        catch
        {
        }
    }
    protected void cb_header1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_header1.Text = "--Select--";
            if (cb_header1.Checked == true)
            {

                for (i = 0; i < cbl_header1.Items.Count; i++)
                {
                    cbl_header1.Items[i].Selected = true;
                }
                txt_header1.Text = "Header(" + (cbl_header1.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_header1.Items.Count; i++)
                {
                    cbl_header1.Items[i].Selected = false;
                }
                txt_header1.Text = "--Select--";
            }
            ledger1bind();

        }
        catch { }

    }
    protected void cbl_header1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_header1.Checked = false;
            commcount = 0;
            txt_header1.Text = "--Select--";
            for (i = 0; i < cbl_header1.Items.Count; i++)
            {
                if (cbl_header1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_header1.Items.Count)
                {
                    cb_header1.Checked = true;
                }
                txt_header1.Text = "Header(" + commcount.ToString() + ")";
            }
            ledger1bind();

        }
        catch { }
    }


    protected void cb_ledger1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_ledger1.Text = "--Select--";
            if (cb_ledger1.Checked == true)
            {

                for (i = 0; i < cbl_ledger1.Items.Count; i++)
                {
                    cbl_ledger1.Items[i].Selected = true;
                }
                txt_ledger1.Text = "Ledger(" + (cbl_ledger1.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_ledger1.Items.Count; i++)
                {
                    cbl_ledger1.Items[i].Selected = false;
                }
            }

        }
        catch { }

    }
    protected void cbl_ledger1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_ledger1.Checked = false;
            commcount = 0;
            txt_ledger1.Text = "--Select--";
            for (i = 0; i < cbl_ledger1.Items.Count; i++)
            {
                if (cbl_ledger1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_ledger1.Items.Count)
                {
                    cb_ledger1.Checked = true;
                }
                txt_ledger1.Text = "Ledger(" + commcount.ToString() + ")";
            }

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
    #endregion

    #region financial year
    protected Hashtable getFinyear()
    {
        Hashtable htfin = new Hashtable();
        try
        {
            string SelQ = "  select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)))as finyear,finyearpk,collegecode from fm_finyearmaster where collegecode='" + collegecode + "'";
            DataSet dsval = da.select_method_wo_parameter(SelQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!htfin.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["finyearpk"])))
                        htfin.Add(Convert.ToString(dsval.Tables[0].Rows[row]["finyearpk"]), Convert.ToString(dsval.Tables[0].Rows[row]["finyear"]));
                }
            }
        }
        catch { htfin.Clear(); }
        return htfin;
    }
    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + collegecode + "'  order by FinYearPK desc";
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
    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
        }
        catch (Exception ex)
        { }

    }
    protected void bindsem()
    {
        try
        {
            string clgvalue = collegecode;
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = da.loadFeecategory(clgvalue, usercode, ref linkName);
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



    #endregion


    #region loadspreadfunction


    #endregion
    //protected void loadspread(DataSet dsvalue)
    //{
    //    #region design Spread

    //    FarPoint.Web.Spread.TextCellType txtcel = new FarPoint.Web.Spread.TextCellType();
    //    spreadDet.Sheets[0].RowCount = 0;
    //    spreadDet.Sheets[0].ColumnCount = 0;
    //    spreadDet.CommandBar.Visible = false;
    //    spreadDet.Sheets[0].AutoPostBack = true;
    //    spreadDet.Sheets[0].RowHeader.Visible = false;
    //    Hashtable htActYr = getFinyear();

    //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    //    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //    darkstyle.ForeColor = Color.White;
    //    spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
    //    spreadDet.Sheets[0].ColumnCount++;
    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //    spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    //    spreadDet.Sheets[0].ColumnCount++;
    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Class";

    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
    //    spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //    spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
    //    spreadDet.Sheets[0].Columns[1].Width = 350;
    //    int checkva = 0;
    //    int sno = 0;

    //    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
    //    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
    //    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
    //    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
    //    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);




    //    #region getvalue
    //    Dictionary<string, string> dicfeecat = new Dictionary<string, string>();

    //    string feecatquery = "";
    //    for (int i = 0; i < cbl_sem.Items.Count; i++)
    //    {
    //        if (cbl_sem.Items[i].Selected == true)
    //        {
    //            string feecat = cbl_sem.Items[i].Value.ToString();
    //            if (feecatquery == "")
    //            {
    //                feecatquery = feecat;
    //            }
    //            else
    //            {
    //                feecatquery = feecatquery + "," + feecat;
    //            }
    //            string getval = cbl_sem.Items[i].Text.ToString();
    //            string[] spt = getval.Split(' ');
    //            if (spt.GetUpperBound(0) >= 0)
    //            {
    //                if (spt[1].ToString().ToLower().Trim().Contains("semester"))
    //                {
    //                    if (spt[0].ToString().Trim() == "1" || spt[0].ToString().Trim() == "2")
    //                    {
    //                        if (!dicfeecat.ContainsKey("1"))
    //                        {
    //                            dicfeecat.Add("1", feecat);
    //                        }
    //                        else
    //                        {
    //                            string setval = dicfeecat["1"] + ',' + feecat;
    //                            dicfeecat["1"] = setval;
    //                        }
    //                    }
    //                    else if (spt[0].ToString().Trim() == "3" || spt[0].ToString().Trim() == "4")
    //                    {
    //                        if (!dicfeecat.ContainsKey("2"))
    //                        {
    //                            dicfeecat.Add("2", feecat);
    //                        }
    //                        else
    //                        {
    //                            string setval = dicfeecat["2"] + ',' + feecat;
    //                            dicfeecat["2"] = setval;
    //                        }
    //                    }
    //                    else if (spt[0].ToString().Trim() == "5" || spt[0].ToString().Trim() == "6")
    //                    {
    //                        if (!dicfeecat.ContainsKey("3"))
    //                        {
    //                            dicfeecat.Add("3", feecat);
    //                        }
    //                        else
    //                        {
    //                            string setval = dicfeecat["3"] + ',' + feecat;
    //                            dicfeecat["3"] = setval;
    //                        }
    //                    }
    //                    else if (spt[0].ToString().Trim() == "7" || spt[0].ToString().Trim() == "8")
    //                    {
    //                        if (!dicfeecat.ContainsKey("4"))
    //                        {
    //                            dicfeecat.Add("4", feecat);
    //                        }
    //                        else
    //                        {
    //                            string setval = dicfeecat["4"] + ',' + feecat;
    //                            dicfeecat["4"] = setval;
    //                        }
    //                    }
    //                    else if (spt[0].ToString().Trim() == "9" || spt[0].ToString().Trim() == "10")
    //                    {
    //                        if (!dicfeecat.ContainsKey("5"))
    //                        {
    //                            dicfeecat.Add("5", feecat);
    //                        }
    //                        else
    //                        {
    //                            string setval = dicfeecat["5"] + ',' + feecat;
    //                            dicfeecat["5"] = feecat;
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    if (spt[0].ToString().Trim() == "1")
    //                    {
    //                        if (!dicfeecat.ContainsKey("1"))
    //                        {
    //                            dicfeecat.Add("1", feecat);
    //                        }
    //                        else
    //                        {
    //                            string setval = dicfeecat["1"] + ',' + feecat;
    //                            dicfeecat["1"] = setval;
    //                        }
    //                    }
    //                    else if (spt[0].ToString().Trim() == "2")
    //                    {
    //                        if (!dicfeecat.ContainsKey("2"))
    //                        {
    //                            dicfeecat.Add("2", feecat);
    //                        }
    //                        else
    //                        {
    //                            string setval = dicfeecat["2"] + ',' + feecat;
    //                            dicfeecat["2"] = setval;
    //                        }
    //                    }
    //                    else if (spt[0].ToString().Trim() == "3")
    //                    {
    //                        if (!dicfeecat.ContainsKey("3"))
    //                        {
    //                            dicfeecat.Add("3", feecat);
    //                        }
    //                        else
    //                        {
    //                            string setval = dicfeecat["3"] + ',' + feecat;
    //                            dicfeecat["3"] = setval;
    //                        }
    //                    }
    //                    else if (spt[0].ToString().Trim() == "4")
    //                    {
    //                        if (!dicfeecat.ContainsKey("4"))
    //                        {
    //                            dicfeecat.Add("4", feecat);
    //                        }
    //                        else
    //                        {
    //                            string setval = dicfeecat["4"] + ',' + feecat;
    //                            dicfeecat["4"] = setval;
    //                        }
    //                    }
    //                    else if (spt[0].ToString().Trim() == "5")
    //                    {
    //                        if (!dicfeecat.ContainsKey("5"))
    //                        {
    //                            dicfeecat.Add("5", feecat);
    //                        }
    //                        else
    //                        {
    //                            string setval = dicfeecat["5"] + ',' + feecat;
    //                            dicfeecat["5"] = setval;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    string finYearFk = string.Empty;
    //    string finYearFkal = string.Empty;
    //    for (int fk = 0; fk < chklsfyear.Items.Count; fk++)
    //    {
    //        if (chklsfyear.Items[fk].Selected)
    //        {
    //            if (string.IsNullOrEmpty(finYearFk))
    //            {
    //                finYearFk = chklsfyear.Items[fk].Value;
    //            }
    //            else
    //            {
    //                finYearFk += "','" + chklsfyear.Items[fk].Value;
    //            }
    //        }
    //    }
    //    finYearFkal = " and f.finyearfk in ('" + finYearFk + "') ";
    //    finYearFk = " and a.finyearfk in ('" + finYearFk + "') ";



    //    #endregion
    //    Hashtable htColCnt = new Hashtable();
    //    int pCnt = 0;
    //    //double StrDur = 0;
    //    string currSem = string.Empty;
    //    string oddOrEvenSem = string.Empty;
    //    string fnlYear = Convert.ToString(getCblSelectedValue(chklsfyear));

    //    //if (dsvalue.Tables.Count > 0 && dsvalue.Tables[0].Rows.Count > 0)
    //    //{
    //    //    for (int row = 0; row < dsvalue.Tables[0].Rows.Count; row++)
    //    //    {



    //    //        try
    //    //        {

    //    //            if (dsvalue.Tables[0].Rows.Count > 0)
    //    //                currSem = Convert.ToString(dsvalue.Tables[0].Rows[row]["current_semester"]);
    //    //            if (!string.IsNullOrEmpty(currSem))
    //    //                currSem = getCurSem(currSem, ref oddOrEvenSem);
    //    //            string StrYear = string.Empty;


    //    //            sno++;
    //    //            spreadDet.Sheets[0].RowCount++;
    //    //            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
    //    //            string degreename = Convert.ToString(dsvalue.Tables[0].Rows[row]["Course_Name"]);
    //    //            string Deptname = Convert.ToString(dsvalue.Tables[0].Rows[row]["Dept_Name"]);
    //    //            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(degreename) + " - " + Convert.ToString(currSem) + '(' + Convert.ToString(Deptname) + ')';

    //    //        }

    //    //        catch { currSem = "1 Year"; }
    //    //    }

    //    //}





    //    spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
    //    int coluval = 2;


    //    pCnt = spreadDet.Sheets[0].ColumnCount + 1;

    //    DataView dsheader = new DataView();
    //    dsheader = dsvalue.Tables[1].DefaultView;


    //    for (int s = 0; s < cbl_header1.Items.Count; s++)
    //    {
    //        int spancount = spreadDet.Sheets[0].ColumnCount;
    //        int tempcnt = 0;
    //        if (cbl_header1.Items[s].Selected == true)
    //        {

    //            checkva++;
    //            if (checkva > 1)
    //                tempcnt = spreadDet.Sheets[0].ColumnCount;
    //            //else
    //            //    spreadDet.Sheets[0].ColumnCount++;
    //            if (pCnt == 0)
    //                pCnt = tempcnt;

    //            htColCnt.Add(Convert.ToString(cbl_header1.Items[s].Value), spreadDet.Sheets[0].ColumnCount - 1);




    //            //spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Columns.Count - 3, 1, 3);

    //            for (int j = 0; j < chklsfyear.Items.Count; j++)
    //            {
    //                //int tempcnt = 0;
    //                if (chklsfyear.Items[j].Selected == true)
    //                {
    //                    spreadDet.Sheets[0].ColumnCount++;
    //                    checkva++;
    //                    if (checkva > 1)
    //                        tempcnt = spreadDet.Sheets[0].ColumnCount;
    //                    if (pCnt == 0)
    //                        pCnt = tempcnt;

    //                    spreadDet.Sheets[0].ColumnHeader.Cells[0, coluval + j].Text = Convert.ToString(cbl_header1.Items[s].Text);
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[0, coluval + j].Tag = Convert.ToString(cbl_header1.Items[s].Value);
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
    //                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

    //                    spreadDet.Sheets[0].ColumnHeader.Cells[1, coluval + j].Text = Convert.ToString(htActYr[Convert.ToString(chklsfyear.Items[j].Value).Trim()]);
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[1, coluval + j].Tag = Convert.ToString(chklsfyear.Items[j].Value);
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
    //                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
    //                    string finYearValue = Convert.ToString(chklsfyear.Items[j].Value);
    //                    string finYearText = Convert.ToString(chklsfyear.Items[j].Text);

    //                }
    //            }
    //            coluval = coluval + chklsfyear.Items.Count;
    //            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spancount, 1, chklsfyear.Items.Count);
    //        }
    //    }

    //    Hashtable hat = new Hashtable();
    //    int srno = 0;
    //    //for (int i = 0; i < cbl_seat.Items.Count; i++)
    //    //{
    //    for (int d = 0; d < dsvalue.Tables[0].Rows.Count; d++)
    //    {
    //        string degreecode = dsvalue.Tables[0].Rows[d]["degree_code"].ToString();
    //        string course = dsvalue.Tables[0].Rows[d]["Course_Name"].ToString();
    //        string department = dsvalue.Tables[0].Rows[d]["Dept_Name"].ToString();
    //        string courseid = dsvalue.Tables[0].Rows[d]["Course_id"].ToString();
    //        for (int i = 0; i < cbl_seat.Items.Count; i++)
    //        {
    //            if (cbl_seat.Items[i].Selected == true)
    //            {
    //                for (int y = 1; y <= 5; y++)
    //                {
    //                    if (dicfeecat.ContainsKey(y.ToString()))
    //                    {
    //                        string feecat = "and FeeCategory in(" + dicfeecat[y.ToString()] + ")";
    //                        dsvalue.Tables[1].DefaultView.RowFilter = "Degree_Code='" + degreecode + "' and seattype='" + Convert.ToString(cbl_seat.Items[i].Value) + "' " + feecat + "";
    //                        DataView dvdegree = dsvalue.Tables[1].DefaultView;
    //                        if (dvdegree.Count > 0)
    //                        {
    //                            if (!hat.Contains(courseid))
    //                            {
    //                                if (hat.Count > 0)
    //                                {

    //                                    spreadDet.Sheets[0].RowCount++;
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Total";
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
    //                                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 2);
    //                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightGray;

    //                                    for (int c = 2; c < spreadDet.Sheets[0].ColumnCount; c++)
    //                                    {
    //                                        Double tamou = 0;
    //                                        int endrow = 0;
    //                                        for (int r = spreadDet.Sheets[0].RowCount - 2; r >= endrow; r--)
    //                                        {
    //                                            if (spreadDet.Sheets[0].Cells[r, 0].Text.ToString() != "Total")
    //                                            {
    //                                                string text = spreadDet.Sheets[0].Cells[r, c].Text.ToString();
    //                                                if (text.Trim() != "")
    //                                                {
    //                                                    tamou = tamou + Convert.ToDouble(text);
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                endrow = spreadDet.Sheets[0].RowCount;
    //                                            }
    //                                        }
    //                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Text = tamou.ToString();
    //                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
    //                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Bold = true;
    //                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
    //                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
    //                                    }
    //                                }
    //                                hat.Add(courseid, spreadDet.Sheets[0].RowCount);
    //                            }
    //                            //row text
    //                            spreadDet.Sheets[0].RowCount++;
    //                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = course + " - " + department + " - " + y + " Year" + "-" + Convert.ToString(cbl_seat.Items[i].Text);
    //                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].HorizontalAlign = HorizontalAlign.Center;
    //                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightSkyBlue;
    //                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
    //                            srno++;
    //                            spreadDet.Sheets[0].RowCount++;
    //                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = course;
    //                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
    //                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

    //                            Double total = 0, htotla = 0;
    //                            for (int c = 2; c < spreadDet.Sheets[0].ColumnCount - 1; c++)
    //                            {
    //                                if (spreadDet.Sheets[0].ColumnHeader.Cells[1, c].Text != "Total")
    //                                {
    //                                    string headid = Convert.ToString(spreadDet.Sheets[0].ColumnHeader.Cells[1, c].Note);
    //                                    string accid = Convert.ToString(spreadDet.Sheets[0].ColumnHeader.Cells[1, c].Tag);
    //                                    Double amount = 0;

    //                                    dsvalue.Tables[1].DefaultView.RowFilter = "Degree_Code='" + degreecode + "' and HeaderfK='" + headid + "' and FinYearFK='" + accid + "' and seattype='" + Convert.ToString(cbl_seat.Items[i].Value) + "' " + feecat + "";
    //                                    DataView dvfeecode = dsvalue.Tables[1].DefaultView;
    //                                    for (int f = 0; f < dvfeecode.Count; f++)
    //                                    {
    //                                        //stuflag = true;
    //                                        amount = amount + Convert.ToDouble(dvfeecode[f]["feeamount"].ToString());
    //                                        total = total + amount;
    //                                        htotla = htotla + amount;
    //                                    }
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Text = amount.ToString();
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
    //                                }
    //                                else
    //                                {
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Text = htotla.ToString();
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].BackColor = Color.LightGray;
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
    //                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
    //                                    htotla = 0;
    //                                }
    //                            }
    //                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = total.ToString();
    //                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
    //                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].BackColor = Color.LightGray;
    //                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
    //                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }




    //    spreadDet.Visible = true;
    //    #endregion


    //}
    //protected void btnGo_Click(object sender, EventArgs e)
    //{
    //    DataSet dsvalue = new DataSet();
    //    dsvalue = loaddetails();
    //    if (dsvalue.Tables.Count > 0 && dsvalue.Tables[0].Rows.Count > 0)
    //    {
    //        loadspread(dsvalue);
    //    }
    //    else
    //    {

    //    }



    //}
    //protected DataSet loaddetails()
    //{
    //    DataSet dsval = new DataSet();
    //    try
    //    {
    //        string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
    //        string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
    //        string dept = Convert.ToString(getCblSelectedValue(cbl_degree));
    //        string fnlYear = Convert.ToString(getCblSelectedValue(chklsfyear));
    //        string sem = Convert.ToString(getCblSelectedValue(cbl_sem));
    //        string headerValue = string.Empty;
    //        string ledgerValue = string.Empty;
    //        headerValue = Convert.ToString(getCblSelectedValue(cbl_header1));
    //        ledgerValue = Convert.ToString(getCblSelectedValue(cbl_ledger1));
    //        string query = string.Empty;
    //        string feecatquery = "";
    //        for (int i = 0; i < cbl_sem.Items.Count; i++)
    //        {
    //            if (cbl_sem.Items[i].Selected == true)
    //            {
    //                string feecat = cbl_sem.Items[i].Value.ToString();
    //                if (feecatquery == "")
    //                {
    //                    feecatquery = feecat;
    //                }
    //                else
    //                {
    //                    feecatquery = feecatquery + "," + feecat;
    //                }
    //            }
    //        }
    //        string seat = "";
    //        string seatval = "";
    //        for (int i = 0; i < cbl_seat.Items.Count; i++)
    //        {
    //            if (cbl_seat.Items[i].Selected == true)
    //            {
    //                seat = cbl_seat.Items[i].Value.ToString();
    //                if (seatval == "")
    //                {
    //                    seatval = "'" + seat + "'";
    //                }
    //                else
    //                {
    //                    seatval = seatval + ",'" + seat + "'";
    //                }
    //            }
    //        }

    //        query += "select distinct c.type,c.Edu_Level,c.Course_Name,c.Course_Id,de.Dept_Name,d.Degree_Code from Degree d,Course c,Department de ,Registration r where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.Batch_Year in('" + batch + "') and r.degree_code in('" + degree + "') order by c.type,c.Edu_Level desc,c.Course_Id,d.Degree_Code";
    //        query += " select fi.priority,fi.ledgerName, f.BatchYear,d.Degree_Code,f.FinYearFK,f.FeeCategory,f.HeaderFK,sum(f.FeeAmount) as feeamount,seattype from FT_FeeAllotDegree  f,Degree d,FM_HeaderMaster a,FM_LedgerMaster fi where  f.DegreeCode=d.Degree_Code and a.HeaderPK=f.HeaderFK and fi.HeaderFK=a.HeaderPK and fi.HeaderFK=f.HeaderFK and fi.LedgerPK=f.LedgerFK  and isnull(f.FeeAmount,'0')>0 and f.BatchYear in('" + batch + "') and d.degree_code in('" + degree + "') and f.FeeCategory in(" + feecatquery + ") and f.HeaderFK in('" + headerValue + "') and f.LedgerFK in('" + ledgerValue + "') and FinYearFK in('" + fnlYear + "') and seattype in(" + seatval + ") group by f.BatchYear,d.Degree_Code,f.HeaderFK,f.FinYearFK,f.FeeCategory,seattype,fi.priority,fi.ledgerName order by d.Degree_Code,f.BatchYear desc,f.FinYearFK,f.FeeCategory,f.HeaderFK,isnull(fi.priority,1000),fi.ledgerName asc";//and seattype in('" + seatval + "') 


    //        dsval.Clear();
    //        dsval = d2.select_method_wo_parameter(query, "Text");
    //    }
    //    catch
    //    {
    //    }
    //    return dsval;
    //}
    protected string getCurSem(string curSem, ref string oddOrEvenSem)
    {
        string curSemVal = string.Empty;
        try
        {
            switch (curSem)
            {
                case "1":
                case "2":
                    curSemVal = "1 Year";
                    break;
                case "3":
                case "4":
                    curSemVal = "2 Year";
                    break;
                case "5":
                case "6":
                    curSemVal = "3 Year";
                    break;
                case "7":
                case "8":
                    curSemVal = "4 Year";
                    break;
                case "9":
                case "10":
                    curSemVal = "5 Year";
                    break;
                case "11":
                case "12":
                    curSemVal = "6 Year";
                    break;
                default:
                    curSemVal = "1";
                    break;

            }
            oddOrEvenSem = "Odd Semster";
            if (Convert.ToInt32(curSem) % 2 == 0)
                oddOrEvenSem = "Even Semster";
        }
        catch { }
        return curSemVal;
    }


    protected void chktype_batchchanged(object sender, EventArgs e)
    {
        try
        {
            // clear();
            if (chktype.Checked == true)
            {
                for (int i = 0; i < chklstype.Items.Count; i++)
                {
                    chklstype.Items[i].Selected = true;
                }
                txttype.Text = "Type (" + (chklstype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstype.Items.Count; i++)
                {
                    chklstype.Items[i].Selected = false;
                }
                txttype.Text = "--Select--";
            }
            ;
            //loadheader();
            //loadledger();
        }
        catch (Exception ex)
        {

        }
    }
    protected void chklstype_batchselected(object sender, EventArgs e)
    {
        try
        {

            int count = 0;
            chktype.Checked = false;
            txttype.Text = "---Select---";
            for (int i = 0; i < chklstype.Items.Count; i++)
            {
                if (chklstype.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                txttype.Text = "Type (" + count + ")";
                if (count == chklstype.Items.Count)
                {
                    chktype.Checked = true;
                }
            }

            //loadheader();
            //loadledger();
        }
        catch (Exception ex)
        {

        }
    }

    public void loadseat()
    {

        try
        {

            cbl_seat.Items.Clear();

            string seat = "";
            string deptquery = "select distinct TextCode,TextVal from TextValTable  where TextCriteria='seat' and college_code='" + ddl_collegename.SelectedItem.Value + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_seat.DataSource = ds;
                cbl_seat.DataTextField = "TextVal";
                cbl_seat.DataValueField = "TextCode";
                cbl_seat.DataBind();

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
            else
            {
                txt_seat.Text = "--Select--";

            }
        }
        catch
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
    public void loadtype()
    {
        try
        {
            int count = 0;
            chktype.Checked = false;
            txttype.Text = "---Select---";
            chklstype.Items.Clear();
            collegecode = ddl_collegename.SelectedValue.ToString();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txttype.Enabled = true;
                chklstype.DataSource = ds;
                chklstype.DataTextField = "type";
                chklstype.DataBind();
                txttype.Enabled = true;
                for (int i = 0; i < chklstype.Items.Count; i++)
                {
                    chklstype.Items[i].Selected = true;
                    count++;
                }
                if (count > 0)
                {
                    txttype.Text = "Type (" + count + ")";
                    if (count == chklstype.Items.Count)
                    {
                        chktype.Checked = true;
                    }
                }
            }
            else
            {
                txttype.Enabled = false;
            }
        }
        catch
        {
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            #region get value
            //clear();
            string batchquery = "";
            for (int b = 0; b < cbl_batch.Items.Count; b++)
            {
                if (cbl_batch.Items[b].Selected == true)
                {
                    if (batchquery == "")
                    {
                        batchquery = cbl_batch.Items[b].Text;
                    }
                    else
                    {
                        batchquery = batchquery + "," + cbl_batch.Items[b].Text;
                    }
                }
            }
            if (batchquery.Trim() == "")
            {
                //errmsg.Visible = true;
                //errmsg.Text = "Please Select The Batch Year And Then Proceed";
                //return;
            }

            string degreequery = "";
            for (int b = 0; b < cbl_dept.Items.Count; b++)
            {
                if (cbl_dept.Items[b].Selected == true)
                {
                    if (degreequery == "")
                    {
                        degreequery = cbl_dept.Items[b].Value.ToString();
                    }
                    else
                    {
                        degreequery = degreequery + "," + cbl_dept.Items[b].Value.ToString();
                    }
                }
            }
            if (degreequery.Trim() == "")
            {
                //errmsg.Visible = true;
                //errmsg.Text = "Please Select The Degree And Branch And Then Proceed";
                //return;
            }


            string headercode = "";
            for (int b = 0; b < cbl_header1.Items.Count; b++)
            {
                if (cbl_header1.Items[b].Selected == true)
                {
                    if (headercode == "")
                    {
                        headercode = cbl_header1.Items[b].Value.ToString();
                    }
                    else
                    {
                        headercode = headercode + "," + cbl_header1.Items[b].Value.ToString();
                    }
                }
            }
            if (headercode.Trim() == "")
            {
                //errmsg.Visible = true;
                //errmsg.Text = "Please Select The Header And Then Proceed";
                //return;
            }



            int hcount = 0;
            string actidquery = "";
            for (int i = 0; i < chklsfyear.Items.Count; i++)
            {
                if (chklsfyear.Items[i].Selected == true)
                {
                    hcount++;
                    string accid = chklsfyear.Items[i].Value.ToString();
                    if (actidquery == "")
                    {
                        actidquery = "'" + accid + "'";
                    }
                    else
                    {
                        actidquery = actidquery + ",'" + accid + "'";
                    }
                }
            }

            if (actidquery.Trim() == "")
            {
                //errmsg.Visible = true;
                //errmsg.Text = "Please Select The Finance Year And The Proceed";
                //return;
            }
            string ledgerValue = Convert.ToString(getCblSelectedValue(cbl_ledger1));
            //seat type
            string seat = "";
            string seatval = "";
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                if (cbl_seat.Items[i].Selected == true)
                {
                    seat = cbl_seat.Items[i].Value.ToString();
                    if (seatval == "")
                    {
                        seatval = "'" + seat + "'";
                    }
                    else
                    {
                        seatval = seatval + ",'" + seat + "'";
                    }
                }
            }

            if (seatval.Trim() == "")
            {
                //errmsg.Visible = true;
                //errmsg.Text = "Please Select The Finance Year And The Proceed";
                //return;
            }

            Dictionary<string, string> dicfeecat = new Dictionary<string, string>();

            string feecatquery = "";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    string feecat = cbl_sem.Items[i].Value.ToString();
                    if (feecatquery == "")
                    {
                        feecatquery = feecat;
                    }
                    else
                    {
                        feecatquery = feecatquery + "," + feecat;
                    }
                    string getval = cbl_sem.Items[i].Text.ToString();
                    string[] spt = getval.Split(' ');
                    if (spt.GetUpperBound(0) >= 0)
                    {
                        if (spt[1].ToString().ToLower().Trim().Contains("semester"))
                        {
                            if (spt[0].ToString().Trim() == "1" || spt[0].ToString().Trim() == "2")
                            {
                                if (!dicfeecat.ContainsKey("1"))
                                {
                                    dicfeecat.Add("1", feecat);
                                }
                                else
                                {
                                    string setval = dicfeecat["1"] + ',' + feecat;
                                    dicfeecat["1"] = setval;
                                }
                            }
                            else if (spt[0].ToString().Trim() == "3" || spt[0].ToString().Trim() == "4")
                            {
                                if (!dicfeecat.ContainsKey("2"))
                                {
                                    dicfeecat.Add("2", feecat);
                                }
                                else
                                {
                                    string setval = dicfeecat["2"] + ',' + feecat;
                                    dicfeecat["2"] = setval;
                                }
                            }
                            else if (spt[0].ToString().Trim() == "5" || spt[0].ToString().Trim() == "6")
                            {
                                if (!dicfeecat.ContainsKey("3"))
                                {
                                    dicfeecat.Add("3", feecat);
                                }
                                else
                                {
                                    string setval = dicfeecat["3"] + ',' + feecat;
                                    dicfeecat["3"] = setval;
                                }
                            }
                            else if (spt[0].ToString().Trim() == "7" || spt[0].ToString().Trim() == "8")
                            {
                                if (!dicfeecat.ContainsKey("4"))
                                {
                                    dicfeecat.Add("4", feecat);
                                }
                                else
                                {
                                    string setval = dicfeecat["4"] + ',' + feecat;
                                    dicfeecat["4"] = setval;
                                }
                            }
                            else if (spt[0].ToString().Trim() == "9" || spt[0].ToString().Trim() == "10")
                            {
                                if (!dicfeecat.ContainsKey("5"))
                                {
                                    dicfeecat.Add("5", feecat);
                                }
                                else
                                {
                                    string setval = dicfeecat["5"] + ',' + feecat;
                                    dicfeecat["5"] = feecat;
                                }
                            }
                        }
                        else
                        {
                            if (spt[0].ToString().Trim() == "1")
                            {
                                if (!dicfeecat.ContainsKey("1"))
                                {
                                    dicfeecat.Add("1", feecat);
                                }
                                else
                                {
                                    string setval = dicfeecat["1"] + ',' + feecat;
                                    dicfeecat["1"] = setval;
                                }
                            }
                            else if (spt[0].ToString().Trim() == "2")
                            {
                                if (!dicfeecat.ContainsKey("2"))
                                {
                                    dicfeecat.Add("2", feecat);
                                }
                                else
                                {
                                    string setval = dicfeecat["2"] + ',' + feecat;
                                    dicfeecat["2"] = setval;
                                }
                            }
                            else if (spt[0].ToString().Trim() == "3")
                            {
                                if (!dicfeecat.ContainsKey("3"))
                                {
                                    dicfeecat.Add("3", feecat);
                                }
                                else
                                {
                                    string setval = dicfeecat["3"] + ',' + feecat;
                                    dicfeecat["3"] = setval;
                                }
                            }
                            else if (spt[0].ToString().Trim() == "4")
                            {
                                if (!dicfeecat.ContainsKey("4"))
                                {
                                    dicfeecat.Add("4", feecat);
                                }
                                else
                                {
                                    string setval = dicfeecat["4"] + ',' + feecat;
                                    dicfeecat["4"] = setval;
                                }
                            }
                            else if (spt[0].ToString().Trim() == "5")
                            {
                                if (!dicfeecat.ContainsKey("5"))
                                {
                                    dicfeecat.Add("5", feecat);
                                }
                                else
                                {
                                    string setval = dicfeecat["5"] + ',' + feecat;
                                    dicfeecat["5"] = setval;
                                }
                            }
                        }
                    }
                }
            }
            if (feecatquery.Trim() == "")
            {
                //errmsg.Visible = true;
                //errmsg.Text = "Please Select The Category And The Proceed";
                //return;
            }

            #endregion
            Hashtable htActYr = getFinyear();
            spreadDet.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            rptprint2.Visible = true;
            spreadDet.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].DefaultStyle.Font.Bold = false;
            spreadDet.Sheets[0].SheetCorner.RowCount = 1;
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            spreadDet.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            spreadDet.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            spreadDet.Sheets[0].AllowTableCorner = true;
            spreadDet.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.Black;
            style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");

            spreadDet.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            spreadDet.Visible = true;
            spreadDet.Sheets[0].AutoPostBack = true;

            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 2;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
            spreadDet.Sheets[0].RowCount = 0;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Class";
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

            spreadDet.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            Boolean stuflag = false;

            #region query

            string strdegreequery = "select distinct c.type,c.Edu_Level,c.Course_Name,c.Course_Id,de.Dept_Name,d.Degree_Code from Degree d,Course c,Department de ,Registration r where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") order by c.type,c.Edu_Level desc,c.Course_Id,d.Degree_Code";
            DataSet dsdegree = da.select_method_wo_parameter(strdegreequery, "Text");

            // string strfeedeine="select f.Batch,d.Degree_Code,a.acct_id,f.FeeCat,f.Headid,sum(f.FeeAmount) as feeamount from feedefine f,Degree d,acctheader a,fee_info fi where f.DegreeCode=d.Course_Id and f.DepCode=d.Dept_Code and a.header_id=f.Headid and fi.header_id=a.header_id ";
            //  strfeedeine = strfeedeine + "and fi.header_id=f.Headid and fi.fee_code=f.FeeCode and f.FeeCat<>'0' and isnull(f.FeeAmount,'0')>0 and f.batch in(" + batchquery + ") and d.degree_code in(" + degreequery + ") and f.FeeCat in(" + feecatquery + ") and f.Headid in(" + headercode + ") and f.FeeCode in(" + feecodequery + ") group by f.Batch,d.Degree_Code,f.Headid,a.acct_id,f.FeeCat  order by d.Degree_Code,f.Batch desc,a.acct_id,f.FeeCat,f.Headid";

            //string strfeedeine = " select fi.priority,fi.ledgerName, f.BatchYear,d.Degree_Code,f.FinYearFK,f.FeeCategory,f.HeaderFK,sum(f.FeeAmount) as feeamount,seattype from FT_FeeAllotDegree  f,Degree d,FM_HeaderMaster a,FM_LedgerMaster fi where  f.DegreeCode=d.Degree_Code and a.HeaderPK=f.HeaderFK and fi.HeaderFK=a.HeaderPK and fi.HeaderFK=f.HeaderFK and fi.LedgerPK=f.LedgerFK  and isnull(f.FeeAmount,'0')>0 and f.BatchYear in(" + batchquery + ") and d.degree_code in(" + degreequery + ") and f.FeeCategory in(" + feecatquery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in('" + ledgerValue  + "') and FinYearFK in(" + actidquery + ") and seattype in(" + seatval + ") group by f.BatchYear,d.Degree_Code,f.HeaderFK,f.FinYearFK,f.FeeCategory,seattype,fi.priority,fi.ledgerName order by d.Degree_Code,f.BatchYear desc,f.FinYearFK,f.FeeCategory,f.HeaderFK,isnull(fi.priority,1000),fi.ledgerName asc";

            // select max(TotalAmount)TotalAmount,f.HeaderFK, f.LedgerFK,r.degree_code,r.Batch_Year,f.FeeCategory,r.college_code from Registration r,FT_FeeAllot f,applyn a where r.App_No=f.App_No and a.app_no=r.App_No   and r.Batch_Year in('2017','2016','2015') and r.degree_code in('79','138','139','140','141','142','143','144','145','146','147','148','149','80','81','82','83','84','85','86','94','95','87','88','89','90','91','92','93','65','66','67','68','69','70','71','122','78','72','73','74','75','76','77','101','103','107','100','102','106','105','104','113','110','109','108','112','111') and f.HeaderFK in('12')  and LedgerFK in('197','198','199','200','201','202','203','605','204','205','206','207','208','209','210','211','212')  and f.FinYearFK in ('2','3','4','5')  and a.seattype in('3553','3554') and f.feecategory in('3543','3544','3545','3546','3547','3548','32521') group by f.HeaderFK, f.LedgerFK,r.degree_code,r.Batch_Year,f.FeeCategory,r.college_code order by r.Batch_Year desc

            string strfeedeine = "select max(TotalAmount)TotalAmount,f.HeaderFK, f.LedgerFK,r.degree_code,r.Batch_Year,seattype,f.FeeCategory,f.FinYearFK,r.college_code from Registration r,FT_FeeAllot f,applyn a where r.App_No=f.App_No and a.app_no=r.App_No and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.FeeCategory in(" + feecatquery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in('" + ledgerValue + "') and FinYearFK in(" + actidquery + ") and seattype in(" + seatval + ") group by f.HeaderFK, f.LedgerFK,r.degree_code,r.Batch_Year,seattype,f.FeeCategory,f.FinYearFK,r.college_code order by  f.HeaderFK, f.LedgerFK,r.degree_code,r.Batch_Year desc";
            //order by isnull(l.priority,1000),l.ledgerName asc
            DataSet dsfeedeine = da.select_method_wo_parameter(strfeedeine, "Text");
            #endregion

            #region fspread columnheader bind
            hcount = hcount + 1;
            if (dsfeedeine.Tables[0].Rows.Count > 0)
            {
                for (int b = 0; b < cbl_header1.Items.Count; b++)
                {
                    if (cbl_header1.Items[b].Selected == true)
                    {
                        string headtest = cbl_header1.Items[b].Text.ToString();
                        string hid = cbl_header1.Items[b].Value.ToString();
                        dsfeedeine.Tables[0].DefaultView.RowFilter = "HeaderFK='" + hid + "'";
                        DataView dvhead = dsfeedeine.Tables[0].DefaultView;
                        if (dvhead.Count > 0)
                        {
                            for (int i = 0; i < chklsfyear.Items.Count; i++)
                            {
                                if (chklsfyear.Items[i].Selected == true)
                                {
                                    string accid = chklsfyear.Items[i].Value.ToString();
                                    spreadDet.Sheets[0].ColumnCount++;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(htActYr[Convert.ToString(chklsfyear.Items[i].Value).Trim()]); //chklsfyear.Items[i].Text.ToString();
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Tag = accid;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Note = hid;
                                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                }
                            }
                            spreadDet.Sheets[0].ColumnCount++;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total";
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Visible = false;
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - hcount].Text = headtest;
                            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - hcount, 1, hcount);
                        }
                    }
                }
            }
            #endregion

            //spreadDet.Sheets[0].ColumnCount++;
            //spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total";
            //spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
            //spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

            Hashtable hat = new Hashtable();
            int srno = 0;
            //for (int i = 0; i < cbl_seat.Items.Count; i++)
            //{
            for (int d = 0; d < dsdegree.Tables[0].Rows.Count; d++)
            {
                string degreecode = dsdegree.Tables[0].Rows[d]["degree_code"].ToString();
                string course = dsdegree.Tables[0].Rows[d]["Course_Name"].ToString();
                string department = dsdegree.Tables[0].Rows[d]["Dept_Name"].ToString();
                string courseid = dsdegree.Tables[0].Rows[d]["Course_id"].ToString();
                for (int i = 0; i < cbl_seat.Items.Count; i++)
                {
                    if (cbl_seat.Items[i].Selected == true)
                    {
                        for (int y = 1; y <= 5; y++)
                        {
                            if (dicfeecat.ContainsKey(y.ToString()))
                            {
                                string feecat = "and FeeCategory in(" + dicfeecat[y.ToString()] + ")";
                                dsfeedeine.Tables[0].DefaultView.RowFilter = "Degree_Code='" + degreecode + "' and seattype='" + Convert.ToString(cbl_seat.Items[i].Value) + "' " + feecat + "";
                                DataView dvdegree = dsfeedeine.Tables[0].DefaultView;
                                if (dvdegree.Count > 0)
                                {
                                    if (!hat.Contains(courseid))
                                    {
                                        if (hat.Count > 0)
                                        {

                                            spreadDet.Sheets[0].RowCount++;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Total";
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 2);
                                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightGray;

                                            for (int c = 2; c < spreadDet.Sheets[0].ColumnCount; c++)
                                            {
                                                Double tamou = 0;
                                                int endrow = 0;
                                                for (int r = spreadDet.Sheets[0].RowCount - 2; r >= endrow; r--)
                                                {
                                                    if (spreadDet.Sheets[0].Cells[r, 0].Text.ToString() != "Total")
                                                    {
                                                        string text = spreadDet.Sheets[0].Cells[r, c].Text.ToString();
                                                        if (text.Trim() != "")
                                                        {
                                                            tamou = tamou + Convert.ToDouble(text);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        endrow = spreadDet.Sheets[0].RowCount;
                                                    }
                                                }
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Text = tamou.ToString();
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Bold = true;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                                            }
                                        }
                                        hat.Add(courseid, spreadDet.Sheets[0].RowCount);
                                    }
                                    //row text
                                    //spreadDet.Sheets[0].RowCount++;
                                    //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = course + " - " + department + " - " + y + " Year" + "-" + Convert.ToString(cbl_seat.Items[i].Text);
                                    //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightSkyBlue;
                                    //spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                    srno++;
                                    spreadDet.Sheets[0].RowCount++;
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = course + " - " + department + " - " + y + " Year";
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                    Double total = 0, htotla = 0;
                                    for (int c = 2; c < spreadDet.Sheets[0].ColumnCount - 1; c++)
                                    {
                                        if (spreadDet.Sheets[0].ColumnHeader.Cells[1, c].Text != "Total")
                                        {
                                            string headid = Convert.ToString(spreadDet.Sheets[0].ColumnHeader.Cells[1, c].Note);
                                            string accid = Convert.ToString(spreadDet.Sheets[0].ColumnHeader.Cells[1, c].Tag);
                                            Double amount = 0;

                                            dsfeedeine.Tables[0].DefaultView.RowFilter = "Degree_Code='" + degreecode + "' and HeaderfK='" + headid + "' and FinYearFK='" + accid + "' and seattype='" + Convert.ToString(cbl_seat.Items[i].Value) + "' " + feecat + "";
                                            DataView dvfeecode = dsfeedeine.Tables[0].DefaultView;
                                            for (int f = 0; f < dvfeecode.Count; f++)
                                            {
                                                stuflag = true;
                                                amount = amount + Convert.ToDouble(dvfeecode[f]["TotalAmount"].ToString());
                                                total = total + amount;
                                                htotla = htotla + amount;
                                            }
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Text = amount.ToString();
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                        else
                                        {
                                            //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Text = htotla.ToString();
                                            //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                                            //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].BackColor = Color.LightGray;
                                            //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                            //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                            //htotla = 0;
                                        }
                                    }
                                    //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = total.ToString();
                                    //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                    //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].BackColor = Color.LightGray;
                                    //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                }
                            }
                        }
                    }
                }
            }
            if (stuflag == true)
            {
                spreadDet.Sheets[0].RowCount++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Total";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightCoral;
                for (int c = 2; c < spreadDet.Sheets[0].ColumnCount; c++)
                {
                    Double tamou = 0;
                    int endrow = 0;
                    for (int r = spreadDet.Sheets[0].RowCount - 2; r > endrow; r--)
                    {
                        if (spreadDet.Sheets[0].Cells[r, 0].Text.ToString() != "Total")
                        {
                            string text = spreadDet.Sheets[0].Cells[r, c].Text.ToString();
                            if (text.Trim() != "")
                            {
                                tamou = tamou + Convert.ToDouble(text);
                            }
                        }
                        else
                        {
                            endrow = spreadDet.Sheets[0].RowCount;
                        }
                    }
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Text = tamou.ToString();
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                }

                spreadDet.Sheets[0].RowCount++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;

                for (int c = 2; c < spreadDet.Sheets[0].ColumnCount; c++)
                {
                    Double tamou = 0;
                    for (int r = 0; r < spreadDet.Sheets[0].RowCount - 1; r++)
                    {
                        if (spreadDet.Sheets[0].Cells[r, 0].Text.ToString() != "Total")
                        {
                            string text = spreadDet.Sheets[0].Cells[r, c].Text.ToString();
                            if (text.Trim() != "")
                            {
                                tamou = tamou + Convert.ToDouble(text);
                            }
                        }
                    }
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Text = tamou.ToString();
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                }


                spreadDet.Visible = true;
                //lblrptname.Visible = true;
                //txtexcelname.Visible = true;
                //btnxl.Visible = true;
                //btnmasterprint.Visible = true;
            }
            else
            {
                //clear();
                //errmsg.Visible = true;
                //errmsg.Text = "No Records Found";
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            //errmsg.Visible = true;
            //errmsg.Text = ex.ToString();
        }
    }
    protected void btnprintmaster2_Click(object sender, EventArgs e)
    {
        try
        {
            string dptname = "financeyearwisecollectionreport";
            string pagename = "financeyearwisecollectionreport.aspx";

            Printcontrol2.loadspreaddetails(spreadDet, pagename, dptname);
            Printcontrol2.Visible = true;
            lbl_norec2.Visible = false;
        }
        catch
        {
        }
    }
    protected void btnExcel2_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {

                d2.printexcelreport(spreadDet, reportname);


                lbl_norec2.Visible = false;
            }
            else
            {
                lbl_norec2.Text = "Please Enter Your Report Name";
                lbl_norec2.Visible = true;
                txtexcelname2.Focus();
            }
        }
        catch
        {

        }
    }
}